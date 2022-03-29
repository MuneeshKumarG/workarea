using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class FastLineSegment : CartesianSegment, ILineDrawing
    {
        #region Fields

        /// <summary>
        /// Array holds drawing pixel positions.
        /// </summary>
        private float[]? drawPoints;
        private int arrayCount;
        internal List<double>? XValues;
        internal IList<double>? YValues;
        private bool enableAntiAliasing = false;
        private Brush? stroke;

#if __ANDROID__
        private readonly float displayScale;
#endif
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public FastLineSegment()
        {
#if __ANDROID__
#nullable disable
           displayScale = Android.Content.Res.Resources.System.DisplayMetrics.Density;
#nullable enable
#endif
        }

        #endregion

        #region Properties

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        Color ILineDrawing.Stroke
        {
            get
            {
                if (Fill is SolidColorBrush brush)
                    return brush.Color;
                else
                    return Colors.Black;
            }
            set => stroke = new SolidColorBrush(value);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        double ILineDrawing.StrokeWidth
        {
            get => StrokeWidth;
            set => StrokeWidth = value;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        bool ILineDrawing.EnableAntiAliasing
        {
            get
            {
                if (Series is FastLineSeries fastLineSeries)
                    return fastLineSeries.EnableAntiAliasing;
                else
                    return false;
            }
            set => enableAntiAliasing = value;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        float ILineDrawing.Opacity
        {
            get => Opacity;
            set => Opacity = value;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        DoubleCollection? ILineDrawing.StrokeDashArray
        {
            get => StrokeDashArray;
            set => StrokeDashArray = value;
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series == null || Empty || drawPoints == null)
            {
                return;
            }

            if (Series.CanAnimate())
            {
                AnimateSeriesClipRect(canvas, Series.AnimationValue);
            }

            canvas.DrawLines(drawPoints, this);
        }

        /// <inheritdoc />
        protected internal override void OnLayout()
        {
            var fastlineSeries = Series as FastLineSeries;

            if (fastlineSeries == null || fastlineSeries.ActualXAxis == null || fastlineSeries.ActualYAxis == null)
            {
                return;
            }

           var chart = fastlineSeries.ChartArea;

            if (XValues == null || chart == null || YValues == null)
                return;

            bool isTransposed = chart.IsTransposed;
            float preXPos = 0, preYPos = 0;
            double preXValue = 0d, preYValue = 0d;
            int dataCount = XValues.Count;
            float[] linePoints = new float[dataCount * 4];
            arrayCount = 0;

            var xAxis = fastlineSeries.ActualXAxis;
            float xAxisWidth = xAxis.RenderedRect.Width;
            float xAxisHeight = xAxis.RenderedRect.Height;
            float xAxisLeftOffset = xAxis.LeftOffset;
            float xAxisTopOffset = xAxis.TopOffset;
            double xStart = xAxis.VisibleRange.Start;
            double xDelta = xAxis.VisibleRange.Delta;
            double xEnd = xAxis.VisibleRange.End;
            bool xAxisIsVertical = xAxis.IsVertical;
            bool xAxisIsInversed = xAxis.IsInversed;

            var yAxis = fastlineSeries.ActualYAxis;
            float yAxisHeight = yAxis.RenderedRect.Height;
            float yAxisWidth = yAxis.RenderedRect.Width;
            float yAxisTopOffset = yAxis.TopOffset;
            float yAxisLeftOffset = yAxis.LeftOffset;
            double yDelta = yAxis.VisibleRange.Delta;
            double yEnd = yAxis.VisibleRange.End;
            double yStart = yAxis.VisibleRange.Start;
            bool yAxisIsVertical = yAxis.IsVertical;
            bool yAxisIsInversed = yAxis.IsInversed;

            double xSize = isTransposed ? xAxisHeight : xAxisWidth;
            double ySize = isTransposed ? yAxisWidth : yAxisHeight;

            double xTolerance = fastlineSeries.ToleranceCoefficient * (xDelta > 0 ? xDelta : -xDelta) / xSize;
            double yTolerance = fastlineSeries.ToleranceCoefficient * (yDelta > 0 ? yDelta : -yDelta) / ySize;

            if (XValues != null && !fastlineSeries.IsIndexed)
            {
                if (dataCount > 0)
                {
                    preXValue = XValues[0];
                    preYValue = YValues[0];

                    preXPos = fastlineSeries.TransformToVisibleX(preXValue, preYValue);
                    preYPos = fastlineSeries.TransformToVisibleY(preXValue, preYValue);
#if IOS || MACCATALYST
                    linePoints[arrayCount++] = preXPos;
                    linePoints[arrayCount++] = preYPos;
#endif
                }

                for (int i = 1; i < dataCount; i++)
                {
                    if (i >= XValues.Count || i >= YValues.Count)
                        break;

                    double xValue = XValues[i];
                    double yValue = YValues[i];

                    if ((xEnd <= xValue && xStart >= XValues[i - 1]) &&
                        ((yStart >= yValue && yEnd <= YValues[i - 1])
                        || (yEnd <= yValue && yStart >= YValues[i - 1])))
                    {
                        float x = fastlineSeries.TransformToVisibleX(xValue, yValue);
                        float y = fastlineSeries.TransformToVisibleY(xValue, yValue);
                        preXPos = fastlineSeries.TransformToVisibleX(XValues[i - 1], YValues[i - 1]);
                        preYPos = fastlineSeries.TransformToVisibleY(XValues[i - 1], YValues[i - 1]);

                        UpdateLinePoints(linePoints, preXPos, preYPos, x, y);

                        preXPos = x;
                        preYPos = y;
                        preXValue = xValue;
                        preYValue = yValue;
                    }
                    else if ((xValue <= xEnd && xValue >= xStart) ||
                        (yValue >= yStart && yValue <= yEnd) ||
                        (preXValue <= xEnd && preXValue >= xStart) ||
                        (preYValue <= yEnd && preYValue >= yStart) ||
                        ((i != dataCount - 1) && ((XValues[i + 1] <= xEnd && XValues[i + 1] >= xStart) ||
                        (YValues[i + 1] >= yStart && YValues[i + 1] <= yEnd))))
                    {
                        double xDiff = preXValue - xValue;
                        double yDiff = preYValue - yValue;

                        if ((xDiff > 0 ? xDiff : -xDiff) >= xTolerance ||
                            (yDiff > 0 ? yDiff : -yDiff) >= yTolerance ||
                            double.IsNaN(xDiff) || double.IsNaN(yDiff))
                        {
                            float x, y;

                            if (isTransposed)
                            {
                                x = ValueToPoint(yValue, yStart, yDelta, yAxisIsInversed, yAxisIsVertical,
                                    yAxisWidth, yAxisHeight, yAxisLeftOffset, yAxisTopOffset);

                                y = ValueToPoint(xValue, xStart, xDelta, xAxisIsInversed, xAxisIsVertical,
                                  xAxisWidth, xAxisHeight, xAxisLeftOffset, xAxisTopOffset);
                            }
                            else
                            {
                                x = ValueToPoint(xValue, xStart, xDelta, xAxisIsInversed, xAxisIsVertical,
                                   xAxisWidth, xAxisHeight, xAxisLeftOffset, xAxisTopOffset);

                                y = ValueToPoint(yValue, yStart, yDelta, yAxisIsInversed, yAxisIsVertical,
                                   yAxisWidth, yAxisHeight, yAxisLeftOffset, yAxisTopOffset);
                            }

                            UpdateLinePoints(linePoints, preXPos, preYPos, x, y);

                            preXPos = x;
                            preYPos = y;
                            preXValue = xValue;
                            preYValue = yValue;
                        }
                    }
                }
            }
            else
            {
                if (dataCount > 0)
                {
                    preXValue = 0;
                    preYValue = YValues[0];
                    preXPos = fastlineSeries.TransformToVisibleX(preXValue, preYValue);
                    preYPos = fastlineSeries.TransformToVisibleY(preXValue, preYValue);
#if IOS || MACCATALYST
                    linePoints[arrayCount++] = preXPos;
                    linePoints[arrayCount++] = preYPos;
#endif
                }

                for (int i = 1; i < dataCount; i++)
                {
                    if (i >= YValues.Count)
                        break;

                    double yValue = YValues[i];
                    if ((i <= xEnd + 1) && (i >= xStart - 1))
                    {
                        double xDiff = preXValue - i;
                        double yDiff = preYValue - yValue;

                        if ((xDiff > 0 ? xDiff : -xDiff) >= xTolerance
                            || (yDiff > 0 ? yDiff : -yDiff) >= yTolerance || double.IsNaN(xDiff) || double.IsNaN(yDiff))
                        {
                            float x, y;

                            if (isTransposed)
                            {
                                x = ValueToPoint(yValue, yStart, yDelta, yAxisIsInversed, yAxisIsVertical,
                                       yAxisWidth, yAxisHeight, yAxisLeftOffset, yAxisTopOffset);

                                y = ValueToPoint(i, xStart, xDelta, xAxisIsInversed, xAxisIsVertical,
                                   xAxisWidth, xAxisHeight, xAxisLeftOffset, xAxisTopOffset);
                            }
                            else
                            {
                                x = ValueToPoint(i, xStart, xDelta, xAxisIsInversed, xAxisIsVertical,
                                   xAxisWidth, xAxisHeight, xAxisLeftOffset, xAxisTopOffset);

                                y = ValueToPoint(yValue, yStart, yDelta, yAxisIsInversed, yAxisIsVertical,
                                    yAxisWidth, yAxisHeight, yAxisLeftOffset, yAxisTopOffset);
                            }

                            UpdateLinePoints(linePoints, preXPos, preYPos, x, y);

                            preXPos = x;
                            preYPos = y;
                            preXValue = i;
                            preYValue = yValue;
                        }
                    }
                }
            }

            this.drawPoints = new float[arrayCount];
            //Here, we copied calculated linePoints to DrawPoints. Because we need float array draw points for fast line rendering. 
            //So, we created empty float array with user given data size. Based on series's ToleranceCoefficient, we reduce draw points. 
            //Then we copy calculated draw points alone for drawing. 
            Array.Copy(linePoints, 0, this.drawPoints, 0, arrayCount);
        }

        /// <summary>
        /// Sets the values for this segment.
        /// </summary>
        /// <param name="xValues">Segment xvalues.</param>
        /// <param name="yValues">Segment yvalues.</param>
        internal void SetData(List<double> xValues, IList<double> yValues)
        {
            if (Series is XYDataSeries series && series.ActualYAxis != null)
            {
                double xMin = double.MaxValue, xMax = double.MinValue, yMin = double.MaxValue, yMax = double.MinValue;
                XValues = xValues;
                YValues = yValues;
                int dataCount = YValues.Count;

                if (dataCount == 0)
                {
                    return;
                }

                if (series.IsIndexed)
                {
                    xMin = 0;
                    xMax = dataCount - 1;
                    for (int i = 0; i < dataCount; i++)
                    {
                        if (i >= YValues.Count)
                            break;

                        double yValue = YValues[i];

                        if (!Empty && double.IsNaN(yValue))
                            Empty = true;

                        if (yValue > yMax)
                        {
                            yMax = yValue;
                        }

                        if (yValue < yMin)
                        {
                            yMin = yValue;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < dataCount; i++)
                    {
                        if (i >= XValues.Count || i >= YValues.Count)
                            break;

                        var xValue = XValues[i];
                        var yValue = YValues[i];

                        if (!Empty && (double.IsNaN(yValue) || double.IsNaN(xValue)))
                            Empty = true;

                        if (xValue > xMax)
                        {
                            xMax = xValue;
                        }

                        if (xValue < xMin)
                        {
                            xMin = xValue;
                        }

                        if (yValue > yMax)
                        {
                            yMax = yValue;
                        }

                        if (yValue < yMin)
                        {
                            yMin = yValue;
                        }
                    }
                }

                if (xMin == double.MaxValue)
                {
                    xMin = double.NaN;
                }

                if (xMax == double.MinValue)
                {
                    xMax = double.NaN;
                }

                if (yMin == double.MaxValue)
                {
                    yMin = double.NaN;
                }

                if (yMax == double.MinValue)
                {
                    yMax = double.NaN;
                }


                Series.XRange += new DoubleRange(xMin, xMax);
                Series.YRange += new DoubleRange(yMin, yMax);
            }
        }

        /// <summary>
        /// Update calculated pixels values in array.
        /// </summary>
        /// <param name="linePoints"></param>
        /// <param name="preXPos"></param>
        /// <param name="preYPos"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void UpdateLinePoints(float[] linePoints, float preXPos, float preYPos, float x, float y)
        {
            if (!double.IsNaN(preYPos))
            {
#if __ANDROID__
                linePoints[arrayCount++] = preXPos * displayScale;
                linePoints[arrayCount++] = preYPos * displayScale;
                linePoints[arrayCount++] = x * displayScale;
                linePoints[arrayCount++] = y * displayScale;
#elif WINDOWS
                linePoints[arrayCount++] = preXPos;
                linePoints[arrayCount++] = preYPos;
                linePoints[arrayCount++] = x;
                linePoints[arrayCount++] = y;
#else

                linePoints[arrayCount++] = x;
                linePoints[arrayCount++] = y;
#endif
            }
        }

        /// <summary>
        /// Method used to calculate data value to pixel value. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <param name="delta"></param>
        /// <param name="isInversed"></param>
        /// <param name="isVertical"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="leftOffset"></param>
        /// <param name="topOffset"></param>
        /// <returns></returns>
        private float ValueToPoint(double value, double start, double delta, bool isInversed,
           bool isVertical, float width, float height, float leftOffset, float topOffset)
        {
            double result;

            result = (value - start) / delta;

            float coefficient = (float)(isInversed ? 1f - result : result);

            if (!isVertical)
            {
                return (width * coefficient) + leftOffset;
            }
            else
            {
                return (height * (1 - coefficient)) + topOffset;
            }
        }

        #endregion
    }
}
