using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="FastScatterBitmapSeries"/> is a special kind of scatter series that can render a collection with large number of data points.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="FastScatterBitmapSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="Stroke"/>, <see cref="XyDataSeries.StrokeThickness"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="FastScatterBitmapSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="FastScatterBitmapSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
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
    ///           <chart:FastScatterBitmapSeries ItemsSource="{Binding Data}"
    ///                                          XBindingPath="XValue"
    ///                                          YBindingPath="YValue"/>
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
    ///     FastScatterBitmapSeries series = new FastScatterBitmapSeries();
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
    /// <seealso cref="FastScatterBitmapSegment"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class FastScatterBitmapSeries : XyDataSeries
    {
        #region Dependency Property Regisrtation
        /// <summary>
        /// Identifies the PointWidth dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for PointWidth dependency property.
        /// </value> 
        public static readonly DependencyProperty PointWidthProperty =
            DependencyProperty.Register(nameof(PointWidth), typeof(double), typeof(FastScatterBitmapSeries),
            new PropertyMetadata(3d, OnScatterWidthChanged));

        /// <summary>
        /// Identifies the PointHeight dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for PointHeight dependency property.
        /// </value> 
        public static readonly DependencyProperty PointHeightProperty =
            DependencyProperty.Register(nameof(PointHeight), typeof(double), typeof(FastScatterBitmapSeries),
            new PropertyMetadata(3d, OnScatterHeightChanged));

        /// <summary>
        /// Identifies the Type dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for Type dependency property.
        /// </value> 
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(
                nameof(Type),
                typeof(ShapeType),
                typeof(FastScatterBitmapSeries),
                new PropertyMetadata(ShapeType.Circle, OnTypePropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(FastScatterBitmapSeries),
            new PropertyMetadata(null, OnStrokeChanged));

        #endregion

        #region Fields

        #region Private Fields

        private IList<double> xValues;

        private Point hitPoint = new Point();

        private Point startPoint = new Point();

        private Point endPoint = new Point();

        bool isAdornmentsBending;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that specifies the width of the FastScatterBitmap segment.
        /// </summary>
        /// <value>
        /// It accepts double value and the default value is 3.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:FastScatterBitmapSeries ItemsSource="{Binding Data}"
        ///                                         XBindingPath="XValue"
        ///                                         YBindingPath="YValue"
        ///                                         PointWidth = "30"/>
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
        ///     FastScatterBitmapSeries series = new FastScatterBitmapSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           PointWidth = 30,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double PointWidth
        {
            get { return (double)GetValue(PointWidthProperty); }
            set { SetValue(PointWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the height of the FastScatterBitmap segment.
        /// </summary>
        /// <value>
        /// It accepts double value and the default value is 3.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:FastScatterBitmapSeries ItemsSource="{Binding Data}"
        ///                                         XBindingPath="XValue"
        ///                                         YBindingPath="YValue"
        ///                                         PointHeight = "30"/>
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
        ///     FastScatterBitmapSeries series = new FastScatterBitmapSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           PointHeight = 30,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double PointHeight
        {
            get { return (double)GetValue(PointHeightProperty); }
            set { SetValue(PointHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets different types of shapes in a fast scatter bitmap series.
        /// </summary>
        /// <value>This property accepts <see cref="ShapeType"/> values, and the default shape type is ellipse.
        /// </value>
        /// <remarks>
        /// Fast scatter bitmap series does not support Custom, HorizontalLine and VerticalLine shapes. By using the above shapes for the fast scatter bitmap series, you can render only the default type, which is an ellipse.
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:FastScatterBitmapSeries ItemsSource="{Binding Data}"
        ///                                         XBindingPath="XValue"
        ///                                         YBindingPath="YValue"
        ///                                         Type = "Diamond"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-9)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     FastScatterBitmapSeries series = new FastScatterBitmapSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Type = ShapeType.Diamond,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ShapeType Type
        {
            get { return (ShapeType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of a chart series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-10)
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
        /// # [C#](#tab/tabid-11)
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
        public Brush Stroke {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        #endregion

        #region Protected Internal Properties

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

        private FastScatterBitmapSegment Segment { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Used to create the segment of <see cref="FastScatterBitmapSeries"/>.
        /// </summary>
        internal override void GenerateSegments()
        {
            bool isGrouping = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).ArrangeByIndex;
            if (isGrouping)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();
            if (isGrouping)
            {
                Segments.Clear();
                Adornments.Clear();

                if (Segments == null || Segments.Count == 0)
                {
                    Segment = CreateSegment() as FastScatterBitmapSegment;
                    if (Segment != null)
                    {
                        Segment.Series = this;
                        Segment.Item = ActualData;
                        Segment.SetData(xValues, GroupedSeriesYValues[0]);
                        Segments.Add(Segment);
                    }
                }
            }
            else
            {
                ClearUnUsedAdornments(this.PointsCount);
                if (Segments == null || Segments.Count == 0)
                {
                    Segment = CreateSegment() as FastScatterBitmapSegment;
                    if (Segment != null)
                    {
                        Segment.Series = this;
                        Segment.Item = ActualData;
                        Segment.SetData(xValues, YValues);
                        Segments.Add(Segment);
                    }
                }
                else if (ActualXValues != null)
                {
                    Segment.SetData(xValues, YValues);
                    Segment.Item = ActualData;
                }
            }

            isAdornmentsBending = true;
        }

        #endregion

        #region Internal Override Methods

        /// <summary>
        /// Method used to return the hittest series while mouse action.
        /// </summary>
        /// <returns></returns>
        internal override bool IsHitTestSeries()
        {
            // Gets the current mouse position chart data point
            ChartDataPointInfo datapoint = GetDataPoint(Chart.adorningCanvasPoint);

            if (datapoint != null)
                return true;

            return false;
        }

        /// <summary>
        /// This method used to get the chart data at an index.
        /// </summary>
        /// <returns></returns>
        internal override void GeneratePixels()
        {
            WriteableBitmap bmp = Chart.fastRenderSurface;

            ChartTransform.ChartCartesianTransformer cartesianTransformer = CreateTransformer(new Size(Chart.SeriesClipRect.Width,
                Chart.SeriesClipRect.Height),
                true) as ChartTransform.ChartCartesianTransformer;

            double xChartValue = dataPoint.XData;
            double yChartValue = dataPoint.YData;
            int i = dataPoint.Index;
            double xValue, yValue;

            if (IsIndexed)
            {
                bool isGrouped = ActualXAxis is CategoryAxis && !((ActualXAxis as CategoryAxis).IsIndexed);
                if (!IsActualTransposed)
                {
                    double xVal = (isGrouped) ? xChartValue : i;
                    double yVal = yChartValue;
                    Point point = cartesianTransformer.TransformToVisible(xVal, yVal);
                    xValue = point.X;
                    yValue = point.Y;
                }
                else
                {
                    double xVal = (isGrouped) ? xChartValue : i;
                    double yVal = yChartValue;
                    Point point = cartesianTransformer.TransformToVisible(xVal, yVal);
                    xValue = point.Y;
                    yValue = point.X;
                }
            }
            else
            {
                if (!IsActualTransposed)
                {
                    double xVal = xChartValue;
                    double yVal = yChartValue;
                    Point point = cartesianTransformer.TransformToVisible(xVal, yVal);
                    xValue = point.X;
                    yValue = point.Y;
                }
                else
                {
                    double xVal = xChartValue;
                    double yVal = yChartValue;
                    Point point = cartesianTransformer.TransformToVisible(xVal, yVal);
                    xValue = point.Y;
                    yValue = point.X;
                }
            }

            double xr = PointHeight, yr = PointWidth;
            int width = (int)Chart.SeriesClipRect.Width;
            int height = (int)Chart.SeriesClipRect.Height;
            selectedSegmentPixels.Clear();

            if (IsActualTransposed)
            {
                if (yValue > -1)
                {
                    selectedSegmentPixels = bmp.GetEllipseCentered(height, width, (int)yValue, (int)xValue, (int)xr, (int)yr, selectedSegmentPixels);
                }
            }
            else
            {
                if (yValue > -1)
                {
                    selectedSegmentPixels = bmp.GetEllipseCentered(height, width, (int)xValue, (int)yValue, (int)xr, (int)yr, selectedSegmentPixels);
                }
            }
        }

        /// <summary>
        /// This method used to gets the chart data point at a position.
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        internal override ChartDataPointInfo GetDataPoint(Point mousePos)
        {
            bool isGrouped = ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed;
            if (isGrouped)
                xValues = GroupedXValuesIndexes;
            else
                xValues = (ActualXValues is IList<double>) ? ActualXValues as IList<double> : GetXValues();

            hitPoint.X = mousePos.X - this.Chart.SeriesClipRect.Left;
            hitPoint.Y = mousePos.Y - this.Chart.SeriesClipRect.Top;

            hitPoint.X = hitPoint.X - PointWidth;
            hitPoint.Y = hitPoint.Y - PointHeight;

            startPoint.X = ActualArea.ActualPointToValue(ActualXAxis, hitPoint);
            startPoint.Y = ActualArea.ActualPointToValue(ActualYAxis, hitPoint);

            hitPoint.X = hitPoint.X + (2 * PointWidth);
            hitPoint.Y = hitPoint.Y + (2 * PointHeight);

            endPoint.X = ActualArea.ActualPointToValue(ActualXAxis, hitPoint);
            endPoint.Y = ActualArea.ActualPointToValue(ActualYAxis, hitPoint);

            Rect rect = new Rect(startPoint, endPoint);

            dataPoint = null;

            for (int i = 0; i < YValues.Count; i++)
            {
                if (isGrouped)
                {
                    if (i < xValues.Count)
                    {
                        hitPoint.X = xValues[i];
                        hitPoint.Y = GroupedSeriesYValues[0][i];
                    }
                    else
                        return dataPoint;
                }
                else
                {
                    hitPoint.X = IsIndexed ? i : xValues[i];
                    hitPoint.Y = YValues[i];
                }

                if (rect.Contains(hitPoint))
                {
                    dataPoint = new ChartDataPointInfo();
                    dataPoint.Index = i;
                    dataPoint.XData = xValues[i];
                    dataPoint.YData = (isGrouped) ?
                                      GroupedSeriesYValues[0][i] : YValues[i];
                    dataPoint.Series = this;
                    if (i > -1 && ActualData.Count > i)
                        dataPoint.Item = ActualData[i];
                    break;
                }
            }

            return dataPoint;
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            ChartDataPointInfo chartPointinfo = ToolTipTag as ChartDataPointInfo;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(chartPointinfo.XData, chartPointinfo.YData);
            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top - PointHeight / 2;
            if (newPosition.Y - tooltip.DesiredSize.Height < ActualArea.SeriesClipRect.Top)
            {
                newPosition.Y += PointHeight;
            }

            return newPosition;
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            return new FastScatterBitmapSegment();
        }

        /// <summary>
        /// Invoked when VisibleRange property changed.
        /// </summary>
        /// <param name="e"><see cref="VisibleRangeChangedEventArgs"/> that contains the event data.</param>
        internal override void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
        {
            bool isGrouped = (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed);
            if (AdornmentsInfo != null && ShowDataLabels && isAdornmentsBending)
            {
                List<double> xValues = null;
                if (isGrouped)
                    xValues = GroupedXValuesIndexes;
                else
                    xValues = GetXValues();
                if (xValues != null && ActualXAxis != null
                    && !ActualXAxis.VisibleRange.IsEmpty)
                {
                    for (int i = 0; i < PointsCount; i++)
                    {
                        if (isGrouped)
                        {
                            if (i < xValues.Count)
                            {
                                AddAdornments(xValues[i], GroupedSeriesYValues[0][i], i);
                            }
                            else
                                return;
                        }
                        else
                            AddAdornments(xValues[i], YValues[i], i);
                    }

                    isAdornmentsBending = false;
                }
            }
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
                    dataPoint.Index = index;

                    if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed
                       && GroupedXValuesIndexes.Count > index)
                    {
                        dataPoint.XData = GroupedXValuesIndexes[index];
                        dataPoint.YData = GroupedSeriesYValues[0][index];
                    }
                    else
                    {
                        if (xValues.Count > index)
                            dataPoint.XData = xValues[index];
                        if (YValues.Count > index)
                            dataPoint.YData = YValues[index];
                    }

                    dataPoint.Series = this;
                    if (ActualData.Count > index)
                        dataPoint.Item = ActualData[index];
                    UpdateSeriesTooltip(dataPoint);
                }
            }
        }

        #endregion

        #region Private Static Method

        private static void OnScatterWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FastScatterBitmapSeries series = d as FastScatterBitmapSeries;
            if (series != null)
                series.ScheduleUpdateChart();
        }

        private static void OnScatterHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FastScatterBitmapSeries series = d as FastScatterBitmapSeries;
            if (series != null)
                series.ScheduleUpdateChart();
        }

        private static void OnTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FastScatterBitmapSeries series = d as FastScatterBitmapSeries;

            if (series == null)
            {
                return;
            }

            if (series.Type == ShapeType.Custom || series.Type == ShapeType.HorizontalLine || series.Type == ShapeType.VerticalLine)
            {
                series.Type = ShapeType.Circle;
            }

            if (series.LegendIcon == ChartLegendIcon.SeriesType)
            {
                series.UpdateLegendIconTemplate(true);
            }

            series.ScheduleUpdateChart();

        }

        #endregion

        #region Private Methods

        private void AddAdornments(double x, double yValue, int i)
        {
            double adornX = 0d, adornY = 0d;
            adornX = x;
            adornY = yValue;
            if (i < Adornments.Count)
            {
                Adornments[i].SetData(adornX, adornY, adornX, adornY);
            }
            else
            {
                Adornments.Add(this.CreateAdornment(this, adornX, adornY, adornX, adornY));
            }

            Adornments[i].Item = ActualData[i];
        }

        #endregion

        #endregion
    }
}
