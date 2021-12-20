using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// This class serves as an event data for the long press action on the view.
    /// </summary>
    public class LongPressEventArgs
    {
        private readonly Point _touchPoint;

        /// <summary>
        /// Initializes when <see cref="LongPressEventArgs"/>.
        /// </summary>
        public LongPressEventArgs(Point touchPoint)
        {
            _touchPoint = touchPoint;
        }

        /// <summary>
        /// Returns actual touch point on long press.
        /// </summary>
        public Point TouchPoint { get { return _touchPoint; } }

    }
}
