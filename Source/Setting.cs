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
    public partial class Setting : Form
    {
        
     
        FormSetting data;
        bool state = true;
        public Setting()
        {
            InitializeComponent();

            state = Properties.Settings.Default.TimerOrWindow;
            if (!state)
            {
                windowRadioB.Checked = true;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.FixedSingle;

            GroupBox groupBox1 = new GroupBox();
            RadioButton timerRadioB= new RadioButton();
            RadioButton windowRadioB = new RadioButton();
            TextBox timerTb = new TextBox();

    
            groupBox1.Controls.Add(timerRadioB);
            groupBox1.Controls.Add(windowRadioB);

      

        }


        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["TimerOrWindow"] = true;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file
            data = new FormSetting(true);
            timerTb.ReadOnly = false;

        }


        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default["TimerOrWindow"] = false;
            Properties.Settings.Default.Save(); // Saves settings in application configuration file
            data = new FormSetting(false);
            timerTb.ReadOnly = true;
        }


        private void timerTb_TextChanged(object sender, EventArgs e)
        {
            try {
                
                data = new FormSetting(Convert.ToInt32(timerTb.Text));
            }
            catch(Exception ex)
            {

                if (timerTb.Text != "" && timerTb != null)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter the timer limit in minutes");
                    timerTb.Text = "";
                }
               
            }


     
        }


    }
}
