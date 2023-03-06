using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
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
    /// Renders funnel chart for more user-friendly data representation and greater UI visualization.
    /// </summary>
    /// <remarks>
    /// <para>Funnel chart control is used to analyze the various stages in a process.</para>
    ///
    /// <para>SfFunnelChart class allows to customize the chart elements such as legend, data label, and tooltip features. </para>
    ///
    /// <para><b>Legend</b></para>
    /// 
    /// <para>The Legend contains list of data points in chart series. The information provided in each legend item helps to identify the corresponding data point in chart series. The Series <see cref="ChartSeries.XBindingPath"/> property value will be displayed in the associated legend item.</para>
    /// 
    /// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="ChartBase.Legend"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code> <![CDATA[
    /// <chart:SfFunnelChart ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue">
    ///
    ///        <chart:SfFunnelChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfFunnelChart.DataContext>
    ///        <chart:SfFunnelChart.Legend>
    ///            <chart:ChartLegend/>
    ///        </chart:SfFunnelChart.Legend>
    ///    
    /// </chart:SfFunnelChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// ViewModel viewModel = new ViewModel();
    /// 
    /// SfFunnelChart chart = new SfFunnelChart()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue"
    /// };
    /// 
    /// chart.DataContext = viewModel;
    /// chart.Legend = new ChartLegend();
    /// 
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
    /// <para><b>Tooltip</b></para>
    /// 
    /// <para>Tooltip displays information while tapping or mouse hover on the segment. To display the tooltip on the chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="ChartSeries"/>. </para>
    /// 
    /// <para>To customize the appearance of the tooltip elements like Background, TextColor and Font, create an instance of <see cref="ChartTooltipBehavior"/> class, modify the values, and assign it to the chart’s <see cref="ChartBase.TooltipBehavior"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-4)
    /// <code><![CDATA[
    /// <chart:SfFunnelChart EnableTooltip = "True"
    ///                      ItemsSource="{Binding Data}"
    ///                      XBindingPath="XValue"
    ///                      YBindingPath="YValue">
    /// 
    ///         <chart:SfFunnelChart.DataContext>
    ///             <local:ViewModel/>
    ///         </chart:SfFunnelChart.DataContext>
    /// 
    ///         <chart:SfFunnelChart.TooltipBehavior>
    ///             <chart:ChartTooltipBehavior/>
    ///         </chart:SfFunnelChart.TooltipBehavior>
    /// 
    /// </chart:SfFunnelChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-5)
    /// <code><![CDATA[
    /// ViewModel viewModel = new ViewModel();
    /// 
    /// SfFunnelChart chart = new SfFunnelChart()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue",
    ///    EnableTooltip = true
    /// };
    /// 
    ///	chart.DataContext = viewModel;
    ///
    /// chart.TooltipBehavior = new ChartTooltipBehavior();
    ///
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Data Label</b></para>
    /// 
    /// <para>Data labels are used to display values related to a chart segment. To render the data labels, you need to enable the <see cref="SfFunnelChart.ShowDataLabels"/> property as <b>true</b> in <b>SfFunnelChart</b> class. </para>
    /// 
    /// <para>To customize the chart data labels alignment, placement and label styles, need to create an instance of <see cref="FunnelDataLabelSettings"/> and set to the <see cref="SfFunnelChart.DataLabelSettings"/> property.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-6)
    /// <code><![CDATA[
    /// <chart:SfFunnelChart ShowDataLabels = "True"
    ///                      ItemsSource="{Binding Data}"
    ///                      XBindingPath="XValue"
    ///                      YBindingPath="YValue">
    ///
    ///        <chart:SfFunnelChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfFunnelChart.DataContext>
    ///    
    /// </chart:SfFunnelChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-7)
    /// <code><![CDATA[
    /// ViewModel viewModel = new ViewModel();
    /// 
    /// SfFunnelChart chart = new SfFunnelChart()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue",
    ///     ShowDataLabels = true
    /// };
    /// 
    ///.chart.DataContext = viewModel;
    /// 
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    public class SfFunnelChart : ChartBase
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="SelectionBehavior"/> property.       .
        /// </summary>
        public static readonly DependencyProperty SelectionBehaviorProperty =
            DependencyProperty.Register(
                nameof(SelectionBehavior),
                typeof(DataPointSelectionBehavior),
                typeof(SfFunnelChart),
                new PropertyMetadata(null, OnSelectionBehaviorChanged));

        /// Identifies the <see cref="PaletteBrushes"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PaletteBrushes</c> dependency property.
        /// </value>      
        public static readonly DependencyProperty PaletteBrushesProperty =
            DependencyProperty.Register(nameof(PaletteBrushes), typeof(IList<Brush>), typeof(SfFunnelChart),
                new PropertyMetadata(ChartColorModel.DefaultBrushes, OnPaletteBrushesChanged));

        /// <summary>
        /// Identifies the <c>MinimumWidth</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>MinimumWidth</c> dependency property.
        /// </value> 
        public static readonly DependencyProperty MinimumWidthProperty =
            DependencyProperty.Register(
                nameof(MinimumWidth),
                typeof(double),
                typeof(SfFunnelChart),
                new PropertyMetadata(40d, OnMinWidthChanged));


        /// <summary>
        /// Identifies the <c>YBindingPath</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>YBindingPath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty YBindingPathProperty =
            DependencyProperty.Register(nameof(YBindingPath), typeof(string), typeof(SfFunnelChart),
            new PropertyMetadata(null, OnYPathChanged));

        /// <summary>
        /// Identifies the <c>ExplodeIndex</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ExplodeIndex</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ExplodeIndexProperty =
            DependencyProperty.Register(nameof(ExplodeIndex), typeof(int), typeof(SfFunnelChart),
            new PropertyMetadata(-1, OnExplodeIndexChanged));

        /// <summary>
        /// Identifies the <c>ExplodeOnTap</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ExplodeOnTap</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ExplodeOnTapProperty =
            DependencyProperty.Register(nameof(ExplodeOnTap), typeof(bool), typeof(SfFunnelChart),
            new PropertyMetadata(false, OnExplodeOnClickChanged));

        /// <summary>
        /// Identifies the <c>GapRatio</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>GapRatio</c> dependency property.
        /// </value>   
        public static readonly DependencyProperty GapRatioProperty =
            DependencyProperty.Register(
                nameof(GapRatio),
                typeof(double),
                typeof(SfFunnelChart),
                new PropertyMetadata(0d, new PropertyChangedCallback(OnGapRatioChanged)));

        /// <summary>
        /// Identifies the <c>ExplodeOffset</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>ExplodeOffset</c> dependency property.
        /// </value>   
        public static readonly DependencyProperty ExplodeOffsetProperty =
            DependencyProperty.Register(
                nameof(ExplodeOffset),
                typeof(double),
                typeof(SfFunnelChart),
                new PropertyMetadata(40d, new PropertyChangedCallback(OnExplodeOffsetChanged)));

        /// <summary>
        /// Identifies the <c>Mode</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>Mode</c> dependency property.
        /// </value> 
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(
                nameof(Mode),
                typeof(ChartFunnelMode),
                typeof(SfFunnelChart),
                new PropertyMetadata(ChartFunnelMode.ValueIsHeight, OnFunneldModeChanged));


        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>DataLabelSettings</c> dependency property.
        /// </value>
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(FunnelDataLabelSettings), typeof(SfFunnelChart),
          new PropertyMetadata(null, OnAdornmentsInfoChanged));

        /// <summary>
        /// Identifies the <c>ItemsSource</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ItemsSource</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(SfFunnelChart),
            new PropertyMetadata(null, new PropertyChangedCallback(OnDataSourceChanged)));

        /// <summary>
        /// Identifies the <c>XBindingPath</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>XBindingPath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty XBindingPathProperty =
        DependencyProperty.Register(nameof(XBindingPath), typeof(string), typeof(SfFunnelChart),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnBindingPathXChanged)));

        /// <summary>
        /// Identifies the <c>TooltipTemplate</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>TooltipTemplate</c> dependency property.
        /// </value>
        public static readonly DependencyProperty TooltipTemplateProperty =
            DependencyProperty.Register(nameof(TooltipTemplate), typeof(DataTemplate), typeof(SfFunnelChart),
                new PropertyMetadata(null, new PropertyChangedCallback(OnTooltipTemplateChanged)));

		/// <summary>
		/// Identifies the <c>EnableTooltip</c> dependency property.
		/// </summary>        
		/// <value>
		/// The identifier for <c>EnableTooltip</c> dependency property.
		/// </value>
		public static readonly DependencyProperty EnableTooltipProperty =
            DependencyProperty.Register(nameof(EnableTooltip), typeof(bool), typeof(SfFunnelChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnEnableTooltipChanged)));

        /// <summary>
        /// Identifies the <see cref="ShowDataLabels"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ShowDataLabels</c> dependency property and it default value is false.
        /// </value>   
        public static readonly DependencyProperty ShowDataLabelsProperty =
            DependencyProperty.Register(nameof(ShowDataLabels), typeof(bool), typeof(SfFunnelChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnShowDataLabelsChanged)));

        /// <summary>
        /// The DependencyProperty for <see cref="Series"/> property.
        /// </summary>
        internal static readonly DependencyProperty FunnelProperty =
#pragma warning disable CS0618 // Type or member is obsolete
            DependencyProperty.Register(nameof(Funnel), typeof(FunnelSeries), typeof(SfFunnelChart),
#pragma warning restore CS0618 // Type or member is obsolete
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Series"/> property.
        /// </summary>
        internal static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(nameof(Series), typeof(ChartSeriesCollection), typeof(SfFunnelChart),
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfFunnelChart"/>.
        /// </summary>
        public SfFunnelChart()
        {
#if NETCORE
#if SyncfusionLicense
           if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
               LicenseHelper.ValidateLicense();
            }
#endif
#endif
            DefaultStyleKey = typeof(SfFunnelChart);
#pragma warning disable CS0618 // Type or member is obsolete
            Funnel = new FunnelSeries();
#pragma warning restore CS0618 // Type or member is obsolete
            Series = new ChartSeriesCollection();
            Series.Add(Funnel);
            DataLabelSettings = new FunnelDataLabelSettings();
        }

        #endregion

        #region Public Property 

        /// <summary>
        /// Gets or sets a selection behavior that enables you to select or highlight a segment in a series.
        /// </summary>
        /// <value>This property takes the <see cref="DataPointSelectionBehavior"/> instance as a value, and its default value is null.</value>
        /// 
        /// <remarks>
        /// To highlight the selected segment, set the value for the <see cref="ChartSelectionBehavior.SelectionBrush"/> property in the <see cref="DataPointSelectionBehavior"/> class.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-8)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue">
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfFunnelChart.SelectionBehavior>
        ///          <chart:DataPointSelectionBehavior SelectionBrush="Red" />
        ///     </chart:SfFunnelChart.SelectionBehavior>
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-9)
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
        /// Gets or sets the list of brushes that can be used to customize the appearance of the chart.
        /// </summary>
        /// <remarks>It allows custom brushes, and gradient brushes to customize the appearance.</remarks>
        /// <value>This property accepts a list of brushes as input and comes with a set of predefined brushes by default.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-10)
        /// <code><![CDATA[
        /// <Grid>
        /// <Grid.Resources>
        ///    <BrushCollection x:Key="customBrushes">
        ///       <SolidColorBrush Color="#4dd0e1"/>
        ///       <SolidColorBrush Color="#26c6da"/>
        ///       <SolidColorBrush Color="#00bcd4"/>
        ///       <SolidColorBrush Color="#00acc1"/>
        ///       <SolidColorBrush Color="#0097a7"/>
        ///       <SolidColorBrush Color="#00838f"/>
        ///    </BrushCollection>
        /// </Grid.Resources>
        /// 
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}"  
        ///                      XBindingPath="Category"
        ///                      YBindingPath="Value"
        ///                      PaletteBrushes="{StaticResource customBrushes}">
        ///
        /// </chart:SfFunnelChart>
        /// </Grid>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-11)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        /// ViewModel viewModel = new ViewModel();
        /// List<Brush> CustomBrushes = new List<Brush>();
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 77, 208, 225)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 38, 198, 218)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 188, 212)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 172, 193)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 151, 167)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 131, 143)));
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
        /// Gets or sets the minimum width that can be used to customize the funnel chart's neck width.
        /// </summary>
        /// <value> This property accepts a double value and has a default value of 40.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-12)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource = "{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      MinimumWidth ="50">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-13)
        /// <code><![CDATA[
        /// ViewModel viewModel = new ViewModel();
        /// 
        /// SfFunnelChart chart = new SfFunnelChart();
        /// chart.DataContext = viewModel;
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.MinimumWidth = 50;
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double MinimumWidth
        {
            get { return (double)GetValue(MinimumWidthProperty); }
            set { SetValue(MinimumWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate that can be used to customize the appearance of the tooltip.
        /// </summary>
        /// <value>
        /// This accepts a <see cref="DataTemplate"/> value.
        /// </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-14)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}"
        ///                      XBindingPath="XValue"
        ///                      YBindingPath="YValue"
        ///                      EnableTooltip="True" >
        ///     <chart:SfFunnelChart.TooltipTemplate>
        ///         <DataTemplate>
        ///            <Border Background = "DarkGreen"
        ///                    CornerRadius="5"
        ///                    BorderThickness="2"
        ///                    BorderBrush="Black"
        ///                    Width="50" Height="30">
        ///                <TextBlock Text = "{Binding Item.YValue}"
        ///                           Foreground="White"
        ///                           FontWeight="Bold"
        ///                           HorizontalAlignment="Center"
        ///                           VerticalAlignment="Center"/>
        ///            </Border>
        ///        </DataTemplate>
        ///     </chart:SfFunnelChart.TooltipTemplate>
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// </example>
        public DataTemplate TooltipTemplate
        {
            get { return (DataTemplate)GetValue(TooltipTemplateProperty); }
            set { SetValue(TooltipTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the tooltip for the series should be shown or hidden.
        /// </summary>
        /// <remarks>The series tooltip will appear when you click or tap the series area.</remarks>
        /// <value>It accepts bool values, and its default value is <c>False</c>.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-15)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      EnableTooltip="True">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-16)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewModel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.EnableTooltip = true;
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
        /// Gets or sets a collection of data points that will be used to generate a chart.
        /// </summary>
        /// <example>
        /// <value>It accepts the data points collections.</value>
        /// # [MainWindow.xaml](#tab/tabid-17)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-18)
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
            get
            {
                return (object)GetValue(ItemsSourceProperty);
            }

            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a x value to the series.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the primary axis and its default value is null.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-19)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-20)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewmodel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string XBindingPath
        {
            get
            {
                return (string)GetValue(XBindingPathProperty);
            }

            set
            {
                SetValue(XBindingPathProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the data labels for the funnel chart should be enabled.
        /// </summary>
        /// <value>It takes the bool value, and its default value is false.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-21)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      ShowDataLabels="True">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-22)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///     
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewmodel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.ShowDataLabels = true;
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowDataLabels
        {
            get
            {
                return (bool)GetValue(ShowDataLabelsProperty);
            }

            set
            {
                SetValue(ShowDataLabelsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value to customize the appearance of the displaying data labels in the funnel chart.
        /// </summary>
        /// <remarks> This allows us to change the look of the displaying labels' content, shapes, and connector lines at the data point.</remarks>
        /// <value>
        /// It takes the <see cref="FunnelDataLabelSettings"/>.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-23)
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
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-24)
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
            get
            {
                return (FunnelDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }

            set
            {
                SetValue(DataLabelSettingsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a y value to the series.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the y plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-25)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue">
        ///
        ///    <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-26)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewmodel.Data;
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
        /// Gets or sets a value that can be used to explode any specific segment.
        /// </summary>
        /// <value>It accepts integer values, and its default value is -1.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-27)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      ExplodeIndex ="1">
        ///
        ///    <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-28)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewmodel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.ExplodeIndex = 1;
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public int ExplodeIndex
        {
            get { return (int)GetValue(ExplodeIndexProperty); }
            set { SetValue(ExplodeIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether segment slices will explode when clicked or tapped.
        /// </summary>
        /// <remarks>
        /// The segment will explode when you click or tap it.
        /// </remarks>
        /// <value>It accepts bool values, and its default value is false.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-29)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      ExplodeOnTap ="True">
        ///
        ///    <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-30)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewmodel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.ExplodeOnTap = true;
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ExplodeOnTap
        {
            get { return (bool)GetValue(ExplodeOnTapProperty); }
            set { SetValue(ExplodeOnTapProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ratio of the distance between the funnel segment blocks.
        /// </summary>
        /// <value>It accepts double values, and its default value is 0. Its value ranges from 0 to 1. </value>
        /// <remarks>It is used to provide the spacing between the segments</remarks>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-31)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      GapRatio="0.5">
        ///
        ///     <!--omitted for brevity-->
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
        /// chart.GapRatio = 0.5;
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double GapRatio
        {
            get { return (double)GetValue(GapRatioProperty); }
            set { SetValue(GapRatioProperty, value); }
        }

        /// <summary>
        /// Gets or sets the distance from the origination positions where the segment is exploded when the <see cref="ExplodeIndex"/> value is given.
        /// </summary>
        /// <value>It accepts double values, and its default value is 40.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-33)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      ExplodeOffset="50">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-34)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewmodel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.ExplodeOffset = 50;
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double ExplodeOffset
        {
            get { return (double)GetValue(ExplodeOffsetProperty); }
            set { SetValue(ExplodeOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the y-value should interpret the length or surface of the funnel block.
        /// </summary>
        /// <value>
        /// It accepts <see cref="ChartFunnelMode"/> values and its default value is <see cref="ChartFunnelMode.ValueIsHeight"/>.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-35)
        /// <code><![CDATA[
        /// <chart:SfFunnelChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      Mode="ValueIsHeight">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-36)
        /// <code><![CDATA[
        /// SfFunnelChart chart = new SfFunnelChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewmodel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.Mode = ChartFunnelMode.ValueIsHeight;
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartFunnelMode Mode
        {
            get { return (ChartFunnelMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        #endregion

        #region Internal Property

#pragma warning disable CS0618 // Type or member is obsolete
        internal FunnelSeries Funnel
        {
            get { return (FunnelSeries)GetValue(FunnelProperty); }
#pragma warning restore CS0618 // Type or member is obsolete
            set { SetValue(FunnelProperty, value); }
        }

        internal ChartSeriesCollection Series
        {
            get { return (ChartSeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        #endregion

        #region Protected override Method

        protected override void OnApplyTemplate()
        {
            FunnelPlotArea plotArea = GetTemplateChild("FunnelPlotArea") as FunnelPlotArea;
            plotArea.FunnelSeries = Funnel;
            PlotArea = plotArea;

            base.OnApplyTemplate();

            FunnelAreaPanel areaPanel = AreaPanel as FunnelAreaPanel;
            areaPanel.Chart = this;
            areaPanel.FunnelSeries = Funnel;
        }

        /// <inheritdoc/>
        internal override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (Funnel != null)
            {
                Funnel = null;
            }

            SelectionBehavior = null;
        }

        #endregion

        #region Private Static Methods

        private static void OnGapRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.GapRatio = funnelChart.GapRatio;
            }
        }

        private static void OnExplodeOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.ExplodeOffset = funnelChart.ExplodeOffset;
            }
        }

        private static void OnFunneldModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.FunnelMode = funnelChart.Mode;
            }

        }

        private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.YBindingPath = funnelChart.YBindingPath;
            }
        }

        private static void OnExplodeIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.ExplodeIndex = funnelChart.ExplodeIndex;
            }
        }

        private static void OnAdornmentsInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.DataLabelSettings = funnelChart.DataLabelSettings;
            }
        }
        private static void OnBindingPathXChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SfFunnelChart funnelChart = obj as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.XBindingPath = funnelChart.XBindingPath;
            }
        }
        private static void OnDataSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SfFunnelChart funnelChart = obj as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.ItemsSource = funnelChart.ItemsSource;
            }
        }

        private static void OnTooltipTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.TooltipTemplate = funnelChart.TooltipTemplate;
            }
        }

        private static void OnEnableTooltipChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.EnableTooltip = funnelChart.EnableTooltip;
            }
        }

        private static void OnMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.MinWidth = funnelChart.MinimumWidth;
            }
        }

        private static void OnExplodeOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.ExplodeOnTap = funnelChart.ExplodeOnTap;
            }
        }

        private static void OnShowDataLabelsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.ShowDataLabels = funnelChart.ShowDataLabels;
            }
        }

        private static void OnSelectionBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.SelectionBehavior = funnelChart.SelectionBehavior;
            }
        }

        #endregion

        internal override IList GetSeriesCollection()
        {
            return Series;
        }

        internal override ObservableCollection<ChartSeries> GetChartSeriesCollection()
        {
            return new ObservableCollection<ChartSeries>(Series);
        }

        internal override void SetSeriesCollection(IList seriesCollection)
        {
            Series = (ChartSeriesCollection)seriesCollection;
        }

        internal override bool IsNullPaletteBrushes()
        {
            if (this.PaletteBrushes == null)
                return true;

            return false;
        }

        private static void OnPaletteBrushesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SfFunnelChart)d).OnPaletteBrushesChanged(e);
        }

        private void OnPaletteBrushesChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Funnel != null)
            {
                Funnel.PaletteBrushes = this.PaletteBrushes;
            }
        }
    }
}
