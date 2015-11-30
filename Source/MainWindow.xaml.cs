#region

using System;
using System.ComponentModel;

using System.Windows;
using System.Windows.Forms;
using WesternMichgian.SeniorDesign.KinectProject.CaptureUtil;
using WesternMichgian.SeniorDesign.KinectProject.Recording;

#endregion

namespace WesternMichgian.SeniorDesign.KinectProject
{
    /// <summary>
    ///     Interaction logic for the MainWindow
    /// </summary>
    public partial class MainWindow
    {
        private readonly KinectHandle _kinectHandle;
        private readonly UtilWindow _utilWindow;
        private readonly QuietHandsWindow _quietHandsWindow;
        private readonly Settings _windowSettings;
        private int CurrentTime { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            //Initialize Kinect structure

            Timer applicationTimer = new Timer();
            _windowSettings        = new Settings();
            _kinectHandle          = new KinectHandle();
            _utilWindow            = new UtilWindow(_kinectHandle.GetGestureNames);
            _quietHandsWindow      = new QuietHandsWindow();

            //Assign Event triggered functions
            _kinectHandle.OnDataChange        += _kinectHandle_OnDataChange;
            _kinectHandle.OnSkeletonChange    += _kinectHandle_OnSkeletonChange;
            _kinectHandle.OnLimitReach        += _kinectHandle_OnLimitReach;
            _utilWindow.OnGestureTargetChange += _utilWindow_OnGestureTargetChange;
            applicationTimer.Tick             += _applicationTimer_Tick;
            applicationTimer.Enabled = true;
            applicationTimer.Interval = 1;
            applicationTimer.Start();

            // initialize the MainWindow
            InitializeComponent();

            // set our data context objects for display in UI
            DataContext = this;
            if (_kinectHandle.KinectBodyView != null)
                kinectBodyViewbox.DataContext = _kinectHandle.KinectBodyView;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _applicationTimer_Tick(object sender, EventArgs e)
        {
            var timeObj = (Timer) sender;
            if (_kinectHandle.TrackingBodies &&
                 _quietHandsWindow.Visible == false &&
                 _windowSettings.ReturnSucessTimerEnabled)
            {
                CurrentTime += timeObj.Interval;
                if (CurrentTime >= _windowSettings.ReturnSucessTimerInterval)
                {
                    CurrentTime = 0;
                    _quietHandsWindow.PlayGoodSound();
                }
            }
            else
            {
                CurrentTime = 0;
            }

        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Event triggered when ever the utility window wants to target a different 
        /// gesture on a specified body
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void _utilWindow_OnGestureTargetChange(object source, UtilTargetGestArgs e)
        {
            _kinectHandle.SetTargetGesture(e.GetBodyId(), e.GetGestureName());
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Event triggered anytime, data frames of targeted gesture of triggered
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void _kinectHandle_OnDataChange(object source, UtilDataArgs e)
        {
            _utilWindow.UpdateData(e.GetBodyId(), e.GetFrameValue());
        }

        //--------------------------------------------------------------------------------
//        /// <summary>
//        /// ****Not implemented yet****
//        /// Event is triggered every time a period change for target gesture is found.
//        /// </summary>
//        /// <param name="source"></param>
//        /// <param name="e"></param>
//        private void _kinectHandle_OnPeriodChange(object source, UtilPeriodArgs e)
//        {
//            throw new NotImplementedException();
//        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Event is triggered anytime a skeleton status of being tracked is updated
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void _kinectHandle_OnSkeletonChange(object source, UtilBodyArgs e)
        {
            _utilWindow.UpdateTracking(e.GetBodyId(),e.IsBeingTracked());
        }


        //--------------------------------------------------------------------------------
        /// <summary>
        /// Handles the event that occurs when sufficient hand-flapping is detected.
        /// </summary>
        /// <param name="source">Instance of recording table class</param>
        /// <param name="e">Name of recorded gesture</param>
        private void _kinectHandle_OnLimitReach(object source, RecordEventArgs e)
        {
            string message = "Gesture : " + e.GetInfo();
            if (_quietHandsWindow.Visible == false)
            {
                if (_windowSettings.ReturnFullScreenEnabled)
                {
                    _quietHandsWindow.DisplayFullScreen(_windowSettings.TimeoutInterval);
                }
                else
                {

                    CurrentTime = 0;
                    System.Windows.Forms.MessageBox.Show(message);
                    _quietHandsWindow.PlayBadSound();
                }  
            }
        }


        //--------------------------------------------------------------------------------
        /// <summary>
        /// Opens the settings dialog menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            // ... Cast sender object.
            if (_windowSettings.Visible == false)
            {
                _windowSettings.Show();
            }

        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Event triggered when the utility button from the menu is selected.
        /// This will display the utility menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Util_Click(object sender, RoutedEventArgs e)
        {
            _utilWindow.Show();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        ///     Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            _kinectHandle.Dispose();
        }
    }
}