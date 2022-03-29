using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Charts
{
    internal class DataLabelView : DrawableView
    {
        readonly ChartPlotArea chartPlotArea;

        public DataLabelView(ChartPlotArea plotArea)
        {
            chartPlotArea = plotArea;
        }

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            var visibleSeries = chartPlotArea.VisibleSeries;    

            if (visibleSeries != null)
            {
                foreach (var series in visibleSeries)
                {
                    if (series.IsVisible && !series.CanAnimate() && series.ShowDataLabels && series.Segments.Count > 0)
                    {
                        canvas.SaveState();

                        if (series.NeedToAnimateDataLabel)
                        {
                            canvas.Alpha = series.AnimationValue;
                        }

                        series.DrawDataLabels(canvas);
                        canvas.RestoreState();
                    }
                }
            }
        }
    }
}