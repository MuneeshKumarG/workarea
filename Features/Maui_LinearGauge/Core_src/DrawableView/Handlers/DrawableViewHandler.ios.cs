using Microsoft.Maui.Graphics.Native;
using Microsoft.Maui.Handlers;
using UIKit;

namespace Syncfusion.Maui.Graphics.Internals
{
    public partial class DrawableViewHandler : ViewHandler<IDrawableView, NativeGraphicsView>
    {
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override NativeGraphicsView CreateNativeView()
        {
            return new NativeGraphicsView(VirtualView) { BackgroundColor = UIColor.Clear };
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
