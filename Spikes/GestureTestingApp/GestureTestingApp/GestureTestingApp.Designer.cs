namespace GestureTestingApp
{
    partial class GestureTestingApp
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
            this.Open = new System.Windows.Forms.Button();
            this.FilenameLabel = new System.Windows.Forms.Label();
            this.DurationLabel = new System.Windows.Forms.Label();
            this.ProcessingLabel = new System.Windows.Forms.Label();
            this.FilenameValue = new System.Windows.Forms.Label();
            this.DurationValue = new System.Windows.Forms.Label();
            this.ProcessingValue = new System.Windows.Forms.Label();
            this.DurationNote = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Open
            // 
            this.Open.Location = new System.Drawing.Point(12, 12);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(343, 68);
            this.Open.TabIndex = 0;
            this.Open.Text = "Play clip";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.PlayClip);
            // 
            // FilenameLabel
            // 
            this.FilenameLabel.AutoSize = true;
            this.FilenameLabel.Location = new System.Drawing.Point(12, 98);
            this.FilenameLabel.Name = "FilenameLabel";
            this.FilenameLabel.Size = new System.Drawing.Size(49, 13);
            this.FilenameLabel.TabIndex = 1;
            this.FilenameLabel.Text = "Filename";
            // 
            // DurationLabel
            // 
            this.DurationLabel.AutoSize = true;
            this.DurationLabel.Location = new System.Drawing.Point(12, 122);
            this.DurationLabel.Name = "DurationLabel";
            this.DurationLabel.Size = new System.Drawing.Size(47, 13);
            this.DurationLabel.TabIndex = 2;
            this.DurationLabel.Text = "Duration";
            // 
            // ProcessingLabel
            // 
            this.ProcessingLabel.AutoSize = true;
            this.ProcessingLabel.Location = new System.Drawing.Point(12, 145);
            this.ProcessingLabel.Name = "ProcessingLabel";
            this.ProcessingLabel.Size = new System.Drawing.Size(59, 13);
            this.ProcessingLabel.TabIndex = 3;
            this.ProcessingLabel.Text = "Processing";
            // 
            // FilenameValue
            // 
            this.FilenameValue.AutoSize = true;
            this.FilenameValue.Location = new System.Drawing.Point(85, 98);
            this.FilenameValue.Name = "FilenameValue";
            this.FilenameValue.Size = new System.Drawing.Size(16, 13);
            this.FilenameValue.TabIndex = 4;
            this.FilenameValue.Text = "...";
            // 
            // DurationValue
            // 
            this.DurationValue.AutoSize = true;
            this.DurationValue.Location = new System.Drawing.Point(85, 122);
            this.DurationValue.Name = "DurationValue";
            this.DurationValue.Size = new System.Drawing.Size(16, 13);
            this.DurationValue.TabIndex = 5;
            this.DurationValue.Text = "...";
            // 
            // ProcessingValue
            // 
            this.ProcessingValue.AutoSize = true;
            this.ProcessingValue.Location = new System.Drawing.Point(85, 145);
            this.ProcessingValue.Name = "ProcessingValue";
            this.ProcessingValue.Size = new System.Drawing.Size(16, 13);
            this.ProcessingValue.TabIndex = 6;
            this.ProcessingValue.Text = "...";
            // 
            // DurationNote
            // 
            this.DurationNote.AutoSize = true;
            this.DurationNote.Location = new System.Drawing.Point(12, 175);
            this.DurationNote.Name = "DurationNote";
            this.DurationNote.Size = new System.Drawing.Size(344, 13);
            this.DurationNote.TabIndex = 10;
            this.DurationNote.Text = "Note: Processing time is double the duration due to skeletal tagging lag.";
            // 
            // GestureTestingApp
            // 
            this.ClientSize = new System.Drawing.Size(367, 200);
            this.Controls.Add(this.DurationNote);
            this.Controls.Add(this.ProcessingValue);
            this.Controls.Add(this.DurationValue);
            this.Controls.Add(this.FilenameValue);
            this.Controls.Add(this.ProcessingLabel);
            this.Controls.Add(this.DurationLabel);
            this.Controls.Add(this.FilenameLabel);
            this.Controls.Add(this.Open);
            this.Name = "GestureTestingApp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindowClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Open;
        private System.Windows.Forms.Label FilenameLabel;
        private System.Windows.Forms.Label DurationLabel;
        private System.Windows.Forms.Label ProcessingLabel;
        private System.Windows.Forms.Label FilenameValue;
        private System.Windows.Forms.Label DurationValue;
        private System.Windows.Forms.Label ProcessingValue;
        private System.Windows.Forms.Label DurationNote;
    }
}

