#region

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using WesternMichgian.SeniorDesign.KinectProject.CaptureUtil;

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

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            //Initialize Kinect structure
            _kinectHandle = new KinectHandle();
            _utilWindow   = new UtilWindow(_kinectHandle.GetGestureNames);
            _kinectHandle.OnDataChange        += _kinectHandle_OnDataChange;
            _kinectHandle.OnSkeletonChange    += _kinectHandle_OnSkeletonChange;
            //_kinectHandle.OnPeriodChange      += _kinectHandle_OnPeriodChange;
            _utilWindow.OnGestureTargetChange += _utilWindow_OnGestureTargetChange;
            // initialize the MainWindow
            InitializeComponent();

            // set our data context objects for display in UI
            DataContext = this;
            if (_kinectHandle.KinectBodyView != null)
                kinectBodyViewbox.DataContext = _kinectHandle.KinectBodyView;
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
        /// <summary>
        /// ****Not implemented yet****
        /// Event is triggered every time a period change for target gesture is found.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void _kinectHandle_OnPeriodChange(object source, UtilPeriodArgs e)
        {
            throw new NotImplementedException();
        }

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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // ... Cast sender object.
            Setting frm = new Setting();
            frm.Show();
        
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