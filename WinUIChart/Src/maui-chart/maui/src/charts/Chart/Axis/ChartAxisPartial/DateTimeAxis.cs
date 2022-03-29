using Microsoft.Maui.Controls;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DateTimeAxis
    {
        #region Bindable Properties
        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty IntervalTypeProperty = BindableProperty.Create(
            nameof(IntervalType),
            typeof(DateTimeIntervalType),
            typeof(DateTimeAxis),
            DateTimeIntervalType.Auto,
            BindingMode.Default,
            null,
            OnIntervalTypePropertyChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty MinimumProperty = BindableProperty.Create(
            nameof(Minimum),
            typeof(DateTime?),
            typeof(DateTimeAxis),
            null,
            BindingMode.Default,
            null,
            OnMinimumPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty IntervalProperty = BindableProperty.Create(
            nameof(Interval),
            typeof(double),
            typeof(DateTimeAxis),
            double.NaN,
            BindingMode.Default,
            null,
            OnIntervalPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty MaximumProperty = BindableProperty.Create(
            nameof(Maximum),
            typeof(DateTime?),
            typeof(DateTimeAxis),
            null,
            BindingMode.Default,
            null,
            OnMaximumPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty RangePaddingProperty = BindableProperty.Create(
            nameof(RangePadding),
            typeof(DateTimeRangePadding),
            typeof(DateTimeAxis),
            DateTimeRangePadding.Auto,
            BindingMode.Default,
            null,
            OnRangePaddingPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        internal static readonly BindableProperty AutoScrollingDeltaTypeProperty = BindableProperty.Create(
            nameof(AutoScrollingDeltaType),
            typeof(DateTimeDeltaType),
            typeof(DateTimeAxis),
            DateTimeDeltaType.Auto,
            BindingMode.Default,
            null,
            OnAutoScrollingDeltaTypePropertyChanged);
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
        public DateTime? Minimum
        {
            get { return (DateTime?)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Maximum
        {
            get { return (DateTime?)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeRangePadding RangePadding
        {
            get { return (DateTimeRangePadding)GetValue(RangePaddingProperty); }
            set { SetValue(RangePaddingProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeIntervalType IntervalType
        {
            get { return (DateTimeIntervalType)GetValue(IntervalTypeProperty); }
            set { SetValue(IntervalTypeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal DateTimeIntervalType ActualIntervalType { get; set; }

        /// <summary>
        /// Gets or sets the date time unit of the value specified in the <c>AutoScrollingDelta</c> property. 
        /// </summary>
        /// <value>This property takes the DateTimeDeltaType.</value>
        internal DateTimeDeltaType AutoScrollingDeltaType
        {
            get { return (DateTimeDeltaType)GetValue(AutoScrollingDeltaTypeProperty); }
            set { SetValue(AutoScrollingDeltaTypeProperty, value); }
        }

        #endregion

        #region Callback Methods

        private static void OnIntervalPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as DateTimeAxis;
            if (axis != null)
            {
                axis.AxisInterval = (double)newValue;
                axis.UpdateLayout();
            }
        }

        private static void OnRangePaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as DateTimeAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnIntervalTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as DateTimeAxis;
            if (axis != null)
            {
                axis.ActualIntervalType = (DateTimeIntervalType)newValue;
                axis.UpdateLayout();
            }
        }

        private static void OnAutoScrollingDeltaTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as DateTimeAxis;
            if (axis != null)
            {
                axis.UpdateLayout();
            }
        }

        private static void OnMaximumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as DateTimeAxis;
            if (axis != null)
            {
                axis.ActualMaximum = (DateTime?)newValue;
                axis.OnMinMaxChanged();
                axis.UpdateLayout();
            }
        }

        private static void OnMinimumPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var axis = bindable as DateTimeAxis;
            if (axis != null)
            {
                axis.ActualMinimum = (DateTime?)newValue;
                axis.OnMinMaxChanged();
                axis.UpdateLayout();
            }
        }

        private void StripLine_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }
        #endregion
    }
}
