using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// It used to render or customize the segments for funnel chart.
    /// </summary>
    public class FunnelSegment : ChartSegment, IPyramidLabels
    {
        #region Fields

        private double topRadius, bottomRadius;
        internal IPyramidChartDependent? Chart { get; set; }
        private bool isBroken;
        private RectF actualBounds;
        private float[] values = new float[12];
        #endregion

        #region Properties
        /// <summary>
        /// Represents the draw points of the funnel segments.
        /// </summary>
        /// <remarks>
        /// <para>Funnel segments are often drawn with six points, which are listed below.</para>
        /// Points[0] : Top Left
        /// Points[1] : Top Right
        /// Points[2] : Middle Right
        /// Points[3] : Bottom Right
        /// Points[4] : Bottom Left
        /// Points[5] : Middle Left
        /// </remarks>
        public Point[] Points { get; internal set; } = new Point[6];
        private double top;
        private double bottom;
        private double NeckWidth;
        private double NeckHeight;

        private double dataLabelYValue;
        private object? datalabelXValue;

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

        #region Internal methods

        internal void SetData(double y, double height, double neckWidth, double neckHeight, object? xValue, double yValue)
        {
            top = y;
            bottom = y + height;
            topRadius = y / 2;
            bottomRadius = (y + height) / 2;
            NeckWidth = neckWidth;
            NeckHeight = neckHeight;
            this.datalabelXValue = xValue;
            this.dataLabelYValue = yValue;
            Empty = double.IsNaN(yValue) || yValue == 0;
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (SegmentBounds.Contains(x, y))
            {
                var slopeLeft = Math.Atan((values[8] - values[0]) / (values[9] - values[1]));
                var slopeRight = Math.Atan((values[6] - values[2]) / (values[7] - values[3]));
                var sl = Math.Atan((values[8] - x) / (values[9] - y));
                var sr = Math.Atan((values[6] - x) / (values[7] - y));

                if (isBroken)
                {
                    if (values[5] <= y)
                    {
                        if ((values[4] >= x) && (values[10] <= x))
                        {
                            return Index;
                        }
                    }
                    else
                    {
                        slopeLeft = Math.Atan((values[10] - values[0]) / (values[11] - values[1]));
                        slopeRight = Math.Atan((values[4] - values[2]) / (values[5] - values[3]));
                        sl = Math.Atan((values[10] - x) / (values[11] - y));
                        sr = Math.Atan((values[4] - x) / (values[5] - y));
                        if (sl <= slopeLeft && sr >= slopeRight)
                        {
                            return Index;
                        }
                    }
                }
                else if (sl <= slopeLeft && sr >= slopeRight)
                {
                    return Index;
                }
            }

            return -1;
        }

        #endregion

        #region Protected internal methods 

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
                double minWidth = 0.5f * (1 - (NeckWidth / boundsWidth));
                double bottomY = minWidth * (this.bottom - this.top) / (bottomRadius - topRadius);
                double top = Math.Min(topRadius, minWidth);
                double bottom = Math.Min(bottomRadius, minWidth);
                isBroken = (topRadius >= minWidth) ^ (bottomRadius > minWidth);

                values = new float[12];
                values[0] = (float)(boundsLeft + (top * boundsWidth));
                values[1] = values[3] = (float)(boundsTop + (this.top * boundsHeight));
                values[2] = (float)(boundsLeft + ((float)(1 - top) * boundsWidth));
                values[6] = (float)(boundsLeft + ((float)(1 - bottom) * boundsWidth));
                values[7] = values[9] = (float)(boundsTop + (this.bottom * boundsHeight));
                values[4] = isBroken ? (float)(boundsLeft + ((float)(1 - minWidth) * boundsWidth)) : (values[2]);
                values[5] = values[11] = isBroken ? ((float)(boundsTop + (bottomY * boundsHeight))) : (values[3]);
                values[8] = (float)(boundsLeft + ((float)bottom * boundsWidth));
                values[10] = isBroken? (float)(boundsLeft + ((float)minWidth * boundsWidth)) : values[0];

                Points[0] = new Point(values[0], values[1]);
                Points[1] = new Point(values[2], values[3]);
                Points[2] = new Point(values[4], values[5]);
                Points[3] = new Point(values[6], values[7]);
                Points[4] = new Point(values[8], values[9]);
                Points[5] = new Point(values[10], values[11]);

                SegmentBounds = new RectF(values[0], values[1], values[2] - values[0], values[7] - values[1]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        protected internal override void Draw(ICanvas canvas)
        {
            PathF path = new PathF();

            if (values == null)
            {
                return;
            }

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

        internal override void OnDataLabelLayout()
        {
            if (Chart != null && !Empty && Chart.DataLabelSettings.LabelStyle is ChartLabelStyle style)
            {
                var bounds = Chart.SeriesBounds;
                labelXPosition = (float)bounds.X + SegmentBounds.Center.X;
                labelYPosition = (float)bounds.Y + SegmentBounds.Center.Y;

                DataLabel = Chart.DataLabelSettings.GetLabelContent(datalabelXValue, dataLabelYValue);
                double left;
                double right;
                
                if (isBroken)
                {
                    right = (values[5] + bounds.Y > labelYPosition ? ((values[4] + values[2]) / 2) : ((values[4] + values[6]) / 2)) + bounds.Left;
                    left = (values[11] + bounds.Y > labelYPosition ? ((values[10] + values[0]) / 2) : ((values[10] + values[8]) / 2)) + bounds.Left;
                }
                else
                {
                    right = ((values[2] + values[6]) / 2) + bounds.Left;
                    left = ((values[0] + values[8]) / 2) + bounds.Left;
                }

                var y = ((values[3] + values[7]) / 2) + bounds.Top;

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

        #endregion

    }
}
