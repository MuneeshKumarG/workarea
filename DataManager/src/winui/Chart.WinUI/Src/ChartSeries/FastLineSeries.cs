using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="FastLineSeries"/> is a special kind of line series that can render a collection with a large number of data points.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="FastLineSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XyDataSeries.StrokeWidth"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="FastLineSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="FastLineSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
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
    ///           <chart:FastLineSeries ItemsSource="{Binding Data}"
    ///                                 XBindingPath="XValue"
    ///                                 YBindingPath="YValue"/>
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
    ///     FastLineSeries series = new FastLineSeries();
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
    /// <seealso cref="FastLineSegment"/>
    public class FastLineSeries : XyDataSeries
    {
        #region Dependency Property Registration
        /// <summary>
        /// Identifies the CustomTemplate dependency property.
        /// </summary>
        /// <value>
        /// The identifier for CustomTemplate dependency property.
        /// </value> 
        public static readonly DependencyProperty CustomTemplateProperty =
            DependencyProperty.Register(nameof(CustomTemplate), typeof(DataTemplate), typeof(FastLineSeries),
            new PropertyMetadata(null, OnCustomTemplateChanged));

        #endregion

        #region Fields

        #region Private Fields

        private Storyboard sb;

        private RectAnimation animation;

        private IList<double> xValues;

        bool isAdornmentPending;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the DataTemplate that can be used to customize the appearance of polyline. 
        /// </summary>
        /// <value>
        /// The template that defines the polyline.
        /// </value>
        /// <remarks>
        /// Properties such as StrokeDashArray, StrokeWidth, and Stroke can be used to customize poly lines.
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:FastLineSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue">
        ///               <chart:FastLineSeries.CustomTemplate>
        ///                   <DataTemplate>
        ///                       <Polyline Points = "{Binding Points}" StrokeDashArray="5,3" StrokeWidth="2" Stroke="Red"/>
        ///                   </DataTemplate>
        ///               </chart:FastLineSeries.CustomTemplate>
        ///           </chart:FastLineSeries>
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DataTemplate CustomTemplate
        {
            get { return (DataTemplate)GetValue(CustomTemplateProperty); }
            set { SetValue(CustomTemplateProperty, value); }
        }

        #endregion

        #region Private Properties
        
        private FastLineSegment Segment { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Used to create the segment of <see cref="FastLineSeries"/>.
        /// </summary>
        internal override void GenerateSegments()
        {
            bool isGrouping = this.ActualXAxis is CategoryAxis ? (this.ActualXAxis as CategoryAxis).ArrangeByIndex : true;
            if (!isGrouping)
                xValues = GroupedXValuesIndexes;
            else
                xValues = (ActualXValues is IList<double> && !IsIndexed) ? ActualXValues as IList<double> : GetXValues();
            if (!isGrouping)
            {
                Segments.Clear();
                Adornments.Clear();

                if (Segment == null || Segments.Count == 0)
                {
                    FastLineSegment segment = CreateSegment() as FastLineSegment;
                    if (segment != null)
                    {
                        segment.Series = this;
                        segment.customTemplate = CustomTemplate;
                        segment.SetData(xValues, GroupedSeriesYValues[0]);
                        segment.Item = ActualData;
                        Segment = segment;
                        Segments.Add(segment);
                    }
                }
            }
            else
            {
                ClearUnUsedAdornments(this.PointsCount);

                if (Segment == null || Segments.Count == 0)
                {
                    FastLineSegment segment = CreateSegment() as FastLineSegment;
                    if (segment != null)
                    {
                        segment.Series = this;
                        segment.customTemplate = CustomTemplate;
                        segment.SetData(xValues, YValues);
                        segment.Item = ActualData;
                        Segment = segment;
                        Segments.Add(segment);
                    }
                }
                else if (ActualXValues != null)
                {
                    Segment.SetData(xValues, YValues);
                    Segment.Item = ActualData;
                }
            }

            isAdornmentPending = true;
        }

        #endregion

        #region Internal Override Methods

        internal override void UpdateTooltip(object originalSource)
        {
            if (EnableTooltip)
            {
                ChartDataPointInfo dataPoint = new ChartDataPointInfo();
                if (((originalSource as FrameworkElement) is Polyline))
                {
                    dataPoint = GetDataPoint(mousePosition);
                }
                else
                {
                    int index = ChartExtensionUtils.GetAdornmentIndex(originalSource);
                    if (index > -1)
                    {
                        dataPoint.Index = index;
                        if (xValues.Count > index)
                            dataPoint.XData = xValues[index];
                        if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed
                            && GroupedSeriesYValues[0].Count > index)
                            dataPoint.YData = GroupedSeriesYValues[0][index];
                        else if (YValues.Count > index)
                            dataPoint.YData = YValues[index];
                        dataPoint.Series = this;
                        if (ActualData.Count > index)
                            dataPoint.Item = ActualData[index];
                    }
                }

                UpdateSeriesTooltip(dataPoint);
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
            double startXVal, endXVal;
            List<int> hitIndexes = new List<int>();

            double stackedYValue = double.NaN;

            CalculateHittestRect(mousePos, out startIndex, out endIndex, out rect);

            Point point = new Point(mousePos.X - Chart.SeriesClipRect.Left, mousePos.Y - Chart.SeriesClipRect.Top);

            //WPF - 52504-FastLineSeries tooltip displays wrong information.
            FindNearestChartPoint(point, out startXVal, out endXVal, out stackedYValue);

            int index = xValues.IndexOf(startXVal);
          
            dataPoint = new ChartDataPointInfo();
            dataPoint.Index = index;
            dataPoint.XData = startXVal;
            dataPoint.YData = endXVal;
            if (index > -1 && ActualData.Count > index)
                dataPoint.Item = ActualData[index];
            dataPoint.Series = this;
            
            return dataPoint;
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            ChartDataPointInfo chartPointinfo = ToolTipTag as ChartDataPointInfo;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(chartPointinfo.XData, chartPointinfo.YData);
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
                    var scaleTransform = new ScaleTransform() { ScaleX = 0.6, ScaleY = 0.6 };

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
            if (Segment != null)
            {
                Segment.Dispose();
                Segment = null;
            }

            if (xValues != null)
            {
                xValues.Clear();
                xValues = null;
            }

            if (CustomTemplate != null)
            {
                CustomTemplate = null;
            }

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

        #region Protected Override Methods

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            return new FastLineSegment();
        }


#if NETFX_CORE
        /// <inheritdoc/>
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Touch)
                UpdateTooltip(e.OriginalSource);
        }
#endif

        /// <summary>
        /// Called when <see cref="ChartSeries.ItemsSource"/> property changed.
        /// </summary>
        /// <param name="oldValue">ItemsSource old value.</param>
        /// <param name="newValue">ItemsSource new value</param>
        /// <seealso cref="ChartSeries.XBindingPath"/>
        /// <seealso cref="XyDataSeries.YBindingPath"/>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            Segment = null;
            base.OnDataSourceChanged(oldValue, newValue);
        }

        /// <summary>
        /// Invoked when VisibleRange property changed.
        /// </summary>
        /// <param name="e"><see cref="VisibleRangeChangedEventArgs"/> that contains the event data.</param>
        internal override void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
        {
            if (AdornmentsInfo != null && ShowDataLabels && isAdornmentPending)
            {
                if (xValues != null && ActualXAxis != null
                    && !ActualXAxis.VisibleRange.IsEmpty)
                {
                    double start = ActualXAxis.VisibleRange.Start;
                    double end = ActualXAxis.VisibleRange.End;

                    for (int i = 0; i < PointsCount; i++)
                    {
                        double x, y;
                        if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
                        {
                            if (i < xValues.Count)
                            {
                                y = GroupedSeriesYValues[0][i];
                                x = xValues[i];
                            }
                            else
                                return;
                        }
                        else
                        {
                            x = xValues[i];
                            y = YValues[i];
                        }

                        double edgeValue = x;
                        if (edgeValue >= start && edgeValue <= end && !double.IsNaN(y))
                        {
                            if (i < Adornments.Count)
                            {
                                Adornments[i].SetData(x, y, x, y);
                                Adornments[i].Item = ActualData[i];
                            }
                            else
                            {
                                Adornments.Add(this.CreateAdornment(this, x, y, x, y));
                                Adornments[Adornments.Count - 1].Item = ActualData[i];
                            }
                        }
                    }

                    isAdornmentPending = false;
                }
            }
        }

        #endregion

        #region Private Static Methods

        private static void OnCustomTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as FastLineSeries;

            if (series.Chart == null) return;

            series.Segments.Clear();
            series.Chart.ScheduleUpdate();
        }

        #endregion

        #endregion
    }
}
