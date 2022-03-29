using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Charts;

namespace ChartTestBedSamples
{
	public partial class Custom_DataLabel : ContentPage
	{

		public Custom_DataLabel()
		{
			InitializeComponent();
		}
    }

	public class SplineSeriesExt : SplineSeries
    {
        protected override void DrawDataLabel(ICanvas canvas, string label, PointF point, int index)
        {
            double value = Convert.ToDouble(label);

            if (value < 40)
            {
                canvas.FillColor = Colors.Red;
                canvas.FontColor = Colors.White;
                canvas.SetToSystemFont();
            }
            else
            {
                canvas.FillColor = Colors.DarkGreen;
                canvas.FontColor = Colors.White;
                canvas.SetToBoldSystemFont();
            }

            base.DrawDataLabel(canvas, label, point, index);
        }
    }

	public class NumericalAxisExt : NumericalAxis
	{
		protected override void DrawGridLine(ICanvas canvas, double position, float x1, float y1, float x2, float y2)
		{
			if (position == 40)
			{
				canvas.SaveState();
				canvas.StrokeSize = 2;
				canvas.StrokeColor = Colors.Blue;

				base.DrawGridLine(canvas, position, x1, y1, x2, y2);
				canvas.RestoreState();
			}
			else
				base.DrawGridLine(canvas, position, x1, y1, x2, y2);
		}

		protected override void OnLabelCreated(ChartAxisLabel label)
		{
			if (label.Position == 40)
			{
				label.Content = "Peak";
				label.LabelStyle = new ChartAxisLabelStyle() { TextColor = Colors.Blue };
			}

			base.OnLabelCreated(label);
		}

	}

}
