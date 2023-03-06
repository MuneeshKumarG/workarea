using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Charts;

namespace ChartTestBedSamples
{
	public class SeriesProperties
	{
		public static Grid ArrangeSeriesProperties(ChartSeries series)
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
					new ColumnDefinition { Width = new GridLength ( 3, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength ( 2, GridUnitType.Star) }
				}
			};

			Label header = new Label();
			header.Text = "Common Properties";
			header.TextColor = Colors.Red;
			grid.Add(header, 0, 0);

			AddIsVisibleProperty(grid, series);
			AddColorProperty(grid, series);
			AddStrokeWidthProperty(grid, series);
			AddColorModelPaletteProperty(grid, series);
			AddCustomColorProperty(grid, series);
			return grid;
		}

		public static Grid AddItemSourceProperties(ChartSeries series)
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
			header.Text = "ItemsSource";
			header.TextColor = Colors.Red;
			grid.Add(header, 0, 0);

			AddCategoryData(grid, series, 1);
			AddNumericData(grid, series, 2);
			AddDateTimeData(grid, series, 3);
			AddItemSourceProperty(grid, series, 4);
			AddXBindingPathProperty(grid, series, 5);
			AddDynamicPoints(grid, series, 6);

			return grid;
		}


		static void AddIsVisibleProperty(Grid grid, ChartSeries series)
		{
			Label lblIsVisible = new Label();
			lblIsVisible.HorizontalOptions = LayoutOptions.StartAndExpand;
			lblIsVisible.HorizontalTextAlignment = TextAlignment.Start;
			lblIsVisible.VerticalTextAlignment = TextAlignment.Center;
			lblIsVisible.Text = "IsVisible";
			lblIsVisible.TextColor = Colors.Black;

			Switch switchIsVisible = new Switch();
			switchIsVisible.StyleId = "SerVisible";
			switchIsVisible.SetBinding(Switch.IsToggledProperty, CreateBinding("Visible", series, null));

			grid.Add(lblIsVisible, 0, 1);
			grid.Add(switchIsVisible, 1, 1);
		}

		static void AddColorProperty(Grid grid, ChartSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "Color";
			label.TextColor = Colors.Black;

			Picker picker = new Picker();
			picker.StyleId = "SerColor";
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
			picker.Items.Add("Transparent");
			picker.Items.Add("Black");
			picker.SetBinding(Picker.SelectedIndexProperty, CreateBinding("Color", series, new ColorConverter()));

			grid.Add(label, 0, 2);
			grid.Add(picker, 1, 2);
		}

		static void AddStrokeWidthProperty(Grid grid, ChartSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "StrokeWidth";
			label.TextColor = Colors.Black;

			Slider slider = new Slider();
			slider.StyleId = "SerStrkWidth";
			slider.HorizontalOptions = LayoutOptions.FillAndExpand;
			slider.Minimum = -20;
			slider.Maximum = 20;
			slider.SetBinding(Slider.ValueProperty, CreateBinding("StrokeWidth", series, null));

			grid.Add(label, 0, 3);
			grid.Add(slider, 1, 3);
		}

		static void AddColorModelPaletteProperty(Grid grid, ChartSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "Palette";
			label.TextColor = Colors.Black;

			Picker picker = new Picker();
			picker.StyleId = "SerPalette";
			picker.WidthRequest = 100;
			picker.SelectedIndex = 1;
			picker.HorizontalOptions = LayoutOptions.Start;
			picker.Items.Add("None");
			picker.Items.Add("Metro");
			picker.Items.Add("Custom");
			picker.SetBinding(Picker.SelectedIndexProperty, CreateBinding("Palette", series, new PaletteConverter()));

			grid.Add(label, 0, 4);
			grid.Add(picker, 1, 4);
		}

		static void AddCustomColorProperty(Grid grid, ChartSeries series)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "CustomColors";
			label.TextColor = Colors.Black;

			Picker picker = new Picker();
			picker.StyleId = "SerCstBrushes";
			picker.WidthRequest = 100;
			picker.SelectedIndex = 1;
			picker.HorizontalOptions = LayoutOptions.Start;
			picker.Items.Add("RGBGL");
			picker.Items.Add("LON");
			picker.Items.Add("RGBColors");
			picker.Items.Add("ARGBColors");
			picker.Items.Add("GSColors");
			picker.SetBinding(Picker.SelectedIndexProperty, CreateBinding("CustomColors", series, new CustomColorConverter()));

			grid.Add(label, 0, 5);
			grid.Add(picker, 1, 5);
		}

		#region ItemsSource 

		public static void AddCategoryData(Grid grid, ChartSeries series, int row)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "CategoryData";
			label.TextColor = Colors.Black;

			Picker picker1 = new Picker();
			picker1.StyleId = "CatData";
			picker1.WidthRequest = 100;
			picker1.HorizontalOptions = LayoutOptions.Start;
			picker1.Items.Add("EmptyData");
			picker1.Items.Add("DataPNEg");
			picker1.Items.Add("DataPPos");
			picker1.Items.Add("DataPENP");
			picker1.Items.Add("FLEmp");
			picker1.Items.Add("SingleEmp");
			picker1.Items.Add("SinglePos");
			picker1.Items.Add("SingleNeg");
			picker1.Items.Add("WithoutDP");
			picker1.Items.Add("Null");
			picker1.Items.Add("DatePos");
			picker1.Items.Add("MoreData");
			picker1.Items.Add("AllZero");
			picker1.SetBinding(Picker.SelectedIndexProperty, CreateBinding("ItemsSource", series, new CategoryItemsSourceConverter()));

			grid.Add(label, 0, row);
			grid.Add(picker1, 1, row);
		}

		public static void AddNumericData(Grid grid, ChartSeries series, int row)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "NumericData";
			label.TextColor = Colors.Black;

			Picker picker1 = new Picker();
			picker1.StyleId = "NumData";
			picker1.WidthRequest = 100;
			picker1.HorizontalOptions = LayoutOptions.Start;
			picker1.Items.Add("EmptyData");
			picker1.Items.Add("DataPNEg");
			picker1.Items.Add("DataPPos");
			picker1.Items.Add("DataPENP");
			picker1.Items.Add("FLEmp");
			picker1.Items.Add("SingleEmp");
			picker1.Items.Add("SinglePos");
			picker1.Items.Add("SingleNeg");
			picker1.Items.Add("WithoutDP");
			picker1.Items.Add("Null");
			picker1.Items.Add("FNLPEmpty");
			picker1.Items.Add("AllZero");
			picker1.Items.Add("LongXVal");
			picker1.SetBinding(Picker.SelectedIndexProperty, CreateBinding("ItemsSource", series, new NumericItemsSourceConverter()));

			grid.Add(label, 0, row);
			grid.Add(picker1, 1, row);
		}

		public static void AddDateTimeData(Grid grid, ChartSeries series, int row)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "DateTimeData";
			label.TextColor = Colors.Black;

			Picker picker1 = new Picker();
			picker1.StyleId = "DatData";
			picker1.WidthRequest = 100;
			picker1.HorizontalOptions = LayoutOptions.Start;
			picker1.Items.Add("EmptyData");
			picker1.Items.Add("DataPNEg");
			picker1.Items.Add("DataPPos");
			picker1.Items.Add("DataPENP");
			picker1.Items.Add("FLEmp");
			picker1.Items.Add("SingleEmp");
			picker1.Items.Add("SinglePos");
			picker1.Items.Add("SingleNeg");
			picker1.Items.Add("WithoutDP");
			picker1.Items.Add("Null");
			picker1.Items.Add("DateMillisecond");
			picker1.SetBinding(Picker.SelectedIndexProperty, CreateBinding("ItemsSource", series, new DateTimeItemsSourceConverter()));

			grid.Add(label, 0, row);
			grid.Add(picker1, 1, row);
		}

		public static void AddItemSourceProperty(Grid grid, ChartSeries series, int row)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "ItemsSource";
			label.TextColor = Colors.Black;

			Picker picker1 = new Picker();
			picker1.StyleId = "ItmSrc";
			picker1.WidthRequest = 100;
			picker1.HorizontalOptions = LayoutOptions.Start;
			picker1.Items.Add("DateDatas");
			picker1.Items.Add("StrData");
			picker1.Items.Add("NumData");
			picker1.Items.Add("OHLCData");
			picker1.Items.Add("EmptyData");
			picker1.Items.Add("DateData1");
			picker1.Items.Add("StrData1");
			picker1.Items.Add("NumData1");
			picker1.Items.Add("OHLCData1");
			picker1.Items.Add("NullSrc");
			picker1.Items.Add("DataPNeg");
			picker1.Items.Add("DataPPos");
			picker1.Items.Add("DataPENP");
			picker1.Items.Add("WithoutPoint");
			picker1.Items.Add("WithoutDP");
			picker1.Items.Add("FLEmp");
			picker1.Items.Add("FNLPEmp");
			picker1.Items.Add("SingleEmp");
			picker1.Items.Add("SinglePos");
			picker1.Items.Add("SingleNeg");
			picker1.Items.Add("ListData");
			picker1.SetBinding(Picker.SelectedIndexProperty, CreateBinding("ItemsSource", series, new ItemsSourceConverter()));

			grid.Add(label, 0, row);
			grid.Add(picker1, 1, row);
		}

		static void AddDynamicPoints(Grid grid, ChartSeries chart, int row)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "DynamicPoints";
			label.TextColor = Colors.Black;

			Picker picker1 = new Picker();
			picker1.StyleId = "dynmicPnts";
			picker1.WidthRequest = 100;
			picker1.HorizontalOptions = LayoutOptions.Start;
			picker1.Items.Add("One");
			picker1.Items.Add("Two");
			picker1.Items.Add("Clear");

			//picker1.SelectedIndexChanged += (object sender, EventArgs e) => {
			//	AxisViewModel view = new AxisViewModel();
			//	int i = picker1.SelectedIndex;

			//	switch (i) {
			//	case 0:
			//		view.NumPositive.Add(new ChartDataPoint (0 ,30));
			//		break;
			//	case 1:
			//		view.NumPositive.Add(new ChartDataPoint (6 ,23));
			//		view.NumPositive.Add(new ChartDataPoint (7 ,42));
			//                 break;
			//                 case 2:
			//		view.NumPositive.Clear();
			//		break;
			//	default:
			//		break;
			//	}
			//};

			grid.Add(label, 0, row);
			grid.Add(picker1, 1, row);
		}


		public static void AddXBindingPathProperty(Grid grid, ChartSeries series, int row)
		{
			Label label = new Label();
			label.HorizontalOptions = LayoutOptions.StartAndExpand;
			label.HorizontalTextAlignment = TextAlignment.Start;
			label.VerticalTextAlignment = TextAlignment.Center;
			label.Text = "XBindingPath";
			label.TextColor = Colors.Black;

			Picker picker1 = new Picker();
			picker1.StyleId = "XPath";
			picker1.WidthRequest = 100;
			picker1.HorizontalOptions = LayoutOptions.Start;
			picker1.Items.Add("XDate");
			picker1.Items.Add("XValue");
			picker1.Items.Add("XString");
			picker1.Items.Add("Null");
			picker1.Items.Add("Empty");
			picker1.SetBinding(Picker.SelectedIndexProperty, CreateBinding("XBindingPath", series, new XBindingPathConverter()));


			grid.Add(label, 0, row);
			grid.Add(picker1, 1, row);
		}



		static void AddYBindingPathProperty(Grid grid, XYDataSeries series)
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
			picker1.SetBinding(Picker.SelectedIndexProperty, CreateBinding("YBindingPath", series, new YBindingPathConverter()));


			grid.Add(label, 0, 2);
			grid.Add(picker1, 1, 2);
		}

		#endregion

		public static Binding CreateBinding(string path, object source, IValueConverter converter)
		{
			var bindingProvider = new Binding
			{
				Path = path,
				Source = source,
				Converter = converter,
				Mode = BindingMode.TwoWay
			};
			return bindingProvider;
		}

	}
}