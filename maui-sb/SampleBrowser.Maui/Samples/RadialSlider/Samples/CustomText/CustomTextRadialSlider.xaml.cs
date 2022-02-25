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
using Syncfusion.Maui.Sliders;

namespace SampleBrowser.Maui.RadialSlider
{
    public partial class CustomTextRadialSlider : SampleView
    {
        public CustomTextRadialSlider()
        {
            InitializeComponent();
        }

		private void customTextMarker_ValueChanged(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
		{
			if (e.Value > 99)
			{
				Brush brush = new SolidColorBrush(Color.FromRgb(0, 168, 181));
				(customTextAxis.Pointers[0] as RangePointer).Fill = brush;
				(customTextAxis.Pointers[1] as MarkerPointer).Stroke = Color.FromRgb(0, 168, 181);
				customTextAnnotation.Text = "Done";

			}
			else
			{
				Brush brush = new SolidColorBrush(Colors.Orange);
				(customTextAxis.Pointers[0] as RangePointer).Fill = brush;
				(customTextAxis.Pointers[1] as MarkerPointer).Stroke = Colors.Orange;
				customTextAnnotation.Text = "In-progress";
			}
		}
	}
}