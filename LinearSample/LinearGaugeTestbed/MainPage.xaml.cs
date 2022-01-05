using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;

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
    }
}
