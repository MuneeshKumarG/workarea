using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using NativeColor = Windows.UI.Color;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class FastColumnBitmapSegment : ChartSegment
    {
        #region Fields

        #region Internal Properties

        internal IList<double> y1ChartVals;

        #endregion

        #region Pivate Fields

        private List<int> actualIndexes;

        private WriteableBitmap bitmap;

        private byte[] fastBuffer;

        private IList<double> x1ChartVals;
        
        private IList<double> x2ChartVals;

        private IList<double> y2ChartVals;

        Windows.UI.Color seriesSelectionColor = Colors.Transparent;

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
        /// 
        /// </summary>
        public FastColumnBitmapSegment()
        {
            x1Values = new List<float>();
            x2Values = new List<float>();
            y1Values = new List<float>();
            y2Values = new List<float>();
            actualIndexes = new List<int>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="series"></param>
        public FastColumnBitmapSegment(ChartSeries series)
        {
            x1Values = new List<float>();
            x2Values = new List<float>();
            y1Values = new List<float>();
            y2Values = new List<float>();
            actualIndexes = new List<int>();
            Series = series;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1Values"></param>
        /// <param name="y1Values"></param>
        /// <param name="x2Values"></param>
        /// <param name="y2Values"></param>
        /// <param name="series"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        public FastColumnBitmapSegment(IList<double> x1Values, IList<double> y1Values, IList<double> x2Values, IList<double> y2Values, ChartSeries series)
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

        internal override void Dispose()
        {
            if(this.y1ChartVals != null)
            {
                y1ChartVals.Clear();
                y1ChartVals = null;
            }

            if(this.y2ChartVals != null)
            {
                y2ChartVals.Clear();
                y2ChartVals = null;
            }

            if(this.x1ChartVals != null)
            {
                x1ChartVals.Clear();
                x1ChartVals = null;
            }

            if(this.x2ChartVals != null)
            {
                x2ChartVals.Clear();
                x2ChartVals = null;
            }

            if(this.actualIndexes != null)
            {
                this.actualIndexes.Clear();
                this.actualIndexes = null;
            }


            if (this.fastBuffer != null)
                fastBuffer = null;

            if (bitmap != null)
                bitmap = null;

            base.Dispose();
        }

        /// <inheritdoc/>     
        internal override UIElement CreateVisual(Size size)
        {
            bitmap = (Series as ChartSeries).Chart.GetFastRenderSurface();
            fastBuffer = (Series as ChartSeries).Chart.GetFastBuffer();

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
            YRange = new DoubleRange((Y_MIN < Y2_Min && Y_MAX > Y2_Min) ? Y_MIN : Y2_Min, Y2_Min < Y_MAX ? Y_MAX : Y_MIN);

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
                ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
                x_isInversed = cartesianTransformer.XAxis.IsInversed;
                y_isInversed = cartesianTransformer.YAxis.IsInversed;

                x1Values.Clear();
                x2Values.Clear();
                y1Values.Clear();
                y2Values.Clear();
                actualIndexes.Clear();
                //Removed the existing screen point calculation methods and added the TransformVisible method.
                CalculatePoints(cartesianTransformer);
                UpdateVisual(true);

            }
        }
        
        /// <inheritdoc/>
        internal override void OnSizeChanged(Size size)
        {
            bitmap = (Series as ChartSeries).Chart.GetFastRenderSurface();
            fastBuffer = (Series as ChartSeries).Chart.GetFastBuffer();
        }

        #endregion

        #region Internal Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        internal void UpdateVisual(bool updateHiLoLine)
        {
            var fastColumnSeries = Series;
            float x1 = 0;
            float x2 = 0;
            float y1 = 0;
            float y2 = 0;
            bool isMultiColor = fastColumnSeries.PaletteBrushes != null && fastColumnSeries.Fill == null;
            NativeColor color;
            ChartBase chart = fastColumnSeries.Chart;
            NativeColor segmentColor = GetColor(this.Fill);
            isSeriesSelected = false;

            //Set SeriesSelectionBrush and Check EnableSeriesSelection        
            if (chart.GetEnableSeriesSelection())
            {
                Brush seriesSelectionBrush = chart.GetSeriesSelectionBrush(fastColumnSeries);
                if (seriesSelectionBrush != null && chart.SelectedSeriesCollection.Contains(fastColumnSeries))
                {
                    isSeriesSelected = true;
                    seriesSelectionColor = ((SolidColorBrush)seriesSelectionBrush).Color;
                }
            }

            int dataCount = x1Values.Count;
            if (bitmap != null && x1Values.Count != 0)
            {
                fastBuffer = fastColumnSeries.Chart.GetFastBuffer();
                int width = (int)fastColumnSeries.Chart.SeriesClipRect.Width;
                int height = (int)fastColumnSeries.Chart.SeriesClipRect.Height;

                if (fastColumnSeries is FastColumnBitmapSeries)
                {
                    for (int i = 0; i < dataCount; i++)
                    {
                        if (double.IsNaN(y1Values[i])) continue;

                        if (isSeriesSelected)
                            color = seriesSelectionColor;
                        else if (isMultiColor)
                            color = GetColor(fastColumnSeries.PaletteBrushes[actualIndexes[i] % fastColumnSeries.PaletteBrushes.Count()]);
                        else
                            color = segmentColor;
                        startIndex++;
                        x1 = x1Values[i];
                        x2 = x2Values[i];
                        y1 = y1ChartVals[i] > 0 ? y1Values[i] : y2Values[i];
                        y2 = y1ChartVals[i] > 0 ? y2Values[i] : y1Values[i];
                        if (y1 == 0 && y2 == 0)
                            continue;
                        
                        {
                            double spacing = (fastColumnSeries as ISegmentSpacing).SegmentSpacing;
                            if (spacing > 0 && spacing <= 1)
                            {
                                double leftpos = (Series as ISegmentSpacing).CalculateSegmentSpacing(spacing, x2, x1);
                                double rightpos = (Series as ISegmentSpacing).CalculateSegmentSpacing(spacing, x1, x2);
                                x1 = (float)(leftpos);
                                x2 = (float)(rightpos);
                            }
                        }

                        if (y1 < y2)
                        {
                            bitmap.FillRectangle(fastBuffer, width, height, (int)(x1), (int)y1, (int)x2, (int)y2, color, fastColumnSeries.bitmapPixels);
                            Series.bitmapRects.Add(new Rect(new Point(x1, y1), new Point(x2, y2)));
                        }
                        else
                        {
                            bitmap.FillRectangle(fastBuffer, width, height, (int)(x1), (int)y2, (int)x2, (int)y1, color, fastColumnSeries.bitmapPixels);
                            Series.bitmapRects.Add(new Rect(new Point(x1, y2), new Point(x2, y1)));
                        }
                    }
                }

            }
            fastColumnSeries.Chart.CanRenderToBuffer = true;
            x1Values.Clear();
            y1Values.Clear();
            y2Values.Clear();
            x2Values.Clear();
            actualIndexes.Clear();
        }

#endregion

#region Protected Override Methods

        /// <inheritdoc/>      
        internal override void SetVisualBindings(Shape element)
        {
            base.SetVisualBindings(element);
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Stroke");
            element.SetBinding(Shape.StrokeProperty, binding);
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
                startIndex = start;
                for (int i = start; i <= end; i++)
                {
                    AddDataPoint(cartesianTransformer, i);
                }
            }
            else if (this.Series.IsLinearData)
            {
                double start = xAxis.VisibleRange.Start;
                double end = xAxis.VisibleRange.End;
                startIndex = 0;
                int i = 0;
                int count = x1ChartVals.Count - 1;
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
            double x1Val = 0, x2Val = 0, y2Val = 0, y1Val = 0;
            GetXYPoints(index, out x1Val, out x2Val, out y1Val, out y2Val);
            Point tlpoint = cartesianTransformer.TransformToVisible(x1Val, y1Val);
            Point rbpoint = cartesianTransformer.TransformToVisible(x2Val, y2Val);
            x1Values.Add((float)tlpoint.X);
            x2Values.Add((float)rbpoint.X);
            y1Values.Add((float)tlpoint.Y);
            y2Values.Add((float)rbpoint.Y);
            actualIndexes.Add(index);
        }
        private void InsertDataPoint(ChartTransform.ChartCartesianTransformer cartesianTransformer, int index)
        {
            double x1Val = 0, x2Val = 0, y2Val = 0, y1Val = 0;
            GetXYPoints(index, out x1Val, out x2Val, out y1Val, out y2Val);
            Point tlpoint = cartesianTransformer.TransformToVisible(x1Val, y1Val);
            Point rbpoint = cartesianTransformer.TransformToVisible(x2Val, y2Val);
            x1Values.Insert(0, ((float)tlpoint.X));
            x2Values.Insert(0, ((float)rbpoint.X));
            y1Values.Insert(0, ((float)tlpoint.Y));
            y2Values.Insert(0, ((float)rbpoint.Y));
            actualIndexes.Insert(0, index);
        }
        private void GetXYPoints(int index, out double x1Value, out double x2Value,
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
