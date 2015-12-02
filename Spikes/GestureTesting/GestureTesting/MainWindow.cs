using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestureTesting
{
    public partial class MainWindow : Form
    {
        // Thread to receive messages from testing utility.
        // Right now for "exit" and sharing information.
        private Thread ReceiverThread;
        private MessageReception Receiver;

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initialize window and message reception thread with Kinect handle.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Receiver = new MessageReception(this);
            ReceiverThread = new Thread(Receiver.Monitor);
            ReceiverThread.Start();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// For taking down the KinectHandle window with the TestUtility window.
        /// </summary>
        public void Exit()
        {
            // This is what takes this window down with the TestUtility.
            Environment.Exit(0);
        }

        //================================================================================
        /// <summary>
        /// Holds the KinectHandle, access to the mailbox, and a reference to its
        /// calling window so that the destruction of all three can be achieved here.
        /// </summary>
        private class MessageReception
        {
            // Modified KinectHandle for gesture testing.
            KinectHandle Handle;
            // Mailbox for receiving "exit" commands, and sharing any necessary info.
            MailslotServer Mailbox;
            // Calling window so the receiver can talk to it.
            MainWindow CallingWindow;

            public MessageReception(MainWindow w)
            {
                Handle = new KinectHandle();
                Mailbox = new MailslotServer("kinect");
                CallingWindow = w;
            }

            /// <summary>
            /// Thread function that loops until exit waiting for messages from the TestUtility.
            /// </summary>
            public void Monitor()
            {
                // Get any messages. If being told to exit, exit.
                // Otherwise, message is a filepath or a number of frames.
                using (Mailbox)
                {
                    while (true)
                    {
                        try
                        {
                            var msg = Mailbox.GetNextMessage();

                            // If the thread sleeps, there will be lag,
                            // But it's an option.
                            while (msg == null)
                            {
                                Thread.Sleep(10);
                                msg = Mailbox.GetNextMessage();
                            }
                            // Been given the signal to terminate?
                            if (msg == "exit")
                            {
                                Dispose();
                                // Send termination signal to window.
                                CallingWindow.Exit();
                            }
                            // Signifies a frame number for passing to the Kinect handle.
                            else if (msg.StartsWith("~"))
                            {
                                Handle.NumFrames = Int32.Parse(msg.Substring(1));
                            }
                            // Otherwise, msg is a filename, also for passing to the Kinect.
                            else
                            {
                                Handle.FileName = msg;
                            }
                        }
                        // If the thread is aborted, dispose of it properly.
                        catch (ThreadAbortException ex)
                        {
                            Dispose();
                        }
                        // Catch any other exception that might.
                        catch (Exception ex)
                        {
                            MessageBox.Show("Mailbox error : " + ex.GetType().ToString() + " : " + ex.Message);
                        }
                    }
                }
            }

            //--------------------------------------------------------------------------------
            /// <summary>
            /// Cleans up the Kinect handle and the mailbox.
            /// The Kinect is important because it will take care of the saving of the files.
            /// </summary>
            private void Dispose()
            {
                if (Handle != null)
                {
                    Handle.Dispose();
                    Handle = null;
                }
                if (Mailbox != null)
                {
                    Mailbox.Dispose();
                    Mailbox = null;
                }
            }
        }

        //================================================================================
        /// <summary>
        /// If somebody closes the window, the final thread must go with it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowClosing(object sender, FormClosingEventArgs e)
        {
            ReceiverThread.Abort();
        }

        /// <summary>
        /// Initialize the window minimized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLoad(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}
