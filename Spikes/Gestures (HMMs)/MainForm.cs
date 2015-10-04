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

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the core components of the application.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            _kinectHandle                   = new KinectHandle();  //Start kinect Gestures
            _hmm                            = new Hmm();           //Start HMM ML Algo
            gridSamples.AutoGenerateColumns = false;
            gridSamples.DataSource          = _hmm.CLASSIFYDB.Samples;
            cbClass.SelectedItem            = cbClass.Items[0].ToString();

            openDataDialog.InitialDirectory =
                Path.Combine(Application.StartupPath, "Resources");
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Resets the GUI to its original form
        /// </summary>
        private void Reset()
        {
            if(chartPattern.Series[0].Points.Count > 0)
                chartPattern.Series[0].Points.Clear();

            if (chartKinectRaw.Series[0].Points.Count > 0)
                chartKinectRaw.Series[0].Points.Clear();

            panelUserLabeling.Visible   = false;
            panelClassification.Visible = false;
            lbRecError.Visible          = false;
            btnLearnHMM.Enabled         = false;
            btnLearnHCRF.Enabled        = false;
        }

        //--------------------------------------------------------------------------------
        private void btnLearnHMM_Click(object sender, EventArgs e)
        {
           //HMM.HMM
            _hmm.LearnHmm();
            btnLearnHCRF.Enabled = true;

        }
        //--------------------------------------------------------------------------------
        private void btnLearnHCRF_Click(object sender, EventArgs e)
        {
            _hmm.LearnHcrf();
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
                _hmm.CLASSIFYDB.Load(stream);
            }

            if (_hmm.CanLearn())
            {
                _hmm.LearnHmm();
                _hmm.LearnHcrf();
                btnLearnHCRF.Enabled = true;
            }


        }
        //--------------------------------------------------------------------------------
        private void saveDataDialog_FileOk(object sender, CancelEventArgs e)
        {
            //Save database
            using (var stream = saveDataDialog.OpenFile())
                _hmm.CLASSIFYDB.Save(stream);
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
            btnInsert_Click(sender,e);
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
            var label      = cbClass.SelectedItem.ToString();

            if (_hmm.AddPattern(dataPoints, label) == false)
            {
                MessageBox.Show("Error adding pattern to database");
            }
            else if(_hmm.CanLearn()) //Can we learn now?
            {
                btnLearnHMM.Enabled = true;
            }

            panelClassification.Visible = false;
            panelUserLabeling.Visible   = false;
            panelUserLabeling.Refresh();
            panelClassification.Refresh();
        }

        //--------------------------------------------------------------------------------
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Perform special processing to enable aero
            if (SafeNativeMethods.IsAeroEnabled)
            {
                ThemeMargins margins = new ThemeMargins();
                margins.TopHeight    = panel.Top;
                margins.LeftWidth    = panel.Left;
                margins.RightWidth   = ClientRectangle.Right - gridSamples.Right;
                margins.BottomHeight = ClientRectangle.Bottom - panel.Bottom;

                // Extend the Frame into client area
                SafeNativeMethods.ExtendAeroGlassIntoClientArea(this, margins);
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        ///   Paints the background of the control.
        /// </summary>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (SafeNativeMethods.IsAeroEnabled)
            {
                // paint background black to enable include glass regions
                e.Graphics.Clear(Color.FromArgb(0, this.BackColor));
            }
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
            if (Recording == false)
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

                Recording          = true;
                btnRecord.Text     = "Stop Recording...";
                btnRecord.BackColor = Color.Red;
                btnRecord.ForeColor = Color.White;
            }
            else
            {
                Recording          = false;
                btnRecord.Text     = "Start Recording...";
                btnRecord.BackColor = Color.FromKnownColor(KnownColor.ControlLight);
                btnRecord.ForeColor = Color.Black;

                RunAnalysis();
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
                lbDoesPatternEql.Text       = $"Does the pattern match {label}?";
                cbClass.SelectedItem        = label;
                panelClassification.Visible = true;

            }
            else
            {
                panelUserLabeling.Visible = true;
            }

        }
    }
}
