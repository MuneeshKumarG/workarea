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
    public abstract partial class ChartBase : Control, IPlotArea
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

        private Size? rootPanelDesiredSize;
        internal Size AvailableSize;
        private EventHandler<LegendItemEventArgs> legendItemsToggled;

        internal bool isUpdateDispatched = false;
        internal LegendDockPanel LegendDockPanel;
        internal Panel SyncfusionPlotArea;

        #endregion

        #region Constructor

        /// <summary>
        /// Called when instance created for <see cref="ChartBase"/>. 
        /// </summary>

        public ChartBase()
        {
            SizeChanged += OnSizeChanged;
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


        internal Size? RootPanelDesiredSize
        {
            get { return rootPanelDesiredSize; }
            set
            {
                if (rootPanelDesiredSize == value) return;
                rootPanelDesiredSize = value;

                OnRootPanelSizeChanged(value != null ? value.Value : new Size());
            }
        }

        #endregion

        #region Legend

        ObservableCollection<ILegendItem> IPlotArea.LegendItems
        {
            get
            {
                if (Legend != null)
                    return Legend.LegendItems;
                else return null;
            }
        }

        Rect IPlotArea.PlotAreaBounds
        {
            get
            {
                if (SyncfusionPlotArea != null)
                    return new Rect(new Point(0, 0), SyncfusionPlotArea.DesiredSize);
                return Rect.Empty;
            }
        }

        event EventHandler<LegendItemEventArgs> IPlotArea.LegendItemToggled { add { legendItemsToggled += value; } remove { legendItemsToggled -= value; } }

        void IPlotArea.UpdateLegendItems()
        {

        }

        #endregion

        #region Methods

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            LegendDockPanel = GetTemplateChild("LegendDockPanel") as LegendDockPanel;
            SyncfusionPlotArea = GetTemplateChild("SyncfusionPlotArea") as Panel;
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
            bool needForceSizeChanged = false;
            double width = availableSize.Width, height = availableSize.Height;

            if (double.IsInfinity(width))
            {
                width = ActualWidth == 0d ? 500d : ActualWidth;
                needForceSizeChanged = true;
            }
            if (double.IsInfinity(height))
            {
                height = ActualHeight == 0d ? 300d : ActualHeight;
                needForceSizeChanged = true;
            }
            if (needForceSizeChanged)
                AvailableSize = new Size(width, height);
            else
                AvailableSize = availableSize;

            return base.MeasureOverride(AvailableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }

        #region Protected Virtual Methods

        /// <summary>
        /// Called when root panel size changed.
        /// </summary>
        /// <param name="size">The size.</param>
        internal virtual void OnRootPanelSizeChanged(Size size)
        {
            ScheduleUpdate();
        }

        #endregion

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

        void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize != AvailableSize)
                InvalidateMeasure();
        }

        void ToggleLegendItem(ILegendItem legendItem)
        {
           
        }

        internal void ScheduleUpdate()
        {
            var _isInDesignMode = DesignMode.DesignModeEnabled;

            if (!isUpdateDispatched && !_isInDesignMode)
            {
                DispatcherQueue.TryEnqueue(() => { UpdateArea(); });
                isUpdateDispatched = true;
            }
            else if (_isInDesignMode)
                UpdateArea(true);
        }

        internal void UpdateArea()
        {
            UpdateArea(false);
        }

        internal virtual void UpdateArea(bool forceUpdate)
        {
            LegendDockPanel.Legend = Legend;
        }

       

        #endregion
    }
}
