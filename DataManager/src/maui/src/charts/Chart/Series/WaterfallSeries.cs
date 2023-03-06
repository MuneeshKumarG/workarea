using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="WaterfallSeries"/> shows that an initial value is affected by a series of intermediate positive or negative values, leading to a final value.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="WaterfallSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XYDataSeries.StrokeWidth"/>,  and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="WaterfallSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="ChartSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="WaterfallSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para><b>Spacing - </b> To specify the spacing between segments using the <see cref="Spacing"/> property.</para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.XAxes>
    ///               <chart:CategoryAxis/>
    ///           </chart:SfCartesianChart.XAxes>
    ///
    ///           <chart:SfCartesianChart.YAxes>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.YAxes>
    ///
    ///           <chart:SfCartesianChart.Series>
    ///               <chart:WaterfallSeries
    ///                   ItemsSource="{Binding Sales}"
    ///                   XBindingPath="Department"
    ///                   YBindingPath="Value"/>
    ///           </chart:SfCartesianChart.Series>  
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     
    ///     NumericalAxis xAxis = new CategoryAxis();
    ///     NumericalAxis yAxis = new NumericalAxis();
    ///     
    ///     chart.XAxes.Add(xAxis);
    ///     chart.YAxes.Add(yAxis);
    ///     
    ///     ViewModel viewModel = new ViewModel();
    /// 
    ///     ColumnSeries series = new WaterfallSeries();
    ///     series.ItemsSource = viewModel.Sales;
    ///     series.XBindingPath = "Department";
    ///     series.YBindingPath = "Value";
    ///     chart.Series.Add(series);
    ///     
    /// ]]></code>
    /// # [ViewModel](#tab/tabid-3)
    /// <code><![CDATA[
    ///     public ObservableCollection<Model> Sales { get; set; }
    /// 
    ///     public ViewModel()
    ///     {
    ///        Sales = new ObservableCollection<Model>();
    ///        Sales.Add(new ChartDataModel() { Department = "Income", Value = 46 });
    ///        Sales.Add(new ChartDataModel() { Department = "Sales", Value = -14 });
    ///        Sales.Add(new ChartDataModel() { Department = "Research", Value = -9});
    ///        Sales.Add(new ChartDataModel() { Department = "Revenue", Value = 15 });
    ///        Sales.Add(new ChartDataModel() { Department = "Balance", Value = 38 , IsSummary= true });
    ///        Sales.Add(new ChartDataModel() { Department = "Expense", Value = -13 });
    ///        Sales.Add(new ChartDataModel() { Department = "Tax", Value = -8 });
    ///        Sales.Add(new ChartDataModel() { Department = "Profit", Value =17,IsSummary=true });
    ///     }
    /// ]]></code>
    /// ***
    /// </example>
    /// <seealso cref="WaterfallSegment"/>
    public class WaterfallSeries : XYDataSeries
    {

        #region  Private Fields

        private double bottomValue;

        #endregion

        #region  Properties

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="AllowAutoSum"/> bindable property.
        /// </summary>
        public static readonly BindableProperty AllowAutoSumProperty =
            BindableProperty.Create(
                nameof(AllowAutoSum),
                typeof(bool),
                typeof(WaterfallSeries),
                true,
                BindingMode.Default,
                null, propertyChanged: OnAllowAutoSumChanged);

        /// <summary>
        /// Identifies the <see cref="ShowConnectorLine"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ShowConnectorProperty =
            BindableProperty.Create(
                nameof(ShowConnectorLine),
                typeof(bool),
                typeof(WaterfallSeries),
                defaultValue: true, propertyChanged: OnShowConnectorChanged);

        /// <summary>
        /// Identifies the <see cref="ConnectorLineStyle"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ConnectorLineStyleProperty =
            BindableProperty.Create(
                nameof(ConnectorLineStyle),
                typeof(ChartLineStyle),
                typeof(WaterfallSeries),
                null, propertyChanged: OnConnectorLineStyleChanged);

        /// <summary>
        ///Identifies the <see cref="SummaryBindingPath"/> bindable property.
        /// </summary>
        public static readonly BindableProperty SummaryBindingPathProperty =
            BindableProperty.Create(
                nameof(SummaryBindingPath),
                typeof(string),
                typeof(WaterfallSeries),
                defaultValue: string.Empty,
                propertyChanged: OnSummaryBindingPathChanged);

        /// <summary>
        /// Identifies the <see cref="SummaryPointsBrush"/> bindable property.
        /// </summary>
        public static readonly BindableProperty SummaryPointsBrushProperty =
            BindableProperty.Create(
                nameof(SummaryPointsBrush),
                typeof(Brush),
                typeof(WaterfallSeries),
                null,
                BindingMode.Default,
                null, propertyChanged: OnSummaryPointsBrushChanged);

        /// <summary>
        /// Identifies the <see cref="NegativePointsBrush"/> bindable property.
        /// </summary>
        public static readonly BindableProperty NegativePointsBrushProperty =
            BindableProperty.Create(
                nameof(NegativePointsBrush),
                typeof(Brush),
                typeof(WaterfallSeries),
                null,
                BindingMode.Default,
                null, propertyChanged: OnNegativePointsBrushChanged);


        /// <summary>
        /// Identifies the <see cref="Width"/> bindable property.
        /// </summary>  
        public static readonly BindableProperty WidthProperty =
            BindableProperty.Create(
                nameof(Width),
                typeof(double),
                typeof(WaterfallSeries),
                defaultValue: 0.8d,
                BindingMode.Default,
                null, propertyChanged: OnWidthChanged);

        /// <summary>
        /// Identifies the <see cref="Spacing"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty SpacingProperty =
            BindableProperty.Create(
                nameof(Spacing),
                typeof(double),
                typeof(WaterfallSeries),
                0d,
                BindingMode.Default,
                null, propertyChanged: OnSpacingChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Get or set an indication whether the intermediate sum values in a series should be automatically calculated or not.
        /// </summary>
        /// <value>It accepts <see cref="bool"/> and the default is true.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource="{Binding Sales}"   
        ///                                                     XBindingPath="Department"   
        ///                                                     YBindingPath="Value"
        ///                                                     AllowAutoSum="False"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           AllowAutoSum="False",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public bool AllowAutoSum
        {
            get { return (bool)GetValue(AllowAutoSumProperty); }
            set { SetValue(AllowAutoSumProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the connector lines or not in the chart.
        /// </summary>
        /// <value>It accepts <see cref="bool"/> and the default  is true.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource="{Binding Sales}"   
        ///                                                     XBindingPath="Department"   
        ///                                                     YBindingPath="Value"
        ///                                                     ShowConnectorline="False"/>      
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           ShowConnectorline="False",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public bool ShowConnectorLine
        {
            get { return (bool)GetValue(ShowConnectorProperty); }
            set { SetValue(ShowConnectorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for connector line and it is often used to customize the appearance of connector lines for visual purposes.
        /// </summary>
        /// <value>Its default is null/> </value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource="{Binding Sales}"   
        ///                                                     XBindingPath="Department"   
        ///                                                     YBindingPath="Value"/>
        ///               <chart:WaterfallSeries.ConnectorLineStyle>
        ///                       <chart:ChartLineStyle Stroke="Red" >
        ///                </chart:WaterfallSeries.ConnectorLineStyle>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///     };
        ///     series.ConnectorLineStyle = new ChartLineStyle()
        ///     {
        ///          Stroke = new SolidColorBrush(Colors.Red),
        ///      }
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ChartLineStyle ConnectorLineStyle
        {
            get { return (ChartLineStyle)GetValue(ConnectorLineStyleProperty); }
            set { SetValue(ConnectorLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets string indicates that the segment is calculated based on the sum of previous segment values.
        /// </summary>
        /// <value>It accepts <see cref="string"/> and the default is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource="{Binding Sales}"   
        ///                                                     XBindingPath="Department"   
        ///                                                     YBindingPath="Value"
        ///                                                     SummaryBindingpath="IsSummary"/>
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           SummaryBindingpath="IsSummary",
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string SummaryBindingPath
        {
            get { return (string)GetValue(SummaryBindingPathProperty); }
            set { SetValue(SummaryBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color that indicates the summary segment's interior in Waterfall series
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values, Its default is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource="{Binding Sales}"   
        ///                                                     XBindingPath="Department"   
        ///                                                     YBindingPath="Value"
        ///                                                     SummaryPointsBrush="Blue"/>
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
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           SummaryPointsBrush=Colors.Blue,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush SummaryPointsBrush
        {
            get { return (Brush)GetValue(SummaryPointsBrushProperty); }
            set { SetValue(SummaryPointsBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color that indicates the Negative segment's interior in waterfall series.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values. Its default is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource="{Binding Sales}"   
        ///                                                     XBindingPath="Department"   
        ///                                                     YBindingPath="Value"
        ///                                                     NegativePointsBrush="Red"/>
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
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           NegativePointsBrush=Colors.Red,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public Brush NegativePointsBrush
        {
            get { return (Brush)GetValue(NegativePointsBrushProperty); }
            set { SetValue(NegativePointsBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of Waterfall series.
        /// </summary>
        /// <value>It accepts <see cref="double"/>. The default is 0.8/> </value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource="{Binding Sales}"   
        ///                                                     XBindingPath="Department"   
        ///                                                     YBindingPath="Value"
        ///                                                     Width="1"/>
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
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           Width=1,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }

        /// <summary>
        ///  Gets or sets the spacing between the segments across the series in cluster mode.
        /// </summary>
        /// <value>It accepts <see cref="double"/> .The default is 0.0/> </value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///           <chart:WaterfallSeries ItemsSource="{Binding Sales}"   
        ///                                                     XBindingPath="Department"   
        ///                                                     YBindingPath="Value"
        ///                                                     Spacing="1"/>
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
        ///     WaterfallSeries series = new WaterfallSeries()
        ///     {
        ///           ItemsSource = viewModel.Sales,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Value",
        ///           Spacing=1,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }


        #endregion

        #region Internal Property

        /// <summary>
        /// Gets a value indicating whether this WaterfallSeries is side by side.
        /// </summary>
        /// <value><c>true</c> if is side by side; otherwise, <c>false</c>.</value>
        internal override bool IsSideBySide => true;

        #endregion

        #region Private Property

        private List<bool> summaryValues;

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the WaterfallSeries class.
        /// </summary>
        public WaterfallSeries()
        {
            ConnectorLineStyle = new ChartLineStyle()
            {
                Stroke = Color.FromArgb("#ABABAB"),
                StrokeWidth = 1
            };

            summaryValues = new List<bool>();

        }
        #endregion

        #region Methods

        #region Internal Override Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            double x1, x2, y1, y2;

            GetSummaryValues();

            if (xValues == null || xValues.Count == 0 || YValues.Count == 0)
            {
                return;
            }

            if (ActualXAxis != null)
            {
                bottomValue = double.IsNaN(ActualXAxis.ActualCrossingValue) ? 0 : ActualXAxis.ActualCrossingValue;
            }
            
            if (IsIndexed || xValues == null)
            {
                for (var i = 0; i < PointsCount; i++)
                {
                    if (xValues != null)
                    {
                        OnCalculateSegmentValues(out x1, out x2, out y1, out y2, i, bottomValue, xValues[i]);

                        if (i < Segments.Count && Segments[i] is WaterfallSegment)
                        {
                            ((WaterfallSegment)Segments[i]).SetData(new[] { x1, x2, y1, y2, i, YValues[i] });
                        }
                        else
                        {
                            CreateSegment(seriesView, new[] { x1, x2, y1, y2, i, YValues[i] }, i);
                        }
                    }
                }
            }
            else
            {
                for (var i = 0; i < PointsCount; i++)
                {
                    var x = xValues[i];

                    OnCalculateSegmentValues(out x1, out x2, out y1, out y2, i, bottomValue, xValues[i]);

                    if (i < Segments.Count && Segments[i] is WaterfallSegment)
                    {
                        ((WaterfallSegment)Segments[i]).SetData(new[] { x1, x2, y1, y2, x, YValues[i] });
                    }
                    else
                    {
                        CreateSegment(seriesView, new[] { x1, x2, y1, y2, x, YValues[i] }, i);
                    }
                }
            }
        }

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            TooltipInfo? tooltipInfo = base.GetTooltipInfo(tooltipBehavior, x, y);
            if (tooltipInfo != null)
            {
                if (Segments[tooltipInfo.Index] is WaterfallSegment waterfallSegment)
                {
                    if (waterfallSegment.SegmentType == WaterfallSegmentType.Sum)
                    {
                        tooltipInfo.Text = waterfallSegment.Sum.ToString();
                    }
                }
            }

            return tooltipInfo;
        }

        internal override void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            WaterfallSegment? waterfallSegment = Segments[tooltipInfo.Index] as WaterfallSegment;
           
            if (waterfallSegment != null)
            {
                RectF targetRect = waterfallSegment.SegmentBounds;
                float xPosition = tooltipInfo.X;
                float yPosition;
                float width = targetRect.Width;
                float height = targetRect.Height;

                if (ChartArea != null && ChartArea.IsTransposed)
                {
                    xPosition = waterfallSegment.SegmentBounds.Center.X;
                    yPosition = waterfallSegment.SegmentBounds.Top;
                }
                else
                {
                    yPosition = waterfallSegment.Top;
                }

                targetRect = new Rect(xPosition - width / 2, yPosition, width, height);
                tooltipInfo.TargetRect = targetRect;
            }
        }

        internal override double GetActualWidth()
        {
            return Width;
        }

        internal override double GetActualSpacing()
        {
            return Spacing;
        }

        internal override double GetDataLabelPositionAtIndex(int index)
        {
            double dataLabelPositionAtIndex = 0;
            if (Segments.Count >= index)
            {
                WaterfallSegment? segment = Segments[index] as WaterfallSegment;

                if (segment != null)
                {
                    double median = segment.y1 + ((segment.y2 - segment.y1) / 2);
                    var segmentType = segment.SegmentType;
                    double waterfallSum = segment.WaterfallSum;
                    if (segmentType is WaterfallSegmentType.Sum)
                    {
                        dataLabelPositionAtIndex = AllowAutoSum ? 
                            (DataLabelSettings.BarAlignment == DataLabelAlignment.Middle) ? (waterfallSum / 2) : 
                            (waterfallSum >= 0) ? segment.y1 : segment.y2 : (DataLabelSettings.BarAlignment == DataLabelAlignment.Middle) ? median : (YValues[index] >= 0) ? segment.y1 : segment.y2;
                    }
                    else if (DataLabelSettings.BarAlignment == DataLabelAlignment.Top)
                    {
                        dataLabelPositionAtIndex = (segmentType is WaterfallSegmentType.Positive) ? segment.y1 : segment.y2;
                    }
                    else if (DataLabelSettings.BarAlignment == DataLabelAlignment.Bottom)
                    {
                        dataLabelPositionAtIndex = (segmentType is WaterfallSegmentType.Positive) ? segment.y2 : segment.y1;
                    }
                    else
                        dataLabelPositionAtIndex = median;
                }
            }
            
            return dataLabelPositionAtIndex;
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

        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabelSettings = ChartDataLabelSettings;

            if (dataLabelSettings == null) return;

            ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;

            foreach (ChartSegment datalabel in Segments)
            {

                if (!datalabel.IsEmpty)
                {
                    UpdateDataLabelAppearance(canvas, datalabel, dataLabelSettings, labelStyle);
                }
            }
        }

        internal override Brush? GetFillColor(int index)
        {
            Brush? fillColor = base.GetFillColor(index);

            if(fillColor == Chart?.GetSelectionBrush(this) || fillColor == GetSelectionBrush(index))
            {
                return fillColor;
            }

            if (Segments[index] is WaterfallSegment segment)
            {
                switch (segment.SegmentType)
                {
                    case WaterfallSegmentType.Negative:
                        return NegativePointsBrush != null ? NegativePointsBrush : fillColor;
                    case WaterfallSegmentType.Sum:
                        return SummaryPointsBrush != null ? SummaryPointsBrush : fillColor;
                }
            }

            return fillColor;
        }

        internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos, ref bool isSidebySide)
        {
            var xValues = GetXValues();
            float xPosition = 0f;
            float yPosition = 0f;
            if (nearestDataPoints != null && ActualData != null && xValues != null && SeriesYValues != null)
            {
                IList<double> yValues = SeriesYValues[0];
                foreach (object point in nearestDataPoints)
                {
                    int index = ActualData.IndexOf(point);
                    var xValue = xValues[index];
                    WaterfallSegment? segment = Segments[index] as WaterfallSegment;
                    double yValue = yValues[index];
                    
                    if (segment != null)
                    {
                        yValue = segment.y1;
                        if (segment.SegmentType == WaterfallSegmentType.Negative)
                            yValue = segment.y2;
                    }
                    
                    string label = yValue.ToString();
                    
                    if (IsSideBySide)
                    {
                        isSidebySide = true;
                        double xMidVal = xValue + SbsInfo.Start + ((SbsInfo.End - SbsInfo.Start) / 2);
                        xPosition = TransformToVisibleX(xMidVal, yValue);
                        yPosition = TransformToVisibleY(xMidVal, yValue);
                    }

                    TrackballPointInfo? chartPointInfo = CreateTrackballPointInfo(xPosition, yPosition, label, point);

                    if (chartPointInfo != null)
                    {
                        chartPointInfo.XValue = xValue;
                        pointInfos.Add(chartPointInfo);
                    }
                }
            }
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Creates the Waterfall segments.
        /// </summary>
        protected override ChartSegment? CreateSegment()
        {
            return new WaterfallSegment();
        }

        #endregion

        #region Private Methods

        private void GetSummaryValues()
        {
            List<bool> doubleToBoolList = new List<bool>();

            if (YDoubleList == null || YDoubleList.Count == 0) return;

            if (YDataPaths.Contains(this.SummaryBindingPath))
            {
                int index = YDataPaths.IndexOf(this.SummaryBindingPath);
                if (index >= 0)
                {
                    doubleToBoolList.AddRange(YDoubleList[index].Select(item => item != 0));
                    this.summaryValues = doubleToBoolList;
                }
            }
        }

        private void OnCalculateSegmentValues(out double x1, out double x2, out double y1, out double y2, int i, double bottomValue, double xVal)
        {
            x1 = xVal + SbsInfo.Start;
            x2 = xVal + SbsInfo.End;
            y1 = y2 = double.NaN;
            
            //Calculation for First Segment
            if (i == 0)
            {
                if (YValues[i] >= 0)
                {
                    y1 = YValues[i];
                    y2 = bottomValue;
                }
                else if (double.IsNaN(YValues[i]))
                {
                    y2 = bottomValue;
                    y1 = bottomValue;
                }
                else
                {
                    y2 = YValues[i];
                    y1 = bottomValue;
                }
            }
            else
            {
                if (Segments[i - 1] is WaterfallSegment prevSegment)
                {
                    // Positive value calculation                       
                    if (YValues[i] >= 0)
                    {
                        if (YValues[i - 1] >= 0 || prevSegment.SegmentType == WaterfallSegmentType.Sum)
                        {
                            if (!AllowAutoSum && prevSegment.SegmentType == WaterfallSegmentType.Sum && YValues[i - 1] < 0)
                            {
                                y1 = YValues[i] + prevSegment.y2;
                                y2 = prevSegment.y2;
                            }
                            else
                            {
                                y1 = YValues[i] + prevSegment.y1;
                                y2 = prevSegment.y1;
                            }
                        }
                        else if (double.IsNaN(YValues[i - 1]))
                        {
                            y1 = YValues[i] == 0 ? prevSegment.y2
                                : prevSegment.y2 + YValues[i];
                            y2 = prevSegment.y2;
                        }
                        else
                        {
                            y1 = YValues[i] + prevSegment.y2;
                            y2 = prevSegment.y2;
                        }
                    }
                    else if (double.IsNaN(YValues[i]))
                    {
                        // Empty value calculation
                        if (YValues[i - 1] >= 0 || prevSegment.SegmentType == WaterfallSegmentType.Sum)
                            y1 = y2 = prevSegment.y1;
                        else
                            y1 = y2 = prevSegment.y2;
                    }
                    else
                    {
                        // Negative value calculation
                        if (YValues[i - 1] >= 0 || prevSegment.SegmentType == WaterfallSegmentType.Sum)
                        {
                            if (!AllowAutoSum && prevSegment.SegmentType == WaterfallSegmentType.Sum && YValues[i - 1] < 0)
                            {
                                y1 = prevSegment.y2;
                                y2 = YValues[i] + prevSegment.y2;
                            }
                            else
                            {
                                y1 = prevSegment.y1;
                                y2 = YValues[i] + prevSegment.y1;
                            }
                        }
                        else
                        {
                            y1 = prevSegment.y2;
                            y2 = YValues[i] + prevSegment.y2;
                        }
                    }
                }
            }
        }

        private void CreateSegment(SeriesView seriesView, double[] values, int index)
        {
            var segment = CreateSegment() as WaterfallSegment;

            if (segment != null)
            {
                segment.Series = this;
                segment.SeriesView = seriesView;
                segment.SetData(values);
                segment.Index = index;

                //Updating the Values for Summary Segments
                OnUpdateSumSegmentValues(segment, index);
                
                Segments.Add(segment);
            }
        }

        private void OnUpdateSumSegmentValues(WaterfallSegment segment, int index)
        {
            if ((index - 1) >= 0)
            {
                segment.PreviousWaterfallSegment = Segments[index - 1] as WaterfallSegment;
            }

            if (summaryValues != null && summaryValues.Count > index && summaryValues[index] == true)
            {
                segment.SegmentType = WaterfallSegmentType.Sum;

                if (segment.PreviousWaterfallSegment != null)
                {
                    segment.WaterfallSum = segment.PreviousWaterfallSegment.Sum;
                }
                else
                {
                    segment.WaterfallSum = YValues[index];
                }

                //Assigning the values for Summary Segment
                if (AllowAutoSum && segment.PreviousWaterfallSegment != null)
                {
                    segment.y1 = segment.WaterfallSum;
                    segment.y2 = bottomValue;
                }
                else
                {
                    if (YValues[index] >= 0)
                    {
                        segment.y1 = YValues[index];
                        segment.y2 = bottomValue;
                    }
                    else if (double.IsNaN(YValues[index]))
                    {
                        segment.Bottom = segment.Top = (float)bottomValue;
                    }
                    else
                    {
                        segment.y1 = bottomValue;
                        segment.y2 = YValues[index];
                    }
                }

                YRange += new DoubleRange(segment.y1, segment.y2);
            }
            else
            {
                if (YValues[index] < 0)
                {
                    segment.SegmentType = WaterfallSegmentType.Negative;
                }
                else
                {
                    segment.SegmentType = WaterfallSegmentType.Positive;
                }
            }

            //Sum Value Calculation
            var sum = double.NaN;
            if (AllowAutoSum == false && segment.SegmentType == WaterfallSegmentType.Sum)
                sum = YValues[index];
            else if (segment.PreviousWaterfallSegment != null && segment.SegmentType != WaterfallSegmentType.Sum) //If segment is positive or negative
                sum = YValues[index] + segment.PreviousWaterfallSegment.Sum;
            else if (segment.PreviousWaterfallSegment != null) //If segment is sum type
                sum = segment.PreviousWaterfallSegment.Sum;
            else
                sum = YValues[index];
            
            segment.Sum = sum;
        }

        private static void OnWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.UpdateSbsSeries();
            }
        }

        private static void OnAllowAutoSumChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnShowConnectorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.ScheduleUpdateChart();
            }
        }

        private static void OnSummaryBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                if (bindable is XYDataSeries xyDataSeries)
                {
                    OnYBindingPathChanged(bindable, oldValue, newValue);
                }
            }
        }

        private static void OnSummaryPointsBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnNegativePointsBrushChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

        private static void OnSpacingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as WaterfallSeries;

            if (series != null && series.ChartArea != null)
            {
                series.InvalidateSeries();
            }
        }


        private static void OnConnectorLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WaterfallSeries series)
            {
                series.OnStylePropertyChanged(oldValue as ChartLineStyle, newValue as ChartLineStyle);
            }
        }

        private void OnStylePropertyChanged(ChartLineStyle? oldValue, ChartLineStyle? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= ConnectorLineStyles_PropertyChanged;
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += ConnectorLineStyles_PropertyChanged; ;
                SetInheritedBindingContext(newValue, BindingContext);
            }

            if (AreaBounds != Rect.Zero)
            {
                InvalidateSeries();
            }
        }

        private void ConnectorLineStyles_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateSeries();
        }

        #endregion

        #endregion
    }
}
