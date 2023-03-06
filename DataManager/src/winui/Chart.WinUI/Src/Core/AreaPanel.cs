namespace Syncfusion.UI.Xaml.Charts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Windows.ApplicationModel;
    using Windows.Foundation;

    /// <summary>
    /// Represents the panel where all the child elements of Chart will be arranged.
    /// </summary>
    [Browsable(false)]
    public class AreaPanel : Panel
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for MeasurePriorityIndex property.
        /// </summary>
        public static readonly DependencyProperty MeasurePriorityIndexProperty =
            DependencyProperty.RegisterAttached(
                "MeasurePriorityIndex",
                typeof(int),
                typeof(AreaPanel),
                new PropertyMetadata(0));

        #endregion

        #region Fields

        internal bool IsUpdateDispatched = false;

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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                Children[i].Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
            }

            return finalSize;
        }

        internal void ScheduleUpdate()
        {
            var _isInDesignMode = DesignMode.DesignModeEnabled;

            if (!IsUpdateDispatched && !_isInDesignMode)
            {
                DispatcherQueue.TryEnqueue(() => { UpdateArea(); });
                IsUpdateDispatched = true;
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

        internal virtual void Dispose()
        {

        }

        #endregion
    }
}
