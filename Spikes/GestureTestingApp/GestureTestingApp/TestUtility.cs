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
using System.IO.Pipes;

namespace GestureTestingApp
{
    public delegate void FileChangeEvent(object sender, FileChangeEventArgs e);
    public delegate void NewFileEvent(object sender, NewFileEventArgs e);
    public delegate void PreviousFileEvent(object sender, PreviousFileEventArgs e);
    public delegate void TestingCompleteEvent(object sender, TestingCompleteEventArgs e);
    public delegate void ClosingEvent(object sender, EventArgs e);
    /// <summary>
    /// For playing Kinect clips.
    /// </summary>
    public class TestUtility : IDisposable
    {
        //// For talking to the Kinect handler.
        //private MailslotClient Client;
        //// Mailbox for receiving hand flapping detected signals from handler.
        //private MailslotServer Server;
        // Let's try pipes.
        private NamedPipeClientStream Client;
        private NamedPipeServerStream Server;
        // Path of current file to be played, its filename, and its duration in seconds.
        private string Filepath;
        private string Filename;
        private double Duration;
        private bool HandflappingDetected;
        // For if any errors were detected.
        private bool Error;
        // The number by which to divide the length of a Kinect clip to estimate its duration.
        private const double KinectClipConstant = 33287554.5;

        public event FileChangeEvent FileChanged;
        public event NewFileEvent NewFile;
        public event PreviousFileEvent PreviousFile;
        public event TestingCompleteEvent TestingComplete;
        public event ClosingEvent Closing;

        Thread PlaybackThread;

        private List<string> PositiveList;
        private List<string> NegativeList;

        private double PositiveDuration;
        private double NegativeDuration;

        public void RunTests()
        {
            int i = 1;
            int Count = PositiveList.Count + NegativeList.Count;
            Thread PlayThread;
            int TotalTests = Count,
                FaultyTests = 0,
                SuccessfulTests = 0,
                FalsePositives = 0,
                FalseNegatives = 0;
            // Loop through each file in each list and sleep for a sec.
            for (int j = 0; j < PositiveList.Count; j++)
            {
                Filepath = PositiveList[j];
                Filename = Path.GetFileName(Filepath);
                FileInfo f = new FileInfo(Filepath);
                Duration = f.Length / KinectClipConstant;
                HandflappingDetected = false;
                NewFileEventArgs e = new NewFileEventArgs(i + " / " + Count, Filename, Duration);

                NewFile?.Invoke(this, e);
                PlayThread = new Thread(PlayFile);
                PlayThread.Start();
                PlayThread.Join();

                // Handflapping and no handflapping detected?
                if (!HandflappingDetected)
                    FalseNegatives++;
                else
                    SuccessfulTests++;

                PreviousFileEventArgs g = new PreviousFileEventArgs(Filename, Duration, HandflappingDetected);

                PreviousFile?.Invoke(this, g);
                i++;
            }
            for (int j = 0; j < NegativeList.Count; j++)
            {
                Filepath = NegativeList[j];
                Filename = Path.GetFileName(Filepath);
                FileInfo f = new FileInfo(Filepath);
                Duration = f.Length / KinectClipConstant;
                HandflappingDetected = false;
                NewFileEventArgs e = new NewFileEventArgs(i + " / " + Count, Filename, Duration);

                NewFile?.Invoke(this, e);
                PlayThread = new Thread(PlayFile);
                PlayThread.Start();
                PlayThread.Join();

                // No handflapping and handflapping detected?
                if (HandflappingDetected)
                    FalsePositives++;
                else
                    SuccessfulTests++;

                PreviousFileEventArgs g = new PreviousFileEventArgs(Filename, Duration, HandflappingDetected);

                PreviousFile?.Invoke(this, g);
                i++;
            }

            TestingCompleteEventArgs h = new TestingCompleteEventArgs
                (TotalTests, FaultyTests, SuccessfulTests, FalsePositives, FalseNegatives);

            if (TotalTests > 0)
                TestingComplete?.Invoke(this, h);
        }

        private void Play()
        {
            Thread.Sleep(1000);
        }

        // Return estimated number of seconds of all the clips in the folder.
        public double TotalDuration
        {
            get
            {
                return PositiveDuration + NegativeDuration;
            }
        }

        public string PositiveDirectory
        {
            set
            {
                try
                {
                    string[] FileList = Directory.GetFiles(value);
                    PositiveList.Clear();
                    PositiveDuration = 0;

                    foreach (string file in FileList)
                    {
                        FileInfo f = new FileInfo(file);
                        if (f != null)
                        {
                            if (f.Extension == ".xef")
                            {
                                PositiveList.Add(file);
                                PositiveDuration += (f.Length / KinectClipConstant);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in PositiveDirectory: " +
                        ex.GetType().ToString() + " : " + ex.Message);
                }
            }
        }
        public string NegativeDirectory
        {
            set
            {
                try
                {
                    string[] FileList = Directory.GetFiles(value);
                    NegativeList.Clear();
                    NegativeDuration = 0;

                    foreach (string file in FileList)
                    {
                        FileInfo f = new FileInfo(file);
                        if (f != null)
                        {
                            if (f.Extension == ".xef")
                            {
                                NegativeList.Add(file);
                                NegativeDuration += (f.Length / KinectClipConstant);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in PositiveDirectory: " +
                        ex.GetType().ToString() + " : " + ex.Message);
                }
            }
        }

        public int NumberOfFiles
        {
            get
            {
                int count = 0;

                if (PositiveList != null)
                    count += PositiveList.Count;
                if (NegativeList != null)
                    count += NegativeList.Count;

                return count;
            }
        }

        public TestUtility()
        {
            // Kinect handler cannot be local to this process and it cannot simply
            // be placed in a new thread,
            // Kinect handler must be in another process.
            Client = new NamedPipeClientStream("testutility");
            Client.Connect();
            Server = new NamedPipeServerStream("kinectmanager");
            Server.WaitForConnection();

            //Client = new MailslotClient("kinectmanager");
            //Server = new MailslotServer("testutility");

            Filepath = null;
            Filename = null;
            Duration = -1;
            HandflappingDetected = false;

            Error = false;

            PlaybackThread = null;

            PositiveList = new List<string>();
            NegativeList = new List<string>();

            PositiveDuration = 0;
            NegativeDuration = 0;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Play the clip specified.
        /// </summary>
        /// <param name="filepath"></param>
        public void PlayClip(string filepath)
        {
            //HandflappingDetected = false;

            //// Information must be gathered here (which means approximating the duration)
            //// because once the KStudioPlayback is instantiated, the window will not be updated.
            //// The precise duration is obtained from the KStudioPlayback, so it must be
            //// approximated using the length of the file.
            //Filepath = filepath;
            //FileInfo f = new FileInfo(Filepath);
            //Filename = Path.GetFileName(Filepath);
            //bool xef = (f.Extension == ".xef");
            //if (xef)
            //{
            //    Duration = f.Length / KinectClipConstant;
            //    Error = false;
            //}
            //else
            //{
            //    Duration = 0;
            //    Error = true;
            //}

            //FileChangeEventArgs e = new FileChangeEventArgs(Filename, Duration, "Processing");

            //// Update fields in the window.
            //FileChanged?.Invoke(this, e);

            //// Allow users to try bad files, but ultimately only try to play .xefs.
            //if (xef)
            //{
                // Playback must be in its own thread because once KStudioPlayback
                // has been instantiated, the form will not be updated until
                // playback has terminated.
                PlaybackThread = new Thread(PlayFile);
                PlaybackThread.Start();
                PlaybackThread.Join();
            //}

            //// Here send processing status.
            //if (Error)
            //{
            //    e.Status = "Error";
            //}
            //else
            //{
            //    if (HandflappingDetected)
            //        e.Status = "Detected";
            //    else
            //        e.Status = "Not detected";
            //}
            //FileChanged?.Invoke(this, e);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Thread function for playing the Kinect clip.
        /// </summary>
        private void PlayFile()
        {
            // Send file name to kinect handler so it can save the data from each file.
            //using (MailslotClient Client = new MailslotClient("kinectmanager"))
            //{
            //    Client.SendMessage(Filename);
            //}

            // Play the clip.
            using (KStudioClient KStudioClient = KStudio.CreateClient())
            {
                KStudioClient.ConnectToService();

                try
                {
                    using (KStudioPlayback PlaybackFile = KStudioClient.CreatePlayback(Filepath))
                    {
                        // The number of frames of the clip is needed for data collection.
                        int num_frames = (int)(PlaybackFile.Duration.TotalMilliseconds / 30);
                        //using (MailslotClient Client = new MailslotClient("kinectmanager"))
                        //{
                        //    Client.SendMessage(framesMsg);
                        //}
                        //using (NamedPipeServerStream Server = new NamedPipeServerStream("kinectmanager"))
                        try
                        {
                            using (Server)
                            {
                                //Server.WaitForConnection();
                                using (StreamWriter ss = new StreamWriter(Server))
                                {
                                    ss.WriteLine(Filename + ";" + num_frames);
                                }
                            }
                        }
                        catch (ObjectDisposedException ex)
                        {
                            Server = new NamedPipeServerStream("kinectmanager");
                            Server.WaitForConnection();
                            using (StreamWriter ss = new StreamWriter(Server))
                            {
                                ss.WriteLine(Filename + ";" + num_frames);
                            }
                        }
                        // Clip must be looped because of skeletal tagging lag in intro.
                        PlaybackFile.LoopCount = 1;
                        PlaybackFile.Start();

                        while (PlaybackFile.State == KStudioPlaybackState.Playing)
                        {
                            // Check if there is a message about hand flapping detection.
                            //using (MailslotServer Server = new MailslotServer("testutility"))
                            //{
                            //    msg = Server.GetNextMessage();
                            //}
                            //using (NamedPipeClientStream Client = new NamedPipeClientStream("testutility"))
                            //{
                            //    Client.Connect();
                            //    StreamReader ss = new StreamReader(Client);
                            //    msg = ss.ReadLine();
                            //}
                            //if (msg != null && msg == "HandflappingDetected")
                            //{
                            //    HandflappingDetected = true;
                            //}
                            Thread.Sleep(500);
                        }

                        if (PlaybackFile.State == KStudioPlaybackState.Error)
                        {
                            MessageBox.Show("Error: Playback failed in TestUtility.PlayFile!");
                            Error = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in TestUtility.PlayFile : " +
                        ex.GetType().ToString() + " : " + ex.Message);
                    Error = true;
                }

                KStudioClient.DisconnectFromService();
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Function that loops until exit waiting for messages from the KinectManager.
        /// </summary>
        public void Monitor()
        {
            // Get any messages. If being told to exit, exit.
            // Otherwise, message is a filepath or a number of frames.
            while (true)
            {
                try
                {
                    string msg;
                    do
                    {
                        //using (MailslotServer Server = new MailslotServer("testutility"))
                        //{
                        //    msg = Server.GetNextMessage();
                        //}
                        using (Client)
                        {
                            //Client.Connect();
                            using (StreamReader ss = new StreamReader(Client))
                            {
                                msg = ss.ReadLine();
                            }
                        }
                        // All Kinect clips should be at least 1/2 second long.
                        if (msg == null)
                        {
                            Thread.Sleep(500);
                        }
                    } while (msg == null);
                    // Been told that hand flapping's occurred?
                    if (msg == "HandflappingDetected")
                    {
                        HandflappingDetected = true;
                    }
                }
                // If the thread is aborted, dispose of it properly.
                catch (ThreadAbortException ex)
                {
                    if (this.PlaybackThread != null)
                        this.PlaybackThread.Abort();
                }
                catch (ObjectDisposedException ex)
                {
                    Client = new NamedPipeClientStream("testutility");
                    Client.Connect();
                    continue;
                }
                // Catch any other exception that might be thrown.
                catch (Exception ex)
                {
                    MessageBox.Show("Error in TestUtility.Monitor : " +
                        ex.GetType().ToString() + " : " + ex.Message);
                }
            }

            //Closing?.Invoke(this, EventArgs.Empty);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Properly dispose of anything used.
        /// </summary>
        public void Dispose()
        {
            //if (Client != null)
            //{
            // Tell KinectManager to exit.
            //using (MailslotClient Client = new MailslotClient("kinectmanager"))
            //{
            //    Client.SendMessage("exit");
            //}
            try
            {
                using (Server)
                {
                    using (StreamWriter ss = new StreamWriter(Server))
                    {
                        ss.WriteLine("exit");
                    }
                }
            }
            catch (ObjectDisposedException ex)
            {
                Server = new NamedPipeServerStream("kinectmanager");
                Server.WaitForConnection();
                using (Server)
                {
                    using (StreamWriter ss = new StreamWriter(Server))
                    {
                        ss.WriteLine("exit");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in TestUtility.Dispose : " +
                    ex.GetType().ToString() + " : " + ex.Message);
            }

            //Server.Dispose();
            //Client.Dispose();
            //    Client.Dispose();
            //    Client = null;
            //}
            //if (Server != null)
            //{
            //    Server.Dispose();
            //    Server = null;
            //}
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

        protected virtual void OnNewFile(NewFileEventArgs e)
        {
            NewFileEvent handler = NewFile;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnPreviousFile(PreviousFileEventArgs e)
        {
            PreviousFileEvent handler = PreviousFile;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnTestingComplete(TestingCompleteEventArgs e)
        {
            TestingCompleteEvent handler = TestingComplete;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }

    //====================================================================================
    /// <summary>
    /// When a new file is selected to be played, change info displayed in UI.
    /// </summary>
    /// _kinectHandle.OnSkeletonChange += _kinectHandle_OnSkeletonChange
    /// public event FileChangeEvent OnFileChange;
    /// OnFileChange?.Invoke(this, new FileChangeEventArgs(filename, duration));
    public class FileChangeEventArgs : EventArgs
    {
        public string Filename { get; set; }
        public double Duration { get; set; }
        public string Status { get; set; }

        public FileChangeEventArgs(string filename, double duration, string status)
        {
            Filename = filename;
            Duration = duration;
            Status = status;
        }
    }

    /// <summary>
    /// When a new file is selected to be played, update UI.
    /// </summary>
    public class NewFileEventArgs : EventArgs
    {
        public string Status { get; set; }
        public string Filename { get; set; }
        public double Duration { get; set; }

        public NewFileEventArgs(string status, string filename, double duration)
        {
            Status = status;
            Filename = filename;
            Duration = duration;
        }
    }

    /// <summary>
    /// When a file has just finished being played, update UI.
    /// </summary>
    public class PreviousFileEventArgs : EventArgs
    {
        public string Filename { get; set; }
        public double Duration { get; set; }
        public bool Detected { get; set; }

        public PreviousFileEventArgs(string filename, double duration, bool detected)
        {
            Filename = filename;
            Duration = duration;
            Detected = detected;
        }
    }

    /// <summary>
    /// When testing has finished, update UI.
    /// </summary>
    public class TestingCompleteEventArgs : EventArgs
    {
        public int TotalTests { get; set; }
        public int FaultyTests { get; set; }
        public int SuccessfulTests { get; set; }
        public int FalsePositives { get; set; }
        public int FalseNegatives { get; set; }

        public TestingCompleteEventArgs(int total, int faulty, int successful, int falsepos, int falseneg)
        {
            TotalTests = total;
            FaultyTests = faulty;
            SuccessfulTests = successful;
            FalsePositives = falsepos;
            FalseNegatives = falseneg;
        }
    }
}
