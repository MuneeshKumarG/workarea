using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Create the pointer to indicate the value with built-in shape.
    /// To highlight values, set the marker pointer type to a built-in shape, such as a circle, triangle, inverted triangle, square, or diamond.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// <gauge:SfRadialGauge>
    ///     <gauge:SfRadialGauge.Axes>
    ///         <gauge:RadialAxis>
    ///             <gauge:RadialAxis.Pointers>
    ///                 <gauge:MarkerPointer Value="50" />
    ///             </gauge:RadialAxis.Pointers>
    ///         </gauge:RadialAxis>
    ///     </gauge:SfRadialGauge.Axes>
    /// </gauge:SfRadialGauge>
    /// ]]></code>
    /// </example>
    public class MarkerPointer : RadialPointer
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="BorderWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="BorderWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(MarkerPointer), 0d, propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MarkerHeight"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MarkerHeight"/> bindable property.
        /// </value>
        public static readonly BindableProperty MarkerHeightProperty =
            BindableProperty.Create(nameof(MarkerHeight), typeof(double), typeof(MarkerPointer), 16d, propertyChanged: OnMarkerPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MarkerWidth"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MarkerWidth"/> bindable property.
        /// </value>
        public static readonly BindableProperty MarkerWidthProperty =
            BindableProperty.Create(nameof(MarkerWidth), typeof(double), typeof(MarkerPointer), 16d, propertyChanged: OnMarkerPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MarkerOffset"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MarkerOffset"/> bindable property.
        /// </value>
        public static readonly BindableProperty MarkerOffsetProperty =
            BindableProperty.Create(nameof(MarkerOffset), typeof(double), typeof(MarkerPointer), double.NaN, propertyChanged: OnMarkerPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="OffsetUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OffsetUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty OffsetUnitProperty =
            BindableProperty.Create(nameof(OffsetUnit), typeof(SizeUnit), typeof(MarkerPointer), SizeUnit.Pixel, propertyChanged: OnMarkerPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MarkerType"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MarkerType"/> bindable property.
        /// </value>
        public static readonly BindableProperty MarkerTypeProperty =
            BindableProperty.Create(nameof(MarkerType), typeof(MarkerType), typeof(MarkerPointer), MarkerType.InvertedTriangle, propertyChanged: OnMarkerTypePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="MarkerTemplate"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="MarkerTemplate"/> bindable property.
        /// </value>
        public static readonly BindableProperty MarkerTemplateProperty =
            BindableProperty.Create(nameof(MarkerTemplate), typeof(DataTemplate), typeof(MarkerPointer), null, propertyChanged: OnMarkerTemplatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(MarkerPointer), new SolidColorBrush(Color.FromRgb(73, 89, 99)), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Stroke"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(MarkerPointer), Color.FromRgb(73, 89, 99), propertyChanged: OnInvalidatePropertyChanged);

        /// <summary>
        /// Identifies the <see cref="OverlayFill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OverlayFill"/> bindable property.
        /// </value>
        public static readonly BindableProperty OverlayFillProperty =
            BindableProperty.Create(nameof(OverlayFill), typeof(Brush), typeof(MarkerPointer), null);

        /// <summary>
        /// Identifies the <see cref="OverlayRadius"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="OverlayRadius"/> bindable property.
        /// </value>
        public static readonly BindableProperty OverlayRadiusProperty =
            BindableProperty.Create(nameof(OverlayRadius), typeof(double), typeof(MarkerPointer), double.NaN);

        /// <summary>
        /// Identifies the <see cref="HasShadow"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="HasShadow"/> bindable property.
        /// </value>
        public static readonly BindableProperty HasShadowProperty =
            BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(MarkerPointer), false, propertyChanged: OnInvalidatePropertyChanged);

        #endregion

        #region Fields

        /// <summary>
        /// Represents marker pointer angle.
        /// </summary>
        private float markerAngle;

        /// <summary>
        /// Represents marker pointer position. 
        /// </summary>
        private PointF markerPosition;

        /// <summary>
        /// Represents marker custom view.
        /// </summary>
        internal View? CustomView;

        /// <summary>
        /// Reprsents marker gets hovered with mouse or not. 
        /// </summary>
        internal bool IsHovered;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the width of the marker border outline.
        /// </summary>
        /// <value>
        /// It defines the thickness of the border. The default value is <c>0</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer BorderWidth="2" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double BorderWidth
        {
            get { return (double)this.GetValue(BorderWidthProperty); }
            set { this.SetValue(BorderWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the marker height in logical pixels.
        /// </summary>
        /// <value>
        /// It defines the height of the marker. The default value is <c>16</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer MarkerHeight="25" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double MarkerHeight
        {
            get { return (double)this.GetValue(MarkerHeightProperty); }
            set { this.SetValue(MarkerHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the marker width in logical pixels.
        /// </summary>
        /// <value>
        /// It defines the width of the marker. The default value is <c>16</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer MarkerWidth="25" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double MarkerWidth
        {
            get { return (double)this.GetValue(MarkerWidthProperty); }
            set { this.SetValue(MarkerWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the marker position value either in logical pixel or radius factor.
        /// </summary>
        /// <value>
        /// It defines the position offset of the marker either pixel or factor. The default value is <c>double.NaN</c>.
        /// </value>
        /// <example>
        /// If <see cref="OffsetUnit"/> is <see cref="SizeUnit.Factor"/>, value will be given from 0 to 1. Here pointer placing position is calculated by <see cref="MarkerOffset"/> * axis radius value.
        /// Example: <see cref="MarkerOffset"/> value is 0.2 and axis radius is 100, pointer is moving 20(0.2 * 100) logical pixels from axis outer radius. If <see cref="OffsetUnit"/> is <see cref="SizeUnit.Pixel"/>, defined value distance pointer will move from the outer radius axis.
        /// When you specify <see cref="MarkerOffset"/> is negative, the pointer will be positioned outside the axis.
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer MarkerOffset="0.2" OffsetUnit="Factor"/>
        ///             </ gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double MarkerOffset
        {
            get { return (double)this.GetValue(MarkerOffsetProperty); }
            set { this.SetValue(MarkerOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates to calculate the marker pointer offset in logical pixel or radius factor.
        /// </summary>
        /// <value>
        /// One of the <see cref="SizeUnit"/> enumeration that specifies how the <see cref="MarkerOffset"/> value is considered.
        /// The default mode is <see cref="SizeUnit.Pixel"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer OffsetUnit="Pixel" MarkerOffset="20"/>
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
        /// Gets or sets a value that specifies the marker type for the pointer. 
        /// </summary>
        /// <value>
        /// One of the enumeration values that specifies the marker type of marker pointer in the radial gauge.
        /// The default is <see cref="MarkerType.InvertedTriangle"/>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer MarkerType="Triangle" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public MarkerType MarkerType
        {
            get { return (MarkerType)this.GetValue(MarkerTypeProperty); }
            set { this.SetValue(MarkerTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the data template to customizes the marker view.
        /// </summary>
        /// <value>
        /// The custom view for the marker pointer. The default is <c>null</c>.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                <gauge:MarkerPointer Value="10" >
        ///                    <gauge:MarkerPointer.MarkerTemplate>
        ///                       <DataTemplate>
        ///                           <VerticalStackLayout>
        ///                               <Image Source = "sun.png" HeightRequest="30"
        ///                                      WidthRequest="30"/>
        ///                               <Label Text = "Cloud" TextColor="Black"/>
        ///                           </VerticalStackLayout>
        ///                       </DataTemplate>
        ///                   </gauge:MarkerPointer.MarkerTemplate>
        ///                 </gauge:MarkerPointer>
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public DataTemplate MarkerTemplate
        {
            get { return (DataTemplate)this.GetValue(MarkerTemplateProperty); }
            set { this.SetValue(MarkerTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the marker fill color of pointer. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer Fill="Red" />
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
        /// Gets or sets a value that specifies the marker stroke color of pointer. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer Stroke="Red" />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public Color Stroke
        {
            get { return (Color)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the marker overlay fill color of pointer. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer Stroke="Red" MarkerType="Circle" OverlayRadius="15" OverlayFill="#65FF0000" IsInteractive="True"  />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public Brush OverlayFill
        {
            get { return (Brush)this.GetValue(OverlayFillProperty); }
            set { this.SetValue(OverlayFillProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the marker overlay fill radius of pointer. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer Stroke="Red" MarkerType="Circle" OverlayRadius="15" IsInteractive="True"  />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double OverlayRadius
        {
            get { return (double)this.GetValue(OverlayRadiusProperty); }
            set { this.SetValue(OverlayRadiusProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that specifies the shadow effect for marker pointer. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.Pointers>
        ///                 <gauge:MarkerPointer Stroke="Red" HasShadow="True"  />
        ///             </gauge:RadialAxis.Pointers>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public bool HasShadow
        {
            get { return (bool)this.GetValue(HasShadowProperty); }
            set { this.SetValue(HasShadowProperty, value); }
        }

        #endregion

        #region Override methods

        internal override void Draw(ICanvas canvas)
        {
            if (this.RadialAxis != null && this.CustomView == null)
            {
                DrawMarkerPointer(canvas);
            }
        }

        internal override void CreatePointer()
        {
            this.CreateMarkerPointer();
        }

        internal override void UpdatePointer()
        {
            if (this.RadialAxis != null && !this.RadialAxis.AvailableSize.IsZero)
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

                double actualMarkerOffset = 0;

                if (!double.IsNaN(this.MarkerOffset))
                {
                    actualMarkerOffset = this.RadialAxis.CalculateActualSize(this.MarkerOffset, this.OffsetUnit, true);
                }

                double radius = ((double.IsNaN(this.MarkerOffset) ? this.RadialAxis.Radius : actualMarkerOffset) * this.RadialAxis.RadiusFactor) -
                    (this.RadialAxis.AxisLineRadiusDifference + (this.RadialAxis.ActualAxisLineWidth / 2));
                radius = radius < 0 ? 0 : radius;
                double factor = this.RadialAxis.ValueToFactor(actualValue);
                Point vector = this.RadialAxis.FactorToPoint(factor);

                double markerHeight, markerWidth;

                if (this.CustomView != null)
                {
                    Size size = this.CustomView.ComputeDesiredSize(this.RadialAxis.AvailableSize.Width, this.RadialAxis.AvailableSize.Height);
                    markerHeight = size.Height;
                    markerWidth = size.Width;
                }
                else
                {
                    markerHeight = this.MarkerHeight;
                    markerWidth = this.MarkerWidth;
                }

                markerPosition = new PointF((float)(this.RadialAxis.Center.X + (radius * vector.X) - (markerWidth / 2)),
                        (float)(this.RadialAxis.Center.Y + (radius * vector.Y) - (markerHeight / 2)));

                markerAngle = (float)this.RadialAxis.FactorToAngle(factor) + 90;

                //Calculate dragging rectangle. 
                this.PointerRect = new RectangleF(this.markerPosition.X - DraggingOffset, this.markerPosition.Y - DraggingOffset,
                (float)markerWidth + (DraggingOffset * 2), (float)markerHeight + (DraggingOffset * 2));

                if (this.CustomView != null)
                {
                    if (this.RadialAxis.CanAnimate && this.AnimationValue != null && this.CustomView.Opacity == 0)
                    {
                        this.CustomView.Opacity = 1;
                    }

                    AbsoluteLayout.SetLayoutBounds(this.CustomView, new Rectangle(markerPosition, new Size(markerWidth, markerHeight)));
                }
            }
        }

        internal override void UpdatePointerReleased()
        {
            base.UpdatePointerReleased();

            if (this.OverlayRadius > 0)
            {
                this.InvalidateDrawable();
                this.IsHovered = false;
            }
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Method used to add <see cref="MarkerTemplate"/> in radial gauge pointers layout.
        /// </summary>
        /// <param name="view">Represents the view to add.</param>
        internal void AddCustomView(View view)
        {
            //TODO : Here we added marker template view to annotation layout for below grid issues. 
            //https://github.com/dotnet/maui/issues/3509
            //https://github.com/dotnet/maui/issues/3510
            //We have to re-structure the marker template parent, once these issues get resolved. 
            if (this.RadialAxis != null && !this.RadialAxis.AnnotationsLayout.Children.Contains(view))
            {
                view.BindingContext = this;
                this.RadialAxis.AnnotationsLayout.Children.Add(view);
            }
        }

        /// <summary>
        /// Method used to remove <see cref="MarkerTemplate"/> in radial gauge pointers layout.
        /// </summary>
        /// <param name="view">Represents the view to remove.</param>
        internal void RemoveCustomView(View view)
        {
            if (this.RadialAxis != null && this.RadialAxis.AnnotationsLayout.Children.Contains(view))
            {
                this.RadialAxis.AnnotationsLayout.Children.Remove(view);
            }
        }

        #endregion

        #region Property changed

        /// <summary>
        /// Called when marker pointer <see cref="MarkerTemplate"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMarkerTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MarkerPointer markerPointer)
            {
                if (oldValue is DataTemplate && markerPointer.CustomView != null)
                {
                    markerPointer.RemoveCustomView(markerPointer.CustomView);
                    markerPointer.CustomView = null;
                }

                if (newValue is DataTemplate newView)
                {
                    var layout = newView.CreateContent();

                    if (layout is ViewCell viewCell)
                    {
                        markerPointer.CustomView = viewCell.View;
                    }
                    else if (layout is View view)
                    {
                        markerPointer.CustomView = view;
                    }

                    if (markerPointer.CustomView != null)
                    {
                        markerPointer.AddCustomView(markerPointer.CustomView);
                    }
                }

                markerPointer.UpdatePointer();
                markerPointer.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when marker pointer <see cref="MarkerHeight"/>, <see cref="MarkerWidth"/>, <see cref="MarkerOffset"/>, and <see cref="OffsetUnit"/> changed.
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

        /// <summary>
        /// Called when marker pointer <see cref="MarkerType"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnMarkerTypePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MarkerPointer markerPointer)
            {
                markerPointer.UpdatePointer();
                markerPointer.InvalidateDrawable();
            }
        }

        /// <summary>
        /// Called when marker pointer <see cref="Fill"/>, <see cref="Stroke"/>, and <see cref="BorderWidth"/> changed.
        /// </summary>
        /// <param name="bindable">The BindableObject.</param>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        private static void OnInvalidatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is MarkerPointer markerPointer)
            {
                markerPointer.InvalidateDrawable();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// To create marker pointer.
        /// </summary>
        private void CreateMarkerPointer()
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
                this.UpdatePointer();
            }
        }

        /// <summary>
        /// Draw marker pointer defined shapes. 
        /// </summary>
        /// <param name="canvas"></param>
        private void DrawMarkerPointer(ICanvas canvas)
        {
            canvas.SaveState();

            float halfBorderWidth = (float)this.BorderWidth / 2;
            float halfWidth = (float)this.MarkerWidth / 2;
            float halfHeight = (float)this.MarkerHeight / 2;
            float positionX = this.markerPosition.X + halfBorderWidth;
            float positionY = this.markerPosition.Y + halfBorderWidth;
            float width = (float)this.MarkerWidth - (float)this.BorderWidth;
            float height = (float)this.MarkerHeight - (float)this.BorderWidth;

            //Set stroke and stroke size for marker.
            if (this.BorderWidth > 0)
            {
                canvas.StrokeColor = Stroke;
                canvas.StrokeSize = (float)this.BorderWidth;
            }

            //Set rotation angle for marker.
            if (this.MarkerType == MarkerType.Diamond)
            {
                canvas.Rotate(this.markerAngle + 45, this.markerPosition.X + halfWidth, this.markerPosition.Y + halfHeight);
            }
            else if (this.MarkerType == MarkerType.Triangle)
            {
                canvas.Rotate(this.markerAngle + 180, this.markerPosition.X + halfWidth, this.markerPosition.Y + halfHeight);
            }
            else
            {
                canvas.Rotate(this.markerAngle, this.markerPosition.X + halfWidth, this.markerPosition.Y + halfHeight);
            }

            //Draw marker shape overlay.
            if (this.OverlayRadius > 0 && (this.IsPressed || this.IsHovered))
            {
                DrawMarkerOverlay(canvas);
            }

            if (this.HasShadow)
                canvas.SetShadow(new SizeF((float)this.MarkerWidth / 2, (float)this.MarkerHeight / 2), 10f, Colors.Gray);

            canvas.SetFillPaint(this.Fill, new RectangleF(positionX, positionY, width, height));

            //Draw marker shape.
            switch (this.MarkerType)
            {
                case MarkerType.Circle:

                    canvas.FillEllipse(positionX, positionY, width, height);

                    if (BorderWidth > 0)
                    {
                        canvas.DrawEllipse(positionX, positionY, width, height);
                    }

                    break;
                case MarkerType.Diamond:
                case MarkerType.Rectangle:

                    canvas.FillRectangle(positionX, positionY, width, height);

                    if (BorderWidth > 0)
                    {
                        canvas.DrawRectangle(positionX, positionY, width, height);
                    }

                    break;
                case MarkerType.InvertedTriangle:
                case MarkerType.Triangle:
                    PathF path = new PathF();

                    path.LineTo(positionX, positionY);
                    path.LineTo(positionX + width, positionY);
                    path.LineTo(positionX + width / 2, positionY + height);
                    path.Close();

                    canvas.FillPath(path);

                    if (BorderWidth > 0)
                    {
                        canvas.DrawPath(path);
                    }

                    break;
                default:
                    break;
            }

            canvas.RestoreState();
        }

        /// <summary>
        /// Method used to draw overlay for marker pointer. 
        /// </summary>
        /// <param name="canvas"></param>
        private void DrawMarkerOverlay(ICanvas canvas)
        {
            float overlayPositionX = this.markerPosition.X - (float)Math.Abs(this.MarkerWidth / 2 - this.OverlayRadius);
            float overlayPositionY = this.markerPosition.Y - (float)Math.Abs(this.MarkerHeight / 2 - this.OverlayRadius);
            float overlayWidth = (float)this.OverlayRadius * 2;
            float overlayHeight = (float)this.OverlayRadius * 2;

            if (this.OverlayFill == null)
            {
                canvas.SetFillPaint(this.Fill, new RectangleF(overlayPositionX, overlayPositionY, overlayWidth, overlayHeight));
                canvas.Alpha = 0.5f;
            }
            else
            {
                canvas.SetFillPaint(this.OverlayFill, new RectangleF(overlayPositionX, overlayPositionY, overlayWidth, overlayHeight));
            }

            switch (this.MarkerType)
            {
                case MarkerType.Circle:
                    canvas.FillEllipse(overlayPositionX, overlayPositionY, overlayWidth, overlayHeight);

                    break;
                case MarkerType.Diamond:
                case MarkerType.Rectangle:
                    canvas.FillRectangle(overlayPositionX, overlayPositionY, overlayWidth, overlayHeight);

                    break;
                case MarkerType.InvertedTriangle:
                case MarkerType.Triangle:
                    PathF path = new PathF();
                    path.LineTo(overlayPositionX, overlayPositionY);
                    path.LineTo(overlayPositionX + overlayWidth, overlayPositionY);
                    path.LineTo(overlayPositionX + overlayWidth / 2, overlayPositionY + overlayHeight);
                    path.Close();

                    canvas.FillPath(path);
                    break;
                default:
                    break;
            }

            if (this.OverlayFill == null)
                canvas.Alpha = 1f;
        }

        #endregion
    }
}
