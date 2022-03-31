// <copyright file="LegendDockPanel.cs" company="Syncfusion. Inc">
// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>
namespace Syncfusion.UI.Xaml.Charts
{
    using Microsoft.UI;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media;
    using System.Collections.ObjectModel;
    using Windows.Foundation;
    using Rect = Windows.Foundation.Rect;

    /// <summary>
    /// Arranges child elements around the edges of the panel. Optionally, 
    /// last added child element can occupy the remaining space.
    /// </summary>
    /// <QualityBand>Stable</QualityBand>   
    internal class LegendDockPanel : Panel
    {
        #region Dependency Property Registration

        /// <summary>
        ///  The DependencyProperty for Dock property.
        /// </summary>
        public static readonly DependencyProperty DockProperty =
            DependencyProperty.RegisterAttached(nameof(DockProperty), typeof(LegendDock), typeof(LegendDockPanel), new PropertyMetadata(LegendDock.Top));

        /// <summary>
        /// The DependencyProperty for <see cref="PlotArea"/> property.
        /// </summary>
        public static readonly DependencyProperty PlotAreaProperty =
          DependencyProperty.Register(nameof(PlotArea), typeof(UIElement), typeof(LegendDockPanel), new PropertyMetadata(null, new PropertyChangedCallback(OnRootElementChanged)));

        #endregion

        #region Fields

        private UIElement plotArea;
        private ILegend legend;

        internal SfLegend SfLegend;

        #endregion

        #region Constructor

        public LegendDockPanel()
        {
            CreateLegend();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the root element. This is a dependency property.
        /// </summary>
        /// <value>The root element.</value>
        public UIElement PlotArea
        {
            get
            {
                return plotArea;
            }

            set
            {
                if (plotArea == null && plotArea != value)
                {
                    SetValue(PlotAreaProperty, value);
                    plotArea = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the legend element. This is a dependency property.
        /// </summary>
        /// <value>The root element.</value>
        internal ILegend Legend
        {
            get
            {
                return legend;
            }

            set
            {
                if (legend == null && legend != value)
                {
                    legend = value;
                    CreateLegend();
                }
            }
        }

        #endregion


        #endregion

        #region Methods

        #region Protected Methods

        protected override Size MeasureOverride(Size availableSize)
        {
            SfLegend?.Measure(availableSize);
            return base.MeasureOverride(availableSize);
        }

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines a size for a <see cref="FrameworkElement"/> derived class.
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        protected override Size ArrangeOverride(Size finalSize)
        {
            var areaBounds = new Rect(0, 0, finalSize.Width, finalSize.Height);

            if (SfLegend != null)
            {
                var legendSize = SfLegend.DesiredSize;

                if (SfLegend.Placement == LegendDock.Top)
                {
                    SfLegend.Arrange(new Rect(0, 0, finalSize.Width, legendSize.Height));

                    areaBounds = new Rect(0, legendSize.Height, (float)finalSize.Width, finalSize.Height - legendSize.Height);
                }
            }

            plotArea?.Arrange(areaBounds);

            return base.ArrangeOverride(finalSize);
        }

        #endregion

        #region Private Static Methods


        /// <summary>
        /// Called when root element is changed.
        /// </summary>
        /// <param name="dpObj">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnRootElementChanged(DependencyObject dpObj, DependencyPropertyChangedEventArgs e)
        {
            LegendDockPanel dockPanel = dpObj as LegendDockPanel;

            if (dockPanel != null)
            {
                if (e.OldValue != null)
                {
                    dockPanel.Children.Remove(e.OldValue as UIElement);
                }

                if (e.NewValue != null)
                {
                    dockPanel.Children.Add(e.NewValue as UIElement);
                }
            }
        }

        /// <summary>
        /// Called when legend element is changed.
        /// </summary>
        /// <param name="dpObj">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnLegendChanged(DependencyObject dpObj, DependencyPropertyChangedEventArgs e)
        {
            LegendDockPanel dockPanel = dpObj as LegendDockPanel;

            if (dockPanel != null)
            {
                if (e.OldValue != null)
                {
                    dockPanel.Children.Remove(e.OldValue as UIElement);
                }

                if (e.NewValue != null)
                {
                    dockPanel.Children.Add(e.NewValue as UIElement);
                }
            }
        }


        #endregion

        private void CreateLegend()
        {
            SfLegend = new SfLegend();
            SfLegend.Background = new SolidColorBrush(Colors.Red);

            ObservableCollection<ILegendItem> LegendItems = new ObservableCollection<ILegendItem>();
            LegendItems.Add(new LegendItem()
            {
                Text="Item 1"
            });
            LegendItems.Add(new LegendItem()
            {
                Text = "Item 2"
            });
            SfLegend.ItemsSource = LegendItems;
            this.Children.Add(SfLegend);
        }


        #endregion
    }
}
