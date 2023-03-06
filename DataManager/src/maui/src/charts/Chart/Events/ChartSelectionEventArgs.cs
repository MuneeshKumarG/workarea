using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Provides data for the chart <b>SelectionChanged</b> event.
    /// </summary>
    /// <remarks>This class contains information about the new and old selected items.</remarks>
    public class ChartSelectionChangedEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        /// Gets the indexes of the newly selected items.
        /// </summary>
        public List<int> NewIndexes { get; internal set; }

        /// <summary>
        /// Gets the indexes of the previously selected items.
        /// </summary>
        public List<int> OldIndexes { get; internal set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSelectionChangedEventArgs"/> class.
        /// </summary>
        public ChartSelectionChangedEventArgs()
        {
            NewIndexes = new List<int>();
            OldIndexes = new List<int>();
        }
        #endregion
    }

    /// <summary>
    /// Provides data for the chart <b>SelectionChanging</b> event.
    /// </summary>
    /// <remarks>This class contains information about the new and old selected items and allows to cancel the selection change.</remarks>
    public class ChartSelectionChangingEventArgs : CancelEventArgs
    {
        #region Public Properties

        /// <summary>
        /// Gets the indexes of the newly selected items.
        /// </summary>
        public List<int> NewIndexes { get; internal set; }

        /// <summary>
        /// Gets the indexes of the previously selected items.
        /// </summary>
        public List<int> OldIndexes { get; internal set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartSelectionChangingEventArgs"/> class.
        /// </summary>
        public ChartSelectionChangingEventArgs()
        {
            NewIndexes = new List<int>();
            OldIndexes = new List<int>();
        }

        #endregion

    }
}
