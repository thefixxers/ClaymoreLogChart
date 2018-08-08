using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using WeifenLuo.WinFormsUI.Docking;
using WeifenLuo.WinFormsUI.Docking.Configuration;
using BrightIdeasSoftware;
using Appccelerate.Events;
using Appccelerate.EventBroker;
using Appccelerate.EventBroker.Handlers;

using ClaymoreLogChart.DataModel;
using ClaymoreLogChart.Parsers;
using ClaymoreLogChart.Dialogs;
using ClaymoreLogChart.DockPanels;
using ClaymoreLogChart.Properties;
using ClaymoreLogChart.VisualStylers;

namespace ClaymoreLogChart
{
    public partial class Form1 : Form
    {
        BackgroundLogParser parser;

        private DockPanel mainPanel = new DockPanel();
        private GpusPanel gpuPanel = new GpusPanel();
        private HashrateStatsPanel hashPanel = new HashrateStatsPanel();
        private TempFanStatsPanel tempPanel = new TempFanStatsPanel();
        private StatisticsChartsPanel statsPanel = new StatisticsChartsPanel();
        private List<ChartPanel> chartPanels = new List<ChartPanel>();

        private EventBroker shadowBroker = new EventBroker();

        private string currentFile = "";

        public Form1()
        {
            InitializeComponent();

            MainMenuStrip.BackColor = ThemeColors.WindowContentBackgroundDarkTheme;
            MainMenuStrip.ForeColor = ThemeColors.WindowContentForegroundDarkTheme;
            MainMenuStrip.Renderer = new MenuStripCustomRenderer();

            mainPanel.DocumentStyle = DocumentStyle.DockingWindow;
            mainPanel.Theme = new VS2015DarkTheme();
            mainPanel.Dock = DockStyle.Fill;

            PanelMain.SuspendLayout();
            PanelMain.Controls.Add(mainPanel);
            PanelMain.ResumeLayout();

            DoubleBuffered = true;

            MenuItemNewHistoryChart.Click += MenuItemNewHistoryChart_Click;
        }

        private void MenuItemNewHistoryChart_Click(object sender, EventArgs e)
        {
            int numCharts = chartPanels.Count;
            string chartId = (numCharts + 1).ToString();
            string menuText = "History Chart " + chartId;
            string menuItemName = "MenuItem_Chart_" + chartId;
            ChartPanel newPanel = new ChartPanel();
            newPanel.FormClosing += (s, a) => { RemoveChartPanelFromBrokerAndMenu(newPanel); };
            newPanel.Text = menuText;
            newPanel.Tag = menuItemName;
            newPanel.Show(mainPanel, DockState.Document);
            chartPanels.Add(newPanel);

            shadowBroker.Register(newPanel);

            ToolStripMenuItem newMenuItem = new ToolStripMenuItem(menuText, null, HistoryChartMenuItemClicked /*(s, ea) => { chartPanels.Find(x => x.Text == menuText)?.Focus(); }*/, menuItemName);
            MenuView.DropDownItems.Add(newMenuItem);
        }
        private void HistoryChartMenuItemClicked(object sender, EventArgs ea)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            
            ChartPanel relatedPanel = chartPanels.Find(x => x.Text == menuItem.Text);
            //MessageBox.Show(relatedPanel.Text);
            relatedPanel.Show();
        }
        private void RemoveChartPanelFromBrokerAndMenu(ChartPanel panel)
        {
            chartPanels.Remove(panel);
            shadowBroker.Unregister(panel);

            ToolStripItem[] items = MenuView.DropDownItems.Find(panel.Tag.ToString(), true);
            if (items != null && items.Length > 0)
                MenuView.DropDownItems.Remove(items[0]);
        }

        private IDockContent InstanciateDockContentPanel(string objectType)
        {
            IDockContent[] defaultPanels = new IDockContent[] { gpuPanel, hashPanel, tempPanel, statsPanel };
            foreach (IDockContent panel in defaultPanels)
                if (panel.GetType().ToString() == objectType)
                    return panel;

            return null;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //Loading Prveious Window Location and State
            

            BindPanelVisibilityToMenuItem(gpuPanel, MenuItemGpuPanel);
            BindPanelVisibilityToMenuItem(hashPanel, MenuItemHashrateStats);
            BindPanelVisibilityToMenuItem(tempPanel, MenuItemTempFanStats);
            BindPanelVisibilityToMenuItem(statsPanel, MenuItemStatisticalCharts);

            LoadWindowAndPanelStates();

            shadowBroker.Register(this);
            shadowBroker.Register(gpuPanel);
            shadowBroker.Register(hashPanel);
            shadowBroker.Register(tempPanel);
            shadowBroker.Register(statsPanel);
            

#if DEBUG
            //ParseLogFile("..\\..\\Logs\\1522270644_log.txt");
#endif
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            
        }

        private void BindPanelVisibilityToMenuItem(DockContent panel, ToolStripMenuItem menuItem)
        {
            Binding b = panel.DataBindings.Add("IsHidden", menuItem, "Checked");
            b.Format += (obj, arg) => { arg.Value = !(bool)arg.Value; };
            b.Parse += (obj, arg) => { arg.Value = !(bool)arg.Value; };
        }

        private void MenuItemOpenLog_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            if(fileDlg.ShowDialog() == DialogResult.OK)
            {
                ParseLogFile(fileDlg.FileName);
                currentFile = fileDlg.FileName;

                string saveDirectory = Path.GetDirectoryName(currentFile);
                string gpusFileName = Path.Combine(saveDirectory, Globals.GpuNamesFileName);
                if (File.Exists(gpusFileName))
                    LoadGpuNames(gpusFileName);
            }
        }

        private void LoadGpuNames(string gpusFileName)
        {
            /*string saveDirectory = Path.GetDirectoryName(currentFile);
            string gpusFileName = Path.Combine(saveDirectory, Globals.GpuNamesFileName);*/

            using (StreamReader sr = new StreamReader(gpusFileName))
            {
                int numGpus = 0;
                if (!int.TryParse(sr.ReadLine(), out numGpus))
                    return;

                if(numGpus!=Globals.GPUs.Count)
                {
                    MessageBox.Show("Number of GPUs in the log file do not match the number of GPUs in the GPU Names file. GPU names are ignored.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                int gpuId;
                string pciPort;
                string orgName;
                string nickname;
                string[] dataParts;
                for (int i = 0; i < Globals.GPUs.Count; i++)
                {
                    dataParts = sr.ReadLine().Split('\t');
                    if(dataParts.Length!=4 || !int.TryParse(dataParts[0], out gpuId))
                    {
                        MessageBox.Show("GPU Names file seems to be corrupt. GPU names are ignored.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    pciPort = dataParts[1];
                    orgName = dataParts[2];
                    nickname = dataParts[3];

                    if (gpuId == Globals.GPUs[i].Index &&
                        pciPort == Globals.GPUs[i].PciePort &&
                        orgName == Globals.GPUs[i].Name)
                    {
                        Globals.GPUs[i].NickName = nickname;
                    }
                    else
                    {
                        MessageBox.Show("GPU{0] does not match the information dtored in GPU Names file. GPU Names are ignored.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        foreach (GpuData gpu in Globals.GPUs)
                            gpu.NickName = string.Empty;
                        return;
                    }
                }
                sr.Close();
            }

            foreach (GpuData gpu in Globals.GPUs)
                GpuRenamed?.Invoke(this, new EventArgs<int>(gpu.Index));
        }

        private void ParseLogFile(string filename)
        {
            parser = new BackgroundLogParser(filename);
            LoadLogFileDialog loadDlg = new Dialogs.LoadLogFileDialog(
                parser,
                (s, ea) => { this.Enabled = false; },
                (s, ea) => { this.Enabled = true; this.Focus(); },
                filename
                );

            loadDlg.ShowDialog();

            gpuPanel.SetObjects(parser.gpus.Values.ToList());

            Globals.GPUs = parser.gpus.Values.ToList();
            DocumentChanged?.Invoke(this, null);
        }


        #region -- Event Publications --
        [EventPublication("topic://Document/Changed")]
        public event EventHandler DocumentChanged;
        [EventPublication("topic://Gpu/Renamed")]
        public event EventHandler<EventArgs<int>> GpuRenamed;
        #endregion

        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadWindowAndPanelStates()
        {
            //https://stackoverflow.com/questions/1873658/net-windows-forms-remember-windows-size-and-location
            WindowState = FormWindowState.Normal;
            DesktopBounds = Settings.Default.WindowPosition;
            //Window State
            if (Settings.Default.IsMaximized)
                WindowState = FormWindowState.Maximized;
            else if (Screen.AllScreens.Any(screen => screen.WorkingArea.IntersectsWith(Settings.Default.WindowPosition)))
            {
                StartPosition = FormStartPosition.Manual;
                WindowState = FormWindowState.Normal;
            }


            //Panel STates
            if (System.IO.File.Exists("layout.xml"))
            {
                mainPanel.LoadFromXml("layout.xml", InstanciateDockContentPanel);
            }
            else
            {
                gpuPanel.Show(mainPanel, DockState.DockLeft);

                /*ChartPanel p = new ChartPanel();
                p.Show(mainPanel, DockState.Document);

                p = new DockPanels.ChartPanel();
                p.Show(mainPanel, DockState.Document);*/

                hashPanel.Show(gpuPanel.Pane, DockAlignment.Bottom, 0.33);
                tempPanel.Show(gpuPanel.Pane, DockAlignment.Bottom, 0.5);
                statsPanel.Show(mainPanel, DockState.Document);
            }
        }
        private void SaveWindowAndPanelsStates()
        {
            mainPanel.SaveAsXml("layout.xml");

            Settings.Default.IsMaximized = WindowState == FormWindowState.Maximized;
            Settings.Default.WindowPosition = DesktopBounds;
            Settings.Default.Save();

            Settings.Default.Save();

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveWindowAndPanelsStates();
        }

        private void mnuAbout_Click(object sender, EventArgs e)
        {
            (new AboutDialog()).ShowDialog();
        }

        private void MenuItemSaveGpuNames_Click(object sender, EventArgs e)
        {
            string saveDirectory = Path.GetDirectoryName(currentFile);
            using (StreamWriter sw = new StreamWriter(Path.Combine(saveDirectory, Globals.GpuNamesFileName)))
            {
                sw.WriteLine(Globals.GPUs.Count);
                foreach (GpuData gpu in Globals.GPUs)
                    sw.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", gpu.Index, gpu.PciePort, gpu.Name, gpu.NickName));
                sw.Close();
            }
        }

        private void MenuItemOpenGpuNames_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                LoadGpuNames(fileDlg.FileName);
            }
        }
    }
}
