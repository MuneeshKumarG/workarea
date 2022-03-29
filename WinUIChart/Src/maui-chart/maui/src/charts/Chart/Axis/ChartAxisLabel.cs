
namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartAxisLabel
    {
        #region Fields
        private bool isVisible = true;
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public object? Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Position { get; internal set; }

        internal bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ChartAxisLabelStyle? LabelStyle { get; set; }

        internal double RotateOriginX { get; set; }

        internal double RotateOriginY { get; set; }
        
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartAxisLabel(double position, object? labelContent)
        {
            this.Position = position;
            this.Content = labelContent;
        }

        #endregion
    }

}
