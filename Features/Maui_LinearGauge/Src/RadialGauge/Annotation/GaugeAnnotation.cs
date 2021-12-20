  using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// <see cref="Gauges.RadialAxis"/> allows to add views such as text and image as an annotation to a specific point of interest in the radial gauge.
    /// <see cref="GaugeAnnotation"/> provides options to add any image, text or other views over a gauge with respect to angle or axis value. Display the current progress or pointer value inside the gauge using a text annotation.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// <gauge:SfRadialGauge>
    ///     <gauge:SfRadialGauge.Axes>
    ///         <gauge:RadialAxis>
    ///             <gauge:RadialAxis.Annotations>
    ///                 <gauge:GaugeAnnotation>
    ///                     <gauge:GaugeAnnotation.Content>
    ///                             <Label Text="Syncfusion"
    ///                                        TextColor="Black" />
    ///                     </gauge:GaugeAnnotation.Content>
    ///                 </gauge:GaugeAnnotation>
    ///             </gauge:RadialAxis.Annotations>
    ///         </gauge:RadialAxis>
    ///     </gauge:SfRadialGauge.Axes>
    /// </gauge:SfRadialGauge>
    /// ]]></code>
    /// </example>
    public class GaugeAnnotation : BindableObject
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="DirectionValue"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DirectionValue"/> bindable property.
        /// </value>
        public static readonly BindableProperty DirectionValueProperty =
            BindableProperty.Create(nameof(DirectionValue), typeof(double), typeof(GaugeAnnotation), 0d, propertyChanged: OnAnnotationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="DirectionUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DirectionUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty DirectionUnitProperty =
            BindableProperty.Create(nameof(DirectionUnit), typeof(AnnotationDirection), typeof(GaugeAnnotation), AnnotationDirection.Angle, propertyChanged: OnAnnotationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="PositionFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PositionFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty PositionFactorProperty =
            BindableProperty.Create(nameof(PositionFactor), typeof(double), typeof(GaugeAnnotation), 0d, propertyChanged: OnAnnotationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="HorizontalAlignment"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HorizontalAlignment"/> bindable property.
        /// </value>
        public static readonly BindableProperty HorizontalAlignmentProperty =
            BindableProperty.Create(nameof(HorizontalAlignment), typeof(GaugeAlignment), typeof(GaugeAnnotation), GaugeAlignment.Center, propertyChanged: OnAnnotationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="VerticalAlignment"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="VerticalAlignment"/> bindable property.
        /// </value>
        public static readonly BindableProperty VerticalAlignmentProperty =
            BindableProperty.Create(nameof(VerticalAlignment), typeof(GaugeAlignment), typeof(GaugeAnnotation), GaugeAlignment.Center, propertyChanged: OnAnnotationPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Content"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Content"/> bindable property.
        /// </value>
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(GaugeAnnotation), null, propertyChanged: OnContentPropertyChanged);

        #endregion

        #region Fields

        /// <summary>
        /// Holds radial axis instance.
        /// </summary>
        internal RadialAxis? RadialAxis;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to adjusts the annotation distance from center. You can specify value either in axis value or angle using the <see cref="DirectionUnit"/> property.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Annotations>
        ///                 <gauge:GaugeAnnotation DirectionValue="90" PositionFactor="0.3">
        ///                     <gauge:GaugeAnnotation.Content>
        ///                         <Label Text="Syncfusion"
        ///                                        TextColor="Black" />
        ///                     </gauge:GaugeAnnotation.Content>
        ///                 </gauge:GaugeAnnotation>
        ///             </gauge:RadialAxis.Annotations>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double DirectionValue
        {
            get { return (double)this.GetValue(DirectionValueProperty); }
            set { this.SetValue(DirectionValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the direction of annotation calculated based on axis value or angle.
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the direction unit of annotation in the radial gauge.
        /// The default is <see cref="AnnotationDirection.Angle"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Annotations>
        ///                 <gauge:GaugeAnnotation DirectionValue="90" PositionFactor="0.3" DirectionUnit="AxisValue">
        ///                     <gauge:GaugeAnnotation.Content>
        ///                         <Label Text="Syncfusion"
        ///                                        TextColor="Black" />
        ///                     </gauge:GaugeAnnotation.Content>
        ///                 </gauge:GaugeAnnotation>
        ///             </gauge:RadialAxis.Annotations>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public AnnotationDirection DirectionUnit
        {
            get { return (AnnotationDirection)this.GetValue(DirectionUnitProperty); }
            set { this.SetValue(DirectionUnitProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the position of annotation in radius factor. PositionFactor value of 0 is starting from the center and 1 is ending at the edge of the radius.
        /// </summary>
        /// <example>
        /// <see cref="PositionFactor"/> value of 0 is starting from the center and 1 is ending at the edge of the radius. <see cref="PositionFactor"/> must be between 0 to 1.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Annotations>
        ///                 <gauge:GaugeAnnotation DirectionValue="90" PositionFactor="0.5">
        ///                     <gauge:GaugeAnnotation.Content>
        ///                         <Label Text="Syncfusion"
        ///                                        TextColor="Black" />
        ///                     </gauge:GaugeAnnotation.Content>
        ///                 </gauge:GaugeAnnotation>
        ///             </gauge:RadialAxis.Annotations>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double PositionFactor
        {
            get { return (double)this.GetValue(PositionFactorProperty); }
            set { this.SetValue(PositionFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the horizontal alignment of annotation.
        /// </summary>
        /// <value>
        /// The default is <see cref="GaugeAlignment.Center"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Annotations>
        ///                 <gauge:GaugeAnnotation HorizontalAlignment="Start" >
        ///                     <gauge:GaugeAnnotation.Content>
        ///                         <Label Text="Syncfusion"
        ///                                        TextColor="Black" />
        ///                     </gauge:GaugeAnnotation.Content>
        ///                 </gauge:GaugeAnnotation>
        ///             </gauge:RadialAxis.Annotations>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public GaugeAlignment HorizontalAlignment
        {
            get { return (GaugeAlignment)this.GetValue(HorizontalAlignmentProperty); }
            set { this.SetValue(HorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the vertical alignment of annotation.
        /// </summary>
        /// <value>
        /// The default is <see cref="GaugeAlignment.Center"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Annotations>
        ///                 <gauge:GaugeAnnotation VerticalAlignment="Start" >
        ///                     <gauge:GaugeAnnotation.Content>
        ///                         <Label Text="Syncfusion"
        ///                                        TextColor="Black" />
        ///                     </gauge:GaugeAnnotation.Content>
        ///                 </gauge:GaugeAnnotation>
        ///             </gauge:RadialAxis.Annotations>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public GaugeAlignment VerticalAlignment
        {
            get { return (GaugeAlignment)this.GetValue(VerticalAlignmentProperty); }
            set { this.SetValue(VerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that represents annotation's content.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Annotations>
        ///                 <gauge:GaugeAnnotation>
        ///                     <gauge:GaugeAnnotation.Content>
        ///                         <Label Text="Syncfusion"
        ///                                        TextColor="Black" />
        ///                     </gauge:GaugeAnnotation.Content>
        ///                 </gauge:GaugeAnnotation>
        ///             </gauge:RadialAxis.Annotations>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public View Content
        {
            get { return (View)this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Method used to create annotation.
        /// </summary>
        internal void CreateAnnotation()
        {
            if (this.RadialAxis == null || this.RadialAxis.AvailableSize.IsZero || this.Content == null)
            {
                return;
            }

            double actualPosition = this.RadialAxis.CalculateActualSize(1 - this.PositionFactor, SizeUnit.Factor, true);
            double actualDirectionValue;
            Point vector = new Point();
            switch (this.DirectionUnit)
            {
                case AnnotationDirection.Angle:
                    actualDirectionValue = this.DirectionValue % 360;
                    if (this.RadialAxis.IsInversed)
                    {
                        actualDirectionValue = 360 - actualDirectionValue;
                    }

                    vector = Utility.AngleToVector(actualDirectionValue);
                    break;
                case AnnotationDirection.AxisValue:
                    actualDirectionValue = Math.Clamp(this.DirectionValue, this.RadialAxis.ActualMinimum, this.RadialAxis.ActualMaximum);
                    vector = this.RadialAxis.ValueToPoint(actualDirectionValue);
                    break;
            }

            double radius = (double.IsNaN(this.PositionFactor) ? this.RadialAxis.Radius : actualPosition) * this.RadialAxis.RadiusFactor;
            Point point = new Point(this.RadialAxis.Center.X + (radius * vector.X), this.RadialAxis.Center.Y + (radius * vector.Y));

            Size desiredSize = this.Content.ComputeDesiredSize(this.RadialAxis.AvailableSize.Width, this.RadialAxis.AvailableSize.Height);

            switch (this.HorizontalAlignment)
            {
                case GaugeAlignment.Center:
                    point.X -= desiredSize.Width / 2;
                    break;
                case GaugeAlignment.Start:
                    point.X -= desiredSize.Width;
                    break;
            }

            switch (this.VerticalAlignment)
            {
                case GaugeAlignment.Center:
                    point.Y -= desiredSize.Height / 2;
                    break;
                case GaugeAlignment.Start:
                    point.Y -= desiredSize.Height;
                    break;
            }

            AbsoluteLayout.SetLayoutBounds(this.Content, new Rectangle(point, desiredSize));
        }

        /// <summary>
        /// Method used to un hook event for annotation's content. 
        /// </summary>
        internal void UnHookMeasureInvalidated()
        {
            this.Content.MeasureInvalidated -= this.Content_MeasureInvalidated;
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when <see cref="DirectionValue"/> or <see cref="DirectionUnit"/> or <see cref="PositionFactor"/> or <see cref="HorizontalAlignment"/> or <see cref="VerticalAlignment"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value of bindable property changed.</param>
        /// <param name="newValue">The new value of bindable property changed.</param>
        private static void OnAnnotationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is GaugeAnnotation gaugeAnnotation)
            {
                gaugeAnnotation.CreateAnnotation();
            }
        }

        /// <summary>
        /// Called when <see cref="Content"/> property changed.
        /// </summary>
        /// <param name="bindable">The bindable object.</param>
        /// <param name="oldValue">The old value of bindable property changed.</param>
        /// <param name="newValue">The new value of bindable property changed.</param>
        private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is GaugeAnnotation gaugeAnnotation)
            {
                if(oldValue is View oldContent)
                {
                    if (gaugeAnnotation.RadialAxis != null && gaugeAnnotation.RadialAxis.AnnotationsLayout.Children.Contains(oldContent))
                    {
                        gaugeAnnotation.RadialAxis.AnnotationsLayout.Children.Remove(oldContent);
                    }
                    gaugeAnnotation.UnHookMeasureInvalidated();
                }

                if (newValue is View newContent)
                {
                    if (gaugeAnnotation.RadialAxis != null && !gaugeAnnotation.RadialAxis.AnnotationsLayout.Children.Contains(newContent))
                    {
                        gaugeAnnotation.RadialAxis.AnnotationsLayout.Children.Add(newContent);
                    }
                    newContent.MeasureInvalidated += gaugeAnnotation.Content_MeasureInvalidated;

                    gaugeAnnotation.CreateAnnotation();
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Called when content's size changed. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Content_MeasureInvalidated(object? sender, EventArgs e)
        {
            this.CreateAnnotation();
        }

        #endregion
    }
}
