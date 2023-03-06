using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="StackedLineSeries"/> is a special kind of line series which is similar to regular line series except that the Y-values stack on top of each other.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="StackedLineSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="StrokeDashArray"/>, <see cref="XyDataSeries.StrokeWidth"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="StackedLineSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="StackedLineSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
    /// <para> <b>Selection - </b> To enable the data point selection in a chart, create an instance of <see cref="DataPointSelectionBehavior"/> and set it to the <see cref="StackedLineSeries.SelectionBehavior"/> property of the chart series. To highlight the selected segment data label, set the value for <see cref="ChartSelectionBehavior.SelectionBrush"/> property in <see cref="DataPointSelectionBehavior"/>.</para>
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
    ///           <chart:StackedLineSeries ItemsSource="{Binding Data}"
    ///                                    XBindingPath="XValue"
    ///                                    YBindingPath="YValue"/>
    ///
    ///           <chart:StackedLineSeries ItemsSource="{Binding Data}"
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
    ///     StackedLineSeries series = new StackedLineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    /// 
    ///     StackedLineSeries series1 = new StackedLineSeries();
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
    /// <seealso cref="LineSegment"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class StackedLineSeries : StackedSeriesBase
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the StrokeDashArray dependency property.
        /// </summary>
        /// <value>
        /// The identifier for StrokeDashArray dependency property.
        /// </value>
        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register(
                nameof(StrokeDashArray),
                typeof(DoubleCollection),
                typeof(StackedLineSeries),
                new PropertyMetadata(null));

        #endregion

        #region Fields

        private Storyboard sb;

        Point hitPoint = new Point();

        private RectAnimation animation;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the stroke dash array to customize the appearance of a line stroke.
        /// </summary>
        /// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:StackedLineSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   YBindingPath="YValue"
        ///                                   StrokeDashArray="5,3" />
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
        ///     StackedLineSeries series = new StackedLineSeries()
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

        #region Protected Override Properties

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            return new LineSegment();
        }

        /// <inheritdoc/>
        internal override bool IsStacked
        {
            get
            {
                return true;
            }
        }

        #endregion

        //#region Public Override Methods

        /// <summary>
        /// Creates the segments of <see cref="StackedLineSeries"/>.
        /// </summary>
        internal override void GenerateSegments()
        {
            var index = -1;
            List<double> xValues = null;
            var isGrouped = ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed;
            double Origin = 0;

            var stackingValues = GetCumulativeStackValues(this);
            if (isGrouped)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();
            if (stackingValues != null)
            {
                YRangeStartValues = stackingValues.StartValues;
                YRangeEndValues = stackingValues.EndValues;

                if (YRangeStartValues == null)
                {
                    YRangeStartValues = (from val in xValues select Origin).ToList();
                }

                if (xValues != null)
                {
                    if (isGrouped)
                    {
                        Segments.Clear();
                        Adornments.Clear();
                        for (int i = 0; i < xValues.Count; i++)
                        {
                            index = i + 1;
                            if (GroupedSeriesYValues != null)
                            {
                                if (index < xValues.Count)
                                   CreateSegment(new[] { xValues[i], YRangeEndValues[i], xValues[index], YRangeEndValues[index] }, ActualData[i]);

                                if (AdornmentsInfo != null && ShowDataLabels)
                                {
                                    Adornments.Add(this.CreateAdornment(this, xValues[i], YRangeEndValues[i], xValues[i], YRangeEndValues[i]));
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
                                    (Segments[i]).SetData(xValues[i], YRangeEndValues[i], xValues[index], YRangeEndValues[index]);
                                    (Segments[i] as LineSegment).Item = ActualData[i];
                                    (Segments[i] as LineSegment).YData = YRangeEndValues[i];
                                }
                                else
                                    Segments.RemoveAt(i);
                            }
                            else
                            {
                                if (index < this.PointsCount)
                                   CreateSegment(new[] { xValues[i], YRangeEndValues[i], xValues[index], YRangeEndValues[index] }, ActualData[i]);
                            }

                        if (AdornmentsInfo != null && ShowDataLabels)
                            {
                                if (i < Adornments.Count)
                                {
                                    Adornments[i].SetData(xValues[i], YRangeEndValues[i], xValues[i], YRangeEndValues[i]);
                                }
                                else
                                {
                                    Adornments.Add(this.CreateAdornment(this, xValues[i], YRangeEndValues[i], xValues[i], YRangeEndValues[i]));
                                }

                                Adornments[i].Item = ActualData[i];
                            }
                            
                        }
                    }
                }
            }
        }

        #region Internal Override Methods

        /// <summary>
        /// This method used to gets the chart data point at a position.
        /// </summary>
        /// <param name="mousePos">Point</param>
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
                        chartSegment = index != -1 ? new LineSegment() : null;
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
                    if (ActualXAxis is CategoryAxis && (ActualXAxis as CategoryAxis).IsIndexed)
                        index = YValues.IndexOf(yVal);
                    else
                        index = this.GetXValues().IndexOf(xVal);

                    var seriesCollection = Chart.GetSeriesCollection();
                    foreach (var series in seriesCollection)
                    {
                        if (series is StackedLineSeries lineSeries && lineSeries == this && index >= 0)
                            data = this.ActualData[index];
                    }
                }

                if (data == null) return; // To ignore tooltip when mousePos is not inside the SeriesClipRect.
                if (this.Chart.Tooltip == null)
                    this.Chart.Tooltip = new ChartTooltip();
                var chartTooltip = this.Chart.Tooltip as ChartTooltip;

                var stackingLineSegment = chartSegment as LineSegment;
                stackingLineSegment.Item = data;
                stackingLineSegment.XData = xVal;
                stackingLineSegment.YData = yVal;
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
            LineSegment lineSegment = ToolTipTag as LineSegment;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(lineSegment.XData, YRangeEndValues[ActualData.IndexOf(lineSegment.Item)]);
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
                        BeginTime = TimeSpan.FromSeconds(i * secondsPerPoint)
                    };
                    keyFrames1.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(AnimationDuration.TotalSeconds / 2));

                    keyFrames1.EnableDependentAnimation = true;
                    Storyboard.SetTargetProperty(keyFrames1, adornTransXPath);
                    Storyboard.SetTarget(keyFrames1, label);
                    sb.Children.Add(keyFrames1);
                    keyFrames1 = new DoubleAnimation()
                    {
                        From = 0.6,
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

        #region Private Static Methods

        /// <summary>
        /// Add the <see cref="LineSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="actualData">The actualData.</param>
        private void CreateSegment(double[] values, object actualData)
        {
            var segment = CreateSegment() as LineSegment;
            if (segment != null)
            {
                segment.Series = this;
                segment.Item = actualData;
                segment.SetData(values);
                Segments.Add(segment);
            }
        }

        #endregion
    }
}
