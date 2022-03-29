using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartSelectionChangedEventArgs : EventArgs
    {
        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public int PreviousIndex { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentIndex { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ChartSeries? Series { get; internal set; }

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public ChartSelectionChangedEventArgs()
        {
            CurrentIndex = -1;
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChartSelectionChangingEventArgs : ChartSelectionChangedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Cancel { get; set; }
    }
}
