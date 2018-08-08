using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ClaymoreLogChart.Parsers;
using ClaymoreLogChart.VisualStylers;

namespace ClaymoreLogChart.Dialogs
{
    public partial class GpuRenameDialog : Form
    {
        public GpuRenameDialog()
        {
            InitializeComponent();

            this.BackColor = ThemeColors.WindowContentBackgroundDarkTheme;
            this.ForeColor = ThemeColors.WindowContentForegroundDarkTheme;

            LabelGpuName.Text = "Nickname:";
            this.Activated += GpuRenameDialog_Activated;
            
        }

        private void GpuRenameDialog_Activated(object sender, EventArgs e)
        {
            TextBoxNickname.Focus();
        }
    }
}
