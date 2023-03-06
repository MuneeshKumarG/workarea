using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace Syncfusion.UI.Xaml.Charts
{
	internal class PolarPlotArea : ChartPlotArea
	{
        #region Properties

        internal PolarSeriesCollection SeriesCollection { get; set; }

        #endregion

        #region Methods

        internal override void UpdateLegendItemsSource()
        {
            if (SeriesCollection == null || Legend == null || !Legend.IsVisible)
            {
                return;
            }

            LegendItems.Clear();
            int index = 0;

            foreach (PolarSeries series in SeriesCollection)
            {
                if (series.IsVisibleOnLegend)
                {
                    var legendItem = new PolarLegendItem();
                    SetBinding(legendItem, LegendItem.TextProperty, series, new PropertyPath(nameof(series.Label)));
                    SetBinding(legendItem, LegendItem.IsToggledProperty, series, new PropertyPath(nameof(series.IsSeriesVisible)));
                    legendItem.Series = series;
                    legendItem.Index = index;
                    BindLegendItemProperties(legendItem, series);
                    LegendItems?.Add(legendItem);
                    index++;
                }
            }

            ShouldPopulateLegendItems = false;
        }

        internal override void Dispose()
        {
            base.Dispose();

            SeriesCollection?.Clear();
            SeriesCollection = null;
        }

        #endregion
    }
}
