using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents information about the axis ticks. 
    /// </summary>
    internal class AxisTickInfo
    {
        /// <summary>
        /// Gets or sets the start position value of the tick.
        /// </summary>
        internal PointF StartPoint { get; set; }

        /// <summary>
        /// Gets or sets the end position value of the tick.
        /// </summary>
        internal PointF EndPoint { get; set; }

        /// <summary>
        /// Gets or sets the tick position associated with axis value.
        /// </summary>
        internal double Value { get; set; }
    }
}
