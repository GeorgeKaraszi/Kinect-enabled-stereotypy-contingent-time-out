namespace CaptureUtil
{
    partial class MainForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series9 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.btnRecord = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.cbGestureTarget = new System.Windows.Forms.ComboBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSave = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbCapDis = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chartRecording = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartKinectRaw = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.udRecTime = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.timerRecord = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.rbPV = new System.Windows.Forms.RadioButton();
            this.rbHillBuild = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRecording)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartKinectRaw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRecTime)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnRecord
            // 
            this.btnRecord.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnRecord.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnRecord.Location = new System.Drawing.Point(244, 72);
            this.btnRecord.Name = "btnRecord";
            this.btnRecord.Size = new System.Drawing.Size(264, 23);
            this.btnRecord.TabIndex = 2;
            this.btnRecord.Text = "Start Recording....";
            this.btnRecord.UseVisualStyleBackColor = false;
            this.btnRecord.Click += new System.EventHandler(this.btnRecord_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbGestureTarget);
            this.panel1.Location = new System.Drawing.Point(0, 527);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(748, 43);
            this.panel1.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(266, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 16);
            this.label3.TabIndex = 7;
            this.label3.Text = "Target Gesture";
            // 
            // cbGestureTarget
            // 
            this.cbGestureTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.cbGestureTarget.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGestureTarget.FormattingEnabled = true;
            this.cbGestureTarget.Location = new System.Drawing.Point(383, 12);
            this.cbGestureTarget.Name = "cbGestureTarget";
            this.cbGestureTarget.Size = new System.Drawing.Size(121, 21);
            this.cbGestureTarget.TabIndex = 5;
            this.cbGestureTarget.SelectedIndexChanged += new System.EventHandler(this.cbGestureTarget_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(748, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOpen,
            this.toolStripSave});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripOpen
            // 
            this.toolStripOpen.Name = "toolStripOpen";
            this.toolStripOpen.Size = new System.Drawing.Size(103, 22);
            this.toolStripOpen.Text = "Open";
            this.toolStripOpen.Click += new System.EventHandler(this.toolStripOpen_Click);
            // 
            // toolStripSave
            // 
            this.toolStripSave.Name = "toolStripSave";
            this.toolStripSave.Size = new System.Drawing.Size(103, 22);
            this.toolStripSave.Text = "Save";
            this.toolStripSave.Click += new System.EventHandler(this.toolStripSave_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.lbCapDis);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.chartRecording);
            this.panel2.Controls.Add(this.chartKinectRaw);
            this.panel2.Location = new System.Drawing.Point(0, 120);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(748, 401);
            this.panel2.TabIndex = 4;
            // 
            // lbCapDis
            // 
            this.lbCapDis.AutoSize = true;
            this.lbCapDis.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbCapDis.Location = new System.Drawing.Point(3, 8);
            this.lbCapDis.Name = "lbCapDis";
            this.lbCapDis.Size = new System.Drawing.Size(97, 17);
            this.lbCapDis.TabIndex = 6;
            this.lbCapDis.Text = "Captured Data";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(3, 210);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Kinect Raw Stream";
            // 
            // chartRecording
            // 
            this.chartRecording.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartRecording.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            chartArea5.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea5.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea5.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea5.AxisY.Maximum = 1D;
            chartArea5.AxisY.Minimum = 0D;
            chartArea5.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            chartArea5.Name = "ChartArea1";
            this.chartRecording.ChartAreas.Add(chartArea5);
            this.chartRecording.Location = new System.Drawing.Point(-29, 28);
            this.chartRecording.Name = "chartRecording";
            this.chartRecording.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
            series7.ChartArea = "ChartArea1";
            series7.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series7.Color = System.Drawing.Color.Black;
            series7.Name = "Series1";
            series8.ChartArea = "ChartArea1";
            series8.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series8.Color = System.Drawing.Color.OrangeRed;
            series8.IsValueShownAsLabel = true;
            series8.Label = "#VAL";
            series8.Name = "Series2";
            this.chartRecording.Series.Add(series7);
            this.chartRecording.Series.Add(series8);
            this.chartRecording.Size = new System.Drawing.Size(816, 171);
            this.chartRecording.TabIndex = 3;
            this.chartRecording.Text = "chart1";
            // 
            // chartKinectRaw
            // 
            this.chartKinectRaw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chartKinectRaw.BackColor = System.Drawing.Color.Maroon;
            chartArea6.AxisX.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea6.AxisX2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea6.AxisY.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea6.AxisY.Maximum = 1D;
            chartArea6.AxisY.Minimum = 0D;
            chartArea6.AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.False;
            chartArea6.BackColor = System.Drawing.Color.Maroon;
            chartArea6.BorderColor = System.Drawing.Color.Maroon;
            chartArea6.Name = "ChartArea1";
            this.chartKinectRaw.ChartAreas.Add(chartArea6);
            this.chartKinectRaw.Location = new System.Drawing.Point(-29, 227);
            this.chartKinectRaw.Name = "chartKinectRaw";
            series9.ChartArea = "ChartArea1";
            series9.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series9.Color = System.Drawing.Color.White;
            series9.CustomProperties = "LineTension=0.5";
            series9.Name = "Series1";
            this.chartKinectRaw.Series.Add(series9);
            this.chartKinectRaw.Size = new System.Drawing.Size(816, 171);
            this.chartKinectRaw.TabIndex = 2;
            this.chartKinectRaw.Text = "chart1";
            // 
            // udRecTime
            // 
            this.udRecTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.udRecTime.Location = new System.Drawing.Point(415, 35);
            this.udRecTime.Name = "udRecTime";
            this.udRecTime.Size = new System.Drawing.Size(54, 20);
            this.udRecTime.TabIndex = 5;
            this.udRecTime.ValueChanged += new System.EventHandler(this.udRecTime_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(241, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Recording Length (0 = Unlimited)";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "Compressed Database files|*.cdb|All files|*.*";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Compressed Database files|*.cdb|All files|*.*";
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // timerRecord
            // 
            this.timerRecord.Tick += new System.EventHandler(this.timerRecord_Tick);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(475, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Sec\'s";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rbHillBuild);
            this.panel3.Controls.Add(this.rbPV);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(231, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(517, 28);
            this.panel3.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(10, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(123, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "Algorithm Method";
            // 
            // rbPV
            // 
            this.rbPV.AutoSize = true;
            this.rbPV.Location = new System.Drawing.Point(152, 6);
            this.rbPV.Name = "rbPV";
            this.rbPV.Size = new System.Drawing.Size(75, 17);
            this.rbPV.TabIndex = 8;
            this.rbPV.TabStop = true;
            this.rbPV.Text = "PV  Period";
            this.rbPV.UseVisualStyleBackColor = true;
            this.rbPV.CheckedChanged += new System.EventHandler(this.Algorithm_CheckedChanged);
            // 
            // rbHillBuild
            // 
            this.rbHillBuild.AutoSize = true;
            this.rbHillBuild.Location = new System.Drawing.Point(247, 6);
            this.rbHillBuild.Name = "rbHillBuild";
            this.rbHillBuild.Size = new System.Drawing.Size(79, 17);
            this.rbHillBuild.TabIndex = 9;
            this.rbHillBuild.TabStop = true;
            this.rbHillBuild.Text = "Hill Building";
            this.rbHillBuild.UseVisualStyleBackColor = true;
            this.rbHillBuild.CheckedChanged += new System.EventHandler(this.Algorithm_CheckedChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(748, 570);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.udRecTime);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnRecord);
            this.Controls.Add(this.menuStrip1);
            this.Name = "MainForm";
            this.Text = "Capture Util";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartRecording)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartKinectRaw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udRecTime)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRecord;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbGestureTarget;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripOpen;
        private System.Windows.Forms.ToolStripMenuItem toolStripSave;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbCapDis;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRecording;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartKinectRaw;
        private System.Windows.Forms.NumericUpDown udRecTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Timer timerRecord;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rbHillBuild;
        private System.Windows.Forms.RadioButton rbPV;
        private System.Windows.Forms.Label label5;
    }
}

