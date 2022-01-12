using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Gauges;

namespace LinearGaugeTestbed
{
	public partial class Page2 : ContentPage
	{
		public Page2()
		{
			InitializeComponent();

            orientation.Clicked += Orientation_Clicked;
		}

        private void Orientation_Clicked(object sender, EventArgs e)
        {
			if (gauge.Orientation == GaugeOrientation.Horizontal)
				gauge.Orientation = GaugeOrientation.Vertical;
			else
				gauge.Orientation = GaugeOrientation.Horizontal;
		}
    }
}
