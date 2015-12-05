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
            this.FilenameValue.Text = e.Filename;
            this.DurationValue.Text = e.Duration.ToString("F2") + " seconds";
            this.HandflappingValue.Text = e.Status;
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
    }
}
