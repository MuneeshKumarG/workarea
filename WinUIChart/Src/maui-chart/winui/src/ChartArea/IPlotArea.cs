using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    internal interface IPlotArea
    {
        ObservableCollection<ILegendItem> LegendItems { get; set; }

        Rect PlotAreaBounds { get; }

        event EventHandler<LegendItemEventArgs> LegendItemToggled
        {
            add
            {
                throw new NotImplementedException();
            }
            remove
            {
                throw new NotImplementedException();
            }
        }

        void UpdateLegendItems();
    }
}
