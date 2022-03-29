using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
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
    /// Represents a circular chart, which is ideal for displaying proportional values in different categories. It supports the animation and interactive features such as tooltip and selection.
    /// </summary>
    /// <remarks>This circular chart control supports <see cref="PieSeries"/> and <see cref="DoughnutSeries"/>.</remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCircularChart>
    ///
    ///           <chart:SfCircularChart.Series>
    ///               <chart:DoughnutSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"/>
    ///           </chart:SfCircularChart.Series>  
    ///           
    ///     </chart:SfCircularChart>
    /// ]]></code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCircularChart chart = new SfCircularChart();
    ///     
    ///     DoughnutSeries series = new DoughnutSeries();
    ///     series.ItemsSource = viewmodel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    ///     
    ///     chart.Series.Add(series1);
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
    [ContentProperty(Name = "Series")]
    public class SfCircularChart : ChartBase
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <see cref="Series"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Series</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(
                nameof(Series),
                typeof(CircularSeriesCollection),
                typeof(SfCircularChart),
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));
        
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SfCircularChart class.
        /// </summary>
        public SfCircularChart()
        {
#if NETCORE
#if SyncfusionLicense
           if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
               LicenseHelper.ValidateLicense();
            }
#endif
#endif
            DefaultStyleKey = typeof(SfCircularChart);
            Series = new CircularSeriesCollection();
        }

        #endregion

        #region Property

        /// <summary>
        /// Gets or sets a collection of series to be added to the chart. To render a series, create an instance of required series class, and add it to the collection.
        /// </summary>
        /// <value>It takes the <see cref="CircularSeriesCollection"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCircularChart>
        ///
        ///           <chart:SfCircularChart.Series>
        ///               <chart:PieSeries
        ///                   ItemsSource="{Binding Data}"
        ///                   XBindingPath="XValue"
        ///                   YBindingPath="YValue"/>
        ///           </chart:SfCircularChart.Series>  
        ///           
        ///     </chart:SfCircularChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCircularChart chart = new SfCircularChart();
        ///     
        ///     PieSeries series = new PieSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///     
        ///     chart.Series.Add(series1);
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
        public CircularSeriesCollection Series
        {
            get { return (CircularSeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
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

        internal override void UnHookSeriesCollection(IList seriesCollection)
        {
            if (seriesCollection is CircularSeriesCollection)
            {
                (seriesCollection as CircularSeriesCollection).CollectionChanged -= OnSeriesCollectionChanged;
            }
            else
            {
                base.UnHookSeriesCollection(seriesCollection);
            }
        }

        internal override void HookSeriesCollection(IList seriesCollection)
        {
            if (seriesCollection is CircularSeriesCollection)
            {
                (seriesCollection as CircularSeriesCollection).CollectionChanged += OnSeriesCollectionChanged;
            }
            else
            {
                base.HookSeriesCollection(seriesCollection);
            }
        }

        internal override void SetSeriesCollection(IList seriesCollection)
        {
            Series = (CircularSeriesCollection)seriesCollection;
        }
    }
}
