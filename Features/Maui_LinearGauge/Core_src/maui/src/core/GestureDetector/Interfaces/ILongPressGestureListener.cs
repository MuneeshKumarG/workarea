using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// This interface used to call the long press gesture method.
    /// </summary>
    public interface ILongPressGestureListener : IGestureListener
    {
        /// <summary>
        /// Invoke on long press interaction.
        /// </summary>
        /// <param name="e"></param>
        void OnLongPress(LongPressEventArgs e);
    }
}
