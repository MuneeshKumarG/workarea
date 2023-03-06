using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Charts;

namespace ChartTestBedSamples
{
	public class CircularSeriesProperties
	{
		public static Grid AddCircularSeriesProperties(CircularSeries series)
		{

			Grid grid = new Grid
			{
				Padding = new Thickness(5, 5, 5, 5),
				RowDefinitions =
				{
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
					new ColumnDefinition { Width = new GridLength ( 3, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength ( 2, GridUnitType.Star) }
				}
			};
			Label header = new Label();
			header.Text = "CircularSeries Properties";
			header.TextColor = Colors.Red;
			grid.Add(header, 0, 0);

			AddYBindingPathProperty(grid, series);
			AddCircleCoefficientProperty(grid, series);
			AddStartAngleProperty(grid, series);
			AddEndAngleProperty(grid, series);
			AddExplodeOnTouchProperty(grid, series);
			AddExplodeAllProperty(grid, series);
			AddExplodeIndexProperty(grid, series);
			AddExplodeRadiusProperty(grid, series);
			AddStrokeColorProperty(grid, series);
			return grid;
		}

		static void AddYBindingPathProperty(Grid grid, CircularSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "YBindingPath";
			label.TextColor = Colors.Black;

			Picker picker1 = new Picker();
			picker1.StyleId = "YPath";
			picker1.WidthRequest = 100;
			picker1.HorizontalOptions = LayoutOptions.Start;
			picker1.Items.Add("YValue");
			picker1.Items.Add("YNegValue");
			picker1.Items.Add("YEmptyValue");
			picker1.Items.Add("Empty");
			picker1.Items.Add("Null");
			picker1.SetBinding(Picker.SelectedIndexProperty, SeriesProperties.CreateBinding("YBindingPath", series, new YBindingPathConverter()));


			grid.Add(label, 0, 1);
			grid.Add(picker1, 1, 1);
		}


		static void AddCircleCoefficientProperty(Grid grid, CircularSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "CircularCoefficient";
			label.TextColor = Colors.Black;

			Entry txt = new Entry();
			txt.StyleId = "CircleCoef";
			txt.HorizontalOptions = LayoutOptions.Start;
			txt.WidthRequest = 100;
			txt.SetBinding(Entry.TextProperty, SeriesProperties.CreateBinding("CircularCoefficient", series, null));

			grid.Add(label, 0, 2);
			grid.Add(txt, 1, 2);
		}

		static void AddStartAngleProperty(Grid grid, CircularSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "StartAngle";
			label.TextColor = Colors.Black;

			Entry txtStartAngle = new Entry();
			txtStartAngle.StyleId = "StartAngle";
			txtStartAngle.HorizontalOptions = LayoutOptions.Start;
			txtStartAngle.WidthRequest = 100;
			txtStartAngle.SetBinding(Entry.TextProperty, SeriesProperties.CreateBinding("StartAngle", series, null));

			grid.Add(label, 0, 3);
			grid.Add(txtStartAngle, 1, 3);
		}

		static void AddEndAngleProperty(Grid grid, CircularSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "EndAngle";
			label.TextColor = Colors.Black;

			Entry txtStartAngle = new Entry();
			txtStartAngle.StyleId = "EndAngle";
			txtStartAngle.HorizontalOptions = LayoutOptions.Start;
			txtStartAngle.WidthRequest = 100;
			txtStartAngle.SetBinding(Entry.TextProperty, SeriesProperties.CreateBinding("EndAngle", series, null));

			grid.Add(label, 0, 4);
			grid.Add(txtStartAngle, 1, 4);
		}

		static void AddExplodeOnTouchProperty(Grid grid, CircularSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.FillAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "ExplodeOnTouch";
			label.TextColor = Colors.Black;

			Switch switchIsVisible = new Switch();
			switchIsVisible.StyleId = "ExpOnTouch";
			switchIsVisible.SetBinding(Switch.IsToggledProperty, SeriesProperties.CreateBinding("ExplodeOnTouch", series, null));

			grid.Add(label, 0, 5);
			grid.Add(switchIsVisible, 1, 5);
		}

		static void AddExplodeAllProperty(Grid grid, CircularSeries series)
		{
			Label lblIsVisible = new Label();
			lblIsVisible.HorizontalOptions = LayoutOptions.StartAndExpand;
			lblIsVisible.HorizontalTextAlignment = TextAlignment.Start;
			lblIsVisible.VerticalTextAlignment = TextAlignment.Center;
			lblIsVisible.Text = "ExplodeAll";
			lblIsVisible.TextColor = Colors.Black;

			Switch switchIsVisible = new Switch();
			switchIsVisible.StyleId = "ExpAll";
			switchIsVisible.SetBinding(Switch.IsToggledProperty, SeriesProperties.CreateBinding("ExplodeAll", series, null));

			grid.Add(lblIsVisible, 0, 6);
			grid.Add(switchIsVisible, 1, 6);
		}

		static void AddExplodeIndexProperty(Grid grid, CircularSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "ExplodeIndex";
			label.TextColor = Colors.Black;

			Entry txt = new Entry();
			txt.StyleId = "ExpIndex";
			txt.HorizontalOptions = LayoutOptions.Start;
			txt.WidthRequest = 100;
			txt.SetBinding(Entry.TextProperty, SeriesProperties.CreateBinding("ExplodeIndex", series, null));

			grid.Add(label, 0, 7);
			grid.Add(txt, 1, 7);
		}

		static void AddExplodeRadiusProperty(Grid grid, CircularSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "ExplodeRadius";
			label.TextColor = Colors.Black;

			Entry txt = new Entry();
			txt.StyleId = "ExpRadius";
			txt.HorizontalOptions = LayoutOptions.Start;
			txt.WidthRequest = 100;
			txt.SetBinding(Entry.TextProperty, SeriesProperties.CreateBinding("ExplodeRadius", series, null));

			grid.Add(label, 0, 8);
			grid.Add(txt, 1, 8);
		}

		static void AddStrokeColorProperty(Grid grid, CircularSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "StrokeColor";
			label.TextColor = Colors.Black;

			Picker picker = new Picker();
			picker.StyleId = "AccStrkColor";
			picker.WidthRequest = 100;
			picker.SelectedIndex = 1;
			picker.HorizontalOptions = LayoutOptions.Start;
			picker.Items.Add("Red");
			picker.Items.Add("Blue");
			picker.Items.Add("Gray");
			picker.Items.Add("Default");
			picker.Items.Add("HexaColor");
			picker.Items.Add("RGBColor");
			picker.Items.Add("ARGBColor");
			picker.Items.Add("GSColor");
			picker.Items.Add("Lime");
			picker.SetBinding(Picker.SelectedIndexProperty, SeriesProperties.CreateBinding("StrokeColor", series, new ColorConverter()));

			grid.Add(label, 0, 9);
			grid.Add(picker, 1, 9);
		}
	}
}