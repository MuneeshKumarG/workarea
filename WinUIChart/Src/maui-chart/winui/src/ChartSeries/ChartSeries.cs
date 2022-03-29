using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Windows.UI;
using AdornmentsPosition = Syncfusion.UI.Xaml.Charts.BarLabelAlignment;
using ChartAdornmentPresenter = Syncfusion.UI.Xaml.Charts.ChartDataMarkerPresenter;
using StackingSeriesBase = Syncfusion.UI.Xaml.Charts.StackedSeriesBase;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a base class for all series in chart.
    /// </summary>
    public abstract class ChartSeries : ChartSeriesBase
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(nameof(Stroke), typeof(Brush), typeof(ChartSeries),
            new PropertyMetadata(null, OnAppearanceChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="StrokeThickness"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(ChartSeries),
            new PropertyMetadata(2d, OnAppearanceChanged));

        #endregion

        #region Fields

        #region Internal Fields

        internal List<int> selectedSegmentPixels = new List<int>();

        internal HashSet<int> upperSeriesPixels = new HashSet<int>();

        private bool isLoading = true;

        #endregion
        
        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the value that specifies the stroke thickness of chart series. This is a bindable property.
        /// </summary>
        /// <value>
        /// The default value is 2.
        /// </value>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets the <see cref="ChartBase"/> instance.
        /// </summary>
        public ChartBase Area
        {
            get { return ActualArea as ChartBase; }
            internal set
            {
                ActualArea = value;

                if (ActualArea != null)
                {
                    ActualArea.IsLoading = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a brush that specifies the stroke color for chart series to customize its appearance. This is a bindable property.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        /// <remarks>Use ChartSeries Color property to change the line color of StackingLineSeries and StackingLine100Series.</remarks> 
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        internal bool IsLoading
        {
            get
            {
                return isLoading;
            }

            set
            {
                if (isLoading == value || Area == null)
                {
                    return;
                }

                isLoading = value;

                if (!isLoading)
                {
                    if (Area.VisibleSeries != null && Area.VisibleSeries.Count == 0 && Area.TechnicalIndicators != null && Area.TechnicalIndicators.Count == 0)
                    {
                        Area.IsLoading = false;
                        return;
                    }

                    if (Area.TechnicalIndicators != null)
                    {
                        foreach (ChartSeries indicator in Area.TechnicalIndicators)
                        {
                            if (indicator.IsLoading)
                            {
                                return;
                            }
                        }
                    }

                    if (Area.VisibleSeries != null)
                    {
                        foreach (ChartSeries series in Area.VisibleSeries)
                        {
                            if (series.IsLoading)
                            {
                                return;
                            }
                        }
                    }

                    Area.IsLoading = false;
                }
                else
                {
                    Area.IsLoading = true;
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        internal override void Dispose()
        {
            // Note: The segments collection is disposed in chart series panel since the collection changed has to be unhooked.
            Area = null;
            base.Dispose();
        }

        /// <summary>
        /// This method used to get the SfChart data index at an SfChart co-ordinates
        /// </summary>
        /// <param name="x">Used to specify X co-ordinates</param>
        /// <param name="y">Used to specify Y co-ordinates</param>
        /// <returns>Returns data index of type <c>int</c></returns>
        public int GetDataPointIndex(double x, double y)
        {
            return GetDataPointIndex(new Point(x, y));
        }

        #endregion

        #region Internal Static Methods
        
        /// <summary>
        /// Method used to gets the byte value of given color.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        internal static int ConvertColor(Color color)
        {
            var a = color.A + 1;
            var col = (color.A << 24)
                     | ((byte)((color.R * a) >> 8) << 16)
                     | ((byte)((color.G * a) >> 8) << 8)
                     | ((byte)((color.B * a) >> 8));
            return col;
        }

        #endregion

        #region Internal Virtual Methods

        /// <summary>
        /// This method used to get the chart data index at an SfChart co-ordinates
        /// </summary>
        /// <param name="point">Used to indicate the current x and y co-ordinates</param>
        /// <returns>Returns data index of type <c>int</c></returns>
        internal virtual int GetDataPointIndex(Point point)
        {
            Canvas canvas = Area.GetAdorningCanvas();
            double left = Area.ActualWidth - canvas.ActualWidth;
            double top = Area.ActualHeight - canvas.ActualHeight;
            ChartDataPointInfo data = null;
            point.X = point.X - left + Area.Margin.Left;
            point.Y = point.Y - top + Area.Margin.Top;

            data = GetDataPoint(point);

            if (data != null)
                return data.Index;
            else
                return -1;
        }

        /// <summary>
        /// This method used to generate bitmap segment pixels.
        /// </summary>
        internal virtual void GeneratePixels()
        {
        }

        /// <summary>
        /// Method used to return the hittest series while mouse action.
        /// </summary>
        /// <returns></returns>
        internal virtual bool IsHitTestSeries()
        {
            if (!Area.isBitmapPixelsConverted)
                Area.ConvertBitmapPixels();

            if (Pixels.Contains(Area.currentBitmapPixel))
                return true;

            return false;
        }

        #endregion

        #region Internal Methods 

        /// <summary>
        /// Method is used to select/reset the bitmap segment.
        /// </summary>
        /// <param name="pixels"></param>
        /// <param name="brush"></param>
        /// <param name="isSelected"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal void OnBitmapSelection(List<int> pixels, Brush brush, bool isSelected)
        {
            if (pixels != null && pixels.Count > 0)
            {
                var seriesCollection = Area.GetChartSeriesCollection();
                int seriesIndex = seriesCollection.IndexOf(this);

                if (!Area.isBitmapPixelsConverted)
                    Area.ConvertBitmapPixels();

                // Gets the upper series from the selected series
                var upperSeriesCollection = (from series in seriesCollection
                                             where seriesCollection.IndexOf(series) > seriesIndex
                                             select series).ToList();

                // Gets the upper series pixels in to single collection
                foreach (var series in upperSeriesCollection)
                {
                    upperSeriesPixels.UnionWith(series.Pixels);
                }

                {
                    byte[] buffer = Area.GetFastBuffer();
                    int j = 0;
                    Color uiColor;

                    if (isSelected && brush != null)
                        uiColor = (brush as SolidColorBrush).Color;
                    else
                    {
                        if (this is FastHiLoOpenCloseBitmapSeries)
                        {
                            uiColor = ((Segments[0] as FastHiLoOpenCloseSegment).GetSegmentBrush(dataPoint.Index));
                        }
                        else if (this is FastCandleBitmapSeries)
                        {
                            uiColor = ((Segments[0] as FastCandleBitmapSegment).GetSegmentBrush(dataPoint.Index));
                        }
                        else
                        {
                            Brush interior = GetInteriorColor(dataPoint.Index);
                            var linearGradienBrush = interior as LinearGradientBrush;
                            uiColor = linearGradienBrush != null ? linearGradienBrush.GradientStops[0].Color : (interior as SolidColorBrush).Color;
                        }
                    }

                    foreach (var pixel in pixels)
                    {
                        if (Pixels.Contains(pixel) && !upperSeriesPixels.Contains(pixel))
                        {
                            if (j == 0)
                            {
                                buffer[pixel] = uiColor.B;
                                j = j + 1;
                            }
                            else if (j == 1)
                            {
                                buffer[pixel] = uiColor.G;
                                j = j + 1;
                            }
                            else if (j == 2)
                            {
                                buffer[pixel] = uiColor.R;
                                j = j + 1;
                            }
                            else if (j == 3)
                            {
                                buffer[pixel] = uiColor.A;
                                j = 0;
                            }
                        }
                    }

                    Area.RenderToBuffer();
                }

                upperSeriesPixels.Clear();
            }
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Invoke to render SfChart.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AdornmentPresenter = this.GetTemplateChild("SyncfusionChartDataMarkerPresenter") as ChartAdornmentPresenter;
            SeriesRootPanel = this.GetTemplateChild("SyncfusionChartSeriesRootPanel") as Panel;
            SeriesPanel = this.GetTemplateChild("SyncfusionChartSeriesPanel") as ChartSeriesPanel;
            SeriesPanel.Series = this;

            /* In case of stacking series, we position the corresponding adornments above series panel, 
              we have specifically checked for adornments position as bottom to show adornment label for next series*/
            var stackingSeriesBase = this as StackingSeriesBase;
            if (stackingSeriesBase != null)
            {
                if (this is FastStackingColumnBitmapSeries) // WPF-19742-We have specifically checked this condition for stacking bitmap series.
                    return;
                
                if (stackingSeriesBase.IsSideBySide)
                {
                    if (adornmentInfo != null)
                    {
                        AdornmentsPosition markerPosition = this.adornmentInfo.GetAdornmentPosition();
                        switch (markerPosition)
                        {
                            case AdornmentsPosition.Middle:
                            case AdornmentsPosition.Top:
                                Canvas.SetZIndex(SeriesPanel.Series, -ActualArea.GetSeriesIndex(this));
                                break;
                            default:
                                Canvas.SetZIndex(SeriesPanel.Series, ActualArea.GetSeriesIndex(this));
                                break;
                        }
                    }
                }
                else
                {
                    Canvas.SetZIndex(SeriesPanel.Series, -ActualArea.GetSeriesIndex(this));
                }
            }
        }

        /// <summary>
        /// Creates an instance of series segment.
        /// </summary>
        /// <example>
        /// This sample shows how to call the <see cref="CreateSegment"/> method to customize the line series segments.
        /// <code>
        /// public class LineSeriesExt : LineSeries
        /// {​ 
        ///     protected override ChartSegment CreateSegment()
        ///     {​
        ///         return new LineSegmentExt();
        ///     }​
        /// }​
        ///
        /// public class LineSegmentExt : LineSegment
        /// {​
        ///     public override UIElement CreateVisual(Size size)
        ///     {​
        ///         // Write your customization code here.
        ///     }​
        ///
        ///     public override void Update(IChartTransformer transformer)
        ///     {​
        ///         // Write your customization code here.
        ///     }​
        /// }
        /// </code>
        /// </example>
        /// <returns>
        /// Returns the instance of corresponding series segment.
        /// </returns>
        /// <remarks>
        /// This customization is not supported for fast type series and technical indicators.
        /// </remarks>
        protected abstract ChartSegment CreateSegment();

        #endregion

        #region Private Static Methods

        private static void OnAppearanceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var chartSeries = obj as ChartSeries;
            if (chartSeries != null)
                OnAppearanceChanged(chartSeries);
        }

        private static void OnAppearanceChanged(ChartSeries obj)
        {
            if (obj.IsBitmapSeries)
                obj.UpdateArea();
        }

        #endregion

        #endregion
            
    }
}
