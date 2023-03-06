using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Windows.Data.Xml.Dom;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The ChartAxis is the base class for all types of axes.
    /// </summary>
    /// <remarks>
    /// <para>The ChartAxis is used to locate a data point inside the chart area. Charts typically have two axes that are used to measure and categorize data.</para>
    /// <para>The vertical(y) axis always uses numerical scale.</para>
    /// <para>The horizontal(x) axis supports the Category, Numeric and Date-time.</para>  
    /// </remarks>
	public partial class ChartAxis
	{
		#region Dependency Property Registration

		/// <summary>
		/// The DependencyProperty for <see cref="PlotOffsetStart"/> property.
		/// </summary>
		public static readonly DependencyProperty PlotOffsetStartProperty =
			DependencyProperty.Register(
				nameof(PlotOffsetStart),
				typeof(double),
				typeof(ChartAxis),
				new PropertyMetadata(0d, OnPlotOffsetStartChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="PlotOffsetEnd"/> property.
		/// </summary>
		public static readonly DependencyProperty PlotOffsetEndProperty =
			DependencyProperty.Register(
				nameof(PlotOffsetEnd),
				typeof(double),
				typeof(ChartAxis),
				new PropertyMetadata(0d, OnPlotOffsetEndChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="AxisLineOffset"/> property.
		/// </summary>
		public static readonly DependencyProperty AxisLineOffsetProperty =
			DependencyProperty.Register(
				nameof(AxisLineOffset),
				typeof(double),
				typeof(ChartAxis),
				new PropertyMetadata(0d, OnPropertyChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="LabelsIntersectAction"/> property.
		/// </summary>
		public static readonly DependencyProperty LabelsIntersectActionProperty =
			DependencyProperty.Register(
				nameof(LabelsIntersectAction),
				typeof(AxisLabelsIntersectAction),
				typeof(ChartAxis),
				new PropertyMetadata(AxisLabelsIntersectAction.Hide, OnPropertyChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="LabelExtent"/> property.
		/// </summary>
		public static readonly DependencyProperty LabelExtentProperty =
			DependencyProperty.Register(
				nameof(LabelExtent),
				typeof(double),
				typeof(ChartAxis),
				new PropertyMetadata(0.0, OnPropertyChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="LabelRotation"/> property.
		/// </summary>
		public static readonly DependencyProperty LabelRotationProperty =
			DependencyProperty.Register(
				nameof(LabelRotation),
				typeof(double),
				typeof(ChartAxis),
				new PropertyMetadata(0d, new PropertyChangedCallback(OnLabelRotationChanged)));

		/// <summary>
		///The DependencyProperty for <see cref="AxisLineStyle"/> property.
		/// </summary>
		public static readonly DependencyProperty AxisLineStyleProperty =
			DependencyProperty.Register(
				nameof(AxisLineStyle),
				typeof(Style),
				typeof(ChartAxis),
				null);

		/// <summary>
		/// The DependencyProperty for <see cref="OpposedPosition"/> property.
		/// </summary>
		public static readonly DependencyProperty OpposedPositionProperty =
			DependencyProperty.Register(
				nameof(OpposedPosition),
				typeof(bool),
				typeof(ChartAxis),
				new PropertyMetadata(false, OnOpposedPositionChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="Header"/> property.
		/// </summary>
		public static readonly DependencyProperty HeaderProperty =
			DependencyProperty.Register(
				nameof(Header),
				typeof(object),
				typeof(ChartAxis),
				new PropertyMetadata(null, OnPropertyChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="HeaderStyle"/> property.
		/// </summary>
		public static readonly DependencyProperty HeaderStyleProperty =
			DependencyProperty.Register(
				nameof(HeaderStyle),
				typeof(LabelStyle),
				typeof(ChartAxis),
				new PropertyMetadata(null, OnHeaderStyleChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="HeaderTemplate"/> property.
		/// </summary>
		public static readonly DependencyProperty HeaderTemplateProperty =
			DependencyProperty.Register(
				nameof(HeaderTemplate),
				typeof(DataTemplate),
				typeof(ChartAxis),
				new PropertyMetadata(null, OnPropertyChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="TickLineSize"/> property.
		/// </summary>
		public static readonly DependencyProperty TickLineSizeProperty =
			DependencyProperty.Register(
				nameof(TickLineSize),
				typeof(double),
				typeof(ChartAxis),
				new PropertyMetadata(8d, OnPropertyChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="IsInversed"/> property.
		/// </summary>
		public static readonly DependencyProperty IsInversedProperty =
			DependencyProperty.Register(
				nameof(IsInversed),
				typeof(bool),
				typeof(ChartAxis),
				new PropertyMetadata(false, OnIsInversedChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="EdgeLabelsDrawingMode"/> property.
		/// </summary>
		public static readonly DependencyProperty EdgeLabelsDrawingModeProperty =
			DependencyProperty.Register(
				nameof(EdgeLabelsDrawingMode),
				typeof(EdgeLabelsDrawingMode),
				typeof(ChartAxis),
				new PropertyMetadata(EdgeLabelsDrawingMode.Center, OnPropertyChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="MajorGridLineStyle"/> property.
		/// </summary>
		public static readonly DependencyProperty MajorGridLineStyleProperty =
			DependencyProperty.Register(nameof(MajorGridLineStyle), typeof(Style), typeof(ChartAxis), new PropertyMetadata(null, OnMajorGridLineStyleChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="MajorTickStyle"/> property.
		/// </summary>
		public static readonly DependencyProperty MajorTickStyleProperty =
			DependencyProperty.Register(
				nameof(MajorTickStyle),
				typeof(Style),
				typeof(ChartAxis),
				new PropertyMetadata(null, OnMajorTickStyleChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="ShowTrackballLabel"/> property.
		/// </summary>
		public static readonly DependencyProperty ShowTrackballLabelProperty =
			DependencyProperty.Register(
				nameof(ShowTrackballLabel),
				typeof(bool),
				typeof(ChartAxis),
				new PropertyMetadata(false));

		/// <summary>
		///The DependencyProperty for <see cref="TrackballLabelTemplate"/> property.
		/// </summary>
		public static readonly DependencyProperty TrackballLabelTemplateProperty =
			DependencyProperty.Register(
				nameof(TrackballLabelTemplate),
				typeof(DataTemplate),
				typeof(ChartAxis),
				null);

		/// <summary>
		/// The DependencyProperty for <see cref="CrosshairLabelTemplate"/> property.
		/// </summary>
		public static readonly DependencyProperty CrosshairLabelTemplateProperty =
			DependencyProperty.Register(
				nameof(CrosshairLabelTemplate),
				typeof(DataTemplate),
				typeof(ChartAxis),
				null);

		/// <summary>
		/// The DependencyProperty for <see cref="ShowMajorGridLines"/> property.
		/// </summary>
		public static readonly DependencyProperty ShowMajorGridLinesProperty =
			DependencyProperty.Register(
				nameof(ShowMajorGridLines),
				typeof(bool),
				typeof(ChartAxis),
				new PropertyMetadata(true, new PropertyChangedCallback(OnShowMajorGridLinesChanged)));

		/// <summary>
		/// The DependencyProperty for <see cref="LabelStyle"/> property.
		/// </summary>
		public static readonly DependencyProperty LabelStyleProperty =
			DependencyProperty.Register(
				nameof(LabelStyle),
				typeof(LabelStyle),
				typeof(ChartAxis),
				new PropertyMetadata(null, OnPropertyChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="LabelTemplate"/> property.
		/// </summary>
		public static readonly DependencyProperty LabelTemplateProperty =
			DependencyProperty.Register(
				nameof(LabelTemplate),
				typeof(DataTemplate),
				typeof(ChartAxis),
				new PropertyMetadata(null, new PropertyChangedCallback(OnLabelTemplateChanged)));

		/// <summary>
		/// The DependencyProperty for <see cref="EnableAutoIntervalOnZooming"/> property.
		/// </summary>
		public static readonly DependencyProperty EnableAutoIntervalOnZoomingProperty =
			DependencyProperty.Register(
				nameof(EnableAutoIntervalOnZooming),
				typeof(bool),
				typeof(ChartAxis),
				new PropertyMetadata(true));

        /// <summary>
        /// The DependencyProperty for <see cref="AutoScrollingMode"/> property.. 
        /// </summary>
        internal static readonly DependencyProperty AutoScrollingModeProperty =
			DependencyProperty.Register(
				nameof(AutoScrollingMode),
				typeof(ChartAutoScrollingMode),
				typeof(ChartAxis),
				new PropertyMetadata(ChartAutoScrollingMode.End, OnAutoScrollingModeChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="AutoScrollingDelta"/> property. 
        /// </summary>
        internal static readonly DependencyProperty AutoScrollingDeltaProperty =
			DependencyProperty.Register(
				nameof(AutoScrollingDelta),
				typeof(double),
				typeof(ChartAxis), new PropertyMetadata(double.NaN, OnAutoScrollingDeltaChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="AxisVisibility"/> property.
		/// </summary>
		internal static readonly DependencyProperty AxisVisibilityProperty =
			DependencyProperty.Register(
				nameof(AxisVisibility),
				typeof(Visibility),
				typeof(ChartAxis),
				new PropertyMetadata(Visibility.Visible, OnPropertyChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="ZoomPosition"/> property.
		/// </summary>
		public static readonly DependencyProperty ZoomPositionProperty =
			DependencyProperty.Register(
				nameof(ZoomPosition),
				typeof(double),
				typeof(ChartAxis),
				new PropertyMetadata(0d, OnZoomPositionChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="ZoomFactor"/> property.
		/// </summary>
		public static readonly DependencyProperty ZoomFactorProperty =
			DependencyProperty.Register(
				nameof(ZoomFactor),
				typeof(double),
				typeof(ChartAxis),
				new PropertyMetadata(1d, OnZoomFactorChanged));

		#endregion

		#region Fields

		#region Internal Fields

		internal bool axisElementsUpdateRequired = false;

		internal ILayoutCalculator? AxisLayoutPanel;

		internal ILayoutCalculator? axisLabelsPanel;

		internal ILayoutCalculator? axisElementsPanel;

		internal CompositorLinesPanel? MajorGridLinesPanel;

		internal CompositorLinesPanel? MinorGridLinesPanel;

		internal ContentControl? headerContent;

		internal ChartCartesianAxisPanel? axisPanel;

		#endregion

		#region Private Fields

		private Panel? labelsPanel;

		private Panel? elementsPanel;

		private int maximumLabels = 3;

		#endregion

		#endregion

		#region Events

		#region Public Events

        /// <summary>
        /// This event occurs when the actual range is changed.
        /// </summary>
        /// <remarks>
        /// The <see cref="ActualRangeChangedEventArgs"/> contains information on the chart axis' minimum and maximum values.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-47)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis ActualRangeChanged="OnActualRangeChanged" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-48)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis();
        /// xaxis.ActualRangeChanged += OnActualRangeChanged;
        /// chart.XAxes.Add(xaxis);
        /// 
        /// private void OnActualRangeChanged(object sender, ActualRangeChangedEventArgs e)
        /// {
        ///     var minimumValue = e.ActualMinimum;
        ///     var maximumValue = e.ActualMaximum;
        /// }
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public event EventHandler<ActualRangeChangedEventArgs> ActualRangeChanged;

        /// <summary>
        /// This event occurs when the axis label is created.
        /// </summary>
        /// <remarks>The <see cref="ChartAxisLabelEventArgs"/> contains the information of AxisLabel.</remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-49)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis LabelCreated="OnLabelCreated" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-50)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis();
        /// xaxis.LabelCreated += OnLabelCreated;
        /// chart.XAxes.Add(xaxis);
        /// 
        /// private void OnLabelCreated(object sender, ChartAxisLabelEventArgs e)
        /// {
        ///      // You can customize the content of the axis label.
        /// }
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public event EventHandler<ChartAxisLabelEventArgs> LabelCreated;

		#endregion

		#region Internal Events

		internal event EventHandler<VisibleRangeChangedEventArgs>? VisibleRangeChanged;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        internal int MaximumLabels
		{
			get { return maximumLabels; }
			set { maximumLabels = value; }
		}

        /// <summary>
        /// Gets or sets a value to provide padding to the axis at the start position.
        /// </summary>
        /// <value>It accepts <c>double</c> values and its default value is 0.</value>
        /// <remarks>
        /// <para><see cref="PlotOffsetStart"/> applies padding at the start of a plot area where the axis and its elements are rendered in a chart with padding at the start.</para>
        /// <para><see cref="PlotOffsetStart"/> is not applicable for the polar and radar chart series.</para>
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis PlotOffsetStart = "30" />
        ///     </chart:SfCartesianChart.XAxes>
        ///
        ///     <chart:SfCartesianChart.YAxes>
        ///         <chart:NumericalAxis PlotOffsetStart = "30" />
        ///     </chart:SfCartesianChart.YAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    PlotOffsetStart = 30
        /// };
        /// 
        /// NumericalAxis yaxis = new NumericalAxis()
        /// {
        ///    PlotOffsetStart = 30
        /// };
        ///
        /// chart.XAxes.Add(xaxis);
        /// chart.YAxes.Add(yaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        /// <see cref="PlotOffsetEnd"/>
		public double PlotOffsetStart
		{
			get { return (double)GetValue(PlotOffsetStartProperty); }
			set { SetValue(PlotOffsetStartProperty, value); }
		}

        /// <summary>
        /// Gets or sets a value to provide padding to the axis at end position.
        /// </summary>
        /// <value>It accepts <c>double</c> values and its default value is 0.</value>
        /// <remarks>
        /// <para><see cref="PlotOffsetEnd"/> applies padding at end of the plot area where the axis and its elements are rendered in the chart with padding at the end.</para>
        /// <para><see cref="ChartAxis.PlotOffsetEnd"/> is not applicable for polar and radar chart series.</para>
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis PlotOffsetEnd = "30" />
        ///     </chart:SfCartesianChart.XAxes>
        ///
        ///     <chart:SfCartesianChart.YAxes>
        ///         <chart:NumericalAxis PlotOffsetEnd = "30" />
        ///     </chart:SfCartesianChart.YAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    PlotOffsetEnd = 30
        /// };
        /// 
        /// NumericalAxis yaxis = new NumericalAxis()
        /// {
        ///    PlotOffsetEnd = 30
        /// };
        ///
        /// chart.XAxes.Add(xaxis);
        /// chart.YAxes.Add(yaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        /// <see cref="PlotOffsetStart"/>
		public double PlotOffsetEnd
		{
			get { return (double)GetValue(PlotOffsetEndProperty); }
			set { SetValue(PlotOffsetEndProperty, value); }
		}

        /// <summary>
        /// Gets or sets a value to provide padding to the axis line.
        /// </summary>
        /// <value>It accepts <c>double</c> values and its default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis AxisLineOffset = "30" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    AxisLineOffset = 30
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public double AxisLineOffset
		{
			get { return (double)GetValue(AxisLineOffsetProperty); }
			set { SetValue(AxisLineOffsetProperty, value); }
		}

        /// <summary>
        /// Gets or sets the value for the rotation angle of the axis labels.
        /// </summary>
        /// <value>It accepts the <c>double</c> values and the default value is 0.</value>
        /// <remarks>
        /// The label will be rotated with the center as the origin.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis LabelRotation = "90" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    LabelRotation = 90,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public double LabelRotation
		{
			get { return (double)GetValue(LabelRotationProperty); }
			set { SetValue(LabelRotationProperty, value); }
		}

        /// <summary>
        /// Gets or sets the value to customize the appearance of the chart axis line.
        /// </summary>
        /// <value>This property accepts the <see cref="Style"/> value.</value>
        /// <remarks>
        /// To customize the axis line appearance, you need to create an instance of the <see cref="Style"/> class and set to the <see cref="AxisLineStyle"/> property.
        /// </remarks>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis>
        ///                 <chart:NumericalAxis.AxisLineStyle>
        ///                     <Style TargetType = "Line" >
        ///                         <Setter Property="StrokeThickness" Value="2"/>
        ///                         <Setter Property = "Stroke" Value="Red" />
        ///                     </Style>
        ///                 </chart:NumericalAxis.AxisLineStyle>
        ///             </chart:NumericalAxis>
        ///         </chart:SfCartesianChart.XAxes>
        ///
        ///         <chart:SfCartesianChart.YAxes>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-10)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     Style axisLineStyle = new Style() { TargetType = typeof(Line), };
        ///     axisLineStyle.Setters.Add(new Setter(Path.StrokeThicknessProperty, 2));
        ///     axisLineStyle.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Red)));
        ///     xAxis.AxisLineStyle = axisLineStyle;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
		public Style AxisLineStyle
		{
			get { return (Style)GetValue(AxisLineStyleProperty); }
			set { SetValue(AxisLineStyleProperty, value); }
		}

        /// <summary>
        /// Gets or sets the value to customize the appearance of chart axis labels. 
        /// </summary>
        /// <value>It accepts the <see cref="Charts.LabelStyle"/> value.</value>
        /// <remarks>
        /// To customize the axis labels appearance, you need to create an instance of the <see cref="Charts.LabelStyle"/> class and set to the <see cref="LabelStyle"/> property.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-11)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis>
        ///            <chart:CategoryAxis.LabelStyle>
        ///                <chart:LabelStyle Foreground = "Red" FontSize="14"/>
        ///            </chart:CategoryAxis.LabelStyle>
        ///        </chart:CategoryAxis>
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-12)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis();
        /// xaxis.LabelStyle = new LabelStyle()
        /// {
        ///     Foreground = new SolidColorBrush(Colors.Red),
        ///     FontSize = 14,
        /// };
        /// chart.XAxes.Add(xaxis);
        ///
        /// ]]>
        /// </code>
        /// *** 
        /// </example> 
		public LabelStyle LabelStyle
		{
			get { return (LabelStyle)GetValue(LabelStyleProperty); }
			set { SetValue(LabelStyleProperty, value); }
		}

        /// <summary>
        /// Gets or sets a value for the chart axis header.
        /// </summary>
        /// <value>It accepts any <see cref="UIElement"/> as content.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-13)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis Header="XValue" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-14)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    Header="XValue",
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public object Header
		{
			get { return (object)GetValue(HeaderProperty); }
			set { SetValue(HeaderProperty, value); }
		}

        /// <summary>
        /// Gets or sets the label style to customize the appearance of the chart axis header.
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-15)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis Header="XValue">
        ///                 <chart:NumericalAxis.HeaderStyle>
        ///                     <chart:LabelStyle Foreground = "Red"/>
        ///                 </chart:NumericalAxis.HeaderStyle>
        ///             </chart:NumericalAxis>
        ///         </chart:SfCartesianChart.XAxes>
        ///
        ///         <chart:SfCartesianChart.YAxes>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-16)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///
        ///     NumericalAxis xAxis = new NumericalAxis() { Header = "XValue" };
        ///     LabelStyle headerStyle = new LabelStyle() { Foreground = new SolidColorBrush(Colors.Red) };
        ///     xAxis.HeaderStyle = headerStyle;
        ///     chart.XAxes.Add(xAxis);
        ///
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        /// <value>It accepts the <see cref="Charts.LabelStyle"/> value.</value>
		public LabelStyle HeaderStyle
		{
			get { return (LabelStyle)GetValue(HeaderStyleProperty); }
			set { SetValue(HeaderStyleProperty, value); }
		}

        /// <summary>
        /// Gets or sets the custom template to customize the appearance of the chart axis header.
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-17)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis Header="XValue">
        ///                 <chart:NumericalAxis.HeaderTemplate>
        ///                     <DataTemplate>
        ///                         <Border Background = "LightGreen" >
        ///                             <TextBlock Text="{Binding}" Margin="5" />
        ///                         </Border>
        ///                     </DataTemplate>
        ///                 </chart:NumericalAxis.HeaderTemplate>
        ///             </chart:NumericalAxis>
        ///         </chart:SfCartesianChart.XAxes>
        ///
        ///         <chart:SfCartesianChart.YAxes>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        /// <value>It accepts the <see cref="DataTemplate"/> value.</value>
		public DataTemplate HeaderTemplate
		{
			get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
			set { SetValue(HeaderTemplateProperty, value); }
		}

        /// <summary>
        /// Gets or sets the value that indicates whether the axis's visible range is inversed.
        /// </summary>
        /// <value>It accepts the bool values and its default value is <c>False</c>.</value>
        /// <remarks>When the axis is inversed, it will render points from right to left for the horizontal axis, and top to bottom for the vertical axis.</remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-18)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis IsInversed ="True" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-19)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    IsInversed = true,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public bool IsInversed
		{
			get { return (bool)GetValue(IsInversedProperty); }
			set { SetValue(IsInversedProperty, value); }
		}

        /// <summary>
        ///  Gets or sets a value to customize the rendering position of the edge labels. 
        /// </summary>
        /// <value>It accepts the <see cref="Charts.EdgeLabelsDrawingMode"/> value and its default value is <see cref="EdgeLabelsDrawingMode.Center"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-20)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis EdgeLabelsDrawingMode="Fit" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-21)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    EdgeLabelsDrawingMode= EdgeLabelsDrawingMode.Fit,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public EdgeLabelsDrawingMode EdgeLabelsDrawingMode
		{
			get { return (EdgeLabelsDrawingMode)GetValue(EdgeLabelsDrawingModeProperty); }
			set { SetValue(EdgeLabelsDrawingModeProperty, value); }
		}

        /// <summary>
        /// Gets or sets the style to customize the appearance of the major grid lines.
        /// </summary>
        /// <value>It accepts the <see cref="Style"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-22)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis>
        ///                 <chart:NumericalAxis.MajorGridLineStyle>
        ///                     <Style TargetType = "Line" >
        ///                         <Setter Property="StrokeThickness" Value="2"/>
        ///                         <Setter Property = "Stroke" Value="Red" />
        ///                     </Style>
        ///                 </chart:NumericalAxis.MajorGridLineStyle>
        ///             </chart:NumericalAxis>
        ///         </chart:SfCartesianChart.XAxes>
        ///
        ///         <chart:SfCartesianChart.YAxes>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-23)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     Style majorGridLineStyle = new Style() { TargetType = typeof(Line), };
        ///     majorGridLineStyle.Setters.Add(new Setter(Path.StrokeThicknessProperty, 2));
        ///     majorGridLineStyle.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Red)));
        ///     xAxis.MajorGridLineStyle = majorGridLineStyle;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
		public Style MajorGridLineStyle
		{
			get { return (Style)GetValue(MajorGridLineStyleProperty); }
			set { SetValue(MajorGridLineStyleProperty, value); }
		}

        /// <summary>
        /// Gets or sets the style to customize the appearance of the major tick lines.
        /// </summary>
        /// <value>It accepts the <see cref="Style"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-24)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis>
        ///                 <chart:NumericalAxis.MajorTickStyle>
        ///                     <Style TargetType = "Line" >
        ///                         <Setter Property="StrokeThickness" Value="2"/>
        ///                         <Setter Property = "Stroke" Value="Red" />
        ///                     </Style>
        ///                 </chart:NumericalAxis.MajorTickStyle>
        ///             </chart:NumericalAxis>
        ///         </chart:SfCartesianChart.XAxes>
        ///
        ///         <chart:SfCartesianChart.YAxes>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfCartesianChart.YAxes>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-25)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     Style majorTickStyle = new Style() { TargetType = typeof(Line), };
        ///     majorTickStyle.Setters.Add(new Setter(Path.StrokeThicknessProperty, 2));
        ///     majorTickLStyle.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Red)));
        ///     xAxis.MajorTickStyle = majorTickStyle;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
		public Style MajorTickStyle
		{
			get { return (Style)GetValue(MajorTickStyleProperty); }
			set { SetValue(MajorTickStyleProperty, value); }
		}

        /// <summary>
        /// Gets or sets the value that defines the zoom position for the actual range of the axis.
        /// </summary>
        /// <remarks> The value must be between 0 and 1.</remarks>
        /// <value>It accepts the double values and its default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-26)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:CategoryAxis ZoomFactor="0.3" ZoomPosition="0.5" />
        ///         </chart:SfCartesianChart.XAxes>
        ///
        ///         <chart:SfCartesianChart.ZoomPanBehavior>
        ///              <chart:ChartZoomPanBehavior/>
        ///         </chart:SfCartesianChart.ZoomPanBehavior>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-27)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis(){  ZoomFactor = 0.3, ZoomPosition = 0.5  };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// chart.ZoomPanBehavior = new ChartZoomPanBehavior();
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public double ZoomPosition
		{
			get { return (double)GetValue(ZoomPositionProperty); }
			set { SetValue(ZoomPositionProperty, value); }
		}

        /// <summary>
        /// Gets or sets the value that defines the percentage of the visible range from the total range of axis values.
        /// </summary>
        /// <remarks>The value must be between 0 and 1.</remarks>
        /// <value>It accepts the double values and its default value is 1.</value> 
        /// <summary>
        /// Gets or sets the value that defines the position for the actual range of the axis.
        /// </summary>
        /// <remarks> The value must be between 0 and 1.</remarks>
        /// <value>It accepts the double values and its default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-28)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:CategoryAxis ZoomFactor="0.3" ZoomPosition="0.5" />
        ///         </chart:SfCartesianChart.XAxes>
        ///         
        ///         <chart:SfCartesianChart.ZoomPanBehavior>
        ///              <chart:ChartZoomPanBehavior/>
        ///         </chart:SfCartesianChart.ZoomPanBehavior>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-29)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis(){  ZoomFactor = 0.3, ZoomPosition = 0.5  };
        /// chart.XAxes.Add(xaxis);	
        /// 
        /// ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior();
        /// chart.ZoomPanBehavior = zoomPanBehavior;
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public double ZoomFactor
		{
			get { return (double)GetValue(ZoomFactorProperty); }
			set { SetValue(ZoomFactorProperty, value); }
		}

        /// <summary>
        /// Gets or sets a value indicating whether the axis grid lines can be displayed or not.
        /// </summary>
        /// <value>It accepts the bool value and its default value is <c>True</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-30)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis ShowMajorGridLines = "False" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-31)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    ShowMajorGridLines = false,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public bool ShowMajorGridLines
		{
			get { return (bool)GetValue(ShowMajorGridLinesProperty); }
			set { SetValue(ShowMajorGridLinesProperty, value); }
		}

        /// <summary>
        /// Gets or sets a value indicating whether to calculate the axis intervals on zooming. 
        /// </summary>
        /// <value>It accepts the bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-32)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis EnableAutoIntervalOnZooming = "False" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-33)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    EnableAutoIntervalOnZooming = false,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public bool EnableAutoIntervalOnZooming
		{
			get { return (bool)GetValue(EnableAutoIntervalOnZoomingProperty); }
			set { SetValue(EnableAutoIntervalOnZoomingProperty, value); }
		}

        /// <summary>
        /// Gets or sets a value indicating the actions to be taken when two labels intersect.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="AxisLabelsIntersectAction"/> values and the default value is <c>Hide</c>.
        /// </value>
        /// <remarks>
        /// Overlapping labels can be hidden, rotated, or moved to the next row.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-34)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis LabelsIntersectAction="Auto" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-35)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    LabelsIntersectAction = AxisLabelsIntersectAction.Auto,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example> 
		public AxisLabelsIntersectAction LabelsIntersectAction
		{
			get { return (AxisLabelsIntersectAction)GetValue(LabelsIntersectActionProperty); }
			set { SetValue(LabelsIntersectActionProperty, value); }
		}

        /// <summary>
        /// Gets or sets the value that determines the distance between the axis label and axis header.
        /// </summary>
        /// <value>It accepts the <c>double</c> values and the default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-36)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis Header="XValue" LabelExtent="50" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-37)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    Header="XValue",
        ///    LabelExtent = 50
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example> 
		public double LabelExtent
		{
			get { return (double)GetValue(LabelExtentProperty); }
			set { SetValue(LabelExtentProperty, value); }
		}

        /// <summary>
        /// Gets or sets a value indicating whether to enable the axis to a position opposite its actual position.
        /// </summary>
        /// <remarks>That is, the other side of plot area.</remarks>
        /// <value>It accepts bool values and the default vaue is <c>False</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-38)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis OpposedPosition="True" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-39)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    OpposedPosition = true,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public bool OpposedPosition
		{
			get { return (bool)GetValue(OpposedPositionProperty); }
			set { SetValue(OpposedPositionProperty, value); }
		}

        /// <summary>
        /// Gets or sets a value to modify the axis tick line size.
        /// </summary>
        /// <value>It accepts <c>double</c> values and its default value is 8.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-40)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis TickLineSize="15" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-41)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    TickLineSize = 15,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public double TickLineSize
		{
			get { return (double)GetValue(TickLineSizeProperty); }
			set { SetValue(TickLineSizeProperty, value); }
		}

        /// <summary>
        /// Gets or sets the value that indicates whether to show the axis information when the trackball is shown.
        /// </summary>
        /// <value>It accepts the bool value and its default value is <c>False</c>.</value> 
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-42)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis ShowTrackballLabel = "True" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-43)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis()
        /// {
        ///    ShowTrackballLabel = true,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example> 
		public bool ShowTrackballLabel
		{
			get { return (bool)GetValue(ShowTrackballLabelProperty); }
			set { SetValue(ShowTrackballLabelProperty, value); }
		}

        /// <summary>
        /// Gets or sets the custom template to customize the appearance of the chart axis trackball label.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="DataTemplate"/> value.
        /// </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-44)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.TrackballBehavior>
        ///        <chart:ChartTrackballBehavior />
        ///    </chart:SfCartesianChart.TrackballBehavior>
        ///    
        ///    <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis ShowTrackballLabel = "True" >
        ///            <chart:CategoryAxis.TrackballLabelTemplate>
        ///                <DataTemplate>
        ///                    <Border CornerRadius = "5" BorderThickness="1" 
        ///                            BorderBrush="Black" Background="LightGreen" Padding="5">
        ///                        <TextBlock Foreground = "Black" Text="{Binding ValueX}"/>
        ///                    </Border>
        ///                </DataTemplate>
        ///            </chart:CategoryAxis.TrackballLabelTemplate>
        ///        </chart:CategoryAxis>
        ///    </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public DataTemplate TrackballLabelTemplate
		{
			get { return (DataTemplate)GetValue(TrackballLabelTemplateProperty); }
			set { SetValue(TrackballLabelTemplateProperty, value); }
		}

        /// <summary>
        /// Gets or sets the custom template to customize the appearance of the crosshair labels.
        /// </summary>
        /// <value>It accepts the <see cref="DataTemplate"/> value.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-45)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///
        ///     <chart:SfCartesianChart.CrosshairBehavior>
        ///        <chart:ChartCrosshairBehavior />
        ///    </chart:SfCartesianChart.CrosshairBehavior>
        ///
        ///    <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis ShowTrackballLabel = "True" >
        ///           <chart:CategoryAxis.CrosshairLabelTemplate>
        ///                <DataTemplate>
        ///                    <Border CornerRadius = "5" BorderThickness="1" 
        ///                            BorderBrush="Black" Background="LightGreen" Padding="5">
        ///                        <TextBlock Foreground = "Black" Text="{Binding ValueX}"/>
        ///                    </Border>
        ///                </DataTemplate>
        ///            </chart:CategoryAxis.CrosshairLabelTemplate>
        ///        </chart:CategoryAxis>
        ///    </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// *** 
        /// </example> 
		public DataTemplate CrosshairLabelTemplate
		{
			get { return (DataTemplate)GetValue(CrosshairLabelTemplateProperty); }
			set { SetValue(CrosshairLabelTemplateProperty, value); }
		}

        /// <summary>
        /// Gets or sets the custom template to customize the appearance of the axis labels.
        /// </summary>
        /// <value>It accepts the <see cref="DataTemplate"/> values.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-46)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///    <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis ShowTrackballLabel = "True" >
        ///            <chart:CategoryAxis.LabelTemplate>
        ///                <DataTemplate>
        ///                    <TextBlock Foreground = "Red" Text="{Binding Content}" />
        ///                </DataTemplate>
        ///            </chart:CategoryAxis.LabelTemplate>
        ///        </chart:CategoryAxis>
        ///    </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public DataTemplate LabelTemplate
		{
			get { return (DataTemplate)GetValue(LabelTemplateProperty); }
			set { SetValue(LabelTemplateProperty, value); }
		}

        /// <summary>
        /// Gets or sets the enum <see cref="ChartAutoScrollingMode"/> to determine whether the axis should be auto scrolled at start or end position. 
        /// </summary>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///    <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis AutoScrollingMode="Start"/>
        ///    </chart:SfCartesianChart.XAxes>
        ///    <chart:SfCartesianChart.YAxes>
        ///        <chart:NumericalAxis AutoScrollingMode="End"/>
        ///    </chart:SfCartesianChart.YAxes>
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xAxis = new CategoryAxis();
        /// xAxis.AutoScrollingMode = ChartAutoScrollingMode.Start;
        /// 
        /// NumericalAxis yAxis = new NumericalAxis();
        /// yaxis.AutoScrollingMode = ChartAutoScrollingMode.End;
        /// 
        /// chart.XAxes.Add(xAxis);	
        /// chart.YAxes.Add(yAxis);	
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        /// <value>This property takes the <see cref="ChartAutoScrollingMode"/> as its value.</value>
        ///<value>Default value is <see cref="ChartAutoScrollingMode.End"/></value>
        internal ChartAutoScrollingMode AutoScrollingMode
		{
			get { return (ChartAutoScrollingMode)GetValue(AutoScrollingModeProperty); }
			set { SetValue(AutoScrollingModeProperty, value); }
		}

        /// <summary>
        /// Gets or sets the value that determines the range of values to be visible during auto scrolling. 
        /// </summary>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///    <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis AutoScrollingDelta="3"/>
        ///    </chart:SfCartesianChart.XAxes>
        ///    <chart:SfCartesianChart.YAxes>
        ///        <chart:NumericalAxis AutoScrollingDelta="2"/>
        ///    </chart:SfCartesianChart.YAxes>
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xAxis = new CategoryAxis();
        /// xAxis.AutoScrollingDelta = 3;
        /// 
        /// NumericalAxis yAxis = new NumericalAxis();
        /// yaxis.AutoScrollingDelta = 2;
        /// 
        /// chart.XAxes.Add(xAxis);	
        /// chart.YAxes.Add(yAxis);	
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        ///<value>This property takes the <see cref="double"/> as its value.</value>
        ///<value>Default value is double.NaN</value>
		internal double AutoScrollingDelta
		{
			get { return (double)GetValue(AutoScrollingDeltaProperty); }
			set { SetValue(AutoScrollingDeltaProperty, value); }
		}

		internal Visibility AxisVisibility
		{
			get { return (Visibility)GetValue(AxisVisibilityProperty); }
			set { SetValue(AxisVisibilityProperty, value); }
		}

		#endregion

		#region Internal Properties

		//// Need to remove the below properties
		#region TODO Sections

		internal ChartPolarAngle PolarAngle { get; set; }

		#endregion

		internal List<ChartSeries> RegisteredSeries { get; set; }

		internal bool IsZoomed
		{
			get
			{
				return ZoomFactor != 1d;
			}
		}

		/// <summary>
		/// Gets or sets zoom factor. Value must fall within 0 to 1. It determines delta of visible range.
		/// </summary>
		internal double ActualZoomFactor
		{
			get
			{
				return (ZoomFactor == 0) ? 0.01 : ZoomFactor;
			}
		}

		#endregion

		#endregion

		#region Methods

		#region Public Virtual Methods

		/// <summary>
		/// Converts Coefficient of Value related to chart control to Polar/Radar type axis unit.
		/// </summary>
		/// <param name="value"> Polar/Radar type axis Coefficient Value</param>
		/// <returns> The value of point on Polar/Radar type axis</returns>
		internal virtual double PolarCoefficientToValue(double value)
		{
			double result = double.NaN;

			value = ValueBasedOnAngle(value);

			if (VisibleLabels?.Count > 0)
				value /= 1 - 1 / (double)VisibleLabels.Count; // WPF-49606 Tooltip position Fix based on - WRT-2476-Polar and radar series segments are plotted before the Actual position.
			else
				value /= 1 - 1 / (VisibleRange.Delta + 1);

			result = VisibleRange.Start + VisibleRange.Delta * value;

			return result;
		}

		/// <summary>
		/// Converts co-ordinate of point related to chart control to Polar/Radar type axis unit.
		/// </summary>
		/// <param name="value">The absolute point value.</param>
		/// <returns>The value of point on axis.</returns>
		/// <seealso cref="ChartAxis.ValueToPolarCoefficient"/>  
		internal virtual double ValueToPolarCoefficient(double value)
		{
			double result = double.NaN;

			var start = VisibleRange.Start;
			var delta = VisibleRange.Delta;

			result = (value - start) / delta;
			if (VisibleLabels?.Count > 0)
				result *= 1 - 1 / (double)VisibleLabels.Count;//WRT-2476-Polar and radar series segments are plotted before the Actual position
			else
				result *= 1 - 1 / (delta + 1);

			return ValueBasedOnAngle(result);
		}

		//// TODO: Remove GetLabelContent method while providing trackball and crosshair behavior support for MAUI chart.
		/// <summary>
		/// Return Object value from the given position value
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		internal virtual object GetLabelContent(double position)
		{
			return GetActualLabelContent(position);
		}

		internal virtual object GetActualLabelContent(double position)
		{
			var labelFormat = LabelStyle != null ? LabelStyle.LabelFormat : String.Empty;
			return Math.Round(position, CRoundDecimals).ToString(labelFormat, CultureInfo.CurrentCulture);
		}

		internal virtual void InitializeMinorGridLinesPanel()
		{

		}

		#endregion

		#region Internal Virtual Methods

		//// TODO: Maintaining this method in partial class to avoid more conditional tags.
		internal virtual void ComputeDesiredSize(Size size)
		{
			this.ClearValue(HeightProperty);
			this.ClearValue(WidthProperty);

			AvailableSize = size;
			CalculateRangeAndInterval(size);
			if (Visibility != Visibility.Collapsed || Area?.AreaType == ChartAreaType.PolarAxes)
			{
				ApplyTemplate();
				if (axisPanel != null)
				{
					UpdatePanels();
					UpdateLabels();
					ComputedDesiredSize = axisPanel.ComputeSize(size);
				}
			}
			else
			{
				ActualPlotOffset = 0;
				ActualPlotOffsetStart = PlotOffsetStart;
				ActualPlotOffsetEnd = PlotOffsetEnd;
				InsidePadding = 0;
				UpdateLabels();
				ComputedDesiredSize = IsVertical ? new Size(0, size.Height) : new Size(size.Width, 0);
			}
		}

		internal virtual void Dispose()
		{
			if (AssociatedAxes != null)
			{
				AssociatedAxes.Clear();
			}

			if(this.VisibleLabels != null)
			{
				this.VisibleLabels.Clear();
				this.VisibleLabels = null;
			}

			if (this.TickPositions != null)
			{
				this.TickPositions.Clear();
				this.TickPositions = null;
			}

			DisposeEvents();
			DisposePanels();
			LabelStyle = null;
			LabelTemplate = null;
			Area = null;
		}

		#endregion

		#region Internal Methods

		//// The following methods are only used in WinUI chart axis.
		#region WinUI axis methods

		internal void OnVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
		{
			if (RegisteredSeries != null)
			{
				foreach (var series in RegisteredSeries)
				{
					var cartesianSeries = series as CartesianSeries;
					if (cartesianSeries != null)
						cartesianSeries.OnVisibleRangeChanged(sender, e);
				}
			}
		}

		internal bool GetTrackballInfo()
		{
			return this.ShowTrackballLabel;
		}

		internal DataTemplate GetTrackBallTemplate()
		{
			return this.TrackballLabelTemplate;
		}

		internal AxisLabelsIntersectAction GetLabelIntersection()
		{
			return this.LabelsIntersectAction;
		}

		internal DoubleRange CalculateRange(DoubleRange actualRange, double zoomPosition, double zoomFactor)
		{
			DoubleRange baseRange = actualRange;
			DoubleRange range = new DoubleRange();

			double start = ActualRange.Start + ZoomPosition * actualRange.Delta;
			////Exception thrown, while using ZoomFactor and ZoomPosition as 0/WPF-14141

			double end = start + ActualZoomFactor * actualRange.Delta;

			if (start < baseRange.Start)
			{
				end = end + (baseRange.Start - start);
				start = baseRange.Start;
			}

			if (end > baseRange.End)
			{
				start = start - (end - baseRange.End);
				end = baseRange.End;
			}

			range = new DoubleRange(start, end);
			return range;
		}

		// To calculate the co-efficient value based on the start angle
		internal double ValueBasedOnAngle(double result)
		{
			if (PolarAngle == ChartPolarAngle.Rotate270)
				result = this.IsInversed ? result : (1d - result);
			else if (PolarAngle == ChartPolarAngle.Rotate0)
				result = this.IsInversed ? (0.75d + result) : (0.75d - result);
			else if (PolarAngle == ChartPolarAngle.Rotate90)
				result = this.IsInversed ? (0.5d + result) : (0.5d - result);
			else if (PolarAngle == ChartPolarAngle.Rotate180)
				result = this.IsInversed ? (0.25d + result) : (0.25d - result);
			result = result < 0 ? result + 1d : result;
			result = result > 1 ? result - 1d : result;
			return result;
		}

		internal void OnPropertyChanged()
		{
			if (this.Area != null)
			{
				this.Area.ScheduleUpdate();
			}
		}

		#endregion

		#endregion

		#region Protected Internal Virtual Methods

		//// This method is used to arrange elements in WinUI chart axis based on ArrangeRect.
		internal virtual void OnAxisBoundsChanged()
		{
			if (axisPanel != null)
			{
				axisPanel.ArrangeElements(new Size(ArrangeRect.Width, ArrangeRect.Height));
			}
		}

		#endregion

		#region Protected Override Methods

		/// <inheritdoc />
		protected override Size MeasureOverride(Size availableSize)
		{
			if (Area != null && Area.VisibleSeries.Count > 0)
				return new Size(0, 0);

			base.MeasureOverride(availableSize);

			return new Size(ArrangeRect.Width, ArrangeRect.Height);
		}

		/// <inheritdoc />
		protected override void OnApplyTemplate()
		{
			{
				ClearItems();
				base.OnApplyTemplate();

				labelsPanel = this.GetTemplateChild("SyncfusionChartAxisLabelsPanel") as Panel;
				elementsPanel = this.GetTemplateChild("SyncfusionChartAxisElementPanel") as Panel;
				axisPanel = this.GetTemplateChild("SyncfusionChartCartesianAxisPanel") as ChartCartesianAxisPanel;

				if (axisPanel != null)
					axisPanel.Axis = this;

				UpdatePanels();
				headerContent = this.GetTemplateChild("SyncfusionChartAxisHeaderContent") as ContentControl;
				this.UpdateHeaderStyle();

				if (axisElementsUpdateRequired)
				{
					if (axisElementsPanel != null)
						axisElementsPanel.UpdateElements();
					if (axisLabelsPanel != null)
						axisLabelsPanel.UpdateElements();
					axisElementsUpdateRequired = false;
				}

				if (this.Area != null && Area.GridLinesPanel != null)
				{
					MajorGridLinesPanel = new CompositorLinesPanel(MajorGridLineStyle);
                    Area.GridLinesPanel.Children.Add(MajorGridLinesPanel);
					InitializeMinorGridLinesPanel();
                }
            }
		}

		#endregion

		#region Private Static Methods

		private static void OnPlotOffsetStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var axis = d as ChartAxis;
			if (axis != null)
			{
				axis.UpdateActualPlotOffsetStart((double)e.NewValue);
				if (axis.Area != null)
					axis.Area.ScheduleUpdate();
			}
		}

		private static void OnPlotOffsetEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var axis = d as ChartAxis;
			if (axis != null)
			{
				axis.UpdateActualPlotOffsetEnd((double)e.NewValue);
				if (axis.Area != null)
					axis.Area.ScheduleUpdate();
			}
		}

		private static void OnOpposedPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{		
			if(d is ChartAxis chartAxis)
                chartAxis.OnPropertyChanged();
        }

		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if (d is ChartAxis chartAxis)
                chartAxis.OnPropertyChanged();
        }

		private static void OnMajorTickStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ChartAxis chartAxis)
			{
				if (chartAxis.axisElementsPanel is ChartCartesianAxisElementsPanel panel &&
                panel.MajorTicksPanel != null && e.NewValue is Style style)
                {
					panel.MajorTicksPanel.ApplyLineStyle(style);
				}
				else
				{
					chartAxis.OnPropertyChanged();
				}
			}
		}

		private static void OnMajorGridLineStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ChartAxis axis && axis.axisElementsPanel is ChartCartesianAxisElementsPanel &&
                axis.MajorGridLinesPanel != null && e.NewValue is Style style)
                {
				axis.MajorGridLinesPanel.ApplyLineStyle(style);
			}
		}

        private static void OnIsInversedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if (d is ChartAxis chartAxis)
                chartAxis.OnIsInversedChanged(e);
        }

		private static void OnShowMajorGridLinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if (d is ChartAxis chartAxis)
                chartAxis.OnShowMajorGridLines((bool)e.NewValue);
        }

		private static void OnHeaderStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if (d is ChartAxis chartAxis)
                chartAxis.UpdateHeaderStyle();
        }

		private static void OnLabelTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ChartAxis axis && axis.Area != null)
				axis.Area.ScheduleUpdate();
		}

        private static void OnLabelRotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if (d is ChartAxis axis && axis.Area != null)
                axis.Area.ScheduleUpdate();
        }

		private static void OnAutoScrollingModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ChartAxis axis)
			{
                axis.CanAutoScroll = true;
                axis.OnPropertyChanged();
            }
		}

        private static void OnAutoScrollingDeltaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartAxis axis)
            {
                var delta = Convert.ToDouble(e.NewValue ?? double.NaN);
                bool needAutoScroll = !double.IsNaN(delta) && delta > 0;
                if(needAutoScroll)
                {
                    axis.ActualAutoScrollDelta = delta;
                    axis.CanAutoScroll = true;
                    axis.OnPropertyChanged();
                }
            }
        }

        private static void OnZoomFactorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if (d is ChartAxis axis)
                axis.OnZoomDataChanged(e);
        }

		private static void OnZoomPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if (d is ChartAxis axis)
                axis.OnZoomDataChanged(e);
        }

		#endregion

		#region Private Methods

		private void InitializeConstructor()
		{
			DefaultStyleKey = typeof(ChartAxis);
			RegisteredSeries = new List<ChartSeries>();
			Binding visibilityBinding = new Binding();
			visibilityBinding.Source = this;
			visibilityBinding.Path = new PropertyPath("Visibility");
			BindingOperations.SetBinding(this, AxisVisibilityProperty, visibilityBinding);
		}


		private void DisposeEvents()
		{
			if (this.VisibleRangeChanged != null)
			{
				foreach (var handler in VisibleRangeChanged.GetInvocationList())
				{
					this.VisibleRangeChanged -= handler as EventHandler<VisibleRangeChangedEventArgs>;
				}

				this.VisibleRangeChanged = null;
			}

			if (ActualRangeChanged != null)
			{
				foreach (var handler in ActualRangeChanged.GetInvocationList())
				{
					this.ActualRangeChanged -= handler as EventHandler<ActualRangeChangedEventArgs>;
				}

				ActualRangeChanged = null;
			}

			if (LabelCreated != null)
			{
				foreach (var handler in LabelCreated.GetInvocationList())
				{
					this.LabelCreated -= handler as EventHandler<ChartAxisLabelEventArgs>;
				}

				LabelCreated = null;
			}
        }

		private void DisposePanels()
		{
			if (axisPanel != null)
			{
				if (axisPanel.LayoutCalc != null)
				{
					axisPanel.LayoutCalc.Clear();
				}

				axisPanel.Axis = null;
				axisPanel.Children.Clear();
				axisPanel = null;
			}

			if (labelsPanel != null)
			{
				labelsPanel.Children.Clear();
				labelsPanel = null;
			}

			if (elementsPanel != null)
			{
				elementsPanel.Children.Clear();
				elementsPanel = null;
			}

			if (axisLabelsPanel != null)
			{
				var circularAxisPanel = axisLabelsPanel as ChartCircularAxisPanel;
				if (circularAxisPanel != null)
				{
					circularAxisPanel.Axis = null;
				}

				var cartesianAxisPanel = axisLabelsPanel as ChartCartesianAxisLabelsPanel;
				if (cartesianAxisPanel != null)
				{
					if (cartesianAxisPanel.children != null)
					{
						cartesianAxisPanel.children.Clear();
					}

					cartesianAxisPanel.Dispose();
				}

				axisLabelsPanel = null;
			}

			var cartesianElementsPanel = axisElementsPanel as ChartCartesianAxisElementsPanel;
			if (cartesianElementsPanel != null)
			{
				if (axisElementsPanel?.Children != null)
				{
					axisElementsPanel.Children.Clear();
				}

				cartesianElementsPanel.Dispose();
				axisElementsPanel = null;

			}

			if (MajorGridLinesPanel != null)
			{
				if (Area != null && Area.GridLinesPanel != null &&
					Area.GridLinesPanel.Children.Contains(MajorGridLinesPanel))
					Area.GridLinesPanel.Children.Remove(MajorGridLinesPanel);

				MajorGridLinesPanel.Dispose();
				MajorGridLinesPanel = null;
			}

			if (MinorGridLinesPanel != null)
			{
				if (Area != null && Area.GridLinesPanel != null &&
					Area.GridLinesPanel.Children.Contains(MinorGridLinesPanel))
					Area.GridLinesPanel.Children.Remove(MinorGridLinesPanel);

				MinorGridLinesPanel.Dispose();
				MinorGridLinesPanel = null;
			}
		}

		private void ClearItems()
		{
			if (MajorGridLinesPanel != null)
			{
				MajorGridLinesPanel.Dispose();
				MajorGridLinesPanel = null;
			}

			if (MinorGridLinesPanel != null)
			{
				MinorGridLinesPanel.Dispose();
				MinorGridLinesPanel = null;
			}

			if (labelsPanel != null)
			{
				labelsPanel.Children.Clear();
				labelsPanel = null;
			}

			if (elementsPanel != null)
			{
				elementsPanel.Children.Clear();
				elementsPanel = null;
			}

			if (axisPanel != null)
			{
				axisPanel.Children.Clear();
				axisPanel = null;
			}

			if (axisLabelsPanel != null)
			{
				axisLabelsPanel.Children.Clear();
				axisLabelsPanel = null;
			}

			if (axisElementsPanel != null)
			{
				axisElementsPanel.Children.Clear();
				axisElementsPanel = null;
			}

			if (headerContent != null)
				headerContent = null;
		}

		void UpdatePanels()
		{
			if (axisPanel != null)
			{
				if (AxisLayoutPanel is ChartPolarAxisLayoutPanel
					&& !(axisLabelsPanel is ChartCircularAxisPanel))
				{
					if (axisLabelsPanel != null)
					{
						axisLabelsPanel.DetachElements();
					}

					if (axisElementsPanel != null)
					{
						axisElementsPanel.DetachElements();
					}

					axisPanel.LayoutCalc.Clear();

					if (labelsPanel != null)
					{
						axisLabelsPanel = new ChartCircularAxisPanel(labelsPanel)
						{
							Axis = this
						};
						axisPanel.LayoutCalc.Add(axisLabelsPanel as ILayoutCalculator);
					}
				}
				else if (!(AxisLayoutPanel is ChartPolarAxisLayoutPanel)
					&& !(axisLabelsPanel is ChartCartesianAxisLabelsPanel))
				{
					if (axisLabelsPanel != null)
					{
						axisLabelsPanel.DetachElements();
					}

					if (axisElementsPanel != null)
					{
						axisElementsPanel.DetachElements();
					}

					axisPanel.LayoutCalc.Clear();

					if (labelsPanel != null)
					{
						axisLabelsPanel = new ChartCartesianAxisLabelsPanel(labelsPanel)
						{
							Axis = this
						};
						axisPanel.LayoutCalc.Add(axisLabelsPanel as ILayoutCalculator);
					}

					if (elementsPanel != null)
					{
						axisElementsPanel = new ChartCartesianAxisElementsPanel(elementsPanel)
						{
							Axis = this
						};
						axisPanel.LayoutCalc.Add(axisElementsPanel as ILayoutCalculator);
					}
				}
			}
		}

		private void UpdateAxisLayout()
		{
			this.OnAxisBoundsChanged();
		}

		private void UpdateAxisElement()
		{
			if (axisLabelsPanel != null)
			{
				axisLabelsPanel.UpdateElements();
				if (axisElementsPanel != null)
					axisElementsPanel.UpdateElements();
			}
			else
			{
				axisElementsUpdateRequired = true;
			}
		}

		private void OnIsInversedChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this.Area != null)
			{
				this.Area.ScheduleUpdate();
			}
		}

		private void OnShowMajorGridLines(bool value)
		{
			if (Area != null)
			{
				if (Area.AreaType == ChartAreaType.CartesianAxes && Area.GridLinesLayout != null)
				{
					if (!value && MajorGridLinesPanel != null)
					{
						MajorGridLinesPanel.Clear();
					}
					else if (value && VisibleLabels?.Count > 0)
					{
						if (!double.IsInfinity(Area.AvailableSize.Height) && !double.IsInfinity(Area.AvailableSize.Width))
						{
							Area.GridLinesLayout.UpdateElements();
							Area.GridLinesLayout.Measure(Area.AvailableSize);
							Area.GridLinesLayout.Arrange(Area.AvailableSize);
						}
					}
				}
			}
		}

		private void OnZoomDataChanged(DependencyPropertyChangedEventArgs e)
		{
			if (this.Area != null)
			{
				if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
					this.Area.CompositionScheduleUpdate();
				else
					this.Area.ScheduleUpdate();
			}
		}

		private void UpdateHeaderStyle()
		{
			if (HeaderTemplate == null && headerContent != null && HeaderStyle != null)
			{
				if (HeaderStyle.Foreground != null)
					headerContent.Foreground = HeaderStyle.Foreground;
				if (HeaderStyle.FontSize != 0.0)
					headerContent.FontSize = HeaderStyle.FontSize;
				if (HeaderStyle.FontFamily != null)
					headerContent.FontFamily = HeaderStyle.FontFamily;
			}
		}

        #endregion

        #endregion
    }

	/// <summary>
	/// 
	/// </summary>
	internal class VisibleRangeChangedEventArgs : EventArgs
	{
		#region Properties

		#region Public Properties

		/// <summary>
		/// 
		/// </summary>
		public DoubleRange NewRange { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DoubleRange OldRange { get; set; }

		#endregion

		#endregion
	}
}
