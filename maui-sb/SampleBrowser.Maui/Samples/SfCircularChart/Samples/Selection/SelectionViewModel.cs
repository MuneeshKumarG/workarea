﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBrowser.Maui.SfCircularChart
{
    public class SelectionViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataModel> CircularData { get; set; }

        public SelectionViewModel()
        {
            CircularData = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel("CHN", 17, 54, 9),
                new ChartDataModel("USA", 19, 67, 14),
                new ChartDataModel("IDN", 29, 65, 6),
                new ChartDataModel("JAP", 13, 61, 26),
                new ChartDataModel("BRZ", 24, 68, 8)
            };
        }
    }
}
