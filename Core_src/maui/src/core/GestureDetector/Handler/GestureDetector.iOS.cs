using Microsoft.Maui.Graphics;
using MauiView = Microsoft.Maui.Controls.View;
using GestureStatus = Microsoft.Maui.GestureStatus;
using UIKit;
using System;
using CoreGraphics;
using Microsoft.Maui;
using Foundation;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class GestureDetector
    {
        private UIPinchGestureExt? pinchGesture;
        private UIPanGestureExt? panGesture;
        private UITapGestureExt? tapGesture;
        private UITapGestureExt? doubleTapGesture;
        private UILongPressGestureExt? longPressGesture;

        internal void SubscribeNativeGestureEvents(MauiView? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                UIView? nativeView = handler?.NativeView as UIView;
                if (nativeView != null)
                {
                    // TODO : If dynamically add the gesture listeners, it won't work. Because below native gesture listeners created based on listener collection count, but dynamic case, this method executed before listener added to the collection. Need to do it proper and optimized way. 

                    if (pinchGestureListeners?.Count > 0)
                    {
                        pinchGesture = new UIPinchGestureExt(this);
                        nativeView.AddGestureRecognizer(pinchGesture);
                    }

                    if (panGestureListeners?.Count > 0)
                    {
                        panGesture = new UIPanGestureExt(this);
                        nativeView.AddGestureRecognizer(panGesture);
                    }

                    if (doubleTapGestureListeners?.Count > 0)
                    {
                        doubleTapGesture = new UITapGestureExt(this) { NumberOfTapsRequired = 2 };
                        nativeView.AddGestureRecognizer(doubleTapGesture);
                    }

                    if (tapGestureListeners?.Count > 0)
                    {
                        tapGesture = new UITapGestureExt(this) { NumberOfTapsRequired = 1 };
                        nativeView.AddGestureRecognizer(tapGesture);
                    }

                    if (longPressGestureListeners?.Count > 0)
                    {
                        longPressGesture = new UILongPressGestureExt(this);
                        nativeView.AddGestureRecognizer(longPressGesture);
                    }
                }
            }
        }

        internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                UIView? nativeView = handler.NativeView as UIView;

                if (nativeView != null)
                {
                    var gestures = nativeView.GestureRecognizers;

                    if (gestures != null)
                    {
                        if (pinchGesture != null)
                        {
                            nativeView.RemoveGestureRecognizer(pinchGesture);
                        }
                        if (panGesture != null)
                        {
                            nativeView.RemoveGestureRecognizer(panGesture);
                        }
                        if (tapGesture != null)
                        {
                            nativeView.RemoveGestureRecognizer(tapGesture);
                        }
                        if (doubleTapGesture != null)
                        {
                            nativeView.RemoveGestureRecognizer(doubleTapGesture);
                        }
                        if (longPressGesture != null)
                        {
                            nativeView.RemoveGestureRecognizer(longPressGesture);
                        }
                    }
                }
            }
        }

        internal class UIPanGestureExt : UIPanGestureRecognizer
        {
            IGestureListener? gestureListener;
            public UIPanGestureExt(GestureDetector gestureDetector)
            {
                if (gestureDetector.MauiView is IGestureListener _gestureListener)
                    gestureListener = _gestureListener;

                this.AddTarget(() => OnScroll(gestureDetector));

                ShouldRecognizeSimultaneously += GestureRecognizer;
            }

            bool GestureRecognizer(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
            {
                if (otherGestureRecognizer is UITouchRecognizerExt || otherGestureRecognizer is UIScrollRecognizerExt || gestureListener == null)
                {
                    return true;
                }

                return !gestureListener.IsTouchHandled;
            }

            private void OnScroll(GestureDetector gestureDetector)
            {
                if (!gestureDetector.IsEnabled || gestureDetector.InputTransparent)
                {
                    return;
                }

                var locationInView = LocationInView(View);
                var translateLocation = TranslationInView(View);
                var state = GestureStatus.Completed;

                switch (State)
                {
                    case UIGestureRecognizerState.Began:
                        state = GestureStatus.Started;
                        break;
                    case UIGestureRecognizerState.Changed:
                        state = GestureStatus.Running;
                        break;
                    case UIGestureRecognizerState.Cancelled:
                    case UIGestureRecognizerState.Failed:
                        state = GestureStatus.Canceled;
                        break;
                    case UIGestureRecognizerState.Ended:
                        state = GestureStatus.Completed;
                        break;
                }

                gestureDetector.OnScroll(state, new Point(locationInView.X, locationInView.Y), new Point(translateLocation.X, translateLocation.Y));
                SetTranslation(CGPoint.Empty, View);
            }
        }

        private class UIPinchGestureExt : UIPinchGestureRecognizer
        {
            IGestureListener? gestureListener;
            public UIPinchGestureExt(GestureDetector gestureDetector)
            {
                if (gestureDetector.MauiView is IGestureListener _gestureListener)
                    gestureListener = _gestureListener;

                this.AddTarget(() => OnPinch(gestureDetector));

                ShouldRecognizeSimultaneously = (g, o) => gestureListener == null || !gestureListener.IsTouchHandled;
            }

            private void OnPinch(GestureDetector gestureDetector)
            {
                if (!gestureDetector.IsEnabled || gestureDetector.InputTransparent)
                {
                    return;
                }

                var locationInView = LocationInView(View);
                var state = GestureStatus.Completed;
                double angle = double.NaN;
                if (NumberOfTouches == 2)
                {
                    CGPoint touchStart = LocationOfTouch(0, View);
                    CGPoint touchEnd = LocationOfTouch(1, View);
                    angle = MathUtils.GetAngle((float)touchStart.X, (float)touchEnd.X, (float)touchStart.Y, (float)touchEnd.Y);
                }

                switch (State)
                {
                    case UIGestureRecognizerState.Began:
                        state = GestureStatus.Started;
                        break;
                    case UIGestureRecognizerState.Changed:
                        state = GestureStatus.Running;
                        break;
                    case UIGestureRecognizerState.Cancelled:
                    case UIGestureRecognizerState.Failed:
                        state = GestureStatus.Canceled;
                        break;
                    case UIGestureRecognizerState.Ended:
                        state = GestureStatus.Completed;
                        break;
                }

                gestureDetector.OnPinch(state, new Point(locationInView.X, locationInView.Y), angle, (float)Scale);
                Scale = 1; // Resetting the previous scale value.
            }
        }

        private class UITapGestureExt : UITapGestureRecognizer
        {
            IGestureListener? gestureListener;
            public UITapGestureExt(GestureDetector gestureDetector)
            {
                if (gestureDetector.MauiView is IGestureListener _gestureListener)
                    gestureListener = _gestureListener;

                this.AddTarget(() => OnTap(gestureDetector));

                ShouldRecognizeSimultaneously = (g, o) => gestureListener == null || !gestureListener.IsTouchHandled;
            }

            private void OnTap(GestureDetector gestureDetector)
            {
                if (!gestureDetector.IsEnabled || gestureDetector.InputTransparent)
                {
                    return;
                }

               var locationInView = LocationInView(View);
               gestureDetector.OnTapped(new Point(locationInView.X, locationInView.Y), (int)NumberOfTapsRequired);
            }
        }

        private class UILongPressGestureExt : UILongPressGestureRecognizer
        {
            IGestureListener? gestureListener;
            public UILongPressGestureExt(GestureDetector gestureDetector)
            {
                if (gestureDetector.MauiView is IGestureListener _gestureListener)
                    gestureListener = _gestureListener;

                this.AddTarget(() => OnLongPress(gestureDetector));

                ShouldRecognizeSimultaneously = (g, o) => gestureListener == null || !gestureListener.IsTouchHandled;
            }

            private void OnLongPress(GestureDetector gestureDetector)
            {
                if (!gestureDetector.IsEnabled || gestureDetector.InputTransparent)
                {
                    return;
                }

                var locationInView = LocationInView(View);
                gestureDetector.OnLongPress(new Point(locationInView.X, locationInView.Y));
            }
        }
    }
}

