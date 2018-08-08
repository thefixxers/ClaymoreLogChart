using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

using WeifenLuo.WinFormsUI.Docking;

using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Appccelerate.Events;

using ClaymoreLogChart.DataModel;
using ClaymoreLogChart.VisualStylers;

namespace ClaymoreLogChart.DockPanels
{
    public partial class ChartPanel : DockContent
    {
        private const int AREA_HASHRATE = 0;
        private const int AREA_FANTEMP = 1;
        private const string AREANAME_HASHRATE = "Hashrate";
        private const string AREANAME_FANTEMP = "Fan Speed & Temperature";

        private List<Series> hashes = new List<Series>();
        private List<Series> temps = new List<Series>();
        private List<Series> fans = new List<Series>();
        private List<GpuData> gpus = new List<GpuData>();
        private List<int> myGpuIndices = new List<int>();

        public ChartPanel()
        {
            InitializeComponent();

            Charter.Series.Clear();
            Charter.ChartAreas.Clear();
            //while (Charter.ChartAreas.Count > 1)
            //    Charter.ChartAreas.RemoveAt(1);
            Charter.ChartAreas.Add(AREANAME_HASHRATE);
            Charter.ChartAreas.Add(AREANAME_FANTEMP);

            SetCharterVisualStyles();

            //Wiring Events
            this.DragEnter += ChartPanel_DragEnter;
            this.DragOver += ChartPanel_DragOver;
            this.DragDrop += ChartPanel_DragDrop;

            CheckBoxHashrate.CheckedChanged += CheckBoxCheckedChanged;
            CheckBoxTemp.CheckedChanged += CheckBoxCheckedChanged;
            CheckBoxFan.CheckedChanged += CheckBoxCheckedChanged;

            Charter.MouseEnter += Charter_MouseEnter;
            Charter.MouseLeave += Charter_MouseLeave;
            Charter.MouseWheel += ChartPanel_MouseWheel;
        }

        #region -- Mouse Events --
        private void Charter_MouseLeave(object sender, EventArgs e)
        {
            if (Charter.Focused)
                Charter.Parent.Focus();
        }

        private void Charter_MouseEnter(object sender, EventArgs e)
        {
            if (!Charter.Focused)
                Charter.Focus();
        }

        private void ChartPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            //
            // https://stackoverflow.com/questions/13584061/how-to-enable-zooming-in-microsoft-chart-control-by-using-mouse-wheel
            //
            var chart = (Chart)sender;
            var xAxis = chart.ChartAreas[0].AxisX;
            var yAxis = chart.ChartAreas[0].AxisY;
            
            try
            {
                if (e.Delta < 0) // Scrolled down.
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) * 4;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) * 4;

                    if (posXStart > 0)
                    {
                        //xAxis.ScaleView.Zoom(posXStart, posXFinish);
                        chart.ChartAreas[AREA_HASHRATE].AxisX.ScaleView.Zoom(posXStart, posXFinish);
                        chart.ChartAreas[AREA_FANTEMP].AxisX.ScaleView.Zoom(posXStart, posXFinish);
                    }
                    else
                    {
                        //xAxis.ScaleView.ZoomReset();
                        chart.ChartAreas[AREA_HASHRATE].AxisX.ScaleView.ZoomReset();
                        chart.ChartAreas[AREA_FANTEMP].AxisX.ScaleView.ZoomReset();
                    }
                }
                else if (e.Delta > 0) // Scrolled up.
                {
                    var xMin = xAxis.ScaleView.ViewMinimum;
                    var xMax = xAxis.ScaleView.ViewMaximum;

                    var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 4;
                    var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 4;

                    //xAxis.ScaleView.Zoom(posXStart, posXFinish);
                    chart.ChartAreas[AREA_HASHRATE].AxisX.ScaleView.Zoom(posXStart, posXFinish);
                    chart.ChartAreas[AREA_FANTEMP].AxisX.ScaleView.Zoom(posXStart, posXFinish);
                }

                this.Text = Charter.ChartAreas[AREANAME_HASHRATE].AxisX.ScaleView.SmallScrollSize.ToString();
            }
            catch { }
        }
        #endregion

        #region -- Drag & Drop Events --
        private void ChartPanel_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
            CursorManager.Instance.EffectMode = CursorManager.Effect.Copy;
        }

        private void ChartPanel_DragDrop(object sender, DragEventArgs e)
        {

            List<GpuData> gpus = (List<GpuData>)e.Data.GetData(typeof(List<GpuData>));
            AddGpuDataToChart(gpus);
            
            Charter.Invalidate();
        }

        private void ChartPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect =  e.AllowedEffect;
            this.Text += ".";
        }
        #endregion

        private void CheckBoxCheckedChanged(object sender, EventArgs e)
        {
            //Hashrate
            if (sender.Equals(CheckBoxHashrate))
            {
                if (CheckBoxHashrate.Checked)
                {
                    foreach (Series serie in hashes)
                        Charter.Series.Add(serie);
                    Charter.ChartAreas[AREANAME_HASHRATE].Visible = true;
                }
                else
                {
                    foreach (Series serie in hashes)
                        Charter.Series.Remove(serie);
                    Charter.ChartAreas[AREANAME_HASHRATE].Visible = false;
                }
            }

            //Temps
            if (sender.Equals(CheckBoxTemp))
            {
                if (CheckBoxTemp.Checked)
                {
                    foreach (Series serie in temps)
                        Charter.Series.Add(serie);
                    Charter.ChartAreas[AREANAME_FANTEMP].Visible = true;
                }
                else
                {
                    foreach (Series serie in temps)
                        Charter.Series.Remove(serie);
                }
            }

            //Fans
            if (sender.Equals(CheckBoxFan))
            {
                if (CheckBoxFan.Checked)
                {
                    foreach (Series serie in fans)
                        Charter.Series.Add(serie);
                    Charter.ChartAreas[AREANAME_FANTEMP].Visible = true;
                }
                else
                {
                    foreach (Series serie in fans)
                        Charter.Series.Remove(serie);
                }
            }

            if (!CheckBoxFan.Checked && !CheckBoxTemp.Checked)
                Charter.ChartAreas[AREANAME_FANTEMP].Visible = false;
        }

        private void AddGpuDataToChart(List<GpuData> draggedGpus)
        {
            LegendItem litem;
            Series hashSeries, fanSeries, tempSeries;
            foreach (GpuData gpu in draggedGpus)
            {
                if (gpus.Contains(gpu))
                    continue;

                gpus.Add(gpu);
                hashSeries = new Series();
                
                hashSeries.Points.DataBindXY(gpu.GetHashRateTimes(), gpu.GetHashRateValues());
                //series.Points.DataBindXY(times, hashValues);
                hashSeries.Color = ThemeColors.PresetColors[gpu.Index];
                hashSeries.BorderWidth = 1;
                hashSeries.ChartType = SeriesChartType.FastLine;
                hashSeries.XValueType = ChartValueType.DateTime;
                hashSeries.IsVisibleInLegend = false;
                //hashSeries.LegendText = string.Format("GPU {0} - {1}", gpu.Index, gpu.Text);

                tempSeries = new Series();
                tempSeries.Points.DataBindXY(gpu.GetTempTimes(), gpu.GetTempValues());
                tempSeries.Color = ThemeColors.PresetColors[gpu.Index];
                tempSeries.BorderWidth = 3;
                tempSeries.ChartType = SeriesChartType.FastLine;
                tempSeries.XValueType = ChartValueType.DateTime;
                tempSeries.ChartArea = AREANAME_FANTEMP;
                tempSeries.IsVisibleInLegend = false;

                fanSeries = new Series();
                fanSeries.Points.DataBindXY(gpu.GetTempTimes(), gpu.GetFanValues());
                fanSeries.Color = Color.FromArgb(160, ThemeColors.PresetColors[gpu.Index]);
                fanSeries.BorderWidth = 1;
                fanSeries.ChartType = SeriesChartType.FastLine;
                fanSeries.XValueType = ChartValueType.DateTime;
                fanSeries.ChartArea = AREANAME_FANTEMP;
                fanSeries.IsVisibleInLegend = false;
                fanSeries["PointWidth"] = "0.4";
                fanSeries["DrawingStyle"] = "Default";

                /*Charter.ChartAreas[AREA_HASHRATE].CursorX.AutoScroll = true;
                Charter.ChartAreas[AREA_HASHRATE].AxisX.ScaleView.Zoomable = true;
                Charter.ChartAreas[AREA_HASHRATE].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
                Charter.ChartAreas[AREA_HASHRATE].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;*/

                Charter.Series.Add(hashSeries);
                Charter.Series.Add(tempSeries);
                Charter.Series.Add(fanSeries);

                hashes.Add(hashSeries);
                temps.Add(tempSeries);
                fans.Add(fanSeries);

                litem = new LegendItem();
                litem.Name = string.Format("GPU {0} - {1}", gpu.Index, gpu.Text);
                litem.Color = ThemeColors.PresetColors[gpu.Index];
                litem.ImageStyle = LegendImageStyle.Rectangle;
                Charter.Legends[0].CustomItems.Add(litem);
            }
        }

        private void SetCharterVisualStyles()
        {
            Charter.BackColor = BackColor = ThemeColors.WindowContentBackgroundDarkTheme;
            Charter.ForeColor = ForeColor = ThemeColors.WindowContentForegroundDarkTheme;

            CheckBoxHashrate.BackColor = Charter.BackColor;
            CheckBoxHashrate.ForeColor = Charter.ForeColor;
            CheckBoxHashrate.FlatAppearance.BorderColor = ThemeColors.ToolbarStripBorderColor;
            CheckBoxHashrate.FlatAppearance.CheckedBackColor = ThemeColors.UnfocusedSelectedBackgroundColor;
            CheckBoxHashrate.FlatAppearance.MouseOverBackColor = ThemeColors.UnfocusedSelectedBackgroundColor;

            CheckBoxTemp.BackColor = Charter.BackColor;
            CheckBoxTemp.ForeColor = Charter.ForeColor;
            CheckBoxTemp.FlatAppearance.BorderColor = ThemeColors.ToolbarStripBorderColor;
            CheckBoxTemp.FlatAppearance.CheckedBackColor = ThemeColors.UnfocusedSelectedBackgroundColor;
            CheckBoxTemp.FlatAppearance.MouseOverBackColor = ThemeColors.UnfocusedSelectedBackgroundColor;

            CheckBoxFan.BackColor = Charter.BackColor;
            CheckBoxFan.ForeColor = Charter.ForeColor;
            CheckBoxFan.FlatAppearance.BorderColor = ThemeColors.ToolbarStripBorderColor;
            CheckBoxFan.FlatAppearance.CheckedBackColor = ThemeColors.UnfocusedSelectedBackgroundColor;
            CheckBoxFan.FlatAppearance.MouseOverBackColor = ThemeColors.UnfocusedSelectedBackgroundColor;

            for (int i = 0; i < Charter.ChartAreas.Count; i++)
            {
                Charter.ChartAreas[i].BackColor = BackColor;

                //Axis Colors and Fonts
                Charter.ChartAreas[i].AxisX.LineColor = ThemeColors.ChartAxisColor;
                Charter.ChartAreas[i].AxisY.LineColor = ThemeColors.ChartAxisColor;
                Charter.ChartAreas[i].AxisX.MajorGrid.LineColor = ThemeColors.ChartMajorLineColor;
                Charter.ChartAreas[i].AxisY.MajorGrid.LineColor = ThemeColors.ChartMajorLineColor;
                Charter.ChartAreas[i].AxisX.MajorTickMark.LineColor = ThemeColors.ChartAxisColor;
                Charter.ChartAreas[i].AxisY.MajorTickMark.LineColor = ThemeColors.ChartAxisColor;
                Charter.ChartAreas[i].AxisX.LabelStyle.ForeColor = ThemeColors.ChartAxisColor;
                Charter.ChartAreas[i].AxisY.LabelStyle.ForeColor = ThemeColors.ChartAxisColor;
                Charter.ChartAreas[i].AxisX.TitleForeColor = ThemeColors.ChartAxisColor;
                Charter.ChartAreas[i].AxisY.TitleForeColor = ThemeColors.ChartAxisColor;
                Charter.ChartAreas[i].AxisX.TitleFont = new Font(this.Font.FontFamily, 14);
                Charter.ChartAreas[i].AxisY.TitleFont = new Font(this.Font.FontFamily, 14);
                
                //Axis X Scroll Bar
                Charter.ChartAreas[i].AxisX.ScrollBar.BackColor = ThemeColors.ChartScrollBarBackColor;
                Charter.ChartAreas[i].AxisX.ScrollBar.ButtonColor = ThemeColors.ChartScrollBarButtonColor;
                Charter.ChartAreas[i].AxisX.ScrollBar.LineColor = ThemeColors.ChartScrollBarBorderColor;


                Charter.ChartAreas[i].AxisX.LabelStyle.Format = "Dd-HH:mm";
                
                //Setting Selection, Scrolling and Zooming
                Charter.ChartAreas[i].CursorX.IsUserEnabled = true;
                Charter.ChartAreas[i].CursorX.IsUserSelectionEnabled = true;
                Charter.ChartAreas[i].CursorX.Interval = 0.002;
                Charter.ChartAreas[i].CursorX.AutoScroll = true;
                Charter.ChartAreas[i].AxisX.ScaleView.Zoomable = true;
                Charter.ChartAreas[i].AxisX.ScaleView.SmallScrollSize = 10;
                Charter.ChartAreas[i].AxisX.ScaleView.SmallScrollSizeType = DateTimeIntervalType.Minutes;
                Charter.ChartAreas[i].AxisX.ScaleView.SmallScrollMinSize = 1;
                Charter.ChartAreas[i].AxisX.ScaleView.SmallScrollMinSizeType = DateTimeIntervalType.Minutes;
                Charter.ChartAreas[i].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;

                Charter.ChartAreas[i].InnerPlotPosition.Auto = true;
            }

            Charter.Legends[0].BackColor = BackColor;
            Charter.Legends[0].ForeColor = ForeColor;

            //Hashrate Area Properties
            Charter.ChartAreas[AREA_HASHRATE].AxisX.Title = "Time";
            Charter.ChartAreas[AREA_HASHRATE].AxisY.Title = "Hashrate";
            Charter.ChartAreas[AREA_HASHRATE].AxisY.Minimum = 20;

            Charter.ChartAreas[AREA_FANTEMP].AxisX.Title = "Time";
            Charter.ChartAreas[AREA_FANTEMP].AxisY.Title = "Fan Speed";
            Charter.ChartAreas[AREA_FANTEMP].AxisY.Minimum = 40;
            //Charter.ChartAreas[AREA_FANTEMP].
            //Charter.ChartAreas[AREA_FANSPEED].AxisY2.Title = "Temperature";

            Charter.ChartAreas[AREANAME_HASHRATE].AlignmentOrientation = AreaAlignmentOrientations.Vertical ;
            Charter.ChartAreas[AREANAME_HASHRATE].AlignmentStyle = AreaAlignmentStyles.Cursor  | AreaAlignmentStyles.PlotPosition | AreaAlignmentStyles.AxesView;
            Charter.ChartAreas[AREANAME_HASHRATE].AlignWithChartArea = AREANAME_FANTEMP;

            Charter.Legends[0].Docking = Docking.Bottom;
        }

        private void CreateLegendsPanel()
        {
            Charter.Legends[0].CustomItems.Clear();

            LegendItem litem;
            foreach (GpuData gpu in gpus)
            {
                litem = new LegendItem();
                litem.Name = string.Format("GPU {0} - {1}", gpu.Index, gpu.Text);
                litem.Color = ThemeColors.PresetColors[gpu.Index];
                litem.ImageStyle = LegendImageStyle.Rectangle;
                Charter.Legends[0].CustomItems.Add(litem);
            }
            //Charter.Legends[0].DockedToChartArea = false;
            //Charter.Legends[0].Docking = Docking.Bottom;
        }

        [EventSubscription("topic://Gpu/Renamed", typeof(OnUserInterface))]
        public void GpuSelectedHandler(object sender, EventArgs<int> ea)
        {
            CreateLegendsPanel();
        }
    }
}
