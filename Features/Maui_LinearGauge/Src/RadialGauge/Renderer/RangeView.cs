using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents a view that used to draw <see cref="RadialRange"/>. 
    /// </summary>
    internal class RangeView : DrawableView
    {
        #region Fields

        /// <summary>
        /// Represents <see cref="RadialRange"/> instance.
        /// </summary>
        private RadialRange range;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeView"/> class.
        /// </summary>
        /// <param name="range">Represents <see cref="RadialRange"/> instance.</param>
        public RangeView(RadialRange range)
        {
            this.range = range;
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Method used to draw <see cref="RadialRange"/> visual elememts. 
        /// </summary>
        /// <param name="canvas">Represents rendering canvas.</param>
        /// <param name="dirtyRect">Represents rendering region.</param>
        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);

            this.range.DrawRange(canvas);

            this.range.DrawLabel(canvas);
        }

        #endregion
    }
}
