using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    internal class PlotArea : Grid, IPlotArea
    {
        private EventHandler<LegendItemEventArgs> legendItemsToggled;

        internal SfCartesianChart Area;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SfCartesianChart class.
        /// </summary>
        public PlotArea()
        {
        }

        #endregion


        #region Legend

        ObservableCollection<ILegendItem> IPlotArea.LegendItems
        {
            get
            {
                if (Area.Legend != null)
                    return Area.Legend.LegendItems;
                else return null;
            }
            set
            {

            }
        }

        Rect IPlotArea.PlotAreaBounds
        {
            get
            {
                return Rect.Empty;
            }
        }

        event EventHandler<LegendItemEventArgs> IPlotArea.LegendItemToggled { add { legendItemsToggled += value; } remove { legendItemsToggled -= value; } }

        void IPlotArea.UpdateLegendItems()
        {

        }

        #endregion
    }
}
