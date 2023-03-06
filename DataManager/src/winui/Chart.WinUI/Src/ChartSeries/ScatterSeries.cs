using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="ScatterSeries"/> displays a collection of data points represented by a circle of equal size.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="ScatterSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XyDataSeries.StrokeWidth"/>, <see cref="Stroke"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>Size - </b> Specify the circle size using the <see cref="PointHeight"/>, and <see cref="PointWidth"/> properties.</para>
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="ScatterSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="ScatterSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
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
    ///           <chart:ScatterSeries ItemsSource="{Binding Data}"
    ///                               XBindingPath="XValue"
    ///                               YBindingPath="YValue"
    ///                               PointHeight = "30"
    ///                               PointWidth = "30"/>
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
    ///     ScatterSeries series = new ScatterSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.PointHeight = 30;
    ///     series.PointWidth = 30;
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
    ///        Data.Add(new Model() { XValue = 10, YValue = 100});
    ///        Data.Add(new Model() { XValue = 20, YValue = 150});
    ///        Data.Add(new Model() { XValue = 30, YValue = 110});
    ///        Data.Add(new Model() { XValue = 40, YValue = 230});
    ///     }
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    /// <seealso cref="ScatterSegment"/>
    public class ScatterSeries : XyDataSeries
    {
        #region Dependency Property Registration
        
        /// <summary>
        /// The DependencyProperty for <see cref="DragDirection"/> property.       
        /// </summary>
        /// Using a DependencyProperty as the backing store for DragDirection.  This enables animation, styling, binding, etc...
        internal static readonly DependencyProperty DragDirectionProperty =
            DependencyProperty.Register(
                nameof(DragDirection), 
                typeof(DragType),
                typeof(ScatterSeries), 
                new PropertyMetadata(DragType.XY));

        /// <summary>
        /// The DependencyProperty for <see cref="PointWidth"/> property.       
        /// </summary>
        public static readonly DependencyProperty PointWidthProperty =
            DependencyProperty.Register(
                nameof(PointWidth),
                typeof(double),
                typeof(ScatterSeries),
                new PropertyMetadata(20d, OnScatterWidthChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="PointHeight"/> property.       
        /// </summary>
        public static readonly DependencyProperty PointHeightProperty =
            DependencyProperty.Register(
                nameof(PointHeight), 
                typeof(double), 
                typeof(ScatterSeries),
                new PropertyMetadata(20d, OnScatterHeightChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="CustomTemplate"/> property.       
        /// </summary>
        public static readonly DependencyProperty CustomTemplateProperty =
            DependencyProperty.Register(
                nameof(CustomTemplate), 
                typeof(DataTemplate), 
                typeof(ScatterSeries),
                new PropertyMetadata(null, OnCustomTemplateChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(ScatterSeries),
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
        /// 
        /// </summary>
        internal DragType DragDirection
        {
            get { return (DragType)GetValue(DragDirectionProperty); }
            set { SetValue(DragDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that defines the width of the scatter segment size.
        /// </summary>
        /// <value>It accepts double values and its default value is 20.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ScatterSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            PointWidth = "30"/>
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
        ///     ScatterSeries series = new ScatterSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           PointWidth = 30,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double PointWidth
        {
            get { return (double)GetValue(PointWidthProperty); }
            set { SetValue(PointWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that defines the height of the scatter segment size.
        /// </summary>
        /// <value>It accepts double values and its default value is 20.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ScatterSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            PointHeight = "30"/>
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
        ///     ScatterSeries series = new ScatterSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           PointHeight = 30,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double PointHeight
        {
            get { return (double)GetValue(PointHeightProperty); }
            set { SetValue(PointHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the template to customize the appearance of scatter series.
        /// </summary>
        /// <value>It accepts <see cref="DataTemplate"/> value.</value>
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
        ///          <chart:ScatterSeries ItemsSource="{Binding Data}"
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
        ///     ScatterSeries scatterSeries = new ScatterSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth= 3,
        ///     };
        ///     
        ///     chart.Series.Add(scatterSeries);
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

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of ScatterSeries.
        /// </summary>
        internal override void GenerateSegments()
        {
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
                    for (int i = 0; i < xValues.Count; i++)
                    {
                        if (i < GroupedSeriesYValues[0].Count)
                        {
                            ScatterSegment scatterSegment = CreateSegment() as ScatterSegment;
                            if (scatterSegment != null)
                            {
                                scatterSegment.Series = this;
                                scatterSegment.CustomTemplate = CustomTemplate;
                                scatterSegment.SetData(xValues[i], GroupedSeriesYValues[0][i]);
                                scatterSegment.YData = GroupedSeriesYValues[0][i];
                                scatterSegment.XData = xValues[i];
                                scatterSegment.Item = ActualData[i];
                                Segments.Add(scatterSegment);
                            }
                            if (AdornmentsInfo != null && ShowDataLabels)
                                AddAdornments(xValues[i], GroupedSeriesYValues[0][i], i);
                        }
                    }
                }
                else
                {
                    ClearUnUsedSegments(this.PointsCount);
                    ClearUnUsedAdornments(this.PointsCount);
                    for (int i = 0; i < this.PointsCount; i++)
                    {
                        if (i < Segments.Count)
                        {
                            (Segments[i].Item) = ActualData[i];
                            (Segments[i]).SetData(xValues[i], YValues[i]);
                            (Segments[i] as ScatterSegment).XData = xValues[i];
                            (Segments[i] as ScatterSegment).Item = ActualData[i];
                        }
                        else
                        {
                            ScatterSegment scatterSegment = CreateSegment() as ScatterSegment;
                            if (scatterSegment != null)
                            {
                                scatterSegment.Series = this;
                                scatterSegment.CustomTemplate = CustomTemplate;
                                scatterSegment.SetData(xValues[i], YValues[i]);
                                scatterSegment.YData = YValues[i];
                                scatterSegment.XData = xValues[i];
                                scatterSegment.Item = ActualData[i];
                                Segments.Add(scatterSegment);
                            }
                        }

                        if (AdornmentsInfo != null && ShowDataLabels)
                            AddAdornments(xValues[i], YValues[i], i);
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
                Ellipse ellipse = segment.GetRenderedVisual() as Ellipse;
                if (ellipse != null && EllipseContainsPoint(ellipse, point))
                    return Segments.IndexOf(segment);
            }

            return -1;
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            ScatterSegment scatterSegment = ToolTipTag as ScatterSegment;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(scatterSegment.XData, scatterSegment.YData);
            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top - scatterSegment.PointHeight / 2;

            if (newPosition.Y - tooltip.DesiredSize.Height < ActualArea.SeriesClipRect.Top)
            {
                newPosition.Y += scatterSegment.PointHeight;
            }

            return newPosition;
        }

        internal override bool GetAnimationIsActive()
        {
            return sb != null && sb.GetCurrentState() == ClockState.Active;
        }

        internal override void Animate()
        {
            int i = 0;
            Random rand = new Random();

            // WPF-25124 Animation not working properly when resize the window.
            if (sb != null)
            {
                sb.Stop();
                if (!canAnimate)
                {
                    foreach (ScatterSegment segment in this.Segments)
                    {
                        FrameworkElement element = segment.GetRenderedVisual() as FrameworkElement;
                        element.ClearValue(FrameworkElement.RenderTransformProperty);
                    }

                    ResetAdornmentAnimationState();
                    return;
                }
            }

            sb = new Storyboard();
            foreach (ScatterSegment segment in this.Segments)
            {
                int randomValue = rand.Next(0, 50);
                TimeSpan beginTime = TimeSpan.FromMilliseconds(randomValue * 20);

                var element = (FrameworkElement)segment.GetRenderedVisual();

                // UWP-8445 Fix for xamarin forms uwp animation for custom template in scatter series.
                if (CustomTemplate != null)
                {
                    var contentPresenter = VisualTreeHelper.GetChild(element, 0);
                    var canvas = VisualTreeHelper.GetChild(contentPresenter, 0) as Canvas;

                    if (canvas != null)
                    {
                        var customElement = VisualTreeHelper.GetChild(canvas, 0) as FrameworkElement;

                        if (customElement != null)
                        {
                            element = customElement;
                        }
                    }
                }

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
                keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 30) / 100));

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
                keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                keyFrame.Value = 0;
                keyFrames.KeyFrames.Add(keyFrame);
                keyFrame = new SplineDoubleKeyFrame();
                ////keyFrame.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(AnimationDuration.TotalMilliseconds - (randomValue * (2 * AnimationDuration.TotalMilliseconds) / 100)));
                keyFrame.KeyTime = keyFrame.KeyTime.GetKeyTime(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 30) / 100));

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

                    DoubleAnimation animation = new DoubleAnimation() { To = 1, From = 0, BeginTime = TimeSpan.FromSeconds(beginTime.TotalSeconds + (beginTime.Seconds * 90) / 100) };
                    animation.Duration = new Duration().GetDuration(TimeSpan.FromSeconds((AnimationDuration.TotalSeconds * 50) / 100));
                    Storyboard.SetTargetProperty(animation, "FrameworkElement.Opacity");
                    Storyboard.SetTarget(animation, label);
                    sb.Children.Add(animation);
                }

                i++;
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
        internal override ChartSegment CreateSegment()
        {
            return new ScatterSegment();
        }

#endregion

#region Private Static Methods

        
        private static void OnCustomTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as ScatterSeries;

            if (series.Chart == null) return;

            series.Segments.Clear();
            series.Chart.ScheduleUpdate();
        }

        /// <summary>
        /// This method used to check the position within the ellipse
        /// </summary>
        /// <param name="Ellipse"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private static bool EllipseContainsPoint(Ellipse Ellipse, Point point)
        {
            Point center = new Point(
                  Canvas.GetLeft(Ellipse) + (Ellipse.Width / 2),
                  Canvas.GetTop(Ellipse) + (Ellipse.Height / 2));

            double x = Ellipse.Width / 2;
            double y = Ellipse.Height / 2;

            if (x <= 0.0 || y <= 0.0)
                return false;

            Point result = new Point(
                point.X - center.X,
                point.Y - center.Y);

            return ((double)(result.X * result.X)
                     / (x * x)) + ((double)(result.Y * result.Y) / (y * y))
                <= 1.0;
        }

        private static void OnScatterHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScatterSeries series = d as ScatterSeries;

            foreach (ChartSegment segment in series.Segments)
            {
                (segment as ScatterSegment).PointHeight = series.PointHeight;
            }

            if (series != null)
                series.ScheduleUpdateChart();
        }

        private static void OnScatterWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScatterSeries series = d as ScatterSeries;

            foreach (ChartSegment segment in series.Segments)
            {
                (segment as ScatterSegment).PointWidth = series.PointWidth;
            }

            if (series != null)
                series.ScheduleUpdateChart();
        }

#endregion

#region Private Methods

        private void AddAdornments(double x, double yValue, int i)
        {
            double adornX = 0d, adornY = 0d;
            adornX = x;
            adornY = yValue;
            if (i < Adornments.Count)
            {
                Adornments[i].SetData(adornX, adornY, adornX, adornY);
            }
            else
            {
                Adornments.Add(this.CreateAdornment(this, adornX, adornY, adornX, adornY));
            }

            Adornments[i].Item = ActualData[i];
        }

#endregion

#endregion
    }
}