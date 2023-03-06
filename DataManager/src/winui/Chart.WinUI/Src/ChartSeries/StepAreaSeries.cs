using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// The <see cref="StepAreaSeries"/> is similar to <see cref="AreaSeries"/> and it uses vertical and horizontal lines to connect the data points in a series forming a step-like progression.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="StepAreaSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XyDataSeries.StrokeWidth"/>, <see cref="ChartSeries.Stroke"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="StepAreaSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="StepAreaSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
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
    ///           <chart:StepAreaSeries ItemsSource="{Binding Data}"
    ///                             XBindingPath="XValue"
    ///                             YBindingPath="YValue"/>
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
    ///     StepAreaSeries series = new StepAreaSeries();
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
    /// <seealso cref="StepAreaSegment"/>
    public class StepAreaSeries : XyDataSeries
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="IsClosed"/> property.       .
        /// </summary>
        public static readonly DependencyProperty IsClosedProperty =
            DependencyProperty.Register(
                nameof(IsClosed),
                typeof(bool), 
                typeof(StepAreaSeries),
                new PropertyMetadata(true, OnPropertyChanged));


        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(StepAreaSeries),
            new PropertyMetadata(null, OnStrokeChanged));

        #endregion

        #region Fields

        Storyboard sb;

        Point hitPoint = new Point();

        private RectAnimation animation;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether area path should be closed or opened.
        /// </summary>
        /// <remarks>
        /// If its <c>true</c>, the area stroke will be closed; otherwise the stroke will be applied on top of the series only.
        /// </remarks>
        /// <value>It accepts bool values and the default value is <c>True</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///          <!-- ... Eliminated for simplicity-->
        ///          <chart:StepAreaSeries ItemsSource="{Binding Data}"
        ///                                XBindingPath="XValue"
        ///                                YBindingPath="YValue"
        ///                                IsClosed = "True"/>
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
        ///     StepAreaSeries series = new StepAreaSeries();
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
        ///          <chart:StepAreaSeries ItemsSource="{Binding Data}"
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
        ///     StepAreaSeries stepAreaSeries = new StepAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth= 3,
        ///     };
        ///     
        ///     chart.Series.Add(stepAreaSeries);
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

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of StepAreaSeries.
        /// </summary>       
        internal override void GenerateSegments()
        {
            List<double> xValues = null;
            bool isGrouping = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
            if (isGrouping)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();
            if (xValues.Count == 0) return;
            double Origin = 0;
            
            if (isGrouping)
            {
                Segments.Clear();
                Adornments.Clear();

                var stepAreaPoints = new List<ChartPoint>
                {
                new ChartPoint((xValues[xValues.Count - 1]), Origin),
                new ChartPoint(xValues[0], Origin)
                };

                for (int i = 0; i < xValues.Count; i++)
                {
                    stepAreaPoints.Add(new ChartPoint(xValues[i], GroupedSeriesYValues[0][i]));
                    if (i != xValues.Count - 1)
                    {
                        stepAreaPoints.Add(new ChartPoint(xValues[i + 1], GroupedSeriesYValues[0][i]));
                    }
                }

                if (Segments.Count == 0)
                {
                    StepAreaSegment segment = CreateSegment() as StepAreaSegment;
                    if (segment != null)
                    {
                        segment.Series = this;
                        segment.Item = ActualData;
                        segment.SetData(stepAreaPoints);
                        Segments.Add(segment);
                    }
                }

                if (AdornmentsInfo != null && ShowDataLabels)
                    AddAreaAdornments(GroupedSeriesYValues[0]);
            }
            else
            {
                ClearUnUsedAdornments(PointsCount);
                var stepAreaPoints = new List<ChartPoint>
                {
                new ChartPoint((xValues[PointsCount - 1]), Origin),
                new ChartPoint(xValues[0], Origin)
                };

                for (int i = 0; i < PointsCount; i++)
                {
                    stepAreaPoints.Add(new ChartPoint(xValues[i], YValues[i]));
                    if (i != PointsCount - 1)
                    {
                        stepAreaPoints.Add(new ChartPoint(xValues[i + 1], YValues[i]));
                    }
                }

                if (Segments.Count == 0)
                {
                    StepAreaSegment segment = CreateSegment() as StepAreaSegment;
                    if (segment != null)
                    {
                        segment.Series = this;
                        segment.Item = ActualData;
                        segment.SetData(stepAreaPoints);
                        Segments.Add(segment);
                    }
                }
                else
                {
                    Segments[0].Item = ActualData;
                    Segments[0].SetData(stepAreaPoints);
                }

                if (AdornmentsInfo != null && ShowDataLabels)
                    AddAreaAdornments(YValues);
            }
        }

        #endregion

        #region Internal Override Methods

        internal override void ResetAdornmentAnimationState()
        {
            if (adornmentInfo != null)
            {
                foreach (var child in this.AdornmentPresenter.Children)
                {
                    var frameworkELement = child as FrameworkElement;
                    frameworkELement.ClearValue(FrameworkElement.RenderTransformProperty);
                    frameworkELement.ClearValue(FrameworkElement.OpacityProperty);
                }
            }
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            StepAreaSegment stepAreaSegment = ToolTipTag as StepAreaSegment;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(stepAreaSegment.XData, stepAreaSegment.YData);
            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top;
            return newPosition;
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

                if (sb != null)
                    sb.Stop();

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
            AnimateAdornments();
        }

        internal override void SelectedSegmentsIndexes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null && SelectionBehavior != null && SelectionBehavior.EnableMultiSelection)
                    {
                        int oldIndex = PreviousSelectedIndex;

                        int newIndex = (int)e.NewItems[0];

                        if (newIndex >= 0)
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

            for (int i = startIndex; i <= endIndex; i++)
            {
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
                if (i > -1 && ActualData.Count > i)
                    dataPoint.Item = ActualData[i];

                return dataPoint;
            }
            else
                return dataPoint;
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
                        chartSegment = index != -1 ? new StepAreaSegment() : null;
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
                    index = this.GetXValues().IndexOf(xVal);
                    data = this.ActualData[index];
                }

                if (data == null) return; // To ignore tooltip when mousePos is not inside the SeriesClipRect.
                if (this.Chart.Tooltip == null)
                    this.Chart.Tooltip = new ChartTooltip();
                var chartTooltip = this.Chart.Tooltip as ChartTooltip;
                var stepAreaSegment = chartSegment as StepAreaSegment;
                stepAreaSegment.Item = data;
                stepAreaSegment.XData = xVal;
                stepAreaSegment.YData = yVal;
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
        /// <param name="newIndex">new index</param>
        /// <param name="oldIndex">old index</param>
        internal override void SelectedIndexChanged(int newIndex, int oldIndex)
        {
            if (ActualArea != null && SelectionBehavior != null)
            {
                // Resets the adornment selection when the mouse pointer moved away from the adornment or clicked the same adornment.
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

        #region Protected Override Methods

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            return new StepAreaSegment();
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
            (d as StepAreaSeries).ScheduleUpdateChart();
        }

        #endregion

        #region Private Methods

        private void AnimateAdornments()
        {
            if (this.AdornmentsInfo != null && ShowDataLabels)
            {
                sb = new Storyboard();
                double totalDuration = AnimationDuration.TotalSeconds;

                // UWP-185-RectAnimation takes some delay to render series.
                totalDuration *= 1.2;

                foreach (var child in this.AdornmentPresenter.Children)
                {
                    DoubleAnimationUsingKeyFrames keyFrames1 = new DoubleAnimationUsingKeyFrames();
                    SplineDoubleKeyFrame frame1 = new SplineDoubleKeyFrame();

                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                    frame1.Value = 0;
                    keyFrames1.KeyFrames.Add(frame1);

                    frame1 = new SplineDoubleKeyFrame();
                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(totalDuration));
                    frame1.Value = 0;
                    keyFrames1.KeyFrames.Add(frame1);

                    frame1 = new SplineDoubleKeyFrame();
                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(totalDuration + 1));
                    frame1.Value = 1;
                    keyFrames1.KeyFrames.Add(frame1);

                    KeySpline keySpline = new KeySpline();
                    keySpline.ControlPoint1 = new Point(0.64, 0.84);

                    keySpline.ControlPoint2 = new Point(0, 1);
                    keyFrames1.EnableDependentAnimation = true;
                    Storyboard.SetTargetProperty(keyFrames1, "(Opacity)");
                    frame1.KeySpline = keySpline;

                    Storyboard.SetTarget(keyFrames1, child as FrameworkElement);
                    sb.Children.Add(keyFrames1);
                }

                sb.Begin();
            }
        }

        #endregion

        #endregion
    }
}
