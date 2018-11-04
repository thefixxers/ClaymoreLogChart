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


using ClaymoreLogChart.DataModel;
using ClaymoreLogChart.VisualStylers;
using Appccelerate.Events;

namespace ClaymoreLogChart.DockPanels
{
    public partial class TempFanStatsPanel : DockContent
    {
        public TempFanStatsPanel()
        {
            InitializeComponent();

            BackColor = ThemeColors.WindowContentBackgroundDarkTheme;
            ForeColor = ThemeColors.WindowContentForegroundDarkTheme;

            
            MinLabel.ForeColor =
                AverageLabel.ForeColor = 
                MaxLabel.ForeColor =
                SdLabel.ForeColor =
                ThemeColors.FocusedSelectedBackgroundColor;
            FanMinLabel.ForeColor =
                FanAverageLabel.ForeColor =
                FanMaxLabel.ForeColor =
                FanSdLabel.ForeColor =
                ThemeColors.FocusedSelectedBackgroundColor;

            //Drag & Drop
            DragEnter += HashrateStatsPanel_DragEnter;
            DragOver += HashrateStatsPanel_DragOver;
            DragDrop += HashrateStatsPanel_DragDrop;

            UpdateValues(null);
        }

        private void HashrateStatsPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void HashrateStatsPanel_DragDrop(object sender, DragEventArgs e)
        {
            List<GpuData> gpus = (List<GpuData>)e.Data.GetData(typeof(List<GpuData>));
            UpdateValues(gpus[0]);
        }

        private void HashrateStatsPanel_DragOver(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
            CursorManager.Instance.EffectMode = CursorManager.Effect.Copy;
        }

        private void UpdateValues(GpuData gpu)
        {
            if (gpu != null)
            {
                PanelTitleLabel.BackColor  = ThemeColors.PresetColors[gpu.Index];
                PanelTitleLabel.Text = string.Format("GPU {0} - {1}", gpu.Index, gpu.Text);
                AverageLabel.Text = gpu.AvergaeTemperature.ToString("n2");
                MinLabel.Text = gpu.MinimumTemperature.ToString("n0");
                MaxLabel.Text = gpu.MaximumTemperature.ToString("n0");
                SdLabel.Text = gpu.StdDevTemperature.ToString("n2");


                FanAverageLabel.Text = gpu.AvergaeFanSpeed.ToString("n2");
                FanMinLabel.Text = gpu.MinimumFanSpeed.ToString("n0");
                FanMaxLabel.Text = gpu.MaximumFanSpeed.ToString("n0");
                FanSdLabel.Text = gpu.StdDevFanSpeed.ToString("n2");
            }
            else
            {
                PanelTitleLabel.BackColor = this.BackColor;
                PanelTitleLabel.Text = "";
                AverageLabel.Text = "N/A";
                MinLabel.Text = "N/A";
                MaxLabel.Text = "N/A";
                SdLabel.Text = "N/A";


                FanAverageLabel.Text = "N/A";
                FanMinLabel.Text = "N/A";
                FanMaxLabel.Text = "N/A";
                FanSdLabel.Text = "N/A";
            }
             
        }

        #region -- Event Subscriptions --
        [EventSubscription("topic://Gpu/Renamed", typeof(OnUserInterface))]
        [EventSubscription("topic://Gpu/Selected", typeof(OnUserInterface))]
        public void GpuSelectedHandler(object sender, EventArgs<int> ea)
        {
            GpuData gpu = Globals.GPUs[ea.Value];
            UpdateValues(gpu);
        }
        #endregion
    }
}
