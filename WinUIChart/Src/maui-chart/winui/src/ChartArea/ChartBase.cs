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
using ChartAdornmentInfo = Syncfusion.UI.Xaml.Charts.ChartDataLabelSettings;
using AdornmentSeries = Syncfusion.UI.Xaml.Charts.DataMarkerSeries;
using StackingSeriesBase = Syncfusion.UI.Xaml.Charts.StackedSeriesBase;
using SelectionStyle = Syncfusion.UI.Xaml.Charts.SelectionType;
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
    /// <summary>
    /// <see cref="ChartBase"/> is a base class for chart. Which represents a chart control with basic presentation characteristics. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public abstract partial class ChartBase : Control, IDisposable
    {
        #region Constructor

        /// <summary>
        /// Called when instance created for <see cref="ChartBase"/>. 
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
            TechnicalIndicators = new ObservableCollection<ChartSeries>();
            Annotations = new AnnotationCollection();
            VisibleSeries = new ChartVisibleSeriesCollection();
            InternalAxes = new ChartAxisCollection();
           
            DependentSeriesAxes = new List<ChartAxis>();
            Printing = new Printing(this);
            InternalAxes.CollectionChanged += Axes_CollectionChanged;
            Behaviors = new ChartBehaviorsCollection(this);
            ColorModel = new ChartColorModel(this.Palette);
            InitializeLegendItems();
        }

        #endregion

        #region Dependency Property Registrations


        /// <summary>
        /// The DependencyProperty for <see cref="Watermark"/> property.
        /// </summary>
        internal static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register(
                "Watermark",
                typeof(Watermark),
                typeof(ChartBase),
                new PropertyMetadata(null, OnWatermarkChanged));


        /// <summary>
        /// The DependencyProperty for <see cref="Annotations"/> property.
        /// </summary>
        internal static readonly DependencyProperty AnnotationsProperty =
            DependencyProperty.Register(
                "Annotations",
                typeof(AnnotationCollection),
                typeof(ChartBase),
                new PropertyMetadata(null, OnAnnotationsChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="TechnicalIndicators"/> property.
        /// </summary>
        internal static readonly DependencyProperty TechnicalIndicatorsProperty =
            DependencyProperty.Register(
                "TechnicalIndicators",
                typeof(ObservableCollection<ChartSeries>),
                typeof(ChartBase),
                new PropertyMetadata(null, OnTechnicalIndicatorsPropertyChanged));

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
                new PropertyMetadata(0, OnRowColumnChanged));

        /// <summary>
        /// The DependencyProperty for ColumnSpan property.
        /// </summary>
        internal static readonly DependencyProperty ColumnSpanProperty =
            DependencyProperty.RegisterAttached(
                "ColumnSpan",
                typeof(int),
                typeof(ChartBase),
                new PropertyMetadata(1, OnRowColumnChanged));

        /// <summary>
        /// The DependencyProperty for RowSpan property.
        /// </summary>
        internal static readonly DependencyProperty RowSpanProperty =
            DependencyProperty.RegisterAttached(
                "RowSpan", 
                typeof(int), 
                typeof(ChartBase),
                new PropertyMetadata(1, OnRowColumnChanged));

        /// <summary>
        /// The DependencyProperty for Column" property.
        /// </summary>
        internal static readonly DependencyProperty ColumnProperty =
            DependencyProperty.RegisterAttached(
                "Column", 
                typeof(int),
                typeof(ChartBase),
                new PropertyMetadata(0, OnRowColumnChanged));

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
        /// The DependencyProperty for <see cref="Palette"/> property.
        /// </summary>
        public static readonly DependencyProperty PaletteProperty =
            DependencyProperty.Register(
                "Palette",
                typeof(ChartColorPalette), 
                typeof(ChartBase),
                new PropertyMetadata(ChartColorPalette.Metro, OnPaletteChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="SelectedSeriesIndex"/> property.
        /// </summary>
        internal static readonly DependencyProperty SelectedSeriesIndexProperty =
            DependencyProperty.Register(
                nameof(SelectedSeriesIndex), 
                typeof(int),
                typeof(ChartBase),
                new PropertyMetadata(-1, OnSeriesSelectedIndexChanged));
              
      
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
        /// The DependencyProperty for <see cref="ColorModel"/> property.
        /// </summary>
        public static readonly DependencyProperty ColorModelProperty =
            DependencyProperty.Register(
                "ColorModel", 
                typeof(ChartColorModel), 
                typeof(ChartBase),
                new PropertyMetadata(null, OnColorModelChanged));     

        /// <summary>
        /// The DependencyProperty for <see cref="Tooltip"/> property.
        /// </summary>
        internal static readonly DependencyProperty TooltipProperty =
            DependencyProperty.Register("Tooltip", typeof(ChartTooltip), typeof(ChartBase), new PropertyMetadata(null));

        #endregion

        #region Field

        #region Internal Fields


#if WinUI_Desktop
        internal bool isRenderSeriesDispatched = false;

        internal bool isUpdateDispatched = false;
#else
        internal IAsyncAction renderSeriesAction;

         internal IAsyncAction updateAreaAction;
#endif

        internal Panel chartAxisPanel;

        internal int currentBitmapPixel = -1;

        internal Point adorningCanvasPoint;

        internal bool isBitmapPixelsConverted;

        internal bool HoldUpdate;

        internal ZoomingToolBar zoomingToolBar;

        internal ChartZoomPanBehavior chartZoomBehavior;

        internal WriteableBitmap fastRenderSurface;

        internal double InternalDoughnutHoleSize = 0.5;

        internal Canvas AdorningCanvas;

        internal Canvas ToolkitCanvas;

        internal bool ActualShowTooltip
        {
            get;
            set;
        }

        internal bool IsTemplateApplied;

        internal bool IsUpdateLegend;

        internal List<ChartSeriesBase> ActualSeries = new List<ChartSeriesBase>();

        internal List<ChartSeriesBase> SelectedSeriesCollection = new List<ChartSeriesBase>();

        internal ChartSeriesBase CurrentSelectedSeries, PreviousSelectedSeries;

        #endregion

        #region Protected Fields

        internal Printing Printing;

        #endregion

        #region Private Fields

        private Panel gridLinesPanel;

        private bool clearPixels;

        private Panel seriesPresenter;

        private ChartRootPanel rootPanel;

        List<double> sumItems = new List<double>();

        private ChartSeries previousSeries; //This field used in UpdateBitmapTooltip for storing the series as temp.

        private IList rowBorderLines, columnBorderLines;

#if NETFX_CORE
        bool isTap;
#endif

        private ChartBehaviorsCollection behaviors;

        private Panel internalCanvas;

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

        private ChartSelectionBehavior selectionBehavior;

        private ChartTooltipBehavior tooltipBehavior;

        private Dictionary<object, int> seriesPosition = new Dictionary<object, int>();

        private bool isLoading = true;

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Event correspond to series and segment selection. It invokes once selection changed from a series or segment.
        /// </summary>
        /// <remarks>
        ///     <see cref="ChartSelectionChangedEventArgs"/>
        /// </remarks>
        public event EventHandler<ChartSelectionChangedEventArgs> SelectionChanged;

        /// <summary>
        /// Event correspond to series and segment selection. It invokes before selection changing from a series or segment.
        /// </summary>
        /// <remarks>
        ///     <see cref="ChartSelectionChangingEventArgs"/>
        /// </remarks>
        public event EventHandler<ChartSelectionChangingEventArgs> SelectionChanging;

        /// <summary>
        /// Event correspond to plot area bound. It invokes when the plot area size changes.
        /// </summary>
        /// <remarks>
        ///     <see cref="ChartSeriesBoundsEventArgs"/>
        /// </remarks>
        public event EventHandler<ChartSeriesBoundsEventArgs> SeriesBoundsChanged;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the chart watermark.
        /// </summary>
        internal Watermark Watermark
        {
            get { return (Watermark)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }


        /// <summary>
        /// Gets or sets the collection of chart behaviors.
        /// </summary>
        public ChartBehaviorsCollection Behaviors
        {
            get { return behaviors; }
            set
            {
                OnBehaviorPropertyChanged(behaviors, value);
                behaviors = value;
            }
        }

        /// <summary>
        /// Gets or sets the collection of annotations to the chart.
        /// </summary>
        internal AnnotationCollection Annotations
        {
            get { return (AnnotationCollection)GetValue(AnnotationsProperty); }
            set { SetValue(AnnotationsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of technical indicators to the chart.
        /// </summary>
        internal ObservableCollection<ChartSeries> TechnicalIndicators
        {
            get { return (ObservableCollection<ChartSeries>)GetValue(TechnicalIndicatorsProperty); }
            set { SetValue(TechnicalIndicatorsProperty, value); }
        }

        /// <summary>
        /// Gets or sets thickness to the axis.
        /// </summary>
        internal Thickness AxisThickness
        {
            get { return (Thickness)GetValue(AxisThicknessProperty); }
            set { SetValue(AxisThicknessProperty, value); }
        }

        /// <summary>
        /// Gets a bounds of chart area excluding axis and chart header.
        /// </summary>
        public Rect SeriesClipRect
        {
            get
            {
                return seriesClipRect;
            }
            internal set
            {
                if (seriesClipRect == value) return;
                var oldRect = seriesClipRect;
                seriesClipRect = value;
                OnSeriesBoundsChanged(new ChartSeriesBoundsEventArgs { OldBounds = oldRect, NewBounds = value });
                //OnPropertyChanged("SeriesClipRect");
            }
        }
        
        /// <summary>
        /// Gets the collection of visible series in the chart.
        /// </summary>
        /// <remarks>
        /// This property is intended to be used for custom <see>
        /// <cref>ChartArea</cref>
        /// </see>
        /// templates.
        /// </remarks>        
        public ChartVisibleSeriesCollection VisibleSeries
        {
            get { return (ChartVisibleSeriesCollection)GetValue(VisibleSeriesProperty); }
            internal set { SetValue(VisibleSeriesProperty, value); }
        }

        /// <summary>
        /// Gets or sets palette for chart. By default, it is <see cref="ChartColorPalette.Metro"/>.
        /// </summary> 
        /// <value>
        /// <see cref="ChartColorPalette"/>
        /// </value>
        public ChartColorPalette Palette
        {
            get { return (ChartColorPalette)GetValue(PaletteProperty); }
            set { SetValue(PaletteProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets the index to select the series.
        /// </summary>        
        public int SelectedSeriesIndex
        {
            get { return (int)GetValue(SelectedSeriesIndexProperty); }
            set { SetValue(SelectedSeriesIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the collection of ChartColumnDefinition objects defined in the chart.
        /// </summary>        
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

        /// <summary>
        /// Gets or sets the collection of ChartRowDefinition objects defined in chart.
        /// </summary>        
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
        
        /// <summary>
        /// Gets the collection of horizontal and vertical axis.
        /// </summary>
        internal ChartAxisCollection InternalAxes { get; set; }

       

        /// <summary>
        /// Gets or sets title for the chart.
        /// </summary>        
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal alignment for the header.
        /// </summary>
        /// <value>
        /// <see cref="HorizontalAlignment"/>
        /// </value>
        public HorizontalAlignment HorizontalHeaderAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalHeaderAlignmentProperty); }
            set { SetValue(HorizontalHeaderAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical alignment for the header.
        /// </summary>
        /// <value>
        /// <see cref="VerticalAlignment"/>
        /// </value>
        public VerticalAlignment VerticalHeaderAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalHeaderAlignmentProperty); }
            set { SetValue(VerticalHeaderAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color schemes for all series in the chart.
        /// </summary>
        /// <value>
        /// <see cref="ChartColorModel"/>
        /// </value>
        public ChartColorModel ColorModel
        {
            get { return (ChartColorModel)GetValue(ColorModelProperty); }
            set { SetValue(ColorModelProperty, value); }
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

        internal Panel InternalCanvas
        {
            get
            {
                return internalCanvas;
            }
            set
            {
                internalCanvas = value;
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

        internal Canvas ChartAnnotationCanvas
        {
            get;
            set;
        }

        internal Canvas SeriesAnnotationCanvas
        {
            get;
            set;
        }

        internal AnnotationManager AnnotationManager
        {
            get;
            set;
        }

        internal Canvas BottomAdorningCanvas
        {
            get;
            set;
        }

     
        /// <summary>
        /// Gets or sets the intermediate PrimaryAxis object used for internal calculation.
        /// </summary>
        internal ChartAxis InternalPrimaryAxis { get; set; }

        /// <summary>
        /// Gets or sets the intermediate DepthAxis object used for internal calculation.
        /// </summary>
        internal ChartAxis InternalDepthAxis { get; set; }

        /// <summary>
        /// Gets or sets the intermediate SecondaryAxis object used for internal calculation.
        /// </summary>
        internal ChartAxis InternalSecondaryAxis { get; set; }

        internal bool IsChartLoaded { get; set; }

        internal ChartDockPanel ChartDockPanel { get; set; }

        internal List<ChartAxis> DependentSeriesAxes { get; set; }

        internal ChartSelectionBehavior SelectionBehaviour
        {
            get
            {
                if (selectionBehavior == null)
                    SetSelectionBehaviour();
                return selectionBehavior;
            }
            set
            {
                selectionBehavior = value;
            }
        }

        internal ChartTooltipBehavior TooltipBehavior
        {
            get
            {
                if (tooltipBehavior == null)
                    SetTooltipBehavior();
                return tooltipBehavior;
            }
            set
            {
                tooltipBehavior = value;
            }
        }

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

        /// <summary>
        /// Gets the calclulated minimum delta value.
        /// </summary>
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
                                //XAMARIN-35525 DateTimeAxis not rendered properly when series have single datapoint with different x position
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

        /// <summary>
        /// Gets or sets the type of the chart area.
        /// </summary>
        /// <value>
        /// <see cref="ChartAreaType"/>
        /// </value>
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

        /// <summary>
        /// Gets or sets the chart axis layout panel.
        /// </summary>
        /// <value>
        /// The chart axis layout panel.
        /// </value>
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
        
        /// <summary>
        /// Gets or sets the current Tooltip object, which is displaying in the chart.
        /// </summary>
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

        /// <summary>
        /// Property to get the list of lines added for a row definition.
        /// </summary>
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

        /// <summary>
        /// Property to get the list of lines added for a column definition.
        /// </summary>
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
                return VisibleSeries.Any(ser => ser.IsBitmapSeries)
                       || (TechnicalIndicators != null && TechnicalIndicators.Any(indicator => indicator is MACDTechnicalIndicator
                       && indicator.Visibility == Visibility.Visible
                       && (indicator as MACDTechnicalIndicator).Type != MACDType.Line));
            }
        }

        #endregion

        #region Protected Internal Properties

        /// <summary>
        /// Gets the selected segments in this series, when we enable the multiple selection.
        /// </summary>
        /// <returns>
        /// It returns list of <see cref="ChartSegment"/>.
        /// </returns>
        internal virtual List<ChartSegment> SelectedSegments
        {
            get
            {
                if (VisibleSeries.Count > 0)
                {
                    return VisibleSeries.Where(series => series.SelectedSegments != null).
                        SelectMany(series => series.SelectedSegments).ToList();
                }
                else
                    return null;
            }
        }

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
                /// <summary>
                /// Return row value from the given ChartAxis.
                /// </summary>
                /// <param name="obj">The UIElement.</param>
                /// <returns>Row value of given object.</returns>
                internal static int GetRow(UIElement obj)
        {
            return (int)obj.GetValue(RowProperty);
        }

        /// <summary>
        /// Method implementation for set row value to ChartAxis.
        /// </summary>
        /// <param name="obj">The UIElement.</param>
        /// <param name="value">Row value of the object.</param>   
        internal static void SetRow(UIElement obj, int value)
        {
            obj.SetValue(RowProperty, value);
        }

        /// <summary>
        /// Get the column value from the given ChartAxis.
        /// </summary>
        /// <param name="obj">The UIElement.</param>
        /// <returns>Column value of given object.</returns>  
        internal static int GetColumn(UIElement obj)
        {
            return (int)obj.GetValue(ColumnProperty);
        }

        /// <summary>
        /// Gets the value of the Syncfusion.UI.Xaml.Charts.ColumnSpan attached property from a given UIElement. 
        /// </summary>
        /// <param name="element">The element from which to read the property value.</param>
        /// <returns>The value of the Syncfusion.UI.Xaml.Charts.ColumnSpan attached property.</returns>     
        internal static int GetColumnSpan(UIElement element)
        {
            return (int)element.GetValue(ColumnSpanProperty);
        }

        /// <summary>
        /// Gets the value of the Syncfusion.UI.Xaml.Charts.RowSpan attached property from a given UIElement.
        /// </summary>
        /// <param name="element">The element from which to read the property value.</param>
        /// <returns>The value of the Syncfusion.UI.Xaml.Charts.RowSpan attached property.</returns>
        internal static int GetRowSpan(UIElement element)
        {
            return (int)element.GetValue(RowSpanProperty);
        }

        /// <summary>
        /// Set column to ChartAxis.
        /// </summary>
        /// <param name="obj">The UIElement.</param>
        /// <param name="value">Column value of the object.</param>   
        internal static void SetColumn(UIElement obj, int value)
        {
            obj.SetValue(ColumnProperty, value);
        }

        /// <summary>
        /// Sets the value of the Syncfusion.UI.Xaml.Charts.ColumnSpan attached property to a given UIElement.
        /// </summary>
        /// <param name="element"> The element on which to set the Syncfusion.UI.Xaml.Charts.ColumnSpan attached property.</param>
        /// <param name="value">The property value to set.</param>   
        internal static void SetColumnSpan(UIElement element, int value)
        {
            element.SetValue(ColumnSpanProperty, value);
        }

        /// <summary>
        /// Sets the value of the Syncfusion.UI.Xaml.Charts.RowSpan attached property to a given UIElement.
        /// </summary>
        /// <param name="element"> The element on which to set the Syncfusion.UI.Xaml.Charts.RowSpan attached property.</param>
        /// <param name="value">The property value to set.</param>     
        internal static void SetRowSpan(UIElement element, int value)
        {
            element.SetValue(RowSpanProperty, value);
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Suspends all the series from updating the data till ResumeSeriesNotification is called. It is specifically used when you need to append the collection of data.
        /// </summary>        
        public void SuspendSeriesNotification()
        {
            if (ActualSeries != null)
                foreach (ChartSeriesBase series in this.ActualSeries)
                {
                    series.SuspendNotification();
                }
        }

        /// <summary>
        /// Processes the data that is added to the data source after the SuspendSeriesNotification is called.
        /// </summary>        
        public void ResumeSeriesNotification()
        {
            if (ActualSeries != null)
                foreach (ChartSeriesBase series in this.ActualSeries)
                {
                    series.ResumeNotification();
                }
        }

        /// <summary>
        /// Clone the entire chart control.
        /// </summary>
        internal DependencyObject Clone()
        {
            return CloneChart();
        }

        /// <summary>
        /// Returns the stacked value of the series.
        /// </summary>
        /// <param name="series">ChartSeries</param>
        /// <param name="reqNegStack">RequiresNegativeStack</param>
        /// <returns>StackedYValues collection.</returns>
        internal List<double> GetCumulativeStackInfo(ChartSeriesBase series, bool reqNegStack)
        {
            if (series != null)
            {
                var y = series.ActualYAxis.Origin;
                double currtY;
                var calcYValues = new List<double>();

                foreach (var ser in VisibleSeries)
                {
                    var yValues = ((XyDataSeries)ser).YValues;
                    if (ser.ActualXValues != null)
                    {
                        if (calcYValues.Count > 0)
                        {
                            for (var i = 0; i < ser.DataCount; i++)
                            {
                                currtY = reqNegStack ? Math.Abs(yValues[i]) : yValues[i];
                                if (i < calcYValues.Count)
                                    calcYValues[i] += currtY + y;
                                else
                                    calcYValues.Add(currtY + y);
                            }
                        }
                        else
                        {
                            for (var i = 0; i < ser.DataCount; i++)
                            {
                                currtY = reqNegStack ? Math.Abs(yValues[i]) : yValues[i];
                                calcYValues.Add(currtY + y);
                            }
                        }
                        if (series == ser)
                            break;
                    }
                }
                return calcYValues;
            }
            return null;
        }

        #endregion

        #region Public Virtual Methods



       

        /// <summary>
        /// Converts screen point to chart value.
        /// </summary>
        /// <param name="axis">The axis value.</param>
        /// <param name="point">The point.</param>
        /// <returns>The double point to value.</returns>        
        internal virtual double ActualPointToValue(ChartAxis axis, Point point)
        {
            if (axis != null)
            {
                if (axis.Orientation.Equals(Orientation.Horizontal))
                {
                    return axis.CoefficientToValueCalc((point.X - (axis.RenderedRect.Left - axis.Area.SeriesClipRect.Left)) / axis.RenderedRect.Width);
                }
                return axis.CoefficientToValueCalc(1d - ((point.Y - (axis.RenderedRect.Top - axis.Area.SeriesClipRect.Top)) / axis.RenderedRect.Height));
            }
            return double.NaN;
        }

#endregion

#region Internal Virtual Methods

       
        /// <summary>
        /// Clone the entire chart.
        /// </summary>
        internal virtual DependencyObject CloneChart()
        {
            return null;
        }



#endregion

#region Internal Methods

#if WinUI_Desktop
        /// <summary>
        /// Change cursor type while mouse over.
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="isChanged"></param>
        internal void ChangeCursorType(InputSystemCursorShape cursor, bool isChanged)
        {
            if (ProtectedCursor != null && isChanged)
                this.ProtectedCursor = InputSystemCursor.Create(cursor);
            else
                this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        }
#endif

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
            if(SelectionChanged != null)
            {
                foreach(var handler in SelectionChanged.GetInvocationList())
                {
                    SelectionChanged -= handler as EventHandler<ChartSelectionChangedEventArgs>;
                }

                SelectionChanged = null;              
            }

            if(SelectionChanging != null)
            {
                foreach(var handler in SelectionChanging.GetInvocationList())
                {
                    SelectionChanging -= handler as EventHandler<ChartSelectionChangingEventArgs>;
                }

                SelectionChanging = null;
            }

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
        /// Gets actual row value from the given ChartAxis.
        /// </summary>
        /// <param name="obj">The UIElement.</param>
        /// <returns>Actual row value.</returns>
        internal int GetActualRow(UIElement obj)
        {
            var actualPos = RowDefinitions.Count;
            var pos = GetRow(obj);
            var result = pos >= actualPos ? actualPos - 1 : (pos < 0 ? 0 : pos);
            return result < 0 ? 0 : result;
        }

        /// <summary>
        /// Gets actual column value from the given ChartAxis.
        /// </summary>
        /// <param name="obj">The UIElement.</param>
        /// <returns>Actual column value.</returns>        
        internal int GetActualColumn(UIElement obj)
        {
            var actualPos = ColumnDefinitions.Count;
            var pos = GetColumn(obj);
            var result = pos >= actualPos ? actualPos - 1 : (pos < 0 ? 0 : pos);
            return result < 0 ? 0 : result;
        }

        /// <summary>
        /// Gets the actual value of the Syncfusion.UI.Xaml.Charts.ColumnSpan attached property from a given UIElement.
        /// </summary>
        /// <param name="element">The element from which to read the property value.</param>
        /// <returns>The value of the Syncfusion.UI.Xaml.Charts.ColumnSpan attached property.</returns>        
        internal int GetActualColumnSpan(UIElement element)
        {
            var count = ColumnDefinitions.Count;
            var span = GetColumnSpan(element);
            return span > count ? count : (span < 0 ? 0 : span);
        }
        /// <summary>
        /// Gets the actual value of the Syncfusion.UI.Xaml.Charts.RowSpan attached property from a given UIElement.
        /// </summary>
        /// <param name="obj">The element from which to read the property value.</param>
        /// <returns>The value of the Syncfusion.UI.Xaml.Charts.RowSpan attached property.</returns>        
        internal int GetActualRowSpan(UIElement obj)
        {
            var count = RowDefinitions.Count;
            var span = GetRowSpan(obj);
            return span > count ? count : (span < 0 ? 0 : span);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        internal void GetMinPointsDelta(List<double> values, ref double minPointsDelta, ChartSeriesBase series, bool isIndexed)
        {
            if (!series.isLinearData) // WPF 17950 Series is not rendered properly while adding data statically and dynamically between the DateTime Range
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

        /// <summary>
        /// Method used to get brush for series selection.
        /// </summary>
        /// <param name="series">The chart series.</param>
        /// <returns>The brush value for series selection.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal Brush GetSeriesSelectionBrush(ChartSeriesBase series)
        {
            if (this is ChartBase && SelectionBehaviour != null)
                return SelectionBehaviour.SeriesSelectionBrush;
            
            return null;
        }

        /// <summary>
        /// Method used to get EnableSeriesSelection property value.
        /// </summary>
        /// <returns>The bool value to enable/disable the series selection.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal bool GetEnableSeriesSelection()
        {
            if (this is ChartBase && SelectionBehaviour != null)
                return (SelectionBehaviour.Type == SelectionStyle.Series || SelectionBehaviour.Type == SelectionType.MultiSeries);

            return false;
        }

        /// <summary>
        /// Method used to get EnableSegmentSelection property value.
        /// </summary>
        /// <returns>The bool value to enable/disable the segment selection.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal bool GetEnableSegmentSelection()
        {
            if (SelectionBehaviour != null)
                return (SelectionBehaviour.Type == SelectionType.Point || SelectionBehaviour.Type == SelectionType.MultiPoint);

            return false;
        }

        internal int GetSeriesIndex(ChartSeriesBase series)
        {
            return ActualSeries.IndexOf(series);
        }

        /// <summary>
        /// Method used to set selection behavior.
        /// </summary>
        internal void SetSelectionBehaviour()
        {
            if (this is ChartBase)
            {
                var behaviors = (from behavior in Behaviors
                                 where behavior is ChartSelectionBehavior
                                 select behavior).ToList();

                if (behaviors != null && behaviors.Count > 0)
                    SelectionBehaviour = behaviors[0] as ChartSelectionBehavior;
                else
                    SelectionBehaviour = null;
            }
        }

        /// <summary>
        /// Method used to set the TooltipBehavior.
        /// </summary>
        internal void SetTooltipBehavior()
        {
            if (this is ChartBase)
            {
                var behaviors = (from behavior in Behaviors
                                 where behavior is ChartTooltipBehavior
                                 select behavior).ToList();

                if (behaviors != null && behaviors.Count > 0)
                    TooltipBehavior = behaviors[0] as ChartTooltipBehavior;
                else
                    TooltipBehavior = null;
            }
        }

        internal void ScheduleUpdate()
        {
         var _isInDesignMode = DesignMode.DesignModeEnabled;
#if WinUI_Desktop
            if (!isUpdateDispatched && !_isInDesignMode)
            {
                DispatcherQueue.TryEnqueue(() => { UpdateArea(); });
                isUpdateDispatched = true;
            }
#else
            if (updateAreaAction == null && !_isInDesignMode)
            {
                updateAreaAction = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateArea);
            }
#endif
            else if (_isInDesignMode)
                UpdateArea(true);
        }


#if NETFX_CORE
        /// <summary>
        /// Renders the chart using composition rendering.
        /// </summary>
        internal void CompositionScheduleUpdate()
        {
#if WinUI_Desktop
            if (!isUpdateDispatched)
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
            UpdateArea(false);
        }

        /// <summary>
        /// Method to raise SelectionChanged event when SeriesSelectedIndex is set at chart load time.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal void RaiseSeriesSelectionChangedEvent()
        {
            ChartSeriesBase series = null;
            var seriesCollection = GetSeriesCollection();

            if (SelectedSeriesIndex < seriesCollection.Count)
                series = seriesCollection[this.SelectedSeriesIndex] as ChartSeriesBase;

            OnSelectionChanged(new ChartSelectionChangedEventArgs()
            {
                SelectedSegment = null,
                SelectedSeries = series,
                SelectedSeriesCollection = SelectedSeriesCollection,
                SelectedIndex = SelectedSeriesIndex,
                PreviousSelectedIndex = -1,
                IsDataPointSelection = false
            });
        }

        #endregion

        #region Protected Internal Virtual Methods


        /// <summary>
        /// Called when selection changed in SfChart.
        /// </summary>
        /// <param name="eventArgs">ChartSelectionChangedEventArgs.</param>
        internal virtual void OnSelectionChanged(ChartSelectionChangedEventArgs eventArgs)
        {

            if (SelectionBehaviour != null)
            {
                if (SelectionChanged != null && eventArgs != null)
                    SelectionChanged(this, eventArgs);

                SelectionBehaviour.OnSelectionChanged(eventArgs);
            }
        }

        /// <summary>
        /// Called when selection changed in SfChart.
        /// </summary>
        /// <param name="eventArgs">ChartSelectionChangedEventArgs.</param>
        internal virtual void OnBoxSelectionChanged(ChartSelectionChangedEventArgs eventArgs)
        {
            if (SelectionChanged != null && eventArgs != null)
                SelectionChanged(this, eventArgs);

            if (SelectionBehaviour != null)
                SelectionBehaviour.OnSelectionChanged(eventArgs);

        }

        /// <summary>
        /// It's a preview event before SelectionChanged.
        /// </summary>
        /// <param name="eventArgs">ChartSelectionChangingEventArgs</param>
        internal virtual void OnSelectionChanging(ChartSelectionChangingEventArgs eventArgs)
        {
            if (SelectionChanging != null && eventArgs != null)
                SelectionChanging(this, eventArgs);

            if (SelectionBehaviour != null)
                    SelectionBehaviour.OnSelectionChanging(eventArgs);
            
        }

#endregion

#region Protected Virtual Methods

        /// <summary>
        /// Called when root panel size changed.
        /// </summary>
        /// <param name="size">The size.</param>
        internal virtual void OnRootPanelSizeChanged(Size size)
        {
            if (!IsTemplateApplied || !RootPanelDesiredSize.HasValue) return;
            UpdateAction |= UpdateAction.LayoutAndRender;

            UpdateArea(true);
        }

#endregion

#region Private Static Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private static void OnRowColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chartArea = d is ChartAxis ? (d as ChartAxis).Area
                            : (d is ChartLegend) ? (d as ChartLegend).ChartArea : null;
            if (chartArea != null)
                chartArea.ScheduleUpdate();
        }

        private static void OnSeriesSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ChartBase chartBase = d as ChartBase;
            chartBase.SeriesSelectedIndexChanged((int)args.NewValue, (int)args.OldValue);

        }

        internal static void OnSideBySideSeriesPlacementProperty(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((ChartBase)d).ScheduleUpdate();
        }

        private static void OnColorModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chartBase = d as ChartBase;
            if (chartBase.ColorModel != null)
            {
                chartBase.ColorModel.Palette = chartBase.Palette;
                chartBase.ColorModel.ChartArea = chartBase;
                chartBase.SetSeriesColorModel(chartBase.ColorModel);
            }
        }

        private static void OnPaletteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ChartBase)d).OnPaletteChanged(e);
        }

        #endregion

        #region Private Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        private void OnPaletteChanged(DependencyPropertyChangedEventArgs e)
        {
            if (ColorModel != null)
                ColorModel.Palette = Palette;
            else
                ColorModel = new ChartColorModel(Palette);

            SetSeriesColorModel(ColorModel);

            if (this.VisibleSeries.Count > 0)//ColorModel custom brush dynamic update not working in native control.-WP-610
            {
                for (int index = 0; index < VisibleSeries.Count; index++)
                {
                    (this.VisibleSeries[index] as ChartSeriesBase).Segments.Clear();
                }
                IsUpdateLegend = true;
                ScheduleUpdate();
            }
        }
        
#if NETFX_CORE
        
        /// <summary>
        /// Handler for the composition rendering.
        /// </summary>
        /// <param name="sender">The source of the composition rendering.</param>
        /// <param name="e">The details of the composition rendering.</param>
        private void SmoothCompositeRendering(object sender, object e)
        {
            CompositionTarget.Rendering -= this.SmoothCompositeRendering;
#if WinUI_Desktop
            DispatcherQueue.TryEnqueue(() => { UpdateArea(); });
            isUpdateDispatched = true;
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
        /// Performs application-defined tasks accociated with freeing, releasing, or resetting unmanaged resource in <see cref="ChartBase"/>.
        /// </summary>
        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;

            Dispose(true);
        }

        /// <summary>
        /// Disposing chart objects.
        /// </summary>
        /// <param name="disposing">Used to indicate perform dispose or not.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            DisposeAnnotation();

            if (previousSeries != null)
            {
                previousSeries.Dispose();
                previousSeries = null;
            }

            DisposeSeriesAndIndicators();
            DisposeBehaviors();
            DisposeZoomEvents();
            DisposeSelectionEvents();
            DisposeLegend();
            DisposeAxis();

            if (ColorModel != null)
            {
                ColorModel.Dispose();
            }

            SizeChanged -= OnSfChartSizeChanged;
            SizeChanged -= OnSizeChanged;

            DisposeRowColumnsDefinitions();
            DisposePanels();

            Watermark = null;
            RootPanelDesiredSize = null;
            GridLinesLayout = null;
            ChartAxisLayoutPanel = null;
            SelectionBehaviour = null;
            TooltipBehavior = null;

            if (Printing != null)
            {
                Printing.Chart = null;
                Printing = null;
            }
        }

        private void DisposePanels()
        {
            if (rootPanel != null)
            {
                rootPanel.Area = null;
                rootPanel = null;
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

            if (ChartDockPanel != null && ChartDockPanel.Children.Count > 0)
            {
                ChartDockPanel.RootElement = null;
                ChartDockPanel.Children.Clear();
                ChartDockPanel = null;
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

            SetPrimaryAxis(null);
            SetSecondaryAxis(null);

            InternalPrimaryAxis = null;
            InternalSecondaryAxis = null;
            InternalDepthAxis = null;
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
            var indicatorColl = TechnicalIndicators;

            if (seriesColl != null)
            {
                UnHookSeriesCollection(seriesColl);
            }

            if (TechnicalIndicators != null)
            {
                TechnicalIndicators.CollectionChanged -= OnTechnicalIndicatorsCollectionChanged;
            }

            SetSeriesCollection(null);
            TechnicalIndicators = null;

            if (seriesColl != null)
            {
                foreach (ChartSeries series in seriesColl)
                {
                    series.Dispose();
                }
            }

            if (indicatorColl != null)
            {
                foreach (var indicator in indicatorColl)
                {
                    indicator.Dispose();
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

                behaviors.CollectionChanged -= Behaviors_CollectionChanged;
                behaviors.Area = null;
            }

            if (zoomingToolBar != null)
            {
                zoomingToolBar.Dispose();
                zoomingToolBar = null;
            }

            chartZoomBehavior = null;
            SelectionBehaviour = null;
            TooltipBehavior = null;
        }

        private void DisposeLegend()
        {
            if (Legend != null)
            {
                if (LegendItems != null)
                {
                    foreach (var legendItem in LegendItems)
                    {
                        foreach (var item in legendItem)
                        {
                            item.Dispose();
                        }

                        legendItem.Clear();
                    }

                    LegendItems.Clear();
                }

                var chartLegend = Legend as ChartLegend;
                if (chartLegend is ChartLegend)
                {
                    chartLegend.Dispose();
                    chartLegend = null;
                }
                else
                {
                    var legendCollection = Legend as ChartLegendCollection;
                    if (legendCollection != null)
                    {
                        foreach (var legend in legendCollection)
                        {
                            legend.Dispose();
                        }

                        legendCollection.CollectionChanged -= LegendCollectionChanged;
                        legendCollection.Clear();
                    }
                }
            }
        }

        internal virtual void DisposeZoomEvents()
        {

        }

        private void DisposeAnnotation()
        {
            if (Annotations != null)
            {
                foreach (var annotation in Annotations)
                {
                    annotation.Dispose();
                }
                Annotations.CollectionChanged -= OnAnnotationsCollectionChanged;
                Annotations.Clear();
                Annotations = null;
            }

            if (AnnotationManager != null)
            {
                AnnotationManager.Dispose();
                AnnotationManager = null;
            }
        }

        #region Public Override Methods

        /// <summary>
        /// Method used to highlight selected index series.
        /// </summary>
        /// <param name="newIndex">Used to indicate current selected index.</param>
        /// <param name="oldIndex">Used to indicate previous selected index.</param>  
        internal virtual void SeriesSelectedIndexChanged(int newIndex, int oldIndex)
        {
            var seriesCollection = GetSeriesCollection();
            //Reset the oldIndex series Interior
            if (oldIndex < seriesCollection.Count && oldIndex >= 0
                && !SelectionBehaviour.EnableMultiSelection)
            {
                ChartSeries series = seriesCollection[oldIndex] as ChartSeries;
                if (SelectedSeriesCollection.Contains(series))
                    SelectedSeriesCollection.Remove(series);

                OnResetSeries(series);
            }

            if (newIndex >= 0 && GetEnableSeriesSelection() && newIndex < VisibleSeries.Count)
            {
                ChartSeries series = seriesCollection[newIndex] as ChartSeries;

                if (!SelectedSeriesCollection.Contains(series))
                    SelectedSeriesCollection.Add(series);

                //For adornment selection implementation
                if (series.adornmentInfo is ChartAdornmentInfo && series.adornmentInfo.HighlightOnSelection)
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

                ChartSelectionChangedEventArgs selectionChnagedEventArgs = new ChartSelectionChangedEventArgs()
                {
                    SelectedSegment = null,
                    SelectedSeries = series,
                    SelectedSeriesCollection = SelectedSeriesCollection,
                    SelectedIndex = newIndex,
                    PreviousSelectedIndex = oldIndex,
                    IsDataPointSelection = false,
                    IsSelected = true,
                    PreviousSelectedSegment = null,
                    PreviousSelectedSeries = null
                };

                if (oldIndex != -1)
                    selectionChnagedEventArgs.PreviousSelectedSeries = seriesCollection[oldIndex] as ChartSeriesBase;

                //Raise the selection changed event
                OnSelectionChanged(selectionChnagedEventArgs);

            }
            else if (newIndex == -1)
            {
                OnSelectionChanged(new ChartSelectionChangedEventArgs()
                {
                    SelectedSegment = null,
                    SelectedSeries = null,
                    SelectedSeriesCollection = SelectedSeriesCollection,
                    SelectedIndex = newIndex,
                    PreviousSelectedIndex = oldIndex,
                    IsDataPointSelection = false,
                    IsSelected = false,
                    PreviousSelectedSegment = null,
                    PreviousSelectedSeries = seriesCollection[oldIndex] as ChartSeriesBase
                });
            }
        }

        ///// <summary>
        ///// Converts the chart value to screen point.
        ///// </summary>
        ///// <param name="axis">The Chart axis.</param>
        ///// <param name="value">The value.</param>
        ///// <returns>The double value to point.</returns>        
        //public virtual double ValueToPoint(ChartAxis axis, double value)
        //{
        //    if (axis != null)
        //    {
        //        if (axis.Orientation.Equals(Orientation.Horizontal))
        //        {
        //            if (axis.ActualWidth == 0)
        //                return axis.ValueToCoefficientCalc(value) * axis.Area.SeriesClipRect.Width;
        //            return (axis.RenderedRect.Left - axis.Area.SeriesClipRect.Left)
        //                + (axis.ValueToCoefficientCalc(value) * axis.RenderedRect.Width);
        //        }
        //        return (axis.RenderedRect.Top - axis.Area.SeriesClipRect.Top) + (1 - axis.ValueToCoefficientCalc(value)) * axis.RenderedRect.Height;
        //    }
        //    return double.NaN;
        //}
        /// <summary>
        /// Converts Value to point.
        /// </summary>
        /// <param name="axis">The Chart axis .</param>
        /// <param name="value">The value.</param>
        /// <returns>The double value to point.</returns>
        internal virtual double ActualValueToPoint(ChartAxis axis, double value)
        {
            if (axis != null)
            {
                if (axis.Orientation == Orientation.Horizontal)
                {
                    return (axis.RenderedRect.Left - axis.Area.SeriesClipRect.Left)
                        + (axis.ValueToCoefficientCalc(value) * axis.RenderedRect.Width);
                }
                return (axis.RenderedRect.Top - axis.Area.SeriesClipRect.Top) + (1 - axis.ValueToCoefficientCalc(value)) * axis.RenderedRect.Height;
            }

            return double.NaN;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Converts Value to point.
        /// </summary>
        /// <param name="axis">The Chart axis .</param>
        /// <param name="value">The value.</param>
        /// <returns>The double value to point.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        internal double ValueToPointRelativeToAnnotation(ChartAxis axis, double value)
        {
            if (axis != null)
            {
                if (axis.Orientation == Orientation.Horizontal)
                {
                    return (axis.RenderedRect.Left)
                        + (axis.ValueToCoefficientCalc(value) * axis.RenderedRect.Width);
                }
                return (axis.RenderedRect.Top) + (1 - axis.ValueToCoefficientCalc(value)) * axis.RenderedRect.Height;
            }

            return double.NaN;
        }

        #endregion

        #region Internal Static Methods

        /// <summary>
        /// Method used to gets the byte value of given color.
        /// </summary>
        /// <param name="color">The color value which is used to byte value.</param>
        /// <returns>The byte value of given color.</returns>
        internal static int ConvertColor(Color color)
        {
            var a = color.A + 1;
            var col = (color.A << 24)
                     | ((byte)((color.R * a) >> 8) << 16)
                     | ((byte)((color.G * a) >> 8) << 8)
                     | ((byte)((color.B * a) >> 8));
            return col;
        }

        internal static double PointToAnnotationValue(ChartAxis axis, Point point)
        {
            if (axis != null)
            {
                if (axis.Orientation == Orientation.Horizontal)
                {
                    return axis.CoefficientToValueCalc((point.X - axis.RenderedRect.Left) / axis.RenderedRect.Width);
                }
                else
                {
                    return axis.CoefficientToValueCalc(1d - ((point.Y - axis.RenderedRect.Top) / axis.RenderedRect.Height));
                }

            }

            return double.NaN;
        }

        #endregion

        #region Internal Override Methods

        /// <summary>
        /// Notifies the row and column collection changed in Chart.
        /// </summary>
        /// <param name="e">NotifyCollectionChanged event arguments.</param>
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

        internal virtual void UpdateArea(bool forceUpdate)
        {
#if WinUI_Desktop
            if (isUpdateDispatched || forceUpdate)
            {
#else
            if (updateAreaAction != null || forceUpdate)
            {
                if (disposed)
                    return;
#endif
                if (AreaType == ChartAreaType.CartesianAxes)
                {
                    if (ColumnDefinitions.Count == 0)
                        ColumnDefinitions.Add(new ChartColumnDefinition());
                    if (RowDefinitions.Count == 0)
                        RowDefinitions.Add(new ChartRowDefinition());
                }
                if (VisibleSeries == null)
                    return;


                if ((UpdateAction & UpdateAction.Create) == UpdateAction.Create)
                {
                    foreach (ChartSeriesBase series in VisibleSeries)
                    {
                        if (!series.IsPointGenerated)
                            series.GeneratePoints();
                        if (series.ShowTooltip)
                            ActualShowTooltip = true;
                    }

                    //Initialize default axes for SfChart when PrimaryAxis or SecondayAxis is not set
                    InitializeDefaultAxes();

                    // For stacked grouping scenario
                    if (AreaType == ChartAreaType.CartesianAxes)
                    {
                        foreach (ChartSeriesBase series in VisibleSeries)
                        {
                            ISupportAxes2D cartesianSeries = series as ISupportAxes2D;
                            if (cartesianSeries.XAxis == null && InternalPrimaryAxis != null
                                && !InternalPrimaryAxis.RegisteredSeries.Contains(cartesianSeries))
                                InternalPrimaryAxis.RegisteredSeries.Add(cartesianSeries);
                            if (cartesianSeries.YAxis == null && InternalSecondaryAxis != null
                                && !InternalSecondaryAxis.RegisteredSeries.Contains(cartesianSeries))
                                InternalSecondaryAxis.RegisteredSeries.Add(cartesianSeries);
                        }
                    }

                    var seriesCollection = GetSeriesCollection();
                    if (seriesCollection != null && seriesCollection.Count > 0)
                    {
                        if (InternalPrimaryAxis == null || !InternalAxes.Contains(InternalPrimaryAxis))
                            InternalPrimaryAxis = (seriesCollection[0] as ChartSeriesBase).ActualXAxis;
                        if (InternalSecondaryAxis == null || !InternalAxes.Contains(InternalSecondaryAxis))
                            InternalSecondaryAxis = (seriesCollection[0] as ChartSeriesBase).ActualYAxis;
                    }

                    //Add selected index while loading 
                    if (!IsChartLoaded && SelectionBehaviour != null)
                    {
                        foreach (var series in VisibleSeries)
                        {
                            var segmentSelectableSeries = series as ISegmentSelectable;
                            if (segmentSelectableSeries != null && segmentSelectableSeries.SelectedIndex >= 0
                                && GetEnableSegmentSelection())
                            {
                                int index = segmentSelectableSeries.SelectedIndex;
                                if (!series.SelectedSegmentsIndexes.Contains(index))
                                    series.SelectedSegmentsIndexes.Add(index);
                            }
                        }

                        if (GetEnableSeriesSelection() && SelectedSeriesIndex >= 0)
                        {
                            ChartSeriesBase series = VisibleSeries[SelectedSeriesIndex];

                            if (!SelectedSeriesCollection.Contains(series))
                                SelectedSeriesCollection.Add(series);
                        }
                    }

                    if (((InternalPrimaryAxis is CategoryAxis) && (!(InternalPrimaryAxis as CategoryAxis).IsIndexed))
                       )
                        CategoryAxisHelper.GroupData(VisibleSeries);

                    foreach (ChartSeriesBase series in VisibleSeries)
                    {
                        series.Invalidate();
                    }

                    if (ActualShowTooltip && this.Tooltip == null)
                        this.Tooltip = new ChartTooltip();

                    if (TechnicalIndicators != null && AreaType == ChartAreaType.CartesianAxes)
                    {
                        foreach (FinancialTechnicalIndicator indicator in TechnicalIndicators)
                        {
                            if (!indicator.IsPointGenerated)
                            {

                                if (indicator.ItemsSource == null && VisibleSeries.Count > 0
                                    && seriesCollection != null && seriesCollection.Count > 0)
                                {
                                    ChartSeriesBase series = GetSeries(indicator.Name) ?? seriesCollection[0] as ChartSeriesBase;
                                    indicator.SetSeriesItemSource(series);
                                }
                                else
                                    indicator.GeneratePoints();
                            }
                            indicator.Invalidate();
                        }
                    }
                }

                if (IsUpdateLegend && (this.ChartDockPanel != null))
                {
                    UpdateLegend(Legend, false);
                    IsUpdateLegend = false;
                }
                if ((UpdateAction & UpdateAction.UpdateRange) == UpdateAction.UpdateRange)
                {
                    foreach (ChartSeriesBase series in VisibleSeries)
                    {
                        series.UpdateRange();
                    }

                    if (TechnicalIndicators != null)
                    {
                        foreach (FinancialTechnicalIndicator indicator in TechnicalIndicators)
                        {
                            indicator.UpdateRange();
                        }
                    }
                }


                if (RootPanelDesiredSize != null)
                {
                    if ((UpdateAction & UpdateAction.Layout) == UpdateAction.Layout)
                        LayoutAxis(RootPanelDesiredSize.Value);
                    UpdateLegendArrangeRect();
                    if ((UpdateAction & UpdateAction.Render) == UpdateAction.Render)
                    {
                        if (!IsChartLoaded)
                        {
                            ScheduleRenderSeries();
                            IsChartLoaded = true;

                            //Raise the SelectionChanged event when SeriesSelectedIndex is set at chart load time.
                            if (SelectedSeriesIndex >= 0 && VisibleSeries.Count > 0 && GetEnableSeriesSelection())
                                RaiseSeriesSelectionChangedEvent();
                        }
#if WinUI_Desktop
                        else if (!isRenderSeriesDispatched)
#else
                        else if (renderSeriesAction == null)
#endif
                        {
                            RenderSeries();
                        }
                    }
                }

                UpdateAction = UpdateAction.Invalidate;

#if WinUI_Desktop
                isUpdateDispatched = false;
#else
                updateAreaAction = null;
#endif
                if (Behaviors != null)
                {
                    foreach (var behavior in Behaviors)
                    {
                        behavior.OnLayoutUpdated();
                    }
                }
            }
        }

        internal virtual void UpdateAxisLayoutPanels()
        {
            if (internalCanvas != null)
                internalCanvas.Clip = null;
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

        /// <summary>
        /// Converts Value to Log point.
        /// </summary>
        /// <param name="axis">The Logarithmic axis.</param>
        /// <param name="value">The value.</param>
        /// <returns>The double value to point.</returns>
        internal virtual double ValueToLogPoint(ChartAxis axis, double value)
        {
            if (axis != null)
            {
                var logarithmicAxis = axis as LogarithmicAxis;
                value = logarithmicAxis != null ? Math.Log(value, logarithmicAxis.LogarithmicBase) : value;
                return ActualValueToPoint(axis, value);
            }
            return double.NaN;
        }

        #endregion

        #region Internal Methods

        internal void Behaviors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ChartBehavior behavior = (ChartBehavior)e.NewItems[0];
                    this.AddChartBehavior(behavior);
                    break;
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    behavior = (ChartBehavior)e.OldItems[0];
                    if (e.Action == NotifyCollectionChangedAction.Replace)
                    {
                        behavior = (ChartBehavior)e.NewItems[0];
                        this.AddChartBehavior(behavior);
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;
            }
        }

        internal void Axes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                var axis = e.NewItems[0] as ChartAxisBase2D;
                if (axis != null && axis.Area == null)
                {
                    axis.Area = this;
                }

            }

            if (e.OldItems != null && e.OldItems.Count > 0)
            {
                var axis = e.OldItems[0] as ChartAxisBase2D;
                if (axis != null)
                {
                    axis.Area = null;
                }
            }

            ScheduleUpdate();
        }


        internal void AnnotationsChanged(DependencyPropertyChangedEventArgs args)
        {
            AnnotationCollection newAnnotations = args.NewValue as AnnotationCollection;
            AnnotationCollection oldAnnotations = args.OldValue as AnnotationCollection;

            if (oldAnnotations != null)
                (oldAnnotations as AnnotationCollection).CollectionChanged -= OnAnnotationsCollectionChanged;

            if (newAnnotations != null)
                Annotations.CollectionChanged += OnAnnotationsCollectionChanged;

            if (newAnnotations != null && newAnnotations.Count > 0)
            {
                if (this.AnnotationManager != null)
                    AnnotationManager.Annotations = newAnnotations;
                else if (IsTemplateApplied)
                {
                    AnnotationManager = new AnnotationManager() { Chart = this, Annotations = newAnnotations };

                    //Updating the annotation clips manually in dynamic case, since it is updated only at chart schedule update.
                    UpdateAnnotationClips();
                }
            }
        }

        /// <summary>
		/// This method is called while the annotation adding dynamically.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
        internal void OnAnnotationsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (AnnotationManager == null && IsTemplateApplied)
            {
                AnnotationManager = new AnnotationManager() { Chart = this, Annotations = Annotations };

                //Updating the annotation clips manually in dynamic case, since it is updated only at chart schedule update.
                UpdateAnnotationClips();

                //Removing the CollectionChanged since the annotation manager's collection is hooked while setting Annotations.
                (sender as AnnotationCollection).CollectionChanged -= OnAnnotationsCollectionChanged;
            }
        }

        internal void ChangeToolBarState()
        {
            if (zoomingToolBar.ItemsSource != null)
            {
                var toolBarItems = (from item in zoomingToolBar.ItemsSource as List<ZoomingToolBarItem> where item is ZoomOut || item is ZoomReset select item).ToList();
                foreach (var item in toolBarItems)
                {
                    item.IsEnabled = true;
                    item.IconBackground = item.EnableColor;
                }
            }
        }

        internal void ResetToolBarState()
        {
            foreach (ZoomingToolBarItem item in zoomingToolBar.Items)
            {
                if ((item is ZoomOut) || (item is ZoomReset))
                {
                    item.IconBackground = item.DisableColor;
                    item.IsEnabled = false;
                }
                else if (item is ZoomPan)
                {
                    item.IconBackground = item.DisableColor;
                }
                else if (item is SelectionZoom)
                {
                    item.IconBackground = item.EnableColor;
                    chartZoomBehavior.InternalEnableSelectionZooming = true;
                }
            }
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

            if (series.adornmentInfo is ChartAdornmentInfo)
                series.AdornmentPresenter.ResetAdornmentSelection(null, true);

            foreach (var index in series.SelectedSegmentsIndexes)
            {
                var segmentSelectableSeries = series as ISegmentSelectable;
                if (segmentSelectableSeries != null && index > -1 && GetEnableSegmentSelection())
                {
                    if (series.adornmentInfo is ChartAdornmentInfo && series.adornmentInfo.HighlightOnSelection)
                        series.UpdateAdornmentSelection(index);

                    if (series.IsBitmapSeries)
                    {
                        series.dataPoint = series.GetDataPoint(index);

                        if (series.dataPoint != null && segmentSelectableSeries.SelectionBrush != null)
                        {
                            //Generate pixels for the particular data point
                            if (series.Segments.Count > 0) series.GeneratePixels();

                            //Set the SegmentSelectionBrush to the selected segment pixels
                            series.OnBitmapSelection(series.selectedSegmentPixels, segmentSelectableSeries.SelectionBrush, true);
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

        /// <summary>
        /// Method used to update selection in bitmap series.
        /// </summary>
        /// <param name="bitmapSeries">Used to specify bitmap series to update.</param>
        /// <param name="isReset">Used to indicate corresponding need to reset or not</param>
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
                if (bitmapSeries is FastLineBitmapSeries || bitmapSeries is FastStepLineBitmapSeries
                    || bitmapSeries is FastRangeAreaBitmapSeries)
                {
                    ScheduleRenderSeries();
                }
                else
                {
                    for (int i = 0; i < bitmapSeries.DataCount; i++)
                    {
                        bitmapSeries.dataPoint = bitmapSeries.GetDataPoint(i);

                        if (bitmapSeries.dataPoint != null)
                        {
                            //Generate pixels for the particular data point
                            if (bitmapSeries.Segments.Count > 0) bitmapSeries.GeneratePixels();

                            if (bitmapSeries is FastHiLoOpenCloseBitmapSeries)
                            {
                                uiColor = ((bitmapSeries.Segments[0] as FastHiLoOpenCloseSegment).GetSegmentBrush(i));
                            }
                            else if (bitmapSeries is FastCandleBitmapSeries)
                            {
                                uiColor = ((bitmapSeries.Segments[0] as FastCandleBitmapSegment).GetSegmentBrush(i));
                            }
                            else
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

        internal void UpdateStripLines()
        {
            if (GridLinesLayout != null && (GridLinesLayout is ChartCartesianGridLinesPanel))
            {
                (GridLinesLayout as ChartCartesianGridLinesPanel).UpdateStripLines();
                ScheduleUpdate();
            }
            else if (GridLinesLayout != null && (GridLinesLayout is ChartPolarGridLinesPanel))
            {
                (GridLinesLayout as ChartPolarGridLinesPanel).UpdateStripLines();
                ScheduleUpdate();
            }
        }

        /// <summary>
        /// Set default axes for <see cref="ChartBase"/>.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal void InitializeDefaultAxes()
        {
            var cartersianArea = this as SfCartesianChart;
            var primaryAxis = GetPrimaryAxis();
            var secondaryAxis = GetSecondaryAxis();
#if NETFX_CORE
            if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
#endif
                if (primaryAxis != null && !InternalAxes.Contains(primaryAxis))
                {
                    ClearPrimaryAxis();
                }

                if (secondaryAxis != null && !InternalAxes.Contains(secondaryAxis))
                {
                    if (cartersianArea != null)
                    {
                        ClearValue(SfCartesianChart.SecondaryAxisProperty);
                    }
                    else
                    {
#pragma warning disable CS0618 // Type or member is obsolete
                        ClearValue(SfChart.SecondaryAxisProperty);
#pragma warning restore CS0618 // Type or member is obsolete
                    }
                }
#if NETFX_CORE
            }
#endif
            var seriesPropertyCollection = GetSeriesCollection();
            if (primaryAxis == null || primaryAxis.IsDefault)
            {
                if ((seriesPropertyCollection == null || seriesPropertyCollection.Count == 0) && (TechnicalIndicators == null
                    || TechnicalIndicators.Count == 0))
                {
                    if (primaryAxis == null)
                        SetPrimaryAxis(new NumericalAxis() { IsDefault = true });
                }
                else
                {
                    if (seriesPropertyCollection != null)
                    {
                        var collectionSeries = GetChartSeriesCollection();
                        var chartSeries = (from series in collectionSeries
                                           where (series is HistogramSeries) || (series is CartesianSeries && ((series as CartesianSeries).XAxis == null))
                                           || (series is PolarRadarSeriesBase && (series as PolarRadarSeriesBase).XAxis == null)
                                           select series).ToList();
                        if (chartSeries.Count != 0)
                        {
                            //get the XAxisValueType from the each series in Series collection 
                            var valueTypes = (from series in collectionSeries
                                              where (series is HistogramSeries) ||
                                              (series is CartesianSeries && (series.ActualXAxis == null || !InternalAxes.Contains(series.ActualXAxis)
                                              || series.ActualXAxis.IsDefault) || series is PolarRadarSeriesBase && (series.ActualXAxis == null || !InternalAxes.Contains(series.ActualXAxis) || (series.ActualXAxis.IsDefault)))
                                              select series.XAxisValueType).ToList();

                            if (valueTypes.Count > 0)
                                SetPrimaryAxis(valueTypes[0]);//Set PrimaryAxis for SfChart based on XAxisValueType
                            else
                            {
                                SetPrimaryAxis(primaryAxis != null ? (seriesPropertyCollection[0] as ChartSeriesBase).ActualXAxis as ChartAxisBase2D : primaryAxis);
                                if (Annotations != null)
                                    foreach (var annotation in Annotations)
                                        annotation.SetAxisFromName();
                            }
                        }
                        else if (TechnicalIndicators != null)
                        {
                            var technicalIndicator = (from series in TechnicalIndicators
                                                      where (series is FinancialTechnicalIndicator && (series as FinancialTechnicalIndicator).YAxis == null)
                                                      select series).ToList();
                            if (technicalIndicator.Count != 0)
                            {
                                var valueTypes = (from series in TechnicalIndicators
                                                  where (series is FinancialTechnicalIndicator && (series.ActualXAxis == null || !InternalAxes.Contains(series.ActualXAxis)
                                                  || series.ActualXAxis.IsDefault))
                                                  select series.XAxisValueType).ToList();
                                if (valueTypes.Count > 0)
                                    SetPrimaryAxis(valueTypes[0]);
                            }
                            else
                                ClearPrimaryAxis();
                        }
                        else
                            ClearPrimaryAxis();
                    }
                }
            }

            if (secondaryAxis == null || secondaryAxis.IsDefault)
            {
                if ((seriesPropertyCollection == null || seriesPropertyCollection.Count == 0) && (TechnicalIndicators == null ||
                     TechnicalIndicators.Count == 0))
                {
                    if (secondaryAxis == null)
                        SetSecondaryAxis(new NumericalAxis() { IsDefault = true });
                }
                else
                {
                    if (seriesPropertyCollection != null)
                    {
                        var collectionSeries = GetChartSeriesCollection();

                        var chartSeries = (from series in collectionSeries
                                           where (series is HistogramSeries) || (series is CartesianSeries && ((series as CartesianSeries).YAxis == null))
                                           || (series is PolarRadarSeriesBase && (series as PolarRadarSeriesBase).YAxis == null)
                                           select series).ToList();
                        if (chartSeries.Count != 0)
                        {
                            var seriesCollection = (from series in collectionSeries
                                                    where (series is HistogramSeries) ||
                                                    (series is CartesianSeries && (series.ActualYAxis == null || !InternalAxes.Contains(series.ActualYAxis)
                                                    || series.ActualYAxis.IsDefault) || (series is PolarRadarSeriesBase && (series.ActualYAxis == null || !InternalAxes.Contains(series.ActualYAxis)
                                                    || series.ActualYAxis.IsDefault)))
                                                    select series).ToList();

                            if (seriesCollection.Count > 0 && secondaryAxis == null)
                            {
                                if (InternalAxes.Contains(InternalSecondaryAxis))
                                    InternalAxes.Remove(InternalSecondaryAxis);
                                SetSecondaryAxis(new NumericalAxis() { IsDefault = true });
                            }
                            else
                            {
                                SetSecondaryAxis(secondaryAxis != null ? secondaryAxis : (seriesPropertyCollection[0] as ChartSeriesBase).ActualYAxis as RangeAxisBase);
                                if (Annotations != null)
                                    foreach (var annotation in Annotations)
                                        annotation.SetAxisFromName();
                            }
                        }
                        else if (TechnicalIndicators != null)
                        {
                            var technicalIndicator = (from series in TechnicalIndicators
                                                      where (series is FinancialTechnicalIndicator && (series as FinancialTechnicalIndicator).XAxis == null)
                                                      select series).ToList();
                            if (technicalIndicator.Count != 0)
                            {
                                var seriesCollection = (from series in TechnicalIndicators
                                                        where (series is FinancialTechnicalIndicator && (series.ActualYAxis == null || !InternalAxes.Contains(series.ActualYAxis)
                                                        || series.ActualYAxis.IsDefault))
                                                        select series).ToList();
                                if (seriesCollection.Count > 0 && secondaryAxis == null)
                                    SetSecondaryAxis(new NumericalAxis() { IsDefault = true });
                                else
                                {
                                    SetSecondaryAxis(secondaryAxis != null ? secondaryAxis : TechnicalIndicators[0].ActualYAxis as RangeAxisBase);
                                }
                            }
                            else
                                ClearSecondaryAxis();
                        }
                        else
                            ClearSecondaryAxis();
                    }
                }
            }
        }

        /// <summary>
        /// Method is used to convert list collection in to hashset.
        /// </summary>
        internal void ConvertBitmapPixels()
        {
            var seriesCollection = GetSeriesCollection();
            foreach (ChartSeriesBase series in seriesCollection)
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

        internal virtual void SetPrimaryAxis(ChartAxisBase2D chartAxis)
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

        internal virtual ChartSeriesBase GetSeries(string seriesName)
        {
             return null;
        }

        internal virtual Thickness GetAreaBorderThickness()
        {
           return new Thickness().GetThickness(0, 0, 0, 0);
        }
        internal virtual ChartAxisBase2D GetPrimaryAxis()
        {
            return null;
        }

        internal virtual RangeAxisBase GetSecondaryAxis()
        {
            return null;
        }

        internal virtual void SetSecondaryAxis(RangeAxisBase chartAxis)
        {
           
        }

        internal virtual void SetSeriesColorModel(ChartColorModel chartColorModel)
        {

        }

        /// <summary>
        /// Method used to set PrimaryAxis for SfChart.
        /// </summary>
        internal void SetPrimaryAxis(ChartValueType type)
        {
            var primaryAxis = GetPrimaryAxis();
            if (primaryAxis == null && InternalAxes.Contains(InternalPrimaryAxis))
                InternalAxes.Remove(InternalPrimaryAxis);
            switch (type)
            {
                case ChartValueType.Double:
                    if (primaryAxis == null || primaryAxis.GetType() != typeof(NumericalAxis))
                        primaryAxis = new NumericalAxis() { IsDefault = true };
                    break;
                case ChartValueType.DateTime:
                    if (primaryAxis == null || primaryAxis.GetType() != typeof(DateTimeAxis))
                        primaryAxis = new DateTimeAxis() { IsDefault = true };
                    break;
                case ChartValueType.String:
                    if (primaryAxis == null || primaryAxis.GetType() != typeof(CategoryAxis))
                        primaryAxis = new CategoryAxis() { IsDefault = true };
                    break;
                case ChartValueType.TimeSpan:
                    if (primaryAxis == null || primaryAxis.GetType() != typeof(TimeSpanAxis))
                        primaryAxis = new TimeSpanAxis() { IsDefault = true };
                    break;
            }

            SetPrimaryAxis(primaryAxis);
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
                    foreach (ChartSeriesBase series in VisibleSeries.Where(item => item.Visibility == Visibility.Visible))
                    {
                        series.UpdateOnSeriesBoundChanged(size);
                    }
                }

                if (TechnicalIndicators != null)
                {
                    foreach (FinancialTechnicalIndicator indicator in TechnicalIndicators)
                    {
                        indicator.UpdateOnSeriesBoundChanged(size);
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

        internal double AreaValueToPoint(ChartAxis axis, double value)
        {
            if (axis != null)
            {
                if (axis.Orientation == Orientation.Horizontal)
                {
                    return axis.ValueToPoint(value) + GetOffetValue(axis);
                }
                return axis.ValueToPoint(value) + GetOffetValue(axis);

            }
            return double.NaN;
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

        internal double GetPercentage(IList<ISupportAxes> seriesColl, double item, int index, bool reCalculation)
        {
            double totalValues = 0;
            if (reCalculation)
            {
                if (index == 0)
                    sumItems.Clear();
                foreach (var stackingSeries in seriesColl)
                {
                    StackingSeriesBase stackingChart = stackingSeries as StackingSeriesBase;
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

       
        /// <summary>
        /// Event invokes when the plot area size changed.
        /// </summary>
        /// <param name="args">Which indicates <see cref="ChartSeriesBoundsEventArgs"/>.</param>
        internal virtual void OnSeriesBoundsChanged(ChartSeriesBoundsEventArgs args)
        {
            CreateFastRenderSurface();

            if (InternalCanvas != null)
            {
                InternalCanvas.Clip = new RectangleGeometry()
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

        /// <summary>
        /// Occurs when zooming position changed in chart.
        /// </summary>
        /// <param name="args">ZoomChangedEventArgs</param>
        internal virtual void OnZoomChanged(ZoomChangedEventArgs args)
        {

        }

        /// <summary>
        /// Occurs when zooming takes place in chart.
        /// </summary>
        /// <param name="args">ZoomChangingEventArgs</param>
        internal virtual void OnZoomChanging(ZoomChangingEventArgs args)
        {

        }

        /// <summary>
        /// Occurs at the start of selection zooming.
        /// </summary>
        /// <param name="args">SelectionZoomingStartEventArgs</param>
        internal virtual void OnSelectionZoomingStart(SelectionZoomingStartEventArgs args)
        {

        }

        /// <summary>
        /// Occurs at the end of selection zooming.
        /// </summary>
        /// <param name="args">SelectionZoomingEndEventArgs</param>
        internal virtual void OnSelectionZoomingEnd(SelectionZoomingEndEventArgs args)
        {

        }

        /// <summary>
        /// Occurs while selection zooming in chart.
        /// </summary>
        /// <param name="args">SelectionZoomingDeltaEventArgs</param>
        internal virtual void OnSelectionZoomingDelta(SelectionZoomingDeltaEventArgs args)
        {

        }

        /// <summary>
        /// Occurs when panning position changed in chart.
        /// </summary>
        /// <param name="args">PanChangedEventArgs</param>
        internal virtual void OnPanChanged(PanChangedEventArgs args)
        {

        }

        /// <summary>
        /// Occurs when panning takes place in chart.
        /// </summary>
        /// <param name="args">PanChangingEventArgs</param>
        internal virtual void OnPanChanging(PanChangingEventArgs args)
        {

        }

        /// <summary>
        /// Occurs when zoom is reset.
        /// </summary>
        /// <param name="args">ResetZoomEventArgs.</param>
        internal virtual void OnResetZoom(ResetZoomEventArgs args)
        {

        }

        #endregion

        #region Protected Internal Methods
        /// <summary>
        /// Used to add the zooming toolbar in canvas.
        /// </summary>
        /// <param name="chartZoomingToolBar">ZoomingToolBar value.</param>
        /// <param name="zoomBehavior">ChartZoomPanBehavior instance.</param>
        internal void AddZoomToolBar(ZoomingToolBar chartZoomingToolBar, ChartZoomPanBehavior zoomBehavior)
        {
            zoomingToolBar = chartZoomingToolBar;
            chartZoomBehavior = zoomBehavior;
            this.ToolkitCanvas.Children.Add(chartZoomingToolBar);
        }

        /// <summary>
        /// Used to remove the zooming toolbar in canvas.
        /// </summary>
        /// <param name="chartZoomingToolBar">ZoomingToolBar value</param>
        internal void RemoveZoomToolBar(ZoomingToolBar chartZoomingToolBar)
        {
            this.ToolkitCanvas.Children.Remove(chartZoomingToolBar);
            zoomingToolBar = null;
            chartZoomBehavior = null;
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Invoke to render <see cref="ChartBase"/>.
        /// </summary>
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
            InternalCanvas = GetTemplateChild("SyncfusionChartInternalCanvas") as Panel;
            AdorningCanvas = GetTemplateChild("SyncfusionChartAdorningCanvas") as Canvas;
            ChartDockPanel = GetTemplateChild("SyncfusionChartDockPanel") as ChartDockPanel;
            rootPanel = GetTemplateChild("SyncfusionChartRootPanel") as ChartRootPanel;
            rootPanel.Area = this;
            ChartAnnotationCanvas = this.GetTemplateChild("SyncfusionChartAnnotationCanvas") as Canvas;
            SeriesAnnotationCanvas = this.GetTemplateChild("SyncfusionChartSeriesAnnotationCanvas") as Canvas;

            if (Annotations != null && Annotations.Count > 0)
                AnnotationManager = new AnnotationManager { Chart = this, Annotations = this.Annotations };

            BottomAdorningCanvas = this.GetTemplateChild("SyncfusionChartBottomAdorningCanvas") as Canvas;
            ToolkitCanvas = this.GetTemplateChild("SyncfusionChartToolkitCanvas") as Canvas;

            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.AdorningCanvas = AdorningCanvas;
                behavior.BottomAdorningCanvas = BottomAdorningCanvas;
                behavior.InternalAttachElements();
            }

            var seriesCollection = GetSeriesCollection();

            if (seriesCollection != null)
            {
                foreach (ChartSeries series in seriesCollection)
                {
                    series.Area = this;
                    if (series.ShowTooltip)
                    {
                        ActualShowTooltip = true;
                    }
                }

                if (ActualShowTooltip)
                {
                    ChartSeriesBase.AddTooltipBehavior(this);
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
                        tooltip.ChartArea = this;
                    }
                }
            }

            if (TechnicalIndicators != null)
            {
                foreach (FinancialTechnicalIndicator indicator in TechnicalIndicators)
                {
                    indicator.Area = this;
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
                    foreach (ChartSeriesBase series in seriesCollection)
                    {
                        this.seriesPresenter.Children.Add(series);
                    }
                }
                if (TechnicalIndicators != null)
                {
                    foreach (FinancialTechnicalIndicator indicator in TechnicalIndicators)
                    {
                        if (!this.seriesPresenter.Children.Contains(indicator))
                            this.seriesPresenter.Children.Add(indicator);
                    }
                }
            }

            UpdateLegend(Legend, true);
            if (Watermark != null)
                AddOrRemoveWatermark(Watermark, null);
            IsTemplateApplied = true;
        }

        /// <summary>
        /// Provides the behavior for the Measure pass of Silverlight layout. Classes can override this method to define their own Measure pass behavior.
        /// </summary>
        /// <returns>
        /// The size that this object determines it needs during layout, based on its calculations of the allocated sizes for child objects; or based on other considerations, such as a fixed container size.
        /// </returns>
        /// <param name="availableSize">The size value.</param>
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


        /// <summary>
        /// called when lost focus from the <see cref="ChartBase"/>.
        /// </summary>
        /// <param name="e">RoutedEventArgs.</param>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnLostFocus(e);
            }

            base.OnLostFocus(e);
        }
        /// <summary>
        /// Called when got focus in the <see cref="ChartBase"/>.
        /// </summary>
        /// <param name="e">RoutedEventArgs.</param>
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnGotFocus(e);
            }

            base.OnGotFocus(e);
        }

        /// <summary>
        /// Called when point capture lost in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs.</param>
        protected override void OnPointerCaptureLost(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerCaptureLost(e);
            }

            base.OnPointerCaptureLost(e);
        }

        /// <summary>
        /// Called when tapped in the chart.
        /// </summary>
        /// <param name="e">TappedRoutedEventArgs.</param>
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnTapped(e);
            }

            base.OnTapped(e);
        }

        /// <summary>
        /// Called when right click in the chart.
        /// </summary>
        /// <param name="e">RightTappedRoutedEventArgs.</param>
        protected override void OnRightTapped(RightTappedRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnRightTapped(e);
            }

            base.OnRightTapped(e);
        }

        /// <summary>
        /// Called when pointer wheel changed in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs.</param>
        protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerWheelChanged(e);
            }

            base.OnPointerWheelChanged(e);
        }

        /// <summary>
        /// Called when pointer exited from the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs.</param>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerExited(e);
            }

            base.OnPointerExited(e);
        }

        /// <summary>
        /// Called when pointer entered in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs.</param>
        protected override void OnPointerEntered(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerEntered(e);
            }

            base.OnPointerEntered(e);
        }

        /// <summary>
        /// Called when pointer cancelled in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs.</param>
        protected override void OnPointerCanceled(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerCanceled(e);
            }

            base.OnPointerCanceled(e);
        }

        /// <summary>
        /// called when pointer key up in the chart.
        /// </summary>
        /// <param name="e">KeyRoutedEventArgs.</param>
        protected override void OnKeyUp(KeyRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnKeyUp(e);
            }

            base.OnKeyUp(e);
        }

        /// <summary>
        /// Called when key down in the chart.
        /// </summary>
        /// <param name="e">KeyRoutedEventArgs.</param>
        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnKeyDown(e);
            }

            base.OnKeyDown(e);
        }

        /// <summary>
        /// Called when holding the pointer in the chart.
        /// </summary>
        /// <param name="e">HoldingRoutedEventArgs.</param>
        protected override void OnHolding(HoldingRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnHolding(e);
            }

            base.OnHolding(e);
        }

        /// <summary>
        /// called when manipulation starting in the chart.
        /// </summary>
        /// <param name="e">ManipulationStartingRoutedEventArgs.</param>
        protected override void OnManipulationStarting(ManipulationStartingRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnManipulationStarting(e);
            }

            base.OnManipulationStarting(e);
        }

        /// <summary>
        /// called when manipulation started in the sfchart.
        /// </summary>
        /// <param name="e">ManipulationStartedRoutedEventArgs.</param>
        protected override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnManipulationStarted(e);
            }

            base.OnManipulationStarted(e);
        }

        /// <summary>
        /// called when manipulation inertia starting in the chart.
        /// </summary>
        /// <param name="e">ManipulationInertiaStartingRoutedEventArgs.</param>
        protected override void OnManipulationInertiaStarting(ManipulationInertiaStartingRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnManipulationInertiaStarting(e);
            }

            base.OnManipulationInertiaStarting(e);
        }

        /// <summary>
        /// Called when manipulation completed in the chart.
        /// </summary>
        /// <param name="e">ManipulationCompletedRoutedEventArgs.</param>
        protected override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnManipulationCompleted(e);
            }

            base.OnManipulationCompleted(e);
        }

        /// <summary>
        /// Called when manipulation delta changed.
        /// </summary>
        /// <param name="e">ManipulationDeltaRoutedEventArgs.</param>
        protected override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnManipulationDelta(e);
            }

            base.OnManipulationDelta(e);
        }

        /// <summary>
        /// Called when pointer pressed in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if (Annotations.FirstOrDefault(item => item is ShapeAnnotation && (item as ShapeAnnotation).CanDrag) != null
                || Behaviors.FirstOrDefault(item => item is ChartZoomPanBehavior) != null)
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

            foreach (ChartAxisBase2D axis in InternalAxes)
            {
                var pointerPoint = e.GetCurrentPoint(chartAxisPanel);

                if (this.AreaType == ChartAreaType.PolarAxes)
                {
                    if (axis.Orientation == Orientation.Horizontal)
                    {
                        if (!(e.OriginalSource is ChartCartesianAxisPanel))
                            axis.SetLabelDownArguments(e.OriginalSource);
                    }
                }
                else
                {
                    if (!(e.OriginalSource is ChartCartesianAxisPanel))
                    {
                        pointerPoint = e.GetCurrentPoint(chartAxisPanel);
                        var labelsPanel = (axis.axisLabelsPanel as ChartCartesianAxisLabelsPanel);
                        if (labelsPanel != null)
                        {
                            var rect = labelsPanel.Bounds;
                            if (rect.Contains(pointerPoint.Position))
                                axis.SetLabelDownArguments(e.OriginalSource);
                        }
                    }
                }
            }
            base.OnPointerPressed(e);
        }

        /// <summary>
        /// Called when pointer moved in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs.</param>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerMoved(e);
            }

            base.OnPointerMoved(e);
        }
        /// <summary>
        /// Called when pointer released in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs.</param>
        protected override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnPointerReleased(e);
            }

            base.OnPointerReleased(e);
        }
        /// <summary>
        /// Called when double tapped in the chart.
        /// </summary>
        /// <param name="e">DoubleTappedRoutedEventArgs.</param>
        protected override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnDoubleTapped(e);
            }

            base.OnDoubleTapped(e);
        }

        /// <summary>
        /// Called when drop the pointer in the chart.
        /// </summary>
        /// <param name="e">DragEventArgs.</param>
        protected override void OnDrop(DragEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnDrop(e);
            }

            base.OnDrop(e);
        }
        /// <summary>
        /// Called when drag over the chart.
        /// </summary>
        /// <param name="e">DragEventArgs.</param>
        protected override void OnDragOver(DragEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnDragOver(e);
            }

            base.OnDragOver(e);
        }
        /// <summary>
        /// Called when drag leave from the chart.
        /// </summary>
        /// <param name="e">DragEventArgs.</param>
        protected override void OnDragLeave(DragEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnDragLeave(e);
            }

            base.OnDragLeave(e);
        }
        /// <summary>
        /// Called when drag enter into the chart.
        /// </summary>
        /// <param name="e">DragEventArgs.</param>
        protected override void OnDragEnter(DragEventArgs e)
        {
            foreach (ChartBehavior behavior in Behaviors)
            {
                behavior.OnDragEnter(e);
            }

            base.OnDragEnter(e);
        }

        #endregion

        #region Private Static Methods

        internal static void OnPrimaryAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chartAxis = e.NewValue as ChartAxis;

            var oldAxis = e.OldValue as ChartAxis;

            ChartBase chartArea = d as ChartBase;

            if (chartAxis != null)
            {
                chartAxis.Area = chartArea;
                chartAxis.Orientation = Orientation.Horizontal;
                chartArea.InternalPrimaryAxis = (ChartAxis)e.NewValue;
                chartAxis.VisibleRangeChanged += chartAxis.OnVisibleRangeChanged;
            }

            if (oldAxis != null)
            {
                if (chartArea != null && chartArea.InternalAxes != null && chartArea.InternalAxes.Contains(oldAxis))
                {
                    chartArea.InternalAxes.RemoveItem(oldAxis, chartArea.DependentSeriesAxes.Contains(oldAxis));
                    chartArea.DependentSeriesAxes.Remove(oldAxis);
                }

                oldAxis.VisibleRangeChanged -= oldAxis.OnVisibleRangeChanged;
                oldAxis.RegisteredSeries.Clear();
                oldAxis.Dispose();
            }
            var seriesCollection = chartArea.GetSeriesCollection();
            if (seriesCollection != null && chartAxis != null)
                foreach (ChartSeries series in seriesCollection)
                {
                    var cartesianSeries = series as CartesianSeries;
                    PolarRadarSeriesBase polarSeriesBase = series as PolarRadarSeriesBase;
                    if ((cartesianSeries != null && cartesianSeries.XAxis == null) || (polarSeriesBase != null && polarSeriesBase.XAxis == null))
                    {
                        CheckSeriesTransposition(series);
                        chartAxis.RegisteredSeries.Add((ISupportAxes)series);
                    }
                }

            if (chartArea.TechnicalIndicators != null && chartAxis != null)
                foreach (var series in chartArea.TechnicalIndicators)
                {
                    var financialTechnicalIndicator = series as FinancialTechnicalIndicator;
                    if (financialTechnicalIndicator != null && financialTechnicalIndicator.XAxis == null)
                    {
                        CheckSeriesTransposition(series);
                        chartAxis.RegisteredSeries.Add((ISupportAxes)series);
                    }
                }

            chartArea.OnAxisChanged(e);

        }

        internal static void OnSecondaryAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chartAxis = e.NewValue as ChartAxis;

            var oldAxis = e.OldValue as ChartAxis;

            ChartBase chartArea = d as ChartBase;

            if (chartAxis != null)
            {
                chartAxis.Area = chartArea;
                chartAxis.Orientation = Orientation.Vertical;
                chartArea.InternalSecondaryAxis = (ChartAxis)e.NewValue;
                chartAxis.IsSecondaryAxis = true;
            }

            if (oldAxis != null)
            {
                var axis = oldAxis as NumericalAxis;
                if (axis != null && axis.AxisScaleBreaks != null && axis.AxisScaleBreaks.Count >= 0)
                    axis.ClearBreakElements();
                if (chartArea != null && chartArea.InternalAxes != null && chartArea.InternalAxes.Contains(oldAxis))
                {
                    chartArea.InternalAxes.RemoveItem(oldAxis, chartArea.DependentSeriesAxes.Contains(oldAxis));
                    chartArea.DependentSeriesAxes.Remove(oldAxis);
                }

                oldAxis.RegisteredSeries.Clear();
                oldAxis.Dispose();
            }
            var seriesCollection = chartArea.GetSeriesCollection();
            if (seriesCollection != null && chartAxis != null)
                foreach (ChartSeries series in seriesCollection)
                {
                    var cartesianSeries = series as CartesianSeries;
                    if (cartesianSeries != null && cartesianSeries.YAxis == null)
                    {
                        CheckSeriesTransposition(series);
                        chartAxis.RegisteredSeries.Add((ISupportAxes)series);
                    }
                }

            if (chartArea.TechnicalIndicators != null && chartAxis != null)
                foreach (var series in chartArea.TechnicalIndicators)
                {
                    var financialTechnicalIndicator = series as FinancialTechnicalIndicator;
                    if (financialTechnicalIndicator != null && financialTechnicalIndicator.YAxis == null)
                    {
                        CheckSeriesTransposition(series);
                        chartAxis.RegisteredSeries.Add((ISupportAxes)series);
                    }
                }

            chartArea.OnAxisChanged(e);

        }

        private static void OnWatermarkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartBase).OnWaterMarkChanged(e);
        }

        private void OnBehaviorPropertyChanged(ChartBehaviorsCollection oldValue, ChartBehaviorsCollection newValue)
        {
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= Behaviors_CollectionChanged;
            }

            if (newValue != null)
            {
                newValue.CollectionChanged += Behaviors_CollectionChanged;

                foreach (var behavior in newValue)
                {
                    this.AddChartBehavior(behavior);
                }
            }
        }

        private void AddChartBehavior(ChartBehavior behavior)
        {
            if (behavior is ChartTooltipBehavior)
            {
                behavior.ChartArea = this;
                RemoveDefaultTooltipBehavior((ChartTooltipBehavior)behavior);
            }
        }

        private void RemoveDefaultTooltipBehavior(ChartTooltipBehavior tooltipBehavior)
        {
            int index = -1;

            foreach (ChartBehavior behavior in Behaviors)
            {
                if (behavior is ChartTooltipBehavior && behavior != tooltipBehavior)
                {
                    index = Behaviors.IndexOf(behavior);
                    break;
                }
            }

            if (index >= 0 && index < Behaviors.Count - 1)
            {
                Behaviors.RemoveAt(index);
                TooltipBehavior = null;
            }
        }

        internal static void OnSeriesPropertyCollectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartBase).OnSeriesPropertyCollectionChanged(e);
        }

        private static void OnTechnicalIndicatorsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartBase).OnTechnicalIndicatorsPropertyChanged(e);
        }

        private static void OnAnnotationsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as ChartBase).AnnotationsChanged(args);
        }

        private static void CheckSeriesTransposition(ChartSeries series)
        {
            if (series.ActualXAxis == null || series.ActualYAxis == null) return;
            series.ActualXAxis.Orientation = series.IsActualTransposed ? Orientation.Vertical : Orientation.Horizontal;
            series.ActualYAxis.Orientation = series.IsActualTransposed ? Orientation.Horizontal : Orientation.Vertical;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to remove the border lines in row and column definition.
        /// </summary>
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

        private void OnWaterMarkChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.ChartDockPanel != null)
            {
                AddOrRemoveWatermark(e.NewValue as Watermark, e.OldValue as Watermark);
            }
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
                            cartesianSeries.XAxis = null;
                            cartesianSeries.YAxis = null;
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
                    else if (this is SfCartesianChart || this is SfChart)
#pragma warning restore CS0618 // Type or member is obsolete
                        AreaType = ChartAreaType.CartesianAxes;
                    else
                        AreaType = ChartAreaType.None;

                    foreach (ChartSeries series in seriesCollection)
                    {
                        series.UpdateLegendIconTemplate(false);
                        series.Area = this;

                        var supportAxisSeries = series as ISupportAxes;
                        if (series.ActualXAxis != null)
                        {
                            series.ActualXAxis.Area = this;
                            if (supportAxisSeries != null && !series.ActualXAxis.RegisteredSeries.Contains(supportAxisSeries))
                                series.ActualXAxis.RegisteredSeries.Add(supportAxisSeries);
                            if (!this.InternalAxes.Contains(series.ActualXAxis))
                            {
                                this.InternalAxes.Add(series.ActualXAxis);
                                this.DependentSeriesAxes.Add(series.ActualXAxis);
                            }
                        }
                        if (series.ActualYAxis != null)
                        {
                            series.ActualYAxis.Area = this;
                            if (supportAxisSeries != null && !series.ActualYAxis.RegisteredSeries.Contains(supportAxisSeries))
                                series.ActualYAxis.RegisteredSeries.Add(supportAxisSeries);
                            if (!this.InternalAxes.Contains(series.ActualYAxis))
                            {
                                this.InternalAxes.Add(series.ActualYAxis);
                                this.DependentSeriesAxes.Add(series.ActualYAxis);
                            }
                        }
                        CheckSeriesTransposition(series);
                        if (this.seriesPresenter != null && !this.seriesPresenter.Children.Contains(series))
                        {
                            this.seriesPresenter.Children.Add(series);
                        }
                        if (series.IsSeriesVisible)
                        {
                            if (AreaType == ChartAreaType.PolarAxes && (series is PolarSeries || series is RadarSeries))
                            {
                                VisibleSeries.Add(series);
                            }
                            else if (AreaType == ChartAreaType.None && (series is AccumulationSeriesBase))
                            {
                                VisibleSeries.Add(series);
                            }
                            else if (AreaType == ChartAreaType.CartesianAxes && (series is CartesianSeries || series is HistogramSeries))
                            {
                                VisibleSeries.Add(series);
                            }
                        }
                        ActualSeries.Add(series);
                    }
                }
                UpdateLegend(Legend, false);
                AddOrRemoveBitmap();
            }
            else
                UpdateLegend(Legend, true);

            ScheduleUpdate();
        }

        private void RemoveVisualChild()
        {
            if (seriesPresenter != null)
            {
                for (int i = seriesPresenter.Children.Count - 1; i >= 0; i--)
                {
                    if (seriesPresenter.Children[i] is AdornmentSeries ||
                        seriesPresenter.Children[i] is HistogramSeries)
                    {
                        var series = seriesPresenter.Children[i] as ISupportAxes;
                        if (series != null)
                        {
                            var cartesianSeries = series as CartesianSeries;
                            if (cartesianSeries != null)
                            {
                                if (cartesianSeries.ActualXAxis != null)
                                    cartesianSeries.ActualXAxis.RegisteredSeries.Clear();

                                if (cartesianSeries.ActualYAxis != null)
                                    cartesianSeries.ActualYAxis.RegisteredSeries.Clear();

                                if (InternalPrimaryAxis == cartesianSeries.XAxis)
                                    InternalPrimaryAxis = null;
                                if (InternalSecondaryAxis == cartesianSeries.YAxis)
                                    InternalSecondaryAxis = null;
                            }
                        }
                        seriesPresenter.Children.RemoveAt(i);
                    }
                }
            }
            VisibleSeries.Clear();
            ActualSeries.Clear();
        }

        private void OnTechnicalIndicatorsPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                if (seriesPresenter != null)
                {
                    AddOrRemoveBitmap();
                    for (int i = seriesPresenter.Children.Count - 1; i >= 0; i--)
                    {
                        if (seriesPresenter.Children[i] is FinancialTechnicalIndicator)
                        {
                            var series = seriesPresenter.Children[i] as ISupportAxes2D;
                            series.XAxis = null;
                            series.YAxis = null;
                            seriesPresenter.Children.RemoveAt(i);
                        }
                    }
                }

                (e.OldValue as ObservableCollection<ChartSeries>).CollectionChanged -= OnTechnicalIndicatorsCollectionChanged;
            }

            if (TechnicalIndicators != null)
            {
                TechnicalIndicators.CollectionChanged += OnTechnicalIndicatorsCollectionChanged;
                if (TechnicalIndicators.Count > 0)
                {
                    FinancialTechnicalIndicator financialIndicator;
                    foreach (var indicator in TechnicalIndicators)
                    {
                        indicator.Area = this;
                        financialIndicator = indicator as FinancialTechnicalIndicator;
                        if (financialIndicator.XAxis != null && !InternalAxes.Contains(financialIndicator.XAxis))
                        {
                            financialIndicator.XAxis.Area = this;
                            InternalAxes.Add(financialIndicator.XAxis);
                            if (!financialIndicator.XAxis.RegisteredSeries.Contains(financialIndicator))
                                financialIndicator.XAxis.RegisteredSeries.Add(financialIndicator);
                        }
                        if (financialIndicator.YAxis != null && !InternalAxes.Contains(financialIndicator.YAxis))
                        {
                            financialIndicator.YAxis.Area = this;
                            InternalAxes.Add(financialIndicator.YAxis);
                            if (!financialIndicator.YAxis.RegisteredSeries.Contains(financialIndicator))
                                financialIndicator.YAxis.RegisteredSeries.Add(financialIndicator);
                        }
                        else
                        {
                            if (!(financialIndicator is SimpleAverageIndicator || financialIndicator is TriangularAverageIndicator
                                || financialIndicator is BollingerBandIndicator || financialIndicator is ExponentialAverageIndicator))
                                financialIndicator.YAxis = new NumericalAxis() { OpposedPosition = true, RangePadding = NumericalPadding.Round };
                        }
                        if (seriesPresenter != null && !seriesPresenter.Children.Contains(indicator))
                        {
                            seriesPresenter.Children.Add(indicator);
                        }
                    }
                }
                AddOrRemoveBitmap();
            }
            ScheduleUpdate();
        }

        private void UpdateAnnotationClips()
        {
            //Updating the clips for the annotations.
            foreach (var annotation in Annotations)
            {
                annotation.SetAxisFromName();
            }

            this.ChartAnnotationCanvas.Clip = new RectangleGeometry() { Rect = new Rect(new Point(0, 0), AvailableSize) };
            this.SeriesAnnotationCanvas.Clip = new RectangleGeometry() { Rect = this.SeriesClipRect };
        }

        private void OnAxisChanged(DependencyPropertyChangedEventArgs e)
        {
            var chartAxis = e.NewValue as ChartAxis;

            if (InternalAxes != null && chartAxis != null && !InternalAxes.Contains(chartAxis))
            {
                chartAxis.Area = this;
                InternalAxes.Insert(0, chartAxis);
                DependentSeriesAxes.Add(chartAxis);
            }

            if (AnnotationManager != null)
                AnnotationManager.Annotations = Annotations;
            ScheduleUpdate();
        }

#if NETFX_CORE

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
                ChartSeries series = VisibleSeries[i] as ChartSeries;

                if (!isTap)
                {
                    if (series.ShowTooltip && (series.ActualTooltipPosition == TooltipPosition.Pointer || !series.Timer.IsEnabled))
                    {
                        series.RemoveTooltip();
                    }
                }
            }
            isTap = false;
        }
#else
        void fastRenderDevice_MouseLeave(object sender, MouseEventArgs e)

        {
            for (int i = VisibleSeries.Count - 1; i >= 0; i--)
            {
                ChartSeries series = VisibleSeries[i] as ChartSeries;

                if (series.ShowTooltip && (series.ActualTooltipPosition == TooltipPosition.Pointer || !series.Timer.IsEnabled))
                {
                    series.RemoveTooltip();
                }
            }
        }

#endif
        private void OnTechnicalIndicatorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (seriesPresenter != null)
                {
                    for (int i = seriesPresenter.Children.Count - 1; i >= 0; i--)
                    {
                        if (seriesPresenter.Children[i] is FinancialTechnicalIndicator)
                        {
                            var series = seriesPresenter.Children[i] as ISupportAxes;
                            UnRegisterSeries(series);
                            seriesPresenter.Children.RemoveAt(i);
                        }
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                FinancialTechnicalIndicator indicator = e.OldItems[0] as FinancialTechnicalIndicator;
                if (indicator == null) return;
                if (indicator.ActualYAxis.RegisteredSeries != null &&
                    indicator.ActualYAxis.RegisteredSeries.Contains(indicator))
                {
                    indicator.YAxis = null;
                    indicator.XAxis = null;
                }
                if (seriesPresenter.Children.Contains(indicator))
                    seriesPresenter.Children.Remove(indicator);
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (FinancialTechnicalIndicator indicator in e.NewItems)
                {
                    indicator.UpdateLegendIconTemplate(false);
                    indicator.Area = this;
                    if (indicator.XAxis != null && !this.InternalAxes.Contains(indicator.XAxis))
                    {
                        indicator.XAxis.Area = this;
                        this.InternalAxes.Add(indicator.XAxis);
                        if (!indicator.XAxis.RegisteredSeries.Contains(indicator))
                            indicator.XAxis.RegisteredSeries.Add(indicator);
                    }
                    if (indicator.YAxis != null && !this.InternalAxes.Contains(indicator.YAxis))
                    {
                        indicator.YAxis.Area = this;
                        this.InternalAxes.Add(indicator.YAxis);
                        if (!indicator.YAxis.RegisteredSeries.Contains(indicator))
                            indicator.YAxis.RegisteredSeries.Add(indicator);
                    }
                    else if (!(indicator is SimpleAverageIndicator || indicator is TriangularAverageIndicator
                                || indicator is BollingerBandIndicator || indicator is ExponentialAverageIndicator))
                        indicator.YAxis = new NumericalAxis() { OpposedPosition = true, RangePadding = NumericalPadding.Round };
                    if (this.seriesPresenter != null && !this.seriesPresenter.Children.Contains(indicator))
                    {
                        this.seriesPresenter.Children.Add(indicator);
                    }
                }
            }
            AddOrRemoveBitmap();
            ScheduleUpdate();
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
                        if (seriesPresenter.Children[i] is AdornmentSeries ||
                            seriesPresenter.Children[i] is HistogramSeries)
                        {
                            if (seriesPresenter.Children[i] is ISupportAxes)
                            {
                                var series = seriesPresenter.Children[i] as ISupportAxes;
                                UnRegisterSeries(series);
                            }

                            var currentSeries = seriesPresenter.Children[i];
                            var doughnutSeries = currentSeries as DoughnutSeries;
                            if (doughnutSeries != null)
                            {
                                doughnutSeries.RemoveCenterView(doughnutSeries.CenterView);
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

                if (LegendItems != null)
                {
                    if (Legend is ChartLegendCollection)
                        foreach (var item in LegendItems)
                            item.Clear();
                    else if (LegendItems.Count > 0)
                        LegendItems[0].Clear();
                }
                ActualSeries.Clear();
                VisibleSeries.Clear();
                SelectedSeriesCollection.Clear();
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
            {
                ChartSeriesBase series = e.OldItems[0] as ChartSeriesBase;

                if (VisibleSeries.Contains(series))
                    VisibleSeries.Remove(series);
                if (ActualSeries.Contains(series))
                    ActualSeries.Remove(series);
                if (SelectedSeriesCollection.Contains(series))
                    SelectedSeriesCollection.Remove(series);

                UnRegisterSeries(series as ISupportAxes);
                if (series is AccumulationSeriesBase || series is CircularSeries)
                {
                    if (Legend != null && LegendItems != null)
                    {
                        var filterSeriesLegend = LegendItems.Where(item => item.Where(it => it.Series == series).Count() > 0).ToList();
                        if (filterSeriesLegend.Count > 0)
                        {
                            filterSeriesLegend[0].Clear();
                        }
                    }
                }
                else
                {
                    if (Legend != null && LegendItems != null)
                    {
                        var filterSeriesLegend = LegendItems.Where(item => item.Where(it => it.Series == series).Count() > 0).ToList();

                        if (filterSeriesLegend.Count > 0)
                        {
                            var itemIndex = filterSeriesLegend[0].IndexOf(filterSeriesLegend[0].Where(item => item.Series == series).FirstOrDefault());
                            var index = LegendItems.IndexOf(filterSeriesLegend[0]);
                            if (filterSeriesLegend[0].Count() > 0 && LegendItems[index].Contains(filterSeriesLegend[0][itemIndex]))
                            {
                                LegendItems[index].Remove(filterSeriesLegend[0][itemIndex]);
                                if (series is CartesianSeries)
                                    foreach (var item in (series as CartesianSeries).Trendlines)
                                    {
                                        var containlegendtrenditem = LegendItems[index].Where(it => it.Trendline == item).ToList();
                                        if (containlegendtrenditem.Count() > 0 && LegendItems[index].Contains(containlegendtrenditem[0]))
                                        {
                                            LegendItems[index].Remove(containlegendtrenditem[0]);
                                        }
                                    }
                            }
                        }
                    }
                }
                if (this.seriesPresenter != null)
                    this.seriesPresenter.Children.Remove(series);
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
                        if (this is SfPolarChart || ((this is SfChart) && (this as SfChart).Series[0] is PolarRadarSeriesBase))
                            AreaType = ChartAreaType.PolarAxes;
                        else if (this is SfCartesianChart || ((this is SfChart) && (this as SfChart).Series[0] is CartesianSeries))
                            AreaType = ChartAreaType.CartesianAxes;
                        else
                            AreaType = ChartAreaType.None;
                        UpdateVisibleSeries(seriesCollection, e.NewStartingIndex);
                    }
                }
                else if (VisibleSeries.Count == 0 && seriesCollection.Count > 0)
                {
                    if (this is SfPolarChart || ((this is SfChart) && (this as SfChart).Series[0] is PolarRadarSeriesBase))
                        AreaType = ChartAreaType.PolarAxes;
                    else if (this is SfCartesianChart || ((this is SfChart) && (this as SfChart).Series[0] is CartesianSeries))
                        AreaType = ChartAreaType.CartesianAxes;
                    else
                        AreaType = ChartAreaType.None;
                }
                else if (VisibleSeries.Count == 0 && seriesCollection.Count == 0)
                {
                    AreaType = ChartAreaType.CartesianAxes;
                }

                //WP-795: update the remaining series Chart Palette while remove the series
                if (Palette != ChartColorPalette.None)
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
                    if (this is SfPolarChart || ((this is SfChart) && (this as SfChart).Series[0] is PolarRadarSeriesBase))
                        AreaType = ChartAreaType.PolarAxes;
                    else if (this is SfCartesianChart || ((this is SfChart) && (this as SfChart).Series[0] is CartesianSeries))
                        AreaType = ChartAreaType.CartesianAxes;
                    else
                        AreaType = ChartAreaType.None;
                }
#pragma warning restore CS0618 // Type or member is obsolete
                if (e.OldItems == null && GetEnableSeriesSelection()
                    && SelectedSeriesIndex < seriesCollection.Count && SelectedSeriesIndex != -1)
                {
                    SelectedSeriesCollection.Add(seriesCollection[SelectedSeriesIndex] as ChartSeriesBase);
                }

                UpdateVisibleSeries(e.NewItems, e.NewStartingIndex);

                //WINUI-1448 Series color(interior) is not updated with index when dynamically inserting the series.
                if (Palette != ChartColorPalette.None && e.NewStartingIndex < VisibleSeries.Count - 1)
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
                if (canvas.Children.Contains((this.Tooltip as ChartTooltip)))
                    canvas.Children.Remove(this.Tooltip as ChartTooltip);
            }
            IsUpdateLegend = true;
            AddOrRemoveBitmap();
            this.ScheduleUpdate();
            SBSInfoCalculated = false;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private void UnRegisterSeries(ISupportAxes series)
        {
            if (series != null)
            {
                if (series is CartesianSeries)
                {
                    CartesianSeries cartesianSeries = series as CartesianSeries;
                    if (cartesianSeries.YAxis == null && InternalSecondaryAxis != null)
                        InternalSecondaryAxis.RegisteredSeries.Remove(series);
                    else
                    {
                        if (InternalSecondaryAxis == cartesianSeries.YAxis)
                            InternalSecondaryAxis = null;
                        cartesianSeries.ClearValue(CartesianSeries.YAxisProperty);
                    }

                    if (cartesianSeries.XAxis == null)
                        InternalPrimaryAxis.RegisteredSeries.Remove(series);
                    else
                    {
                        if (InternalPrimaryAxis == cartesianSeries.XAxis)
                            InternalPrimaryAxis = null;
                        cartesianSeries.ClearValue(CartesianSeries.XAxisProperty);
                    }
                }
                else if (series is FinancialTechnicalIndicator)
                {
                    //Todo:Move to Cartesian Chart
                    //FinancialTechnicalIndicator financialIndicator = series as FinancialTechnicalIndicator;
                    //if (financialIndicator.YAxis == null)
                    //    InternalSecondaryAxis.RegisteredSeries.Remove(series);
                    //else
                    //{
                    //    if (financialIndicator.YAxis.Equals(SecondaryAxis))
                    //        this.ClearValue(SecondaryAxisProperty);
                    //    financialIndicator.ClearValue(FinancialTechnicalIndicator.YAxisProperty);
                    //}

                    //if (financialIndicator.XAxis == null)
                    //    InternalPrimaryAxis.RegisteredSeries.Remove(series);
                    //else
                    //{
                    //    if (financialIndicator.XAxis == PrimaryAxis)
                    //        this.ClearValue(PrimaryAxisProperty);
                    //    financialIndicator.ClearValue(FinancialTechnicalIndicator.XAxisProperty);
                    //}
                }
            }
            if (InternalPrimaryAxis != null && InternalSecondaryAxis != null
                && InternalPrimaryAxis.RegisteredSeries.Count == 0 && InternalSecondaryAxis.RegisteredSeries.Count == 0
                && (series != null && (series as ChartSeries).IsActualTransposed))
            {
                InternalPrimaryAxis.Orientation = Orientation.Horizontal;
                InternalSecondaryAxis.Orientation = Orientation.Vertical;
            }
        }

        private void UpdateVisibleSeries(IList seriesColl, int seriesIndex)
        {
            foreach (ChartSeries series in seriesColl)
            {
                if (series == null) continue;
                series.UpdateLegendIconTemplate(false);
                series.Area = this;

                CheckSeriesTransposition(series);

                var supportAxisSeries = series as ISupportAxes;
                if (series.ActualXAxis != null)
                {
                    series.ActualXAxis.Area = this;

                    if (supportAxisSeries != null && !series.ActualXAxis.RegisteredSeries.Contains(supportAxisSeries))
                        series.ActualXAxis.RegisteredSeries.Add(supportAxisSeries);
                    if (!this.InternalAxes.Contains(series.ActualXAxis))
                    {
                        InternalAxes.Add(series.ActualXAxis);
                        DependentSeriesAxes.Add(series.ActualXAxis);
                    }
                }
                if (series.ActualYAxis != null)
                {
                    series.ActualYAxis.Area = this;

                    if (supportAxisSeries != null && !series.ActualYAxis.RegisteredSeries.Contains(supportAxisSeries))
                        series.ActualYAxis.RegisteredSeries.Add(supportAxisSeries);
                    if (!this.InternalAxes.Contains(series.ActualYAxis))
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

                    if (AreaType == ChartAreaType.PolarAxes && series is PolarRadarSeriesBase && !VisibleSeries.Contains(series))
                    {
                        VisibleSeries.Insert(visibleSeriesIndex, series);
                    }
                    else if (AreaType == ChartAreaType.None && series is AccumulationSeriesBase && !VisibleSeries.Contains(series))
                    {
                        VisibleSeries.Insert(visibleSeriesIndex, series);
                    }
                    else if (AreaType == ChartAreaType.CartesianAxes && (series is CartesianSeries || series is HistogramSeries) && !VisibleSeries.Contains(series))
                    {
                        VisibleSeries.Insert(visibleSeriesIndex, series);
                    }
                }
                if (!ActualSeries.Contains(series))
                    ActualSeries.Insert(seriesIndex, series);
            }
        }

        private void AddOrRemoveWatermark(Watermark newWatermark, Watermark oldWatermark)
        {
            if (this.ChartDockPanel.Children.Contains(oldWatermark))
                this.ChartDockPanel.Children.Remove(oldWatermark);
            if (newWatermark != null && !this.rootPanel.Children.Contains(newWatermark))
            {
                this.Watermark.SetValue(ChartDockPanel.DockProperty, ChartDock.Floating);
                this.ChartDockPanel.Children.Insert(0, newWatermark);//WRT-2656-Need to Change the Default ZIndex of Watermark.
            }
        }

        /// <summary>
        /// This method is to update bitmap series tooltip.
        /// </summary>
        private void UpdateBitmapToolTip()
        {
            for (int i = VisibleSeries.Count - 1; i >= 0; i--)
            {
                ChartSeries series = VisibleSeries[i] as ChartSeries;

                if (series.ShowTooltip && series.IsHitTestSeries())
                {
                    //Gets the current mouse position chart data point
                    ChartDataPointInfo datapoint = series.GetDataPoint(adorningCanvasPoint);

                    if (datapoint != null)
                    {
                        series.mousePos = adorningCanvasPoint;

                        if (this.Tooltip != null && this.Tooltip.PreviousSeries != null)
                        {
                            if (series.ActualTooltipPosition == TooltipPosition.Auto && !series.Equals(this.Tooltip.PreviousSeries))
                            {
                                series.RemoveTooltip();
                                series.Timer.Stop();
                            }
                        }

                        if (series.mousePos != series.previousMousePosition)
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
                if ((Legend as ChartLegend).DockPosition == ChartDock.Left || (Legend as ChartLegend).DockPosition == ChartDock.Top)
                {
                    if (axis.Orientation == Orientation.Horizontal)
                        return (ChartDockPanel.DesiredSize.Width - rootPanel.DesiredSize.Width);
                    return (ChartDockPanel.DesiredSize.Height - rootPanel.DesiredSize.Height);
                }
            }
            return 0d;
        }

        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize != AvailableSize)
                InvalidateMeasure();
        }

        private void LayoutAxis(Size availableSize)
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

            if (AdorningCanvas != null)
            {
                foreach (var axis in InternalAxes)
                {
                    var linearAxis = axis as NumericalAxis;
                    if (linearAxis != null && linearAxis.AxisScaleBreaks.Count > 0 && linearAxis.IsSecondaryAxis)
                    {
                        linearAxis.DrawScaleBreakLines();
                    }
                }
            }
        }

        #endregion

        #endregion

    }
}
