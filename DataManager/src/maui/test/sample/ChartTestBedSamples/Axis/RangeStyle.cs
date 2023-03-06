using System.Collections.ObjectModel;
using ChartTestBedSamples.Axis;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Charts;

namespace ChartTestBedSamples
{
	public partial class RangeStyle : ContentPage
	{
		public RangeStyle()
		{
			InitializeComponent();
		}
	}

	public class RangeStyleViewModel
    {
		public ObservableCollection<ChartDataModel> LiveData1 { get; set; }
		public ObservableCollection<ChartDataModel> LiveData2 { get; set; }

		public ObservableCollection<ChartDataModel> SplineAreaData1 { get; set; }

		public ObservableCollection<ChartDataModel> SplineAreaData2 { get; set; }

		public ObservableCollection<ChartDataModel> SplineData1 { get; set; }

		public ObservableCollection<ChartDataModel> SplineData2 { get; set; }


		public RangeStyleViewModel()
        {
			LiveData1 = new ObservableCollection<ChartDataModel>();
			LiveData2 = new ObservableCollection<ChartDataModel>();

			SplineData1 = new ObservableCollection<ChartDataModel>
			{
				new ChartDataModel("Sun", 15),
				new ChartDataModel("Mon", 22),
				new ChartDataModel("Tue", 32),
				new ChartDataModel("Wed", 31),
				new ChartDataModel("Thu", 29),
				new ChartDataModel("Fri", 26),
				new ChartDataModel("Sat", 18),
			};

			SplineData2 = new ObservableCollection<ChartDataModel>
			{
				new ChartDataModel("Sun", 10),
				new ChartDataModel("Mon", 18),
				new ChartDataModel("Tue", 28),
				new ChartDataModel("Wed", 28),
				new ChartDataModel("Thu", 26),
				new ChartDataModel("Fri", 20),
				new ChartDataModel("Sat", 15),
			};

			SplineAreaData1 = new ObservableCollection<ChartDataModel>
			{
				new ChartDataModel(2002, 2.2),
				new ChartDataModel(2003, 3.4),
				new ChartDataModel(2004, 2.8),
				new ChartDataModel(2005, 1.6),
				new ChartDataModel(2006, 2.3),
				new ChartDataModel(2007, 2.5),
				new ChartDataModel(2008, 2.9),
				new ChartDataModel(2009, 3.8),
				new ChartDataModel(2010, 1.4),
				new ChartDataModel(2011, 3.1),
			};

			SplineAreaData2 = new ObservableCollection<ChartDataModel>
			{
				new ChartDataModel(2002, 2.0),
				new ChartDataModel(2003, 1.7),
				new ChartDataModel(2004, 1.8),
				new ChartDataModel(2005, 2.1),
				new ChartDataModel(2006, 2.3),
				new ChartDataModel(2007, 1.7),
				new ChartDataModel(2008, 1.5),
				new ChartDataModel(2009, 2.8),
				new ChartDataModel(2010, 1.5),
				new ChartDataModel(2011, 2.3),

			};

			Random r = new Random();

			for (int i = 0; i < 40; i++)
			{
				var rnd = r.Next(40, 110);
				LiveData1.Add(new ChartDataModel(i, rnd));
				LiveData2.Add(new ChartDataModel(i, rnd - r.Next(20,40)));
			}
        }
	}

	public class CustomPrimaryAxis : NumericalAxis
    {
		protected override void DrawGridLine(ICanvas canvas, double position, float x1, float y1, float x2, float y2)
		{
			if (position == 2 || position == 8)
			{
				canvas.SaveState();
				canvas.StrokeSize = 1;
				canvas.StrokeColor = Colors.Blue;
				canvas.StrokeDashPattern = new float[] { 6, 5, 3, 2 };
				base.DrawGridLine(canvas, position, x1, y1, x2, y2);
				canvas.RestoreState();
			}
			else
				base.DrawGridLine(canvas, position, x1, y1, x2, y2);
		}


		protected override void DrawMajorTick(ICanvas canvas, double position, PointF point1, PointF point2)
		{
			if (position == 2 || position == 8)
			{
				canvas.SaveState();
				canvas.FontColor = Colors.Red;
				canvas.FontSize = 10;
				if (position == 2)
					canvas.DrawString("<>", point2.X, point2.Y + 1, HorizontalAlignment.Center);
				else
					canvas.DrawString("<>", point2.X, point2.Y + 1, HorizontalAlignment.Center);
				//base.OnDrawMajorTicks(position, canvas, point1, point2);
				canvas.RestoreState();
			}
			else
				base.DrawMajorTick(canvas, position, point1, point2);
		}

		protected override void OnLabelCreated(ChartAxisLabel label)
        {
			if (label.Position == 2)
			{
				label.Content = "start";
				label.LabelStyle = new ChartAxisLabelStyle() { TextColor = Colors.Blue };
			}

			if (label.Position == 8)
			{
				label.Content = "end";
				label.LabelStyle = new ChartAxisLabelStyle() { TextColor = Colors.Blue };
			}


			base.OnLabelCreated(label);
        }

    }

	public class CustomNumericalAxis : NumericalAxis
    {
        protected override void DrawGridLine(ICanvas canvas, double position, float x1, float y1, float x2, float y2)
        {
			if (position == 50 || position == 100 || position == 70)
			{
				canvas.SaveState();
				canvas.StrokeSize = 1;
				if (position == 70)
				{
					canvas.StrokeColor = Colors.Green;
					canvas.StrokeDashPattern = new float[] { 6, 5, 3, 2 };
				}
				else
					canvas.StrokeColor = Colors.Red;

				base.DrawGridLine(canvas, position, x1, y1, x2, y2);
				canvas.RestoreState();
			}
			else
				base.DrawGridLine(canvas, position, x1, y1, x2, y2);
        }

        protected override void OnLabelCreated(ChartAxisLabel label)
        {
			if (label.Position == 50)
			{
				label.Content = "Low";
				label.LabelStyle = new ChartAxisLabelStyle() { TextColor = Colors.Red };
			}

			if (label.Position == 100)
			{
				label.Content = "High";
				label.LabelStyle = new ChartAxisLabelStyle() { TextColor = Colors.Red };
			}

			base.OnLabelCreated(label);
        }

    }

}
