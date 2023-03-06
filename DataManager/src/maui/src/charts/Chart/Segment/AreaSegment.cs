using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents a segment of an area chart.
    /// </summary>
    public partial class AreaSegment : CartesianSegment, IMarkerDependentSegment
    {
        #region Properties

        /// <summary>
        /// Gets the data point values from the series that are bound with x for the segment.
        /// </summary>
        public double[]? XValues { get; internal set; }

        /// <summary>
        /// Gets the data point values from the series that are bound with y for the segment.
        /// </summary>
        public double[]? YValues { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        internal List<float>? FillPoints { get; set; }

        internal List<float>? StrokePoints { get; set; }

        internal List<float>? PreviousFillPoints { get; set; } = null;

        internal List<float>? PreviousStrokePoints { get; set; } = null;

        private PathF? path;

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Empty)
            {
                return;
            }

            canvas.Alpha = Opacity;

            if (HasStroke)
            {
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.StrokeColor = Stroke.ToColor();
            }

            if (StrokeDashArray != null)
            {
                canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
            }

            DrawPath(canvas, FillPoints, StrokePoints);
        }

        private void DrawPath(ICanvas canvas, List<float>? fillPoints, List<float>? strokePoints)
        {
            if (Series == null) return;

            path = new PathF();

            float animationValue = Series.AnimationValue;
            bool isDynamicAnimation = Series.CanAnimate() && Series.XRange.Equals(Series.PreviousXRange) && PreviousFillPoints != null && fillPoints != null && fillPoints.Count == PreviousFillPoints.Count;

            if (Series.CanAnimate() && !isDynamicAnimation)
            {
                AnimateSeriesClipRect(canvas, animationValue);
            }

            if (fillPoints != null)
            {
                for (int i = 0; i < fillPoints.Count; i++)
                {
                    var x = fillPoints[i];
                    var y = fillPoints[i + 1];

                    if (isDynamicAnimation && PreviousFillPoints != null)
                    {
                        x = GetDynamicAnimationValue(animationValue, x, PreviousFillPoints[i], x);
                        y = GetDynamicAnimationValue(animationValue, y, PreviousFillPoints[i + 1], y);
                    }

                    if (i == 0)
                    {
                        path.MoveTo(x, y);
                    }
                    else
                    {
                        path.LineTo(x, y);
                    }

                    i++;
                }

                canvas.SetFillPaint(Fill, path.Bounds);
                canvas.FillPath(path);
            }

            if (HasStroke && strokePoints != null)
            {
                path = new PathF();
                for (int i = 0; i < strokePoints.Count; i++)
                {
                    var x = strokePoints[i];
                    var y = strokePoints[i + 1];

                    if (isDynamicAnimation && PreviousStrokePoints != null)
                    {
                        x = GetDynamicAnimationValue(animationValue, x, PreviousStrokePoints[i], x);
                        y = GetDynamicAnimationValue(animationValue, y, PreviousStrokePoints[i + 1], y);
                    }

                    if (i == 0)
                    {
                        path.MoveTo(x, y);
                    }
                    else
                    {
                        path.LineTo(x, y);
                    }

                    i++;
                }

                canvas.DrawPath(path);
            }
        }

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            if (XValues == null)
            {
                return;
            }

            this.CalculateInteriorPoints();

            if (HasStroke)
            {
                this.CalculateStrokePoints();
            }
        }

        /// <summary>
        /// Calculate interior points.
        /// </summary>
        private void CalculateInteriorPoints()
        {
            var cartesian = Series as CartesianSeries;
            if (cartesian != null && cartesian.ActualXAxis != null && XValues != null && YValues != null)
            {
                var crossingValue = cartesian.ActualXAxis.ActualCrossingValue;
                var count = XValues.Length;
                this.FillPoints = new List<float>();
                double yValue = this.YValues[0], xValue = this.XValues[0];
                crossingValue = double.IsNaN(crossingValue) ? 0 : crossingValue;
                this.FillPoints.Add(cartesian.TransformToVisibleX(xValue, 0));
                FillPoints.Add(cartesian.TransformToVisibleY(xValue, crossingValue));

                for (int i = 0; i < count; i++)
                {
                    xValue = this.XValues[i];
                    yValue = this.YValues[i];
                    this.FillPoints.Add(cartesian.TransformToVisibleX(xValue, yValue));
                    this.FillPoints.Add(cartesian.TransformToVisibleY(xValue, yValue));
                }

                xValue = this.XValues[count - 1];
                this.FillPoints.Add(cartesian.TransformToVisibleX(xValue, 0));
                FillPoints.Add(cartesian.TransformToVisibleY(xValue, crossingValue));
            }
        }

        /// <summary>
        /// Calculate stroke points.
        /// </summary>
        private void CalculateStrokePoints()
        {
            if (Series != null && XValues != null && YValues != null)
            {
                float x, y;
                var halfStrokeWidth = (float)StrokeWidth / 2;
                this.StrokePoints = new List<float>();
                double yValue;
                var count = XValues.Length;
                for (int i = 0; i < count; i++)
                {
                    yValue = this.YValues[i];
                    x = this.Series.TransformToVisibleX(this.XValues[i], yValue);
                    y = this.Series.TransformToVisibleY(this.XValues[i], yValue);
                    this.StrokePoints.Add(x);
                    this.StrokePoints.Add(y += yValue >= 0 ? halfStrokeWidth : -halfStrokeWidth);
                }
            }
        }

        /// <summary>
        /// Sets the values for this segment.
        /// </summary>
        /// <param name="xValues">The x values.</param>
        /// <param name="yValues">The y values.</param>
        internal override void SetData(IList xValues, IList yValues)
        {
            var series = Series as CartesianSeries;
            if (series != null && series.ActualYAxis != null)
            {

                var count = xValues.Count;
                this.YValues = new double[count];
                this.XValues = new double[count];
                xValues.CopyTo(this.XValues, 0);
                yValues.CopyTo(this.YValues, 0);

                var yMin = this.YValues.Min();
                yMin = double.IsNaN(yMin) ? this.YValues.Any() ? this.YValues.Where(e => !double.IsNaN(e)).DefaultIfEmpty().Min() : 0 : yMin;
                yMin = yMin == 0 ? series.ActualYAxis.VisibleRange.Start : yMin;

                Empty = double.IsNaN(yMin);

                series.XRange += new DoubleRange(this.XValues.Min(), this.XValues.Max());
                series.YRange += new DoubleRange(yMin, this.YValues.Max());
            }
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            return -1;
        }

        Rect IMarkerDependentSegment.GetMarkerRect(double markerWidth, double markerHeight, int tooltipIndex)
        {
            Rect rect = new Rect();
            if (Series != null && FillPoints != null)
            {
                if (XValues?.Length > tooltipIndex)
                {
                    var xIndex = (2 * tooltipIndex) + 2;
                    rect = new Rect(FillPoints[xIndex] - (markerWidth / 2), FillPoints[xIndex + 1] - (markerHeight / 2), markerWidth, markerHeight);
                }
            }
            
            return rect;
        }

        void IMarkerDependentSegment.DrawMarker(ICanvas canvas)
        {
            if (Series is IMarkerDependent series && FillPoints != null)
            {
                var marker = series.MarkerSettings;
                var fill = marker.Fill;
                var type = marker.Type;
                for (int i = 2; i < FillPoints.Count - 3; i += 2)
                {
                    var rect = new Rect(FillPoints[i] - (marker.Width / 2), FillPoints[i + 1] - (marker.Height / 2), marker.Width, marker.Height);

                    canvas.SetFillPaint(fill == default(Brush) ? this.Fill : fill, rect);

                    series.DrawMarker(canvas, Index, type, rect);
                }
            }
        }

        #endregion
    }
}
