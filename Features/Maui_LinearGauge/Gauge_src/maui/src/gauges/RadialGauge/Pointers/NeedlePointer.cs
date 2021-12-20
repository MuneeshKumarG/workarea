using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Create the pointer to indicate the value with needle or arrow shape.
    /// <see cref="NeedlePointer"/> contains three parts, namely needle, knob, and tail and that can be placed on a gauge to mark the values.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// <gauge:SfRadialGauge>
    ///     <gauge:SfRadialGauge.Axes>
    ///         <gauge:RadialAxis>
    ///             <gauge:RadialAxis.Pointers>
    ///                 <gauge:NeedlePointer Value="50" />
    ///             </gauge:RadialAxis.Pointers>
    ///         </gauge:RadialAxis>
    ///     </gauge:SfRadialGauge.Axes>
    /// </gauge:SfRadialGauge>
    /// ]]></code>
    /// </example>
    public class NeedlePointer : RadialPointer
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="KnobRadius"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="KnobRadius"/> bindable property.
        /// </value>
        public static readonly BindableProperty KnobRadiusProperty =
            BindableProperty.Create(nameof(KnobRadius), typeof(double), typeof(NeedlePointer), 0.07d, propertyChanged: OnKnobPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="KnobStrokeThickness"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="KnobStrokeThickness"/> bindable property.
        /// </value>
        public static readonly BindableProperty KnobStrokeThicknessProperty =
            BindableProperty.Create(nameof(KnobStrokeThickness), typeof(double), typeof(NeedlePointer), 0d, propertyChanged: OnKnobPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="KnobFill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="KnobFill"/> bindable property.
        /// </value>
        public static readonly BindableProperty KnobFillProperty =
            BindableProperty.Create(nameof(KnobFill), typeof(Brush), typeof(NeedlePointer), new SolidColorBrush(Color.FromRgb(73, 89, 99)), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="KnobStroke"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="KnobStroke"/> bindable property.
        /// </value>
        public static readonly BindableProperty KnobStrokeProperty =
            BindableProperty.Create(nameof(KnobStroke), typeof(Color), typeof(NeedlePointer), Color.FromRgb(73, 89, 99), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="KnobSizeUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="KnobSizeUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty KnobSizeUnitProperty =
            BindableProperty.Create(nameof(KnobSizeUnit), typeof(SizeUnit), typeof(NeedlePointer), SizeUnit.Factor, propertyChanged: OnKnobPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="NeedleLength"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NeedleLength"/> bindable property.
        /// </value>
        public static readonly BindableProperty NeedleLengthProperty =
            BindableProperty.Create(nameof(NeedleLength), typeof(double), typeof(NeedlePointer), 0.75d, propertyChanged: OnNeedlePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="NeedleLengthUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NeedleLengthUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty NeedleLengthUnitProperty =
            BindableProperty.Create(nameof(NeedleLengthUnit), typeof(SizeUnit), typeof(NeedlePointer), SizeUnit.Factor, propertyChanged: OnNeedlePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="NeedleStartWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NeedleStartWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty NeedleStartWidthProperty =
            BindableProperty.Create(nameof(NeedleStartWidth), typeof(double), typeof(NeedlePointer), 2d, propertyChanged: OnNeedlePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="NeedleEndWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NeedleEndWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty NeedleEndWidthProperty =
            BindableProperty.Create(nameof(NeedleEndWidth), typeof(double), typeof(NeedlePointer), 8d, propertyChanged: OnNeedlePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="NeedleFill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="NeedleFill"/> bindable property.
        /// </value>
        public static readonly BindableProperty NeedleFillProperty =
            BindableProperty.Create(nameof(NeedleFill), typeof(Brush), typeof(NeedlePointer), new SolidColorBrush(Color.FromRgb(73, 89, 99)), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TailLength"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TailLength"/> bindable property.
        /// </value>
        public static readonly BindableProperty TailLengthProperty =
            BindableProperty.Create(nameof(TailLength), typeof(double), typeof(NeedlePointer), 0d, propertyChanged: OnTailPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TailFill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TailFill"/> bindable property.
        /// </value>
        public static readonly BindableProperty TailFillProperty =
            BindableProperty.Create(nameof(TailFill), typeof(Brush), typeof(NeedlePointer), new SolidColorBrush(Color.FromRgb(73, 89, 99)), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TailLengthUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TailLengthUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty TailLengthUnitProperty =
            BindableProperty.Create(nameof(TailLengthUnit), typeof(SizeUnit), typeof(NeedlePointer), SizeUnit.Factor, propertyChanged: OnTailPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TailWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TailWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty TailWidthProperty =
            BindableProperty.Create(nameof(TailWidth), typeof(double), typeof(NeedlePointer), 8d, propertyChanged: OnTailPropertyChanged);

        #endregion

        #region Fields

        /// <summary>
        /// Backing field to store needle path.
        /// </summary>
        private PathF? needlePath;

        private RectangleF? tailRectangle;

        private float needlePathRotationAngle;

        private double knobRadius, knobActualBorderThickness;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value that specifies the knob radius size. 
        /// </summary>
        /// <value>
        /// It defines the radius of the knob either in pixel or factor. The default value is <c>0.07d</c>.
        /// </value>
        /// <example>
        /// If <see cref="KnobSizeUnit"/> is <see cref="SizeUnit.Factor"/>, value will be given from 0 to 1. Here knob radius size is calculated by <see cref="KnobRadius"/> * axis radius value.
        /// Example: <see cref="KnobRadius"/> value is 0.2 and axis radius is 100, knob radius is 20(0.2 * 100) logical pixels. if <see cref="KnobSizeUnit"/> is <see cref="SizeUnit.Pixel"/>, defined value is set to the knob radius.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      KnobRadius="0.08" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double KnobRadius
        {
            get { return (double)this.GetValue(KnobRadiusProperty); }
            set { this.SetValue(KnobRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets the width of the knob stroke outline.
        /// </summary>
        /// <value>
        /// It defines the thickness of the knob stroke. The default value is <c>0</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      KnobRadius="0.02"
        ///                                      KnobStrokeThickness="2" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double KnobStrokeThickness
        {
            get { return (double)this.GetValue(KnobStrokeThicknessProperty); }
            set { this.SetValue(KnobStrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Brush that paints the interior area of the knob.
        /// </summary>
        /// <value>
        /// A <c>Brush</c> that specifies how the knob is painted.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      KnobRadius="0.02"
        ///                                      KnobFill="Red" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public Brush KnobFill
        {
            get { return (Brush)this.GetValue(KnobFillProperty); }
            set { this.SetValue(KnobFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the knob border color.
        /// </summary>
        /// <value>
        /// A <c>Color</c> that specifies how the knob border is painted.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      KnobRadius="0.02"
        ///                                      KnobFill="Red"
        ///                                      KnobStrokeThickness="2"
        ///                                      KnobStroke="Blue" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public Color KnobStroke
        {
            get { return (Color)this.GetValue(KnobStrokeProperty); }
            set { this.SetValue(KnobStrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates to calculate the knob radius in logical pixel or radius factor.
        /// </summary>
        /// <value>
        /// One of the <see cref="SizeUnit"/> enumeration that specifies how the <see cref="KnobRadius"/> value is considered.
        /// The default mode is <see cref="SizeUnit.Factor"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      KnobRadius="0.02"
        ///                                      KnobFill="Red"
        ///                                      KnobStrokeThickness="2"
        ///                                      KnobStroke="Blue"
        ///                                      KnobSizeUnit="Factor" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public SizeUnit KnobSizeUnit
        {
            get { return (SizeUnit)this.GetValue(KnobSizeUnitProperty); }
            set { this.SetValue(KnobSizeUnitProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the length of the needle pointer.
        /// You can specify value either in logical pixel or radius factor using the <see cref="NeedleLengthUnit"/> property.
        /// </summary>
        /// <value>
        /// It defines the length of the needle either in pixel or factor. The default value is <c>0.75</c>.
        /// </value>
        /// <example>
        /// If <see cref="NeedleLengthUnit"/> is <see cref="SizeUnit.Factor"/>, value will be given from 0 to 1. Here needle length is calculated by <see cref="NeedleLength"/> * axis radius value.
        /// Example: <see cref="NeedleLength"/> value is 0.2 and axis radius is 100, needle length is 20(0.2 * 100) logical pixels. if <see cref="NeedleLengthUnit"/> is <see cref="SizeUnit.Pixel"/>, defined value is set to the needle length.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      NeedleLength="0.7" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double NeedleLength
        {
            get { return (double)this.GetValue(NeedleLengthProperty); }
            set { this.SetValue(NeedleLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies to calculate the needle pointer length either in logical pixel or radius factor.
        /// </summary>
        /// <value>
        /// One of the <see cref="SizeUnit"/> enumeration that specifies how the <see cref="NeedleLength"/> value is considered.
        /// The default mode is <see cref="SizeUnit.Factor"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      NeedleLength="0.7"
        ///                                      NeedleLengthUnit="Factor" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public SizeUnit NeedleLengthUnit
        {
            get { return (SizeUnit)this.GetValue(NeedleLengthUnitProperty); }
            set { this.SetValue(NeedleLengthUnitProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the start width of the needle pointer.
        /// </summary>
        /// <value>
        /// It defines the width of the needle start. The default value is <c>2</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      NeedleLength="0.7"
        ///                                      NeedleLengthUnit="Factor"
        ///                                      NeedleStartWidth="10" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double NeedleStartWidth
        {
            get { return (double)this.GetValue(NeedleStartWidthProperty); }
            set { this.SetValue(NeedleStartWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the end width of the needle pointer.
        /// </summary>
        /// <value>
        /// It defines the width of the needle end. The default value is <c>8</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      NeedleLength="0.7"
        ///                                      NeedleLengthUnit="Factor"
        ///                                      NeedleStartWidth="10"
        ///                                      NeedleEndWidth="10" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double NeedleEndWidth
        {
            get { return (double)this.GetValue(NeedleEndWidthProperty); }
            set { this.SetValue(NeedleEndWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Brush that paints the interior area of the needle shape.
        /// </summary>
        /// <value>
        /// A <c>Brush</c> that specifies how the needle is painted.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      NeedleLength="0.7"
        ///                                      NeedleLengthUnit="Factor"
        ///                                      NeedleStartWidth="10"
        ///                                      NeedleEndWidth="10"
        ///                                      NeedleFill="Red" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public Brush NeedleFill
        {
            get { return (Brush)this.GetValue(NeedleFillProperty); }
            set { this.SetValue(NeedleFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the needle pointer tail length.
        /// You can specify value either in logical pixel or radius factor using the <see cref="TailLengthUnit"/> property.
        /// </summary>
        /// <value>
        /// It defines the tail length. The default value is <c>0</c>.
        /// </value>
        /// <example>
        /// If <see cref="TailLengthUnit"/> is <see cref="SizeUnit.Factor"/>, value will be given from 0 to 1. Here tail length is calculated by <see cref="TailLength"/> * axis radius value.
        /// Example: <see cref="TailLength"/> value is 0.2 and axis radius is 100, tail length is 20(0.2 * 100) logical pixels. if <see cref="TailLengthUnit"/> is <see cref="SizeUnit.Pixel"/>, defined value is set to the tail length.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      TailLength="0.2" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double TailLength
        {
            get { return (double)this.GetValue(TailLengthProperty); }
            set { this.SetValue(TailLengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Brush that paints the interior area of the tail shape.
        /// </summary>
        /// <value>
        /// A <c>Brush</c> that specifies how the tail is painted.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      TailLength="0.1"
        ///                                      TailFill="Red" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public Brush TailFill
        {
            get { return (Brush)this.GetValue(TailFillProperty); }
            set { this.SetValue(TailFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies to calculate the needle pointer tail length either in logical pixel or radius factor.
        /// </summary>
        /// <value>
        /// One of the <see cref="SizeUnit"/> enumeration that specifies how the <see cref="TailWidth"/> value is considered.
        /// The default mode is <see cref="SizeUnit.Factor"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      TailLength="0.1"
        ///                                      TailFill="Red"
        ///                                      TailLengthUnit="Factor" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public SizeUnit TailLengthUnit
        {
            get { return (SizeUnit)this.GetValue(TailLengthUnitProperty); }
            set { this.SetValue(TailLengthUnitProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the start width of the needle pointer's tail.
        /// </summary>
        /// <value>
        /// It defines the width of the tail. The default value is <c>8</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:NeedlePointer Value="50"
        ///                                      TailLength="0.1"
        ///                                      TailFill="Red"
        ///                                      TailLengthUnit="Factor"
        ///                                      TailWidth="20" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double TailWidth
        {
            get { return (double)this.GetValue(TailWidthProperty); }
            set { this.SetValue(TailWidthProperty, value); }
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Method to create pointer.
        /// </summary>
        internal override void CreatePointer()
        {
            CreateNeedlePointer();
        }

        /// <summary>
        /// Method to update needle pointer position. 
        /// </summary>
        internal override void UpdatePointer()
        {
            this.UpdateNeedleValue();
        }

        /// <summary>
        /// Method to draw needle pointer elements. 
        /// </summary>
        /// <param name="canvas"></param>
        internal override void Draw(ICanvas canvas)
        {
            DrawNeedle(canvas);

            DrawTail(canvas);

            DrawKnob(canvas);
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when knob related properties changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnKnobPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is NeedlePointer needlePointer)
            {
                needlePointer.CreateKnob();
                needlePointer.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when needle related properties changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnNeedlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is NeedlePointer needlePointer)
            {
                needlePointer.CreateNeedle();
                needlePointer.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when tail related properties changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnTailPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is NeedlePointer needlePointer)
            {
                needlePointer.CreateTail();
                needlePointer.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when invalidate properties changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnInvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is NeedlePointer needlePointer)
            {
                needlePointer.InvalidateDrawable();
            }
        }

        #endregion

        #region Private method

        /// <summary>
        /// Draw needle for pointer. 
        /// </summary>
        /// <param name="canvas"></param>
        private void DrawNeedle(ICanvas canvas)
        {
            if (this.needlePath != null && this.RadialAxis != null)
            {
                canvas.SaveState();

                //Setting needle path fill color.
                canvas.SetFillPaint(this.NeedleFill, this.needlePath.Bounds);

                canvas.Rotate(this.needlePathRotationAngle, this.RadialAxis.Center.X, this.RadialAxis.Center.Y);

                //Draw needle path.
                canvas.FillPath(this.needlePath);

                canvas.RestoreState();
            }
        }

        /// <summary>
        /// Draw needle knob.
        /// </summary>
        /// <param name="canvas"></param>
        private void DrawKnob(ICanvas canvas)
        {
            if (this.RadialAxis != null && this.knobRadius > 0)
            {
                canvas.SaveState();

                double radius = this.knobRadius - this.knobActualBorderThickness;

                //Setting needle path fill color.
                canvas.SetFillPaint(this.KnobFill, new Rectangle(this.RadialAxis.Center.X - radius, this.RadialAxis.Center.Y - knobRadius,
                    2 * radius, 2 * radius));

                canvas.FillCircle((float)this.RadialAxis.Center.X, (float)this.RadialAxis.Center.Y, (float)radius);

                if (this.knobActualBorderThickness > 0)
                {
                    //Setting needle path fill color.
                    canvas.StrokeColor = this.KnobStroke;

                    canvas.StrokeSize = (float)this.knobActualBorderThickness;

                    radius = radius + this.knobActualBorderThickness / 2;

                    canvas.DrawCircle((float)this.RadialAxis.Center.X, (float)this.RadialAxis.Center.Y, (float)radius);
                }

                canvas.RestoreState();
            }
        }

        /// <summary>
        /// Draw needle tail. 
        /// </summary>
        /// <param name="canvas"></param>
        private void DrawTail(ICanvas canvas)
        {
            if (this.tailRectangle != null && this.RadialAxis != null)
            {
                canvas.SaveState();

                canvas.Rotate(this.needlePathRotationAngle, this.RadialAxis.Center.X, this.RadialAxis.Center.Y);

                //Setting needle path fill color.
                canvas.SetFillPaint(this.TailFill, new Rectangle(this.tailRectangle.Value.X, this.tailRectangle.Value.Y, this.tailRectangle.Value.Width, this.tailRectangle.Value.Height));

                //Draw needle path.
                canvas.FillRectangle(this.tailRectangle.Value.X, this.tailRectangle.Value.Y, this.tailRectangle.Value.Width, this.tailRectangle.Value.Height);

                canvas.RestoreState();
            }
        }

        /// <summary>
        /// To create the needle pointer
        /// </summary>
        private void CreateNeedlePointer()
        {
            if (this.RadialAxis == null || this.RadialAxis.AvailableSize.IsZero)
            {
                return;
            }

            this.CreateNeedle();
            this.CreateKnob();
            this.CreateTail();
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
        /// To create needle.
        /// </summary>
        private void CreateNeedle()
        {
            if (this.RadialAxis == null || this.RadialAxis.AvailableSize.IsZero)
            {
                return;
            }

            double actualNeedleLength = this.RadialAxis.CalculateActualSize(this.NeedleLength, this.NeedleLengthUnit, false) * this.RadialAxis.RadiusFactor;
            float actualNeedleStartWidth = (float)this.RadialAxis.CalculateActualSize(this.NeedleStartWidth, SizeUnit.Pixel, false);
            float actualNeedleEndWidth = (float)this.RadialAxis.CalculateActualSize(this.NeedleEndWidth, SizeUnit.Pixel, false);
            Point vector = Utility.AngleToVector(270);
            PointF point = new PointF((float)(this.RadialAxis.Center.X + (actualNeedleLength * vector.X)), (float)(this.RadialAxis.Center.Y + (actualNeedleLength * vector.Y)));

            this.needlePath = new PathF();
            this.needlePath.LineTo(this.RadialAxis.Center.X - (actualNeedleEndWidth / 2), this.RadialAxis.Center.Y);
            this.needlePath.LineTo(point.X - (actualNeedleStartWidth / 2), point.Y);
            this.needlePath.LineTo(point.X + (actualNeedleStartWidth / 2), point.Y);
            this.needlePath.LineTo(this.RadialAxis.Center.X + (actualNeedleEndWidth / 2), this.RadialAxis.Center.Y);
        }

        /// <summary>
        /// Create needle knob. 
        /// </summary>
        private void CreateKnob()
        {
            if (this.RadialAxis == null || this.RadialAxis.AvailableSize.IsZero)
            {
                return;
            }

            knobRadius = this.RadialAxis.CalculateActualSize(this.KnobRadius, this.KnobSizeUnit, false) * this.RadialAxis.RadiusFactor;
            knobActualBorderThickness = this.RadialAxis.CalculateActualSize(this.KnobStrokeThickness, this.KnobSizeUnit, false);
        }

        /// <summary>
        /// Create needle tail. 
        /// </summary>
        private void CreateTail()
        {
            if (this.RadialAxis == null || this.RadialAxis.AvailableSize.IsZero)
            {
                return;
            }

            double actualTailLength = this.RadialAxis.CalculateActualSize(this.TailLength, this.TailLengthUnit, false) * this.RadialAxis.RadiusFactor;
            double actualTailWidth = this.RadialAxis.CalculateActualSize(this.TailWidth, SizeUnit.Pixel, false);

            if (actualTailLength > 0 && actualTailWidth > 0)
            {
                tailRectangle = new RectangleF((float)(this.RadialAxis.Center.X - (actualTailWidth * 0.5)),
                    (float)this.RadialAxis.Center.Y, (float)actualTailWidth, (float)actualTailLength);
            }
        }

        /// <summary>
        /// To update the needle based on value.
        /// </summary>
        private void UpdateNeedleValue()
        {
            if (this.needlePath != null && this.RadialAxis != null)
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

                needlePathRotationAngle = (float)this.RadialAxis.ValueToAngle(actualValue) + 90;

                this.CalculatePointerRect();
            }
        }

        /// <summary>
        /// Calculate needle pointer dragging bounds.
        /// </summary>
        private void CalculatePointerRect()
        {
            if (this.RadialAxis != null)
            {
                PointF center = this.RadialAxis.Center;
                double actualNeedleLength = this.RadialAxis.CalculateActualSize(this.NeedleLength, this.NeedleLengthUnit, false) * this.RadialAxis.RadiusFactor;
                var value = Math.Clamp(this.Value, this.RadialAxis.ActualMinimum, this.RadialAxis.ActualMaximum);
                var angle = this.RadialAxis.ValueToAngle(value);
                var radian = Utility.DegreeToRadian(angle);
                float x1 = center.X;
                float y1 = center.Y;

                float x2 = (float)(center.X + actualNeedleLength * Math.Cos(radian));
                float y2 = (float)(center.Y + actualNeedleLength * Math.Sin(radian));

                if (x1 > x2)
                {
                    float temp = x1;
                    x1 = x2;
                    x2 = temp;
                }

                if (y1 > y2)
                {
                    float temp = y1;
                    y1 = y2;
                    y2 = temp;
                }

                if (y2 - y1 < 20)
                {
                    y1 -= 10; // Creates the pointer rect with minimum height
                    y2 += 10;
                }

                float needleMaxWidth = (float)Math.Max(this.NeedleStartWidth, this.NeedleEndWidth) + DraggingOffset;
                
                if (x2 - x1 < needleMaxWidth)
                {
                    x1 -= needleMaxWidth / 2; // Creates the pointer rect with minimum width
                    x2 += needleMaxWidth / 2;
                }

                PointerRect = RectangleF.FromLTRB(x1, y1, x2, y2);
            }
        }

        #endregion
    }
}
