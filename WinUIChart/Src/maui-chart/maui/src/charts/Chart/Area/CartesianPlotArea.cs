using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;

namespace Syncfusion.Maui.Charts
{
    internal class CartesianPlotArea : ChartPlotArea
    {
        #region Fields

        internal readonly CartesianGridLineLayout GridLineLayout;

        internal readonly CartesianChartArea chartArea;

        #endregion

        #region Constructor

        public CartesianPlotArea(CartesianChartArea area) : base()
        {
            BatchBegin();
            chartArea = area;
            GridLineLayout = new CartesianGridLineLayout(area);
            AbsoluteLayout.SetLayoutBounds(GridLineLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(GridLineLayout, AbsoluteLayoutFlags.All);
            Insert(0, GridLineLayout);
            BatchCommit();
        }

        #endregion

        #region Protected Methods

        protected override void UpdateLegendItemsSource()
        {
            if (Series == null || legend == null || !legend.IsVisible)
            {
                return;
            }

            legendItems.Clear();
            int index = 0;

            foreach (CartesianSeries series in Series)
            {
                if (series.IsVisibleOnLegend)
                {
                    var legendItem = new CartesianLegendItem();
                    legendItem.IconType = ChartUtils.GetShapeType(series.LegendIcon);
                    legendItem.Series = series;
                    Brush? solidColor = series.GetFillColor(index);
                    legendItem.IconBrush = solidColor != null ? solidColor : new SolidColorBrush(Colors.Transparent);
                    legendItem.Text = series.Label;
                    legendItem.Index = index;
                    legendItem.IsToggled = !series.IsVisible;
                    legendItems?.Add(legendItem);
                    index++;
                }
            }
        }

        #endregion

        #region Internal Methods

        internal void InvalidateRender()
        {
            GridLineLayout?.InvalidateDrawable();
        }

        internal void ResetSegments()
        {
            var visibleSeries = VisibleSeries;

            if (visibleSeries != null)
            {
                foreach (var chartSeries in visibleSeries)
                {
                    chartSeries.SegmentsCreated = false;
                }
            }
        }

        internal override void AddSeries(int index, object chartSeries)
        {
            var cartesian = chartSeries as CartesianSeries;
            if (cartesian != null)
            {
                cartesian.ChartArea = chartArea;
                ResetSegments();
                cartesian.NeedToAnimateSeries = cartesian.EnableAnimation;
                chartArea.SideBySideSeriesPosition = null;
            }
            base.AddSeries(index, chartSeries);
        }

        internal override void RemoveSeries(int index, object series)
        {
            base.RemoveSeries(index, series);

            var cartesian = series as CartesianSeries;
            if (cartesian != null && cartesian.IsSideBySide)
            {
                chartArea.ResetSBSSegments();
            }
        }
        
        #endregion
    }
}
