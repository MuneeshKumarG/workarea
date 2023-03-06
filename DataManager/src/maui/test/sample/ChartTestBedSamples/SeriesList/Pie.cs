using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Charts;

namespace ChartTestBedSamples
{
	public class Pie : ContentPage
	{
		public SfCircularChart Chart { get; set; }

		public PieSeries Series
		{
			get;
			set;
		}

		Grid grid;
		public Pie(SfCircularChart chart)
		{
			this.Chart = chart;
			ScrollView scrollview = new ScrollView();
			ViewModel view = new ViewModel();
			Series = new PieSeries();
			Series.ItemsSource = view.DataPointPos;

			grid = new Grid
			{

				Padding = new Thickness(5, 10, 5, 5),
				RowDefinitions =
				{
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto }
				},
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = new GridLength ( 1, GridUnitType.Star) }
				}
			};

			grid.Add(CircularSeriesProperties.AddCircularSeriesProperties(Series), 0, 1);
			grid.Add(SeriesProperties.ArrangeSeriesProperties(Series), 0, 2);
			grid.Add(SeriesProperties.AddItemSourceProperties(Series), 0, 3);
			//grid.Children.Add (SeriesProperties.AddDataMarkerProperty (grid, Series,4),0,4);


			var stackLayout = new StackLayout();
			//stackLayout.Children.Add(grid);
			scrollview.Content = grid;
			this.SetContent(scrollview, null);
		}
	}
}