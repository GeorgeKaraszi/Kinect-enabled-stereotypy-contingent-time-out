using System;
using System.Collections;
using WesternMichgian.SeniorDesign.KinectProject.Algorithms;
using WesternMichgian.SeniorDesign.KinectProject.CaptureUtil;

namespace WesternMichgian.SeniorDesign.KinectProject.Recording
{
    public delegate void AnalysisEventHandeler(object source, RecordEventArgs e);

    //====================================================================================
    /// <summary>
    /// Event Argument parameter class is designed to carry the information we need for 
    /// an event to be casted.
    /// </summary>
    public class RecordEventArgs : EventArgs
    {
        /// <summary>
        /// Holds the gesture name that has triggered the event
        /// </summary>
        private readonly string _gestureName;

        //--------------------------------------------------------------------------------
        /// <summary>
        /// </summary>
        /// <param name="name">Gesture name</param>
        public RecordEventArgs(string name) { _gestureName = name; }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Gets the gesture name that is being held in the event argument
        /// </summary>
        /// <returns>Gesture name</returns>
        public string GetInfo()
        {
            return _gestureName;
        }

    }

    //====================================================================================
    /// <summary>
    /// Hash table used to keep individual gesture recording records.
    /// </summary>
    public class RecordingTable
    {
        /// <summary>
        /// Hash table that holds class objects of individual gestures, to access to, 
        /// for better performance.
        /// </summary>
        private readonly Hashtable _hashTblRecord;

        /// <summary>
        /// Event that is activated when the capture limit has been hit
        /// </summary>
        public event AnalysisEventHandeler OnLimitReach;
        /// <summary>
        /// Triggered event when ever a change in stream data occurs
        /// </summary>
        public event ChangeInDataEvent OnChangeInData;

        // Frame at which the last event was triggered.
        private int LastTrigger { get; set; }
        // Minimum number of frames in between events triggered.
        private const int TriggerDistance = 45;

        private int BodyId { get; }

        public string TargetGesture { get; set; }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initialize the hash table routine
        /// </summary>
        public RecordingTable(int bodyid)
        {
            _hashTblRecord = new Hashtable();
            LastTrigger    = 0;
            BodyId         = bodyid;
            TargetGesture  = "Empty";
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Add a gesture to the database of gestures that are going to be recored from
        /// </summary>
        /// <param name="name">Name of gesture</param>
        /// <returns>
        /// -1 = Hash table is null and needs to be initialized
        ///  0 = A Gesture by the inserted name, already exists
        ///  1 = The gesture was inserted successfully
        /// </returns>
        public int AddGesture(string name)
        {
            int returnValue = -1;
            if (_hashTblRecord != null)
            {
                if (_hashTblRecord.ContainsKey(name) == false)
                {
                    _hashTblRecord.Add(name, new GestureInterpreter());
                    returnValue = 1;
                }
                else
                {
                    returnValue = 0;
                }
            }

            return returnValue;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Adds a confidence value from the kinect to the recordings of select gesture
        /// </summary>
        /// <param name="name">Gesture name</param>
        /// <param name="value">Value to be added</param>
        /// <returns>
        /// -1 = Hash table does not contain the gesture requested
        ///  0 = Value was added with no event triggered
        ///  1 = Value was added but an event was triggered
        /// </returns>
        public int AddValue(string name, double value)
        {
            int returnValue = -1;

            if (_hashTblRecord.ContainsKey(name))
            {
                var classifier = (GestureInterpreter) _hashTblRecord[name];

                //If gesture is targeted on the Capture Util. Report it.
                if (TargetGesture == name)
                {
                    OnChangeInData?.Invoke(this, 
                        new UtilDataArgs(value,classifier.Frame + 1, BodyId));
                }

                if (classifier.ProcessPoint(value))
                {
                    // If not enough time has passed since last trigger, ignore.
                    if (classifier.Frame - LastTrigger < TriggerDistance)
                    {
                        returnValue = 0;
                    }
                    //trigger event
                    else
                    {
                        LastTrigger = classifier.Frame;
                        OnLimitReach?.Invoke(this, new RecordEventArgs(name));
                        returnValue = 1;
                    }
                }
                else
                {
                    returnValue = 0;
                }
            }

            return returnValue;
        }

        /// <summary>
        /// Reset the gesture interpreters in the recording table for this body.
        /// </summary>
        public void Reset()
        {
            foreach (String key in _hashTblRecord.Keys)
            {
                var classifier = (GestureInterpreter) _hashTblRecord[key];

                classifier.Reset();
            }
        }
    }
}
