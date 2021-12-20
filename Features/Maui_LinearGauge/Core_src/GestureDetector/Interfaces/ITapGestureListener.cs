using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// This interface used to call the tap gesture method.
    /// </summary>
    public interface ITapGestureListener : IGestureListener
    {
        /// <summary>
        /// Invoke on tap interaction.
        /// </summary>
        /// <param name="e"></param>
        void OnTap(TapEventArgs e);

    }
}
