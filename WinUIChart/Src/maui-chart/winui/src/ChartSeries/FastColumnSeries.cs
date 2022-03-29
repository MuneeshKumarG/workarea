using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;
using AdornmentsPosition = Syncfusion.UI.Xaml.Charts.BarLabelAlignment;
using SelectionStyle = Syncfusion.UI.Xaml.Charts.SelectionType;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a special kind of column series which uses writeablebitmap for rendering chart points. FastColumnBitmapSeries allows to render a collection with large number of data points.
    /// </summary>
    /// <remarks>
    /// FastColumnBitmapSeries renders large quantity of data in fraction of milliseconds using writeablebitmap. 
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
    ///           <chart:FastColumnBitmapSeries
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
    ///     FastColumnBitmapSeries series = new FastColumnBitmapSeries();
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
    /// <seealso cref="FastLineBitmapSeries"/>
    /// <seealso cref="FastBarBitmapSeries"/>
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
        
        #endregion
        
        #region Fields

        #region Private Fields

        private List<double> xValues;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the spacing between the segments across the series in cluster mode.
        /// </summary>
        /// <value>
        /// Default value is 0 and its value ranges from 0 to 1.
        /// </value>
        public double SegmentSpacing
        {
            get { return (double)GetValue(SegmentSpacingProperty); }
            set { SetValue(SegmentSpacingProperty, value); }
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

        /// <summary>
        /// Used to create the segment of <see cref="FastColumnBitmapSeries"/>.
        /// </summary>
        public override void CreateSegments()
        {
            var isGrouped = (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed);
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
                ClearUnUsedAdornments(this.DataCount);
                DoubleRange sbsInfo = this.GetSideBySideInfo(this);
                double origin = ActualXAxis != null ? ActualXAxis.Origin : 0;
                if (ActualXAxis != null && ActualXAxis.Origin == 0 && ActualYAxis is LogarithmicAxis &&
                    (ActualYAxis as LogarithmicAxis).Minimum != null)
                    origin = (double)(ActualYAxis as LogarithmicAxis).Minimum;

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
                            if (i < xValues.Count)
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
                                    AdornmentsPosition markerPosition = this.adornmentInfo.GetAdornmentPosition();
                                    if (markerPosition == AdornmentsPosition.Top)
                                        AddColumnAdornments(i, yValue, x1Values[count], y1Values[count], count, sbsInfo.Delta / 2);
                                    else if (markerPosition == AdornmentsPosition.Bottom)
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
                        ClearUnUsedAdornments(this.DataCount);
                        for (int i = 0; i < this.DataCount; i++)
                        {
                            x1Values.Add(xValues[i] + sbsInfo.Start);
                            x2Values.Add(xValues[i] + sbsInfo.End);
                            y1Values.Add(YValues[i]);
                            y2Values.Add(origin);  // setting origin value for fastcolumn segment
                        }
                    }
                    else
                    {
                        for (int i = 0; i < this.DataCount; i++)
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
                        for (int i = 0; i < this.DataCount; i++)
                        {
                            if (i < this.DataCount)
                            {
                                AdornmentsPosition markerPosition = this.adornmentInfo.GetAdornmentPosition();
                                if (markerPosition == AdornmentsPosition.Top)
                                    AddColumnAdornments(xValues[i], YValues[i], x1Values[i], y1Values[i], i, sbsInfo.Delta / 2);
                                else if (markerPosition == AdornmentsPosition.Bottom)
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
            var point = new Point(Area.adorningCanvasPoint.X - this.ActualArea.SeriesClipRect.Left,
                Area.adorningCanvasPoint.Y - this.ActualArea.SeriesClipRect.Top);

            foreach (var rect in bitmapRects)
            {
                if (rect.Contains(point))
                    return true;
            }

            return false;
        }

        internal override int GetDataPointIndex(Point point)
        {
            Canvas canvas = Area.GetAdorningCanvas();
            double left = Area.ActualWidth - canvas.ActualWidth;
            double top = Area.ActualHeight - canvas.ActualHeight;
            ChartDataPointInfo data = null;
            point.X = point.X - left + Area.Margin.Left;
            point.Y = point.Y - top + Area.Margin.Top;

            Point mousePos = new Point(point.X - Area.SeriesClipRect.Left, point.Y - Area.SeriesClipRect.Top);

            double currentBitmapPixel = (Area.fastRenderSurface.PixelWidth * (int)mousePos.Y + (int)mousePos.X);

            if (!Area.isBitmapPixelsConverted)
                Area.ConvertBitmapPixels();

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
        protected override ChartSegment CreateSegment()
        {
            if (IsActualTransposed)
                return new FastBarBitmapSegment();
            else
                return new FastColumnBitmapSegment();
        }

        /// <summary>
        /// Called when pointer or mouse moves on chart area.
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
        /// Called when <see cref="ChartSeriesBase.ItemsSource"/> property changed.
        /// </summary>
        /// <param name="oldValue">ItemsSource old value.</param>
        /// <param name="newValue">ItemsSource new value</param>
        /// <seealso cref="ChartSeriesBase.XBindingPath"/>
        /// <seealso cref="XyDataSeries.YBindingPath"/>
        protected override void OnDataSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
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
        protected double CalculateSegmentSpacing(double spacing, double Right, double Left)
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
            if (series.Area != null)
                series.Area.ScheduleUpdate();
        }

        #endregion
        
        #endregion
    }
}
