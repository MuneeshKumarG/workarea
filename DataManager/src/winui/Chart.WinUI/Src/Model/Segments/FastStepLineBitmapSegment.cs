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
    public class FastStepLineBitmapSegment : ChartSegment
    {
        #region Fields

        #region Private Fields

        private IList<double> xChartVals;

        private IList<double> yChartVals;

        private WriteableBitmap bitmap;

        private byte[] fastBuffer;

        List<double> xValues;

        List<double> yValues;

        int startIndex = 0;

        NativeColor seriesSelectionColor = Colors.Transparent;

        bool isSeriesSelected;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public FastStepLineBitmapSegment()
        {
            xValues = new List<double>();
            yValues = new List<double>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="series"></param>
        public FastStepLineBitmapSegment(ChartSeries series)
        {
            base.Series = series;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xVals"></param>
        /// <param name="yVals"></param>
        /// <param name="series"></param>
        public FastStepLineBitmapSegment(IList<double> xVals, IList<double> yVals, ChartSeries series)
            : this(series)
        {
            xValues = new List<double>();
            yValues = new List<double>();
            Series = series;
            xChartVals = xVals;
            yChartVals = yVals;
            base.Item = series.ActualData;

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
            bitmap = (Series as ChartSeries).Chart.GetFastRenderSurface();
            fastBuffer = (Series as ChartSeries).Chart.GetFastBuffer();

            return null;
        }

        /// <inheritdoc/>
        internal override void OnSizeChanged(Size size) { }

        /// <inheritdoc/>
        internal override UIElement GetRenderedVisual()
        {
            return null;
        }

        /// <inheritdoc/>
        internal override void Update(IChartTransformer transformer)
        {
            bitmap = (Series as ChartSeries).Chart.GetFastRenderSurface();
            if (transformer != null && Series.PointsCount > 0)
            {
                ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
                xValues.Clear();
                yValues.Clear(); //WPF-14440 In seconday axis scrolling not apply for FastStepLineBitmapSeries
                //Removed the existing screen point calculation methods and added the TransformVisible method.
                CalculatePoints(cartesianTransformer);
                UpdateVisual();
            }
        }

        #endregion

        #region Internal Methods

        internal void SetRange()
        {
            var fastSeries = Series;
            var isGrouping = (fastSeries.ActualXAxis is CategoryAxis) ? (fastSeries.ActualXAxis as CategoryAxis).ArrangeByIndex : true;
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
                    double X_MAX = !isGrouping ? xChartVals.Max() : fastSeries.PointsCount - 1;
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
            var fastSeries = Series as XyDataSeries;
            double xStart, yStart;
            bool isMultiColor = Series.PaletteBrushes != null && fastSeries.Fill == null;
            NativeColor color = GetColor(this.Fill);

            if (bitmap != null && xValues.Count > 0 && fastSeries.StrokeWidth > 0)
            {
                fastBuffer = fastSeries.Chart.GetFastBuffer();

                xStart = xValues[0];
                yStart = yValues[0];

                int width = (int)fastSeries.Chart.SeriesClipRect.Width;
                int height = (int)fastSeries.Chart.SeriesClipRect.Height;

                int leftThickness = (int)fastSeries.StrokeWidth / 2;
                int rightThickness = (int)(fastSeries.StrokeWidth % 2 == 0
                    ? (fastSeries.StrokeWidth / 2) - 1 : fastSeries.StrokeWidth / 2);

                if (fastSeries is FastStepLineBitmapSeries)
                {
                    ChartBase chart = fastSeries.Chart;
                    isSeriesSelected = false;

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

                    if (!fastSeries.IsActualTransposed)
                    {
                        UpdateVisualHorizontal(xStart, yStart, width, height, color, isMultiColor, leftThickness, rightThickness);
                    }
                    else
                    {
                        UpdateVisualVertical(xStart, yStart, width, height, color, isMultiColor, leftThickness, rightThickness);
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
        internal override void SetVisualBindings(Shape element) { }

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

        private void AddDataPoint(ChartTransform.ChartCartesianTransformer cartesianTransformer, int index)
        {
            double xValue, yValue;
            GetXYValue(index, out xValue, out yValue);
            Point point = cartesianTransformer.TransformToVisible(xValue, yValue);
            if (!Series.IsActualTransposed)
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

        private void InsertDataPoint(ChartTransform.ChartCartesianTransformer cartesianTransformer, int index)
        {
            double xValue, yValue;
            GetXYValue(index, out xValue, out yValue);
            Point point = cartesianTransformer.TransformToVisible(xValue, yValue);
            if (!Series.IsActualTransposed)
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
        private void CalculatePoints(ChartTransform.ChartCartesianTransformer cartesianTransformer)
        {
            var fastSeries = Series;
            ChartAxis xAxis = cartesianTransformer.XAxis;
            int cnt = xChartVals.Count - 1;
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
                    double start = xAxis.VisibleRange.Start;
                    double end = xAxis.VisibleRange.End;
                    startIndex = 0;
                    int i = 0;
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
                    for (int i = 0; i <= cnt; i++)
                    {
                        AddDataPoint(cartesianTransformer, i);
                    }
                }
            }

        }

        private void UpdateVisualVertical(double xStart, double yStart, int width, int height, NativeColor color, bool isMultiColor, int leftThickness, int rightThickness)
        {
            var fastSeries = Series;
            double xEnd, yEnd;

            if (((FastStepLineBitmapSeries)fastSeries).EnableAntiAliasing)
            {
                for (int i = 1; i < xValues.Count; i++)
                {
                    if (isSeriesSelected)
                        color = seriesSelectionColor;
                    else if (isMultiColor)
                    {
                        Brush brush = Series.PaletteBrushes[startIndex % Series.PaletteBrushes.Count()];
                        color = GetColor(brush);
                    }
                    xEnd = xValues[i];
                    yEnd = yValues[i];
                    startIndex++;
                    if (!double.IsNaN(yStart) && !double.IsNaN(yEnd))
                    {
                        var leftOffset = xStart - leftThickness;
                        var rightOffset = xStart + rightThickness;
                        if (yEnd < yStart)
                            bitmap.FillRectangle(fastBuffer, width, height, (int)yEnd, (int)leftOffset, (int)yStart, (int)rightOffset, color, fastSeries.bitmapPixels);
                        else
                            bitmap.FillRectangle(fastBuffer, width, height, (int)yStart, (int)leftOffset, (int)yEnd, (int)rightOffset, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)yStart, (int)leftOffset, (int)yEnd, (int)leftOffset, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)yStart, (int)leftOffset, (int)yStart, (int)rightOffset, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)yStart, (int)rightOffset, (int)yEnd, (int)rightOffset, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)yEnd, (int)leftOffset, (int)yEnd, (int)rightOffset, color, fastSeries.bitmapPixels);
                        leftOffset = yEnd - leftThickness;
                        rightOffset = yEnd + rightThickness;
                        bitmap.FillRectangle(fastBuffer, width, height, (int)leftOffset, (int)xEnd, (int)rightOffset, (int)xStart, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)leftOffset, (int)xEnd, (int)rightOffset, (int)xEnd, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)leftOffset, (int)xEnd, (int)leftOffset, (int)xStart, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)leftOffset, (int)xStart, (int)rightOffset, (int)xStart, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)rightOffset, (int)xEnd, (int)rightOffset, (int)xStart, color, fastSeries.bitmapPixels);
                    }
                    xStart = xEnd;
                    yStart = yEnd;
                }
            }
            else
            {
                for (int i = 1; i < xValues.Count; i++)
                {
                    if (isSeriesSelected)
                        color = seriesSelectionColor;
                    else if (isMultiColor)
                    {
                        Brush brush = Series.PaletteBrushes[startIndex % Series.PaletteBrushes.Count()];
                        color = GetColor(brush);
                    }
                    xEnd = xValues[i];
                    yEnd = yValues[i];
                    startIndex++;
                    if (!double.IsNaN(yStart) && !double.IsNaN(yEnd))
                    {
                        var leftOffset = xStart - leftThickness;
                        var rightOffset = xStart + rightThickness;
                        if (yEnd < yStart)
                            bitmap.FillRectangle(fastBuffer, width, height, (int)yEnd, (int)leftOffset, (int)yStart, (int)rightOffset, color, fastSeries.bitmapPixels);
                        else
                            bitmap.FillRectangle(fastBuffer, width, height, (int)yStart, (int)leftOffset, (int)yEnd, (int)rightOffset, color, fastSeries.bitmapPixels);
                        leftOffset = yEnd - leftThickness;
                        rightOffset = yEnd + rightThickness;
                        if (xEnd < xStart)
                            bitmap.FillRectangle(fastBuffer, width, height, (int)leftOffset, (int)xEnd, (int)rightOffset, (int)xStart, color, fastSeries.bitmapPixels);
                        else
                            bitmap.FillRectangle(fastBuffer, width, height, (int)leftOffset, (int)xStart, (int)rightOffset, (int)xEnd, color, fastSeries.bitmapPixels);
                    }
                    xStart = xEnd;
                    yStart = yEnd;
                }
            }
        }

        private void UpdateVisualHorizontal(double xStart, double yStart, int width, int height, NativeColor color, bool isMultiColor, int leftThickness, int rightThickness)
        {
            var fastSeries = Series;
            double xEnd, yEnd;
            if (((FastStepLineBitmapSeries)fastSeries).EnableAntiAliasing)
            {
                for (int i = 1; i < xValues.Count; i++)
                {
                    if (isSeriesSelected)
                        color = seriesSelectionColor;
                    else if (isMultiColor)
                    {
                        Brush brush = Series.PaletteBrushes[startIndex % Series.PaletteBrushes.Count()];
                        color = GetColor(brush);
                    }
                    startIndex++;
                    xEnd = xValues[i];
                    yEnd = yValues[i];
                    if (!double.IsNaN(yStart) && !double.IsNaN(yEnd))
                    {
                        var leftOffset = yStart - leftThickness;
                        var rightOffset = yStart + rightThickness;
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)xStart, (int)leftOffset, (int)xEnd, (int)leftOffset, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)xStart, (int)leftOffset, (int)xStart, (int)rightOffset, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)xStart, (int)rightOffset, (int)xEnd, (int)rightOffset, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)xEnd, (int)leftOffset, (int)xEnd, (int)rightOffset, color, fastSeries.bitmapPixels);
                        bitmap.FillRectangle(fastBuffer, width, height, (int)xStart, (int)leftOffset, (int)xEnd, (int)rightOffset, color, fastSeries.bitmapPixels);
                        leftOffset = xEnd - leftThickness;
                        rightOffset = xEnd + rightThickness;
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)leftOffset, (int)yStart, (int)rightOffset, (int)yStart, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)leftOffset, (int)yStart, (int)leftOffset, (int)yEnd, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)leftOffset, (int)yEnd, (int)rightOffset, (int)yEnd, color, fastSeries.bitmapPixels);
                        bitmap.DrawLineAa(fastBuffer, width, height, (int)rightOffset, (int)yStart, (int)rightOffset, (int)yEnd, color, fastSeries.bitmapPixels);
                        if (yStart < yEnd)
                            bitmap.FillRectangle(fastBuffer, width, height, (int)leftOffset, (int)yStart, (int)rightOffset, (int)yEnd, color, fastSeries.bitmapPixels);
                        else
                            bitmap.FillRectangle(fastBuffer, width, height, (int)leftOffset, (int)yEnd, (int)rightOffset, (int)yStart, color, fastSeries.bitmapPixels);
                    }
                    xStart = xEnd;
                    yStart = yEnd;
                }
            }
            else
            {
                for (int i = 1; i < xValues.Count; i++)
                {
                    if (isSeriesSelected)
                        color = seriesSelectionColor;
                    else if (isMultiColor)
                    {
                        Brush brush = Series.PaletteBrushes[startIndex % Series.PaletteBrushes.Count()];
                        color = GetColor(brush);
                    }
                    xEnd = xValues[i];
                    yEnd = yValues[i];
                    startIndex++;
                    if (!double.IsNaN(yStart) && !double.IsNaN(yEnd))
                    {
                        var leftOffset = yStart - leftThickness;
                        var rightOffset = yStart + rightThickness;
                        if (xStart < xEnd)
                            bitmap.FillRectangle(fastBuffer, width, height, (int)xStart, (int)leftOffset, (int)xEnd, (int)rightOffset, color, fastSeries.bitmapPixels);
                        else
                            bitmap.FillRectangle(fastBuffer, width, height, (int)xEnd, (int)leftOffset, (int)xStart, (int)rightOffset, color, fastSeries.bitmapPixels);

                        leftOffset = xEnd - leftThickness;
                        rightOffset = xEnd + rightThickness;
                        if (yStart < yEnd)
                            bitmap.FillRectangle(fastBuffer, width, height, (int)leftOffset, (int)yStart, (int)rightOffset, (int)yEnd, color, fastSeries.bitmapPixels);
                        else
                            bitmap.FillRectangle(fastBuffer, width, height, (int)leftOffset, (int)yEnd, (int)rightOffset, (int)yStart, color, fastSeries.bitmapPixels);
                    }

                    xStart = xEnd;
                    yStart = yEnd;
                }
            }
        }

        #endregion

        #endregion    
    }
}
