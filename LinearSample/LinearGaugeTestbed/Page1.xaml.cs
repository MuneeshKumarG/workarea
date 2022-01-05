using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Syncfusion.Maui.Gauges;

namespace LinearGaugeTestbed
{
	public partial class Page1 : ContentPage
	{
		public Page1()
		{
			InitializeComponent();

		}

        private void MinimumIncrease_Clicked(object sender, EventArgs e)
        {
            gauge.Minimum += 1;
        }

        private void MinimumDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.Minimum -= 1;
        }

        private void MaximumIncrease_Clicked(object sender, EventArgs e)
        {
            gauge.Maximum += 1;
        }

        private void MaximumDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.Maximum -= 1;
        }

        double interval = 10;
        private void IntervalIncrease_Clicked(object sender, EventArgs e)
        {
            interval++;
            gauge.Interval = interval;
        }

        private void IntervalDecrease_Clicked(object sender, EventArgs e)
        {
            interval--;
            gauge.Interval = interval;
        }

        private void MinorTicksPerIntervalIncrease_Clicked(object sender, EventArgs e)
        {
            gauge.MinorTicksPerInterval += 1;
        }

        private void MinorTicksPerIntervalDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.MinorTicksPerInterval -= 1;
        }

        private void MaximumLabelsCountIncrease_Clicked(object sender, EventArgs e)
        {
            gauge.MaximumLabelsCount += 1;
        }

        private void MaximumLabelsCountDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.MaximumLabelsCount -= 1;
        }

        private void LabelFormatSet_Clicked(object sender, EventArgs e)
        {
            gauge.LabelFormat = labelFormatValue.Text;
        }

        private void LabelPositionInside_Clicked(object sender, EventArgs e)
        {
            gauge.LabelPosition = GaugeLabelsPosition.Inside;
        }

        private void LabelPositionOutside_Clicked(object sender, EventArgs e)
        {
            gauge.LabelPosition = GaugeLabelsPosition.Outside;
        }

        double labelOffset = 0;
        private void LabelsOffsetIncrease_Clicked(object sender, EventArgs e)
        {
            labelOffset++;
            gauge.LabelOffset = labelOffset;
        }

        private void LabelsOffsetDecrease_Clicked(object sender, EventArgs e)
        {
            labelOffset--;
            gauge.LabelOffset = labelOffset;
        }

        private void TickPositionInside_Clicked(object sender, EventArgs e)
        {
            gauge.TickPosition = GaugeElementPosition.Inside;
        }

        private void TickPositionOutside_Clicked(object sender, EventArgs e)
        {
            gauge.TickPosition = GaugeElementPosition.Outside;
        }

        private void TickPositionCross_Clicked(object sender, EventArgs e)
        {
            gauge.TickPosition = GaugeElementPosition.Cross;
        }

        double tickOffset = 0;
        private void TickOffsetIncrease_Clicked(object sender, EventArgs e)
        {
            tickOffset++;
            gauge.TickOffset = tickOffset;
        }

        private void TickOffsetDecrease_Clicked(object sender, EventArgs e)
        {
            tickOffset--;
            gauge.TickOffset = tickOffset;
        }

        private void OrientationHorizontal_Clicked(object sender, EventArgs e)
        {
            gauge.Orientation = GaugeOrientation.Horizontal;
        }

        private void OrientationVertical_Clicked(object sender, EventArgs e)
        {
            gauge.Orientation = GaugeOrientation.Vertical;
        }
    }
}
