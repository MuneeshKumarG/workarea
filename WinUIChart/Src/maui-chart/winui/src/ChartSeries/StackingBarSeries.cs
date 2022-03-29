using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Interop;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;
using Windows.UI;
using AdornmentsPosition = Syncfusion.UI.Xaml.Charts.BarLabelAlignment;
using SelectionStyle = Syncfusion.UI.Xaml.Charts.SelectionType;

namespace Syncfusion.UI.Xaml.Charts
{
    ///<summary>
    /// Represents a special kind of bar series which is similar to regular bar series except that the Y-values stack on top of each other.
    /// </summary>
    /// <remarks>
    /// StackedBarSeries is typically preferred in cases of multiple series of type <see cref="BarSeries"/>. Each series is stacked vertically one above the other.
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfChart>
    ///
    ///           <chart:SfChart.PrimaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfChart.PrimaryAxis>
    ///
    ///           <chart:SfChart.SecondaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfChart.SecondaryAxis>
    ///
    ///           <chart:StackedBarSeries
    ///               ItemsSource="{Binding Data}"
    ///               XBindingPath="XValue"
    ///               YBindingPath="YValue"/>
    ///               
    ///           <chart:StackedBarSeries
    ///               ItemsSource="{Binding Data}"
    ///               XBindingPath="XValue"
    ///               YBindingPath="YValue1"/>
    ///                    
    ///     </chart:SfChart>
    /// ]]></code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfChart chart = new SfChart();
    ///     
    ///     NumericalAxis primaryAxis = new NumericalAxis();
    ///     NumericalAxis secondaryAxis = new NumericalAxis();
    ///     
    ///     chart.PrimaryAxis = primaryAxis;
    ///     chart.SecondaryAxis = secondaryAxis;
    ///     
    ///     StackedBarSeries series = new StackedBarSeries();
    ///     series.ItemsSource = viewmodel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    ///     
    ///     StackedBarSeries series1 = new StackedBarSeries();
    ///     series1.ItemsSource = viewmodel.Data;
    ///     series1.XBindingPath = "XValue";
    ///     series1.YBindingPath = "YValue1";
    ///     chart.Series.Add(series1);
    /// ]]></code>
    /// # [ViewModel.cs](#tab/tabid-3)
    /// <code><![CDATA[
    /// public ObservableCollection<Model> Data { get; set; }
    /// 
    /// public ViewModel()
    /// {
    ///    Data = new ObservableCollection<Model>();
    ///    Data.Add(new Model() { XValue = 10, YValue = 100, YValue1 = 110 });
    ///    Data.Add(new Model() { XValue = 20, YValue = 150, YValue1 = 100 });
    ///    Data.Add(new Model() { XValue = 30, YValue = 110, YValue1 = 130 });
    ///    Data.Add(new Model() { XValue = 40, YValue = 230, YValue1 = 180 });
    /// }
    /// ]]></code>
    /// ***
    /// </example>
    /// <seealso cref="StackedBarSegment"/>
    /// <seealso cref="StackedColumnSeries"/>
    /// <seealso cref="StackedAreaSeries"/>
    /// <seealso cref="BarSeries"/> 
    public class StackedBarSeries : StackedSeriesBase, ISegmentSelectable, ISegmentSpacing
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the SelectionBrush dependency property.
        /// </summary>
        /// <value>
        /// The identifier for SelectionBrush dependency property.
        /// </value>
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(
                nameof(SelectionBrush),
                typeof(Brush), 
                typeof(StackedBarSeries),
                new PropertyMetadata(null, OnSegmentSelectionBrush));

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
                typeof(StackedBarSeries),
                new PropertyMetadata(0.0, OnSegmentSpacingChanged));

        /// <summary>
        /// Identifies the SelectedIndex dependency property.
        /// </summary>
        /// <value>
        /// The identifier for SelectedIndex dependency property.
        /// </value>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(
                nameof(SelectedIndex),
                typeof(int),
                typeof(StackedBarSeries),
                new PropertyMetadata(-1, OnSelectedIndexChanged));

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
                typeof(StackedBarSeries),
                new PropertyMetadata(null, OnCustomTemplateChanged));

        #endregion

        #region Fields

        #region Private Fields

        private Storyboard sb;
        
        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the StackingBarSeries.
        /// </summary>
        public StackedBarSeries()
        {
            IsActualTransposed = true;
        }

        #endregion

        #region Properties

        #region Public Properties

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
        /// Gets or sets the spacing between the segments across the series in cluster mode.
        /// </summary>
        /// <value>
        /// The value ranges from 0 to 1 and its default value is 0.
        /// </value>
        public double SegmentSpacing
        {
            get { return (double)GetValue(SegmentSpacingProperty); }
            set { SetValue(SegmentSpacingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the first segment in the current selection or returns negative one (-1) if the selection is empty.
        /// </summary>
        /// <value>
        /// The index of first segment in the current selection. The default value is negative one (-1).
        /// </value>
        /// <seealso cref="SelectionBrush"/>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to customize the shape appearence. 
        /// </summary>
        /// <value>
        /// The template that specifies shape inside canvas. The default is null.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfChart>
        ///
        ///           <chart:SfChart.PrimaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfChart.PrimaryAxis>
        ///
        ///           <chart:SfChart.SecondaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfChart.SecondaryAxis>
        ///
        ///           <chart:StackedBarSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue">
        ///                    <chart:StackedBarSeries.CustomTemplate>
        ///                        <DataTemplate>
        ///                            <Canvas>
        ///                                <Rectangle Fill="{Binding Interior}" Width="{Binding Width}" Height="{Binding Height}"  
        ///                                Canvas.Left="{Binding RectX}" Canvas.Top="{Binding RectY}" Stroke="{Binding Stroke}"/>
        ///                            </Canvas>
        ///                        </DataTemplate>
        ///                    </chart:StackedBarSeries.CustomTemplate>
        ///           </chart:StackedBarSeries>
        ///                    
        ///     </chart:SfChart>
        /// ]]></code>
        /// ***
        /// </example>
        public DataTemplate CustomTemplate
        {
            get { return (DataTemplate)GetValue(CustomTemplateProperty); }
            set { SetValue(CustomTemplateProperty, value); }
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
            get
            {
                return true;
            }
        }

        #endregion

        #region Protected Override Properties

        /// <inheritdoc/>
        protected override bool IsStacked
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
        /// Creates the segments of <see cref="StackedBarSeries"/>.
        /// </summary>       
        public override void CreateSegments()
        {
            double x1, x2, y1, y2;
            List<double> xValues = null;
            double Origin = ActualXAxis != null ? ActualXAxis.Origin : 0d;
            if (ActualXAxis != null && ActualXAxis.Origin == 0 && ActualYAxis is LogarithmicAxis &&
                (ActualYAxis as LogarithmicAxis).Minimum != null)
                Origin = (double)(ActualYAxis as LogarithmicAxis).Minimum;
            if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
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
                    DoubleRange sbsInfo = this.GetSideBySideInfo(this);

                    double median = sbsInfo.Delta / 2;
                    int segmentCount = 0;
                    if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                    {
                        Segments.Clear();
                        Adornments.Clear();

                        for (int i = 0; i < DistinctValuesIndexes.Count; i++)
                        {
                            for (int j = 0; j < DistinctValuesIndexes[i].Count; j++)
                            {
                                if (j < xValues.Count)
                                {
                                    int index = DistinctValuesIndexes[i][j];
                                    x1 = i + sbsInfo.Start;
                                    x2 = i + sbsInfo.End;
                                    y2 = double.IsNaN(YRangeStartValues[segmentCount]) ? Origin : YRangeStartValues[segmentCount];
                                    y1 = double.IsNaN(YRangeEndValues[segmentCount]) ? Origin : YRangeEndValues[segmentCount];

                                    StackedBarSegment barSegment = CreateSegment() as StackedBarSegment;
                                    if (barSegment != null)
                                    {
                                        barSegment.Series = this;
                                        barSegment.SetData(x1, y1, x2, y2);
                                        barSegment.customTemplate = CustomTemplate;
                                        barSegment.XData = xValues[segmentCount];
                                        barSegment.YData = GroupedSeriesYValues[0][index];
                                        barSegment.Item = GroupedActualData[segmentCount];
                                        Segments.Add(barSegment);
                                    }

                                    if (AdornmentsInfo != null && ShowDataLabels)
                                    {
                                        AdornmentsPosition markerPosition = this.adornmentInfo.GetAdornmentPosition();
                                        if (markerPosition == AdornmentsPosition.Top)
                                            AddColumnAdornments(i, GroupedSeriesYValues[0][index], x1, y1, segmentCount, median);
                                        else if (markerPosition == AdornmentsPosition.Bottom)
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
                        ClearUnUsedSegments(this.DataCount);
                        ClearUnUsedAdornments(this.DataCount);
                        for (int i = 0; i < DataCount; i++)
                        {
                            x1 = xValues[i] + sbsInfo.Start;
                            x2 = xValues[i] + sbsInfo.End;
                            y2 = double.IsNaN(YRangeStartValues[i]) ? Origin : YRangeStartValues[i];
                            y1 = double.IsNaN(YRangeEndValues[i]) ? Origin : YRangeEndValues[i];

                            if (i < Segments.Count)
                            {
                                (Segments[i]).Item = ActualData[i];
                                (Segments[i]).SetData(x1, y1, x2, y2);
                                (Segments[i] as StackedBarSegment).XData = xValues[i];
                                (Segments[i] as StackedBarSegment).YData = YValues[i];
                                if (SegmentColorPath != null && !Segments[i].IsEmptySegmentInterior && ColorValues.Count > 0 && !Segments[i].IsSelectedSegment)
                                    Segments[i].Interior = (Interior != null) ? Interior : ColorValues[i];
                            }
                            else
                            {
                                StackedBarSegment barSegment = CreateSegment() as StackedBarSegment;
                                if (barSegment != null)
                                {
                                    barSegment.Series = this;
                                    barSegment.SetData(x1, y1, x2, y2);
                                    barSegment.customTemplate = CustomTemplate;
                                    barSegment.XData = xValues[i];
                                    barSegment.YData = ActualSeriesYValues[0][i];
                                    barSegment.Item = ActualData[i];
                                    Segments.Add(barSegment);
                                }
                            }

                            if (AdornmentsInfo != null && ShowDataLabels)
                            {
                                AdornmentsPosition markerPosition = this.adornmentInfo.GetAdornmentPosition();
                                if (markerPosition == AdornmentsPosition.Top)
                                    AddColumnAdornments(xValues[i], YValues[i], x1, y1, i, median);
                                else if (markerPosition == AdornmentsPosition.Bottom)
                                    AddColumnAdornments(xValues[i], YValues[i], x1, y2, i, median);
                                else
                                    AddColumnAdornments(xValues[i], YValues[i], x1, y1 + (y2 - y1) / 2, i, median);
                            }
                        }
                    }

                    if (ShowEmptyPoints)
                    {
                        if (this is StackedBar100Series)
                        {
                            var index = EmptyPointIndexes[0];
                            if (EmptyPointStyle == Charts.EmptyPointStyle.Symbol || EmptyPointStyle == Charts.EmptyPointStyle.SymbolAndInterior)

                                foreach (var n in index)
                                {
                                    this.ActualSeriesYValues[0][n] = double.IsNaN(YRangeEndValues[n]) ? 0 : this.YRangeEndValues[n];
                                }

                            UpdateEmptyPointSegments(xValues, true);
                            ReValidateYValues(EmptyPointIndexes);
                            ValidateYValues();
                        }
                        else
                            UpdateEmptyPointSegments(xValues, true);
                    }
                }
            }
        }

        #endregion

        #region Internal Override Methods

        internal override void OnTransposeChanged(bool val)
        {
            IsActualTransposed = !val;
        }

        internal override bool GetAnimationIsActive()
        {
            return sb != null && sb.GetCurrentState() == ClockState.Active;
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            StackedBarSegment stackingBarSegment = ToolTipTag as StackedBarSegment;
            Point newPosition = new Point();
            Point point;
            if (IsActualTransposed)
            {
                point = ChartTransformer.TransformToVisible(ActualXAxis.IsInversed ? stackingBarSegment.XRange.Start : stackingBarSegment.XRange.End, stackingBarSegment.YRange.Median);
            }
            else
            {
                point = ChartTransformer.TransformToVisible(stackingBarSegment.XRange.Median, stackingBarSegment.YData);
            }

            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top;
            return newPosition;
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
            double startWidth = Area.ActualValueToPoint(Area.InternalSecondaryAxis, 0);
            string path = IsActualTransposed ? "(Canvas.Left)" : "(Canvas.Top)";
            string scalePath = IsActualTransposed ? "(UIElement.RenderTransform).(ScaleTransform.ScaleX)" : "(UIElement.RenderTransform).(ScaleTransform.ScaleY)";
            string adornTransPath = IsActualTransposed ? "(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)" : "(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)";

            foreach (ChartSegment segment in Segments)
            {
                double elementSize;
                var element = (FrameworkElement)segment.GetRenderedVisual();
                if (this.ShowEmptyPoints)
                    elementSize = IsActualTransposed ? element.Width : element.Height;
                else
                {
                    var barSegment = segment as BarSegment;
                    elementSize = IsActualTransposed
                        ? barSegment.segmentSize.Width
                        : barSegment.segmentSize.Height;
                }

                if (!double.IsNaN(elementSize))
                {
                    double canvasSize = IsActualTransposed ? Canvas.GetLeft(element) : Canvas.GetTop(element);
                    element.RenderTransform = new ScaleTransform();
                    DoubleAnimationUsingKeyFrames keyFrames = new DoubleAnimationUsingKeyFrames();
                    SplineDoubleKeyFrame keyFrame = new SplineDoubleKeyFrame();
                    keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                    keyFrame.Value = startWidth;
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
                        //keyFrame1.Value = (YValues[i] > 0) ? -(elementWidth * 10) / 100 : (elementWidth * 10) / 100;
                        keyFrame1.Value = -(elementSize * 10) / 100;
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
        protected override ChartSegment CreateSegment()
        {
            return new StackedBarSegment();
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
        protected double CalculateSegmentSpacing(double spacing, double Right, double Left)
        {
            double diff = Right - Left;
            double totalspacing = diff * spacing / 2;
            Left = Left + totalspacing;
            Right = Right - totalspacing;
            return Left;
        }
        
#endregion

#region Private Static Methods

        private static void OnSegmentSelectionBrush(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as StackedBarSeries).UpdateArea();
        }
        
        private static void OnSegmentSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as StackedBarSeries;
            if (series.Area != null)
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
        
        private static void OnCustomTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as StackedBarSeries;

            if (series.Area == null) return;

            series.Segments.Clear();
            series.Area.ScheduleUpdate();
        }

#endregion

#endregion
    }
}
