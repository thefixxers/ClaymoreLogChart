using System;
using System.IO;
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
            ReportProgress(90, "Extracting GPU Incorrect Shares Data\n");
            ExtractIncorrectSharesData();
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
            string totPattern = @"Total cards: ";

            Regex gpuRgx = new Regex(gpuPattern, RegexOptions.Compiled);
            Regex totRgx = new Regex(totPattern, RegexOptions.Compiled);

            int charIndex, totalGpus = 0;
            int gpuIndex = 0;
            string temp;

            GpuData data;
            gpus = new Dictionary<int, GpuData>();

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


                    gpus[gpuIndex] = data;
                }
                else if (totRgx.IsMatch(entry.LogText))
                {
                    totalGpus = int.Parse(entry.LogText.Substring(13));
                    Console.WriteLine(totalGpus);
                    break;
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
            string[] importantLogTexts = { "args:", "GPU", "FOUND", "incorrect", "Total" };

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
