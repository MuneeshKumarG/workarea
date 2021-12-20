using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public long Id { private set; get; }

        /// <summary>
        /// 
        /// </summary>
        public TouchActions Action { private set; get; }

        /// <summary>
        /// 
        /// </summary>
        public Point TouchPoint { private set; get; }

    }
}
