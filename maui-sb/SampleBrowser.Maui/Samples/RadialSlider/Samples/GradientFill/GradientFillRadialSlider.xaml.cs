using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using SampleBrowser.Maui.Core;
using Syncfusion.Maui.Gauges;
using Syncfusion.Maui.Sliders;

namespace SampleBrowser.Maui.RadialSlider
{
    public partial class GradientFillRadialSlider : SampleView
    {
        public GradientFillRadialSlider()
        {
            InitializeComponent();

			fahrenheitAnnotationLabel.Text = "60°F";
			celsiusAnnotationLabel.Text = "15.6°C";
		}

		private void gradientMarker_ValueChanging(object sender, ValueChangingEventArgs e)
		{
			double value = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);
			fahrenheitAnnotationLabel.Text = value.ToString() + "°F";
			celsiusAnnotationLabel.Text = Math.Round((value - 32) / 1.8, 1).ToString() + "°C";
		}

		private void RadialAxis_LabelCreated(object sender, LabelCreatedEventArgs e)
		{
			double axisValue = double.Parse(e.Text);
			double celsiusValue = (axisValue - 32) / 1.8;
			e.Text = Math.Round(celsiusValue, 1).ToString();
		}
	}
}
