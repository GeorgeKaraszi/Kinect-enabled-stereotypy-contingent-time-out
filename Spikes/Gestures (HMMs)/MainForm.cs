// Accord.NET Sample Applications
// http://accord-framework.net
//
// Copyright © 2009-2014, César Souza
// All rights reserved. 3-BSD License:
//
//   Redistribution and use in source and binary forms, with or without
//   modification, are permitted provided that the following conditions are met:
//
//      * Redistributions of source code must retain the above copyright
//        notice, this list of conditions and the following disclaimer.
//
//      * Redistributions in binary form must reproduce the above copyright
//        notice, this list of conditions and the following disclaimer in the
//        documentation and/or other materials provided with the distribution.
//
//      * Neither the name of the Accord.NET Framework authors nor the
//        names of its contributors may be used to endorse or promote products
//        derived from this software without specific prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
//  DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
//  DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
//  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
//  LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//  ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 

using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using Gestures.HMMs.Native;

namespace Gestures.HMMs
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// The Markov Model class handler
        /// </summary>
        private Hmm _hmm;

        /// <summary>
        /// Kinect function and gesture handler.
        /// </summary>
        private KinectHandle _kinectHandle;

        public static bool Recording { get; set; } = false;
        private int _timeleft;
        private int[] _timeintervals = {2000, 2100};
        private Random _randomTime;

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the core components of the application.
        /// </summary>
        public MainForm()
        {
            _randomTime = new Random();
            this._timeleft = 2000;
            InitializeComponent();

            _kinectHandle                   = new KinectHandle(); //Start kinect Gestures
            _hmm                            = new Hmm(); //Start HMM ML Algo
            cbClass.SelectedItem            = cbClass.Items[0].ToString();
            cbWaveType.SelectedItem         = cbWaveType.Items[0].ToString();
            UpdateResultText();

            cbGesture.Items.Clear();
            //Load the kinect gestures name's into the drop down menu
            cbGesture.Items.AddRange(_kinectHandle.GetGestureNames().ToArray());
            //Select the first one in the list to display in the drop down
            cbGesture.SelectedItem = cbGesture.Items[0].ToString();
            //Tell kinect to focus on just that one gesture
            _kinectHandle.SetGesture(cbGesture.Items[0].ToString());
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Resets the GUI to its original form
        /// </summary>
        private void Reset()
        {
            if (chartPattern.Series[0].Points.Count > 0)
                chartPattern.Series[0].Points.Clear();

            if (chartKinectRaw.Series[0].Points.Count > 0)
                chartKinectRaw.Series[0].Points.Clear();

            panelUserLabeling.Visible = false;
            panelClassification.Visible = false;
            lbRecError.Visible = false;
            btnLearnHMM.Enabled = false;
            btnLearnHCRF.Enabled = false;
        }

        //--------------------------------------------------------------------------------
        private void btnLearnHMM_Click(object sender, EventArgs e)
        {
            //HMM.HMM
            _hmm.LearnHmm();
            lbHmmMatch.Text = $"HMM Matched {_hmm.HmmMatches}";
            btnLearnHCRF.Enabled = true;

        }

        //--------------------------------------------------------------------------------
        private void btnLearnHCRF_Click(object sender, EventArgs e)
        {
            _hmm.LearnHcrf();
            lbHcrfMatch.Text = $"HCRF Matched {_hmm.HcrfMatches}";
        }

        //--------------------------------------------------------------------------------
        // Load and save database methods
        private void openDataStripMenuItem_Click(object sender, EventArgs e)
        {
            openDataDialog.ShowDialog();
        }

        //--------------------------------------------------------------------------------
        private void saveDataStripMenuItem_Click(object sender, EventArgs e)
        {
            saveDataDialog.ShowDialog();
        }

        //--------------------------------------------------------------------------------
        private void openDataDialog_FileOk(object sender,
                                           CancelEventArgs e)
        {
            //Open file dialog and load database
            using (var stream = openDataDialog.OpenFile())
            {
                this.Reset();
                _hmm.Clear();
                _hmm.LoadHcrf(_hmm.CLASSIFYDB.Load(stream));
            }

            UpdateResultText();
            //btnCompAuto_Click(sender, e);


        }

        //--------------------------------------------------------------------------------
        private void saveDataDialog_FileOk(object sender, CancelEventArgs e)
        {
            //Save database
            using (var stream = saveDataDialog.OpenFile())
                _hmm.CLASSIFYDB.Save(stream, _hmm.GetHcrf());
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Show the load and save options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFile_MouseDown(object sender, MouseEventArgs e)
        {
            menuFile.Show(button4, button4.PointToClient(Cursor.Position));
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// The user stated the pattern recorded was not detected correctly. So insert 
        /// into the database as the opposite of what was found.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNo_Click(object sender, EventArgs e)
        {
            //NO THIS IS NOT THE GESTURE... Switch the selected label
            foreach (object item in cbClass.Items)
            {
                if (item.ToString() == cbClass.SelectedItem.ToString())
                    continue;

                if (item.ToString() == String.Empty)
                    continue;

                cbClass.SelectedItem = item.ToString();
                break;
            }

            //Submit recorded pattern as the opposite of what was found.
            btnInsert_Click(sender, e);
        }

        //--------------------------------------------------------------------------------
        // Bottom user interaction panel box events
        private void btnClear_Click(object sender, EventArgs e)
        {
            panelUserLabeling.Visible = false;
        }

        //--------------------------------------------------------------------------------
        private void btnInsert_Click(object sender, EventArgs e)
        {
            var dataPoints = chartPattern.Series[0].Points;
            var label = cbClass.SelectedItem.ToString();

            if (_hmm.AddPattern(dataPoints, label) == false)
            {
                MessageBox.Show("Error adding pattern to database");
            }
            else if (_hmm.CanLearn()) //Can we learn now?
            {
                btnLearnHMM.Enabled = true;
            }

            panelClassification.Visible = false;
            panelUserLabeling.Visible = false;
            panelUserLabeling.Refresh();
            panelClassification.Refresh();
        }

        //--------------------------------------------------------------------------------
        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        ///   Paints the background of the control.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Button click event, that handles recording states and fires off the analyzer 
        /// from the recored pattern.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecord_Click(object sender, EventArgs e)
        {
            if (recordTimer.Enabled == false)
            {
                //Clear error text (If there was one)
                lbRecError.Visible = false;
                lbRecError.Refresh();

                //Clear the pattern from last capture (if there was one).
                if (chartPattern.Series[0].Points.Count > 0)
                {
                    chartPattern.Series[0].Points.Clear();
                    chartPattern.Refresh();
                }

                _timeleft = 2000;
                Recording = true;
                recordTimer.Enabled = true;
                recordTimer.Start();
                btnRecord.Text = "Recording...";
                btnRecord.BackColor = Color.Red;
                btnRecord.ForeColor = Color.White;
            }
        }

        /// <summary>
        /// Computes the results of the captured pattern
        /// </summary>
        private void RunAnalysis()
        {
            string label = _hmm.ComputeResults(chartPattern.Series[0].Points);

            if (label == null)
            {
                lbRecError.Visible = true;
                lbRecError.Refresh();
            }
            if (label != String.Empty)
            {
                lbDoesPatternEql.Text = $"Does the pattern match {label}?";
                cbClass.SelectedItem = label;
                panelClassification.Visible = true;

            }
            else
            {
                panelUserLabeling.Visible = true;
            }

        }

        private void cbGesture_SelectedIndexChanged(object sender, EventArgs e)
        {
            _kinectHandle.SetGesture(cbGesture.SelectedItem.ToString());
            lbLoadedGesture.Text = $"Gesture {cbGesture.SelectedItem} loaded";
        }

        private void captureTime_Tick(object sender, EventArgs e)
        {
            if (_timeleft > 0)
            {
                _timeleft -= captureTime.Interval;
            }
            else
            {
                Recording = false;
                captureTime.Stop();
                var dataPoints = chartPattern.Series[0].Points;
                var label = cbWaveType.SelectedItem.ToString();

                if (_hmm.AddPattern(dataPoints, label) == false)
                {
                    MessageBox.Show("Error adding pattern to database");
                }

                chartPattern.Series[0].Points.Clear();
                chartPattern.Refresh();
                _timeleft = _timeintervals[_randomTime.Next(0, _timeintervals.Length)];

                Recording = true;
                captureTime.Start();
            }

            UpdateResultText();
        }

        private void btnautotrain_Click(object sender, EventArgs e)
        {
            chartPattern.Series[0].Points.Clear();
            chartPattern.Refresh();

            if (captureTime.Enabled == false)
            {
                cbWaveType.Enabled     = false;
                captureTime.Enabled    = true;
                Recording              = true;

                btnautotrain.Text      = "Stop Auto Train...";
                btnautotrain.BackColor = Color.Red;
                btnautotrain.ForeColor = Color.White;

                _timeleft = _timeintervals[0];
                captureTime.Start();
            }
            else
            {
                Recording              = false;
                captureTime.Enabled    = false;
                cbWaveType.Enabled     = true;

                captureTime.Stop();

                btnautotrain.Text      = "Start Auto Training";
                btnautotrain.BackColor = Color.FromKnownColor(KnownColor.ControlLight);
                btnautotrain.ForeColor = Color.Black;
            }

        }

        private void UpdateResultText()
        {
            int totalHmmPercent  = 0;
            int totalHcrfPercent = 0;

            if (_hmm.CLASSIFYDB.Count != 0)
            {
                totalHmmPercent  = (_hmm.HmmMatches*100) / _hmm.CLASSIFYDB.Count;
                totalHcrfPercent = (_hmm.HcrfMatches*100) / _hmm.CLASSIFYDB.Count;
            }

            lbHmmMatch.Text      = $"HMM Total Matched {_hmm.HmmMatches} : {totalHmmPercent}%";
            lbHcrfMatch.Text     = $"HCRF Total Matched {_hmm.HcrfMatches} : {totalHcrfPercent}%";

            lbHmmGoodWave.Text   = $"HMM Good wave {_hmm.HmmGoodMatches}";
            lbHcrfGoodWave.Text  = $"HCRF Good wave {_hmm.HcrfGoodMatches}";

            lbHcrfBadWave.Text   = $"HCRF Bad Wave: {_hmm.HcrfBadMatches}";
            lbHmmBadWave.Text    = $"HMM Bad Wave: {_hmm.HmmBadMatches}";

            lbTotalRecs.Text     = $"Total Records: {_hmm.CLASSIFYDB.Count}";
        }

        private void btnCompAuto_Click(object sender, EventArgs e)
        {
            if (_hmm.CanLearn())
            {
                _hmm.LearnHmm();
                _hmm.LearnHcrf();
            }
            else
            {
                MessageBox.Show("We need more samples & classifiers to compute!");
            }

            UpdateResultText();
        }

        private void recordTimer_Tick(object sender, EventArgs e)
        {
            if (_timeleft > 0)
            {
                _timeleft -= recordTimer.Interval;
            }
            else
            {
                recordTimer.Stop();
                Recording = false;
                recordTimer.Enabled = false;

                btnRecord.Text = "Start Recording...";
                btnRecord.BackColor = Color.FromKnownColor(KnownColor.ControlLight);
                btnRecord.ForeColor = Color.Black;
                RunAnalysis();
            }
        }

        private void saveFileWaveDialog_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
