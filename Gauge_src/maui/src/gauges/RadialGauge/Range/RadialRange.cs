using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Create the range to add color bar in the gauge.
    /// <see cref="RadialRange"/> is a visual that helps to quickly visualize
    /// where a value falls on the axis.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// <gauge:SfRadialGauge>
    ///     <gauge:SfRadialGauge.Axes>
    ///         <gauge:RadialAxis>
    ///             <gauge:RadialAxis.Ranges>
    ///                 <gauge:RadialRange StartValue="0" EndValue="35" Fill="Red" />
    ///                 <gauge:RadialRange StartValue="35" EndValue="75" Fill="Yellow"/>
    ///                 <gauge:RadialRange StartValue="75" EndValue="100" Fill="Green"/>
    ///             </gauge:RadialAxis.Ranges>
    ///         </gauge:RadialAxis>
    ///     </gauge:SfRadialGauge.Axes>
    /// </gauge:SfRadialGauge>
    /// ]]></code>
    /// </example>
    public class RadialRange : BindableObject
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="StartValue"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StartValue"/> bindable property.
        /// </value>
        public static readonly BindableProperty StartValueProperty =
            BindableProperty.Create(nameof(StartValue), typeof(double), typeof(RadialRange), 0d, propertyChanged: OnStartEndValueChanged);

        /// <summary>
        /// Identifies the <see cref="EndValue"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EndValue"/> bindable property.
        /// </value>
        public static readonly BindableProperty EndValueProperty =
            BindableProperty.Create(nameof(EndValue), typeof(double), typeof(RadialRange), 0d, propertyChanged: OnStartEndValueChanged);

        /// <summary>
        /// Identifies the <see cref="RangeOffset"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="RangeOffset"/> bindable property.
        /// </value>
        public static readonly BindableProperty RangeOffsetProperty =
           BindableProperty.Create(nameof(RangeOffset), typeof(double), typeof(RadialRange), double.NaN, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="OffsetUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OffsetUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty OffsetUnitProperty =
           BindableProperty.Create(nameof(OffsetUnit), typeof(SizeUnit), typeof(RadialRange), SizeUnit.Pixel, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="WidthUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="WidthUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty WidthUnitProperty =
           BindableProperty.Create(nameof(WidthUnit), typeof(SizeUnit), typeof(RadialRange), SizeUnit.Pixel, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="StartWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StartWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty StartWidthProperty =
            BindableProperty.Create(nameof(StartWidth), typeof(double), typeof(RadialRange), 10d, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EndWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="EndWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty EndWidthProperty =
            BindableProperty.Create(nameof(EndWidth), typeof(double), typeof(RadialRange), 10d, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Label"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Label"/> bindable property.
        /// </value>
        public static readonly BindableProperty LabelProperty =
            BindableProperty.Create(nameof(Label), typeof(string), typeof(RadialRange), null, propertyChanged: OnLabelPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="LabelStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LabelStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty LabelStyleProperty =
            BindableProperty.Create(nameof(LabelStyle), typeof(GaugeLabelStyle), typeof(RadialRange), null, propertyChanged: OnLabelStylePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(RadialRange), null, defaultValueCreator: bindable => new SolidColorBrush(Color.FromRgb(67, 160, 71)), propertyChanged: OnFillPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="GradientStops"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="GradientStops"/> bindable property.
        /// </value>
        public static readonly BindableProperty GradientStopsProperty =
         BindableProperty.Create(nameof(GradientStops), typeof(ObservableCollection<GaugeGradientStop>), typeof(RadialRange), null, propertyChanged: OnGradientStopsPropertyChanged);

        #endregion

        #region Fields

        /// <summary>
        /// <see cref="GaugeLabelInfo"/> instance.
        /// </summary>
        private GaugeLabelInfo labelInfo;

        /// <summary>
        /// <see cref="RadialRange"/> rendering path.
        /// </summary>
        private PathF? rangePath;

        /// <summary>
        /// Contains <see cref="RadialRange"/> gradient arcs.
        /// </summary>
        private List<GaugeArcInfo>? gradientArcPaths;

        private double outerRadius, innerStartRadius, innerEndRadius;
        private double outerArcStartAngle, outerArcEndAngle;

        /// <summary>
        /// Holds <see cref="RadialAxis"/> instance. 
        /// </summary>
        internal RadialAxis? RadialAxis;

        /// <summary>
        /// Represents actual start value.
        /// </summary>
        internal double ActualStartValue;

        /// <summary>
        /// Represents actual end value.
        /// </summary>
        internal double ActualEndValue;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialRange"/> class.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue="50" EndValue="100" />
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public RadialRange()
        {
            this.RangeView = new RangeView(this);
            this.labelInfo = new GaugeLabelInfo();
            this.LabelStyle = new GaugeLabelStyle();
            this.GradientStops = new ObservableCollection<GaugeGradientStop>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value that specifies the range start value.
        /// </summary>
        /// <value>
        /// It defines the start value of the range. The default value is <c>0</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue="50" />
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double StartValue
        {
            get { return (double)this.GetValue(StartValueProperty); }
            set { this.SetValue(StartValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the range end value.
        /// </summary>
        /// <value>
        /// It defines the end value of the range. The default value is <c>0</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange EndValue="100" />
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double EndValue
        {
            get { return (double)this.GetValue(EndValueProperty); }
            set { this.SetValue(EndValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the range position.
        /// You can specify value either in logical pixel or radius factor using the <see cref="OffsetUnit"/> property.
        /// </summary>
        /// <value>
        /// It defines offset of the range either in pixel or factor. The default value is <c>double.NaN</c>.
        /// </value>
        /// <example>
        /// If <see cref="WidthUnit"/> is <see cref="SizeUnit.Factor"/>, value will be given from 0 to 1. Here range placing position is calculated by <see cref="RangeOffset"/> * axis radius value.
        /// Example: <see cref="RangeOffset"/> value is 0.2 and axis radius is 100, range is moving 20(0.2 * 100) logical pixels from axis outer radius. If <see cref="WidthUnit"/> is <see cref="SizeUnit.Pixel"/>, the given value distance range moves from the outer radius axis.
        /// When you specify <see cref="RangeOffset"/> is negative, the range will be positioned outside the axis.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue="50" EndValue="100" RangeOffset="10" />
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double RangeOffset
        {
            get { return (double)this.GetValue(RangeOffsetProperty); }
            set { this.SetValue(RangeOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the to calculate the range offset either in logical pixel or factor.
        /// </summary>
        /// <value>
        /// One of the <see cref="SizeUnit"/> enumeration that specifies how the <see cref="RangeOffset"/> value is considered.
        /// The default mode is <see cref="SizeUnit.Pixel"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue="50" EndValue="100" RangeOffset="10" OffsetUnit="Factor" />
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
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
        /// Gets or sets the value that indicates the whether range start and end width can be calculated using logical pixel or radius factor.
        /// </summary>
        /// <value>
        /// One of the <see cref="SizeUnit"/> enumeration that specifies how the <see cref="StartWidth"/> and <see cref="EndWidth"/> values are considered.
        /// The default mode is <see cref="SizeUnit.Pixel"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue="50" EndValue="100" RangeOffset="10" OffsetUnit="Factor" SizeUnit="Factor" />
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public SizeUnit WidthUnit
        {
            get { return (SizeUnit)this.GetValue(WidthUnitProperty); }
            set { this.SetValue(WidthUnitProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the range start width. 
        /// You can specify value either in logical pixel or radius factor using the <see cref="WidthUnit"/> property.
        /// </summary>
        /// <value>
        /// It defines the start width of the range. The default value is <c>10</c>.
        /// </value>
        /// <example>
        /// If <see cref="WidthUnit"/> is <see cref="SizeUnit.Factor"/>, value must be given from 0 to 1. Here range start width is calculated by <see cref="StartWidth"/> * axis radius value.
        /// Example: <see cref="StartWidth"/> value is 0.2 and axis radius is 100, range start width is 20(0.2 * 100) logical pixels. if <see cref="WidthUnit"/> is <see cref="SizeUnit.Pixel"/>, the defined value is set for the start width of the range.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue="50"
        ///                                   EndValue="100"
        ///                                   RangeOffset="10"
        ///                                   OffsetUnit="Factor"
        ///                                   StartWidth="10"/>
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double StartWidth
        {
            get { return (double)this.GetValue(StartWidthProperty); }
            set { this.SetValue(StartWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the range end width.
        /// You can specify value either in logical pixel or radius factor using the <see cref="WidthUnit"/> property.
        /// </summary>
        /// <value>
        /// It defines the end width of the range. The default value is <c>10</c>.
        /// </value>
        /// <example>
        /// If <see cref="WidthUnit"/> is <see cref="SizeUnit.Factor"/>, value must be given from 0 to 1. Here range start width is calculated by <see cref="EndWidth"/> * axis radius value.
        /// Example: <see cref="EndWidth"/> value is 0.2 and axis radius is 100, range start width is 20(0.2 * 100) logical pixels. if <see cref="WidthUnit"/> is <see cref="SizeUnit.Pixel"/>, the defined value is set for the end width of the range.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue="50"
        ///                                   EndValue="100"
        ///                                   RangeOffset="10"
        ///                                   OffsetUnit="Factor"
        ///                                   StartWidth="10"/>
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double EndWidth
        {
            get { return (double)this.GetValue(EndWidthProperty); }
            set { this.SetValue(EndWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that specifies the text for range.
        /// </summary>
        /// <value>
        /// It defines the string for the range label. The default value is <c>null</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis ShowTicks="False"
        ///                           ShowLabels="False">
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue="0"
        ///                                   EndValue="30"
        ///                                   StartWidth="0.5"
        ///                                   EndWidth="0.5"
        ///                                   WidthUnit="Factor"
        ///                                   Fill="Red"
        ///                                   Label="Slow" />
        ///                 <gauge:RadialRange StartValue="30"
        ///                                   EndValue="70"
        ///                                   StartWidth="0.5"
        ///                                   EndWidth="0.5"
        ///                                   WidthUnit="Factor"
        ///                                   Fill="Yellow"
        ///                                   Label="Moderate" />
        ///                 <gauge:RadialRange StartValue="70"
        ///                                   EndValue="100"
        ///                                   StartWidth="0.5"
        ///                                   EndWidth="0.5"
        ///                                   WidthUnit="Factor"
        ///                                   Fill="Green"
        ///                                   Label="Fast" />
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public string Label
        {
            get { return (string)this.GetValue(LabelProperty); }
            set { this.SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Gets or sets <see cref="GaugeLabelStyle"/>, that helps to customize label in range. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///            <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue = "70" EndValue="100" Label="Syncfusion">
        ///                     <gauge:RadialRange.LabelStyle>
        ///                         <gauge:GaugeLabelStyle TextColor="Red" />
        ///                     </gauge:RadialRange.LabelStyle>
        ///                 </gauge:RadialRange>
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public GaugeLabelStyle LabelStyle
        {
            get { return (GaugeLabelStyle)this.GetValue(LabelStyleProperty); }
            set { this.SetValue(LabelStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Color that paints the interior area of the range.
        /// </summary>
        /// <value>
        /// A <c>Color</c> that specifies how the range is painted.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue="70"
        ///                                   EndValue="100"
        ///                                     Fill="Red" />
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public Brush Fill
        {
            get { return (Brush)this.GetValue(FillProperty); }
            set { this.SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of <see cref="GaugeGradientStop"/> to fill the gradient brush to the gauge range.
        /// </summary>
        /// <value>
        /// A collection of the <see cref="GaugeGradientStop"/> objects associated with the brush, each of which specifies a color and an offset along the axis.
        /// The default is an empty collection.
        /// </value>
        /// <example>
        /// The below examples shows, how to add a collection of gradient stop to the gauge range. 
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Ranges>
        ///                 <gauge:RadialRange StartValue="0"
        ///                                   EndValue="100">
        ///                     <gauge:RadialRange.GradientStops>
        ///                                 <gauge:GaugeGradientStop Value="0"
        ///                                                          Color="Green" />
        ///                                 <gauge:GaugeGradientStop Value= "50"
        ///                                                          Color="Yellow" />
        ///                                 <gauge:GaugeGradientStop Value="100"
        ///                                                          Color="Red" />
        ///                             </gauge:RadialRange.GradientStops>
        ///                 </gauge:RadialRange>
        ///             </gauge:RadialAxis.Ranges>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public ObservableCollection<GaugeGradientStop> GradientStops
        {
            get { return (ObservableCollection<GaugeGradientStop>)this.GetValue(GradientStopsProperty); }
            set { this.SetValue(GradientStopsProperty, value); }
        }

        /// <summary>
        /// Holds <see cref="Gauges.RangeView"/> instance. 
        /// </summary>
        internal RangeView RangeView { get; set; }

        #endregion

        #region Override methods

        /// <summary>
        /// Invoked whenever the binding context of the View changes.
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.LabelStyle != null)
            {
                SetInheritedBindingContext(this.LabelStyle, this.BindingContext);
            }

            if(this.GradientStops != null)
            {
                foreach (var gradientStop in this.GradientStops)
                    SetInheritedBindingContext(gradientStop, BindingContext);
            }
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// To create range arc. 
        /// </summary>
        internal void CreateRangeArc()
        {
            if (this.RadialAxis == null || this.RadialAxis.AvailableSize.IsZero)
            {
                return;
            }

            double startValue = this.StartValue;
            double endValue = this.EndValue;

            //For free hand rotation, checked 360 degree arc condition.
            if (this.RadialAxis.ActualSweepAngle != 359.99)
            {
                Utility.ValidateMinimumMaximumValue(ref startValue, ref endValue);
            }
            
            this.ActualStartValue = Math.Clamp(startValue, this.RadialAxis.ActualMinimum, this.RadialAxis.ActualMaximum);
            this.ActualEndValue = Math.Clamp(endValue, this.RadialAxis.ActualMinimum, this.RadialAxis.ActualMaximum);

            if (this.ActualStartValue == this.ActualEndValue)
            {
                this.gradientArcPaths = null;
                this.rangePath = null;
                return;
            }

            double? actualRangeOffset = null;
            PointF? innerArcTopLeft = null, innerArcBottomRight = null;
            float? innerArcStartAngle = null, innerArcEndAngle = null;
            double actualEndWidth = this.RadialAxis.CalculateActualSize(this.EndWidth, this.WidthUnit, false);
            double actualStartWidth = this.RadialAxis.CalculateActualSize(this.StartWidth, this.WidthUnit, false);

            if (!double.IsNaN(this.RangeOffset))
            {
                actualRangeOffset = this.RadialAxis.CalculateActualSize(this.RangeOffset, this.OffsetUnit, true);
            }

            if (actualRangeOffset == null)
            {
                outerRadius = (this.RadialAxis.Radius * this.RadialAxis.RadiusFactor) - this.RadialAxis.AxisLineRadiusDifference;
            }
            else
            {
                outerRadius = (double)actualRangeOffset * this.RadialAxis.RadiusFactor - this.RadialAxis.AxisLineRadiusDifference;
            }

            outerRadius = outerRadius < 0 ? 0 : outerRadius;

            //For free hand rotation, checked 360 degree arc condition.
            if (this.ActualStartValue > this.ActualEndValue && this.RadialAxis.ActualSweepAngle != 359.99)
            {
                innerStartRadius = outerRadius - actualEndWidth;
                innerEndRadius = outerRadius - actualStartWidth;
            }
            else
            {
                innerStartRadius = outerRadius - actualStartWidth;
                innerEndRadius = outerRadius - actualEndWidth;
            }

            innerStartRadius = innerStartRadius < 0 ? 0 : innerStartRadius;
            innerEndRadius = innerEndRadius < 0 ? 0 : innerEndRadius;

            //Calculating outer arc bounds.
            PointF outerArcTopLeft = new PointF((float)(this.RadialAxis.Center.X - outerRadius),
                (float)(this.RadialAxis.Center.Y - outerRadius));
            PointF outerArcBottomRight = new PointF((float)(this.RadialAxis.Center.X + outerRadius),
                (float)(this.RadialAxis.Center.Y + outerRadius));

            bool clockwise = this.RadialAxis.IsInversed;
            //Calculating start and end angle for outer arc.
            outerArcStartAngle = this.RadialAxis.ValueToAngle(this.ActualStartValue);
            outerArcEndAngle = this.RadialAxis.ValueToAngle(this.ActualEndValue);

            if (this.GradientStops != null && this.GradientStops.Count > 1 && this.ActualStartValue < this.ActualEndValue)
            {
                //Create gradient arc data collection.
                this.gradientArcPaths = this.RadialAxis.CreateGradientArcSegments(this.GradientStops.ToList(), innerStartRadius,
                    innerEndRadius, this.ActualStartValue, this.ActualEndValue);

                for (int i = 0; i < gradientArcPaths.Count; i++)
                {
                    GaugeArcInfo data = gradientArcPaths[i];

                    if (Math.Abs(innerEndRadius - innerStartRadius) == 0)
                    {
                        innerArcStartAngle = data.StartAngle;
                        innerArcEndAngle = data.EndAngle;

                        innerArcTopLeft = new PointF((float)(this.RadialAxis.Center.X - innerStartRadius),
                       (float)(this.RadialAxis.Center.Y - innerStartRadius));

                        innerArcBottomRight = new PointF((float)(this.RadialAxis.Center.X + innerEndRadius),
                            (float)(this.RadialAxis.Center.Y + innerEndRadius));
                    }
                    else
                    {
                        bool allowAngleValidation = true;

                        //In inner arc angle calculation, angle bounds updated with wrong value when outer arc beyond 360.
                        //To avoid this problem, stopped calculated inner arc validation. 
                        if ((data.StartAngle >= 360 && data.EndAngle < 360) || (data.EndAngle >= 360 && data.StartAngle < 360))
                        {
                            allowAngleValidation = false;
                        }

                        //Calculate gradient inner arc angles. 
                        var arcData = CalculateDifferentWidthInnerArc(data.StartAngle, data.EndAngle,
                        data.InnerStartRadius, data.InnerEndRadius, allowAngleValidation);

                        if (arcData != null)
                        {
                            innerArcTopLeft = arcData.Value.TopLeft;
                            innerArcBottomRight = arcData.Value.BottomRight;
                            innerArcStartAngle = arcData.Value.StartAngle;
                            innerArcEndAngle = arcData.Value.EndAngle;
                        }
                    }

                    if (innerArcTopLeft != null && innerArcBottomRight != null && innerArcStartAngle != null && innerArcEndAngle != null)
                    {
                        //Draw range outer arc. 
                        data.ArcPath.AddArc(outerArcTopLeft, outerArcBottomRight, -(float)data.EndAngle, -(float)data.StartAngle, clockwise);

                        //Draw range inner arc.
                        data.ArcPath.AddArc((PointF)innerArcTopLeft, (PointF)innerArcBottomRight, -(float)innerArcStartAngle, -(float)innerArcEndAngle, !clockwise);

                        data.ArcPath.Close();
                    }
                }
            }
            else
            {
                this.gradientArcPaths = null;

                //Calculate inner arc bounds.
                if (Math.Abs(innerEndRadius - innerStartRadius) > 0)
                {
                    GaugeArcInfo? arcInfo = CalculateDifferentWidthInnerArc(outerArcStartAngle, outerArcEndAngle, innerStartRadius, innerEndRadius);

                    if (arcInfo != null)
                    {
                        innerArcTopLeft = arcInfo.Value.TopLeft;
                        innerArcBottomRight = arcInfo.Value.BottomRight;
                        innerArcStartAngle = arcInfo.Value.StartAngle;
                        innerArcEndAngle = arcInfo.Value.EndAngle;
                    }
                }
                else
                {
                    innerArcStartAngle = (float)outerArcStartAngle;
                    innerArcEndAngle = (float)outerArcEndAngle;

                    innerArcTopLeft = new PointF((float)(this.RadialAxis.Center.X - innerStartRadius),
                   (float)(this.RadialAxis.Center.Y - innerStartRadius));

                    innerArcBottomRight = new PointF((float)(this.RadialAxis.Center.X + innerEndRadius),
                        (float)(this.RadialAxis.Center.Y + innerEndRadius));
                }

                if (innerArcTopLeft != null && innerArcBottomRight != null &&
            innerArcStartAngle != null && innerArcEndAngle != null)
                {
                    rangePath = new PathF();

                    //Draw range outer arc. 
                    rangePath.AddArc(outerArcTopLeft, outerArcBottomRight, -(float)outerArcEndAngle, -(float)outerArcStartAngle, clockwise);

                    //Draw range inner arc.
                    rangePath.AddArc((PointF)innerArcTopLeft, (PointF)innerArcBottomRight, -(float)innerArcStartAngle, -(float)innerArcEndAngle, !clockwise);

                    rangePath.Close();
                }
            }

            this.UpdateRangeLabelPosition();
        }

        /// <summary>
        /// Draw Range Arc.
        /// </summary>
        /// <param name="canvas">canvas</param>
        internal void DrawRange(ICanvas canvas)
        {
            if (this.RadialAxis != null)
            {
                canvas.SaveState();

                if (this.gradientArcPaths != null && this.gradientArcPaths.Count > 0)
                {
                    foreach (var path in gradientArcPaths)
                    {
                        canvas.SetFillPaint(path.FillPaint, path.ArcPath.Bounds);
                        canvas.FillPath(path.ArcPath);
                    }
                }
                else if (this.rangePath != null)
                {
                    //Setting range fill color.
                    if (this.GradientStops != null && (this.GradientStops.Count == 1 ||
                        (this.GradientStops.Count > 0 && this.ActualStartValue > this.ActualEndValue)))
                        canvas.FillColor = this.GradientStops[0].Color;
                    else
                        canvas.SetFillPaint(this.Fill, this.rangePath.Bounds);

                    canvas.FillPath(this.rangePath);
                }

                canvas.RestoreState();
            }
        }

        /// <summary>
        /// Draw range label.
        /// </summary>
        /// <param name="canvas">canvas</param>
        internal void DrawLabel(ICanvas canvas)
        {
            if (this.RadialAxis != null && !string.IsNullOrEmpty(this.Label))
            {
                canvas.SaveState();

                float labelPointX = (float)(labelInfo.Position.X + (labelInfo.DesiredSize.Width / 2));
#if __ANDROID__
                 float labelPointY = (float)(labelInfo.Position.Y - (labelInfo.DesiredSize.Height / 2));
#else
                float labelPointY = (float)(labelInfo.Position.Y + (labelInfo.DesiredSize.Height / 2));
#endif

                canvas.Rotate((float)labelInfo.RotationAngle, labelPointX, labelPointY);

                if (LabelStyle != null)
                {
                    canvas.DrawText(this.Label, (float)labelInfo.Position.X, (float)labelInfo.Position.Y, LabelStyle);
                }

                canvas.RestoreState();
            }
        }

        /// <summary>
        /// Invalidate range view.
        /// </summary>
        internal void InvalidateDrawable()
        {
            if (this.RangeView != null)
                this.RangeView.InvalidateDrawable();
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when <see cref="StartValue"/> or <see cref="EndValue"/> changed
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnStartEndValueChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialRange radialRange)
            {
                radialRange.CreateRangeArc();
                if (radialRange.RadialAxis != null && radialRange.RadialAxis.UseRangeColorForAxis)
                {
                    radialRange.RadialAxis.InvalidateDrawable();
                }
                radialRange.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="StartWidth"/> or <see cref="EndWidth"/> or <see cref="RangeOffset"/> or <see cref="OffsetUnit"/> or <see cref="WidthUnit"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialRange radialRange && radialRange.RadialAxis != null)
            {
                radialRange.CreateRangeArc();

                radialRange.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="Label"/> property changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnLabelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialRange radialRange && radialRange.RadialAxis != null)
            {
                radialRange.UpdateRangeLabelPosition();
                radialRange.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="LabelStyle"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnLabelStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialRange radialRange)
            {
                if (oldValue is GaugeLabelStyle gaugeLabelStyle)
                {
                    gaugeLabelStyle.PropertyChanged -= radialRange.LabelStyle_PropertyChanged;
                }

                if (newValue is GaugeLabelStyle newGaugeLabelStyle)
                {
                    newGaugeLabelStyle.PropertyChanged += radialRange.LabelStyle_PropertyChanged;
                }

                if (radialRange.RadialAxis != null)
                {
                    radialRange.UpdateRangeLabelPosition();
                    radialRange.InvalidateDrawable();
                }
            }
        }

        /// <summary>
        /// Called when <see cref="Fill"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialRange radialRange)
            {
                if (radialRange.RadialAxis != null)
                {
                    radialRange.InvalidateDrawable();
                }
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
            if (bindable is RadialRange radialRange)
            {
                if (oldValue != null)
                {
                    if (oldValue is ObservableCollection<GaugeGradientStop> gradientStops)
                        gradientStops.CollectionChanged -= radialRange.GradientStops_CollectionChanged;
                }

                if (newValue != null)
                {
                    if (newValue is ObservableCollection<GaugeGradientStop> gradientStops)
                        gradientStops.CollectionChanged += radialRange.GradientStops_CollectionChanged;
                }

                radialRange.CreateRangeArc();
                radialRange.InvalidateDrawable();
            }
        }
#nullable enable

        #endregion

        #region Private methods

        /// <summary>
        /// To update range label position.
        /// </summary>
        private void UpdateRangeLabelPosition()
        {
            if (this.RadialAxis == null || string.IsNullOrEmpty(this.Label))
            {
                return;
            }

            double midAngleRadian = Utility.DegreeToRadian(outerArcStartAngle + (outerArcEndAngle - outerArcStartAngle) / 2);

            PointF labelPoint = new PointF();
            double startRadius = outerRadius - (outerRadius - innerStartRadius) / 2;
            double endRadius = outerRadius - (outerRadius - innerEndRadius) / 2;
            double midRadius = startRadius + (endRadius - startRadius) / 2;
            labelPoint.X = (float)(midRadius * Math.Cos(midAngleRadian) + this.RadialAxis.Center.X);
            labelPoint.Y = (float)(midRadius * Math.Sin(midAngleRadian) + this.RadialAxis.Center.Y);

            if (LabelStyle != null)
            {
                labelInfo.DesiredSize = this.Label.Measure(LabelStyle);
#if __ANDROID__
            labelInfo.Position = new Point(labelPoint.X - (labelInfo.DesiredSize.Width / 2),
                    labelPoint.Y + (labelInfo.DesiredSize.Height / 2));
#else
                labelInfo.Position = new Point(labelPoint.X - (labelInfo.DesiredSize.Width / 2),
                    labelPoint.Y - (labelInfo.DesiredSize.Height / 2));
#endif

            }

            double value = this.ActualStartValue + (this.ActualEndValue - this.ActualStartValue) / 2;
            labelInfo.RotationAngle = this.RadialAxis.ValueToAngle(value) + 90;
        }

        /// <summary>
        /// Called when <see cref="LabelStyle"/> property changed.
        /// </summary>
        /// <param name="sender">The BindableObject.</param>
        /// <param name="e">The PropertyChangedEventArgs value.</param>
        private void LabelStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == GaugeLabelStyle.FontSizeProperty.PropertyName)
            {
                this.UpdateRangeLabelPosition();
            }

            this.InvalidateDrawable();
        }

        /// <summary>
        /// Method used to calculate arc bounds with start and end angle. 
        /// </summary>
        /// <param name="outerStartAngle">Outer arc start angle.</param>
        /// <param name="outerEndAngle">Outer arc end angle.</param>
        /// <param name="innerArcStartRadius">Inner arc start radius.</param>
        /// <param name="innerArcEndRadius">Inner arc end radius.</param>
        /// <param name="allowAngleValidation">Boolean value, allow angle validation.</param>
        /// <returns></returns>
        private GaugeArcInfo? CalculateDifferentWidthInnerArc(double outerStartAngle, double outerEndAngle,
            double innerArcStartRadius, double innerArcEndRadius, bool allowAngleValidation = true)
        {
            if (this.RadialAxis != null)
            {
                double startAngleRadian = Utility.DegreeToRadian(outerStartAngle);
                double endAngleRadian = Utility.DegreeToRadian(outerEndAngle);
                double midAngleRadian = Utility.DegreeToRadian(outerStartAngle + (outerEndAngle - outerStartAngle) / 2);

                PointF startPoint = new PointF();
                startPoint.X = (float)(innerArcStartRadius * Math.Cos(startAngleRadian) + this.RadialAxis.Center.X);
                startPoint.Y = (float)(innerArcStartRadius * Math.Sin(startAngleRadian) + this.RadialAxis.Center.Y);

                PointF endPoint = new PointF();
                endPoint.X = (float)(innerArcEndRadius * Math.Cos(endAngleRadian) + this.RadialAxis.Center.X);
                endPoint.Y = (float)(innerArcEndRadius * Math.Sin(endAngleRadian) + this.RadialAxis.Center.Y);

                PointF midPoint = new PointF();
                double midRadius = innerArcStartRadius + (innerArcEndRadius - innerArcStartRadius) / 2;
                midPoint.X = (float)(midRadius * Math.Cos(midAngleRadian) + this.RadialAxis.Center.X);
                midPoint.Y = (float)(midRadius * Math.Sin(midAngleRadian) + this.RadialAxis.Center.Y);

                PointF innerArcCenter = Utility.GetCenterPoint(startPoint, midPoint, endPoint);
                float innerArcRadius = (float)Math.Sqrt(Math.Pow(startPoint.X - innerArcCenter.X, 2) + Math.Pow(startPoint.Y - innerArcCenter.Y, 2));
                float innerArcStartAngle = Utility.RadianToDegree((float)Math.Atan2(startPoint.Y - innerArcCenter.Y, startPoint.X - innerArcCenter.X));
                float innerArcEndAngle = Utility.RadianToDegree((float)Math.Atan2(endPoint.Y - innerArcCenter.Y, endPoint.X - innerArcCenter.X));

                if (allowAngleValidation)
                {
                    if (innerArcStartAngle < 0)
                    {
                        innerArcStartAngle += 360;
                    }

                    if (innerArcEndAngle < 0)
                    {
                        innerArcEndAngle += 360;
                    }
                }

                //For free hand rotation, checked 360 degree arc condition.
                if (this.ActualStartValue > this.ActualEndValue && this.RadialAxis.ActualSweepAngle != 359.99)
                {
                    float temp = (float)innerArcEndAngle;
                    innerArcEndAngle = innerArcStartAngle;
                    innerArcStartAngle = temp;
                }

                PointF innerArcTopLeft = new PointF((float)(innerArcCenter.X - innerArcRadius), (float)(innerArcCenter.Y - innerArcRadius));

                PointF innerArcBottomRight = new PointF((float)(innerArcCenter.X + innerArcRadius), (float)(innerArcCenter.Y + innerArcRadius));

                return new GaugeArcInfo()
                {
                    TopLeft = innerArcTopLeft,
                    BottomRight = innerArcBottomRight,
                    StartAngle = innerArcStartAngle,
                    EndAngle = innerArcEndAngle
                };
            }

            return null;
        }


        /// <summary>
        /// Called when <see cref="GradientStops"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void GradientStops_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.CreateRangeArc();
            this.InvalidateDrawable();
        }

        #endregion
    }
}
