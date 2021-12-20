using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// Enables MAUI view to recognize scale and scroll interaction.
    /// </summary>
    public partial class GestureDetector : IDisposable
    {
        private List<ITapGestureListener>? tapGestureListeners;
        private List<IDoubleTapGestureListener>? doubleTapGestureListeners;
        private List<IPinchGestureListener>? pinchGestureListeners;
        private List<IPanGestureListener>? panGestureListeners;
        private List<ILongPressGestureListener>? longPressGestureListeners;
        private bool _disposed;
        internal readonly View MauiView;

        /// <summary>
        /// Invoke on Gesture listener created
        /// </summary>
        /// <param name="mauiView"> is type of <see cref="IGestureListener"/> </param>
        public GestureDetector(View mauiView)
        {
            this.MauiView = mauiView;

            if (mauiView.Handler != null)
            {
                SubscribeNativeGestureEvents(mauiView);
            }
            else
            {
                mauiView.HandlerChanged += MauiView_HandlerChanged;
                mauiView.HandlerChanging += MauiView_HandlerChanging;
            }
        }

        private void MauiView_HandlerChanged(object? sender, EventArgs e)
        {
            if (sender is View view && view.Handler != null)
                SubscribeNativeGestureEvents(view);
        }

        private void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
        {
            UnsubscribeNativeGestureEvents(e.OldHandler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public void AddListener(IGestureListener listener)
        {
            if (listener is IPanGestureListener panGesture)
            {
                if (panGestureListeners == null)
                    panGestureListeners = new List<IPanGestureListener>();
                panGestureListeners.Add(panGesture);
            }

            if (listener is IPinchGestureListener pinchGesture)
            {
                if (pinchGestureListeners == null)
                    pinchGestureListeners = new List<IPinchGestureListener>();
                pinchGestureListeners.Add(pinchGesture);
            }

            if (listener is ILongPressGestureListener longPressGesture)
            {
                if (longPressGestureListeners == null)
                    longPressGestureListeners = new List<ILongPressGestureListener>();
                longPressGestureListeners.Add(longPressGesture);
            }

            if (listener is ITapGestureListener tapGesture)
            {
                if (tapGestureListeners == null)
                    tapGestureListeners = new List<ITapGestureListener>();
                tapGestureListeners.Add(tapGesture);
            }

            if (listener is IDoubleTapGestureListener doubleTapGesture)
            {
                if (doubleTapGestureListeners == null)
                    doubleTapGestureListeners = new List<IDoubleTapGestureListener>();
                doubleTapGestureListeners.Add(doubleTapGesture);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public void RemoveListener(IGestureListener listener)
        {
            if (listener is IPanGestureListener panGesture && panGestureListeners != null && panGestureListeners.Contains(panGesture))
                panGestureListeners.Remove(panGesture);

            if (listener is IPinchGestureListener pinchGesture && pinchGestureListeners != null && pinchGestureListeners.Contains(pinchGesture))
                pinchGestureListeners.Remove(pinchGesture);

            if (listener is ILongPressGestureListener longPressGesture && longPressGestureListeners != null && longPressGestureListeners.Contains(longPressGesture))
                longPressGestureListeners.Remove(longPressGesture);

            if (listener is ITapGestureListener tapGesture && tapGestureListeners != null && tapGestureListeners.Contains(tapGesture))
                tapGestureListeners.Remove(tapGesture);

            if (listener is IDoubleTapGestureListener doubleTapGesture && doubleTapGestureListeners != null && doubleTapGestureListeners.Contains(doubleTapGesture))
                doubleTapGestureListeners.Remove(doubleTapGesture);
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearListeners()
        {
            tapGestureListeners?.Clear();
            doubleTapGestureListeners?.Clear();
            pinchGestureListeners?.Clear();
            panGestureListeners?.Clear();
            longPressGestureListeners?.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasListener()
        {
            return tapGestureListeners?.Count > 0 || doubleTapGestureListeners?.Count > 0 || longPressGestureListeners?.Count > 0
                || pinchGestureListeners?.Count > 0 || panGestureListeners?.Count > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _disposed = true;

            if (disposing)
            {
                ClearListeners();
                this.Unsubscribe(MauiView);
            }
        }


        /// <summary>
        /// Invoke on pinch interaction.
        /// </summary>
        /// <param name="state">Type of<see cref="GestureStatus"/> </param>
        /// <param name="point">Type of <see cref="Point"/></param>
        /// <param name="pinchAngle">Type of <see cref="double"/></param>
        /// <param name="scale">Type of <see cref="float"/></param>
        internal virtual void OnPinch(GestureStatus state, Point point, double pinchAngle, float scale)
        {
            if (pinchGestureListeners != null)
            {
                PinchEventArgs eventArgs = new PinchEventArgs(state, point, pinchAngle, scale);
                foreach (var listener in pinchGestureListeners)
                {
                    listener.OnPinch(eventArgs);
                }
            }
        }

        /// <summary>
        /// Invoke on pan interaction.
        /// </summary>
        /// <param name="startPoint">Type of <see cref="Point"/></param>
        /// <param name="scalePoint">Type of <see cref="Point"/></param>
        internal virtual void OnScroll(Point startPoint, Point scalePoint)
        {
            if (panGestureListeners != null)
            {
                PanEventArgs eventArgs = new PanEventArgs(startPoint, scalePoint);
                foreach (var listener in panGestureListeners)
                {
                    listener.OnPan(eventArgs);
                }
            }
        }

        /// <summary>
        /// Invoke on double tap interaction.
        /// </summary>
        internal virtual void OnTapped(Point touchPoint, int tapCount)
        {
            TapEventArgs eventArgs;
            if (tapCount == 1 && tapGestureListeners != null)
            {
                eventArgs = new TapEventArgs(touchPoint, tapCount);
                foreach (var listener in tapGestureListeners)
                {
                    listener.OnTap(eventArgs);
                }
            }

            if (tapCount == 2 && doubleTapGestureListeners != null)
            {
                eventArgs = new TapEventArgs(touchPoint, tapCount);
                foreach (var listener in doubleTapGestureListeners)
                {
                    listener.OnDoubleTap(eventArgs);
                }
            }
        }

        /// <summary>
        /// Invoke on long press interaction.
        /// </summary>
        internal virtual void OnLongPress(Point touchPoint)
        {
            if (longPressGestureListeners != null)
            {
                LongPressEventArgs eventArgs = new LongPressEventArgs(touchPoint);
                foreach (var listener in longPressGestureListeners)
                {
                    listener.OnLongPress(eventArgs);
                }
            }
        }

       /// <summary>
       /// Unsubscribe the events 
       /// </summary>
       /// <param name="mauiView"></param>
        private void Unsubscribe(View? mauiView)
        {
            if (mauiView != null)
            {
                UnsubscribeNativeGestureEvents(mauiView.Handler);
                mauiView.HandlerChanged -= MauiView_HandlerChanged;
                mauiView.HandlerChanging -= MauiView_HandlerChanging;
                mauiView = null;
            }
        }

        
    }
}
