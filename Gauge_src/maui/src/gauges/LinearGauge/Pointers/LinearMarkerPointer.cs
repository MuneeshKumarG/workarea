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
        /// Identifies the <see cref="Alignment"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Alignment"/> bindable property.
        /// </value>
        public static readonly BindableProperty AlignmentProperty = BindableProperty.Create(nameof(Alignment), 
            typeof(GaugeAlignment), typeof(LinearMarkerPointer), GaugeAlignment.Center, propertyChanged: OnMarkerPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Position"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Position"/> bindable property.
        /// </value>
        public static readonly BindableProperty PositionProperty = BindableProperty.Create(nameof(Position), 
            typeof(GaugeElementPosition), typeof(LinearMarkerPointer), GaugeElementPosition.Outside, propertyChanged: OnMarkerPropertyChanged);

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
        /// Gets or sets the placement (start, center or end) of the marker pointer relative to its position. 
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the alignment of marker in the linear gauge.
        /// The default is <see cref="GaugeAlignment.Center"/>.
        /// </value>
        public GaugeAlignment Alignment
        {
            get { return (GaugeAlignment)this.GetValue(AlignmentProperty); }
            set { this.SetValue(AlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the placement (top, center or bottom) of the marker pointer relative to scale. 
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the vertical position of marker in the linear gauge.
        /// The default is <see cref="GaugeElementPosition.Cross"/>.
        /// </value>
        public GaugeElementPosition Position
        {
            get { return (GaugeElementPosition)this.GetValue(PositionProperty); }
            set { this.SetValue(PositionProperty, value); }
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
                GaugeElementPosition position = this.GetActualViewAlignment(this.Position);

                switch (position)
                {
                    case GaugeElementPosition.Inside:
                        y += halfHeight;
                        break;
                    case GaugeElementPosition.Outside:
                        y -= halfHeight;
                        break;
                }

                switch (Alignment)
                {
                    case GaugeAlignment.Start:
                        x -= halfWidth;
                        break;
                    case GaugeAlignment.End:
                        x += halfWidth;
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
        /// <param name="markerSize">The pointer size.</param>
        internal void GetMarkerSizeWithNegativeOffset(ref double outsidePointerSize, ref double insidePointerSize,
            double actualAxisLineThickness, double pointerOffset, double markerSize)
        {
            double positiveOffset = Math.Abs(pointerOffset);
            switch (Position)
            {
                case GaugeElementPosition.Inside:
                    if (actualAxisLineThickness < positiveOffset)
                    {
                        outsidePointerSize = Math.Max(positiveOffset - actualAxisLineThickness, outsidePointerSize);
                        if (markerSize > actualAxisLineThickness)
                        {
                            insidePointerSize = Math.Max(Math.Max(markerSize - positiveOffset, 0), insidePointerSize);
                        }
                    }
                    else
                    {
                        insidePointerSize = Math.Max(markerSize - pointerOffset, insidePointerSize);
                    }

                    break;
                case GaugeElementPosition.Cross:
                    outsidePointerSize = Math.Max(((markerSize - actualAxisLineThickness) / 2) + positiveOffset, outsidePointerSize);
                    if (markerSize > actualAxisLineThickness)
                    {
                        if (positiveOffset < (markerSize - actualAxisLineThickness))
                        {
                            insidePointerSize = Math.Max(((markerSize - actualAxisLineThickness) / 2) + pointerOffset, insidePointerSize);
                        }
                    }

                    break;
                case GaugeElementPosition.Outside:
                    outsidePointerSize = Math.Max(markerSize + positiveOffset, outsidePointerSize);
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
        /// <param name="markerSize">The pointer size.</param>
        internal void GetMarkerSizeWithPositiveOffset(ref double outsidePointerSize, ref double insidePointerSize,
            double actualAxisLineThickness, double pointerOffset, double markerSize)
        {
            switch (Position)
            {
                case GaugeElementPosition.Inside:
                    insidePointerSize = Math.Max(markerSize + pointerOffset, insidePointerSize);
                    break;
                case GaugeElementPosition.Cross:
                    insidePointerSize = Math.Max(((markerSize - actualAxisLineThickness) / 2) + pointerOffset, insidePointerSize);
                    if (markerSize > actualAxisLineThickness)
                    {
                        if (pointerOffset < (markerSize - actualAxisLineThickness))
                        {
                            outsidePointerSize = Math.Max(((markerSize - actualAxisLineThickness) / 2) - pointerOffset, outsidePointerSize);
                        }
                    }

                    break;
                case GaugeElementPosition.Outside:
                    if (actualAxisLineThickness < pointerOffset)
                    {
                        insidePointerSize = Math.Max(pointerOffset - actualAxisLineThickness, insidePointerSize);
                        if (markerSize > actualAxisLineThickness)
                        {
                            outsidePointerSize = Math.Max(Math.Max(markerSize - pointerOffset, 0), outsidePointerSize);
                        }
                    }
                    else
                    {
                        outsidePointerSize = Math.Max(markerSize - pointerOffset, outsidePointerSize);
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
        /// <param name="markerSize">The pointer size.</param>
        internal void GetMarkerSize(ref double outsidePointerSize, ref double insidePointerSize,
            double actualAxisLineThickness, double markerSize)
        {
            switch (Position)
            {
                case GaugeElementPosition.Inside:
                    insidePointerSize = Math.Max(Math.Max(0, markerSize), insidePointerSize);
                    break;
                case GaugeElementPosition.Cross:
                    outsidePointerSize = Math.Max(Math.Max(0, (markerSize / 2) - (actualAxisLineThickness / 2)), outsidePointerSize);
                    insidePointerSize = Math.Max(Math.Max(0, (markerSize / 2) - (actualAxisLineThickness / 2)), insidePointerSize);
                    break;
                case GaugeElementPosition.Outside:
                    outsidePointerSize = Math.Max(Math.Max(0, markerSize), outsidePointerSize);
                    break;
            }
        }

        /// <summary>
        /// To get actual view alignment based on <see cref="SfLinearGauge.IsMirrored"/> property value.
        /// </summary>
        /// <param name="position">The current view alignment.</param>
        /// <returns>Actual view alignment based on <see cref="SfLinearGauge.IsMirrored"/> property value.</returns>
        private GaugeElementPosition GetActualViewAlignment(GaugeElementPosition position)
        {
            if (this.Scale != null && this.Scale.IsMirrored)
            {
                switch (position)
                {
                    case GaugeElementPosition.Outside:
                        return GaugeElementPosition.Inside;
                    case GaugeElementPosition.Inside:
                        return GaugeElementPosition.Outside;
                }
            }

            return position;
        }

        #endregion
    }
}
