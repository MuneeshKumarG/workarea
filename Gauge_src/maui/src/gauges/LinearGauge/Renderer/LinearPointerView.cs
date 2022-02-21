using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents a view that used to draw <see cref="LinearPointer"/>. 
    /// </summary>
    internal class LinearPointerView : DrawableView
    {
        #region Fields


        /// <summary>
        /// Represents <see cref="LinearPointer"/> instance.
        /// </summary>
        private LinearPointer? linearPointer;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerView"/> class.
        /// </summary>
        /// <param name="pointer">Represents <see cref="LinearPointer"/> instance.</param>
        public LinearPointerView(LinearPointer pointer)
        {
            this.linearPointer = pointer;
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Method used to draw <see cref="RadialPointer"/> visual elememts. 
        /// </summary>
        /// <param name="canvas">Represents rendering canvas.</param>
        /// <param name="dirtyRect">Represents rendering region.</param>
        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);

            if (this.linearPointer != null && this.linearPointer.Scale != null && this.linearPointer.Scale.CanDrawPointer)
            {
                this.linearPointer.Draw(canvas);
            }
        }

        #endregion
    }
}
