using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="BubbleSeries"/> displays a set of closely packed circles, whose areas are proportional to the quantities.
    /// </summary>
    /// <remarks>
    /// <see cref="BubbleSeries"/> requires an additional data binding parameter <see cref="BubbleSeries.Size"/> in addition to the X,Y parameters.
    /// <para>To render a series, create an instance of <see cref="BubbleSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XyDataSeries.StrokeThickness"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>Size - </b> Specify the bubble size using the <see cref="Size"/> property.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="BubbleSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="BubbleSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
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
    ///           <chart:BubbleSeries ItemsSource="{Binding Data}"
    ///                               XBindingPath="XValue"
    ///                               YBindingPath="YValue"
    ///                               Size = "Size"/>
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
    ///     BubbleSeries series = new BubbleSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.Size = "Size";
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
    ///        Data.Add(new Model() { XValue = 10, YValue = 100, Size = 1});
    ///        Data.Add(new Model() { XValue = 20, YValue = 150, Size = 1});
    ///        Data.Add(new Model() { XValue = 30, YValue = 110, Size = 1});
    ///        Data.Add(new Model() { XValue = 40, YValue = 230, Size = 1});
    ///     }
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    /// <seealso cref="BubbleSegment"/>
    public class BubbleSeries : XyDataSeries
    {
        #region Dependency Property Registration
       
        /// <summary>
        /// The DependencyProperty of <see cref="ShowZeroBubbles"/> property
        /// </summary>
        internal static readonly DependencyProperty ShowZeroBubblesProperty =
            DependencyProperty.Register(nameof(ShowZeroBubbles), typeof(bool), typeof(BubbleSeries), new PropertyMetadata(true, OnShowZeroBubblesPropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="MinimumRadius"/> property.
        /// </summary>
        public static readonly DependencyProperty MinimumRadiusProperty =
            DependencyProperty.Register(nameof(MinimumRadius), typeof(double), typeof(BubbleSeries),
            new PropertyMetadata(10d, new PropertyChangedCallback(OnSizeChanged)));

        /// <summary>
        /// The DependencyProperty for <see cref="MaximumRadius"/> property.
        /// </summary>
        public static readonly DependencyProperty MaximumRadiusProperty =
            DependencyProperty.Register(nameof(MaximumRadius), typeof(double), typeof(BubbleSeries),
            new PropertyMetadata(30d, new PropertyChangedCallback(OnSizeChanged)));

        /// <summary>
        /// The DependencyProperty for <see cref="Size"/> property.
        /// </summary>
        public static readonly DependencyProperty SizeProperty =
            DependencyProperty.Register(nameof(Size), typeof(string), typeof(BubbleSeries),
            new PropertyMetadata(null, new PropertyChangedCallback(OnSizeChanged)));

        /// <summary>
        /// The DependencyProperty for <see cref="CustomTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty CustomTemplateProperty =
            DependencyProperty.Register(nameof(CustomTemplate), typeof(DataTemplate), typeof(BubbleSeries),
            new PropertyMetadata(null, OnCustomTemplateChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(BubbleSeries),
            new PropertyMetadata(null, OnStrokeChanged));

        #endregion

        #region Fields

        #region Private Fields

        private List<double> sizeValues;

        private Storyboard sb;

        private bool IsSegmentAtEdge;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BubbleSeries"/>.
        /// </summary>
        public BubbleSeries()
        {
            sizeValues = new List<double>();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether to show the bubble segment when apply size value to 0. This is a bindable property.
        /// </summary>
        internal bool ShowZeroBubbles
        {
            get { return (bool)GetValue(ShowZeroBubblesProperty); }
            set { SetValue(ShowZeroBubblesProperty, value); }
        }


        /// <summary>
        /// Gets or sets the minimum size for the each bubble.
        /// </summary>
        /// <value>It takes the double value, and its default value is 10.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BubbleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              Size = "Size"
        ///                              MinimumRadius ="15">
        ///          </chart:BubbleSeries>
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
        ///     BubbleSeries series = new BubbleSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.Size = "Size";
        ///     series.MinimumRadius = 15;
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double MinimumRadius
        {
            get { return (double)GetValue(MinimumRadiusProperty); }
            set { SetValue(MinimumRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum size for each bubble.
        /// </summary>
        /// <value>It takes the double value, and its default value is 30.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BubbleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              Size = "Size"
        ///                              MaximumRadius ="40">
        ///          </chart:BubbleSeries>
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
        ///     BubbleSeries series = new BubbleSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.Size = "Size";
        ///     series.MaximumRadius = 40;
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double MaximumRadius
        {
            get { return (double)GetValue(MaximumRadiusProperty); }
            set { SetValue(MaximumRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the bubble size.
        /// </summary>
        /// <value>The string that represents the bubble size and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BubbleSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              Size = "Size" >
        ///          </chart:BubbleSeries>
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
        ///     BubbleSeries series = new BubbleSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.Size = "Size";
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string Size
        {
            get { return (string)GetValue(SizeProperty); }
            set { SetValue(SizeProperty, value); }
        }


        /// <summary>
        /// Gets or sets the custom template to customize the appearance of bubble series.
        /// </summary>
        /// <value>It takes the <see cref="DataTemplate"/> value, and its default value is null.</value>
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
        /// # [Xaml](#tab/tabid-10)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BubbleSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red"
        ///                            StrokeWidth = "3"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-11)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     BubbleSeries bubbleSeries = new BubbleSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth= 3,
        ///     };
        ///     
        ///     chart.Series.Add(bubbleSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush Stroke 
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        #endregion

        #region Internal Override Properties

        internal override bool IsMultipleYPathRequired
        {
            get
            {
                return true;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the Segments of BubbleSeries.
        /// </summary>
        internal override void GenerateSegments()
        {
            double maximumSize = 0d, segmentRadius = 0d;
            List<double> xValues = null;
            var isGrouped = (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed);
            if (isGrouped)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();
            maximumSize = (from val in sizeValues select val).Max();
            double minRadius = this.MinimumRadius;
            double maxradius = this.MaximumRadius;
            double radius = maxradius - minRadius;

            if (xValues != null)
            {
                if (isGrouped)
                {
                    Segments.Clear();
                    Adornments.Clear();
                    for (int i = 0; i < xValues.Count; i++)
                    {
                        var relativeSize = radius * Math.Abs(sizeValues[i] / maximumSize);
                        relativeSize = double.IsNaN(relativeSize) ? 0 : relativeSize;
                        if (ShowZeroBubbles)
                            segmentRadius = minRadius + relativeSize;
                        else
                            segmentRadius = (sizeValues[i] != 0) ? (minRadius + relativeSize) : 0;
                        if (i < xValues.Count && GroupedSeriesYValues[0].Count > i)
                        {
                            BubbleSegment bubbleSegment = CreateSegment() as BubbleSegment;
                            if (bubbleSegment != null)
                            {
                                bubbleSegment.Radius = segmentRadius;
                                bubbleSegment.Series = this;
                                bubbleSegment.SetData(xValues[i], GroupedSeriesYValues[0][i]);
                                bubbleSegment.Item = ActualData[i];
                                bubbleSegment.Size = sizeValues[i];
                                Segments.Add(bubbleSegment);
                            }
                        }

                        if (AdornmentsInfo != null && ShowDataLabels)
                            AddAdornmentAtXY(xValues[i], GroupedSeriesYValues[0][i], i);
                    }
                }
                else
                {
                    ClearUnUsedSegments(this.PointsCount);
                    ClearUnUsedAdornments(this.PointsCount);
                    for (int i = 0; i < this.PointsCount; i++)
                    {
                        var relativeSize = radius * Math.Abs(sizeValues[i] / maximumSize);
                        relativeSize = double.IsNaN(relativeSize) ? 0 : relativeSize;
                        if (ShowZeroBubbles)
                            segmentRadius = minRadius + relativeSize;
                        else
                            segmentRadius = (sizeValues[i] != 0) ? (minRadius + relativeSize) : 0;
                        if (i < Segments.Count)
                        {
                            (Segments[i]).SetData(xValues[i], YValues[i]);
                            (Segments[i] as BubbleSegment).Radius = segmentRadius;
                            (Segments[i] as BubbleSegment).Item = ActualData[i];
                            (Segments[i] as BubbleSegment).Size = sizeValues[i];
                            (Segments[i] as BubbleSegment).YData = YValues[i];
                            (Segments[i] as BubbleSegment).XData = xValues[i];
                        }
                        else
                        {
                            BubbleSegment bubbleSegment = CreateSegment() as BubbleSegment;
                            if (bubbleSegment != null)
                            {
                                bubbleSegment.Series = this;
                                bubbleSegment.Radius = segmentRadius;
                                bubbleSegment.SetData(xValues[i], YValues[i]);
                                bubbleSegment.Item = ActualData[i];
                                bubbleSegment.Size = sizeValues[i];
                                Segments.Add(bubbleSegment);
                            }
                        }

                        if (AdornmentsInfo != null && ShowDataLabels)
                            AddAdornmentAtXY(xValues[i], YValues[i], i);
                    }
                }
            }
        }

        #endregion

        #region Internal Override Methods

        internal override int GetDataPointIndex(Point point)
        {
            Canvas canvas = Chart.GetAdorningCanvas();
            double left = Chart.ActualWidth - canvas.ActualWidth;
            double top = Chart.ActualHeight - canvas.ActualHeight;

            point.X = point.X - left - Chart.SeriesClipRect.Left + Chart.Margin.Left;
            point.Y = point.Y - top - Chart.SeriesClipRect.Top + Chart.Margin.Top;

            foreach (var segment in Segments)
            {
                var ellipse = segment.GetRenderedVisual() as Ellipse;

                if (ellipse != null && EllipseContainsPoint(ellipse, point))
                    return Segments.IndexOf(segment);
            }

            return -1;
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            IsSegmentAtEdge = false;
            BubbleSegment bubbleSegment = ToolTipTag as BubbleSegment;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(bubbleSegment.XData, bubbleSegment.YData);
            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top - bubbleSegment.Radius;

            if (newPosition.Y - tooltip.DesiredSize.Height < ActualArea.SeriesClipRect.Top)
            {
                //WPF-57206 - Bubble segment at edge shows tooltip on it. To make it in correct position, constant offset 8 has been added.
                newPosition.Y += (bubbleSegment.Radius * 2) + tooltip.DesiredSize.Height + 8;
                IsSegmentAtEdge = true;
            }

            return newPosition;
        }

        internal override VerticalPosition GetVerticalPosition(VerticalPosition verticalPosition)
        {
            return IsSegmentAtEdge ? VerticalPosition.Top : base.GetVerticalPosition(verticalPosition);
        }

        internal override void Animate()
        {
            int i = 0;
            Random rand = new Random();

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

            foreach (ChartSegment segment in this.Segments)
            {
                int randomValue = rand.Next(0, 20);
                TimeSpan beginTime = TimeSpan.FromMilliseconds(randomValue * 20);
                var element = (FrameworkElement)segment.GetRenderedVisual();
                element.RenderTransform = null;
                element.RenderTransform = new ScaleTransform() { ScaleY = 0, ScaleX = 0 };
                element.RenderTransformOrigin = new Point(0.5, 0.5);
                DoubleAnimationUsingKeyFrames keyFrames = new DoubleAnimationUsingKeyFrames();
                keyFrames.BeginTime = beginTime;
                SplineDoubleKeyFrame keyFrame = new SplineDoubleKeyFrame();

#if !WinUI_UWP
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
#else
                keyFrame.KeyTime = KeyTimeHelper.FromTimeSpan(TimeSpan.FromSeconds(0));
#endif
              
                keyFrame.Value = 0;
                keyFrames.KeyFrames.Add(keyFrame);
                keyFrame = new SplineDoubleKeyFrame();
                ////keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(AnimationDuration.TotalMilliseconds - (randomValue * (2 * AnimationDuration.TotalMilliseconds) / 100)));
#if !WinUI_UWP
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 50) / 100));
#else
                keyFrame.KeyTime = KeyTimeHelper.FromTimeSpan(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 50) / 100));
#endif

                KeySpline keySpline = new KeySpline();
                keySpline.ControlPoint1 = new Point(0.64, 0.84);
                keySpline.ControlPoint2 = new Point(0.67, 0.95);
                keyFrame.KeySpline = keySpline;
                keyFrames.KeyFrames.Add(keyFrame);
                keyFrame.Value = 1;
                keyFrames.EnableDependentAnimation = true;
                Storyboard.SetTargetProperty(keyFrames, "(UIElement.RenderTransform).(ScaleTransform.ScaleY)");
                Storyboard.SetTarget(keyFrames, element);
                sb.Children.Add(keyFrames);

                keyFrames = new DoubleAnimationUsingKeyFrames();
                keyFrames.BeginTime = beginTime;
                keyFrame = new SplineDoubleKeyFrame();
#if !WinUI_UWP
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds(0));
#else
                keyFrame.KeyTime = KeyTimeHelper.FromTimeSpan(TimeSpan.FromSeconds(0));
#endif
               
                keyFrame.Value = 0;
                keyFrames.KeyFrames.Add(keyFrame);
                keyFrame = new SplineDoubleKeyFrame();
                ////keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(AnimationDuration.TotalMilliseconds - (randomValue * (2 * AnimationDuration.TotalMilliseconds) / 100)));

#if !WinUI_UWP
                keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 50) / 100));
#else
                keyFrame.KeyTime = KeyTimeHelper.FromTimeSpan(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 50) / 100));
#endif


                keySpline = new KeySpline();
                keySpline.ControlPoint1 = new Point(0.64, 0.84);
                keySpline.ControlPoint2 = new Point(0.67, 0.95);
                keyFrame.KeySpline = keySpline;
                keyFrames.KeyFrames.Add(keyFrame);
                keyFrame.Value = 1;
                keyFrames.EnableDependentAnimation = true;
                Storyboard.SetTargetProperty(keyFrames, "(UIElement.RenderTransform).(ScaleTransform.ScaleX)");
                Storyboard.SetTarget(keyFrames, element);
                sb.Children.Add(keyFrames);

                if (AdornmentsInfo != null && AdornmentsInfo.Visible)
                {
                    UIElement label = this.AdornmentsInfo.LabelPresenters[i];
                    label.Opacity = 0;

                    DoubleAnimation animation = new DoubleAnimation() { To = 1, From = 0, BeginTime = TimeSpan.FromSeconds(beginTime.TotalSeconds + (beginTime.Seconds * 90) / 100)};
                    animation.Duration = new Duration().GetDuration(TimeSpan.FromSeconds(AnimationDuration.TotalSeconds / 2));
                    Storyboard.SetTargetProperty(animation, "FrameworkElement.Opacity");
                    Storyboard.SetTarget(animation, label);
                    sb.Children.Add(animation);
                }

                i++;
            }

            sb.Begin();
        }

        internal override bool GetAnimationIsActive()
        {
            return sb != null && sb.GetCurrentState() == ClockState.Active;
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

        #region Protected Internal Override Methods

        /// <summary>
        /// Method used to generate points for bubble series.
        /// </summary>
        internal override void GenerateDataPoints()
        {
            sizeValues.Clear();
            GeneratePoints(new string[] { YBindingPath, Size }, YValues, sizeValues);
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            return new BubbleSegment();
        }

        /// <summary>
        /// Called when ItemsSource property changed.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            YValues.Clear();
            sizeValues.Clear();
            GeneratePoints(new string[] { YBindingPath, Size }, YValues, sizeValues);
            this.ScheduleUpdateChart();
        }

        internal override void OnBindingPathChanged()
        {
            YValues.Clear();
            sizeValues.Clear();
            ResetData();
            GeneratePoints(new[] { YBindingPath, Size }, YValues, sizeValues);
            if (this.Chart != null && this.Chart.PlotArea != null)
                this.Chart.PlotArea.ShouldPopulateLegendItems = true;
            canAnimate = true;
            isTotalCalculated = false;
            this.ScheduleUpdateChart();
        }

        #endregion
        
        #region Private Static Methods

        /// <summary>
        /// This method used to check the position within the ellipse
        /// </summary>
        /// <param name="Ellipse"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private static bool EllipseContainsPoint(Ellipse Ellipse, Point point)
        {
            Point center = new Point(Canvas.GetLeft(Ellipse) + (Ellipse.Width / 2),
                  Canvas.GetTop(Ellipse) + (Ellipse.Height / 2));

            double x = Ellipse.Width / 2;
            double y = Ellipse.Height / 2;

            if (x <= 0.0 || y <= 0.0)
                return false;

            Point result = new Point(point.X - center.X,
                                         point.Y - center.Y);

            return ((double)(result.X * result.X)
                     / (x * x)) + ((double)(result.Y * result.Y) / (y * y))
                <= 1.0;
        }

        private static void OnShowZeroBubblesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BubbleSeries).ScheduleUpdateChart();
        }

        private static void OnSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BubbleSeries).OnBindingPathChanged();
        }

        private static void OnCustomTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as BubbleSeries;

            if (series.Chart == null) return;

            series.Segments.Clear();
            series.Chart.ScheduleUpdate();
        }

        private static void OnStrokeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is BubbleSeries chartSeries) 
            {
                OnStrokeChanged(chartSeries);
            }
        }

        private static void OnStrokeChanged(BubbleSeries obj)
        {
            if (obj.IsBitmapSeries)
                obj.ScheduleUpdateChart();
        }

        #endregion

        #endregion
    }
}
