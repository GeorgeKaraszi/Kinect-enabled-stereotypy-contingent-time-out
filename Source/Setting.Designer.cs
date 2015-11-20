namespace WesternMichgian.SeniorDesign.KinectProject
{
    partial class Setting
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
            this.timerRadioB = new System.Windows.Forms.RadioButton();
            this.windowRadioB = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.timerTb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timerRadioB
            // 
            this.timerRadioB.AutoSize = true;
            this.timerRadioB.Location = new System.Drawing.Point(65, 53);
            this.timerRadioB.Name = "timerRadioB";
            this.timerRadioB.Size = new System.Drawing.Size(83, 17);
            this.timerRadioB.TabIndex = 0;
            this.timerRadioB.TabStop = true;
            this.timerRadioB.Text = "Timer option";
            this.timerRadioB.UseVisualStyleBackColor = true;
            this.timerRadioB.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // windowRadioB
            // 
            this.windowRadioB.AutoSize = true;
            this.windowRadioB.Location = new System.Drawing.Point(64, 84);
            this.windowRadioB.Name = "windowRadioB";
            this.windowRadioB.Size = new System.Drawing.Size(98, 17);
            this.windowRadioB.TabIndex = 1;
            this.windowRadioB.TabStop = true;
            this.windowRadioB.Text = "Window Option";
            this.windowRadioB.UseVisualStyleBackColor = true;
            this.windowRadioB.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 24);
            this.label1.TabIndex = 3;
            this.label1.Text = "Timer";
            // 
            // timerTb
            // 
            this.timerTb.Location = new System.Drawing.Point(100, 14);
            this.timerTb.Name = "timerTb";
            this.timerTb.Size = new System.Drawing.Size(38, 20);
            this.timerTb.TabIndex = 4;
            this.timerTb.Text = "1";
            this.timerTb.TextChanged += new System.EventHandler(this.timerTb_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(144, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Minutes";
            // 
            // Setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 127);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.timerTb);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.windowRadioB);
            this.Controls.Add(this.timerRadioB);
            this.Name = "Setting";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton timerRadioB;
        private System.Windows.Forms.RadioButton windowRadioB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox timerTb;
        private System.Windows.Forms.Label label2;
    }
}