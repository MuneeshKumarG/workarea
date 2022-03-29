using System;

namespace Syncfusion.UI.Xaml.Charts
{

    /// <summary>
    /// Represents <see cref="TooltipPosition"/>.
    /// </summary>
    internal enum TooltipPosition
    {
        /// <summary>
        /// Position the tooltip at data point.
        /// </summary>
        Auto,
        /// <summary>
        /// Position the tooltip at mouse pointer.
        /// </summary>
        Pointer
    }


    internal enum HorizontalPosition
    {
        Left,
        Right,
        Center
    }

    internal enum VerticalPosition
    {
        Top,
        Bottom,
        Center
    }

    /// <summary>
    /// Represents <see cref="PolarChartGridLineType"/> type of circular axis.
    /// </summary>
    public enum PolarChartGridLineType
    {
        /// <summary>
        /// A circular axis is drawn as a circle.
        /// </summary>
        Circle,

        /// <summary>
        /// A circular axis is drawn as a polygon. 
        /// </summary>
        Polygon
    }

    /// <summary>
    /// Represents <see cref="ChartAutoScrollingMode"/> mode of axis.
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
        End
    }

    /// <summary>
    /// Represents the doughnut series cap style.
    /// </summary>
    public enum DoughnutCapStyle
    {
        /// <summary>
        /// The both edges are flat.
        /// </summary>
        BothFlat = 0,

        /// <summary>
        /// The both edges are curve.
        /// </summary>
        BothCurve = 1,

        /// <summary>
        /// The start edge only curve.
        /// </summary>
        StartCurve = 2,

        /// <summary>
        /// The end edge only curve.
        /// </summary>
        EndCurve = 3,
    }

    internal enum EmptyStroke
    {
        Left = 1,
        Right = 2,
        Both = 3,
        None = 0
    }

    internal enum DragType
    {
        X,
        Y,
        XY
    }

    internal enum BoxPlotMode
    {
        Exclusive,
        Inclusive,
        Normal
    }

    internal enum ScaleBreakPosition
    {
        DataCount,
        Scale,
        Percent
    }

    /// <summary>
    /// Circular series segment grouping based on group mode.
    /// </summary>
    public enum PieGroupMode
    {
        /// <summary>
        /// Circular series segment grouping based on value.
        /// </summary>
        Value,

        /// <summary>
        /// Circular series segment grouping based on percentage.
        /// </summary>
        Percentage,

        /// <summary>
        /// Circular series segment grouping based on angle.
        /// </summary>
        Angle
    }

    internal enum BreakLineType
    {
        StraightLine,
        Wave        
    }

    internal enum UIElementLeftShift
    {
        LeftShift,
        RightShift,
        LeftHalfShift,
        RightHalfShift,
        Default
    }

    internal enum UIElementTopShift
    {
        TopShift,
        BottomShift,
        TopHalfShift,
        BottomHalfShift,
        Default
    }

    internal enum AxisPosition3D
    {
        FrontLeft,
        DepthFrontLeft,

        FrontRight,
        DepthFrontRight,

        BackRight,
        DepthBackRight,

        BackLeft,
        DepthBackLeft
    }

    #region SelectionActivationMode

    /// <summary>
    /// Defines the way of series or segment selection.
    /// </summary>
    internal enum SelectionActivationMode
    {
        /// <summary>
        /// Select the segment using mouse or pointer click.
        /// </summary>
        Click,

        /// <summary>
        /// Select the segment while mouse hovering.
        /// </summary>
        Move
    }
    #endregion

    #region SelectionType

    /// <summary>
    /// Defines the way of series or segment selection.
    /// </summary>
    public enum SelectionType
    {
        /// <summary>
        /// Do not select the series or segment using mouse or pointer click.
        /// </summary>
        None,

        /// <summary>
        /// Select the single segment using mouse or pointer click.
        /// </summary>
        Point,

        /// <summary>
        /// Select the single series while mouse or pointer click.
        /// </summary>
        Series,
        
        /// <summary>
        /// Select the multiple segment while mouse or pointer click.
        /// </summary>
        MultiPoint,
        
        /// <summary>
        /// Select the multiple series while mouse or pointer click.
        /// </summary>
        MultiSeries
    }
    #endregion

    #region dragAndDrop

    internal enum SnapToPoint
    {
        None,
        Round,
        Floor,
        Ceil
    }
    #endregion

    #region DateTimeRangePadding

    /// <summary>
    /// Represents range padding to the minimum and maximum extremes of the chart axis range for the <see cref="DateTimeAxis"/>.
    /// </summary>
    public enum DateTimeRangePadding
    {
        /// <summary>
        /// RangePadding will be automatically chosen based on the orientation of the axis.
        /// </summary>
        Auto,

        /// <summary>
        /// The visible range sets to exact minimum and maximum value of the items source.
        /// </summary>
        None,

        /// <summary>
        /// The visible range start and end round to nearest interval value.
        /// </summary>
        Round,

        /// <summary>
        /// The visible range start and end will be added with an additional interval.
        /// </summary>
        Additional,

        /// <summary>
        /// The visible range start round to nearest interval value.
        /// </summary>
        RoundStart,

        /// <summary>
        /// The visible range end round to nearest interval value.
        /// </summary>
        RoundEnd,

        /// <summary>
        /// The visible range start will be prepended with an additional interval.
        /// </summary>
        PrependInterval,

        /// <summary>
        /// The visible range start will be appended with an additional interval.
        /// </summary>
        AppendInterval
    }
    #endregion

    #region MACDType

    internal enum MACDType
    {
        Line,
        Histogram,
        Both
    }
    #endregion

    #region SelectionStyle

 

    internal enum SelectionStyle3D
    {
        Single,
        Multiple,
    }
    #endregion

    #region RangeNavigator BarPosition
    internal enum BarPosition
    {
        Inside,
        Outside
    }

    #endregion


    /// <summary>
    /// Defines to find the working days for <see cref="DateTimeAxis"/>
    /// </summary>
    [Flags]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1714: Flags enums should have plural names")]
    public enum Day
    {
        /// <summary>
        /// Sunday
        /// </summary>
        Sunday = 1,

        /// <summary>
        /// Monday
        /// </summary>
        Monday = 2,

        /// <summary>
        /// Tuesday
        /// </summary>
        Tuesday = 4,

        /// <summary>
        /// Wednesday
        /// </summary>
        Wednesday = 8,

        /// <summary>
        /// Thursday
        /// </summary>
        Thursday = 16,

        /// <summary>
        /// Friday
        /// </summary>
        Friday = 32,

        /// <summary>
        /// Saturday
        /// </summary>
        Saturday = 64
    }

    [Flags]
    internal enum ZoomToolBarItems
    {
        All=1,
        ZoomIn=2,
        ZoomOut=4,
        Reset=8,
        SelectZooming=16
    }

    /// <summary>
    /// Represents range padding to the minimum and maximum extremes of the chart axis range for the <see cref="NumericalAxis"/>.
    /// </summary>
    public enum NumericalPadding
    {

        /// <summary>
        /// RangePadding will be automatically chosen based on the orientation of the axis. 
        /// </summary>
        Auto,

        /// <summary>
        /// The visible range sets to exact minimum and maximum value of the items source.
        /// </summary>
        None,

        /// <summary>
        /// The visible range start and end round to nearest interval value.
        /// </summary>
        Round,

        /// <summary>
        /// The visible range will be the actual range calculated from given items source and series types.
        /// </summary>
        Normal,

        /// <summary>
        /// The visible range start and end will be added with an additional interval.
        /// </summary>
        Additional,

        /// <summary>
        /// The visible range start round to nearest interval value.
        /// </summary>
        RoundStart,

        /// <summary>
        /// The visible range end round to nearest interval value.
        /// </summary>
        RoundEnd,

        /// <summary>
        /// The visible range start will be prepended with an additional interval.
        /// </summary>
        PrependInterval,

        /// <summary>
        /// The visible range end will be appended with an additional interval.
        /// </summary>
        AppendInterval
    }

    internal enum ErrorBarType
    {
        Fixed,
        Percentage,
        StandardDeviation,
        StandardErrors,
        Custom
    }

    internal enum ErrorBarMode
    {
        Horizontal,
        Vertical,
        Both
    }

    internal enum ErrorBarDirection
    {
        Both,
        Minus,
        Plus
    }

    /// <summary>
    /// Legend position in chart area.
    /// </summary>
    public enum LegendPosition
    {
        /// <summary>
        /// Positioning the legend inside of chart area.
        /// </summary>
        Inside,

        /// <summary>
        /// Positioning the legend outside of chart area.
        /// </summary>
        Outside

    }

    internal enum TrendlineType
    {
        Linear,
        Exponential,
        Power,
        Logarithmic,
        Polynomial  
    }

    #region DateTimeintervalType
    /// <summary>
    /// A date time interval.
    /// </summary>
    public enum DateTimeIntervalType
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

    #region enum ChartValueType
    /// <summary>
    /// Specifies the different values that are natively used.
    /// </summary>
    /// <seealso cref="ChartAxis"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1720: Identifiers should not contain type names")]
    public enum ChartValueType
    {
        /// <summary>
        ///  <see cref="Double"/> value
        /// </summary>
        Double,

        /// <summary>
        ///  <see cref="DateTime"/> value
        /// </summary>
        DateTime,

        /// <summary>
        ///   <see cref="String"/> value
        /// </summary>
        String,

        /// <summary>
        ///   <see cref="TimeSpan"/> value
        /// </summary>
        TimeSpan,

        /// <summary>
        ///   Logarithmic value
        /// </summary>
        Logarithmic
    }


    #endregion

    #region Rendering modes
    /// <summary>
    /// Specifies the rendering mode to be used to render the chart series.
    /// </summary>
    internal enum RenderingMode
    {
        /// <summary>
        /// Default element will be used to render the series.
        /// </summary>
        Default,

        /// <summary>
        /// WriteableBitmap will be used to render the series.
        /// </summary>
        WriteableBitmap,

        /// <summary>
        /// DirectX will be used to render the series.
        /// </summary>
        DirectX
    }
    #endregion

    #region Legend icon symbol
    /// <summary>
    /// Represents the Icon for the Chartlegend
    /// </summary>  
    /// <seealso cref="ChartSeriesBase"/>
   
    public enum ChartLegendIcon
    {
        /// <summary>
        /// Default behaviour
        /// </summary>
        None,

        /// <summary>
        /// Represents the Icon of Series type
        /// </summary>
        SeriesType,

        /// <summary>
        /// Represents the Rectangular Icon
        /// </summary>
        Rectangle,

        /// <summary>
        ///Represents the Straight Line
        /// </summary>       
        StraightLine,

        /// <summary>
        /// Represents the VerticalLine
        /// </summary>       
        VerticalLine,

        /// <summary>
        /// Represents the Circle
        /// </summary>       
        Circle,

        /// <summary>
        /// Represents the Diamond
        /// </summary>
        Diamond,

        /// <summary>
        /// Represents the Pentagon
        /// </summary>      
        Pentagon,

        /// <summary>
        /// Represents the Hexagon
        /// </summary>       
        Hexagon,

        /// <summary>
        /// Represents the Triangle
        /// </summary>
        Triangle,

        /// <summary>
        /// Represents the Inverted Triangle
        /// </summary>   
        InvertedTriangle,

        /// <summary>
        /// Represents the Cross
        /// </summary>       
        Cross,
        
        /// <summary>
        /// Represents the Plus
        /// </summary>       
        Plus,
    }
    #endregion

    /// <summary>
    ///  Defines the way of display mode for trackball labels. 
    /// </summary>
    public enum TrackballLabelDisplayMode
    {
        /// <summary>
        /// Trackball displays only the nearest label to the touch point.
        /// </summary>
        NearestPoint,

        /// <summary>
        /// Trackball displays all the nearest labels in same x values.
        /// </summary>
        FloatAllPoints,

        /// <summary>
        /// Trackball displays label for all the data points that are grouped.
        /// </summary>
        GroupAllPoints
    }
    

    #region enum Direction
    /// <summary>
    /// Represents sorting direction
    /// </summary>
    internal enum Direction
    {
        /// <summary>
        /// Orders the items in increasing order.
        /// </summary>
        Ascending,
        /// <summary>
        /// Orders the items in decreasing order.
        /// </summary>
        Descending
    }
    #endregion

    #region enum SortingAxis

    /// <summary>
    /// Represents Sorting Axis.
    /// </summary>
    internal enum SortingAxis
    {
        /// <summary>
        /// Sorting will be done based on values related to x-axis.
        /// </summary>
        X,
        /// <summary>
        /// Sorting will be done based on values related to y-axis.
        /// </summary>
        Y,
    }
    #endregion

    #region Adornment symbols

    /// <summary>
    /// Represents the data marker types.
    /// </summary>    
    public enum ChartSymbol
    {
        /// <summary>
        /// Custom option to set User-defined SymbolTemplates
        /// </summary>
        Custom,

        /// <summary>
        /// Renders Ellipse symbol
        /// </summary>
        Ellipse,

        /// <summary>
        /// Renders Cross symbol
        /// </summary>
        Cross,

        /// <summary>
        /// Renders Diamond symbol
        /// </summary>
        Diamond,

        /// <summary>
        /// Renders Hexagon symbol
        /// </summary>
        Hexagon,

        /// <summary>
        /// Renders HorizontalLine symbol
        /// </summary>
        HorizontalLine,

        /// <summary>
        /// Renders InvertedTriangle symbol
        /// </summary>
        InvertedTriangle,

        /// <summary>
        /// Renders Pentagon symbol
        /// </summary>
        Pentagon,

        /// <summary>
        /// Renders Plus symbol
        /// </summary>
        Plus,

        /// <summary>
        /// Renders Square symbol
        /// </summary>
        Square,

        /// <summary>
        /// Renders Traingle symbol
        /// </summary>
        Triangle,

        /// <summary>
        /// Renders VerticalLine symbol
        /// </summary>
        VerticalLine,
    }

    #endregion

    /// <summary>
    /// Defines the way of category axis label placement.
    /// </summary>
    public enum LabelPlacement
    {
        /// <summary>
        /// Labels placed on ticks.
        /// </summary>
        OnTicks,

        /// <summary>
        /// Labels placed on between ticks.
        /// </summary>
        BetweenTicks
    }

    internal enum ActualLabelPosition
    {
        Top,
        Left,
        Right,
        Bottom
    }

    /// <summary>
    /// Represents label placement in Axis.
    /// </summary>
    public enum LabelAlignment
    {
        /// <summary>
        /// Label placed center to the axis tick. 
        /// </summary>
        Center,

        /// <summary>
        /// Label placed Far to the axis tick.
        /// </summary>
        Far,

        /// <summary>
        /// Label placed Near to the axis tick.
        /// </summary>
        Near
    }

    internal enum BorderType
    {
        Brace,
        None,
        Rectangle, 
        WithoutTopAndBottomBorder,   
    }

    /// <summary>
    /// Represents the polar and radar chart axis start angle for primary axis, secondary axis, or both axes.
    /// </summary>
    public enum ChartPolarAngle
    {
        /// <summary>
        /// Indicates chart polar and radar angle axis start position at 0 degree angle. 
        /// </summary>
        Rotate0,

        /// <summary>
        /// Indicates chart polar and radar angle axis start position at 90 degree angle. 
        /// </summary>
        Rotate90,

        /// <summary>
        /// Indicates chart polar and radar angle axis start position at 180 degree angle. 
        /// </summary>
        Rotate180,

        /// <summary>
        /// Indicates chart polar and radar angle axis start position at 270 degree angle. 
        /// </summary>
        Rotate270
    }

#region Axis element position
    /// <summary>
    /// Represents axis elements position in <see cref="ChartAxis"/> elements panel.
    /// </summary>
    public enum AxisElementPosition
    {
        /// <summary>
        /// Positions the elements above the axis line.
        /// </summary>
        Inside,
        /// <summary>
        /// Positions the elements below the axis line.
        /// </summary>
        Outside
    }
    #endregion

    #region Axis header position
    /// <summary>
    /// Represents the axis header position<see cref="ChartAxis"/>.
    /// </summary>
    internal enum AxisHeaderPosition
    {
        /// <summary>
        /// Positions the header near the axis.
        /// </summary>
        Near,
        /// <summary>
        /// Positions the header far away from the axis.
        /// </summary>
        Far
    }
    #endregion

    #region Aggregation functions
    /// <summary>
    /// Represents the aggregation functions<see cref="ChartAxis"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1717: Only FlagsAttribute enums should have plural names")]
    internal enum AggregateFunctions
    {
        Average,
        Count,
        Max,
        Min,
        Sum,
        None
    }
    #endregion

    #region EdgelabelsDrawingMode
    /// <summary>
    /// Represents the modes for placing edge labels in <see cref="ChartAxis"/>.
    /// </summary>

    public enum EdgeLabelsDrawingMode
    {
        /// <summary>
        /// Value indicating that the edge label should appear at the center of its GridLines.
        /// </summary>
        Center,

        /// <summary>
        /// Value indicating that edge labels should be shifted to either left or right so that it comes within the area of Chart.
        /// </summary>
        Shift,

        /// <summary>
        /// Value indicating that the edge labels should be fit within the area of <see cref="ChartBase"/>.
        /// </summary>
        Fit,

        /// <summary>
        /// Value indicating that the edge labels will be hidden.
        /// </summary>
        Hide,

    }
    #endregion

    #region EdgeLabelsVisibilityMode
    /// <summary>
    /// Represents the visibility for edge label<see cref="ChartAxis"/>.
    /// </summary>
   
    public enum EdgeLabelsVisibilityMode
    {
        /// <summary>
        /// Value indicating that default behavior of axis.
        /// </summary>
        Default,

        /// <summary>
        /// Value indicating that edge labels should be visible all cases.
        /// </summary>
        AlwaysVisible,

        /// <summary>
        /// Value indicating that edge labels should be visible in non zoom mode.
        /// </summary>
        Visible,

    }
    #endregion

    #region LabelsIntersectAction

    /// <summary>
    ///  Specifies the options for the action that is to be taken when labels intersect each other.
    /// </summary>
    /// <seealso cref="ChartAxis"/>
    public enum AxisLabelIntersectAction
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
        /// Labels are rotated to avoid intersection.
        /// </summary>
        Auto,

        /// <summary>
        /// Labels are wrapped to next line to aviod intersection.
        /// </summary>
        Wrap

    }
    #endregion

    #region Color Palette
    /// <summary>
    /// Represents the different types of color palette available in <see cref="ChartBase"/> library.
    /// </summary>
   
    public enum ChartColorPalette
    {
        /// <summary>
        /// No palette will be set.
        /// </summary>
        None,

        /// <summary>
        /// Metro palette will be set.
        /// </summary>
        /// 
        Metro,

        /// <summary>
        /// Custom palette will be set, and color values will be taken from <see cref="ChartColorModel.CustomBrushes"/> collection.
        /// </summary>   
        Custom,

        /// <summary>
        /// AutumnBrights palette will be set
        /// </summary>
        AutumnBrights,

        /// <summary>
        /// FloraHues palette will be set
        /// </summary>
        FloraHues,

        /// <summary>
        /// Pineapple palette will be set
        /// </summary>
        Pineapple,

        /// <summary>
        /// TomatoSpectram palette will be set
        /// </summary>
        TomatoSpectrum,

        /// <summary>
        /// RedChrome palette will be set
        /// </summary>
        RedChrome,

        /// <summary>
        /// PurpleChrome palette will be set
        /// </summary>
        PurpleChrome,

        /// <summary>
        /// BlueChrome palette will be set
        /// </summary>
        BlueChrome,

        /// <summary>
        /// GreenChrome palette will be set
        /// </summary>
        GreenChrome,

        /// <summary>
        /// Elite palette will be set
        /// </summary>
        Elite,

        /// <summary>
        /// SandyBeach palette will be set
        /// </summary>
        SandyBeach,

        /// <summary>
        /// LightCandy palette will be set
        /// </summary>
        LightCandy
    }

#endregion

#region ChartLegendAlignment
    /// <summary>
    /// A custom <see cref="ChartBase"/> alignment to handle both horizontal and vertical alignment types in a generalized way. 
    /// </summary>
   
    public enum ChartAlignment
    {
        /// <summary>
        /// Positions the element as like setting left/top alignment.
        /// </summary>
        Near,
        /// <summary>
        /// Positions the element as like setting right/bottom alignment.
        /// </summary>
        Far,
        /// <summary>
        /// Positions the element with center alignment.
        /// </summary>
        Center,
        /// <summary>
        /// Positions the element with default alignment when the series is transposed.
        /// </summary>
        Auto
    }
    #endregion

    #region ChartSeriesDrawType
    /// <summary>
    /// Represents modes of drawing radar and polar types.
    /// </summary>
    public enum ChartSeriesDrawType
    {
        /// <summary>
        /// Draw the Filled Area in the Polar Chart type
        /// </summary>
        Area,

        /// <summary>
        /// Draw the Lines in the Polar chart type
        /// </summary>
        Line,
    }

#endregion

#region Orientation
    /// <summary>
    /// Represents modes of Chart orientation
    /// </summary>
   
    public enum ChartOrientation
    {
        /// <summary>
        /// Orienatation will be automatically analyzed based on the panel's docking position.
        /// </summary>
        Default,
        /// <summary>
        /// Horizontal Orientation will be set.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Vertical Orientation will be set.
        /// </summary>
        Vertical

    }
#endregion

#region ChartAreaType

    /// <summary>
    /// Identifies axes types enumeration.
    /// </summary>
    /// <example>
    /// Intended for internal use
    /// </example>
    /// <seealso>
    ///     <cref>ChartArea</cref>
    /// </seealso>
   
    public enum ChartAreaType
    {
        /// <summary>
        /// Represents No axis.
        /// </summary>
        None,

        /// <summary>
        /// Cartesian axis.
        /// </summary>
        CartesianAxes,

        /// <summary>
        /// Polar axis.
        /// </summary>
        PolarAxes
    }
#endregion

#region ChartUnitType
    /// <summary>
    /// Represents modes for chart rows/columns space allocations.
    /// </summary>
    internal enum ChartUnitType
    {
        /// <summary>
        /// Height/Width will be auto adjusted.
        /// </summary>
        Star,
        /// <summary>
        /// Height/Width will be based on the pixel units given.
        /// </summary>
        Pixels
    }

#endregion

#region zoom mode
    /// <summary>
    /// Represents zooming modes of <see cref="ChartBase"/>
    /// </summary>
    
    public enum ZoomMode
    {
        /// <summary>
        /// Zooming will be done along x-axis
        /// </summary>
        X,
        /// <summary>
        /// Zooming will be done along y-axis
        /// </summary>
        Y,
        /// <summary>
        /// Zooming will be done along both axis.
        /// </summary>
        XY
    }

#endregion

#region
    /// <summary>
    /// Represents label position modes available for PieSeries adornments.
    /// </summary>
    public enum CircularSeriesLabelPosition
    {
        /// <summary>
        /// CircularSeries data labels will be  placed inside over the CircularSeries.
        /// </summary>
        Inside,
        /// <summary>
        /// CircularSeries data labels will be  placed just outside over the CircularSeries.
        /// </summary>
        Outside,
        /// <summary>
        /// CircularSeries data labels will be placed outside over the CircularSeries at a certain distance.
        /// </summary>
        OutsideExtended
    }

    /// <summary>
    /// Represents the type  of connector line that connects the data label and data point.
    /// </summary>
    public enum ConnectorMode
    {
        /// <summary>
        /// This draws a Bezier curve as connector line. 
        /// </summary>
        Bezier,

        /// <summary>
        ///  This draws a solid line as connector line.
        /// </summary>
        Line,

        /// <summary>
        /// This draws a horizontal straight line as connector line. 
        /// </summary>
        StraightLine,
    }

    /// <summary>
    /// Represents the circular series connector line position.
    /// </summary>
    internal enum ConnectorLinePosition 
    {
        /// <summary>
        /// Connector line will be positioned automatically.
        /// </summary>
        Auto,

        /// <summary>
        /// Connector line will be positioned at center.
        /// </summary>
        Center,
    }

    #endregion
    #region AdornmnetsLabelPosition

    /// <summary>
    /// Represents the positioning of data labels.
    /// </summary>
    public enum DataLabelPosition
    {
        /// <summary>
        /// Positions the data labels at Default.
        /// </summary>
        Default,

        /// <summary>
        /// Positions the data labels at Auto.
        /// </summary>
        Auto,

        /// <summary>
        /// Positions the data labels at Inner.
        /// </summary>
        Inner,

        /// <summary>
        /// Positions the data labels at Outer.
        /// </summary>
        Outer,

        /// <summary>
        /// Positions the data labels at Center.
        /// </summary>
        Center

    }

    #endregion

    #region AdornmnetsPosition

    /// <summary>
    /// Represents modes for positioning the bar chart data label.
    /// </summary>
    /// <remarks>
    /// <see cref="ChartDataLabelSettings"/> value cannot be specified for all series types.
    /// The values in data labels position will be applicable only to certain series.
    /// </remarks>
    public enum BarLabelAlignment
    {
        /// <summary>
        /// Positions the data label at the top edge point of a chart segment.
        /// </summary>
        Top,

        /// <summary>
        /// Positions the data label at the bottom edge point of a chart segment.
        /// </summary>
        Bottom,

        /// <summary>
        /// Positions the data label at the center point of a chart segment.
        /// </summary>
        Middle

    }

    #endregion

    #region enum LabelContent

    /// <summary>
    /// Enumeration represents series data label content.
    /// </summary>
    public enum LabelContext
    {
        /// <summary>
        /// Identifies that label should contain X value of series point.
        /// </summary>
        XValue,

        /// <summary>
        /// Identifies that label should contain Y value of series point.
        /// </summary>
        YValue,

        /// <summary>
        /// Identifies that label should contain percentage value of series point among other points.
        /// </summary>
        Percentage,
#if !WinUI
        /// <summary>
        /// Identifies that label should contain value of Y of total values.
        /// </summary>
        YofTot,
#endif
        /// <summary>
        /// Identifies that label should contain <see cref="DateTime"/> value.
        /// </summary>
        DateTime,

        /// <summary>
        /// Label's content will be retrieved from the <c>ChartDataLabel.Item</c> property.
        /// </summary>
        DataLabelItem
    }
#endregion

#region EmptyPointStyle

    /// <summary>
    /// Represents modes of displaying empty points.
    /// </summary>
    internal enum EmptyPointStyle
    {
        /// <summary>
        /// The empty point segment resembles the shape of a normal segment.
        /// Fills the empty point segments with the color value specified in series <see cref="ChartSeriesBase.EmptyPointInterior"/> property.
        /// </summary>
        Interior,

        /// <summary>
        /// The empty point segment resembles the shape of a symbol control.       
        /// </summary>
        Symbol,

        /// <summary>
        /// The empty point segment resembles the shape of a symbol control.
        ///Fills the symbol segments with the color value specified in series <see cref="ChartSeriesBase.EmptyPointInterior"/> property.
        /// </summary>
        SymbolAndInterior

    }
#endregion

#region EmptyPointValue

    /// <summary>
    /// Represents modes for handling empty points.
    /// </summary> 
    internal enum EmptyPointValue
    {
        /// <summary>
        /// Validates the empty points in a series and sets the points y-value to Zero.
        /// </summary>
        Zero,

        /// <summary>
        /// Validates the empty points in a series and sets the points y-value to an average value based on its neighbouring points.
        /// </summary>
        Average
    }
#endregion

#region FunnelMode

    /// <summary>
    /// Represents modes of the funnel types.
    /// </summary>
    /// <seealso>
    ///     <cref>ChartFunnelMode</cref>
    /// </seealso>
    public enum ChartFunnelMode
    {
        /// <summary>
        /// The specified Y value is used to compute the width of the corresponding block.
        /// </summary>
        ValueIsWidth,

        /// <summary>
        /// The specified Y value is used to compute the height of the corresponding block.
        /// </summary>
        ValueIsHeight
    }
#endregion

#region PyramidMode

/// <summary>
/// Specifies the mode in which the Y values should be interpreted in the Pyramid chart.
/// </summary>
/// <seealso>
///     <cref>ChartPyramidMode</cref>
/// </seealso>
    public enum ChartPyramidMode
    {
        /// <summary>
        /// The Y values are proportional to the length of the sides of the pyramid.
        /// </summary>
        Linear,

        /// <summary>
        /// The Y values are proportional to the surface area of the corresponding blocks.
        /// </summary>
        Surface
    }
#endregion

#region IntervalType
/// <summary>
/// Specifies the Interval type in which the navigator values should be displayed.
/// </summary>
    internal enum NavigatorIntervalType
    {
        /// <summary>
        /// One year interval.
        /// </summary>
        Year,

        /// <summary>
        /// One Quarter interval
        /// </summary>
        Quarter,

        /// <summary>
        /// One Month interval
        /// </summary>
        Month,

        /// <summary>
        /// One Week interval
        /// </summary>
        Week,

        /// <summary>
        /// One Day interval
        /// </summary>
        Day,

        /// <summary>
        /// One Day interval
        /// </summary>
        Hour

    }
#endregion

#region NavigatorRangePadding
    internal enum NavigatorRangePadding
    {
        None,
        Round
    }
#endregion

#region Annotations
    internal enum CoordinateUnit
    {
        /// <summary>
        /// The pixel mode for the CoordinateUnit of Annotation
        /// </summary>
        Pixel,

        /// <summary>
        /// The axis mode for the CoordianteUint of Annotation
        /// </summary>
        Axis
    }

    internal enum AxisMode
    {
        Horizontal,
        Vertical,
        All
    }

    internal enum LineCap
    {
        None,
        Arrow
    }

    /// <summary>
    /// Defines the way of positioning the tooltip labels.
    /// </summary>
    public enum ToolTipLabelPlacement
    {
        /// <summary>
        /// Represents the tooltip position left to the interaction point. 
        /// </summary>
        Left,

        /// <summary>
        /// Represents the tooltip position right to the interaction point.
        /// </summary>
        Right,

        /// <summary>
        /// Represents the tooltip position top to the interaction point.
        /// </summary>
        Top,

        /// <summary>
        /// Represents the tooltip position left to the interaction point.
        /// </summary>
        Bottom
    }

    
#endregion

#region UpdateAction
    
    [Flags]
    internal enum UpdateAction
    {
        Create = 2,
        UpdateRange = 4,
        Layout= 8,
        Render = 16,
        LayoutAndRender = Layout | Render,
        UpdateRangeAndArrange = UpdateRange | Layout | Render,
        Invalidate = Create | UpdateRange | Layout | Render,
    }

#endregion

#region Surface Type
    /// <summary>
    /// Specifies the type of surface
    /// </summary>
    internal enum SurfaceType
    {
        Surface,
        WireframeSurface,
        Contour,
        WireframeContour
    }

#endregion

#region Camera Projection

/// <summary>
/// Specifies the mode of surface projection
/// </summary>
    internal enum CameraProjection
    {
        /// <summary>
        /// Represents Perspective CameraProjection
        /// </summary>
        Perspective,

        /// <summary>
        /// Represents Orthographic CameraProjection
        /// </summary>
        Orthographic
    }

#endregion
#region Comparison Mode
/// <summary>
/// Specifies which price need to consider for fluctuation detection
/// </summary>
    internal enum FinancialPrice
    {
        High,
        Low,
        Open,
        Close,
        None
    }
    #endregion

    #region WaterfallSegment type

    /// <summary>
    /// Specifies which type segment consider for rendering.
    /// </summary>
    internal enum WaterfallSegmentType
    {
        Positive,
        Negative,
        Sum
    }

    #endregion

    #region Spline type
    /// <summary>
    /// Specifies the type of spline.
    /// </summary>
    public enum SplineType
    {
        /// <summary>
        /// This type used natural spline for data rendering.
        /// </summary>
        Natural,

        /// <summary>
        /// This type used monotonic spline for data rendering.
        /// </summary>
        Monotonic,

        /// <summary>
        /// This type used cartinal spline for data rendering.
        /// </summary>
        Cardinal,

        /// <summary>
        /// This type used clamped spline data rendering.
        /// </summary>
        Clamped
    }
    #endregion
}
