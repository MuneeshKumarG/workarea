using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Create the pointer to indicate the value with built-in shape.
    /// To highlight values, set the bar pointer pointer type to a built-in shape, such as a circle, triangle, inverted triangle, square, or diamond.
    /// </summary>
    public class BarPointer : LinearPointer
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Offset"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Offset"/> bindable property.
        /// </value>
        public static readonly BindableProperty OffsetProperty =
            BindableProperty.Create(nameof(Offset), typeof(double), typeof(BarPointer), 0d, propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="PointerSize"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PointerSize"/> bindable property.
        /// </value>
        public static readonly BindableProperty PointerSizeProperty =
            BindableProperty.Create(nameof(PointerSize), typeof(double), typeof(BarPointer), 10d, propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CornerStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CornerStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty CornerStyleProperty =
            BindableProperty.Create(nameof(CornerStyle), typeof(CornerStyle), typeof(BarPointer), CornerStyle.BothFlat, propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BorderWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BorderWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(BarPointer), 0d, propertyChanged: OnInvalidatePropertyChanged);

        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(BarPointer), new SolidColorBrush(Color.FromRgb(73, 89, 99)), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Stroke"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(BarPointer), Color.FromRgb(73, 89, 99), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="GradientStops"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="GradientStops"/> bindable property.
        /// </value>
        public static readonly BindableProperty GradientStopsProperty =
          BindableProperty.Create(nameof(GradientStops), typeof(ObservableCollection<GaugeGradientStop>), typeof(BarPointer), null);

        #endregion

        #region Fields

        private PathF? barPointerPath;
        private LinearGradientBrush? linearGradientBrush;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to adjusts the <see cref="BarPointer"/> position from the axis line.
        /// </summary>
        /// <value>
        /// Its default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// Specifies the distance between the pointer and the axis line. <see cref="BarPointer"/> rendered over the axis line by default. 
        /// </remarks>
        public double Offset
        {
            get { return (double)this.GetValue(OffsetProperty); }
            set { this.SetValue(OffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the height of <see cref="BarPointer"/> for a horizontal linear gauge, or to the width of bar pointer for a vertical linear gauge.
        /// </summary>
        /// <value>
        /// The size of the <see cref="BarPointer"/>, in pixels. The default value is <c>4</c>.
        /// </value>
        public double PointerSize
        {
            get { return (double)this.GetValue(PointerSizeProperty); }
            set { this.SetValue(PointerSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a <see cref="CornerStyle"/> enumeration value that describes the corner/edge style of the  <see cref="BarPointer"/>.
        /// </summary>
        /// <value>
        /// A value of the enumeration. The default is <see cref="CornerStyle.BothFlat"/>.
        /// </value>
        public CornerStyle CornerStyle
        {
            get { return (CornerStyle)this.GetValue(CornerStyleProperty); }
            set { this.SetValue(CornerStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the collection of <see cref="GaugeGradientStop"/> that specifies how interior is painted with gradient brush on the <see cref="BarPointer"/>.
        /// </summary>
        /// <value>
        /// A collection of the <see cref="GaugeGradientStop"/> objects associated with the brush, each of which specifies a color and an offset along the axis.
        /// The default is an empty collection.
        /// </value>
        public ObservableCollection<GaugeGradientStop> GradientStops
        {
            get { return (ObservableCollection<GaugeGradientStop>)this.GetValue(GradientStopsProperty); }
            set { this.SetValue(GradientStopsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the bar pointer border outline.
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
        /// Gets or sets a value that specifies the bar pointer fill color of pointer. 
        /// </summary>
        public Brush Fill
        {
            get { return (Brush)this.GetValue(FillProperty); }
            set { this.SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the bar pointer stroke color of pointer. 
        /// </summary>
        public Color Stroke
        {
            get { return (Color)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        #endregion

        #region Override methods

        internal override void Draw(ICanvas canvas)
        {
            if (this.Scale != null)
            {
                canvas.SaveState();

                if (this.barPointerPath != null)
                {
                    if (this.linearGradientBrush != null)
                        canvas.SetFillPaint(this.linearGradientBrush, barPointerPath.Bounds);
                    else
                        canvas.SetFillPaint(this.Fill, barPointerPath.Bounds);

                    canvas.FillPath(this.barPointerPath);
                }

                canvas.RestoreState();
            }
        }

        internal override void CreatePointer()
        {
            this.CreateBarPointer();
        }

        internal override void UpdatePointer()
        {
            if (this.Scale != null && !this.Scale.ScaleAvailableSize.IsZero)
            {
                double actualStartValue = this.Scale.ActualMinimum;
                double actualEndValue = Math.Clamp(this.EnableAnimation && this.AnimationValue != null ?
                    (double)this.AnimationValue : this.Value, this.Scale.ActualMinimum, this.Scale.ActualMaximum);

                Utility.ValidateMinimumMaximumValue(ref actualStartValue, ref actualEndValue);
                double halfWidth = this.PointerSize > 0 ? this.PointerSize / 2 : 0d;
                double halfLineThickness = this.Scale.GetActualAxisLineThickness();
                double pointerStartPosition = this.Scale.GetPositionFromValue(actualStartValue);
                double pointerEndPosition = this.Scale.GetPositionFromValue(actualEndValue);
                double halfPointerWidth = this.PointerSize / 2;
                if (this.Scale.Orientation == GaugeOrientation.Vertical ^ this.Scale.IsInversed)
                {
                    halfPointerWidth *= -1;
                }

                if (this.CornerStyle == CornerStyle.BothCurve || this.CornerStyle == CornerStyle.StartCurve)
                {
                    pointerStartPosition += halfPointerWidth;
                }

                if (this.CornerStyle == CornerStyle.BothCurve || this.CornerStyle == CornerStyle.EndCurve)
                {
                    pointerEndPosition -= halfPointerWidth;
                }

                barPointerPath = new PathF();
                double axisLinePositionX = this.Scale.AxisLinePosition.X;
                double axisLinePositionY = this.Scale.AxisLinePosition.Y;

                if (this.Scale.IsMirrored)
                {
                    MoveToRangePath(axisLinePositionX + pointerStartPosition, axisLinePositionY + halfLineThickness - this.Offset + halfWidth);
                    LineToRangePath(axisLinePositionX + pointerEndPosition, axisLinePositionY + halfLineThickness - this.Offset + halfWidth);
                    LineToRangePath(axisLinePositionX + pointerEndPosition, axisLinePositionY + halfLineThickness - this.Offset - halfWidth);
                    LineToRangePath(axisLinePositionX + pointerStartPosition, axisLinePositionY + halfLineThickness - this.Offset - halfWidth);
                }
                else
                {
                    MoveToRangePath(axisLinePositionX + pointerStartPosition, axisLinePositionY + halfLineThickness + this.Offset - halfWidth);
                    LineToRangePath(axisLinePositionX + pointerEndPosition, axisLinePositionY + halfLineThickness + this.Offset - halfWidth);
                    LineToRangePath(axisLinePositionX + pointerEndPosition, axisLinePositionY + halfLineThickness + this.Offset + halfWidth);
                    LineToRangePath(axisLinePositionX + pointerStartPosition, axisLinePositionY + halfLineThickness + this.Offset + halfWidth);
                }

                barPointerPath.Close();
            }
        }

        private void MoveToRangePath(double x, double y)
        {
            if (this.Scale == null || barPointerPath == null)
                return;

            if (this.Scale.Orientation == GaugeOrientation.Horizontal)
                barPointerPath.MoveTo((float)x, (float)y);
            else
                barPointerPath.MoveTo((float)y, (float)x);
        }

        private void LineToRangePath(double x, double y)
        {
            if (this.Scale == null || barPointerPath == null)
                return;

            if (this.Scale.Orientation == GaugeOrientation.Horizontal)
                barPointerPath.LineTo((float)x, (float)y);
            else
                barPointerPath.LineTo((float)y, (float)x);
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when bar pointer <see cref="Fill"/>, <see cref="Stroke"/>, and <see cref="BorderWidth"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnInvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BarPointer barPointer)
            {
                barPointer.InvalidateDrawable();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// To create bar pointer.
        /// </summary>
        private void CreateBarPointer()
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
