using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class TriangularSeriesBase : ChartSeries
    {

        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="YBindingPath"/> property.
        /// </summary>
        public static readonly DependencyProperty YBindingPathProperty =
            DependencyProperty.Register(nameof(YBindingPath), typeof(string), typeof(TriangularSeriesBase),
            new PropertyMetadata(null, OnYPathChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ExplodeIndex"/> property.
        /// </summary>
        public static readonly DependencyProperty ExplodeIndexProperty =
            DependencyProperty.Register(nameof(ExplodeIndex), typeof(int), typeof(TriangularSeriesBase),
            new PropertyMetadata(-1, OnExplodeIndexChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ExplodeOnTap"/> property.
        /// </summary>
        public static readonly DependencyProperty ExplodeOnTapProperty =
            DependencyProperty.Register(nameof(ExplodeOnTap), typeof(bool), typeof(TriangularSeriesBase),
            new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <c>GapRatio</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>GapRatio</c> dependency property.
        /// </value>   
        public static readonly DependencyProperty GapRatioProperty =
            DependencyProperty.Register(
                nameof(GapRatio),
                typeof(double),
                typeof(TriangularSeriesBase),
                new PropertyMetadata(0d, new PropertyChangedCallback(OnGapRatioChanged)));

        /// <summary>
        /// Identifies the <c>ExplodeOffset</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>ExplodeOffset</c> dependency property.
        /// </value>   
        public static readonly DependencyProperty ExplodeOffsetProperty =
            DependencyProperty.Register(
                nameof(ExplodeOffset),
                typeof(double),
                typeof(TriangularSeriesBase),
                new PropertyMetadata(40d, new PropertyChangedCallback(OnExplodeOffsetChanged)));

        #endregion

        #region Constructor

        /// <summary>
        /// Called when instance created for TriangularSeriesBase.
        /// </summary>
        public TriangularSeriesBase()
        {
            YValues = new List<double>();
        }

        #endregion

        #region Fields

        #region Private Fields

        private bool allowExplode;

        private ChartSegment mouseUnderSegment;

        #endregion

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
        /// Gets or sets the index of data point (or segment) of chart series to be exploded.
        /// </summary>
        public int ExplodeIndex
        {
            get { return (int)GetValue(ExplodeIndexProperty); }
            set { SetValue(ExplodeIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether segment slices will explode on click or tap.
        /// </summary>
        /// <value>
        /// if <c>true</c>, the segment will explode on click or tap.
        /// </value>
        public bool ExplodeOnTap
        {
            get { return (bool)GetValue(ExplodeOnTapProperty); }
            set { SetValue(ExplodeOnTapProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double GapRatio
        {
            get { return (double)GetValue(GapRatioProperty); }
            set { SetValue(GapRatioProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double ExplodeOffset
        {
            get { return (double)GetValue(ExplodeOffsetProperty); }
            set { SetValue(ExplodeOffsetProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the Y values collection binded with this series.
        /// </summary>
        internal IList<double> YValues { get; set; }

        #endregion

        #region Internal Virtual Properties

        internal override bool IsSingleAccumulationSeries
        {
            get
            {
                return true;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Virtual Methods

        /// <summary>
        /// Method implementation for ExplodeIndex.
        /// </summary>
        /// <param name="i"></param>
        internal virtual void SetExplodeIndex(int i)
        {
        }

        #endregion

        #region Internal Override Methods

        internal override void Dispose()
        {
            YValues = null;
            base.Dispose();
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AdornmentPresenter = new ChartDataMarkerPresenter();
            AdornmentPresenter.Series = this;

            if (Chart != null && AdornmentsInfo != null && ShowDataLabels)
            {
                Chart.DataLabelPresenter.Children.Add(AdornmentPresenter);
                AdornmentsInfo.PanelChanged(AdornmentPresenter);
            }
        }

        internal override void UpdateOnSeriesBoundChanged(Size size)
        {
            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                AdornmentsInfo.UpdateElements();
            }

            base.UpdateOnSeriesBoundChanged(size);

            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                AdornmentPresenter.Update(size);
                AdornmentPresenter.Arrange(size);
            }
        }

        /// <summary>
        /// Called when ItemsSource property changed.
        /// </summary>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            base.OnDataSourceChanged(oldValue, newValue);

            if (AdornmentsInfo != null)
            {
                VisibleAdornments.Clear();
                Adornments.Clear();
                AdornmentsInfo.UpdateElements();
            }

            if (YValues != null)
                YValues.Clear();
            if (Segments != null)
                Segments.Clear();
            GeneratePoints(new[] { YBindingPath }, YValues);
            if (this.Chart != null && Chart.PlotArea != null)
                this.Chart.PlotArea.ShouldPopulateLegendItems = true;
            ScheduleUpdateChart();
        }

        /// <summary>
        /// Called when binding path changed.
        /// </summary>
        internal override void OnBindingPathChanged()
        {
            YValues.Clear();
            ResetData();
            GeneratePoints(new[] { YBindingPath }, YValues);
            if (this.Chart != null && this.Chart.PlotArea != null)
                this.Chart.PlotArea.ShouldPopulateLegendItems = true;
            base.OnBindingPathChanged();
        }

        internal override void SelectedSegmentsIndexes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ChartSegment? selectedSegment = null, oldSegment = null;
            if (SelectionBehavior == null) return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null && SelectionBehavior != null && SelectionBehavior.EnableMultiSelection)
                    {
                        int oldIndex = PreviousSelectedIndex;
                        int newIndex = (int)e.NewItems[0];

                        if (oldIndex != -1 && oldIndex < ActualData?.Count)
                        {
                            oldSegment = Segments.Where(segment => segment.Item == ActualData[oldIndex]).FirstOrDefault();
                        }

                        if (newIndex >= 0 && GetEnableSegmentSelection())
                        {
                            if (Segments.Count == 0)
                            {
                                triggerSelectionChangedEventOnLoad = true;
                                return;
                            }

                            if (newIndex < Segments.Count || newIndex < ActualData?.Count)
                            {
                                // For adornment selection implementation
                                if (adornmentInfo is ChartDataLabelSettings && adornmentInfo.HighlightOnSelection)
                                    UpdateAdornmentSelection(newIndex);

                                // Set the SegmentSelectionBrush to newIndex segment Interior
                                if (ActualData != null)
                                {
                                    selectedSegment = Segments.Where(segment => segment.Item == ActualData[newIndex]).FirstOrDefault();
                                    if (selectedSegment != null)
                                    {
                                        if (SelectionBehavior.SelectionBrush != null)
                                            selectedSegment.BindProperties();
                                        selectedSegment.IsSelectedSegment = true;
                                    }
                                }
                            }

                            if (newIndex < Segments.Count || newIndex < ActualData?.Count)
                            {
                                OnSelectionChanged(newIndex, oldIndex);
                                PreviousSelectedIndex = newIndex;
                            }
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:

                    if (e.OldItems != null && SelectionBehavior.EnableMultiSelection)
                    {
                        int newIndex = (int)e.OldItems[0];

                        if (PreviousSelectedIndex != -1 && PreviousSelectedIndex < ActualData?.Count)
                        {
                            oldSegment = Segments.Where(segment => segment.Item == ActualData[PreviousSelectedIndex]).FirstOrDefault();
                        }

                        OnSelectionChanged(newIndex, PreviousSelectedIndex);
                        PreviousSelectedIndex = newIndex;
                        OnResetSegment(newIndex);
                    }

                    break;
            }
        }

        internal override void OnResetSegment(int index)
        {
            if (index >= 0 && ActualData !=null)
            {
                var resetSegment = Segments.Where(segment => segment.Item == ActualData[index]).FirstOrDefault();
                if (resetSegment != null)
                {
                    resetSegment.BindProperties();
                    resetSegment.IsSelectedSegment = false;
                }

                if (adornmentInfo is ChartDataLabelSettings)
                    AdornmentPresenter.ResetAdornmentSelection(index, false);
            }
        }

        /// <summary>
        /// This method used to get the chart data at an index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal override ChartDataPointInfo? GetDataPoint(int index)
        {
            IList<double> xValues = ActualXValues as IList<double> ?? GetXValues();
            dataPoint = null;
            if (index < xValues.Count)
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

        internal override int GetDataPointIndex(Point point)
        {
            Canvas canvas = Chart.GetAdorningCanvas();
            double left = Chart.ActualWidth - canvas.ActualWidth;
            double top = Chart.ActualHeight - canvas.ActualHeight;
            point.X = point.X - left + Chart.Margin.Left;
            point.Y = point.Y - top + Chart.Margin.Top;

            foreach (var segment in Segments)
            {
                Path? path = segment.GetRenderedVisual() as Path;
                bool isHit = false;
                var pathData = path?.Data as PathGeometry;
                if (pathData != null)
                    isHit = pathData.Bounds.Contains(point);
                if (isHit)
                    return Segments.IndexOf(segment);
            }

            return -1;
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            Point newPosition = new Point();

            if (Chart == null || Chart.RootPanelDesiredSize == null)
                return newPosition;

            var actualwidth = Chart.RootPanelDesiredSize.Value.Width;
            var actualHeight = Chart.RootPanelDesiredSize.Value.Height;

            newPosition.X = actualwidth / 2;

            if (tooltip.DataContext is FunnelSegment funnelSegment)
            {
                newPosition.Y = (funnelSegment.top * actualHeight) + funnelSegment.height;
            }
            else if (tooltip.DataContext is PyramidSegment pyramidSegment)
            {
                newPosition.Y = (pyramidSegment.y * actualHeight) + pyramidSegment.height;
            }

            return newPosition;
        }

        /// <summary>
        /// Method used to set SegmentSelectionBrush to selectedindex chartsegment.
        /// </summary>
        /// <param name="newIndex">new index</param>
        /// <param name="oldIndex">old index</param>
        internal override void SelectedIndexChanged(int newIndex, int oldIndex)
        {
            if (ActualArea != null && SelectionBehavior != null)
            {
                ChartSegment? selectedSegment = null, oldSegment = null;

                // Reset the oldIndex segment Interior
                if (!SelectionBehavior.EnableMultiSelection)
                {
                    if (SelectedSegmentsIndexes.Contains(oldIndex))
                        SelectedSegmentsIndexes.Remove(oldIndex);

                    OnResetSegment(oldIndex);
                }

                if (IsItemsSourceChanged)
                {
                    return;
                }

                if (oldIndex != -1 && oldIndex < ActualData?.Count)
                {
                    oldSegment = Segments.Where(segment => segment.Item == ActualData[oldIndex]).FirstOrDefault();
                }

                if (newIndex >= 0 && GetEnableSegmentSelection())
                {
                    if (!SelectedSegmentsIndexes.Contains(newIndex))
                        SelectedSegmentsIndexes.Add(newIndex);
                    if (Segments.Count == 0)
                    {
                        triggerSelectionChangedEventOnLoad = true;
                        return;
                    }

                    if (newIndex < Segments.Count || newIndex < ActualData?.Count)
                    {
                        // For adornment selection implementation
                        if (adornmentInfo is ChartDataLabelSettings && adornmentInfo.HighlightOnSelection)
                            UpdateAdornmentSelection(newIndex);

                        // Set the SegmentSelectionBrush to newIndex segment Interior
                        if (ActualData != null)
                        {
                            selectedSegment = Segments.Where(segment => segment.Item == ActualData[newIndex]).FirstOrDefault();
                            if (selectedSegment != null)
                            {
                                if (SelectionBehavior.SelectionBrush != null)
                                    selectedSegment.BindProperties();

                                selectedSegment.IsSelectedSegment = true;
                            }
                        }
                    }

                    if (newIndex < Segments.Count || newIndex < ActualData?.Count)
                    {
                        OnSelectionChanged(newIndex, oldIndex);
                        PreviousSelectedIndex = newIndex;
                    }
                }
                else if (newIndex == -1)
                {
                    OnSelectionChanged(newIndex, oldIndex);
                    PreviousSelectedIndex = newIndex;
                }
            }
        }

        /// <summary>
        /// Method used to generate points for accumulation series.
        /// </summary>
        internal override void GenerateDataPoints()
        {
            GeneratePoints(new[] { YBindingPath }, YValues);
        }

        /// <summary>
        /// Called when the chart mouse up.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="position">position</param>
        internal override void OnSeriesMouseUp(object source, Point position)
        {
            var element = source as FrameworkElement;
            var segment = element != null ? element.Tag as ChartSegment : null;
            int index = -1;
            if (ExplodeOnTap && allowExplode && mouseUnderSegment == segment)
            {
                if (segment != null && segment.Series is TriangularSeriesBase && ActualData != null)
                    index = ActualData.IndexOf(segment.Item);
                else if (Adornments.Count > 0)
                    index = ChartExtensionUtils.GetAdornmentIndex(source);
                var newIndex = index;
                var oldIndex = ExplodeIndex;
                if (newIndex != oldIndex)
                    ExplodeIndex = newIndex;
                else if (ExplodeIndex >= 0)
                    ExplodeIndex = -1;
                allowExplode = false;
            }
        }

        /// <summary>
        /// Called when the chart mouse down.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="position">position</param>
        internal override void OnSeriesMouseDown(object source, Point position)
        {
            if (GetAnimationIsActive()) return;

            allowExplode = true;
            if (source is FrameworkElement element && element.Tag is ChartSegment chartSegment)
                mouseUnderSegment = chartSegment;
        }

        #endregion

        #region Private Static Methods

        private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is TriangularSeriesBase series)
                series.OnBindingPathChanged();
        }

        private static void OnExplodeIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as TriangularSeriesBase;
            if (series != null)
                series.SetExplodeIndex((int)e.NewValue);
        }

        private static void OnGapRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var triangularSeriesBase = d as TriangularSeriesBase;
            if (triangularSeriesBase != null && triangularSeriesBase.Chart != null)
                triangularSeriesBase.Chart.ScheduleUpdate();
        }

        private static void OnExplodeOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var triangularSeriesBase = d as TriangularSeriesBase;
            if (triangularSeriesBase != null && triangularSeriesBase.Chart != null)
            {
                triangularSeriesBase.Chart.ScheduleUpdate();
            }
        }
        #endregion

        #endregion
    }
}
