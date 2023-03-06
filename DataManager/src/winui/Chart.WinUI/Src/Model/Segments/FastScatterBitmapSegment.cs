using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using NativeColor = Windows.UI.Color;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class FastScatterBitmapSegment : ChartSegment
    {
        #region Fields

        #region Private Fields

        private List<double> xValues;

        private List<double> yValues;

        private IList<double> xChartVals, yChartVals;

        int startIndex = 0;

        private WriteableBitmap bitmap;

        NativeColor seriesSelectionColor = Colors.Transparent;

        NativeColor segmentSelectionColor = Colors.Transparent;

        bool isSeriesSelected;

        private byte[] fastBuffer;

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public FastScatterBitmapSegment()
        {
            xValues = new List<double>();
            yValues = new List<double>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xVals"></param>
        /// <param name="yVals"></param>
        /// <param name="series"></param>
        public FastScatterBitmapSegment(IList<double> xVals, IList<double> yVals, FastScatterBitmapSeries series)
        {
            xValues = new List<double>();
            yValues = new List<double>();
            base.Series = series;
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
            return null;
        }

        /// <inheritdoc/>
        internal override void OnSizeChanged(Size size)
        {
        }

        /// <inheritdoc/>
        internal override UIElement GetRenderedVisual()
        {
            return null;
        }

        /// <inheritdoc/>
        internal override void Update(IChartTransformer transformer)
        {
            bitmap = (Series as FastScatterBitmapSeries).Chart.GetFastRenderSurface();
            if (transformer != null && Series.PointsCount > 0)
            {
                ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
                xValues.Clear();
                yValues.Clear();
                //Removed the existing screen point calculation methods and added the TransformVisible method.
                CalculatePoints(cartesianTransformer);
                UpdateVisual();
            }
        }

        #endregion

        #region Internal Methods

        internal void SetRange()
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            if (fastSeries.PointsCount > 0)
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
                if (fastSeries.IsIndexed)
                {
                    var isGrouping = (fastSeries.ActualXAxis is CategoryAxis) ? (fastSeries.ActualXAxis as CategoryAxis).ArrangeByIndex : true;
                    double X_MAX = 0;
                    if (!isGrouping)
                        X_MAX = xChartVals.Max();
                    else
                        X_MAX = fastSeries.PointsCount - 1;
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

        internal void UpdateVisual()
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            double xr = fastSeries.PointWidth, yr = fastSeries.PointHeight;
            bool isMultiColor = Series.PaletteBrushes != null && fastSeries.Fill == null;
            NativeColor color = GetColor(Fill);
            if (bitmap != null && xValues.Count > 0)
            {
                fastBuffer = fastSeries.Chart.GetFastBuffer();
                int width = (int)fastSeries.Chart.SeriesClipRect.Width;
                int height = (int)fastSeries.Chart.SeriesClipRect.Height;

                if (fastSeries is FastScatterBitmapSeries)
                {
                    isSeriesSelected = false;
                    ChartBase chart = fastSeries.Chart;

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

                    DrawScatterType(fastSeries.Type, width, height, color, isMultiColor, xr, yr);
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

        }

        internal override void Dispose()
        {
            if (xChartVals !=null)
            {
                xChartVals.Clear();
                xChartVals = null;
            }

            if (yChartVals !=null)
            {
                yChartVals.Clear();
                yChartVals = null;
            }

            if(this.bitmap !=null)
                bitmap = null;

            if (this.fastBuffer != null)
                fastBuffer = null;

            base.Dispose();
        }

        #endregion

        #region Private Methods

        private NativeColor GetSegmentInterior(NativeColor color, bool isMultiColor)
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            if (isSeriesSelected)
            {
                color = seriesSelectionColor;
            }
            else if (isMultiColor)
            {
                Brush brush = Series.PaletteBrushes[startIndex % Series.PaletteBrushes.Count()];
                color = GetColor(brush);
            }

            return color;
        }

        private void DrawScatterType(ShapeType type, int width, int height, NativeColor color, bool isMultiColor, double xr, double yr)
        {
            double xValue = 0, yValue = 0;

            for (int i = 0; i < xValues.Count; i++)
            {
                xValue = xValues[i];
                yValue = yValues[i];
                NativeColor segmentColor = GetSegmentInterior(color, isMultiColor);
                startIndex++;

                if (yValue > -1)
                {
                    switch (type)
                    {
                        case ShapeType.Circle:
                            DrawEllipse(width, height, segmentColor, xValue, yValue, xr, yr);
                            break;
                        case ShapeType.Rectangle:
                            DrawRectangle(width, height, segmentColor, xValue, yValue, xr, yr);
                            break;
                        case ShapeType.Cross:
                            DrawCross(width, height, segmentColor, xValue, yValue, xr, yr);
                            break;
                        case ShapeType.Diamond:
                            DrawDiamond(width, height, segmentColor, xValue, yValue, xr, yr);
                            break;
                        case ShapeType.Hexagon:
                            DrawHexagon(width, height, segmentColor, xValue, yValue, xr, yr);
                            break;
                        case ShapeType.InvertedTriangle:
                            DrawInvertedTriangle(width, height, segmentColor, xValue, yValue, xr, yr);
                            break;
                        case ShapeType.Pentagon:
                            DrawPentagon(width, height, segmentColor, xValue, yValue, xr, yr);
                            break;
                        case ShapeType.Plus:
                            DrawPlus(width, height, segmentColor, xValue, yValue, xr, yr);
                            break;
                        case ShapeType.Triangle:
                            DrawTriangle(width, height, segmentColor, xValue, yValue, xr, yr);
                            break;
                    }
                }
            }
        }

        private void DrawEllipse(int width, int height, NativeColor color, double xValue, double yValue, double scatterWidth, double scatterHeight)
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            if (fastSeries.IsActualTransposed)
            {
                bitmap.FillEllipseCentered(fastBuffer, height, width, (int)yValue, (int)xValue, (int)scatterWidth, (int)scatterHeight, color, fastSeries.bitmapPixels);
            }
            else
            {
                bitmap.FillEllipseCentered(fastBuffer, height, width, (int)xValue, (int)yValue, (int)scatterWidth, (int)scatterHeight, color, fastSeries.bitmapPixels);
            }
        }

        private void DrawRectangle(int width, int height, NativeColor color, double xValue, double yValue, double scatterWidth, double scatterHeight)
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            double x1 = xValue - scatterWidth / 2;
            double y1 = yValue - scatterHeight / 2;
            double x2 = xValue + scatterWidth / 2;
            double y2 = yValue + scatterHeight / 2;

            if (y1 < y2)
            {
                if (fastSeries.IsActualTransposed)
                {
                    bitmap.FillRectangle(fastBuffer, width, height, (int)y1, (int)x1, (int)y2, (int)x2, color, fastSeries.bitmapPixels);
                    Series.bitmapRects.Add(new Rect(new Point(y1, x1), new Point(y2, x2)));
                }
                else
                {
                    bitmap.FillRectangle(fastBuffer, width, height, (int)x1, (int)y1, (int)x2, (int)y2, color, fastSeries.bitmapPixels);
                    Series.bitmapRects.Add(new Rect(new Point(x1, y1), new Point(x2, y2)));
                }
            }
            else
            {
                if (fastSeries.IsActualTransposed)
                {
                    bitmap.FillRectangle(fastBuffer, width, height, (int)y2, (int)x1, (int)y1, (int)x2, color, fastSeries.bitmapPixels);
                    Series.bitmapRects.Add(new Rect(new Point(y2, x1), new Point(y1, x2)));
                }
                else
                {
                    bitmap.FillRectangle(fastBuffer, width, height, (int)x1, (int)y2, (int)x2, (int)y1, color, fastSeries.bitmapPixels);
                    Series.bitmapRects.Add(new Rect(new Point(x1, y2), new Point(x2, y1)));
                }
            }
        }

        private void DrawTriangle(int width, int height, NativeColor color, double xValue, double yValue, double scatterWidth, double scatterHeight)
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            double x1 = xValue - scatterWidth / 2;
            double y1 = !fastSeries.IsActualTransposed ? yValue + scatterHeight / 2 : yValue - scatterHeight / 2;
            double x2 = xValue + scatterWidth / 2;
            double y2 = !fastSeries.IsActualTransposed ? yValue + scatterHeight / 2 : yValue - scatterHeight / 2;
            double x3 = xValue;
            double y3 = !fastSeries.IsActualTransposed ? yValue - scatterHeight / 2 : yValue + scatterHeight / 2;

            if (fastSeries.IsActualTransposed)
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)y1, (int)x1, (int)y2, (int)x2, (int)y3, (int)x3, (int)y1, (int)x1 }, width, height, color, fastSeries.bitmapPixels);
            }
            else
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)x1, (int)y1, (int)x2, (int)y2, (int)x3, (int)y3, (int)x1, (int)y1 }, width, height, color, fastSeries.bitmapPixels);
            }
        }

        private void DrawInvertedTriangle(int width, int height, NativeColor color, double xValue, double yValue, double scatterWidth, double scatterHeight)
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            double x1 = xValue - scatterWidth / 2;
            double y1 = !fastSeries.IsActualTransposed ? yValue - scatterHeight / 2 : yValue + scatterHeight / 2;
            double x2 = xValue + scatterWidth / 2;
            double y2 = !fastSeries.IsActualTransposed ? yValue - scatterHeight / 2 : yValue + scatterHeight / 2;
            double x3 = xValue;
            double y3 = !fastSeries.IsActualTransposed ? yValue + scatterHeight / 2 : yValue - scatterHeight / 2;

            if (fastSeries.IsActualTransposed)
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)y1, (int)x1, (int)y2, (int)x2, (int)y3, (int)x3, (int)y1, (int)x1 }, width, height, color, fastSeries.bitmapPixels);
            }
            else
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)x1, (int)y1, (int)x2, (int)y2, (int)x3, (int)y3, (int)x1, (int)y1 }, width, height, color, fastSeries.bitmapPixels);
            }
        }

        private void DrawDiamond(int width, int height, NativeColor color, double xValue, double yValue, double scatterWidth, double scatterHeight)
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            double x1 = xValue - scatterWidth / 2;
            double y1 = yValue;
            double x2 = xValue;
            double y2 = yValue - scatterHeight / 2;
            double x3 = xValue + scatterWidth / 2;
            double y3 = yValue;
            double x4 = xValue;
            double y4 = yValue + scatterHeight / 2;

            if (fastSeries.IsActualTransposed)
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)y1, (int)x1, (int)y2, (int)x2, (int)y3, (int)x3, (int)y4, (int)x4, (int)y1, (int)x1 }, width, height, color, fastSeries.bitmapPixels);
            }
            else
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)x1, (int)y1, (int)x2, (int)y2, (int)x3, (int)y3, (int)x4, (int)y4, (int)x1, (int)y1 }, width, height, color, fastSeries.bitmapPixels);
            }

        }

        private void DrawHexagon(int width, int height, NativeColor color, double xValue, double yValue, double scatterWidth, double scatterHeight)
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            double x1 = xValue - scatterWidth / 2;
            double y1 = yValue;
            double x2 = xValue - scatterWidth / 4;
            double y2 = yValue - scatterHeight / 2;
            double x3 = xValue + scatterWidth / 4;
            double y3 = yValue - scatterHeight / 2;
            double x4 = xValue + scatterWidth / 2;
            double y4 = yValue;
            double x5 = xValue + scatterWidth / 4;
            double y5 = yValue + scatterHeight / 2;
            double x6 = xValue - scatterWidth / 4;
            double y6 = yValue + scatterHeight / 2;

            if (fastSeries.IsActualTransposed)
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)y1, (int)x1, (int)y2, (int)x2, (int)y3, (int)x3, (int)y4, (int)x4, (int)y5, (int)x5, (int)y6, (int)x6, (int)y1, (int)x1 }, width, height, color, fastSeries.bitmapPixels);
            }
            else
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)x1, (int)y1, (int)x2, (int)y2, (int)x3, (int)y3, (int)x4, (int)y4, (int)x5, (int)y5, (int)x6, (int)y6, (int)x1, (int)y1 }, width, height, color, fastSeries.bitmapPixels);
            }
        }

        private void DrawPentagon(int width, int height, NativeColor color, double xValue, double yValue, double scatterWidth, double scatterHeight)
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            double x1 = xValue - scatterWidth / 4;
            double y1 = !fastSeries.IsActualTransposed ? yValue + scatterHeight / 2 : yValue - scatterHeight / 2;
            double x2 = xValue - scatterWidth / 2;
            double y2 = !fastSeries.IsActualTransposed ? yValue - scatterHeight / 8 : yValue + scatterHeight / 8;
            double x3 = xValue;
            double y3 = !fastSeries.IsActualTransposed ? yValue - scatterHeight / 2 : yValue + scatterHeight / 2;
            double x4 = xValue + scatterWidth / 2;
            double y4 = !fastSeries.IsActualTransposed ? yValue - scatterHeight / 8 : yValue + scatterHeight / 8;
            double x5 = xValue + scatterWidth / 4;
            double y5 = !fastSeries.IsActualTransposed ? yValue + scatterHeight / 2 : yValue - scatterHeight / 2;

            if (fastSeries.IsActualTransposed)
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)y1, (int)x1, (int)y2, (int)x2, (int)y3, (int)x3, (int)y4, (int)x4, (int)y5, (int)x5, (int)y1, (int)x1 }, width, height, color, fastSeries.bitmapPixels);
            }
            else
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)x1, (int)y1, (int)x2, (int)y2, (int)x3, (int)y3, (int)x4, (int)y4, (int)x5, (int)y5, (int)x1, (int)y1 }, width, height, color, fastSeries.bitmapPixels);
            }
        }

        private void DrawPlus(int width, int height, NativeColor color, double xValue, double yValue, double scatterWidth, double scatterHeight)
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            double x1 = xValue - scatterWidth / 8;
            double y1 = yValue + scatterHeight / 8;
            double x2 = xValue - scatterWidth / 2;
            double y2 = yValue + scatterHeight / 8;
            double x3 = xValue - scatterWidth / 2;
            double y3 = yValue - scatterHeight / 8;
            double x4 = xValue - scatterWidth / 8;
            double y4 = yValue - scatterHeight / 8;
            double x5 = xValue - scatterWidth / 8;
            double y5 = yValue - scatterHeight / 2;
            double x6 = xValue + scatterWidth / 8;
            double y6 = yValue - scatterHeight / 2;
            double x7 = xValue + scatterWidth / 8;
            double y7 = yValue - scatterHeight / 8;
            double x8 = xValue + scatterWidth / 2;
            double y8 = yValue - scatterHeight / 8;
            double x9 = xValue + scatterWidth / 2;
            double y9 = yValue + scatterHeight / 8;
            double x10 = xValue + scatterWidth / 8;
            double y10 = yValue + scatterHeight / 8;
            double x11 = xValue + scatterWidth / 8;
            double y11 = yValue + scatterHeight / 2;
            double x12 = xValue - scatterWidth / 8;
            double y12 = yValue + scatterHeight / 2;

            if (fastSeries.IsActualTransposed)
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)y1, (int)x1, (int)y2, (int)x2, (int)y3, (int)x3, (int)y4, (int)x4, (int)y5, (int)x5, (int)y6, (int)x6, (int)y7, (int)x7, (int)y8, (int)x8, (int)y9, (int)x9, (int)y10, (int)x10, (int)y11, (int)x11, (int)y12, (int)x12, (int)y1, (int)x1 }, width, height, color, fastSeries.bitmapPixels);
            }
            else
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)x1, (int)y1, (int)x2, (int)y2, (int)x3, (int)y3, (int)x4, (int)y4, (int)x5, (int)y5, (int)x6, (int)y6, (int)x7, (int)y7, (int)x8, (int)y8, (int)x9, (int)y9, (int)x10, (int)y10, (int)x11, (int)y11, (int)x12, (int)y12, (int)x1, (int)y1 }, width, height, color, fastSeries.bitmapPixels);
            }

        }

        private void DrawCross(int width, int height, NativeColor color, double xValue, double yValue, double scatterWidth, double scatterHeight)
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            double x1 = xValue;
            double y1 = yValue + scatterHeight / 4;
            double x2 = xValue - scatterWidth / 4;
            double y2 = yValue + scatterHeight / 2;
            double x3 = xValue - scatterWidth / 2;
            double y3 = yValue + scatterHeight / 4;
            double x4 = xValue - scatterWidth / 4;
            double y4 = yValue;
            double x5 = xValue - scatterWidth / 2;
            double y5 = yValue - scatterHeight / 4;
            double x6 = xValue - scatterWidth / 4;
            double y6 = yValue - scatterHeight / 2;
            double x7 = xValue;
            double y7 = yValue - scatterHeight / 4;
            double x8 = xValue + scatterWidth / 4;
            double y8 = yValue - scatterHeight / 2;
            double x9 = xValue + scatterWidth / 2;
            double y9 = yValue - scatterHeight / 4;
            double x10 = xValue + scatterWidth / 4;
            double y10 = yValue;
            double x11 = xValue + scatterWidth / 2;
            double y11 = yValue + scatterHeight / 4;
            double x12 = xValue + scatterWidth / 4;
            double y12 = yValue + scatterHeight / 2;

            if (fastSeries.IsActualTransposed)
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)y1, (int)x1, (int)y2, (int)x2, (int)y3, (int)x3, (int)y4, (int)x4, (int)y5, (int)x5, (int)y6, (int)x6, (int)y7, (int)x7, (int)y8, (int)x8, (int)y9, (int)x9, (int)y10, (int)x10, (int)y11, (int)x11, (int)y12, (int)x12, (int)y1, (int)x1 }, width, height, color, fastSeries.bitmapPixels);
            }
            else
            {
                bitmap.FillPolygon(fastBuffer, new int[] { (int)x1, (int)y1, (int)x2, (int)y2, (int)x3, (int)y3, (int)x4, (int)y4, (int)x5, (int)y5, (int)x6, (int)y6, (int)x7, (int)y7, (int)x8, (int)y8, (int)x9, (int)y9, (int)x10, (int)y10, (int)x11, (int)y11, (int)x12, (int)y12, (int)x1, (int)y1 }, width, height, color, fastSeries.bitmapPixels);
            }

        }

        private void CalculatePoints(ChartTransform.ChartCartesianTransformer cartesianTransformer)
        {
            var fastSeries = Series as FastScatterBitmapSeries;
            ChartAxis xAxis = cartesianTransformer.XAxis;
            if (fastSeries.IsIndexed)
            {
                var isGrouping = (fastSeries.ActualXAxis is CategoryAxis) &&
                                 (fastSeries.ActualXAxis as CategoryAxis).IsIndexed;
                int start = 0, end = 0;
                if (!isGrouping)
                {
                    start = 0;
                    end = xChartVals.Count - 1;
                }
                else
                {
                    start = (int)Math.Floor(xAxis.VisibleRange.Start);
                    end = (int)Math.Ceiling(xAxis.VisibleRange.End);
                    end = end > yChartVals.Count - 1 ? yChartVals.Count - 1 : end;
                }
                start = start < 0 ? 0 : start;
                startIndex = start;
                for (int i = start; i <= end; i++)
                {
                    AddDataPoint(cartesianTransformer, i);
                }
            }
            else
            {
                if (fastSeries.IsLinearData)
                {
                    int i = 0;
                    double start = xAxis.VisibleRange.Start;
                    double end = xAxis.VisibleRange.End;
                    startIndex = 0;
                    int count = xChartVals.Count - 1;
                    for (i = 1; i < count; i++)
                    {
                        double xValue = xChartVals[i];
                        if (xValue >= start && xValue <= end)
                            AddDataPoint(cartesianTransformer, i);
                        else if (xValue < start)
                            startIndex = i;
                        else if (xValue > end)
                        {
                            AddDataPoint(cartesianTransformer, i);
                            break;
                        }
                    }
                    InsertDataPoint(cartesianTransformer, startIndex);
                    if (i == count)
                        AddDataPoint(cartesianTransformer, count);
                }
                else
                {
                    startIndex = 0;
                    for (int i = 0; i < this.Series.PointsCount; i++)
                    {
                        AddDataPoint(cartesianTransformer, i);
                    }
                }
            }
        }
        private void AddDataPoint(ChartTransform.ChartCartesianTransformer cartesianTransformer,
            int index)
        {
            double xValue, yValue;
            GetXYValue(index, out xValue, out yValue);
            Point point = cartesianTransformer.TransformToVisible(xValue, yValue);
            if (!(Series as FastScatterBitmapSeries).IsActualTransposed)
            {
                xValues.Add(point.X);
                yValues.Add(point.Y);
            }
            else
            {
                xValues.Add(point.Y);
                yValues.Add(point.X);
            }
        }

        private void InsertDataPoint(ChartTransform.ChartCartesianTransformer cartesianTransformer,
           int index)
        {
            double xValue, yValue;
            GetXYValue(index, out xValue, out yValue);
            Point point = cartesianTransformer.TransformToVisible(xValue, yValue);
            if (!(Series as FastScatterBitmapSeries).IsActualTransposed)
            {
                xValues.Insert(0, point.X);
                yValues.Insert(0, point.Y);
            }
            else
            {
                xValues.Insert(0, point.Y);
                yValues.Insert(0, point.X);
            }
        }

        private void GetXYValue(int index, out double xValue, out double yValue)
        {
            xValue = xChartVals[index];
            yValue = yChartVals[index];
        }

        #endregion

        #endregion
    }
}
