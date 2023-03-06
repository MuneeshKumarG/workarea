using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="PieSeries"/> displays data as a proportion of the whole. Its most commonly used to make comparisons among a set of given data.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of the pie series class, and add it to the <see cref="SfCircularChart.Series"/> collection.</para>
    /// 
    /// <para>It Provides options for <see cref="ChartSeries.PaletteBrushes"/>, <see cref="ChartSeries.Fill"/>, <see cref="CircularSeries.Stroke"/>, <see cref="CircularSeries.StrokeThickness"/>, and <see cref="CircularSeries.Radius"/> to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> Tooltips display information while tapping or mouse hovering over the segment. To display the tooltip on the chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in the <see cref="PieSeries"/>, and also refer to the <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in the <see cref="PieSeries"/> class. To customize the chart data labels alignment, placement and label styles, need to create an instance of <see cref="CircularDataLabelSettings"/> and set to the <see cref="CircularSeries.DataLabelSettings"/> property. </para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>Selection - </b> To enable the data point selection in the series, create an instance of the <see cref="DataPointSelectionBehavior"/> and set it to the <see cref="PieSeries.SelectionBehavior"/> property of pie series. To highlight the selected segment, set the value for the <see cref="ChartSelectionBehavior.SelectionBrush"/> property in the <see cref="DataPointSelectionBehavior"/> class.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/>, and <see cref="ChartSeries.LegendIconTemplate"/> properties.</para>
    ///  
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCircularChart>
    ///
    ///         <chart:PieSeries ItemsSource="{Binding Data}"
    ///                          XBindingPath="XValue"
    ///                          YBindingPath="YValue"/>
    ///           
    ///     </chart:SfCircularChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCircularChart chart = new SfCircularChart();
    ///     
    ///     ViewModel viewModel = new ViewModel();
    /// 
    ///     PieSeries series = new PieSeries();
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
    /// <seealso cref="DoughnutSegment"/>
    /// <seealso cref="DoughnutSeries"/>
    /// <seealso cref="PieSegment"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class PieSeries : CircularSeries
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="ExplodeRadius"/> property.
        /// </summary>
        public static readonly DependencyProperty ExplodeRadiusProperty =
            DependencyProperty.Register(nameof(ExplodeRadius), typeof(double), typeof(PieSeries),
            new PropertyMetadata(30d, OnExplodeRadiusChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ExplodeIndex"/> property.
        /// </summary>
        public static readonly DependencyProperty ExplodeIndexProperty =
            DependencyProperty.Register(nameof(ExplodeIndex), typeof(int), typeof(PieSeries),
            new PropertyMetadata(-1, OnExplodeIndexChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ExplodeAll"/> property.
        /// </summary>
        public static readonly DependencyProperty ExplodeAllProperty =
            DependencyProperty.Register(nameof(ExplodeAll), typeof(bool), typeof(PieSeries),
            new PropertyMetadata(false, OnExplodeAllChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ExplodeOnTap"/> property.
        /// </summary>
        public static readonly DependencyProperty ExplodeOnTapProperty =
            DependencyProperty.Register(nameof(ExplodeOnTap), typeof(bool), typeof(PieSeries),
            new PropertyMetadata(false));

        #endregion

        #region Fields

        #region Internal Fields

        internal double InternalPieCoefficient = 0.8;

        #endregion

        #region Private Fields


        private bool allowExplode;

        private ChartSegment mouseUnderSegment;

        private Storyboard sb;

        private double grandTotal = 0d;

        private double ARCLENGTH;

        private double arcStartAngle, arcEndAngle;

        private int animateCount = 0;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSeries"/> class.
        /// </summary>
        public PieSeries()
        {
            DefaultStyleKey = typeof(PieSeries);
            this.PaletteBrushes = ChartColorModel.DefaultBrushes;
        }

        #endregion

        #region Properties

        #region Public Properties


        /// <summary>
        /// Gets or sets a value that can be used to define the radial distance for the exploded segment from the center.
        /// </summary>
        /// <value>It accepts double values, and the default value is 30.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                           XBindingPath="XValue"
        ///                           YBindingPath="YValue"
        ///                           ExplodeRadius = "50"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ExplodeRadius = 50,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double ExplodeRadius {
            get { return (double)GetValue(ExplodeRadiusProperty); }
            set { SetValue(ExplodeRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of segment in series to be exploded.
        /// </summary>
        /// <value>This property takes an <see cref="int"/> value, and its default value is <c>-1</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-17)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ExplodeIndex = "2"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-18)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ExplodeIndex = 2,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public int ExplodeIndex {
            get { return (int)GetValue(ExplodeIndexProperty); }
            set { SetValue(ExplodeIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to explode all the pie slices (segments).
        /// </summary>
        /// <value>
        ///     <c>True</c>, will explode all the segments.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-19)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ExplodeAll = "True"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-20)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ExplodeAll = true,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public bool ExplodeAll {
            get { return (bool)GetValue(ExplodeAllProperty); }
            set { SetValue(ExplodeAllProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether segment slices will explode on mouse click or tap.
        /// </summary>
        /// <value>
        /// if <c>true</c>, the segment will explode on click or tap.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-21)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ExplodeOnTap = "True"/>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-22)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ExplodeOnTap = true,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public bool ExplodeOnTap {
            get { return (bool)GetValue(ExplodeOnTapProperty); }
            set { SetValue(ExplodeOnTapProperty, value); }
        }

#if NETFX_CORE
        public PieSegment Segment
        {
            get { return null; }
        }
#endif



        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of PieSeries.
        /// </summary>
        internal override void GenerateSegments()
        {
            IList<double> toggledYValues = null;
            List<double> xValues = null;
            var actualData = ActualData;

            if (double.IsNaN(GroupTo))
            {
                if (ToggledLegendIndex.Count > 0)
                    toggledYValues = GetYValues();
                else
                    toggledYValues = YValues;

                xValues = GetXValues();
            }
            else
            {
                if (Adornments != null) Adornments.Clear();
                if (Segments != null) Segments.Clear();

                var sumOfYValues = (from val in YValues
                                    select (val) > 0 ? val : Math.Abs(double.IsNaN(val) ? 0 : val)).Sum();
                double xIndexValues = 0d;
                xValues = (from val in YValues where GetGroupModeValue(val, sumOfYValues) > GroupTo select (xIndexValues++)).ToList();
                if (YValues.Count > xValues.Count) xValues.Add(xIndexValues);

                var groupToValues = GetGroupToYValues();
                actualData = groupToValues.Item2;

                if (ToggledLegendIndex.Count > 0)
                    toggledYValues = GetToggleYValues(groupToValues.Item1);
                else
                    toggledYValues = groupToValues.Item1;
            }

            ClearUnUsedAdornments(this.PointsCount);
            ClearUnUsedSegments(this.PointsCount);
            int explodedIndex = ExplodeIndex;
            bool explodedAll = ExplodeAll;
            arcStartAngle = DegreeToRadianConverter(StartAngle);
            arcEndAngle = DegreeToRadianConverter(EndAngle);
            if (arcStartAngle == arcEndAngle)
                Segments.Clear();
            ARCLENGTH = arcEndAngle - arcStartAngle;
            if (Math.Abs(Math.Round(ARCLENGTH, 2)) > TotalArcLength)
                ARCLENGTH = ARCLENGTH % TotalArcLength;
            if (xValues != null)
            {
                grandTotal = (from val in toggledYValues
                              select (val) > 0 ? val : Math.Abs(double.IsNaN(val) ? 0 : val)).Sum();
                for (int i = 0; i < xValues.Count; i++)
                {
                    arcEndAngle = grandTotal == 0 ? 0 : (Math.Abs(double.IsNaN(toggledYValues[i]) ? 0 : toggledYValues[i]) * (ARCLENGTH / grandTotal));

                    if (i < Segments.Count)
                    {
                        (Segments[i] as PieSegment).SetData(
                            arcStartAngle,
                            arcStartAngle + arcEndAngle,
                            this,
                            actualData[i]);
                        (Segments[i] as PieSegment).XData = xValues[i];
                        (Segments[i] as PieSegment).YData = !double.IsNaN(GroupTo) ? Math.Abs(toggledYValues[i]) : Math.Abs(YValues[i]);
                        (Segments[i] as PieSegment).AngleOfSlice = (2 * arcStartAngle + arcEndAngle) / 2;
                        (Segments[i] as PieSegment).IsExploded = explodedAll || (explodedIndex == i);
                        (Segments[i] as PieSegment).Item = actualData[i];
                        if (ToggledLegendIndex.Contains(i))
                            Segments[i].IsSegmentVisible = false;
                        else
                            Segments[i].IsSegmentVisible = true;
                    }
                    else
                    {
                        var segment = CreateSegment() as PieSegment;
                        if (segment != null)
                        {
                            segment.SetData(arcStartAngle, arcStartAngle + arcEndAngle, this, actualData[i]);
                            segment.XData = xValues[i];
                            segment.YData = !double.IsNaN(GroupTo) ? Math.Abs(toggledYValues[i]) : Math.Abs(YValues[i]);
                            segment.AngleOfSlice = (2 * arcStartAngle + arcEndAngle) / 2;
                            segment.IsExploded = explodedAll || explodedIndex == i;
                            segment.Item = actualData[i];
                            if (ToggledLegendIndex.Contains(i))
                                segment.IsSegmentVisible = false;
                            else
                                segment.IsSegmentVisible = true;
                            Segments.Add(segment);
                        }
                    }

                    if (AdornmentsInfo != null && ShowDataLabels)
                        AddPieAdornments(xValues[i], toggledYValues[i], arcStartAngle, arcStartAngle + arcEndAngle, i);
                    arcStartAngle += arcEndAngle;
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
            if (this.Segments.Count > 0)
            {
                double startAngle = DegreeToRadianConverter(StartAngle);

                // WPF-25124 Animation not working properly when resize the window.
                if (sb != null && (animateCount <= Segments.Count))
                    sb = new Storyboard();
                else if (sb != null)
                {
                    sb.Stop();
                    if (!canAnimate)
                    {
                        foreach (PieSegment segment in this.Segments)
                        {
                            if ((segment.EndAngle - segment.StartAngle) == 0) continue;
                            FrameworkElement element = segment.GetRenderedVisual() as FrameworkElement;
                            element.ClearValue(FrameworkElement.RenderTransformProperty);
                            segment.ActualStartAngle = segment.StartAngle;
                            segment.ActualEndAngle = segment.EndAngle;
                        }

                        ResetAdornmentAnimationState();
                        return;
                    }
                }
                else
                    sb = new Storyboard();

                AnimateAdornments(sb);

                foreach (PieSegment segment in this.Segments)
                {
                    animateCount++;
                    double segStartAngle = segment.StartAngle;
                    double segEndAngle = segment.EndAngle;
                    if ((segEndAngle - segStartAngle) == 0) continue;
                    FrameworkElement element = segment.GetRenderedVisual() as FrameworkElement;
                    element.Width = this.DesiredSize.Width;
                    element.Height = this.DesiredSize.Height;
                    element.RenderTransform = new ScaleTransform();
                    element.RenderTransformOrigin = new Point(0.5, 0.5);

                    DoubleAnimationUsingKeyFrames keyFrames = new DoubleAnimationUsingKeyFrames();
                    SplineDoubleKeyFrame keyFrame = new SplineDoubleKeyFrame
                    {
                        Value = startAngle
                    };
                    keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                    keyFrames.KeyFrames.Add(keyFrame);

                    keyFrame = new SplineDoubleKeyFrame
                    {
                        Value = segStartAngle
                    };
                    keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(AnimationDuration);
                    var keySpline = new KeySpline();
                    keySpline.ControlPoint1 = new Point(0.64, 0.84);
                    keySpline.ControlPoint2 = new Point(0.67, 0.95);
                    keyFrame.KeySpline = keySpline;

                    keyFrames.KeyFrames.Add(keyFrame);
                    Storyboard.SetTargetProperty(keyFrames, "PieSegment.ActualStartAngle");
                    keyFrames.EnableDependentAnimation = true;
                    Storyboard.SetTarget(keyFrames, segment);
                    sb.Children.Add(keyFrames);

                    keyFrames = new DoubleAnimationUsingKeyFrames();
                    keyFrame = new SplineDoubleKeyFrame
                    {
                        Value = startAngle
                    };
                    keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                    keyFrames.KeyFrames.Add(keyFrame);

                    keyFrame = new SplineDoubleKeyFrame
                    {
                        Value = segEndAngle
                    };
                    keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(AnimationDuration);
                    keySpline = new KeySpline
                    {
                        ControlPoint1 = new Point(0.64, 0.84),
                        ControlPoint2 = new Point(0.67, 0.95)
                    };
                    keyFrame.KeySpline = keySpline;

                    keyFrames.KeyFrames.Add(keyFrame);
                    Storyboard.SetTargetProperty(keyFrames, "PieSegment.ActualEndAngle");
                    keyFrames.EnableDependentAnimation = true;
                    Storyboard.SetTarget(keyFrames, segment);
                    sb.Children.Add(keyFrames);

                    keyFrames = new DoubleAnimationUsingKeyFrames();
                    keyFrame = new SplineDoubleKeyFrame
                    {
                        Value = 0
                    };
                    keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                    keyFrames.KeyFrames.Add(keyFrame);

                    keyFrame = new SplineDoubleKeyFrame
                    {
                        Value = 1
                    };
                    keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 80 / 100)));
                    keySpline = new KeySpline
                    {
                        ControlPoint1 = new Point(0.64, 0.84),
                        ControlPoint2 = new Point(0.67, 0.95)
                    };
                    keyFrame.KeySpline = keySpline;

                    keyFrames.KeyFrames.Add(keyFrame);
                    Storyboard.SetTargetProperty(keyFrames, "(UIElement.RenderTransform).(ScaleTransform.ScaleX)");
                    Storyboard.SetTarget(keyFrames, element);
                    sb.Children.Add(keyFrames);
                    keyFrames = new DoubleAnimationUsingKeyFrames();
                    keyFrame = new SplineDoubleKeyFrame();
                    keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                    keyFrame.Value = 0;
                    keyFrames.KeyFrames.Add(keyFrame);
                    keyFrame = new SplineDoubleKeyFrame
                    {
                        Value = 1
                    };
                    keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 80 / 100)));
                    keySpline = new KeySpline
                    {
                        ControlPoint1 = new Point(0.64, 0.84),
                        ControlPoint2 = new Point(0.67, 0.95)
                    };
                    keyFrame.KeySpline = keySpline;

                    keyFrames.KeyFrames.Add(keyFrame);
                    Storyboard.SetTargetProperty(keyFrames, "(UIElement.RenderTransform).(ScaleTransform.ScaleY)");
                    Storyboard.SetTarget(keyFrames, element);
                    sb.Children.Add(keyFrames);
                }

                sb.Begin();
            }
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

        /// <summary>
        /// Called when the chart mouse up.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="position">position</param>
        internal override void OnSeriesMouseUp(object source, Point position) {
            var element = source as FrameworkElement;
            var segment = element != null ? element.Tag as ChartSegment : null;
            int index = -1;
            if (ExplodeOnTap && allowExplode && mouseUnderSegment == segment) {
                if (segment != null && segment.Series is CircularSeries)
                    index = !double.IsNaN(((CircularSeries)segment.Series).GroupTo) ? Segments.IndexOf(segment) : ActualData.IndexOf(segment.Item);
                else if (Adornments.Count > 0)
                    index = ChartExtensionUtils.GetAdornmentIndex(source);
                var newIndex = index;
                var oldIndex = ExplodeIndex;
                if (newIndex != oldIndex)
                    ExplodeIndex = newIndex;
                else if (ExplodeIndex >= 0)
                    ExplodeIndex = -1;
                allowExplode = false;
            }
        }

        /// <summary>
        /// Called when the chart mouse down.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="position">position</param>
        internal override void OnSeriesMouseDown(object source, Point position)
        {
            if (GetAnimationIsActive()) return;

            allowExplode = true;
            var element = source as FrameworkElement;
            mouseUnderSegment = element.Tag as ChartSegment;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Gets the pie series count.
        /// </summary>
        /// <returns></returns>
        internal int GetPieSeriesCount()
        {
            return (from series in Chart.VisibleSeries where series is PieSeries select series).ToList().Count;
        }

        #endregion

        #region Protected Internal Override Methods

        /// <summary>
        /// Return IChartTranform value based upon the given size.
        /// </summary>
        /// <param name="size">size</param>
        /// <param name="create">bool</param>
        /// <returns></returns>
        internal override IChartTransformer CreateTransformer(Size size, bool create)
        {
            if (create || ChartTransformer == null)
            {
                ChartTransformer = ChartTransform.CreateSimple(size);
            }

            return ChartTransformer;
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            return new PieSegment();
        }

        internal override ChartDataLabel CreateAdornment(ChartSeries series, double xVal, double yVal, double angle, double radius)
        {
            return CreateDataMarker(series, xVal, yVal, angle, radius);
        }

        /// <summary>
        ///  Create the data marker for doughnut series.
        /// </summary>
        /// <param name="series">series</param>
        /// <param name="xVal">xvalue</param>
        /// <param name="yVal">yvalue</param>
        /// <param name="angle">angle</param>
        /// <param name="radius">radius</param>
        /// <returns>ChartAdornment</returns>
        internal override ChartDataLabel CreateDataMarker(ChartSeries series, double xVal, double yVal, double angle, double radius)
        {
            var segment = new ChartPieDataLabel(xVal, yVal, angle, radius, series);
            segment.SetValues(xVal, yVal, angle, radius, series);
            return segment;
        }

        /// <summary>
        /// Method implementation for ExplodeIndex.
        /// </summary>
        /// <param name="i"></param>
        internal override void SetExplodeIndex(int i)
        {
            if (Segments.Count > 0)
            {
                foreach (PieSegment segment in Segments)
                {
                    int index = !double.IsNaN(GroupTo) ? Segments.IndexOf(segment) : ActualData.IndexOf(segment.Item);
                    if (i == index)
                    {
                        segment.IsExploded = !segment.IsExploded;
                        UpdateSegments(i, NotifyCollectionChangedAction.Remove);
                    }
                    else if (i == -1)
                    {
                        segment.IsExploded = false;
                        UpdateSegments(i, NotifyCollectionChangedAction.Remove);
                    }
                }
            }
        }

        /// <summary>
        /// Virtual Method for ExplodeRadius.
        /// </summary>
        internal override void SetExplodeRadius()
        {
            if (Segments.Count > 0)
            {
                foreach (PieSegment segment in Segments)
                {
                    int index = Segments.IndexOf(segment);
                    UpdateSegments(index, NotifyCollectionChangedAction.Replace);
                }
            }
        }

        /// <summary>
        /// Virtual method for ExplodeAll.
        /// </summary>
        internal override void SetExplodeAll()
        {
            if (Segments.Count > 0)
            {
                foreach (PieSegment segment in Segments)
                {
                    int index = Segments.IndexOf(segment);
                    segment.IsExploded = true;
                    UpdateSegments(index, NotifyCollectionChangedAction.Replace);
                }
            }
        }

        /// <summary>
        /// Called when ItemsSource changed.
        /// </summary>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            if (oldValue != null)
                animateCount = 0;
            base.OnDataSourceChanged(oldValue, newValue);
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            PieSegment pie = ToolTipTag as PieSegment;
            Point newPosition = new Point();
            double angle = pie.AngleOfSlice;
            double radius = pie.IsExploded ? ExplodeRadius + CircularRadius / 2 : CircularRadius / 2;
            newPosition.X = Center.X + (Math.Cos(angle) * radius);
            newPosition.Y = Center.Y + (Math.Sin(angle) * radius);
            return newPosition;
        }

        #endregion

        #region Private Static Methods

        internal override void OnCircularRadiusChanged(DependencyPropertyChangedEventArgs e)
        {
            InternalPieCoefficient = ChartMath.MinMax((double)e.NewValue, 0, 1);
            ScheduleUpdateChart();
        }

        #endregion

        #region Private Methods

        private static void OnExplodeRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PieSeries pieSeries) 
            {
                pieSeries.SetExplodeRadius();
            }
        }

        private static void OnExplodeIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PieSeries pieSeries)
            {
                pieSeries.SetExplodeIndex((int)e.NewValue);
            }
        }

        private static void OnExplodeAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        {
            if (d is PieSeries pieSeries) 
            {
                pieSeries.SetExplodeAll();
            }  
        }


        private void AddPieAdornments(double x, double y, double startAngle, double endAngle, int index)
        {
            double angle = (startAngle + endAngle) / 2;

            if (Chart == null || Chart.RootPanelDesiredSize == null)
                return;

            var actualheight = Chart.RootPanelDesiredSize.Value.Height;
            var actualwidth = Chart.RootPanelDesiredSize.Value.Width;

            var pieSeries = (from series in Chart.VisibleSeries where series is PieSeries select series).ToList();
            var pieSeriesCount = pieSeries.Count();

            double pieIndex = pieSeries.IndexOf(this);
            double actualRadius = (Math.Min(actualwidth, actualheight)) / 2;
            double equalParts = actualRadius / (pieSeriesCount);
            double radius = (equalParts * (pieIndex + 1)) - (equalParts * (1 - InternalPieCoefficient));

            if (index < Adornments.Count)
                (Adornments[index] as ChartPieDataLabel).SetData(x, y, angle, radius);
            else
                Adornments.Add(this.CreateAdornment(this, x, y, angle, radius));

            Adornments[index].Item = !double.IsNaN(GroupTo) ? Segments[index].Item : ActualData[index];
        }

        #endregion

        #endregion
    }
}
