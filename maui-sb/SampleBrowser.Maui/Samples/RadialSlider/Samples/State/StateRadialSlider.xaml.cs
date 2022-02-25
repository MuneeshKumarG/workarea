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

namespace SampleBrowser.Maui.RadialSlider
{
    public partial class StateRadialSlider : SampleView
    {
        public StateRadialSlider()
        {
            InitializeComponent();
        }

		private void markerPointer_ValueChanging(object sender, ValueChangingEventArgs e)
		{
			if (Math.Abs(e.NewValue - e.OldValue) > 20)
				e.Cancel = true;
			else
			{
				double value = e.NewValue;
				value = value > 50 ? Math.Ceiling(value) : Math.Floor(value);

				annotationLabel.Text = value.ToString() + "%";
			}
		}
	}
}
