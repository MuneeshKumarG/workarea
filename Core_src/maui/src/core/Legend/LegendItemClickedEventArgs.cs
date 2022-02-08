using System;

namespace Syncfusion.Maui.Core
{
    internal class LegendItemClickedEventArgs : EventArgs
    {
        internal LegendItem? LegendItem { get; set; }
    }

    /// <summary>
    /// Delegate for the LegendItemToggleHandler event.  
    /// </summary>
    /// <param name="legendItem">Used to specifyt he legend item.</param>
    public delegate void LegendHandler(ILegendItem legendItem);
}
