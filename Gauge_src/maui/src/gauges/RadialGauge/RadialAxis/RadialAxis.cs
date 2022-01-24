using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Core.Internals;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using GradientStop = Microsoft.Maui.Graphics.GradientStop;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// The RadialAxis is a circular arc in which a set of values are displayed along a linear or custom scale based on the design requirements.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// <gauge:SfRadialGauge>
    ///    <gauge:SfRadialGauge.Axes>
    ///        <gauge:RadialAxis RadiusFactor="0.85"
    ///                          Interval="10"
    ///                          FontSize="14"
    ///                          Minimum="-60"
    ///                          Maximum="60">
    ///
    ///            <gauge:RadialAxis.Pointers>
    ///                <gauge:NeedlePointer EnableAnimation="True"
    ///                                     NeedleLengthUnit="Factor"
    ///                                     NeedleLength="0.7"
    ///                                     Value="9.94"
    ///                                     NeedleStartWidth="2"
    ///                                     NeedleEndWidth="10"
    ///                                     KnobRadius="0.05"
    ///                                     KnobSizeUnit="Factor"
    ///                                     TailLength="0.1"
    ///                                     TailWidth="10">
    ///                </gauge:NeedlePointer>
    ///            </gauge:RadialAxis.Pointers>
    ///        </gauge:RadialAxis>
    ///    </gauge:SfRadialGauge.Axes>
    /// </gauge:SfRadialGauge>
    /// ]]></code>
    /// </example>
    public class RadialAxis : View, IContentView, ITouchListener, IVisualTreeElement
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Minimum"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Minimum"/> bindable property.
        /// </value>
        public static readonly BindableProperty MinimumProperty =
            BindableProperty.Create(nameof(Minimum), typeof(double), typeof(RadialAxis), 0d, propertyChanged: OnMinMaxPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Maximum"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Maximum"/> bindable property.
        /// </value>
        public static readonly BindableProperty MaximumProperty =
            BindableProperty.Create(nameof(Maximum), typeof(double), typeof(RadialAxis), 100d, propertyChanged: OnMinMaxPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Interval"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Interval"/> bindable property.
        /// </value>
        public static readonly BindableProperty IntervalProperty =
            BindableProperty.Create(nameof(Interval), typeof(double), typeof(RadialAxis), double.NaN, propertyChanged: OnAxisPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MinorTicksPerInterval"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MinorTicksPerInterval"/> bindable property.
        /// </value>
        public static readonly BindableProperty MinorTicksPerIntervalProperty =
            BindableProperty.Create(nameof(MinorTicksPerInterval), typeof(int), typeof(RadialAxis), 1, propertyChanged: OnAxisPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MaximumLabelsCount"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MaximumLabelsCount"/> bindable property.
        /// </value>
        public static readonly BindableProperty MaximumLabelsCountProperty =
            BindableProperty.Create(nameof(MaximumLabelsCount), typeof(int), typeof(RadialAxis), 3, propertyChanged: OnAxisPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StartAngle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StartAngle"/> bindable property.
        /// </value>
        public static readonly BindableProperty StartAngleProperty =
            BindableProperty.Create(nameof(StartAngle), typeof(double), typeof(RadialAxis), 130d, propertyChanged: OnAnglePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EndAngle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EndAngle"/> bindable property.
        /// </value>
        public static readonly BindableProperty EndAngleProperty =
            BindableProperty.Create(nameof(EndAngle), typeof(double), typeof(RadialAxis), 50d, propertyChanged: OnAnglePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="RadiusFactor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RadiusFactor"/> bindable property.
        /// </value>
        public static readonly BindableProperty RadiusFactorProperty =
            BindableProperty.Create(nameof(RadiusFactor), typeof(double), typeof(RadialAxis), 0.8d,
                coerceValue: (bindable, value) =>
                {
                    return Math.Clamp((double)value, 0, 1);
                },
        propertyChanged: OnRadiusFactorPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CanScaleToFit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CanScaleToFit"/> bindable property.
        /// </value>
        public static readonly BindableProperty CanScaleToFitProperty =
            BindableProperty.Create(nameof(CanScaleToFit), typeof(bool), typeof(RadialAxis), true, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowTicks"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowTicks"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShowTicksProperty =
            BindableProperty.Create(nameof(ShowTicks), typeof(bool), typeof(RadialAxis), true, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowAxisLine"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowAxisLine"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShowAxisLineProperty =
            BindableProperty.Create(nameof(ShowAxisLine), typeof(bool), typeof(RadialAxis), true, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowLabels"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowLabels"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShowLabelsProperty =
            BindableProperty.Create(nameof(ShowLabels), typeof(bool), typeof(RadialAxis), true, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowFirstLabel"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowFirstLabel"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShowFirstLabelProperty =
            BindableProperty.Create(nameof(ShowFirstLabel), typeof(bool), typeof(RadialAxis), true, propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ShowLastLabel"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ShowLastLabel"/> bindable property.
        /// </value>
        public static readonly BindableProperty ShowLastLabelProperty =
            BindableProperty.Create(nameof(ShowLastLabel), typeof(bool), typeof(RadialAxis), true, propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="IsInversed"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="IsInversed"/> bindable property.
        /// </value>
        public static readonly BindableProperty IsInversedProperty =
            BindableProperty.Create(nameof(IsInversed), typeof(bool), typeof(RadialAxis), false, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CanRotateLabels"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CanRotateLabels"/> bindable property.
        /// </value>
        public static readonly BindableProperty CanRotateLabelsProperty =
            BindableProperty.Create(nameof(CanRotateLabels), typeof(bool), typeof(RadialAxis), false, propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="UseRangeColorForAxis"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="UseRangeColorForAxis"/> bindable property.
        /// </value>
        public static readonly BindableProperty UseRangeColorForAxisProperty =
            BindableProperty.Create(nameof(UseRangeColorForAxis), typeof(bool), typeof(RadialAxis), false, propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EnableLoadingAnimation"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EnableLoadingAnimation"/> bindable property.
        /// </value>
        public static readonly BindableProperty EnableLoadingAnimationProperty =
            BindableProperty.Create(nameof(EnableLoadingAnimation), typeof(bool), typeof(RadialAxis), false);

        /// <summary>
        /// Identifies the <see cref="AnimationDuration"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AnimationDuration"/> bindable property.
        /// </value>
        public static readonly BindableProperty AnimationDurationProperty =
            BindableProperty.Create(nameof(AnimationDuration), typeof(double), typeof(RadialAxis), 1500d);

        /// <summary>
        /// Identifies the <see cref="TickPosition"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TickPosition"/> bindable property.
        /// </value>
        public static readonly BindableProperty TickPositionProperty =
            BindableProperty.Create(nameof(TickPosition), typeof(GaugeElementPosition), typeof(RadialAxis),
             GaugeElementPosition.Inside, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="TickOffset"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TickOffset"/> bindable property.
        /// </value>
        public static readonly BindableProperty TickOffsetProperty =
            BindableProperty.Create(nameof(TickOffset), typeof(double), typeof(RadialAxis), double.NaN, propertyChanged: OnTickOffsetPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LabelPosition"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LabelPosition"/> bindable property.
        /// </value>
        public static readonly BindableProperty LabelPositionProperty =
           BindableProperty.Create(nameof(LabelPosition), typeof(GaugeLabelsPosition), typeof(RadialAxis),
           GaugeLabelsPosition.Inside, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LabelOffset"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LabelOffset"/> bindable property.
        /// </value>
        public static readonly BindableProperty LabelOffsetProperty =
           BindableProperty.Create(nameof(LabelOffset), typeof(double), typeof(RadialAxis), double.NaN, propertyChanged: OnLabelOffsetPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="OffsetUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OffsetUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty OffsetUnitProperty =
            BindableProperty.Create(nameof(OffsetUnit), typeof(SizeUnit), typeof(RadialAxis), SizeUnit.Pixel, propertyChanged: OnOffsetPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LabelFormat"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LabelFormat"/> bindable property.
        /// </value>
        public static readonly BindableProperty LabelFormatProperty =
            BindableProperty.Create(nameof(LabelFormat), typeof(string), typeof(RadialAxis), null, propertyChanged: OnLabelFormatPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AxisLineStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AxisLineStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty AxisLineStyleProperty =
            BindableProperty.Create(nameof(AxisLineStyle), typeof(RadialLineStyle), typeof(RadialAxis), null, propertyChanged: OnStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MajorTickStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MajorTickStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty MajorTickStyleProperty =
            BindableProperty.Create(nameof(MajorTickStyle), typeof(RadialTickStyle), typeof(RadialAxis), null, propertyChanged: OnMajorTickStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MinorTickStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MinorTickStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty MinorTickStyleProperty =
            BindableProperty.Create(nameof(MinorTickStyle), typeof(RadialTickStyle), typeof(RadialAxis), null, propertyChanged: OnMinorTickStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="AxisLabelStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="AxisLabelStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty AxisLabelStyleProperty =
            BindableProperty.Create(nameof(AxisLabelStyle), typeof(GaugeLabelStyle), typeof(RadialAxis), null, propertyChanged: OnStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="BackgroundContent"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BackgroundContent"/> bindable property.
        /// </value>
        public static readonly BindableProperty BackgroundContentProperty =
            BindableProperty.Create(nameof(BackgroundContent), typeof(View), typeof(RadialAxis), null, propertyChanged: OnBackgroundContentPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Ranges"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Ranges"/> bindable property.
        /// </value>
        public static readonly BindableProperty RangesProperty =
            BindableProperty.Create(nameof(Ranges), typeof(ObservableCollection<RadialRange>), typeof(RadialAxis), null, propertyChanged: OnRangesPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Pointers"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Pointers"/> bindable property.
        /// </value>
        public static readonly BindableProperty PointersProperty =
           BindableProperty.Create(nameof(Pointers), typeof(ObservableCollection<RadialPointer>), typeof(RadialAxis), null, propertyChanged: OnPointersPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Annotations"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Annotations"/> bindable property.
        /// </value>
        public static readonly BindableProperty AnnotationsProperty =
          BindableProperty.Create(nameof(Annotations), typeof(ObservableCollection<GaugeAnnotation>), typeof(RadialAxis), null, propertyChanged: OnAnnotationsPropertyChanged);

        #endregion

        #region Fields

        private double radius;
        private PathF? axisLinePath;
        private Size arrangeSize = Size.Zero;
        private bool isTouchHandled;
        private bool canAnimate = true;

        /// <summary>
        /// Gets or sets double value, that used to customize <see cref="BackgroundContent"/> radius. The default value is <c>1</c>.
        /// </summary>
        protected double BackgroundContentRadiusFactor { get; set; } = 1;

        private List<GaugeArcInfo>? gradientArcPaths;

        private RadialAxisView axisView { get; set; }

        /// <summary>
        /// Gets or sets the major ticks radius difference.
        /// </summary>
        internal double MajorTicksRadiusDifference;

        /// <summary>
        /// Gets or sets the minor ticks radius difference.
        /// </summary>
        internal double MinorTicksRadiusDifference;

        /// <summary>
        /// Gets or sets the labels radius difference.
        /// </summary>
        internal double LabelsRadiusDifference;

        /// <summary>
        /// Gets or sets the actual minimum value of the axis.
        /// </summary>
        internal double ActualMinimum;

        /// <summary>
        /// Gets or sets the actual maximum value of the axis.
        /// </summary>
        internal double ActualMaximum;

        /// <summary>
        /// Gets or sets the visible labels in Axis.
        /// </summary>
        internal List<GaugeLabelInfo>? VisibleLabels;
        internal List<AxisTickInfo> MajorTickPositions;
        internal List<AxisTickInfo> MinorTickPositions;
        internal PointF Center;
        internal double ActualStartAngle;
        internal double ActualEndAngle;
        internal double ActualSweepAngle;
        internal double ActualMajorTickLength;
        internal double ActualMinorTickLength;
        internal double ActualAxisLineWidth;
        internal double ActualInterval;
        internal double? ActualTickOffset;
        internal double LabelMaximumSize;
        internal double? ActualLabelOffset;
        internal Size AvailableSize = Size.Zero;
        internal double AxisLineRadiusDifference;
        internal Grid RangesGrid;
        internal Grid PointersGrid;
        internal AbsoluteLayout AnnotationsLayout;
        internal double? AxisLoadingAnimationValue;
        internal Grid ParentGrid;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="RadialAxis"/> class.
        /// </summary>
        /// <example>
        /// Create RadialAxis with the default or required scale range and customized axis properties.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis StartAngle = "130"
        ///                           EndAngle="50"
        ///                           IsInversed="False"
        ///                           RadiusFactor="0.9"
        ///                           CanRotateLabels="False"
        ///                           ShowLabels="True"
        ///                           ShowAxisLine="True"
        ///                           ShowTicks="True"
        ///                           ShowFirstLabel="True"
        ///                           ShowLastLabel="True"
        ///                           Minimum="0"
        ///                           Maximum="100"
        ///                           Interval="10"
        ///                           MinorTicksPerInterval="5"
        ///                           >
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public RadialAxis()
        {
            this.AddTouchListener(this);

            this.axisView = new RadialAxisView(this);
            this.ParentGrid = new Grid();
            this.ParentGrid.Children.Add(this.axisView);

            this.Ranges = new ObservableCollection<RadialRange>();
            this.Pointers = new ObservableCollection<RadialPointer>();
            this.Annotations = new ObservableCollection<GaugeAnnotation>();

            this.AxisLineStyle = new RadialLineStyle();
            this.MajorTickStyle = new RadialTickStyle();
            this.MinorTickStyle = new RadialTickStyle();
            this.AxisLabelStyle = new GaugeLabelStyle();
            this.MajorTickPositions = new List<AxisTickInfo>();
            this.MinorTickPositions = new List<AxisTickInfo>();

            this.RangesGrid = new Grid();
            this.PointersGrid = new Grid();
            this.AnnotationsLayout = new AbsoluteLayout();

            this.ValidateStartEndAngle();
            this.ValidateMinimumMaximum();
        }

        #endregion

        #region Events

#nullable disable

        /// <summary>
        /// Called when an axis label is created
        /// </summary>
        /// <example>
        ///  # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis LabelCreated="RadialAxis_LabelCreated" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        ///  # [C#](#tab/tabid-2)
        ///  <code><![CDATA[
        ///  private void RadialAxis_LabelCreated(object sender, LabelCreatedEventArgs e)
        ///  {
        ///      e.Text += "%";
        ///  }
        ///  ]]></code>
        ///  ***
        /// </example>
        public event EventHandler<LabelCreatedEventArgs> LabelCreated;

#nullable enable

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the minimum value of the axis. The axis starts from this value.
        /// </summary>
        /// <value>
        /// It defines the minimum values of the <see cref="RadialAxis"/>. The default value is <c>0</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis Minimum="10" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double Minimum
        {
            get { return (double)this.GetValue(MinimumProperty); }
            set { this.SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value of the axis. The axis ends at this value.
        /// </summary>
        /// <value>
        /// It defines the maximum value of the <see cref="RadialAxis"/>. The default value is <c>100</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis Maximum="150" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double Maximum
        {
            get { return (double)this.GetValue(MaximumProperty); }
            set { this.SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interval value of the axis. Using this, the axis labels can be displayed after a certain interval value.
        /// </summary>
        /// <value>
        /// It defines the interval of the <see cref="RadialAxis"/>. The default value is <c>double.NaN</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis Interval="10" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double Interval
        {
            get { return (double)this.GetValue(IntervalProperty); }
            set { this.SetValue(IntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interval of the minor ticks.
        /// </summary>
        /// <value>
        /// It defines number of minor ticks will be rendered between the major ticks. The default value is <c>1</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis MinorTicksPerInterval="5" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public int MinorTicksPerInterval
        {
            get { return (int)this.GetValue(MinorTicksPerIntervalProperty); }
            set { this.SetValue(MinorTicksPerIntervalProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum number of labels to be displayed in an axis in 100 logical pixels.
        /// </summary>
        /// <value>
        /// Maximum number of labels to be displayed in a axis in 100 logical pixels. Its default value is <c>3</c>. 
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis MaximumLabelsCount="3" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public int MaximumLabelsCount
        {
            get { return (int)this.GetValue(MaximumLabelsCountProperty); }
            set { this.SetValue(MaximumLabelsCountProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the <see cref="StartAngle"/> of axis.
        /// </summary>
        /// <value>
        /// It defines the start angle of the axis. The default value is <c>130</c>.
        /// </value>
        /// <example>
        /// By setting the StartAngle, EndAngle and IsInversed, we can change the shape the radial gauge into 
        /// full radial gauge, half radial gauge, and quarter radial gauge.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis StartAngle="130" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double StartAngle
        {
            get { return (double)this.GetValue(StartAngleProperty); }
            set { this.SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the <see cref="EndAngle"/> of axis.
        /// </summary>
        /// <value>
        /// It defines the end angle of the axis. The default value is <c>50</c>.
        /// </value>
        /// <example>
        /// By setting the StartAngle, EndAngle and IsInversed, we can change the shape the radial gauge into 
        /// full radial gauge, half radial gauge, and quarter radial gauge.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis EndAngle="50" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double EndAngle
        {
            get { return (double)this.GetValue(EndAngleProperty); }
            set { this.SetValue(EndAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the radius of the <see cref="RadialAxis"/>. This value ranges from 0 to 1.
        /// </summary>
        /// <value>
        /// It defines the radius factor of the axis. The default value is <c>0.8</c>.
        /// </value>
        /// <example>
        /// The size of the axis, expressed as the radius (half the diameter) in factor.
        /// The radiusFactor must be between 0 and 1. Axis radius is determined by multiplying this factor value to the minimum width or height of the control.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis RadiusFactor="0.9" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double RadiusFactor
        {
            get { return (double)this.GetValue(RadiusFactorProperty); }
            set { this.SetValue(RadiusFactorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to adjust the half or quarter gauge to fit the axis boundary.
        /// If <see cref="CanScaleToFit"/> true, the center and radius position of the axis will be modified on the basis of the angle value.
        /// </summary>
        /// <value>
        /// <b>true</b> if CanScaleToFit is enabled; otherwise, <b>false</b>.The default value is <b>true</b>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis CanScaleToFit="False" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public bool CanScaleToFit
        {
            get { return (bool)this.GetValue(CanScaleToFitProperty); }
            set { this.SetValue(CanScaleToFitProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to shows or hides the axis tick lines.
        /// </summary>
        /// <value>
        /// <b>true</b> if axis line ticks are displayed; otherwise, <b>false</b>.The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the axis ticks will be rendered or not.
        /// If <see cref="ShowTicks"/> is <c>true</c>, the axis ticks will be rendered, otherwise not rendered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis ShowTicks="False" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public bool ShowTicks
        {
            get { return (bool)this.GetValue(ShowTicksProperty); }
            set { this.SetValue(ShowTicksProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to shows or hides the axis line.
        /// </summary>
        /// <value>
        /// <b>true</b> if axis line is displayed; otherwise, <b>false</b>.The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the axis line will be rendered or not.
        /// If <see cref="ShowAxisLine"/> is <c>true</c>, the axis line will be rendered, otherwise not rendered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis ShowAxisLine="False" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public bool ShowAxisLine
        {
            get { return (bool)this.GetValue(ShowAxisLineProperty); }
            set { this.SetValue(ShowAxisLineProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to shows or hides the axis labels.
        /// </summary>
        /// <value>
        /// <b>true</b> if axis labels are displayed; otherwise, <b>false</b>.The default value is <b>true</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the axis labels will be rendered or not.
        /// If <see cref="ShowLabels"/> is <c>true</c>, the axis labels will be rendered, otherwise not rendered.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis ShowLabels="False" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public bool ShowLabels
        {
            get { return (bool)this.GetValue(ShowLabelsProperty); }
            set { this.SetValue(ShowLabelsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether shows or hides the first label of the axis
        /// </summary>
        /// <value>
        /// <b>true</b> if first label is displayed; otherwise, <b>false</b>. The default value is <b>true</b>.
        /// </value>
        /// <example>
        /// When startAngle and endAngle are the same, the first and last labels are intersected.To prevent this, enable this property to be false, if ShowLastLabel is true.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis ShowFirstLabel="False" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public bool ShowFirstLabel
        {
            get { return (bool)this.GetValue(ShowFirstLabelProperty); }
            set { this.SetValue(ShowFirstLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether shows or hides the last label of the axis.
        /// </summary>
        /// <value>
        /// <b>true</b> if last label is displayed; otherwise, <b>false</b>. The default value is <b>true</b>.
        /// </value>
        /// <example>
        /// When startAngle and endAngle are the same, the first and last labels are intersected.To prevent this, enable this property to be false, if ShowFirstLabel is true.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis ShowLastLabel="False" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public bool ShowLastLabel
        {
            get { return (bool)this.GetValue(ShowLastLabelProperty); }
            set { this.SetValue(ShowLastLabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether inverts the axis from right to left.
        /// </summary>
        /// <value>
        /// <b>true</b> if axis is inversed; otherwise, <b>false</b>.The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// <c>IsInversed</c> decides whether the axis will be inversed or not.
        /// If <see cref="IsInversed"/> is <c>true</c>, the axis will be inversed, otherwise not inversed.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis IsInversed="True" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public bool IsInversed
        {
            get { return (bool)this.GetValue(IsInversedProperty); }
            set { this.SetValue(IsInversedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to rotate the labels.
        /// </summary>
        /// <value>
        /// <b>true</b> if labels are rotated; otherwise, <b>false</b>. The default value is <b>false</b>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis CanRotateLabels="True" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public bool CanRotateLabels
        {
            get { return (bool)this.GetValue(CanRotateLabelsProperty); }
            set { this.SetValue(CanRotateLabelsProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to use the range color for axis elements such as labels and ticks.
        /// </summary>
        /// <value>
        /// <b>true</b> if use range color is enabled; otherwise, <b>false</b>.The default value is <b>false</b>.
        /// </value>
        /// <remarks>
        /// It decides whether the corresponding range color will be applied to the axis elements like labels and ticks or not.
        /// If <see cref="UseRangeColorForAxis"/> is <c>true</c>, the corresponding range colors will be applied, otherwise not.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis UseRangeColorForAxis="True" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public bool UseRangeColorForAxis
        {
            get { return (bool)this.GetValue(UseRangeColorForAxisProperty); }
            set { this.SetValue(UseRangeColorForAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to animate radial axis each elements in load time.
        /// </summary>
        /// <value>
        /// <b>The default value is false</b>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge >
        ///        <gauge:SfRadialGauge.Axes>
        ///            <gauge:RadialAxis EnableLoadingAnimation = "True" AnimationDuration="2000">
        ///
        ///                <gauge:RadialAxis.Ranges>
        ///                    <gauge:RadialRange EndValue = "50" Label="Range"/>
        ///                </gauge:RadialAxis.Ranges>
        ///                <gauge:RadialAxis.Pointers>
        ///                    <gauge:RangePointer Value = "30" PointerOffset="30"/>
        ///                    <gauge:NeedlePointer Value = "80" />
        ///                    < gauge:MarkerPointer Value = "60" />
        ///
        ///                 </ gauge:RadialAxis.Pointers>
        ///                <gauge:RadialAxis.Annotations>
        ///                    <gauge:GaugeAnnotation DirectionValue = "90"
        ///                                           DirectionUnit="Angle" 
        ///                                           PositionFactor="0.5">
        ///                        <gauge:GaugeAnnotation.Content>
        ///                            <Label Text = "10" TextColor="Black"/>
        ///                        </gauge:GaugeAnnotation.Content>
        ///                    </gauge:GaugeAnnotation>
        ///                </gauge:RadialAxis.Annotations>
        ///            </gauge:RadialAxis>
        ///
        ///        </gauge:SfRadialGauge.Axes>
        ///    </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public bool EnableLoadingAnimation
        {
            get { return (bool)this.GetValue(EnableLoadingAnimationProperty); }
            set { this.SetValue(EnableLoadingAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the loading time animation duration in milliseconds. 
        /// </summary>
        /// <value>
        /// The default value is <c>1500</c> milliseconds.
        /// </value>
        /// <remarks>
        /// It specifies how long the loading time radial axis elements animation will take.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge >
        ///        <gauge:SfRadialGauge.Axes>
        ///            <gauge:RadialAxis EnableLoadingAnimation = "True" AnimationDuration="2000">
        ///
        ///                <gauge:RadialAxis.Ranges>
        ///                    <gauge:RadialRange EndValue = "50" Label="Range"/>
        ///                </gauge:RadialAxis.Ranges>
        ///                <gauge:RadialAxis.Pointers>
        ///                    <gauge:RangePointer Value = "30" PointerOffset="30"/>
        ///                    <gauge:NeedlePointer Value = "80" />
        ///                    < gauge:MarkerPointer Value = "60" />
        ///
        ///                 </ gauge:RadialAxis.Pointers>
        ///                <gauge:RadialAxis.Annotations>
        ///                    <gauge:GaugeAnnotation DirectionValue = "90"
        ///                                           DirectionUnit="Angle" 
        ///                                           PositionFactor="0.5">
        ///                        <gauge:GaugeAnnotation.Content>
        ///                            <Label Text = "10" TextColor="Black"/>
        ///                        </gauge:GaugeAnnotation.Content>
        ///                    </gauge:GaugeAnnotation>
        ///                </gauge:RadialAxis.Annotations>
        ///            </gauge:RadialAxis>
        ///
        ///        </gauge:SfRadialGauge.Axes>
        ///    </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double AnimationDuration
        {
            get { return (double)this.GetValue(AnimationDurationProperty); }
            set { this.SetValue(AnimationDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the position of the ticks inside, center, or outside the axis line.
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the position of ticks in the radial gauge.
        /// The default is <see cref="GaugeElementPosition.Inside"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis TickPosition="Outside" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public GaugeElementPosition TickPosition
        {
            get { return (GaugeElementPosition)this.GetValue(TickPositionProperty); }
            set { this.SetValue(TickPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to adjusts the axis ticks position from the axis lines. You can specify value either in logical pixel or radius factor using the <see cref="OffsetUnit"/> property.
        /// </summary>
        /// <value>
        /// It defines the offset of the ticks. The default value is <c>double.NaN</c>.
        /// </value>
        /// <example>
        /// If offsetUnit is GaugeSizeUnit.factor, value will be given from 0 to 1. Here ticks placing position is calculated by TickOffset * axis outer radius value.
        /// Example: TickOffset value is 0.2 and axis radius is 100, ticks moving 20(0.2 * 100) logical pixels from axis line outer end. If offsetUnit is GaugeSizeUnit.logicalPixel, the defined value distance ticks will move from the outer end of the axis.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis TickOffset="30" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double TickOffset
        {
            get { return (double)this.GetValue(TickOffsetProperty); }
            set { this.SetValue(TickOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the position of the axis labels inside or outside the axis line.
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the position of labels in the radial gauge.
        /// The registered default is <see cref="GaugeLabelsPosition.Inside"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis LabelPosition="Outside" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public GaugeLabelsPosition LabelPosition
        {
            get { return (GaugeLabelsPosition)this.GetValue(LabelPositionProperty); }
            set { this.SetValue(LabelPositionProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to adjusts the axis label position from tick end. You can specify value either in logical pixel or radius factor using the <see cref="OffsetUnit"/> property.
        /// </summary>
        /// <value>
        /// It defines the offset of the axis labels. The default value is <c>double.NaN</c>.
        /// </value>
        /// <example>
        /// If offsetUnit is GaugeSizeUnit.factor, value will be given from 0 to 1. Here labels placing position is calculated by LabelOffset * axis outer radius value.
        /// Example: labelOffset value is 0.2 and axis radius is 100, labels moving 20(0.2 * 100) logical pixels from axis line outer end. If offsetUnit is GaugeSizeUnit.logicalPixel, the defined value distance labels will move from the outer end of the axis.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis LabelOffset="30" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double LabelOffset
        {
            get { return (double)this.GetValue(LabelOffsetProperty); }
            set { this.SetValue(LabelOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates to calculate the labels and ticks offset either in logical pixel or factor.
        /// </summary>
        /// <value>
        /// One of the <see cref="SizeUnit"/> enumeration that specifies how the <see cref="LabelOffset"/> and <see cref="TickOffset"/> values are considered.
        /// The default mode is <see cref="SizeUnit.Pixel"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis OffsetUnit="Factor" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public SizeUnit OffsetUnit
        {
            get { return (SizeUnit)this.GetValue(OffsetUnitProperty); }
            set { this.SetValue(OffsetUnitProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to formats the axis labels with globalized string formats.
        /// </summary>
        /// <value>
        /// The string that specifies the globalized string formats for the axis labels. Its default value is <c>string.Empty</c>. 
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis LabelFormat="c" />
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public string LabelFormat
        {
            get { return (string)this.GetValue(LabelFormatProperty); }
            set { this.SetValue(LabelFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets <see cref="RadialTickStyle"/>, that used to customize major ticks.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.MajorTickStyle>
        ///                 <gauge:RadialTickStyle StrokeDashArray="2,2" />
        ///             </ gauge:RadialAxis.MajorTickStyle>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public RadialTickStyle MajorTickStyle
        {
            get { return (RadialTickStyle)this.GetValue(MajorTickStyleProperty); }
            set { this.SetValue(MajorTickStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets <see cref="RadialTickStyle"/>, that used to customize minor ticks.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.MinorTickStyle>
        ///                 <gauge:RadialTickStyle StrokeDashArray="2,2" />
        ///             </ gauge:RadialAxis.MinorTickStyle>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public RadialTickStyle MinorTickStyle
        {
            get { return (RadialTickStyle)this.GetValue(MinorTickStyleProperty); }
            set { this.SetValue(MinorTickStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets <see cref="GaugeLabelStyle"/>, that used to customize gauge axis labels.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        ///        <gauge:SfRadialGauge>
        ///            <gauge:SfRadialGauge.Axes>
        ///                <gauge:RadialAxis>
        ///                    <gauge:RadialAxis.AxisLabelStyle>
        ///                        <gauge:GaugeLabelStyle FontSize="14" />
        ///                    </ gauge:RadialAxis.AxisLabelStyle>
        ///                </gauge:RadialAxis>
        ///            </gauge:SfRadialGauge.Axes>
        ///        </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public GaugeLabelStyle AxisLabelStyle
        {
            get { return (GaugeLabelStyle)this.GetValue(AxisLabelStyleProperty); }
            set { this.SetValue(AxisLabelStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the axis line.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.AxisLineStyle>
        ///                 <gauge:RadialLineStyle Fill="Red" CornerStyle="BothCurve"/>
        ///             </gauge:RadialAxis.AxisLineStyle>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public RadialLineStyle AxisLineStyle
        {
            get { return (RadialLineStyle)this.GetValue(AxisLineStyleProperty); }
            set { this.SetValue(AxisLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a image to set in axis background.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis RadiusFactor="1" StartAngle="0"
        ///                              EndAngle="360"
        ///                              ShowAxisLine="False"
        ///                              CanRotateLabels="True"
        ///                              TickOffset="0.32"
        ///                              OffsetUnit="Factor"
        ///                              LabelOffset="0.05"
        ///                              Maximum="360"
        ///                              Minimum="0"
        ///                              Interval="30"
        ///                              MinorTicksPerInterval="4">
        ///                <gauge:RadialAxis.BackgroundContent>
        ///                    <Image Source="axisbackground.png" />
        ///                </ gauge:RadialAxis.BackgroundContent>
        ///                <gauge:RadialAxis.AxisLabelStyle>
        ///                    <gauge:GaugeLabelStyle TextColor="White" />
        ///                </ gauge:RadialAxis.AxisLabelStyle>
        ///                <gauge:RadialAxis.MajorTickStyle>
        ///                    <gauge:RadialTickStyle LengthUnit="Factor" Length="0.087"/>
        ///                </gauge:RadialAxis.MajorTickStyle>
        ///                <gauge:RadialAxis.MinorTickStyle>
        ///                    <gauge:RadialTickStyle LengthUnit= Factor" Length="0.058"/>
        ///                </gauge:RadialAxis.MinorTickStyle>
        ///            </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public View BackgroundContent
        {
            get { return (View)this.GetValue(BackgroundContentProperty); }
            set { this.SetValue(BackgroundContentProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="RadialRange"/> to the axis.
        /// </summary>
        /// <value>
        /// The collection of linear range to display the current value of the axis. The default value is empty collection.
        /// </value>
        /// <example>
        /// The below examples shows, how to add a collection of gauge range to the radial axis and customize each range by adding it to the ranges collection.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue="0" EndValue="35" Fill="Green" />
        ///                 <gauge:RadialRange StartValue="35" EndValue="70" Fill="Orange" />
        ///                 <gauge:RadialRange StartValue="70" EndValue="100" Fill="Red" />
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public ObservableCollection<RadialRange> Ranges
        {
            get { return (ObservableCollection<RadialRange>)this.GetValue(RangesProperty); }
            set { this.SetValue(RangesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="RadialPointer"/> to the axis. 
        /// </summary>
        /// <value>
        /// The collection of linear range to display the current value of the axis. The default value is empty collection.
        /// </value>
        /// <example>
        /// The below examples shows, how to add a collection of gauge pointer to the radial axis and customize each pointer by adding it to the pointers collection.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer Value="50" />
        ///                 <gauge:NeedlePointer Value="60" />
        ///                 <gauge:RangePointer Value="70"/>
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public ObservableCollection<RadialPointer> Pointers
        {
            get { return (ObservableCollection<RadialPointer>)this.GetValue(PointersProperty); }
            set { this.SetValue(PointersProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="GaugeAnnotation"/> to the axis. 
        /// </summary>
        /// <value>
        /// The collection of <see cref="GaugeAnnotation"/> to display the specific point of interest in the radial gauge.
        /// The default value is empty collection.
        /// </value>
        /// <example>
        /// The below examples shows, how to add a collection of annotation to the radial axis and customize each annotation by adding it to the annotation collection.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Annotations>
        ///                 <gauge:GaugeAnnotation DirectionUnit="Angle"
        ///                                        DirectionValue="90"
        ///                                        PositionFactor="0.6">
        ///                         <gauge:GaugeAnnotation.Content>
        ///                                 <Label Text="GaugeAnnotation"
        ///                                            TextColor="Black" />
        ///                         </gauge:GaugeAnnotation.Content>
        ///                 </gauge:GaugeAnnotation>
        ///             </gauge:RadialAxis.Annotations>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public ObservableCollection<GaugeAnnotation> Annotations
        {
            get { return (ObservableCollection<GaugeAnnotation>)this.GetValue(AnnotationsProperty); }
            set { this.SetValue(AnnotationsProperty, value); }
        }

        /// <summary>
        /// Gets or sets double value that represents actual radius value.
        /// </summary>
        internal double Radius
        {
            get
            {
                return this.radius;
            }

            set
            {
                if (this.radius != value)
                {
                    this.radius = value;
                    this.CalculateActualValues();

                    //Update background content size.
                    if (this.BackgroundContent != null)
                    {
                        double contentSize = this.radius * 2 * this.BackgroundContentRadiusFactor;
                        this.BackgroundContent.HeightRequest = contentSize;
                        this.BackgroundContent.WidthRequest = contentSize;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets boolean value, that represents the loading animation.
        /// </summary>
        internal bool CanAnimate
        {
            get 
            {
                return canAnimate && EnableLoadingAnimation && AnimationDuration > 0;
            }
            set 
            {
                canAnimate = value; 
            }
        }

        /// <summary>
        /// Gets the boolean value indicating to pass the touches on either the parent or child view.
        /// </summary>
        bool ITouchListener.IsTouchHandled
        {
            get
            {
                return isTouchHandled;
            }
        }

        /// <summary>
        /// Gets the root content of radial axis. 
        /// </summary>
        object IContentView.Content
        {
            get
            {
                return ParentGrid;
            }
        }

        /// <summary>
        /// Gets the root content of radial axis. 
        /// </summary>
        IView? IContentView.PresentedContent
        {
            get
            {
                return ParentGrid;
            }
        }

        /// <summary>
        /// Gets the padding value.
        /// </summary>
        Thickness IPadding.Padding
        {
            get
            {
                return new Thickness(0);
            }
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Method used to measure required size for axis. 
        /// </summary>
        /// <param name="widthConstraint">Available width.</param>
        /// <param name="heightConstraint">Available height.</param>
        /// <returns></returns>
        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            var width = double.IsPositiveInfinity(widthConstraint) ? 350d : widthConstraint;
            var height = double.IsPositiveInfinity(heightConstraint) ? 350d : heightConstraint;

            if (width > 0 && height > 0)
            {
                this.AvailableSize = new Size(width, height);
            }

            if(this.BackgroundContent != null)
            { 
                CalculateRadius();
            }

            return base.MeasureOverride(width, height);
        }

        /// <summary>
        /// Arrange the Axix view 
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Rectangle bounds)
        {
            if (this.AvailableSize.Width > 0 && this.AvailableSize.Height > 0)
            {
                if (this.AvailableSize.Width != this.arrangeSize.Width ||
                    this.AvailableSize.Height != this.arrangeSize.Height)
                {
                    this.arrangeSize = this.AvailableSize;
                    this.UpdateAxis();
                }
            }

            return base.ArrangeOverride(bounds);
        }


        /// <summary>
        /// Invoked whenever the binding context of the View changes.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.BackgroundContent != null)
            {
                SetInheritedBindingContext(this.BackgroundContent, this.BindingContext);
            }

            if (this.AxisLabelStyle != null)
            {
                SetInheritedBindingContext(this.AxisLabelStyle, this.BindingContext);
            }

            if (this.MajorTickStyle != null)
            {
                SetInheritedBindingContext(this.MajorTickStyle, this.BindingContext);
            }

            if (this.MinorTickStyle != null)
            {
                SetInheritedBindingContext(this.MinorTickStyle, this.BindingContext);
            }

            if (this.AxisLineStyle != null)
            {
                SetInheritedBindingContext(this.AxisLineStyle, this.BindingContext);
            }

            foreach (var range in this.Ranges)
            {
                SetInheritedBindingContext(range, this.BindingContext);
            }

            foreach (var pointer in this.Pointers)
            {
                SetInheritedBindingContext(pointer, this.BindingContext);
            }

            foreach (var annotation in this.Annotations)
            {
                SetInheritedBindingContext(annotation, this.BindingContext);
                SetInheritedBindingContext(annotation.Content, this.BindingContext);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Converts axis value to vector point.
        /// </summary>
        /// <param name="value">The axis value to convert as vector.</param>
        /// <returns>Vector point of the provided value.</returns>
        public Point ValueToPoint(double value)
        {
            return this.FactorToPoint(this.ValueToFactor(value));
        }

        /// <summary>
        /// Converts circular axis value to respective direction angle.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Angle of the given value.</returns>
        public double ValueToAngle(double value)
        {
            return this.FactorToAngle(this.ValueToFactor(value));
        }

        #endregion

        #region Virtual methods

        /// <summary>
        /// Calculates the visible labels based on axis interval and range.
        /// </summary>
        /// <returns>The visible label collection.</returns>
        /// <example>
        ///  # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <local:RadialAxisExt Minimum = "0"
        ///                              Maximum="150"
        ///                              ShowTicks="False"
        ///                              AxisLineWidthUnit="Factor"
        ///                              AxisLineWidth="0.15">
        ///         </local:RadialAxisExt>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        ///  # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// public class RadialAxisExt : RadialAxis
        /// {
        ///     protected override List<GaugeLabelInfo> GenerateVisibleLabels()
        ///     {
        ///         List<GaugeLabelInfo> customLabels = new List<GaugeLabelInfo>();
        ///         for (int i = 0; i < 9; i++)
        ///         {
        ///             double value = CalculateLabelValue(i);
        ///             GaugeLabelInfo label = new GaugeLabelInfo
        ///             {
        ///                 Value = value,
        ///                 Text = value.ToString()
        ///             };
        ///             customLabels.Add(label);
        ///         }
        /// 
        ///         return customLabels;
        ///     }
        ///     
        ///     double CalculateLabelValue(double value)
        ///     {
        ///         if (value == 0)
        ///         {
        ///             return 0;
        ///         }
        ///         else if (value == 1)
        ///         {
        ///             return 2;
        ///         }
        ///         else if (value == 2)
        ///         {
        ///             return 5;
        ///         }
        ///         else if (value == 3)
        ///         {
        ///             return 10;
        ///         }
        ///         else if (value == 4)
        ///         {
        ///             return 20;
        ///         }
        ///         else if (value == 5)
        ///         {
        ///             return 30;
        ///         }
        ///         else if (value == 6)
        ///         {
        ///             return 50;
        ///         }
        ///         else if (value == 7)
        ///         {
        ///             return 100;
        ///         }
        ///         else
        ///         {
        ///             return 150;
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// ***
        /// </example>
        protected virtual List<GaugeLabelInfo> GenerateVisibleLabels()
        {
            this.MinorTickPositions.Clear();
            List<GaugeLabelInfo> visibleLabels = new List<GaugeLabelInfo>();

            if (this.ActualInterval != 0 && this.ActualMinimum != this.ActualMaximum)
            {
                for (double i = this.ActualMinimum; i <= this.ActualMaximum; i += this.ActualInterval)
                {
                    GaugeLabelInfo currentLabel = this.GetAxisLabel(i);
                    visibleLabels.Add(currentLabel);
                    if (this.MinorTicksPerInterval > 0)
                    {
                        this.AddMinorTicksPoint(i);
                    }
                }

                GaugeLabelInfo label = visibleLabels[visibleLabels.Count - 1];
                if (label.Value != this.ActualMaximum && label.Value < this.ActualMaximum)
                {
                    GaugeLabelInfo currentLabel = this.GetAxisLabel(this.ActualMaximum);
                    visibleLabels.Add(currentLabel);
                }
            }

            return visibleLabels;
        }

        /// <summary>
        /// Converts axis value to circular factor value.
        /// </summary>
        /// <param name="value">The axis value to convert as factor.</param>
        /// <returns>Circular factor of the provided value.</returns>
        /// <example>
        ///  # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <local:RadialAxisExt Minimum = "0"
        ///                              Maximum="150"
        ///                              ShowTicks="False"
        ///                              AxisLineWidthUnit="Factor"
        ///                              AxisLineWidth="0.15">
        ///             <gauge:RangePointer EnableAnimation = "True"
        ///                                 Value="60"
        ///                                 LengthSize="0.15"
        ///                                 SizeUnit="Factor">
        ///                 <gauge:RangePointer.GradientStops>
        ///                     <gauge:GaugeGradientStop Value = "5"
        ///                                              Color="#FF9E40DC" />
        ///                     <gauge:GaugeGradientStop Value = "45"
        ///                                              Color="#FFE63B86" />
        ///                 </gauge:RangePointer.GradientStops>
        ///             </gauge:RangePointer>
        ///         </local:RadialAxisExt>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        ///  # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        /// public class RadialAxisExt : RadialAxis
        /// {
        ///     public override double ValueToFactor(double value)
        ///     {
        ///         if (value >= 0 && value <= 2)
        ///         {
        ///             return (value * 0.125) / 2;
        ///         }
        ///         else if (value > 2 && value <= 5)
        ///         {
        ///             return (((value - 2) * 0.125) / (5 - 2)) + (1 * 0.125);
        ///         }
        ///         else if (value > 5 && value <= 10)
        ///         {
        ///             return (((value - 5) * 0.125) / (10 - 5)) + (2 * 0.125);
        ///         }
        ///         else if (value > 10 && value <= 20)
        ///         {
        ///             return (((value - 10) * 0.125) / (20 - 10)) + (3 * 0.125);
        ///         }
        ///         else if (value > 20 && value <= 30)
        ///         {
        ///             return (((value - 20) * 0.125) / (30 - 20)) + (4 * 0.125);
        ///         }
        ///         else if (value > 30 && value <= 50)
        ///         {
        ///             return (((value - 30) * 0.125) / (50 - 30)) + (5 * 0.125);
        ///         }
        ///         else if (value > 50 && value <= 100)
        ///         {
        ///             return (((value - 50) * 0.125) / (100 - 50)) + (6 * 0.125);
        ///         }
        ///         else if (value > 100 && value <= 150)
        ///         {
        ///             return (((value - 100) * 0.125) / (150 - 100)) + (7 * 0.125);
        ///         }
        ///         else
        ///         {
        ///             return 1;
        ///         }
        ///     }
        /// }
        /// ]]></code>
        /// ***
        /// </example>
        public virtual double ValueToFactor(double value)
        {
            var start = this.ActualMinimum;
            var delta = this.GetActualRange();
            var result = (value - start) / delta;
            return this.IsInversed ? 1d - result : result;
        }

        #endregion

        #region Internal methods       

        /// <summary>
        /// Calculate the actual size based on size unit.
        /// </summary>
        /// <param name="size">The original size.</param>
        /// <param name="sizeUnit">The size unit.</param>
        /// <param name="isOffset">Indicate whether the provided value is offset or not.</param>
        /// <returns>Actual size of given size</returns>
        internal double CalculateActualSize(double size, SizeUnit sizeUnit, bool isOffset)
        {
            double actualValue = 0;
            switch (sizeUnit)
            {
                case SizeUnit.Factor:
                    {
                        size = size > 1 ? 1 : size;
                        if (isOffset)
                        {
                            actualValue = (1 - size) * this.Radius;
                            return actualValue;
                        }

                        size = size < 0 ? 0 : size;
                        actualValue = size * this.Radius;
                    }

                    break;
                case SizeUnit.Pixel:
                    {
                        if (isOffset)
                        {
                            return this.Radius - size;
                        }

                        actualValue = size < 0 ? 0 : size;
                    }

                    break;
            }

            return actualValue;
        }

        /// <summary>
        /// Converts circular factor value to vector point.
        /// </summary>
        /// <param name="factor">The factor value to convert as vector.</param>
        /// <returns>Factor value to vector point.</returns>
        internal Point FactorToPoint(double factor)
        {
            double angle = this.ActualStartAngle + (factor * this.ActualSweepAngle);
            double angleRadian = Utility.DegreeToRadian(angle);
            return new Point(Math.Cos(angleRadian), Math.Sin(angleRadian));
        }

        /// <summary>
        /// Converts factor value to angle.
        /// </summary>
        /// <param name="factor">Input factor value.</param>
        /// <returns>Returns angle value.</returns>
        internal double FactorToAngle(double factor)
        {
            return this.ActualStartAngle + (factor * this.ActualSweepAngle);
        }

        /// <summary>
        /// To get the current value based on current screen point.
        /// </summary>
        /// <param name="currentPoint">The current screen point</param>
        /// <param name="dragValue">The current value.</param>
        /// <returns>true if current point converted successfully; otherwise, false.</returns>
        internal bool PointToValue(Point currentPoint, ref double dragValue)
        {
            double angle = (Math.Atan2(currentPoint.Y - this.Center.Y, currentPoint.X - this.Center.X) * 180 / Math.PI) + 360;
            angle += (angle < 360 && angle > 180) ? 360 : 0;
            if (angle > this.ActualEndAngle)
            {
                angle %= 360;
            }

            if (angle >= this.ActualStartAngle && angle <= this.ActualEndAngle)
            {
                if (this.IsInversed)
                {
                    dragValue = this.ActualMaximum - ((angle - this.ActualStartAngle) * ((this.ActualMaximum - this.ActualMinimum) / this.ActualSweepAngle));
                    return true;
                }
                else
                {
                    dragValue = this.ActualMinimum + ((angle - this.ActualStartAngle) * ((this.ActualMaximum - this.ActualMinimum) / this.ActualSweepAngle));
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Method used to create closed arc.
        /// </summary>
        /// <param name="pathF">Rendering path.</param>
        /// <param name="outerArcTopLeft">Outer arc top left.</param>
        /// <param name="outerArcBottomRight">Outer arc bottom right.</param>
        /// <param name="innerArcTopLeft">Inner arc top left.</param>
        /// <param name="innerArcBottomRight">Inner arc bottom right.</param>
        /// <param name="arcCenter">Arc center.</param>
        /// <param name="startAngle">Start angle.</param>
        /// <param name="endAngle">End angle.</param>
        /// <param name="outerRadius">Outer arc radius</param>
        /// <param name="innerRadius">Inner arc radius.</param>
        internal void CreateFilledArc(PathF pathF, PointF outerArcTopLeft, PointF outerArcBottomRight,
            PointF innerArcTopLeft, PointF innerArcBottomRight, PointF arcCenter,
            double startAngle, double endAngle, double outerRadius, double innerRadius)
        {
            if (startAngle > endAngle)
            {
                double temp = endAngle;
                endAngle = startAngle;
                startAngle = temp;
            }

#if WINDOWS
            //TODO : Here we moved the path before adding shapes to already closed path.
            //We don't need this move for other platforms. We reported this problem in below link. 
            //https://github.com/dotnet/maui/issues/3507
            //Once the problem resolved, we need to remove this part.

            var startAngleVector = Utility.AngleToVector(startAngle);
            var startPoint = new PointF((float)(arcCenter.X + (outerRadius * startAngleVector.X)),
               (float)(arcCenter.Y + (outerRadius * startAngleVector.Y)));
            pathF.MoveTo(startPoint);
#endif
            //Draw outer arc.  
            pathF.AddArc(outerArcTopLeft, outerArcBottomRight, -(float)startAngle, -(float)endAngle, true);

            Point vector = Utility.AngleToVector(endAngle);
            PointF point = new PointF((float)(arcCenter.X + (innerRadius * vector.X)),
               (float)(arcCenter.Y + (innerRadius * vector.Y)));

            //Draw line to inner arc end angle.
            pathF.LineTo(point);

            //Draw inner arc.               
            pathF.AddArc(innerArcTopLeft, innerArcBottomRight, -(float)endAngle, -(float)startAngle, false);

            vector = Utility.AngleToVector(startAngle);
            point = new PointF((float)(arcCenter.X + (outerRadius * vector.X)),
               (float)(arcCenter.Y + (outerRadius * vector.Y)));

            //Draw line to outer arc start angle. 
            pathF.LineTo(point);

            //Close the path.
            pathF.Close();
        }

        /// <summary>
        /// Method used to create gradient arc segments. 
        /// </summary>
        /// <param name="gradientStops">Gradient stops.</param>
        /// <param name="innerStartRadius">Inner start radius.</param>
        /// <param name="innerEndRadius">Inner end radius.</param>
        /// <param name="rangeStart">Range start value.</param>
        /// <param name="rangeEnd">Range end value.</param>
        /// <returns></returns>
        internal List<GaugeArcInfo> CreateGradientArcSegments(List<GaugeGradientStop> gradientStops,
            double innerStartRadius, double innerEndRadius, double rangeStart, double rangeEnd)
        {
            gradientStops = Utility.UpdateGradientStopCollection(gradientStops, rangeStart, rangeEnd);

            var gradientRange = Utility.GetGradientRange(gradientStops, rangeStart, rangeEnd);

            double tempInnerStart = innerStartRadius;
            double tempInnerEnd = innerEndRadius;
            List<GaugeArcInfo> arcInfoCollection = new List<GaugeArcInfo>();

            for (int i = 0; i < gradientRange.Count - 1; i++)
            {
                double startAngle = this.ValueToAngle(gradientRange[i]);
                double endAngle = this.ValueToAngle(gradientRange[i + 1]);

                double offset = this.IsInversed ? -0.5 : 0.5; // Added .5 degree at the end angle to avoid line difference.
                endAngle += i < gradientRange.Count - 2 ? offset : 0;
                double rangeMidAngle = Math.Abs(endAngle - startAngle) / 2;
                
                Color color1 = gradientStops[i].Color;
                Color color2 = gradientStops[i + 1].Color;
                for (int j = 0; j < gradientStops.Count - 1; j++)
                {
                    if (gradientStops[j].ActualValue <= gradientRange[i] && gradientStops[j + 1].ActualValue > gradientRange[i])
                    {
                        double offset1 = (gradientRange[i] - gradientStops[j].ActualValue) / (gradientStops[j + 1].ActualValue - gradientStops[j].ActualValue);
                        color1 = Utility.GradientColorConvertion(gradientStops[j].Color, gradientStops[j + 1].Color, offset1);
                        double offset2 = (gradientRange[i + 1] - gradientStops[j].ActualValue) / (gradientStops[j + 1].ActualValue - gradientStops[j].ActualValue);
                        color2 = Utility.GradientColorConvertion(gradientStops[j].Color, gradientStops[j + 1].Color, offset2);
                    }
                }

                if (innerStartRadius != innerEndRadius)
                {
                    double innerOffsetFraction = (tempInnerEnd - tempInnerStart) / (rangeEnd - rangeStart);
                    double rangeStartOffset = rangeStart * innerOffsetFraction;
                    innerEndRadius = tempInnerStart + (gradientRange[i + 1] * innerOffsetFraction) - rangeStartOffset;
                    innerStartRadius = tempInnerStart + (gradientRange[i] * innerOffsetFraction) - rangeStartOffset;
                }

                if (rangeMidAngle <= 90)
                {
                    GaugeArcInfo arcInfo = this.CreateGradientArcSegment(startAngle, endAngle, color1, color2, innerStartRadius, innerEndRadius);
                    arcInfoCollection.Add(arcInfo);
                }
                else
                {
                    Color midColor = Utility.GradientColorConvertion(color1, color2, 0.5);
                    double midValue = gradientRange[i] + ((gradientRange[i + 1] - gradientRange[i]) / 2);
                    var midAngle = this.ValueToAngle(midValue);

                    GaugeArcInfo arcInfo = this.CreateGradientArcSegment(startAngle, midAngle, color1, midColor, innerStartRadius, innerEndRadius);
                    arcInfoCollection.Add(arcInfo);

                    arcInfo = this.CreateGradientArcSegment(midAngle, endAngle, midColor, color2, innerStartRadius, innerEndRadius);
                    arcInfoCollection.Add(arcInfo);
                }
            }

            return arcInfoCollection;
        }

        /// <summary>
        /// To calculate the center point and radius
        /// </summary>
        /// <param name="radius">The radius</param>
        protected internal virtual Point GetCenter(double radius)
        {
            if (!this.CanScaleToFit)
            {
                return new Point(this.AvailableSize.Width / 2, this.AvailableSize.Height / 2);
            }
            else
            {
                Point centerPoint;
                centerPoint = new Point(this.AvailableSize.Width * 0.5d, this.AvailableSize.Height * 0.5d);
                if (this.ActualSweepAngle == 359.99)
                {
                    return centerPoint;
                }

                double startAngle = this.ActualStartAngle;
                double endAngle = this.ActualEndAngle;

                var arraySize = ((Math.Max(Math.Abs((int)startAngle / 90), Math.Abs((int)endAngle / 90)) + 1) * 2) + 1;
                double[] regions = new double[arraySize];

                int arrayIndex = 0;
                for (int i = -(arraySize / 2); i < (arraySize / 2) + 1; i++)
                {
                    regions[arrayIndex] = i * 90;
                    arrayIndex++;
                }

                List<int> region = new List<int>();
                if (startAngle < endAngle)
                {
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (regions[i] > startAngle && regions[i] < endAngle)
                        {
                            region.Add((int)((regions[i] % 360) < 0 ? (regions[i] % 360) + 360 : (regions[i] % 360)));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < regions.Length; i++)
                    {
                        if (regions[i] < startAngle && regions[i] > endAngle)
                        {
                            region.Add((int)((regions[i] % 360) < 0 ? (regions[i] % 360) + 360 : (regions[i] % 360)));
                        }
                    }
                }

                return CalculateActualCenter(centerPoint, region, radius);
            }
        }

        /// <summary>
        /// Draw axis line.
        /// </summary>
        /// <param name="canvas">canvas</param>
        internal void DrawAxisLine(ICanvas canvas)
        {
            if (this.ActualAxisLineWidth > 0)
            {
                if (gradientArcPaths != null && gradientArcPaths.Count > 0)
                {
                    foreach (var path in gradientArcPaths)
                    {
                        canvas.SetFillPaint(path.FillPaint, path.ArcPath.Bounds);
                        canvas.FillPath(path.ArcPath);
                    }
                }
                else if (this.AxisLineStyle != null && this.axisLinePath != null)
                {
                    if (this.AxisLineStyle.GradientStops != null && this.AxisLineStyle.GradientStops.Count == 1)
                        canvas.FillColor = this.AxisLineStyle.GradientStops[0].Color;
                    else
                        canvas.SetFillPaint(this.AxisLineStyle.Fill, axisLinePath.Bounds);

                    //Drawing axis line with calculated path. 
                    canvas.FillPath(axisLinePath);
                }
            }
        }

        /// <summary>
        /// Draw axis ticks.
        /// </summary>
        /// <param name="canvas">canvas</param>
        internal void DrawMajorTicks(ICanvas canvas)
        {
            Paint? majorStrokePaint = null;

            //Setting major tick style.
            if (this.MajorTickStyle != null)
            {
                if (this.MajorTickStyle.StrokeThickness >= 0)
                {
                    canvas.StrokeSize = (float)this.MajorTickStyle.StrokeThickness;
                }

                //Setting axis major tick line dash array.
                if (this.MajorTickStyle.StrokeDashArray?.Count > 0)
                {
                    canvas.StrokeDashPattern = this.MajorTickStyle.StrokeDashArray.ToFloatArray();
                }
                else
                {
                    canvas.StrokeDashPattern = null;
                }

                if(this.MajorTickStyle.Stroke != null)
                {
                    majorStrokePaint = this.MajorTickStyle.Stroke;
                }
            }

            //Drawing major ticks.
            double length = this.MajorTickPositions.Count;

            if (this.AxisLoadingAnimationValue != null)
                length = length * (double)this.AxisLoadingAnimationValue;

            for (int i = 0; i < length; i++)
            {
                AxisTickInfo tick = this.MajorTickPositions[i];
            
                Color? rangeColor = GetRangeColor(tick.Value);

                if (rangeColor != null)
                {
                    canvas.StrokeColor = rangeColor;
                }
                else if (majorStrokePaint != null)
                {
                    // TODO: Add Paint support for Stroke in Microsoft.Maui.Graphics.
                    // For now, only support a solid color.
                    canvas.StrokeColor = majorStrokePaint.ToColor();
                }

                canvas.DrawLine(tick.StartPoint, tick.EndPoint);
            }
        }

        internal void DrawMinorTicks(ICanvas canvas)
        {
            Paint? minorStrokePaint = null;

            //Setting minor tick style.
            if (this.MinorTickStyle != null)
            {
                if (this.MinorTickStyle.StrokeThickness >= 0)
                {
                    canvas.StrokeSize = (float)this.MinorTickStyle.StrokeThickness;
                }

                //Setting axis major tick line dash array.
                if (this.MinorTickStyle.StrokeDashArray?.Count > 0)
                {
                    canvas.StrokeDashPattern = this.MinorTickStyle.StrokeDashArray.ToFloatArray();
                }
                else
                {
                    canvas.StrokeDashPattern = null;
                }
                if (this.MinorTickStyle.Stroke != null)
                {
                    minorStrokePaint = this.MinorTickStyle.Stroke;
                }
            }

            //Drawing minor ticks.
            double length = this.MinorTickPositions.Count;

            if (this.AxisLoadingAnimationValue != null)
                length = length * (double)this.AxisLoadingAnimationValue;

            for (int i = 0; i < length; i++)
            {
                AxisTickInfo tick = this.MinorTickPositions[i];

                Color ? rangeColor = GetRangeColor(tick.Value);

                if (rangeColor != null)
                {
                    canvas.StrokeColor = rangeColor;
                }
                else if (minorStrokePaint != null)
                {
                    // TODO: Add Paint support for Stroke in Microsoft.Maui.Graphics.
                    // For now, only support a solid color.
                    canvas.StrokeColor = minorStrokePaint.ToColor();
                }

                canvas.DrawLine(tick.StartPoint, tick.EndPoint);
            }
        }

        /// <summary>
        /// Draw axis labels.
        /// </summary>
        /// <param name="canvas">canvas</param>
        internal void DrawAxisLabels(ICanvas canvas)
        {
            if (this.VisibleLabels != null && this.VisibleLabels.Count > 0)
            {
                double length = this.VisibleLabels.Count;

                if (this.AxisLoadingAnimationValue != null)
                    length = length * (double)this.AxisLoadingAnimationValue;

                for (int i = 0; i < length; i++)
                {
                    GaugeLabelInfo label = this.VisibleLabels[i];

                    if (label.LabelStyle == null) continue;

                    PointF position = label.Position;

                    if ((!this.ShowFirstLabel && this.ActualMinimum == label.Value)
                        || (!this.ShowLastLabel && this.ActualMaximum == label.Value))
                    {
                        continue;
                    }

                    //Setting text color axis labels.
                    Color? rangeColor = GetRangeColor(label.Value);
                    GaugeLabelStyle? labelStyle = null;
                    if (rangeColor != null)
                    {
                        labelStyle = new GaugeLabelStyle() { TextColor = rangeColor, FontAttributes = label.LabelStyle.FontAttributes, 
                            FontFamily = label.LabelStyle.FontFamily, FontSize = label.LabelStyle.FontSize };

                    }

                    //Drawing axis labels.
                    if (this.CanRotateLabels)
                    {
                        canvas.SaveState();

                        float degree = (float)this.ValueToAngle(label.Value);
#if __ANDROID__
                   Point point = new Point(position.X + (label.DesiredSize.Width / 2),
                      position.Y - (label.DesiredSize.Height / 2));
#else
                        Point point = new Point(position.X + (label.DesiredSize.Width / 2),
                      position.Y + (label.DesiredSize.Height / 2));
#endif
                        canvas.Rotate(degree + 90, (float)point.X, (float)point.Y);

                        DrawText(canvas, label.Text, position.X, position.Y, labelStyle ?? label.LabelStyle);
                        canvas.RestoreState();
                    }
                    else
                    {
                        DrawText(canvas, label.Text, position.X, position.Y, labelStyle ?? label.LabelStyle);
                    }
                }
            }
        }

        /// <summary>
        /// Invalidate the axis view.
        /// </summary>
        internal void InvalidateDrawable()
        {
            if (this.axisView != null)
                this.axisView.InvalidateDrawable();
        }

        /// <summary>
        /// Method used to pass touch action and point to pointers.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="isTouchHandled"></param>
        internal void OnTouchListener(TouchEventArgs e, ref bool isTouchHandled)
        {
            if (Pointers != null && Pointers.Count > 0)
            {
                switch (e.Action)
                {
                    case TouchActions.Pressed:
                        for (int i = Pointers.Count - 1; i >= 0; i--)
                        {
                            RadialPointer pointer = Pointers[i];
                            if (pointer.IsInteractive)
                            {
                                if (!pointer.IsPressed)
                                    pointer.UpdatePointerPressed(e.TouchPoint);

                                if (pointer.IsPressed)
                                {
                                    isTouchHandled = true;
                                    break;
                                }
                            }
                        }
                        break;
                    case TouchActions.Moved:
                        foreach (var pointer in Pointers)
                        {
                            if (pointer.IsPressed)
                            {
                                pointer.DragPointer(e.TouchPoint);
                            }
                        }
                        break;
                    case TouchActions.Released:
                        foreach (var pointer in Pointers)
                        {
                            if (pointer.IsPressed)
                            {
                                pointer.UpdatePointerReleased();
                                isTouchHandled = false;
                            }
                        }
                        break;
                }
            }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when <see cref="MinorTicksPerInterval"/>, <see cref="Interval"/> and <see cref="MaximumLabelsCount"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnAxisPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                radialAxis.UpdateAxisElements();
                radialAxis.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when axis properties changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                radialAxis.UpdateAxis();
                radialAxis.InvalidateAxis();
            }
        }

#nullable disable
        /// <summary>
        /// Called when <see cref="Ranges"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnRangesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                if (oldValue != null)
                {
                    if (oldValue is ObservableCollection<RadialRange> ranges)
                    {
                        ranges.CollectionChanged -= radialAxis.Ranges_CollectionChanged;
                    }
                }

                if (newValue != null)
                {
                    if (newValue is ObservableCollection<RadialRange> ranges)
                    {
                        ranges.CollectionChanged += radialAxis.Ranges_CollectionChanged;
                    }
                }
            }
        }

        /// <summary>
        /// Called when <see cref="Pointers"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnPointersPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                if (oldValue != null)
                {
                    if (oldValue is ObservableCollection<RadialPointer> pointers)
                        pointers.CollectionChanged -= radialAxis.Pointers_CollectionChanged;
                }

                if (newValue != null)
                {
                    if (newValue is ObservableCollection<RadialPointer> pointers)
                        pointers.CollectionChanged += radialAxis.Pointers_CollectionChanged;
                }
            }
        }

        /// <summary>
        /// Called when <see cref="Annotations"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnAnnotationsPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                if (oldValue != null)
                {
                    if (oldValue is ObservableCollection<GaugeAnnotation> annotations)
                        annotations.CollectionChanged -= radialAxis.Annotations_CollectionChanged;
                }

                if (newValue != null)
                {
                    if (newValue is ObservableCollection<GaugeAnnotation> annotations)
                        annotations.CollectionChanged += radialAxis.Annotations_CollectionChanged;
                }
            }
        }

#nullable enable

        /// <summary>
        /// Called when axis drawing related properties changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnInvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                radialAxis.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="StartAngle"/> or <see cref="EndAngle"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnAnglePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                radialAxis.ValidateStartEndAngle();
                radialAxis.UpdateAxis();
                radialAxis.InvalidateAxis();
            }
        }

        /// <summary>
        /// Called when <see cref="RadiusFactor"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnRadiusFactorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                radialAxis.UpdateAxis();
                radialAxis.InvalidateAxis();
            }
        }

        /// <summary>
        /// Called when <see cref="Minimum"/> or <see cref="Maximum"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMinMaxPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                radialAxis.ValidateMinimumMaximum();
                radialAxis.UpdateAxis();
                radialAxis.InvalidateAxis();
            }
        }

        /// <summary>
        /// Called when <see cref="TickOffset"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnTickOffsetPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                if (!double.IsNaN((double)newValue))
                {
                    radialAxis.ActualTickOffset = radialAxis.CalculateOffsetValue((double)newValue, radialAxis.OffsetUnit);
                }
                else
                {
                    radialAxis.ActualTickOffset = null;
                }

                if (radialAxis.TickPosition != GaugeElementPosition.Inside
                    && (double.IsNaN((double)oldValue) || double.IsNaN((double)newValue)))
                {
                    radialAxis.UpdateAxis();
                    radialAxis.InvalidateAxis();
                }
                else
                {
                    radialAxis.UpdateAxisElements();
                    radialAxis.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Called when <see cref="LabelOffset"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnLabelOffsetPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                if (!double.IsNaN((double)newValue))
                {
                    radialAxis.ActualLabelOffset = radialAxis.CalculateOffsetValue((double)newValue, radialAxis.OffsetUnit);
                }
                else
                {
                    radialAxis.ActualLabelOffset = null;
                }

                if (radialAxis.LabelPosition == GaugeLabelsPosition.Outside && (double.IsNaN((double)oldValue) || double.IsNaN((double)newValue)))
                {
                    radialAxis.UpdateAxis();
                    radialAxis.InvalidateAxis();
                }
                else
                {
                    radialAxis.UpdateAxisElements();
                    radialAxis.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Called when <see cref="OffsetUnit"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnOffsetPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                if (!double.IsNaN(radialAxis.TickOffset))
                {
                    radialAxis.ActualTickOffset = radialAxis.CalculateOffsetValue(radialAxis.TickOffset, radialAxis.OffsetUnit);
                }

                if (!double.IsNaN(radialAxis.LabelOffset))
                {
                    radialAxis.ActualLabelOffset = radialAxis.CalculateOffsetValue(radialAxis.LabelOffset, radialAxis.OffsetUnit);
                }
                radialAxis.UpdateAxisElements();
                radialAxis.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="LabelFormat"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnLabelFormatPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                if (radialAxis.LabelPosition == GaugeLabelsPosition.Outside)
                {
                    radialAxis.UpdateAxis();
                    radialAxis.InvalidateAxis();
                }
                else
                {
                    radialAxis.UpdateAxisElements();
                    radialAxis.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Called when <see cref="BackgroundContent"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnBackgroundContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                if (oldValue is View oldView && radialAxis.ParentGrid.Children.Contains(oldView))
                {
                    radialAxis.ParentGrid.Children.Remove(oldView);
                }

                if (newValue is View newView && !radialAxis.ParentGrid.Children.Contains(newView))
                {
                    newView.HorizontalOptions = LayoutOptions.Center;
                    newView.VerticalOptions = LayoutOptions.Center;
                    radialAxis.ParentGrid.Children.Insert(0, newView);

                    if (!radialAxis.AvailableSize.IsZero)
                    {
                        SetInheritedBindingContext(newView, radialAxis.BindingContext);
                    }
#if WINDOWS || IOS
                    //TODO : Maui-WinUI does not trigger measure override for dynamic time children collection change.
                    //We reported this problem in below link.
                    //https://github.com/dotnet/maui/issues/3512
                    //We need to remove this section, once this problem get resolved.

                    if (!radialAxis.AvailableSize.IsZero)
                    {
                        double contentSize = radialAxis.radius * 2 * radialAxis.BackgroundContentRadiusFactor;
                        newView.HeightRequest = contentSize;
                        newView.WidthRequest = contentSize;
                    }
#endif
                }
            }
        }

        /// <summary>
        /// Called when <see cref="AxisLabelStyle"/> or property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                if (oldValue is RadialLineStyle oldAxisLineStyle)
                {
#nullable disable
                    if (oldAxisLineStyle.GradientStops is ObservableCollection<GaugeGradientStop> gradientStops)
                    {
                        gradientStops.CollectionChanged -= radialAxis.GradientStops_CollectionChanged;
                    }
#nullable enable
                    oldAxisLineStyle.PropertyChanged -= radialAxis.AxisLineStyle_PropertyChanged;
                }
                else if (oldValue is GaugeLabelStyle oldGaugeLabelStyle)
                {
                    oldGaugeLabelStyle.PropertyChanged -= radialAxis.GaugeLabelStyle_PropertyChanged;
                }

                if (newValue is RadialLineStyle axisLineStyle)
                {
#nullable disable
                    if (axisLineStyle.GradientStops is ObservableCollection<GaugeGradientStop> gradientStops)
                    {
                        gradientStops.CollectionChanged += radialAxis.GradientStops_CollectionChanged;
                    }
#nullable enable
                    axisLineStyle.PropertyChanged += radialAxis.AxisLineStyle_PropertyChanged;
                }
                else if (newValue is GaugeLabelStyle gaugeLabelStyle)
                {
                    gaugeLabelStyle.PropertyChanged += radialAxis.GaugeLabelStyle_PropertyChanged;
                }

                radialAxis.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="MinorTickStyle"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMinorTickStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                if (oldValue is RadialTickStyle oldMinorTickStyle)
                {
                    oldMinorTickStyle.PropertyChanged -= radialAxis.AxisMinorStyle_PropertyChanged;
                }

                if (newValue is RadialTickStyle newMinorTickStyle)
                {
                    newMinorTickStyle.PropertyChanged += radialAxis.AxisMinorStyle_PropertyChanged;
                }

                radialAxis.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="MajorTickStyle"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMajorTickStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialAxis radialAxis)
            {
                if (oldValue is RadialTickStyle oldMajorTickStyle)
                {
                    oldMajorTickStyle.PropertyChanged -= radialAxis.AxisMajorStyle_PropertyChanged;
                }

                if (newValue is RadialTickStyle newMajorTickStyle)
                {
                    newMajorTickStyle.IsMajorTicks = true;
                    newMajorTickStyle.PropertyChanged += radialAxis.AxisMajorStyle_PropertyChanged;
                }

                radialAxis.InvalidateDrawable();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// OnTouchAction method implemented by ITouchListener. 
        /// </summary>
        /// <param name="e"></param>
        void ITouchListener.OnTouch(TouchEventArgs e)
        {
            this.OnTouchListener(e, ref isTouchHandled);
        }

        /// <summary>
        /// Measure the content.
        /// </summary>
        /// <param name="widthConstraint"></param>
        /// <param name="heightConstraint"></param>
        /// <returns></returns>
        Size IContentView.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
        {
            return this.MeasureContent(widthConstraint, heightConstraint);
        }

        /// <summary>
        /// Arrange the content.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        Size IContentView.CrossPlatformArrange(Rectangle bounds)
        {
            this.ArrangeContent(bounds);
            return bounds.Size;
        }

        /// <summary>
        /// Annotation content collection added in the visual tree elements for hot reload case. 
        /// </summary>
        /// <returns></returns>
        IReadOnlyList<IVisualTreeElement> IVisualTreeElement.GetVisualChildren()
        {
            {
                if (this.Annotations != null && Annotations.Count > 0)
                {
                    List<Element> _logicalChildren = new();
                    foreach (var annotation in this.Annotations)
                        _logicalChildren.Add(annotation.Content);
                    return _logicalChildren.AsReadOnly();
                }
                return new List<IVisualTreeElement>();
            }
        }

        /// <summary>
        /// To raise label created event.
        /// </summary>
        /// <param name="label">Corresponding axis label instance.</param>
        private void RaiseLabelCreated(GaugeLabelInfo label)
        {
            if (this.LabelCreated == null)
            {
                return;
            }

            var args = new LabelCreatedEventArgs
            {
                Text = label.Text
            };
            this.LabelCreated(this, args);
            label.Text = args.Text;
            label.LabelStyle = args.Style ?? this.AxisLabelStyle;
        }

        /// <summary>
        /// Method used to get value match range color.
        /// </summary>
        /// <param name="value">Input axis value</param>
        /// <returns>Returns range color.</returns>
        private Color? GetRangeColor(double value)
        {
            RadialRange? range = null;
            if (UseRangeColorForAxis && this.Ranges != null && this.Ranges.Count > 0)
            {
                range = this.Ranges.FirstOrDefault(item => value <= item.ActualEndValue &&
                    value >= item.ActualStartValue);
            }

            if (range != null)
            {
                Paint rangeFillPaint = range.Fill;
                return rangeFillPaint.ToColor();
            }
            else
            {
                return null;
            }
        }

        #region Calculation methods

        /// <summary>
        /// To update the axis elements.
        /// </summary>
        private void UpdateAxis()
        {
             if (!this.AvailableSize.IsZero && 
                (!this.CanAnimate || !AnimationExtensions.AnimationIsRunning(this, "GaugeLoadingAnimation")))
            {
                this.CalculateRadius();
                this.UpdateAxisElements();
                this.UpdateAxisSubElements();
            }
        }

        /// <summary>
        /// To update axis line, ticks and labels axis elements.
        /// </summary>
        private void UpdateAxisElements()
        {
            if (!this.AvailableSize.IsZero)
            {
                this.ActualInterval = this.CalculateActualInterval();
                this.VisibleLabels = this.GenerateVisibleLabels();
                this.MeasureLabels();
                this.CalculateElementsRadiusDifference();

                if(!this.CanAnimate)
                	this.CalculateAxisLine();

                if (this.ShowTicks)
                {
                    this.CalculateMajorTickPosition();

                    if (this.MinorTicksPerInterval > 0)
                    {
                        this.CalculateMinorTickPosition();
                    }
                }
                if (this.ShowLabels)
                {
                    this.CalculateAxisLabelsPosition();
                }
            }
        }

        /// <summary>
        /// To calculate the radius difference for axis elements.
        /// </summary>
        private void CalculateElementsRadiusDifference()
        {
            double maximumTickLineSize = this.ActualMajorTickLength;
            if (this.MinorTicksPerInterval > 0)
            {
                maximumTickLineSize = this.ActualMajorTickLength > this.ActualMinorTickLength ? this.ActualMajorTickLength : this.ActualMinorTickLength;
            }
            double tickOffset = this.ActualTickOffset ?? 0;
            if (!this.ShowTicks)
            {
                maximumTickLineSize = 0;
                tickOffset = 0;
            }

            double labelMaximumSize = this.LabelMaximumSize;
            double labelOffset = this.ActualLabelOffset ?? 10;
            if (!this.ShowLabels)
            {
                labelMaximumSize = 0;
                labelOffset = 0;
            }

            double actualAxisLineWidth = this.ActualAxisLineWidth;
            if (!this.ShowAxisLine)
            {
                actualAxisLineWidth = 0;
            }

            switch (this.TickPosition)
            {
                case GaugeElementPosition.Cross:
                    switch (this.LabelPosition)
                    {
                        case GaugeLabelsPosition.Inside:
                            this.AxisLineRadiusDifference = 0;
                            this.MajorTicksRadiusDifference = (actualAxisLineWidth / 2) + (this.ActualMajorTickLength / 2);
                            this.MinorTicksRadiusDifference = (actualAxisLineWidth / 2) + (this.ActualMinorTickLength / 2);
                            this.LabelsRadiusDifference = actualAxisLineWidth + labelOffset + (labelMaximumSize / 2);
                            break;
                        case GaugeLabelsPosition.Outside:
                            this.AxisLineRadiusDifference = 0;
                            this.MajorTicksRadiusDifference = (this.ActualMajorTickLength / 2) + (actualAxisLineWidth / 2);
                            this.MinorTicksRadiusDifference = (this.ActualMinorTickLength / 2) + (actualAxisLineWidth / 2);
                            this.LabelsRadiusDifference = -(labelOffset + labelMaximumSize / 2);
                            break;
                    }

                    break;
                case GaugeElementPosition.Inside:
                    switch (this.LabelPosition)
                    {
                        case GaugeLabelsPosition.Inside:
                            this.AxisLineRadiusDifference = 0;
                            this.MajorTicksRadiusDifference = actualAxisLineWidth + this.ActualMajorTickLength + tickOffset;
                            this.MinorTicksRadiusDifference = actualAxisLineWidth + this.ActualMinorTickLength + tickOffset;
                            this.LabelsRadiusDifference = actualAxisLineWidth + maximumTickLineSize + labelOffset + (labelMaximumSize / 2) + tickOffset;
                            break;
                        case GaugeLabelsPosition.Outside:
                            this.LabelsRadiusDifference = -(labelOffset + labelMaximumSize / 2);
                            this.AxisLineRadiusDifference = 0;
                            this.MajorTicksRadiusDifference = actualAxisLineWidth + this.ActualMajorTickLength + tickOffset;
                            this.MinorTicksRadiusDifference = actualAxisLineWidth + this.ActualMinorTickLength + tickOffset;
                            break;
                    }

                    break;
                case GaugeElementPosition.Outside:
                    switch (this.LabelPosition)
                    {
                        case GaugeLabelsPosition.Inside:
                            this.MajorTicksRadiusDifference = this.MinorTicksRadiusDifference = -tickOffset;
                            this.LabelsRadiusDifference = actualAxisLineWidth + labelOffset + (labelMaximumSize / 2);
                            this.AxisLineRadiusDifference = 0;
                            break;
                        case GaugeLabelsPosition.Outside:
                            this.LabelsRadiusDifference = -(maximumTickLineSize + labelOffset + tickOffset + labelMaximumSize / 2);
                            this.MajorTicksRadiusDifference = this.MinorTicksRadiusDifference = -tickOffset;
                            this.AxisLineRadiusDifference = 0;
                            break;
                    }

                    break;
            }

        }

        /// <summary>
        /// To calculate actual values.
        /// </summary>
        private void CalculateActualValues()
        {
            if (this.AxisLineStyle != null)
            {
                this.ActualAxisLineWidth = this.CalculateActualSize(this.AxisLineStyle.Thickness, this.AxisLineStyle.ThicknessUnit, false);
            }

            if (this.MajorTickStyle != null)
            {
                this.ActualMajorTickLength = this.CalculateActualSize(this.MajorTickStyle.Length, this.MajorTickStyle.LengthUnit, false);
            }

            if (this.MinorTickStyle != null)
            {
                this.ActualMinorTickLength = this.CalculateActualSize(this.MinorTickStyle.Length, this.MinorTickStyle.LengthUnit, false);
            }

            if (!double.IsNaN(this.TickOffset))
            {
                this.ActualTickOffset = this.CalculateOffsetValue(this.TickOffset, this.OffsetUnit);
            }

            if (!double.IsNaN(this.LabelOffset))
            {
                this.ActualLabelOffset = this.CalculateOffsetValue(this.LabelOffset, this.OffsetUnit);
            }
        }

        /// <summary>
        /// Method used to calculate major tick positions. 
        /// </summary>
        private void CalculateMajorTickPosition()
        {
            if (this.VisibleLabels != null)
            {
                this.MajorTickPositions.Clear();

                for (int i = 0; i < this.VisibleLabels.Count; i++)
                {
                    AxisTickInfo tickInfo = new AxisTickInfo() { Value = this.VisibleLabels[i].Value };
                    if (i == 0)
                    {
                        CalculateTickPosition(tickInfo, true, !IsInversed, IsInversed);
                    }
                    else if (i == this.VisibleLabels.Count - 1)
                    {
                        CalculateTickPosition(tickInfo, true, IsInversed, !IsInversed);
                    }
                    else
                    {
                        CalculateTickPosition(tickInfo, true);
                    }
                    
                    this.MajorTickPositions.Add(tickInfo);
                }
            }
        }

        /// <summary>
        /// Method used to calculate minor tick positions. 
        /// </summary>
        private void CalculateMinorTickPosition()
        {
            foreach (var tickInfo in this.MinorTickPositions)
            {
                CalculateTickPosition(tickInfo, false);
            }
        }

        /// <summary>
        /// Method used to calculate tick positions. 
        /// </summary>
        private void CalculateTickPosition(AxisTickInfo tickInfo, bool isMajorTick, bool isFirstTick = false, bool isLastTick = false)
        {
            Point vector;

            //Adjust edge major tick position match with axis line. 
            if (isMajorTick && this.MajorTickStyle != null && this.ActualSweepAngle != 359.99 && (isFirstTick || isLastTick))
            {
                double radiusAngle = Utility.CornerRadiusAngle(this.Radius * this.RadiusFactor, this.MajorTickStyle.StrokeThickness / 2);
                double angle = this.ValueToAngle(tickInfo.Value);

                if (isFirstTick)
                    vector = Utility.AngleToVector(angle + radiusAngle);
                else
                    vector = Utility.AngleToVector(angle - radiusAngle);
            }
            else
                vector = ValueToPoint(tickInfo.Value);

            double radius = (this.Radius * this.RadiusFactor) - (isMajorTick ? this.MajorTicksRadiusDifference : this.MinorTicksRadiusDifference);

            Point point = new Point(this.Center.X + (radius * vector.X), this.Center.Y + (radius * vector.Y));
            double actualTickLength = isMajorTick ? this.ActualMajorTickLength : this.ActualMinorTickLength;
            if (radius < 0)
            {
                actualTickLength += radius;
                actualTickLength = actualTickLength < 0 ? 0 : actualTickLength;
                point.X = this.Center.X;
                point.Y = this.Center.Y;
            }

            tickInfo.StartPoint = new PointF((float)point.X, (float)point.Y);
            tickInfo.EndPoint = new PointF((float)(point.X + (actualTickLength * vector.X)), (float)(point.Y + (actualTickLength * vector.Y)));

        }

        /// <summary>
        /// Method used to calculate axis labels position. 
        /// </summary>
        private void CalculateAxisLabelsPosition()
        {
            if (this.VisibleLabels != null)
            {
                double radius;
                Point point;

                foreach (var labelInfo in this.VisibleLabels)
                {
                    Point vector = ValueToPoint(labelInfo.Value);

                    radius = (this.Radius * this.RadiusFactor) - this.LabelsRadiusDifference;
#if __ANDROID__
                    point = new Point(this.Center.X + (radius * vector.X) - (labelInfo.DesiredSize.Width / 2),
                        Center.Y + (radius * vector.Y) + (labelInfo.DesiredSize.Height / 2));
#else
                    point = new Point(this.Center.X + (radius * vector.X) - (labelInfo.DesiredSize.Width / 2),
                        Center.Y + (radius * vector.Y) - (labelInfo.DesiredSize.Height / 2));
#endif

                    if (radius < 0)
                    {
                        point.X = this.Center.X - (labelInfo.DesiredSize.Width / 2);
                        point.Y = this.Center.Y - (labelInfo.DesiredSize.Height / 2);
                    }

                    labelInfo.Position = new PointF((float)point.X, (float)point.Y);
                }
            }
        }

        /// <summary>
        /// Method used to calculate axis line. 
        /// </summary>
        private void CalculateAxisLine()
        {
            if (this.AvailableSize.IsZero)
                return;

            //Calculating axis line radius.
            var radius = (this.Radius * this.RadiusFactor) - this.AxisLineRadiusDifference;
            float outerRadius = radius < 0 ? 0 : (float)radius;
            float innerRadius = outerRadius == 0 ? 0 : outerRadius - (float)this.ActualAxisLineWidth;
            innerRadius = innerRadius < 0 ? 0 : innerRadius;
            float lineHalfWidth = (float)this.ActualAxisLineWidth / 2;
            double cornerRadiusAngle = Utility.CornerRadiusAngle(this.Radius, lineHalfWidth);
            double arcStartAngle = this.ActualStartAngle;
            double arcEndAngle = this.ActualEndAngle;
            Point? endCurveCapCenter = null;
            Point? startCurveCapCenter = null;
            float midRadius = outerRadius == 0 ? 0 : outerRadius - lineHalfWidth;
            double dashLineLength = 0, dashLineGap = 0;
            axisLinePath = new PathF();

            //Calculating start angle and start curve cap center.
            if ((this.AxisLineStyle.CornerStyle == CornerStyle.StartCurve && !IsInversed) ||
                (this.AxisLineStyle.CornerStyle == CornerStyle.EndCurve && IsInversed) ||
                this.AxisLineStyle.CornerStyle == CornerStyle.BothCurve)
            {
                arcStartAngle += cornerRadiusAngle;
                Point vector = Utility.AngleToVector(arcStartAngle);
                startCurveCapCenter = new Point(this.Center.X + (midRadius * vector.X), this.Center.Y + (midRadius * vector.Y));
            }

            //Calculating end angle and end curve cap center.
            if ((this.AxisLineStyle.CornerStyle == CornerStyle.EndCurve && !IsInversed) ||
                (this.AxisLineStyle.CornerStyle == CornerStyle.StartCurve && IsInversed) ||
                this.AxisLineStyle.CornerStyle == CornerStyle.BothCurve)
            {
                arcEndAngle -= cornerRadiusAngle;
                Point vector = Utility.AngleToVector(arcEndAngle);
                endCurveCapCenter = new Point(this.Center.X + (midRadius * vector.X), this.Center.Y + (midRadius * vector.Y));
            }

            PointF outerArcTopLeft = new PointF(this.Center.X - outerRadius, this.Center.Y - outerRadius);
            PointF outerArcBottomRight = new PointF(this.Center.X + outerRadius, this.Center.Y + outerRadius);
            PointF innerArcTopLeft = new PointF(this.Center.X - innerRadius, this.Center.Y - innerRadius);
            PointF innerArcBottomRight = new PointF(this.Center.X + innerRadius, this.Center.Y + innerRadius);

            if (this.AxisLineStyle.GradientStops != null && this.AxisLineStyle.GradientStops.Count > 1)
            {
                //Create gradient arc data collection.
                this.gradientArcPaths = this.CreateGradientArcSegments(this.AxisLineStyle.GradientStops.ToList(), innerRadius,
                    innerRadius, this.ActualMinimum, this.ActualMaximum);

                for (int i = 0; i < gradientArcPaths.Count; i++)
                {
                    GaugeArcInfo arcInfo = gradientArcPaths[i];

                    //Update gradient arc angles based on corner style.
                    if (i == 0 && ((!this.IsInversed && startCurveCapCenter != null) ||
                        (this.IsInversed && endCurveCapCenter != null)))
                    {
                        arcInfo.StartAngle += this.IsInversed ? -(float)cornerRadiusAngle : (float)cornerRadiusAngle;
                    }

                    if (i == gradientArcPaths.Count - 1 && ((!this.IsInversed && endCurveCapCenter != null) ||
                        (this.IsInversed && startCurveCapCenter != null)))
                    {
                        arcInfo.EndAngle -= this.IsInversed ? -(float)cornerRadiusAngle : (float)cornerRadiusAngle;
                    }

                    //Create gradient arc path. 
                    this.CreateFilledArc(arcInfo.ArcPath, outerArcTopLeft, outerArcBottomRight, innerArcTopLeft,
                   innerArcBottomRight, this.Center, arcInfo.StartAngle, arcInfo.EndAngle, outerRadius, innerRadius);
                }

                //Calculate axis line edge curve path.
                if (this.IsInversed)
                {
                    CalculateAxisLineEndCurve(gradientArcPaths[0].ArcPath, arcEndAngle, endCurveCapCenter, outerRadius, lineHalfWidth);
                    CalculateAxisLineStartCurve(gradientArcPaths[gradientArcPaths.Count - 1].ArcPath, arcStartAngle, startCurveCapCenter, innerRadius, lineHalfWidth);
                }
                else
                {
                    CalculateAxisLineStartCurve(gradientArcPaths[0].ArcPath, arcStartAngle, startCurveCapCenter, innerRadius, lineHalfWidth);
                    CalculateAxisLineEndCurve(gradientArcPaths[gradientArcPaths.Count - 1].ArcPath, arcEndAngle, endCurveCapCenter, outerRadius, lineHalfWidth);
                }
            }
            else
            {
                this.gradientArcPaths = null;

                if (this.AxisLineStyle.DashArray != null && this.AxisLineStyle.DashArray.Count > 1)
                {
                    dashLineLength = this.AxisLineStyle.DashArray[0];
                    dashLineGap = this.AxisLineStyle.DashArray[1];
                }

                if (dashLineLength > 0 && dashLineGap > 0)
                {
                    float dashArrayStartAngle, dashArrayEndAngle;
                    dashArrayStartAngle = (float)arcStartAngle;
                    dashArrayEndAngle = dashArrayStartAngle + (float)dashLineLength;

                    while (dashArrayEndAngle <= arcEndAngle)
                    {
                        this.CreateFilledArc(axisLinePath, outerArcTopLeft, outerArcBottomRight, innerArcTopLeft,
                            innerArcBottomRight, this.Center, dashArrayStartAngle, dashArrayEndAngle, outerRadius, innerRadius);

                        dashArrayStartAngle = dashArrayEndAngle + (float)dashLineGap;
                        dashArrayEndAngle = dashArrayStartAngle + (float)dashLineLength;
                    }

                    if (dashArrayEndAngle != arcEndAngle)
                    {
                        dashArrayStartAngle = (float)arcEndAngle - 1;
                        dashArrayEndAngle = (float)arcEndAngle;

                        this.CreateFilledArc(axisLinePath, outerArcTopLeft, outerArcBottomRight, innerArcTopLeft,
                            innerArcBottomRight, this.Center, dashArrayStartAngle, dashArrayEndAngle, outerRadius, innerRadius);
                    }
                }
                else
                {
                    this.CreateFilledArc(axisLinePath, outerArcTopLeft, outerArcBottomRight, innerArcTopLeft,
                            innerArcBottomRight, this.Center, arcStartAngle, arcEndAngle, outerRadius, innerRadius);

                    //For 360 angle, fill arc not displaying. To resolve the issue drawn 359.99 angle arc and small gap filled by circle.
                    if (this.ActualSweepAngle == 359.99 && this.AxisLineStyle.CornerStyle == CornerStyle.BothFlat)
                    {
                        Point vector = Utility.AngleToVector(arcStartAngle);
                        PointF point = new PointF((float)(this.Center.X + (midRadius * vector.X)),
                           (float)(this.Center.Y + (midRadius * vector.Y)));

                        axisLinePath.AppendCircle(point, lineHalfWidth);
                    }
                }

                //Calculate axis line edge curve path.
                CalculateAxisLineStartCurve(axisLinePath, arcStartAngle, startCurveCapCenter, innerRadius, lineHalfWidth);
                CalculateAxisLineEndCurve(axisLinePath, arcEndAngle, endCurveCapCenter, outerRadius, lineHalfWidth);
            }
        }

        /// <summary>
        /// To create gradient segment. 
        /// </summary>
        /// <param name="startAngle">Start angle.</param>
        /// <param name="endAngle">End angle.</param>
        /// <param name="color1">Start color.</param>
        /// <param name="color2">End color.</param>
        /// <param name="innerStartRadius">Inner start radius.</param>
        /// <param name="innerEndRadius">Inner end radius.</param>
        /// <returns></returns>
        private GaugeArcInfo CreateGradientArcSegment(double startAngle, double endAngle, Color color1, Color color2, double innerStartRadius, double innerEndRadius)
        {
            Point startPoint, endPoint;

            CalculateGradientArcOffset(startAngle, endAngle, out startPoint, out endPoint);

            LinearGradientPaint gradient = new LinearGradientPaint()
            {
                StartPoint = startPoint,
                EndPoint = endPoint
            };

            gradient.GradientStops = new GradientStop[]
            {
                new GradientStop( 0.1f ,color1),
                new GradientStop(0.9f, color2)
            };

            GaugeArcInfo gaugeArcInfo = new GaugeArcInfo()
            {
                FillPaint = gradient,
                ArcPath = new PathF(),
                StartAngle = (float)startAngle,
                EndAngle = (float)endAngle,
                InnerStartRadius = innerStartRadius,
                InnerEndRadius = innerEndRadius
            };

            return gaugeArcInfo;
        }

        /// <summary>
        /// Calculate gradient arc offset.
        /// </summary>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        private void CalculateGradientArcOffset(double startAngle, double endAngle, out Point startPoint, out Point endPoint)
        {
            var start = startAngle > 360 ? startAngle % 360 : startAngle;

            start += this.IsInversed ? -0.5 : 0.5;// Added .5 degree at the end angle to avoid line difference.

            var end = endAngle > 360 ? endAngle % 360 : endAngle;

            if (start >= 0 && start < 90)
            {
                if (this.IsInversed)
                {
                    startPoint = new Point(0.5, 1);
                    endPoint = new Point(0.5, 0);
                }
                else
                {
                    startPoint = new Point(1, 0.5);
                    endPoint = new Point(0, 0.5);
                }
            }
            else if (start >= 90 && start < 180)
            {
                if (this.IsInversed)
                {
                    startPoint = new Point(0, 0.5);
                    endPoint = new Point(1, 0.5);
                }
                else
                {
                    startPoint = new Point(0.5, 1);
                    endPoint = new Point(0.5, 0);
                }
            }
            else if (start >= 180 && start < 270)
            {
                if (this.IsInversed)
                {
                    startPoint = new Point(0.5, 0);
                    endPoint = new Point(0.5, 1);
                }
                else
                {
                    if (end >= 180 && end < 270)
                    {
                        startPoint = new Point(0.5, 1);
                        endPoint = new Point(0.5, 0);
                    }
                    else
                    {
                        startPoint = new Point(0, 0.5);
                        endPoint = new Point(1, 0.5);
                    }
                }
            }
            else
            {
                if (this.IsInversed)
                {
                    if (end >= 270 && end < 360)
                    {
                        startPoint = new Point(0.5, 1);
                        endPoint = new Point(0.5, 0);
                    }
                    else
                    {
                        startPoint = new Point(1, 0.5);
                        endPoint = new Point(0, 0.5);
                    }
                }
                else
                {
                    startPoint = new Point(0.5, 0);
                    endPoint = new Point(0.5, 1);
                }
            }
        }

        /// <summary>
        /// Create axis line path start curve.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="arcStartAngle"></param>
        /// <param name="startCurveCapCenter"></param>
        /// <param name="innerRadius"></param>
        /// <param name="lineHalfWidth"></param>
        private void CalculateAxisLineStartCurve(PathF path, double arcStartAngle, Point? startCurveCapCenter,
            double innerRadius, double lineHalfWidth)
        {
            if (path != null && startCurveCapCenter != null)
            {
                //Draw start angle curve.
                Point vector = Utility.AngleToVector(arcStartAngle);
                PointF startCurvePoint = new PointF((float)(this.Center.X + (innerRadius * vector.X)),
                    (float)(this.Center.Y + (innerRadius * vector.Y)));
#if WINDOWS
                //TODO : Here we moved the path before adding shapes to already closed path.
                //We don't need this move for other platforms. We reported this problem in below link. 
                //https://github.com/dotnet/maui/issues/3507
                //Once the problem resolved, we need to remove this part.

                path.MoveTo(startCurvePoint);
#endif
                path.AddArc((float)(startCurveCapCenter.Value.X - lineHalfWidth),
                    (float)(startCurveCapCenter.Value.Y - lineHalfWidth),
                    (float)(startCurveCapCenter.Value.X + lineHalfWidth),
                    (float)(startCurveCapCenter.Value.Y + lineHalfWidth),
                    -(float)(arcStartAngle + 180), -(float)arcStartAngle, true);

                path.LineTo(startCurvePoint);
                path.Close();
            }
        }

        /// <summary>
        /// Create axis line path end curve.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="arcEndAngle"></param>
        /// <param name="endCurveCapCenter"></param>
        /// <param name="outerRadius"></param>
        /// <param name="lineHalfWidth"></param>
        private void CalculateAxisLineEndCurve(PathF path, double arcEndAngle, Point? endCurveCapCenter,
            double outerRadius, double lineHalfWidth)
        {
            if (path != null && endCurveCapCenter != null)
            {
                //Draw end angle curve. 
                Point vector = Utility.AngleToVector(arcEndAngle);
                PointF endCurvePoint = new PointF((float)(this.Center.X + (outerRadius * vector.X)),
                    (float)(this.Center.Y + (outerRadius * vector.Y)));
#if WINDOWS
                //TODO : Here we moved the path before adding shapes to already closed path.
                //We don't need this move for other platforms. We reported this problem in below link. 
                //https://github.com/dotnet/maui/issues/3507
                //Once the problem resolved, we need to remove this part.

                path.MoveTo(endCurvePoint);
#endif
                path.AddArc((float)(endCurveCapCenter.Value.X - lineHalfWidth),
                    (float)(endCurveCapCenter.Value.Y - lineHalfWidth),
                    (float)(endCurveCapCenter.Value.X + lineHalfWidth),
                    (float)(endCurveCapCenter.Value.Y + lineHalfWidth),
                    -(float)arcEndAngle, -(float)(arcEndAngle + 180), true);

                path.LineTo(endCurvePoint);
                path.Close();
            }
        }

        /// <summary>
        /// To measure the axis labels
        /// </summary>
        private void MeasureLabels()
        {
            if (this.VisibleLabels != null)
            {
                double maximumWidth = 0, maximumHeight = 0;

                foreach (var labelInfo in this.VisibleLabels)
                {
                    if (labelInfo.LabelStyle == null && this.AxisLabelStyle != null)
                    {
                        labelInfo.LabelStyle = this.AxisLabelStyle;
                    }

                    if (labelInfo.LabelStyle != null)
                    {
                        labelInfo.DesiredSize = labelInfo.Text.Measure(labelInfo.LabelStyle);
                    }

                    maximumHeight = Math.Max(maximumHeight, labelInfo.DesiredSize.Height);
                    maximumWidth = Math.Max(maximumWidth, labelInfo.DesiredSize.Width);
                    if (AvailableSize.Height <= AvailableSize.Width)
                    {
                        this.LabelMaximumSize = maximumHeight;
                    }
                    else
                    {
                        this.LabelMaximumSize = maximumWidth;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current axis labels
        /// </summary>
        /// <param name="value">The value of the label.</param>
        /// <returns>The corresponding axis label of given value.</returns>
        private GaugeLabelInfo GetAxisLabel(double value)
        {
            GaugeLabelInfo label = new GaugeLabelInfo
            {
                Value = value
            };
            string labelText = value.ToString(this.LabelFormat, CultureInfo.CurrentCulture);
            label.Text = labelText;
            this.RaiseLabelCreated(label);
            return label;
        }

        /// <summary>
        /// This method is used to calculate the bounds
        /// </summary>
        private void CalculateRadius()
        {
            this.Radius = Math.Min(this.AvailableSize.Width, this.AvailableSize.Height) * 0.5;

            this.Center = this.GetCenter(this.Radius);
            if (CanScaleToFit)
            {
                double diff;
                double centerYDiff = Math.Abs((this.AvailableSize.Height / 2) - this.Center.Y);
                double centerXDiff = Math.Abs((this.AvailableSize.Width / 2) - this.Center.X);
                if (this.AvailableSize.Width > this.AvailableSize.Height)
                {
                    diff = centerYDiff / 2;
                    double radius = (this.AvailableSize.Height / 2) + diff;

                    if ((this.AvailableSize.Width / 2) < radius)
                    {
                        double actualDiff = (this.AvailableSize.Width / 2) - (this.AvailableSize.Height / 2);
                        diff = actualDiff * 0.7f;
                    }
                }
                else
                {
                    diff = centerXDiff / 2;
                    double radius = (this.AvailableSize.Width / 2) + diff;

                    if (this.AvailableSize.Height / 2 < radius)
                    {
                        double actualDiff = (this.AvailableSize.Height / 2) - (this.AvailableSize.Width / 2);
                        diff = actualDiff * 0.7f;
                    }
                }

                this.Radius += diff;
            }
        }

        /// <summary>
        /// To validate Start and End Angle.
        /// </summary>
        private void ValidateStartEndAngle()
        {
            var start = double.IsNaN(this.StartAngle) ? 0 : this.StartAngle > 360 ? this.StartAngle % 360 : this.StartAngle;
            var end = double.IsNaN(this.EndAngle) ? 0 : this.EndAngle > 360 ? this.EndAngle % 360 : this.EndAngle;
            this.ActualStartAngle = start;
            end = (end - start) % 360 == 0 ? end - 0.01 : end;
            while (end < start)
            {
                end += 360;
            }

            this.ActualEndAngle = end;
            this.ActualSweepAngle = Utility.CalculateSweepAngle(this.ActualStartAngle, this.ActualEndAngle);
        }

        /// <summary>
        /// To validate Minimum and Maximum.
        /// </summary>
        private void ValidateMinimumMaximum()
        {
            double minimum = Minimum;
            double maximum = Maximum;
            Utility.ValidateMinimumMaximumValue(ref minimum, ref maximum);
            ActualMinimum = minimum;
            ActualMaximum = maximum;
        }

        /// <summary>
        /// Calculate actual interval for calculation. 
        /// </summary>
        /// <returns></returns>
        private double CalculateActualInterval()
        {
            return double.IsNaN(this.Interval) ? this.CalculateNiceInterval() : this.Interval;
        }

        /// <summary>
        /// Calculate axis's nice interval for axis labels and tick positions. 
        /// </summary>
        /// <returns></returns>
        private double CalculateNiceInterval()
        {
            double circumference = 2 * Math.PI * Math.Min(this.AvailableSize.Width / 2, this.AvailableSize.Height / 2) * (this.ActualSweepAngle / 360);
            double desiredIntervalCount = Math.Max(circumference * (0.533 * this.MaximumLabelsCount / 100), 1);
            double niceInterval = this.GetActualRange() / desiredIntervalCount;
            double minimumInterval = Math.Pow(10, Math.Floor(Math.Log(niceInterval) / Math.Log(10)));
            List<double> intervalDivisions = new List<double>() { 10, 5, 2, 1 };
            for (int i = 0; i < intervalDivisions.Count; i++)
            {
                double currentInterval = minimumInterval * intervalDivisions[i];
                if (desiredIntervalCount < (this.GetActualRange() / currentInterval))
                {
                    break;
                }

                niceInterval = currentInterval;
            }

            return niceInterval;
        }

        /// <summary>
        /// Get the axis range based on its minimum and maximum
        /// </summary>
        /// <returns>Returns the axis range based on its minimum and maximum</returns>
        internal double GetActualRange()
        {
            return this.ActualMaximum - this.ActualMinimum;
        }

        /// <summary>
        /// Calculate actual center for radial axis. 
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="region"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private Point CalculateActualCenter(Point centerPoint, List<int> region, double radius)
        {
            var startRadian = 2 * Math.PI * this.ActualStartAngle / 360;
            var endRadian = 2 * Math.PI * this.ActualEndAngle / 360;
            Point startPoint = new Point(centerPoint.X + (radius * Math.Cos(startRadian)), centerPoint.Y + (radius * Math.Sin(startRadian)));
            Point endPoint = new Point(centerPoint.X + (radius * Math.Cos(endRadian)), centerPoint.Y + (radius * Math.Sin(endRadian)));
            Point actualCenter = centerPoint;

            switch (region.Count)
            {
                case 0:
                    var longX = Math.Abs(centerPoint.X - startPoint.X) > Math.Abs(centerPoint.X - endPoint.X) ? startPoint.X : endPoint.X;
                    var longY = Math.Abs(centerPoint.Y - startPoint.Y) > Math.Abs(centerPoint.Y - endPoint.Y) ? startPoint.Y : endPoint.Y;
                    var midPoint = new Point(Math.Abs(centerPoint.X + longX) / 2, Math.Abs(centerPoint.Y + longY) / 2);
                    actualCenter.X = centerPoint.X + (centerPoint.X - midPoint.X);
                    actualCenter.Y = centerPoint.Y + (centerPoint.Y - midPoint.Y);
                    break;

                case 1:
                    midPoint = CalculateRegionMidPoint(startPoint, endPoint, centerPoint, region[0]);
                    actualCenter.X = centerPoint.X + ((centerPoint.X - midPoint.X) >= radius ? 0 : (centerPoint.X - midPoint.X));
                    actualCenter.Y = centerPoint.Y + ((centerPoint.Y - midPoint.Y) >= radius ? 0 : (centerPoint.Y - midPoint.Y));
                    break;

                case 2:
                    midPoint = CalculateRegionMidPoint(startPoint, endPoint, centerPoint, region[0], region[1]);
                    actualCenter.X = centerPoint.X + (midPoint.X == 0 ? 0 : (centerPoint.X - midPoint.X) >= radius ? 0 : (centerPoint.X - midPoint.X));
                    actualCenter.Y = centerPoint.Y + (midPoint.Y == 0 ? 0 : (centerPoint.Y - midPoint.Y) >= radius ? 0 : (centerPoint.Y - midPoint.Y));
                    break;

                case 3:
                    midPoint = CalculateRegionMidPoint(startPoint, endPoint, centerPoint, region[0], region[1], region[2]);
                    actualCenter.X = centerPoint.X + (midPoint.X == 0 ? 0 : (centerPoint.X - midPoint.X) >= radius ? 0 : (centerPoint.X - midPoint.X));
                    actualCenter.Y = centerPoint.Y + (midPoint.Y == 0 ? 0 : (centerPoint.Y - midPoint.Y) >= radius ? 0 : (centerPoint.Y - midPoint.Y));
                    break;
            }

            return actualCenter;
        }

        /// <summary>
        /// Calculate region mid point for center calculation. 
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="centerPoint"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        private Point CalculateRegionMidPoint(Point startPoint, Point endPoint, Point centerPoint, int region)
        {
            Point point1 = new Point(), point2 = new Point();
            var maxRadian = 2 * Math.PI * region / 360;
            var maxPoint = new Point(centerPoint.X + (radius * Math.Cos(maxRadian)), centerPoint.Y + (radius * Math.Sin(maxRadian)));
            switch (region)
            {
                case 270:
                    point1 = new Point(startPoint.X, maxPoint.Y);
                    point2 = new Point(endPoint.X, centerPoint.Y);
                    break;
                case 0:
                case 360:
                    point1 = new Point(centerPoint.X, endPoint.Y);
                    point2 = new Point(maxPoint.X, startPoint.Y);
                    break;
                case 90:
                    point1 = new Point(endPoint.X, centerPoint.Y);
                    point2 = new Point(startPoint.X, maxPoint.Y);
                    break;
                case 180:
                    point1 = new Point(maxPoint.X, startPoint.Y);
                    point2 = new Point(centerPoint.X, endPoint.Y);
                    break;
            }

            return new Point((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
        }

        /// <summary>
        /// Calculate region mid point for center calculation. 
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="centerPoint"></param>
        /// <param name="region1"></param>
        /// <param name="region2"></param>
        /// <returns></returns>
        private Point CalculateRegionMidPoint(Point startPoint, Point endPoint, Point centerPoint, int region1, int region2)
        {
            Point point1, point2;
            var minRadian = 2 * Math.PI * region1 / 360;
            var maxRadian = 2 * Math.PI * region2 / 360;
            var maxPoint = new Point(centerPoint.X + (radius * Math.Cos(maxRadian)), centerPoint.Y + (radius * Math.Sin(maxRadian)));
            Point minPoint = new Point(centerPoint.X + (radius * Math.Cos(minRadian)), centerPoint.Y + (radius * Math.Sin(minRadian)));
            if ((region1 == 0 && region2 == 90) || (region1 == 180 && region2 == 270))
            {
                point1 = new Point(minPoint.X, maxPoint.Y);
            }
            else
            {
                point1 = new Point(maxPoint.X, minPoint.Y);
            }

            if (region1 == 0 || region1 == 180)
            {
                point2 = new Point(Utility.GetMinMaxValue(startPoint, endPoint, region1), Utility.GetMinMaxValue(startPoint, endPoint, region2));
            }
            else
            {
                point2 = new Point(Utility.GetMinMaxValue(startPoint, endPoint, region2), Utility.GetMinMaxValue(startPoint, endPoint, region1));
            }

            return new Point(Math.Abs(point1.X - point2.X) / 2 >= radius ? 0 : (point1.X + point2.X) / 2, y: Math.Abs(point1.Y - point2.Y) / 2 >= radius ? 0 : (point1.Y + point2.Y) / 2);

        }

        /// <summary>
        /// Calculate region mid point for center calculation. 
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="centerPoint"></param>
        /// <param name="region1"></param>
        /// <param name="region2"></param>
        /// <param name="region3"></param>
        /// <returns></returns>
        private Point CalculateRegionMidPoint(Point startPoint, Point endPoint, Point centerPoint, int region1, int region2, int region3)
        {
            float region0Radian = (float)(2 * Math.PI * region1 / 360);
            float region1Radian = (float)(2 * Math.PI * region2 / 360);
            float region2Radian = (float)(2 * Math.PI * region3 / 360);
            Point region0Point = new Point((float)(centerPoint.X + (radius * Math.Cos(region0Radian))), (float)(centerPoint.Y + (radius * Math.Sin(region0Radian))));
            Point region1Point = new Point((float)(centerPoint.X + (radius * Math.Cos(region1Radian))), (float)(centerPoint.Y + (radius * Math.Sin(region1Radian))));
            Point region2Point = new Point((float)(centerPoint.X + (radius * Math.Cos(region2Radian))), (float)(centerPoint.Y + (radius * Math.Sin(region2Radian))));
            Point regionPoint1 = new Point(), regionPoint2 = new Point();
            switch (region3)
            {
                case 0:
                case 360:
                    regionPoint1 = new Point(region0Point.X, region1Point.Y);
                    regionPoint2 = new Point(region2Point.X, Math.Max(startPoint.Y, endPoint.Y));
                    break;
                case 90:
                    regionPoint1 = new Point(Math.Min(startPoint.X, endPoint.X), region0Point.Y);
                    regionPoint2 = new Point(region1Point.X, region2Point.Y);
                    break;
                case 180:
                    regionPoint1 = new Point(region2Point.X, Math.Min(startPoint.Y, endPoint.Y));
                    regionPoint2 = new Point(region0Point.X, region1Point.Y);
                    break;
                case 270:
                    regionPoint1 = new Point(region1Point.X, region2Point.Y);
                    regionPoint2 = new Point(Math.Max(startPoint.X, endPoint.X), region0Point.Y);
                    break;
            }

            return new Point(Math.Abs(regionPoint1.X - regionPoint2.X) / 2 >= radius ? 0 : (regionPoint1.X + regionPoint2.X) / 2,
                Math.Abs(regionPoint1.Y - regionPoint2.Y) / 2 >= radius ? 0 : (regionPoint1.Y + regionPoint2.Y) / 2);

        }

        /// <summary>
        /// Calculate offset pixel value. 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="sizeUnit"></param>
        /// <returns></returns>
        private double CalculateOffsetValue(double offset, SizeUnit sizeUnit)
        {
            double value = 0;
            switch (sizeUnit)
            {
                case SizeUnit.Factor:
                    offset = Math.Clamp(offset, 0, 1);
                    value = offset * this.Radius;
                    break;
                case SizeUnit.Pixel:
                    value = offset;
                    break;
            }

            return value;
        }

        /// <summary>
        /// To add minor tick points
        /// </summary>
        /// <param name="position">The position</param>
        private void AddMinorTicksPoint(double position)
        {
            var tickInterval = this.ActualInterval / (this.MinorTicksPerInterval + 1);
            var tickPosition = position + tickInterval;
            position += this.ActualInterval;
            while (tickPosition < position && tickPosition <= this.ActualMaximum)
            {
                if (this.ActualMinimum <= tickPosition && tickPosition <= this.ActualMaximum)
                {
                    this.MinorTickPositions.Add(new AxisTickInfo() { Value = tickPosition });
                }

                tickPosition += tickInterval;

                // While adding two decimal digits, some minute difference get added.
                // For example while adding 0.2 + 0.4 results 0.60000000000000009
                // Due to this, additional minor ticks get added and overlapped with the major ticks. In order to avoid this rounded the result.
                tickPosition = Math.Round(tickPosition, 8);
            }
        }

        /// <summary>
        /// To update axis sub elements (ranges and pointers).
        /// </summary>
        private void UpdateAxisSubElements()
        {
            this.CreateRanges();
            this.CreatePointers();
            this.CreateAnnotations();
        }

        /// <summary>
        /// To create the ranges.
        /// </summary>
        private void CreateRanges()
        {
            foreach (RadialRange radialRange in this.Ranges)
            {
                if(CanAnimate)
                    radialRange.RangeView.Opacity = 0;

                radialRange.CreateRangeArc();
            }
        }

        /// <summary>
        /// To create the pointers.
        /// </summary>
        private void CreatePointers()
        {
            foreach (RadialPointer pointer in this.Pointers)
            {
                if (CanAnimate && pointer is MarkerPointer markerPointer && markerPointer.CustomView != null)
                    markerPointer.CustomView.Opacity = 0;

                pointer.CreatePointer();
            }
        }

        /// <summary>
        /// To create the pointers.
        /// </summary>
        private void CreateAnnotations()
        {
            foreach (GaugeAnnotation gaugeAnnotation in this.Annotations)
            {
                if (CanAnimate)
                    gaugeAnnotation.Content.Opacity = 0;

                gaugeAnnotation.CreateAnnotation();
            }
        }

        /// <summary>
        /// Method used to invalidate axis. 
        /// </summary>
        private void InvalidateAxis()
        {
            this.InvalidateDrawable();

            foreach (var range in Ranges)
            {
                range.InvalidateDrawable();
            }

            foreach (var pointer in Pointers)
            {
                pointer.InvalidateDrawable();
            }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when <see cref="AxisLineStyle"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void AxisLineStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == RadialLineStyle.ThicknessProperty.PropertyName || e.PropertyName == RadialLineStyle.ThicknessUnitProperty.PropertyName)
            {
                this.ActualAxisLineWidth = this.CalculateActualSize(AxisLineStyle.Thickness, AxisLineStyle.ThicknessUnit, false);
                this.UpdateAxisElements();
            }
            else if (e.PropertyName == RadialLineStyle.CornerStyleProperty.PropertyName || e.PropertyName == RadialLineStyle.DashArrayProperty.PropertyName)
            {
                this.CalculateAxisLine();
            }
            else if (e.PropertyName == RadialLineStyle.GradientStopsProperty.PropertyName)
            {
#nullable disable
                if (this.AxisLineStyle.GradientStops is ObservableCollection<GaugeGradientStop> gradientStops)
                {
                    gradientStops.CollectionChanged += this.GradientStops_CollectionChanged;
                }
#nullable enable
                this.CalculateAxisLine();
            }

            this.InvalidateDrawable();
        }

        /// <summary>
        /// Called when <see cref="AxisLabelStyle"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void GaugeLabelStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GaugeLabelStyle.TextColorProperty.PropertyName)
            {
                this.InvalidateDrawable();
            }
            else if (e.PropertyName == GaugeLabelStyle.FontAttributesProperty.PropertyName || e.PropertyName == GaugeLabelStyle.FontFamilyProperty.PropertyName || e.PropertyName == GaugeLabelStyle.FontSizeProperty.PropertyName)
            {
                if (this.LabelPosition == GaugeLabelsPosition.Inside)
                {
                    this.UpdateAxisElements();
                    this.InvalidateDrawable();
                }
                else
                {
                    this.UpdateAxis();
                    this.InvalidateAxis();
                }
            }
        }

        /// <summary>
        /// Called when <see cref="MinorTickStyle"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void AxisMinorStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                AxisTicStyle_PropertyChanged(e.PropertyName, false);
            }
        }

        /// <summary>
        /// Called when <see cref="MajorTickStyle"/> property got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The PropertyChangedEventArgs.</param>
        private void AxisMajorStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != null)
            {
                AxisTicStyle_PropertyChanged(e.PropertyName, true);
            }
        }

        /// <summary>
        /// Called when <see cref="MajorTickStyle"/> or <see cref="MinorTickStyle"/> property got changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="isMajorTick">Boolean to identify major tick or not.</param>
        private void AxisTicStyle_PropertyChanged(string propertyName, bool isMajorTick)
        {
            if (propertyName == RadialTickStyle.LengthUnitProperty.PropertyName || propertyName == RadialTickStyle.LengthProperty.PropertyName)
            {
                if (isMajorTick)
                {
                    this.ActualMajorTickLength = this.CalculateActualSize(this.MajorTickStyle.Length, this.MajorTickStyle.LengthUnit, false);
                }
                else
                {
                    this.ActualMinorTickLength = this.CalculateActualSize(this.MinorTickStyle.Length, this.MinorTickStyle.LengthUnit, false);
                }

                if (this.TickPosition != GaugeElementPosition.Inside)
                {
                    this.UpdateAxis();
                    this.InvalidateAxis();
                }
                else
                {
                    this.UpdateAxisElements();
                    this.InvalidateDrawable();
                }
            }
            else if (propertyName == GaugeTickStyle.StrokeThicknessProperty.PropertyName)
            {
                this.UpdateAxisElements();
                this.InvalidateDrawable();
            }
            else
            {
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="Ranges"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void Ranges_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems != null && e.NewItems.Count > 0)
                    {
                        foreach (RadialRange radialRange in e.NewItems)
                        {
                            radialRange.RadialAxis = this;
                            if (!this.RangesGrid.Contains(radialRange.RangeView))
                                this.RangesGrid.Add(radialRange.RangeView);

                            if (!this.AvailableSize.IsZero)
                            {
                                SetInheritedBindingContext(radialRange, this.BindingContext);
                                radialRange.CreateRangeArc();
                            }
                        }

                        if (this.RangesGrid.Children.Count > 0 && !this.ParentGrid.Children.Contains(RangesGrid))
                        {
                            this.ParentGrid.Children.Add(RangesGrid);
                        }
                    }

                    if (e.OldItems != null && e.OldItems.Count > 0)
                    {
                        foreach (RadialRange radialRange in e.OldItems)
                        {
                            radialRange.RadialAxis = null;

                            if (this.RangesGrid.Children.Contains(radialRange.RangeView))
                            {
                                this.RangesGrid.Children.Remove(radialRange.RangeView);
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    this.RangesGrid.Children.Clear();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Called when <see cref="Pointers"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void Pointers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems != null && e.NewItems.Count > 0)
                    {
                        foreach (RadialPointer radialPointer in e.NewItems)
                        {
                            radialPointer.RadialAxis = this;
                            radialPointer.CanAnimate = true;

                            if (!this.PointersGrid.Children.Contains(radialPointer.PointerView))
                            {
                                this.PointersGrid.Children.Add(radialPointer.PointerView);
                            }

                            if (radialPointer is MarkerPointer markerPointer && markerPointer.CustomView != null)
                            {
                                markerPointer.AddCustomView(markerPointer.CustomView);

                                if (this.AnnotationsLayout.Children.Count > 0 && !this.ParentGrid.Children.Contains(this.AnnotationsLayout))
                                {
                                    this.ParentGrid.Children.Add(this.AnnotationsLayout);
                                }
                            }
                            else if (this.PointersGrid.Children.Count > 0 && !this.ParentGrid.Children.Contains(PointersGrid))
                            {
                                this.ParentGrid.Children.Add(PointersGrid);
                            }

                            if (!this.AvailableSize.IsZero)
                            {
                                SetInheritedBindingContext(radialPointer, this.BindingContext);
                                radialPointer.CreatePointer();
                            }
                        }
                    }

                    if (e.OldItems != null && e.OldItems.Count > 0)
                    {
                        foreach (RadialPointer radialPointer in e.OldItems)
                        {
                            radialPointer.RadialAxis = null;

                            if (this.PointersGrid.Children.Contains(radialPointer.PointerView))
                            {
                                this.PointersGrid.Children.Remove(radialPointer.PointerView);
                            }

                            if (radialPointer is MarkerPointer markerPointer && markerPointer.CustomView != null)
                            {
                                markerPointer.RemoveCustomView(markerPointer.CustomView);
                            }
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.PointersGrid.Children.Clear();
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Called when <see cref="Annotations"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void Annotations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Replace:
                    if (e.NewItems != null && e.NewItems.Count > 0)
                    {
                        foreach (GaugeAnnotation gaugeAnnotation in e.NewItems)
                        {
                            gaugeAnnotation.RadialAxis = this;

                            if (!this.AnnotationsLayout.Children.Contains(gaugeAnnotation.Content))
                            {
                                this.AnnotationsLayout.Children.Add(gaugeAnnotation.Content);
                            }

                            if (!this.AvailableSize.IsZero)
                            {
                                SetInheritedBindingContext(gaugeAnnotation, this.BindingContext);
                                SetInheritedBindingContext(gaugeAnnotation.Content, this.BindingContext);
                                gaugeAnnotation.CreateAnnotation();
                            }

                            if (this.AnnotationsLayout.Children.Count > 0 && !this.ParentGrid.Children.Contains(this.AnnotationsLayout))
                            {
                                this.ParentGrid.Children.Add(this.AnnotationsLayout);
                            }
                        }
                    }

                    if (e.OldItems != null && e.OldItems.Count > 0)
                    {
                        foreach (GaugeAnnotation gaugeAnnotation in e.OldItems)
                        {
                            gaugeAnnotation.RadialAxis = null;

                            if (this.AnnotationsLayout.Children.Contains(gaugeAnnotation.Content))
                            {
                                this.AnnotationsLayout.Children.Remove(gaugeAnnotation.Content);
                                gaugeAnnotation.UnHookMeasureInvalidated();
                            }
                        }
                    }
                    break;
               
                case NotifyCollectionChangedAction.Reset:
                    this.AnnotationsLayout.Children.Clear();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Called when <see cref="RadialLineStyle.GradientStops"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void GradientStops_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!this.AvailableSize.IsZero)
            {
                this.CalculateAxisLine();
                this.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Method used to draw text.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="label"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="textElement"></param>
        private static void DrawText(ICanvas canvas, string label, float x, float y, ITextElement textElement)
        {
            canvas.DrawText(label, x, y, textElement);
        }

        #endregion

        #region Loading animation

        /// <summary>
        /// Method used to start axis line load time animation.
        /// </summary>
        internal void PerformLoadingAnimation()
        {
            //Prepare loading animation section list.
            List<string> list = new List<string>();
            if (this.ShowAxisLine)
                list.Add("AxisLine");
            if (this.ShowTicks || this.ShowLabels)
                list.Add("AxisTicksAndLabels");
            if (this.Ranges.Count > 0)
                list.Add("Ranges");
            if (this.Pointers.Count > 0)
                list.Add("Pointers");
            if (this.Annotations.Count > 0)
                list.Add("Annotations");

            if (list.Count == 0) return;

            //Calculate animation duration factor for each section.
            double animationDurationFactor = 1d / list.Count;
            double animationBegin = 0;

            var parentAnimation = new Animation();

            //Create axis line animation.
            if (list.Contains("AxisLine"))
            {
                double startAngle = this.ActualStartAngle;
                double endAngle = this.ActualEndAngle;
                if (this.IsInversed)
                {
                    double temp = endAngle;
                    endAngle = startAngle;
                    startAngle = temp;
                }

                var animation = new Animation(this.OnAxisLineAnimationUpdate, startAngle, endAngle,
                Easing.Linear, null);

                double animationEnd = animationBegin + animationDurationFactor;
                animationEnd = animationEnd > 1 ? 1 : animationEnd;
                parentAnimation.Add(animationBegin, animationEnd, animation); ;
                animationBegin = animationEnd;
            }

            //Create axis ticks and labels animation.
            if (list.Contains("AxisTicksAndLabels"))
            {
                var animation = new Animation(this.OnAxisTicksAndLabelsAnimationUpdate, 0, 1,
                Easing.Linear, null);

                double animationEnd = animationBegin + animationDurationFactor;
                animationEnd = animationEnd > 1 ? 1 : animationEnd;
                parentAnimation.Add(animationBegin, animationEnd, animation); 
                animationBegin = animationEnd;
            }

            //Create range animation.
            if (list.Contains("Ranges"))
            {
                var animation = new Animation(this.OnAxisRangeAnimationUpdate, 0, 1,
                Easing.Linear, null);

                double animationEnd = animationBegin + animationDurationFactor;
                animationEnd = animationEnd > 1 ? 1 : animationEnd;
                parentAnimation.Add(animationBegin, animationEnd, animation);
                animationBegin = animationEnd;
            }

            //Create pointers animation.
            if (list.Contains("Pointers"))
            {
                double animationEnd = animationBegin + animationDurationFactor;
                animationEnd = animationEnd > 1 ? 1 : animationEnd;

                foreach (var pointer in Pointers)
                {
                    var animation = new Animation(pointer.OnAnimationUpdate, this.ActualMinimum,
                        pointer.Value, Easing.Linear, null);
                    pointer.CanAnimate = false;
                    parentAnimation.Add(animationBegin, animationEnd, animation); ;

                }
                animationBegin = animationEnd;
            }

            //Create annotations animation.
            if (list.Contains("Annotations"))
            {
                var animation = new Animation(this.OnAnnotationAnimationUpdate, 0, 1,
                Easing.Linear, null);

                double animationEnd = animationBegin + animationDurationFactor;
                animationEnd = animationEnd > 1 ? 1 : animationEnd;
                parentAnimation.Add(animationBegin, animationEnd, animation); 
            }

            //Start loading animation.
            parentAnimation.Commit(this, "GaugeLoadingAnimation", 16, (uint)AnimationDuration,
                Easing.Linear, OnLoadingAnimationFinished, null);
        }

        /// <summary>
        /// Update axis line animation value.
        /// </summary>
        /// <param name="value">Represents animation value.</param>
        private void OnAxisLineAnimationUpdate(double value)
        {
            //Update angle for loading animation.
            if (this.IsInversed)
                this.ActualStartAngle = value;
            else
                this.ActualEndAngle = value;

            this.ActualSweepAngle = Utility.CalculateSweepAngle(this.ActualStartAngle, this.ActualEndAngle);
            this.CalculateAxisLine();
            this.InvalidateDrawable();
        }

        /// <summary>
        /// Update axis ticks and labels animation value.
        /// </summary>
        /// <param name="value">Represents animation value.</param>
        private void OnAxisTicksAndLabelsAnimationUpdate(double value)
        {
            //Update factor value to render ticks and labels.
            this.AxisLoadingAnimationValue = value;
            this.InvalidateDrawable();
        }

        /// <summary>
        /// Update axis range animation value.
        /// </summary>
        /// <param name="value">Represents animation value.</param>
        private void OnAxisRangeAnimationUpdate(double value)
        {
            //Update ranges opacity for loading animation.
            foreach (var range in this.Ranges)
            {
                range.RangeView.Opacity = value;
            }
        }

        /// <summary>
        /// Update axis annotations animation value.
        /// </summary>
        /// <param name="value">Represents animation value.</param>
        private void OnAnnotationAnimationUpdate(double value)
        {
            //Update annotations opacity for loading animation.
            foreach (var annotation in this.Annotations)
            {
                annotation.Content.Opacity = value;
            }
        }

        /// <summary>
        /// Called when initial loading animation completed. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="isCompleted"></param>
        private void OnLoadingAnimationFinished(double value, bool isCompleted)
        {
            CanAnimate = false;
            AxisLoadingAnimationValue = null;
            foreach (var pointer in this.Pointers)
            {
                pointer.AnimationValue = null;
            }
            AnimationExtensions.AbortAnimation(this, "GaugeLoadingAnimation");
        }

        #endregion

        #endregion
    }
}
