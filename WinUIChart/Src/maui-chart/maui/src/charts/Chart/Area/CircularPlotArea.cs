using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    internal class CircularPlotArea : ChartPlotArea
    {
        #region Private Fields

        private readonly CircularChartArea circularChartArea;

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="chartArea"></param>
        public CircularPlotArea(CircularChartArea chartArea):base()
        {
            circularChartArea = chartArea;
        }

        #endregion

        #region Methods

        protected override void UpdateLegendItemsSource()
        {
            if (Series == null || legend == null || !legend.IsVisible)
            {
                return;
            }

            legendItems.Clear();

            foreach (ChartSeries series in Series)
            {
                int index = 0;
                if (series.IsVisibleOnLegend)
                {
                    for (int i = 0; i < series.PointsCount; i++)
                    {
                        var legendItem = new CircularLegendItem();
                        legendItem.IconType = ChartUtils.GetShapeType(series.LegendIcon);
                        Brush? solidColor = series.GetFillColor(index);
                        legendItem.IconBrush = index == series.SelectedIndex && series.SelectionBrush != null ? series.SelectionBrush : solidColor != null ? solidColor : new SolidColorBrush(Colors.Transparent);
                        legendItem.Text = series.GetActualXValue(index)?.ToString() ?? string.Empty;
                        legendItem.Index = index;
                        legendItem.Item = series.ActualData?[index];
                        legendItems.Add(legendItem);
                        index++;
                    }
                }
            }
        }

        internal override void AddSeries(int index, object chartSeries)
        {
            var circularSeries = chartSeries as CircularSeries;
            if (circularSeries != null)
            {
                circularSeries.ChartArea = circularChartArea;
            }
            base.AddSeries(index, chartSeries);
        }

        #endregion
    }

}
