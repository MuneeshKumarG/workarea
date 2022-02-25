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

namespace SampleBrowser.Maui.RadialSlider
{
    public partial class LabelsAndTicksRadialSlider : SampleView
    {
        public LabelsAndTicksRadialSlider()
        {
            InitializeComponent();
        }

		private void labelsAndTicksPointer_ValueChanging(object sender, ValueChangingEventArgs e)
		{
			if (Math.Abs(e.NewValue - e.OldValue) > 2.4)
				e.Cancel = true;
			else
			{
				double value = e.NewValue;
				value = value > 6 ? Math.Ceiling(value) : Math.Floor(value);
				clockAnnotationLabel.Text = value.ToString() + " hrs";
			}
		}
	}
}