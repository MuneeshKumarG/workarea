﻿using System;
using System.Collections.ObjectModel;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    public class SeriesSelectionViewModel : BaseViewModel
    {
        public ObservableCollection<Brush> CustomAlphaColor { get; set; }
        public ObservableCollection<ChartDataModel> SelectionData { get; set; }


        public SeriesSelectionViewModel()
        {
            CustomAlphaColor = new ObservableCollection<Brush>()
            {
                 new SolidColorBrush(Color.FromArgb("#40314A6E")),
                 new SolidColorBrush(Color.FromArgb("#48988B")),
                 new SolidColorBrush(Color.FromArgb("#405E498C")),
                 new SolidColorBrush(Color.FromArgb("#4074BD6F")),
                 new SolidColorBrush(Color.FromArgb("#40597FCA"))
            };

            SelectionData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel("CHN",17.5,68.3,14.2),
                new ChartDataModel("IND",25.8,67.4, 6.8),
                new ChartDataModel("PAK",34.6,  61, 4.4),
#if WINDOWS || MACCATALYST
                new ChartDataModel("IDN",25.6,67.9, 6.5),
                new ChartDataModel("BGD",26.3,68.4, 5.3),
#endif
            };
        }

    }
}
