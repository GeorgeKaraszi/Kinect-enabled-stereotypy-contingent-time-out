using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WesternMichgian.SeniorDesign.KinectProject
{
    public partial class Settings : Form
    {
        public bool ReturnSucessTimerEnabled { get; set; } = false;
        public int ReturnSucessTimerInterval { get; set; } = 500;
        public bool ReturnFullScreenEnabled { get; set; } = false;
        public int TimeoutInterval { get; set; } = 50;

        public Settings()
        {
            InitializeComponent();
        }
        private void sucessTimeChk_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox tmpBox = (CheckBox) sender;
            ReturnSucessTimerEnabled = tmpBox.Checked;
            successtimer_number.Enabled = tmpBox.Checked;
        }

        private void successtimer_number_ValueChanged(object sender, EventArgs e)
        {
            var tmpNumber = (NumericUpDown) sender;
            ReturnSucessTimerInterval = Convert.ToInt32(tmpNumber.Value) * 100;
        }

        private void fullscreen_ckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox tmpBox = (CheckBox)sender;
            ReturnFullScreenEnabled = tmpBox.Checked;
            fullscren_timernumber.Enabled = tmpBox.Checked;
        }

        private void fullscren_timernumber_ValueChanged(object sender, EventArgs e)
        {
            var tmpNumber = (NumericUpDown)sender;
            TimeoutInterval = Convert.ToInt32(tmpNumber.Value) * 10;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
