﻿using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Charts
{
    internal static class ChartMath
    {
        /// <summary>
        /// Subtracts the thickness.
        /// </summary>
        /// <param name="rect">The Rectangle value.</param>
        /// <param name="thickness">The thickness.</param>
        /// <returns>The Rectangle</returns>
        public static RectF SubtractThickness(this Rect rect, Thickness thickness)
        {
            rect.X += thickness.Left;
            rect.Y += thickness.Top;
            if (rect.Width > thickness.Left + thickness.Right)
            {
                rect.Width -= thickness.Left + thickness.Right;
            }
            else
            {
                rect.Width = 0;
            }

            if (rect.Height > (thickness.Top + thickness.Bottom))
            {
                rect.Height -= thickness.Top + thickness.Bottom;
            }
            else
            {
                rect.Height = 0;
            }

            return rect;
        }

        public static float DegreeToRadian(float angle)
        {
            return (float)(Math.PI * angle) / 180;
        }

        public static double RadianToDegree(double degree)
        {
            return (degree * 180) / Math.PI;
        }

        public static double MinMax(double value, double min, double max)
        {
            return value > max ? max : (value < min ? min : value);
        }
    }
}
