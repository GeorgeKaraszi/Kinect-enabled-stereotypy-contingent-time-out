using System.Collections.Generic;
using Microsoft.Kinect;

namespace Gestures.HMMs
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

        public KinectHandle()
        {
            // only one sensor is currently supported
            _kinectSensor = KinectSensor.GetDefault();

            // set IsAvailableChanged event notifier
            _kinectSensor.IsAvailableChanged += Sensor_IsAvailableChanged;

            // open the sensor
            _kinectSensor.Open();

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
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get a list of all gestures that have been loaded.
        /// </summary>
        /// <returns>List of gesture string names</returns>
        public List<string> GetGestureNames()
        {
            return _gestureDetectorList[0].GestureNames;
        }

        /// <summary>
        /// Set the gesture that wants to be loaded
        /// </summary>
        /// <param name="gesture">Name containing the gesture to be loaded</param>
        public void SetGesture(string gesture)
        {
            foreach (var gest in _gestureDetectorList)
            {
                gest.SelectedGesture = gesture;
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

            if (dataReceived)
            {
                // we may have lost/acquired bodies, so update the corresponding 
                //gesture detectors
                if (_bodies != null)
                {
                    // loop through all bodies to see if any of the gesture detectors 
                    //need to be updated
                    int maxBodies = _kinectSensor.BodyFrameSource.BodyCount;
                    for (int i = 0; i < maxBodies; ++i)
                    {
                        Body body = _bodies[i];
                        ulong trackingId = body.TrackingId;

                        // if the current body TrackingId changed, update the 
                        //corresponding gesture detector with the new value
                        if (trackingId != _gestureDetectorList[i].TrackingId)
                        {
                            _gestureDetectorList[i].TrackingId = trackingId;

                            // if the current body is tracked, un-pause its detector to 
                            //get VisualGestureBuilderFrameArrived events if the current 
                            //body is not tracked, pause its detector so we don't waste 
                            //resources trying to get invalid gesture results
                            _gestureDetectorList[i].IsPaused = trackingId == 0;
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------------
        private void Sensor_IsAvailableChanged(object sender,
                                               IsAvailableChangedEventArgs e)
        { }
    }
}