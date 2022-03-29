using Microsoft.Maui;
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
    ///               <chart:ColumnSeries
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
    ///     ColumnSeries series = new ColumnSeries();
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
    /// <seealso cref="ColumnSegment"/>
    public class ColumnSeries : XYDataSeries
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary> 
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(
                nameof(Stroke),
                typeof(Brush),
                typeof(ColumnSeries),
                SolidColorBrush.Transparent,
                BindingMode.Default,
                null,
                OnStrokeColorChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty SpacingProperty =
            BindableProperty.Create(
                nameof(Spacing),
                typeof(double),
                typeof(ColumnSeries),
                0d,
                BindingMode.Default,
                null,
                OnSpacingChanged);

        /// <summary>
        /// 
        /// </summary>  
        public static readonly BindableProperty WidthProperty =
            BindableProperty.Create(
                nameof(Width),
                typeof(double),
                typeof(ColumnSeries),
                0.8d,
                BindingMode.Default,
                null,
                OnWidthChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ColumnSeries),
                null,
                BindingMode.Default,
                null,
                OnCornerRadiusChanged);

        #endregion

        #region Public Properties

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
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal override bool IsSideBySide => true;

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override ChartSegment CreateSegment()
        {
            return new ColumnSegment();
        }

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            double bottomValue = 0;
            if (ActualXAxis != null)
            {
                bottomValue = double.IsNaN(ActualXAxis.ActualCrossingValue) ? 0 : ActualXAxis.ActualCrossingValue;
            }

            if (IsIndexed || xValues == null)
            {
                    for (var i = 0; i < PointsCount; i++)
                    {
                        if (i < Segments.Count && Segments[i] is ColumnSegment)
                        {
                            ((ColumnSegment)Segments[i]).SetData(new[] { i + SbsInfo.Start, i + SbsInfo.End, YValues[i], bottomValue, i });
                        }
                        else
                        {
                            CreateSegment(seriesView, new[] { i + SbsInfo.Start, i + SbsInfo.End, YValues[i], bottomValue, i }, i);
                        }
                    }
            }
            else
            {
                for (var i = 0; i < PointsCount; i++)
                {
                    var x = xValues[i];
                    if (i < Segments.Count && Segments[i] is ColumnSegment)
                    {
                        ((ColumnSegment)Segments[i]).SetData(new[] { x + SbsInfo.Start, x + SbsInfo.End, YValues[i], bottomValue, x });
                    }
                    else
                    {
                        CreateSegment(seriesView, new[] { x + SbsInfo.Start, x + SbsInfo.End, YValues[i], bottomValue, x }, i);
                    }
                }
            }
        }

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override double GetActualSpacing()
        {
            return Spacing;
        }

        internal override double GetActualWidth()
        {
            return Width;
        }

        internal override double GetDataLabelPositionAtIndex(int index)
        {
            if (DataLabelSettings == null) return 0;

            var yValue = YValues?[index] ?? 0f;

            switch (DataLabelSettings.BarAlignment)
            {
                case DataLabelAlignment.Bottom:
                    yValue = 0;
                    break;
                case DataLabelAlignment.Middle:
                    yValue = yValue / 2;
                    break;
            }

            return yValue;
        }

        internal override void CalculateDataPointPosition(int index, ref double x, ref double y)
        {
            if (ChartArea == null) return;

            var x1 = SbsInfo.Start + x;
            var x2 = SbsInfo.End + x;

            var xCal = x1 + ((x2 - x1) / 2);
            var yCal = y;

            if (ActualYAxis != null && ActualXAxis != null && !double.IsNaN(yCal))
            {
                y = ChartArea.IsTransposed ? ActualXAxis.ValueToPoint(xCal) : ActualYAxis.ValueToPoint(yCal);
            }

            if (ActualXAxis != null && ActualYAxis != null && !double.IsNaN(x))
            {
                x = ChartArea.IsTransposed ? ActualYAxis.ValueToPoint(yCal) : ActualXAxis.ValueToPoint(xCal);
            }
        }

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            if (ChartArea == null) return labelPosition;

            if (ChartArea.IsTransposed)
            {
                return DataLabelSettings.GetLabelPositionForTransposedRectangularSeries(this, dataLabel.Index, labelSize, labelPosition, padding, DataLabelSettings.BarAlignment);
            }

            return DataLabelSettings.GetLabelPositionForRectangularSeries(this, dataLabel.Index, labelSize, labelPosition, padding, DataLabelSettings.BarAlignment);
        }

        internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            if (ChartArea == null) return;

            float xPosition = tooltipInfo.X;
            float yPosition = tooltipInfo.Y;
            ColumnSegment? columnSegment = Segments[tooltipInfo.Index] as ColumnSegment;

            if (columnSegment != null)
            {
                if (ChartArea.IsTransposed)
                {
                    float width = columnSegment.SegmentBounds.Width;
                    float height = columnSegment.SegmentBounds.Height;
                    Rect targetRect = new Rect(xPosition - width, yPosition - height/2, width, height);
                    tooltipInfo.TargetRect = targetRect;
                }
                else
                {
                    float width = columnSegment.SegmentBounds.Width;
                    float height = 1;
                    Rect targetRect = new Rect(xPosition - width / 2, yPosition, width, height);
                    tooltipInfo.TargetRect = targetRect;
                }
            }
        }

        #endregion

        #region Private Methods

        private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var columnseries = bindable as ColumnSeries;

            if (columnseries != null && columnseries.Chart != null)
            {
                columnseries.InvalidateSeries();
            }
        }

        private static void OnWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var columnseries = bindable as ColumnSeries;

            if (columnseries != null && columnseries.ChartArea != null)
            {
                columnseries.UpdateSbsSeries();
            }
        }

        private static void OnSpacingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var columnseries = bindable as ColumnSeries;

            if (columnseries != null && columnseries.ChartArea != null)
            {
                columnseries.UpdateSbsSeries();
            }
        }

        private static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var columnseries = bindable as ColumnSeries;

            if (columnseries != null && columnseries.Chart != null)
            {
                columnseries.UpdateStrokeColor();
                columnseries.InvalidateSeries();
            }
        }

        private void CreateSegment(SeriesView seriesView, double[] values, int index)
        {
            var segment = CreateSegment() as ColumnSegment;

            if (segment != null)
            {
                segment.Series = this;
                segment.SeriesView = seriesView;
                segment.SetData(values);
                segment.Index = index;
                Segments.Add(segment);

                if (OldSegments != null && OldSegments.Count > 0 && OldSegments.Count > index)
                {
                    var oldSegment = OldSegments[index] as ColumnSegment;

                    if (oldSegment != null)
                        segment.SetPreviousData(new[] { oldSegment.Y1, oldSegment.Y2 });
                }
            }
        }

        #endregion

        #endregion
    }
}
