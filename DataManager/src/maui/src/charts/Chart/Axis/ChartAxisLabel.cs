#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
    /// <summary>
    /// Represents the content and label style for the axis label.
    /// </summary>
    public class ChartAxisLabel
    {
        #region Fields
        private bool isVisible = true;
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the axis label text.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Gets the axis label position.
        /// </summary>
        public double Position { get; internal set; }

        internal bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }


        /// <summary>
        /// Gets or sets the axis label style to customize the label appearance.
        /// </summary>
#if WinUI
        public LabelStyle LabelStyle { get; set; }
#else
        public ChartAxisLabelStyle? LabelStyle { get; set; }
#endif

        internal double RotateOriginX { get; set; }

        internal double RotateOriginY { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartAxisLabel"/> class.
        /// </summary>
        public ChartAxisLabel(double position, object labelContent)
        {
            this.Position = position;
            this.Content = labelContent;
        }

        #endregion
    }

}
