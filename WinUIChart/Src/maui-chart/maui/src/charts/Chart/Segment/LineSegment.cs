using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LineSegment : CartesianSegment
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public float X1 { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public float Y1 { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public float X2 { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public float Y2 { get; internal set; }

        internal double X1Pos { get; set; }

        internal double Y1Pos { get; set; }

        internal double X2Pos { get; set; }

        internal double Y2Pos { get; set; }

        internal float PreviousX1 { get; set; } = float.NaN;

        internal float PreviousY1 { get; set; } = float.NaN;

        internal float PreviousX2 { get; set; } = float.NaN;

        internal float PreviousY2 { get; set; } = float.NaN;

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series == null || SeriesView == null || Empty)
            {
                return;
            }

            float x1 = X1;
            float y1 = Y1;
            float x2 = X2;
            float y2 = Y2;

            if (Series.CanAnimate())
            {
                float animationValue = Series.AnimationValue;

                if (Series.IsDataPointAddedDynamically)
                {
                    int index = Series.Segments.IndexOf(this);

                    if (!Series.XRange.Equals(Series.PreviousXRange) && Series.Segments.Count - 1 == index + 1 )
                    {
                        LineSegment? previousSegment = Series.Segments[index - 1] as LineSegment;

                        if (previousSegment != null)
                        {
                            float prevX = previousSegment.X2;
                            float prevY = previousSegment.Y2;
                            x1 = prevX + (x1 - prevX) * animationValue;
                            y1 = prevY + (y1 - prevY) * animationValue;
                            x2 = x1 + (x2 - x1) * animationValue;
                            y2 = y1 + (y2 - y1) * animationValue;
                        }
                    }
                }
                else if (!Series.XRange.Equals(Series.PreviousXRange) || (float.IsNaN(PreviousX1) && float.IsNaN(PreviousY1) && float.IsNaN(PreviousX2) && float.IsNaN(PreviousY2)))
                {
                    AnimateSeriesClipRect(canvas, animationValue);
                }
                else
                {
                    y1 = GetDynamicAnimationValue(animationValue, y1, PreviousY1, Y1);
                    y2 = GetDynamicAnimationValue(animationValue, y2, PreviousY2, Y2);
                    x1 = GetDynamicAnimationValue(animationValue, x1, PreviousX1, X1);
                    x2 = GetDynamicAnimationValue(animationValue, x2, PreviousX2, X2);
                }
            }

            canvas.StrokeSize = (float)StrokeWidth;
            canvas.StrokeColor = Fill.ToColor();
            canvas.Alpha = Opacity;

            if (StrokeDashArray != null && StrokeDashArray.Count > 0)
            {
                canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
            }

            canvas.DrawLine(x1, y1, x2, y2);
        }

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            var series = Series as CartesianSeries;
            if(series == null || series.ActualXAxis == null)
            {
                return;
            }

            var xAxis = series.ActualXAxis;
            var start = Math.Floor(xAxis.VisibleRange.Start);
            var end = Math.Ceiling(xAxis.VisibleRange.End);
            this.X1 = this.Y1 = this.X2 = this.Y2 = float.NaN;

            if ((X1Pos >= start && X1Pos <= end) || (X2Pos >= start && X2Pos <= end) || (start >= X1Pos && start <= X2Pos))
            {
                this.X1 = series.TransformToVisibleX(X1Pos, Y1Pos);
                this.Y1 = series.TransformToVisibleY(X1Pos, Y1Pos);
                this.X2 = series.TransformToVisibleX(X2Pos, Y2Pos);
                this.Y2 = series.TransformToVisibleY(X2Pos, Y2Pos);
            }
        }

        /// <summary>
        /// Sets the values for this segment.
        /// </summary>
        /// <param name="values">The values.</param>
        internal override void SetData(double[] values)
        {
            if(Series == null)
            {
                return;
            }

            this.X1Pos = values[0];
            this.Y1Pos = values[1];
            this.X2Pos = values[2];
            this.Y2Pos = values[3];

            Empty = double.IsNaN(X1Pos) || double.IsNaN(X2Pos) || double.IsNaN(Y1Pos) || double.IsNaN(Y2Pos);

            if (Series.PointsCount == 1)
            {
                this.Series.XRange = new DoubleRange(this.X1Pos, this.X1Pos);
                this.Series.YRange = new DoubleRange(this.Y1Pos, this.Y1Pos);
            }
            else
            {
                this.Series.XRange += new DoubleRange(this.X1Pos, this.X2Pos);
                this.Series.YRange += new DoubleRange(this.Y1Pos, this.Y2Pos);
            }
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (Series != null)
            {
                if (IsRectContains(X1, Y1, x, y, (float)StrokeWidth))
                {
                    return Series.Segments.IndexOf(this);
                }
                else if (Series.Segments.IndexOf(this) == Series.Segments.Count - 1 && IsRectContains(X2, Y2, x, y, (float)StrokeWidth))
                {
                    return Series.Segments.IndexOf(this) + 1;
                }
            }

            return -1;
        }

        internal void SetPreviousData(float[] values)
        {
            this.PreviousX1 = values[0];
            this.PreviousY1 = values[1];
            this.PreviousX2 = values[2];
            this.PreviousY2 = values[3];
        }

        internal override void OnDataLabelLayout()
        {
            CalculateDataLabelPosition(X1Pos, Y1Pos);
        }
        #endregion
    }
}
