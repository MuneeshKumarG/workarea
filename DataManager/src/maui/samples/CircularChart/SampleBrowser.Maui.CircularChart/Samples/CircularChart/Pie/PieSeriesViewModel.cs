﻿using System.Collections.ObjectModel;

namespace SampleBrowser.Maui.CircularChart.SfCircularChart
{
    public class PieSeriesViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> PieSeriesData { get; set; }
        public ObservableCollection<ChartDataModel> SemiCircularData { get; set; }
        public PieSeriesViewModel()
        {
            PieSeriesData = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("David", 16.6),
                new ChartDataModel("Steve", 14.6),
                new ChartDataModel("Jack", 18.6),
                new ChartDataModel("John", 20.5),
                new ChartDataModel("Regev", 39.5),
           };

            SemiCircularData = new ObservableCollection<ChartDataModel>
            {
                new ChartDataModel("Product A", 750),
                new ChartDataModel("Product B", 463),
                new ChartDataModel("Product C", 389),
                new ChartDataModel("Product D", 697),
                new ChartDataModel("Product E", 251)
            };
        }
    }
}
