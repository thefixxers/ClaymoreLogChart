using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using BrightIdeasSoftware;
using WeifenLuo.WinFormsUI.Docking;
using Appccelerate.EventBroker;
using Appccelerate.Events;
using Appccelerate.EventBroker.Extensions;
using Appccelerate.EventBroker.Handlers;


using ClaymoreLogChart.DataModel;
using ClaymoreLogChart.VisualStylers;

namespace ClaymoreLogChart.DockPanels
{
    public partial class GpuDetailPanel : DockContent
    {
        public GpuDetailPanel()
        {
            InitializeComponent();

            BackColor = ThemeColors.WindowContentBackgroundDarkTheme;
            ForeColor = ThemeColors.WindowContentForegroundDarkTheme;

            //Jus Some Color Games
            ObjectListGpus.BackColor = BackColor;
            ObjectListGpus.ForeColor = ForeColor;
            ObjectListGpus.SelectedBackColor = ThemeColors.UnfocusedSelectedBackgroundColor;
            ObjectListGpus.UnfocusedSelectedBackColor = ThemeColors.UnfocusedSelectedBackgroundColor;
            ObjectListGpus.UnfocusedSelectedForeColor = ObjectListGpus.ForeColor;
            

            //Setting ObjectListView Aspect Handlers
            ColumnColor.AspectToStringConverter = (obj) => { return " "; };
            ColumnColor.RendererDelegate = ColorRender;
            ColumnBrand.ImageGetter = (obj) => { return (obj as GpuData).Manufacturer.ToString().ToLower() + "_full.png"; };

            //AspectGetter for Nullable Fields. It shows "N/A" instead of just being empty
            ColumnCClock.AspectToStringConverter = NullFieldAspectGetter;
            ColumnMClock.AspectToStringConverter = NullFieldAspectGetter;
            ColumnMvddc.AspectToStringConverter = NullFieldAspectGetter;
            ColumnCvddc.AspectToStringConverter = NullFieldAspectGetter;
            ColumnPowLim.AspectToStringConverter = NullFieldAspectGetter;
            ColumnTT.AspectToStringConverter = NullFieldAspectGetter;

            //AspectGetter for fields which are calculated from the data
            ColumnAvgTemp.AspectGetter = (obj) => { return string.Format("{0:0.00}", (obj as GpuData).AvergaeTemperature); };
            ColumnAvgHash.AspectGetter = (obj) => { return string.Format("{0:0.00}",(obj as GpuData).AvergaeHashRate); };
            ColumnSdHash.AspectGetter = (obj) => { return string.Format("{0:0.00}", (obj as GpuData).StdDevHashRate); };
            ColumnAvgFan.AspectGetter = (obj) => { return string.Format("{0:0.00}", (obj as GpuData).AvergaeFanSpeed); };

            SetColumnHeaderStyles();

        }

        public string NullFieldAspectGetter(object obj)
        {
            if (obj == null) return "---";
            return obj.ToString();
        }
        public void SetObjects(List<GpuData> gpuObjects)
        {
            ObjectListGpus.ClearObjects();
            ObjectListGpus.AddObjects(gpuObjects);
            ObjectListGpus.Refresh();
            ObjectListGpus.Update();
        }
        private bool ColorRender(EventArgs e, Graphics g, Rectangle r, Object rowObject)
        {
            GpuData gpu = rowObject as GpuData;
            Color color = ThemeColors.PresetColors[gpu.Index];

            if (ObjectListGpus.SelectedObject == rowObject)
                g.Clear(ObjectListGpus.Focused ? ObjectListGpus.SelectedBackColor : ObjectListGpus.UnfocusedSelectedBackColor);
            else
                g.Clear(ObjectListGpus.BackColor);

            RectangleF cellRect = new Rectangle(r.X + 2, r.Y + 2, r.Width - 5, r.Height - 5);
            g.FillRectangle(new SolidBrush(color), cellRect);

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            using (SolidBrush textBrush = new SolidBrush(ObjectListGpus.ForeColor))
                g.DrawString(gpu.Index.ToString(), ObjectListGpus.Font, textBrush, cellRect, format);

            return true;
        }

        private void SetColumnHeaderStyles()
        {
            ObjectListGpus.HeaderUsesThemes = false; 

            HeaderFormatStyle headerStyle = new HeaderFormatStyle();
            
            headerStyle.SetBackColor(ThemeColors.ListHeaderNormalBackground);
            headerStyle.SetForeColor(ThemeColors.ListHeaderNormalForeground);
            headerStyle.SetFont(this.Font);
            ObjectListGpus.HeaderWordWrap = true;
            ObjectListGpus.HeaderFormatStyle = headerStyle;
        }
    }
}
