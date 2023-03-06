#nullable enable
namespace Syncfusion.UI.Xaml.Charts
{
    using Microsoft.UI.Xaml;

    /// <summary>
    /// ChartTooltipBehavior is often used to specify extra information when the mouse pointer moved over an element.
    /// </summary>
    /// <remarks>
    /// <para>The tooltip displays information while tapping or mouse hovering on the segment. To display the tooltip on the chart, you need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in chart series.</para>
    /// <para>Create an instance of the <see cref="ChartTooltipBehavior"/> and set it to the chart’s <see cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para>It provides the following options to customize the appearance of the tooltip:</para>
    /// <para><b>LabelStyle - </b> To customize the appearance of the tooltip label, refer to the <see cref="LabelStyle"/> property.</para>
    /// <para><b>Style - </b> To customize the appearance of tooltips, refer to the <see cref="Style"/> property.</para>
    /// <para><b>Duration - </b> To show the tooltip with delay and indicate how long the tooltip will be visible, refer to the <see cref="InitialShowDelay"/>, and <see cref="Duration"/> properties.</para>
    /// <para><b>EnableAnimation - </b> To indicate the animation for the tooltip, refer to the <see cref="EnableAnimation"/> property.</para>
    /// </remarks>
    /// <example>
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    /// 
    ///     <!--omitted for brevity-->
    ///
    ///     <chart:SfCartesianChart.TooltipBehavior>
    ///         <chart:ChartTooltipBehavior/>
    ///     </chart:SfCartesianChart.TooltipBehavior>
    ///
    ///     <chart:LineSeries ItemsSource="{Binding Data}"
    ///                      XBindingPath="XValue"
    ///                      YBindingPath="YValue"
    ///                      EnableTooltip="True"/>
    /// 
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// ViewModel viewModel = new ViewModel();
    ///
    /// // omitted for brevity
    /// chart.TooltipBehavior = new ChartTooltipBehavior();
    /// 
    /// LineSeries series = new LineSeries()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue",
    ///    EnableTooltip = true
    /// };
    /// chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    /// </example>
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
        /// Identifies the <see cref="Duration"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the <see cref="Duration"/> dependency property.
        /// </value>
        public static readonly DependencyProperty DurationProperty =
            DependencyProperty.Register(nameof(Duration), typeof(int), typeof(ChartTooltipBehavior), new PropertyMetadata(1000));

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
            if (ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipPathStyle"] is Style style1)
            {
                Style = style1;
            }

            if (ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipLabelStyle"] is Style style2)
            {
                LabelStyle = style2;
            }
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the horizontal alignment for the tooltip.
        /// </summary>
        /// <value>It accepts the <see cref="Microsoft.UI.Xaml.HorizontalAlignment"/> value and the default value is <see cref="HorizontalAlignment.Center"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior HorizontalAlignment ="Right"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    HorizontalAlignment = HorizontalAlignment.Right,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public HorizontalAlignment HorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalAlignmentProperty); }
            set { SetValue(HorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the vertical alignment for the tooltip.
        /// </summary>
        /// <value>It accepts the <see cref="Microsoft.UI.Xaml.VerticalAlignment"/> value and the default value is <see cref="VerticalAlignment.Top"/>.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior VerticalAlignment ="Center"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    VerticalAlignment = VerticalAlignment.Center,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public VerticalAlignment VerticalAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalAlignmentProperty); }
            set { SetValue(VerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to position the tooltip horizontally.
        /// </summary>
        /// <remarks>
        /// The <see cref="HorizontalOffset"/> and the <see cref="VerticalOffset"/> property values provide additional adjustment to position the tooltip. 
        /// </remarks>
        /// <value>It accepts the double values and the default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior HorizontalOffset ="5"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    HorizontalOffset = 5,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double HorizontalOffset
        {
            get { return (double)GetValue(HorizontalOffsetProperty); }
            set { SetValue(HorizontalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to position the tooltip vertically.
        /// </summary>
        /// <remarks>
        /// The <see cref="HorizontalOffset"/> and the <see cref="VerticalOffset"/> property values provide additional adjustment to position the tooltip. 
        /// </remarks>
        /// <value>It accepts the double values and the default value is 0.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior VerticalOffset ="5"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-10)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    VerticalOffset = 5,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        /// <summary>
        /// Gets or sets the delay in milliseconds to show the tooltip once the user interacts with the series.
        /// </summary>
        /// <value>
        /// It accepts the integer values and the default value is 0 milliseconds.
        /// </value>
        /// <remarks>
        /// It specifies the amount of time the user must wait in milliseconds before the tooltip appears when hovering the mouse or touching a chart series.
        /// <para><b>Note:</b> Initial delay only works for the positive values.</para>
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-11)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior InitialShowDelay ="500"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-12)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    InitialShowDelay = 500,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public int InitialShowDelay
        {
            get { return (int)GetValue(InitialShowDelayProperty); }
            set { SetValue(InitialShowDelayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates how long the tooltip will be visible.
        /// </summary>
        /// <value>
        /// It accepts the integer values and the default value is 1000 milliseconds.
        /// </value>
        /// <remarks>
        /// This property specifies how long a tooltip remains visible while the user moves the mouse pointer over the chart series area.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-13)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior Duration ="1500"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-14)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    Duration = 1500,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        public int Duration
        {
            get { return (int)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether the animation is enabled when the tooltip is displayed.
        /// </summary>
        /// <value>
        /// It accepts the bool values and the default value is <c>true</c>.
        /// </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-15)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior EnableAnimation="True"/>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-16)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// // omitted for brevity
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///    EnableAnimation = true,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example> 
        public bool EnableAnimation
        {
            get { return (bool)GetValue(EnableAnimationProperty); }
            set { SetValue(EnableAnimationProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style to customize the appearance of tooltip.
        /// </summary>
        /// <value>
        /// It accepts <c>Style</c> for tooltip.
        /// </value>
        /// <remarks>It's used to customize the fill and stroke of the tooltip.</remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-17)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior>
        ///             <chart:ChartTooltipBehavior.Style>
        ///                <Style TargetType = "Path">
        ///                    <Setter Property="Stroke" Value="Black" />
        ///                    <Setter Property ="Fill" Value="LightGreen" />
        ///                </Style>
        ///             </chart:ChartTooltipBehavior.Style>
        ///         </chart:ChartTooltipBehavior>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///         
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-18)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// var style = new Style() { TargetType = typeof(Path) };
        /// style.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Black)));
        ///	style.Setters.Add(new Setter(Path.FillProperty, new SolidColorBrush(Colors.Black)));
        ///	chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///	    Style = style,
        ///	};
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Style Style
        {
            get { return (Style)GetValue(StyleProperty); }
            set { SetValue(StyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style to customize the appearance of tooltip label.
        /// </summary>
        /// <value>
        /// It accepts <c>Style</c> for the tooltip label.
        /// </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-19)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        /// 
        ///     <chart:SfCartesianChart.TooltipBehavior>
        ///         <chart:ChartTooltipBehavior>
        ///             <chart:ChartTooltipBehavior.LabelStyle>
        ///                <Style TargetType = "TextBlock" >
        ///                    <Setter Property ="FontSize" Value="14"/>
        ///                    <Setter Property ="Foreground" Value="Red"/>
        ///                    <Setter Property ="FontStyle" Value="Italic"/>
        ///                </Style>
        ///            </chart:ChartTooltipBehavior.LabelStyle>
        ///         </chart:ChartTooltipBehavior>
        ///     </chart:SfCartesianChart.TooltipBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"
        ///                       EnableTooltip="True"/>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-20)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///
        /// var labelStyle = new Style() { TargetType = typeof(TextBlock) };
        /// labelStyle.Setters.Add(new Setter(TextBlock.FontSizeProperty, 14));
        /// labelStyle.Setters.Add(new Setter(TextBlock.ForegroundProperty, new SolidColorBrush(Colors.Red)));
        /// labelStyle.Setters.Add(new Setter(TextBlock.FontStyleProperty, FontStyle.Italic));
        /// chart.TooltipBehavior = new ChartTooltipBehavior()
        /// {
        ///.    LabelStyle = labelStyle,
        ///	};
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        ///    EnableTooltip = true
        /// };
        /// chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        #endregion

        #endregion

    }
}