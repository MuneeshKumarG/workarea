﻿using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;
using Syncfusion.Maui.Graphics.Internals;
using System.Collections.ObjectModel;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    public partial class Column_RoundedEdges : SampleView
    {
        public Column_RoundedEdges()
        {
            InitializeComponent();
        }
        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart1.Handler?.DisconnectHandler();
        }
        }

    public class RoundedColumnSeries : ColumnSeries
    {
        protected override void DrawSeries(ICanvas canvas, ReadOnlyObservableCollection<ChartSegment> segments, RectF clipRect)
        {
            base.DrawSeries(canvas, segments, clipRect);
            if (ActualXAxis == null || ActualYAxis == null) return;
            var y = ActualYAxis.ValueToPoint(50);

            canvas.SaveState();
            var color = Color.FromArgb("#F06C64");
            var text = "Overflow";
            var size = text.Measure(12);
            var textY = y - (float)size.Height * 2;
            var textX = clipRect.Left + (float)size.Width / 4;

            canvas.StrokeColor = color;
            canvas.StrokeSize = 2;
            canvas.StrokeDashPattern = new float[] { 15, 6, 5, 3 };
            canvas.FontColor = color;
            canvas.FontSize = 15;

            ChartAxisLabelStyle chartAxisLabelStyle = new ChartAxisLabelStyle();
            chartAxisLabelStyle.TextColor = color;
            chartAxisLabelStyle.FontSize = 15;

            canvas.DrawText(text, textX, textY, chartAxisLabelStyle);
            canvas.DrawLine(clipRect.Left, y, clipRect.Right, y);
            canvas.RestoreState();
        }
    }
}
