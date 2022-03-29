using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents the pyramid chart, which is the form of a triangle with lines dividing it into sections, each section with a different width. Depending on the Y coordinates, this width indicates a level of hierarchy among other categories. It supports data label, tooltip, and selection.
    /// </summary>
    /// <remarks>
    /// It is a single chart, representing data as portions of 100% and does not use any axes. 
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfPyramidChart ItemsSource="{Binding Data}" XBindingPath="XValue" 
    ///               YBindingPath="YValue">
    ///
    ///          <chart:SfPyramidChart.DataContext>
    ///            <local:ViewModel/>
    ///          </chart:SfPyramidChart.DataContext>
    ///
    ///     </chart:SfPyramidChart>
    /// ]]></code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfPyramidChart chart = new SfPyramidChart();
    ///     
    ///     ViewModel viewmodel = new ViewModel();
    ///
    ///     chart.ItemsSource = viewmodel.Data;
    ///     chart.XBindingPath = "XValue";
    ///     chart.YBindingPath = "YValue";
    /// ]]></code>
    /// ***
    /// </example>
    public class SfPyramidChart : ChartBase
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <c>YBindingPath</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>YBindingPath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty YBindingPathProperty =
            DependencyProperty.Register(nameof(YBindingPath), typeof(string), typeof(SfPyramidChart),
            new PropertyMetadata(null, OnYPathChanged));

        /// <summary>
        /// Identifies the <c>ExplodeIndex</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ExplodeIndex</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ExplodeIndexProperty =
            DependencyProperty.Register(nameof(ExplodeIndex), typeof(int), typeof(SfPyramidChart),
            new PropertyMetadata(-1, OnExplodeIndexChanged));

        ///// <summary>
        ///// Identifies the <c>ExplodeAll</c> dependency property.
        ///// </summary>        
        ///// <value>
        ///// The identifier for <c>ExplodeAll</c> dependency property.
        ///// </value>
        //public static readonly DependencyProperty ExplodeAllProperty =
        //    DependencyProperty.Register(nameof(ExplodeAll), typeof(bool), typeof(SfPyramidChart),
        //    new PropertyMetadata(false, OnExplodeAllChanged));

        /// <summary>
        /// Identifies the <c>ExplodeOnTap</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ExplodeOnTap</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ExplodeOnTapProperty =
            DependencyProperty.Register(nameof(ExplodeOnTap), typeof(bool), typeof(SfPyramidChart),
            new PropertyMetadata(false, OnExplodeOnClickChanged));

        /// <summary>
        /// Identifies the <c>SelectionBrush</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>SelectionBrush</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(SfPyramidChart),
            new PropertyMetadata(null, OnSelectionBrush));

        /// <summary>
        /// Identifies the <c>SelectedIndex</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>SelectedIndex</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(SfPyramidChart),
            new PropertyMetadata(-1, OnSelectedIndexChanged));

        /// <summary>
        /// Identifies the <c>GapRatio</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>GapRatio</c> dependency property.
        /// </value>   
        public static readonly DependencyProperty GapRatioProperty =
            DependencyProperty.Register(
                nameof(GapRatio),
                typeof(double),
                typeof(SfPyramidChart),
                new PropertyMetadata(0d, new PropertyChangedCallback(OnGapRatioChanged)));

        /// <summary>
        /// Identifies the <c>ExplodeOffset</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>ExplodeOffset</c> dependency property.
        /// </value>     
        public static readonly DependencyProperty ExplodeOffsetProperty =
            DependencyProperty.Register(
                nameof(ExplodeOffset),
                typeof(double),
                typeof(SfPyramidChart),
                new PropertyMetadata(40d, new PropertyChangedCallback(OnExplodeOffsetChanged)));

        /// <summary>
        /// Identifies the <c>Mode</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>Mode</c> dependency property.
        /// </value> 
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(
                nameof(Mode),
                typeof(ChartPyramidMode),
                typeof(SfPyramidChart),
                new PropertyMetadata(ChartPyramidMode.Linear, OnPyramidModeChanged));

        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> dependency property.        
        /// </summary>        
        /// <value>
        /// The identifier for <c>DataLabelSettings</c> dependency property.
        /// </value>
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(PyramidDataLabelSettings), typeof(SfPyramidChart),
          new PropertyMetadata(null, OnAdornmentsInfoChanged));

        /// <summary>
        /// Identifies the <c>ItemsSource</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ItemsSource</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(SfPyramidChart),
            new PropertyMetadata(null, new PropertyChangedCallback(OnDataSourceChanged)));

        /// <summary>
        /// Identifies the <c>XBindingPath</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>XBindingPath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty XBindingPathProperty =
        DependencyProperty.Register(nameof(XBindingPath), typeof(string), typeof(SfPyramidChart),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnBindingPathXChanged)));

        /// <summary>
        /// Identifies the <c>TooltipTemplate</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>TooltipTemplate</c> dependency property.
        /// </value>
        public static readonly DependencyProperty TooltipTemplateProperty =
            DependencyProperty.Register(nameof(TooltipTemplate), typeof(DataTemplate), typeof(SfPyramidChart),
                new PropertyMetadata(null, new PropertyChangedCallback(OnTooltipTemplateChanged)));

        /// <summary>
        /// Identifies the <c>ShowTooltip</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ShowTooltip</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ShowTooltipProperty =
            DependencyProperty.Register(nameof(ShowTooltip), typeof(bool), typeof(SfPyramidChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnShowTooltipChanged)));

        /// <summary>
        /// Identifies the <c>ColorValuePath</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ColorValuePath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ColorValuePathProperty =
            DependencyProperty.Register(nameof(ColorValuePath), typeof(string), typeof(SfPyramidChart),
                   new PropertyMetadata(null, new PropertyChangedCallback(OnSegmentColorPathChanged)));

        /// <summary>
        /// Identifies the <see cref="ShowDataLabels"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ShowDataLabels</c> dependency property and its default value is false.
        /// </value>   
        public static readonly DependencyProperty ShowDataLabelsProperty =
            DependencyProperty.Register(nameof(ShowDataLabels), typeof(bool), typeof(SfFunnelChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnShowDataLabelsChanged)));

        /// <summary>
        /// The DependencyProperty for <see cref="Series"/> property.
        /// </summary>
        internal static readonly DependencyProperty PyramidProperty =
#pragma warning disable CS0618 // Type or member is obsolete
            DependencyProperty.Register(nameof(Pyramid), typeof(PyramidSeries), typeof(SfPyramidChart),
#pragma warning restore CS0618 // Type or member is obsolete
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Series"/> property.
        /// </summary>
        internal static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(nameof(Series), typeof(ChartSeriesCollection), typeof(SfPyramidChart),
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SfPyramidChart class.
        /// </summary>
        public SfPyramidChart()
        {
#if NETCORE
#if SyncfusionLicense
           if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
               LicenseHelper.ValidateLicense();
            }
#endif
#endif
            DefaultStyleKey = typeof(SfPyramidChart);
#pragma warning disable CS0618 // Type or member is obsolete
            Pyramid = new PyramidSeries();
#pragma warning restore CS0618 // Type or member is obsolete
            Series = new ChartSeriesCollection();
            Series.Add(Pyramid);
            DataLabelSettings = new PyramidDataLabelSettings();
        }
        #endregion

        #region Public Property 

        /// <summary>
        /// Gets or sets the DataTemplate used to customize the tooltip appearence.
        /// </summary>
        /// <value>
        /// This accepts a <see cref="DataTemplate"/> value.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        ///   <syncfusion:SfPyramidChart ShowTooltip="True" ItemsSource="{Binding Demands} XBindingPath="Demand" YBindingPath="Year2010">
        ///      <syncfusion:SfPyramidChart.TooltipTemplate>
        ///        <DataTemplate>
        ///            <Border Background="DarkGreen" CornerRadius="5" BorderThickness="2" BorderBrush="Black" Width="50" Height="30">
        ///                <TextBlock Text="{Binding Item.Year2010}" Foreground="White" FontWeight="Bold"  HorizontalAlignment="Center" VerticalAlignment="Center"/>
        ///            </Border>
        ///        </DataTemplate>
        ///      </syncfusion:SfPyramidChart.TooltipTemplate>
        ///   </syncfusion:SfPyramidChart>        
        /// ]]></code>
        /// </example>
        public DataTemplate TooltipTemplate
        {
            get { return (DataTemplate)GetValue(TooltipTemplateProperty); }
            set { SetValue(TooltipTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show or hide the tooltip for series.
        /// </summary>
        /// <value>
        /// if <c>true</c>, the series tooltip will show on clicking or tapping the series area.
        /// </value>
        public bool ShowTooltip
        {
            get { return (bool)GetValue(ShowTooltipProperty); }
            set { SetValue(ShowTooltipProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of data points used to generate chart.
        /// </summary>
        /// <value>The ItemsSource value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfPyramidChart ItemsSource="{Binding Data}" XBindingPath="XValue" 
        ///               YBindingPath="YValue">
        ///
        ///          <chart:SfPyramidChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfPyramidChart.DataContext>
        ///
        ///     </chart:SfPyramidChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPyramidChart chart = new SfPyramidChart();
        ///     
        ///     ViewModel viewmodel = new ViewModel();
        ///
        ///     chart.ItemsSource = viewmodel.Data;
        ///     chart.XBindingPath = "XValue";
        ///     chart.YBindingPath = "YValue";
        /// ]]></code>
        /// ***
        /// </example>
        public object ItemsSource
        {
            get
            {
                return (object)GetValue(ItemsSourceProperty);
            }

            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the property name that associates the primary axis with a property in the itemssource.
        /// </summary>
        /// <value>
        /// The string that represents the property name for primary axis. The default value is null.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfPyramidChart XBindingPath="XValue" 
        ///               YBindingPath="YValue" ItemsSource="{Binding Data}">
        ///
        ///          <chart:SfPyramidChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfPyramidChart.DataContext>
        ///
        ///     </chart:SfPyramidChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPyramidChart chart = new SfPyramidChart();
        ///     
        ///     ViewModel viewmodel = new ViewModel();
        ///     
        ///     chart.XBindingPath = "XValue";
        ///     chart.YBindingPath = "YValue";
        ///     chart.ItemsSource = viewmodel.Data;
        /// ]]></code>
        /// ***
        /// </example>
        public string XBindingPath
        {
            get
            {
                return (string)GetValue(XBindingPathProperty);
            }

            set
            {
                SetValue(XBindingPathProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets a value that indicates to enable the data labels for the pyramid chart.
        /// </summary>
        /// <value>It takes the bool value and its default value is false.</value>
        public bool ShowDataLabels
        {
            get
            {
                return (bool)GetValue(ShowDataLabelsProperty);
            }

            set
            {
                SetValue(ShowDataLabelsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the property binding path for segment color. 
        /// </summary>
        /// <value>
        /// The string that represents the property name for segment color. The default value is null.
        /// </value>
        public string ColorValuePath
        {
            get
            {
                return (string)GetValue(ColorValuePathProperty);
            }

            set
            {
                SetValue(ColorValuePathProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets data labels for the series. This allows us to customize the appearance of a data point by displaying labels, shapes and connector lines.
        /// </summary>
        /// <value>
        /// The <see cref="ChartDataLabelSettings" /> value.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfPyramidChart ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue" ShowDataLabels="True">
        ///
        ///          <chart:SfPyramidChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfPyramidChart.DataContext>
        ///          
        ///          <syncfusion:SfPyramidChart.DataLabelSettings>
        ///                <chart:PyramidDataLabelSettings/>
        ///          <syncfusion:SfPyramidChart.DataLabelSettings>
        ///
        ///     </chart:SfPyramidChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPyramidChart chart = new SfPyramidChart();
        ///     
        ///     ViewModel viewmodel = new ViewModel();
        ///     
        ///     chart.ItemsSource = viewmodel.Data;
        ///     chart.XBindingPath = "XValue";
        ///     chart.YBindingPath = "YValue";
        ///     chart.ShowDataLabels = "True";
        ///     
        ///     chart.DataLabelSettings = new PyramidDataLabelSettings();
        /// ]]></code>
        /// ***
        /// </example>
        public PyramidDataLabelSettings DataLabelSettings
        {
            get
            {
                return (PyramidDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }

            set
            {
                SetValue(DataLabelSettingsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the property name that associates the secondary axis with a property in the itemssource.
        /// </summary>
        /// <value>
        /// The string that represents the property name for secondary axis. The default value is null.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfPyramidChart YBindingPath="YValue" 
        ///               XBindingPath="XValue" ItemsSource="{Binding Data}">
        ///
        ///          <chart:SfPyramidChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfPyramidChart.DataContext>
        ///
        ///     </chart:SfPyramidChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPyramidChart chart = new SfPyramidChart();
        ///     
        ///     ViewModel viewmodel = new ViewModel();
        ///     
        ///     chart.YBindingPath = "YValue";
        ///     chart.XBindingPath = "XValue";
        ///     chart.ItemsSource = viewmodel.Data;
        /// ]]></code>
        /// ***
        /// </example>
        public string YBindingPath
        {
            get { return (string)GetValue(YBindingPathProperty); }
            set { SetValue(YBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of data point (or segment) of chart series to be exploded.
        /// </summary>
        /// <value>Default value is -1.</value>
        public int ExplodeIndex
        {
            get { return (int)GetValue(ExplodeIndexProperty); }
            set { SetValue(ExplodeIndexProperty, value); }
        }

        ///// <summary>
        ///// Gets or sets a value indicating whether to explode all the pyramid slices (segments).
        ///// </summary>
        ///// <value>
        /////     <c>True</c>, will explode all the segments.
        ///// </value>
        //public bool ExplodeAll
        //{
        //    get { return (bool)GetValue(ExplodeAllProperty); }
        //    set { SetValue(ExplodeAllProperty, value); }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether segment slices will explode on clicking or tappping.
        /// </summary>
        /// <value>
        /// if <c>true</c>, the segment will explode on clicking or tappping.
        /// </value>
        public bool ExplodeOnTap
        {
            get { return (bool)GetValue(ExplodeOnTapProperty); }
            set { SetValue(ExplodeOnTapProperty, value); }
        }

        /// <summary>
        /// Gets or sets the interior (brush) for the selected segment(s).
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush SelectionBrush
        {
            get { return (Brush)GetValue(SelectionBrushProperty); }
            set { SetValue(SelectionBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the first segment in the current selection or returns negative one (-1) if the selection is empty.
        /// </summary>
        /// <value>
        /// The index of first segment in the current selection. The default value is negative one (-1).
        /// </value> 
        /// <seealso cref="SelectionBrush"/>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }


        /// <summary>
        /// Gets or sets the ratio of distance between the pyramid segment blocks.
        /// </summary>
        /// <value>Default value is 0 and its value ranges from 0 to 1.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfPyramidChart GapRatio="0.5" YBindingPath="YValue" 
        ///               XBindingPath="XValue" ItemsSource="{Binding Data}">
        ///
        ///          <chart:SfPyramidChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfPyramidChart.DataContext>
        ///
        ///     </chart:SfPyramidChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPyramidChart chart = new SfPyramidChart();
        ///     
        ///     ViewModel viewmodel = new ViewModel();
        ///     
        ///     chart.GapRatio = "0.5";
        ///     chart.YBindingPath = "YValue";
        ///     chart.XBindingPath = "XValue";
        ///     chart.ItemsSource = viewmodel.Data;
        /// ]]></code>
        /// ***
        /// </example>
        /// <remarks>Its used to provide the spacing between the segments.</remarks>
        public double GapRatio
        {
            get { return (double)GetValue(GapRatioProperty); }
            set { SetValue(GapRatioProperty, value); }
        }

        /// <summary>
        /// Gets or sets the distance where the segment is exploded from its origination positions when <c>ExplodeAll</c> is true or <c>ExplodeIndex</c> value is given.
        /// </summary>
        /// <value>Default value is 40.</value>
        public double ExplodeOffset
        {
            get { return (double)GetValue(ExplodeOffsetProperty); }
            set { SetValue(ExplodeOffsetProperty, value); }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the y value should interpret the length or surface of the pyramid block.
        /// </summary>
        /// <value>
        /// One of the <see cref="ChartPyramidMode"/> enumeration values. The default value is <see cref="Syncfusion.UI.Xaml.Charts.ChartPyramidMode.Linear"/>
        /// </value>
        public ChartPyramidMode Mode
        {
            get { return (ChartPyramidMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        #endregion

        #region Internal Property

#pragma warning disable CS0618 // Type or member is obsolete
        internal PyramidSeries Pyramid
        {
            get { return (PyramidSeries)GetValue(PyramidProperty); }
#pragma warning restore CS0618 // Type or member is obsolete
            set { SetValue(PyramidProperty, value); }
        }

        internal ChartSeriesCollection Series
        {
            get { return (ChartSeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }
        
        #endregion

        #region Protected override Method

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (Pyramid != null)
            {
                Pyramid = null;
            }
        }

        #endregion

        #region Private Static Methods

        private static void OnGapRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.GapRatio = pyramidChart.GapRatio;
            }
        }

        private static void OnExplodeOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.ExplodeOffset = pyramidChart.ExplodeOffset;
            }
        }

        private static void OnPyramidModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.PyramidMode = pyramidChart.Mode;
            }
               
        }

        private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.YBindingPath = pyramidChart.YBindingPath;
            }
        }

        private static void OnSelectionBrush(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.SelectionBrush = pyramidChart.SelectionBrush;
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.SelectedIndex = pyramidChart.SelectedIndex;
            }
        }

        private static void OnExplodeIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.ExplodeIndex = pyramidChart.ExplodeIndex;
            }
        }

        //private static void OnExplodeAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    SfPyramidChart pyramidChart = d as SfPyramidChart;
        //    if (pyramidChart != null && pyramidChart.Pyramid != null)
        //    {
        //        pyramidChart.Pyramid.ExplodeAll = pyramidChart.ExplodeAll;
        //    }
        //}

        private static void OnAdornmentsInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.DataLabelSettings = pyramidChart.DataLabelSettings;
            }
        }
        private static void OnBindingPathXChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SfPyramidChart pyramidChart = obj as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
               pyramidChart.Pyramid.XBindingPath = pyramidChart.XBindingPath;
            }
        }
        private static void OnDataSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SfPyramidChart pyramidChart = obj as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.ItemsSource = pyramidChart.ItemsSource;
            }
        }
        private static void OnSegmentColorPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.SegmentColorPath = pyramidChart.ColorValuePath;
            }
        }

        private static void OnTooltipTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.TooltipTemplate = pyramidChart.TooltipTemplate;
            }
        }

        private static void OnShowTooltipChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.ShowTooltip = pyramidChart.ShowTooltip;
            }
        }

        private static void OnExplodeOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.ExplodeOnTap = pyramidChart.ExplodeOnTap;
            }
        }

        private static void OnShowDataLabelsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPyramidChart pyramidChart = d as SfPyramidChart;
            if (pyramidChart != null && pyramidChart.Pyramid != null)
            {
                pyramidChart.Pyramid.ShowDataLabels = pyramidChart.ShowDataLabels;
            }
        }

        #endregion

        internal override ChartSeriesBase GetSeries(string seriesName)
        {
            return Series[seriesName];
        }

        internal override IList GetSeriesCollection()
        {
            return Series;
        }

        internal override ObservableCollection<ChartSeries> GetChartSeriesCollection()
        {
            return new ObservableCollection<ChartSeries>(Series);
        }

        internal override void SetSeriesCollection(IList seriesCollection)
        {
            Series = (ChartSeriesCollection)seriesCollection;
        }

        internal override void SetSeriesColorModel(ChartColorModel chartColorModel)
        {
            if (Pyramid != null)
            {
                Pyramid.Palette = chartColorModel.Palette;
                Pyramid.ColorModel = chartColorModel;
            }
        }
    }
}
