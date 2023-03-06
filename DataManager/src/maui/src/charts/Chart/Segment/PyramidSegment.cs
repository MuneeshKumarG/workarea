using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// It used to render or customize the segments for pyramid chart.
    /// </summary>
    public class PyramidSegment : ChartSegment , IPyramidLabels
    {
        #region Fields
        internal IPyramidChartDependent? Chart { get; set; }
        private RectF actualBounds;
        private float[] values = new float[8];
        #endregion

        #region Properties
        /// <summary>
        /// Represents the draw points of the pyramid segments.
        /// </summary>
        /// <remarks>
        /// <para>Pyramid segments are often drawn with four points, which are listed below.</para>
        /// Points[0] : Top Left
        /// Points[1] : Top Right
        /// Points[2] : Bottom Right
        /// Points[3] : Bottom Left
        /// </remarks>
        public Point[] Points { get; internal set; } = new Point[4];

        private double yValue;
        private double height;

        private double dataLabelYValue;
        private object? dataLabelXValue;
        private Rect labelRect;
        private float labelXPosition;
        private float labelYPosition;
        private Point[]? linePoints = new Point[3];
        private bool isIntersected = false;
        private bool isLabelVisible = true;
        private Size dataLabelSize = Size.Zero;
        private Size actualSize = Size.Zero;
        private Point slopeCenter = Point.Zero;
        private DataLabelPlacement position = DataLabelPlacement.Auto;
        float IPyramidLabels.DataLabelX { get => labelXPosition; set => labelXPosition = value; }
        float IPyramidLabels.DataLabelY { get => labelYPosition; set => labelYPosition = value; }
        Point[]? IPyramidLabels.LinePoints { get => linePoints; set => linePoints = value; }
        Rect IPyramidLabels.LabelRect { get => labelRect; set => labelRect = value; }
        Size IPyramidLabels.DataLabelSize { get => dataLabelSize; set => dataLabelSize = value; }
        Size IPyramidLabels.ActualLabelSize { get => actualSize; set => actualSize = value; }
        string IPyramidLabels.DataLabel => DataLabel ?? string.Empty;
        bool IPyramidLabels.IsEmpty { get => Empty; set => Empty = value; }
        bool IPyramidLabels.IsIntersected { get => isIntersected; set => isIntersected = value; }
        bool IPyramidLabels.IsLabelVisible { get => isLabelVisible; set => isLabelVisible = value; }
        Brush IPyramidLabels.Fill => Fill ?? SolidColorBrush.Transparent;
        Point IPyramidLabels.SlopePoint => slopeCenter;
        DataLabelPlacement IPyramidLabels.Position { get => position; set => position = value; }
        #endregion

        #region Methods

        #region Internal Methods

        internal void SetData(double x, double y, object? xValue, double yValue)
        {
            this.yValue = x;
            height = y;
            this.dataLabelYValue = yValue;
            this.dataLabelXValue = xValue;
            Empty = double.IsNaN(yValue) || yValue == 0;
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (SegmentBounds.Contains(x, y))
            {
                var slopeLeft = Math.Atan((values[6] - values[0]) / (values[7] - values[1]));
                var slopeRight = Math.Atan((values[4] - values[2]) / (values[5] - values[3]));
                if ((Math.Atan((values[6] - x) / (values[7] - y)) <= slopeLeft && slopeRight <= (Math.Atan((values[4] - x) / (values[5] - y)))))
                {
                    return Index;
                }
            }

            return -1;
        }

        #endregion

        #region Protected Internal methods

        /// <summary>
        /// 
        /// </summary>
        protected internal override void OnLayout()
        {
            if (Chart != null)
            {
                actualBounds = new RectF(new Point(0, 0), Chart.SeriesBounds.Size);
                double boundsHeight = actualBounds.Height;
                double boundsWidth = actualBounds.Width;
                double boundsTop = actualBounds.Top;
                double boundsLeft = actualBounds.Left;
                float top = (float)yValue;
                float bottom = (float)(yValue + height);
                float topRadius = 0.5f * (1 - (float)yValue);
                float bottomRadius = 0.5f * (1 - bottom);

                values = new float[8];
                values[0] = (float)(boundsLeft + (topRadius * boundsWidth));
                values[1] = values[3] = (float)(boundsTop + (top * boundsHeight));
                values[2] = (float)(boundsLeft + ((1 - topRadius) * boundsWidth));
                values[4] = (float)(boundsLeft + ((1 - bottomRadius) * boundsWidth));
                values[5] = values[7] = (float)(boundsTop + (bottom * boundsHeight));
                values[6] = (float)(boundsLeft + (bottomRadius * boundsWidth));

                Points[0] = new Point(values[0], values[1]);
                Points[1] = new Point(values[2], values[3]);
                Points[2] = new Point(values[4], values[5]);
                Points[3] = new Point(values[6], values[7]);

                SegmentBounds = new RectF(values[6], values[1], values[4] - values[6], values[5] - values[1]);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        protected internal override void Draw(ICanvas canvas)
        {
            PathF path = new PathF();
            path.MoveTo(Points[0]);
            for (int i = 1; i < Points.Count(); i++)
            {
                path.LineTo(Points[i]);
            }

            path.Close();
            canvas.SetFillPaint(Fill, path.Bounds);
            canvas.FillPath(path);

            if (HasStroke)
            {
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.DrawPath(path);
            }
        }

        #endregion

        #endregion

        #region Data label

        internal override void OnDataLabelLayout()
        {
            if (Chart != null && !Empty && Chart.DataLabelSettings.LabelStyle is ChartLabelStyle style)
            {
                labelXPosition = (float)Chart.SeriesBounds.X + SegmentBounds.Center.X;
                labelYPosition = (float)Chart.SeriesBounds.Y + SegmentBounds.Center.Y;


                DataLabel = Chart.DataLabelSettings.GetLabelContent(dataLabelXValue, dataLabelYValue);
                var left = ((values[0] + values[6]) / 2) + Chart.SeriesBounds.Left;
                var right = ((values[2] + values[4]) / 2) + Chart.SeriesBounds.Left;
                var y = ((values[3] + values[5]) / 2) + Chart.SeriesBounds.Top;
                slopeCenter = new Point(right - 3, y);

                dataLabelSize = DataLabel.Measure(style);
                actualSize = new Size(dataLabelSize.Width + style.Margin.HorizontalThickness, dataLabelSize.Height + style.Margin.VerticalThickness);

                this.position = DataLabelPlacement.Inner;
                switch (Chart.DataLabelSettings.LabelPlacement)
                {
                    case DataLabelPlacement.Center:
                    case DataLabelPlacement.Auto:
                        if (actualSize.Width > right - left || actualSize.Height > SegmentBounds.Height)
                            this.position = DataLabelPlacement.Outer;
                        break;
                    case DataLabelPlacement.Outer:
                        this.position = DataLabelPlacement.Outer;
                        break;
                }

                Chart.DataLabelHelper.AddLabel(this);
            }
        }

        #endregion
    }
}