using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Runtime.Versioning;

namespace Syncfusion.Maui.Graphics.Internals
{
    /// <summary>
    /// Represents a view that can be drawn on using native drawing options. 
    /// </summary>
    public class DrawableView : View, IDrawableView
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dirtyRect"></param>
        public virtual void Draw(ICanvas canvas, RectangleF dirtyRect)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void InvalidateDrawable()
        {
            if (this.Handler is DrawableViewHandler handler)
                handler.Invalidate();
        }
    }
}
