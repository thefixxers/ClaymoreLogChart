using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ClaymoreLogChart.VisualStylers
{
    public static class ThemeColors
    {
        public static Color WindowContentBackgroundDarkTheme = Color.FromArgb(255, 37, 37, 38);
        public static Color WindowContentForegroundDarkTheme = Color.FromArgb(255, 221, 221, 221);
        public static Color WindowSubTitleBackgroundDarkTheme = Color.FromArgb(255, 51, 51, 52);
        //public static Color ScrollBarThumbColor = Color.FromArgb(255, 104, 104, 104);
        //public static Color ScrollBarThumbMouseOverColor = Color.FromArgb(255, 153, 153, 153);
        //public static Color ScrollBarThumbMouseDownColor = Color.FromArgb(255, 202, 202, 202);
        public static Color ErrorMessageForegroundColor = WindowContentForegroundDarkTheme;
        public static Color UnfocusedSelectedBackgroundColor = Color.FromArgb(255, 85, 85, 85);
        public static Color FocusedSelectedBackgroundColor = Color.FromArgb(255, 14, 97, 152);

        public static Color ToolbarStripBackgroundColor = Color.FromArgb(255, 45, 45, 48);
        public static Color ToolbarStripForegroundColor = WindowContentForegroundDarkTheme;
        public static Color ToolbarStripWindowColor = Color.FromArgb(255, 27, 27, 27);
        public static Color ToolbarStripBorderColor = Color.FromArgb(255, 63, 63, 70);
        public static Color ToolbarStripDisabledColor = ToolbarStripBorderColor;

        public static Color ListHeaderNormalBackground = Color.FromArgb(255, 51, 51, 51);
        public static Color ListHeaderNormalForeground = Color.FromArgb(255, 255, 255, 255);

        public static Color ChartAxisColor = Color.FromArgb(255, 241, 241, 241);
        public static Color ChartMajorLineColor = Color.FromArgb(255, 85, 85, 85);
        public static Color ChartScrollBarBorderColor = Color.FromArgb(255, 37, 37, 38);
        public static Color ChartScrollBarBackColor = Color.FromArgb(255, 37, 37, 38);
        public static Color ChartScrollBarButtonColor = Color.FromArgb(255, 85, 85, 85);

        public static Color[] BotntonColors = {
                                                Color.FromArgb(0, 0, 160),      // Blue
                                                Color.FromArgb(160, 0, 0),      // Red
                                                Color.FromArgb(0, 160, 0),      // Green
                                                Color.FromArgb(160, 160, 0),    // Yellow
                                                Color.FromArgb(160, 0, 160),    // Magenta
                                                Color.FromArgb(160, 80, 80),  // Pink
                                                Color.FromArgb(80, 80, 80),  // Gray
                                                Color.FromArgb(80, 80, 0),      // Brown
                                                Color.FromArgb(160, 80, 0),    // Orange};
                                              };

        public static Color[] PresetColors = {
                                                Color.FromArgb(150,50,50),
                                                Color.FromArgb(50,120,50),
                                                Color.FromArgb(50,50,150),
                                                Color.FromArgb(160,160,70),
                                                Color.FromArgb(60,160,160),
                                                Color.FromArgb(160,50,160),
                                                Color.FromArgb(170,100,50),
                                                Color.FromArgb(100,50,170),

                                                Color.Red,
                                                Color.Green,
                                                Color.Blue,
                                                Color.Yellow,
                                                Color.Cyan,
                                                Color.Magenta,

                                                Color.Maroon,
                                                Color.Olive,
                                                Color.Navy,
                                                Color.Brown,
                                                Color.Teal,
                                                Color.Purple,

                                                Color.Gray,

                                                Color.Pink,
                                                Color.Orange,
                                                Color.MintCream,
                                                Color.Coral,
                                                Color.Lavender,

                                                Color.Beige,
                                                Color.Lime,
                                                Color.White
        };
    }
}
