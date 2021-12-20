using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Graphics.Internals
{
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// Draw lines for group points.
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="points"></param>
		/// <param name="lineDrawing"></param>
		public static void DrawLines(this ICanvas canvas, float[] points, ILineDrawing lineDrawing)
		{
		}
	}
}
