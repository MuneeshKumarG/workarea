using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Core;
using Syncfusion.Maui.Core.Internals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Point = Microsoft.Maui.Graphics.Point;
using PointerEventArgs = Syncfusion.Maui.Core.Internals.PointerEventArgs;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Provides the funnel chart with a unique style of data representation that is more UI-visualising and user-friendly.
    /// </summary>
    /// <remarks>
    /// <para>The streamlined data is displayed using a funnel chart, where each slice of the funnel indicates a process that has filtered out data.</para>
    /// <para>SfFunnelChart class allows to customize the chart elements such as legend, data label, and tooltip features.</para>
    /// 
    ///  # [MainPage.xaml](#tab/tabid-1)
    /// <code> <![CDATA[
    /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
    ///                      XBindingPath="XValue" 
    ///                      YBindingPath="YValue">
    ///      <chart:SfFunnelChart.BindingContext>
    ///          <model:ChartViewModel/>
    ///      </chart:SfFunnelChart.BindingContext>
    /// </chart:SfFunnelChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfFunnelChart chart = new SfFunnelChart();
    /// ChartViewModel viewModel = new ChartViewModel();
    /// chart.ItemsSource = viewModel.Data;
    /// chart.XBindingPath = "XValue";
    /// chart.YBindingPath = "YValue";
    /// ]]>
    /// </code>
    /// # [ViewModel.cs](#tab/tabid-3)
    /// <code><![CDATA[
    /// public ObservableCollection<Model> Data { get; set; }
    /// 
    /// public ViewModel()
    /// {
    ///    Data = new ObservableCollection<Model>();
    ///    Data.Add(new Model() { XValue = "A", YValue = 18 });
    ///    Data.Add(new Model() { XValue = "B", YValue = 34 });
    ///    Data.Add(new Model() { XValue = "C", YValue = 52 });
    ///    Data.Add(new Model() { XValue = "D", YValue = 68 });
    ///    Data.Add(new Model() { XValue = "E", YValue = 100 });
    /// }
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Legend</b></para>
    /// 
    /// <para>The Legend contains list of data points in chart. The information provided in each legend item helps to identify the corresponding data point in funnel chart. The chart <see cref="SfFunnelChart.XBindingPath"/> property value will be displayed in the associated legend item.</para>
    /// 
    /// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="ChartBase.Legend"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-4)
    /// <code> <![CDATA[
    /// <chart:SfFunnelChart ItemsSource = "{Binding Data}"
    ///                      XBindingPath="XValue" 
    ///                      YBindingPath="YValue">
    ///   <chart:SfFunnelChart.Legend>
    ///        <chart:ChartLegend/>
    ///   </chart:SfFunnelChart.Legend>
    /// </chart:SfFunnelChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-5)
    /// <code><![CDATA[
    /// SfFunnelChart chart = new SfFunnelChart();
    /// chart.Legend = new ChartLegend();
    /// ChartViewModel viewModel = new ChartViewModel();
    /// chart.ItemsSource = viewModel.Data;
    /// chart.XBindingPath = "XValue";
    /// chart.YBindingPath = "YValue";
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Tooltip</b></para>
    /// 
    /// <para>Tooltip displays information while tapping or mouse hover on the segment. To display the tooltip on the chart, you need to set the <see cref="SfFunnelChart.EnableTooltip"/> property as <b>true</b> in <see cref="SfFunnelChart"/>. </para>
    /// 
    /// <para>To customize the appearance of the tooltip elements like Background, TextColor and Font, create an instance of <see cref="ChartTooltipBehavior"/> class, modify the values, and assign it to the chart’s <see cref="ChartBase.TooltipBehavior"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-6)
    /// <code><![CDATA[
    /// <chart:SfFunnelChart ItemsSource = "{Binding Data}"
    ///                      XBindingPath="XValue" 
    ///                      YBindingPath="YValue"
    ///                      EnableTooltip="True">
    /// </chart:SfFunnelChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-7)
    /// <code><![CDATA[
    /// SfFunnelChart chart = new SfFunnelChart();
    /// ChartViewModel viewModel = new ChartViewModel();
    /// chart.ItemsSource = viewModel.Data;
    /// chart.XBindingPath = "XValue";
    /// chart.YBindingPath = "YValue";
    /// chart.EnableTooltip=true;
    ///
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Data Label</b></para>
    /// 
    /// <para>Data labels are used to display values related to a chart segment. To render the data labels, you need to enable the <see cref="SfFunnelChart.ShowDataLabels"/> property as <b>true</b> in <see cref="SfFunnelChart"/> class. </para>
    /// 
    /// <para>To customize the chart data labels alignment, placement and label styles, need to create an instance of <see cref="FunnelDataLabelSettings"/> and set to the <see cref="SfFunnelChart.DataLabelSettings"/> property.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-8)
    /// <code><![CDATA[
    /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
    ///                      XBindingPath="XValue" 
    ///                      YBindingPath="YValue"
    ///                      ShowDataLabels="True">
    /// </chart:SfFunnelChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-9)
    /// <code><![CDATA[
    /// SfFunnelChart chart = new SfFunnelChart();
    /// ChartViewModel viewModel = new ChartViewModel();
    /// chart.ItemsSource = viewModel.Data;
    /// chart.XBindingPath = "XValue";
    /// chart.YBindingPath = "YValue";
    /// chart.ShowDataLabels=true;
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    public class SfFunnelChart : ChartBase, IPyramidChartDependent, ITapGestureListener, ITouchListener
    {

        #region Private fields

        private readonly ObservableCollection<ChartSegment> segments;
        private ChartValueType xValueType;
        private readonly PyramidChartArea chartArea;
        private readonly PyramidDataLabelHelper dataLabelHelper;
        private bool segmentsCreated = false;

        #region ItemsSource related fields

        private bool isComplexYProperty;
        private delegate object? GetReflectedProperty(object obj, string[] paths);
        private IEnumerable? actualXValues;
        private string[][]? yComplexPaths;
        private int pointsCount;
        private List<object>? actualData;
        private bool isLinearData = true;
        private double xData;
        private string[]? xComplexPaths;
        private IEnumerable? xValues;
        private IList<double> yValues;
        internal Rect seriesBounds;
        private string[]? yPaths;

        #endregion
        #endregion

        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(object), typeof(SfFunnelChart), null, BindingMode.Default, null, propertyChanged: OnItemsSourceChanged);

        /// <summary>
        /// Identifies the <see cref="YBindingPath"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty YBindingPathProperty =
            BindableProperty.Create(nameof(YBindingPath), typeof(string), typeof(SfFunnelChart), null, BindingMode.Default, null, propertyChanged: OnYBindingPathChanged);

        /// <summary>
        /// Identifies the <see cref="XBindingPath"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty XBindingPathProperty =
         BindableProperty.Create(nameof(XBindingPath), typeof(string), typeof(SfFunnelChart), null, BindingMode.Default, null, propertyChanged: OnXBindingPathChanged);

        /// <summary>
        /// Identifies the <see cref="PaletteBrushes"/> bindable property.
        /// </summary>           
        public static readonly BindableProperty PaletteBrushesProperty = PyramidChartBase.PaletteBrushesProperty;

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeProperty = PyramidChartBase.StrokeProperty;

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeWidthProperty = PyramidChartBase.StrokeWidthProperty;

        /// <summary>
        /// Identifies the <see cref="LegendIcon"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty LegendIconProperty = PyramidChartBase.LegendIconProperty;

        /// <summary>
        /// Identifies the <see cref="TooltipTemplate"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty TooltipTemplateProperty = PyramidChartBase.TooltipTemplateProperty;

        /// <summary>
        /// Identifies the <see cref="EnableTooltip"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EnableTooltipProperty = PyramidChartBase.EnableTooltipProperty;

        /// <summary>
        /// Identifies the <see cref="SelectionBehavior"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty SelectionBehaviorProperty = PyramidChartBase.SelectionBehaviorProperty;

        /// <summary>
        /// Identifies the <see cref="ShowDataLabels"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ShowDataLabelsProperty = PyramidChartBase.ShowDataLabelsProperty;

        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty DataLabelSettingsProperty =
       BindableProperty.Create(nameof(DataLabelSettings), typeof(FunnelDataLabelSettings), typeof(SfFunnelChart), null, BindingMode.Default, null, OnDataLabelSettingsChanged);

        /// <summary>
        /// Identifies the <see cref="GapRatio"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty GapRatioProperty = PyramidChartBase.GapRatioProperty;
       
        internal static readonly BindableProperty NeckWidthProperty =
           BindableProperty.Create(nameof(NeckWidth), typeof(double), typeof(SfFunnelChart), 40d, BindingMode.Default, null, propertyChanged: NeckWidthChanged);
       
        internal static readonly BindableProperty NeckHeightProperty =
           BindableProperty.Create(nameof(NeckHeight), typeof(double), typeof(SfFunnelChart), 0d, BindingMode.Default, null, propertyChanged: NeckHeightChanged);

        #endregion

        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the <see cref="SfFunnelChart"/> class.
        /// </summary>
        public SfFunnelChart()
        {
            this.ValidateLicense();
            DataLabelSettings = new FunnelDataLabelSettings();
            PaletteBrushes = ChartColorModel.DefaultBrushes;
            chartArea = (PyramidChartArea)LegendLayout.AreaBase;
            dataLabelHelper = new PyramidDataLabelHelper(this);
            yValues = new List<double>();
            segments = new ObservableCollection<ChartSegment>();
            PyramidChartBase.InvokeSegmentsCollectionChanged(this, segments);
            this.AddGestureListener(this);
            this.AddTouchListener(this);
        }

        internal override AreaBase CreateChartArea()
        {
            return new PyramidChartArea(this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a data points collection that will be used to plot a chart.
        /// </summary>
        /// <value>It accepts the data points collections and its default value is null.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-10)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue">
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-11)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///     
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a x value to the chart.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the x plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-12)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue">
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-13)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string XBindingPath
        {
            get { return (string)GetValue(XBindingPathProperty); }
            set { SetValue(XBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a y value to the chart.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the y plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-14)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue">
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-15)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string YBindingPath
        {
            get { return (string)GetValue(YBindingPathProperty); }
            set { SetValue(YBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the list of brushes that can be used to customize the appearance of the chart.
        /// </summary>
        /// <remarks>It allows custom brushes, and gradient brushes to customize the appearance.</remarks>
        /// <value>This property accepts a list of brushes as input and comes with a set of predefined brushes by default.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-16)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}"  
        ///                      XBindingPath="Category"
        ///                      YBindingPath="Value"
        ///                      PaletteBrushes="{Binding CustomBrushes}">
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-17)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        /// ViewModel viewModel = new ViewModel();
        /// List<Brush> CustomBrushes = new List<Brush>();
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(77, 208, 225)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(38, 198, 218)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 188, 212)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 172, 193)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 151, 167)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromRgb(0, 131, 143)));
        ///
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "Category";
        /// chart.YBindingPath = "Value";
        /// chart.PaletteBrushes = CustomBrushes;
        /// 
        /// this.Content = chart;
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public IList<Brush> PaletteBrushes
        {
            get { return (IList<Brush>)GetValue(PaletteBrushesProperty); }
            set { SetValue(PaletteBrushesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color used to paint the funnel segments' outline.
        /// </summary>
        /// <value>Its default value is <c>Transparent</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-18)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}"
        ///                      XBindingPath="Category"
        ///                      YBindingPath="Value"
        ///                      Stroke="Red">
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-19)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        /// ViewModel viewModel = new ViewModel();
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "Category";
        /// chart.YBindingPath = "Value";
        /// chart.Stroke = Colors.Red;
        /// 
        /// this.Content = chart;
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
        /// Gets or sets a value to specify the width of the stroke drawn.
        /// </summary>
        /// <value>Its default value is 2.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-20)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}"
        ///                      XBindingPath="Category"
        ///                      YBindingPath="Value"
        ///                      Stroke="Red"
        ///                      StrokeWidth="4">
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-21)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        /// ViewModel viewModel = new ViewModel();
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "Category";
        /// chart.YBindingPath = "Value";
        /// chart.Stroke = Colors.Red;
        /// chart.StrokeWidth = 4;
        /// 
        /// this.Content = chart;
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
		}

        /// <summary>
        /// Gets or sets a legend icon that will be displayed in the associated legend item.
        /// </summary>
        /// <value>This property takes the list of <see cref="ChartLegendIconType"/> and its default value is <see cref="ChartLegendIconType.Circle"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-22)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}"
        ///                      XBindingPath="Category"
        ///                      YBindingPath="Value"
        ///                      LegendIcon="Diamond">
        ///    <chart:SfFunnelChart.Legend>
        ///        <chart:ChartLegend />
        ///    </chart:SfFunnelChart.Legend>
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-23)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        /// ViewModel viewModel = new ViewModel();
        /// chart.Legend = new ChartLegend();
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "Category";
        /// chart.YBindingPath = "Value";
        /// chart.LegendIcon = ChartLegendIconType.Diamond;
        /// 
        /// this.Content = chart;
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        public ChartLegendIconType LegendIcon
        {
            get { return (ChartLegendIconType)GetValue(LegendIconProperty); }
            set { SetValue(LegendIconProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate that can be used to customize the appearance of the tooltip.
        /// </summary>
        /// <value>Its default value is null.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-24)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}"
        ///                      XBindingPath="Category"
        ///                      YBindingPath="Value"
        ///                      EnableTooltip="True">
        ///    <chart:SfFunnelChart.TooltipTemplate>
        ///        <DataTemplate>
        ///            <Border Background = "DarkGreen"
        ///                    StrokeThickness="2" Stroke="Black" >
        ///            <Label Text = "{Binding Item.YValue}"
        ///                   TextColor="White" FontAttributes="Bold"
        ///                   HorizontalOptions="Center" VerticalOptions="Center"/>
        ///            </Border>
        ///        </DataTemplate>
        ///    </chart:SfFunnelChart.TooltipTemplate>
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        public DataTemplate TooltipTemplate
        {
            get { return (DataTemplate)GetValue(TooltipTemplateProperty); }
            set { SetValue(TooltipTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a Boolean value indicating whether the tooltip for chart should be shown or hidden.
        /// </summary>
        /// <remarks>The tooltip will appear when you mouse over or tap on the funnel segments.</remarks>
        /// <value>Its default value is <c>False</c>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-25)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}"
        ///                      XBindingPath="Category"
        ///                      YBindingPath="Value"
        ///                      EnableTooltip="True">
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-26)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        /// ViewModel viewModel = new ViewModel();
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "Category";
        /// chart.YBindingPath = "Value";
        /// chart.EnableTooltip= true;
        /// 
        /// this.Content = chart;
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        public bool EnableTooltip
        {
            get { return (bool)GetValue(EnableTooltipProperty); }
            set { SetValue(EnableTooltipProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for initiating selection or highlighting of a single or multiple data points in the chart.
        /// </summary>
        /// <value>Its default value is null.</value>
        /// 
        /// <remarks>
        /// To highlight the selected segment, set the value for the <see cref="ChartSelectionBehavior.SelectionBrush"/> property in the <see cref="DataPointSelectionBehavior"/> class.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-27)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource = "{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue">
        ///     <chart:SfFunnelChart.SelectionBehavior>
        ///          <chart:DataPointSelectionBehavior SelectionBrush="Red" />
        ///     </chart:SfFunnelChart.SelectionBehavior>
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-28)
        /// <code><![CDATA[
        /// ViewModel viewModel = new ViewModel();
        /// 
        /// SfFunnelChart chart = new SfFunnelChart();
        /// chart.DataContext = viewModel;
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.SelectionBehavior = new DataPointSelectionBehavior()
        /// {
        ///     SelectionBrush = new SolidColorBrush(Colors.Red),
        /// };
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        /// <seealso cref="ChartSelectionBehavior.SelectedIndex"/>
        /// <seealso cref="ChartSelectionBehavior.SelectionBrush"/>
        /// <seealso cref="ChartSelectionBehavior.SelectionChanging"/>
        /// <seealso cref="ChartSelectionBehavior.SelectionChanged"/>
        public DataPointSelectionBehavior SelectionBehavior
        {
            get { return (DataPointSelectionBehavior)GetValue(SelectionBehaviorProperty); }
            set { SetValue(SelectionBehaviorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates to enable the data labels for the chart.
        /// </summary>
        /// <value>Its default value is False.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-29)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      ShowDataLabels="True">
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-30)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///     
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.ShowDataLabels = true;
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowDataLabels
        {
            get { return (bool)GetValue(ShowDataLabelsProperty); }
            set { SetValue(ShowDataLabelsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the appearance of the displaying data labels in the chart.
        /// </summary>
        /// <remarks> This allows us to customize the appearance and position of data label.</remarks>
        /// <value>
        /// It takes the <see cref="FunnelDataLabelSettings"/>.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-31)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      ShowDataLabels="True">
        ///
        ///     <chart:SfFunnelChart.DataLabelSettings>
        ///         <chart:FunnelDataLabelSettings />
        ///     </chart:SfFunnelChart.DataLabelSettings>
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-32)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewmodel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.ShowDataLabels = true;
        /// chart.DataLabelSettings = new FunnelDataLabelSettings();
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public FunnelDataLabelSettings DataLabelSettings
        {
            get { return (FunnelDataLabelSettings)GetValue(DataLabelSettingsProperty); }
            set { SetValue(DataLabelSettingsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ratio of the distance between the chart segments.
        /// </summary>
        /// <value>Its default value is 0. Its value ranges from 0 to 1.</value>
        /// <remarks>It is used to provide the spacing between the segments</remarks>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-33)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      GapRatio="0.3">
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-34)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///
        /// ViewModel viewModel = new ViewModel();
        ///
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.GapRatio = 0.3;
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double GapRatio
        {
            get { return (double)GetValue(GapRatioProperty); }
            set { SetValue(GapRatioProperty, value); }
        }

        internal double NeckWidth
        {
            get { return (double)GetValue(NeckWidthProperty); }
            set { SetValue(NeckWidthProperty, value); }
        }

        internal double NeckHeight
        {
            get { return (double)GetValue(NeckHeightProperty); }
            set { SetValue(NeckHeightProperty, value); }
        }

        #endregion

        #region Methods

        #region Interface Implementation

        //GestureListener interface implementation
        void ITapGestureListener.OnTap(TapEventArgs e)
        {
            OnTapAction(this, e.TapPoint, e.TapCount);
        }

        void ITouchListener.OnTouch(PointerEventArgs e)
        {
            Point point = e.TouchPoint;

            switch (e.Action)
            {
                case PointerActions.Moved:
                    OnTouchMove(this, point, e.PointerDeviceType);
                    break;
            }
        }

        //IDatapointSelectionDependent interfacae implementation
        ObservableCollection<ChartSegment> IDatapointSelectionDependent.Segments => segments;

        Rect IDatapointSelectionDependent.AreaBounds => this is IChart chart ? chart.ActualSeriesClipRect : Rect.Zero;

        //IPyramidChartDependent  interfacae implementation

        IPyramidDataLabelSettings IPyramidChartDependent.DataLabelSettings => DataLabelSettings ?? new FunnelDataLabelSettings();
        bool IPyramidChartDependent.ArrangeReverse => true;

        PyramidDataLabelHelper IPyramidChartDependent.DataLabelHelper => dataLabelHelper;
        AreaBase IPyramidChartDependent.Area => chartArea;
        ChartLegend? IPyramidChartDependent.ChartLegend => Legend;

        Rect IPyramidChartDependent.SeriesBounds
        {
            get { return seriesBounds; }

            set
            {
                seriesBounds = value;
            }
        }

        bool IPyramidChartDependent.SegmentsCreated
        {
            get { return segmentsCreated; }

            set
            {
                segmentsCreated = value;
            }
        }

        void IPyramidChartDependent.UpdateLegendItemsSource(ObservableCollection<ILegendItem> legendItems)
        {
            if (Legend == null || !Legend.IsVisible)
            {
                return;
            }

            legendItems.Clear();

            for (int i = pointsCount - 1; i >= 0; i--)
            {
                var legendItem = new LegendItem();
                legendItem.IconType = ChartUtils.GetShapeType(LegendIcon);
                Brush? solidColor = PyramidChartBase.GetFillColor(this, i);
                legendItem.IconBrush = solidColor != null ? solidColor : new SolidColorBrush(Colors.Transparent);
                legendItem.Text = GetActualXValue(i)?.ToString() ?? string.Empty;
                legendItem.Index = i;
                legendItem.Item = actualData?[i];
                legendItems.Add(legendItem);
            }
        }

        void IPyramidChartDependent.GenerateSegments()
        {
            double total = CalculateTotalValue();
            CalculatetSegment(total, yValues);
        }

        void IPyramidChartDependent.OnSelectionBehaviorPropertyChanged(object oldValue, object newValue)
        {
            if (newValue is DataPointSelectionBehavior selection)
            {
                selection.Source = this;
                SetInheritedBindingContext(selection, BindingContext);
            }
        }

        #endregion

        #region Internal override methods

        internal override TooltipInfo? GetTooltipInfo(ChartTooltipBehavior behavior, float x, float y)
        {
            if (EnableTooltip)
            {
                int index = this.GetDataPointIndex(x, y);
                if (index < 0 || actualData == null || yValues == null)
                {
                    return null;
                }

                object dataPoint = actualData[index];
                double yValue = yValues[index];
                var segment = (FunnelSegment)segments[index];
                TooltipInfo tooltipInfo = new TooltipInfo(this);
                tooltipInfo.X = segment.SegmentBounds.Center.X + (float)seriesBounds.Left;
                tooltipInfo.Y = segment.SegmentBounds.Center.Y + (float)seriesBounds.Top;
                tooltipInfo.Index = index;
                tooltipInfo.Margin = behavior.Margin;
                tooltipInfo.TextColor = behavior.TextColor;
                tooltipInfo.FontFamily = behavior.FontFamily;
                tooltipInfo.FontSize = behavior.FontSize;
                tooltipInfo.FontAttributes = behavior.FontAttributes;
                tooltipInfo.Background = behavior.Background;
                tooltipInfo.Text = yValue.ToString("#.##");
                tooltipInfo.Item = dataPoint;
                return tooltipInfo;
            }

            return null;
        }

        internal override void OnPlotAreaBackgroundChanged(object newValue)
        {
            if (chartArea != null)
            {
                chartArea.PlotAreaBackgroundView = (View)newValue;
            }
        }

        #endregion

        #region private Static property changed methods

        private static void OnXBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = bindable as SfFunnelChart;
            if (chart != null)
            {
                if (newValue != null && newValue is string)
                {
                    chart.xComplexPaths = ((string)newValue).Split(new char[] { '.' });
                }

                chart.OnBindingPathChanged();
            }
        }

        private static void OnYBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = bindable as SfFunnelChart;
            if (chart != null)
            {
                chart.OnBindingPathChanged();
            }
        }

        private static void NeckWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = bindable as IPyramidChartDependent;
            if (chart != null)
            {
                chart.ScheduleUpdateChart();
            }
        }

        private static void NeckHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = bindable as IPyramidChartDependent;
            if (chart != null)
            {
                chart.ScheduleUpdateChart();
            }
        }

        #endregion

        #region Private Methods

        private bool OnTapAction(IChart chart, Point tapPoint, int tapCount)
        {
            var tooltipBehavior = chart.ActualTooltipBehavior;
            if (tooltipBehavior != null)
                tooltipBehavior.OnTapped(this, tapPoint, tapCount);

            if (SelectionBehavior != null && SelectionBehavior.Type != 0)
            {
                return SelectionBehavior.OnTapped((float)(tapPoint.X - seriesBounds.Left), (float)(tapPoint.Y - seriesBounds.Top));
            }

            return false;
        }

        private void OnTouchMove(IChart chart, Point point, PointerDeviceType deviceType)
        {
            var tooltipBehavior = chart.ActualTooltipBehavior;
            if (tooltipBehavior != null)
            {
                tooltipBehavior.OnTouchMove(chart, (float)point.X, (float)point.Y, deviceType);
            }
        }

        private void CalculatetSegment(double total, IList<double> yValues)
        {
            if (yValues != null)
            {
                double currY = 0;
                double coefHeight = 1 / (total * (1 + (GapRatio / (1 - GapRatio))));
                int dataCount = pointsCount;
                //TODO: Data count used for gap ratio calculation, need to consider with datamanager implementation.[Get Non - nan values count]
                for (int i = 0; i < dataCount; i++)
                {
                    if (double.IsNaN(yValues[i]))
                    {
                        dataCount -= 1;
                    }
                }

                double spacing = GapRatio / (dataCount - 1);

                for (int i = pointsCount - 1; i >= 0; i--)
                {
                    float yValue = (float)yValues[i];
                    double height = coefHeight * Math.Abs(float.IsNaN(yValue) ? 0 : yValue);

                    if (pointsCount <= segments.Count && segments[i] is FunnelSegment)
                    {
                        ((FunnelSegment)segments[i]).SetData(currY, height, NeckWidth, NeckHeight, GetActualXValue(i) , yValue);
                    }
                    else
                    {
                        var funnelSegment = (FunnelSegment)CreateSegment();
                        funnelSegment.Chart = this;
                        funnelSegment.SetData(currY, height, NeckWidth, NeckHeight, GetActualXValue(i), yValue);
                        funnelSegment.Index = i;
                        segments.Insert(0, funnelSegment);
                    }

                    if (!float.IsNaN(yValue))
                    {
                        currY += height + spacing;
                    }
                }
            }
        }

        private double CalculateTotalValue()
        {
            double sumValues = 0;
            for (int i = 0; i < pointsCount; i++)
            {
                sumValues += Math.Max(0, Math.Abs(double.IsNaN(yValues[i]) ? 0 : yValues[i]));
            }

            return sumValues;
        }

        private void OnBindingPathChanged()
        {
            ResetData();
            GeneratePoints(new[] { YBindingPath }, yValues);
            UpdateLegendItems();
            segmentsCreated = false;
            ScheduleUpdateChart();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected internal virtual ChartSegment CreateSegment()
        {
            return new FunnelSegment();
        }

        private object? GetActualXValue(int index)
        {
            if (xValues == null || index > pointsCount)
            {
                return null;
            }

            if (xValueType == ChartValueType.String)
            {
                return ((IList<string>)xValues)[index];
            }
            else if (xValueType == ChartValueType.DateTime)
            {
                return DateTime.FromOADate(((IList<double>)xValues)[index]).ToString("MM/dd/yyyy");
            }
            else if (xValueType == ChartValueType.Double || xValueType == ChartValueType.Logarithmic)
            {
                //Logic is to cut off the 0 decimal value from the number.
                object label = ((List<double>)xValues)[index];
                var actualVal = (double)label;
                if (actualVal == (long)actualVal)
                {
                    label = (long)actualVal;
                }

                return label;
            }
            else
            {
                return ((IList)xValues)[index];
            }
        }

        private static void OnDataLabelSettingsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var chart = bindable as SfFunnelChart;
            if (chart != null)
            {
                chart.OnDataLabelSettingsPropertyChanged(oldValue as ChartDataLabelSettings, newValue as ChartDataLabelSettings);
            }
        }

        private void OnDataLabelSettingsPropertyChanged(ChartDataLabelSettings? oldValue, ChartDataLabelSettings? newValue)
        {
            if (oldValue is FunnelDataLabelSettings oldSettings)
            {
                oldSettings.PropertyChanged -= Settings_PropertyChanged;
                if (oldSettings.LabelStyle != null)
                {
                    oldSettings.LabelStyle.PropertyChanged -= Settings_PropertyChanged;
                }
            }

            if (newValue is FunnelDataLabelSettings newSettings)
            {
                newSettings.PropertyChanged += Settings_PropertyChanged;
                SetInheritedBindingContext(newSettings, BindingContext);

                if (newSettings.LabelStyle is ChartDataLabelStyle style)
                {
                    style.PropertyChanged += Settings_PropertyChanged;
                    SetInheritedBindingContext(style, BindingContext);
                }
            }
        }

        private void Settings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ScheduleUpdateChart();
        }

        #endregion

        #region ItemSource Update

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfFunnelChart chart)
            {
                chart.OnItemsSourceChanged(oldValue, newValue);
            }
        }

        private void OnItemsSourceChanged(object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            UpdateLegendItems();
            ResetData();
            OnDataSourceChanged(oldValue, newValue);
            HookAndUnhookCollectionChangedEvent(oldValue, newValue);
            segmentsCreated = false;
            ScheduleUpdateChart();
        }

        private void UpdateLegendItems()
        {
            if (!chartArea.AreaBounds.IsEmpty)
            {
                chartArea.PlotArea.ShouldPopulateLegendItems = true;
            }
        }

        private void OnDataSourceChanged(object oldValue, object newValue)
        {
            yValues.Clear();
            GeneratePoints(new[] { YBindingPath }, yValues);
        }

        void ScheduleUpdateChart()
        {
            if (chartArea != null)
            {
                chartArea.NeedsRelayout = true;
                chartArea.ScheduleUpdateArea();
            }
        }

        private static object? GetPropertyValue(object obj, string[] paths)
        {
            object? parentObj = obj;
            for (int i = 0; i < paths.Length; i++)
            {
                parentObj = ReflectedObject(parentObj, paths[i]);
            }

            if (parentObj != null)
            {
                if (parentObj.GetType().IsArray)
                {
                    return null;
                }
            }

            return parentObj;
        }

        private void GenerateDataPoints()
        {
            GeneratePoints(new[] { YBindingPath }, yValues);
        }

        private void HookAndUnhookCollectionChangedEvent(object oldValue, object? newValue)
        {
            if (newValue != null)
            {
                var newCollectionValue = newValue as INotifyCollectionChanged;
                if (newCollectionValue != null)
                {
                    newCollectionValue.CollectionChanged += OnDataSource_CollectionChanged;
                }
            }

            if (oldValue != null)
            {
                var oldCollectionValue = oldValue as INotifyCollectionChanged;
                if (oldCollectionValue != null)
                {
                    oldCollectionValue.CollectionChanged -= OnDataSource_CollectionChanged;
                }
            }
        }

        private void OnDataSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((index, obj) => AddDataPoint(index, obj), (index, obj) => RemoveData(index), ResetDataPoint);

            UpdateLegendItems();
            ScheduleUpdateChart();
        }

        private void AddDataPoint(int index, object data)
        {
            SetIndividualPoint(index, data, false);
        }

        private void ResetDataPoint()
        {
            ResetData();
            if (ItemsSource != null)
            {
                var items = ItemsSource is IList ? ItemsSource as IList : null;
                if (items == null)
                {
                    var source = ItemsSource as IEnumerable;
                    if (source != null)
                    {
                        //TODO: Consider removing the toList();
                        items = source.Cast<object>().ToList();
                    }
                }

                if (items != null && items.Count > 0)
                {
                    GenerateDataPoints();
                }
            }
        }

        private void RemoveData(int index)
        {
            if (xValues is IList<double>)
            {
                ((IList<double>)xValues).RemoveAt(index);
                pointsCount--;
            }
            else if (xValues is IList<string>)
            {
                ((IList<string>)xValues).RemoveAt(index);
                pointsCount--;
            }

            actualData?.RemoveAt(index);
            yValues?.RemoveAt(index);
        }

        private ChartValueType GetDataType(IEnumerator enumerator, string[] paths)
        {
            // GetArrayPropertyValue method is used to get value from the path of current object
            object? parentObj = GetArrayPropertyValue(enumerator.Current, paths);

            return GetDataTypes(parentObj);
        }

        private void ResetData()
        {
            if (actualXValues is IList && xValues is IList)
            {
                ((IList)xValues).Clear();
                ((IList)actualXValues).Clear();
            }

            actualData?.Clear();
            yValues.Clear();

            pointsCount = 0;

            if (XBindingPath != null && yPaths != null && yPaths.Any())
            {
                segments.Clear();
            }
        }

        private ChartValueType GetDataType(FastReflection fastReflection, IEnumerable dataSource)
        {
            if (dataSource == null)
            {
                return ChartValueType.Double;
            }

            var enumerator = dataSource.GetEnumerator();
            object? obj = null;
            if (enumerator.MoveNext())
            {
                do
                {
                    obj = fastReflection.GetValue(enumerator.Current);
                }
                while (enumerator.MoveNext() && obj == null);
            }

            return GetDataType(obj);
        }

        private ChartValueType GetDataType(object? xValue)
        {
            if (xValue is string || xValue is string[])
            {
                return ChartValueType.String;
            }
            else if (xValue is DateTime || xValue is DateTime[])
            {
                return ChartValueType.DateTime;
            }
            else if (xValue is TimeSpan || xValue is TimeSpan[])
            {
                return ChartValueType.TimeSpan;
            }
            else
            {
                return ChartValueType.Double;
            }
        }

        private static object? ReflectedObject(object? parentObj, string actualPath)
        {
            var fastReflection = new FastReflection();
            if (parentObj != null && fastReflection.SetPropertyName(actualPath, parentObj))
            {
                return fastReflection.GetValue(parentObj);
            }

            return null;
        }

        private static object? GetArrayPropertyValue(object obj, string[]? paths)
        {
            var parentObj = obj;

            if (paths == null)
                return parentObj;

            for (int i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                if (path.Contains("["))
                {
                    int index = Convert.ToInt32(path.Substring(path.IndexOf('[') + 1, path.IndexOf(']') - path.IndexOf('[') - 1));
                    string actualPath = path.Replace(path.Substring(path.IndexOf('[')), string.Empty);
                    parentObj = ReflectedObject(parentObj, actualPath);

                    if (parentObj == null)
                    {
                        return null;
                    }

                    var array = parentObj as IList;
                    if (array != null && array.Count > index)
                    {
                        parentObj = array[index];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    parentObj = ReflectedObject(parentObj, path);

                    if (parentObj == null)
                    {
                        return null;
                    }

                    if (parentObj.GetType().IsArray)
                    {
                        return null;
                    }
                }
            }

            return parentObj;
        }

        private static ChartValueType GetDataTypes(object? xValue)
        {
            if (xValue is string || xValue is string[])
            {
                return ChartValueType.String;
            }
            else if (xValue is DateTime || xValue is DateTime[])
            {
                return ChartValueType.DateTime;
            }
            else if (xValue is TimeSpan || xValue is TimeSpan[])
            {
                return ChartValueType.TimeSpan;
            }
            else
            {
                return ChartValueType.Double;
            }
        }

        private void GeneratePoints(string[] yPaths, params IList<double>[] yValueLists)
        {
            if (yPaths == null)
            {
                return;
            }

            IList<double>[]? yLists = null;
            isComplexYProperty = false;
            bool isArrayProperty = false;
            yComplexPaths = new string[yPaths.Length][];
            for (int i = 0; i < yPaths.Length; i++)
            {
                if (string.IsNullOrEmpty(yPaths[i]))
                {
                    return;
                }

                yComplexPaths[i] = yPaths[i].Split(new char[] { '.' });
                if (yPaths[i].Contains('.'))
                {
                    isComplexYProperty = true;
                }

                if (yPaths[i].Contains('['))
                {
                    isArrayProperty = true;
                }
            }

            yLists = yValueLists;

            this.yPaths = yPaths;

            if (actualData == null)
            {
                actualData = new List<object>();
            }

            if (ItemsSource != null && !string.IsNullOrEmpty(XBindingPath))
            {
                if (ItemsSource is IEnumerable)
                {
                    if (XBindingPath.Contains('[') || isArrayProperty)
                    {
                        GenerateComplexPropertyPoints(yPaths, yLists, GetArrayPropertyValue);
                    }
                    else if (XBindingPath.Contains('.') || isComplexYProperty)
                    {
                        GenerateComplexPropertyPoints(yPaths, yLists, GetPropertyValue);
                    }
                    else
                    {
                        GeneratePropertyPoints(yPaths, yLists);
                    }
                }
            }
        }

        private void GeneratePropertyPoints(string[] yPaths, IList<double>[] yLists)
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

                xValueType = GetDataType(xProperty, enumerable);

                if (xValueType == ChartValueType.DateTime || xValueType == ChartValueType.Double ||
                    xValueType == ChartValueType.Logarithmic || xValueType == ChartValueType.TimeSpan)
                {
                    if (!(actualXValues is List<double>))
                    {
                        this.actualXValues = this.xValues = new List<double>();
                    }
                }
                else
                {
                    if (!(actualXValues is List<string>))
                    {
                        this.actualXValues = this.xValues = new List<string>();
                    }
                }

                string yPath;

                if (string.IsNullOrEmpty(yPaths[0]))
                {
                    return;
                }
                else
                {
                    yPath = yPaths[0];
                }

                var yProperty = new FastReflection();

                if (!yProperty.SetPropertyName(yPath, currObj) || yProperty.IsArray(currObj))
                {
                    return;
                }

                IList<double> yValue = yLists[0];

                if (xValueType == ChartValueType.String)
                {
                    var xValue = this.xValues as List<string>;
                    if (xValue != null)
                    {
                        do
                        {
                            var xVal = xProperty.GetValue(enumerator.Current);
                            var yVal = yProperty.GetValue(enumerator.Current);
                            xValue.Add(xVal.Tostring());
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            actualData?.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        pointsCount = xValue.Count;
                    }
                }
                else if (xValueType == ChartValueType.DateTime)
                {
                    var xValue = this.xValues as List<double>;
                    if (xValue != null)
                    {
                        do
                        {
                            var xVal = xProperty.GetValue(enumerator.Current);
                            var yVal = yProperty.GetValue(enumerator.Current);

                            xData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && xData <= xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(xData);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            actualData?.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        pointsCount = xValue.Count;
                    }
                }
                else if (xValueType == ChartValueType.Double ||
                         xValueType == ChartValueType.Logarithmic)
                {
                    var xValue = this.xValues as List<double>;
                    if (xValue != null)
                    {
                        do
                        {
                            var xVal = xProperty.GetValue(enumerator.Current);
                            var yVal = yProperty.GetValue(enumerator.Current);
                            xData = Convert.ToDouble(xVal ?? double.NaN);

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && xData <= xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(xData);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            actualData?.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        pointsCount = xValue.Count;
                    }
                }
                else if (xValueType == ChartValueType.TimeSpan)
                {
                    //TODO: ensure while implementing timespane.
                }
            }
        }

        private void GenerateComplexPropertyPoints(string[] yPaths, IList<double>[] yLists, GetReflectedProperty? getPropertyValue)
        {
            var enumerable = ItemsSource as IEnumerable;
            var enumerator = enumerable?.GetEnumerator();

            if (enumerable != null && enumerator != null && getPropertyValue != null && enumerator.MoveNext() && xComplexPaths != null && yComplexPaths != null)
            {
                xValueType = GetDataType(enumerator, xComplexPaths);
                if (xValueType == ChartValueType.DateTime || xValueType == ChartValueType.Double ||
                    xValueType == ChartValueType.Logarithmic || xValueType == ChartValueType.TimeSpan)
                {
                    if (!(xValues is List<double>))
                    {
                        this.actualXValues = this.xValues = new List<double>();
                    }
                }
                else
                {
                    if (!(xValues is List<string>))
                    {
                        this.actualXValues = this.xValues = new List<string>();
                    }
                }

                string[] tempYPath = yComplexPaths[0];
                if (string.IsNullOrEmpty(yPaths[0]))
                {
                    return;
                }

                IList<double> yValue = yLists[0];
                object? xVal, yVal;
                if (xValueType == ChartValueType.String)
                {
                    var xValue = this.xValues as List<string>;
                    if (xValue != null)
                    {
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, xComplexPaths);
                            yVal = getPropertyValue(enumerator.Current, tempYPath);
                            if (xVal == null)
                            {
                                return;
                            }

                            xValue.Add((string)xVal);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            actualData?.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        pointsCount = xValue.Count;
                    }
                }
                else if (xValueType == ChartValueType.Double ||
                    xValueType == ChartValueType.Logarithmic)
                {
                    var xValue = this.xValues as List<double>;
                    if (xValue != null)
                    {
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, xComplexPaths);
                            yVal = getPropertyValue(enumerator.Current, tempYPath);
                            xData = Convert.ToDouble(xVal ?? double.NaN);

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && xData <= xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(xData);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            actualData?.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        pointsCount = xValue.Count;
                    }
                }
                else if (xValueType == ChartValueType.DateTime)
                {
                    var xValue = this.xValues as List<double>;
                    if (xValue != null)
                    {
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, xComplexPaths);
                            yVal = getPropertyValue(enumerator.Current, tempYPath);

                            xData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && xData <= xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(xData);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            actualData?.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        pointsCount = xValue.Count;
                    }
                }
                else if (xValueType == ChartValueType.TimeSpan)
                {
                    //TODO: Ensure for timespan;
                }
            }
        }

        private void SetIndividualPoint(int index, object value, bool replace)
        {
            if (yValues != null && yPaths != null && ItemsSource != null)
            {
                var xvalueType = GetArrayPropertyValue(value, xComplexPaths);
                if (xvalueType != null)
                {
                    xValueType = GetDataType(xvalueType);
                }

                double yData;
                var tempYPath = yComplexPaths != null ? yComplexPaths[0] : null;
                var yValue = yValues;
                switch (xValueType)
                {
                    case ChartValueType.String:
                        {
                            if (!(this.xValues is List<string>))
                            {
                                xValues = actualXValues = new List<string>();
                            }

                            IList<string> xValue = (List<string>)xValues;
                            var xVal = GetArrayPropertyValue(value, xComplexPaths);
                            var yVal = GetArrayPropertyValue(value, tempYPath);
                            yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;
                            if (replace && xValue.Count > index)
                            {
                                xValue[index] = xVal.Tostring();
                            }
                            else
                            {
                                xValue.Insert(index, xVal.Tostring());
                            }

                            if (replace && yValue.Count > index)
                            {
                                yValue[index] = yData;
                            }
                            else
                            {
                                yValue.Insert(index, yData);
                            }

                            pointsCount = xValue.Count;
                        }

                        break;
                    case ChartValueType.Double:
                    case ChartValueType.Logarithmic:
                        {
                            if (!(xValues is List<double>))
                            {
                                xValues = actualXValues = new List<double>();
                            }

                            IList<double> xValue = (List<double>)xValues;
                            var xVal = GetArrayPropertyValue(value, xComplexPaths);
                            var yVal = GetArrayPropertyValue(value, tempYPath);
                            xData = xVal != null ? Convert.ToDouble(xVal) : double.NaN;
                            yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && xData <= xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            if (replace && xValue.Count > index)
                            {
                                xValue[index] = xData;
                            }
                            else
                            {
                                xValue.Insert(index, xData);
                            }

                            if (replace && yValue.Count > index)
                            {
                                yValue[index] = yData;
                            }
                            else
                            {
                                yValue.Insert(index, yData);
                            }

                            pointsCount = xValue.Count;
                        }

                        break;
                    case ChartValueType.DateTime:
                        {
                            if (!(this.xValues is List<double>))
                            {
                                this.xValues = actualXValues = new List<double>();
                            }

                            IList<double> xValue = (List<double>)xValues;
                            var xVal = GetArrayPropertyValue(value, xComplexPaths);
                            var yVal = GetArrayPropertyValue(value, tempYPath);
                            xData = Convert.ToDateTime(xVal).ToOADate();
                            yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && xData <= xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            if (replace && xValue.Count > index)
                            {
                                xValue[index] = xData;
                            }
                            else
                            {
                                xValue.Insert(index, xData);
                            }

                            if (replace && yValue.Count > index)
                            {
                                yValue[index] = yData;
                            }
                            else
                            {
                                yValue.Insert(index, yData);
                            }

                            pointsCount = xValue.Count;
                        }

                        break;
                    case ChartValueType.TimeSpan:
                        {
                            //TODO: Ensure on time span implementation.
                        }

                        break;
                }

                if (actualData != null)
                {
                    if (replace && actualData.Count > index)
                    {
                        actualData[index] = value;
                    }
                    else if (actualData.Count == index)
                    {
                        actualData.Add(value);
                    }
                    else
                    {
                        actualData.Insert(index, value);
                    }
                }
            }
        }

        #endregion

        #endregion
    }
}