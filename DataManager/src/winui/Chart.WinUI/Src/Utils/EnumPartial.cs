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
    /// Represents the doughnut series cap style.
    /// </summary>
    internal enum DoughnutCapStyle
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

    internal enum DragType
    {
        X,
        Y,
        XY
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
    /// <seealso cref="ChartSeries"/>
   
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
    public enum LabelDisplayMode
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
    internal enum ChartSeriesDrawType
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
   
    internal enum ChartOrientation
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

    internal enum ChartAreaType
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

}
