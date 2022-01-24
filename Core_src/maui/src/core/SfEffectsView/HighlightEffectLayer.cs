// <copyright file="HighlightEffectLayer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;
    using Syncfusion.Maui.Graphics.Internals;

    /// <summary>
    /// Represents the HighlightEffectLayer class.
    /// </summary>
    internal class HighlightEffectLayer : DrawableView
    {
        #region Fields

        /// <summary>
        /// Represents the highlight transparency factor.
        /// </summary>
        private const float HighlightTransparencyFactor = 0.04f;

        /// <summary>
        /// Represents highlight bounds.
        /// </summary>
        private Rectangle highlightBounds;

        /// <summary>
        /// Represents the highlight color.
        /// </summary>
        private Brush highlightColor = new SolidColorBrush(Colors.Black);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightEffectLayer"/> class.
        /// </summary>
        /// <param name="highlightColor">The highlight color.</param>
        public HighlightEffectLayer(Brush highlightColor)
        {
            this.highlightColor = highlightColor;
            this.IsEnabled = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The draw method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="dirtyRect">The rectangle.</param>
        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            if (this.highlightColor != null)
            {
                canvas.Alpha = HighlightTransparencyFactor;
                canvas.SetFillPaint(this.highlightColor, this.highlightBounds);
                canvas.FillRectangle(this.highlightBounds);
            }
        }

        /// <summary>
        /// Update highlight bounds method.
        /// </summary>
        /// <param name="width">The width property.</param>
        /// <param name="height">The height property.</param>
        /// <param name="highlightColor">The highlight color.</param>
        internal void UpdateHighlightBounds(double width = 0, double height = 0, Brush? highlightColor = null)
        {
            if (highlightColor == null)
            {
                highlightColor = new SolidColorBrush(Colors.Transparent);
            }

            this.highlightColor = highlightColor;
            this.highlightBounds = new Rectangle(0, 0, width, height);
            this.InvalidateDrawable();
        }
    }

    #endregion
}
