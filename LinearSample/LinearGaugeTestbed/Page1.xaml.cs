using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Essentials;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Gauges;

namespace LinearGaugeTestbed
{
	public partial class Page1 : ContentPage
	{
        private double position = 10;

        public double Position
        {
            get { return position; }
            set { position = value; }
        }
        double interval;
        public Page1()
		{
			InitializeComponent();
            this.BindingContext = this;
            interval = double.IsNaN(gauge.Interval) ? 10 : gauge.Interval;

            barPointer.ValueChanging += BarPointer_ValueChanging;
            barPointer.ValueChanged += BarPointer_ValueChanged;
            barPointer.ValueChangeStarted += BarPointer_ValueChangeStarted;
            barPointer.ValueChangeCompleted += BarPointer_ValueChangeCompleted;
        }

        private void BarPointer_ValueChangeCompleted(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
        {
            Debug.WriteLine("BarPointer_ValueChangeCompleted");
        }

        private void BarPointer_ValueChangeStarted(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
        {
            Debug.WriteLine("BarPointer_ValueChangeStarted");
        }

        private void BarPointer_ValueChanged(object sender, Syncfusion.Maui.Gauges.ValueChangedEventArgs e)
        {
            Debug.WriteLine("BarPointer_ValueChanged");
        }

        private void BarPointer_ValueChanging(object sender, ValueChangingEventArgs e)
        {

            Debug.WriteLine("BarPointer_ValueChanging");

            if (e.NewValue > 70)
                e.Cancel = true;
        }

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
                gauge.LineStyle.Fill = new SolidColorBrush(Colors.Red);
            else if (color == "Black")
                gauge.LineStyle.Fill = new SolidColorBrush(Colors.Black);
            else if (color == "Green")
                gauge.LineStyle.Fill = new SolidColorBrush(Colors.Green);
            else if (color == "Blue")
                gauge.LineStyle.Fill = new SolidColorBrush(Colors.Blue);
            else if (color == "Transparent")
                gauge.LineStyle.Fill = new SolidColorBrush(Colors.Transparent);
            else if (color == "Gradient")
            {
                linearGradientBrush.StartPoint = new Point(0, 0.5);
                linearGradientBrush.EndPoint = new Point(1, 0.5);
                linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Red, 0.25f));
                linearGradientBrush.GradientStops.Add(new Microsoft.Maui.Controls.GradientStop(Colors.Green, 0.75f));

                gauge.LineStyle.Fill = linearGradientBrush;
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
                gauge.LineStyle.CornerRadius = new Thickness(0, 5, 5, 5);
            else if (radius == "5,0,5,5")
                gauge.LineStyle.CornerRadius = new Thickness(5, 0, 5, 5);
            else if (radius == "5,5,0,5")
                gauge.LineStyle.CornerRadius = new Thickness(5, 5, 0, 5);
            else if (radius == "5,5,5,0")
                gauge.LineStyle.CornerRadius = new Thickness(5, 5, 5, 0);
            else if (radius == "5,5,5,5")
                gauge.LineStyle.CornerRadius = new Thickness(5, 5, 5, 5);
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

        public Array ShapeTypes
        {
            get
            {
                return Enum.GetValues(typeof(ShapeType));
            }

        }

        private void Picker_SelectedIndexChanged3(object sender, EventArgs e)
        {
            gauge.LineStyle.CornerStyle = (CornerStyle)(sender as Picker).SelectedItem;
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
                gauge.LineStyle.DashArray = new DoubleCollection() {2,2 };
            else if (dashArray == "4,4")
                gauge.LineStyle.DashArray = new DoubleCollection() { 4,4};
            else if (dashArray == "Null")
                gauge.LineStyle.DashArray = null;
        }

        private void AxisLineThicknessIncrease_Clicked(object sender, EventArgs e)
        {
            gauge.LineStyle.Thickness += 1;
        }

        private void AxisLineThicknessDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.LineStyle.Thickness -= 1;
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
            gauge.LabelStyle.FontSize += 1;
        }

        private void AxisLabelFontSizeDecrease_Clicked(object sender, EventArgs e)
        {
            gauge.LabelStyle.FontSize -= 1;
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

            gauge.LabelStyle.TextColor = brush;
        }


        private void Picker_SelectedIndexChanged10(object sender, EventArgs e)
        {
            gauge.LabelStyle.FontAttributes = (FontAttributes)(sender as Picker).SelectedItem;
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

        private void Picker_SelectedIndexChanged12(object sender, EventArgs e)
        {
            barPointer.CornerStyle = (CornerStyle)(sender as Picker).SelectedItem;
        }

        private void Picker_SelectedIndexChanged13(object sender, EventArgs e)
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

            barPointer.Fill = brush;
        }

        private void Picker_SelectedIndexChanged14(object sender, EventArgs e)
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
                brush = Colors.Transparent;

           // barPointer.Stroke = brush;
        }

        private void Picker_SelectedIndexChanged15(object sender, EventArgs e)
        {
            shapePointer.ShapeType = (ShapeType)(sender as Picker).SelectedItem;
        }

        private void Picker_SelectedIndexChanged16(object sender, EventArgs e)
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

            shapePointer.Fill = brush;
        }

        private void Picker_SelectedIndexChanged17(object sender, EventArgs e)
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
                brush = Colors.Transparent;

             shapePointer.Stroke = brush;
        }

        private void AxisLineGradientStopsSet_Clicked(object sender, EventArgs e)
        {
            var gradientStops = new System.Collections.ObjectModel.ObservableCollection<GaugeGradientStop>();
            

            gradientStops.Add(new GaugeGradientStop { Value = 0, Color = Colors.Red });
            gradientStops.Add(new GaugeGradientStop { Value = 50, Color = Colors.Blue });
            gradientStops.Add(new GaugeGradientStop { Value = 100, Color = Colors.Green });

           gauge.LineStyle.GradientStops = gradientStops;

        }

        private void AxisLineGradientStopsSetNull_Clicked(object sender, EventArgs e)
        {
            gauge.LineStyle.GradientStops = null;
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
            if(double.IsNaN(range.MidWidth))
                range.MidWidth = range.StartWidth;
            range.MidWidth += 1;
        }

        private void RangeMidWidthDecrease_Clicked(object sender, EventArgs e)
        {
            if (double.IsNaN(range.MidWidth))
                range.MidWidth = range.StartWidth;

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


            gradientStops.Add(new GaugeGradientStop { Value = 20, Color = Colors.Red });
            gradientStops.Add(new GaugeGradientStop { Value =35, Color = Colors.Blue });
            gradientStops.Add(new GaugeGradientStop { Value = 50, Color = Colors.Green });

            range.GradientStops = gradientStops;
        }

        private void RangeGradientStopsSetNull_Clicked(object sender, EventArgs e)
        {
            range.GradientStops = null;
        }

        private void BarPointerValueIncrease_Clicked(object sender, EventArgs e)
        {
            barPointer.Value += 1;
        }

        private void BarPointerValueDecrease_Clicked(object sender, EventArgs e)
        {
            barPointer.Value -= 1;
        }

        private void BarPointerOffsetIncrease_Clicked(object sender, EventArgs e)
        {
            barPointer.Offset += 1;
        }

        private void BarPointerOffsetDecrease_Clicked(object sender, EventArgs e)
        {
            barPointer.Offset -= 1;
        }

        private void BarPointerSizeIncrease_Clicked(object sender, EventArgs e)
        {
            barPointer.PointerSize += 1;
        }

        private void BarPointerSizeDecrease_Clicked(object sender, EventArgs e)
        {
            barPointer.PointerSize -= 1;
        }

        private void BarPointerBorderSizeIncrease_Clicked(object sender, EventArgs e)
        {
           // barPointer.BorderWidth += 1;
        }

        private void BarPointerBorderSizeDecrease_Clicked(object sender, EventArgs e)
        {
           // barPointer.BorderWidth -= 1;
        }

        private void BarPointerGradientStopsSet_Clicked(object sender, EventArgs e)
        {
            var gradientStops = new System.Collections.ObjectModel.ObservableCollection<GaugeGradientStop>();


            gradientStops.Add(new GaugeGradientStop { Value = 0, Color = Colors.Red });
            gradientStops.Add(new GaugeGradientStop { Value = 30, Color = Colors.Blue });
            gradientStops.Add(new GaugeGradientStop { Value = 70, Color = Colors.Green });

            barPointer.GradientStops = gradientStops;

        }

        private void BarPointerGradientStopsSetNull_Clicked(object sender, EventArgs e)
        {
            barPointer.GradientStops = null;
        }

        private void RangeChildSet_Clicked(object sender, EventArgs e)
        {
            range.Child = new Image()
            {
                Source = "alexandar.png"
            };
        }

        private void RangeChildSetNull_Clicked(object sender, EventArgs e)
        {
            range.Child = null;
        }

        private void RangeAdd_Clicked(object sender, EventArgs e)
        {
            gauge.Ranges.Add(new LinearRange()
            {
                StartValue = 30,
                EndValue =60,
                Fill=new SolidColorBrush(Colors.Red),
            });
        }

        private void RangeInsert_Clicked(object sender, EventArgs e)
        {
            gauge.Ranges.Insert(0,new LinearRange()
            {
                StartValue = 30,
                EndValue = 60,
                Fill = new SolidColorBrush(Colors.Yellow),
            });
        }

        private void RangeClear_Clicked(object sender, EventArgs e)
        {
            gauge.Ranges.Clear();
        }

        private void BarPointerChildSet_Clicked(object sender, EventArgs e)
        {
            barPointer.Child = new Image()
            {
                Source = "alexandar.png"
            };
        }

        private void BarPointerChildSetNull_Clicked(object sender, EventArgs e)
        {
            barPointer.Child = null;
        }

        private void BarPointerAdd_Clicked(object sender, EventArgs e)
        {
            gauge.BarPointers.Add(new BarPointer()
            {
                Value = 80,
                Fill = new SolidColorBrush(Colors.Red),
            });
        }

        private void BarPointerInsert_Clicked(object sender, EventArgs e)
        {
            gauge.BarPointers.Insert(0, new BarPointer()
            {
                Value = 80,
                Fill = new SolidColorBrush(Colors.Yellow),
            });
        }

        private void BarPointerClear_Clicked(object sender, EventArgs e)
        {
            gauge.BarPointers.Clear();
        }

        private void ShapePointerValueIncrease_Clicked(object sender, EventArgs e)
        {
            shapePointer.Value += 1;
        }

        private void ShapePointerValueDecrease_Clicked(object sender, EventArgs e)
        {
            shapePointer.Value -= 1;
        }

        private void ShapePointerOffsetLeftIncrease_Clicked(object sender, EventArgs e)
        {
            UpdateOffset(shapePointer, 1, 0);
        }

        private void ShapePointerOffseLeftDecrease_Clicked(object sender, EventArgs e)
        {
            UpdateOffset(shapePointer, -1, 0);
        }

        private void ShapePointerOffsetTopIncrease_Clicked(object sender, EventArgs e)
        {
            UpdateOffset(shapePointer, 0, 1);
        }

        private void ShapePointerOffseTopDecrease_Clicked(object sender, EventArgs e)
        {
            UpdateOffset(shapePointer, 0, -1);
        }

        private void UpdateOffset(LinearMarkerPointer pointer, double left, double top)
        {
            if (pointer.OffsetPoint == null)
                pointer.OffsetPoint = new Point(0, 0);

            pointer.OffsetPoint = new Point(pointer.OffsetPoint.X + left, pointer.OffsetPoint.Y + top);
        }

        private void BarPositionHAlignmentStart_Clicked(object sender, EventArgs e)
        {
            UpdateHorizontalAlignment(shapePointer, GaugeAlignment.Start);
        }

        private void BarPositionHAlignmentCenter_Clicked(object sender, EventArgs e)
        {
            UpdateHorizontalAlignment(shapePointer, GaugeAlignment.Center);
        }

        private void BarPositionHAlignmentEnd_Clicked(object sender, EventArgs e)
        {
            UpdateHorizontalAlignment(shapePointer, GaugeAlignment.End);
        }

        private void UpdateHorizontalAlignment(LinearMarkerPointer pointer, GaugeAlignment gaugeAlignment)
        {
            pointer.HorizontalAlignment= gaugeAlignment;
        }

        private void BarPositionVAlignmentStart_Clicked(object sender, EventArgs e)
        {
            UpdateVerticalAlignment(shapePointer, GaugeAlignment.Start);
        }

        private void BarPositionVAlignmentCenter_Clicked(object sender, EventArgs e)
        {
            UpdateVerticalAlignment(shapePointer, GaugeAlignment.Center);
        }

        private void BarPositionVAlignmentEnd_Clicked(object sender, EventArgs e)
        {
            UpdateVerticalAlignment(shapePointer, GaugeAlignment.End);
        }

        private void UpdateVerticalAlignment(LinearMarkerPointer pointer, GaugeAlignment gaugeAlignment)
        {
            pointer.VerticalAlignment = gaugeAlignment;
        }

        private void ShapePointerHeightIncrease_Clicked(object sender, EventArgs e)
        {
            shapePointer.ShapeHeight += 1;
        }

        private void ShapePointerHeightDecrease_Clicked(object sender, EventArgs e)
        {
            shapePointer.ShapeHeight -= 1;
        }

        private void ShapePointerWidthIncrease_Clicked(object sender, EventArgs e)
        {
            shapePointer.ShapeWidth += 1;
        }

        private void ShapePointerWidthDecrease_Clicked(object sender, EventArgs e)
        {
            shapePointer.ShapeWidth -= 1;
        }

        private void ShapePointerBorderIncrease_Clicked(object sender, EventArgs e)
        {
            shapePointer.StrokeThickness += 1;
        }

        private void ShapePointerBorderDecrease_Clicked(object sender, EventArgs e)
        {
            shapePointer.StrokeThickness -= 1;
        }


        private void ContentPointerValueIncrease_Clicked(object sender, EventArgs e)
        {
            contentPointer.Value += 1;
        }

        private void ContentPointerValueDecrease_Clicked(object sender, EventArgs e)
        {
            contentPointer.Value -= 1;
        }

        private void ContentPointerOffsetLeftIncrease_Clicked(object sender, EventArgs e)
        {
            UpdateOffset(contentPointer, 1, 0);
        }

        private void ContentPointerOffseLeftDecrease_Clicked(object sender, EventArgs e)
        {
            UpdateOffset(contentPointer, -1, 0);
        }

        private void ContentPointerOffsetTopIncrease_Clicked(object sender, EventArgs e)
        {
            UpdateOffset(contentPointer, 0, 1);
        }

        private void ContentPointerOffseTopDecrease_Clicked(object sender, EventArgs e)
        {
            UpdateOffset(contentPointer, 0, -1);
        }


        private void ContentPositionHAlignmentStart_Clicked(object sender, EventArgs e)
        {
            UpdateHorizontalAlignment(contentPointer, GaugeAlignment.Start);
        }

        private void ContentPositionHAlignmentCenter_Clicked(object sender, EventArgs e)
        {
            UpdateHorizontalAlignment(contentPointer, GaugeAlignment.Center);
        }

        private void ContentPositionHAlignmentEnd_Clicked(object sender, EventArgs e)
        {
            UpdateHorizontalAlignment(contentPointer, GaugeAlignment.End);
        }

        private void ContentPositionVAlignmentStart_Clicked(object sender, EventArgs e)
        {
            UpdateVerticalAlignment(contentPointer, GaugeAlignment.Start);
        }

        private void ContentPositionVAlignmentCenter_Clicked(object sender, EventArgs e)
        {
            UpdateVerticalAlignment(contentPointer, GaugeAlignment.Center);
        }

        private void ContentPositionVAlignmentEnd_Clicked(object sender, EventArgs e)
        {
            UpdateVerticalAlignment(contentPointer, GaugeAlignment.End);
        }

        private void ContentPointerChildSet_Clicked(object sender, EventArgs e)
        {
            contentPointer.Content = new Image()
            {
                Source = "alexandar.png"
            };
        }

        private void ContentPointerChildSetNull_Clicked(object sender, EventArgs e)
        {
            contentPointer.Content = null;
        }

    }
}
