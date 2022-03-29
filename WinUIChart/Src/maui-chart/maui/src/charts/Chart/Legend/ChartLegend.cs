

using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Core;
using System.Collections;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.Legend>
    ///               <chart:ChartLegend/>
    ///           </chart:SfCartesianChart.Legend>
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     chart.Legend = new ChartLegend ();
    ///     
    /// ]]></code>
    /// ***
    /// </example>
    public class ChartLegend : BindableObject, ILegend
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty PlacementProperty = BindableProperty.Create(nameof(Placement), typeof(LegendPlacement), typeof(ChartLegend), LegendPlacement.Top, BindingMode.Default, null, null, null, null);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty ToggleSeriesVisibilityProperty = BindableProperty.Create(nameof(ToggleSeriesVisibility), typeof(bool), typeof(ChartLegend), false, BindingMode.Default, null, null, null, null);

        /// <summary>
        /// 
        /// </summary>        
        internal static readonly BindableProperty OrientationProperty = BindableProperty.Create(nameof(Orientation), typeof(LegendOrientation), typeof(ChartLegend), LegendOrientation.Default, BindingMode.Default, null, null, null, null);

        /// <summary>
        /// 
        /// </summary>        
        internal static readonly BindableProperty ItemMarginProperty = BindableProperty.Create(nameof(ItemMargin), typeof(Thickness), typeof(ChartLegend), new Thickness(double.NaN), BindingMode.Default, null, null, null, null);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(ChartLegend), true, BindingMode.Default, null, null, null, null);

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
        public LegendPlacement Placement
        {
            get { return (LegendPlacement)GetValue(PlacementProperty); }
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

        IEnumerable? itemsSource;
#pragma warning disable CS8603 // Possible null reference return.
        IEnumerable ILegend.ItemsSource { get => itemsSource; set => itemsSource= value; }
#pragma warning restore CS8603 // Possible null reference return.
        bool ILegend.ToggleVisibility { get => ToggleSeriesVisibility; set => ToggleSeriesVisibility = value; }
        LegendOrientation ILegend.Orientation { get => Orientation; set => Orientation = value; }

        #endregion

        #region Methods

        internal void Dispose()
        {

        }

        #endregion
    }
}
