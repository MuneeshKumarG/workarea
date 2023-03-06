
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Charts;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="SplineSeries"/> is a set of data points linked together by smooth beizer curves.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="SplineSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="StrokeDashArray"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="SplineSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="SplineSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
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
    ///               <chart:SplineSeries
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
    ///     SplineSeries series = new SplineSeries();
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
    /// <seealso cref="SplineAreaSeries"/>
    /// <seealso cref="SplineAreaSegment"/>
    /// <seealso cref="SplineSegment"/>
    public partial class SplineSeries : XYDataSeries, IMarkerDependent
    {

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create(
                nameof(Type),
                typeof(SplineType),
                typeof(SplineSeries),
                SplineType.Natural,
                BindingMode.Default,
                null,
                OnSplineTypeChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeDashArray"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeDashArrayProperty =
            BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(SplineSeries), null, BindingMode.Default, null, OnStrokeDashArrayPropertyChanged);

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
        ///          <chart:SplineSeries ItemsSource="{Binding Data}"
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
        ///     SplineSeries series = new SplineSeries()
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
        /// Gets or sets a value that indicates the shape of the spline series.
        /// </summary>
        /// <value>It accepts <see cref="SplineType"/> values and its default value is <see cref="SplineType.Natural"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:SplineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Type = "Monotonic"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     SplineSeries series = new SplineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Type = SplineType.Monotonic,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public SplineType Type
        {
            get { return (SplineType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        ///  Gets or sets the value indicating whether to show markers for the series data point.
        /// </summary>
        /// <value>It accepts boolean values and its default value is false.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:SplineSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              ShowMarkers="True"/>
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
        ///     SplineSeries series = new SplineSeries()
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
        ///          <chart:SplineSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              ShowMarkers="True">
        ///               <chart:SplineSeries.MarkerSettings>
        ///                     <chart:ChartMarkerSettings Fill="Red" Height="15" Width="15" />
        ///               </chart:SplineSeries.MarkerSettings>
        ///          </chart:SplineSeries>
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
        ///     SplineSeries series = new SplineSeries()
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
        /// Initializes a new instance of the <see cref="SplineSeries"/> class.
        /// </summary>
        public SplineSeries(): base()
        {
            MarkerSettings = new ChartMarkerSettings();
        }
        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override ChartSegment CreateSegment()
        {
            return new SplineSegment();
        }

        /// <summary>
        /// 
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
        /// Creates the segments of <see cref="SplineSeries"/>.
        /// </summary>
        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            if (xValues == null || xValues.Count == 0)
            {
                return;
            }

            double[]? dx = null;

            if (PointsCount == 1)
            {
                CreateSegment(seriesView, new[] { xValues[0], YValues[0], double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN }, 0);
            }
            else
            {
                double[]? yCoef;
                if (Type == SplineType.Monotonic)
                {
                    yCoef = GetMonotonicSpline(xValues, YValues, out dx);
                }
                else if (Type == SplineType.Cardinal)
                {
                    yCoef = GetCardinalSpline(xValues, YValues);
                }
                else
                {
                    yCoef = NaturalSpline(YValues, Type);
                }

                if (yCoef == null)
                {
                    return;
                }

                for (var i = 0; i < PointsCount; i++)
                {
                    var x = xValues[i];
                    var y = YValues[i];
                    bool isLastData = i == PointsCount - 1;

                    var nextX = !isLastData ? xValues[i + 1] : double.NaN;
                    var nextY = !isLastData ? YValues[i + 1] : double.NaN;

                    List<double>? controlPoints;

                    if (isLastData)
                    {
                        controlPoints = new List<double>() { double.NaN, double.NaN, double.NaN, double.NaN };
                    }
                    else if (dx != null && Type == SplineType.Monotonic && dx.Length > 0)
                    {
                        controlPoints = CalculateControlPoints(x, y, nextX, nextY, yCoef[i], yCoef[i + 1], dx[i]);
                    }
                    else if (Type == SplineType.Cardinal)
                    {
                        controlPoints = CalculateControlPoints(x, y, nextX, nextY, yCoef[i], yCoef[i + 1]);
                    }
                    else
                    {
                        controlPoints = CalculateControlPoints(YValues, yCoef[i], yCoef[i + 1], i);
                    }

                    if (controlPoints != null)
                    {
                        if (i < Segments.Count)
                        {
                            Segments[i].SetData(new[] { x, y, controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3], nextX, nextY });
                        }
                        else
                        {
                            CreateSegment(seriesView, new[] { x, y, controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3], nextX, nextY }, i);
                        }
                    }
                }
            }
        }

        internal override bool IsIndividualSegment()
        {
            return false;
        }

        internal override void SetDashArray(ChartSegment segment)
        {
            segment.StrokeDashArray = StrokeDashArray;
        }

        internal override bool SeriesContainsPoint(PointF point)
        {
            if (Chart != null)
            {
                if (base.SeriesContainsPoint(point))
                {
                    return true;
                }

                var dataPoint = FindNearestChartPoint((float)point.X, (float)point.Y);

                if (dataPoint == null || ActualData == null)
                {
                    return false;
                }

                TooltipDataPointIndex = ActualData.IndexOf(dataPoint);

                if (Segments.Count == 0 || TooltipDataPointIndex < 0 || double.IsNaN(YValues[TooltipDataPointIndex]))
                {
                    return false;
                }

                SplineSegment? startSegment = null;
                SplineSegment? endSegment = null;
                var seriesClipRect = AreaBounds;
                point.X = point.X - ((float)seriesClipRect.Left);
                point.Y = point.Y - ((float)seriesClipRect.Top);

                if (TooltipDataPointIndex == 0)
                {
                    startSegment = Segments[TooltipDataPointIndex] as SplineSegment;
                }
                else if (TooltipDataPointIndex == PointsCount - 1)
                {
                    startSegment = Segments[TooltipDataPointIndex - 1] as SplineSegment;
                }
                else
                {
                    startSegment = Segments[TooltipDataPointIndex - 1] as SplineSegment;
                    endSegment = Segments[TooltipDataPointIndex] as SplineSegment;
                }

                if (startSegment != null && ChartUtils.SegmentContains(startSegment, point, this))
                {
                    return true;
                }

                if (endSegment != null)
                {
                    return ChartUtils.SegmentContains(endSegment, point, this);
                }
            }

            return false;
        }

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float tooltipX, float tooltipY)
        {
            var tooltipInfo = base.GetTooltipInfo(tooltipBehavior, tooltipX, tooltipY);

            if (ShowMarkers && tooltipInfo != null && Segments.Count > tooltipInfo.Index)
            {
                var index = tooltipInfo.Index;
                if(Segments[index] is IMarkerDependentSegment segment && this is IMarkerDependent series)
                {
                    tooltipInfo.TargetRect = segment.GetMarkerRect(series.MarkerSettings.Width, series.MarkerSettings.Height, index);
                }
            }

            return tooltipInfo;
        }

        /// <summary>
        /// Add the <see cref="SplineSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="index">The index value.</param>
		/// <param name="seriesView">The seriesview.</param>
        private void CreateSegment(SeriesView seriesView, double[] values, int index)
        {
            var segment = CreateSegment() as SplineSegment;
            if (segment != null)
            {
                segment.Series = this;
                segment.SeriesView = seriesView;
                segment.Index = index;
                segment.SetData(values);
                Segments.Add(segment);

                if (OldSegments != null && OldSegments.Count > 0 && OldSegments.Count > index)
                {
                    SplineSegment? oldSegment = OldSegments[index] as SplineSegment;

                    if (oldSegment != null)
                        segment.SetPreviousData(new[] { oldSegment.X1, oldSegment.Y1, oldSegment.X2, oldSegment.Y2, oldSegment.StartControlX, oldSegment.StartControlY, oldSegment.EndControlX, oldSegment.EndControlY });
                }
            }
        }

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            return DataLabelSettings.GetLabelPositionForContinuousSeries(this, dataLabel.Index, labelSize, labelPosition, padding);
        }

        #endregion

        #region Private Methods

        private static void OnSplineTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as SplineSeries;
            if (series != null)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
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

