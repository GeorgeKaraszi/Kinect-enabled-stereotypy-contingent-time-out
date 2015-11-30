using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Kinect;
using WesternMichgian.SeniorDesign.KinectProject.CaptureUtil;
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
        /// Determines if there are skeletons being currently tracked
        /// </summary>
        public bool TrackingBodies
        {
            get
            { return _gestureDetectorList.Any(g => g.IsPaused == false); }
        }

        /// <summary> 
        /// KinectBodyView object which handles drawing the Kinect bodies to a View box 
        /// in the UI 
        /// </summary>
        public readonly KinectBodyView KinectBodyView;

        /// <summary>
        /// Event that handles incoming and outgoing data to the utility window
        /// </summary>
        public event ChangeInDataEvent OnDataChange
        {
            add
            {
                foreach (var g in _gestureDetectorList)
                {
                    g.RecordingTable.OnChangeInData += value;
                }
            }
            remove
            {
                foreach (var g in _gestureDetectorList)
                {
                    g.RecordingTable.OnChangeInData -= value;
                }
            }
        }

        /// <summary>
        /// Event handler that deals with gestures that have reached their period limit
        /// </summary>
        public event AnalysisEventHandeler OnLimitReach
        {
            add
            {
                foreach (var g in _gestureDetectorList)
                {
                    g.RecordingTable.OnLimitReach += value;
                }
            }
            remove
            {
                foreach (var g in _gestureDetectorList)
                {
                    g.RecordingTable.OnLimitReach -= value;
                }
            }
        }


        //public event ChangeInPeriodEvent OnPeriodChange;
        public event ChangeInSkeletonsEvent OnSkeletonChange;

        public string[] GetGestureNames => 
            _gestureDetectorList[0].GestureNameList.ToArray();

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
                GestureDetector detector = new GestureDetector(_kinectSensor, result, i);

                _gestureDetectorList.Add(detector);
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Sets the target gesture the utility window will require to record from
        /// </summary>
        /// <param name="bodyid">Id of the body being recorded</param>
        /// <param name="gestureName">Name of the new target gesture</param>
        public void SetTargetGesture(int bodyid, string gestureName)
        {
            _gestureDetectorList[bodyid].RecordingTable.TargetGesture = gestureName;
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

                // If the gesture detector is NOW paused, reset its interpretation data.
                if (_gestureDetectorList[i].IsPaused)
                {
                    OnSkeletonChange?.Invoke(this,new UtilBodyArgs(i, false));
                    _gestureDetectorList[i].ResetInterpreter();
                }
                else
                {
                    OnSkeletonChange?.Invoke(this, new UtilBodyArgs(i, true));
                }
            }
        }

        //--------------------------------------------------------------------------------
        private void Sensor_IsAvailableChanged(object sender,
                                               IsAvailableChangedEventArgs e)
        {
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Locks all gestures from recording (Mutex thread lock)
        /// </summary>
        public void LockGestures()
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
        public void UnlockGestures()
        {
            foreach (var gesture in _gestureDetectorList)
            {
                gesture.MutexLockGesture = false;
            }
        }
    }
}