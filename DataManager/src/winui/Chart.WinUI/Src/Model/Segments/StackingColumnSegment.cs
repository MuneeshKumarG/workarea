
namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class StackedColumnSegment : ColumnSegment
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public StackedColumnSegment()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="series"></param>
        public StackedColumnSegment(double x1, double y1, double x2, double y2, StackedColumnSeries series) : base(x1, y1, x2, y2)
        {
            base.Series = series;
            customTemplate = series.CustomTemplate;
        }

        #endregion

        #region Methods

        #region Public Override Methods

        /// <inheritdoc/>
        internal override void Update(IChartTransformer transformer)
        {
            base.Update(transformer);
        }

        #endregion

        #endregion
    }
}
