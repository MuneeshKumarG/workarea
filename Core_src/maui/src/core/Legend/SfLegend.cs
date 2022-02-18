using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal class SfLegend : ContentView, ILegend
    {
        #region Fields

        private const double maxSize = 8388607.5;

        #endregion

        #region Bindable Properties

        #region Public Bindable Properties

        /// <summary>
        /// Gets or sets the items source for the legend. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty ItemsSourceProperty =
           BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(SfLegend), null, BindingMode.Default, null, OnItemsSourceChanged);

        /// <summary>
        /// The DependencyProperty for <see cref="ToggleVisibility"/> property.
        /// </summary>
        public static readonly BindableProperty ToggleVisibilityProperty = BindableProperty.Create(nameof(ToggleVisibility), typeof(bool), typeof(SfLegend), false, BindingMode.Default, null, OnToggleVisibilityChanged, null, null);

        #endregion

        #region Internal Bindable Properties

        /// <summary>
        /// Gets or sets a data template for all series legend item. This is a bindable property.
        /// </summary>
        internal static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(SfLegend), null, BindingMode.Default, null, OnItemTemplateChanged, null, null);

        /// <summary>
        /// Gets or sets any Layout view for the legend. This is a bindable property.
        /// </summary>
        internal static readonly BindableProperty LegendLayoutProperty = BindableProperty.Create(nameof(LegendLayout), typeof(Layout), typeof(SfLegend), null, BindingMode.Default, null, OnLegendLayoutPropertyChanged);

        /// <summary>
        ///  Gets or sets placement of the legend. This is a bindable property.
        /// </summary>
        internal static readonly BindableProperty PlacementProperty = BindableProperty.Create(nameof(Placement), typeof(LegendPlacement), typeof(SfLegend), LegendPlacement.Top, BindingMode.Default, null, OnPlacementChanged, null, null);


        /// <summary>
        /// Gets or sets the orientation of the legend items. This is a bindable property.
        /// </summary>
        internal static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(LegendOrientation), typeof(SfLegend), LegendOrientation.Default, BindingMode.Default, null, null, null, null);

        
        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        ///  Gets or sets the ItemsSource for the legend.
        /// </summary>
        /// <remarks>The default will be of <see cref="LegendItem"/> type.</remarks>
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to bind the series visibility with its corresponding legend item in the legend. This is bindable property.
        /// </summary>
        public bool ToggleVisibility
        {
            get { return (bool)GetValue(ToggleVisibilityProperty); }
            set { SetValue(ToggleVisibilityProperty, value); }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the data template for legend item.
        /// </summary>
        /// <value>This property takes the DataTemplate value.</value>
        internal DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets any Layout for the legend.
        /// </summary>
        /// <remarks>The default layout is <see cref="StackLayout"/>.
        /// </remarks>
        internal Layout LegendLayout
        {
            get { return (Layout)GetValue(LegendLayoutProperty); }
            set { this.SetValue(LegendLayoutProperty, value); }
        }

        /// <summary>
        /// Gets or sets placement of the legend. This is a bindable property.
        /// </summary>
        internal LegendPlacement Placement
        {
            get { return (LegendPlacement)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        /// <summary>
        /// Gets or sets the orientation of the legend items. This is a bindable property.
        /// </summary>
        /// <value>This property takes <see cref="LegendOrientation"/> as its value.</value>
        internal LegendOrientation Orientation
        {
            get { return (LegendOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        internal ScrollView? LegendScrollView { get; set; }

        internal ContentView? ScrollViewContent { get; set; }

        LegendPlacement ILegend.Placement { get => Placement; set {} }

        LegendOrientation ILegend.Orientation { get => Orientation; set {} }

        bool ILegend.IsVisible { get; set; }

        internal event EventHandler<LegendItemClickedEventArgs>? ItemClicked;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfLegend"/> class.
        /// </summary>
        public SfLegend()
        {
           Initialize();
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Method used to get the legend rect size.
        /// </summary>
        /// <param name="legend"></param>
        /// <param name="availableSize"></param>
        /// <param name="maxSizePercentage"></param>
        /// <returns></returns>
        public static Rectangle GetLegendRectangle(SfLegend legend, Rectangle availableSize, double maxSizePercentage)
        {
            if (legend != null)
            {
                Size size = Size.Zero;
                double maxSize = 8388607.5;
                double position = 0;
                double maxWidth = 0;
                switch (legend.Placement)
                {
                    case LegendPlacement.Top:

                        size = legend.Measure(availableSize.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
                        maxWidth = availableSize.Height * maxSizePercentage < size.Height ? availableSize.Height * maxSizePercentage : size.Height;
                        position = (availableSize.Height != maxSize) ? availableSize.Height - maxWidth : 0;
                        return new Rectangle(availableSize.X, availableSize.Y, availableSize.Width, maxWidth);

                    case LegendPlacement.Bottom:

                        size = legend.Measure(availableSize.Width, double.PositiveInfinity, MeasureFlags.IncludeMargins);
                        maxWidth = availableSize.Height * maxSizePercentage < size.Height ? availableSize.Height * maxSizePercentage : size.Height;
                        position = (availableSize.Height != maxSize) ? availableSize.Height - maxWidth : 0;
                        return new Rectangle(availableSize.X, availableSize.Y + position, availableSize.Width, maxWidth);

                    case LegendPlacement.Left:

                        size = legend.Measure(double.PositiveInfinity, availableSize.Height, MeasureFlags.IncludeMargins);
                        maxWidth = availableSize.Width * maxSizePercentage < size.Width ? availableSize.Width * maxSizePercentage : size.Width;
                        return new Rectangle(availableSize.X, availableSize.Y, maxWidth, availableSize.Height);

                    case LegendPlacement.Right:

                        size = legend.Measure(double.PositiveInfinity, availableSize.Height, MeasureFlags.IncludeMargins);
                        maxWidth = availableSize.Width * maxSizePercentage < size.Width ? availableSize.Width * maxSizePercentage : size.Width;
                        position = (availableSize.Width != maxSize) ? availableSize.Width - maxWidth : 0;
                        return new Rectangle(availableSize.X + position, availableSize.Y, maxWidth, availableSize.Height);
                }
            }

            return Rectangle.Zero;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Creates the <see cref="SfShapeView"/> type for the default legend icon.
        /// </summary>
        /// <returns>Returns the ShapeView.</returns>
        protected virtual SfShapeView CreateShapeView()
        {
            return new SfShapeView();
        }

        /// <summary>
        /// Creates the <see cref="Label"/> type for the default legend text.
        /// </summary>
        /// <returns>Returns the Label.</returns>
        /// <remarks>Method must return label type and should not return any null value.</remarks>
        protected virtual Label CreateLabelView()
        {
            return new Label();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            //Todo: Need to remove once layout issue is resolved without measure and arrange method
            var baseSize = base.MeasureOverride(widthConstraint, heightConstraint);
            if (baseSize.Width != maxSize && baseSize.Height != maxSize)
            {

            }

            return baseSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Rectangle bounds)
        {
            //Todo: Need to remove once layout issue is resolved without measure and arrange method
            var baseSize = base.ArrangeOverride(bounds);
            if (baseSize.Width != maxSize && baseSize.Height != maxSize)
            {

            }

            return baseSize;
        }

        #endregion

        #region Private Call backs

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var legend = bindable as SfLegend;
            if (legend != null)
            {
                legend.OnItemsSourceChanged(oldValue, newValue);
            }
        }

        private static void OnLegendLayoutPropertyChanged(BindableObject bindableObject, object oldValue, object newValue)
        {
            var legend = bindableObject as SfLegend;
            if (legend != null)
            {
                legend.LegendLayoutChanged(oldValue, newValue);
            }
        }

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var legend = bindable as SfLegend;
            if (legend != null)
            {
                legend.OnItemTemplateChanged(oldValue, newValue);
            }
        }

        private static void OnPlacementChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var legend = bindable as SfLegend;
            if (legend != null)
            {
                legend.Initialize();
            }
        }

        private static void OnToggleVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        #endregion

        #region Private Methods

        internal void Initialize()
        {
            //Initialize default layout as StackLayout. Todo:// Need to delete the these code and while change the layout dynamicaly
            //if (LegendLayout == null)
           // {
                if (Placement == LegendPlacement.Top || Placement == LegendPlacement.Bottom)
                {
                    if (Orientation == LegendOrientation.Vertical)
                    {
                        LegendLayout = new VerticalStackLayout() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Fill };
                    }
                    else
                    {
                        //CenterAndExpand not working in windows platform, hence used FillAndExpand. 
#if WINDOWS
                        LegendLayout = new HorizontalStackLayout() { HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };
#else
                        LegendLayout = new HorizontalStackLayout() { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Fill };
#endif
                    }
                }
                else
                {
                    if (Orientation == LegendOrientation.Horizontal)
                    {
                        LegendLayout = new HorizontalStackLayout() { HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Center };
                    }
                    else
                    {
                        LegendLayout = new VerticalStackLayout() { HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Center };
                    }
                }
           // }

            ScrollViewContent = new ContentView();

            if (Placement == LegendPlacement.Top || Placement == LegendPlacement.Bottom)
            {
                ScrollViewContent.HorizontalOptions = LayoutOptions.Center;
                ScrollViewContent.VerticalOptions = LayoutOptions.Fill;
            }
            else
            {
                ScrollViewContent.HorizontalOptions = LayoutOptions.Fill;
                ScrollViewContent.VerticalOptions = LayoutOptions.Center;
            }

            if(Orientation == LegendOrientation.Vertical)
            {
                //Excpetion thrown when using Padding property in windows, so used Margin property for windows alone.
#if WINDOWS
                ScrollViewContent.Margin = new Thickness(2);
#else
                ScrollViewContent.Padding = new Thickness(2);
#endif
            }
            else
            {
#if WINDOWS
                ScrollViewContent.Margin = new Thickness(5, 2, 5, 2);
#else
                ScrollViewContent.Padding = new Thickness(5, 2, 5, 2);
#endif
            }

            ScrollViewContent.Content = LegendLayout;

            LegendScrollView = new ScrollView()
            {
                Orientation = ScrollOrientation.Both,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Never,
                VerticalScrollBarVisibility = ScrollBarVisibility.Never,
                Content = ScrollViewContent
            };

            if (Placement == LegendPlacement.Top || Placement == LegendPlacement.Bottom)
            {
                LegendScrollView.HorizontalOptions = LayoutOptions.Fill;
                LegendScrollView.VerticalOptions = LayoutOptions.Fill;
            }
            else
            {
                LegendScrollView.HorizontalOptions = LayoutOptions.Fill;
                LegendScrollView.VerticalOptions = LayoutOptions.Fill;
            }

            this.Content = LegendScrollView;
        }

        private void LegendTapGesture_Tapped(object? sender, EventArgs e)
        {
            var content = sender as ContentView;
            if (ToggleVisibility && content != null && content.BindingContext != null)
            {
                var legendItem = content.BindingContext as LegendItem;
                if (legendItem != null)
                {
                    legendItem.IsToggled = !legendItem.IsToggled;

                    if (ItemClicked != null)
                    {
                        ItemClicked(this, new LegendItemClickedEventArgs() { LegendItem = legendItem });
                    }
                }
            }
        }

        /// <summary>
        /// Metod used to get the default legend template.
        /// </summary>
        /// <returns></returns>
        private DataTemplate GetDefaultLegendTemplate()
        {
            var template = new DataTemplate(() =>
            {
                HorizontalStackLayout stack = new HorizontalStackLayout()
                {
                    Spacing = 6,
                    Padding = new Thickness(8,10)
                };

                ToggleColorConverter toggleColorConverter = new ToggleColorConverter();
                Binding binding;
                Binding binding1;
                MultiBinding multiBinding;

                SfShapeView shapeView = CreateShapeView();
                if (shapeView != null)
                {
                    shapeView.HorizontalOptions = LayoutOptions.Start;
                    shapeView.VerticalOptions = LayoutOptions.Center;
                    binding = new Binding(nameof(LegendItem.IsToggled));
                    binding.Converter = toggleColorConverter;
                    binding.ConverterParameter = shapeView;
                    binding1 = new Binding(nameof(LegendItem.IconBrush));
                    multiBinding = new MultiBinding()
                    {
                        Bindings = new List<BindingBase>() { binding, binding1 },
                        Converter = new MultiBindingIconBrushConverter(),
                        ConverterParameter = shapeView
                    };

                    shapeView.SetBinding(SfShapeView.IconBrushProperty, multiBinding);
                    shapeView.SetBinding(SfShapeView.ShapeTypeProperty, nameof(LegendItem.IconType));
                    shapeView.SetBinding(SfShapeView.HeightRequestProperty, nameof(LegendItem.IconHeight));
                    shapeView.SetBinding(SfShapeView.WidthRequestProperty, nameof(LegendItem.IconWidth));
                    stack.Children.Add(shapeView);
                }

                Label label = CreateLabelView();
                if (label != null)
                {
                    label.VerticalTextAlignment = TextAlignment.Center;
                    label.SetBinding(Label.TextProperty, nameof(LegendItem.Text));
                    binding = new Binding(nameof(LegendItem.IsToggled));
                    binding.Converter = toggleColorConverter;
                    binding.ConverterParameter = label;
                    label.SetBinding(Label.TextColorProperty, binding);
                    label.SetBinding(Label.MarginProperty, nameof(LegendItem.TextMargin));
                    label.SetBinding(Label.FontSizeProperty, nameof(LegendItem.FontSize));
                    label.SetBinding(Label.FontFamilyProperty, nameof(LegendItem.FontFamily));
                    label.SetBinding(Label.FontAttributesProperty, nameof(LegendItem.FontAttributes));
                    stack.Children.Add(label);
                }

                return stack;
            });

            return template;
        }

        private void LegendLayoutChanged(object oldValue, object newValue)
        {
            //Todo: Need to change these dynamic change issue when resolved the bindable layout with dynamic changes.
            if (oldValue != null)
            {
                var layout = oldValue as Layout;
                if (layout != null)
                {
                    layout.Children.Clear();
                    layout.ChildAdded -= Layout_ChildAdded;
                }

                if (ScrollViewContent != null)
                {
                    ScrollViewContent.Content = null;
                }
            }

            if (newValue != null)
            {
                var layoutView = newValue as Layout;
                var layoutView1 = newValue as Microsoft.Maui.ILayout;

                if (layoutView != null)
                {
                    layoutView.ChildAdded += Layout_ChildAdded;
                }

                var template = new DataTemplate(() =>
                {
                    var frame = new ContentView() { Padding = 0, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };
                    var legendTapGesture = new TapGestureRecognizer();
                    legendTapGesture.NumberOfTapsRequired = 1;
                    legendTapGesture.Tapped += LegendTapGesture_Tapped;
                    frame.GestureRecognizers.Add(legendTapGesture);

                    DataTemplate dataTemplate = ItemTemplate;

                    if (dataTemplate == null)
                    {
                        dataTemplate = GetDefaultLegendTemplate();
                    }

                    if (dataTemplate != null)
                    {
                        var layout = dataTemplate.CreateContent();
                        var customView = layout is ViewCell ? (layout as ViewCell)?.View : layout as View;
                        if (customView != null)
                        {
                            frame.Content = customView;
                        }
                    }

                    return frame;
                });

                BindableLayout.SetItemTemplate(LegendLayout, template);

                if (ItemsSource != null)
                {
                    BindableLayout.SetItemsSource(LegendLayout, ItemsSource);
                }

                if (ScrollViewContent != null)
                {
                    ScrollViewContent.Content = LegendLayout;
                }
            }
        }

        private void Layout_ChildAdded(object? sender, ElementEventArgs e)
        {
            DataTemplate template = ItemTemplate;
            if (template == null)
            {
                if (ItemsSource != null && ItemsSource is IEnumerable<LegendItem>)
                {
                    template = GetDefaultLegendTemplate();
                }
            }

            if (template != null)
            {
                var layout = template.CreateContent();
                var customView = layout is ViewCell ? (layout as ViewCell)?.View : layout as View;
                if (customView != null && e.Element != null)
                {
                    var contentView = e.Element as ContentView;
                    if (contentView != null)
                    {
                        contentView.Content = customView;
                    }
                }
            }
        }

        private void OnItemsSourceChanged(object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            if (oldValue != null)
            {
                //Todo: Need to change these dynamic change issue when resolved the bindable layout with dynamic changes.

                //if (LegendLayout != null && LegendLayout.Children != null)
                //{
                //    LegendLayout.Children.Clear();
                //}
            }

            if (newValue != null && LegendLayout != null)
            {
                BindableLayout.SetItemsSource(LegendLayout, newValue as IEnumerable);
            }
        }

        private void OnItemTemplateChanged(object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            if (oldValue != null && LegendLayout != null && LegendLayout.Children != null && LegendLayout.Children.Count > 0)
            {
                LegendLayout.Children.Clear();
            }

            if (newValue != null && LegendLayout != null && LegendLayout.Children != null)
            {
                foreach (ContentView item in LegendLayout.Children)
                {
                    var template = newValue as DataTemplate;
                    if (template != null)
                    {
                        var layout = template.CreateContent();
                        var customView = layout is ViewCell ? (layout as ViewCell)?.View : layout as View;

                        if (customView != null)
                        {
                            item.Content = customView;
                        }
                    }
                }
            }
        }

        #endregion

        #endregion
    }

    internal class LegendItemClickedEventArgs : EventArgs
    {
        internal LegendItem? LegendItem { get; set; }
    }

    /// <summary>
    /// Delegate for the LegendItemToggleHandler event.  
    /// </summary>
    /// <param name="legendItem">Used to specifyt he legend item.</param>
    public delegate void LegendHandler(ILegendItem legendItem);

    /// <summary>
    /// This class serves as an event data for the <see cref="LegendItemEventArgs"/> event. The event data holds the information when the legend item. 
    /// </summary>
    public class LegendItemEventArgs
    {
        /// <summary>
        /// Gets the corresponding legend item.
        /// </summary>
        public readonly ILegendItem LegendItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendItemEventArgs"/> class.
        /// </summary>
        /// <param name="legendItem">Used to specifyt he legend item.</param>
        public LegendItemEventArgs(ILegendItem legendItem)
        {
            LegendItem = legendItem;
        }
    }
}
