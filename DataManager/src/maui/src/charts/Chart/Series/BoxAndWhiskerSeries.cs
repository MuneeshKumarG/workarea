﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Text;
using Microsoft.Maui.Platform;
using Syncfusion.Maui.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="BoxAndWhiskerSeries"/> class represents a series that plots box-and-whisker diagrams in a <see cref="SfCartesianChart"/>.
    /// </summary>
    /// <remarks>
    /// <para>A box-and-whisker plot is a chart that shows the distribution of a dataset, indicating the median, quartiles, and outliers.</para>
    /// <para>To render a series, create an instance of <see cref="BoxAndWhiskerSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>,<see cref="XYDataSeries.StrokeWidth"/>, <see cref="Stroke"/>, and <see cref="ChartSeries.Opacity"/> to customize the appearance.</para>
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering above a segment. To display the tooltip on a chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="BoxAndWhiskerSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>LegendIcon - </b> Customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// <para><b>Spacing - </b> To specify the spacing between segments using the <see cref="Spacing"/> property.</para>
    /// <para><b>Width - </b> To specify the width of the box using the <see cref="Width"/> property.</para>
    /// <para> This series does not yet support trackball behaviour.</para>
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
    ///               <chart:BoxAndWhiskerSeries
    ///                   ItemsSource="{Binding BoxWhiskerData}"
    ///                   XBindingPath="Department"
    ///                   YBindingPath="Age"/>
    ///           </chart:SfCartesianChart.Series>  
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     
    ///     CategoryAxis xAxis = new CategoryAxis();
    ///     NumericalAxis yAxis = new NumericalAxis();
    ///     
    ///     chart.XAxes.Add(xAxis);
    ///     chart.YAxes.Add(yAxis);
    ///     
    ///     ViewModel viewModel = new ViewModel();
    /// 
    ///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries();
    ///     series.ItemsSource = viewModel.BoxWhiskerData;
    ///     series.XBindingPath = "Department";
    ///     series.YBindingPath = "Age";
    ///     chart.Series.Add(series);
    ///     
    /// ]]></code>
    /// # [ViewModel](#tab/tabid-3)
    /// <code><![CDATA[
    ///     public ObservableCollection<Model> Data { get; set; }
    /// 
    ///     public ViewModel()
    ///     {
    ///         BoxWhiskerData= new ObservableCollection<Model>(); 
    ///         BoxWhiskerData.Add(new Model(){Department="Development",Age=new List<double>{22, 22,23,25,25,25,26,27,27,28,28,29,30,32,34,32, 34,36,35,38};
    ///         BoxWhiskerData.Add(new Model(){Department="HR",Age=new List<double>{22, 24, 25, 30, 32, 34, 36, 38, 39, 41, 35, 36, 40, 56};
    ///         BoxWhiskerData.Add(new Model(){Department="Finance",Age=new List<double>{26, 27, 28, 30, 32, 34, 35, 37, 35, 37, 45};
    ///         BoxWhiskerData.Add(new Model(){Department="Inventory",Age=new List<double>{21, 23, 24, 25, 26, 27, 28, 30, 34, 36, 38};
    ///         BoxWhiskerData.Add(new Model(){Department="R&D",Age=new List<double>{27, 26, 28, 29, 29, 29, 32, 35, 32, 38, 53};
    ///         BoxWhiskerData.Add(new Model(){Department="Graphics",Age=new List<double>{26, 27, 29, 32, 34, 35, 36, 37, 38, 39, 41, 43, 58};
    ///      }
    ///         
    /// ]]></code>
    /// ***
    /// </example>
    /// <seealso cref="BoxAndWhiskerSegment"/>
    public class BoxAndWhiskerSeries : XYDataSeries
    {
        #region Private Field

        bool isEvenList;

        #endregion

        #region BindableProperties
        /// <summary>
        /// Identifies the <see cref="BoxPlotMode"/> bindable property.
        /// </summary>
        public static readonly BindableProperty BoxPlotModeProperty =
            BindableProperty.Create(nameof(BoxPlotMode),
                typeof(BoxPlotMode),
                typeof(BoxAndWhiskerSeries),
                BoxPlotMode.Exclusive,
                BindingMode.Default,
                null,
                OnBoxPlotModePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="OutlierShapeType"/> bindable property.
        /// </summary>
        public static readonly BindableProperty OutlierShapeTypeProperty = BindableProperty.Create("OutlierSymbolType",
            typeof(ShapeType),
            typeof(BoxAndWhiskerSeries),
            ShapeType.Circle,
            BindingMode.Default,
            null,
            OnShapeTypePropertyChanged);   

        /// <summary>
        /// Identifies the <see cref="ShowMedian"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ShowMedianProperty =
            BindableProperty.Create(nameof(ShowMedian),
                typeof(bool),
                typeof(BoxAndWhiskerSeries),
                defaultValue: false, 
                BindingMode.Default, 
                null, 
                OnMedianPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowOutlier"/> bindable property.
        /// </summary>
        public static readonly BindableProperty ShowOutlierProperty =
            BindableProperty.Create(nameof(ShowOutlier),
                typeof(bool),
                typeof(BoxAndWhiskerSeries),
                defaultValue: true, 
                BindingMode.Default, 
                null, 
                OnBoxPlotModePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Spacing"/> bindable property.
        /// </summary>
        public static readonly BindableProperty SpacingProperty =
            BindableProperty.Create(
                nameof(Spacing),
                typeof(double),
                typeof(BoxAndWhiskerSeries),
                0d,
                BindingMode.Default,
                propertyChanged: OnSpacingChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        public static readonly BindableProperty StrokeProperty =
           BindableProperty.Create(
               nameof(Stroke),
               typeof(Brush),
               typeof(BoxAndWhiskerSeries),
               SolidColorBrush.Black,
               BindingMode.Default,
               propertyChanged: OnStrokeColorChanged);

        /// <summary>
        /// Identifies the <see cref="Width"/> bindable property.
        /// </summary>
        public static readonly BindableProperty WidthProperty =
            BindableProperty.Create(
                nameof(Width),
                typeof(double),
                typeof(BoxAndWhiskerSeries),
                0.8d,
                BindingMode.Default,
                propertyChanged: OnWidthChanged);

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the plot mode, which defines how the box plot series should be rendered.
        /// </summary>
        /// <remarks>
        /// <para> <b><see cref="BoxPlotMode.Normal"/> - </b> This is the default value, which plots the minimum, maximum, median, and quartiles of the data.</para>
        /// <para> <b><see cref="BoxPlotMode.Inclusive"/> - </b> This plots all the data values within the interquartile range (IQR), in addition to the minimum, maximum, median, and quartiles.</para>
        /// <para> <b><see cref="BoxPlotMode.Exclusive"/> - </b> This plots all the data values outside the interquartile range (IQR), in addition to the minimum, maximum, median, and quartiles.</para>
        /// </remarks>
        /// 
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///         <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
        ///                              XBindingPath="Department"
        ///                              YBindingPath="Age"
        ///                              BoxPlotMode = "Inclusive" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
        ///     {
        ///           ItemsSource = viewModel.BoxWhiskerData,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Age",
        ///           BoxPlotMode = BoxPlotMode.Inclusive,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public BoxPlotMode BoxPlotMode
        {
            get { return (BoxPlotMode)GetValue(BoxPlotModeProperty); }
            set { SetValue(BoxPlotModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the shape type for the outlier.
        /// </summary>
        ///  <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
        ///                              XBindingPath="Department"
        ///                              YBindingPath="Age"
        ///                              OutlierShapeType = "Plus" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
        ///     {
        ///           ItemsSource = viewModel.BoxWhiskerData,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Age",
        ///           OutlierShapeType = ShapeType.Plus,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ShapeType OutlierShapeType
        {
            get { return (ShapeType)GetValue(OutlierShapeTypeProperty); }
            set { SetValue(OutlierShapeTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show the median in the box plot or not. 
        /// </summary>
        /// <value>Its default is <c>False</c></value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
        ///                              XBindingPath="Department"
        ///                              YBindingPath="Age"
        ///                              ShowMedian = "True"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
        ///     {
        ///           ItemsSource = viewModel.BoxWhiskerData,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Age",
        ///           ShowMedian = true,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowMedian
        {
            get { return (bool)GetValue(ShowMedianProperty); }
            set { SetValue(ShowMedianProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether to show or hide the outlier of the box plot.
        /// </summary>
        /// <value>
        /// The default value is <c>True</c>.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
        ///                              XBindingPath="Department"
        ///                              YBindingPath="Age"
        ///                              ShowOutlier = "False" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
        ///     {
        ///           ItemsSource = viewModel.BoxWhiskerData,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Age",
        ///           ShowOutlier = false,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowOutlier
        {
            get { return (bool)GetValue(ShowOutlierProperty); }
            set { SetValue(ShowOutlierProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the borders appearance of the BoxAndWhisker series.
        /// </summary>
        /// <value>Its default is <c>SolidColorBrush.Black</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BoxAndWhiskerSeries ItemsSource = "{Binding BoxWhiskerData}"
        ///                              XBindingPath = "Department"
        ///                              YBindingPath = "Age"
        ///                              Stroke = "Red" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
        ///     {
        ///           ItemsSource = viewModel.BoxWhiskerData,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Age",
        ///           Stroke = new SolidColorBrush(Colors.Red),
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to indicate gap between the segments across the series.
        /// </summary>
        /// <value>It accepts values between 0 and 1, and its default is 0.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BoxAndWhiskerSeries ItemsSource = "{Binding BoxWhiskerData}"
        ///                              XBindingPath = "Department"
        ///                              YBindingPath = "Age"
        ///                              Spacing ="0.5" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
        ///     {
        ///           ItemsSource = viewModel.BoxWhiskerData,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Age",
        ///           Spacing = 0.5,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to indicate the width of the box plot.
        /// </summary>
        /// <value>It accepts values between 0 and 1, and default is 0.8.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:BoxAndWhiskerSeries ItemsSource="{Binding BoxWhiskerData}"
        ///                            XBindingPath="Department"
        ///                            YBindingPath="Age"
        ///                            Width = "0.7"/>
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
        ///     BoxAndWhiskerSeries series = new BoxAndWhiskerSeries()
        ///     {
        ///           ItemsSource = viewModel.BoxWhiskerData,
        ///           XBindingPath = "Department",
        ///           YBindingPath = "Age",
        ///           Width = 0.7,
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

        #endregion

        #region Internal Properties

        internal bool IsOutlierTouch { get; set; }

        internal override bool IsSideBySide => true;

        #endregion

        #region Protected Internal Properties

        /// <summary>
        /// Gets or sets the y values collections.
        /// </summary>
        protected internal List<IList<double>>? YDataCollection
        {
            get;
            set;
        }

        #endregion

        #region Methods

        #region Internal Methods 
        
        #region Override Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            if (YDataCollection != null)
            {
                YDataCollection.Clear();
            }
            else
            {
                YDataCollection = new List<IList<double>>();
            }

            base.OnDataSourceChanged(oldValue, newValue);
        }

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override double GetActualSpacing()
        {
            return Spacing;
        }

        internal override double GetActualWidth()
        {
            return Width;
        }

        #region ItemsSource

        internal override void GeneratePropertyPoints(string[] yPaths, IList<double>[] yLists)
        {
            var enumerable = ItemsSource as IEnumerable;
            var enumerator = enumerable?.GetEnumerator();

            if (enumerable != null && enumerator != null && enumerator.MoveNext())
            {
                var currObj = enumerator.Current;

                FastReflection xProperty = new FastReflection();

                if (!xProperty.SetPropertyName(XBindingPath, currObj) || xProperty.IsArray(currObj))
                {
                    return;
                }

                XValueType = GetDataType(xProperty, enumerable);

                if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
                    XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
                {
                    if (!(ActualXValues is List<double>))
                    {
                        this.ActualXValues = this.XValues = new List<double>();
                    }
                }
                else
                {
                    if (!(ActualXValues is List<string>))
                    {
                        this.ActualXValues = this.XValues = new List<string>();
                    }
                }

                FastReflection yProperty = new FastReflection();

                if (!yProperty.SetPropertyName(yPaths[0], currObj) || yProperty.IsArray(currObj))
                {
                    return;
                }

                if (XValueType == ChartValueType.String)
                {
                    var xValue = this.XValues as List<string>;
                    if (xValue != null)
                    {
                        do
                        {
                            var xVal = xProperty.GetValue(enumerator.Current);
                            var yVal = yProperty.GetValue(enumerator.Current);
                            xValue.Add(xVal != null ? (string)xVal : string.Empty);
                            YDataCollection?.Add((IList<double>?)yVal ?? new double[0]);
                            ActualData?.Add(enumerator.Current);
                        }

                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                }
                else if (XValueType == ChartValueType.Double ||
                      XValueType == ChartValueType.Logarithmic)
                {
                    var xValue = this.XValues as List<double>;

                    if (xValue != null)
                    {
                        do
                        {
                            var xVal = xProperty.GetValue(enumerator.Current);
                            var yVal = yProperty.GetValue(enumerator.Current);
                            XData = Convert.ToDouble(xVal != null ? xVal : double.NaN);
                            xValue.Add(XData);
                            YDataCollection?.Add((IList<double>?)yVal ?? new double[0]);
                            ActualData?.Add(enumerator.Current);
                        }

                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                }
                else if (XValueType == ChartValueType.DateTime)
                {
                    var xValue = this.XValues as List<double>;
                    if (xValue != null)
                    {
                        do
                        {
                            var xVal = xProperty.GetValue(enumerator.Current);
                            if (xVal != null)
                            {
                                var yVal = yProperty.GetValue(enumerator.Current);
                                XData = ((DateTime)xVal).ToOADate();
                                xValue.Add(XData);
                                YDataCollection?.Add((IList<double>?)yVal ?? new double[0]);
                                ActualData?.Add(enumerator.Current);
                            }
                        }

                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                }
                else if (XValueType == ChartValueType.TimeSpan)
                {
                    //TODO:Validate on timespan implementation.
                }
            }
        }

        internal override void SetIndividualPoint(int index, object obj, bool replace)
        {
            if (YDataCollection != null && YPaths != null && ItemsSource != null)
            {
                var xvalueType = GetArrayPropertyValue(obj, XComplexPaths);
                if (xvalueType != null)
                    XValueType = GetDataType(xvalueType);

                var tempYPath = YComplexPaths != null ? YComplexPaths[0] : null;
                var yVal = GetArrayPropertyValue(obj, tempYPath);
                if (yVal != null)
                {
                    IList<double> yValue = (IList<double>)yVal;
                    if (XValueType == ChartValueType.String)
                    {
                        if (!(this.XValues is List<string>))
                            this.XValues = this.ActualXValues = new List<string>();
                        var xValue = (List<string>)this.XValues;
                        var xVal = GetArrayPropertyValue(obj, XComplexPaths);

                        if (replace && xValue.Count > index)
                        {
                                xValue[index] = xVal.Tostring();
                        }
                        else
                        {
                            xValue.Insert(index, xVal.Tostring());
                        }

                        if (replace && YDataCollection.Count > index)
                        {
                            YDataCollection[index] = yValue;
                        }
                        else
                        {                        
                            YDataCollection.Insert(index, yValue);
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.Double ||
                        XValueType == ChartValueType.Logarithmic)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        var xValue = (List<double>)this.XValues;
                        var xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        //var yVal = GetArrayPropertyValue(obj, tempYPath);
                        XData = xVal != null ? Convert.ToDouble(xVal) : double.NaN;                      

                        // Check the Data Collection is linear or not
                        if (IsLinearData && (index > 0 && XData < xValue[index - 1]) || (index == 0 && xValue.Count > 0 && XData > xValue[0]))
                        {
                            IsLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {                            
                                xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        if (replace && YDataCollection.Count > index)
                        {
                            YDataCollection[index] = yValue;
                        }
                        else
                        {
                            YDataCollection[index] = yValue;
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.DateTime)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        var xValue = (List<double>)this.XValues;
                        var xVal = GetArrayPropertyValue(obj, XComplexPaths);                       
                        XData = Convert.ToDateTime(xVal).ToOADate();
                   
                        // Check the Data Collection is linear or not
                        if (IsLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            IsLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {                                                       
                             xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        if (replace && YDataCollection.Count > index)
                        {
                            YDataCollection[index] = yValue;
                        }
                        else
                        {
                            YDataCollection[index] = yValue;
                        }

                        PointsCount = xValue.Count;
                    }
                    if (ActualData != null)
                    {
                        if (replace && ActualData.Count > index)
                            ActualData[index] = obj;
                        else if (ActualData.Count == index)
                            ActualData.Add(obj);
                        else
                            ActualData.Insert(index, obj);
                    }
                }           
            }
        }

        internal override void GenerateComplexPropertyPoints(string[] yPaths, IList<double>[] yLists, GetReflectedProperty? getPropertyValue)
        {
            var enumerable = ItemsSource as IEnumerable;
            var enumerator = enumerable?.GetEnumerator();

            if (enumerable != null && enumerator != null && getPropertyValue != null && enumerator.MoveNext() && XComplexPaths != null && YComplexPaths != null)
            {
                XValueType = GetDataType(enumerator, XComplexPaths);
                if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
                    XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
                {
                    if (!(XValues is List<double>))
                    {
                        this.ActualXValues = this.XValues = new List<double>();
                    }
                }
                else
                {
                    if (!(XValues is List<string>))
                    {
                        this.ActualXValues = this.XValues = new List<string>();
                    }
                }

                string[] tempYPath = YComplexPaths[0];
                if (string.IsNullOrEmpty(yPaths[0]))
                {
                    return;
                }

                IList<double> yValue = yLists[0];
                object? xVal, yVal;
                if (XValueType == ChartValueType.String)
                {
                    var xValue = this.XValues as List<string>;
                    if (xValue != null)
                    {
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                            yVal = getPropertyValue(enumerator.Current, tempYPath);
                            if (xVal == null)
                            {
                                return;
                            }

                            xValue.Add((string)xVal);
                            YDataCollection?.Add((IList<double>?)yVal ?? new double[0]);
                            ActualData?.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                }
                else if (XValueType == ChartValueType.Double ||
                    XValueType == ChartValueType.Logarithmic)
                {
                    var xValue = this.XValues as List<double>;
                    if (xValue != null)
                    {
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                            yVal = getPropertyValue(enumerator.Current, tempYPath);
                            XData = Convert.ToDouble(xVal ?? double.NaN);

                            // Check the Data Collection is linear or not
                            if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                            {
                                IsLinearData = false;
                            }

                            xValue.Add(XData);
                            YDataCollection?.Add((IList<double>?)yVal ?? new double[0]);
                            ActualData?.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                }
                else if (XValueType == ChartValueType.DateTime)
                {
                    var xValue = this.XValues as List<double>;
                    if (xValue != null)
                    {
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                            yVal = getPropertyValue(enumerator.Current, tempYPath);

                            XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

                            // Check the Data Collection is linear or not
                            if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                            {
                                IsLinearData = false;
                            }

                            xValue.Add(XData);
                            YDataCollection?.Add((IList<double>?)yVal ?? new double[0]);
                            ActualData?.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                }
                else if (XValueType == ChartValueType.TimeSpan)
                {
                    //TODO: Ensure for timespan;
                }
            }
        }

        internal override void RemoveData(int index, NotifyCollectionChangedEventArgs e)
        {
            if (XValues is IList<double>)
            {
                ((IList<double>)XValues).RemoveAt(index);
                PointsCount--;
            }
            else if (XValues is IList<string>)
            {
                ((IList<string>)XValues).RemoveAt(index);
                PointsCount--;
            }
            
            YDataCollection?.RemoveAt(index); 

            ActualData?.RemoveAt(index);
        }
        
        #endregion

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();

            double bottomValue = 0, median = 0d, lowerQuartile = 0d, upperQuartile = 0d, minimum = 0d, maximum = 0d, average = 0d;

            if (ActualXAxis != null)
            {
                bottomValue = double.IsNaN(ActualXAxis.ActualCrossingValue) ? 0 : ActualXAxis.ActualCrossingValue;
            }

            YDataCollection = this.YDoubleList.Select(x => x as IList<double>).ToList();
            
            if (xValues == null || xValues.Count == 0 || YDataCollection == null || YDataCollection.Count == 0)
            {
                return;
            }

            for (int i = 0; i < PointsCount; i++)
            {
                var yList = YDataCollection[i].Where(x => !double.IsNaN(x)).ToArray();

                List<double> outliers = new List<double>();

                if (yList.Count() > 0)
                {
                    Array.Sort(yList);
                    average = yList.Average();
                }

                int yCount = yList.Length;

                isEvenList = yCount % 2 == 0;

                if (yCount == 0)
                {
                    if (i < Segments.Count && Segments[i] is BoxAndWhiskerSegment)
                    {
                        ((BoxAndWhiskerSegment)Segments[i]).SetData(new[] { i + SbsInfo.Start, i + SbsInfo.End, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN });
                    }
                    else
                    {
                        CreateSegment(seriesView, new[] { i + SbsInfo.Start, i + SbsInfo.End, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN}, i,outliers);
                    }

                    continue;
                }

                // Provides BoxPlotMode Exclusive
                if (BoxPlotMode == BoxPlotMode.Exclusive)
                {
                    lowerQuartile = GetExclusiveQuartileValue(yList, yCount, 0.25d);
                    upperQuartile = GetExclusiveQuartileValue(yList, yCount, 0.75d);
                    median = GetExclusiveQuartileValue(yList, yCount, 0.5d);
                }
                // Provides BoxPlotMode Inclusive
                else if (BoxPlotMode == BoxPlotMode.Inclusive)
                {
                    lowerQuartile = GetInclusiveQuartileValue(yList, yCount, 0.25d);
                    upperQuartile = GetInclusiveQuartileValue(yList, yCount, 0.75d);
                    median = GetInclusiveQuartileValue(yList, yCount, 0.5d);
                }
                // Provides BoxPlotMode Normal
                else
                {
                    median = GetMedianValue(yList);
                    GetQuartileValues(yList, yCount, out lowerQuartile, out upperQuartile);
                }
                
                if (ShowOutlier)
                {
                    GetMinMaxandOutlier(lowerQuartile, upperQuartile, yList, out minimum, out maximum, outliers);
                }
                else
                {
                    minimum = yList.Min();
                    maximum = yList.Max();
                }

                double actualMinimum = minimum;
                double actualMaximum = maximum;

                if (outliers.Count > 0)
                {
                    actualMinimum = Math.Min(outliers.Min(), actualMinimum);
                    actualMaximum = Math.Max(outliers.Max(), actualMaximum);
                }
                
                if (IsIndexed && xValues != null)
                {
                    if (i < Segments.Count && Segments[i] is BoxAndWhiskerSegment)
                    {
                        ((BoxAndWhiskerSegment)Segments[i]).SetData(new[] { i + SbsInfo.Start, i + SbsInfo.End, actualMinimum, minimum, lowerQuartile, median, upperQuartile, maximum, actualMaximum, xValues[i] + SbsInfo.Median, average });
                    }
                    else
                    {
                        CreateSegment(seriesView, new[] { i + SbsInfo.Start, i + SbsInfo.End, actualMinimum, minimum, lowerQuartile, median, upperQuartile, maximum, actualMaximum, xValues[i] + SbsInfo.Median, average }, i,outliers);
                    }
                }
                else
                {
                    if (xValues != null)
                    {
                        var x = xValues[i];

                        if (i < Segments.Count && Segments[i] is BoxAndWhiskerSegment)
                        {
                            ((BoxAndWhiskerSegment)Segments[i]).SetData(new[] { x + SbsInfo.Start, x + SbsInfo.End, actualMinimum, minimum, lowerQuartile, median, upperQuartile, maximum, actualMaximum, xValues[i] + SbsInfo.Median, average });
                        }
                        else
                        {
                            CreateSegment(seriesView, new[] { x + SbsInfo.Start, x + SbsInfo.End, actualMinimum, minimum, lowerQuartile, median, upperQuartile, maximum, actualMaximum, xValues[i] + SbsInfo.Median, average }, i,outliers);
                        }
                    }
                }

                isEvenList= false;
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
           
            double xValue = xValues[index];

            if (Segments[index] is BoxAndWhiskerSegment segment)
            {             
                double yValue;
                float xPosition,yPosition;

                if(IsOutlierTouch)
                {
                    yValue = segment.Outliers[segment.outlierIndex];                   
                }
                else
                {
                    yValue = segment.Maximum;
                }

                xPosition = TransformToVisibleX(xValue, yValue);

                if (!double.IsNaN(xPosition) && !double.IsNaN(yValue))
                {
                    yPosition = TransformToVisibleY(xValue, yValue);

                    if (IsSideBySide)
                    {
                        double xMidVal = xValue + SbsInfo.Start + ((SbsInfo.End - SbsInfo.Start) / 2);
                        xPosition = TransformToVisibleX(xMidVal, yValue);
                        yPosition = TransformToVisibleY(xMidVal, yValue);
                    }

                    TooltipInfo tooltipInfo = new TooltipInfo(this);
                    tooltipInfo.X = xPosition;
                    tooltipInfo.Y = yPosition;
                    tooltipInfo.Index = index;

                    if(IsOutlierTouch)
                    {
                        tooltipInfo.Text = yValue.ToString();
                    }
                    else
                    {
                        tooltipInfo.Text = yValue.ToString("  #.##") + "/" + segment.UpperQuartile.ToString("  #.##") + "/" + segment.Median.ToString("  #.##") + "/" + segment.LowerQuartile.ToString("  #.##") + "/" + segment.Minimum.ToString("  #.##"); 
                    }
                    
                    return tooltipInfo;
                }
            }

            return null;
        }

        internal override DataTemplate? GetDefaultTooltipTemplate(TooltipInfo info)
        {
            return new DataTemplate(() =>
            {
                StackLayout stackLayout = new StackLayout();

                if (IsOutlierTouch)
                {
                    stackLayout.Children.Add(new Label()
                    {
                        Text = info.Text,
                        Background = info.Background,
                        FontAttributes = info.FontAttributes,
                        FontSize = info.FontSize,
                        TextColor = info.TextColor,
                        Margin = info.Margin
                    });
                }
                else
                {
                    var texts = info.Text.Split('/');
                    string maximumFormat = SfCartesianChartResources.Maximum + " :";
                    string minimumFormat = SfCartesianChartResources.Minimum + "  :";
                    string Q3Format = SfCartesianChartResources.Q3 + "\t    :";
                    string Q1Format = SfCartesianChartResources.Q1 + "\t    :";
                    string medianFormat = SfCartesianChartResources.Median + "      :";
                    
                    var labels = new[]
                    {
                       new { LabelText = maximumFormat, ValueText = texts[0] },
                       new { LabelText = Q3Format, ValueText = texts[1] },
                       new { LabelText = medianFormat, ValueText = texts[2] },
                       new { LabelText = Q1Format, ValueText = texts[3] },
                       new { LabelText = minimumFormat, ValueText = texts[4] }
                    };

                    BindableLayout.SetItemsSource(stackLayout, labels);

                    BindableLayout.SetItemTemplate(stackLayout, new DataTemplate(() =>
                    {
                        StackLayout stackLayout1 = new StackLayout();
                        stackLayout1.Orientation = StackOrientation.Horizontal;
                        
                        stackLayout1.Add(new Label
                        {
                            HorizontalOptions = LayoutOptions.Start,
                            VerticalOptions = LayoutOptions.Start,
                            TextColor = info.TextColor,
                            Margin = info.Margin,
                            Background = info.Background,
                            FontAttributes = info.FontAttributes,
                            FontSize = info.FontSize,
                        });
                        
                        stackLayout1.Add(new Label
                        {
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Start,
                            HorizontalTextAlignment = TextAlignment.Start,
                            TextColor = info.TextColor,
                            Margin = info.Margin,
                            Background = info.Background,
                            FontAttributes = info.FontAttributes,
                            FontSize = info.FontSize,
                        });

                        ((Label)stackLayout1.Children[0]).SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding("LabelText"));
                        ((Label)stackLayout1.Children[1]).SetBinding(Microsoft.Maui.Controls.Label.TextProperty, new Binding("ValueText"));
                        return stackLayout1;
                    }));
                }

                return stackLayout;
            });
        }

        internal override void GenerateTrackballPointInfo(List<object> nearestDataPoints, List<TrackballPointInfo> pointInfos, ref bool isSidebySide)
        {
            
        }

        # region Protected Methods

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            return new BoxAndWhiskerSegment();
        }

        #endregion

        #endregion

        #endregion

        #region Private Static Methods

        private static void OnBoxPlotModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BoxAndWhiskerSeries boxAndWhiskerSeries)
            {
                boxAndWhiskerSeries.UpdateSbsSeries();
            }
        }

        private static void OnShapeTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BoxAndWhiskerSeries boxAndWhiskerSeries)
            {
                boxAndWhiskerSeries.InvalidateSeries();
            }
        }

        private static void OnMedianPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BoxAndWhiskerSeries boxAndWhiskerSeries)
            {
                boxAndWhiskerSeries.InvalidateSeries();
            }
        }

        private static void OnStrokeColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BoxAndWhiskerSeries series)
            {
                series.UpdateStrokeColor();
                series.InvalidateSeries();
            }
        }

        private static void OnSpacingChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BoxAndWhiskerSeries series)
            {
                series.UpdateSbsSeries();
            }
        }

        private static void OnWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BoxAndWhiskerSeries series)
            {
                series.UpdateSbsSeries();
            }
        }

        //Getting Quartile values for Exclusive Mode
        private static double GetExclusiveQuartileValue(double[] yList, int yCount, double percentile)
        {
            if (yCount == 0)
            {
                return 0;
            }
            else if (yCount == 1)
            {
                return yList[0];
            }

            double rank = percentile * (yCount + 1);
            int integerRank = (int)Math.Abs(rank);
            double fractionRank = rank - integerRank;
            double value;
            
            if (integerRank == 0)
            {
                value = yList[0];
            }
            else if (integerRank > yCount - 1)
            {
                value = yList[yCount - 1];
            }
            else
            {
                value = fractionRank * (yList[integerRank] - yList[integerRank - 1]) + yList[integerRank - 1];
            }

            return value;
        }

        //Getting Quartile values for Inclusive Mode
        private static double GetInclusiveQuartileValue(double[] yList, int yCount, double percentile)
        {
            if (yCount == 0)
                return 0;
            else if (yCount == 1)
                return yList[0];

            double rank = percentile * (yCount - 1);
            int integerRank = (int)Math.Abs(rank);
            double fractionRank = rank - integerRank;
            double value = fractionRank * (yList[integerRank + 1] - yList[integerRank]) + yList[integerRank];
            return value;
        }

        private static void GetMinMaxandOutlier(double lowerQuartile, double upperQuartile, double[] ylist,
                                         out double minimum, out double maximum, List<double> outliers)
        {
            minimum = 0d;
            maximum = 0d;
            double interquartile = upperQuartile - lowerQuartile;
            double rangeIQR = 1.5 * interquartile;

            for (int i = 0; i < ylist.Length; i++)
            {
                if (ylist[i] < lowerQuartile - rangeIQR)
                {
                    outliers.Add(ylist[i]);
                }
                else
                {
                    minimum = ylist[i];
                    break;
                }
            }

            for (int i = ylist.Length - 1; i >= 0; i--)
            {
                if (ylist[i] > upperQuartile + rangeIQR)
                {
                    outliers.Add(ylist[i]);
                }
                else
                {
                    maximum = ylist[i];
                    break;
                }
            }
        }

        #endregion

        #region Private Methods

        private void GetQuartileValues(double[] yList, int len, out double lowerQuartile, out double upperQuartile)
        {
            double[] lowerQuartileArray;
            double[] upperQuartileArray;

            if (len == 1)
            {
                lowerQuartile = yList[0];
                upperQuartile = yList[0];
                return;
            }

            var halfLength = len / 2;

            lowerQuartileArray = new double[halfLength];
            upperQuartileArray = new double[halfLength];

            Array.Copy(yList, 0, lowerQuartileArray, 0, halfLength);
            Array.Copy(yList, isEvenList ? halfLength : halfLength + 1, upperQuartileArray, 0, halfLength);

            lowerQuartile = GetMedianValue(lowerQuartileArray);
            upperQuartile = GetMedianValue(upperQuartileArray);
        }

        //Getting median value for Box Plot Mode Normal
        private double GetMedianValue(double[] yList)
        {
            int len = yList.Length;

            int middleindex = (int)Math.Round(len / 2.0, MidpointRounding.AwayFromZero);

            if (len % 2 == 0)
                return (yList[middleindex - 1] + yList[middleindex]) / 2;
            else
                return yList[middleindex - 1];
        }

        private void CreateSegment(SeriesView seriesView, double[] values, int index,List<double> outliers)
        {
            var segment = CreateSegment() as BoxAndWhiskerSegment;
            var xValues = GetXValues();

            if (segment != null)
            {
                foreach (var outlier in outliers)
                {
                    if (xValues != null)
                    {
                        segment.Outliers.Add(outlier);
                    }
                }

                segment.Series = this;
                segment.SeriesView = seriesView;
                segment.SetData(values);
                segment.Index = index;
                Segments.Add(segment);
            }
        }

        #endregion

        #endregion        
    }
}

