using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using SampleBrowser.Maui.Core;
using Syncfusion.Maui.Gauges;
using System;
using System.Globalization;
using System.Threading.Tasks;
using LinearGauge = Syncfusion.Maui.Gauges.SfLinearGauge;

namespace SampleBrowser.Maui.SfLinearGauge
{
    public partial class LinearGaugeTrack : SampleView
    {
       
        public LinearGaugeTrack()
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
			button1.Background= new SolidColorBrush(Colors.White);
			button2.Background = new SolidColorBrush(Color.FromRgb(0, 116, 227));
			button2.TextColor = Colors.White;
			button1.TextColor = Colors.Black;
		}

		private VerticalStackLayout GetHorizontalLinearGauges()
		{
			VerticalStackLayout verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 350;

			Label label = new Label();
			label.Text = "Default axis";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			LinearGauge sfLinearGauge = new LinearGauge();
			verticalStackLayout.Children.Add(sfLinearGauge);

			label = new Label();
			label.Text = "Edge style";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 40, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.EnableAxisAnimation = true;
			sfLinearGauge.LineStyle.CornerStyle = CornerStyle.BothCurve;
			sfLinearGauge.LineStyle.Thickness = 20;
			verticalStackLayout.Children.Add(sfLinearGauge);

			label = new Label();
			label.Text = "Inversed axis";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 40, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.EnableAxisAnimation = true;
			sfLinearGauge.BarPointers.Add(new BarPointer()
			{
				Value = 70,
				EnableAnimation = true
			});
			sfLinearGauge.MarkerPointers.Add(new ShapePointer()
			{
				Value=70,
				EnableAnimation=true
			});
			verticalStackLayout.Children.Add(sfLinearGauge);

			label = new Label();
			label.Text = "Range color for axis";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 40, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.EnableAxisAnimation = true;
			sfLinearGauge.EnableRangeAnimation = true;
			sfLinearGauge.Ranges.Add(new LinearRange()
			{
				EndValue = 30,
				Position=GaugeElementPosition.Cross,
				Fill = new SolidColorBrush(Color.FromRgb(244, 86, 86))
			});
			sfLinearGauge.Ranges.Add(new LinearRange()
			{
				StartValue = 30,
				Position = GaugeElementPosition.Cross,
				EndValue =70,
				Fill = new SolidColorBrush(Color.FromRgb(255, 201, 62))
			});
			sfLinearGauge.Ranges.Add(new LinearRange()
			{
				StartValue = 70,
				Position = GaugeElementPosition.Cross,
				EndValue = 100,
				Fill = new SolidColorBrush(Color.FromRgb(13, 201, 171))
			});
			verticalStackLayout.Children.Add(sfLinearGauge);

			return verticalStackLayout;

		}

		private WrapLayout GetVerticalLinearGauges()
		{
			WrapLayout layout = new WrapLayout();
			layout.WidthRequest = 300;

			VerticalStackLayout verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 150;
			Label label = new Label();
			label.Text = "Default axis";
			label.HorizontalOptions = LayoutOptions.Center;
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			LinearGauge sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 150;
			label = new Label();
			label.Text = "Edge style";
			label.FontSize = 14;
			label.HorizontalOptions = LayoutOptions.Center;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			sfLinearGauge.EnableAxisAnimation = true;
			sfLinearGauge.LineStyle.CornerStyle = CornerStyle.BothCurve;
			sfLinearGauge.LineStyle.Thickness = 20;
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 150;
			label = new Label();
			label.Text = "Inversed axis";
			label.HorizontalOptions = LayoutOptions.Center;
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			sfLinearGauge.EnableAxisAnimation = true;
			sfLinearGauge.BarPointers.Add(new BarPointer()
			{
				Value = 70,
				EnableAnimation = true
			});
			sfLinearGauge.MarkerPointers.Add(new ShapePointer()
			{
				Value = 70,
				EnableAnimation = true
			});
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 150;
			label = new Label();
			label.Text = "Range color for axis";
			label.HorizontalOptions = LayoutOptions.Center;
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			sfLinearGauge.EnableAxisAnimation = true;
			sfLinearGauge.EnableRangeAnimation = true;
			sfLinearGauge.Ranges.Add(new LinearRange()
			{
				EndValue = 30,
				Position = GaugeElementPosition.Cross,
				Fill = new SolidColorBrush(Color.FromRgb(244, 86, 86))
			});
			sfLinearGauge.Ranges.Add(new LinearRange()
			{
				StartValue = 30,
				Position = GaugeElementPosition.Cross,
				EndValue = 70,
				Fill = new SolidColorBrush(Color.FromRgb(255, 201, 62))
			});
			sfLinearGauge.Ranges.Add(new LinearRange()
			{
				StartValue = 70,
				Position = GaugeElementPosition.Cross,
				EndValue = 100,
				Fill = new SolidColorBrush(Color.FromRgb(13, 201, 171))
			});
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			return layout;

		}
	}
}
