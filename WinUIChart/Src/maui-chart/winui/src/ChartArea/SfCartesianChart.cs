using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents the cartesian chart control which is used to visualize the data graphically. The chart is often used to make it easier to
    /// understand large amount of data and the relationship between different parts
    /// of the data. 
    /// </summary>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.PrimaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.PrimaryAxis>
    ///
    ///           <chart:SfCartesianChart.SecondaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.SecondaryAxis>
    ///
    ///           <chart:SfCartesianChart.Series>
    ///               <chart:ColumnSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"/>
    ///                   
    ///               <chart:LineSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue1"/>
    ///           </chart:SfCartesianChart.Series>  
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     
    ///     NumericalAxis primaryAxis = new NumericalAxis();
    ///     NumericalAxis secondaryAxis = new NumericalAxis();
    ///     
    ///     chart.PrimaryAxis = primaryAxis;
    ///     chart.SecondaryAxis = secondaryAxis;
    ///     
    ///     ColumnSeries series = new ColumnSeries();
    ///     series.ItemsSource = viewmodel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    ///     
    ///     LineSeries series1 = new LineSeries();
    ///     series1.ItemsSource = viewmodel.Data;
    ///     series1.XBindingPath = "XValue";
    ///     series1.YBindingPath = "YValue1";
    ///     chart.Series.Add(series1);
    /// ]]></code>
    /// # [ViewModel.cs](#tab/tabid-3)
    /// <code><![CDATA[
    ///     public ObservableCollection<Model> Data { get; set; }
    /// 
    ///     public ViewModel()
    ///     {
    ///        Data = new ObservableCollection<Model>();
    ///        Data.Add(new Model() { XValue = 10, YValue = 100, YValue1 = 110 });
    ///        Data.Add(new Model() { XValue = 20, YValue = 150, YValue1 = 100 });
    ///        Data.Add(new Model() { XValue = 30, YValue = 110, YValue1 = 130 });
    ///        Data.Add(new Model() { XValue = 40, YValue = 230, YValue1 = 180 });
    ///     }
    /// ]]></code>
    /// ***
    /// </example>
    [ContentProperty(Name = "Series")]
    public class SfCartesianChart : ChartBase
    {
        #region Dependency Property Registration

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
                typeof(SfCartesianChart),
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
                typeof(SfCartesianChart),
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
                typeof(CartesianSeriesCollection),
                typeof(SfCartesianChart),
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));


        /// <summary>
        /// Identifies the <see cref="EnableSideBySideSeriesPlacement"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>EnableSideBySideSeriesPlacement</c> dependency property.
        /// </value>
        public static readonly DependencyProperty EnableSideBySideSeriesPlacementProperty =
            DependencyProperty.Register(
                nameof(EnableSideBySideSeriesPlacement),
                typeof(bool),
                typeof(SfCartesianChart),
                new PropertyMetadata(true, OnSideBySideSeriesPlacementProperty));

        /// <summary>
        /// Identifies the <see cref="PlotAreaBorderBrush"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PlotAreaBorderBrush</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PlotAreaBorderBrushProperty =
            DependencyProperty.Register(nameof(PlotAreaBorderBrush), typeof(Brush), typeof(SfCartesianChart), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="PlotAreaBorderThickness"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PlotAreaBorderThickness</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PlotAreaBorderThicknessProperty =
            DependencyProperty.Register(
                nameof(PlotAreaBorderThickness),
                typeof(Thickness),
                typeof(SfCartesianChart),
                new PropertyMetadata(new Thickness().GetThickness(0, 0, 0, 0)));

        /// <summary>
        /// Identifies the <see cref="PlotAreaBackground"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PlotAreaBackground</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PlotAreaBackgroundProperty =
            DependencyProperty.Register(nameof(PlotAreaBackground), typeof(Brush), typeof(SfCartesianChart), new PropertyMetadata(null));


        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the SfCartesianChart class.
        /// </summary>
        public SfCartesianChart()
        {
#if NETCORE
#if SyncfusionLicense
           if (!(bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(System.Windows.DependencyObject)).DefaultValue)
            {
               LicenseHelper.ValidateLicense();
            }
#endif
#endif

            DefaultStyleKey = typeof(SfCartesianChart);
            Series = new CartesianSeriesCollection();
            Axes = new ChartAxisCollection();
            Axes.CollectionChanged += Axes_CollectionChanged;

        }

        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the collection of horizontal and vertical axis.
        /// </summary>
        /// <value>It takes the <see cref="ChartAxisCollection"/> value.</value>
        public ChartAxisCollection Axes { get; internal set; }

        /// <summary>
        /// Gets or sets primary axis for <see cref="SfCartesianChart"/>.
        /// </summary>
        /// <value>It takes the <see cref="ChartAxisBase2D"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.PrimaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.NumericalAxis>
        ///
        ///           <chart:SfCartesianChart.SecondaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.SecondaryAxis>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
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
        /// Gets or sets secondary axis for <see cref="SfCartesianChart"/>.
        /// </summary>
        /// <value>It takes the <see cref="RangeAxisBase"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.PrimaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.NumericalAxis>
        ///
        ///           <chart:SfCartesianChart.SecondaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.SecondaryAxis>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
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

        /// <summary>
        /// Gets or sets a collection of series to be added to the chart. To render a series, create an instance of required series class, and add it to the collection.
        /// </summary>
        /// <value>It takes the <see cref="CartesianSeriesCollection"/> value.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.PrimaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.PrimaryAxis>
        ///
        ///           <chart:SfCartesianChart.SecondaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.SecondaryAxis>
        ///
        ///           <chart:SfCartesianChart.Series>
        ///               <chart:ColumnSeries
        ///                   ItemsSource="{Binding Data}"
        ///                   XBindingPath="XValue"
        ///                   YBindingPath="YValue"/>
        ///                   
        ///               <chart:ColumnSeries
        ///                   ItemsSource="{Binding Data}"
        ///                   XBindingPath="XValue"
        ///                   YBindingPath="YValue1"/>
        ///           </chart:SfCartesianChart.Series>  
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis primaryAxis = new NumericalAxis();
        ///     NumericalAxis secondaryAxis = new NumericalAxis();
        ///     
        ///     chart.PrimaryAxis = primaryAxis;
        ///     chart.SecondaryAxis = secondaryAxis;
        ///     
        ///     ColumnSeries series = new ColumnSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///     
        ///     ColumnSeries series1 = new ColumnSeries();
        ///     series1.ItemsSource = viewmodel.Data;
        ///     series1.XBindingPath = "XValue";
        ///     series1.YBindingPath = "YValue1";
        ///     chart.Series.Add(series1);
        /// ]]></code>
        /// # [ViewModel.cs](#tab/tabid-3)
        /// <code><![CDATA[
        /// public ObservableCollection<Model> Data { get; set; }
        /// 
        /// public ViewModel()
        /// {
        ///    Data = new ObservableCollection<Model>();
        ///    Data.Add(new Model() { XValue = 10, YValue = 100, YValue1 = 110 });
        ///    Data.Add(new Model() { XValue = 20, YValue = 150, YValue1 = 100 });
        ///    Data.Add(new Model() { XValue = 30, YValue = 110, YValue1 = 130 });
        ///    Data.Add(new Model() { XValue = 40, YValue = 230, YValue1 = 180 });
        /// }
        /// ]]></code>
        /// ***
        /// </example>
        public CartesianSeriesCollection Series
        {
            get { return (CartesianSeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color to paint the outline of chart area.
        /// </summary>
        /// <value>
        /// It takes the <see cref="Brush"/> value.
        /// </value>
        public Brush PlotAreaBorderBrush
        {
            get { return (Brush)GetValue(PlotAreaBorderBrushProperty); }
            set { SetValue(PlotAreaBorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the outline thickness of chart area.
        /// </summary>
        /// <value>Default value is 0 and it takes the <see cref="Thickness"/> value.</value>
        public Thickness PlotAreaBorderThickness
        {
            get { return (Thickness)GetValue(PlotAreaBorderThicknessProperty); }
            set { SetValue(PlotAreaBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background color of the plotting area.
        /// </summary>
        /// <value>
        /// It takes the <see cref="Brush"/> value.
        /// </value>
        public Brush PlotAreaBackground
        {
            get { return (Brush)GetValue(PlotAreaBackgroundProperty); }
            set { SetValue(PlotAreaBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether the series can be placed side by side.
        /// </summary>        
        /// <value>It takes the bool value and its default value is true.</value>
        public bool EnableSideBySideSeriesPlacement
        {
            get { return (bool)GetValue(EnableSideBySideSeriesPlacementProperty); }
            set { SetValue(EnableSideBySideSeriesPlacementProperty, value); }
        }
        #endregion

        #region Events

        /// <summary>
        /// Occurs when chart zooming value is changed.
        /// </summary>
        /// <value>The <see cref="ZoomChangedEventArgs"/> that contains the event data.</value>
        internal event EventHandler<ZoomChangedEventArgs> ZoomChanged;

        /// <summary>
        /// Occurs when zooming takes place in chart.
        /// </summary>
        /// <value>The <see cref="ZoomChangingEventArgs"/> that contains the event data.</value>
        internal event EventHandler<ZoomChangingEventArgs> ZoomChanging;

        /// <summary>
        /// Occurs at the start of selection zooming.
        /// </summary>
        /// <value>The <see cref="SelectionZoomingStartEventArgs"/> that contains the event data.</value>
        internal event EventHandler<SelectionZoomingStartEventArgs> SelectionZoomingStart;

        /// <summary>
        /// Occurs during selection zooming.
        /// </summary>
        /// <value>The <see cref="SelectionZoomingDeltaEventArgs"/> that contains the event data.</value>
        internal event EventHandler<SelectionZoomingDeltaEventArgs> SelectionZoomingDelta;

        /// <summary>
        /// Occurs at the end of selection zooming.
        /// </summary>
        /// <value>The <see cref="SelectionZoomingEndEventArgs"/> that contains the event data.</value>
        internal event EventHandler<SelectionZoomingEndEventArgs> SelectionZoomingEnd;

        /// <summary>
        /// Occurs when panning position of chart is changed.
        /// </summary>
        /// <value>The <see cref="PanChangedEventArgs"/> that contains the event data.</value>
        internal event EventHandler<PanChangedEventArgs> PanChanged;

        /// <summary>
        /// Occurs when panning takes place in chart.
        /// </summary>
        /// <value>The <see cref="PanChangingEventArgs"/> that contains the event data.</value>
        internal event EventHandler<PanChangingEventArgs> PanChanging;

        /// <summary>
        /// Occurs when the zoom is reset.
        /// </summary>
        /// <value>The <see cref="ResetZoomEventArgs"/> that contains the event data.</value>
        internal event EventHandler<ResetZoomEventArgs> ResetZooming;


        #endregion

        #region Public Methods

        /// <summary>
        /// Converts screen point to chart value.
        /// </summary>
        /// <param name="axis">The axis value.</param>
        /// <param name="point">The point.</param>
        /// <returns>The double point to value.</returns>        
        public double PointToValue(ChartAxis axis, Point point)
        {
            return ActualPointToValue(axis, point);
        }
        /// <summary>
        /// Converts Value to point.
        /// </summary>
        /// <param name="axis">The Chart axis .</param>
        /// <param name="value">The value.</param>
        /// <returns>The double value to point.</returns>
        public double ValueToPoint(ChartAxis axis, double value)
        {
            return ActualValueToPoint(axis, value);
        }

        #endregion

        #region Internal override methods

        internal override Thickness GetAreaBorderThickness()
        {
            return PlotAreaBorderThickness;
        }

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
            if (seriesCollection is CartesianSeriesCollection)
            {
                (seriesCollection as CartesianSeriesCollection).CollectionChanged -= OnSeriesCollectionChanged;
            }
            else
            {
                base.UnHookSeriesCollection(seriesCollection);
            }
        }

        internal override void HookSeriesCollection(IList seriesCollection)
        {
            if (seriesCollection is CartesianSeriesCollection)
            {
                (seriesCollection as CartesianSeriesCollection).CollectionChanged += OnSeriesCollectionChanged;
            }
            else
            {
                base.HookSeriesCollection(seriesCollection);
            }
        }

        internal override void SetSeriesCollection(IList seriesCollection)
        {
            Series = (CartesianSeriesCollection)seriesCollection;
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

        internal override bool SideBySideSeriesPlacement => EnableSideBySideSeriesPlacement;

        internal override void DisposeZoomEvents()
        {
            if (ZoomChanged != null)
            {
                foreach (var handler in ZoomChanged.GetInvocationList())
                {
                    ZoomChanged -= handler as EventHandler<ZoomChangedEventArgs>;
                }

                ZoomChanged = null;
            }

            if (ZoomChanging != null)
            {
                foreach (var handler in ZoomChanging.GetInvocationList())
                {
                    ZoomChanging -= handler as EventHandler<ZoomChangingEventArgs>;
                }

                ZoomChanging = null;
            }

            if (SelectionZoomingStart != null)
            {
                foreach (var handler in SelectionZoomingStart.GetInvocationList())
                {
                    SelectionZoomingStart -= handler as EventHandler<SelectionZoomingStartEventArgs>;
                }

                SelectionZoomingStart = null;
            }

            if (SelectionZoomingDelta != null)
            {
                foreach (var handler in SelectionZoomingDelta.GetInvocationList())
                {
                    SelectionZoomingDelta -= handler as EventHandler<SelectionZoomingDeltaEventArgs>;
                }

                SelectionZoomingDelta = null;
            }

            if (SelectionZoomingEnd != null)
            {
                foreach (var handler in SelectionZoomingEnd.GetInvocationList())
                {
                    SelectionZoomingEnd -= handler as EventHandler<SelectionZoomingEndEventArgs>;
                }

                SelectionZoomingEnd = null;
            }

            if (PanChanged != null)
            {
                foreach (var handler in PanChanged.GetInvocationList())
                {
                    PanChanged -= handler as EventHandler<PanChangedEventArgs>;
                }

                PanChanged = null;
            }

            if (PanChanging != null)
            {
                foreach (var handler in PanChanging.GetInvocationList())
                {
                    PanChanging -= handler as EventHandler<PanChangingEventArgs>;
                }

                PanChanging = null;
            }

            if (ResetZooming != null)
            {
                foreach (var handler in ResetZooming.GetInvocationList())
                {
                    ResetZooming -= handler as EventHandler<ResetZoomEventArgs>;
                }

                ResetZooming = null;
            }
        }
        #endregion

        #region Protected Internal Virtual Methods

        /// <summary>
        /// Occurs when zooming position changed in chart.
        /// </summary>
        /// <param name="args">The <see cref="ZoomChangedEventArgs"/> that contains the event data.</param>
        internal override void OnZoomChanged(ZoomChangedEventArgs args)
        {
            if (ZoomChanged != null && args != null)
            {
                ZoomChanged(this, args);
            }
        }

        /// <summary>
        /// Occurs when zooming takes place in chart.
        /// </summary>
        /// <param name="args">The <see cref="ZoomChangingEventArgs"/> that contains the event data.</param>
        internal override void OnZoomChanging(ZoomChangingEventArgs args)
        {
            if (ZoomChanging != null && args != null)
            {
                ZoomChanging(this, args);
            }
        }

        /// <summary>
        /// Occurs at the start of selection zooming.
        /// </summary>
        /// <param name="args">The <see cref="SelectionZoomingStartEventArgs"/> that contains the event data.</param>
        internal override void OnSelectionZoomingStart(SelectionZoomingStartEventArgs args)
        {
            if (SelectionZoomingStart != null && args != null)
            {
                SelectionZoomingStart(this, args);
            }
        }

        /// <summary>
        /// Occurs at the end of selection zooming.
        /// </summary>
        /// <param name="args">The <see cref="SelectionZoomingEndEventArgs"/> that contains the event data.</param>
        internal override void OnSelectionZoomingEnd(SelectionZoomingEndEventArgs args)
        {
            if (SelectionZoomingEnd != null && args != null)
            {
                SelectionZoomingEnd(this, args);
            }
        }

        /// <summary>
        /// Occurs while selection zooming in chart.
        /// </summary>
        /// <param name="args">The <see cref="SelectionZoomingDeltaEventArgs"/> that contains the event data.</param>
        internal override void OnSelectionZoomingDelta(SelectionZoomingDeltaEventArgs args)
        {
            if (SelectionZoomingDelta != null && args != null)
            {
                SelectionZoomingDelta(this, args);
            }
        }

        /// <summary>
        /// Occurs when panning position changed in chart.
        /// </summary>
        /// <param name="args">The <see cref="PanChangedEventArgs"/> that contains the event data.</param>
        internal override void OnPanChanged(PanChangedEventArgs args)
        {
            if (PanChanged != null && args != null)
            {
                PanChanged(this, args);
            }
        }

        /// <summary>
        /// Occurs when panning takes place in chart.
        /// </summary>
        /// <param name="args">The <see cref="PanChangingEventArgs"/> that contains the event data.</param>
        internal override void OnPanChanging(PanChangingEventArgs args)
        {
            if (PanChanging != null && args != null)
            {
                PanChanging(this, args);
            }
        }

        /// <summary>
        /// Occurs when zooming is reset.
        /// </summary>
        /// <param name="args">The <see cref="ResetZoomEventArgs"/> that contains the event data.</param>
        internal override void OnResetZoom(ResetZoomEventArgs args)
        {
            if (ResetZooming != null && args != null)
            {
                ResetZooming(this, args);
            }
        }

        #endregion
    }
}
