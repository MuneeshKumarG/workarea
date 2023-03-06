using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Core;
using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SampleBrowser.Maui.CircularChart.SfCircularChart
{
    public partial class CustomizedRadialBarChart : SampleView
    {
        public CustomizedRadialBarChart()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
#if IOS
            if (IsCardView)
            {
                chart.WidthRequest = 350;
                chart.HeightRequest = 400;
                chart.VerticalOptions = LayoutOptions.Start;
            }
#endif
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            chart.Handler?.DisconnectHandler();
        }
    }

    public class IndexToItemSourceConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is LegendItem legendItem)
            {
                List<object?> collection = new List<object?>();
                collection.Add(legendItem.Item);
                return collection;
            }

            return null;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class RadialBarSizeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                double num = System.Convert.ToDouble(value);
                if (!double.IsNaN(num))
                {
                    return (double)num / 2 * 100.0;
                }
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

}

