using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Core.Internals
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TouchDetector : IDisposable
    {
        private readonly List<ITouchListener> touchListeners;
        private bool _disposed;
        internal readonly View MauiView;
        internal bool InputTransparent;
        internal bool IsEnabled;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mauiView"></param>
        public TouchDetector(View mauiView)
        {
            MauiView = mauiView;
            touchListeners = new List<ITouchListener>();
            if (mauiView.Handler != null)
            {
                SubscribeNativeTouchEvents(mauiView);
            }
            else
            {
                mauiView.HandlerChanged += MauiView_HandlerChanged;
                mauiView.HandlerChanging += MauiView_HandlerChanging;
            }
            mauiView.PropertyChanged += MauiView_PropertyChanged;
            IsEnabled = mauiView.IsEnabled;
            InputTransparent = mauiView.InputTransparent;
        }

        private void MauiView_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == VisualElement.InputTransparentProperty.PropertyName)
                InputTransparent = MauiView.InputTransparent;
            else if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
                IsEnabled = MauiView.IsEnabled;
        }
       
        private void MauiView_HandlerChanged(object? sender, EventArgs e)
        {
            if (sender is View view && view.Handler != null)
                SubscribeNativeTouchEvents(view);
        }

        private void MauiView_HandlerChanging(object? sender, HandlerChangingEventArgs e)
        {
            UnsubscribeNativeTouchEvents(e.OldHandler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public void AddListener(ITouchListener listener)
        {
            if (!touchListeners.Contains(listener))
                touchListeners.Add(listener);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listener"></param>
        public void RemoveListener(ITouchListener listener)
        {
            if (touchListeners.Contains(listener))
                touchListeners.Remove(listener);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool HasListener()
        {
            return touchListeners.Count > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearListeners()
        {
            touchListeners.Clear();
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
                this.ClearListeners();
                this.Unsubscribe(MauiView);
            }
        }

        internal void OnTouchAction(long pointerId, TouchActions action, Point point)
        {
            TouchEventArgs eventArgs = new TouchEventArgs(pointerId, action, point);
            OnTouchAction(eventArgs);
        }

        internal void OnTouchAction(long pointerId, TouchActions action, PointerDeviceType deviceType , Point point)
        {
            TouchEventArgs eventArgs = new TouchEventArgs(pointerId, action, deviceType, point);
            OnTouchAction(eventArgs);
        }

        internal void OnTouchAction(TouchEventArgs eventArgs)
        {
            foreach (var listener in touchListeners)
            {
                listener.OnTouch(eventArgs);
            }
        }

        internal void OnScrollAction(long pointerId, Point origin, double direction)
        {
            ScrollEventArgs eventArgs = new ScrollEventArgs(pointerId, origin, direction);
            foreach (var listener in touchListeners)
            {
                listener.OnScrollWheel(eventArgs);
            }
        }

        /// <summary>
        /// Unsubscribe the events.
        /// </summary>
        private void Unsubscribe(View? mauiView)
        {
            if (mauiView != null)
            {
                UnsubscribeNativeTouchEvents(mauiView.Handler!);
                mauiView.HandlerChanged -= MauiView_HandlerChanged;
                mauiView.HandlerChanging -= MauiView_HandlerChanging;
                MauiView.PropertyChanged -= MauiView_PropertyChanged;
                mauiView = null;
            }
        }
    }
}
