using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// 
    /// </summary>
    public class TouchEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <param name="touchPoint"></param>
        public TouchEventArgs(long id, TouchActions action, Point touchPoint)
        {
            Id = id;
            Action = action;
            TouchPoint = touchPoint;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <param name="deviceType"></param>
        /// <param name="touchPoint"></param>
        public TouchEventArgs(long id, TouchActions action, PointerDeviceType deviceType, Point touchPoint)
        {
            Id = id;
            Action = action;
            TouchPoint = touchPoint;
            PointerDeviceType = deviceType;
        }

        /// <summary>
        /// 
        /// </summary>
        public long Id { private set; get; }

        /// <summary>
        /// 
        /// </summary>
        public TouchActions Action { private set; get; }

        /// <summary>
        /// Returns actual touch point.
        /// </summary>
        public Point TouchPoint { private set; get; }

        /// <summary>
        /// Returns device type of the pointer.
        /// </summary>
        /// 
#if MACCATALYST
        public PointerDeviceType PointerDeviceType { internal set; get; } = PointerDeviceType.Mouse;
#else
        public PointerDeviceType PointerDeviceType { internal set; get; } = PointerDeviceType.Touch;
#endif
        /// <summary>
        /// Returns true if left mouse button pressed for desktop devices.
        /// </summary>
        public bool IsLeftButtonPressed { internal set; get; } = false;

        /// <summary>
        /// Returns true if right mouse button pressed for desktop devices.
        /// </summary>
        /// <remarks>
        /// Currently not support for Mac Catalyst
        /// </remarks>
        [UnsupportedOSPlatform("MACCATALYST")]
        public bool IsRightButtonPressed { internal set; get; } = false;
    }
}
