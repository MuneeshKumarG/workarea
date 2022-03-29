using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;

namespace Syncfusion.Maui.Charts
{
    internal interface IChart
    {
        //Todo: Need to recheck BackgroundColor property for constract color.
        Color BackgroundColor { get; }

        ChartSelectionBehavior? ActualSelectionBehavior { get; set; }

        ChartTooltipBehavior? ActualTooltipBehavior { get; set; }

        SfTooltip? TooltipView { get; set; }

        AbsoluteLayout BehaviorLayout { get;}

        ChartLegend? Legend { get; set; }

        public IArea Area { get; }

        double TitleHeight { get; }

        Rect ActualSeriesClipRect { get; set; }

        internal void RaiseSelectionChangedEvent(ChartSelectionChangedEventArgs args);

        internal void RaiseSelectionChangingEvent(ChartSelectionChangingEventArgs args);

        internal void ResetTooltip()
        {
            ActualTooltipBehavior?.Hide(this);
        }
    }
}
