using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace SampleBrowser.Maui.Chart
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
		}
		int count = 0;
		private void OnCounterClicked(object sender, EventArgs e)
		{
			count++;
			CounterLabel.Text = $"Current count: {count}";
		}
	}
}
