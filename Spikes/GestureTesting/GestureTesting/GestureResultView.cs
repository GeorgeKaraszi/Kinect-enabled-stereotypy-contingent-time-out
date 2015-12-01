#region

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Windows.Media;

#endregion

namespace GestureTesting
{
    /// <summary>
    ///     Stores discrete gesture results for the GestureDetector.
    ///     Properties are stored/updated for display in the UI.
    /// </summary>
    public sealed class GestureResultView : INotifyPropertyChanged
    {
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

        /// <summary> Image to display in UI which corresponds to tracking/detection state </summary>
        private ImageSource _imageSource;

        /// <summary> True, if the body is currently being tracked </summary>
        private bool _isTracked;

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
                if (!Equals(_bodyColor, value))
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
        ///     Gets an image for display in the UI which represents the current gesture result for the associated body
        /// </summary>
        public ImageSource ImageSource
        {
            get { return _imageSource; }

            private set
            {
                if (!Equals(ImageSource, value))
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
        public GestureResultView(int bodyIndex, bool isTracked)
        {
            BodyIndex = bodyIndex;
            IsTracked = isTracked;
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to 
        /// bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Updates the values associated with the discrete gesture detection result
        /// </summary>
        /// <param name="isBodyTrackingIdValid">
        ///     True, if the body associated with the GestureResultView object is still 
        ///     being tracked
        /// </param>
        public void UpdateBodyView(bool isBodyTrackingIdValid)
        {
            IsTracked = isBodyTrackingIdValid;

            BodyColor = !IsTracked ? Brushes.Gray : _trackedColors[BodyIndex];
        }

        /// <summary>
        ///     Notifies UI that a property has changed
        /// </summary>
        /// <param name="propertyName">Name of property that has changed</param>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}