using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ChartAxis
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(
            nameof(IsVisible),
            typeof(bool),
            typeof(ChartAxis),
            true,
            BindingMode.Default,
            null,
            OnIsVisibleChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty AxisLineOffsetProperty = BindableProperty.Create(
            nameof(AxisLineOffset),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.Default,
            null,
            OnAxisLineOffsetChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty LabelRotationProperty = BindableProperty.Create(
            nameof(LabelRotation),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.Default,
            null,
            OnLabelRotationAngleChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty LabelStyleProperty = BindableProperty.Create(
            nameof(LabelStyle),
            typeof(ChartAxisLabelStyle),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnLabelStyleChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty AxisLineStyleProperty = BindableProperty.Create(
            nameof(AxisLineStyle),
            typeof(ChartLineStyle),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnAxisLineStyleChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty CrossesAtProperty = BindableProperty.Create(
            nameof(CrossesAt),
            typeof(object),
            typeof(ChartAxis),
            double.NaN,
            BindingMode.Default,
            null,
            OnCrossesChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty RenderNextToCrossingValueProperty = BindableProperty.Create(
            nameof(RenderNextToCrossingValue),
            typeof(bool),
            typeof(ChartAxis),
            true,
            BindingMode.Default,
            null,
            OnRenderNextToCrossingValuePropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty CrossAxisNameProperty = BindableProperty.Create(
            nameof(CrossAxisName),
            typeof(string),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnCrossAxisNameChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(
            nameof(Title),
            typeof(ChartAxisTitle),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnTitleChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty IsInversedProperty = BindableProperty.Create(
            nameof(IsInversed),
            typeof(bool),
            typeof(ChartAxis),
            false,
            BindingMode.Default,
            null,
            OnInversedChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty EdgeLabelsDrawingModeProperty = BindableProperty.Create(
            nameof(EdgeLabelsDrawingMode),
            typeof(EdgeLabelsDrawingMode),
            typeof(ChartAxis),
            EdgeLabelsDrawingMode.Center,
            BindingMode.Default,
            null,
            OnEdgeLabelsDrawingModeChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty MajorGridLineStyleProperty = BindableProperty.Create(
            nameof(MajorGridLineStyle),
            typeof(ChartLineStyle),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnMajorGridLineStyleChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty MajorTickStyleProperty = BindableProperty.Create(
            nameof(MajorTickStyle),
            typeof(ChartAxisTickStyle),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnMajorTickStyleChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty ZoomPositionProperty = BindableProperty.Create(
            nameof(ZoomPosition),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.TwoWay,
            null,
            OnZoomPositionChanged);

        /// <summary>
        ///
        /// </summary>        
        public static readonly BindableProperty ZoomFactorProperty = BindableProperty.Create(
            nameof(ZoomFactor),
            typeof(double),
            typeof(ChartAxis),
            1d,
            BindingMode.TwoWay,
            null,
            OnZoomFactorChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty ShowMajorGridLinesProperty = BindableProperty.Create(
            nameof(ShowMajorGridLines),
            typeof(bool),
            typeof(ChartAxis),
            true,
            BindingMode.Default,
            null,
            OnShowMajorGridLinesChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty NameProperty = BindableProperty.Create(
            nameof(Name),
            typeof(string),
            typeof(ChartAxis),
            string.Empty,
            BindingMode.Default,
            null,
            OnNameChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty PlotOffsetStartProperty = BindableProperty.Create(
            nameof(PlotOffsetStart),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.Default,
            null,
            OnPlotOffsetStartChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty PlotOffsetEndProperty = BindableProperty.Create(
            nameof(PlotOffsetEnd),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.Default,
            null,
            OnPlotOffsetEndChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty EnableAutoIntervalOnZoomingProperty = BindableProperty.Create(
            nameof(EnableAutoIntervalOnZooming),
            typeof(bool),
            typeof(ChartAxis),
            true,
            BindingMode.Default,
            null,
            OnEnableAutoIntervalOnZoomingPropertyChanged);

        #region Internal Properties

        //Todo: Need to provide support for next release.

        /// <summary>
        /// Gets or sets an actions to be taken when two labels intersect in bounds. 
        /// </summary>
        internal static readonly BindableProperty LabelsIntersectActionProperty = BindableProperty.Create(
            nameof(LabelsIntersectAction),
            typeof(AxisLabelsIntersectAction),
            typeof(ChartAxis),
            AxisLabelsIntersectAction.Hide,
            BindingMode.Default,
            null,
            OnLabelsIntersectActionChanged);

        /// <summary>
        /// Gets or sets the enum <see cref="ChartAutoScrollingMode"/> to determine whether the axis should be auto scrolled at start or end position. 
        /// </summary>
        internal static readonly BindableProperty AutoScrollingModeProperty = BindableProperty.Create(
            nameof(AutoScrollingMode),
            typeof(ChartAutoScrollingMode),
            typeof(ChartAxis),
            ChartAutoScrollingMode.End,
            BindingMode.Default,
            null,
            OnAutoScrollingMode);

        /// <summary>
		/// Gets or sets the value that determines the distance between the axis label and axis title. 
        /// </summary>
        internal static readonly BindableProperty LabelExtentProperty = BindableProperty.Create(
            nameof(LabelExtent),
            typeof(double),
            typeof(ChartAxis),
            0d,
            BindingMode.Default,
            null,
            OnLabelExtentChanged);

        /// <summary>
		/// Gets or sets the options for customizing for the axis range. 
        /// </summary>
        internal static readonly BindableProperty RangeStylesProperty = BindableProperty.Create(
            nameof(RangeStyles),
            typeof(ChartAxisRangeStyleCollection),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnRangeStylesPropertyChanged);

        /// <summary>
		/// Gets or sets the value that determines the range of value to be visible during auto scrolling. 
        /// </summary>
        internal static readonly BindableProperty AutoScrollingDeltaProperty = BindableProperty.Create(
            nameof(AutoScrollingDelta),
            typeof(double?),
            typeof(ChartAxis),
            null,
            BindingMode.Default,
            null,
            OnAutoScrollingDeltaPropertyChanged);

        /// <summary>
        /// Gets or sets the position of the axis tick lines. 
        /// </summary>
        internal static readonly BindableProperty TickPositionProperty = BindableProperty.Create(
            nameof(TickPosition),
            typeof(AxisElementPosition),
            typeof(ChartAxis),
            AxisElementPosition.Outside,
            BindingMode.Default,
            null,
            OnTickPositionChanged);

        /// <summary>
		/// Gets or sets the value that determines the number of labels to be displayed per 100 pixels. 
        /// </summary>
        internal static readonly BindableProperty MaximumLabelsProperty = BindableProperty.Create(
            nameof(MaximumLabels),
            typeof(int),
            typeof(ChartAxis),
            3,
            BindingMode.Default,
            null,
            OnMaximumLabelsChanged);

        #endregion

        #endregion

        #region Public Properties

        /// <summary>
		/// 
        /// </summary>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double PlotOffsetStart
        {
            get { return (double)GetValue(PlotOffsetStartProperty); }
            set { SetValue(PlotOffsetStartProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double PlotOffsetEnd
        {
            get { return (double)GetValue(PlotOffsetEndProperty); }
            set { SetValue(PlotOffsetEndProperty, value); }
        }

        /// <summary>
		/// 
        /// </summary>
        public double AxisLineOffset
        {
            get { return (double)GetValue(AxisLineOffsetProperty); }
            set { SetValue(AxisLineOffsetProperty, value); }
        }

        /// <summary>
		/// 
        /// </summary>
        public double LabelRotation
        {
            get { return (double)GetValue(LabelRotationProperty); }
            set { SetValue(LabelRotationProperty, value); }
        }

        /// <summary>
		/// 
        /// </summary>
        public ChartAxisLabelStyle LabelStyle
        {
            get { return (ChartAxisLabelStyle)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
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
        ///               <chart:NumericalAxis>
        ///                   <chart:NumericalAxis.AxisLineStyle>
        ///                       <chart:ChartLineStyle StrokeWidth="2" Stroke="Red"/>
        ///                   </chart:NumericalAxis.AxisLineStyle>
        ///               </chart:NumericalAxis>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     ChartLineStyle axisLineStyle = new ChartLineStyle();
        ///     axisLineStyle.Stroke = SolidColorBrush.Red;
        ///     axisLineStyle.StrokeWidth = 2;
        ///     xAxis.AxisLineStyle = axisLineStyle;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]></code>
        /// ***
        /// </example>
        public ChartLineStyle AxisLineStyle
        {
            get { return (ChartLineStyle)GetValue(AxisLineStyleProperty); }
            set { SetValue(AxisLineStyleProperty, value); }
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
        ///               <chart:NumericalAxis CrossesAt="{Static x:Double.MaxValue}"/>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     xAxis.CrossesAt = double.MaxValue;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]></code>
        /// ***
        /// </example>
        public object CrossesAt
        {
            get { return (object)GetValue(CrossesAtProperty); }
            set { SetValue(CrossesAtProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool RenderNextToCrossingValue
        {
            get { return (bool)GetValue(RenderNextToCrossingValueProperty); }
            set { SetValue(RenderNextToCrossingValueProperty, value); }
        }

        /// <summary>
        /// 
        ///</summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis CrossesAt="5" CrossingAxisName="yAxis"/>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis Name="yAxis"/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     NumericalAxis.Name = "yAxis";
        ///     chart.YAxes.Add(yAxis);;
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     xAxis.CrossesAt = 5;
        ///     xAxis.CrossingAxisName = "yAxis";
        ///     chart.XAxes.Add(xAxis);
        ///     
        /// ]]></code>
        /// ***
        /// </example>
        public string CrossAxisName
        {
            get { return (string)GetValue(CrossAxisNameProperty); }
            set { SetValue(CrossAxisNameProperty, value); }
        }

        /// <summary>
		/// 
        /// </summary>
        public ChartAxisTitle Title
        {
            get { return (ChartAxisTitle)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsInversed
        {
            get { return (bool)GetValue(IsInversedProperty); }
            set { SetValue(IsInversedProperty, value); }
        }

        /// <summary>
		///  
        /// </summary>
        public EdgeLabelsDrawingMode EdgeLabelsDrawingMode
        {
            get { return (EdgeLabelsDrawingMode)GetValue(EdgeLabelsDrawingModeProperty); }
            set { SetValue(EdgeLabelsDrawingModeProperty, value); }
        }

        /// <summary>
		/// 
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///      
        ///          <chart:SfCartesianChart.Resources>
        ///              <DoubleCollection x:Key="dashArray">
        ///                  <x:Double>3</x:Double>
        ///                  <x:Double>3</x:Double>
        ///              </DoubleCollection>
        ///          </chart:SfCartesianChart.Resources>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis>
        ///                   <chart:NumericalAxis.MajorGridLineStyle>
        ///                       <chart:ChartLineStyle StrokeDashArray="{StaticResource dashArray}" Stroke="Black" StrokeWidth="2" />
        ///                   </chart:NumericalAxis.MajorGridLineStyle>
        ///               </chart:NumericalAxis>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(3);
        ///     doubleCollection.Add(3);
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     ChartLineStyle axisLineStyle = new ChartLineStyle();
        ///     axisLineStyle.Stroke = SolidColorBrush.Black;
        ///     axisLineStyle.StrokeWidth = 2;
        ///     axisLineStyle.StrokeDashArray = doubleCollection
        ///     xAxis.MajorGridLineStyle = axisLineStyle;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]></code>
        /// ***
        /// </example>
        public ChartLineStyle MajorGridLineStyle
        {
            get { return (ChartLineStyle)GetValue(MajorGridLineStyleProperty); }
            set { SetValue(MajorGridLineStyleProperty, value); }
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
        ///               <chart:NumericalAxis MinorTicksPerInterval="4">
        ///                   <chart:NumericalAxis.MajorTickStyle>
        ///                       <chart:ChartAxisTickStyle Stroke="Red" StrokeWidth="1"/>
        ///                   </chart:NumericalAxis.MajorTickStyle>
        ///               </chart:NumericalAxis>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     xAxis.MajorTickStyle.StrokeWidth = 1;
        ///     xAxis.MajorTickStyle.Stroke = SolidColorBrush.Red;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis)
        /// ]]></code>
        /// ***
        /// </example>
        public ChartAxisTickStyle MajorTickStyle
        {
            get { return (ChartAxisTickStyle)GetValue(MajorTickStyleProperty); }
            set { SetValue(MajorTickStyleProperty, value); }
        }

        /// <summary>
		/// 
        /// </summary>
        public double ZoomPosition
        {
            get { return (double)GetValue(ZoomPositionProperty); }
            set { SetValue(ZoomPositionProperty, value); }
        }

        /// <summary>
		/// 
        /// </summary>
        public double ZoomFactor
        {
            get { return (double)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }

        /// <summary>
		/// 
        /// </summary>
        public bool ShowMajorGridLines
        {
            get { return (bool)GetValue(ShowMajorGridLinesProperty); }
            set { SetValue(ShowMajorGridLinesProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool EnableAutoIntervalOnZooming
        {
            get { return (bool)GetValue(EnableAutoIntervalOnZoomingProperty); }
            set { SetValue(EnableAutoIntervalOnZoomingProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ChartAxisLabelEventArgs>? LabelCreated;

        /// <summary>
		/// 
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis ActualRangeChanged="xAxis_ActualRangeChanged"/>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
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
        ///     xAxis.ActualRangeChanged += xAxis_ActualRangeChanged;
        ///     
        ///     private void xAxis_ActualRangeChanged(object sender, Syncfusion.Maui.Charts.ActualRangeChangedEventArgs e)
        ///     {
        ///         e.VisibleMinimum = 2;
        ///         e.VisibleMaximum = 5;
        ///         e.ActualMinimum = 0;
        ///         e.ActualMaximum = 8;
        ///     } 
        /// ]]></code>
        /// ***
        /// </example>
        public event EventHandler<ActualRangeChangedEventArgs>? ActualRangeChanged;

        #region Internal Properties

        //Todo: Need to provide support for next release.

        /// <summary>
        /// Gets or sets an actions to be taken when two labels intersect in bounds. 
        /// </summary>
        /// <value>This property takes the <see cref="AxisLabelsIntersectAction"/> as its value.</value>
        internal AxisLabelsIntersectAction LabelsIntersectAction
        {
            get { return (AxisLabelsIntersectAction)GetValue(LabelsIntersectActionProperty); }
            set { SetValue(LabelsIntersectActionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the enum <see cref="ChartAutoScrollingMode"/> to determine whether the axis should be auto scrolled at start or end position. 
        /// </summary>
        /// <value>This property takes the <see cref="ChartAutoScrollingMode"/> as its value.</value>
        internal ChartAutoScrollingMode AutoScrollingMode
        {
            get { return (ChartAutoScrollingMode)GetValue(AutoScrollingModeProperty); }
            set { SetValue(AutoScrollingModeProperty, value); }
        }

        /// <summary>
		/// Gets or sets the value that determines the distance between the axis label and axis title. 
        /// </summary>
        /// <value>This property take double value.</value>
        /// TODO: Validating for remove support.
        internal double LabelExtent
        {
            get { return (double)GetValue(LabelExtentProperty); }
            set { SetValue(LabelExtentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the collection of the ChartAxisRangeStyle to customize the axis GridLine, TickLine and LabelStyle for specific range. 
        /// </summary>
        /// <value>This property takes the <see cref="ChartAxisRangeStyleCollection"/> as its value.</value>
        internal ChartAxisRangeStyleCollection RangeStyles
        {
            get { return (ChartAxisRangeStyleCollection)GetValue(RangeStylesProperty); }
            set { SetValue(RangeStylesProperty, value); }
        }

        /// <summary>
		/// Gets or sets the unique name of the axis, which will be used to identify the segment axis of the strip line. 
        /// </summary>
        /// <value>This property takes the string value.</value>
        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
		/// Gets or sets the value that determines the range of value to be visible during auto scrolling. 
        /// </summary>
        /// <value>This property takes the double value.</value>
        internal double? AutoScrollingDelta
        {
            get { return (double?)GetValue(AutoScrollingDeltaProperty); }
            set { SetValue(AutoScrollingDeltaProperty, value); }
        }

        /// <summary>
		/// Gets or sets the position of the axis tick lines. 
        /// </summary>
        /// <value>This property takes the <see cref="AxisElementPosition"/> as its value.</value>
        internal AxisElementPosition TickPosition
        {
            get { return (AxisElementPosition)GetValue(TickPositionProperty); }
            set { SetValue(TickPositionProperty, value); }
        }

        /// <summary>
		/// Gets or sets the value that determines the number of labels to be displayed per 100 pixels. 
        /// </summary>
        /// <value>This property takes the integer value.</value>
        /// <remarks>This property used to give constrain over the auto generated labels, which reduces the number elements rendering in view.</remarks>
        ///TODO: Provide alter way for this support. 
        internal int MaximumLabels
        {
            get { return (int)GetValue(MaximumLabelsProperty); }
            set { SetValue(MaximumLabelsProperty, value); }
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Methods
        /// <summary>
        /// Method used to get exact value in hit point.
        /// </summary>
        /// <param name="point">The point.</param>
        /// <returns>The value.</returns>
        internal float PointToValue(PointF point)
        {
            if (point == PointF.Zero)
            {
                return float.NaN;
            }

            return (float)PointToValue(point.X, point.Y);
        }

        internal virtual double CoefficientToValue(double coefficient)
        {
            double result;

            coefficient = IsInversed ? 1d - coefficient : coefficient;

            result = VisibleRange.Start + (VisibleRange.Delta * coefficient);

            return result;
        }

        internal virtual float ValueToCoefficient(double value)
        {
            double result;

            double start = VisibleRange.Start;
            double delta = VisibleRange.Delta;

            result = (value - start) / delta;

            return (float)(IsInversed ? 1f - result : result);
        }

        internal virtual void Dispose()
        {
            if (Title != null)
            {
                Title.Axis = null;
            }

            if (AxisLineStyle != null)
            {
                AxisLineStyle.PropertyChanged -= Style_PropertyChanged;
            }

            if (LabelStyle != null)
            {
                LabelStyle.PropertyChanged -= Style_PropertyChanged;
            }

            if (MajorGridLineStyle != null)
            {
                MajorGridLineStyle.PropertyChanged -= Style_PropertyChanged;
            }

            if (MajorTickStyle != null)
            {
                MajorTickStyle.PropertyChanged -= Style_PropertyChanged;
            }

            if (RegisteredSeries != null)
            {
                RegisteredSeries.Clear();
            }

            if (AssociatedAxes != null)
            {
                AssociatedAxes.Clear();
            }

            AxisLabelsRenderer = null;
            AxisElementRenderer = null;
            Area = null;
        }

        #endregion

        #region Protected Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="arrangeRect"></param>
        protected internal virtual void DrawAxis(ICanvas canvas, Rect arrangeRect)
        {
            if (AxisRenderer != null)
            {
                UpdateLayoutTranslate(canvas, AxisRenderer, arrangeRect);
                AxisRenderer.OnDraw(canvas);
            }
        }

        /// <summary>
        /// Translate the axis position to edge of series clip when axis crosses enable. 
        /// </summary>
        private void UpdateLayoutTranslate(ICanvas canvas, CartesianAxisRenderer axisRenderer, Rect arrangeRect)
        {
            var axisArrangeRect = new Rect(arrangeRect.Location, arrangeRect.Size);
            if (CanRenderNextToCrossingValue() && cartesianArea != null)
            {
                Rect clip = cartesianArea.ActualSeriesClipRect;

                if (IsVertical)
                {
                    if (axisArrangeRect.Left < clip.Left)
                    {
                        axisArrangeRect.Left = clip.Left;
                    }

                    if (axisArrangeRect.Right > clip.Right)
                    {
                        axisArrangeRect.Left = clip.Right - arrangeRect.Width;
                    }
                }
                else
                {
                    if (axisArrangeRect.Top < clip.Top)
                    {
                        axisArrangeRect.Top = clip.Top;
                    }

                    if (axisArrangeRect.Bottom > clip.Bottom)
                    {
                        axisArrangeRect.Top = clip.Bottom - arrangeRect.Height;
                    }
                }
            }

            if (axisArrangeRect != arrangeRect)
            {
                //Hide line and ticks when axis tries to render out side of series clip.
                axisRenderer.UpdateRendererVisible(false);
                canvas.Translate((float)axisArrangeRect.Left, (float)axisArrangeRect.Top);
            }
            else
            {
                axisRenderer.UpdateRendererVisible(true);
                canvas.Translate((float)arrangeRect.Left, (float)arrangeRect.Top);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawMajorTick(ICanvas canvas, double tickPosition, PointF point1, PointF point2)
        {
            canvas.DrawLine(point1, point2);
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawMinorTick(ICanvas canvas, double tickPosition, PointF point1, PointF point2)
        {
            canvas.DrawLine(point1, point2);
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawAxisLine(ICanvas canvas, float x1, float y1, float x2, float y2)
        {
            canvas.DrawLine(x1, y1, x2, y2);
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void DrawGridLine(ICanvas canvas, double position, float x1, float y1, float x2, float y2)
        {
            canvas.DrawLine(x1, y1, x2, y2);
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void OnLabelCreated(ChartAxisLabel label)
        {

        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (AxisLineStyle != null)
            {
                SetInheritedBindingContext(AxisLineStyle, BindingContext);
            }

            if (MajorGridLineStyle != null)
            {
                SetInheritedBindingContext(MajorGridLineStyle, BindingContext);
            }

            if (MajorTickStyle != null)
            {
                SetInheritedBindingContext(MajorTickStyle, BindingContext);
            }

            if (LabelStyle != null)
            {
                SetInheritedBindingContext(LabelStyle, BindingContext);
            }

            if (Title != null)
            {
                SetInheritedBindingContext(Title, BindingContext);
            }
        }

    #endregion

        #region Private Methods

        private static void OnShowMajorGridLinesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnMajorTickStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis == null)
            {
                return;
            }

            if (oldValue != null)
            {
                var style = newValue as ChartAxisTickStyle;
                if (style != null)
                {
                    style.PropertyChanged -= axis.Style_PropertyChanged;
                }
            }

            if (newValue != null)
            {
                var tickStyle = newValue as ChartAxisTickStyle;
                if (tickStyle != null)
                {
                    SetInheritedBindingContext(tickStyle, axis.BindingContext);
                    tickStyle.PropertyChanged += axis.Style_PropertyChanged; ;
                }
            }

            axis?.UpdateLayout();
        }

        private static void OnMajorGridLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if(axis == null)
            {
                return;
            }

            if (oldValue != null)
            {
                var style = newValue as ChartLineStyle;
                if (style != null)
                {
                    style.PropertyChanged -= axis.Style_PropertyChanged;
                }
            }

            if (newValue != null)
            {
                var tickStyle = newValue as ChartLineStyle;
                if (tickStyle != null)
                {
                    SetInheritedBindingContext(tickStyle, axis.BindingContext);
                    tickStyle.PropertyChanged += axis.Style_PropertyChanged;
                }
            }

            axis?.UpdateLayout();
        }

        private static void OnEdgeLabelsDrawingModeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnInversedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                var title = newValue as ChartAxisTitle;
                if (title != null)
                {
                    SetInheritedBindingContext(title, axis.BindingContext);
                    axis.InitTitle(title);
                }
            }
        }

        private static void OnCrossesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null && newValue != null)
            {
                axis.ActualCrossingValue = ChartUtils.ConvertToDouble(newValue);
                axis.UpdateLayout();
            }
        }

        private static void OnRenderNextToCrossingValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnCrossAxisNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnAxisLineStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis == null)
            {
                return;
            }

            if (oldValue != null)
            {
                var style = newValue as ChartLineStyle;
                if (style != null)
                {
                    style.PropertyChanged -= axis.Style_PropertyChanged;
                }
            }

            if (newValue != null)
            {
                var lineStyle = newValue as ChartLineStyle;
                if (lineStyle != null)
                {
                    SetInheritedBindingContext(lineStyle, axis.BindingContext);
                    lineStyle.PropertyChanged += axis.Style_PropertyChanged;
                }
            }

            axis?.UpdateLayout();
        }

        private static void OnRangeStylesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if(oldValue != null)
            {
                //TODO: Unhook collection changed
            }

            if (newValue != null)
            {
                //TODO: Hook collection changed.
            }

            (bindable as ChartAxis)?.UpdateLayout();
        }

        private static void OnLabelStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis == null)
            {
                return;
            }

            if (oldValue != null)
            {
                var style = newValue as ChartAxisLabelStyle;
                if (style != null)
                {
                    style.PropertyChanged -= axis.Style_PropertyChanged;
                }
            }

            if (newValue != null && axis != null)
            {
                var style = newValue as ChartAxisLabelStyle;
                if (style != null)
                {
                    SetInheritedBindingContext(style, axis.BindingContext);
                    style.PropertyChanged += axis.Style_PropertyChanged;
                }
            }

            axis?.UpdateLayout();
        }

        private static void OnLabelRotationAngleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnLabelExtentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnAutoScrollingMode(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnLabelsIntersectActionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnAxisLineOffsetChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnPlotOffsetStartChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            var plotOffset = (double)newValue;
            if (axis != null)
            {
                axis.ActualPlotOffsetStart = (float)(double.IsNaN(plotOffset) ? 0 : plotOffset);
                axis.UpdateLayout();
            }
        }

        private static void OnPlotOffsetEndChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            var plotOffset = (double)newValue;
            if (axis != null)
            {
                axis.ActualPlotOffsetEnd = (float)(double.IsNaN(plotOffset) ? 0 : plotOffset);
                axis.UpdateLayout();
            }
        }

        private static void OnIsVisibleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnMaximumLabelsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnTickPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as ChartAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnAutoScrollingDeltaPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnEnableAutoIntervalOnZoomingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnZoomFactorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //TODO: zoom factor need to set between 0 to 1
            var axis = bindable as ChartAxis;
            if (axis != null)
            {

                var oldFactor = oldValue == null ? 1 : (double)oldValue;
                var newFactor = newValue == null ? 1 : (double)newValue > 1 ? 1 : (double)newValue < 0 ? 0 : (double)newValue;

                if (oldFactor != newFactor)
                {
                    //TODO: Set new factor to actual value;
                    axis.UpdateLayout();
                }
            }
        }

        private static void OnZoomPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //TODO: Zoompostion need to set between 0 to 1
            var axis = bindable as ChartAxis;
            if (axis != null)
            {

                var oldPosition = oldValue == null ? 1 : (double)oldValue;
                var newPosition = newValue == null ? 1 : (double)newValue > 1 ? 1 : (double)newValue < 0 ? 0 : (double)newValue;

                if (oldPosition != newPosition)
                {
                    //TODO: Set new position to actual value;
                    axis.UpdateLayout();
                }
            }
        }

        private void Style_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.UpdateLayout();
        }

        #endregion

        #endregion
    }
}
