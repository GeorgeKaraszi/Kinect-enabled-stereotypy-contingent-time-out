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

            playback = new Playback();
        }

        private void OpenClip(object sender, EventArgs e)
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
    }
}
