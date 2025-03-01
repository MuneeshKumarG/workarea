﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents the pointer that is used to indicate the value with any visual content.
    /// </summary>
    public class LinearContentPointer : LinearMarkerPointer
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Content"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Content"/> bindable property.
        /// </value>
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(LinearContentPointer), null, propertyChanged: OnContentPropertyChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the content of a <see cref="LinearContentPointer"/>. 
        /// </summary>
        /// <value>
        /// An object that contains the pointer's visual content. The default value is <c>null</c>.
        /// </value>
        public View Content
        {
            get { return (View)this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        #endregion

        #region Override methods

        internal override void CreatePointer()
        {
            this.CreateShapePointer();
        }

        internal override void Draw(ICanvas canvas) { }

        internal override void UpdatePointer()
        {
            if (this.Scale != null && !this.Scale.ScaleAvailableSize.IsZero && this.Content != null)
            {
                double actualValue;

                if (this.EnableAnimation && this.AnimationValue != null)
                {
                    actualValue = (double)this.AnimationValue;
                }
                else
                {
                    actualValue = Math.Clamp(this.Value, this.Scale.ActualMinimum, this.Scale.ActualMaximum);
                }

                double valuePosition = this.Scale.GetPositionFromValue(actualValue);
                this.Content.Measure(this.Scale.ScaleAvailableSize.Width, this.Scale.ScaleAvailableSize.Height);
                double halfWidth = this.Content.DesiredSize.Width / 2;
                double halfHeight = this.Content.DesiredSize.Height / 2;
                if (this.Scale.Orientation == GaugeOrientation.Vertical)
                {
                    Utility.Swap(ref halfWidth, ref halfHeight);
                }

                double actualAxisLineThickness = this.Scale.GetActualScaleLineThickness();
                double x = this.Scale.ScalePosition.X + valuePosition - halfWidth;
                double y = this.Scale.ScalePosition.Y + (actualAxisLineThickness / 2) - halfHeight;
                this.GetPointerPosition(halfWidth, halfHeight + (actualAxisLineThickness / 2), ref x, ref y);
                if (this.Scale.Orientation == GaugeOrientation.Vertical)
                {
                    Utility.Swap(ref x, ref y);
                }

                Rectangle rectangle = new Rectangle(new Point(x, y), Content.DesiredSize);
                AbsoluteLayout.SetLayoutBounds(Content, rectangle);

                //Calculate dragging rectangle. 
                this.PointerRect = new Rectangle(x - DragOffset, y - DragOffset,
                Content.DesiredSize.Width + (DragOffset * 2), Content.DesiredSize.Height + (DragOffset * 2));
            }
        }

        /// <summary>
        /// Invoked whenever the binding context of the View changes.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.Content != null)
                SetInheritedBindingContext(Content, this);
        }
        #endregion

        #region Property changed

        /// <summary>
        /// Called when shape pointer <see cref="Content"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LinearContentPointer pointer)
            {
                if (pointer.Scale != null && pointer.Scale.MarkerPointersLayout.Contains(pointer.PointerView))
                {
                    pointer.Scale.MarkerPointerChildUpdate(oldValue, newValue);

                    if (newValue is View newChild)
                    {
                        SetInheritedBindingContext(newChild, pointer);
                    }
                    pointer.Scale.ScaleInvalidateMeasureOverride();
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// To create bar pointer.
        /// </summary>
        private void CreateShapePointer()
        {
            if (this.Scale == null || this.Scale.ScaleAvailableSize.IsZero)
            {
                return;
            }

            if (this.CanAnimate)
            {
                this.AnimatePointer(this.Scale.ActualMinimum, this.Value);
            }
            else
            {
                this.UpdatePointer();
            }
            this.CanAnimate = false;
        }

        #endregion
    }
}
