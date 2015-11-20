using System.ComponentModel;
using System.Windows.Forms;

namespace WesternMichgian.SeniorDesign.KinectProject
{
    partial class QuietHandsWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer_lbl = new System.Windows.Forms.Label();
            this.qhands_lbl = new System.Windows.Forms.Label();
            this.qhand_timer = new System.Windows.Forms.Timer(this.components);
            this.quitTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.progressBar1.Location = new System.Drawing.Point(255, 292);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(350, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // timer_lbl
            // 
            this.timer_lbl.AutoSize = true;
            this.timer_lbl.Location = new System.Drawing.Point(410, 249);
            this.timer_lbl.Name = "timer_lbl";
            this.timer_lbl.Size = new System.Drawing.Size(79, 13);
            this.timer_lbl.TabIndex = 4;
            this.timer_lbl.Text = "0 secounds left";
            // 
            // qhands_lbl
            // 
            this.qhands_lbl.AutoSize = true;
            this.qhands_lbl.Location = new System.Drawing.Point(392, 211);
            this.qhands_lbl.Name = "qhands_lbl";
            this.qhands_lbl.Size = new System.Drawing.Size(66, 13);
            this.qhands_lbl.TabIndex = 3;
            this.qhands_lbl.Text = "Quiet Hands";
            this.qhands_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // qhand_timer
            // 
            this.qhand_timer.Tick += new System.EventHandler(this.qhand_timer_Tick);
            // 
            // quitTimer
            // 
            this.quitTimer.Tick += new System.EventHandler(this.quitTimer_Tick);
            // 
            // QuietHandsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(861, 527);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.timer_lbl);
            this.Controls.Add(this.qhands_lbl);
            this.Name = "QuietHandsWindow";
            this.Text = "QuietHandsWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.onClose);
            this.Load += new System.EventHandler(this.QuietHandsWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ProgressBar progressBar1;
        private Label timer_lbl;
        private Label qhands_lbl;
        private Timer qhand_timer;
        private Timer quitTimer;
    }
}