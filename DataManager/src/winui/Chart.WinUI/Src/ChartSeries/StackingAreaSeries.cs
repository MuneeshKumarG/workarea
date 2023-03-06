using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="StackedAreaSeries"/> is a special kind of area series which is similar to regular area series except that the Y-values stack on top of each other.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="StackedAreaSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="Stroke"/>, <see cref="XyDataSeries.StrokeThickness"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="StackedAreaSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="StackedAreaSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
    /// <para> <b>Selection - </b> To enable the data point selection in a chart, create an instance of <see cref="DataPointSelectionBehavior"/> and set it to the <see cref="StackedAreaSeries.SelectionBehavior"/> property of the chart series. To highlight the selected segment data label, set the value for <see cref="ChartSelectionBehavior.SelectionBrush"/> property in <see cref="DataPointSelectionBehavior"/>.</para>
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
    ///           <chart:StackedAreaSeries ItemsSource="{Binding Data}"
    ///                                    XBindingPath="XValue"
    ///                                    YBindingPath="YValue"/>
    ///                   
    ///           <chart:StackedAreaSeries ItemsSource="{Binding Data}"
    ///                                    XBindingPath="XValue"
    ///                                    YBindingPath="YValue1"/> 
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
    ///     StackedAreaSeries series = new StackedAreaSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    /// 
    ///     StackedAreaSeries series1 = new StackedAreaSeries();
    ///     series1.ItemsSource = viewModel.Data;
    ///     series1.XBindingPath = "XValue";
    ///     series1.YBindingPath = "YValue1";
    ///     chart.Series.Add(series1); 
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
    ///        Data.Add(new Model() { XValue = 10, YValue = 100, YValue1 = 110 });
    ///        Data.Add(new Model() { XValue = 20, YValue = 150, YValue1 = 100 });
    ///        Data.Add(new Model() { XValue = 30, YValue = 110, YValue1 = 130 });
    ///        Data.Add(new Model() { XValue = 40, YValue = 230, YValue1 = 180 });
    ///     }
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    /// <seealso cref="StackedAreaSegment"/>
    public class StackedAreaSeries : StackedSeriesBase
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the IsClosed dependency property.
        /// </summary>
        /// <value>
        /// The identifier for IsClosed dependency property.
        /// </value> 
        public static readonly DependencyProperty IsClosedProperty =
            DependencyProperty.Register(
                nameof(IsClosed),
                typeof(bool), 
                typeof(StackedAreaSeries),
                new PropertyMetadata(true, OnPropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(StackedAreaSeries),
            new PropertyMetadata(null, OnStrokeChanged));

        #endregion

        #region Fields

        #region Private Fields

        Storyboard sb;

        Point hitPoint = new Point();

        private RectAnimation animation;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the area path's stroke should be closed or open.
        /// </summary>
        /// <value>If its <c>true</c>, the area path will be closed by stroke; otherwise, it will be opened.</value>
        /// <remarks>To highlight the close path, set the Stroke and StrokeWidth properties in the series class.</remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///          <!-- ... Eliminated for simplicity-->
        ///          <chart:StackedAreaSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   YBindingPath="YValue"
        ///                                   IsClosed = "True"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///    // Eliminated for simplicity
        ///
        ///     StackedAreaSeries series = new StackedAreaSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.IsClosed = true;
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }

        /// Gets or sets a value to customize the stroke appearance of a chart series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:StackedAreaSeries ItemsSource="{Binding Data}"
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
        ///     StackedAreaSeries StackedAreaSeries1 = new StackedAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth= 3,
        ///     };
        ///     
        ///     chart.Series.Add(stackedAreaSeries1);
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

        #region Protected Override Properties

        /// <inheritdoc/>
        internal override bool IsStacked
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Private Properties

        private StackedAreaSegment Segment { get; set; }
        
        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of <see cref="StackedAreaSeries"/>.
        /// </summary>
        internal override void GenerateSegments()
        {
            ClearUnUsedAdornments(this.PointsCount);
            List<double> xValues = new List<double>();
            List<double> drawingListXValues = new List<double>();
            List<double> drawingListYValues = new List<double>();
            double Origin = 0d;
            if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
            {
                Adornments.Clear();
                xValues = GroupedXValuesIndexes;
            }
            else
                xValues = GetXValues();

            var stackingValues = GetCumulativeStackValues(this);
            if (stackingValues != null)
            {
                YRangeStartValues = stackingValues.StartValues;
                YRangeEndValues = stackingValues.EndValues;

                if (YRangeStartValues != null)
                {
                    drawingListXValues.AddRange(xValues);
                    drawingListYValues.AddRange(YRangeStartValues);
                }
                else
                {
                    drawingListXValues.AddRange(xValues);
                    drawingListYValues = (from val in xValues select Origin).ToList();
                }

                drawingListXValues.AddRange((from val in xValues select val).Reverse().ToList());
                drawingListYValues.AddRange((from val in YRangeEndValues select val).Reverse().ToList());

                if (Segment == null || Segments.Count == 0)
                {
                    Segment = CreateSegment() as StackedAreaSegment;
                    if (Segment != null)
                    {
                        Segment.Series = this;
                        Segment.Item = ActualData;
                        Segment.SetData(drawingListXValues, drawingListYValues);
                        Segments.Add(Segment);
                    }
                }
                else
                {
                    Segment.Item = ActualData;
                    Segment.SetData(drawingListXValues, drawingListYValues);
                }

                if (AdornmentsInfo != null && ShowDataLabels)
                    AddStackingAreaAdornments(YRangeEndValues);
            }
        }

        #endregion

        #region Internal Override Methods

        internal override void SelectedSegmentsIndexes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null && SelectionBehavior != null && SelectionBehavior.EnableMultiSelection) 
                    {
                        int oldIndex = PreviousSelectedIndex;

                        int newIndex = (int)e.NewItems[0];

                        if (newIndex >= 0 && GetEnableSegmentSelection())
                        {
                            // Selects the adornment when the mouse is over or clicked on adornments(adornment selection).
                            if (adornmentInfo != null && adornmentInfo.HighlightOnSelection)
                            {
                                UpdateAdornmentSelection(newIndex);
                            }

                            if (ActualArea != null && Segments.Count != 0)
                            {
                                OnSelectionChanged(newIndex, oldIndex);
                                PreviousSelectedIndex = newIndex;
                            }
                            else
                            {
                                triggerSelectionChangedEventOnLoad = true;
                            }
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:

                    if (e.OldItems != null && SelectionBehavior != null && SelectionBehavior.EnableMultiSelection) 
                    {
                        int newIndex = (int)e.OldItems[0];

                        OnSelectionChanged(newIndex, PreviousSelectedIndex);
                        OnResetSegment(newIndex);
                        PreviousSelectedIndex = newIndex;
                    }

                    break;
            }
        }

        internal override void OnResetSegment(int index)
        {
            if (index >= 0 && adornmentInfo != null)
            {
                AdornmentPresenter.ResetAdornmentSelection(index, false);
            }
        }

        /// <summary>
        /// This method used to gets the chart data point at a position.
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        internal override ChartDataPointInfo GetDataPoint(Point mousePos)
        {
            Rect rect;
            int startIndex, endIndex;
            List<int> hitIndexes = new List<int>();
            IList<double> xValues = (ActualXValues is IList<double>) ? ActualXValues as IList<double> : GetXValues();

            CalculateHittestRect(mousePos, out startIndex, out endIndex, out rect);

            for (int i = startIndex; i < endIndex; i++)
            {
                if (i >= YValues.Count) break;
                hitPoint.X = IsIndexed ? i : xValues[i];
                hitPoint.Y = YValues[i];

                if (rect.Contains(hitPoint))
                    hitIndexes.Add(i);
            }

            if (hitIndexes.Count > 0)
            {
                int i = hitIndexes[hitIndexes.Count / 2];
                hitIndexes = null;

                dataPoint = new ChartDataPointInfo();
                dataPoint.Index = i;
                dataPoint.XData = xValues[i];
                dataPoint.YData = YValues[i];
                dataPoint.Series = this;

                if (ActualData.Count > i)
                    dataPoint.Item = ActualData[i];

                return dataPoint;
            }
            else
                return dataPoint;
        }

        internal override bool GetAnimationIsActive()
        {
            return animation != null && animation.IsActive;
        }

        internal override void Animate()
        {
            // WPF-25124 Animation not working properly when resize the window.
            if (animation != null)
            {
                animation.Stop();

                if (!canAnimate)
                {
                    ResetAdornmentAnimationState();
                    return;
                }
            }

            var seriesRect = Chart.SeriesClipRect;
            animation = new RectAnimation()
            {
                From = (IsActualTransposed) ? new Rect(0, seriesRect.Bottom, seriesRect.Width, seriesRect.Height) : new Rect(0, seriesRect.Y, 0, seriesRect.Height),
                To = (IsActualTransposed) ? new Rect(0, seriesRect.Y, 0, seriesRect.Height) : new Rect(0, seriesRect.Y, seriesRect.Width, seriesRect.Height),
                Duration = AnimationDuration.TotalSeconds == 1 ? TimeSpan.FromSeconds(0.4) : AnimationDuration
            };

            animation.SetTarget(SeriesRootPanel);
            animation.Begin();

            if (this.AdornmentsInfo != null && ShowDataLabels)
            {
                var adornTransXPath = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)";
                var adornTransYPath = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)";
                
                sb = new Storyboard();
                double secondsPerPoint = (AnimationDuration.TotalSeconds / YValues.Count);
                secondsPerPoint *= 2;
                int i = 0;

                foreach (FrameworkElement label in this.AdornmentsInfo.LabelPresenters)
                {
                    var transformGroup = label.RenderTransform as TransformGroup;
                    var scaleTransform = new ScaleTransform() { ScaleX = 0, ScaleY = 0 };

                    if (transformGroup != null)
                    {
                        if (transformGroup.Children.Count > 0 && transformGroup.Children[0] is ScaleTransform)
                        {
                            transformGroup.Children[0] = scaleTransform;
                        }
                        else
                        {
                            transformGroup.Children.Insert(0, scaleTransform);
                        }
                    }

                    label.RenderTransformOrigin = new Point(0.5, 0.5);
                    DoubleAnimation keyFrames1 = new DoubleAnimation()
                    {
                        From = 0.3,
                        To = 1,
                        BeginTime = TimeSpan.FromSeconds(i * secondsPerPoint)
                    };

                    keyFrames1.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(AnimationDuration.TotalSeconds / 2));

                    keyFrames1.EnableDependentAnimation = true;
                    Storyboard.SetTargetProperty(keyFrames1, adornTransXPath);
                    Storyboard.SetTarget(keyFrames1, label);
                    sb.Children.Add(keyFrames1);
                    keyFrames1 = new DoubleAnimation()
                    {
                        From = 0.3,
                        To = 1,
                        BeginTime = TimeSpan.FromSeconds(i * secondsPerPoint)
                    };
                   keyFrames1.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(AnimationDuration.TotalSeconds / 2));
                    keyFrames1.EnableDependentAnimation = true;
                    Storyboard.SetTargetProperty(keyFrames1, adornTransYPath);
                    Storyboard.SetTarget(keyFrames1, label);
                    sb.Children.Add(keyFrames1);
                    i++;
                }

                sb.Begin();
            }
        }

        internal override void UpdateTooltip(object originalSource)
        {
            if (EnableTooltip)
            {
                FrameworkElement element = originalSource as FrameworkElement;
                object chartSegment = null;
                int index = -1;
                if (element != null)
                {
                    if (element.Tag is ChartSegment)
                        chartSegment = element.Tag;
                    else if (Segments.Count > 0)
                        chartSegment = Segments[0];
                    else
                    {
                        // WPF-28526- Tooltip not shown when set the single data point with adornments for continuous series.
                        index = ChartExtensionUtils.GetAdornmentIndex(element);
                        chartSegment = index != -1 ? new StackedAreaSegment() : null;
                    }
                }

                var segment = chartSegment as ChartSegment;

                SetTooltipDuration();
                var canvas = this.Chart.GetAdorningCanvas();
                double xVal = 0;
                double yVal = 0;
                double stackedYValue = double.NaN;
                object data = null;
                index = 0;

                if (this.Chart.SeriesClipRect.Contains(mousePosition))
                {
                    var point = new Point(
                        mousePosition.X - this.Chart.SeriesClipRect.Left,
                        mousePosition.Y - this.Chart.SeriesClipRect.Top);

                    this.FindNearestChartPoint(point, out xVal, out yVal, out stackedYValue);
                    if (double.IsNaN(xVal)) return;
                    if (ActualXAxis is CategoryAxis && (ActualXAxis as CategoryAxis).IsIndexed)
                        index = YValues.IndexOf(yVal);
                    else
                        index = this.GetXValues().IndexOf(xVal);

                    var seriesCollection = Chart.GetSeriesCollection();
                    foreach (ChartSeries series in seriesCollection)
                    {
                        if (series == this && index >= 0)
                            data = this.ActualData[index];
                    }
                }

                if (data == null) return; // To ignore tooltip when mousePos is not inside the SeriesClipRect.
                if (this.Chart.Tooltip == null)
                    this.Chart.Tooltip = new ChartTooltip();
                var chartTooltip = this.Chart.Tooltip as ChartTooltip;

                var stackingAreaSegment = chartSegment as StackedAreaSegment;
                stackingAreaSegment.Item = data;
                stackingAreaSegment.XData = xVal;
                stackingAreaSegment.YData = yVal;

                if (chartTooltip != null)
                {
                    ToolTipTag = chartSegment;
                    chartTooltip.PolygonPath = " ";

                    if (canvas.Children.Count == 0 || (canvas.Children.Count > 0 && !IsTooltipAvailable(canvas)))
                    {
                        chartTooltip.DataContext = chartSegment;
                        if (chartTooltip.DataContext == null)
                        {
                            RemoveTooltip();
                            return;
                        }

                        if (ChartTooltip.GetActualInitialShowDelay(ActualArea.TooltipBehavior, ChartTooltip.GetInitialShowDelay(this)) == 0)
                        {
                            canvas.Children.Add(chartTooltip);
                        }

                        chartTooltip.ContentTemplate = this.GetTooltipTemplate();
                        AddTooltip();

                        if (ChartTooltip.GetActualEnableAnimation(ActualArea.TooltipBehavior, ChartTooltip.GetEnableAnimation(this)))
                        {
                            SetDoubleAnimation(chartTooltip);
                            Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                            Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                        }
                        else
                        {
                            Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                            Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                        }
                    }
                    else
                    {
                        foreach (var child in canvas.Children)
                        {
                            var tooltip = child as ChartTooltip;
                            if (tooltip != null)
                                chartTooltip = tooltip;
                        }

                        chartTooltip.DataContext = chartSegment;

                        if (chartTooltip.DataContext == null)
                        {
                            RemoveTooltip();
                            return;
                        }

                        AddTooltip();

                        Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                        Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                    }
                }
            }
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            StackedAreaSegment stackingAreaSegment = ToolTipTag as StackedAreaSegment;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(stackingAreaSegment.XData, YRangeEndValues[ActualData.IndexOf(stackingAreaSegment.Item)]);
            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top;
            return newPosition;
        }

        internal override void Dispose()
        {
            if (animation != null)
            {
                animation.Stop();
                animation = null;
            }

            if (sb != null)
            {
                sb.Stop();
                sb.Children.Clear();
                sb = null;
            }
            base.Dispose();
        }

        #endregion

        #region Protected Internal Override Methods

        /// <summary>
        /// Method used to set SelectionBrush to SelectedIndex segment.
        /// </summary>
        /// <param name="newIndex">Used to specify the new selected index.</param>
        /// <param name="oldIndex">Used to specify the old selected index.</param>
        internal override void SelectedIndexChanged(int newIndex, int oldIndex)
        {
            if (ActualArea != null && SelectionBehavior != null)
            {
                // Resets the adornment selection when the mouse pointer moved away from the adornment or clicked the same adornment.
                if (SelectionBehavior != null && !SelectionBehavior.EnableMultiSelection) 
                {
                    if (SelectedSegmentsIndexes.Contains(oldIndex))
                        SelectedSegmentsIndexes.Remove(oldIndex);

                    OnResetSegment(oldIndex);
                }

                if (IsItemsSourceChanged)
                {
                    return;
                }

                if (newIndex >= 0 && GetEnableSegmentSelection())
                {
                    if (!SelectedSegmentsIndexes.Contains(newIndex))
                        SelectedSegmentsIndexes.Add(newIndex);

                    // Selects the adornment when the mouse is over or clicked on adornments(adornment selection).
                    if (adornmentInfo != null && adornmentInfo.HighlightOnSelection)
                    {
                        UpdateAdornmentSelection(newIndex);
                    }

                    if (ActualArea != null && Segments.Count != 0)
                    {
                        OnSelectionChanged(newIndex, oldIndex);
                        PreviousSelectedIndex = newIndex;
                    }
                    else
                    {
                        triggerSelectionChangedEventOnLoad = true;
                    }
                }
                else if (newIndex == -1 && ActualArea != null)
                {
                    OnSelectionChanged(newIndex, oldIndex);
                    PreviousSelectedIndex = newIndex;
                }
            }
            else if (newIndex >= 0 && Segments.Count == 0)
            {
                triggerSelectionChangedEventOnLoad = true;
            }
        }

        #endregion

        #region Protected Override 

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            return new StackedAreaSegment();
        }

        /// <summary>
        /// Invoked when ItemsSource property changed.
        /// </summary>
        /// <param name="oldValue">Old itemssource collection.</param>
        /// <param name="newValue">New itemssource collection.</param>
        /// <seealso cref="ChartSeries.XBindingPath"/>
        /// <seealso cref="XyDataSeries.YBindingPath"/>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            Segment = null;
            base.OnDataSourceChanged(oldValue, newValue);
        }

#if NETFX_CORE
        /// <inheritdoc/>
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Touch)
                UpdateTooltip(e.OriginalSource);
        }
#endif

        #endregion

        #region Private Static Methods

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as StackedAreaSeries).ScheduleUpdateChart();
        }

        #endregion

        #region Private Methods

        private void AddStackingAreaAdornments(IList<double> yValues)
        {
            double adornX = 0d, adornY = 0d;
            int i = 0;
            List<double> xValues = null;
            IList<double> actualYValues = YValues;

            if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
            {
                xValues = GroupedXValuesIndexes;
                actualYValues = GroupedSeriesYValues[0];
            }
            else
                xValues = GetXValues();

            for (i = 0; i < xValues.Count; i++)
            {
                adornX = xValues[i];
                adornY = yValues[i];

                if (i < Adornments.Count)
                {
                    Adornments[i].SetData(adornX, actualYValues[i], adornX, adornY);
                }
                else
                {
                    Adornments.Add(this.CreateAdornment(this, adornX, actualYValues[i], adornX, adornY));
                }

                Adornments[i].Item = ActualData[i];
            }
        }

        #endregion

        #endregion
    }
}
