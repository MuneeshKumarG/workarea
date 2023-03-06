using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Core.Internals;

namespace Syncfusion.Maui.Charts
{
    internal class CircularChartArea : AreaBase, IChartArea
    {
        #region Fields
        private ChartSeriesCollection? series;
        private readonly CircularPlotArea circularPlotArea;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public CircularChartArea(IChart chart)
        {
            BatchBegin();
            circularPlotArea = new CircularPlotArea(this);
            circularPlotArea.Chart = chart;
            AbsoluteLayout.SetLayoutBounds(circularPlotArea, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(circularPlotArea, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            Add(circularPlotArea);
            AbsoluteLayout.SetLayoutBounds(chart.BehaviorLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(chart.BehaviorLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            circularPlotArea.Add(chart.BehaviorLayout);
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
                    series = value;
                    circularPlotArea.Series = value;

                }
            }
        }

        public ReadOnlyObservableCollection<ChartSeries>? VisibleSeries => Series?.GetVisibleSeries();
        public override IPlotArea PlotArea => circularPlotArea;

        #endregion

        #region Methods

        protected override void UpdateAreaCore()
        {
            if (circularPlotArea.Chart is IChart chart)
            {
                chart.ResetTooltip();
                chart.ActualSeriesClipRect = ChartUtils.GetSeriesClipRect(AreaBounds, circularPlotArea.Chart.TitleHeight);
            }

            circularPlotArea.PopulateDataManager();
            circularPlotArea.UpdateVisibleSeries();
        }
        
        #endregion
    }
}
