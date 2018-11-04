using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using ClaymoreLogChart.DataModel;

namespace ClaymoreLogChart.Parsers
{
    public class BackgroundLogParser : System.ComponentModel.BackgroundWorker
    {
        public List<LogEntry> logs;
        public Dictionary<int, GpuData> gpus;
        public int TotalFileEntries;
        public int ProcessedEntries;

        private bool isSetRigStartTime = false;
        private DateTime rigStartTime;
        private string filePath = "";

        public BackgroundLogParser(string filename)
        {
            filePath = filename;

            ResetCounters();
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;

            DoWork += LogParser_DoWork;
        }

        public void Start()
        {
            RunWorkerAsync();
        }

        private void LogParser_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            if (!File.Exists(filePath))
            {
                ReportProgress(0, "File corrupt or does not exist: " + filePath);
                return;
            }

            ReportProgress(0, "Creating Log Entries List.\n");
            CreateLogEntriesList();
            ReportProgress(50, "Extracting GPU Data.\n");
            ExtractGpuCountAndInfo();
            ReportProgress(70, "Extracting GPU Hashrate and Temps.\n");
            ExtractHashrateAndTemps();
            ReportProgress(80, "Extracting GPU Shares Found Data\n");
            ExtractFoundSharesData();
            ReportProgress(87, "Extracting GPU Incorrect Shares Data\n");
            ExtractIncorrectSharesData();
            ReportProgress(94, "Extracting Miner GPU Settings\n");
            ExtractMinerAndGpuSettings();

            foreach (GpuData gpu in gpus.Values)
                gpu.RigStartTime = rigStartTime;
        }

        private void ResetCounters()
        {
            TotalFileEntries = 1;
            ProcessedEntries = 0;
        }

        private void CreateLogEntriesList()
        {
            if (!File.Exists(filePath))
            {
                //TODO: Give an error message here
                return;
            }

            ResetCounters();

            bool beginDefined = false;
            TimeSpan beginTime = new TimeSpan(0, 0, 0, 0);
            TimeSpan logTime, prevTime = beginTime;
            int days = 0;
            string[] lines = File.ReadAllLines(filePath);
            string[] lineParts;
            LogEntry entry;
            int shouldReport = 0;

            logs = new List<LogEntry>();
            TotalFileEntries = lines.Length;

            ReportProgress(1, string.Format("{0} Log Entries Found.\n", TotalFileEntries));

            foreach (string line in lines)
            {
                if (CancellationPending)
                    break;

                lineParts = line.Split('\t');
                if (lineParts.Length != 3)
                    continue;
                lineParts[0] = FixTimeSpanFormat(lineParts[0]);
                logTime = TimeSpan.Parse(lineParts[0]);

                if (!beginDefined)
                {
                    prevTime = beginTime = logTime;
                    beginDefined = true;
                }

                if (logTime < prevTime && (prevTime - logTime).TotalMinutes > 5)
                {
                    days++;
                    //logTime = logTime.Add(new TimeSpan(24, 0, 0));
                }

                prevTime = logTime;
                logTime = logTime.Add(new TimeSpan(days * 24, 0, 0)) - beginTime;

                ProcessedEntries++;

                if (ShouldKeepLog(lineParts[2]))
                {
                    entry = new LogEntry(logTime, lineParts[2]);
                    logs.Add(entry);
                    //Console.WriteLine(lineParts[2]);

                }

                if (++shouldReport == 5000)
                {
                    shouldReport = 0;
                    ReportProgress(ProcessedEntries * 80 / TotalFileEntries);
                    System.Threading.Thread.Sleep(1);
                }

            }
        }

        private void ExtractGpuCountAndInfo()
        {
            string gpuPattern = @"GPU #\d*: ";
            string nvidiaPattern = @"NVIDIA";
            string totPattern = @"Total cards: ";

            Regex gpuRgx = new Regex(gpuPattern, RegexOptions.Compiled);
            Regex totRgx = new Regex(totPattern, RegexOptions.Compiled);

            int charIndex, totalGpus = 0;
            int gpuIndex = 0;
            string temp;

            GpuData data;
            gpus = new Dictionary<int, GpuData>();

            GpuManufacturer gpuType = GpuManufacturer.AMD;
            foreach (LogEntry entry in logs)
            {
                if (gpuRgx.IsMatch(entry.LogText))
                {
                    temp = entry.LogText.Substring(5);
                    gpuIndex = int.Parse(temp.Substring(0, temp.IndexOf(":")));

                    if (!gpus.ContainsKey(gpuIndex))
                        gpus.Add(gpuIndex, new GpuData());

                    data = gpus[gpuIndex];
                    data.Index = gpuIndex;

                    if (temp.Contains("algorithm"))
                        data.Algorithm = temp.Substring(temp.IndexOf("algorithm") + 10).Trim();
                    else
                    {
                        charIndex = temp.IndexOf(':') + 1;
                        data.Name = temp.Substring(charIndex, temp.IndexOf(',') - charIndex).Trim();
                        charIndex = temp.IndexOf("pci bus");
                        data.PciePort = temp.Substring(charIndex+7, temp.LastIndexOf(')') - charIndex - 7).Trim();
                    }

                    data.Manufacturer = gpuType;

                    gpus[gpuIndex] = data;
                }
                else if (totRgx.IsMatch(entry.LogText))
                {
                    totalGpus = int.Parse(entry.LogText.Substring(13));
                    Console.WriteLine(totalGpus);
                    break;
                }
                else if (entry.LogText.StartsWith(nvidiaPattern))
                {
                    //In claymore logs, all the AMD cards are listed first, thereofre after this line all the reported gpus will be nvidia
                    gpuType = GpuManufacturer.NVidia;
                }
            }

            if (totalGpus != gpus.Count)
            {
                //throw new Exception("Total Number of GPUS does not match GPU data");
                ReportProgress(100, "ERROR: Total Number of GPUS does not match GPU data");
                CancelAsync();
            }
        }

        private void ExtractHashrateAndTemps()
        {
            string ethHashLinePattern = @"ETH: GPU\d*";
            string gpuTempFanLineClue = "fan=";
            string gpuSplitPattern = "GPU";

            Regex ethHashLine = new Regex(ethHashLinePattern, RegexOptions.Compiled);
            Regex gpuSplitter = new Regex(gpuSplitPattern, RegexOptions.Compiled);

            string[] spliRes;
            int i, entIndex;
            LogEntry entry;
            for (entIndex = 0; entIndex < logs.Count; entIndex++)
            {
                entry = logs[entIndex];
                if (ethHashLine.IsMatch(entry.LogText))
                {
                    spliRes = gpuSplitter.Split(entry.LogText);
                    for (i = 1; i < spliRes.Length; i++)
                        ParseHashrateData(entry.Time, spliRes[i]);
                }
                else if (entry.LogText.Contains(gpuTempFanLineClue))
                {
                    spliRes = gpuSplitter.Split(entry.LogText);
                    for (i = 1; i < spliRes.Length; i++)
                        ParseTempFanData(entry.Time, spliRes[i]);
                }
            }
        }

        private void ExtractFoundSharesData()
        {
            string shareFoundLineClue = "SHARE FOUND";
            string gpuIndexData;
            LogEntry entry;
            int gpuIndexStart, gpuIndexEnd, gpuIndex;

            for(int entIndex=0; entIndex<logs.Count; entIndex++)
            {
                entry = logs[entIndex];
                if(entry.LogText.Contains(shareFoundLineClue))
                {
                    if(!isSetRigStartTime)
                    {
                        try
                        {
                            string dateTimeText = entry.LogText.Substring(4, entry.LogText.IndexOf("SHARE") - 3 - 4).Trim().Replace('-', ' ');
                            Regex format = new Regex(@"(?<month>\d+)/(?<day>\d+)/(?<year>\d+) (?<hour>\d+):(?<min>\d+):(?<sec>\d+)");
                            Match matches = format.Match(dateTimeText);
                            int y = int.Parse(matches.Groups["year"].Value) + 2000;
                            int m = int.Parse(matches.Groups["month"].Value);
                            int d = int.Parse(matches.Groups["day"].Value);
                            int h = int.Parse(matches.Groups["hour"].Value);
                            int n = int.Parse(matches.Groups["min"].Value);
                            int s = int.Parse(matches.Groups["sec"].Value);
                            rigStartTime = new DateTime(y, m, d, h, n, s) - entry.Time;
                        }
                        catch(Exception x)
                        {
                            rigStartTime = new DateTime();
                        }
                        isSetRigStartTime = true;
                    }
                    gpuIndexStart = entry.LogText.IndexOf("(");
                    gpuIndexEnd = entry.LogText.IndexOf(")");
                    gpuIndexData = entry.LogText.Substring(gpuIndexStart, gpuIndexEnd - gpuIndexStart);
                    gpuIndexData = gpuIndexData.Replace("GPU ", "").TrimStart('(').TrimEnd(')');
                    gpuIndex = int.Parse(gpuIndexData);

                    gpus[gpuIndex].SharesFound++;
                }
            }
        }

        private void ExtractIncorrectSharesData()
        {
            string incorrectShareLineClue = "got incorrect";
            string gpuIndexData;
            LogEntry entry;
            int gpuIndexEnd, gpuIndex;

            for (int entIndex = 0; entIndex < logs.Count; entIndex++)
            {
                entry = logs[entIndex];
                if (entry.LogText.Contains(incorrectShareLineClue))
                {
                    gpuIndexEnd = entry.LogText.IndexOf("got");
                    gpuIndexData = entry.LogText.Substring(0, gpuIndexEnd);
                    gpuIndexData = gpuIndexData.Replace("GPU ", "").Trim(' ', 'g', '#');
                    gpuIndex = int.Parse(gpuIndexData);

                    gpus[gpuIndex].IncorrectShares++;
                }
            }
        }

        private void ExtractMinerAndGpuSettings()
        {
            int argsLineIndex = 0;
            string argsLinePattern = "args:";

            while (argsLineIndex < logs.Count && !logs[argsLineIndex].LogText.StartsWith(argsLinePattern))
                argsLineIndex++;

            if (argsLineIndex == logs.Count)
                return;

            int?[] argValues;
            //Read Core Clock Values
            argValues = ReadArgValues(logs[argsLineIndex].LogText, "cclock", true, false);
            for (int i = 0; i < gpus.Count; i++) gpus[i].CoreClock = argValues[i];

            //Read Memory Clock Values
            argValues = ReadArgValues(logs[argsLineIndex].LogText, "mclock", true, false);
            for (int i = 0; i < gpus.Count; i++) gpus[i].MemoryClock = argValues[i];

            //Read Power Limit Values
            argValues = ReadArgValues(logs[argsLineIndex].LogText, "powlim", true, true);
            for (int i = 0; i < gpus.Count; i++) gpus[i].PowerLimit = argValues[i];

            //Read Core Voltage Values
            argValues = ReadArgValues(logs[argsLineIndex].LogText, "cvddc", true, false);
            for (int i = 0; i < gpus.Count; i++) gpus[i].CoreVoltage = argValues[i];

            //Read Memory Voltage Values
            argValues = ReadArgValues(logs[argsLineIndex].LogText, "mvddc", true, false);
            for (int i = 0; i < gpus.Count; i++) gpus[i].MemoryVoltage = argValues[i];

            //Read Goal Temperatures Values
            argValues = ReadArgValues(logs[argsLineIndex].LogText, "tt", true, true);
            for (int i = 0; i < gpus.Count; i++) gpus[i].GoalTemperature = argValues[i];

        }
        //Reads a given arg values and assigns them to the gpus in the same order. The last value will be repeated for all remaining GPUs excpet NVidia ones.
        //NOTE: The copy for NVidia should only be set to true if the arg can be copied from AMD to NVIDIA, for example power limit (powlim) or temp (tt)
        private int?[] ReadArgValues(string logLine, string arg, bool copyLast, bool copyForNvidia)
        {
            int?[] argValues = new int?[gpus.Count];

            logLine = logLine.ToUpper();
            arg = arg.ToUpper();

            int argStart = logLine.IndexOf(arg);
            if (argStart < 0)
                return argValues;

            string cutTheBS = logLine.Substring(argStart+arg.Length);
            Regex nextArg = new Regex(@"-[A-Z]", RegexOptions.Compiled);

            cutTheBS = nextArg.Replace(cutTheBS, ">");
            int argEnd = cutTheBS.IndexOf(">");
            if(argEnd!=-1)
                cutTheBS = cutTheBS.Substring(0, argEnd).Trim();

            string[] values = cutTheBS.Split(',');
            int numToCopy = Math.Min(gpus.Count, values.Length);

            int argValue;
            
            for(int i=0;i<numToCopy;i++)
            {
                if (int.TryParse(values[i], out argValue))
                    argValues[i] = argValue;
            }

            if(copyLast && gpus.Count > values.Length)
            {
                int lastIndex = values.Length - 1;

                for (int i = values.Length - 1; i < gpus.Count; i++)
                    if(gpus[i].Manufacturer!= GpuManufacturer.NVidia || copyForNvidia || gpus[lastIndex].Manufacturer==GpuManufacturer.NVidia)
                        argValues[i] = argValues[values.Length - 1];
            }

            return argValues;
        }

        private void ParseTempFanData(TimeSpan time, string gpuTempFan)
        {
            gpuTempFan = gpuTempFan.Replace("t=", "").Replace("fan=", "").Replace("%", "").Replace("; ", "").Replace("C", "").Replace(",", "");

            string[] parts = gpuTempFan.Split(' ');
            try
            {
                int gpuIndex = int.Parse(parts[0]);
                int temp = int.Parse(parts[1]);
                int fan = int.Parse(parts[2]);

                gpus[gpuIndex].Temps.Add(new TempratureData(time, temp, fan));

            }
            catch (Exception x)
            {
                Console.Write("ParseHashrateData Failed. Input was: " + gpuTempFan);
            }
        }

        private void ParseHashrateData(TimeSpan time, string gpuHashStr)
        {
            string[] parts = gpuHashStr.Split(' ');
            try
            {
                int gpuIndex = int.Parse(parts[0]);
                //float hashRate = float.Parse(parts[1]);
                float hashRate;
                if (!float.TryParse(parts[1], out hashRate))
                    hashRate = 0;

                gpus[gpuIndex].HashRate.Add(new HashRateData(time, hashRate));

            }
            catch (Exception x)
            {
                Console.Write("ParseHashrateData Failed. Input was: " + gpuHashStr);
            }

        }

        private string FixTimeSpanFormat(string time)
        {
            int lastColonLocation = time.LastIndexOf(':');
            if (lastColonLocation == -1) return time;
            return time.Substring(0, lastColonLocation) + "." + time.Substring(lastColonLocation + 1, time.Length - lastColonLocation - 1);
        }

        private bool ShouldKeepLog(string log)
        {
            bool shouldKeep = false;
            string[] importantLogTexts = { "args:", "GPU", "FOUND", "incorrect", "Total", "NVIDIA" };

            foreach (string str in importantLogTexts)
                if (log.Contains(str))
                {
                    shouldKeep = true;
                    break;
                }

            return shouldKeep;
        }
    }
}
