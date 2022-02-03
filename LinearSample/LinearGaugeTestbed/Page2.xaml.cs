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

			SfRadialGauge sfRadialGauge = new SfRadialGauge();

			RadialAxis radialAxis = new RadialAxis();
			radialAxis.AxisLineStyle.Fill=new SolidColorBrush(Colors.Yellow);
			sfRadialGauge.Axes.Add(radialAxis);

			MarkerPointer markerPointer = new MarkerPointer();
			markerPointer.Value = 50;
			markerPointer.IsInteractive = true;
			markerPointer.MarkerType=MarkerType.Circle;
			radialAxis.Pointers.Add(markerPointer);

			this.Content = sfRadialGauge;
		}
    }

	
}
