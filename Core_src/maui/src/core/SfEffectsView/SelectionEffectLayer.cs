// <copyright file="SelectionEffectLayer.cs" company="PlaceholderCompany">
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
    /// Represents the SelectionEffectLayer class.
    /// </summary>
    internal class SelectionEffectLayer : DrawableView
    {
        #region Fields

        /// <summary>
        /// Represents the selection transparency factor.
        /// </summary>
        private const float SelectionTransparencyFactor = 0.12f;

        /// <summary>
        /// Represents the default bounds.
        /// </summary>
        private Rectangle selectionBounds;

        /// <summary>
        /// Represents the default selection color.
        /// </summary>
        private Brush selectionColor = new SolidColorBrush(Colors.Black);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectionEffectLayer"/> class.
        /// </summary>
        /// <param name="selectionColor">The selection color.</param>
        public SelectionEffectLayer(Brush selectionColor)
        {
            this.selectionColor = selectionColor;
            this.IsEnabled = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draw method.
        /// </summary>
        /// <param name="canvas">The canvas.</param>
        /// <param name="dirtyRect">The rectangle.</param>
        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            if (this.selectionColor != null)
            {
                canvas.Alpha = SelectionTransparencyFactor;
                canvas.SetFillPaint(this.selectionColor, this.selectionBounds);
                canvas.FillRectangle(this.selectionBounds);
            }
        }

        /// <summary>
        /// Update selection bounds method.
        /// </summary>
        /// <param name="width">Width property.</param>
        /// <param name="height">Height property.</param>
        /// <param name="selectionColor">SelectionColor.</param>
        internal void UpdateSelectionBounds(double width = 0, double height = 0, Brush? selectionColor = null)
        {
            if (selectionColor == null)
            {
                selectionColor = new SolidColorBrush(Colors.Transparent);
            }

            this.selectionColor = selectionColor;
            this.selectionBounds = new Rectangle(0, 0, width, height);
            this.InvalidateDrawable();
        }

        #endregion
    }
}
