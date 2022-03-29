using Microsoft.Maui;
using Microsoft.Maui.Controls;
using System;


namespace ChartTestBedSamples
{
	public class Doughnut : ContentPage
	{
		//public SfChart Chart {get;set;}

		//public DoughnutSeries Series {
		//	get;
		//	set;
		//}

		//Grid grid;
		//public Doughnut ( SfChart chart)
		//{
		//	this.Chart = chart;
		//	ScrollView scrollview = new ScrollView ();
		//	ViewModel view = new ViewModel ();
		//	Series = new DoughnutSeries ();
		//	Series.ItemsSource = view.DataPointPos;
		//	grid = new Grid
		//	{

		//		Padding = new Thickness(5,10,5,5),
		//		RowDefinitions = 
		//		{
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto }
		//		},
		//		ColumnDefinitions = 
		//		{
		//			new ColumnDefinition { Width = new GridLength ( 1, GridUnitType.Star) }
		//		}
		//		};
		//	//grid.Children.Add (AddDoughnutSeriesProperties (Series), 0,0);
		//	grid.Children.Add (AccumulationSeriesProperties.AddAccumulationSeriesProperties (Series), 0,1);
		//	grid.Children.Add (CircularSeriesProperties.AddCircularSeriesProperties (Series), 0,2);
		//	grid.Children.Add (SeriesProperties.ArrangeSeriesProperties (Series), 0,3);
		//	grid.Children.Add (SeriesProperties.AddItemSourceProperties (Series), 0, 4);
		//	grid.Children.Add (SeriesProperties.AddDataMarkerProperty (grid, Series,5),0,5);


  //          var stackLayout = new StackLayout();
  //          stackLayout.Children.Add(grid);
  //          scrollview.Content = stackLayout;
  //          this.SetContent(scrollview, grid);
		//}

		//public static Grid AddDoughnutSeriesProperties(DoughnutSeries series)
		//{
		//	Grid grid = new Grid
		//	{
		//		Padding = new Thickness(5,5,5,5),
		//		RowDefinitions = 
		//		{
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto },
		//			new RowDefinition { Height = GridLength.Auto }
		//		},
		//		ColumnDefinitions = 
		//		{
		//			new ColumnDefinition { Width = new GridLength ( 3, GridUnitType.Star) },
		//			new ColumnDefinition { Width = new GridLength ( 2, GridUnitType.Star) }
		//		}
		//		};
		//	Label header = new Label ();
		//	header.Text = "DoughnutSeries Properties";
		//	header.TextColor = Color.Red;
		//	grid.Children.Add (header,0,0);

		//	AddDoughnutCoefficientProperty (grid, series);
		//	return grid;
		//}

		//static void AddDoughnutCoefficientProperty (Grid grid, DoughnutSeries series)
		//{
		//	Label label = new Label ();
		//	label.HorizontalOptions = LayoutOptions.StartAndExpand;
		//	label.HorizontalTextAlignment = TextAlignment.Start;
		//	label.VerticalTextAlignment = TextAlignment.Center;
		//	label.Text = "DoughnutCoefficient";

		//	Slider slider = new Slider ();
		//	slider.StyleId = "DoughnutCoeff";
		//	slider.HorizontalOptions = LayoutOptions.Start;
		//	slider.Minimum = -1;
		//	slider.Maximum = 1;
		//	slider.SetBinding (Slider.ValueProperty, SeriesProperties.CreateBinding ("DoughnutCoefficient", series,null));

		//	grid.Children.Add (label, 0, 1);
		//	grid.Children.Add (slider, 1, 1);
		//}
	}
}


