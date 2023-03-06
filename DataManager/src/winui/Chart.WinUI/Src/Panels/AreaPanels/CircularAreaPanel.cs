namespace Syncfusion.UI.Xaml.Charts
{
    using Windows.Foundation;

    /// <summary>
    /// Represents the panel where all the child elements of Chart will be arranged.
    /// </summary>
    internal class CircularAreaPanel : AreaPanel
    {
        /// <summary>
        /// Gets or sets the circular chart.
        /// </summary>
        internal SfCircularChart Chart { get; set; }

        internal override void UpdateArea(bool forceUpdate)
        {
            if (Chart == null || Chart.VisibleSeries == null)
                return;

            if (IsUpdateDispatched || forceUpdate)
            {
                foreach (ChartSeries series in Chart.VisibleSeries)
                {
                    if (series.EnableTooltip)
                        Chart.ActualEnableTooltip = true;
                }

                //Add selected index while loading 
                if (!Chart.IsChartLoaded)
                {
                    foreach (var series in Chart.VisibleSeries)
                    {
                        if (series != null)
                        {
                            DataPointSelectionBehavior selectionBehavior = series.SelectionBehavior;
                            if (series.GetEnableSegmentSelection() && (selectionBehavior.SelectedIndex >= 0 ||
                                (selectionBehavior.SelectedIndexes != null && selectionBehavior.SelectedIndexes.Count > 0)))
                            {
                                selectionBehavior.UpdateSelectedIndexSelection(series);
                            }
                        }
                    }
                }

                foreach (ChartSeries series in Chart.VisibleSeries)
                {
                    series.Invalidate();
                }

                if (Chart.ActualEnableTooltip && Chart.Tooltip == null)
                    Chart.Tooltip = new ChartTooltip();

                if (Chart.PlotArea != null && Chart.PlotArea.ShouldPopulateLegendItems)
                {
                    Chart.PlotArea.PopulateLegendItems();
                    Chart.PlotArea.ShouldPopulateLegendItems = false;
                }

                foreach (ChartSeries series in Chart.VisibleSeries)
                {
                    series.UpdateRange();
                }

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
        }
    }
}
