
namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a chart segment which renders collection of points using bar shape.
    /// </summary>
    /// <seealso cref="StackedBarSeries"/>   
    public class StackedBarSegment : BarSegment
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedBarSegment"/>. 
        /// </summary>
        public StackedBarSegment()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedBarSegment"/>.
        /// </summary>
        /// <param name="x1">Used to specify the segment x1 value.</param>
        /// <param name="y1">Used to specify the segment y1 value.</param>
        /// <param name="x2">Used to specify the segment x2 value.</param>
        /// <param name="y2">Used to specify the segment y2 value.</param>
        /// <param name="series">Used to specify the <see cref="StackedBarSeries"/> instance.</param>
        public StackedBarSegment(double x1,double y1,double x2,double y2,StackedBarSeries series):base(x1,y1,x2,y2)
        {
            base.Series = series;
            customTemplate = series.CustomTemplate;
        }

        #endregion
    }
}
