using System;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Gauges;

namespace LinearGaugeTestbed
{
	public partial class Page2 : ContentPage
	{
		LinearGradientBrush linearGradientBrush = new LinearGradientBrush();
		SfRadialGauge sfRadialGauge;
		GaugeAnnotation gaugeAnnotation;

		public List<string> ColorCollection
		{
			get
			{
				return new List<string>()
				{
					"Red",
					"Black",
					"Green",
					"Blue",
					"Transparent",
					"Gradient",
					"Null"
				};
			}
		}

		public Page2()
		{
			this.BindingContext = this;
			InitializeComponent();

		}

		private void OverlayRadiusIncrease_Clicked(object sender, EventArgs e)
		{
			if (double.IsNaN(marker.OverlayRadius))
				marker.OverlayRadius = marker.MarkerWidth/2;

			marker.OverlayRadius += 1;
		}

		private void OverlayRadiusDecrease_Clicked(object sender, EventArgs e)
		{
			if (double.IsNaN(marker.OverlayRadius))
				marker.OverlayRadius = marker.MarkerWidth / 2;

			marker.OverlayRadius -= 1;
		}

		private void OverlayRadiusNull_Clicked(object sender, EventArgs e)
		{
			marker.OverlayRadius = double.NaN;
		}

		private void OverlayRadiusZero_Clicked(object sender, EventArgs e)
		{
			marker.OverlayRadius = 0;
		}

		private void Picker_SelectedIndexChanged1(object sender, EventArgs e)
		{
			string color = (sender as Picker).SelectedItem.ToString();

			if (color == "Red")
				marker.OverlayFill = new SolidColorBrush(Color.FromRgba(1,0,0,0.5));
			else if (color == "Black")
				marker.OverlayFill = new SolidColorBrush(Colors.Black);
			else if (color == "Green")
				marker.OverlayFill = new SolidColorBrush(Color.FromRgba(0, 1, 0, 0.5));
			else if (color == "Blue")
				marker.OverlayFill = new SolidColorBrush(Color.FromRgba(0, 0, 1, 0.5));
			else if (color == "Transparent")
				marker.OverlayFill = new SolidColorBrush(Colors.Transparent);
			else if (color == "Null")
				marker.OverlayFill = null;
			else if (color == "Gradient")
			{
				linearGradientBrush.StartPoint = new Point(0, 0.5);
				linearGradientBrush.EndPoint = new Point(1, 0.5);
				linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Red, 0.25f));
				linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Green, 0.75f));

				marker.OverlayFill = linearGradientBrush;
			}
		}
	}

	
}
