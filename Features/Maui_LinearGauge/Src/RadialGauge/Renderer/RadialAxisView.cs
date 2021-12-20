using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents a view that used to draw <see cref="RadialAxis"/>. 
    /// </summary>
    internal class RadialAxisView : DrawableView
    {
        #region Fields

        /// <summary>
        /// Represents <see cref="RadialAxis"/> instance.
        /// </summary>
        private RadialAxis radialAxis;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialAxisView"/> class.
        /// </summary>
        /// <param name="radialAxis">Represents <see cref="RadialAxis"/> instance.</param>
        public RadialAxisView(RadialAxis radialAxis)
        {
            this.radialAxis = radialAxis;
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

            if (this.radialAxis.CanAnimate && !AnimationExtensions.AnimationIsRunning(this.radialAxis, "GaugeLoadingAnimation"))
            {
                //Animate axis line in load time.
                this.radialAxis.PerformLoadingAnimation();
            }
            else if (this.radialAxis.ShowAxisLine)
            {
                this.radialAxis.DrawAxisLine(canvas);
            }

            if (!this.radialAxis.CanAnimate || this.radialAxis.AxisLoadingAnimationValue != null)
            {
                if (this.radialAxis.ShowTicks)
                {
                    this.radialAxis.DrawMajorTicks(canvas);
                    this.radialAxis.DrawMinorTicks(canvas);
                }

                if (this.radialAxis.ShowLabels)
                {
                    this.radialAxis.DrawAxisLabels(canvas);
                }
            }
            canvas.RestoreState();
        }

        #endregion
    }
}
