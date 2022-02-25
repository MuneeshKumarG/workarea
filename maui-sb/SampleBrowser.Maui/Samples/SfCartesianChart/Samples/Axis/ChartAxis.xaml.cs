﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using SampleBrowser.Maui.Core;
using Syncfusion.Maui.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleBrowser.Maui.SfCartesianChart
{
    public partial class ChartAxis : SampleView
    {
        int month = int.MaxValue;

        public ChartAxis()
        {
            InitializeComponent();
            dateTimeChart.PrimaryAxis.LabelCreated += Primary_LabelCreated;
        }

        public override void OnScrollingToNewCardViewExt(CardViewExt cardViewExt)
        {
            var content = cardViewExt.MainContent as Syncfusion.Maui.Charts.SfCartesianChart;
            content.AnimateSeries();
        }

        public override void OnExpandedViewAppearing(View view)
        {
            base.OnExpandedViewAppearing(view);

            if (view is Syncfusion.Maui.Charts.SfCartesianChart cartesianChart)
            {
                foreach (var item in cartesianChart.ChartBehaviors)
                {
                    if (item is ChartZoomPanBehavior behavior)
                    {
                        if (behavior.ZoomMode == ZoomMode.X)
                        {
                            behavior.EnablePanning = true;
                        }
                        else
                        {
                            behavior.EnablePinchZooming = behavior.EnableDoubleTap = behavior.EnablePanning = true;
                        }
                    }
                }
            }
        }

        public override void OnExpandedViewDisappearing(View view)
        {
            base.OnExpandedViewDisappearing(view);

            zooming.EnableDoubleTap = zooming.EnablePanning = zooming.EnablePinchZooming = false;
            zooming1.EnablePanning = false;

            view.Handler?.DisconnectHandler();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            categoryChart.Handler?.DisconnectHandler();
            dateTimeChart.Handler?.DisconnectHandler();
            numericChart.Handler?.DisconnectHandler();
            axisCrossingChart.Handler?.DisconnectHandler();
        }

        private void Primary_LabelCreated(object sender, ChartAxisLabelEventArgs e)
        {
            DateTime baseDate = new DateTime(1899, 12, 30);
            var date = baseDate.AddDays(e.Position);
            if (date.Month != month)
            {
                ChartAxisLabelStyle labelStyle = new ChartAxisLabelStyle();
                labelStyle.LabelFormat = "MMM-dd";
                labelStyle.FontAttributes = FontAttributes.Bold;
                e.LabelStyle = labelStyle;

                month = date.Month;
            }
            else
            {
                ChartAxisLabelStyle labelStyle = new ChartAxisLabelStyle();
                labelStyle.LabelFormat = "dd";
                e.LabelStyle = labelStyle;
            }
        }
    }
}
