using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.Layouts;

namespace ChartTestBedSamples
{
	public partial class MainPage : ContentPage
	{

		public MainPage()
		{
			InitializeComponent();

			AbsolutePositionLayout layout = new AbsolutePositionLayout();
			layout.HorizontalOptions = LayoutOptions.FillAndExpand;
			layout.VerticalOptions = LayoutOptions.FillAndExpand;
			var grid = new GridLayout() { BackgroundColor = Colors.Red };
			AbsolutePositionLayout.SetLayoutBounds(grid, new Rectangle(10, 10, 100, 100));
			layout.Children.Add(grid);
			var grid1 = new GridLayout() { BackgroundColor = Colors.Blue };
			AbsolutePositionLayout.SetLayoutBounds(grid1, new Rectangle(10, 130, 100, 100));
			layout.Children.Add(grid1);
			//Content = layout;
			listview.ItemsSource = new List<string>() { "Cartesian Charts", "Circular Charts", "Live Update", "Axis Range Style","Date Time", "Category values", "Scatter Chart" , "Area Chart", "Spline Series" , "Spline Area", "Column Chart", "DataLabels", "Zoom Pan"};
		}

		private void Listview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			int index = e.SelectedItemIndex;

			if (index == 0)
			{
				Navigation.PushAsync(new SamplePage("Cartesian Charts"));
			}
			else if (index == 1)
			{
				Navigation.PushAsync(new SamplePage("Circular Charts"));
			}
			else if (index == 2)
			{
				Navigation.PushAsync(new LiveUpdate() { Title = "Live update" });
			}
			else if (index == 3)
			{
				Navigation.PushAsync(new RangeStyle() { Title = "Axis Range Style" });
			}
			else if (index == 4)
			{
				Navigation.PushAsync(new DateTimeChart() { Title = "DateTime Axis" });
			}
			else if (index == 5)
			{
				Navigation.PushAsync(new Category() { Title = "Category Axis" });
			}
			else if (index == 6)
			{
				Navigation.PushAsync(new ScatterChart() { Title = "Scatter Plotting" });
			}
			else if (index == 7)
			{
				Navigation.PushAsync(new AreaChart() { Title = "Area Plotting" });
			}
			else if (index == 8)
			{
				Navigation.PushAsync(new SplineChart() { Title = "Scatter Plotting" });
			}
			else if (index == 9)
			{
				Navigation.PushAsync(new SplineAreaChart() { Title = "Spline Area Plotting" });
			}
			else if (index == 10)
			{
				Navigation.PushAsync(new ColumnChart() { Title = "Column Plotting" });
			}
			else if (index == 11)
			{
				Navigation.PushAsync(new DataLabelListPage());
			}
			else if (index == 12)
			{
				Navigation.PushAsync(new ChartZoom() { Title = "Chart Zoom Pan behavior" });
			}
		}
	}
}
