using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    public class RangeBarSerieViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> RangeBarData { get; set; }
        public RangeBarSerieViewModel()
        {
            RangeBarData = new ObservableCollection<ChartDataModel>()
        {
#if ANDROID || IOS
                   new ChartDataModel("Jan",13, 3),
                   new ChartDataModel("Feb",14, 3),
                   new ChartDataModel("Mar",17, 6),
                   new ChartDataModel("Apr",20, 8),
                   new ChartDataModel("May",24, 13),
                   new ChartDataModel("Jun",29, 17),
#else
                   new ChartDataModel("January",13, 3),
                   new ChartDataModel("February",14, 3),
                   new ChartDataModel("March",17, 6),
                   new ChartDataModel("April",20, 8),
                   new ChartDataModel("May",24, 13),
                   new ChartDataModel("June",29, 17),
                   new ChartDataModel("July",32, 19),
                   new ChartDataModel("August",30, 18),
                   new ChartDataModel("September",27, 16),
                   new ChartDataModel("October",23, 12),
                   new ChartDataModel("November",18, 8),
                   new ChartDataModel("December",15, 4),
#endif
            };
        }
    }
}
