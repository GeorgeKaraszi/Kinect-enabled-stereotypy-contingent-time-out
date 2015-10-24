using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using CaptureUtil.Algorithms;
using CaptureUtil.Properties;

namespace CaptureUtil
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Kinect function and gesture handler.
        /// </summary>
        private readonly KinectHandle _kinectHandle;

        /// <summary>
        /// Global timer for handling recording time.
        /// </summary>
        private int _recordingTimer;

        /// <summary>
        /// Signal flag to tell the confidence capture to gather more values.
        /// </summary>
        public static bool Recording;

        /// <summary>
        /// Flag to which algorithm is used to analyze the captured wave.
        /// </summary>
        private int _algorithmSelected;

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Main GUI and Kinect initializer.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            _kinectHandle = new KinectHandle();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Main loading section when the application starts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //Set default timer to two seconds.
            udRecTime.Value = 2;

            cbGestureTarget.Items.Clear();
            //Load the kinect gestures name's into the drop down menu
            if (_kinectHandle != null)
            {
                // ReSharper disable once CoVariantArrayConversion
                cbGestureTarget.Items.AddRange(_kinectHandle.GetGestureNames().ToArray());
                //Select the first one in the list to display in the drop down
                cbGestureTarget.SelectedItem = cbGestureTarget.Items[0].ToString();
                //Tell Kinect to focus on just that one gesture
                _kinectHandle.SetGesture(cbGestureTarget.SelectedItem.ToString());
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Simple About menu item that displays simplified information about the program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.about_info_body, Resources.about_info_title);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Every time the record timer changes on the GUI, store the results in 
        /// milliseconds to the recording timer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void udRecTime_ValueChanged(object sender, EventArgs e)
        {
            if (udRecTime.Value != 0)
                _recordingTimer = Convert.ToInt32(udRecTime.Value*1000);
            else
                _recordingTimer = -1;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Start the recording process of the application when the GUI record button is 
        /// clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (timerRecord.Enabled == false)
            {
                //Disable Algorithm option.
                panelAlgorithm.Visible = false;

                //Enable recording and start the timer
                Recording = true;
                timerRecord.Enabled = true;
                timerRecord.Start();

                //Clear chart before recording starts
                chartRecording.Series[0].Points.Clear();
                chartRecording.Series[1].Points.Clear();

                //Change the button display
                btnRecord.BackColor = Color.Red;
                btnRecord.Text = Resources.stop_recording;
                lbCapDis.Text = Resources.capture_data;
            }
            else
            {
                //Enable the algorithm for captured data
                panelAlgorithm.Visible = true;

                //Disable all recording and timing
                Recording = false;
                timerRecord.Enabled = false;
                timerRecord.Stop();

                //Change the button to display data
                btnRecord.BackColor = DefaultBackColor;
                btnRecord.Text = Resources.start_recording;

                //Open save dialog to save the recorded points
                //saveFileDialog.ShowDialog();

                udRecTime_ValueChanged(sender, e);
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Every time a new gesture is selected from the available gesture list displayed 
        /// on the GUI. Change the selected gesture in the kinect.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbGestureTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Change the gesture the recorder is listening  to.
            _kinectHandle.SetGesture(cbGestureTarget.SelectedItem.ToString());
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Top menu that opens the open file dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripOpen_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Top menu that opens the save file dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripSave_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// The core timer that is responsible for timing data capturing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerRecord_Tick(object sender, EventArgs e)
        {
            if (_recordingTimer > 0)
            {
                _recordingTimer -= timerRecord.Interval;
            }
            else if (_recordingTimer != -1)
            {
                btnRecord_Click(sender,e);
            }
        }

      

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Check for a change in the algorithm and run the Analyst when the run button 
        /// is clicked on.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAlgorRun_Click(object sender, EventArgs e)
        {
            //Check to see if one of the algorithms where checked.
            if (rbPV.Checked)
            {
                _algorithmSelected = 1;
            }
            else if (rbHillBuild.Checked)
            {
                _algorithmSelected = 2;
            }
            else
            {
                _algorithmSelected = 0;
            }

            //Gather the wave data points
            var wave = chartRecording.Series[0].Points
                                .Select(point => point.YValues[0]).ToList();

            //Run the analyst
            RunAnalyst(wave);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Runs the given wave through a selected algorithm to produce a analysis.
        /// </summary>
        /// <param name="wave"></param>
        private void RunAnalyst(List<double> wave)
        {
            chartRecording.Series[1].Points.Clear();

            switch (_algorithmSelected)
            {
                case 1:
                    var pv = new PeaksAndValleys().FindPeaksAndValleys(wave);

                    foreach (Tuple<int, double> p in pv)
                    {
                        chartRecording.Series[1].Points.AddXY(p.Item1, p.Item2);
                    }
                    //Do something with peaks and valleys here.
                    break;
                case 2:
                    //Do something with hill building here
                    break;
                default:
                    break;
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Load a chart that was saved from an earlier time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            //Get the extension of the given file
            string extentionType = Path.GetExtension(openFileDialog.FileName);

            var pv = new List<Tuple<int, double>>();
            var datapoints = new List<double>();

            //Open the selected file
            using (var stream = openFileDialog.OpenFile())
            {
                //Check to see if file type is text or otherwise binary
                if (extentionType == ".txt")
                {
                    string lineread = String.Empty;

                    using (var sr = new StreamReader(stream))
                    {
                        _algorithmSelected = Convert.ToInt32(sr.ReadLine());
                        if (sr.ReadLine() == "dp start")
                        {

                            while ((lineread = sr.ReadLine()) != "dp end")
                            {
                                datapoints.Add(Convert.ToDouble(lineread));
                            }
                        }

                        if (sr.ReadLine() == "pv start")
                        {
                            while ((lineread = sr.ReadLine()) != "pv end")
                            {
                                var split = lineread.Split(':');
                                var tmp =
                                    new Tuple<int, double>(Convert.ToInt32(split[0]),
                                                           Convert.ToDouble(split[1]));
                                pv.Add(tmp);
                            }
                        }
                    }
                }
                else
                {
                    //Load the Peak's and valley, and base data points to the wave
                    _algorithmSelected = (int)new BinaryFormatter().Deserialize(stream);
                    datapoints = (List<double>)new BinaryFormatter().Deserialize(stream);
                    pv =
                        (List<Tuple<int, double>>)
                            new BinaryFormatter().Deserialize(stream);
                }
            }

            //Clear all points from the chart
            chartRecording.Series[0].Points.Clear();
            chartRecording.Series[1].Points.Clear();

            //Load the data points into the chart to display
            for (int i = 0; i < datapoints.Count; i++)
            {
                chartRecording.Series[0].Points.AddXY(i, datapoints[i]);
            }

            foreach (var point in pv)
            {
                chartRecording.Series[1].Points.AddXY(point.Item1, point.Item2);
            }

            lbCapDis.Text = Resources.load_and_display_data;
            panelAlgorithm.Visible = true;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Save the results from the recording chart display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            //Get the saving data type
            string extentionType = Path.GetExtension(saveFileDialog.FileName);

            var series0Points = chartRecording.Series[0].Points;
            var series1Points = chartRecording.Series[1].Points;

            //Gather all points from the recored chart
            var datapoints = series0Points.Select(point => point.YValues[0]).ToList();
            var pv = series1Points.Select(point =>
                new Tuple<int, double>((int)point.XValue, point.YValues[0])).ToList();

            using (var stream = saveFileDialog.OpenFile())
            {
                //Save data in a text file format
                if (extentionType == ".txt")
                {
                    using (var sw = new StreamWriter(stream))
                    {
                        sw.WriteLine("{0}\ndp start", _algorithmSelected);
                        foreach (var point in datapoints)
                        {
                            sw.WriteLine("{0}", point);
                        }
                        sw.WriteLine("dp end\npv start");
                        foreach (var point in pv)
                        {
                            sw.WriteLine("{0}:{1}", point.Item1, point.Item2);
                        }
                        sw.WriteLine("pv end");
                    }
                }
                else //Otherwise save it as binary, more accurate format
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(stream, _algorithmSelected);
                    bf.Serialize(stream, datapoints);
                    bf.Serialize(stream, pv);
                }
            }
        }
    }
}
