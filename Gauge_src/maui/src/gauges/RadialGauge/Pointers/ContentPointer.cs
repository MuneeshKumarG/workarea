using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents the pointer that is used to indicate the value with any visual content.
    /// </summary>
    public class ContentPointer : MarkerPointer
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

        internal override void Draw(ICanvas canvas) { }

        internal override void CreatePointer()
        {
            this.CreateContentPointer();
        }

        internal override void UpdatePointer()
        {
            if (this.RadialAxis != null && !this.RadialAxis.AvailableSize.IsZero && this.Content != null)
            {
                double actualValue;
                if ((this.EnableAnimation || this.RadialAxis.CanAnimate) && this.AnimationValue != null)
                {
                    actualValue = (double)this.AnimationValue;
                }
                else
                {
                    actualValue = Math.Clamp(this.Value, this.RadialAxis.ActualMinimum, this.RadialAxis.ActualMaximum);
                }

                double actualMarkerOffset = 0;

                if (!double.IsNaN(this.Offset))
                {
                    actualMarkerOffset = this.RadialAxis.CalculateActualSize(this.Offset, this.OffsetUnit, true);
                }

                double radius = ((double.IsNaN(this.Offset) ? this.RadialAxis.Radius : actualMarkerOffset) * this.RadialAxis.RadiusFactor) -
                    (this.RadialAxis.AxisLineRadiusDifference + (this.RadialAxis.ActualAxisLineWidth / 2));
                radius = radius < 0 ? 0 : radius;
                double factor = this.RadialAxis.ValueToFactor(actualValue);
                Point vector = this.RadialAxis.FactorToPoint(factor);

                double markerHeight, markerWidth;

                Size size = this.Content.ComputeDesiredSize(this.RadialAxis.AvailableSize.Width, this.RadialAxis.AvailableSize.Height);
                markerHeight = size.Height;
                markerWidth = size.Width;

                PointF markerPosition = new PointF((float)(this.RadialAxis.Center.X + (radius * vector.X) - (markerWidth / 2)),
                        (float)(this.RadialAxis.Center.Y + (radius * vector.Y) - (markerHeight / 2)));

                //Calculate dragging rectangle. 
                this.PointerRect = new RectangleF(markerPosition.X - DraggingOffset, markerPosition.Y - DraggingOffset,
                (float)markerWidth + (DraggingOffset * 2), (float)markerHeight + (DraggingOffset * 2));

                if (this.Content != null)
                {
                    if (this.RadialAxis.CanAnimate && this.AnimationValue != null && this.Content.Opacity == 0)
                    {
                        this.Content.Opacity = 1;
                    }

                    AbsoluteLayout.SetLayoutBounds(this.Content, new Rectangle(markerPosition, new Size(markerWidth, markerHeight)));
                }
            }
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Method used to add <see cref="Content"/> in radial gauge pointers layout.
        /// </summary>
        /// <param name="view">Represents the view to add.</param>
        internal void AddCustomView(View view)
        {
            if (this.RadialAxis != null && !this.RadialAxis.AnnotationsLayout.Children.Contains(view))
            {
                view.BindingContext = this;
                this.RadialAxis.AnnotationsLayout.Children.Add(view);
            }
        }

        /// <summary>
        /// Method used to remove <see cref="Content"/> in radial gauge pointers layout.
        /// </summary>
        /// <param name="view">Represents the view to remove.</param>
        internal void RemoveCustomView(View view)
        {
            if (this.RadialAxis != null && this.RadialAxis.AnnotationsLayout.Children.Contains(view))
            {
                this.RadialAxis.AnnotationsLayout.Children.Remove(view);
            }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when marker pointer <see cref="Content"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ContentPointer contentPointer)
            {
                if (oldValue is View oldView)
                {
                    oldView.PropertyChanged -= contentPointer.Content_PropertyChanged;
                    contentPointer.RemoveCustomView(oldView);
                }

                if (newValue is View newView)
                {
                    newView.PropertyChanged += contentPointer.Content_PropertyChanged;
                    contentPointer.AddCustomView(newView);
                }

                contentPointer.UpdatePointer();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// To create marker pointer.
        /// </summary>
        private void CreateContentPointer()
        {
            if (this.RadialAxis == null || this.RadialAxis.AvailableSize.IsZero)
            {
                return;
            }

            if (this.CanAnimate)
            {
                this.AnimatePointer(this.RadialAxis.ActualMinimum, this.Value);
                this.CanAnimate = false;
            }
            else
            {
                this.UpdatePointer();
            }
        }

        /// <summary>
        /// Called when <see cref="Content"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void Content_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == VisualElement.HeightProperty.PropertyName ||
                e.PropertyName == VisualElement.WidthRequestProperty.PropertyName)
            {
                this.UpdatePointer();
            }
        }

        #endregion
    }
}
