using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using AdornmentSeries = Syncfusion.UI.Xaml.Charts.DataMarkerSeries;
using ChartAdornmentInfo = Syncfusion.UI.Xaml.Charts.ChartDataLabelSettings;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a base class for all the triangular and circular series. This class has options to customize the appearance of triangular and circular series.
    /// </summary>
    public abstract class AccumulationSeriesBase : AdornmentSeries
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="YBindingPath"/> property.
        /// </summary>
        public static readonly DependencyProperty YBindingPathProperty =
            DependencyProperty.Register(nameof(YBindingPath), typeof(string), typeof(AccumulationSeriesBase),
            new PropertyMetadata(null, OnYPathChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ExplodeIndex"/> property.
        /// </summary>
        public static readonly DependencyProperty ExplodeIndexProperty =
            DependencyProperty.Register(nameof(ExplodeIndex), typeof(int), typeof(AccumulationSeriesBase),
            new PropertyMetadata(-1, OnExplodeIndexChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ExplodeAll"/> property.
        /// </summary>
        public static readonly DependencyProperty ExplodeAllProperty =
            DependencyProperty.Register(nameof(ExplodeAll), typeof(bool), typeof(AccumulationSeriesBase),
            new PropertyMetadata(false, OnExplodeAllChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ExplodeOnTap"/> property.
        /// </summary>
        public static readonly DependencyProperty ExplodeOnTapProperty =
            DependencyProperty.Register(nameof(ExplodeOnTap), typeof(bool), typeof(AccumulationSeriesBase),
            new PropertyMetadata(false));

        /// <summary>
        /// The DependencyProperty for <see cref="SelectionBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(AccumulationSeriesBase),
            new PropertyMetadata(null, OnSegmentSelectionBrush));

        /// <summary>
        /// The DependencyProperty for <see cref="SelectedIndex"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(AccumulationSeriesBase),
            new PropertyMetadata(-1, OnSelectedIndexChanged));

        #endregion

        #region Fields

        #region Private Fields


        private bool allowExplode;

        private ChartSegment mouseUnderSegment;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Called when instance created for AccumulationSeriesBase
        /// </summary>
        public AccumulationSeriesBase()
        {
            YValues = new List<double>();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the property name that associates the secondary axis with a property in the itemssource.
        /// </summary>
        /// <value>
        /// The string that represents the property name for secondary axis. The default value is null.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfChart>
        ///
        ///          <chart:SfChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfChart.DataContext>
        ///
        ///           <chart:PieSeries
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
        ///     ViewModel viewmodel = new ViewModel();
        ///     
        ///     PieSeries series = new PieSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        /// ]]></code>
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
        /// Gets or sets a value indicating whether to explode all the pie slices (segments).
        /// </summary>
        /// <value>
        ///     <c>True</c>, will explode all the segments.
        /// </value>
        public bool ExplodeAll
        {
            get { return (bool)GetValue(ExplodeAllProperty); }
            set { SetValue(ExplodeAllProperty, value); }
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
        /// Gets or sets the interior (brush) for the selected segment(s).
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the selected segment.
        /// </summary>
        /// <value>
        /// <c>Int</c> value represents the index of the data point(or segment) to be selected and its default value is -1.
        /// </value>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Returns the corresponding series selected segment.
        /// </summary>
        protected internal override ChartSegment SelectedSegment
        {
            get
            {
                int index = SelectedIndex;
                if (this is ISegmentSelectable && index >= 0 && index < ActualData.Count)
                {
                    var circularSeries = this as CircularSeries;
                    object item = circularSeries != null && !double.IsNaN(circularSeries.GroupTo) ? Segments[index].Item: ActualData[index];
                    var segment = Segments.FirstOrDefault(segments => segments.Item == item);
                    return segment;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets the selected segments in this series, when we enable the multiple selection.
        /// </summary>
        /// <returns>
        /// It returns list of <see cref="ChartSegment"/>.
        /// </returns>
        protected internal override List<ChartSegment> SelectedSegments
        {
            get
            {
                if (SelectedSegmentsIndexes.Count > 0)
                {
                    List<ChartSegment> chartCollection = new List<ChartSegment>();

                    foreach (int SelectedIndexes in SelectedSegmentsIndexes)
                    {
                        var circularSeries = this as CircularSeries;
                        object item = circularSeries != null && !double.IsNaN(circularSeries.GroupTo) ? Segments[SelectedIndexes].Item : ActualData[SelectedIndexes];
                        var segments = Segments.Where(segment => segment.Item == item).FirstOrDefault();
                        if (segments != null)
                            chartCollection.Add(segments);
                    }

                    return chartCollection;
                }
                else
                    return null;
            }
        }

        #endregion

        #region Protected Internal Properties        

        /// <summary>
        /// Gets or sets the Y values collection binded with this series.
        /// </summary>
        protected internal IList<double> YValues { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Internal Override Methods

        internal override void SelectedSegmentsIndexes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ChartSelectionChangedEventArgs chartSelectionChangedEventArgs;
            ChartSegment selectedSegment = null, oldSegment = null;
            CircularSeries circularseries = this as CircularSeries;
            bool isGroupTo = circularseries != null && !double.IsNaN(circularseries.GroupTo);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null && ActualArea.SelectionBehaviour.EnableMultiSelection)
                    {
                        int oldIndex = PreviousSelectedIndex;
                        int newIndex = (int)e.NewItems[0];

                        if (oldIndex != -1 && oldIndex < ActualData.Count)
                        {
                            object oldItem = isGroupTo ? Segments[oldIndex].Item : ActualData[oldIndex];
                            oldSegment = Segments.Where(segment => segment.Item == oldItem).FirstOrDefault();
                        }

                        if (newIndex >= 0 && ActualArea.GetEnableSegmentSelection())
                        {
                            if (Segments.Count == 0)
                            {
                                triggerSelectionChangedEventOnLoad = true;
                                return;
                            }

                            if (newIndex < Segments.Count || newIndex < ActualData.Count)
                            {
                                // For adornment selection implementation
                                if (adornmentInfo is ChartAdornmentInfo && adornmentInfo.HighlightOnSelection)
                                    UpdateAdornmentSelection(newIndex);

                                // Set the SegmentSelectionBrush to newIndex segment Interior
                                if (this is ISegmentSelectable)
                                {
                                    object newItem = isGroupTo ? Segments[newIndex].Item : ActualData[newIndex];
                                    selectedSegment = Segments.Where(segment => segment.Item == newItem).FirstOrDefault();
                                    if (selectedSegment != null)
                                    {
                                        if (this.SelectionBrush != null)
                                            selectedSegment.BindProperties();
                                        selectedSegment.IsSelectedSegment = true;
                                    }
                                }
                            }

                            if (newIndex < Segments.Count || newIndex < ActualData.Count)
                            {
                                chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                                {
                                    SelectedSegment = selectedSegment,
                                    SelectedSegments = Area.SelectedSegments,
                                    SelectedSeries = this,
                                    SelectedIndex = newIndex,
                                    PreviousSelectedIndex = oldIndex,
                                    PreviousSelectedSegment = oldSegment,
                                    NewPointInfo = selectedSegment != null ? selectedSegment.Item : null,
                                    OldPointInfo = oldSegment != null ? oldSegment.Item : null,
                                    PreviousSelectedSeries = null,
                                    IsSelected = true
                                };

                                if (this.ActualArea.PreviousSelectedSeries != null && oldIndex != -1)
                                {
                                    chartSelectionChangedEventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
                                }

                                (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
                                PreviousSelectedIndex = newIndex;
                            }
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:

                    if (e.OldItems != null && ActualArea.SelectionBehaviour.EnableMultiSelection)
                    {
                        int newIndex = (int)e.OldItems[0];

                        if (PreviousSelectedIndex != -1 && PreviousSelectedIndex < ActualData.Count)
                        {
                            object oldItem = isGroupTo ? Segments[PreviousSelectedIndex].Item : ActualData[PreviousSelectedIndex];
                            oldSegment = Segments.Where(segment => segment.Item == oldItem).FirstOrDefault();
                        }

                        chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                        {
                            SelectedSegment = null,
                            SelectedSegments = Area.SelectedSegments,
                            SelectedSeries = null,
                            SelectedIndex = newIndex,
                            PreviousSelectedIndex = PreviousSelectedIndex,
                            PreviousSelectedSegment = oldSegment,
                            NewPointInfo = null,
                            OldPointInfo = oldSegment.Item,
                            PreviousSelectedSeries = this,
                            IsSelected = false
                        };
                        (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
                        PreviousSelectedIndex = newIndex;
                        OnResetSegment(newIndex);
                    }

                    break;
            }
        }

        internal override void OnResetSegment(int index)
        {
            if (index >= 0)
            {
                var circularSeries = this as CircularSeries;
                object item = circularSeries != null && !double.IsNaN(circularSeries.GroupTo) ? Segments[index].Item : ActualData[index];
                var resetSegment = Segments.Where(segment => segment.Item == item).FirstOrDefault();
                if (resetSegment != null)
                {
                    resetSegment.BindProperties();
                    resetSegment.IsSelectedSegment = false;
                }

                if (adornmentInfo is ChartAdornmentInfo)
                    AdornmentPresenter.ResetAdornmentSelection(index, false);
            }
        }

        /// <summary>
        /// This method used to get the chart data at an index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal override ChartDataPointInfo GetDataPoint(int index)
        {
            IList<double> xValues = (ActualXValues is IList<double>) ? ActualXValues as IList<double> : GetXValues();
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

                if (ActualData.Count > index)
                    dataPoint.Item = ActualData[index];
            }

            return dataPoint;
        }

        internal override int GetDataPointIndex(Point point)
        {
            Canvas canvas = Area.GetAdorningCanvas();
            double left = Area.ActualWidth - canvas.ActualWidth;
            double top = Area.ActualHeight - canvas.ActualHeight;
            point.X = point.X - left + Area.Margin.Left;
            point.Y = point.Y - top + Area.Margin.Top;

            foreach (var segment in Segments)
            {
                Path path = segment.GetRenderedVisual() as Path;
                bool isHit = false;
                var pathData = path.Data as PathGeometry;
                if (pathData != null)
                    isHit = pathData.Bounds.Contains(point);
                if (isHit)
                    return Segments.IndexOf(segment);
            }

            return -1;
        }

        /// <summary>
        /// Validate the datapoints for segment implementation.
        /// </summary>
        internal override void ValidateYValues()
        {
            foreach (var yValue in YValues)
            {
                if (double.IsNaN(yValue) && ShowEmptyPoints)
                {
                    ValidateDataPoints(YValues); break;
                }
            }
        }

        /// <summary>
        /// Validate the datapoints for segment implementation.
        /// </summary>
        internal override void ReValidateYValues(List<int>[] emptyPointIndex)
        {
            foreach (var item in emptyPointIndex)
            {
                foreach (var index in item)
                    YValues[index] = double.NaN;
            }
        }

        #endregion

        #region Internal Methods

#if NETFX_CORE
        internal override void Dispose()
        {
            YValues = null;
            base.Dispose();
        }
#endif

        #endregion

        #region Protected Internal Override Methods

        /// <summary>
        /// Method used to set SegmentSelectionBrush to selectedindex chartsegment.
        /// </summary>
        /// <param name="newIndex">new index</param>
        /// <param name="oldIndex">old index</param>
        protected internal override void SelectedIndexChanged(int newIndex, int oldIndex)
        {
            CircularSeries circularseries = this as CircularSeries;
            bool isGroupTo = circularseries != null && !double.IsNaN(circularseries.GroupTo);
            ChartSelectionChangedEventArgs chartSelectionChangedEventArgs;
            if (ActualArea != null && ActualArea.SelectionBehaviour != null)
            {
                ChartSegment selectedSegment = null, oldSegment = null;

                // Reset the oldIndex segment Interior
                if (!ActualArea.SelectionBehaviour.EnableMultiSelection)
                {
                    if (SelectedSegmentsIndexes.Contains(oldIndex))
                        SelectedSegmentsIndexes.Remove(oldIndex);

                    OnResetSegment(oldIndex);
                }

                if (IsItemsSourceChanged)
                {
                    return;
                }

                if (oldIndex != -1 && oldIndex < ActualData.Count)
                {
                    object oldItem = isGroupTo ? Segments[oldIndex].Item : ActualData[oldIndex];
                    oldSegment = Segments.Where(segment => segment.Item == oldItem).FirstOrDefault();
                }

                if (newIndex >= 0 && ActualArea.GetEnableSegmentSelection())
                {
                    if (!SelectedSegmentsIndexes.Contains(newIndex))
                        SelectedSegmentsIndexes.Add(newIndex);
                    if (Segments.Count == 0)
                    {
                        triggerSelectionChangedEventOnLoad = true;
                        return;
                    }

                    if (newIndex < Segments.Count || newIndex < ActualData.Count)
                    {
                        // For adornment selection implementation
                        if (adornmentInfo is ChartAdornmentInfo && adornmentInfo.HighlightOnSelection)
                            UpdateAdornmentSelection(newIndex);

                        // Set the SegmentSelectionBrush to newIndex segment Interior
                        if (this is ISegmentSelectable)
                        {
                            object newItem = isGroupTo ? Segments[newIndex].Item : ActualData[newIndex];
                            selectedSegment = Segments.Where(segment => segment.Item == newItem).FirstOrDefault();
                            if (selectedSegment != null)
                            {
                                if (this.SelectionBrush != null)
                                    selectedSegment.BindProperties();
                                selectedSegment.IsSelectedSegment = true;
                            }
                        }
                    }

                    if (newIndex < Segments.Count || newIndex < ActualData.Count)
                    {
                        chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                        {
                            SelectedSegment = selectedSegment,
                            SelectedSegments = Area.SelectedSegments,
                            SelectedSeries = this,
                            SelectedIndex = newIndex,
                            PreviousSelectedIndex = oldIndex,
                            PreviousSelectedSegment = oldSegment,
                            NewPointInfo = selectedSegment != null ? selectedSegment.Item : null,
                            OldPointInfo = oldSegment != null ? oldSegment.Item : null,
                            PreviousSelectedSeries = null,
                            IsSelected = true
                        };

                        if (this.ActualArea.PreviousSelectedSeries != null && oldIndex != -1)
                        {
                            chartSelectionChangedEventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
                        }

                        (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
                        PreviousSelectedIndex = newIndex;
                    }
                }
                else if (newIndex == -1)
                {
                    chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                    {
                        SelectedSegment = null,
                        SelectedSegments = Area.SelectedSegments,
                        SelectedSeries = null,
                        SelectedIndex = newIndex,
                        PreviousSelectedIndex = oldIndex,
                        PreviousSelectedSegment = oldSegment,
                        NewPointInfo = null,
                        OldPointInfo = oldSegment.Item,
                        PreviousSelectedSeries = this,
                        IsSelected = false
                    };
                    (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
                    PreviousSelectedIndex = newIndex;
                }
            }
        }

        /// <summary>
        /// Method used to generate points for accumulation series.
        /// </summary>
        protected internal override void GeneratePoints()
        {
            GeneratePoints(new[] { YBindingPath }, YValues);
        }

        /// <summary>
        /// Called when the chart mouse up.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="position">position</param>
        protected internal override void OnSeriesMouseUp(object source, Point position)
        {
            var element = source as FrameworkElement;
            var segment = element != null ? element.Tag as ChartSegment : null;
            int index = -1;
            if (ExplodeOnTap && allowExplode && mouseUnderSegment == segment)
            {
                if (segment != null && segment.Series is AccumulationSeriesBase)
                    index = segment.Series is CircularSeries && !double.IsNaN(((CircularSeries)segment.Series).GroupTo) ? Segments.IndexOf(segment) : ActualData.IndexOf(segment.Item);
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
        protected internal override void OnSeriesMouseDown(object source, Point position)
        {
            if (GetAnimationIsActive()) return;

            allowExplode = true;
            var element = source as FrameworkElement;
            mouseUnderSegment = element.Tag as ChartSegment;
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Method implementation for ExplodeIndex.
        /// </summary>
        /// <param name="i"></param>
        internal virtual void SetExplodeIndex(int i)
        {
        }

        /// <summary>
        /// Virtual Method for ExplodeRadius.
        /// </summary>
        protected virtual void SetExplodeRadius()
        {
        }

        /// <summary>
        /// Virtual method for ExplodeAll.
        /// </summary>
        protected virtual void SetExplodeAll()
        {
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Called when ItemsSource property changed.
        /// </summary>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnDataSourceChanged(oldValue, newValue);
            if (YValues != null)
                YValues.Clear();
            if (Segments != null)
                Segments.Clear();
            GeneratePoints(new[] { YBindingPath }, YValues);
            isPointValidated = false;
            if (this.Area != null)
                this.Area.IsUpdateLegend = true;
            UpdateArea();
        }

        /// <summary>
        /// Called when binding path changed.
        /// </summary>
        /// <param name="args">args</param>
        protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
        {
            YValues.Clear();
            Segments.Clear();
            if (this.Area != null)
                this.Area.IsUpdateLegend = true;
            base.OnBindingPathChanged(args);
        }

        #endregion

        #region Private Static Methods

        private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AccumulationSeriesBase).OnBindingPathChanged(e);
        }

        private static void OnSegmentSelectionBrush(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as AccumulationSeriesBase).UpdateArea();
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartSeries series = d as ChartSeries;
            series.OnPropertyChanged("SelectedIndex");
            if (series.ActualArea == null || series.ActualArea.SelectionBehaviour == null) return;
            if (!(series.ActualArea as ChartBase).SelectionBehaviour.EnableMultiSelection)
                series.SelectedIndexChanged((int)e.NewValue, (int)e.OldValue);
            else if ((int)e.NewValue != -1)
                series.SelectedSegmentsIndexes.Add((int)e.NewValue);

            CircularSeries circularseries = series as CircularSeries;
            if (circularseries != null && !double.IsNaN(circularseries.GroupTo) && series.ActualArea.Legend != null)
                series.ActualArea.UpdateLegend(series.ActualArea.Legend, false);
        }

        private static void OnExplodeIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as AccumulationSeriesBase;
            if (series != null)
                series.SetExplodeIndex((int)e.NewValue);
        }

        private static void OnExplodeAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as CircularSeries;
            if (series != null)
                series.SetExplodeAll();
        }

        #endregion

        #endregion
    }
}
