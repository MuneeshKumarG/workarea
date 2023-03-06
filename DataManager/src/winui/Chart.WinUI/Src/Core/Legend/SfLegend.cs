using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    internal class SfLegend : ItemsControl
    {
        #region Dependency properties

        /// <summary>
        /// The DependencyProperty for <see cref="ToggleVisibility"/> property.
        /// </summary>
        public static readonly DependencyProperty ToggleVisibilityProperty =
            DependencyProperty.Register(nameof(ToggleVisibility), typeof(bool), typeof(SfLegend), new PropertyMetadata(false));

        /// <summary>
        /// The DependencyProperty for <see cref="IsVisible"/> property.
        /// </summary>
        public static readonly DependencyProperty IsVisibleProperty =
            DependencyProperty.Register(nameof(IsVisible), typeof(bool), typeof(SfLegend), new PropertyMetadata(true, OnIsVisiblePropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Placement"/> property.
        /// </summary>
        public static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register(nameof(Placement), typeof(LegendPlacement), typeof(SfLegend), new PropertyMetadata(LegendPlacement.Top, OnPlacementPropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Header"/> property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(SfLegend), null);

        /// <summary>
        /// The DependencyProperty for <see cref="HeaderTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(SfLegend), null);

        /// <summary>
        /// The DependencyProperty for <see cref="LegendItemTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty LegendItemTemplateProperty =
            DependencyProperty.Register(nameof(LegendItemTemplate), typeof(DataTemplate), typeof(SfLegend), new PropertyMetadata(null, OnItemTemplatePropertyChanged));

        /// <summary>
        /// Identifies the HorizontalHeaderAlignment dependency property.
        /// The DependencyProperty for <see cref="HorizontalHeaderAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty HorizontalHeaderAlignmentProperty =
          DependencyProperty.RegisterAttached(nameof(HorizontalHeaderAlignment), typeof(HorizontalAlignment), typeof(SfLegend), new PropertyMetadata(HorizontalAlignment.Center));

        #endregion

        #region Fields

        internal LegendPanel LegendPanel { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfLegend"/> class.
        /// </summary>
        public SfLegend()
        {
            this.DefaultStyleKey = typeof(SfLegend);
            HorizontalContentAlignment = HorizontalAlignment.Center;
            this.Loaded += SfLegend_Loaded;
        }

        #endregion

        #region Properties

        public bool ToggleVisibility
        {
            get { return (bool)GetValue(ToggleVisibilityProperty); }
            set { SetValue(ToggleVisibilityProperty, value); }
        }

        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        public LegendPlacement Placement
        {
            get { return (LegendPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataTemplate LegendItemTemplate
        {
            get { return (DataTemplate)GetValue(LegendItemTemplateProperty); }
            set { SetValue(LegendItemTemplateProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public HorizontalAlignment HorizontalHeaderAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalHeaderAlignmentProperty); }
            set { SetValue(HorizontalHeaderAlignmentProperty, value); }
        }

        #endregion

        #region Methods

        private void SfLegend_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeOrientation();
        }

        /// <summary>
        /// Called when the pointer pressed on the <see cref="LegendItem"/>.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if (!ToggleVisibility)
                return;

            var element = e.OriginalSource as FrameworkElement;
            if (element != null)
            {
                var legendItem = element.DataContext as LegendItem;
                if (legendItem != null)
                    legendItem.IsToggled = !legendItem.IsToggled;
            }
        }

        /// <summary>
        /// Method used to get the legend size.
        /// </summary>
        /// <param name="legend"></param>
        /// <param name="availableSize"></param>
        /// <param name="maxSizePercentage"></param>
        /// <returns></returns>
        internal Size GetLegendMaxSize(Size availableSize, double maxSizePercentage)
        {
            Size size = DesiredSize;
            Size desiredSize;
            switch (Placement)
            {
                case LegendPlacement.Top:
                case LegendPlacement.Bottom:

                    double quarterheight = availableSize.Height * maxSizePercentage;
                    double desiredHeight = quarterheight < size.Height ? quarterheight : size.Height;
                    desiredSize = new Size(availableSize.Width, desiredHeight);

                    if (desiredHeight < size.Height)
                        Measure(desiredSize);

                    return desiredSize;

                case LegendPlacement.Left:
                case LegendPlacement.Right:

                    double quarterwidth = availableSize.Width * maxSizePercentage;
                    double desiredWidth = quarterwidth < size.Width ? quarterwidth : size.Width;
                    desiredSize = new Size(desiredWidth, availableSize.Height);

                    if (desiredWidth < size.Width)
                        Measure(desiredSize);

                    return desiredSize;
            }

            return Size.Empty;
        }

        private static void OnPlacementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfLegend sfLegend = d as SfLegend;
            sfLegend.ChangeOrientation();
            sfLegend.LegendPanel.InvalidateMeasure();
            sfLegend.LegendPanel.InvalidateArrange();

        }

        private static void OnIsVisiblePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfLegend sfLegend = d as SfLegend;
            sfLegend.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void OnItemTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfLegend sfLegend = d as SfLegend;

            if (e.NewValue is DataTemplate dataTemplate)
                sfLegend.ItemTemplate = dataTemplate;
        }

        internal void ChangeOrientation()
        {
            ItemsPresenter itemsPresenter = ChartLayoutUtils.GetVisualChild<ItemsPresenter>(this);
            if (itemsPresenter != null)
            {
                if (VisualTreeHelper.GetChildrenCount(itemsPresenter) > 0)
                {
                    StackPanel itemsPanel = VisualTreeHelper.GetChild(itemsPresenter, 1) as StackPanel;

                    if (itemsPanel != null && string.Equals(itemsPanel.Name, "LegendItemsPanel"))
                    {
                        if (this.Placement == LegendPlacement.Left || this.Placement == LegendPlacement.Right)
                            itemsPanel.Orientation = Orientation.Vertical;
                        else
                            itemsPanel.Orientation = Orientation.Horizontal;
                    }
                }
            }
        }

        internal void Dispose()
        {
            LegendPanel = null;
            ItemsSource = null;
            this.Loaded -= SfLegend_Loaded;
        }

        #endregion
    }
} 
