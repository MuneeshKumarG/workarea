using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="StackedColumnSeries"/> is a special kind of column series which is similar to the regular column series except that the Y-values stack on top of each other.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="StackedColumnSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="Stroke"/>, <see cref="ChartSeries.StrokeThickness"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="StackedColumnSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="StackedColumnSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
    /// <para> <b>Selection - </b> To enable the data point selection in a chart, create an instance of <see cref="DataPointSelectionBehavior"/> and set it to the <see cref="StackedColumnSeries.SelectionBehavior"/> property of the chart series. To highlight the selected segment data label, set the value for <see cref="ChartSelectionBehavior.SelectionBrush"/> property in <see cref="DataPointSelectionBehavior"/>.</para>
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
    ///           <chart:StackedColumnSeries ItemsSource="{Binding Data}"
    ///                                      XBindingPath="XValue"
    ///                                      YBindingPath="YValue"/>
    ///                   
    ///           <chart:StackedColumnSeries ItemsSource="{Binding Data}"
    ///                                      XBindingPath="XValue"
    ///                                      YBindingPath="YValue1"/>
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
    ///     StackedColumnSeries series = new StackedColumnSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    /// 
    ///     StackedColumnSeries series1 = new StackedColumnSeries();
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
    /// <seealso cref="StackedColumnSegment"/>
    public class StackedColumnSeries : StackedSeriesBase, ISegmentSpacing
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the SegmentSpacing dependency property.
        /// </summary>
        /// <value>
        /// The identifier for SegmentSpacing dependency property.
        /// </value>
        public static readonly DependencyProperty SegmentSpacingProperty =
            DependencyProperty.Register(
                nameof(SegmentSpacing),
                typeof(double),
                typeof(StackedColumnSeries),
                new PropertyMetadata(0.0, OnSegmentSpacingChanged));


        /// <summary>
        /// Identifies the CustomTemplate dependency property.
        /// </summary>
        /// <value>
        /// The identifier for CustomTemplate dependency property.
        /// </value>
        public static readonly DependencyProperty CustomTemplateProperty =
            DependencyProperty.Register(
                nameof(CustomTemplate), 
                typeof(DataTemplate),
                typeof(StackedColumnSeries),
                new PropertyMetadata(null, OnCustomTemplateChanged));


        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(StackedColumnSeries),
            new PropertyMetadata(null, OnStrokeChanged));

        #endregion

        #region Fields

        #region Private Fields

        private Storyboard sb;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that can be used to change the spacing between two segments.
        /// </summary>
        /// <value>It accepts double values and the default value is 0. Here, the value ranges from 0 to 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:StackedColumnSeries ItemsSource="{Binding Data}"
        ///                                     XBindingPath="XValue"
        ///                                     YBindingPath="YValue"
        ///                                     SegmentSpacing = "0.3"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     StackedColumnSeries series = new StackedColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           SegmentSpacing = 0.3,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double SegmentSpacing
        {
            get { return (double)GetValue(SegmentSpacingProperty); }
            set { SetValue(SegmentSpacingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate that can be used to customize the appearence of coulum shape. 
        /// </summary>
        /// <value>
        /// The template that specifies the shape inside the canvas. The default is null.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///          <!-- ... Eliminated for simplicity-->
        ///          <chart:StackedColumnSeries ItemsSource="{Binding Data}"
        ///                                     XBindingPath="XValue"
        ///                                     YBindingPath="YValue">
        ///              <chart:StackedColumnSeries.CustomTemplate>
        ///                  <DataTemplate>
        ///                      <Canvas>
        ///                          <Rectangle Fill = "{Binding Fill}" Width="{Binding Width}" Height="{Binding Height}"  
        ///                                     Canvas.Left="{Binding RectX}" Canvas.Top="{Binding RectY}" Stroke="{Binding Stroke}"/>
        ///                      </Canvas>
        ///                  </DataTemplate>
        ///              </chart:StackedColumnSeries.CustomTemplate>
        ///          </chart:StackedColumnSeries>
        ///
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

        /// Gets or sets a value to customize the stroke appearance of a chart series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:StackedColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red"
        ///                            StrokeWidth = "3"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     StackedColumnSeries stackedColumnSeries = new StackedColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth= 3,
        ///     };
        ///     
        ///     chart.Series.Add(stackedColumnSeries);
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

        #region Protected Internal Override Properties

        /// <summary>
        /// Gets a value indicating whether this series is placed side by side.
        /// </summary>
        /// <returns>
        /// It returns <c>true</c>, if the series is placed side by side [cluster mode].
        /// </returns>
        internal override bool IsSideBySide
        {
            get { return true; }
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

        #endregion

        #region Methods

        #region Interface Methods

        /// <inheritdoc/>
        double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
        {
            return CalculateSegmentSpacing(spacing, Right, Left);
        }

        #endregion

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of <see cref="StackedColumnSeries"/>.
        /// </summary>
        internal override void GenerateSegments()
        {
            List<double> xValues = null;
            var isGrouped = ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed;
            DoubleRange sbsInfo = this.GetSideBySideInfo(this);
            double x1, x2, y1, y2;
            double Origin = 0;

            double median = sbsInfo.Delta / 2;
            if (isGrouped)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();
            var stackingValues = GetCumulativeStackValues(this);
            if (stackingValues != null)
            {
                YRangeStartValues = stackingValues.StartValues;
                YRangeEndValues = stackingValues.EndValues;
#if NETFX_CORE 
                // Designer crashes when rebuild the SfChart
                bool _isInDesignMode = Windows.ApplicationModel.DesignMode.DesignModeEnabled;
                if (_isInDesignMode)
                    if (YRangeStartValues.Count == 0 && YRangeEndValues.Count == 0)
                        return;
#endif
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
                        int segmentCount = 0;

                        for (int i = 0; i < DistinctValuesIndexes.Count; i++)
                        {
                            for (int j = 0; j < DistinctValuesIndexes[i].Count; j++)
                            {
                                int index = DistinctValuesIndexes[i][j];
                                if (j < xValues.Count)
                                {
                                    x1 = i + sbsInfo.Start;
                                    x2 = i + sbsInfo.End;
                                    y2 = double.IsNaN(YRangeStartValues[segmentCount]) ? Origin : YRangeStartValues[segmentCount];
                                    y1 = double.IsNaN(YRangeEndValues[segmentCount]) ? Origin : YRangeEndValues[segmentCount];

                                    StackedColumnSegment columnSegment = CreateSegment() as StackedColumnSegment;
                                    if (columnSegment != null)
                                    {
                                        columnSegment.Series = this;
                                        columnSegment.SetData(x1, y1, x2, y2);
                                        columnSegment.customTemplate = CustomTemplate;
                                        columnSegment.XData = xValues[segmentCount];
                                        columnSegment.YData = GroupedSeriesYValues[0][index];
                                        columnSegment.Item = GroupedActualData[segmentCount];
                                        Segments.Add(columnSegment);
                                    }

                                    if (AdornmentsInfo != null && ShowDataLabels)
                                    {
                                        BarLabelAlignment markerPosition = this.adornmentInfo.GetAdornmentPosition();
                                        if (markerPosition == BarLabelAlignment.Top)
                                            AddColumnAdornments(i, GroupedSeriesYValues[0][index], x1, y1, segmentCount, median);
                                        else if (markerPosition == BarLabelAlignment.Bottom)
                                            AddColumnAdornments(i, GroupedSeriesYValues[0][index], x1, y2, segmentCount, median);
                                        else
                                            AddColumnAdornments(i, GroupedSeriesYValues[0][index], x1, y1 + (y2 - y1) / 2, segmentCount, median);
                                    }

                                    segmentCount++;
                                }
                            }
                        }
                    }
                    else
                    {
                        ClearUnUsedSegments(this.PointsCount);
                        ClearUnUsedAdornments(this.PointsCount);
                        for (int i = 0; i < PointsCount; i++)
                        {
                            x1 = xValues[i] + sbsInfo.Start;
                            x2 = xValues[i] + sbsInfo.End;
                            y2 = double.IsNaN(YRangeStartValues[i]) ? Origin : YRangeStartValues[i];
                            y1 = double.IsNaN(YRangeEndValues[i]) ? Origin : YRangeEndValues[i];

                            if (i < Segments.Count)
                            {
                                (Segments[i]).Item = ActualData[i];
                                (Segments[i] as StackedColumnSegment).XData = xValues[i];
                                (Segments[i] as StackedColumnSegment).YData = YValues[i];
                                (Segments[i]).SetData(x1, y1, x2, y2);
                            }
                            else
                            {
                                StackedColumnSegment columnSegment = CreateSegment() as StackedColumnSegment;
                                if (columnSegment != null)
                                {
                                    columnSegment.Series = this;
                                    columnSegment.SetData(x1, y1, x2, y2);
                                    columnSegment.customTemplate = CustomTemplate;
                                    columnSegment.XData = xValues[i];
                                    columnSegment.YData = ActualSeriesYValues[0][i];
                                    columnSegment.Item = ActualData[i];
                                    Segments.Add(columnSegment);
                                }
                            }

                            if (AdornmentsInfo != null && ShowDataLabels)
                            {
                                BarLabelAlignment markerPosition = this.adornmentInfo.GetAdornmentPosition();
                                if (markerPosition == BarLabelAlignment.Top)
                                    AddColumnAdornments(xValues[i], YValues[i], x1, y1, i, median);
                                else if (markerPosition == BarLabelAlignment.Bottom)
                                    AddColumnAdornments(xValues[i], YValues[i], x1, y2, i, median);
                                else
                                    AddColumnAdornments(xValues[i], YValues[i], x1, y1 + (y2 - y1) / 2, i, median);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Internal Override Methods

        internal override bool GetAnimationIsActive()
        {
            return sb != null && sb.GetCurrentState() == ClockState.Active;
        }

        internal override void Animate()
        {
            int i = 0;

            // WPF-25124 Animation not working properly when resize the window.
            if (sb != null)
            {
                sb.Stop();
                if (!canAnimate)
                {
                    ResetAdornmentAnimationState();
                    return;
                }
            }

            sb = new Storyboard();
            string path = IsActualTransposed ? "(Canvas.Left)" : "(Canvas.Top)";
            string scalePath = IsActualTransposed ? "(UIElement.RenderTransform).(ScaleTransform.ScaleX)" : "(UIElement.RenderTransform).(ScaleTransform.ScaleY)";
            string adornTransPath = IsActualTransposed ? "(UIElement.RenderTransform).(TranslateTransform.X)" : "(UIElement.RenderTransform).(TranslateTransform.Y)";
            double startHeight = Chart.ActualValueToPoint(Chart.InternalSecondaryAxis, 0);
            foreach (ChartSegment segment in Segments)
            {
                double elementSize;
                var element = (FrameworkElement)segment.GetRenderedVisual();
                if (element == null) return;

                var columnSegment = segment as ColumnSegment;
                elementSize = IsActualTransposed
                    ? columnSegment.Width
                    : columnSegment.Height;

                if (!double.IsNaN(elementSize))
                {
                    element.RenderTransform = new ScaleTransform();
                    double canvasSize = IsActualTransposed ? Canvas.GetLeft(element) : Canvas.GetTop(element);
                    DoubleAnimationUsingKeyFrames keyFrames = new DoubleAnimationUsingKeyFrames();
                    SplineDoubleKeyFrame keyFrame = new SplineDoubleKeyFrame();
                    keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                    keyFrame.Value = startHeight;
                    keyFrames.KeyFrames.Add(keyFrame);

                    keyFrame = new SplineDoubleKeyFrame();
                    keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(AnimationDuration);
                    keyFrame.Value = canvasSize;
                    KeySpline keySpline = new KeySpline();
                    keySpline.ControlPoint1 = new Point(0.64, 0.84);
                    keySpline.ControlPoint2 = new Point(0.67, 0.95);
                    keyFrame.KeySpline = keySpline;

                    keyFrames.KeyFrames.Add(keyFrame);
                    Storyboard.SetTargetProperty(keyFrames, path);
                    Storyboard.SetTarget(keyFrames, element);
                    sb.Children.Add(keyFrames);

                    DoubleAnimationUsingKeyFrames keyFrames1 = new DoubleAnimationUsingKeyFrames();
                    SplineDoubleKeyFrame keyFrame1 = new SplineDoubleKeyFrame();
                    keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                    keyFrame1.Value = 0;
                    keyFrames1.KeyFrames.Add(keyFrame1);
                    keyFrame1 = new SplineDoubleKeyFrame();
                    keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(AnimationDuration);

                    KeySpline keySpline1 = new KeySpline();
                    keySpline1.ControlPoint1 = new Point(0.64, 0.84);
                    keySpline1.ControlPoint2 = new Point(0.67, 0.95);
                    keyFrame1.KeySpline = keySpline1;
                    keyFrames1.KeyFrames.Add(keyFrame1);
                    keyFrame1.Value = 1;

                    keyFrames1.EnableDependentAnimation = true;
                    Storyboard.SetTargetProperty(keyFrames1, scalePath);
                    Storyboard.SetTarget(keyFrames1, element);
                    sb.Children.Add(keyFrames1);
                    if (this.AdornmentsInfo != null && AdornmentsInfo.Visible)
                    {
                        FrameworkElement label = this.AdornmentsInfo.LabelPresenters[i];
                        label.RenderTransform = new TranslateTransform() { };
                        keyFrames1 = new DoubleAnimationUsingKeyFrames();
                        keyFrame1 = new SplineDoubleKeyFrame();
                        keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 80) / 100));
                        keyFrame1.Value = (YValues[i] > 0) ? (elementSize * 10) / 100 : -(elementSize * 10) / 100;
                        keyFrames1.KeyFrames.Add(keyFrame1);
                        keyFrame1 = new SplineDoubleKeyFrame();
                        keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(AnimationDuration);

                        keySpline1 = new KeySpline();
                        keySpline1.ControlPoint1 = new Point(0.64, 0.84);
                        keySpline1.ControlPoint2 = new Point(0.67, 0.95);

                        keyFrame1.KeySpline = keySpline1;
                        keyFrames1.KeyFrames.Add(keyFrame1);
                        keyFrame1.Value = 0;

                        keyFrames1.EnableDependentAnimation = true;
                        Storyboard.SetTargetProperty(keyFrames1, adornTransPath);
                        Storyboard.SetTarget(keyFrames1, label);
                        sb.Children.Add(keyFrames1);
                        label.Opacity = 0;

                        DoubleAnimation animation = new DoubleAnimation()
                        {
                            From = 0,
                            To = 1,                           
                            BeginTime = TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 80) / 100)
                        };
                        animation.Duration = new Duration().GetDuration(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 20) / 100));
                        Storyboard.SetTarget(animation, label);
                        Storyboard.SetTargetProperty(animation, "(UIElement.Opacity)");
                        sb.Children.Add(animation);
                    }

                    i++;
                }
            }

            sb.Begin();
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            StackedColumnSegment stackingColumnSegment = ToolTipTag as StackedColumnSegment;
            Point newPosition = new Point();
            Point point;
            if (IsActualTransposed)
            {
                point = ChartTransformer.TransformToVisible(ActualXAxis.IsInversed ? stackingColumnSegment.XRange.Start : stackingColumnSegment.XRange.End, stackingColumnSegment.YRange.Median);
            }
            else
            {
                point = ChartTransformer.TransformToVisible(stackingColumnSegment.XRange.Median, stackingColumnSegment.YRange.End);
            }

            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top;
            return newPosition;
        }

        internal override void Dispose()
        {
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
            return new StackedColumnSegment();
        }

#endregion

#region Protected Methods

        ///<summary>
        ///Method used to calculate the segment spacing.
        ///</summary>
        /// <param name="spacing">Segment spacing</param>
        /// <param name="Right">Segment right value</param>
        /// <param name="Left">Segment left value</param>
        /// <returns>Returns the segment spacing value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        internal double CalculateSegmentSpacing(double spacing, double Right, double Left)
        {
            double diff = Right - Left;
            double totalspacing = diff * spacing / 2;
            Left = Left + totalspacing;
            Right = Right - totalspacing;
            return Left;
        }

#endregion

#region Private Static Methods

        private static void OnSegmentSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as StackedColumnSeries;
            if (series.Chart != null)
                series.Chart.ScheduleUpdate();
        }
        
        private static void OnCustomTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as StackedColumnSeries;

            if (series.Chart == null) return;

            series.Segments.Clear();
            series.Chart.ScheduleUpdate();
        }

#endregion

#endregion
    }
}
