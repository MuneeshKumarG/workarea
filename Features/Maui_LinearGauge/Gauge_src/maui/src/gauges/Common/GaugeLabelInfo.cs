using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Gauges
{
    /// <summary>
    /// Represents information about the axis labels that associate a numeric value position for major scale tick marks.
    /// </summary>
    /// <example>
    /// <code><![CDATA[
    /// public class RadialAxisExt : RadialAxis
    /// {
    ///     public override List<GaugeLabelInfo> GenerateVisibleLabels()
    ///     {
    ///         List<GaugeLabelInfo> customLabels = new List<GaugeLabelInfo>();
    ///         for (int i = 0; i < 9; i++)
    ///         {
    ///             double value = i;
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
    /// }
    /// ]]></code>
    /// </example>
    public class GaugeLabelInfo
    {
        #region Fields

        internal Size DesiredSize;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GaugeLabelInfo"/> class.
        /// </summary>
        /// <example>
        ///  # [XAML](#tab/tabid-1)
        /// <code><![CDATA[
        /// <gauge:SfRadialGauge>
        ///     <gauge:SfRadialGauge.Axes>
        ///         <local:RadialAxisExt Minimum="0"
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
        ///     public override List<GaugeLabelInfo> GenerateVisibleLabels()
        ///     {
        ///         List<GaugeLabelInfo> customLabels = new List<GaugeLabelInfo>();
        ///         for (int i = 0; i < 9; i++)
        ///         {
        ///             double value = i;
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
        /// }
        /// ]]></code>
        /// </example>
        public GaugeLabelInfo()
        {
            this.Text = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the position value of the axis label.
        /// </summary>
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
        ///     public override List<GaugeLabelInfo> GenerateVisibleLabels()
        ///     {
        ///         List<GaugeLabelInfo> customLabels = new List<GaugeLabelInfo>();
        ///         for (int i = 0; i < 9; i++)
        ///         {
        ///             double value = i;
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
        /// }
        /// ]]></code>
        /// </example>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the text contents of the axis label.
        /// </summary>
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
        ///     public override List<GaugeLabelInfo> GenerateVisibleLabels()
        ///     {
        ///         List<GaugeLabelInfo> customLabels = new List<GaugeLabelInfo>();
        ///         for (int i = 0; i < 9; i++)
        ///         {
        ///             double value = i;
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
        /// }
        /// ]]></code>
        /// </example>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the rotation angle of label.
        /// </summary>
        internal double RotationAngle { get; set; }

        /// <summary>
        /// Gets or sets the position of the axis label.
        /// </summary>
        internal PointF Position { get; set; }

        /// <summary>
        /// To set the font style to gauge label text.
        /// </summary>
        internal GaugeLabelStyle? LabelStyle { get; set; }

        #endregion
    }
}
