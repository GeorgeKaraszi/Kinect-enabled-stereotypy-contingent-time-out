using System;
using System.Windows.Forms;

namespace GestureTestingApp
{
    public partial class GestureTestingApp : Form
    {
        private readonly Playback playback;

        public GestureTestingApp()
        {
            InitializeComponent();

            playback = new Playback(this);

            playback.FileChanged += _FileChanged;
        }

        /// <summary>
        /// Let user choose a file to play and play it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayClip(object sender, EventArgs e)
        {
            // Open dialog box to allow user to choose a Kinect clip to play.
            OpenFileDialog openFile = new OpenFileDialog();

            openFile.InitialDirectory = "C:\\";

            if (openFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    playback.PlayClip(openFile.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in file open: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// On close, properly dispose.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowClosing(object sender, FormClosingEventArgs e)
        {
            playback.Dispose();
        }

        ///// <summary>
        ///// Method for notifying window to update its fields.
        ///// </summary>
        ///// <param name="filename"></param>
        ///// <param name="duration"></param>
        ///// <param name="completed"></param>
        ///// <returns></returns>
        //public bool UpdateFile(string filename, double duration, bool completed)
        //{
        //    this.FilenameValue.Text = filename;
        //    this.DurationValue.Text = duration.ToString();
        //    if (!completed)
        //        this.ProcessingValue.Text = "In Process";
        //    else if (completed)
        //        this.ProcessingValue.Text = "Completed";

        //    return true;
        //}

        /// <summary>
        /// Event for notifying window to update its fields.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _FileChanged(object sender, FileChangeEventArgs e)
        {
            this.FilenameValue.Text = e.Filename;
            this.DurationValue.Text = e.Duration.ToString("F2") + " seconds";
            if (!e.Completed)
                this.ProcessingValue.Text = "In Process";
            else if (e.Completed)
                this.ProcessingValue.Text = "Completed";
        }
    }
}
