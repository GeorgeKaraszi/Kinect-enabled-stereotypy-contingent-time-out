namespace CaptureReportTool
{
    partial class CRTForm
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
            this.activeTracking = new System.Windows.Forms.NumericUpDown();
            this.trackBtn = new System.Windows.Forms.Button();
            this.openWndBtn = new System.Windows.Forms.Button();
            this.rmv_topbtn = new System.Windows.Forms.Button();
            this.rmv_midbtn = new System.Windows.Forms.Button();
            this.rmv_btmbtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gestureLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.activeTracking)).BeginInit();
            this.SuspendLayout();
            // 
            // activeTracking
            // 
            this.activeTracking.Location = new System.Drawing.Point(62, 69);
            this.activeTracking.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.activeTracking.Name = "activeTracking";
            this.activeTracking.Size = new System.Drawing.Size(105, 20);
            this.activeTracking.TabIndex = 0;
            this.activeTracking.ValueChanged += new System.EventHandler(this.activeTracking_ValueChanged);
            // 
            // trackBtn
            // 
            this.trackBtn.Location = new System.Drawing.Point(74, 95);
            this.trackBtn.Name = "trackBtn";
            this.trackBtn.Size = new System.Drawing.Size(75, 23);
            this.trackBtn.TabIndex = 1;
            this.trackBtn.Text = "Track";
            this.trackBtn.UseVisualStyleBackColor = true;
            this.trackBtn.Click += new System.EventHandler(this.trackBtn_Click);
            // 
            // openWndBtn
            // 
            this.openWndBtn.Location = new System.Drawing.Point(62, 13);
            this.openWndBtn.Name = "openWndBtn";
            this.openWndBtn.Size = new System.Drawing.Size(109, 23);
            this.openWndBtn.TabIndex = 2;
            this.openWndBtn.Text = "Open Tool Window";
            this.openWndBtn.UseVisualStyleBackColor = true;
            this.openWndBtn.Click += new System.EventHandler(this.openWndBtn_Click);
            // 
            // rmv_topbtn
            // 
            this.rmv_topbtn.Location = new System.Drawing.Point(55, 158);
            this.rmv_topbtn.Name = "rmv_topbtn";
            this.rmv_topbtn.Size = new System.Drawing.Size(112, 23);
            this.rmv_topbtn.TabIndex = 3;
            this.rmv_topbtn.Text = "Remove Top";
            this.rmv_topbtn.UseVisualStyleBackColor = true;
            this.rmv_topbtn.Click += new System.EventHandler(this.rmv_topbtn_Click);
            // 
            // rmv_midbtn
            // 
            this.rmv_midbtn.Location = new System.Drawing.Point(55, 187);
            this.rmv_midbtn.Name = "rmv_midbtn";
            this.rmv_midbtn.Size = new System.Drawing.Size(112, 23);
            this.rmv_midbtn.TabIndex = 4;
            this.rmv_midbtn.Text = "Remove Middle";
            this.rmv_midbtn.UseVisualStyleBackColor = true;
            this.rmv_midbtn.Click += new System.EventHandler(this.rmv_midbtn_Click);
            // 
            // rmv_btmbtn
            // 
            this.rmv_btmbtn.Location = new System.Drawing.Point(55, 216);
            this.rmv_btmbtn.Name = "rmv_btmbtn";
            this.rmv_btmbtn.Size = new System.Drawing.Size(112, 23);
            this.rmv_btmbtn.TabIndex = 5;
            this.rmv_btmbtn.Text = "Remove Bottom";
            this.rmv_btmbtn.UseVisualStyleBackColor = true;
            this.rmv_btmbtn.Click += new System.EventHandler(this.rmv_btmbtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(178, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Target Gesture Event:";
            // 
            // gestureLbl
            // 
            this.gestureLbl.AutoSize = true;
            this.gestureLbl.Location = new System.Drawing.Point(297, 105);
            this.gestureLbl.Name = "gestureLbl";
            this.gestureLbl.Size = new System.Drawing.Size(102, 13);
            this.gestureLbl.TabIndex = 7;
            this.gestureLbl.Text = "(#id):(GestureName)";
            // 
            // CRTForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 266);
            this.Controls.Add(this.gestureLbl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rmv_btmbtn);
            this.Controls.Add(this.rmv_midbtn);
            this.Controls.Add(this.rmv_topbtn);
            this.Controls.Add(this.openWndBtn);
            this.Controls.Add(this.trackBtn);
            this.Controls.Add(this.activeTracking);
            this.Name = "CRTForm";
            this.Text = "Main Window";
            ((System.ComponentModel.ISupportInitialize)(this.activeTracking)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown activeTracking;
        private System.Windows.Forms.Button trackBtn;
        private System.Windows.Forms.Button openWndBtn;
        private System.Windows.Forms.Button rmv_topbtn;
        private System.Windows.Forms.Button rmv_midbtn;
        private System.Windows.Forms.Button rmv_btmbtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label gestureLbl;
    }
}

