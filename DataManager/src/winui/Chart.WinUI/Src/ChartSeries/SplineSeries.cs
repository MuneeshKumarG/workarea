using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="SplineSeries"/> is a set of data points linked together by smooth beizer curves.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="SplineSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XyDataSeries.StrokeWidth"/>, <see cref="StrokeDashArray"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="SplineSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="SplineSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
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
    ///           <chart:SplineSeries ItemsSource="{Binding Data}"
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
    ///     SplineSeries series = new SplineSeries();
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
    /// <seealso cref="SplineSegment"/>
    public class SplineSeries : XyDataSeries
    {
        #region Dependency Property Registration

        /// <summary>
        ///  The DependencyProperty for <see cref="CustomTemplate"/> property.       
        /// </summary>
        public static readonly DependencyProperty CustomTemplateProperty =
            DependencyProperty.Register(
                nameof(CustomTemplate),
                typeof(DataTemplate), 
                typeof(SplineSeries),
                new PropertyMetadata(null, OnCustomTemplateChanged));

        /// <summary>
        /// Using a DependencyProperty as the backing store for SplineType.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(
                nameof(Type), 
                typeof(SplineType),
                typeof(SplineSeries), 
                new PropertyMetadata(SplineType.Natural, OnSplineTypeChanged));
        
        /// <summary>
        /// The Dependency property for <see cref="StrokeDashArray"/>
        /// </summary>
        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register(
                nameof(StrokeDashArray),
                typeof(DoubleCollection),
                typeof(SplineSeries),
                new PropertyMetadata(null));

        #endregion

        #region Fields

        Storyboard sb;

        List<SplineSegment> segments;

        List<double> previewYValues;

        Point hitPoint = new Point();

        List<ChartPoint> startControlPoints;

        List<ChartPoint> endControlPoints;

        private RectAnimation animation;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the template to customize the appearance of spline series.
        /// </summary>
        /// <value>It accepts <see cref="DataTemplate"/> value.</value>
        public DataTemplate CustomTemplate
        {
            get { return (DataTemplate)GetValue(CustomTemplateProperty); }
            set { SetValue(CustomTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicate the spline type.
        /// </summary>
        /// <value>It accepts <see cref="SplineType"/> values and the default value is <see cref="SplineType.Natural"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:SplineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Type="Cardinal" />
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
        ///     SplineSeries series = new SplineSeries()
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
        /// Gets or sets the stroke dash array to customize the appearance of spline curve.
        /// </summary>
        /// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:SplineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            StrokeDashArray="5,3" />
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
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(5);
        ///     doubleCollection.Add(3);
        ///     SplineSeries series = new SplineSeries()
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
        /// Creates the segments of SplineSeries.
        /// </summary>
        internal override void GenerateSegments()
        {
            int index = -1;
            double[] yCoef = null;
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

                    if (Type == SplineType.Monotonic)
                        GetMonotonicSpline(xValues, GroupedSeriesYValues[0]);
                    else if (Type == SplineType.Cardinal)
                        GetCardinalSpline(xValues, GroupedSeriesYValues[0]);
                    else
                        this.NaturalSpline(xValues, GroupedSeriesYValues[0], out yCoef);//Calculate natural curve points. 

                    for (int i = 0; i < xValues.Count; i++)
                    {
                        index = i + 1;
                        ChartPoint startPoint = new ChartPoint(xValues[i], GroupedSeriesYValues[0][i]);
                        if (index < xValues.Count && index < GroupedSeriesYValues[0].Count)
                        {
                            ChartPoint endPoint = new ChartPoint(xValues[index], GroupedSeriesYValues[0][index]);
                            ChartPoint startControlPoint;
                            ChartPoint endControlPoint;
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
                                GetBezierControlPoints(
                                    startPoint, 
                                    endPoint,
                                    yCoef[i],
                                    yCoef[index],
                                    out startControlPoint,
                                    out endControlPoint);

                            SplineSegment splineSegment = CreateSegment() as SplineSegment;
                            splineSegment.Series = this;
                            splineSegment.SetData(startPoint, startControlPoint, endControlPoint, endPoint);
                            splineSegment.X1 = xValues[i];
                            splineSegment.X2 = xValues[index];
                            splineSegment.Y1 = GroupedSeriesYValues[0][i];
                            splineSegment.Y2 = GroupedSeriesYValues[0][index];
                            splineSegment.Item = this.ActualData[i];
                            Segments.Add(splineSegment);
                        }
                        else if (index == Segments.Count)
                            Segments.RemoveAt(i);
                        if (AdornmentsInfo != null && ShowDataLabels)
                            AddAdornmentAtXY(startPoint.X, startPoint.Y, i);
                    }
                }
                else
                {
                    ClearUnUsedSegments(this.PointsCount);
                    ClearUnUsedAdornments(this.PointsCount);
                    if (Type == SplineType.Monotonic)
                        GetMonotonicSpline(xValues, YValues);
                    else if (Type == SplineType.Cardinal)
                        GetCardinalSpline(xValues, YValues);
                    else
                        this.NaturalSpline(xValues, YValues, out yCoef);

                    for (int i = 0; i < PointsCount; i++)
                    {
                        index = i + 1;
                        ChartPoint startPoint = new ChartPoint(xValues[i], YValues[i]);
                        if (index < PointsCount)
                        {
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

                            if (i < Segments.Count && Segments[i] is SplineSegment)
                            {
                                (Segments[i]).SetData(startPoint, startControlPoint, endControlPoint, endPoint);
                                (Segments[i] as SplineSegment).X1 = xValues[i];
                                (Segments[i] as SplineSegment).X2 = xValues[index];
                                (Segments[i] as SplineSegment).Y1 = YValues[i];
                                (Segments[i] as SplineSegment).Y2 = YValues[index];
                                (Segments[i] as SplineSegment).Item = this.ActualData[i];
                                (Segments[i] as SplineSegment).YData = YValues[i];
                            }
                            else
                            {
                                SplineSegment splineSegment = CreateSegment() as SplineSegment;
                                splineSegment.Series = this;
                                splineSegment.SetData(startPoint, startControlPoint, endControlPoint, endPoint);
                                splineSegment.X1 = xValues[i];
                                splineSegment.X2 = xValues[index];
                                splineSegment.Y1 = YValues[i];
                                splineSegment.Y2 = YValues[index];
                                splineSegment.Item = this.ActualData[i];
                                Segments.Add(splineSegment);
                            }
                        }
                        else if (index == Segments.Count)
                            Segments.RemoveAt(i);
                        if (AdornmentsInfo != null && ShowDataLabels)
                            AddAdornmentAtXY(startPoint.X, startPoint.Y, i);
                    }
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
                int index = -1;
                if (element != null)
                {
                    if (element.Tag is ChartSegment)
                        chartSegment = element.Tag;
                    else if (element.DataContext is ChartSegment && !(element.DataContext is ChartDataLabel))
                        chartSegment = element.DataContext;
                    else
                    {
                        index = ChartExtensionUtils.GetAdornmentIndex(element);
                        if (index != -1)
                        {
                            if (index < Segments.Count)
                                chartSegment = Segments[index];
                            else if (index < Adornments.Count)
                            {
                                // WPF-53847- Tooltip shows the previous data point instead of last data point.
                                // WPF-28526- Tooltip not shown when set the single data point with adornments for continuous series.
                                ChartDataLabel adornment = Adornments[index];
                                chartSegment = new SplineSegment()
                                {
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
                    var lineSegment = segment as SplineSegment;

                    chartSegment = new SplineSegment()
                    {
                        X1Data = lineSegment.X1Data,
                        Y1Data = lineSegment.Y1Data,
                        X1 = lineSegment.X1,
                        Y1 = lineSegment.Y1,
                        Y2 = lineSegment.Y2,
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
                        P1 = lineSegment.P1,
                        P2 = lineSegment.P2,
                        Q1 = lineSegment.Q1,
                        XRange = lineSegment.XRange,
                        Q2 = lineSegment.Q2,
                        Data = lineSegment.Data,
                        YRange = lineSegment.YRange,
                    };
                }

                SetTooltipDuration();
                var canvas = this.Chart.GetAdorningCanvas();
                if (this.Chart.Tooltip == null)
                    this.Chart.Tooltip = new ChartTooltip();
                var chartTooltip = this.Chart.Tooltip as ChartTooltip;
                if (chartTooltip != null)
                {
                    var lineSegment = chartSegment as SplineSegment;
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
            SplineSegment splineSegment = ToolTipTag as SplineSegment;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(splineSegment.XData, splineSegment.YData);
            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top;
            return newPosition;
        }
        private void SetTooltipSegmentItem(SplineSegment lineSegment)
        {
            double xVal = 0;
            double yVal = 0;
            double stackValue = double.NaN;
            var point = new Point(mousePosition.X - this.Chart.SeriesClipRect.Left, mousePosition.Y - this.Chart.SeriesClipRect.Top);

            FindNearestChartPoint(point, out xVal, out yVal, out stackValue);
            if (lineSegment != null)
                lineSegment.YData = yVal == lineSegment.Y1 ? lineSegment.Y1 : lineSegment.Y2;
            lineSegment.XData = xVal;
            var segmentIndex = this.GetXValues().IndexOf(xVal);
            if (!IsIndexed)
            {
                IList<double> xValues = this.GetXValues();
                int i = segmentIndex;
                double nearestY = this.ActualSeriesYValues[0][i];
                while (!IsIndexed && xValues.Count > i && xValues[i] == xVal)
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

            if (segmentIndex > 0)
                lineSegment.Item = this.ActualData[segmentIndex];
        }

        internal override bool GetAnimationIsActive()
        {
            return animation != null && animation.IsActive;
        }

        internal override void Animate()
        {
            var seriesRect = Chart.SeriesClipRect;
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

        internal override void UpdatePreviewSegmentDragging(Point mousePos)
        {
            UpdatePreviewSegmentAndSeries(mousePos);
            base.UpdatePreviewSegmentDragging(mousePos);
        }

        internal override void UpdatePreivewSeriesDragging(Point mousePos)
        {
            UpdatePreviewSegmentAndSeries(mousePos);
            base.UpdatePreivewSeriesDragging(mousePos);
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
            return new SplineSegment();
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

            double a = 6;
            double[] u = new double[count];
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

        private static void OnCustomTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as SplineSeries;

            if (series.Chart == null) return;

            series.Segments.Clear();
            series.Chart.ScheduleUpdate();
        }
        
        private static void OnSplineTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SplineSeries)d).ScheduleUpdateChart();
        }
        
#endregion

#region Private Methods

        private void UpdatePreviewSegmentAndSeries(Point mousePos)
        {
            try
            {
                Brush brush = (Segments[0] as SplineSegment).Fill;
                var xValues = GetXValues();
                double[] yCoef = null;

                if (segments == null)
                {
                    segments = new List<SplineSegment>();
                    previewYValues = new List<double>();
                    if (Type == SplineType.Monotonic)
                        GetMonotonicSpline(xValues, YValues);
                    else if (Type == SplineType.Cardinal)
                        GetCardinalSpline(xValues, YValues);
                    else
                        NaturalSpline(xValues, YValues, out yCoef);
                    var chartTransformer = this.ActualArea != null ? CreateTransformer(GetAvailableSize(), true) : null;
                    if (chartTransformer == null) return;

                    for (var i = 0; i < PointsCount; i++)
                    {
                        var index = i + 1;
                        var startPoint = new ChartPoint(xValues[i], YValues[i]);
                        previewYValues.Add(YValues[i]);
                        if (index >= PointsCount) continue;
                        var endPoint = new ChartPoint(xValues[index], YValues[index]);
                        ChartPoint startControlPoint;
                        ChartPoint endControlPoint;
                        //Calculate curve points. 
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
                        var splineSegment = new SplineSegment(startPoint, startControlPoint, endControlPoint, endPoint, this);
                        splineSegment.SetData(startPoint, startControlPoint, endControlPoint, endPoint);
                        var segment = splineSegment.CreateVisual(Size.Empty);
                        splineSegment.Update(chartTransformer);

                        if (CustomTemplate == null)
                        {
                            var segmentPath = segment as Path;
                            segmentPath.Stroke = ((Path)Segments[0].GetRenderedVisual()).Stroke;
                            segmentPath.StrokeThickness = StrokeWidth;
                            segment.Opacity = ((Path)Segments[0].GetRenderedVisual()).StrokeThickness;
                            segment.Opacity = 0.5;
                            segments.Add(splineSegment);
                            SeriesPanel.Children.Add(segment);
                        }
                        else
                        {
                            segments.Add(splineSegment);
                            SeriesPanel.Children.Add(segment);
                        }
                    }
                }
                else
                {
                    if (Type == SplineType.Monotonic)
                        GetMonotonicSpline(xValues, previewYValues);//Calculate monotone curve points.
                    else if (Type == SplineType.Cardinal)
                        GetCardinalSpline(xValues, YValues);
                    else
                        NaturalSpline(xValues, previewYValues, out yCoef);

                    var chartTransformer = this.ActualArea != null ? CreateTransformer(GetAvailableSize(), true) : null;
                    if (chartTransformer == null) return;

                    for (var i = 0; i < PointsCount; i++)
                    {
                        var index = i + 1;
                        var startPoint = new ChartPoint(xValues[i], previewYValues[i]);
                        if (index >= PointsCount) continue;
                        var endPoint = new ChartPoint(xValues[index], previewYValues[index]);
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

                        segments[i].SetData(startPoint, startControlPoint, endControlPoint, endPoint);
                        segments[i].Update(chartTransformer);
                    }
                }
            }
            catch
            {

            }
        }

#endregion

#endregion
    }
}
