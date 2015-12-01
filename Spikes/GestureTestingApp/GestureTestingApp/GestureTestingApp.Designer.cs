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
            this.SuspendLayout();
            // 
            // Open
            // 
            this.Open.Location = new System.Drawing.Point(12, 12);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(199, 101);
            this.Open.TabIndex = 0;
            this.Open.Text = "Open clip";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.OpenClip);
            // 
            // GestureTestingApp
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.Open);
            this.Name = "GestureTestingApp";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button play;
        private System.Windows.Forms.Button Open;
    }
}

