namespace ClaymoreLogChart.DockPanels
{
    partial class GpusPanel
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.ObjectListGpus = new BrightIdeasSoftware.ObjectListView();
            this.ColumnIndex = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ColumnColor = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
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
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(272, 339);
            this.panel1.TabIndex = 16;
            // 
            // ObjectListGpus
            // 
            this.ObjectListGpus.AllColumns.Add(this.ColumnIndex);
            this.ObjectListGpus.AllColumns.Add(this.ColumnName);
            this.ObjectListGpus.AllColumns.Add(this.ColumnColor);
            this.ObjectListGpus.AllowDrop = true;
            this.ObjectListGpus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.ObjectListGpus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ObjectListGpus.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.ObjectListGpus.CellEditUseWholeCell = false;
            this.ObjectListGpus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnIndex,
            this.ColumnName,
            this.ColumnColor});
            this.ObjectListGpus.Cursor = System.Windows.Forms.Cursors.Default;
            this.ObjectListGpus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ObjectListGpus.EmptyListMsg = "No Log File Loaded.";
            this.ObjectListGpus.EmptyListMsgFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectListGpus.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ObjectListGpus.FullRowSelect = true;
            this.ObjectListGpus.HasCollapsibleGroups = false;
            this.ObjectListGpus.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ObjectListGpus.HideSelection = false;
            this.ObjectListGpus.Location = new System.Drawing.Point(10, 10);
            this.ObjectListGpus.Margin = new System.Windows.Forms.Padding(10);
            this.ObjectListGpus.Name = "ObjectListGpus";
            this.ObjectListGpus.OwnerDrawnHeader = true;
            this.ObjectListGpus.Size = new System.Drawing.Size(252, 319);
            this.ObjectListGpus.TabIndex = 16;
            this.ObjectListGpus.UseCompatibleStateImageBehavior = false;
            this.ObjectListGpus.View = System.Windows.Forms.View.Details;
            // 
            // ColumnIndex
            // 
            this.ColumnIndex.AspectName = "Index";
            this.ColumnIndex.Groupable = false;
            this.ColumnIndex.IsEditable = false;
            this.ColumnIndex.Text = "";
            this.ColumnIndex.Width = 30;
            // 
            // ColumnName
            // 
            this.ColumnName.AspectName = "";
            this.ColumnName.FillsFreeSpace = true;
            this.ColumnName.Groupable = false;
            this.ColumnName.Hideable = false;
            this.ColumnName.Text = "Name";
            // 
            // ColumnColor
            // 
            this.ColumnColor.AspectName = "Index";
            this.ColumnColor.IsEditable = false;
            this.ColumnColor.Width = 20;
            // 
            // GpusPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(37)))), ((int)(((byte)(37)))), ((int)(((byte)(38)))));
            this.ClientSize = new System.Drawing.Size(272, 339);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "GpusPanel";
            this.Text = "GPUs";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ObjectListGpus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        public BrightIdeasSoftware.ObjectListView ObjectListGpus;
        private BrightIdeasSoftware.OLVColumn ColumnIndex;
        private BrightIdeasSoftware.OLVColumn ColumnName;
        private BrightIdeasSoftware.OLVColumn ColumnColor;
    }
}