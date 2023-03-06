using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents a segment of a pie chart.
    /// </summary>
    public partial class PieSegment : ChartSegment
    {
        #region Fields

        private RectF actualBounds, currentBounds;

        private RectF innerRect;

        //Space between connector line edge and data label.
        const float labelGap = 4;

        const double connectorLength = 0.15;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the start angle of the pie segment.
        /// </summary>
        public double StartAngle { get; internal set; }

        /// <summary>
        /// Gets the end angle of the pie segment.
        /// </summary>
        public double EndAngle { get; internal set; }

        /// <summary>
        /// Gets the y-value of the pie segment.
        /// </summary>
        public double YValue { get; internal set; }

        /// <summary>
        /// Gets the radius of the pie segment.
        /// </summary>
        public float Radius { get; internal set; }

        internal bool Exploded { get; set; }

        internal double MidAngle { get; set; }

        internal double PreviousStartAngle { get; set; } = double.NaN;

        internal double PreviousEndAngle { get; set; } = double.NaN;

        internal Rect LabelRect { get; set; }

        internal bool IsLeft { get; set; } = false;

        internal RenderingPosition RenderingPosition { get; set; }

        internal string TrimmedText { get; set; } = string.Empty;

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            var pieSeries = Series as PieSeries;
            if (double.IsNaN(YValue) || pieSeries == null) return;

            if (Index == 0)
            {
                pieSeries.ActualRadius = pieSeries.GetRadius();
                pieSeries.Center = pieSeries.GetCenter();
            }

            var minSize = Math.Min(pieSeries.AreaBounds.Width, pieSeries.AreaBounds.Height);
            float centerX = pieSeries.Center.X;
            float centerY = pieSeries.Center.Y;
            //For calculating pie center angle.
            MidAngle = (StartAngle + (EndAngle / 2)) * 0.0174532925f;
            Radius = pieSeries.ActualRadius;

            float minScale = (float)(minSize * pieSeries.Radius);
            float x = ((centerX * 2) - minScale) / 2;
            float y = ((centerY * 2) - minScale) / 2;

            actualBounds = new RectF(x, y, minScale, minScale);
            currentBounds = actualBounds;

            if (Index == pieSeries.ExplodeIndex)
            {
                actualBounds = actualBounds.Offset((float)(pieSeries.ExplodeRadius * Math.Cos(MidAngle)), (float)(pieSeries.ExplodeRadius * Math.Sin(MidAngle)));
                currentBounds = actualBounds;
            }
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            var pieSeries = Series as PieSeries;

            if (pieSeries == null || double.IsNaN(YValue))
            {
                return;
            }

            var newWidth = actualBounds.Width;
            var newHeight = actualBounds.Height;
            float drawStartAngle = (float)StartAngle;
            float drawEndAngle = (float)EndAngle;

            if (pieSeries.CanAnimate())
            {
                float animationValue = pieSeries.AnimationValue;

                if (!double.IsNaN(PreviousStartAngle) && !double.IsNaN(PreviousEndAngle))
                {
                    drawStartAngle = GetDynamicAnimationAngleValue(animationValue, PreviousStartAngle, StartAngle);
                    drawEndAngle = GetDynamicAnimationAngleValue(animationValue, PreviousEndAngle, EndAngle);
                }
                else
                {
                    newWidth = actualBounds.Width * animationValue;
                    newHeight = actualBounds.Height * animationValue;
                    drawStartAngle = (float)(pieSeries.StartAngle + ((StartAngle - pieSeries.StartAngle) * animationValue));
                    drawEndAngle = (float)(EndAngle * animationValue);
                }
            }

            var offsetX = actualBounds.Left + ((actualBounds.Width - newWidth) / 2);
            var offsetY = actualBounds.Top + ((actualBounds.Height - newHeight) / 2);
            currentBounds = new RectF(offsetX, offsetY, newWidth, newHeight);

            //Drawing segment.
            var path = new PathF();
            path.MoveTo(currentBounds.Left + (currentBounds.Width / 2), currentBounds.Top + (currentBounds.Height / 2));
            path.AddArc(currentBounds.Left, currentBounds.Top, currentBounds.Right, currentBounds.Bottom, -drawStartAngle, -(drawStartAngle + drawEndAngle), true);
            path.Close();
            canvas.SetFillPaint(Fill, path.Bounds);
            canvas.Alpha = Opacity;
            canvas.FillPath(path);

            //Drawing stroke.
            if (HasStroke)
            {
                path.MoveTo(currentBounds.Left + (currentBounds.Width / 2), currentBounds.Top + (currentBounds.Height / 2));
                path.AddArc(currentBounds.Left, currentBounds.Top, currentBounds.Right, currentBounds.Bottom, -drawStartAngle, -(drawStartAngle + drawEndAngle), true);
                path.Close();
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.DrawPath(path);
            }
        }

        #endregion

        #region Internal Methods

        internal virtual void SetData(double startAngle, double endAngle, double yValue)
        {
            StartAngle = startAngle;
            EndAngle = endAngle;
            YValue = yValue;
        }

        internal void SetPreviousData(double[] values)
        {
            this.PreviousStartAngle = values[0];
            this.PreviousEndAngle = values[1];
        }

        internal float GetDynamicAnimationAngleValue(float animationValue, double oldAngle, double currentAngle)
        {
            return (float)((currentAngle > oldAngle) ?
                    oldAngle + ((currentAngle - oldAngle) * animationValue)
                    : oldAngle - ((oldAngle - currentAngle) * animationValue));
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            var pieSeries = Series as PieSeries;
            if (pieSeries != null && IsPointInPieSegment(pieSeries.ActualRadius, x, y))
            {
                return pieSeries.Segments.IndexOf(this);
            }

            return -1;
        }

        internal override void OnDataLabelLayout()
        {
            var pieSeries = Series as PieSeries;
            var dataLabelSettings = pieSeries?.DataLabelSettings;
            IsEmpty = double.IsNaN(YValue) || YValue == 0;

            if (double.IsNaN(YValue) || pieSeries == null || dataLabelSettings == null) return;

            float segmentRadius = pieSeries.GetDataLabelRadius();
            segmentRadius = Index == pieSeries.ExplodeIndex ? segmentRadius + (float)pieSeries.ExplodeRadius : segmentRadius;
            PointF center = pieSeries.Center;
            DataLabelXPosition = center.X + (Math.Cos(MidAngle) * segmentRadius);
            DataLabelYPosition = center.Y + (Math.Sin(MidAngle) * segmentRadius);
            DataLabel = dataLabelSettings.GetLabelContent(YValue);
            PointF labelPosition = new PointF((float)DataLabelXPosition, (float)DataLabelYPosition);
            ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;
            SizeF labelSize = labelStyle.MeasureLabel(DataLabel);

            LabelPositionPoint = GetDataLabelPosition(pieSeries, this, dataLabelSettings, labelSize, labelPosition, (float)labelStyle.LabelPadding);
        }

        #endregion

        #region Private Methods

        private bool IsPointInPieSegment(double radius, double x, double y)
        {
            var pieSeries = Series as PieSeries;
            
            if (pieSeries != null)
            {
                var dx = x - ((currentBounds.Left + currentBounds.Right) / 2);
                var dy = y - ((currentBounds.Top + currentBounds.Bottom) / 2);
                var pointLength = Math.Sqrt((dx * dx) + (dy * dy));
                var angle = GetAngle((float)x, (float)y);
                double endAngle = Math.Abs(StartAngle) + Math.Abs(EndAngle);

                if (pieSeries.StartAngle < pieSeries.EndAngle)
                {
                    if (pieSeries.StartAngle > 0 && endAngle > 360 && angle < pieSeries.StartAngle)
                    {
                        angle = angle + 360;
                    }
                }
                else
                {
                    if (EndAngle > 0 && endAngle < 360 && angle > StartAngle)
                    {
                        angle = angle + 360;
                    }
                }

                if (pointLength <= radius)
                {
                    if (pieSeries.StartAngle > pieSeries.EndAngle)
                    {
                        if (StartAngle > angle && angle > endAngle)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Math.Abs(StartAngle) < angle && angle < endAngle)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private double GetAngle(float x, float y)
        {
            var pieSeries = Series as PieSeries;
            if (pieSeries != null)
            {
                double dx = x - pieSeries.Center.X;
                double dy = -(y - pieSeries.Center.Y);

                var inRads = Math.Atan2(dy, dx);
                inRads = inRads < 0 ? Math.Abs(inRads) : (2 * Math.PI) - inRads;
                return ChartMath.RadianToDegree(inRads);
            }

            return double.NaN;
        }

        private PointF GetDataLabelPosition(CircularSeries series, PieSegment dataLabel, CircularDataLabelSettings dataLabelSettings, SizeF labelSize, PointF labelPosition, float padding)
        {
            XPoints.Clear();
            YPoints.Clear();

            float x = (float)labelPosition.X;
            float y = (float)labelPosition.Y;
            float x1 = x;
            float y1 = y;
            ChartDataLabelStyle labelstyle = dataLabelSettings.LabelStyle;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;

            x1 = x1 + ((float)labelstyle.OffsetX);
            y1 = y1 + ((float)labelstyle.OffsetY);

            if (dataLabelSettings.LabelPlacement == DataLabelPlacement.Inner || dataLabelSettings.LabelPlacement == DataLabelPlacement.Auto)
            {
                innerRect = new RectF(x1, y1, labelSize.Width + borderWidth * 2 + padding, labelSize.Height + borderWidth * 2 + padding);
                PointF point1 = GetPositionForInsideSmartLabel(series, dataLabel, x1, y1, labelSize, borderWidth, padding);
                x1 = point1.X;
                y1 = point1.Y;
            }
            else if (dataLabelSettings.LabelPlacement == DataLabelPlacement.Outer)
            {
                PointF point = GetPositionForOutsideSmartLabel(series, dataLabel, labelSize, borderWidth, padding);
                x1 = point.X;
                y1 = point.Y;
            }

            return new PointF(x1, y1);
        }

        private PointF GetPositionForInsideSmartLabel(CircularSeries series, PieSegment? dataMarkerLabel, float actualX, float actualY, SizeF labelSize, float borderWidth, float padding)
        {
            if (dataMarkerLabel == null) return PointF.Zero;

            bool isIntersected = false;
            float x = actualX, y = actualY;
            PointF point2 = new PointF(x, y);
            innerRect = new RectF(point2.X, point2.Y, innerRect.Size.Width + borderWidth * 2 + padding, innerRect.Size.Height + borderWidth * 2 + padding);

            if (series.SmartLabelAlignment == SmartLabelAlignment.Shift)
            {
                dataMarkerLabel.LabelRect = innerRect;
                dataMarkerLabel.LabelPositionPoint = new Point(innerRect.X, innerRect.Y);
                dataMarkerLabel.IsLeft = false;
                int currentPointIndex = dataMarkerLabel.Index;

                for (int i = 0; i < currentPointIndex; i++)
                {
                    var pieSegment = series.Segments[i] as PieSegment;
                    if (pieSegment == null) return PointF.Zero;

                    if (series.IsOverlap(dataMarkerLabel.LabelRect, pieSegment.LabelRect))
                    {
                        isIntersected = true;
                    }
                }

                isIntersected = !isIntersected ? series.AdjacentLabelRect.IntersectsWith(innerRect) : isIntersected;
            }

            if (isIntersected)
            {
                float angle = (float)dataMarkerLabel.MidAngle;
                PointF center = series.GetCenter();
                float segmentRadius = series.GetRadius();
                dataMarkerLabel.DataLabelXPosition = center.X + (Math.Cos(angle) * segmentRadius);
                dataMarkerLabel.DataLabelYPosition = center.Y + (Math.Sin(angle) * segmentRadius);
                Point rect = GetPositionForOutsideSmartLabel(series, dataMarkerLabel, labelSize, borderWidth, padding);
                innerRect.X = (float)rect.X;
                innerRect.Y = (float)rect.Y;
                point2 = new PointF(innerRect.X, innerRect.Y);
            }
            else
            {
                double circularAngle = Math.Abs(dataMarkerLabel.MidAngle) % (Math.PI * 2);
                bool isLeft = circularAngle > 1.57 && circularAngle < 4.71;
                dataMarkerLabel.LabelRect = new Rect(innerRect.X, innerRect.Y, labelSize.Width, labelSize.Height);
                dataMarkerLabel.LabelPositionPoint = new Point(innerRect.X, innerRect.Y);
                dataMarkerLabel.IsLeft = isLeft;
                dataMarkerLabel.RenderingPosition = RenderingPosition.Inner;
            }

            innerRect.X = point2.X;
            innerRect.Y = point2.Y;
            series.InnerBounds.Add(innerRect);
            series.AdjacentLabelRect = innerRect;

            return new PointF(innerRect.X, innerRect.Y);
        }

        private PointF GetPositionForOutsideSmartLabel(CircularSeries series, PieSegment dataMarkerLabel, SizeF labelSize, float borderWidth, float padding)
        {
            if (series.AreaBounds == Rect.Zero) return PointF.Zero;

            float x = (float)dataMarkerLabel.DataLabelXPosition;
            float y = (float)dataMarkerLabel.DataLabelYPosition;
            double angle = dataMarkerLabel.MidAngle;
            float actualXPos = x;
            float actualYPos = y;
            dataMarkerLabel.XPoints = new List<float>();
            dataMarkerLabel.YPoints = new List<float>();

            dataMarkerLabel.XPoints.Add(x);
            dataMarkerLabel.YPoints.Add(y);

            float radius = series.GetRadius();
            float connectorHeight = (float)(radius * connectorLength);

            x += (float)Math.Cos(angle) * connectorHeight;
            y += (float)Math.Sin(angle) * connectorHeight;

            dataMarkerLabel.XPoints.Add(x);
            dataMarkerLabel.YPoints.Add(y);

            double circularAngle = Math.Abs(angle) % (Math.PI * 2);
            bool isLeft = circularAngle > 1.57 && circularAngle < 4.71;
            Rect clipRect = series.AreaBounds;
            float size = (float)(Math.Min(clipRect.Width, clipRect.Height) / 100) * 5;
            x = isLeft ? x - size : x + size;
            float distanceFromOrigin = (float)Math.Sqrt(Math.Pow(actualXPos - x, 2) + Math.Pow(actualYPos - y, 2)) / 5;
            x = isLeft ? x - distanceFromOrigin : x + distanceFromOrigin;

            //Adding labelGap value to give space between label and connector line.
            float x1 = isLeft ? x + labelGap : x - labelGap;
            dataMarkerLabel.XPoints.Add(x1);
            dataMarkerLabel.YPoints.Add(y);

            //Labels draws with center alignment and its drawn from connector line center, so changing the x value.
            x = isLeft ? x - (labelSize.Width / 2) : x + (labelSize.Width / 2);
            Rect rect = new Rect(x, y, labelSize.Width, labelSize.Height);

            dataMarkerLabel.LabelRect = rect;
            dataMarkerLabel.LabelPositionPoint = new PointF(x, y);
            dataMarkerLabel.IsLeft = isLeft;
            dataMarkerLabel.RenderingPosition = series.SmartLabelAlignment == SmartLabelAlignment.Shift ? RenderingPosition.Outer : dataMarkerLabel.RenderingPosition;

            return new PointF(x, y);
        }

        #endregion

        #endregion
    }

    internal enum RenderingPosition
    {
        Inner,
        Outer
    }

}
