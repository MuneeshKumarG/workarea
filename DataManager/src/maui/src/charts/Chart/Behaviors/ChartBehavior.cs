﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core.Internals;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The ChartBehavior is the base class for <see cref="ChartSelectionBehavior"/>, <see cref="ChartTooltipBehavior"/>, <see cref="ChartTrackballBehavior"/>, and <see cref="ChartZoomPanBehavior"/>.
    /// </summary>
    public abstract class ChartBehavior : BindableObject
    {

        #region Methods
        internal void OnTapped(IChart chart, Point touchPoint, int tapCount)
        {
            if (tapCount == 1)
            {
                OnSingleTap(chart, (float)touchPoint.X, (float)touchPoint.Y);
            }
            else if(tapCount == 2)
            {
                OnDoubleTap(chart, (float)touchPoint.X, (float)touchPoint.Y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void OnTouchDown(float pointX, float pointY)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void OnTouchCancel(float pointX, float pointY)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void OnTouchUp(IChart chart, float pointX, float pointY)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void OnTouchMove(IChart chart, float pointX, float pointY, PointerDeviceType pointerDeviceType)
        {
        }

        internal virtual void OnSingleTap(IChart chart, float pointX, float pointY)
        {

        }

        internal virtual void OnDoubleTap(IChart chart, float x, float y)
        {

        }

        internal virtual void SetTouchHandled(IChart chart)
        {
 
        }

        internal virtual void OnLongPressActivation(IChart chart, float x, float y)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void OnTouchDown(IChart chart, float pointX, float pointY)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        internal virtual void OnLongPress(IChart chart, float pointX, float pointY)
        {

        }

        internal virtual void OnTouchExit()
        {

        }

        #endregion
    }
}
