using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartAxisLabelEventArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartAxisLabelEventArgs(string? labelContent, double position)
        {
            Label = labelContent;
            Position = position;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Position { get; internal set; }

        /// <summary>
        ///
        /// </summary>
        public ChartAxisLabelStyle? LabelStyle { get; set; }
        #endregion
    }
}
