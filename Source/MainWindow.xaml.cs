#region

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace WesternMichgian.SeniorDesign.KinectProject
{
    /// <summary>
    ///     Interaction logic for the MainWindow
    /// </summary>
    public partial class MainWindow
    {
        private readonly KinectHandle _kinectHandle;

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            //Initialize Kinect structure
            _kinectHandle = new KinectHandle();
            // initialize the MainWindow
            InitializeComponent();

            // set our data context objects for display in UI
            DataContext = this;
            if (_kinectHandle.KinectBodyView != null)
                kinectBodyViewbox.DataContext = _kinectHandle.KinectBodyView;
        }



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            // ... Cast sender object.
            MenuItem item = sender as MenuItem;
            Setting frm = new Setting();
            frm.Show();
        
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