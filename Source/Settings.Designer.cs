namespace WesternMichgian.SeniorDesign.KinectProject
{
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.sucessTimeChk = new System.Windows.Forms.CheckBox();
            this.fullscreen_ckbox = new System.Windows.Forms.CheckBox();
            this.successtimer_number = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fullscren_timernumber = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.successtimer_number)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fullscren_timernumber)).BeginInit();
            this.SuspendLayout();
            // 
            // sucessTimeChk
            // 
            this.sucessTimeChk.AutoSize = true;
            this.sucessTimeChk.Location = new System.Drawing.Point(26, 23);
            this.sucessTimeChk.Name = "sucessTimeChk";
            this.sucessTimeChk.Size = new System.Drawing.Size(96, 17);
            this.sucessTimeChk.TabIndex = 0;
            this.sucessTimeChk.Text = "Success Timer";
            this.sucessTimeChk.UseVisualStyleBackColor = true;
            this.sucessTimeChk.CheckedChanged += new System.EventHandler(this.sucessTimeChk_CheckedChanged);
            // 
            // fullscreen_ckbox
            // 
            this.fullscreen_ckbox.AutoSize = true;
            this.fullscreen_ckbox.Location = new System.Drawing.Point(26, 84);
            this.fullscreen_ckbox.Name = "fullscreen_ckbox";
            this.fullscreen_ckbox.Size = new System.Drawing.Size(121, 17);
            this.fullscreen_ckbox.TabIndex = 1;
            this.fullscreen_ckbox.Text = "Full Screen Window";
            this.fullscreen_ckbox.UseVisualStyleBackColor = true;
            this.fullscreen_ckbox.CheckedChanged += new System.EventHandler(this.fullscreen_ckbox_CheckedChanged);
            // 
            // successtimer_number
            // 
            this.successtimer_number.Enabled = false;
            this.successtimer_number.Location = new System.Drawing.Point(92, 46);
            this.successtimer_number.Name = "successtimer_number";
            this.successtimer_number.Size = new System.Drawing.Size(96, 20);
            this.successtimer_number.TabIndex = 2;
            this.successtimer_number.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.successtimer_number.ValueChanged += new System.EventHandler(this.successtimer_number_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(194, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Secounds";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Secounds";
            // 
            // fullscren_timernumber
            // 
            this.fullscren_timernumber.Enabled = false;
            this.fullscren_timernumber.Location = new System.Drawing.Point(92, 114);
            this.fullscren_timernumber.Name = "fullscren_timernumber";
            this.fullscren_timernumber.Size = new System.Drawing.Size(96, 20);
            this.fullscren_timernumber.TabIndex = 4;
            this.fullscren_timernumber.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.fullscren_timernumber.ValueChanged += new System.EventHandler(this.fullscren_timernumber_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Timeout Timer";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Time Interval";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 183);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fullscren_timernumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.successtimer_number);
            this.Controls.Add(this.fullscreen_ckbox);
            this.Controls.Add(this.sucessTimeChk);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Settings";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.successtimer_number)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fullscren_timernumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox sucessTimeChk;
        private System.Windows.Forms.CheckBox fullscreen_ckbox;
        private System.Windows.Forms.NumericUpDown successtimer_number;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown fullscren_timernumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}