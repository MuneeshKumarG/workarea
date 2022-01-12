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
}
