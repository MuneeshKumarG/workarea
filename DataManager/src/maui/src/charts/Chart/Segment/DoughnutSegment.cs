using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents a segment of a doughnut chart.
    /// </summary>
    public class DoughnutSegment : PieSegment
    {
        #region Fields

        private RectF actualBounds, currentBounds;

        private double startAngle, endAngle;

        private double segmentRadius;

        #endregion

        #region Internal Properties

        internal float InnerRadius { get; set; }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            var series = Series as DoughnutSeries;

            if (series == null || double.IsNaN(YValue))
            {
                return;
            }

            //For calculating doughnut center angle.
            MidAngle = (StartAngle + (EndAngle / 2)) * 0.0174532925f;
            UpdatePosition();
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            DoughnutSeries? series = Series as DoughnutSeries;

            if (series == null || double.IsNaN(YValue))
            {
                return;
            }

            float drawStartAngle = (float)StartAngle;
            float drawEndAngle = (float)EndAngle;

            if (series.CanAnimate())
            {
                float animationValue = series.AnimationValue;

                if (!double.IsNaN(PreviousStartAngle) && !double.IsNaN(PreviousEndAngle))
                {
                    drawStartAngle = GetDynamicAnimationAngleValue(animationValue, PreviousStartAngle, StartAngle);
                    drawEndAngle = GetDynamicAnimationAngleValue(animationValue, PreviousEndAngle, EndAngle);
                }
                else
                {
                    drawStartAngle = (float)(series.StartAngle + ((StartAngle - series.StartAngle) * animationValue));
                    drawEndAngle = (float)EndAngle * animationValue;
                }
            }

            UpdatePosition();

            PathF path = new PathF();

            //Drawing segment.
            path.AddArc(actualBounds.Left, actualBounds.Top, actualBounds.Right, actualBounds.Bottom, -drawStartAngle, -(drawStartAngle + drawEndAngle), true);
            path.AddArc(currentBounds.Left, currentBounds.Top, currentBounds.Right, currentBounds.Bottom, -(drawStartAngle + drawEndAngle), -drawStartAngle, false);
            path.Close();
            canvas.SetFillPaint(Fill, path.Bounds);
            canvas.Alpha = Opacity;
            canvas.FillPath(path);

            //Drawing stroke.
            if (HasStroke)
            {
#if WINDOWS
                //MAUI-582: Exception faced "Value not fall within expected range, due to path closed before stroke drawing" 
                var radius = (float)series.InnerRadius * (Math.Min(actualBounds.Width, actualBounds.Height) / 2);
                var x = (float)(currentBounds.X + (currentBounds.Width / 2) + (radius * Math.Cos(drawStartAngle * (Math.PI / 180))));
                var y = (float)(currentBounds.Y + (currentBounds.Width / 2) + (radius * Math.Sin(drawStartAngle * (Math.PI / 180))));
                path.MoveTo(x, y);
#endif
                path.AddArc(actualBounds.Left, actualBounds.Top, actualBounds.Right, actualBounds.Bottom, -drawStartAngle, -(drawStartAngle + drawEndAngle), true);
                path.AddArc(currentBounds.Left, currentBounds.Top, currentBounds.Right, currentBounds.Bottom, -(drawStartAngle + drawEndAngle), -drawStartAngle, false);
                path.Close();
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.DrawPath(path);
            }
        }

        #endregion

        #region Internal Methods

        internal override void SetData(double mStartAngle, double mEndAngle, double mYValue)
        {
            startAngle = mStartAngle;
            endAngle = mEndAngle;
            StartAngle = mStartAngle;
            EndAngle = mEndAngle;
            YValue = mYValue;
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (Series != null && IsPointInDoughnutSegment(segmentRadius, x, y))
            {
                return Series.Segments.IndexOf(this);
            }

            return -1;
        }

        #endregion

        #region private Methods

        private void UpdatePosition()
        {
            var series = Series as DoughnutSeries;

            if (series == null)
            {
                return;
            }

            if (Index == 0)
            {
                series.ActualRadius = series.GetRadius();
                series.Center = series.GetCenter();
            }

            var center = series.Center;
            RectF seriesClipRect = series.AreaBounds;
            double minScale = Math.Min(seriesClipRect.Width, seriesClipRect.Height) * series.Radius;
            actualBounds = new RectF((float)((center.X * 2) - minScale) / 2, (float)((center.Y * 2) - minScale) / 2, (float)minScale, (float)minScale);
            currentBounds = new RectF(actualBounds.Left, actualBounds.Top, actualBounds.Width, actualBounds.Height);

            if (series.ExplodeIndex == Index)
            {
                float angle = (float)((2f * (series.StartAngle + ((StartAngle - series.StartAngle) * series.AnimationValue)) * 0.0174532925f) + ((EndAngle * series.AnimationValue) * 0.0174532925f)) / 2f;
                actualBounds = actualBounds.Offset((float)(series.ExplodeRadius * Math.Cos(angle)), (float)(series.ExplodeRadius * Math.Sin(angle)));
                currentBounds = actualBounds;
            }

            InnerRadius = (float)series.InnerRadius * (Math.Min(actualBounds.Width, actualBounds.Height) / 2);
            currentBounds = new RectF(currentBounds.X + (currentBounds.Width / 2) - InnerRadius, currentBounds.Y + (currentBounds.Height / 2) - InnerRadius, 2 * InnerRadius, 2 * InnerRadius);
            series.ActualRadius = (float)minScale / 2;
            segmentRadius = series.ActualRadius;
        }

        private bool IsPointInDoughnutSegment(double radius, double x, double y)
        {
            var doughnutSeries = Series as DoughnutSeries;
            if (doughnutSeries != null)
            {
                double dx, dy;

                dx = x - ((currentBounds.Left + currentBounds.Right) / 2);
                dy = y - ((currentBounds.Top + currentBounds.Bottom) / 2);

                double angle = ChartMath.RadianToDegree(Math.Atan2(dy, dx));
                if (angle < 0)
                {
                    angle = angle + 360;
                }

                double distanceSquare = (dx * dx) + (dy * dy);
                double segmentEndAngle = Math.Abs(startAngle) + Math.Abs(endAngle);

                if (doughnutSeries.StartAngle < doughnutSeries.EndAngle)
                {
                    if (doughnutSeries.StartAngle > 0 && segmentEndAngle > 360 && angle < doughnutSeries.StartAngle)
                    {
                        angle = angle + 360;
                    }
                }
                else
                {
                    if (endAngle > 0 && segmentEndAngle < 360 && angle > startAngle)
                    {
                        angle = angle + 360;
                    }
                }

                var innerRadius = InnerRadius;
                var outerRadius = radius;

                if (distanceSquare >= innerRadius * innerRadius && distanceSquare <= outerRadius * outerRadius)
                {
                    if (doughnutSeries.StartAngle > doughnutSeries.EndAngle)
                    {
                        if (startAngle > angle && angle > segmentEndAngle)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if (Math.Abs(startAngle) < angle && angle < segmentEndAngle)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #endregion
    }
}