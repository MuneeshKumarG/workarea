using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents a segment of a radial bar chart.
    /// </summary>
    public class RadialBarSegment : ChartSegment
    {
        #region Fields
        private RadialBarSeries? currentSeries;
        private float angleDeviation, actualRadius, innerRadius;
        private bool isClockWise, isTrack;
        internal bool isCircularBar;
        private RectF actualBounds, currentBounds;

        private float drawStartAngle, drawEndAngle, segmentStartAngle,
                      segmentEndAngle, trackStartAngle, trackEndAngle;
        
        private Point endCurveStartPoint, endCurveEndPoint, trackEndCurveStartPoint, trackEndCurveEndPoint;

        private Point trackInnerCurveStartPoint, trackOuterCurveStartPoint, trackStartCurveEndPoint,
                      trackStartCurveStartPoint, trackInnerCurveEndPoint, trackOuterCurveEndPoint,
                      trackInnerLinePoint, trackOuterLinePoint;

        private Point innerCurveStartPoint, outerCurveStartPoint, startCurveEndPoint,
                     startCurveStartPoint, innerCurveEndPoint, outerCurveEndPoint,
                     segmentInnerLinePoint, segmentOuterLinePoint;
        #endregion

        #region Internal Properties

        internal const float CurveDepth = 2;

        internal float InnerRingRadius { get; set; }

        internal object? Item { get; set; }

        internal float OuterRingRadius { get; set; }

        internal Brush? TrackStroke { get; set; }

        internal float TrackStrokeWidth { get; set; }

        internal Brush? TrackFill { get; set; }

        internal bool HasTrackStroke
        {
            get => TrackStrokeWidth > 0 && !ChartColor.IsEmpty(TrackStroke.ToColor());
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets a value that represents segment start angle.
        /// </summary>
        public float StartAngle { get; internal set; }

        /// <summary>
        /// Gets a value that represents segment end angle.
        /// </summary>
        public float EndAngle { get; internal set; }

        #endregion

        #region Methods

        #region internal methods

        internal void SetData(float startAngle, float endAngle)
        {
            StartAngle = startAngle;
            EndAngle = endAngle;
            currentSeries = Series as RadialBarSeries;
            if (currentSeries != null)
            {
                TrackStroke = currentSeries.TrackStroke;
                TrackStrokeWidth = (float)currentSeries.TrackStrokeWidth;
                TrackFill = currentSeries.TrackFill;
            }
        }

        /// <inheritdoc />
        protected internal override void OnLayout()
        {
            if (currentSeries == null)
            {
                return;
            }

            CalculateSegmentRadius();

            float angleDifference = (float)(currentSeries.GetAngleDifference() - 0.01);
            trackStartAngle = (float)currentSeries.StartAngle;
            trackEndAngle = (float)(currentSeries.StartAngle + angleDifference);
            isCircularBar = Math.Round(currentSeries.EndAngle - currentSeries.StartAngle - 0.0001, 1).Equals(360);
            isTrack = true;
            CalculateSegmentAngle(trackStartAngle, trackEndAngle);

            drawStartAngle = StartAngle;
            drawEndAngle = EndAngle + StartAngle - 0.01f;
            isCircularBar = Math.Round(drawEndAngle - drawStartAngle - 0.0001, 1).Equals(360);
            CalculateSegmentAngle(drawStartAngle, drawEndAngle);

        }

        /// <inheritdoc />
        protected internal override void Draw(ICanvas canvas)
        {
            if (currentSeries == null)
            {
                return;
            }

            if (currentSeries.CanAnimate())
            {
                float animationValue = (float)currentSeries.AnimationValue;
                drawStartAngle = (float)(currentSeries.StartAngle + ((StartAngle - currentSeries.StartAngle) * animationValue));
                drawEndAngle = (float)StartAngle + (EndAngle * animationValue);
                isTrack = false;
                CalculateSegmentAngle(drawStartAngle, drawEndAngle);
            }

            DrawTrack(canvas);
            DrawSegment(canvas);
        }

        /// <inheritdoc />
        internal override int GetDataPointIndex(float x, float y)
        {
            if (Series != null && IsPointInRadialBarSegment(x, y))
            {
                return Series.Segments.IndexOf(this);
            }

            return -1;
        }

        #endregion

        #region Private methods
        private void DrawSegment(ICanvas canvas)
        {
            if (currentSeries == null || currentSeries.YValues[Index] == 0) return;
            
            isCircularBar = Math.Round(drawEndAngle - drawStartAngle - 0.0001, 1).Equals(360);
            var segmentPath = new PathF();
            // To draw the start curve for the segment
            if ((currentSeries.CapStyle == CapStyle.BothCurve || currentSeries.CapStyle == CapStyle.StartCurve) && !isCircularBar )
            {
                segmentPath.MoveTo(innerCurveStartPoint);
                segmentPath.CurveTo(startCurveStartPoint, startCurveEndPoint, outerCurveStartPoint);
            }
            // To draw the outer arc for the segment
            segmentPath.AddArc(actualBounds.Left, actualBounds.Top, actualBounds.Right, actualBounds.Bottom, -segmentStartAngle, -segmentEndAngle, isClockWise);
            // To draw the inner arc for the segment
            segmentPath.AddArc(currentBounds.Left, currentBounds.Top, currentBounds.Right, currentBounds.Bottom, -segmentEndAngle, -segmentStartAngle, !isClockWise);
            segmentPath.Close();
            // To draw the end curve for the segment
            if ((currentSeries.CapStyle == CapStyle.EndCurve || currentSeries.CapStyle == CapStyle.BothCurve) && !isCircularBar )
            {
                segmentPath.MoveTo(innerCurveEndPoint);
                segmentPath.CurveTo(endCurveStartPoint, endCurveEndPoint, outerCurveEndPoint);
            }

            canvas.SetFillPaint(Fill, segmentPath.Bounds);
            canvas.Alpha = Opacity;
            canvas.FillPath(segmentPath);
            DrawSegmentStroke(canvas);
        }

        private void DrawTrack(ICanvas canvas)
        {
            if (currentSeries == null) return;

            isCircularBar = Math.Round(currentSeries.EndAngle - currentSeries.StartAngle - 0.0001, 1).Equals(360);
            isClockWise = trackEndAngle >= trackStartAngle;
            var track = new PathF();
            // To draw the start curve for the track
            if ((currentSeries.CapStyle == CapStyle.BothCurve || currentSeries.CapStyle == CapStyle.StartCurve) && !isCircularBar)
            {
                track.MoveTo(trackInnerCurveStartPoint);
                track.CurveTo(trackStartCurveStartPoint, trackStartCurveEndPoint, trackOuterCurveStartPoint);
            }
            // To draw the inner arc for the track
            track.AddArc(currentBounds.Left, currentBounds.Top, currentBounds.Right, currentBounds.Bottom, -trackStartAngle, -trackEndAngle, isClockWise);
            // To draw the outer arc for the track
            track.AddArc(actualBounds.Left, actualBounds.Top, actualBounds.Right, actualBounds.Bottom, -trackEndAngle, -trackStartAngle, !isClockWise);
            track.Close();
            // To draw the end curve for the track
            if ((currentSeries.CapStyle == CapStyle.EndCurve || currentSeries.CapStyle == CapStyle.BothCurve) && !isCircularBar )
            {
                track.MoveTo(trackInnerCurveEndPoint);
                track.CurveTo(trackEndCurveStartPoint, trackEndCurveEndPoint, trackOuterCurveEndPoint);
            }

            canvas.SetFillPaint(currentSeries.TrackFill, track.Bounds);
            canvas.Alpha = Opacity;
            canvas.FillPath(track);
            DrawTrackStroke(canvas);
        }

        private void DrawTrackStroke(ICanvas canvas)
        {
            if (HasTrackStroke && currentSeries != null)
            {
                if (isCircularBar) 
                {
                    // To draw the outer arc for the track stroke
                    var outerTrack = new PathF();
                    outerTrack.AddArc(actualBounds.Left, actualBounds.Top, actualBounds.Right, actualBounds.Bottom, -trackStartAngle, -trackEndAngle, isClockWise);
                    // To draw the inner arc for the track stroke
                    var innerTrack = new PathF();
                    innerTrack.AddArc(currentBounds.Left, currentBounds.Top, currentBounds.Right, currentBounds.Bottom, -trackEndAngle, -trackStartAngle, !isClockWise);
                    canvas.StrokeColor = TrackStroke.ToColor();
                    canvas.StrokeSize = (float)TrackStrokeWidth;
                    canvas.DrawPath(outerTrack);
                    canvas.DrawPath(innerTrack);
                }
                else 
                {
                    var trackStrokePath = new PathF();
                    // To draw the start curve for the track stroke
                    if ((currentSeries.CapStyle == CapStyle.BothCurve || currentSeries.CapStyle == CapStyle.StartCurve) && !isCircularBar)
                    {
                        trackStrokePath.MoveTo(trackInnerCurveStartPoint);
                        trackStrokePath.CurveTo(trackStartCurveStartPoint, trackStartCurveEndPoint, trackOuterCurveStartPoint);
                    }
                    // To draw the outer arc for the track stroke
                    trackStrokePath.AddArc(actualBounds.Left, actualBounds.Top, actualBounds.Right, actualBounds.Bottom, -trackStartAngle, -trackEndAngle, isClockWise);
                    // To draw the end curve for the track stroke
                    if ((currentSeries.CapStyle == CapStyle.EndCurve || currentSeries.CapStyle == CapStyle.BothCurve) && !isCircularBar)
                    {
                        trackStrokePath.MoveTo(trackInnerCurveEndPoint);
                        trackStrokePath.CurveTo(trackEndCurveStartPoint, trackEndCurveEndPoint, trackOuterCurveEndPoint);
                        trackStrokePath.MoveTo(trackInnerCurveEndPoint);
                    }
                    // To draw the inner arc for the track stroke
                    trackStrokePath.AddArc(currentBounds.Left, currentBounds.Top, currentBounds.Right, currentBounds.Bottom, -trackEndAngle, -trackStartAngle, !isClockWise);
                    canvas.StrokeColor = TrackStroke.ToColor();
                    canvas.StrokeSize = (float)TrackStrokeWidth;
                    canvas.DrawPath(trackStrokePath);
                    if (currentSeries.CapStyle == CapStyle.BothFlat || currentSeries.CapStyle == CapStyle.EndCurve) 
                    {
                        // To draw line for the track stroke
                        canvas.DrawLine(trackInnerLinePoint, trackOuterLinePoint);
                        canvas.StrokeColor = TrackStroke.ToColor();
                        canvas.StrokeSize = (float)TrackStrokeWidth;
                    }
                }
            }
        }

        private void DrawSegmentStroke(ICanvas canvas)
        {
            if (HasStroke && currentSeries != null)
            {
                var strokePath = new PathF();
                // To draw the start curve for the segment stroke
                if ((currentSeries.CapStyle == CapStyle.BothCurve || currentSeries.CapStyle == CapStyle.StartCurve) && !isCircularBar)
                {
                    strokePath.MoveTo(innerCurveStartPoint);
                    strokePath.CurveTo(startCurveStartPoint, startCurveEndPoint, outerCurveStartPoint);
                }
                // To draw the outer arc for the segment stroke
                strokePath.AddArc(actualBounds.Left, actualBounds.Top, actualBounds.Right, actualBounds.Bottom, -segmentStartAngle, -segmentEndAngle, isClockWise);
                // To draw the end curve for the segment stroke
                if ((currentSeries.CapStyle == CapStyle.EndCurve || currentSeries.CapStyle == CapStyle.BothCurve) && !isCircularBar)
                {
                    strokePath.MoveTo(innerCurveEndPoint);
                    strokePath.CurveTo(endCurveStartPoint, endCurveEndPoint, outerCurveEndPoint);
                    strokePath.MoveTo(innerCurveEndPoint);
                }
                // To draw the inner arc for the segment stroke
                strokePath.AddArc(currentBounds.Left, currentBounds.Top, currentBounds.Right, currentBounds.Bottom, -segmentEndAngle, -segmentStartAngle, !isClockWise);
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.DrawPath(strokePath);
                if (currentSeries.CapStyle == CapStyle.BothFlat || currentSeries.CapStyle == CapStyle.EndCurve)
                {
                    // To draw line for the segment stroke
                    canvas.DrawLine(segmentInnerLinePoint, segmentOuterLinePoint);
                    canvas.StrokeColor = Stroke.ToColor();
                    canvas.StrokeSize = (float)StrokeWidth;
                }
            }
        }

        private void CapStyleCalculation(float midRadius, float segmentRadius, float startCurveAngle, float drawStartAngle, float drawEndAngle)
        {
            float calculatedRadius = (InnerRingRadius + OuterRingRadius) / 2;
            angleDeviation = ChartUtils.CalculateAngleDeviation(calculatedRadius, (float)(segmentRadius / 2), 360) * (isClockWise ? 1 : -1);
            if (currentSeries != null && currentSeries.CapStyle != CapStyle.BothFlat)
            {
                if (!isCircularBar)
                {
                    UpdateSegmentAngleForCurvePosition(segmentRadius, drawStartAngle, drawEndAngle);
                    if (currentSeries.CapStyle == CapStyle.StartCurve || currentSeries.CapStyle == CapStyle.BothCurve)
                    {
                        if (isTrack)
                        {
                            TrackStartCurveCalculation(midRadius, segmentRadius, startCurveAngle);
                        }
                        else
                        {
                            SegmentStartCurveCalculation(midRadius,segmentRadius,startCurveAngle);
                        }
                    }
                    if (currentSeries.CapStyle == CapStyle.EndCurve || currentSeries.CapStyle == CapStyle.BothCurve)
                    {
                        if (isTrack)
                        {
                            TrackEndCurveCalculation(segmentEndAngle, midRadius, segmentRadius);
                        }
                        else
                        {
                            SegmentEndCurveCalculation(midRadius, segmentRadius);
                        }
                    }
                }
            }
            else
            {  
                segmentEndAngle += Math.Abs(EndAngle) < Math.Abs(angleDeviation) ? (2 * angleDeviation) : 0;
            }

            isTrack = false;
            isCircularBar = false;
        }

        private void SegmentEndCurveCalculation(float midRadius, float segmentRadius)
        {
            if (currentSeries == null) return;

            float curvePoint = (float)(drawEndAngle + (angleDeviation / 1.65));
            float endingPoint = segmentEndAngle - (2 * angleDeviation);
            if (currentSeries.StartAngle < currentSeries.EndAngle)
            {
                if (curvePoint > endingPoint)
                {
                    if (segmentStartAngle > endingPoint)
                    {
                        endingPoint = segmentStartAngle;
                        curvePoint = segmentStartAngle + angleDeviation + (2 * angleDeviation);
                    }
                }

                segmentEndAngle = segmentEndAngle - (2 * angleDeviation);
                segmentEndAngle = ((endingPoint >= segmentEndAngle) && segmentEndAngle < segmentStartAngle) ? segmentStartAngle : segmentEndAngle;
            }
            else
            {
                if (endingPoint > curvePoint)
                {
                    if (segmentStartAngle < endingPoint)
                    {
                        endingPoint = segmentStartAngle;
                        curvePoint = segmentStartAngle + angleDeviation + (2 * angleDeviation);
                    }
                }

                segmentEndAngle = segmentEndAngle - (2 * angleDeviation);
                segmentEndAngle = (segmentStartAngle < segmentEndAngle) ? segmentStartAngle : segmentEndAngle;
            }

            Point endPoint = ChartUtils.AngleToVector(endingPoint);
            innerCurveEndPoint = new Point(currentBounds.Center.X + (InnerRingRadius * endPoint.X),
                                 currentBounds.Center.Y + (InnerRingRadius * endPoint.Y));
            outerCurveEndPoint = new Point(currentBounds.Center.X + (OuterRingRadius * endPoint.X),
                                 currentBounds.Center.Y + (OuterRingRadius * endPoint.Y));
            Point previousAngle = ChartUtils.AngleToVector(curvePoint);
            // We have used default value for curve depth
            endCurveStartPoint = new Point(currentBounds.Center.X + ((midRadius - segmentRadius / 1.75) * previousAngle.X),
                                       currentBounds.Center.Y + ((midRadius - segmentRadius / 1.75) * previousAngle.Y));
            endCurveEndPoint = new Point(currentBounds.Center.X + ((midRadius + segmentRadius / 1.5) * previousAngle.X),
                                     currentBounds.Center.Y + ((midRadius + segmentRadius / 1.5) * previousAngle.Y));
        }

        private void SegmentStartCurveCalculation(float midRadius,float segmentRadius,float startCurveAngle)
        {
            if(currentSeries == null) return;

            float curvePoint = (float)(drawStartAngle - (angleDeviation / 1.75));
            float endingPoint = startCurveAngle + (2 * angleDeviation);
            if (currentSeries.StartAngle < currentSeries.EndAngle)
            {
                if (endingPoint > curvePoint)
                {
                    if (curvePoint > segmentStartAngle)
                    {
                        curvePoint = segmentStartAngle;
                        endingPoint = segmentStartAngle;
                    }

                    segmentStartAngle = endingPoint;
                    segmentEndAngle = (segmentStartAngle > segmentEndAngle) ? segmentStartAngle : segmentEndAngle;
                }
            }
            else
            {
                if (curvePoint > endingPoint)
                {
                    segmentStartAngle = endingPoint;
                    segmentEndAngle = (segmentStartAngle < segmentEndAngle) ? segmentStartAngle : segmentEndAngle;
                }
            }

            // We have used default value for curve depth
            Point vectorPoint = ChartUtils.AngleToVector(curvePoint);
            startCurveStartPoint = new Point(currentBounds.Center.X + ((midRadius - segmentRadius / 1.75) * vectorPoint.X),
                                       currentBounds.Center.Y + ((midRadius - segmentRadius / 1.75) * vectorPoint.Y));
            startCurveEndPoint = new Point(currentBounds.Center.X + ((midRadius + segmentRadius / 1.2) * vectorPoint.X),
                                 currentBounds.Center.Y + ((midRadius + segmentRadius / 1.2) * vectorPoint.Y));
            Point centerPoint = ChartUtils.AngleToVector(endingPoint);
            innerCurveStartPoint = new Point(currentBounds.Center.X + (InnerRingRadius * centerPoint.X),
                                   currentBounds.Center.Y + (InnerRingRadius * centerPoint.Y));
            outerCurveStartPoint = new Point(currentBounds.Center.X + ((midRadius + segmentRadius) * centerPoint.X),
                                   currentBounds.Center.Y + ((midRadius + segmentRadius) * centerPoint.Y));
        }

        private void TrackStartCurveCalculation(float midRadius, float segmentRadius, float startCurveAngle)
        {
            if (currentSeries == null) return;
            // We have used default value for curve depth
            Point vectorPoint = ChartUtils.AngleToVector((float)(trackStartAngle - angleDeviation / 1.75));
            trackStartCurveStartPoint = new Point(currentBounds.Center.X + ((midRadius - segmentRadius / 1.75) * vectorPoint.X),
                                       currentBounds.Center.Y + ((midRadius - segmentRadius / 1.75) * vectorPoint.Y));
            trackStartCurveEndPoint = new Point(currentBounds.Center.X + ((midRadius + segmentRadius / 1.2) * vectorPoint.X),
                                      currentBounds.Center.Y + ((midRadius + segmentRadius / 1.2) * vectorPoint.Y));
            
            startCurveAngle += (2 * angleDeviation);

            Point centerPoint = ChartUtils.AngleToVector(startCurveAngle);
            trackInnerCurveStartPoint = new Point(currentBounds.Center.X + (InnerRingRadius * centerPoint.X),
                                        currentBounds.Center.Y + (InnerRingRadius * centerPoint.Y));
            trackOuterCurveStartPoint = new Point(currentBounds.Center.X + ((midRadius + segmentRadius) * centerPoint.X),
                                        currentBounds.Center.Y + ((midRadius + segmentRadius) * centerPoint.Y));
            
            trackStartAngle = currentSeries.CapStyle == CapStyle.StartCurve ? startCurveAngle : trackStartAngle;
        }

        private void TrackEndCurveCalculation(float outerSegmentEndAngle, float midRadius, float segmentRadius)
        {
            if (currentSeries == null) return;
            
            Point endPoint = ChartUtils.AngleToVector(outerSegmentEndAngle - (2 * angleDeviation));
            trackInnerCurveEndPoint = new Point(currentBounds.Center.X + (InnerRingRadius * endPoint.X),
                                      currentBounds.Center.Y + (InnerRingRadius * endPoint.Y));
            trackOuterCurveEndPoint = new Point(currentBounds.Center.X + (OuterRingRadius * endPoint.X),
                                      currentBounds.Center.Y + (OuterRingRadius * endPoint.Y));
            // We have used default value for curve depth
            Point previousAngle = ChartUtils.AngleToVector((float)(trackEndAngle + (angleDeviation / 1.65)));
            trackEndCurveStartPoint = new(currentBounds.Center.X + ((midRadius - segmentRadius / 1.75) * previousAngle.X),
                                       currentBounds.Center.Y + ((midRadius - segmentRadius / 1.75) * previousAngle.Y));
            trackEndCurveEndPoint = new Point(currentBounds.Center.X + ((midRadius + segmentRadius / 1.5) * previousAngle.X),
                                     currentBounds.Center.Y + ((midRadius + segmentRadius / 1.5) * previousAngle.Y));
            trackEndAngle = (float)(outerSegmentEndAngle - (2 * angleDeviation));
            trackStartAngle = currentSeries.CapStyle == CapStyle.BothCurve ? trackStartAngle + (2 * angleDeviation) : trackStartAngle;

            isTrack = false;
        }

        private void UpdateSegmentAngleForCurvePosition(float segmentRadius, float drawStartAngle, float drawEndAngle)
        {
            if (currentSeries == null || segmentRadius == 0) return;

            if (currentSeries.CapStyle != CapStyle.EndCurve)
            {
                segmentStartAngle = isClockWise ? drawStartAngle +
                    segmentRadius * CurveDepth / InnerRingRadius :
                    drawStartAngle - segmentRadius * CurveDepth / InnerRingRadius;
            }

            if (currentSeries.CapStyle != CapStyle.StartCurve)
            {
                segmentEndAngle = !isClockWise ? drawEndAngle +
                    segmentRadius * CurveDepth / OuterRingRadius :
                    drawEndAngle - segmentRadius * CurveDepth / OuterRingRadius;
            }
        }

        private bool IsPointInRadialBarSegment(double x, double y)
        {
            var radialBarSeries = Series as RadialBarSeries;
            if (radialBarSeries != null)
            {
                double dx, dy;
                dx = x - ((currentBounds.Left + currentBounds.Right) / 2);
                dy = y - ((currentBounds.Top + currentBounds.Bottom) / 2);

                double angle = ChartMath.RadianToDegree(Math.Atan2(dy, dx));
                double distanceSquare = (dx * dx) + (dy * dy);
                double segmentEndAngle = Math.Abs(EndAngle) + Math.Abs(StartAngle);
                if (angle < 0)
                {
                    angle = angle + 360;
                }

                if (radialBarSeries.StartAngle < radialBarSeries.EndAngle)
                {

                    if (radialBarSeries.StartAngle < 0 && segmentEndAngle < 360 && angle < radialBarSeries.StartAngle)
                    {
                        angle = angle + 360;
                    }
                }
                else
                {

                    if (EndAngle > 0 && segmentEndAngle < 360 && angle > StartAngle)
                    {
                        angle = angle + 360;
                    }
                }

                if (distanceSquare >= InnerRingRadius * InnerRingRadius && distanceSquare <= OuterRingRadius * OuterRingRadius)
                {
                    if (radialBarSeries.StartAngle > radialBarSeries.EndAngle)
                    {
                        if (StartAngle > angle && angle > drawEndAngle)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        if ((StartAngle < angle) && (angle < drawEndAngle))
                        {
                            return true;
                        }
                        else if (StartAngle < 0 && drawEndAngle < angle)
                        {
                            angle = angle - 360;
                            if ((StartAngle < angle) && (angle < drawEndAngle))
                            {
                                return true;
                            }

                        }
                    }
                }
            }

            return false;
        }

        private void CalculateSegmentAngle(float drawStartAngle, float drawEndAngle)
        {
            if (currentSeries == null || (currentSeries.YValues[Index] == 0 && !isTrack)) return;
            
            isClockWise = currentSeries.EndAngle > currentSeries.StartAngle;
            float segmentRadius = (OuterRingRadius - InnerRingRadius) / 2;
            float midRadius = OuterRingRadius - segmentRadius;
            segmentEndAngle = drawEndAngle;
            segmentStartAngle = drawStartAngle;
            if (currentSeries.EndAngle > currentSeries.StartAngle)
            {
                segmentEndAngle = segmentStartAngle > segmentEndAngle ? segmentStartAngle : segmentEndAngle;
            }

            float startCurveAngle = drawStartAngle;

            if (isTrack)
            {
                if (currentSeries.CapStyle == CapStyle.BothFlat || currentSeries.CapStyle == CapStyle.EndCurve)
                {
                    Point startPoint = ChartUtils.AngleToVector(trackStartAngle);
                    trackInnerLinePoint = new Point(currentBounds.Center.X +
                        (InnerRingRadius * startPoint.X), currentBounds.Center.Y +
                        (InnerRingRadius * startPoint.Y));
                    trackOuterLinePoint = new Point(currentBounds.Center.X +
                        ((midRadius + segmentRadius) * startPoint.X), currentBounds.Center.Y +
                        ((midRadius + segmentRadius) * startPoint.Y));
                }
            }
            else if (currentSeries.CapStyle == CapStyle.BothFlat || currentSeries.CapStyle == CapStyle.EndCurve) 
            {
                Point startPoint = ChartUtils.AngleToVector(drawStartAngle);
                segmentInnerLinePoint = new Point(currentBounds.Center.X +
                    (InnerRingRadius * startPoint.X), currentBounds.Center.Y +
                    (InnerRingRadius * startPoint.Y));
                segmentOuterLinePoint = new Point(currentBounds.Center.X +
                    ((midRadius + segmentRadius) * startPoint.X), currentBounds.Center.Y +
                    ((midRadius + segmentRadius) * startPoint.Y));
            }

            CapStyleCalculation(midRadius, segmentRadius, startCurveAngle, drawStartAngle, drawEndAngle);
        }

        private void CalculateSegmentRadius()
        {
            if (currentSeries == null || double.IsNaN(currentSeries.YValues[Index]))
            {
                return;
            }

            if (Index == 0)
            {
                currentSeries.ActualRadius = currentSeries.GetRadius();
                currentSeries.Center = currentSeries.GetCenter();
            }

            var center = currentSeries.Center;
            RectF seriesClipRectangle = currentSeries.AreaBounds;
            double minScale = Math.Min(seriesClipRectangle.Width, seriesClipRectangle.Height) - (Math.Min(seriesClipRectangle.Width, seriesClipRectangle.Height) * 0.2);
            minScale = minScale * currentSeries.Radius;
            actualBounds = new RectF((float)((center.X * 2) - minScale) / 2, (float)((center.Y * 2) - minScale) / 2, (float)minScale, (float)minScale);
            currentBounds = new RectF(actualBounds.Left, actualBounds.Top, actualBounds.Width, actualBounds.Height);
            innerRadius = (float)((float)(Math.Min(actualBounds.Height, actualBounds.Width) / 2) * currentSeries.InnerRadius);
            actualRadius = (actualBounds.Width / 2 - innerRadius);
            float centerRadius = (float)((float)(Math.Min(actualBounds.Height, actualBounds.Width) / 2) * 0.2);
            double radius = (actualRadius / currentSeries.PointsCount) * (1 - currentSeries.GapRatio);
            InnerRingRadius = innerRadius + (actualRadius / currentSeries.PointsCount) * Index + centerRadius;
            OuterRingRadius = (float)(InnerRingRadius + radius);
            currentBounds = new RectF(currentBounds.X + (currentBounds.Width / 2) -
                InnerRingRadius, currentBounds.Y + (currentBounds.Height / 2) - InnerRingRadius,
                2 * InnerRingRadius, 2 * InnerRingRadius);
            actualBounds = new RectF(actualBounds.X + (actualBounds.Width / 2) - OuterRingRadius,
                actualBounds.Y + (actualBounds.Height / 2) - OuterRingRadius, 2 * OuterRingRadius,
                2 * OuterRingRadius);
        }
        #endregion
        #endregion
    }
}
