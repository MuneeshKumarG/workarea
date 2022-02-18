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
    /// Highlight effectlayer.
    /// </summary>
    internal class HighlightEffectLayer : DrawableView
    {
        #region fields

        /// <summary>
        /// Represents highlight bounds.
        /// </summary>
        private Rectangle highlightBounds;

        /// <summary>
        /// Represents the highlight color.
        /// </summary>
        private Brush highlightColor = new SolidColorBrush(Colors.Black);

        /// <summary>
		/// Represents the highlight transparency factor.
		/// </summary>
		private const float highlightTransparencyFactor = 0.04f;

        #endregion

        #region constructor

        /// <summary>
        /// Highlight effect layer.
        /// </summary>
        /// <param name="_highlightColor">The highlight color.</param>
        public HighlightEffectLayer(Brush _highlightColor)
        {
            highlightColor = _highlightColor;
            this.IsEnabled = false;
        }
        #endregion

        #region methods

        /// <summary>
        /// The draw method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="dirtyRect">The rectangle.</param>
        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            if (highlightColor != null)
            {
                canvas.Alpha = highlightTransparencyFactor;
               
                canvas.SetFillPaint(highlightColor, this.highlightBounds);
                canvas.FillRectangle(this.highlightBounds);
            }
        }

        /// <summary>
        /// Update highlight bounds method.
        /// </summary>
        /// <param name="width">The width property.</param>
        /// <param name="height">The height property.</param>
        /// <param name="_highlightColor">The highlight color.</param>
        internal void UpdateHighlightBounds(double width = 0, double height = 0, Brush? _highlightColor = null)
        {
            if (_highlightColor == null)
            {
                _highlightColor = new SolidColorBrush(Colors.Transparent);
            }
            this.highlightColor = _highlightColor;
            this.highlightBounds = new Rectangle(0, 0, width, height);
            this.InvalidateDrawable();
        }
    }

    #endregion
}
