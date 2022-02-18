using MauiView = Microsoft.Maui.Controls.View;
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
            if (mauiView != null && mauiView.Handler != null)
            {
                var handler = mauiView.Handler;
                UIView? nativeView = handler?.NativeView as UIView;

                if (nativeView != null)
                {
                    UITouchRecognizerExt touchRecognizer = new UITouchRecognizerExt(this);
                    nativeView.AddGestureRecognizer(touchRecognizer);

                    UIHoverRecognizerExt hoverGesture = new UIHoverRecognizerExt(this);
                    nativeView.AddGestureRecognizer(hoverGesture);

                    UIScrollRecognizerExt scrollRecognizer = new UIScrollRecognizerExt(this);
                    nativeView.AddGestureRecognizer(scrollRecognizer);
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
                            if (item is UITouchRecognizerExt || item is UIHoverRecognizerExt || item is UIScrollRecognizerExt)
                            {
                                nativeView.RemoveGestureRecognizer(item);
                            }
                        }
                    }
                }
            }
        }
    }

    internal class UIHoverRecognizerExt : UIHoverGestureRecognizer
    {
        TouchDetector touchDetector;
        ITouchListener? touchListener;

        public UIHoverRecognizerExt(TouchDetector listener) : base(Hovering)
        {
            touchDetector = listener;
            if (touchDetector.MauiView is ITouchListener _touchListener)
                touchListener = _touchListener;
            ShouldRecognizeSimultaneously += GestureRecognizer;

            this.AddTarget(() => OnHover(touchDetector));
        }

        /// <summary>
        /// Having static member for base action hence <see cref="UIHoverGestureRecognizer"/> does not have default consturctor.
        /// </summary>
        private static void Hovering()
        {

        }

        bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is UITouchRecognizerExt || touchListener == null)
            {
                return true;
            }

            return !touchListener.IsTouchHandled;
        }

        private void OnHover(TouchDetector gestureDetecture)
        {
            if (!touchDetector.IsEnabled || touchDetector.InputTransparent)
            {
                return;
            }

            var state = State == UIGestureRecognizerState.Began ? TouchActions.Entered :
                State == UIGestureRecognizerState.Changed ? TouchActions.Moved :
                State == UIGestureRecognizerState.Ended ? TouchActions.Exited : TouchActions.Cancelled;

            long pointerId = Handle.Handle.ToInt64();
            CGPoint point = LocationInView(View);

            gestureDetecture.OnTouchAction(pointerId, state, new Point(point.X, point.Y));
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
            if (otherGestureRecognizer is GestureDetector.UIPanGestureExt || otherGestureRecognizer is UIScrollRecognizerExt || touchListener == null)
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
                long pointerId = touch.Handle.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);

                touchDetector.OnTouchAction(
                    new TouchEventArgs(pointerId, TouchActions.Pressed, new Point(point.X, point.Y))
                    {
                        IsLeftButtonPressed = touch.TapCount == 1
                    });
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
                long pointerId = touch.Handle.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                touchDetector.OnTouchAction(
                   new TouchEventArgs(pointerId, TouchActions.Moved, new Point(point.X, point.Y))
                   {
                       IsLeftButtonPressed = touch.TapCount == 1
                   });
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
                long pointerId = touch.Handle.Handle.ToInt64();
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
                long pointerId = touch.Handle.Handle.ToInt64();
                CGPoint point = touch.LocationInView(View);
                touchDetector.OnTouchAction(pointerId, TouchActions.Cancelled, new Point(point.X, point.Y));
            }
        }
    }

    internal class UIScrollRecognizerExt : UIPanGestureRecognizer
    {
        TouchDetector touchDetector;
        ITouchListener? touchListener;

        internal UIScrollRecognizerExt(TouchDetector listener)
        {
            touchDetector = listener;
            if (touchDetector.MauiView is ITouchListener _touchListener)
                touchListener = _touchListener;

            this.AddTarget(() => OnScroll(this));

            AllowedScrollTypesMask = UIScrollTypeMask.All;
            ShouldRecognizeSimultaneously += GestureRecognizer;
            ShouldReceiveTouch += GesturerTouchRecognizer;
        }

        bool GesturerTouchRecognizer(UIGestureRecognizer recognizer, UITouch touch)
        {
            return false;
        }

        private void OnScroll(UIScrollRecognizerExt touchRecognizerExt)
        {
            if (!touchDetector.IsEnabled || touchDetector.InputTransparent)
            {
                return;
            }

            long pointerId = touchRecognizerExt.Handle.Handle.ToInt64();
            CGPoint delta = touchRecognizerExt.TranslationInView(View);
            CGPoint point = touchRecognizerExt.LocationInView(View);

            touchDetector.OnScrollAction(pointerId, new Point(point.X, point.Y), delta.Y != 0 ? delta.Y : delta.X);
        }

        bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            if (otherGestureRecognizer is GestureDetector.UIPanGestureExt || otherGestureRecognizer is UITouchRecognizerExt || touchListener == null)
            {
                return true;
            }

            return !touchListener.IsTouchHandled;
        }
    }
}
