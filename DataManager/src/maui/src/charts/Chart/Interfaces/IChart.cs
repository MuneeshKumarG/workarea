using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;
using Syncfusion.Maui.Core.Internals;

namespace Syncfusion.Maui.Charts
{
    internal interface IChart
    {
        //Todo: Need to recheck BackgroundColor property for constract color.
        Color BackgroundColor { get; }

        ChartTooltipBehavior? ActualTooltipBehavior { get; set; }

        SfTooltip? TooltipView { get; set; }

        AbsoluteLayout BehaviorLayout { get;}

        ChartLegend? Legend { get; set; }

        public IArea Area { get; }

        double TitleHeight { get; }

        Rect ActualSeriesClipRect { get; set; }

        Brush? GetSelectionBrush(ChartSeries series);

        TooltipInfo? GetTooltipInfo(ChartTooltipBehavior behavior, float x, float y);

        internal void ResetTooltip()
        {
            ActualTooltipBehavior?.Hide(this);
        }

        DataManager DataManager { get; }

        bool IsDataPopulated { get; set; }
    }
}
