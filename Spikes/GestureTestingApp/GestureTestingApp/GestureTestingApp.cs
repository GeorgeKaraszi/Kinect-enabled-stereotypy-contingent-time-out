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

        /// <summary>
        /// Initialize the window and TestUtility.
        /// </summary>
        public GestureTestingApp()
        {
            InitializeComponent();

            // Create KinectManager.
            ProcessCreator Creator = new ProcessCreator();

            TestUtility = new TestUtility();
            // On file information change for display.
            TestUtility.FileChanged += _FileChanged;
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

            string durationFormatted = "";
            if (h > 0)
            {
                m = m % 60;
                if (s != 1) durationFormatted = h + " hours " + m + " minutes " + s + " seconds";
                else durationFormatted = h + " hours " + m + " minutes " + s + " second";
            }
            else if (m > 0)
            {
                if (s != 1) durationFormatted = m + " minutes " + s + " seconds";
                else durationFormatted = m + " minutes " + s + " second";
            }
            else
            {
                if (s != 1) durationFormatted = s + " seconds";
                else durationFormatted = s + " second";
            }

            return durationFormatted;
        }
        
        private void RunTests(object sender, EventArgs e)
        {
            //TestUtility.RunTests();
        }
    }
}
