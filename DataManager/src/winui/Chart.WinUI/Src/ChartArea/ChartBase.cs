using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
#if WinUI_Desktop
using System.ComponentModel;
#endif
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Windows.ApplicationModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using Windows.UI;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes")]
    static class NamespaceDoc
    {

    }
    
    /// <summary>
    /// The <see cref="ChartBase"/> class is the base for <see cref="SfCartesianChart"/>, <see cref="SfCircularChart"/>, <see cref="SfFunnelChart"/>, <see cref="SfPyramidChart"/>, and <see cref="SfPolarChart"/> types.
    /// </summary>
    public abstract partial class ChartBase : Control, IDisposable
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartBase"/> class.
        /// </summary>
        public ChartBase()
        {
#if NETCORE
#if SyncfusionLicense
           if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
               LicenseHelper.ValidateLicense();
            }
#endif
#endif
            UpdateAction = UpdateAction.Invalidate;
            VisibleSeries = new ChartVisibleSeriesCollection();
            InternalAxes = new ChartAxisCollection();
           
            DependentSeriesAxes = new List<ChartAxis>();
            InternalAxes.CollectionChanged += Axes_CollectionChanged;
            Behaviors = new ChartBehaviorsCollection(this);
        }

        #endregion

        #region Dependency Property Registrations

        /// <summary>
        /// The DependencyProperty for <see cref="Legend"/> property.
        /// </summary>
        public static readonly DependencyProperty LegendProperty =
            DependencyProperty.Register(nameof(Legend), typeof(ChartLegend), typeof(ChartBase), new PropertyMetadata(null, OnLegendChanged));


        /// <summary>
        /// The DependencyProperty for <see cref="AxisThickness"/> property.
        /// </summary>        
        internal static readonly DependencyProperty AxisThicknessProperty =
            DependencyProperty.Register(
                "AxisThickness",
                typeof(Thickness),
                typeof(ChartBase),
                new PropertyMetadata(new Thickness().GetThickness(0, 0, 0, 0)));

        /// <summary>
        /// The DependencyProperty for Row property.
        /// </summary>
        internal static readonly DependencyProperty RowProperty =
            DependencyProperty.RegisterAttached(
                "Row",
                typeof(int), 
                typeof(ChartBase),
                new PropertyMetadata(0));

        /// <summary>
        /// The DependencyProperty for ColumnSpan property.
        /// </summary>
        internal static readonly DependencyProperty ColumnSpanProperty =
            DependencyProperty.RegisterAttached(
                "ColumnSpan",
                typeof(int),
                typeof(ChartBase),
                new PropertyMetadata(1));

        /// <summary>
        /// The DependencyProperty for RowSpan property.
        /// </summary>
        internal static readonly DependencyProperty RowSpanProperty =
            DependencyProperty.RegisterAttached(
                "RowSpan", 
                typeof(int), 
                typeof(ChartBase),
                new PropertyMetadata(1));

        /// <summary>
        /// The DependencyProperty for Column" property.
        /// </summary>
        internal static readonly DependencyProperty ColumnProperty =
            DependencyProperty.RegisterAttached(
                "Column", 
                typeof(int),
                typeof(ChartBase),
                new PropertyMetadata(0));

        /// <summary>
        /// The DependencyProperty for <see cref="VisibleSeries"/> property.
        /// </summary>
        public static readonly DependencyProperty VisibleSeriesProperty =
            DependencyProperty.Register(
                "VisibleSeries", 
                typeof(ChartVisibleSeriesCollection),
                typeof(ChartBase), 
                new PropertyMetadata(null));
      
        /// <summary>
        /// The DependencyProperty for <see cref="Header"/> property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(ChartBase), new PropertyMetadata(null));

        /// <summary>
        /// The DependencyProperty for <see cref="HorizontalHeaderAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty HorizontalHeaderAlignmentProperty =
            DependencyProperty.Register(
                "HorizontalHeaderAlignment",
                typeof(HorizontalAlignment), 
                typeof(ChartBase),
                new PropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// The DependencyProperty for <see cref="VerticalHeaderAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty VerticalHeaderAlignmentProperty =
            DependencyProperty.Register(
                "VerticalHeaderAlignment",
                typeof(VerticalAlignment), 
                typeof(ChartBase),
                new PropertyMetadata(VerticalAlignment.Center));

        /// <summary>
        /// The DependencyProperty for <see cref="Tooltip"/> property.
        /// </summary>
        internal static readonly DependencyProperty TooltipProperty =
            DependencyProperty.Register("Tooltip", typeof(ChartTooltip), typeof(ChartBase), new PropertyMetadata(null));

       

        /// <summary>
        /// The DependencyProperty for <see cref="TooltipBehavior"/> property.
        /// </summary>
        public static readonly DependencyProperty TooltipBehaviorProperty =
            DependencyProperty.Register(
                nameof(TooltipBehavior),
                typeof(ChartTooltipBehavior),
                typeof(ChartBase),
                new PropertyMetadata(null, OnBehaviorChanged));

        #endregion

        #region Field

        #region Internal Fields


#if WinUI_Desktop
        internal bool isRenderSeriesDispatched = false;

#else
        internal IAsyncAction renderSeriesAction;

         internal IAsyncAction updateAreaAction;
#endif

        internal Panel chartAxisPanel;

        internal int currentBitmapPixel = -1;

        internal Point adorningCanvasPoint;

        internal bool isBitmapPixelsConverted;

        internal bool HoldUpdate;

        internal ChartZoomPanBehavior chartZoomBehavior;

        internal WriteableBitmap fastRenderSurface;

        internal double InternalDoughnutHoleSize = 0.5;

        internal Canvas AdorningCanvas;

        internal bool triggerSelectionChangedEventOnLoad;

        internal bool ActualEnableTooltip
        {
            get;
            set;
        }

        internal bool IsTemplateApplied;

        internal List<ChartSeries> ActualSeries = new List<ChartSeries>();

        internal List<ChartSeries> SelectedSeriesCollection = new List<ChartSeries>();

        internal ChartSeries CurrentSelectedSeries, PreviousSelectedSeries;

        #endregion

        #region Private Fields

        private Panel gridLinesPanel;

        private bool clearPixels;

        private Panel seriesPresenter;

        List<double> sumItems = new List<double>();

        private ChartSeries previousSeries; //This field used in UpdateBitmapTooltip for storing the series as temp.

        private IList rowBorderLines, columnBorderLines;

#if NETFX_CORE
        bool isTap;
#endif

        private ChartBehaviorsCollection behaviors;

        private ChartPlotArea plotArea;

        private Image fastRenderDevice = new Image();

        private byte[] fastBuffer;

        private Stream fastRenderSurfaceStream;

        private ChartRowDefinitions rowDefinitions;
        
        private ChartColumnDefinitions columnDefinitions;

        private ILayoutCalculator gridLinesLayout;

        private Rect seriesClipRect;

        private ILayoutCalculator chartAxisLayoutPanel;
        
        private double m_minPointsDelta = double.NaN;

        private bool isSbsWithOneData;
        
        private Size? rootPanelDesiredSize;

        private ChartAreaType areaType = ChartAreaType.CartesianAxes;

        private Dictionary<object, int> seriesPosition = new Dictionary<object, int>();

        private bool isLoading = true;

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Event correspond to plot area bound. It invokes when the plot area size changes.
        /// </summary>
        /// <remarks>
        /// <see cref="ChartSeriesBoundsEventArgs"/>
        /// </remarks>
        public event EventHandler<ChartSeriesBoundsEventArgs> SeriesBoundsChanged;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the legend that helps to identify the corresponding series or data point in chart.
        /// </summary>
        /// <value>This property takes a <see cref="ChartLegend"/> instance as value and its default value is null.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-1)
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
        ///        <chart:PieSeries ItemsSource="{Binding Data}" 
        ///                         XBindingPath="XValue"
        ///                         YBindingPath="YValue"/>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-2)
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
        ///     YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        /// 
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        /// 
        ///<remarks>
        /// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="Legend"/> property.</para>
        ///</remarks>
        public ChartLegend Legend
        {
            get { return (ChartLegend)GetValue(LegendProperty); }
            set { SetValue(LegendProperty, value); }
        }

        /// <summary>
        /// Gets or sets a tooltip behavior that allows to customize the default tooltip appearance in the chart. 
        /// </summary>
        /// <value>This property takes <see cref="ChartTooltipBehavior"/> instance as value and its default value is null.</value>
        /// <remarks>
        /// 
        /// <para>To display the tooltip on the chart, set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <b>ChartSeries</b>.</para>
        /// 
        /// <para>To customize the appearance of the tooltip elements like Background, Foreground and FontSize, create an instance of <see cref="ChartTooltipBehavior"/> class, modify the values, and assign it to the chart’s <see cref="TooltipBehavior"/> property. </para>
        /// 
        /// # [MainPage.xaml](#tab/tabid-3)
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
        ///         <chart:PieSeries EnableTooltip="True"
        ///                          ItemsSource="{Binding Data}"
        ///                          XBindingPath="XValue"
        ///                          YBindingPath="YValue"/>
        /// 
        /// </chart:SfCircularChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        ///  SfCircularChart chart = new SfCircularChart();
        ///  
        ///  ViewModel viewModel = new ViewModel();
        ///  chart.DataContext = viewModel;
        ///  
        ///  chart.TooltipBehavior = new ChartTooltipBehavior();
        ///  
        ///  PieSeries series = new PieSeries()
        ///  {
        ///     ItemsSource = viewmodel.Data,
        ///     XBindingPath = "XValue",
        ///     YBindingPath = "YValue",
        ///     EnableTooltip = true
        ///  };
        ///  chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// 
        /// </remarks>
        /// <seealso cref="ChartSeries.EnableTooltip"/>
        public ChartTooltipBehavior TooltipBehavior
        {
            get { return (ChartTooltipBehavior)GetValue(TooltipBehaviorProperty); }
            set { SetValue(TooltipBehaviorProperty, value); }
        }

        
        internal ChartBehaviorsCollection Behaviors
        {
            get { return behaviors; }
            set
            {
                behaviors = value;
            }
        }

       /// <summary>
       /// 
       /// </summary>
        internal Thickness AxisThickness
        {
            get { return (Thickness)GetValue(AxisThicknessProperty); }
            set { SetValue(AxisThicknessProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal Rect SeriesClipRect
        {
            get
            {
                return seriesClipRect;
            }
            set
            {
                if (seriesClipRect == value) return;
                var oldRect = seriesClipRect;
                seriesClipRect = value;

                if (LegendPanel != null && this is SfCartesianChart)
                {
                    LegendPanel.ArrangeRect = value;
                }

                OnSeriesBoundsChanged(new ChartSeriesBoundsEventArgs { OldBounds = oldRect, NewBounds = value });
                //OnPropertyChanged("SeriesClipRect");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal ChartVisibleSeriesCollection VisibleSeries
        {
            get { return (ChartVisibleSeriesCollection)GetValue(VisibleSeriesProperty); }
            set { SetValue(VisibleSeriesProperty, value); }
        }

       
        internal int SelectedSeriesIndex
        {
            get
            {
                //TODO : Move this section to selection behavior. 
                var selection = GetSeriesSelectionBehavior();

                if (selection != null)
                    return selection.SelectedIndex;
                else
                    return -1;
            }
        }

        internal ChartColumnDefinitions ColumnDefinitions
        {
            get
            {
                if (columnDefinitions != null) return columnDefinitions;
                columnDefinitions = new ChartColumnDefinitions();
                columnDefinitions.CollectionChanged += OnRowColChanged;

                return columnDefinitions;
            }
            set
            {
                if (columnDefinitions != null)
                {
                    columnDefinitions.CollectionChanged -= OnRowColChanged;
                }
                columnDefinitions = value;
                if (columnDefinitions != null)
                {
                    columnDefinitions.CollectionChanged -= OnRowColChanged;
                }
                ScheduleUpdate();
            }
        }

        internal ChartRowDefinitions RowDefinitions
        {
            get
            {
                if (rowDefinitions != null) return rowDefinitions;
                rowDefinitions = new ChartRowDefinitions();
                rowDefinitions.CollectionChanged += OnRowColChanged;

                return rowDefinitions;
            }
            set
            {
                if (rowDefinitions != null)
                {
                    rowDefinitions.CollectionChanged -= OnRowColChanged;
                }
                rowDefinitions = value;
                if (rowDefinitions != null)
                {
                    rowDefinitions.CollectionChanged += OnRowColChanged;
                }
                ScheduleUpdate();
            }
        }
        
        internal ChartAxisCollection InternalAxes { get; set; }

        /// <summary>
        /// Gets or sets the title for chart. It supports the string or any view as title.
        /// </summary>
        /// <value>Default value is null.</value>
        /// 
        /// <remarks>
        /// 
        /// <para>Example code for string as header.</para>
        /// 
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart Header="Average High/Low Temperature">
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     chart.Header = "Average High / Low Temperature";
        /// ]]>
        /// </code>
        /// ***
        /// 
        /// <para>Example code for View as header.</para>
        /// 
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        /// 
        ///        <chart:SfCartesianChart.Header>
        ///            <TextBlock Text = "Average High/Low Temperature" 
        ///                       HorizontalAlignment="Center"
        ///                       HorizontalTextAlignment="Center"
        ///                       VerticalAlignment="Center"
        ///                       FontSize="16"
        ///                       Foreground="Black"/>
        ///        </chart:SfCartesianChart.Header>
        /// 
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// 
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     chart.Header = new TextBlock() 
        ///     { 
        ///         Text = "Average High / Low Temperature",
        ///         HorizontalAlignment = HorizontalAlignment.Center,
        ///         HorizontalTextAlignment = TextAlignment.Center,
        ///         VerticalAlignment = VerticalAlignment.Center,
        ///         FontSize = 16,
        ///         Foreground = new SolidColorBrush(Colors.Black)
        ///     };
        /// ]]>
        /// </code>
        /// ***
        /// </remarks>
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to modify the horizontal alignment of the header.
        /// </summary>
        /// <value>It accepts <see cref="HorizontalAlignment"/> values and the default values is HorizontalAlignment.Center.</value>
        public HorizontalAlignment HorizontalHeaderAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalHeaderAlignmentProperty); }
            set { SetValue(HorizontalHeaderAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to modify the vertical alignment of the header.
        /// </summary>
        /// <value>It accepts <see cref="VerticalAlignment"/> values and the default value is VerticalAlignment.Center.</value>
        public VerticalAlignment VerticalHeaderAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalHeaderAlignmentProperty); }
            set { SetValue(VerticalHeaderAlignmentProperty, value); }
        }

        #endregion

        #region Internal Properties
        internal bool IsMultipleArea
        {
            get
            {
                return this.RowDefinitions.Count > 1 || this.ColumnDefinitions.Count > 1;
            }
        }

        internal ChartPlotArea PlotArea
        {
            get
            {
                return plotArea;
            }
            set
            {
                plotArea = value;
            }
        }

        internal Panel GridLinesPanel
        {
            get
            {
                return gridLinesPanel;
            }
        }

        internal bool CanRenderToBuffer
        {
            get;
            set;
        }

        internal AreaPanel AreaPanel { get; set; }

        internal Canvas DataLabelPresenter { get; set; }

        internal ChartAxis InternalPrimaryAxis { get; set; }

        internal ChartAxis InternalSecondaryAxis { get; set; }

        internal bool IsChartLoaded { get; set; }

        internal LegendPanel LegendPanel { get; set; }

        internal List<ChartAxis> DependentSeriesAxes { get; set; }

        internal bool SBSInfoCalculated //sbs - sidebyside
        {
            get;
            set;
        }
        
        internal Size? RootPanelDesiredSize
        {
            get { return rootPanelDesiredSize; }
            set
            {
                if (rootPanelDesiredSize == value) return;
                rootPanelDesiredSize = value;

                OnRootPanelSizeChanged(value != null ? value.Value : new Size());
            }
        }

        internal Size AvailableSize
        {
            get;
            set;
        }

        internal UpdateAction UpdateAction
        {
            get;
            set;
        }

        internal Dictionary<object, int> SeriesPosition
        {
            get { return seriesPosition; }
            set { seriesPosition = value; }
        }

        internal Dictionary<object, StackingValues> StackedValues { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1814:PreferJaggedArraysOverMultidimensional")]
        internal int[,] SbsSeriesCount   //sbs  - sidebyside
        {
            get;
            set;
        }

        internal double MinPointsDelta
        {
            get
            {
                m_minPointsDelta = double.MaxValue;

                foreach (var series in VisibleSeries)
                {
                    var xValues = (series.ActualXValues as List<double>);
                    if (!series.IsIndexed && xValues != null && series.IsSideBySide) // ColumnSegment width is changed while adding LineSeries dynamically-WPF-19670                 
                        GetMinPointsDelta(xValues, ref m_minPointsDelta, series, series.IsIndexed);
                }

                if (VisibleSeries.Count > 1 && isSbsWithOneData)
                {
                    List<double> previousXValues = new List<double>();

                    foreach (var series in VisibleSeries)
                    {
                        var xValues = (series.ActualXValues as List<double>);
                        if (!series.IsIndexed && xValues != null && xValues.Count > 0)
                        {
                            if (!series.IsSideBySide)
                                GetMinPointsDelta(xValues, ref m_minPointsDelta, series, series.IsIndexed);
                            else
                            {
                                //XAMARIN-35525 DateTimeAxis not rendered properly when series have single data point with different x position
                                var actualXValues = xValues.ToList();

                                if (actualXValues == null)
                                {
                                    continue;
                                }

                                previousXValues.AddRange(actualXValues);
                                //XAMARIN-35525 m_minPointsDelta value goes negative when data is non-linear.
                                previousXValues.Sort();
                                GetMinPointsDelta(previousXValues, ref m_minPointsDelta, series, series.IsIndexed);
                                previousXValues = actualXValues;
                            }
                        }
                    }

                    isSbsWithOneData = false;
                }
                else if (isSbsWithOneData)
                {
                    foreach (var series in VisibleSeries)
                    {
                        var dateTimeAxis = series.ActualXAxis as DateTimeAxis;
                        if (dateTimeAxis != null)
                        {
                            m_minPointsDelta = dateTimeAxis.ActualRange.End - dateTimeAxis.ActualRange.Start;
                        }
                    }
                }

                m_minPointsDelta = ((m_minPointsDelta == double.MaxValue || m_minPointsDelta < 0) ? 1 : m_minPointsDelta);

                return m_minPointsDelta;
            }
        }

        internal ILayoutCalculator GridLinesLayout
        {
            get { return gridLinesLayout; }
            set { gridLinesLayout = value; }
        }

        internal ChartAreaType AreaType
        {
            get
            {
                return areaType;
            }
            set
            {
                if (areaType == value) return;
                areaType = value;
                OnAreaTypeChanged();
            }
        }

        internal ILayoutCalculator ChartAxisLayoutPanel
        {
            get
            {
                return chartAxisLayoutPanel;
            }
            set
            {
                chartAxisLayoutPanel = value;
            }
        }
        
        
        internal ChartTooltip Tooltip
        {
            get { return (ChartTooltip)GetValue(TooltipProperty); }
            set { SetValue(TooltipProperty, value); }
        }

        internal bool IsLoading
        {
            get
            {
                return isLoading;
            }

            set
            {
                if (isLoading == value)
                {
                    return;
                }

                isLoading = value;
                //OnPropertyChanged("IsLoading");
            }
        }

        #endregion

        #region Private Properties

        private IList RowBorderLines
        {
            get
            {
                if (rowBorderLines == null)
                {
                    rowBorderLines = new List<Line>();
                }
                return rowBorderLines;
            }
            set
            {
                rowBorderLines = value;
            }
        }

        private IList ColumnBorderLines
        {
            get
            {
                if (columnBorderLines == null)
                {
                    columnBorderLines = new List<Line>();
                }
                return columnBorderLines;
            }
            set
            {
                columnBorderLines = value;
            }
        }

        private bool disposed;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        bool HasBitmapSeries
        {
            get
            {
                return VisibleSeries.Any(ser => ser.IsBitmapSeries);
            }
        }

        #endregion

        #region Protected Internal Properties

        internal virtual bool SideBySideSeriesPlacement
        {
            get
            {
                return false;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Static Methods
        
        internal static int GetRow(UIElement obj)
        {
            return (int)obj.GetValue(RowProperty);
        }

        internal static int GetColumn(UIElement obj)
        {
            return (int)obj.GetValue(ColumnProperty);
        }

        internal static int GetColumnSpan(UIElement element)
        {
            return (int)element.GetValue(ColumnSpanProperty);
        }

        internal static int GetRowSpan(UIElement element)
        {
            return (int)element.GetValue(RowSpanProperty);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public void SuspendSeriesNotification()
        {
            if (ActualSeries != null)
                foreach (ChartSeries series in this.ActualSeries)
                {
                    series.SuspendNotification();
                }
        }

        /// <summary>
        /// 
        /// </summary>    
        public void ResumeSeriesNotification()
        {
            if (ActualSeries != null)
                foreach (ChartSeries series in ActualSeries)
                {
                    series.ResumeNotification();
                }
        }

        #endregion

        #region Internal Virtual Methods


        internal virtual SeriesSelectionBehavior GetSeriesSelectionBehavior()
        {
            return null;
        }
       

        internal virtual double ActualPointToValue(ChartAxis axis, Point point)
        {
            if (axis != null)
            {
                if (!axis.IsVertical)
                {
                    return axis.CoefficientToValue((point.X - (axis.RenderedRect.Left - axis.Area.SeriesClipRect.Left)) / axis.RenderedRect.Width);
                }
                return axis.CoefficientToValue(1d - ((point.Y - (axis.RenderedRect.Top - axis.Area.SeriesClipRect.Top)) / axis.RenderedRect.Height));
            }
            return double.NaN;
        }

#endregion

#region Internal Methods

        internal void DisposeRowColumnsDefinitions()
        {
            if (rowDefinitions != null)
            {
                foreach (var row in rowDefinitions)
                {
                    row.Dispose();
                }

                rowDefinitions.CollectionChanged -= OnRowColChanged;
                rowDefinitions.Clear();
            }

            if (columnDefinitions != null)
            {
                foreach (var column in columnDefinitions)
                {
                    column.Dispose();
                }

                columnDefinitions.Clear();
                columnDefinitions.CollectionChanged -= OnRowColChanged;
            }
        }
        
        internal void DisposeSelectionEvents()
        {
            
            if(SeriesBoundsChanged != null)
            {
                foreach(var handler in SeriesBoundsChanged.GetInvocationList())
                {
                    SeriesBoundsChanged -= handler as EventHandler<ChartSeriesBoundsEventArgs>;
                }

                SeriesBoundsChanged = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal int GetActualRow(UIElement obj)
        {
            var actualPos = RowDefinitions.Count;
            var pos = GetRow(obj);
            var result = pos >= actualPos ? actualPos - 1 : (pos < 0 ? 0 : pos);
            return result < 0 ? 0 : result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>     
        internal int GetActualColumn(UIElement obj)
        {
            var actualPos = ColumnDefinitions.Count;
            var pos = GetColumn(obj);
            var result = pos >= actualPos ? actualPos - 1 : (pos < 0 ? 0 : pos);
            return result < 0 ? 0 : result;
        }

        internal int GetActualColumnSpan(UIElement element)
        {
            var count = ColumnDefinitions.Count;
            var span = GetColumnSpan(element);
            return span > count ? count : (span < 0 ? 0 : span);
        }
        
        internal int GetActualRowSpan(UIElement obj)
        {
            var count = RowDefinitions.Count;
            var span = GetRowSpan(obj);
            return span > count ? count : (span < 0 ? 0 : span);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        internal void GetMinPointsDelta(List<double> values, ref double minPointsDelta, ChartSeries series, bool isIndexed)
        {
            if (!series.IsLinearData) // WPF 17950 Series is not rendered properly while adding data statically and dynamically between the DateTime Range
            {
                values = values.ToList();
                values.Sort();
            }

            if (values.Count == 1)
                isSbsWithOneData = true;

            for (var i = 1; i < values.Count; i++)
            {
                var delta = values[i] - values[i - 1];
                if (delta != 0 && !double.IsNaN(delta))
                {
                    minPointsDelta = Math.Min(minPointsDelta, delta);
                }
            }
        }

        internal Canvas GetAdorningCanvas()
        {
            return AdorningCanvas;
        }

        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal Brush GetSeriesSelectionBrush(ChartSeries series)
        {
            var selectionBehavior = GetSeriesSelectionBehavior();
            if (selectionBehavior != null)
                return selectionBehavior.SelectionBrush;
            
            return null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal bool GetEnableSeriesSelection()
        {
            var selectionBehavior = GetSeriesSelectionBehavior();

            if (selectionBehavior != null && (selectionBehavior.Type == ChartSelectionType.Single || selectionBehavior.Type == ChartSelectionType.Multiple))
                return true;

            return false;
        }

        internal int GetSeriesIndex(ChartSeries series)
        {
            return ActualSeries.IndexOf(series);
        }


        internal void ScheduleUpdate()
        {
            AreaPanel?.ScheduleUpdate();
        }


#if NETFX_CORE
       
        internal void CompositionScheduleUpdate()
        {
#if WinUI_Desktop
            if (AreaPanel != null && !AreaPanel.IsUpdateDispatched)
#else
            if (updateAreaAction == null)
#endif
            {
                CompositionTarget.Rendering -= this.SmoothCompositeRendering;
                CompositionTarget.Rendering += this.SmoothCompositeRendering;
            }
        }
#endif

        internal void UpdateArea()
        {
            AreaPanel?.UpdateArea(false);
        }

        
        internal void RaiseSeriesSelectionChangedEvent()
        {
            var selectionBehavior = GetSeriesSelectionBehavior();
            if (selectionBehavior != null && !triggerSelectionChangedEventOnLoad)
            {
                if (selectionBehavior.SelectedIndex >= 0)
                {
                    var newIndexes = new List<int>() { selectionBehavior.SelectedIndex };
                    var oldIndexes = new List<int>();

                    selectionBehavior.OnSelectionChanged(newIndexes, oldIndexes, this);
                }
                else if (selectionBehavior.SelectedIndexes.Count > 0)
                {
                    var oldIndexes = new List<int>();
                    selectionBehavior.OnSelectionChanged(selectionBehavior.SelectedIndexes, oldIndexes, this);
                }

                triggerSelectionChangedEventOnLoad = true;
            }
        }

        #endregion

        #region Protected Internal Virtual Methods


        internal void OnSeriesSelectionChanged(int newIndex, int oldIndex)
        {
            var selectionBehavior = GetSeriesSelectionBehavior();
            if (selectionBehavior != null)
            {
                var newIndexes = new List<int>() { newIndex };
                var oldIndexes = new List<int>() { oldIndex };

                selectionBehavior.OnSelectionChanged(newIndexes, oldIndexes, this);
                triggerSelectionChangedEventOnLoad = true;
            }
        }

        internal bool OnSeriesSelectionChanging(int newIndex, int oldIndex)
        {
            var selectionBehavior = GetSeriesSelectionBehavior();

            if (selectionBehavior != null)
            {
                var newIndexes = new List<int>() { newIndex };
                var oldIndexes = new List<int>() { oldIndex };

                return selectionBehavior.OnSelectionChanging(newIndexes, oldIndexes, this);
            }

            return false;
        }

#endregion

#region Protected Virtual Methods

        internal virtual void OnRootPanelSizeChanged(Size size)
        {
            if (!IsTemplateApplied || !RootPanelDesiredSize.HasValue) return;
            UpdateAction |= UpdateAction.LayoutAndRender;

            AreaPanel?.UpdateArea(true);
        }

#endregion

#region Private Static Methods

        internal static void OnSideBySideSeriesPlacementProperty(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((ChartBase)d).ScheduleUpdate();
        }

        private static void OnLegendChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ChartBase chartBase = d as ChartBase;

            if (chartBase == null || chartBase.LegendPanel == null) return;

            chartBase.PlotArea.Legend = chartBase.LegendPanel.Legend = args.NewValue as ILegend;
        }


        internal static void OnBehaviorChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ChartBase chartBase = d as ChartBase;
            
            if (chartBase == null) return;

            if (args.OldValue is ChartBehavior behavior && chartBase.Behaviors.Contains(behavior))
            {
                chartBase.Behaviors.Remove(behavior);
            }

            if (args.NewValue is ChartBehavior newBehavior && !chartBase.Behaviors.Contains(newBehavior))
            {
                newBehavior.Chart = chartBase;
                chartBase.Behaviors.Add(newBehavior);
            }

        }

        #endregion

        #region Private Methods

#if NETFX_CORE
        
        private void SmoothCompositeRendering(object sender, object e)
        {
            CompositionTarget.Rendering -= this.SmoothCompositeRendering;
#if WinUI_Desktop
            DispatcherQueue.TryEnqueue(() => { UpdateArea(); });
            AreaPanel.IsUpdateDispatched = true;
#else
            updateAreaAction = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateArea);
#endif
        }
#endif

        private void OnAreaTypeChanged()
        {
            UpdateAxisLayoutPanels();
        }

        void OnRowColChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // WPF-52481:We have notified here only for reset and remove. As the border lines are added when arranging the axis.
            if (e.Action == NotifyCollectionChangedAction.Reset || e.Action == NotifyCollectionChangedAction.Remove)
            {
                OnRowColumnCollectionChanged(e);
            }

            ScheduleUpdate();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;

            Dispose(true);
        }

        internal virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            if (previousSeries != null)
            {
                previousSeries.Dispose();
                previousSeries = null;
            }

            AreaPanel?.Dispose();

            DisposeSeriesAndIndicators();
            DisposeBehaviors();
            DisposeZoomEvents();
            DisposeSelectionEvents();
            DisposeLegend();
            DisposeAxis();

            SizeChanged -= OnSfChartSizeChanged;
            SizeChanged -= OnSizeChanged;

            DisposeRowColumnsDefinitions();
            DisposePanels();

            AreaPanel = null;
            RootPanelDesiredSize = null;
            GridLinesLayout = null;
            ChartAxisLayoutPanel = null;
            TooltipBehavior = null;
        }

        private void DisposePanels()
        {
            if (AreaPanel != null)
            {
                AreaPanel = null;
            }

            var cartesianAxisLayoutPanel = ChartAxisLayoutPanel as ChartCartesianAxisLayoutPanel;
            if (cartesianAxisLayoutPanel != null)
            {
                cartesianAxisLayoutPanel.Area = null;
                cartesianAxisLayoutPanel.Children.Clear();
                cartesianAxisLayoutPanel = null;
            }
            else
            {
                var polarAxisLayoutPanel = ChartAxisLayoutPanel as ChartPolarAxisLayoutPanel;
                if (polarAxisLayoutPanel != null)
                {
                    polarAxisLayoutPanel.Area = null;
                    polarAxisLayoutPanel.PolarAxis = null;
                    polarAxisLayoutPanel.CartesianAxis = null;
                    polarAxisLayoutPanel.Children.Clear();
                    polarAxisLayoutPanel = null;
                }
            }

            var cartesialGridLinesPanel = GridLinesLayout as ChartCartesianGridLinesPanel;
            if (cartesialGridLinesPanel != null)
            {
                cartesialGridLinesPanel.Area = null;
                cartesialGridLinesPanel.Children.Clear();
                cartesialGridLinesPanel = null;
            }
            else
            {
                var polarGridLinesPanl = GridLinesLayout as ChartPolarGridLinesPanel;
                if (polarGridLinesPanl != null)
                {
                    polarGridLinesPanl.Dispose();
                    polarGridLinesPanl = null;
                }
            }

            if (LegendPanel != null && LegendPanel.Children.Count > 0)
            {
                LegendPanel.AreaPanel = null;
                LegendPanel.Children.Clear();
                LegendPanel = null;
            }
        }

        private void DisposeAxis()
        {
            if (InternalAxes != null)
            {
                foreach (var axis in InternalAxes)
                {
                    axis.Dispose();
                }
                InternalAxes.Clear();
                InternalAxes.CollectionChanged -= Axes_CollectionChanged;
                InternalAxes = null;
            }

         

            InternalPrimaryAxis = null;
            InternalSecondaryAxis = null;
        }

        private void DisposeSeriesAndIndicators()
        {
            if (VisibleSeries != null)
            {
                VisibleSeries.Clear();
            }

            if (ActualSeries != null)
            {
                ActualSeries.Clear();
            }

            if (SelectedSeriesCollection != null)
            {
                SelectedSeriesCollection.Clear();
            }

            CurrentSelectedSeries = null;
            PreviousSelectedSeries = null;

            var seriesColl = GetSeriesCollection();

            if (seriesColl != null)
            {
                UnHookSeriesCollection(seriesColl);
            }

            SetSeriesCollection(null);

            if (seriesColl != null)
            {
                foreach (ChartSeries series in seriesColl)
                {
                    series.Dispose();
                }
            }

        }

        private void DisposeBehaviors()
        {
            if (behaviors != null)
            {
                foreach (var behavior in behaviors)
                {
                    var zoomPanBehavior = behavior as ChartZoomPanBehavior;

                    if (zoomPanBehavior != null)
                    {
                        zoomPanBehavior.DisposeZoomEventArguments();
                    }

                    behavior.Dispose();
                }

                behaviors.Area = null;
            }

            chartZoomBehavior = null;
            TooltipBehavior = null;
        }

        private void DisposeLegend()
        {
            if (PlotArea != null)
            {
                PlotArea.Dispose();
                PlotArea = null;
            }

            if (LegendPanel != null)
            {
                LegendPanel.Dispose();
                LegendPanel = null;
            }

            if (Legend != null)
            {
                Legend.Header = null;
                Legend = null;
            }
        }
        internal virtual void DisposeZoomEvents()
        {

        }

        #region Public Override Methods

         
        internal virtual void SeriesSelectedIndexChanged(int newIndex, int oldIndex)
        {
            var seriesCollection = VisibleSeries;
            //Reset the oldIndex series Interior
            if (oldIndex < seriesCollection.Count && oldIndex >= 0
                && !GetSeriesSelectionBehavior().EnableMultiSelection)
            {
                ChartSeries series = seriesCollection[oldIndex];
                if (SelectedSeriesCollection.Contains(series))
                    SelectedSeriesCollection.Remove(series);

                OnResetSeries(series);
            }

            if (newIndex >= 0 && GetEnableSeriesSelection() && newIndex < VisibleSeries.Count)
            {
                ChartSeries series = seriesCollection[newIndex];

                if (!SelectedSeriesCollection.Contains(series))
                    SelectedSeriesCollection.Add(series);

                //For adornment selection implementation
                if (series.adornmentInfo is ChartDataLabelSettings && series.adornmentInfo.HighlightOnSelection &&
                    series.AdornmentPresenter != null)
                {
                    List<int> indexes = (from adorment in series.Adornments
                                         select series.Adornments.IndexOf(adorment)).ToList();

                    series.AdornmentPresenter.UpdateAdornmentSelection(indexes, false);
                }

                //Set the SeriestSelectionBrush to newIndex series Interior
                foreach (var segment in series.Segments)
                {
                    segment.BindProperties();
                    segment.IsSelectedSegment = true;
                }

                if (series.IsBitmapSeries)
                    UpdateBitmapSeries(series, false);
            }

            //Raise the selection changed event
            OnSeriesSelectionChanged(newIndex, oldIndex);
        }

        
        internal virtual double ActualValueToPoint(ChartAxis axis, double value)
        {
            if (axis != null)
            {
                if (!axis.IsVertical)
                {
                    return (axis.RenderedRect.Left - axis.Area.SeriesClipRect.Left)
                        + (axis.ValueToCoefficient(value) * axis.RenderedRect.Width);
                }
                return (axis.RenderedRect.Top - axis.Area.SeriesClipRect.Top) + (1 - axis.ValueToCoefficient(value)) * axis.RenderedRect.Height;
            }

            return double.NaN;
        }

        #endregion

        #region Public Methods

        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        internal double ValueToPointRelativeToAnnotation(ChartAxis axis, double value)
        {
            if (axis != null)
            {
                if (!axis.IsVertical)
                {
                    return (axis.RenderedRect.Left)
                        + (axis.ValueToCoefficient(value) * axis.RenderedRect.Width);
                }
                return (axis.RenderedRect.Top) + (1 - axis.ValueToCoefficient(value)) * axis.RenderedRect.Height;
            }

            return double.NaN;
        }

        #endregion

        #region Internal Override Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnRowColumnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            // WPF - 52481 : Overided the method to notfiy the row or column definiton collection changed.
            if (gridLinesPanel == null || gridLinesPanel.Children.Count <= 0)
            {
                return;
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                this.RemoveBorderLines();
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems != null)
                {
                    if (e.OldItems[0] is ChartRowDefinition)
                    {
                        Line borderLine = this.RowBorderLines[e.OldStartingIndex] as Line;
                        this.gridLinesPanel.Children.Remove(borderLine);
                        this.RowBorderLines.RemoveAt(e.OldStartingIndex);
                    }
                    else if (e.OldItems[0] is ChartColumnDefinition)
                    {
                        Line borderLine = this.ColumnBorderLines[e.OldStartingIndex] as Line;
                        this.gridLinesPanel.Children.Remove(borderLine);
                        this.ColumnBorderLines.RemoveAt(e.OldStartingIndex);
                    }
                }
            }

            
        }

        internal virtual void UpdateAxisLayoutPanels()
        {
            if (plotArea != null)
                plotArea.Clip = null;
            AxisThickness = new Thickness().GetThickness(0, 0, 0, 0);
            if (ChartAxisLayoutPanel != null)
            {
                ChartAxisLayoutPanel.DetachElements();
            }

            if (GridLinesLayout != null)
            {
                GridLinesLayout.DetachElements();
            }

            if (chartAxisPanel != null)
            {
                if (AreaType == ChartAreaType.PolarAxes)
                {
                    ChartAxisLayoutPanel = new ChartPolarAxisLayoutPanel(chartAxisPanel)
                    {
                        Area = this
                    };
                    ChartAxisLayoutPanel.UpdateElements();
                    GridLinesLayout = new ChartPolarGridLinesPanel(gridLinesPanel)
                    {
                        Area = this
                    };
                }
                else if (AreaType == ChartAreaType.CartesianAxes)
                {
                    ChartAxisLayoutPanel = new ChartCartesianAxisLayoutPanel(chartAxisPanel)
                    {
                        Area = this
                    };
                    ChartAxisLayoutPanel.UpdateElements();
                    GridLinesLayout = new ChartCartesianGridLinesPanel(gridLinesPanel)
                    {
                        Area = this
                    };
                }
                else
                {
                    ChartAxisLayoutPanel = null;
                    GridLinesLayout = null;
                }
            }
        }

        internal virtual double ValueToLogPoint(ChartAxis axis, double value)
        {
            if (axis != null)
            {
                return ActualValueToPoint(axis, value);
            }
            return double.NaN;
        }

        #endregion

        #region Internal Methods

        internal void Axes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                var axis = e.NewItems[0] as ChartAxis;
                if (axis != null && axis.Area == null)
                {
                    axis.Area = this;
                }

            }

            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                var axis = e.OldItems[0] as ChartAxis;
                if (axis != null)
                {
                    axis.Area = null;
                }
            }

            ScheduleUpdate();
        }


        internal void OnResetSeries(ChartSeries series)
        {
            foreach (var segment in series.Segments)
            {
                segment.BindProperties();
                segment.IsSelectedSegment = false;
            }

            if (series.IsBitmapSeries)
                UpdateBitmapSeries(series, true);

            if (series.adornmentInfo is ChartDataLabelSettings)
                series.AdornmentPresenter.ResetAdornmentSelection(null, true);

            foreach (var index in series.SelectedSegmentsIndexes)
            {
                if (index > -1 && series.GetEnableSegmentSelection())
                {
                    if (series.adornmentInfo is ChartDataLabelSettings && series.adornmentInfo.HighlightOnSelection)
                        series.UpdateAdornmentSelection(index);

                    if (series.IsBitmapSeries)
                    {
                        series.dataPoint = series.GetDataPoint(index);

                        DataPointSelectionBehavior selectionBehavior = series.SelectionBehavior;
                        if (selectionBehavior != null && series.dataPoint != null && selectionBehavior.SelectionBrush != null)
                        {
                            //Generate pixels for the particular data point
                            if (series.Segments.Count > 0) series.GeneratePixels();

                            //Set the SegmentSelectionBrush to the selected segment pixels
                            series.OnBitmapSelection(series.selectedSegmentPixels, selectionBehavior.SelectionBrush, true);
                        }
                    }
                }
            }
        }

        internal void CreateFastRenderSurface()
        {
            if (this.seriesPresenter != null && this.seriesPresenter.Children.Contains(fastRenderDevice) &&
                !SeriesClipRect.IsEmpty && SeriesClipRect.Width >= 1 && SeriesClipRect.Height >= 1)
            {
                if (fastRenderDevice != null)
                {
                    fastRenderDevice.PointerMoved -= FastRenderDevicePointerMoved;
                    fastRenderDevice.PointerExited -= FastRenderDevicePointerExited;
                    fastRenderDevice.Tapped -= FastRenderDevice_Tapped;
                    this.fastRenderSurface = new WriteableBitmap((int)SeriesClipRect.Width, (int)SeriesClipRect.Height);
                    this.fastRenderDevice.Height = SeriesClipRect.Height;
                    this.fastRenderDevice.Width = SeriesClipRect.Width;
                    this.fastRenderDevice.Source = this.fastRenderSurface;
#if NETFX_CORE
                    this.fastRenderSurfaceStream = this.fastRenderSurface.PixelBuffer.AsStream();
                    CreateBuffer(new Size(SeriesClipRect.Width, SeriesClipRect.Height));
#endif
                    fastRenderDevice.PointerMoved += FastRenderDevicePointerMoved;
                    fastRenderDevice.PointerExited += FastRenderDevicePointerExited;
                    fastRenderDevice.Tapped += FastRenderDevice_Tapped;
                }
            }
        }

        internal void AddOrRemoveBitmap()
        {
            if (this.seriesPresenter != null && this.seriesPresenter.Children.Contains(fastRenderDevice) && !HasBitmapSeries)
            {
                this.seriesPresenter.Children.Remove(fastRenderDevice);
                this.fastRenderSurface = null;
#if NETFX_CORE
                this.fastRenderSurfaceStream = null;
#endif
                this.fastBuffer = null;
            }
            else if (this.seriesPresenter != null && !this.seriesPresenter.Children.Contains(fastRenderDevice) && HasBitmapSeries)
            {
                this.seriesPresenter.Children.Insert(0, fastRenderDevice);
                if (fastRenderSurface == null)
                    this.CreateFastRenderSurface();
            }
        }

        internal void UpdateBitmapSeries(ChartSeries bitmapSeries, bool isReset)
        {
            var seriesCollection = GetSeriesCollection();
            int seriesIndex = seriesCollection.IndexOf(bitmapSeries);

            if (!isBitmapPixelsConverted)
                ConvertBitmapPixels();

            var collectionSeries = GetChartSeriesCollection();
            //Gets the upper series from the selected series
            var upperSeriesCollection = (from series in collectionSeries
                                         where collectionSeries.IndexOf(series) > seriesIndex
                                         select series).ToList();

            //Gets the upper series pixels in to single collection
            foreach (var series in upperSeriesCollection)
            {
                bitmapSeries.upperSeriesPixels.UnionWith(series.Pixels);
            }

            byte[] buffer = GetFastBuffer();
            int j = 0;
            Color uiColor;
            Brush brush = GetSeriesSelectionBrush(bitmapSeries);

            if (isReset)
            {
                if (bitmapSeries is FastLineBitmapSeries || bitmapSeries is FastStepLineBitmapSeries)
                {
                    ScheduleRenderSeries();
                }
                else
                {
                    for (int i = 0; i < bitmapSeries.PointsCount; i++)
                    {
                        bitmapSeries.dataPoint = bitmapSeries.GetDataPoint(i);

                        if (bitmapSeries.dataPoint != null)
                        {
                            //Generate pixels for the particular data point
                            if (bitmapSeries.Segments.Count > 0) bitmapSeries.GeneratePixels();

                            uiColor = ((SolidColorBrush)bitmapSeries.GetInteriorColor(i)).Color;

                            foreach (var pixel in bitmapSeries.selectedSegmentPixels)
                            {
                                if (!bitmapSeries.upperSeriesPixels.Contains(pixel))
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
                        }
                    }
                }
            }
            else if (brush != null)
            {
                uiColor = (brush as SolidColorBrush).Color;

                foreach (var pixel in bitmapSeries.Pixels)
                {
                    if (!bitmapSeries.upperSeriesPixels.Contains(pixel))
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
            }

            RenderToBuffer();
            bitmapSeries.upperSeriesPixels.Clear();
        }

        internal void ConvertBitmapPixels()
        {
            var seriesCollection = GetSeriesCollection();
            foreach (ChartSeries series in seriesCollection)
            {
                if (series.bitmapPixels.Count > 0)
                    series.Pixels = new HashSet<int>(series.bitmapPixels);
                series.bitmapPixels.Clear();
            }

            isBitmapPixelsConverted = true;
        }

        internal virtual void ClearPrimaryAxis()
        {
           
        }

        internal virtual void ClearSecondaryAxis()
        {
            
        }

        internal virtual void SetPrimaryAxis(ChartAxis chartAxis)
        {
           
        }

        internal virtual void SetSeriesCollection(IList seriesCollection)
        {

        }

        internal virtual void HookSeriesCollection(IList seriesCollection)
        {
            if (seriesCollection is ChartSeriesCollection)
            {
                (seriesCollection as ChartSeriesCollection).CollectionChanged += OnSeriesCollectionChanged;
            }
        }

        internal virtual void UnHookSeriesCollection(IList seriesCollection)
        {
            if (seriesCollection is ChartSeriesCollection)
            {
                (seriesCollection as ChartSeriesCollection).CollectionChanged -= OnSeriesCollectionChanged;
            }
        }

        internal virtual ObservableCollection<ChartSeries> GetChartSeriesCollection()
        {
            return null;
        }

        internal virtual IList GetSeriesCollection()
        {
            return null;
        }

        internal virtual bool IsNullPaletteBrushes()
        {
            return true;
        }

        internal virtual Brush GetPaletteBrush(int index)
        {
            return null;
        }


        internal void RenderSeries()
        {
            if (RootPanelDesiredSize != null)
            {
                clearPixels = true;

                byte[] previousBuffer = this.fastBuffer;


                var size = AreaType != ChartAreaType.None
                               ? new Size(SeriesClipRect.Width, SeriesClipRect.Height)
                               : RootPanelDesiredSize.Value;


                if (VisibleSeries != null)
                {
                    isBitmapPixelsConverted = false;
                    foreach (ChartSeries series in VisibleSeries.Where(item => item.Visibility == Visibility.Visible))
                    {
                        series.UpdateOnSeriesBoundChanged(size);
                    }
                }

                if (!CanRenderToBuffer)
                    this.fastBuffer = previousBuffer;
                RenderToBuffer();
            }

#if WinUI_Desktop
            isRenderSeriesDispatched = false;
#else
            renderSeriesAction = null;
#endif
            StackedValues = null;
        }

        internal byte[] GetFastBuffer()
        {
            return this.fastBuffer;
        }

        internal WriteableBitmap GetFastRenderSurface()
        {
            return this.fastRenderSurface;
        }

        internal void CreateBuffer(Size size)
        {
            CanRenderToBuffer = false;
            this.fastBuffer = new byte[(int)(size.Width) * (int)(size.Height) * 4];
        }

        internal void ClearBuffer()
        {
            if (clearPixels)
            {
                if (this.fastRenderSurfaceStream != null)
                {
                    CreateBuffer(new Size(SeriesClipRect.Width, SeriesClipRect.Height));
                    this.fastRenderSurface.Clear(this.fastRenderSurfaceStream, this.fastBuffer);
                    this.fastRenderSurface.Invalidate();
                }

                clearPixels = false;
            }
        }

        internal void RenderToBuffer()
        {
            if (this.fastRenderSurfaceStream != null && this.fastBuffer != null)
            {
                this.fastRenderSurfaceStream.Position = 0;
                this.fastRenderSurfaceStream.Write(this.fastBuffer, 0, this.fastBuffer.Count());
                this.fastRenderSurface.Invalidate();
            }

            CanRenderToBuffer = false;
        }

        internal void ScheduleRenderSeries()
        {
#if WinUI_Desktop
            if (!isRenderSeriesDispatched)
            {
                DispatcherQueue.TryEnqueue(() => { RenderSeries(); });
                isRenderSeriesDispatched = true;
            }
#else
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                RenderSeries();
            else if (renderSeriesAction == null)
                renderSeriesAction = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, RenderSeries);
#endif
        }

        internal double GetPercentage(IList<ChartSeries> seriesColl, double item, int index, bool reCalculation)
        {
            double totalValues = 0;
            if (reCalculation)
            {
                if (index == 0)
                    sumItems.Clear();
                foreach (var stackingSeries in seriesColl)
                {
                    StackedSeriesBase stackingChart = stackingSeries as StackedSeriesBase;
                    if (!stackingChart.IsSeriesVisible) continue;
                    if (stackingChart != null && stackingChart.YValues.Count != 0)
                    {
                        double value = index < stackingChart.YValues.Count ? stackingChart.YValues[index] : 0;
                        if (double.IsNaN(value))
                            value = 0;
                        totalValues += Math.Abs(value);
                    }
                }
                sumItems.Add(totalValues);
            }
            if (sumItems.Count != 0)
                item = (item / sumItems[index]) * 100;
            return item;
        }

        #endregion

        #region Protected Internal Override Methods

       
        internal virtual void OnSeriesBoundsChanged(ChartSeriesBoundsEventArgs args)
        {
            CreateFastRenderSurface();

            if (PlotArea != null)
            {
                PlotArea.Clip = new RectangleGeometry()
                {
                    Rect =
                            new Rect(0, 0, SeriesClipRect.Width + 0.5, SeriesClipRect.Height + 0.5)
                };
            }

            if (SeriesBoundsChanged != null && args != null)
                SeriesBoundsChanged(this, args);
        }

        #endregion

        #region Protected Internal Virtual Methods

        internal virtual void OnZoomChanged(ZoomChangedEventArgs args)
        {

        }

        internal virtual void OnZoomChanging(ZoomChangingEventArgs args)
        {

        }

        internal virtual void OnSelectionZoomingStart(SelectionZoomingStartEventArgs args)
        {

        }

        internal virtual void OnSelectionZoomingEnd(SelectionZoomingEndEventArgs args)
        {

        }

        internal virtual void OnSelectionZoomingDelta(SelectionZoomingDeltaEventArgs args)
        {

        }

        internal virtual void OnPanChanged(PanChangedEventArgs args)
        {

        }

        internal virtual void OnPanChanging(PanChangingEventArgs args)
        {

        }

        internal virtual void OnResetZoom(ResetZoomEventArgs args)
        {

        }

        #endregion


        #region Protected Override Methods

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.SizeChanged += OnSfChartSizeChanged;
            if (seriesPresenter != null &&
                seriesPresenter.Children.Contains(fastRenderDevice))
            {
                seriesPresenter.Children.Remove(fastRenderDevice);
            }

            seriesPresenter = GetTemplateChild("SyncfusionChartSeriesPresenter") as Panel;
            chartAxisPanel = GetTemplateChild("SyncfusionChartAxisPanel") as Panel;
            gridLinesPanel = GetTemplateChild("SyncfusionChartGridLinesPanel") as Panel;
            AdorningCanvas = GetTemplateChild("SyncfusionChartAdorningCanvas") as Canvas;
            LegendPanel = GetTemplateChild("SyncfusionLegendPanel") as LegendPanel;
            AreaPanel = GetTemplateChild("AreaPanel") as AreaPanel;
            DataLabelPresenter = GetTemplateChild("DataLabelPresenter") as Canvas;

            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.AdorningCanvas = AdorningCanvas;
                behavior.InternalAttachElements();
            }

            var seriesCollection = GetSeriesCollection();

            if (seriesCollection != null)
            {
                foreach (ChartSeries series in seriesCollection)
                {

                    //TODO: Need to move this codes to SfCartesianChart.
                    if (this is SfCartesianChart cartesianChart)
                        cartesianChart.UpdateActualAxis(series);

                    series.Chart = this;
                    if (series.EnableTooltip)
                    {
                        ActualEnableTooltip = true;
                    }
                }

                if (ActualEnableTooltip)
                {
                    ChartSeries.AddTooltipBehavior(this);
                    this.Tooltip = new ChartTooltip();
                }
            }

            if (Behaviors != null)
            {
                foreach (ChartBehavior behavior in Behaviors)
                {
                    if (behavior is ChartTooltipBehavior)
                    {
                        ChartTooltipBehavior tooltip = behavior as ChartTooltipBehavior;
                        tooltip.Chart = this;
                    }
                }
            }

            foreach (ChartAxis axis in InternalAxes)
            {
                axis.Area = this;
            }
            UpdateAxisLayoutPanels();

            if (seriesPresenter != null)
            {
                AddOrRemoveBitmap();

                if (seriesCollection != null)
                {
                    foreach (ChartSeries series in seriesCollection)
                    {
                        this.seriesPresenter.Children.Add(series);
                    }
                }
            }
            
            PlotArea.Legend = Legend;
            LegendPanel.Legend = Legend;

            IsTemplateApplied = true;
        }

        /// <inheritdoc />
        protected override Size MeasureOverride(Size availableSize)
        {
            bool needForceSizeChanged = false;
            double width = availableSize.Width, height = availableSize.Height;

            if (double.IsInfinity(width))
            {
                width = ActualWidth == 0d ? 500d : ActualWidth;
                needForceSizeChanged = true;
            }
            if (double.IsInfinity(height))
            {
                height = ActualHeight == 0d ? 300d : ActualHeight;
                needForceSizeChanged = true;
            }
            if (needForceSizeChanged)
            {
                SizeChanged -= OnSizeChanged;
                SizeChanged += OnSizeChanged;
                AvailableSize = new Size(width, height);
            }
            else
                AvailableSize = availableSize;

            return base.MeasureOverride(AvailableSize);
        }


        /// <inheritdoc />
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnLostFocus(e);
            }

            base.OnLostFocus(e);
        }

        /// <inheritdoc />
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnGotFocus(e);
            }

            base.OnGotFocus(e);
        }

        /// <inheritdoc />
        protected override void OnPointerCaptureLost(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerCaptureLost(e);
            }

            base.OnPointerCaptureLost(e);
        }

        /// <inheritdoc />
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnTapped(e);
            }

            base.OnTapped(e);
        }

        /// <inheritdoc />
        protected override void OnRightTapped(RightTappedRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnRightTapped(e);
            }

            base.OnRightTapped(e);
        }

        /// <inheritdoc />
        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerWheelChanged(e);
            }

            base.OnPointerWheelChanged(e);
        }

        /// <inheritdoc />
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerExited(e);
            }

            base.OnPointerExited(e);
        }

        /// <inheritdoc />
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerEntered(e);
            }

            base.OnPointerEntered(e);
        }

        /// <inheritdoc />
        protected override void OnPointerCanceled(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerCanceled(e);
            }

            base.OnPointerCanceled(e);
        }

        /// <inheritdoc />
        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnKeyUp(e);
            }

            base.OnKeyUp(e);
        }

        /// <inheritdoc />
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnKeyDown(e);
            }

            base.OnKeyDown(e);
        }

        /// <inheritdoc />
        protected override void OnHolding(HoldingRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnHolding(e);
            }

            base.OnHolding(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationStarting(ManipulationStartingRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnManipulationStarting(e);
            }

            base.OnManipulationStarting(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnManipulationStarted(e);
            }

            base.OnManipulationStarted(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnManipulationInertiaStarting(e);
            }

            base.OnManipulationInertiaStarting(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnManipulationCompleted(e);
            }

            base.OnManipulationCompleted(e);
        }

        /// <inheritdoc />
        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnManipulationDelta(e);
            }

            base.OnManipulationDelta(e);
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if (Behaviors.FirstOrDefault(item => item is ChartZoomPanBehavior) != null)
                ManipulationMode = ManipulationModes.Scale
                                   | ManipulationModes.TranslateRailsX
                                   | ManipulationModes.TranslateRailsY
                                   | ManipulationModes.TranslateX
                                   | ManipulationModes.TranslateY
                                   | ManipulationModes.TranslateInertia
                                   | ManipulationModes.Rotate;
            else if (ManipulationMode != ManipulationModes.System)
                ManipulationMode = ManipulationModes.System;

            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerPressed(e);
            }

            base.OnPointerPressed(e);
        }

        /// <inheritdoc />
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerMoved(e);
            }

            base.OnPointerMoved(e);
        }

        /// <inheritdoc />
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerReleased(e);
            }

            base.OnPointerReleased(e);
        }

        /// <inheritdoc />
        protected override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnDoubleTapped(e);
            }

            base.OnDoubleTapped(e);
        }

        #endregion

        #region Private Static Methods

        internal static void OnSeriesPropertyCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartBase).OnSeriesPropertyCollectionChanged(e);
        }

        #endregion

        #region Private Methods

        private void RemoveBorderLines()
        {
            if (this.ColumnDefinitions.Count == 0)
            {
                foreach (Line columnLine in ColumnBorderLines)
                {
                    this.gridLinesPanel.Children.Remove(columnLine);
                }
                this.ColumnBorderLines.Clear();
            }

            if (this.RowDefinitions.Count == 0)
            {
                foreach (Line rowLine in RowBorderLines)
                {
                    this.gridLinesPanel.Children.Remove(rowLine);
                }
                this.RowBorderLines.Clear();
            }
        }

        void FastRenderDevicePointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var mousePoint = e.GetCurrentPoint(fastRenderDevice);

            currentBitmapPixel = (fastRenderSurface.PixelWidth *
                (int)mousePoint.Position.Y + (int)mousePoint.Position.X) * 4;

            adorningCanvasPoint = e.GetCurrentPoint(GetAdorningCanvas()).Position;
            if (e.Pointer.PointerDeviceType != PointerDeviceType.Touch)
                UpdateBitmapToolTip();
        }

        private void OnSeriesPropertyCollectionChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                RemoveVisualChild();
                AddOrRemoveBitmap();
                var seriesPropertyCollection = (e.OldValue as IList);

                if (seriesPropertyCollection != null)
                {
                    UnHookSeriesCollection(seriesPropertyCollection);
                    foreach (ChartSeries series in seriesPropertyCollection)
                    {
                        var cartesianSeries = series as CartesianSeries;
                        if (cartesianSeries != null)
                        {
                            cartesianSeries.ActualXAxis = null;
                            cartesianSeries.ActualYAxis = null;
                        }
                    }
                }
            }
            var seriesCollection = GetSeriesCollection();
            if (seriesCollection != null)
            {
                HookSeriesCollection(seriesCollection);
                if (seriesCollection.Count > 0)
                {
                    if (this is SfPolarChart)
                        AreaType = ChartAreaType.PolarAxes;
#pragma warning disable CS0618 // Type or member is obsolete
                    else if (this is SfCartesianChart)
#pragma warning restore CS0618 // Type or member is obsolete
                        AreaType = ChartAreaType.CartesianAxes;
                    else
                        AreaType = ChartAreaType.None;

                    foreach (ChartSeries series in seriesCollection)
                    {
                        series.UpdateLegendIconTemplate(false);
                        series.Chart = this;

                        if (series.ActualXAxis != null)
                        {
                            series.ActualXAxis.Area = this;
                            if (!series.ActualXAxis.RegisteredSeries.Contains(series))
                                series.ActualXAxis.RegisteredSeries.Add(series);
                            if (!InternalAxes.Contains(series.ActualXAxis))
                            {
                                InternalAxes.Add(series.ActualXAxis);
                                DependentSeriesAxes.Add(series.ActualXAxis);
                            }
                        }
                        if (series.ActualYAxis != null)
                        {
                            series.ActualYAxis.Area = this;
                            if (!series.ActualYAxis.RegisteredSeries.Contains(series))
                                series.ActualYAxis.RegisteredSeries.Add(series);
                            if (!InternalAxes.Contains(series.ActualYAxis))
                            {
                                InternalAxes.Add(series.ActualYAxis);
                                DependentSeriesAxes.Add(series.ActualYAxis);
                            }
                        }
                        
                        if (seriesPresenter != null && !this.seriesPresenter.Children.Contains(series))
                        {
                            seriesPresenter.Children.Add(series);
                        }
                        if (series.IsSeriesVisible)
                        {
                            VisibleSeries.Add(series);
                        }
                        ActualSeries.Add(series);
                    }
                }
                AddOrRemoveBitmap();
            }

            ScheduleUpdate();
        }

        private void RemoveVisualChild()
        {
            if (seriesPresenter != null)
            {
                for (int i = seriesPresenter.Children.Count - 1; i >= 0; i--)
                {
                    if (seriesPresenter.Children[i] is ChartSeries series)
                    {
                        if (series != null)
                        {
                            var cartesianSeries = series as CartesianSeries;
                            if (cartesianSeries != null)
                            {
                                if (cartesianSeries.ActualXAxis != null)
                                    cartesianSeries.ActualXAxis.RegisteredSeries.Clear();

                                if (cartesianSeries.ActualYAxis != null)
                                    cartesianSeries.ActualYAxis.RegisteredSeries.Clear();

                                    cartesianSeries.ActualXAxis = null;
                                    cartesianSeries.ActualYAxis = null;
                            }
                        }
                        seriesPresenter.Children.RemoveAt(i);
                    }
                }
            }
            VisibleSeries.Clear();
            ActualSeries.Clear();
        }

        private void FastRenderDevice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Touch)
            {
                UpdateBitmapToolTip();
                isTap = true;
            }
        }

        void FastRenderDevicePointerExited(object sender, PointerRoutedEventArgs e)
        {
            for (int i = VisibleSeries.Count - 1; i >= 0; i--)
            {
                ChartSeries series = VisibleSeries[i];

                if (!isTap)
                {
                    if (series.EnableTooltip && (series.ActualTooltipPosition == TooltipPosition.Pointer || !series.Timer.IsEnabled))
                    {
                        series.RemoveTooltip();
                    }
                }
            }
            isTap = false;
        }
       
        internal void OnSeriesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var seriesCollection = GetSeriesCollection();
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (seriesPresenter != null)
                {
                    for (int i = seriesPresenter.Children.Count - 1; i >= 0; i--)
                    {
                        if (seriesPresenter.Children[i] is ChartSeries series)
                        {
                            UnRegisterSeries(series);
                            var currentSeries = seriesPresenter.Children[i];
                            var doughnutSeries = currentSeries as DoughnutSeries;
                            if (doughnutSeries != null)
                            {
                                doughnutSeries.RemoveCenterView(doughnutSeries.CenterView);
                            }

                            if (DataLabelPresenter != null)
                            {
                                if (series != null && series.AdornmentPresenter != null && this.DataLabelPresenter.Children.Contains(series.AdornmentPresenter))
                                    DataLabelPresenter.Children.Remove(series.AdornmentPresenter);
                            }

                            seriesPresenter.Children.Remove(currentSeries);
                        }
                    }
                }
                else
                {
                    foreach (var axis in InternalAxes)
                    {
                        axis.RegisteredSeries.Clear();
                    }
                }

                ActualSeries.Clear();
                VisibleSeries.Clear();
                SelectedSeriesCollection.Clear();
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
            {
                ChartSeries series = e.OldItems[0] as ChartSeries;

                if (VisibleSeries.Contains(series))
                    VisibleSeries.Remove(series);
                if (ActualSeries.Contains(series))
                    ActualSeries.Remove(series);
                if (SelectedSeriesCollection.Contains(series))
                    SelectedSeriesCollection.Remove(series);

                UnRegisterSeries(series);
                
                if (seriesPresenter != null)
                    seriesPresenter.Children.Remove(series);
                if (DataLabelPresenter != null)
                {
                    if (series.AdornmentPresenter != null && DataLabelPresenter.Children.Contains(series.AdornmentPresenter))
                        DataLabelPresenter.Children.Remove(series.AdornmentPresenter);
                }
                series.RemoveTooltip();

                var doughnutSeries = series as DoughnutSeries;
                if (doughnutSeries != null)
                {
                    doughnutSeries.RemoveCenterView(doughnutSeries.CenterView);
                }

                if (e.Action == NotifyCollectionChangedAction.Replace)
                {

                    if (seriesCollection.Count > 0)
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        if (this is SfPolarChart )
                            AreaType = ChartAreaType.PolarAxes;
                        else if (this is SfCartesianChart)
                            AreaType = ChartAreaType.CartesianAxes;
                        else
                            AreaType = ChartAreaType.None;
                        UpdateVisibleSeries(seriesCollection, e.NewStartingIndex);
                    }
                }
                else if (VisibleSeries.Count == 0 && seriesCollection.Count > 0)
                {
                    if (this is SfPolarChart)
                        AreaType = ChartAreaType.PolarAxes;
                    else if (this is SfCartesianChart)
                        AreaType = ChartAreaType.CartesianAxes;
                    else
                        AreaType = ChartAreaType.None;
                }
                else if (VisibleSeries.Count == 0 && seriesCollection.Count == 0)
                {
                    AreaType = ChartAreaType.CartesianAxes;
                }

                //WP-795: update the remaining series Chart Palette while remove the series
                if (!IsNullPaletteBrushes())
                {
                    foreach (var segment in VisibleSeries.SelectMany(ser => ser.Segments))
                    {
                        segment.BindProperties();
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewStartingIndex == 0)
                {
                    if (this is SfPolarChart)
                        AreaType = ChartAreaType.PolarAxes;
                    else if (this is SfCartesianChart)
                        AreaType = ChartAreaType.CartesianAxes;
                    else
                        AreaType = ChartAreaType.None;
                }
#pragma warning restore CS0618 // Type or member is obsolete
                if (e.OldItems == null && GetEnableSeriesSelection()
                    && SelectedSeriesIndex < seriesCollection.Count && SelectedSeriesIndex != -1)
                {
                    SelectedSeriesCollection.Add(seriesCollection[SelectedSeriesIndex] as ChartSeries);
                }

                UpdateVisibleSeries(e.NewItems, e.NewStartingIndex);

                //WINUI-1448 Series color(interior) is not updated with index when dynamically inserting the series.
                if (!IsNullPaletteBrushes() && e.NewStartingIndex < VisibleSeries.Count - 1)
                {
                    foreach (var segment in VisibleSeries.SelectMany(ser => ser.Segments))
                    {
                        segment.BindProperties();
                    }
                }
            }
            var canvas = this.GetAdorningCanvas();
            if (canvas != null)
            {
                if (canvas.Children.Contains(Tooltip))
                    canvas.Children.Remove(Tooltip);
            }
            if (PlotArea != null)
                PlotArea.ShouldPopulateLegendItems = true;

            AddOrRemoveBitmap();
            this.ScheduleUpdate();
            SBSInfoCalculated = false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private void UnRegisterSeries(ChartSeries series)
        {
            if (series != null)
            {
                if (series is CartesianSeries)
                {
                    CartesianSeries cartesianSeries = series as CartesianSeries;
                    if (cartesianSeries.ActualYAxis != null)
                    {
                        cartesianSeries.ActualYAxis.RegisteredSeries.Remove(series);
                        cartesianSeries.ActualYAxis = null;
                    }

                    if (cartesianSeries.ActualXAxis != null)
                    {
                        cartesianSeries.ActualXAxis.RegisteredSeries.Remove(series);
                        cartesianSeries.ActualXAxis = null;
                    }
                }
            }

            if (InternalPrimaryAxis != null && InternalSecondaryAxis != null
                && InternalPrimaryAxis.RegisteredSeries.Count == 0 && InternalSecondaryAxis.RegisteredSeries.Count == 0
                && series != null && series.IsActualTransposed)
            {
                InternalPrimaryAxis.IsVertical = false;
                InternalSecondaryAxis.IsVertical = true;
            }
        }

        private void UpdateVisibleSeries(IList seriesColl, int seriesIndex)
        {
            foreach (ChartSeries series in seriesColl)
            {
                if (series == null) continue;
                series.UpdateLegendIconTemplate(false);

                //TODO: Need to move this codes to SfCartesianChart.
                if (this is SfCartesianChart cartesianChart && series is CartesianSeries cartesianSeries)
                {
                    if (cartesianSeries.ActualXAxis == null)
                    {
                        cartesianSeries.ActualXAxis = !string.IsNullOrEmpty(cartesianSeries.XAxisName) ? cartesianSeries.GetXAxisByName(cartesianSeries.XAxisName, cartesianChart.XAxes) : InternalPrimaryAxis;
                    }

                    if (cartesianSeries.ActualYAxis == null)
                    {
                        cartesianSeries.ActualYAxis = !string.IsNullOrEmpty(cartesianSeries.YAxisName) ? cartesianSeries.GetYAxisByName(cartesianSeries.YAxisName, cartesianChart.YAxes) : InternalSecondaryAxis;
                    }
                }
                else if (this is SfPolarChart)
                {
                    //TODO: Need to move this codes to SfPlolarChart.
                    series.ActualXAxis = InternalPrimaryAxis;
                    series.ActualYAxis = InternalSecondaryAxis;
                }
                else if(series is DoughnutSeries doughnutSeries)
                {
                    this.InternalDoughnutHoleSize = doughnutSeries.InnerRadius;
                }

                series.Chart     = this;
                if (series.ActualXAxis != null)
                {
                    series.ActualXAxis.Area = this;

                    if (!series.ActualXAxis.RegisteredSeries.Contains(series))
                        series.ActualXAxis.RegisteredSeries.Add(series);
                    if (!InternalAxes.Contains(series.ActualXAxis))
                    {
                        InternalAxes.Add(series.ActualXAxis);
                        DependentSeriesAxes.Add(series.ActualXAxis);
                    }
                }
                if (series.ActualYAxis != null)
                {
                    series.ActualYAxis.Area = this;

                    if (!series.ActualYAxis.RegisteredSeries.Contains(series))
                        series.ActualYAxis.RegisteredSeries.Add(series);
                    if (!InternalAxes.Contains(series.ActualYAxis))
                    {
                        InternalAxes.Add(series.ActualYAxis);
                        DependentSeriesAxes.Add(series.ActualYAxis);
                    }
                }
                if (seriesPresenter != null && !this.seriesPresenter.Children.Contains(series))
                {
                    seriesPresenter.Children.Insert(seriesIndex, series);
                }
                if (series.IsSeriesVisible)
                {
                    int count = VisibleSeries.Count;
                    int visibleSeriesIndex = seriesIndex > count ? count : seriesIndex;

                    if (!VisibleSeries.Contains(series))
                    {
                        VisibleSeries.Insert(visibleSeriesIndex, series);
                    }
                }
                if (!ActualSeries.Contains(series))
                    ActualSeries.Insert(seriesIndex, series);

            }
        }

        /// <summary>
        /// This method is to update bitmap series tooltip.
        /// </summary>
        private void UpdateBitmapToolTip()
        {
            for (int i = VisibleSeries.Count - 1; i >= 0; i--)
            {
                ChartSeries series = VisibleSeries[i];

                if (series.EnableTooltip && series.IsHitTestSeries())
                {
                    //Gets the current mouse position chart data point
                    ChartDataPointInfo datapoint = series.GetDataPoint(adorningCanvasPoint);

                    if (datapoint != null)
                    {
                        series.mousePosition = adorningCanvasPoint;

                        if (this.Tooltip != null && this.Tooltip.PreviousSeries != null)
                        {
                            if (series.ActualTooltipPosition == TooltipPosition.Auto && !series.Equals(this.Tooltip.PreviousSeries))
                            {
                                series.RemoveTooltip();
                                series.Timer.Stop();
                            }
                        }

                        if (series.mousePosition != series.previousMousePosition)
                        {
                            series.UpdateSeriesTooltip(datapoint);
                        }

                        previousSeries = series;
                    }
                    break;
                }
                if (previousSeries != null)
                {
                    if (series.ActualTooltipPosition == TooltipPosition.Pointer || !series.Timer.IsEnabled)
                    {
                        series.Timer.Stop();
                        previousSeries.RemoveTooltip();
                    }
                }
            }
            currentBitmapPixel = -1;
        }

        void OnSfChartSizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (var behavior in Behaviors)
            {
                behavior.OnSizeChanged(e);
            }
        }

        private double GetOffetValue(ChartAxis axis)
        {
            if (Legend != null && Legend is ChartLegend)
            {
                if ((Legend as ChartLegend).Placement == LegendPlacement.Left || (Legend as ChartLegend).Placement == LegendPlacement.Top)
                {
                    if (!axis.IsVertical)
                        return (LegendPanel.DesiredSize.Width - AreaPanel.DesiredSize.Width);
                    return (LegendPanel.DesiredSize.Height - AreaPanel.DesiredSize.Height);
                }
            }
            return 0d;
        }

        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize != AvailableSize)
                InvalidateMeasure();
        }

        internal void LayoutAxis(Size availableSize)
        {
            if (ChartAxisLayoutPanel != null)
            {
                ChartAxisLayoutPanel.UpdateElements();
                ChartAxisLayoutPanel.Measure(availableSize);
                ChartAxisLayoutPanel.Arrange(availableSize);
            }

            foreach (var item in ColumnDefinitions)
            {
                if (gridLinesPanel != null && item != null && item.BorderLine != null && !gridLinesPanel.Children.Contains(item.BorderLine))
                {
                    //WPF-52481:Adding the border lines in collection to identify whether column or row. 
                    ColumnBorderLines.Add(item.BorderLine);
                    gridLinesPanel.Children.Add(item.BorderLine);
                }
            }
            foreach (var item in RowDefinitions)
            {
                if (gridLinesPanel != null && item != null && item.BorderLine != null && !gridLinesPanel.Children.Contains(item.BorderLine))
                {
                    //WPF-52481:Adding the border lines in collection to identify whether column or row. 
                    RowBorderLines.Add(item.BorderLine);
                    gridLinesPanel.Children.Add(item.BorderLine);
                }
            }

            if (GridLinesLayout != null)
            {
                GridLinesLayout.UpdateElements();
                GridLinesLayout.Measure(availableSize);
                GridLinesLayout.Arrange(availableSize);
            }

        }

        #endregion

        #endregion

    }

    /// <summary>
    /// Represents the chart series bounds event arguments.
    /// </summary>
    /// <remarks>
    /// It contains data for the old and new bounds of the rectangle.
    /// </remarks>
    public class ChartSeriesBoundsEventArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        /// Called when an instance is created for the <see cref="ChartSeriesBoundsEventArgs"/>.
        /// </summary>
        public ChartSeriesBoundsEventArgs()
        {

        }
        #endregion

        #region Properties

        #region Public Properties
        
        /// <summary>
        /// Gets or sets the new bounds of the rectangle.
        /// </summary>
        public Rect NewBounds { get; set; }

        /// <summary>
        /// Gets or sets the old bounds of the rectangle.
        /// </summary>
        public Rect OldBounds { get; set; }
        #endregion

        #endregion
    }
}
