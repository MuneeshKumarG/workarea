using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// CartesianSeries is the base class for all cartesian charts such as column, line, area, and so on.
    /// </summary>
    public abstract class CartesianSeries : ChartSeries
    {
        
        private ChartAxis? actualXAxis;
        private ChartAxis? actualYAxis;

        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="XAxisName"/> bindable property.
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
        /// Identifies the <see cref="YAxisName"/> bindable property.
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
        /// Identifies the <see cref="DataLabelSettings"/> bindable property.
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
        /// Identifies the <see cref="Label"/> bindable property.
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

        /// <summary>
        /// Identifies the <see cref="ShowTrackballLabel"/> bindable property.
        /// </summary>       
        public static readonly BindableProperty ShowTrackballLabelProperty =
            BindableProperty.Create(
                nameof(ShowTrackballLabel),
                typeof(bool),
                typeof(CartesianSeries),
                true,
                BindingMode.Default,
                null);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianSeries"/>.
        /// </summary>
        public CartesianSeries()
        {
            DataLabelSettings = new CartesianDataLabelSettings();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the name of the (horizontal) axis in the <see cref="SfCartesianChart.XAxes"/> collection which is used to plot the series with particular axis.
        /// </summary>
        /// <value>It takes the string value and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis x:Name="XAxis1"/>
        ///        <chart:NumericalAxis x:Name="XAxis2"/>
        ///    </chart:SfCartesianChart.XAxes>
        ///    
        ///    <chart:SfCartesianChart.YAxes>
        ///       <chart:NumericalAxis />
        ///   </chart:SfCartesianChart.YAxes>
        ///
        ///          <chart:SplineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" 
        ///                            XAxisName="XAxis1" />
        ///          <chart:ColumnSeries ItemsSource = "{Binding Data}"
        ///                        XBindingPath="XValue"
        ///                        YBindingPath="YValue"
        ///                        XAxisName="XAxis2" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     var XAxis1 = new CategoryAxis() { Name = "XAXis1" };
        ///     var XAxis2 = new NumericalAxis() { Name = "XAXis2" }; 
        ///     chart.XAxes.Add(XAxis1);
        ///     chart.XAxes.Add(XAxis2);
        ///     var YAxis = new NumericalAxis();
        ///     chart.YAxes.Add(YAxis);
        ///
        ///     SplineSeries splineSeries = new SplineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           XAxisName = "XAXis1",
        ///     };
        ///     
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           XAxisName = "XAXis2",
        ///     };
        ///     
        ///     chart.Series.Add(splineSeries);
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string XAxisName
        {
            get { return (string)GetValue(XAxisNameProperty); }
            set { SetValue(XAxisNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the (vertical) axis in the <see cref="SfCartesianChart.YAxes"/> collection which is used to plot the series with particular axis.
        /// </summary>
        /// <value>It takes the string value and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis />
        ///    </chart:SfCartesianChart.XAxes>
        ///    
        ///    <chart:SfCartesianChart.YAxes>
        ///       <chart:NumericalAxis x:Name="YAxis1"/>
        ///       <chart:NumericalAxis x:Name="YAxis2"/>
        ///   </chart:SfCartesianChart.YAxes>
        ///
        ///          <chart:SplineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" 
        ///                            YAxisName="YAxis1" />
        ///          <chart:ColumnSeries ItemsSource = "{Binding Data}"
        ///                        XBindingPath="XValue"
        ///                        YBindingPath="YValue"
        ///                        YAxisName="YAxis2" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     var XAxis = new CategoryAxis();
        ///     chart.XAxes.Add(XAxis);
        ///     var YAxis1 = new NumericalAxis(){Name = "YAXis1"};
        ///     var YAxis2 = new NumericalAxis(){Name = "YAXis2"};
        ///     chart.YAxes.Add(YAxis1);
        ///     chart.YAxes.Add(YAxis2);
        ///
        ///     SplineSeries splineSeries = new SplineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           YAxisName = "YAXis1",
        ///     };
        ///     
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           YAxisName = "YAXis2",
        ///     };
        ///     
        ///     chart.Series.Add(splineSeries);
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string YAxisName
        {
            get { return (string)GetValue(YAxisNameProperty); }
            set { SetValue(YAxisNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the appearance of the displaying data labels in the cartesian series.
        /// </summary>
        /// <remarks> This allows us to change the look of the displaying labels' content, and shapes at the data point.</remarks>
        /// <value>
        /// It takes the <see cref="CartesianDataLabelSettings"/>.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-5)
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
        ///               <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   YBindingPath="YValue"
        ///                                   ShowDataLabels="True">
        ///                    <chart:ColumnSeries.DataLabelSettings>
        ///                         <chart:CartesianDataLabelSettings BarAlignment="Middle" />
        ///                    </ chart:ColumnSeries.DataLabelSettings>
        ///               </chart:ColumnSeries> 
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-6)
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
        ///         BarAlignment = DataLabelAlignment.Middle,
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
        /// Gets or sets a value that will be displayed in the associated legend item.
        /// </summary>
        /// <value>It accepts a string value and its default value is string.Empty.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Label = "ColumnSeries"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Label = "ColumnSeries",
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Gets the actual XAxis value.
        /// </summary>
        public ChartAxis? ActualXAxis
        {
            get
            {
                return actualXAxis;
            }

            internal set
            {
                if (actualXAxis != null && value == null)
                {
                    actualXAxis.ClearRegisteredSeries();
                }

                if (actualXAxis != value)
                {
                    actualXAxis = value;
                }
            }
        }

        /// <summary>
        /// Gets the actual YAxis value.
        /// </summary>
        public ChartAxis? ActualYAxis
        {
            get
            {
                return actualYAxis;
            }

            internal set
            {
                if (actualXAxis != null && value == null)
                {
                    actualXAxis.ClearRegisteredSeries();
                }

                if (actualYAxis != value)
                {
                    actualYAxis = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets whether to show trackball label on the corresponding series
        /// </summary>
        public bool ShowTrackballLabel
        {
            get { return (bool)GetValue(ShowTrackballLabelProperty); }
            set { SetValue(ShowTrackballLabelProperty, value); }
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

        internal bool IsGrouped
        {
            get 
            {
                if (this.ActualXValues is CategoryAxis category)
                    return category.ArrangeByIndex;

                return false;
            }
        }

        internal DoubleRange SbsInfo { get; set; } = DoubleRange.Empty;

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
#if WinUI
  if (ActualXValues == null)
            {
                return null;
            }

            double xIndexValues = 0d;
            var xValues = ActualXValues as List<double>;

            if (IsIndexed || xValues == null)
            {
                if (ActualXAxis is CategoryAxis categoryAxis && !categoryAxis.ArrangeByIndex || ActualXAxis == null)
                {
                    xValues = GroupedXValuesIndexes.Count > 0 ? GroupedXValuesIndexes : (from val in (ActualXValues as List<string>) select (xIndexValues++)).ToList();
                }
                else
                {
                    xValues = xValues != null ? (from val in xValues select (xIndexValues++)).ToList() : (from val in (ActualXValues as List<string>) select (xIndexValues++)).ToList();
                }
            }
#else
            var xValues = GetActualXValues();
#endif
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

            Rect targetRect = tooltipInfo.TargetRect.IsEmpty ? new Rect(xPosition - halfSizeValue, yPosition, sizeValue, sizeValue) : tooltipInfo.TargetRect;

            if (tooltipInfo.TargetRect.IsEmpty)
            {

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
            }
            else
            {
                var markerToolTip = tooltipInfo.TargetRect;

                if ((markerToolTip.X + markerToolTip.Width / 2 + seriesBounds.Left) == seriesBounds.Left)
                {
                    targetRect = new Rect(markerToolTip.X + markerToolTip.Width / 2, markerToolTip.Y, markerToolTip.Width / 2, markerToolTip.Height);
                    tooltipInfo.Position = Core.TooltipPosition.Right;
                }
                else if ((markerToolTip.X + markerToolTip.Width / 2) == seriesBounds.Width)
                {
                    tooltipInfo.Position = Core.TooltipPosition.Left;
                }
            }
            
            tooltipInfo.TargetRect = targetRect;
        }

        internal override void UpdateLegendIconColor()
        {
            var legend = Chart?.Legend;
            var legendItems = ChartArea?.PlotArea.LegendItems;

            if (legend != null && legend.IsVisible && legendItems != null)
            {
                foreach (LegendItem legendItem in legendItems)
                {
                    if (legendItem != null && legendItem.Item == this)
                    {
                        legendItem.IconBrush = GetFillColor(legendItem.Index) ?? new SolidColorBrush(Colors.Transparent);
                        break;
                    }
                }
            }
        }

        internal override void UpdateLegendIconColor(int index)
        {
            var legend = Chart?.Legend;
            var legendItems = ChartArea?.PlotArea.LegendItems;

            if (legend != null && legend.IsVisible && legendItems != null && index < legendItems.Count)
            {
                if (legendItems[index] is LegendItem legendItem && legendItem.Item == this)
                {
                    legendItem.IconBrush = GetFillColor(index) ?? new SolidColorBrush(Colors.Transparent);
                }
            }
        }

        internal object? FindNearestChartPoint(float pointX, float pointY)
        {
            List<object> dataPointsList = FindNearestChartPoints(pointX, pointY);
            if (dataPointsList.Count > 0)
            {
                return dataPointsList[0];
            }
            else
            {
                return null;
            }
        }

        internal List<object> FindNearestChartPoints(float pointX, float pointY)
        {
            List<object> dataPointsList = new List<object>();
            double delta = 0;
            if (actualXAxis != null && ChartArea != null)
            {
                double xStart = actualXAxis.VisibleRange.Start;
                double xEnd = actualXAxis.VisibleRange.End;
                //TODO: Need to recheck this case if the bounds is correct or not.
                double xValue = actualXAxis.PointToValue(pointX - AreaBounds.Left, pointY - AreaBounds.Top);

                if (IsIndexed)
                {
                    xValue = Math.Round(xValue);
                    var isGrouped = this.ActualXAxis is CategoryAxis category && !category.ArrangeByIndex;
                    int dataCount = isGrouped ? GroupedXValues.Count : PointsCount;

                    if (xValue <= xEnd && xValue >= xStart && xValue < dataCount && xValue >= 0)
                    {
                        var dataPoint = new object();

                        if (isGrouped)
                        {
                            dataPoint = GroupedActualData != null ? GroupedActualData[(int)xValue] : null;
                        }
                        else
                        {
                            dataPoint = ActualData != null ? ActualData[(int)xValue] : null;
                        }

                        if (dataPoint != null)
                        {
                            dataPointsList.Add(dataPoint);
                        }
                    }
                }
                else
                {
                    var xValues = GetXValues();
                    if (xValues != null)
                    {
                        if (IsLinearData)
                        {
                            var index = ChartUtils.BinarySearch(xValues, xValue, 0, PointsCount - 1);
                            var dataPoint = ActualData != null ? ActualData[index] : null;
                            if (dataPoint != null)
                            {
                                dataPointsList.Add(dataPoint);
                            }
                        }
                        else
                        {
                            double nearPointX = xStart;
                            float xPoint = ChartArea.IsTransposed ? pointY:pointX;
                            for (int i = 0; i < PointsCount; i++)
                            {
                                double currX = actualXAxis.ValueToPoint(xValues[i]) + AreaBounds.Left;
                                if (delta == xPoint - currX)
                                {
                                    var dataPoint = ActualData != null ? ActualData[i] : null;
                                    if (dataPoint != null)
                                    {
                                        dataPointsList.Add(dataPoint);
                                    }
                                }
                                else if (Math.Abs(pointX - currX) <= Math.Abs(pointX - nearPointX))
                                {
                                    nearPointX = currX;
                                    delta = pointX - currX;
                                    dataPointsList.Clear();
                                    var dataPoint = ActualData != null ? ActualData[i] : null;
                                    if (dataPoint != null)
                                    {
                                        dataPointsList.Add(dataPoint);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return dataPointsList;
        }

        internal virtual void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos, ref bool isSidebySide)
        {
            GeneratePointInfos(nearestDataPoints, pointInfos);
        }

        private void GeneratePointInfos(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos)
        {
            var xValues = GetXValues();

            if (nearestDataPoints != null && ActualData != null && xValues != null && SeriesYValues != null)
            {
                IList<double> yValues = SeriesYValues[0];
                foreach (object point in nearestDataPoints)
                {
                    int index = ActualData.IndexOf(point);
                    var xValue = xValues[index];
                    double yValue = yValues[index];
                    string label = yValue.ToString();
                    var xPoint = TransformToVisibleX(xValue, yValue);
                    var yPoint = TransformToVisibleY(xValue, yValue);


                    // Checking YValue is contain in plotarea
                    //Todo: need to check with transposed
                    //if (!AreaBounds.Contains(xPoint + AreaBounds.Left, yPoint + AreaBounds.Top)) continue;

                    TrackballPointInfo? chartPointInfo = CreateTrackballPointInfo(xPoint, yPoint, label, point);

                    if (chartPointInfo != null)
                    {
                        chartPointInfo.XValue = xValue;
                        pointInfos.Add(chartPointInfo);
                    }
                }
            }
        }

        internal TrackballPointInfo? CreateTrackballPointInfo(float x, float y, string label, object data)
        {
            if (Chart is SfCartesianChart chart)
            {
                TrackballPointInfo pointInfo = new TrackballPointInfo(this)
                {
                    DataItem = data,
                    X = x,
                    Y = y,
                    Label = label,
                };

                return pointInfo;
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
            if (bindable is CartesianSeries series && series.ChartArea != null)
            {
                series.ChartArea.RequiredAxisReset = true;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CartesianSeries chartSeries)
            {
                var legendItems = chartSeries.ChartArea?.PlotArea.LegendItems;

                if (legendItems != null)
                {
                    foreach (LegendItem legendItem in legendItems)
                    {
                        if (legendItem != null && legendItem.Item == chartSeries)
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
