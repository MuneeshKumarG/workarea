using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Charts
{
    internal interface IPlotArea
    {
        public ILegend? Legend { get; set; }

        public ReadOnlyObservableCollection<ILegendItem> LegendItems { get; }

        public Rect PlotAreaBounds { get; }

        public bool ShouldPopulateLegendItems { get; set;}

        public LegendHandler LegendItemToggleHandler
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<LegendItemEventArgs> LegendItemToggled
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

        public event EventHandler<EventArgs> LegendItemsUpdated
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

        public void UpdateLegendItems()
        {

        }
    }
}
