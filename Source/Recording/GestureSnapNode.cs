using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Kinect.VisualGestureBuilder;

namespace WesternMichgian.SeniorDesign.KinectProject.Recording
{
    public class GestureSnapNode
    {
        private List<float> ConfidenceFloat { get; }

        private readonly Stopwatch _stopwatch;
        private readonly long _timespan;

        public string GestureName { get; }
        public GestureType GestureType { get; }

        public List<float> ProgressList => ConfidenceFloat;

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initialize new instance node of gesture being tracked.
        /// </summary>
        /// <param name="gesture">Tracked gesture</param>
        /// <param name="timespan">Length of time to record values</param>
        public GestureSnapNode(Gesture gesture, long timespan)
        {
            GestureName     = gesture.Name;
            GestureType     = gesture.GestureType;
            _timespan       = timespan;
            ConfidenceFloat = new List<float>();
            _stopwatch      = new Stopwatch();

        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Add confidence value to a list
        /// </summary>
        /// <param name="value">confidence value</param>
        public void AddProgress(float value)
        {
            if(_stopwatch.IsRunning == false)
                _stopwatch.Start();

            if (CanContinue())
                ConfidenceFloat.Add(value);
            else
            {
                _stopwatch.Stop();
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Check to see if the span of recording values is done.
        /// </summary>
        /// <returns>true if values can be added</returns>
        public bool CanContinue()
        {
            if (_stopwatch.IsRunning && _stopwatch.ElapsedMilliseconds <= _timespan)
                return true;

            return false;
        }


        //--------------------------------------------------------------------------------
        /// <summary>
        /// Clears all confidence values from the node
        /// </summary>
        public void ResetProgress()
        {
            ConfidenceFloat.Clear();                //Clear of confidence values
            _stopwatch.Stop();                      //Stop stopwatch if its running
            _stopwatch.Reset();                     //Reset it to zero
        }
    }
}