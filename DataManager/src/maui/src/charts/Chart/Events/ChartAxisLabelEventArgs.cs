using System;

#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
    /// <summary>
    /// Provides data for the chart axis <b>LabelCreated</b> event.
    /// </summary>
    /// <remarks>This class contains information about the text, position, and label style of the chart axis label.</remarks>
    public class ChartAxisLabelEventArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the <see cref="ChartAxisLabelEventArgs"/> class.
        /// </summary>
        public ChartAxisLabelEventArgs(string? labelContent, double position)
        {
            Label = labelContent;
            Position = position;
        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets a string value that displays on the chart axis label.
        /// </summary>
        public string? Label { get; set; }

        /// <summary>
        /// Gets a value for the chart axis label's position.
        /// </summary>
        public double Position { get; internal set; }

        /// <summary>
        /// Gets or sets the value to customize the appearance of chart axis labels. 
        /// </summary>
        /// <value>It accepts the <see cref="Charts.ChartAxisLabelStyle"/> value.</value>
#if WinUI
        public LabelStyle LabelStyle { get; set; }
#else
        public ChartAxisLabelStyle? LabelStyle { get; set; }
#endif
        #endregion
    }
}
