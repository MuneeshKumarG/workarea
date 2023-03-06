using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;
using System;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// It is the base class for all types of polar series.
    /// </summary>
    public abstract class PolarSeries : ChartSeries
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>DataLabelSettings</c> dependency property.
        /// </value>  
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(PolarDataLabelSettings), typeof(PolarSeries),
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
                typeof(PolarSeries),
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
                typeof(PolarSeries),
                new PropertyMetadata(true, OnDrawValueChanged));

        /// <summary>
        /// Identifies the <c>DrawType</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>DrawType</c> dependency property.
        /// </value>   
        internal static readonly DependencyProperty DrawTypeProperty =
            DependencyProperty.Register(
                nameof(DrawType),
                typeof(ChartSeriesDrawType),
                typeof(PolarSeries),
                new PropertyMetadata(ChartSeriesDrawType.Area, OnDrawValueChanged));

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
                typeof(PolarSeries),
                new PropertyMetadata(null, OnDrawValueChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(PolarSeries),
            new PropertyMetadata(null, OnStrokeChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="StrokeWidth"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeWidthProperty =
            DependencyProperty.Register(nameof(StrokeWidth), typeof(double), typeof(PolarSeries),
            new PropertyMetadata(2d, OnStrokeChanged));

        #endregion

        #region Fields

        #region Private Fields

        Storyboard sb;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PolarRadarSeriesBase"/> class.
        /// </summary>
        public PolarSeries()
        {
            YValues = new List<double>();
            DataLabelSettings = new PolarDataLabelSettings();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to customize the appearance of the displaying data labels in the polar series.
        /// </summary>
        /// <remarks>This allows us to change the look of a data point by displaying labels, shapes, and connector lines.</remarks>
        /// <value>
        /// It takes the <see cref="PolarDataLabelSettings" />.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfPolarChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:PolarAreaSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" 
        ///                            ShowDataLabels="True">
        ///          <syncfusion:PolarAreaSeries.DataLabelSettings>
        ///              <chart:PolarDataLabelSettings />
        ///          <syncfusion:PolarAreaSeries.DataLabelSettings>
        ///     </chart:PolarAreaSeries> 
        ///
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// //omitted for brevity
        /// PolarAreaSeries series = new PolarAreaSeries();
        /// series.ItemsSource = viewmodel.Data;
        /// series.XBindingPath = "XValue";
        /// series.YBindingPath = "YValue";
        /// series.ShowDataLabels = true;
        /// chart.Series.Add(series);
        ///
        /// series.DataLabelSettings = new PolarDataLabelSettings();
        /// ]]>
        /// </code>
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
        /// Gets or sets a path value on the source object to serve a y value to the series.
        /// </summary>
        /// <value> The string that represents the property name for the y plotting data, and its default value is null.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfPolarChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:PolarAreaSeries ItemsSource="{Binding Data}" 
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"/>
        ///
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// //omitted for brevity
        /// PolarAreaSeries series = new PolarAreaSeries();
        /// series.ItemsSource = viewmodel.Data;
        /// series.XBindingPath = "XValue";
        /// series.YBindingPath = "YValue";
        /// chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string YBindingPath
        {
            get { return (string)GetValue(YBindingPathProperty); }
            set { SetValue(YBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the area path for the polar series should be closed or opened.
        /// </summary>
        /// <remarks> If its <c>true</c>, series path will be closed; otherwise opened.</remarks>
        /// <value>It accepts bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfPolarChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:PolarAreaSeries ItemsSource="{Binding Data}" 
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            IsClosed="True"/>
        ///
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// //omitted for brevity
        /// PolarAreaSeries series = new PolarAreaSeries();
        /// series.ItemsSource = viewmodel.Data;
        /// series.XBindingPath = "XValue";
        /// series.YBindingPath = "YValue";
        /// series.IsClosed = true;
        /// chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool IsClosed
        {
            get { return (bool)GetValue(IsClosedProperty); }
            set { SetValue(IsClosedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the stroke thickness of a chart series.
        /// </summary>
        /// <value>It accepts double values and its default value is 2.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PolarAreaSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red"
        ///                            StrokeWidth = "3"/>
        ///
        ///     </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     PolarAreaSeries polarAreaSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth= 3,
        ///     };
        ///     
        ///     chart.Series.Add(polarAreaSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double StrokeWidth {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of a chart series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-9)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:PolarAreaSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red"
        ///                            StrokeWidth = "3"/>
        ///
        ///     </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-10)
        ///<code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     PolarAreaSeries polarAreaSeries = new PolarAreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///           StrokeWidth= 3,
        ///     };
        ///     
        ///     chart.Series.Add(polarAreaSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush Stroke {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal ChartSeriesDrawType DrawType
        {
            get { return (ChartSeriesDrawType)GetValue(DrawTypeProperty); }
            set { SetValue(DrawTypeProperty, value); }
        }

        /// <summary>
        /// Gets the start and end range values of series x-axis. 
        /// </summary>
        internal DoubleRange VisibleXRange { get; set; }

        /// <summary>
        /// Gets the start and end range values of series y-axis.
        /// </summary>
        internal DoubleRange VisibleYRange { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DoubleCollection"/> value for stroke dash array to customize the stroke appearance of <see cref="PolarSeries"/>.
        /// </summary>
        /// <value>
        /// It takes <see cref="DoubleCollection"/> value and the default value is null.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfPolarChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:PolarLineSeries ItemsSource="{Binding Data}" 
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            StrokeDashArray="5,3"/>
        ///
        /// </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        /// 
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// // omitted for brevity
        /// PolarLineSeries series = new PolarLineSeries();
        /// series.ItemsSource = viewmodel.Data;
        /// series.XBindingPath = "XValue";
        /// series.YBindingPath = "YValue";
        /// chart.Series.Add(series);
        ///
        /// DoubleCollection doubleCollection = new DoubleCollection();
        /// doubleCollection.Add(5);
        /// doubleCollection.Add(3);
        /// series.StrokeDashArray = doubleCollection;
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

        #region Protected Properties

        /// <summary>
        /// Gets or sets YValues to render the series.
        /// </summary>
        /// <value>It takes the collection of double values.</value>
        internal IList<double> YValues { get; set; }

        /// <summary>
        /// Gets or sets the chart segment.
        /// </summary>
        /// <value>It takes the chart segment value.</value>
        internal ChartSegment Segment { get; set; }

        #endregion

        #endregion
        
        #region Methods

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
            VisibleXRange = DoubleRange.Empty;
            VisibleYRange = DoubleRange.Empty;

            foreach (ChartSegment segment in Segments)
            {
                VisibleXRange += segment.XRange;
                VisibleYRange += segment.YRange;
            }
        }

        internal override void RemoveTooltip()
        {
            if (this.Chart == null)
            {
                return;
            }

            var canvas = this.Chart.GetAdorningCanvas();

            if (canvas != null && canvas.Children.Contains((this.Chart.Tooltip as ChartTooltip)))
                canvas.Children.Remove(this.Chart.Tooltip as ChartTooltip);
        }

        internal override int GetDataPointIndex(Point point)
        {
            Canvas canvas = Chart.GetAdorningCanvas();
            double left = Chart.ActualWidth - canvas.ActualWidth;
            double top = Chart.ActualHeight - canvas.ActualHeight;

            point.X = point.X - left - Chart.SeriesClipRect.Left + Chart.Margin.Left;
            point.Y = point.Y - top - Chart.SeriesClipRect.Top + Chart.Margin.Top;
            double xVal = 0;
            double xStart = ActualXAxis.VisibleRange.Start;
            double xEnd = ActualXAxis.VisibleRange.End;
            int index = -1;
            double center = 0.5 * Math.Min(this.Chart.SeriesClipRect.Width, this.Chart.SeriesClipRect.Height);
            double radian = ChartTransform.PointToPolarRadian(point, center);
            double coeff = ChartTransform.RadianToPolarCoefficient(radian);
            xVal = Math.Round(this.Chart.InternalPrimaryAxis.PolarCoefficientToValue(coeff));
            if (xVal <= xEnd && xVal >= xStart)
                index = this.GetXValues().IndexOf((int)xVal);
            return index;
        }

        internal override void UpdateTooltip(object originalSource)
        {
            if (EnableTooltip)
            {
                var shape = (originalSource as Shape);
                if (shape == null || (shape != null && shape.Tag == null))
                    return;
                SetTooltipDuration();
                var canvas = this.Chart.GetAdorningCanvas();

                object data = null;
                double x = double.NaN;
                double y = double.NaN;
                double stackYValue = double.NaN;
                if (this.Chart.SeriesClipRect.Contains(mousePosition))
                {
                    var point = new Point(
                        mousePosition.X - this.Chart.SeriesClipRect.Left,
                        mousePosition.Y - this.Chart.SeriesClipRect.Top);

                    FindNearestChartPoint(point, out x, out y, out stackYValue);

                    if (NearestSegmentIndex > -1 && NearestSegmentIndex < ActualData.Count)
                        data = this.ActualData[NearestSegmentIndex];
                }

                var chartTooltip = this.Chart.Tooltip as ChartTooltip;
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

                        AddTooltip();
                        Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                        Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                    }
                }
            }
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            return mousePosition;
        }

        /// <summary>
        /// Method used to generate the data points for Polar and Radar series.
        /// </summary>
        internal override void GenerateDataPoints()
        {
            GeneratePoints(new string[] { YBindingPath }, YValues);
        }

        /// <inheritdoc/>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            if (AdornmentsInfo != null)
            {
                VisibleAdornments.Clear();
                Adornments.Clear();
                AdornmentsInfo.UpdateElements();
            }

            YValues.Clear();
            Segment = null;
            GeneratePoints(new string[] { YBindingPath }, YValues);
            this.ScheduleUpdateChart();
        }

        /// <summary>
        /// Invoked when XBindingPath or YBindingPath properties changed.
        /// </summary>
        /// <see cref="ChartSeries.XBindingPath"/>
        /// <see cref="PolarRadarSeriesBase.YBindingPath"/>
        internal override void OnBindingPathChanged()
        {
            YValues.Clear();
            Segment = null;
            ResetData();
            GeneratePoints(new[] { YBindingPath }, YValues);
            if (this.Chart != null && this.Chart.PlotArea != null)
                this.Chart.PlotArea.ShouldPopulateLegendItems = true;
            base.OnBindingPathChanged();
        }

        internal override void UpdateOnSeriesBoundChanged(Size size)
        {
            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                AdornmentsInfo.UpdateElements();
            }

            base.UpdateOnSeriesBoundChanged(size);

            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                AdornmentPresenter.Update(size);
                AdornmentPresenter.Arrange(size);
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

        #endregion

        #region Internal Virtual Methods

        /// <summary>
        /// Method implementation for add AreaAdornments in Chart.
        /// </summary>
        /// <param name="values">values</param>
        internal virtual void AddAreaAdornments(params IList<double>[] values)
        {
            IList<double> yValues = values[0];
            List<double> xValues = new List<double>();
            if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();

            if (values.Length == 1)
            {
                int i;
                for (i = 0; i < PointsCount; i++)
                {
                    if (i < xValues.Count && i < yValues.Count)
                    {
                        double adornX = xValues[i];
                        double adornY = yValues[i];

                        if (i < Adornments.Count)
                        {
                            Adornments[i].SetData(xValues[i], yValues[i], adornX, adornY);
                        }
                        else
                        {
                            Adornments.Add(CreateAdornment(this, xValues[i], yValues[i], adornX, adornY));
                        }

                        Adornments[i].Item = ActualData[i];
                    }
                }
            }
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AdornmentPresenter = new ChartDataMarkerPresenter();
            AdornmentPresenter.Series = this;

            if (Chart != null && AdornmentsInfo != null && ShowDataLabels)
            {
                Chart.DataLabelPresenter.Children.Add(AdornmentPresenter);
                AdornmentsInfo.PanelChanged(AdornmentPresenter);
            }
        }

#if NETFX_CORE
        /// <inheritdoc/>
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Touch)
                UpdateTooltip(e.OriginalSource);
        }
#endif
        #endregion

        #region Public Override Methods

        /// <inheritdoc/>
        internal override IChartTransformer CreateTransformer(Size size, bool create)
        {
            if (create || ChartTransformer == null)
            {
                ChartTransformer = ChartTransform.CreatePolar(new Rect(new Point(), size), this);
            }

            return ChartTransformer;
        }

        /// <summary>
        /// Creates the segments of PolarSeries.
        /// </summary>
        internal override void GenerateSegments()
        {
            List<double> tempYValues = new List<double>();
            Segments.Clear(); Segment = null;

            if (DrawType == ChartSeriesDrawType.Area)
            {
                double Origin = 0;
                List<double> xValues = GetXValues().ToList();
                tempYValues = (from val in YValues select val).ToList();

                if (xValues != null)
                {
                    if (!IsClosed)
                    {
                        xValues.Insert((int)PointsCount - 1, xValues[(int)PointsCount - 1]);
                        xValues.Insert(0, xValues[0]);
                        tempYValues.Insert(0, Origin);
                        tempYValues.Insert(tempYValues.Count, Origin);
                    }
                    else
                    {
                        xValues.Insert(0, xValues[0]);
                        tempYValues.Insert(0, YValues[0]);
                        xValues.Insert(0, xValues[(int)PointsCount]);
                        tempYValues.Insert(0, YValues[(int)PointsCount - 1]);
                    }

                    if (Segment == null)
                    {
                        Segment = CreateSegment() as AreaSegment;
                        if (Segment != null)
                        {
                            Segment.Series = this;
                            Segment.SetData(xValues, tempYValues);
                            Segments.Add(Segment);
                        }
                    }
                    else
                        Segment.SetData(xValues, tempYValues);

                    if (AdornmentsInfo != null && ShowDataLabels)
                        AddAreaAdornments(YValues);
                }
            }
            else if (DrawType == ChartSeriesDrawType.Line)
            {
                int index = -1;
                int i = 0;
                double xIndexValues = 0d;
                List<double> xValues = ActualXValues as List<double>;

                if (IsIndexed || xValues == null)
                {
                    xValues = xValues != null ? (from val in (xValues) select (xIndexValues++)).ToList()
                          : (from val in (ActualXValues as List<string>) select (xIndexValues++)).ToList();
                }

                if (xValues != null)
                {
                    for (i = 0; i < this.PointsCount; i++)
                    {
                        index = i + 1;

                        if (index < PointsCount)
                        {
                            if (i < Segments.Count)
                            {
                                (Segments[i]).SetData(xValues[i], YValues[i], xValues[index], YValues[index]);
                            }
                            else
                            {
                                CreateSegment(new[] { xValues[i], YValues[i], xValues[index], YValues[index] });
                            }
                        }

                        if (AdornmentsInfo != null && ShowDataLabels)
                        {
                            if (i < Adornments.Count)
                            {
                                Adornments[i].SetData(xValues[i], YValues[i], xValues[i], YValues[i]);
                            }
                            else
                            {
                                Adornments.Add(this.CreateAdornment(this, xValues[i], YValues[i], xValues[i], YValues[i]));
                            }
                        }
                    }

                    if (IsClosed)
                    {
                        CreateSegment(new[] { xValues[0], YValues[0], xValues[i - 1], YValues[i - 1] });
                    }
                }
            }
        }

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            if (DrawType == ChartSeriesDrawType.Area)
            {
                return new AreaSegment();
            }
            else
            {
                return new LineSegment();
            }
        }

        /// <summary>
        /// Add the <see cref="LineSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="values">The values.</param>
        private void CreateSegment(double[] values)
        {
            LineSegment segment = CreateSegment() as LineSegment;
            if (segment != null)
            {
                segment.Series = this;
                segment.Item = this;
                segment.SetData(values);
                Segment = segment;
                Segments.Add(segment);
            }
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
       
        #region Private Static Methods

        private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PolarSeries).OnBindingPathChanged();
        }

        private static void OnDrawValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PolarSeries).ScheduleUpdateChart();
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// The <see cref="PolarLineSeries"/> is a series that displays data in terms of values and angles by using a collection of straight lines. It allows for the visual comparison of several quantitative or qualitative aspects of a situation.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of polar line series class, and add it to the <see cref="SfPolarChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="PolarSeries.StrokeWidth"/>, <see cref="PolarRadarSeriesBase.StrokeDashArray"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para><b>Data Label</b></para>
    /// <para> To customize the appearance of data labels, refer to the <see cref="DataMarkerSeries.ShowDataLabels"/>, and <see cref="PolarRadarSeriesBase.DataLabelSettings"/> properties.</para>
    /// 
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    ///
    ///         <chart:PolarLineSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                ShowDataLabels="True">
    ///             <chart:PolarLineSeries.DataLabelSettings>
    ///                 <chart:PolarDataLabelSettings />
    ///             </chart:PolarLineSeries.DataLabelSettings>
    ///         </chart:PolarLineSeries>
    ///
    ///     </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     PolarLineSeries series = new PolarLineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.ShowDataLabels = true;
    ///     series.DataLabelSettings = new PolarDataLabelSettings();
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// *** 
    ///
    /// <para><b>Animation</b></para>
    /// <para>To animate the series, refer to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    ///
    /// # [Xaml](#tab/tabid-3)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    ///
    ///         <chart:PolarLineSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                EnableAnimation="True"/>
    ///
    ///     </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-4)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     PolarLineSeries series = new PolarLineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.EnableAnimation = true;
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <para><b>LegendIcon</b></para>
    /// <para>To customize the legend icon, refer to the <see cref="ChartSeries.LegendIcon"/>, <see cref="ChartSeries.LegendIconTemplate"/>, and <see cref="ChartSeries.VisibilityOnLegend"/> properties.</para>
    ///
    /// # [Xaml](#tab/tabid-5)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    ///
    ///         <chart:SfPolarChart.Legend>
    ///             <chart:ChartLegend/>
    ///         </chart:SfPolarChart.Legend>
    ///
    ///         <chart:PolarLineSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                LegendIcon="Diamond"/>
    ///
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-6)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     chart.Legend = new ChartLegend();
    /// 
    ///     PolarLineSeries series = new PolarLineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.LegendIcon = ChartLegendIcon.Diamond;
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    /// <seealso cref="LineSegment"/>
    public class PolarLineSeries : PolarSeries
    {
        /// <summary>
        /// Initializes a new instance of the PolarLineSeries.
        /// </summary>
        public PolarLineSeries()
        {
            DrawType = ChartSeriesDrawType.Line;
        }
    }

    /// <summary>
    /// The <see cref="PolarAreaSeries"/> is a series that displays data in terms of values and angles using a filled polygon shape. It allows for the visual comparison of several quantitative or qualitative aspects of a situation.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of polar area series class, and add it to the <see cref="SfPolarChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="PolarSeries.StrokeWidth"/>, <see cref="PolarSeries.Stroke"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para><b>Data Label</b></para>
    /// <para>To customize the appearance of data labels, refer to the <see cref="DataMarkerSeries.ShowDataLabels"/>, and <see cref="PolarRadarSeriesBase.DataLabelSettings"/> properties.</para>
    ///
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    ///
    ///         <chart:PolarAreaSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                ShowDataLabels="True">
    ///             <chart:PolarAreaSeries.DataLabelSettings>
    ///                 <chart:PolarDataLabelSettings />
    ///             </chart:PolarAreaSeries.DataLabelSettings>
    ///         </chart:PolarAreaSeries>
    ///           
    ///     </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///     
    ///     PolarAreaSeries series = new PolarAreaSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.ShowDataLabels = true;
    ///     series.DataLabelSettings = new PolarDataLabelSettings();
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// *** 
    ///
    /// <para><b>Animation</b></para>
    /// <para>To animate the series, refer to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    ///
    /// # [Xaml](#tab/tabid-3)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    ///
    ///         <chart:PolarAreaSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                EnableAnimation="True"/>
    ///
    ///     </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-4)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     PolarAreaSeries series = new PolarAreaSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.EnableAnimation = true;
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <para><b>LegendIcon</b></para>
    /// <para>To customize the legend icon, refer to the <see cref="ChartSeries.LegendIcon"/>, <see cref="ChartSeries.LegendIconTemplate"/>, and <see cref="ChartSeries.VisibilityOnLegend"/> properties.</para>
    ///
    /// # [Xaml](#tab/tabid-5)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///
    ///         <!--omitted for brevity-->
    /// 
    ///         <chart:SfPolarChart.Legend>
    ///             <chart:ChartLegend/>
    ///         </chart:SfPolarChart.Legend>
    ///
    ///         <chart:PolarAreaSeries ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"
    ///                                LegendIcon="Diamond"/>
    ///
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-6)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     chart.Legend = new ChartLegend();
    ///
    ///     PolarAreaSeries series = new PolarAreaSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.LegendIcon = ChartLegendIcon.Diamond;
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    /// <seealso cref="AreaSegment"/>
    public class PolarAreaSeries : PolarSeries
    {
        /// <summary>
        /// Initializes a new instance of the PolarAreaSeries.
        /// </summary>
        public PolarAreaSeries()
        {
            DrawType = ChartSeriesDrawType.Area;
        }
    }
}