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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GestureTestingApp));
            this.Open = new System.Windows.Forms.Button();
            this.FormerFilenameLabel = new System.Windows.Forms.Label();
            this.FormerDurationLabel = new System.Windows.Forms.Label();
            this.FormerFilenameValue = new System.Windows.Forms.Label();
            this.FormerDurationValue = new System.Windows.Forms.Label();
            this.DurationNote = new System.Windows.Forms.Label();
            this.FormerHandflappingLabel = new System.Windows.Forms.Label();
            this.FormerHandflappingValue = new System.Windows.Forms.Label();
            this.InformationLabel = new System.Windows.Forms.Label();
            this.HandflappingPositiveButton = new System.Windows.Forms.Button();
            this.HandflappingNegativeButton = new System.Windows.Forms.Button();
            this.PositiveDirectory = new System.Windows.Forms.Label();
            this.NegativeDirectory = new System.Windows.Forms.Label();
            this.NumberOfClipsLabel = new System.Windows.Forms.Label();
            this.NumberOfClipsValue = new System.Windows.Forms.Label();
            this.TotalDurationLabel = new System.Windows.Forms.Label();
            this.EstimatedRuntimeLabel = new System.Windows.Forms.Label();
            this.TotalDurationValue = new System.Windows.Forms.Label();
            this.EstimatedRuntimeValue = new System.Windows.Forms.Label();
            this.RunTestsButton = new System.Windows.Forms.Button();
            this.InProcessLabel = new System.Windows.Forms.Label();
            this.CurrentDurationLabel = new System.Windows.Forms.Label();
            this.LastProcessedLabel = new System.Windows.Forms.Label();
            this.PreviousFilenameLabel = new System.Windows.Forms.Label();
            this.PreviousDurationLabel = new System.Windows.Forms.Label();
            this.HandflappingLabel = new System.Windows.Forms.Label();
            this.InProcessValue = new System.Windows.Forms.Label();
            this.CurrentDurationValue = new System.Windows.Forms.Label();
            this.PreviousFilenameValue = new System.Windows.Forms.Label();
            this.PreviousDurationValue = new System.Windows.Forms.Label();
            this.HandflappingValue = new System.Windows.Forms.Label();
            this.ResultsLabel = new System.Windows.Forms.Label();
            this.TotalTestsLabel = new System.Windows.Forms.Label();
            this.FaultyTestsLabel = new System.Windows.Forms.Label();
            this.SuccessfulTestsLabel = new System.Windows.Forms.Label();
            this.FalsePositivesLabel = new System.Windows.Forms.Label();
            this.FalseNegativesLabel = new System.Windows.Forms.Label();
            this.TotalTestsValue = new System.Windows.Forms.Label();
            this.FaultyTestsValue = new System.Windows.Forms.Label();
            this.SuccessfulTestsValue = new System.Windows.Forms.Label();
            this.FalsePositivesValue = new System.Windows.Forms.Label();
            this.FalseNegativesValue = new System.Windows.Forms.Label();
            this.CurrentFilenameLabel = new System.Windows.Forms.Label();
            this.CurrentFilenameValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Open
            // 
            this.Open.Location = new System.Drawing.Point(12, 533);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(349, 30);
            this.Open.TabIndex = 0;
            this.Open.Text = "Play clip";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.PlayClip);
            // 
            // FormerFilenameLabel
            // 
            this.FormerFilenameLabel.AutoSize = true;
            this.FormerFilenameLabel.Location = new System.Drawing.Point(15, 579);
            this.FormerFilenameLabel.Name = "FormerFilenameLabel";
            this.FormerFilenameLabel.Size = new System.Drawing.Size(49, 13);
            this.FormerFilenameLabel.TabIndex = 1;
            this.FormerFilenameLabel.Text = "Filename";
            // 
            // FormerDurationLabel
            // 
            this.FormerDurationLabel.AutoSize = true;
            this.FormerDurationLabel.Location = new System.Drawing.Point(15, 601);
            this.FormerDurationLabel.Name = "FormerDurationLabel";
            this.FormerDurationLabel.Size = new System.Drawing.Size(47, 13);
            this.FormerDurationLabel.TabIndex = 2;
            this.FormerDurationLabel.Text = "Duration";
            // 
            // FormerFilenameValue
            // 
            this.FormerFilenameValue.AutoSize = true;
            this.FormerFilenameValue.Location = new System.Drawing.Point(106, 579);
            this.FormerFilenameValue.Name = "FormerFilenameValue";
            this.FormerFilenameValue.Size = new System.Drawing.Size(16, 13);
            this.FormerFilenameValue.TabIndex = 4;
            this.FormerFilenameValue.Text = "...";
            // 
            // FormerDurationValue
            // 
            this.FormerDurationValue.AutoSize = true;
            this.FormerDurationValue.Location = new System.Drawing.Point(106, 601);
            this.FormerDurationValue.Name = "FormerDurationValue";
            this.FormerDurationValue.Size = new System.Drawing.Size(16, 13);
            this.FormerDurationValue.TabIndex = 5;
            this.FormerDurationValue.Text = "...";
            // 
            // DurationNote
            // 
            this.DurationNote.AutoSize = true;
            this.DurationNote.Location = new System.Drawing.Point(15, 647);
            this.DurationNote.Name = "DurationNote";
            this.DurationNote.Size = new System.Drawing.Size(344, 13);
            this.DurationNote.TabIndex = 10;
            this.DurationNote.Text = "Note: Processing time is double the duration due to skeletal tagging lag.";
            // 
            // FormerHandflappingLabel
            // 
            this.FormerHandflappingLabel.AutoSize = true;
            this.FormerHandflappingLabel.Location = new System.Drawing.Point(15, 623);
            this.FormerHandflappingLabel.Name = "FormerHandflappingLabel";
            this.FormerHandflappingLabel.Size = new System.Drawing.Size(76, 13);
            this.FormerHandflappingLabel.TabIndex = 11;
            this.FormerHandflappingLabel.Text = "Handflapping?";
            // 
            // FormerHandflappingValue
            // 
            this.FormerHandflappingValue.AutoSize = true;
            this.FormerHandflappingValue.Location = new System.Drawing.Point(106, 623);
            this.FormerHandflappingValue.Name = "FormerHandflappingValue";
            this.FormerHandflappingValue.Size = new System.Drawing.Size(16, 13);
            this.FormerHandflappingValue.TabIndex = 12;
            this.FormerHandflappingValue.Text = "...";
            // 
            // InformationLabel
            // 
            this.InformationLabel.AutoSize = true;
            this.InformationLabel.Location = new System.Drawing.Point(15, 13);
            this.InformationLabel.MaximumSize = new System.Drawing.Size(350, 0);
            this.InformationLabel.Name = "InformationLabel";
            this.InformationLabel.Size = new System.Drawing.Size(340, 39);
            this.InformationLabel.TabIndex = 13;
            this.InformationLabel.Text = resources.GetString("InformationLabel.Text");
            // 
            // HandflappingPositiveButton
            // 
            this.HandflappingPositiveButton.Location = new System.Drawing.Point(12, 64);
            this.HandflappingPositiveButton.Name = "HandflappingPositiveButton";
            this.HandflappingPositiveButton.Size = new System.Drawing.Size(132, 23);
            this.HandflappingPositiveButton.TabIndex = 14;
            this.HandflappingPositiveButton.Text = "Handflapping positive :";
            this.HandflappingPositiveButton.UseVisualStyleBackColor = true;
            this.HandflappingPositiveButton.Click += new System.EventHandler(this.ChoosePositiveDirectory);
            // 
            // HandflappingNegativeButton
            // 
            this.HandflappingNegativeButton.Location = new System.Drawing.Point(12, 94);
            this.HandflappingNegativeButton.Name = "HandflappingNegativeButton";
            this.HandflappingNegativeButton.Size = new System.Drawing.Size(132, 23);
            this.HandflappingNegativeButton.TabIndex = 15;
            this.HandflappingNegativeButton.Text = "Handflapping negative :";
            this.HandflappingNegativeButton.UseVisualStyleBackColor = true;
            this.HandflappingNegativeButton.Click += new System.EventHandler(this.ChooseNegativeDirectory);
            // 
            // PositiveDirectory
            // 
            this.PositiveDirectory.AutoSize = true;
            this.PositiveDirectory.Location = new System.Drawing.Point(153, 69);
            this.PositiveDirectory.Name = "PositiveDirectory";
            this.PositiveDirectory.Size = new System.Drawing.Size(16, 13);
            this.PositiveDirectory.TabIndex = 16;
            this.PositiveDirectory.Text = "...";
            // 
            // NegativeDirectory
            // 
            this.NegativeDirectory.AutoSize = true;
            this.NegativeDirectory.Location = new System.Drawing.Point(153, 99);
            this.NegativeDirectory.Name = "NegativeDirectory";
            this.NegativeDirectory.Size = new System.Drawing.Size(16, 13);
            this.NegativeDirectory.TabIndex = 17;
            this.NegativeDirectory.Text = "...";
            // 
            // NumberOfClipsLabel
            // 
            this.NumberOfClipsLabel.AutoSize = true;
            this.NumberOfClipsLabel.Location = new System.Drawing.Point(15, 129);
            this.NumberOfClipsLabel.Name = "NumberOfClipsLabel";
            this.NumberOfClipsLabel.Size = new System.Drawing.Size(103, 13);
            this.NumberOfClipsLabel.TabIndex = 18;
            this.NumberOfClipsLabel.Text = "Number of .xef files :";
            // 
            // NumberOfClipsValue
            // 
            this.NumberOfClipsValue.AutoSize = true;
            this.NumberOfClipsValue.Location = new System.Drawing.Point(124, 129);
            this.NumberOfClipsValue.Name = "NumberOfClipsValue";
            this.NumberOfClipsValue.Size = new System.Drawing.Size(13, 13);
            this.NumberOfClipsValue.TabIndex = 19;
            this.NumberOfClipsValue.Text = "0";
            // 
            // TotalDurationLabel
            // 
            this.TotalDurationLabel.AutoSize = true;
            this.TotalDurationLabel.Location = new System.Drawing.Point(15, 151);
            this.TotalDurationLabel.Name = "TotalDurationLabel";
            this.TotalDurationLabel.Size = new System.Drawing.Size(78, 13);
            this.TotalDurationLabel.TabIndex = 20;
            this.TotalDurationLabel.Text = "Total duration :";
            // 
            // EstimatedRuntimeLabel
            // 
            this.EstimatedRuntimeLabel.AutoSize = true;
            this.EstimatedRuntimeLabel.Location = new System.Drawing.Point(15, 173);
            this.EstimatedRuntimeLabel.Name = "EstimatedRuntimeLabel";
            this.EstimatedRuntimeLabel.Size = new System.Drawing.Size(96, 13);
            this.EstimatedRuntimeLabel.TabIndex = 21;
            this.EstimatedRuntimeLabel.Text = "Estimated runtime :";
            // 
            // TotalDurationValue
            // 
            this.TotalDurationValue.AutoSize = true;
            this.TotalDurationValue.Location = new System.Drawing.Point(124, 151);
            this.TotalDurationValue.Name = "TotalDurationValue";
            this.TotalDurationValue.Size = new System.Drawing.Size(28, 13);
            this.TotalDurationValue.TabIndex = 22;
            this.TotalDurationValue.Text = "0:00";
            // 
            // EstimatedRuntimeValue
            // 
            this.EstimatedRuntimeValue.AutoSize = true;
            this.EstimatedRuntimeValue.Location = new System.Drawing.Point(124, 173);
            this.EstimatedRuntimeValue.Name = "EstimatedRuntimeValue";
            this.EstimatedRuntimeValue.Size = new System.Drawing.Size(28, 13);
            this.EstimatedRuntimeValue.TabIndex = 23;
            this.EstimatedRuntimeValue.Text = "0:00";
            // 
            // RunTestsButton
            // 
            this.RunTestsButton.Location = new System.Drawing.Point(12, 202);
            this.RunTestsButton.Name = "RunTestsButton";
            this.RunTestsButton.Size = new System.Drawing.Size(349, 28);
            this.RunTestsButton.TabIndex = 24;
            this.RunTestsButton.Text = "Run Tests";
            this.RunTestsButton.UseVisualStyleBackColor = true;
            this.RunTestsButton.Click += new System.EventHandler(this.RunTests);
            // 
            // InProcessLabel
            // 
            this.InProcessLabel.AutoSize = true;
            this.InProcessLabel.Location = new System.Drawing.Point(15, 242);
            this.InProcessLabel.Name = "InProcessLabel";
            this.InProcessLabel.Size = new System.Drawing.Size(65, 13);
            this.InProcessLabel.TabIndex = 25;
            this.InProcessLabel.Text = "In process : ";
            // 
            // CurrentDurationLabel
            // 
            this.CurrentDurationLabel.AutoSize = true;
            this.CurrentDurationLabel.Location = new System.Drawing.Point(15, 286);
            this.CurrentDurationLabel.Name = "CurrentDurationLabel";
            this.CurrentDurationLabel.Size = new System.Drawing.Size(53, 13);
            this.CurrentDurationLabel.TabIndex = 26;
            this.CurrentDurationLabel.Text = "Duration :";
            // 
            // LastProcessedLabel
            // 
            this.LastProcessedLabel.AutoSize = true;
            this.LastProcessedLabel.Location = new System.Drawing.Point(143, 310);
            this.LastProcessedLabel.Name = "LastProcessedLabel";
            this.LastProcessedLabel.Size = new System.Drawing.Size(80, 13);
            this.LastProcessedLabel.TabIndex = 27;
            this.LastProcessedLabel.Text = "Last Processed";
            // 
            // PreviousFilenameLabel
            // 
            this.PreviousFilenameLabel.AutoSize = true;
            this.PreviousFilenameLabel.Location = new System.Drawing.Point(15, 333);
            this.PreviousFilenameLabel.Name = "PreviousFilenameLabel";
            this.PreviousFilenameLabel.Size = new System.Drawing.Size(55, 13);
            this.PreviousFilenameLabel.TabIndex = 28;
            this.PreviousFilenameLabel.Text = "Filename :";
            // 
            // PreviousDurationLabel
            // 
            this.PreviousDurationLabel.AutoSize = true;
            this.PreviousDurationLabel.Location = new System.Drawing.Point(15, 355);
            this.PreviousDurationLabel.Name = "PreviousDurationLabel";
            this.PreviousDurationLabel.Size = new System.Drawing.Size(53, 13);
            this.PreviousDurationLabel.TabIndex = 29;
            this.PreviousDurationLabel.Text = "Duration :";
            // 
            // HandflappingLabel
            // 
            this.HandflappingLabel.AutoSize = true;
            this.HandflappingLabel.Location = new System.Drawing.Point(15, 377);
            this.HandflappingLabel.Name = "HandflappingLabel";
            this.HandflappingLabel.Size = new System.Drawing.Size(76, 13);
            this.HandflappingLabel.TabIndex = 30;
            this.HandflappingLabel.Text = "Handflapping?";
            // 
            // InProcessValue
            // 
            this.InProcessValue.AutoSize = true;
            this.InProcessValue.Location = new System.Drawing.Point(112, 242);
            this.InProcessValue.Name = "InProcessValue";
            this.InProcessValue.Size = new System.Drawing.Size(16, 13);
            this.InProcessValue.TabIndex = 31;
            this.InProcessValue.Text = "...";
            // 
            // CurrentDurationValue
            // 
            this.CurrentDurationValue.AutoSize = true;
            this.CurrentDurationValue.Location = new System.Drawing.Point(112, 286);
            this.CurrentDurationValue.Name = "CurrentDurationValue";
            this.CurrentDurationValue.Size = new System.Drawing.Size(16, 13);
            this.CurrentDurationValue.TabIndex = 32;
            this.CurrentDurationValue.Text = "...";
            // 
            // PreviousFilenameValue
            // 
            this.PreviousFilenameValue.AutoSize = true;
            this.PreviousFilenameValue.Location = new System.Drawing.Point(112, 333);
            this.PreviousFilenameValue.Name = "PreviousFilenameValue";
            this.PreviousFilenameValue.Size = new System.Drawing.Size(16, 13);
            this.PreviousFilenameValue.TabIndex = 33;
            this.PreviousFilenameValue.Text = "...";
            // 
            // PreviousDurationValue
            // 
            this.PreviousDurationValue.AutoSize = true;
            this.PreviousDurationValue.Location = new System.Drawing.Point(112, 355);
            this.PreviousDurationValue.Name = "PreviousDurationValue";
            this.PreviousDurationValue.Size = new System.Drawing.Size(16, 13);
            this.PreviousDurationValue.TabIndex = 34;
            this.PreviousDurationValue.Text = "...";
            // 
            // HandflappingValue
            // 
            this.HandflappingValue.AutoSize = true;
            this.HandflappingValue.Location = new System.Drawing.Point(112, 377);
            this.HandflappingValue.Name = "HandflappingValue";
            this.HandflappingValue.Size = new System.Drawing.Size(16, 13);
            this.HandflappingValue.TabIndex = 35;
            this.HandflappingValue.Text = "...";
            // 
            // ResultsLabel
            // 
            this.ResultsLabel.AutoSize = true;
            this.ResultsLabel.Location = new System.Drawing.Point(165, 399);
            this.ResultsLabel.Name = "ResultsLabel";
            this.ResultsLabel.Size = new System.Drawing.Size(42, 13);
            this.ResultsLabel.TabIndex = 36;
            this.ResultsLabel.Text = "Results";
            // 
            // TotalTestsLabel
            // 
            this.TotalTestsLabel.AutoSize = true;
            this.TotalTestsLabel.Location = new System.Drawing.Point(15, 417);
            this.TotalTestsLabel.Name = "TotalTestsLabel";
            this.TotalTestsLabel.Size = new System.Drawing.Size(62, 13);
            this.TotalTestsLabel.TabIndex = 37;
            this.TotalTestsLabel.Text = "Total tests :";
            // 
            // FaultyTestsLabel
            // 
            this.FaultyTestsLabel.AutoSize = true;
            this.FaultyTestsLabel.Location = new System.Drawing.Point(15, 439);
            this.FaultyTestsLabel.Name = "FaultyTestsLabel";
            this.FaultyTestsLabel.Size = new System.Drawing.Size(66, 13);
            this.FaultyTestsLabel.TabIndex = 38;
            this.FaultyTestsLabel.Text = "Faulty tests :";
            // 
            // SuccessfulTestsLabel
            // 
            this.SuccessfulTestsLabel.AutoSize = true;
            this.SuccessfulTestsLabel.Location = new System.Drawing.Point(15, 461);
            this.SuccessfulTestsLabel.Name = "SuccessfulTestsLabel";
            this.SuccessfulTestsLabel.Size = new System.Drawing.Size(90, 13);
            this.SuccessfulTestsLabel.TabIndex = 39;
            this.SuccessfulTestsLabel.Text = "Successful tests :";
            // 
            // FalsePositivesLabel
            // 
            this.FalsePositivesLabel.AutoSize = true;
            this.FalsePositivesLabel.Location = new System.Drawing.Point(15, 483);
            this.FalsePositivesLabel.Name = "FalsePositivesLabel";
            this.FalsePositivesLabel.Size = new System.Drawing.Size(82, 13);
            this.FalsePositivesLabel.TabIndex = 40;
            this.FalsePositivesLabel.Text = "False positives :";
            // 
            // FalseNegativesLabel
            // 
            this.FalseNegativesLabel.AutoSize = true;
            this.FalseNegativesLabel.Location = new System.Drawing.Point(15, 506);
            this.FalseNegativesLabel.Name = "FalseNegativesLabel";
            this.FalseNegativesLabel.Size = new System.Drawing.Size(87, 13);
            this.FalseNegativesLabel.TabIndex = 41;
            this.FalseNegativesLabel.Text = "False negatives :";
            // 
            // TotalTestsValue
            // 
            this.TotalTestsValue.AutoSize = true;
            this.TotalTestsValue.Location = new System.Drawing.Point(112, 417);
            this.TotalTestsValue.Name = "TotalTestsValue";
            this.TotalTestsValue.Size = new System.Drawing.Size(16, 13);
            this.TotalTestsValue.TabIndex = 42;
            this.TotalTestsValue.Text = "...";
            // 
            // FaultyTestsValue
            // 
            this.FaultyTestsValue.AutoSize = true;
            this.FaultyTestsValue.Location = new System.Drawing.Point(112, 439);
            this.FaultyTestsValue.Name = "FaultyTestsValue";
            this.FaultyTestsValue.Size = new System.Drawing.Size(16, 13);
            this.FaultyTestsValue.TabIndex = 43;
            this.FaultyTestsValue.Text = "...";
            // 
            // SuccessfulTestsValue
            // 
            this.SuccessfulTestsValue.AutoSize = true;
            this.SuccessfulTestsValue.Location = new System.Drawing.Point(112, 461);
            this.SuccessfulTestsValue.Name = "SuccessfulTestsValue";
            this.SuccessfulTestsValue.Size = new System.Drawing.Size(16, 13);
            this.SuccessfulTestsValue.TabIndex = 44;
            this.SuccessfulTestsValue.Text = "...";
            // 
            // FalsePositivesValue
            // 
            this.FalsePositivesValue.AutoSize = true;
            this.FalsePositivesValue.Location = new System.Drawing.Point(112, 483);
            this.FalsePositivesValue.Name = "FalsePositivesValue";
            this.FalsePositivesValue.Size = new System.Drawing.Size(16, 13);
            this.FalsePositivesValue.TabIndex = 45;
            this.FalsePositivesValue.Text = "...";
            // 
            // FalseNegativesValue
            // 
            this.FalseNegativesValue.AutoSize = true;
            this.FalseNegativesValue.Location = new System.Drawing.Point(112, 506);
            this.FalseNegativesValue.Name = "FalseNegativesValue";
            this.FalseNegativesValue.Size = new System.Drawing.Size(16, 13);
            this.FalseNegativesValue.TabIndex = 46;
            this.FalseNegativesValue.Text = "...";
            // 
            // CurrentFilenameLabel
            // 
            this.CurrentFilenameLabel.AutoSize = true;
            this.CurrentFilenameLabel.Location = new System.Drawing.Point(15, 264);
            this.CurrentFilenameLabel.Name = "CurrentFilenameLabel";
            this.CurrentFilenameLabel.Size = new System.Drawing.Size(55, 13);
            this.CurrentFilenameLabel.TabIndex = 47;
            this.CurrentFilenameLabel.Text = "Filename :";
            // 
            // CurrentFilenameValue
            // 
            this.CurrentFilenameValue.AutoSize = true;
            this.CurrentFilenameValue.Location = new System.Drawing.Point(112, 264);
            this.CurrentFilenameValue.Name = "CurrentFilenameValue";
            this.CurrentFilenameValue.Size = new System.Drawing.Size(16, 13);
            this.CurrentFilenameValue.TabIndex = 48;
            this.CurrentFilenameValue.Text = "...";
            // 
            // GestureTestingApp
            // 
            this.ClientSize = new System.Drawing.Size(373, 672);
            this.Controls.Add(this.CurrentFilenameValue);
            this.Controls.Add(this.CurrentFilenameLabel);
            this.Controls.Add(this.FalseNegativesValue);
            this.Controls.Add(this.FalsePositivesValue);
            this.Controls.Add(this.SuccessfulTestsValue);
            this.Controls.Add(this.FaultyTestsValue);
            this.Controls.Add(this.TotalTestsValue);
            this.Controls.Add(this.FalseNegativesLabel);
            this.Controls.Add(this.FalsePositivesLabel);
            this.Controls.Add(this.SuccessfulTestsLabel);
            this.Controls.Add(this.FaultyTestsLabel);
            this.Controls.Add(this.TotalTestsLabel);
            this.Controls.Add(this.ResultsLabel);
            this.Controls.Add(this.HandflappingValue);
            this.Controls.Add(this.PreviousDurationValue);
            this.Controls.Add(this.PreviousFilenameValue);
            this.Controls.Add(this.CurrentDurationValue);
            this.Controls.Add(this.InProcessValue);
            this.Controls.Add(this.HandflappingLabel);
            this.Controls.Add(this.PreviousDurationLabel);
            this.Controls.Add(this.PreviousFilenameLabel);
            this.Controls.Add(this.LastProcessedLabel);
            this.Controls.Add(this.CurrentDurationLabel);
            this.Controls.Add(this.InProcessLabel);
            this.Controls.Add(this.RunTestsButton);
            this.Controls.Add(this.EstimatedRuntimeValue);
            this.Controls.Add(this.TotalDurationValue);
            this.Controls.Add(this.EstimatedRuntimeLabel);
            this.Controls.Add(this.TotalDurationLabel);
            this.Controls.Add(this.NumberOfClipsValue);
            this.Controls.Add(this.NumberOfClipsLabel);
            this.Controls.Add(this.NegativeDirectory);
            this.Controls.Add(this.PositiveDirectory);
            this.Controls.Add(this.HandflappingNegativeButton);
            this.Controls.Add(this.HandflappingPositiveButton);
            this.Controls.Add(this.InformationLabel);
            this.Controls.Add(this.FormerHandflappingValue);
            this.Controls.Add(this.FormerHandflappingLabel);
            this.Controls.Add(this.DurationNote);
            this.Controls.Add(this.FormerDurationValue);
            this.Controls.Add(this.FormerFilenameValue);
            this.Controls.Add(this.FormerDurationLabel);
            this.Controls.Add(this.FormerFilenameLabel);
            this.Controls.Add(this.Open);
            this.Name = "GestureTestingApp";
            this.Text = "Handflapping Test Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindowClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Open;
        private System.Windows.Forms.Label FormerFilenameLabel;
        private System.Windows.Forms.Label FormerDurationLabel;
        private System.Windows.Forms.Label FormerFilenameValue;
        private System.Windows.Forms.Label FormerDurationValue;
        private System.Windows.Forms.Label DurationNote;
        private System.Windows.Forms.Label FormerHandflappingLabel;
        private System.Windows.Forms.Label FormerHandflappingValue;
        private System.Windows.Forms.Label InformationLabel;
        private System.Windows.Forms.Button HandflappingPositiveButton;
        private System.Windows.Forms.Button HandflappingNegativeButton;
        private System.Windows.Forms.Label PositiveDirectory;
        private System.Windows.Forms.Label NegativeDirectory;
        private System.Windows.Forms.Label NumberOfClipsLabel;
        private System.Windows.Forms.Label NumberOfClipsValue;
        private System.Windows.Forms.Label TotalDurationLabel;
        private System.Windows.Forms.Label EstimatedRuntimeLabel;
        private System.Windows.Forms.Label TotalDurationValue;
        private System.Windows.Forms.Label EstimatedRuntimeValue;
        private System.Windows.Forms.Button RunTestsButton;
        private System.Windows.Forms.Label InProcessLabel;
        private System.Windows.Forms.Label CurrentDurationLabel;
        private System.Windows.Forms.Label LastProcessedLabel;
        private System.Windows.Forms.Label PreviousFilenameLabel;
        private System.Windows.Forms.Label PreviousDurationLabel;
        private System.Windows.Forms.Label HandflappingLabel;
        private System.Windows.Forms.Label InProcessValue;
        private System.Windows.Forms.Label CurrentDurationValue;
        private System.Windows.Forms.Label PreviousFilenameValue;
        private System.Windows.Forms.Label PreviousDurationValue;
        private System.Windows.Forms.Label HandflappingValue;
        private System.Windows.Forms.Label ResultsLabel;
        private System.Windows.Forms.Label TotalTestsLabel;
        private System.Windows.Forms.Label FaultyTestsLabel;
        private System.Windows.Forms.Label SuccessfulTestsLabel;
        private System.Windows.Forms.Label FalsePositivesLabel;
        private System.Windows.Forms.Label FalseNegativesLabel;
        private System.Windows.Forms.Label TotalTestsValue;
        private System.Windows.Forms.Label FaultyTestsValue;
        private System.Windows.Forms.Label SuccessfulTestsValue;
        private System.Windows.Forms.Label FalsePositivesValue;
        private System.Windows.Forms.Label FalseNegativesValue;
        private System.Windows.Forms.Label CurrentFilenameLabel;
        private System.Windows.Forms.Label CurrentFilenameValue;
    }
}

