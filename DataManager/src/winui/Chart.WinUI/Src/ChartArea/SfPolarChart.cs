using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Markup;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Renders polar line and area charts for data representation and enhanced user interface visualization.
    /// </summary>
    /// <remarks>
    /// <para>Polar chart control is used to visualize the data in terms of values and angles.</para>
    ///
    /// <para>SfPolarChart class properties provides an option to add the series collection, allows to customize the chart elements such as legend, data label, and tooltip features. </para>
    ///
    /// <para><b>Series</b></para>
    /// 
    /// <para>ChartSeries is the visual representation of data. SfPolarChart offers <see cref="PolarAreaSeries"/> and <see cref="PolarLineSeries"/>.</para>
    /// 
    /// <para>To render a series, create an instance of required series class, and add it to the <see cref="Series"/> collection.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code> <![CDATA[
    /// <chart:SfPolarChart>
    ///
    ///        <chart:SfPolarChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfPolarChart.DataContext>
    ///
    ///        <chart:PolarAreaSeries ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>
    /// </chart:SfPolarChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfPolarChart chart = new SfPolarChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    /// 
    /// PolarAreaSeries series = new PolarAreaSeries()
    /// {
    ///     ItemsSource = viewmodel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue"
    /// };
    /// chart.Series.Add(series);
    /// ]]>
    /// </code>
    /// # [ViewModel.cs](#tab/tabid-3)
    /// <code><![CDATA[
    /// public ObservableCollection<Model> Data { get; set; }
    /// 
    /// public ViewModel()
    /// {
    ///    Data = new ObservableCollection<Model>();
    ///    Data.Add(new Model() { XValue = "A", YValue = 100 });
    ///    Data.Add(new Model() { XValue = "B", YValue = 150 });
    ///    Data.Add(new Model() { XValue = "C", YValue = 110 });
    ///    Data.Add(new Model() { XValue = "D", YValue = 230 });
    /// }
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Legend</b></para>
    /// 
    /// <para>The Legend contains list of data points in chart series. The information provided in each legend item helps to identify the corresponding data point in chart series. The Series <see cref="ChartSeries.XBindingPath"/> property value will be displayed in the associated legend item.</para>
    /// 
    /// <para>To render a legend, create an instance of <see cref="ChartLegend"/>, and assign it to the <see cref="ChartBase.Legend"/> property. </para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-4)
    /// <code> <![CDATA[
    /// <chart:SfPolarChart>
    ///
    ///        <chart:SfPolarChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfPolarChart.DataContext>
    ///
    ///        <chart:SfPolarChart.Legend>
    ///            <chart:ChartLegend/>
    ///        </chart:SfPolarChart.Legend>
    ///
    ///        <chart:PolarAreaSeries Label="Series 1" ItemsSource = "{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"/>
    /// </chart:SfCircularChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-5)
    /// <code><![CDATA[
    /// SfPolarChart chart = new SfPolarChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    ///	
    /// chart.Legend = new ChartLegend();
    /// 
    /// PolarAreaSeries series = new PolarAreaSeries()
    /// {
    ///     ItemsSource = viewmodel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue",
    ///     Label="Series 1"
    /// };
    /// chart.Series.Add(series);
    /// ]]>
    /// </code>
    /// ***
    /// 
    /// <para><b>Data Label</b></para>
    /// 
    /// <para>Data labels are used to display values related to a chart segment. To render the data labels, you need to enable the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <b>ChartSeries</b> class. </para>
    /// 
    /// <para>To customize the chart data labels alignment, placement and label styles, need to create an instance of <see cref="PolarDataLabelSettings"/> and set to the <see cref="PolarRadarSeriesBase.DataLabelSettings"/> property.</para>
    /// 
    /// # [MainPage.xaml](#tab/tabid-6)
    /// <code><![CDATA[
    /// <chart:SfPolarChart>
    ///
    ///        <chart:SfPolarChart.DataContext>
    ///            <local:ViewModel/>
    ///        </chart:SfPolarChart.DataContext>
    ///
    ///        < chart:PolarAreaSeries ShowDataLabels = "True"
    ///                                ItemsSource="{Binding Data}"
    ///                                XBindingPath="XValue"
    ///                                YBindingPath="YValue"/>
    /// </chart:SfPolarChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-7)
    /// <code><![CDATA[
    /// SfPolarChart chart = new SfPolarChart();
    /// 
    /// ViewModel viewModel = new ViewModel();
    ///	chart.DataContext = viewModel;
    /// 
    /// PolarAreaSeries series = new PolarAreaSeries()
    /// {
    ///     ItemsSource = viewmodel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue",
    ///     ShowDataLabels = true
    /// };
    /// chart.Series.Add(series);
    /// ]]>
    /// </code>
    /// ***
    /// </remarks>
    [ContentProperty(Name = "Series")]
    public class SfPolarChart : ChartBase
    {
        #region Dependency Property Registration


        /// <summary>
        /// Identifies the <see cref="PaletteBrushes"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PaletteBrushes</c> dependency property.
        /// </value>      
        public static readonly DependencyProperty PaletteBrushesProperty =
            DependencyProperty.Register(
                nameof(PaletteBrushes),
                typeof(IList<Brush>),
                typeof(SfPolarChart),
                new PropertyMetadata(ChartColorModel.DefaultBrushes, OnPaletteBrushesChanged));

        /// <summary>
        /// Identifies the <see cref="GridLineType"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>GridLineType</c> dependency property.
        /// </value>
        public static readonly DependencyProperty GridLineTypeProperty =
            DependencyProperty.Register(nameof(GridLineType), typeof(PolarChartGridLineType), typeof(SfPolarChart),
            new PropertyMetadata(PolarChartGridLineType.Circle, OnShapeStyleChanged));

        /// <summary>
        /// Identifies the <see cref="PrimaryAxis"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PrimaryAxis</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PrimaryAxisProperty =
            DependencyProperty.Register(
                nameof(PrimaryAxis),
                typeof(ChartAxis),
                typeof(SfPolarChart),
                new PropertyMetadata(null, OnPrimaryAxisChanged));

        /// <summary>
        /// Identifies the <see cref="SecondaryAxis"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>SecondaryAxis</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SecondaryAxisProperty =
            DependencyProperty.Register(
                nameof(SecondaryAxis),
                typeof(RangeAxisBase),
                typeof(SfPolarChart),
                new PropertyMetadata(null, OnSecondaryAxisChanged));


        /// <summary>
        /// Identifies the <see cref="Series"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Series</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(
                nameof(Series),
                typeof(PolarSeriesCollection),
                typeof(SfPolarChart),
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));

        /// <summary>
        /// Identifies the <see cref="StartAngle"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>StartAngle</c> dependency property.
        /// </value>
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register(
                nameof(StartAngle),
                typeof(ChartPolarAngle),
                typeof(SfPolarChart),
                new PropertyMetadata(ChartPolarAngle.Rotate270, OnStartAngleChanged));
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfPolarChart"/> class.
        /// </summary>
        public SfPolarChart()
        {
#if NETCORE
#if SyncfusionLicense
           if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
               LicenseHelper.ValidateLicense();
            }
#endif
#endif
            DefaultStyleKey = typeof(SfPolarChart);
            Series = new PolarSeriesCollection();
            AreaType = ChartAreaType.PolarAxes;
        }

        #endregion

        #region Public Property 

        /// <summary>
        /// Gets or sets the list of brushes that can be used to customize the appearance of the chart.
        /// </summary>
        /// <remarks>It allows custom brushes, and gradient brushes to customize the appearance.</remarks>
        /// <value>This property accepts a list of brushes as input and comes with a set of predefined brushes by default.</value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-8)
        /// <code><![CDATA[
        /// <Grid>
        /// <Grid.Resources>
        ///    <BrushCollection x:Key="customBrushes">
        ///       <SolidColorBrush Color="#4dd0e1"/>
        ///       <SolidColorBrush Color="#26c6da"/>
        ///       <SolidColorBrush Color="#00bcd4"/>
        ///       <SolidColorBrush Color="#00acc1"/>
        ///       <SolidColorBrush Color="#0097a7"/>
        ///       <SolidColorBrush Color="#00838f"/>
        ///    </BrushCollection>
        /// </Grid.Resources>
        /// 
        /// <chart:SfPolarChart PaletteBrushes="{StaticResource customBrushes}">
        /// 
        /// <!--omitted for brevity-->
        /// 
        /// </chart:SfPolarChart>
        /// </Grid>
        /// ]]>
        /// </code>
        ///
        /// # [MainPage.xaml.cs](#tab/tabid-9)
        /// <code><![CDATA[
        /// SfPolarChart chart = new SfPolarChart();
        /// ViewModel viewModel = new ViewModel();
        /// List<Brush> CustomBrushes = new List<Brush>();
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 77, 208, 225)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 38, 198, 218)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 188, 212)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 172, 193)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 151, 167)));
        /// CustomBrushes.Add(new SolidColorBrush(Color.FromArgb(255, 0, 131, 143)));
        ///
        /// // omitted for brevity
        /// 
        /// chart.PaletteBrushes = CustomBrushes;
        /// 
        /// this.Content = chart;
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public IList<Brush> PaletteBrushes
        {
            get { return (IList<Brush>)GetValue(PaletteBrushesProperty); }
            set { SetValue(PaletteBrushesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that can be used to modify the series starting angle.
        /// </summary>
        /// <remarks>It allows for modifying the series rendering position on four degrees: 0, 90, 180, and 270.</remarks>
        /// <value>It accepts the <see cref="ChartPolarAngle"/> value and it has the default value of <see cref="ChartPolarAngle.Rotate270"/>.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-10)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart StartAngle="Rotate0">
        ///  
        ///     <!--omitted for brevity-->
        ///
        ///          <chart:PolarAreaSeries ItemsSource="{Binding Data}" 
        ///                                 XBindingPath="XValue" 
        ///                                 YBindingPath="YValue"/>
        ///                    
        ///     </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-11)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     chart.StartAngle = ChartPolarAngle.Rotate0;
        ///     ViewModel viewmodel = new ViewModel();
        ///
        ///     // omitted for brevity
        ///     PolarAreaSeries series = new PolarAreaSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartPolarAngle StartAngle
        {
            get { return (ChartPolarAngle)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of series to be added to the chart.
        /// </summary>
        /// <remarks>To render a series, create an instance of required series class, and add it to the collection.</remarks>
        /// <value>It accepts the <see cref="PolarSeriesCollection"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-12)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///  
        ///          <!--omitted for brevity-->
        ///
        ///          <chart:PolarAreaSeries ItemsSource="{Binding Data}" 
        ///                                 XBindingPath="XValue" 
        ///                                 YBindingPath="YValue"/>
        ///                    
        ///     </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-13)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     ViewModel viewmodel = new ViewModel();
        ///     
        ///     // omitted for brevity
        ///     PolarAreaSeries series = new PolarAreaSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public PolarSeriesCollection Series
        {
            get { return (PolarSeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the gridline type value that can be used to modify the polar chart grid line type to Polygon or Circle.
        /// </summary>
        /// <value> It accepts the <see cref="PolarChartGridLineType"/> value and its default value is <see cref="PolarChartGridLineType.Circle"/>.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-14)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart GridLineType="Polygon">
        ///
        ///     <!--omitted for brevity-->
        ///
        ///          <chart:PolarAreaSeries ItemsSource="{Binding Data}"
        ///                                 XBindingPath="XValue"
        ///                                 YBindingPath="YValue"/>
        ///
        ///     </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-15)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     chart.GridLineType = PolarChartGridLineType.Polygon;
        ///     ViewModel viewmodel = new ViewModel();
        ///
        ///     // omitted for brevity
        ///     PolarAreaSeries series = new PolarAreaSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public PolarChartGridLineType GridLineType
        {
            get { return (PolarChartGridLineType)GetValue(GridLineTypeProperty); }
            set { SetValue(GridLineTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the x- axis for <see cref="SfPolarChart"/>.
        /// </summary>
        /// <value>It accepts the <see cref="ChartAxis"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-16)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///           <chart:SfPolarChart.PrimaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfPolarChart.NumericalAxis>
        ///
        ///           <chart:SfPolarChart.SecondaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfPolarChart.SecondaryAxis>
        ///
        ///     </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-17)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     
        ///     chart.PrimaryAxis = new NumericalAxis();
        ///     chart.SecondaryAxis = new NumericalAxis();
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartAxis PrimaryAxis
        {
            get { return (ChartAxis)GetValue(PrimaryAxisProperty); }
            set { SetValue(PrimaryAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets the y-axis for <see cref="SfPolarChart"/>.
        /// </summary>
        /// <value>It accepts the <see cref="RangeAxisBase"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-18)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///
        ///           <chart:SfPolarChart.PrimaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfPolarChart.NumericalAxis>
        ///
        ///           <chart:SfPolarChart.SecondaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfPolarChart.SecondaryAxis>
        ///
        ///     </chart:SfPolarChart>
        /// ]]>
        /// </code>
        /// # [MainWindow.cs](#tab/tabid-19)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     
        ///     chart.PrimaryAxis = new NumericalAxis();
        ///     chart.SecondaryAxis = new NumericalAxis();
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public RangeAxisBase SecondaryAxis
        {
            get { return (RangeAxisBase)GetValue(SecondaryAxisProperty); }
            set { SetValue(SecondaryAxisProperty, value); }
        }

        #endregion

        #region Private Static Methods
        private static void OnShapeStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPolarChart polarChart = d as SfPolarChart;
            if (polarChart != null)
            {
                polarChart.ScheduleUpdate();
            }
        }

        private static void OnStartAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SfPolarChart polarChart = d as SfPolarChart;
            if (polarChart != null && polarChart.PrimaryAxis != null && polarChart.SecondaryAxis != null)
            {
                polarChart.PrimaryAxis.PolarAngle = polarChart.StartAngle;
                polarChart.SecondaryAxis.PolarAngle = polarChart.StartAngle;
                polarChart.ScheduleUpdate();
            }
        }

        private static void OnPrimaryAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chartAxis = e.NewValue as ChartAxis;

            var oldAxis = e.OldValue as ChartAxis;

            SfPolarChart chartArea = d as SfPolarChart;

            if (chartAxis != null)
            {
                chartAxis.Area = chartArea;
                chartAxis.IsVertical = false;
                chartArea.InternalPrimaryAxis = (ChartAxis)e.NewValue;
                chartAxis.VisibleRangeChanged += chartAxis.OnVisibleRangeChanged;

                if (chartArea.Series != null)
                {
                    foreach(var series in chartArea.Series)
                    {
                        series.ActualXAxis = chartAxis;

                        if (series != null && !chartAxis.RegisteredSeries.Contains(series))
                            chartAxis.RegisteredSeries.Add(series);

                        if (!chartArea.InternalAxes.Contains(chartAxis))
                        {
                            chartArea.InternalAxes.Add(chartAxis);
                            chartArea.DependentSeriesAxes.Add(chartAxis);
                        }
                    }
                }
            }

            if (oldAxis != null)
            {
                if (chartArea != null && chartArea.InternalAxes != null && chartArea.InternalAxes.Contains(oldAxis))
                {
                    chartArea.InternalAxes.RemoveItem(oldAxis, chartArea.DependentSeriesAxes.Contains(oldAxis));

                    chartArea.DependentSeriesAxes.Remove(oldAxis);
                }

                oldAxis.VisibleRangeChanged -= oldAxis.OnVisibleRangeChanged;
                oldAxis.RegisteredSeries.Clear();
                oldAxis.Dispose();
            }

            chartArea.OnAxisChanged(e);

        }

        private static void OnSecondaryAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var chartAxis = e.NewValue as ChartAxis;

            var oldAxis = e.OldValue as ChartAxis;

            SfPolarChart chartArea = d as SfPolarChart;

            if (chartAxis != null)
            {
                chartAxis.Area = chartArea;
                chartAxis.IsVertical = true;
                chartArea.InternalSecondaryAxis = (ChartAxis)e.NewValue;

                if (chartArea.Series != null)
                {
                    foreach (var series in chartArea.Series)
                    {
                        series.ActualYAxis = chartAxis;

                        if (series != null && !chartAxis.RegisteredSeries.Contains(series))
                            chartAxis.RegisteredSeries.Add(series);

                        if (!chartArea.InternalAxes.Contains(chartAxis))
                        {
                            chartArea.InternalAxes.Add(chartAxis);
                            chartArea.DependentSeriesAxes.Add(chartAxis);
                        }
                    }
                }
            }

            if (oldAxis != null)
            {
                var axis = oldAxis as NumericalAxis;

                if (chartArea != null && chartArea.InternalAxes != null && chartArea.InternalAxes.Contains(oldAxis))
                {
                    chartArea.InternalAxes.RemoveItem(oldAxis, chartArea.DependentSeriesAxes.Contains(oldAxis));

                    chartArea.DependentSeriesAxes.Remove(oldAxis);
                }

                oldAxis.RegisteredSeries.Clear();
                oldAxis.Dispose();
            }

            chartArea.OnAxisChanged(e);

        }
        void OnAxisChanged(DependencyPropertyChangedEventArgs e)
        {
            var chartAxis = e.NewValue as ChartAxis;

            if (InternalAxes != null && chartAxis != null && !InternalAxes.Contains(chartAxis))
            {
                chartAxis.Area = this;
                InternalAxes.Insert(0, chartAxis);
                DependentSeriesAxes.Add(chartAxis);
            }

            ScheduleUpdate();
        }
        #endregion

        protected override void OnApplyTemplate()
        {
            PolarPlotArea plotArea = GetTemplateChild("PolarPlotArea") as PolarPlotArea;
            plotArea.SeriesCollection = Series;
            PlotArea = plotArea;

            base.OnApplyTemplate();

            (AreaPanel as PolarAreaPanel).Chart = this;
        }

        internal override void ClearPrimaryAxis()
        {
#pragma warning disable CA1416 // Validate platform compatibility
            ClearValue(PrimaryAxisProperty);
#pragma warning restore CA1416 // Validate platform compatibility
        }

        internal override void ClearSecondaryAxis()
        {
#pragma warning disable CA1416 // Validate platform compatibility
            ClearValue(SecondaryAxisProperty);
#pragma warning restore CA1416 // Validate platform compatibility
        }

        internal override IList GetSeriesCollection()
        {
            return Series;
        }

        internal override void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            base.Dispose(disposing);

            PrimaryAxis = null;
            SecondaryAxis = null;
        }


        internal override ObservableCollection<ChartSeries> GetChartSeriesCollection()
        {
            return new ObservableCollection<ChartSeries>(Series);
        }

        internal override void UnHookSeriesCollection(IList seriesCollection)
        {
            if (seriesCollection is PolarSeriesCollection)
            {
                (seriesCollection as PolarSeriesCollection).CollectionChanged -= OnSeriesCollectionChanged;
            }
            else
            {
                base.UnHookSeriesCollection(seriesCollection);
            }
        }

        internal override void HookSeriesCollection(IList seriesCollection)
        {
            if (seriesCollection is PolarSeriesCollection)
            {
                (seriesCollection as PolarSeriesCollection).CollectionChanged += OnSeriesCollectionChanged;
            }
            else
            {
                base.HookSeriesCollection(seriesCollection);
            }
        }

        internal override void SetSeriesCollection(IList seriesCollection)
        {
            Series = (PolarSeriesCollection)seriesCollection;
        }

        internal override Brush GetPaletteBrush(int index)
        {
            if (this.PaletteBrushes != null)
                return this.PaletteBrushes[index % this.PaletteBrushes.Count()];

            return new SolidColorBrush(Colors.Transparent);
        }

        internal override bool IsNullPaletteBrushes()
        {
            if (this.PaletteBrushes == null)
                return true;

            return false;
        }

        private static void OnPaletteBrushesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SfPolarChart)d).OnPaletteBrushesChanged(e);
        }

        private void OnPaletteBrushesChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.VisibleSeries.Count > 0)//ColorModel custom brush dynamic update not working in native control.-WP-610
            {
                for (int index = 0; index < VisibleSeries.Count; index++)
                {
                    (this.VisibleSeries[index] as ChartSeries).Segments.Clear();
                }

                if (PlotArea != null)
                    PlotArea.ShouldPopulateLegendItems = true;

                ScheduleUpdate();
            }
        }
    }
}
