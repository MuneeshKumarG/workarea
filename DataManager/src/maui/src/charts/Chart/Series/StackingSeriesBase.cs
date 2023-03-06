using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// StackingSeriesBase is the base class for Stacking column series.
    /// </summary>
    public abstract class StackingSeriesBase : XYDataSeries
    {
        #region Bindable Properties

        /// <summary>
        ///  Identifies the <see cref="GroupingLabel"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty GroupingLabelProperty =
            BindableProperty.Create(
                nameof(GroupingLabel),
                typeof(string),
                typeof(StackingSeriesBase), string.Empty, BindingMode.Default, propertyChanged: OnGroupingLabelChanged);

        #endregion

        #region Public properties

        /// <summary>
        /// Using this property we can group the series
        /// </summary>
        public string GroupingLabel
        {
            get { return (string)GetValue(GroupingLabelProperty); }
            set { SetValue(GroupingLabelProperty, value); }
        }
        
        #endregion

        #region Internal Properties

        internal IList<double>? TopValues { get; set; }

        internal IList<double>? BottomValues { get; set; }

        #endregion

        #region Methods

        #region Private Methods
        
        private static void OnGroupingLabelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            StackingSeriesBase? series = bindable as StackingSeriesBase;

            if (series != null && series.ChartArea != null)
            {
                series.RefreshSeries();
            }
        }
       
        private void RefreshSeries()
        {
            if (ChartArea == null || ChartArea.Series == null)
            {
                return;
            }

            foreach (var series in ChartArea.Series.OfType<StackingSeriesBase>())
            {
                series.SegmentsCreated = false;
            }

            ChartArea.SideBySideSeriesPosition = null;
            ChartArea.ScheduleUpdateArea();
        }

        #endregion

        #endregion


    }
}
