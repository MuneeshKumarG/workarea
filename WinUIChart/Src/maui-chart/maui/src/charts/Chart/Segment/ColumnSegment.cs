using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ColumnSegment : CartesianSegment
    {
        #region Fields

        private double x1, y1, x2, y2, xvalue;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public float Left { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public float Top { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public float Right { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public float Bottom { get; internal set; }

        #endregion

        #region Internal  Properties

        internal float Y1 { get; set; } = float.NaN;
        internal float Y2 { get; set; } = float.NaN;
        internal float PreviousY1 { get; set; } = float.NaN;
        internal float PreviousY2 { get; set; } = float.NaN;

        #endregion

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            ColumnSeries? series = Series as ColumnSeries;

            if (series == null)
            {
                return;
            }

            Layout(series);
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            ColumnSeries? series = Series as ColumnSeries;

            if (series == null)
            {
                return;
            }

            if (series.CanAnimate())
            {
                Layout(series);
            }

            if (!float.IsNaN(Left) && !float.IsNaN(Top) && !float.IsNaN(Right) && !float.IsNaN(Bottom))
            {
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.StrokeColor = Stroke.ToColor();
                canvas.Alpha = Opacity;
                CornerRadius cornerRadius = series.CornerRadius;

                //Drawing segment.
                var rect = new Rect() { Left = Left, Top = Top, Right = Right, Bottom = Bottom };
                canvas.SetFillPaint(Fill, rect);

                if (cornerRadius.TopLeft > 0 || cornerRadius.TopRight > 0 || cornerRadius.BottomLeft > 0 || cornerRadius.BottomRight > 0)
                {
                    canvas.FillRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
                }
                else
                {
                    canvas.FillRectangle(rect);
                }

                //Drawing stroke.
                if (HasStroke)
                {
                    if (cornerRadius.TopLeft > 0 || cornerRadius.TopRight > 0 || cornerRadius.BottomLeft > 0 || cornerRadius.BottomRight > 0)
                    {
                        canvas.DrawRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
                    }
                    else
                    {
                        canvas.DrawRectangle(rect);
                    }
                }
            }
        }

        #endregion

        #region Internal Methods

        internal override void SetData(double[] values)
        {
            ColumnSeries? series = Series as ColumnSeries;

            if (series == null)
            {
                return;
            }

            x1 = values[0];
            x2 = values[1];
            y1 = values[2];
            y2 = values[3];
            xvalue = values[4];

            Empty = double.IsNaN(x1) || double.IsNaN(x2) || double.IsNaN(y1) || double.IsNaN(y2);

            series.XRange += DoubleRange.Union(xvalue);
            series.YRange += new DoubleRange(y1, y2);
        }

        internal void SetPreviousData(float[] values)
        {
            this.PreviousY1 = values[0];
            this.PreviousY2 = values[1];
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (Series != null && SegmentBounds.Contains(x, y))
            {
                return Series.Segments.IndexOf(this);
            }

            return -1;
        }

        internal override void OnDataLabelLayout()
        {
            CalculateDataLabelPosition(xvalue, y1);            
        }

        #endregion

        #region private Methods

        private void Layout(ColumnSeries? series)
        {
            var xAxis = series?.ActualXAxis;

            if (series == null || series.ChartArea == null || xAxis == null)
            {
                return;
            }

            var crossingValue = xAxis.ActualCrossingValue;
            var start = Math.Floor(xAxis.VisibleRange.Start);
            var end = Math.Ceiling(xAxis.VisibleRange.End);
            double y1Value = y1;
            double y2Value = y2;
            var seriesIndex = series.ChartArea.Series?.IndexOf(series);

            if (!double.IsNaN(crossingValue))
            {
                //Helps value specifing for column segments only, hence we have a possibility of inheriting this class for stacking column. 
                if (seriesIndex == 0 || (seriesIndex != 0 && Series is ColumnSeries))
                {
                    y2Value = crossingValue;
                }
            }

            if (series.CanAnimate())
            {
                float animationValue = series.AnimationValue;

                if (!series.XRange.Equals(series.PreviousXRange) || (float.IsNaN(PreviousY1) && float.IsNaN(PreviousY2)))
                {
                    y1Value = y1Value * animationValue;
                    y2Value = y2Value * animationValue;
                }
                else
                {
                    y1Value = GetColumnDynamicAnimationValue(animationValue, PreviousY1, y1);
                    y2Value = GetColumnDynamicAnimationValue(animationValue, PreviousY2, y2);
                }
            }

            Left = Top = Right = Bottom = float.NaN;

            if (x1 <= end && x2 >= start)
            {
                Left = series.TransformToVisibleX(x1, y1Value);
                Top = series.TransformToVisibleY(x1, y1Value);
                Right = series.TransformToVisibleX(x2, y2Value);
                Bottom = series.TransformToVisibleY(x2, y2Value);

                if (Left > Right)
                {
                    var temp = Left;
                    Left = Right;
                    Right = temp;
                }

                if (Top > Bottom)
                {
                    var temp = Top;
                    Top = Bottom;
                    Bottom = temp;
                }

                Y1 = (float)y1Value;
                Y2 = (float)y2Value;
            }
            else
            {
                this.Left = float.NaN;
            }

            SegmentBounds = new RectF(Left, Top, Right - Left, Bottom - Top);
        }

        private float GetColumnDynamicAnimationValue(float animationValue, double oldValue, double currentValue)
        {
            if (!double.IsNaN(oldValue) && !double.IsNaN(currentValue))
            {
                return (float)((currentValue > oldValue) ?
                    oldValue + ((currentValue - oldValue) * animationValue)
                    : oldValue - ((oldValue - currentValue) * animationValue));
            }
            else
            {
                return double.IsNaN(oldValue) ? (float)currentValue * animationValue : (float)(oldValue - (oldValue * animationValue));
            }
        }

        #endregion

        #endregion
    }
}
