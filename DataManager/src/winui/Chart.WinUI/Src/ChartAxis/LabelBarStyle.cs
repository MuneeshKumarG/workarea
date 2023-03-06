namespace Syncfusion.UI.Xaml.Charts
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Media;

    /// <summary>
    /// <see cref="LabelStyle"/> class is used to customize the text properties such as font family, size, and foreground.
    /// </summary>
    public class LabelStyle : DependencyObject
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="FontFamily"/> property.
        /// </summary>
        public static readonly DependencyProperty FontFamilyProperty =
            DependencyProperty.Register(
                "FontFamily",
                typeof(FontFamily),
                typeof(LabelStyle),
                new PropertyMetadata(null));

        /// <summary>
        /// The DependencyProperty for <see cref="Foreground"/> property.
        /// </summary>
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register(
                "Foreground",
                typeof(SolidColorBrush),
                typeof(LabelStyle),
                new PropertyMetadata(null));

        /// <summary>
        /// The DependencyProperty for <see cref="FontSize"/> property.
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register(
                "FontSize",
                typeof(double),
                typeof(LabelStyle),
                new PropertyMetadata(null));

        /// <summary>
        /// The DependencyProperty for <see cref="LabelFormat"/> property.
        /// </summary>        
        public static readonly DependencyProperty LabelFormatProperty =
            DependencyProperty.Register(
                "LabelFormat",
                typeof(string),
                typeof(LabelStyle),
                new PropertyMetadata(string.Empty));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LabelStyle"/> class.
        /// </summary>
        public LabelStyle()
        {
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to customize the font family for label.
        /// </summary>
        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the foreground color for label.
        /// </summary>
        public SolidColorBrush Foreground
        {
            get { return (SolidColorBrush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the font size for label.
        /// </summary>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value for the label formatting.
        /// </summary>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///  
        ///     <chart:SfCartesianChart.XAxes>
        ///         <chart:DateTimeAxis >
        ///            <chart:DateTimeAxis.LabelStyle>
        ///                <chart:LabelStyle LabelFormat = "MMM-dd" />
        ///            </chart:DateTimeAxis.LabelStyle>
        ///     </chart:DateTimeAxis>
        ///     </chart:SfCartesianChart.XAxes>
        /// 
        /// </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-2)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// 
        /// LabelStyle labelStyle = new LabelStyle()
        /// {
        ///     LabelFormat = "MMM-dd",
        /// }
        /// DateTimeAxis xaxis = new DateTimeAxis()
        /// {
        ///     LabelStyle = labelStyle,
        /// }
        /// 
        /// chart.XAxes.Add(xaxis);
        /// 
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public string LabelFormat
        {
            get { return (string)GetValue(LabelFormatProperty); }
            set { SetValue(LabelFormatProperty, value); }
        }

        #endregion

        #endregion
    }
}
