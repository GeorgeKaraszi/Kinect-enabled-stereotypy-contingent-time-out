using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using CaptureUtil.Algorithms;

namespace CaptureUtil
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// Kinect function and gesture handler.
        /// </summary>
        private KinectHandle _kinectHandle;

        /// <summary>
        /// Global timer for handling recording time.
        /// </summary>
        private int _recordingTimer;

        /// <summary>
        /// Signal flag to tell the confidence capture to gather more values.
        /// </summary>
        public static bool Recording = false;

        /// <summary>
        /// Flag to which algorithm is used to analyze the captured wave.
        /// </summary>
        private int _AlgorithmSelected;

        public MainForm()
        {
            InitializeComponent();

            _kinectHandle = new KinectHandle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //Set default timer to two seconds.
            udRecTime.Value = 2;

            cbGestureTarget.Items.Clear();
            //Load the kinect gestures name's into the drop down menu
            cbGestureTarget.Items.AddRange(_kinectHandle.GetGestureNames().ToArray());
            //Select the first one in the list to display in the drop down
            cbGestureTarget.SelectedItem = cbGestureTarget.Items[0].ToString();
            //Tell Kinect to focus on just that one gesture
            _kinectHandle.SetGesture(cbGestureTarget.SelectedItem.ToString());

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void udRecTime_ValueChanged(object sender, EventArgs e)
        {
            if (udRecTime.Value != 0)
                this._recordingTimer = Convert.ToInt32(udRecTime.Value*1000);
            else
                this._recordingTimer = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (timerRecord.Enabled == false)
            {
                //Enable recording and start the timer
                Recording = true;
                timerRecord.Enabled = true;
                timerRecord.Start();

                //Clear chart before recording starts
                chartRecording.Series[0].Points.Clear();

                //Change the button display
                btnRecord.BackColor = Color.Red;
                btnRecord.Text = "Stop Recording...";
                lbCapDis.Text = "Captured Data";
            }
            else
            {
                //Disable all recording and timing
                Recording = false;
                timerRecord.Enabled = false;
                timerRecord.Stop();

                //Change the button to display data
                btnRecord.BackColor = DefaultBackColor;
                btnRecord.Text = "Start Recording...";

                //Open save dialog to save the recorded points
                saveFileDialog.ShowDialog();

                udRecTime_ValueChanged(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbGestureTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Change the gesture the recorder is listening  to.
            _kinectHandle.SetGesture(cbGestureTarget.SelectedItem.ToString());
        }

        /// <summary>
        /// Top menu that opens the open file dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripOpen_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
        }

        /// <summary>
        /// Top menu that opens the save file dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripSave_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog();
        }

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

        /// <summary>
        /// Load a chart that was saved from an earlier time.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var pv = new List<Tuple<int, double>>();
            var datapoints = new List<double>();

            //Open the selected file
            using (var stream = openFileDialog.OpenFile())
            {
                //Load the Peak's and valley, and base data points to the wave
                _AlgorithmSelected = (int) new BinaryFormatter().Deserialize(stream);
                pv = (List<Tuple<int, double>>) new BinaryFormatter().Deserialize(stream);
                datapoints = (List<double>) new BinaryFormatter().Deserialize(stream);
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

            lbCapDis.Text = "Loaded And Displayed Data";
        }

        /// <summary>
        /// Save the results from the recording chart display
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            var pv = new List<Tuple<int, double>>();
            var datapoints = new List<double>();

            var series = chartRecording.Series[0].Points;

            foreach (var point in chartRecording.Series[0].Points)
            {
                datapoints.Add(point.YValues[0]);
            }

            using (var stream = saveFileDialog.OpenFile())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(stream,_AlgorithmSelected);
                bf.Serialize(stream, pv);
                bf.Serialize(stream, datapoints);
            }
        }

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
                _AlgorithmSelected = 1;
            }
            else if (rbHillBuild.Checked)
            {
                _AlgorithmSelected = 2;
            }
            else
            {
                _AlgorithmSelected = 0;
            }

            //Gather the wave datapoints
            var wave = new List<double>();
            foreach (var point in chartRecording.Series[0].Points)
            {
                wave.Add(point.YValues[0]);
            }

            //Run the analyst
            RunAnalyst(wave);
        }

        /// <summary>
        /// Runs the given wave through a selected algorithm to produce a analysis.
        /// </summary>
        /// <param name="wave"></param>
        private void RunAnalyst(List<double> wave)
        {
            chartRecording.Series[1].Points.Clear();

            switch (_AlgorithmSelected)
            {
                case 1:
                    var pv = new PeaksAndValleys().FindPeaksAndValleys(wave);

                    foreach (var p in pv)
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

            //chartRecording.Series[1].LabelFormat = "{0:0,}K";
        }
    }
}
