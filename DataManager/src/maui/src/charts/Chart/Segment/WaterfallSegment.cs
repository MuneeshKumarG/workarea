using Microsoft.Maui.Graphics;
using System;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents a segment of a Waterfall chart.
    /// </summary>
    public class WaterfallSegment : CartesianSegment
    {
        #region Fields

        #region Private Fields

        private double differenceValue;

        private float lineX1, lineY1, lineX2, lineY2;

        #endregion

        #region Internal Fields

        internal WaterfallSegment? PreviousWaterfallSegment;

        internal double x1, y1, x2, y2, xValue;

        #endregion

        #endregion

        #region Properties

        #region Internal Property

        internal WaterfallSegmentType SegmentType { get; set; }

        #endregion

        #region Public  Property

        /// <summary>
        /// Gets the left position value for the Waterfallsegment.
        /// </summary>
        public float Left { get; internal set; }

        /// <summary>
        /// Gets the Top position value for the Waterfallsegment.
        /// </summary>
        public float Top { get; internal set; }

        /// <summary>
        /// Gets the Right position value for the Waterfallsegment.
        /// </summary>
        public float Right { get; internal set; }

        /// <summary>
        /// Gets the Bottom position value for the Waterfallsegment.
        /// </summary>
        public float Bottom { get; internal set; }

        internal double WaterfallSum { get; set; }

        internal double Sum { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Protected Internal Override Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            if (Series is WaterfallSeries series)
            {
                if (series == null)
                {
                    return;
                }

                Layout(series);
            }
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {

            if (Series is not WaterfallSeries waterfallSeries)
            {
                return;
            }

            if (!double.IsNaN(Left) && !double.IsNaN(Top) && !double.IsNaN(Right) && !double.IsNaN(Bottom))
            {
                canvas.Alpha = Opacity;
                if (waterfallSeries.CanAnimate())
                {
                    Layout(waterfallSeries);
                }

                //Drawing rectangle segment.
                var rect = new Rect() { Left = Left, Top = Top, Right = Right, Bottom = Bottom };
                canvas.SetFillPaint(Fill, rect);
                canvas.FillRectangle(rect);

                if (HasStroke)
                {
                    canvas.DrawRectangle(rect);
                }

                //Drawing Connector Line.
                if (waterfallSeries.ShowConnectorLine == true && PreviousWaterfallSegment != null)
                {
                    var connectorLineStyle = waterfallSeries.ConnectorLineStyle;

                    if (connectorLineStyle != null)
                    {
                        canvas.StrokeColor = (connectorLineStyle.Stroke as SolidColorBrush)?.Color ?? Colors.Black;
                        canvas.StrokeSize = (float)connectorLineStyle.StrokeWidth;

                        if (connectorLineStyle.StrokeDashArray != null)
                            canvas.StrokeDashPattern = connectorLineStyle.StrokeDashArray?.ToFloatArray();
                    }

                    canvas.DrawLine(lineX1, lineY1, lineX2, lineY2);
                }
            }
        }

        #endregion

        #region Internal Override Methods

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
            if (Series is WaterfallSeries waterfallSeries)
            {
                if (waterfallSeries != null && waterfallSeries.AllowAutoSum == true)
                {
                    CalculateDataLabelPosition(xValue, SegmentType == WaterfallSegmentType.Sum ? y1 : differenceValue, SegmentType == WaterfallSegmentType.Sum ? y1 : differenceValue);
                }
                else
                {
                    CalculateDataLabelPosition(xValue, differenceValue, differenceValue);
                }
            }
        }

        internal override void SetData(double[] values)
        {
            if (Series is WaterfallSeries series)
            {
                x1 = values[0];
                x2 = values[1];
                y1 = values[2];
                y2 = values[3];
                xValue = values[4];
                differenceValue = values[5];
                series.XRange += DoubleRange.Union(xValue);
                series.YRange += new DoubleRange(y1, y2);
            }
        }

        #endregion

        #region Private Methods

        private void Layout(WaterfallSeries? series)
        {
            var xAxis = series?.ActualXAxis;
            
            if (series == null || series.ChartArea == null || xAxis == null)
            {
                return;
            }

            var start = Math.Floor(xAxis.VisibleRange.Start);
            var end = Math.Ceiling(xAxis.VisibleRange.End);
            Left = Top = Right = Bottom = float.NaN;
          
            if (x1 <= end && x2 >= start)
            {
                Left = series.TransformToVisibleX(x1, y1);
                Top = series.TransformToVisibleY(x1, y1);
                Right = series.TransformToVisibleX(x2, y2);
                Bottom = series.TransformToVisibleY(x2, y2);
               
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
            }
            else
            {
                this.Left = float.NaN;
            }
            
            float midPoint = ((Top + Bottom) / 2) - Top;
            float midPointTransposed = (Right + Left) / 2 - Right;
            
            if (series.CanAnimate())
            {
                if (series.ChartArea.IsTransposed)
                {
                    Right +=  midPointTransposed * (1 - series.AnimationValue);
                    Left -=  midPointTransposed * (1 - series.AnimationValue);
                }
                else
                {
                    Top += midPoint * (1 - series.AnimationValue);
                    Bottom -= midPoint * (1 - series.AnimationValue);
                }
            }

            if(!series.CanAnimate())
            //Updating the ConnectorLine points
            UpdateConnectorLine(series);
            SegmentBounds = new RectF(Left, Top, Right - Left, Bottom - Top);
        }

        private void UpdateConnectorLine(WaterfallSeries series)
        {
            if (PreviousWaterfallSegment != null)
            {
                //Setting X-Coordinates
                lineX1 = PreviousWaterfallSegment.Right;
                lineX2 = Left;

                //Setting Y-Coordinates for summary type segments
                if (SegmentType == WaterfallSegmentType.Sum)
                {
                    if (series.AllowAutoSum)
                    {
                        if (WaterfallSum >= 0)
                            lineY1 = lineY2 = Top;
                        else
                            lineY1 = lineY2 = Bottom;
                    }
                    else
                    {
                        if (series.YValues[((int)xValue) - 1] >= 0)
                            lineY2 = Top;
                        else
                            lineY2 = Bottom;

                        if (PreviousWaterfallSegment.SegmentType == WaterfallSegmentType.Positive || (PreviousWaterfallSegment.SegmentType == WaterfallSegmentType.Sum && series.YValues[(int)PreviousWaterfallSegment.xValue] >= 0))
                            lineY1 = PreviousWaterfallSegment.Top;
                        else
                            lineY1 = PreviousWaterfallSegment.Bottom;
                    }
                }
                //Setting Y-Coordinates for negative type segments
                else if (SegmentType == WaterfallSegmentType.Negative)
                {
                    lineY1 = Top;
                    lineY2 = Top;
                }
                //Setting Y-Coordinates for positive type segments
                else
                {
                    lineY1 = Bottom;
                    lineY2 = Bottom;
                }

                if (series.ChartArea != null && series.ChartArea.IsTransposed)
                {
                    if (SegmentType == WaterfallSegmentType.Negative && PreviousWaterfallSegment.SegmentType == WaterfallSegmentType.Positive)
                    {
                        lineX1 = PreviousWaterfallSegment.Right;
                        lineY1 = PreviousWaterfallSegment.Top;
                        lineX2 = Right;
                        lineY2 = Bottom;
                    }

                    if (PreviousWaterfallSegment.SegmentType == WaterfallSegmentType.Negative)
                    {
                        lineX1 = PreviousWaterfallSegment.Left;
                        lineY1 = PreviousWaterfallSegment.Top;

                        if (SegmentType == WaterfallSegmentType.Sum)
                        {
                            lineX2 = Right;
                            lineY2 = Bottom;
                        }
                        else
                        {
                            lineX2 = Left;
                            lineY2 = Bottom;
                        }
                    }

                    if (PreviousWaterfallSegment.SegmentType == WaterfallSegmentType.Positive && SegmentType == WaterfallSegmentType.Positive)
                    {
                        lineX1 = PreviousWaterfallSegment.Right;
                        lineY1 = PreviousWaterfallSegment.Top;
                        lineX2 = Left;
                        lineY2 = Bottom;
                    }

                    if (PreviousWaterfallSegment.SegmentType == WaterfallSegmentType.Negative && SegmentType == WaterfallSegmentType.Negative)
                    {
                        lineX1 = PreviousWaterfallSegment.Left;
                        lineY1 = PreviousWaterfallSegment.Top;
                        lineX2 = Right;
                        lineY2 = Bottom;
                    }

                    if (PreviousWaterfallSegment.SegmentType == WaterfallSegmentType.Positive && SegmentType == WaterfallSegmentType.Sum)
                    {
                        lineX1 = PreviousWaterfallSegment.Right;
                        lineY1 = PreviousWaterfallSegment.Top;
                        lineX2 = Right;
                        lineY2 = Bottom;
                    }

                    if (PreviousWaterfallSegment.SegmentType == WaterfallSegmentType.Sum)
                    {
                        lineX1 = PreviousWaterfallSegment.Right;
                        lineY1 = PreviousWaterfallSegment.Top;
                        if (SegmentType == WaterfallSegmentType.Positive)
                        {
                            lineX2 = Left;
                            lineY2 = Bottom;
                        }
                        if (SegmentType == WaterfallSegmentType.Negative)
                        {
                            lineX2 = Right;
                            lineY2 = Bottom;
                        }
                    }
                }

                if (series.ActualYAxis != null && series.ActualYAxis.IsInversed)
                {
                    lineX1 = PreviousWaterfallSegment.Right;
                    lineY1 = PreviousWaterfallSegment.Bottom;
                    lineX2 = Left;
                    lineY2 = Top;
                }
                else if (series.ActualXAxis != null && series.ActualXAxis.IsInversed && SegmentType == WaterfallSegmentType.Sum)
                {

                    lineX1 = PreviousWaterfallSegment.Left;
                    lineY1 = PreviousWaterfallSegment.Top;
                    lineX2 = Right;
                    lineY2 = Top;
                }

                float midPointX = ((lineX1 + lineX2) / 2);
                float midPointY = ((lineY1 + lineY2) / 2);
                lineX1 = midPointX + (lineX1 - midPointX);
                lineY1 = midPointY + (lineY1 - midPointY);
                lineX2 = lineX1 + (lineX2 - lineX1);
                lineY2 = lineY1 + (lineY2 - lineY1);
            }
        }

        #endregion

        #endregion
    }
}