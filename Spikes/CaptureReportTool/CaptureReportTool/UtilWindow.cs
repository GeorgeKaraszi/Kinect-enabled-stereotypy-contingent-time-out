using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CaptureReportTool
{
    public partial class UtilWindow : Form
    {
        public event ChangeInTargetGestureEvent OnGestureTargetChange;

        /// <summary>
        /// List of available gestures that can be targeted
        /// </summary>
        private string[] GestureList { get; set; }
        /// <summary>
        /// List of panels that can be displayed for tracked bodies
        /// </summary>
        private List<PanelFrame> ActivePanel { get; set; }
        /// <summary>
        /// List of recording tables of tracked bodies
        /// </summary>
        private Hashtable HashTblRecord { get; set; }
        /// <summary>
        /// Window size of the displayed graph
        /// </summary>
        private int WindowSize { get; } = 100;

        //--------------------------------------------------------------------------------
        public UtilWindow()
        {
            InitializeComponent();
            SetupPanels();
        }

        public UtilWindow(string[] gestureNames)
        {
            InitializeComponent();
            GestureList = gestureNames;
            SetupPanels();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the list of tracked panels being displayed.
        /// </summary>
        private void SetupPanels()
        {
            HashTblRecord = new Hashtable();
            ActivePanel = new List<PanelFrame>
                          {
                              new PanelFrame(false,false, -1, panel1),
                              new PanelFrame(false,false, -1, panel2),
                              new PanelFrame(false,false, -1, panel3),
                              new PanelFrame(false,false, -1, panel4),
                              new PanelFrame(false,false, -1, panel5),
                              new PanelFrame(false,false, -1, panel6)
                          };
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Updates the GUI with a change in tracking of a body
        /// </summary>
        /// <param name="bodyId">Body of the new tracking event</param>
        /// <param name="trackingStatus">Status of the body in question</param>
        public void UpdateTracking(int bodyId, bool trackingStatus)
        {
            var targetPanel = ActivePanel.FirstOrDefault(x => x.BodyId == bodyId);

            if (targetPanel == null)
            {
                var panel = ActivePanel.FirstOrDefault(x => x.IsActive == false);
                if (panel != null)
                {
                    panel.IsActive = true;
                    panel.IsRecording = false;
                    panel.BodyId = bodyId;
                    UpdatePanelDefaults(panel.Panel, trackingStatus);
                }
            }
            else if (trackingStatus == false)
            {
                targetPanel.IsActive = false;
                targetPanel.IsRecording = false;
                targetPanel.BodyId = -1;
                targetPanel.Panel.Visible = false;

                if (HashTblRecord.Contains(bodyId))
                    HashTblRecord.Remove(bodyId);

                RearrangePanels();
            }

        }


        //--------------------------------------------------------------------------------
        /// <summary>
        /// Records inserted data if the target skeleton is marked for recording
        /// </summary>
        /// <param name="bodyid">ID of targeted skeleton</param>
        /// <param name="frame">
        /// Tuple containing frame number and value for that frame
        /// </param>
        public void AddWaveData(int bodyid, Tuple<int, double> frame)
        {
            var trackedBody = ActivePanel.FirstOrDefault(x => x.BodyId == bodyid);

            if (trackedBody == null)
                return;

            //If Body is flagged for recording but has no table assigned
            if (trackedBody.IsRecording &&
                HashTblRecord.Contains(trackedBody.BodyId) == false)
            {
                HashTblRecord.Add(trackedBody.BodyId, new List<Tuple<int, double>>());
            }

            if (trackedBody.IsRecording)
            {
                //Body is flagged for recording and data will be added
                var frameList =
                    (List<Tuple<int, double>>)HashTblRecord[trackedBody.BodyId];
                frameList.Add(frame);
            }

            //Insert data into the visible GUI chart
            UpdateChartFrame(trackedBody.Panel, frame);

        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// In the case of a skeleton is no longer tracked between two active one's. 
        /// The GUI must rearrange the GUI in order to prevent gaps.
        /// 
        /// Due to the nature of the application. A simple swap of panels would cause 
        /// error's and a higher level of complexity since each panel needs to be 
        /// tracked individually.
        /// 
        /// This function does a 1 time pass through that swaps all data from an active to 
        /// an inactive panel that is located above.
        /// </summary>
        private void RearrangePanels()
        {
            PanelFrame above;
            PanelFrame below;
            Chart chartTop = null;
            Chart chartBtm = null;
            for (int i = 0; ( i + 1 ) < ActivePanel.Count; i++)
            {
                above = ActivePanel[i];
                below = ActivePanel[i + 1];

                if (above.IsActive == false && below.IsActive)
                {
                    above.IsActive      = below.IsActive;
                    above.IsRecording   = below.IsRecording;
                    above.BodyId        = below.BodyId;
                    above.Panel.Visible = below.Panel.Visible;
                    below.Panel.Visible = false;
                    below.IsActive      = false;
                    below.BodyId        = -1;
                    var topPanel        = above.Panel.Controls;
                    var btmPanel        = below.Panel.Controls;

                    //Obtain chart object for top panel
                    foreach (var con in topPanel.Cast<object>()
                                                .Where( con =>
                                                        con.GetType() == typeof (Chart)))
                    {
                        chartTop = (Chart) con;
                    }

                    //Obtain chart object for bottom panel
                    foreach (var con in btmPanel.Cast<object>()
                                                .Where( con =>
                                                        con.GetType() == typeof (Chart)))
                    {
                        chartBtm = (Chart) con;
                    }

                    //Make sure these values are not null
                    if (chartTop == null || chartBtm == null)
                        continue;

                    //Swap chart display's
                    chartTop.Series[0].Points.Clear();
                    foreach (var point in chartBtm.Series[0].Points)
                    {
                        chartTop.Series[0].Points.Add(point);
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Inserts frames of data into the viable chart on the GUI
        /// </summary>
        /// <param name="panel">Control panel</param>
        /// <param name="frame">Tuple containing data for the frame and it's value</param>
        private void UpdateChartFrame(Panel panel, Tuple<int, double> frame)
        {
            //Loop through each control till the chart object comes up
            foreach (var con in panel.Controls)
            {
                if (con.GetType() == typeof (Chart))
                {
                    ( (Chart) con ).Series[0].Points.AddXY(frame.Item1%WindowSize, 
                                                           frame.Item2);
                    ( (Chart) con ).Refresh();
                }
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Sets the defaults that the panels are initialized to
        /// </summary>
        /// <param name="targetPanel">Panel that is going to be modified</param>
        /// <param name="showStatus">Status of it's visibility</param>
        private void UpdatePanelDefaults(Panel targetPanel, bool showStatus)
        {
            foreach (Control p in targetPanel.Controls)
            {
                if (p.GetType() == typeof (Chart))
                {
                    var tmpChart = (Chart) p;
                    tmpChart.Series[0].Points.Clear();
                }
                else if (p.GetType() == typeof (ComboBox))
                {
                    SetComboBoxDefualts((ComboBox)p);
                }
                else if (p.GetType() == typeof (Button))
                {
                    ( (Button) p ).Text = @"Record";
                    ( (Button) p ).BackColor = DefaultBackColor;
                }
            }

            targetPanel.Visible = showStatus;
        }


        //--------------------------------------------------------------------------------
        /// <summary>
        /// Set's the default field's of the combo box
        /// </summary>
        /// <param name="combo"></param>
        private void SetComboBoxDefualts(ComboBox combo)
        {
            combo.Items.Clear();
            // ReSharper disable once CoVariantArrayConversion
            combo.Items.AddRange(GestureList);
            combo.SelectedItem = combo.Items[0].ToString();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Obtains the panel in which an object belongs to
        /// </summary>
        /// <param name="panel"></param>
        /// <returns>Tuple pair containing owner panel</returns>
        private PanelFrame GetPanelFrame(Control panel)
        {
            if (panel == null)
                throw new ArgumentNullException(nameof(panel));

            return ActivePanel.FirstOrDefault(x => x.Panel == panel);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Event triggered when the record button on the panel is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recordBtn_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button) sender;
            PanelFrame activePanel = GetPanelFrame(clickedButton.Parent);

            if (activePanel == null)
                return;

            //Get the inverse of the recording type
            activePanel.IsRecording = !activePanel.IsRecording;
            if (activePanel.IsRecording)
            {
                activePanel.StartTime   = DateTime.Now;
                clickedButton.Text      = @"Stop Recording";
                clickedButton.BackColor = Color.Red;
            }
            else
            {
                activePanel.EndTime     = DateTime.Now;
                clickedButton.Text      = @"Record";
                clickedButton.BackColor = DefaultBackColor;
                SaveRecordedValues(activePanel);
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Saves the recordings to a file designated by the user
        /// </summary>
        /// <param name="panelFrame">Selected frame to save recordings</param>
        private void SaveRecordedValues(PanelFrame panelFrame)
        {
            if (HashTblRecord.Contains(panelFrame.BodyId))
            {
                var frameSet =
                    (List<Tuple<int, double>>) HashTblRecord[panelFrame.BodyId];

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                using (var sw = new StreamWriter(saveFileDialog.FileName))
                {
                    sw.Write("Start Time:{0}\tEnd Time{1}\n", 
                             panelFrame.StartTime, panelFrame.EndTime);
                    foreach (var points in frameSet)
                    {
                        sw.Write("Frame {0}\tValue {1}\n", points.Item1, points.Item2);
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Event triggered when a gesture target is changed. Which will cause an event 
        /// on tracking watch gesture is processed on the main program.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cb_SelectedGestureChange(object sender, EventArgs e)
        {
            ComboBox clickedBox = (ComboBox) sender;
            var activePanel = GetPanelFrame(clickedBox.Parent);

            //Trigger event that effects main program here
            var eventArg = new UtilTargetGestArgs(activePanel.BodyId,
                                                  clickedBox.SelectedItem.ToString());

            OnGestureTargetChange?.Invoke(sender, eventArg);
        }
    }

    //====================================================================================
    /// <summary>
    /// Container class for keeping track of what panel belongs to what body
    /// </summary>
    public class PanelFrame
    {
        /// <summary>
        /// Is the panel active?
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Is the active panel recording data?
        /// </summary>
        public bool IsRecording { get; set; }
        /// <summary>
        /// Body index id for the recorded panel
        /// </summary>
        public int BodyId { get; set; }
        /// <summary>
        /// Start time of when the recording started
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// End time of when the recording stopped
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// Panel that contains all necessary visual GUI controls
        /// </summary>
        public Panel Panel { get; }
        public PanelFrame(bool isActive, bool isRecording, int bodyId, Panel panel)
        {
            IsActive    = isActive;
            IsRecording = isRecording;
            BodyId      = bodyId;
            Panel       = panel;
        }
    }
}
