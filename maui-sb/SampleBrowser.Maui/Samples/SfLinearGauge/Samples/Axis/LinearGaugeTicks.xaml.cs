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
    public partial class LinearGaugeTicks : SampleView
    {
       
        public LinearGaugeTicks()
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
			label.Text = "Outside ticks";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			LinearGauge sfLinearGauge = new LinearGauge();
			sfLinearGauge.TickPosition = GaugeElementPosition.Outside;
			sfLinearGauge.LabelPosition = GaugeLabelsPosition.Outside;
			sfLinearGauge.EnableAxisAnimation = true;
			verticalStackLayout.Children.Add(sfLinearGauge);

			label = new Label();
			label.Text = "Cross ticks";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 40, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.IsMirrored = true;
			sfLinearGauge.TickPosition = GaugeElementPosition.Cross;
			sfLinearGauge.EnableAxisAnimation = true;
			verticalStackLayout.Children.Add(sfLinearGauge);

			label = new Label();
			label.Text = "Inside ticks";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 40, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.EnableAxisAnimation = true;
			verticalStackLayout.Children.Add(sfLinearGauge);

			label = new Label();
			label.Text = "Ticks with offset";
			label.FontSize = 14;
			label.Margin = new Thickness(0, 40, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.EnableAxisAnimation = true;
			sfLinearGauge.TickOffset = 20;
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
			label.Text = "Outside ticks";
			label.HorizontalOptions = LayoutOptions.Center;
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			LinearGauge sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			sfLinearGauge.TickPosition = GaugeElementPosition.Outside;
			sfLinearGauge.LabelPosition = GaugeLabelsPosition.Outside;
			sfLinearGauge.EnableAxisAnimation = true;
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 150;
			label = new Label();
			label.Text = "Cross ticks";
			label.FontSize = 14;
			label.HorizontalOptions = LayoutOptions.Center;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			sfLinearGauge.TickPosition = GaugeElementPosition.Cross;
			sfLinearGauge.LabelPosition = GaugeLabelsPosition.Outside;
			sfLinearGauge.EnableAxisAnimation = true;
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 150;
			label = new Label();
			label.Text = "Inside ticks";
			label.HorizontalOptions = LayoutOptions.Center;
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			sfLinearGauge.EnableAxisAnimation = true;
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			verticalStackLayout = new VerticalStackLayout();
			verticalStackLayout.WidthRequest = 150;
			label = new Label();
			label.Text = "Ticks with offset";
			label.HorizontalOptions = LayoutOptions.Center;
			label.FontSize = 14;
			label.Margin = new Thickness(0, 0, 0, 10);
			verticalStackLayout.Add(label);
			sfLinearGauge = new LinearGauge();
			sfLinearGauge.HorizontalOptions = LayoutOptions.Center;
			sfLinearGauge.Orientation = GaugeOrientation.Vertical;
			sfLinearGauge.TickOffset = 20;
			sfLinearGauge.EnableAxisAnimation = true;
			verticalStackLayout.Children.Add(sfLinearGauge);
			layout.Children.Add(verticalStackLayout);

			return layout;

		}
	}
}
