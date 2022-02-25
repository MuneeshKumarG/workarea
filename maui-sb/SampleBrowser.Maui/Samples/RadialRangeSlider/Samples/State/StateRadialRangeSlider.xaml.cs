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

namespace SampleBrowser.Maui.RadialRangeSlider
{
    public partial class StateRadialRangeSlider : SampleView
    {
        public StateRadialRangeSlider()
        {
            InitializeComponent();
        }

		private void markerPointer_ValueChanging1(object sender, ValueChangingEventArgs e)
		{
			if (Math.Abs(e.NewValue - e.OldValue) > 20 || e.NewValue >= markerPointer2.Value ||
			Math.Abs(e.NewValue - markerPointer1.Value) > 10)
				e.Cancel = true;
			else
				UpdateAnnotationLabel(e.NewValue, true);
		}

		private void markerPointer_ValueChanging2(object sender, ValueChangingEventArgs e)
		{
			if (Math.Abs(e.NewValue - e.OldValue) > 20 || e.NewValue <= markerPointer1.Value ||
			Math.Abs(e.NewValue - markerPointer2.Value) > 10)
				e.Cancel = true;
			else
				UpdateAnnotationLabel(e.NewValue, false);
		}

		private void UpdateAnnotationLabel(double newValue, bool isFirstMarker)
		{
			double value = newValue;
			value = value > 50 ? Math.Ceiling(value) : Math.Floor(value);

			if (isFirstMarker)
				annotationLabel.Text = value.ToString() + " - " + (int)markerPointer2.Value + "%";
			else
				annotationLabel.Text = (int)markerPointer1.Value + " - " + value.ToString() + "%";
		}
	}
}
