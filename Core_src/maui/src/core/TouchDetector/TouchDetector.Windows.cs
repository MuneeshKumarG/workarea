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
                UIElement? nativeView = handler?.NativeView as UIElement;
                if (nativeView != null)
                {
                    nativeView.PointerPressed += NativeView_PointerPressed;
                    nativeView.PointerMoved += NativeView_PointerMoved;
                    nativeView.PointerReleased += NativeView_PointerReleased;
                    nativeView.PointerCanceled += NativeView_PointerCanceled;
                    nativeView.PointerWheelChanged += NativeView_PointerWheelChanged;
                    nativeView.PointerEntered += NativeView_PointerEntered;
                    nativeView.PointerExited += NativeView_PointerExited;
                }
            }
        }

        private void NativeView_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                var pointerPoint = e.GetCurrentPoint(nativeView);
                var property = pointerPoint.Properties;
                TouchEventArgs eventArgs = new TouchEventArgs(pointerPoint.PointerId, TouchActions.Exited, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y))
                {
                    IsLeftButtonPressed = property.IsLeftButtonPressed,
                    IsRightButtonPressed = property.IsRightButtonPressed,
                };

                OnTouchAction(eventArgs);
            }
        }

        private void NativeView_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                var pointerPoint = e.GetCurrentPoint(nativeView);
                var property = pointerPoint.Properties;
                TouchEventArgs eventArgs = new TouchEventArgs(pointerPoint.PointerId, TouchActions.Entered, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y))
                {
                    IsLeftButtonPressed = property.IsLeftButtonPressed,
                    IsRightButtonPressed = property.IsRightButtonPressed,
                };

                OnTouchAction(eventArgs);
            }
        }

        private void NativeView_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                var pointerPoint = e.GetCurrentPoint(nativeView);
                OnScrollAction(pointerPoint.PointerId, new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y), pointerPoint.Properties.MouseWheelDelta);
            }
        }

        private void NativeView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                nativeView.CapturePointer(e.Pointer);
                var pointerPoint = e.GetCurrentPoint(nativeView);
                var property = pointerPoint.Properties;

                TouchEventArgs eventArgs = new TouchEventArgs(pointerPoint.PointerId, TouchActions.Pressed, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y))
                {
                    IsLeftButtonPressed = property.IsLeftButtonPressed,
                    IsRightButtonPressed = property.IsRightButtonPressed,
                };

                OnTouchAction(eventArgs);

                if (touchListeners[0].IsTouchHandled)
                    nativeView.ManipulationMode = ManipulationModes.None;
            }
        }

        private void NativeView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                var pointerPoint = e.GetCurrentPoint(nativeView);
                OnTouchAction(pointerPoint.PointerId, TouchActions.Moved, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));
            }
        }

        private void NativeView_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                nativeView.ReleasePointerCapture(e.Pointer);
                var pointerPoint = e.GetCurrentPoint(nativeView);
                OnTouchAction(pointerPoint.PointerId, TouchActions.Cancelled, GetDeviceType(pointerPoint.PointerDeviceType), new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));

                if (nativeView.ManipulationMode == ManipulationModes.None)
                    nativeView.ManipulationMode = ManipulationModes.System;
            }
        }

        private void NativeView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                nativeView.ReleasePointerCapture(e.Pointer);
                var pointerPoint = e.GetCurrentPoint(nativeView);
                OnTouchAction(pointerPoint.PointerId, TouchActions.Released, GetDeviceType(pointerPoint.PointerDeviceType),  new Microsoft.Maui.Graphics.Point(pointerPoint.Position.X, pointerPoint.Position.Y));

                if (nativeView.ManipulationMode == ManipulationModes.None)
                    nativeView.ManipulationMode = ManipulationModes.System;
            }
        }

        private static PointerDeviceType GetDeviceType(Microsoft.UI.Input.PointerDeviceType deviceType)
        {
            return deviceType == Microsoft.UI.Input.PointerDeviceType.Mouse ? PointerDeviceType.Mouse : PointerDeviceType.Touch;
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
                    nativeView.PointerWheelChanged -= NativeView_PointerWheelChanged;
                    nativeView.PointerEntered -= NativeView_PointerEntered;
                    nativeView.PointerExited -= NativeView_PointerExited;
                }
            }
        }
    }
}
