using Microsoft.Maui.Controls;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    internal interface ICartesianChartArea : IChartArea
    {
        internal ChartAxis? PrimaryAxis { get; set; }

        internal RangeAxisBase? SecondaryAxis { get; set; }

        internal bool IsTransposed { get; set; }

        internal bool EnableSideBySideSeriesPlacement { get; set; }
    }
}
