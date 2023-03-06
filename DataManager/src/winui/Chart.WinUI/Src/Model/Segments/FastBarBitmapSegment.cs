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
    /// Represents a chart segment which renders collection of points using writeablebitmap.
    /// </summary>
    /// <seealso cref="FastBarBitmapSeries"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    internal class FastBarBitmapSegment : ChartSegment
    {
        #region Fields

        #region Private Fields

        private WriteableBitmap bitmap;
#if NETFX_CORE 
        private byte[] fastBuffer;
#endif
        private Size availableSize;

        private IList<double> x1ChartVals, y1ChartVals, x2ChartVals, y2ChartVals;
        
        double xStart, xEnd, yStart, yEnd, width, height, left, top;

        NativeColor seriesSelectionColor = Colors.Transparent;

        NativeColor segmentSelectionColor = Colors.Transparent;

        bool isSeriesSelected;

        List<float> x1Values;

        List<float> x2Values;

        List<float> y1Values;

        List<float> y2Values;

        int startIndex = 0;

        #endregion

        #endregion
        
        #region Constructor

        /// <summary>
        /// Called when instance created for <see cref="FastBarBitmapSegment"/>.
        /// </summary>
        public FastBarBitmapSegment()
        {
            x1Values = new List<float>();
            x2Values = new List<float>();
            y1Values = new List<float>();
            y2Values = new List<float>();
        }

        /// <summary>
        /// Called when instance created for <see cref="FastBarBitmapSegment"/>.
        /// </summary>
        /// <param name="series">Specifies the instance of series.</param>
        public FastBarBitmapSegment(ChartSeries series)
        {
            x1Values = new List<float>();
            x2Values = new List<float>();
            y1Values = new List<float>();
            y2Values = new List<float>();
            Series = series;
        }

        /// <summary>
        /// Called when instance created for <see cref="FastBarBitmapSegment"/>.
        /// </summary>
        /// <param name="x1Values">specifies the x1 values.</param>
        /// <param name="y1Values">specifies the y1 values.</param>
        /// <param name="x2Values">specifies the x2 values.</param>
        /// <param name="y2Values">specifies the y2 values.</param>
        /// <param name="series">instance of the series.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        public FastBarBitmapSegment(IList<double> x1Values, IList<double> y1Values, IList<double> x2Values, IList<double> y2Values, ChartSeries series)
            : this(series)
        {
            base.Series = series;
            if (Series.ActualXAxis is CategoryAxis && !(Series.ActualXAxis as CategoryAxis).IsIndexed)
                base.Item = series.GroupedActualData;
            else
                base.Item = series.ActualData;
        }

        #endregion

        #region Methods

        #region Public Override Methods

        /// <inheritdoc/>
        internal override UIElement CreateVisual(Size size)
        {
            return null;
        }

        /// <inheritdoc/>
        internal override void SetData(IList<double> x1Values, IList<double> y1Values, IList<double> x2Values, IList<double> y2Values)
        {
            this.x1ChartVals = x1Values;
            this.y1ChartVals = y1Values;
            this.x2ChartVals = x2Values;
            this.y2ChartVals = y2Values;
            double X_MAX = x2Values.Max();
            double Y_MAX = y1Values.Max();
            double X_MIN = x1Values.Min();
            double _Min = y1ChartVals.Min();
            double Y_MIN;
            if (double.IsNaN(_Min))
            {
                var yVal = y1ChartVals.Where(e => !double.IsNaN(e));
                Y_MIN = (!yVal.Any()) ? 0 : yVal.Min();
            }
            else
            {
                Y_MIN = _Min;
            }

            double Y2_Min = y2ChartVals.Min();

            XRange = new DoubleRange(X_MIN, X_MAX);
            YRange = new DoubleRange(Y_MIN > Y2_Min ? Y2_Min : Y_MIN, Y_MAX);

        }

        /// <inheritdoc/>
        internal override UIElement GetRenderedVisual()
        {
            return null;
        }

        /// <inheritdoc/>
        internal override void Update(IChartTransformer transformer)
        {
            bitmap = (Series as ChartSeries).Chart.GetFastRenderSurface();
            if (transformer != null && Series.PointsCount != 0)
            {
                ChartTransform.ChartCartesianTransformer cartesianTransformer =
                    transformer as ChartTransform.ChartCartesianTransformer;
                x_isInversed = cartesianTransformer.XAxis.IsInversed;
                y_isInversed = cartesianTransformer.YAxis.IsInversed;

                xStart = cartesianTransformer.XAxis.VisibleRange.Start;
                xEnd = cartesianTransformer.XAxis.VisibleRange.End;

                yStart = cartesianTransformer.YAxis.VisibleRange.Start;
                yEnd = cartesianTransformer.YAxis.VisibleRange.End;


                width = cartesianTransformer.XAxis.RenderedRect.Height;
                height = cartesianTransformer.YAxis.RenderedRect.Width;

                // WPF-14441 - Calculating Bar Position for the Series  
                left = (Series as ChartSeries).Chart.SeriesClipRect.Right - cartesianTransformer.YAxis.RenderedRect.Right;
                top = (Series as ChartSeries).Chart.SeriesClipRect.Bottom - cartesianTransformer.XAxis.RenderedRect.Bottom;

                availableSize = new Size(width, height);
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

                x1Values.Clear();
                x2Values.Clear();
                y1Values.Clear();
                y2Values.Clear();
                //Removed the screen point calculation methods and added the point to value method.
                CalculatePoints(cartesianTransformer);
                UpdateVisual();
            }
        }

        /// <inheritdoc/>
        internal override void OnSizeChanged(Size size)
        {
        }

        #endregion

        #region Internal Methods

        internal void UpdateVisual()
        {
            var fastBarSeries = Series;
            bool isMultiColor = fastBarSeries.PaletteBrushes != null && fastBarSeries.Fill == null;
            float x1 = 0, x2, y1, y2, diff = 0;
            NativeColor color;
            ChartBase chart = fastBarSeries.Chart;
            isSeriesSelected = false;
            NativeColor segmentColor = GetColor(this.Fill);

            //Set SeriesSelectionBrush and Check EnableSeriesSelection        
            if (chart.GetEnableSeriesSelection())
            {
                Brush seriesSelectionBrush = chart.GetSeriesSelectionBrush(fastBarSeries);
                if (seriesSelectionBrush != null
                    && chart.SelectedSeriesCollection.Contains(fastBarSeries))
                {
                    isSeriesSelected = true;
                    seriesSelectionColor = ((SolidColorBrush)seriesSelectionBrush).Color;
                }
            }

            int dataCount = x1Values.Count;
            if (bitmap != null && x1Values.Count != 0)
            {
                fastBuffer = chart.GetFastBuffer();
                double width = (int)chart.SeriesClipRect.Width;
                double height = (int)chart.SeriesClipRect.Height;

                for (int i = 0; i < dataCount; i++)
                {
                    if (isSeriesSelected)
                        color = seriesSelectionColor;
                    else if (isMultiColor)
                    {
                        Brush brush = fastBarSeries.PaletteBrushes[startIndex % fastBarSeries.PaletteBrushes.Count()];
                        color = GetColor(brush);
                    }
                    else
                        color = segmentColor;

                    startIndex++;
                    x1 = x1Values[i];
                    x2 = x2Values[i];
                    y1 = y1ChartVals[i] > 0 ? y1Values[i] : y2Values[i];
                    y2 = y1ChartVals[i] > 0 ? y2Values[i] : y1Values[i];
                    double spacing = (fastBarSeries as ISegmentSpacing).SegmentSpacing;
                    if (spacing > 0 && spacing <= 1)
                    {
                        double leftpos = (Series as ISegmentSpacing).CalculateSegmentSpacing(spacing, x1, x2);
                        double rightpos = (Series as ISegmentSpacing).CalculateSegmentSpacing(spacing, x2, x1);
                        x2 = (float)leftpos;
                        x1 = (float)rightpos;
                    }
                    diff = x2 - x1;

                    if (y1 < y2)
                    {
                        bitmap.FillRectangle(fastBuffer, (int)width, (int)height, (int)(width - y2), (int)(height - x1 - diff),
                           (int)(width - y1), (int)(height - x1), color, fastBarSeries.bitmapPixels);

                        Series.bitmapRects.Add(new Rect(new Point((width - y2), (height - x1 - diff)),
                           new Point((width - y1), (height - x1))));
                    }
                    else
                    {
                        bitmap.FillRectangle(fastBuffer, (int)width, (int)height, (int)(width - y1), (int)(height - x1 - diff),
                           (int)(width - y2), (int)(height - x1), color, fastBarSeries.bitmapPixels);

                        Series.bitmapRects.Add(new Rect(new Point((width - y1), (height - x1 - diff)),
                            new Point((width - y2), (height - x1))));
                    }
                }
            }
            chart.CanRenderToBuffer = true;

            x1Values.Clear();
            x2Values.Clear();
            y1Values.Clear();
            y2Values.Clear();
        }


        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        internal override void SetVisualBindings(Shape element)
        {
        }

        #endregion
        
        #region Private Methods

        private void CalculatePoints(ChartTransform.ChartCartesianTransformer cartesianTransformer)
        {
            ChartAxis xAxis = cartesianTransformer.XAxis;
            if (this.Series.IsIndexed)
            {
                var isGrouping = (Series.ActualXAxis is CategoryAxis) &&
                               (Series.ActualXAxis as CategoryAxis).IsIndexed;
                int start = 0, end = 0;
                if (!isGrouping)
                {
                    start = 0;
                    end = x1ChartVals.Count - 1;
                }
                else
                {
                    start = (int)Math.Floor(xAxis.VisibleRange.Start);
                    end = (int)Math.Ceiling(xAxis.VisibleRange.End);
                    end = end > y1ChartVals.Count - 1 ? y1ChartVals.Count - 1 : end;
                }
                start = start < 0 ? 0 : start;
                for (int i = start; i <= end; i++)
                {
                    AddDataPoint(cartesianTransformer, i);
                }
                startIndex = start;
            }
            else if (this.Series.IsLinearData)
            {
                double start = xAxis.VisibleRange.Start;
                double end = xAxis.VisibleRange.End;
                int i = 0;
                int count = x1ChartVals.Count - 1;
                startIndex = 0;
                for (i = 1; i < count; i++)
                {
                    double xValue = x1ChartVals[i];
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
                    AddDataPoint(cartesianTransformer, i);
            }
        }

        private void AddDataPoint(ChartTransform.ChartCartesianTransformer cartesianTransformer, int index)
        {
            float x1Value = 0, x2Value = 0, y1Value = 0, y2Value = 0;
            double x1Val = 0, x2Val = 0, y2Val = 0, y1Val = 0;
            GetPoints(index, out x1Val, out x2Val, out y1Val, out y2Val);
            x1Value = (float)(top + availableSize.Width * cartesianTransformer.XAxis.ValueToCoefficient(x1Val));
            x2Value = (float)(top + availableSize.Width * cartesianTransformer.XAxis.ValueToCoefficient(x2Val));
            y1Value = (float)(left + availableSize.Height * (1 - cartesianTransformer.YAxis.ValueToCoefficient(y1Val)));
            y2Value = (float)(left + availableSize.Height * (1 - cartesianTransformer.YAxis.ValueToCoefficient(y2Val)));
            x1Values.Add(x1Value);
            x2Values.Add(x2Value);
            y1Values.Add(y1Value);
            y2Values.Add(y2Value);
        }

        private void InsertDataPoint(ChartTransform.ChartCartesianTransformer cartesianTransformer, int index)
        {
            double x1Val = 0, x2Val = 0, y2Val = 0, y1Val = 0;
            GetPoints(index, out x1Val, out x2Val, out y1Val, out y2Val);
            x1Values.Insert(0, (float)(top + availableSize.Width * cartesianTransformer.XAxis.ValueToCoefficient(x1Val)));
            x2Values.Insert(0, (float)(top + availableSize.Width * cartesianTransformer.XAxis.ValueToCoefficient(x2Val)));
            y1Values.Insert(0, (float)(left + availableSize.Height * (1 - cartesianTransformer.YAxis.ValueToCoefficient(y1Val))));
            y2Values.Insert(0, (float)(left + availableSize.Height * (1 - cartesianTransformer.YAxis.ValueToCoefficient(y2Val))));
        }

        private void GetPoints(int index, out double x1Value, out double x2Value,
            out double y1Value, out double y2Value)
        {
            if (x_isInversed)
            {
                x1Value = x2ChartVals[index];
                x2Value = x1ChartVals[index];
            }
            else
            {
                x1Value = x1ChartVals[index];
                x2Value = x2ChartVals[index];
            }
            if (y_isInversed)
            {
                y1Value = y2ChartVals[index];
                y2Value = y1ChartVals[index];
            }
            else
            {
                y1Value = y1ChartVals[index];
                y2Value = y2ChartVals[index];
            }
        }

        #endregion

        #endregion        
    }
}
