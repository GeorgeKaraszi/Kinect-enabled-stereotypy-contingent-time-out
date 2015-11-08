using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuietHands
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        private System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
        private QuietHandsWindow win;
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            //gets all the processes ids
            //mutes them all
            //unmutes the process id of the current program
            //after the windows is shown, unmutes the rest of the process ids
            win = new QuietHandsWindow();
            Process[] processlist = Process.GetProcesses();

            foreach (Process theprocess in processlist)
            {
                try
                {
                    VolumeMixer.SetApplicationMute(theprocess.Id, true);
                }
                catch (Exception)
                {

                }
            }

            int nProcessID = Process.GetCurrentProcess().Id;

            VolumeMixer.SetApplicationMute(nProcessID, false);

            win.ShowDialog();

            foreach (Process theprocess in processlist)
            {
                try
                {
                    VolumeMixer.SetApplicationMute(theprocess.Id, false);
                }
                catch (Exception)
                {

                }
            }
        }

 
    }
}
