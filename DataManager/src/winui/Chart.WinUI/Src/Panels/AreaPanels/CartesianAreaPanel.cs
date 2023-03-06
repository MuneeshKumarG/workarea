namespace Syncfusion.UI.Xaml.Charts
{
using Microsoft.UI.Xaml.Controls;
    using Windows.Foundation;

    /// <summary>
    /// Represents the panel where all the child elements of Chart will be arranged.
    /// </summary>
    internal class CartesianAreaPanel : AreaPanel
    {
        /// <summary>
        /// Gets or sets the cartessian chart.
        /// </summary>
        internal SfCartesianChart Chart { get; set; }

        internal override void UpdateArea(bool forceUpdate)
        {
            if (Chart == null || Chart.VisibleSeries == null)
                return;

            if (IsUpdateDispatched || forceUpdate)
            {
                if (Chart.ColumnDefinitions.Count == 0)
                    Chart.ColumnDefinitions.Add(new ChartColumnDefinition());
                if (Chart.RowDefinitions.Count == 0)
                    Chart.RowDefinitions.Add(new ChartRowDefinition());

                foreach (ChartSeries series in Chart.VisibleSeries)
                {
                    if (series.EnableTooltip)
                        Chart.ActualEnableTooltip = true;
                }


                foreach (ChartSeries series in Chart.VisibleSeries)
                {
                    CartesianSeries cartesianSeries = series as CartesianSeries;
                    if (cartesianSeries.ActualXAxis == Chart.InternalPrimaryAxis && Chart.InternalPrimaryAxis != null
                        && !Chart.InternalPrimaryAxis.RegisteredSeries.Contains(cartesianSeries))
                        Chart.InternalPrimaryAxis.RegisteredSeries.Add(cartesianSeries);
                    if (cartesianSeries.ActualYAxis == Chart.InternalSecondaryAxis && Chart.InternalSecondaryAxis != null
                        && !Chart.InternalSecondaryAxis.RegisteredSeries.Contains(cartesianSeries))
                        Chart.InternalSecondaryAxis.RegisteredSeries.Add(cartesianSeries);
                }

                var seriesCollection = Chart.GetSeriesCollection();
                if (seriesCollection != null && seriesCollection.Count > 0)
                {
                    if (Chart.InternalPrimaryAxis == null || !Chart.InternalAxes.Contains(Chart.InternalPrimaryAxis))
                        Chart.InternalPrimaryAxis = (seriesCollection[0] as ChartSeries).ActualXAxis;
                    if (Chart.InternalSecondaryAxis == null || !Chart.InternalAxes.Contains(Chart.InternalSecondaryAxis))
                        Chart.InternalSecondaryAxis = (seriesCollection[0] as ChartSeries).ActualYAxis;
                }

                //Add selected index while loading 
                if (!Chart.IsChartLoaded)
                {
                    var seriesSelection = Chart.GetSeriesSelectionBehavior();
                    if (Chart.GetEnableSeriesSelection() && (seriesSelection.SelectedIndex >= 0 ||
                        (seriesSelection.SelectedIndexes != null && seriesSelection.SelectedIndexes.Count > 0)))
                    {
                        seriesSelection.UpdateSelectedIndexSelection();

                        //Raise the SelectionChanged event when SelectedIndex or SelectedIndexes is set at chart load time.
                        if (Chart.VisibleSeries.Count > 0)
                            Chart.RaiseSeriesSelectionChangedEvent();
                    }
                    else
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
                }

                if ((Chart.InternalPrimaryAxis is CategoryAxis) && (!(Chart.InternalPrimaryAxis as CategoryAxis).ArrangeByIndex))
                    (Chart.InternalPrimaryAxis as CategoryAxis).GroupData(Chart.VisibleSeries);

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
                    Chart.LayoutAxis(Chart.RootPanelDesiredSize.Value);

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
