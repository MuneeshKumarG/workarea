using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// This class serves as an event data for the panning action on the view.
    /// </summary>
    public class PanEventArgs
    {
        private readonly Point _touchPoint;
        private readonly Point _translatePoint;

        /// <summary>
        /// Initializes when <see cref="PanEventArgs"/>
        /// </summary>
        public PanEventArgs(Point touchPoint, Point translatePoint)
        {
            _touchPoint = touchPoint;
            _translatePoint = translatePoint;
        }

        /// <summary>
        /// Returns translate distance point on panning.
        /// </summary>
        public Point TranslatePoint { get { return _translatePoint; } }

        /// <summary>
        /// Returns actual touch point on panning.
        /// </summary>
        public Point TouchPoint { get { return _touchPoint; } }

    }
}
