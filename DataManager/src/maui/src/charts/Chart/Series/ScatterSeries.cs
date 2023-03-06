using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="ScatterSeries"/> displays a collection of data points represented by a circle of equal size.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="ScatterSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>, <see cref="Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
    /// 
    /// <para> <b>Size - </b> Specify the circle size using the <see cref="PointHeight"/>, and <see cref="PointWidth"/> properties.</para>
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="ScatterSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="ScatterSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
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
    ///               <chart:ScatterSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"
    ///                   PointHeight="10"
    ///                   PointWidth="10"/>
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
    ///     ScatterSeries series = new ScatterSeries();
    ///     series.PointHeight = 10;
    ///     series.PointWidth = 10;
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
    /// <seealso cref="ScatterSegment"/>
    public partial class ScatterSeries : XYDataSeries
    {
        #region Bindable Properties
        /// <summary>
        /// Identifies the <see cref="PointHeight"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty PointHeightProperty =
                  BindableProperty.Create(
                  nameof(PointHeight),
                  typeof(double),
                  typeof(ScatterSeries),
                  5d,
                  BindingMode.Default,
                  null,
                  OnScatterHeightChanged);

        /// <summary>
        /// Identifies the <see cref="PointWidth"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty PointWidthProperty =
                BindableProperty.Create(
                nameof(PointWidth),
                typeof(double),
                typeof(ScatterSeries),
                5d,
                BindingMode.Default,
                null,
                OnScatterWidthChanged);

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
        /// Identifies the <see cref="Type"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty TypeProperty =
                BindableProperty.Create(
                nameof(Type),
                typeof(ShapeType),
                typeof(ScatterSeries),
                ShapeType.Circle,
                BindingMode.Default,
                null,
                OnScatterTypeChanged);
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets a value that defines the height of the scatter segment size.
        /// </summary>
        /// <value>It accepts double values and its default value is 5.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ScatterSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            PointHeight = "20"/>
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
        ///     ScatterSeries series = new ScatterSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           PointHeight = 20,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double PointHeight
        {
            get { return (double)GetValue(PointHeightProperty); }
            set { SetValue(PointHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that defines the width of the scatter segment size.
        /// </summary>
        /// <value>It accepts double values and its default value is 5.</value>
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
        ///                            PointWidth = "20"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
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
        ///           PointWidth = 20,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double PointWidth
        {
            get { return (double)GetValue(PointWidthProperty); }
            set { SetValue(PointWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the stroke appearance of the scatter series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ScatterSeries ItemsSource="{Binding Data}"
        ///                               XBindingPath="XValue"
        ///                               YBindingPath="YValue"
        ///                               Stroke = "Red" />
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
        ///     ScatterSeries series = new ScatterSeries()
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
        /// Gets or sets a value that indicates the shape of the scatter series.
        /// </summary>
        /// <value>It accepts <see cref="ShapeType"/> values and its default value is <see cref="ShapeType.Circle"/>.</value>
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
        ///                            ShapeType = "Diamond"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
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
        ///           Type = ShapeType.Diamond,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ShapeType Type
        {
            get { return (ShapeType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ScatterSeries"/> class.
        /// </summary>
        public ScatterSeries() : base()
        {

        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        protected override ChartSegment CreateSegment()
        {
            return new ScatterSegment();
        }
        #endregion

        #region Internal Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            if (xValues == null)
            {
                return;
            }

            for (int i = 0; i < PointsCount; i++)
            {
                if (i < Segments.Count)
                {
                    Segments[i].SetData(new double[] { xValues[i], YValues[i] });
                }
                else
                {
                    CreateSegment(seriesView, xValues[i], YValues[i], i);
                }
            }
        }

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            return DataLabelSettings.GetLabelPositionForSeries(this, labelSize, labelPosition, padding, new Size(PointHeight, PointWidth));
        }

        internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            if (Chart == null) return;

            ScatterSegment? scatterSegment = Segments[tooltipInfo.Index] as ScatterSegment;

            if (scatterSegment != null)
            {
                RectF targetRect = scatterSegment.SegmentBounds;

                float xPosition = targetRect.X;
                float yPosition = targetRect.Y;
                float height = targetRect.Height;
                float width = targetRect.Width;

                if ((xPosition + width / 2 + seriesBounds.Left) == seriesBounds.Left)
                {
                    targetRect = new Rect(xPosition + width / 2, yPosition, width / 2, height);
                    tooltipInfo.Position = Core.TooltipPosition.Right;
                }
                else if ((xPosition + width/2) == seriesBounds.Width)
                {
                    targetRect = new Rect(xPosition, yPosition, width, height);
                    tooltipInfo.Position = Core.TooltipPosition.Left;
                }

                tooltipInfo.TargetRect = targetRect;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add the <see cref="ScatterSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="index">The index value.</param>
        /// <param name="seriesView">The seriesview.</param>
        private void CreateSegment(SeriesView seriesView, double x, double y, int index)
        {
            var segment = CreateSegment() as ScatterSegment;
            if (segment != null)
            {
                segment.Type = Type;
                segment.Series = this;
                segment.SeriesView = seriesView;
                segment.Index = index;
                segment.SetData(new double[] { x, y });
                Segments.Add(segment);

                if (OldSegments != null && OldSegments.Count > 0 && OldSegments.Count > index)
                {
                    ScatterSegment? oldSegment = OldSegments[index] as ScatterSegment;

                    if (oldSegment != null)
                        segment.PreviousSegmentBounds = oldSegment.SegmentBounds;
                }
            }
        }
        
        private void UpdateScatterType(ShapeType type)
        {
            foreach (ScatterSegment item in Segments)
            {
                item.Type = type;
            }
        }

        #endregion

        #endregion

        #region Callbacks
        private static void OnScatterHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as ScatterSeries;
            if (series != null)
            {
               series.ScheduleUpdateChart();
            }
        }

        private static void OnScatterWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as ScatterSeries;
            if (series != null)
            {
                series.ScheduleUpdateChart();
            }
        }

        private static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as ScatterSeries;
            if (series != null)
            {
                series.UpdateStrokeColor();
                series.InvalidateSeries(); 
            }
        }

        private static void OnScatterTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as ScatterSeries;
            if (series != null)
            {
                series.UpdateScatterType((ShapeType)newValue);
                series.InvalidateSeries(); 
            }
        }

        #endregion
    }
}