using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Base class for <see cref="ShapePointer"/>.
    /// It holds the common properties and logics for customizing <see cref="ShapePointer"/> and content pointer.
    /// </summary>
    public abstract class LinearMarkerPointer : LinearPointer
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="OffsetPoint"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OffsetPoint"/> bindable property.
        /// </value>
        public static readonly BindableProperty OffsetPointProperty = BindableProperty.Create(nameof(OffsetPoint), typeof(Point), 
            typeof(LinearMarkerPointer), null, propertyChanged: OnMarkerPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalAlignment"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HorizontalAlignment"/> bindable property.
        /// </value>
        public static readonly BindableProperty HorizontalAlignmentProperty = BindableProperty.Create(nameof(HorizontalAlignment), 
            typeof(GaugeAlignment), typeof(LinearMarkerPointer), GaugeAlignment.Center, propertyChanged: OnMarkerPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalAlignment"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="VerticalAlignment"/> bindable property.
        /// </value>
        public static readonly BindableProperty VerticalAlignmentProperty = BindableProperty.Create(nameof(VerticalAlignment), 
            typeof(GaugeAlignment), typeof(LinearMarkerPointer), GaugeAlignment.Center, propertyChanged: OnMarkerPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AllowClip"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AllowClip"/> bindable property.
        /// </value>
        public static readonly BindableProperty AllowClipProperty =
            BindableProperty.Create(nameof(AllowClip), typeof(bool), typeof(LinearMarkerPointer), false, propertyChanged: OnMarkerPropertyChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that specifies the marker position value. 
        /// </summary>
        /// <value>
        /// Its default value is <c>0,0</c>.
        /// </value>
        /// <remarks>
        /// Offset point x defines the left or right distance from the pointer value. 
        /// Offset point y defines the bottom or top distance from the pointer value. 
        /// </remarks>
        public Point OffsetPoint
        {
            get { return (Point)this.GetValue(OffsetPointProperty); }
            set { this.SetValue(OffsetPointProperty, value); }
        }

        /// <summary>
        /// Gets or sets the horizontal placement (left, center or right) of the marker pointer relative to its position. 
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the horizontal position of marker in the linear gauge.
        /// The default is <see cref="GaugeAlignment.Center"/>.
        /// </value>
        public GaugeAlignment HorizontalAlignment
        {
            get { return (GaugeAlignment)this.GetValue(HorizontalAlignmentProperty); }
            set { this.SetValue(HorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical placement (top, center or bottom) of the marker pointer relative to its position. 
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the vertical position of marker in the linear gauge.
        /// The default is <see cref="GaugeAlignment.Center"/>.
        /// </value>
        public GaugeAlignment VerticalAlignment
        {
            get { return (GaugeAlignment)this.GetValue(VerticalAlignmentProperty); }
            set { this.SetValue(VerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to clip markers in scale edge or not. 
        /// </summary>
        /// <value>
        /// <b>The default value is false</b>.
        /// </value>
        public bool AllowClip
        {
            get { return (bool)this.GetValue(AllowClipProperty); }
            set { this.SetValue(AllowClipProperty, value); }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when marker pointer <see cref="OffsetPoint"/>, <see cref="HorizontalAlignment"/>, or <see cref="VerticalAlignment"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMarkerPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is LinearMarkerPointer linearMarkerPointer && linearMarkerPointer.Scale != null)
            {
                linearMarkerPointer.Scale.ScaleInvalidateMeasureOverride();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// To get the pointer position.
        /// </summary>
        /// <param name="halfWidth">The pointer half width.</param>
        /// <param name="halfHeight">The pointer half height.</param>
        /// <param name="x">Current x position.</param>
        /// <param name="y">Current y position.</param>
        internal void GetPointerPosition(double halfWidth, double halfHeight, ref double x, ref double y)
        {
            if (this.Scale != null)
            {
                GaugeAlignment verticalPosition = this.GetActualViewAlignment(this.Scale.Orientation == GaugeOrientation.Horizontal ?
                    this.VerticalAlignment : this.HorizontalAlignment);

                GaugeAlignment horizontalPosition = this.GetActualViewAlignment(this.Scale.Orientation == GaugeOrientation.Horizontal ?
                    this.HorizontalAlignment : this.VerticalAlignment);
                switch (verticalPosition)
                {
                    case GaugeAlignment.Start:
                        y += halfHeight;
                        break;
                    case GaugeAlignment.End:
                        y -= halfHeight;
                        break;
                }

                switch (horizontalPosition)
                {
                    case GaugeAlignment.Start:
                        x += halfWidth;
                        break;
                    case GaugeAlignment.End:
                        x -= halfWidth;
                        break;
                }

                if (this.Scale.Orientation == GaugeOrientation.Horizontal)
                {
                    x += this.OffsetPoint.X;
                    if (this.Scale.IsMirrored)
                    {
                        y -= this.OffsetPoint.Y;
                    }
                    else
                    {
                        y += this.OffsetPoint.Y;
                    }
                }
                else
                {
                    x += this.OffsetPoint.Y;
                    if (this.Scale.IsMirrored)
                    {
                        y -= this.OffsetPoint.X;
                    }
                    else
                    {
                        y += this.OffsetPoint.X;
                    }
                }
            }
        }

        /// <summary>
        /// To calculate marker pointer margins when it have negative offset.
        /// </summary>
        /// <param name="outsidePointerSize">The outside positioned pointer height.</param>
        /// <param name="insidePointerSize">The inside positioned pointer height.</param>
        /// <param name="actualAxisLineThickness">The actual axis line thickness.</param>
        /// <param name="pointerOffset">The pointer offset.</param>
        /// <param name="pointerPosition">The pointer current position.</param>
        /// <param name="markerSize">The pointer size.</param>
        internal void GetMarkerSizeWithNegativeOffset(ref double outsidePointerSize, ref double insidePointerSize,
            double actualAxisLineThickness, double pointerOffset, GaugeAlignment pointerPosition, double markerSize)
        {
            switch (pointerPosition)
            {
                case GaugeAlignment.Start:
                    if (actualAxisLineThickness / 2 < Math.Abs(pointerOffset))
                    {
                        outsidePointerSize = Math.Max(Math.Abs(pointerOffset) - (actualAxisLineThickness / 2), outsidePointerSize);
                        if (markerSize > actualAxisLineThickness)
                        {
                            insidePointerSize = Math.Max(Math.Max(markerSize - Math.Abs(pointerOffset) - (actualAxisLineThickness / 2), 0), insidePointerSize);
                        }
                    }
                    else
                    {
                        insidePointerSize = Math.Max(markerSize - (actualAxisLineThickness / 2) + pointerOffset, insidePointerSize);
                    }

                    break;
                case GaugeAlignment.Center:
                    outsidePointerSize = Math.Max(((markerSize - actualAxisLineThickness) / 2) + Math.Abs(pointerOffset), outsidePointerSize);
                    if (markerSize > actualAxisLineThickness)
                    {
                        if (Math.Abs(pointerOffset) < (markerSize - actualAxisLineThickness))
                        {
                            insidePointerSize = Math.Max(((markerSize - actualAxisLineThickness) / 2) + pointerOffset, insidePointerSize);
                        }
                    }

                    break;
                case GaugeAlignment.End:
                    outsidePointerSize = Math.Max(markerSize - (actualAxisLineThickness / 2) + Math.Abs(pointerOffset), outsidePointerSize);
                    break;
            }
        }

        /// <summary>
        /// To calculate marker pointer margins when it have positive offset.
        /// </summary>
        /// <param name="outsidePointerSize">The outside positioned pointer height.</param>
        /// <param name="insidePointerSize">The inside positioned pointer height.</param>
        /// <param name="actualAxisLineThickness">The actual axis line thickness.</param>
        /// <param name="pointerOffset">The pointer offset.</param>
        /// <param name="pointerPosition">The pointer current position.</param>
        /// <param name="markerSize">The pointer size.</param>
        internal void GetMarkerSizeWithPositiveOffset(ref double outsidePointerSize, ref double insidePointerSize,
            double actualAxisLineThickness, double pointerOffset, GaugeAlignment pointerPosition, double markerSize)
        {
            switch (pointerPosition)
            {
                case GaugeAlignment.Start:
                    insidePointerSize = Math.Max(markerSize - (actualAxisLineThickness / 2) + pointerOffset, insidePointerSize);
                    break;
                case GaugeAlignment.Center:
                    insidePointerSize = Math.Max(((markerSize - actualAxisLineThickness) / 2) + pointerOffset, insidePointerSize);
                    if (markerSize > actualAxisLineThickness)
                    {
                        if (pointerOffset < (markerSize - actualAxisLineThickness))
                        {
                            outsidePointerSize = Math.Max(((markerSize - actualAxisLineThickness) / 2) - pointerOffset, outsidePointerSize);
                        }
                    }

                    break;
                case GaugeAlignment.End:
                    if (actualAxisLineThickness / 2 < pointerOffset)
                    {
                        insidePointerSize = Math.Max(pointerOffset - (actualAxisLineThickness / 2), insidePointerSize);
                        if (markerSize > actualAxisLineThickness)
                        {
                            outsidePointerSize = Math.Max(Math.Max(markerSize - pointerOffset - (actualAxisLineThickness / 2), 0), outsidePointerSize);
                        }
                    }
                    else
                    {
                        outsidePointerSize = Math.Max(markerSize - (actualAxisLineThickness / 2) - pointerOffset, outsidePointerSize);
                    }

                    break;
            }
        }

        /// <summary>
        /// To calculate marker pointer margins when it does not have offset.
        /// </summary>
        /// <param name="outsidePointerSize">The outside positioned pointer height.</param>
        /// <param name="insidePointerSize">The inside positioned pointer height.</param>
        /// <param name="actualAxisLineThickness">The actual axis line thickness.</param>
        /// <param name="pointerPosition">The pointer current position.</param>
        /// <param name="markerSize">The pointer size.</param>
        internal void GetMarkerSize(ref double outsidePointerSize, ref double insidePointerSize,
            double actualAxisLineThickness, GaugeAlignment pointerPosition, double markerSize)
        {
            switch (pointerPosition)
            {
                case GaugeAlignment.Start:
                    insidePointerSize = Math.Max(Math.Max(0, markerSize - (actualAxisLineThickness / 2)), insidePointerSize);
                    break;
                case GaugeAlignment.Center:
                    outsidePointerSize = Math.Max(Math.Max(0, (markerSize / 2) - (actualAxisLineThickness / 2)), outsidePointerSize);
                    insidePointerSize = Math.Max(Math.Max(0, (markerSize / 2) - (actualAxisLineThickness / 2)), insidePointerSize);
                    break;
                case GaugeAlignment.End:
                    outsidePointerSize = Math.Max(Math.Max(0, markerSize - (actualAxisLineThickness / 2)), outsidePointerSize);
                    break;
            }
        }

        /// <summary>
        /// To get actual view alignment based on <see cref="SfLinearGauge.IsMirrored"/> property value.
        /// </summary>
        /// <param name="viewAlignment">The current view alignment.</param>
        /// <returns>Actual view alignment based on <see cref="SfLinearGauge.IsMirrored"/> property value.</returns>
        private GaugeAlignment GetActualViewAlignment(GaugeAlignment viewAlignment)
        {
            if (this.Scale != null && this.Scale.IsMirrored)
            {
                switch (viewAlignment)
                {
                    case GaugeAlignment.Start:
                        return GaugeAlignment.End;
                    case GaugeAlignment.End:
                        return GaugeAlignment.Start;
                }
            }

            return viewAlignment;
        }

        #endregion
    }
}
