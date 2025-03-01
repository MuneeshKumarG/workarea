﻿// <copyright file="ChartRootPanel.cs" company="Syncfusion. Inc">
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
    using Syncfusion.UI.Xaml.Charts;
    using Windows.ApplicationModel;
    using Windows.Foundation;

    /// <summary>
    /// Represents the panel where all the child elements of Chart will be arranged.
    /// </summary>
    public class AreaPanel : Panel
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for MeasurePriorityIndex property.
        /// </summary>
        public static readonly DependencyProperty MeasurePriorityIndexProperty =
            DependencyProperty.RegisterAttached("MeasurePriorityIndex", typeof(int), typeof(AreaPanel), new PropertyMetadata(0));

        #endregion

        #region Fields

        internal bool isUpdateDispatched = false;

        #endregion

        #region Methods

        /// <summary>
        /// Gets measure priority for this obj.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetMeasurePriorityIndex(DependencyObject obj)
        {
            return (int)obj.GetValue(MeasurePriorityIndexProperty);
        }

        /// <summary>
        /// Sets the measure priority for this obj.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetMeasurePriorityIndex(DependencyObject obj, int value)
        {
            obj.SetValue(MeasurePriorityIndexProperty, value);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Provides the behavior for the Measure pass of Silverlight layout. Classes can override this method to define their own Measure pass behavior.
        /// </summary>
        /// <returns>
        /// The size that this object determines it needs during layout, based on its calculations of the allocated sizes for child objects; or based on other considerations, such as a fixed container size.
        /// </returns>
        /// <param name="availableSize">The Available Size</param>
        protected override Size MeasureOverride(Size availableSize)
        {
            var elements = new List<UIElement>();

            Size size = ChartLayoutUtils.CheckSize(availableSize);

            foreach (UIElement element in Children)
            {
                elements.Add(element);
            }

            IEnumerable<UIElement> uiElements = elements.OrderBy(GetMeasurePriorityIndex);

            foreach (UIElement element in uiElements)
            {
                element.Measure(availableSize);
            }

            return size;
        }

        /// <summary>
        /// Provides the behavior for the Arrange pass of Silverlight layout. Classes can override this method to define their own Arrange pass behavior.
        /// </summary>
        /// <returns>
        /// The actual size that is used after the element is arranged in layout.
        /// </returns>
        /// <param name="finalSize">The final area within the parent that this object should use to arrange itself and its children.</param>
        protected override Size ArrangeOverride(Size finalSize)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                Children[i].Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            }

            ScheduleUpdate();

            return finalSize;
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

        }

        #endregion
    }
}
