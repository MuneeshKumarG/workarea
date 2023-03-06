using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Renders two different types of circular charts, including doughnut and pie. Each chart has a different presentation of the data and is made to be more user-friendly.
    /// </summary>
    /// <remarks>
    /// <para>Circular chart control is used to visualize the data graphically and to display the data with proportions and percentage of different categories.</para>
    ///
    /// <para>SfCircularChart class properties provides an option to add the series collection, allows to customize the chart elements such as legend, data label, selection, and tooltip features. </para>
    ///
    /// <img src="https://cdn.syncfusion.com/content/images/maui/MAUI_Pie.jpg"/>
    /// 
    /// <para><b>Series</b></para>
    /// 
    /// <para>ChartSeries is the visual representation of data. SfCircularChart offers <see cref="PieSeries"/> and <see cref="DoughnutSeries"/>.</para>
    /// 
    /// <para>To render a series, create an instance of required series class, and add it to the <see cref="Series"/> collection.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code> <![CDATA[
    /// <chart:SfCircularChart>
    ///
    ///        <chart:SfCircularChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCircularChart.DataContext>
    ///
    ///        <chart:PieSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>
    ///
    /// </chart:SfCircularChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCircularChart chart = new SfCircularChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    /// 
    /// PieSeries series = new PieSeries()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue"
    /// };
    /// chart.Series.Add(series);
    /// ]]>
    /// </code>
    /// # [ViewModel.cs](#tab/tabid-3)
    /// <code><![CDATA[
    /// public ObservableCollection<Model> Data { get; set; }
    /// 
    /// public ViewModel()
    /// {
    ///    Data = new ObservableCollection<Model>();
    ///    Data.Add(new Model() { XValue = "A", YValue = 100 });
    ///    Data.Add(new Model() { XValue = "B", YValue = 150 });
    ///    Data.Add(new Model() { XValue = "C", YValue = 110 });
    ///    Data.Add(new Model() { XValue = "D", YValue = 230 });
    /// }
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <para><b>Legend</b></para>
    ///
    /// <para>The Legend contains list of data points in chart series. The information provided in each legend item helps to identify the corresponding data point in chart series. The Series <see cref="ChartSeries.XBindingPath"/> property value will be displayed in the associated legend item.</para>
    ///
    /// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="ChartBase.Legend"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-4)
    /// <code> <![CDATA[
    /// <chart:SfCircularChart>
    ///
    ///        <chart:SfCircularChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCircularChart.DataContext>
    ///
    ///        <chart:SfCircularChart.Legend>
    ///            <chart:ChartLegend/>
    ///        </chart:SfCircularChart.Legend>
    ///
    ///        <chart:PieSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>
    ///
    /// </chart:SfCircularChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-5)
    /// <code><![CDATA[
    /// SfCircularChart chart = new SfCircularChart();
    ///
    /// ViewModel viewModel = new ViewModel();
    /// chart.DataContext = viewModel;
    ///
    /// chart.Legend = new ChartLegend();
    /// 
    /// PieSeries series = new PieSeries()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue"
    /// };
    /// chart.Series.Add(series);
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <para><b>Tooltip</b></para>
    ///
    /// <para>Tooltip displays information while tapping or mouse hover on the segment. To display the tooltip on the chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="ChartSeriesBase"/>. </para>
    ///
    /// <para>To customize the appearance of the tooltip elements like Background, TextColor and Font, create an instance of <see cref="ChartTooltipBehavior"/> class, modify the values, and assign it to the chart’s <see cref="ChartBase.TooltipBehavior"/> property. </para>
    ///
    /// # [MainPage.xaml](#tab/tabid-6)
    /// <code><![CDATA[
    /// <chart:SfCircularChart>
    /// 
    ///         <chart:SfCircularChart.DataContext>
    ///             <local:ViewModel/>
    ///         </chart:SfCircularChart.DataContext>
    ///
    ///         <chart:SfCircularChart.TooltipBehavior>
    ///             <chart:ChartTooltipBehavior/>
    ///         </chart:SfCircularChart.TooltipBehavior>
    ///
    ///         <chart:PieSeries EnableTooltip = "True" 
    ///                          ItemsSource="{Binding Data}"
    ///                          XBindingPath="XValue"
    ///                          YBindingPath="YValue"/>
    ///
    /// </chart:SfCircularChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-7)
    /// <code><![CDATA[
    /// SfCircularChart chart = new SfCircularChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    ///
    /// chart.TooltipBehavior = new ChartTooltipBehavior();
    ///
    /// PieSeries series = new PieSeries()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue",
    ///    EnableTooltip = true
    /// };
    /// chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <para><b>Data Label</b></para>
    ///
    /// <para>Data labels are used to display values related to a chart segment. To render the data labels, you need to enable the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <b>ChartSeries</b> class. </para>
    ///
    /// <para>To customize the chart data labels alignment, placement and label styles, need to create an instance of <see cref="CircularDataLabelSettings"/> and set to the <see cref="CircularSeries.DataLabelSettings"/> property.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-8)
    /// <code><![CDATA[
    /// <chart:SfCircularChart>
    ///
    ///        <chart:SfCircularChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfCircularChart.DataContext>
    ///
    ///        <chart:DoughnutSeries ShowDataLabels = "True"
    ///                              ItemsSource="{Binding Data}"
    ///                              XBindingPath="XValue"
    ///                              YBindingPath="YValue">
    ///             <chart:DoughnutSeries.DataLabelSettings>
    ///                 <chart:CircularDataLabelSettings />
    ///             </chart:DoughnutSeries.DataLabelSettings>
    ///        </chart:DoughnutSeries>
    ///
    /// </chart:SfCircularChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-9)
    /// <code><![CDATA[
    /// SfCircularChart chart = new SfCircularChart();
    ///
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    /// CircularDataLabelSettings dataLabelSettings = new CircularDataLabelSettings();
    /// DoughnutSeries series = new DoughnutSeries()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue",
    ///     ShowDataLabels = true,
    ///     DataLabelSettings = dataLabelSettings,
    /// };
    /// chart.Series.Add(series);
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <para><b>Selection</b></para>
    /// 
    /// <para>SfCircularChart allows you to select or highlight a segment in the chart by using <see cref="DataPointSelectionBehavior"/>.</para>
    /// 
    /// <para>To enable the segment selection in the chart, create an instance of <see cref="DataPointSelectionBehavior"/> and set it to the <see cref="PieSeries.SelectionBehavior"/> property of series.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-10)
    /// <code><![CDATA[
    /// <chart:SfCircularChart>
    /// 
    ///         <chart:SfCircularChart.DataContext>
    ///             <local:ViewModel/>
    ///         </chart:SfCircularChart.DataContext>
    /// 
    ///         <chart:PieSeries EnableTooltip = "True"
    ///                          ItemsSource="{Binding Data}"
    ///                          XBindingPath="XValue"
    ///                          YBindingPath="YValue">
    ///             <chart:PieSeries.SelectionBehavior>
    ///                 <chart:DataPointSelectionBehavior SelectionBrush = "Green" />
    ///             </chart:PieSeries.SelectionBehavior>
    ///         </chart:PieSeries>
    ///
    /// </chart:SfCircularChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-11)
    /// <code><![CDATA[
    /// SfCircularChart chart = new SfCircularChart();
    ///
    /// DataPointSelectionBehavior selectionBehavior = new DataPointSelectionBehavior()
    /// {
    ///     SelectionBrush=new SolidColorBrush(Colors.Green)
    /// };
    ///
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    ///
    /// PieSeries series = new PieSeries()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue",
    ///    EnableTooltip = true,
    ///    Selection = selectionBehavior
    /// };
    /// chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    [ContentProperty(Name = "Series")]
    public class SfCircularChart : ChartBase
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <see cref="Series"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Series</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(
                nameof(Series),
                typeof(CircularSeriesCollection),
                typeof(SfCircularChart),
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfCircularChart"/> class.
        /// </summary>
        public SfCircularChart()
        {
#if NETCORE
#if SyncfusionLicense
           if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
               LicenseHelper.ValidateLicense();
            }
#endif
#endif
            DefaultStyleKey = typeof(SfCircularChart);
            Series = new CircularSeriesCollection();
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets a collection of chart series to be added to the circular chart.
        /// </summary>
        /// <value>This property takes <see cref="CircularSeriesCollection"/> instance as value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-12)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        /// 
        ///           <chart:SfCircularChart.DataContext>
        ///               <local:ViewModel/>
        ///           </chart:SfCircularChart.DataContext>
        ///
        ///           <chart:DoughnutSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue"/>
        /// 
        ///     </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-13)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        /// 
        ///     ViewModel viewModel = new ViewModel();
        ///     chart.DataContext = viewModel;
        /// 
        ///     DoughnutSeries series = new DoughnutSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        /// 
        /// ]]>
        /// </code>
        /// # [ViewModel](#tab/tabid-14)
        /// <code><![CDATA[
        /// public ObservableCollection<Model> Data { get; set; }
        /// 
        /// public ViewModel()
        /// {
        ///    Data = new ObservableCollection<Model>();
        ///    Data.Add(new Model() { XValue = "A", YValue = 100 });
        ///    Data.Add(new Model() { XValue = "B", YValue = 150 });
        ///    Data.Add(new Model() { XValue = "C", YValue = 110 });
        ///    Data.Add(new Model() { XValue = "D", YValue = 230 });
        /// }
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        /// <remarks><para>To render a series, create an instance of required series class, and add it to the <see cref="Series"/> collection.</para></remarks>
        public CircularSeriesCollection Series
        {
            get { return (CircularSeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        #endregion

        protected override void OnApplyTemplate()
        {
            CircularPlotArea plotArea = GetTemplateChild("CircularPlotArea") as CircularPlotArea;
            plotArea.SeriesCollection = Series;
            PlotArea = plotArea;

            base.OnApplyTemplate();

            (AreaPanel as CircularAreaPanel).Chart = this;
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
            if (seriesCollection is CircularSeriesCollection)
            {
                (seriesCollection as CircularSeriesCollection).CollectionChanged -= OnSeriesCollectionChanged;
            }
            else
            {
                base.UnHookSeriesCollection(seriesCollection);
            }
        }

        internal override void HookSeriesCollection(IList seriesCollection)
        {
            if (seriesCollection is CircularSeriesCollection)
            {
                (seriesCollection as CircularSeriesCollection).CollectionChanged += OnSeriesCollectionChanged;
            }
            else
            {
                base.HookSeriesCollection(seriesCollection);
            }
        }

        internal override void SetSeriesCollection(IList seriesCollection)
        {
            Series = (CircularSeriesCollection)seriesCollection;
        }
    }
}
