using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// A collection class which holds ChartBehaviors.
    /// </summary>
    internal class ChartBehaviorsCollection : ObservableCollection<ChartBehavior>
    {
        internal ChartBase Area
        {
            get;
            set;
        }

        /// <summary>
        /// Called when instance created for ChartBehaviourCollection
        /// </summary>
        /// <param name="area"></param>
        public ChartBehaviorsCollection(ChartBase area)
        {
            Area = area;
        }

        /// <summary>
        /// Called when instance created for ChartBehaviorsCollection
        /// </summary>
        public ChartBehaviorsCollection()
        {

        }

        /// <inheritdoc/>
        protected override void InsertItem(int index, ChartBehavior item)
        {
            item.Chart = Area;
            item.AdorningCanvas = Area.GetAdorningCanvas();
            if (item.AdorningCanvas != null)
                item.InternalAttachElements();
            base.InsertItem(index, item);
        }

        /// <inheritdoc/>
        protected override void RemoveItem(int index)
        {
            var item = this.Items[index];            
            item.DetachElements();
            item.Chart = Area;
            base.RemoveItem(index);
        }

        /// <inheritdoc/>
        protected override void ClearItems()
        {
            foreach (ChartBehavior behavior in Items)
            {
                behavior.DetachElements();
                behavior.Chart = Area;
            }

            base.ClearItems();
        }
    }

    /// <summary>
    /// Represents a collection of <see cref="ChartAxisLabel"/>.
    /// </summary>
    internal class ChartAxisLabelCollection : ObservableCollection<ChartAxisLabel>
    {
        /// <inheritdoc/>
        protected override void InsertItem(int index, ChartAxisLabel item)
        {
            base.InsertItem(index, item);
        }

        /// <inheritdoc/>
        protected override void ClearItems()
        {
            base.ClearItems();
        }
    }
}
