using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="AreaSeries"/> is a collection of data points connected to form a closed loop area, filled with the specified color.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="AreaSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="AreaSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="AreaSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
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
    ///           <chart:SfCartesianChart.Series>
    ///               <chart:AreaSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"/>
    ///           </chart:SfCartesianChart.Series>  
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
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
    ///     AreaSeries series = new AreaSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    ///     
    /// ]]></code>
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
    /// ]]></code>
    /// ***
    /// </example>
    /// <seealso cref="AreaSegment"/>
    /// <seealso cref="SplineAreaSeries"/>
    public partial class AreaSeries : XYDataSeries, IMarkerDependent
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeProperty =
                BindableProperty.Create(
                nameof(Stroke),
                typeof(Brush),
                typeof(ScatterSeries),
                null,
                BindingMode.Default,
                null,
                OnStrokeChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeDashArray"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeDashArrayProperty =
            BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(AreaSeries), null, BindingMode.Default, null, OnStrokeDashArrayPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowMarkers"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ShowMarkersProperty = ChartMarker.ShowMarkersProperty;

        /// <summary>
        /// Identifies the <see cref="MarkerSettings"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty MarkerSettingsProperty = ChartMarker.MarkerSettingsProperty;
        
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the stroke dash array to customize the appearance of stroke.
        /// </summary>
        /// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:AreaSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            StrokeDashArray="5,3"
        ///                            Stroke = "Red" />
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
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(5);
        ///     doubleCollection.Add(3);
        ///     AreaSeries series = new AreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           StrokeDashArray = doubleCollection,
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of the area series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:AreaSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Stroke = "Red" />
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
        ///     AreaSeries series = new AreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///     };
        ///     
        ///     chart.Series.Add(series);
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

        /// <summary>
        /// Gets or sets the value indicating whether to show markers for the series data point.
        /// </summary>
        /// <value>It accepts boolean values and its default value is false.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:AreaSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ShowMarkers="True"/>
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
        ///     AreaSeries series = new AreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowMarkers= true,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowMarkers
        {
            get { return (bool)GetValue(ShowMarkersProperty); }
            set { SetValue(ShowMarkersProperty, value); }
        }

        /// <summary>
        /// Gets or sets the option for customize the series markers.
        /// </summary>
        /// <value>It accepts <see cref="ChartMarkerSettings"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:AreaSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ShowMarkers="True">
        ///               <chart:AreaSeries.MarkerSettings>
        ///                     <chart:ChartMarkerSettings Fill="Red" Height="15" Width="15" />
        ///               </chart:AreaSeries.MarkerSettings>
        ///          </chart:AreaSeries>
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
        ///    ChartMarkerSettings chartMarkerSettings = new ChartMarkerSettings()
        ///    {
        ///        Fill = new SolidColorBrush(Colors.Red),
        ///        Height = 15,
        ///        Width = 15,
        ///    };
        ///     AreaSeries series = new AreaSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowMarkers= true,
        ///           MarkerSettings= chartMarkerSettings,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartMarkerSettings MarkerSettings
        {
            get { return (ChartMarkerSettings)GetValue(MarkerSettingsProperty); }
            set { SetValue(MarkerSettingsProperty, value); }
        }

        #endregion

        #region Private Properties

        bool needToAnimateMarker;

        bool IMarkerDependent.NeedToAnimateMarker { get => needToAnimateMarker; set => needToAnimateMarker = this.EnableAnimation; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaSeries"/> class.
        /// </summary>
        public AreaSeries(): base()
        {
            MarkerSettings = new ChartMarkerSettings();
        }

        #endregion

        #region Methods

        #region Public Override Methods

        /// <inheritdoc/>
        public override int GetDataPointIndex(float pointX, float pointY)
        {
            if (Chart != null)
            {
                var dataPoint = FindNearestChartPoint(pointX, pointY);
                if (dataPoint == null || ActualData == null)
                {
                    return -1;
                }

                var tooltipIndex = ActualData.IndexOf(dataPoint);
                if (tooltipIndex < 0)
                {
                    return -1;
                }

                List<PointF> segPoints = new List<PointF>();
                var xValues = GetXValues();
                foreach (AreaSegment segment in Segments)
                {
                    var points = segment.FillPoints;
                    if (points != null)
                    {
                        var index = ((tooltipIndex + 1) * 2);

                        float xValue = points[index];
                        float yValue = points[index + 1];


                        if (((pointX - (float)AreaBounds.Left) > xValue) || tooltipIndex == 0)
                        {
                            segPoints.Add(new PointF(xValue, points[1]));
                            segPoints.Add(new PointF(xValue, yValue));

                            xValue = points[index + 2];
                            yValue = points[index + 3];
                            if (!double.IsNaN(yValue))
                            {
                                segPoints.Add(new PointF(xValue, yValue));
                                segPoints.Add(new PointF(xValue, points[1]));
                            }
                        }
                        else if (((pointX - (float)AreaBounds.Left) < xValue) && tooltipIndex != 0)
                        {
                            xValue = points[index - 2];
                            yValue = points[index - 1];

                            if (!double.IsNaN(yValue))
                            {
                                segPoints.Add(new PointF(xValue, points[1]));
                                segPoints.Add(new PointF(xValue, yValue));
                            }

                            xValue = points[index];
                            yValue = points[index + 1];

                            segPoints.Add(new PointF(xValue, yValue));
                            segPoints.Add(new PointF(xValue, points[1]));
                        }
                    }
                }

                var x = pointX - (float)AreaBounds.Left;
                var y = pointY - (float)AreaBounds.Top;
                if (ChartUtils.IsPathContains(segPoints, x, y))
                {
                    return tooltipIndex;
                }
            }

            return -1;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///
        /// </summary>
        protected override ChartSegment CreateSegment()
        {
            return new AreaSegment();
        }

        /// <summary>
        /// Calls when drawing markers.
        /// </summary>
        protected virtual void DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect)
        {
            if (this is IMarkerDependent markerDependent)
            {
                canvas.DrawShape(rect, shapeType: type, markerDependent.MarkerSettings.HasBorder, false);
            }
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates the segments of <see cref="AreaSeries"/>.
        /// </summary>
        internal override void GenerateSegments(SeriesView seriesView)
        {
            var actualXValues = GetXValues();
            if (actualXValues == null)
            {
                return;
            }

            List<double>? xValues = null, yValues = null;
            for (int i = 0; i < PointsCount; i++)
            {
                if (!double.IsNaN(YValues[i]))
                {
                    if (xValues == null)
                    {
                        xValues = new List<double>();
                        yValues = new List<double>();
                    }

                    xValues.Add(actualXValues[i]);
                    yValues?.Add(YValues[i]);
                }

                if (double.IsNaN(YValues[i]) || i == PointsCount - 1)
                {
                    if (xValues != null)
                    {
                        var segment = CreateSegment() as AreaSegment;
                        if (segment != null)
                        {
                            segment.Series = this;
                            segment.SeriesView = seriesView;
                            if (yValues != null)
                                segment.SetData(xValues, yValues);
                            Segments.Add(segment);

                            if (OldSegments != null && OldSegments.Count > 0)
                            {
                                AreaSegment? oldSegment = OldSegments[0] as AreaSegment;

                                if (oldSegment != null)
                                {
                                    segment.PreviousFillPoints = oldSegment.FillPoints;
                                    segment.PreviousStrokePoints = oldSegment.StrokePoints;
                                }
                            }
                        }

                        yValues = xValues = null;
                    }

                    if (double.IsNaN(YValues[i]))
                    {
                        xValues = new List<double> { actualXValues[i] };
                        yValues = new List<double> { YValues[i] };
                        var segment = (AreaSegment)CreateSegment();
                        segment.Series = this;
                        segment.SetData(xValues, yValues);
                        yValues = xValues = null;
                    }
                }
            }
        }

        internal override void SetDashArray(ChartSegment segment)
        {
            segment.StrokeDashArray = StrokeDashArray;
        }

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override void UpdateRange()
        {
            double yStart = YRange.Start;
            double yEnd = YRange.End;

            if (yStart > 0)
            {
                yStart = 0;
            }

            if (yEnd < 0)
            {
                yEnd = 0;
            }

            YRange = new DoubleRange(yStart, yEnd);
            base.UpdateRange();
        }

        internal override bool IsIndividualSegment()
        {
            return false;
        }

        internal override bool SeriesContainsPoint(PointF point)
        {
            //Todo: Need to implement the get tooltip index from FindNearestPoint method.

            return base.SeriesContainsPoint(point);
        }

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            return DataLabelSettings.GetLabelPositionForAreaSeries(this, dataLabel, labelSize, labelPosition, padding);
        }

        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabeSettings = DataLabelSettings;

            if (dataLabeSettings == null || Segments == null || Segments.Count <= 0) return;

            ChartDataLabelStyle labelStyle = DataLabelSettings.LabelStyle;
            AreaSegment? datalabel = Segments[0] as AreaSegment;

            if (datalabel == null || datalabel.XValues == null || datalabel.YValues == null) return;

            for (int i = 0; i < PointsCount; i++)
            {
                double x = datalabel.XValues[i], y = datalabel.YValues[i];

                if (double.IsNaN(y)) continue;

                CalculateDataPointPosition(i, ref x, ref y);
                PointF labelPoint = new PointF((float)x, (float)y);
               
                datalabel.DataLabel = dataLabeSettings.GetLabelContent(datalabel.YValues[i]);
                datalabel.LabelPositionPoint = dataLabeSettings.CalculateDataLabelPoint(this, datalabel, labelPoint, labelStyle);
                UpdateDataLabelAppearance(canvas, datalabel, dataLabeSettings, labelStyle);
            }
        }

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float tooltipX, float tooltipY)
        {
            var tooltipInfo = base.GetTooltipInfo(tooltipBehavior, tooltipX, tooltipY);

            if (ShowMarkers && tooltipInfo != null && Segments.Count > 0)
            {
                if (Segments[0] is IMarkerDependentSegment segment && this is IMarkerDependent series)
                {
                    tooltipInfo.TargetRect = segment.GetMarkerRect(series.MarkerSettings.Width, series.MarkerSettings.Height, tooltipInfo.Index);
                }
            }

            return tooltipInfo;
        }
        #endregion

        #region Private Methods

        private static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as ScatterSeries;
            if (series != null)
            {
                series.UpdateStrokeColor();
                series.InvalidateSeries(); 
            }
        }

        private static void OnStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as LineSeries;
            if (series != null)
            {
                series.UpdateDashArray();
                series.InvalidateSeries();
            }
        }

        void IMarkerDependent.InvalidateDrawable()
        {
            InvalidateSeries();
        }

        ChartMarkerSettings IMarkerDependent.MarkerSettings => MarkerSettings ?? new ChartMarkerSettings();

        void IMarkerDependent.DrawMarker(ICanvas canvas, int index, ShapeType type, Rect rect) => this.DrawMarker(canvas, index, type, rect);

        #endregion

        #endregion
    }
}
