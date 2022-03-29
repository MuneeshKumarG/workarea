using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if WinUI
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
#elif !WPF
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#endif
namespace Syncfusion.UI.Xaml.Charts.Panels
{
    public class ChartAxisHeaderPanel:ILayoutCalculator 
    {
#pragma warning disable 649
        private Panel labelsPanels;
#pragma warning disable 649
        private Size desiredSize;

       internal ContentControl headerContent;
        public List<UIElement> Children
        {
            get
            {
                if (labelsPanels != null)
                {
                    return labelsPanels.Children.Cast<UIElement>().ToList();
                }

                return null;
            }
        }

        public Panel Panel
        {
            get { return labelsPanels; }
        }

        public double Left
        {
            get;
            set;
        }

        public double Top
        {
            get;
            set;
        }

        public Size DesiredSize
        {
            get
            {
                return desiredSize;
            }
        }

        public Size Measure(Windows.Foundation.Size availableSize)
        {
            throw new NotImplementedException();
        }

        public Size Arrange(Windows.Foundation.Size finalSize)
        {
            throw new NotImplementedException();
        }

        public void UpdateElements()
        {
            throw new NotImplementedException();
        }

        public void DetachElements()
        {
            throw new NotImplementedException();
        }
        public ChartAxisHeaderPanel()
        {
            headerContent = new ContentControl();
            headerContent.Content = "Header";
        }
    }
}
