using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Charts;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ChartTestBedSamples
{
	public class SamplePage : ContentPage
	{
		

		private SfCartesianChart cartesianChart;
		private SfCircularChart circularChart;
		private Label label;
		private Label testingLabel;
		private ListPage listPage;
		public static bool IsManualTesting { get; set; }

		public SamplePage(string chartname)
		{
			this.Title = "Chart Series";

			StackLayout layout = new StackLayout() { Padding = new Thickness(5, 15, 0, 5), HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand };
			label = new Label();
			label.HorizontalOptions = LayoutOptions.Center;
			label.FontSize = 8;
			label.Text = "Event Count:";
			label.IsVisible = false;
			layout.Children.Add(label);

			Button button = new Button();
			button.Text = " Goto List Page";
			button.HorizontalOptions = LayoutOptions.Center;
			button.Clicked += Button_Clicked;
			layout.Children.Add(button);

			if (chartname.Equals("Cartesian Charts"))
				layout.Children.Add(GetCartesinChart());
			else if (chartname.Equals("Circular Charts"))
				layout.Children.Add(GetCircularChart());

			testingLabel = new Label();
			testingLabel.HorizontalOptions = LayoutOptions.Center;
			testingLabel.FontSize = 25;
			testingLabel.Text = "ChartSize changed";
			testingLabel.IsVisible = false;
			layout.Children.Add(testingLabel);

			Content = layout;

			listPage = new ListPage(cartesianChart, circularChart, label, testingLabel);

			this.ToolbarItems.Add(new ToolbarItem
			{
				Text = "Manual",
				StyleId = "Manual",
				Order = ToolbarItemOrder.Primary,
				Command = new Command(() => Action()),
			});

			this.ToolbarItems.Add(new ToolbarItem
			{
				Text = "Automation",
				StyleId = "Automation",
				Order = ToolbarItemOrder.Primary,
				Command = new Command(Automation),
			});
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			Action();
		}

		private void Automation()
		{
			IsManualTesting = false;
			((ListView)listPage.Content).SelectedItem = null;
			Navigation.PushAsync(listPage);
		}

		private void Action()
		{
			IsManualTesting = true;
			((ListView)listPage.Content).SelectedItem = null;
			Navigation.PushAsync(listPage);
		}

		public SfCartesianChart GetCartesinChart()
		{
			cartesianChart = new SfCartesianChart
			{
				StyleId = "Chart",
				Title = new Label() { Text = "Cartesian Chart", TextColor = Colors.Black, HorizontalTextAlignment = TextAlignment.Center }
			};

			//cartesianChart.Legend = new ChartLegend();

			cartesianChart.VerticalOptions = LayoutOptions.FillAndExpand;
			cartesianChart.HorizontalOptions = LayoutOptions.FillAndExpand;

			//ColumnSeries columnSeries = new()
			//{
			//	ItemsSource = new ViewModel().NumericalData,
			//	XBindingPath = "XValue1",
			//	//YBindingPath = "YValue1"
			//};

			//cartesianChart.Series.Add(columnSeries);

			return cartesianChart;
		}

		public SfCircularChart GetCircularChart()
		{
			circularChart = new SfCircularChart
			{
				StyleId = "Chart",
				Title = new Label() { Text = "Circular Chart", TextColor = Colors.Black, HorizontalTextAlignment = TextAlignment.Center }
			};

			//chart.Legend = new ChartLegend();

			circularChart.VerticalOptions = LayoutOptions.FillAndExpand;
			circularChart.HorizontalOptions = LayoutOptions.FillAndExpand;

			PieSeries pieSeries = new()
			{
				ItemsSource = new ViewModel().NumericalData,
				XBindingPath = "XValue1",
				YBindingPath = "YValue1"
			};

			circularChart.Series.Add(pieSeries);

			return circularChart;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			circularChart = null;
			cartesianChart = null;
		}
	}

	public static class Ext
	{
		public static void Add(this Grid grid, View view, int row, int column)
		{
            Grid.SetRow(view, column);
            Grid.SetColumn(view, row);
            grid.Add(view);

		}

		public static void SetContent(this ContentPage contentPage, View content, IViewContainer<View> rootView)
		{
			if (SamplePage.IsManualTesting)
			{
				contentPage.Content = content;
			}
			else
			{
				var masterEntry = new EntryExt(rootView);
				masterEntry.VerticalOptions = LayoutOptions.FillAndExpand;
				masterEntry.HorizontalOptions = LayoutOptions.FillAndExpand;
				var stackLayout = new StackLayout();
				stackLayout.VerticalOptions = LayoutOptions.FillAndExpand;
				stackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
				stackLayout.Children.Add(masterEntry);
				contentPage.Content = stackLayout;
			}
		}
	}

	public class EntryExt : Editor
	{
		private readonly IViewContainer<View> layout;

		public EntryExt(IViewContainer<View> layout)
		{
			this.layout = layout;
			StyleId = "MasterEntry";
			TextChanged += EntryExt_TextChanged;
		}

		private void EntryExt_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (!e.NewTextValue.Contains(";") || e.NewTextValue.Length < 1) return;

			IEnumerator listStrLineElements = (sender as Editor).Text.Split(',').GetEnumerator();
			while (listStrLineElements.MoveNext())
			{
				var item = listStrLineElements.Current;
				string label = item.ToString();

				if (label.Contains(";"))
					label = label.Replace(";", "");

				if (label.Contains(","))
					label = label.Replace(",", "");

				var values = (label).Split('-');
				var name = values[0];
				var value = values[1];

				if (value == "~")
					value = "";
				if (values.Length > 2)
				{
					if (!values[2].Contains("&"))
					{
						var doubleValue = Double.Parse(values[2]);
						doubleValue *= -1;
						value = doubleValue.ToString();
					}

				}

				var child = GetChild(layout, name);

				if (child == null)
				{
					throw new InvalidOperationException("The specified element " + name + " is not found");
				}

				if (child is Entry)
				{
					(child as Entry).Text = value;
				}
				else if (child is Slider)
				{
					(child as Slider).Value = Convert.ToDouble(value);
				}
				else if (child is Picker)
				{
					var picker = child as Picker;
					picker.SelectedIndex = picker.Items.IndexOf(value);
				}
				else if (child is Switch)
				{
					(child as Switch).IsToggled = Convert.ToInt32(value) != 0;
				}
				else if (child is Button)
				{
					(child as Button).Command.Execute(child);
				}
			}
			(sender as Editor).Text = string.Empty;
		}

		public View GetChild(IViewContainer<View> layout, string name)
		{
            foreach (var child in layout.Children)
            {
                //if (child is Grid || child is StackLayout)
                //{
                //    var result = GetChild(child as LayoutView, name);
                //    if (result != null)
                //        return result;
                //}
                //if (child.StyleId == name)
                //{
                //    return child;
                //}
            }
            return null;
		}
	}
}
