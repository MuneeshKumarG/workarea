using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents the style of axis line, that used to customize axis line color, corner style, thickness and dash array.
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
    public class RadialLineStyle : BindableObject
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Fill"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Fill"/> bindable property.
        /// </value>
        public static readonly BindableProperty FillProperty =
           BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(RadialLineStyle), null, defaultValueCreator: bindable => new SolidColorBrush(Color.FromRgba(33, 33, 33, 20)));
        
        /// <summary>
        /// Identifies the <see cref="GradientStops"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="GradientStops"/> bindable property.
        /// </value>
        public static readonly BindableProperty GradientStopsProperty =
          BindableProperty.Create(nameof(GradientStops), typeof(ObservableCollection<GaugeGradientStop>), typeof(RadialLineStyle), null);

        /// <summary>
        /// Identifies the <see cref="Thickness"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Thickness"/> bindable property.
        /// </value>
        public static readonly BindableProperty ThicknessProperty =
            BindableProperty.Create(nameof(Thickness), typeof(double), typeof(RadialLineStyle), 10d);

        /// <summary>
        /// Identifies the <see cref="ThicknessUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="ThicknessUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty ThicknessUnitProperty =
            BindableProperty.Create(nameof(ThicknessUnit), typeof(SizeUnit), typeof(RadialLineStyle), SizeUnit.Pixel);

        /// <summary>
        /// Identifies the <see cref="CornerStyle"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="CornerStyle"/> bindable property.
        /// </value>
        public static readonly BindableProperty CornerStyleProperty =
            BindableProperty.Create(nameof(CornerStyle), typeof(CornerStyle), typeof(RadialLineStyle), CornerStyle.BothFlat);

        /// <summary>
        /// Identifies the <see cref="DashArray"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DashArray"/> bindable property.
        /// </value>
        public static readonly BindableProperty DashArrayProperty =
            BindableProperty.Create(nameof(DashArray), typeof(DoubleCollection), typeof(RadialLineStyle), null);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RadialLineStyle"/> class.
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
        public RadialLineStyle()
        {
            GradientStops = new ObservableCollection<GaugeGradientStop>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets color of axis line. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.AxisLineStyle>
        ///                 <gauge:RadialLineStyle Fill="Red"/>
        ///             </gauge:RadialAxis.AxisLineStyle>
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
        /// Gets or sets <see cref="GaugeGradientStop"/> collection, that used to fill axis line in with gradient effect. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///              <gauge:RadialAxis.AxisLineStyle>
        ///                 <gauge:RadialLineStyle >
        ///                     <gauge:RadialLineStyle.GradientStops>
        ///                         <gauge:GaugeGradientStop Value = "36" Color="#FFFF7676" />
        ///                         <gauge:GaugeGradientStop Value = "100"  Color="#FFF54EA2" />
        ///                     </gauge:RadialLineStyle.GradientStops>
        ///                 </gauge:RadialLineStyle>
        ///             </gauge:RadialAxis.AxisLineStyle>
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
        /// Gets or sets thickness unit of axis line. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.AxisLineStyle>
        ///                 <gauge:RadialLineStyle ThicknessUnit="Factor" Thickness="0.2"/>
        ///             </gauge:RadialAxis.AxisLineStyle>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public SizeUnit ThicknessUnit
        {
            get { return (SizeUnit)this.GetValue(ThicknessUnitProperty); }
            set { this.SetValue(ThicknessUnitProperty, value); }
        }

        /// <summary>
        /// Gets or sets thickness of axis line. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.AxisLineStyle>
        ///                 <gauge:RadialLineStyle ThicknessUnit="Factor" Thickness="0.2"/>
        ///             </gauge:RadialAxis.AxisLineStyle>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double Thickness
        {
            get { return (double)this.GetValue(ThicknessProperty); }
            set { this.SetValue(ThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets corner style of axis line.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.AxisLineStyle>
        ///                 <gauge:RadialLineStyle CornerStyle="BothCurve"/>
        ///             </gauge:RadialAxis.AxisLineStyle>
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
        /// Gets or sets double collection, that represents dash array of axis line. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.AxisLineStyle>
        ///                 <gauge:RadialLineStyle DashArray="2,4"/>
        ///             </gauge:RadialAxis.AxisLineStyle>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public DoubleCollection DashArray
        {
            get { return (DoubleCollection)this.GetValue(DashArrayProperty); }
            set { this.SetValue(DashArrayProperty, value); }
        }

        #endregion

        #region Override methods

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
        }

        #endregion
    }
}
