namespace Syncfusion.UI.Xaml.Charts
{
    using System;
    using System.Collections.ObjectModel;
#if WinUI_Desktop
    using System.ComponentModel;
#endif
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media;
    using Microsoft.UI.Xaml.Input;
    using Windows.Foundation;
    using Windows.UI.Core;

    /// <summary>
    /// Represents legend for a <see cref="ChartBase"/>.
    /// </summary>
    /// <remarks>
    /// Chart legend will be added as chart's child. Each item in legend contain key information about the <see cref="ChartSeriesBase"/>. Legend has all abilities such as docking, enabling or
    /// disabling desired series in a <see cref="ChartBase"/>.
    ///</remarks>
    /// <example>
    /// <code language="XAML">
    ///     &lt;syncfusion:SfChart&gt;
    ///           &lt;syncfusion:SfChart.Legend&gt;
    ///                 &lt;syncfusion:ChartLegend/&gt;
    ///           &lt;/syncfusion:SfChart.Legend&gt;
    ///           &lt;syncfusion:Series/&gt;
    ///     &lt;/syncfusion:SfChart &gt;
    /// </code>
    /// <code language="C#">
    ///     ChartLegend chartLegend = new ChartLegend();
    ///     ChartArea.Legend = chartLegend;
    /// </code>
    /// </example>
    public class ChartLegend : ItemsControl
    {
        #region Dependency Property Registration
        
        /// <summary>
        /// The DependencyProperty for <see cref="DockPosition"/> property.
        /// </summary>
        public static readonly DependencyProperty DockPositionProperty =
            DependencyProperty.Register(
                nameof(DockPosition), 
                typeof(ChartDock),
                typeof(ChartLegend),
                new PropertyMetadata(
                    ChartDock.Top, 
                    new PropertyChangedCallback(OnDockPositionChanged)));

        /// <summary>
        /// Identifies the OffsetX dependency property.
        /// The DependencyProperty for <see cref="OffsetX"/> property.
        /// </summary>
        public static readonly DependencyProperty OffsetXProperty =
         DependencyProperty.Register(
             nameof(OffsetX),
             typeof(double),
             typeof(ChartLegend),
             new PropertyMetadata(
                 0d, 
                 new PropertyChangedCallback(OnOffsetValueChanged)));

        /// <summary>
        /// Identifies the OffsetY dependency property.
        /// The DependencyProperty for <see cref="OffsetY"/> property.
        /// </summary>
        public static readonly DependencyProperty OffsetYProperty =
        DependencyProperty.Register(
            nameof(OffsetY), 
            typeof(double), 
            typeof(ChartLegend),
            new PropertyMetadata(
                0d, 
                new PropertyChangedCallback(OnOffsetValueChanged)));

        /// <summary>
        /// The DependencyProperty for <see cref="Series"/> property.
        /// </summary>
        public static readonly DependencyProperty SeriesProperty =
DependencyProperty.Register(
    nameof(Series),
    typeof(ChartSeriesBase),
    typeof(ChartLegend),
    new PropertyMetadata(
        null,
        new PropertyChangedCallback(OnSeriesPropertyChanged)));

        /// <summary>
        /// The DependencyProperty for <see cref="Position"/> property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(
                nameof(Position), 
                typeof(LegendPosition), 
                typeof(ChartLegend),
                new PropertyMetadata(
                    LegendPosition.Outside,
                    OnLegendPositionChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Header"/> property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                nameof(Header),
                typeof(object), 
                typeof(ChartLegend), 
                null);

        /// <summary>
        /// The DependencyProperty for <see cref="HeaderTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(
                nameof(HeaderTemplate), 
                typeof(DataTemplate), 
                typeof(ChartLegend),
                null);
        
        /// <summary>
        /// The DependencyProperty for <see cref="Orientation"/> property.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(
                nameof(Orientation), 
                typeof(ChartOrientation), 
                typeof(ChartLegend),
                new PropertyMetadata(
                    ChartOrientation.Default, 
                    new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// Identifies the CornerRadius dependency property.
        /// The DependencyProperty for <see cref="CornerRadius"/> property.
        /// </summary>
#if !WinUI_UWP
        public static new DependencyProperty CornerRadiusProperty =
             DependencyProperty.Register(
                 nameof(CornerRadius), 
                 typeof(CornerRadius), 
                 typeof(ChartLegend),
                 new PropertyMetadata(new CornerRadius(0)));
#else
        public static new DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(CornerRadius),
                typeof(ChartLegend),
                new PropertyMetadata(CornerRadiusHelper.FromUniformRadius(0)));
#endif

        /// <summary>
        /// Identifies the CheckBoxVisibility dependency property.
        /// The DependencyProperty for <see cref="CheckBoxVisibility"/> property.
        /// </summary>
        public static DependencyProperty CheckBoxVisibilityProperty =
          DependencyProperty.Register(
              nameof(CheckBoxVisibility), 
              typeof(Visibility),
              typeof(ChartLegend),
              new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// Identifies the ToggleSeriesVisibility dependency property.
        /// The DependencyProperty for <see cref="ToggleSeriesVisibility"/> property.
        /// </summary>
        public static DependencyProperty ToggleSeriesVisibilityProperty =
          DependencyProperty.Register(
              nameof(ToggleSeriesVisibility),
              typeof(bool), 
              typeof(ChartLegend),
              new PropertyMetadata(false));

        /// <summary>
        /// Identifies the IconVisibility dependency property.
        /// The DependencyProperty for <see cref="IconVisibility"/> property.
        /// </summary>
        public static DependencyProperty IconVisibilityProperty =
          DependencyProperty.Register(
              nameof(IconVisibility),
              typeof(Visibility), 
              typeof(ChartLegend),
              new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// Identifies the IconWidth dependency property.
        /// The DependencyProperty for <see cref="IconWidth"/> property.
        /// </summary>
        public static DependencyProperty IconWidthProperty =
          DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(ChartLegend),
         new PropertyMetadata(8d, new PropertyChangedCallback(OnLegendIconSizeChanged)));

        /// <summary>
        /// Identifies the IconHeight dependency property.
        /// The DependencyProperty for <see cref="IconHeight"/> property.
        /// </summary>
        public static DependencyProperty IconHeightProperty =
          DependencyProperty.Register(
              nameof(IconHeight),
              typeof(double),
              typeof(ChartLegend),
        new PropertyMetadata(8d, new PropertyChangedCallback(OnLegendIconSizeChanged)));

        /// <summary>
        /// Identifies the ItemMargin dependency property.
        /// The DependencyProperty for <see cref="ItemMargin"/> property.
        /// </summary>
        public static DependencyProperty ItemMarginProperty =
          DependencyProperty.RegisterAttached(
              nameof(ItemMargin), 
              typeof(Thickness), 
              typeof(ChartLegend), 
              new PropertyMetadata(new Thickness().GetThickness(0,0,0,0)));


#endregion

        #region Fields

        #region Internal Fields

        internal bool isSegmentsUpdated;

#endregion

#region Private Fields

        private ChartDock internalDockPosition = ChartDock.Top;

        private bool isLayoutUpdated;

#endregion

#endregion

#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartLegend"/> class.
        /// </summary>
        public ChartLegend()
        {
            this.DefaultStyleKey = typeof(ChartLegend);
            this.Loaded += ChartLegend_Loaded;
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the orientation of chart legend.
        /// </summary>
        /// <value>
        /// <see cref="ChartOrientation"/>
        /// </value>
        public ChartOrientation Orientation
        {
            get { return (ChartOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the position of the ChartLegend.
        /// </summary>
        /// <value>
        /// <see cref="ChartDock"/>
        /// </value>
        public ChartDock DockPosition
        {
            get { return (ChartDock)GetValue(DockPositionProperty); }
            set { SetValue(DockPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the legend position in the chart.
        /// </summary>
        /// <value>
        /// <see cref="LegendPosition"/>
        /// </value>
        public LegendPosition Position
        {
            get { return (LegendPosition)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the margin for legend item.
        /// </summary>
        public Thickness ItemMargin
        {
            get
            {
                return (Thickness)this.GetValue(ChartLegend.ItemMarginProperty);
            }

            set
            {
                this.SetValue(ChartLegend.ItemMarginProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets the header for the legend.
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the legend header template.
        /// </summary>
        /// <value>
        /// <see cref="DataTemplate"/>
        /// </value>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the CornerRadius of legend's border.
        /// </summary>
        /// <value>
        /// <see cref="CornerRadius"/>
        /// </value>
        public new CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)GetValue(ChartLegend.CornerRadiusProperty);
            }
            set
            {
                SetValue(ChartLegend.CornerRadiusProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets a value that determines whether to show/hide CheckBox in legend item.
        ///</summary>
        ///<value>
        ///<see cref="Visibility"/>
        ///</value>
        public Visibility CheckBoxVisibility
        {
            get
            {
                return (Visibility)this.GetValue(CheckBoxVisibilityProperty);
            }

            set
            {
                this.SetValue(CheckBoxVisibilityProperty, value);
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether ToggleSeriesVisibility is enabled.
        ///</summary>
        public bool ToggleSeriesVisibility
        {
            get
            {
                return (bool)this.GetValue(ToggleSeriesVisibilityProperty);
            }

            set
            {
                this.SetValue(ToggleSeriesVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the visibility of the legend icon.
        /// </summary>
        /// <value>
        /// <see cref="Visibility"/>
        /// </value>
        public Visibility IconVisibility
        {
            get
            {
                return (Visibility)this.GetValue(IconVisibilityProperty);
            }

            set
            {
                this.SetValue(IconVisibilityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets width of the legend icon.
        /// </summary>
        public double IconWidth
        {
            get
            {
                return (double)this.GetValue(ChartLegend.IconWidthProperty);
            }

            set
            {
                this.SetValue(IconWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets height of the legend icon. 
        /// </summary>
        public double IconHeight
        {
            get
            {
                return (double)this.GetValue(ChartLegend.IconHeightProperty);
            }

            set
            {
                this.SetValue(ChartLegend.IconHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the value used to move the ChartLegend on x-coordinate.
        /// </summary>
		/// <remarks>
		/// This property is working for dock position as Floating only.
		/// </remarks>
        public double OffsetX
        {
            get { return (double)GetValue(OffsetXProperty); }
            set { SetValue(OffsetXProperty, value); }
        }

        /// <summary>
        /// Gets or sets the series value to get data point based legend items.
        /// </summary>
        public ChartSeriesBase Series
        {
            get { return (ChartSeriesBase)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value used to move the ChartLegend on y-coordinate.
        /// </summary>
		/// <remarks>
		/// This property is working for dock position as Floating only.
		/// </remarks>
        public double OffsetY
        {
            get { return (double)GetValue(OffsetYProperty); }
            set { SetValue(OffsetYProperty, value); }
        }

#endregion

#region InternalProperties

        internal ChartAxis XAxis
        {
            get;
            set;
        }

        internal ChartAxis YAxis
        {
            get;
            set;
        }

        internal ChartBase ChartArea
        {
            get;
            set;
        }

        internal int RowColumnIndex { get; set; }

        internal Rect ArrangeRect { get; set; }
        
        internal ChartDock InternalDockPosition
        {
            get { return internalDockPosition; }
            set { internalDockPosition = value; }
        }

#endregion

#endregion

#region Methods

#region Internal Methods
        
        internal void Dispose()
        {
            XAxis = null;
            YAxis = null;
            ChartArea = null;
        }

        internal void ComputeToggledSegment(ChartSeriesBase series, LegendItem legendItem)
        {
            var toggledSegment = GetSegment(series.Segments, legendItem.Item);
            var circularSeries = series as CircularSeries;
            int toggledIndex = circularSeries != null && !double.IsNaN(circularSeries.GroupTo) ? series.Segments.IndexOf(toggledSegment) : series.ActualData.IndexOf(legendItem.Item);
            if (toggledSegment != null && toggledSegment.IsSegmentVisible)
            {
                series.ToggledLegendIndex.Add(toggledIndex);
                toggledSegment.IsSegmentVisible = false;
                legendItem.Opacity = 0.5d;
            }
            else
            {
                series.ToggledLegendIndex.Remove(toggledIndex);
                toggledSegment.IsSegmentVisible = true;
                legendItem.Opacity = 1d;
            }
            ChartArea.ScheduleUpdate();
        }

        internal void ChangeOrientation()
        {
            ItemsPresenter itemsPresenter = ChartLayoutUtils.GetVisualChild<ItemsPresenter>(this);
            if (itemsPresenter != null)
            {
                if (VisualTreeHelper.GetChildrenCount(itemsPresenter) > 0)
                {
                    StackPanel itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 1) as StackPanel;

                    if (itemsPanel != null)
                    {
                        itemsPanel.Orientation = (Orientation)Enum.Parse(
                            typeof(Orientation)
                            , (this.Orientation == ChartOrientation.Default
                            ? ((this.DockPosition != ChartDock.Left && this.DockPosition != ChartDock.Right)
                            ? ChartOrientation.Horizontal : ChartOrientation.Vertical)
                            : this.Orientation).ToString(),
                            false);
                    }
                }
            }
        }

        internal LegendPosition GetPosition()
        {
            return this.Position;
        }

#endregion

#region Protected Override Methods

        /// <summary>
        /// Called when the pointer moved on the <see cref="LegendItem"/>.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element != null && element.DataContext is LegendItem && ToggleSeriesVisibility)
            {
                if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
                {
#if WinUI_Desktop
                    if (CoreWindow.GetForCurrentThread() != null && CoreWindow.GetForCurrentThread().PointerCursor.Type == CoreCursorType.Arrow)
                    {
                        CoreWindow.GetForCurrentThread().PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
                    }
#else
                    if (Window.Current.CoreWindow.PointerCursor.Type == CoreCursorType.Arrow)
                    {
                        Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);
                    }
#endif     
                }
            }
        }

        /// <summary>
        /// Called when the pointer pressed on the <see cref="LegendItem"/>.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            var element = e.OriginalSource as FrameworkElement;
            if (element != null)
            {
                var legendItem = element.DataContext as LegendItem;

                if (legendItem != null)
                {
                    if (ToggleSeriesVisibility && !ChartArea.HasDataPointBasedLegend())
                    {
                        legendItem.IsSeriesVisible = !legendItem.IsSeriesVisible;
                    }

                    legendItem.OnPropertyChanged("HookLegendClick");
                }
            }
        }
        
        /// <summary>
        /// Called when the pointer leaved from the chart legend.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnPointerExited(PointerRoutedEventArgs e)
        {
            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
            {
#if WinUI_Desktop
                if (CoreWindow.GetForCurrentThread() != null && CoreWindow.GetForCurrentThread().PointerCursor.Type != CoreCursorType.Arrow)
                {
                    CoreWindow.GetForCurrentThread().PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
                }
#else
                if (Window.Current.CoreWindow.PointerCursor.Type != CoreCursorType.Arrow)
                {
                    Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 0);
                }
#endif 
            }
        }

#endregion

#region Private Static Methods

        private static void OnLegendPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            (d as ChartLegend).OnLegendPositionChanged();
        }

        private static void OnDockPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartLegend).OnDockPositionChanged((ChartDock)e.OldValue, (ChartDock)e.NewValue);
        }

        private static void OnSeriesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartLegend legend = d as ChartLegend;
            if (legend != null && legend.ChartArea != null)
            {
                legend.ChartArea.IsUpdateLegend = true;
                legend.ChartArea.UpdateLegend(legend, true);
            }
        }

        private static void OnOffsetValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartLegend legend = d as ChartLegend;
            legend.OnOffsetValueChanged(e);
        }

        private static ChartSegment GetSegment(ObservableCollection<ChartSegment> segments, object item)
        {
            ChartSegment toggledSegment = null;
            foreach (var segment in segments)
            {
                if (segment.Item == item)
                {
                    toggledSegment = segment;
                    break;
                }

                if (segment.Series is CircularSeries && !double.IsNaN(((CircularSeries)segment.Series).GroupTo))
                {
                    if (((CircularSeries)segment.Series).GroupedData == segment.Item)
                        toggledSegment = segment;
                }
            }
            
            return toggledSegment;
        }

#if WinUI

        private static void OnLegendIconSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartLegend legend = d as ChartLegend;
            if (legend != null && legend.ChartArea != null)
            {
                legend.ChartArea.UpdateLegend(legend, true);
            }
        }
#endif

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChartLegend sender = d as ChartLegend;
            sender.ChangeOrientation();
        }

#endregion

#region Private Methods

        private void OnLegendPositionChanged()
        {
            if (this.GetPosition() == Charts.LegendPosition.Inside)
                InternalDockPosition = ChartDock.Floating;
            else
                InternalDockPosition = DockPosition;

            ChartDockPanel.SetDock(this, InternalDockPosition);

            if (Parent != null)
            {
                ChartArea.LayoutLegends();
                ChartArea.UpdateLegendArrangeRect();
                (Parent as ChartDockPanel).InvalidateMeasure();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        private void OnDockPositionChanged(ChartDock oldValue, ChartDock newValue)
        {
            if (this.GetPosition() == LegendPosition.Outside)
            {
                InternalDockPosition = DockPosition;
                if (ChartArea != null)
                {
                    ChartArea.LayoutLegends();
                    ChartArea.UpdateLegendArrangeRect();
                }
            }

            ChangeOrientation();

            if (this.GetPosition() == LegendPosition.Inside)
            {
                var dockPanel = Parent as ChartDockPanel;
                if (dockPanel == null) return;
                Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                ChartArea.UpdateLegendArrangeRect();
                dockPanel.InvalidateMeasure();
            }
            else if (ChartDockPanel.GetDock(this) != InternalDockPosition)
                ChartDockPanel.SetDock(this, InternalDockPosition);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        private void OnOffsetValueChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Parent is ChartDockPanel)
            {
                (Parent as ChartDockPanel).InvalidateArrange();
            }
        }

        private void ChartLegend_Loaded(object sender, RoutedEventArgs e)
        {

            if (this.Visibility == Visibility.Collapsed && Orientation == ChartOrientation.Vertical)
            {
                this.RegisterPropertyChangedCallback(VisibilityProperty, OnVisibilityPropertyChanged);
                this.LayoutUpdated += ChartLegend_LayoutUpdated;
            }

            ChangeOrientation();

        }
        
        private void OnVisibilityPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (dp == VisibilityProperty)
            {
                isLayoutUpdated = true;
            }
        }

        private void ChartLegend_LayoutUpdated(object sender, object e)
        {
            if (isLayoutUpdated)
            {
                ChangeOrientation();
                isLayoutUpdated = false;
            }
        }

#endregion

#endregion
    }

    /// <summary>
    /// Represents the class implementation for LegendItem.
    /// </summary>
    public class LegendItem : DependencyObject , INotifyPropertyChanged
    {
#region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="Label"/> property.
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(
                "Label",
                typeof(string),
                typeof(LegendItem),
                new PropertyMetadata(
                    string.Empty,
                    OnLabelChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="IconTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register(
                "IconTemplate", 
                typeof(DataTemplate),
                typeof(LegendItem),
                new PropertyMetadata(
                    null,
                    OnLegendIconTemplateChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                "Stroke", 
                typeof(Brush),
                typeof(LegendItem), 
                new PropertyMetadata(null, OnStrokePropertyChanged));
        
        /// <summary>
        /// The DependencyProperty for <see cref="StrokeThickness"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness", 
                typeof(double), 
                typeof(LegendItem), 
                new PropertyMetadata(0d, OnStrokeThicknessProperty));
        
        /// <summary>
        /// The DependencyProperty for <see cref="Interior"/> property.
        /// </summary>
        public static readonly DependencyProperty InteriorProperty =
            DependencyProperty.Register(
                "Interior", 
                typeof(Brush),
                typeof(LegendItem),
                new PropertyMetadata(
                    null, 
                    OnInteriorChanged));
        
        /// <summary>
        /// The DependencyProperty for <see cref="IconVisibility"/> property.
        /// </summary>
        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register(
                "IconVisibility",
                typeof(Visibility), 
                typeof(LegendItem),
                new PropertyMetadata(
                    Visibility.Collapsed,
                    OnIconVisibilityChanged));
        
        /// <summary>
        /// The DependencyProperty for <see cref="CheckBoxVisibility"/> property.
        /// </summary>
        public static readonly DependencyProperty CheckBoxVisibilityProperty =
            DependencyProperty.Register(
                "CheckBoxVisibility", 
                typeof(Visibility), 
                typeof(LegendItem),
                new PropertyMetadata(
                    Visibility.Collapsed, 
                    OnCheckBoxVisibilityChanged));
        
        /// <summary>
        /// The DependencyProperty for <see cref="IconWidth"/> property.
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register(
                "IconWidth", 
                typeof(double), 
                typeof(LegendItem),
                new PropertyMetadata(
                    double.NaN, 
                    OnIconWidthPropertyChanged));
        
        /// <summary>
        /// The DependencyProperty for <see cref="IconHeight"/> property.
        /// </summary>
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register(
                "IconHeight", 
                typeof(double), 
                typeof(LegendItem),
                new PropertyMetadata(
                    double.NaN, 
                    OnIconHeightPropertyChanged));
        
        /// <summary>
        /// The DependencyProperty for <see cref="ItemMargin"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register(
                "ItemMargin", 
                typeof(Thickness),
                typeof(LegendItem),
                new PropertyMetadata(
                    new Thickness().GetThickness(0,0,0,0), 
                    OnItemMarginPropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="IsSeriesVisible"/> property.
        /// </summary>
        public static readonly DependencyProperty IsSeriesVisibleProperty =
            DependencyProperty.Register(
                "IsSeriesVisible", 
                typeof(bool),
                typeof(LegendItem),
                new PropertyMetadata(
                    true, 
                    OnSeriesVisible));
        
        /// <summary>
        /// The DependencyProperty for <see cref="VisibilityOnLegend"/> property.
        /// </summary>
        public static readonly DependencyProperty VisibilityOnLegendProperty =
            DependencyProperty.Register(
                "VisibilityOnLegend", 
                typeof(Visibility), 
                typeof(LegendItem),
                new PropertyMetadata(
                    Visibility.Visible, 
                    OnVisibilityOnLegend));
        
        /// <summary>
        /// The DependencyProperty for <see cref="Opacity"/> property.
        /// </summary>
        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register(
                "Opacity", 
                typeof(double),
                typeof(LegendItem),
                new PropertyMetadata(
                    1d, 
                    OnOpacityChanged));

#endregion

#region Fields

#region Private Fields

        private ChartSegment segment;
        
        private object item;
        
        private ChartSeriesBase series;

        private TrendlineBase trendline;

        private ChartLegend legend;

#endregion

#endregion
        
#region Events

        /// <summary>
        /// Occurs when a property value changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the value for the legend item label text.
        /// </summary>
        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the data template for the legend icon.
        /// </summary>
        /// <value>
        /// <see cref="DataTemplate"/>
        /// </value>
        public DataTemplate IconTemplate
        {
            get { return (DataTemplate)GetValue(IconTemplateProperty); }
            set { SetValue(IconTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets stroke for the legend item.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets stroke thickness for legend item.
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets interior for legend item.
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
        /// Gets or sets icon visibility of the legend item.
        /// </summary>
        public Visibility IconVisibility
        {
            get { return (Visibility)GetValue(IconVisibilityProperty); }
            set { SetValue(IconVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets checkbox visibility of the legend item.
        /// </summary>
        public Visibility CheckBoxVisibility
        {
            get { return (Visibility)GetValue(CheckBoxVisibilityProperty); }
            set { SetValue(CheckBoxVisibilityProperty, value); }
        }

        /// <summary>
        /// Gets or sets width of the legend item icon.
        /// </summary>
        public double IconWidth
        {
            get { return (double)GetValue(IconWidthProperty); }
            set { SetValue(IconWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets height of the legend item icon.
        /// </summary>
        public double IconHeight
        {
            get { return (double)GetValue(IconHeightProperty); }
            set { SetValue(IconHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets margin of the legend item.
        /// </summary>
        public Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is series visible or not.
        /// </summary>
        public bool IsSeriesVisible
        {
            get { return (bool)GetValue(IsSeriesVisibleProperty); }
            set { SetValue(IsSeriesVisibleProperty, value); }
        }

        /// <summary>
        /// Gets or sets value whether legend is visible or not. 
        /// </summary>
        public Visibility VisibilityOnLegend
        {
            get { return (Visibility)GetValue(VisibilityOnLegendProperty); }
            set { SetValue(VisibilityOnLegendProperty, value); }
        }

        /// <summary>
        /// Gets or sets Opacity of the legend item.
        /// </summary>
        public double Opacity
        {
            get { return (double)GetValue(OpacityProperty); }
            set { SetValue(OpacityProperty, value); }
        }

        /// <summary>
        /// Gets or sets Segment of the legend item.
        /// </summary>
        public ChartSegment Segment
        {
            get
            {
                return segment;
            }
            set
            {
                segment = value;
                if (segment != null)
                {
                    Binding binding = new Binding();
                    binding.Source = segment;
                    binding.Path = new PropertyPath("Interior");
                    binding.Converter = new InteriorConverter(segment.Series);
                    binding.ConverterParameter = segment.Series.Segments.IndexOf(segment);
                    BindingOperations.SetBinding(this, LegendItem.InteriorProperty, binding);
                }
            }
        }

        /// <summary>
        /// Gets or sets Item of the legend item.
        /// </summary>
        public object Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
            }
        }

        /// <summary>
        /// Gets or sets the Trendline of the legend item.
        /// </summary>
        internal TrendlineBase Trendline
        {
            get
            {
                return trendline;
            }
            set
            {
                trendline = value;

                if (trendline != null)
                {
                    Binding binding = new Binding();
                    binding.Source = trendline;
                    binding.Path = new PropertyPath("Stroke");
                    BindingOperations.SetBinding(this, LegendItem.InteriorProperty, binding);

                    binding = new Binding();
                    binding.Source = trendline;
                    binding.Path = new PropertyPath("Label");
                    BindingOperations.SetBinding(this, LegendItem.LabelProperty, binding);

                    binding = new Binding();
                    binding.Source = trendline;
                    binding.Path = new PropertyPath("LegendIconTemplate");
                    BindingOperations.SetBinding(this, LegendItem.IconTemplateProperty, binding);

                    binding = new Binding();
                    binding.Source = trendline;
                    binding.Path = new PropertyPath("VisibilityOnLegend");
                    BindingOperations.SetBinding(this, LegendItem.VisibilityOnLegendProperty, binding);

                    binding = new Binding();
                    binding.Source = trendline;
                    binding.Mode = BindingMode.TwoWay;
                    binding.Path = new PropertyPath("IsTrendlineVisible");
                    BindingOperations.SetBinding(this, LegendItem.IsSeriesVisibleProperty, binding);

                    binding = new Binding();
                    binding.Source = trendline;
                    binding.Path = new PropertyPath("Stroke");
                    BindingOperations.SetBinding(this, LegendItem.StrokeProperty, binding);

                    binding = new Binding();
                    binding.Source = trendline;
                    binding.Path = new PropertyPath("StrokeThickness");
                    BindingOperations.SetBinding(this, LegendItem.StrokeThicknessProperty, binding);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Series of the legend item.
        /// </summary>
        public ChartSeriesBase Series
        {
            get
            {
                return series;
            }
            set
            {
                series = value;

                if (series != null)
                {
                    Binding binding = new Binding();
                    binding.Source = series;
                    binding.Path = new PropertyPath("Interior");
                    binding.Converter = new InteriorConverter(series);
                    binding.ConverterParameter = Index;
                    BindingOperations.SetBinding(this, LegendItem.InteriorProperty, binding);

                    binding = new Binding();
                    binding.Source = series;
                    binding.Path = new PropertyPath("Label");
                    BindingOperations.SetBinding(this, LegendItem.LabelProperty, binding);

                    binding = new Binding();
                    binding.Source = series;
                    binding.Path = new PropertyPath("LegendIconTemplate");
                    BindingOperations.SetBinding(this, LegendItem.IconTemplateProperty, binding);

                    binding = new Binding();
                    binding.Source = series;
                    binding.Path = new PropertyPath("VisibilityOnLegend");
                    BindingOperations.SetBinding(this, LegendItem.VisibilityOnLegendProperty, binding);

                    if (!(Series.IsSingleAccumulationSeries))
                    {
                        binding = new Binding();
                        binding.Source = series;
                        binding.Mode = BindingMode.TwoWay;
                        binding.Path = new PropertyPath("IsSeriesVisible");
                        BindingOperations.SetBinding(this, LegendItem.IsSeriesVisibleProperty, binding);
                    }

                    if (series is ChartSeries)
                    {
                        binding = new Binding();
                        binding.Source = series;
                        binding.Path = new PropertyPath("Stroke");
                        BindingOperations.SetBinding(this, LegendItem.StrokeProperty, binding);
                        binding = new Binding();
                        binding.Source = series;
                        binding.Path = new PropertyPath("StrokeThickness");
                        BindingOperations.SetBinding(this, LegendItem.StrokeThicknessProperty, binding);
                    }
                }
            }
        }

        #endregion

#region Internal Properties

        internal int Index { get; set; }

        internal ChartLegend Legend
        {
            get
            {
                return legend;
            }
            set
            {
                legend = value;

                if (legend != null)
                {
                    if (this.Segment != null && this.Segment.Series is AccumulationSeriesBase)
                    {
                        Binding binding = new Binding();
                        binding.Source = legend;
                        binding.Path = new PropertyPath("IconVisibility");
                        BindingOperations.SetBinding(this, LegendItem.IconVisibilityProperty, binding);

                        binding = new Binding();
                        binding.Source = legend;
                        binding.Path = new PropertyPath("CheckBoxVisibility");
                        BindingOperations.SetBinding(this, LegendItem.CheckBoxVisibilityProperty, binding);
                        this.CheckBoxVisibility = Visibility.Collapsed;
                    }
                    else
                    {
                        Binding binding = new Binding();
                        binding.Source = legend;
                        binding.Path = new PropertyPath("IconVisibility");
                        BindingOperations.SetBinding(this, LegendItem.IconVisibilityProperty, binding);

                        binding = new Binding();
                        binding.Source = legend;
                        binding.Path = new PropertyPath("CheckBoxVisibility");
                        BindingOperations.SetBinding(this, LegendItem.CheckBoxVisibilityProperty, binding);

                        binding = new Binding();
                        binding.Source = legend;
                        binding.Path = new PropertyPath("IconWidth");
                        BindingOperations.SetBinding(this, LegendItem.IconWidthProperty, binding);

                        binding = new Binding();
                        binding.Source = legend;
                        binding.Path = new PropertyPath("IconHeight");
                        BindingOperations.SetBinding(this, LegendItem.IconHeightProperty, binding);

                        binding = new Binding();
                        binding.Source = legend;
                        binding.Path = new PropertyPath("ItemMargin");
                        BindingOperations.SetBinding(this, LegendItem.ItemMarginProperty, binding);
                    }
                }
            }
        }

#endregion

#endregion

#region Methods

#region Internal Methods

        internal void Dispose()
        {
            if (PropertyChanged != null)
            {
                var invocationList = PropertyChanged.GetInvocationList();

                foreach (var handler in invocationList)
                {
                    PropertyChanged -= handler as PropertyChangedEventHandler;
                }

                PropertyChanged = null;
            }

            segment = null;
            item = null;
            series = null;
            trendline = null;
            legend = null;
        }

        internal void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

#endregion

#region Private Static Methods

        private static void OnStrokePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("Stroke");
        }

        private static void OnStrokeThicknessProperty(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("StrokeThickness");
        }

        private static void OnLabelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("Label");
        }

        private static void OnLegendIconTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("IconTemplate");
        }
        
        private static void OnInteriorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("Interior");
        }
        
        private static void OnIconVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("IconVisibility");
        }
        
        private static void OnCheckBoxVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("CheckBoxVisibility");
        }
        
        private static void OnIconWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("IconWidth");
        }
        
        private static void OnIconHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("IconHeight");
        }
        
        private static void OnItemMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("ItemMargin");
        }
        
        private static void OnSeriesVisible(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LegendItem legendItem = d as LegendItem;
            legendItem.OnPropertyChanged("IsSeriesVisible");
            var series = legendItem.legend.ChartArea.ActualSeries;
         
            if (series[0].IsSingleAccumulationSeries && !legendItem.legend.isSegmentsUpdated)
            {
                legendItem.legend.ComputeToggledSegment(series[0], legendItem);
            }
            else
            {
                if (legendItem.IsSeriesVisible)
                    legendItem.Opacity = 1d;
                else
                    legendItem.Opacity = 0.5d;
            }
        }

        private static void OnVisibilityOnLegend(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("VisibilityOnLegend");
        }
        
        private static void OnOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged("Opacity");
        }
        
#endregion

#endregion
    }
}

     
