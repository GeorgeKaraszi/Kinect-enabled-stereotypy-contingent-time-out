//To Use:
//	Program basic use                 = QuietHandsWindow QHWin = new QuietHandsWindow();
//	Pause Program till display exists = QHWin.ShowDialog();
//	Keep program running              = QHWin.Show();
// 


using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace WesternMichgian.SeniorDesign.KinectProject
{
    public partial class QuietHandsWindow : Form
    {

        private int _timeLeft = 60; //6 seconds
        private int _quitTime = 20; //2 seconds

        public QuietHandsWindow()
        {
            Setupwindow();
            InitializeComponent();
        }

        private void QuietHandsWindow_Load(object sender, EventArgs e)
        {
            SetupTimer();
            //SetupDisplay();
            StartTimer();
            SetupQHSound();
        }
        /// <summary>
        /// Looks for a hot-key Ctrl+Q to quite the window at hand.
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.Q) || keyData == Keys.Escape)
            {

                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Sets the windows size and functionality. This also hides the mouse courser from displaying
        /// </summary>
        private void Setupwindow()
        {
            //Cursor.Hide();
            //FormBorderStyle = FormBorderStyle.None;
            //WindowState = FormWindowState.Maximized;
            WindowState = FormWindowState.Minimized;
            //TopMost = true;
        }

        /// <summary>
        /// Set up the timer's inital display text and progress bar values
        /// </summary>
        private void SetupTimer()
        {
            timer_lbl.Text = (_timeLeft / 10) + " seconds";
            progressBar1.Maximum = _timeLeft;
            progressBar1.Value = _timeLeft;
            progressBar1.Minimum = 0;
        }

        /// <summary>
        /// Start the timer that hands the count down
        /// </summary>
        private void StartTimer()
        {
            qhand_timer.Start();
        }

        /// <summary>
        /// Aligns the objects displayed on the screen to their correct positions.
        /// </summary>
        private void SetupDisplay()
        {
            qhands_lbl.Text = "Quiet Hands";
            //Scale the font of the text upward
            qhands_lbl.Font = new Font(qhands_lbl.Font.FontFamily, 40);
            timer_lbl.Font = new Font(timer_lbl.Font.FontFamily, 20);

            //Center the text with the screen.
            //Move the text slightly up to make it more appealing
            int xQhands = Size.Width / 2 - qhands_lbl.Width / 2;
            int yQhands = (Size.Height / 2 - qhands_lbl.Height / 2) - 150;

            int xTimer = Size.Width / 2 - timer_lbl.Width / 2;
            int yTimer = (Size.Height / 2 - timer_lbl.Height / 2) + 100;

            int xPbar = Size.Width / 2 - progressBar1.Width / 2;
            int yPbar = (Size.Height / 2 - progressBar1.Height / 2) + 150;

            //Center objects
            qhands_lbl.Location = new Point(xQhands, yQhands);
            timer_lbl.Location = new Point(xTimer, yTimer);
            progressBar1.Location = new Point(xPbar, yPbar);
        }

        //WindowsMediaPlayer Player;
        /// <summary>
        /// Launches the audio for saying quite hands to the listener
        /// </summary>
        private void SetupQHSound()
        {
            SoundPlayer qhandsPlayer = new SoundPlayer(@".\Resources\QuietHandsSound.wav");
            qhandsPlayer.Play();
        }

        private void qhand_timer_Tick(object sender, EventArgs e)
        {
            if (_timeLeft > 0)
            {
                _timeLeft -= 1;
                progressBar1.Value = _timeLeft;
                timer_lbl.Text = (_timeLeft / 10) + " seconds";
            }
            else
            {
                qhand_timer.Stop();
                timer_lbl.Text = "Times up! You may continue!";
                SetupDisplay();
                quitTimer.Start();

            }
        }

        private void quitTimer_Tick(object sender, EventArgs e)
        {
            if (_quitTime > 0)
                _quitTime -= 1;
            else
            {
                quitTimer.Stop();
                Close();
            }
        }

        /// <summary>
        /// Be sure to re-enable the mouse, otherwise course wont display 
        /// while the rest of the application is running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onClose(object sender, FormClosingEventArgs e)
        {
            Cursor.Show();
        }
    }
}
