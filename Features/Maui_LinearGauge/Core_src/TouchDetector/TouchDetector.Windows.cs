using System;
using MauiView = Microsoft.Maui.Controls.View;
using Microsoft.UI.Xaml;
using Microsoft.Maui;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class TouchDetector
    {
        internal void SubscribeNativeTouchEvents(MauiView? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                UIElement? nativeView = handler.NativeView as UIElement;
                if (nativeView != null)
                {
                    nativeView.PointerPressed += NativeView_PointerPressed;
                    nativeView.PointerMoved += NativeView_PointerMoved;
                    nativeView.PointerReleased += NativeView_PointerReleased;
                    nativeView.PointerCanceled += NativeView_PointerCanceled;
                }
            }
        }

        private void NativeView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                nativeView.CapturePointer(e.Pointer);
                var pointerPoint = e.GetCurrentPoint(nativeView);
                OnTouchAction(pointerPoint.PointerId, TouchActions.Pressed, new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));

                if (touchListeners[0].IsTouchHandled)
                    nativeView.ManipulationMode = ManipulationModes.None;
            }
        }

        private void NativeView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                var pointerPoint = e.GetCurrentPoint(nativeView);
                OnTouchAction(pointerPoint.PointerId, TouchActions.Moved, new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));
            }
        }

        private void NativeView_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                nativeView.ReleasePointerCapture(e.Pointer);
                var pointerPoint = e.GetCurrentPoint(nativeView);
                OnTouchAction(pointerPoint.PointerId, TouchActions.Cancelled, new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));

                if (nativeView.ManipulationMode == ManipulationModes.None)
                    nativeView.ManipulationMode = ManipulationModes.System;
            }
        }

        private void NativeView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                nativeView.ReleasePointerCapture(e.Pointer);
                var pointerPoint = e.GetCurrentPoint(nativeView);
                OnTouchAction(pointerPoint.PointerId, TouchActions.Released, new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));

                if (nativeView.ManipulationMode == ManipulationModes.None)
                    nativeView.ManipulationMode = ManipulationModes.System;
            }
        }

        internal void UnsubscribeNativeTouchEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                UIElement? nativeView = handler.NativeView as UIElement;
                if (nativeView != null)
                {
                    nativeView.PointerPressed -= NativeView_PointerPressed;
                    nativeView.PointerMoved -= NativeView_PointerMoved;
                    nativeView.PointerReleased -= NativeView_PointerReleased;
                    nativeView.PointerCanceled -= NativeView_PointerCanceled;
                }
            }
        }
    }
}
