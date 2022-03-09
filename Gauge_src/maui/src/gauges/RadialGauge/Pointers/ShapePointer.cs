using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Create the pointer to indicate the value with built-in shape.
    /// To highlight values, set the shape pointer type to a built-in shape, such as a circle, triangle, inverted triangle, square, or diamond.
    /// </summary>
    public class ShapePointer : MarkerPointer
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="BorderWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BorderWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(ShapePointer), 0d, propertyChanged: OnInvalidatePropertyChanged);

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
        public static readonly BindableProperty ShapeTypeProperty =
            BindableProperty.Create(nameof(ShapeType), typeof(ShapeType), typeof(ShapePointer), ShapeType.InvertedTriangle, propertyChanged: OnShapeTypePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(ShapePointer), new SolidColorBrush(Color.FromRgb(73, 89, 99)), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Stroke"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(ShapePointer), Color.FromRgb(73, 89, 99), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="OverlayFill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OverlayFill"/> bindable property.
        /// </value>
        public static readonly BindableProperty OverlayFillProperty =
            BindableProperty.Create(nameof(OverlayFill), typeof(Brush), typeof(ShapePointer), null);

        /// <summary>
        /// Identifies the <see cref="OverlayRadius"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OverlayRadius"/> bindable property.
        /// </value>
        public static readonly BindableProperty OverlayRadiusProperty =
            BindableProperty.Create(nameof(OverlayRadius), typeof(double), typeof(ShapePointer), double.NaN);

        /// <summary>
        /// Identifies the <see cref="HasShadow"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HasShadow"/> bindable property.
        /// </value>
        public static readonly BindableProperty HasShadowProperty =
            BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(ShapePointer), false, propertyChanged: OnInvalidatePropertyChanged);

        #endregion

        #region Fields

        /// <summary>
        /// Represents shape pointer angle.
        /// </summary>
        private float shapeAngle;

        /// <summary>
        /// Represents shape pointer position. 
        /// </summary>
        private PointF shapePosition;

        /// <summary>
        /// Reprsents shape gets hovered with mouse or not. 
        /// </summary>
        internal bool IsHovered;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the width of the shape border outline.
        /// </summary>
        /// <value>
        /// It defines the thickness of the border. The default value is <c>0</c>.
        /// </value>
        public double BorderWidth
        {
            get { return (double)this.GetValue(BorderWidthProperty); }
            set { this.SetValue(BorderWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the shape height in logical pixels.
        /// </summary>
        /// <value>
        /// It defines the height of the shape. The default value is <c>16</c>.
        /// </value>
        public double ShapeHeight
        {
            get { return (double)this.GetValue(ShapeHeightProperty); }
            set { this.SetValue(ShapeHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the shape width in logical pixels.
        /// </summary>
        /// <value>
        /// It defines the width of the shape. The default value is <c>16</c>.
        /// </value>
        public double ShapeWidth
        {
            get { return (double)this.GetValue(ShapeWidthProperty); }
            set { this.SetValue(ShapeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the shape type for the pointer. 
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the shape type of shape pointer in the radial gauge.
        /// The default is <see cref="ShapeType.InvertedTriangle"/>.
        /// </value>
        public ShapeType ShapeType
        {
            get { return (ShapeType)this.GetValue(ShapeTypeProperty); }
            set { this.SetValue(ShapeTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the shape fill color of pointer. 
        /// </summary>
        public Brush Fill
        {
            get { return (Brush)this.GetValue(FillProperty); }
            set { this.SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the shape stroke color of pointer. 
        /// </summary>
        public Color Stroke
        {
            get { return (Color)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the shape overlay fill color of pointer. 
        /// </summary>
        /// <value>
        /// It defines the color of the overlay fill. The default value is <c>Null</c>.
        /// </value>
        public Brush OverlayFill
        {
            get { return (Brush)this.GetValue(OverlayFillProperty); }
            set { this.SetValue(OverlayFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the shape overlay fill radius of pointer. 
        /// </summary>
        /// <value>
        /// It defines the radius of overlay fill. The default value is <c>double.NaN</c>.
        /// </value>
        public double OverlayRadius
        {
            get { return (double)this.GetValue(OverlayRadiusProperty); }
            set { this.SetValue(OverlayRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the shadow effect for shape pointer. 
        /// </summary>
        /// <value>
        /// It defines the shadow visibility of shape pointer. The default value is <c>False</c>.
        /// </value>
        public bool HasShadow
        {
            get { return (bool)this.GetValue(HasShadowProperty); }
            set { this.SetValue(HasShadowProperty, value); }
        }

        internal bool CanDrawOverlay
        {
            get
            {
                return double.IsNaN(OverlayRadius) || this.OverlayRadius > this.ShapeWidth/2;
            }
        }

        #endregion

        #region Override methods

        internal override void Draw(ICanvas canvas)
        {
            if (this.RadialAxis != null)
            {
                DrawShapePointer(canvas);
            }
        }

        internal override void CreatePointer()
        {
            this.CreateShapePointer();
        }

        internal override void UpdatePointer()
        {
            if (this.RadialAxis != null && !this.RadialAxis.AvailableSize.IsZero)
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

                markerHeight = this.ShapeHeight;
                markerWidth = this.ShapeWidth;

                shapePosition = new PointF((float)(this.RadialAxis.Center.X + (radius * vector.X) - (markerWidth / 2)),
                        (float)(this.RadialAxis.Center.Y + (radius * vector.Y) - (markerHeight / 2)));

                shapeAngle = (float)this.RadialAxis.FactorToAngle(factor) + 90;

                //Calculate dragging rectangle. 
                this.PointerRect = new RectangleF(this.shapePosition.X - DraggingOffset, this.shapePosition.Y - DraggingOffset,
                (float)markerWidth + (DraggingOffset * 2), (float)markerHeight + (DraggingOffset * 2));

            }
        }

        internal override void UpdatePointerReleased()
        {
            if (this.IsPressed)
            {
                this.InvalidateDrawable();
                this.IsHovered = false;
            }

            base.UpdatePointerReleased();
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
            if (bindable is ShapePointer pointer)
            {
                pointer.UpdatePointer();
                pointer.InvalidateDrawable();
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
            if (bindable is ShapePointer pointer)
            {
                pointer.UpdatePointer();
                pointer.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when shape pointer <see cref="Fill"/>, <see cref="Stroke"/>, and <see cref="BorderWidth"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnInvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ShapePointer pointer)
            {
                pointer.InvalidateDrawable();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// To create shape pointer.
        /// </summary>
        private void CreateShapePointer()
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
        /// Draw shape pointer defined shapes. 
        /// </summary>
        /// <param name="canvas"></param>
        private void DrawShapePointer(ICanvas canvas)
        {
            canvas.SaveState();

            float halfBorderWidth = (float)this.BorderWidth / 2;
            float halfWidth = (float)this.ShapeWidth / 2;
            float halfHeight = (float)this.ShapeHeight / 2;
            float positionX = this.shapePosition.X + halfBorderWidth;
            float positionY = this.shapePosition.Y + halfBorderWidth;
            float width = (float)this.ShapeWidth - (float)this.BorderWidth;
            float height = (float)this.ShapeHeight - (float)this.BorderWidth;

            //Set stroke and stroke size for shape.
            if (this.BorderWidth > 0)
            {
                canvas.StrokeColor = Stroke;
                canvas.StrokeSize = (float)this.BorderWidth;
            }

            float angle;
            //Set rotation angle for shape.
            if (this.ShapeType == ShapeType.Diamond)
            {
                angle = this.shapeAngle + 45;
                canvas.Rotate(angle, this.shapePosition.X + halfWidth, this.shapePosition.Y + halfHeight);
            }
            else if (this.ShapeType == ShapeType.Triangle)
            {
                angle = this.shapeAngle + 180;
                canvas.Rotate(angle, this.shapePosition.X + halfWidth, this.shapePosition.Y + halfHeight);
            }
            else
            {
                angle = this.shapeAngle;
                canvas.Rotate(angle, this.shapePosition.X + halfWidth, this.shapePosition.Y + halfHeight);
            }

            //Draw shape shape overlay.
            if (this.CanDrawOverlay && (this.IsPressed || this.IsHovered))
            {
                DrawShapeOverlay(canvas);
            }

            if (this.HasShadow)
            {
                canvas.SetShadow(new SizeF(0, 2), 10, Color.FromRgb(148, 148, 148));
            }

            canvas.SetFillPaint(this.Fill, new RectangleF(positionX, positionY, width, height));

            //Draw shape shape.
            DrawShape(positionX, positionY, width, height, canvas, BorderWidth > 0 ? true : false);

            canvas.RestoreState();
        }

        /// <summary>
        /// Method used to draw overlay for shape pointer. 
        /// </summary>
        /// <param name="canvas"></param>
        private void DrawShapeOverlay(ICanvas canvas)
        {
            if (this.OverlayFill == null && this.Fill == null)
                return;

            bool isNanOverlayRadius = double.IsNaN(this.OverlayRadius);
            float actualOverlayWidth = (float)(isNanOverlayRadius ? this.ShapeWidth : this.OverlayRadius)- (float)this.ShapeWidth / 2;
            float actualOverlayHeight = (float)(isNanOverlayRadius ? this.ShapeHeight : this.OverlayRadius)- (float)this.ShapeHeight / 2;
            float overlayPositionX = this.shapePosition.X - actualOverlayWidth;
            float overlayPositionY = this.shapePosition.Y - actualOverlayHeight;
            float overlayWidth = (2 * actualOverlayWidth) + (float)this.ShapeWidth;
            float overlayHeight = (2 * actualOverlayHeight) + (float)this.ShapeHeight;

            if (this.OverlayFill == null)
            {
                canvas.FillColor = ((Paint)this.Fill)?.ToColor()?.WithAlpha(0.12f);
            }
            else
            {
                canvas.SetFillPaint(this.OverlayFill, new RectangleF(overlayPositionX, overlayPositionY, overlayWidth, overlayHeight));
            }

            DrawShape(overlayPositionX, overlayPositionY, overlayWidth, overlayHeight, canvas, false);
        }

        private void DrawShape(float positionX,float positionY, float width, float height, 
            ICanvas canvas, bool drawBorder)
        {
            switch (this.ShapeType)
            {
                case ShapeType.Circle:
                    canvas.FillEllipse(positionX, positionY, width, height);

                    if (drawBorder)
                    {
                        canvas.DrawEllipse(positionX, positionY, width, height);
                    }
                    break;
                case ShapeType.Diamond:
                case ShapeType.Rectangle:
                    canvas.FillRectangle(positionX, positionY, width, height);

                    if (drawBorder)
                    {
                        canvas.DrawRectangle(positionX, positionY, width, height);
                    }
                    break;
                case ShapeType.InvertedTriangle:
                case ShapeType.Triangle:
                    PathF path = new PathF();
                    path.LineTo(positionX, positionY);
                    path.LineTo(positionX + width, positionY);
                    path.LineTo(positionX + width / 2, positionY + height);
                    path.Close();
                    canvas.FillPath(path);

                    if (drawBorder)
                    {
                        canvas.DrawPath(path);
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
