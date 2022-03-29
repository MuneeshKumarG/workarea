using System.Collections.ObjectModel;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartSeriesCollection : ObservableCollection<ChartSeries>
    {
        private readonly ReadOnlyObservableCollection<ChartSeries> readOnlyItems;

        /// <summary>
        /// 
        /// </summary>
        public ChartSeriesCollection()
        {
            readOnlyItems = new ReadOnlyObservableCollection<ChartSeries>(this);
        }

        internal ReadOnlyObservableCollection<ChartSeries> GetVisibleSeries()
        {
            //TODO:Need to check the alternative solution.
            return new ReadOnlyObservableCollection<ChartSeries>(new ObservableCollection<ChartSeries>(from chartSeries in this
                                                                                                     where chartSeries.IsVisible
                                                                                                     select chartSeries));
        }

        internal ReadOnlyObservableCollection<ChartSeries> AsReadOnly()
        {
            return readOnlyItems;
        }
    }
}
