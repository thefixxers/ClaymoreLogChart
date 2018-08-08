using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

using ClaymoreLogChart.Parsers;
using ClaymoreLogChart.VisualStylers;

namespace ClaymoreLogChart.Dialogs
{
    class LoadLogFileDialog: Form
    {
        EventHandler beforeWorkStart, afterWorkFinish;
        BackgroundLogParser parser;
        private Label LabelFileName;
        private Panel PanelProgress;
        private RichTextBox RichTextInfo;
        private Button ButtonCancel;
        string logFilePath;
        int progress = 0;

        public LoadLogFileDialog(BackgroundLogParser logParser, EventHandler beforeWork, EventHandler afterWork, string filePath)
        {
            InitializeComponent();

            logFilePath = filePath;
            LabelFileName.Text = "Parsing " + System.IO.Path.GetFileName(filePath);

            ButtonCancel.BackColor = this.BackColor = ThemeColors.WindowContentBackgroundDarkTheme;
            ButtonCancel.ForeColor = this.ForeColor = ThemeColors.WindowContentForegroundDarkTheme;
            RichTextInfo.BackColor = ThemeColors.WindowSubTitleBackgroundDarkTheme;
            
            this.Paint += (s, ea) => { ea.Graphics.DrawRectangle(Pens.Gray, 0, 0, Width - 1, Height - 1); };
            PanelProgress.Paint += PanelProgress_Paint;

            parser = logParser;

            beforeWorkStart = beforeWork;
            afterWorkFinish = afterWork;

            parser.ProgressChanged += HandleProgressChange ;
            parser.RunWorkerCompleted += HandleParserDone;

            this.Load += HandleDialogLoad;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void HandleDialogLoad(object sender, EventArgs e)
        {
            beforeWorkStart?.Invoke(this, e);
            parser.Start();
        }


        private void PanelProgress_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            Bitmap bmp = new Bitmap(PanelProgress.Width, PanelProgress.Height);
            Graphics bmpGr = Graphics.FromImage(bmp);

            using (Pen forePen=new Pen(ThemeColors.WindowSubTitleBackgroundDarkTheme))
            {
                bmpGr.DrawRectangle(forePen, 0, 0, PanelProgress.Width - 1, PanelProgress.Height - 1);
            }
            using (SolidBrush brush = new SolidBrush(ThemeColors.FocusedSelectedBackgroundColor))
            {
                bmpGr.FillRectangle(brush, 1, 1, progress * (PanelProgress.Width - 3)/100, PanelProgress.Height - 3);
            }

            e.Graphics.DrawImage(bmp, 0, 0);

            bmpGr.Dispose();
            bmp.Dispose();
        }

        private void HandleProgressChange(object sender, ProgressChangedEventArgs e)
        {
            progress = e.ProgressPercentage;
            if (e.UserState != null)
            {
                RichTextInfo.AppendText(e.UserState.ToString());
                RichTextInfo.ScrollToCaret();
            }
                
            PanelProgress.Invalidate();
            //Application.DoEvents();
            
        }

        private void HandleParserDone(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
            afterWorkFinish?.Invoke(sender, e);
            
        }
      
        
        private void InitializeComponent()
        {
            this.LabelFileName = new System.Windows.Forms.Label();
            this.PanelProgress = new System.Windows.Forms.Panel();
            this.RichTextInfo = new System.Windows.Forms.RichTextBox();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LabelFileName
            // 
            this.LabelFileName.AutoSize = true;
            this.LabelFileName.Location = new System.Drawing.Point(34, 32);
            this.LabelFileName.Name = "LabelFileName";
            this.LabelFileName.Size = new System.Drawing.Size(38, 13);
            this.LabelFileName.TabIndex = 0;
            this.LabelFileName.Text = "label1";
            // 
            // PanelProgress
            // 
            this.PanelProgress.Location = new System.Drawing.Point(37, 62);
            this.PanelProgress.Name = "PanelProgress";
            this.PanelProgress.Size = new System.Drawing.Size(517, 27);
            this.PanelProgress.TabIndex = 1;
            // 
            // RichTextInfo
            // 
            this.RichTextInfo.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.RichTextInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.RichTextInfo.Enabled = false;
            this.RichTextInfo.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RichTextInfo.Location = new System.Drawing.Point(37, 106);
            this.RichTextInfo.Name = "RichTextInfo";
            this.RichTextInfo.Size = new System.Drawing.Size(517, 106);
            this.RichTextInfo.TabIndex = 2;
            this.RichTextInfo.TabStop = false;
            this.RichTextInfo.Text = "";
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ButtonCancel.Location = new System.Drawing.Point(436, 233);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(118, 25);
            this.ButtonCancel.TabIndex = 28;
            this.ButtonCancel.Text = "CANCEL";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            // 
            // LoadLogFileDialog
            // 
            this.ClientSize = new System.Drawing.Size(601, 282);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.RichTextInfo);
            this.Controls.Add(this.PanelProgress);
            this.Controls.Add(this.LabelFileName);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoadLogFileDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
