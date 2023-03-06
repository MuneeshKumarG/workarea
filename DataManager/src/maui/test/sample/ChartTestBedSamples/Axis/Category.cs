using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChartTestBedSamples.Axis;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Charts;

namespace ChartTestBedSamples
{
	public partial class Category : ContentPage
	{
		public Category()
		{
			InitializeComponent();
        }
    }

    public class CategoryViewModel
    {
        public ObservableCollection<ChartDataModel> Data1 { get; set; }
        public ObservableCollection<ChartDataModel> Data2 { get; set; }
        public ObservableCollection<ChartDataModel> Data3 { get; set; }


        public CategoryViewModel()
        {
            Data1 = new ObservableCollection<ChartDataModel>()
            {
                new ChartDataModel("Jan", 78),
                new ChartDataModel("Feb", 75),
                new ChartDataModel("Mar", 63),
                new ChartDataModel("Apr", 78),
                new ChartDataModel("May", 74),
            };

            Data2 = new ObservableCollection<ChartDataModel>()
            {
                 new ChartDataModel("Jan", 13),
                new ChartDataModel("Feb", 19),
                new ChartDataModel("Mar", 35),
                new ChartDataModel("Apr", 60),
                new ChartDataModel("May", 58),
            };

            Data3 = new ObservableCollection<ChartDataModel>() {
                 new ChartDataModel("Jan", 30),
                new ChartDataModel("Feb", 25),
                new ChartDataModel("Mar", 36),
                new ChartDataModel("Apr", 50),
                new ChartDataModel("May", 40),
            };
        }
    }
}
