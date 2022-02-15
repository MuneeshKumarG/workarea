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
    /// Create the pointer to indicate the value with bar shape.
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
            BindableProperty.Create(nameof(Offset), typeof(double), typeof(BarPointer), 0d, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="PointerSize"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PointerSize"/> bindable property.
        /// </value>
        public static readonly BindableProperty PointerSizeProperty =
            BindableProperty.Create(nameof(PointerSize), typeof(double), typeof(BarPointer), 12d, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CornerStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CornerStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty CornerStyleProperty =
            BindableProperty.Create(nameof(CornerStyle), typeof(CornerStyle), typeof(BarPointer), CornerStyle.BothFlat, propertyChanged: OnBarPointerPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(BarPointer), new SolidColorBrush(Color.FromRgb(73, 89, 99)), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="GradientStops"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="GradientStops"/> bindable property.
        /// </value>
        public static readonly BindableProperty GradientStopsProperty =
          BindableProperty.Create(nameof(GradientStops), typeof(ObservableCollection<GaugeGradientStop>), typeof(BarPointer), null, propertyChanged: OnGradientStopsPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Child"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Child"/> bindable property.
        /// </value>
        public static readonly BindableProperty ChildProperty =
          BindableProperty.Create(nameof(Child), typeof(View), typeof(BarPointer), null, propertyChanged: OnChildPropertyChanged);


        /// <summary>
        /// Identifies the <see cref="BarPosition"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BarPosition"/> bindable property.
        /// </value>
        public static readonly BindableProperty BarPositionProperty =
            BindableProperty.Create(nameof(BarPosition), typeof(GaugeElementPosition), typeof(BarPointer), GaugeElementPosition.Cross, propertyChanged: OnPropertyChanged);

        #endregion

        #region Fields

        private PathF? barPointerPath;
        private LinearGradientBrush? linearGradientBrush;
        private double actualStartValue, actualEndValue;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to adjusts the <see cref="BarPointer"/> position from the scale line.
        /// </summary>
        /// <value>
        /// Its default value is <c>0</c>.
        /// </value>
        /// <remarks>
        /// Specifies the distance between the pointer and the scale line. <see cref="BarPointer"/> rendered over the scale line by default. 
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
        /// A collection of the <see cref="GaugeGradientStop"/> objects associated with the brush, each of which specifies a color and an offset along the scale.
        /// The default is an empty collection.
        /// </value>
        public ObservableCollection<GaugeGradientStop> GradientStops
        {
            get { return (ObservableCollection<GaugeGradientStop>)this.GetValue(GradientStopsProperty); }
            set { this.SetValue(GradientStopsProperty, value); }
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
        /// Gets or sets the child content of a <see cref="BarPointer"/>. 
        /// </summary>
        /// <value>
        /// An object that contains the pointer's visual child content. The default value is <c>null</c>.
        /// </value>
        public View Child
        {
            get { return (View)this.GetValue(ChildProperty); }
            set { this.SetValue(ChildProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the bar pointer position. Specify the value in <see cref="GaugeElementPosition"/>.
        /// </summary>
        /// <value>
        /// The default value is <see cref="GaugeElementPosition.Cross"/>.
        /// </value>
        public GaugeElementPosition BarPosition
        {
            get { return (GaugeElementPosition)this.GetValue(BarPositionProperty); }
            set { this.SetValue(BarPositionProperty, value); }
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
                actualStartValue = this.Scale.ActualMinimum;
                actualEndValue = Math.Clamp((this.EnableAnimation || this.Scale.CanAnimate) && this.AnimationValue != null ?
                    (double)this.AnimationValue : this.Value, this.Scale.ActualMinimum, this.Scale.ActualMaximum);

                Utility.ValidateMinimumMaximumValue(ref actualStartValue, ref actualEndValue);
                float halfWidth = this.PointerSize > 0 ? (float)this.PointerSize / 2 : 0f;
                float scaleLinePositionY;
                float lineThickness = (float)this.Scale.GetActualScaleLineThickness();
                float pointerStartPosition = (float)this.Scale.GetPositionFromValue(actualStartValue);
                float pointerEndPosition = (float)this.Scale.GetPositionFromValue(actualEndValue);
                float halfPointerWidth = (float)this.PointerSize / 2;
                bool isInversed = false;

                if (this.BarPosition == GaugeElementPosition.Cross)
                    scaleLinePositionY = (float)this.Scale.ScalePosition.Y + lineThickness / 2;
                else if (this.BarPosition == GaugeElementPosition.Inside)
                    scaleLinePositionY = (float)this.Scale.ScalePosition.Y + lineThickness + halfWidth;
                else
                    scaleLinePositionY = (float)this.Scale.ScalePosition.Y - halfWidth;

                if (this.Scale.Orientation == GaugeOrientation.Vertical ^ this.Scale.IsInversed)
                {
                    halfPointerWidth *= -1;
                    isInversed = true;
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
                float x1, x2, y1, y2;
                float startPointX1 = (float)this.Scale.ScalePosition.X + pointerStartPosition;
                float startPointX2 = (float)this.Scale.ScalePosition.X + pointerEndPosition;

                bool canDrawBarPointer = false;

                if ((isInversed && startPointX1 > startPointX2) || (!isInversed && startPointX1 < startPointX2))
                {
                    canDrawBarPointer = true;
                }
                
                if (this.Scale.IsMirrored)
                {
                    y1 = (float)(scaleLinePositionY - this.Offset - halfWidth);
                    y2 = (float)(scaleLinePositionY - this.Offset + halfWidth);

                    if (canDrawBarPointer)
                    {
                        this.Scale.MoveToPath(barPointerPath, startPointX1, y2);
                        this.Scale.LineToPath(barPointerPath, startPointX2, y2);
                        this.Scale.LineToPath(barPointerPath, startPointX2, y1);
                        this.Scale.LineToPath(barPointerPath, startPointX1, y1);
                        barPointerPath.Close();
                    }
                }
                else
                {
                    y1 = (float)(scaleLinePositionY + this.Offset - halfWidth);
                    y2 = (float)(scaleLinePositionY + this.Offset + halfWidth);

                    if (canDrawBarPointer)
                    {
                        this.Scale.MoveToPath(barPointerPath, startPointX1, y1);
                        this.Scale.LineToPath(barPointerPath, startPointX2, y1);
                        this.Scale.LineToPath(barPointerPath, startPointX2, y2);
                        this.Scale.LineToPath(barPointerPath, startPointX1, y2);
                        barPointerPath.Close();
                    }
                }

                if (this.CornerStyle == CornerStyle.StartCurve || this.CornerStyle == CornerStyle.BothCurve)
                {
                    x1 = (float)(startPointX1 - halfWidth);
                    x2 = (float)(startPointX1 + halfWidth);
                    float curveX1 = (float)startPointX1;

                    if ((this.Scale.Orientation == GaugeOrientation.Horizontal && !this.Scale.IsInversed) ||
                        (this.Scale.Orientation == GaugeOrientation.Vertical && this.Scale.IsInversed))
                    {
                        if (startPointX1 > startPointX2)
                        {
                            double diff = Math.Abs(startPointX1 - startPointX2);
                            curveX1 = startPointX2;
                            x1 = (float)(curveX1 - halfWidth + diff);
                            x2 = (float)(curveX1 + halfWidth - diff);
                        }
                    }
                    else if (startPointX1 < startPointX2)
                    {
                        double diff = Math.Abs(startPointX1 - startPointX2);
                        curveX1 = startPointX2;
                        x1 = (float)(curveX1 - halfWidth + diff);
                        x2 = (float)(curveX1 + halfWidth - diff);

                    }

                    if (x1 < x2)
                    {
                        Scale.MoveToPath(barPointerPath, curveX1, y2);

                        if (Scale.Orientation == GaugeOrientation.Horizontal)
                            barPointerPath.AddArc(x1, y1, x2, y2, Scale.IsInversed ? 270 : 90, Scale.IsInversed ? 90 : 270, false);
                        else
                            barPointerPath.AddArc(y1, x1, y2, x2, Scale.IsInversed ? 360 : 180, Scale.IsInversed ? 180 : 360, false);

                        barPointerPath.Close();
                    }
                }

                if (this.CornerStyle == CornerStyle.EndCurve || this.CornerStyle == CornerStyle.BothCurve)
                {
                    x1 = (float)(startPointX2 - halfWidth);
                    x2 = (float)(startPointX2 + halfWidth);

                    if ((this.Scale.Orientation == GaugeOrientation.Horizontal && !this.Scale.IsInversed) ||
                        (this.Scale.Orientation == GaugeOrientation.Vertical && this.Scale.IsInversed))
                    {
                        if (startPointX1 > x1)
                        {
                            double diff = Math.Abs(startPointX1 - x1);
                            x1 = (float)(startPointX2 - halfWidth + diff);
                            x2 = (float)(startPointX2 + halfWidth - diff);
                        }
                    }
                    else if (startPointX1 < x2)
                    {
                        double diff = Math.Abs(startPointX1 - x2);
                        x1 = (float)(startPointX2 - halfWidth + diff);
                        x2 = (float)(startPointX2 + halfWidth - diff);

                    }

                    if (x1 < x2)
                    {
                        Scale.MoveToPath(barPointerPath, (float)startPointX2, y2);

                        if (Scale.Orientation == GaugeOrientation.Horizontal)
                            barPointerPath.AddArc(x1, y1, x2, y2, Scale.IsInversed ? 90 : 270, Scale.IsInversed ? 270 : 90, false);
                        else
                            barPointerPath.AddArc(y1, x1, y2, x2, Scale.IsInversed ? 180 : 360, Scale.IsInversed ? 360 : 180, false);

                        barPointerPath.Close();
                    }
                }

                this.CreateGradient();

                if (this.Child != null)
                {
                    Scale.UpdateChild(this.Child, barPointerPath.Bounds);
                }

                //Calculate interaction pointer rect.
                float size = DraggingOffset * 2;

                if (this.CornerStyle == CornerStyle.EndCurve || this.CornerStyle == CornerStyle.BothCurve)
                    startPointX2 = startPointX2 + (Scale.Orientation == GaugeOrientation.Vertical ^ this.Scale.IsInversed ? -halfWidth : halfWidth);

                if (Scale.Orientation == GaugeOrientation.Horizontal)
                    this.PointerRect = new Rectangle(startPointX2 - DraggingOffset, y1 - DraggingOffset, size, size + PointerSize);
                else
                    this.PointerRect = new Rectangle(y1 - DraggingOffset, startPointX2 - DraggingOffset, size + PointerSize, size);
            }
        }

        /// <summary>
        /// Invoked whenever the binding context of the View changes.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.GradientStops != null)
            {
                foreach (var gradientStop in this.GradientStops)
                    SetInheritedBindingContext(gradientStop, BindingContext);
            }

            if (this.Child != null)
                SetInheritedBindingContext(Child, this);
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when bar pointer <see cref="Fill"/> changed.
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

        /// <summary>
        /// Called when bar pointer <see cref="CornerStyle"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnBarPointerPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BarPointer pointer)
            {
                pointer.UpdatePointer();
                pointer.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when bar pointer <see cref="Offset"/>, <see cref="PointerSize"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BarPointer pointer && pointer.Scale != null)
            {
                pointer.Scale.ScaleInvalidateMeasureOverride();
            }
        }

        /// <summary>
        /// Called when bar pointer <see cref="Child"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnChildPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BarPointer pointer)
            {
                if (pointer.Scale != null && pointer.Scale.BarPointersLayout.Contains(pointer.PointerView))
                {
                    pointer.Scale.BarPointerChildUpdate(oldValue, newValue);

                    if (newValue is View newChild)
                    {
                        SetInheritedBindingContext(newChild, pointer);

                        if (pointer.barPointerPath != null)
                            pointer.Scale.UpdateChild(newChild, pointer.barPointerPath.Bounds);
                    }
                }
               
                pointer.InvalidateDrawable();
            }
        }

#nullable disable
        /// <summary>
        /// Called when <see cref="GradientStops"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnGradientStopsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is BarPointer barPointer)
            {
                if (oldValue != null)
                {
                    if (oldValue is ObservableCollection<GaugeGradientStop> gradientStops)
                        gradientStops.CollectionChanged -= barPointer.GradientStops_CollectionChanged;
                }

                if (newValue != null)
                {
                    if (newValue is ObservableCollection<GaugeGradientStop> gradientStops)
                        gradientStops.CollectionChanged += barPointer.GradientStops_CollectionChanged;
                }

                barPointer.CreateGradient();
                barPointer.InvalidateDrawable();
            }
        }
#nullable enable

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
            }
            else
            {
                this.UpdatePointer();
            }
            this.CanAnimate = false;
        }

        private void CreateGradient()
        {
            if (this.Scale != null && this.GradientStops != null)
            {
                linearGradientBrush = this.Scale.GetLinearGradient(this.GradientStops, actualStartValue, actualEndValue);
            }
            else
                linearGradientBrush = null;
        }

        /// <summary>
        /// Called when <see cref="GradientStops"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void GradientStops_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.CreateGradient();
            this.InvalidateDrawable();
        }

        #endregion
    }
}
