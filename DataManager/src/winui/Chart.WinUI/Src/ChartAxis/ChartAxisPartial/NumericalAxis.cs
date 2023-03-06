using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The numerical axis uses a numerical scale and it displays numbers as labels.
    /// </summary>
    /// <remarks>
    /// <para>It supports both horizontal and vertical axes. To render an axis, create an instance of <see cref="NumericalAxis"/> and add it to the <see cref="SfCartesianChart.XAxes"/> and <see cref="SfCartesianChart.YAxes"/> collection.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///  
    ///         <chart:SfCartesianChart.XAxes>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfCartesianChart.XAxes>
    ///         
    ///         <chart:SfCartesianChart.YAxes>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfCartesianChart.YAxes>
    /// 
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// NumericalAxis xaxis = new NumericalAxis();
    /// chart.XAxes.Add(xaxis);
    /// 
    /// NumericalAxis yaxis = new NumericalAxis();
    /// chart.YAxes.Add(yaxis);
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
    /// <para> <b>Interval - </b> Define the axis labels interval using the <see cref="Interval"/> property.</para>
    /// </remarks>
	public partial class NumericalAxis
	{
		#region Dependency Property Registration

		/// <summary>
		/// The DependencyProperty for <see cref="Interval"/> property.
		/// </summary>
		public static readonly DependencyProperty IntervalProperty =
			DependencyProperty.Register(
				nameof(Interval),
				typeof(double),
				typeof(NumericalAxis),
				new PropertyMetadata(double.NaN, OnIntervalChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="Minimum"/> property.
		/// </summary>
		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register(
				nameof(Minimum),
				typeof(double),
				typeof(NumericalAxis),
				new PropertyMetadata(double.NaN, OnMinimumChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="Maximum"/> property.
		/// </summary>
		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register(
				nameof(Maximum),
				typeof(double),
				typeof(NumericalAxis),
				new PropertyMetadata(double.NaN, OnMaximumChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="RangePadding"/> property.
		/// </summary>
		public static readonly DependencyProperty RangePaddingProperty =
			DependencyProperty.Register(
				nameof(RangePadding),
				typeof(NumericalPadding),
				typeof(NumericalAxis),
				new PropertyMetadata(NumericalPadding.Auto, OnPropertyChanged));

        /// <summary>
		/// The DependencyProperty for <see cref="ActualMaximum"/> property.
		/// </summary>
		internal static readonly DependencyProperty ActualMaximumProperty =
            DependencyProperty.Register(
                nameof(ActualMaximum),
                typeof(double),
                typeof(NumericalAxis),
                new PropertyMetadata(double.NaN));

        /// <summary>
        /// The DependencyProperty for <see cref="ActualMinimum"/> property.
        /// </summary>
        internal static readonly DependencyProperty ActualMinimumProperty =
            DependencyProperty.Register(
                nameof(ActualMinimum),
                typeof(double),
                typeof(NumericalAxis),
                new PropertyMetadata(double.NaN));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericalAxis"/> class.
        /// </summary>
        public NumericalAxis()
		{
			DefaultStyleKey = typeof(NumericalAxis);
		}

		#endregion

		#region Properties

		#region Public Properties

        /// <summary>
        /// Gets or sets a value that determines the interval in axis's range.
        /// </summary>
        /// <remarks> 
        /// If this property is not set, the interval will be calculated automatically.
        /// </remarks>
        /// <value>The default value is double.NaN.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis Interval="10" />
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
        /// xaxis.Interval = 10;
        /// chart.XAxes.Add(xaxis);	
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
		public double Interval
		{
			get { return (double)GetValue(IntervalProperty); }
			set { SetValue(IntervalProperty, value); }
		}

        /// <summary>
        /// Gets or sets the minimum value for the axis range.
        /// </summary>
        /// <remarks>
        /// If we didn't set the minimum value, it will be calculated from the underlying collection.
        /// </remarks>
        /// <value>The default value is double.NaN.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis Maximum="100" Minimum="0"/>
        ///         </chart:SfCartesianChart.XAxes>
        ///         
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// NumericalAxis xaxis = new NumericalAxis()
        /// {
        ///     Maximum = 100,
        ///     Minimum = 0,
        /// };
        /// chart.XAxes.Add(xaxis);	
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
		public double Minimum
		{
			get { return (double)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}

        /// <summary>
        /// Gets or sets the maximum value for the axis range.
        /// </summary>
        /// <remarks>
        /// If we didn't set the maximum value, it will be calculated from the underlying collection.
        /// </remarks>
        /// <value>The default value is double.NaN.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis Maximum="100" Minimum="0"/>
        ///         </chart:SfCartesianChart.XAxes>
        ///         
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// NumericalAxis xaxis = new NumericalAxis()
        /// {
        ///     Maximum = 100,
        ///     Minimum = 0,
        /// };
        /// chart.XAxes.Add(xaxis);	
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
		public double Maximum
		{
			get { return (double)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}

        /// <summary>
        /// Gets or sets a padding type for the numerical axis range. 
        /// </summary>
        /// <value>It accepts the <see cref="NumericalPadding"/> value and its default value is <see cref="NumericalPadding.Auto"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis RangePadding ="Round"/>
        ///         </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-10)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// NumericalAxis xaxis = new NumericalAxis()
        /// {
        ///     RangePadding = NumericalPadding.Round,
        /// };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
		public NumericalPadding RangePadding
		{
			get { return (NumericalPadding)GetValue(RangePaddingProperty); }
			set { SetValue(RangePaddingProperty, value); }
		}

        /// <summary>
        /// Gets the actual minimum value of the NumericalAxis
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-11)
        /// <code><![CDATA[
        ///   <StackPanel>
        ///      <TextBlock Text="{Binding ElementName=xAxis, Path=ActualMinimum, Mode=TwoWay}" />
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis x:Name="xAxis"/>
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
        /// # [MainWindow.cs](#tab/tabid-12)
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
        public double ActualMinimum
        {
            get { return (double)GetValue(ActualMinimumProperty); }
            internal set { SetValue(ActualMinimumProperty, value); }
        }

        /// <summary>
        /// Gets the actual maximum value of the NumericalAxis
        /// </summary>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-13)
        /// <code><![CDATA[
        ///   <StackPanel>
        ///      <TextBlock Text="{Binding ElementName=xAxis, Path=ActualMaximum, Mode=TwoWay}" />
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis x:Name="xAxis"/>
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
        public double ActualMaximum
        {
            get { return (double)GetValue(ActualMaximumProperty); }
            internal set { SetValue(ActualMaximumProperty, value); }
        }

        #endregion

        #endregion

        #region Methods

        #region Private Static Methods

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
            if(d is NumericalAxis axis)
                axis.OnPropertyChanged();
        }

		private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var axis = d as NumericalAxis;
			if (axis != null)
			{
				axis.UpdateDefaultMinimum((double)e.NewValue);
				axis.OnMinMaxChanged();
				if (axis.Area != null)
					axis.Area.ScheduleUpdate();
			}
		}

		/// <summary>
		/// Called Maximum property changed
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var axis = d as NumericalAxis;
			if (axis != null)
			{
				axis.UpdateDefaultMaximum((double)e.NewValue);
				axis.OnMinMaxChanged();
				if (axis.Area != null)
					axis.Area.ScheduleUpdate();
			}
		}

		private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var axis = d as NumericalAxis;
			if (axis != null)
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
