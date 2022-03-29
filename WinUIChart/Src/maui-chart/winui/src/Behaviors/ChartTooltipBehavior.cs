namespace Syncfusion.UI.Xaml.Charts
{
    using Microsoft.UI.Xaml;

    /// <summary>
    /// <see cref="ChartTooltipBehavior"/> displays tooltip for the data points nearer to mouse over position or at touch contact point inside a chart area when <see cref="ChartSeriesBase.ShowTooltip"/> property is <c>true</c>.
    /// </summary>
    /// <example>
    /// # [XAML](#tab/tabid-1)
    /// <code language="XAML">
    /// &lt;syncfusion:SfChart&gt;
    ///    &lt;syncfusion:SfChart.Behaviors&gt;
    ///        &lt;syncfusion:ChartTooltipBehavior/&gt;
    ///    &lt;/syncfusion:SfChart.Behaviors&gt;
    /// &lt;/syncfusion:SfChart&gt;
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code language="C#">
    /// ChartTooltipBehavior tooltip = new ChartTooltipBehavior();
    /// chartArea.Behaviors.Add(tooltip);
    /// </code>
    /// </example>
    /// <remarks>
    /// <para> 
    /// The <see cref="ChartTooltipBehavior"/> is commonly used for all series to customize the tooltip. 
    /// You can use the attached <see cref="ChartTooltip"/> properties in a series if you need to customize the appearance of the tooltip based on a particular series. 
    /// Series attached properties is considered as high precedence.</para>
    /// </remarks>
    public class ChartTooltipBehavior : ChartBehavior
    {
        #region DependencyProperty

        /// <summary>
        /// Identifies the <see cref="HorizontalAlignment"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="HorizontalAlignment"/> dependency property.
        /// </value>
        public static readonly DependencyProperty HorizontalAlignmentProperty =
            DependencyProperty.Register(nameof(HorizontalAlignment), typeof(HorizontalAlignment), typeof(ChartTooltipBehavior), new PropertyMetadata(HorizontalAlignment.Center));

        /// <summary>
        /// Identifies the <see cref="VerticalAlignment"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="VerticalAlignment"/> dependency property.
        /// </value>
        public static readonly DependencyProperty VerticalAlignmentProperty =
            DependencyProperty.Register(nameof(VerticalAlignment), typeof(VerticalAlignment), typeof(ChartTooltipBehavior), new PropertyMetadata(VerticalAlignment.Top));

        /// <summary>
        /// Identifies the <see cref="HorizontalOffset"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="HorizontalOffset"/> dependency property.
        /// </value>
        public static readonly DependencyProperty HorizontalOffsetProperty =
            DependencyProperty.Register(nameof(HorizontalOffset), typeof(double), typeof(ChartTooltipBehavior), new PropertyMetadata(0d));

        /// <summary>
        /// Identifies the <see cref="VerticalOffset"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="VerticalOffset"/> dependency property.
        /// </value>
        public static readonly DependencyProperty VerticalOffsetProperty =
            DependencyProperty.Register(nameof(VerticalOffset), typeof(double), typeof(ChartTooltipBehavior), new PropertyMetadata(0d));

        /// <summary>
        /// Identifies the <see cref="InitialShowDelay"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="InitialShowDelay"/> dependency property.
        /// </value>
        public static readonly DependencyProperty InitialShowDelayProperty =
            DependencyProperty.Register(nameof(InitialShowDelay), typeof(int), typeof(ChartTooltipBehavior), new PropertyMetadata(0));

        /// <summary>
        /// Identifies the <see cref="ShowDuration"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="ShowDuration"/> dependency property.
        /// </value>
        public static readonly DependencyProperty ShowDurationProperty =
            DependencyProperty.Register(nameof(ShowDuration), typeof(int), typeof(ChartTooltipBehavior), new PropertyMetadata(1000));

        /// <summary>
        /// Identifies the <see cref="EnableAnimation"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="EnableAnimation"/> dependency property.
        /// </value>
        public static readonly DependencyProperty EnableAnimationProperty =
            DependencyProperty.Register(nameof(EnableAnimation), typeof(bool), typeof(ChartTooltipBehavior), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="Style"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="Style"/> dependency property.
        /// </value>
        public static readonly DependencyProperty StyleProperty =
            DependencyProperty.Register(
                nameof(Style),
                typeof(Style),
                typeof(ChartTooltipBehavior),
                null);

        /// <summary>
        /// Identifies the <see cref="LabelStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="LabelStyle"/> dependency property.
        /// </value>
        public static readonly DependencyProperty LabelStyleProperty =
            DependencyProperty.Register(
                nameof(LabelStyle),
                typeof(Style),
                typeof(ChartTooltipBehavior),
                new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Margin"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="Margin"/> dependency property.
        /// </value>
        internal static readonly DependencyProperty MarginProperty =
            DependencyProperty.Register(nameof(Margin), typeof(Thickness), typeof(ChartTooltipBehavior), new PropertyMetadata(new Thickness().GetThickness(0, 0, 0, 0)));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartTooltipBehavior"/>.
        /// </summary>
        public ChartTooltipBehavior()
        {
            Style = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipPathStyle"] as Style;
            LabelStyle = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipLabelStyle"] as Style;
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the horizontal alignment of tooltip label with respect to the cursor position.
        /// </summary>
        /// <value>
        /// <c>HorizontalAlignment</c>. The default value is <c>HorizontalAlignment.Center</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code language = "XAML">
        /// <![CDATA[
        /// <chart:SfChart.Behaviors>
        ///    <chart:ChartTooltipBehavior x:Name="chartTooltipBehavior" HorizontalAlignment="Right"/>
        /// </chart:SfChart.Behaviors>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language = "C#">
        /// <![CDATA[
        /// ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
        /// chartTooltipBehavior.HorizontalAlignment = HorizontalAlignment.Right;
        /// chart.Behaviors.Add(chartTooltipBehavior);
        /// ]]>
        /// </code>
        /// </example>
        public HorizontalAlignment HorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentProperty); }
            set { SetValue(HorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical alignment of tooltip label with respect to the cursor position.
        /// </summary>
        /// <value>
        /// <c>VerticalAlignment</c>. The default value is <c>VerticalAlignment.Top</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code language = "XAML">
        /// <![CDATA[
        /// <chart:SfChart.Behaviors>
        ///    <chart:ChartTooltipBehavior x:Name="chartTooltipBehavior" VerticalAlignment="Bottom"/>
        /// </chart:SfChart.Behaviors>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language = "C#">
        /// <![CDATA[
        /// ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
        /// chartTooltipBehavior.VerticalAlignment = VerticalAlignment.Bottom;
        /// chart.Behaviors.Add(chartTooltipBehavior);
        /// ]]>
        /// </code>
        /// </example>
        public VerticalAlignment VerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignmentProperty); }
            set { SetValue(VerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to position the tooltip at a distance from the cursor horizontally.
        /// </summary>
        /// <value>
        /// <c>double</c>. The default value is 0.
        /// </value>
        /// <remarks>
        /// The <see cref="HorizontalOffset"/> property and the <see cref="VerticalOffset"/> property values provide additional adjustment to position the tooltip. 
        /// </remarks>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code language = "XAML">
        /// <![CDATA[
        /// <chart:SfChart.Behaviors>
        ///    <chart:ChartTooltipBehavior x:Name="chartTooltipBehavior" HorizontalOffset="10"/>
        /// </chart:SfChart.Behaviors>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language = "C#">
        /// <![CDATA[
        /// ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
        /// chartTooltipBehavior.HorizontalOffset = 10;
        /// chart.Behaviors.Add(chartTooltipBehavior);
        /// ]]>
        /// </code>
        /// </example>
        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to position the tooltip at a distance from the cursor vertically.
        /// </summary>
        /// <value>
        /// <c>double</c>. The default value is 0.
        /// </value>
        /// <remarks>
        /// The <see cref="HorizontalOffset"/> property and the <see cref="VerticalOffset"/> property values provide additional adjustment to position the tooltip.
        /// </remarks>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code language = "XAML">
        /// <![CDATA[
        /// <chart:SfChart.Behaviors>
        ///     <chart:ChartTooltipBehavior x:Name="chartTooltipBehavior" VerticalOffset="10"/>
        /// </chart:SfChart.Behaviors>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language = "C#">
        /// <![CDATA[
        /// ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
        /// chartTooltipBehavior.VerticalOffset = 10;
        /// chart.Behaviors.Add(chartTooltipBehavior);
        /// ]]>
        /// </code>
        /// </example>
        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the delay in milliseconds to show the tooltip once user interact with series.
        /// </summary>
        /// <value>
        /// <c>integer</c>. The default value is 0 milliseconds.
        /// </value>
        /// <remarks>
        /// Used to specify the amount of time before the user has to wait when hover the mouse or touch on chart series in milliseconds before tooltip display.
        /// <para><b>Note:</b> Initial delay only works for the positive values.</para>
        /// </remarks>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code language = "XAML">
        /// <![CDATA[
        /// <chart:SfChart.Behaviors>
        ///    <chart:ChartTooltipBehavior x:Name="chartTooltipBehavior" InitialShowDelay="1000"/>
        /// </chart:SfChart.Behaviors>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language = "C#">
        /// <![CDATA[
        /// ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
        /// chartTooltipBehavior.InitialShowDelay = 1000;
        /// chart.Behaviors.Add(chartTooltipBehavior);
        /// ]]>
        /// </code>
        /// </example>
        public int InitialShowDelay
        {
            get { return (int)GetValue(InitialShowDelayProperty); }
            set { SetValue(InitialShowDelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the amount of time that the tooltip remains visible in milliseconds.
        /// </summary>
        /// <value>
        /// <c>integer</c>. The default value is 1000 milliseconds.
        /// </value>
        /// <remarks>
        /// This property defines the time that a tooltip remains visible while the user pauses the mouse pointer over the chart series area that defines the tooltip.
        /// </remarks>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code language = "XAML">
        /// <![CDATA[
        /// <chart:SfChart.Behaviors>
        ///    <chart:ChartTooltipBehavior x:Name="chartTooltipBehavior" ShowDuration="3000"/>
        /// </chart:SfChart.Behaviors>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language = "C#">
        /// <![CDATA[
        /// ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
        /// chartTooltipBehavior.ShowDuration = 3000;
        /// chart.Behaviors.Add(chartTooltipBehavior);
        /// ]]>
        /// </code>
        /// </example>
        public int ShowDuration
        {
            get { return (int)GetValue(ShowDurationProperty); }
            set { SetValue(ShowDurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether to enable the animation when showing tooltip.
        /// </summary>
        /// <value>
        /// <c>bool</c>. The default value is <c>true</c>.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code language = "XAML">
        /// <![CDATA[
        /// <chart:SfChart.Behaviors>
        ///     <chart:ChartTooltipBehavior x:Name="chartTooltipBehavior" EnableAnimation="true"/>
        /// </chart:SfChart.Behaviors>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language = "C#">
        /// <![CDATA[
        /// ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
        /// chartTooltipBehavior.EnableAnimation = true;
        /// chart.Behaviors.Add(chartTooltipBehavior);
        /// ]]>
        /// </code>
        /// </example>
        public bool EnableAnimation
        {
            get { return (bool)GetValue(EnableAnimationProperty); }
            set { SetValue(EnableAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style to customize the fill and stroke of tooltip.
        /// </summary>
        /// <value>
        /// The <c>Style</c> for tooltip.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code language = "XAML">
        /// <![CDATA[
        /// <syncfusion:SfChart.Resources>
        ///     <Style TargetType="Path" x:Key="style">
        ///        <Setter Property="Stroke" Value="Black"/>
        ///        <Setter Property="Fill" Value="Gray"/>
        ///     </Style>
        /// </syncfusion:SfChart.Resources>
        /// <syncfusion:SfChart.Behaviors>
        ///     <syncfusion:ChartTooltipBehavior Style = {StaticResource style}/>
        /// </syncfusion:SfChart.Behaviors>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language = "C#">
        /// <![CDATA[
        /// SfChart chart = new SfChart();
        /// Style style = new Style(typeof(Path));
        /// style.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Black)));
        /// style.Setters.Add(new Setter(Path.FillProperty, new SolidColorBrush(Colors.Gray)));
        /// ChartTooltipBehavior tooltipBehavior = new ChartTooltipBehavior();
        /// tooltipBehavior.Style = style;
        /// chart.Behaviors.Add(tooltipBehavior);
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// To define a <c>Style</c> for tooltip, specify the style of TargetType as <c>Path</c>.
        /// </remarks>
        public Style Style
        {
            get { return (Style)GetValue(StyleProperty); }
            set { SetValue(StyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style to customize the tooltip label.
        /// </summary>
        /// <value>
        /// The <c>Style</c> for tooltip label.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code language = "XAML">
        /// <![CDATA[
        /// <syncfusion:SfChart.Resources>
        ///     <Style TargetType="TextBlock" x:Key="labelStyle">
        ///        <Setter Property="FontSize" Value="14"/>
        ///        <Setter Property="Foreground" Value="Red"/>
        ///     </Style>
        /// </syncfusion:SfChart.Resources>
        /// <syncfusion:SfChart.Behaviors>
        ///    <syncfusion:ChartTooltipBehavior LabelStyle = {StaticResource labelStyle}/>
        /// </syncfusion:SfChart.Behaviors>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language = "C#">
        /// <![CDATA[
        /// SfChart chart = new SfChart();
        /// Style labelStyle = new Style(typeof(TextBlock));
        /// labelStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, 14d));
        /// labelStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(Colors.Red)));
        /// ChartTooltipBehavior tooltipBehavior = new ChartTooltipBehavior();
        /// tooltipBehavior.LabelStyle = labelStyle;
        /// chart.Behaviors.Add(tooltipBehavior);
        /// ]]>
        /// </code>
        /// </example>
        /// <remarks>
        /// To define a <c>Style</c> for tooltip label, specify the style of TargetType as <c>TextBlock</c>.
        /// </remarks>
        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that used to set margin for tooltip label.
        /// </summary>
        /// <value>
        /// <c>Thickness</c>. The default value of margin is 0.
        /// </value>
        /// <example>
        /// # [XAML](#tab/tabid-1)
        /// <code language = "XAML">
        /// <![CDATA[
        /// <chart:SfChart.Behaviors>
        ///    <chart:ChartTooltipBehavior x:Name="chartTooltipBehavior" Margin="5,5,5,5"/>
        /// </chart:SfChart.Behaviors>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language = "C#">
        /// <![CDATA[
        /// ChartTooltipBehavior chartTooltipBehavior = new ChartTooltipBehavior();
        /// chartTooltipBehavior.Margin = new Thickness(5);
        /// chart.Behaviors.Add(chartTooltipBehavior);
        /// ]]>
        /// </code>
        /// </example>
        internal Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        #endregion

        #endregion

    }
}