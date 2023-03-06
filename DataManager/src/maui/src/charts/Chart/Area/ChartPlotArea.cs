using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Core;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Syncfusion.Maui.Core.Internals;

namespace Syncfusion.Maui.Charts
{
    internal class ChartPlotArea : AbsoluteLayout, IPlotArea
    {
        #region Fields
        internal readonly AbsoluteLayout SeriesViews;
        internal readonly DataLabelView DataLabelView;
        internal readonly ObservableCollection<ILegendItem> legendItems;
        private ChartSeriesCollection? series;
        internal IChart? Chart;
        internal ILegend? legend;
        private View? plotAreaBackgroundView;
        private bool shouldPopulateLegendItems = true;
        private EventHandler<EventArgs>? legendItemsUpdated;
        private EventHandler<LegendItemEventArgs>? legendItemsToggled;
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartPlotArea()
        {
            BatchBegin();
            series = new ChartSeriesCollection();
            legendItems = new ObservableCollection<ILegendItem>();
            SeriesViews = new AbsoluteLayout();
            DataLabelView = new DataLabelView(this);
            AbsoluteLayout.SetLayoutBounds(SeriesViews, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(SeriesViews, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(DataLabelView, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(DataLabelView, AbsoluteLayoutFlags.All);
            Add(SeriesViews);
            Add(DataLabelView);
            BatchCommit();
        }


        #endregion

        #region Properties

        public ChartSeriesCollection? Series
        {
            get
            {
                return series;
            }
            set
            {
                if (value != series)
                {
                    OnSeriesCollectionChanging();
                    series = value;
                    OnSeriesCollectionChanged();

                }
            }
        }

        public Rect PlotAreaBounds { get; set; }

        public bool ShouldPopulateLegendItems 
        {
            get
            {
              return shouldPopulateLegendItems;
            }
            set
            {
                shouldPopulateLegendItems = value;
            }
        }

        internal View? PlotAreaBackgroundView
        {
            get
            {
                return plotAreaBackgroundView;
            }
            set
            {
                if (plotAreaBackgroundView != null && Contains(plotAreaBackgroundView))
                {
                    plotAreaBackgroundView.RemoveBinding(AbsoluteLayout.LayoutBoundsProperty);
                    plotAreaBackgroundView.RemoveBinding(AbsoluteLayout.LayoutFlagsProperty);
                    Remove(plotAreaBackgroundView);
                }

                if (value != null)
                {
                    plotAreaBackgroundView = value;
                    AbsoluteLayout.SetLayoutBounds(plotAreaBackgroundView, new Rect(0, 0, 1, 1));
                    AbsoluteLayout.SetLayoutFlags(plotAreaBackgroundView, AbsoluteLayoutFlags.All);
                    Insert(0, plotAreaBackgroundView);
                }
            }
        }

        public ReadOnlyObservableCollection<ILegendItem> LegendItems => new ReadOnlyObservableCollection<ILegendItem>(legendItems);

        public ReadOnlyObservableCollection<ChartSeries>? VisibleSeries => series?.GetVisibleSeries();

        ILegend? IPlotArea.Legend
        {
            get
            {
                return legend;
            }
            set
            {
                if (value != legend)
                {
                    legend = value;
                }
            }
        }

        LegendHandler IPlotArea.LegendItemToggleHandler
        {
            get
            {
                return ToggleLegendItem;
            }
        }
       
        event EventHandler<EventArgs> IPlotArea.LegendItemsUpdated { add { legendItemsUpdated += value; } remove { legendItemsUpdated -= value; } }
        event EventHandler<LegendItemEventArgs>? IPlotArea.LegendItemToggled { add { legendItemsToggled += value; } remove { legendItemsToggled -= value; } }

        #endregion

        #region Methods

        #region Public Methods 

        public void UpdateLegendItems()
        {
            if (shouldPopulateLegendItems)
            {
                UpdateLegendItemsSource();
                shouldPopulateLegendItems = false;
                legendItemsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        public void UpdateVisibleSeries()
        {
            foreach (SeriesView seriesView in SeriesViews.Children)
            {
                if (seriesView != null && seriesView.IsVisible)
                {
                    seriesView.UpdateSeries();
                }
            }

            DataLabelView?.InvalidateDrawable();
        }

        internal void PopulateDataManager()
        {
            if (Chart?.DataManager != null && !Chart.IsDataPopulated)
            {
                Chart.DataManager.PopulateData();
                Chart.IsDataPopulated = true;
            }
        }
        #endregion

        #region Protected Methods

        protected virtual void UpdateLegendItemsSource()
        {

        }

        #endregion

        #region Series CollectionChanged Methods

        internal virtual void AddSeries(int index, object series)
        {
            var chartSeries = (ChartSeries) series;

            if (chartSeries != null)
            {
                chartSeries.Chart = Chart;
                SetInheritedBindingContext(chartSeries, this.BindingContext);
                chartSeries.SegmentsCreated = false;
                var seriesView = new SeriesView(chartSeries, this);
                AbsoluteLayout.SetLayoutBounds(seriesView, new Rect(0, 0, 1, 1));
                AbsoluteLayout.SetLayoutFlags(seriesView, AbsoluteLayoutFlags.All);
                SeriesViews.Insert(index, seriesView);
                chartSeries.OnAttachedToChart();
            }
        }

        internal virtual void RemoveSeries(int index, object series)
        {
            var chartSeries = (ChartSeries)series;

            if (chartSeries != null)
            {
                chartSeries.ResetData();
                chartSeries.SegmentsCreated = false;
                SeriesViews.Children.RemoveAt(index);
                chartSeries.OnDetachedToChart();    
                chartSeries.Chart = null;
            }
        }

        private void OnSeriesCollectionChanging()
        {
            if (series != null)
            {
                series.CollectionChanged -= Series_CollectionChanged;
                ResetSeries();
            }
        }

        private void OnSeriesCollectionChanged()
        {
            if (series != null)
            {
                series.CollectionChanged += Series_CollectionChanged;

                int count = 0;
                foreach (var chartSeries in series)
                {
                    //Todo:Validate series before add into the collection. its cartesian or circular chart.
                    AddSeries(count, chartSeries);
                    count++;
                }

                if (legend != null)
                    UpdateLegendItems();

            }
        }

        private void Series_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((index, series) => AddSeries(index, series), (index, series) => RemoveSeries(index, series), ResetSeries);
            ShouldPopulateLegendItems = true;

            var area = Chart?.Area;
            if (area != null)
            {
                area.NeedsRelayout = true;
                area.ScheduleUpdateArea();
            }
        }

        private void ResetSeries()
        {
            //Todo:Need to clear the ChartSeries ResetData method and ensure the dynamic changes
            foreach (SeriesView seriesView in SeriesViews.Children)
            {
                seriesView.Series.ResetData();
            }

            SeriesViews.Clear();
        }

        #endregion

        #region Private Methods

        void ToggleLegendItem(ILegendItem legendItem)
        {
            if (legend != null && legend.IsVisible && legend.ToggleVisibility)
            {
                var cartesianLegendItem = legendItem as LegendItem;

                if (cartesianLegendItem != null && cartesianLegendItem.Item != null)
                {
                    //Todo:Need to change ShouldLegendRefresh in Area class
                    //chart.RequiredLegendRefresh = false;
                    //Todo: Need to invalidate the series creation and drawing for dynamic changes.
                    if(cartesianLegendItem.Item is CartesianSeries series)
                    {
                        series.IsVisible = !legendItem.IsToggled;
                    }                      
                }
            }
        }

        #endregion

        #endregion
    }
}
