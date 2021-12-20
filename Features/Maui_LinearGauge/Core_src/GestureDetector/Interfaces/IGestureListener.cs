using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// Enable scroll and scale listener.
    /// </summary>
    public interface IGestureListener
    {
        /// <summary>
        /// Gets the boolean value indicating to pass the touches on either the parent or child view.
        /// </summary>
        /// <remarks>Only <see cref="IGestureListener"/> inheriting maui <see cref="View"/> can use this property.</remarks>
        bool IsTouchHandled
        {
            get { return false; }
        }
    }
}
