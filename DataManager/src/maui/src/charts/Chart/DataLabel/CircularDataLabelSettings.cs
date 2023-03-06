using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// The CircularDataLabelSettings class is used to customize the appearance of circular series data labels.
    /// </summary>
    /// <remarks>
    /// <para>To customize data labels, create an instance of the <see cref="CircularDataLabelSettings"/> class, and set it to the DataLabelSettings property of the circular series.</para>
    /// 
    /// <para><b>ShowDataLabels</b></para>
    /// <para>Data labels can be added to a chart series by enabling the <see cref="ChartSeries.ShowDataLabels"/> option.</para>
    /// 
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///    <chart:SfCircularChart>
    ///
    ///       <chart:PieSeries ItemsSource ="{Binding Data}"
    ///                        XBindingPath="XValue"
    ///                        YBindingPath="YValue"
    ///                        ShowDataLabels="True"/>
    ///
    ///    </chart:SfCircularChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCircularChart chart = new SfCircularChart();
    ///     ViewModel viewModel = new ViewModel();
    ///     
    ///     PieSeries series = new PieSeries()
    ///     {
    ///        ItemsSource = viewModel.Data,
    ///        XBindingPath = "XValue",
    ///        YBindingPath = "YValue",
    ///        ShowDataLabels = true
    ///     };
    ///
    ///     chart.Series.Add(series);
    ///
    /// ]]></code>
    /// ***
    /// 
    /// <para><b>Customization</b></para>
    /// <para>To change the appearance of data labels, it offers <see cref="ChartDataLabelSettings.LabelStyle"/> options.</para>
    ///
    /// # [Xaml](#tab/tabid-3)
    /// <code><![CDATA[
    ///    <chart:SfCircularChart>
    ///
    ///       <chart:PieSeries ItemsSource ="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"
    ///                        ShowDataLabels="True">
    ///          <chart:PieSeries.DataLabelSettings>
    ///              <chart:CircularDataLabelSettings>
    ///                    <chart:CircularDataLabelSettings.LabelStyle>
    ///                        <chart:ChartDataLabelStyle Background = "Red" FontSize="14" TextColor="Black" />
    ///                    </chart:CircularDataLabelSettings.LabelStyle>
    ///                </chart:CircularDataLabelSettings>
    ///          </chart:PieSeries.DataLabelSettings>
    ///       </chart:PieSeries>
    ///    </chart:SfCircularChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-4)
    /// <code><![CDATA[
    ///     SfCircularChart chart = new SfCircularChart();
    ///     ViewModel viewModel = new ViewModel();
    ///     
    ///     PieSeries series = new PieSeries()
    ///     {
    ///        ItemsSource = viewModel.Data,
    ///        XBindingPath = "XValue",
    ///        YBindingPath = "YValue",
    ///        ShowDataLabels = true,
    ///     };
    ///     
    ///     var labelStyle = new ChartDataLabelStyle()
    ///     {
    ///         Background = new SolidColorBrush(Colors.Red),
    ///         TextColor = Colors.Black,
    ///         FontSize = 14,
    ///     };
    ///     series.DataLabelSettings = new CircularDataLabelSettings()
    ///     {
    ///         LabelStyle = labelStyle,
    ///     };
    ///     
    ///     chart.Series.Add(series);
    ///
    /// ]]></code>
    /// *** 
    /// 
    /// </remarks>
    public class CircularDataLabelSettings : ChartDataLabelSettings
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="ConnectorType"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ConnectorTypeProperty =
            BindableProperty.Create(nameof(ConnectorType), typeof(ConnectorType), typeof(CircularDataLabelSettings), ConnectorType.Line, BindingMode.Default, null, null);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularDataLabelSettings"/>.
        /// </summary>
        public CircularDataLabelSettings()
        {
            LabelStyle = new ChartDataLabelStyle() { FontSize = 14, Margin = new Thickness(8, 6) };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that can be used to specify the connector type.
        /// </summary>
        /// <value>It accepts <see cref="Charts.ConnectorType"/> values and its default value is <see cref="ConnectorType.Line"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///          <chart:PieSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ShowDataLabels="True">
        ///                <chart:PieSeries.DataLabelSettings>
        ///                    <chart:CircularDataLabelSettings ConnectorType="Curve" />
        ///                </chart:PieSeries.DataLabelSettings>
        ///              </chart:PieSeries>
        ///
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     var dataLabelSettings = new CircularDataLabelSettings()
        ///     {
        ///         ConnectorType = ConnectorType.Curve
        ///     };
        ///     PieSeries series = new PieSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowDataLabels = true,
        ///           DataLabelSettings = dataLabelSettings,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public ConnectorType ConnectorType
        {
            get { return (ConnectorType)GetValue(ConnectorTypeProperty); }
            set { SetValue(ConnectorTypeProperty, value); }
        }

        #endregion
    }
}