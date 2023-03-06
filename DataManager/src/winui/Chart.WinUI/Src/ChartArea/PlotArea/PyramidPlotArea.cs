
namespace Syncfusion.UI.Xaml.Charts
{
	internal class PyramidPlotArea : ChartPlotArea
	{
        #region Properties

        internal PyramidSeries PyramidSeries { get; set; }

        #endregion

        #region Methods

        internal override void UpdateLegendItemsSource()
        {
            if (PyramidSeries == null || Legend == null || !Legend.IsVisible)
            {
                return;
            }

            LegendItems.Clear();
            int index = 0;

            if (PyramidSeries.IsVisibleOnLegend)
            {
                if (PyramidSeries.Segments.Count == 0)
                    return;

                foreach (var segment in PyramidSeries.Segments)
                {
                    var legendItem = new PyramidLegendItem();
                    BindLegendItemProperties(legendItem, PyramidSeries, segment);
                    legendItem.Series = PyramidSeries;
                    legendItem.Segment = segment;
                    legendItem.Index = index;
                    legendItem.Item = segment.Item;
                    legendItem.Text = PyramidSeries.GetActualXValue(index)?.ToString() ?? string.Empty;
                    LegendItems?.Add(legendItem);
                    index++;
                }
            }

            ShouldPopulateLegendItems = false;
        }

        internal override void Dispose()
        {
            base.Dispose();

            PyramidSeries = null;
        }

        #endregion
    }
}
