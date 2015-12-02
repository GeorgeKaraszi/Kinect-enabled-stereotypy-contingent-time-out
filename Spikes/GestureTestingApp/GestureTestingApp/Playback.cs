using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Kinect.Tools;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace GestureTestingApp
{
    public delegate void FileChangeEvent(object sender, FileChangeEventArgs e);
    /// <summary>
    /// For playing Kinect clips.
    /// </summary>
    class Playback
    {
        // For talking to calling window and updating fields.
        private GestureTestingApp CallingWindow;
        // For talking to the Kinect handler.
        private MailslotClient Mailbox;
        // Path of current file to be played, its filename, and its duration in seconds.
        private String Filepath;
        private String Filename;
        private double Duration;
        // The number by which to divide the length of a Kinect clip to estimate its duration.
        private const double KinectClipConstant = 33287554.5;

        public event FileChangeEvent FileChanged;

        public Playback(GestureTestingApp w)
        {
            // Kinect handler cannot be local to this process and it cannot simply
            // be placed in a new thread,
            // Kinect handler must be in another process.
            ProcessCreator Creator = new ProcessCreator();
            
            Mailbox = new MailslotClient("kinect");

            CallingWindow = w;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Play the clip specified.
        /// </summary>
        /// <param name="filepath"></param>
        public void PlayClip(String filepath)
        {
            if (filepath == null)
            {
                MessageBox.Show("Error: Null input file.");
                return;
            }

            // Information must be gathered here (which means approximating the duration)
            // because once the KStudioPlayback is instantiated, the window will not be updated.
            // The precise duration is obtained from the KStudioPlayback, so it must be
            // approximated using the length of the file.
            Filepath = filepath;
            FileInfo f = new FileInfo(Filepath);
            Filename = Path.GetFileName(Filepath);
            Duration = f.Length / KinectClipConstant;

            FileChangeEventArgs e = new FileChangeEventArgs(Filename, Duration, false);

            // Can use event or method call to update window.
            FileChanged?.Invoke(this, e);
            //CallingWindow.UpdateFile(Filename, Duration, false);

            // Playback must be in its own thread because once KStudioPlayback
            // has been instantiated, the form will not be updated until
            // playback has terminated.
            Thread PlaybackThread = new Thread(PlayFile);
            PlaybackThread.Start();
            PlaybackThread.Join();

            // Here send event that processing has completed.
            e.Completed = true;
            FileChanged?.Invoke(this, e);
            //CallingWindow.UpdateFile(Filename, Duration, true);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Thread function for playing the Kinect clip so that the other process can run.
        /// </summary>
        private void PlayFile()
        {
            // Send file name to kinect handler so it can save the data from each file.
            using (Mailbox)
            {
                Mailbox.SendMessage(Filename);
            }

            // Play the clip.
            using (KStudioClient Client = KStudio.CreateClient())
            {
                Client.ConnectToService();

                try
                {
                    using (KStudioPlayback PlaybackFile = Client.CreatePlayback(Filepath))
                    {
                        // The number of frames of the clip is needed for data collection.
                        int num_frames = (int)(PlaybackFile.Duration.TotalMilliseconds / 30);
                        String framesMsg = String.Concat("~", num_frames.ToString());
                        using (Mailbox)
                        {
                            Mailbox.SendMessage(framesMsg);
                        }
                        // Clip must be looped because of skeletal tagging lag in intro.
                        PlaybackFile.LoopCount = 1;
                        PlaybackFile.Start();

                        while (PlaybackFile.State == KStudioPlaybackState.Playing)
                        {
                            Thread.Sleep(500);
                        }

                        if (PlaybackFile.State == KStudioPlaybackState.Error)
                        {
                            throw new InvalidOperationException("Error: Playback failed!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in playback: " + ex.GetType().ToString() + " : " + ex.Message);
                }
                Client.DisconnectFromService();
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Properly dispose of anything used.
        /// </summary>
        public void Dispose()
        {
            // Send message to Kinect handler process to exit.
            using (Mailbox)
            {
                Mailbox.SendMessage("exit");
            }

            Mailbox.Dispose();

            Application.Exit();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// When a file is changed, update form.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFileChange(FileChangeEventArgs e)
        {
            FileChangeEvent handler = FileChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    //====================================================================================
    /// <summary>
    /// When a file is selected to be played, display it's information in the form.
    /// </summary>
    /// _kinectHandle.OnSkeletonChange += _kinectHandle_OnSkeletonChange
    /// public event FileChangeEvent OnFileChange;
    /// OnFileChange?.Invoke(this, new FileChangeEventArgs(filename, duration));
    public class FileChangeEventArgs : EventArgs
    {
        public string Filename { get; set; }
        public double Duration { get; set; }
        public bool Completed { get; set; }

        public FileChangeEventArgs(string filename, double duration, bool completed)
        {
            Filename = filename;
            Duration = duration;
            Completed = completed;
        }
    }
}
