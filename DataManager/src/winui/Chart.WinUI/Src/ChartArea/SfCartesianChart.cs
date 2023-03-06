using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Renders different types of cartesian-type charts, each representing a unique style of representing data with a more user-friendly and greater UI visualization.
    /// </summary>
    /// <remarks>
    /// <para>The Cartesian chart control is used to visualize the data graphically, it typically have horizontal and vertical axes. </para>
    ///
    /// <para>SfCartesianChart class properties provides an option to add the series and axis collection, allows to customize the chart elements such as series, axis, legend, data label and tooltip features.</para>
    ///
    /// <img src="https://cdn.syncfusion.com/content/images/maui/MAUI_CartesianChart.png"/>
    /// 
    /// <b>Axis</b>
    /// 
    /// <para><b>ChartAxis</b> is used to locate a data point inside the chart area. Charts typically have two axes that are used to measure and categorize data. 
    /// <b>Vertical(Y)</b> axis always uses numerical scale. <b>Horizontal(X)</b> axis supports the <b>Category, Numeric and Date time</b>.</para>
    /// 
    /// <para>To render an axis, the chart axis instance has to be added in chart’s <see cref="XAxes"/> and <see cref="YAxes"/> collection as per the following code snippet.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    /// 
    ///         <chart:SfCartesianChart.DataContext>
    ///             <local:ViewModel/>
    ///         </chart:SfCartesianChart.DataContext>
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
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
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
    /// <para><b>Series</b></para>
    /// 
    /// <para>ChartSeries is the visual representation of data. SfCartesianChart offers many types such as Line, Fast line, Fast column, Fast scatter, Fast step line, Spline, Column, Scatter, Bubble, Area and SplineArea series. Based on your requirements and specifications, any type of series can be added for data visualization.</para>
    /// 
    /// <para>To render a series, create an instance of required series class, and add it to the <see cref="Series"/> collection.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-3)
    /// <code> <![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.DataContext>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///        <chart:SfCartesianChart.Series>
    ///            <chart:LineSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
    ///            <chart:LineSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2"/>
    ///        </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-4)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    /// 
    /// NumericalAxis xaxis = new NumericalAxis();
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// NumericalAxis yaxis = new NumericalAxis();
    /// chart.YAxes.Add(yaxis);
    /// 
    /// LineSeries series1 = new LineSeries()
    /// {
    ///     ItemsSource = viewmodel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue1"
    /// };
    /// chart.Series.Add(series1);
    /// 
    /// LineSeries series2 = new LineSeries()
    /// {
    ///     ItemsSource = viewmodel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue2"
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// # [ViewModel.cs](#tab/tabid-5)
    /// <code><![CDATA[
    /// public ObservableCollection<Model> Data { get; set; }
    /// 
    /// public ViewModel()
    /// {
    ///    Data = new ObservableCollection<Model>();
    ///    Data.Add(new Model() { XValue = 10, YValue1 = 100, YValue2 = 110 });
    ///    Data.Add(new Model() { XValue = 20, YValue1 = 150, YValue2 = 100 });
    ///    Data.Add(new Model() { XValue = 30, YValue1 = 110, YValue2 = 130 });
    ///    Data.Add(new Model() { XValue = 40, YValue1 = 230, YValue2 = 180 });
    /// }
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Legend</b></para>
    /// 
    /// <para>The Legend contains list of chart series or data points in chart. The information provided in each legend item helps to identify the corresponding data series in chart. The Series <see cref="ChartSeries.Label"/> property text will be displayed in the associated legend item.</para>
    /// 
    /// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="ChartBase.Legend"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-6)
    /// <code> <![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.DataContext>
    ///        
    ///        <chart:SfCartesianChart.Legend>
    ///            <chart:ChartLegend/>
    ///        </chart:SfCartesianChart.Legend>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///        <chart:SfCartesianChart.Series>
    ///            <chart:LineSeries Label="Singapore" ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1"/>
    ///            <chart:LineSeries Label="Spain" ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2"/>
    ///        </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-7)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    ///	
    /// chart.Legend = new ChartLegend();
    /// 
    /// NumericalAxis xaxis = new NumericalAxis();
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// NumericalAxis yaxis = new NumericalAxis();
    /// chart.YAxes.Add(yaxis);
    /// 
    /// LineSeries series1 = new LineSeries()
    /// {
    ///     ItemsSource = viewmodel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue1",
    ///     Label = "Singapore"
    /// };
    /// chart.Series.Add(series1);
    /// 
    /// LineSeries series2 = new LineSeries()
    /// {
    ///     ItemsSource = viewmodel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue2",
    ///     Label = "Spain"
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Tooltip</b></para>
    /// 
    /// <para>Tooltip displays information while tapping or mouse hover on the segment. To display the tooltip on chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="ChartSeries"/>. </para>
    /// 
    /// <para>To customize the appearance of the tooltip elements like Background, TextColor and Font, create an instance of <see cref="ChartTooltipBehavior"/> class, modify the values, and assign it to the chart’s <see cref="ChartBase.TooltipBehavior"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-8)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    /// 
    ///         <chart:SfCartesianChart.DataContext>
    ///             <local:ViewModel/>
    ///         </chart:SfCartesianChart.DataContext>
    /// 
    ///         <chart:SfCartesianChart.TooltipBehavior>
    ///             <chart:ChartTooltipBehavior/>
    ///         </chart:SfCartesianChart.TooltipBehavior>
    /// 
    ///         <chart:SfCartesianChart.XAxes>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfCartesianChart.XAxes>
    /// 
    ///         <chart:SfCartesianChart.YAxes>
    ///             <chart:NumericalAxis/>
    ///         </chart:SfCartesianChart.YAxes>
    /// 
    ///         <chart:SfCartesianChart.Series>
    ///             <chart:LineSeries EnableTooltip="True"
    ///                               ItemsSource="{Binding Data}"
    ///                               XBindingPath="XValue"
    ///                               YBindingPath="YValue1"/>
    ///             <chart:LineSeries EnableTooltip="True"
    ///                               ItemsSource="{Binding Data}"
    ///                               XBindingPath="XValue"
    ///                               YBindingPath="YValue2"/>
    ///         </chart:SfCartesianChart.Series>
    /// 
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-9)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    ///
    /// chart.TooltipBehavior = new ChartTooltipBehavior();
    ///
    /// NumericalAxis xaxis = new NumericalAxis();
    /// chart.XAxes.Add(xaxis);	
    ///
    /// NumericalAxis yaxis = new NumericalAxis();
    /// chart.YAxes.Add(yaxis);
    ///
    /// LineSeries series1 = new LineSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue1",
    ///    EnableTooltip = true
    /// };
    /// chart.Series.Add(series1);
    ///
    /// LineSeries series2 = new LineSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue2",
    ///    EnableTooltip = true
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Data Label</b></para>
    /// 
    /// <para>Data labels are used to display values related to a chart segment. To render the data labels, you need to enable the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <b>DataMarkerSeries</b> class. </para>
    /// 
    /// <para>To customize the chart data labels alignment, placement and label styles, need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-10)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.DataContext>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///        <chart:SfCartesianChart.Series>
    ///            <chart:ColumnSeries ShowDataLabels="True"
    ///                                ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue1"/>
    ///        </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-11)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    /// 
    /// NumericalAxis xaxis = new NumericalAxis();
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// NumericalAxis yaxis = new NumericalAxis();
    /// chart.YAxes.Add(yaxis);
    /// 
    /// ColumnSeries series = new ColumnSeries()
    /// {
    ///     ItemsSource = viewmodel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue1",
    ///     ShowDataLabels = true
    /// };
    /// chart.Series.Add(series);
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Zooming and Panning</b></para>
    /// 
    /// <para>SfCartesianChart allows you to zoom the chart area with the help of the zoom feature. This behavior is mostly used to view the data point in the specific area, when there are large number of data points inside the chart.</para>
    /// 
    /// <para>Zooming and panning provides you to take a close-up look of the data point plotted in the series. To enable the zooming and panning in the chart, create an instance of <see cref="ChartZoomPanBehavior"/> and set it to the <see cref="ZoomPanBehavior"/> property of SfCartesianChart.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-12)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.DataContext>
    ///        
    ///        <chart:SfCartesianChart.ZoomPanBehavior>
    ///            <chart:ChartZoomPanBehavior EnablePanning = "True" EnableDoubleTap="True" EnablePinchZooming="True"/>
    ///        </chart:SfCartesianChart.ZoomPanBehavior>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///        <chart:SfCartesianChart.Series>
    ///             <chart:LineSeries ItemsSource="{Binding Data}"
    ///                               XBindingPath="XValue"
    ///                               YBindingPath="YValue1"/>
    ///             <chart:LineSeries ItemsSource="{Binding Data}"
    ///                               XBindingPath="XValue"
    ///                               YBindingPath="YValue2"/>
    ///         </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-13)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    ///	
    /// chart.ZoomPanBehavior = new ChartZoomPanBehavior() { EnablePinchZooming = true, EnableDoubleTap = true, EnablePanning = true };
    /// 
    /// NumericalAxis xaxis = new NumericalAxis();
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// NumericalAxis yaxis = new NumericalAxis();
    /// chart.YAxes.Add(yaxis);
    /// 
    /// LineSeries series1 = new LineSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue1",
    /// };
    /// chart.Series.Add(series1);
    ///
    /// LineSeries series2 = new LineSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue2",
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Selection</b></para>
    /// 
    /// <para>SfCartesianChart allows you to select or highlight a segment or series in the chart by using ChartSelectionBehavior.</para>
    /// 
    /// <para>SfCartessianChart provides seperate behaviors for series and segment selection. To enable the series selection in the chart, create an instance of <see cref="SeriesSelectionBehavior"/> and set it to the <see cref="SelectionBehavior"/> property of SfCartesianChart.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-14)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.DataContext>
    ///        
    ///        <chart:SfCartesianChart.SelectionBehavior>
    ///            <chart:SeriesSelectionBehavior SelectionBrush="Red"/>
    ///        </chart:SfCartesianChart.SelectionBehavior>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///        <chart:SfCartesianChart.Series>
    ///             <chart:ColumnSeries ItemsSource="{Binding Data}"
    ///                                 XBindingPath="XValue"
    ///                                 YBindingPath="YValue1"/>
    ///             <chart:ColumnSeries ItemsSource="{Binding Data}"
    ///                                 XBindingPath="XValue"
    ///                                 YBindingPath="YValue2"/>
    ///         </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-15)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    ///	
    /// chart.SelectionBehavior = new SeriesSelectionBehavior() { SelectionBrush=new SolidColorBrush(Colors.Green) };
    /// 
    /// NumericalAxis xaxis = new NumericalAxis();
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// NumericalAxis yaxis = new NumericalAxis();
    /// chart.YAxes.Add(yaxis);
    /// 
    /// ColumnSeries series1 = new ColumnSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue1",
    /// };
    /// chart.Series.Add(series1);
    ///
    /// ColumnSeries series2 = new ColumnSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue2",
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><see cref="DataPointSelectionBehavior"/> is applicable only to certain series such as <see cref="ColumnSeries"/>,
    /// <see cref="AreaSeries"/>, <see cref="BubbleSeries"/>, <see cref="LineSeries"/>, <see cref="ScatterSeries"/>, 
    /// <see cref="SplineSeries"/>, <see cref="SplineAreaSeries"/>, <see cref="StackedAreaSeries"/>, <see cref="StackedColumnSeries"/>,
    /// <see cref="StepAreaSeries"/>, <see cref="StepLineSeries"/>.</para>
    /// 
    /// <para>To enable the segment selection in the chart, create an instance of <see cref="DataPointSelectionBehavior"/> and set it to the <see cref="SelectionBehavior"/> property of series.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-16)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.DataContext>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///         <chart:SfCartesianChart.Series>
    ///             <chart:ColumnSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1">
    ///                 <chart:ColumnSeries.SelectionBehavior>
    ///                     <chart:DataPointSelectionBehavior SelectionBrush = "Green" />
    ///                 </chart:ColumnSeries.SelectionBehavior>
    ///             </chart:ColumnSeries>
    ///         </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-17)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    ///	
    /// NumericalAxis xaxis = new NumericalAxis();
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// NumericalAxis yaxis = new NumericalAxis();
    /// chart.YAxes.Add(yaxis);
    /// 
    /// DataPointSelectionBehavior selectionBehavior = new DataPointSelectionBehavior()
    /// {
    ///    SelectionBrush=new SolidColorBrush(Colors.Green)
    /// };
    /// ColumnSeries series1 = new ColumnSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue1",
    ///    Selection = selectionBehavior
    /// };
    /// chart.Series.Add(series1);
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Cross Hair</b></para>
    /// 
    /// <para>SfCartesianChart allows you to view the informations related to chart coordinates, at mouse over position or at touch contact point inside a chart.</para>
    /// 
    /// <para>ChartCrosshairBehavior displays a vertical line, horizontal line and a popup like control displaying information about the data point at touch contact point or at mouse over position. To enable the crosshair in the chart, create an instance of <see cref="ChartCrosshairBehavior"/> and set it to the <see cref="CrosshairBehavior"/> property of SfCartesianChart.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-18)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.DataContext>
    ///        
    ///        <chart:SfCartesianChart.CrosshairBehavior>
    ///            <chart:ChartCrosshairBehavior/>
    ///        </chart:SfCartesianChart.CrosshairBehavior>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///        <chart:SfCartesianChart.Series>
    ///             <chart:LineSeries ItemsSource="{Binding Data}"
    ///                               XBindingPath="XValue"
    ///                               YBindingPath="YValue1"/>
    ///             <chart:LineSeries ItemsSource="{Binding Data}"
    ///                               XBindingPath="XValue"
    ///                               YBindingPath="YValue2"/>
    ///         </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-19)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    ///	
    /// chart.CrosshairBehavior = new ChartCrosshairBehavior();
    /// 
    /// NumericalAxis xaxis = new NumericalAxis();
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// NumericalAxis yaxis = new NumericalAxis();
    /// chart.YAxes.Add(yaxis);
    /// 
    /// LineSeries series1 = new LineSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue1",
    /// };
    /// chart.Series.Add(series1);
    ///
    /// LineSeries series2 = new LineSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue2",
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Track Ball</b></para>
    /// 
    /// <para>SfCartesianChart allows you to view tooltip for the data points that are nearer to mouse over position or at touch contact point in a Chart.</para>
    /// 
    /// <para>To enable the track ball in the chart, create an instance of <see cref="ChartTrackballBehavior"/> and set it to the <see cref="TrackballBehavior"/> property of SfCartesianChart.</para>
    ///
    /// <para>To view the trackball label in the particular axis, you have to enable the <see cref="ChartAxis.ShowTrackballLabel"/> property in that axis.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-20)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCartesianChart.DataContext>
    ///        
    ///        <chart:SfCartesianChart.TrackballBehavior>
    ///            <chart:ChartTrackballBehavior/>
    ///        </chart:SfCartesianChart.TrackballBehavior>
    ///
    ///        <chart:SfCartesianChart.XAxes>
    ///            <chart:NumericalAxis ShowTrackballLabel="true"/>
    ///        </chart:SfCartesianChart.XAxes>
    ///
    ///        <chart:SfCartesianChart.YAxes>
    ///            <chart:NumericalAxis/>
    ///        </chart:SfCartesianChart.YAxes>
    ///
    ///        <chart:SfCartesianChart.Series>
    ///             <chart:LineSeries ItemsSource="{Binding Data}"
    ///                               XBindingPath="XValue"
    ///                               YBindingPath="YValue1"/>
    ///             <chart:LineSeries ItemsSource="{Binding Data}"
    ///                               XBindingPath="XValue"
    ///                               YBindingPath="YValue2"/>
    ///         </chart:SfCartesianChart.Series>
    /// </chart:SfCartesianChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-21)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    ///	
    /// chart.TrackballBehavior = new ChartTrackballBehavior();
    /// 
    /// NumericalAxis xaxis = new NumericalAxis();
    /// xaxis.ShowTrackballLabel = true;
    /// chart.XAxes.Add(xaxis);	
    /// 
    /// NumericalAxis yaxis = new NumericalAxis();
    /// chart.YAxes.Add(yaxis);
    /// 
    /// LineSeries series1 = new LineSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue1",
    /// };
    /// chart.Series.Add(series1);
    ///
    /// LineSeries series2 = new LineSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue2",
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    [ContentProperty(Name = "Series")]
    public class SfCartesianChart : ChartBase
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <see cref="PaletteBrushes"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PaletteBrushes</c> dependency property.
        /// </value>      
        public static readonly DependencyProperty PaletteBrushesProperty =
            DependencyProperty.Register(nameof(PaletteBrushes), typeof(IList<Brush>), typeof(SfCartesianChart), new PropertyMetadata(ChartColorModel.DefaultBrushes, OnPaletteBrushesChanged));


        /// <summary>
        /// Identifies the <see cref="Series"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Series</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(
                nameof(Series),
                typeof(CartesianSeriesCollection),
                typeof(SfCartesianChart),
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="IsTransposed"/> property.
        /// </summary>
        public static readonly DependencyProperty IsTransposedProperty =
            DependencyProperty.Register(nameof(IsTransposed), typeof(bool), typeof(SfCartesianChart),
            new PropertyMetadata(false, OnTransposeChanged));

        /// <summary>
        /// Identifies the <see cref="EnableSideBySideSeriesPlacement"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>EnableSideBySideSeriesPlacement</c> dependency property.
        /// </value>
        public static readonly DependencyProperty EnableSideBySideSeriesPlacementProperty =
            DependencyProperty.Register(
                nameof(EnableSideBySideSeriesPlacement),
                typeof(bool),
                typeof(SfCartesianChart),
                new PropertyMetadata(true, OnSideBySideSeriesPlacementProperty));

        /// <summary>
        /// Identifies the <see cref="PlotAreaBorderBrush"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PlotAreaBorderBrush</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PlotAreaBorderBrushProperty =
            DependencyProperty.Register(nameof(PlotAreaBorderBrush), typeof(Brush), typeof(SfCartesianChart), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="PlotAreaBorderThickness"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PlotAreaBorderThickness</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PlotAreaBorderThicknessProperty =
            DependencyProperty.Register(
                nameof(PlotAreaBorderThickness),
                typeof(Thickness),
                typeof(SfCartesianChart),
                new PropertyMetadata(new Thickness().GetThickness(0, 0, 0, 0)));

        /// <summary>
        /// Identifies the <see cref="PlotAreaBackground"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PlotAreaBackground</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PlotAreaBackgroundProperty =
            DependencyProperty.Register(nameof(PlotAreaBackground), typeof(Brush), typeof(SfCartesianChart), new PropertyMetadata(null));

        /// <summary>
        /// The DependencyProperty for <see cref="ZoomPanBehavior"/> property.
        /// </summary>
        public static readonly DependencyProperty ZoomPanBehaviorProperty =
            DependencyProperty.Register(
                nameof(ZoomPanBehavior),
                typeof(ChartZoomPanBehavior),
                typeof(SfCartesianChart),
                new PropertyMetadata(null, OnBehaviorChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="TrackballBehavior"/> property.
        /// </summary>
        public static readonly DependencyProperty TrackballBehaviorProperty =
            DependencyProperty.Register(
                nameof(TrackballBehavior),
                typeof(ChartTrackballBehavior),
                typeof(SfCartesianChart),
                new PropertyMetadata(null, OnBehaviorChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="CrosshairBehavior"/> property.
        /// </summary>
        public static readonly DependencyProperty CrosshairBehaviorProperty =
            DependencyProperty.Register(
                nameof(CrosshairBehavior),
                typeof(ChartCrosshairBehavior),
                typeof(SfCartesianChart),
                new PropertyMetadata(null, OnBehaviorChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="SelectionBehavior"/> property.
        /// </summary>
        public static readonly DependencyProperty SelectionBehaviorProperty =
            DependencyProperty.Register(
                nameof(SelectionBehavior),
                typeof(SeriesSelectionBehavior),
                typeof(SfCartesianChart),
                new PropertyMetadata(null, OnBehaviorChanged));

        #endregion

        #region Fields

        private ObservableCollection<ChartAxis> xAxes;
        private ObservableCollection<RangeAxisBase> yAxes;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfCartesianChart"/> class.
        /// </summary>
        public SfCartesianChart()
        {
#if NETCORE
#if SyncfusionLicense
           if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
               LicenseHelper.ValidateLicense();
            }
#endif
#endif

            DefaultStyleKey = typeof(SfCartesianChart);
            Series = new CartesianSeriesCollection();
            xAxes = new ObservableCollection<ChartAxis>();
            yAxes = new ObservableCollection<RangeAxisBase>();

            yAxes.CollectionChanged += YAxes_CollectionChanged;
            xAxes.CollectionChanged += XAxes_CollectionChanged;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value for initiating selection or highlighting of a single or multiple series in the chart.
        /// </summary>
        /// <value>This property takes a <see cref="SeriesSelectionBehavior"/> instance as a value, and its default value is null.</value>
        ///  <example>
        /// # [MainPage.xaml](#tab/tabid-22)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:DateTimeAxis/>
        ///         </chart:SfCartesianChart.XAxes>
        ///         <chart:SfCartesianChart.YAxes>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfCartesianChart.YAxes>
        ///         <chart:SfCartesianChart.SelectionBehavior>
        ///             <chart:SeriesSelectionBehavior/>
        ///         </chart:SfCartesianChart.SelectionBehavior>
        ///         <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="Date"
        ///                             YBindingPath="High"/>
        ///         <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="Date"
        ///                             YBindingPath="Low"/>
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-23)
        /// <code><![CDATA[
        ///  SfCartesianChart chart = new SfCartesianChart();
        ///  
        ///  ViewModel viewModel = new ViewModel();
        ///	 chart.DataContext = viewModel;
        /// 
        ///  chart.XAxes.Add(new DateTimeAxis())
        ///  chart.YAxes.Add(new NumericalAxis())
        /// 
        ///  chart.SelectionBehavior = new SeriesSelectionBehavior() {SelectionBrush = Colors.Blue};
        ///  
        ///  ColumnSeries series = new ColumnSeries()
        ///  {
        ///     ItemsSource = viewmodel.Data,
        ///     XBindingPath = "Date",
        ///     YBindingPath = "High"
        ///  };
        /// 
        /// ColumnSeries series2 = new ColumnSeries()
        ///  {
        ///     ItemsSource = viewmodel.Data,
        ///     XBindingPath = "Date",
        ///     YBindingPath = "Low"
        ///  };
        /// 
        ///  chart.Series.Add(series);
        ///  chart.Series.Add(series2);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public SeriesSelectionBehavior SelectionBehavior
        {
            get { return (SeriesSelectionBehavior)GetValue(SelectionBehaviorProperty); }
            set { SetValue(SelectionBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for initiating the zooming and panning operations in chart.
        /// </summary>
        /// <value>This property takes the <see cref="ChartZoomPanBehavior"/> value and its default value is null.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-24)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        /// 
        ///           <chart:SfCartesianChart.DataContext>
        ///               <local:ViewModel/>
        ///           </chart:SfCartesianChart.DataContext>
        /// 
        ///           <chart:SfCartesianChart.ZoomPanBehavior>
        ///               <chart:ChartZoomPanBehavior EnablePinchZooming="True" EnablePanning="True"/>
        ///           </chart:SfCartesianChart.ZoomPanBehavior>
        /// 
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.XAxes>
        /// 
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        /// 
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue1"/>
        /// 
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-25)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        /// 
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.DataContext = viewModel;
        /// 
        ///     chart.ZoomPanBehavior = new ChartZoomPanBehavior() { EnablePinchZooming = true, EnablePanning = true };
        /// 
        ///     NumericalAxis xaxis = new NumericalAxis();
        ///     chart.XAxes.Add(xaxis);
        /// 
        ///     NumericalAxis yaxis = new NumericalAxis();
        ///     chart.YAxes.Add(yaxis);
        /// 
        ///     LineSeries series = new LineSeries()
        ///     {
        ///        ItemsSource = viewModel.Data,
        ///        XBindingPath = "XValue",
        ///        YBindingPath = "YValue1",
        ///     };
        ///     chart.Series.Add(series);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartZoomPanBehavior ZoomPanBehavior
        {
            get { return (ChartZoomPanBehavior)GetValue(ZoomPanBehaviorProperty); }
            set { SetValue(ZoomPanBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that allows tracking a data point closer to the cursor position in the chart.
        /// </summary>
        /// <value>This property takes the <see cref="ChartTrackballBehavior"/> value and its default value is null.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-26)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        /// 
        ///           <chart:SfCartesianChart.DataContext>
        ///               <local:ViewModel/>
        ///           </chart:SfCartesianChart.DataContext>
        /// 
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior ShowLine="True" DisplayMode="GroupAllPoints"/>
        ///           </chart:SfCartesianChart.TrackballBehavior>
        /// 
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.XAxes>
        /// 
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        /// 
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue1"/>
        /// 
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-27)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        /// 
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.DataContext = viewModel;
        /// 
        ///     chart.TrackballBehavior = new ChartTrackballBehavior() { DisplayMode = TrackballLabelDisplayMode.GroupAllPoints, ShowLine = true };
        /// 
        ///     NumericalAxis xaxis = new NumericalAxis();
        ///     chart.XAxes.Add(xaxis);
        /// 
        ///     NumericalAxis yaxis = new NumericalAxis();
        ///     chart.YAxes.Add(yaxis);
        /// 
        ///     LineSeries series = new LineSeries()
        ///     {
        ///        ItemsSource = viewModel.Data,
        ///        XBindingPath = "XValue",
        ///        YBindingPath = "YValue1",
        ///     };
        ///     chart.Series.Add(series);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartTrackballBehavior TrackballBehavior
        {
            get { return (ChartTrackballBehavior)GetValue(TrackballBehaviorProperty); }
            set { SetValue(TrackballBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that allows the crosshair to display the cursor position in the chart.
        /// </summary>
        /// <value>This property takes the <see cref="ChartCrosshairBehavior"/> value and its default value is null.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        /// 
        ///           <chart:SfCartesianChart.DataContext>
        ///               <local:ViewModel/>
        ///           </chart:SfCartesianChart.DataContext>
        /// 
        ///           <chart:SfCartesianChart.CrosshairBehavior>
        ///               <chart:ChartCrosshairBehavior />
        ///           </chart:SfCartesianChart.CrosshairBehavior>
        /// 
        ///           <chart:SfCartesianChart.XAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.XAxes>
        /// 
        ///           <chart:SfCartesianChart.YAxes>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.YAxes>
        /// 
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue1"/>
        /// 
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        /// 
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.DataContext = viewModel;
        /// 
        ///     chart.CrosshairBehavior = new ChartCrosshairBehavior();
        /// 
        ///     NumericalAxis xaxis = new NumericalAxis();
        ///     chart.XAxes.Add(xaxis);
        /// 
        ///     NumericalAxis yaxis = new NumericalAxis();
        ///     chart.YAxes.Add(yaxis);
        /// 
        ///     LineSeries series = new LineSeries()
        ///     {
        ///        ItemsSource = viewModel.Data,
        ///        XBindingPath = "XValue",
        ///        YBindingPath = "YValue1",
        ///     };
        ///     chart.Series.Add(series);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartCrosshairBehavior CrosshairBehavior
        {
            get { return (ChartCrosshairBehavior)GetValue(CrosshairBehaviorProperty); }
            set { SetValue(CrosshairBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the palette brushes for chart.
        /// </summary>
        /// <value>This property takes the list of <see cref="Brush"/> and its default value is predefined palette.</value>
        public IList<Brush> PaletteBrushes
        {
            get { return (IList<Brush>)GetValue(PaletteBrushesProperty); }
            set { SetValue(PaletteBrushesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of chart series to be added to the cartesian chart.
        /// </summary>
        /// <value>This property takes <see cref="CartesianSeriesCollection"/> instance as value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-30)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///        <chart:SfCartesianChart.DataContext>
        ///            <local:ViewModel/>
        ///        </chart:SfCartesianChart.DataContext>
        ///
        ///        <chart:SfCartesianChart.XAxes>
        ///            <chart:NumericalAxis/>
        ///        </chart:SfCartesianChart.XAxes>
        ///
        ///        <chart:SfCartesianChart.YAxes>
        ///            <chart:NumericalAxis/>
        ///        </chart:SfCartesianChart.YAxes>
        ///
        ///        <chart:LineSeries ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue1"/>
        ///        <chart:LineSeries ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue2"/>
        /// 
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-31)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.DataContext = viewModel;
        /// 
        ///     NumericalAxis xaxis = new NumericalAxis();
        ///     chart.XAxes.Add(xaxis);
        /// 
        ///     NumericalAxis yaxis = new NumericalAxis();
        ///     chart.YAxes.Add(yaxis);
        /// 
        ///     LineSeries series1 = new LineSeries()
        ///     {
        ///         ItemsSource = viewModel.Data,
        ///         XBindingPath = "XValue",
        ///         YBindingPath = "YValue1"
        ///     };
        ///     chart.Series.Add(series1);
        ///     
        ///     LineSeries series2 = new LineSeries()
        ///     {
        ///         ItemsSource = viewModel.Data,
        ///         XBindingPath = "XValue",
        ///         YBindingPath = "YValue2"
        ///     };
        ///     chart.Series.Add(series2);
        ///     
        /// ]]>
        /// </code>
        /// # [ViewModel](#tab/tabid-32)
        /// <code><![CDATA[
        /// public ObservableCollection<Model> Data { get; set; }
        /// 
        /// public ViewModel()
        /// {
        ///    Data = new ObservableCollection<Model>();
        ///    Data.Add(new Model() { XValue = 10, YValue1 = 100, YValue2 = 110 });
        ///    Data.Add(new Model() { XValue = 20, YValue1 = 150, YValue2 = 100 });
        ///    Data.Add(new Model() { XValue = 30, YValue1 = 110, YValue2 = 130 });
        ///    Data.Add(new Model() { XValue = 40, YValue1 = 230, YValue2 = 180 });
        /// }
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        /// <remarks><para>To render a series, create an instance of required series class, and add it to the <see cref="Series"/> collection.</para></remarks>
        public CartesianSeriesCollection Series
        {
            get { return (CartesianSeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// Gets the collection of horizontal axes in the chart.
        /// </summary>
        /// <remarks>
        /// <b>Horizontal(X)</b> axis supports the <b>Category, Numeric and Date time</b>.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-33)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///         <chart:SfCartesianChart.DataContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCartesianChart.DataContext>
        /// 
        ///         <chart:SfCartesianChart.XAxes>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-34)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// ViewModel viewModel = new ViewModel();
        /// chart.DataContext = viewModel;
        /// 
        /// NumericalAxis xaxis = new NumericalAxis();
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        /// <value>Returns the collection of <see cref="ChartAxis"/>.</value>
        public ObservableCollection<ChartAxis> XAxes
        {
            get
            {
                return xAxes;
            }
        }

        /// <summary>
        /// Gets the collection of vertical axes in the chart.
        /// </summary>
        /// <remarks><b>Vertical(Y)</b> axis always uses numerical scale.</remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-35)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///         <chart:SfCartesianChart.DataContext>
        ///             <local:ViewModel/>
        ///         </chart:SfCartesianChart.DataContext>
        /// 
        ///         <chart:SfCartesianChart.YAxes>
        ///             <chart:NumericalAxis/>
        ///         </chart:SfCartesianChart.YAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-36)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// ViewModel viewModel = new ViewModel();
        /// chart.DataContext = viewModel;
        /// 
        /// NumericalAxis yaxis = new NumericalAxis();
        /// chart.YAxes.Add(yaxis);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        /// <value>Returns the collection of <see cref="RangeAxisBase"/>.</value>
        public ObservableCollection<RangeAxisBase> YAxes
        {
            get
            {
                return yAxes;
            }
        }

        /// <summary>
        /// Gets or sets the brush value to customize the outline appearance of the chart area.
        /// </summary>
        /// <value>
        /// It accepts <see cref="Brush"/> value.
        /// </value>
        public Brush PlotAreaBorderBrush
        {
            get { return (Brush)GetValue(PlotAreaBorderBrushProperty); }
            set { SetValue(PlotAreaBorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to customize the outline thickness of chart area.
        /// </summary>
        /// <value>
        /// It accepts <see cref="Thickness"/> value.
        /// </value>
        public Thickness PlotAreaBorderThickness
        {
            get { return (Thickness)GetValue(PlotAreaBorderThicknessProperty); }
            set { SetValue(PlotAreaBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush value to customize the background appearance of the chart area.
        /// </summary>
        /// <value>
        /// It accepts <see cref="Brush"/> value.
        /// </value>
        public Brush PlotAreaBackground
        {
            get { return (Brush)GetValue(PlotAreaBackgroundProperty); }
            set { SetValue(PlotAreaBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value that indicates whether to change the cartesian chart orientation.
        /// </summary>
        /// <value>This proeprty takes the boolean value and its default value is <c>false</c>.</value>
        /// <remarks>If the value is true, the orientation of x-axis is set to vertical and orientation of y-axis is set to horizontal.</remarks>
        public bool IsTransposed
        {
            get { return (bool)GetValue(IsTransposedProperty); }
            set { SetValue(IsTransposedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value that indicates whether the series are placed side by side or overlapped.
        /// </summary>
        /// <value>This proeprty takes the boolean value and its default value is <c>true</c>.</value>
        /// <remarks>If the value is true, series placed side by side, else series rendered one over other(overlapped).</remarks>
        public bool EnableSideBySideSeriesPlacement
        {
            get { return (bool)GetValue(EnableSideBySideSeriesPlacementProperty); }
            set { SetValue(EnableSideBySideSeriesPlacementProperty, value); }
        }
        #endregion

        #region Events

        /// <summary>
        /// Occurs when chart zooming value is changed.
        /// </summary>
        /// <value>The <see cref="ZoomChangedEventArgs"/> that contains the event data.</value>
        internal event EventHandler<ZoomChangedEventArgs> ZoomChanged;

        /// <summary>
        /// Occurs when zooming takes place in chart.
        /// </summary>
        /// <value>The <see cref="ZoomChangingEventArgs"/> that contains the event data.</value>
        internal event EventHandler<ZoomChangingEventArgs> ZoomChanging;

        /// <summary>
        /// Occurs at the start of selection zooming.
        /// </summary>
        /// <value>The <see cref="SelectionZoomingStartEventArgs"/> that contains the event data.</value>
        internal event EventHandler<SelectionZoomingStartEventArgs> SelectionZoomingStart;

        /// <summary>
        /// Occurs during selection zooming.
        /// </summary>
        /// <value>The <see cref="SelectionZoomingDeltaEventArgs"/> that contains the event data.</value>
        internal event EventHandler<SelectionZoomingDeltaEventArgs> SelectionZoomingDelta;

        /// <summary>
        /// Occurs at the end of selection zooming.
        /// </summary>
        /// <value>The <see cref="SelectionZoomingEndEventArgs"/> that contains the event data.</value>
        internal event EventHandler<SelectionZoomingEndEventArgs> SelectionZoomingEnd;

        /// <summary>
        /// Occurs when panning position of chart is changed.
        /// </summary>
        /// <value>The <see cref="PanChangedEventArgs"/> that contains the event data.</value>
        internal event EventHandler<PanChangedEventArgs> PanChanged;

        /// <summary>
        /// Occurs when panning takes place in chart.
        /// </summary>
        /// <value>The <see cref="PanChangingEventArgs"/> that contains the event data.</value>
        internal event EventHandler<PanChangingEventArgs> PanChanging;

        /// <summary>
        /// Occurs when the zoom is reset.
        /// </summary>
        /// <value>The <see cref="ResetZoomEventArgs"/> that contains the event data.</value>
        internal event EventHandler<ResetZoomEventArgs> ResetZooming;


        #endregion

        #region Internal override methods

        protected override void OnApplyTemplate()
        {
            CartesianPlotArea cartesianPlotArea = GetTemplateChild("CartesianPlotArea") as CartesianPlotArea;
            cartesianPlotArea.SeriesCollection = Series;
            PlotArea = cartesianPlotArea;

            base.OnApplyTemplate();
            
            (AreaPanel as CartesianAreaPanel).Chart = this;
        }

        internal override SeriesSelectionBehavior GetSeriesSelectionBehavior()
        {
            return SelectionBehavior;
        }

        internal override IList GetSeriesCollection()
        {
            return Series;
        }

        internal override ObservableCollection<ChartSeries> GetChartSeriesCollection()
        {
            return new ObservableCollection<ChartSeries>(Series);
        }

        internal override void UnHookSeriesCollection(IList seriesCollection)
        {
            if (seriesCollection is CartesianSeriesCollection)
            {
                (seriesCollection as CartesianSeriesCollection).CollectionChanged -= OnSeriesCollectionChanged;
            }
            else
            {
                base.UnHookSeriesCollection(seriesCollection);
            }
        }

        internal override void HookSeriesCollection(IList seriesCollection)
        {
            if (seriesCollection is CartesianSeriesCollection)
            {
                (seriesCollection as CartesianSeriesCollection).CollectionChanged += OnSeriesCollectionChanged;
            }
            else
            {
                base.HookSeriesCollection(seriesCollection);
            }
        }

        internal override void SetSeriesCollection(IList seriesCollection)
        {
            Series = (CartesianSeriesCollection)seriesCollection;
        }
   
        internal override bool SideBySideSeriesPlacement => EnableSideBySideSeriesPlacement;

        internal override void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            base.Dispose(disposing);

            if(XAxes != null)
                XAxes.CollectionChanged -= XAxes_CollectionChanged;

            if (YAxes != null)
                YAxes.CollectionChanged -= YAxes_CollectionChanged;

            SelectionBehavior = null;
        }

        internal override void DisposeZoomEvents()
        {
            if (ZoomChanged != null)
            {
                foreach (var handler in ZoomChanged.GetInvocationList())
                {
                    ZoomChanged -= handler as EventHandler<ZoomChangedEventArgs>;
                }

                ZoomChanged = null;
            }

            if (ZoomChanging != null)
            {
                foreach (var handler in ZoomChanging.GetInvocationList())
                {
                    ZoomChanging -= handler as EventHandler<ZoomChangingEventArgs>;
                }

                ZoomChanging = null;
            }

            if (SelectionZoomingStart != null)
            {
                foreach (var handler in SelectionZoomingStart.GetInvocationList())
                {
                    SelectionZoomingStart -= handler as EventHandler<SelectionZoomingStartEventArgs>;
                }

                SelectionZoomingStart = null;
            }

            if (SelectionZoomingDelta != null)
            {
                foreach (var handler in SelectionZoomingDelta.GetInvocationList())
                {
                    SelectionZoomingDelta -= handler as EventHandler<SelectionZoomingDeltaEventArgs>;
                }

                SelectionZoomingDelta = null;
            }

            if (SelectionZoomingEnd != null)
            {
                foreach (var handler in SelectionZoomingEnd.GetInvocationList())
                {
                    SelectionZoomingEnd -= handler as EventHandler<SelectionZoomingEndEventArgs>;
                }

                SelectionZoomingEnd = null;
            }

            if (PanChanged != null)
            {
                foreach (var handler in PanChanged.GetInvocationList())
                {
                    PanChanged -= handler as EventHandler<PanChangedEventArgs>;
                }

                PanChanged = null;
            }

            if (PanChanging != null)
            {
                foreach (var handler in PanChanging.GetInvocationList())
                {
                    PanChanging -= handler as EventHandler<PanChangingEventArgs>;
                }

                PanChanging = null;
            }

            if (ResetZooming != null)
            {
                foreach (var handler in ResetZooming.GetInvocationList())
                {
                    ResetZooming -= handler as EventHandler<ResetZoomEventArgs>;
                }

                ResetZooming = null;
            }
        }
        #endregion

        #region Protected Internal Virtual Methods

        /// <summary>
        /// Occurs when zooming position changed in chart.
        /// </summary>
        /// <param name="args">The <see cref="ZoomChangedEventArgs"/> that contains the event data.</param>
        internal override void OnZoomChanged(ZoomChangedEventArgs args)
        {
            if (ZoomChanged != null && args != null)
            {
                ZoomChanged(this, args);
            }
        }

        /// <summary>
        /// Occurs when zooming takes place in chart.
        /// </summary>
        /// <param name="args">The <see cref="ZoomChangingEventArgs"/> that contains the event data.</param>
        internal override void OnZoomChanging(ZoomChangingEventArgs args)
        {
            if (ZoomChanging != null && args != null)
            {
                ZoomChanging(this, args);
            }
        }

        /// <summary>
        /// Occurs at the start of selection zooming.
        /// </summary>
        /// <param name="args">The <see cref="SelectionZoomingStartEventArgs"/> that contains the event data.</param>
        internal override void OnSelectionZoomingStart(SelectionZoomingStartEventArgs args)
        {
            if (SelectionZoomingStart != null && args != null)
            {
                SelectionZoomingStart(this, args);
            }
        }

        /// <summary>
        /// Occurs at the end of selection zooming.
        /// </summary>
        /// <param name="args">The <see cref="SelectionZoomingEndEventArgs"/> that contains the event data.</param>
        internal override void OnSelectionZoomingEnd(SelectionZoomingEndEventArgs args)
        {
            if (SelectionZoomingEnd != null && args != null)
            {
                SelectionZoomingEnd(this, args);
            }
        }

        /// <summary>
        /// Occurs while selection zooming in chart.
        /// </summary>
        /// <param name="args">The <see cref="SelectionZoomingDeltaEventArgs"/> that contains the event data.</param>
        internal override void OnSelectionZoomingDelta(SelectionZoomingDeltaEventArgs args)
        {
            if (SelectionZoomingDelta != null && args != null)
            {
                SelectionZoomingDelta(this, args);
            }
        }

        /// <summary>
        /// Occurs when panning position changed in chart.
        /// </summary>
        /// <param name="args">The <see cref="PanChangedEventArgs"/> that contains the event data.</param>
        internal override void OnPanChanged(PanChangedEventArgs args)
        {
            if (PanChanged != null && args != null)
            {
                PanChanged(this, args);
            }
        }

        /// <summary>
        /// Occurs when panning takes place in chart.
        /// </summary>
        /// <param name="args">The <see cref="PanChangingEventArgs"/> that contains the event data.</param>
        internal override void OnPanChanging(PanChangingEventArgs args)
        {
            if (PanChanging != null && args != null)
            {
                PanChanging(this, args);
            }
        }

        /// <summary>
        /// Occurs when zooming is reset.
        /// </summary>
        /// <param name="args">The <see cref="ResetZoomEventArgs"/> that contains the event data.</param>
        internal override void OnResetZoom(ResetZoomEventArgs args)
        {
            if (ResetZooming != null && args != null)
            {
                ResetZooming(this, args);
            }
        }

        #endregion

        #region Internal Methods

        internal void UpdateActualAxis(ChartSeries series)
        {
            if (series is CartesianSeries cartesianSeries)
            {
                if (cartesianSeries.ActualXAxis == null)
                {
                    var xName = cartesianSeries.XAxisName;
                    var axis = string.IsNullOrEmpty(xName) ? InternalPrimaryAxis : cartesianSeries.GetXAxisByName(xName, XAxes) ?? InternalPrimaryAxis;
                    cartesianSeries.ActualXAxis = (ChartAxis)axis;
                }

                if (cartesianSeries.ActualYAxis == null)
                {
                    var yName = cartesianSeries.YAxisName;
                    var axis = string.IsNullOrEmpty(yName) ? InternalSecondaryAxis : cartesianSeries.GetYAxisByName(yName, YAxes) ?? InternalSecondaryAxis;
                    cartesianSeries.ActualYAxis = (RangeAxisBase)axis;
                }

                cartesianSeries.UpdateAssociatedAxes();
            }
        }

        internal override Brush GetPaletteBrush(int index)
        {
            if (this.PaletteBrushes != null)
                return this.PaletteBrushes[index % this.PaletteBrushes.Count()];

            return new SolidColorBrush(Colors.Transparent);
        }

        internal override bool IsNullPaletteBrushes()
        {
            if (this.PaletteBrushes == null)
                return true;

            return false;
        }

        private static void OnPaletteBrushesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SfCartesianChart)d).OnPaletteBrushesChanged(e);
        }

        private void OnPaletteBrushesChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.VisibleSeries.Count > 0)//ColorModel custom brush dynamic update not working in native control.-WP-610
            {
                for (int index = 0; index < VisibleSeries.Count; index++)
                {
                    (this.VisibleSeries[index] as ChartSeries).Segments.Clear();
                }

                if (PlotArea != null)
                    PlotArea.ShouldPopulateLegendItems = true;
                ScheduleUpdate();
            }
        }

        #endregion

        #region Private Methods

        // TODO : While adding public setter for XAxes and YAxes, we need to reuse this code. 
        //private static void OnXAxesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        //{
        //    if (d is SfCartesianChart cartesianChart)
        //    {
        //        if (args.OldValue is ChartAxisCollection oldCollection)
        //        {
        //            oldCollection.CollectionChanged -= cartesianChart.XAxes_CollectionChanged;
        //            cartesianChart.RemoveOldAxisCollection(oldCollection);
        //        }

        //        if (args.NewValue is ChartAxisCollection newCollection)
        //        {
        //            newCollection.CollectionChanged += cartesianChart.XAxes_CollectionChanged;

        //            int index = 0;
        //            foreach (ChartAxis axis in newCollection)
        //            {
        //                cartesianChart.AddXAxis(index, axis);
        //                index++;
        //            }
        //        }
        //    }
        //}

        //private static void OnYAxesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        //{
        //    if (d is SfCartesianChart cartesianChart)
        //    {
        //        if (args.OldValue is RangeAxisCollection oldCollection)
        //        {
        //            oldCollection.CollectionChanged -= cartesianChart.YAxes_CollectionChanged;
        //            cartesianChart.RemoveOldAxisCollection(oldCollection);
        //        }

        //        if (args.NewValue is RangeAxisCollection newCollection)
        //        {
        //            newCollection.CollectionChanged += cartesianChart.YAxes_CollectionChanged;

        //            int index = 0;
        //            foreach (ChartAxis axis in newCollection)
        //            {
        //                cartesianChart.AddYAxis(index, axis);
        //                index++;
        //            }
        //        }
        //    }
        //}

        private void XAxes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((index, series) => AddXAxis(index, series), (index, series) => RemoveXAxis(index, series), ResetXAxes);
        }

        private void YAxes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((index, series) => AddYAxis(index, series), (index, series) => RemoveYAxis(index, series), ResetYAxes);
        }

        private void RemoveOldAxisCollection(IList oldCollection)
        {
            foreach (ChartAxis axis in oldCollection)
            {
                if (InternalAxes != null && InternalAxes.Contains(axis))
                {
                    InternalAxes.RemoveItem(axis, DependentSeriesAxes.Contains(axis));

                    DependentSeriesAxes.Remove(axis);
                }

                axis.VisibleRangeChanged -= axis.OnVisibleRangeChanged;
                axis.RegisteredSeries.Clear();
                InternalAxes.Remove(axis);
                axis.Dispose();
            }
        }

        private static void OnTransposeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SfCartesianChart chart)
            {
                if (chart.XAxes != null)
                    foreach (var axis in chart.XAxes)
                        axis.IsVertical = (bool)e.NewValue ? true : false;
                
                if (chart.YAxes != null)
                    foreach (var axis in chart.YAxes)
                        axis.IsVertical = (bool)e.NewValue ? false : true;

                chart.ScheduleUpdate();
            }
        }

        private void AddRegisterSeries(ChartAxis chartAxis, CartesianSeries series)
        {
            if (chartAxis != null && !chartAxis.RegisteredSeries.Contains(series))
                chartAxis.RegisteredSeries.Add(series);
        }

        private void AddXAxis(int index, object obj)
        {
            if (obj is ChartAxis chartAxis)
            {
                if (index == 0)
                    InternalPrimaryAxis = chartAxis;
                else
                    index = InternalAxes.IndexOf(XAxes[index - 1]) + 1;

                chartAxis.VisibleRangeChanged += chartAxis.OnVisibleRangeChanged;
                chartAxis.IsVertical = IsTransposed ? true : false;
                InternalAxes.Insert(index, chartAxis);
                DependentSeriesAxes.Add(chartAxis);

                if (Series != null)
                {
                    foreach (var series in Series)
                    {
                        var xName = series.XAxisName;
                        var xAxis = string.IsNullOrEmpty(xName) ? InternalPrimaryAxis : xName == chartAxis.Name ? chartAxis : series.ActualXAxis != null ? series.ActualXAxis : InternalPrimaryAxis;
                        series.ActualXAxis = xAxis;
                        AddRegisterSeries(xAxis, series);
                    }
                }
            }
        }

        private void RemoveXAxis(int index, object obj)
        {
            if (obj is ChartAxis oldAxis)
            {
                if (index == 0 && XAxes?.Count > 0)
                    InternalPrimaryAxis = XAxes[0];

                var seriesCollection = Series.Where(x => x.ActualXAxis == oldAxis);
                foreach (var series in seriesCollection)
                {
                    series.ActualXAxis = InternalPrimaryAxis;
                    AddRegisterSeries(InternalPrimaryAxis, series);
                }

                RemoveOldAxisCollection(new List<ChartAxis>() { oldAxis });
            }

            if (XAxes == null || XAxes?.Count == 0)
                ResetXAxes();
        }

        private void ResetXAxes()
        {
            var isvertical = IsTransposed ? true : false;
            var collection = InternalAxes.Where(x => x.IsVertical == isvertical).ToList();
            RemoveOldAxisCollection(collection);
            InternalPrimaryAxis = null;

            foreach (var series in Series)
            {
                series.ResetXAxis();
            }
        }

        private void AddYAxis(int index, object obj)
        {
            if (obj is ChartAxis chartAxis)
            {
                if (index == 0)
                    InternalSecondaryAxis = chartAxis;
                else
                    index = InternalAxes.IndexOf(YAxes[index - 1]) + 1;

                chartAxis.VisibleRangeChanged += chartAxis.OnVisibleRangeChanged;
                chartAxis.IsVertical = IsTransposed ? false : true;
                InternalAxes.Insert(index, chartAxis);
                DependentSeriesAxes.Add(chartAxis);

                if (Series != null)
                {
                    foreach (var series in Series)
                    {
                        var yName = series.YAxisName;
                        var yAxis = string.IsNullOrEmpty(yName) ? InternalSecondaryAxis : yName == chartAxis.Name ? chartAxis : series.ActualYAxis != null ? series.ActualYAxis : InternalSecondaryAxis;
                        series.ActualYAxis = yAxis;
                        AddRegisterSeries(yAxis, series);
                    }
                }
            }
        }

        private void RemoveYAxis(int index, object obj)
        {
            if (obj is ChartAxis oldAxis)
            {
                if (index == 0 && YAxes?.Count > 0)
                {
                    InternalSecondaryAxis = YAxes[0];
                }

                var seriesCollection = Series.Where(x => x.ActualYAxis == oldAxis);
                foreach (var series in seriesCollection)
                {
                    series.ActualYAxis = InternalSecondaryAxis;
                    AddRegisterSeries(InternalSecondaryAxis, series);
                }

                RemoveOldAxisCollection(new List<ChartAxis>() { oldAxis });
            }

            if (YAxes == null || YAxes?.Count == 0)
                ResetYAxes();
        }

        private void ResetYAxes()
        {
            var isvertical = IsTransposed ? false : true;
            var collection = InternalAxes.Where(x => x.IsVertical == isvertical).ToList();
            RemoveOldAxisCollection(collection);
            InternalSecondaryAxis = null;

            foreach (var series in Series)
            {
                series.ResetYAxis();
            }
        }

        #endregion
    }
}
