
using System.Linq;

namespace Syncfusion.UI.Xaml.Charts
{
	internal class FunnelPlotArea : ChartPlotArea
	{
        #region Properties

        internal FunnelSeries FunnelSeries { get; set; }

        #endregion

        #region Methods

        internal override void UpdateLegendItemsSource()
        {
            if (FunnelSeries == null || Legend == null || !Legend.IsVisible)
            {
                return;
            }

            LegendItems.Clear();
            int index = 0;

            if (FunnelSeries.IsVisibleOnLegend)
            {
                if (FunnelSeries.Segments.Count == 0)
                    return;

                foreach (var segment in FunnelSeries.Segments)
                {
                    var legendItem = new FunnelLegendItem();
                    BindLegendItemProperties(legendItem, FunnelSeries, segment);
                    legendItem.Series = FunnelSeries;
                    legendItem.Segment = segment;
                    legendItem.Index = index;
                    legendItem.Item = segment.Item;
                    legendItem.Text = FunnelSeries.GetActualXValue(index)?.ToString() ?? string.Empty;
                    LegendItems?.Add(legendItem);
                    index++;
                }
            }

            ShouldPopulateLegendItems = false;
        }

        internal override void Dispose()
        {
            base.Dispose();

            FunnelSeries = null;
        }
        #endregion
    }
}
