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
	public partial class DateTimeChart : ContentPage
	{
		public DateTimeChart()
		{
			InitializeComponent();
		}
	}

	public class DateTimeViewModel
	{
		public ObservableCollection<ChartDataModel> liveData1 { get; set; }

        public DateTimeViewModel()
        {
			liveData1 = new ObservableCollection<ChartDataModel>();

			Random r = new Random();
			DateTime date = DateTime.Now;
			for (int i = 0; i < 40; i++)
			{
				liveData1.Add(new ChartDataModel(date, r.Next(10, 110)));
				date = date.AddMonths(1);
			}
        }
	}
}
