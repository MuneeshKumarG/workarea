using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.UI.Xaml.Charts
{
    public class ChartLegend : DependencyObject, ILegend
    {
        #region Dependency properties

        public static readonly DependencyProperty PlacementProperty = 
            DependencyProperty.Register(nameof(Placement), typeof(LegendDock), typeof(ChartLegend), new PropertyMetadata(LegendDock.Top));

        public static readonly DependencyProperty LegendItemsProperty = 
            DependencyProperty.Register(nameof(LegendItems), typeof(ObservableCollection<ILegendItem>), typeof(ChartLegend), new PropertyMetadata(null, OnLegendItemsChanged));

        public static readonly DependencyProperty ToggleSeriesVisibilityProperty = 
            DependencyProperty.Register(nameof(ToggleSeriesVisibility), typeof(bool), typeof(ChartLegend), new PropertyMetadata(false));

        internal static readonly DependencyProperty OrientationProperty = 
            DependencyProperty.Register(nameof(Orientation), typeof(LegendOrientation), typeof(ChartLegend), new PropertyMetadata(LegendOrientation.Default));

        internal static readonly DependencyProperty ItemMarginProperty = 
            DependencyProperty.Register(nameof(ItemMargin), typeof(Thickness), typeof(ChartLegend), new PropertyMetadata(new Thickness(double.NaN)));

        public static readonly DependencyProperty IsVisibleProperty = 
            DependencyProperty.Register(nameof(IsVisible), typeof(bool), typeof(ChartLegend), new PropertyMetadata(true));

        #endregion

        #region Constructor

        public ChartLegend()
        {
            LegendItems=new ObservableCollection<ILegendItem>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public bool IsVisible
        {
            get { return (bool)GetValue(IsVisibleProperty); }
            set { SetValue(IsVisibleProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public LegendDock Placement
        {
            get { return (LegendDock)GetValue(PlacementProperty); }
            set { SetValue(PlacementProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal LegendOrientation Orientation
        {
            get { return (LegendOrientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ToggleSeriesVisibility
        {
            get { return (bool)GetValue(ToggleSeriesVisibilityProperty); }
            set { SetValue(ToggleSeriesVisibilityProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal Thickness ItemMargin
        {
            get { return (Thickness)GetValue(ItemMarginProperty); }
            set { SetValue(ItemMarginProperty, value); }
        }

        public ObservableCollection<ILegendItem> LegendItems
        {
            get { return (ObservableCollection<ILegendItem>)GetValue(LegendItemsProperty); }
            set { SetValue(LegendItemsProperty, value); }
        }

        bool ILegend.ToggleVisibility { get => ToggleSeriesVisibility; set => ToggleSeriesVisibility = value; }
        LegendOrientation ILegend.Orientation { get => Orientation; set => Orientation = value; }

        #endregion

        #region Methods
        private static void OnLegendItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var area = d as ChartLegend;
            if (area != null)
            {
                
            }
        }

        #endregion
    }
}
