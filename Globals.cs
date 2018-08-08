using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClaymoreLogChart.DataModel;

namespace ClaymoreLogChart
{
    public static class Globals
    {
        public static List<GpuData> GPUs;

        public static string CursorsFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\Graphics\\Cursors");
        public static string GpuNamesFileName = "gpus.txt";
        //public static string ProcessedLogFileExtension = "pcf";
        //public static string LogChartProjectFileExtension = "lcprj";


        #region -- Extenders --
        //https://stackoverflow.com/questions/3141692/standard-deviation-of-generic-list/10147392
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            double avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }
        #endregion
    }
}
