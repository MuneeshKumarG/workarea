using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using SampleBrowser.Maui.Core;
using Syncfusion.Maui.Gauges;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SampleBrowser.Maui.RadialRangeSlider
{
    public partial class CustomTextRadialRangeSlider : SampleView
    {
        public CustomTextRadialRangeSlider()
        {
            InitializeComponent();
        }

		private void customTextMarker_ValueChanging1(object sender, ValueChangingEventArgs e)
		{
			if (e.NewValue >= customTextMarker2.Value || Math.Abs(e.NewValue - customTextMarker1.Value) > 10)
				e.Cancel = true;
			else
				UpdateCustomTextAnnotationLabel(Math.Abs(customTextMarker2.Value - customTextMarker1.Value));
		}

		private void customTextMarker_ValueChanging2(object sender, ValueChangingEventArgs e)
		{
			if (e.NewValue <= customTextMarker1.Value || Math.Abs(e.NewValue - customTextMarker2.Value) > 10)
				e.Cancel = true;
			else
				UpdateCustomTextAnnotationLabel(Math.Abs(customTextMarker2.Value - customTextMarker1.Value));
		}

		private void UpdateCustomTextAnnotationLabel(double value)
		{
			if (value > 99)
			{
				Brush brush = new SolidColorBrush(Color.FromRgb(0, 168, 181));
				customTextAxis.Ranges[0].Fill = brush;
				(customTextAxis.Pointers[0] as MarkerPointer).Stroke = Color.FromRgb(0, 168, 181);
				(customTextAxis.Pointers[1] as MarkerPointer).Stroke = Color.FromRgb(0, 168, 181);
				customTextAnnotation.Text = "Done";

			}
			else
			{
				Brush brush = new SolidColorBrush(Colors.Orange);
				customTextAxis.Ranges[0].Fill = brush;
				(customTextAxis.Pointers[0] as MarkerPointer).Stroke = Colors.Orange;
				(customTextAxis.Pointers[1] as MarkerPointer).Stroke = Colors.Orange;
				customTextAnnotation.Text = "In-progress";
			}
		}
	}
}