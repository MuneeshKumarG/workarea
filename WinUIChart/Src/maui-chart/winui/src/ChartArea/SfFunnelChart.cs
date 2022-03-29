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
    /// Represents a funnel chart which displays data in a funnel shape that equals to 100% when totaled, and its plots streamlined data to show various stages in a process. It supports data label, tooltip, and selection.
    /// </summary>
    /// <remarks>
    /// It is a single chart, representing data as portions of 100% and does not use any axes.
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfFunnelChart ItemsSource="{Binding Data}" XBindingPath="XValue" 
    ///               YBindingPath="YValue">
    ///
    ///          <chart:SfFunnelChart.DataContext>
    ///            <local:ViewModel/>
    ///          </chart:SfFunnelChart.DataContext>
    ///
    ///     </chart:SfFunnelChart>
    /// ]]></code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfFunnelChart chart = new SfFunnelChart();
    ///     
    ///     ViewModel viewmodel = new ViewModel();
    ///
    ///     chart.ItemsSource = viewmodel.Data;
    ///     chart.XBindingPath = "XValue";
    ///     chart.YBindingPath = "YValue";
    /// ]]></code>
    /// ***
    /// </example>
    public class SfFunnelChart : ChartBase
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <c>MinimumWidth</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>MinimumWidth</c> dependency property.
        /// </value> 
        public static readonly DependencyProperty MinimumWidthProperty =
            DependencyProperty.Register(
                nameof(MinimumWidth),
                typeof(double),
                typeof(SfFunnelChart),
                new PropertyMetadata(40d, OnMinWidthChanged));


        /// <summary>
        /// Identifies the <c>YBindingPath</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>YBindingPath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty YBindingPathProperty =
            DependencyProperty.Register(nameof(YBindingPath), typeof(string), typeof(SfFunnelChart),
            new PropertyMetadata(null, OnYPathChanged));

        /// <summary>
        /// Identifies the <c>ExplodeIndex</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ExplodeIndex</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ExplodeIndexProperty =
            DependencyProperty.Register(nameof(ExplodeIndex), typeof(int), typeof(SfFunnelChart),
            new PropertyMetadata(-1, OnExplodeIndexChanged));

        ///// <summary>
        ///// Identifies the <c>ExplodeAll</c> dependency property.
        ///// </summary>        
        ///// <value>
        ///// The identifier for <c>ExplodeAll</c> dependency property.
        ///// </value>
        //public static readonly DependencyProperty ExplodeAllProperty =
        //    DependencyProperty.Register(nameof(ExplodeAll), typeof(bool), typeof(SfFunnelChart),
        //    new PropertyMetadata(false, OnExplodeAllChanged));

        /// <summary>
        /// Identifies the <c>ExplodeOnTap</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ExplodeOnTap</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ExplodeOnTapProperty =
            DependencyProperty.Register(nameof(ExplodeOnTap), typeof(bool), typeof(SfFunnelChart),
            new PropertyMetadata(false, OnExplodeOnClickChanged));

        /// <summary>
        /// Identifies the <c>SelectionBrush</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>SelectionBrush</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SelectionBrushProperty =
            DependencyProperty.Register(nameof(SelectionBrush), typeof(Brush), typeof(SfFunnelChart),
            new PropertyMetadata(null, OnSelectionBrush));

        /// <summary>
        /// Identifies the <c>SelectedIndex</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>SelectedIndex</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(SfFunnelChart),
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
                typeof(SfFunnelChart),
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
                typeof(SfFunnelChart),
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
                typeof(ChartFunnelMode),
                typeof(SfFunnelChart),
                new PropertyMetadata(ChartFunnelMode.ValueIsHeight, OnFunneldModeChanged));


        /// <summary>
        /// Identifies the <see cref="DataLabelSettings"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>DataLabelSettings</c> dependency property.
        /// </value>
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(FunnelDataLabelSettings), typeof(SfFunnelChart),
          new PropertyMetadata(null, OnAdornmentsInfoChanged));

        /// <summary>
        /// Identifies the <c>ItemsSource</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ItemsSource</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(SfFunnelChart),
            new PropertyMetadata(null, new PropertyChangedCallback(OnDataSourceChanged)));

        /// <summary>
        /// Identifies the <c>XBindingPath</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>XBindingPath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty XBindingPathProperty =
        DependencyProperty.Register(nameof(XBindingPath), typeof(string), typeof(SfFunnelChart),
            new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnBindingPathXChanged)));

        /// <summary>
        /// Identifies the <c>TooltipTemplate</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>TooltipTemplate</c> dependency property.
        /// </value>
        public static readonly DependencyProperty TooltipTemplateProperty =
            DependencyProperty.Register(nameof(TooltipTemplate), typeof(DataTemplate), typeof(SfFunnelChart),
                new PropertyMetadata(null, new PropertyChangedCallback(OnTooltipTemplateChanged)));

        /// <summary>
        /// Identifies the <c>ShowTooltip</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ShowTooltip</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ShowTooltipProperty =
            DependencyProperty.Register(nameof(ShowTooltip), typeof(bool), typeof(SfFunnelChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnShowTooltipChanged)));

        /// <summary>
        /// Identifies the <c>ColorValuePath</c> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ColorValuePath</c> dependency property.
        /// </value>
        public static readonly DependencyProperty ColorValuePathProperty =
            DependencyProperty.Register(nameof(ColorValuePath), typeof(string), typeof(SfFunnelChart),
                   new PropertyMetadata(null, new PropertyChangedCallback(OnSegmentColorPathChanged)));

        /// <summary>
        /// Identifies the <see cref="ShowDataLabels"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ShowDataLabels</c> dependency property and it default value is false.
        /// </value>   
        public static readonly DependencyProperty ShowDataLabelsProperty =
            DependencyProperty.Register(nameof(ShowDataLabels), typeof(bool), typeof(SfFunnelChart),
                new PropertyMetadata(false, new PropertyChangedCallback(OnShowDataLabelsChanged)));

        /// <summary>
        /// The DependencyProperty for <see cref="Series"/> property.
        /// </summary>
        internal static readonly DependencyProperty FunnelProperty =
#pragma warning disable CS0618 // Type or member is obsolete
            DependencyProperty.Register(nameof(Funnel), typeof(FunnelSeries), typeof(SfFunnelChart),
#pragma warning restore CS0618 // Type or member is obsolete
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Series"/> property.
        /// </summary>
        internal static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(nameof(Series), typeof(ChartSeriesCollection), typeof(SfFunnelChart),
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SfFunnelChart class.
        /// </summary>
        public SfFunnelChart()
        {
#if NETCORE
#if SyncfusionLicense
           if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
               LicenseHelper.ValidateLicense();
            }
#endif
#endif
            DefaultStyleKey = typeof(SfFunnelChart);
#pragma warning disable CS0618 // Type or member is obsolete
            Funnel = new FunnelSeries();
#pragma warning restore CS0618 // Type or member is obsolete
            Series = new ChartSeriesCollection();
            Series.Add(Funnel);
            DataLabelSettings = new FunnelDataLabelSettings();
        }

        #endregion

        #region Public Property 

        /// <summary>
        /// Gets or sets the minimum width for the funnel block.
        /// </summary>
        /// <value>Default value is 40.</value>
        public double MinimumWidth
        {
            get { return (double)GetValue(MinimumWidthProperty); }
            set { SetValue(MinimumWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to customize the tooltip appearence.
        /// </summary>
        /// <value>
        /// This accepts a <see cref="DataTemplate"/> value.
        /// </value>
        /// <example>
        /// <code><![CDATA[
        ///   <syncfusion:SfFunnelChart ShowTooltip="True" ItemsSource="{Binding Demands} XBindingPath="Demand" YBindingPath="Year2010">
        ///      <syncfusion:SfFunnelChart.TooltipTemplate>
        ///        <DataTemplate>
        ///            <Border Background="DarkGreen" CornerRadius="5" BorderThickness="2" BorderBrush="Black" Width="50" Height="30">
        ///                <TextBlock Text="{Binding Item.Year2010}" Foreground="White" FontWeight="Bold"  HorizontalAlignment="Center" VerticalAlignment="Center"/>
        ///            </Border>
        ///        </DataTemplate>
        ///      </syncfusion:SfFunnelChart.TooltipTemplate>
        ///   </syncfusion:SfFunnelChart>        
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
        ///     <chart:SfFunnelChart ItemsSource="{Binding Data}" XBindingPath="XValue" 
        ///               YBindingPath="YValue">
        ///
        ///          <chart:SfFunnelChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfFunnelChart.DataContext>
        ///
        ///     </chart:SfFunnelChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfFunnelChart chart = new SfFunnelChart();
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
        ///     <chart:SfFunnelChart XBindingPath="XValue" 
        ///               YBindingPath="YValue" ItemsSource="{Binding Data}">
        ///
        ///          <chart:SfFunnelChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfFunnelChart.DataContext>
        ///
        ///     </chart:SfFunnelChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfFunnelChart chart = new SfFunnelChart();
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
        /// Gets or sets a value that indicates to enable the data labels for the funnel chart.
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
        ///     <chart:SfFunnelChart ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue" ShowDataLabels="True">
        ///
        ///          <chart:SfFunnelChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfFunnelChart.DataContext>
        ///          
        ///          <syncfusion:SfFunnelChart.DataLabelSettings>
        ///                <chart:FunnelDataLabelSettings/>
        ///          <syncfusion:SfFunnelChart.DataLabelSettings>
        ///
        ///     </chart:SfFunnelChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfFunnelChart chart = new SfFunnelChart();
        ///     
        ///     ViewModel viewmodel = new ViewModel();
        ///     
        ///     chart.ItemsSource = viewmodel.Data;
        ///     chart.XBindingPath = "XValue";
        ///     chart.YBindingPath = "YValue";
        ///     chart.ShowDataLabels = "True";
        ///     
        ///     chart.DataLabelSettings = new FunnelDataLabelSettings();
        /// ]]></code>
        /// ***
        /// </example>
        public FunnelDataLabelSettings DataLabelSettings
        {
            get
            {
                return (FunnelDataLabelSettings)GetValue(DataLabelSettingsProperty);
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
        ///     <chart:SfFunnelChart YBindingPath="YValue" 
        ///               XBindingPath="XValue" ItemsSource="{Binding Data}">
        ///
        ///          <chart:SfFunnelChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfFunnelChart.DataContext>
        ///
        ///     </chart:SfFunnelChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfFunnelChart chart = new SfFunnelChart();
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
        ///// Gets or sets a value indicating whether to explode all the funnel slices (segments).
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
        /// Gets or sets the ratio of distance between the funnel segment blocks.
        /// </summary>
        /// <value>Default value is 0 and its value ranges from 0 to 1.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfFunnelChart GapRatio="0.5" YBindingPath="YValue" 
        ///               XBindingPath="XValue" ItemsSource="{Binding Data}">
        ///
        ///          <chart:SfFunnelChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfFunnelChart.DataContext>
        ///
        ///     </chart:SfFunnelChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfFunnelChart chart = new SfFunnelChart();
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
        /// One of the <see cref="ChartFunnelMode"/> enumeration values. The default value is <see cref="Syncfusion.UI.Xaml.Charts.ChartFunnelMode.ValueIsHeight"/>
        /// </value>
        public ChartFunnelMode Mode
        {
            get { return (ChartFunnelMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }
        #endregion

        #region Internal Property

#pragma warning disable CS0618 // Type or member is obsolete
        internal FunnelSeries Funnel
        {
            get { return (FunnelSeries)GetValue(FunnelProperty); }
#pragma warning restore CS0618 // Type or member is obsolete
            set { SetValue(FunnelProperty, value); }
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

            if (Funnel != null)
            {
                Funnel = null;
            }
        }

        #endregion

        #region Private Static Methods

        private static void OnGapRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.GapRatio = funnelChart.GapRatio;
            }
        }

        private static void OnExplodeOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.ExplodeOffset = funnelChart.ExplodeOffset;
            }
        }

        private static void OnFunneldModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.FunnelMode = funnelChart.Mode;
            }

        }

        private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.YBindingPath = funnelChart.YBindingPath;
            }
        }

        private static void OnSelectionBrush(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.SelectionBrush = funnelChart.SelectionBrush;
            }
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.SelectedIndex = funnelChart.SelectedIndex;
            }
        }

        private static void OnExplodeIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.ExplodeIndex = funnelChart.ExplodeIndex;
            }
        }

        //private static void OnExplodeAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    SfFunnelChart funnelChart = d as SfFunnelChart;
        //    if (funnelChart != null && funnelChart.Funnel != null)
        //    {
        //        funnelChart.Funnel.ExplodeAll = funnelChart.ExplodeAll;
        //    }
        //}

        private static void OnAdornmentsInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.DataLabelSettings = funnelChart.DataLabelSettings;
            }
        }
        private static void OnBindingPathXChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SfFunnelChart funnelChart = obj as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.XBindingPath = funnelChart.XBindingPath;
            }
        }
        private static void OnDataSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            SfFunnelChart funnelChart = obj as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.ItemsSource = funnelChart.ItemsSource;
            }
        }
        private static void OnSegmentColorPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.SegmentColorPath = funnelChart.ColorValuePath;
            }
        }

        private static void OnTooltipTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.TooltipTemplate = funnelChart.TooltipTemplate;
            }
        }

        private static void OnShowTooltipChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.ShowTooltip = funnelChart.ShowTooltip;
            }
        }

        private static void OnMinWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.MinWidth = funnelChart.MinimumWidth;
            }
        }

        private static void OnExplodeOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.ExplodeOnTap = funnelChart.ExplodeOnTap;
            }
        }

        private static void OnShowDataLabelsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfFunnelChart funnelChart = d as SfFunnelChart;
            if (funnelChart != null && funnelChart.Funnel != null)
            {
                funnelChart.Funnel.ShowDataLabels = funnelChart.ShowDataLabels;
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
            if (Funnel != null)
            {
                Funnel.Palette = chartColorModel.Palette;
                Funnel.ColorModel = chartColorModel;
            }
        }
    }
}
