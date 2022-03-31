
namespace Syncfusion.UI.Xaml.Charts
{

    /// <summary>
    /// Represents the position of a child element in the chart.
    /// </summary>  
    public enum LegendDock
    {
        /// <summary>
        /// Docks element at the left side of panel.
        /// </summary>
        Left,

        /// <summary>
        /// Docks element at the top side of panel.
        /// </summary>
        Top,

        /// <summary>
        /// Docks element at the right side of panel.
        /// </summary>
        Right,

        /// <summary>
        /// Docks element at the bottom side of panel.
        /// </summary>
        Bottom,

        /// <summary>
        /// Docks element at any position on  panel
        /// </summary>
        Floating,
    }

    internal enum LegendOrientation
    {
        /// <summary>
        /// Orientation will be automatically analyzed based on the layout docking position.
        /// </summary>
        Default,

        /// <summary>
        /// Horizontal Orientation will be set.
        /// </summary>
        Horizontal,

        /// <summary>
        /// Vertical Orientation will be set.
        /// </summary>
        Vertical,
    }

    public enum ShapeType
    {
        /// <summary>
        /// Custom shape type. 
        /// </summary>
        Custom,
        /// <summary>
        /// Rectangle
        /// </summary>
        Rectangle,
        /// <summary>
        /// HorizontalLine
        /// </summary>
        HorizontalLine,
        /// <summary>
        /// Circle
        /// </summary>
        Circle,
        /// <summary>
        /// Diamond
        /// </summary>
        Diamond,
        /// <summary>
        /// Pentagon
        /// </summary>
        Pentagon,
        /// <summary>
        /// Triangle
        /// </summary>
        Triangle,
        /// <summary>
        /// InvertedTriangle
        /// </summary>
        InvertedTriangle,
        /// <summary>
        /// Cross
        /// </summary>
        Cross,
        /// <summary>
        /// Plus
        /// </summary>
        Plus,
        /// <summary>
        /// Hexagon
        /// </summary>
        Hexagon,
        /// <summary>
        /// VerticalLine
        /// </summary>
        VerticalLine
    }
}
