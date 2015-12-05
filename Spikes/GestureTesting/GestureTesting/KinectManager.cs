using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Pipes;
using System.IO;

namespace GestureTesting
{
    public delegate void ClosingEvent(object sender, EventArgs e);
    /// <summary>
    /// Holds the KinectHandle, access to the mailboxes, and a reference to its
    /// calling window so that the destruction of all three can be achieved here.
    /// </summary>
    public class KinectManager : IDisposable
    {
        // Modified KinectHandle for gesture testing.
        KinectHandle Handle;
        //// Mailbox for receiving "exit" commands, and any necessary info.
        //MailslotServer Server;
        //// Mailbox for sending hand flapping detected signals.
        //MailslotClient Client;
        // Try pipes.
        NamedPipeServerStream Server;
        NamedPipeClientStream Client;

        public event ClosingEvent Closing;

        public KinectManager()
        {
            Handle = new KinectHandle(this);
            // Add event handler to Kinect's HandFlappingDetectedEvent.

            //Server = new MailslotServer("kinectmanager");
            //Client = new MailslotClient("testutility");

            Server = new NamedPipeServerStream("testutility");
            Server.WaitForConnection();
            Client = new NamedPipeClientStream("kinectmanager");
            Client.Connect();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Function that loops until exit waiting for messages from the TestUtility.
        /// </summary>
        public void Monitor()
        {
            // Get any messages. If being told to exit, exit.
            // Otherwise, message is a filepath or a number of frames.
            while (true)
            {
                try
                {
                    String msg;
                    do
                    {
                        //using (MailslotServer Server = new MailslotServer("kinectmanager"))
                        //{
                        //    msg = Server.GetNextMessage();
                        //}
                        //using (NamedPipeClientStream Client = new NamedPipeClientStream("kinectmanager"))
                        using (Client)
                        {
                            using (StreamReader ss = new StreamReader(Client))
                            {
                                msg = ss.ReadLine();
                            }
                        }
                        // All Kinect clips should be at least 1/2 second long.
                        if (msg == null || msg.Length == 0)
                        {
                            Thread.Sleep(500);
                        }
                    } while (msg == null);
                    // Been given the signal to terminate?
                    if (msg == "exit")
                    {
                        break;
                    }
                    // Otherwise, msg is a filename with number of frames.
                    else
                    {
                        String[] tokens;
                        char[] delims = { ';' };
                        tokens = msg.Split(delims);
                        if (tokens.Length == 2)
                        {
                            Handle.FileName = tokens[0];
                            Handle.NumFrames = Int32.Parse(tokens[1]);
                        }
                    }
                }
                catch (ThreadAbortException ex)
                {
                }
                catch (ObjectDisposedException ex)
                {
                    Client = new NamedPipeClientStream("kinectmanager");
                    Client.Connect();
                    continue;
                }
                //Catch any other exception that might be thrown.
                catch (Exception ex)
                {
                    MessageBox.Show("Error in KinectManager.Monitor : " +
                        ex.GetType().ToString() + " : " + ex.Message);
                }
            }

            // Send termination signal to window.
            Closing?.Invoke(this, EventArgs.Empty);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Cleans up the Kinect handle and the mailbox.
        /// The Kinect is important because it will take care of the saving of the files.
        /// </summary>
        public void Dispose()
        {
            if (Handle != null)
            {
                Handle.Dispose();
                Handle = null;
            }
            //if (Server != null)
            //{
            //    Server.Dispose();
            //    Server = null;
            //}
            //if (Client != null)
            //{
            //    Client.Dispose();
            //    Client = null;
            //}
            //Server.Dispose();
            //Client.Dispose();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Event for notifying the TestUtility that hand flapping has been deteced.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandflappingDetected(object sender, RecordEventArgs e)
        {
            //using (MailslotClient Client = new MailslotClient("testutility"))
            //{
            //    Client.SendMessage("HandflappingDetected");
            //}
            try
            {
                using (Server)
                {
                    using (StreamWriter ss = new StreamWriter(Server))
                    {
                        ss.WriteLine("HandflappingDetected");
                    }
                }
            }
            catch (ObjectDisposedException ex)
            {
                Server = new NamedPipeServerStream("testutility");
                Server.WaitForConnection();
                using (Server)
                {
                    using (StreamWriter ss = new StreamWriter(Server))
                    {
                        ss.WriteLine("HandflappingDetected");
                    }
                }
            }
        }
    }
}
