using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaymoreLogChart.DataModel
{
    public class GpuData
    {
        public int Index;
        public string Name;
        public string NickName;
        public string PciePort;
        public string Algorithm;
        public GpuManufacturer Manufacturer;

        public int SharesFound = 0;
        public int IncorrectShares = 0;

        public int? CoreClock = null;
        public int? MemoryClock = null;
        public int? PowerLimit = null;
        public int? CoreVoltage = null;
        public int? MemoryVoltage = null;
        public int? GoalTemperature = null;

        private float avrHash = float.NaN;
        private float avrTemp = float.NaN;
        private float avrFan = float.NaN;
        private float minHash = float.NaN;
        private float minTemp = float.NaN;
        private float minFan = float.NaN;
        private float maxHash = float.NaN;
        private float maxTemp = float.NaN;
        private float maxFan = float.NaN;
        private float sdvHash = float.NaN;
        private float sdvTemp = float.NaN;
        private float sdvFan = float.NaN;
        
        public float AvergaeHashRate { get { if (float.IsNaN(avrHash)) CalculateAveragesAndMinMax(); return avrHash; } }
        public float AvergaeTemperature { get { if (float.IsNaN(avrTemp)) CalculateAveragesAndMinMax(); return avrTemp; } }
        public float AvergaeFanSpeed { get { if (float.IsNaN(avrFan)) CalculateAveragesAndMinMax(); return avrFan; } }
        public float MinimumHashRate { get { if (float.IsNaN(minHash)) CalculateAveragesAndMinMax(); return minHash; } }
        public float MinimumTemperature { get { if (float.IsNaN(minTemp)) CalculateAveragesAndMinMax(); return minTemp; } }
        public float MinimumFanSpeed { get { if (float.IsNaN(minFan)) CalculateAveragesAndMinMax(); return minFan; } }
        public float MaximumHashRate { get { if (float.IsNaN(maxHash)) CalculateAveragesAndMinMax(); return maxHash; } }
        public float MaximumTemperature { get { if (float.IsNaN(maxTemp)) CalculateAveragesAndMinMax(); return maxTemp; } }
        public float MaximumFanSpeed { get { if (float.IsNaN(maxFan)) CalculateAveragesAndMinMax(); return maxFan; } }
        public float StdDevHashRate { get { if (float.IsNaN(sdvHash)) CalculateAveragesAndMinMax(); return sdvHash; } }
        public float StdDevTemperature { get { if (float.IsNaN(sdvTemp)) CalculateAveragesAndMinMax(); return sdvTemp; } }
        public float StdDevFanSpeed { get { if (float.IsNaN(sdvFan)) CalculateAveragesAndMinMax(); return sdvFan; } }

        public string Text
        {
            get
            {
                if (NickName == null || NickName == "" || NickName == string.Empty)
                    return Name;
                return NickName;
            }
        }
        public float SharesPerHour
        {
            get
            {
                if (HashRate.Count > 0)
                {
                    return (float)(SharesFound * 3600 / HashRate[HashRate.Count - 1].Time.TotalSeconds);
                }
                return float.NaN;
            }
        }
        public string TimeForOneShare
        {
            get
            {
                if (HashRate.Count > 0)
                {
                    TimeSpan ts = new TimeSpan( (HashRate[HashRate.Count - 1].Time.Ticks / SharesFound));
                    return ts.ToString("%m' min '%s' sec'");
                }
                return "N/A";
            }
        }

        public List<HashRateData> HashRate = new List<HashRateData>();
        public List<TempratureData> Temps = new List<TempratureData>();

        private void CalculateAveragesAndMinMax()
        {
            avrHash = (float)HashRate.Skip(HashRate.Count > 20 ? 20 : 0).Average(x => x.HashRate);
            minHash = HashRate.SkipWhile(x => x.HashRate == 0).Min(x => x.HashRate);
            maxHash = HashRate.Max(x => x.HashRate);

            avrTemp = (float)Temps.Skip(Temps.Count > 20 ? 20 : 0).Average(x => x.Temprature);
            minTemp = Temps.Skip(Temps.Count > 20 ? 20 : 0).Min(x => x.Temprature);
            maxTemp = Temps.Skip(Temps.Count > 20 ? 20 : 0).Max(x => x.Temprature);

            avrFan = (float)Temps.Skip(Temps.Count > 20 ? 20 : 0).Average(x => x.FanSpeed);
            minFan = Temps.Skip(Temps.Count > 20 ? 20 : 0).Min(x => x.FanSpeed);
            maxFan = Temps.Skip(Temps.Count > 20 ? 20 : 0).Max(x => x.FanSpeed);

            sdvHash = (float)HashRate.Skip(HashRate.Count > 20 ? 20 : 0).Select(x => (double)x.HashRate).StandardDeviation();
            sdvTemp = (float)Temps.Skip(Temps.Count > 20 ? 20 : 0).Select(x => (double)x.Temprature).StandardDeviation();
            sdvFan = (float)Temps.Skip(Temps.Count > 20 ? 20 : 0).Select(x => (double)x.FanSpeed).StandardDeviation();
        }


        public double[] GetHashRateTimes()
        {
            return HashRate.Select(hash => new DateTime(1900,1,1).Add(hash.Time).ToOADate()).ToArray();
        }
        public float[] GetHashRateValues()
        {
            return HashRate.Select(hash => hash.HashRate).ToArray();
        }

        public double[] GetTempTimes()
        {
            return Temps.Select(temp => new DateTime(1900, 1, 1).Add(temp.Time).ToOADate()).ToArray();
        }
        public int[] GetTempValues()
        {
            return Temps.Select(temp => temp.Temprature).ToArray();
        }
        public int[] GetFanValues()
        {
            return Temps.Select(temp => temp.FanSpeed).ToArray();
        }
    }

    public enum GpuManufacturer
    {
        AMD,
        NVidia,
        Other
    }
}
