using System;
using System.Collections;
using System.Collections.Generic;
using ChartTestBedSamples.Axis;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Charts;

namespace ChartTestBedSamples
{
	public partial class LiveUpdate : ContentPage
	{
		LiveUpdateViewModel viewModel;
		public LiveUpdate()
		{
			InitializeComponent();
			viewModel = Chart.BindingContext as LiveUpdateViewModel;
			viewModel.UpdateLiveData();
			(Chart.SecondaryAxis as NumericalAxis).Maximum = 4;
			(Chart.SecondaryAxis as NumericalAxis).Minimum = -4;

			//start_button.Clicked += Rg_button_Clicked;
   //         stop_button.Clicked += Stop_button_Clicked;
		}

        private void Stop_button_Clicked(object sender, EventArgs e)
        {
			viewModel.StopTimer();
        }

        private void Rg_button_Clicked(object sender, EventArgs e)
		{
			viewModel.StartTimer();
		}



        protected override void OnDisappearing()
        {
			viewModel.StopTimer();
			base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

			viewModel.StartTimer();
		}
    }
}
