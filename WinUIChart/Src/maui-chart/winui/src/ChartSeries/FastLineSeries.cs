using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using SelectionStyle = Syncfusion.UI.Xaml.Charts.SelectionType;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a special kind of line series which uses poly lines for rendering chart points. FastLineSeries allows to render a collection with large number of data points.
    /// </summary>
    /// <remarks>
    /// FastLineSeries renders large quantity of data in fraction of milliseconds using poly lines. FastLineBitmapSeries also can be used for better performance which uses WriteableBitmap to render the series.
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
    ///           <chart:FastLineSeries
    ///               ItemsSource="{Binding Data}"
    ///               XBindingPath="XValue"
    ///               YBindingPath="YValue"/>
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
    ///     FastLineSeries series = new FastLineSeries();
    ///     series.ItemsSource = viewmodel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    /// ]]></code>
    /// # [ViewModel.cs](#tab/tabid-3)
    /// <code><![CDATA[
    /// public ObservableCollection<Model> Data { get; set; }
    /// 
    /// public ViewModel()
    /// {
    ///    Data = new ObservableCollection<Model>();
    ///    Data.Add(new Model() { XValue = 10, YValue = 100 });
    ///    Data.Add(new Model() { XValue = 20, YValue = 150 });
    ///    Data.Add(new Model() { XValue = 30, YValue = 110 });
    ///    Data.Add(new Model() { XValue = 40, YValue = 230 });
    /// }
    /// ]]></code>
    /// ***
    /// </example>
    /// <seealso cref="FastLineSegment"/>
    /// <seealso cref="FastLineBitmapSeries"/>
    /// <seealso cref="LineSeries"/>   
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
        /// Gets or sets the DataTemplate used to customize the poly line appearence. 
        /// </summary>
        /// <value>
        /// The template that specifies poly line inside canvas. The default is null.
        /// </value>
        /// <remarks>
        /// This data template should be loaded with poly line inside canvas where poly line appearence properties like stroke dasharray, stroke dashoffset, stroke dashcap, stroke linejoin can be customized.
        /// </remarks>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfChart>
        ///           <chart:SfChart.Resources>
        ///             <DataTemplate x:Key="seriesTemplate">
        ///                 <Canvas>
        ///                     <Polyline Points="{Binding Points}" StrokeDashArray="3,3" StrokeThickness="3" Stroke="Yellow"/>
        ///                 </Canvas>
        ///             </DataTemplate>
        ///           </chart:SfChart.Resources>
        ///          
        ///           <chart:SfChart.PrimaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfChart.PrimaryAxis>
        ///
        ///           <chart:SfChart.SecondaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfChart.SecondaryAxis>
        ///
        ///           <chart:FastLineSeries
        ///               CustomTemplate="{StaticResource seriesTemplate}"
        ///               ItemsSource="{Binding Data}"
        ///               XBindingPath="XValue"
        ///               YBindingPath="YValue"/>
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
        ///     FastLineSeries series = new FastLineSeries();
        ///     series.CustomTemplate = chart.Resources["seriesTemplate"] as DataTemplate;
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
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

        /// <inheritdoc/>
        internal override bool IsLinear
        {
            get
            {
                return true;
            }
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
        public override void CreateSegments()
        {
            if (GroupedSeriesYValues != null && GroupedSeriesYValues[0].Contains(double.NaN))
            {
                List<List<double>> yValList;
                List<List<double>> xValList;
                this.CreateEmptyPointSegments(GroupedSeriesYValues[0], out yValList, out xValList);
            }
            else if (YValues.Contains(double.NaN))
            {
                List<List<double>> yValList;
                List<List<double>> xValList;
                this.CreateEmptyPointSegments(YValues, out yValList, out xValList);
            }
            else
            {
                bool isGrouping = this.ActualXAxis is CategoryAxis ? (this.ActualXAxis as CategoryAxis).IsIndexed : true;
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
                    ClearUnUsedAdornments(this.DataCount);

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
        }

        /// <summary>
        /// MEthod used to create empty point segments.
        /// </summary>
        /// <param name="YValues">Yvalues</param>
        /// <param name="yValList">YValues list</param>
        /// <param name="xValList">XValues list</param>
        internal override void CreateEmptyPointSegments(IList<double> YValues, out List<List<double>> yValList, out List<List<double>> xValList)
        {
            if (this.ActualXAxis is CategoryAxis && (!(this.ActualXAxis as CategoryAxis).IsIndexed))
            {
                xValues = GroupedXValuesIndexes;
                base.CreateEmptyPointSegments(GroupedSeriesYValues[0], out yValList, out xValList);
            }
            else
            {
                xValues = (ActualXValues is IList<double> && !IsIndexed) ? ActualXValues as IList<double> : GetXValues();
                base.CreateEmptyPointSegments(YValues, out yValList, out xValList);
            }

            int j = 0;

            // EmptyPoint calculation
            if (Segments.Count != yValList.Count)
            {
                Segments.Clear();
            }

            ClearUnUsedAdornments(this.DataCount);
            if (Segment == null || Segments.Count == 0)
            {
                for (int i = 0; i < yValList.Count && i < xValList.Count; i++)
                {
                    if (i < xValList.Count && i < yValList.Count && xValList[i].Count > 0 && yValList[i].Count > 0)
                    {
                        Segment = new FastLineSegment(xValList[i], yValList[i], this);
                        Segments.Add(Segment);
                    }
                }
            }
            else if (xValues != null)
            {
                foreach (var segment in Segments)
                {
                    if (j < xValList.Count && j < yValList.Count && xValList[j].Count > 0 && yValList[j].Count > 0)
                    {
                        segment.SetData(xValList[j], yValList[j]);
                        (segment as FastLineSegment).SetRange();
                    }

                    j++;
                }
            }

            isAdornmentPending = true;
        }

        #endregion

        #region Internal Override Methods

        internal override void UpdateTooltip(object originalSource)
        {
            if (ShowTooltip)
            {
                ChartDataPointInfo dataPoint = new ChartDataPointInfo();
                if (((originalSource as FrameworkElement) is Polyline))
                {
                    dataPoint = GetDataPoint(mousePos);
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

            Point point = new Point(mousePos.X - Area.SeriesClipRect.Left, mousePos.Y - Area.SeriesClipRect.Top);

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
        protected override ChartSegment CreateSegment()
        {
            return new FastLineSegment();
        }


#if NETFX_CORE
        /// <summary>
        /// Used to update the series tooltip when tapping on the series segments.
        /// </summary>
        /// <param name="e"><see cref="TappedRoutedEventArgs"/> that contains the event data.</param>
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Touch)
                UpdateTooltip(e.OriginalSource);
        }
#endif

        /// <summary>
        /// Called when <see cref="ChartSeriesBase.ItemsSource"/> property changed.
        /// </summary>
        /// <param name="oldValue">ItemsSource old value.</param>
        /// <param name="newValue">ItemsSource new value</param>
        /// <seealso cref="ChartSeriesBase.XBindingPath"/>
        /// <seealso cref="XyDataSeries.YBindingPath"/>
        protected override void OnDataSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            Segment = null;
            base.OnDataSourceChanged(oldValue, newValue);
        }

        /// <summary>
        /// Invoked when VisibleRange property changed.
        /// </summary>
        /// <param name="e"><see cref="VisibleRangeChangedEventArgs"/> that contains the event data.</param>
        protected override void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
        {
            if (AdornmentsInfo != null && ShowDataLabels && isAdornmentPending)
            {
                if (xValues != null && ActualXAxis != null
                    && !ActualXAxis.VisibleRange.IsEmpty)
                {
                    double xBase = ActualXAxis.IsLogarithmic ? (ActualXAxis as LogarithmicAxis).LogarithmicBase : 1;
                    bool xIsLogarithmic = ActualXAxis.IsLogarithmic;
                    double start = ActualXAxis.VisibleRange.Start;
                    double end = ActualXAxis.VisibleRange.End;

                    for (int i = 0; i < DataCount; i++)
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

                        double edgeValue = xIsLogarithmic ? Math.Log(x, xBase) : x;
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
                }

                isAdornmentPending = false;
            }
        }

        #endregion

        #region Private Static Methods

        private static void OnCustomTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as FastLineSeries;

            if (series.Area == null) return;

            series.Segments.Clear();
            series.Area.ScheduleUpdate();
        }

        #endregion

        #endregion
    }
}
