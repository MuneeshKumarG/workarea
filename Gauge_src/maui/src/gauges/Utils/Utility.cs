using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Contains utility methods.
    /// </summary>
    internal static class Utility
    {
        /// <summary>
        /// To convert the given degree to radian.
        /// </summary>
        /// <param name="degree">The degree.</param>
        /// <returns>Radian equivalent of given degree.</returns>
        internal static double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }

        /// <summary>
        /// To get intersect point for given points collection.
        /// </summary>
        /// <param name="startPoint">Start point.</param>
        /// <param name="midPoint">Mid point.</param>
        /// <param name="endPoint">End point.</param>
        /// <returns>Intersect point of given points.</returns>
        internal static PointF GetCenterPoint(PointF startPoint, PointF midPoint, PointF endPoint)
        {
            float ab = (startPoint.Y - midPoint.Y) / (startPoint.X - midPoint.X);
            float bc = (endPoint.Y - midPoint.Y) / (endPoint.X - midPoint.X);
            PointF center = new PointF()
            {
                X = (ab * bc * (endPoint.Y - startPoint.Y) + ab * (midPoint.X + endPoint.X) - bc * (startPoint.X + midPoint.X)) / (2 * (ab - bc))
            };
            center.Y = -(1 / ab) * (center.X - ((startPoint.X + midPoint.X) / 2)) + ((startPoint.Y + midPoint.Y) / 2);

            return center;
        }

        /// <summary>
        /// To convert the given radian to degree.
        /// </summary>
        /// <param name="radian">The radian value.</param>
        /// <returns>Degree equivalent of given radian.</returns>
        internal static float RadianToDegree(float radian)
        {
            return (float)(180 / Math.PI * radian);
        }

        /// <summary>
        /// Validate the minimum and maximum values, if maximum is less than minimum then swap the values.
        /// </summary>
        /// <param name="minimum">The minimum</param>
        /// <param name="maximum">The maximum</param>
        internal static void ValidateMinimumMaximumValue(ref double minimum, ref double maximum)
        {
            double min = double.IsNaN(minimum) ? 0 : minimum;
            double max = double.IsNaN(maximum) ? 0 : maximum;
            minimum = min < max ? min : max;
            maximum = min > max ? min : max;
        }

        /// <summary>
        /// To calculate SweepAngle.
        /// </summary>
        /// <param name="startAngle">The start angle.</param>
        /// <param name="endAngle">The end angle.</param>
        /// <returns>Sweep angle between start and end angle</returns>
        internal static double CalculateSweepAngle(double startAngle, double endAngle)
        {
            double actualEndAngle = endAngle > 360 ? endAngle % 360 : endAngle;
            double sweepAngle = actualEndAngle - startAngle;
            sweepAngle = sweepAngle <= 0 ? (sweepAngle + 360) : sweepAngle;
            return sweepAngle;
        }

        /// <summary>
        /// To convert angle to vector.
        /// </summary>
        /// <param name="angle">The angle</param>
        /// <returns>Vector of given angle</returns>
        internal static Point AngleToVector(double angle)
        {
            double angleRadian = Utility.DegreeToRadian(angle);
            return new Point(Math.Cos(angleRadian), Math.Sin(angleRadian));
        }

        /// <summary>
        /// To find the minimum and maximum value
        /// </summary>
        /// <param name="point1">Sets the point1</param>
        /// <param name="point2">Sets the point2</param>
        /// <param name="degree">Sets the degree</param>
        /// <returns>Min and Max value.</returns>
        internal static double GetMinMaxValue(Point point1, Point point2, int degree)
        {
            var minX = Math.Min(point1.X, point2.X);
            var minY = Math.Min(point1.Y, point2.Y);
            var maxX = Math.Max(point1.X, point2.X);
            var maxY = Math.Max(point1.Y, point2.Y);
            switch (degree)
            {
                case 270:
                    return maxY;
                case 0:
                case 360:
                    return minX;
                case 90:
                    return minY;
                case 180:
                    return maxX;
            }

            return 0d;
        }

        /// <summary>
        /// To calculate angle for corner radius
        /// </summary>
        /// <param name="radius">axis radius.</param>
        /// <param name="circleRadius">Corner radius ellipse radius</param>
        /// <returns>Angle for corner radius</returns>
        internal static double CornerRadiusAngle(double radius, double circleRadius)
        {
            var perimeter = ((2 * radius) + circleRadius) / 2;
            var area = Math.Sqrt(perimeter * (perimeter - radius) * (perimeter - radius) * (perimeter - circleRadius));
            return Math.Asin(2 * area / (radius * radius)) * (180 / Math.PI);
        }

        /// <summary>
        /// To update the gradient stops based actual range.
        /// </summary>
        /// <param name="gradientStops">The gradient stops collection.</param>
        /// <param name="rangeStart">The range start.</param>
        /// <param name="rangeEnd">The range end.</param>
        /// <returns>Updated gradient stops collection.</returns>
        internal static List<GaugeGradientStop> UpdateGradientStopCollection(List<GaugeGradientStop> gradientStops,
            double rangeStart, double rangeEnd)
        {
            gradientStops = gradientStops.OrderBy(x => x.ActualValue).ToList();

            if (gradientStops[0].ActualValue > rangeStart)
            {
                GaugeGradientStop GaugeGradientStop = new GaugeGradientStop
                {
                    Color = gradientStops[0].Color,
                    ActualValue = rangeStart
                };

                gradientStops.Insert(0, GaugeGradientStop);
            }

            if (gradientStops[gradientStops.Count - 1].ActualValue < rangeEnd)
            {
                GaugeGradientStop gaugeGradientStop = new GaugeGradientStop
                {
                    Color = gradientStops[gradientStops.Count - 1].Color,
                    ActualValue = rangeEnd
                };

                gradientStops.Add(gaugeGradientStop);
            }

            return gradientStops;
        }

        /// <summary>
        /// To get the gradient range based on gradient stops collection, range start and end values.
        /// </summary>
        /// <param name="gradientStops">The gradient stops.</param>
        /// <param name="rangeStart">The range start.</param>
        /// <param name="rangeEnd">The range end.</param>
        /// <returns>The gradient range based on gradient stops collection, range start and end values.</returns>
        internal static List<double> GetGradientRange(List<GaugeGradientStop> gradientStops, double rangeStart, double rangeEnd)
        {
            List<double> gradientRange = new List<double>()
            {
                rangeStart,
                rangeEnd
            };
            foreach (GaugeGradientStop gradient in gradientStops)
            {
                if (gradient.ActualValue > rangeStart && gradient.ActualValue < rangeEnd)
                {
                    if (!gradientRange.Contains(gradient.ActualValue))
                    {
                        gradientRange.Add(gradient.ActualValue);
                    }
                }
            }

            gradientRange.Sort();
            return gradientRange;
        }

        /// <summary>
        /// Convert into the middle color based on offset.
        /// </summary>
        /// <param name="color1">The first color.</param>
        /// <param name="color2">The second color.</param>
        /// <param name="offset">The offset.</param>
        /// <returns>Middle color based on provided offset.</returns>
        internal static Color GradientColorConvertion(Color color1, Color color2, double offset)
        {
            offset = offset > 1 ? 1 : offset;
            offset = offset < 0 ? 0 : offset;
            Color gradientColor = new Color((float)((color2.Red * offset) + (color1.Red * (1 - offset))),
                (float)((color2.Green * offset) + (color1.Green * (1 - offset))),
                (float)((color2.Blue * offset) + (color1.Blue * (1 - offset))),
                255);
            return gradientColor;
        }

        /// <summary>
        /// To swap the given values.
        /// </summary>
        /// <param name="firstValue">The first value.</param>
        /// <param name="secondValue">The second value.</param>
        internal static void Swap(ref double firstValue, ref double secondValue)
        {
            double temporary = firstValue;
            firstValue = secondValue;
            secondValue = temporary;
        }
    }

#nullable disable
    /// <summary>
    /// Extension for common NotifyCollectionChanges. 
    /// </summary>
    internal static class NotifyCollectionChangedEventArgsExtensions
    {
        public static void ApplyCollectionChanges(this NotifyCollectionChangedEventArgs self, Action<object, int, bool> insertAction, Action<object, int> removeAction, Action resetAction)
        {
            switch (self.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (self.NewStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.NewItems.Count; i++)
                        insertAction(self.NewItems[i], i + self.NewStartingIndex, true);

                    break;

                case NotifyCollectionChangedAction.Move:
                    if (self.NewStartingIndex < 0 || self.OldStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.OldItems.Count; i++)
                        removeAction(self.OldItems[i], self.OldStartingIndex);

                    int insertIndex = self.NewStartingIndex;
                    if (self.OldStartingIndex < self.NewStartingIndex)
                        insertIndex -= self.OldItems.Count - 1;

                    for (var i = 0; i < self.OldItems.Count; i++)
                        insertAction(self.OldItems[i], insertIndex + i, false);

                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (self.OldStartingIndex < 0)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.OldItems.Count; i++)
                        removeAction(self.OldItems[i], self.OldStartingIndex);
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (self.OldStartingIndex < 0 || self.OldItems.Count != self.NewItems.Count)
                        goto case NotifyCollectionChangedAction.Reset;

                    for (var i = 0; i < self.OldItems.Count; i++)
                    {
                        removeAction(self.OldItems[i], i + self.OldStartingIndex);
                        insertAction(self.NewItems[i], i + self.OldStartingIndex, true);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    {
                        resetAction();
                        break;
                    }
            }
        }
    }
#nullable enable
}
