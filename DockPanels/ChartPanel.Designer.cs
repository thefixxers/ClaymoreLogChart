namespace ClaymoreLogChart.DockPanels
{
    partial class ChartPanel
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.panel1 = new System.Windows.Forms.Panel();
            this.CheckBoxFan = new System.Windows.Forms.CheckBox();
            this.CheckBoxTemp = new System.Windows.Forms.CheckBox();
            this.CheckBoxHashrate = new System.Windows.Forms.CheckBox();
            this.Charter = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Charter)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CheckBoxFan);
            this.panel1.Controls.Add(this.CheckBoxTemp);
            this.panel1.Controls.Add(this.CheckBoxHashrate);
            this.panel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(624, 54);
            this.panel1.TabIndex = 0;
            // 
            // CheckBoxFan
            // 
            this.CheckBoxFan.Appearance = System.Windows.Forms.Appearance.Button;
            this.CheckBoxFan.Checked = true;
            this.CheckBoxFan.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxFan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckBoxFan.Location = new System.Drawing.Point(225, 12);
            this.CheckBoxFan.Name = "CheckBoxFan";
            this.CheckBoxFan.Size = new System.Drawing.Size(104, 27);
            this.CheckBoxFan.TabIndex = 2;
            this.CheckBoxFan.Text = "FAN SPEED";
            this.CheckBoxFan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CheckBoxFan.UseVisualStyleBackColor = true;
            // 
            // CheckBoxTemp
            // 
            this.CheckBoxTemp.Appearance = System.Windows.Forms.Appearance.Button;
            this.CheckBoxTemp.Checked = true;
            this.CheckBoxTemp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxTemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckBoxTemp.Location = new System.Drawing.Point(118, 12);
            this.CheckBoxTemp.Name = "CheckBoxTemp";
            this.CheckBoxTemp.Size = new System.Drawing.Size(104, 27);
            this.CheckBoxTemp.TabIndex = 1;
            this.CheckBoxTemp.Text = "TEMPERATURE";
            this.CheckBoxTemp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CheckBoxTemp.UseVisualStyleBackColor = true;
            // 
            // CheckBoxHashrate
            // 
            this.CheckBoxHashrate.Appearance = System.Windows.Forms.Appearance.Button;
            this.CheckBoxHashrate.Checked = true;
            this.CheckBoxHashrate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxHashrate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CheckBoxHashrate.Location = new System.Drawing.Point(12, 12);
            this.CheckBoxHashrate.Name = "CheckBoxHashrate";
            this.CheckBoxHashrate.Size = new System.Drawing.Size(104, 27);
            this.CheckBoxHashrate.TabIndex = 0;
            this.CheckBoxHashrate.Text = "HASHRATE";
            this.CheckBoxHashrate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CheckBoxHashrate.UseVisualStyleBackColor = true;
            // 
            // Charter
            // 
            chartArea1.Name = "ChartArea1";
            this.Charter.ChartAreas.Add(chartArea1);
            this.Charter.Cursor = System.Windows.Forms.Cursors.Default;
            this.Charter.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.Charter.Legends.Add(legend1);
            this.Charter.Location = new System.Drawing.Point(0, 54);
            this.Charter.Name = "Charter";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.Charter.Series.Add(series1);
            this.Charter.Size = new System.Drawing.Size(624, 431);
            this.Charter.TabIndex = 1;
            this.Charter.Text = "chart1";
            // 
            // ChartPanel
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 485);
            this.Controls.Add(this.Charter);
            this.Controls.Add(this.panel1);
            this.Cursor = System.Windows.Forms.Cursors.PanSouth;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ChartPanel";
            this.Text = "ChartPanel";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Charter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataVisualization.Charting.Chart Charter;
        private System.Windows.Forms.CheckBox CheckBoxHashrate;
        private System.Windows.Forms.CheckBox CheckBoxFan;
        private System.Windows.Forms.CheckBox CheckBoxTemp;
    }
}