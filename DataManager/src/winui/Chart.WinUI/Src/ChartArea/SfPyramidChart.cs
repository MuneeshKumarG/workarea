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
    /// Renders pyramid chart for more user-friendly data representation and greater UI visualization.
    /// </summary>
    /// <remarks>
    /// <para>Pyramid chart control is used to visualize the proportions of a total in hierarchies.</para>
    ///
    /// <para>SfPyramidChart class allows to customize the chart elements such as legend, data label, and tooltip features. </para>
    ///
    /// <para><b>Legend</b></para>
    /// 
    /// <para>The Legend contains list of data points in chart series. The information provided in each legend item helps to identify the corresponding data point in chart series. The Series <see cref="ChartSeries.XBindingPath"/> property value will be displayed in the associated legend item.</para>
    /// 
    /// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="ChartBase.Legend"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code> <![CDATA[
    /// <chart:SfPyramidChart ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue">
    ///
    ///        <chart:SfPyramidChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfPyramidChart.DataContext>
    ///        <chart:SfPyramidChart.Legend>
    ///            <chart:ChartLegend/>
    ///        </chart:SfPyramidChart.Legend>
    ///    
    /// </chart:SfPyramidChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// ViewModel viewModel = new ViewModel();
    /// 
    /// SfPyramidChart chart = new SfPyramidChart()
    /// {
    ///     ItemsSource = viewmodel.Data,
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
    /// <chart:SfPyramidChart EnableTooltip = "True"
    ///                       ItemsSource="{Binding Data}"
    ///                       XBindingPath="XValue"
    ///                       YBindingPath="YValue">
    /// 
    ///         <chart:SfPyramidChart.DataContext>
    ///             <local:ViewModel/>
    ///         </chart:SfPyramidChart.DataContext>
    /// 
    ///         <chart:SfPyramidChart.TooltipBehavior>
    ///             <chart:ChartTooltipBehavior/>
    ///         </chart:SfPyramidChart.TooltipBehavior>
    /// 
    /// </chart:SfPyramidChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-5)
    /// <code><![CDATA[
    /// ViewModel viewModel = new ViewModel();
    /// 
    /// SfPyramidChart chart = new SfPyramidChart()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue",
    ///    EnableTooltip = true
    /// };
    /// 
    /// chart.DataContext = viewModel;
    ///
    /// chart.TooltipBehavior = new ChartTooltipBehavior();
    ///
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Data Label</b></para>
    /// 
    /// <para>Data labels are used to display values related to a chart segment. To render the data labels, you need to enable the <see cref="ShowDataLabels"/> property as <b>true</b> in <b>SfPyramidChart</b> class. </para>
    /// 
    /// <para>To customize the chart data labels alignment, placement and label styles, need to create an instance of <see cref="PyramidDataLabelSettings"/> and set to the <see cref="SfPyramidChart.DataLabelSettings"/> property.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-6)
    /// <code><![CDATA[
    /// <chart:SfPyramidChart ShowDataLabels = "True"
    ///                       ItemsSource="{Binding Data}"
    ///                       XBindingPath="XValue"
    ///                       YBindingPath="YValue">
    ///
    ///        <chart:SfPyramidChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfPyramidChart.DataContext>
    ///    
    /// </chart:SfPyramidChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-7)
    /// <code><![CDATA[
    /// ViewModel viewModel = new ViewModel();
    /// 
    /// SfPyramidChart chart = new SfPyramidChart()
    /// {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue",
    ///     ShowDataLabels = true
    /// };
    /// 
    /// chart.DataContext = viewModel;
    /// 
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    public class SfPyramidChart : ChartBase
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="SelectionBehavior"/> property.       .
        /// </summary>
        public static readonly DependencyProperty SelectionBehaviorProperty =
            DependencyProperty.Register(
                nameof(SelectionBehavior),
                typeof(DataPointSelectionBehavior),
                typeof(SfPyramidChart),
                new PropertyMetadata(null, OnSelectionBehaviorChanged));

        /// Identifies the <see cref="PaletteBrushes"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PaletteBrushes</c> dependency property.
        /// </value>      
        public static readonly DependencyProperty PaletteBrushesProperty =
            DependencyProperty.Register(nameof(PaletteBrushes), typeof(IList<Brush>), typeof(SfPyramidChart),
                new PropertyMetadata(ChartColorModel.DefaultBrushes, OnPaletteBrushesChanged));

        /// <summary>
        /// Identifies the <c>YBindingPath</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>YBindingPath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty YBindingPathProperty =
            DependencyProperty.Register(nameof(YBindingPath), typeof(string), typeof(SfPyramidChart),
            new PropertyMetadata(null, OnYPathChanged));

        /// <summary>
        /// Identifies the <c>ExplodeIndex</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ExplodeIndex</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ExplodeIndexProperty =
            DependencyProperty.Register(nameof(ExplodeIndex), typeof(int), typeof(SfPyramidChart),
            new PropertyMetadata(-1, OnExplodeIndexChanged));


        /// <summary>
        /// Identifies the <c>ExplodeOnTap</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ExplodeOnTap</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ExplodeOnTapProperty =
            DependencyProperty.Register(nameof(ExplodeOnTap), typeof(bool), typeof(SfPyramidChart),
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
                typeof(SfPyramidChart),
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
                typeof(SfPyramidChart),
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
                typeof(ChartPyramidMode),
                typeof(SfPyramidChart),
                new PropertyMetadata(ChartPyramidMode.Linear, OnPyramidModeChanged));

        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> dependency property.        
        /// </summary>        
        /// <value>
        /// The identifier for <c>DataLabelSettings</c> dependency property.
        /// </value>
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(PyramidDataLabelSettings), typeof(SfPyramidChart),
          new PropertyMetadata(null, OnAdornmentsInfoChanged));

        /// <summary>
        /// Identifies the <c>ItemsSource</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ItemsSource</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(SfPyramidChart),
            new PropertyMetadata(null, new PropertyChangedCallback(OnDataSourceChanged)));

        /// <summary>
        /// Identifies the <c>XBindingPath</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>XBindingPath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty XBindingPathProperty =
        DependencyProperty.Register(nameof(XBindingPath), typeof(string), typeof(SfPyramidChart),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnBindingPathXChanged)));

        /// <summary>
        /// Identifies the <c>TooltipTemplate</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>TooltipTemplate</c> dependency property.
        /// </value>
        public static readonly DependencyProperty TooltipTemplateProperty =
            DependencyProperty.Register(nameof(TooltipTemplate), typeof(DataTemplate), typeof(SfPyramidChart),
                new PropertyMetadata(null, new PropertyChangedCallback(OnTooltipTemplateChanged)));

		/// <summary>
		/// Identifies the <c>EnableTooltip</c> dependency property.
		/// </summary>        
		/// <value>
		/// The identifier for <c>EnableTooltip</c> dependency property.
		/// </value>
		public static readonly DependencyProperty EnableTooltipProperty =
            DependencyProperty.Register(nameof(EnableTooltip), typeof(bool), typeof(SfPyramidChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnEnableTooltipChanged)));

        /// <summary>
        /// Identifies the <see cref="ShowDataLabels"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ShowDataLabels</c> dependency property and its default value is false.
        /// </value>   
        public static readonly DependencyProperty ShowDataLabelsProperty =
            DependencyProperty.Register(nameof(ShowDataLabels), typeof(bool), typeof(SfPyramidChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnShowDataLabelsChanged)));

        /// <summary>
        /// The DependencyProperty for <see cref="Series"/> property.
        /// </summary>
        internal static readonly DependencyProperty PyramidProperty =
#pragma warning disable CS0618 // Type or member is obsolete
            DependencyProperty.Register(nameof(Pyramid), typeof(PyramidSeries), typeof(SfPyramidChart),
#pragma warning restore CS0618 // Type or member is obsolete
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Series"/> property.
        /// </summary>
        internal static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(nameof(Series), typeof(ChartSeriesCollection), typeof(SfPyramidChart),
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfPyramidChart"/>.
        /// </summary>
        public SfPyramidChart()
        {
#if NETCORE
#if SyncfusionLicense
           if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
               LicenseHelper.ValidateLicense();
            }
#endif
#endif
            DefaultStyleKey = typeof(SfPyramidChart);
#pragma warning disable CS0618 // Type or member is obsolete
            Pyramid = new PyramidSeries();
#pragma warning restore CS0618 // Type or member is obsolete
            Series = new ChartSeriesCollection();
            Series.Add(Pyramid);
            DataLabelSettings = new PyramidDataLabelSettings();
        }
        #endregion

        #region Public Property 

        /// <summary>
        /// Gets or sets a selection behavior that enables you to select or highlight a segment in a series.
        /// </summary>
        /// <value>This property takes the <see cref="DataPointSelectionBehavior"/> instance as a value, and its default value is null.</value>
        /// 
        /// <remarks>
        /// <para>To highlight the selected segment, set the value for the <see cref="ChartSelectionBehavior.SelectionBrush"/> property in the <see cref="DataPointSelectionBehavior"/> class.</para>
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-8)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue">
        /// 
        ///     <!--omitted for brevity-->
        /// 
        ///     <chart:SfPyramidChart.SelectionBehavior>
        ///          <chart:DataPointSelectionBehavior SelectionBrush="Red" />
        ///     </chart:SfPyramidChart.SelectionBehavior>
        /// 
        /// </chart:SfPyramidChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-9)
        /// <code><![CDATA[
        /// ViewModel viewModel = new ViewModel();
        /// 
        /// SfPyramidChart chart = new SfPyramidChart();
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
        /// <chart:SfPyramidChart  ItemsSource="{Binding Data}" 
        ///                        XBindingPath="Category"
        ///                        YBindingPath="Value"
        ///                        PaletteBrushes="{StaticResource customBrushes}">
        ///
        /// </chart:SfPyramidChart>
        /// </Grid>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-11)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
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
        /// Gets or sets the DataTemplate that can be used to customize the appearance of the tooltip.
        /// </summary>
        /// <value>
        /// This accepts a <see cref="DataTemplate"/> value.
        /// </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-12)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True" >
        ///     <chart:SfPyramidChart.TooltipTemplate>
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
        ///     </chart:SfPyramidChart.TooltipTemplate>
        /// </chart:SfPyramidChart>
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
        /// # [MainWindow.xaml](#tab/tabid-13)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}" 
        ///                       XBindingPath="XValue" 
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfPyramidChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-14)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewmodel.Data;
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
        /// <value>It accepts the data points collections.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-15)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}" 
        ///                       XBindingPath="XValue" 
        ///                       YBindingPath="YValue">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfPyramidChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-16)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
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
        /// # [MainWindow.xaml](#tab/tabid-17)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}" 
        ///                       XBindingPath="XValue" 
        ///                       YBindingPath="YValue">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfPyramidChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-18)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
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
        /// <value>It takes bool values, and its default value is false.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-19)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}" 
        ///                       XBindingPath="XValue" 
        ///                       YBindingPath="YValue"
        ///                       ShowDataLabels="True">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfPyramidChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-20)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
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
        /// Gets or sets a value to customize the appearance of the displaying data labels in the pyramid chart.
        /// </summary>
        /// <remarks> This allows us to change the look of the displaying labels' content, shapes, and connector lines at the data point.</remarks>
        /// <value>
        /// It takes the <see cref="PyramidDataLabelSettings"/>.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-21)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}" 
        ///                       XBindingPath="XValue" 
        ///                       YBindingPath="YValue"
        ///                       ShowDataLabels="True">
        ///
        ///     <chart:SfPyramidChart.DataLabelSettings>
        ///         <chart:PyramidDataLabelSettings />
        ///     </chart:SfPyramidChart.DataLabelSettings>
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfPyramidChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-22)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewmodel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.ShowDataLabels = true;
        /// chart.DataLabelSettings = new PyramidDataLabelSettings();
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public PyramidDataLabelSettings DataLabelSettings
        {
            get
            {
                return (PyramidDataLabelSettings)GetValue(DataLabelSettingsProperty);
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
        /// # [MainWindow.xaml](#tab/tabid-23)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}" 
        ///                       XBindingPath="XValue" 
        ///                       YBindingPath="YValue">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfPyramidChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-24)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
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
        /// # [MainWindow.xaml](#tab/tabid-25)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      ExplodeIndex ="1">
        ///
        ///    <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-26)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
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
        /// # [MainWindow.xaml](#tab/tabid-27)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      ExplodeOnTap ="True">
        ///
        ///    <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-28)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
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
        /// Gets or sets the ratio of the distance between the pyramid segment blocks.
        /// </summary>
        /// <value>It accepts double values, and its default value is 0. Its value ranges from 0 to 1.</value>
        /// <remarks>It is used to provide the spacing between the segments.</remarks>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-29)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}" 
        ///                       XBindingPath="XValue" 
        ///                       YBindingPath="YValue"
        ///                       GapRatio="0.5">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfPyramidChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-30)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
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
        /// # [MainWindow.xaml](#tab/tabid-31)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}" 
        ///                      XBindingPath="XValue" 
        ///                      YBindingPath="YValue"
        ///                      ExplodeOffset="50">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-32)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
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
        /// Gets or sets a value indicating whether the y value should interpret the height or area of the pyramid block.
        /// </summary>
        /// <value>
        /// It accepts the <see cref="ChartPyramidMode"/> values and its default value is <see cref="ChartPyramidMode.Linear"/>.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-33)
        /// <code><![CDATA[
        /// <chart:SfPyramidChart ItemsSource="{Binding Data}" 
        ///                       XBindingPath="XValue" 
        ///                       YBindingPath="YValue"
        ///                       Mode="Surface">
        ///
        ///     <!--omitted for brevity-->
        ///
        /// </chart:SfFunnelChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-34)
        /// <code><![CDATA[
        /// SfPyramidChart chart = new SfPyramidChart();
        ///
        /// ViewModel viewmodel = new ViewModel();
        ///
        /// chart.ItemsSource = viewmodel.Data;
        /// chart.XBindingPath = "XValue";
        /// chart.YBindingPath = "YValue";
        /// chart.Mode = ChartPyramidMode.Surface;
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        public ChartPyramidMode Mode
        {
            get { return (ChartPyramidMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        #endregion

        #region Internal Property

#pragma warning disable CS0618 // Type or member is obsolete
        internal PyramidSeries Pyramid
        {
            get { return (PyramidSeries)GetValue(PyramidProperty); }
#pragma warning restore CS0618 // Type or member is obsolete
            set { SetValue(PyramidProperty, value); }
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
            PyramidPlotArea plotArea = GetTemplateChild("PyramidPlotArea") as PyramidPlotArea;
            plotArea.PyramidSeries = Pyramid;

            PlotArea = plotArea;
            PlotArea.Legend = Legend;

            base.OnApplyTemplate();

            PyramidAreaPanel areaPanel = AreaPanel as PyramidAreaPanel;
            areaPanel.Chart = this;
            areaPanel.PyramidSeries = Pyramid;
        }

        /// <inheritdoc/>
        internal override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (Pyramid != null)
            {
                Pyramid = null;
            }

            SelectionBehavior = null;
        }

        #endregion

        #region Private Static Methods

        private static void OnGapRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.GapRatio = pyramidChart.GapRatio;
            }
        }

        private static void OnExplodeOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.ExplodeOffset = pyramidChart.ExplodeOffset;
            }
        }

        private static void OnPyramidModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.PyramidMode = pyramidChart.Mode;
            }
               
        }

        private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.YBindingPath = pyramidChart.YBindingPath;
            }
        }


        private static void OnExplodeIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.ExplodeIndex = pyramidChart.ExplodeIndex;
            }
        }

        private static void OnAdornmentsInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.DataLabelSettings = pyramidChart.DataLabelSettings;
            }
        }
        private static void OnBindingPathXChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SfPyramidChart pyramidChart = obj as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
               pyramidChart.Pyramid.XBindingPath = pyramidChart.XBindingPath;
            }
        }
        private static void OnDataSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SfPyramidChart pyramidChart = obj as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.ItemsSource = pyramidChart.ItemsSource;
            }
        }

        private static void OnTooltipTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.TooltipTemplate = pyramidChart.TooltipTemplate;
            }
        }

        private static void OnEnableTooltipChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.EnableTooltip = pyramidChart.EnableTooltip;
            }
        }

        private static void OnExplodeOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.ExplodeOnTap = pyramidChart.ExplodeOnTap;
            }
        }

        private static void OnShowDataLabelsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.ShowDataLabels = pyramidChart.ShowDataLabels;
            }
        }

        private static void OnSelectionBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.SelectionBehavior = pyramidChart.SelectionBehavior;
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
            ((SfPyramidChart)d).OnPaletteBrushesChanged(e);
        }

        private void OnPaletteBrushesChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Pyramid != null)
            {
                Pyramid.PaletteBrushes = this.PaletteBrushes;
            }
        }
    }
}
