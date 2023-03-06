using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents a segment of a <see cref="BubbleSeries"/> type charts.
    /// </summary>
    public partial class BubbleSegment : CartesianSegment
    {
        #region Fields
        private double x, y, sizeValue;

        #endregion

        #region Properties

        #region Public Proerties
        
        /// <summary>
        /// Gets the X-value of the bubble center.
        /// </summary>
        public float CenterX { get; internal set; }

        /// <summary>
        /// Gets the Y-value of the bubble center
        /// </summary>
        public float CenterY { get; internal set; }

        /// <summary>
        /// Gets the radius of the bubble. 
        /// </summary>
        public float Radius { get; internal set; }

        #endregion

        #region Internal Properties
        internal float PreviousCenterX { get; set; } = float.NaN;

        internal float PreviousCenterY { get; set; } = float.NaN;

        internal float PreviousRadius { get; set; } = float.NaN;

        #endregion

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            var series = Series as BubbleSeries;

            if (series == null || series.ActualXAxis == null)
            {
                return;
            }

            var xAxis = series.ActualXAxis;
            var start = Math.Floor(xAxis.VisibleRange.Start);
            var end = Math.Ceiling(xAxis.VisibleRange.End);

            CenterX = CenterY = float.NaN;

            if (x <= end && x >= start)
            {
                CenterX = series.TransformToVisibleX(x, y);
                CenterY = series.TransformToVisibleY(x, y);
            }

            SegmentBounds = new RectF((float.IsNaN(CenterX) ? 0 : CenterX) - Radius,
                (float.IsNaN(CenterY) ? 0 : CenterY) - Radius, Radius * 2, Radius * 2);
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series == null || Empty)
            {
                return;
            }

            float radius = Radius;
            float centerX = CenterX;
            float centerY = CenterY;

            if (Series.CanAnimate())
            {
                float animationValue = Series.AnimationValue;

                if (!Series.XRange.Equals(Series.PreviousXRange) || (float.IsNaN(PreviousCenterX) && float.IsNaN(PreviousCenterY) && float.IsNaN(PreviousRadius)))
                {
                    radius *= animationValue;
                }
                else
                {
                    centerX = GetDynamicAnimationValue(animationValue, centerX, PreviousCenterX, CenterX);
                    centerY = GetDynamicAnimationValue(animationValue, centerY, PreviousCenterY, CenterY);
                    radius = GetDynamicAnimationValue(animationValue, radius, PreviousRadius, Radius);
                }
            }

            canvas.SetFillPaint(Fill, SegmentBounds);
            canvas.Alpha = Opacity;
            canvas.FillCircle(centerX, centerY, radius);

            if (HasStroke)
            {
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.StrokeColor = Stroke.ToColor();
                canvas.DrawCircle(centerX, centerY, radius);
            }           
        }

        /// <summary>
        /// Sets the values for this segment.
        /// </summary>
        /// <param name="values"></param>
        internal override void SetData(double[] values)
        {
            this.x = values[0];
            this.y = values[1];
            this.sizeValue = values[2];
            this.Radius = (float)values[3];

            Empty = double.IsNaN(x) || double.IsNaN(y) || double.IsNaN(sizeValue) || double.IsNaN(Radius);

            if (Series != null)
            {
                Series.XRange += !double.IsNaN(this.x) ? DoubleRange.Union(x) : DoubleRange.Empty;
                Series.YRange += !double.IsNaN(this.y) ? DoubleRange.Union(y) : DoubleRange.Empty;
            }
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            double xPoint = x - CenterX;
            double yPoint = y - CenterY;
            double pointLength = Math.Sqrt((xPoint * xPoint) + (yPoint * yPoint));

            if (Series != null)
            {
                if (pointLength <= Radius + StrokeWidth)
                {
                    return Series.Segments.IndexOf(this);
                }
            }
            return -1;
        }

        internal void SetPreviousData(float[] values)
        {
            this.PreviousCenterX = values[0];
            this.PreviousCenterY = values[1];
            this.PreviousRadius = values[2];
        }

        internal override void OnDataLabelLayout()
        {
            CalculateDataLabelPosition(x, y, y);
        }

        #endregion

    }
}
