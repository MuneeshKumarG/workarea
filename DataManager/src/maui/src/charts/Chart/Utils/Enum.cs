
#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
	/// <summary>
	/// 
	/// </summary>
	internal enum ChartValueType
    {
        /// <summary>
        ///  
        /// </summary>
        Double,
        
        /// <summary>
        ///  
        /// </summary>
        DateTime,

        /// <summary>
        ///   
        /// </summary>
        String,

        /// <summary>
        ///   
        /// </summary>
        TimeSpan,

        /// <summary>
        /// 
        /// </summary>
        Logarithmic
    }

    /// <summary>
    /// Represents the axis elements position in <c> ChartAxis</c> elements panel. The axis elements can be positioned inside or outside the chart area. 
    /// </summary>
    internal enum AxisElementPosition
    {
        /// <summary>
        /// Positions the elements above the axis line.
        /// </summary>
        Inside,

        /// <summary>
        /// Positions the elements below the axis line.
        /// </summary>
        Outside,
    }

    /// <summary>
    /// Specifies the options for the action that is to be taken when labels intersect each other.
    /// </summary>
    /// <seealso cref="ChartAxis"/>
#if WinUI
    public enum AxisLabelsIntersectAction
#else
    internal enum AxisLabelsIntersectAction
#endif
    {
        /// <summary>
        /// No special action is taken. Labels may intersect.
        /// </summary>
        None,

        /// <summary>
        /// Labels are wrapped into multiple rows to avoid intersection.
        /// </summary>
        MultipleRows,

        /// <summary>
        /// Labels are hidden to avoid intersection.
        /// </summary>
        Hide,
#if WinUI
        /// <summary>
        /// Labels are rotated to avoid intersection.
        /// </summary>
        Auto,
#endif
        /// <summary>
        /// Labels are wrapped into next line to avoid intersection.
        /// </summary>
        Wrap,
    }

    /// <summary>
    /// Represents auto scrolling delta mode of axis. The axis can be scrolled from the start position or end position.
    /// </summary>
    public enum ChartAutoScrollingMode
    {
        /// <summary>
        /// Indicates AutoScrollingDelta calculated in axis start position. 
        /// </summary>
        Start,

        /// <summary>
        /// Indicates AutoScrollingDelta calculated in axis end position. 
        /// </summary>
        End,
    }

    /// <summary>
    ///Represents the radial bar segment cap style. The radial bar cap style can be positioned at the segment start, end, or both ends.
    /// </summary>
    public enum CapStyle
    {
        /// <summary>
        /// Indicates that a flat shape should appear at the start and end positions.
        /// </summary>
        BothFlat = 0,

        /// <summary>
        /// Indicates that a curve shape should appear at the start and end positions.
        /// </summary>
        BothCurve = 1,

        /// <summary>
        /// Indicates that a curve shape should appear at the start position.
        /// </summary>
        StartCurve = 2,

        /// <summary>
        /// Indicates that a curve shape should appear at the end position.
        /// </summary>
        EndCurve = 3,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum EdgeLabelsDrawingMode
    {
        /// <summary>
        /// Indicates the edge labels should appear at the center of its GridLines.
        /// </summary>
        Center,

        /// <summary>
        /// Indicates the edge labels should be shifted to either the left or right.
        /// </summary>
        Shift,

        /// <summary>
        /// Indicates that the edge labels must fit within the chart area. 
        /// </summary>
        Fit,

        /// <summary>
        /// Indicates the edge labels will be hidden.
        /// </summary>
        Hide,
    }

    /// <summary>
    /// Represents the visibility for the edge labels in the <see cref="ChartAxis"/>.
    /// </summary>
    public enum EdgeLabelsVisibilityMode
    {
        /// <summary>
        /// Indicates the default behavior of the axis.
        /// </summary>
        Default,

        /// <summary>
        /// Indicates that the edge labels should be visible for all cases.
        /// </summary>
        AlwaysVisible,

        /// <summary>
        /// Indicates that the edge labels should be visible in non-zoom mode.
        /// </summary>
        Visible
    }

    /// <summary>
    /// Represents range padding to the minimum and maximum extremes of the chart axis range for the <see cref="NumericalAxis"/>.
    /// </summary>
    public enum NumericalPadding
    {
        /// <summary>
        /// RangePadding will be chosen automatically based on the axis orientation.
        /// </summary>
        Auto,

        /// <summary>
        /// The visible range specifies the exact minimum and maximum value of the item source.
        /// </summary>
        None,

        /// <summary>
        /// The start and end visible ranges are rounded to the nearest interval value.
        /// </summary>
        Round,

        /// <summary>
        /// The visible range will be the actual range calculated from given items source and series types.
        /// </summary>
        Normal,

        /// <summary>
        /// An additional interval will be added with the start and end visible ranges.
        /// </summary>
        Additional,

        /// <summary>
        /// The start visible range is rounded to the nearest interval value.
        /// </summary>
        RoundStart,

        /// <summary>
        /// The end visible range is rounded to the nearest interval value.
        /// </summary>
        RoundEnd,

        /// <summary>
        /// An additional interval will be prepended to the start visible range.
        /// </summary>
        PrependInterval,

        /// <summary>
        /// An additional interval will be appended to the end visible range.
        /// </summary>
        AppendInterval,
    }

    #region BoxPlotMode
    /// <summary>
    /// Represents the BoxPlot series mode.
    /// </summary>
    public enum BoxPlotMode
    {
        /// <summary>
        /// BoxPlotMode is Exclusive.
        /// </summary>
        Exclusive,

        /// <summary>
        /// BoxPlotMode is Inclusive.
        /// </summary>
        Inclusive,

        /// <summary>
        /// BoxPlotMode is Normal.
        /// </summary>
        Normal,
    }
    #endregion

    /// <summary>
    /// Defines the placement of category axis labels.
    /// </summary>
    public enum LabelPlacement
    {
        /// <summary>
        /// Labels positioned as the center of ticks
        /// </summary>
        OnTicks,

        /// <summary>
        /// Labels positioned between the ticks
        /// </summary>
        BetweenTicks,
    }

#region DateTimeIntervalType

    /// <summary>
    /// Defines a date time interval.
    /// </summary>
    public enum DateTimeIntervalType
    {
        /// <summary>
        /// Interval type will be determined automatically.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// Indicates the interval type is milliseconds.
        /// </summary>
        Milliseconds = 1,

        /// <summary>
        ///  Indicates the interval type is seconds.
        /// </summary>
        Seconds = 2,

        /// <summary>
        /// Indicates the interval type is minutes.
        /// </summary>
        Minutes = 3,

        /// <summary>
        /// Indicates the interval type is hours.
        /// </summary>
        Hours = 4,

        /// <summary>
        /// Indicates the interval type is days.
        /// </summary>
        Days = 5,

        /// <summary>
        /// Indicates the interval type is months.
        /// </summary>
        Months = 6,

        /// <summary>
        ///  Indicates the interval type is years.
        /// </summary>
        Years = 7,
    }

#endregion

#region DateTimeRangePadding

    /// <summary>
    /// Represents range padding to the minimum and maximum extremes of the chart axis range for the <see cref="DateTimeAxis"/>.
    /// </summary>
    public enum DateTimeRangePadding
    {
        /// <summary>
        /// An additional interval will be added with the start and end visible ranges.
        /// </summary>
        Additional,

        /// <summary>
        /// The visible range specifies the exact minimum and maximum value of the item source.
        /// </summary>
        None,

        /// <summary>
        /// RangePadding will be chosen automatically based on the axis orientation.
        /// </summary>
        Auto,

        /// <summary>
        /// The start and end visible ranges are rounded to the nearest interval value.
        /// </summary>
        Round,

        /// <summary>
        /// The start visible range is rounded to the nearest interval value.
        /// </summary>
        RoundStart,

        /// <summary>
        /// The end visible range is rounded to the nearest interval value.
        /// </summary>
        RoundEnd,

        /// <summary>
        /// An additional interval will be prepended to the start visible range.
        /// </summary>
        PrependInterval,

        /// <summary>
        /// An additional interval will be appended to the end visible range.
        /// </summary>
        AppendInterval
    }

#endregion

    #region Spline type
    /// <summary>
    /// Indicates the type of spline.
    /// </summary>
    public enum SplineType
    {
        /// <summary>
        /// Natural spline is used to render data.
        /// </summary>
        Natural,

        /// <summary>
        /// Monotonic spline is used to render data.
        /// </summary>
        Monotonic,

        /// <summary>
        /// Cardinal spline is used to render data.
        /// </summary>
        Cardinal,

        /// <summary>
        /// Clamped spline is used to render data.
        /// </summary>
        Clamped
    }
    #endregion

    #region Selection behavior

    /// <summary>
    /// Defines the selection type for a <see cref="ChartSelectionBehavior"/>.
    /// </summary>
    public enum ChartSelectionType
    {
        /// <summary>
        /// No items can be selected.
        /// </summary>
        None,

        /// <summary>
        /// The user can select only one item at a time.
        /// </summary>
        Single,

        /// <summary>
        /// The user can select two or many items at a time.
        /// </summary>
        Multiple,

        /// <summary>
        /// The user can select and deselect only one item at a time.
        /// </summary>
        SingleDeselect,
    }

    #endregion

    /// <summary>
    /// Indicates zooming modes of the chart.
    /// </summary>
    public enum ZoomMode
    {
        /// <summary>
        /// Zooming will take place on the x-axis.
        /// </summary>
        X,

        /// <summary>
        /// Zooming will take place on the y-axis.
        /// </summary>
        Y,

        /// <summary>
        /// Zooming will take place on both axes.
        /// </summary>
        XY,
    }

    #region ScatterChartShapes
    /// <summary>
    /// Represents scatter shape types.
    /// </summary>
    public enum ShapeType
    {
        /// <summary>
        /// Custom option to set User-defined symbol.
        /// </summary>
        Custom,

        /// <summary>
        /// Indicates to render rectangle symbol.
        /// </summary>
        Rectangle,

        /// <summary>
        /// Indicates to render horizontal line symbol.
        /// </summary>
        HorizontalLine,

        /// <summary>
        /// Indicates to render circle symbol.
        /// </summary>
        Circle,

        /// <summary>
        /// Indicates to render diamond symbol.
        /// </summary>
        Diamond,

        /// <summary>
        /// Indicates to render pentagon symbol.
        /// </summary>
        Pentagon,

        /// <summary>
        /// Indicates to render triangle symbol.
        /// </summary>
        Triangle,

        /// <summary>
        /// Indicates to render cross symbol.
        /// </summary>
        InvertedTriangle,

        /// <summary>
        /// Indicates to render rectangle symbol.
        /// </summary>
        Cross,

        /// <summary>
        /// Indicates to render plus symbol.
        /// </summary>
        Plus,

        /// <summary>
        /// Indicates to render hexagon symbol.
        /// </summary>
        Hexagon,

        /// <summary>
        /// Indicates to render vertical line symbol.
        /// </summary>
        VerticalLine
    }
    #endregion

#if !WinUI

    /// <summary>
    /// Represents a label's alignment on an axis.
    /// </summary>
    public enum ChartAxisLabelAlignment
    {
        /// <summary>
        /// The label is positioned before the axis tick.
        /// </summary>
        Start,

        /// <summary>
        /// The label is positioned center of the axis tick.
        /// </summary>
        Center,

        /// <summary>
        /// The label is positioned after the axis tick.
        /// </summary>
        End,
    }

    internal enum ChartTextWrapMode
    {
        /// <summary>
        /// Labels are wrapped by word.
        /// </summary>
        WordWrap,

        /// <summary>
        /// Labels are wrapped by character.
        /// </summary>
        CharacterWrap
    }
   

    #region DataLabel

    /// <summary>
    /// Represents different ways to position the chart data labels.
    /// </summary>
    /// <remarks>The values in the data label's position will be applicable only to certain series.</remarks>
    public enum DataLabelAlignment
    {
        /// <summary>
        /// Positions the data label at the top edge point of the chart segment.
        /// </summary>
        Top,

        /// <summary>
        /// Positions the data label at the center point of the chart segment.
        /// </summary>
        Middle,

        /// <summary>
        /// Positions the data label at the bottom edge point of the chart segment.
        /// </summary>
        Bottom
    }

    /// <summary>
    /// Represents the positioning of data labels.
    /// </summary>
    public enum DataLabelPlacement
    {
        /// <summary>
        /// Positions the data labels automatically.
        /// </summary>
        Auto,

        /// <summary>
        /// Positions the data labels at inner.
        /// </summary>
        Inner,

        /// <summary>
        /// Positions the data labels at center.
        /// </summary>
        Center,

        /// <summary>
        /// Positions the data labels at outer.
        /// 
        /// </summary>
        Outer,
    }

    /// <summary>
    /// Represents the types of connector line that connects the label and data point.
    /// </summary>
    public enum ConnectorType
    {
        /// <summary>
        /// This draws a curve as connector line.
        /// </summary>
        Curve,

        /// <summary>
        /// This draws a line as connector line.
        /// </summary>
        Line,
    }

    /// <summary>
    /// 
    /// </summary>
    internal enum SmartLabelAlignment
    {
        /// <summary>
        /// 
        /// </summary>
        Shift,

        /// <summary>
        ///
        /// </summary>
        None,

        /// <summary>
        /// 
        /// </summary>
        Hide
    }

    #endregion


    /// <summary>
    /// Represents the icon symbol for the chart legend.
    /// </summary>
    public enum ChartLegendIconType
    {
        /// <summary>
        /// Indicates the circle icon.
        /// </summary>
        Circle,
        /// <summary>
        /// Indicates the rectangle icon.
        /// </summary>
        Rectangle,
        /// <summary>
        /// Indicates the horizontal line icon.
        /// </summary>
        HorizontalLine,
        /// <summary>
        /// Indicates the diamond icon.
        /// </summary>
        Diamond,
        /// <summary>
        /// Indicates the pentagon icon.
        /// </summary>
        Pentagon,
        /// <summary>
        /// Indicates the triangle icon.
        /// </summary>
        Triangle,
        /// <summary>
        /// Indicates the inverted triangle icon.
        /// </summary>
        InvertedTriangle,
        /// <summary>
        /// Indicates the cross icon.
        /// </summary>
        Cross,
        /// <summary>
        /// Indicates the plus icon.
        /// </summary>
        Plus,
        /// <summary>
        /// Indicates the hexagon icon.
        /// </summary>
        Hexagon,
        /// <summary>
        /// Indicates the vertical line icon.
        /// </summary>
        VerticalLine
    }

    /// <summary>
    /// Specify whether to display trackball labels for all the data points along the vertical line or display only a single label.
    /// </summary>
    public enum LabelDisplayMode
    {
        /// <summary>
        /// Trackball labels are displayed for all the data points.
        /// </summary>
        FloatAllPoints,

        /// <summary>
        /// The nearest data point label is displayed.
        /// </summary>
        NearestPoint,

       // GroupAllPoints,
    }

    /// <summary>
    /// Specifies which type of segment to consider for rendering.
    /// </summary>
    internal enum WaterfallSegmentType
    {
        Positive,
        Negative,
        Sum
    }

    #region Mode For Pyramid chart
    /// <summary>
    /// 
    /// </summary>
    internal enum PyramidMode
    {
        /// <summary>
        /// 
        /// </summary>
        Surface,

        /// <summary>
        /// 
        /// </summary>
        Linear
    }


    /// <summary>
    /// Represents the content of the pyramid chart data label; it should be XValue or YValue.
    /// </summary>
    public enum PyramidDataLabelContext
    {
        /// <summary>
        /// Indicates that the label should contain the Y value of the data points.
        /// </summary>
        YValue,
        /// <summary>
        /// Indicates that the label should contain the X value of the data points.
        /// </summary>
        XValue,
    }

    /// <summary>
    /// Represents the content of the funnel chart data label; it should be XValue or YValue.
    /// </summary>
    public enum FunnelDataLabelContext
    {
        /// <summary>
        /// Indicates that the label should contain the Y value of the data points.
        /// </summary>
        YValue,
        /// <summary>
        /// Indicates that the label should contain the X value of the data points.
        /// </summary>
        XValue,
    }


    #endregion

    #region ErrorBarSeries

    /// <summary>
    /// Represents the error bar mode for <see cref="ErrorBarSeries"/>.
    /// </summary>
    public enum ErrorBarMode
    {
        /// <summary>
        /// Specify whether to display both the horizontal error bar and vertical error bar.
        /// </summary>
        Both,

        /// <summary>
        /// Specify whether to display only horizontal error bar.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Specify whether to display only vertical error bar.
        /// </summary>
        Vertical
    }

    /// <summary>
    /// Represents the error bar type for <see cref="ErrorBarSeries"/>.
    /// </summary>
    public enum ErrorBarType
    {
        /// <summary>
        /// This refers to a type of error bar where the error bars are fixed to a specified value, rather than being determined by the data.
        /// </summary>
        Fixed,

        /// <summary>
        /// Specified to Calculate the percentage error for a given data point and display the error bar accordingly
        /// </summary>
        Percentage,

        /// <summary>
        /// Indicate a statistical measure that measures the amount of variation or dispersion of a set of values from their mean or average value.
        /// </summary>
        StandardDeviation,

        /// <summary>
        /// This type specifies that the statistical measure of the variability of sample statistics, and is often used to represent the amount of uncertainty or variation associated with the estimated mean or other summary statistic in an error bar chart
        /// </summary>
        StandardError,

        /// <summary>
        /// By using this type, we can able to customize the horizontal error and vertical error for a each data point. 
        /// </summary>
        Custom
    }

    /// <summary>
    /// Represents the error bar direction for <see cref="ErrorBarSeries"/>.
    /// </summary>
    public enum ErrorBarDirection
    {
        /// <summary>
        /// This indicates the error bars are drawn in both directions.
        /// </summary>
        Both,

        /// <summary>
        /// This indicates the error bars are only drawn in a negative direction..
        /// </summary>
        Minus,

        /// <summary>
        /// This indicates the error bars are only drawn in a positive direction.
        /// </summary>
        Plus
    }

    /// <summary>
    /// Represents the error bar stroke cap for <see cref="ErrorBarSeries"/>.
    /// </summary>
    public enum ErrorBarStrokeCap
    {
        /// <summary>
        /// Specify whether the edges of the error bar caps are drawn in a flat shape.
        /// </summary>
        Flat,

        /// <summary>
        /// Specify whether the edges of the error bar caps are drawn in a round shape.
        /// </summary>
        Round,

        /// <summary>
        /// Specify whether the edges of the error bar caps are drawn in a square shape.
        /// </summary>
        Square
    }

    #endregion
#endif

}
