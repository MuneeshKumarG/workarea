using Android.Views;
using MauiView = Microsoft.Maui.Controls.View;
using AGestureDetector = Android.Views.GestureDetector;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using GestureStatus = Microsoft.Maui.Controls.GestureStatus;
using System;
using Microsoft.Maui.Platform;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class GestureDetector
    {
        private ScaleListener? scaleListener;
        private ScaleGestureDetector? scaleGestureDetector;

        private ScrollListener? scrollListener;
        private AGestureDetector? scrollGestureDetector;
        private double pinchAngle = double.NaN;

        internal void SubscribeNativeGestureEvents(MauiView? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                View? nativeView = handler.NativeView as View;

                if (nativeView != null)
                {
                    // TODO : If dynamically add the gesture listeners, it won't work. Because below native gesture listeners created based on listener collection count, but dynamic case, this method executed before listener added to the collection. Need to do it proper and optimized way. 
                    if (pinchGestureListeners?.Count > 0)
                    {
                        scaleListener = new ScaleListener(this);
                        scaleGestureDetector = new ScaleGestureDetector(nativeView.Context, scaleListener);
                    }

                    if (panGestureListeners?.Count > 0 || longPressGestureListeners?.Count > 0 ||
                        tapGestureListeners?.Count > 0 || doubleTapGestureListeners?.Count > 0)
                    {
                        scrollListener = new ScrollListener(this);
                        scrollGestureDetector = new AGestureDetector(nativeView.Context, scrollListener);
                    }

                    nativeView.Touch += NativeView_Touch; ;
                }
            }
        }

        private void NativeView_Touch(object? sender, View.TouchEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            var motionEvent = e.Event;

            if (motionEvent != null)
            {
                int pointer1Index = motionEvent.FindPointerIndex(0);
                int pointer2Index = motionEvent.FindPointerIndex(1);

                View? nativeView = sender as View;

                if (nativeView == null) return;

                Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
                Point point1 = Point.Zero;
                Point point2 = Point.Zero;

                if (pointer1Index != -1)
                {
                    point1 = new Point(motionEvent.GetX(pointer1Index), motionEvent.GetY(pointer1Index));
                    point1 = new Point(fromPixels(point1.X), fromPixels(point1.Y));
                    bool isHandled = false;
                    if (tapGestureListeners?.Count > 0 && tapGestureListeners[0].IsTouchHandled || doubleTapGestureListeners?.Count > 0 && doubleTapGestureListeners[0].IsTouchHandled || longPressGestureListeners?.Count > 0 && longPressGestureListeners[0].IsTouchHandled
                || pinchGestureListeners?.Count > 0 && pinchGestureListeners[0].IsTouchHandled || panGestureListeners?.Count > 0 && panGestureListeners[0].IsTouchHandled)
                    {
                        isHandled = true;
                    }

                    var handled = isHandled || motionEvent.Action == MotionEventActions.Pointer2Down || motionEvent.PointerCount > 1;

                    if (nativeView.ParentForAccessibility != null)
                    {
                        nativeView.ParentForAccessibility.RequestDisallowInterceptTouchEvent(handled);
                    }
                }

                if (pointer1Index != -1 && pointer2Index != -1)
                {
                    point2 = new Point(motionEvent.GetX(pointer2Index), motionEvent.GetY(pointer2Index));
                    point2 = new Point(fromPixels(point2.X), fromPixels(point2.Y));
                    pinchAngle = MathUtils.GetAngle(point1.X, point2.X, point1.Y, point2.Y);
                    scaleGestureDetector?.OnTouchEvent(motionEvent);
                }
                
                scrollGestureDetector?.OnTouchEvent(motionEvent);
            }
        }

        internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                View? nativeView = handler.NativeView as View;

                if (nativeView != null)
                {
                    nativeView.Touch -= NativeView_Touch;
                    scaleListener?.Dispose();
                    scaleGestureDetector?.Dispose();

                    scrollListener?.Dispose();
                    scrollGestureDetector?.Dispose();
                }
            }
        }

        private double GetPinchAngle()
        {
            return pinchAngle;
        }

        private class ScaleListener : ScaleGestureDetector.SimpleOnScaleGestureListener
        {
            GestureDetector detector;
            public ScaleListener(GestureDetector gestureDetector)
            {
                detector = gestureDetector;
            }
            public override bool OnScale(ScaleGestureDetector? detector)
            {
                if (detector != null)
                {
                    Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
                    this.detector.OnPinch(GestureStatus.Running, new Point(fromPixels(detector.FocusX), fromPixels(detector.FocusY)), this.detector.GetPinchAngle(), detector.ScaleFactor);
                }

                return true;
            }

            public override bool OnScaleBegin(ScaleGestureDetector? detector)
            {
                if (detector != null)
                {
                    Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
                    this.detector.OnPinch(GestureStatus.Started, new Point(fromPixels(detector.FocusX), fromPixels(detector.FocusY)), this.detector.GetPinchAngle(), detector.ScaleFactor);
                }

                return true;
            }

            public override void OnScaleEnd(ScaleGestureDetector? detector)
            {
                if (detector != null)
                {
                    Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
                    this.detector.OnPinch(GestureStatus.Completed, new Point(fromPixels(detector.FocusX), fromPixels(detector.FocusY)), this.detector.GetPinchAngle(), detector.ScaleFactor);
                }
            }
        }

        private class ScrollListener : AGestureDetector.SimpleOnGestureListener
        {
            GestureDetector detector;

            public ScrollListener(GestureDetector gestureDetector)
            {
                detector = gestureDetector;
            }

            public override bool OnDoubleTap(MotionEvent? e)
            {
                if (e != null)
                {
                    Point point = new Point(e.GetX(e.ActionIndex), e.GetY(e.ActionIndex));
                    Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
                    point = new Point(fromPixels(point.X), fromPixels(point.Y));

                    detector?.OnTapped(point, 2);
                    return true;
                }

                return false;
            }

            public override void OnLongPress(MotionEvent? e)
            {
                if (e != null)
                {
                    Point point = new Point(e.GetX(e.ActionIndex), e.GetY(e.ActionIndex));
                    Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
                    point = new Point(fromPixels(point.X), fromPixels(point.Y));

                    detector?.OnLongPress(point);
                }
                base.OnLongPress(e);
            }

            public override bool OnSingleTapUp(MotionEvent? e)
            {
                if (e != null)
                {
                    Point point = new Point(e.GetX(e.ActionIndex), e.GetY(e.ActionIndex));
                    Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
                    point = new Point(fromPixels(point.X), fromPixels(point.Y));
                    detector?.OnTapped(point, 1);
                    return true;
                }

                return false;
            }

            public override bool OnScroll(MotionEvent? e1, MotionEvent? e2, float distanceX, float distanceY)
            {
                if (e1 != null && e2 != null && e2.PointerCount == 1)
                {
                    Point point = new Point(e1.GetX(e1.ActionIndex), e1.GetY(e1.ActionIndex));
                    Func<double, double> fromPixels = Android.App.Application.Context.FromPixels;
                    point = new Point(fromPixels(point.X), fromPixels(point.Y));
                    detector?.OnScroll(point, new Point(fromPixels(distanceX), fromPixels(distanceY)));
                }

                return true;
            }
        }
    }
}
