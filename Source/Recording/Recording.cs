using System;
using System.Collections;
using System.Collections.Generic;

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

        public bool TriggeredWindow { get; set; }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initialize the hash table routine
        /// </summary>
        public RecordingTable()
        {
            TriggeredWindow = false;
            _hashTblRecord = new Hashtable();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Add a gesture to the database of gestures that are going to be recored from
        /// </summary>
        /// <param name="name">Name of gesture</param>
        /// <param name="captureLimitEvent">
        /// Maximum recording values for an 
        /// event to trigger 
        /// </param>
        /// <returns>
        /// -1 = Hash table is null and needs to be initialized
        ///  0 = A Gesture by the inserted name, already exists
        ///  1 = The gesture was inserted successfully
        /// </returns>
        public int AddGesture(string name, int captureLimitEvent)
        {
            int returnValue = -1;
            if (_hashTblRecord != null)
            {
                if (_hashTblRecord.ContainsKey(name) == false)
                {
                    _hashTblRecord.Add(name, new Recording(captureLimitEvent));
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

            if (_hashTblRecord.ContainsKey(name) && TriggeredWindow == false)
            {
                var recording = (Recording) _hashTblRecord[name];

                if (recording.AddValue(value))
                {
                    //trigger event
                    OnLimitReach?.Invoke(this, new RecordEventArgs(name));
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
        /// Obtains the recording class of the gesture
        /// </summary>
        /// <param name="name">Gesture name</param>
        /// <returns></returns>
        public Recording GetGestureRecording(string name) {
            return _hashTblRecord.ContainsKey(name)
                ? (Recording) _hashTblRecord[name]
                : null;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Clears all the recorded values from all recording gestures
        /// </summary>
        public void ClearAllValues()
        {
            if (_hashTblRecord == null)
                return;

            foreach (DictionaryEntry record in _hashTblRecord)
            {
                ((Recording)record.Value).Clear();
            }
        }
    }

    //====================================================================================
    /// <summary>
    /// Recording structure that contains all the values of a selected gesture
    /// </summary>
    public class Recording
    {
        private readonly int _captureLimitEvent;
        private readonly List<double> _recoredValues;

        public Recording(int captureLimitEvent)
        {
            _captureLimitEvent = captureLimitEvent;
            _recoredValues = new List<double>(captureLimitEvent);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Adds a value to the recording list
        /// </summary>
        /// <param name="value">Value that is going to be added</param>
        /// <returns>True if the amount of recordings equal the capture limit</returns>
        public bool AddValue(double value)
        {
            _recoredValues.Add(value);

            return _recoredValues.Count == _captureLimitEvent;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Gets the list of recordings that has been captured
        /// </summary>
        /// <returns></returns>
        public List<double> GetRecordValues()
        {
            return _recoredValues;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Clears the list of recorded values
        /// </summary>
        public void Clear()
        {
            _recoredValues.Clear();
        }
    }
}
