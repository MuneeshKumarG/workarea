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

        internal LegendDockPanel LegendDockPanel { get; set; }

        #region Constructor

        /// <summary>
        /// Called when instance created for <see cref="ChartBase"/>. 
        /// </summary>

        public ChartBase()
        {
        }

        #endregion

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            LegendDockPanel = GetTemplateChild("LegendDockPanel") as LegendDockPanel;
        }

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
    }
}
