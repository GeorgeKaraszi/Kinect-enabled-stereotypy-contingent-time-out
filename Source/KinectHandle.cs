using System.Collections.Generic;
using Microsoft.Kinect;
using WesternMichgian.SeniorDesign.KinectProject.Algorithms;
using WesternMichgian.SeniorDesign.KinectProject.Recording;

namespace WesternMichgian.SeniorDesign.KinectProject
{
    public class KinectHandle
    {
        /// <summary> Active Kinect sensor </summary>
        private KinectSensor _kinectSensor;

        /// <summary> Reader for body frames </summary>
        private BodyFrameReader _bodyFrameReader;

        /// <summary>
        /// List of detected gestures
        /// </summary>
        private List<GestureDetector> _gestureDetectorList;

        /// <summary> Array for the bodies the kinect tracks </summary>
        private Body[] _bodies;

        /// <summary> 
        /// KinectBodyView object which handles drawing the Kinect bodies to a View box 
        /// in the UI 
        /// </summary>
        public readonly KinectBodyView KinectBodyView;

        public KinectHandle()
        {
            // only one sensor is currently supported
            _kinectSensor = KinectSensor.GetDefault();

            // set IsAvailableChanged event notifier
            _kinectSensor.IsAvailableChanged += Sensor_IsAvailableChanged;

            // open the sensor
            _kinectSensor.Open();

            //initialize the Body view to display skeleton
            KinectBodyView = new KinectBodyView(_kinectSensor);

            // open the reader for the body frames
            _bodyFrameReader = _kinectSensor.BodyFrameSource.OpenReader();

            // set the BodyFramedArrived event notifier
            _bodyFrameReader.FrameArrived += Reader_BodyFrameArrived;

            // initialize the gesture detection objects for our gestures
            _gestureDetectorList = new List<GestureDetector>();


            // create a gesture detector for each body (6 bodies => 6 detectors) 
            //and create content controls to display results in the UI
            int maxBodies = _kinectSensor.BodyFrameSource.BodyCount;
            for (int i = 0; i < maxBodies; ++i)
            {
                GestureResultView result = new GestureResultView(i, false);
                GestureDetector detector = new GestureDetector(_kinectSensor, result);

                //Insert gesture event trigger
                detector.RecordingTable.OnLimitReach += OnLimitReachEvent;

                _gestureDetectorList.Add(detector);
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Disposes of all allocated resources
        /// </summary>
        public void Dispose()
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
                // The GestureDetector contains disposable members 
                // (VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader)
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

        //--------------------------------------------------------------------------------
        /// <summary>
        ///     Handles the body frame data arriving from the sensor and updates 
        ///     the associated gesture detector object for each body
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
                        // creates an array of 6 bodies, which is the max number of 
                        //bodies that Kinect can track simultaneously
                        _bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will 
                    //allocate each Body in the array. As long as those body objects 
                    //are not disposed and not set to null in the array, those body 
                    //objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(_bodies);
                    dataReceived = true;
                }
            }

            if (!dataReceived)
                return;

            KinectBodyView.UpdateBodyFrame(_bodies);

            // we may have lost/acquired bodies, so update the corresponding 
            //gesture detectors
            if (_bodies == null)
                return;

            // loop through all bodies to see if any of the gesture detectors 
            //need to be updated
            int maxBodies = _kinectSensor.BodyFrameSource.BodyCount;
            for (int i = 0; i < maxBodies; ++i)
            {
                Body body = _bodies[i];
                ulong trackingId = body.TrackingId;

                // if the current body TrackingId changed, update the 
                //corresponding gesture detector with the new value
                if (trackingId == _gestureDetectorList[i].TrackingId)
                    continue;

                _gestureDetectorList[i].TrackingId = trackingId;

                // if the current body is tracked, un-pause its detector to 
                //get VisualGestureBuilderFrameArrived events if the current 
                //body is not tracked, pause its detector so we don't waste 
                //resources trying to get invalid gesture results
                _gestureDetectorList[i].IsPaused = trackingId == 0;
            }
        }

        //--------------------------------------------------------------------------------
        private void Sensor_IsAvailableChanged(object sender,
                                               IsAvailableChangedEventArgs e) { }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Handel's the event that occurs when a gesture reaches it's recording limit
        /// </summary>
        /// <param name="source">Instance of recording table class</param>
        /// <param name="e">Name of recorded gesture</param>
        private void OnLimitReachEvent(object source, RecordEventArgs e)
        {
            PeaksAndValleys pv = new PeaksAndValleys();
            var recTable = (RecordingTable)source;
            var recording = recTable.GetGestureRecording(e.GetInfo());
            var period = pv.GetPeriodOfWave(recording.GetRecordValues());

            if (period >= pv.PeriodThreshold)  //Do action
            {
                LockGestures();            //Mutex lock threads from recording
                new QuietHandsWindow().ShowDialog();
                recTable.ClearAllValues(); //Gesture found, reset all recordings
                UnlockGestures();          //Mutex unlock threads 
            }
            else
            {
                recording.Clear();        //Gesture was not detected, reset this recording
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Locks all gestures from recording (Mutex thread lock)
        /// </summary>
        private void LockGestures()
        {
           foreach (var gesture in _gestureDetectorList)
           {
                gesture.MutexLockGesture = true;
           }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Unlocks all gestures for recording (Mutex thread unlock)
        /// </summary>
        private void UnlockGestures()
        {
            foreach (var gesture in _gestureDetectorList)
            {
                gesture.MutexLockGesture = false;
            }
        }


    }
}