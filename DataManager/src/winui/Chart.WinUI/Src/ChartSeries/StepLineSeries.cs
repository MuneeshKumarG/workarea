using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="StepLineSeries"/> uses vertical and horizontal lines to connect the data points in a series forming a step-like progression.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="StepLineSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="ChartSeries.StrokeThickness"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="StepLineSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="StepLineSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
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
    ///           <chart:StepLineSeries ItemsSource="{Binding Data}"
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
    ///     StepLineSeries series = new StepLineSeries();
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
    /// <seealso cref="StepLineSegment"/>
    public class StepLineSeries : XyDataSeries
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="CustomTemplate"/> property.       .
        /// </summary>
        public static readonly DependencyProperty CustomTemplateProperty =
            DependencyProperty.Register(
                nameof(CustomTemplate),
                typeof(DataTemplate),
                typeof(StepLineSeries),
                new PropertyMetadata(null, OnCustomTemplateChanged));
       
        #endregion

        #region Fields

        #region Private Fields

        Point hitPoint = new Point();
        
        private RectAnimation animation;

        Storyboard sb;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the template to customize the appearance of step line series.
        /// </summary>
        /// <value>It accepts <see cref="DataTemplate"/> value.</value>
        public DataTemplate CustomTemplate
        {
            get { return (DataTemplate)GetValue(CustomTemplateProperty); }
            set { SetValue(CustomTemplateProperty, value); }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of StepLineSeries.
        /// </summary>      
        internal override void GenerateSegments()
        {
            List<double>? xValues = null;
            bool isGrouping = this.ActualXAxis is CategoryAxis categoryAxis && !categoryAxis.IsIndexed;
            if (isGrouping)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();
            if (xValues == null) return;

            if (isGrouping)
            {
                Segments.Clear();
                Adornments.Clear();

                for (int i = 0; i < xValues.Count; i++)
                {
                    int index = i + 1;
                    ChartPoint point1, point2, stepPoint;

                    if (ActualData != null && AdornmentsInfo != null && ShowDataLabels)
                    {
                        Adornments.Add(this.CreateAdornment(this, xValues[i], GroupedSeriesYValues[0][i], xValues[i], GroupedSeriesYValues[0][i]));
                        Adornments[i].Item = ActualData[i];
                    }

                    if (index < xValues.Count)
                    {
                        point1 = new ChartPoint(xValues[i], GroupedSeriesYValues[0][i]);
                        point2 = new ChartPoint(xValues[index], GroupedSeriesYValues[0][i]);
                        stepPoint = new ChartPoint(xValues[index], GroupedSeriesYValues[0][index]);
                    }
                    else
                    {
                        point1 = new ChartPoint(xValues[i], GroupedSeriesYValues[0][i]);
                        point2 = new ChartPoint(xValues[i], GroupedSeriesYValues[0][i]);
                        stepPoint = new ChartPoint(xValues[i], GroupedSeriesYValues[0][i]);
                    }

                    StepLineSegment? segment = CreateSegment() as StepLineSegment;
                    if (segment != null && ActualData != null)
                    {
                        segment.Item = ActualData[i];
                        segment.Series = this;
                        List<ChartPoint>? listPoints = new List<ChartPoint>();
                        listPoints.Add(point1);
                        listPoints.Add(stepPoint);
                        listPoints.Add(point2);
                        segment.SetData(listPoints);
                        Segments.Add(segment);
                        listPoints = null;
                    }
                }
            }
            else
            {
                ClearUnUsedStepLineSegment(PointsCount);
                ClearUnUsedAdornments(PointsCount);
                for (int i = 0; i < PointsCount; i++)
                {
                    int index = i + 1;
                    ChartPoint point1, point2, stepPoint;

                    if (ActualData != null && AdornmentsInfo != null && ShowDataLabels)
                    {
                        if (i < Adornments.Count)
                        {
                            Adornments[i].SetData(xValues[i], YValues[i], xValues[i], YValues[i]);
                            Adornments[i].Item = ActualData[i];
                        }
                        else
                        {
                            Adornments.Add(this.CreateAdornment(this, xValues[i], YValues[i], xValues[i], YValues[i]));
                            Adornments[i].Item = ActualData[i];
                        }
                    }

                    if (index < PointsCount)
                    {
                        point1 = new ChartPoint(xValues[i], YValues[i]);
                        point2 = new ChartPoint(xValues[index], YValues[i]);
                        stepPoint = new ChartPoint(xValues[index], YValues[index]);
                    }
                    else
                    {
                        point1 = new ChartPoint(xValues[i], YValues[i]);
                        point2 = new ChartPoint(xValues[i], YValues[i]);
                        stepPoint = new ChartPoint(xValues[i], YValues[i]);
                    }

                    if (i < Segments.Count)
                    {
                        Segments[i].SetData(new List<ChartPoint> { point1, stepPoint, point2 });
                        Segments[i].Item = ActualData[i];
                        (Segments[i] as StepLineSegment).YData = YValues[i];
                    }
                    else
                    {
                        StepLineSegment segment = CreateSegment() as StepLineSegment;
                        if (segment != null)
                        {
                            segment.Item = ActualData[i];
                            segment.Series = this;
                            List<ChartPoint> listPoints = new List<ChartPoint>();
                            listPoints.Add(point1);
                            listPoints.Add(stepPoint);
                            listPoints.Add(point2);
                            segment.SetData(listPoints);
                            Segments.Add(segment);
                            listPoints = null;
                        }
                    }
                }
            }
        }

        #endregion

        #region Internal Override Methods

        internal override bool GetAnimationIsActive()
        {
            return animation != null && animation.IsActive;
        }

        internal override void ResetAdornmentAnimationState()
        {
            if (adornmentInfo != null)
            {
                foreach (var child in this.AdornmentPresenter.Children)
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null)
                    {
                        frameworkElement.ClearValue(FrameworkElement.RenderTransformProperty);
                        frameworkElement.ClearValue(FrameworkElement.OpacityProperty);
                    }
                }
            }
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
                    if (e.NewItems != null && SelectionBehavior!=null && SelectionBehavior.EnableMultiSelection)
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

                            if (ActualArea != null && newIndex < Segments.Count)
                            {
                                OnSelectionChanged(newIndex, oldIndex);
                                PreviousSelectedIndex = newIndex;
                            }
                            else if (ActualArea != null && Segments.Count > 0 && newIndex == Segments.Count)
                            {
                                OnSelectionChanged(newIndex, oldIndex);
                                PreviousSelectedIndex = newIndex;
                            }
                            else if (Segments.Count == 0)
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

                if (element != null)
                {
                    if (element.Tag is ChartSegment)
                        chartSegment = element.Tag;
                    else if (element.DataContext is ChartSegment && !(element.DataContext is ChartDataLabel))
                        chartSegment = element.DataContext;
                    else
                    {
                        int index = ChartExtensionUtils.GetAdornmentIndex(element);
                        if (index != -1)
                        {
                            if (index < Segments.Count)
                                chartSegment = Segments[index];
                            else if (index < Adornments.Count)
                            {
                                // WPF-53847- Tooltip shows the previous data point instead of last data point.
                                // WPF-28526- Tooltip not shown when set the single data point with adornments for continuous series.
                                ChartDataLabel adornment = Adornments[index];
                                chartSegment = new StepLineSegment()
                                {
                                    X1Value = adornment.XData,
                                    Y1Value = adornment.YData,
                                    X1Data = adornment.XData,
                                    Y1Data = adornment.YData,
                                    X1 = adornment.XData,
                                    Y1 = adornment.YData,
                                    XData = adornment.XData,
                                    YData = adornment.YData,
                                    Item = adornment.Item,
                                    Series = adornment.Series,
                                    Fill = adornment.Fill,
                                    IsAddedToVisualTree = adornment.IsAddedToVisualTree,
                                    IsSegmentVisible = adornment.IsSegmentVisible,
                                    IsSelectedSegment = adornment.IsSelectedSegment,
                                    PolygonPoints = adornment.PolygonPoints,
                                    Stroke = adornment.Stroke,
                                    StrokeDashArray = adornment.StrokeDashArray,
                                    StrokeWidth = adornment.StrokeWidth,
                                    XRange = adornment.XRange,
                                    YRange = adornment.YRange,
                                };
                            }
                        }
                    }
                }

                var segment = chartSegment as ChartSegment;
                var segCount = Segments.Count;
                // WPF-53847- Tooltip shows the previous data point instead of last data point.
                if (TooltipTemplate != null && segCount < ActualData.Count && segment.Item == ActualData[segCount] && Segments.Contains(segment) && Adornments != null && Adornments.Count == 0)
                {
                    var lineSegment = segment as StepLineSegment;

                    chartSegment = new StepLineSegment()
                    {
                        X1Value = lineSegment.X1Value,
                        Y1Value = lineSegment.Y1Value,
                        Y2Value = lineSegment.Y2Value,
                        X1Data = lineSegment.X1Data,
                        Y1Data = lineSegment.YData,
                        X1 = lineSegment.X1,
                        Y1 = lineSegment.Y1,
                        XData = lineSegment.XData,
                        YData = lineSegment.YData,
                        Item = lineSegment.Item,
                        Series = lineSegment.Series,
                        Fill = lineSegment.Fill,
                        IsAddedToVisualTree = lineSegment.IsAddedToVisualTree,
                        IsSegmentVisible = lineSegment.IsSegmentVisible,
                        IsSelectedSegment = lineSegment.IsSelectedSegment,
                        PolygonPoints = lineSegment.PolygonPoints,
                        Stroke = lineSegment.Stroke,
                        StrokeDashArray = lineSegment.StrokeDashArray,
                        StrokeWidth = lineSegment.StrokeWidth,
                        X2 = lineSegment.X2,
                        X2Value = lineSegment.X2Value,
                        XRange = lineSegment.XRange,
                        Y2 = lineSegment.Y2,
                        YRange = lineSegment.YRange,
                        Points = lineSegment.Points,
                        X3 = lineSegment.X3,
                        Y3 = lineSegment.Y3
                    };
                }

                SetTooltipDuration();
                var canvas = this.Chart.GetAdorningCanvas();
                if (this.Chart.Tooltip == null)
                    this.Chart.Tooltip = new ChartTooltip();
                var chartTooltip = this.Chart.Tooltip as ChartTooltip;
                if (chartTooltip != null)
                {
                    var lineSegment = chartSegment as StepLineSegment;
                    SetTooltipSegmentItem(lineSegment);
                    ToolTipTag = lineSegment;
                    chartTooltip.PolygonPath = " ";

                    if (canvas.Children.Count == 0 || (canvas.Children.Count > 0 && !IsTooltipAvailable(canvas)))
                    {
                        chartTooltip.DataContext = lineSegment;
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

                        chartTooltip.DataContext = lineSegment;
                        if (chartTooltip.DataContext == null)
                        {
                            RemoveTooltip();
                            return;
                        }

                        chartTooltip.ContentTemplate = this.GetTooltipTemplate();
                        AddTooltip();

                        Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                        Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                    }
                }
            }
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            StepLineSegment stepLineSegment = ToolTipTag as StepLineSegment;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(stepLineSegment.XData, stepLineSegment.YData);
            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top;
            return newPosition;
        }

        private void SetTooltipSegmentItem(StepLineSegment lineSegment)
        {
            double xVal = 0;
            double yVal = 0;
            double stackValue = double.NaN;
            var point = new Point(mousePosition.X - this.Chart.SeriesClipRect.Left, mousePosition.Y - this.Chart.SeriesClipRect.Top);

            FindNearestChartPoint(point, out xVal, out yVal, out stackValue);
            if (double.IsNaN(xVal)) return;
            if (lineSegment != null)
                lineSegment.YData = yVal == lineSegment.Y1Value ? lineSegment.Y1Value : lineSegment.Y1Data;
            lineSegment.XData = xVal;
            var segmentIndex = this.GetXValues().IndexOf(xVal);
            if (!IsIndexed)
            {
                IList<double> xValues = this.GetXValues();
                int i = segmentIndex;
                double nearestY = this.ActualSeriesYValues[0][i];
                while (xValues.Count > i && xValues[i] == xVal)
                {
                    double yValue = ActualArea.ActualPointToValue(ActualYAxis, point);
                    var validateYValue = ActualSeriesYValues[0][i];
                    if (Math.Abs(yValue - validateYValue) <= Math.Abs(yValue - nearestY))
                    {
                        segmentIndex = i;
                        nearestY = validateYValue;
                    }

                    i++;
                }
            }

            lineSegment.Item = this.ActualData[segmentIndex];
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
        /// Method used to set SelectionBrush to selectedindex chartsegment.
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

                    if (ActualArea != null && newIndex < Segments.Count)
                    {
                        OnSelectionChanged(newIndex, oldIndex);
                    }
                    else if (ActualArea != null && Segments.Count > 0 && newIndex == Segments.Count)
                    {
                        OnSelectionChanged(newIndex, oldIndex);
                        PreviousSelectedIndex = newIndex;
                    }
                    else if (Segments.Count == 0)
                    {
                        triggerSelectionChangedEventOnLoad = true;
                    }
                }
                else if (newIndex == -1 && ActualArea != null && oldIndex < Segments.Count)
                {
                    OnSelectionChanged(newIndex, oldIndex);
                    PreviousSelectedIndex = newIndex;
                }
                else if (newIndex == -1 && ActualArea != null && Segments.Count > 0 && oldIndex == Segments.Count)
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
            return new StepLineSegment();
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

        private static void OnCustomTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as StepLineSeries;

            if (series?.Chart == null) return;

            series.Segments.Clear();
            series.Chart.ScheduleUpdate();
        }
        
#endregion

#region Private Methods

        /// <summary>
        /// Add the <see cref="StepLineSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="values">The values.</param>
        private void CreateSegment(double[] values)
        {
            var segment = CreateSegment() as StepLineSegment;
            if (segment != null)
            {
                segment.Series = this;
                segment.SetData(values);
                Segments.Add(segment);
            }
        }

        /// <summary>
        /// Removes the unused segments
        /// </summary>       
        private void ClearUnUsedStepLineSegment(int startIndex)
        {
            if (this.Segments.Count >= startIndex && this.Segments.Count != 0)
            {
                int count = this.Segments.Count;

                for (int i = startIndex; i <= count; i++)
                {
                    if (i == count)
                        this.Segments.RemoveAt(startIndex - 1);
                    else
                        this.Segments.RemoveAt(startIndex);
                }
            }
        }

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
                    keySpline.ControlPoint2 = new Point(0, 1); // Animation have to provide same easing effect in all platforms.
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
