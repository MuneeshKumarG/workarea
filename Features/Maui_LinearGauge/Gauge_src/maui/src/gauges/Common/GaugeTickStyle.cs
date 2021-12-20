using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents the base style of axis ticks, that used to customize axis ticks color, length, thickness and dash array.
    /// </summary>
    public abstract class GaugeTickStyle : BindableObject
    {
        #region Bindable properties

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Stroke"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeProperty =
           BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(GaugeTickStyle), null, defaultValueCreator: bindable => new SolidColorBrush(Color.FromRgb(125, 143, 155)));

        /// <summary>
        /// Identifies the <see cref="StrokeThickness"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StrokeThickness"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeThicknessProperty =
           BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(GaugeTickStyle), 1d);

        /// <summary>
        /// Identifies the <see cref="Length"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Length"/> bindable property.
        /// </value>
        public static readonly BindableProperty LengthProperty =
           BindableProperty.Create(nameof(Length), typeof(double), typeof(GaugeTickStyle), defaultValueCreator: bindable =>
           {
               if (bindable is RadialTickStyle radialTickStyle)
               {
                   return radialTickStyle.IsMajorTicks ? 8d : 4d;
               }
               return 0d;
           });

        /// <summary>
        /// Identifies the <see cref="StrokeDashArray"/> bindable property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="StrokeDashArray"/> bindable property.
        /// </value>
        public static readonly BindableProperty StrokeDashArrayProperty =
           BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(GaugeTickStyle), null);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets color of axis major or minor ticks. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.MajorTickStyle>
        ///                 <gauge:RadialTickStyle Stroke="Red" />
        ///             </ gauge:RadialAxis.MajorTickStyle>
        ///             <gauge:RadialAxis.MinorTickStyle>
        ///                 <gauge:RadialTickStyle Stroke="Black" />
        ///             </ gauge:RadialAxis.MinorTickStyle>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public Brush Stroke
        {
            get { return (Brush)this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets tick thickness of axis major or minor ticks. 
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <gauge:RadialAxis>
        ///             <gauge:RadialAxis.MajorTickStyle>
        ///                 <gauge:RadialTickStyle StrokeThickness="1.5" />
        ///             </ gauge:RadialAxis.MajorTickStyle>
        ///             <gauge:RadialAxis.MinorTickStyle>
        ///                 <gauge:RadialTickStyle StrokeThickness="1" />
        ///             </ gauge:RadialAxis.MinorTickStyle>
        ///         </gauge:RadialAxis>
        ///     </gauge:SfRadialGauge.Axes>
        /// </gauge:SfRadialGauge>
        /// ]]></code>
        /// </example>
        public double StrokeThickness
        {
            get { return (double)this.GetValue(StrokeThicknessProperty); }
            set { this.SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets length of axis major or minor ticks. 
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
        public double Length
        {
            get { return (double)this.GetValue(LengthProperty); }
            set { this.SetValue(LengthProperty, value); }
        }

        /// <summary>
        /// Gets or sets double collection, that represents dash array of major or minor tick.  
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
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)this.GetValue(StrokeDashArrayProperty); }
            set { this.SetValue(StrokeDashArrayProperty, value); }
        }

        #endregion
    }
}
