﻿using SampleBrowser.Maui.Base;
using Syncfusion.Maui.Charts;

namespace SampleBrowser.Maui.CartesianChart.SfCartesianChart
{
    public partial class Column_Customization : SampleView
    {
        public Column_Customization()
        {
            InitializeComponent();
        }

        public override void OnAppearing()
        {
            base.OnAppearing();
            hyperLinkLayout.IsVisible = !IsCardView;
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            Chart3.Handler?.DisconnectHandler();
        }
    }
}
