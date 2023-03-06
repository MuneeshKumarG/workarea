using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Windows.Data.Xml.Dom;
using NativeColor = Windows.UI.Color;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Contains utility methods to manipulate data.
    /// </summary>
    internal static class ChartDataUtils
    {

        /// <summary>
        /// Gets the property from the specified object.
        /// </summary>
        /// <param name="obj">Object to retrieve a property.</param>
        /// <param name="path">Property name</param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo(object obj, string path)
        {
#if SyncfusionFramework4_0
            return obj.GetType().GetTypeInfo().GetDeclaredProperty(path);
#else
               return obj.GetType().GetRuntimeProperty(path);
#endif
        }

		internal static bool EqualDoubleValues(double x, double y)
		{
			return x.ToString() == y.ToString();
		}

        internal static DateTime IncreaseInterval(DateTime date, double visibleInterval, DateTimeIntervalType actualIntervalType)
        {
            var interval = visibleInterval;
            var span = new TimeSpan(0);
            switch (actualIntervalType)
            {
                case DateTimeIntervalType.Days:
                    span = TimeSpan.FromDays(interval);
                    break;
                case DateTimeIntervalType.Hours:
                    span = TimeSpan.FromHours(interval);
                    break;
                case DateTimeIntervalType.Milliseconds:
                    span = TimeSpan.FromMilliseconds(interval);
                    break;
                case DateTimeIntervalType.Seconds:
                    span = TimeSpan.FromSeconds(interval);
                    break;
                case DateTimeIntervalType.Minutes:
                    span = TimeSpan.FromMinutes(interval);
                    break;
                case DateTimeIntervalType.Months:

                    date = date.AddMonths((int)interval);
                    double dayInterval = interval - (int)interval;

                    if (dayInterval > 0)
                    {
                        return AddMonth(date, dayInterval);
                    }
                    else
                    {
                        return date;
                    }

                case DateTimeIntervalType.Years:

                    date = date.AddYears((int)interval);
                    double monthInterval = interval - (int)interval;

                    if (monthInterval > 0)
                    {
                        return date.AddMonths((int)(monthInterval * 12));
                    }
                    else
                    {
                        return date;
                    }
            }

            return date.Add(span);
        }

        internal static DateTime AddMonth(DateTime date, double interval)
        {
            int days = 31;
            switch (date.Month)
            {
                case 2:
                case 4:
                case 7:
                case 9:
                    days = 30;
                    break;
                case 0:
                    int year = date.Year;
                    if ((year % 400 == 0) || ((year % 4 == 0) && (year % 100 != 0)))
                    {
                        days = 28;
                    }
                    else
                    {
                        days = 29;
                    }

                    break;
            }

            return date.AddMilliseconds((long)(60 * 60 * 24 * days * interval));
        }

        internal static PointCollection GetTooltipPolygonPoints(Rect rect, double seriesTipHeight, bool isReversed, ChartAlignment horizontalAlignment, ChartAlignment verticalAlignment)
        {
           
            double seriesTipHypotenuse;
            double hypotenuse;
            double x = rect.X; double y = rect.Y; double labelWidth = rect.Width; double labelHeight = rect.Height;

            if ((verticalAlignment == ChartAlignment.Center && horizontalAlignment != ChartAlignment.Center) ||
                (horizontalAlignment == ChartAlignment.Center && verticalAlignment != ChartAlignment.Center))
            {
                seriesTipHypotenuse = (2 * seriesTipHeight) / Math.Sqrt(3);
                hypotenuse = seriesTipHypotenuse / 2;
            }
            else
            {
                seriesTipHypotenuse = labelHeight / 3;
                hypotenuse = seriesTipHypotenuse > 25 ? (seriesTipHypotenuse * 2) / 3 : seriesTipHypotenuse;
            }

            PointCollection polygonPoints = new PointCollection();

            polygonPoints.Add(new Point(0, 0));

            // Fix the series label tip at left.
            if (horizontalAlignment == ChartAlignment.Far)
            {
                polygonPoints.Add(new Point(0, +labelHeight / 2 - hypotenuse));

                if (!isReversed || verticalAlignment == ChartAlignment.Center)
                {
                    if (verticalAlignment == ChartAlignment.Near)
                        polygonPoints.Add(new Point(-seriesTipHeight, (y - 4)));
                    else if (verticalAlignment == ChartAlignment.Far)
                        polygonPoints.Add(new Point(-seriesTipHeight, y + 4));
                    else
                        polygonPoints.Add(new Point(-seriesTipHeight, y));
                }

                polygonPoints.Add(new Point(0, labelHeight / 2 + hypotenuse));
            }

            polygonPoints.Add(new Point(0, labelHeight));

            // Fix the series label tip at bottom.
            if (verticalAlignment == ChartAlignment.Near)
            {
                polygonPoints.Add(new Point(labelWidth / 2 - hypotenuse, labelHeight));

                if (isReversed || horizontalAlignment == ChartAlignment.Center)
                {
                    if (horizontalAlignment == ChartAlignment.Far)
                        polygonPoints.Add(new Point(x, seriesTipHeight + labelHeight));
                    else if (horizontalAlignment == ChartAlignment.Near)
                        polygonPoints.Add(new Point(x - 4, seriesTipHeight + labelHeight));
                    else
                        polygonPoints.Add(new Point(x, seriesTipHeight + labelHeight));
                }

                polygonPoints.Add(new Point(labelWidth / 2 + hypotenuse, labelHeight));
            }

            polygonPoints.Add(new Point(labelWidth, labelHeight));

            // Fix the series label tip right.
            if (horizontalAlignment == ChartAlignment.Near)
            {
                polygonPoints.Add(new Point(labelWidth, labelHeight / 2 + hypotenuse));

                if (!isReversed || verticalAlignment == ChartAlignment.Center)
                {
                    if (verticalAlignment == ChartAlignment.Near)
                        polygonPoints.Add(new Point(seriesTipHeight + labelWidth, (y - 4)));
                    else if (verticalAlignment == ChartAlignment.Far)
                        polygonPoints.Add(new Point(seriesTipHeight + labelWidth, (y + 4)));
                    else
                        polygonPoints.Add(new Point(seriesTipHeight + labelWidth, (y)));
                }

                polygonPoints.Add(new Point(labelWidth, labelHeight / 2 - hypotenuse));
            }

            polygonPoints.Add(new Point(labelWidth, 0));

            // Fix series label tip top
            if (verticalAlignment == ChartAlignment.Far)
            {
                polygonPoints.Add(new Point(labelWidth / 2 + hypotenuse, 0));

                if (isReversed || horizontalAlignment == ChartAlignment.Center)
                {
                    if (horizontalAlignment == ChartAlignment.Far)
                        polygonPoints.Add(new Point(x, -seriesTipHeight));
                    else if (horizontalAlignment == ChartAlignment.Near)
                        polygonPoints.Add(new Point(x - 4, -seriesTipHeight));
                    else
                        polygonPoints.Add(new Point(x, -seriesTipHeight));
                }

                polygonPoints.Add(new Point(labelWidth / 2 - hypotenuse, 0));
            }

            polygonPoints.Add(new Point(0, 0));

            return polygonPoints;
        }

        internal static string GenerateTooltipPolygon(Size labelSize, HorizontalPosition horizontal, VerticalPosition vertical)
        {
            double arcSize = 3;
            double rotateAngle = 0;
            double isLargeArc = 0;
            double sweepDirection = 1;
            Point drawPoint = new Point(arcSize, 0);
            double labelWidth = labelSize.Width;
            double labelHeight = labelSize.Height;
            string startString = "M" + "0" + "," + arcSize.ToString();

            for (int i = 0; i < 4; i++)
            {
                string arc;
                if (i == 0)
                {
                    arc = CreateArc(arcSize, rotateAngle, isLargeArc, sweepDirection, drawPoint);
                    startString += arc;
                    startString += " ";
                    if (vertical == VerticalPosition.Top)
                    {
                        startString += drawNosePointer(horizontal, vertical, labelWidth, labelHeight);
                    }
                    else
                    {
                        startString += "L" + (labelWidth - arcSize).ToString() + "," + "0";
                    }
                }
                else if (i == 1)
                {
                    drawPoint = new Point(labelWidth, arcSize);
                    arc = CreateArc(arcSize, rotateAngle, isLargeArc, sweepDirection, drawPoint);
                    startString += arc;
                    startString += " ";                 
                    startString += "L" + (labelWidth).ToString() + "," + (labelHeight - arcSize).ToString();                    
                }
                else if (i == 2)
                {
                    drawPoint = new Point(labelWidth - arcSize, labelHeight);
                    arc = CreateArc(arcSize, rotateAngle, isLargeArc, sweepDirection, drawPoint);
                    startString += arc;
                    startString += " ";
                    if (vertical == VerticalPosition.Bottom)
                    {
                        startString += drawNosePointer(horizontal, vertical, labelWidth, labelHeight);
                    }
                    else
                    {
                        startString += "L" + arcSize.ToString() + "," + labelHeight.ToString();
                    }
                }
                else if (i == 3)
                {
                    drawPoint = new Point(0, labelHeight - arcSize);
                    arc = CreateArc(arcSize, rotateAngle, isLargeArc, sweepDirection, drawPoint);
                    startString += arc;
                    startString += " ";                   
                }
            }
            startString += "z";
            return startString;
        }

        private static string CreateArc(double arcSize, double rotateAngle, double isLargeArc, double sweepDirection, Point drawPoint)
        {
            return " A" + arcSize.ToString() + "," + arcSize.ToString() + " " + rotateAngle.ToString() + " " + isLargeArc.ToString()
                 + " " + sweepDirection.ToString() + " " + drawPoint.X.ToString() + "," + drawPoint.Y.ToString();
        }

        internal static string drawNosePointer(HorizontalPosition horizontal, VerticalPosition vertical, double width,
            double height)
        {
            double depth = 4;
            double depthWidth = depth + 1.5;
            double x1 = (width / 2) + depthWidth;
            double x2 = (width / 2) - depthWidth;
            double y1 = (height / 2) + depthWidth;
            double y2 = (height / 2) - depthWidth;
            double center = (x1 + x2) / 2;
            string startString = "";           
            if (vertical == VerticalPosition.Top)
            {
                startString = DrawTopPolygon(width, depthWidth, depth, horizontal);
            }
            else if (vertical == VerticalPosition.Bottom)
            {
                startString = DrawBottomPolygon(width, height, depthWidth, depth, horizontal);
            }          
            return startString;
        }

        private static string DrawBottomPolygon(double width, double height, double depthWidth, double depth, 
            HorizontalPosition horizontal)
        {
            string startString = "";
            Point startPoint = new Point();
            Point depthPoint = new Point();
            Point depthEndPoint = new Point();
            Point endPoint = new Point();
            double x1;
            double x2;
            double center;
            switch (horizontal)
            {
                case HorizontalPosition.Center:
                    x1 = (width / 2) + depthWidth;
                    x2 = (width / 2) - depthWidth;
                    center = (x1 + x2) / 2;
                    startPoint = new Point(x1, height);
                    depthPoint = new Point(center, height + depth);
                    depthEndPoint = new Point(x2, height);
                    endPoint = new Point(3, height);
                    break;
                case HorizontalPosition.Left:
                    x1 = 3 + (depthWidth * 2);
                    x2 = 3 + 1;
                    center = (x1 + x2) / 2;
                    startPoint = new Point(x1, height);
                    depthPoint = new Point(0, height + depth);
                    depthEndPoint = new Point(x2, height);
                    endPoint = new Point(3, height);
                    break;
                case HorizontalPosition.Right:
                    x1 = width - (3 + 1);
                    x2 = width - (depthWidth * 2);
                    center = (x1 + x2) / 2;
                    startPoint = new Point(x1, height);
                    depthPoint = new Point(width, height + depth);
                    depthEndPoint = new Point(x2, height);
                    endPoint = new Point(3, height);
                    break;
            }

            startString += "L" + startPoint.X.ToString() + "," + startPoint.Y.ToString();
            startString += "L" + depthPoint.X.ToString() + "," + depthPoint.Y.ToString();           
            startString += "L" + depthEndPoint.X.ToString() + "," + depthEndPoint.Y.ToString();
            startString += "L" + endPoint.X.ToString() + "," + endPoint.Y.ToString();

            return startString;
        }

        private static string DrawTopPolygon(double width, double depthWidth, double depth, HorizontalPosition horizontal)
        {
            string startString = "";
            Point startPoint = new Point();
            Point depthPoint = new Point();
            Point depthEndPoint = new Point();
            Point endPoint = new Point();
            double x1;
            double x2;

            double center;
            switch (horizontal)
            {
                case HorizontalPosition.Center:
                    x1 = (width / 2) + depthWidth;
                    x2 = (width / 2) - depthWidth;
                    center = (x1 + x2) / 2;
                    startPoint = new Point(x2, 0);
                    depthPoint = new Point(center, -depth);
                    depthEndPoint = new Point(x1, 0);
                    endPoint = new Point(width - 3, 0);
                    break;
                case HorizontalPosition.Left:
                    x1 = 3 + (depthWidth * 2);
                    x2 = 3 + 1;
                    center = (x1 + x2) / 2;
                    startPoint = new Point(x2, 0);
                    depthPoint = new Point(0, -depth);
                    depthEndPoint = new Point(x1, 0);
                    endPoint = new Point(width - 3, 0);
                    break;
                case HorizontalPosition.Right:
                    x1 = width - (3 + 1);
                    x2 = width - (depthWidth * 2);
                    center = (x1 + x2) / 2;
                    startPoint = new Point(x2, 0);
                    depthPoint = new Point(width, -depth);
                    depthEndPoint = new Point(x1, 0);
                    endPoint = new Point(width - 3, 0);
                    break;
            }

            startString += "L" + startPoint.X.ToString() + "," + startPoint.Y.ToString();
            startString += "L" + depthPoint.X.ToString() + "," + depthPoint.Y.ToString();         
            startString += "L" + depthEndPoint.X.ToString() + "," + depthEndPoint.Y.ToString();
            startString += "L" + endPoint.X.ToString() + "," + endPoint.Y.ToString();

            return startString;
        }
    }
    /// <summary>
    /// Custom comaprer to compare the chart points by x-value.
    /// </summary>
    internal class PointsSortByXComparer : Comparer<Point>
    {
        #region Members
        /// <summary>
        /// Initializes diff
        /// </summary>
        private double diff;
        #endregion

        /// <summary>
        /// Compares the specified p1 with the specified p2.
        /// </summary>
        /// <param name="point1">The point1.</param>
        /// <param name="point2">The point2.</param>
        /// <returns>
        /// negative value if point1 &lt; point2
        /// <para>
        /// zero if point1 = point2.
        /// </para>
        /// <para>
        /// positive value if point1 &gt; point2
        /// </para>
        /// </returns>
        public override int Compare(Point point1,Point point2)
        {
            diff = point1.X - point2.X;
            if (diff == 0)
            {
                return 0;
            }

            return diff < 0 ? -1 : 1;
        }
    }

    /// <summary>
    /// ChartColorModifed To modify a given color.
    /// </summary>
    internal static class ChartColorModifier
    {
        /// <summary>
        /// Gets the darkened color which was set.
        /// </summary>
        /// <param name="brush">The point1.</param>
        /// <param name="darkCoefficient">The point2.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal static Brush GetDarkenedColor(Brush brush, double darkCoefficient)
        {
            var color = (brush is SolidColorBrush) ? (brush as SolidColorBrush).Color
                         : (brush is GradientBrush) && (brush as GradientBrush).GradientStops.Count > 0
                         ? (brush as GradientBrush).GradientStops[0].Color
                         : new SolidColorBrush(Colors.Transparent).Color;

            var alpha = (byte)color.A;
            var red = (byte)(color.R * darkCoefficient);
            var green = (byte)(color.G * darkCoefficient);
            var blue = (byte)(color.B * darkCoefficient);

          return new SolidColorBrush(NativeColor.FromArgb(alpha, red, green, blue));
        }
    }
}
