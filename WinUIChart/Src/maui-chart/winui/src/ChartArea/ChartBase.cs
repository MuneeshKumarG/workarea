using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
#if WinUI_Desktop
using System.ComponentModel;
#endif
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Windows.ApplicationModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using Windows.UI;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// <see cref="ChartBase"/> is a base class for chart. Which represents a chart control with basic presentation characteristics. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public abstract partial class ChartBase : Control
    {
        #region Dependency properties

        /// <summary>
        /// The DependencyProperty for <see cref="Header"/> property.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(ChartBase), new PropertyMetadata(null));


        /// <summary>
        /// The DependencyProperty for <see cref="Legend"/> property.
        /// </summary>
        public static readonly DependencyProperty LegendProperty =
            DependencyProperty.Register(nameof(Legend), typeof(ChartLegend), typeof(ChartBase), new PropertyMetadata(null, OnLegendPropertyChanged));

        #endregion
        
        #region Fields

        internal Size AvailableSize;
        internal LegendDockPanel LegendDockPanel;
        internal CartessianAreaPanel CartessianAreaPanel;
        //internal Panel SyncfusionPlotArea;

        #endregion

        #region Constructor

        /// <summary>
        /// Called when instance created for <see cref="ChartBase"/>. 
        /// </summary>

        public ChartBase()
        {
            LegendItems = new ObservableCollection<LegendItem>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets title for the chart.
        /// </summary>        
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Gets or sets a legend that helps to identify the series in the chart.
        /// </summary>
        /// <value>
        /// The legend.
        /// </value>
        public ChartLegend Legend
        {
            get { return (ChartLegend)GetValue(LegendProperty); }
            set { SetValue(LegendProperty, value); }
        }

        internal ObservableCollection<LegendItem> LegendItems;

        #endregion


        #region Methods

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            LegendDockPanel = GetTemplateChild("LegendDockPanel") as LegendDockPanel;
            CartessianAreaPanel = GetTemplateChild("CartessianAreaPanel") as CartessianAreaPanel;
        }

        /// <summary>
        /// Provides the behavior for the Measure pass of Silverlight layout. Classes can override this method to define their own Measure pass behavior.
        /// </summary>
        /// <returns>
        /// The size that this object determines it needs during layout, based on its calculations of the allocated sizes for child objects; or based on other considerations, such as a fixed container size.
        /// </returns>
        /// <param name="availableSize">The size value.</param>
        protected override Size MeasureOverride(Size availableSize)
        {
            bool isHeightNotContains = double.IsPositiveInfinity(availableSize.Height);
            bool isWidthNotContains = double.IsPositiveInfinity(availableSize.Width);

            if (isHeightNotContains || isWidthNotContains)
                AvailableSize = new Size(isWidthNotContains ? ActualWidth == 0d ? 500d : ActualWidth : availableSize.Width,
                    isHeightNotContains ? ActualHeight == 0d ? 300d : ActualHeight : availableSize.Height);
            else
                AvailableSize = availableSize;

            return base.MeasureOverride(AvailableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }

        private static void OnLegendPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var area = d as ChartBase;
            if (area != null && area.LegendDockPanel != null)
            {
                if (e.NewValue is ChartLegend element)
                    area.LegendDockPanel.Legend = element;
                else
                    area.LegendDockPanel.Legend = null;
            }
        }

        #endregion
    }
}
