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
using ChartAdornment = Syncfusion.UI.Xaml.Charts.ChartDataLabel;
using SelectionStyle = Syncfusion.UI.Xaml.Charts.SelectionType;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// StepLineSeries displays its data points using line segments.
    /// </summary>
    /// <example>
    /// <code language = "XAML" >
    ///       &lt;syncfusion:StepLineSeries ItemsSource="{Binding Data}" XBindingPath="Year" YBindingPath="Value"&gt;
    ///       &lt;/syncfusion:StepLineSeries&gt;
    /// </code>
    /// <code language= "C#" >
    ///       StepLineSeries series1 = new StepLineSeries();
    ///       series1.ItemsSource = viewmodel.Data;
    ///       series1.XBindingPath = "Year";
    ///       series1.YBindingPath = "Value";
    ///       chart.Series.Add(series1);
    /// </code>
    /// </example>
    /// <seealso cref="StepAreaSeries"/>   
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class StepLineSeries : XyDataSeries, ISegmentSelectable
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

        /// <summary>
        /// The DependencyProperty for <see cref="SelectedIndex"/> property.       .
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(
                nameof(SelectedIndex),
                typeof(int), 
                typeof(StepLineSeries),
                new PropertyMetadata(-1, OnSelectedIndexChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="SelectionBrush"/> property.       .
        /// </summary>
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(
                nameof(SelectionBrush),
                typeof(Brush),
                typeof(StepLineSeries),
                new PropertyMetadata(null, OnSegmentSelectionBrush));

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
        /// Gets or sets the custom template for this series.
        /// </summary>
        /// <value>
        /// <see cref="DataTemplate"/>
        /// </value>
        /// <example>
        /// This example, we are using <see cref="ScatterSeries"/>.
        /// </example>
        /// <example>
        ///     <code language="XAML">
        ///         &lt;syncfusion:ScatterSeries ItemsSource="{Binding Demands}" XBindingPath="Demand" ScatterHeight="40" 
        ///                         YBindingPath="Year2010" ScatterWidth="40"&gt;
        ///            &lt;syncfusion:ScatterSeries.CustomTemplate&gt;
        ///                 &lt;DataTemplate&gt;
        ///                     &lt;Canvas&gt;
        ///                        &lt;Path Data="M20.125,32l0.5,12.375L10.3125,12.375L10.3125,0.5L29.9375,0.5L29.9375,12.375L39.75,12.375Z" Stretch="Fill"
        ///                                 Fill="{Binding Interior}" Height="{Binding ScatterHeight}" Width="{Binding ScatterWidth}" 
        ///                                 Canvas.Left="{Binding RectX}" Canvas.Top="{Binding RectY}"/&gt;
        ///                     &lt;/Canvas&gt;
        ///                 &lt;/DataTemplate&gt;
        ///             &lt;/syncfusion:ScatterSeries.CustomTemplate&gt;
        ///         &lt;/syncfusion:ScatterSeries&gt;
        ///     </code>
        /// </example>
        public DataTemplate CustomTemplate
        {
            get { return (DataTemplate)GetValue(CustomTemplateProperty); }
            set { SetValue(CustomTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the selected segment.
        /// </summary>
        /// <value>
        /// <c>Int</c> value represents the index of the data point(or segment) to be selected. 
        /// </value>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interior(brush) for the selected segment(s).
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        #endregion

        #region Protected Internal Override Properties

        /// <summary>
        /// Gets the value which confirms whether this series in linearity.
        /// </summary>
        /// <remarks>
        ///  Returns <c>true</c> if its linear, otherwise it returns <c>false</c>.
        /// </remarks>
        internal override bool IsLinear
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the selected segments in this series, when we enable the multiple selection.
        /// </summary>
        /// <returns>
        /// It returns the list of <see cref="ChartSegment"/>.
        /// </returns>
        protected internal override List<ChartSegment> SelectedSegments
        {
            get
            {
                if (SelectedSegmentsIndexes.Count > 0 && Segments.Count != 0)
                    return (from index in SelectedSegmentsIndexes
                            where index <= Segments.Count
                            select index == Segments.Count ? Segments[index - 1] : Segments[index]).ToList();
                else
                    return null;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of StepLineSeries.
        /// </summary>      
        public override void CreateSegments()
        {
            List<double> xValues = null;
            bool isGrouping = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
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

                    if (AdornmentsInfo != null && ShowDataLabels)
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
            else
            {
                ClearUnUsedStepLineSegment(DataCount);
                ClearUnUsedAdornments(DataCount);
                for (int i = 0; i < DataCount; i++)
                {
                    int index = i + 1;
                    ChartPoint point1, point2, stepPoint;

                    if (AdornmentsInfo != null && ShowDataLabels)
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

                    if (index < DataCount)
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
                        if (SegmentColorPath != null && !Segments[i].IsEmptySegmentInterior && ColorValues.Count > 0 && !Segments[i].IsSelectedSegment)
                            Segments[i].Interior = (Interior != null) ? Interior : ColorValues[i];
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

            if (ShowEmptyPoints)
                UpdateEmptyPointSegments(xValues, false);
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

            var seriesRect = Area.SeriesClipRect;
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
            ChartSelectionChangedEventArgs chartSelectionChangedEventArgs;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null && ActualArea.SelectionBehaviour.EnableMultiSelection)
                    {
                        int oldIndex = PreviousSelectedIndex;

                        int newIndex = (int)e.NewItems[0];

                        if (newIndex >= 0 && ActualArea.GetEnableSegmentSelection())
                        {
                            // Selects the adornment when the mouse is over or clicked on adornments(adornment selection).
                            if (adornmentInfo != null && adornmentInfo.HighlightOnSelection)
                            {
                                UpdateAdornmentSelection(newIndex);
                            }

                            if (ActualArea != null && newIndex < Segments.Count)
                            {
                                chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                                {
                                    SelectedSegment = Segments[newIndex],
                                    SelectedSegments = Area.SelectedSegments,
                                    SelectedSeries = this,
                                    SelectedIndex = newIndex,
                                    PreviousSelectedIndex = oldIndex,
                                    NewPointInfo = Segments[newIndex].Item,
                                    PreviousSelectedSegment = null,
                                    PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : null,
                                    IsSelected = true
                                };

                                if (oldIndex != -1)
                                {
                                    if (oldIndex == Segments.Count)
                                    {
                                        chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[oldIndex - 1];
                                        chartSelectionChangedEventArgs.OldPointInfo = Segments[oldIndex - 1].Item;
                                    }
                                    else
                                    {
                                        chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[oldIndex];
                                        chartSelectionChangedEventArgs.OldPointInfo = Segments[oldIndex].Item;
                                    }
                                }

                                (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
                                PreviousSelectedIndex = newIndex;
                            }
                            else if (ActualArea != null && Segments.Count > 0 && newIndex == Segments.Count)
                            {
                                chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                                {
                                    SelectedSegment = Segments[newIndex - 1],
                                    SelectedSegments = Area.SelectedSegments,
                                    SelectedSeries = this,
                                    SelectedIndex = newIndex,
                                    PreviousSelectedIndex = oldIndex,
                                    NewPointInfo = Segments[newIndex - 1].Item,
                                    PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : null,
                                    PreviousSelectedSegment = null,
                                    IsSelected = true
                                };

                                if (oldIndex != -1)
                                {
                                    if (oldIndex == Segments.Count)
                                    {
                                        chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[oldIndex - 1];
                                        chartSelectionChangedEventArgs.OldPointInfo = Segments[oldIndex - 1].Item;
                                    }
                                    else
                                    {
                                        chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[oldIndex];
                                        chartSelectionChangedEventArgs.OldPointInfo = Segments[oldIndex].Item;
                                    }
                                }

                                (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
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

                    if (e.OldItems != null && ActualArea.SelectionBehaviour.EnableMultiSelection)
                    {
                        int newIndex = (int)e.OldItems[0];

                        chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                        {
                            SelectedSegment = null,
                            SelectedSegments = Area.SelectedSegments,
                            SelectedSeries = null,
                            SelectedIndex = newIndex,
                            PreviousSelectedIndex = PreviousSelectedIndex,
                            PreviousSelectedSegment = Segments[PreviousSelectedIndex],
                            OldPointInfo = Segments[PreviousSelectedIndex].Item,
                            PreviousSelectedSeries = this,
                            IsSelected = false
                        };

                        (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
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
            if (ShowTooltip)
            {
                FrameworkElement element = originalSource as FrameworkElement;
                object chartSegment = null;

                if (element != null)
                {
                    if (element.Tag is ChartSegment)
                        chartSegment = element.Tag;
                    else if (element.DataContext is ChartSegment && !(element.DataContext is ChartAdornment))
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
                                ChartAdornment adornment = Adornments[index];
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
                                    Interior = adornment.Interior,
                                    IsAddedToVisualTree = adornment.IsAddedToVisualTree,
                                    IsEmptySegmentInterior = adornment.IsEmptySegmentInterior,
                                    IsSegmentVisible = adornment.IsSegmentVisible,
                                    IsSelectedSegment = adornment.IsSelectedSegment,
                                    PolygonPoints = adornment.PolygonPoints,
                                    Stroke = adornment.Stroke,
                                    StrokeDashArray = adornment.StrokeDashArray,
                                    StrokeThickness = adornment.StrokeThickness,
                                    XRange = adornment.XRange,
                                    YRange = adornment.YRange,
                                };
                            }
                        }
                    }
                }

                var segment = chartSegment as ChartSegment;
                if (segment == null || segment is TrendlineSegment || segment.Item is Trendline) return;

                // WPF-53847- Tooltip shows the previous data point instead of last data point.
                if (TooltipTemplate != null && segment.Item == ActualData[Segments.Count] && Segments.Contains(segment) && Adornments != null && Adornments.Count == 0)
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
                        Interior = lineSegment.Interior,
                        IsAddedToVisualTree = lineSegment.IsAddedToVisualTree,
                        IsEmptySegmentInterior = lineSegment.IsEmptySegmentInterior,
                        IsSegmentVisible = lineSegment.IsSegmentVisible,
                        IsSelectedSegment = lineSegment.IsSelectedSegment,
                        PolygonPoints = lineSegment.PolygonPoints,
                        Stroke = lineSegment.Stroke,
                        StrokeDashArray = lineSegment.StrokeDashArray,
                        StrokeThickness = lineSegment.StrokeThickness,
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
                var canvas = this.Area.GetAdorningCanvas();
                if (this.Area.Tooltip == null)
                    this.Area.Tooltip = new ChartTooltip();
                var chartTooltip = this.Area.Tooltip as ChartTooltip;
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
                            HastoolTip = true;
                            canvas.Children.Add(chartTooltip);
                        }

                        chartTooltip.ContentTemplate = this.GetTooltipTemplate();
                        AddTooltip();

                        if (ChartTooltip.GetActualEnableAnimation(ActualArea.TooltipBehavior, ChartTooltip.GetEnableAnimation(this)))
                        {
                            SetDoubleAnimation(chartTooltip);
                            _stopwatch.Start();
                            Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                            Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                        }
                        else
                        {
                            Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                            Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                            _stopwatch.Start();
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
            var point = new Point(mousePos.X - this.Area.SeriesClipRect.Left, mousePos.Y - this.Area.SeriesClipRect.Top);

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
        protected internal override void SelectedIndexChanged(int newIndex, int oldIndex)
        {
            ChartSelectionChangedEventArgs chartSelectionChangedEventArgs;
            if (ActualArea != null && ActualArea.SelectionBehaviour != null)
            {
                // Resets the adornment selection when the mouse pointer moved away from the adornment or clicked the same adornment.
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

                if (newIndex >= 0 && ActualArea.GetEnableSegmentSelection())
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
                        chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                        {
                            SelectedSegment = Segments[newIndex],
                            SelectedSegments = Area.SelectedSegments,
                            SelectedSeries = this,
                            SelectedIndex = newIndex,
                            PreviousSelectedIndex = oldIndex,
                            NewPointInfo = Segments[newIndex].Item,
                            PreviousSelectedSegment = null,
                            PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : null,
                            IsSelected = true
                        };

                        if (oldIndex >= 0 && oldIndex <= Segments.Count)
                        {
                            if (oldIndex == Segments.Count)
                            {
                                chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[oldIndex - 1];
                                chartSelectionChangedEventArgs.OldPointInfo = Segments[oldIndex - 1].Item;
                            }
                            else
                            {
                                chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[oldIndex];
                                chartSelectionChangedEventArgs.OldPointInfo = Segments[oldIndex].Item;
                            }
                        }

                        (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
                    }
                    else if (ActualArea != null && Segments.Count > 0 && newIndex == Segments.Count)
                    {
                        chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                        {
                            SelectedSegment = Segments[newIndex - 1],
                            SelectedSegments = Area.SelectedSegments,
                            SelectedSeries = this,
                            SelectedIndex = newIndex,
                            PreviousSelectedIndex = oldIndex,
                            NewPointInfo = Segments[newIndex - 1].Item,
                            PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : null,
                            PreviousSelectedSegment = null,
                            IsSelected = true
                        };

                        if (oldIndex >= 0 && oldIndex <= Segments.Count)
                        {
                            if (oldIndex == Segments.Count)
                            {
                                chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[oldIndex - 1];
                                chartSelectionChangedEventArgs.OldPointInfo = Segments[oldIndex - 1].Item;
                            }
                            else
                            {
                                chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[oldIndex];
                                chartSelectionChangedEventArgs.OldPointInfo = Segments[oldIndex].Item;
                            }
                        }

                        (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
                        PreviousSelectedIndex = newIndex;
                    }
                    else if (Segments.Count == 0)
                    {
                        triggerSelectionChangedEventOnLoad = true;
                    }
                }
                else if (newIndex == -1 && ActualArea != null && oldIndex < Segments.Count)
                {
                    (ActualArea as ChartBase).OnSelectionChanged(new ChartSelectionChangedEventArgs()
                    {
                        SelectedSegment = null,
                        SelectedSegments = Area.SelectedSegments,
                        SelectedSeries = null,
                        SelectedIndex = newIndex,
                        PreviousSelectedIndex = oldIndex,
                        PreviousSelectedSegment = Segments[oldIndex],
                        OldPointInfo = Segments[oldIndex].Item,
                        PreviousSelectedSeries = this,
                        IsSelected = false
                    });
                    PreviousSelectedIndex = newIndex;
                }
                else if (newIndex == -1 && ActualArea != null && Segments.Count > 0 && oldIndex == Segments.Count)
                {
                    (ActualArea as ChartBase).OnSelectionChanged(new ChartSelectionChangedEventArgs()
                    {
                        SelectedSegment = null,
                        SelectedSegments = Area.SelectedSegments,
                        SelectedSeries = null,
                        SelectedIndex = newIndex,
                        PreviousSelectedIndex = oldIndex,
                        PreviousSelectedSegment = Segments[oldIndex - 1],
                        OldPointInfo = Segments[oldIndex - 1].Item,
                        PreviousSelectedSeries = this,
                        IsSelected = false
                    });
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
        protected override ChartSegment CreateSegment()
        {
            return new StepLineSegment();
        }

#if NETFX_CORE
        /// <summary>
        /// Used to update the series tooltip when tapping on the series segments.
        /// </summary>
        /// <param name="e">TappedRoutedEventArgs</param>
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

            if (series.Area == null) return;

            series.Segments.Clear();
            series.Area.ScheduleUpdate();
        }
        
        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartSeries series = d as ChartSeries;
            series.OnPropertyChanged("SelectedIndex");
            if (series.ActualArea == null || series.ActualArea.SelectionBehaviour == null) return;
            if (!series.ActualArea.SelectionBehaviour.EnableMultiSelection)
                series.SelectedIndexChanged((int)e.NewValue, (int)e.OldValue);
            else if ((int)e.NewValue != -1)
                series.SelectedSegmentsIndexes.Add((int)e.NewValue);
        }
        
        private static void OnSegmentSelectionBrush(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as StepLineSeries).UpdateArea();
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
            var emptySegments = new List<ChartSegment>();

            foreach (var segment in Segments.Where(item => item is EmptyPointSegment))
            {
                emptySegments.Add(segment);
            }

            foreach (var segment in emptySegments)
            {
                Segments.Remove(segment);
            }

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
