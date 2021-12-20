﻿using System;
using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// Represents the extension class that create instance for <see cref="TouchDetector"/> class and set to target class.
    /// </summary>
    public static class TouchListenerExtension
    {
        internal static BindableProperty TouchDetectorProperty = BindableProperty.Create(nameof(TouchDetector), typeof(TouchDetector), typeof(View), null);

        /// <summary>
        /// Create the touch detector and add the listener to it. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="listener"></param>
        public static void AddTouchListener(this View target, ITouchListener listener)
        {
            var touchDetector = target.GetValue(TouchDetectorProperty) as TouchDetector;

            if (touchDetector == null)
            {
                touchDetector = new TouchDetector(target);
                target.SetValue(TouchDetectorProperty, touchDetector);
            }

            touchDetector.AddListener(listener);
        }       

        /// <summary>
        /// Remove the listener and detector. 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="listener"></param>
        public static void RemoveTouchListener(this View target, ITouchListener listener)
        {
            var touchDetector = target.GetValue(TouchDetectorProperty) as TouchDetector;

            if (touchDetector != null)
            {
                touchDetector.RemoveListener(listener);
                if(!touchDetector.HasListener())
                {
                    touchDetector.Dispose();
                    target.SetValue(TouchDetectorProperty, null);
                }
            }
        }

        /// <summary>
        /// Clear the listeners and touch detector.
        /// </summary>
        /// <param name="target"></param>
        public static void ClearTouchListeners(this View target)
        {
            var touchDetector = target.GetValue(TouchDetectorProperty) as TouchDetector;

            if (touchDetector != null)
            {
                touchDetector.Dispose();
                target.SetValue(TouchDetectorProperty, null);
            }
        }
    }
}

