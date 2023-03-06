using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel;

namespace Syncfusion.UI.Xaml.Charts
{
    internal class LegendItem : DependencyObject, INotifyPropertyChanged, ILegendItem
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="Text"/> property.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(LegendItem),
                new PropertyMetadata(string.Empty, OnTextChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="IconWidth"/> property.
        /// </summary>
        public static readonly DependencyProperty IconWidthProperty =
            DependencyProperty.Register(nameof(IconWidth), typeof(double), typeof(LegendItem),
                new PropertyMetadata(12d, OnIconWidthPropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="IconHeight"/> property.
        /// </summary>
        public static readonly DependencyProperty IconHeightProperty =
            DependencyProperty.Register(nameof(IconHeight), typeof(double), typeof(LegendItem),
                new PropertyMetadata(12d, OnIconHeightPropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="IconVisibility"/> property.
        /// </summary>
        public static readonly DependencyProperty IconVisibilityProperty =
            DependencyProperty.Register(nameof(IconVisibility), typeof(Visibility), typeof(LegendItem),
                new PropertyMetadata(Visibility.Visible, OnIconVisibilityChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="CheckBoxVisibility"/> property.
        /// </summary>
        public static readonly DependencyProperty CheckBoxVisibilityProperty =
            DependencyProperty.Register(nameof(CheckBoxVisibility), typeof(Visibility), typeof(LegendItem),
                new PropertyMetadata(Visibility.Collapsed, OnCheckBoxVisibilityChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Opacity"/> property.
        /// </summary>
        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register(nameof(Opacity), typeof(double), typeof(LegendItem),
                new PropertyMetadata(1d, OnOpacityChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Visibility"/> property.
        /// </summary>
        public static readonly DependencyProperty VisibilityProperty =
            DependencyProperty.Register(nameof(Visibility), typeof(Visibility), typeof(LegendItem),
                new PropertyMetadata(Visibility.Visible, OnVisibilityChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ItemMargin"/> property.
        /// </summary>
        public static readonly DependencyProperty ItemMarginProperty =
            DependencyProperty.Register(nameof(ItemMargin), typeof(Thickness), typeof(LegendItem),
                new PropertyMetadata(new Thickness(0), OnItemMarginPropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="IsToggled"/> property.
        /// </summary>
        public static readonly DependencyProperty IsToggledProperty =
            DependencyProperty.Register(nameof(IsToggled), typeof(bool), typeof(LegendItem),
                new PropertyMetadata(true, OnIsToggledChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="IconTemplate"/> property.
        /// </summary>
        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register(nameof(IconTemplate), typeof(DataTemplate), typeof(LegendItem),
                new PropertyMetadata(null, OnLegendIconTemplateChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="IconBrush"/> property.
        /// </summary>
        public static readonly DependencyProperty IconBrushProperty =
            DependencyProperty.Register(nameof(IconBrush), typeof(Brush), typeof(LegendItem),
                new PropertyMetadata(new SolidColorBrush(Colors.Orange), OnIconBrushChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(LegendItem),
                new PropertyMetadata(null));

        /// <summary>
        /// The DependencyProperty for <see cref="StrokeThickness"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(LegendItem),
                new PropertyMetadata(0d));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendItem"/> class.
        /// </summary>
        public LegendItem()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the corresponding label for legend item text.
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Gets or sets icon color for legend item.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush IconBrush
        {
            get { return (Brush)GetValue(IconBrushProperty); }
            set { SetValue(IconBrushProperty, value); }
        }

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
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
        /// Gets or sets a value indicating whether the legend item is clicked or not.
        /// </summary>
        public bool IsToggled
        {
            get { return (bool)GetValue(IsToggledProperty); }
            set { SetValue(IsToggledProperty, value); }
        }

        /// <summary>
        /// Gets or sets value whether legend is visible or not. 
        /// </summary>
        public Visibility Visibility
        {
            get { return (Visibility)GetValue(VisibilityProperty); }
            set { SetValue(VisibilityProperty, value); }
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

        public int Index { get; set; }

        public object Item { get; set; }

        #endregion

        #region Event

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        #region Private Static Methods

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged(nameof(Text));
        }

        private static void OnIconBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged(nameof(IconBrush));
        }

        private static void OnIconVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged(nameof(IconVisibility));
        }

        private static void OnCheckBoxVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged(nameof(CheckBoxVisibility));
        }

        private static void OnIconWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged(nameof(IconWidth));
        }

        private static void OnIconHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged(nameof(IconHeight));
        }

        private static void OnIsToggledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LegendItem item = d as LegendItem;
            item.OnPropertyChanged(nameof(IsToggled));
            item.UpdateToggle(item.IsToggled);
        }

        private static void OnVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged(nameof(Visibility));
        }

        private static void OnOpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged(nameof(Opacity));
        }

        private static void OnItemMarginPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged(nameof(ItemMargin));
        }

        private static void OnLegendIconTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LegendItem).OnPropertyChanged(nameof(IconTemplate));
        }

        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateToggle(bool isToggled)
        {
            IsToggled = isToggled;
            Opacity = IsToggled ? 1 : 0.5d;
            ToggledSegment();
        }

        internal virtual void ToggledSegment()
        {

        }

        #endregion
    }
}
