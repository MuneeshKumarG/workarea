﻿using System;
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

namespace SampleBrowser.Maui.RadialRangeSlider
{
    public partial class GradientFillRadialRangeSlider : SampleView
    {
        public GradientFillRadialRangeSlider()
        {
            InitializeComponent();
			fahrenheitAnnotationLabel.Text = "0°F to 60°F";
			celsiusAnnotationLabel.Text = "-17.8°C to 15.6°C";
		}

		private void RadialAxis_LabelCreated(object sender, LabelCreatedEventArgs e)
		{
			double axisValue = double.Parse(e.Text);
			double celsiusValue = (axisValue - 32) / 1.8;
			e.Text = Math.Round(celsiusValue, 1).ToString();
		}

		private void gradientMarker1_ValueChanging(object sender, ValueChangingEventArgs e)
		{
			if (e.NewValue >= gradientMarker2.Value || Math.Abs(e.NewValue - gradientMarker1.Value) > 10)
				e.Cancel = true;
			else
			{
				double firstMarkerValue = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);
				double secondMarkerValue = (int)gradientMarker2.Value;

				UpdateGradientAnnotationLabel(firstMarkerValue, secondMarkerValue);
			}
		}

		private void gradientMarker2_ValueChanging(object sender, ValueChangingEventArgs e)
		{
			if (e.NewValue <= gradientMarker1.Value || Math.Abs(e.NewValue - gradientMarker2.Value) > 10)
				e.Cancel = true;
			else
			{
				double firstMarkerValue = (int)gradientMarker1.Value;
				double secondMarkerValue = e.NewValue > 50 ? Math.Ceiling(e.NewValue) : Math.Floor(e.NewValue);

				UpdateGradientAnnotationLabel(firstMarkerValue, secondMarkerValue);
			}
		}

		private void UpdateGradientAnnotationLabel(double firstMarkerValue, double secondMarkerValue)
		{
			fahrenheitAnnotationLabel.Text = firstMarkerValue.ToString() + "°F to " + secondMarkerValue + "°F";
			celsiusAnnotationLabel.Text = Math.Round((firstMarkerValue - 32) / 1.8, 1) + "°C to " +
				Math.Round((secondMarkerValue - 32) / 1.8, 1) + "°C";
		}
	}
}
