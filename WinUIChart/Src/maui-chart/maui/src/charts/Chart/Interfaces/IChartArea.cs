using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    internal interface IChartArea
    {
        public ChartSeriesCollection? Series { get; set; }

        public ReadOnlyObservableCollection<ChartSeries>? VisibleSeries { get; }

        public void UpdateVisibleSeries()
        {

        }
    }
}
