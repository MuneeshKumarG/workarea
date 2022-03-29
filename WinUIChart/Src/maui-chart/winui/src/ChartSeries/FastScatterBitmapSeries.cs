using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Foundation;
using SelectionStyle = Syncfusion.UI.Xaml.Charts.SelectionType;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a special kind of scatter series which uses writeablebitmap for rendering chart points. FastScatterBitmapSeries allows to render a collection with large number of data points.
    /// </summary>
    /// <remarks>
    /// FastScatterBitmapSeries renders large quantity of data in fraction of milliseconds using writeablebitmap. 
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfChart>
    ///
    ///           <chart:SfChart.PrimaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfChart.PrimaryAxis>
    ///
    ///           <chart:SfChart.SecondaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfChart.SecondaryAxis>
    ///
    ///           <chart:FastScatterBitmapSeries
    ///               ItemsSource="{Binding Data}"
    ///               XBindingPath="XValue"
    ///               YBindingPath="YValue"/>
    ///                    
    ///     </chart:SfChart>
    /// ]]></code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfChart chart = new SfChart();
    ///     
    ///     NumericalAxis primaryAxis = new NumericalAxis();
    ///     NumericalAxis secondaryAxis = new NumericalAxis();
    ///     
    ///     chart.PrimaryAxis = primaryAxis;
    ///     chart.SecondaryAxis = secondaryAxis;
    ///     
    ///     FastScatterBitmapSeries series = new FastScatterBitmapSeries();
    ///     series.ItemsSource = viewmodel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    /// ]]></code>
    /// # [ViewModel.cs](#tab/tabid-3)
    /// <code><![CDATA[
    /// public ObservableCollection<Model> Data { get; set; }
    /// 
    /// public ViewModel()
    /// {
    ///    Data = new ObservableCollection<Model>();
    ///    Data.Add(new Model() { XValue = 10, YValue = 100 });
    ///    Data.Add(new Model() { XValue = 20, YValue = 150 });
    ///    Data.Add(new Model() { XValue = 30, YValue = 110 });
    ///    Data.Add(new Model() { XValue = 40, YValue = 230 });
    /// }
    /// ]]></code>
    /// ***
    /// </example>
    /// <seealso cref="Syncfusion.UI.Xaml.Charts.XyDataSeries" />
    /// <seealso cref="Syncfusion.UI.Xaml.Charts.ISegmentSelectable" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class FastScatterBitmapSeries : XyDataSeries
    {
        #region Dependency Property Regisrtation
        /// <summary>
        /// Identifies the ScatterWidth dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for ScatterWidth dependency property.
        /// </value> 
        public static readonly DependencyProperty ScatterWidthProperty =
            DependencyProperty.Register(nameof(ScatterWidth), typeof(double), typeof(FastScatterBitmapSeries),
            new PropertyMetadata(3d, OnScatterWidthChanged));

        /// <summary>
        /// Identifies the ScatterHeight dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for ScatterHeight dependency property.
        /// </value> 
        public static readonly DependencyProperty ScatterHeightProperty =
            DependencyProperty.Register(nameof(ScatterHeight), typeof(double), typeof(FastScatterBitmapSeries),
            new PropertyMetadata(3d, OnScatterHeightChanged));

        /// <summary>
        /// Identifies the ShapeType dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for ShapeType dependency property.
        /// </value> 
        public static readonly DependencyProperty ShapeTypeProperty =
            DependencyProperty.Register(
                nameof(ShapeType),
                typeof(ChartSymbol),
                typeof(FastScatterBitmapSeries),
                new PropertyMetadata(ChartSymbol.Ellipse, OnShapeTypePropertyChanged));

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
        /// The default value is 3.
        /// </value>
        public double ScatterWidth
        {
            get { return (double)GetValue(ScatterWidthProperty); }
            set { SetValue(ScatterWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the height of the FastScatterBitmap segment.
        /// </summary>
        /// <value>
        /// The default value is 3.
        /// </value>
        public double ScatterHeight
        {
            get { return (double)GetValue(ScatterHeightProperty); }
            set { SetValue(ScatterHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets different types of shapes in a fast scatter bitmap series.
        /// </summary>
        /// <value>This property takes fast scatter shape value, and its default shape type is ellipse.
        /// </value>
        /// <remarks>
        /// Fast scatter bitmap series does not support Custom, HorizontalLine and VerticalLine shapes.
        /// By using the above shapes for fast scatter bitmap series, you can render only the default type, which is ellipse. 
        /// </remarks>
        public ChartSymbol ShapeType
        {
            get { return (ChartSymbol)GetValue(ShapeTypeProperty); }
            set { SetValue(ShapeTypeProperty, value); }
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
        public override void CreateSegments()
        {
            bool isGrouping = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
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
                ClearUnUsedAdornments(this.DataCount);
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
            ChartDataPointInfo datapoint = GetDataPoint(Area.adorningCanvasPoint);

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
            WriteableBitmap bmp = Area.fastRenderSurface;

            ChartTransform.ChartCartesianTransformer cartesianTransformer = CreateTransformer(new Size(Area.SeriesClipRect.Width,
                Area.SeriesClipRect.Height),
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

            double xr = ScatterHeight, yr = ScatterWidth;
            int width = (int)Area.SeriesClipRect.Width;
            int height = (int)Area.SeriesClipRect.Height;
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

            hitPoint.X = mousePos.X - this.Area.SeriesClipRect.Left;
            hitPoint.Y = mousePos.Y - this.Area.SeriesClipRect.Top;

            hitPoint.X = hitPoint.X - ScatterWidth;
            hitPoint.Y = hitPoint.Y - ScatterHeight;

            startPoint.X = ActualArea.ActualPointToValue(ActualXAxis, hitPoint);
            startPoint.Y = ActualArea.ActualPointToValue(ActualYAxis, hitPoint);

            hitPoint.X = hitPoint.X + (2 * ScatterWidth);
            hitPoint.Y = hitPoint.Y + (2 * ScatterHeight);

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
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top - ScatterHeight / 2;
            if (newPosition.Y - tooltip.DesiredSize.Height < ActualArea.SeriesClipRect.Top)
            {
                newPosition.Y += ScatterHeight;
            }

            return newPosition;
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return new FastScatterBitmapSegment();
        }

        /// <summary>
        /// Invoked when VisibleRange property changed.
        /// </summary>
        /// <param name="e"><see cref="VisibleRangeChangedEventArgs"/> that contains the event data.</param>
        protected override void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
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
                    for (int i = 0; i < DataCount; i++)
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
                }

                isAdornmentsBending = false;
            }
        }

        /// <summary>
        /// Called when pointer or mouse move on chart area.
        /// </summary>
        /// <param name="e">Event args that contains the event data.</param>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            if (ShowTooltip)
            {
                Canvas canvas = ActualArea.GetAdorningCanvas();
                mousePos = e.GetCurrentPoint(canvas).Position;
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
                series.UpdateArea();
        }

        private static void OnScatterHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FastScatterBitmapSeries series = d as FastScatterBitmapSeries;
            if (series != null)
                series.UpdateArea();
        }

        private static void OnShapeTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FastScatterBitmapSeries series = d as FastScatterBitmapSeries;

            if (series == null)
            {
                return;
            }

            if (series.ShapeType == ChartSymbol.Custom || series.ShapeType == ChartSymbol.HorizontalLine || series.ShapeType == ChartSymbol.VerticalLine)
            {
                series.ShapeType = ChartSymbol.Ellipse;
            }

            if (series.LegendIcon == ChartLegendIcon.SeriesType)
            {
                series.UpdateLegendIconTemplate(true);
            }

            series.UpdateArea();

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
