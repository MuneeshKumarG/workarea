

namespace Syncfusion.Maui.Charts
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
    internal enum AxisLabelsIntersectAction
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

        /// <summary>
        /// Labels are wrapped into next line to avoid intersection.
        /// </summary>
        Wrap,
    }

    /// <summary>
    /// Represents auto scrolling delta mode of axis. The axis can be scrolled from the start position or end position.
    /// </summary>
    internal enum ChartAutoScrollingMode
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
    /// 
    /// </summary>
    public enum EdgeLabelsDrawingMode
    {
        /// <summary>
		/// 
        /// </summary>
        Center,

        /// <summary>
		/// 
        /// </summary>
        Shift,

        /// <summary>
		/// 
        /// </summary>
        Fit,

        /// <summary>
		/// 
        /// </summary>
        Hide,
    }

    /// <summary>
    ///  
    /// </summary>
    public enum ChartAxisLabelAlignment
    {
        /// <summary>
        /// 
        /// </summary>
        Start,

        /// <summary>
        ///  
        /// </summary>
        Center,

        /// <summary>
        /// 
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

    /// <summary>
    /// 
    /// </summary>
    public enum EdgeLabelsVisibilityMode
    {
        /// <summary>
        /// 
        /// </summary>
        Default,

        /// <summary>
        /// 
        /// </summary>
        AlwaysVisible,

        /// <summary>
        /// 
        /// </summary>
        Visible
    }

    /// <summary>
    ///  
    /// </summary>
    public enum NumericalPadding
    {
        /// <summary>
        /// 
        /// </summary>
        Auto,

        /// <summary>
        /// 
        /// </summary>
        None,

        /// <summary>
        /// 
        /// </summary>
        Round,

        /// <summary>
        /// 
        /// </summary>
        Normal,

        /// <summary>
        /// 
        /// </summary>
        Additional,

        /// <summary>
        /// 
        /// </summary>
        RoundStart,

        /// <summary>
        /// 
        /// </summary>
        RoundEnd,

        /// <summary>
        /// 
        /// </summary>
        PrependInterval,

        /// <summary>
        /// 
        /// </summary>
        AppendInterval,
    }

    /// <summary>
    ///  
    /// </summary>
    public enum LabelPlacement
    {
        /// <summary>
        ///
        /// </summary>
        OnTicks,

        /// <summary>
        /// 
        /// </summary>
        BetweenTicks,
    }

    #region DateTimeIntervalType

    /// <summary>
    ///  
    /// </summary>
    public enum DateTimeIntervalType
    {
        /// <summary>
        ///  
        /// </summary>
        Auto = 0,

        /// <summary>
        /// 
        /// </summary>
        Milliseconds = 1,

        /// <summary>
        ///  
        /// </summary>
        Seconds = 2,

        /// <summary>
        /// 
        /// </summary>
        Minutes = 3,

        /// <summary>
        /// 
        /// </summary>
        Hours = 4,

        /// <summary>
        /// 
        /// </summary>
        Days = 5,

        /// <summary>
        /// 
        /// </summary>
        Months = 6,

        /// <summary>
        ///  
        /// </summary>
        Years = 7,
    }

    #endregion

    #region DateTimeDeltaType

    /// <summary>
    /// 
    /// </summary>
    internal enum DateTimeDeltaType
    {
        /// <summary>
        /// Automatically determine interval.
        /// </summary>
        Auto = 0,

        /// <summary>
        /// Interval type is milliseconds.
        /// </summary>
        Milliseconds = 1,

        /// <summary>
        /// Interval type is seconds.
        /// </summary>
        Seconds = 2,

        /// <summary>
        /// Interval type is minutes.
        /// </summary>
        Minutes = 3,

        /// <summary>
        /// Interval type is hours.
        /// </summary>
        Hours = 4,

        /// <summary>
        /// Interval type is days.
        /// </summary>
        Days = 5,

        /// <summary>
        /// Interval type is months.
        /// </summary>
        Months = 6,

        /// <summary>
        /// Interval type is years.
        /// </summary>
        Years = 7,
    }

    #endregion

    #region DateTimeRangePadding

    /// <summary>
    ///  
    /// </summary>
    public enum DateTimeRangePadding
    {
        /// <summary>
        ///
        /// </summary>
        Additional,

        /// <summary>
        /// 
        /// </summary>
        None,

        /// <summary>
        ///  
        /// </summary>
        Auto,

        /// <summary>
        ///  
        /// </summary>
        Round,

        /// <summary>
        /// 
        /// </summary>
        RoundStart,

        /// <summary>
        /// 
        /// </summary>
        RoundEnd,

        /// <summary>
        /// 
        /// </summary>
        PrependInterval,

        /// <summary>
        ///  
        /// </summary>
        AppendInterval
    }

    #endregion

    #region ScatterChartShapes
    /// <summary>
    /// 
    /// </summary>
    public enum ShapeType
    {
        /// <summary>
        /// 
        /// </summary>
        Custom,

        /// <summary>
        /// 
        /// </summary>
        Rectangle,

        /// <summary>
        /// 
        /// </summary>
        HorizontalLine,

        /// <summary>
        /// 
        /// </summary>
        Circle,

        /// <summary>
        /// 
        /// </summary>
        Diamond,

        /// <summary>
        /// 
        /// </summary>
        Pentagon,

        /// <summary>
        /// 
        /// </summary>
        Triangle,

        /// <summary>
        /// 
        /// </summary>
        InvertedTriangle,

        /// <summary>
        /// 
        /// </summary>
        Cross,

        /// <summary>
        /// 
        /// </summary>
        Plus,

        /// <summary>
        /// 
        /// </summary>
        StraightLine,

        /// <summary>
        /// 
        /// </summary>
        Hexagon,

        /// <summary>
        /// 
        /// </summary>
        VerticalLine
    }
    #endregion

    #region Spline type
    /// <summary>
    /// 
    /// </summary>
    public enum SplineType
    {
        /// <summary>
        /// 
        /// </summary>
        Natural,

        /// <summary>
        /// 
        /// </summary>
        Monotonic,

        /// <summary>
        /// 
        /// </summary>
        Cardinal,

        /// <summary>
        /// 
        /// </summary>
        Clamped
    }
    #endregion

    #region Selection behavior

    /// <summary>
    /// 
    /// </summary>
    internal enum SelectionType
    {
        /// <summary>
        /// Do not select any segment.
        /// </summary>
        None,

        /// <summary>
        /// Select the single point.
        /// </summary>
        Point,
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    public enum ZoomMode
    {
        /// <summary>
        /// 
        /// </summary>
        X,

        /// <summary>
        ///
        /// </summary>
        Y,

        /// <summary>
        /// 
        /// </summary>
        XY,
    }

    #region DataLabel

    /// <summary>
    /// 
    /// </summary>
    public enum DataLabelAlignment
    {
        /// <summary>
        /// 
        /// </summary>
        Top,

        /// <summary>
        ///
        /// </summary>
        Middle,

        /// <summary>
        ///
        /// </summary>
        Bottom
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DataLabelPlacement
    {
        /// <summary>
		/// 
		/// </summary>
		Auto,

        /// <summary>
        ///
        /// </summary>
        Inner,

        /// <summary>
		///
		/// </summary>
		Center,

        /// <summary>
        /// 
        /// </summary>
        Outer,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ConnectorType
    {
        /// <summary>
        /// 
        /// </summary>
        Curve,

        /// <summary>
        /// 
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
    /// 
    /// </summary>
    public enum ChartLegendIconType
    {
        /// <summary>
        ///
        /// </summary>
        Circle,
        /// <summary>
        ///
        /// </summary>
        Rectangle,
        /// <summary>
        ///
        /// </summary>
        HorizontalLine,
        /// <summary>
        ///
        /// </summary>
        Diamond,
        /// <summary>
        ///
        /// </summary>
        Pentagon,
        /// <summary>
        ///
        /// </summary>
        Triangle,
        /// <summary>
        /// 
        /// </summary>
        InvertedTriangle,
        /// <summary>
        ///
        /// </summary>
        Cross,
        /// <summary>
        ///
        /// </summary>
        Plus,
        /// <summary>
        ///
        /// </summary>
        Hexagon,
        /// <summary>
        ///
        /// </summary>
        VerticalLine
    }
}
