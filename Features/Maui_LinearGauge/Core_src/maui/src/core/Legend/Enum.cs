using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Represents the default shape types in <see cref="SfShapeView"/>.
    /// </summary>
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

    /// <summary>
    /// Represents the legend placement to customize the legend position.
    /// </summary>
    public enum LegendPlacement
    {
        /// <summary>
        /// Position legend at left.
        /// </summary>
        Left,
        /// <summary>
        /// Position legend at top.
        /// </summary>
        Top,
        /// <summary>
        /// Position legend at right.
        /// </summary>
        Right,
        /// <summary>
        /// Position legend at bottom.
        /// </summary>
        Bottom,
    }

    /// <summary>
    /// 
    /// </summary>
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
}
