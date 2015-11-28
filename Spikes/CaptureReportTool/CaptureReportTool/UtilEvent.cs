using System;

namespace CaptureReportTool
{
    public delegate void ChangeInSkeletonsEvent(object source, UtilBodyArgs e);

    public delegate void ChangeInDataEvent(object source, UtilDataArgs e);

    public delegate void ChangeInPeriodEvent(object source, UtilPeriodArgs e);

    public delegate void ChangeInTargetGestureEvent(object source, UtilTargetGestArgs e);

    //====================================================================================
    /// <summary>
    /// Event arguments for obtaining body index ID
    /// </summary>
    public class UtilBodyArgs : EventArgs
    {
        private int BodyId { get; set; }
        private bool TrackStatus { get; }

        public UtilBodyArgs(int bodyid, bool tracked)
        {
            BodyId = bodyid;
            TrackStatus = tracked;
        }

        public int GetBodyId()
        {
            return BodyId;
        }

        public bool IsBeingTracked()
        {
            return TrackStatus;
        }
    }

    //====================================================================================
    /// <summary>
    /// Event arguments for new data on tracked id
    /// </summary>
    public class UtilDataArgs : EventArgs
    {
        private double Value{ get; }
        private int Frame { get; }

        public UtilDataArgs(double value, int frame)
        {
            Value = value;
            Frame = frame;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Get the frame and value of the invoked event
        /// </summary>
        /// <returns>
        /// Tuple containing frame and value
        /// Item1:Frame #
        /// Item2:Value at Frame #
        /// </returns>
        public Tuple<int,double> GetFrameValue()
        {
            return new Tuple<int, double>(Frame,Value);
        }
    }

    //====================================================================================
    /// <summary>
    /// Obtains the Period of the newly analyses field.
    /// </summary>
    public class UtilPeriodArgs : EventArgs
    {
        private int Period { get; }

        public UtilPeriodArgs(int period) { Period = period; }

        public int GetPeriod()
        {
            return Period;
        }
    }

    //====================================================================================
    /// <summary>
    /// Obtains the new Target gesture to capture during the recording phase
    /// </summary>
    public class UtilTargetGestArgs : EventArgs
    {
        private int BodyId { get; }
        private string GestureName { get; }

        public UtilTargetGestArgs(int bodyId, string gestureName)
        {
            BodyId = bodyId;
            GestureName = gestureName;
        }

        public int GetBodyId() { return BodyId; }
        public string GetGestureName() { return GestureName;}
    }
}