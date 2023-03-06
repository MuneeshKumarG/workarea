using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Core.Internals;
using System;
using Syncfusion.Maui.Graphics.Internals;
using Syncfusion.Maui.Core;
using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Charts
{
    internal class PyramidChartArea : AreaBase, IPlotArea
    {
        #region Fields

        private readonly IPyramidChartDependent chart;
        private bool shouldPopulateLegendItems = true;
        private const double sizeRatio = 0.8;
        #endregion

        internal ILegend? legend;
        internal readonly PyramidChartView SeriesView;
        internal readonly PyramidDataLabelsView DataLabelView;
        internal readonly ObservableCollection<ILegendItem> legendItems;
        private EventHandler<EventArgs>? legendItemsUpdated;
        private EventHandler<LegendItemEventArgs>? legendItemsToggled;
        private View? plotAreaBackgroundView;

        #region Constructor

        /// <summary>
        /// It helps to create chart area to hold the view for pyramid and funnel charts.
        /// </summary>
        /// <param name="chart"></param>
        public PyramidChartArea(IPyramidChartDependent chart)
        {
            BatchBegin();
            this.chart = chart;
            legendItems = new ObservableCollection<ILegendItem>();
            SeriesView = new PyramidChartView(chart);
            DataLabelView = new PyramidDataLabelsView(chart);
            AbsoluteLayout.SetLayoutBounds(SeriesView, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(SeriesView, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(DataLabelView, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(DataLabelView, AbsoluteLayoutFlags.All);
            Add(SeriesView);
            Add(DataLabelView);

            if (chart is IChart ichart)
            {
                AbsoluteLayout.SetLayoutBounds(ichart.BehaviorLayout, new Rect(0, 0, 1, 1));
                AbsoluteLayout.SetLayoutFlags(ichart.BehaviorLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
                Add(ichart.BehaviorLayout);
            }

            BatchCommit();
        }

        #endregion

        #region Properties

        public override IPlotArea PlotArea => this;

        event EventHandler<EventArgs> IPlotArea.LegendItemsUpdated { add { legendItemsUpdated += value; } remove { legendItemsUpdated -= value; } }
        event EventHandler<LegendItemEventArgs>? IPlotArea.LegendItemToggled { add { legendItemsToggled += value; } remove { legendItemsToggled -= value; } }

        LegendHandler IPlotArea.LegendItemToggleHandler
        {
            get
            {
                return ToggleLegendItem;
            }
        }

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
        public ReadOnlyObservableCollection<ILegendItem> LegendItems => new ReadOnlyObservableCollection<ILegendItem>(legendItems);

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

        #endregion

        #region Methods

        protected override void UpdateAreaCore()
        {
            if (chart is IChart chartView)
            {
                chartView.ResetTooltip();
                var bounds = ChartUtils.GetSeriesClipRect(AreaBounds, chartView.TitleHeight);
                chartView.ActualSeriesClipRect = bounds;
                
                //Rendering funnel and pyramid only 80% of its actual size.
                var width = bounds.Width * sizeRatio;
                var height = bounds.Height * sizeRatio;
                var x = (bounds.Width - width) / 2;
                var y = (bounds.Height - height) / 2;
                var seriesBounds = new Rect(x, y, width, height);
                chart.SeriesBounds = seriesBounds;
            }

            if (!chart.SegmentsCreated)
            {
                if (chart.Segments.Count != 0)
                {
                    chart.Segments.Clear();
                }
            }

            chart.GenerateSegments();
            chart.LayoutSegments();
            InvalidateChart();
        }

        public void UpdateLegendItems()
        {
            if (shouldPopulateLegendItems)
            {
                UpdateLegendItemsSource();
                shouldPopulateLegendItems = false;
                legendItemsUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        internal void InvalidateChart()
        {
            SeriesView?.InvalidateDrawable();
            DataLabelView?.InvalidateDrawable();
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

        private void UpdateLegendItemsSource()
        {
            if (chart is IPyramidChartDependent triangleChart)
            {
                triangleChart.UpdateLegendItemsSource(legendItems);
            }
        }

        void ToggleLegendItem(ILegendItem legendItem)
        {
            if (legend != null && legend.IsVisible && legend.ToggleVisibility)
            {

            }
        }

        #endregion
    }
}
