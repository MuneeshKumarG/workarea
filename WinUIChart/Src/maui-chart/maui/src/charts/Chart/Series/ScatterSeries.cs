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
        /// 
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
        /// 
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
        /// 
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
        /// 
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
        /// 
        /// </summary>
        public double PointHeight
        {
            get { return (double)GetValue(PointHeightProperty); }
            set { SetValue(PointHeightProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double PointWidth
        {
            get { return (double)GetValue(PointWidthProperty); }
            set { SetValue(PointWidthProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ShapeType Type
        {
            get { return (ShapeType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// 
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