using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents a view that used to draw <see cref="RadialPointer"/>. 
    /// </summary>
    internal class PointerView : DrawableView
    {
        #region Fields

        /// <summary>
        /// Represents <see cref="RadialPointer"/> instance.
        /// </summary>
        private RadialPointer pointer;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PointerView"/> class.
        /// </summary>
        /// <param name="pointer">Represents <see cref="RadialPointer"/> instance.</param>
        public PointerView(RadialPointer pointer)
        {
            this.pointer = pointer;
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

            if (this.pointer.RadialAxis != null &&
                (!this.pointer.RadialAxis.CanAnimate || this.pointer.AnimationValue != null))
            {
                this.pointer.Draw(canvas);
            }
        }

        #endregion
    }
}
