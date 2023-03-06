// <copyright file="ChartDockPanel.cs" company="Syncfusion. Inc">
// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>
namespace Syncfusion.UI.Xaml.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media;
    using Windows.Foundation;
    using Rect = Windows.Foundation.Rect;

    /// <summary>
    /// Represents the position of a child element in the chart.
    /// </summary>  
    public enum LegendPlacement
    {
        /// <summary>
        /// Docks element at the left side of panel.
        /// </summary>
        Left,

        /// <summary>
        /// Docks element at the top side of panel.
        /// </summary>
        Top,

        /// <summary>
        /// Docks element at the right side of panel.
        /// </summary>
        Right,

        /// <summary>
        /// Docks element at the bottom side of panel.
        /// </summary>
        Bottom,
    }

    /// <summary>
    /// Arranges child elements around the edges of the panel. Optionally, 
    /// last added child element can occupy the remaining space.
    /// </summary>
    /// <QualityBand>Stable</QualityBand>   
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class LegendPanel : Panel
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="LastChildFill"/> property.
        /// </summary>
        public static readonly DependencyProperty LastChildFillProperty =
            DependencyProperty.Register(
                "LastChildFill",
                typeof(bool),
                typeof(LegendPanel),
                new PropertyMetadata(true, OnLastChildFillPropertyChanged));

        /// <summary>
        ///  The DependencyProperty for Dock property.
        /// </summary>
        public static readonly DependencyProperty DockProperty =
            DependencyProperty.RegisterAttached(
                "Dock",
                typeof(LegendPlacement),
                typeof(LegendPanel),
                new PropertyMetadata(LegendPlacement.Top, OnDockPropertyChanged));

        /// <summary>
        ///  The DependencyProperty for <see cref="Host"/> property.
        /// </summary>
        internal static readonly DependencyProperty HostProperty =
            DependencyProperty.Register(
                "Host",
                typeof(string),
                typeof(LegendPanel),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// The DependencyProperty for <see cref="ElementMargin"/> property.
        /// </summary>
        public static readonly DependencyProperty ElementMarginProperty =
          DependencyProperty.Register(
              "ElementMargin",
              typeof(Thickness),
              typeof(LegendPanel),
            new PropertyMetadata(new Thickness().GetThickness(0, 0, 0, 0)));

        /// <summary>
        /// The DependencyProperty for <see cref="AreaPanel"/> property.
        /// </summary>
        public static readonly DependencyProperty AreaPanelProperty =
          DependencyProperty.Register(
              "RootElement",
              typeof(UIElement),
              typeof(LegendPanel),
              new PropertyMetadata(null, new PropertyChangedCallback(OnAreaPanelChanged)));

        #endregion

        #region Fields

        /// <summary>
        /// Initializes m_rootElement
        /// </summary>
        public UIElement areaPanel;

        /// <summary>
        /// Initializes m_controlsThickness
        /// </summary>
        private Thickness m_controlsThickness = new Thickness();

        /// <summary>
        /// Initializes m_resultDockRect
        /// </summary>
        private Rect m_resultDockRect = new Rect();

        /// <summary>
        /// A value indicating whether a dependency property change handler
        /// should ignore the next change notification.  This is used to reset
        /// the value of properties without performing any of the actions in
        /// their change handlers.
        /// </summary>
        private static bool _ignorePropertyChange;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to enable the lastChildFillProperty.
        /// </summary>
        public bool LastChildFill
        {
            get { return (bool)GetValue(LastChildFillProperty); }
            set { SetValue(LastChildFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the root element. This is a dependency property.
        /// </summary>
        /// <value>The root element.</value>
        public UIElement AreaPanel
        {
            get
            {
                return areaPanel;
            }

            set
            {
                if (areaPanel == null && areaPanel != value)
                {
                    SetValue(LegendPanel.AreaPanelProperty, value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the element margin. This is a dependency property.
        /// </summary>
        /// <value>The element margin.</value>
        public Thickness ElementMargin
        {
            get
            {
                return (Thickness)GetValue(LegendPanel.ElementMarginProperty);
            }

            set
            {
                SetValue(LegendPanel.ElementMarginProperty, value);
            }
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the sync chart area.
        /// </summary>
        /// <value>The sync chart area.</value>
        internal string Host
        {
            set { SetValue(HostProperty, value); }
            get { return (string)GetValue(HostProperty); }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Static Methods

        /// <summary>
        /// Gets an element's dock position in the Chart area.
        /// </summary>
        /// <param name="element">any UIElement</param>
        /// <returns>returns dock position of UIElement.</returns>
        public static LegendPlacement GetDock(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (LegendPlacement)element.GetValue(DockProperty);
        }

        /// <summary>
        ///Sets an element's dock position in the Chart area.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="dock"></param>
        public static void SetDock(UIElement element, LegendPlacement dock)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            element.SetValue(DockProperty, dock);
        }

        #endregion

        #region Protected Methods

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            var topSizes = new List<double>();
            var leftSizes = new List<double>();
            var bottomSizes = new List<double>();
            var rightSizes = new List<double>();

            Thickness margin = this.ElementMargin;
            m_controlsThickness = this.ElementMargin;
            foreach (UIElement element in Children)
            {
                if (element == null) continue;
                if (element == areaPanel) continue;
                element.Measure(availableSize);
                Size elemSize = ChartLayoutUtils.Addthickness(element.DesiredSize, margin);
                var chartLegend = element as ChartLegend;
                switch (GetDock(element))
                {
                    case LegendPlacement.Left:
                        if (chartLegend != null)
                        {
                            var index = chartLegend.RowColumnIndex;
                            if (leftSizes.Count <= index)
                                leftSizes.Add(elemSize.Width);
                            else if (leftSizes[index] < elemSize.Width)
                                leftSizes[index] = elemSize.Width;
                        }
                        else
                            m_controlsThickness.Left += elemSize.Width;
                        break;

                    case LegendPlacement.Right:
                        if (chartLegend != null)
                        {
                            var index = chartLegend.RowColumnIndex;
                            if (rightSizes.Count <= index)
                                rightSizes.Add(elemSize.Width);
                            else if (rightSizes[index] < elemSize.Width)
                                rightSizes[index] = elemSize.Width;
                        }
                        else
                            m_controlsThickness.Right += elemSize.Width;
                        break;

                    case LegendPlacement.Top:
                        if (chartLegend != null)
                        {
                            var index = chartLegend.RowColumnIndex;
                            if (topSizes.Count <= index)
                                topSizes.Add(elemSize.Height);
                            else if (topSizes[index] < elemSize.Height)
                                topSizes[index] = elemSize.Height;
                        }
                        else
                            m_controlsThickness.Top += elemSize.Height;
                        break;

                    case LegendPlacement.Bottom:
                        if (chartLegend != null)
                        {
                            var index = chartLegend.RowColumnIndex;
                            if (bottomSizes.Count <= index)
                                bottomSizes.Add(elemSize.Height);
                            else if (bottomSizes[index] < elemSize.Height)
                                bottomSizes[index] = elemSize.Height;
                        }
                        else
                            m_controlsThickness.Bottom += elemSize.Height;
                        break;
                }
            }

            m_controlsThickness.Left += leftSizes.Sum();
            m_controlsThickness.Right += rightSizes.Sum();
            m_controlsThickness.Top += topSizes.Sum();
            m_controlsThickness.Bottom += bottomSizes.Sum();

            try
            {
                areaPanel.Measure(ChartLayoutUtils.Subtractthickness(availableSize, m_controlsThickness));
            }
            catch (Exception)
            {
            }

            return availableSize;
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        protected override Size ArrangeOverride(Size finalSize)
        {
            m_resultDockRect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0, 0), finalSize), m_controlsThickness);
            Rect currRect = new Rect(new Point(0, 0), finalSize);
            Rect resRect = currRect;

            #region Arrange Central Element
            if (areaPanel != null)
            {
                try
                {
                    areaPanel.Arrange(m_resultDockRect);
                }
                catch (Exception)
                {
                }
            }
            #endregion

            #region Arrange All Elements

            double legendTop = -1d;

            for (int i = 0; i < Children.Count; i++)
            {
                UIElement element = Children[i];
                Size elemSize = ChartLayoutUtils.Addthickness(element.DesiredSize, ElementMargin);
                double scale;
                if (element != null && element != areaPanel)
                {
                    var chartLegend = element as ChartLegend;
                    var offsetX = 0d;
                    var offsetY = 0d;
                    if (chartLegend != null)
                    {
                        offsetX = double.IsNaN(chartLegend.OffsetX) ? 0d : chartLegend.OffsetX;
                        offsetY = double.IsNaN(chartLegend.OffsetY) ? 0d : chartLegend.OffsetY;
                    }

                    #region Orientation == Orientation.Horizontal
                    switch (GetDock(element))
                    {
                        case LegendPlacement.Left:
                            if (element is ChartLegend)
                            {
                                var arrangeRect = (element as ChartLegend).ArrangeRect;
                                ArrangeElement(element, LegendPlacement.Left, new Rect(arrangeRect.Left + offsetX + currRect.Left, arrangeRect.Top + offsetY + m_controlsThickness.Top, arrangeRect.Width, arrangeRect.Height));
                            }
                            else
                            {
                                ArrangeElement(element, LegendPlacement.Left, new Rect(currRect.Left, resRect.Y, elemSize.Width, resRect.Height));
                                currRect.X += elemSize.Width;
                                scale = currRect.Width - elemSize.Width;
                                currRect.Width = scale > 0 ? scale : 0;
                            }
                            break;

                        case LegendPlacement.Right:
                            if (element is ChartLegend)
                            {
                                var arrangeRect = (element as ChartLegend).ArrangeRect;
                                ArrangeElement(element, LegendPlacement.Right, new Rect(arrangeRect.Left + offsetX + m_controlsThickness.Left, arrangeRect.Top + offsetY + m_controlsThickness.Top, arrangeRect.Width, arrangeRect.Height));

                            }
                            else
                            {
                                scale = currRect.Width - elemSize.Width;
                                currRect.Width = scale > 0 ? scale : 0;
                                ArrangeElement(element, LegendPlacement.Right, new Rect(currRect.Right, resRect.Top + m_controlsThickness.Top, elemSize.Width, resRect.Height));
                            }
                            break;

                        case LegendPlacement.Top:
                            if (element is ChartLegend)
                            {
                                if (legendTop == -1)
                                {
                                    legendTop = currRect.Top;
                                }
                                var arrangeRect = (element as ChartLegend).ArrangeRect;
                                ArrangeElement(element, LegendPlacement.Top, new Rect(arrangeRect.Left + offsetX + m_controlsThickness.Left, arrangeRect.Top + offsetY + legendTop, arrangeRect.Width, arrangeRect.Height));

                            }
                            else
                            {
                                ArrangeElement(element, LegendPlacement.Top, new Rect(0, currRect.Top, finalSize.Width, elemSize.Height));
                                currRect.Y += elemSize.Height;
                                scale = currRect.Height - elemSize.Height;
                                currRect.Height = scale > 0 ? scale : 0;
                            }
                            break;

                        case LegendPlacement.Bottom:
                            if (element is ChartLegend)
                            {
                                var arrangeRect = (element as ChartLegend).ArrangeRect;
                                ArrangeElement(element, LegendPlacement.Bottom, new Rect(arrangeRect.Left + offsetX + m_controlsThickness.Left, arrangeRect.Top + offsetY + m_controlsThickness.Top, arrangeRect.Width, arrangeRect.Height));
                            }
                            else
                            {
                                scale = currRect.Height - elemSize.Height;
                                currRect.Height = scale > 0 ? scale : 0;
                                ArrangeElement(element, LegendPlacement.Bottom, new Rect(0, currRect.Bottom, finalSize.Width, elemSize.Height));
                            }
                            break;
                    }
                    #endregion
                }
            }
            #endregion

            return base.ArrangeOverride(finalSize);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// LastChildFillProperty property changed handler.
        /// </summary>
        /// <param name="d">DockPanel that changed its LastChildFill.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnLastChildFillPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            LegendPanel source = d as LegendPanel;
            source.InvalidateArrange();
        }

        /// <summary>
        /// DockProperty property changed handler.
        /// </summary>
        /// <param name="d">UIElement that changed its ChartDock.</param>
        /// <param name="e">Event arguments.</param>
        private static void OnDockPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue.Equals(e.OldValue)) return;

            // Ignore the change if requested
            if (_ignorePropertyChange)
            {
                _ignorePropertyChange = false;
                return;
            }

            UIElement element = (UIElement)d;

            // Cause the DockPanel to update its layout when a child changes
            LegendPanel panel = VisualTreeHelper.GetParent(element) as LegendPanel;
            if (panel != null)
            {
                panel.InvalidateMeasure();
            }
        }

        /// <summary>
        /// Called when root element is changed.
        /// </summary>
        /// <param name="dpObj">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnAreaPanelChanged(DependencyObject dpObj, DependencyPropertyChangedEventArgs e)
        {
            LegendPanel dockPanel = dpObj as LegendPanel;

            if (dockPanel != null)
            {
                if (e.OldValue != null)
                {
                    dockPanel.Children.Remove(e.OldValue as UIElement);
                    dockPanel.areaPanel = null;
                }

                if (e.NewValue != null)
                {
                    dockPanel.areaPanel = e.NewValue as UIElement;
                    dockPanel.Children.Add(e.NewValue as UIElement);
                }
            }
        }

        /// <summary>
        /// Ensures the rectangle is inside specified bounds.
        /// </summary>
        /// <param name="bounds">The bounds.</param>
        /// <param name="rect">The rectangle.</param>
        /// <returns>Returns the Rectangle</returns>
        private static Rect EnsureRectIsInside(Rect bounds, Rect rect)
        {
            if (rect.Bottom > bounds.Bottom)
            {
                rect.Y -= rect.Bottom - bounds.Bottom;
            }

            if (rect.Right > bounds.Right)
            {
                rect.X -= rect.Right - bounds.Right;
            }

            if (rect.Top < bounds.Top)
            {
                rect.Y -= rect.Top - bounds.Top;
            }

            if (rect.Left < bounds.Left)
            {
                rect.X -= rect.Left - bounds.Left;
            }

            return rect;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Invalidates the layout when parent grid size changed.
        /// </summary>
        /// <param name="sender">The Sender Object</param>
        /// <param name="e">The Event Arguments</param>
        private void parentgrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.InvalidateMeasure();
            this.InvalidateArrange();
        }

        /// <summary>
        /// Arranges the elements inside the passing element.
        /// </summary>
        /// <param name="element">The Element</param>
        /// <param name="dock">The Dock Position</param>
        /// <param name="rect">The Reference Size <see cref="Rect"/></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        private void ArrangeElement(UIElement element, LegendPlacement dock, Rect rect)
        {
            element.Arrange(ChartLayoutUtils.Subtractthickness(rect, ElementMargin));
        }

        #endregion

        #endregion
    }
}
