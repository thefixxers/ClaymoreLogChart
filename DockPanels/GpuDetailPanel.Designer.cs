namespace ClaymoreLogChart.DockPanels
{
    partial class GpuDetailPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GpuDetailPanel));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ObjectListGpus = new BrightIdeasSoftware.ObjectListView();
            this.ColumnColor = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnBrand = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnCClock = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnMClock = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnCvddc = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnMvddc = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnPowLim = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnTT = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnAvgHash = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnSdHash = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnAvgTemp = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnAvgFan = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.SmallImageList = new System.Windows.Forms.ImageList(this.components);
            this.ColumnIndex = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ObjectListGpus)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.panel1.Controls.Add(this.ObjectListGpus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(25);
            this.panel1.Size = new System.Drawing.Size(1034, 781);
            this.panel1.TabIndex = 17;
            // 
            // ObjectListGpus
            // 
            this.ObjectListGpus.AllColumns.Add(this.ColumnColor);
            this.ObjectListGpus.AllColumns.Add(this.ColumnBrand);
            this.ObjectListGpus.AllColumns.Add(this.ColumnName);
            this.ObjectListGpus.AllColumns.Add(this.ColumnCClock);
            this.ObjectListGpus.AllColumns.Add(this.ColumnMClock);
            this.ObjectListGpus.AllColumns.Add(this.ColumnCvddc);
            this.ObjectListGpus.AllColumns.Add(this.ColumnMvddc);
            this.ObjectListGpus.AllColumns.Add(this.ColumnPowLim);
            this.ObjectListGpus.AllColumns.Add(this.ColumnTT);
            this.ObjectListGpus.AllColumns.Add(this.ColumnAvgHash);
            this.ObjectListGpus.AllColumns.Add(this.ColumnSdHash);
            this.ObjectListGpus.AllColumns.Add(this.ColumnAvgTemp);
            this.ObjectListGpus.AllColumns.Add(this.ColumnAvgFan);
            this.ObjectListGpus.AllowDrop = true;
            this.ObjectListGpus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.ObjectListGpus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ObjectListGpus.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.ObjectListGpus.CellEditUseWholeCell = false;
            this.ObjectListGpus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnColor,
            this.ColumnBrand,
            this.ColumnName,
            this.ColumnCClock,
            this.ColumnMClock,
            this.ColumnCvddc,
            this.ColumnMvddc,
            this.ColumnPowLim,
            this.ColumnTT,
            this.ColumnAvgHash,
            this.ColumnSdHash,
            this.ColumnAvgTemp,
            this.ColumnAvgFan});
            this.ObjectListGpus.Cursor = System.Windows.Forms.Cursors.Default;
            this.ObjectListGpus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObjectListGpus.EmptyListMsg = "No Log File Loaded.";
            this.ObjectListGpus.EmptyListMsgFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectListGpus.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectListGpus.FullRowSelect = true;
            this.ObjectListGpus.HasCollapsibleGroups = false;
            this.ObjectListGpus.HideSelection = false;
            this.ObjectListGpus.Location = new System.Drawing.Point(25, 25);
            this.ObjectListGpus.Margin = new System.Windows.Forms.Padding(12, 13, 12, 13);
            this.ObjectListGpus.Name = "ObjectListGpus";
            this.ObjectListGpus.OwnerDraw = false;
            this.ObjectListGpus.RowHeight = 24;
            this.ObjectListGpus.Size = new System.Drawing.Size(984, 731);
            this.ObjectListGpus.SmallImageList = this.SmallImageList;
            this.ObjectListGpus.TabIndex = 16;
            this.ObjectListGpus.UseCompatibleStateImageBehavior = false;
            this.ObjectListGpus.View = System.Windows.Forms.View.Details;
            // 
            // ColumnColor
            // 
            this.ColumnColor.AspectName = "Index";
            this.ColumnColor.Groupable = false;
            this.ColumnColor.IsEditable = false;
            this.ColumnColor.Searchable = false;
            this.ColumnColor.Sortable = false;
            this.ColumnColor.Text = "GPU#";
            this.ColumnColor.Width = 55;
            // 
            // ColumnBrand
            // 
            this.ColumnBrand.DisplayIndex = 2;
            this.ColumnBrand.Groupable = false;
            this.ColumnBrand.Sortable = false;
            this.ColumnBrand.Text = "";
            this.ColumnBrand.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnBrand.Width = 100;
            // 
            // ColumnName
            // 
            this.ColumnName.AspectName = "Text";
            this.ColumnName.DisplayIndex = 1;
            this.ColumnName.FillsFreeSpace = true;
            this.ColumnName.Groupable = false;
            this.ColumnName.Hideable = false;
            this.ColumnName.Sortable = false;
            this.ColumnName.Text = "Name";
            // 
            // ColumnCClock
            // 
            this.ColumnCClock.AspectName = "CoreClock";
            this.ColumnCClock.Groupable = false;
            this.ColumnCClock.Sortable = false;
            this.ColumnCClock.Text = "cclock";
            this.ColumnCClock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnCClock.Width = 80;
            // 
            // ColumnMClock
            // 
            this.ColumnMClock.AspectName = "MemoryClock";
            this.ColumnMClock.Groupable = false;
            this.ColumnMClock.Sortable = false;
            this.ColumnMClock.Text = "mclock";
            this.ColumnMClock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnMClock.Width = 80;
            // 
            // ColumnCvddc
            // 
            this.ColumnCvddc.AspectName = "CoreVoltage";
            this.ColumnCvddc.Groupable = false;
            this.ColumnCvddc.Sortable = false;
            this.ColumnCvddc.Text = "cvddc";
            this.ColumnCvddc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnCvddc.Width = 80;
            // 
            // ColumnMvddc
            // 
            this.ColumnMvddc.AspectName = "MemoryVoltage";
            this.ColumnMvddc.Groupable = false;
            this.ColumnMvddc.Sortable = false;
            this.ColumnMvddc.Text = "mvddc";
            this.ColumnMvddc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnMvddc.Width = 80;
            // 
            // ColumnPowLim
            // 
            this.ColumnPowLim.AspectName = "PowerLimit";
            this.ColumnPowLim.Groupable = false;
            this.ColumnPowLim.Sortable = false;
            this.ColumnPowLim.Text = "powlim";
            this.ColumnPowLim.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnPowLim.Width = 80;
            // 
            // ColumnTT
            // 
            this.ColumnTT.AspectName = "GoalTemperature";
            this.ColumnTT.Groupable = false;
            this.ColumnTT.Sortable = false;
            this.ColumnTT.Text = "tt";
            this.ColumnTT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnTT.Width = 80;
            // 
            // ColumnAvgHash
            // 
            this.ColumnAvgHash.AspectName = "";
            this.ColumnAvgHash.Groupable = false;
            this.ColumnAvgHash.Sortable = false;
            this.ColumnAvgHash.Text = "Avg. Rate";
            this.ColumnAvgHash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnAvgHash.Width = 80;
            // 
            // ColumnSdHash
            // 
            this.ColumnSdHash.AspectName = "";
            this.ColumnSdHash.Groupable = false;
            this.ColumnSdHash.Sortable = false;
            this.ColumnSdHash.Text = "StdDev";
            this.ColumnSdHash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnSdHash.Width = 80;
            // 
            // ColumnAvgTemp
            // 
            this.ColumnAvgTemp.AspectName = "";
            this.ColumnAvgTemp.Groupable = false;
            this.ColumnAvgTemp.Sortable = false;
            this.ColumnAvgTemp.Text = "Avg. Temp.";
            this.ColumnAvgTemp.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnAvgTemp.Width = 80;
            // 
            // ColumnAvgFan
            // 
            this.ColumnAvgFan.AspectName = "";
            this.ColumnAvgFan.Groupable = false;
            this.ColumnAvgFan.Sortable = false;
            this.ColumnAvgFan.Text = "Avg. Fan";
            this.ColumnAvgFan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ColumnAvgFan.Width = 80;
            this.ColumnAvgFan.WordWrap = true;
            // 
            // SmallImageList
            // 
            this.SmallImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("SmallImageList.ImageStream")));
            this.SmallImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.SmallImageList.Images.SetKeyName(0, "amd_full.png");
            this.SmallImageList.Images.SetKeyName(1, "nvidia_full.png");
            // 
            // ColumnIndex
            // 
            this.ColumnIndex.AspectName = "Index";
            this.ColumnIndex.DisplayIndex = 0;
            this.ColumnIndex.Groupable = false;
            this.ColumnIndex.IsEditable = false;
            this.ColumnIndex.IsVisible = false;
            this.ColumnIndex.Text = "";
            this.ColumnIndex.Width = 30;
            // 
            // GpuDetailPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(1034, 781);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "GpuDetailPanel";
            this.Text = "GpuDetailPanel";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ObjectListGpus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public BrightIdeasSoftware.ObjectListView ObjectListGpus;
        private BrightIdeasSoftware.OLVColumn ColumnName;
        private BrightIdeasSoftware.OLVColumn ColumnColor;
        private BrightIdeasSoftware.OLVColumn ColumnBrand;
        private BrightIdeasSoftware.OLVColumn ColumnCClock;
        private BrightIdeasSoftware.OLVColumn ColumnMClock;
        private BrightIdeasSoftware.OLVColumn ColumnCvddc;
        private BrightIdeasSoftware.OLVColumn ColumnMvddc;
        private BrightIdeasSoftware.OLVColumn ColumnPowLim;
        private BrightIdeasSoftware.OLVColumn ColumnTT;
        private BrightIdeasSoftware.OLVColumn ColumnAvgHash;
        private BrightIdeasSoftware.OLVColumn ColumnSdHash;
        private BrightIdeasSoftware.OLVColumn ColumnAvgTemp;
        private BrightIdeasSoftware.OLVColumn ColumnAvgFan;
        private System.Windows.Forms.ImageList SmallImageList;
        private BrightIdeasSoftware.OLVColumn ColumnIndex;
    }
}