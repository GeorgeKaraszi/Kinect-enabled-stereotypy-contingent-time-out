#region

using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Kinect;

#endregion

namespace WesternMichgian.SeniorDesign.KinectProject
{
    /// <summary>
    ///     Interaction logic for the MainWindow
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        /// <summary> KinectBodyView object which handles drawing the Kinect bodies to a View box in the UI </summary>
        private readonly KinectBodyView _kinectBodyView;

        /// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
        private Body[] _bodies;

        /// <summary> Reader for body frames </summary>
        private BodyFrameReader _bodyFrameReader;

        /// <summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
        private List<GestureDetector> _gestureDetectorList;

        /// <summary> Active Kinect sensor </summary>
        private KinectSensor _kinectSensor;

        /// <summary> Current status text to display </summary>
        private string _statusText;

        /// <summary>
        ///     Gets or sets the current status text to display
        /// </summary>
        public string StatusText
        {
            get { return _statusText; }

            set
            {
                if (_statusText != value)
                {
                    _statusText = value;

                    // notify any bound elements that the text has changed
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            // only one sensor is currently supported
            _kinectSensor = KinectSensor.GetDefault();

            // set IsAvailableChanged event notifier
            _kinectSensor.IsAvailableChanged += Sensor_IsAvailableChanged;

            // open the sensor
            _kinectSensor.Open();

            // set the status text
            StatusText = "RUnning";

            // open the reader for the body frames
            _bodyFrameReader = _kinectSensor.BodyFrameSource.OpenReader();

            // set the BodyFramedArrived event notifier
            _bodyFrameReader.FrameArrived += Reader_BodyFrameArrived;

            // initialize the BodyViewer object for displaying tracked bodies in the UI
            _kinectBodyView = new KinectBodyView(_kinectSensor);

            // initialize the gesture detection objects for our gestures
            _gestureDetectorList = new List<GestureDetector>();

            // initialize the MainWindow
            InitializeComponent();

            // set our data context objects for display in UI
            DataContext = this;
            kinectBodyViewbox.DataContext = _kinectBodyView;

            // create a gesture detector for each body (6 bodies => 6 detectors) and create content controls to display results in the UI
            int maxBodies = _kinectSensor.BodyFrameSource.BodyCount;
            for (int i = 0; i < maxBodies; ++i)
            {
                GestureResultView result = new GestureResultView(i,
                                                                 false,
                                                                 false,
                                                                 0.0f,
                                                                 false,
                                                                 false);
                GestureDetector detector = new GestureDetector(_kinectSensor, result);
                _gestureDetectorList.Add(detector);
            }
        }

        /// <summary>
        ///     INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (_bodyFrameReader != null)
            {
                // BodyFrameReader is IDisposable
                _bodyFrameReader.FrameArrived -= Reader_BodyFrameArrived;
                _bodyFrameReader.Dispose();
                _bodyFrameReader = null;
            }

            if (_gestureDetectorList != null)
            {
                // The GestureDetector contains disposable members (VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader)
                foreach (GestureDetector detector in _gestureDetectorList)
                {
                    detector.Dispose();
                }

                _gestureDetectorList.Clear();
                _gestureDetectorList = null;
            }

            if (_kinectSensor != null)
            {
                _kinectSensor.IsAvailableChanged -= Sensor_IsAvailableChanged;
                _kinectSensor.Close();
                _kinectSensor = null;
            }
        }

        /// <summary>
        ///     Handles the event when the sensor becomes unavailable (e.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender,
                                               IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            StatusText = "Don't know";
        }

        /// <summary>
        ///     Handles the body frame data arriving from the sensor and updates the associated gesture detector object for each
        ///     body
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (_bodies == null)
                    {
                        // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
                        _bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(_bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                // visualize the new body data
                _kinectBodyView.UpdateBodyFrame(_bodies);

                // we may have lost/acquired bodies, so update the corresponding gesture detectors
                if (_bodies != null)
                {
                    // loop through all bodies to see if any of the gesture detectors need to be updated
                    int maxBodies = _kinectSensor.BodyFrameSource.BodyCount;
                    for (int i = 0; i < maxBodies; ++i)
                    {
                        Body body = _bodies[i];
                        ulong trackingId = body.TrackingId;

                        // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                        if (trackingId != _gestureDetectorList[i].TrackingId)
                        {
                            _gestureDetectorList[i].TrackingId = trackingId;

                            // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                            // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                            _gestureDetectorList[i].IsPaused = trackingId == 0;
                        }
                    }
                }
            }
        }
    }
}