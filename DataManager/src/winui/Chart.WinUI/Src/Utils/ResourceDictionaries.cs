using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.UI.Xaml;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Contains Chart resource dictionaries
    /// </summary>
    internal static class ChartDictionaries
    {
        [ThreadStatic]
        private static ResourceDictionary genericLegendDictionary;

        internal static ResourceDictionary GenericLegendDictionary
        {

            get
            {
                if (genericLegendDictionary == null)
                {

                    genericLegendDictionary = new ResourceDictionary()
                    {
                        Source = new Uri(@"ms-appx:///Syncfusion.Chart.WinUI/Core/Legend/Themes/LegendSymbols.xaml")
                    };

                }
                return genericLegendDictionary;
            }
        }

        [ThreadStatic]
        private static ResourceDictionary genericSymbolDictionary;

        internal static ResourceDictionary GenericSymbolDictionary
        {
            get
            {
                if(genericSymbolDictionary == null)
                {
                    genericSymbolDictionary = new ResourceDictionary()
                    {
                        Source = new Uri(@"ms-appx:///Syncfusion.Chart.WinUI/ChartSeries/Themes/Generic.Symbol.xaml")
                    };
                }

                return genericSymbolDictionary;
            }
        }

        [ThreadStatic]
        private static ResourceDictionary genericCommonDictionary;

        internal static ResourceDictionary GenericCommonDictionary
        {
            get
            {
                if(genericCommonDictionary == null)
                {
                    genericCommonDictionary = new ResourceDictionary()
                    {
                        Source = new Uri(@"ms-appx:///Syncfusion.Chart.WinUI/Themes/generic.common.xaml")
                    };

                }

                return genericCommonDictionary;
            }
        }
    }
}
