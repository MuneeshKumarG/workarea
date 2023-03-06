using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class XyDataSeries : CartesianSeries
    {
        #region Dependency Property Registration
        
        /// <summary>
        /// The DependencyProperty for <see cref="YBindingPath"/> property.       .
        /// </summary>
        public static readonly DependencyProperty YBindingPathProperty =
            DependencyProperty.Register(
                nameof(YBindingPath), 
                typeof(string),
                typeof(XyDataSeries),
                new PropertyMetadata(null, OnYBindingPathChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="StrokeWidth"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeWidthProperty =
            DependencyProperty.Register(nameof(StrokeWidth), typeof(double), typeof(XyDataSeries),
            new PropertyMetadata(2d, OnStrokeChanged));

        #endregion

        #region Constructor

        /// <summary>
        /// Called when instance created for XyDataSeries 
        /// </summary>
        public XyDataSeries()
        {
            YValues = new List<double>();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a path value on the source object to serve a y value to the series.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the y plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string YBindingPath
        {
            get { return (string)GetValue(YBindingPathProperty); }
            set { SetValue(YBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the stroke thickness of a chart series.
        /// </summary>
        /// <value>It accepts double values and its default value is 2.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red"
        ///                            StrokeWidth = "3"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth= 3,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double StrokeWidth {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        #endregion

        #region Protected Internal Properties

        /// <summary>
        /// Gets or sets the y values collection.
        /// </summary>
        internal IList<double> YValues
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Virutal Methods

        internal virtual void UpdatePreivewSeriesDragging(Point mousePos)
        {
        }

        internal virtual void UpdatePreviewSegmentDragging(Point mousePos)
        {
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// This method is used to gets the selected data point segment pixel positions
        /// </summary>
        internal void GenerateColumnPixels()
        {
            if (dataPoint != null && !double.IsNaN(dataPoint.YData))
            {
                WriteableBitmap bmp = Chart.fastRenderSurface;

                IChartTransformer chartTransformer = CreateTransformer(
                    new Size(
                        Chart.SeriesClipRect.Width,
                        Chart.SeriesClipRect.Height),
                        true);
                bool x_isInversed = ActualXAxis.IsInversed;
                bool y_isInversed = ActualYAxis.IsInversed;

                DoubleRange sbsInfo = GetSideBySideInfo(this);
                double origin = 0;
                double x1 = x_isInversed ? dataPoint.XData + sbsInfo.End : dataPoint.XData + sbsInfo.Start;
                double x2 = x_isInversed ? dataPoint.XData + sbsInfo.Start : dataPoint.XData + sbsInfo.End;
                double y1 = y_isInversed ? origin : dataPoint.YData;
                double y2 = y_isInversed ? dataPoint.YData : origin;

                Point tlpoint = chartTransformer.TransformToVisible(x1, y1);
                Point rbpoint = chartTransformer.TransformToVisible(x2, y2);

                double _x1 = tlpoint.X;
                double _x2 = rbpoint.X;
                double _y1 = y1 > 0 ? tlpoint.Y : rbpoint.Y;
                double _y2 = y1 > 0 ? rbpoint.Y : tlpoint.Y;
                int width = (int)Chart.SeriesClipRect.Width;
                int height = (int)Chart.SeriesClipRect.Height;

                var spacingSegment = this as ISegmentSpacing;
                if (spacingSegment != null)
                {
                    double spacing = spacingSegment.SegmentSpacing;
                    if (spacing > 0 && spacing <= 1)
                    {
                        double leftpos = spacingSegment.CalculateSegmentSpacing(spacing, _x2, _x1);
                        double rightpos = spacingSegment.CalculateSegmentSpacing(spacing, _x1, _x2);
                        _x1 = (float)(leftpos);
                        _x2 = (float)(rightpos);
                    }
                }

                selectedSegmentPixels.Clear();

                if (_y1 < _y2)
                    selectedSegmentPixels = bmp.GetRectangle(width, height, (int)(_x1), (int)_y1, (int)_x2, (int)_y2, selectedSegmentPixels);
                else
                    selectedSegmentPixels = bmp.GetRectangle(width, height, (int)(_x1), (int)_y2, (int)_x2, (int)_y1, selectedSegmentPixels);
            }
        }

        /// <summary>
        /// This method used to gets the selected data point segment pixel positions 
        /// </summary>
        internal void GenerateBarPixels()
        {
            WriteableBitmap bmp = Chart.fastRenderSurface;

            ChartTransform.ChartCartesianTransformer? cartesianTransformer = CreateTransformer(
               new Size(
                   Chart.SeriesClipRect.Width,
                   Chart.SeriesClipRect.Height),
               true) as ChartTransform.ChartCartesianTransformer;

            DoubleRange sbsInfo = this.GetSideBySideInfo(this);

            float x1Value = 0, x2Value = 0, y1Value = 0, y2Value = 0;

            double x1 = dataPoint.XData + sbsInfo.Start;
            double x2 = dataPoint.XData + sbsInfo.End;
            double y1 = dataPoint.YData;
            double y2 = 0;

            double xStart = cartesianTransformer.XAxis.VisibleRange.Start;
            double xEnd = cartesianTransformer.XAxis.VisibleRange.End;

            double yStart = cartesianTransformer.YAxis.VisibleRange.Start;
            double yEnd = cartesianTransformer.YAxis.VisibleRange.End;

            double width = cartesianTransformer.XAxis.RenderedRect.Height;
            double height = cartesianTransformer.YAxis.RenderedRect.Width;

            // WPF-14441 - Calculating Bar Position for the Series  
            double left = Chart.SeriesClipRect.Right - cartesianTransformer.YAxis.RenderedRect.Right;
            double top = Chart.SeriesClipRect.Bottom - cartesianTransformer.XAxis.RenderedRect.Bottom;

            Size availableSize = new Size(width, height);

            if (ActualXAxis.IsInversed)
            {
                double temp = xStart;
                xStart = xEnd;
                xEnd = temp;
            }

            if (ActualYAxis.IsInversed)
            {
                double temp = yStart;
                yStart = yEnd;
                yEnd = temp;
            }

            {
                double x1Val = ActualXAxis.IsInversed
                          ? x2 < xEnd ? xEnd : x2
                          : x1 < xStart ? xStart : x1;
                double x2Val = ActualXAxis.IsInversed
                                   ? x1 > xStart ? xStart : x1
                                   : x2 > xEnd ? xEnd : x2;

                double y1Val = ActualYAxis.IsInversed
                                   ? y2 > yStart ? yStart : y2 < yEnd ? yEnd : y2
                                   : y1 > yEnd ? yEnd : y1 < yStart ? yStart : y1;
                double y2Val = ActualYAxis.IsInversed
                                   ? y1 < yEnd ? yEnd : y1 > yStart ? yStart : y1
                                   : y2 < yStart ? yStart : y2 > yEnd ? yEnd : y2;
                x1Value = (float)(top + (availableSize.Width) * cartesianTransformer.XAxis.ValueToCoefficient(x1Val));
                x2Value = (float)(top + (availableSize.Width) * cartesianTransformer.XAxis.ValueToCoefficient(x2Val));
                y1Value = (float)(left + (availableSize.Height) * (1 - cartesianTransformer.YAxis.ValueToCoefficient(y1Val)));
                y2Value = (float)(left + (availableSize.Height) * (1 - cartesianTransformer.YAxis.ValueToCoefficient(y2Val)));
            }
           

            double _x1 = x1Value;
            double _x2 = x2Value;
            double _y1 = y1 > 0 ? y1Value : y2Value;
            double _y2 = y1 > 0 ? y2Value : y1Value;

            var spacingSegment = this as ISegmentSpacing;

            if (spacingSegment != null)
            {
                double spacing = spacingSegment.SegmentSpacing;
                if (spacing > 0 && spacing <= 1)
                {
                    double leftpos = spacingSegment.CalculateSegmentSpacing(spacing, _x2, _x1);
                    double rightpos = spacingSegment.CalculateSegmentSpacing(spacing, _x1, _x2);
                    _x1 = (float)(leftpos);
                    _x2 = (float)(rightpos);
                }
            }

            double diff = _x2 - _x1;
            width = (int)Chart.SeriesClipRect.Width;
            height = (int)Chart.SeriesClipRect.Height;

            selectedSegmentPixels.Clear();

            if (_y1 < _y2)
                selectedSegmentPixels = bmp.GetRectangle((int)width, (int)height, (int)(width - _y2), (int)(height - _x1 - diff), (int)(width - _y1), (int)(height - _x1), selectedSegmentPixels);
            else
                selectedSegmentPixels = bmp.GetRectangle((int)width, (int)height, (int)(width - y1), (int)(height - x1 - diff), (int)(width - y2), (int)(height - x1), selectedSegmentPixels);
        }

        #endregion

        #region Internal Override Methods

        /// <summary>
        /// This method used to get the chart data at an index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal override ChartDataPointInfo? GetDataPoint(int index)
        {
            if (this.ActualXAxis is CategoryAxis axis && !axis.IsIndexed)
            {
                IList<double> xValues = GroupedXValuesIndexes;
                dataPoint = null;
                if (index >= 0 && index < xValues.Count)
                {
                    dataPoint = new ChartDataPointInfo();

                    if (xValues.Count > index)
                        dataPoint.XData = xValues[index];

                    dataPoint.Index = index;
                    dataPoint.Series = this;

                    if (this is ColumnSeries || this is FastColumnBitmapSeries
                        || this is StackedColumnSeries)
                    {
                        if (GroupedSeriesYValues[0].Count > index)
                            dataPoint.YData = GroupedSeriesYValues[0][index];
                        if (GroupedActualData.Count > index)
                            dataPoint.Item = GroupedActualData[index];
                    }
                    else
                    {
                        if (YValues.Count > index)
                            dataPoint.YData = YValues[index];
                        if (ActualData?.Count > index)
                            dataPoint.Item = ActualData[index];
                    }
                }

                return dataPoint;
            }
            else
            {
                IList<double> xValues = GetXValues();
                dataPoint = null;
                if (index >= 0 && index < xValues.Count)
                {
                    dataPoint = new ChartDataPointInfo();

                    if (xValues.Count > index)
                        dataPoint.XData = IsIndexed ? index : xValues[index];

                    if (YValues.Count > index)
                        dataPoint.YData = YValues[index];

                    dataPoint.Index = index;
                    dataPoint.Series = this;

                    if (ActualData?.Count > index)
                        dataPoint.Item = ActualData[index];
                }

                return dataPoint;
            }
        }

        /// <summary>
        /// This method used to get the chart data at an index.
        /// </summary>
        internal override void GeneratePixels()
        {
            if (Chart is SfCartesianChart chart && dataPoint != null)
            {
                if (this is FastColumnBitmapSeries)
                {
                    if (!chart.IsTransposed)
                        GenerateColumnPixels();
                    else
                        GenerateBarPixels();
                }
            }
        }

        #endregion

        #region Protected Internal Methods

        /// <summary>
        /// Method for Generate Points for XYDataSeries
        /// </summary>
        internal override void GenerateDataPoints()
        {
            if (YBindingPath != null)
                GeneratePoints(new string[] { YBindingPath }, YValues);
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            base.OnDataSourceChanged(oldValue, newValue);
            YValues.Clear();
            GeneratePoints(new string[] { YBindingPath }, YValues);
            this.ScheduleUpdateChart();
        }

        /// <inheritdoc/>
        internal override void OnBindingPathChanged()
        {
            YValues.Clear();
            ResetData();
            GeneratePoints(new[] { YBindingPath }, YValues);
            if (this.Chart != null && this.Chart.PlotArea != null)
                this.Chart.PlotArea.ShouldPopulateLegendItems = true;
            base.OnBindingPathChanged();
        }

        #endregion

        #region Private Static Methods

        private static void OnYBindingPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is XyDataSeries series)
                series.OnBindingPathChanged();
        }
       
        #endregion

        #endregion
    }
}
