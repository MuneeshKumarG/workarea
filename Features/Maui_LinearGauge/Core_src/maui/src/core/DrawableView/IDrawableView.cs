using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Graphics.Internals
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDrawableView : IView, IDrawable
    {
        /// <summary>
        /// 
        /// </summary>
        void InvalidateDrawable();
    }
}
