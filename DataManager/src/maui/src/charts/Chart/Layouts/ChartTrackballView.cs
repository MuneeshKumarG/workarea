using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;

namespace Syncfusion.Maui.Charts.Chart.Layouts
{
    internal class ChartTrackballView : SfView
    {
        public ChartTrackballBehavior? Behavior { get; internal set; }

        public ChartTrackballView()
        {
            DrawingOrder = DrawingOrder.AboveContent;
        }

        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            if (Behavior?.PointInfos.Count > 0)
            {
                canvas.SaveState();
                Behavior.DrawElements(canvas, dirtyRect);
                canvas.RestoreState();
            }

        }
    }
}
