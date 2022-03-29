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
using ChartAdornment = Syncfusion.UI.Xaml.Charts.ChartDataLabel;
using ChartAdornmentInfo = Syncfusion.UI.Xaml.Charts.ChartDataLabelSettings;
using ChartAdornmentContainer = Syncfusion.UI.Xaml.Charts.ChartDataMarkerContainer;
using SelectionStyle = Syncfusion.UI.Xaml.Charts.SelectionType;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Line chart displays series as a set of points connected using a straight line to represent large amounts of data observed over a continuous period of time.
    /// </summary>
    /// <remarks>
    /// LineChart appearance can be customized by using <see cref="LineSeries.CustomTemplate"/> property.
    /// </remarks>
    /// <example>
    /// <code language = "XAML" >
    ///       &lt;syncfusion:LineSeries ItemsSource="{Binding Data}" XBindingPath="Year" YBindingPath="Value"&gt;
    ///       &lt;/syncfusion:LineSeries&gt;
    /// </code>
    /// <code language= "C#" >
    ///       LineSeries series1 = new LineSeries();
    ///       series1.ItemsSource = viewmodel.Data;
    ///       series1.XBindingPath = "Year";
    ///       series1.YBindingPath = "Value";
    ///       chart.Series.Add(series1);
    /// </code>
    /// </example>
    /// <seealso cref="LineSegment"/>
    /// <seealso cref="FastLineSeries"/>
    /// <seealso cref="FastLineBitmapSeries"/>
    /// <seealso cref="SplineSeries"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class LineSeries : XySeriesDraggingBase, ISegmentSelectable
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
        /// The DependencyProperty for <see cref="SelectedIndex"/> property.       
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(
                nameof(SelectedIndex), 
                typeof(int), 
                typeof(LineSeries), 
                new PropertyMetadata(-1, OnSelectedIndexChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="SelectionBrush"/> property.       
        /// </summary>
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(
                nameof(SelectionBrush),
                typeof(Brush),
                typeof(LineSeries),
                new PropertyMetadata(null, OnSegmentSelectionBrush));

        /// <summary>
        /// The Dependency property for<see cref="StrokeDashArray"/> property.
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

        Line previewCurrLine, previewPreLine;

        double offsetPosition, initialPosition;

        PointCollection pointCollection;

        bool isReversed, isDragged;

        bool hasTemplate;

        Point hitPoint = new Point();

        private RectAnimation animation;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the custom template for this series.
        /// </summary>
        /// <example>
        /// This example, we are using <see cref="ScatterSeries"/>.
        /// </example>
        /// <example>
        ///     <code language="XAML">
        ///         &lt;syncfusion:ScatterSeries ItemsSource="{Binding Demands}" XBindingPath="Demand" YBindingPath="Year2010" 
        ///                        ScatterHeight="40" ScatterWidth="40"&gt;
        ///            &lt;syncfusion:ScatterSeries.CustomTemplate&gt;
        ///                 &lt;DataTemplate&gt;
        ///                     &lt;Canvas&gt;
        ///                        &lt;Path Data="M20.125,32l0.5,12.375L10.3125,12.375L10.3125,0.5L29.9375,0.5L29.9375,12.375L39.75,12.375Z" Stretch="Fill"
        ///                            Fill="{Binding Interior}" Height="{Binding ScatterHeight}" Width="{Binding ScatterWidth}" 
        ///                            Canvas.Left="{Binding RectX}" Canvas.Top="{Binding RectY}"/&gt;
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


        /// <summary>
        /// Gets or sets the stroke dash array for line to customize the appearance of LineSeries. This is a bindable property.
        /// </summary>
        /// <value>
        /// <see cref="DoubleCollection"/>.
        /// </value>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
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
        /// It returns list of <see cref="ChartSegment"/>.
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
        /// Creates the segments of LineSeries.
        /// </summary>
        public override void CreateSegments()
        {
            int index = -1;
            List<double> xValues = null;
            bool isGrouping = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
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
                    ClearUnUsedSegments(this.DataCount);
                    ClearUnUsedAdornments(this.DataCount);
                    for (int i = 0; i < this.DataCount; i++)
                    {
                        index = i + 1;
                        if (i < Segments.Count)
                        {
                            Segments[i].Item = ActualData[i];
                            if (index < this.DataCount)
                            {
                                (Segments[i]).SetData(xValues[i], YValues[i], xValues[index], YValues[index]);
                                (Segments[i] as LineSegment).Item = ActualData[i];
                                (Segments[i] as LineSegment).YData = YValues[i];
                                if (SegmentColorPath != null && !Segments[i].IsEmptySegmentInterior && ColorValues.Count > 0 && !Segments[i].IsSelectedSegment)
                                    Segments[i].Interior = (Interior != null) ? Interior : ColorValues[i];
                            }
                            else
                                Segments.RemoveAt(i);
                        }
                        else
                        {
                            if (index < this.DataCount)
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

                if (ShowEmptyPoints)
                    UpdateEmptyPointSegments(xValues, false);
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
                                    PreviousSelectedSegment = null,
                                    PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : null,
                                    NewPointInfo = Segments[newIndex].Item,
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
                                    PreviousSelectedSegment = null,
                                    PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : null,
                                    NewPointInfo = Segments[newIndex - 1].Item,
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

                        chartSelectionChangedEventArgs = (new ChartSelectionChangedEventArgs()
                        {
                            SelectedSegment = null,
                            SelectedSegments = Area.SelectedSegments,
                            SelectedSeries = null,
                            SelectedIndex = newIndex,
                            PreviousSelectedIndex = PreviousSelectedIndex,
                            PreviousSelectedSegment = Segments[PreviousSelectedIndex],
                            PreviousSelectedSeries = this,
                            OldPointInfo = Segments[PreviousSelectedIndex].Item,
                            IsSelected = false
                        });

                        if (PreviousSelectedIndex != -1 && PreviousSelectedIndex < Segments.Count)
                            selectionChangedEventArgs.PreviousSelectedSegment = GetDataPoint(PreviousSelectedIndex);

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

        internal override void UpdateTooltip(object originalSource)
        {
            var canvas = this.Area.GetAdorningCanvas();
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
                    };
                }

                var lineSeg = chartSegment as LineSegment;
                SetTooltipSegmentItem(lineSeg);
                SetTooltipDuration();
                ToolTipTag = lineSeg;
                var chartTooltip = this.Area.Tooltip as ChartTooltip;

                if (chartTooltip != null)
                {
                    chartTooltip.PolygonPath = " ";
                    chartTooltip.DataContext = lineSeg;

                    if (canvas.Children.Count == 0 || (canvas.Children.Count > 0 && !IsTooltipAvailable(canvas)))
                    {
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
                        }

                        Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                        Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                        _stopwatch.Start();
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
                mousePos.X - this.Area.SeriesClipRect.Left,
                mousePos.Y - this.Area.SeriesClipRect.Top);

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

            var seriesRect = Area.SeriesClipRect;
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

        internal override void ActivateDragging(Point mousePos, object element)
        {
            try
            {
                if (previewCurrLine != null || PreviewSeries != null || hasTemplate) return;
                FrameworkElement chartElement = element as FrameworkElement;
                DraggingSegment = null;

                if (chartElement != null)
                {
                    if (chartElement.Tag is LineSegment) DraggingSegment = chartElement.Tag as LineSegment;
                }

                isReversed = false;
                if (chartElement != null && (DraggingSegment == null
                    && !(chartElement.DataContext is ChartAdornmentContainer 
                         || chartElement.DataContext is ChartAdornment)))
                    return;
                base.ActivateDragging(mousePos, chartElement);
                if (SegmentIndex < 0) return;

                if (EnableSeriesDragging && CustomTemplate == null)
                {
                    var line = Segments[0].GetRenderedVisual() as Line;
                    if (pointCollection == null)
                    {
                        PreviewSeries = new Polyline
                        {
                            Opacity = 0.6,
                            Stroke = line.Stroke,
                            StrokeThickness = line.StrokeThickness
                        };
                        SeriesPanel.Children.Add(PreviewSeries);
                        pointCollection = new PointCollection();
                        (PreviewSeries as Polyline).Points = pointCollection;
                    }

                    pointCollection.Clear();
                    pointCollection.Add(new Point(line.X1, line.Y1));
                    foreach (var segment in Segments)
                    {
                        line = segment.GetRenderedVisual() as Line;
                        pointCollection.Add(new Point(line.X2, line.Y2));
                    }

                    offsetPosition = initialPosition = IsActualTransposed ? mousePos.X : mousePos.Y;
                }
                else if (EnableSeriesDragging && (DraggingSegment != null
                        || chartElement.DataContext is ChartAdornmentInfo
                        || chartElement.DataContext is ChartAdornmentContainer
                        || chartElement.DataContext is ChartAdornment))
                {
                    hasTemplate = true;
                    PreviewSeries = new Polyline();
                    offsetPosition = initialPosition = IsActualTransposed ? mousePos.X : mousePos.Y;
                }
                else if (DraggingSegment != null
                        || chartElement.DataContext is ChartAdornmentContainer
                        || chartElement.DataContext is ChartAdornmentInfo
                        || chartElement.DataContext is ChartAdornment)
                {
                    double segmentPosition = Area.ActualValueToPoint(ActualXAxis, SegmentIndex);
                    if (mousePos.X <= segmentPosition)
                        isReversed = true;
                    if (SegmentIndex == Segments.Count)
                        DraggingSegment = Segments[SegmentIndex - 1] as LineSegment;
                    else
                        DraggingSegment = Segments[SegmentIndex] as LineSegment;
                    isReversed = false;
                }
            }
            catch
            {
                ResetDraggingElements("Exception", true);
            }
        }

        internal override void UpdatePreivewSeriesDragging(Point mousePos)
        {
            ////if(!isTemplateSet) return;
            if (CustomTemplate == null)
            {
                if (IsActualTransposed)
                {
                    double newValue = Area.ActualPointToValue(ActualXAxis, new Point(mousePos.Y, mousePos.X));
                    double baseValue = Area.ActualPointToValue(ActualXAxis, new Point(0, initialPosition));
                    DraggedValue = baseValue - newValue;
                    var dragEvent = new XySeriesDragEventArgs { Delta = DraggedValue, BaseXValue = SegmentIndex };
                    RaiseDragDelta(dragEvent);

                    if (dragEvent.Cancel)
                    {
                        ResetDraggingElements("Cancel", true);
                        return;
                    }

                    var offset = mousePos.X - offsetPosition;

                    for (var i = 0; i < pointCollection.Count; i++)
                    {
                        var x = pointCollection[i].X;
                        var y = pointCollection[i].Y;
                        pointCollection[i] = new Point(x + offset, y);
                    }

                    offsetPosition = mousePos.X;
                    isDragged = true;
                    UpdateSeriesDragValueToolTip(
                        mousePos,
                        Segments[0].Interior,
                        DraggedValue,
                        0,
                        Area.ActualValueToPoint(ActualYAxis, YValues[0]));
                }
                else
                {
                    double newValue = Area.ActualPointToValue(ActualYAxis, mousePos);
                    double baseValue = Area.ActualPointToValue(ActualYAxis, new Point(0, initialPosition));
                    DraggedValue = newValue - baseValue;
                    var dragEvent = new XySeriesDragEventArgs { Delta = DraggedValue, BaseXValue = SegmentIndex };
                    RaiseDragDelta(dragEvent);

                    if (dragEvent.Cancel)
                    {
                        ResetDraggingElements("Cancel", true);
                        return;
                    }

                    var offset = mousePos.Y - offsetPosition;

                    for (var i = 0; i < pointCollection.Count; i++)
                    {
                        var x = pointCollection[i].X;
                        var y = pointCollection[i].Y;
                        pointCollection[i] = new Point(x, y + offset);
                    }

                    offsetPosition = mousePos.Y;
                    isDragged = true;
                    UpdateSeriesDragValueToolTip(
                        mousePos, 
                        Segments[0].Interior,
                        DraggedValue,
                        YValues[0],
                        pointCollection[0].X);
                }
            }
            else
            {
                if (IsActualTransposed)
                {
                    double newValue = Area.ActualPointToValue(ActualXAxis, new Point(mousePos.Y, mousePos.X));
                    double baseValue = Area.ActualPointToValue(ActualXAxis, new Point(0, initialPosition));
                    DraggedValue = baseValue - newValue;
                    var dragEvent = new XySeriesDragEventArgs { Delta = DraggedValue, BaseXValue = SegmentIndex };
                    RaiseDragDelta(dragEvent);

                    if (dragEvent.Cancel)
                    {
                        ResetDraggingElements("Cancel", true);
                        return;
                    }

                    offsetPosition = mousePos.X;
                    isDragged = true;
                    UpdateSeriesDragValueToolTip(
                        mousePos, 
                        Segments[0].Interior, 
                        DraggedValue, 
                        0,
                        Area.ActualValueToPoint(ActualYAxis, YValues[0]));
                }
                else
                {
                    double newValue = Area.ActualPointToValue(ActualYAxis, mousePos);
                    double baseValue = Area.ActualPointToValue(ActualYAxis, new Point(0, initialPosition));
                    DraggedValue = newValue - baseValue;
                    var dragEvent = new XySeriesDragEventArgs { Delta = DraggedValue, BaseXValue = SegmentIndex };
                    RaiseDragDelta(dragEvent);

                    if (dragEvent.Cancel)
                    {
                        ResetDraggingElements("Cancel", true);
                        return;
                    }

                    isDragged = true;
                    offsetPosition = mousePos.Y;
                    UpdateSeriesDragValueToolTip(
                        mousePos,
                        Segments[0].Interior, 
                        DraggedValue,
                        YValues[0],
                       (Segments[0] as LineSegment).X1);
                }
            }
        }

        internal override void UpdatePreviewSegmentDragging(Point mousePos)
        {
            try
            {
                DraggedValue = Area.ActualPointToValue(ActualYAxis, mousePos);
                var dragEvent = new XySegmentDragEventArgs
                {
                    BaseYValue = YValues[SegmentIndex],
                    NewYValue = DraggedValue,
                    Segment = DraggingSegment,
                    Delta = GetActualDelta()
                };

                prevDraggedValue = DraggedValue;
                RaiseDragDelta(dragEvent);
                if (dragEvent.Cancel)
                    return;
                double newHeight = 0d;

                if (CustomTemplate != null)
                    hasTemplate = true;
                else
                {
                    var currentLine = DraggingSegment.GetRenderedVisual() as Line;

                    if (previewCurrLine == null)
                    {
                        previewCurrLine = new Line()
                        {
                            Opacity = 0.6,
                            Stroke = currentLine.Stroke,
                            StrokeThickness = currentLine.StrokeThickness
                        };

                        SeriesPanel.Children.Add(previewCurrLine);

                        previewPreLine = new Line()
                        {
                            Opacity = 0.6,
                            Stroke = currentLine.Stroke,
                            StrokeThickness = currentLine.StrokeThickness
                        };

                        SeriesPanel.Children.Add(previewPreLine);
                    }

                    if (!this.IsActualTransposed)
                    {
                        if (SegmentIndex == 0)
                        {
                            previewCurrLine.Y2 = currentLine.Y2;
                            previewCurrLine.Y1 = newHeight = mousePos.Y;
                        }
                        else if (SegmentIndex == Segments.Count)
                        {
                            previewCurrLine.Y2 = newHeight = mousePos.Y;
                            previewCurrLine.Y1 = currentLine.Y1;
                        }
                        else
                        {
                            Line nextLine = null;
                            if (!isReversed)
                            {
                                nextLine = Segments[SegmentIndex - 1].GetRenderedVisual() as Line;
                                previewCurrLine.Y1 = previewPreLine.Y2 = newHeight = mousePos.Y;
                                previewCurrLine.Y2 = currentLine.Y2;
                                previewPreLine.Y1 = nextLine.Y1;
                            }
                            else
                            {
                                nextLine = Segments[SegmentIndex].GetRenderedVisual() as Line;
                                previewCurrLine.Y1 = currentLine.Y1;
                                previewCurrLine.Y2 = previewPreLine.Y1 = newHeight = mousePos.Y;
                                previewPreLine.Y2 = nextLine.Y2;
                            }

                            previewPreLine.X2 = nextLine.X2;
                            previewPreLine.X1 = nextLine.X1;
                        }

                        previewCurrLine.X1 = currentLine.X1;
                        previewCurrLine.X2 = currentLine.X2;
                    }
                    else
                    {
                        if (SegmentIndex == 0)
                        {
                            previewCurrLine.X2 = currentLine.X2;
                            previewCurrLine.X1 = newHeight = mousePos.X;
                        }
                        else if (SegmentIndex == Segments.Count)
                        {
                            previewCurrLine.X2 = newHeight = mousePos.X;
                            previewCurrLine.X1 = currentLine.X1;
                        }
                        else
                        {
                            Line nextLine = null;
                            if (!isReversed)
                            {
                                nextLine = Segments[SegmentIndex - 1].GetRenderedVisual() as Line;
                                previewCurrLine.X1 = previewPreLine.X2 = newHeight = mousePos.X;
                                previewCurrLine.X2 = currentLine.X2;
                                previewPreLine.X1 = nextLine.X1;
                            }
                            else
                            {
                                nextLine = Segments[SegmentIndex].GetRenderedVisual() as Line;
                                previewCurrLine.X1 = currentLine.X1;
                                previewCurrLine.X2 = previewPreLine.X1 = newHeight = mousePos.X;
                                previewPreLine.X2 = nextLine.X2;
                            }

                            previewPreLine.Y2 = nextLine.Y2;
                            previewPreLine.Y1 = nextLine.Y1;
                        }

                        previewCurrLine.Y1 = currentLine.Y1;
                        previewCurrLine.Y2 = currentLine.Y2;
                    }
                }

                isDragged = true;
                var xPos = Segments.Count == SegmentIndex ? (DraggingSegment as LineSegment).X2Value : (DraggingSegment as LineSegment).X1Value;

                if (DraggingPointIndicator != null)
                {
                    var segmentWidth = DraggingPointIndicator.ActualWidth / 2;
                    var segmentHeight = DraggingPointIndicator.ActualHeight / 2;
                    Canvas.SetTop(DraggingPointIndicator, newHeight - segmentHeight);
                    UpdateSegmentDragValueToolTip(new Point((DraggingSegment as LineSegment).X1Value, mousePos.Y), DraggingSegment, 0, DraggedValue, segmentWidth, segmentHeight);
                    return;
                }

                if (IsActualTransposed)
                    UpdateSegmentDragValueToolTip(new Point(mousePos.X, Area.ActualValueToPoint(ActualXAxis, xPos)), DraggingSegment, 0, DraggedValue, 0, 0);
                else
                    UpdateSegmentDragValueToolTip(new Point(Area.ActualValueToPoint(ActualXAxis, xPos), mousePos.Y), DraggingSegment, 0, DraggedValue, 0, 0);
            }
            catch
            {
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
                            PreviousSelectedSegment = null,
                            PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : null,
                            NewPointInfo = Segments[newIndex].Item,
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
                        selectionChangedEventArgs.PreviousSelectedIndex = chartSelectionChangedEventArgs.SelectedIndex;
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
                            PreviousSelectedSegment = null,
                            PreviousSelectedSeries = oldIndex != -1 ? this.ActualArea.PreviousSelectedSeries : null,
                            NewPointInfo = Segments[newIndex - 1].Item,
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
                        selectionChangedEventArgs.PreviousSelectedIndex = chartSelectionChangedEventArgs.SelectedIndex;
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
                        PreviousSelectedSeries = this,
                        OldPointInfo = Segments[oldIndex].Item,
                        IsSelected = false
                    });
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
                        PreviousSelectedSeries = this,
                        OldPointInfo = Segments[oldIndex - 1].Item,
                        IsSelected = false
                    });
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
            return new LineSegment();
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

        /// <summary>
        /// Resets the dragging elements.
        /// </summary>
        /// <param name="reason">The reason.</param>
        /// <param name="dragEndEvent">if set to <c>true</c>, DragEndEvent will raise.</param>
        protected override void ResetDraggingElements(string reason, bool dragEndEvent)
        {
            hasTemplate = false;
            if (SeriesPanel == null) return;
            base.ResetDraggingElements(reason, dragEndEvent);

            if (SeriesPanel.Children.Contains(previewPreLine))
            {
                SeriesPanel.Children.Remove(previewPreLine);
                SeriesPanel.Children.Remove(previewCurrLine);
            }

            if (SeriesPanel.Children.Contains(PreviewSeries))
            {
                (PreviewSeries as Polyline).Points.Clear();
                SeriesPanel.Children.Remove(PreviewSeries);
            }

            pointCollection = null;
            previewPreLine = null;
            previewCurrLine = null;
            DraggingSegment = null;
            PreviewSeries = null;
            isDragged = false;
            DraggedValue = 0;
        }

        /// <summary>
        /// Called when dragging started.
        /// </summary>
        /// <param name="mousePos">mouse position</param>
        /// <param name="originalSource">original source</param>
        protected override void OnChartDragStart(Point mousePos, object originalSource)
        {
            if (EnableSeriesDragging || EnableSegmentDragging)
            {
                ActivateDragging(mousePos, originalSource);
            }
        }

        /// <summary>
        /// Called when dragging ended.
        /// </summary>
        /// <param name="mousePos">mouse position</param>
        /// <param name="originalSource">original source</param>
        protected override void OnChartDragEnd(Point mousePos, object originalSource)
        {
            UpdateDraggedSource();
            base.OnChartDragEnd(mousePos, originalSource);
        }

#endregion

#region Private Static Methods

        private static void OnCustomTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as LineSeries;

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
            (d as LineSeries).UpdateArea();
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

        private void UpdateDraggedSource()
        {
            try
            {
                if (isDragged)
                {
                    var baseValue = YValues[SegmentIndex];
                    var dragPreviewEnd = new XyPreviewEndEventArgs { BaseYValue = baseValue, NewYValue = DraggedValue };
                    RaisePreviewEnd(dragPreviewEnd);

                    if (dragPreviewEnd.Cancel)
                    {
                        ResetDraggingElements("", false);
                        return;
                    }

                    if (PreviewSeries != null)
                    {
                        for (var i = 0; i < YValues.Count; i++)
                        {
                            YValues[i] = GetSnapToPoint(YValues[i] + DraggedValue);
                        }

                        if (UpdateSource)
                            UpdateUnderLayingModel(YBindingPath, YValues);
                    }
                    else
                    {
                        DraggedValue = GetSnapToPoint(DraggedValue);
                        YValues[SegmentIndex] = DraggedValue;
                        if (UpdateSource && !IsSortData)
                            UpdateUnderLayingModel(YBindingPath, SegmentIndex, DraggedValue);
                    }

                    UpdateArea();
                    RaiseDragEnd(new ChartDragEndEventArgs { BaseYValue = baseValue, NewYValue = DraggedValue });
                }

                ResetDraggingElements("", false);
            }
            catch
            {
                ResetDraggingElements("Exception", true);
            }
        }

#endregion

#endregion
    }
}