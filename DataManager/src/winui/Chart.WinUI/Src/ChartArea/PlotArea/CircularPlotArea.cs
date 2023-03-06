using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace Syncfusion.UI.Xaml.Charts
{
	internal class CircularPlotArea : ChartPlotArea
	{
        #region Properties
        
        internal CircularSeriesCollection SeriesCollection { get; set; }

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

            if (SeriesCollection.Count > 1)
            {
                foreach (CircularSeries series in SeriesCollection)
                {
                    if (series.IsVisibleOnLegend)
                    {
                        var legendItem = new CircularLegendItem();
                        SetBinding(legendItem, LegendItem.TextProperty, series, new PropertyPath(nameof(series.Label)));
                        SetBinding(legendItem, LegendItem.IsToggledProperty, series, new PropertyPath(nameof(series.IsSeriesVisible)));
                        legendItem.Series = series;
                        legendItem.Index = index;
                        BindLegendItemProperties(legendItem, series);
                        LegendItems?.Add(legendItem);
                        index++;
                    }
                }
            }
            else if (SeriesCollection.Count == 1)
            {
                ChartSeries chartSeries = SeriesCollection[0];
                if (chartSeries.IsVisibleOnLegend)
                {
                    if (chartSeries.Segments.Count == 0)
                        return;

                    foreach (var segment in chartSeries.Segments)
                    {
                        var legendItem = new CircularLegendItem();
                        legendItem.Series = chartSeries as CircularSeries;
                        legendItem.Segment = segment;
                        legendItem.Index = index;
                        legendItem.Item = segment.Item;
                        legendItem.Text = chartSeries.GetActualXValue(index)?.ToString() ?? string.Empty;
                        BindLegendItemProperties(legendItem, chartSeries);
                        LegendItems?.Add(legendItem);
                        index++;
                    }
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
