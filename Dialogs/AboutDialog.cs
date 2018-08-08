using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ClaymoreLogChart.VisualStylers;

namespace ClaymoreLogChart.Dialogs
{
    public partial class AboutDialog : Form
    {
        public AboutDialog()
        {
            InitializeComponent();

            this.BackColor = rtbText.BackColor = ThemeColors.WindowContentBackgroundDarkTheme;
            this.ForeColor = rtbText.ForeColor = ThemeColors.WindowContentForegroundDarkTheme;

            rtbText.LinkClicked += RtbText_LinkClicked;
        }

        private void RtbText_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        private void AboutDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
