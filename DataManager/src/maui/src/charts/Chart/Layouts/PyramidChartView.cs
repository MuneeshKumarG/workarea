using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    internal class PyramidChartView : SfDrawableView
    {
        #region Field

        internal readonly IPyramidChartDependent chart;

        #endregion

        #region Constructor

        internal PyramidChartView(IPyramidChartDependent charts)
        {
            this.chart = charts;
        }

        #endregion

        #region Override method

        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            canvas.ClipRectangle(dirtyRect);
            var bounds = chart.SeriesBounds;
            canvas.Translate((float)bounds.X, (float)bounds.Y);

            foreach (var segment in chart.Segments)
            {
                segment.Draw(canvas);
            }

            canvas.RestoreState();
        }

        #endregion
    }
}
