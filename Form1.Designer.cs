namespace ClaymoreLogChart
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.MenuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemOpenLog = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemOpenFiltered = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemOpenGpuNames = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemOpenProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemSaveFiltered = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSaveGpuNames = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemSaveProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuView = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemGpuPanel = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemHashrateStats = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemTempFanStats = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItemStatisticalCharts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.MenuItemNewHistoryChart = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.PanelMain = new System.Windows.Forms.Panel();
            this.MainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.BackColor = System.Drawing.SystemColors.Control;
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuFile,
            this.MenuView,
            this.aboutToolStripMenuItem});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(1177, 24);
            this.MainMenuStrip.TabIndex = 0;
            this.MainMenuStrip.Text = "menuStrip1";
            // 
            // MenuFile
            // 
            this.MenuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemOpenLog,
            this.MenuItemOpenFiltered,
            this.MenuItemOpenGpuNames,
            this.MenuItemOpenProject,
            this.toolStripMenuItem4,
            this.MenuItemSaveFiltered,
            this.MenuItemSaveGpuNames,
            this.MenuItemSaveProject,
            this.toolStripMenuItem1,
            this.MenuItemExit});
            this.MenuFile.Name = "MenuFile";
            this.MenuFile.Size = new System.Drawing.Size(37, 20);
            this.MenuFile.Text = "&File";
            // 
            // MenuItemOpenLog
            // 
            this.MenuItemOpenLog.Name = "MenuItemOpenLog";
            this.MenuItemOpenLog.Size = new System.Drawing.Size(201, 22);
            this.MenuItemOpenLog.Text = "Open Claymore Log File";
            this.MenuItemOpenLog.Click += new System.EventHandler(this.MenuItemOpenLog_Click);
            // 
            // MenuItemOpenFiltered
            // 
            this.MenuItemOpenFiltered.Name = "MenuItemOpenFiltered";
            this.MenuItemOpenFiltered.Size = new System.Drawing.Size(201, 22);
            this.MenuItemOpenFiltered.Text = "Open Filtered Log";
            this.MenuItemOpenFiltered.Visible = false;
            // 
            // MenuItemOpenGpuNames
            // 
            this.MenuItemOpenGpuNames.Name = "MenuItemOpenGpuNames";
            this.MenuItemOpenGpuNames.Size = new System.Drawing.Size(201, 22);
            this.MenuItemOpenGpuNames.Text = "Import GPU Names File";
            this.MenuItemOpenGpuNames.Click += new System.EventHandler(this.MenuItemOpenGpuNames_Click);
            // 
            // MenuItemOpenProject
            // 
            this.MenuItemOpenProject.Name = "MenuItemOpenProject";
            this.MenuItemOpenProject.Size = new System.Drawing.Size(201, 22);
            this.MenuItemOpenProject.Text = "Open Project";
            this.MenuItemOpenProject.Visible = false;
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(198, 6);
            this.toolStripMenuItem4.Visible = false;
            // 
            // MenuItemSaveFiltered
            // 
            this.MenuItemSaveFiltered.Name = "MenuItemSaveFiltered";
            this.MenuItemSaveFiltered.Size = new System.Drawing.Size(201, 22);
            this.MenuItemSaveFiltered.Text = "Save Filtered Log";
            this.MenuItemSaveFiltered.Visible = false;
            // 
            // MenuItemSaveGpuNames
            // 
            this.MenuItemSaveGpuNames.Name = "MenuItemSaveGpuNames";
            this.MenuItemSaveGpuNames.Size = new System.Drawing.Size(201, 22);
            this.MenuItemSaveGpuNames.Text = "Save GPU Names";
            this.MenuItemSaveGpuNames.Click += new System.EventHandler(this.MenuItemSaveGpuNames_Click);
            // 
            // MenuItemSaveProject
            // 
            this.MenuItemSaveProject.Name = "MenuItemSaveProject";
            this.MenuItemSaveProject.Size = new System.Drawing.Size(201, 22);
            this.MenuItemSaveProject.Text = "Save Project";
            this.MenuItemSaveProject.Visible = false;
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(198, 6);
            // 
            // MenuItemExit
            // 
            this.MenuItemExit.Name = "MenuItemExit";
            this.MenuItemExit.Size = new System.Drawing.Size(201, 22);
            this.MenuItemExit.Text = "Exit";
            this.MenuItemExit.Click += new System.EventHandler(this.MenuItemExit_Click);
            // 
            // MenuView
            // 
            this.MenuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemGpuPanel,
            this.MenuItemHashrateStats,
            this.MenuItemTempFanStats,
            this.MenuItemStatisticalCharts,
            this.toolStripMenuItem2,
            this.MenuItemNewHistoryChart,
            this.toolStripMenuItem3});
            this.MenuView.Name = "MenuView";
            this.MenuView.Size = new System.Drawing.Size(44, 20);
            this.MenuView.Text = "View";
            // 
            // MenuItemGpuPanel
            // 
            this.MenuItemGpuPanel.Checked = true;
            this.MenuItemGpuPanel.CheckOnClick = true;
            this.MenuItemGpuPanel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuItemGpuPanel.Name = "MenuItemGpuPanel";
            this.MenuItemGpuPanel.Size = new System.Drawing.Size(244, 22);
            this.MenuItemGpuPanel.Text = "GPUs";
            // 
            // MenuItemHashrateStats
            // 
            this.MenuItemHashrateStats.Checked = true;
            this.MenuItemHashrateStats.CheckOnClick = true;
            this.MenuItemHashrateStats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuItemHashrateStats.Name = "MenuItemHashrateStats";
            this.MenuItemHashrateStats.Size = new System.Drawing.Size(244, 22);
            this.MenuItemHashrateStats.Text = "GPU Hashrate Stats";
            // 
            // MenuItemTempFanStats
            // 
            this.MenuItemTempFanStats.Checked = true;
            this.MenuItemTempFanStats.CheckOnClick = true;
            this.MenuItemTempFanStats.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuItemTempFanStats.Name = "MenuItemTempFanStats";
            this.MenuItemTempFanStats.Size = new System.Drawing.Size(244, 22);
            this.MenuItemTempFanStats.Text = "GPU Temperature and Fan Stats.";
            // 
            // MenuItemStatisticalCharts
            // 
            this.MenuItemStatisticalCharts.Checked = true;
            this.MenuItemStatisticalCharts.CheckOnClick = true;
            this.MenuItemStatisticalCharts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MenuItemStatisticalCharts.Name = "MenuItemStatisticalCharts";
            this.MenuItemStatisticalCharts.Size = new System.Drawing.Size(244, 22);
            this.MenuItemStatisticalCharts.Text = "Statistical Charts Window";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(241, 6);
            // 
            // MenuItemNewHistoryChart
            // 
            this.MenuItemNewHistoryChart.Name = "MenuItemNewHistoryChart";
            this.MenuItemNewHistoryChart.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.MenuItemNewHistoryChart.Size = new System.Drawing.Size(244, 22);
            this.MenuItemNewHistoryChart.Text = "New History Chart Window";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(241, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(107, 22);
            this.mnuAbout.Text = "About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // PanelMain
            // 
            this.PanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelMain.Location = new System.Drawing.Point(0, 24);
            this.PanelMain.Name = "PanelMain";
            this.PanelMain.Size = new System.Drawing.Size(1177, 642);
            this.PanelMain.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(1177, 666);
            this.Controls.Add(this.PanelMain);
            this.Controls.Add(this.MainMenuStrip);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Log Chart for Claymore\'s ETH Miner";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuFile;
        private System.Windows.Forms.ToolStripMenuItem MenuItemOpenLog;
        private System.Windows.Forms.ToolStripMenuItem MenuItemOpenFiltered;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSaveFiltered;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MenuItemExit;
        private System.Windows.Forms.Panel PanelMain;
        private System.Windows.Forms.ToolStripMenuItem MenuView;
        private System.Windows.Forms.ToolStripMenuItem MenuItemGpuPanel;
        private System.Windows.Forms.ToolStripMenuItem MenuItemHashrateStats;
        private System.Windows.Forms.ToolStripMenuItem MenuItemTempFanStats;
        private System.Windows.Forms.ToolStripMenuItem MenuItemStatisticalCharts;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem MenuItemNewHistoryChart;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripMenuItem MenuItemOpenGpuNames;
        private System.Windows.Forms.ToolStripMenuItem MenuItemOpenProject;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSaveGpuNames;
        private System.Windows.Forms.ToolStripMenuItem MenuItemSaveProject;
    }
}

