using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;

namespace Syncfusion.Maui.Core.Internals
{
    public partial class GestureDetector
    {
        bool wasPinchStarted;
        bool isPinching;
        Windows.Foundation.Point touchMovePoint;
        readonly List<uint> fingers = new List<uint>();

        internal void SubscribeNativeGestureEvents(View? mauiView)
        {
            if (mauiView != null)
            {
                var handler = mauiView.Handler;
                UIElement? nativeView = handler.NativeView as UIElement;
                if (nativeView != null)
                {
                    if (tapGestureListeners != null && tapGestureListeners.Count > 0)
                    {
                        nativeView.Tapped += NativeView_Tapped;
                    }

                    if (doubleTapGestureListeners != null && doubleTapGestureListeners.Count > 0)
                    {
                        nativeView.DoubleTapped += NativeView_DoubleTapped;
                    }

                    if (longPressGestureListeners != null && longPressGestureListeners.Count > 0)
                    {
                        nativeView.Holding += NativeView_Holding;
                    }

                    if ((panGestureListeners != null && panGestureListeners.Count > 0) || (pinchGestureListeners != null && pinchGestureListeners.Count > 0))
                    {
                        nativeView.ManipulationStarted += NativeView_ManipulationStarted;
                        nativeView.ManipulationDelta += NativeView_ManipulationDelta;
                        nativeView.ManipulationCompleted += NativeView_ManipulationCompleted;
                        nativeView.PointerPressed += NativeView_PointerPressed;
                        nativeView.PointerMoved += NativeView_PointerMoved;
                        nativeView.PointerReleased += NativeView_PointerReleased;
                        nativeView.PointerCanceled += NativeView_PointerCanceled;
                        nativeView.PointerExited += NativeView_PointerExited;
                    }
                }
            }
        }

        private void NativeView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            //TODO : need to check and refix the issue with e.Position value of ManipulationRoutedDelatEventArgs instead using below position.
            touchMovePoint = e.GetCurrentPoint(sender as UIElement).Position;
        }

        private void NativeView_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            uint id = e.Pointer.PointerId;
            if (fingers.Contains(id))
                fingers.Remove(id);
            PinchCompleted(true);
        }

        private void NativeView_PointerCanceled(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            uint id = e.Pointer.PointerId;
            if (fingers.Contains(id))
                fingers.Remove(id);

            PinchCompleted(false);
        }

        private void NativeView_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            uint id = e.Pointer.PointerId;
            if (fingers.Contains(id))
                fingers.Remove(id);

            PinchCompleted(true);
        }

        private void NativeView_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            uint id = e.Pointer.PointerId;
            if (!fingers.Contains(id))
                fingers.Add(id);

            //TODO: Need to revisit this fix for the issue scrolling is not working while panning on the screen in the control. 
            var nativeView = sender as UIElement;
            if (nativeView != null)
            {
                if ((panGestureListeners != null && panGestureListeners.Count > 0 && panGestureListeners[0].IsTouchHandled) ||
                    (pinchGestureListeners != null && pinchGestureListeners.Count > 0 && pinchGestureListeners[0].IsTouchHandled))
                {
                    nativeView.ManipulationMode = ManipulationModes.Scale
                                      | ManipulationModes.TranslateX
                                      | ManipulationModes.TranslateY;
                }
            }
        }

        private void NativeView_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            var scalePoint = new Point(e.Position.X, e.Position.Y);
            var translationPoint = new Point(e.Cumulative.Translation.X, e.Cumulative.Translation.Y);
            double angle = MathUtils.GetAngle(scalePoint.X, translationPoint.X, scalePoint.Y, translationPoint.Y);
            PinchCompleted(true,scalePoint, angle);
        }

        private void NativeView_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            bool isTouchHandled = false;

            OnPinch(e);
            OnPan(e);

            //Check touch handled in pan gesture or not.
            if (panGestureListeners?.Count >= 0 && panGestureListeners[0].IsTouchHandled)
            {
                isTouchHandled = true;
            }

            //Check touch handled in pinch gesture or not.
            if (pinchGestureListeners?.Count >= 0 && pinchGestureListeners[0].IsTouchHandled)
            {
                isTouchHandled = true;
            }

            if (sender is UIElement nativeView && !isTouchHandled)
            {
                nativeView.ManipulationMode = ManipulationModes.System;
            }
        }

        private void NativeView_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            wasPinchStarted = false;
        }

        void OnPan(ManipulationDeltaRoutedEventArgs e)
        {
            if (panGestureListeners == null || panGestureListeners.Count == 0 || fingers.Count != 1)
                return;

            var touchPoint = new Point(touchMovePoint.X, touchMovePoint.Y);
            var translationPoint = new Point(e.Delta.Translation.X, e.Delta.Translation.Y);
           
            foreach (var listener in panGestureListeners)
            {
                PanEventArgs eventArgs = new PanEventArgs(touchPoint, translationPoint);
                listener.OnPan(eventArgs);
            }
        }

        void OnPinch(ManipulationDeltaRoutedEventArgs e)
        {
            if (pinchGestureListeners == null || pinchGestureListeners.Count == 0 || fingers.Count != 2)
                return;

            var scalePoint = new Point(e.Position.X, e.Position.Y);
            var translationPoint = new Point(e.Cumulative.Translation.X, e.Cumulative.Translation.Y);
            GestureStatus state = GestureStatus.Started;
            double angle = MathUtils.GetAngle(scalePoint.X, translationPoint.X, scalePoint.Y, translationPoint.Y);
            float scale = e.Delta.Scale;
            isPinching = true;

            foreach (var listener in pinchGestureListeners)
            {
                if (wasPinchStarted)
                {
                    state = GestureStatus.Running;
                }
                else
                {
                    scale = 1;
                    angle = double.NaN;
                }

                PinchEventArgs eventArgs = new PinchEventArgs(state, scalePoint, angle, scale);
                listener.OnPinch(eventArgs);
            }

            wasPinchStarted = true;
        }

        void PinchCompleted(bool isCompleted, Point scalePoint = new Point(), double angle = double.NaN, float scale = 1)
        {
            if (pinchGestureListeners == null || pinchGestureListeners.Count == 0 || !isPinching) return;

            foreach (var listener in pinchGestureListeners)
            {
                PinchEventArgs eventArgs = new PinchEventArgs(isCompleted ? GestureStatus.Completed : GestureStatus.Canceled, scalePoint, angle, scale);
                listener.OnPinch(eventArgs);
            }
            isPinching = false;
        }

        private void NativeView_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            if (longPressGestureListeners == null || longPressGestureListeners.Count == 0) return;

            var touchPoint = e.GetPosition(sender as UIElement);

            foreach (var listener in longPressGestureListeners)
            {
                if (e.HoldingState == Microsoft.UI.Input.HoldingState.Started)
                {
                    listener.OnLongPress(new LongPressEventArgs(new Point(touchPoint.X, touchPoint.Y)));
                }
            }

            if (longPressGestureListeners[0].IsTouchHandled)
            {
                e.Handled = true;
            }
        }

        private void NativeView_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            if (doubleTapGestureListeners == null || doubleTapGestureListeners.Count == 0) return;

            var touchPoint = e.GetPosition(sender as UIElement);

            foreach (var listener in doubleTapGestureListeners)
            {
                listener.OnDoubleTap(new TapEventArgs(new Point(touchPoint.X, touchPoint.Y), 2));
            }

            if (doubleTapGestureListeners[0].IsTouchHandled)
            {
                e.Handled = true;
            }
        }

        private void NativeView_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!IsEnabled || InputTransparent)
            {
                return;
            }

            if (tapGestureListeners == null || tapGestureListeners.Count == 0) return;

            var touchPoint = e.GetPosition(sender as UIElement);

            foreach (var listener in tapGestureListeners)
            {
                listener.OnTap(new TapEventArgs(new Point(touchPoint.X, touchPoint.Y), 1));
            }

            if (tapGestureListeners[0].IsTouchHandled)
            {
                e.Handled = true;
            }
        }

        internal void UnsubscribeNativeGestureEvents(IElementHandler handler)
        {
            if (handler != null)
            {
                UIElement? nativeView = handler.NativeView as UIElement;

                if (nativeView != null)
                {
                    nativeView.ManipulationMode = ManipulationModes.None;

                    nativeView.Tapped -= NativeView_Tapped;
                    nativeView.DoubleTapped -= NativeView_DoubleTapped;
                    nativeView.Holding -= NativeView_Holding;
                    nativeView.ManipulationStarted -= NativeView_ManipulationStarted;
                    nativeView.ManipulationDelta -= NativeView_ManipulationDelta;
                    nativeView.ManipulationCompleted -= NativeView_ManipulationCompleted;
                    nativeView.PointerPressed -= NativeView_PointerPressed;
                    nativeView.PointerMoved -= NativeView_PointerMoved;
                    nativeView.PointerReleased -= NativeView_PointerReleased;
                    nativeView.PointerCanceled -= NativeView_PointerCanceled;
                    nativeView.PointerExited -= NativeView_PointerExited;
                }
            }
        }
    }
}
