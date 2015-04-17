using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace Stereotypy_Kinect_Project
{
    public class ImageProcesser
    {
        private WriteableBitmap    _colorBitmap = null; //Used to display to a given screen.
        private readonly Int32Rect _colorImgRect;   //Default value for filling 
                                                    //unchanged images from being displayed.

        //-------------------------------------------------------------------------------
        /// <summary>
        /// Display frame that has been rendered for the GUI is display
        /// </summary>
        public WriteableBitmap ColorImageDisplay
        {
            get { return this._colorBitmap; }
        }

        //-------------------------------------------------------------------------------
        /// <summary>
        /// Registers the Image that is being transferred from the Kinect sensor to be
        ///  displayed to the GUI screen.
        /// </summary>
        /// <param name="kinectSensor"></param>
        public ImageProcesser(KinectSensor kinectSensor)
        {
            //Get Kinects color image resolution properties
            FrameDescription cFrameDescription = kinectSensor.ColorFrameSource.
                CreateFrameDescription(ColorImageFormat.Bgra);

            //Register the image information with the display variable
            this._colorBitmap = new WriteableBitmap(cFrameDescription.Width,
                            cFrameDescription.Height,
                            96.0, 96.0, PixelFormats.Bgr32, null);

            //Blank image that is rendered if no image has come throw the pipe
            this._colorImgRect = new Int32Rect(0, 0,
                        this._colorBitmap.PixelWidth,
                        this._colorBitmap.PixelHeight);
        }


        //-------------------------------------------------------------------------------
        /// <summary>
        /// Processes the incoming video frame
        /// </summary>
        /// <param name="colorFrame">Frame that is being captured</param>
        public void ProcessColorVideo(ColorFrame colorFrame)
        {
            FrameDescription cFrameDescription = colorFrame.FrameDescription;

            using (KinectBuffer colorBuffer = colorFrame.LockRawImageBuffer())
            {
                //Lock the image from being messed with till update is complete.
                this._colorBitmap.Lock();

                // verify data and write the new color frame data to the display bitmap
                if ((cFrameDescription.Width == this._colorBitmap.PixelWidth) &&
                    (cFrameDescription.Height == this._colorBitmap.PixelHeight))
                {
                    colorFrame.CopyConvertedFrameDataToIntPtr(
                        this._colorBitmap.BackBuffer,
                        (uint)(cFrameDescription.Width * cFrameDescription.Height * 4),
                        ColorImageFormat.Bgra);

                    this._colorBitmap.AddDirtyRect(_colorImgRect);
                }

                //Allow for image to be utilized now after the frame as been updated.
                this._colorBitmap.Unlock();
            }
        }


    }
}