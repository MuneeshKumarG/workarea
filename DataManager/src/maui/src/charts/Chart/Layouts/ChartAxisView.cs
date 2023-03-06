using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    internal class ChartAxisView : SfDrawableView
    {
        internal CartesianChartArea Area { get; set; }

        public ChartAxisView(CartesianChartArea area)
        {
            Area = area;
        }

        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            var axisLayout = Area.AxisLayout;
            OnDrawAxis(canvas, axisLayout.HorizontalAxes);
            OnDrawAxis(canvas, axisLayout.VerticalAxes);

            canvas.RestoreState();
        }

        private void OnDrawAxis(ICanvas canvas, ObservableCollection<ChartAxis>? axes)
        {
            if (axes == null) return;

            foreach (ChartAxis chartAxis in axes)
            {
                Rect arrangeRect = chartAxis.ArrangeRect;
                if (arrangeRect != Rect.Zero)
                {
                    canvas.SaveState();

                    chartAxis.DrawAxis(canvas, arrangeRect);

                    canvas.RestoreState();
                }
            }
        }
    }
}
