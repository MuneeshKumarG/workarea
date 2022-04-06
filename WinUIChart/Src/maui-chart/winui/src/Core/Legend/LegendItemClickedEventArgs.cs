using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.UI.Xaml.Charts
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
