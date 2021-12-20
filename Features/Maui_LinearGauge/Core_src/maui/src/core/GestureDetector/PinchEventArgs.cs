using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// This class serves as an event data for the pinch action on the view.
    /// </summary>
    public class PinchEventArgs
    {
        private readonly Point _touchPoint;
        private readonly double _angle;
        private readonly float _scale;
        private readonly GestureStatus _status;

        /// <summary>
        /// Initializes when <see cref="PinchEventArgs"/>
        /// </summary>
        public PinchEventArgs(GestureStatus status, Point touchPoint, double angle, float scale)
        {
            _status = status;
            _touchPoint = touchPoint;
            _scale = scale;
            _angle = angle;
        }

        /// <summary>
        /// Returns <see cref="GestureStatus"/> on pinch interaction.
        /// </summary>
        public GestureStatus Status { get { return _status; } }

        /// <summary>
        /// Returns scale value on pinch interaction.
        /// </summary>
        public float Scale { get { return _scale; } }

        /// <summary>
        /// Returns actual touch point on pinch interaction.
        /// </summary>
        public Point TouchPoint { get { return _touchPoint; } }

        /// <summary>
        /// Returns angle on pinch interaction.
        /// </summary>
        public double Angle { get { return _angle; } }
    }
}
