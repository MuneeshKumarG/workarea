using Microsoft.Maui.Controls;
using Syncfusion.Maui.Sliders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using SampleBrowser.Maui.Core;
using Syncfusion.Maui.Gauges;

namespace SampleBrowser.Maui.RadialRangeSlider
{
    public partial class LabelsAndTicksRadialRangeSlider : SampleView
    {
        public LabelsAndTicksRadialRangeSlider()
        {
            InitializeComponent();
        }

		private void clockMarkerPointer1_ValueChanging(object sender, ValueChangingEventArgs e)
		{
			if (Math.Abs(e.NewValue - e.OldValue) > 2.4 || e.NewValue >= clockMarkerPointer2.Value ||
			Math.Abs(e.NewValue - clockMarkerPointer1.Value) > 1.2)
				e.Cancel = true;
			else
				UpdateClockAnnotationLabel(e.NewValue, true);
		}

		private void clockMarkerPointer2_ValueChanging(object sender, ValueChangingEventArgs e)
		{
			if (Math.Abs(e.NewValue - e.OldValue) > 2.4 || e.NewValue <= clockMarkerPointer1.Value ||
			Math.Abs(e.NewValue - clockMarkerPointer2.Value) > 1.2)
				e.Cancel = true;
			else
				UpdateClockAnnotationLabel(e.NewValue, false);
		}

		private void UpdateClockAnnotationLabel(double newValue, bool isFirstMarker)
		{
			double value = newValue;
			value = value > 6 ? Math.Ceiling(value) : Math.Floor(value);

			if (isFirstMarker)
			{
				double secondValue = (int)clockMarkerPointer2.Value == 0 ? 12 : (int)clockMarkerPointer2.Value;
				clockAnnotationLabel.Text = (value == 0 ? 12 : value) + " AM - " + secondValue + " AM";
			}
			else
			{
				double firstValue = (int)clockMarkerPointer1.Value == 0 ? 12 : (int)clockMarkerPointer1.Value;
				clockAnnotationLabel.Text = firstValue + " AM - " + (value == 0 ? 12 : value) + " AM";
			}
		}
	}
}