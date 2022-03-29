using Syncfusion.Maui.Core;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The <see cref="ChartLegendItem"/> is serve as a base class for both circular and cartesian legend item.
    /// </summary>
    internal abstract class ChartLegendItem : LegendItem
    {
    }

    /// <summary>
    /// The <see cref="CartesianLegendItem"/> is used to identify the corresponding series in the cartesian chart.
    /// </summary>
    internal class CartesianLegendItem : ChartLegendItem
    {
        internal CartesianSeries? Series { get; set; }
    }

    /// <summary>
    /// The <see cref="CircularLegendItem"/> is used to identify the corresponding segment of circular series in the circular chart.
    /// </summary>
    internal class CircularLegendItem : ChartLegendItem
    {

    }
}
