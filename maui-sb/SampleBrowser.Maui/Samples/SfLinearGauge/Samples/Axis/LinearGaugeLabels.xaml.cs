using Microsoft.Maui;
using Microsoft.Maui.Controls;
using SampleBrowser.Maui.Core;
using Syncfusion.Maui.Gauges;
using System;
using System.Globalization;
using System.Threading.Tasks;
using LinearGauge = Syncfusion.Maui.Gauges.SfLinearGauge;
using Microsoft.Maui.Graphics;

namespace SampleBrowser.Maui.SfLinearGauge
{
    public partial class LinearGaugeLabels : SampleView
    {
       
        public LinearGaugeLabels()
        {
            InitializeComponent();
			scrollView.Content = GetHorizontalLinearGauges();
		}

		private void Button_Clicked(object sender, EventArgs e)
		{
			scrollView.Content = GetHorizontalLinearGauges();
			button1.Background = new SolidColorBrush(Color.FromRgb(0, 116, 227));
			button1.TextColor = Colors.White;
			button2.TextColor = Colors.Black;
			button2.Background = new SolidColorBrush(Colors.White);
		}

		private void Button_Clicked_1(object sender, EventArgs e)
		{
			scrollView.Content = GetVerticalLinearGauges();
			button1.Background = new SolidColorBrush(Colors.White);
			button2.Background = new SolidColorBrush(Color.FromRgb(0, 116, 227));
			button2.TextColor = Colors.White;
			button1.TextColor = Colors.Black;
		}

		private VerticalStackLayout GetHorizontalLinearGauges()
		{
			VerticalStackLayout verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 350;

			Label label = new Label();
			label.Text = "Custom labels";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			LinearGauge sfLinearGauge = new LinearGauge();
			sfLinearGauge.Minimum = 5;
			sfLinearGauge.Maximum = 20;
			sfLinearGauge.Interval = 5;
            sfLinearGauge.LabelCreated += Custom_LabelCreated;
			sfLinearGauge.EnableAxisAnimation = true;
			verticalStackLayout.Children.Add(sfLinearGauge);

			label = new Label();
			label.Text = "Label offset";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 40, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.EnableAxisAnimation = true;
			sfLinearGauge.LabelOffset = 20;
			verticalStackLayout.Children.Add(sfLinearGauge);

			label = new Label();
			label.Text = "Text labels";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 40, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.Minimum = 5;
			sfLinearGauge.Maximum = 20;
			sfLinearGauge.Interval = 5;
			sfLinearGauge.ShowTicks = false;
			sfLinearGauge.LabelOffset = 20;
			sfLinearGauge.MinorTicksPerInterval = 0;
			sfLinearGauge.LineStyle.Fill = new SolidColorBrush(Colors.LightGray);
			sfLinearGauge.LabelCreated += Custom_LabelCreated1;
			sfLinearGauge.BarPointers.Add(new BarPointer()
			{
				Value = 15,
				Fill = new SolidColorBrush(Color.FromRgb(5, 195, 221))
			});
			sfLinearGauge.MarkerPointers.Add(new ShapePointer()
			{
				Fill = new SolidColorBrush(Color.FromRgb(5, 195, 221)),
				ShapeType = MarkerType.Circle,
				Position = GaugeElementPosition.Cross,
				Value = 5
			});
			sfLinearGauge.MarkerPointers.Add(new ShapePointer()
			{
				Fill = new SolidColorBrush(Color.FromRgb(5, 195, 221)),
				ShapeType = MarkerType.Circle,
				Position = GaugeElementPosition.Cross,
				Value = 10
			});
			sfLinearGauge.MarkerPointers.Add(new ShapePointer()
			{
				Fill = new SolidColorBrush(Color.FromRgb(5, 195, 221)),
				ShapeType = MarkerType.Circle,
				Position = GaugeElementPosition.Cross,
				Value = 15
			});
			sfLinearGauge.MarkerPointers.Add(new ShapePointer()
			{
				Fill = new SolidColorBrush(Colors.LightGray),
				ShapeType = MarkerType.Circle,
				Position = GaugeElementPosition.Cross,
				Value = 20
			});
			verticalStackLayout.Children.Add(sfLinearGauge);

			label = new Label();
			label.Text = "Label style customization";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 40, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.LabelStyle.FontSize = 10;
			sfLinearGauge.LabelStyle.TextColor = Color.FromRgb(111, 32, 240);
			verticalStackLayout.Children.Add(sfLinearGauge);

			return verticalStackLayout;

		}

        private void Custom_LabelCreated(object sender, LabelCreatedEventArgs e)
        {
			e.Text = "$" + e.Text;
        }

		private void Custom_LabelCreated1(object sender, LabelCreatedEventArgs e)
		{
			if (e.Text == "5")
				e.Text = "Ordered";
			else if (e.Text == "10")
				e.Text = "Packed";
			else if (e.Text == "15")
				e.Text = "Shipped";
			else if (e.Text == "20")
				e.Text = "Delivered";
		}

		private WrapLayout GetVerticalLinearGauges()
		{
			WrapLayout layout = new WrapLayout();
			layout.WidthRequest = 300;

			VerticalStackLayout verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 150;
			Label label = new Label();
			label.Text = "Custom labels";
			label.HorizontalOptions = LayoutOptions.Center;
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			LinearGauge sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			sfLinearGauge.Minimum = 5;
			sfLinearGauge.Maximum = 20;
			sfLinearGauge.Interval = 5;
			sfLinearGauge.LabelCreated += Custom_LabelCreated;
			sfLinearGauge.EnableAxisAnimation = true;
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 150;
			label = new Label();
			label.Text = "Label offset";
			label.FontSize = 14;
			label.HorizontalOptions = LayoutOptions.Center;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			sfLinearGauge.EnableAxisAnimation = true;
			sfLinearGauge.LabelOffset = 20;
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 150;
			label = new Label();
			label.Text = "Text labels";
			label.HorizontalOptions = LayoutOptions.Center;
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			sfLinearGauge.Minimum = 5;
			sfLinearGauge.Maximum = 20;
			sfLinearGauge.Interval = 5;
			sfLinearGauge.ShowTicks = false;
			sfLinearGauge.LabelOffset = 20;
			sfLinearGauge.LineStyle.Fill = new SolidColorBrush(Colors.LightGray);
			sfLinearGauge.MinorTicksPerInterval = 0;
			sfLinearGauge.LabelCreated += Custom_LabelCreated1;
			sfLinearGauge.BarPointers.Add(new BarPointer()
			{
				Value = 15,
				Fill = new SolidColorBrush(Color.FromRgb(5, 195, 221))
			});
			sfLinearGauge.MarkerPointers.Add(new ShapePointer()
			{
				Fill = new SolidColorBrush(Color.FromRgb(5, 195, 221)),
				ShapeType = MarkerType.Circle,
				Position = GaugeElementPosition.Cross,
				Value = 5
			});
			sfLinearGauge.MarkerPointers.Add(new ShapePointer()
			{
				Fill = new SolidColorBrush(Color.FromRgb(5, 195, 221)),
				ShapeType = MarkerType.Circle,
				Position = GaugeElementPosition.Cross,
				Value = 10
			});
			sfLinearGauge.MarkerPointers.Add(new ShapePointer()
			{
				Fill = new SolidColorBrush(Color.FromRgb(5, 195, 221)),
				ShapeType = MarkerType.Circle,
				Position = GaugeElementPosition.Cross,
				Value = 15
			});
			sfLinearGauge.MarkerPointers.Add(new ShapePointer()
			{
				Fill = new SolidColorBrush(Colors.LightGray),
				ShapeType = MarkerType.Circle,
				Position = GaugeElementPosition.Cross,
				Value = 20
			});
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 150;
			label = new Label();
			label.Text = "Label style customization";
			label.HorizontalOptions = LayoutOptions.Center;
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			sfLinearGauge.LabelStyle.FontSize = 10;
			sfLinearGauge.LabelStyle.TextColor = Color.FromRgb(111, 32, 240);
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			return layout;

		}
	}
}
