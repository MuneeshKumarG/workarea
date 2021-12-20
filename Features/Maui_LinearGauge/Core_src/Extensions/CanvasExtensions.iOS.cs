using CoreGraphics;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Native;
using System;
using UIKit;

namespace Syncfusion.Maui.Graphics.Internals
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class CanvasExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="value"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="textElement"></param>
        public static void DrawText(this ICanvas canvas, string value, float x, float y, ITextElement textElement)
        {
            if (canvas is NativeCanvas)
            {
                IFontManager? fontManager = MauiUIApplicationDelegate.Current.Services.GetRequiredService<IFontManager>();
                UIFont? uiFont = fontManager.GetFont(textElement.Font, textElement.FontSize);
                UIStringAttributes? uiStringAttributes = new UIStringAttributes();
                uiStringAttributes.ForegroundColor = textElement.TextColor.AsUIColor();
                uiStringAttributes.Font = uiFont;
                NSString drawText = new NSString(value);
                drawText.DrawString(new CGPoint(x, y), uiStringAttributes);
                drawText.Dispose();
            }

        }

        /// <summary>
		/// Draw the text with in specified rectangle area.
		/// </summary>
		/// <param name="canvas">The canvas value.</param>
		/// <param name="value">The text value.</param>
		/// <param name="rect">The rectangle area thet specifies the text bound.</param>
		/// <param name="horizontalAlignment">Text horizontal alignment option.</param>
		/// <param name="verticalAlignment">Text vertical alignment option.</param>
		/// <param name="textElement">The text style.</param>
        public static void DrawText(this ICanvas canvas, string value, Rectangle rect, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, ITextElement textElement)
        {
            if (canvas is NativeCanvas)
            {
                IFontManager? fontManager = MauiUIApplicationDelegate.Current.Services.GetRequiredService<IFontManager>();
                UIFont? uiFont = fontManager.GetFont(textElement.Font, textElement.FontSize);
                UIStringAttributes? uiStringAttributes = new UIStringAttributes();
                uiStringAttributes.ForegroundColor = textElement.TextColor.AsUIColor();
                uiStringAttributes.Font = uiFont;
                NSString drawText = new NSString(value);
                CGSize measuredTextSize = drawText.GetSizeUsingAttributes(uiStringAttributes);
                double x = rect.X;
                double y = rect.Y;
                if (horizontalAlignment == HorizontalAlignment.Center)
                {
                    x = x + (rect.Width / 2 - measuredTextSize.Width / 2);
                    x = x < rect.X ? rect.X : x;
                }
                else if (horizontalAlignment == HorizontalAlignment.Right)
                {
                    x = x + rect.Width - measuredTextSize.Width;
                    x = x < rect.X ? rect.X : x;
                }

                if (verticalAlignment == VerticalAlignment.Center)
                {
                    y = y + (rect.Height / 2 - measuredTextSize.Height / 2);
                    y = y < rect.Y ? rect.Y : y;
                }

                drawText.DrawString(new CGRect(x, y, rect.Width, rect.Height), uiStringAttributes);
                drawText.Dispose();
            }
        }

        /// <summary>
		/// Draw lines for group points.
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="points"></param>
		/// <param name="lineDrawing"></param>
        public static void DrawLines(this ICanvas canvas, float[] points, ILineDrawing lineDrawing)
        {
            if (canvas is NativeCanvas nativeCanvas)
            {
                CGContext context = nativeCanvas.Context;
                int j = 0;
                int count = points.Length / 2;
                CGPoint[] cgpoints = new CGPoint[count];
                for (int i = 0; i < count; i++)
                {
                    cgpoints[i] = new CGPoint(points[j++], points[j++]);
                }

                context.SaveState();
                context.SetLineWidth((nfloat)lineDrawing.StrokeWidth);
                context.SetShouldAntialias(lineDrawing.EnableAntiAliasing);
                context.AddLines(cgpoints);

                if (lineDrawing.StrokeDashArray != null && lineDrawing.StrokeDashArray.Count > 0)
                {
                    context.SetLineDash(0, GetNativeDashArrays(lineDrawing.StrokeDashArray));
                }

                context.SetAlpha(((NSNumber)lineDrawing.Opacity).FloatValue);
                context.SetStrokeColor(lineDrawing.Stroke.AsCGColor());

                context.StrokePath();
                context.RestoreState();
            }
        }

        /// <summary>
        /// Method used to convert <see cref="DoubleCollection"/> to native float array.
        /// </summary>
        /// <param name="dashes"></param>
        /// <returns></returns>
        private static nfloat[]? GetNativeDashArrays(DoubleCollection dashes)
        {
            if (dashes != null)
            {
                int count = dashes.Count;
                nfloat[] strokeDashes = new nfloat[count];

                int i = 0;
                foreach (var value in dashes)
                {
                    strokeDashes[i] = ((NSNumber)value).FloatValue;
                    i++;
                }

                return strokeDashes;
            }

            return null;
        }
    }
}
