using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents a view that used to draw <see cref="LinearRange"/>. 
    /// </summary>
    internal class LinearRangeView : DrawableView
    {
        #region Fields

        /// <summary>
        /// Represents <see cref="LinearRange"/> instance.
        /// </summary>
        private LinearRange linearRange;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearRangeView"/> class.
        /// </summary>
        /// <param name="linearRange">Represents <see cref="LinearRange"/> instance.</param>
        public LinearRangeView(LinearRange linearRange)
        {
            this.linearRange = linearRange;
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Method used to draw <see cref="LinearRange"/> visual elememts. 
        /// </summary>
        /// <param name="canvas">Represents rendering canvas.</param>
        /// <param name="dirtyRect">Represents rendering region.</param>
        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);

            canvas.SaveState();

            this.linearRange.DrawRange(canvas);

            canvas.RestoreState();
        }

        #endregion
    }
}
