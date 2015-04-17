using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace Stereotypy_Kinect_Project
{
    public class KinectManager
    {
        private KinectSensor _kinectSensor = null;
        private readonly ColorFrameReader _colorFrameReader = null;
        private BodyFrameReader _bodyFrameReader = null;
        private readonly ImageProcesser _imageProcesser = null;

        //-------------------------------------------------------------------------------


        /// <summary>
        /// Display variable that the GUI uses to display video to the screen.
        /// </summary>
        public WriteableBitmap ColorBit
        {
            get
            {
                if (_imageProcesser != null)
                    return _imageProcesser.ColorImageDisplay;

                return null;
            }
        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// Manages all the Kinect's evens and initial construction assignments
        /// </summary>
        public KinectManager()
        {
            //Get's the first available sensor
            this._kinectSensor = KinectSensor.GetDefault();

            //Add an event to see the status change of the connected sensor
            this._kinectSensor.IsAvailableChanged += Sensor_StatusChange;

            //Start the sensor to start the video capturing element
            this._kinectSensor.Open();

            _colorFrameReader = this._kinectSensor.ColorFrameSource.OpenReader();
           // bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            _colorFrameReader.FrameArrived += colorFrameReader_FrameArrived;
           // bodyFrameReader.FrameArrived += bodyFrameReader_FrameArrived;

            //Start allowing for images to be rendered
            _imageProcesser = new ImageProcesser(_kinectSensor);

        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// Starts the gesture and recording process
        /// </summary>
        public void Start()
        {



        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bodyFrameReader_FrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// Color Frame's that are used to display video onto the GUI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void colorFrameReader_FrameArrived(object sender, ColorFrameArrivedEventArgs e)
        {
            // ColorFrame is IDisposable
            using (ColorFrame colorFrame = e.FrameReference.AcquireFrame())
            {
                if (colorFrame != null)
                {
                    this._imageProcesser.ProcessColorVideo(colorFrame);
                }
            }
        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// Gets status updates from the Kinect's sensor. These updates are used to tell
        ///  if the Kinect is actually connected or disconnected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sensor_StatusChange(object sender, IsAvailableChangedEventArgs e)
        {
            
        }
    }
}