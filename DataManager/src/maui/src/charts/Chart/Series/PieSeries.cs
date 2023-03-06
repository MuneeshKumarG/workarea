using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="PieSeries"/> displays data as a proportion of the whole. Its most commonly used to make comparisons among a set of given data.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of the pie series class, and add it to the <see cref="SfCircularChart.Series"/> collection.</para>
    /// 
    /// <para>It Provides options for <see cref="ChartSeries.PaletteBrushes"/>, <see cref="ChartSeries.Fill"/>, <see cref="CircularSeries.Stroke"/>, <see cref="CircularSeries.StrokeWidth"/>, and <see cref="CircularSeries.Radius"/> to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> The tooltip displays information while tapping or mouse hovering on the segment. To display the tooltip on the chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="PieSeries"/> and refer to the <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in the <see cref="PieSeries"/> class. To customize the chart data labels’ alignment, placement, and label styles, you need to create an instance of <see cref="CircularDataLabelSettings"/> and set it to the <see cref="CircularSeries.DataLabelSettings"/> property. </para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>Selection - </b> To enable the data point selection in the series, create an instance of the <see cref="DataPointSelectionBehavior"/> and set it to the <see cref="ChartSeries.SelectionBehavior"/> property of the pie series. To highlight the selected segment, set the value for the <see cref="ChartSelectionBehavior.SelectionBrush"/> property in the <see cref="DataPointSelectionBehavior"/> class.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// 
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCircularChart>
    ///
    ///           <chart:SfCircularChart.Series>
    ///               <chart:PieSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"/>
    ///           </chart:SfCircularChart.Series>  
    ///           
    ///     </chart:SfCircularChart>
    /// ]]></code>
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
    /// <seealso cref="DoughnutSegment"/>
    /// <seealso cref="DoughnutSeries"/>
    /// <seealso cref="PieSegment"/>
    public class PieSeries : CircularSeries
    {
        #region Fields
        double total = 0;
        float yValue;
        float pieEndAngle;
        float pieStartAngle;
        double angleDifference;
        #endregion

        #region Bindable Properties

        /// <summary>
        /// Gets or sets the index value of the segment to be exploded. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty ExplodeIndexProperty =
                BindableProperty.Create(
                nameof(ExplodeIndex),
                typeof(int),
                typeof(PieSeries),
                -1,
                BindingMode.TwoWay,
                null,
                OnExplodeIndexChanged);

        /// <summary>
        /// Gets or sets the value that defines the exploding distance of the segment. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty ExplodeRadiusProperty =
            BindableProperty.Create(
                nameof(ExplodeRadius),
                typeof(double),
                typeof(PieSeries),
                10d,
                BindingMode.Default,
                null,
                OnExplodeRadiusChanged);

        /// <summary>
        /// Gets or sets the value that indicates whether to explode the segment on touch. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty ExplodeOnTouchProperty =
                BindableProperty.Create(
                    nameof(ExplodeOnTouch),
                    typeof(bool),
                    typeof(CircularSeries),
                    false,
                    BindingMode.Default,
                    null,
                    null);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PieSeries"/> class.
        /// </summary>
        public PieSeries() : base()
        {
            PaletteBrushes = ChartColorModel.DefaultBrushes;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the index value of the segment to be exploded.
        /// </summary>
        /// <value>This property takes an <see cref="int"/> value, and its default value is <c>-1</c>.</value>
        /// <remarks>
        /// <para>To explode a segment, create an instance for any circular series, and assign value to the <see cref="ExplodeIndex"/> property. </para>
        ///</remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///           <chart:SfCircularChart.Series>
        ///               <chart:PieSeries ItemsSource="{Binding Data}" XBindingPath="XValue"
        ///                   YBindingPath="YValue" ExplodeIndex = "3"/>
        ///           </chart:SfCircularChart.Series>  
        ///           
        ///     </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public int ExplodeIndex
        {
            get { return (int)GetValue(ExplodeIndexProperty); }
            set { SetValue(ExplodeIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that defines the exploding distance of the segments.
        /// </summary>
        /// <value>This property takes a <see cref="double"/> value, and its default value is <c>10d</c>.</value>
        /// <remarks>
        /// <para>To explode a segment to a specific length, create an instance for any circular series, and assign value to both <see cref="ExplodeIndex"/> and <see cref="ExplodeRadius"/> properties.</para>
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///           <chart:SfCircularChart.Series>
        ///               <chart:PieSeries ItemsSource="{Binding Data}" XBindingPath="XValue"
        ///                   YBindingPath="YValue" ExplodeIndex = "3" ExplodeRadius="20"/>
        ///           </chart:SfCircularChart.Series>  
        ///           
        ///     </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double ExplodeRadius
        {
            get { return (double)GetValue(ExplodeRadiusProperty); }
            set { SetValue(ExplodeRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to explode the segment by touch or tap interaction.
        /// </summary>
        /// <value>This property takes the <see cref="bool"/> value, and its default value is <c>false</c>.</value>
        /// <remarks>
        /// <para>To explode a selected segment by tap action, create an instance for any circular series, and assign the <see cref="ExplodeOnTouch"/> property value as <c>"True"</c> </para>
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        ///  <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///           <chart:SfCircularChart.Series>
        ///               <chart:PieSeries ItemsSource="{Binding Data}" XBindingPath="XValue"
        ///                   YBindingPath="YValue" ExplodeOnTouch="True"/>
        ///           </chart:SfCircularChart.Series>  
        ///           
        ///     </chart:SfCircularChart>
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ExplodeOnTouch
        {
            get { return (bool)GetValue(ExplodeOnTouchProperty); }
            set { SetValue(ExplodeOnTouchProperty, value); }
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override ChartSegment CreateSegment()
        {
            return new PieSegment();
        }

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
             GetActualXValues();
            
            if (YValues != null && YValues.Count > 0)
            {
                pieStartAngle = (float)StartAngle;
                angleDifference = GetAngleDifference();
                total = CalculateTotalYValues();
                var oldSegments = OldSegments != null && OldSegments.Count > 0 && PointsCount == OldSegments.Count ? OldSegments : null;

                for (int i = 0; i < YValues.Count; i++)
                {
                    yValue = (float)YValues[i];
                    pieEndAngle = (float)(Math.Abs(float.IsNaN(yValue) ? 0 : yValue) * (angleDifference / total));

                    if (i < Segments.Count && Segments[i] is PieSegment)
                    {
                        ((PieSegment)Segments[i]).SetData(pieStartAngle, pieEndAngle, yValue);
                    }
                    else
                    {
                        PieSegment pieSegment = (PieSegment)CreateSegment();
                        pieSegment.Series = this;
                        pieSegment.SeriesView = seriesView;
                        pieSegment.Index = i;
                        pieSegment.Exploded = ExplodeIndex == i;
                        pieSegment.SetData(pieStartAngle, pieEndAngle, yValue);
                        Segments.Add(pieSegment);

                        if (oldSegments != null)
                        {
                            PieSegment? oldSegment = oldSegments[i] as PieSegment;

                            if (oldSegment != null)
                                pieSegment.SetPreviousData(new[] { oldSegment.StartAngle, oldSegment.EndAngle });
                        }
                    }

                    if (Segments[i].IsVisible)
                    {
                        pieStartAngle += pieEndAngle;
                    }
                }
            }
        }

        internal override float GetDataLabelRadius()
        {
            float radius = DataLabelSettings.LabelPlacement == DataLabelPlacement.Inner || DataLabelSettings.LabelPlacement == DataLabelPlacement.Auto || DataLabelSettings.LabelPlacement == DataLabelPlacement.Center ? GetRadius() / 2 : GetRadius();
            return radius;
        }

        internal virtual float GetTooltipRadius()
        {
            return GetRadius() / 2;
        }

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            if (AreaBounds == Rect.Zero) return null;

            int index = GetDataPointIndex(x, y);

            if (index < 0 || ActualData == null || YValues == null)
            {
                return null;
            }

            object dataPoint = ActualData[index];
            double yValue = YValues[index];
            var pieSegment = Segments[index] as PieSegment;

            if (pieSegment == null) return null;

            float segmentRadius = GetTooltipRadius();
            segmentRadius = pieSegment.Index == ExplodeIndex ? segmentRadius + (float)ExplodeRadius : segmentRadius;
            PointF center = Center;
            double midAngle = (pieSegment.StartAngle + (pieSegment.EndAngle / 2)) * 0.0174532925f;
            float xPosition = (float)(center.X + (Math.Cos(midAngle) * segmentRadius));
            float yPosition = (float)(center.Y + (Math.Sin(midAngle) * segmentRadius));

            TooltipInfo tooltipInfo = new TooltipInfo(this);
            tooltipInfo.X = xPosition;
            tooltipInfo.Y = yPosition;
            tooltipInfo.Index = index;
            tooltipInfo.Margin = tooltipBehavior.Margin;
            tooltipInfo.TextColor = tooltipBehavior.TextColor;
            tooltipInfo.FontFamily = tooltipBehavior.FontFamily;
            tooltipInfo.FontSize = tooltipBehavior.FontSize;
            tooltipInfo.FontAttributes = tooltipBehavior.FontAttributes;
            tooltipInfo.Background = tooltipBehavior.Background;
            tooltipInfo.Text = yValue.ToString("#.##");
            tooltipInfo.Item = dataPoint;
            return tooltipInfo;
        }

        internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            float xPosition = tooltipInfo.X;
            float yPosition = tooltipInfo.Y;
            float sizeValue = 1;
            float noseOffset = 2;
            float halfSizeValue = 0.5f;

            Rect targetRect = new Rect(xPosition - halfSizeValue, yPosition + noseOffset, sizeValue, sizeValue);
            tooltipInfo.TargetRect = targetRect;
        }

        internal void UpdateExplodeOnTouch(float pointX, float pointY)
        {
            var dataPointIndex = this.GetDataPointIndex(pointX, pointY);
            if (dataPointIndex >= 0)
            {
                ExplodeIndex = (ExplodeIndex != dataPointIndex) ? dataPointIndex : -1;
            }
        }

        #endregion

        #region Private Methods

        private static void OnExplodeIndexChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as PieSeries;

            if (series != null && series.AreaBounds != Rect.Zero)
            {
                series.OnExplodePropertiesChanged();
            }
        }

        private static void OnExplodeRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as PieSeries;

            if (series != null && series.AreaBounds != Rect.Zero)
            {
                series.OnExplodePropertiesChanged();
            }
        }

        private void UpdateExplode()
        {
            for (int i = 0; i < Segments.Count; i++)
            {
                var segment = (PieSegment)Segments[i];
                segment.Exploded = ExplodeIndex == i;
            }
        }

        private void OnExplodePropertiesChanged()
        {
            UpdateExplode();
            Invalidate();
            InvalidateSeries();

            if (ShowDataLabels)
                InvalidateDataLabel();
        }

        #endregion

        #endregion
    }
}
