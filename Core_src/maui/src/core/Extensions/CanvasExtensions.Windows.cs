using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Win2D;
using Microsoft.Maui.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

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
			if (canvas is W2DCanvas w2DCanvas)
			{
				using (var format = new CanvasTextFormat())
				{
					IFontManager? fontManager = MauiWinUIApplication.Current.Services.GetRequiredService<IFontManager>();
					var font = textElement.Font;
					format.FontFamily = fontManager.GetFontFamily(font).ToString();
					format.FontSize = (float)textElement.FontSize;
					format.FontStyle = font.ToFontStyle();
					format.FontWeight = font.ToFontWeight();
					w2DCanvas.Session.DrawText(value, new Vector2(x, y), textElement.TextColor.AsColor(), format);
				}
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
			if (canvas is W2DCanvas w2DCanvas)
			{
				using (var format = new CanvasTextFormat())
				{
					IFontManager? fontManager = MauiWinUIApplication.Current.Services.GetRequiredService<IFontManager>();
					var font = textElement.Font;
					format.FontFamily = fontManager.GetFontFamily(font).ToString();
					format.FontSize = (float)textElement.FontSize;
					format.FontStyle = font.ToFontStyle();
					format.FontWeight = font.ToFontWeight();

					CanvasVerticalAlignment canvasVerticallAlignment = CanvasVerticalAlignment.Top;
					if (verticalAlignment == VerticalAlignment.Center)
					{
						canvasVerticallAlignment = CanvasVerticalAlignment.Center;
					}
					else if (verticalAlignment == VerticalAlignment.Bottom)
					{
						canvasVerticallAlignment = CanvasVerticalAlignment.Bottom;
					}

					format.VerticalAlignment = canvasVerticallAlignment;
					CanvasHorizontalAlignment canvasHorizontalAlignment = CanvasHorizontalAlignment.Left;
					if (horizontalAlignment == HorizontalAlignment.Center)
					{
						canvasHorizontalAlignment = CanvasHorizontalAlignment.Center;
					}
					else if (horizontalAlignment == HorizontalAlignment.Right)
					{
						canvasHorizontalAlignment = CanvasHorizontalAlignment.Right;
					}

					format.HorizontalAlignment = canvasHorizontalAlignment;
					format.Options = CanvasDrawTextOptions.Clip;
					w2DCanvas.Session.DrawText(value, new Windows.Foundation.Rect(rect.X, rect.Y, rect.Width, rect.Height), textElement.TextColor.AsColor(), format);
				}
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
			if (canvas is W2DCanvas w2DCanvas)
			{
				int j = 0;

				w2DCanvas.StrokeSize = (float)lineDrawing.StrokeWidth;
				w2DCanvas.StrokeColor = lineDrawing.Stroke;
				w2DCanvas.Antialias = lineDrawing.EnableAntiAliasing;
				w2DCanvas.Alpha = lineDrawing.Opacity;

				if (lineDrawing.StrokeDashArray != null)
					w2DCanvas.StrokeDashPattern = lineDrawing.StrokeDashArray.ToFloatArray();
				//Draw path.

				PathF pathF = new PathF();
				while (j + 1 < points.Length)
				{
					pathF.LineTo(points[j++], points[j++]);
				}
				w2DCanvas.DrawPath(pathF);
			}
		}
	}
}
