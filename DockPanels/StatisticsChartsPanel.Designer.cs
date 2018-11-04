namespace ClaymoreLogChart.DockPanels
{
    partial class StatisticsChartsPanel
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
            this.PanelCombos = new System.Windows.Forms.Panel();
            this.Charter = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.Charter)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelCombos
            // 
            this.PanelCombos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PanelCombos.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelCombos.Location = new System.Drawing.Point(0, 0);
            this.PanelCombos.Name = "PanelCombos";
            this.PanelCombos.Size = new System.Drawing.Size(626, 54);
            this.PanelCombos.TabIndex = 2;
            // 
            // Charter
            // 
            chartArea1.Name = "ChartArea1";
            this.Charter.ChartAreas.Add(chartArea1);
            this.Charter.Cursor = System.Windows.Forms.Cursors.Default;
            this.Charter.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.Charter.Legends.Add(legend1);
            this.Charter.Location = new System.Drawing.Point(0, 0);
            this.Charter.Name = "Charter";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.Charter.Series.Add(series1);
            this.Charter.Size = new System.Drawing.Size(626, 534);
            this.Charter.TabIndex = 3;
            this.Charter.Text = "chart1";
            // 
            // StatisticsChartsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 534);
            this.Controls.Add(this.Charter);
            this.Controls.Add(this.PanelCombos);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Name = "StatisticsChartsPanel";
            this.Text = "General Stats";
            ((System.ComponentModel.ISupportInitialize)(this.Charter)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelCombos;
        private System.Windows.Forms.DataVisualization.Charting.Chart Charter;
    }
}