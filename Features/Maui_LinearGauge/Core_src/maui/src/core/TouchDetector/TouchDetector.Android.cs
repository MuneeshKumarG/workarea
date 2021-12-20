using Android.Views;
using MauiView = Microsoft.Maui.Controls.View;
using System;
using Microsoft.Maui;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class TouchDetector
    {
        internal void SubscribeNativeTouchEvents(MauiView? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                View? nativeView = handler.NativeView as View;

                if (nativeView != null)
                {
                    nativeView.Touch += OnTouch;
                }
            }
        }

        internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                View? nativeView = handler.NativeView as View;

                if (nativeView != null)
                {
                    nativeView.Touch -= OnTouch;
                }
            }
        }

        internal void OnTouch(object? sender, View.TouchEventArgs e)
        {
            
            View? nativeView = sender as View;
            
            MotionEvent? motionEvent = e.Event;

            if (nativeView == null || motionEvent == null) return;
            int actionIndex = motionEvent.ActionIndex;
            int pointerId = motionEvent.GetPointerId(actionIndex);
            Point screenPoint = new Point(motionEvent.GetX(actionIndex), motionEvent.GetY(actionIndex));
            Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
            Point point = new Point(fromPixels(screenPoint.X), fromPixels(screenPoint.Y));
            bool isHandled = touchListeners[0].IsTouchHandled;

            var handled = isHandled || motionEvent.Action == MotionEventActions.Pointer2Down || motionEvent.PointerCount > 1;

            if (nativeView.Parent != null)
            {
                nativeView.Parent.RequestDisallowInterceptTouchEvent(handled);
            }

            switch (motionEvent.ActionMasked)
            {
                case MotionEventActions.Down:
                case MotionEventActions.PointerDown:
                    OnTouchAction(pointerId, TouchActions.Pressed, point);
                    break;
                case MotionEventActions.Move:
                    OnTouchAction(pointerId, TouchActions.Moved, point);
                    break;
                case MotionEventActions.Up:
                case MotionEventActions.Pointer1Up:
                    OnTouchAction(pointerId, TouchActions.Released, point);
                    break;
                case MotionEventActions.Cancel:
                    OnTouchAction(pointerId, TouchActions.Cancelled, point);
                    break;
            }
        }
    }
}
