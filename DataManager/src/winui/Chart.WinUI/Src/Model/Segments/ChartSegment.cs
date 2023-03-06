using System;
using System.Collections.Generic;
#if !WinUI_UWP
using System.ComponentModel;
#endif
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using NativeColor = Windows.UI.Color;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public abstract class ChartSegment : DependencyObject, INotifyPropertyChanged
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="Fill"/> property.
        /// </summary>
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(ChartSegment),
            new PropertyMetadata(null, OnInteriorChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="StrokeWidth"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeWidthProperty =
            DependencyProperty.Register("StrokeWidth", typeof(double), typeof(ChartSegment),
            new PropertyMetadata(1d, OnStrokeThicknessChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="StrokeDashArray"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register("StrokeDashArray", typeof(DoubleCollection), typeof(ChartSegment),
            new PropertyMetadata(null, OnStrokeDashArrayChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke", typeof(Brush), typeof(ChartSegment),
            new PropertyMetadata(null, OnStrokeValueChanged));

        #endregion

        #region Fields

        #region Internal Fields

        internal bool IsAddedToVisualTree = false;

        internal bool IsSegmentVisible = true;

        internal bool x_isInversed, y_isInversed;

        internal Rect rect;

        #endregion

        #region Private Fields

        private object item;

        private PointCollection? polygonPoints;

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        internal DoubleRange XRange
        { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal DoubleRange YRange
        { get; set; }

        /// <summary>
        /// Gets or sets the data object for the segment.
        /// </summary>

        public object Item
        {
            get
            {
                return item;
            }
            set
            {
                item = value;
                OnPropertyChanged("Item");
            }
        }

        /// <summary>
        /// Gets or sets a brush value to customize the background appearance of the segment.
        /// </summary>
        /// <value>It accepts a <see cref="Brush"/> value and its default value is null.</value>
        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets a value to specify the stroke thickness of the chart segment.
        /// </summary>
        /// <value>It accepts double values and its default value is 1.</value>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke dash array to customize the appearance of stroke.
        /// </summary>
        /// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        /// <summary>
        /// Gets or sets a brush value to customize the border appearance of the segment.
        /// </summary>
        /// <value>It accepts a <see cref="Brush"/> value and its default value is null.</value>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets the series associated with the segment.
        /// </summary>
        public ChartSeries Series
        {
            get;
            internal set;
        }

        #endregion

        #region Internal Properties

        internal bool IsSelectedSegment
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the collection of points that define a polygon shape.
        /// </summary>
        public PointCollection PolygonPoints
        {
            get { return polygonPoints; }
            set
            {
                polygonPoints = value;
                OnPropertyChanged("PolygonPoints");
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Abstract Methods

        /// <summary>
        /// Used for creating UIElement for rendering this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="size">Size of the panel</param>
        /// <returns>
        /// returns UIElement
        /// </returns>

        internal abstract UIElement CreateVisual(Size size);

        /// <summary>
        /// Gets the UIElement used for rendering this segment.
        /// </summary>
        /// <returns>returns UIElement</returns>

        internal abstract UIElement GetRenderedVisual();

        /// <summary>
        /// Updates the segments based on its data point value. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="transformer">Reresents the view port of chart control.(refer <see cref="IChartTransformer"/>)</param>

        internal abstract void Update(IChartTransformer transformer);

        /// <summary>
        /// Called whenever the segment's size changed. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="size">Size of the panel.</param>

        internal abstract void OnSizeChanged(Size size);

        #endregion

        #region Internal Virtual Methods

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="xVals">Used to specify the segment x values.</param>
        /// <param name="yVals">Used to specify the segment y values.</param> 
        internal virtual void SetData(IList<double> xVals, IList<double> yVals)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="Values">Used to specify the segment values.</param>
        internal virtual void SetData(params double[] Values)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="AreaPoints"></param>
        [Obsolete("Use SetData(List<ChartPoint> AreaPoints)")]
        internal virtual void SetData(List<Point> AreaPoints)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="AreaPoints"></param>
        internal virtual void SetData(List<ChartPoint> AreaPoints)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="point1"></param>
        /// <param name="point2"></param>
        /// <param name="point3"></param>
        /// <param name="point4"></param>
        [Obsolete("Use SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4)")]
        internal virtual void SetData(Point point1, Point point2, Point point3, Point point4)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not intended to be called explicitly outside the Chart but it can be overridden by any derived class.
        /// </summary>
        internal virtual void SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="x1Values">Used to specify the segment x1 values</param>
        /// <param name="y1Values">Used to specify the segment y1 values</param>
        /// <param name="x2Values">Used to specify the segment x2 values</param>
        /// <param name="y2Values">Used to specify the segment y2 values</param>
        internal virtual void SetData(IList<double> x1Values, IList<double> y1Values, IList<double> x2Values, IList<double> y2Values)
        {

        }

        #endregion

        #region Internal Methods

        internal virtual void Dispose()
        {
            if (PropertyChanged != null)
            {
                foreach (var handler in PropertyChanged.GetInvocationList())
                {
                    PropertyChanged -= handler as PropertyChangedEventHandler;
                }

                PropertyChanged = null;
            }

            if (PolygonPoints != null)
            {
                PolygonPoints.Clear();
                PolygonPoints = null;
            }

            Item = null;
            Series = null;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal void BindProperties()
        {
            var circularSeries = Series as CircularSeries;
            bool isGroupTo = circularSeries != null && !double.IsNaN(circularSeries.GroupTo);

            var isGrouping = (Series.ActualXAxis is CategoryAxis) ? (Series.ActualXAxis as CategoryAxis).IsIndexed : true;

            if (Series.ActualArea.GetEnableSeriesSelection() && Series.ActualArea.SelectedSeriesCollection.Contains(Series))
            {
                var selection = Series.ActualArea.GetSeriesSelectionBehavior();
                Binding binding = new Binding();
                binding.Source = selection;
                binding.Path = new PropertyPath(nameof(selection.SelectionBrush));
                binding.Converter = new SeriesSelectionBrushConverter(Series);
                binding.ConverterParameter = Series.Segments.IndexOf(this);
                BindingOperations.SetBinding(this, ChartSegment.FillProperty, binding);

            }
            else if (Series.GetEnableSegmentSelection() && IsSegmentSelected())
            {
                Binding binding = new Binding();
                binding.Source = Series.SelectionBehavior;
                binding.Path = new PropertyPath("SelectionBrush");
                binding.Converter = new SegmentSelectionBrushConverter(Series);
                if (!isGrouping && Series.IsSideBySide)
                    binding.ConverterParameter = Series.GroupedActualData.IndexOf(Item);
                else
                    binding.ConverterParameter = (Series is CircularSeries || Series is TriangularSeriesBase) && !isGroupTo ? Series.ActualData.IndexOf(Item) : Series.Segments.IndexOf(this);
                
                BindingOperations.SetBinding(this, ChartSegment.FillProperty, binding);
            }
            else
            {
                Binding binding = new Binding();
                binding.Source = Series;
                binding.Path = new PropertyPath("Fill");
                binding.Converter = new InteriorConverter(Series);
                if (!isGrouping && Series.IsSideBySide)
                    binding.ConverterParameter = Series.GroupedActualData.IndexOf(Item);
                else
                    binding.ConverterParameter = (Series is CircularSeries || Series is TriangularSeriesBase) && !isGroupTo ? Series.ActualData.IndexOf(Item) : Series.Segments.IndexOf(this);
                BindingOperations.SetBinding(this, ChartSegment.FillProperty, binding);
            }

            if (Series is ChartSeries)
            {
                Binding binding = new Binding();
                binding.Source = Series;
                binding.Path = new PropertyPath("Stroke");
                BindingOperations.SetBinding(this, ChartSegment.StrokeProperty, binding);
            }
            if (Series is ChartSeries)
            {
                Binding binding2 = new Binding();
                binding2.Source = Series;
                binding2.Path = new PropertyPath("StrokeWidth");
                BindingOperations.SetBinding(this, StrokeWidthProperty, binding2);
            }

            var doughnutSeries = Series as DoughnutSeries;
            if(doughnutSeries != null)
            {
                Binding binding = new Binding();
                binding.Source = Series;
                binding.Path = new PropertyPath("TrackBorderColor");
                BindingOperations.SetBinding(this, DoughnutSegment.TrackBorderColorProperty, binding);

                binding = new Binding();
                binding.Source = Series;
                binding.Path = new PropertyPath("TrackBorderWidth");
                BindingOperations.SetBinding(this, DoughnutSegment.TrackBorderWidthProperty, binding);
            }
        }
        
        /// <summary>
        /// Method used to check the current segment is selected index segment or not
        /// </summary>
        /// <returns></returns>
        internal bool IsSegmentSelected()
        {
            int index = -1;
            var isGrouping = (Series.ActualXAxis is CategoryAxis categoryAxis) ? categoryAxis.IsIndexed : true;

            if (Series is CircularSeries && !double.IsNaN(((CircularSeries)Series).GroupTo))
                index = Series.Segments.IndexOf(this);
            else if (!isGrouping && Series.IsSideBySide)
                index = Series.GroupedActualData.IndexOf(Item);
            else
                index = Series.ActualData.IndexOf(Item);

            return (!Series.IsBitmapSeries
                && Series.SelectedSegmentsIndexes.Contains(index) &&
                (Series.IsSideBySide || Series is TriangularSeriesBase || Series is CircularSeries || Series is BubbleSeries || Series is ScatterSeries));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal static NativeColor GetColor(Brush brush)
        {            
            return brush is SolidColorBrush
                            ? ((SolidColorBrush)brush).Color
                            : brush is LinearGradientBrush linearGradientBrush && linearGradientBrush.GradientStops.Count > 0
                                ? linearGradientBrush.GradientStops[0].Color
                                : new SolidColorBrush(Colors.Transparent).Color;
        }

        internal virtual UIElement CreateSegmentVisual(Size size)
        {
            BindProperties();
            return CreateVisual(size);
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Called when Property changed 
        /// </summary>
        /// <param name="name"></param>
        internal virtual void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Method Implementation for set binding to ChartSegments properties.
        /// </summary>
        /// <param name="element"><see cref="Shape"/> element to be bind.</param>
        internal virtual void SetVisualBindings(Shape element)
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Fill");
            element.SetBinding(Shape.FillProperty, binding);
            element.SetBinding(Shape.StrokeProperty, binding);
            binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("StrokeWidth");
            element.SetBinding(Shape.StrokeThicknessProperty, binding);
            DoubleCollection collection = this.StrokeDashArray;
            if (collection != null && collection.Count > 0)
            {
                DoubleCollection doubleCollection = new DoubleCollection();
                foreach (double value in collection)
                {
                    doubleCollection.Add(value);
                }
                element.StrokeDashArray = doubleCollection;
            }
        }

        #endregion

        #region Private Static Methods

        private static void OnInteriorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSegment)?.OnPropertyChanged("Fill");
        }

        private static void OnStrokeDashArrayChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as ChartSegment;
            if (instance != null)
            {
                var collection = e.NewValue as DoubleCollection;
                var shape = instance.GetRenderedVisual() as Shape;
                if (shape != null)
                    shape.StrokeDashArray = collection == null ? null : GetDoubleCollection(collection);
            }
        }

        private static DoubleCollection GetDoubleCollection(DoubleCollection collection)
        {
            var doubleCollection = new DoubleCollection();
            foreach (double value in collection)
            {
                doubleCollection.Add(value);
            }

            return doubleCollection;
        }

        private static void OnStrokeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ChartSegment)?.OnPropertyChanged("Stroke");
        }

        private static void OnStrokeThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var segment = (d as ChartSegment);
            segment?.OnPropertyChanged("StrokeWidth");

        }

#endregion

#endregion
    }

}
