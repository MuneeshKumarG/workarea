using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using System.Data;
using System.Diagnostics;
using System.Collections.Specialized;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Windows.Foundation;
using System.ComponentModel;
using NativeColor = Windows.UI.Color;
using TimeSpan = System.TimeSpan;
using Shape = Microsoft.UI.Xaml.Shapes.Shape;
using ChartAdornmentInfoBase = Syncfusion.UI.Xaml.Charts.ChartDataLabelSettings;
using ChartAdornmentInfo = Syncfusion.UI.Xaml.Charts.ChartDataLabelSettings;
using ChartAdornment = Syncfusion.UI.Xaml.Charts.ChartDataLabel;
using AdornmentSeries = Syncfusion.UI.Xaml.Charts.DataMarkerSeries;
using ChartAdornmentContainer = Syncfusion.UI.Xaml.Charts.ChartDataMarkerContainer;
using ChartAdornmentPresenter = Syncfusion.UI.Xaml.Charts.ChartDataMarkerPresenter;
using StackingSeriesBase = Syncfusion.UI.Xaml.Charts.StackedSeriesBase;
using StackingColumn100Series = Syncfusion.UI.Xaml.Charts.StackedColumn100Series;
using StackingColumnSeries = Syncfusion.UI.Xaml.Charts.StackedColumnSeries;
using StackingLineSeries = Syncfusion.UI.Xaml.Charts.StackedLineSeries;
using StackingAreaSeries = Syncfusion.UI.Xaml.Charts.StackedAreaSeries;
using StackingBarSeries = Syncfusion.UI.Xaml.Charts.StackedBarSeries;
using SelectionStyle = Syncfusion.UI.Xaml.Charts.SelectionType;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Defines members and methods necessary to customize the display of selected segment in a <see cref="ChartSeriesBase"/>.
    /// </summary>
    /// <seealso cref="ChartSelectionBehavior"/>
    public interface ISegmentSelectable
    {
        #region Properites

        /// <summary>
        /// Gets or sets SelectionBrush property
        /// </summary>
        Brush SelectionBrush
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets SelectionIndex property
        /// </summary>
        int SelectedIndex
        {
            get;
            set;
        }

        #endregion
    }

    /// <summary>
    /// Defines members and methods necessary to customize segment in a <see cref="ChartSeriesBase"/>.
    /// </summary>
    /// <seealso cref="ChartSelectionBehavior"/>
    public interface ISegmentSpacing
    {
        #region Properties
        
        /// <summary>
        /// Gets or sets SegmentSpacing property
        /// </summary>
        double SegmentSpacing
        {
            get;
            set;
        }

        #endregion

        #region Methods

        ///<summary>
        /// Method used to calculate the segment spacing.
        ///</summary>
        /// <param name="spacing">Segment spacing value.</param>
        /// <param name="Right">Segment right value.</param>
        /// <param name="Left">Segment left value.</param>
        /// <returns>Returns the calculated segment space.</returns>
        double CalculateSegmentSpacing(double spacing, double Right, double Left);

        #endregion
    }

    /// <summary>
    /// Represents a base class for all the series types in chart.
    /// </summary>
    /// <remarks>
    /// Data points for ChartSeries would be populated <see cref="ChartSeriesBase"/> from <see cref="ChartSeriesBase.ItemsSource"/> property. Specify the binding paths for X-Values and Y-Values. The number of Y-Values may vary depending on the type of series. For e.g LineSeries requires only one y-value, whereas CandleSeries requires four y-values to plot a point.
    /// </remarks>
    /// <seealso cref="ChartSegment"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822: Mark members as static")]
    public abstract class ChartSeriesBase : Control
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <c>Spacing</c> dependency property. This is attached property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Spacing</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.RegisterAttached("Spacing", typeof(double), typeof(ChartSeriesBase),
                new PropertyMetadata(0.2d, OnSegmentSpacingChanged));

        /// <summary>
        /// Identifies the <see cref="TooltipTemplate"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>TooltipTemplate</c> dependency property.
        /// </value>
        public static readonly DependencyProperty TooltipTemplateProperty =
            DependencyProperty.Register(nameof(TooltipTemplate), typeof(DataTemplate), typeof(ChartSeriesBase),
                new PropertyMetadata(null, new PropertyChangedCallback(OnTooltipTemplateChanged)));

        /// <summary>
        /// Identifies the <see cref="ShowTooltip"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ShowTooltip</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ShowTooltipProperty =
            DependencyProperty.Register(nameof(ShowTooltip), typeof(bool), typeof(ChartSeriesBase),
                new PropertyMetadata(false, new PropertyChangedCallback(OnShowTooltipChanged)));

        /// <summary>
        /// Identifies the <see cref="ListenPropertyChange"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ListenPropertyChange</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ListenPropertyChangeProperty =
            DependencyProperty.Register(nameof(ListenPropertyChange), typeof(bool), typeof(ChartSeriesBase),
                new PropertyMetadata(false, new PropertyChangedCallback(OnListenPropertyChangeChanged)));

        /// <summary>
        /// Identifies the <see cref="IsSeriesVisible"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>IsSeriesVisible</c> dependency property.
        /// </value>
        public static readonly DependencyProperty IsSeriesVisibleProperty =
            DependencyProperty.Register(nameof(IsSeriesVisible), typeof(bool), typeof(ChartSeriesBase),
                new PropertyMetadata(true, OnIsSeriesVisibleChanged));

        /// <summary>
        /// Identifies the <see cref="XBindingPath"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>XBindingPath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty XBindingPathProperty =
        DependencyProperty.Register(nameof(XBindingPath), typeof(string), typeof(ChartSeriesBase),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnBindingPathXChanged)));

        /// <summary>
        /// Identifies the <see cref="SortBy"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>SortBy</c> dependency property.
        /// </value>
        internal static readonly DependencyProperty SortByProperty =
            DependencyProperty.Register(nameof(SortBy), typeof(SortingAxis), typeof(ChartSeriesBase),
                new PropertyMetadata(SortingAxis.X, new PropertyChangedCallback(OnSortDataOrderChanged)));

        /// <summary>
        /// Identifies the <see cref="SortDirection"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>SortDirection</c> dependency property.
        /// </value>
        internal static readonly DependencyProperty SortDirectionProperty =
            DependencyProperty.Register(nameof(SortDirection), typeof(Direction), typeof(ChartSeriesBase),
                new PropertyMetadata(Direction.Ascending, new PropertyChangedCallback(OnSortDataOrderChanged)));

        /// <summary>
        /// Identifies the <see cref="IsSortData"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>IsSortData</c> dependency property.
        /// </value>
        internal static readonly DependencyProperty IsSortDataProperty =
            DependencyProperty.Register(nameof(IsSortData), typeof(bool), typeof(ChartSeriesBase),
                new PropertyMetadata(false, new PropertyChangedCallback(OnSortDataOrderChanged)));

        /// <summary>
        /// Identifies the <see cref="Palette"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Palette</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PaletteProperty =
            DependencyProperty.Register(nameof(Palette), typeof(ChartColorPalette), typeof(ChartSeriesBase),
                new PropertyMetadata(ChartColorPalette.None, OnPaletteChanged));

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ItemsSource</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(ChartSeriesBase),
            new PropertyMetadata(null, new PropertyChangedCallback(OnDataSourceChanged)));

        /// <summary>
        /// Identifies the <see cref="TrackballLabelTemplate"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>TrackballLabelTemplate</c> dependency property.
        /// </value>
        public static readonly DependencyProperty TrackballLabelTemplateProperty =
            DependencyProperty.Register(nameof(TrackballLabelTemplate), typeof(DataTemplate), typeof(ChartSeriesBase), null);

        /// <summary>
        /// Identifies the <see cref="Interior"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Interior</c> dependency property.
        /// </value>
        public static readonly DependencyProperty InteriorProperty =
            DependencyProperty.Register(nameof(Interior), typeof(Brush), typeof(ChartSeriesBase),
                new PropertyMetadata(null, OnAppearanceChanged));

        /// <summary>
        /// Identifies the <see cref="Label"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Label</c> dependency property.
        /// </value>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(ChartSeriesBase), new PropertyMetadata(string.Empty, OnLabelPropertyChanged));

        /// <summary>
        /// Identifies the <see cref="LegendIcon"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>LegendIcon</c> dependency property.
        /// </value>
        public static readonly DependencyProperty LegendIconProperty =
            DependencyProperty.Register(nameof(LegendIcon), typeof(ChartLegendIcon), typeof(ChartSeriesBase),
                new PropertyMetadata(ChartLegendIcon.SeriesType, OnLegendIconChanged));

        /// <summary>
        /// Identifies the <see cref="LegendIconTemplate"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>LegendIconTemplate</c> dependency property.
        /// </value>
        public static readonly DependencyProperty LegendIconTemplateProperty =
            DependencyProperty.Register(nameof(LegendIconTemplate), typeof(DataTemplate), typeof(ChartSeriesBase), new PropertyMetadata(null, OnLegendIconTemplateChanged));

        /// <summary>
        /// Identifies the <see cref="VisibilityOnLegend"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>VisibilityOnLegend</c> dependency property.
        /// </value>
        public static readonly DependencyProperty VisibilityOnLegendProperty =
            DependencyProperty.Register(nameof(VisibilityOnLegend), typeof(Visibility), typeof(ChartSeriesBase),
                new PropertyMetadata(Visibility.Visible, new PropertyChangedCallback(OnVisibilityOnLegendChanged)));

        /// <summary>
        /// Identifies the <see cref="ColorModel"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ColorModel</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ColorModelProperty =
            DependencyProperty.Register(nameof(ColorModel), typeof(ChartColorModel), typeof(ChartSeriesBase),
                new PropertyMetadata(null, OnColorModelChanged));

        /// <summary>
        /// Identifies the <see cref="SegmentColorPath"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>SegmentColorPath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SegmentColorPathProperty =
            DependencyProperty.Register(nameof(SegmentColorPath), typeof(string), typeof(ChartSeriesBase),
                   new PropertyMetadata(null, new PropertyChangedCallback(OnSegmentColorPathChanged)));

        /// <summary>
        /// Identifies the <see cref="EnableAnimation"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>EnableAnimation</c> dependency property.
        /// </value>
        public static readonly DependencyProperty EnableAnimationProperty =
            DependencyProperty.Register(nameof(EnableAnimation), typeof(bool), typeof(ChartSeriesBase), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="AnimationDuration"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>AnimationDuration</c> dependency property.
        /// </value>
        public static readonly DependencyProperty AnimationDurationProperty =
            DependencyProperty.Register(nameof(AnimationDuration), typeof(TimeSpan), typeof(ChartSeriesBase),
                new PropertyMetadata(TimeSpan.FromSeconds(0.8)));

        /// <summary>
        /// Identifies the <see cref="EmptyPointValue"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>EmptyPointValue</c> dependency property.
        /// </value>
        internal static readonly DependencyProperty EmptyPointValueProperty =
            DependencyProperty.Register(nameof(EmptyPointValue), typeof(EmptyPointValue), typeof(ChartSeriesBase),
                new PropertyMetadata(EmptyPointValue.Zero, new PropertyChangedCallback(OnEmptyPointValueChanged)));

        /// <summary>
        /// Identifies the <see cref="EmptyPointStyle"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>EmptyPointStyle</c> dependency property.
        /// </value>
        internal static readonly DependencyProperty EmptyPointStyleProperty =
            DependencyProperty.Register(nameof(EmptyPointStyle), typeof(EmptyPointStyle), typeof(ChartSeriesBase),
                new PropertyMetadata(EmptyPointStyle.Interior, new PropertyChangedCallback(OnEmptyPointStyleChanged)));

        /// <summary>
        /// Identifies the <see cref="EmptyPointSymbolTemplate"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>EmptyPointSymbolTemplate</c> dependency property.
        /// </value>
        internal static readonly DependencyProperty EmptyPointSymbolTemplateProperty =
            DependencyProperty.Register(nameof(EmptyPointSymbolTemplate), typeof(DataTemplate), typeof(ChartSeriesBase), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ShowEmptyPoints"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ShowEmptyPoints</c> dependency property.
        /// </value>
        internal static readonly DependencyProperty ShowEmptyPointsProperty =
            DependencyProperty.Register(nameof(ShowEmptyPoints), typeof(bool), typeof(ChartSeriesBase),
                new PropertyMetadata(false, OnShowEmptyPointsChanged));

        /// <summary>
        /// Identifies the <see cref="EmptyPointInterior"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>EmptyPointInterior</c> dependency property.
        /// </value>
        internal static readonly DependencyProperty EmptyPointInteriorProperty =
            DependencyProperty.Register(nameof(EmptyPointInterior), typeof(Brush), typeof(ChartSeriesBase),
                new PropertyMetadata(new SolidColorBrush(Colors.Green), OnEmptyPointInteriorChanged));

        /// <summary>
        /// Identifies the <see cref="ActualTrackballLabelTemplate"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ActualTrackballLabelTemplate</c> dependency property.
        /// </value>
        internal static readonly DependencyProperty ActualTrackballLabelTemplateProperty =
            DependencyProperty.Register(nameof(ActualTrackballLabelTemplate), typeof(DataTemplate), typeof(ChartSeriesBase), null);

        #endregion

        #region Fields

        #region Internal Fields

        internal readonly Stopwatch _stopwatch = new Stopwatch();

        internal List<int>[] EmptyPointIndexes;

        internal int SeriesYCount;

        internal bool totalCalculated;

        internal List<Rect> bitmapRects = new List<Rect>();

        internal bool IsStacked100;

        internal List<int> bitmapPixels = new List<int>();

        internal ChartDataPointInfo dataPoint; internal ChartSelectionChangingEventArgs selectionChangingEventArgs = new ChartSelectionChangingEventArgs();

        internal ChartSelectionChangedEventArgs selectionChangedEventArgs = new ChartSelectionChangedEventArgs();

        internal ChartAdornmentInfoBase adornmentInfo;

        internal List<double> GroupedXValuesIndexes = new List<double>();

        internal List<string> GroupedXValues = new List<string>();

        internal Dictionary<double, List<int>> DistinctValuesIndexes = new Dictionary<double, List<int>>();

        internal List<object> GroupedActualData = new List<object>();

        internal int PreviousSelectedIndex = -1;

        internal List<int> ToggledLegendIndex;

        internal string[] XComplexPaths;

        internal string[][] YComplexPaths;

        internal List<ChartSegment> selectedSegments = new List<ChartSegment>();

        internal ChartSeriesPanel SeriesPanel;

        internal bool IsPointGenerated;

        internal bool IsComplexYProperty;

        internal bool triggerSelectionChangedEventOnLoad;

        internal int UpdateStartedIndex = -1;

        internal bool isPointValidated;

        internal bool isLinearData = true;

        internal double XData;

        internal bool canAnimate;

        internal DoubleAnimation scaleXAnimation;

        internal DoubleAnimation scaleYAnimation;

        internal Storyboard storyBoard;

        internal Point mousePos;

        internal Point previousMousePosition;

        #endregion

        #region Protected Internal Fields

        /// <summary>
        /// segments variable declarations
        /// </summary>
        protected internal ObservableCollection<ChartSegment> Segments;

        /// <summary>
        /// Timer to handle the show duration of the tooltip.
        /// </summary>
        protected internal DispatcherTimer Timer;

        /// <summary>
        /// Timer to handle the initial delay of showing the tooltip.
        /// </summary>
        protected internal DispatcherTimer InitialDelayTimer;

        #endregion

        #region Protected Fields

        /// <summary>
        /// ChartTransformer variable declarations
        /// </summary>
        protected IChartTransformer ChartTransformer;

        /// <summary>
        /// YPaths variable declarations
        /// </summary>
        protected string[] YPaths;

        #endregion

        #region Private Fields

        private double grandTotal = 0d;

        private ChartValueType xValueType;

        private ObservableCollection<int> _selectedSegmentsIndexes;

        private ObservableCollection<ChartAdornment> m_adornments = new ObservableCollection<ChartAdornment>();

        private ObservableCollection<ChartAdornment> m_visibleAdornments = new ObservableCollection<ChartAdornment>();

        private bool isActualTransposed;

        private const int seriesTipHeight = 6;

#if NETFX_CORE
        bool isTap;
#endif

        DataTemplate toolTipTemplate;

        private int dataCount;

        private bool isNotificationSuspended;

        private bool isPropertyNotificationSuspended;

        private bool isUpdateStarted;

        private DataTemplate defaultTooltipTemplate;

        private double YData;

        private bool isRepeatPoint;

        private bool isSegmentColorChanged;

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Called when instance created for ChartSeries
        /// </summary>
        public ChartSeriesBase()
        {
            DefaultStyleKey = typeof(ChartSeriesBase);
            Segments = new ObservableCollection<ChartSegment>();
            SelectedSegmentsIndexes = new ObservableCollection<int>();
            ToggledLegendIndex = new List<int>();
            EmptyPointInterior = new SolidColorBrush(Colors.Green);
            ColorModel = new ChartColorModel(this.Palette);
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

        #region Delegates

        /// <summary>
        /// References to method that reflects the value from the object
        /// </summary>
        /// <param name="obj">Current object</param>
        /// <param name="paths">Path name</param>
        /// <returns></returns>
        internal delegate object GetReflectedProperty(object obj, string[] paths);

        #endregion

        #region Events

        /// <summary>
        /// Event correspond to property value changed. It invokes when the <see cref="ChartSeriesBase"/> properties changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that determines how to calculate value for empty point.
        /// </summary>
        internal EmptyPointValue EmptyPointValue
        {
            get { return (EmptyPointValue)GetValue(EmptyPointValueProperty); }
            set { SetValue(EmptyPointValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets EmptyPointStyle for an empty point. It determines how to differentiate empty point from other data points.
        /// </summary>
        internal EmptyPointStyle EmptyPointStyle
        {
            get { return (EmptyPointStyle)GetValue(EmptyPointStyleProperty); }
            set { SetValue(EmptyPointStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets DataTemplate to be used when EmptyPointStyle is set to Symbol/ SymbolAndInterior. 
        /// By default, an ellipse will be displayed as symbol.
        /// </summary>
        /// <value>
        /// <see cref="DataTemplate"/>
        /// </value>
        internal DataTemplate EmptyPointSymbolTemplate
        {
            get { return (DataTemplate)GetValue(EmptyPointSymbolTemplateProperty); }
            set { SetValue(EmptyPointSymbolTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show empty points.
        /// </summary>
        internal bool ShowEmptyPoints
        {
            get { return (bool)GetValue(ShowEmptyPointsProperty); }
            set { SetValue(ShowEmptyPointsProperty, value); }
        }

        /// <summary>
        /// Gets or sets interior color for empty point.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        internal Brush EmptyPointInterior
        {
            get { return (Brush)GetValue(EmptyPointInteriorProperty); }
            set { SetValue(EmptyPointInteriorProperty, value); }
        }

        /// <summary>
        /// Gets the number of points given as input.
        /// </summary>
        public int DataCount
        {
            get
            {
                return dataCount;
            }

            internal set
            {
                dataCount = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to sort the datas.
        /// </summary>
        internal bool IsSortData
        {
            get { return (bool)GetValue(IsSortDataProperty); }
            set { SetValue(IsSortDataProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Sorting Direction.
        /// </summary>
        internal Direction SortDirection
        {
            get { return (Direction)GetValue(SortDirectionProperty); }
            set { SetValue(SortDirectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets SortingAxis.
        /// </summary>
        internal SortingAxis SortBy
        {
            get { return (SortingAxis)GetValue(SortByProperty); }
            set { SetValue(SortByProperty, value); }
        }

        /// <summary>
        /// Gets or sets the custom template for Tooltip.
        /// </summary>
        /// <value>
        /// This accepts a <see cref="DataTemplate"/>.
        /// </value>
        /// <value>
        /// </value>
        /// <example>
        /// <code language = "XAML">
        ///   &lt;syncfusion:ColumnSeries ShowTooltip="True" ItemsSource="{Binding Demands} syncfusion:ChartTooltip.VerticalAlignment="Top" XBindingPath="Demand" YBindingPath="Year2010"&gt;
        ///      &lt;syncfusion:ColumnSeries.TooltipTemplate&gt;
        ///        &lt;DataTemplate&gt;
        ///            &lt;Border Background = "DarkGreen" CornerRadius="5" BorderThickness="2" BorderBrush="Black" Width="50" Height="30"&gt;
        ///                &lt;TextBlock Text = "{Binding Item.Year2010}" Foreground="White" FontWeight="Bold"  HorizontalAlignment="Center" VerticalAlignment="Center"/&gt;
        ///            &lt;/Border&gt;
        ///        &lt;/DataTemplate&gt;
        ///      &lt;/syncfusion:ColumnSeries.TooltipTemplate&gt;
        ///   &lt;/syncfusion:ColumnSeries&gt;
        /// </code>
        /// </example>
        public DataTemplate TooltipTemplate
        {
            get { return (DataTemplate)GetValue(TooltipTemplateProperty); }
            set { SetValue(TooltipTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show or hide the tooltip for series.
        /// </summary>
        public bool ShowTooltip
        {
            get { return (bool)GetValue(ShowTooltipProperty); }
            set { SetValue(ShowTooltipProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether to listen property change or not. This is a bindable property.
        /// </summary>
        public bool ListenPropertyChange
        {
            get { return (bool)GetValue(ListenPropertyChangeProperty); }
            set { SetValue(ListenPropertyChangeProperty, value); }
        }

        /// <summary>
        /// Gets the adornments collection.
        /// </summary>
        /// <value>The adornments.</value>
        internal ObservableCollection<ChartAdornment> Adornments
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
        internal ObservableCollection<ChartAdornment> VisibleAdornments
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

        /// <summary>
        /// Gets or sets a value indicating whether is series visible.
        /// </summary>
        public bool IsSeriesVisible
        {
            get { return (bool)GetValue(IsSeriesVisibleProperty); }
            set { SetValue(IsSeriesVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets ChartPalette for series.
        /// </summary>
        public ChartColorPalette Palette
        {
            get { return (ChartColorPalette)GetValue(PaletteProperty); }
            set { SetValue(PaletteProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of data points used to generate Chart.
        /// </summary>
        /// <value>The ItemsSource value.</value>
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
        /// Gets or sets DataTemplate used to display label, when ChartTrackballBehavior is used.
        /// </summary>
        /// <value>
        /// <see cref="DataTemplate"/>
        /// </value>
        /// <example>
        /// <code language = "XAML">
        ///   &lt;syncfusion:ColumnSeries ItemsSource="{Binding Demands} XBindingPath="Demand" YBindingPath="Year2010"&gt;
        ///      &lt;syncfusion:ColumnSeries.TrackBallLabelTemplate&gt;
        ///        &lt;DataTemplate&gt;
        ///            &lt;Border Background = "DarkGreen" CornerRadius="5" BorderThickness="2" BorderBrush="Black" Width="50" Height="30"&gt;
        ///                &lt;TextBlock Text = "{Binding Item.Year2010}" Foreground="White" FontWeight="Bold"  HorizontalAlignment="Center" VerticalAlignment="Center"/&gt;
        ///            &lt;/Border&gt;
        ///        &lt;/DataTemplate&gt;
        ///      &lt;/syncfusion:ColumnSeries.TrackBallLabelTemplate&gt;
        ///   &lt;/syncfusion:ColumnSeries&gt;
        /// </code>
        /// </example>
        public DataTemplate TrackballLabelTemplate
        {
            get { return (DataTemplate)GetValue(TrackballLabelTemplateProperty); }
            set { SetValue(TrackballLabelTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush to paint the interior of the series.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush Interior
        {
            get { return (Brush)GetValue(InteriorProperty); }
            set { SetValue(InteriorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the label that will be displayed in the associated legend item.
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets ChartLegendIcon to be displayed in associated legend item.
        /// </summary>
        /// <value>
        /// <see cref="ChartLegendIcon"/>
        /// </value>
        public ChartLegendIcon LegendIcon
        {
            get { return (ChartLegendIcon)GetValue(LegendIconProperty); }
            set { SetValue(LegendIconProperty, value); }
        }

        /// <summary>
        /// Gets or sets DataTemplate for legend icon.
        /// </summary>
        /// <value>
        /// <see cref="DataTemplate"/>
        /// </value>
        public DataTemplate LegendIconTemplate
        {
            get { return (DataTemplate)GetValue(LegendIconTemplateProperty); }
            set { SetValue(LegendIconTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that determines whether to create a legend item for this series. 
        /// By default, legend item will be visible for this series.
        /// </summary>
        /// <value>
        /// <see cref="Visibility"/>
        /// </value>
        public Visibility VisibilityOnLegend
        {
            get { return (Visibility)GetValue(VisibilityOnLegendProperty); }
            set { SetValue(VisibilityOnLegendProperty, value); }
        }


        /// <summary>
        /// Gets or sets the color models for all series in the chart.
        /// </summary>
        /// <value>
        /// <see cref="ChartColorModel"/>
        /// </value>
        public ChartColorModel ColorModel
        {
            get { return (ChartColorModel)GetValue(ColorModelProperty); }
            set { SetValue(ColorModelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the property path of x-data in ItemsSource to render the chart series. This is a bindable property.
        /// </summary>
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
        /// Gets or sets the property binding path for segment color. 
        /// </summary>
        public string SegmentColorPath
        {
            get
            {
                return (string)GetValue(SegmentColorPathProperty);
            }

            set
            {
                SetValue(SegmentColorPathProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to animate the chart series on loading and whenever ItemsSource change.
        /// </summary>
        public bool EnableAnimation
        {
            get { return (bool)GetValue(EnableAnimationProperty); }
            set { SetValue(EnableAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the duration of the animation.
        /// </summary>
        /// <value>
        /// <see cref="TimeSpan"/>
        /// </value>
        /// <example>
        /// <code language = "XAML">
        ///   &lt;syncfusion:ColumnSeries ItemsSource="{Binding Demands} AnimationDuration="00:00:03" XBindingPath="Demand" YBindingPath="Year2010"&gt;
        ///   &lt;/syncfusion:ColumnSeries&gt;
        /// </code>
        /// </example>
        public TimeSpan AnimationDuration
        {
            get { return (TimeSpan)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }

        #endregion

        #region Internal Properties
        
        internal ChartBase ActualArea { get; set; }

        internal bool IsSingleAccumulationSeries
        {
            get
            {
                return ActualArea != null && ActualArea.ActualSeries.Count == 1 &&
                   ActualArea.ActualSeries[0] is AccumulationSeriesBase;
            }
        }

        internal Panel SeriesRootPanel { get; set; }

        internal DoubleRange SideBySideInfoRangePad { get; set; }

        internal HashSet<int> Pixels { get; set; }

        internal IList<double>[] GroupedSeriesYValues { get; set; }

        internal bool IsActualTransposed
        {
            get
            {
                return isActualTransposed;
            }

            set
            {
                isActualTransposed = value;
                OnActualTransposeChanged();
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

        internal ChartAdornmentPresenter AdornmentPresenter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the x values in an unsorted order or in the order the data has been added to series.
        /// </summary>
        internal IEnumerable XValues
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the y values in an unsorted order or in the order the data has been added to series.
        /// </summary>
        internal IList<double>[] SeriesYValues
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sorted values, if the IsSortData is true.
        /// </summary>
        internal IList<double>[] ActualSeriesYValues
        {
            get;
            set;
        }

        internal List<object> ActualData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether multipleYValues is needed,will be set internally.
        /// </summary>
        internal virtual bool IsMultipleYPathRequired
        {
            get
            {
                return false;
            }
        }

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

        internal ChartValueType XAxisValueType
        {
            get
            {
                return xValueType;
            }

            set
            {
                xValueType = value;
            }
        }
        
        /// <summary>
        /// Gets or sets the nearest segment index corresponding to the mouse point when interactive behaviors used.
        /// Note: This get's updated only when FindNearestChartPoint() method is called for series.
        /// </summary>
        internal int NearestSegmentIndex { get; set; }

        internal object ToolTipTag { get; set; }

        internal DataTemplate OutlierTooltipTemplate { get; set; }

        internal bool IsItemsSourceChanged { get; set; }

        #endregion

        #region Prortected Internal Virtual Properties

        /// <summary>
        /// Gets a value indicating whether this series is placed side by side.
        /// </summary>
        /// <returns>
        /// It returns <c>true</c>, if the series is placed side by side [cluster mode].
        /// </returns>
        /// <value>
        /// Returns the bool value.</value>
        internal virtual bool IsSideBySide
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the value which confirms whether this series in linearity.
        /// </summary>
        /// <remarks>
        ///  Returns <c>true</c> if its linear, otherwise it returns <c>false</c>.
        /// </remarks>
        /// <value>
        /// Returns the bool value.</value>
        internal virtual bool IsLinear
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the value which confirms whether it is area typed series.
        /// </summary>
        /// <remarks>
        ///  Returns <c>true</c> if its linear, otherwise it returns <c>false</c>.
        /// </remarks>
        internal virtual bool IsAreaTypeSeries
        {
            get
            {
                return false;
            }
        }

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

        #endregion

        #region Protected Internal Properties

        /// <summary>
        /// This indicates whether its a colorpath series or not.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        protected internal bool IsColorPathSeries
        {
            get
            {
                return (!IsAreaTypeSeries && !(this is PolarRadarSeriesBase && (this as PolarRadarSeriesBase).DrawType == ChartSeriesDrawType.Area) && !(this is FinancialSeriesBase) && !(this is FastLineSeries));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to treat x values as categories. 
        /// </summary>
        protected internal bool IsIndexed
        {
            get { return this.ActualXAxis is CategoryAxis || this.ActualXAxis is DateTimeCategoryAxis; }
        }

        /// <summary>
        /// Gets or sets a series color values.
        /// </summary>
        protected internal IList<Brush> ColorValues
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sorted values, if the IsSortData is true.
        /// </summary>
        protected internal IEnumerable ActualXValues
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selected segments indexes.
        /// </summary>
        protected internal ObservableCollection<int> SelectedSegmentsIndexes
        {
            get
            {
                return _selectedSegmentsIndexes;
            }

            set
            {
                if (SelectedSegmentsIndexes != null)
                    SelectedSegmentsIndexes.CollectionChanged -= SelectedSegmentsIndexes_CollectionChanged;
                _selectedSegmentsIndexes = value;
                if (_selectedSegmentsIndexes != null)
                    _selectedSegmentsIndexes.CollectionChanged += SelectedSegmentsIndexes_CollectionChanged;
            }
        }

        /// <summary>
        /// Gets the selected segments in this series, when we enable the multiple selection.
        /// </summary>
        /// <returns>
        /// It returns list of <see cref="ChartSegment"/>.
        /// </returns>
        protected internal virtual List<ChartSegment> SelectedSegments
        {
            get
            {
                if (SelectedSegmentsIndexes.Count > 0)
                {
                    return (from index in SelectedSegmentsIndexes
                            where index < Segments.Count
                            select Segments[index]).ToList();
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Returns the corresponding series selected segment.
        /// </summary>
        protected internal virtual ChartSegment SelectedSegment
        {
            get
            {
                var segmentSelectableSeries = this as ISegmentSelectable;

                if (segmentSelectableSeries != null)
                {
                    int index = segmentSelectableSeries.SelectedIndex;
                    if (index < Segments.Count && index > 0)
                        return Segments[index];
                    else
                        return null;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets actual series X-axis.
        /// </summary>
        /// <remarks>
        /// Gets actual XAxis for series with respect to chart type and <see cref="IsActualTransposed"/> value.
        /// </remarks>
        protected internal ChartAxis ActualXAxis
        {
            get
            {
                if (ActualArea != null && this is ISupportAxes)
                {
                    return (this as ISupportAxes2D).XAxis
                        ?? ActualArea.InternalPrimaryAxis;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets actual series Y-axis.
        /// </summary>
        protected internal ChartAxis ActualYAxis
        {
            get
            {
                if (ActualArea != null && this is ISupportAxes)
                {
                    return (this as ISupportAxes2D).YAxis
                        ?? ActualArea.InternalSecondaryAxis;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region Protected Virtual Properties

        /// <summary>
        /// Gets a value indicating whether this series is a stacked series.
        /// </summary>
        /// <value><c>true</c> if the series is one of stacked series; otherwise, <c>false</c>. The default value is <c>true</c>.</value>
        protected virtual bool IsStacked
        {
            get { return false; }
        }

        #endregion

        #endregion

        #region Methods


        #region Public Static Methods

        /// <summary>
        /// Gets the Spacing for the SideBySide segments.
        /// </summary>
        /// <param name="obj">ChartSeries object</param>
        /// <returns>returns a double value.</returns>
        internal static double GetSpacing(DependencyObject obj)
        {
            return (double)obj.GetValue(SpacingProperty);
        }

        /// <summary>
        /// Sets the Spacing for the SideBySide segments.
        /// </summary>
        /// <param name="obj">ChartSeries object</param>
        /// <param name="value">The value to set for calcaulting the segment width</param>
        internal static void SetSpacing(DependencyObject obj, double value)
        {
            obj.SetValue(SpacingProperty, value);
        }

#endregion

#region Public Virtual Methods
       
        /// <summary>
        /// Finds the nearest point in ChartSeries relative to the mouse point/touch position.
        /// </summary>
        /// <param name="point">The co-ordinate point representing the current mouse point /touch position.</param>
        /// <param name="x">x-value of the nearest point.</param>
        /// <param name="y">y-value of the nearest point</param>
        /// <param name="stackedYValue"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
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
                        if (!(this is StackingColumn100Series) && ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed
                            && !(this is WaterfallSeries) && !(this is ErrorBarSeries))
                        {
                            var count = (this is RangeSeriesBase) ? (this as RangeSeriesBase).GroupedXValues.Count :
                            (this is FinancialSeriesBase) ? (this as FinancialSeriesBase).GroupedXValues.Count :
                            GroupedXValues.Count;
                            var range1 = DistinctValuesIndexes[range];
                            int index = (int)range;
                            if (range1.Count > 0)
                                index = range1[0];
                            else
                            {
                                string xvalue = GroupedXValues[(int)range];

                                foreach(var series in ActualArea.VisibleSeries)
                                {
                                    index = ( series.ActualXValues as List<string>).IndexOf(xvalue);

                                    if (index > -1)
                                        break;
                                }
                            }
                            if (range <= xEnd && range >= xStart && range < count && range >= 0)
                            {
                                y = (this is RangeSeriesBase) ? (this as RangeSeriesBase).GroupedSeriesYValues[0][(int)index] :
                                    (this is FinancialSeriesBase) ? (this as FinancialSeriesBase).GroupedSeriesYValues[0][(int)index]
                                    : GroupedSeriesYValues[0][(int)index];
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
                IList<double> xValues = this.ActualXValues as IList<double>;
                var yValues = this.ActualSeriesYValues[0];
                nearPoint.X = ActualXAxis.VisibleRange.Start;

                if (IsSideBySide)
                {
                    DoubleRange sbsInfo = this.GetSideBySideInfo(this);
                    nearPoint.X = ActualXAxis.VisibleRange.Start + sbsInfo.Start;
                }

                nearPoint.Y = ActualYAxis.VisibleRange.Start;
                chartPoint = new ChartPoint(ActualArea.ActualPointToValue(ActualXAxis, point), ActualArea.ActualPointToValue(ActualYAxis, point));
                var logAxis = ActualXAxis as LogarithmicAxis;
                int index = 0;
                double xValue = logAxis != null ? Math.Log(chartPoint.X, logAxis.LogarithmicBase) : chartPoint.X;

                foreach(var x1 in xValues)                
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

        /// <summary>
        /// Method called when creating emptypoint segments for series.
        /// </summary>
        /// <param name="YValues">YValues</param>
        /// <param name="yValList">YValues list</param>
        /// <param name="xValList">XValues list</param>
        internal virtual void CreateEmptyPointSegments(IList<double> YValues, out List<List<double>> yValList, out List<List<double>> xValList)
        {
            IList<double> xValues;
            if (this.ActualXAxis is CategoryAxis && (!(this.ActualXAxis as CategoryAxis).IsIndexed))
                xValues = GroupedXValuesIndexes;
            else
                xValues = (ActualXValues is IList<double> && !IsIndexed) ? ActualXValues as IList<double> : GetXValues();
            var xSubList = new List<double>();
            var ySubList = new List<double>();
            yValList = new List<List<double>>();
            xValList = new List<List<double>>();

            // EmptyPoint calculation
            for (var i = 0; i < YValues.Count; i++)
            {
                if (double.IsNaN(YValues[i]))
                {
                    if (ySubList.Count > 0)
                        yValList.Add(ySubList);

                    ySubList = new List<double>();

                    if (xSubList.Count > 0)
                        xValList.Add(xSubList);

                    xSubList = new List<double>();

                    // We have created seperate segment for each empty point.
                    // If we create a single segment for group of empty points means 
                    // we need to recreate the segments when we change single empty point.
                    xValList.Add(new List<double>() { xValues[i] });
                    yValList.Add(new List<double>() { YValues[i] });
                }
                else
                {
                    ySubList.Add(YValues[i]);
                    xSubList.Add(((List<double>)xValues)[i]);
                }
            }

            if (ySubList.Count > 0)
                yValList.Add(ySubList);
            if (xSubList.Count > 0)
                xValList.Add(xSubList);
        }
        /// <summary>
        /// An abstract method which will called over each time in its child class to update an segment.
        /// </summary>
        /// <param name="index">The index of the segment</param>
        /// <param name="action">The collection changed action which raises the notification</param>
        public virtual void UpdateSegments(int index, NotifyCollectionChangedAction action)
        {
            UpdateArea();
        }

#endregion

#region Public Methods

        /// <summary>
        /// Gets the available size of Chart.
        /// </summary>
        /// <returns>returns size</returns>
        internal Size GetAvailableSize()
        {
            var availableSize = ActualArea == null ? new Size() : ActualArea.AvailableSize;
            return availableSize;
        }


        /// <summary>
        /// Suspends the series from updating the series data till ResumeNotification is called. 
        /// This is specifically used when we need to append collection of datas.
        /// </summary>        
        public void SuspendNotification()
        {
            isNotificationSuspended = true;
        }

        /// <summary>
        /// Processes the data that is added to data source after SuspendNotification.
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

                if (IsSortData)
                {
                    SortActualPoints();
                }

                this.UpdateArea();
            }
            else if (isNotificationSuspended)
            {
                if (!isUpdateStarted || UpdateStartedIndex < 0)
                {
                    UpdateArea();
                    isNotificationSuspended = false;
                    return;
                }

                if (YPaths != null && ActualSeriesYValues != null && ItemsSource != null)
                {
                    GeneratePoints(YPaths, ActualSeriesYValues);
                    UpdateArea();
                }

                isUpdateStarted = false;
                UpdateStartedIndex = -1;
            }

            isNotificationSuspended = false;
        }

        /// <summary>
        /// Invalidates the Series. 
        /// </summary>
        public void Invalidate()
        {
            CalculateSegments();
        }

        /// <summary>
        /// Returns the value of side by side position for a series.
        /// </summary>
        /// <param name="currentseries">ChartSeries.</param>
        /// <returns>The DoubleRange side by side Info</returns>
        public DoubleRange GetSideBySideInfo(ChartSeriesBase currentseries)
        {
            if (ActualArea != null)
            {
                if (this.ActualArea.InternalPrimaryAxis == null || this.ActualArea.InternalSecondaryAxis == null)
                    return DoubleRange.Empty;

                if (!this.ActualArea.SBSInfoCalculated || !this.ActualArea.SeriesPosition.ContainsKey(currentseries))
                    CalculateSideBySidePositions(true);
                double width = 1 - ChartSeriesBase.GetSpacing(this);
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
                if (all == 0)
                {
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

        /// <summary>
        /// An abstract method which will be called over to create segments.
        /// </summary>
        public abstract void CreateSegments();

#endregion

#region Internal Static Methods

        internal static List<double> Clone(IList<double> Values)
        {
            List<double> newValues = new List<double>();
            newValues.AddRange(Values);
            return newValues;
        }

        internal static ChartValueType GetDataType(IPropertyAccessor propertyAccessor, IEnumerable itemsSource)
        {
            if (itemsSource == null) return ChartValueType.Double;
            var enumerator = itemsSource.GetEnumerator();
            object obj = null;
            if (enumerator.MoveNext())
            {
                do
                {
                    obj = propertyAccessor.GetValue(enumerator.Current);
                }
                while (enumerator.MoveNext() && obj == null);
            }

            return GetDataType(obj);
        }

        /// <summary>
        /// Method implementation to set the updated data to the current object
        /// </summary>
        /// <param name="obj">Current object</param>
        /// <param name="paths">XComplexPaths</param>
        /// <param name="data">updated value</param>
        internal static void SetPropertyValue(object obj, string[] paths, object data)
        {
            object parentObj = obj;
            IPropertyAccessor propertyAccessor = null;
            for (int i = 0; i < paths.Length; i++)
            {
                var propertyInfo = ChartDataUtils.GetPropertyInfo(parentObj, paths[i]);
                if (propertyInfo != null)
                    propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
                if (propertyAccessor == null) return;
                if (i == paths.Length - 1)
                    switch (propertyInfo.PropertyType.Name)
                    {
                        case "Int16":
                            propertyAccessor.SetValue(parentObj, Convert.ToInt16(data));
                            break;

                        case "Int32":
                            propertyAccessor.SetValue(parentObj, Convert.ToInt32(data));
                            break;

                        case "Single":
                            propertyAccessor.SetValue(parentObj, Convert.ToSingle(data));
                            break;

                        case "Decimal":
                            propertyAccessor.SetValue(parentObj, Convert.ToDecimal(data));
                            break;

                        case "String":
                            propertyAccessor.SetValue(parentObj, data.ToString());
                            break;

                        default:
                            propertyAccessor.SetValue(parentObj, data);
                            break;
                    }

                parentObj = propertyAccessor.GetValue(parentObj);
            }
        }

        internal static ChartValueType GetDataType(object xval)
        {
            if (xval is string || xval is string[])
                return ChartValueType.String;
            else if (xval is DateTime || xval is DateTime[])
                return ChartValueType.DateTime;
            else if (xval is TimeSpan || xval is TimeSpan[])
                return ChartValueType.TimeSpan;
            else
                return ChartValueType.Double;
        }

#endregion

#region Internal Virtual Methods

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
                XAxisValueType = GetDataType(xvalueType);
                if (XAxisValueType == ChartValueType.DateTime || XAxisValueType == ChartValueType.Double ||
                    XAxisValueType == ChartValueType.Logarithmic || XAxisValueType == ChartValueType.TimeSpan)
                {
                    if (!(XValues is List<double>))
                        this.ActualXValues = this.XValues = new List<double>();
                }
                else
                {
                    if (!(XValues is List<string>))
                        this.ActualXValues = this.XValues = new List<string>();
                }

                if (IsMultipleYPathRequired)
                {
                    if (string.IsNullOrEmpty(yPaths[0]))
                        return;
                    if (XAxisValueType == ChartValueType.String)
                    {
                        IList<string> xValue = this.XValues as List<string>;
                        do
                        {
                            object xVal = null;
                            xVal = (enumerator.Current as DataRow).GetField(this.XBindingPath);
                            xValue.Add((string)xVal);
                            for (int i = 0; i < yPaths.Count(); i++)
                            {
                                object yVal = (enumerator.Current as DataRow).GetField(yPaths[i]);
                                yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                            }

                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.Double ||
                       XAxisValueType == ChartValueType.Logarithmic)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            object xVal = null;
                            xVal = (enumerator.Current as DataRow).GetField(this.XBindingPath);
                            XData = Convert.ToDouble(xVal ?? double.NaN);

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
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
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.DateTime)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            object xVal = null;
                            xVal = (enumerator.Current as DataRow).GetField(this.XBindingPath);
                            XData = ((DateTime)xVal).ToOADate();

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
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
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.TimeSpan)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            object xVal = null;
                            xVal = (enumerator.Current as DataRow).GetField(this.XBindingPath);
                            XData = ((TimeSpan)xVal).TotalMilliseconds;

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
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
                        dataCount = xValue.Count;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(yPaths[0]))
                        return;
                    IList<double> yValue = yLists[0];
                    object xVal = null, yVal = null;
                    if (XAxisValueType == ChartValueType.String)
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
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.Double ||
                        XAxisValueType == ChartValueType.Logarithmic)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            xVal = (enumerator.Current as DataRow).GetField(this.XBindingPath);
                            yVal = (enumerator.Current as DataRow).GetField(yPaths[0]);
                            XData = Convert.ToDouble(xVal ?? double.NaN);

                            // Check the Data Collection is linear or not.
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(XData);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.DateTime)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            xVal = (enumerator.Current as DataRow).GetField(this.XBindingPath);
                            yVal = (enumerator.Current as DataRow).GetField(yPaths[0]);
                            XData = ((DateTime)xVal).ToOADate();

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(XData);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.TimeSpan)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            xVal = (enumerator.Current as DataRow).GetField(this.XBindingPath);
                            yVal = (enumerator.Current as DataRow).GetField(yPaths[0]);
                            XData = ((TimeSpan)xVal).TotalMilliseconds;

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(((TimeSpan)xVal).TotalMilliseconds);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                }
            }

            IsPointGenerated = true;
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
                            this.dataCount--;
                        }
                        else if (XValues is IList<string>)
                        {
                            (XValues as IList<string>).RemoveAt(index);
                            this.dataCount--;
                        }

                        for (int i = 0; i < SeriesYValues.Count(); i++)
                        {
                            SeriesYValues[i].RemoveAt(index);
                        }

                        if (IsSortData)
                        {
                            SortActualPoints();
                        }

                        this.ActualData.RemoveAt(index);
                    }

                    break;
            }

            if (this is AccumulationSeriesBase || ActualArea.HasDataPointBasedLegend())
                ActualArea.IsUpdateLegend = true;
            totalCalculated = false;
            UpdateArea();
        }
       
        internal virtual void GenerateCustomTypeDescriptorPropertyPoints(string[] yPaths, IList<double>[] yLists, IEnumerator enumerator)
        {
            var properites = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
            var xBindingProperty = properites.Find(this.XBindingPath, false);
            if (xBindingProperty == null) return;
            for (int i = 0; i < yPaths.Count(); i++)
            {
                var yBindingProperty = properites.Find(yPaths[i], false);
                if (yBindingProperty == null) return;
            }

            var xAxisValue = xBindingProperty.GetValue(this.XBindingPath);
            double doubleResult;
            if (double.TryParse(xAxisValue.ToString(), out doubleResult))
            {
                XAxisValueType = ChartValueType.Double;
            }
            else if (xAxisValue is DateTime)
            {
                XAxisValueType = ChartValueType.DateTime;
            }
            else if (xAxisValue is TimeSpan)
            {
                XAxisValueType = ChartValueType.TimeSpan;
            }
            else
            {
                XAxisValueType = ChartValueType.String;
            }

            if (XAxisValueType == ChartValueType.Double || XAxisValueType == ChartValueType.Logarithmic)
            {
                if (!(ActualXValues is List<double>))
                    this.ActualXValues = this.XValues = new List<double>();
                IList<double> xValue = this.XValues as List<double>;
                do
                {
                    var itemProperites = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
                    object xVal = itemProperites.Find(this.XBindingPath, false).GetValue(this.XBindingPath);
                    XData = Convert.ToDouble(xVal ?? double.NaN);

                    // Check the Data Collection is linear or not
                    if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                    {
                        isLinearData = false;
                    }

                    xValue.Add(XData);
                    for (int i = 0; i < yPaths.Count(); i++)
                    {
                        var yVal =
                            itemProperites.Find(yPaths[i], false).GetValue(yPaths[i]);
                        yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                    }

                    ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                dataCount = xValue.Count;
            }
            else if (XAxisValueType == ChartValueType.DateTime)
            {
                if (!(ActualXValues is List<double>))
                    this.ActualXValues = this.XValues = new List<double>();
                IList<double> xValue = this.XValues as List<double>;
                do
                {
                    var itemProperites = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
                    object xVal = itemProperites.Find(this.XBindingPath, false).GetValue(this.XBindingPath);
                    XData = ((DateTime)xVal).ToOADate();

                    // Check the Data Collection is linear or not
                    if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                    {
                        isLinearData = false;
                    }

                    xValue.Add(XData);
                    for (int i = 0; i < yPaths.Count(); i++)
                    {
                        var yVal =
                           itemProperites.Find(yPaths[i], false).GetValue(yPaths[i]);
                        yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                    }

                    ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                dataCount = xValue.Count;
            }
            else if (XAxisValueType == ChartValueType.TimeSpan)
            {
                if (!(ActualXValues is List<double>))
                    this.ActualXValues = this.XValues = new List<double>();
                IList<double> xValue = this.XValues as List<double>;
                do
                {
                    var itemProperites = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
                    object xVal = itemProperites.Find(this.XBindingPath, false).GetValue(this.XBindingPath);
                    XData = ((TimeSpan)xVal).TotalMilliseconds;

                    // Check the Data Collection is linear or not
                    if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                    {
                        isLinearData = false;
                    }

                    xValue.Add(XData);
                    for (int i = 0; i < yPaths.Count(); i++)
                    {
                        var yVal =
                            itemProperites.Find(yPaths[i], false).GetValue(yPaths[i]);
                        yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                    }

                    ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                dataCount = xValue.Count;
            }
            else if (XAxisValueType == ChartValueType.String)
            {
                if (!(ActualXValues is List<string>))
                    this.ActualXValues = this.XValues = new List<string>();
                IList<string> xValue = this.XValues as List<string>;
                do
                {
                    var itemProperites = (enumerator.Current as ICustomTypeDescriptor).GetProperties();
                    object xVal = itemProperites.Find(this.XBindingPath, false).GetValue(this.XBindingPath);
                    xValue.Add((string)xVal);
                    for (int i = 0; i < yPaths.Count(); i++)
                    {
                        var yVal =
                            itemProperites.Find(yPaths[i], false).GetValue(yPaths[i]);
                        yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                    }

                    ActualData.Add(enumerator.Current);
                }
                while (enumerator.MoveNext());
                dataCount = xValue.Count;
            }
        }

        /// <summary>
        /// Updates the selection when selected index collection changed.
        /// </summary>
        internal virtual void SelectedSegmentsIndexes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ChartSelectionChangedEventArgs chartSelectionChangedEventArgs;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:

                    if (e.NewItems != null && ActualArea.SelectionBehaviour.EnableMultiSelection)
                    {
                        int oldIndex = PreviousSelectedIndex;

                        int newIndex = (int)e.NewItems[0];

                        if (newIndex >= 0 && ActualArea.GetEnableSegmentSelection())
                        {
                            // For adornment selection implementation
                            if (adornmentInfo is ChartAdornmentInfo && adornmentInfo.HighlightOnSelection)
                            {
                                UpdateAdornmentSelection(newIndex);
                            }

                            // Set the SegmentSelectionBrush to newIndex segment Interior
                            var selectableSegment = this as ISegmentSelectable;
                            if (newIndex < Segments.Count && selectableSegment != null && selectableSegment.SelectionBrush != null)
                            {
                                Segments[newIndex].BindProperties();
                                Segments[newIndex].IsSelectedSegment = true;
                            }

                            if (newIndex < Segments.Count)
                            {
                                chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                                {
                                    SelectedSegment = Segments[newIndex],
                                    SelectedSegments = ActualArea.SelectedSegments,
                                    SelectedSeries = this,
                                    SelectedIndex = newIndex,
                                    PreviousSelectedIndex = oldIndex,
                                    PreviousSelectedSegment = null,
                                    NewPointInfo = Segments[newIndex].Item,
                                    IsSelected = true
                                };

                                chartSelectionChangedEventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;

                                if (oldIndex != -1 && oldIndex < Segments.Count)
                                {
                                    chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[oldIndex];
                                    chartSelectionChangedEventArgs.OldPointInfo = Segments[oldIndex].Item;
                                }

                                (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
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

                    if (e.OldItems != null && ActualArea.SelectionBehaviour.EnableMultiSelection)
                    {
                        int newIndex = (int)e.OldItems[0];

                        chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                        {
                            SelectedSegment = null,
                            SelectedSegments = ActualArea.SelectedSegments,
                            SelectedSeries = null,
                            SelectedIndex = newIndex,
                            PreviousSelectedIndex = PreviousSelectedIndex,
                            PreviousSelectedSegment = null,
                            PreviousSelectedSeries = this,
                            IsSelected = false
                        };

                        if (PreviousSelectedIndex != -1 && PreviousSelectedIndex < Segments.Count)
                        {
                            chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[PreviousSelectedIndex];
                            chartSelectionChangedEventArgs.OldPointInfo = Segments[PreviousSelectedIndex].Item;
                        }

                            (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
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
            var chartTooltip = ActualArea.Tooltip as ChartTooltip;
            chartTooltip.PolygonPath = " ";

            int showDuration = ChartTooltip.GetActualShowDuration(ActualArea.TooltipBehavior, ChartTooltip.GetShowDuration(this));

            if (!canvas.Children.Contains(chartTooltip))
            {
                HastoolTip = true;
                canvas.Children.Add(chartTooltip);
            }

            AddTooltip();
            
            if (ChartTooltip.GetActualEnableAnimation(ActualArea.TooltipBehavior, ChartTooltip.GetEnableAnimation(this)))
            {
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
            Timer.Interval = new TimeSpan(0, 0, 0, 0, showDuration);
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
            previousMousePosition = mousePos;
            var chartTooltip = ActualArea.Tooltip as ChartTooltip;
            chartTooltip.PreviousSeries = this;

            if (ToolTipTag != null)
            {
                var chartSegment = ToolTipTag as ChartSegment;
                chartTooltip.UpdateLayout();
                Size tooltipSize = new Size(chartTooltip.ActualWidth, chartTooltip.ActualHeight);
                Point tooltipPosition = new Point();
                var tooltipBehavior = ActualArea.TooltipBehavior;
                ActualTooltipPosition = TooltipPosition.Auto;

                tooltipPosition = GetDataPointPosition(chartTooltip);
                if (!(this is CircularSeries || this is TriangularSeriesBase || this is PolarRadarSeriesBase))
                {
                    Rect clipRect = this.ActualArea.SeriesClipRect;
                    if (!(this is FinancialSeriesBase || (IsActualTransposed && (this is BarSeries || this is StackingBarSeries
                        || this is ColumnSeries || this is StackingColumnSeries || this is RangeSeriesBase))))
                    {
                        tooltipPosition = SetTooltipMarkerPosition(tooltipPosition, chartTooltip);
                    }

                    if (!clipRect.Contains(tooltipPosition))
                    {
                        tooltipPosition = mousePos;
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
                    chartTooltip.PolygonPath = ChartDataUtils.GenerateTooltipPolygon(tooltipSize, horizontal, vertical);
                }
            }
        }

        private Point SetTooltipMarkerPosition(Point tooltipPosition, ChartTooltip chartTooltip)
        {
            if (adornmentInfo != null && adornmentInfo.ShowMarker && (adornmentInfo.GetAdornmentPosition() == BarLabelAlignment.Top || this is RangeSeriesBase && adornmentInfo.GetAdornmentPosition() == BarLabelAlignment.Middle))
            { 
                if (adornmentInfo.MarkerType == ChartSymbol.Custom && adornmentInfo.MarkerTemplate != null)
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
                else if(adornmentInfo.MarkerType != ChartSymbol.Custom)
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
        /// Calculate the position and orientation of Tooltip nose.
        /// </summary>
        /// <param name="size">DesiredSize of ChartTooltip</param>
        /// <param name="position">Location of Tooltip whether based on Mouse or DataPoint</param>
        /// <param name="horizontal">Orientation of Tooltip nose</param>
        /// <param name="vertical">Orientation of Tooltip nose</param>
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

        /// <summary>
        /// Set the polygon points.
        /// </summary>
        /// <param name="control"></param>
        internal void GenerateTooltipPolygon(ContentControl control)
        {
            PointCollection polygonPoints = new PointCollection();

            var chartSegment = control.Content as ChartSegment;

            if (chartSegment.PolygonPoints != null && chartSegment.PolygonPoints.Count > 1)
            {
                chartSegment.PolygonPoints.Clear();
                chartSegment.PolygonPoints = null;
            }

            control.UpdateLayout();

            double labelWidth = control.DesiredSize.Width;
            double labelHeight = control.DesiredSize.Height;
            ChartAlignment horzontalAlignment = ChartAlignment.Far;
            ChartAlignment verticalAlignment = ChartAlignment.Center;

            double x = labelWidth / 2, y = labelHeight;

            switch (ChartTooltip.GetHorizontalAlignment(this))
            {
                case HorizontalAlignment.Center:
                    x = labelWidth / 2;
                    horzontalAlignment = ChartAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    x = 0;
                    horzontalAlignment = ChartAlignment.Far;
                    break;
                case HorizontalAlignment.Left:
                    x = labelWidth;
                    horzontalAlignment = ChartAlignment.Near;
                    break;
            }

            switch (ChartTooltip.GetVerticalAlignment(this))
            {
                case VerticalAlignment.Top:
                    verticalAlignment = ChartAlignment.Near;
                    y = labelHeight + seriesTipHeight;
                    break;
                case VerticalAlignment.Center:
                    verticalAlignment = ChartAlignment.Center;
                    y = labelHeight / 2;
                    break;
                case VerticalAlignment.Bottom:
                    verticalAlignment = ChartTooltip.GetHorizontalAlignment(this) == HorizontalAlignment.Center ? ChartAlignment.Far : ChartAlignment.Near;
                    y = 0;
                    break;
            }

            chartSegment.PolygonPoints = ChartDataUtils.GetTooltipPolygonPoints(new Rect(x, y, labelWidth, labelHeight), seriesTipHeight, false, horzontalAlignment, verticalAlignment);
        }

        private HorizontalPosition GetHorizontalPosition(HorizontalAlignment horizontalAlignment)
        {
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    return HorizontalPosition.Left;
                case HorizontalAlignment.Center:
                    return HorizontalPosition.Center;
                case HorizontalAlignment.Right:
                    return HorizontalPosition.Right;
                default:
                    return HorizontalPosition.Center;
            }
        }

        private VerticalPosition GetVerticalPosition(VerticalAlignment verticalAlignment)
        {
            switch (verticalAlignment)
            {
                case VerticalAlignment.Top:
                    return VerticalPosition.Top;
                case VerticalAlignment.Center:
                    return VerticalPosition.Center;
                case VerticalAlignment.Bottom:
                    return VerticalPosition.Bottom;
                default:
                    return VerticalPosition.Top;
            }
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

                    HastoolTip = false;
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal virtual void UpdateTooltip(object source)
        {
            if (ShowTooltip)
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
            else if (element.DataContext is ChartSegment && !(element.DataContext is ChartAdornment))
                tooltipTag = element.DataContext;
            else if (element.DataContext is ChartAdornmentContainer)
            {
                tooltipTag = GetSegment((element.DataContext as ChartAdornmentContainer).Adornment.Item);
            }
            else
            {
                var contentPresenter = VisualTreeHelper.GetParent(element) as ContentPresenter;

                if (contentPresenter != null && contentPresenter.Content is ChartAdornment)
                {
                    tooltipTag = GetSegment((contentPresenter.Content as ChartAdornment).Item);
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

        internal virtual void OnTransposeChanged(bool val)
        {
            IsActualTransposed = val;
        }

        /// <summary>
        /// This method used to get the chart data at an index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        internal virtual ChartDataPointInfo GetDataPoint(int index)
        {
            return dataPoint;
        }

        internal virtual void OnResetSegment(int index)
        {
            if (index < Segments.Count && index >= 0)
            {
                Segments[index].BindProperties();
                Segments[index].IsSelectedSegment = false;
                if (adornmentInfo is ChartAdornmentInfo)
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
            selectionChangedEventArgs.SelectedSegment = SelectedSegment;

            selectionChangedEventArgs.SelectedSegments = ActualArea.SelectedSegments;

            SetSelectionChangedEventArgs();

            ActualArea.OnSelectionChanged(selectionChangedEventArgs);

            PreviousSelectedIndex = selectionChangedEventArgs.SelectedIndex;

            triggerSelectionChangedEventOnLoad = false;
        }

        /// <summary>
        /// Set SelectionChanged event args
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal virtual void SetSelectionChangedEventArgs()
        {
            var segmentSelectableSeries = this as ISegmentSelectable;
            if (segmentSelectableSeries != null && Segments.Count != 0)
            {
                // Selects the adornment when the mouse is over or clicked on adornments(adornment selection).
                if (adornmentInfo is ChartAdornmentInfo && adornmentInfo.HighlightOnSelection)
                    UpdateAdornmentSelection(segmentSelectableSeries.SelectedIndex);

                selectionChangedEventArgs.SelectedIndex = segmentSelectableSeries.SelectedIndex;
                if (this.IsAreaTypeSeries || this.IsBitmapSeries || this is FastLineSeries)
                {
                    selectionChangedEventArgs.SelectedSegment = Segments[0];
                    selectionChangedEventArgs.NewPointInfo = GetDataPoint(selectionChangedEventArgs.SelectedIndex);
                }
                else
                    selectionChangedEventArgs.NewPointInfo = ActualData[selectionChangedEventArgs.SelectedIndex];
            }

            selectionChangedEventArgs.SelectedSeries = this;
            selectionChangedEventArgs.PreviousSelectedIndex = -1;
            selectionChangedEventArgs.IsSelected = true;
            selectionChangedEventArgs.PreviousSelectedSegment = null;
            selectionChangedEventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
        }

        internal virtual bool GetAnimationIsActive()
        {
            return false;
        }

        /// <summary>
        /// Finds the nearest point in technical indicator relative to the mouse point/touch position.
        /// </summary>
        /// <param name="technicalIndicator">Represents the indicator to which nearest point has to be calculated.</param>
        /// <param name="point">The co-ordinate point representing the current mouse point /touch position.</param>
        /// <param name="x">x-value of the nearest point.</param>
        /// <param name="y1">Series y-values of the nearest point</param>
        /// <param name="y2">Indicator y-values of the nearest point</param>
        internal virtual void FindNearestFinancialChartPoint(FinancialTechnicalIndicator technicalIndicator, Point point, out double x, out List<double> y1, out List<double> y2)
        {
            ChartPoint chartPoint;
            x = double.NaN;
            y1 = new List<double>();
            y2 = new List<double>();

            if (this.IsIndexed || !(this.ActualXValues is IList<double>))
            {
                if (ActualArea != null)
                {
                    double xStart = ActualXAxis.VisibleRange.Start;
                    double xEnd = ActualXAxis.VisibleRange.End;
                    chartPoint = new ChartPoint(ActualArea.ActualPointToValue(ActualXAxis, point), ActualArea.ActualPointToValue(ActualYAxis, point));
                    int range = (int)Math.Round(chartPoint.X);
                    if (ActualSeriesYValues.Count() > 0)
                    {
                        var count = ActualSeriesYValues[0].Count;
                        if (range <= xEnd && range >= xStart && range < count && range >= 0)
                        {
                            if (technicalIndicator != null)
                            {
                                foreach (ChartSegment segment in technicalIndicator.Segments)
                                {
                                    var fastColumnBitmapSegment = segment as FastColumnBitmapSegment;
                                    if (fastColumnBitmapSegment != null)
                                    {
                                        if (range < fastColumnBitmapSegment.y1ChartVals.Count)
                                            y2.Add(fastColumnBitmapSegment.y1ChartVals[range]);
                                    }
                                    else
                                    {
                                        var technicalIndicatorSegment = segment as TechnicalIndicatorSegment;
                                        if (range < technicalIndicatorSegment.yChartVals.Count)
                                        {
                                            int length = technicalIndicatorSegment.Length;

                                            if (length == 0 ? range < technicalIndicatorSegment.xChartVals[range]
                                                : range < technicalIndicatorSegment.Length - 1)
                                                y2.Add(double.NaN);
                                            else
                                            {
                                                var indicatorYValue = technicalIndicatorSegment.yChartVals[range];
                                                y2.Add(indicatorYValue);
                                            }
                                        }
                                    }
                                }
                            }

                            for (int i = 0; i < ActualSeriesYValues.Count(); i++)
                            {
                                var seriesYValue = ActualSeriesYValues[i][range];
                                y1.Add(seriesYValue);
                                
                                if (this is RangeColumnSeries && !IsMultipleYPathRequired)
                                {
                                    break;
                                }
                            }

                            x = range;
                        }
                    }
                }
            }
            else
            {
                IList<double> xValues = this.ActualXValues as IList<double>;
                ChartPoint nearPoint = new ChartPoint();
                nearPoint.X = ActualXAxis.VisibleRange.Start;
                if (IsSideBySide)
                {
                    DoubleRange sbsInfo = this.GetSideBySideInfo(this);
                    nearPoint.X = ActualXAxis.VisibleRange.Start + sbsInfo.Start;
                }

                nearPoint.Y = ActualYAxis.VisibleRange.Start;
                chartPoint = new ChartPoint(ActualArea.ActualPointToValue(ActualXAxis, point), ActualArea.ActualPointToValue(ActualYAxis, point));
                for (int i = 0; i < DataCount; i++)
                {
                    double xval = xValues[i];
                    double yval = this.ActualSeriesYValues[0][i];

                    if (this.ActualXAxis is LogarithmicAxis)
                    {
                        var logAxis = ActualXAxis as LogarithmicAxis;
                        if (Math.Abs(chartPoint.X - xval) <= Math.Abs(chartPoint.X - nearPoint.X) && (Math.Log(chartPoint.X, logAxis.LogarithmicBase) > ActualXAxis.VisibleRange.Start &&
                           Math.Log(chartPoint.X, logAxis.LogarithmicBase) < ActualXAxis.VisibleRange.End))
                        {
                            nearPoint = new ChartPoint(xval, yval);
                            x = xValues[i];
                            CalculateYValues(technicalIndicator, i, out y1, out y2, x);
                        }
                    }
                    else if (Math.Abs((chartPoint.X - xval)) <= Math.Abs((chartPoint.X - nearPoint.X)) &&
                       (chartPoint.X > ActualXAxis.VisibleRange.Start) && (chartPoint.X < ActualXAxis.VisibleRange.End))
                    {
                        nearPoint = new ChartPoint(xval, yval);
                        x = xValues[i];
                        CalculateYValues(technicalIndicator, i, out y1, out y2, x);
                    }
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals", Justification = "Reviewed")]
        internal virtual void GeneratePropertyPoints(string[] yPaths, IList<double>[] yLists)
        {
            IEnumerator enumerator = (ItemsSource as IEnumerable).GetEnumerator();
			if (enumerator == null || ActualData == null || (yPaths == null || yPaths.Length < 1 ) || (yLists == null || yLists.Length < 1)) return;
            
			PropertyInfo xPropertyInfo, yPropertyInfo;

            if (enumerator.MoveNext())
            {
                if (enumerator.Current is ICustomTypeDescriptor)
                    GenerateCustomTypeDescriptorPropertyPoints(yPaths, yLists, enumerator);
                else
                {
                    for (int i = 0; i < UpdateStartedIndex; i++)
                    {
                        enumerator.MoveNext();
                    }

                    xPropertyInfo = ChartDataUtils.GetPropertyInfo(enumerator.Current, this.XBindingPath);
                    IPropertyAccessor xPropertyAccessor = null;
                    if (xPropertyInfo != null)
                        xPropertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(xPropertyInfo);
                    if (xPropertyAccessor == null) return;
                    Func<object, object> xGetMethod = xPropertyAccessor.GetMethod;
                    if (xGetMethod(enumerator.Current) != null && xGetMethod(enumerator.Current).GetType().IsArray)
                        return;
                    XAxisValueType = GetDataType(xPropertyAccessor, ItemsSource as IEnumerable);

                    if (XAxisValueType == ChartValueType.DateTime || XAxisValueType == ChartValueType.Double ||
                        XAxisValueType == ChartValueType.Logarithmic || XAxisValueType == ChartValueType.TimeSpan)
                    {
                        if (!(ActualXValues is List<double>))
                            this.ActualXValues = this.XValues = new List<double>();
                    }
                    else
                    {
                        if (!(ActualXValues is List<string>))
                            this.ActualXValues = this.XValues = new List<string>();
                    }

                    if (IsMultipleYPathRequired)
                    {
                        List<IPropertyAccessor> yPropertyAccessor = new List<IPropertyAccessor>();
                        if (string.IsNullOrEmpty(yPaths[0]))
                            return;
                        for (int i = 0; i < yPaths.Count(); i++)
                        {
                            yPropertyInfo = ChartDataUtils.GetPropertyInfo(enumerator.Current, yPaths[i]);
                            if (yPropertyInfo == null) return;
                            var yProperty = FastReflectionCaches.PropertyAccessorCache.Get(yPropertyInfo);
                            if (yProperty == null ||
                                (yProperty.GetValue(enumerator.Current) != null &&
                                 yProperty.GetValue(enumerator.Current).GetType().IsArray)) return;
                            yPropertyAccessor.Add(yProperty);
                        }

                        if (XAxisValueType == ChartValueType.String)
                        {
                            IList<string> xValue = this.XValues as List<string>;
                            do
                            {
                                object xVal = xGetMethod(enumerator.Current);
                                xValue.Add((string)xVal);
                                for (int i = 0; i < yPropertyAccessor.Count; i++)
                                {
                                    object yVal = yPropertyAccessor[i].GetValue(enumerator.Current);
                                    yLists[i].Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                                }

                                ActualData.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            dataCount = xValue.Count;
                        }
                        else if (XAxisValueType == ChartValueType.Double ||
                                 XAxisValueType == ChartValueType.Logarithmic)
                        {
                            IList<double> xValue = this.XValues as List<double>;
                            do
                            {
                                object xVal = xGetMethod(enumerator.Current);
                                XData = Convert.ToDouble(xVal != null ? xVal : double.NaN);

                                // Check the Data Collection is linear or not
                                if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                                {
                                    isLinearData = false;
                                }

                                xValue.Add(XData);
                                for (int i = 0; i < yPropertyAccessor.Count; i++)
                                {
                                    object yVal = yPropertyAccessor[i].GetValue(enumerator.Current);
                                    yLists[i].Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                                }

                                ActualData.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            dataCount = xValue.Count;
                        }
                        else if (XAxisValueType == ChartValueType.DateTime)
                        {
                            IList<double> xValue = this.XValues as List<double>;
                            do
                            {
                                object xVal = xGetMethod(enumerator.Current);
                                XData = ((DateTime)xVal).ToOADate();

                                // Check the Data Collection is linear or not
                                if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                                {
                                    isLinearData = false;
                                }

                                xValue.Add(XData);
                                for (int i = 0; i < yPropertyAccessor.Count; i++)
                                {
                                    object yVal = yPropertyAccessor[i].GetValue(enumerator.Current);
                                    yLists[i].Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                                }

                                ActualData.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            dataCount = xValue.Count;
                        }
                        else if (XAxisValueType == ChartValueType.TimeSpan)
                        {
                            IList<double> xValue = this.XValues as List<double>;
                            do
                            {
                                object xVal = xGetMethod(enumerator.Current);
                                XData = ((TimeSpan)xVal).TotalMilliseconds;

                                // Check the Data Collection is linear or not
                                if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                                {
                                    isLinearData = false;
                                }

                                xValue.Add(XData);
                                for (int i = 0; i < yPropertyAccessor.Count; i++)
                                {
                                    object yVal = yPropertyAccessor[i].GetValue(enumerator.Current);
                                    yLists[i].Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                                }

                                ActualData.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            dataCount = xValue.Count;
                        }
                    }
                    else
                    {
                        string yPath = string.Empty;
                        var rangeColumnSeries = this is RangeColumnSeries;

                        for (int i = 0; i < yPaths.Length; i++)
                        {
                            if (string.IsNullOrEmpty(yPaths[i]) && !rangeColumnSeries)
                            {
                                return;
                            }

                            if (rangeColumnSeries && !IsMultipleYPathRequired)
                            {
                                if (!string.IsNullOrEmpty(yPaths[i]))
                                {
                                    yPath = yPaths[i];
                                    break;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                            else
                            {
                                yPath = yPaths[0];
                            }
                        }

                        yPropertyInfo = ChartDataUtils.GetPropertyInfo(enumerator.Current, yPath);
                        IPropertyAccessor yPropertyAccessor = null;
                        if (yPropertyInfo != null)
                            yPropertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(yPropertyInfo);
                        if (yPropertyAccessor == null) return;
                        IList<double> yValue = yLists[0];
                        if (yPropertyAccessor == null) return;
                        Func<object, object> yGetMethod = yPropertyAccessor.GetMethod;
                        if (yGetMethod(enumerator.Current) != null && yGetMethod(enumerator.Current).GetType().IsArray)
                            return;
                        if (XAxisValueType == ChartValueType.String)
                        {
                            IList<string> xValue = this.XValues as List<string>;
                            do
                            {
                                object xVal = xGetMethod(enumerator.Current);
                                object yVal = yGetMethod(enumerator.Current);
                                xValue.Add(xVal != null ? (string)xVal : string.Empty);
                                yValue.Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                                ActualData.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            dataCount = xValue.Count;
                        }
                        else if (XAxisValueType == ChartValueType.Double ||
                                 XAxisValueType == ChartValueType.Logarithmic)
                        {
                            IList<double> xValue = this.XValues as List<double>;
                            do
                            {
                                object xVal = xGetMethod(enumerator.Current);
                                object yVal = yGetMethod(enumerator.Current);
                                XData = Convert.ToDouble(xVal != null ? xVal : double.NaN);

                                // Check the Data Collection is linear or not
                                if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                                {
                                    isLinearData = false;
                                }

                                xValue.Add(XData);
                                yValue.Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                                ActualData.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            dataCount = xValue.Count;
                        }
                        else if (XAxisValueType == ChartValueType.DateTime)
                        {
                            IList<double> xValue = this.XValues as List<double>;

                            do
                            {
                                object xVal = xGetMethod(enumerator.Current);
                                object yVal = yGetMethod(enumerator.Current);
                                XData = ((DateTime)xVal).ToOADate();

                                // Check the Data Collection is linear or not
                                if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                                {
                                    isLinearData = false;
                                }

                                xValue.Add(XData);
                                yValue.Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                                ActualData.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            dataCount = xValue.Count;
                        }
                        else if (XAxisValueType == ChartValueType.TimeSpan)
                        {
                            IList<double> xValue = this.XValues as List<double>;

                            do
                            {
                                object xVal = xGetMethod(enumerator.Current);
                                object yVal = yGetMethod(enumerator.Current);
                                XData = ((TimeSpan)xVal).TotalMilliseconds;

                                // Check the Data Collection is linear or not
                                if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                                {
                                    isLinearData = false;
                                }

                                xValue.Add(XData);
                                yValue.Add(Convert.ToDouble(yVal != null ? yVal : double.NaN));
                                ActualData.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            dataCount = xValue.Count;
                        }
                    }

                    HookPropertyChangedEvent(ListenPropertyChange);
                }
            }

            IsPointGenerated = true;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals", Justification = "Reviewed")]
        internal virtual void GenerateComplexPropertyPoints(string[] yPaths, IList<double>[] yLists, GetReflectedProperty getPropertyValue)
        {
            IEnumerator enumerator = (ItemsSource as IEnumerable).GetEnumerator();
            if (enumerator.MoveNext())
            {
                for (int i = 0; i < UpdateStartedIndex; i++)
                {
                    enumerator.MoveNext();
                }
                
                XAxisValueType = GetDataType(ItemsSource as IEnumerable, XComplexPaths);
                if (XAxisValueType == ChartValueType.DateTime || XAxisValueType == ChartValueType.Double ||
                    XAxisValueType == ChartValueType.Logarithmic || XAxisValueType == ChartValueType.TimeSpan)
                {
                    if (!(XValues is List<double>))
                        this.ActualXValues = this.XValues = new List<double>();
                }
                else
                {
                    if (!(XValues is List<string>))
                        this.ActualXValues = this.XValues = new List<string>();
                }

                if (IsMultipleYPathRequired)
                {
                    if (string.IsNullOrEmpty(yPaths[0]))
                        return;
                    object xVal = null;
                    xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                    if (xVal == null) return;
                    for (int i = 0; i < yPaths.Count(); i++)
                    {
                        var yPropertyValue = getPropertyValue(enumerator.Current, YComplexPaths[i]);
                        if (yPropertyValue == null) return;
                    }

                    if (XAxisValueType == ChartValueType.String)
                    {
                        IList<string> xValue = this.XValues as List<string>;
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                            xValue.Add((string)xVal);
                            for (int i = 0; i < yPaths.Count(); i++)
                            {
                                var yVal = getPropertyValue(enumerator.Current, YComplexPaths[i]);
                                yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                            }

                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.Double ||
                       XAxisValueType == ChartValueType.Logarithmic)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                            XData = Convert.ToDouble(xVal ?? double.NaN);

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(XData);
                            for (int i = 0; i < yPaths.Count(); i++)
                            {
                                var yVal = getPropertyValue(enumerator.Current, YComplexPaths[i]);
                                yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                            }

                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.DateTime)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                            XData = ((DateTime)xVal).ToOADate();

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(XData);
                            for (int i = 0; i < yPaths.Count(); i++)
                            {
                                var yVal = getPropertyValue(enumerator.Current, YComplexPaths[i]);
                                yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                            }

                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.TimeSpan)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                            XData = ((TimeSpan)xVal).TotalMilliseconds;

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(XData);
                            for (int i = 0; i < yPaths.Count(); i++)
                            {
                                var yVal = getPropertyValue(enumerator.Current, YComplexPaths[i]);
                                yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                            }

                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                }
                else
                {
                    string[] tempYPath = YComplexPaths[0];

                    if (string.IsNullOrEmpty(yPaths[0]))
                        return;
                    IList<double> yValue = yLists[0];
                    object xVal = null, yVal = null;

                    if (XAxisValueType == ChartValueType.String)
                    {
                        IList<string> xValue = this.XValues as List<string>;
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                            yVal = getPropertyValue(enumerator.Current, tempYPath);
                            if (xVal == null) return;
                            xValue.Add((string)xVal);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.Double ||
                        XAxisValueType == ChartValueType.Logarithmic)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                            yVal = getPropertyValue(enumerator.Current, tempYPath);
                            if (xVal == null) return;
                            XData = Convert.ToDouble(xVal);

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(XData);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.DateTime)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                            yVal = getPropertyValue(enumerator.Current, tempYPath);
                            if (xVal == null) return;
                            XData = ((DateTime)xVal).ToOADate();

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(XData);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.TimeSpan)
                    {
                        IList<double> xValue = this.XValues as List<double>;
                        do
                        {
                            xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                            yVal = getPropertyValue(enumerator.Current, tempYPath);
                            if (xVal == null) return;
                            XData = ((TimeSpan)xVal).TotalMilliseconds;

                            // Check the Data Collection is linear or not
                            if (isLinearData && xValue.Count > 0 && XData < xValue[xValue.Count - 1])
                            {
                                isLinearData = false;
                            }

                            xValue.Add(XData);
                            yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                            ActualData.Add(enumerator.Current);
                        }
                        while (enumerator.MoveNext());
                        dataCount = xValue.Count;
                    }
                }

                HookPropertyChangedEvent(ListenPropertyChange);
            }

            IsPointGenerated = true;
        }


        internal virtual void OnDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewStartingIndex > -1 && e.NewStartingIndex < this.DataCount && e.NewItems[0] != null)
                    {
                        if (!this.isNotificationSuspended)
                        {
                            SetIndividualPoint(e.NewStartingIndex, e.NewItems[0], true);

                            if (IsSortData)
                            {
                                SortActualPoints();
                            }

                            this.UpdateSegments(e.OldStartingIndex, e.Action);
                        }
                    }

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (this.ItemsSource != null && e.OldStartingIndex < this.DataCount)
                    {
                        if (XValues is IList<double>)
                        {
                            (XValues as IList<double>).RemoveAt(e.OldStartingIndex);
                            this.dataCount--;
                        }
                        else if (XValues is IList<string>)
                        {
                            (XValues as IList<string>).RemoveAt(e.OldStartingIndex);
                            this.dataCount--;
                        }

                        if (SegmentColorPath != null && IsColorPathSeries)
                        {
                            if (ColorValues is IList<Brush> && ColorValues.Count > 0)
                                (ColorValues as IList<Brush>).RemoveAt(e.OldStartingIndex);
                            if (e.OldStartingIndex == 0 && ActualArea.Legend != null)
                                ActualArea.IsUpdateLegend = true;
                        }

                        for (int i = 0; i < SeriesYValues.Count(); i++)
                        {
                            SeriesYValues[i].RemoveAt(e.OldStartingIndex);
                        }

                        if (IsSortData)
                        {
                            SortActualPoints();
                        }

                        this.ActualData.RemoveAt(e.OldStartingIndex);

                        if (ToggledLegendIndex.Count > 0 && IsSingleAccumulationSeries)
                        {
                            var toggledIndexes = new List<int>();
                            foreach (var index in ToggledLegendIndex)
                            {
                                if (e.OldStartingIndex < index)
                                    toggledIndexes.Add(index - 1);
                                else
                                {
                                    if (e.OldStartingIndex != index)
                                        toggledIndexes.Add(index);
                                }
                            }

                            ToggledLegendIndex = toggledIndexes;
                        }

                        if (!this.isNotificationSuspended)
                        {
                            this.UpdateSegments(e.OldStartingIndex, e.Action);

                            UnHookPropertyChangedEvent(ListenPropertyChange, e.OldItems[0]);
                        }
                        else
                            UpdateStartedIndex = UpdateStartedIndex != 0 ? UpdateStartedIndex - e.OldItems.Count : UpdateStartedIndex;
                    }

                    break;

                case NotifyCollectionChangedAction.Add:
                    {
                        if (this.ItemsSource != null)
                        {
                            if (!this.isNotificationSuspended)
                            {
                                this.SetIndividualPoint(e.NewStartingIndex, e.NewItems[0], false);
                                if (SegmentColorPath != null && IsColorPathSeries && ColorValues.Count > 0)
                                {
                                    if (e.NewStartingIndex == 0 && ActualArea.Legend != null)
                                        ActualArea.IsUpdateLegend = true;
                                }

                                if (IsSortData)
                                {
                                    SortActualPoints();
                                }

                                if (ToggledLegendIndex.Count > 0 && IsSingleAccumulationSeries)
                                {
                                    var toggledIndexes = new List<int>();
                                    foreach (var index in ToggledLegendIndex)
                                    {
                                        if (e.NewStartingIndex <= index)
                                            toggledIndexes.Add(index + 1);
                                        else
                                            toggledIndexes.Add(index);
                                    }

                                    ToggledLegendIndex = toggledIndexes;
                                }

                                this.UpdateSegments(e.NewStartingIndex, e.Action);
                            }
                            else if (!isUpdateStarted)
                            {
                                UpdateStartedIndex = e.NewStartingIndex;
                                isUpdateStarted = true;
                            }
                        }

                        break;
                    }

                default:
                    {
                        Refresh();
                        ChartBase chart = ActualArea as ChartBase;
                        if (chart != null)
                        {
                            var seriesCollection = chart.GetChartSeriesCollection();

                            bool isContainsNormalSeries = seriesCollection.Any(chartseries => !chartseries.IsBitmapSeries);
                            //WPF-50490 FastBitmap series not cleared when using with other combination of series
                            if (IsBitmapSeries && isContainsNormalSeries && chart.fastRenderSurface != null)
                            {
                                chart.UpdateBitmapSeries(this as ChartSeries, false);
                            }
                        }

                        break;
                    }
            }

            isPointValidated = false;
            if (ShowEmptyPoints)
                RevalidateEmptyPointsCollection(e.Action, e.NewStartingIndex, e.OldStartingIndex);
            if (this is AccumulationSeriesBase || ActualArea.HasDataPointBasedLegend())
                ActualArea.IsUpdateLegend = true;
            totalCalculated = false;

            var axis2D = ActualXAxis as ChartAxisBase2D;
            if (axis2D != null)
            {
                axis2D.CanAutoScroll = true;
            }

            //WPF-51090 AutoScrolling not working for secondary axis.
            axis2D = ActualYAxis as ChartAxisBase2D;
            if (axis2D != null)
            {
                axis2D.CanAutoScroll = true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal virtual void CalculateSegments()
        {
            ApplyTemplate();
            ////Segments.Clear();
            ////XAMARIN-37971 LogarithmicAxis appears different in all platforms when having a range from 0 to 1
            if (ActualYAxis is LogarithmicAxis)
            {
				if(ActualSeriesYValues != null)
				{
                    foreach (var yValue in ActualSeriesYValues)
                    {
                        ValidateLogEmptyValues(yValue);
                    }
				}
            }

            if (ActualXAxis is LogarithmicAxis)
            {
                ValidateLogEmptyValues(ActualXValues as List<double>);
            }

            if (dataCount > 0)
            {
                if (!isPointValidated)
                {
                    if (ShowEmptyPoints)
                    {
                        ValidateYValues();
                        isPointValidated = true;
                    }
                    else if (EmptyPointIndexes != null)
                    {
                        ReValidateYValues(EmptyPointIndexes);
                    }
                }

                isPointValidated = false;
                CreateSegments();

                if (ActualArea.HasDataPointBasedLegend())
                {
                    ActualArea.IsUpdateLegend = true;
                }

                int index = (this is ISegmentSelectable) ? (this as ISegmentSelectable).SelectedIndex : -1;

                if (triggerSelectionChangedEventOnLoad && index >= 0 && this.DataCount > index)
                    RaiseSelectionChangedEvent();
            }
            else if (Segments == null)
            {
                return;
            }
            else if (dataCount == 0 && Segments.Count > 0) // WPF-13974 -Last segment have cleared from the collection while datamodel is changed.
            {
                ClearUnUsedSegments(dataCount);
            }
        }

        //XAMARIN-37971 LogarithmicAxis appears different in all platforms when having a range from 0 to 1
        private void ValidateLogEmptyValues(IList<double> yValue)
        {
            if (yValue.Any(x => x <= 0))
            {
                for (int i = 0; i < yValue.Count; i++)
                {
                    if (yValue[i] <= 0)
                        yValue[i] = double.NaN;
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal virtual void UpdateEmptyPointSegments(List<double> xValues, bool isSidebySideSeries)
        {
            int eIndex = 0;
            DoubleRange sbsInfo = isSidebySideSeries ? GetSideBySideInfo(this) : new DoubleRange();
            if (EmptyPointIndexes != null && EmptyPointIndexes.Count() > 0)
                foreach (var values in ActualSeriesYValues)
                {
                    switch (EmptyPointStyle)
                    {
                        case EmptyPointStyle.Interior:
                            {
                                if (EmptyPointIndexes.Count() > eIndex)
                                    foreach (var item in EmptyPointIndexes[eIndex])
                                    {
                                        int index = -1;
                                        if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                                            index = xValues.Count - 1;
                                        else
#pragma warning disable CS0618 // Type or member is obsolete
                                            index = (this is FunnelSeries) ? (this.DataCount - 1) - item : item;
#pragma warning restore CS0618 // Type or member is obsolete
                                        if ((this is LineSeries || this is SplineSeries || this is StepLineSeries || this is StackingLineSeries
                                            || (this is PolarRadarSeriesBase && (this as PolarRadarSeriesBase).DrawType == ChartSeriesDrawType.Line)) && item != 0)
                                            Segments[index - 1].IsEmptySegmentInterior = true;
                                        if (Segments.Count > index)
                                            Segments[((Segments.Count == index) ? index - 1 : index)].IsEmptySegmentInterior = true;

                                        if (Adornments != null && index >= 0 && Adornments.Count > index)
                                            Adornments[index].IsEmptySegmentInterior = true;
                                    }

                                eIndex++;
                            }

                            break;

                        case EmptyPointStyle.Symbol:
                            {
                                if (EmptyPointIndexes.Count() > eIndex)
                                    foreach (var item in EmptyPointIndexes[eIndex])
                                    {
                                        double x = xValues[item] + (sbsInfo.Start + sbsInfo.End) / 2;
                                        int index = -1;
                                        if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                                            index = xValues.Count - 1;
                                        else
#pragma warning disable CS0618 // Type or member is obsolete
                                            index = (this is FunnelSeries) ? (this.DataCount - 1) - item : item;
#pragma warning restore CS0618 // Type or member is obsolete
                                        if ((this is LineSeries || this is SplineSeries || this is StepLineSeries || this is StackingLineSeries
                                        ||
                                        (this is PolarRadarSeriesBase &&
                                         (this as PolarRadarSeriesBase).DrawType == ChartSeriesDrawType.Line)) &&
                                       item != 0)
                                        {
                                            Segments[index - 1].Interior =
                                                new SolidColorBrush(NativeColor.FromArgb(0, 255, 255, 255));
                                            Segments[((Segments.Count == index) ? index - 1 : index)] =
                                                new EmptyPointSegment(x, values[index], this, false) { Item = ActualData[index] };
                                        }
                                        else if (!(this is StackingAreaSeries))
                                        {
                                            Segments[index] = new EmptyPointSegment(x, values[index], this, false);
                                            Segments[index].Item = ActualData[index];
                                        }

                                        if (Adornments != null && index >= 0 && Adornments.Count > index)
                                            Adornments[index].IsEmptySegmentInterior = false;
                                    }

                                eIndex++;
                            }

                            break;
                        case EmptyPointStyle.SymbolAndInterior:
                            {
                                if (EmptyPointIndexes.Count() > eIndex)
                                    foreach (var item in EmptyPointIndexes[eIndex])
                                    {
                                        int index = -1;
                                        if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                                            index = xValues.Count - 1;
                                        else
#pragma warning disable CS0618 // Type or member is obsolete
                                            index = (this is FunnelSeries) ? (this.DataCount - 1) - item : item;
#pragma warning restore CS0618 // Type or member is obsolete
                                        double x = xValues[index] + (sbsInfo.Start + sbsInfo.End) / 2;
                                        if ((this is LineSeries || this is SplineSeries || this is StepLineSeries || this is StackingLineSeries
                                             ||
                                             (this is PolarRadarSeriesBase &&
                                              (this as PolarRadarSeriesBase).DrawType == ChartSeriesDrawType.Line)) &&
                                            item != 0)
                                        {
                                            Segments[index - 1].Interior =
                                                new SolidColorBrush(NativeColor.FromArgb(0, 255, 255, 255));
                                            Segments[((Segments.Count == index) ? index - 1 : index)] =
                                                new EmptyPointSegment(x, values[index], this, true) { Item = ActualData[index] };
                                        }
                                        else if (!(this is StackingAreaSeries))
                                        {
                                            Segments[((Segments.Count == index) ? index - 1 : index)].IsEmptySegmentInterior
                                                = true;
                                            Segments[index] = new EmptyPointSegment(x, values[index], this, true);
                                            Segments[index].Item = ActualData[index];
                                        }

                                        if (Adornments != null && index >= 0 && Adornments.Count > index)
                                            Adornments[index].IsEmptySegmentInterior = true;
                                    }

                                eIndex++;
                            }

                            break;

                        default:
                            break;
                    }
                }
        }

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

        internal virtual void UpdateRange()
        {
        }

        /// <summary>
        /// Validate the datapoints for segment implementation.
        /// </summary>
        internal virtual void ValidateYValues()
        {
        }

        /// <summary>
        /// Validate the datapoints for segment implementation.
        /// </summary>
        internal virtual void ReValidateYValues(List<int>[] emptyPointIndexs)
        {
        }

        /// <summary>
        /// Method is used to raise SelectionChanging event
        /// </summary>
        /// <param name="newIndex">Used to indicate current selected index</param>
        /// <param name="oldIndex">Used to indicate previous selected index</param>   
        internal virtual bool RaiseSelectionChanging(int newIndex, int oldIndex)
        {
            selectionChangingEventArgs.SelectedSegments = ActualArea.SelectedSegments;

            if (IsBitmapSeries || IsAreaTypeSeries || this is FastLineSeries)
            {
                selectionChangingEventArgs.SelectedSegment = newIndex != -1 ? GetDataPoint(newIndex) : null;
            }
            else if (this is ISegmentSelectable)
            {
                if (newIndex >= 0 && newIndex < Segments.Count)
                {
                    selectionChangingEventArgs.SelectedSegment = Segments[newIndex];
                }
                else
                    selectionChangingEventArgs.SelectedSegment = null;
            }

            SetSelectionChangingEventArgs(newIndex, oldIndex);

            ActualArea.OnSelectionChanging(selectionChangingEventArgs);

            return selectionChangingEventArgs.Cancel;
        }

        /// <summary>
        /// This method used to get the SfChart data at a mouse position.
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
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
                            if (this is FastStackingColumnBitmapSeries)
                            {
                                values.Reverse();
                                yVal = values[0];
                            }
                            else
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
                    dataPoint.Item = ((this is ColumnSeries || this is FastColumnBitmapSeries || this is StackingColumnSeries
                        || this is BarSeries || this is FastScatterBitmapSeries || this is StackingBarSeries || this is FastStackingColumnBitmapSeries) &&
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
            if (this.adornmentInfo != null && adornmentInfo.Visible && adornmentInfo.LabelPresenters != null)
            {
                foreach (FrameworkElement label in this.adornmentInfo.LabelPresenters)
                {
                    label.ClearValue(FrameworkElement.RenderTransformProperty);
                    label.ClearValue(UIElement.OpacityProperty);
                }
            }
        }

        internal virtual void Animate()
        {

        }

        internal virtual void Dispose()
        {
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

            if(m_visibleAdornments!=null)
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
                Segments = null;
            }

            ItemsSource = null;

            if (ColorModel != null)
            {
                ColorModel.Dispose();
            }

            if (SelectedSegmentsIndexes != null)
                SelectedSegmentsIndexes.CollectionChanged -= SelectedSegmentsIndexes_CollectionChanged;

            ActualArea = null;
            ChartTransformer = null;
            dataPoint = null;
            SeriesRootPanel = null;
            ToolTipTag = null;
        }

#endregion

#region Internal Methods

        internal object GetSegment(object item)
        {
            return Segments.Where(segment => segment.Item == item).FirstOrDefault();
        }

        internal bool HastoolTip = false;

        internal TooltipPosition ActualTooltipPosition { get; set; }

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
                        if (this is BoxAndWhiskerSeries && customTag is ScatterSegment)
                        {
                            if (OutlierTooltipTemplate == null)
                                chartTooltip.ContentTemplate = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipTemplate"] as DataTemplate;
                            else
                                chartTooltip.ContentTemplate = OutlierTooltipTemplate;
                        }
                        else
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
                    _stopwatch.Start();
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
        
        internal List<double> GetYValues()
        {
            var yvalues = new List<double>();
            var accumulationSeries = this as AccumulationSeriesBase;
            for (int i = 0; i < DataCount; i++)
            {
                if (!ToggledLegendIndex.Contains(i))
                {
                    if (accumulationSeries != null)
                        yvalues.Add(accumulationSeries.YValues[i]);
                }
                else
                    yvalues.Add(double.NaN);
            }

            return yvalues;
        }

        internal void ValidateDataPoints(params IList<double>[] yValues)
        {
            if (EmptyPointIndexes == null || EmptyPointIndexes.Count() == 0)
                EmptyPointIndexes = new List<int>[yValues.Length];
            int eindex = 0;
            foreach (var values in yValues)
            {
                if (EmptyPointIndexes[eindex] == null || EmptyPointIndexes[eindex].Count == 0)
                    EmptyPointIndexes[eindex] = new List<int>();
                if (values.Count != 0)
                    switch (EmptyPointValue)
                    {
                        case EmptyPointValue.Zero:
                            for (int i = 0; i < values.Count; i++)
                            {
                                if (double.IsNaN(values[i]))
                                {
                                    values[i] = 0;
                                    if (!EmptyPointIndexes[eindex].Contains(i))
                                        EmptyPointIndexes[eindex].Add(i);
                                }
                            }

                            break;

                        case EmptyPointValue.Average:
                            int j = 0;

                            if (values.Count == 1 && double.IsNaN(values[j]))
                            {
                                values[j] = 0;
                                EmptyPointIndexes[eindex].Add(0);
                                break;
                            }

                            if (double.IsNaN(values[j]))
                            {
                                values[j] = (0 + (double.IsNaN(values[j + 1]) ? 0 : values[j + 1])) / 2;
                                if (!EmptyPointIndexes[eindex].Contains(j))
                                    EmptyPointIndexes[eindex].Add(0);
                            }

                            for (; j < values.Count - 1; j++)
                            {
                                if (double.IsNaN(values[j]))
                                {
                                    values[j] = (values[j - 1] + (double.IsNaN(values[j + 1]) ? 0 : values[j + 1])) / 2;
                                    if (!EmptyPointIndexes[eindex].Contains(j))
                                        EmptyPointIndexes[eindex].Add(j);
                                }
                            }

                            if (double.IsNaN(values[j]))
                            {
                                values[j] = values[j - 1] / 2;
                                if (!EmptyPointIndexes[eindex].Contains(j))
                                    EmptyPointIndexes[eindex].Add(j);
                            }

                            break;

                        default:
                            break;
                    }

                yValues[eindex] = values;
                eindex++;
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
                        legendIcon = fastScatterBitmapSeries.ShapeType == ChartSymbol.Ellipse ? "Circle" : fastScatterBitmapSeries.ShapeType.ToString();
                    }
                    else if (this is WaterfallSeries | this is FastBarBitmapSeries)
                    {
                        legendIcon = "Column";
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
                && ChartDictionaries.GenericLegendDictionary.Keys.Contains(legendIcon))
                {
                    LegendIconTemplate = ChartDictionaries.GenericLegendDictionary[legendIcon] as DataTemplate;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Method used to set SelectionChangingEventArgs values
        /// </summary>
        /// <param name="newIndex"></param>
        /// <param name="oldIndex"></param>
        internal void SetSelectionChangingEventArgs(int newIndex, int oldIndex)
        {
            selectionChangingEventArgs.SelectedIndex = newIndex;
            selectionChangingEventArgs.SelectedSeries = this;
            selectionChangingEventArgs.PreviousSelectedIndex = oldIndex;
            selectionChangingEventArgs.Cancel = false;

            if (this.ActualArea.SelectedSeriesCollection.Count > 0)
            {
                if (this.ActualArea.SelectedSeriesCollection.Contains(this.ActualArea.CurrentSelectedSeries))
                    selectionChangingEventArgs.IsSelected = true;
                else
                    selectionChangingEventArgs.IsSelected = false;
            }
            else if (this.SelectedSegmentsIndexes.Count > 0)
            {
                if (this.SelectedSegmentsIndexes.Contains(newIndex))
                    selectionChangingEventArgs.IsSelected = true;
                else
                    selectionChangingEventArgs.IsSelected = false;
            }
            else
                selectionChangingEventArgs.IsSelected = false;
        }

        /// <summary>
        /// Removes the Segments
        /// </summary>
        internal void RemoveSegments()
        {
            for (int i = 0; i < Segments.Count;)
            {
                Segments.RemoveAt(i);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal Brush GetFinancialSeriesInterior(int segmentIndex)
        {
            if (Interior != null)
                return Interior;
            if (segmentIndex < 0) return Interior;
            var segment = this.IsBitmapSeries ? this.Segments[0] : this.Segments[segmentIndex];

            if (segment is CandleSegment)
            {
                return (segment as CandleSegment).ActualStroke;
            }
            else if (segment is HiLoOpenCloseSegment)
            {
                return (segment as HiLoOpenCloseSegment).ActualInterior;
            }
            else if (segment is FastHiLoOpenCloseSegment)
            {
                var value = (segment as FastHiLoOpenCloseSegment).GetSegmentBrush(segmentIndex);
                return new SolidColorBrush(value);
            }
            else
            {
                var value = (segment as FastCandleBitmapSegment).GetSegmentBrush(segmentIndex);
                return new SolidColorBrush(value);
            }
        }

        internal Brush GetInteriorColor(int segmentIndex)
        {
            int serIndex = ActualArea.GetSeriesIndex(this);
            if (Interior != null)
                return Interior;
            if (SegmentColorPath != null)
            {
                if (ColorValues.Count > 0 && ColorValues[segmentIndex] != null)
                    return ColorValues[segmentIndex];
                else if (Palette != ChartColorPalette.None)
                    return ColorModel.GetBrush(segmentIndex);
                else
                    return ActualArea.ColorModel.GetBrush(serIndex);
            }

            if (Palette != ChartColorPalette.None)
                return ColorModel.GetBrush(segmentIndex);
            if (ActualArea.Palette != ChartColorPalette.None)
            {
                if (serIndex >= 0)
                    return ActualArea.ColorModel.GetBrush(serIndex);
                else if (ActualArea is ChartBase)
                {
                    serIndex = (ActualArea as ChartBase).TechnicalIndicators.IndexOf(this as ChartSeries);
                    return ActualArea.ColorModel.GetBrush(serIndex);
                }
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal void CalculateSideBySideInfoPadding(double minWidth, int all, int pos, bool isXAxis)
        {
            var axis = this.ActualXAxis;
            bool isAlterRange = ((axis is NumericalAxis && (axis as NumericalAxis).RangePadding == NumericalPadding.None)
                    || (axis is DateTimeAxis && (axis as DateTimeAxis).RangePadding == DateTimeRangePadding.None));
            double space = isAlterRange ? 1 - ChartSeriesBase.GetSpacing(this) : ChartSeriesBase.GetSpacing(this);
            double div = minWidth * space / all;
            double padStart = div * (pos - 1) - minWidth * space / 2;
            double padEnd = padStart + div;

            if (isXAxis)
                SideBySideInfoRangePad = new DoubleRange(padStart, padEnd);
        }

        /// <summary>
        /// calculates the side-by-side position for all applicable series.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional")]
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

            var technicalIndicators = new List<ChartSeriesBase>();

            if (ActualArea is ChartBase && (ActualArea as ChartBase).TechnicalIndicators != null)
            {
                foreach (ChartSeries indicator in (ActualArea as ChartBase).TechnicalIndicators)
                {
                    technicalIndicators.Add(indicator as ChartSeriesBase);
                }
            }

            var seriesCollection = from series in ActualXAxis.RegisteredSeries select (ChartSeriesBase)series; // WRT-2246-Side by side info Series isn't properly arranged while its with multiple axis

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    all = 0;
                    var filteredSeries = (from series in this is ChartSeries ? seriesCollection.Union(technicalIndicators) : seriesCollection
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
                    var stackedColumns = new List<ChartSeriesBase>();
                    foreach (ChartSeriesBase item in filteredSeries)
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

                                foreach (var stackedColumn in stackedColumns.Where(stackedColumn => (stackedColumn.ActualYAxis == item.ActualYAxis) && ((stackedColumn as StackingSeriesBase).GroupName == (item as StackingSeriesBase).GroupName)))
                                {
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

        /// <summary>
        /// Method for getting the property values by the property name with its index
        /// </summary>
        /// <param name="parentObj">Current object</param>
        /// <param name="paths">XComplexPaths</param>
        /// <returns>Property value</returns>
        internal object GetArrayPropertyValue(object parentObj, string[] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                parentObj = GetComplexArrayPropertyValue(parentObj, path);
            }

            return parentObj;
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

        /// <summary>
        /// Method for get the property values by the property name
        /// </summary>
        /// <param name="obj">Current object</param>
        /// <param name="paths">XComplexPaths</param>
        /// <returns>Property value</returns>
        internal object GetPropertyValue(object obj, string[] paths)
        {
            object parentObj = obj;

            for (int i = 0; i < paths.Length; i++)
            {
                parentObj = ReflectedObject(parentObj, paths[i]);
            }

            if (parentObj != null)
                if (parentObj.GetType().IsArray) return null;
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
                        if (IsComplexYProperty || XBindingPath.Contains('.'))
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

        internal ChartValueType GetDataType(IEnumerable itemSource, string[] paths)
        {
            var enumerator = itemSource.GetEnumerator();
            object parentObj = null;

            if (enumerator.MoveNext())
            {
                // GetArrayPropertyValue method is used to get value from the path of current object
                parentObj = GetArrayPropertyValue(enumerator.Current, paths);
            }

            return GetDataType(parentObj);
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

                if (model != null)
                {
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

        internal void UpdateEmptyPoints(int index)
        {
            if (EmptyPointIndexes != null && EmptyPointIndexes.Count() > 0 && ActualArea is ChartBase && (ActualArea as ChartBase).GetSeriesCollection() != null && (ActualArea as ChartBase).GetSeriesCollection().Count > 0)
                foreach (var emptyPointIndex in EmptyPointIndexes[0])
                {
                    if (emptyPointIndex == index)
                    {
                        EmptyPointIndexes[0].Remove(emptyPointIndex);
                        Segments[index].IsEmptySegmentInterior = false;
                        break;
                    }
                }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal IEnumerable Clone(IEnumerable XValues)
        {
            IEnumerable newValues = null;

            if (XValues as List<double> != null)
            {
                newValues = new List<double>();
                List<double> newxValues = newValues as List<double>;
                List<double> xValues = XValues as List<double>;
                newxValues.AddRange(xValues);
                dataCount = newxValues.Count;
            }
            else if (XValues as List<string> != null)
            {
                newValues = new List<string>();
                List<string> newxValues = newValues as List<string>;
                List<string> xValues = XValues as List<string>;
                newxValues.AddRange(xValues);
                dataCount = newxValues.Count;
            }

            return newValues;
        }

        internal double GetGrandTotal(IList<double> yValues)
        {
            if (!totalCalculated)
            {
                grandTotal = (from val in yValues where !double.IsNaN(val) select val).Sum();
                totalCalculated = true;
            }

            return grandTotal;
        }

        internal DataTemplate GetTrackballTemplate()
        {
            return this.TrackballLabelTemplate;
        }

        internal object GetActualXValue(int index)
        {
            switch (XAxisValueType)
            {
                case ChartValueType.DateTime:
                    {
                        return ((IList<double>)ActualXValues)[index].FromOADate();
                    }

                case ChartValueType.String:
                    {
                        return ((IList<string>)ActualXValues)[index];
                    }

                case ChartValueType.TimeSpan:
                    {
                        return TimeSpan.FromMilliseconds(((IList<double>)ActualXValues)[index]);
                    }

                default:
                    return ((IList<double>)ActualXValues)[index];
            }
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
                if(circularSeries != null && !double.IsNaN(circularSeries.GroupTo))
                    indexes = (from adorment in Adornments
                               where Segments[index].Item == adorment.Item
                               select Adornments.IndexOf(adorment)).ToList();
                else if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed
                      && (this.IsSideBySide && (!(this is RangeSeriesBase))
                      && (!(this is FinancialSeriesBase)) && !(this is WaterfallSeries)))
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

#endregion

#region Protected Internal Virtual Methods
        /// <summary>
        /// Called when the chart mouse up.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="position">position</param>
        protected internal virtual void OnSeriesMouseUp(object source, Point position)
        {
        }

        /// <summary>
        /// Called when the chart mouse down.
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="position">position</param>
        protected internal virtual void OnSeriesMouseDown(object source, Point position)
        {
        }
        
        /// <summary>
        /// Method used to set SegmentSelectionBrush to selectedindex chartsegment.
        /// </summary>
        /// <param name="newIndex">new index</param>
        /// <param name="oldIndex">old index</param>
        protected internal virtual void SelectedIndexChanged(int newIndex, int oldIndex)
        {
            ChartSelectionChangedEventArgs chartSelectionChangedEventArgs;
            if (ActualArea != null && !ActualArea.GetEnableSeriesSelection() && ActualArea.SelectionBehaviour != null)
            {
                // Reset the oldIndex segment Interior
                if (!ActualArea.SelectionBehaviour.EnableMultiSelection)
                {
                    if (SelectedSegmentsIndexes.Contains(oldIndex))
                        SelectedSegmentsIndexes.Remove(oldIndex);

                    OnResetSegment(oldIndex);
                }

                if (IsItemsSourceChanged)
                {
                    return;
                }

                if (newIndex >= 0 && ActualArea.GetEnableSegmentSelection())
                {
                    if (!SelectedSegmentsIndexes.Contains(newIndex))
                        SelectedSegmentsIndexes.Add(newIndex);

                    // For adornment selection implementation
                    if (adornmentInfo is ChartAdornmentInfo && adornmentInfo.HighlightOnSelection)
                    {
                        UpdateAdornmentSelection(newIndex);
                    }

                    // Set the SegmentSelectionBrush to newIndex segment Interior
                    var segmentSelectableSeries = this as ISegmentSelectable;
                    if (newIndex < Segments.Count && segmentSelectableSeries != null && segmentSelectableSeries.SelectionBrush != null)
                    {
                        Segments[newIndex].BindProperties();
                        Segments[newIndex].IsSelectedSegment = true;
                    }

                    if (newIndex < Segments.Count)
                    {
                        chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                        {
                            SelectedSegment = Segments[newIndex],
                            SelectedSegments = ActualArea.SelectedSegments,
                            SelectedSeries = this,
                            SelectedIndex = newIndex,
                            PreviousSelectedIndex = oldIndex,
                            PreviousSelectedSegment = null,
                            NewPointInfo = Segments[newIndex].Item,
                            IsSelected = true
                        };

                        chartSelectionChangedEventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;

                        if (oldIndex >= 0 && oldIndex < Segments.Count)
                        {
                            chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[oldIndex];
                            chartSelectionChangedEventArgs.OldPointInfo = Segments[oldIndex].Item;
                        }

                        (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
                        PreviousSelectedIndex = newIndex;
                    }
                    else if (Segments.Count == 0)
                    {
                        triggerSelectionChangedEventOnLoad = true;
                    }
                }
                else if (newIndex == -1)
                {
                    chartSelectionChangedEventArgs = new ChartSelectionChangedEventArgs()
                    {
                        SelectedSegment = null,
                        SelectedSegments = ActualArea.SelectedSegments,
                        SelectedSeries = null,
                        SelectedIndex = newIndex,
                        PreviousSelectedIndex = oldIndex,
                        PreviousSelectedSegment = null,
                        PreviousSelectedSeries = this,
                        IsSelected = false
                    };

                    if (oldIndex != -1 && oldIndex < Segments.Count)
                    {
                        chartSelectionChangedEventArgs.PreviousSelectedSegment = Segments[oldIndex];
                        chartSelectionChangedEventArgs.OldPointInfo = Segments[oldIndex].Item;
                    }

                    (ActualArea as ChartBase).OnSelectionChanged(chartSelectionChangedEventArgs);
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
        protected internal virtual IChartTransformer CreateTransformer(Size size, bool create)
        {
            if (create || ChartTransformer == null)
            {
                ChartTransformer = ChartTransform.CreateCartesian(size, this);
            }

            return ChartTransformer;
        }

#endregion

#region Protected Internal Methods

        /// <summary>
        /// method declaration for generatepoints in Chartseries
        /// </summary>
        protected internal abstract void GeneratePoints();

        /// <summary>
        /// Return collection of double values
        /// </summary>
        /// <returns></returns>
        protected internal List<double> GetXValues()
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

#endregion

#region Protected Virtual Methods

        /// <summary>
        /// Set ToolTip duration.
        /// </summary>
        protected virtual void SetTooltipDuration()
        {
            int initialShowDelay = ChartTooltip.GetActualInitialShowDelay(ActualArea.TooltipBehavior, ChartTooltip.GetInitialShowDelay(this));
            int showDuration = ChartTooltip.GetActualShowDuration(ActualArea.TooltipBehavior, ChartTooltip.GetShowDuration(this));
            
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
                Timer.Interval = new TimeSpan(0, 0, 0, 0, showDuration);
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
        protected virtual void SetDoubleAnimation(ChartTooltip chartTooltip)
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
        /// Method implementation for generate points to given index.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="obj">object</param>
        /// <param name="replace">replace the data or not</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals", Justification = "Reviewed")]
        protected virtual void SetIndividualPoint(int index, object obj, bool replace)
        {
            if (SeriesYValues != null && YPaths != null && ItemsSource != null)
            {
                object xvalueType = GetArrayPropertyValue(obj, XComplexPaths);
                if (xvalueType != null)
                    XAxisValueType = GetDataType(xvalueType);
                if (IsMultipleYPathRequired)
                {
                    if (SegmentColorPath != null && IsColorPathSeries)
                        SetIndividualColorValue(index, obj, replace);
                    if (XAxisValueType == ChartValueType.String)
                    {
                        if (!(this.XValues is List<string>))
                            this.XValues = this.ActualXValues = new List<string>();
                        IList<string> xValue = this.XValues as List<string>;
                        string xVal = GetArrayPropertyValue(obj, XComplexPaths) as string;
                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == xVal)
                                isRepeatPoint = true;
                            else
                                xValue[index] = xVal;
                        }
                        else
                        {
                            xValue.Insert(index, xVal);
                        }

                        for (int i = 0; i < YPaths.Count(); i++)
                        {
                            var yVal = GetArrayPropertyValue(obj, YComplexPaths[i]);
                            YData = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            if (replace && SeriesYValues[i].Count > index)
                            {
                                if (SeriesYValues[i][index] == YData && isRepeatPoint)
                                    isRepeatPoint = true;
                                else
                                {
                                    SeriesYValues[i][index] = YData;
                                    isRepeatPoint = false;
                                }
                            }
                            else
                            {
                                SeriesYValues[i].Insert(index, YData);
                            }
                        }

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.Double ||
                       XAxisValueType == ChartValueType.Logarithmic)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        XData = Convert.ToDouble(xVal != null ? xVal : double.NaN);

                        // Check the Data Collection is linear or not
                        if (isLinearData && (index > 0 && XData < xValue[index - 1]) || (index == 0 && xValue.Count > 0 && XData > xValue[0]))
                        {
                            isLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        for (int i = 0; i < YPaths.Count(); i++)
                        {
                            var yVal = GetArrayPropertyValue(obj, YComplexPaths[i]);
                            YData = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            if (replace && SeriesYValues[i].Count > index)
                            {
                                if (SeriesYValues[i][index] == YData && isRepeatPoint)
                                    isRepeatPoint = true;
                                else
                                {
                                    SeriesYValues[i][index] = YData;
                                    isRepeatPoint = false;
                                }
                            }
                            else
                            {
                                SeriesYValues[i].Insert(index, YData);
                            }
                        }

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.DateTime)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        XData = Convert.ToDateTime(xVal).ToOADate();

                        // Check the Data Collection is linear or not
                        if (isLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            isLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        for (int i = 0; i < YPaths.Count(); i++)
                        {
                            var yVal = GetArrayPropertyValue(obj, YComplexPaths[i]);
                            YData = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            if (replace && SeriesYValues[i].Count > index)
                            {
                                if (SeriesYValues[i][index] == YData && isRepeatPoint)
                                    isRepeatPoint = true;
                                else
                                {
                                    SeriesYValues[i][index] = YData;
                                    isRepeatPoint = false;
                                }
                            }
                            else
                            {
                                SeriesYValues[i].Insert(index, YData);
                            }
                        }

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.TimeSpan)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        XData = ((TimeSpan)xVal).TotalMilliseconds;

                        // Check the Data Collection is linear or not
                        if (isLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            isLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        for (int i = 0; i < YPaths.Count(); i++)
                        {
                            var yVal = GetArrayPropertyValue(obj, YComplexPaths[i]);
                            YData = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            if (replace && SeriesYValues[i].Count > index)
                            {
                                if (SeriesYValues[i][index] == YData && isRepeatPoint)
                                    isRepeatPoint = true;
                                else
                                {
                                    SeriesYValues[i][index] = YData;
                                    isRepeatPoint = false;
                                }
                            }
                            else
                            {
                                SeriesYValues[i].Insert(index, YData);
                            }
                        }

                        dataCount = xValue.Count;
                    }
                }
                else
                {
                    string[] tempYPath = YComplexPaths[0];
                    IList<double> yValue = SeriesYValues[0];
                    if (SegmentColorPath != null && IsColorPathSeries)
                        SetIndividualColorValue(index, obj, replace);
                    if (XAxisValueType == ChartValueType.String)
                    {
                        if (!(this.XValues is List<string>))
                            this.XValues = this.ActualXValues = new List<string>();
                        IList<string> xValue = this.XValues as List<string>;
                        object xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        object yVal = GetArrayPropertyValue(obj, tempYPath);
                        YData = (yVal != null ? Convert.ToDouble(yVal) : double.NaN);
                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == Convert.ToString(xVal))
                                isRepeatPoint = true;
                            else
                                xValue[index] = Convert.ToString(xVal);
                        }
                        else
                        {
                            xValue.Insert(index, Convert.ToString(xVal));
                        }

                        if (replace && yValue.Count > index)
                        {
                            if (yValue[index] == YData && isRepeatPoint)
                                isRepeatPoint = true;
                            else
                            {
                                yValue[index] = YData;
                                isRepeatPoint = false;
                            }
                        }
                        else
                        {
                            yValue.Insert(index, YData);
                        }

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.Double ||
                        XAxisValueType == ChartValueType.Logarithmic)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        object yVal = GetArrayPropertyValue(obj, tempYPath);
                        XData = (xVal != null ? Convert.ToDouble(xVal) : double.NaN);
                        YData = (yVal != null ? Convert.ToDouble(yVal) : double.NaN);

                        // Check the Data Collection is linear or not
                        if (isLinearData && (index > 0 && XData < xValue[index - 1]) || (index == 0 && xValue.Count > 0 && XData > xValue[0]))
                        {
                            isLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        if (replace && yValue.Count > index)
                        {
                            if (yValue[index] == YData && isRepeatPoint)
                                isRepeatPoint = true;
                            else
                            {
                                yValue[index] = YData;
                                isRepeatPoint = false;
                            }
                        }
                        else
                        {
                            yValue.Insert(index, YData);
                        }

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.DateTime)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        object yVal = GetArrayPropertyValue(obj, tempYPath);
                        XData = Convert.ToDateTime(xVal).ToOADate();
                        YData = (yVal != null ? Convert.ToDouble(yVal) : double.NaN);

                        // Check the Data Collection is linear or not
                        if (isLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            isLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        if (replace && yValue.Count > index)
                        {
                            if (yValue[index] == YData && isRepeatPoint)
                                isRepeatPoint = true;
                            else
                            {
                                yValue[index] = YData;
                                isRepeatPoint = false;
                            }
                        }
                        else
                        {
                            yValue.Insert(index, YData);
                        }

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.TimeSpan)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        object yVal = GetArrayPropertyValue(obj, tempYPath);
                        XData = ((TimeSpan)xVal).TotalMilliseconds;
                        YData = (yVal != null ? Convert.ToDouble(yVal) : double.NaN);

                        // Check the Data Collection is linear or not
                        if (isLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            isLinearData = false;
                        }

                        if (xVal != null && replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else if (xVal != null)
                        {
                            xValue.Insert(index, XData);
                        }

                        if (replace && yValue.Count > index)
                        {
                            if (yValue[index] == YData && isRepeatPoint)
                                isRepeatPoint = true;
                            else
                            {
                                yValue[index] = YData;
                                isRepeatPoint = false;
                            }
                        }
                        else
                        {
                            yValue.Insert(index, YData);
                        }

                        dataCount = xValue.Count;
                    }
                }

                if (replace && ActualData.Count > index)
                    ActualData[index] = obj;
                else if (ActualData.Count == index)
                    ActualData.Add(obj);
                else
                    ActualData.Insert(index, obj);

                totalCalculated = false;
            }

            UpdateEmptyPoints(index);

            HookPropertyChangedEvent(ListenPropertyChange, obj);
        }

        /// <summary>
        /// Invoked when XBindingPath or YBindingPath properties changed.
        /// </summary>
        /// <param name="args">The <see cref="DependencyPropertyChangedEventArgs"/> that contains the event data</param>
        /// <see cref="ChartSeriesBase.XBindingPath"/>
        /// <see cref="XyDataSeries.YBindingPath"/>
        protected virtual void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
        {
            this.IsPointGenerated = false;
            canAnimate = true;
            
            if (ActualData != null)
            {
                ActualData.Clear();
            }
            
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

            // UWP-540 SeriesYValues is not cleared in case of IsSortData = true.
            if (IsSortData)
                SeriesYValues = null;

            totalCalculated = false;
            Segments.Clear();
            this.dataCount = 0;
            this.UpdateArea();
        }

        /// <summary>
        /// Method implementation for Set points to given index for data table.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="obj">object</param>
        /// <param name="replace">replace the data point or not</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals", Justification = "Reviewed")]
        protected virtual void SetIndividualDataTablePoint(int index, object obj, bool replace)
        {
            if (SeriesYValues != null && YPaths != null && ItemsSource != null)
            {
                var dataRow = obj as DataRow;
                if (IsMultipleYPathRequired)
                {
                    if (XAxisValueType == ChartValueType.String)
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

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.Double ||
                       XAxisValueType == ChartValueType.Logarithmic)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = dataRow.GetField(XBindingPath);

                        XData = Convert.ToDouble(xVal != null ? xVal : double.NaN);

                        // Check the Data Collection is linear or not
                        if (isLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            isLinearData = false;
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

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.DateTime)
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
                        if (isLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            isLinearData = false;
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

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.TimeSpan)
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
                        if (isLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            isLinearData = false;
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

                        dataCount = xValue.Count;
                    }
                }
                else
                {
                    string[] tempYPath = YComplexPaths[0];
                    IList<double> yValue = SeriesYValues[0];
                    if (XAxisValueType == ChartValueType.String)
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

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.Double ||
                        XAxisValueType == ChartValueType.Logarithmic)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = dataRow.GetField(XBindingPath);
                        object yVal = dataRow.GetField(tempYPath[0]);
                        XData = Convert.ToDouble(xVal != null ? xVal : double.NaN);

                        // Check the Data Collection is linear or not
                        if (isLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            isLinearData = false;
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

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.DateTime)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = dataRow.GetField(XBindingPath);
                        object yVal = dataRow.GetField(tempYPath[0]);
                        XData = Convert.ToDateTime(xVal).ToOADate();

                        // Check the Data Collection is linear or not
                        if (isLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            isLinearData = false;
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

                        dataCount = xValue.Count;
                    }
                    else if (XAxisValueType == ChartValueType.TimeSpan)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        IList<double> xValue = this.XValues as List<double>;
                        object xVal = dataRow.GetField(XBindingPath);
                        object yVal = dataRow.GetField(tempYPath[0]);
                        XData = ((TimeSpan)xVal).TotalMilliseconds;

                        // Check the Data Collection is linear or not
                        if (isLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            isLinearData = false;
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

                        dataCount = xValue.Count;
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
        /// Invoked when ItemsSource property changed.
        /// </summary>
        /// <param name="oldValue">Old itemssource collection.</param>
        /// <param name="newValue">New itemssource collection.</param>
        /// <seealso cref="ChartSeriesBase.XBindingPath"/>
        /// <seealso cref="XyDataSeries.YBindingPath"/>
        protected virtual void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
        }

        /// <summary>
        /// Method implementation for clear unused segments.
        /// </summary>
        /// <param name="startIndex"></param>
        protected virtual void ClearUnUsedSegments(int startIndex)
        {
            var emptySegments = new List<ChartSegment>();
            foreach (var segment in Segments.Where(item => item is EmptyPointSegment))
            {
                emptySegments.Add(segment);
            }

            foreach (var segment in emptySegments)
            {
                Segments.Remove(segment);
            }

            if (this.Segments.Count > startIndex)
            {
                int count = this.Segments.Count;

                for (int i = startIndex; i < count; i++)
                {
                    this.Segments.RemoveAt(startIndex);
                }
            }
        }

#endregion

#region Protected Override Methods
        /// <summary>
        /// Called when pointer or mouse pressed.
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
#if !NETFX_CORE
            OnSeriesMouseUp(e.OriginalSource, e.GetPosition(this));
#else
            OnSeriesMouseDown(e.OriginalSource, e.GetCurrentPoint(this).Position);
#endif
        }

        /// <summary>
        /// Called when mouse or pointer move to show the tooltip for series.
        /// </summary>
        /// <param name="e"> Event Arguments</param>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            var canvas = this.ActualArea.GetAdorningCanvas();
            mousePos = e.GetCurrentPoint(canvas).Position;
            if (e.Pointer.PointerDeviceType != PointerDeviceType.Touch && mousePos != previousMousePosition)
            {
                RemovePreviousSeriesTooltip();
                if (!GetAnimationIsActive() && !(this is ErrorBarSeries))
                {
                    UpdateTooltip(e.OriginalSource);
                }
            }
        }

#if NETFX_CORE
        /// <summary>
        /// Used to update the series tooltip when tapping on the series segments.
        /// </summary>
        /// <param name="e">TappedRoutedEventArgs</param>
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Touch)
            {
                if (!(this is ErrorBarSeries))
                    UpdateTooltip(e.OriginalSource);
            }

            base.OnTapped(e);
        }
#endif

        /// <summary>
        /// Called when pointer leave from segment.
        /// </summary>
        /// <param name="e">MouseEventArgs</param>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            if (!isTap)
            {
                ChartTooltipBehavior tooltipBehavior = ActualArea != null && ActualArea.TooltipBehavior != null ? ActualArea.TooltipBehavior : null;
                MousePointerExit(tooltipBehavior);
            }
            isTap = false;
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

#if NETFX_CORE
        /// <summary>
        /// When tap release it will fire.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            isTap = true;
            OnSeriesMouseUp(e.OriginalSource, e.GetCurrentPoint(this).Position);
        }
#endif

        /// <summary>
        /// Invoke to render chart series.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Set the Horizontal and Vertical Alignment for Tooltip.
        /// </summary>
        /// <param name="mousePos">Current Position</param>
        /// <param name="tooltip">Tooltip instance</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        protected Point Position(Point mousePos, ref ChartTooltip tooltip)
        {
            var tooltipBehavior = ActualArea.TooltipBehavior;
            double horizontalOffset = ChartTooltip.GetActualHorizontalOffset(tooltipBehavior, ChartTooltip.GetHorizontalOffset(this));
            double verticalOffset = ChartTooltip.GetActualVerticalOffset(tooltipBehavior, ChartTooltip.GetVerticalOffset(this));

            var newPostion = mousePos;
            if ((tooltip as UIElement).DesiredSize.Height == 0 || (tooltip as UIElement).DesiredSize.Width == 0)
                (tooltip as UIElement).UpdateLayout();

            HorizontalAlignment horizontalAlignment = ChartTooltip.GetActualHorizontalAlignment(tooltipBehavior, ChartTooltip.GetHorizontalAlignment(this));
            VerticalAlignment verticalAlignment = ChartTooltip.GetActualVerticalAlignment(tooltipBehavior, ChartTooltip.GetVerticalAlignment(this));

            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    newPostion.X = mousePos.X - (tooltip as UIElement).DesiredSize.Width - horizontalOffset;
                    break;
                case HorizontalAlignment.Center:
                    newPostion.X = mousePos.X - (tooltip as UIElement).DesiredSize.Width / 2 + horizontalOffset;
                    break;
                case HorizontalAlignment.Right:
                    newPostion.X = mousePos.X + horizontalOffset;
                    break;
            }

            switch (verticalAlignment)
            {
                case VerticalAlignment.Top:
                    newPostion.Y = mousePos.Y - (tooltip as UIElement).DesiredSize.Height - verticalOffset;
                    break;
                case VerticalAlignment.Center:
                    newPostion.Y = mousePos.Y - (tooltip as UIElement).DesiredSize.Height / 2 - verticalOffset;
                    break;
                case VerticalAlignment.Bottom:
                    newPostion.Y = mousePos.Y + verticalOffset;
                    break;
            }

            return newPostion;
        }

        /// <summary>
        /// Method implementation for UpdateArea.
        /// </summary>
        protected void UpdateArea()
        {
            if (ActualArea != null)
            {
                ActualArea.ScheduleUpdate();
            }
        }
        
        /// <summary>
        /// Return the previous series.
        /// </summary>
        /// <param name="series">ChartSeriesBase</param>
        /// <returns></returns>
        internal ChartSeriesBase GetPreviousSeries(ChartSeriesBase series)
        {
            int srIndex = this.ActualArea.VisibleSeries.IndexOf(series) - 1;

            if (srIndex == -1)
                return null;
            return this.ActualArea.VisibleSeries[srIndex];
        }

        /// <summary>
        /// Method used to generate data points for ChartSeries. 
        /// </summary>
        /// <param name="yPaths">Y paths</param>
        /// <param name="yValueLists">YValues list</param>
        protected void GeneratePoints(string[] yPaths, params IList<double>[] yValueLists)
        {
            IList<double>[] yLists = null;
            IsComplexYProperty = false;
            bool isArrayProperty = false;
            YComplexPaths = new string[yPaths.Count()][];
            for (int i = 0; i < yPaths.Count(); i++)
            {
                if (string.IsNullOrEmpty(yPaths[i]))
                {
                    if (this is RangeColumnSeries && !IsMultipleYPathRequired)
                    {
                        break;
                    }
                    else
                    {
                        return;
                    }
                }
                   
                YComplexPaths[i] = yPaths[i].Split('.');
                if (yPaths[i].Contains('.'))
                    IsComplexYProperty = true;
                if (yPaths[i].Contains('['))
                    isArrayProperty = true;
            }

            if (IsSortData)
            {
                if (SeriesYValues == null)
                    CreateYValueCollection(yPaths.Length);
                yLists = SeriesYValues;
                ActualSeriesYValues = yValueLists;
            }
            else
            {
                SeriesYValues = ActualSeriesYValues = yLists = yValueLists;
            }
            
            this.YPaths = yPaths;
            SeriesYCount = yPaths.Length;

            if (ItemsSource != null && !string.IsNullOrEmpty(XBindingPath))
            {
                if (ItemsSource is DataTable)
                    GenerateDataTablePoints(yPaths, yLists);
                else
                if (ItemsSource is IEnumerable)
                    if (XBindingPath.Contains('[') || isArrayProperty)
                        GenerateComplexPropertyPoints(yPaths, yLists, GetArrayPropertyValue);
                    else if (XBindingPath.Contains('.') || IsComplexYProperty)
                        GenerateComplexPropertyPoints(yPaths, yLists, GetPropertyValue);
                    else
                        GeneratePropertyPoints(yPaths, yLists);
            }

            ColorValues = new List<Brush>();
            if (ItemsSource != null && SegmentColorPath != null && IsColorPathSeries)
            {
                GenerateSegmentColor();
                if (ActualArea != null)
                {
                    ActualArea.IsUpdateLegend = true;
                }
            }

            if (ShowEmptyPoints)
            {
                ValidateDataPoints(SeriesYValues);
                isPointValidated = true;
            }

            if (IsSortData)
            {
                SortActualPoints();
            }
        }

        /// <summary>
        /// Called when ItemsSource property changed.
        /// </summary>
        /// <param name="args"></param>
        protected void OnDataSourceChanged(DependencyPropertyChangedEventArgs args)
        {
            canAnimate = true;

            var selectableSegmentSeries = this as ISegmentSelectable;
            if (selectableSegmentSeries != null && args.OldValue != null)
            {
                this.IsItemsSourceChanged = true;
                selectableSegmentSeries.SelectedIndex = -1;
                this.IsItemsSourceChanged = false;
            }

            // WRT-3858 - Emptypoint interior not cleared
            if (EmptyPointIndexes != null)
            {
                foreach (var item in EmptyPointIndexes)
                {
                    item.Clear();
                }
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

            // UWP-540 SeriesYValues is not cleared in case of IsSortData = true.
            if (IsSortData)
                SeriesYValues = null;

            if (args.OldValue is INotifyCollectionChanged)
            {
                (args.OldValue as INotifyCollectionChanged).CollectionChanged -= OnDataCollectionChanged;
            }

            UnHookPropertyChanged(args.OldValue);

            if (args.NewValue is INotifyCollectionChanged)
            {
                (args.NewValue as INotifyCollectionChanged).CollectionChanged += OnDataCollectionChanged;
            }
            //todo: Need to check the use of below  code. 
            totalCalculated = false;
            if (Segments != null)
                Segments.Clear();

            ClearAdornments();
            this.dataCount = 0;
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
                OnDataSourceChanged(args.OldValue as IEnumerable, args.NewValue as IEnumerable);
        }

        private void DataTableCleared(object sender, DataTableClearEventArgs e)
        {
            Refresh();
        }

        private void ClearAdornments()
        {
            var adornmentSeries = this as AdornmentSeries;
            if (adornmentSeries != null)
            {
                adornmentSeries.Adornments.Clear();
                adornmentSeries.VisibleAdornments.Clear();
                if (adornmentInfo != null)
                {
                    if (adornmentInfo.adormentContainers != null)
                        adornmentInfo.adormentContainers.Clear();
                    if (adornmentInfo.ConnectorLines != null)
                        adornmentInfo.ConnectorLines.Clear();
                    if (adornmentInfo.LabelPresenters != null)
                        adornmentInfo.LabelPresenters.Clear();
                }
            }
        }

#endregion

#region Private Static Methods

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
#if !NETFX_CORE
            Storyboard.SetTargetProperty(fadeInAnimation, new PropertyPath("Opacity"));
#else
            Storyboard.SetTargetProperty(fadeInAnimation, "Opacity");
#endif
            storyBoard1.Children.Add(fadeInAnimation);
            storyBoard1.Begin();
        }

        private static void OnTooltipTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var series = (ChartSeriesBase)d;
            if (series != null && series.ActualArea != null && series.ActualArea.Tooltip != null)
                ((ChartTooltip)series.ActualArea.Tooltip).ContentTemplate = args.NewValue as DataTemplate;
        }

        private static void OnListenPropertyChangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            (d as ChartSeriesBase).HookPropertyChangedEvent((bool)args.NewValue);
        }

        private static void OnShowTooltipChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var instance = (ChartSeriesBase)d;
            if (instance != null && instance.ActualArea != null && (bool)args.NewValue == true)
            {
                instance.ActualArea.Tooltip = new ChartTooltip();
            }
            else if (instance != null && instance.ActualArea != null && instance.ActualArea.Tooltip != null)
            {
                Canvas canvas = (instance.ActualArea as ChartBase).GetAdorningCanvas();

                if (canvas != null && canvas.Children.Contains((instance.ActualArea.Tooltip as ChartTooltip)))
                    canvas.Children.Remove(instance.ActualArea.Tooltip as ChartTooltip);
            }

            if (instance != null && instance.ActualArea != null && instance.ActualArea is ChartBase && (bool)args.NewValue == true)
            {
                AddTooltipBehavior((ChartBase)instance.ActualArea);
            }
        }

        internal static void AddTooltipBehavior(ChartBase chart)
        {
            if (chart != null && chart.Behaviors != null)
            {
                bool containsToolTipBehavior = false;
                foreach (ChartBehavior chartBehavior in chart.Behaviors)
                {
                    if (chartBehavior is ChartTooltipBehavior)
                    {
                        containsToolTipBehavior = true;
                        break;
                    }
                }

                if (!containsToolTipBehavior)
                {
                    var tooltipBehavior = new ChartTooltipBehavior();
                    tooltipBehavior.ChartArea = chart;
                    chart.Behaviors.Add(tooltipBehavior);
                }
            }
        }

        private static void OnSegmentSpacingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = (d as ChartSeriesBase);

            if (series.ActualArea != null)
                series.ActualArea.ScheduleUpdate();
        }

        private static void OnLabelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
        }

        private static void OnVisibilityOnLegendChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ChartSeriesBase)d;
            if (e.NewValue != null && (Visibility)e.NewValue == Visibility.Visible && instance != null && instance.ActualArea != null)
                instance.ActualArea.UpdateLegend(instance.ActualArea.Legend, false);
        }

        private static void OnSeriesSelectionBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ChartSeriesBase series = d as ChartSeriesBase;

            series.Segments.Clear();

            if (series.ActualArea != null)
                series.ActualArea.ScheduleUpdate();
        }

        private static void OnSegmentColorPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartSeriesBase series = d as ChartSeriesBase;
            if (series.IsColorPathSeries)
            {
                if (series.ActualArea != null)
                    series.ActualArea.IsUpdateLegend = true;
                if (series.ColorValues == null)
                    series.OnBindingPathChanged(e);
                else
                {
                    series.ColorValues.Clear();
                    if (series.SegmentColorPath != null)
                        series.GenerateSegmentColor();
                    series.Segments.Clear();
                    series.UpdateArea();
                }
            }
        }


        private static void OnIsSeriesVisibleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            (obj as ChartSeriesBase).IsSeriesVisibleChanged(args);
        }

        private static void OnColorModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSeriesBase).OnColorModelChanged();
        }

        private static void OnLegendIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSeriesBase).UpdateLegendIconTemplate(true);
        }

        private static void OnLegendIconTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSeriesBase).UpdateLegendIconTemplate(false);
        }

        private static void OnSortDataOrderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartSeriesBase series = d as ChartSeriesBase;
            if (series.IsPointGenerated)
            {
                if (e.Property == IsSortDataProperty)
                {
                    if (series.IsSortData)
                    {
                        series.CreateYValueCollection(series.YPaths.Length);

                        for (int i = 0; i < series.YPaths.Length; i++)
                        {
                            for (int j = 0; j < series.ActualSeriesYValues[i].Count; j++)
                            {
                                series.SeriesYValues[i].Add(series.ActualSeriesYValues[i][j]);
                            }
                        }

                        series.SortActualPoints();
                    }
                    else
                    {
                        for (int i = 0; i < series.YPaths.Length; i++)
                        {
                            series.ActualSeriesYValues[i].Clear();
                            for (int j = 0; j < series.SeriesYValues[i].Count; j++)
                            {
                                series.ActualSeriesYValues[i].Add(series.SeriesYValues[i][j]);
                            }
                        }

                        series.ActualXValues = series.XValues;
                    }
                }
                else
                {
                    if (series.IsSortData)
                        series.SortChartPoints();
                }

                series.UpdateArea();
            }
        }

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSeriesBase).OnPaletteChanged(e);
        }

        private static void OnEmptyPointStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSeriesBase).RevalidateEmptyPointsStyle();
        }

        private static void OnEmptyPointValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSeriesBase).RevalidateEmptyPointsValue();
        }

        private static void OnShowEmptyPointsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chartSeriesBase = d as ChartSeriesBase;
            chartSeriesBase.UpdateArea();
            if ((bool)e.NewValue && chartSeriesBase.SeriesYValues != null
                && !String.IsNullOrEmpty(chartSeriesBase.XBindingPath.ToString()) && chartSeriesBase.YPaths != null && chartSeriesBase.ItemsSource != null)
                chartSeriesBase.ValidateDataPoints(chartSeriesBase.SeriesYValues);
            if (d is AccumulationSeriesBase && chartSeriesBase.ActualArea != null)
                chartSeriesBase.ActualArea.IsUpdateLegend = true;
        }

        private static void OnEmptyPointInteriorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSeriesBase).RevalidateEmptyPointsValue();
        }

        private static object ReflectedObject(object parentObj, string actualPath)
        {
            IPropertyAccessor propertyAccessor = null;
            var propertyInfo = ChartDataUtils.GetPropertyInfo(parentObj, actualPath);
            if (propertyInfo != null)
                propertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo);
            if (propertyAccessor == null) return null;
            return propertyAccessor.GetValue(parentObj);
        }

        private static void OnBindingPathXChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var chartSeriesBase = obj as ChartSeriesBase;
            if (args.NewValue != null)
                chartSeriesBase.XComplexPaths = args.NewValue.ToString().Split('.');
            chartSeriesBase.OnBindingPathChanged(args);
        }

        private static void OnAppearanceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var chartSeriesBase = obj as ChartSeriesBase;
            chartSeriesBase.OnAppearanceChanged(chartSeriesBase);
        }

        private static void OnDataSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ChartSeriesBase series = obj as ChartSeriesBase;

            series.OnDataSourceChanged(args);
        }

#endregion

#region Private Methods

        void Refresh()
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

            if (EmptyPointIndexes != null && EmptyPointIndexes.Count() > 0)
            {
                // Clearing empty point indexes-UWP-650
                foreach (IList<int> list in EmptyPointIndexes)
                {
                    if (list != null)
                    {
                        list.Clear();
                    }
                }
            }


            if (this is ISegmentSelectable)
            {
                if (ActualArea.GetEnableSegmentSelection())
                    SelectedSegmentsIndexes.Clear();
                else
                    (this.ActualArea.SelectedSeriesCollection).Clear();
            }

            this.dataCount = 0;

            if (XBindingPath != null && YPaths != null && YPaths.Count() > 0)
            {
                GeneratePoints();
                Segments.Clear();
                ClearAdornments();
                UpdateArea();
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
            Timer.Stop();
        }

        private void OnActualTransposeChanged()
        {
            if (this is CartesianSeries && this.ActualXAxis != null && this.ActualYAxis != null)
            {
                this.ActualXAxis.Orientation = IsActualTransposed ? Orientation.Vertical : Orientation.Horizontal;
                this.ActualYAxis.Orientation = IsActualTransposed ? Orientation.Horizontal : Orientation.Vertical;
            }
        }

        private void IsSeriesVisibleChanged(DependencyPropertyChangedEventArgs args)
        {
            if (ActualArea != null)
            {
                var isPolarRadarSeriesBase = this is PolarRadarSeriesBase;
                if ((bool)args.NewValue)
                {
                    if (ActualArea.ActualSeries.Contains(this) && !ActualArea.VisibleSeries.Contains(this) && !isPolarRadarSeriesBase)
                    {
                        int pos = ActualArea.GetSeriesIndex(this);
                        int count = ActualArea.VisibleSeries.Count;
                        ActualArea.VisibleSeries.Insert(pos > count ? count : pos, this);
                    }

                    Visibility = Visibility.Visible;
                }
                else
                {
                    if (ActualArea.VisibleSeries.Contains(this) && !isPolarRadarSeriesBase)
                    {
                        ActualArea.VisibleSeries.Remove(this);
                    }

                    Visibility = Visibility.Collapsed;
                    RemoveTooltip();
                    Timer.Stop();
                }

                if (ActualArea.Legend != null)
                {
                    if (IsSingleAccumulationSeries)
                    {
                        foreach (var item in ((ActualArea.Legend as ChartLegend).Items))
                        {
                            var legendItem = item as LegendItem;
                            if (Visibility == Visibility.Visible)
                                legendItem.IsSeriesVisible = true;
                            else
                                legendItem.IsSeriesVisible = false;
                        }
                    }
                }

                ActualArea.SBSInfoCalculated = false;
                if (ActualArea is ChartBase)
                    (ActualArea as ChartBase).AddOrRemoveBitmap();
                UpdateArea();
            }
        }

        private void CalculateYValues(FinancialTechnicalIndicator technicalIndicator, int i, out List<double> y1, out List<double> y2, double x)
        {
            y1 = new List<double>();
            y2 = new List<double>();

            for (int j = 0; j < ActualSeriesYValues.Count(); j++)
            {
                var seriesYValue = ActualSeriesYValues[j][i];
                y1.Add(seriesYValue);
                
                if (this is RangeColumnSeries && !IsMultipleYPathRequired)
                {
                   break;
                }
            }

            if (technicalIndicator != null)
                foreach (ChartSegment segment in technicalIndicator.Segments)
                {
                    var fastColumnBitmapSegment = segment as FastColumnBitmapSegment;
                    if (fastColumnBitmapSegment != null)
                    {
                        if (i < fastColumnBitmapSegment.y1ChartVals.Count)
                            y2.Add(fastColumnBitmapSegment.y1ChartVals[i]);
                    }
                    else
                    {
                        var technicalIndicatorSement = segment as TechnicalIndicatorSegment;
                        if (i < technicalIndicatorSement.yChartVals.Count)
                        {
                            int length = technicalIndicatorSement.Length;

                            if (length == 0 ? x < technicalIndicatorSement.xChartVals[i]
                                : i < technicalIndicatorSement.Length - 1)
                                y2.Add(double.NaN);
                            else
                            {
                                var indicatorYValue = technicalIndicatorSement.yChartVals[i];
                                y2.Add(indicatorYValue);
                            }
                        }
                    }
                }
        }

        private void OnColorModelChanged()
        {
            if (ColorModel != null)
            {
                ColorModel.Palette = Palette;
                ColorModel.Series = this;
            }

            if (this.ActualArea != null && Palette == ChartColorPalette.Custom)
            {
                this.Segments.Clear();
                ActualArea.IsUpdateLegend = true;
                UpdateArea();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        private void OnPaletteChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ColorModel != null)
                ColorModel.Palette = this.Palette;

            if (SegmentColorPath != null && IsColorPathSeries && ColorValues != null)
            {
                ColorValues.Clear();
                GenerateSegmentColor();
            }

            if (this.ActualArea != null)
            {
                this.Segments.Clear();
                ActualArea.IsUpdateLegend = true;
                UpdateArea();
            }
        }

        private void RevalidateEmptyPointsStyle()
        {
            if (Segments.Count > 0)
            {
                UpdateArea();
            }
        }


        private void CreateYValueCollection(int count)
        {
            SeriesYValues = new IList<double>[count];
            for (int i = 0; i < count; i++)
            {
                SeriesYValues[i] = new List<double>();
            }
        }

        private void GenerateSegmentColor()
        {
            IEnumerator enumerator = (ItemsSource as IEnumerable).GetEnumerator();
            PropertyInfo segmentColorPropertInfo;
            if (enumerator.MoveNext())
            {
                IPropertyAccessor segmentColorPropertyAccessor = null;
                segmentColorPropertInfo = ChartDataUtils.GetPropertyInfo(enumerator.Current, SegmentColorPath);
                if (segmentColorPropertInfo != null)
                    segmentColorPropertyAccessor = FastReflectionCaches.PropertyAccessorCache.Get(segmentColorPropertInfo);
                if (segmentColorPropertyAccessor == null) return;
                Func<object, object> colorGetMethod = segmentColorPropertyAccessor.GetMethod;
                do
                {
                    object colVal = colorGetMethod(enumerator.Current);
                    ColorValues.Add(colVal != null ? (Brush)colVal : null);
                }
                while (enumerator.MoveNext());
            }
        }

        private void SortActualData()
        {
            if (ItemsSource != null)
            {
                IEnumerable dataCollection = (ActualData as IEnumerable);
                var data = ActualData.ToList();
                var xyDataSeries = this as XyDataSeries;

                IList actualXValues = null;
                IList actualYValues = null;

                if (XValues is IList<string>)
                {
                    actualXValues = ActualXValues as List<string>;
                }
                else
                {
                    actualXValues = ActualXValues as List<double>;
                }

                actualYValues = ActualSeriesYValues[0] as List<double>;

                foreach (var obj in dataCollection)
                {
                    var parentXObj = ReflectedObject(obj, XBindingPath);
                    var parentYObj = xyDataSeries != null ? ReflectedObject(obj, xyDataSeries.YBindingPath) : null;

                    for (int i = 0; i < actualXValues.Count; i++)
                    {
                        if ((parentXObj != null && actualXValues[i].Equals(parentXObj))
                            && (xyDataSeries == null || (parentYObj != null && actualYValues[i].Equals(parentYObj))))
                        {
                            data.RemoveAt(i);
                            data.Insert(i, obj);
                            break;
                        }
                    }
                }

                ActualData = data;
            }
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

        private void SortActualPoints()
        {
            if (XValues is IList<double>)
            {
                ActualXValues = new List<double>();

                List<double> actualXValues = ActualXValues as List<double>;
                List<double> xValues = XValues as List<double>;

                for (int i = 0; i < DataCount; i++)
                {
                    actualXValues.Add(xValues[i]);
                }
            }
            else
            {
                ActualXValues = new List<string>();

                List<string> actualXValues = ActualXValues as List<string>;
                List<string> xValues = XValues as List<string>;

                for (int i = 0; i < DataCount; i++)
                {
                    actualXValues.Add(xValues[i]);
                }
            }

            if (YPaths != null)
            {
                for (int i = 0; i < YPaths.Length; i++)
                {
                    ActualSeriesYValues[i].Clear();
                    for (int j = 0; j < SeriesYValues[i].Count; j++)
                    {
                        ActualSeriesYValues[i].Add(SeriesYValues[i][j]);
                    }
                }
            }

            SortChartPoints();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void OnItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (isNotificationSuspended)
            {
                isPropertyNotificationSuspended = true;
                return;
            }

            if (IsComplexYProperty || XBindingPath.Contains('.'))
            {
                ComplexPropertyChanged(sender, e);
            }
            else
                if (XBindingPath == e.PropertyName
                    || YPaths != null && YPaths.Contains(e.PropertyName) || SegmentColorPath == e.PropertyName
                    || (this is WaterfallSeries && (this as WaterfallSeries).SummaryBindingPath == e.PropertyName))
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

                    if (IsSortData)
                    {
                        SortActualPoints();
                    }

                    //WPF-53274 Legend item label does not update properly while changing the X-Value dynamically in PieSeries
                    if (this is AccumulationSeriesBase && this.ActualArea != null)
                    {
                        this.ActualArea.IsUpdateLegend = true;
                    }

                    if (!isRepeatPoint || isSegmentColorChanged)
                        this.UpdateArea();
                    isSegmentColorChanged = false;
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
                || isZPath)
            {
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

                    if (IsSortData)
                    {
                        SortActualPoints();
                    }

                    this.UpdateArea();
                }
            }
        }

        /// <summary>
        /// Sorts the Chart Points 
        /// </summary>
        /// <returns></returns>
        private void SortChartPoints()
        {
            if (xValueType != ChartValueType.String)
            {
                var xValues = ActualXValues as List<double>;
                GetTuple(xValues);
            }
            else
            {
                var xValues = ActualXValues as List<string>;
                GetTuple(xValues);
            }

            SortActualData();
        }

        private void GetTuple<T>(List<T> xValues) where T : IComparable<T>
        {
            switch (this.SeriesYCount)
            {
                case 1:
                    List<Tuple<T, double>> pair = new List<Tuple<T, double>>();
                    var y0Values = ActualSeriesYValues[0] as List<double>;
                    for (int j = 0; j < DataCount; j++)
                    {
                        pair.Add(Tuple.Create<T, double>(xValues[j], y0Values[j]));
                    }

                    if (pair.Count > 0)
                    {
                        this.Sort<T, double>(pair);
                    }

                    break;

                case 2:
                    List<Tuple<T, double, double>> triple = new List<Tuple<T, double, double>>();
                    var y00Values = ActualSeriesYValues[0] as List<double>;
                    var y1Values = ActualSeriesYValues[1] as List<double>;
                    for (int j = 0; j < DataCount; j++)
                    {
                        triple.Add(Tuple.Create<T, double, double>(xValues[j], y00Values[j], y1Values[j]));
                    }

                    if (triple.Count > 0)
                    {
                        this.Sort<T, double, double>(triple);
                    }

                    break;

                case 4:
                    List<Tuple<T, double, double, double, double>> quintuple = new List<Tuple<T, double, double, double, double>>();
                    var y01Values = ActualSeriesYValues[0] as List<double>;
                    var y11Values = ActualSeriesYValues[1] as List<double>;
                    var y2Values = ActualSeriesYValues[2] as List<double>;
                    var y3Values = ActualSeriesYValues[3] as List<double>;
                    for (int j = 0; j < DataCount; j++)
                    {
                        quintuple.Add(Tuple.Create<T, double, double, double, double>(xValues[j], y01Values[j], y11Values[j], y2Values[j], y3Values[j]));
                    }

                    if (quintuple.Count > 0)
                    {
                        this.Sort<T, double, double, double, double>(quintuple);
                    }

                    break;

                case 5:
                    List<Tuple<T, double, double, double, double, double>> hextuple = new List<Tuple<T, double, double, double, double, double>>();
                    var y02Values = ActualSeriesYValues[0] as List<double>;
                    var y12Values = ActualSeriesYValues[1] as List<double>;
                    var y20Values = ActualSeriesYValues[2] as List<double>;
                    var y30Values = ActualSeriesYValues[3] as List<double>;
                    var y40Values = ActualSeriesYValues[4] as List<double>;
                    for (int j = 0; j < DataCount; j++)
                    {
                        hextuple.Add(Tuple.Create<T, double, double, double, double, double>(xValues[j], y02Values[j], y12Values[j], y20Values[j], y30Values[j], y40Values[j]));
                    }

                    if (hextuple.Count > 0)
                    {
                        this.Sort<T, double, double, double, double, double>(hextuple);
                    }

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Sort the ActualXValues and ActualYValues 
        /// </summary>
        /// <typeparam name="T">XValues</typeparam>
        /// <typeparam name="T1">YValues</typeparam>
        /// <param name="list"></param>
        private void Sort<T, T1>(List<Tuple<T, T1>> list)
            where T : IComparable<T>
            where T1 : IComparable<T1>
        {
            switch (this.SortBy)
            {
                case SortingAxis.X:
                    {
                        list.Sort((t1, t2) => t1.Item1.CompareTo(t2.Item1));
                        ActualSort<T, T1>(list);
                    }

                    break;

                case SortingAxis.Y:
                    {
                        list.Sort((t1, t2) => t1.Item2.CompareTo(t2.Item2));
                        ActualSort<T, T1>(list);
                    }

                    break;

                default:
                    break;
            }
        }

        private void ActualSort<T, T1>(List<Tuple<T, T1>> list)
        {
            ActualXValues = (from x in list select x.Item1).ToList<T>();
            if (SortDirection == Direction.Descending)
                (ActualXValues as List<T>).Reverse();

            if (ActualSeriesYValues != null)
            {
                var y1 = ActualSeriesYValues[0] as List<T1>;
                int k = 0;
                foreach (var item in list)
                {
                    y1[k] = item.Item2;
                    k++;
                }

                if (SortDirection == Direction.Descending)
                {
                    y1.Reverse();
                }
            }
        }

        /// <summary>
        /// Sort the ActualXValues and ActualYValues 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="list"></param>
        private void Sort<T, T1, T2>(List<Tuple<T, T1, T2>> list)
            where T : IComparable<T>
            where T1 : IComparable<T1>
            where T2 : IComparable<T2>
        {
            switch (this.SortBy)
            {
                case SortingAxis.X:
                    {
                        list.Sort((t1, t2) => t1.Item1.CompareTo(t2.Item1));
                        ActualSort<T, T1, T2>(list);
                    }

                    break;

                case SortingAxis.Y:
                    {
                        list.Sort((t1, t2) => t1.Item2.CompareTo(t2.Item2));
                        ActualSort<T, T1, T2>(list);
                    }

                    break;

                default:
                    break;
            }
        }

        private void ActualSort<T, T1, T2>(List<Tuple<T, T1, T2>> list)
        {
            ActualXValues = (from x in list select x.Item1).ToList<T>();
            if (SortDirection == Direction.Descending)
                (ActualXValues as List<T>).Reverse();

            if (ActualSeriesYValues != null)
            {
                var y1 = ActualSeriesYValues[0] as List<T1>;
                var y2 = ActualSeriesYValues[1] as List<T2>;
                int k = 0;
                foreach (var item in list)
                {
                    y1[k] = item.Item2;
                    y2[k] = item.Item3;
                    k++;
                }

                if (SortDirection == Direction.Descending)
                {
                    y1.Reverse();
                    y2.Reverse();
                }
            }
        }

        /// <summary>
        /// Sort the ActualXValues and ActualYValues
        /// </summary>
        /// <typeparam name="T">XValues</typeparam>
        /// <typeparam name="T1">Y0Values</typeparam>
        /// <typeparam name="T2">Y1Values</typeparam>
        /// <typeparam name="T3">Y2Values</typeparam>
        /// <typeparam name="T4">Y3Values</typeparam>
        /// <param name="list"></param>
        private void Sort<T, T1, T2, T3, T4>(List<Tuple<T, T1, T2, T3, T4>> list)
            where T : IComparable<T>
            where T1 : IComparable<T1>
            where T2 : IComparable<T2>
            where T3 : IComparable<T3>
            where T4 : IComparable<T4>
        {
            switch (this.SortBy)
            {
                case SortingAxis.X:
                    {
                        list.Sort((t1, t2) => t1.Item1.CompareTo(t2.Item1));
                        ActualSort<T, T1, T2, T3, T4>(list);
                    }

                    break;

                case SortingAxis.Y:
                    {
                        list.Sort((t1, t2) => t1.Item2.CompareTo(t2.Item2));
                        ActualSort<T, T1, T2, T3, T4>(list);
                    }

                    break;

                default:
                    break;
            }
        }

        private void ActualSort<T, T1, T2, T3, T4>(List<Tuple<T, T1, T2, T3, T4>> list)
        {
            ActualXValues = (from x in list select x.Item1).ToList<T>();
            if (SortDirection == Direction.Descending)
                (ActualXValues as List<T>).Reverse();

            if (ActualSeriesYValues != null)
            {
                var y1 = ActualSeriesYValues[0] as List<T1>;
                var y2 = ActualSeriesYValues[1] as List<T2>;
                int k = 0;
                foreach (var item in list)
                {
                    (ActualSeriesYValues[0] as List<T1>)[k] = item.Item2;
                    (ActualSeriesYValues[1] as List<T2>)[k] = item.Item3;
                    (ActualSeriesYValues[2] as List<T3>)[k] = item.Item4;
                    (ActualSeriesYValues[3] as List<T4>)[k] = item.Item5;
                    k++;
                }

                if (SortDirection == Direction.Descending)
                {
                    y1.Reverse();
                    y2.Reverse();
                }
            }
        }

        private void Sort<T, T1, T2, T3, T4, T5>(List<Tuple<T, T1, T2, T3, T4, T5>> list)
            where T : IComparable<T>
            where T1 : IComparable<T1>
            where T2 : IComparable<T2>
        {
            switch (this.SortBy)
            {
                case SortingAxis.X:
                    {
                        list.Sort((t1, t2) => t1.Item1.CompareTo(t2.Item1));
                        ActualSort<T, T1, T2, T3, T4, T5>(list);
                    }

                    break;

                case SortingAxis.Y:
                    {
                        list.Sort((t1, t2) => t1.Item2.CompareTo(t2.Item2));
                        ActualSort<T, T1, T2, T3, T4, T5>(list);
                    }

                    break;

                default:
                    break;
            }
        }

        private void ActualSort<T, T1, T2, T3, T4, T5>(List<Tuple<T, T1, T2, T3, T4, T5>> list)
        {
            ActualXValues = (from x in list select x.Item1).ToList<T>();
            if (SortDirection == Direction.Descending)
                (ActualXValues as List<T>).Reverse();

            if (ActualSeriesYValues != null)
            {
                var y1 = ActualSeriesYValues[0] as List<T1>;
                var y2 = ActualSeriesYValues[1] as List<T2>;
                var y3 = ActualSeriesYValues[1] as List<T3>;
                var y4 = ActualSeriesYValues[1] as List<T4>;
                var y5 = ActualSeriesYValues[1] as List<T5>;
                int k = 0;
                foreach (var item in list)
                {
                    y1[k] = item.Item2;
                    y2[k] = item.Item3;
                    y3[k] = item.Item4;
                    y4[k] = item.Item5;
                    y5[k] = item.Item6;
                    k++;
                }

                if (SortDirection == Direction.Descending)
                {
                    y1.Reverse();
                    y2.Reverse();
                    y3.Reverse();
                    y4.Reverse();
                    y5.Reverse();
                }
            }
        }

        private void OnAppearanceChanged(ChartSeriesBase obj)
        {
            if (IsBitmapSeries || (SegmentColorPath != null && IsColorPathSeries))
                obj.UpdateArea();
            else if (obj.adornmentInfo != null)
            {
                obj.adornmentInfo.UpdateLabels(); // WPF-19938 - UseSeriesPalette not updated when series interior is changed
                obj.adornmentInfo.UpdateConnectingLines();

                //XAMARIN-33164 - Chart DataMarker (Marker/Symbol) color not applying properly when dynamically change the series color
                if (obj.adornmentInfo.UseSeriesPalette && obj.adornmentInfo.IsAdornmentLabelCreatedEventHooked)
                {
                    foreach (ChartAdornment adornment in obj.Adornments)
                    {
                        if (adornment.CustomAdornmentLabel != null)
                        {
                            adornment.CustomAdornmentLabel.MarkerInterior = obj.Interior;
                            adornment.CustomAdornmentLabel.MarkerStroke = obj.Interior;
                        }
                    }
                }
            }
        }

        private void SetIndividualColorValue(int newStartingIndex, object obj, bool isReplace)
        {
            var colorVal = ReflectedObject(obj, this.SegmentColorPath) as Brush;
            if (isReplace)
            {
                if (ColorValues[newStartingIndex] == colorVal)
                    isSegmentColorChanged = false;
                else
                {
                    ColorValues[newStartingIndex] = colorVal;
                    isSegmentColorChanged = true;
                }
            }
            else
                ColorValues.Insert(newStartingIndex, colorVal != null ? colorVal : null);
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

        private void RevalidateEmptyPointsCollection(NotifyCollectionChangedAction action, int newIndex, int oldIndex)
        {
            if (EmptyPointIndexes != null)
            {
                foreach (var index in EmptyPointIndexes[0])
                {
                    if (Segments.Count > index)
                        Segments[index].IsEmptySegmentInterior = false;
                }

                switch (action)
                {
                    case NotifyCollectionChangedAction.Replace:
                        if (double.IsNaN(SeriesYValues[0][newIndex]))
                        {
                            if (!(EmptyPointIndexes[0].Contains(newIndex)))
                            {
                                EmptyPointIndexes[0].Add(newIndex);
                            }
                        }
                        else if (EmptyPointIndexes[0].Contains(newIndex))
                        {
                            EmptyPointIndexes[0].Remove(newIndex);
                        }

                        break;

                    case NotifyCollectionChangedAction.Add:
                        if (double.IsNaN(SeriesYValues[0][newIndex]))
                        {
                            EmptyPointIndexes[0].Add(newIndex);
                        }

                        break;

                    case NotifyCollectionChangedAction.Remove:
                        var postIndex = new List<int>();
                        foreach (var items in EmptyPointIndexes)
                        {
                            if (items.Contains(oldIndex))
                            {
                                items.Remove(oldIndex);
                            }

                            foreach (var value in items.Where(item => item > oldIndex))
                            {
                                postIndex.Add(items.IndexOf(value));
                            }

                            foreach (var index in postIndex)
                            {
                                items[index] = --items[index];
                            }

                            postIndex.Clear();
                        }

                        break;
                }
            }

            RevalidateEmptyPointsValue();
        }

        private void RevalidateEmptyPointsValue()
        {
            if (Segments != null && Segments.Count > 0)
            {
                var yValues = SeriesYValues;

                var pathIndex = 0;
                if (EmptyPointIndexes != null)
                    foreach (var items in EmptyPointIndexes)
                    {
                        switch (EmptyPointValue)
                        {
                            case EmptyPointValue.Zero:
                                foreach (var index in items)
                                {
                                    yValues[pathIndex][index] = 0;
                                }

                                break;

                            case EmptyPointValue.Average:
                                var currYValues = yValues[pathIndex];
                                double x, y;
                                var sortedItems = from val in items
                                                  orderby val ascending
                                                  select val;
                                foreach (var index in sortedItems)
                                {
                                    if (index == 0 && currYValues.Count > 1)
                                    {
                                        x = double.IsNaN(currYValues[1]) ? 0 : currYValues[1];
                                        currYValues[0] = x / 2;
                                    }
                                    else if (index == currYValues.Count - 1 && currYValues.Count > 1)
                                    {
                                        x = double.IsNaN(currYValues[index - 1]) ? 0 : currYValues[index - 1];
                                        currYValues[index] = x / 2;
                                    }
                                    else if (index == 0 && currYValues.Count == 1)
                                    {
                                        // Dynamically remove the single empty point data.
                                        x = double.IsNaN(currYValues[0]) ? 0 : currYValues[0];
                                        currYValues[index] = x / 2;
                                    }
                                    else
                                    {
                                        x = double.IsNaN(currYValues[index - 1]) ? 0 : currYValues[index - 1];
                                        y = double.IsNaN(currYValues[index + 1]) ? 0 : currYValues[index + 1];
                                        currYValues[index] = (x + y) / 2;
                                    }
                                }

                                break;
                        }

                        pathIndex++;
                    }
            }

            UpdateArea();
        }

#endregion

#endregion
    }

    /// <summary>
    /// Define the members that used in SfChart data.
    /// </summary>
    internal class ChartDataPointInfo : ChartSegment
    {
#region Fields

#region Public Fields

        /// <summary>
        /// Define the index of the data point.
        /// </summary>
        public int Index = -1;

#endregion

#endregion

#region Properties

#region Public Properties

        /// <summary>
        /// Gets or sets the x-axis data of the series data point.
        /// </summary>
        public double XData { get; set; }

        /// <summary>
        /// Gets or sets the y-axis data of the XY data series data point.
        /// </summary>
        public double YData { get; set; }

        /// <summary>
        /// Gets or sets the high value of the range/financial series data point.
        /// </summary>
        public double High { get; set; }

        /// <summary>
        /// Gets or sets the low value of the range/financial series data point.
        /// </summary>
        public double Low { get; set; }

        /// <summary>
        /// Gets or sets the Open value of the financial series data point.
        /// </summary>
        public double Open { get; set; }

        /// <summary>
        /// Gets or sets the close value of the financial series data point.
        /// </summary>
        public double Close { get; set; }

#endregion

#endregion

#region Public Override Methods

        /// <summary>
        /// Method implementation for CreateVisual method.
        /// </summary>
        /// <param name="size">Size</param>
        /// <returns>UIElement</returns>
        public override UIElement CreateVisual(Size size)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method implementation for GetRenderedVisual method.
        /// </summary>
        /// <returns>UIElement</returns>
        public override UIElement GetRenderedVisual()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method implementation for Update method.
        /// </summary>
        /// <param name="transformer"></param>
        public override void Update(IChartTransformer transformer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method implementation for OnSizeChanged method.
        /// </summary>
        /// <param name="size">Size</param>
        public override void OnSizeChanged(Size size)
        {
            throw new NotImplementedException();
        }

#endregion
    }
}
