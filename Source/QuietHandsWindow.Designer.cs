﻿using System.ComponentModel;
using System.Windows.Forms;

namespace WesternMichgian.SeniorDesign.KinectProject
{
    partial class QuietHandsWindow
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timer_lbl = new System.Windows.Forms.Label();
            this.qhands_lbl = new System.Windows.Forms.Label();
            this.qhand_timer = new System.Windows.Forms.Timer(this.components);
            this.quitTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.BackColor = System.Drawing.SystemColors.Window;
            this.progressBar1.ForeColor = System.Drawing.Color.Transparent;
            this.progressBar1.Location = new System.Drawing.Point(308, 370);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(290, 29);
            this.progressBar1.TabIndex = 5;
            // 
            // timer_lbl
            // 
            this.timer_lbl.AutoSize = true;
            this.timer_lbl.BackColor = System.Drawing.Color.Transparent;
            this.timer_lbl.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.timer_lbl.Location = new System.Drawing.Point(380, 284);
            this.timer_lbl.Name = "timer_lbl";
            this.timer_lbl.Size = new System.Drawing.Size(73, 13);
            this.timer_lbl.TabIndex = 4;
            this.timer_lbl.Text = "0 seconds left";
            // 
            // qhands_lbl
            // 
            this.qhands_lbl.AutoSize = true;
            this.qhands_lbl.BackColor = System.Drawing.Color.Transparent;
            this.qhands_lbl.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.qhands_lbl.Location = new System.Drawing.Point(380, 237);
            this.qhands_lbl.Name = "qhands_lbl";
            this.qhands_lbl.Size = new System.Drawing.Size(66, 13);
            this.qhands_lbl.TabIndex = 3;
            this.qhands_lbl.Text = "Quiet Hands";
            this.qhands_lbl.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
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
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.BackgroundImage = global::WesternMichgian.SeniorDesign.KinectProject.Properties.Resources.pic;
            this.ClientSize = new System.Drawing.Size(935, 527);
            this.ControlBox = false;
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.timer_lbl);
            this.Controls.Add(this.qhands_lbl);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ForeColor = System.Drawing.Color.Transparent;
            this.Name = "QuietHandsWindow";
            this.Text = "QuietHandsWindow";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.onClose);
            this.Load += new System.EventHandler(this.QuietHandsWindow_Load);
            this.VisibleChanged += new System.EventHandler(this.QuietHandsWindow_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label timer_lbl;
        private System.Windows.Forms.Label qhands_lbl;
        private System.Windows.Forms.Timer qhand_timer;
        private System.Windows.Forms.Timer quitTimer;
    
}
}