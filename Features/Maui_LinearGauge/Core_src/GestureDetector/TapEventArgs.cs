
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// This class serves as an event data for the tap action on the view.
    /// </summary>
    public class TapEventArgs
    {
        private readonly int _noOfTapCount;
        private readonly Point _tapPoint;

        /// <summary>
        /// Initializes when <see cref="TapEventArgs"/>.
        /// </summary>
        public TapEventArgs(Point touchPoint, int tapCount)
        {
            _tapPoint = touchPoint;
            _noOfTapCount = tapCount;    
        }

        /// <summary>
        /// Returns actual touch point.
        /// </summary>
        public Point TapPoint { get { return _tapPoint; } }

        /// <summary>
        /// Returns tap count on touch point.
        /// </summary>
        public int TapCount { get { return _noOfTapCount; } }

    }
}
