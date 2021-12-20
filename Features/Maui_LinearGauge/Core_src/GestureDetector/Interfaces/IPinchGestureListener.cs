using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// This interface used to call the pinch gesture method.
    /// </summary>
    public interface IPinchGestureListener : IGestureListener
    {
        /// <summary>
        /// Invoke on scale interaction.
        /// </summary>
        void OnPinch(PinchEventArgs e);
    }
}
