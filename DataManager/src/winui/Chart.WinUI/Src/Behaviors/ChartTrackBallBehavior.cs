using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;
using Microsoft.UI;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// ChartTrackballBehavior tracks data points in a chart closer to the mouse hover position or at a touch contact point.
    /// </summary>
    /// <remarks>
    /// <para>To enable the track ball in the chart, create an instance of <see cref="ChartTrackballBehavior"/> and set it to the <see cref="TrackballBehavior"/> property of SfCartesianChart.</para>
    /// <para>To view the trackball label in the particular axis, you have to enable the <see cref="ChartAxis.ShowTrackballLabel"/> property in that axis.</para>
    /// <para>It provides options to customize the trackball line, symbol, and label.</para>
    ///
    /// <b>LineStyle</b>
    /// 
    /// <para><b>LineStyle</b> is used to customize the appearance of the trackball line.</para>
    /// 
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <!--omitted for brevity-->
    ///           
    ///           <chart:SfCartesianChart.TrackballBehavior>
    ///               <chart:ChartTrackballBehavior>
    ///                   <chart:ChartTrackballBehavior.LineStyle>
    ///                       <Style TargetType = "Line">
    ///                           <Setter Property="StrokeDashArray" Value="5,2"/>
    ///                           <Setter Property = "Stroke" Value="Red"/>
    ///                       </Style>
    ///                   </chart:ChartTrackballBehavior.LineStyle>
    ///               </chart:ChartTrackballBehavior>
    ///           </chart:SfCartesianChart.TrackballBehavior>
    ///
    ///           <chart:LineSeries ItemsSource="{Binding Data}"
    ///                             XBindingPath="XValue"
    ///                             YBindingPath="YValue"/>
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     ViewModel viewModel = new ViewModel();
    ///     
    ///     // omitted for brevity
    ///     DoubleCollection doubleCollection = new DoubleCollection();
    ///	    doubleCollection.Add(5);
    ///	    doubleCollection.Add(3);
    ///	    var lineStyle = new Style() { TargetType = typeof(Line) };
    ///     lineStyle.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Red)));
    ///	    lineStyle.Setters.Add(new Setter(Path.StrokeDashArrayProperty, doubleCollection));
    ///	    chart.TrackballBehavior = new ChartTrackballBehavior()
    ///     {
    ///        LineStyle = lineStyle,
    ///	    };
    /// 
    ///     LineSeries series = new LineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    ///     
    /// ]]>
    /// </code>
    /// # [ViewModel](#tab/tabid-3)
    /// <code><![CDATA[
    ///     public ObservableCollection<Model> Data { get; set; }
    /// 
    ///     public ViewModel()
    ///     {
    ///        Data = new ObservableCollection<Model>();
    ///        Data.Add(new Model() { XValue = 10, YValue = 100 });
    ///        Data.Add(new Model() { XValue = 20, YValue = 150 });
    ///        Data.Add(new Model() { XValue = 30, YValue = 110 });
    ///        Data.Add(new Model() { XValue = 40, YValue = 230 });
    ///     }
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <b>ChartTrackballStyle</b>
    ///
    /// <para><b>ChartTrackballStyle</b> is used to customize the appearance of the trackball symbol. By default, the trackball symbol is displayed as an ellipse.</para>
    ///
    /// # [Xaml](#tab/tabid-4)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <!--omitted for brevity-->
    ///
    ///           <chart:SfCartesianChart.TrackballBehavior>
    ///               <chart:ChartTrackballBehavior>
    ///                   <chart:ChartTrackballBehavior.ChartTrackballStyle>
    ///                       <Style TargetType="chart:ChartTrackballControl">
    ///                           <Setter Property = "Background" Value="Red"/>
    ///                       </Style>
    ///                   </chart:ChartTrackballBehavior.ChartTrackballStyle>
    ///               </chart:ChartTrackballBehavior>
    ///           </chart:SfCartesianChart.TrackballBehavior>
    ///
    ///           <chart:LineSeries ItemsSource="{Binding Data}"
    ///                             XBindingPath="XValue"
    ///                             YBindingPath="YValue"/>
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-5)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///    // omitted for brevity
    ///    var trackballStyle = new Style() { TargetType = typeof(ChartTrackballControl) };
    ///    trackballStyle.Setters.Add(new Setter(ChartTrackballControl.BackgroundProperty, new SolidColorBrush(Colors.Red)));
    ///    chart.TrackballBehavior = new ChartTrackballBehavior()
    ///    {
    ///       ChartTrackballStyle = trackballStyle,
    ///    };
    ///
    ///     LineSeries series = new LineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <b>UseSeriesPalette</b>
    /// 
    /// <para>Series color is applied to the trackball label by setting <b>UseSeriesPalette</b> to true, and its default value is false.</para>
    ///
    /// # [Xaml](#tab/tabid-6)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <!--omitted for brevity-->
    ///
    ///           <chart:SfCartesianChart.TrackballBehavior>
    ///               <chart:ChartTrackballBehavior UseSeriesPalette="True"/>
    ///           </chart:SfCartesianChart.TrackballBehavior>
    ///
    ///           <chart:LineSeries ItemsSource="{Binding Data}"
    ///                             XBindingPath="XValue"
    ///                             YBindingPath="YValue"/>  
    ///
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-7)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///    // omitted for brevity
    ///    chart.TrackballBehavior = new ChartTrackballBehavior()
    ///    {
    ///       UseSeriesPalette = true,
    ///    };
    ///
    ///     LineSeries series = new LineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <b>ShowTrackballLabel</b>
    ///
    /// <para>The axis label will be viewed when the ShowTrackballLabel property is set to true. The default value of ShowTrackballLabel is false</para>
    ///
    /// # [Xaml](#tab/tabid-8)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.XAxes>
    ///               <chart:NumericalAxis ShowTrackballLabel="True"/>
    ///           </chart:SfCartesianChart.XAxes>
    ///
    ///           <chart:SfCartesianChart.YAxes>
    ///               <chart:NumericalAxis  ShowTrackballLabel="True"/>
    ///           </chart:SfCartesianChart.YAxes>
    ///           
    ///           <chart:SfCartesianChart.TrackballBehavior>
    ///               <chart:ChartTrackballBehavior />
    ///           </chart:SfCartesianChart.TrackballBehavior>
    ///
    ///           <chart:LineSeries ItemsSource="{Binding Data}"
    ///                             XBindingPath="XValue"
    ///                             YBindingPath="YValue"
    ///                             ShowTrackballLabel="True"/>
    ///
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-9)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     NumericalAxis xAxis = new NumericalAxis();
    ///     xAxis.ShowTrackballLabel= true;
    ///     chart.XAxes.Add(xAxis);
    ///     
    ///     NumericalAxis yAxis = new NumericalAxis();
    ///     yAxis.ShowTrackballLabel= true;
    ///     chart.YAxes.Add(yAxis);
    ///
    ///     chart.TrackballBehavior = new ChartTrackballBehavior();
    ///
    ///     LineSeries series = new LineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     series.ShowTrackballLabel= true;
    ///     chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <b>TrackballLabelTemplate</b>
    ///
    /// <para>TrackballLabelTemplate is used to customize the appearance of the series label in the trackball.</para>
    ///
    /// # [Xaml](#tab/tabid-10)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <!--omitted for brevity-->
    ///
    ///           <chart:SfCartesianChart.TrackballBehavior>
    ///               <chart:ChartTrackballBehavior />
    ///           </chart:SfCartesianChart.TrackballBehavior>
    ///
    ///           <chart:LineSeries ItemsSource="{Binding Data}"
    ///                             XBindingPath="XValue"
    ///                             YBindingPath="YValue">
    ///                <chart:LineSeries.TrackballLabelTemplate>
    ///                    <DataTemplate>
    ///                        <Border CornerRadius = "5" BorderThickness="1" 
    ///                             BorderBrush="Black" Background="LightGreen" Padding="5">
    ///                            <TextBlock Foreground = "Black" Text="{Binding ValueY}"/>
    ///                        </Border>
    ///                    </DataTemplate>
    ///                </chart:LineSeries.TrackballLabelTemplate>
    ///           </chart:LineSeries>
    ///
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class ChartTrackballBehavior : ChartBehavior
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="AxisLabelAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty AxisLabelAlignmentProperty =
            DependencyProperty.Register(
                nameof(AxisLabelAlignment),
                typeof(ChartAlignment),
                typeof(ChartTrackballBehavior),
                new PropertyMetadata(ChartAlignment.Center));

        /// <summary>
        /// The DependencyProperty for <see cref="DisplayMode"/> property.
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register(
             nameof(DisplayMode),
             typeof(LabelDisplayMode),
             typeof(ChartTrackballBehavior),
             new PropertyMetadata(LabelDisplayMode.FloatAllPoints));

        /// <summary>
        /// The DependencyProperty for <see cref="LabelBackground"/> property.
        /// </summary>
        /// <remarks>
        /// Background brush for grouped labels. 
        /// </remarks>
        internal static readonly DependencyProperty LabelBackgroundProperty =
            DependencyProperty.Register(nameof(LabelBackground), typeof(Brush), typeof(ChartTrackballBehavior),
                new PropertyMetadata(new SolidColorBrush(Colors.White)));

        /// <summary>
        /// The DependencyProperty for <see cref="LineStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty LineStyleProperty =
            DependencyProperty.Register(
                nameof(LineStyle),
                typeof(Style),
                typeof(ChartTrackballBehavior),
                new PropertyMetadata(ChartDictionaries.GenericCommonDictionary["SyncfusionChartTrackballLineStyle"]));

        /// <summary>
        /// The DependencyProperty for <see cref="ShowLine"/> property.
        /// </summary>
        public static readonly DependencyProperty ShowLineProperty =
            DependencyProperty.Register(
                nameof(ShowLine),
                typeof(bool),
                typeof(ChartTrackballBehavior),
                new PropertyMetadata(true, OnShowLinePropertyChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="LabelVerticalAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty LabelVerticalAlignmentProperty =
            DependencyProperty.Register(
                nameof(LabelVerticalAlignment),
                typeof(ChartAlignment),
                typeof(ChartTrackballBehavior),
                new PropertyMetadata(ChartAlignment.Auto));

        /// <summary>
        /// The DependencyProperty for <see cref="LabelHorizontalAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty LabelHorizontalAlignmentProperty =
            DependencyProperty.Register(
                nameof(LabelHorizontalAlignment),
                typeof(ChartAlignment),
                typeof(ChartTrackballBehavior),
                new PropertyMetadata(ChartAlignment.Auto));

        /// <summary>
        /// The DependencyProperty for <see cref="ChartTrackballStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty ChartTrackballStyleProperty =
            DependencyProperty.Register(
                nameof(ChartTrackballStyle),
                typeof(Style),
                typeof(ChartTrackballBehavior),
                null);

        /// <summary>
        /// The DependencyProperty for <see cref="UseSeriesPalette"/> property.
        /// </summary>
        public static readonly DependencyProperty UseSeriesPaletteProperty =
        DependencyProperty.Register(
            nameof(UseSeriesPalette),
            typeof(bool),
            typeof(ChartTrackballBehavior),
            new PropertyMetadata(false, OnLayoutUpdated));
        
        #endregion

        #region Fields

        #region Internal Fields

        internal string labelXValue;
        internal string labelYValue;
        internal bool isOpposedAxis;

        #endregion

        #region Private Fields

        private const int seriesTipHeight = 6;

        private const int axisTipHeight = 6;

        private const int trackLabelSpacing = 8;

        internal string previousXLabel { get; set; }

        string currentXLabel = string.Empty;

        internal string previousYLabel { get; set; } 

        string currentYLabel = string.Empty;

        private bool isActivated;

        private bool isCancel;

        private int seriesCount;

        private Border border;

        private int fingerCount = 0;

        private Line line;

        private List<FrameworkElement> elements;

        private ObservableCollection<ChartPointInfo> pointInfos;

        private List<ChartTrackballControl> trackBalls;

        private ObservableCollection<ChartPointInfo> previousPointInfos;

        private double trackballWidth;

        List<double> yValues;

        private double tempXPos = double.MinValue;

        List<double> Values = new List<double>();

#if WinUI_Desktop
        bool isTrackBallUpdateDispatched = false;
#else
        IAsyncAction updateTrackBallAction;
#endif
        private List<ContentControl> labelElements;

        private List<ContentControl> axisLabelElements;

        private List<ContentControl> groupLabelElements;

        private Dictionary<ChartAxis, ChartPointInfo> axisLabels;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartTrackballBehavior"/> class.
        /// </summary>
        public ChartTrackballBehavior()
        {
            elements = new List<FrameworkElement>();
            pointInfos = new ObservableCollection<ChartPointInfo>();
            line = new Line();
            labelElements = new List<ContentControl>();
            groupLabelElements = new List<ContentControl>();
            axisLabelElements = new List<ContentControl>();
            axisLabels = new Dictionary<ChartAxis, ChartPointInfo>();
            trackBalls = new List<ChartTrackballControl>();
            previousXLabel = string.Empty;
            previousYLabel = string.Empty;
        }

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        internal event EventHandler<PositionChangingEventArgs> PositionChanging;

        /// <summary>
        /// 
        /// </summary>
        internal event EventHandler<PositionChangedEventArgs> PositionChanged;

        #endregion

        #region Properties

        #region Public Properties
        
        internal ObservableCollection<ChartPointInfo> PointInfos
        {
            get
            {
                return pointInfos;
            }

            set
            {
                pointInfos = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ChartAlignment"/> for the label appearing in the axis.
        /// </summary>
        /// <value>This property takes the <see cref="ChartAlignment"/> value and its default value is <see cref="ChartAlignment.Center"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-11)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior AxisLabelAlignment="Near"/>
        ///           </chart:SfCartesianChart.TrackballBehavior>
        ///
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue"/>  
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-12)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///    // omitted for brevity
        ///    chart.TrackballBehavior = new ChartTrackballBehavior()
        ///    {
        ///       AxisLabelAlignment = ChartAlignment.Near,
        ///    };
        ///
        ///     LineSeries series = new LineSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code> 
        /// ***
        /// </example>
        public ChartAlignment AxisLabelAlignment
        {
            get { return (ChartAlignment)GetValue(AxisLabelAlignmentProperty); }
            set { SetValue(AxisLabelAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style to customize the appearance of the trackball line.
        /// </summary>
        /// <value>It takes <see cref="Style"/> value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-13)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///           
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior>
        ///                   <chart:ChartTrackballBehavior.LineStyle>
        ///                       <Style TargetType = "Line" >
        ///                           <Setter Property="StrokeDashArray" Value="5,2"/>
        ///                           <Setter Property = "Stroke" Value="Red"/>
        ///                       </Style>
        ///                   </chart:ChartTrackballBehavior.LineStyle>
        ///               </chart:ChartTrackballBehavior>
        ///           </chart:SfCartesianChart.TrackballBehavior>
        ///
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue"/>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-14)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // omitted for brevity
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(5);
        ///     doubleCollection.Add(3);
        ///     var lineStyle = new Style() { TargetType = typeof(Line) };
        ///     lineStyle.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Red)));
        ///     lineStyle.Setters.Add(new Setter(Path.StrokeDashArrayProperty, doubleCollection));
        ///     chart.TrackballBehavior = new ChartTrackballBehavior()
        ///     {
        ///        LineStyle = lineStyle,
        ///     };
        ///
        ///     LineSeries series = new LineSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Style LineStyle
        {
            get { return (Style)GetValue(LineStyleProperty); }
            set { SetValue(LineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the trackball display mode for the label.
        /// </summary>
        /// <value>This property takes the <see cref="LabelDisplayMode"/> value and its default value is <see cref="LabelDisplayMode.FloatAllPoints"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-15)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior DisplayMode="GroupAllPoints"/>
        ///           </chart:SfCartesianChart.TrackballBehavior>
        ///
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue"/>  
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-16)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///    // omitted for brevity
        ///    chart.TrackballBehavior = new ChartTrackballBehavior()
        ///    {
        ///       DisplayMode = TrackballLabelDisplayMode.GroupAllPoints,
        ///    };
        ///
        ///     LineSeries series = new LineSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public LabelDisplayMode DisplayMode
        {
            get { return (LabelDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush value to customize the label's background appearance.
        /// </summary>
        internal Brush LabelBackground 
        {
            get { return (Brush)GetValue(LabelBackgroundProperty); }
            set { SetValue(LabelBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether to show or hide the trackball line.
        /// </summary>
        /// <value>This property takes the bool values and its default value is <c>True</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-17)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior ShowLine="True"/>
        ///           </chart:SfCartesianChart.TrackballBehavior>
        ///
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue"/>  
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-18)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///    // omitted for brevity
        ///    chart.TrackballBehavior = new ChartTrackballBehavior()
        ///    {
        ///       ShowLine = true,
        ///    };
        ///
        ///     LineSeries series = new LineSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowLine
        {
            get { return (bool)GetValue(ShowLineProperty); }
            set { SetValue(ShowLineProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="ChartAlignment"/> for the vertical alignment of the label.
        /// </summary>
        /// <value>This property takes the <see cref="ChartAlignment"/> value and its default value is <see cref="ChartAlignment.Auto"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-19)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior LabelVerticalAlignment="Near" />
        ///           </chart:SfCartesianChart.TrackballBehavior>
        ///
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue"/>  
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-20)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///    // omitted for brevity
        ///    chart.TrackballBehavior = new ChartTrackballBehavior()
        ///    {
        ///       LabelVerticalAlignment= ChartAlignment.Near,
        ///    };
        ///
        ///     LineSeries series = new LineSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartAlignment LabelVerticalAlignment
        {
            get { return (ChartAlignment)GetValue(LabelVerticalAlignmentProperty); }
            set { SetValue(LabelVerticalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="ChartAlignment"/> for the horizontal alignment of the label.
        /// </summary>
        /// <value>This property takes the <see cref="ChartAlignment"/> value and its default value is <see cref="ChartAlignment.Auto"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-21)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior LabelHorizontalAlignment="Near" />
        ///           </chart:SfCartesianChart.TrackballBehavior>
        ///
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue"/>  
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-22)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///    // omitted for brevity
        ///    chart.TrackballBehavior = new ChartTrackballBehavior()
        ///    {
        ///       LabelHorizontalAlignment= ChartAlignment.Near,
        ///    };
        ///
        ///     LineSeries series = new LineSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartAlignment LabelHorizontalAlignment
        {
            get { return (ChartAlignment)GetValue(LabelHorizontalAlignmentProperty); }
            set { SetValue(LabelHorizontalAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for <see cref="ChartTrackballControl"/> to customize the appearance of the trackball.
        /// </summary>
        /// <value>It takes <see cref="Style"/> value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-23)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior>
        ///                   <chart:ChartTrackballBehavior.ChartTrackballStyle>
        ///                       <Style TargetType="chart:ChartTrackballControl">
        ///                           <Setter Property = "Background" Value="Red"/>
        ///                       </Style>
        ///                   </chart:ChartTrackballBehavior.ChartTrackballStyle>
        ///               </chart:ChartTrackballBehavior>
        ///           </chart:SfCartesianChart.TrackballBehavior>
        ///
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-24)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///    // omitted for brevity
        ///    var trackballStyle = new Style() { TargetType = typeof(ChartTrackballControl) };
        ///    trackballStyle.Setters.Add(new Setter(ChartTrackballControl.BackgroundProperty, new SolidColorBrush(Colors.Red)));
        ///	   chart.TrackballBehavior = new ChartTrackballBehavior()
        ///    {
        ///       ChartTrackballStyle = trackballStyle,
        ///	   };
        ///
        ///     LineSeries series = new LineSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Style ChartTrackballStyle
        {
            get { return (Style)GetValue(ChartTrackballStyleProperty); }
            set { SetValue(ChartTrackballStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to change the color of the labels based on the series color.
        /// </summary>
        /// <value>This property takes the bool values and its default value is <c>False</c>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-25)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior UseSeriesPalette="True"/>
        ///           </chart:SfCartesianChart.TrackballBehavior>
        ///
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue"/>  
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-26)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///    // omitted for brevity
        ///    chart.TrackballBehavior = new ChartTrackballBehavior()
        ///    {
        ///       UseSeriesPalette = true,
        ///    };
        ///
        ///     LineSeries series = new LineSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool UseSeriesPalette
        {
            get { return (bool)GetValue(UseSeriesPaletteProperty); }
            set { SetValue(UseSeriesPaletteProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal Point CurrentPoint { get; set; }
        
        #endregion

        #region Protected Internal Properties

        internal bool IsActivated
        {
            get
            {
                return isActivated;
            }

            set
            {
                isActivated = value;
                Activate(isActivated);
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Methods

        internal void ScheduleTrackBallUpdate()
        {
#if WinUI_Desktop
            if (!isTrackBallUpdateDispatched)
            {
                DispatcherQueue.TryEnqueue(() => { OnPointerPositionChanged(); });
                isTrackBallUpdateDispatched = true;
            }
#else
            if (updateTrackBallAction == null)
            {
                updateTrackBallAction = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, OnPointerPositionChanged);
            }
#endif
        }

        #endregion

        #region Protected Internal Methods

        internal virtual void OnPointerPositionChanged()
        {
            if(Chart == null)
            {
                return;
            }
#if WinUI_Desktop
            isTrackBallUpdateDispatched = false;
#else
            updateTrackBallAction = null;
#endif
            if (!IsActivated)
            {
                return;
            }

            SetPositionChangingEventArgs();
            if (isCancel)
                return;
            previousPointInfos = new ObservableCollection<ChartPointInfo>(pointInfos);
            Point point = CurrentPoint;

            int index = 0;
            double leastX = 0;

            IEnumerable<IGrouping<ChartAxis, ChartSeries>> groupedSeries = this.Chart.VisibleSeries.GroupBy<ChartSeries, ChartAxis>((series) => series.ActualXAxis);
            PointInfos.Clear();
            axisLabels.Clear();
            int count = 0;

            foreach (IGrouping<ChartAxis, ChartSeries> group in groupedSeries)
            {
                ChartAxis axis = group.Key;
                seriesCount = group.Count();
                if (axis == null)
                    continue;
                double leastXPoint = 0, leastYPoint = 0;
                double leastIndex = 0;
                double leastXVal = 0;
                double tempValue = 0;
                Values.Clear();

                foreach (ChartSeries series in group)
                {
                    if (series is CartesianSeries cartesianSeries && (!cartesianSeries.ShowTrackballLabel))
                        continue;

                    bool isGrouping = series.ActualXAxis is CategoryAxis && (series.ActualXAxis is CategoryAxis actualXAxis) && !actualXAxis.IsIndexed;
                    if (series.IsActualTransposed)
                        IsReversed = true;
                    else
                        IsReversed = false;

                    if (series.PointsCount > 0)
                    {
                        double xVal = 0;
                        double yVal = 0;
                        double stackedYValue = double.NaN;
                        double x = 0;
                        double y = 0;
                        yValues = new List<double>();
                        bool isStackedSeries = series is StackedSeriesBase;
                        if (isGrouping && !(series is StackedColumn100Series)
                            && !(series is StackedArea100Series))
                        {
                            point = CurrentPoint;
                            double xStart = series.ActualXAxis.VisibleRange.Start;
                            double xEnd = series.ActualXAxis.VisibleRange.End;
                            point = new Point(series.ActualArea.ActualPointToValue(series.ActualXAxis, point), series.ActualArea.ActualPointToValue(series.ActualYAxis, point));
                            double range = Math.Round(point.X);
                            if (range <= xEnd && range >= xStart && range >= 0)
                            {
                                xVal = range;
                                if (series.DistinctValuesIndexes.Count > 0
                                    && series.DistinctValuesIndexes.ContainsKey(xVal))
                                {
                                    var value = series.DistinctValuesIndexes[xVal];
                                    for (int i = 0; i < value.Count; i++)
                                    {
                                        yValues.Add(series.GroupedSeriesYValues[0][value[i]]);
                                    }
                                }

                                if (isStackedSeries)
                                    yValues.Reverse();
                            }
                        }
                        else
                        {
                            series.FindNearestChartPoint(point, out xVal, out yVal, out stackedYValue);
                            if (series.IsIndexed || !(series.ActualXValues is IList<double>))
                                yValues.Add(yVal);
                            else
                            {
                                var yValuesList = GetYValuesBasedOnValue(xVal, series) as List<double>;
                                if (yValuesList != null)
                                    yValues = yValuesList;
                            }
                        }

                        x = this.Chart.ValueToLogPoint(series.ActualXAxis, xVal);
                        for (int k = 0; k < yValues.Count; k++)
                        {
                            double yValue = yValues[k];
                            if (isGrouping && !(series is StackedColumn100Series)
                            && !(series is StackedArea100Series))
                            {
                                if (series.ActualArea.VisibleSeries.IndexOf(series) == 0)
                                    tempValue = double.IsNaN(yValue) ? 0 : yValue;
                                else
                                {
                                    if (Values.Count > 0)
                                    {
                                        tempValue = yValue + Values[count];
                                        count++;
                                    }
                                    else
                                        tempValue = yValue;
                                }

                                Values.Add(tempValue);

                                {
                                    y = this.Chart.ValueToLogPoint(series.ActualYAxis, (isStackedSeries ? tempValue : yValue));
                                }
                            }
                            else
                            {
                                if (isStackedSeries && k != 0)
                                    stackedYValue += yValue;

                                {
                                    y = this.Chart.ValueToLogPoint(series.ActualYAxis, (isStackedSeries ? stackedYValue : yValue));
                                }
                            }

                            if (!double.IsNaN(x) && !double.IsNaN(y))
                            {
                                if ((!(((group.ElementAt(0) == series || group.ElementAt(group.Count() - 1) == series)) && series.PointsCount == 1)) || group.Count() == 1)
                                {
                                    if (index == 0)
                                        leastX = x;

                                    if (leastIndex == 0)
                                    {
                                        leastYPoint = y;
                                        leastXPoint = x;
                                        leastXVal = xVal;
                                    }
                                    
                                    if (Math.Abs(leastX - point.X) > Math.Abs(point.X - x))
                                    {
                                        leastX = x;
                                    }

                                    if (Math.Abs(leastXPoint - point.X) > Math.Abs(point.X - x))
                                    {
                                        leastXPoint = x;
                                        leastXVal = xVal;
                                    }

                                    if (Math.Abs(leastYPoint - point.Y) > Math.Abs(leastYPoint - y))
                                    {
                                        leastYPoint = y;
                                    }
                                }

                                Rect rect = new Rect(
                                    this.Chart.SeriesClipRect.Left - 1, 
                                    this.Chart.SeriesClipRect.Top - 1,
                                    this.Chart.SeriesClipRect.Width + 2, 
                                    this.Chart.SeriesClipRect.Height + 2);
                                if (IsReversed)
                                {
                                    if (!rect.Contains(
                                        new Point(
                                            leastYPoint + this.Chart.SeriesClipRect.Left,
                                            x + this.Chart.SeriesClipRect.Top)))
                                    {
                                        continue;
                                    }
                                }
                                else
                                {
                                    if (!rect.Contains(
                                        new Point(
                                            leastXPoint + this.Chart.SeriesClipRect.Left,
                                            y + this.Chart.SeriesClipRect.Top)))
                                    {
                                        continue;
                                    }
                                }

                                ChartPointInfo pointInfo = new ChartPointInfo();
                                pointInfo.X = x + this.Chart.SeriesClipRect.Left;
                                pointInfo.Y = y + this.Chart.SeriesClipRect.Top;
                                pointInfo.Series = series;

                                if (series.IsIndexed)
                                {
                                    pointInfo.ValueX = series.ActualXAxis.GetLabelContent((int)xVal).ToString();
                                }
                                else
                                {
                                    pointInfo.ValueX = series.ActualXAxis.GetLabelContent(xVal).ToString();
                                }

                                labelXValue = xVal.ToString();
                                pointInfo.SeriesValues.Add(xVal.ToString());
                                int xIndex = -1;
                                if (isGrouping && series.GroupedXValuesIndexes != null && series.GroupedXValuesIndexes.Count > 0
                                   && !(series is StackedColumn100Series)
                                   && !(series is StackedArea100Series)
                                   && series.DistinctValuesIndexes.ContainsKey(xVal))
                                {
                                    if (series.IsSideBySide)
                                    {
                                        xIndex = series.DistinctValuesIndexes[xVal][k];
                                        pointInfo.Item = series.GroupedActualData[xIndex];
                                    }
                                    else
                                    {
                                        xIndex = series.DistinctValuesIndexes[xVal][k];
                                    }
                                }
                                else
                                {
                                    xIndex = series.GetXValues().IndexOf(xVal);
                                    pointInfo.Item = series.ActualData[xIndex];
                                }

                                pointInfo.Interior = series.GetInteriorColor(xIndex);
                                pointInfo.ValueY = series.ActualYAxis.GetLabelContent(yValue).ToString();
                                labelYValue = yValue.ToString();

                                if (series is BubbleSeries && series.Segments[0] is BubbleSegment segment && !(IsReversed))
                                {
                                    pointInfo.SeriesValues.Add(yValue.ToString());
                                    pointInfo.SeriesValues.Add(segment.Size.ToString());
                                }
                                else if (IsReversed)
                                {
                                    pointInfo.SeriesValues.Add(yValue.ToString());
                                    pointInfo.X = this.Chart.SeriesClipRect.Height - (x + this.Chart.SeriesClipRect.Top);
                                    pointInfo.Y = y + this.Chart.SeriesClipRect.Left;
                                }
                                else
                                {
                                    pointInfo.SeriesValues.Add(yValue.ToString());
                                }

                                if (UseSeriesPalette)
                                {
                                    pointInfo.Foreground = new SolidColorBrush(Colors.White);
                                    pointInfo.BorderBrush = new SolidColorBrush(Colors.White);
                                }
                                else
                                {
#if NETFX_CORE
                                    pointInfo.Interior = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipFill"] as SolidColorBrush;
                                    pointInfo.Foreground = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipForeground"] as SolidColorBrush;
                                    pointInfo.BorderBrush = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipStroke"] as SolidColorBrush;
#endif
                                }

                                if (IsReversed)
                                {
                                    double tempX = pointInfo.X;
                                    pointInfo.BaseX = pointInfo.Y;
                                    pointInfo.BaseY = Chart.SeriesClipRect.Height - tempX;
                                    pointInfo.HorizontalAlignment = LabelVerticalAlignment == ChartAlignment.Auto ? ChartAlignment.Center : LabelVerticalAlignment;
                                    pointInfo.VerticalAlignment = LabelHorizontalAlignment == ChartAlignment.Auto ? ChartAlignment.Far : LabelHorizontalAlignment;
                                }
                                else
                                {
                                    pointInfo.BaseX = pointInfo.X;
                                    pointInfo.BaseY = pointInfo.Y;
                                    pointInfo.HorizontalAlignment = LabelHorizontalAlignment == ChartAlignment.Auto ? ChartAlignment.Far : LabelHorizontalAlignment;
                                    pointInfo.VerticalAlignment = LabelVerticalAlignment == ChartAlignment.Auto ? ChartAlignment.Center : LabelVerticalAlignment;
                                }

                                PointInfos.Add(pointInfo);
                                var seriesCollection = Chart.GetChartSeriesCollection();
                                if (seriesCollection.Count > 1 && seriesCollection.All(chartSeries => chartSeries.PointsCount == 1))
                                {
                                    leastXVal = xVal;
                                    leastX = leastXPoint = x;
                                }

                                if ((!(((group.ElementAt(0) == series || group.ElementAt(group.Count() - 1) == series)) && series.PointsCount == 1)) || group.Count() == 1)
                                {
                                    index++;
                                    leastIndex++;
                                }
                            }
                        }
                    }
                }

                if (seriesCount > 1)
                {
                    ObservableCollection<ChartPointInfo> chartPointInfos;

                    if (IsReversed)
                    {
                        chartPointInfos = new ObservableCollection<ChartPointInfo>((from info in PointInfos
                                                                                    where (info.X == this.Chart.SeriesClipRect.Height - (leastX + this.Chart.SeriesClipRect.Top))
                                                                                    select info));
                    }
                    else
                    {
                        chartPointInfos = new ObservableCollection<ChartPointInfo>((from info in PointInfos
                                                                                    where (Math.Abs((info.X - (leastX + this.Chart.SeriesClipRect.Left))) < 0.0001)
                                                                                    select info));
                    }

                    pointInfos = chartPointInfos;
                }

                if (PointInfos.Count == 0)
                    continue;
                ChartPointInfo ptInfo = new ChartPointInfo();
                ptInfo.Axis = axis;
                if (IsReversed)
                {
                    if (axis.IsVertical)
                    {
                        if (Chart.VisibleSeries.Count > 0 && Chart.VisibleSeries[0].IsIndexed)
                        {
                            ptInfo.ValueX = axis.GetLabelContent((int)leastXVal).ToString();
                        }
                        else
                        {
                            ptInfo.ValueX = axis.GetLabelContent(leastXVal).ToString();
                        }

                        if (DisplayMode == LabelDisplayMode.NearestPoint)
                        {
                            foreach (ChartPointInfo pointInfo in PointInfos)
                            {
                                if (pointInfo.Y == GetNearestYValue())
                                {
                                    ptInfo.X = pointInfo.X;
                                    ptInfo.ValueX = pointInfo.ValueX;
                                    currentYLabel = pointInfo.ValueY;
                                }
                            }
                        }
                        else
                            ptInfo.X = this.Chart.SeriesClipRect.Height - (leastXPoint + this.Chart.SeriesClipRect.Top);
                    }
                    else
                    {
                        ptInfo.ValueY = axis.GetLabelContent(leastXVal).ToString();
                        ptInfo.Y = point.Y;
                    }
                }
                else
                {
                    if (!axis.IsVertical)
                    {
                        if (Chart.VisibleSeries.Count > 0 && Chart.VisibleSeries[0].IsIndexed)
                        {
                            ptInfo.ValueX = axis.GetLabelContent((int)leastXVal).ToString();
                        }
                        else
                        {
                            ptInfo.ValueX = axis.GetLabelContent(leastXVal).ToString();
                        }

                        if (DisplayMode == LabelDisplayMode.NearestPoint)
                        {
                            foreach (ChartPointInfo pointInfo in PointInfos)
                            {
                                if (pointInfo.Y == GetNearestYValue())
                                {
                                    ptInfo.X = pointInfo.X;
                                    ptInfo.ValueX = pointInfo.ValueX;
                                    currentYLabel = pointInfo.ValueY;
                                }
                            }
                        }
                        else
                            ptInfo.X = leastXPoint + this.Chart.SeriesClipRect.Left;
                    }
                    else
                    {
                        ptInfo.ValueY = axis.GetLabelContent(leastXVal).ToString();
                        ptInfo.Y = point.Y;
                    }
                }

                currentXLabel = ptInfo.ValueX;
                ApplyDefaultBrushes(ptInfo);

                if (IsReversed)
                {
                    double tempX = ptInfo.X;
                    ptInfo.BaseX = ptInfo.Y;
                    ptInfo.BaseY = Chart.SeriesClipRect.Height - tempX;
                }
                else
                {
                    ptInfo.BaseX = ptInfo.X;
                    ptInfo.BaseY = ptInfo.Y;
                }

                isOpposedAxis = axis.OpposedPosition;
                axisLabels.Add(axis, ptInfo);
            }

            if (previousXLabel == currentXLabel && tempXPos == leastX)
            {
                if ((DisplayMode == LabelDisplayMode.NearestPoint && previousYLabel != currentYLabel && this.Chart.VisibleSeries.Count > 1))
                    previousYLabel = currentYLabel;
                else
                    return;
            }
            else
                previousXLabel = currentXLabel;

            tempXPos = leastX;
            ClearItems();
            GenerateAxisLabels();
            GenerateTrackballs();

            if (trackBalls.Count > 0)
                trackballWidth = trackBalls[0].Width;

            double xPos = leastX + this.Chart.SeriesClipRect.Left;

            if (IsReversed && pointInfos.Count > 0)
            {
                xPos = leastX + this.Chart.SeriesClipRect.Top;
                line.X1 = this.Chart.SeriesClipRect.Left;
                line.X2 = this.Chart.SeriesClipRect.Left + this.Chart.SeriesClipRect.Width;
                line.Y2 = line.Y1 = xPos;
            }
            else if (pointInfos.Count > 0)
            {
                line.Y1 = this.Chart.SeriesClipRect.Top;
                line.Y2 = this.Chart.SeriesClipRect.Top + this.Chart.SeriesClipRect.Height;
                line.X1 = line.X2 = xPos;
            }

            GenerateLabels();
            elements.Add(line);

            if (labelElements != null && labelElements.Count > 1 && labelElements[0].Content is ChartPointInfo)
            {
                if (!IsReversed)
                    labelElements = new List<ContentControl>(labelElements.OrderByDescending(x => (x.Content as ChartPointInfo).Y));
                else
                    labelElements = new List<ContentControl>(labelElements.OrderBy(x => (x.Content as ChartPointInfo).X));
                SmartAlignLabels();
                RenderSeriesBeakForSmartAlignment();
            }

            SetPositionChangedEventArgs();
        }


        internal override void DetachElements()
        {
            if (this.AdorningCanvas != null)
            {
                foreach (var element in elements)
                {
                    AdorningCanvas.Children.Remove(element);
                }
            }
        }

        internal override void OnSizeChanged(SizeChangedEventArgs e)
        {
            if(Chart == null)
            {
                return;
            }

            double y1 = this.Chart.ValueToLogPoint(this.Chart.InternalSecondaryAxis, (Convert.ToDouble(labelYValue)));
            double x1 = this.Chart.ValueToLogPoint(this.Chart.InternalPrimaryAxis, (Convert.ToDouble(labelXValue)));
            if (!double.IsNaN(y1) && !double.IsNaN(x1))
            {
                foreach (ContentControl control in labelElements)
                {
                    if (this.AdorningCanvas.Children.Contains(control))
                        this.AdorningCanvas.Children.Remove(control);
                }

                foreach (ContentControl control in axisLabelElements)
                {
                    if (this.AdorningCanvas.Children.Contains(control))
                        this.AdorningCanvas.Children.Remove(control);
                }

                foreach (Control control in trackBalls)
                {
                    if (this.AdorningCanvas.Children.Contains(control))
                        this.AdorningCanvas.Children.Remove(control);
                }

                PointInfos.Clear();

                labelElements.Clear();

                axisLabelElements.Clear();

                trackBalls.Clear();

                axisLabels.Clear();

                elements.Clear();

                CurrentPoint = new Point(x1, y1);
                OnPointerPositionChanged();
            }
        }

        /// <inheritdoc />
        protected internal override void OnHolding(HoldingRoutedEventArgs e)
        {
            if(Chart == null)
            {
                return;
            }

            IsActivated = true;
            if (e.PointerDeviceType == PointerDeviceType.Touch)
                Chart.HoldUpdate = true;

            if (this.Chart != null && IsActivated)
            {
                Point point = e.GetPosition(this.AdorningCanvas);

                if (Chart.SeriesClipRect.Contains(point))
                {
                    point = new Point(
                        point.X - Chart.SeriesClipRect.Left,
                        point.Y - Chart.SeriesClipRect.Top);

                    CurrentPoint = point;
                    OnPointerPositionChanged();
                }
            }
        }


        /// <inheritdoc />
        protected internal override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                IsActivated = false;
            else
                fingerCount++;
        }
        
        internal override void OnLayoutUpdated()
        {
            if (IsActivated)
                ScheduleTrackBallUpdate();
        }

        /// <inheritdoc />
        protected internal sealed override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(this.AdorningCanvas);
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
                if (fingerCount > 1) return;
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse &&
                !pointer.Properties.IsLeftButtonPressed)
                IsActivated = true;

            if (this.Chart != null && this.Chart.AreaType == ChartAreaType.CartesianAxes && IsActivated)
            {
                Point point = new Point(pointer.Position.X, pointer.Position.Y);

                if (Chart.SeriesClipRect.Contains(point))
                {
                    point = new Point(
                        point.X - Chart.SeriesClipRect.Left,
                        point.Y - Chart.SeriesClipRect.Top);

                    if (CurrentPoint.X != point.X || CurrentPoint.Y != point.Y)
                    {
                        CurrentPoint = point;
                        ScheduleTrackBallUpdate();
                    }
                }
            }
        }

        /// <inheritdoc />
        protected internal override void OnPointerExited(PointerRoutedEventArgs e)
        {
            if (Chart == null)
            {
                return;
            }

            if (IsActivated)
            {
                IsActivated = false; // Trackball behavior when moved out of Chart area-WRT-3895
                Chart.HoldUpdate = false;
#if NETFX_CORE
                fingerCount--;
#endif
            }
        }

        /// <inheritdoc />
        protected internal override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            if(Chart == null)
            {
                return;
            }

            if (e.Pointer.PointerDeviceType != PointerDeviceType.Mouse)
            {
                if (IsActivated)
                {
                    IsActivated = false;
                    Chart.HoldUpdate = false;

                }

                fingerCount--;
            }
        }

        internal override void AlignDefaultLabel(
            ChartAlignment verticalAlignemnt, 
            ChartAlignment horizontalAlignment,
            double x, 
            double y, 
            ContentControl control)
        {
            if (control != null && !double.IsInfinity(x) && (control as ContentControl).Content is ChartPointInfo pointInfo
                && !double.IsInfinity(y) && !double.IsNaN(x) && !double.IsNaN(y))
            {
                if (horizontalAlignment == ChartAlignment.Far && control is ContentControl)
                {
                    if (pointInfo.Series != null)
                    {
                        x = x + (trackballWidth * 0.75 + seriesTipHeight - 2);
                    }

                    pointInfo.X = x;
                }

                if (horizontalAlignment == ChartAlignment.Near)
                {
                    x = x - control.DesiredSize.Width;
                    if (control is ContentControl)
                    {
                        if (pointInfo.Series != null)
                        {
                            x = x - (trackballWidth * 0.75 + seriesTipHeight - 2);
                        }

                        pointInfo.X = x;
                    }
                }
                else if (horizontalAlignment == ChartAlignment.Center)
                {
                    x = x - control.DesiredSize.Width / 2;
                    if (control is ContentControl)
                        pointInfo.X = x;
                }

                if (verticalAlignemnt == ChartAlignment.Far && control is ContentControl)
                {
                    if (pointInfo.Series != null)
                    {
                        y = y + (trackballWidth * 0.75 + seriesTipHeight);
                    }

                    pointInfo.Y = y;
                }

                if (verticalAlignemnt == ChartAlignment.Near)
                {
                    y = y - control.DesiredSize.Height;
                    if (control is ContentControl)
                    {
                        if (pointInfo.Series != null)
                        {
                            y = y - (trackballWidth * 0.75 + seriesTipHeight);
                        }

                        pointInfo.Y = y;
                    }
                }
                else if (verticalAlignemnt == ChartAlignment.Center)
                {
                    y = y - control.DesiredSize.Height / 2;
                    if (control is ContentControl)
                        pointInfo.Y = y;
                }

                Canvas.SetLeft(control, x);
                Canvas.SetTop(control, y);
            }
        }

        #endregion

        #region Protected Override Methods

        internal override void AttachElements()
        {
            Binding binding = new Binding();
            binding.Path = new PropertyPath("LineStyle");
            binding.Source = this;
            line.SetBinding(Line.StyleProperty, binding);

            if (this.AdorningCanvas != null && !AdorningCanvas.Children.Contains(line) && this.ShowLine)
            {
                AdorningCanvas.Children.Add(line);
                elements.Add(line);
            }
        }

        #endregion

        #region Protected Virtual Methods
        
        private void GenerateLabels()
        {
            if (PointInfos.Count == 0)
                return;

            if (DisplayMode == LabelDisplayMode.GroupAllPoints)
            {
                ////XAMARIN-35059_Reverse the pointInfo for grouped label
                RearrangeStackingSeriesInfo();
                AddGroupLabels();
                return;
            }

            foreach (ChartPointInfo pointInfo in PointInfos)
            {
                bool canAddLabel = pointInfo.Series.IsActualTransposed ? Chart.SeriesClipRect.Contains(new Point(pointInfo.Y, pointInfo.X)) : Chart.SeriesClipRect.Contains(new Point(pointInfo.X, pointInfo.Y));
                if (canAddLabel)
                {
                    if (DisplayMode == LabelDisplayMode.FloatAllPoints)
                    {
                        if (seriesCount > 1 && PointInfos.Any(info => info.Series.IsSideBySide))
                        {
                            AddGroupLabels();
                            break;
                        }
                        else
                            AddLabel(
                                pointInfo, 
                                pointInfo.VerticalAlignment,
                                pointInfo.HorizontalAlignment,
                                GetLabelTemplate(pointInfo));
                    }
                    else if (DisplayMode == LabelDisplayMode.NearestPoint)
                    {
                        if (pointInfo.Y == GetNearestYValue())
                        {
                            if (IsReversed)
                                line.Y1 = line.Y2 = Chart.SeriesClipRect.Height - pointInfo.X;
                            else
                                line.X1 = line.X2 = pointInfo.X;
                            AddLabel(
                                pointInfo, 
                                pointInfo.VerticalAlignment, 
                                pointInfo.HorizontalAlignment,
                                GetLabelTemplate(pointInfo));
                            return;
                        }
                    }
                }
            }
        }

        private void RearrangeStackingSeriesInfo()
        {
            if (pointInfos.Count == 0)
            {
                return;
            }

            bool isContainStackedSeries = false;
            var stackingPointInfo = new ObservableCollection<ChartPointInfo>();
            foreach (var pointInfo in PointInfos)
            {
                if (pointInfo.Series is StackedSeriesBase && pointInfo.Series.ActualYAxis != null && !pointInfo.Series.ActualYAxis.IsInversed)
                {
                    stackingPointInfo.Insert(0, pointInfo);
                    isContainStackedSeries = true;
                }
                else
                {
                    stackingPointInfo.Insert(stackingPointInfo.Count == 0 ? 0 : stackingPointInfo.Count, pointInfo);
                }
            }

            if (isContainStackedSeries)
            {
                PointInfos = stackingPointInfo;
            }
        }

        private void AddGroupLabels()
        {
            foreach (ChartPointInfo pointInfo in PointInfos)
            {
                ContentControl control = new ContentControl();
                control.Content = pointInfo;
                pointInfo.Foreground = UseSeriesPalette ? pointInfo.Interior : new SolidColorBrush(Colors.Black);

                if ((pointInfo.Series.GetTrackballTemplate() != null && UseSeriesPalette && pointInfo.Series.Tag != null &&
                     pointInfo.Series.Tag.Equals("FromTheme")) || pointInfo.Series.GetTrackballTemplate() == null)
                {
                    control.ContentTemplate = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTrackballGroupLabelTemplate"] as DataTemplate;
                }
                else
                {
                    control.ContentTemplate = pointInfo.Series.GetTrackballTemplate();
                }

                groupLabelElements.Add(control);
            }

            var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
            stackPanel.Margin = new Thickness().GetThickness(3, 0, 3, 0);

            foreach (var label in groupLabelElements)
            {
                stackPanel.Children.Add(label);
                if (groupLabelElements.Count > 1 && label != groupLabelElements.Last())
                {
                    Rectangle separator = new Rectangle();
                    separator.Fill = new SolidColorBrush(Colors.Gray);
                    separator.StrokeThickness = 1;
                    separator.Height = 0.5;
                    label.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    separator.Width = label.Width;
                    stackPanel.Children.Add(separator);
                }
            }

            border = new Border();
            border.BorderBrush = new SolidColorBrush(Colors.Black);
            border.BorderThickness = new Thickness().GetThickness(1, 1, 1, 1);
            border.Background = LabelBackground;
#if !WinUI_UWP
            border.CornerRadius = new CornerRadius(1);
#else
            border.CornerRadius = CornerRadiusHelper.FromUniformRadius(1);
#endif
            border.Child = stackPanel;
            AddElement(border);
            ArrangeGroupLabel();
        }

        private void AddLabel(
            object obj,
            ChartAlignment verticalAlignment, 
            ChartAlignment horizontalAlignment,
            DataTemplate labelTemplate, 
            double x, 
            double y)
        {
            if (labelTemplate == null)
                return;
            ContentControl control = new ContentControl();
            control.Content = obj;
            control.IsHitTestVisible = false;
            control.ContentTemplate = labelTemplate;
            AddElement(control);
            control.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var pointInfo = control.Content as ChartPointInfo;

            if (CanApplyDefaultTemplate(obj))
            {
                if (pointInfo == null)
                    return;

                // Align Series labels.
                if (pointInfo.Series != null)
                {
                    // The labels get's aligned for the smart labels as well as for the boundry conditions.
                    labelElements.Add(control);

                    // Please note inversed case for the axis labels are already done.         
                    AlignDefaultLabel(verticalAlignment, horizontalAlignment, x, y, control);
                    ArrangeLabelsOnBounds(control, true);
                    AlignSeriesToolTipPolygon(control);
                }
                else if (pointInfo.Axis != null)
                {
                    // Align Axis labels.
                    axisLabelElements.Add(control);
                    if (IsReversed)
                        AlignDefaultLabel(horizontalAlignment, verticalAlignment, y, Chart.SeriesClipRect.Height - x, control);
                    else
                        AlignDefaultLabel(verticalAlignment, horizontalAlignment, x, y, control);
                    AlignAxisToolTipPolygon(control, verticalAlignment, horizontalAlignment, x, y, this);
                }
            }
            else
            {
                // Check for the case of datatemplate labels.

                if (pointInfo == null || (pointInfo != null && pointInfo.Series == null && pointInfo.Axis == null))
                {
                    // Check for the case of custom labels.
                    labelElements.Add(control);
                    if (IsReversed)
                        AlignElement(control, horizontalAlignment, verticalAlignment, y, Chart.SeriesClipRect.Height - x);
                    else
                        AlignElement(control, verticalAlignment, horizontalAlignment, x, y);
                }
                else
                {
                    if (pointInfo != null)
                    {
                        // Align Series labels.
                        if (pointInfo.Series != null)
                        {
                            labelElements.Add(control);
                            AlignElement(control, verticalAlignment, horizontalAlignment, x, y);
                            ArrangeLabelsOnBounds(control, false);
                            AlignSeriesToolTipPolygon(control);
                        }
                        else if (pointInfo.Axis != null)
                        {
                            // Align axis labels.
                            axisLabelElements.Add(control);
                            if (IsReversed)
                                AlignElement(control, horizontalAlignment, verticalAlignment, y, Chart.SeriesClipRect.Height - x);
                            else
                                AlignElement(control, verticalAlignment, horizontalAlignment, x, y);
                            AlignAxisToolTipPolygon(control, verticalAlignment, horizontalAlignment, x, y, this);
                        }
                    }
                }
            }
#if WinUI && !WinUI_Desktop
	// PolygonPoints does not bind with updated values.So, Reapplied the ContentTemplate,Content of ContentControl.
            control.ContentTemplate = null;
            control.Content = null;
            control.Content = obj;
            control.ContentTemplate = labelTemplate;
            control.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
           
#endif

        }

        private void GenerateTrackballs()
        {
            foreach (ChartPointInfo pointInfo in PointInfos)
            {
                bool canAddTrackball = pointInfo.Series.IsActualTransposed ? Chart.SeriesClipRect.Contains(new Point(pointInfo.Y, pointInfo.X)) : Chart.SeriesClipRect.Contains(new Point(pointInfo.X, pointInfo.Y));
                if (canAddTrackball)
                {
                    if (DisplayMode == LabelDisplayMode.FloatAllPoints || DisplayMode == LabelDisplayMode.GroupAllPoints)
                    {
                        CallTrackball(pointInfo);
                    }
                    else if (DisplayMode == LabelDisplayMode.NearestPoint)
                    {
                        if (pointInfo.Y == GetNearestYValue())
                        {
                            CallTrackball(pointInfo);
                            return;
                        }
                    }
                }
            }
        }

        internal void CallTrackball(ChartPointInfo pointInfo)
        {
            AddTrackball(pointInfo);
        }

        private void AddTrackball(ChartPointInfo pointInfo)
        {
            ChartTrackballControl control = new ChartTrackballControl(pointInfo.Series);
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("ChartTrackballStyle");
            control.SetBinding(ChartTrackballControl.StyleProperty, binding);

            trackBalls.Add(control);
            AddElement(control);
            control.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            if (IsReversed)
            {
                AlignElement(control, ChartAlignment.Center, ChartAlignment.Center, pointInfo.Y, Chart.SeriesClipRect.Height - pointInfo.X);
            }
            else
            {
                AlignElement(control, ChartAlignment.Center, ChartAlignment.Center, pointInfo.X, pointInfo.Y);
            }
        }

#endregion

#region Protected Methods
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        private IList<double> GetYValuesBasedOnValue(double x, ChartSeries series)
        {
            List<double> Values = new List<double>();

            if (series.ActualXValues is List<double> actualXValues)
            {
                List<double> xValues = actualXValues;

                for (int i = 0; i < series.PointsCount; i++)
                {
                    if (xValues[i] == x && series.ActualSeriesYValues != null)
                    {
                        foreach (var item in series.ActualSeriesYValues)
                        {
                            Values.Add(item[i]);
                        }
                    }
                }
            }

            return Values;
        }
        
        private void ClearItems()
        {
            foreach (ContentControl control in labelElements)
            {
                if (this.AdorningCanvas.Children.Contains(control))
                    this.AdorningCanvas.Children.Remove(control);
            }

            foreach (ContentControl control in axisLabelElements)
            {
                if (this.AdorningCanvas.Children.Contains(control))
                    this.AdorningCanvas.Children.Remove(control);
            }

            foreach (Control control in trackBalls)
            {
                if (this.AdorningCanvas.Children.Contains(control))
                    this.AdorningCanvas.Children.Remove(control);
            }

            foreach (ContentControl control in groupLabelElements)
            {
                if (this.AdorningCanvas.Children.Contains(control))
                    this.AdorningCanvas.Children.Remove(control);
            }

            if (AdorningCanvas != null && this.AdorningCanvas.Children.Contains(border))
                this.AdorningCanvas.Children.Remove(border);
            groupLabelElements.Clear();

            labelElements.Clear();

            axisLabelElements.Clear();

            trackBalls.Clear();

            line.ClearUIValues();

            elements.Clear();
        }

        private void AddLabel(ChartPointInfo obj, ChartAlignment verticalAlignment, ChartAlignment horizontalAlignment, DataTemplate template)
        {
            if (obj != null && template != null)
            {
                if (obj.Series == null)
                    AddLabel(obj, verticalAlignment, horizontalAlignment, template, obj.X, obj.Y);
                else
                    AddLabel(obj, verticalAlignment, horizontalAlignment, template, obj.BaseX, obj.BaseY);
            }
        }

        private void AddElement(UIElement element)
        {
            if (!this.AdorningCanvas.Children.Contains(element) && element is FrameworkElement frameworkElement)
            {
                this.AdorningCanvas.Children.Add(element);
                elements.Add(frameworkElement);
            }
        }

        #endregion

        #region Private Static Methods

        private static void OnShowLinePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartTrackballBehavior behavior)
            {
                behavior.OnShowLinePropertyChanged(e);
            }
        }

        private static void OnLayoutUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartTrackballBehavior behavior)
            {
                behavior.OnLayoutUpdated();
            }
        }

        private static ChartAlignment GetChartAlignment(bool isOpposed, ChartAlignment alignment)
        {
            if (isOpposed)
            {
                if (alignment == ChartAlignment.Near)
                    return ChartAlignment.Far;
                else if (alignment == ChartAlignment.Far)
                    return ChartAlignment.Near;
                else
                    return ChartAlignment.Center;
            }
            else
                return alignment;
        }
        
        private static bool CheckLabelCollision(ContentControl previousLabel, ContentControl currentLabel)
        {
            Rect rect1 = GetRenderedRect(previousLabel);
            Rect rect2 = GetRenderedRect(currentLabel);

            return CheckLabelCollisionRect(rect1, rect2);
        }

        private static bool CheckLabelCollisionRect(Rect rect1, Rect rect2)
        {
            return !(Math.Round((rect1.Y + rect1.Height), 2) <= Math.Round((rect2.Y), 2) ||
                                Math.Round(rect1.Y, 2) >= Math.Round(rect2.Y + rect2.Height, 2) ||
                                Math.Round(rect1.X + rect1.Width, 2) <= Math.Round(rect2.X, 2) ||
                                Math.Round(rect1.X, 2) >= (Math.Round(rect2.X + rect2.Width)));
        }

        private static Rect GetRenderedRect(ContentControl control)
        {
            ChartPointInfo pointInfo = control.Content as ChartPointInfo;
            return new Rect(pointInfo.X, pointInfo.Y, control.DesiredSize.Width, control.DesiredSize.Height);
        }

        private static bool CanApplyDefaultTemplate(object obj)
        {
            var chartPointInfo = obj as ChartPointInfo;
            if (chartPointInfo != null
                &&
                (chartPointInfo.Series != null &&
                 ((chartPointInfo.Series.GetTrackballTemplate() != null &&
                  chartPointInfo.Series.Tag != null && chartPointInfo.Series.Tag.Equals("FromTheme")) ||
                 chartPointInfo.Series.GetTrackballTemplate() == null)))
                return true;
            else if (chartPointInfo != null
                     &&
                     (chartPointInfo.Axis != null &&
                      ((chartPointInfo.Axis.GetTrackBallTemplate() != null &&
                       chartPointInfo.Axis.Tag != null && chartPointInfo.Axis.Tag.Equals("FromTheme")) ||
                      chartPointInfo.Axis.GetTrackBallTemplate() == null)))
                return true;
            return false;
        }

        private static void ApplyDefaultBrushes(object obj)
        {
            if(obj is ChartPointInfo chartPointInfo)
            {
#if NETFX_CORE
                chartPointInfo.Interior = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipFill"] as SolidColorBrush;
                chartPointInfo.Foreground = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipForeground"] as SolidColorBrush;
                chartPointInfo.BorderBrush = ChartDictionaries.GenericCommonDictionary["SyncfusionChartTooltipStroke"] as SolidColorBrush;
#endif
            }
        }

        private static void AlignElement(
            Control control,
            ChartAlignment verticalAlignemnt,
            ChartAlignment horizontalAlignment,
           double x,
           double y)
        {
            if (control != null && !double.IsInfinity(x)
                && !double.IsInfinity(y) && !double.IsNaN(x) && !double.IsNaN(y))
            {
                if (horizontalAlignment == ChartAlignment.Near)
                {
                    x = x - control.DesiredSize.Width;
                }
                else if (horizontalAlignment == ChartAlignment.Center)
                {
                    x = x - control.DesiredSize.Width / 2;
                }

                if (verticalAlignemnt == ChartAlignment.Near)
                {
                    y = y - control.DesiredSize.Height;
                }
                else if (verticalAlignemnt == ChartAlignment.Center)
                {
                    y = y - control.DesiredSize.Height / 2;
                }

                var contentControl = control as ContentControl;
                if (contentControl != null)
                {
                    var chartPointInfo = contentControl.Content as ChartPointInfo;
                    if (chartPointInfo != null)
                    {
                        chartPointInfo.X = x;
                        chartPointInfo.Y = y;
                    }
                }

                Canvas.SetLeft(control, x);
                Canvas.SetTop(control, y);
            }
        }

#endregion

#region Private Methods
        
        private void OnShowLinePropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                if ((bool)e.NewValue)
                    AttachElements();
                else if (this.AdorningCanvas != null)
                {
                    DetachElement(line);
                    elements.Remove(line);
                }
            }
        }

        private void SetPositionChangedEventArgs()
        {
            PositionChangedEventArgs e = new PositionChangedEventArgs();
            e.CurrentPointInfos = PointInfos;
            e.PreviousPointInfos = previousPointInfos;
            if (PositionChanged != null)
                PositionChanged(this, e);
        }

        private void SetPositionChangingEventArgs()
        {
            PositionChangingEventArgs e = new PositionChangingEventArgs();
            e.PointInfos = PointInfos;
            if (PositionChanging != null)
                PositionChanging(this, e);
            isCancel = e.Cancel;
        }

        private void RenderSeriesBeakForSmartAlignment()
        {
            foreach (var label in labelElements)
            {
                AlignSeriesToolTipPolygon(label as ContentControl);
            }
        }

        private void ArrangeLabelsOnBounds(ContentControl label, bool withPolygon)
        {
            bool leftCollision = false;
            bool rightCollision = false;

            if(label.Content is ChartPointInfo pointInfo)
            {

                // Calculate the shift width for the tips.
                double xFactor = (trackballWidth * 0.75 + seriesTipHeight - 2) * 2;
                double yFactor = (trackballWidth * 0.75 + seriesTipHeight) * 2;

                double left = pointInfo.X;
                double top = pointInfo.Y;

                if (Chart.SeriesClipRect.Left > pointInfo.X)
                    leftCollision = true;
                if (Chart.SeriesClipRect.Right < pointInfo.X + label.DesiredSize.Width)
                    rightCollision = true;

                // Left Collision
                if (Chart.SeriesClipRect.Left > left)
                {
                    if (pointInfo.HorizontalAlignment == ChartAlignment.Center)
                        pointInfo.X += withPolygon ? (label.DesiredSize.Width + xFactor) / 2 : label.DesiredSize.Width / 2;
                    else
                        pointInfo.X += withPolygon ? label.DesiredSize.Width + xFactor : label.DesiredSize.Width;
                    left = pointInfo.X;
                    pointInfo.HorizontalAlignment = ChartAlignment.Far;
                }

                // Top Collision
                if (Chart.SeriesClipRect.Top > top)
                {
                    if (pointInfo.VerticalAlignment == ChartAlignment.Center)
                        pointInfo.Y += withPolygon ? (label.DesiredSize.Height + yFactor) / 2 : label.DesiredSize.Height / 2;
                    else
                        pointInfo.Y += withPolygon ? label.DesiredSize.Height + yFactor : label.DesiredSize.Height;
                    top = pointInfo.Y;
                    pointInfo.VerticalAlignment = ChartAlignment.Far;

                    if (pointInfo.HorizontalAlignment == ChartAlignment.Center && !leftCollision)
                    {
                        if (!rightCollision)
                        {
                            pointInfo.X = withPolygon ? pointInfo.X + ((label.DesiredSize.Width + xFactor) / 2) : pointInfo.X + label.DesiredSize.Width / 2;
                            pointInfo.HorizontalAlignment = ChartAlignment.Far;
                        }
                        else
                        {
                            pointInfo.X = withPolygon ? pointInfo.X - ((label.DesiredSize.Width + xFactor) / 2) : pointInfo.X - label.DesiredSize.Width / 2;
                            pointInfo.HorizontalAlignment = ChartAlignment.Near;
                        }

                        left = pointInfo.X;
                    }
                }

                // Right Collision
                if (Chart.SeriesClipRect.Right < left + label.DesiredSize.Width)
                {
                    if (pointInfo.HorizontalAlignment == ChartAlignment.Center)
                        pointInfo.X = withPolygon ? pointInfo.X - (label.DesiredSize.Width + xFactor) / 2 : pointInfo.X - label.DesiredSize.Width / 2;
                    else
                        pointInfo.X = withPolygon ? pointInfo.X - label.DesiredSize.Width - xFactor : pointInfo.X - label.DesiredSize.Width;

                    left = pointInfo.X;
                    pointInfo.HorizontalAlignment = ChartAlignment.Near;
                }

                // Bottom Collision
                if (Chart.SeriesClipRect.Bottom < top + label.DesiredSize.Height)
                {
                    if (pointInfo.VerticalAlignment == ChartAlignment.Center)
                        pointInfo.Y = withPolygon ? pointInfo.Y - (label.DesiredSize.Height + yFactor) / 2 : pointInfo.Y - label.DesiredSize.Height / 2;
                    else
                        pointInfo.Y = withPolygon ? pointInfo.Y - label.DesiredSize.Height - yFactor : pointInfo.Y - label.DesiredSize.Height;

                    top = pointInfo.Y;
                    pointInfo.VerticalAlignment = ChartAlignment.Near;
                }

                Canvas.SetLeft(label, left);
                Canvas.SetTop(label, top);
            }
        }

        private void GenerateAxisLabels()
        {
            foreach (KeyValuePair<ChartAxis, ChartPointInfo> keyVal in axisLabels)
            {
                ChartAxis axis = keyVal.Key;
                var trackBallLabelTemplate = ChartDictionaries.GenericCommonDictionary["SyncfusionChartAxisTrackballLabelTemplate"] as DataTemplate;
                var trackballLabelTemplate = axis.GetTrackBallTemplate() ?? trackBallLabelTemplate;
                ChartPointInfo pointInfo = keyVal.Value;

                if (axis.GetTrackballInfo() && trackballLabelTemplate != null)
                {
                    chartAxis = axis;
                    if (IsReversed)
                    {
                        if (axis.IsVertical)
                        {
                            if (axis.GetTrackBallTemplate() == null || (axis.Tag != null && axis.Tag.Equals("FromTheme")))
                            {
                                pointInfo.Y = axis.OpposedPosition ? axis.ArrangeRect.Left + axisTipHeight : axis.ArrangeRect.Right - axisTipHeight;
                                AddLabel(
                                    pointInfo,
                                    GetChartAlignment(axis.OpposedPosition, ChartAlignment.Near),
                                    AxisLabelAlignment,
                                    trackballLabelTemplate);
                            }
                            else
                            {
                                pointInfo.Y = axis.OpposedPosition ? axis.ArrangeRect.Right : axis.ArrangeRect.Left;
                                AddLabel(
                                    pointInfo, 
                                    GetChartAlignment(axis.OpposedPosition, ChartAlignment.Far),
                                    AxisLabelAlignment,
                                    trackballLabelTemplate);
                            }
                        }
                        else
                        {
                            // Need to revisit the codes since else condition is not hit
                            pointInfo.X = axis.OpposedPosition ? axis.ArrangeRect.Left : axis.ArrangeRect.Right;
                            AddLabel(
                                pointInfo, 
                                AxisLabelAlignment,
                                GetChartAlignment(axis.OpposedPosition, ChartAlignment.Near),
                                trackballLabelTemplate);
                        }
                    }
                    else
                    {
                        // Need to revisit the codes since this if condition is not hit.
                        if (axis.IsVertical)
                        {
                            pointInfo.X = axis.OpposedPosition ? axis.ArrangeRect.Left : axis.ArrangeRect.Right;
                            AddLabel(
                                pointInfo, 
                                AxisLabelAlignment,
                                GetChartAlignment(axis.OpposedPosition, ChartAlignment.Near),
                                trackballLabelTemplate);
                        }
                        else
                        {
                            if (axis.GetTrackBallTemplate() == null || (axis.Tag != null && axis.Tag.Equals("FromTheme")))
                            {
                                pointInfo.Y = axis.OpposedPosition ? axis.ArrangeRect.Bottom - axisTipHeight : axis.ArrangeRect.Top + axisTipHeight;
                                AddLabel(
                                    pointInfo, 
                                    GetChartAlignment(axis.OpposedPosition, ChartAlignment.Far),
                                    AxisLabelAlignment,
                                    trackballLabelTemplate);
                            }
                            else
                            {
                                pointInfo.Y = axis.OpposedPosition ? axis.ArrangeRect.Bottom : axis.ArrangeRect.Top;
                                AddLabel(
                                    pointInfo, 
                                    GetChartAlignment(axis.OpposedPosition, ChartAlignment.Far),
                                    AxisLabelAlignment,
                                    trackballLabelTemplate);
                            }
                        }
                    }
                }
            }
        }

        private double GetNearestYValue()
        {
            double preValue = Double.MaxValue;
            var yValues = (from info in PointInfos select info.Y);
            double y = 0d;
            foreach (double value in yValues)
            {
                var diff = IsReversed ? Math.Abs(value - (CurrentPoint.X + this.Chart.SeriesClipRect.Left)) :
                      Math.Abs(value - CurrentPoint.Y);
                if (diff < preValue)
                {
                    y = value;
                    preValue = diff;
                }
            }

            return y;
        }

        private DataTemplate GetLabelTemplate(ChartPointInfo pointInfo)
        {
            DataTemplate? trackballLabelTemplate = null;

            {
#if WinUI
                if (ChartDictionaries.GenericCommonDictionary["SyncfusionChartTrackballLabelTemplate"] is DataTemplate trackballTemplate)
                {
                    pointInfo.Series.ActualTrackballLabelTemplate = trackballTemplate;
                }
#else
                pointInfo.Series.ActualTrackballLabelTemplate = ChartDictionaries.GenericCommonDictionary["defaultTrackBallLabel"] as DataTemplate;
#endif
            }

            if (((pointInfo.Series.GetTrackballTemplate() != null && UseSeriesPalette && pointInfo.Series.Tag != null &&
                pointInfo.Series.Tag.Equals("FromTheme")) || pointInfo.Series.GetTrackballTemplate() == null) 
                && pointInfo.Series.ActualTrackballLabelTemplate!= null)
            {
                trackballLabelTemplate = pointInfo.Series.ActualTrackballLabelTemplate;
            }
            else
            {
                trackballLabelTemplate = pointInfo.Series.GetTrackballTemplate();
            }

            return trackballLabelTemplate;
        }

        private void ArrangeGroupLabel()
        {
            double yPos = 0;
            double xPos = 0;
            border.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            if (IsReversed)
            {
                if (pointInfos.Count == 1)
                    xPos = CalculateHorizontalAlignment(pointInfos[0]);
                else
                    xPos = this.Chart.SeriesClipRect.Left + (this.Chart.SeriesClipRect.Width / 2 - border.DesiredSize.Width / 2);
                yPos = CalculateVerticalAlignment(pointInfos[0]);
            }
            else
            {
                if (pointInfos.Count == 1)
                    yPos = CalculateVerticalAlignment(pointInfos[0]);
                else
                    yPos = this.Chart.SeriesClipRect.Top + (this.Chart.SeriesClipRect.Height / 2 - border.DesiredSize.Height / 2);
                xPos = CalculateHorizontalAlignment(pointInfos[0]);
            }
            if (DisplayMode == LabelDisplayMode.GroupAllPoints)
            {
                if (IsReversed)
                {
                    xPos = this.Chart.SeriesClipRect.Right - border.DesiredSize.Width;
                }
                else
                {
                    yPos = this.Chart.SeriesClipRect.Top;
                }
            }

            CheckCollision(xPos, yPos);
        }

        private double CalculateVerticalAlignment(ChartPointInfo chartPointInfo)
        {
            double yPos = 0;
            if (chartPointInfo.VerticalAlignment == ChartAlignment.Center)
                yPos = pointInfos[0].BaseY - border.DesiredSize.Height / 2;
            else if (chartPointInfo.VerticalAlignment == ChartAlignment.Near)
                yPos = pointInfos[0].BaseY - border.DesiredSize.Height - 5;
            else
                yPos = pointInfos[0].BaseY + 5;
            if (DisplayMode == LabelDisplayMode.GroupAllPoints)
            {
                yPos = pointInfos[0].BaseY - border.DesiredSize.Height / 2;
            }
            return yPos;
        }

        private double CalculateHorizontalAlignment(ChartPointInfo chartPointInfo)
        {
            double xPos = 0;
            if (chartPointInfo.HorizontalAlignment == ChartAlignment.Center)
                xPos = pointInfos[0].BaseX - border.DesiredSize.Width / 2;
            else if (chartPointInfo.HorizontalAlignment == ChartAlignment.Near)
                xPos = pointInfos[0].BaseX - border.DesiredSize.Width - 5;
            else
                xPos = pointInfos[0].BaseX + 5;
            if (DisplayMode == LabelDisplayMode.GroupAllPoints)
            {
                xPos = pointInfos[0].BaseX - border.DesiredSize.Width / 2;
            }
            return xPos;
        }

        private void CheckCollision(double xPos, double yPos)
        {
            double left = xPos;
            double top = yPos;

            // Left Collision
            if (Chart.SeriesClipRect.Left > xPos)
            {
                if (pointInfos[0].HorizontalAlignment == ChartAlignment.Center || pointInfos[0].HorizontalAlignment == ChartAlignment.Auto)
                    xPos += (border.DesiredSize.Width) / 2;
                else
                    xPos += border.DesiredSize.Width + 10;
                if (!IsReversed)
                {
                    if (DisplayMode == LabelDisplayMode.GroupAllPoints)
                    {
                        xPos = pointInfos[0].BaseX;
                    }
                }
                left = xPos;
            }

            // Bottom Collision
            if (Chart.SeriesClipRect.Bottom < (yPos + border.DesiredSize.Height))
            {
                if (pointInfos[0].VerticalAlignment == ChartAlignment.Center)
                    yPos = yPos - (border.DesiredSize.Height) / 2;
                else
                    yPos = yPos - border.DesiredSize.Height - 10;
                top = yPos;
            }

           // Top Collision
            if (Chart.SeriesClipRect.Top > yPos)
            {
                if (pointInfos[0].VerticalAlignment == ChartAlignment.Center)
                    yPos += (border.DesiredSize.Height) / 2;
                else
                    yPos += border.DesiredSize.Height + 10;
                top = yPos;
            }

            // Right Collision
            if (Chart.SeriesClipRect.Right < (xPos + border.DesiredSize.Width))
            {
                if (pointInfos[0].HorizontalAlignment == ChartAlignment.Center || pointInfos[0].HorizontalAlignment == ChartAlignment.Auto)
                    xPos = xPos - (border.DesiredSize.Width) / 2;
                else
                    xPos = xPos - border.DesiredSize.Width - 10;
                if (!IsReversed)
                {
                    if (DisplayMode == LabelDisplayMode.GroupAllPoints)
                    {
                        xPos = pointInfos[0].BaseX - border.DesiredSize.Width;
                    }
                }

                left = xPos;
            }
         
            Canvas.SetLeft(border, left);
            Canvas.SetTop(border, top);
        }
             
        /// <summary>
        /// To align the trackball labels smartly.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals", Justification = "Reviewed")]
        private void SmartAlignLabels()
        {
            List<List<Control>> intersectedGroups = new List<List<Control>>();
            List<Control> tempIntersectedLabels = new List<Control>();

            // Label Intersection logic
            tempIntersectedLabels.Add(labelElements[0]);
            for (int i = 0; i + 1 < labelElements.Count; i++)
            {
                if (CheckLabelCollision(labelElements[i], labelElements[i + 1]))
                {
                    tempIntersectedLabels.Add(labelElements[i + 1]);
                }
                else
                {
                    intersectedGroups.Add(new List<Control>(tempIntersectedLabels));
                    tempIntersectedLabels.Clear();
                    tempIntersectedLabels.Add(labelElements[i + 1]);
                }
            }

            // To add the last collided labels.
            if (tempIntersectedLabels.Count > 0)
            {
                intersectedGroups.Add(new List<Control>(tempIntersectedLabels));
                tempIntersectedLabels.Clear();
            }

            if (IsReversed)
            {
                foreach (var intersectGroupLabels in intersectedGroups)
                {
                    // Smart align the labels inside group.
                    if (intersectGroupLabels.Count > 1 && (intersectGroupLabels[0] is ContentControl ctrl && ctrl.Content is ChartPointInfo pointInfo))
                    {
                        ChartAlignment tempHorizontalAlign = pointInfo.HorizontalAlignment;
                        double tempXValue = pointInfo.X;

                        if (tempHorizontalAlign == ChartAlignment.Far)
                        {
                            for (int i = 0; i < intersectGroupLabels.Count; i++)
                            {
                                double width = intersectGroupLabels[i].DesiredSize.Width;
                                ChartPointInfo currentPointInfo = ((intersectGroupLabels[i] as ContentControl).Content as ChartPointInfo);

                                currentPointInfo.HorizontalAlignment = tempHorizontalAlign;
                                currentPointInfo.X = tempXValue + (width + trackLabelSpacing) * i;
                                Canvas.SetLeft(intersectGroupLabels[i], currentPointInfo.X);
                            }
                        }
                        else if (tempHorizontalAlign == ChartAlignment.Near)
                        {
                            double halfHeight = ((intersectGroupLabels[0].DesiredSize.Width * intersectGroupLabels.Count)
                                                 + trackLabelSpacing * (intersectGroupLabels.Count - 1))
                                                 - intersectGroupLabels[0].DesiredSize.Width;

                            for (int i = 0; i < intersectGroupLabels.Count; i++)
                            {
                                double width = intersectGroupLabels[i].DesiredSize.Width;
                                ChartPointInfo currentPointInfo = (intersectGroupLabels[i] as ContentControl).Content as ChartPointInfo;
                                currentPointInfo.HorizontalAlignment = tempHorizontalAlign;

                                currentPointInfo.X = tempXValue - halfHeight + i * (width + trackLabelSpacing);
                                Canvas.SetLeft(intersectGroupLabels[i], currentPointInfo.X);
                            }
                        }
                        else
                        {
                            double halfHeight = ((intersectGroupLabels[0].DesiredSize.Width * intersectGroupLabels.Count)
                                                  + trackLabelSpacing * (intersectGroupLabels.Count - 1)) / 2
                                                  - intersectGroupLabels[0].DesiredSize.Width / 2;

                            for (int i = 0; i < intersectGroupLabels.Count; i++)
                            {
                                double width = intersectGroupLabels[i].DesiredSize.Width;
                                ChartPointInfo currentPointInfo = (intersectGroupLabels[i] as ContentControl).Content as ChartPointInfo;
                                currentPointInfo.HorizontalAlignment = tempHorizontalAlign;

                                currentPointInfo.X = tempXValue - halfHeight + i * (width + trackLabelSpacing);
                                Canvas.SetLeft(intersectGroupLabels[i], currentPointInfo.X);
                            }
                        }
                    }
                }

                // Smart align the entire group.
                if (isOpposedAxis)
                {
                    // Arrange the lowest label according to the border.
                    ArrangeLowestLabelonBounds(intersectedGroups[intersectedGroups.Count - 1]);

                    if (intersectedGroups.Count > 1)
                        for (int i = intersectedGroups.Count - 1; i - 1 > -1; i--)
                        {
                            if (CheckGroupsCollision(intersectedGroups[i], intersectedGroups[i - 1]))
                            {
                                ChartPointInfo ptInfo = (intersectedGroups[i][0] as ContentControl).Content as ChartPointInfo;
                                double halfHeight = ptInfo.X - intersectedGroups[i - 1].Count * (intersectedGroups[i - 1][0].DesiredSize.Width + trackLabelSpacing);

                                for (int k = 0; k < intersectedGroups[i - 1].Count; k++)
                                {
                                    double width = intersectedGroups[i - 1][k].DesiredSize.Width;
                                    ChartPointInfo pointInfo = (intersectedGroups[i - 1][k] as ContentControl).Content as ChartPointInfo;

                                    pointInfo.X = halfHeight + k * (width + trackLabelSpacing);
                                    Canvas.SetLeft(intersectedGroups[i - 1][k], pointInfo.X);
                                }
                            }
                        }
                }
                else
                {
                    // Arrange the lowest label according to the border.
                    ArrangeLowestLabelonBounds(intersectedGroups[0]);

                    if (intersectedGroups.Count > 1)
                        for (int i = 0; i + 1 < intersectedGroups.Count; i++)
                        {
                            if (CheckGroupsCollision(intersectedGroups[i], intersectedGroups[i + 1]))
                            {
                                ChartPointInfo ptInfo = (intersectedGroups[i][intersectedGroups[i].Count - 1] as ContentControl).Content as ChartPointInfo;
                                double halfHeight = ptInfo.X + (intersectedGroups[i][0].DesiredSize.Width + trackLabelSpacing);

                                for (int k = 0; k < intersectedGroups[i + 1].Count; k++)
                                {
                                    double width = intersectedGroups[i + 1][k].DesiredSize.Width;
                                    ChartPointInfo pointInfo = (intersectedGroups[i + 1][k] as ContentControl).Content as ChartPointInfo;

                                    pointInfo.X = halfHeight + k * (width + trackLabelSpacing);
                                    Canvas.SetLeft(intersectedGroups[i + 1][k], pointInfo.X);
                                }
                            }
                        }
                }
            }
            else
            {
                foreach (var intersectGroupLabels in intersectedGroups)
                {
                    // Smart align the labels inside group.
                    if (intersectGroupLabels.Count > 1)
                    {
                        ChartPointInfo pointInfo = (intersectGroupLabels[0] as ContentControl).Content as ChartPointInfo;
                        ChartAlignment tempVerticalAlign = pointInfo.VerticalAlignment;
                        double tempYValue = pointInfo.Y;
                        if (tempVerticalAlign == ChartAlignment.Far)
                        {
                            for (int i = 0; i < intersectGroupLabels.Count; i++)
                            {
                                double height = intersectGroupLabels[i].DesiredSize.Height;
                                ChartPointInfo currentPointInfo = ((intersectGroupLabels[i] as ContentControl).Content as ChartPointInfo);
                                currentPointInfo.VerticalAlignment = tempVerticalAlign;
                                currentPointInfo.Y = tempYValue + ((height + trackLabelSpacing) * i);
                                Canvas.SetTop(intersectGroupLabels[i], currentPointInfo.Y);
                            }
                        }
                        else if (tempVerticalAlign == ChartAlignment.Near)
                        {
                            double halfHeight = ((intersectGroupLabels[0].DesiredSize.Height * intersectGroupLabels.Count)
                                                 + trackLabelSpacing * (intersectGroupLabels.Count - 1))
                                                 - intersectGroupLabels[0].DesiredSize.Height;

                            for (int i = 0; i < intersectGroupLabels.Count; i++)
                            {
                                double height = intersectGroupLabels[i].DesiredSize.Height;
                                ChartPointInfo currentPointInfo = (intersectGroupLabels[i] as ContentControl).Content as ChartPointInfo;
                                currentPointInfo.VerticalAlignment = tempVerticalAlign;
                                currentPointInfo.Y = tempYValue + halfHeight - i * (height + trackLabelSpacing);
                                Canvas.SetTop(intersectGroupLabels[i], currentPointInfo.Y);
                            }
                        }
                        else
                        {
                            double halfHeight = ((intersectGroupLabels[0].DesiredSize.Height * intersectGroupLabels.Count)
                                                  + trackLabelSpacing * (intersectGroupLabels.Count - 1)) / 2
                                                  - intersectGroupLabels[0].DesiredSize.Height / 2;

                            for (int i = 0; i < intersectGroupLabels.Count; i++)
                            {
                                double height = intersectGroupLabels[i].DesiredSize.Height;
                                ChartPointInfo currentPointInfo = (intersectGroupLabels[i] as ContentControl).Content as ChartPointInfo;
                                currentPointInfo.VerticalAlignment = tempVerticalAlign;
                                currentPointInfo.Y = tempYValue + halfHeight - i * (height + trackLabelSpacing);
                                Canvas.SetTop(intersectGroupLabels[i], currentPointInfo.Y);
                            }
                        }
                    }
                }

                // Smart align the entire group.
                if (isOpposedAxis)
                {
                    // Arrange the lowest label according to the border.
                    ArrangeLowestLabelonBounds(intersectedGroups[intersectedGroups.Count - 1]);

                    if (intersectedGroups.Count > 1)
                        for (int i = intersectedGroups.Count - 1; i - 1 > -1; i--)
                        {
                            if (CheckGroupsCollision(intersectedGroups[i], intersectedGroups[i - 1]))
                            {
                                ChartPointInfo ptInfo = (intersectedGroups[i][0] as ContentControl).Content as ChartPointInfo;
                                double halfHeight = ptInfo.Y + intersectedGroups[i - 1].Count * (intersectedGroups[i][0].DesiredSize.Height + trackLabelSpacing);

                                for (int k = 0; k < intersectedGroups[i - 1].Count; k++)
                                {
                                    double height = intersectedGroups[i - 1][k].DesiredSize.Height;
                                    ChartPointInfo pointInfo = (intersectedGroups[i - 1][k] as ContentControl).Content as ChartPointInfo;

                                    pointInfo.Y = halfHeight - k * (height + trackLabelSpacing);
                                    Canvas.SetTop(intersectedGroups[i - 1][k], pointInfo.Y);
                                }
                            }
                        }
                }
                else
                {
                    // Arrange the lowest label according to the border.
                    ArrangeLowestLabelonBounds(intersectedGroups[0]);

                    if (intersectedGroups.Count > 1)
                        for (int i = 0; i + 1 < intersectedGroups.Count; i++)
                        {
                            if (CheckGroupsCollision(intersectedGroups[i], intersectedGroups[i + 1]))
                            {
                                ChartPointInfo ptInfo = (intersectedGroups[i][intersectedGroups[i].Count - 1] as ContentControl).Content as ChartPointInfo;
                                double halfHeight = ptInfo.Y - (intersectedGroups[i + 1][0].DesiredSize.Height + trackLabelSpacing);

                                // Reverse Counter is used since the hight of the pixel is from top to bottom.
                                int reverseCounter = 0;
                                for (int k = 0; k < intersectedGroups[i + 1].Count; k++)
                                {
                                    double height = intersectedGroups[i + 1][k].DesiredSize.Height;
                                    ChartPointInfo pointInfo = (intersectedGroups[i + 1][k] as ContentControl).Content as ChartPointInfo;

                                    pointInfo.Y = halfHeight - k * (height + trackLabelSpacing);
                                    Canvas.SetTop(intersectedGroups[i + 1][k], pointInfo.Y);
                                    reverseCounter++;
                                }
                            }
                        }
                }
            }
        }

        private void ArrangeLowestLabelonBounds(List<Control> list)
        {
            if (IsReversed)
            {
                if (isOpposedAxis)
                {
                    ChartPointInfo ptInfo = ((list[list.Count - 1] as ContentControl).Content as ChartPointInfo);
                    double width = (list[list.Count - 1] as ContentControl).DesiredSize.Width;

                    double X = ptInfo.X + width;
                    if (X > Math.Round(Chart.SeriesClipRect.Right, 2))
                    {
                        double halfHeight = Chart.SeriesClipRect.Right - list.Count * (width + trackLabelSpacing);

                        for (int i = 0; i < list.Count; i++)
                        {
                            ChartPointInfo pointInfo = (list[i] as ContentControl).Content as ChartPointInfo;
                            pointInfo.X = halfHeight + i * (width + trackLabelSpacing);
                            Canvas.SetLeft(list[i], pointInfo.X);
                        }
                    }
                }
                else
                {
                    ChartPointInfo ptInfo = ((list[0] as ContentControl).Content as ChartPointInfo);

                    double width = (list[0] as ContentControl).DesiredSize.Width;
                    double X = ptInfo.X;
                    if (X < Math.Round(Chart.SeriesClipRect.Left, 2))
                    {
                        double halfHeight = Chart.SeriesClipRect.Left + trackLabelSpacing;

                        for (int i = 0; i < list.Count; i++)
                        {
                            ChartPointInfo pointInfo = (list[i] as ContentControl).Content as ChartPointInfo;
                            pointInfo.X = halfHeight + i * (width + trackLabelSpacing);
                            Canvas.SetLeft(list[i], pointInfo.X);
                        }
                    }
                }
            }
            else
            {
                if (isOpposedAxis)
                {
                    ChartPointInfo ptInfo = ((list[list.Count - 1] as ContentControl).Content as ChartPointInfo);
                    double height = (list[list.Count - 1] as ContentControl).DesiredSize.Height;

                    double Y = ptInfo.Y;
                    if (Y < Math.Round(Chart.SeriesClipRect.Top, 2))
                    {
                        double halfHeight = Chart.SeriesClipRect.Top + (list.Count - 1) * height + list.Count * trackLabelSpacing;

                        for (int i = 0; i < list.Count; i++)
                        {
                            ChartPointInfo pointInfo = (list[i] as ContentControl).Content as ChartPointInfo;
                            pointInfo.Y = halfHeight - i * (height + trackLabelSpacing);
                            Canvas.SetTop(list[i], pointInfo.Y);
                        }
                    }
                }
                else
                {
                    ChartPointInfo ptInfo = ((list[0] as ContentControl).Content as ChartPointInfo);

                    double height = (list[0] as ContentControl).DesiredSize.Height;
                    double Y = ptInfo.Y + height;
                    if (Y > Math.Round(Chart.SeriesClipRect.Bottom, 2))
                    {
                        double halfHeight = Chart.SeriesClipRect.Bottom - (height + trackLabelSpacing);

                        for (int i = 0; i < list.Count; i++)
                        {
                            ChartPointInfo pointInfo = (list[i] as ContentControl).Content as ChartPointInfo;
                            pointInfo.Y = halfHeight - i * (height + trackLabelSpacing);
                            Canvas.SetTop(list[i], pointInfo.Y);
                        }
                    }
                }
            }
        }

        private bool CheckGroupsCollision(List<Control> list1, List<Control> list2)
        {
            ChartPointInfo list1_PtInfoFirst = (list1[0] as ContentControl).Content as ChartPointInfo;
            ChartPointInfo list1_PtInfoLast = (list1[list1.Count - 1] as ContentControl).Content as ChartPointInfo;
            ChartPointInfo list2_PtInfoFirst = (list2[0] as ContentControl).Content as ChartPointInfo;
            ChartPointInfo list2_PtInfoLast = (list2[list2.Count - 1] as ContentControl).Content as ChartPointInfo;

            Point point1;
            Point point2;
            Rect rect1;
            Rect rect2;

            if (IsReversed)
            {
                point1 = new Point(list1_PtInfoFirst.X, list1_PtInfoFirst.Y);
                point2 = new Point(list1_PtInfoLast.X + list1[list1.Count - 1].DesiredSize.Width, list1_PtInfoLast.Y + list1[list1.Count - 1].DesiredSize.Height);
                rect1 = new Rect(point1, point2);

                point1 = new Point(list2_PtInfoFirst.X, list2_PtInfoFirst.Y);
                point2 = new Point(list2_PtInfoLast.X + list2[list2.Count - 1].DesiredSize.Width, list2_PtInfoLast.Y + list2[list2.Count - 1].DesiredSize.Height);
                rect2 = new Rect(point1, point2);
            }
            else
            {
                point1 = new Point(list1_PtInfoLast.X, list1_PtInfoLast.Y);
                point2 = new Point(list1_PtInfoFirst.X + list1[0].DesiredSize.Width, list1_PtInfoFirst.Y + list1[0].DesiredSize.Height);
                rect1 = new Rect(point1, point2);

                point1 = new Point(list2_PtInfoLast.X, list2_PtInfoLast.Y);
                point2 = new Point(list2_PtInfoFirst.X + list2[0].DesiredSize.Width, list2_PtInfoFirst.Y + list2[0].DesiredSize.Height);
                rect2 = new Rect(point1, point2);
            }

            return CheckLabelCollisionRect(rect1, rect2);
        }

        private void AlignSeriesToolTipPolygon(ContentControl control)
        {
            double labelHeight = control.DesiredSize.Height;
            double labelWidth = control.DesiredSize.Width;
            if(control.Content is ChartPointInfo label)
            {
                var baseX = label.BaseX;
                var baseY = label.BaseY;

                label.PolygonPoints = ChartUtils.GetTooltipPolygonPoints(new Rect(baseX - label.X, baseY - label.Y, labelWidth, labelHeight), seriesTipHeight, IsReversed, label.HorizontalAlignment, label.VerticalAlignment);
            }
        }

        internal void Activate(bool activate)
        {
            foreach (UIElement element in elements)
            {
                element.Visibility = activate ? Visibility.Visible : Visibility.Collapsed;
            }
        }

#endregion

#endregion

    }

    /// <summary>
    /// Sets the fill color for the track ball control.
    /// </summary>
    public class ChartTrackballColorConverter : IValueConverter
    {
#region Public Methods

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="System.Type"/> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is ChartTrackballControl trackBall)
            {
                if (trackBall.Background != null)
                    return trackBall.Background;
                if (trackBall.Series != null)
                {
                    return trackBall.Series.GetInteriorColor(1);
                }
            }

            return null;
        }

        /// <summary>
        ///  Modifies the target data before passing it to the source object. 
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="System.Type"/> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="language">The language of the conversion.</param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

#endregion
    }

    /// <summary>
    /// Defines the trackball control template.
    /// </summary>
    public class ChartTrackballControl : Control
    {
#region Dependency Property Registration
        
        /// <summary>
        /// The DependencyProperty for <see cref="Series"/> property.
        /// </summary>
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(
                "Series",
                typeof(ChartSeries),
                typeof(ChartTrackballControl),
                new PropertyMetadata(null));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                "Stroke",
                typeof(Brush),
                typeof(ChartTrackballControl),
                new PropertyMetadata(null));

        /// <summary>
        /// The DependencyProperty for <see cref="StrokeThickness"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                "StrokeThickness",
                typeof(double),
                typeof(ChartTrackballControl),
                new PropertyMetadata(1d));

#endregion

#region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="series"></param>
        public ChartTrackballControl(ChartSeries series)
        {
            this.Series = series;
            DefaultStyleKey = typeof(ChartTrackballControl);
        }

#endregion

#region Properties

#region Public Properties
        
        /// <summary>
        /// Gets or sets the series associated with the trackball.
        /// </summary>
        public ChartSeries Series
        {
            get { return (ChartSeries)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a brush to highlight the stroke of trakball.
        /// </summary>
        /// <value>It takes <see cref="Brush"/> value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior>
        ///                   <chart:ChartTrackballBehavior.ChartTrackballStyle>
        ///                       <Style TargetType="chart:ChartTrackballControl">
        ///                           <Setter Property = "Stroke" Value="Red"/>
        ///                       </Style>
        ///                   </chart:ChartTrackballBehavior.ChartTrackballStyle>
        ///               </chart:ChartTrackballBehavior>
        ///           </chart:SfCartesianChart.TrackballBehavior>
        ///
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///    // omitted for brevity
        ///    var trackballStyle = new Style() { TargetType = typeof(ChartTrackballControl) };
        ///    trackballStyle.Setters.Add(new Setter(ChartTrackballControl.StrokeProperty, new SolidColorBrush(Colors.Red)));
        ///	   chart.TrackballBehavior = new ChartTrackballBehavior()
        ///    {
        ///       ChartTrackballStyle = trackballStyle,
        ///	   };
        ///
        ///     LineSeries series = new LineSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value for stroke thickness.
        /// </summary>
        /// <value>This property takes a double value and its default value is 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///
        ///           <chart:SfCartesianChart.TrackballBehavior>
        ///               <chart:ChartTrackballBehavior>
        ///                   <chart:ChartTrackballBehavior.ChartTrackballStyle>
        ///                       <Style TargetType="chart:ChartTrackballControl">
        ///                           <Setter Property = "StrokeThickness" Value="2"/>
        ///                       </Style>
        ///                   </chart:ChartTrackballBehavior.ChartTrackballStyle>
        ///               </chart:ChartTrackballBehavior>
        ///           </chart:SfCartesianChart.TrackballBehavior>
        ///
        ///           <chart:LineSeries ItemsSource="{Binding Data}"
        ///                             XBindingPath="XValue"
        ///                             YBindingPath="YValue"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///    // omitted for brevity
        ///    var trackballStyle = new Style() { TargetType = typeof(ChartTrackballControl) };
        ///    trackballStyle.Setters.Add(new Setter(ChartTrackballControl.StrokeThicknessProperty, 2));
        ///	   chart.TrackballBehavior = new ChartTrackballBehavior()
        ///    {
        ///       ChartTrackballStyle = trackballStyle,
        ///	   };
        ///
        ///     LineSeries series = new LineSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

#endregion

#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class PositionChangingEventArgs : EventArgs
    {
#region Properties
    
#region Public Properties

             
        public ObservableCollection<ChartPointInfo> PointInfos
        {
            get;
            internal set;
        }

        public bool Cancel
        {
            get;
            set;
        }

#endregion

#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class PositionChangedEventArgs : EventArgs
    {
#region Properties

#region Public Properties

        public ObservableCollection<ChartPointInfo> PreviousPointInfos
        {
            get;
            internal set;
        }

        public ObservableCollection<ChartPointInfo> CurrentPointInfos
        {
            get;
            internal set;
        }

#endregion

#endregion

    }
}
