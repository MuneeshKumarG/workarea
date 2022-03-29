using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
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
    ///               <chart:FastLineSeries
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
    ///     FastLineSeries series = new FastLineSeries();
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
    /// <seealso cref="FastLineSegment"/>
    public partial class FastLineSeries : XYDataSeries
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty EnableAntiAliasingProperty =
            BindableProperty.Create(nameof(EnableAntiAliasing), typeof(bool), typeof(FastLineSeries), false, BindingMode.Default, null, OnInvalidatePropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty StrokeDashArrayProperty =
            BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(FastLineSeries), null, BindingMode.Default, null, OnStrokeDashArrayPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public bool EnableAntiAliasing
        {
            get { return (bool)GetValue(EnableAntiAliasingProperty); }
            set { SetValue(EnableAntiAliasingProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        #endregion

        #region Fields

        /// <summary>
        /// Holds pixels value, that used to maintain data count for pixel. 
        /// </summary>
        internal double ToleranceCoefficient { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public FastLineSeries() : base()
        {
            ToleranceCoefficient = 1;
        }

        #endregion

        #region Methods

        #region Public methods

        /// <inheritdoc />
        public override int GetDataPointIndex(float pointX, float pointY)
        {
            if (ActualXAxis != null && ActualYAxis != null && Segments != null && Segments.Count > 0)
            {
                RectF seriesBounds = AreaBounds;
                float xPos = pointX - seriesBounds.Left;
                float yPos = pointY - seriesBounds.Top;

                foreach (FastLineSegment segment in Segments)
                {
                    var xValues = segment.XValues;
                    var yValues = segment.YValues;

                    if (xValues == null || yValues == null) return -1;

                    for (int i = 0; i < xValues.Count; i++)
                    {
                        var xval = xValues[i];
                        var yval = yValues[i];
                        float xPoint = TransformToVisibleX(xval, yval);
                        float yPoint = TransformToVisibleY(xval, yval);
                        if (ChartSegment.IsRectContains(xPoint, yPoint, xPos, yPos, (float)StrokeWidth))
                        {
                            return i;
                        }
                    }
                }
            }

            return -1;
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc />
        protected override ChartSegment CreateSegment()
        {
            return new FastLineSegment();
        }

        #endregion

        #region Internal Methods

        /// <inheritdoc />
        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            if (xValues == null || xValues.Count == 0)
            {
                return;
            }

            if (Segments.Count == 0)
            {
                var segment = CreateSegment() as FastLineSegment;
                if (segment != null)
                {
                    segment.Series = this;
                    segment.SeriesView = seriesView;
                    Segments.Add(segment);
                    segment.SetData(xValues, YValues);
                }
            }
            else
            {
                foreach (FastLineSegment segment in Segments)
                {
                    segment.SetData(xValues, YValues);
                }
            }
        }

        /// <inheritdoc />
        internal override bool IsIndividualSegment()
        {
            return false;
        }

        /// <inheritdoc />
        internal override void SetDashArray(ChartSegment segment)
        {
            segment.StrokeDashArray = StrokeDashArray;
        }

        /// <inheritdoc />
        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            return DataLabelSettings.GetLabelPositionForContinuousSeries(this, dataLabel.Index, labelSize, labelPosition, padding);
        }

        /// <inheritdoc />
        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabeSettings = DataLabelSettings;

            if (dataLabeSettings == null || Segments == null || Segments.Count <= 0) return;

            ChartDataLabelStyle labelStyle = DataLabelSettings.LabelStyle;

            foreach (FastLineSegment segment in Segments)
            {
                var xValues = segment.XValues;
                var yValues = segment.YValues;

                if (xValues == null || yValues == null) return;

                for (int i = 0; i < xValues.Count; i++)
                {
                    double x = xValues[i], y = yValues[i];
                    var isDataInVisibleRange = IsDataInVisibleRange(x, y);

                    if (double.IsNaN(y) || !isDataInVisibleRange) continue;

                    CalculateDataPointPosition(i, ref x, ref y);
                    PointF labelPoint = new PointF((float)x, (float)y);
                    segment.DataLabel = dataLabeSettings.GetLabelContent(yValues[i]);
                    segment.LabelPositionPoint = dataLabeSettings.CalculateDataLabelPoint(this, segment, labelPoint, labelStyle);
                    UpdateDataLabelAppearance(canvas, segment, dataLabeSettings, labelStyle);
                }
            }
        }

        #endregion

        #region private callback

        /// <summary>
        /// Called when <see cref="StrokeDashArray"/> property changed. 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as FastLineSeries;
            if (series != null)
            {
                series.UpdateDashArray();
                series.InvalidateSeries();
            }
        }

        /// <summary>
        /// Called when <see cref="EnableAntiAliasing"/> property changed. 
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnInvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as FastLineSeries;
            if (series != null)
            {
                series.InvalidateSeries();
            }
        }

        #endregion

        #endregion
    }
}
