using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CategoryAxis
    {
        #region Bindable Properties
        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
            nameof(Interval),
            typeof(double),
            typeof(CategoryAxis),
            double.NaN,
            BindingMode.Default,
            null,
            OnIntervalPropertyChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty LabelPlacementProperty = BindableProperty.Create(
            nameof(LabelPlacement),
            typeof(LabelPlacement),
            typeof(CategoryAxis),
            LabelPlacement.OnTicks,
            BindingMode.Default,
            null,
            OnLabelPlacementPropertyChanged);

        #endregion

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public LabelPlacement LabelPlacement
        {
            get { return (LabelPlacement)GetValue(LabelPlacementProperty); }
            set { SetValue(LabelPlacementProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        #endregion

        #region Private Methods
        private static void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as CategoryAxis;
            if (axis != null)
            {
                axis.AxisInterval = (double)newValue;
                axis.UpdateLayout();
            }
        }

        private static void OnLabelPlacementPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as CategoryAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        #endregion
    }
}
