using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaymoreLogChart.DataModel
{
    public class HashRateData: IComparable<HashRateData>
    {
        public TimeSpan Time;
        public float HashRate;

        public HashRateData(TimeSpan time, float hash)
        {
            Time = time;
            HashRate = hash;
        }

        public int CompareTo(HashRateData other)
        {
            return Math.Sign(HashRate - other.HashRate);
        }
    }
}
