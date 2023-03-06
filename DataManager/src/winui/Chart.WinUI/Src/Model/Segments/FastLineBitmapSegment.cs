using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class FastLineBitmapSegment : ChartSegment
    {
        #region Fields

        #region Private Fields
        
        private List<double> xValues = new List<double>();

        private List<double> yValues = new List<double>();

        private IList<double> xChartVals;

        private IList<double> yChartVals;

        private int[] points1;

        private int[] points2;

        private Point intersectingPoint;

        private WriteableBitmap bitmap;

        private byte[] fastBuffer;

        private double xStart, xEnd, yStart, yEnd, xDelta, yDelta, xSize, ySize, xOffset, yOffset;

        private double xTolerance, yTolerance;

        private int count, start;

        private bool isSeriesSelected;

        Windows.UI.Color seriesSelectionColor = Colors.Transparent;

        bool isGrouping = true;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public FastLineBitmapSegment()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="series"></param>
        public FastLineBitmapSegment(ChartSeries series)
        {
            base.Series = series;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xVals"></param>
        /// <param name="yVals"></param>
        /// <param name="series"></param>
        public FastLineBitmapSegment(IList<double> xVals, IList<double> yVals, ChartSeries series)
            : this(series)
        {
            base.Series = series;
            if (Series.ActualXAxis is CategoryAxis && !(Series.ActualXAxis as CategoryAxis).IsIndexed)
                base.Item = series.GroupedActualData;
            else
                base.Item = series.ActualData;
            this.xChartVals = xVals;
            this.yChartVals = yVals;

            SetRange();
        }

        #endregion

        #region Methods

        #region Public Override Methods

        /// <inheritdoc/>     
        internal override void SetData(IList<double> xVals, IList<double> yVals)
        {
            this.xChartVals = xVals;
            this.yChartVals = yVals;

            SetRange();
        }

        /// <inheritdoc/>    
        internal override UIElement CreateVisual(Size size)
        {
            var fastSeries = Series;
            bitmap = fastSeries.Chart.GetFastRenderSurface();
            fastBuffer = fastSeries.Chart.GetFastBuffer();

            return null;
        }

        /// <inheritdoc/>
        internal override void OnSizeChanged(Size size)
        {
            var fastSeries = Series;
            bitmap = fastSeries.Chart.GetFastRenderSurface();
            fastBuffer = fastSeries.Chart.GetFastBuffer();
        }

        /// <inheritdoc/>
        internal override UIElement GetRenderedVisual()
        {
            return null;
        }

        /// <inheritdoc/>
        internal override void Update(IChartTransformer transformer)
        {
            var fastSeries = Series;
            bitmap = fastSeries.Chart.GetFastRenderSurface();
            //if (transformer != null && chartPoints != null && chartPoints.Count > 1)
            if (transformer != null && fastSeries.PointsCount > 1)
            {
                ChartTransform.ChartCartesianTransformer cartesianTransformer =
                    transformer as ChartTransform.ChartCartesianTransformer;
                x_isInversed = cartesianTransformer.XAxis.IsInversed;
                y_isInversed = cartesianTransformer.YAxis.IsInversed;
                xStart = cartesianTransformer.XAxis.VisibleRange.Start;
                xEnd = cartesianTransformer.XAxis.VisibleRange.End;
                yStart = cartesianTransformer.YAxis.VisibleRange.Start;
                yEnd = cartesianTransformer.YAxis.VisibleRange.End;
                xDelta = x_isInversed ? xStart - xEnd : xEnd - xStart;
                yDelta = y_isInversed ? yStart - yEnd : yEnd - yStart;
                if (fastSeries.IsActualTransposed)
                {
                    ySize = (int)cartesianTransformer.YAxis.RenderedRect.Width;
                    xSize = (int)cartesianTransformer.XAxis.RenderedRect.Height;
                    yOffset = cartesianTransformer.YAxis.RenderedRect.Left - fastSeries.Chart.SeriesClipRect.Left;
                    xOffset = cartesianTransformer.XAxis.RenderedRect.Top - fastSeries.Chart.SeriesClipRect.Top;
                }
                else
                {
                    ySize = (int)cartesianTransformer.YAxis.RenderedRect.Height;
                    xSize = (int)cartesianTransformer.XAxis.RenderedRect.Width;
                    yOffset = cartesianTransformer.YAxis.RenderedRect.Top - fastSeries.Chart.SeriesClipRect.Top;
                    xOffset = cartesianTransformer.XAxis.RenderedRect.Left - fastSeries.Chart.SeriesClipRect.Left;
                }
                xTolerance = Math.Abs((xDelta * 1) / xSize);
                yTolerance = Math.Abs((yDelta * 1) / ySize);
                count = (int)(Math.Ceiling(xEnd));
                start = (int)(Math.Floor(xStart));
                if (x_isInversed)
                {
                    double temp = xStart;
                    xStart = xEnd;
                    xEnd = temp;
                }
                if (y_isInversed)
                {
                    double temp = yStart;
                    yStart = yEnd;
                    yEnd = temp;
                }
                xValues.Clear();
                yValues.Clear();
                TransformToScreenCo();
                UpdateVisual(true);
            }
        }

        #endregion

        #region Internal Methods

        internal void SetRange()
        {
            isGrouping = (Series.ActualXAxis is CategoryAxis) ? (Series.ActualXAxis as CategoryAxis).ArrangeByIndex : true;
            if (Series.PointsCount > 0)
            {
                double _Min = yChartVals.Min();
                double Y_MIN;
                if (double.IsNaN(_Min))
                {
                    var yVal = yChartVals.Where(e => !double.IsNaN(e));
                    Y_MIN = (!yVal.Any()) ? 0 : yVal.Min();
                }
                else
                {
                    Y_MIN = _Min;
                }
                if (Series.IsIndexed)
                {
                    double X_MAX = !isGrouping ? xChartVals.Max() : Series.PointsCount - 1;
                    double Y_MAX = yChartVals.Max();
                    double X_MIN = 0;

                    XRange = new DoubleRange(X_MIN, X_MAX);
                    YRange = new DoubleRange(Y_MIN, Y_MAX);
                }
                else
                {
                    double X_MAX = xChartVals.Max();
                    double Y_MAX = yChartVals.Max();
                    double X_MIN = xChartVals.Min();

                    XRange = new DoubleRange(X_MIN, X_MAX);
                    YRange = new DoubleRange(Y_MIN, Y_MAX);
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        internal void UpdateVisual(bool updatePolyline)
        {
            var fastSeries = Series as XyDataSeries;
            ChartBase chart = fastSeries.Chart;
            isSeriesSelected = false;
            bool isMultiColor = fastSeries.PaletteBrushes != null;
            //(WPF-14137) PaletteBrushes and Interior not working properly in series and Null Exception thrown, while without give custom brushes.
            var checkColor = isMultiColor
                            ? GetColor(fastSeries.GetInteriorColor(0))
                            : (this.Fill == null) ? (new SolidColorBrush(Colors.Transparent)).Color : ((SolidColorBrush)this.Fill).Color;


            //Set SeriesSelectionBrush and Check EnableSeriesSelection        
            if (chart.GetEnableSeriesSelection())
            {
                Brush seriesSelectionBrush = chart.GetSeriesSelectionBrush(fastSeries);
                if (seriesSelectionBrush != null && chart.SelectedSeriesCollection.Contains(fastSeries))
                {
                    isSeriesSelected = true;
                    seriesSelectionColor = ((SolidColorBrush)seriesSelectionBrush).Color;
                }
            }

            Windows.UI.Color color = isSeriesSelected ? seriesSelectionColor : checkColor;

            //byte[] fastBuffer = new byte[(int)(availableSize.Width * availableSize.Height * 4)];
            //WPF-14142 : FastLineBitmapSeries not working, While setting the StrokeWidth as 0

            if (bitmap != null && xValues.Count > 1 && fastSeries.StrokeWidth > 0)
            {
                fastBuffer = fastSeries.Chart.GetFastBuffer();

                xStart = xValues[0];
                yStart = yValues[0];

                int width = (int)fastSeries.Chart.SeriesClipRect.Width;
                int height = (int)fastSeries.Chart.SeriesClipRect.Height;

                double leftThickness = fastSeries.StrokeWidth / 2;
                double rightThickness = (fastSeries.StrokeWidth % 2 == 0
                                                ? (fastSeries.StrokeWidth / 2) - 1
                                                : fastSeries.StrokeWidth / 2);

                if (fastSeries is FastLineBitmapSeries)
                {
                    var fastLineBitmapSeries = (FastLineBitmapSeries)fastSeries;

                    if (((FastLineBitmapSeries)fastSeries).EnableAntiAliasing)
                    {
                        if (fastLineBitmapSeries.StrokeDashArray == null
                            ||
                            (fastLineBitmapSeries.StrokeDashArray != null &&
                             fastLineBitmapSeries.StrokeDashArray.Count <= 1))
                        {
                            if ((fastSeries.IsActualTransposed))
                                DrawLineAa(yValues, xValues, width, height, color, leftThickness, rightThickness,
                                                   isMultiColor);
                            else
                            {
                                DrawLineAa(xValues, yValues, width, height, color, leftThickness, rightThickness,
                                                     isMultiColor);
                            }
                        }
                        else
                            DrawDashedAaLines(width, height, color, leftThickness, rightThickness);
                    }
                    else
                    {
                        if (fastLineBitmapSeries.StrokeDashArray == null
                            ||
                            (fastLineBitmapSeries.StrokeDashArray != null &&
                             fastLineBitmapSeries.StrokeDashArray.Count <= 1))
                        {
                            if (fastSeries.IsActualTransposed)
                                DrawLine(yValues, xValues, width, height, color, leftThickness,
                                                          rightThickness, isMultiColor);
                            else
                            {

                                DrawLine(xValues, yValues, width, height, color, leftThickness,
                                                            rightThickness, isMultiColor);
                            }
                        }
                        else
                            DrawDashedLines(width, height, color, leftThickness, rightThickness);
                    }
                }

            }

            fastSeries.Chart.CanRenderToBuffer = true;

            xValues.Clear();
            yValues.Clear();

        }

#endregion

#region Protected Override Methods

        /// <inheritdoc/>
        internal override void SetVisualBindings(Shape element)
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Fill");
            element.SetBinding(Shape.StrokeProperty, binding);
            binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("StrokeWidth");
            element.SetBinding(Shape.StrokeThicknessProperty, binding);
        }

#endregion

#region Private Static Methods

        private static void GetLinePoints(double x1, double y1, double x2, double y2,
                             double leftThickness, double rightThickness, int[] points)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var radian = Math.Atan2(dy, dx);
            var cos = Math.Cos(-radian);
            var sin = Math.Sin(-radian);
            var x11 = (x1 * cos) - (y1 * sin);
            var y11 = (x1 * sin) + (y1 * cos);
            var x12 = (x2 * cos) - (y2 * sin);
            var y12 = (x2 * sin) + (y2 * cos);
            cos = Math.Cos(radian);
            sin = Math.Sin(radian);
            var leftTopX = (x11 * cos) - ((y11 + leftThickness) * sin);
            var leftTopY = (x11 * sin) + ((y11 + leftThickness) * cos);
            var rightTopX = (x12 * cos) - ((y12 + leftThickness) * sin);
            var rightTopY = (x12 * sin) + ((y12 + leftThickness) * cos);
            var leftBottomX = (x11 * cos) - ((y11 - rightThickness) * sin);
            var leftBottomY = (x11 * sin) + ((y11 - rightThickness) * cos);
            var rightBottomX = (x12 * cos) - ((y12 - rightThickness) * sin);
            var rightBottomY = (x12 * sin) + ((y12 - rightThickness) * cos);
            points[0] = (int)leftTopX;
            points[1] = (int)leftTopY;
            points[2] = (int)rightTopX;
            points[3] = (int)rightTopY;
            points[4] = (int)rightBottomX;
            points[5] = (int)rightBottomY;
            points[6] = (int)leftBottomX;
            points[7] = (int)leftBottomY;
            points[8] = (int)leftTopX;
            points[9] = (int)leftTopY;
        }
        private static double CalcLenOfLine(double x1, double x2, double y1, double y2)
        {
            double x = x2 - x1;
            double y = y2 - y1;
            return Math.Sqrt((x * x) + (y * y));
        }

#endregion

#region Private Methods

        private void TransformToScreenCo()
        {
            if (!Series.IsActualTransposed)
            {
                TransformToScreenCoHorizontal();
            }
            else
            {
                TransformToScreenCoVertical();
            }
        }

        private void TransformToScreenCoHorizontal()
        {
            var fastSeries = Series;
            int i = 0;
            double prevXValue = 0;
            double prevYValue = 0;
            double yCoefficient = 0;
            var numericalAxis = fastSeries.ActualYAxis as NumericalAxis;

            if (fastSeries.IsIndexed)
            {
                start = start < 0 ? 0 : start;
                prevXValue = 1;
                if (!isGrouping)
                    count = xChartVals.Count - 1;
                else
                    count = count > yChartVals.Count - 1 ? yChartVals.Count - 1 : count;
                for (i = start; i <= count; i++)
                {
                    double yVal = 0;
                    if (yChartVals.Count > i)
                        yVal = yChartVals[i];
                    if (Math.Abs(prevXValue - i) >= xTolerance || Math.Abs(prevYValue - yVal) >= yTolerance)
                    {
                        xValues.Add((xOffset + xSize * ((!isGrouping ? xChartVals[i] : i - xStart) / xDelta)));
                        yCoefficient = 1 - (yVal - yStart) / yDelta;
                        yValues.Add((yOffset + ySize * yCoefficient));
                        prevXValue = i;
                        prevYValue = yVal;
                    }
                }

                if (start > 0 && start < yChartVals.Count)
                {
                    i = start - 1;
                    double yVal = yChartVals[i];
                    xValues.Insert(0, (xOffset + xSize * ((i - xStart) / xDelta)));
                    yCoefficient = 1 - (yVal - yStart) / yDelta;
                    yValues.Insert(0, (yOffset + ySize * yCoefficient));
                }

                if (count < yChartVals.Count - 1)
                {
                    i = count + 1;
                    double yVal = yChartVals[i];
                    xValues.Add(xOffset + xSize * ((i - xStart) / xDelta));
                    yCoefficient = 1 - (yVal - yStart) / yDelta;
                    yValues.Add((yOffset + ySize * yCoefficient));
                }
            }
            else
            {
                int startIndex = 0;
                prevXValue = xChartVals[0];
                prevYValue = yChartVals[0];
                int cnt = xChartVals.Count - 1;
                if (fastSeries.IsLinearData)
                {

                    for (i = 1; i < cnt; i++)
                    {
                        double xVal = xChartVals[i];
                        double yVal = yChartVals[i];

                        if ((xVal <= xEnd) && (xVal >= xStart))
                        {
                            if (Math.Abs(prevXValue - xVal) >= xTolerance || Math.Abs(prevYValue - yVal) >= yTolerance)
                            {
                                xValues.Add((xOffset + xSize * ((xVal - xStart) / xDelta)));
                                yCoefficient = 1 - (yVal - yStart) / yDelta;
                                yValues.Add((yOffset + ySize * yCoefficient));
                                prevXValue = xVal;
                                prevYValue = yVal;
                            }
                        }
                        else if (xVal < xStart)
                        {
                            if (x_isInversed)
                            {
                                xValues.Add((xOffset + xSize * ((xVal - xStart) / xDelta)));
                                yCoefficient = 1 - (yVal - yStart) / yDelta;
                                yValues.Add(yOffset + ySize * yCoefficient);
                            }
                            else
                                startIndex = i;
                        }
                        else if (xVal > xEnd)
                        {
                            xValues.Add((xOffset + xSize * ((xVal - xStart) / xDelta)));
                            yCoefficient = 1 - (yVal - yStart) / yDelta;
                            yValues.Add((yOffset + ySize * yCoefficient));
                            break;
                        }
                    }
                }
                else
                {
                    for (i = 1; i < cnt; i++)
                    {
                        double xVal = xChartVals[i];
                        double yVal = yChartVals[i];
                        if (Math.Abs(prevXValue - xVal) >= xTolerance || Math.Abs(prevYValue - yVal) >= yTolerance)
                        {
                            xValues.Add((xOffset + xSize * ((xVal - xStart) / xDelta)));
                            yCoefficient = 1 - (yVal - yStart) / yDelta;
                            yValues.Add((yOffset + ySize * yCoefficient));
                            prevXValue = xVal;
                            prevYValue = yVal;
                        }

                    }
                }

                xValues.Insert(0, xOffset + xSize * ((xChartVals[startIndex] - xStart) / xDelta));
                yCoefficient = 1 - (yChartVals[startIndex] - yStart) / yDelta;
                yValues.Insert(0, yOffset + ySize * yCoefficient);

                if (i == cnt)
                {
                    xValues.Add(xOffset + xSize * ((xChartVals[cnt] - xStart) / xDelta));
                    yCoefficient = 1 - (yChartVals[cnt] - yStart) / yDelta;
                    yValues.Add(yOffset + ySize * yCoefficient);
                }
            }
        }

        private void TransformToScreenCoVertical()
        {
            double prevXValue = 0;
            double prevYValue = 0;
            int i = 0;
            double yCoefficient = 0;
            var numericalAxis = Series.ActualYAxis as NumericalAxis;

            if (Series.IsIndexed)
            {
                start = start < 0 ? 0 : start;
                if (!isGrouping)
                    count = xChartVals.Count - 1;
                else
                    count = count > yChartVals.Count - 1 ? yChartVals.Count - 1 : count;
                prevXValue = 1;
                for (i = start; i <= count; i++)
                {
                    double yVal = yChartVals[i];
                    if (Math.Abs(prevXValue - i) >= xTolerance || Math.Abs(prevYValue - yVal) >= yTolerance)
                    {
                        xValues.Add((xOffset + xSize * ((xEnd - i) / xDelta)));
                        yCoefficient = 1 - (yEnd - yVal) / yDelta;
                        yValues.Add((yOffset + ySize * yCoefficient));
                        prevXValue = i;
                        prevYValue = yVal;
                    }
                }

                if (start > 0 && start < yChartVals.Count)
                {
                    i = start - 1;
                    double yVal = yChartVals[i];
                    xValues.Insert(0, (xOffset + xSize * ((xEnd - i) / xDelta)));
                    yCoefficient = 1 - (yEnd - yVal) / yDelta;
                    yValues.Insert(0, (yOffset + ySize * yCoefficient));
                }

                if (count < yChartVals.Count - 1)
                {
                    i = count + 1;
                    double yVal = yChartVals[i];
                    xValues.Add(xOffset + xSize * ((xEnd - i) / xDelta));
                    yCoefficient = 1 - (yEnd - yVal) / yDelta;
                    yValues.Add((yOffset + ySize * yCoefficient));
                }
            }
            else
            {
                int startIndex = 0;

                prevXValue = xChartVals[0];
                prevYValue = yChartVals[0];
                int cnt = xChartVals.Count - 1;

                if (Series.IsLinearData)
                {
                    for (i = 1; i < cnt; i++)
                    {
                        double xVal = xChartVals[i];
                        double yVal = yChartVals[i];

                        if ((xVal <= xEnd) && (xVal >= xStart))
                        {
                            if (Math.Abs(prevXValue - xVal) >= xTolerance || Math.Abs(prevYValue - yVal) >= yTolerance)
                            {
                                xValues.Add((xOffset + xSize * ((xEnd - xVal) / xDelta)));
                                yCoefficient = 1 - (yEnd - yVal) / yDelta;
                                yValues.Add((yOffset + ySize * yCoefficient));
                                prevXValue = xVal;
                                prevYValue = yVal;
                            }
                        }
                        else if (xVal < xStart)
                        {
                            if (x_isInversed)
                            {
                                xValues.Add((xOffset + xSize * ((xEnd - xVal) / xDelta)));
                                yCoefficient = 1 - (yEnd - yVal) / yDelta;
                                yValues.Add((yOffset + ySize * yCoefficient));
                            }
                            else
                                startIndex = i;
                        }
                        else if (xVal > xEnd)
                        {
                            xValues.Add((xOffset + xSize * ((xEnd - xVal) / xDelta)));
                            yCoefficient = 1 - (yEnd - yVal) / yDelta;
                            yValues.Add((yOffset + ySize * yCoefficient));
                            break;
                        }
                    }
                }
                else
                {
                    for (i = 1; i < cnt; i++)
                    {
                        double xVal = xChartVals[i];
                        double yVal = yChartVals[i];
                        if (Math.Abs(prevXValue - xVal) >= xTolerance || Math.Abs(prevYValue - yVal) >= yTolerance)
                        {
                            xValues.Add((xOffset + xSize * ((xEnd - xVal) / xDelta)));
                            yCoefficient = 1 - (yEnd - yVal) / yDelta;
                            yValues.Add((yOffset + ySize * yCoefficient));
                            prevXValue = xVal;
                            prevYValue = yVal;
                        }
                    }
                }

                xValues.Insert(0, xOffset + xSize * ((xEnd - xChartVals[startIndex]) / xDelta));
                yCoefficient = 1 - (yEnd - yChartVals[startIndex]) / yDelta;
                yValues.Insert(0, yOffset + ySize * yCoefficient);
                if (i == cnt)
                {
                    xValues.Add(xOffset + xSize * ((xEnd - xChartVals[cnt]) / xDelta));
                    yCoefficient = 1 - (yEnd - yChartVals[cnt]) / yDelta;
                    yValues.Add(yOffset + ySize * yCoefficient);
                }
            }
        }

        private void DrawLine(List<double> xVals, List<double> yVals, int width, int height, Windows.UI.Color color,
                                    double leftThickness, double rightThickness, bool isMultiColor)
        {
            var fastSeries = Series as XyDataSeries;
            xStart = xVals[0];
            yStart = yVals[0];
            xEnd = 0;
            yEnd = 0;
            bool isNonEmptyPoint;
            if (fastSeries.StrokeWidth <= 1)
            {
                for (int i = 1; i < xVals.Count; i++)
                {
                    xEnd = xVals[i];
                    yEnd = yVals[i];
                    isNonEmptyPoint = CheckEmptyPoint();
                    if (isNonEmptyPoint)
                    {
                        if (fastSeries.Chart.IsMultipleArea && fastSeries.Clip != null)
                        {
                            var clip = fastSeries.Clip.Bounds;

                            bitmap.DrawLineBresenham(fastBuffer, width, height, (int)xStart, (int)yStart, (int)xEnd, (int)yEnd, color
                            , fastSeries.bitmapPixels, clip);
                        }
                        else
                        {
                            bitmap.DrawLineBresenham(fastBuffer, width, height, (int)xStart, (int)yStart, (int)xEnd, (int)yEnd, color
                          , fastSeries.bitmapPixels);
                        }
                    }

                    xStart = xEnd;
                    yStart = yEnd;
                    if (isSeriesSelected)
                        color = seriesSelectionColor;
                    else if (isMultiColor)
                        color = GetColor(fastSeries.GetInteriorColor(i - 1));
                }
            }
            else
            {
                if (points1 == null)
                    points1 = new int[10];
                xEnd = xVals[1];
                yEnd = yVals[1];
                GetLinePoints(xStart, yStart, xEnd, yEnd, leftThickness, rightThickness, points1);
                for (int i = 1; i < xVals.Count;)
                {
                    points2 = new int[10];
                    xStart = xEnd;
                    yStart = yEnd;
                    i++;
                    if (i < xVals.Count)
                    {
                        xEnd = xVals[i];
                        yEnd = yVals[i];
                        UpdatePoints2(xStart, yStart, xEnd, yEnd, leftThickness, rightThickness);
                    }
                    DrawLine(color, width, height);
                    points1 = points2;
                    points2 = null;
                    if (isSeriesSelected)
                        color = seriesSelectionColor;
                    else if (isMultiColor)
                        color = GetColor(fastSeries.GetInteriorColor(i - 1));
                }

                points1 = null;
                points2 = null;
            }
        }

        private bool FindIntersectingPoints(double x11, double y11, double x12, double y12,
                                          double x21, double y21, double x22, double y22)
        {
            double pixelHeight;
            pixelHeight = bitmap.PixelHeight;
            if (y11 <= -pixelHeight) y11 = -2 * pixelHeight;
            if (y12 <= -pixelHeight) y12 = -2 * pixelHeight;
            if (y21 <= -pixelHeight) y21 = -2 * pixelHeight;
            if (y22 <= -pixelHeight) y22 = -2 * pixelHeight;

            if (y11 >= 2 * pixelHeight) y11 = 4 * pixelHeight;
            if (y12 >= 2 * pixelHeight) y12 = 4 * pixelHeight;
            if (y21 >= 2 * pixelHeight) y21 = 4 * pixelHeight;
            if (y22 >= 2 * pixelHeight) y22 = 4 * pixelHeight;

            intersectingPoint = new Point();
            //First  line  slope and intersect( y = mx+c )
            double m = WriteableBitmapExtensions.Slope(x11, y11, x12, y12);
            double c = double.NaN;
            if (!double.IsInfinity(m))
                c = WriteableBitmapExtensions.Intersect(x12, y12, m);

            //Second line slope and intersect
            double m1 = WriteableBitmapExtensions.Slope(x21, y21, x22, y22);
            double c1 = double.NaN;
            if (!double.IsInfinity(m1))
                c1 = WriteableBitmapExtensions.Intersect(x21, y21, m1);


            // point intersecting for both straight line.(Cross point for lines)
            double x = (c1 - c) / (m - m1);
            intersectingPoint.X = (int)x;
            intersectingPoint.Y = (int)(m * x) + c;

            return (double.IsNaN(x) || double.IsNaN(intersectingPoint.Y)) ? false : true;
        }

        private void UpdatePoints2(double xStart, double yStart, double xEnd, double yEnd,
                                   double leftThickness, double rightThickness)
        {
            GetLinePoints(xStart, yStart, xEnd, yEnd, leftThickness, rightThickness, points2);

            bool parallelLine = false;

            if (points1[0] == points1[2])
                parallelLine = !(points2[0] == points2[2] && points1[0] != points2[0]);
            else if (points2[0] == points2[2])
                parallelLine = true;
            else
            {
                // both lines are not parallel to the y-axis;
                var m1 = Math.Floor(WriteableBitmapExtensions.Slope(points1[2], points1[1], points1[0], points1[3]));
                var m2 = Math.Floor(WriteableBitmapExtensions.Slope(points2[2], points2[1], points2[0], points2[3]));
                if (m1 != m2) parallelLine = true;
            }

            if (parallelLine)
            {
                bool line1 = FindIntersectingPoints(points1[0], points1[1], points1[2], points1[3], points2[0], points2[1], points2[2], points2[3]);
                if (line1)
                {
                    var intersectingX = (int)intersectingPoint.X;

                    //  WPF-48371 Prevent intersection point deviating from segment.
                    if ((points1[0] < intersectingX && intersectingX < points2[2]) || points1[2] > points2[2] || points1[0] > points2[0])
                    {
                        points1[2] = points2[0] = points2[8] = intersectingX;
                        points1[3] = points2[1] = points2[9] = (int)(intersectingPoint.Y);
                    }
                }
            }
            parallelLine = false;
            if (points1[4] == points1[6])
                parallelLine = !(points2[4] == points2[6] && points1[4] != points2[4]);
            else if (points2[4] == points2[6])
                parallelLine = true;
            else
            {
                var m3 = Math.Floor(WriteableBitmapExtensions.Slope(points1[6], points1[5], points1[4], points1[7]));
                var m4 = Math.Floor(WriteableBitmapExtensions.Slope(points2[6], points2[5], points2[4], points2[7]));
                if (m3 != m4) parallelLine = true;
            }
            if (parallelLine)
            {
                bool line = FindIntersectingPoints(points1[4], points1[5], points1[6], points1[7], points2[4], points2[5], points2[6], points2[7]);
                if (line)
                {
                    var intersectingX = (int)intersectingPoint.X;

                    //  WPF-48371 Prevent intersection point deviating from segment.
                    if((points1[6] < intersectingX && intersectingX < points2[4]) || points1[4] > points2[4] || points1[6] > points2[6])
                    {
                        points1[5] = points2[7] = (int)(intersectingPoint.Y);
                        points1[4] = points2[6] = (int)(intersectingPoint.X);
                    }
                }
            }
        }


        private bool CheckEmptyPoint()
        {
            if (!Series.IsActualTransposed)
                return (!double.IsNaN(yStart) && !double.IsNaN(yEnd));
            else
                return (!double.IsNaN(xStart) && !double.IsNaN(xEnd));
        }

        private void DrawLineAa(List<double> xVals, List<double> yVals, int width, int height, Windows.UI.Color color, double leftThickness,
                                double rightThickness, bool isMultiColor)
        {
            var fastSeries = Series as XyDataSeries;
            xStart = xVals[0];
            yStart = yVals[0];
            xEnd = 0;
            yEnd = 0;
            bool isNonEmptyPoint;
            if (fastSeries.StrokeWidth <= 1)
            {
                for (int i = 1; i <= xVals.Count - 1; i++)
                {
                    xEnd = xVals[i];
                    yEnd = yVals[i];
                    isNonEmptyPoint = CheckEmptyPoint();
                    if (isNonEmptyPoint)
                    {
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)xStart, (int)yStart, (int)xEnd, (int)yEnd, color, fastSeries.bitmapPixels);
                    }
                    if (isSeriesSelected)
                        color = seriesSelectionColor;
                    else if (isMultiColor)
                        color = GetColor(fastSeries.GetInteriorColor(i - 1));
                    xStart = xEnd;
                    yStart = yEnd;

                }
            }
            else
            {
                if (points1 == null)
                    points1 = new int[10];
                xEnd = xVals[1];
                yEnd = yVals[1];
                GetLinePoints(xStart, yStart, xEnd, yEnd, leftThickness, rightThickness, points1);
                for (int i = 1; i < xVals.Count;)
                {
                    points2 = new int[10];
                    xStart = xEnd;
                    yStart = yEnd;
                    i++;
                    if (i < xVals.Count)
                    {
                        xEnd = xVals[i];
                        yEnd = yVals[i];
                        UpdatePoints2(xStart, yStart, xEnd, yEnd, leftThickness, rightThickness);
                    }
                    DrawLineAa(color, width, height);
                    points1 = points2;
                    points2 = null;
                    if (isSeriesSelected)
                        color = seriesSelectionColor;
                    else if (isMultiColor)
                        color = GetColor(fastSeries.GetInteriorColor(i - 1));
                }

                points1 = null;
                points2 = null;
            }
        }

        private void DrawDashedAaLines(int width, int height, Windows.UI.Color color, double leftThickness,
                                      double rightThickness)
        {
            if (!Series.IsActualTransposed)
                DrawDashedAaLines(xValues, yValues, width, height, color, leftThickness, rightThickness);
            else
                DrawDashedAaLines(yValues, xValues, width, height, color, leftThickness, rightThickness);
        }

        private void DrawDashedAaLines(List<double> xVals, List<double> yVals, int w, int h, Windows.UI.Color color,
                                     double leftThickness,
                                     double rightThickness)
        {
            var fastSeries = Series;
            var bitmapSeries = (FastLineBitmapSeries)fastSeries;
            double multiplier = bitmapSeries.StrokeWidth;
            double xEnd;
            double yEnd;
            xStart = xVals[0];
            yStart = yVals[0];
            DoubleCollection dashes = bitmapSeries.StrokeDashArray;
            bool isDash = true;
            double currentLen = dashes[0] * multiplier;
            double x1, y1, x2, y2;
            int dashIndex = 1;
            bool moveToNext = false;
            bool hasRemaining = true;
            bool isBeginning = true;
            bool isPreviousDash = false;
            if (points1 == null)
                points1 = new int[10];
            for (int i = 1; i < xVals.Count; i++)
            {
                xEnd = xVals[i];
                yEnd = yVals[i];

                if (xStart == 0 && xEnd == 0)
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue;
                }
                if ((xStart == xEnd) && (yStart == yEnd))
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue; // edge case causing invDFloat to overflow, found by Shai Rubinshtein
                }

                if (yStart < 0 && yEnd < 0)
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue;
                }

                if (yStart > h && yEnd > h)
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue;
                }

                if (xStart < 0 && xEnd < 0)
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue;
                }
                if (xStart > w && xEnd > w)
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue;
                }


                double sy1 = yStart;
                double sy2 = yEnd;
                double sx1 = xStart;
                double sx2 = xEnd;

                double slope;

                double intersect;
                if (yEnd < -0.5)
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx2, sy2, slope);
                        xEnd = (int)((-intersect) / slope);
                    }
                    yEnd = 0;
                }
                if (yStart < -0.5)
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx1, sy1, slope);
                        xStart = (int)((-intersect) / slope);
                    }
                    yStart = 0;
                }
                if (xStart < -0.5)
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx1, sy1, slope);
                        yStart = (int)(intersect);
                    }
                    xStart = 0;
                }

                if (xEnd < -0.5)
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx2, sy2, slope);
                        yEnd = (int)(intersect);
                    }
                    xEnd = 0;
                }
                if (xStart > (w + 0.5))
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx1, sy1, slope);
                        yStart = (int)(slope * (w) + intersect);
                    }
                    xStart = w;
                }
                if (xEnd > (w + 0.5))
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx2, sy2, slope);
                        yEnd = (int)(slope * (w) + intersect);
                    }
                    xEnd = w;
                }


                if (yStart > (h + 0.5))
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx1, sy1, slope);
                        xStart = (int)(((h) - intersect) / slope);
                    }
                    yStart = h;
                }
                if (yEnd > (h + 0.5))
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx2, sy2, slope);
                        xEnd = (int)(((h) - intersect) / slope);
                    }
                    yEnd = h;
                }
                x1 = xStart;
                y1 = yStart;
                x2 = xEnd;
                y2 = yEnd;
                double totalLen = CalcLenOfLine(x1, x2, y1, y2);
                if (!double.IsNaN(totalLen))
                {
                    if (isSeriesSelected)
                        color = seriesSelectionColor;
                    else if (fastSeries.PaletteBrushes != null)
                        color = GetColor(fastSeries.GetInteriorColor(i - 1));
                    moveToNext = false;
                    while (!moveToNext)
                    {
                        points2 = new int[10];
                        if (totalLen < currentLen)
                        {
                            currentLen = currentLen - totalLen;
                            hasRemaining = true;
                            moveToNext = true;
                        }
                        else
                        {
                            if (totalLen != currentLen)
                            {
                                //Calculating the point at certain distance in a line segment.
                                double lenRatio = currentLen / totalLen;
                                x2 = x1 + (lenRatio * (x2 - x1));
                                y2 = y1 + (lenRatio * (y2 - y1));
                            }
                            totalLen = totalLen - currentLen;
                            currentLen = dashes[dashIndex] * multiplier;
                            dashIndex = dashIndex + 1 == dashes.Count() ? 0 : dashIndex + 1;
                            moveToNext = totalLen == 0;
                            hasRemaining = false;
                        }

                        if (isBeginning)
                        {
                            GetLinePoints(x1, y1, x2, y2, leftThickness, rightThickness, points1);
                        }
                        else
                        {
                            UpdatePoints2(x1, y1, x2, y2, leftThickness, rightThickness);
                        }


                        if (isPreviousDash)
                        {
                            DrawLineAa(color, w, h);
                        }

                        if (!isBeginning)
                        {
                            points1 = points2;
                            points2 = null;
                        }
                        isPreviousDash = isDash;
                        isBeginning = false;
                        isDash = hasRemaining ? isDash : !isDash;

                        x1 = x2;
                        y1 = y2;
                        x2 = xEnd;
                        y2 = yEnd;
                    }
                }
                else
                {
                    points1 = new int[10];
                }
                xStart = xVals[i];
                yStart = yVals[i];
            }

            if (isPreviousDash)
            {
                DrawLineAa(color, w, h);
            }

            points1 = null;
            points2 = null;
        }

        private void DrawDashedLines(int width, int height, Windows.UI.Color color, double leftThickness,
                                       double rightThickness)
        {
            if (!Series.IsActualTransposed)
                DrawDashedLines(xValues, yValues, width, height, color, leftThickness, rightThickness);
            else
                DrawDashedLines(yValues, xValues, width, height, color, leftThickness, rightThickness);
        }

        private void DrawDashedLines(List<double> xVals, List<double> yVals, int w, int h, Windows.UI.Color color,
                                    double leftThickness,
                                    double rightThickness)
        {
            var fastSeries = Series;
            var bitmapSeries = (FastLineBitmapSeries)fastSeries;
            double multiplier = bitmapSeries.StrokeWidth;
            double xEnd;
            double yEnd;
            xStart = xVals[0];
            yStart = yVals[0];
            DoubleCollection dashes = bitmapSeries.StrokeDashArray;
            bool isDash = true;
            double currentLen = dashes[0] * multiplier;
            double x1, y1, x2, y2;
            int dashIndex = 1;
            bool moveToNext = false;
            bool hasRemaining = true;
            bool isBeginning = true;
            bool isPreviousDash = false;
            if (points1 == null)
                points1 = new int[10];
            for (int i = 1; i < xVals.Count; i++)
            {
                xEnd = xVals[i];
                yEnd = yVals[i];

                if (xStart == 0 && xEnd == 0)
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue;
                }
                if ((xStart == xEnd) && (yStart == yEnd))
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue; // edge case causing invDFloat to overflow, found by Shai Rubinshtein
                }

                if (yStart < 0 && yEnd < 0)
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue;
                }

                if (yStart > h && yEnd > h)
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue;
                }

                if (xStart < 0 && xEnd < 0)
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue;
                }
                if (xStart > w && xEnd > w)
                {
                    yStart = yVals[i];
                    xStart = xVals[i];
                    continue;
                }

                double sy1 = yStart;
                double sy2 = yEnd;
                double sx1 = xStart;
                double sx2 = xEnd;

                double slope;

                double intersect;
                if (yEnd < -0.5)
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx2, sy2, slope);
                        xEnd = (int)((-intersect) / slope);
                    }
                    yEnd = 0;
                }
                if (yStart < -0.5)
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx1, sy1, slope);
                        xStart = (int)((-intersect) / slope);
                    }
                    yStart = 0;
                }
                if (xStart < -0.5)
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx1, sy1, slope);
                        yStart = (int)(intersect);
                    }
                    xStart = 0;
                }

                if (xEnd < -0.5)
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx2, sy2, slope);
                        yEnd = (int)(intersect);
                    }
                    xEnd = 0;
                }
                if (xStart > (w + 0.5))
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx1, sy1, slope);
                        yStart = (int)(slope * (w) + intersect);
                    }
                    xStart = w;
                }
                if (xEnd > (w + 0.5))
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx2, sy2, slope);
                        yEnd = (int)(slope * (w) + intersect);
                    }
                    xEnd = w;
                }


                if (yStart > (h + 0.5))
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx1, sy1, slope);
                        xStart = (int)(((h) - intersect) / slope);
                    }
                    yStart = h;
                }
                if (yEnd > (h + 0.5))
                {
                    slope = WriteableBitmapExtensions.Slope(sx1, sy1, sx2, sy2);
                    if (!double.IsInfinity(slope) && !double.IsNaN(slope))
                    {
                        intersect = WriteableBitmapExtensions.Intersect(sx2, sy2, slope);
                        xEnd = (int)(((h) - intersect) / slope);
                    }
                    yEnd = h;
                }

                x1 = xStart;
                y1 = yStart;
                x2 = xEnd;
                y2 = yEnd;

                double totalLen = CalcLenOfLine(x1, x2, y1, y2);
                if (!double.IsNaN(totalLen))
                {
                    if (isSeriesSelected)
                        color = seriesSelectionColor;
                    else if (fastSeries.PaletteBrushes != null)
                        color = GetColor(fastSeries.GetInteriorColor(i - 1));
                    moveToNext = false;
                    while (!moveToNext)
                    {
                        points2 = new int[10];
                        if (totalLen < currentLen)
                        {
                            currentLen = currentLen - totalLen;
                            hasRemaining = true;
                            moveToNext = true;
                        }
                        else
                        {
                            if (totalLen != currentLen)
                            {
                                //Calculating the point at certain distance in a line segment.
                                double lenRatio = currentLen / totalLen;
                                x2 = x1 + (lenRatio * (x2 - x1));
                                y2 = y1 + (lenRatio * (y2 - y1));
                            }
                            totalLen = totalLen - currentLen;
                            currentLen = dashes[dashIndex] * multiplier;
                            dashIndex = dashIndex + 1 == dashes.Count() ? 0 : dashIndex + 1;
                            moveToNext = totalLen == 0;
                            hasRemaining = false;
                        }

                        if (isBeginning)
                        {
                            GetLinePoints(x1, y1, x2, y2, leftThickness, rightThickness, points1);
                        }
                        else
                        {
                            UpdatePoints2(x1, y1, x2, y2, leftThickness, rightThickness);
                        }


                        if (isPreviousDash)
                        {
                            DrawLine(color, w, h);
                        }

                        if (!isBeginning)
                        {
                            points1 = points2;
                            points2 = null;
                        }
                        isPreviousDash = isDash;
                        isBeginning = false;
                        isDash = hasRemaining ? isDash : !isDash;

                        x1 = x2;
                        y1 = y2;
                        x2 = xEnd;
                        y2 = yEnd;
                    }
                }
                else
                {
                    points1 = new int[10];
                }
                xStart = xVals[i];
                yStart = yVals[i];
            }

            if (isPreviousDash)
            {
                DrawLine(color, w, h);
            }

            points1 = null;
            points2 = null;
        }

        private void DrawLine(Windows.UI.Color color, int width, int height)
        {
            var fastSeries = Series;

            if (fastSeries.Chart.IsMultipleArea && fastSeries.Clip != null)
            {
                    var clip = fastSeries.Clip.Bounds;
                    bitmap.DrawLineBresenham(fastBuffer, width, height, points1[0], points1[1], points1[2], points1[3],
                   color, fastSeries.bitmapPixels, clip);
                    bitmap.FillPolygon(fastBuffer, points1, width, height, color, fastSeries.bitmapPixels, clip);
                    bitmap.DrawLineBresenham(fastBuffer, width, height, points1[4], points1[5], points1[6], points1[7],
                        color, fastSeries.bitmapPixels, clip);
            }
            else
            {
                bitmap.DrawLineBresenham(fastBuffer, width, height, points1[0], points1[1], points1[2], points1[3],
               color, fastSeries.bitmapPixels);
                bitmap.FillPolygon(fastBuffer, points1, width, height, color, fastSeries.bitmapPixels);
                bitmap.DrawLineBresenham(fastBuffer, width, height, points1[4], points1[5], points1[6], points1[7],
                    color, fastSeries.bitmapPixels);
            }
        }

        private void DrawLineAa(Windows.UI.Color color, int width, int height)
        {
            var fastSeries = Series;

            if (fastSeries.Chart.IsMultipleArea && fastSeries.Clip != null)
            {
                    var clip = fastSeries.Clip.Bounds;
                    bitmap.DrawLineAa(fastBuffer, width, height, points1[0], points1[1], points1[2], points1[3], color, fastSeries.bitmapPixels, clip);
                    bitmap.FillPolygon(fastBuffer, points1, width, height, color, fastSeries.bitmapPixels, clip);
                    bitmap.DrawLineAa(fastBuffer, width, height, points1[4], points1[5], points1[6], points1[7], color, fastSeries.bitmapPixels, clip);
            }
            else
            {
                bitmap.DrawLineAa(fastBuffer, width, height, points1[0], points1[1], points1[2], points1[3], color, fastSeries.bitmapPixels);
                bitmap.FillPolygon(fastBuffer, points1, width, height, color, fastSeries.bitmapPixels);
                bitmap.DrawLineAa(fastBuffer, width, height, points1[4], points1[5], points1[6], points1[7], color, fastSeries.bitmapPixels);
            }
        }

#endregion

#endregion
    }
}
