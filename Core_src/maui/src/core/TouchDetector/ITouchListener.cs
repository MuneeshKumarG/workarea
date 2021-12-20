using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITouchListener
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void OnTouch(TouchEventArgs e);

        /// <summary>
        /// Gets the boolean value indicating to pass the touches on either the parent or child view.
        /// </summary>
        bool IsTouchHandled
        {
            get { return false; }
        }
    }
}
