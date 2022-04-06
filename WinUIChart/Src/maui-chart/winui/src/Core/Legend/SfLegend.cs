namespace Syncfusion.UI.Xaml.Charts
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using System.Collections;
    using System.Collections.ObjectModel;
    using Windows.Foundation;

    public class SfLegend : ItemsControl, ILegend
    {
        #region Dependency properties

        /// <summary>
        /// The DependencyProperty for <see cref="ToggleVisibility"/> property.
        /// </summary>
        internal static readonly DependencyProperty ToggleVisibilityProperty =
            DependencyProperty.Register(nameof(ToggleVisibility), typeof(bool), typeof(SfLegend), new PropertyMetadata(false));

        /// <summary>
        /// The DependencyProperty for <see cref="Orientation"/> property.
        /// </summary>
        internal static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(LegendOrientation), typeof(SfLegend), new PropertyMetadata(LegendOrientation.Horizontal));

        /// <summary>
        /// The DependencyProperty for <see cref="Placement"/> property.
        /// </summary>
        internal static readonly DependencyProperty PlacementProperty =
            DependencyProperty.Register(nameof(Placement), typeof(LegendDock), typeof(SfLegend), new PropertyMetadata(LegendDock.Top));

        /// <summary>
        /// The DependencyProperty for <see cref="LegendItems"/> property.
        /// </summary>
        internal static readonly DependencyProperty LegendItemsProperty =
            DependencyProperty.Register(nameof(LegendItems), typeof(ObservableCollection<ILegendItem>), typeof(SfLegend), new PropertyMetadata(null));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfLegend"/> class.
        /// </summary>
        public SfLegend()
        {
            this.DefaultStyleKey = typeof(SfLegend);
        }

        #endregion

        #region Properties

        
        public ObservableCollection<ILegendItem> LegendItems
        {
            get { return (ObservableCollection<ILegendItem>)GetValue(LegendItemsProperty); }
            set { SetValue(LegendItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to bind the series visibility with its corresponding legend item in the legend. This is bindable property.
        /// </summary>
        public bool ToggleVisibility
        {
            get { return (bool)GetValue(ToggleVisibilityProperty); }
            set { SetValue(ToggleVisibilityProperty, value); }
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

        internal LegendDock Placement
        {
            get { return (LegendDock)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        bool ILegend.IsVisible { get; set; }

        LegendDock ILegend.Placement { get => Placement; set { } }

        LegendOrientation ILegend.Orientation { get => Orientation; set { } }

        #endregion
    }
}

     
