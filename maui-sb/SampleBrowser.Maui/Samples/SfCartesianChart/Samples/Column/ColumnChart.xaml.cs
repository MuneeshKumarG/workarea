﻿using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using SampleBrowser.Maui.Core;
using Chart = Syncfusion.Maui.Charts;
using Syncfusion.Maui.Graphics.Internals;
using sfChart = Syncfusion.Maui.Charts;

namespace SampleBrowser.Maui.SfCartesianChart
{
	public partial class ColumnChart : SampleView
	{
		public ColumnChart()
		{
			InitializeComponent();

			if (!RunTimeDevice.IsMobileDevice())
				viewModel.StartTimer();
		}
		public override void OnExpandedViewAppearing(View view)
		{
			base.OnExpandedViewAppearing(view);

			var content = view as Chart.SfCartesianChart;
			if (RunTimeDevice.IsMobileDevice() && content != null && content.BindingContext is DynamicAnimationViewModel)
			{
				viewModel.StopTimer();
				viewModel.StartTimer();
			}
		}

		public override void OnExpandedViewDisappearing(View view)
		{
			base.OnExpandedViewDisappearing(view);
			var content = view as Chart.SfCartesianChart;
			if (RunTimeDevice.IsMobileDevice() && content != null && content.BindingContext is DynamicAnimationViewModel)
			{
				viewModel.StopTimer();
			}

			view.Handler?.DisconnectHandler();
		}


		public override void OnScrollingToNewCardViewExt(CardViewExt cardViewExt)
		{
			if (RunTimeDevice.IsMobileDevice())
			{
				if (cardViewExt.Title == "Dynamic update animation" && viewModel != null)
				{
					viewModel.StopTimer();
					viewModel.StartTimer();
				}
				else
				{
					viewModel.StopTimer();
					var content = cardViewExt.MainContent as Syncfusion.Maui.Charts.SfCartesianChart;
					content.AnimateSeries();
				}
			}
		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			if (viewModel != null)
				viewModel.StopTimer();

            Chart.Handler?.DisconnectHandler();
			Chart1.Handler?.DisconnectHandler();
			Chart2.Handler?.DisconnectHandler();
			Chart3.Handler?.DisconnectHandler();
			Chart4.Handler?.DisconnectHandler();
		}
	}

	public class RoundedColumnSeries : Syncfusion.Maui.Charts.ColumnSeries
	{
        protected override void DrawSeries(ICanvas canvas, RectangleF clipRect)
        {
            base.DrawSeries(canvas, clipRect);

			var x = ActualXAxis.ValueToPoint(0);
			var y = ActualYAxis.ValueToPoint(50);

			canvas.SaveState();
			var color = Color.FromArgb("#F06C64");
			var text = "Overflow";
			var size = text.Measure(12);
			var texty = y - (float)size.Height;
			var textx = x + (float)size.Width + (float)size.Width / 2;

			if (!RunTimeDevice.IsMobileDevice())
			{
				textx = clipRect.Left + (2 * (float)size.Width);
			}

			canvas.StrokeColor = color;
			canvas.StrokeSize = 2;
			canvas.StrokeDashPattern = new float[] { 15, 6, 5, 3 };

			canvas.FontColor = color;
			//canvas.SetToBoldSystemFont();
			canvas.FontSize = 15;
			canvas.DrawString(text, textx, texty, HorizontalAlignment.Center);
			canvas.DrawLine(x, y, clipRect.Right, y);
			canvas.RestoreState();

		}
	}

}
