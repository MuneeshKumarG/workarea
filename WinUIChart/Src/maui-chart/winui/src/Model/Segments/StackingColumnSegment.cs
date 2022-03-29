
namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a chart segment which renders collection of points using column shape.
    /// </summary>
    /// <seealso cref="StackedColumnSeries"/>
    public class StackedColumnSegment : ColumnSegment
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedColumnSegment"/>. 
        /// </summary>
        public StackedColumnSegment()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedColumnSegment"/>.
        /// </summary>
        /// <param name="x1">Used to specify the segment x1 value.</param>
        /// <param name="y1">Used to specify the segment y1 value.</param>
        /// <param name="x2">Used to specify the segment x2 value.</param>
        /// <param name="y2">Used to specify the segment y2 value.</param>
        /// <param name="series">Used to specify the <see cref="StackedColumnSeries"/> instance.</param>
        public StackedColumnSegment(double x1, double y1, double x2, double y2, StackedColumnSeries series) : base(x1, y1, x2, y2)
        {
            base.Series = series;
            customTemplate = series.CustomTemplate;
        }

        #endregion

        #region Methods

        #region Public Override Methods

        /// <inheritdoc/>
        public override void Update(IChartTransformer transformer)
        {
            base.Update(transformer);
        }

        #endregion

        #endregion
    }
}
