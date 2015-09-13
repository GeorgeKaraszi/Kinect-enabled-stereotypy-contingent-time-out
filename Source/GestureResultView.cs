#region

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Media;

#endregion

namespace WesternMichgian.SeniorDesign.KinectProject
{
    /// <summary>
    ///     Stores discrete gesture results for the GestureDetector.
    ///     Properties are stored/updated for display in the UI.
    /// </summary>
    public sealed class GestureResultView : INotifyPropertyChanged
    {
        private const int COUNT_CONST = 10;

        /// <summary> Timer </summary>
        private readonly Timer _timer;

        //private readonly ImageSource notTrackedImage = new BitmapImage(new Uri(@"Images\NotTracked.png", UriKind.Relative));
        /// <summary> Image to show when the 'detected' property is true for a tracked body </summary>
        /// <summary> Image to show when the 'detected' property is false for a tracked body </summary>
        /// <summary> Image to show when the body associated with the GestureResultView object is not being tracked </summary>
        /// <summary>
        ///     Array of brush colors to use for a tracked body; array position corresponds to the body colors used in the
        ///     KinectBodyView class
        /// </summary>
        private readonly Brush[] _trackedColors =
        {
            Brushes.Red,
            Brushes.Orange,
            Brushes.Green,
            Brushes.Blue,
            Brushes.Indigo,
            Brushes.Violet
        };

        /// <summary> Brush color to use as background in the UI </summary>
        private Brush _bodyColor = Brushes.Gray;

        /// <summary> The body index (0-5) associated with the current gesture detector </summary>
        private int _bodyIndex;

        /// <summary> Current confidence value reported by the discrete gesture </summary>
        private float _confidence;

        private int _countdown;

        /// <summary> True, if the discrete gesture is currently being detected </summary>
        private bool _detected;

        /// <summary> Image to display in UI which corresponds to tracking/detection state </summary>
        private ImageSource _imageSource;

        private bool _isHandAboveHead, _previous;

        /// <summary> True, if the body is currently being tracked </summary>
        private bool _isTracked;

        private bool _quietWindowRunning;

        /// <summary>
        ///     Gets the body index associated with the current gesture detector result
        /// </summary>
        public int BodyIndex
        {
            get { return _bodyIndex; }

            private set
            {
                if (_bodyIndex != value)
                {
                    _bodyIndex = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        ///     Gets the body color corresponding to the body index for the result
        /// </summary>
        public Brush BodyColor
        {
            get { return _bodyColor; }

            private set
            {
                if (_bodyColor != value)
                {
                    _bodyColor = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        ///     Gets a value indicating whether or not the body associated with the gesture detector is currently being tracked
        /// </summary>
        public bool IsTracked
        {
            get { return _isTracked; }

            private set
            {
                if (IsTracked != value)
                {
                    _isTracked = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        ///     Gets a value indicating whether or not the discrete gesture has been detected
        /// </summary>
        public bool Detected
        {
            get { return _detected; }

            private set
            {
                if (_detected != value)
                {
                    _detected = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        ///     Gets a float value which indicates the detector's confidence that the gesture is occurring for the associated body
        /// </summary>
        public float Confidence
        {
            get { return _confidence; }

            private set
            {
                if (_confidence != value)
                {
                    _confidence = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        ///     Gets an image for display in the UI which represents the current gesture result for the associated body
        /// </summary>
        public ImageSource ImageSource
        {
            get { return _imageSource; }

            private set
            {
                if (ImageSource != value)
                {
                    _imageSource = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        ///     Initializes a new instance of the GestureResultView class and sets initial property values
        /// </summary>
        /// <param name="bodyIndex">Body Index associated with the current gesture detector</param>
        /// <param name="isTracked">True, if the body is currently tracked</param>
        /// <param name="detected">True, if the gesture is currently detected for the associated body</param>
        /// <param name="confidence">Confidence value for detection of the 'Seated' gesture</param>
        public GestureResultView(int bodyIndex,
                                 bool isTracked,
                                 bool detected,
                                 float confidence,
                                 bool isHandAboveHead,
                                 bool previous)
        {
            BodyIndex = bodyIndex;
            IsTracked = isTracked;
            Detected = detected;
            Confidence = confidence;
            _isHandAboveHead = isHandAboveHead;
            _previous = previous;
            //this.ImageSource = this.notTrackedImage;

            _timer = new Timer();
            _countdown = COUNT_CONST;

            _timer.Tick += timer_Tick;
        }

        /// <summary>
        ///     INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void timer_Tick(object sender, EventArgs e)
        {
            if (_countdown > 0)
                _countdown -= 1;
            else
            {
                _quietWindowRunning = true;
                _countdown = COUNT_CONST;
                QuietHandsWindow win = new QuietHandsWindow();
                win.ShowDialog();
                _timer.Stop();
                _quietWindowRunning = false;
            }
        }

        /// <summary>
        ///     Updates the values associated with the discrete gesture detection result
        /// </summary>
        /// <param name="isBodyTrackingIdValid">
        ///     True, if the body associated with the GestureResultView object is still being
        ///     tracked
        /// </param>
        /// <param name="isGestureDetected">True, if the discrete gesture is currently detected for the associated body</param>
        /// <param name="detectionConfidence">Confidence value for detection of the discrete gesture</param>
        public void UpdateGestureResult(bool isBodyTrackingIdValid,
                                        bool isGestureDetected,
                                        float detectionConfidence)
        {
            IsTracked = isBodyTrackingIdValid;
            Confidence = 0.0f;

            if (!IsTracked)
            {
                //this.ImageSource = this.notTrackedImage;
                Detected = false;
                BodyColor = Brushes.Gray;
            }
            else
            {
                Detected = isGestureDetected;
                BodyColor = _trackedColors[BodyIndex];

                if (_quietWindowRunning == false)
                {
                    if (Detected)
                    {
                        Confidence = detectionConfidence;
                        //this.ImageSource = this.seatedImage;

                        _timer.Start();
                    }
                    else
                    {
                        //this.ImageSource = this.notSeatedImage;
                        _timer.Stop();
                        _countdown = COUNT_CONST;
                    }
                }
                else
                {
                    _timer.Stop();
                    _countdown = COUNT_CONST;
                }
            }
        }

        /// <summary>
        ///     Notifies UI that a property has changed
        /// </summary>
        /// <param name="propertyName">Name of property that has changed</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}