using System;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// This interface used to call the double tap gesture method.
    /// </summary>
    public interface IDoubleTapGestureListener : IGestureListener
    {
        /// <summary>
        /// Invoke on tap interaction.
        /// </summary>
        /// <param name="e"></param>
        void OnDoubleTap(TapEventArgs e);

    }
}

