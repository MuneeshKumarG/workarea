using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI;

namespace Syncfusion.UI.Xaml.Charts
{
    
    internal class ChartColorModel
    {
        /// <summary>
        /// Contains a number of predefined colors.
        /// </summary>
        public static List<Brush> DefaultBrushes = new List<Brush>
        {
            new SolidColorBrush(Color.FromArgb(255, 0, 189, 174)),
            /*#00bdae*/
            new SolidColorBrush(Color.FromArgb(255, 64, 64, 65)),
            /*#404041*/
            new SolidColorBrush (Color.FromArgb(255, 53, 124, 210)),
            /*#357cd2*/
            new SolidColorBrush( Color.FromArgb(255, 229, 101, 144)),
            /*#e56590*/
            new SolidColorBrush(Color.FromArgb(255, 248, 184, 131)),
            /*#f8b883*/	
            new SolidColorBrush(Color.FromArgb(255, 112, 173, 71)),
            /*#70ad47*/	
            new SolidColorBrush(Color.FromArgb(255, 221, 138, 189)),
            /*#dd8abd*/	
            new SolidColorBrush(Color.FromArgb(255, 27, 132, 232)),
            /*#7f84e8*/	
            new SolidColorBrush(Color.FromArgb(255, 123, 180, 235)),
            /*#7bb4eb*/	
            new SolidColorBrush(Color.FromArgb(255, 234, 122, 87))
            /*#ea7a57*/
        };
    }
}
