using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaymoreLogChart.DataModel
{
    public struct LogEntry
    {
        public TimeSpan Time;
        public string LogText;

        public LogEntry(TimeSpan time, string text)
        {
            Time = time;
            LogText = text;
        }
    }
}
