using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CartesianSeries : ChartSeries
    {
        
        private ChartAxis? actualXAxis;
        private ChartAxis? actualYAxis;

        #region Bindable properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty XAxisNameProperty =
    BindableProperty.Create(
        nameof(XAxisName),
        typeof(string),
        typeof(CartesianSeries),
        null,
        BindingMode.Default,
        null,
        OnAxisNameChanged); 
        
        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty YAxisNameProperty =
    BindableProperty.Create(
        nameof(YAxisName),
        typeof(string),
        typeof(CartesianSeries),
        null,
        BindingMode.Default,
        null,
        OnAxisNameChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty DataLabelSettingsProperty =
            BindableProperty.Create(
                nameof(DataLabelSettings),
                typeof(CartesianDataLabelSettings),
                typeof(CartesianSeries),
                null,
                BindingMode.Default,
                null,
                OnDataLabelSettingsChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty LabelProperty =
            BindableProperty.Create(
                nameof(Label),
                typeof(string),
                typeof(CartesianSeries),
                string.Empty,
                BindingMode.Default,
                null,
                OnLabelPropertyChanged);
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public CartesianSeries()
        {
            DataLabelSettings = new CartesianDataLabelSettings();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public string XAxisName
        {
            get { return (string)GetValue(XAxisNameProperty); }
            set { SetValue(XAxisNameProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string YAxisName
        {
            get { return (string)GetValue(YAxisNameProperty); }
            set { SetValue(YAxisNameProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
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
        ///               <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue" ShowDataLabels="True">
        ///                    <chart:ColumnSeries.DataLabelSettings>
        ///                         <chart:CartesianDataLabelSettings BarAlignment="Middle" />
        ///                    </ chart:ColumnSeries.DataLabelSettings>
        ///               </chart:ColumnSeries> 
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     
        ///     chart.XAxes.Add(xAxis);
        ///     chart.YAxes.Add(yAxis);
        ///     
        ///     ColumnSeries series = new ColumnSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.ShowDataLabels = "True";
        ///     
        ///     series.DataLabelSettings = new CartesianDataLabelSettings()
        ///     {
        ///         BarAlignment = Alignment.Middle,
        ///     };
        ///     chart.Series.Add(series);
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public CartesianDataLabelSettings DataLabelSettings
        {
            get { return (CartesianDataLabelSettings)GetValue(DataLabelSettingsProperty); }
            set { SetValue(DataLabelSettingsProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public ChartAxis? ActualXAxis
        {
            get
            {
                return actualXAxis;
            }

            internal set
            {
                if (actualXAxis != value)
                {
                    actualXAxis = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ChartAxis? ActualYAxis
        {
            get
            {
                return actualYAxis;
            }

            internal set
            {
                if (actualYAxis != value)
                {
                    actualYAxis = value;
                }
            }
        }
        #endregion

        #region Internal Properties

        internal CartesianChartArea? ChartArea { get; set; }

        internal double XAxisMin { get; set; } = double.MaxValue;

        internal double XAxisMax { get; set; } = double.MinValue;

        internal double YAxisMin { get; set; } = double.MaxValue;

        internal double YAxisMax { get; set; } = double.MinValue;

        internal bool IsIndexed
        {
            get { return this.ActualXAxis is CategoryAxis; }
        }

        internal DoubleRange SbsInfo { get; set; }

        internal int SideBySideIndex { get; set; }

        internal bool IsSbsValueCalculated { get; set; }

        internal override ChartDataLabelSettings ChartDataLabelSettings => DataLabelSettings;

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (DataLabelSettings != null)
            {
                SetInheritedBindingContext(DataLabelSettings, BindingContext);
            }
        }

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            base.GenerateSegments(seriesView);
        }

        internal override float TransformToVisibleX(double x, double y)
        {
            if (ChartArea != null && ChartArea.IsTransposed)
            {
                if (ActualYAxis != null)
                {
                    return ActualYAxis.ValueToPoint(y);
                }
            }
            else
            {
                if (ActualXAxis != null)
                {
                    return ActualXAxis.ValueToPoint(x);
                }
            }

            return float.NaN;
        }

        internal override float TransformToVisibleY(double x, double y)
        {
            if (ChartArea != null && ChartArea.IsTransposed)
            {
                if (ActualXAxis != null)
                {
                    return ActualXAxis.ValueToPoint(x);
                }
            }
            else
            {
                if (ActualYAxis != null)
                {
                    return ActualYAxis.ValueToPoint(y);
                }
            }

            return float.NaN;
        }

        internal virtual double GetActualSpacing()
        {
            return 0;
        }

        internal virtual double GetActualWidth()
        {
            return 0;
        }

        internal void UpdateAssociatedAxes()
        {
            if (actualXAxis != null && actualYAxis != null)
            {
                if (!actualXAxis.AssociatedAxes.Contains(actualYAxis))
                {
                    actualXAxis.AssociatedAxes.Add(actualYAxis);
                }

                if (!actualYAxis.AssociatedAxes.Contains(actualXAxis))
                {
                    actualYAxis.AssociatedAxes.Add(actualXAxis);
                }
            }
        }

        internal override void UpdateRange()
        {
            if (ChartArea == null)
                return;
            VisibleXRange = XRange;
            VisibleYRange = YRange;

            if (PointsCount <= 0)
            {
				XRange = DoubleRange.Empty;
				YRange = DoubleRange.Empty;
				VisibleXRange = XRange;
				VisibleYRange = YRange;
            }
            else
            {
                if (XAxisMin == double.MaxValue)
                {
                    XAxisMin = 0;
                }

                if (XAxisMax == double.MinValue)
                {
                    XAxisMax = 0;
                }

                if (YAxisMin == double.MaxValue)
                {
                    YAxisMin = 0;
                }

                if (YAxisMax == double.MinValue)
                {
                    YAxisMax = 0;
                }

                //TODO: Need to remove use of chart area. 
                
                var sbsMinWidth = ChartArea != null && ChartArea.SideBySideMinWidth != double.MaxValue ? ChartArea.SideBySideMinWidth : 1;

                var diff = sbsMinWidth / 2;

                if (IsSideBySide && ItemsSource != null)
                {
                    VisibleXRange = new DoubleRange(XRange.Start - diff, XRange.End + diff);

                    if ((ActualXAxis is NumericalAxis axisNumeric &&
                         axisNumeric.RangePadding == NumericalPadding.None)
                        ||
                        (ActualXAxis is DateTimeAxis axisDateTime &&
                        axisDateTime.RangePadding == DateTimeRangePadding.None))
                    {
                        diff = sbsMinWidth / 2;
                        VisibleXRange = new DoubleRange(VisibleXRange.Start + diff, VisibleXRange.End - diff);
                    }
                }
                else if (PointsCount == 1 && ItemsSource != null && Segments != null && Segments.Count == 0)
                {
                    var xValues = GetXValues();
                    var yValues =  ((XYDataSeries)this).YValues;

                    if (xValues != null && xValues.Count > 0)
                    {
                        VisibleXRange = new DoubleRange(xValues[0], xValues[0]);
                    }

                    if (yValues != null && yValues.Count > 0)
                    {
                        VisibleYRange = new DoubleRange(yValues[0], yValues[0]);
                    }
                }
            }
        }

        internal List<double>? GetXValues()
        {
            if (ActualXValues == null)
            {
                return null;
            }

            double xIndexValues = 0d;
            var xValues = ActualXValues as List<double>;

            if (IsIndexed || xValues == null)
            {
                xValues = xValues != null ? (from val in xValues select (xIndexValues++)).ToList() : (from val in (ActualXValues as List<string>) select (xIndexValues++)).ToList();
            }

            return xValues;
        }

        internal void OnActualTransposeChanged(bool transpose)
        {
            if (ActualXAxis == null || ActualYAxis == null)
            {
                return;
            }

            ActualXAxis.IsVertical = transpose;
            ActualYAxis.IsVertical = !transpose;
        }

        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            InvalidateSideBySideSeries();
            base.OnDataSourceChanged(oldValue, newValue);
        }

        internal override void HookAndUnhookCollectionChangedEvent(object oldValue, object? newValue)
        {
            base.HookAndUnhookCollectionChangedEvent(oldValue, newValue);

            if (ChartArea != null)
            {
                ChartArea.SideBySideSeriesPosition = null;
            }
        }

        internal void InvalidateSideBySideSeries()
        {
            //TODO: Reset segments for side by side changes.

            if (ChartArea != null)
            {
                ChartArea.InvalidateMinWidth();

                if (ChartArea.PreviousSBSMinWidth != ChartArea.SideBySideMinWidth)
                {
                    ChartArea.UpdateSBS();
                }
            }
        }

        internal void UpdateSbsSeries()
        {
            if (ChartArea != null)
            {
                var sideBySideSeries = ChartArea.VisibleSeries?.Where(series => series.IsSideBySide).ToList();

                if (sideBySideSeries != null && sideBySideSeries.Count > 0)
                {
                    foreach (var chartSeries in sideBySideSeries)
                    {
                        chartSeries.SegmentsCreated = false;
                    }
                }

                if (ChartArea.SideBySideSeriesPosition != null)
                {
                    ChartArea.UpdateSBS();
                }

                ScheduleUpdateChart();
            }
        }

        internal override Brush? GetFillColor(int index)
        {
            Brush? fillColor = base.GetFillColor(index);

            if (fillColor == null && ChartArea != null)
            {
                if (ChartArea.PaletteColors != null && ChartArea.PaletteColors.Count > 0)
                {
                    if (ChartArea.Series is ChartSeriesCollection series)
                    {
                        var seriesIndex = series.IndexOf(this);

                        if (seriesIndex >= 0)
                        {
                            fillColor = ChartArea.PaletteColors[seriesIndex % ChartArea.PaletteColors.Count];
                        }
                    }
                }
            }

            return fillColor != null ? fillColor : new SolidColorBrush(Colors.Transparent);
        }

        internal virtual void CalculateDataPointPosition(int index, ref double x, ref double y)
        {
            if (ActualYAxis == null || ActualXAxis == null || ChartArea == null) return;


            double X = x;

            if (ActualXAxis != null && !double.IsNaN(x))
            {
                x = ChartArea.IsTransposed ? ActualYAxis.ValueToPoint(y) : ActualXAxis.ValueToPoint(X);
            }

            if (ActualYAxis != null && !double.IsNaN(y))
            {
                y = ChartArea.IsTransposed ? ActualXAxis != null ? ActualXAxis.ValueToPoint(X) : double.NaN : ActualYAxis.ValueToPoint(y);
            }
        }

        internal bool IsDataInVisibleRange(double xValue, double yValue)
        {
            if (ActualYAxis == null || ActualXAxis == null) return false;

            if (xValue < ActualXAxis.VisibleRange.Start || xValue > ActualXAxis.VisibleRange.End
                          || yValue < ActualYAxis.VisibleRange.Start || yValue > ActualYAxis.VisibleRange.End)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            if (Segments == null) return null;

            int index = IsSideBySide ? GetDataPointIndex(x, y) : SeriesContainsPoint(new PointF(x, y)) ? TooltipDataPointIndex : -1;

            if (index < 0 || ItemsSource == null || ActualData == null || ActualXAxis == null
                || ActualYAxis == null || SeriesYValues == null)
            {
                return null;
            }

            var xValues = GetXValues();

            if (xValues == null || ChartArea == null) return null;

            object dataPoint = ActualData[index];
            double xValue = xValues[index];
            IList<double> yValues = SeriesYValues[0];
            double yValue = Convert.ToDouble(yValues[index]);
            float xPosition = TransformToVisibleX(xValue, yValue);

            if (!double.IsNaN(xPosition) && !double.IsNaN(yValue))
            {
                float yPosition = TransformToVisibleY(xValue, yValue);

                if (IsSideBySide)
                {
                    double xMidVal = xValue + SbsInfo.Start + ((SbsInfo.End - SbsInfo.Start) / 2);
                    xPosition = TransformToVisibleX(xMidVal, yValue);
                    yPosition = TransformToVisibleY(xMidVal, yValue);
                }

                RectF seriesBounds = AreaBounds;
                seriesBounds = new RectF(0, 0, seriesBounds.Width, seriesBounds.Height);

                yPosition = seriesBounds.Top < yPosition ? yPosition : seriesBounds.Top;
                yPosition = seriesBounds.Bottom > yPosition ? yPosition : seriesBounds.Bottom;
                xPosition = seriesBounds.Left < xPosition ? xPosition : seriesBounds.Left;
                xPosition = seriesBounds.Right > xPosition ? xPosition : seriesBounds.Right;

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
            return null;
        }

        internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            float xPosition = tooltipInfo.X;
            float yPosition = tooltipInfo.Y;
            float sizeValue = 1;
            float halfSizeValue = 0.5f;

            Rect targetRect = new Rect(xPosition - halfSizeValue, yPosition, sizeValue, sizeValue);

            if ((xPosition + seriesBounds.Left) == seriesBounds.Left)
            {
                targetRect = new Rect(xPosition - sizeValue, yPosition - halfSizeValue, sizeValue, sizeValue);
                tooltipInfo.Position = Core.TooltipPosition.Right;
            }
            else if (xPosition == seriesBounds.Width)
            {
                targetRect = new Rect(xPosition + sizeValue, yPosition - halfSizeValue, sizeValue, sizeValue);
                tooltipInfo.Position = Core.TooltipPosition.Left;
            }
            else if (yPosition == seriesBounds.Top)
            {
                targetRect = new Rect(xPosition - halfSizeValue, -sizeValue, sizeValue, sizeValue);
                tooltipInfo.Position = Core.TooltipPosition.Bottom;
            }

            tooltipInfo.TargetRect = targetRect;
        }

        internal override void UpdateLegendIconColor()
        {
            var legend = Chart?.Legend;
            var legendItems = ChartArea?.PlotArea.LegendItems;

            if (legend != null && legend.IsVisible && legendItems != null)
            {
                foreach (CartesianLegendItem legendItem in legendItems)
                {
                    if (legendItem != null && legendItem.Series == this)
                    {
                        legendItem.IconBrush = GetFillColor(legendItem.Index) ?? new SolidColorBrush(Colors.Transparent);
                        break;
                    }
                }
            }
        }

        internal object? FindNearestChartPoint(float pointX, float pointY)
        {
            if (actualXAxis != null && ChartArea != null)
            {
                double xStart = actualXAxis.VisibleRange.Start;
                double xEnd = actualXAxis.VisibleRange.End;
                //TODO: Need to recheck this case if the bounds is correct or not.
                double xValue = actualXAxis.PointToValue(pointX - AreaBounds.Left, pointY - AreaBounds.Top);

                if (IsIndexed)
                {
                    xValue = Math.Round(xValue);
                    //Todo: Grouping
                    var categoryAxis = actualXAxis as CategoryAxis;
                    if (categoryAxis != null)
                    {
                        int dataCount = PointsCount;
                        if (xValue <= xEnd && xValue >= xStart && xValue < dataCount && xValue >= 0)
                        {
                            return ActualData != null ? ActualData[(int)xValue] : null;
                        }
                    }
                }
                else
                {
                    double nearestX = xStart;
                    object? data = null;
                    var xValues = GetXValues();

                    if (xValues != null)
                    {
                        for (int i = 0; i < PointsCount; i++)
                        {
                            var validateXValue = xValues[i];
                            if (Math.Abs(xValue - validateXValue) <= Math.Abs(xValue - nearestX))
                            {
                                nearestX = validateXValue;
                                data = ActualData != null ? ActualData[i] : null;
                            }
                        }
                    }

                    return data;
                }
            }

            return null;
        }

        #endregion

        #region Callbacks 

        private static void OnDataLabelSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chartSeries = bindable as CartesianSeries;

            if (chartSeries != null)
            {
                chartSeries.OnDataLabelSettingsPropertyChanged(oldValue as ChartDataLabelSettings, newValue as ChartDataLabelSettings);
            }
        }

        private static void OnAxisNameChanged(BindableObject bindable, object oldValue, object newValue)
        {

        }

        private static void OnLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CartesianSeries chartSeries)
            {
                var legendItems = chartSeries.ChartArea?.PlotArea.LegendItems;

                if (legendItems != null)
                {
                    foreach (CartesianLegendItem legendItem in legendItems)
                    {
                        if (legendItem != null && legendItem.Series == chartSeries)
                        {
                            legendItem.Text = chartSeries.Label;
                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
