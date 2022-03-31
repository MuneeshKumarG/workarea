using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using System.Reflection;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Media.Imaging;
using Rect = Windows.Foundation.Rect;
using NativeColor = Windows.UI.Color;
using NativeKeyTime = Microsoft.UI.Xaml.Media.Animation.KeyTime;
using Microsoft.UI;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Contains Chart extension methods.
    /// </summary>
    internal static class ChartExtensionUtils
    {
        internal static DateTime ValidateNonWorkingDate(this DateTime date, string days, bool isToBack,int nonWokingDays)
        {
            var i = 0;
            var isNonWorkingDay = false;
            do
            {
                isNonWorkingDay = false;
                if (!days.Contains(date.DayOfWeek.ToString()))
                {
                    i++;
                    isNonWorkingDay = true;
                    date = date.AddDays(isToBack ? -nonWokingDays : nonWokingDays);
                    break;
                }
            } while (isNonWorkingDay);
            return date;
        }

        internal static DateTime ValidateNonWorkingHours(this DateTime date, double startTime, double endTime, bool isToBack)
        {

            var time = date.TimeOfDay.TotalHours;
            if (isToBack)
            {
                if (time < startTime)
                {
                    date = date.AddDays(-1);//.AddHours(-(endTime - (time - startTime)));
                    date = new DateTime(date.Year, date.Month, date.Day, (int)(endTime - (startTime - time)), date.Minute, date.Second);
                }
                else if (time > endTime)
                {
                    date = date.AddHours(-(time - endTime));
                }
            }
            else
            {
                if (time < startTime)
                {
                    date = date.AddHours(startTime - time);
                }
                else if (time > endTime)
                {
                    date = new DateTime(date.Year, date.Month, date.Day).AddHours(24).AddHours(startTime);
                }
            }
            return date;
        }

        /// <summary>
        /// Method used to gets or sets intersect of two rectangle.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IntersectsWith(this Rect rect, Rect other)
        {
            if (other.Bottom < rect.Top || other.Right < rect.Left
             || other.Top > rect.Bottom || other.Left > rect.Right)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method  used to set the offset value.
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Rect Offset(this Rect rect, double x, double y)
        {
            return new Rect(rect.X + x, rect.Y + y, rect.Width - x, rect.Height - y);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
           (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source != null ? source.Where(element => seenKeys.Add(keySelector(element))) : null;
        }

        private static DateTime BaseDate = new DateTime(1899, 12, 30);

        /// <summary>
        /// Converts the value of this instance to the equivalent OLE Automation date.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
       
        public static double ToOADate(this DateTime time)
        {
            return time.Subtract(BaseDate).TotalDays;
        }

        /// <summary>
        /// Returns a DateTime equivalent to the specified OLE Automation Date.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
       
        public static DateTime FromOADate(this Double value)
        {
            return BaseDate.AddDays(value);
        }

#if NETFX_CORE 

        internal static MethodInfo GetSetMethod(this PropertyInfo propertyInfo)
        {
            return propertyInfo.SetMethod;
        }

        internal static MethodInfo GetGetMethod(this PropertyInfo propertyInfo)
        {
            return propertyInfo.GetMethod;
        }

#endif

        

        internal static int BinarySearch(List<double> xValues, double touchValue, int min, int max)
        {
            var closerIndex = 0;
            var closerDelta = double.MaxValue;

            while (min <= max)
            {
                int mid = (min + max) / 2;
                var xValue = xValues[mid];
                var delta = Math.Abs(touchValue - xValue);

                if (delta < closerDelta)
                {
                    closerDelta = delta;
                    closerIndex = mid;
                }

                if (touchValue == xValue)
                {
                    return mid;
                }
                else if (touchValue < xValues[mid])
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }
            }

            return closerIndex;
        }

       
    }


    internal static class ClearUIElementProperties
    {
        internal static void ClearUIValues(this Shape element)
        {
            if (element is Line)
            {
                element.ClearValue(Line.X1Property);
                element.ClearValue(Line.X2Property);
                element.ClearValue(Line.Y1Property);
                element.ClearValue(Line.Y2Property);
            }
            else if (element is Rectangle)
            {
                element.ClearValue(Rectangle.WidthProperty);
                element.ClearValue(Rectangle.HeightProperty);
            }
            else if (element is Ellipse)
            {
                element.ClearValue(Ellipse.WidthProperty);
                element.ClearValue(Ellipse.HeightProperty);
            }
        }
    }

    /// <summary>
    /// Create and compare chart points.
    /// </summary>
   public struct ChartPoint : IEquatable<ChartPoint>
   {
        /// <summary>
        /// Gets or sets point X.
        /// </summary>
        public double X
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets point Y.
        /// </summary>
        public double Y
        {
            get;
            set;
        }

        /// <summary>
        /// Called when instance created for ChartPoint.
        /// </summary>
        /// <param name="x">Used to specify <see cref="X"/> point value.</param>
        /// <param name="y">Used to specify <see cref="Y"/> point value.</param>
        public ChartPoint(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        /// <summary>
        ///  Returns the hash code for this instance.
        /// </summary>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if obj and this instance are the same type and represent the same value otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is ChartPoint))
            {
                return false;
            }

            return Equals((ChartPoint)obj);
        }

        /// <summary>
        /// Indicates whether this instance and a specified points are equal.
        /// </summary>
        /// <param name="point">The point to compare with the current instance.</param>
        /// <returns>true if x and y of <paramref name="point"/> and current instance are the same type and represent the same value otherwise, false.</returns>
        public bool Equals(ChartPoint point)
        {
            if (X != point.X)
            {
                return false;
            }

            return Y == point.Y;
        }

        /// <summary>
        /// Indicates whether the both instance and a specified points are equal.
        /// </summary>
        /// <returns>true if <paramref name="point1"/> and <paramref name="point2"/> instance are the same type and represent the same value otherwise, false.</returns>
        public static bool operator ==(ChartPoint point1, ChartPoint point2)
        {
            return point1.Equals(point2);
        }

        /// <summary>
        /// Indicates whether both the specified points are not equal.
        /// </summary>
        /// <returns>true if <paramref name="point1"/> and <paramref name="point2"/> are represent not the same value otherwise, false.</returns>
        public static bool operator !=(ChartPoint point1, ChartPoint point2)
        {
            return !point1.Equals(point2);
        }       
    }

    internal static class KeyTimeExtension
    {
        internal static NativeKeyTime GetKeyTime(this NativeKeyTime key, TimeSpan span)
        {
#if WinUI_UWP
            return KeyTimeHelper.FromTimeSpan(span);
#else
            return NativeKeyTime.FromTimeSpan(span);
#endif
        }
    }

    internal static class OrientationExtension
    {
        internal static bool Equals(this Orientation orientation,Orientation refOrientation)
        {
            return orientation.Equals(refOrientation);
        }
    }

    internal static class ThicknessExtension
    {
        internal static Thickness GetThickness(this Thickness thickness, double left,double top,double right, double bottom)
        {
#if WinUI_UWP
           return ThicknessHelper.FromLengths(left, top, right, bottom);            
#else
            return  new Thickness(left, top, right, bottom);
#endif
        }
    }

    internal static class DurationExtension
    {
        internal static Duration GetDuration(this Duration duration, TimeSpan span)
        {
#if WinUI_UWP
            return  DurationHelper.FromTimeSpan(span);
#else
            return  new Duration(span);
#endif
        }
    }

    internal static class ColorExtension
    {
        internal static Brush GetContrastColor(this Brush brush)
        {
            SolidColorBrush colorBrush = brush as SolidColorBrush;

            if (colorBrush != null)
            {
                NativeColor color = colorBrush.Color;
                float red = color.R;
                float green = color.G;
                float blue = color.B;
                double colorBrightness = 1 - (0.299 * red + 0.587 * green + 0.114 * blue) / 255;
                return colorBrightness < 0.5 ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.White);
            }
            else
            {
                return new SolidColorBrush(Colors.Black);
            }
                       
        }
    }
}
