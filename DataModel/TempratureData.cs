using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaymoreLogChart.DataModel
{
    public class TempratureData
    {
        public TimeSpan Time;
        public int Temprature;
        public int FanSpeed;

        public TempratureData(TimeSpan time, int t, int fan)
        {
            Time = time;
            Temprature = t;
            FanSpeed = fan;
        }
    }
}
