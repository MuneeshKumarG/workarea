using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{
	/// <summary>
	/// Selection EffectLayer.
	/// </summary>
	internal class SelectionEffectLayer : DrawableView
	{
		#region fields

		/// <summary>
		/// Represents the default bounds.
		/// </summary>
		private Rectangle selectionBounds;

		/// <summary>
		/// Represents the default selection color.
		/// </summary>
		private Brush selectionColor = new SolidColorBrush(Colors.Black);

		/// <summary>
		/// Represents the selection transparency factor.
		/// </summary>
		private const float selectionTransparencyFactor = 0.12f;

        #endregion

		#region constructor

		/// <summary>
		/// SelectionEffectLayer
		/// </summary>
		/// <param name="_selectionColor"></param>
		public SelectionEffectLayer(Brush _selectionColor)
		{
			selectionColor = _selectionColor;
			this.IsEnabled = false;
		}

		#endregion

		/// <summary>
		/// Draw method.
		/// </summary>
		/// <param name="canvas">The canvas.</param>
		/// <param name="dirtyRect">The rectangle.</param>
		public override void Draw(ICanvas canvas, RectangleF dirtyRect)
		{
			if (selectionColor != null)
			{
				canvas.Alpha = selectionTransparencyFactor;
				canvas.SetFillPaint(selectionColor, this.selectionBounds);
				canvas.FillRectangle(this.selectionBounds);
			}
		}

		/// <summary>
		/// Update selection bounds method.
		/// </summary>
		/// <param name="width">Width property</param>
		/// <param name="height">Height property.</param>
		/// <param name="_selectionColor">SelectionColor</param>
		internal void UpdateSelectionBounds(double width = 0, double height = 0, Brush? _selectionColor = null)
		{

			if (_selectionColor == null)
			{
				_selectionColor = new SolidColorBrush(Colors.Transparent);
			}
			this.selectionColor = _selectionColor;
			this.selectionBounds = new Rectangle(0, 0, width, height);
			this.InvalidateDrawable();
		}
	}
}
