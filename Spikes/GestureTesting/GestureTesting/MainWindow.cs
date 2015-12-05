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
        // Thread to receive messages from the TestUtility.
        private Thread ManagerThread;
        private KinectManager KinectManager;

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initialize window and KinectManager.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            KinectManager = new KinectManager();
            // If given the word to exit from the TestUtility.
            KinectManager.Closing += _Closing;
            // Thread to run the message receiver of the TestUtility.
            ManagerThread = new Thread(KinectManager.Monitor);
            ManagerThread.Start();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// If somebody closes the window, the final thread must go with it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowClosing(object sender, FormClosingEventArgs e)
        {
            ManagerThread.Abort();
            KinectManager.Closing -= _Closing;
            KinectManager.Dispose();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initialize the window minimized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormLoad(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// The signal from the TestUtility window to tear down the KinectManager.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _Closing(object sender, EventArgs e)
        {
            //ManagerThread.Join();
            //ManagerThread.Abort();
            KinectManager.Closing -= _Closing;
            KinectManager.Dispose();
            // This is what takes this window down with the TestUtility.
            Environment.Exit(0);
        }
    }
}
