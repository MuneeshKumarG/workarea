using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// This class serves as an event data for the panning action on the view.
    /// </summary>
    public interface IPanGestureListener : IGestureListener
    {
        /// <summary>
        /// Invoke on panning interaction.
        /// </summary>
        void OnPan(PanEventArgs e);

    }
}
