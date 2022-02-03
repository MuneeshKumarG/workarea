using System;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;

namespace LinearGaugeTestbed
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void button1_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Page1());
		}

		private void button2_Clicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Page2());
		}
	}

	public class GraphicsDrawable : IDrawable
	{
		public void Draw(ICanvas canvas, RectangleF dirtyRect)
		{
			canvas.SaveState();


			canvas.FillColor = Colors.CornflowerBlue;
			canvas.SetShadow(new SizeF(50, 50), 5, Colors.Grey);
			canvas.FillRectangle(50, 100, 50, 50);

			canvas.RestoreState();
		}
	}
}
