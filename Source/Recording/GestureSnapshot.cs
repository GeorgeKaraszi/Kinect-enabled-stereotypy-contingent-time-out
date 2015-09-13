using System;
using System.Collections.Generic;
using Microsoft.Kinect.VisualGestureBuilder;

namespace WesternMichgian.SeniorDesign.KinectProject.Recording
{
    class GestureSnapshot
    {
        private long Timeframe { get; }             //Length of capture
        private readonly List<GestureSnapNode> _listNode;

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of capturing gesture confidence data over a 
        /// period of time.
        /// </summary>
        /// <param name="timeframe">Length of capture</param>
        public GestureSnapshot(long timeframe = 10000)
        {
            _listNode = new List<GestureSnapNode>();
            Timeframe = timeframe;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Add a list of all gestures from a database source
        /// </summary>
        /// <param name="avaibleGestures"></param>
        public void AddGestures(IReadOnlyList<Gesture> avaibleGestures)
        {
            if (avaibleGestures != null)
            {
                foreach (Gesture guester in avaibleGestures)
                {
                    _listNode.Add(new GestureSnapNode(guester,Timeframe));
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(avaibleGestures));
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Add a single gesture to be monitored.
        /// </summary>
        /// <param name="gesture"></param>
        public void AddGesture(Gesture gesture)
        {
            if (gesture != null)
            {
                _listNode.Add(new GestureSnapNode(gesture, Timeframe));
            }
            else
            {
                throw new ArgumentNullException(nameof(gesture));
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Find a gesture based on its name
        /// </summary>
        /// <param name="name">Gesture name</param>
        /// <returns>Found gesture node</returns>
        private GestureSnapNode FindNodeByName(string name)
        {
            return _listNode.Find(gestureSnap => name.Equals(gestureSnap.GestureName));
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Find all gestures in snapshot database that has a type of gesture
        /// </summary>
        /// <param name="gestureType">The type of gesture</param>
        /// <returns>Array of found gesture nodes that match</returns>
        private GestureSnapNode[] FindAllByType(GestureType gestureType)
        {
            return  _listNode.FindAll(
                gestureSnap => gestureType.Equals(gestureSnap.GestureType)).ToArray();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Snap shot(record) the results from the received frame
        /// </summary>
        /// <param name="gesture">Detected gesture</param>
        /// <param name="result">Confidence return</param>
        /// <returns>true if gesture can continue to take data in</returns>
        public bool Snapshot(Gesture gesture, ContinuousGestureResult result)
        {
            GestureSnapNode gestureSnap = FindNodeByName(gesture?.Name ?? "null");

            if (gestureSnap != null)
            {
                gestureSnap.AddProgress(result.Progress);
                return gestureSnap.CanContinue();
            }
            else
            {
                throw new ArgumentException($"Did not find {gesture?.Name ?? "null"}");
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Returns a list of progress that has been captured
        /// </summary>
        /// <param name="gesture">Gesture that needs a progress report</param>
        /// <returns>Progress/Confidence list</returns>
        public List<float> GetProgress(Gesture gesture)
        {
            GestureSnapNode gestureSnap = FindNodeByName(gesture?.Name ?? "null");

           if (gestureSnap != null)
            {
                return gestureSnap.ProgressList;
            }
            else
            {
                throw new ArgumentException($"Did not find {gesture?.Name ?? "null"}");
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Resets progress for a specified gesture
        /// </summary>
        /// <param name="gesture">Gesture that wanted to be reset</param>
        public void ResetProgess(Gesture gesture)
        {
            GestureSnapNode gestureSnap = FindNodeByName(gesture?.Name ?? "null");

            if (gestureSnap != null)
            {
                gestureSnap.ResetProgress();
            }
            else
            {
                throw new ArgumentException($"Did not find {gesture?.Name ?? "null"}");
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Resets all progress for all gestures
        /// </summary>
        public void ResetAllProgress()
        {
            foreach (GestureSnapNode gestureSnap in _listNode)
            {
                gestureSnap.ResetProgress();
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Snap shot(record) the results from the received frame
        /// </summary>
        /// <param name="gesture">Detected gesture</param>
        /// <param name="result">Confidence return</param>
        /// <returns>true if gesture can continue to take data in</returns>
        public bool Snapshot(Gesture gesture, DiscreteGestureResult result)
        {
            return true;
        }
    }
}
