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
    /// The <see cref="SplineAreaSeries"/> is a collection of data points using smooth Bezier line curves with the areas below filled.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="SplineAreaSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XyDataSeries.StrokeWidth"/>, <see cref="Stroke"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="SplineAreaSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="SplineAreaSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
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
    ///           <chart:SplineAreaSeries ItemsSource="{Binding Data}"
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
    ///     SplineAreaSeries series = new SplineAreaSeries();
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
    /// <seealso cref="SplineAreaSegment"/>
    public class SplineAreaSeries : XyDataSeries
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="IsClosed"/> property.
        /// </summary>
        internal static readonly DependencyProperty IsClosedProperty =
            DependencyProperty.Register(
                nameof(IsClosed),
                typeof(bool), 
                typeof(SplineAreaSeries),
                new PropertyMetadata(true, OnPropertyChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for SplineType.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(
                nameof(Type),
                typeof(SplineType),
                typeof(SplineAreaSeries),
                new PropertyMetadata(SplineType.Natural, OnSplineTypeChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(SplineAreaSeries),
            new PropertyMetadata(null, OnStrokeChanged));

        #endregion

        #region Fields

        #region Private Fields

        Storyboard sb;

        Point hitPoint = new Point();

        List<ChartPoint> startControlPoints;

        List<ChartPoint> endControlPoints;

        private RectAnimation animation;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        internal bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicate the spline type.
        /// </summary>
        /// <value>It accepts <see cref="SplineType"/> values and the default value is <see cref="SplineType.Natural"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:SplineAreaSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Type="Cardinal" />
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
        ///     SplineAreaSeries series = new SplineAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Type = SplineType.Cardinal,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public SplineType Type
        {
            get { return (SplineType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
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
        ///          <chart:SplineAreaSeries ItemsSource="{Binding Data}"
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
        ///     SplineAreaSeries splineAreaSeries = new SplineAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth= 3,
        ///     };
        ///     
        ///     chart.Series.Add(splineAreaSeries);
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

        #region Private Properties

        private SplineAreaSegment Segment { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of SplineAreaSeries.
        /// </summary>      
        internal override void GenerateSegments()
        {
            int index = -1;
            double[] yCoef = null;
            var segmentPoints = new List<ChartPoint>();
            double Origin = 0;
            
            List<double> xValues = null;
            bool isGrouping = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
            if (isGrouping)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();

            if (xValues != null && xValues.Count > 1)
            {
                if (isGrouping)
                {
                    Segments.Clear();
                    Adornments.Clear();

                    if (Type == SplineType.Monotonic)
                        GetMonotonicSpline(xValues, GroupedSeriesYValues[0]);
                    else if (Type == SplineType.Cardinal)
                        GetCardinalSpline(xValues, GroupedSeriesYValues[0]);
                    else
                        this.NaturalSpline(xValues, GroupedSeriesYValues[0], out yCoef);

                    ChartPoint initialPoint = new ChartPoint(xValues[0], Origin);
                    segmentPoints.Add(initialPoint);
                    initialPoint = new ChartPoint(xValues[0], double.IsNaN(GroupedSeriesYValues[0][0]) ? Origin : GroupedSeriesYValues[0][0]);
                    segmentPoints.Add(initialPoint);
                    for (int i = 0; i < xValues.Count; i++)
                    {
                        index = i + 1;
                        if (index < xValues.Count && index < GroupedSeriesYValues[0].Count)
                        {
                            ChartPoint startPoint = new ChartPoint(xValues[i], GroupedSeriesYValues[0][i]);
                            ChartPoint endPoint = new ChartPoint(xValues[index], GroupedSeriesYValues[0][index]);
                            ChartPoint startControlPoint;
                            ChartPoint endControlPoint;

                            // Calculate curve points. 
                            if (Type == SplineType.Monotonic)
                            {
                                startControlPoint = startControlPoints[index - 1];
                                endControlPoint = endControlPoints[index - 1];
                            }
                            else if (Type == SplineType.Cardinal)
                            {
                                startControlPoint = startControlPoints[index - 1];
                                endControlPoint = endControlPoints[index - 1];
                            }
                            else
                                GetBezierControlPoints(startPoint, endPoint, yCoef[i], yCoef[index], out startControlPoint, out endControlPoint);

                            segmentPoints.AddRange(new ChartPoint[] { startControlPoint, endControlPoint, endPoint });
                        }
                    }

                    if (Segment == null || Segments.Count == 0)
                    {
                        Segment = CreateSegment() as SplineAreaSegment;
                        if (Segment != null)
                        {
                            Segment.Series = this;
                            Segment.Item = ActualData;
                            Segment.SetData(segmentPoints, xValues, GroupedSeriesYValues[0]);
                            Segments.Add(Segment);
                        }
                    }

                    if (AdornmentsInfo != null && ShowDataLabels)
                        AddAreaAdornments(GroupedSeriesYValues[0]);
                }
                else
                {
                    ClearUnUsedAdornments(this.PointsCount);
                    if (Type == SplineType.Monotonic)
                        GetMonotonicSpline(xValues, YValues);
                    else if (Type == SplineType.Cardinal)
                        GetCardinalSpline(xValues, YValues);
                    else
                        this.NaturalSpline(xValues, YValues, out yCoef);

                    ChartPoint initialPoint = new ChartPoint(xValues[0], Origin);
                    segmentPoints.Add(initialPoint);
                    initialPoint = new ChartPoint(xValues[0], double.IsNaN(YValues[0]) ? Origin : YValues[0]);
                    segmentPoints.Add(initialPoint);

                    for (int i = 0; i < PointsCount; i++)
                    {
                        index = i + 1;
                        if (index < PointsCount)
                        {
                            ChartPoint startPoint = new ChartPoint(xValues[i], YValues[i]);
                            ChartPoint endPoint = new ChartPoint(xValues[index], YValues[index]);
                            ChartPoint startControlPoint;
                            ChartPoint endControlPoint;

                            // Calculate curve points. 
                            if (Type == SplineType.Monotonic)
                            {
                                startControlPoint = startControlPoints[index - 1];
                                endControlPoint = endControlPoints[index - 1];
                            }
                            else if (Type == SplineType.Cardinal)
                            {
                                startControlPoint = startControlPoints[index - 1];
                                endControlPoint = endControlPoints[index - 1];
                            }
                            else
                                GetBezierControlPoints(startPoint, endPoint, yCoef[i], yCoef[index], out startControlPoint, out endControlPoint);

                            segmentPoints.AddRange(new ChartPoint[] { startControlPoint, endControlPoint, endPoint });
                        }
                    }

                    if (Segment == null || Segments.Count == 0)
                    {
                        Segment = CreateSegment() as SplineAreaSegment;
                        if (Segment != null)
                        {
                            Segment.Series = this;
                            Segment.Item = ActualData;
                            Segment.SetData(segmentPoints, xValues, YValues);
                            Segments.Add(Segment);
                        }
                    }
                    else
                    {
                        Segment.Item = ActualData;
                        (Segment as SplineAreaSegment).SetData(segmentPoints, xValues, YValues);
                    }

                    if (AdornmentsInfo != null && ShowDataLabels)
                        AddAreaAdornments(YValues);
                }
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

        internal override void UpdateRange()
        {
            base.UpdateRange();
            VisibleYRange = new DoubleRange(VisibleYRange.Start > 0 ? 0 : VisibleYRange.Start, VisibleYRange.End);
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
                        chartSegment = index != -1 ? new SplineAreaSegment() : null;
                    }
                }

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
                    if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                        index = GroupedSeriesYValues[0].IndexOf(yVal);
                    else
                        index = this.GetXValues().IndexOf(xVal);
                    data = this.ActualData[index];
                }

                if (data == null) return; // To ignore tooltip when mousePos is not inside the SeriesClipRect.
                if (this.Chart.Tooltip == null)
                    this.Chart.Tooltip = new ChartTooltip();
                var chartTooltip = this.Chart.Tooltip as ChartTooltip;
                var splineAreaSegment = chartSegment as SplineAreaSegment;
                splineAreaSegment.Item = data;
                splineAreaSegment.XData = xVal;
                splineAreaSegment.YData = yVal;

                if (chartTooltip != null)
                {
                    ToolTipTag = splineAreaSegment;
                    chartTooltip.PolygonPath = " ";

                    if (canvas.Children.Count == 0 || (canvas.Children.Count > 0 && !IsTooltipAvailable(canvas)))
                    {
                        chartTooltip.DataContext = splineAreaSegment;
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

                        chartTooltip.DataContext = splineAreaSegment;

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
            SplineAreaSegment splineAreaSegment = ToolTipTag as SplineAreaSegment;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(splineAreaSegment.XData, splineAreaSegment.YData);
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
            return new SplineAreaSegment();
        }
        /// <summary>
        /// Called when ItemsSource property changed.
        /// </summary>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
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

        #region Protected Methods
        
        /// <summary>
        /// Method used to calculate cardinal spline values.
        /// </summary>
        /// <param name="xValues">xvalues</param>
        /// <param name="yValues">yvalues</param>
        internal void GetCardinalSpline(List<double> xValues, IList<double> yValues)
        {
            int count = 0;
            startControlPoints = new List<ChartPoint>(PointsCount);
            endControlPoints = new List<ChartPoint>(PointsCount);
            bool isGrouping = this.ActualXAxis is CategoryAxis ? (this.ActualXAxis as CategoryAxis).IsIndexed : true;

            if (!isGrouping)
                count = (int)xValues.Count;
            else
                count = (int)PointsCount;

            double[] tangentsX = new double[count];
            double[] tangentsY = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (i == 0 && xValues.Count > 2)
                    tangentsX[i] = (0.5 * (xValues[i + 2] - xValues[i]));
                else if (i == count - 1 && count - 3 >= 0)
                    tangentsX[i] = (0.5 * (xValues[count - 1] - xValues[count - 3]));
                else if (i - 1 >= 0 && xValues.Count > i + 1)
                    tangentsX[i] = (0.5 * (xValues[i + 1] - xValues[i - 1]));
                if (double.IsNaN(tangentsX[i]))
                    tangentsX[i] = 0;

                if (ActualXAxis is DateTimeAxis)
                {
                    DateTime date = xValues[i].FromOADate();
                    if ((ActualXAxis as DateTimeAxis).IntervalType == DateTimeIntervalType.Auto ||
                            (ActualXAxis as DateTimeAxis).IntervalType == DateTimeIntervalType.Years)
                    {
                        int year = DateTime.IsLeapYear(date.Year) ? 366 : 365;
                        tangentsY[i] = tangentsX[i] / year;
                    }
                    else if ((ActualXAxis as DateTimeAxis).IntervalType == DateTimeIntervalType.Months)
                    {
                        double month = DateTime.DaysInMonth(date.Year, date.Month);
                        tangentsY[i] = tangentsX[i] / month;
                    }
                }
                else
                    tangentsY[i] = tangentsX[i];
            }

            for (int i = 0; i < tangentsX.Length - 1; i++)
            {
                startControlPoints.Add(new ChartPoint(xValues[i] + tangentsX[i] / 3, yValues[i] + tangentsY[i] / 3));
                endControlPoints.Add(new ChartPoint(xValues[i + 1] - tangentsX[i + 1] / 3, yValues[i + 1] - tangentsY[i + 1] / 3));
            }
        }

        /// <summary>
        /// Method used to calculate monotonic spline values.
        /// </summary>
        /// <param name="xValues">xvalues</param>
        /// <param name="yValues">yvalues</param>
        internal void GetMonotonicSpline(List<double> xValues, IList<double> yValues)
        {
            int count = 0;
            startControlPoints = new List<ChartPoint>(PointsCount);
            endControlPoints = new List<ChartPoint>(PointsCount);
            bool isGrouping = this.ActualXAxis is CategoryAxis ? (this.ActualXAxis as CategoryAxis).IsIndexed : true;
            if (!isGrouping)
                count = (int)xValues.Count;
            else
                count = (int)PointsCount;
            double[] dx = new double[count - 1];
            double[] slope = new double[count - 1];
            List<double> coefficent = new List<double>();

            // Find the slope between the values.
            for (int i = 0; i < count - 1; i++)
            {
                if (!double.IsNaN(yValues[i + 1]) && !double.IsNaN(yValues[i])
                    && !double.IsNaN(xValues[i + 1]) && !double.IsNaN(xValues[i]))
                {
                    dx[i] = xValues[i + 1] - xValues[i];
                    slope[i] = (yValues[i + 1] - yValues[i]) / dx[i];
                    if (double.IsInfinity(slope[i]))
                        slope[i] = 0;
                }
            }

            // Add the first and last coefficent value as Slope[0] and Slope[n-1]
            if (slope.Length == 0) return;
            coefficent.Add(double.IsNaN(slope[0]) ? 0 : slope[0]);
            for (int i = 0; i < dx.Length - 1; i++)
            {
                if (slope.Length > i + 1)
                {
                    double m = slope[i], m_next = slope[i + 1];
                    if (m * m_next <= 0)
                    {
                        coefficent.Add(0);
                    }
                    else
                    {
                        if (dx[i] == 0)
                            coefficent.Add(0);
                        else
                        {
                            double firstPoint = dx[i], nextPoint = dx[i + 1];
                            double interPoint = firstPoint + nextPoint;
                            coefficent.Add(3 * interPoint / ((interPoint + nextPoint) / m + (interPoint + firstPoint) / m_next));
                        }
                    }
                }
            }

            coefficent.Add(double.IsNaN(slope[slope.Length - 1]) ? 0 : slope[slope.Length - 1]);

            for (int i = 0; i < coefficent.Count; i++)
            {
                if (i + 1 < coefficent.Count && dx.Length > 0)
                {
                    double value = (dx[i] / 3);
                    startControlPoints.Add(new ChartPoint(xValues[i] + value, yValues[i] + coefficent[i] * value));
                    endControlPoints.Add(new ChartPoint(xValues[i + 1] - value, yValues[i + 1] - coefficent[i + 1] * value));
                }
            }
        }

        /// <summary>
        /// Method used to calculate the natural spline values.
        /// </summary>
        /// <param name="xValues">xvalues</param>
        /// <param name="yValues">yvalues</param>
        /// <param name="ys2">ys2</param>
        internal void NaturalSpline(List<double> xValues, IList<double> yValues, out double[] ys2)
        {
            int count = 0;

            bool isGrouping = this.ActualXAxis is CategoryAxis ? (this.ActualXAxis as CategoryAxis).IsIndexed : true;
            if (!isGrouping)
                count = (int)xValues.Count;
            else
                count = (int)PointsCount;
            ys2 = new double[count];
            if (count == 1)
                return;
            double a = 6;
            double[] u = new double[count - 1];
            double p;

            if (Type == SplineType.Natural)
            {
                ys2[0] = u[0] = 0;
                ys2[count - 1] = 0;
            }
            else if (xValues.Count > 1)
            {
                double d0 = (xValues[1] - xValues[0]) / (yValues[1] - yValues[0]);
                double dn = (xValues[count - 1] - xValues[count - 2]) / (yValues[count - 1] - yValues[count - 2]);
                u[0] = 0.5;
                ys2[0] = (3 * (yValues[1] - yValues[0])) / (xValues[1] - xValues[0]) - 3 * d0;
                ys2[count - 1] = 3 * dn - (3 * (yValues[count - 1] - yValues[count - 2])) / (xValues[count - 1] - xValues[count - 2]);
                if (double.IsInfinity(ys2[0]) || double.IsNaN(ys2[0]))
                    ys2[0] = 0;
                if (double.IsInfinity(ys2[count - 1]) || double.IsNaN(ys2[count - 1]))
                    ys2[count - 1] = 0;
            }

            for (int i = 1; i < count - 1; i++)
            {
                if (yValues.Count > i + 1 && !double.IsNaN(yValues[i + 1]) && !double.IsNaN(yValues[i - 1]) && !double.IsNaN(yValues[i]))
                {
                    double d1 = xValues[i] - xValues[i - 1];
                    double d2 = xValues[i + 1] - xValues[i - 1];
                    double d3 = xValues[i + 1] - xValues[i];
                    double dy1 = yValues[i + 1] - yValues[i];
                    double dy2 = yValues[i] - yValues[i - 1];

                    if (xValues[i] == xValues[i - 1] || xValues[i] == xValues[i + 1])
                    {
                        ys2[i] = 0;
                        u[i] = 0;
                    }
                    else
                    {
                        p = 1 / (d1 * ys2[i - 1] + 2 * d2);
                        ys2[i] = -p * d3;
                        u[i] = p * (a * (dy1 / d3 - dy2 / d1) - d1 * u[i - 1]);
                    }
                }
            }

            for (int k = count - 2; k >= 0; k--)
            {
                ys2[k] = ys2[k] * ys2[k + 1] + u[k];
            }
        }

        /// <summary>
        /// Method used to calculate the bezier values.
        /// </summary>
        /// <param name="point1">chart point</param>
        /// <param name="point2">chart point</param>
        /// <param name="ys1">ys1</param>
        /// <param name="ys2">ys2</param>
        /// <param name="controlPoint1">chart point</param>
        /// <param name="controlPoint2">chart point</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        internal void GetBezierControlPoints(ChartPoint point1, ChartPoint point2, double ys1, double ys2, out ChartPoint controlPoint1, out ChartPoint controlPoint2)
        {
            const double One_thrid = 1 / 3.0d;
            double deltaX2 = point2.X - point1.X;

            deltaX2 = deltaX2 * deltaX2;

            double dx1 = 2 * point1.X + point2.X;
            double dx2 = point1.X + 2 * point2.X;

            double dy1 = 2 * point1.Y + point2.Y;
            double dy2 = point1.Y + 2 * point2.Y;

            double y1 = One_thrid * (dy1 - One_thrid * deltaX2 * (ys1 + 0.5f * ys2));
            double y2 = One_thrid * (dy2 - One_thrid * deltaX2 * (0.5f * ys1 + ys2));

            controlPoint1 = new ChartPoint(dx1 * One_thrid, y1);
            controlPoint2 = new ChartPoint(dx2 * One_thrid, y2);
        }

        #endregion

        #region Private Static Methods

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SplineAreaSeries).ScheduleUpdateChart();
        }

        private static void OnSplineTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SplineAreaSeries)d).ScheduleUpdateChart();
        }


        #endregion

        #endregion
    }
}
