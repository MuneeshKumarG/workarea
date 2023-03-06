namespace Syncfusion.UI.Xaml.Charts
{
    using Windows.Foundation;

    /// <summary>
    /// Represents the panel where all the child elements of Chart will be arranged.
    /// </summary>
    internal class PyramidAreaPanel : AreaPanel
    {
        internal PyramidSeries PyramidSeries { get; set; }

        /// <summary>
        /// Gets or sets the pyramid chart.
        /// </summary>
        internal SfPyramidChart Chart { get; set; }

        internal override void UpdateArea(bool forceUpdate)
        {
            if (IsUpdateDispatched || forceUpdate)
            {
                if (Chart == null || PyramidSeries == null)
                    return;

                if (PyramidSeries.EnableTooltip)
                    Chart.ActualEnableTooltip = true;

                //Add selected index while loading 
                if (!Chart.IsChartLoaded)
                {
                    DataPointSelectionBehavior selectionBehavior = PyramidSeries.SelectionBehavior;
                    if (PyramidSeries.GetEnableSegmentSelection() && (selectionBehavior.SelectedIndex >= 0 ||
                        (selectionBehavior.SelectedIndexes != null && selectionBehavior.SelectedIndexes.Count > 0)))
                    {
                        selectionBehavior.UpdateSelectedIndexSelection(PyramidSeries);
                    }
                }

                PyramidSeries.Invalidate();

                if (Chart.ActualEnableTooltip && Chart.Tooltip == null)
                    Chart.Tooltip = new ChartTooltip();

                if (Chart.PlotArea != null && Chart.PlotArea.ShouldPopulateLegendItems)
                {
                    Chart.PlotArea.PopulateLegendItems();
                    Chart.PlotArea.ShouldPopulateLegendItems = false;
                }

                PyramidSeries.UpdateRange();

                if (Chart.RootPanelDesiredSize != null)
                {
                    if (!Chart.IsChartLoaded)
                    {
                        Chart.ScheduleRenderSeries();
                        Chart.IsChartLoaded = true;
                    }
                    else if (!Chart.isRenderSeriesDispatched)
                    {
                        Chart.RenderSeries();
                    }
                }

                IsUpdateDispatched = false;

                if (Chart.Behaviors != null)
                {
                    foreach (var behavior in Chart.Behaviors)
                    {
                        behavior.OnLayoutUpdated();
                    }
                }
            }
        }

        /// <summary>
        /// Provides the behavior for the Measure pass of Silverlight layout. Classes can override this method to define their own Measure pass behavior.
        /// </summary>
        /// <returns>
        /// The size that this object determines it needs during layout, based on its calculations of the allocated sizes for child objects; or based on other considerations, such as a fixed container size.
        /// </returns>
        /// <param name="availableSize">The Available Size</param>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = ChartLayoutUtils.CheckSize(availableSize);
            if (Chart != null)
            {
                Chart.RootPanelDesiredSize = size;
            }

            return base.MeasureOverride(size);
        }

        internal override void Dispose()
        {
            Chart = null;
            PyramidSeries = null;
        }
    }
}
