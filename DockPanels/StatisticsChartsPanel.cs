using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WeifenLuo.WinFormsUI.Docking;
using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;
using Appccelerate.Events;

using ClaymoreLogChart.DataModel;
using ClaymoreLogChart.VisualStylers;
using System.Windows.Forms.DataVisualization.Charting;

namespace ClaymoreLogChart.DockPanels
{
    public partial class StatisticsChartsPanel : DockContent
    {
        private delegate void ChartPopulatorFunc(string indexId);
        private const int maxCharts = 9;

        private struct MenuItemFuncRelation { public string text; public ChartPopulatorFunc func; public MenuItemFuncRelation(string t, ChartPopulatorFunc f) { text = t; func = f; } };
        private MenuItemFuncRelation[] menuItems;
        private Dictionary<string, Label> topLabels = new Dictionary<string, Label>();

        private ChartArea selectedChartArea = null;
        private bool[] chartAreaVisibilities = new bool[maxCharts];
        private bool isInZoomMode = false;


        public StatisticsChartsPanel()
        {
            InitializeComponent();

            menuItems = new MenuItemFuncRelation[] {
                new MenuItemFuncRelation("None",                               RemoveSeriesIfExists),
                new MenuItemFuncRelation("-",                                  null),
                new MenuItemFuncRelation("Average Hashrate",                   ShowAverageHashrates),
                new MenuItemFuncRelation("Average Temperature",                ShowAverageTemperatures),
                new MenuItemFuncRelation("Average Fan Speed",                  ShowAverageFanSpeed),
                new MenuItemFuncRelation("-",                                  null),
                new MenuItemFuncRelation("Min-Max Hashrate",                   ShowMinMaxHashrates),
                new MenuItemFuncRelation("Min-Max Temperature",                ShowMinMaxTemperatures),
                new MenuItemFuncRelation("Min-Max Fan Speed",                  ShowMinMaxFanSpeed),
                new MenuItemFuncRelation("-",                                  null),
                new MenuItemFuncRelation("Shares Found",                       ShowSharesFound),
                new MenuItemFuncRelation("Incorrect Shares",                   ShowIncorrectShares),
                new MenuItemFuncRelation("Hashrate Standard Deviation",        ShowHashrateStandardDeviation),
                new MenuItemFuncRelation("Shares Found / Average Hashrate",    ShowFoundOverHashrate)
            };

            ResetAllAreas();

            Charter.MouseMove += Charter_MouseMove; ;
            Charter.MouseClick += Charter_MouseClick;
            Charter.MouseDoubleClick += Charter_MouseDoubleClick;
        }

        private void Charter_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ChartArea chart = GetChartAreaUnderMouse(e.Location);
            SelectChartArea(chart);

            if (chart == null) return;

            if (!isInZoomMode)
            {
                for (int i = 0; i < maxCharts; i++)
                {
                    chartAreaVisibilities[i] = Charter.ChartAreas[i].Visible;
                    if (Charter.ChartAreas[i] != chart)
                        Charter.ChartAreas[i].Visible = false;
                    else
                    {
                        //The idea was to write the labels on the columns, but it is abandoned for now
                        //MessageBox.Show(chart.Name.ToString());
                        Charter.PostPaint += Charter_PostPaint;
                    }
                }
            }
            else
            {
                for (int i = 0; i < maxCharts; i++)
                {
                    Charter.ChartAreas[i].Visible = chartAreaVisibilities[i];
                }
                Charter.PostPaint -= Charter_PostPaint;
            }
            isInZoomMode = !isInZoomMode;
        }

        private void Charter_PostPaint(object sender, ChartPaintEventArgs e)
        {
            ChartArea area = e.ChartElement as ChartArea;
            if (area != selectedChartArea)
                return;

            int cx, cy, ey;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Far;
            format.LineAlignment = StringAlignment.Center;

            Graphics grfx = e.ChartGraphics.Graphics;
            Bitmap bmp;
            using (Font font = new Font(FontFamily.GenericSansSerif, 12))
            {
                for (int index = 0; index < Globals.GPUs.Count; index++)
                {
                    cx = (int)area.AxisX.ValueToPixelPosition(index + 1);
                    cy = (int)area.AxisY.ValueToPixelPosition(0);
                    ey = (int)area.AxisY.ValueToPixelPosition(e.Chart.Series[area.Name].Points[index].YValues[0]);

                    SizeF textSize = grfx.MeasureString(Globals.GPUs[index].Text, font, cy - ey, format);
                    //RectangleF textRect = new RectangleF(cx - textSize.Width, ey, textSize.Width, cy - ey);

                    bmp = new Bitmap((int)textSize.Width, (int)textSize.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                    using (Graphics bgrfx = Graphics.FromImage(bmp))
                    {
                        //bgrfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
                        //using (SolidBrush backBr = new SolidBrush(ThemeColors.PresetColors[index]))
                        //    bgrfx.FillRectangle(backBr, 0, 0, bmp.Width, bmp.Height);
                        bgrfx.DrawString(Globals.GPUs[index].Text, font, Brushes.White, 0, 0);
                        //TextRenderer.DrawText(bgrfx, Globals.GPUs[index].Text, font, new Point(0,0), Color.White);
                    }
                    bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    //grfx.FillRectangle(Brushes.Lime, cx - textSize.Width / 2, cy - textSize.Height, textSize.Width, textSize.Height);
                    grfx.DrawImage(bmp, cx - bmp.Width / 2, cy - bmp.Height);
                    //grfx.DrawImage(bmp, 10, 10);
                }
            }
        }

        private void AddDetailsToChart(ChartArea chart)
        {
            
        }

        private void Charter_MouseMove(object sender, MouseEventArgs e)
        {
            ChartArea chart = GetChartAreaUnderMouse(Charter.PointToClient(System.Windows.Forms.Cursor.Position));
            SelectChartArea(chart);
        }

        private void Charter_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            ChartArea chart = GetChartAreaUnderMouse(e.Location);
            SelectChartArea(chart);
            
            if (chart == null) return;
            
            Label relatedLabel = GetRelatedLabel(chart.Name);
            ContextMenuStrip menu = GetContextMenu(relatedLabel);
            menu.Items[0].Text = "Close";
            menu.Items.Insert(1, new ToolStripMenuItem("Close All But This", null, (s, ea) => { RemoveAllButThis(chart.Name); }, "NoName"));
            Point cursorPos = System.Windows.Forms.Cursor.Position;
            Point menuPos = new Point(cursorPos.X - menu.Width / 2, cursorPos.Y);
            menu.Show(menuPos);
        }
        private void SelectChartArea(ChartArea chart)
        {
            if (chart == selectedChartArea) return;
            if (selectedChartArea != null) selectedChartArea.BackColor = Color.Transparent;
            if (chart != null)
            {
                chart.BackColor = Color.FromArgb(80, ThemeColors.UnfocusedSelectedBackgroundColor);
                selectedChartArea = chart;
            }
        }

        private Label GetRelatedLabel(string id)
        {
            try
            {
                Label relatedLabel = (Label)this.Controls.Find("Label_" + id, true)[0];
                return relatedLabel;
            }
            catch(Exception x)
            {
                return null;
            }
        }

        #region -- Chart Functions --
        private void RemoveAllButThis(string chartAreaId)
        {
            string idStr = "";
            for(int i=0;i<Charter.ChartAreas.Count;i++)
            {
                idStr = i.ToString();
                if (idStr == chartAreaId) continue;
                RemoveSeriesIfExists(idStr);

                Label label = GetRelatedLabel(idStr);
                if (label != null) label.Text = string.Format("Chart {0}: None", chartAreaId);
            }
        }
        private void RemoveSeriesIfExists(string chartAreaId)
        {
            Series current;
            if ((current = Charter.Series.FindByName(chartAreaId)) != null)
                Charter.Series.Remove(current);
            if ((current = Charter.Series.FindByName(chartAreaId+"_extra")) != null)
                Charter.Series.Remove(current);

            if(Charter.ChartAreas.FindByName(chartAreaId)!=null)
                Charter.ChartAreas[chartAreaId].Visible = false;

            
        }
        private void ShowAverageHashrates(string chartAreaId)
        {
            
            float[] values = Globals.GPUs.Select(x => x.AvergaeHashRate).ToArray();
            CreateAndBindSeries(chartAreaId, chartAreaId, SeriesChartType.Column, 50, true, values);
            
            FixBarColors();
            
            Charter.ChartAreas[chartAreaId].AxisX.Title = "Average Hasrate";
        }
        private void ShowAverageTemperatures(string chartAreaId)
        {

            float[] values = Globals.GPUs.Select(x => x.AvergaeTemperature).ToArray();
            CreateAndBindSeries(chartAreaId, chartAreaId, SeriesChartType.Column, 50, true, values);
            FixBarColors();
            Charter.ChartAreas[chartAreaId].AxisX.Title = "Average Temperatures";
        }
        private void ShowAverageFanSpeed(string chartAreaId)
        {

            float[] values = Globals.GPUs.Select(x => x.AvergaeFanSpeed).ToArray();
            CreateAndBindSeries(chartAreaId, chartAreaId, SeriesChartType.Column, 50, true, values);
            FixBarColors();
            Charter.ChartAreas[chartAreaId].AxisX.Title = "Average Fan Speeds";
        }
        private void ShowSharesFound(string chartAreaId)
        {

            float[] values = Globals.GPUs.Select(x => (float)x.SharesFound).ToArray();
            CreateAndBindSeries(chartAreaId, chartAreaId, SeriesChartType.Column, 50, false, values);
            FixBarColors();
            Charter.ChartAreas[chartAreaId].AxisX.Title = "Shares Found";
        }
        private void ShowIncorrectShares(string chartAreaId)
        {

            float[] values = Globals.GPUs.Select(x => (float)x.IncorrectShares).ToArray();
            CreateAndBindSeries(chartAreaId, chartAreaId, SeriesChartType.Column, 50, false, values);
            FixBarColors();
            Charter.ChartAreas[chartAreaId].AxisX.Title = "Incorrect Shares";
        }
        private void ShowHashrateStandardDeviation(string chartAreaId)
        {

            float[] values = Globals.GPUs.Select(x => x.StdDevHashRate).ToArray();
            CreateAndBindSeries(chartAreaId, chartAreaId, SeriesChartType.Column, 50, false, values);
            FixBarColors();
            Charter.ChartAreas[chartAreaId].AxisX.Title = "Hashrate Standard Deviation";
        }
        private void ShowFoundOverHashrate(string chartAreaId)
        {

            float[] values = Globals.GPUs.Select(x => x.SharesFound/x.AvergaeHashRate).ToArray();
            CreateAndBindSeries(chartAreaId, chartAreaId, SeriesChartType.Column, 50, false, values);
            FixBarColors();
            Charter.ChartAreas[chartAreaId].AxisX.Title = "Shares Found / Hashrate";
        }
        private void ShowMinMaxHashrates(string chartAreaId)
        {

            float[] values = Globals.GPUs.Select(x => x.MinimumHashRate).ToArray();
            float[] values2 = Globals.GPUs.Select(x => x.MaximumHashRate).ToArray();

            CreateAndBindSeries(chartAreaId, chartAreaId, SeriesChartType.RangeColumn, 50, false, values, values2);

            values = Globals.GPUs.Select(x => x.AvergaeHashRate - 0.15f).ToArray();
            values2 = Globals.GPUs.Select(x => x.AvergaeHashRate + 0.15f).ToArray();

            CreateAndBindSeries(chartAreaId, chartAreaId+"_extra", SeriesChartType.RangeColumn, 35, false, values, values2);
           
            FixBarColors();
            Charter.ChartAreas[chartAreaId].AxisX.Title = "Hashrate Min/Max and Avr";
        }
        private void ShowMinMaxTemperatures(string chartAreaId)
        {

            float[] values = Globals.GPUs.Select(x => x.MinimumTemperature).ToArray();
            float[] values2 = Globals.GPUs.Select(x => x.MaximumTemperature).ToArray();

            CreateAndBindSeries(chartAreaId, chartAreaId, SeriesChartType.RangeColumn, 50, false, values, values2);

            values = Globals.GPUs.Select(x => x.AvergaeTemperature - 0.15f).ToArray();
            values2 = Globals.GPUs.Select(x => x.AvergaeTemperature + 0.15f).ToArray();

            CreateAndBindSeries(chartAreaId, chartAreaId + "_extra", SeriesChartType.RangeColumn, 35, false, values, values2);

            FixBarColors();
            Charter.ChartAreas[chartAreaId].AxisX.Title = "Temperature Min/Max and Avr";
        }
        private void ShowMinMaxFanSpeed(string chartAreaId)
        {

            float[] values = Globals.GPUs.Select(x => x.MinimumFanSpeed).ToArray();
            float[] values2 = Globals.GPUs.Select(x => x.MaximumFanSpeed).ToArray();

            CreateAndBindSeries(chartAreaId, chartAreaId, SeriesChartType.RangeColumn, 50, false, values, values2);

            values = Globals.GPUs.Select(x => x.AvergaeFanSpeed - 0.15f).ToArray();
            values2 = Globals.GPUs.Select(x => x.AvergaeFanSpeed + 0.15f).ToArray();

            CreateAndBindSeries(chartAreaId, chartAreaId + "_extra", SeriesChartType.RangeColumn, 35, false, values, values2);

            FixBarColors();
            Charter.ChartAreas[chartAreaId].AxisX.Title = "Fan Speed Min/Max and Avr";
            /*string seriesId = chartAreaId;
            int[][] values = Globals.GPUs.Select(x => x.GetFanValues()).ToArray();
            string[] gpuNames = Globals.GPUs.Select(x => x.Text).ToArray();

            RemoveSeriesIfExists(seriesId);

            Series series = new Series(seriesId);
            //series.YValuesPerPoint = values[0].Length;
            series.Points.DataBindXY(gpuNames, values);
            series.ChartType = SeriesChartType.BoxPlot;
            series.ChartArea = chartAreaId;
            series.IsVisibleInLegend = false;

            series["BoxPlotWhiskerPercentile"] = "10";
            series["BoxPlotShowAverage"] = "true";
            series["BoxPlotShowMedian"] = "true";
            series["BoxPlotShowUnusualValues"] = "true";

            Charter.Series.Add(series);
            Charter.ChartAreas[chartAreaId].Visible = true;*/
        }
        #endregion

        //https://stackoverflow.com/questions/36766330/detect-which-chartarea-is-being-double-clicked
        private ChartArea GetChartAreaUnderMouse(Point mouseLocation)
        {
            foreach(ChartArea chartArea in Charter.ChartAreas)
            {
                if (chartArea.Visible && ChartAreaClientRectangle(Charter, chartArea).Contains(mouseLocation))
                {
                    return chartArea;
                    break;
                }
            }
            return null;
        }
        private RectangleF ChartAreaClientRectangle(Chart chart, ChartArea CA)
        {
            RectangleF CAR = CA.Position.ToRectangleF();
            float pw = chart.ClientSize.Width / 100f;
            float ph = chart.ClientSize.Height / 100f;
            return new RectangleF(pw * CAR.X, ph * CAR.Y, pw * CAR.Width, ph * CAR.Height);
        }

        private void CreateAndBindSeries(string chartAreaId, string seriesId, SeriesChartType chartType, int maxWidth, bool withErrorBar, params float[][] YValues)
        {
            string[] gpuNames = Globals.GPUs.Select(x => x.Text).ToArray();

            RemoveSeriesIfExists(seriesId);

            Series series = new Series(seriesId);
            series.YValuesPerPoint = YValues.Length;
            series.Points.DataBindXY(gpuNames, YValues);
            series.ChartType = chartType;
            series.ChartArea = chartAreaId;
            series.IsVisibleInLegend = false;
            series["DrawSideBySide"] = "false";
            series["PointWidth"] = "0.5";
            series["MaxPixelPointWidth"] = maxWidth.ToString();

            Series errorSerie = null;
            if (withErrorBar && YValues.Length == 1)
            {
                errorSerie = new Series(seriesId + "_extra");
                errorSerie.YValuesPerPoint = 3;
                errorSerie.IsVisibleInLegend = false;
                errorSerie.Color = ThemeColors.WindowContentForegroundDarkTheme;
                errorSerie.ChartType = SeriesChartType.ErrorBar;
                errorSerie.ChartArea = chartAreaId;
                errorSerie["ErrorBarSeries"] = seriesId;
                errorSerie["ErrorBarType"] = "StandardDeviation()";
                errorSerie["ErrorBarStyle"] = "Both";
                errorSerie["ErrorBarCenterMarkerStyle"] = "Line";
                errorSerie["PointWidth"] = "0.1";
                errorSerie.MarkerStyle = MarkerStyle.None;
            }
            else if (withErrorBar)
                MessageBox.Show("Error bars for multi Y-value charts are not developed. Yet!");

            Charter.Series.Add(series);
            if (errorSerie != null) Charter.Series.Add(errorSerie);

            Charter.ChartAreas[chartAreaId].Visible = true;
        }

        private void FixBarColors()
        {
            DataPoint point;
            foreach (Series serie in Charter.Series)
            {
                for (int i = 0; i < serie.Points.Count; i++)
                {
                    point = serie.Points[i];
                    point.Color = ThemeColors.PresetColors[i];
                    //point.BorderColor = Color.Black;
                    if (serie.Name.Contains("extra"))
                        point.Color = Color.FromArgb(160, Color.White);
                }
            }
        }

        private void AddTopCombos()
        {
            //FlowLayoutPanel panel = new FlowLayoutPanel();
            TableLayoutPanel panel = new TableLayoutPanel();
            Label label;

            topLabels.Clear();

            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(20);
            panel.RowCount = 1;
            panel.ColumnCount = maxCharts;
            //panel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            for (int i = 0; i < maxCharts; i++)
            {
                label = new Label();
                label.Name = "Label_" + i.ToString();
                label.Tag = i;
                label.Text = string.Format("Chart {0}: None", i);
                label.Margin = new Padding(0, 0, 15, 0);
                label.Cursor = Cursors.Hand;
                label.AutoSize = true;
                label.Click += CreateContextMenuOnItemClick;

                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25));
                panel.Controls.Add(label, i, 0);

                topLabels.Add(label.Name, label);
            }

            PanelCombos.SuspendLayout();
            PanelCombos.Controls.Add(panel);
            PanelCombos.ResumeLayout();
        }

        private void ChangeChartTypeOnItemClick(object sender, EventArgs e)
        {
            MessageBox.Show((sender as ToolStripMenuItem).Name.ToString());
            //ShowAverageHashrates();
        }

        private void CreateContextMenuOnItemClick(object sender, EventArgs e)
        {
            Label clickedLabel = (sender as Label);
            ContextMenuStrip menu = GetContextMenu(clickedLabel);
            Point cursorPos = System.Windows.Forms.Cursor.Position;
            Point menuPos = new Point(cursorPos.X - menu.Width / 2, cursorPos.Y);
            menu.Show(menuPos);
        }
        private ContextMenuStrip GetContextMenu(Label relatedLabel)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            menu.Renderer = new MenuStripCustomRenderer();

            string senderId = relatedLabel.Tag.ToString();

            foreach (MenuItemFuncRelation menuFunc in menuItems)
            {
                if (menuFunc.text == "-")
                    menu.Items.Add(new ToolStripSeparator());
                else
                    menu.Items.Add(new ToolStripMenuItem(menuFunc.text, null, (s, ea) => { relatedLabel.Text = string.Format("Chart {0}: {1}", senderId, menuFunc.text); menuFunc.func?.Invoke(senderId); }, senderId));
            }

            return menu;
        }

        private void SetCharterVisualStyles()
        {
            Charter.BackColor = BackColor = ThemeColors.WindowContentBackgroundDarkTheme;
            Charter.ForeColor = ForeColor = ThemeColors.WindowContentForegroundDarkTheme;

            for (int i = 0; i < Charter.ChartAreas.Count; i++)
            {
                Charter.ChartAreas[i].BackColor = BackColor;
                Charter.ChartAreas[i].Visible = false;

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

                Charter.ChartAreas[i].AxisX.LabelStyle.Enabled = false;

                //Axis X Scroll Bar
                Charter.ChartAreas[i].AxisX.ScrollBar.BackColor = ThemeColors.ChartScrollBarBackColor;
                Charter.ChartAreas[i].AxisX.ScrollBar.ButtonColor = ThemeColors.ChartScrollBarButtonColor;
                Charter.ChartAreas[i].AxisX.ScrollBar.LineColor = ThemeColors.ChartScrollBarBorderColor;


                Charter.ChartAreas[i].AxisX.LabelStyle.Format = "Dd-HH:mm";

                //Setting Selection, Scrolling and Zooming
                //Charter.ChartAreas[i].CursorX.IsUserEnabled = true;
                //Charter.ChartAreas[i].CursorX.IsUserSelectionEnabled = true;
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
            /*Charter.ChartAreas[AREA_HASHRATE].AxisX.Title = "Time";
            Charter.ChartAreas[AREA_HASHRATE].AxisY.Title = "Hashrate";
            Charter.ChartAreas[AREA_HASHRATE].AxisY.Minimum = 20;

            Charter.ChartAreas[AREA_FANTEMP].AxisX.Title = "Time";
            Charter.ChartAreas[AREA_FANTEMP].AxisY.Title = "Fan Speed";
            Charter.ChartAreas[AREA_FANTEMP].AxisY.Minimum = 40;
            //Charter.ChartAreas[AREA_FANTEMP].
            //Charter.ChartAreas[AREA_FANSPEED].AxisY2.Title = "Temperature";

            Charter.ChartAreas[AREANAME_HASHRATE].AlignmentOrientation = AreaAlignmentOrientations.Vertical;
            Charter.ChartAreas[AREANAME_HASHRATE].AlignmentStyle = AreaAlignmentStyles.Cursor | AreaAlignmentStyles.PlotPosition | AreaAlignmentStyles.AxesView;
            Charter.ChartAreas[AREANAME_HASHRATE].AlignWithChartArea = AREANAME_FANTEMP;
            */
            Charter.Legends[0].Docking = Docking.Right;
        }

        private void ResetAllAreas()
        {
            PanelCombos.Controls.Clear();
            AddTopCombos();

            Charter.Series.Clear();
            Charter.ChartAreas.Clear();

            for (int i = 0; i < maxCharts; i++)
                Charter.ChartAreas.Add(i.ToString());

            SetCharterVisualStyles();
        }

        private void CreateLegendsPanel()
        {
            Charter.Legends[0].CustomItems.Clear();

            LegendItem litem;
            foreach (GpuData gpu in Globals.GPUs)
            {
                litem = new LegendItem();
                litem.Name = string.Format("GPU {0} - {1}", gpu.Index, gpu.Text);
                litem.Color = ThemeColors.PresetColors[gpu.Index];
                litem.ImageStyle = LegendImageStyle.Rectangle;
                Charter.Legends[0].CustomItems.Add(litem);
            }
            //Charter.Legends[0].DockedToChartArea = false;
            Charter.Legends[0].Docking = Docking.Bottom;
        }
        #region -- Event Subscriptions --
        [EventSubscription("topic://Document/Changed", typeof(OnUserInterface))]
        public void DocumentChangedHandler(object sender, EventArgs args)
        {
            ResetAllAreas();
            CreateLegendsPanel();

            //int labelIndex = 0;
            MenuItemFuncRelation mItem;
            for (int i=1, labelIndex=0; labelIndex<maxCharts; i++)
            {
                mItem = menuItems[i];
                if (mItem.text == "-")
                    continue;
                topLabels["Label_" + labelIndex.ToString()].Text = string.Format("Chart {0}: {1}", labelIndex, mItem.text);
                mItem.func?.Invoke(labelIndex.ToString());
                labelIndex++;
            }
        }

        [EventSubscription("topic://Gpu/Renamed", typeof(OnUserInterface))]
        public void GpuSelectedHandler(object sender, EventArgs<int> ea)
        {
            CreateLegendsPanel();
        }
        #endregion
    }
}
