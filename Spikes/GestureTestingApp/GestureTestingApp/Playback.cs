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
    /// <summary>
    /// For playing Kinect clips.
    /// </summary>
    class Playback
    {
        public Playback()
        {
            // Kinect handler cannot be local to this process and it cannot simply
            // be placed in a new thread,
            // Kinect handler must be in another process.
            ProcessCreator Creator = new ProcessCreator();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Play the clip specified.
        /// </summary>
        /// <param name="filepath"></param>
        public void PlayClip(String filepath)
        {
            // Once multiple clips are being played in sequence, the mailbox's scope
            // will spread to the class and it's instantiation to the constructor.
            MailslotClient Mailbox = new MailslotClient("kinect");
            
            if (filepath == null)
            {
                MessageBox.Show("Error: Null input file.");
                return;
            }

            // Send file name to kinect handler so it can save the data from each file.
            String filename = Path.GetFileName(filepath);
            using (Mailbox)
            {
                Mailbox.SendMessage(filename);
            }

            // Play the clip.
            using (KStudioClient Client = KStudio.CreateClient())
            {
                Client.ConnectToService();

                try
                {
                    using (KStudioPlayback Playback = Client.CreatePlayback(filepath))
                    {
                        // The number of frames of the clip is needed for data collection.
                        int num_frames = (int)(Playback.Duration.TotalMilliseconds/30);
                        String framesMsg = String.Concat("~", num_frames.ToString());
                        using (Mailbox)
                        {
                            Mailbox.SendMessage(framesMsg);
                        }

                        // Clip must be looped because of skeletal tagging lag in intro.
                        Playback.LoopCount = 1;
                        Playback.Start();

                        while (Playback.State == KStudioPlaybackState.Playing)
                        {
                            Thread.Sleep(500);
                        }

                        if (Playback.State == KStudioPlaybackState.Error)
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

            // Send message to Kinect handler process to exit.
            using (Mailbox)
            {
                Mailbox.SendMessage("exit");
            }

            Mailbox.Dispose();

            Application.Exit();
        }
    }
}
