using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Base class for <see cref="ShapePointer"/> and <see cref="ContentPointer"/>.
    /// It holds the common properties and logics for customizing <see cref="ShapePointer"/> and <see cref="ContentPointer"/>.
    /// </summary>
    public abstract class MarkerPointer : RadialPointer
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Offset"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Offset"/> bindable property.
        /// </value>
        public static readonly BindableProperty OffsetProperty =
            BindableProperty.Create(nameof(Offset), typeof(double), typeof(MarkerPointer), double.NaN, propertyChanged: OnMarkerPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="OffsetUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OffsetUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty OffsetUnitProperty =
            BindableProperty.Create(nameof(OffsetUnit), typeof(SizeUnit), typeof(MarkerPointer), SizeUnit.Pixel, propertyChanged: OnMarkerPropertyChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that specifies the marker position value either in logical pixel or radius factor.
        /// </summary>
        /// <value>
        /// It defines the position offset of the marker either pixel or factor. The default value is <c>double.NaN</c>.
        /// </value>
        public double Offset
        {
            get { return (double)this.GetValue(OffsetProperty); }
            set { this.SetValue(OffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates to calculate the marker pointer offset in logical pixel or radius factor.
        /// </summary>
        /// <value>
        /// One of the <see cref="SizeUnit"/> enumeration that specifies how the <see cref="Offset"/> value is considered.
        /// The default mode is <see cref="SizeUnit.Pixel"/>.
        /// </value>
        public SizeUnit OffsetUnit
        {
            get { return (SizeUnit)this.GetValue(OffsetUnitProperty); }
            set { this.SetValue(OffsetUnitProperty, value); }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when marker pointer <see cref="Offset"/>, and <see cref="OffsetUnit"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMarkerPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MarkerPointer markerPointer)
            {
                markerPointer.UpdatePointer();
                markerPointer.InvalidateDrawable();
            }
        }

        #endregion
    }
}
