using Microsoft.Maui.Graphics.Platform;
using Microsoft.Maui.Handlers;
using UIKit;

namespace Syncfusion.Maui.Graphics.Internals
{
    public partial class DrawableViewHandler : ViewHandler<IDrawableView, PlatformGraphicsView>
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override PlatformGraphicsView CreateNativeView()
        {
            return new PlatformGraphicsView(VirtualView) { BackgroundColor = UIColor.Clear };
        }

        /// <summary>
        /// 
        /// </summary>
        public void Invalidate()
        {
            this.NativeView?.InvalidateDrawable();
        }

        #endregion
    }
}
