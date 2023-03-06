using Microsoft.Maui.Controls;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The CategoryAxis is an indexed based axis that plots values based on the index of the data point collection. It displays string values in axis labels.
    /// </summary>
    /// <remarks>
    /// 
    /// <para>Category axis supports only for the X(horizontal) axis. </para>
    /// 
    /// <para>To render an axis, add the category axis instance to the chart’s <see cref="SfCartesianChart.XAxes"/> collection as shown in the following code sample.</para>
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
    /// <para> <b>Title - </b> To render the title, refer to this <see cref="ChartAxis.Title"/> property.</para>
    /// <para> <b>Grid Lines - </b> To show and customize the grid lines, refer these <see cref="ChartAxis.ShowMajorGridLines"/>, and <see cref="ChartAxis.MajorGridLineStyle"/> properties.</para>
    /// <para> <b>Axis Line - </b> To customize the axis line using the <see cref="ChartAxis.AxisLineStyle"/> property.</para>
    /// <para> <b>Labels Customization - </b> To customize the axis labels, refer to this <see cref="ChartAxis.LabelStyle"/> property.</para>
    /// <para> <b>Inversed Axis - </b> Inverse the axis using the <see cref="ChartAxis.IsInversed"/> property.</para>
    /// <para> <b>Axis Crossing - </b> For axis crossing, refer these <see cref="ChartAxis.CrossesAt"/>, <see cref="ChartAxis.CrossAxisName"/>, and <see cref="ChartAxis.RenderNextToCrossingValue"/> properties.</para>
    /// <para> <b>Label Placement - </b> To place the axis labels in between or on the tick lines, refer to this <see cref="LabelPlacement"/> property.</para>
    /// <para> <b>Interval - </b> To define the interval between the axis labels, refer to this <see cref="Interval"/> property.</para>
    /// </remarks>
    public partial class CategoryAxis
    {
        #region Bindable Properties
        /// <summary>
        /// Identifies the <see cref="Interval"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
            nameof(Interval),
            typeof(double),
            typeof(CategoryAxis),
            double.NaN,
            BindingMode.Default,
            null,
            OnIntervalPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LabelPlacement"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty LabelPlacementProperty = BindableProperty.Create(
            nameof(LabelPlacement),
            typeof(LabelPlacement),
            typeof(CategoryAxis),
            LabelPlacement.OnTicks,
            BindingMode.Default,
            null,
            OnLabelPlacementPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ArrangeByIndex"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ArrangeByIndexProperty = BindableProperty.Create(
            nameof(ArrangeByIndex),
            typeof(bool),
            typeof(CategoryAxis),
            true,
            BindingMode.Default,
            null,
            OnArrangeByIndexPropertyChanged);

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets a value that determines whether to place the axis label in between or on the tick lines.
        /// </summary>
        /// <remarks>
        /// <para> <b>BetweenTicks - </b> Used to place the axis label between the ticks.</para>
        /// <para> <b>OnTicks - </b> Used to place the axis label with the tick as the center.</para>
        /// </remarks>
        /// <value>It accepts the <see cref="Charts.LabelPlacement"/> values and the default value is <c>OnTicks</c>. </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
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
        /// # [MainPage.xaml.cs](#tab/tabid-4)
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
        /// Gets or sets a value that can be used to customize the interval between the axis labels.
        /// </summary>
        /// <remarks>If this property is not set, the interval will be calculated automatically.</remarks>
        /// <value>It accepts double values and the default value is double.NaN.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
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
        /// # [MainPage.xaml.cs](#tab/tabid-6)
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
        ///  Gets or sets a value that determines whether to arrange the axis labels by index or by value.
        /// </summary>
        /// <remarks>
        /// <para> <b>True - </b> Used to arrange the axis labels by index.</para>
        /// <para> <b>False - </b> Used to arrange the axis labels by value.</para>
        /// </remarks>
        /// <value>Its default is <c>True</c>. </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:CategoryAxis ArrangeByIndex="False" />
        ///         </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// CategoryAxis xaxis = new CategoryAxis(){ ArrangeByIndex = false, };
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public bool ArrangeByIndex
        {
            get { return (bool)GetValue(ArrangeByIndexProperty); }
            set { SetValue(ArrangeByIndexProperty, value); }
        }

        #endregion

        #region Private Methods
        private static void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as CategoryAxis;
            if (axis != null)
            {
                axis.UpdateAxisInterval((double)newValue);
                axis.UpdateLayout();
            }
        }

        private static void OnLabelPlacementPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as CategoryAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnArrangeByIndexPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var categoryAxis = (ChartAxis)bindable;

            if (categoryAxis != null)
            {
                foreach (var series in categoryAxis.RegisteredSeries)
                {
                    series.SegmentsCreated = false;
                }
                
                categoryAxis.UpdateLayout();
            }
        }

        private string GetLabel(CartesianSeries series, int position, string labelFormat)
        {
            string label = string.Empty;

            switch (series.XDataValueType)
            {
                case ValueType.String:
                    {
                        label = ArrangeByIndex ? series.XStringList[position] : series.GroupedXValues[position];
                        break;
                    }
                case ValueType.Double:
                case ValueType.Int:
                case ValueType.Long:
                case ValueType.Float:
                    {
                        label = ArrangeByIndex ? series.XDoubleList[position].ToString(labelFormat) : series.GroupedXValues[position];
                        break;
                    }
                case ValueType.DateTime:
                    {
                        if (ArrangeByIndex)
                        {
                            label = series.XDateTimeList[position].ToString(labelFormat);
                        }
                        else if (double.TryParse(series.GroupedXValues[position], out double result))
                        {
                            label = DateTime.FromOADate(result).ToString(labelFormat);
                        }
                        break;
                    }
            }

            return label;
        }
        #endregion
    }
}
