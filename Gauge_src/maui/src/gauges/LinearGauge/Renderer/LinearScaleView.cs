using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents a view that used to draw <see cref="SfLinearGauge"/>. 
    /// </summary>
    internal class LinearScaleView : DrawableView
    {
        #region Fields

        /// <summary>
        /// Represents <see cref="SfLinearGauge"/> instance.
        /// </summary>
        private SfLinearGauge linearScale;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearScaleView"/> class.
        /// </summary>
        /// <param name="linearScale">Represents <see cref="SfLinearGauge"/> instance.</param>
        public LinearScaleView(SfLinearGauge linearScale)
        {
            this.linearScale = linearScale;
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Method used to draw <see cref="RadialAxis"/> visual elememts. 
        /// </summary>
        /// <param name="canvas">Represents rendering canvas.</param>
        /// <param name="dirtyRect">Represents rendering region.</param>
        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);

            canvas.SaveState();

            this.linearScale.Draw(canvas);

            canvas.RestoreState();
        }

        #endregion
    }
}
