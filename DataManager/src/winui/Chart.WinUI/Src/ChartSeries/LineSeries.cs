using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="LineSeries"/> displays a collection of data points connected using straight lines.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="LineSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XyDataSeries.StrokeThickness"/>, <see cref="StrokeDashArray"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="LineSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="LineSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
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
    ///           <chart:LineSeries ItemsSource="{Binding Data}"
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
    ///     LineSeries series = new LineSeries();
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
    /// ]]
    /// ></code>
    /// ***
    /// </example>
    /// <seealso cref="LineSegment"/>
    public class LineSeries : XyDataSeries
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="CustomTemplate"/> property.       
        /// </summary>
        public static readonly DependencyProperty CustomTemplateProperty =
            DependencyProperty.Register(
                nameof(CustomTemplate), 
                typeof(DataTemplate), 
                typeof(LineSeries),
                new PropertyMetadata(null, OnCustomTemplateChanged));


        /// <summary>
        /// The Dependency property for <see cref="StrokeDashArray"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register(
                nameof(StrokeDashArray),
                typeof(DoubleCollection),
                typeof(LineSeries),
                new PropertyMetadata(null));

        #endregion

        #region Fields

        #region Private Fields

        private Storyboard sb;

        Point hitPoint = new Point();

        private RectAnimation animation;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the template to customize the appearance of line series.
        /// </summary>
        /// <value>It accepts <see cref="DataTemplate"/> value.</value>
        public DataTemplate CustomTemplate
        {
            get { return (DataTemplate)GetValue(CustomTemplateProperty); }
            set { SetValue(CustomTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke dash array to customize the appearance of line stroke.
        /// </summary>
        /// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:LineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            StrokeDashArray="5,3" />
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
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(5);
        ///     doubleCollection.Add(3);
        ///     LineSeries series = new LineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           StrokeDashArray = doubleCollection,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of LineSeries.
        /// </summary>
        internal override void GenerateSegments()
        {
            int index = -1;
            List<double> xValues = null;
            bool isGrouping = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).ArrangeByIndex;
            if (isGrouping)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();
            if (xValues != null)
            {
                if (isGrouping)
                {
                    Segments.Clear();
                    Adornments.Clear();

                    for (int i = 0; i < xValues.Count; i++)
                    {
                        index = i + 1;
                        if (GroupedSeriesYValues != null)
                        {
                            if (index < xValues.Count)
                              CreateSegment(new[] { xValues[i], GroupedSeriesYValues[0][i], xValues[index], GroupedSeriesYValues[0][index] }, ActualData[i]);

                            if (AdornmentsInfo != null && ShowDataLabels)
                            {
                                Adornments.Add(this.CreateAdornment(this, xValues[i], GroupedSeriesYValues[0][i], xValues[i], GroupedSeriesYValues[0][i]));
                                Adornments[i].Item = ActualData[i];
                            }
                        }
                    }
                }
                else
                {
                    ClearUnUsedSegments(this.PointsCount);
                    ClearUnUsedAdornments(this.PointsCount);
                    for (int i = 0; i < this.PointsCount; i++)
                    {
                        index = i + 1;
                        if (i < Segments.Count)
                        {
                            Segments[i].Item = ActualData[i];
                            if (index < this.PointsCount)
                            {
                                (Segments[i]).SetData(xValues[i], YValues[i], xValues[index], YValues[index]);
                                (Segments[i] as LineSegment).Item = ActualData[i];
                                (Segments[i] as LineSegment).YData = YValues[i];
                            }
                            else
                                Segments.RemoveAt(i);
                        }
                        else
                        {
                            if (index < this.PointsCount)
                               CreateSegment(new[] { xValues[i], YValues[i], xValues[index], YValues[index]}, ActualData[i]);
                        }

                        if (AdornmentsInfo != null && ShowDataLabels)
                        {
                            if (i < Adornments.Count)
                            {
                                Adornments[i].SetData(xValues[i], YValues[i], xValues[i], YValues[i]);
                            }
                            else
                            {
                                Adornments.Add(this.CreateAdornment(this, xValues[i], YValues[i], xValues[i], YValues[i]));
                            }

                            Adornments[i].Item = ActualData[i];
                        }
                    }
                }
            }
        }

        #endregion

        #region Internal Override Methods

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
        internal override void SelectedSegmentsIndexes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null && SelectionBehavior.EnableMultiSelection)
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

                    if (e.OldItems != null && SelectionBehavior.EnableMultiSelection)
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

        internal override void UpdateTooltip(object originalSource)
        {
            var canvas = this.Chart.GetAdorningCanvas();
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
                                chartSegment = new LineSegment()
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

                // WPF-53847- Tooltip shows the previous data point instead of last data point.
                if (TooltipTemplate != null && segment.Item == ActualData[Segments.Count] && Segments.Contains(segment) && Adornments != null && Adornments.Count == 0)
                {
                    var lineSegment = segment as LineSegment;

                    chartSegment = new LineSegment()
                    {
                        X1Value = lineSegment.X1Value,
                        Y1Value = lineSegment.Y1Value,
                        Y2Value = lineSegment.Y2Value,
                        X1Data = lineSegment.X1Data,
                        Y1Data = lineSegment.Y1Data,
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
                    };
                }

                var lineSeg = chartSegment as LineSegment;
                SetTooltipSegmentItem(lineSeg);
                SetTooltipDuration();
                ToolTipTag = lineSeg;
                var chartTooltip = this.Chart.Tooltip as ChartTooltip;

                if (chartTooltip != null)
                {
                    chartTooltip.PolygonPath = " ";
                    chartTooltip.DataContext = lineSeg;

                    if (canvas.Children.Count == 0 || (canvas.Children.Count > 0 && !IsTooltipAvailable(canvas)))
                    {
                        if (ChartTooltip.GetActualInitialShowDelay(ActualArea.TooltipBehavior, ChartTooltip.GetInitialShowDelay(this)) == 0)
                        {
                            canvas.Children.Add(chartTooltip);
                        }

                        chartTooltip.ContentTemplate = this.GetTooltipTemplate();
                        AddTooltip();

                        if (ChartTooltip.GetActualEnableAnimation(ActualArea.TooltipBehavior, ChartTooltip.GetEnableAnimation(this)))
                        {
                            SetDoubleAnimation(chartTooltip);
                        }

                        Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                        Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                    }
                    else
                    {
                        foreach (var child in canvas.Children)
                        {
                            var tooltip = child as ChartTooltip;
                            if (tooltip != null)
                                chartTooltip = tooltip;
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
            LineSegment lineSegment = ToolTipTag as LineSegment;
            
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(lineSegment.XData, lineSegment.YData);
            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top;
            return newPosition;
        }

        private void SetTooltipSegmentItem(LineSegment lineSegment)
        {
            double xVal = 0;
            double yVal = 0;
            double stackValue = double.NaN;
            var point = new Point(
                mousePosition.X - this.Chart.SeriesClipRect.Left,
                mousePosition.Y - this.Chart.SeriesClipRect.Top);

            FindNearestChartPoint(point, out xVal, out yVal, out stackValue);
            if (double.IsNaN(xVal)) return;
            if (lineSegment != null)
                lineSegment.YData = yVal == lineSegment.Y1Value
                    ? lineSegment.Y1Value
                    : lineSegment.Y2Value;
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

            if (ToolTipTag != null)
            {
                var previousTag = ToolTipTag as LineSegment;
                int previousIndex = this.ActualData.IndexOf(previousTag.Item);

                if (segmentIndex != previousIndex)
                {
                    RemoveTooltip();
                    Timer.Stop();
                    ActualArea.Tooltip = new ChartTooltip();
                }
            }
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
                sb = new Storyboard();
                double secondsPerPoint = AnimationDuration.TotalSeconds / YValues.Count;

                var adornTransXPath = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleX)";
                var adornTransYPath = "(UIElement.RenderTransform).(TransformGroup.Children)[0].(ScaleTransform.ScaleY)";

                // UWP-185-RectAnimation takes some delay to render series.
                secondsPerPoint *= 1.2;
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
                        From = 0.6,
                        To = 1,
                        Duration = new Duration().GetDuration(TimeSpan.FromSeconds(AnimationDuration.TotalSeconds / 2)),
                        BeginTime = TimeSpan.FromSeconds(i * secondsPerPoint)
                    };

                    keyFrames1.EnableDependentAnimation = true;
                    Storyboard.SetTargetProperty(keyFrames1, adornTransXPath);
                    Storyboard.SetTarget(keyFrames1, label);
                    sb.Children.Add(keyFrames1);
                    keyFrames1 = new DoubleAnimation()
                    {
                        From = 0.6,
                        To = 1,
                        Duration = new Duration().GetDuration(TimeSpan.FromSeconds(AnimationDuration.TotalSeconds / 2)),
                        BeginTime = TimeSpan.FromSeconds(i * secondsPerPoint)
                    };

                    keyFrames1.EnableDependentAnimation = true;
                    Storyboard.SetTargetProperty(keyFrames1, adornTransYPath);
                    Storyboard.SetTarget(keyFrames1, label);
                    sb.Children.Add(keyFrames1);
                    i++;
                }

                sb.Begin();
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

                    if (ActualArea != null && newIndex < Segments.Count)
                    {
                        OnSelectionChanged(newIndex, oldIndex);
                    }
                    else if (ActualArea != null && Segments.Count > 0 && newIndex == Segments.Count)
                    {
                        OnSelectionChanged(newIndex, oldIndex);
                    }
                    else if (Segments.Count == 0)
                    {
                        triggerSelectionChangedEventOnLoad = true;
                    }
                }
                else if (newIndex == -1 && ActualArea != null && oldIndex < Segments.Count)
                {
                    OnSelectionChanged(newIndex,oldIndex);
                }
                else if (newIndex == -1 && ActualArea != null && Segments.Count > 0 && oldIndex == Segments.Count)
                {
                    OnSelectionChanged(newIndex,oldIndex);
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
            return new LineSegment();
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
            var series = d as LineSeries;

            if (series.Chart == null) return;

            series.Segments.Clear();
            series.Chart.ScheduleUpdate();
        }
        

        #endregion

        #region Private Methods

        /// <summary>
        /// Add the <see cref="LineSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="actualData">The actualData.</param>
        private void CreateSegment(double[] values, object actualData)
        {
            LineSegment segment = CreateSegment() as LineSegment;
            if (segment != null)
            {
                segment.Series = this;
                segment.Item = actualData;
                segment.SetData(values);
                Segments.Add(segment);
            }
        }

#endregion

#endregion
    }
}