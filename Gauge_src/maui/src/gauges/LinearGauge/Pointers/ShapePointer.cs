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
    /// Create the pointer to indicate the value with built-in shape.
    /// To highlight values, set the bar pointer pointer type to a built-in shape, such as a circle, triangle, inverted triangle or diamond.
    /// </summary>
    public class ShapePointer : LinearMarkerPointer
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="ShapeHeight"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShapeHeight"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShapeHeightProperty =
            BindableProperty.Create(nameof(ShapeHeight), typeof(double), typeof(ShapePointer), 16d, propertyChanged: OnShapePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShapeWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShapeWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShapeWidthProperty =
            BindableProperty.Create(nameof(ShapeWidth), typeof(double), typeof(ShapePointer), 16d, propertyChanged: OnShapePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShapeType"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShapeType"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShapeTypeProperty = BindableProperty.Create(nameof(ShapeType),
            typeof(MarkerType), typeof(ShapePointer), MarkerType.InvertedTriangle, propertyChanged: OnShapeTypePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty = BindableProperty.Create(nameof(Fill), typeof(Brush),
            typeof(ShapePointer), new SolidColorBrush(Color.FromRgb(73, 89, 99)), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StrokeThickness"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeThicknessProperty =
            BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(ShapePointer), 0d, propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Stroke"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(ShapePointer), Color.FromRgb(73, 89, 99), propertyChanged: OnInvalidatePropertyChanged);

        #endregion

        #region Fields

        private double shapePositionX, shapePositionY;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that specifies the default shape type for the pointer.
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the shape type of shape pointer in the linear gauge.
        /// The default is <see cref="MarkerType.InvertedTriangle"/>.
        /// </value>
        public MarkerType ShapeType
        {
            get { return (MarkerType)this.GetValue(ShapeTypeProperty); }
            set { this.SetValue(ShapeTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the shape height.
        /// </summary>
        /// <value>
        /// The height of the <see cref="ShapePointer"/>, in pixels. The default value is <c>16</c>.
        /// </value>
        public double ShapeHeight
        {
            get { return (double)this.GetValue(ShapeHeightProperty); }
            set { this.SetValue(ShapeHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the shape width.
        /// </summary>
        /// <value>
        /// The width of the <see cref="ShapePointer"/>, in pixels. The default value is <c>16</c>.
        /// </value>
        public double ShapeWidth
        {
            get { return (double)this.GetValue(ShapeWidthProperty); }
            set { this.SetValue(ShapeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush that paints the interior of the shape.
        /// </summary>
        /// <value>
        /// A <c>Brush</c> that specifies how the <see cref="ShapePointer"/> interior is painted.
        /// </value>
        public Brush Fill
        {
            get { return (Brush)this.GetValue(FillProperty); }
            set { this.SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <c>Brush</c> that specifies how the shape outline to be painted.
        /// </summary>
        /// <value>
        /// A <c>Brush</c> that specifies how the <see cref="ShapePointer"/> outline is painted.
        /// </value>
        public Color Stroke
        {
            get { return (Color)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the width of the shape stroke outline.
        /// </summary>
        /// <value>
        /// It specifies the width of the <see cref="ShapePointer"/> stroke outline.
        /// </value>
        public double StrokeThickness
        {
            get { return (double)this.GetValue(StrokeThicknessProperty); }
            set { this.SetValue(StrokeThicknessProperty, value); }
        }

        #endregion

        #region Override methods

        internal override void Draw(ICanvas canvas)
        {
            if (this.Scale != null)
            {
                canvas.SaveState();

                PointF shapePosition = new PointF((float)shapePositionX, (float)shapePositionY);

                float halfBorderWidth = (float)this.StrokeThickness / 2;
                float halfWidth = (float)ShapeWidth / 2;
                float halfHeight = (float)ShapeHeight / 2;
                float positionX = shapePosition.X + halfBorderWidth;
                float positionY = shapePosition.Y + halfBorderWidth;
                float width = (float)(ShapeWidth - this.StrokeThickness);
                float height = (float)(ShapeHeight - this.StrokeThickness);

                //Set stroke and stroke size for shape.
                if (this.StrokeThickness > 0)
                {
                    canvas.StrokeColor = Stroke;
                    canvas.StrokeSize = (float)this.StrokeThickness;
                }

                //Set rotation angle for shape.
                float angle = this.Scale.IsMirrored ? 180 : 0;

                if(this.ShapeType == MarkerType.Triangle)
                    angle += 180;

                canvas.Rotate(angle, shapePosition.X + halfWidth, shapePosition.Y + halfHeight);

                canvas.SetFillPaint(this.Fill, new RectangleF(positionX, positionY, width, height));

                //Draw shape shape.
                switch (this.ShapeType)
                {
                    case MarkerType.Circle:

                        canvas.FillEllipse(positionX, positionY, width, height);

                        if (StrokeThickness > 0)
                        {
                            canvas.DrawEllipse(positionX, positionY, width, height);
                        }

                        break;

                    case MarkerType.Rectangle:
                        canvas.FillRectangle(positionX, positionY, width, height);

                        if (StrokeThickness > 0)
                        {
                            canvas.DrawRectangle(positionX, positionY, width, height);
                        }
                        break;
                    case MarkerType.Diamond:

                        PathF path = new PathF();
                        path.MoveTo(positionX + width / 2, positionY);
                        path.LineTo(positionX + width, positionY + height / 2);
                        path.LineTo(positionX + width / 2, positionY + height);
                        path.LineTo(positionX, positionY + height / 2);
                        path.Close();

                        canvas.FillPath(path);

                        if (StrokeThickness > 0)
                        {
                            canvas.DrawPath(path);
                        }

                        break;
                    case MarkerType.InvertedTriangle:
                    case MarkerType.Triangle:
                        path = new PathF();

                        if (Scale.Orientation == GaugeOrientation.Horizontal)
                        {
                            path.LineTo(positionX, positionY);
                            path.LineTo(positionX + width, positionY);
                            path.LineTo(positionX + width / 2, positionY + height);
                        }
                        else
                        {
                            path.LineTo(positionX, positionY);
                            path.LineTo(positionX, positionY + height);
                            path.LineTo(positionX + width, positionY + height / 2);
                        }
                        path.Close();

                        canvas.FillPath(path);

                        if (StrokeThickness > 0)
                        {
                            canvas.DrawPath(path);
                        }

                        break;
                    default:
                        break;
                }
                canvas.RestoreState();
            }
        }

        internal override void CreatePointer()
        {
            this.CreateShapePointer();
        }

        internal override void UpdatePointer()
        {
            if (this.Scale != null && !this.Scale.ScaleAvailableSize.IsZero)
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

                double halfWidth = this.ShapeWidth / 2;
                double halfHeight = this.ShapeHeight / 2;
                double lineThickness = this.Scale.GetActualScaleLineThickness();
                if (this.Scale.Orientation == GaugeOrientation.Vertical)
                {
                    Utility.Swap(ref halfWidth, ref halfHeight);
                }

                shapePositionX = this.Scale.ScalePosition.X + this.Scale.GetPositionFromValue(actualValue) - halfWidth;
                shapePositionY = this.Scale.ScalePosition.Y + (lineThickness / 2) - halfHeight;

                this.GetPointerPosition(halfWidth, halfHeight + (lineThickness / 2), ref shapePositionX, ref shapePositionY);
                if (this.Scale.Orientation == GaugeOrientation.Vertical)
                {
                    Utility.Swap(ref shapePositionX, ref shapePositionY);
                }

                //Calculate dragging rectangle. 
                this.PointerRect = new Rectangle(shapePositionX - DraggingOffset, shapePositionY - DraggingOffset,
                ShapeWidth + (DraggingOffset * 2), ShapeHeight + (DraggingOffset * 2));
            }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when shape pointer <see cref="ShapeHeight"/>, <see cref="ShapeWidth"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnShapePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ShapePointer shapePointer && shapePointer.Scale != null)
            {
                shapePointer.Scale.ScaleInvalidateMeasureOverride();
            }
        }

        /// <summary>
        /// Called when shape pointer <see cref="ShapeType"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnShapeTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ShapePointer shapePointer)
            {
                shapePointer.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when shape pointer <see cref="Fill"/>, <see cref="Stroke"/>, and <see cref="StrokeThickness"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnInvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ShapePointer shapePointer)
            {
                shapePointer.InvalidateDrawable();
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
