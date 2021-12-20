using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents the style of axis ticks, that used to customize axis ticks color, length, thickness and dash array.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// <gauge:SfRadialGauge>
    ///     <gauge:SfRadialGauge.Axes>
    ///         <gauge:RadialAxis>
    ///             <gauge:RadialAxis.MajorTickStyle>
    ///                 <gauge:RadialTickStyle StrokeDashArray="2,2" />
    ///             </ gauge:RadialAxis.MajorTickStyle>
    ///             <gauge:RadialAxis.MinorTickStyle>
    ///                 <gauge:RadialTickStyle StrokeDashArray="1,1" />
    ///             </ gauge:RadialAxis.MinorTickStyle>
    ///         </gauge:RadialAxis>
    ///     </gauge:SfRadialGauge.Axes>
    /// </gauge:SfRadialGauge>
    /// ]]></code>
    /// </example>
    public class RadialTickStyle : GaugeTickStyle
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="LengthUnit"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="LengthUnit"/> bindable property.
        /// </value>
        public static readonly BindableProperty LengthUnitProperty =
            BindableProperty.Create(nameof(LengthUnit), typeof(SizeUnit), typeof(RadialTickStyle), SizeUnit.Pixel);

        #endregion

        #region Fields

        internal bool IsMajorTicks;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets length unit of axis major or minor ticks. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.MajorTickStyle>
        ///                 <gauge:RadialTickStyle Length="10" LengthUnit="Pixel" />
        ///             </ gauge:RadialAxis.MajorTickStyle>
        ///             <gauge:RadialAxis.MinorTickStyle>
        ///                 <gauge:RadialTickStyle Length="5" LengthUnit="Pixel" />
        ///             </ gauge:RadialAxis.MinorTickStyle>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public SizeUnit LengthUnit
        {
            get { return (SizeUnit)this.GetValue(LengthUnitProperty); }
            set { this.SetValue(LengthUnitProperty, value); }
        }

        #endregion
    }
}
