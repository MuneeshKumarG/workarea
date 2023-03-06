using Microsoft.UI.Xaml;
using System;
using System.Globalization;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// DateTimeAxis is used to plot DateTime values in chart axis.
    /// </summary>
    /// <remarks>
    /// <para>The date-time axis uses date time scale and displays date-time values as axis labels in the specified format.</para>
    /// <para>To render an axis, create an instance of <see cref="DateTimeAxis"/> and add it to the <see cref="SfCartesianChart.XAxes"/> or <see cref="SfCartesianChart.YAxes"/> collection.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///  
    ///         <chart:SfCartesianChart.XAxes>
    ///             <chart:DateTimeAxis/>
    ///         </chart:SfCartesianChart.XAxes>
    ///         
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// DateTimeAxis xaxis = new DateTimeAxis();
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para> <b>Header - </b> To render the header, refer to this <see cref="ChartAxis.Header"/> property.</para>
    /// <para> <b>Grid Lines - </b> To show and customize the grid lines, refer to these <see cref="ChartAxis.ShowMajorGridLines"/>, <see cref="ChartAxis.MajorGridLineStyle"/>, <see cref="RangeAxisBase.MinorTicksPerInterval"/>, and <see cref="RangeAxisBase.MinorGridLineStyle"/> properties.</para>
    /// <para> <b>Tick Lines - </b> To show and customize the tick lines, refer to these <see cref="ChartAxis.MajorTickStyle"/>, <see cref="RangeAxisBase.MinorTickStyle"/>, and <see cref="RangeAxisBase.MinorTicksPerInterval"/> properties.</para>
    /// <para> <b>Axis Line - </b> To customize the axis line using the <see cref="ChartAxis.AxisLineStyle"/> property.</para>
    /// <para> <b>Range Customization - </b> To customize the axis range with help of the <see cref="Minimum"/>, and <see cref="Maximum"/> properties.</para>
    /// <para> <b>Labels Customization - </b> To customize the axis labels, refer to this <see cref="ChartAxis.LabelStyle"/> property.</para>
    /// <para> <b>Inversed Axis - </b> Inverse the axis using the <see cref="ChartAxis.IsInversed"/> property.</para>
    /// <para> <b>Interval - </b> To define the axis labels interval using the <see cref="Interval"/>, and <see cref="IntervalType"/> properties.</para>
    /// </remarks>
	public partial class DateTimeAxis
	{
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="AutoScrollingDeltaType"/> property. 
        /// </summary>
        internal static readonly DependencyProperty AutoScrollingDeltaTypeProperty =
			DependencyProperty.Register(
				nameof(AutoScrollingDeltaType),
				typeof(DateTimeIntervalType),
				typeof(DateTimeAxis),
				new PropertyMetadata(DateTimeIntervalType.Auto, OnAutoScrollingDeltaTypeChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="Minimum"/> property.
		/// </summary>
		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register(nameof(Minimum), typeof(DateTime), typeof(DateTimeAxis), new PropertyMetadata(DateTime.MinValue, OnMinimumChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="Maximum"/> property.
		/// </summary>
		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register(nameof(Maximum), typeof(DateTime), typeof(DateTimeAxis), new PropertyMetadata(DateTime.MaxValue, OnMaximumChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="Interval"/> property.
		/// </summary>
		public static readonly DependencyProperty IntervalProperty =
			DependencyProperty.Register(
				nameof(Interval),
				typeof(double),
				typeof(DateTimeAxis),
				new PropertyMetadata(double.NaN, OnIntervalChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="RangePadding"/> property.
		/// </summary>
		public static readonly DependencyProperty RangePaddingProperty =
			DependencyProperty.Register(
				nameof(RangePadding),
				typeof(DateTimeRangePadding),
				typeof(DateTimeAxis),
				new PropertyMetadata(DateTimeRangePadding.Auto, new PropertyChangedCallback(OnRangePaddingChanged)));

		/// <summary>
		///  The DependencyProperty for <see cref="IntervalType"/> property.
		/// </summary>
		public static readonly DependencyProperty IntervalTypeProperty =
			DependencyProperty.Register(
				nameof(IntervalType),
				typeof(DateTimeIntervalType),
				typeof(DateTimeAxis),
				new PropertyMetadata(
					DateTimeIntervalType.Auto,
					new PropertyChangedCallback(OnIntervalTypeChanged)));

        /// <summary>
        /// The DependencyProperty for <see cref="ActualMaximum"/> property.
        /// </summary>
        internal static readonly DependencyProperty ActualMaximumProperty =
            DependencyProperty.Register(
                nameof(ActualMaximum),
                typeof(DateTime),
                typeof(DateTimeAxis),
                new PropertyMetadata(DateTime.MaxValue));

        /// <summary>
        /// The DependencyProperty for <see cref="ActualMinimum"/> property.
        /// </summary>
        internal static readonly DependencyProperty ActualMinimumProperty =
            DependencyProperty.Register(
                nameof(ActualMinimum),
                typeof(DateTime),
                typeof(DateTimeAxis),
                new PropertyMetadata(DateTime.MinValue));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeAxis"/> class.
        /// </summary>
        public DateTimeAxis()
		{
			DefaultStyleKey = typeof(DateTimeAxis);
		}

		#endregion

		#region Properties

		#region Public Properties

        /// <summary>
        /// Gets or sets the minimum value of the time period to be displayed on the chart axis.
        /// </summary>
        /// <remarks>
        /// If we didn't set the minimum value, it will be calculated from the underlying collection.
        /// </remarks>
        /// <value>The default value is <c>DateTime.MinValue</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:DateTimeAxis Minimum="2021/05/10" Maximum="2021/11/01"/>
        ///         </chart:SfCartesianChart.XAxes>
        ///         
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// DateTimeAxis xaxis = new DateTimeAxis()
        /// {
        ///     Minimum = new DateTime(2021,05,10),
        ///     Maximum = new DateTime(2021,11,01),
        /// };
        /// chart.XAxes.Add(xaxis);	
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// 
        /// </example>
		public DateTime Minimum
		{
			get { return (DateTime)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

        /// <summary>
        /// Gets or sets the maximum value of the time period to be displayed on the chart axis.
        /// </summary>
        /// <remarks>
        /// If we didn't set the maximum value, it will be calculated from the underlying collection.
        /// </remarks>
        /// <value>The default value is <c>DateTime.MaxValue</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:DateTimeAxis Minimum="2021/05/10" Maximum="2021/11/01"/>
        ///         </chart:SfCartesianChart.XAxes>
        ///         
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// DateTimeAxis xaxis = new DateTimeAxis()
        /// {
        ///     Minimum = new DateTime(2021,05,10),
        ///     Maximum = new DateTime(2021,11,01),
        /// };
        /// chart.XAxes.Add(xaxis);	
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// 
        /// </example>
		public DateTime Maximum
		{
			get { return (DateTime)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

        /// <summary>
        /// Gets or sets a value that can be used to change the interval between labels.
        /// </summary>
        /// <remarks>
        /// <para>If this property is not set, the interval will be calculated automatically.</para>
        /// <para>By default, the interval will be calculated based on the minimum and maximum values of the provided data.</para>
        /// 
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:DateTimeAxis Interval="6" IntervalType="Months" />
        ///         </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// DateTimeAxis xaxis = new DateTimeAxis()
        /// {
        ///    Interval = 6, 
        ///    IntervalType = DateTimeIntervalType.Months
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// 
        /// </remarks>
        /// <value>It accepts <c>double</c> values and the default value is 0.</value>
        /// <see cref="IntervalType"/>
		public double Interval
		{
			get { return (double)GetValue(IntervalProperty); }
			set { SetValue(IntervalProperty, value); }
		}

        /// <summary>
        /// Gets or sets a padding type for the date time axis range.
        /// </summary>
        /// <value>It acceps the <see cref="DateTimeRangePadding"/> value and its default value is <see cref="DateTimeRangePadding.Auto"/></value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:DateTimeAxis RangePadding ="Round" />
        ///         </chart:SfCartesianChart.XAxes>
        ///         
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-10)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// DateTimeAxis xaxis = new DateTimeAxis()
        /// {
        ///     RangePadding = DateTimeRangePadding.Round,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// 
        /// </example>
		public DateTimeRangePadding RangePadding
		{
			get { return (DateTimeRangePadding)GetValue(RangePaddingProperty); }
			set { SetValue(RangePaddingProperty, value); }
		}

        /// <summary>
        /// Gets or sets the date time unit of the value specified in the <see cref="Interval"/> property.
        /// </summary>
        /// <value>It accepts the <see cref="DateTimeIntervalType"/> value and its default value is <see cref="DateTimeIntervalType.Auto"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-11)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:DateTimeAxis IntervalType="Months"/>
        ///         </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-12)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// DateTimeAxis xaxis = new DateTimeAxis()
        /// {
        ///     IntervalType = DateTimeIntervalType.Months
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// 
        /// </example>
		public DateTimeIntervalType IntervalType
		{
			get { return (DateTimeIntervalType)GetValue(IntervalTypeProperty); }
			set { SetValue(IntervalTypeProperty, value); }
		}

        /// <summary>
        /// Gets the actual minimum value of the DateTimeAxis.
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-13)
        /// <code><![CDATA[
        ///   <StackPanel>
        ///     <TextBlock Text="{Binding ElementName=xAxis, Path=ActualMinimum, Mode=TwoWay}" />
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:DateTimeAxis x:Name="xAxis"/>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis x:Name="yAxis"/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///           <chart:SfCartesianChart.Series>
        ///               <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   YBindingPath="YValue" />
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        ///   </StackPanel>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-14)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     DateTimeAxis xAxis = new DateTimeAxis();
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     
        ///     chart.XAxes.Add(xAxis);
        ///     chart.YAxes.Add(yAxis);
        ///     
        ///     ColumnSeries series = new ColumnSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///     
        ///     TextBlock textBlock = new TextBlock();
        ///     Binding binding = new Binding();
        ///     binding.Source = xAxis;
        ///     binding.Path = new PropertyPath("ActualMinimum");
        ///     textBlock.SetBinding(TextBlock.TextProperty, binding);
        ///     
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DateTime ActualMinimum
        {
            get { return (DateTime)GetValue(ActualMinimumProperty); }
            internal set { SetValue(ActualMinimumProperty, value); }
        }


        /// <summary>
        /// Gets the actual maximum value of the DateTimeAxis.
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-15)
        /// <code><![CDATA[
        ///   <StackPanel>
        ///     <TextBlock Text="{Binding ElementName=xAxis, Path=ActualMaximum, Mode=TwoWay}" />
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:DateTimeAxis x:Name="xAxis"/>
        ///           </chart:SfCartesianChart.XAxes>
        ///
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis x:Name="yAxis"/>
        ///           </chart:SfCartesianChart.YAxes>
        ///
        ///           <chart:SfCartesianChart.Series>
        ///               <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                                   XBindingPath="XValue"
        ///                                   YBindingPath="YValue" />
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        ///   </StackPanel>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-16)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     DateTimeAxis xAxis = new DateTimeAxis();
        ///     NumericalAxis yAxis = new NumericalAxis();
        ///     
        ///     chart.XAxes.Add(xAxis);
        ///     chart.YAxes.Add(yAxis);
        ///     
        ///     ColumnSeries series = new ColumnSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///     
        ///     TextBlock textBlock = new TextBlock();
        ///     Binding binding = new Binding();
        ///     binding.Source = xAxis;
        ///     binding.Path = new PropertyPath("ActualMaximum");
        ///     textBlock.SetBinding(TextBlock.TextProperty, binding);
        ///     
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DateTime ActualMaximum
        {
            get { return (DateTime)GetValue(ActualMaximumProperty); }
            internal set { SetValue(ActualMaximumProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the enum <see cref="DateTimeIntervalType"/> to determine the unit of time used for the delta value for auto-scrolling on a DateTime axis. 
        /// </summary>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///    <chart:SfCartesianChart.XAxes>
        ///        <chart:DateTimeAxis AutoScrollingDeltaType="Years"/>
        ///    </chart:SfCartesianChart.XAxes>
        ///    <chart:SfCartesianChart.YAxes>
        ///        <chart:NumericalAxis/>
        ///    </chart:SfCartesianChart.YAxes>
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// DateTimeAxis xAxis = new DateTimeAxis();
        /// xAxis.AutoScrollingDeltaType = DateTimeIntervalType.Years;
        /// 
        /// NumericalAxis yAxis = new NumericalAxis();
        /// 
        /// chart.XAxes.Add(xAxis);	
        /// chart.YAxes.Add(yAxis);	
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        /// <value>This property takes the <see cref="DateTimeIntervalType"/> as its value.</value>
        ///<value>Default value is <see cref="DateTimeIntervalType.Auto"/></value>
        internal DateTimeIntervalType AutoScrollingDeltaType
		{
			get { return (DateTimeIntervalType)GetValue(AutoScrollingDeltaTypeProperty); }
			set { SetValue(AutoScrollingDeltaTypeProperty, value); }
		}

		#endregion

		#endregion

		#region Methods

		#region Internal Override Methods

		//// TODO: Remove GetLabelContent method while providing trackball and crosshair behavior support for MAUI chart.
		internal override object GetLabelContent(double position)
		{
			return this.GetActualLabelContent(position);
		}

		internal override object GetActualLabelContent(double position)
		{
			var labelFormat = LabelStyle != null ? LabelStyle.LabelFormat : String.Empty;
			return position.FromOADate().ToString(labelFormat, CultureInfo.CurrentCulture);
		}

		#endregion

		#region Private Static Methods

		private static void OnAutoScrollingDeltaTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var dateTimeAxis = d as DateTimeAxis;

			if (dateTimeAxis != null && dateTimeAxis.Area != null)
			{
                dateTimeAxis.CanAutoScroll = true;
				dateTimeAxis.OnPropertyChanged();
			}
		}

		private static void OnRangePaddingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if (e.NewValue != null && d is DateTimeAxis axis)
                axis.OnPropertyChanged();
		}

		private static void OnIntervalTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var dateTimeAxis = d as DateTimeAxis;
			if (dateTimeAxis != null && e.NewValue != null)
			{
				dateTimeAxis.UpdateActualIntervalType((DateTimeIntervalType)e.NewValue);
				dateTimeAxis.OnPropertyChanged();
			}
		}

		private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var dateTimeAxis = d as DateTimeAxis;
			if (dateTimeAxis != null)
			{
				dateTimeAxis.UpdateDefaultMinimum((DateTime)e.NewValue);
				dateTimeAxis.OnMinMaxChanged();
				if (dateTimeAxis.Area != null)
					dateTimeAxis.Area.ScheduleUpdate();
			}

		}

		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var dateTimeAxis = d as DateTimeAxis;
			if (dateTimeAxis != null)
			{
				dateTimeAxis.UpdateDefaultMaximum((DateTime)e.NewValue);
				dateTimeAxis.OnMinMaxChanged();
				if (dateTimeAxis.Area != null)
					dateTimeAxis.Area.ScheduleUpdate();
			}
		}

		private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if (d is DateTimeAxis axis)
            {
                axis.UpdateAxisInterval((double)e.NewValue);

                if (axis.Area != null)
                    axis.Area.ScheduleUpdate();
            }
        }

		#endregion

		#endregion
	}
}
