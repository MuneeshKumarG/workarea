namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// For a <see cref="SfRadialGauge"/> : Defines the constants that specify the tick position in the gauge. 
    /// </summary>
    public enum GaugeElementPosition
    {
        /// <summary>
        /// For a <see cref="SfRadialGauge"/> : The ticks are placed inside the axis line.
        /// </summary>
        Inside,

        /// <summary>
        /// For a <see cref="SfRadialGauge"/> : The ticks are placed outside the axis line.
        /// </summary>
        Outside,

        /// <summary>
        /// For a <see cref="SfRadialGauge"/> : The ticks are placed center the axis line.
        /// </summary>
        Cross
    }

    /// <summary>
    /// Defines the constants that specify the label position in the gauge. 
    /// </summary>
    public enum GaugeLabelsPosition
    {
        /// <summary>
        /// For a <see cref="SfRadialGauge"/> : The labels are placed inside the axis line. 
        /// </summary>
        Inside,

        /// <summary>
        /// For a <see cref="SfRadialGauge"/> : The labels are placed outside the axis line. 
        /// </summary>
        Outside
    }

    /// <summary>
    /// Specifies the size units in the radial gauge.
    /// </summary>
    public enum SizeUnit
    {
        /// <summary>
        /// Indicates to treat the provided value as pixel.
        /// </summary>
        Pixel,

        /// <summary>
        /// Indicates to treat the provided value as factor.
        /// </summary>
        Factor
    }

    /// <summary>
    /// For a <see cref="SfRadialGauge"/> : Specifies the corner style for <see cref="RadialAxis"/>, <see cref="RangePointer"/>.
    /// </summary>
    public enum CornerStyle
    {
        /// <summary>
        /// Flat does not apply the rounded corner on both side
        /// </summary>
        BothFlat,

        /// <summary>
        /// Curve apply the rounded corner on both side.
        /// </summary>
        BothCurve,

        /// <summary>
        /// Curve apply the rounded corner on end(right) side.
        /// </summary>
        EndCurve,

        /// <summary>
        /// Curve apply the rounded corner on start(left) side.
        /// </summary>
        StartCurve
    }

    /// <summary>
    /// Specifies the different marker type for pointer.
    /// </summary>
    public enum MarkerType
    {
        /// <summary>
        /// Specifies the circle shape for the marker.
        /// </summary>
        Circle,

        /// <summary>
        /// Specifies the diamond shape for the marker.
        /// </summary>
        Diamond,

        /// <summary>
        /// Specifies the inverted triangle shape for the marker.
        /// </summary>
        InvertedTriangle,

        /// <summary>
        /// Specifies the rectangle shape for the marker.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Specifies the triangle shape for the marker.
        /// </summary>
        Triangle,
    }

    /// <summary>
    /// Specifies the different shape type for pointer.
    /// </summary>
    public enum ShapeType
    {
        /// <summary>
        /// Specifies the circle shape for the marker.
        /// </summary>
        Circle,

        /// <summary>
        /// Specifies the diamond shape for the marker.
        /// </summary>
        Diamond,

        /// <summary>
        /// Specifies the inverted triangle shape for the marker.
        /// </summary>
        InvertedTriangle,

        /// <summary>
        /// Specifies the triangle shape for the marker.
        /// </summary>
        Triangle,
    }

    /// <summary>
    /// Specifies whether to position the gauge annotation based on axis value or angle.
    /// </summary>
    public enum AnnotationDirection
    {
        /// <summary>
        /// Specifies the position of the gauge annotation based on angle.
        /// </summary>
        Angle,

        /// <summary>
        /// Specifies the position of the gauge annotation based on axis value.
        /// </summary>
        AxisValue
    }

    /// <summary>
    /// Specifies the horizontal or vertical alignment.
    /// </summary>
    public enum GaugeAlignment
    {
        /// <summary>
        /// Aligns the gauge element to start either the horizontal or vertical.
        /// </summary>
        Start,

        /// <summary>
        /// Aligns the gauge element to center either the horizontal or vertical.
        /// </summary>
        Center,

        /// <summary>
        /// Aligns the gauge element to far either the horizontal or vertical.
        /// </summary>
        End
    }

    /// <summary>
    /// Specifies the horizontal or vertical orientation.
    /// </summary>
    public enum GaugeOrientation
    {
        /// <summary>
        /// Aligns the gauge elements in horizontal orientation.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Aligns the gauge elements in vertical orientation.
        /// </summary>
        Vertical,
    }
}
