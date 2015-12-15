using System;
using System.Threading;
using System.Windows.Forms;

namespace GestureTestingApp
{
    public partial class GestureTestingApp : Form
    {
        // Thread to receive messages from the KinectManager.
        private Thread TestUtilityThread;
        private TestUtility TestUtility;

        static private Mutex Lock;

        /// <summary>
        /// Initialize the window and TestUtility.
        /// </summary>
        public GestureTestingApp()
        {
            InitializeComponent();

            Lock = new Mutex();

            // Create KinectManager.
            ProcessCreator Creator = new ProcessCreator();

            TestUtility = new TestUtility();
            // On file information change for display.
            TestUtility.FileChanged += _FileChanged;
            // Update currently processing file's information.
            TestUtility.NewFile += _NewFile;
            // Update formerly processed file's information.
            TestUtility.PreviousFile += _PreviousFile;
            // Update testing results.
            TestUtility.TestingComplete += _TestingComplete;
            // If the TestUtility wants to close for some reason.
            TestUtility.Closing += _Closing;
            // Thread to run the message receiver of the KinectManager.
            TestUtilityThread = new Thread(TestUtility.Monitor);
            TestUtilityThread.Start();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Let user choose a file to play and play it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayClip(object sender, EventArgs e)
        {
            // Open dialog box to allow user to choose a Kinect clip to play.
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    TestUtility.PlayClip(openFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in GestureTestingApp.PlayClip : " + 
                        ex.GetType().ToString() + " : " + ex.Message);
                }
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// On close, properly dispose.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowClosing(object sender, FormClosingEventArgs e)
        {
            TestUtilityThread.Abort();
            TestUtility.Closing -= _Closing;
            TestUtility.Dispose();
            Environment.Exit(0);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Event for notifying window to update its fields.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _FileChanged(object sender, FileChangeEventArgs e)
        {
            this.FormerFilenameValue.Text = e.Filename;
            this.FormerDurationValue.Text = e.Duration.ToString("F2") + " seconds";
            this.FormerHandflappingValue.Text = e.Status;
        }

        /// <summary>
        /// Event for updating currently processing file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _NewFile(object sender, NewFileEventArgs e)
        {
            Lock.WaitOne();

            this.InProcessValue.Text = e.Status;
            this.CurrentFilenameValue.Text = e.Filename;
            this.CurrentDurationValue.Text = Format(e.Duration * 2);

            Lock.ReleaseMutex();
        }

        /// <summary>
        /// Event for updating formerly processed file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _PreviousFile(object sender, PreviousFileEventArgs e)
        {
            Lock.WaitOne();

            this.PreviousFilenameValue.Text = e.Filename;
            this.PreviousDurationValue.Text = Format(e.Duration * 2);
            if (e.Detected)
                this.HandflappingValue.Text = "Detected";
            else
                this.HandflappingValue.Text = "Not detected";

            Lock.ReleaseMutex();
        }

        /// <summary>
        /// Event for updating testing results section.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _TestingComplete(object sender, TestingCompleteEventArgs e)
        {
            int GoodTests = e.TotalTests - e.FaultyTests;

            Lock.WaitOne();

            this.TotalTestsValue.Text = e.TotalTests.ToString();
            this.FaultyTestsValue.Text = e.FaultyTests.ToString();
            this.SuccessfulTestsValue.Text = e.SuccessfulTests + " / " + GoodTests;
            this.FalsePositivesValue.Text = e.FalsePositives + " / " + GoodTests;
            this.FalseNegativesValue.Text = e.FalseNegatives + " / " + GoodTests;

            if (GoodTests > 0)
            {
                this.SuccessfulTestsPercent.Text = (e.SuccessfulTests * 100 / GoodTests) + " %";
                this.FalsePositivesPercent.Text = (e.FalsePositives * 100 / GoodTests) + " %";
                this.FalseNegativesPercent.Text = (e.FalseNegatives * 100 / GoodTests) + " %";
            }

            Lock.ReleaseMutex();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// If something went wrong in the monitor, close the operation down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Closing(object sender, EventArgs e)
        {
            //TestUtilityThread.Join();
            TestUtilityThread.Abort();
            TestUtility.Closing -= _Closing;
            TestUtility.Dispose();
            Environment.Exit(0);
        }

        private void ChoosePositiveDirectory(object sender, EventArgs e)
        {
            // Open dialog box to allow user to choose a directory.
            FolderBrowserDialog openFolder = new FolderBrowserDialog();

            if (openFolder.ShowDialog() == DialogResult.OK)
            {
                PositiveDirectory.Text = openFolder.SelectedPath;
                TestUtility.PositiveDirectory = openFolder.SelectedPath;
                NumberOfClipsValue.Text = TestUtility.NumberOfFiles.ToString();
                TotalDurationValue.Text = Format(TestUtility.TotalDuration);
                EstimatedRuntimeValue.Text = Format(TestUtility.TotalDuration * 2);
            }
        }

        private void ChooseNegativeDirectory(object sender, EventArgs e)
        {
            FolderBrowserDialog openFolder = new FolderBrowserDialog();

            if (openFolder.ShowDialog() == DialogResult.OK)
            {
                NegativeDirectory.Text = openFolder.SelectedPath;
                TestUtility.NegativeDirectory = openFolder.SelectedPath;
                NumberOfClipsValue.Text = TestUtility.NumberOfFiles.ToString();
                TotalDurationValue.Text = Format(TestUtility.TotalDuration);
                EstimatedRuntimeValue.Text = Format(TestUtility.TotalDuration * 2);
            }
        }

        private string Format(double duration)
        {
            int h, m, s;
            m = (int) duration / 60;
            h = m / 60;
            s = (int) duration - (m * 60);
            m = m % 60;

            string sec, min, hour;

            if (s != 1)
                sec = " seconds";
            else
                sec = " second";

            if (m != 1)
                min = " minutes ";
            else
                min = " minute ";

            if (h != 1)
                hour = " hours ";
            else
                hour = " hour ";

            string durationFormatted = "";
            if (h > 0)
            {
                durationFormatted = h + hour + m + min + s + sec;
            }
            else if (m > 0)
            {
                durationFormatted = m + min + s + sec;
            }
            else
            {
                durationFormatted = s + sec;
            }

            return durationFormatted;
        }
        
        private void RunTests(object sender, EventArgs e)
        {
            TestUtility.RunTests();
        }
    }
}
