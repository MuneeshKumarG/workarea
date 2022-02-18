using System;
using MauiView = Microsoft.Maui.Controls.View;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Foundation;
using CoreGraphics;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class TouchDetector
    {
        internal void SubscribeNativeTouchEvents(MauiView? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                UIView? nativeView = handler.NativeView as UIView;

                if (nativeView != null)
                {
                    UITouchRecognizerExt touchRecognizer = new UITouchRecognizerExt(this);
                    nativeView.AddGestureRecognizer(touchRecognizer);
                }
            }
        }

        internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                UIView? nativeView = handler.NativeView as UIView;

                if (nativeView != null)
                {
                    var gestures = nativeView.GestureRecognizers;

                    if (gestures != null)
                    {
                        foreach (var item in gestures)
                        {
                            if (item is UITouchRecognizerExt)
                            {
                                nativeView.RemoveGestureRecognizer(item);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    internal class UITouchRecognizerExt : UIPanGestureRecognizer
    {
        TouchDetector touchDetector;
        ITouchListener? touchListener;

        internal UITouchRecognizerExt(TouchDetector listener)
        {
            touchDetector = listener;
            if (touchDetector.MauiView is ITouchListener _touchListener)
                touchListener = _touchListener;

            ShouldRecognizeSimultaneously += GestureRecognizer;
        }

        bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is GestureDetector.UIPanGestureExt || touchListener == null)
            {
                return true;
            }

            return !touchListener.IsTouchHandled;
        }

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if (!touchDetector.IsEnabled || touchDetector.InputTransparent)
            {
                return;
            }

            UITouch? touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                long pointerId = touch.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                touchDetector.OnTouchAction(pointerId, TouchActions.Pressed, new Point(point.X, point.Y));
            }
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);
            if (!touchDetector.IsEnabled || touchDetector.InputTransparent)
            {
                return;
            }

            UITouch? touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                long pointerId = touch.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                touchDetector.OnTouchAction(pointerId, TouchActions.Moved, new Point(point.X, point.Y));
            }
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);
            if (!touchDetector.IsEnabled || touchDetector.InputTransparent)
            {
                return;
            }

            UITouch? touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                long pointerId = touch.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                touchDetector.OnTouchAction(pointerId, TouchActions.Released, new Point(point.X, point.Y));
            }
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);
            if (!touchDetector.IsEnabled || touchDetector.InputTransparent)
            {
                return;
            }

            UITouch? touch = touches.AnyObject as UITouch;

            if (touch != null)
            {
                long pointerId = touch.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                touchDetector.OnTouchAction(pointerId, TouchActions.Cancelled, new Point(point.X, point.Y));
            }
        }
    }
}
