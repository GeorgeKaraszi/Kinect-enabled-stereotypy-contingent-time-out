using System;
using Microsoft.Kinect;

namespace Stereotypy_Kinect_Project
{
    public class KinectManager
    {
        private KinectSensor kinectSensor = null;
        private string statusText = null;


        public KinectManager()
        {
            //Get's the frist avaiable sensor
            this.kinectSensor = KinectSensor.GetDefault();

            //Add an event to see the status change of the connected sensor
            this.kinectSensor.IsAvailableChanged += Sensor_StatusChange;

            //Start the sensor to start the video capturing element
            this.kinectSensor.Open();

            //Check to see if the sensor has connected correctly
            this.statusText = this.kinectSensor.IsAvailable
                ? "Kinect sensor is avaiable"
                : "Kinect sesnor is disconnected";

        }

        /// <summary>
        /// Starts the guesture and recording process
        /// </summary>
        public void Start()
        {
            
        }

        private void Sensor_StatusChange(object sender, IsAvailableChangedEventArgs isAvailableChangedEventArgs)
        {
            throw new NotImplementedException();
        }
    }
}