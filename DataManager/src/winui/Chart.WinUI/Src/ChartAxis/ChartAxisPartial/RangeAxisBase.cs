using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.ComponentModel;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="RangeAxisBase"/> is the base class for all types of range axis.
    /// </summary>
	public partial class RangeAxisBase
	{
		#region Dependency Property Registration

		/// <summary>
		/// The DependencyProperty for <see cref="EdgeLabelsVisibilityMode"/> property.
		/// </summary>
		public static readonly DependencyProperty EdgeLabelsVisibilityModeProperty =
			DependencyProperty.Register(
				nameof(EdgeLabelsVisibilityMode),
				typeof(EdgeLabelsVisibilityMode),
				typeof(ChartAxis),
				new PropertyMetadata(EdgeLabelsVisibilityMode.Default, OnEdgeLabelsVisibilityModeChanged));

		/// <summary>
		///  The DependencyProperty for <see cref="MinorTicksPerInterval"/> property.
		/// </summary>
		public static readonly DependencyProperty MinorTicksPerIntervalProperty =
			DependencyProperty.Register(
				nameof(MinorTicksPerInterval),
				typeof(int),
				typeof(ChartAxis),
				new PropertyMetadata(0, new PropertyChangedCallback(OnMinorTicksPerIntervalPropertyChanged)));

		/// <summary>
		/// The DependencyProperty for <see cref="MinorTickLineSize"/> property.
		/// </summary>
		public static readonly DependencyProperty MinorTickLineSizeProperty =
			DependencyProperty.Register(nameof(MinorTickLineSize), typeof(double), typeof(ChartAxis), new PropertyMetadata(5d, OnSmallTicksPropertyChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="MinorGridLineStyle"/> property.
		/// </summary>
		public static readonly DependencyProperty MinorGridLineStyleProperty =
			DependencyProperty.Register(
				nameof(MinorGridLineStyle),
				typeof(Style),
				typeof(RangeAxisBase),
				new PropertyMetadata(null, OnMinorGridLineStylePropertyChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="MinorTickStyle"/> property.
		/// </summary>
		public static readonly DependencyProperty MinorTickStyleProperty =
			DependencyProperty.Register(nameof(MinorTickStyle), typeof(Style), typeof(RangeAxisBase), new PropertyMetadata(null, OnMinorTickStylePropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ShowMinorGridLines"/> property.
        /// </summary>
        public static readonly DependencyProperty ShowMinorGridLinesProperty =
            DependencyProperty.Register(
                nameof(ShowMinorGridLines),
                typeof(bool),
                typeof(RangeAxisBase),
                new PropertyMetadata(true, new PropertyChangedCallback(OnShowMinorGridLinesChanged)));


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAxisBase"/> class.
        /// </summary>
        public RangeAxisBase()
		{
			SmallTickPoints = new List<double>();
		}

		#endregion

		#region Properties

		#region Public Properties

        /// <summary>
        /// Gets or sets the edge labels visibility mode for hiding the edge labels on zooming.
        /// </summary>
        /// <value>It accepts the <see cref="Charts.EdgeLabelsVisibilityMode"/> values and its default value is <see cref="Charts.EdgeLabelsVisibilityMode.Default"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:NumericalAxis EdgeLabelsVisibilityMode ="Visible" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// NumericalAxis xaxis = new NumericalAxis()
        /// {
        ///    EdgeLabelsVisibilityMode = EdgeLabelsVisibilityMode.Visible,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
		public EdgeLabelsVisibilityMode EdgeLabelsVisibilityMode
		{
			get { return (EdgeLabelsVisibilityMode)GetValue(EdgeLabelsVisibilityModeProperty); }
			set { SetValue(EdgeLabelsVisibilityModeProperty, value); }
		}

        /// <summary>
        ///  Gets or sets the value that defines the number of minor tick/grid lines to be drawn between the adjacent major tick/grid lines.
        /// </summary>
        /// <value>It accepts the <c>integer</c> values and its default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis MinorTicksPerInterval="3"  />
        ///         </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// NumericalAxis xaxis = new NumericalAxis();
        /// xaxis.MinorTicksPerInterval= 3;
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
		public int MinorTicksPerInterval
		{
			get { return (int)GetValue(MinorTicksPerIntervalProperty); }
			set { SetValue(MinorTicksPerIntervalProperty, value); }
		}

        /// <summary>
        /// Gets or sets a value to modify the minor tick line size.
        /// </summary>
        /// <value>It accepts double values and its default value is 5.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis MinorTickLineSize="10"  />
        ///         </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// NumericalAxis xaxis = new NumericalAxis();
        /// xaxis.MinorTickLineSize= 10;
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
		public double MinorTickLineSize
		{
			get { return (double)GetValue(MinorTickLineSizeProperty); }
			set { SetValue(MinorTickLineSizeProperty, value); }
		}

        /// <summary>
        /// Gets or sets the style to customize the appearance of the minor grid lines.
        /// </summary>
        /// <value>It accepts the <see cref="Style"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis MinorTicksPerInterval="2">
        ///                 <chart:NumericalAxis.MinorGridLineStyle>
        ///                     <Style TargetType = "Line" >
        ///                         <Setter Property="StrokeThickness" Value="2"/>
        ///                         <Setter Property = "Stroke" Value="Red" />
        ///                     </Style>
        ///                 </chart:NumericalAxis.MinorGridLineStyle>
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
        /// # [MainWindow.cs](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis xAxis = new NumericalAxis();
        ///     xAxis.MinorTicksPerInterval = 2;
        ///     Style minorGridLineStyle = new Style() { TargetType = typeof(Line), };
        ///     minorGridLineStyle.Setters.Add(new Setter(Path.StrokeThicknessProperty, 2));
        ///     minorGridLineStyle.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Red)));
        ///     xAxis.MinorGridLineStyle = minorGridLineStyle;
        ///     chart.XAxes.Add(xAxis);
        ///     
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
		public Style MinorGridLineStyle
		{
			get { return (Style)GetValue(MinorGridLineStyleProperty); }
			set { SetValue(MinorGridLineStyleProperty, value); }
		}

        /// <summary>
        /// Gets or sets the style to customize the appearance of the minor tick lines.
        /// </summary>
        /// <value>It accepts the <see cref="Style"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis MinorTicksPerInterval="2">
        ///                 <chart:NumericalAxis.MinorTickStyle>
        ///                     <Style TargetType = "Line" >
        ///                         <Setter Property="StrokeThickness" Value="2"/>
        ///                         <Setter Property = "Stroke" Value="Red" />
        ///                     </Style>
        ///                 </chart:NumericalAxis.MinorTickStyle>
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
        ///     xAxis.MinorTicksPerInterval = 2;
        ///     Style minorTickStyle = new Style() { TargetType = typeof(Line), };
        ///     minorTickStyle.Setters.Add(new Setter(Path.StrokeThicknessProperty, 2));
        ///     minorTickStyle.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Red)));
        ///     xAxis.MinorTickStyle = minorTickStyle;
        ///     chart.XAxes.Add(xAxis);
        ///
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     chart.YAxes.Add(yAxis);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
		public Style MinorTickStyle
		{
			get { return (Style)GetValue(MinorTickStyleProperty); }
			set { SetValue(MinorTickStyleProperty, value); }
		}

        /// <summary>
        /// Gets or sets a value indicating whether the axis minor grid lines can be displayed or not.
        /// </summary>
        /// <value>It accepts the bool value and its default value is <c>True</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-11)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:NumericalAxis ShowMinorGridLines = "False" 
        ///                              MinorTicksPerInterval="2" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-12)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// NumericalAxis xaxis = new NumericalAxis()
        /// {
        ///    ShowMinorGridLines = false,
        ///    MinorTicksPerInterval = 2,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public bool ShowMinorGridLines
        {
            get { return (bool)GetValue(ShowMinorGridLinesProperty); }
            set { SetValue(ShowMinorGridLinesProperty, value); }
        }

        #endregion

        #endregion

        #region Methods

        internal override void InitializeMinorGridLinesPanel()
        {
            MinorGridLinesPanel = new CompositorLinesPanel(MinorGridLineStyle);
            Area?.GridLinesPanel.Children.Add(MinorGridLinesPanel);
        }

        #region Private Static Methods

        private static void OnEdgeLabelsVisibilityModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ChartAxis axis && axis != null && axis.Area != null)
				axis.Area.ScheduleUpdate();
		}

		private static void OnMinorTicksPerIntervalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ChartAxis axis && axis != null)
			{
				axis.UpdateSmallTickRequired((int)e.NewValue);
				if (axis.Area != null)
					axis.Area.ScheduleUpdate();
			}
		}

		private static void OnSmallTicksPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (d is ChartAxis axis && axis != null && axis.Area != null)
				axis.Area.ScheduleUpdate();
		}

		private static void OnMinorGridLineStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if (d is ChartAxis axis)
            {
                if (axis.axisElementsPanel is ChartCartesianAxisElementsPanel &&
                 axis.MinorGridLinesPanel != null && e.NewValue is Style style)
                {
                    axis.MinorGridLinesPanel.ApplyLineStyle(style);
                }
                else if (axis != null && axis.Area != null)
                    axis.Area.ScheduleUpdate();
            }
		}

        private static void OnMinorTickStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartAxis axis && axis.axisElementsPanel is ChartCartesianAxisElementsPanel panel &&
                panel.MinorTicksPanel != null && e.NewValue is Style style)
                {
                panel.MinorTicksPanel.ApplyLineStyle(style);
            }
        }

        private static void OnShowMinorGridLinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RangeAxisBase rangeAxisBase)
			    rangeAxisBase?.OnShowMinorGridLines((bool)e.NewValue);
        }

        private void OnShowMinorGridLines(bool value)
        {
            if (Area != null && Area.GridLinesLayout != null)
            {
                if (!value && MinorGridLinesPanel != null)
                {
                    MinorGridLinesPanel.Clear();
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

        #endregion

        #endregion

    }
}
