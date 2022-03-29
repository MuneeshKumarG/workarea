using Microsoft.Maui.Controls;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NumericalAxis
    {
        #region Bindable Properties
        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty RangePaddingProperty = BindableProperty.Create(
            nameof(RangePadding),
            typeof(NumericalPadding),
            typeof(NumericalAxis),
            NumericalPadding.Auto,
            BindingMode.Default,
            null,
            OnRangePaddingPropertyChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
                nameof(Interval),
                typeof(double),
                typeof(NumericalAxis),
                double.NaN,
                BindingMode.Default,
                null,
                OnIntervalPropertyChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
            nameof(Minimum),
            typeof(double?),
            typeof(NumericalAxis),
            null,
            BindingMode.Default,
            null,
            OnMinimumPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
                nameof(Maximum),
                typeof(double?),
                typeof(NumericalAxis),
                null,
                BindingMode.Default,
                null,
                OnMaximumPropertyChanged);
        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public double Interval
        {
            get { return (double)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double? Minimum
        {
            get { return (double?)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public double? Maximum
        {
            get { return (double?)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public NumericalPadding RangePadding
        {
            get { return (NumericalPadding)GetValue(RangePaddingProperty); }
            set { SetValue(RangePaddingProperty, value); }
        }
        #endregion

        #region Callback Methods
        private static void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as NumericalAxis;
            if (axis != null)
            {
                axis.AxisInterval = (double)newValue;
                axis.UpdateLayout();
            }
        }

        private static void OnMinimumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as NumericalAxis;
            if (axis != null)
            {
                axis.ActualMinimum = Convert.ToDouble(newValue ?? double.NaN);
                axis.OnMinMaxChanged();
                axis.UpdateLayout();
            }
        }

        private static void OnMaximumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as NumericalAxis;
            if (axis != null)
            {
                axis.ActualMaximum = Convert.ToDouble(newValue ?? double.NaN);
                axis.OnMinMaxChanged();
                axis.UpdateLayout();
            }
        }

        private static void OnRangePaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as ChartAxis)?.UpdateLayout();
        }

        #endregion
    }
}
