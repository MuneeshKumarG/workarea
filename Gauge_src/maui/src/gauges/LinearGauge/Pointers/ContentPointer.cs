using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents the pointer that is used to indicate the value with any visual content.
    /// </summary>
    public class ContentPointer : LinearMarkerPointer
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Content"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Content"/> bindable property.
        /// </value>
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(ContentPointer), null, propertyChanged: OnContentPropertyChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the content of a <see cref="ContentPointer"/>. 
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
                this.GetPointerPosition(halfWidth, halfHeight, ref x, ref y);
                if (this.Scale.Orientation == GaugeOrientation.Vertical)
                {
                    Utility.Swap(ref x, ref y);
                }

                AbsoluteLayout.SetLayoutBounds(Content, new Rectangle(new Point(x, y), Content.DesiredSize));
            }
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
            if (bindable is ContentPointer pointer)
            {
                if (newValue is View newChild)
                {
                    newChild.BindingContext = pointer;
                }
                pointer.Scale?.ShapePointerChildUpdate(oldValue, newValue);
                pointer.UpdatePointer();
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
                this.CanAnimate = false;
            }
            else
            {
                this.UpdatePointer();
            }
        }

        #endregion
    }
}
