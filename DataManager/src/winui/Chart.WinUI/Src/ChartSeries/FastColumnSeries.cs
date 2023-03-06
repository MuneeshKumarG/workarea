using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="FastColumnBitmapSeries"/> is a special kind of column series that can render a collection with large number of data points.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="FastColumnBitmapSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="FastColumnBitmapSeries.Stroke"/>, <see cref="XyDataSeries.StrokeWidth"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="FastColumnBitmapSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="FastColumnBitmapSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// <para> <b>SegmentSpacing - </b> To specify the spacing between segments using the <see cref="SegmentSpacing"/> property.</para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.XAxes>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.XAxes>
    ///
    ///           <chart:SfCartesianChart.YAxes>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.YAxes>
    ///
    ///           <chart:FastColumnBitmapSeries ItemsSource="{Binding Data}"
    ///                                         XBindingPath="XValue"
    ///                                         YBindingPath="YValue"/>
    /// 
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    /// 
    ///     NumericalAxis xAxis = new NumericalAxis();
    ///     NumericalAxis yAxis = new NumericalAxis();
    /// 
    ///     chart.XAxes.Add(xAxis);
    ///     chart.YAxes.Add(yAxis);
    /// 
    ///     ViewModel viewModel = new ViewModel();
    /// 
    ///     FastColumnBitmapSeries series = new FastColumnBitmapSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    /// 
    /// ]]>
    /// </code>
    /// # [ViewModel](#tab/tabid-3)
    /// <code><![CDATA[
    ///     public ObservableCollection<Model> Data { get; set; }
    /// 
    ///     public ViewModel()
    ///     {
    ///        Data = new ObservableCollection<Model>();
    ///        Data.Add(new Model() { XValue = 10, YValue = 100 });
    ///        Data.Add(new Model() { XValue = 20, YValue = 150 });
    ///        Data.Add(new Model() { XValue = 30, YValue = 110 });
    ///        Data.Add(new Model() { XValue = 40, YValue = 230 });
    ///     }
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    /// <seealso cref="FastColumnBitmapSegment"/>
    public class FastColumnBitmapSeries : XyDataSeries, ISegmentSpacing
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the SegmentSpacing dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for SegmentSpacing dependency property.
        /// </value> 
        public static readonly DependencyProperty SegmentSpacingProperty =
            DependencyProperty.Register(nameof(SegmentSpacing), typeof(double), typeof(FastColumnBitmapSeries),
            new PropertyMetadata(0.0, OnSegmentSpacingChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(FastColumnBitmapSeries),
            new PropertyMetadata(null, OnStrokeChanged));

        #endregion

        #region Fields

        #region Private Fields

        private List<double> xValues;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that can be used to change the spacing between two segments.
        /// </summary>
        /// <value>It accepts double values and the default value is 0. Here, the value ranges from 0 to 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:FastColumnBitmapSeries ItemsSource="{Binding Data}"
        ///                                        XBindingPath="XValue"
        ///                                        YBindingPath="YValue"
        ///                                        SegmentSpacing = "0.3"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     FastColumnBitmapSeries series = new FastColumnBitmapSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           SegmentSpacing = 0.3,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double SegmentSpacing
        {
            get { return (double)GetValue(SegmentSpacingProperty); }
            set { SetValue(SegmentSpacingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of a chart series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red"
        ///                            StrokeThickness = "3"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-7)
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
        ///           StrokeThickness= 3,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush Stroke {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        #endregion

        #region Protected Internal Properties

        /// <inheritdoc/>
        internal override bool IsSideBySide
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc/>
        internal override bool IsBitmapSeries
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Private Properties

        private ChartSegment Segment { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Interface Methods
        /// <inheritdoc/>
        double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
        {
            return CalculateSegmentSpacing(spacing, Right, Left);
        }

        #endregion

        #region Public Override Methods

        internal override void Dispose()
        {
            if (this.xValues != null)
            {
                this.xValues.Clear();
                xValues = null;
            }

            if (this.Segment != null)
            {
                this.Segment.Dispose();
                this.Segment = null;
            }

            base.Dispose();
        }

        /// <summary>
        /// Used to create the segment of <see cref="FastColumnBitmapSeries"/>.
        /// </summary>
        internal override void GenerateSegments()
        {
            var isGrouped = (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).ArrangeByIndex);
            if (isGrouped)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();
            IList<double> x1Values, x2Values, y1Values, y2Values;
            x1Values = new List<double>();
            x2Values = new List<double>();
            y1Values = new List<double>();
            y2Values = new List<double>();
            x1Values.Clear();
            x2Values.Clear();
            y1Values.Clear();
            y2Values.Clear();

            if (xValues != null)
            {
                ClearUnUsedAdornments(this.PointsCount);
                DoubleRange sbsInfo = this.GetSideBySideInfo(this);
                double origin = 0;

                if (isGrouped)
                {
                    Segments.Clear();
                    Adornments.Clear();
                    GroupedActualData.Clear();

                    for (int i = 0; i < DistinctValuesIndexes.Count; i++)
                    {
                        var list = (from index in DistinctValuesIndexes[i]
                                    where GroupedSeriesYValues[0].Count > index
                                    select new List<double> { GroupedSeriesYValues[0][index], index }).
                              OrderByDescending(val => val[0]).ToList();

                        for (int j = 0; j < list.Count; j++)
                        {
                            var yValue = list[j][0];
                            GroupedActualData.Add(ActualData[(int)list[j][1]]);
                            
                            if (i < DistinctValuesIndexes.Count)
                            {
                                x1Values.Add(i + sbsInfo.Start);
                                x2Values.Add(i + sbsInfo.End);
                                y1Values.Add(yValue);
                                y2Values.Add(origin);  // setting origin value for fastcolumn segment
                            }
                        }
                    }

                    if (Segment != null && (IsActualTransposed && Segment is FastColumnBitmapSegment)
                             || (!IsActualTransposed && Segment is FastBarBitmapSegment))
                        Segments.Clear();

                    if (Segment == null || Segments.Count == 0)
                    {
                        if (IsActualTransposed)
                            Segment = CreateSegment() as FastBarBitmapSegment;
                        else
                            Segment = CreateSegment() as FastColumnBitmapSegment;
                        if (Segment != null)
                        {
                            Segment.Series = this;

                            if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                                Segment.Item = GroupedActualData;
                            else
                                Segment.Item = ActualData;

                            Segment.SetData(x1Values, y1Values, x2Values, y2Values);
                            this.Segments.Add(Segment);
                        }
                    }

                    if (AdornmentsInfo != null && ShowDataLabels)
                    {
                        int count = 0;
                        for (int i = 0; i < DistinctValuesIndexes.Count; i++)
                        {
                            var list = (from index in DistinctValuesIndexes[i]
                                        select new List<double> { GroupedSeriesYValues[0][index], index }).
                               OrderByDescending(val => val[0]).ToList();
                            for (int j = 0; j < DistinctValuesIndexes[i].Count; j++)
                            {
                                var yValue = list[j][0];
                                if (i < xValues.Count)
                                {
                                    BarLabelAlignment markerPosition = this.adornmentInfo.GetAdornmentPosition();
                                    if (markerPosition == BarLabelAlignment.Top)
                                        AddColumnAdornments(i, yValue, x1Values[count], y1Values[count], count, sbsInfo.Delta / 2);
                                    else if (markerPosition == BarLabelAlignment.Bottom)
                                        AddColumnAdornments(i, yValue, x1Values[count], y2Values[count], count, sbsInfo.Delta / 2);
                                    else
                                        AddColumnAdornments(i, yValue, x1Values[count], y1Values[count] + (y2Values[count] - y1Values[count]) / 2, count, sbsInfo.Delta / 2);
                                }

                                count++;
                            }
                        }
                    }
                }
                else
                {
                    if (!this.IsIndexed)
                    {
                        ClearUnUsedAdornments(this.PointsCount);
                        for (int i = 0; i < this.PointsCount; i++)
                        {
                            x1Values.Add(xValues[i] + sbsInfo.Start);
                            x2Values.Add(xValues[i] + sbsInfo.End);
                            y1Values.Add(YValues[i]);
                            y2Values.Add(origin);  // setting origin value for fastcolumn segment
                        }
                    }
                    else
                    {
                        for (int i = 0; i < this.PointsCount; i++)
                        {
                            x1Values.Add(i + sbsInfo.Start);
                            x2Values.Add(i + sbsInfo.End);
                            y1Values.Add(YValues[i]);
                            y2Values.Add(origin);  // Setting origin value for fastcolumn segment
                        }
                    }

                    if (Segment != null && (IsActualTransposed && Segment is FastColumnBitmapSegment)
                              || (!IsActualTransposed && Segment is FastBarBitmapSegment))
                        Segments.Clear();

                    if (Segment == null || Segments.Count == 0)
                    {
                        if (IsActualTransposed)
                            Segment = CreateSegment() as FastBarBitmapSegment;
                        else
                            Segment = CreateSegment() as FastColumnBitmapSegment;
                        if (Segment != null)
                        {
                            Segment.Series = this;
                            if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                                Segment.Item = GroupedActualData;
                            else
                                Segment.Item = ActualData;

                            Segment.SetData(x1Values, y1Values, x2Values, y2Values);
                            this.Segments.Add(Segment);
                        }
                    }
                    else
                    {
                        if (Segment is FastBarBitmapSegment)
                        {
                            (Segment as FastBarBitmapSegment).Item = ActualData;
                            (Segment as FastBarBitmapSegment).SetData(x1Values, y1Values, x2Values, y2Values);
                        }
                        else
                        {
                            (Segment as FastColumnBitmapSegment).Item = ActualData;
                            (Segment as FastColumnBitmapSegment).SetData(x1Values, y1Values, x2Values, y2Values);
                        }
                    }

                    if (AdornmentsInfo != null && ShowDataLabels)
                    {
                        for (int i = 0; i < this.PointsCount; i++)
                        {
                            if (i < this.PointsCount)
                            {
                                BarLabelAlignment markerPosition = this.adornmentInfo.GetAdornmentPosition();
                                if (markerPosition == BarLabelAlignment.Top)
                                    AddColumnAdornments(xValues[i], YValues[i], x1Values[i], y1Values[i], i, sbsInfo.Delta / 2);
                                else if (markerPosition == BarLabelAlignment.Bottom)
                                    AddColumnAdornments(xValues[i], YValues[i], x1Values[i], y2Values[i], i, sbsInfo.Delta / 2);
                                else
                                    AddColumnAdornments(xValues[i], YValues[i], x1Values[i], y1Values[i] + (y2Values[i] - y1Values[i]) / 2, i, sbsInfo.Delta / 2);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Internal Override Methods

        /// <summary>
        /// Method used to return the hittest series while mouse action.
        /// </summary>
        /// <returns></returns>
        internal override bool IsHitTestSeries()
        {
            var point = new Point(Chart.adorningCanvasPoint.X - this.ActualArea.SeriesClipRect.Left,
                Chart.adorningCanvasPoint.Y - this.ActualArea.SeriesClipRect.Top);

            foreach (var rect in bitmapRects)
            {
                if (rect.Contains(point))
                    return true;
            }

            return false;
        }

        internal override int GetDataPointIndex(Point point)
        {
            Canvas canvas = Chart.GetAdorningCanvas();
            double left = Chart.ActualWidth - canvas.ActualWidth;
            double top = Chart.ActualHeight - canvas.ActualHeight;
            ChartDataPointInfo data = null;
            point.X = point.X - left + Chart.Margin.Left;
            point.Y = point.Y - top + Chart.Margin.Top;

            Point mousePos = new Point(point.X - Chart.SeriesClipRect.Left, point.Y - Chart.SeriesClipRect.Top);

            double currentBitmapPixel = (Chart.fastRenderSurface.PixelWidth * (int)mousePos.Y + (int)mousePos.X);

            if (!Chart.isBitmapPixelsConverted)
                Chart.ConvertBitmapPixels();

            if (Pixels.Contains((int)currentBitmapPixel))
                data = GetDataPoint(point);

            if (data != null)
                return data.Index;
            else
                return -1;
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            ChartDataPointInfo chartPointinfo = ToolTipTag as ChartDataPointInfo;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(chartPointinfo.XData, chartPointinfo.YData);
            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top;
            return newPosition;
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            if (IsActualTransposed)
                return new FastBarBitmapSegment();
            else
                return new FastColumnBitmapSegment();
        }

        /// <inheritdoc/>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            if (EnableTooltip)
            {
                Canvas canvas = ActualArea.GetAdorningCanvas();
                mousePosition = e.GetCurrentPoint(canvas).Position;
                ChartDataPointInfo dataPoint = new ChartDataPointInfo();
                int index = ChartExtensionUtils.GetAdornmentIndex(e.OriginalSource);

                if (index > -1)
                {
                    if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                    {
                        if (xValues.Count > index)
                            dataPoint.XData = GroupedXValuesIndexes[index];
                        if (GroupedSeriesYValues[0].Count > index)
                            dataPoint.YData = GroupedSeriesYValues[0][index];

                        if (GroupedActualData.Count > index)
                            dataPoint.Item = GroupedActualData[index];
                    }
                    else
                    {
                        if (xValues.Count > index)
                            dataPoint.XData = xValues[index];
                        if (YValues.Count > index)
                            dataPoint.YData = YValues[index];

                        if (ActualData.Count > index)
                            dataPoint.Item = ActualData[index];
                    }

                    dataPoint.Index = index;
                    dataPoint.Series = this;
                    UpdateSeriesTooltip(dataPoint);
                }
            }
        }

        /// <summary>
        /// Called when <see cref="ChartSeries.ItemsSource"/> property changed.
        /// </summary>
        /// <param name="oldValue">ItemsSource old value.</param>
        /// <param name="newValue">ItemsSource new value</param>
        /// <seealso cref="ChartSeries.XBindingPath"/>
        /// <seealso cref="XyDataSeries.YBindingPath"/>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            Segment = null;
            base.OnDataSourceChanged(oldValue, newValue);
        }

        #endregion

        #region Protected Methods
        ///<summary>
        /// Method used to calculate the segment spacing.
        ///</summary>
        /// <param name="spacing">Segment spacing value.</param>
        /// <param name="Right">Segment right value.</param>
        /// <param name="Left">Segment left value.</param>
        /// <returns>Returns the calculated segment space.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        private double CalculateSegmentSpacing(double spacing, double Right, double Left)
        {
            double diff = Right - Left;
            double totalspacing = diff * spacing / 2;
            Left = Left + totalspacing;
            Right = Right - totalspacing;
            return Left;
        }

        #endregion

        #region Private Static Methods

        private static void OnSegmentSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as FastColumnBitmapSeries;
            if (series.Chart != null)
                series.Chart.ScheduleUpdate();
        }

        #endregion
        
        #endregion
    }
}
