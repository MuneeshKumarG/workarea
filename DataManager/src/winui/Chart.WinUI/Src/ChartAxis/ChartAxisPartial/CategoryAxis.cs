using Microsoft.UI.Xaml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The category axis is an index-based axis that plots values based on the index of the data point collection. It displays string values in the axis labels.
    /// </summary>
	/// 
    /// <remarks>
    /// 
    /// <para>Category axis supports only for the X(horizontal) axis. </para>
    /// 
    /// <para>To render an axis, create an instance of <see cref="CategoryAxis"/> and add it to the <see cref="SfCartesianChart.XAxes"/> collection.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///  
    ///         <chart:SfCartesianChart.XAxes>
    ///             <chart:CategoryAxis/>
    ///         </chart:SfCartesianChart.XAxes>
    /// 
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// CategoryAxis xaxis = new CategoryAxis();
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para>The CategoryAxis supports the following features. Refer to the corresponding APIs, for more details and example codes.</para>
    /// 
    /// <para> <b>Header - </b> To render the header, refer to this <see cref="ChartAxis.Header"/> property.</para>
    /// <para> <b>Grid Lines - </b> To show and customize the grid lines, refer the <see cref="ChartAxis.ShowMajorGridLines"/>, and <see cref="ChartAxis.MajorGridLineStyle"/> properties.</para>
    /// <para> <b>Axis Line - </b> To customize the axis line using the <see cref="ChartAxis.AxisLineStyle"/> property.</para>
    /// <para> <b>Labels Customization - </b> To customize the axis labels, refer to this <see cref="ChartAxis.LabelStyle"/> property.</para>
    /// <para> <b>Inversed Axis - </b> Inverse the axis using the <see cref="ChartAxis.IsInversed"/> property.</para>
    /// <para> <b>Label Placement - </b> To place the axis labels in between or on the tick lines, refer to this <see cref="LabelPlacement"/> property.</para>
    /// <para> <b>Interval - </b> To define the interval between the axis labels, refer to this <see cref="Interval"/> property.</para>
    /// </remarks>
	public partial class CategoryAxis
	{
		#region Dependency Property Registrations

		/// <summary>
		/// The DependencyProperty for <see cref="Interval"/> property.
		/// </summary>
		public static readonly DependencyProperty IntervalProperty =
			DependencyProperty.Register(
				nameof(Interval),
				typeof(double),
				typeof(CategoryAxis),
				new PropertyMetadata(double.NaN, OnIntervalChanged));

		/// <summary>
		/// The DependencyProperty for <see cref="LabelPlacement"/> property.
		/// </summary>
		public static readonly DependencyProperty LabelPlacementProperty =
			DependencyProperty.Register(
				nameof(LabelPlacement),
				typeof(LabelPlacement),
				typeof(CategoryAxis),
				new PropertyMetadata(LabelPlacement.OnTicks, OnPropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ArrangeByIndex"/> property.
        /// </summary>
        internal static readonly DependencyProperty ArrangeByIndexProperty =
			DependencyProperty.Register(
		   nameof(ArrangeByIndex),
		   typeof(bool),
		   typeof(CategoryAxis),
		   new PropertyMetadata(true, OnArrangeByIndexPropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="IsIndexed"/> property.
        /// </summary>
        internal static readonly DependencyProperty IsIndexedProperty =
			DependencyProperty.Register(nameof(IsIndexed), typeof(bool), typeof(CategoryAxis), new PropertyMetadata(true, OnPropertyChanged));

        
        #endregion

		#region Properties

		#region Public Properties

        /// <summary>
        /// Gets or sets a value that can be used to customize the interval between the axis range.
        /// </summary>
        /// <remarks>If this property is not set, the interval will be calculated automatically.</remarks>
        /// <value>The default value is double.NaN.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:CategoryAxis Interval="2" />
        ///         </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis(){ Interval = 2, };
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
        /// Gets or sets a value that determines whether to place the axis label in between or on the tick lines.
        /// </summary>
        /// <remarks>
        /// 
        /// <para> <b>BetweenTicks - </b> Used to place the axis label between the ticks.</para>
        /// <para> <b>OnTicks - </b> Used to place the axis label with the tick as the center.</para>
        /// 
        /// </remarks>
        /// <value>It accepts the <see cref="Charts.LabelPlacement"/> values and the default value is <c>OnTicks</c>. </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:CategoryAxis LabelPlacement="BetweenTicks" />
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis();
        /// xaxis.LabelPlacement = LabelPlacement.BetweenTicks;
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
		public LabelPlacement LabelPlacement
		{
			get { return (LabelPlacement)GetValue(LabelPlacementProperty); }
			set { SetValue(LabelPlacementProperty, value); }
		}

		/// <summary>
		/// 
		/// </summary>
        internal bool ArrangeByIndex
        {
            get { return (bool)GetValue(ArrangeByIndexProperty); }
            set { SetValue(ArrangeByIndexProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool IsIndexed
		{
			get { return (bool)GetValue(IsIndexedProperty); }
			set { SetValue(IsIndexedProperty, value); }
		}

		#endregion

		#endregion

		#region Methods

		#region Internal Override Methods

		//// TODO: Remove GetLabelContent method while providing trackball and crosshair behavior support for MAUI chart.
		internal override object GetLabelContent(double position)
		{
			ChartSeries actualSeries =
				this.Area.VisibleSeries
				.Where(series => series.ActualXAxis == this)
				.Max(filteredSeries => filteredSeries.PointsCount);

			if (actualSeries != null)
			{
				return GetLabelContent((int)Math.Round(position), actualSeries) ?? string.Empty;
			}

			return position;
		}

		#endregion

		#region Internal Methods

		//// TODO: Remove this method when impletemented IsIndexed property in MAUI chart
		internal void GroupData(ChartVisibleSeriesCollection visibleSeries)
		{
			List<string> groupingValues = new List<string>();

			// Get the x values from the series.
			foreach (var series in visibleSeries)
			{
				if (series is PolarSeries) return;
				if (series.ActualXValues is List<string> xValues)
					groupingValues.AddRange(xValues);
				else
					groupingValues.AddRange(from val in (series.ActualXValues as List<double>)
											select val.ToString());
			}

			var distinctXValues = groupingValues.Distinct().ToList();
			foreach (var series in visibleSeries)
			{
				if (series.ActualXValues is List<string>)
					series.GroupedXValuesIndexes = (from val in (List<string>)series.ActualXValues
													select (double)distinctXValues.IndexOf(val)).ToList();
				else
					series.GroupedXValuesIndexes = (from val in series.ActualXValues as List<double>
													select (double)distinctXValues.IndexOf(val.ToString())).ToList();
				series.GroupedXValues = distinctXValues;
			}

			foreach (var series in visibleSeries)
			{
				series.DistinctValuesIndexes.Clear();
				var aggregateValues = new Dictionary<int, List<double>>[series.ActualSeriesYValues.Length];
				var yValues = new List<double>[series.ActualSeriesYValues.Length];
				series.GroupedSeriesYValues = new IList<double>[series.ActualSeriesYValues.Length];

				for (int i = 0; i < series.ActualSeriesYValues.Length; i++)
				{
					series.GroupedSeriesYValues[i] = new List<double>();
					aggregateValues[i] = new Dictionary<int, List<double>>();
					((List<double>)series.GroupedSeriesYValues[i]).AddRange(series.ActualSeriesYValues[i]);

				}

				var actualXValues = series.ActualXValues is List<string>
					? series.ActualXValues as List<string>
					: (from val in (series.ActualXValues as List<double>) select val.ToString()).ToList();

				for (int i = 0; i < distinctXValues.Count; i++)
				{
					int count = 0;
					var indexes = new List<int>();
					for (int j = 0; j < series.ActualSeriesYValues.Length; j++)
					{
						yValues[j] = new List<double>();

					}

					(from xValues in actualXValues
					 let index = count++
					 where distinctXValues[i] == xValues
					 select index).Select(t1 =>
					 {
						 for (int j = 0; j < series.ActualSeriesYValues.Length; j++)
						 {
							 yValues[j].Add(series.ActualSeriesYValues[j][count - 1]);
							 if (j == 0)
								 indexes.Add(count - 1);
						 }

						 return 0;
					 }).ToList();

					for (int j = 0; j < series.ActualSeriesYValues.Length; j++)
					{
						aggregateValues[j].Add(i, yValues[j]);
					}

					series.DistinctValuesIndexes.Add(i, indexes);
				}
			}
		}

		internal object GetLabelContent(int pos, ChartSeries actualSeries)
		{
			var isIndexed = (actualSeries is PolarSeries) ?
				true : this.IsIndexed;
			if (actualSeries != null)
			{
				object label = string.Empty;
				var labelContent = string.Empty;
				int count = 0;

				foreach (ChartSeries chartSeries in this.RegisteredSeries)
				{
					IEnumerable? pointValues = chartSeries.ActualXValues;
					ChartValueType valueType = chartSeries.XValueType;
					label = string.Empty;
					var labelFormat = this.LabelStyle != null ? this.LabelStyle.LabelFormat : String.Empty;
					var values = pointValues as List<double>;

					if (values != null && pos < values.Count && pos >= 0)
					{

						if (valueType == ChartValueType.DateTime)
						{
							DateTime xDateTime = values[pos].FromOADate();
							label = xDateTime.ToString(labelFormat, CultureInfo.CurrentCulture);
						}
						else if (valueType == ChartValueType.TimeSpan)
						{
							TimeSpan xTimeSpanValue = TimeSpan.FromMilliseconds(values[pos]);
							label = xTimeSpanValue.ToString(labelFormat, CultureInfo.CurrentCulture);
						}

						else if (valueType == ChartValueType.Double || valueType == ChartValueType.Logarithmic)
						{
							label = values[pos].ToString(labelFormat, CultureInfo.CurrentCulture);
						}

					}
					else
					{
						List<string> StrValues = new List<string>();
						StrValues = !(chartSeries is PolarSeries)
							? !isIndexed
							? actualSeries.GroupedXValues
							: pointValues as List<string>
							: actualSeries.XValues as List<string>;
						if (StrValues != null && pos < StrValues.Count && pos >= 0)
						{
							if (!String.IsNullOrEmpty(labelFormat))
								label = String.Format(labelFormat, StrValues[pos]);
							label = StrValues[pos];
						}
					}

					if (!string.IsNullOrEmpty(label.ToString()) && !labelContent.Contains(label.ToString()))
					{
						labelContent = count > 0 && !string.IsNullOrEmpty(labelContent) ? labelContent + ", " + label : label.ToString();
					}

					if (!isIndexed)
					{
						return labelContent;
					}

					if (chartSeries is PolarSeries)
					{
						return labelContent;
					}

					count++;
				}

				return labelContent;
			}

			return pos;
		}

		#endregion

		#region Private Static Methods

		private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var axis = d as CategoryAxis;
			axis.UpdateAxisInterval((double)e.NewValue);
			if (axis.Area != null)
				axis.Area.ScheduleUpdate();
		}

		private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			var axis = d as CategoryAxis;
			if (axis != null && axis.Area != null)
			{
				axis.Area.ScheduleUpdate();
			}
		}

        private static void OnArrangeByIndexPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var categoryAxis =  d as ChartAxis;

            if (categoryAxis != null)
            {
                foreach (var series in categoryAxis.CartesianArea.VisibleSeries)
                {
                    series.ScheduleUpdateChart();
                }
            }
        }
        #endregion

        #endregion
    }
}
