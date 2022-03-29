using Microsoft.UI.Xaml;
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
    /// Represents a series which visualizes data in terms of values and angles. It provides options for visual comparison between several quantitative or qualitative aspects of a situation. 
    /// </summary>
    /// <remarks>
    /// Polar charts are most commonly used to plot polar data, where each data point is determined by an angle and a distance.
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///  
    ///          <chart:SfPolarChart.DataContext>
    ///            <local:ViewModel/>
    ///          </chart:SfPolarChart.DataContext>
    ///
    ///           <chart:SfPolarChart.PrimaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfPolarChart.PrimaryAxis>
    ///
    ///           <chart:SfPolarChart.SecondaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfPolarChart.SecondaryAxis>
    ///
    ///           <chart:PolarAreaSeries
    ///               ItemsSource="{Binding Data}"
    ///               XBindingPath="XValue"
    ///               YBindingPath="YValue"/>
    ///                    
    ///     </chart:SfPolarChart>
    /// ]]></code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     
    ///     ViewModel viewmodel = new ViewModel();
    ///     
    ///     NumericalAxis primaryAxis = new NumericalAxis();
    ///     NumericalAxis secondaryAxis = new NumericalAxis();
    ///     
    ///     chart.PrimaryAxis = primaryAxis;
    ///     chart.SecondaryAxis = secondaryAxis;
    ///     
    ///     PolarAreaSeries series = new PolarAreaSeries();
    ///     series.ItemsSource = viewmodel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    /// ]]></code>
    /// # [ViewModel.cs](#tab/tabid-3)
    /// <code><![CDATA[
    /// public ObservableCollection<Model> Data { get; set; }
    /// 
    /// public ViewModel()
    /// {
    ///    Data = new ObservableCollection<Model>();
    ///    Data.Add(new Model() { XValue = 10, YValue = 100 });
    ///    Data.Add(new Model() { XValue = 20, YValue = 150 });
    ///    Data.Add(new Model() { XValue = 30, YValue = 110 });
    ///    Data.Add(new Model() { XValue = 40, YValue = 230 });
    /// }
    /// ]]></code>
    /// ***
    /// </example>
    /// <seealso cref="PolarAreaSeries"/>
    /// <seealso cref="PolarLineSeries"/>
    [ContentProperty(Name = "Series")]
    public class SfPolarChart : ChartBase
    {
        #region Dependency Property Registration

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
                typeof(ChartAxisBase2D),
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
        /// Initializes a new instance of the SfPolarChart class.
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
        /// Gets or sets the start angle Polar or radar series.
        /// </summary>
        /// <value>It takes the <see cref="ChartPolarAngle"/> value and its default value is <see cref="ChartPolarAngle.Rotate270"/>.</value>
        public ChartPolarAngle StartAngle
        {
            get { return (ChartPolarAngle)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of series to be added to the chart. To render a series, create an instance of required series class, and add it to the collection.
        /// </summary>
        /// <value>It takes the <see cref="PolarSeriesCollection"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart>
        ///  
        ///          <chart:SfPolarChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfPolarChart.DataContext>
        ///
        ///           <chart:SfPolarChart.PrimaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfPolarChart.PrimaryAxis>
        ///
        ///           <chart:SfPolarChart.SecondaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfPolarChart.SecondaryAxis>
        ///
        ///           <chart:PolarAreaSeries
        ///               ItemsSource="{Binding Data}"
        ///               XBindingPath="XValue"
        ///               YBindingPath="YValue"/>
        ///                    
        ///     </chart:SfPolarChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     
        ///     ViewModel viewmodel = new ViewModel();
        ///     
        ///     NumericalAxis primaryAxis = new NumericalAxis();
        ///     NumericalAxis secondaryAxis = new NumericalAxis();
        ///     
        ///     chart.PrimaryAxis = primaryAxis;
        ///     chart.SecondaryAxis = secondaryAxis;
        ///     
        ///     PolarAreaSeries series = new PolarAreaSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        /// ]]></code>
        /// # [ViewModel.cs](#tab/tabid-3)
        /// <code><![CDATA[
        /// public ObservableCollection<Model> Data { get; set; }
        /// 
        /// public ViewModel()
        /// {
        ///    Data = new ObservableCollection<Model>();
        ///    Data.Add(new Model() { XValue = 10, YValue = 100 });
        ///    Data.Add(new Model() { XValue = 20, YValue = 150 });
        ///    Data.Add(new Model() { XValue = 30, YValue = 110 });
        ///    Data.Add(new Model() { XValue = 40, YValue = 230 });
        /// }
        /// ]]></code>
        /// ***
        /// </example>
        public PolarSeriesCollection Series
        {
            get { return (PolarSeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the gridline type value to render the polar or radar chart.
        /// </summary>
        /// <value>Default value is <see cref="PolarChartGridLineType.Circle"/>.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfPolarChart GridLineType="Polygon">
        ///
        ///           <chart:SfPolarChart.PrimaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfPolarChart.NumericalAxis>
        ///
        ///           <chart:SfPolarChart.SecondaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfPolarChart.SecondaryAxis>
        ///           
        ///           <chart:PolarAreaSeries
        ///               ItemsSource="{Binding Data}"
        ///               XBindingPath="XValue"
        ///               YBindingPath="YValue"/>
        ///
        ///     </chart:SfPolarChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     chart.GridLineType = PolarChartGridLineType.Polygon;
        ///     
        ///     NumericalAxis primaryAxis = new NumericalAxis();
        ///     NumericalAxis secondaryAxis = new NumericalAxis();
        ///     
        ///     chart.PrimaryAxis = primaryAxis;
        ///     chart.SecondaryAxis = secondaryAxis;
        ///     
        ///     PolarAreaSeries series = new PolarAreaSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        /// ]]></code>
        /// ***
        /// </example>
        public PolarChartGridLineType GridLineType
        {
            get { return (PolarChartGridLineType)GetValue(GridLineTypeProperty); }
            set { SetValue(GridLineTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets primary axis for <see cref="SfPolarChart"/>.
        /// </summary>
        /// <value>It takes the <see cref="ChartAxisBase2D"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
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
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     
        ///     NumericalAxis primaryAxis = new NumericalAxis();
        ///     NumericalAxis secondaryAxis = new NumericalAxis();
        ///     
        ///     chart.PrimaryAxis = primaryAxis;
        ///     chart.SecondaryAxis = secondaryAxis;
        /// ]]></code>
        /// ***
        /// </example>
        public ChartAxisBase2D PrimaryAxis
        {
            get { return (ChartAxisBase2D)GetValue(PrimaryAxisProperty); }
            set { SetValue(PrimaryAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets secondary axis for <see cref="SfPolarChart"/>.
        /// </summary>
        /// <value>It takes the <see cref="RangeAxisBase"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
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
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfPolarChart chart = new SfPolarChart();
        ///     
        ///     NumericalAxis primaryAxis = new NumericalAxis();
        ///     NumericalAxis secondaryAxis = new NumericalAxis();
        ///     
        ///     chart.PrimaryAxis = primaryAxis;
        ///     chart.SecondaryAxis = secondaryAxis;
        /// ]]></code>
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
            }
        }
        #endregion

        internal override ChartAxisBase2D GetPrimaryAxis()
        {
            return PrimaryAxis;
        }

        internal override RangeAxisBase GetSecondaryAxis()
        {
            return SecondaryAxis;
        }

        internal override void SetSecondaryAxis(RangeAxisBase chartAxis)
        {
            SecondaryAxis = chartAxis;
        }

        internal override void SetPrimaryAxis(ChartAxisBase2D chartAxis)
        {
            PrimaryAxis = chartAxis;
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
    }
}
