using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
#if SyncfusionLicense
    using Syncfusion.Core.WinUI;
#endif
using Windows.UI;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;

using System.Runtime.CompilerServices;

namespace Syncfusion.UI.Xaml.Charts
{

    /// <summary>
    /// Represents chart series bounds changed event arguments.
    /// </summary>
    ///<remarks>
    /// It contains information like old bounds and new bounds.
    /// </remarks>
    public class ChartSeriesBoundsEventArgs : EventArgs
    {
#region Constructor

        /// <summary>
        /// Called when instance created for <see cref="ChartSeriesBoundsEventArgs"/>.
        /// </summary>
        public ChartSeriesBoundsEventArgs()
        {

        }
#endregion

        #region Properties

        #region Public Properties
        /// <summary>
        /// Gets or sets the updated bounds of the rectangle.
        /// </summary>
        public Rect NewBounds { get; set; }

        /// <summary>
        /// Gets or sets the previous bounds of the rectangle.
        /// </summary>
        public Rect OldBounds { get; set; }
#endregion

#endregion
    }

    /// <summary>
    /// Defines the namespace to intialize the SfChart and SfDateTimeRangeNavigator control.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes")]
    static class NamespaceDoc
    {

    }

    /// <summary>
    /// Represents the Chart control which is used to visualize the data graphically.
    /// </summary>
    /// <remarks>
    /// The Chart is often used to make it easier to
    /// understand large amount of data and the relationship between different parts
    /// of the data. Chart can usually be read more quickly than the raw data that they
    /// come from. <para> Certain <see cref="ChartSeriesBase" /> are more useful for
    /// presenting a given data set than others. For example, data that presents
    /// percentages in different groups (such as "satisfied, not satisfied, unsure") are
    /// often displayed in a <see cref="PieSeries" /> chart, but are more easily
    /// understood when presented in a horizontal <see cref="BarSeries" /> chart.
    /// On the other hand, data that represents numbers that change over a period of
    /// time (such as "annual revenue from 2011 to 2012") might be best shown as a <see
    /// cref="LineSeries" /> chart. </para>
    /// </remarks>
    /// <seealso cref="ChartSeriesBase"/>
    /// <seealso cref="ChartLegend"/>
    /// <seealso cref="ChartAxis"/>
    [Obsolete("Use SfCartesianChart, SfCircularChart, SfFunnelChart, SfPyramidChart, SfPolarChart", false)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1816: Call GC.SuppressFinalize correctly")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1063: Implement IDisposable correctly")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    [ContentProperty(Name = "Series")]
    public class SfChart : ChartBase
    {
        #region Dependency Property Registration


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
                typeof(SfChart),
                new PropertyMetadata(true, OnSideBySideSeriesPlacementProperty));

        /// <summary>
        /// Identifies the <see cref="PlotAreaBorderBrush"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PlotAreaBorderBrush</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PlotAreaBorderBrushProperty =
            DependencyProperty.Register(nameof(PlotAreaBorderBrush), typeof(Brush), typeof(SfChart), new PropertyMetadata(null));

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
                typeof(SfChart),
                new PropertyMetadata(new Thickness().GetThickness(0, 0, 0, 0)));

        /// <summary>
        /// Identifies the <see cref="PlotAreaBackground"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PlotAreaBackground</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PlotAreaBackgroundProperty =
            DependencyProperty.Register(nameof(PlotAreaBackground), typeof(Brush), typeof(SfChart), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="PrimaryAxis"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>PrimaryAxis</c> dependency property.
        /// </value>
        public static readonly DependencyProperty PrimaryAxisProperty =
            DependencyProperty.Register(
                "PrimaryAxis",
                typeof(ChartAxisBase2D),
                typeof(SfChart),
                new PropertyMetadata(null, OnPrimaryAxisChanged));

        /// <summary>
        /// Identifies the <see cref="SecondaryAxis"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>SecondaryAxis</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SecondaryAxisProperty =
            DependencyProperty.Register(
                "SecondaryAxis",
                typeof(RangeAxisBase),
                typeof(SfChart),
                new PropertyMetadata(null, OnSecondaryAxisChanged));

        /// <summary>
        /// Identifies the <see cref="Series"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>Series</c> dependency property.
        /// </value>
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(
                "Series",
                typeof(ChartSeriesCollection),
                typeof(SfChart),
                new PropertyMetadata(null, OnSeriesPropertyCollectionChanged));
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets primary axis for <see cref="SfCartesianChart"/>.
        /// </summary>
        public ChartAxisBase2D PrimaryAxis
        {
            get { return (ChartAxisBase2D)GetValue(PrimaryAxisProperty); }
            set { SetValue(PrimaryAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets the secondary axis for <see cref="SfChart"/>.
        /// </summary>
        public RangeAxisBase SecondaryAxis
        {
            get { return (RangeAxisBase)GetValue(SecondaryAxisProperty); }
            set { SetValue(SecondaryAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of series to be added to the chart. To render a series, create an instance of required series class, and add it to the collection.
        /// </summary>
        public ChartSeriesCollection Series
        {
            get { return (ChartSeriesCollection)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color to paint the outline of chart area.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush PlotAreaBorderBrush
        {
            get { return (Brush)GetValue(PlotAreaBorderBrushProperty); }
            set { SetValue(PlotAreaBorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the outline thickness of chart area.
        /// </summary>
        public Thickness PlotAreaBorderThickness
        {
            get { return (Thickness)GetValue(PlotAreaBorderThicknessProperty); }
            set { SetValue(PlotAreaBorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background color of the plotting area.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush PlotAreaBackground
        {
            get { return (Brush)GetValue(PlotAreaBackgroundProperty); }
            set { SetValue(PlotAreaBackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value that indicates whether the series can be placed side by side.
        /// </summary>        
        public bool EnableSideBySideSeriesPlacement
        {
            get { return (bool)GetValue(EnableSideBySideSeriesPlacementProperty); }
            set { SetValue(EnableSideBySideSeriesPlacementProperty, value); }
        }

        #region Events

        /// <summary>
        /// Occurs when chart zooming value is changed.
        /// </summary>
        public event EventHandler<ZoomChangedEventArgs> ZoomChanged;

        /// <summary>
        /// Occurs when zooming takes place in chart.
        /// </summary>
        public event EventHandler<ZoomChangingEventArgs> ZoomChanging;

        /// <summary>
        /// Occurs at the start of selection zooming.
        /// </summary>
        public event EventHandler<SelectionZoomingStartEventArgs> SelectionZoomingStart;

        /// <summary>
        /// Occurs during selection zooming.
        /// </summary>
        public event EventHandler<SelectionZoomingDeltaEventArgs> SelectionZoomingDelta;

        /// <summary>
        /// Occurs at the end of selection zooming.
        /// </summary>
        public event EventHandler<SelectionZoomingEndEventArgs> SelectionZoomingEnd;

        /// <summary>
        /// Occurs when panning position of chart is changed.
        /// </summary>
        public event EventHandler<PanChangedEventArgs> PanChanged;

        /// <summary>
        /// Occurs when panning takes place in chart.
        /// </summary>
        public event EventHandler<PanChangingEventArgs> PanChanging;

        /// <summary>
        /// Occurs when the zoom is reset.
        /// </summary>
        public event EventHandler<ResetZoomEventArgs> ResetZooming;

        /// <summary>
        /// Gets the collection of horizontal and vertical axis.
        /// </summary>
        public ChartAxisCollection Axes { get
            {
                return InternalAxes;
            }
        }


        #endregion

        #endregion

        /// <summary>
        /// Initializes a new instance of the SfChart class.
        /// </summary>
        public SfChart()
        {
            DefaultStyleKey = typeof(SfChart);
            Series = new ChartSeriesCollection();
            this.Loaded += this.OnLoaded;
        }

        /// <summary>
        /// Called when chart control is loaded.
        /// </summary>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
#if SyncfusionLicense
            LicenseHelper.ValidateLicense(this.XamlRoot);
#endif
        }

        /// <summary>
        /// Release the unmanaged resources of <see cref="SfChart"/>.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Loaded -= this.OnLoaded;
            }
        }

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
        internal override void SetSeriesCollection(IList seriesCollection)
        {
            Series = (ChartSeriesCollection)seriesCollection;
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

        internal override bool SideBySideSeriesPlacement => EnableSideBySideSeriesPlacement;

        #region Protected Internal Virtual Methods

        /// <summary>
        /// Occurs when zooming position changed in chart.
        /// </summary>
        /// <param name="args">ZoomChangedEventArgs</param>
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
        /// <param name="args">ZoomChangingEventArgs</param>
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
        /// <param name="args">SelectionZoomingStartEventArgs</param>
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
        /// <param name="args">SelectionZoomingEndEventArgs</param>
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
        /// <param name="args">SelectionZoomingDeltaEventArgs</param>
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
        /// <param name="args">PanChangedEventArgs</param>
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
        /// <param name="args">PanChangingEventArgs</param>
        internal override void OnPanChanging(PanChangingEventArgs args)
        {
            if (PanChanging != null && args != null)
            {
                PanChanging(this, args);
            }
        }

        /// <summary>
        /// Occurs when zoom is reset.
        /// </summary>
        /// <param name="args">ResetZoomEventArgs.</param>
        internal override void OnResetZoom(ResetZoomEventArgs args)
        {
            if (ResetZooming != null && args != null)
            {
                ResetZooming(this, args);
            }
        }

        #endregion
    }


    public class WinUIChart : ChartBase
    {
        public WinUIChart()
        {
            DefaultStyleKey = typeof(WinUIChart);
        }
    }
}
