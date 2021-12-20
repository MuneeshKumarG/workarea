using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents the style of gauge labels, that used to customize labels color, size, font family and font attribute.
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
    public class GaugeLabelStyle : BindableObject, ITextElement
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextColor"/> bindable property.
        /// </value>
        public static readonly BindableProperty TextColorProperty =
         BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(GaugeLabelStyle), Color.FromRgba(0, 0, 0, 0.87));

        /// <summary>
        /// Identifies the <see cref="FontSize"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontSize"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontFamily"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

        /// <summary>
        /// Identifies the <see cref="FontAttributes"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="FontAttributes"/> bindable property.
        /// </value>
        public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets double value that represents size of gauge label.
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
        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets string, that represents font family of gauge label.
        /// </summary>
        public string FontFamily
        {
            get { return (string)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets FontAttributes of gauge label.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        ///        <gauge:SfRadialGauge>
        ///            <gauge:SfRadialGauge.Axes>
        ///                <gauge:RadialAxis>
        ///                    <gauge:RadialAxis.AxisLabelStyle>
        ///                        <gauge:GaugeLabelStyle FontAttributes="Bold" />
        ///                    </ gauge:RadialAxis.AxisLabelStyle>
        ///                </gauge:RadialAxis>
        ///            </gauge:SfRadialGauge.Axes>
        ///        </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)this.GetValue(FontAttributesProperty); }
            set { this.SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets or sets Color, that represents the gauge label color.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        ///        <gauge:SfRadialGauge>
        ///            <gauge:SfRadialGauge.Axes>
        ///                <gauge:RadialAxis>
        ///                    <gauge:RadialAxis.AxisLabelStyle>
        ///                        <gauge:GaugeLabelStyle TextColor="Red" />
        ///                    </ gauge:RadialAxis.AxisLabelStyle>
        ///                </gauge:RadialAxis>
        ///            </gauge:SfRadialGauge.Axes>
        ///        </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public Color TextColor
        {
            get { return (Color)this.GetValue(TextColorProperty); }
            set { this.SetValue(TextColorProperty, value); }
        }

        Font ITextElement.Font => (Font)GetValue(FontElement.FontProperty);

        #endregion

        #region Methods

        double ITextElement.FontSizeDefaultValueCreator()
        {
            return 12d;
        }

        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {

        }

        void ITextElement.OnFontChanged(Font oldValue, Font newValue)
        {

        }

        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {

        }

        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {

        }

        #endregion
    }
}
