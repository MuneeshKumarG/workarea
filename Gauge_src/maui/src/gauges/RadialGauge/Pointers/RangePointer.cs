using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Create the pointer to indicate the value with rounded range bar arc.
    /// A RangePointer is used to indicate the current value relative to the start value of a axis scale.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// <gauge:SfRadialGauge>
    ///     <gauge:SfRadialGauge.Axes>
    ///         <gauge:RadialAxis>
    ///             <gauge:RadialAxis.Pointers>
    ///                 <gauge:RangePointer Value="50" />
    ///             </gauge:RadialAxis.Pointers>
    ///         </gauge:RadialAxis>
    ///     </gauge:SfRadialGauge.Axes>
    /// </gauge:SfRadialGauge>
    /// ]]></code>
    /// </example>
    public class RangePointer : RadialPointer
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="PointerWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PointerWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty PointerWidthProperty =
            BindableProperty.Create(nameof(PointerWidth), typeof(double), typeof(RangePointer), 10d, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="WidthUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="WidthUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty WidthUnitProperty =
            BindableProperty.Create(nameof(WidthUnit), typeof(SizeUnit), typeof(RangePointer), SizeUnit.Pixel, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="PointerOffset"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="PointerOffset"/> bindable property.
        /// </value>
        public static readonly BindableProperty PointerOffsetProperty =
            BindableProperty.Create(nameof(PointerOffset), typeof(double), typeof(RangePointer), double.NaN, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="OffsetUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OffsetUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty OffsetUnitProperty =
            BindableProperty.Create(nameof(OffsetUnit), typeof(SizeUnit), typeof(RangePointer), SizeUnit.Pixel, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="CornerStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CornerStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty CornerStyleProperty =
            BindableProperty.Create(nameof(CornerStyle), typeof(CornerStyle), typeof(RangePointer), CornerStyle.BothFlat, propertyChanged: OnPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(RangePointer), null, defaultValueCreator: bindable => new SolidColorBrush(Color.FromRgb(73, 89, 99)), propertyChanged: OnFillPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="GradientStops"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="GradientStops"/> bindable property.
        /// </value>
        public static readonly BindableProperty GradientStopsProperty =
         BindableProperty.Create(nameof(GradientStops), typeof(ObservableCollection<GaugeGradientStop>), typeof(RangePointer), null, propertyChanged: OnGradientStopsPropertyChanged);

        #endregion

        #region Fields

        /// <summary>
        /// Used to draw range pointer arc without gradient.
        /// </summary>
        private PathF? rangePointerPath;

        /// <summary>
        /// Contains <see cref="RangePointer"/> gradient arcs.
        /// </summary>
        private List<GaugeArcInfo>? gradientArcPaths;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RangePointer"/> class.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:RangePointer Value="50" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public RangePointer()
        {
            this.GradientStops = new ObservableCollection<GaugeGradientStop>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value that specifies the pointer width.
        /// You can specify value either in logical pixel or radius factor using the <see cref="WidthUnit"/> property. 
        /// </summary>
        /// <value>
        /// It defines the width of the pointer. The default value is <c>10</c>.
        /// </value>
        /// <example>
        /// If <see cref="WidthUnit"/> is <see cref="SizeUnit.Factor"/>, value will be given from 0 to 1. Here length size is calculated by <see cref="PointerWidth"/> * axis radius value.
        /// Example: <see cref="PointerWidth"/> value is 0.2 and axis radius is 100, length size is 20(0.2 * 100) logical pixels. if <see cref="WidthUnit"/> is <see cref="SizeUnit.Pixel"/>, defined value is set to the length size.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:RangePointer Value="50" 
        ///                                     PointerWidth="0.2" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double PointerWidth
        {
            get { return (double)this.GetValue(PointerWidthProperty); }
            set { this.SetValue(PointerWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates to calculate the range pointer width either in logical pixel or radius factor.
        /// </summary>
        /// <value>
        /// One of the <see cref="SizeUnit"/> enumeration that specifies how the <see cref="PointerWidth"/> value is considered.
        /// The default mode is <see cref="SizeUnit.Pixel"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:RangePointer Value="50" 
        ///                                     PointerWidth="0.2"
        ///                                     WidthUnit="Factor" />
        ///             </gauge:RadialAxis.Pointers>
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
        /// Gets or sets the value that specifies the position value for pointer.
        ///  You can specify value either in logical pixel or radius factor using the <see cref="OffsetUnit"/> property.
        /// </summary>
        /// <value>
        /// It defines the position offset of the pointer either pixel or factor. The default value is <c>double.NaN</c>.
        /// </value>
        /// <example>
        /// If <see cref="OffsetUnit"/> is <see cref="SizeUnit.Factor"/>, value will be given from 0 to 1. Here pointer placing position is calculated by <see cref="PointerOffset"/> * axis radius value.
        /// Example: <see cref="PointerOffset"/> value is 0.2 and axis radius is 100, pointer is moving 20(0.2 * 100) logical pixels from axis outer radius. If <see cref="OffsetUnit"/> is <see cref="SizeUnit.Pixel"/>, defined value distance pointer will move from the outer radius axis.
        /// When you specify <see cref="PointerOffset"/> is negative, the pointer will be positioned outside the axis.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:RangePointer Value="50" 
        ///                                     PointerWidth="0.2"
        ///                                     WidthUnit="Factor"
        ///                                     PointerOffset="0.2" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double PointerOffset
        {
            get { return (double)this.GetValue(PointerOffsetProperty); }
            set { this.SetValue(PointerOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates the to calculate the range pointer offset either in logical pixel or factor.
        /// </summary>
        /// <value>
        /// One of the <see cref="SizeUnit"/> enumeration that specifies how the <see cref="PointerOffset"/> value is considered.
        /// The default mode is <see cref="SizeUnit.Pixel"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:RangePointer Value="50" 
        ///                                     PointerWidth="0.2"
        ///                                     WidthUnit="Factor"
        ///                                     PointerOffset="0.2"
        ///                                     OffsetUnit="Factor" />
        ///             </gauge:RadialAxis.Pointers>
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
        /// Gets or sets the value that specifies the corner style of range pointer.
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the corner style of range pointer in the radial gauge.
        /// The default is <see cref="CornerStyle.BothFlat"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:RangePointer Value="50" 
        ///                                     PointerWidth="0.2"
        ///                                     WidthUnit="Factor"
        ///                                     PointerOffset="0.2"
        ///                                     OffsetUnit="Factor"
        ///                                     CornerStyle="BothCurve" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public CornerStyle CornerStyle
        {
            get { return (CornerStyle)this.GetValue(CornerStyleProperty); }
            set { this.SetValue(CornerStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Color that paints the interior area of the range.
        /// </summary>
        /// <value>
        /// A <c>Color</c> that specifies how the range path is painted.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:RangePointer Value="50"
        ///                                     Fill="Red" />
        ///             </gauge:RadialAxis.Pointers>
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
        /// Gets or sets a collection of <see cref="GaugeGradientStop"/> to fill the gradient brush to the range pointer.
        /// </summary>
        /// <value>
        /// A collection of the <see cref="GaugeGradientStop"/> objects associated with the brush, each of which specifies a color and an offset along the axis.
        /// The default is an empty collection.
        /// </value>
        /// <example>
        /// The below examples shows, how to add a collection of gradient stop to the range pointer. 
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:RangePointer Value="80"
        ///                                     PointerWidth="0.2"
        ///                                     WidthUnit="Factor"
        ///                                     PointerOffset="0.2"
        ///                                     OffsetUnit="Factor">
        ///                     <gauge:RangePointer.GradientStops>
        ///                         <gauge:GaugeGradientStop Value="0"
        ///                                                  Color="Green" />
        ///                         <gauge:GaugeGradientStop Value="50"
        ///                                                  Color="Yellow" />
        ///                         <gauge:GaugeGradientStop Value="80"
        ///                                                  Color="Red" />
        ///                     </gauge:RangePointer.GradientStops>
        ///                 </gauge:RangePointer>
        ///             </gauge:RadialAxis.Pointers>
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

        #endregion

        #region Internal methods

        /// <summary>
        /// To create range pointer arc
        /// </summary>
        internal void CreateRangePointer()
        {
            if (this.RadialAxis == null || this.RadialAxis.AvailableSize.IsZero)
            {
                return;
            }

            double actualEndValue;
            double? actualRangePointerOffset = null;

            if ((this.EnableAnimation || this.RadialAxis.CanAnimate) && this.AnimationValue != null)
            {
                actualEndValue = (double)this.AnimationValue;
            }
            else
            {
                actualEndValue = Math.Clamp(this.Value, this.RadialAxis.ActualMinimum, this.RadialAxis.ActualMaximum);
            }

            if (this.RadialAxis.ActualMinimum == actualEndValue)
            {
                return;
            }

            double actualPointerWidth = this.RadialAxis.CalculateActualSize(this.PointerWidth, this.WidthUnit, false);

            if (!double.IsNaN(this.PointerOffset))
            {
                actualRangePointerOffset = this.RadialAxis.CalculateActualSize(this.PointerOffset, this.OffsetUnit, true);
            }

            double actualStartAngle = this.RadialAxis.ValueToAngle(this.RadialAxis.ActualMinimum);
            double actualEndAngle = this.RadialAxis.ValueToAngle(actualEndValue);
            double outerRadius, innerRadius;
            if (actualRangePointerOffset == null)
            {
                outerRadius = (this.RadialAxis.Radius * this.RadialAxis.RadiusFactor) - this.RadialAxis.AxisLineRadiusDifference;
            }
            else
            {
                outerRadius = (double)actualRangePointerOffset * this.RadialAxis.RadiusFactor - this.RadialAxis.AxisLineRadiusDifference;
            }

            outerRadius = outerRadius < 0 ? 0 : outerRadius;
            innerRadius = outerRadius - actualPointerWidth;
            innerRadius = innerRadius < 0 ? 0 : innerRadius;

            //Calculating outer arc bounds.
            PointF outerArcTopLeft = new PointF((float)(this.RadialAxis.Center.X - outerRadius),
                (float)(this.RadialAxis.Center.Y - outerRadius));
            PointF outerArcBottomRight = new PointF((float)(this.RadialAxis.Center.X + outerRadius),
                (float)(this.RadialAxis.Center.Y + outerRadius));

            //Calculating inner arc bounds.
            PointF innerArcTopLeft = new PointF((float)(this.RadialAxis.Center.X - innerRadius),
               (float)(this.RadialAxis.Center.Y - innerRadius));

            PointF innerArcBottomRight = new PointF((float)(this.RadialAxis.Center.X + innerRadius),
                (float)(this.RadialAxis.Center.Y + innerRadius));

            Point? endCurveCapCenter = null;
            Point? startCurveCapCenter = null;
            double lineHalfWidth = actualPointerWidth / 2;
            double midRadius = outerRadius == 0 ? 0 : outerRadius - lineHalfWidth;
            double cornerRadiusAngle = Utility.CornerRadiusAngle(this.RadialAxis.Radius, lineHalfWidth);

            //Calculating start angle and its curve cap center.
            if (this.CornerStyle == CornerStyle.StartCurve || this.CornerStyle == CornerStyle.BothCurve)
            {
                actualStartAngle += this.RadialAxis.IsInversed ? -cornerRadiusAngle : cornerRadiusAngle;
                Point vector = Utility.AngleToVector(actualStartAngle);

                startCurveCapCenter = new Point(this.RadialAxis.Center.X + (midRadius * vector.X),
                    this.RadialAxis.Center.Y + (midRadius * vector.Y));
            }

            CalculatePointerRect(actualEndAngle, midRadius, lineHalfWidth);

            //Calculating end angle and its curve cap center.
            if (this.CornerStyle == CornerStyle.EndCurve || this.CornerStyle == CornerStyle.BothCurve)
            {
                actualEndAngle -= this.RadialAxis.IsInversed ? -cornerRadiusAngle : cornerRadiusAngle;
                Point vector = Utility.AngleToVector(actualEndAngle);

                endCurveCapCenter = new Point(this.RadialAxis.Center.X + (midRadius * vector.X),
                    this.RadialAxis.Center.Y + (midRadius * vector.Y));
            }

            if (this.GradientStops != null && this.GradientStops.Count > 1)
            {
                //Create gradient arc data collection. 
                this.gradientArcPaths = this.RadialAxis.CreateGradientArcSegments(this.GradientStops.ToList(), innerRadius,
                    innerRadius, this.RadialAxis.ActualMinimum, actualEndValue);

                for (int i = 0; i < gradientArcPaths.Count; i++)
                {
                    GaugeArcInfo arcInfo = gradientArcPaths[i];

                    //Update gradient arc angles based on corner style. 
                    if (i == 0 && ((!this.RadialAxis.IsInversed && startCurveCapCenter != null) ||
                        (this.RadialAxis.IsInversed && endCurveCapCenter != null)))
                    {
                        arcInfo.StartAngle += this.RadialAxis.IsInversed ? -(float)cornerRadiusAngle : (float)cornerRadiusAngle;
                    }

                    if ((!this.RadialAxis.IsInversed && endCurveCapCenter != null) ||
                        (this.RadialAxis.IsInversed && startCurveCapCenter != null))
                    {
                        float endCornerAngle = this.RadialAxis.IsInversed ? -(float)cornerRadiusAngle : (float)cornerRadiusAngle;

                        if (i == gradientArcPaths.Count - 1)
                        {
                            arcInfo.EndAngle -= endCornerAngle;

                            if (i > 0 && arcInfo.EndAngle < arcInfo.StartAngle)
                                arcInfo.StartAngle = arcInfo.EndAngle;
                        }

                        if (i == gradientArcPaths.Count - 2 && (gradientArcPaths[i + 1].EndAngle - endCornerAngle) < arcInfo.EndAngle)
                        {
                            arcInfo.EndAngle = gradientArcPaths[i + 1].EndAngle - endCornerAngle;
                        }
                    }

                    //Create gradient arc path. 
                    this.RadialAxis.CreateFilledArc(arcInfo.ArcPath, outerArcTopLeft, outerArcBottomRight, innerArcTopLeft,
                   innerArcBottomRight, this.RadialAxis.Center, arcInfo.StartAngle, arcInfo.EndAngle, outerRadius, innerRadius);

                    //Append circle in edge for corner style. 
                    if (i == 0 && startCurveCapCenter != null)
                    {
                        arcInfo.ArcPath.AppendCircle((float)startCurveCapCenter.Value.X, (float)startCurveCapCenter.Value.Y, (float)lineHalfWidth);
                    }
                    
                    if (i == gradientArcPaths.Count - 1 && endCurveCapCenter != null)
                    {
                        arcInfo.ArcPath.AppendCircle((float)endCurveCapCenter.Value.X, (float)endCurveCapCenter.Value.Y, (float)lineHalfWidth);
                    }
                }
            }
            else
            {
                this.rangePointerPath = new PathF();
                this.gradientArcPaths = null;

                //Calculate range pointer arc.
                this.RadialAxis.CreateFilledArc(this.rangePointerPath, outerArcTopLeft, outerArcBottomRight, innerArcTopLeft, innerArcBottomRight,
                    this.RadialAxis.Center, actualStartAngle, actualEndAngle, outerRadius, innerRadius);

                //Calculate range pointer edge curve path.
                if (startCurveCapCenter == null && endCurveCapCenter == null)
                {
                    return;
                }

                if (startCurveCapCenter != null)
                {
                    rangePointerPath.AppendCircle((float)startCurveCapCenter.Value.X, (float)startCurveCapCenter.Value.Y, (float)lineHalfWidth);
                }

                if (endCurveCapCenter != null)
                {
                    rangePointerPath.AppendCircle((float)endCurveCapCenter.Value.X, (float)endCurveCapCenter.Value.Y, (float)lineHalfWidth);
                }
            }
        }

        /// <summary>
        /// To update the range pointer.
        /// </summary>
        internal override void UpdatePointer()
        {
            this.CreateRangePointer();
        }

        /// <summary>
        /// To create range pointer.
        /// </summary>
        internal override void CreatePointer()
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
                this.CreateRangePointer();
            }
        }

        #endregion

        #region Override methods

        /// <summary>
        /// Method used to draw <see cref="RangePointer"/> arc segments.
        /// </summary>
        /// <param name="canvas"></param>
        internal override void Draw(ICanvas canvas)
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
                else if (this.rangePointerPath != null)
                {
                    //Setting range fill color.
                    if (this.GradientStops != null && this.GradientStops.Count == 1)
                        canvas.FillColor = this.GradientStops[0].Color;
                    else
                        canvas.SetFillPaint(this.Fill, this.rangePointerPath.Bounds);

                    canvas.FillPath(this.rangePointerPath);
                }

                canvas.RestoreState();
            }
        }

        /// <summary>
        /// Called when binding context changed. 
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (this.GradientStops != null)
            {
                foreach (var gradientStop in this.GradientStops)
                    SetInheritedBindingContext(gradientStop, BindingContext);
            }
        }
      
        #endregion

        #region Property changed

        /// <summary>
        /// Called when <see cref="RangePointer"/> properties changed.
        /// </summary>
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RangePointer rangePointer)
            {
                rangePointer.CreateRangePointer();
                rangePointer.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when <see cref="Fill"/> changed.
        /// </summary>
        private static void OnFillPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is RadialPointer radialPointer)
            {
                radialPointer.InvalidateDrawable();
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
            if (bindable is RangePointer rangePointer)
            {
                if (oldValue != null)
                {
                    if (oldValue is ObservableCollection<GaugeGradientStop> gradientStops)
                        gradientStops.CollectionChanged -= rangePointer.GradientStops_CollectionChanged;
                }

                if (newValue != null)
                {
                    if (newValue is ObservableCollection<GaugeGradientStop> gradientStops)
                        gradientStops.CollectionChanged += rangePointer.GradientStops_CollectionChanged;
                }

                rangePointer.CreateRangePointer();
                rangePointer.InvalidateDrawable();
            }
        }
#nullable enable

        #endregion

        #region Private methods

        /// <summary>
        /// Method used to calculate pointer dragging rect. 
        /// </summary>
        /// <param name="endAngle"></param>
        /// <param name="midRadius"></param>
        /// <param name="lineHalfWidth"></param>
        private void CalculatePointerRect(double endAngle, double midRadius, double lineHalfWidth)
        {
            if (this.RadialAxis != null)
            {
                Point endAnglevector = Utility.AngleToVector(endAngle);

                var point = new Point(this.RadialAxis.Center.X + (midRadius * endAnglevector.X),
                    this.RadialAxis.Center.Y + (midRadius * endAnglevector.Y));
                float size = (float)(2 * lineHalfWidth) + (DraggingOffset * 2);

                this.PointerRect = new RectangleF((float)(point.X - lineHalfWidth - DraggingOffset),
                    (float)(point.Y - lineHalfWidth - DraggingOffset), size, size);
            }
        }

        /// <summary>
        /// Called when <see cref="GradientStops"/> collection got changed.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The NotifyCollectionChangedEventArgs.</param>
        private void GradientStops_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.CreateRangePointer();
            this.InvalidateDrawable();
        }

        #endregion
    }
}
