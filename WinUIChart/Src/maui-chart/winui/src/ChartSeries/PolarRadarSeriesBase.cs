using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using System.Collections;
using AdornmentSeries = Syncfusion.UI.Xaml.Charts.DataMarkerSeries;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a base class for polar, radar series in chart.
    /// </summary>
    public abstract class PolarRadarSeriesBase : AdornmentSeries, ISupportAxes2D
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>DataLabelSettings</c> dependency property.
        /// </value>  
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(PolarDataLabelSettings), typeof(PolarRadarSeriesBase),
          new PropertyMetadata(null, OnAdornmentsInfoChanged));

        /// <summary>
        /// Identifies the YBindingPath dependency property.
        /// </summary>
        /// <value>
        /// The identifier for YBindingPath dependency property.
        /// </value>   
        public static readonly DependencyProperty YBindingPathProperty =
            DependencyProperty.Register(
                nameof(YBindingPath), 
                typeof(string), 
                typeof(PolarRadarSeriesBase),
                new PropertyMetadata(null, OnYPathChanged));

        /// <summary>
        /// Identifies the IsClosed dependency property.
        /// </summary>
        /// <value>
        /// The identifier for IsClosed dependency property.
        /// </value>   
        public static readonly DependencyProperty IsClosedProperty =
            DependencyProperty.Register(
                nameof(IsClosed),
                typeof(bool), 
                typeof(PolarRadarSeriesBase),
                new PropertyMetadata(true, OnDrawValueChanged));

        /// <summary>
        /// Identifies the <c>DrawType</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>DrawType</c> dependency property.
        /// </value>   
        public static readonly DependencyProperty DrawTypeProperty =
            DependencyProperty.Register(
                nameof(DrawType), 
                typeof(ChartSeriesDrawType),
                typeof(PolarRadarSeriesBase),
                new PropertyMetadata(ChartSeriesDrawType.Area, OnDrawValueChanged));

        /// <summary>
        /// Identifies the XAxis dependency property.
        /// </summary>
        /// <value>
        /// The identifier for XAxis dependency property.
        /// </value>   
        internal static readonly DependencyProperty XAxisProperty =
            DependencyProperty.Register(
                nameof(XAxis), 
                typeof(ChartAxisBase2D),
                typeof(PolarRadarSeriesBase),
                new PropertyMetadata(null, OnXAxisChanged));
                
        /// <summary>
        /// Identifies the YAxis dependency property.
        /// </summary>
        /// <value>
        /// The identifier for YAxis dependency property.
        /// </value>   
        internal static readonly DependencyProperty YAxisProperty =
            DependencyProperty.Register(
                nameof(YAxis),
                typeof(RangeAxisBase),
                typeof(PolarRadarSeriesBase),
                new PropertyMetadata(null, OnYAxisChanged));

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
                typeof(PolarRadarSeriesBase),
                new PropertyMetadata(null, OnDrawValueChanged));

        #endregion

        #region Fields

        #region Private Fields

        Storyboard sb;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the PolarRadarSeriesBase class.
        /// </summary>
        public PolarRadarSeriesBase()
        {
            YValues = new List<double>();
            DataLabelSettings = new PolarDataLabelSettings();
        }

        #endregion

        #region Properties

        #region Public Properties
        /// <summary>
        /// <para>Gets or sets data labels for the polar series.</para> <para>This allows us to customize the appearance of a data point by displaying labels, shapes and connector lines.</para>
        /// </summary>
        /// <value>
        /// The <see cref="ChartDataLabelSettings" /> value.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///           <chart:SfPolarChart.PrimaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfPolarChart.PrimaryAxis>
        ///
        ///           <chart:SfPolarChart.SecondaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfPolarChart.SecondaryAxis>
        ///
        ///           <chart:SfPolarChart.Series>
        ///               <chart:PolarAreaSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue" ShowDataLabels="True">
        ///                    <syncfusion:PolarAreaSeries.DataLabelSettings>
        ///                          <chart:PolarDataLabelSettings />
        ///                    <syncfusion:PolarAreaSeries.DataLabelSettings>
        ///               </chart:ColumnSeries> 
        ///           </chart:SfPolarChart.Series>
        ///           
        ///     </chart:SfPolarChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     
        ///     NumericalAxis primaryAxis = new NumericalAxis();
        ///     NumericalAxis secondaryAxis = new NumericalAxis();
        ///     
        ///     chart.PrimaryAxis = primaryAxis;
        ///     chart.SecondaryAxis = secondaryAxis;
        ///     
        ///     PolarAreaSeries series = new PolarAreaSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.ShowDataLabels = "True";
        ///     chart.Series.Add(series);
        ///     
        ///     series.DataLabelSettings = new PolarDataLabelSettings(){ };
        /// ]]></code>
        /// ***
        /// </example>
        public PolarDataLabelSettings DataLabelSettings
        {
            get
            {
                return (PolarDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }

            set
            {
                SetValue(DataLabelSettingsProperty, value);
            }
        }

        internal override ChartDataLabelSettings AdornmentsInfo
        {
            get
            {
                return (PolarDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }
        }


        /// <summary>
        /// Gets or sets the property name that associates the secondary axis with a property in the itemssource.
        /// </summary>
        /// <value>
        /// The string that represents the property name for secondary axis. The default value is null.
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
        ///           <chart:PolarSeries
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
        ///     PolarSeries series = new PolarSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        /// ]]></code>
        /// ***
        /// </example>
        public string YBindingPath
        {
            get { return (string)GetValue(YBindingPathProperty); }
            set { SetValue(YBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether area path should be closed or opened for Polar/Radar series.
        /// </summary>
        /// <value>
        ///  If its <c>true</c>, Area stroke will be closed; otherwise stroke will be applied on top of the series only.
        /// </value>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of series to be drawn for Radar or Polar series.
        /// </summary>
        /// <value>One of the <see cref="ChartSeriesDrawType"/> enumeration values. The default value is <see cref="ChartSeriesDrawType.Area"/>.</value>
        public ChartSeriesDrawType DrawType
        {
            get { return (ChartSeriesDrawType)GetValue(DrawTypeProperty); }
            set { SetValue(DrawTypeProperty, value); }
        }
                      
        /// <summary>
        /// Gets the series x-axis start and end range values. 
        /// </summary>
        /// <value>A <c>DoubleRange</c> specifies the start and end range of x-axis.</value>
        public DoubleRange XRange { get; internal set; }

        /// <summary>
        /// Gets the series y-axis start and end range values.
        /// </summary>
        /// <value>A <c>DoubleRange</c> specifies the start and end range of y-axis.</value>
        public DoubleRange YRange { get; internal set; }

        /// <summary>
        /// Gets or sets the multiple axis is not applicable for Radar/Polar series.
        /// </summary>
        /// <value>It takes the <see cref="ChartAxisBase2D"/> value.</value>
        internal ChartAxisBase2D XAxis
        {
            get { return (ChartAxisBase2D)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets the multiple axis is not applicable for Radar/Polar series.
        /// </summary>
        /// <value>It takes the <see cref="RangeAxisBase"/> value.</value>
        internal RangeAxisBase YAxis
        {
            get { return (RangeAxisBase)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke dash array for line to customize the appearance of <see cref="PolarSeries"/> and <see cref="RadarSeries"/>.
        /// </summary>
        /// <value>
        /// It takes <see cref="DoubleCollection"/> value and default value is null.
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
        ///           <chart:PolarSeries
        ///               StrokeDashArray="5,3"
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
        ///     PolarSeries series = new PolarSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///     
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(5);
        ///     doubleCollection.Add(3);
        ///     series.StrokeDashArray = doubleCollection;
        /// ]]></code>
        /// ***
        /// </example>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        ///<inheritdoc/>
        ChartAxis ISupportAxes.ActualXAxis
        {
            get { return ActualXAxis; }
        }

        ///<inheritdoc/>
        ChartAxis ISupportAxes.ActualYAxis
        {
            get { return ActualYAxis; }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets or sets YValues to render the series.
        /// </summary>
        /// <value>It takes the collection of double values.</value>
        protected IList<double> YValues { get; set; }

        /// <summary>
        /// Gets or sets the chart segment.
        /// </summary>
        /// <value>It takes the chart segment value.</value>
        internal ChartSegment Segment { get; set; }

        /// <inheritdoc/>
        RangeAxisBase ISupportAxes2D.YAxis { get; set; }

        /// <inheritdoc/>
        ChartAxisBase2D ISupportAxes2D.XAxis { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Finds the nearest point in ChartSeries relative to the mouse point/touch position.
        /// </summary>
        /// <param name="point">The co-ordinate point representing the current mouse point /touch position.</param>
        /// <param name="x">x-value of the nearest point.</param>
        /// <param name="y">y-value of the nearest point.</param>
        /// <param name="stackedYValue">The stacked y value.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal override void FindNearestChartPoint(Point point, out double x, out double y, out double stackedYValue)
        {
            x = double.NaN;
            y = double.NaN;
            stackedYValue = double.NaN;

            // Converting the mouse point to x chart value.
            double center = 0.5 * Math.Min(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height);
            double radian = ChartTransform.PointToPolarRadian(point, center);
            double coeff = ChartTransform.RadianToPolarCoefficient(radian);
            double chartX = this.Area.InternalPrimaryAxis.PolarCoefficientToValue(coeff);
            double xStart = this.ActualXAxis.VisibleRange.Start;
            double xEnd = this.ActualXAxis.VisibleRange.End;

            if (chartX <= xEnd && chartX >= xStart)
            {
                if (this.IsIndexed || !(this.ActualXValues is IList<double>))
                {
                    double nearestRange = Math.Round(chartX);
                    var xValues = this.GetXValues();

                    if (xValues != null && nearestRange < xValues.Count && nearestRange >= 0 && YValues != null)
                    {
                        x = NearestSegmentIndex = xValues.IndexOf((int)nearestRange);
                        y = YValues[NearestSegmentIndex];
                    }
                }
                else
                {
                    ChartPoint nearPoint = new ChartPoint();
                    IList<double> xValues = this.ActualXValues as IList<double>;
                    nearPoint.X = xStart;
                    nearPoint.Y = ActualYAxis.VisibleRange.Start;
                    int index = 0;

                    if (xValues != null && YValues != null)
                    {
                        foreach (var x1 in xValues)
                        {
                            double y1 = YValues[index];
                            if (Math.Abs(chartX - x1) <= Math.Abs(chartX - nearPoint.X))
                            {
                                nearPoint = new ChartPoint(x1, y1);
                                x = x1;
                                y = y1;
                                NearestSegmentIndex = index;
                            }

                            index++;
                        }
                    }
                }
            }
        }

        #endregion

        #region Internal Override Methods

        internal override void SetDataLabelsVisibility(bool isShowDataLabels)
        {
            if (DataLabelSettings != null)
            {
                DataLabelSettings.Visible = isShowDataLabels;
            }
        }

        internal override void ResetAdornmentAnimationState()
        {
            if (adornmentInfo != null)
            {
                foreach (FrameworkElement child in this.AdornmentPresenter.Children)
                {
                    child.ClearValue(FrameworkElement.RenderTransformProperty);
                    child.ClearValue(FrameworkElement.OpacityProperty);
                }
            }
        }

        internal override bool GetAnimationIsActive()
        {
            return sb != null && sb.GetCurrentState() == ClockState.Active;
        }

        internal override void Animate()
        {
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

            string propertyXPath = "(UIElement.RenderTransform).(ScaleTransform.ScaleX)";
            string propertyYPath = "(UIElement.RenderTransform).(ScaleTransform.ScaleY)";

            var panel = (ActualArea as ChartBase).GridLinesPanel;
            Point center = new Point(panel.ActualWidth / 2, panel.ActualHeight / 2);

            if (this.DrawType == ChartSeriesDrawType.Area)
            {
                var segmentCanvas = this.Segment.GetRenderedVisual();
                var path = (segmentCanvas as Canvas).Children[0];
                path.RenderTransform = new ScaleTransform() { CenterX = center.X, CenterY = center.Y };
                AnimateElement(path, propertyXPath, propertyYPath);
            }
            else
            {
                foreach (var segment in this.Segments)
                {
                    var lineSegment = segment.GetRenderedVisual();
                    lineSegment.RenderTransform = new ScaleTransform() { CenterX = center.X, CenterY = center.Y };
                    AnimateElement(lineSegment, propertyXPath, propertyYPath);
                }
            }

            AnimateAdornments();
            sb.Begin();
        }

        internal override void UpdateRange()
        {
            XRange = DoubleRange.Empty;
            YRange = DoubleRange.Empty;

            foreach (ChartSegment segment in Segments)
            {
                XRange += segment.XRange;
                YRange += segment.YRange;
            }
        }

        /// <summary>
        /// Validate the datapoints for segment implementation.
        /// </summary>
        internal override void ValidateYValues()
        {
            foreach (var yValue in YValues)
            {
                if (double.IsNaN(yValue) && ShowEmptyPoints)
                    ValidateDataPoints(YValues); break;
            }
        }

        /// <summary>
        /// Validate the datapoints for segment implementation.
        /// </summary>
        internal override void ReValidateYValues(List<int>[] emptyPointIndex)
        {
            foreach (var item in emptyPointIndex)
            {
                foreach (var index in item)
                    YValues[index] = double.NaN;
            }
        }

        internal override void RemoveTooltip()
        {
            if (this.Area == null)
            { 
                return; 
            }

            var canvas = this.Area.GetAdorningCanvas();
            
            if (canvas != null && canvas.Children.Contains((this.Area.Tooltip as ChartTooltip)))
                canvas.Children.Remove(this.Area.Tooltip as ChartTooltip);
        }

        internal override int GetDataPointIndex(Point point)
        {
            Canvas canvas = Area.GetAdorningCanvas();
            double left = Area.ActualWidth - canvas.ActualWidth;
            double top = Area.ActualHeight - canvas.ActualHeight;

            point.X = point.X - left - Area.SeriesClipRect.Left + Area.Margin.Left;
            point.Y = point.Y - top - Area.SeriesClipRect.Top + Area.Margin.Top;
            double xVal = 0;
            double xStart = ActualXAxis.VisibleRange.Start;
            double xEnd = ActualXAxis.VisibleRange.End;
            int index = -1;
            double center = 0.5 * Math.Min(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height);
            double radian = ChartTransform.PointToPolarRadian(point, center);
            double coeff = ChartTransform.RadianToPolarCoefficient(radian);
            xVal = Math.Round(this.Area.InternalPrimaryAxis.PolarCoefficientToValue(coeff));
            if (xVal <= xEnd && xVal >= xStart)
                index = this.GetXValues().IndexOf((int)xVal);
            return index;
        }

        internal override void UpdateTooltip(object originalSource)
        {
            if (ShowTooltip)
            {
                var shape = (originalSource as Shape);
                if (shape == null || (shape != null && shape.Tag == null))
                    return;
                SetTooltipDuration();
                var canvas = this.Area.GetAdorningCanvas();

                object data = null;
                double x = double.NaN;
                double y = double.NaN; 
                double stackYValue = double.NaN;
                if (this.Area.SeriesClipRect.Contains(mousePos))
                {
                    var point = new Point(
                        mousePos.X - this.Area.SeriesClipRect.Left,
                        mousePos.Y - this.Area.SeriesClipRect.Top);
                   
                    FindNearestChartPoint(point, out x, out y, out stackYValue);

                    if (NearestSegmentIndex > -1 && NearestSegmentIndex < ActualData.Count)
                        data = this.ActualData[NearestSegmentIndex];
                }

                var chartTooltip = this.Area.Tooltip as ChartTooltip;
                if (this.DrawType == ChartSeriesDrawType.Area)
                {
                    var areaSegment = shape.Tag as AreaSegment;
                    areaSegment.Item = data;
                    areaSegment.XData = x;
                    areaSegment.YData = y;
                }
                else
                {
                    var lineSegment = shape.Tag as LineSegment;
                    lineSegment.Item = data;
                    lineSegment.YData = y;
                }

                if (chartTooltip != null)
                {
                    var tag = shape.Tag;
                    ToolTipTag = tag;
                    chartTooltip.PolygonPath = " ";
                    chartTooltip.DataContext = tag;

                    if (canvas.Children.Count == 0 || (canvas.Children.Count > 0 && !IsTooltipAvailable(canvas)))
                    {
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
                            _stopwatch.Start();
                        }
                        else
                        {
                            Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                            Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                            _stopwatch.Start();
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

                        AddTooltip();
                        Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                        Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                    }
                }
            }
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            return mousePos;
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
        /// Method used to generate the data points for Polar and Radar series.
        /// </summary>
        protected internal override void GeneratePoints()
        {
            GeneratePoints(new string[] { YBindingPath }, YValues);
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Invoked when YAxis property changed.
        /// </summary>
        /// <param name="oldAxis">Old chartaxis value.</param>
        /// <param name="newAxis">New chartaxis value.</param>
        internal virtual void OnYAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
        {
            if (oldAxis != null)
            {
                if (oldAxis.RegisteredSeries.Contains(this))
                    oldAxis.RegisteredSeries.Remove(this);
                if (Area != null && oldAxis.RegisteredSeries.Count == 0)
                {
                    if (Area.InternalAxes.Contains(oldAxis))
                    {
                        if (Area.InternalSecondaryAxis != null && Area.InternalSecondaryAxis == oldAxis)
                        {
                            Area.InternalAxes.Remove(oldAxis);
                            if (Area.InternalSecondaryAxis.IsDefault)
                            {
                                var cartersianArea = Area as SfCartesianChart;
                                if (cartersianArea != null)
                                {
                                    cartersianArea.SecondaryAxis = null;
                                }
                                else
                                {
#pragma warning disable CS0618 // Type or member is obsolete
                                    (Area as SfChart).SecondaryAxis = null;
#pragma warning restore CS0618 // Type or member is obsolete
                                } 
                               
                                if (Area.InternalPrimaryAxis != null &&
                                    Area.InternalPrimaryAxis.AssociatedAxes.Contains(oldAxis))
                                    Area.InternalPrimaryAxis.AssociatedAxes.Remove(oldAxis);
                            }
                        }
                    }
                }
            }

            if (newAxis != null && !newAxis.RegisteredSeries.Contains(this))
            {
                newAxis.Area = Area;
                if (Area != null && !Area.InternalAxes.Contains(newAxis))
                    Area.InternalAxes.Add(newAxis);
                newAxis.Orientation = Orientation.Vertical;
                newAxis.RegisteredSeries.Add(this);
            }

            if (Area != null) Area.ScheduleUpdate();
        }

        /// <summary>
        /// Invoked when XAxis property changed.
        /// </summary>
        /// <param name="oldAxis">Old chartaxis value.</param>
        /// <param name="newAxis">New chartaxis value.</param>
        internal virtual void OnXAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
        {
            if (oldAxis != null)
            {
                if (oldAxis.RegisteredSeries.Contains(this))
                    oldAxis.RegisteredSeries.Remove(this);

                if (Area != null && oldAxis.RegisteredSeries.Count > 0)
                {
                    if (Area.InternalAxes.Contains(oldAxis))
                    {
                        if (Area.InternalPrimaryAxis != null && Area.InternalPrimaryAxis == oldAxis)
                        {
                            Area.InternalAxes.Remove(oldAxis);
                            if (Area.InternalPrimaryAxis.IsDefault)
                            {
                               Area.SetPrimaryAxis(null);
                                if (Area.InternalSecondaryAxis != null &&
                                    Area.InternalSecondaryAxis.AssociatedAxes.Contains(oldAxis))
                                    Area.InternalSecondaryAxis.AssociatedAxes.Remove(oldAxis);
                            }
                        }
                    }
                }
            }

            if (newAxis != null)
            {
                if (Area != null && !Area.InternalAxes.Contains(newAxis))
                    Area.InternalAxes.Add(newAxis);
                newAxis.Orientation = Orientation.Horizontal;
                if (!newAxis.RegisteredSeries.Contains(this))
                    newAxis.RegisteredSeries.Add(this);
            }

            if (Area != null) Area.ScheduleUpdate();
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            YValues.Clear();
            Segment = null;
            GeneratePoints(new string[] { YBindingPath }, YValues);
            isPointValidated = false;
            this.UpdateArea();
        }

        /// <summary>
        /// Invoked when XBindingPath or YBindingPath properties changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> that contains the event data</param>
        /// <see cref="ChartSeriesBase.XBindingPath"/>
        /// <see cref="PolarRadarSeriesBase.YBindingPath"/>
        protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
        {
            YValues.Clear();
            Segment = null;
            base.OnBindingPathChanged(args);
        }

#if NETFX_CORE
        /// <summary>
        /// Used to update the series tooltip when tapping on the series segments.
        /// </summary>
        /// <param name="e">The <see cref="TappedRoutedEventArgs"/> that contains the event data.</param>
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Touch)
                UpdateTooltip(e.OriginalSource);
        }
#endif

        #endregion

        #region Private Static Methods

        private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PolarRadarSeriesBase).OnBindingPathChanged(e);
        }

        private static void OnDrawValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PolarRadarSeriesBase).UpdateArea();
        }

        private static void OnYAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PolarRadarSeriesBase).OnYAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
        }

        private static void OnXAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PolarRadarSeriesBase).OnXAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Timer Tick Handler for closing the Tooltip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        void timer_Tick(object sender, object e)
        {
            RemoveTooltip();
            Timer.Stop();
        }

        private void AnimateAdornments()
        {
            if (this.AdornmentsInfo != null && ShowDataLabels)
            {
                foreach (var child in this.AdornmentPresenter.Children)
                {
                    DoubleAnimationUsingKeyFrames keyFrames1 = new DoubleAnimationUsingKeyFrames();

                    SplineDoubleKeyFrame frame1 = new SplineDoubleKeyFrame();
                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(0));
                    frame1.Value = 0;
                    keyFrames1.KeyFrames.Add(frame1);

                    frame1 = new SplineDoubleKeyFrame();
                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(AnimationDuration);
                    frame1.Value = 0;
                    keyFrames1.KeyFrames.Add(frame1);

                    frame1 = new SplineDoubleKeyFrame();
                    frame1.KeyTime = frame1.KeyTime.GetKeyTime(TimeSpan.FromSeconds(AnimationDuration.TotalSeconds + 1));
                    frame1.Value = 1;
                    keyFrames1.KeyFrames.Add(frame1);

                    KeySpline keySpline = new KeySpline();
                    keySpline.ControlPoint1 = new Point(0.64, 0.84);

                    keySpline.ControlPoint2 = new Point(0, 1); // Animation have to provide same easing effect in all platforms.
                    keyFrames1.EnableDependentAnimation = true;
                    Storyboard.SetTargetProperty(keyFrames1, "(Opacity)");
                    frame1.KeySpline = keySpline;

                    Storyboard.SetTarget(keyFrames1, child as FrameworkElement);
                    sb.Children.Add(keyFrames1);
                }
            }
        }

        private void AnimateElement(UIElement element, string propertyXPath, string propertyYPath)
        {
            DoubleAnimation animation_X = new DoubleAnimation();
            animation_X.From = 0;
            animation_X.To = 1;
            animation_X.Duration = new Duration().GetDuration(AnimationDuration);
            Storyboard.SetTarget(animation_X, element);
            Storyboard.SetTargetProperty(animation_X, propertyXPath);
            animation_X.EnableDependentAnimation = true;
            sb.Children.Add(animation_X);

            DoubleAnimation animation_Y = new DoubleAnimation();
            animation_Y.From = 0;
            animation_Y.To = 1;
            animation_Y.Duration = new Duration().GetDuration(AnimationDuration);
            Storyboard.SetTarget(animation_Y, element);
            Storyboard.SetTargetProperty(animation_Y, propertyYPath);
            animation_Y.EnableDependentAnimation = true;
            sb.Children.Add(animation_Y);
        }

#endregion

#endregion
    }
}
