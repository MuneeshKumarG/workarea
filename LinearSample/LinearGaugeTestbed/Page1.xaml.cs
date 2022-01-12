using System;
using System.Collections.Generic;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Gauges;

namespace LinearGaugeTestbed
{
	public partial class Page1 : ContentPage
	{
        public List<string> ColorCollection
        {
            get
            {
                return new List<string>()
                {
                    "Red",
                    "Black",
                    "Green",
                    "Blue",
                    "Transparent",
                    "Gradient"
                };
            }
        }
        LinearGradientBrush linearGradientBrush = new LinearGradientBrush();

        public Page1()
		{
			InitializeComponent();
            this.BindingContext = this;

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

        private void Picker_SelectedIndexChanged1(object sender, EventArgs e)
        {
            string color = (sender as Picker).SelectedItem.ToString();

            if (color == "Red")
                gauge.AxisLineStyle.Fill = new SolidColorBrush(Colors.Red);
            else if (color == "Black")
                gauge.AxisLineStyle.Fill = new SolidColorBrush(Colors.Black);
            else if (color == "Green")
                gauge.AxisLineStyle.Fill = new SolidColorBrush(Colors.Green);
            else if (color == "Blue")
                gauge.AxisLineStyle.Fill = new SolidColorBrush(Colors.Blue);
            else if (color == "Transparent")
                gauge.AxisLineStyle.Fill = new SolidColorBrush(Colors.Transparent);
            else if (color == "Gradient")
            {
                linearGradientBrush.StartPoint = new Point(0, 0.5);
                linearGradientBrush.EndPoint = new Point(1, 0.5);
                linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Red, 0.25f));
                linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Green, 0.75f));

                gauge.AxisLineStyle.Fill = linearGradientBrush;
            }
        }

        public List<string> CornerRadiusCollection
        {
            get
            {
                return new List<string>()
                {
                    "0,5,5,5",
                    "5,0,5,5",
                    "5,5,0,5",
                    "5,5,5,0",
                    "5,5,5,5"
                };
            }
        }
        private void Picker_SelectedIndexChanged2(object sender, EventArgs e)
        {
            string radius = (sender as Picker).SelectedItem.ToString();

            if (radius == "0,5,5,5")
                gauge.AxisLineStyle.CornerRadius = new Thickness(0, 5, 5, 5);
            else if (radius == "5,0,5,5")
                gauge.AxisLineStyle.CornerRadius = new Thickness(5, 0, 5, 5);
            else if (radius == "5,5,0,5")
                gauge.AxisLineStyle.CornerRadius = new Thickness(5, 5, 0, 5);
            else if (radius == "5,5,5,0")
                gauge.AxisLineStyle.CornerRadius = new Thickness(5, 5, 5, 0);
            else if (radius == "5,5,5,5")
                gauge.AxisLineStyle.CornerRadius = new Thickness(5, 5, 5, 5);
        }

        public Array CornerStyles
        {
            get
            {
                return Enum.GetValues(typeof(CornerStyle));
            }

        }

        public Array FontAttributes
        {
            get
            {
                return Enum.GetValues(typeof(FontAttributes));
            }

        }
        private void Picker_SelectedIndexChanged3(object sender, EventArgs e)
        {
            gauge.AxisLineStyle.CornerStyle = (CornerStyle)(sender as Picker).SelectedItem;
        }

        public List<string> DashArrayCollection
        {
            get
            {
                return new List<string>()
                {
                    "2,2",
                    "4,4",
                    "Null",
                };
            }
        }
        private void Picker_SelectedIndexChanged4(object sender, EventArgs e)
        {
            string dashArray = (sender as Picker).SelectedItem.ToString();

            if (dashArray == "2,2")
                gauge.AxisLineStyle.DashArray = new DoubleCollection() {2,2 };
            else if (dashArray == "4,4")
                gauge.AxisLineStyle.DashArray = new DoubleCollection() { 4,4};
            else if (dashArray == "Null")
                gauge.AxisLineStyle.DashArray = null;
        }

        private void AxisLineThicknessIncrease_Clicked(object sender, EventArgs e)
        {
            gauge.AxisLineStyle.Thickness += 1;
        }

        private void AxisLineThicknessDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.AxisLineStyle.Thickness -= 1;
        }

        private void Picker_SelectedIndexChanged5(object sender, EventArgs e)
        {
            string color = (sender as Picker).SelectedItem.ToString();
            Brush brush=null;

            if (color == "Red")
                brush = new SolidColorBrush(Colors.Red);
            else if (color == "Black")
                brush = new SolidColorBrush(Colors.Black);
            else if (color == "Green")
                brush = new SolidColorBrush(Colors.Green);
            else if (color == "Blue")
                brush = new SolidColorBrush(Colors.Blue);
            else if (color == "Transparent")
                brush = new SolidColorBrush(Colors.Transparent);
            else if (color == "Gradient")
            {
                linearGradientBrush.StartPoint = new Point(0, 0.5);
                linearGradientBrush.EndPoint = new Point(1, 0.5);
                linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Red, 0.25f));
                linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Green, 0.75f));

                brush = linearGradientBrush;
            }

            gauge.MajorTickStyle.Stroke = brush;
        }

        private void MajorTickThicknessIncrease_Clicked(object sender, EventArgs e)
        {
            gauge.MajorTickStyle.StrokeThickness += 1;
        }

        private void MajorTickThicknessDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.MajorTickStyle.StrokeThickness -= 1;
        }

        private void MajorTickLengthIncrease_Clicked(object sender, EventArgs e)
        {
            gauge.MajorTickStyle.Length += 1;
        }

        private void MajorTickLengthDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.MajorTickStyle.Length -= 1;
        }

        private void Picker_SelectedIndexChanged6(object sender, EventArgs e)
        {
            string dashArray = (sender as Picker).SelectedItem.ToString();

            if (dashArray == "2,2")
                gauge.MajorTickStyle.StrokeDashArray = new DoubleCollection() { 2, 2 };
            else if (dashArray == "4,4")
                gauge.MajorTickStyle.StrokeDashArray = new DoubleCollection() { 4, 4 };
            else if (dashArray == "Null")
                gauge.MajorTickStyle.StrokeDashArray = null;
        }


        private void Picker_SelectedIndexChanged7(object sender, EventArgs e)
        {
            string color = (sender as Picker).SelectedItem.ToString();
            Brush brush = null;

            if (color == "Red")
                brush = new SolidColorBrush(Colors.Red);
            else if (color == "Black")
                brush= new SolidColorBrush(Colors.Black);
            else if (color == "Green")
                brush = new SolidColorBrush(Colors.Green);
            else if (color == "Blue")
                brush = new SolidColorBrush(Colors.Blue);
            else if (color == "Transparent")
                brush = new SolidColorBrush(Colors.Transparent);
            else if (color == "Gradient")
            {
                linearGradientBrush.StartPoint = new Point(0, 0.5);
                linearGradientBrush.EndPoint = new Point(1, 0.5);
                linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Red, 0.25f));
                linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Green, 0.75f));

                brush = linearGradientBrush;
            }

            gauge.MinorTickStyle.Stroke = brush;
        }

        private void MinorTickThicknessIncrease_Clicked(object sender, EventArgs e)
        {
            gauge.MinorTickStyle.StrokeThickness += 1;
        }

        private void MinorTickThicknessDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.MinorTickStyle.StrokeThickness -= 1;
        }

        private void MinorTickLengthIncrease_Clicked(object sender, EventArgs e)
        {
            gauge.MinorTickStyle.Length += 1;
        }

        private void MinorTickLengthDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.MinorTickStyle.Length -= 1;
        }

        private void Picker_SelectedIndexChanged8(object sender, EventArgs e)
        {
            string dashArray = (sender as Picker).SelectedItem.ToString();

            if (dashArray == "2,2")
                gauge.MinorTickStyle.StrokeDashArray = new DoubleCollection() { 2, 2 };
            else if (dashArray == "4,4")
                gauge.MinorTickStyle.StrokeDashArray = new DoubleCollection() { 4, 4 };
            else if (dashArray == "Null")
                gauge.MinorTickStyle.StrokeDashArray = null;
        }

        private void AxisLabelFontSizeIncrease_Clicked(object sender, EventArgs e)
        {
            gauge.AxisLabelStyle.FontSize += 1;
        }

        private void AxisLabelFontSizeDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.AxisLabelStyle.FontSize -= 1;
        }

        private void Picker_SelectedIndexChanged9(object sender, EventArgs e)
        {
            string color = (sender as Picker).SelectedItem.ToString();
            Color brush = null;

            if (color == "Red")
                brush = Colors.Red;
            else if (color == "Black")
                brush = Colors.Black;
            else if (color == "Green")
                brush = Colors.Green;
            else if (color == "Blue")
                brush = Colors.Blue;
            else if (color == "Transparent")
                brush =Colors.Transparent;

            gauge.AxisLabelStyle.TextColor = brush;
        }


        private void Picker_SelectedIndexChanged10(object sender, EventArgs e)
        {
            gauge.AxisLabelStyle.FontAttributes = (FontAttributes)(sender as Picker).SelectedItem;
        }


        private void Picker_SelectedIndexChanged11(object sender, EventArgs e)
        {
            string color = (sender as Picker).SelectedItem.ToString();
            Brush brush = null;

            if (color == "Red")
                brush = new SolidColorBrush(Colors.Red);
            else if (color == "Black")
                brush = new SolidColorBrush(Colors.Black);
            else if (color == "Green")
                brush = new SolidColorBrush(Colors.Green);
            else if (color == "Blue")
                brush = new SolidColorBrush(Colors.Blue);
            else if (color == "Transparent")
                brush = new SolidColorBrush(Colors.Transparent);
            else if (color == "Gradient")
            {
                linearGradientBrush.StartPoint = new Point(0, 0.5);
                linearGradientBrush.EndPoint = new Point(1, 0.5);
                linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Red, 0.25f));
                linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Green, 0.75f));

                brush = linearGradientBrush;
            }

            range.Fill = brush;
        }

        private void AxisLineGradientStopsSet_Clicked(object sender, EventArgs e)
        {
            var gradientStops = new System.Collections.ObjectModel.ObservableCollection<GaugeGradientStop>();
            

            gradientStops.Add(new GaugeGradientStop { Value = 0, Color = Colors.Red });
            gradientStops.Add(new GaugeGradientStop { Value = 50, Color = Colors.Blue });
            gradientStops.Add(new GaugeGradientStop { Value = 100, Color = Colors.Green });

           gauge.AxisLineStyle.GradientStops = gradientStops;

        }

        private void AxisLineGradientStopsSetNull_Clicked(object sender, EventArgs e)
        {
            gauge.AxisLineStyle.GradientStops = null;
        }

        private void RangeStartIncrease_Clicked(object sender, EventArgs e)
        {
            range.StartValue += 1;
        }

        private void RangeStartDecrease_Clicked(object sender, EventArgs e)
        {
            range.StartValue -= 1;
        }
        private void RangeEndIncrease_Clicked(object sender, EventArgs e)
        {
            range.EndValue += 1;
        }

        private void RangeEndDecrease_Clicked(object sender, EventArgs e)
        {
            range.EndValue -= 1;
        }

        private void RangeStartWidthIncrease_Clicked(object sender, EventArgs e)
        {
            range.StartWidth += 1;
        }

        private void RangeStartWidthDecrease_Clicked(object sender, EventArgs e)
        {
            range.StartWidth -= 1;
        }

        private void RangeMidWidthIncrease_Clicked(object sender, EventArgs e)
        {
            range.MidWidth += 1;
        }

        private void RangeMidWidthDecrease_Clicked(object sender, EventArgs e)
        {
            range.MidWidth -= 1;
        }

        private void RangeEndWidthIncrease_Clicked(object sender, EventArgs e)
        {
            range.EndWidth += 1;
        }

        private void RangeEndWidthDecrease_Clicked(object sender, EventArgs e)
        {
            range.EndWidth -= 1;
        }

        private void RangePositionInside_Clicked(object sender, EventArgs e)
        {
            range.RangePosition = GaugeElementPosition.Inside;
        }

        private void RangePositionOutside_Clicked(object sender, EventArgs e)
        {
            range.RangePosition = GaugeElementPosition.Outside;
        }

        private void RangePositionCross_Clicked(object sender, EventArgs e)
        {
            range.RangePosition = GaugeElementPosition.Cross;
        }

        private void RangeGradientStopsSet_Clicked(object sender, EventArgs e)
        {
            var gradientStops = new System.Collections.ObjectModel.ObservableCollection<GaugeGradientStop>();


            gradientStops.Add(new GaugeGradientStop { Value = 10, Color = Colors.Red });
            gradientStops.Add(new GaugeGradientStop { Value =25, Color = Colors.Blue });
            gradientStops.Add(new GaugeGradientStop { Value = 40, Color = Colors.Green });

            range.GradientStops = gradientStops;
        }

        private void RangeGradientStopsSetNull_Clicked(object sender, EventArgs e)
        {
            range.GradientStops = null;
        }
    }
}
