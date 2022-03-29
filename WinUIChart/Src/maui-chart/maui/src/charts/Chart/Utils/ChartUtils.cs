using Microsoft.Maui.Graphics;
using Color = Microsoft.Maui.Graphics.Color;
using PointF = Microsoft.Maui.Graphics.PointF;
using Rect = Microsoft.Maui.Graphics.Rect;
using Syncfusion.Maui.Core.Internals;
using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;

#if __ANDROID__
using Android.Graphics;
#elif IOS || MACCATALYST
using Foundation;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using ObjCRuntime;
#else
using Microsoft.Maui.Controls.Shapes;
#endif

namespace Syncfusion.Maui.Charts
{
    internal static class ChartUtils
    {
        internal static double ConvertToDouble(object? val)
        {
            if (val == null)
            {
                return double.NaN;
            }

            double doubleVal;

            if (double.TryParse(val.ToString(), out doubleVal))
            {
                return doubleVal;
            }

            //Maui-884 The Following date time formats was acceptable for CrossesAt value. ("MM/dd/yyyy"),("dddd, dd MMMM yyyy"),("dddd, dd MMMM yyyy HH:mm:ss"),("MM/dd/yyyy HH:mm"),("MM/dd/yyyy hh:mm tt"),("MM/dd/yyyy H:mm"),("MM/dd/yyyy h:mm tt"),("MM/dd/yyyy HH:mm:ss"),
            //("MMMM dd"),("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss.fffffffK"),("ddd, dd MMM yyy HH’:’mm’:’ss ‘GMT’"),("yyyy’-‘MM’-‘dd’T’HH’:’mm’:’ss"),("HH:mm"),("hh:mm tt"),("H:mm"),("h:mm tt"),("HH:mm:ss"),("yyyy MMMM") .
            DateTime date;

            if (DateTime.TryParse(val.ToString(), out date))
            {
                if (date == DateTime.MaxValue)
                {
                    return double.MaxValue;
                }
                else if (date == DateTime.MinValue)
                {
                    return double.MinValue;
                }

                return date.ToOADate();
            }

            return double.NaN;
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

        internal static string Tostring(this object? obj)
        {
            var value = obj?.ToString();
            if (value != null)
            {
                return value;
            }

            return string.Empty;
        }

        internal static Size GetRotatedSize(Size measuredSize, float degrees)
        {
#if __ANDROID__
            var size = (SizeF)measuredSize;
            float centerX = size.Width / 2;
            float centerY = size.Height / 2;

            Matrix transform = new Matrix();
            transform.SetRotate(degrees, centerX, centerY);

            float[] pts = new float[2];
            pts[0] = size.Width;
            pts[1] = size.Height;
            transform.MapPoints(pts);
            System.Drawing.Point rightBottom = new System.Drawing.Point((int)pts[0], (int)pts[1]);

            pts[0] = size.Width;
            pts[1] = 0;
            transform.MapPoints(pts);
            System.Drawing.Point rightTop = new System.Drawing.Point((int)pts[0], (int)pts[1]);

            pts[0] = 0;
            pts[1] = 0;
            transform.MapPoints(pts);
            System.Drawing.Point leftTop = new System.Drawing.Point((int)pts[0], (int)pts[1]);

            pts[0] = 0;
            pts[1] = size.Height;
            transform.MapPoints(pts);
            System.Drawing.Point leftBottom = new System.Drawing.Point((int)pts[0], (int)pts[1]);

            float left = Math.Min(Math.Min(leftTop.X, rightTop.X), Math.Min(leftBottom.X, rightBottom.X));
            float top = Math.Min(Math.Min(leftTop.Y, rightTop.Y), Math.Min(leftBottom.Y, rightBottom.Y));
            float right = Math.Max(Math.Max(leftTop.X, rightTop.X), Math.Max(leftBottom.X, rightBottom.X));
            float bottom = Math.Max(Math.Max(leftTop.Y, rightTop.Y), Math.Max(leftBottom.Y, rightBottom.Y));

            return new Size(right - left, bottom - top);
#elif  IOS || MACCATALYST
            nfloat angleInRadians = (nfloat)((2 * Math.PI * degrees) / 360f);

            CGAffineTransform transform = CGAffineTransform.MakeRotation(angleInRadians);

            CGPoint leftTop = transform.TransformPoint(CGPoint.Empty);
            CGPoint rightTop = transform.TransformPoint(new CGPoint(measuredSize.Width, 0.0));
            CGPoint leftBottom = transform.TransformPoint(new CGPoint(0.0, measuredSize.Height));
            CGPoint rightBottom = transform.TransformPoint(new CGPoint(measuredSize.Width, measuredSize.Height));

            float left = (float)Math.Min(Math.Min(leftTop.X, rightTop.X), Math.Min(leftBottom.X, rightBottom.X));
            float top = (float)Math.Min(Math.Min(leftTop.Y, rightTop.Y), Math.Min(leftBottom.Y, rightBottom.Y));
            float right = (float)Math.Max(Math.Max(leftTop.X, rightTop.X), Math.Max(leftBottom.X, rightBottom.X));
            float bottom = (float)Math.Max(Math.Max(leftTop.Y, rightTop.Y), Math.Max(leftBottom.Y, rightBottom.Y));

            return new SizeF(right - left, bottom - top);
#else
            var angleRadians = (2 * Math.PI * degrees) / 360;
            var sine = Math.Sin(angleRadians);
            var cosine = Math.Cos(angleRadians);
            var matrix = new Matrix(cosine, sine, -sine, cosine, 0, 0);

            var leftTop = matrix.Transform(new Point(0, 0));
            var rightTop = matrix.Transform(new Point(measuredSize.Width, 0));
            var leftBottom = matrix.Transform(new Point(0, measuredSize.Height));
            var rightBottom = matrix.Transform(new Point(measuredSize.Width, measuredSize.Height));
            var left = Math.Min(Math.Min(leftTop.X, rightTop.X), Math.Min(leftBottom.X, rightBottom.X));
            var top = Math.Min(Math.Min(leftTop.Y, rightTop.Y), Math.Min(leftBottom.Y, rightBottom.Y));
            var right = Math.Max(Math.Max(leftTop.X, rightTop.X), Math.Max(leftBottom.X, rightBottom.X));
            var bottom = Math.Max(Math.Max(leftTop.Y, rightTop.Y), Math.Max(leftBottom.Y, rightBottom.Y));

            return new Size(right - left, bottom - top);
#endif
        }
        internal static Color GetContrastColor(Color? color)
        {
            return color?.GetLuminosity() > 0.5 ? Colors.Black : Colors.White;
        }

        internal static Core.ShapeType GetShapeType(ChartLegendIconType iconType)
        {
            switch (iconType)
            {
                case ChartLegendIconType.Circle:
                    return Core.ShapeType.Circle;
                case ChartLegendIconType.Rectangle:
                    return Core.ShapeType.Rectangle;
                case ChartLegendIconType.HorizontalLine:
                    return Core.ShapeType.HorizontalLine;
                case ChartLegendIconType.Diamond:
                    return Core.ShapeType.Diamond;
                case ChartLegendIconType.Pentagon:
                    return Core.ShapeType.Pentagon;
                case ChartLegendIconType.Triangle:
                    return Core.ShapeType.Triangle;
                case ChartLegendIconType.InvertedTriangle:
                    return Core.ShapeType.InvertedTriangle;
                case ChartLegendIconType.Cross:
                    return Core.ShapeType.Cross;
                case ChartLegendIconType.Plus:
                    return Core.ShapeType.Plus;
                case ChartLegendIconType.Hexagon:
                    return Core.ShapeType.Hexagon;
                case ChartLegendIconType.VerticalLine:
                    return Core.ShapeType.VerticalLine;
            }

            return Core.ShapeType.Circle;
        }

        //Calculating series clip rect with chart title.
        internal static Rect GetSeriesClipRect(Rect seriesClipRect, double titleHeight)
        {
            return new Rect(
                seriesClipRect.X,
                seriesClipRect.Y + titleHeight,
                seriesClipRect.Width,
                seriesClipRect.Height);
        }

        internal static bool SegmentContains(LineSegment segment, PointF touchPoint, ChartSeries series)
        {
            var pointX = touchPoint.X;
            var pointY = touchPoint.Y;
            var defaultSelectionStrokeWidth = series.DefaultSelectionStrokeWidth;
            var leftPoint = new PointF(pointX - defaultSelectionStrokeWidth, pointY - defaultSelectionStrokeWidth);
            var rightPoint = new PointF(pointX + defaultSelectionStrokeWidth, pointY + defaultSelectionStrokeWidth);
            var topPoint = new PointF(pointX + defaultSelectionStrokeWidth, pointY - defaultSelectionStrokeWidth);
            var botPoint = new PointF(pointX - defaultSelectionStrokeWidth, pointY + defaultSelectionStrokeWidth);
            var startSegment = new PointF(segment.X1, segment.Y1);
            var endSegment = new PointF(segment.X2, segment.Y2);

            if (LineContains(startSegment, endSegment, leftPoint, rightPoint) ||
                LineContains(startSegment, endSegment, topPoint, botPoint))
            {
                return true;
            }

            //Todo: StepLineSeries

            return false;
        }

        internal static bool SegmentContains(SplineSegment segment, PointF touchPoint, ChartSeries series)
        {
            var pointX = touchPoint.X;
            var pointY = touchPoint.Y;
            var defaultSelectionStrokeWidth = series.DefaultSelectionStrokeWidth;
            var leftPoint = new PointF(pointX - defaultSelectionStrokeWidth, pointY - defaultSelectionStrokeWidth);
            var rightPoint = new PointF(pointX + defaultSelectionStrokeWidth, pointY + defaultSelectionStrokeWidth);
            var topPoint = new PointF(pointX + defaultSelectionStrokeWidth, pointY - defaultSelectionStrokeWidth);
            var botPoint = new PointF(pointX - defaultSelectionStrokeWidth, pointY + defaultSelectionStrokeWidth);
            var startPoint = new PointF(segment.X1, segment.Y1);
            var endPoint = new PointF(segment.X2, segment.Y2);
            var startControlPoint = new PointF(segment.StartControlX, segment.StartControlY);
            var endControlPoint = new PointF(segment.EndControlX, segment.EndControlY);

            if (LineContains(startPoint, startControlPoint, leftPoint, rightPoint) ||
                LineContains(startPoint, startControlPoint, topPoint, botPoint) ||
                LineContains(startControlPoint, endControlPoint, leftPoint, rightPoint) ||
                LineContains(startControlPoint, endControlPoint, topPoint, botPoint) ||
                LineContains(endControlPoint, endPoint, leftPoint, rightPoint) ||
                LineContains(endControlPoint, endPoint, topPoint, botPoint))
            {
                return true;
            }

            return false;
        }

        internal static bool LineContains(PointF segmentStartPoint, PointF segmentEndPoint, PointF touchStartPoint, PointF touchEndPoint)
        {
            int topPos = GetPointDirection(segmentStartPoint, segmentEndPoint, touchStartPoint);
            int botPos = GetPointDirection(segmentStartPoint, segmentEndPoint, touchEndPoint);
            int leftPos = GetPointDirection(touchStartPoint, touchEndPoint, segmentStartPoint);
            int rightPos = GetPointDirection(touchStartPoint, touchEndPoint, segmentEndPoint);

            return topPos != botPos && leftPos != rightPos;
        }

        private static int GetPointDirection(PointF point1, PointF point2, PointF point3)
        {
            int value = (int)(((point2.Y - point1.Y) * (point3.X - point2.X)) - ((point2.X - point1.X) * (point3.Y - point2.Y)));
            if (value == 0)
            {
                return 0;
            }

            return (value > 0) ? 1 : 2;
        }

        internal static Color ToColor(this Brush? brush)
        {
            if (brush is SolidColorBrush solidBrush)
                return solidBrush.Color;

            return Colors.Transparent;
        }
    }
}
