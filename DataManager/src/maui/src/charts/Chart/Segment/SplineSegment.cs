﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents a segment of a spline chart.
    /// </summary>
    public partial class SplineSegment : CartesianSegment, IMarkerDependentSegment
    {
        #region Properties

        /// <summary>
        /// Gets the x-value for the starting point of the spline segment.
        /// </summary>
        public float X1 { get; internal set; }

        /// <summary>
        /// Gets the y-value for the starting point of the spline segment.
        /// </summary>
        public float Y1 { get; internal set; }

        /// <summary>
        /// Gets the x-value for the ending point of the spline segment.
        /// </summary>
        public float X2 { get; internal set; }

        /// <summary>
        /// Gets the y-value for the ending point of the spline segment.
        /// </summary>
        public float Y2 { get; internal set; }

        internal double StartPtX { get; set; }

        internal double StartPtY { get; set; }

        internal double StartControlPtX { get; set; }

        internal double StartControlPtY { get; set; }

        internal double EndControlPtX { get; set; }

        internal double EndControlPtY { get; set; }

        internal double EndPtX { get; set; }

        internal double EndPtY { get; set; }

        internal float StartControlX { get; set; }

        internal float StartControlY { get; set; }

        internal float EndControlX { get; set; }

        internal float EndControlY { get; set; }

        internal float PreviousX1 { get; set; } = float.NaN;
        internal float PreviousY1 { get; set; } = float.NaN;
        internal float PreviousX2 { get; set; } = float.NaN;
        internal float PreviousY2 { get; set; } = float.NaN;
        internal float PreviousStartControlX { get; set; } = float.NaN;
        internal float PreviousStartControlY { get; set; } = float.NaN;
        internal float PreviousEndControlX { get; set; } = float.NaN;
        internal float PreviousEndControlY { get; set; } = float.NaN;
        #endregion

        #region Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            var series = Series as CartesianSeries;
            if (series == null || series.ActualXAxis == null)
            {
                return;
            }

            var xAxis = series.ActualXAxis;
            X1 = Y1 = X2 = Y2 = StartControlX = StartControlY = EndControlX = EndControlY = float.NaN;

            X1 = series.TransformToVisibleX(StartPtX, StartPtY);
            Y1 = series.TransformToVisibleY(StartPtX, StartPtY);
            X2 = series.TransformToVisibleX(EndPtX, EndPtY);
            Y2 = series.TransformToVisibleY(EndPtX, EndPtY);

            StartControlX = series.TransformToVisibleX(StartControlPtX, StartControlPtY);
            StartControlY = series.TransformToVisibleY(StartControlPtX, StartControlPtY);
            EndControlX = series.TransformToVisibleX(EndControlPtX, EndControlPtY);
            EndControlY = series.TransformToVisibleY(EndControlPtX, EndControlPtY);
        }

        /// <summary>
        /// Sets the values for this segment.
        /// </summary>
        /// <param name="values">The values.</param>
        internal override void SetData(double[] values)
        {
            var series = Series as CartesianSeries;
            if (series == null)
            {
                return;
            }

            StartPtX = values[0];
            StartPtY = values[1];
            StartControlPtX = values[2];
            StartControlPtY = values[3];
            EndControlPtX = values[4];
            EndControlPtY = values[5];
            EndPtX = values[6];
            EndPtY = values[7];

            Empty = double.IsNaN(StartPtX) || double.IsNaN(StartPtY) || double.IsNaN(EndPtX) || double.IsNaN(EndPtY);

            var xMin = Math.Min(Math.Min(StartPtX, EndPtX), Math.Min(StartControlPtX, EndControlPtX));
            var xMax = Math.Max(Math.Max(StartPtX, EndPtX), Math.Max(StartControlPtX, EndControlPtX));
            var yMin = Math.Min(Math.Min(StartPtY, EndPtY), Math.Min(StartControlPtY, EndControlPtY));
            var yMax = Math.Max(Math.Max(StartPtY, EndPtY), Math.Max(StartControlPtY, EndControlPtY));

            if (series.PointsCount == 1)
            {
                series.XRange = new DoubleRange(StartPtX, StartPtX);
                series.YRange = new DoubleRange(StartPtY, StartPtY);
            }
            else
            {
                series.XRange += new DoubleRange(xMin, xMax);
                series.YRange += new DoubleRange(yMin, yMax);
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

        internal override void OnDataLabelLayout()
        {
            CalculateDataLabelPosition(StartPtX, StartPtY, StartPtY);
        }
        
        Rect IMarkerDependentSegment.GetMarkerRect(double markerWidth, double markerHeight, int tooltipIndex)
        {
            Rect rect = new Rect(this.X1 - (markerWidth / 2), this.Y1 - (markerHeight / 2), markerWidth, markerHeight);
            return rect;
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series == null || Empty)
            {
                return;
            }

            float x1 = X1;
            float y1 = Y1;
            float x2 = X2;
            float y2 = Y2;
            float startControlX = StartControlX;
            float startControlY = StartControlY;
            float endControlX = EndControlX;
            float endControlY = EndControlY;

            if (Series.CanAnimate())
            {
                float animationValue = Series.AnimationValue;

                //Todo: Need to dynamically adding single data point animation. 
                if (!Series.XRange.Equals(Series.PreviousXRange) || (float.IsNaN(PreviousX1) && float.IsNaN(PreviousY1) && float.IsNaN(PreviousX2) && float.IsNaN(PreviousY2)))
                {
                    AnimateSeriesClipRect(canvas, animationValue);
                }
                else
                {
                    y1 = GetDynamicAnimationValue(animationValue, y1, PreviousY1, Y1);
                    y2 = GetDynamicAnimationValue(animationValue, y2, PreviousY2, Y2);
                    x1 = GetDynamicAnimationValue(animationValue, x1, PreviousX1, X1);
                    x2 = GetDynamicAnimationValue(animationValue, x2, PreviousX2, X2);
                    startControlX = GetDynamicAnimationValue(animationValue, startControlX, PreviousStartControlX, StartControlX);
                    startControlY = GetDynamicAnimationValue(animationValue, startControlY, PreviousStartControlY, StartControlY);
                    endControlX = GetDynamicAnimationValue(animationValue, endControlX, PreviousEndControlX, EndControlX);
                    endControlY = GetDynamicAnimationValue(animationValue, endControlY, PreviousEndControlY, EndControlY);
                }
            }

            canvas.StrokeSize = (float)StrokeWidth;
            canvas.StrokeColor = Fill?.ToColor();
            canvas.Alpha = Opacity;

            if (StrokeDashArray != null)
            {
                canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
            }

            DrawQuad(canvas, new PointF(x1, y1), new PointF(x2, y2), new PointF(startControlX, startControlY), new PointF(endControlX, endControlY));
        }

        internal void SetPreviousData(float[] values)
        {
            this.PreviousX1 = values[0];
            this.PreviousY1 = values[1];
            this.PreviousX2 = values[2];
            this.PreviousY2 = values[3];
            this.PreviousStartControlX = values[4];
            this.PreviousStartControlY = values[5];
            this.PreviousEndControlX = values[6];
            this.PreviousEndControlY = values[7];
        }

        private void DrawQuad(ICanvas canvas, PointF startPoint, PointF endPoint, PointF controlStartPoint, PointF controlEndPoint)
        {
            var path = new PathF();

            path.MoveTo(startPoint.X, startPoint.Y);

            path.CurveTo(controlStartPoint.X, controlStartPoint.Y, controlEndPoint.X, controlEndPoint.Y, endPoint.X, endPoint.Y);
            
            canvas.DrawPath(path);
        }

        void IMarkerDependentSegment.DrawMarker(ICanvas canvas)
        {
            if (Series is IMarkerDependent series)
            {
                var marker = series.MarkerSettings;
                var fill = marker.Fill;
                var type = marker.Type;
                var rect = new Rect(X1 - (marker.Width / 2), Y1 - (marker.Height / 2), marker.Width, marker.Height);

                canvas.SetFillPaint(fill == default(Brush) ? this.Fill : fill, rect);

                series.DrawMarker(canvas, Index, type, rect);
            }
        }

        #endregion
    }
}
