using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="ColumnSeries"/> displays a set of vertical bars for the given data point values.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="ColumnSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XyDataSeries.StrokeThickness"/>, <see cref="ColumnSeries.Stroke"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="ColumnSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="ColumnSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// <para> <b>SegmentSpacing - </b> To specify the spacing between segments using the <see cref="SegmentSpacing"/> property.</para>
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
    ///           <chart:ColumnSeries ItemsSource="{Binding Data}"
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
    ///     ColumnSeries series = new ColumnSeries();
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
    /// <seealso cref="ColumnSegment"/>
    public class ColumnSeries : XyDataSeries, ISegmentSpacing 
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="SegmentSpacing"/> property. 
        /// </summary>
        public static readonly DependencyProperty SegmentSpacingProperty =
            DependencyProperty.Register(nameof(SegmentSpacing), typeof(double), typeof(ColumnSeries),
            new PropertyMetadata(0.0, OnSegmentSpacingChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="CustomTemplate"/> property. 
        /// </summary>
        public static readonly DependencyProperty CustomTemplateProperty =
            DependencyProperty.Register(nameof(CustomTemplate), typeof(DataTemplate), typeof(ColumnSeries),
            new PropertyMetadata(null, OnCustomTemplateChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(ColumnSeries),
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
        /// Gets or sets a value to indicate spacing between the segments across the series.
        /// </summary>
        /// <value>It accepts double values and the default value is 0. Here, the value is between 0 and 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            SegmentSpacing = "0.3"/>
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
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           SegmentSpacing = 0.3,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
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
        /// Gets or sets the template to customize the appearance of column series.
        /// </summary>
        /// <value>It accepts <see cref="DataTemplate"/> value.</value>
         /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///          <!-- ... Eliminated for simplicity-->
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                                     XBindingPath="XValue"
        ///                                     YBindingPath="YValue">
        ///              <chart:ColumnSeries.CustomTemplate>
        ///                  <DataTemplate>
        ///                      <Canvas>
        ///                          <Rectangle Fill = "{Binding Fill}" Width="{Binding Width}" Height="{Binding Height}"  
        ///                                     Canvas.Left="{Binding RectX}" Canvas.Top="{Binding RectY}" Stroke="{Binding Stroke}"/>
        ///                      </Canvas>
        ///                  </DataTemplate>
        ///              </chart:ColumnSeries.CustomTemplate>
        ///          </chart:ColumnSeries>
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

        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of a chart series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red"
        ///                            StrokeWidth = "3"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth= 3,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
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

        #region Protected Internal Properties

        /// <summary>
        /// Gets a value indicating whether this series is placed side by side.
        /// </summary>
        /// <returns>
        /// It returns <c>true</c>, if the series is placed side by side [cluster mode].
        /// </returns>
        internal override bool IsSideBySide
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

        double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
        {
            return CalculateSegmentSpacing(spacing, Right, Left);
        }

        #endregion

        #region Public Overide Methods

        /// <summary>
        /// Creates the segments of ColumnSeries.
        /// </summary>
        internal override void GenerateSegments()
        {
            double x1, x2, y1, y2;
            var isGrouped = ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).ArrangeByIndex;
            List<double> xValues = null;
            if (isGrouped)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();
            double median = 0d;
            double origin = 0;
           
            if (xValues != null)
            {
                DoubleRange sbsInfo = this.GetSideBySideInfo(this);
                median = sbsInfo.Delta / 2;
                int segmentCount = 0;

                if (isGrouped)
                {
                    Segments.Clear();
                    Adornments.Clear();
                    GroupedActualData.Clear();
                    for (int i = 0; i < DistinctValuesIndexes.Count; i++)
                    {
                        var list = (from index in DistinctValuesIndexes[i]
                                    where GroupedSeriesYValues[0].Count > index
                                    select new List<double> { GroupedSeriesYValues[0][index], index }).
                                    OrderByDescending(val => val[0]).ToList();
                        for (int j = 0; j < list.Count; j++)
                        {
                            var yValue = list[j][0];
                            x1 = i + sbsInfo.Start;
                            x2 = i + sbsInfo.End;
                            y1 = yValue;
                            y2 = origin; // Setting origin value for column segment
                            GroupedActualData.Add(ActualData[(int)list[j][1]]);

                            CreateSegment(new[] { x1, y1, x2, y2 }, xValues[j], yValue, GroupedActualData[segmentCount]);

                            if (AdornmentsInfo != null && ShowDataLabels) 
                            {
                                BarLabelAlignment markerPosition = this.adornmentInfo.GetAdornmentPosition();
                                if (markerPosition == BarLabelAlignment.Top)
                                    AddColumnAdornments(i, yValue, x1, y1, segmentCount, median);
                                else if (markerPosition == BarLabelAlignment.Bottom)
                                    AddColumnAdornments(i, yValue, x1, y2, segmentCount, median);
                                else
                                    AddColumnAdornments(i, yValue, x1, y1 + (y2 - y1) / 2, segmentCount, median);
                            }

                            segmentCount++;
                        }
                    }
                }
                else
                {
                    ClearUnUsedSegments(this.PointsCount);
                    ClearUnUsedAdornments(this.PointsCount);
                    double start = sbsInfo.Start;
                    double end = sbsInfo.End;
                    List<int> SeriesCount = new List<int>();
                    var seriesCollection = Chart.GetSeriesCollection();
                    foreach (ChartSeries series in seriesCollection)
                    {
                        SeriesCount.Add(series.PointsCount);
                    }

                    for (int i = 0; i < this.PointsCount; i++)
                    {
                        if (i < this.PointsCount)
                        {
                            x1 = xValues[i] + start;
                            x2 = xValues[i] + end;
                            y1 = YValues[i];
                            y2 = origin; // Setting origin value for column segment

                            if (i < Segments.Count)
                            {
                                (Segments[i]).SetData(x1, y1, x2, y2);
                                (Segments[i] as ColumnSegment).XData = xValues[i];
                                (Segments[i] as ColumnSegment).YData = YValues[i];
                                (Segments[i] as ColumnSegment).Item = ActualData[i];
                            }
                            else
                            {
                                CreateSegment(new[] { x1, y1, x2, y2 }, xValues[i], YValues[i], ActualData[i]);
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Reviewed")]
        internal override void Animate()
        {
            int i = 0, j = 0;

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
            string path = IsActualTransposed ? "(UIElement.RenderTransform).(ScaleTransform.ScaleX)" : "(UIElement.RenderTransform).(ScaleTransform.ScaleY)";
            string adornTransPath = IsActualTransposed ? "(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)" : "(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)";
            
            foreach (ChartSegment segment in Segments)
            {
                var element = (FrameworkElement)segment.GetRenderedVisual();
                double elementHeight = IsActualTransposed ? ((ColumnSegment)segment).Width : ((ColumnSegment)segment).Height;

                if (!double.IsNaN(elementHeight) && !double.IsNaN(YValues[i]))
                {
                    if (element == null) return;

                    var scaleTransform = new ScaleTransform();
                    element.RenderTransform = scaleTransform;

                    if (this.CustomTemplate != null)
                    {
                        scaleTransform.CenterY = segment.rect.Y + elementHeight;
                    }

                    if (YValues[i] < 0 && IsActualTransposed)
                        element.RenderTransformOrigin = new Point(1, 1);
                    else if (YValues[i] > 0 && !IsActualTransposed)
                        element.RenderTransformOrigin = new Point(1, 1);

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
                    Storyboard.SetTargetProperty(keyFrames1, path);
                    Storyboard.SetTarget(keyFrames1, element);
                    sb.Children.Add(keyFrames1);

                    if (this.AdornmentsInfo != null && AdornmentsInfo.Visible)
                    {
                        FrameworkElement label = this.AdornmentsInfo.LabelPresenters[j];

                        var transformGroup = label.RenderTransform as TransformGroup;
                        var translateTransform = new TranslateTransform();

                        if (transformGroup != null)
                        {
                            if (transformGroup.Children.Count > 0 && transformGroup.Children[0] is TranslateTransform)
                            {
                                transformGroup.Children[0] = translateTransform;
                            }
                            else
                            {
                                transformGroup.Children.Insert(0, translateTransform);
                            }
                        }                      

                        keyFrames1 = new DoubleAnimationUsingKeyFrames();
                        keyFrame1 = new SplineDoubleKeyFrame();
                        keyFrame1.KeyTime = keyFrame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 80) / 100));                         
                        keyFrame1.Value = (YValues[i] > 0) ? (elementHeight * 10) / 100 : -(elementHeight * 10) / 100;
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
                            Duration = new Duration().GetDuration(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 20) / 100)),
                            BeginTime = TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 80) / 100)
                        };

                        Storyboard.SetTarget(animation, label);
                        Storyboard.SetTargetProperty(animation, "(UIElement.Opacity)");
                        sb.Children.Add(animation);
                        j++;
                    }
                }

                i++;
            }

            sb.Begin();
        }
        
        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            ColumnSegment columnSegment = ToolTipTag as ColumnSegment;
            Point newPosition = new Point();
            Point point;
            if (IsActualTransposed)
            {
                point = ChartTransformer.TransformToVisible(ActualXAxis.IsInversed ? columnSegment.XRange.Start : columnSegment.XRange.End, columnSegment.YRange.Median);
            }
            else
            {
                point = ChartTransformer.TransformToVisible(columnSegment.XRange.Median, columnSegment.YData);
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
            return new ColumnSegment();
        }

#endregion

#region Protected Methods

        ///<summary>
        ///Method used to calculate the segment spacing.
        ///</summary>
        /// <param name="spacing">Spacing</param>
        /// <param name="Right">Right</param>
        /// <param name="Left">Left</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        private double CalculateSegmentSpacing(double spacing, double Right, double Left)
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
            var series = d as ColumnSeries;
            if (series.Chart != null)
                series.Chart.ScheduleUpdate();
        }
        
        private static void OnCustomTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as ColumnSeries;

            if (series.Chart == null) return;

            series.Segments.Clear();
            series.Chart.ScheduleUpdate();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add the <see cref="ColumnSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="xValue">The xValue.</param>
        /// <param name="yValue">The yValue.</param>
        /// <param name="actualData">The actualData.</param>
        private void CreateSegment(double[] values, double xValue, double yValue, object actualData)
        {
            ColumnSegment segment = CreateSegment() as ColumnSegment;
            if (segment != null)
            {
                segment.Series = this;
                segment.SetData(values);
                segment.customTemplate = CustomTemplate;
                segment.XData = xValue;
                segment.YData = yValue;
                segment.Item = actualData;
                Segments.Add(segment);
            }
        }

#endregion

#endregion
    }
}
