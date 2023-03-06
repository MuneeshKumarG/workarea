using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using Windows.Foundation;
using System.Collections.ObjectModel;
using Microsoft.UI;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml.Shapes;
using Windows.UI;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class ChartSeries : Control
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="SelectionBehavior"/> property.       .
        /// </summary>
        public static readonly DependencyProperty SelectionBehaviorProperty =
            DependencyProperty.Register(
                nameof(SelectionBehavior),
                typeof(DataPointSelectionBehavior),
                typeof(ChartSeries),
                new PropertyMetadata(null, OnBehaviorChanged));

        /// <summary>
        /// Identifies the <c>Spacing</c> dependency property. This is attached property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Spacing</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.RegisterAttached("Spacing", typeof(double), typeof(ChartSeries),
                new PropertyMetadata(0.2d, OnSegmentSpacingChanged));

        /// <summary>
        /// Identifies the <see cref="TooltipTemplate"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>TooltipTemplate</c> dependency property.
        /// </value>
        public static readonly DependencyProperty TooltipTemplateProperty =
            DependencyProperty.Register(nameof(TooltipTemplate), typeof(DataTemplate), typeof(ChartSeries),
                new PropertyMetadata(null, new PropertyChangedCallback(OnTooltipTemplateChanged)));

        /// <summary>
        /// Identifies the <see cref="EnableTooltip"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>EnableTooltip</c> dependency property.
        /// </value>
        public static readonly DependencyProperty EnableTooltipProperty =
            DependencyProperty.Register(nameof(EnableTooltip), typeof(bool), typeof(ChartSeries),
                new PropertyMetadata(false, new PropertyChangedCallback(OnEnableTooltipChanged)));

        /// <summary>
        /// Identifies the <see cref="ListenPropertyChange"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ListenPropertyChange</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ListenPropertyChangeProperty =
            DependencyProperty.Register(nameof(ListenPropertyChange), typeof(bool), typeof(ChartSeries),
                new PropertyMetadata(false, new PropertyChangedCallback(OnListenPropertyChangeChanged)));

        /// <summary>
        /// Identifies the <see cref="IsSeriesVisible"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>IsSeriesVisible</c> dependency property.
        /// </value>
        public static readonly DependencyProperty IsSeriesVisibleProperty =
            DependencyProperty.Register(nameof(IsSeriesVisible), typeof(bool), typeof(ChartSeries),
                new PropertyMetadata(true, OnIsSeriesVisibleChanged));

        /// <summary>
        /// Identifies the <see cref="XBindingPath"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>XBindingPath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty XBindingPathProperty =
        DependencyProperty.Register(nameof(XBindingPath), typeof(string), typeof(ChartSeries),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnBindingPathXChanged)));

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ItemsSource</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(ChartSeries),
            new PropertyMetadata(null, new PropertyChangedCallback(OnDataSourceChanged)));

        /// <summary>
        /// Identifies the <see cref="TrackballLabelTemplate"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>TrackballLabelTemplate</c> dependency property.
        /// </value>
        public static readonly DependencyProperty TrackballLabelTemplateProperty =
            DependencyProperty.Register(nameof(TrackballLabelTemplate), typeof(DataTemplate), typeof(ChartSeries), null);

        /// <summary>
        /// Identifies the <see cref="Fill"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Interior</c> dependency property.
        /// </value>
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(nameof(Fill), typeof(Brush), typeof(ChartSeries),
                new PropertyMetadata(null, OnAppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Label"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Label</c> dependency property.
        /// </value>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(ChartSeries), new PropertyMetadata(string.Empty, OnLabelPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="LegendIcon"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>LegendIcon</c> dependency property.
        /// </value>
        public static readonly DependencyProperty LegendIconProperty =
            DependencyProperty.Register(nameof(LegendIcon), typeof(ChartLegendIcon), typeof(ChartSeries),
                new PropertyMetadata(ChartLegendIcon.SeriesType, OnLegendIconChanged));

        /// <summary>
        /// Identifies the <see cref="LegendIconTemplate"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>LegendIconTemplate</c> dependency property.
        /// </value>
        public static readonly DependencyProperty LegendIconTemplateProperty =
            DependencyProperty.Register(nameof(LegendIconTemplate), typeof(DataTemplate), typeof(ChartSeries), new PropertyMetadata(null, OnLegendIconTemplateChanged));

        /// <summary>
        /// Identifies the <see cref="IsVisibleOnLegend"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>IsVisibleOnLegend</c> dependency property.
        /// </value>
        public static readonly DependencyProperty IsVisibleOnLegendProperty =
            DependencyProperty.Register(nameof(IsVisibleOnLegend), typeof(bool), typeof(ChartSeries),
                new PropertyMetadata(true, new PropertyChangedCallback(OnVisibilityOnLegendChanged)));

        /// <summary>
        /// Identifies the <see cref="EnableAnimation"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>EnableAnimation</c> dependency property.
        /// </value>
        public static readonly DependencyProperty EnableAnimationProperty =
            DependencyProperty.Register(nameof(EnableAnimation), typeof(bool), typeof(ChartSeries), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="AnimationDuration"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>AnimationDuration</c> dependency property.
        /// </value>
        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register(nameof(AnimationDuration), typeof(TimeSpan), typeof(ChartSeries),
                new PropertyMetadata(TimeSpan.FromSeconds(0.8)));

        /// <summary>
        /// Identifies the <see cref="PaletteBrushes"/> dependency property.
        /// </summary>  
        ///  /// <value>
        /// The identifier for <c>PaletteBrushes</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PaletteBrushesProperty =
            DependencyProperty.Register(nameof(PaletteBrushes), typeof(IList<Brush>), typeof(ChartSeries),
                new PropertyMetadata(null, OnPaletteBrushesChanged));

        /// <summary>
        /// Identifies the <see cref="ShowDataLabels"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ShowDataLabels</c> dependency property and its default value is false.
        /// </value>   
        public static readonly DependencyProperty ShowDataLabelsProperty =
            DependencyProperty.Register(nameof(ShowDataLabels), typeof(bool), typeof(ChartSeries),
                new PropertyMetadata(false, new PropertyChangedCallback(OnShowDataLabelsChanged)));

        /// <summary>
        /// Identifies the <see cref="ActualTrackballLabelTemplate"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ActualTrackballLabelTemplate</c> dependency property.
        /// </value>
        internal static readonly DependencyProperty ActualTrackballLabelTemplateProperty =
            DependencyProperty.Register(nameof(ActualTrackballLabelTemplate), typeof(DataTemplate), typeof(ChartSeries), null);

        #endregion

        #region Fields

        #region Internal Fields

        internal List<Rect> bitmapRects = new List<Rect>();

        internal bool IsStacked100;

        internal List<int> bitmapPixels = new List<int>();

        internal ChartDataPointInfo? dataPoint;

        internal ChartDataLabelSettings adornmentInfo;

        internal List<double> GroupedXValuesIndexes = new List<double>();

        internal List<string> GroupedXValues = new List<string>();

        internal Dictionary<double, List<int>> DistinctValuesIndexes = new Dictionary<double, List<int>>();

        internal List<object> GroupedActualData = new List<object>();

        internal int PreviousSelectedIndex = -1;

        internal List<int> ToggledLegendIndex;

        internal ChartSeriesPanel SeriesPanel;

        internal bool triggerSelectionChangedEventOnLoad;

        internal int UpdateStartedIndex = -1;

        internal bool canAnimate;

        internal DoubleAnimation scaleXAnimation;

        internal DoubleAnimation scaleYAnimation;

        internal Storyboard storyBoard;

        internal Point mousePosition;

        internal Point previousMousePosition;

        internal List<int> selectedSegmentPixels = new List<int>();

        internal HashSet<int> upperSeriesPixels = new HashSet<int>();

        internal bool HastoolTip = false;

        internal DispatcherTimer Timer;

        internal DispatcherTimer InitialDelayTimer;

        internal IChartTransformer ChartTransformer;

        #endregion

        #region Private Fields

        internal bool isTotalCalculated;

        private double grandTotal = 0d;

        private ObservableCollection<int> _selectedSegmentsIndexes;

        private bool isLoading = true;

        private ObservableCollection<ChartDataLabel> m_adornments = new ObservableCollection<ChartDataLabel>();

        private ObservableCollection<ChartDataLabel> m_visibleAdornments = new ObservableCollection<ChartDataLabel>();

        bool isTap;

        DataTemplate toolTipTemplate;

        private bool isNotificationSuspended;

        private bool isPropertyNotificationSuspended;

        private bool isUpdateStarted;

        private DataTemplate defaultTooltipTemplate;

        private ChartAxis actualXAxis;

        private ChartAxis actualYAxis;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSeries"/>.
        /// </summary>
        public ChartSeries()
        {
            DefaultStyleKey = typeof(ChartSeries);
            Segments = new ObservableCollection<ChartSegment>();
            SelectedSegmentsIndexes = new ObservableCollection<int>();
            ToggledLegendIndex = new List<int>();
            XValues = ActualXValues = new List<double>();
            ActualData = new List<object>();
            CanAnimate = true;
            Timer = new DispatcherTimer();
            Timer.Tick += Timer_Tick;
            InitialDelayTimer = new DispatcherTimer();
            Pixels = new HashSet<int>();
            InitialDelayTimer.Tick += InitialDelayTimer_Tick;
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a selection behavior that enables you to select or highlight a segment in the series.
        /// </summary>
        /// <value>This property takes the a<see cref="DataPointSelectionBehavior"/> instance as a value, and its default value is null.</value>
        ///
        /// <remarks>
        /// <para>To highlight the selected data point, set the value for the <see cref="ChartSelectionBehavior.SelectionBrush"/> property in the <see cref="DataPointSelectionBehavior"/> class.</para>
        /// </remarks>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///     
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                           XBindingPath="XValue"
        ///                           YBindingPath="YValue">
        ///              <chart:PieSeries.SelectionBehavior>
        ///                  <chart:DataPointSelectionBehavior SelectionBrush = "Red" />
        ///              </chart:PieSeries.SelectionBehavior>
        ///          </chart:PieSeries>
        ///
        ///     </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     PieSeries series = new PieSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.SelectionBehavior = new DataPointSelectionBehavior()
        ///     {
        ///        SelectionBrush = new SolidColorBrush(Colors.Red),
        ///     };
        ///     chart.Series.Add(series);
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
        /// Gets or sets the list of brushes that can be used to customize the appearance of the series.
        /// </summary>
        /// <remarks>It allows custom brushes, and gradient brushes to customize the appearance.</remarks>
        /// <value>This property accepts a list of brushes as input and comes with a set of predefined brushes by default.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <Grid>
        /// <Grid.Resources>
        /// <BrushCollection x:Key="customBrushes">
        ///       <SolidColorBrush Color="#4dd0e1"/>
        ///       <SolidColorBrush Color="#26c6da"/>
        ///       <SolidColorBrush Color="#00bcd4"/>
        ///       <SolidColorBrush Color="#00acc1"/>
        ///       <SolidColorBrush Color="#0097a7"/>
        ///       <SolidColorBrush Color="#00838f"/>
        ///    </BrushCollection>
        /// </Grid.Resources>
        /// 
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     
        ///        <chart:ColumnSeries ItemsSource = "{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            PaletteBrushes="{StaticResource customBrushes}" />
        ///
        ///     </chart:SfCartesianChart>
        /// </Grid>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        /// List<Brush> CustomBrushes = new List<Brush>();
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 77, 208, 225)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 38, 198, 218)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 188, 212)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 172, 193)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 151, 167)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 131, 143)));
        ///
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // Eliminated for simplicity
        /// 
        ///  ColumnSeries columnSeries = new ColumnSeries()
        /// {
        ///      ItemsSource = viewModel.Data,
        ///      XBindingPath = "XValue",
        ///      YBindingPath = "YValue",
        ///      PaletteBrushes = CustomBrushes,
        /// };
        ///
        ///     chart.Series.Add(columnSeries);
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
        /// It accepts a <see cref="DataTemplate"/> value.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// 
        /// <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     
        ///     <chart:ColumnSeries  ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue"
        ///                          EnableTooltip="True" >
        ///     <chart:ColumnSeries.TooltipTemplate>
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
        ///     </chart:ColumnSeries.TooltipTemplate>
        ///     </chart:ColumnSeries>
        /// </chart:SfCartesianChart>
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
        /// Gets or sets a boolean value indicating whether the tooltip for series should be shown or hidden.
        /// </summary>
        /// <remarks>The series tooltip will appear when you click or tap the series area.</remarks>
        /// <value>It accepts bool values and its default value is <c>False</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            EnableTooltip="True"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           EnableTooltip = true,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
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
        ///  Gets or sets a value that indicates whether to listen property change or not.
        /// </summary>
        /// <value>It accepts bool values and its default value is <c>False</c>.</value>
        public bool ListenPropertyChange
        {
            get { return (bool)GetValue(ListenPropertyChangeProperty); }
            set { SetValue(ListenPropertyChangeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the series is visible or not.
        /// </summary>
        /// <value>It accepts bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            IsSeriesVisible="False"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           IsSeriesVisible = false,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool IsSeriesVisible 
        {
            get { return (bool)GetValue(IsSeriesVisibleProperty); }
            set { SetValue(IsSeriesVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a data points collection that will be used to plot a chart.
        /// </summary>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-9)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
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
        /// Gets or sets the custom template to customize the appearance of the chart series trackball label.
        /// </summary>
        /// <value>
        /// It accepts a <see cref="DataTemplate"/> value.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-10)
        /// <code><![CDATA[
        /// 
        /// <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     
        ///     <chart:SfCartesianChart.TrackballBehavior>
        ///        <chart:ChartTrackballBehavior />
        ///    </chart:SfCartesianChart.TrackballBehavior>
        ///     
        ///     <chart:ColumnSeries  ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue">
        ///         <chart:ColumnSeries.TrackballLabelTemplate>
        ///             <DataTemplate>
        ///                    <Border CornerRadius = "5" BorderThickness="1" 
        ///                        BorderBrush="Black" Background="LightGreen" Padding="5">
        ///                        <TextBlock Foreground = "Black" Text="{Binding ValueY}"/>
        ///                    </Border>
        ///             </DataTemplate>
        ///         </chart:ColumnSeries.TrackballLabelTemplate>
        ///     </chart:ColumnSeries>
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// </example>
        public DataTemplate TrackballLabelTemplate 
        {
            get { return (DataTemplate)GetValue(TrackballLabelTemplateProperty); }
            set { SetValue(TrackballLabelTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a brush value to customize the series appearance.
        /// </summary>
        /// <value>It accepts a <see cref="Brush"/> value and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-11)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Fill = "Red"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Fill = new SolidColorBrush(Colors.Red),
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush Fill 
       {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that will be displayed in the associated legend item.
        /// </summary>
        /// <value>It accepts a string value and its default value is string.Empty.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-13)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            Label = "ColumnSeries"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-14)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           Label = "ColumnSeries",
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets a legend icon that will be displayed in the associated legend item.
        /// </summary>
        /// <value> It accepts <see cref="ChartLegendIcon"/> values and its default value is <see cref="ChartLegendIcon.SeriesType"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-15)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     
        ///         <chart:SfCartesianChart.Legend>
        ///            <chart:ChartLegend />
        ///         </chart:SfCartesianChart.Legend>
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            LegendIcon = "Diamond"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-16)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     chart.Legend = new ChartLegend();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           LegendIcon = ChartLegendIcon.Diamond,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartLegendIcon LegendIcon
        {
            get { return (ChartLegendIcon)GetValue(LegendIconProperty); }
            set { SetValue(LegendIconProperty, value); }
        }

        /// <summary>
        /// Gets or sets a template to customize the legend icon that appears in the associated legend item.
        /// </summary>
        /// <value> It accepts <see cref="DataTemplate"/> value and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-17)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     
        ///         <chart:SfCartesianChart.Legend>
        ///            <chart:ChartLegend />
        ///         </chart:SfCartesianChart.Legend>
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue">
        ///                 <chart:ColumnSeries.LegendIconTemplate>
        ///                   <DataTemplate x:Key="iconTemplate">
        ///                       <Ellipse Height = "10" Width="10" 
        ///                                Fill="White" Stroke="#4a4a4a"
        ///                                StrokeWidth="2"/>
        ///                    </DataTemplate>
        ///                 </chart:ColumnSeries.LegendIconTemplate>
        ///          </chart:ColumnSeries>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DataTemplate LegendIconTemplate 
        {
            get { return (DataTemplate)GetValue(LegendIconTemplateProperty); }
            set { SetValue(LegendIconTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to show a legend item for this series.
        /// </summary>
        /// <value> It accepts bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-18)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///     
        ///         <chart:SfCartesianChart.Legend>
        ///            <chart:ChartLegend />
        ///         </chart:SfCartesianChart.Legend>
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            IsVisibleOnLegend = "False"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-19)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     chart.Legend = new ChartLegend();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           IsVisibleOnLegend = false,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool IsVisibleOnLegend
        {
            get { return (bool)GetValue(IsVisibleOnLegendProperty); }
            set { SetValue(IsVisibleOnLegendProperty, value); }
        }

        /// <summary>
        /// Gets or sets a path value on the source object to serve a x value to the series.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the x plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-20)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-21)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
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
        /// Gets or sets a boolean value indicating whether to animate the chart series on loading.
        /// </summary>
        /// <value> It accepts bool values and its default value is <c>False</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-22)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            EnableAnimation = "True"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-23)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           EnableAnimation = true,
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool EnableAnimation 
        {
            get { return (bool)GetValue(EnableAnimationProperty); }
            set { SetValue(EnableAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to indicate the timeline for the animation.
        /// </summary>
        ///  <value>It accepts <see cref="TimeSpan"/> value and its default value is <c>TimeSpan.FromSeconds(0.8)</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-24)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            EnableAnimation = "True"
        ///                            AnimationDuration="00:00:02"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-25)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           EnableAnimation = true,
        ///           AnimationDuration= TimeSpan.FromSeconds(2)
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public TimeSpan AnimationDuration 
        {
            get { return (TimeSpan)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets the <see cref="ChartBase"/> instance.
        /// </summary>
        public ChartBase Chart 
        {
            get { return ActualArea; }
            internal set 
            {
                ActualArea = value;

                if (ActualArea != null) 
                {
                    ActualArea.IsLoading = true;
                }

                if (SelectionBehavior != null) 
                {
                    UpdateBehavior(null, SelectionBehavior);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates to enable the data labels for the series.
        /// </summary>
        /// <value>It accepts bool values and the default value is <c>False</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:LineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ShowDataLabels="True"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///     
        ///     LineSeries series = new LineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowDataLabels = true,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
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

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets internal DataTemplate used to display label, when ChartTrackballBehavior is used.
        /// </summary>
        /// <value>
        /// <see cref="DataTemplate"/>
        /// </value>
        internal DataTemplate ActualTrackballLabelTemplate 
        {
            get { return (DataTemplate)GetValue(ActualTrackballLabelTemplateProperty); }
            set { SetValue(ActualTrackballLabelTemplateProperty, value); }
        }

        internal bool IsLoading
        {
            get 
            {
                return isLoading;
            }

            set {
                if (isLoading == value || Chart == null) 
                {
                    return;
                }

                isLoading = value;

                if (!isLoading) 
                {
                    if (Chart.VisibleSeries != null && Chart.VisibleSeries.Count == 0) 
                    {
                        Chart.IsLoading = false;
                        return;
                    }


                    if (Chart.VisibleSeries != null) 
                    {
                        foreach (ChartSeries series in Chart.VisibleSeries) 
                        {
                            if (series.IsLoading) 
                            {
                                return;
                            }
                        }
                    }

                    Chart.IsLoading = false;
                }
                else
                {
                    Chart.IsLoading = true;
                }
            }
        }

        /// <summary>
        /// Gets the adornments collection.
        /// </summary>
        /// <value>The adornments.</value>
        internal ObservableCollection<ChartDataLabel> Adornments
        {
            get
            {
                return m_adornments;
            }
        }

        /// <summary>
        /// Gets the adornments for the visible segments.
        /// </summary>
        /// <value>The adornments.</value>
        internal ObservableCollection<ChartDataLabel> VisibleAdornments
        {
            get
            {
                return m_visibleAdornments;
            }
            set
            {
                if (m_visibleAdornments != value) 
                {
                    m_visibleAdornments = value;
                }
            }
        }

        internal ChartBase ActualArea { get; set; }

        internal Panel SeriesRootPanel { get; set; }

        internal DoubleRange SideBySideInfoRangePad { get; set; }

        internal HashSet<int> Pixels { get; set; }

        internal IList<double>[] GroupedSeriesYValues { get; set; }

        internal bool IsActualTransposed
        {
            get
            {
                if (ActualArea != null && ActualArea is SfCartesianChart cartesianChart)
                    return cartesianChart.IsTransposed;

                return false;
            }
        }

        internal bool CanAnimate
        {
            get
            {
                return (canAnimate && EnableAnimation) || GetAnimationIsActive();
            }

            set
            {
                canAnimate = value;
            }
        }

        internal ChartDataMarkerPresenter AdornmentPresenter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the nearest segment index corresponding to the mouse point when interactive behaviors used.
        /// Note: This get's updated only when FindNearestChartPoint() method is called for series.
        /// </summary>
        internal int NearestSegmentIndex { get; set; }

        internal object ToolTipTag { get; set; }

        internal bool IsItemsSourceChanged { get; set; }

        internal TooltipPosition ActualTooltipPosition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to treat x values as categories. 
        /// </summary>
        internal bool IsIndexed
        {
            get { return this.ActualXAxis is CategoryAxis; }
        }

        /// <summary>
        /// Gets or sets the selected segments indexes.
        /// </summary>
        internal ObservableCollection<int> SelectedSegmentsIndexes
        {
            get 
            {
                return _selectedSegmentsIndexes;
            }

            set {
                if (SelectedSegmentsIndexes != null)
                    SelectedSegmentsIndexes.CollectionChanged -= SelectedSegmentsIndexes_CollectionChanged;
                _selectedSegmentsIndexes = value;
                if (_selectedSegmentsIndexes != null)
                    _selectedSegmentsIndexes.CollectionChanged += SelectedSegmentsIndexes_CollectionChanged;
            }
        }

        internal ChartAxis ActualXAxis 
        {
            get
            {
                return actualXAxis;
            }

            set 
            {
                actualXAxis = value;
            }
        }

        internal ChartAxis ActualYAxis
        {
            get
            {
                return actualYAxis;
            }

            set
            {
                actualYAxis = value;
            }
        }

        #endregion

        #region Internal Virtual Properties

        /// <summary>
        /// Gets whether this series is a bitmap series or not.
        /// </summary>
        /// <value>
        /// Returns the bool value.</value>
        internal virtual bool IsBitmapSeries
        {
            get
            {
                return false;
            }
        }

        internal virtual bool IsStacked 
        {
            get { return false; }
        }

        internal virtual bool IsSingleAccumulationSeries 
        {
            get { return false; }
        }

        internal virtual ChartDataLabelSettings AdornmentsInfo 
        {
            get 
            {
                return null;
            }
        }

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Suspends the series from updating the series data until ResumeNotification is called.
        /// </summary>
        /// <remarks>This is specifically used when we need to append a collection of data.</remarks>
        public void SuspendNotification()
        {
            isNotificationSuspended = true;
        }

        /// <summary>
        /// Processes the data that is added to the data source after SuspendNotification.
        /// </summary>
        public void ResumeNotification()
        {
            if (isNotificationSuspended && isPropertyNotificationSuspended) 
            {
                isPropertyNotificationSuspended = false;
                IEnumerable itemsSource = (ItemsSource as IEnumerable);
                int position = -1;
                foreach (object obj in itemsSource)
                {
                    position++;
                    SetIndividualPoint(position, obj, true);
                }

                this.ScheduleUpdateChart();
            }
            else if (isNotificationSuspended)
            {
                if (!isUpdateStarted || UpdateStartedIndex < 0) 
                {
                    ScheduleUpdateChart();
                    isNotificationSuspended = false;
                    return;
                }

                if (YPaths != null && ActualSeriesYValues != null && ItemsSource != null)
                {
                    GeneratePoints(YPaths, ActualSeriesYValues);
                    ScheduleUpdateChart();
                }

                isUpdateStarted = false;
                UpdateStartedIndex = -1;
            }

            isNotificationSuspended = false;
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Called when ItemsSource property changed.
        /// </summary>
        /// <param name="args"></param>
        internal void OnDataSourceChanged(DependencyPropertyChangedEventArgs args) 
        {
            canAnimate = true;
            
            if (args.OldValue != null) 
            {
                this.IsItemsSourceChanged = true;
                if (SelectionBehavior != null)
                    SelectionBehavior.SelectedIndex = -1;
                this.IsItemsSourceChanged = false;
            }

            if (ActualData != null)
                ActualData.Clear();

            if (ToggledLegendIndex != null)
                ToggledLegendIndex.Clear();

            if (ActualXValues != null) 
            {
                if (ActualXValues is IList<double>) 
                {
                    (XValues as IList<double>).Clear();
                    (ActualXValues as IList<double>).Clear();
                }
                else if (ActualXValues is IList<string>)
                {
                    (XValues as IList<string>).Clear();
                    (ActualXValues as IList<string>).Clear();
                }
            }

            HookAndUnhookCollectionChangedEvent(args.OldValue, args.NewValue);

            //todo: Need to check the use of below  code. 
            isTotalCalculated = false;
            if (Segments != null)
                Segments.Clear();

            ClearAdornments();
            this.PointsCount = 0;
            var newTable = args.NewValue as DataTable;
            var oldTable = args.OldValue as DataTable;
            if (oldTable != null)
            {
                oldTable.RowChanged -= DataTableRowChanged;
                oldTable.RowDeleting -= DataTableRowChanged;
                oldTable.TableCleared -= DataTableCleared;
                oldTable.TableClearing -= DataTableCleared;
            }

            if (newTable != null)
            {
                newTable.RowChanged += DataTableRowChanged;
                newTable.RowDeleting += DataTableRowChanged;
                newTable.TableCleared += DataTableCleared;
                newTable.TableClearing += DataTableCleared;

                OnDataSourceChanged(oldTable == null ? args.OldValue as IEnumerable : oldTable.Rows, newTable.Rows);
            }
            else
                OnDataSourceChanged(args.OldValue, args.NewValue);
        }

        /// <summary>
        /// Gets the available size of Chart.
        /// </summary>
        /// <returns>returns size</returns>
        internal Size GetAvailableSize() 
        {
            var availableSize = ActualArea == null ? new Size() : ActualArea.AvailableSize;
            return availableSize;
        }

        internal void UpdateBehavior(ChartBehavior oldBehavior, ChartBehavior newBehavior) 
        {
            if (ActualArea != null)
            {
                if (oldBehavior != null && ActualArea.Behaviors.Contains(oldBehavior)) 
                {
                    ActualArea.Behaviors.Remove(oldBehavior);
                }

                if (newBehavior != null && !ActualArea.Behaviors.Contains(newBehavior)) 
                {
                    newBehavior.Chart = ActualArea;
                    ActualArea.Behaviors.Add(newBehavior);
                }
            }
        }

        /// <summary>
        /// Invalidates the Series. 
        /// </summary>
        internal void Invalidate() 
        {
            CalculateSegments();
        }

        /// <summary>
        /// Returns the value of side by side position for a series.
        /// </summary>
        /// <param name="currentseries">ChartSeries.</param>
        /// <returns>The DoubleRange side by side Info</returns>
        internal DoubleRange GetSideBySideInfo(ChartSeries currentseries)
        {
            if (ActualArea != null) 
            {
                if (this.ActualArea.InternalPrimaryAxis == null || this.ActualArea.InternalSecondaryAxis == null)
                    return DoubleRange.Empty;

                if (!this.ActualArea.SBSInfoCalculated || !this.ActualArea.SeriesPosition.ContainsKey(currentseries))
                    CalculateSideBySidePositions(true);
                double width = 1 - GetSpacing(this);
                double minWidth = 0d;
                int all = 0;

                // MinPointsDelta is assigned to field since whenever the value is get the MinPointsDelta is calculated.
                double minPointsDelta = this.ActualArea.MinPointsDelta;

                if (!double.IsNaN(minPointsDelta)) 
                {
                    minWidth = minPointsDelta;
                }

                if (!this.ActualArea.SideBySideSeriesPlacement)
                {
                    CalculateSideBySideInfoPadding(minWidth, 1, 1, true);
                    return new DoubleRange((-width * minWidth) / 2, (width * minWidth) / 2);
                }

                int rowPos = currentseries.IsActualTransposed
                    ? ActualArea.GetActualRow(currentseries.ActualXAxis)
                    : ActualArea.GetActualRow(currentseries.ActualYAxis);
                int columnPos = currentseries.IsActualTransposed
                    ? ActualArea.GetActualColumn(currentseries.ActualYAxis)
                    : ActualArea.GetActualColumn(currentseries.ActualXAxis);

                var rowID = currentseries.ActualYAxis == null ? 0 : rowPos;
                var colID = currentseries.ActualXAxis == null ? 0 : columnPos;
                if ((rowID < this.ActualArea.SbsSeriesCount.GetLength(0)) && (colID < this.ActualArea.SbsSeriesCount.GetLength(1)))
                    all = this.ActualArea.SbsSeriesCount[rowID, colID];
                else
                    return DoubleRange.Empty;
                if (!this.ActualArea.SeriesPosition.ContainsKey(currentseries)) return DoubleRange.Empty;
                int pos = this.ActualArea.SeriesPosition[currentseries];
                if (all == 0) {
                    all = 1;
                    pos = 1;
                }

                double div = minWidth * width / all;
                double start = div * (pos - 1) - minWidth * width / 2;
                double end = start + div;

                // For adding additional space on both ends of side by side info series.
                CalculateSideBySideInfoPadding(minWidth, all, pos, true);

                return new DoubleRange(start, end);
            }
            else
            {
                return DoubleRange.Empty;
            }
        }

        internal void Refresh()
        {
            if (ActualData != null)
            {
                ActualData.Clear();
            }

            if (ActualXValues is IList<double>)
            {
                (XValues as IList<double>).Clear();
                (ActualXValues as IList<double>).Clear();
            }
            else if (ActualXValues is IList<string>)
            {
                (XValues as IList<string>).Clear();
                (ActualXValues as IList<string>).Clear();
            }

            if (ActualSeriesYValues != null && ActualSeriesYValues.Count() > 0)
            {
                foreach (IList<double> list in ActualSeriesYValues) 
                {
                    if (list != null)
                    {
                        list.Clear();
                    }
                }

                foreach (IList<double> list in SeriesYValues)
                {
                    if (list != null)
                    {
                        list.Clear();
                    }
                }
            }

            if (GetEnableSegmentSelection())
                SelectedSegmentsIndexes.Clear();
            else
                ActualArea.SelectedSeriesCollection.Clear();

            PointsCount = 0;
            if (XBindingPath != null && YPaths != null && YPaths.Count() > 0)
            {
                GenerateDataPoints();
                Segments.Clear();
                ClearAdornments();
                ScheduleUpdateChart();
            }
        }

        /// <summary>
        /// Method implementation for UpdateArea.
        /// </summary>
        internal void ScheduleUpdateChart()
        {
            if (ActualArea != null) 
            {
                ActualArea.ScheduleUpdate();
            }
        }

        /// <summary>
        /// Return collection of double values
        /// </summary>
        /// <returns></returns>
        internal List<double> GetXValues()
        {
            double xIndexValues = 0d;
            List<double> xValues = ActualXValues as List<double>;
            if (IsIndexed || xValues == null)
            {
                xValues = xValues != null ? (from val in (xValues) select (xIndexValues++)).ToList()
                      : (from val in (ActualXValues as List<string>) select (xIndexValues++)).ToList();
            }

            return xValues;
        }

        internal List<double> GetYValues() 
        {
            var yvalues = new List<double>();
            var triangularSeries = this as TriangularSeriesBase;
            var circularSeries = this as CircularSeries;
            for (int i = 0; i < PointsCount; i++) 
            {
                if (!ToggledLegendIndex.Contains(i))
                {
                    if (triangularSeries != null)
                        yvalues.Add(triangularSeries.YValues[i]);
                    else if (circularSeries != null)
                        yvalues.Add(circularSeries.YValues[i]);
                }
                else
                    yvalues.Add(double.NaN);
            }

            return yvalues;
        }

        internal bool GetEnableSegmentSelection()
        {
            if (SelectionBehavior != null &&
                (SelectionBehavior.Type == ChartSelectionType.Single ||
                SelectionBehavior.Type == ChartSelectionType.Multiple ||
                SelectionBehavior.Type == ChartSelectionType.SingleDeselect))
                return true;

            return false;
        }

        /// <summary>
        /// Called when selection changed.
        /// </summary>
        /// <param name="eventArgs">ChartSelectionChangedEventArgs.</param>
        internal void OnSelectionChanged(int newIndex, int oldIndex)
        {
            var selectionBehavior = SelectionBehavior;
            if (selectionBehavior != null) {
                var newIndexes = new List<int>() { newIndex };
                var oldIndexes = new List<int>() { oldIndex };
                selectionBehavior.OnSelectionChanged(newIndexes, oldIndexes, this);
            }
        }

        /// <summary>
        /// Method implementation for clear unused adornments.
        /// </summary>
        /// <param name="startIndex"></param>
        internal void ClearUnUsedAdornments(int startIndex)
        {
            if (Adornments.Count > startIndex) 
            {
                int count = Adornments.Count;

                for (int i = startIndex; i < count; i++)
                {
                    Adornments.RemoveAt(startIndex);
                }
            }
        }

        /// <summary>
        /// Method is used to raise SelectionChanging event
        /// </summary>
        /// <param name="newIndex">Used to indicate current selected index</param>
        /// <param name="oldIndex">Used to indicate previous selected index</param>   
        internal virtual bool RaiseSelectionChanging(int newIndex, int oldIndex)
        {
            if (SelectionBehavior != null)
            {
                var newIndexes = new List<int>() { newIndex };
                var oldIndexes = new List<int>() { oldIndex };
                return SelectionBehavior.OnSelectionChanging(newIndexes, oldIndexes, this);
            }

            return false;
        }

        internal object GetSegment(object item)
        {
            return Segments.Where(segment => segment.Item == item).FirstOrDefault();
        }

        /// <summary>
        /// Add and Update the series Tooltip
        /// </summary>
        /// <param name="customTag"></param>
        internal void UpdateSeriesTooltip(object customTag)
        {
            if (customTag == null) return;
            ToolTipTag = customTag;
            var chartTooltip = this.ActualArea.Tooltip as ChartTooltip;
            chartTooltip.PolygonPath = " ";
            ChartTooltipBehavior tooltipBehavior = ActualArea.TooltipBehavior;

            SetTooltipDuration();
            var canvas = ActualArea.GetAdorningCanvas();

            if (chartTooltip != null && canvas != null)
            {
                if (!IsTooltipAvailable(canvas)) 
                {
                    chartTooltip.DataContext = customTag as ChartSegment;

                    if (chartTooltip.DataContext == null)
                        return;

                    if (ChartTooltip.GetActualInitialShowDelay(tooltipBehavior, ChartTooltip.GetInitialShowDelay(this)) == 0)
                    {
                        HastoolTip = true;
                        canvas.Children.Add(chartTooltip);
                    }

                    if (TooltipTemplate != null)
                   {
                        chartTooltip.ContentTemplate = this.TooltipTemplate;
                    }
                    else
                    {
                        chartTooltip.ContentTemplate = this.GetTooltipTemplate();
                    }

                    if (chartTooltip.ContentTemplate == null)
                    {
                        if (toolTipTemplate == null)
                        {
                            toolTipTemplate = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipTemplate"] as DataTemplate;
                        }

                        chartTooltip.ContentTemplate = toolTipTemplate;
                    }

                    AddTooltip();

                    if (ChartTooltip.GetActualEnableAnimation(tooltipBehavior, ChartTooltip.GetEnableAnimation(this)) && ActualArea is ChartBase)
                    {
                        SetDoubleAnimation(chartTooltip);
                    }

                    Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                    Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                }
                else
                {
                    foreach (var child in canvas.Children)
                    {
                        var tooltip = child as ChartTooltip;
                        if (tooltip != null)
                            chartTooltip = tooltip;
                    }

                    chartTooltip.DataContext = customTag;
                    if (chartTooltip.DataContext == null)
                    {
                        RemoveTooltip();
                        return;
                    }

                    AddTooltip();
                    Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                    Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                }
            }
        }

        internal void UpdateLegendIconTemplate(bool iconChanged) 
        {
            try
            {
                string legendIcon = LegendIcon.ToString();

                if (LegendIcon == ChartLegendIcon.SeriesType)
                {
                    FastScatterBitmapSeries fastScatterBitmapSeries = this as FastScatterBitmapSeries;

                    if (fastScatterBitmapSeries != null)
                    {
                        legendIcon = fastScatterBitmapSeries.Type == ShapeType.Circle ? "Circle" : fastScatterBitmapSeries.Type.ToString();
                    }
                    else if (this is FastStepLineBitmapSeries)
                    {
                        legendIcon = "StepLine";
                    }
                    else if (this is PolarAreaSeries || this is PolarLineSeries)
                    {
                        legendIcon = "Polar";
                    }
                    else
                    {
                        legendIcon = this.GetType().Name.Replace("Series", "");
                        legendIcon = legendIcon.Replace("3D", "");
                    }
                }

                if ((GetIconTemplate() == null || iconChanged)
                && ChartDictionaries.GenericLegendDictionary.Keys.Contains(legendIcon)) {
                    LegendIconTemplate = ChartDictionaries.GenericLegendDictionary[legendIcon] as DataTemplate;
                }
            }
            catch
            {
            }
        }

        internal Brush GetInteriorColor(int segmentIndex)
        {
            int serIndex = ActualArea.GetSeriesIndex(this);
            if (Fill != null)
                return Fill;

            if (PaletteBrushes != null)
                return PaletteBrushes[segmentIndex % PaletteBrushes.Count()];
            if (!ActualArea.IsNullPaletteBrushes())
            {
                if (serIndex >= 0)
                    return ActualArea.GetPaletteBrush(serIndex);
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        internal void CalculateSideBySideInfoPadding(double minWidth, int all, int pos, bool isXAxis)
        {
            var axis = this.ActualXAxis;
            bool isAlterRange = ((axis is NumericalAxis && (axis as NumericalAxis).RangePadding == NumericalPadding.None)
                    || (axis is DateTimeAxis && (axis as DateTimeAxis).RangePadding == DateTimeRangePadding.None));
            double space = isAlterRange ? 1 - GetSpacing(this) : GetSpacing(this);
            double div = minWidth * space / all;
            double padStart = div * (pos - 1) - minWidth * space / 2;
            double padEnd = padStart + div;

            if (isXAxis)
                SideBySideInfoRangePad = new DoubleRange(padStart, padEnd);
        }

        /// <summary>
        /// calculates the side-by-side position for all applicable series.
        /// </summary>
        internal void CalculateSideBySidePositions(bool isXAxis)
        {
            ActualArea.SeriesPosition.Clear();
            int all = -1;
            int rowCount = this.ActualArea.RowDefinitions.Count;
            int columnCount = this.ActualArea.ColumnDefinitions.Count;
            this.ActualArea.SbsSeriesCount = new int[rowCount, columnCount];

            if (rowCount == 0) 
            {
                ActualArea.SbsSeriesCount = new int[1, columnCount];
                rowCount = 1;
            }

            if (columnCount == 0) 
            {
                ActualArea.SbsSeriesCount = new int[rowCount, 1];
                columnCount = 1;
            }

            var seriesCollection = from series in ActualXAxis.RegisteredSeries select series; // WRT-2246-Side by side info Series isn't properly arranged while its with multiple axis

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    all = 0;
                    var filteredSeries = (from series in seriesCollection
                                          let rowPos =
                                              series.IsActualTransposed
                                                  ? ActualArea.GetActualRow(series.ActualXAxis) : ActualArea.GetActualRow(series.ActualYAxis)
                                          let columnPos =
                                              series.IsActualTransposed
                                                  ? ActualArea.GetActualColumn(series.ActualYAxis)
                                                  : ActualArea.GetActualColumn(series.ActualXAxis)
                                          where columnPos == j && rowPos == i
                                          select series).ToList();

                    int currStack = 0;
                    var stackedColumns = new List<ChartSeries>();
                    foreach (ChartSeries item in filteredSeries)
                    {
                        bool stacked = false;
                        if (item.IsSideBySide && item.IsSeriesVisible)
                        {
                            if (item.IsStacked)
                            {
                                if (stackedColumns.Count == 0)
                                {
                                    all++;
                                    ActualArea.SeriesPosition.Add(item, all);
                                    stackedColumns.Add(item);
                                    continue;
                                }

                                foreach (var stackedColumn in stackedColumns.Where(stackedColumn => (stackedColumn.ActualYAxis == item.ActualYAxis) && ((stackedColumn as StackedSeriesBase).GroupName == (item as StackedSeriesBase).GroupName))) {
                                    stacked = true;
                                    currStack = ActualArea.SeriesPosition[stackedColumn];
                                }

                                stackedColumns.Add(item);

                                if (stacked)
                                {
                                    ActualArea.SeriesPosition.Add(item, currStack);
                                }
                                else
                                {
                                    ActualArea.SeriesPosition.Add(item, ++all);
                                }
                            }
                            else
                            {
                                all++;
                                this.ActualArea.SeriesPosition.Add(item, all);
                            }
                        }
                    }

                    this.ActualArea.SbsSeriesCount[i, j] = all;
                }
            }

            this.ActualArea.SBSInfoCalculated = true;
        }

        internal object GetComplexArrayPropertyValue(object parentObj, string path)
        {
            if (path.Contains('[')) 
            {
                int index = Convert.ToInt32(path.Substring(path.IndexOf('[') + 1, path.IndexOf(']') - path.IndexOf('[') - 1));
                string actualPath = path.Replace(path.Substring(path.IndexOf('[')), string.Empty);
                parentObj = ReflectedObject(parentObj, actualPath);

                if (parentObj == null) return null;

                IList array = parentObj as IList;
                if (array != null && array.Count > index)
                    parentObj = array[index];
                else
                    return null;
            }
            else 
            {
                parentObj = ReflectedObject(parentObj, path);

                if (parentObj == null) return null;

                if (parentObj.GetType().IsArray) return null;
            }

            return parentObj;
        }

        internal void HookPropertyChangedEvent(bool value)
        {
            if (ItemsSource == null || ItemsSource is DataTable) return;
            IEnumerator enumerator = (ItemsSource as IEnumerable).GetEnumerator();
            if (!enumerator.MoveNext()) return;
            INotifyPropertyChanged collection = enumerator.Current as INotifyPropertyChanged;
            if (collection != null)
            {
                if (value)
               {
                    do
                    {
                        if (isComplexYProperty || XBindingPath.Contains('.'))
                        {
                            HookComplexProperty(enumerator.Current, XComplexPaths);
                            for (int i = 0; i < YComplexPaths.Count(); i++)
                                HookComplexProperty(enumerator.Current, YComplexPaths[i]);
                        }

                        (enumerator.Current as INotifyPropertyChanged).PropertyChanged -= OnItemPropertyChanged;
                        (enumerator.Current as INotifyPropertyChanged).PropertyChanged += OnItemPropertyChanged;
                    }
                    while (enumerator.MoveNext());
                }
                else
                {
                    do
                        (enumerator.Current as INotifyPropertyChanged).PropertyChanged -= OnItemPropertyChanged;
                    while (enumerator.MoveNext());
                }
            }
        }

        /// <summary>
        /// Get the Default Template for Tooltip
        /// </summary>
        /// <returns></returns>
        internal DataTemplate GetTooltipTemplate()
        {
            if (TooltipTemplate != null) return TooltipTemplate;

            return GetDefaultTooltipTemplate();
        }

        /// <summary>
        /// Method to hook the PropertyChange event for individual data point
        /// </summary>
        /// <param name="needPropertyChange"></param>
        /// <param name="obj"></param>
        internal void HookPropertyChangedEvent(bool needPropertyChange, object obj) 
        {
            if (needPropertyChange) 
            {
                INotifyPropertyChanged model = obj as INotifyPropertyChanged;

                if (model != null) {
                    model.PropertyChanged -= OnItemPropertyChanged;
                    model.PropertyChanged += OnItemPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Method used to calculate the rect on mouse point to get hittest data point. 
        /// </summary>
        /// <param name="mousePos"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="rect"></param>
        internal void CalculateHittestRect(Point mousePos, out int startIndex, out int endIndex, out Rect rect)
        {
            Point hitPoint = new Point();
            Point startPoint = new Point();
            Point endPoint = new Point();
            double startXVal, endXVal;
            IList<double> xValues = (ActualXValues is IList<double>) ? ActualXValues as IList<double> : GetXValues();

            hitPoint.X = mousePos.X - ActualArea.SeriesClipRect.Left;
            hitPoint.Y = mousePos.Y - ActualArea.SeriesClipRect.Top;

            // Calculate the bounds region from the area width  & height
            double x = Math.Floor(ActualArea.SeriesClipRect.Width * 0.025);
            double y = Math.Floor(ActualArea.SeriesClipRect.Height * 0.025);

            double yVal = 0;
            double stackedYValue = double.NaN;

            hitPoint.X = hitPoint.X - x;
            hitPoint.Y = hitPoint.Y - y;

            // Get the nearest XData from rectangle region starting x value. 
            this.FindNearestChartPoint(hitPoint, out startXVal, out yVal, out stackedYValue);

            // Get the accurate XData from rectangle region starting x value.
            startPoint.X = ActualArea.ActualPointToValue(ActualXAxis, hitPoint);
            startPoint.Y = ActualArea.ActualPointToValue(ActualYAxis, hitPoint);

            hitPoint.X = hitPoint.X + (2 * x);
            hitPoint.Y = hitPoint.Y + (2 * y);

            // Get the nearest XData from rectangle region ending x value. 
            this.FindNearestChartPoint(hitPoint, out endXVal, out yVal, out stackedYValue);

            // Get the accurate XData from rectangle region ending x value. 
            endPoint.X = ActualArea.ActualPointToValue(ActualXAxis, hitPoint);
            endPoint.Y = ActualArea.ActualPointToValue(ActualYAxis, hitPoint);

            rect = new Rect(startPoint, endPoint);

            dataPoint = null;

            if (this.IsIndexed || !(this.ActualXValues is IList<double>)) 
            {
                // Check the start or end value out of the area. 
                startXVal = double.IsNaN(startXVal) ? 0 : startXVal;
                endXVal = double.IsNaN(endXVal) ? xValues.Count - 1 : endXVal;

                startIndex = Convert.ToInt32(startXVal);
                endIndex = Convert.ToInt32(endXVal);
            }
            else
            {
                // Check the start or end value out of the area. 
                startXVal = double.IsNaN(startXVal) ? xValues[0] : startXVal;
                endXVal = double.IsNaN(endXVal) ? xValues[xValues.Count - 1] : endXVal;

                startIndex = xValues.IndexOf(startXVal);
                endIndex = xValues.IndexOf(endXVal);
            }

            if (startIndex == -1)
                startIndex = 0;
            if (endIndex == -1)
                endIndex = 0;
        }

        internal double GetGrandTotal(IList<double> yValues)
        {
            if (!isTotalCalculated) 
            {
                grandTotal = (from val in yValues where !double.IsNaN(val) select val).Sum();
                isTotalCalculated = true;
            }

            return grandTotal;
        }

        internal DataTemplate GetTrackballTemplate()
        {
            return this.TrackballLabelTemplate;
        }

        /// <summary>
        /// Method used to select the adornment in given data point
        /// </summary>
        /// <param name="index"></param>
        internal void UpdateAdornmentSelection(int index)
        {
            if (index >= 0 && index <= ActualData.Count - 1) 
            {
                List<int> indexes = new List<int>();
                var circularSeries = this as CircularSeries;
                if (circularSeries != null && !double.IsNaN(circularSeries.GroupTo))
                    indexes = (from adorment in Adornments
                               where Segments[index].Item == adorment.Item
                               select Adornments.IndexOf(adorment)).ToList();
                else if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed
                      && this.IsSideBySide)
                    indexes = (from adorment in Adornments
                               where GroupedActualData[index] == adorment.Item
                               select Adornments.IndexOf(adorment)).ToList();
                else
                    indexes = (from adorment in Adornments
                               where ActualData[index] == adorment.Item
                               select Adornments.IndexOf(adorment)).ToList();

                if (AdornmentPresenter != null)
                    AdornmentPresenter.UpdateAdornmentSelection(indexes, true);
            }
        }

        internal void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        internal DataTemplate GetIconTemplate() 
        {
            return this.LegendIconTemplate;
        }

        /// <summary>
        /// Method is used to select/reset the bitmap segment.
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="brush"></param>
        /// <param name="isSelected"></param>
        internal void OnBitmapSelection(List<int> pixels, Brush brush, bool isSelected)
        {
            if (pixels != null && pixels.Count > 0)
            {
                var seriesCollection = Chart.GetChartSeriesCollection();
                int seriesIndex = seriesCollection.IndexOf(this);

                if (!Chart.isBitmapPixelsConverted)
                    Chart.ConvertBitmapPixels();

                // Gets the upper series from the selected series
                var upperSeriesCollection = (from series in seriesCollection
                                             where seriesCollection.IndexOf(series) > seriesIndex
                                             select series).ToList();

                // Gets the upper series pixels in to single collection
                foreach (var series in upperSeriesCollection)
                {
                    upperSeriesPixels.UnionWith(series.Pixels);
                }

                {
                    byte[] buffer = Chart.GetFastBuffer();
                    int j = 0;
                    Color uiColor;

                    if (isSelected && brush != null)
                        uiColor = (brush as SolidColorBrush).Color;
                    else
                    {
                        {
                            Brush interior = GetInteriorColor(dataPoint.Index);
                            var linearGradienBrush = interior as LinearGradientBrush;
                            uiColor = linearGradienBrush != null ? linearGradienBrush.GradientStops[0].Color : (interior as SolidColorBrush).Color;
                        }
                    }

                    foreach (var pixel in pixels) 
                    {
                        if (Pixels.Contains(pixel) && !upperSeriesPixels.Contains(pixel))
                        {
                            if (j == 0) 
                            {
                                buffer[pixel] = uiColor.B;
                                j = j + 1;
                            }
                            else if (j == 1)
                            {
                                buffer[pixel] = uiColor.G;
                                j = j + 1;
                            }
                            else if (j == 2)
                            {
                                buffer[pixel] = uiColor.R;
                                j = j + 1;
                            }
                            else if (j == 3)
                            {
                                buffer[pixel] = uiColor.A;
                                j = 0;
                            }
                        }
                    }

                    Chart.RenderToBuffer();
                }

                upperSeriesPixels.Clear();
            }
        }

        internal void UpdateAssociatedAxes() 
        {
            if (ActualXAxis != null && ActualYAxis != null) 
            {
                if (!ActualXAxis.AssociatedAxes.Contains(ActualYAxis))
                {
                    ActualXAxis.AssociatedAxes.Add(ActualYAxis);
                }

                if (!ActualYAxis.AssociatedAxes.Contains(ActualXAxis)) 
                {
                    ActualYAxis.AssociatedAxes.Add(ActualXAxis);
                }
            }
        }

        internal void SeriesPointerMoved(PointerRoutedEventArgs e)
        {
            var canvas = this.ActualArea.GetAdorningCanvas();
            mousePosition = e.GetCurrentPoint(canvas).Position;
            if (e.Pointer.PointerDeviceType != PointerDeviceType.Touch && mousePosition != previousMousePosition) 
            {
                RemovePreviousSeriesTooltip();
                if (!GetAnimationIsActive())
                {
                    UpdateTooltip(e.OriginalSource);
                }
            }
        }

        internal void RemovePreviousSeriesTooltip()
        {
            if (ActualArea.Tooltip != null && ActualArea.Tooltip.PreviousSeries != null)
            {
                if (ActualTooltipPosition == TooltipPosition.Auto && !this.Equals(ActualArea.Tooltip.PreviousSeries)) 
                {
                    RemoveTooltip();
                    Timer.Stop();
                }
            }
        }

        /// <summary>
        /// Calculate the position and orientation of tooltip nose.
        /// </summary>
        /// <param name="size">DesiredSize of ChartTooltip</param>
        /// <param name="position">Location of tooltip whether based on mouse or data point</param>
        /// <param name="horizontal">Orientation of tooltip nose</param>
        /// <param name="vertical">Orientation of tooltip nose</param>
        /// <returns>returns the tooltip aligned point.</returns>
        internal Point AlignTooltip(Size size, Point position, out HorizontalPosition horizontal, out VerticalPosition vertical) 
        {
            var chartTooltip = ActualArea.Tooltip as ChartTooltip;
            double expectedWidth = size.Width + position.X;
            double expectedHeight = size.Height + position.Y;
            Point offset = new Point();
            Rect clipRect = this.ActualArea.SeriesClipRect;
            horizontal = HorizontalPosition.Center;
            vertical = VerticalPosition.Bottom;

            if (ActualArea.AreaType != ChartAreaType.None)
            {
                if (ActualArea is ChartBase) 
                {
                    if (position.Y < clipRect.Top) 
                    {
                        offset.Y = position.Y + size.Height;
                        offset.Y += 4;
                        vertical = VerticalPosition.Top;
                    }
                    else if (expectedHeight > clipRect.Bottom) 
                    {
                        offset.Y = position.Y + (clipRect.Bottom - expectedHeight);
                        offset.Y -= 4;
                        vertical = VerticalPosition.Bottom;
                    }
                    else 
                    {
                        offset.Y = position.Y;
                        offset.Y -= 4;
                        vertical = VerticalPosition.Bottom;
                    }

                    if (position.X < clipRect.Left)
                    {
                        offset.X = position.X + size.Width / 2;
                        offset.X += 4;
                        horizontal = HorizontalPosition.Left;
                    }
                    else if (expectedWidth > clipRect.Right)
                    {
                        offset.X = position.X - size.Width / 2;
                        offset.X -= 4;
                        horizontal = HorizontalPosition.Right;
                    }
                    else
                    {
                        offset.X = position.X;
                        horizontal = HorizontalPosition.Center;
                    }

                    vertical = GetVerticalPosition(vertical);
                    //change the tooltip left and top offset by 2px
                    chartTooltip.LeftOffset = horizontal == HorizontalPosition.Left ? offset.X - 2 : horizontal == HorizontalPosition.Right ? offset.X + 2 : offset.X;
                    chartTooltip.TopOffset = vertical == VerticalPosition.Bottom ? offset.Y - 2 : vertical == VerticalPosition.Top ? offset.Y + 2 : offset.Y;

                }

                //After changing the position with offset, again calculate whether it has within range
                AdjustTooltipAtEdge(chartTooltip);
            }
            else
            {
                offset = position;
                if (ActualArea is ChartBase) 
                {
                    chartTooltip.LeftOffset = offset.X;
                    chartTooltip.TopOffset = offset.Y;
                }

                if (chartTooltip.LeftOffset < 0)
                    chartTooltip.LeftOffset = 0;
                else if ((chartTooltip.LeftOffset + chartTooltip.DesiredSize.Width) > (ActualArea as ChartBase).RootPanelDesiredSize.Value.Width)
                    chartTooltip.LeftOffset = (ActualArea as ChartBase).RootPanelDesiredSize.Value.Width - chartTooltip.DesiredSize.Width;
                else
                    chartTooltip.LeftOffset = chartTooltip.LeftOffset;
                if (chartTooltip.TopOffset < 0)
                    chartTooltip.TopOffset = 0;
                else if (chartTooltip.TopOffset + chartTooltip.DesiredSize.Height > (ActualArea as ChartBase).RootPanelDesiredSize.Value.Height)
                    chartTooltip.TopOffset = (ActualArea as ChartBase).RootPanelDesiredSize.Value.Height - chartTooltip.DesiredSize.Height;
                else
                    chartTooltip.TopOffset = chartTooltip.TopOffset;
            }

            return new Point(offset.X, offset.Y);
        }

        #endregion

        #region Internal Virtual Methods

        internal virtual ChartDataLabel CreateAdornment(ChartSeries series, double xVal, double yVal, double xPos, double yPos)
        {
            return CreateDataMarker(series, xVal, yVal, xPos, yPos);
        }

        /// <summary>
        /// Method implementation for create DataMarkers.
        /// </summary>
        /// <param name="series">series</param>
        /// <param name="xVal">xvalue</param>
        /// <param name="yVal">yvalue</param>
        /// <param name="xPos">xposition</param>
        /// <param name="yPos">yposition</param>
        /// <returns>ChartAdornment</returns>
        internal virtual ChartDataLabel CreateDataMarker(ChartSeries series, double xVal, double yVal, double xPos, double yPos) 
        {
            ChartDataLabel adornment = new ChartDataLabel(xVal, yVal, xPos, yPos, series);
            adornment.XData = xVal;
            adornment.YData = yVal;
            adornment.XPos = xPos;
            adornment.YPos = yPos;
            adornment.Series = series;
            return adornment;
        }


        /// <summary>
        /// Finds the nearest point in ChartSeries relative to the mouse point/touch position.
        /// </summary>
        /// <param name="point">The co-ordinate point representing the current mouse point /touch position.</param>
        /// <param name="x">x-value of the nearest point.</param>
        /// <param name="y">y-value of the nearest point</param>
        /// <param name="stackedYValue"></param>
        internal virtual void FindNearestChartPoint(Point point, out double x, out double y, out double stackedYValue) 
        {
            ChartPoint chartPoint;
            x = double.NaN;
            y = double.NaN;
            stackedYValue = double.NaN;
            if (this.IsIndexed || !(this.ActualXValues is IList<double>)) 
            {
                if (ActualArea != null)
                {
                    double xStart = ActualXAxis.VisibleRange.Start;
                    double xEnd = ActualXAxis.VisibleRange.End;
                    chartPoint = new ChartPoint(ActualArea.ActualPointToValue(ActualXAxis, point), ActualArea.ActualPointToValue(ActualYAxis, point));
                    double range = Math.Round(chartPoint.X);
                    if (ActualSeriesYValues.Count() > 0)
                    {
                        if (!(this is StackedColumn100Series) && ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed
                            ) 
                        {
                            var count = GroupedXValues.Count;
                            var range1 = DistinctValuesIndexes[range];
                            int index = (int)range;
                            if (range1.Count > 0)
                                index = range1[0];
                            else 
                            {
                                string xvalue = GroupedXValues[(int)range];

                                foreach (var series in ActualArea.VisibleSeries)
                                {
                                    index = (series.ActualXValues as List<string>).IndexOf(xvalue);

                                    if (index > -1)
                                        break;
                                }
                            }
                            if (range <= xEnd && range >= xStart && range < count && range >= 0)
                            {
                                y = GroupedSeriesYValues[0][(int)index];
                                x = range;
                                NearestSegmentIndex = (int)x;
                            }
                        }
                        else
                        {
                            var count = ActualSeriesYValues[0].Count;
                            if (range <= xEnd && range >= xStart && range < count && range >= 0)
                            {
                                y = ActualSeriesYValues[0][(int)range];
                                x = range;
                                NearestSegmentIndex = (int)x;
                            }
                        }
                    }
                }
            }
            else
            {
                ChartPoint nearPoint = new ChartPoint();
                List<double> xValues = this.ActualXValues as List<double>;
                var yValues = this.ActualSeriesYValues[0];
                nearPoint.X = ActualXAxis.VisibleRange.Start;

                if (IsSideBySide) 
                {
                    DoubleRange sbsInfo = this.GetSideBySideInfo(this);
                    nearPoint.X = ActualXAxis.VisibleRange.Start + sbsInfo.Start;
                }

                nearPoint.Y = ActualYAxis.VisibleRange.Start;
                chartPoint = new ChartPoint(ActualArea.ActualPointToValue(ActualXAxis, point), ActualArea.ActualPointToValue(ActualYAxis, point));
                int index = 0;
                double xValue = chartPoint.X;

                if (IsLinearData)
                {
                    int dataIndex = ChartExtensionUtils.BinarySearch(xValues, xValue, 0,
                        xValues.Count - 1);
                    x = xValues[dataIndex];
                    y = yValues[dataIndex];
                    NearestSegmentIndex = dataIndex;
                }
                else 
                {
                    foreach (var x1 in xValues) 
                    {
                        double y1 = yValues[index];

                        if (xValue > ActualXAxis.VisibleRange.Start && xValue < ActualXAxis.VisibleRange.End)
                        {
                            if (Math.Abs(chartPoint.X - x1) < Math.Abs(chartPoint.X - nearPoint.X))
                            {
                                nearPoint = new ChartPoint(x1, y1);
                                x = x1;
                                y = y1;
                                NearestSegmentIndex = index;
                            }
                            else if (Math.Abs(chartPoint.X - x1) == Math.Abs(chartPoint.X - nearPoint.X))
                            {
                                if (Math.Abs(chartPoint.Y - y1) < Math.Abs(chartPoint.Y - nearPoint.Y)) 
                                {
                                    nearPoint = new ChartPoint(x1, y1);
                                    x = x1;
                                    y = y1;
                                    NearestSegmentIndex = index;
                                }
                            }
                        }

                        index++;
                    }
                }
            }
        }

        /// <summary>
        /// An abstract method which will called over each time in its child class to update an segment.
        /// </summary>
        /// <param name="index">The index of the segment</param>
        /// <param name="action">The collection changed action which raises the notification</param>
        internal virtual void UpdateSegments(int index, NotifyCollectionChangedAction action)
        {
            ScheduleUpdateChart();
        }

        /// <summary>
        /// Set ToolTip duration.
        /// </summary>
        internal virtual void SetTooltipDuration()
        {
            int initialShowDelay = ChartTooltip.GetActualInitialShowDelay(ActualArea.TooltipBehavior, ChartTooltip.GetInitialShowDelay(this));
            int duration = ChartTooltip.GetActualDuration(ActualArea.TooltipBehavior, ChartTooltip.GetDuration(this));

            if (initialShowDelay > 0)
            {
                Timer.Stop();
                if (!InitialDelayTimer.IsEnabled) 
                {
                    InitialDelayTimer.Interval = new TimeSpan(0, 0, 0, 0, initialShowDelay);
                    InitialDelayTimer.Start();
                }
            }
            else
            {
                Timer.Interval = new TimeSpan(0, 0, 0, 0, duration);
                Timer.Start();
            }
        }

        /// <summary>
        /// Returns the tooltip is available or not in this series.
        /// </summary>
        /// <param name="canvas">canvas</param>
        /// <returns></returns>
        internal virtual bool IsTooltipAvailable(Canvas canvas) 
        {
            foreach (var item in canvas.Children) 
            {
                if (item is ChartTooltip)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Set animation for tooltip.
        /// </summary>
        /// <param name="chartTooltip">ChartTooltip</param>
        internal virtual void SetDoubleAnimation(ChartTooltip chartTooltip) 
        {
            int count = VisualTreeHelper.GetChildrenCount(chartTooltip);
            if (count == 0) 
            {
                return;
            }

            Grid grid = VisualTreeHelper.GetChild(chartTooltip, 0) as Grid;

            if (grid == null) return;

            storyBoard = new Storyboard();
            scaleXAnimation = new DoubleAnimation 
            {
                From = 0.5,
                To = 1.0,
                Duration = new Duration().GetDuration(new TimeSpan(0, 0, 0, 0, 200)),
                EasingFunction = new SineEase() { EasingMode = EasingMode.EaseOut }
            };

            Storyboard.SetTarget(scaleXAnimation, grid);
            Storyboard.SetTargetProperty(scaleXAnimation, "(UIElement.RenderTransform).(ScaleTransform.ScaleX)");

            scaleYAnimation = new DoubleAnimation 
            {
                From = 0.5,
                To = 1.0,
                Duration = new Duration().GetDuration(new TimeSpan(0, 0, 0, 0, 200)),
                EasingFunction = new SineEase() { EasingMode = EasingMode.EaseOut }
            };

            Storyboard.SetTarget(scaleYAnimation, grid);
            Storyboard.SetTargetProperty(scaleYAnimation, "(UIElement.RenderTransform).(ScaleTransform.ScaleY)");

            storyBoard.Children.Add(scaleXAnimation);
            storyBoard.Children.Add(scaleYAnimation);
            storyBoard.Begin();
        }

        /// <summary>
        /// Method implementation for Set points to given index for data table.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="obj">object</param>
        /// <param name="replace">replace the data point or not</param>
        internal virtual void SetIndividualDataTablePoint(int index, object obj, bool replace) 
        {
            if (SeriesYValues != null && YPaths != null && ItemsSource != null) 
            {
                var dataRow = obj as DataRow;
                if (IsMultipleYPathRequired) 
                {
                    if (XValueType == ChartValueType.String) 
                    {
                        if (!(this.XValues is List<string>))
                            this.XValues = this.ActualXValues = new List<string>();
                        IList<string> xValue = this.XValues as List<string>;
                        string xVal = dataRow.GetField(XBindingPath) as string;
                        if (replace && xValue.Count > index) 
                        {
                            xValue[index] = xVal;
                        }
                        else if (xValue.Count == index) 
                        {
                            xValue.Add(xVal);
                        }
                        else 
                        {
                            xValue.Insert(index, xVal);
                        }

                        for (int i = 0; i < YPaths.Count(); i++)
                        {
                            object yVal = dataRow.GetField(YComplexPaths[i][0]);
                            if (!(SeriesYValues[i] is List<double>))
                            {
                                SeriesYValues[i] = new List<double>();
                            }

                            if (replace && SeriesYValues[i].Count > index) 
                            {
                                SeriesYValues[i][index] = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            }
                            else if (SeriesYValues[i].Count == index)
                            {
                                SeriesYValues[i].Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                            }
                            else 
                            {
                                SeriesYValues[i].Insert(index, Convert.ToDouble(yVal != null ? yVal : double.NaN));
                            }
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.Double ||
                       XValueType == ChartValueType.Logarithmic) 
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = dataRow.GetField(XBindingPath);

                        XData = Convert.ToDouble(xVal != null ? xVal : double.NaN);

                        // Check the Data Collection is linear or not
                        if (IsLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            IsLinearData = false;
                        }

                        if (replace && xValue.Count > index) 
                        {
                            xValue[index] = XData;
                        }
                        else if (xValue.Count == index) 
                        {
                            xValue.Add(XData);
                        }
                        else 
                        {
                            xValue.Insert(index, XData);
                        }

                        for (int i = 0; i < YPaths.Count(); i++)
                        {
                            object yVal = dataRow.GetField(YComplexPaths[i][0]);

                            if (!(SeriesYValues[i] is List<double>))
                            {
                                SeriesYValues[i] = new List<double>();
                            }

                            if (replace && SeriesYValues[i].Count > index)
                            {
                                SeriesYValues[i][index] = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            }
                            else if (SeriesYValues[i].Count == index)
                            {
                                SeriesYValues[i].Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                            }
                            else
                            {
                                SeriesYValues[i].Insert(index, Convert.ToDouble(yVal != null ? yVal : double.NaN));
                            }
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.DateTime)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = dataRow.GetField(XBindingPath);
                        XData = Convert.ToDateTime(xVal).ToOADate();

                        if (!(SeriesYValues[0] is List<double>))
                        {
                            SeriesYValues[0] = new List<double>();
                        }

                        // Check the Data Collection is linear or not
                        if (IsLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            IsLinearData = false;
                        }

                        if (replace && xValue.Count > index) 
                        {
                            xValue[index] = XData;
                        }
                        else if (xValue.Count == index) 
                        {
                            xValue.Add(XData);
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        for (int i = 0; i < YPaths.Count(); i++) 
                        {
                            object yVal = dataRow.GetField(YComplexPaths[i][0]);
                            if (index == 0) 
                            {
                                SeriesYValues[i] = new List<double>();
                            }

                            if (replace && SeriesYValues[i].Count > index)
                            {
                                SeriesYValues[i][index] = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            }
                            else if (SeriesYValues[i].Count == index) 
                            {
                                SeriesYValues[i].Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                            }
                            else
                            {
                                SeriesYValues[i].Insert(index, Convert.ToDouble(yVal != null ? yVal : double.NaN));
                            }
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.TimeSpan)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = dataRow.GetField(XBindingPath);
                        XData = ((TimeSpan)xVal).TotalMilliseconds;

                        if (!(SeriesYValues[0] is List<double>))
                        {
                            SeriesYValues[0] = new List<double>();
                        }

                        // Check the Data Collection is linear or not
                        if (IsLinearData && index > 0 && XData < xValue[index - 1]) 
                        {
                            IsLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            xValue[index] = XData;
                        }
                        else if (xValue.Count == index) 
                        {
                            xValue.Add(XData);
                        }
                        else {
                            xValue.Insert(index, XData);
                        }

                        for (int i = 0; i < YPaths.Count(); i++)
                        {
                            object yVal = dataRow.GetField(YComplexPaths[i][0]);

                            if (!(SeriesYValues[i] is List<double>)) 
                            {
                                SeriesYValues[i] = new List<double>();
                            }

                            if (replace && SeriesYValues[i].Count > index) 
                            {
                                SeriesYValues[i][index] = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            }
                            else if (SeriesYValues[i].Count == index) 
                            {
                                SeriesYValues[i].Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                            }
                            else 
                            {
                                SeriesYValues[i].Insert(index, Convert.ToDouble(yVal != null ? yVal : double.NaN));
                            }
                        }

                        PointsCount = xValue.Count;
                    }
                }
                else 
                {
                    string[] tempYPath = YComplexPaths[0];
                    IList<double> yValue = SeriesYValues[0];
                    if (XValueType == ChartValueType.String) 
                    {
                        if (!(this.XValues is List<string>))
                            this.XValues = this.ActualXValues = new List<string>();
                        IList<string> xValue = this.XValues as List<string>;
                        object xVal = dataRow.GetField(XBindingPath);
                        object yVal = dataRow.GetField(tempYPath[0]);
                        if (replace && xValue.Count > index) 
                        {
                            xValue[index] = Convert.ToString(xVal);
                        }
                        else if (xValue.Count == index) 
                        {
                            xValue.Add(Convert.ToString(xVal));
                        }
                        else 
                        {
                            xValue.Insert(index, Convert.ToString(xVal));
                        }

                        if (replace && yValue.Count > index)
                        {
                            yValue[index] = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                        }
                        else if (yValue.Count == index)
                        {
                            yValue.Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                        }
                        else
                        {
                            yValue.Insert(index, Convert.ToDouble(yVal != null ? yVal : double.NaN));
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.Double ||
                        XValueType == ChartValueType.Logarithmic)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = dataRow.GetField(XBindingPath);
                        object yVal = dataRow.GetField(tempYPath[0]);
                        XData = Convert.ToDouble(xVal != null ? xVal : double.NaN);

                        // Check the Data Collection is linear or not
                        if (IsLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            IsLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            xValue[index] = XData;
                        }
                        else if (xValue.Count == index)
                        {
                            xValue.Add(XData);
                        }
                        else 
                        {
                            xValue.Insert(index, XData);
                        }

                        if (replace && yValue.Count > index)
                        {
                            yValue[index] = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                        }
                        else if (yValue.Count == index) 
                        {
                            yValue.Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                        }
                        else 
                        {
                            yValue.Insert(index, Convert.ToDouble(yVal != null ? yVal : double.NaN));
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.DateTime) 
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = dataRow.GetField(XBindingPath);
                        object yVal = dataRow.GetField(tempYPath[0]);
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
                        else if (xValue.Count == index)
                        {
                            xValue.Add(XData);
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        if (replace && yValue.Count > index)
                        {
                            yValue[index] = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                        }
                        else if (yValue.Count == index)
                        {
                            yValue.Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                        }
                        else 
                        {
                            yValue.Insert(index, Convert.ToDouble(yVal != null ? yVal : double.NaN));
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.TimeSpan)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = dataRow.GetField(XBindingPath);
                        object yVal = dataRow.GetField(tempYPath[0]);
                        XData = ((TimeSpan)xVal).TotalMilliseconds;

                        // Check the Data Collection is linear or not
                        if (IsLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            IsLinearData = false;
                        }

                        if (xVal != null && replace && xValue.Count > index)
                        {
                            xValue[index] = XData;
                        }
                        else if (xValue.Count == index)
                        {
                            xValue.Add(XData);
                        }
                        else if (xVal != null)
                        {
                            xValue.Insert(index, XData);
                        }

                        if (yVal != null && replace && yValue.Count > index)
                        {
                            yValue[index] = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                        }
                        else if (yValue.Count == index) 
                        {
                            yValue.Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                        }
                        else
                        {
                            yValue.Insert(index, Convert.ToDouble(yVal != null ? yVal : double.NaN));
                        }

                        PointsCount = xValue.Count;
                    }
                }

                if (replace && ActualData.Count > index)
                    ActualData[index] = obj;
                else if (ActualData.Count == index)
                    ActualData.Add(obj);
                else
                    ActualData.Insert(index, obj);
            }
        }

        /// <summary>
        /// Method implementation for clear unused segments.
        /// </summary>
        /// <param name="startIndex"></param>
        internal virtual void ClearUnUsedSegments(int startIndex) 
        {
            if (Segments.Count > startIndex) 
            {
                int count = Segments.Count;

                for (int i = startIndex; i < count; i++) {
                    Segments.RemoveAt(startIndex);
                }
            }
        }

        /// <summary>
        /// Called when the chart mouse up.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="position">position</param>
        internal virtual void OnSeriesMouseUp(object source, Point position)
        {
        }

        /// <summary>
        /// Called when the chart mouse down.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="position">position</param>
        internal virtual void OnSeriesMouseDown(object source, Point position)
        {
        }

        /// <summary>
        /// Method used to set SegmentSelectionBrush to selectedindex chartsegment.
        /// </summary>
        /// <param name="newIndex">new index</param>
        /// <param name="oldIndex">old index</param>
        internal virtual void SelectedIndexChanged(int newIndex, int oldIndex) 
        {
            if (ActualArea != null && !ActualArea.GetEnableSeriesSelection() && SelectionBehavior != null) 
            {
                // Reset the oldIndex segment Interior
                if (!SelectionBehavior.EnableMultiSelection) 
                {
                    if (SelectedSegmentsIndexes.Contains(oldIndex))
                        SelectedSegmentsIndexes.Remove(oldIndex);

                    OnResetSegment(oldIndex);
                }

                if (IsItemsSourceChanged) 
                {
                    return;
                }

                if (newIndex >= 0 && GetEnableSegmentSelection())
                {
                    if (!SelectedSegmentsIndexes.Contains(newIndex))
                        SelectedSegmentsIndexes.Add(newIndex);

                    // For adornment selection implementation
                    if (adornmentInfo is ChartDataLabelSettings && adornmentInfo.HighlightOnSelection)
                    {
                        UpdateAdornmentSelection(newIndex);
                    }

                    if (newIndex < Segments.Count && SelectionBehavior.SelectionBrush != null)
                    {
                        Segments[newIndex].BindProperties();
                        Segments[newIndex].IsSelectedSegment = true;
                    }

                    if (newIndex < Segments.Count)
                    {
                        OnSelectionChanged(newIndex, oldIndex);
                        PreviousSelectedIndex = newIndex;
                    }
                    else if (Segments.Count == 0) 
                    {
                        triggerSelectionChangedEventOnLoad = true;
                    }
                }
                else if (newIndex == -1) 
                {
                    OnSelectionChanged(newIndex, oldIndex);
                    PreviousSelectedIndex = newIndex;
                }
            }
            else if (newIndex >= 0 && Segments.Count == 0) 
            {
                triggerSelectionChangedEventOnLoad = true;
            }
        }

        /// <summary>
        /// Return IChartTranform value based upon the given size.
        /// </summary>
        /// <param name="size">Size of the panel.</param>
        /// <param name="create">Used to specify whether to create the charttransform for not.</param>
        /// <returns>returns IChartTransformer</returns>
        internal virtual IChartTransformer CreateTransformer(Size size, bool create)
        {
            if (create || ChartTransformer == null) 
            {
                ChartTransformer = ChartTransform.CreateCartesian(size, this);
            }

            return ChartTransformer;
        }

        internal virtual void SetDataLabelsVisibility(bool isShowDataLabels)
        {

        }

        internal virtual void GenerateDataTablePoints(string[] yPaths, IList<double>[] yLists) 
        {
            IEnumerator enumerator = (ItemsSource as DataTable).Rows.GetEnumerator();
            if (enumerator.MoveNext())
            {
                object xvalueType = null;
                for (int i = 0; i < UpdateStartedIndex; i++) 
                {
                    enumerator.MoveNext();
                }

                xvalueType = (enumerator.Current as DataRow).GetField(this.XBindingPath);
                XValueType = GetDataType(xvalueType);
                if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
                    XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
                {
                    if (!(XValues is List<double>))
                       ActualXValues = XValues = new List<double>();
                }
                else
                {
                    if (!(XValues is List<string>))
                        ActualXValues = XValues = new List<string>();
                }

                if (IsMultipleYPathRequired)
                {
                    if (string.IsNullOrEmpty(yPaths[0]))
                        return;
                    if (XValueType == ChartValueType.String) 
                    {
                        IList<string> xValue = XValues as List<string>;
                        do 
                        {
                            object xVal = null;
                            xVal = (enumerator.Current as DataRow).GetField(XBindingPath);
                            xValue.Add((string)xVal);
                            for (int i = 0; i < yPaths.Count(); i++) 
                            {
                                object yVal = (enumerator.Current as DataRow).GetField(yPaths[i]);
                                yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                            }

                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.Double ||
                       XValueType == ChartValueType.Logarithmic) 
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            object xVal = null;
                            xVal = (enumerator.Current as DataRow).GetField(XBindingPath);
                            XData = Convert.ToDouble(xVal ?? double.NaN);

                            // Check the Data Collection is linear or not
                            if (IsLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                IsLinearData = false;
                            }

                            xValue.Add(XData);
                            for (int i = 0; i < yPaths.Count(); i++)
                            {
                                object yVal = (enumerator.Current as DataRow).GetField(yPaths[i]);
                                yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                            }

                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.DateTime)
                    {
                        IList<double> xValue = XValues as List<double>;
                        do 
                        {
                            object xVal = null;
                            xVal = (enumerator.Current as DataRow).GetField(XBindingPath);
                            XData = ((DateTime)xVal).ToOADate();

                            // Check the Data Collection is linear or not
                            if (IsLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                IsLinearData = false;
                            }

                            xValue.Add(XData);
                            for (int i = 0; i < yPaths.Count(); i++)
                            {
                                object yVal = (enumerator.Current as DataRow).GetField(yPaths[i]);
                                yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                            }

                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.TimeSpan) 
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            object xVal = null;
                            xVal = (enumerator.Current as DataRow).GetField(XBindingPath);
                            XData = ((TimeSpan)xVal).TotalMilliseconds;

                            // Check the Data Collection is linear or not
                            if (IsLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1]) 
                            {
                                IsLinearData = false;
                            }

                            xValue.Add(XData);
                            for (int i = 0; i < yPaths.Count(); i++) 
                            {
                                object yVal = (enumerator.Current as DataRow).GetField(yPaths[i]);
                                yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                            }

                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                }
                else 
                {
                    if (string.IsNullOrEmpty(yPaths[0]))
                        return;
                    IList<double> yValue = yLists[0];
                    object xVal = null, yVal = null;
                    if (XValueType == ChartValueType.String) 
                    {
                        IList<string> xValue = this.XValues as List<string>;
                        do 
                        {
                            xVal = (enumerator.Current as DataRow).GetField(this.XBindingPath);
                            yVal = (enumerator.Current as DataRow).GetField(yPaths[0]);
                            xValue.Add((string)xVal);
                            yValue.Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.Double ||
                        XValueType == ChartValueType.Logarithmic)
                    {
                        IList<double> xValue = XValues as List<double>;
                        do
                        {
                            xVal = (enumerator.Current as DataRow).GetField(XBindingPath);
                            yVal = (enumerator.Current as DataRow).GetField(yPaths[0]);
                            XData = Convert.ToDouble(xVal ?? double.NaN);

                            // Check the Data Collection is linear or not.
                            if (IsLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                IsLinearData = false;
                            }

                            xValue.Add(XData);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.DateTime)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do 
                        {
                            xVal = (enumerator.Current as DataRow).GetField(this.XBindingPath);
                            yVal = (enumerator.Current as DataRow).GetField(yPaths[0]);
                            XData = ((DateTime)xVal).ToOADate();

                            // Check the Data Collection is linear or not
                            if (IsLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1]) 
                            {
                                IsLinearData = false;
                            }

                            xValue.Add(XData);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.TimeSpan)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do 
                        {
                            xVal = (enumerator.Current as DataRow).GetField(this.XBindingPath);
                            yVal = (enumerator.Current as DataRow).GetField(yPaths[0]);
                            XData = ((TimeSpan)xVal).TotalMilliseconds;

                            // Check the Data Collection is linear or not
                            if (IsLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                IsLinearData = false;
                            }

                            xValue.Add(((TimeSpan)xVal).TotalMilliseconds);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        PointsCount = xValue.Count;
                    }
                }
            }
        }

        internal virtual void DataTableRowChanged(object sender, DataRowChangeEventArgs e)
        {
            var index = (ItemsSource as DataTable).Rows.IndexOf(e.Row);
            switch (e.Action) 
            {
                case DataRowAction.Add:
                    SetIndividualDataTablePoint(index, e.Row, false);
                    break;
                case DataRowAction.Change:
                    SetIndividualDataTablePoint(index, e.Row, true);
                    break;
                case DataRowAction.Delete:
                    if (this.ItemsSource != null)
                        {
                        if (XValues is IList<double>) 
                        {
                            (XValues as IList<double>).RemoveAt(index);
                            PointsCount--;
                        }
                        else if (XValues is IList<string>)
                        {
                            (XValues as IList<string>).RemoveAt(index);
                            PointsCount--;
                        }

                        for (int i = 0; i < SeriesYValues.Count(); i++)
                        {
                            SeriesYValues[i].RemoveAt(index);
                        }

                        ActualData.RemoveAt(index);
                    }

                    break;
            }

            if ((this is TriangularSeriesBase || this is CircularSeries) && ActualArea.PlotArea != null)
                ActualArea.PlotArea.ShouldPopulateLegendItems = true;
            isTotalCalculated = false;
            ScheduleUpdateChart();
        }

        /// <summary>
        /// Updates the selection when selected index collection changed.
        /// </summary>
        internal virtual void SelectedSegmentsIndexes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) 
        {
            switch (e.Action)
                {
                case NotifyCollectionChangedAction.Add:

                    if (e.NewItems != null && SelectionBehavior != null &&
                                SelectionBehavior.EnableMultiSelection) 
                    {
                        var oldIndex = PreviousSelectedIndex;

                        int newIndex = (int)e.NewItems[0];

                        if (newIndex >= 0) 
                        {
                            // For adornment selection implementation
                            if (adornmentInfo is ChartDataLabelSettings && adornmentInfo.HighlightOnSelection) 
                            {
                                UpdateAdornmentSelection(newIndex);
                            }

                            // Set the SegmentSelectionBrush to newIndex segment Interior
                            if (newIndex < Segments.Count && SelectionBehavior.SelectionBrush != null) 
                            {
                                Segments[newIndex].BindProperties();
                                Segments[newIndex].IsSelectedSegment = true;
                            }

                            if (newIndex < Segments.Count) 
                            {
                                OnSelectionChanged(newIndex, oldIndex);
                                PreviousSelectedIndex = newIndex;
                            }
                            else if (Segments.Count == 0) 
                            {
                                triggerSelectionChangedEventOnLoad = true;
                            }
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:

                    if (e.OldItems != null && SelectionBehavior != null &&
                                SelectionBehavior.EnableMultiSelection)
                    {
                        int newIndex = (int)e.OldItems[0];

                        OnSelectionChanged(newIndex, PreviousSelectedIndex);
                        OnResetSegment(newIndex);
                        PreviousSelectedIndex = newIndex;
                    }

                    break;
            }
        }

        /// <summary>
        ///  Timer Tick Handler for initial delay in opening the Tooltip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal virtual void InitialDelayTimer_Tick(object sender, object e)
        {
            var canvas = ActualArea.GetAdorningCanvas();
            var chartTooltip = ActualArea.Tooltip;
            chartTooltip.PolygonPath = " ";

            int duration = ChartTooltip.GetActualDuration(ActualArea.TooltipBehavior, ChartTooltip.GetDuration(this));

            if (!canvas.Children.Contains(chartTooltip)) 
            {
                canvas.Children.Add(chartTooltip);
            }

            AddTooltip();

            if (ChartTooltip.GetActualEnableAnimation(ActualArea.TooltipBehavior, ChartTooltip.GetEnableAnimation(this))) {
                if (ActualArea is ChartBase)
                {
                    SetDoubleAnimation(chartTooltip);
                }
                else
                {
                    FadeInAnimation(ref chartTooltip);
                }
            }

            Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
            Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);

            InitialDelayTimer.Stop();
            Timer.Interval = new TimeSpan(0, 0, 0, 0, duration);
            Timer.Start();
        }

        internal virtual DataTemplate GetDefaultTooltipTemplate() 
        {
            if (defaultTooltipTemplate == null)
            {
                this.defaultTooltipTemplate = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipTemplate"] as DataTemplate;
            }

            return defaultTooltipTemplate;
        }

        /// <summary>
        /// Calculate and draw tooltip based on Position of Tooltip.
        /// </summary>
        internal virtual void AddTooltip()
        {
            previousMousePosition = mousePosition;
            var chartTooltip = ActualArea.Tooltip as ChartTooltip;
            chartTooltip.PreviousSeries = this;

            if (ToolTipTag != null) 
            {
                chartTooltip.UpdateLayout();
                Size tooltipSize = new Size(chartTooltip.ActualWidth, chartTooltip.ActualHeight);
                Point tooltipPosition = new Point();
                var tooltipBehavior = ActualArea.TooltipBehavior;
                ActualTooltipPosition = TooltipPosition.Auto;

                tooltipPosition = GetDataPointPosition(chartTooltip);
                if (!(this is CircularSeries || this is TriangularSeriesBase || this is PolarSeries))
                {
                    Rect clipRect = this.ActualArea.SeriesClipRect;
                    if (!(IsActualTransposed && (this is ColumnSeries || this is StackedColumnSeries)))
                    {
                        tooltipPosition = SetTooltipMarkerPosition(tooltipPosition, chartTooltip);
                    }

                    if (!clipRect.Contains(tooltipPosition))
                    {
                        tooltipPosition = mousePosition;
                        ActualTooltipPosition = TooltipPosition.Pointer;
                    }
                }

                tooltipPosition = Position(tooltipPosition, ref chartTooltip);
                Point offset;
                HorizontalPosition horizontal = HorizontalPosition.Center;
                VerticalPosition vertical = VerticalPosition.Bottom;
                offset = AlignTooltip(tooltipSize, tooltipPosition, out horizontal, out vertical);
                chartTooltip.Margin = ChartTooltip.GetActualTooltipMargin(tooltipBehavior, ChartTooltip.GetTooltipMargin(this));

                if (tooltipBehavior != null)
                {
                    chartTooltip.BackgroundStyle = tooltipBehavior.Style;
                    chartTooltip.LabelStyle = tooltipBehavior.LabelStyle;
                }

                if (ActualArea is ChartBase) 
                {
                    chartTooltip.PolygonPath = ChartUtils.GenerateTooltipPolygon(tooltipSize, horizontal, vertical);
                }
            }
        }

        /// <summary>
        /// Calculate the position of the tooltip based on ChartSegment.
        /// </summary>
        /// <param name="tooltip">Instance of ChartTooltip</param>
        /// <returns></returns>
        internal virtual Point GetDataPointPosition(ChartTooltip tooltip)
        {
            var chartSegment = tooltip.DataContext as ChartSegment;
            Point newPosition = new Point();
            if (chartSegment != null) 
            {
                Point point = ChartTransformer.TransformToVisible(chartSegment.XRange.End, chartSegment.YRange.End);
                newPosition.X = point.X + ActualArea.SeriesClipRect.Left - (tooltip as UIElement).DesiredSize.Width / 2;
                newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top - (tooltip as UIElement).DesiredSize.Height;
            }

            return newPosition;
        }

        internal virtual VerticalPosition GetVerticalPosition(VerticalPosition verticalPosition)
        {
            return verticalPosition;
        }

        /// <summary>
        /// Remove tooltip from adorning canvas
        /// </summary>
        internal virtual void RemoveTooltip() 
        {
            if (ActualArea == null) 
            {
                return;
            }

            var canvas = ActualArea.GetAdorningCanvas();
            if (canvas == null) return;
            for (int i = 0; i < canvas.Children.Count;) 
            {
                if (canvas.Children[i] is ChartTooltip) 
                {
                    if (storyBoard != null) 
                    {
                        var chartTooltip = canvas.Children[i] as ChartTooltip;
                        Canvas.SetLeft(chartTooltip, chartTooltip.LeftOffset);
                        Canvas.SetTop(chartTooltip, chartTooltip.TopOffset);
                        storyBoard.Stop();
                        storyBoard = null;
                    }

                    canvas.Children.Remove(canvas.Children[i]);
                    continue;
                }

                i++;
            }
        }

        /// <summary>
        /// Add and Update the Tooltip
        /// </summary>
        /// <param name="source"></param>
        internal virtual void UpdateTooltip(object source)
        {
            if (EnableTooltip)
            {
                FrameworkElement element = source as FrameworkElement;

                if (element != null)
                {
                    object customTag = GetTooltipTag(element);
                    if (ToolTipTag != null && !ToolTipTag.Equals(customTag))
                    {
                        RemoveTooltip();
                        Timer.Stop();
                        ActualArea.Tooltip = new ChartTooltip();
                    }

                    UpdateSeriesTooltip(customTag);
                }
            }
        }

        internal virtual object GetTooltipTag(FrameworkElement element) 
        {
            object tooltipTag = null;

            if (element.Tag is ChartSegment)
                tooltipTag = element.Tag;
            else if (element.DataContext is ChartSegment && !(element.DataContext is ChartDataLabel))
                tooltipTag = element.DataContext;
            else if (element.DataContext is ChartDataMarkerContainer)
            {
                tooltipTag = GetSegment((element.DataContext as ChartDataMarkerContainer).Adornment.Item);
            }
            else 
            {
                var contentPresenter = VisualTreeHelper.GetParent(element) as ContentPresenter;

                if (contentPresenter != null && contentPresenter.Content is ChartDataLabel)
                {
                    tooltipTag = GetSegment((contentPresenter.Content as ChartDataLabel).Item);
                }
                else
                {
                    int index = ChartExtensionUtils.GetAdornmentIndex(element);

                    if (index != -1 && index < Adornments.Count && index < Segments.Count) 
                    {
                        tooltipTag = GetSegment(Adornments[index].Item);
                    }
                }
            }

            return tooltipTag;
        }

        /// <summary>
        /// This method used to get the chart data at an index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal virtual ChartDataPointInfo? GetDataPoint(int index)
        {
            return dataPoint;
        }

        internal virtual void OnResetSegment(int index) 
        {
            if (index < Segments.Count && index >= 0) 
            {
                Segments[index].BindProperties();
                Segments[index].IsSelectedSegment = false;
                if (adornmentInfo is ChartDataLabelSettings)
                {
                    AdornmentPresenter.ResetAdornmentSelection(index, false);
                }
            }
        }

        /// <summary>
        /// Called when selection changed in load time
        /// </summary>
        internal virtual void RaiseSelectionChangedEvent() 
        {
            if (triggerSelectionChangedEventOnLoad && SelectionBehavior != null)
            {
                int index = SelectionBehavior.SelectedIndex;

                if (index >= 0) 
                {
                    if (PointsCount > index) 
                    {
                        OnSelectionChanged(index, -1);
                        PreviousSelectedIndex = index;
                    }
                    var newIndexes = new List<int>() { index };
                    UpdateLoadTimeAdornmentSelection(newIndexes);
                }
                else if (SelectionBehavior.SelectedIndexes.Count > 0) 
                {
                    var newIndexes = SelectionBehavior.SelectedIndexes;
                    var oldIndexes = new List<int>();

                    SelectionBehavior.OnSelectionChanged(newIndexes, oldIndexes, this);
                    UpdateLoadTimeAdornmentSelection(newIndexes);
                }

                triggerSelectionChangedEventOnLoad = false;
            }
        }

        /// <summary>
        /// Set SelectionChanged event args
        /// </summary>
        internal virtual void UpdateLoadTimeAdornmentSelection(List<int> indexes)
        {
            if (Segments.Count != 0) 
            {
                // Selects the adornment when the mouse is over or clicked on adornments(adornment selection).
                if (adornmentInfo is ChartDataLabelSettings && adornmentInfo.HighlightOnSelection) 
                {
                    foreach (var index in indexes) 
                    {
                        UpdateAdornmentSelection(index);
                    }
                }
            }
        }

        internal virtual bool GetAnimationIsActive() 
        {
            return false;
        }

        internal virtual void CalculateSegments() 
        {
            ApplyTemplate();

            if (PointsCount > 0) 
            {
                //CartesianSeries segments and data labels are created while ActualXAxis and ActualYAxis are null,
                //so added condition to restrict the segment creation when the axis is null.
                if ((ActualArea.AreaType != ChartAreaType.CartesianAxes) || (this is CartesianSeries && ActualXAxis != null && ActualYAxis != null))
                    GenerateSegments();

                if (triggerSelectionChangedEventOnLoad)
                    RaiseSelectionChangedEvent();
            }
            else if (Segments == null) 
            {
                return;
            }
            else if (PointsCount == 0 && Segments.Count > 0) // WPF-13974 -Last segment have cleared from the collection while datamodel is changed.
            {
                ClearUnUsedSegments(PointsCount);
            }
        }

        internal virtual void GenerateSegments() 
        {
        }

        internal abstract ChartSegment CreateSegment();

        internal virtual void UpdateOnSeriesBoundChanged(Size size) 
        {
            if (Segments == null)
            {
                return;
            }

            if (SeriesPanel == null && Segments.Count > 0) 
            {
                SeriesPanel = Segments[0].Series.GetTemplateChild("SyncfusionChartSeriesPanel") as ChartSeriesPanel;
            }

            if (SeriesPanel != null) 
            {
                foreach (ChartSegment segment in Segments) 
                {
                    segment.OnSizeChanged(size);
                }

                SeriesPanel.Update(size);
            }
        }

        /// <summary>
        /// This method used to get the SfChart data at a mouse position.
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        internal virtual ChartDataPointInfo GetDataPoint(Point mousePos) 
        {
            double xVal = double.NaN;
            double yVal = double.NaN;
            double stackedYValue = double.NaN;
            dataPoint = new ChartDataPointInfo();
            int index = -1;
            if (this.ActualArea.SeriesClipRect.Contains(mousePos))
            {
                var point = new Point(mousePos.X - this.ActualArea.SeriesClipRect.Left,
                                      mousePos.Y - this.ActualArea.SeriesClipRect.Top);

                if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed) 
                {
                    double xStart = ActualXAxis.VisibleRange.Start;
                    double xEnd = ActualXAxis.VisibleRange.End;
                    point = new Point(ActualArea.ActualPointToValue(ActualXAxis, point), ActualArea.ActualPointToValue(ActualYAxis, point));
                    double range = Math.Round(point.X);
                    if (range <= xEnd && range >= xStart && range >= 0)
                    {
                        xVal = range;
                        List<double> values = new List<double>();
                        if (DistinctValuesIndexes.Count > 0 && DistinctValuesIndexes.ContainsKey(xVal))
                        {
                            values = (from value in DistinctValuesIndexes[xVal] select ActualSeriesYValues[0][value]).ToList();
                            yVal = values[0];
                        }
                    }

                    dataPoint.XData = xVal;
                    if (!double.IsNaN(xVal))
                        index = DistinctValuesIndexes[xVal][0];
                    dataPoint.YData = yVal;
                    dataPoint.Index = index;
                    dataPoint.Series = this;
                }
                else
                {
                    this.FindNearestChartPoint(point, out xVal, out yVal, out stackedYValue);
                    dataPoint.XData = xVal;
                    index =
                        this.GetXValues().IndexOf(xVal);
                    dataPoint.YData = yVal;
                    dataPoint.Index = index;
                    dataPoint.Series = this;
                }

                if (index > -1 && ActualData.Count > index)
                    dataPoint.Item = ((this is ColumnSeries || this is FastColumnBitmapSeries || this is StackedColumnSeries
                        || this is FastScatterBitmapSeries) &&
                        ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed) ?
                    GroupedActualData[index] :
                    dataPoint.Item = ActualData[index];
            }

            return dataPoint;
        }

        /// WPF-25124 Animation not working properly when resize the window.
        /// <summary>
        /// This method is used to reset the Adornment label animation.
        /// </summary>
        internal virtual void ResetAdornmentAnimationState() 
        {
            if (adornmentInfo != null && adornmentInfo.Visible && adornmentInfo.LabelPresenters != null) {
                foreach (FrameworkElement label in adornmentInfo.LabelPresenters)
                {
                    label.ClearValue(RenderTransformProperty);
                    label.ClearValue(OpacityProperty);
                }
            }
        }

        internal virtual void Animate() 
        {

        }

        internal virtual void Dispose() 
        {
            if (SelectionBehavior != null)
                SelectionBehavior = null;

            if (adornmentInfo != null) 
            {
                adornmentInfo.Dispose();
                adornmentInfo = null;
            }

            if (m_adornments != null) 
            {
                foreach (var adornment in m_adornments)
                {
                    adornment.Series = null;
                    adornment.Dispose();
                }

                m_adornments.Clear();
            }

            if (m_visibleAdornments != null)
             {
                foreach (var adornment in m_visibleAdornments) 
                {
                    adornment.Series = null;
                    adornment.Dispose();
                }

                m_visibleAdornments.Clear();
            }

            if (AdornmentPresenter != null)
            {
                if (AdornmentPresenter.VisibleSeries != null)
                {
                    AdornmentPresenter.VisibleSeries.Clear();
                }

                AdornmentPresenter.Series = null;
                AdornmentPresenter.Children.Clear();
                AdornmentPresenter.UnHookEvents();
                AdornmentPresenter = null;
            }

            if (PropertyChanged != null)
            {
                foreach (var handler in PropertyChanged.GetInvocationList()) 
                {
                    PropertyChanged -= handler as PropertyChangedEventHandler;
                }

                PropertyChanged = null;
            }

            if (SeriesPanel != null)
                {
                SeriesPanel.Dispose();
                SeriesPanel = null;
            }

            if (Segments != null) 
            {
                foreach (var segment in Segments) 
                {
                    segment.Dispose();
                }
                Segments.Clear();
            }

            if (upperSeriesPixels != null)
            {
                upperSeriesPixels.Clear();
                upperSeriesPixels = null;
            }

            if (selectedSegmentPixels != null) 
            {
                selectedSegmentPixels.Clear();
                selectedSegmentPixels = null;
            }

            if (ToggledLegendIndex != null) 
            {
                ToggledLegendIndex.Clear();
                ToggledLegendIndex = null;
            }

            if (GroupedActualData != null) 
            {
                GroupedActualData.Clear();
                GroupedActualData = null;
            }

            if (DistinctValuesIndexes != null) 
            {
                DistinctValuesIndexes.Clear();
                DistinctValuesIndexes = null;
            }

            if (GroupedXValues != null)
            {
                GroupedXValues.Clear();
                GroupedXValues = null;
            }

            if (GroupedXValuesIndexes != null)
            {
                GroupedXValuesIndexes.Clear();
                GroupedXValuesIndexes = null;
            }

            if (bitmapPixels != null) 
            {
                bitmapPixels.Clear();
                bitmapPixels = null;
            }

            if (bitmapRects != null)
            {
                bitmapRects.Clear();
                bitmapRects = null;
            }

            if (_selectedSegmentsIndexes != null)
            {
                _selectedSegmentsIndexes.Clear();
                _selectedSegmentsIndexes = null;
            }

            if (XValues != null) 
            {
                XValues = null;
            }

            if (ActualData != null) 
            {
                ActualData.Clear();
                ActualData = null;
            }

            if (ActualXValues != null)
            {
                ActualXValues = null;
            }

            if (scaleXAnimation != null)
                scaleXAnimation = null;

            if (scaleYAnimation != null)
                scaleYAnimation = null;

            this.Timer?.Stop();

            this.Timer = null;
            this.InitialDelayTimer = null;
            ItemsSource = null;

            if (SelectedSegmentsIndexes != null)
                SelectedSegmentsIndexes.CollectionChanged -= SelectedSegmentsIndexes_CollectionChanged;

            ActualArea = null;
            ChartTransformer = null;
            dataPoint = null;
            SeriesRootPanel = null;
            ToolTipTag = null;
        }

        /// <summary>
        /// This method used to get the chart data index at an SfChart co-ordinates
        /// </summary>
        /// <param name="point">Used to indicate the current x and y co-ordinates</param>
        /// <returns>Returns data index of type <c>int</c></returns>
        internal virtual int GetDataPointIndex(Point point) 
        {
            Canvas canvas = Chart.GetAdorningCanvas();
            double left = Chart.ActualWidth - canvas.ActualWidth;
            double top = Chart.ActualHeight - canvas.ActualHeight;
            ChartDataPointInfo data = null;
            point.X = point.X - left + Chart.Margin.Left;
            point.Y = point.Y - top + Chart.Margin.Top;

            data = GetDataPoint(point);

            if (data != null)
                return data.Index;
            else
                return -1;
        }

        /// <summary>
        /// This method used to generate bitmap segment pixels.
        /// </summary>
        internal virtual void GeneratePixels() 
        {
        }

        /// <summary>
        /// Method used to return the hittest series while mouse action.
        /// </summary>
        /// <returns></returns>
        internal virtual bool IsHitTestSeries() 
        {
            if (!Chart.isBitmapPixelsConverted)
                Chart.ConvertBitmapPixels();

            if (Pixels.Contains(Chart.currentBitmapPixel))
                return true;

            return false;
        }

        #endregion

        #region Internal Static Methods

        internal static void OnBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ChartSeries series = d as ChartSeries;

            if (args.NewValue is DataPointSelectionBehavior newBehavior) 
            {
                newBehavior.Series = series;
            }

            series.UpdateBehavior(args.OldValue as ChartBehavior, args.NewValue as ChartBehavior);

        }

        /// <summary>
        /// Gets the Spacing for the SideBySide segments.
        /// </summary>
        /// <param name="obj">ChartSeries object</param>
        /// <returns>returns a double value.</returns>
        internal static double GetSpacing(DependencyObject obj) 
        {
            return (double)obj.GetValue(SpacingProperty);
        }

        internal static void AddTooltipBehavior(ChartBase chart)
        {
            if (chart != null && chart.TooltipBehavior == null)
            {
                chart.TooltipBehavior = new ChartTooltipBehavior();
            }
        }

        internal static void OnAdornmentsInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartSeries series)
            {
                if (e.OldValue != null)
                {
                    var adornmentInfo = e.OldValue as ChartDataLabelSettings;
                    series.Adornments.Clear();
                    series.VisibleAdornments.Clear();

                    if (adornmentInfo != null) 
                    {
                        adornmentInfo.ClearChildren();
                        adornmentInfo.Series = null;
                    }
                }

                if (e.NewValue != null)
                {
                    series.adornmentInfo = e.NewValue as ChartDataLabelSettings;
                    series.AdornmentsInfo.Series = series;
                    if (series.Chart != null && series.AdornmentsInfo != null)
                    {
                        ////Panel panel = series.Area.GetMarkerPresenter();
                        Panel panel = series.AdornmentPresenter;
                        if (panel != null)
                        {
                            series.AdornmentsInfo.PanelChanged(panel);
                            series.Chart.ScheduleUpdate();
                        }
                    }
                }
            }
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc />
        protected override void OnPointerPressed(PointerRoutedEventArgs e) 
        {
            OnSeriesMouseDown(e.OriginalSource, e.GetCurrentPoint(this).Position);
        }

        /// <inheritdoc />
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            SeriesPointerMoved(e);
        }

        /// <inheritdoc />
        protected override void OnTapped(TappedRoutedEventArgs e) 
        {
            if (e.PointerDeviceType == PointerDeviceType.Touch) 
            {
                UpdateTooltip(e.OriginalSource);
            }

            base.OnTapped(e);
        }

        /// <inheritdoc />
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            if (!isTap) 
            {
                ChartTooltipBehavior tooltipBehavior = ActualArea != null && ActualArea.TooltipBehavior != null ? ActualArea.TooltipBehavior : null;
                MousePointerExit(tooltipBehavior);
            }
            isTap = false;
        }


        /// <inheritdoc />
        protected override void OnPointerReleased(PointerRoutedEventArgs e) 
        {
            isTap = true;
            OnSeriesMouseUp(e.OriginalSource, e.GetCurrentPoint(this).Position);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate() 
        {
            base.OnApplyTemplate();
            SeriesRootPanel = this.GetTemplateChild("SyncfusionChartSeriesRootPanel") as Panel;
            SeriesPanel = this.GetTemplateChild("SyncfusionChartSeriesPanel") as ChartSeriesPanel;
            SeriesPanel.Series = this;

            /* In case of stacking series, we position the corresponding adornments above series panel, 
              we have specifically checked for adornments position as bottom to show adornment label for next series*/
            var stackingSeriesBase = this as StackedSeriesBase;
            if (stackingSeriesBase != null)
            {
                if (stackingSeriesBase.IsSideBySide)
                {
                    if (adornmentInfo != null) 
                    {
                        BarLabelAlignment markerPosition = this.adornmentInfo.GetAdornmentPosition();
                        switch (markerPosition) 
                        {
                            case BarLabelAlignment.Middle:
                            case BarLabelAlignment.Top:
                                Canvas.SetZIndex(SeriesPanel.Series, -ActualArea.GetSeriesIndex(this));
                                break;
                            default:
                                Canvas.SetZIndex(SeriesPanel.Series, ActualArea.GetSeriesIndex(this));
                                break;
                        }
                    }
                }
                else 
                {
                    Canvas.SetZIndex(SeriesPanel.Series, -ActualArea.GetSeriesIndex(this));
                }
            }
        }

        #endregion

        #region Private Static Methods

        private static void OnTooltipTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs args) 
        {
            if (d is ChartSeries series) 
            {
                if (series.ActualArea != null && series.ActualArea.Tooltip != null)
                    (series.ActualArea.Tooltip).ContentTemplate = args.NewValue as DataTemplate;
            }
        }

        private static void OnListenPropertyChangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            (d as ChartSeries).HookPropertyChangedEvent((bool)args.NewValue);
        }

        private static void OnEnableTooltipChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (d is ChartSeries instance) 
            {
                if (instance.ActualArea != null && (bool)args.NewValue == true) 
                {
                    instance.ActualArea.Tooltip = new ChartTooltip();
                }
                else if (instance.ActualArea != null && instance.ActualArea.Tooltip != null)
                {
                    Canvas canvas = (instance.ActualArea).GetAdorningCanvas();

                    if (canvas != null && canvas.Children.Contains((instance.ActualArea.Tooltip)))
                        canvas.Children.Remove(instance.ActualArea.Tooltip);
                }

                if (instance.ActualArea != null && instance.ActualArea is ChartBase && (bool)args.NewValue == true) 
                {
                    AddTooltipBehavior(instance.ActualArea);
                }
            }
        }

        private static void OnPaletteBrushesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSeries).OnPaletteBrushesChanged(e);
        }

        private static void OnSegmentSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartSeries series)
            {
                if (series.ActualArea != null)
                    series.ActualArea.ScheduleUpdate();
            }
        }

        private static void OnLabelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }

        private static void OnVisibilityOnLegendChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartSeries series)
            {
                if (series.ActualArea != null && series.ActualArea.PlotArea != null)
                {
                    series.ActualArea.PlotArea.ShouldPopulateLegendItems = true;
                    series.ActualArea.ScheduleUpdate();
                }
            }
        }

        private static void OnIsSeriesVisibleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) 
        {
            (obj as ChartSeries).IsSeriesVisibleChanged(args);
        }

        private static void OnLegendIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSeries).UpdateLegendIconTemplate(true);
        }

        private static void OnLegendIconTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSeries).UpdateLegendIconTemplate(false);
        }

        private static void OnBindingPathXChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var chartSeries = obj as ChartSeries;
            if (args.NewValue != null)
                chartSeries.XComplexPaths = args.NewValue.ToString().Split('.');
            chartSeries.OnBindingPathChanged();
        }

        private static void OnAppearanceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var chartSeries = obj as ChartSeries;
            chartSeries.OnAppearanceChanged(chartSeries);
        }

        private static void OnDataSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args) 
        {
            ChartSeries series = obj as ChartSeries;

            series.OnDataSourceChanged(args);
        }


        /// <summary>
        /// set fade animation for initial show delay
        /// </summary>
        /// <param name="chartTooltip"></param>
        private static void FadeInAnimation(ref ChartTooltip chartTooltip)
        {
            var storyBoard1 = new Storyboard();
            var fadeInAnimation = new DoubleAnimation() 
            {
                From = 0,
                To = 1,
                Duration = new Duration().GetDuration(new TimeSpan(0, 0, 0, 1))
            };
            Storyboard.SetTarget(fadeInAnimation, chartTooltip);
            Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");
            storyBoard1.Children.Add(fadeInAnimation);
            storyBoard1.Begin();
        }

        internal static void OnStrokeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            if (obj is ChartSeries chartSeries)
            {
                OnStrokeChanged(chartSeries);
            }
        }

        internal static void OnStrokeChanged(ChartSeries obj)
        {
            if (obj.IsBitmapSeries)
                obj.ScheduleUpdateChart();
        }

        #endregion

        #region Private Methods

        private static void OnShowDataLabelsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) 
        {
            if (d is ChartSeries series)
            {
                series.SetDataLabelsVisibility(series.ShowDataLabels);
                if (series.Chart == null)
                    return;

                Panel panel = series.AdornmentPresenter;
                Canvas chartLabelPresenter = series.Chart.DataLabelPresenter;
                if (panel == null || chartLabelPresenter == null)
                    return;

                if ((bool)e.NewValue) 
                {
                    if (!chartLabelPresenter.Children.Contains(panel))
                        chartLabelPresenter.Children.Add(panel);

                    if (series.AdornmentsInfo != null && series.Adornments.Count > 0)
                    {
                        series.AdornmentPresenter.Update(series.GetAvailableSize());
                        series.AdornmentPresenter.Arrange(series.GetAvailableSize());
                    }
                    else if (series.Adornments != null && series.Adornments.Count == 0)
                    {
                        series.Invalidate();
                        series.AdornmentsInfo?.PanelChanged(panel);
                        series.AdornmentsInfo?.OnAdornmentPropertyChanged();
                    }
                }
                else
                {
                    if (chartLabelPresenter.Children.Contains(panel))
                        chartLabelPresenter.Children.Remove(panel);
                }
            }
        }

        private void OnPaletteBrushesChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.ActualArea != null) 
            {
                this.Segments.Clear();
                if (ActualArea.PlotArea != null)
                    ActualArea.PlotArea.ShouldPopulateLegendItems = true;
                ScheduleUpdateChart();
            }
        }

        private void IsSeriesVisibleChanged(DependencyPropertyChangedEventArgs args)
        {
            if (ActualArea != null) 
            {
                var isPolarRadarSeriesBase = this is PolarSeries;
                if ((bool)args.NewValue) 
                {
                    if (ActualArea.ActualSeries.Contains(this) && !ActualArea.VisibleSeries.Contains(this) && !isPolarRadarSeriesBase) 
                    {
                        int pos = ActualArea.GetSeriesIndex(this);
                        int count = ActualArea.VisibleSeries.Count;
                        ActualArea.VisibleSeries.Insert(pos > count ? count : pos, this);
                    }

                    this.AdornmentPresenter.Visibility = Visibility = Visibility.Visible;
                }
                else 
                {
                    if (ActualArea.VisibleSeries.Contains(this) && !isPolarRadarSeriesBase) 
                    {
                        ActualArea.VisibleSeries.Remove(this);
                    }

                    this.AdornmentPresenter.Visibility = Visibility = Visibility.Collapsed;
                    RemoveTooltip();
                    Timer.Stop();
                }

                ActualArea.SBSInfoCalculated = false;
                if (ActualArea is ChartBase chartBase)
                    chartBase.AddOrRemoveBitmap();
                ScheduleUpdateChart();
            }
        }

        /// <summary>
        /// Timer Tick Handler for closing the Tooltip
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Timer_Tick(object sender, object e)
        {
            RemoveTooltip();
            Timer?.Stop();
        }

        private void HookComplexProperty(object parentObj, string[] paths)
        {
            for (int i = 0; i < paths.Length; i++) 
            {
                parentObj = GetComplexArrayPropertyValue(parentObj, paths[i]);

                var notifcationObject = parentObj as INotifyPropertyChanged;
                if (notifcationObject != null) 
                {
                    notifcationObject.PropertyChanged -= OnItemPropertyChanged;
                    notifcationObject.PropertyChanged += OnItemPropertyChanged;
                }
            }
        }

        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e) 
        {
            if (isNotificationSuspended) 
            {
                isPropertyNotificationSuspended = true;
                return;
            }

            if (isComplexYProperty || XBindingPath.Contains('.'))
            {
                ComplexPropertyChanged(sender, e);
            }
            else if (XBindingPath == e.PropertyName
                    || YPaths != null && YPaths.Contains(e.PropertyName))
            {
                int position = -1;
                IEnumerable itemsSource = (ItemsSource as IEnumerable);
                foreach (object obj in itemsSource) 
                {
                    position++;

                    if (obj == sender)
                        break;
                }

                if (position != -1) 
                {
                    SetIndividualPoint(position, sender, true);

                    //WPF-53274 Legend item label does not update properly while changing the X-Value dynamically in PieSeries
                    if ((this is TriangularSeriesBase || this is CircularSeries) && this.ActualArea != null && this.ActualArea.PlotArea != null)
                    {
                        this.ActualArea.PlotArea.ShouldPopulateLegendItems = true;
                    }

                    if (!isRepeatPoint)
                        this.ScheduleUpdateChart();
                }
            }
        }

        private void ComplexPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int position = -1;
            bool isYPath = false;
            bool isZPath = false;
            object parentObj = null;
            var paths = XComplexPaths;
            for (int i = 0; i < YPaths.Length; i++)
            {
                if (YPaths[i].Contains(e.PropertyName))
                {
                    isYPath = true;
                    if (isYPath)
                        paths = YComplexPaths[i];
                    break;
                }
            }

            if (XBindingPath.Contains(e.PropertyName) || isYPath
                || isZPath) {
                IEnumerable itemsSource = (ItemsSource as IEnumerable);
                foreach (object obj in itemsSource)
                {
                    parentObj = obj;
                    for (int i = 0; i < paths.Length - 1; i++)
                    {
                        parentObj = ReflectedObject(parentObj, paths[i]);
                    }

                    position++;

                    if (parentObj == sender)
                    {
                        parentObj = obj;
                        break;
                    }
                }

                if (position != -1) 
                {
                    SetIndividualPoint(position, parentObj, true);
                    this.ScheduleUpdateChart();
                }
            }
        }

        private void OnAppearanceChanged(ChartSeries obj) 
        {
            if (IsBitmapSeries)
                obj.ScheduleUpdateChart();
            else if (obj.adornmentInfo != null) 
            {
                obj.adornmentInfo.UpdateLabels(); // WPF-19938 - UseSeriesPalette not updated when series interior is changed
                obj.adornmentInfo.UpdateConnectingLines();
            }
        }

        /// <summary>
        /// Method to unhook the collection change event for the given collection
        /// </summary>      
        /// <param name="dataSource"></param>
        private void UnHookPropertyChanged(object dataSource) 
        {
            if (dataSource != null) 
            {
                var enumerableDataSource = dataSource as IEnumerable;
                if (enumerableDataSource == null) return;
                IEnumerator enumerator = enumerableDataSource.GetEnumerator();

                if (!enumerator.MoveNext()) return;
                do
                {
                    INotifyPropertyChanged collection = enumerator.Current as INotifyPropertyChanged;
                    if (collection != null) 
                    {
                        collection.PropertyChanged -= OnItemPropertyChanged;
                    }
                }
                while (enumerator.MoveNext());
            }
        }

        /// <summary>
        /// Method to unhook the PropertyChange event for individual data point
        /// </summary>
        /// <param name="needPropertyChange"></param>
        /// <param name="obj"></param>
        private void UnHookPropertyChangedEvent(bool needPropertyChange, object obj) 
        {
            INotifyPropertyChanged model = obj as INotifyPropertyChanged;
            if (model != null && needPropertyChange)
                model.PropertyChanged -= OnItemPropertyChanged;
        }

        /// <summary>
        /// Set the Horizontal and Vertical Alignment for Tooltip.
        /// </summary>
        /// <param name="mousePos">Current Position</param>
        /// <param name="tooltip">Tooltip instance</param>
        /// <returns></returns>
        private Point Position(Point mousePos, ref ChartTooltip tooltip)
        {
            var tooltipBehavior = ActualArea.TooltipBehavior;
            double horizontalOffset = ChartTooltip.GetActualHorizontalOffset(tooltipBehavior, ChartTooltip.GetHorizontalOffset(this));
            double verticalOffset = ChartTooltip.GetActualVerticalOffset(tooltipBehavior, ChartTooltip.GetVerticalOffset(this));

            var newPostion = mousePos;
            if ((tooltip).DesiredSize.Height == 0 || (tooltip).DesiredSize.Width == 0)
                (tooltip).UpdateLayout();

            HorizontalAlignment horizontalAlignment = ChartTooltip.GetActualHorizontalAlignment(tooltipBehavior, ChartTooltip.GetHorizontalAlignment(this));
            VerticalAlignment verticalAlignment = ChartTooltip.GetActualVerticalAlignment(tooltipBehavior, ChartTooltip.GetVerticalAlignment(this));

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    newPostion.X = mousePos.X - (tooltip).DesiredSize.Width - horizontalOffset;
                    break;
                case HorizontalAlignment.Center:
                    newPostion.X = mousePos.X - (tooltip).DesiredSize.Width / 2 + horizontalOffset;
                    break;
                case HorizontalAlignment.Right:
                    newPostion.X = mousePos.X + horizontalOffset;
                    break;
            }

            switch (verticalAlignment)
            {
                case VerticalAlignment.Top:
                    newPostion.Y = mousePos.Y - (tooltip).DesiredSize.Height - verticalOffset;
                    break;
                case VerticalAlignment.Center:
                    newPostion.Y = mousePos.Y - (tooltip).DesiredSize.Height / 2 - verticalOffset;
                    break;
                case VerticalAlignment.Bottom:
                    newPostion.Y = mousePos.Y + verticalOffset;
                    break;
            }

            return newPostion;
        }

        private void DataTableCleared(object sender, DataTableClearEventArgs e) 
        {
            Refresh();
        }

        private void ClearAdornments() 
        {
            Adornments.Clear();
            VisibleAdornments.Clear();
            if (adornmentInfo != null) {
                if (adornmentInfo.adormentContainers != null)
                    adornmentInfo.adormentContainers.Clear();
                if (adornmentInfo.ConnectorLines != null)
                    adornmentInfo.ConnectorLines.Clear();
                if (adornmentInfo.LabelPresenters != null)
                    adornmentInfo.LabelPresenters.Clear();
            }
        }

        private void MousePointerExit(ChartTooltipBehavior tooltipBehavior)
        {
            if (ActualTooltipPosition == TooltipPosition.Pointer || !Timer.IsEnabled) 
            {
                RemoveTooltip();
                Timer.Stop();
            }

            if (ChartTooltip.GetActualInitialShowDelay(tooltipBehavior, ChartTooltip.GetInitialShowDelay(this)) > 0)
            {
                InitialDelayTimer.Stop();
            }
        }

        private void AdjustTooltipAtEdge(ChartTooltip chartTooltip) 
        {
            if (chartTooltip.LeftOffset < this.ActualArea.SeriesClipRect.Left)
                chartTooltip.LeftOffset = this.ActualArea.SeriesClipRect.Left;
            else if (chartTooltip.LeftOffset + chartTooltip.DesiredSize.Width > this.ActualArea.SeriesClipRect.Right)
                chartTooltip.LeftOffset = this.ActualArea.SeriesClipRect.Right - chartTooltip.DesiredSize.Width;
            else
                chartTooltip.LeftOffset = chartTooltip.LeftOffset;
            if (chartTooltip.TopOffset < this.ActualArea.SeriesClipRect.Top)
                chartTooltip.TopOffset = this.ActualArea.SeriesClipRect.Top;
            else if (chartTooltip.TopOffset + chartTooltip.DesiredSize.Height > this.ActualArea.SeriesClipRect.Bottom)
                chartTooltip.TopOffset = this.ActualArea.SeriesClipRect.Bottom - chartTooltip.DesiredSize.Height;
            else
                chartTooltip.TopOffset = chartTooltip.TopOffset;
        }

        private Point SetTooltipMarkerPosition(Point tooltipPosition, ChartTooltip chartTooltip) 
        {
            if (adornmentInfo != null && adornmentInfo.ShowMarker && (adornmentInfo.GetAdornmentPosition() == BarLabelAlignment.Top))
                {
                if (adornmentInfo.MarkerType == ShapeType.Custom && adornmentInfo.MarkerTemplate != null) 
                    {
                    FrameworkElement symbolElement = adornmentInfo.MarkerTemplate.LoadContent() as Shape;

                    if (symbolElement != null) 
                    {
                        symbolElement.UpdateLayout();
                        var symbolHeight = symbolElement.ActualHeight;
                        var symbolWidth = symbolElement.ActualWidth;
                        tooltipPosition.Y = tooltipPosition.Y - symbolElement.ActualHeight / 2;
                        if (tooltipPosition.Y - chartTooltip.ActualHeight < ActualArea.SeriesClipRect.Top) 
                        {
                            tooltipPosition.Y += symbolElement.ActualHeight;
                        }
                    }
                }
                else if (adornmentInfo.MarkerType != ShapeType.Custom)
                {
                    tooltipPosition.Y = tooltipPosition.Y - adornmentInfo.MarkerHeight / 2;
                    if (tooltipPosition.Y - chartTooltip.ActualHeight < ActualArea.SeriesClipRect.Top) 
                    {
                        tooltipPosition.Y += adornmentInfo.MarkerHeight;
                    }
                }
            }

            return tooltipPosition;
        }

        #endregion
        #endregion
    }
 }