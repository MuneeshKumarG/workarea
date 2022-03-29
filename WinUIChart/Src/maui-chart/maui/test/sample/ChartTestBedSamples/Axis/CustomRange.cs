using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ChartTestBedSamples.Axis;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Charts;

namespace ChartTestBedSamples
{
	public partial class CustomRange : ContentPage
	{
		public CustomRange()
		{
			InitializeComponent();
            //chart.Axes.Add(new CustomXAxis("Actual Range Changed"));
            //chart.Axes.Add(new CustomXAxis1("Visible Range Changed"));
        }
    }

    public class CustomYAxis : NumericalAxis
    {
        protected override Size ComputeDesiredSize(Size availableSize)
        {
            return base.ComputeDesiredSize(availableSize);
            //return new Size(size.Width + 40, size.Height);
        }
    }


    public class CustomXAxis : NumericalAxis
    {
        public CustomXAxis(string text) : base()
        {
            Title = new ChartAxisTitle() { Text = text };
            ShowMajorGridLines = false;
            EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Fit;
        }

        protected override DoubleRange CalculateActualRange()
        {
            return new DoubleRange(- 2, 12);
        }
    }

    public class CustomXAxis1 : NumericalAxis
    {
        public CustomXAxis1(string text) : base()
        {
            Title = new ChartAxisTitle() { Text = text };
            ShowMajorGridLines = false;
            EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Fit;
        }
        protected override DoubleRange CalculateVisibleRange(DoubleRange range, Size availableSize)
        {
            return new DoubleRange(2, 8);
        }

        protected override double CalculateVisibleInterval(DoubleRange visibleRange, Size FavailableSize)
        {
            return 0.5;
        }
    }
}
