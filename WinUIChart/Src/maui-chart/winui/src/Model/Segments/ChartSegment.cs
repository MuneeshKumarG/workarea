﻿using System;
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
    /// An abstract base class for all type of chart segments.
    /// </summary>
    /// <remarks>
    /// You can create a custom chart segment by inheriting from <see cref="ChartSegment"/>. You can also customize the appearance of a chart segment,
    /// by specifying values for <see cref="ChartSegment.Interior"/>,<see cref="ChartSegment.Stroke"/> and <see cref="ChartSegment.StrokeThickness"/> properties.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public abstract class ChartSegment : DependencyObject, INotifyPropertyChanged
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="Interior"/> property.
        /// </summary>
        public static readonly DependencyProperty InteriorProperty =
            DependencyProperty.Register("Interior", typeof(Brush), typeof(ChartSegment),
            new PropertyMetadata(null, OnInteriorChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="StrokeThickness"/> property.
        /// </summary>
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(ChartSegment),
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

        private bool isEmptySegmentInterior = false;

        private object item;

        private PointCollection polygonPoints;

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the x-value range for the segment.
        /// </summary>
        public DoubleRange XRange
        { get; set; }

        /// <summary>
        /// Gets or sets the y-value range for the segment.
        /// </summary>
        public DoubleRange YRange
        { get; set; }

        /// <summary>
        /// Gets or sets the data object that this segment belongs to.
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
        /// Gets or sets the brush to paint the interior of the segment.
        /// </summary>
        ///<remarks>
        ///By default,the interior value for a chart segment will be calculated and set automatically based on the <see cref="ChartSeriesBase.Palette"/> set.
        /// </remarks>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush Interior
        {
            get { return (Brush)GetValue(InteriorProperty); }
            set { SetValue(InteriorProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets the stroke thickness value of the segment.
        /// </summary>
        /// <remarks>
        /// By default, this property inherits its value from series StrokeThickness property.
        /// </remarks>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke dash array for the segment shape.
        /// </summary>
        /// <value>
        /// <see cref="DoubleCollection"/>.
        /// </value>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }
        
        ///<summary>
        /// Gets or sets the stroke value of the segment.
        ///</summary>
        /// <remarks>
        /// By default, this property inherits its value from series Stroke property.
        /// </remarks>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        
        /// <summary>
        /// Gets the value of underlying series of a chart segment.
        /// </summary>
        public ChartSeriesBase Series
        {
            get;
            protected internal set;
        }

        #endregion

        #region Internal Properties

        internal bool IsSelectedSegment
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the point collection.
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

        #region Protected Internal Properties

        /// <summary>
        /// Gets or sets IsEmptySegmentinterior property
        /// </summary>
        protected internal bool IsEmptySegmentInterior
        {
            get
            {
                return isEmptySegmentInterior;
            }
            internal set
            {
                if (isEmptySegmentInterior != value)
                {
                    isEmptySegmentInterior = value;
                    if (Series == null)
                        return;
                    BindProperties();
                }
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Abstract Methods

        /// <summary>
        /// Used for creating UIElement for rendering this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="size">Size of the panel</param>
        /// <returns>
        /// returns UIElement
        /// </returns>

        public abstract UIElement CreateVisual(Size size);

        /// <summary>
        /// Gets the UIElement used for rendering this segment.
        /// </summary>
        /// <returns>returns UIElement</returns>

        public abstract UIElement GetRenderedVisual();

        /// <summary>
        /// Updates the segments based on its data point value. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="transformer">Reresents the view port of chart control.(refer <see cref="IChartTransformer"/>)</param>

        public abstract void Update(IChartTransformer transformer);

        /// <summary>
        /// Called whenever the segment's size changed. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="size">Size of the panel.</param>

        public abstract void OnSizeChanged(Size size);

        #endregion

        #region Public Virtual Methods

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="xVals">Used to specify the segment x values.</param>
        /// <param name="yVals">Used to specify the segment y values.</param> 
        public virtual void SetData(IList<double> xVals, IList<double> yVals)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the 3DChart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="xValues"></param>
        /// <param name="yValues"></param>
        /// <param name="startDepth"></param>
        /// <param name="endDepth"></param>
        public virtual void SetData(List<double> xValues, IList<double> yValues, double startDepth, double endDepth)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="xVals"></param>
        /// <param name="yVals"></param>
        /// <param name="strokeBrush"></param>
        public virtual void SetData(IList<double> xVals, IList<double> yVals, Brush strokeBrush)
        {
        }

        /// <summary>
        /// Sets the values for Technical indicator segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="xVals">Used to specify the segment x-values</param>
        /// <param name="yVals">Used to specify the segment y-values</param>
        /// <param name="strokeBrush">Used to specify the segment stroke brush</param>
        /// <param name="length">Used to specify the segment range</param>
        public virtual void SetData(IList<double> xVals, IList<double> yVals, Brush strokeBrush, int length)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="Values">Used to specify the segment values.</param>
        public virtual void SetData(params double[] Values)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="AreaPoints"></param>
        [Obsolete("Use SetData(List<ChartPoint> AreaPoints)")]
        public virtual void SetData(List<Point> AreaPoints)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="AreaPoints"></param>
        public virtual void SetData(List<ChartPoint> AreaPoints)
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
        public virtual void SetData(Point point1, Point point2, Point point3, Point point4)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not intended to be called explicitly outside the Chart but it can be overridden by any derived class.
        /// </summary>
        public virtual void SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="xValues"></param>
        /// <param name="yHiValues"></param>
        /// <param name="yLowValues"></param>
        public virtual void SetData(IList<double> xValues, IList<double> yHiValues, IList<double> yLowValues)
        {

        }


        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="xValues"></param>
        /// <param name="yHiValues"></param>
        /// <param name="yLowValues"></param>
        /// /// <param name="yOpenValues"></param>
        /// /// <param name="yCloseValues"></param>
        public virtual void SetData(IList<double> xValues, IList<double> yHiValues, IList<double> yLowValues, IList<double> yOpenValues, IList<double> yCloseValues)
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
        public virtual void SetData(IList<double> x1Values, IList<double> y1Values, IList<double> x2Values, IList<double> y2Values)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="leftpoint"></param>
        /// <param name="rightpoint"></param>
        /// <param name="toppoint"></param>
        /// <param name="bottompoint"></param>
        /// <param name="vercappoint"></param>
        /// <param name="horcappoint"></param>
        public virtual void SetData(Point leftpoint, Point rightpoint, Point toppoint, Point bottompoint, Point vercappoint, Point horcappoint)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="BottomLeft"></param>
        /// <param name="RightTop"></param>
        /// <param name="hipoint"></param>
        /// <param name="loPoint"></param>
        /// <param name="isBull"></param>
        [Obsolete("Use SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3,ChartPoint point4,bool isBull)")]
        public virtual void SetData(Point BottomLeft, Point RightTop, Point hipoint, Point loPoint, bool isBull)
        {
        }

        /// <summary>
        /// Sets the values for this segment. This method is not intended to be called explicitly outside the Chart but it can be overridden by any derived class.
        /// </summary>
        public virtual void SetData(ChartPoint BottomLeft, ChartPoint RightTop, ChartPoint hipoint, ChartPoint loPoint, bool isBull)
        {
        }

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="hipoint"></param>
        /// <param name="lopoint"></param>
        /// <param name="sopoint"></param>
        /// <param name="eopoint"></param>
        /// <param name="scpoint"></param>
        /// <param name="ecpoint"></param>
        /// <param name="isBull"></param>
        [Obsolete("Use SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4, ChartPoint point5, ChartPoint point6, bool isbull)")]
        public virtual void SetData(Point hipoint, Point lopoint, Point sopoint, Point eopoint, Point scpoint, Point ecpoint, bool isBull)
        {

        }

        /// <summary>
        /// Sets the values for this segment. This method is not intended to be called explicitly outside the Chart but it can be overridden by any derived class.
        /// </summary>
        public virtual void SetData(ChartPoint hipoint, ChartPoint lopoint, ChartPoint sopoint, ChartPoint eopoint, ChartPoint scpoint, ChartPoint ecpoint, bool isBull)
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

            //This is to prevent the outlier segment from getting bind with the properties of the series. 
            //It must bind with the properties of the segment.
            if ((this is ScatterSegment && this.Series is BoxAndWhiskerSeries)) return;

            var isGrouping = (Series.ActualXAxis is CategoryAxis) ? (Series.ActualXAxis as CategoryAxis).IsIndexed : true;

            if (Series.ActualArea.GetEnableSeriesSelection() && Series.ActualArea.SelectedSeriesCollection.Contains(Series))
            {
                Binding binding = new Binding();
                binding.Source = Series.ActualArea.SelectionBehaviour;
                binding.Path = new PropertyPath(nameof(Series.ActualArea.SelectionBehaviour.SeriesSelectionBrush));
                binding.Converter = new SeriesSelectionBrushConverter(Series);
                binding.ConverterParameter = Series is AccumulationSeriesBase && !isGroupTo ? Series.ActualData.IndexOf(Item) : Series.Segments.IndexOf(this);
                BindingOperations.SetBinding(this, ChartSegment.InteriorProperty, binding);

            }
            else if (Series.ActualArea.GetEnableSegmentSelection() && IsSegmentSelected())
            {
                Binding binding = new Binding();
                binding.Source = Series;
                binding.Path = new PropertyPath("SelectionBrush");
                binding.Converter = new SegmentSelectionBrushConverter(Series);
                if (!isGrouping && (Series.IsSideBySide && (!(Series is RangeSeriesBase)) && (!(Series is FinancialSeriesBase)) && !(Series is WaterfallSeries)))
                    binding.ConverterParameter = Series.GroupedActualData.IndexOf(Item);
                else
                    binding.ConverterParameter = Series is AccumulationSeriesBase && !isGroupTo ? Series.ActualData.IndexOf(Item) : Series.Segments.IndexOf(this);
                BindingOperations.SetBinding(this, ChartSegment.InteriorProperty, binding);
            }
            else if (this is WaterfallSegment)
                BindWaterfallSegmentInterior(this as WaterfallSegment);
            else if (!this.IsEmptySegmentInterior)
            {
                Binding binding = new Binding();
                binding.Source = Series;
                binding.Path = new PropertyPath("Interior");
                binding.Converter = new InteriorConverter(Series);
                if (!isGrouping && (Series.IsSideBySide && (!(Series is RangeSeriesBase)) && (!(Series is FinancialSeriesBase))))
                    binding.ConverterParameter = Series.GroupedActualData.IndexOf(Item);
                else
                    binding.ConverterParameter = Series is AccumulationSeriesBase && !isGroupTo ? Series.ActualData.IndexOf(Item) : Series.Segments.IndexOf(this);
                BindingOperations.SetBinding(this, ChartSegment.InteriorProperty, binding);
            }
            else
            {
                Binding binding = new Binding();
                binding.Source = Series;
                binding.ConverterParameter = Series.Interior;
                binding.Path = new PropertyPath("EmptyPointInterior");
                binding.Converter = new MultiInteriorConverter();
                BindingOperations.SetBinding(this, ChartSegment.InteriorProperty, binding);
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
                binding2.Path = new PropertyPath("StrokeThickness");
                BindingOperations.SetBinding(this, ChartSegment.StrokeThicknessProperty, binding2);
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
        /// Method used to update the segment's interior color based on WaterfallSegmentType. 
        /// </summary>
        /// <param name="segment"></param>
        internal void BindWaterfallSegmentInterior(WaterfallSegment segment)
        {
            Binding binding = new Binding();
            binding.Source = segment.Series;
            if (segment.SegmentType == WaterfallSegmentType.Negative && (segment.Series as WaterfallSeries).NegativeSegmentBrush != null)
            {
                binding.Path = new PropertyPath("NegativeSegmentBrush");
                BindingOperations.SetBinding(this, ChartSegment.InteriorProperty, binding);
            }
            else if (segment.SegmentType == WaterfallSegmentType.Sum && (segment.Series as WaterfallSeries).SummarySegmentBrush != null)
            {
                binding.Path = new PropertyPath("SummarySegmentBrush");
                BindingOperations.SetBinding(this, ChartSegment.InteriorProperty, binding);
            }
            else
            {
                binding.Path = new PropertyPath("Interior");
                binding.Converter = new InteriorConverter(segment.Series);
                binding.ConverterParameter = segment.Series.Segments.IndexOf(segment);
                BindingOperations.SetBinding(this, ChartSegment.InteriorProperty, binding);
            }
        }

        /// <summary>
        /// Method used to check the current segment is selected index segment or not
        /// </summary>
        /// <returns></returns>
        internal bool IsSegmentSelected()
        {
            int index = -1;
            var isGrouping = (Series.ActualXAxis is CategoryAxis) ? (Series.ActualXAxis as CategoryAxis).IsIndexed : true;

            if (Series is CircularSeries && !double.IsNaN(((CircularSeries)Series).GroupTo))
                index = Series.Segments.IndexOf(this);
            else if (!isGrouping && (Series.IsSideBySide && (!(Series is RangeSeriesBase)) && (!(Series is FinancialSeriesBase))
                && !(Series is WaterfallSeries)))
                index = Series.GroupedActualData.IndexOf(Item);
            else
                index = Series.ActualData.IndexOf(Item);

            return (Series is ISegmentSelectable && !Series.IsBitmapSeries
                && Series.SelectedSegmentsIndexes.Contains(index) &&
                (Series.IsSideBySide || Series is AccumulationSeriesBase || Series is BubbleSeries || Series is ScatterSeries));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal static NativeColor GetColor(Brush brush)
        {            
            return brush is SolidColorBrush
                            ? ((SolidColorBrush)brush).Color
                            : brush is LinearGradientBrush && (brush as LinearGradientBrush).GradientStops.Count > 0
                                ? (brush as LinearGradientBrush).GradientStops[0].Color
                                : new SolidColorBrush(Colors.Transparent).Color;
        }

        internal virtual UIElement CreateSegmentVisual(Size size)
        {
            BindProperties();
            return CreateVisual(size);
        }

        /// <summary>
        /// Method used to check corresponding segment is selectable or not.
        /// </summary>
        /// <param name="segmentSelectionBrush"></param>
        /// <returns></returns>
        internal bool GetEnableSegmentSelection(Brush segmentSelectionBrush)
        {
            return (segmentSelectionBrush != null
                    && Series.ActualArea.GetEnableSegmentSelection());
        }

        #endregion

        #region Protected Internal Methods

        /// <summary>
        /// This method is used to change High and Low values,
        /// When the High value lesser than Open value or Low value greater than Close value.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        protected internal double[] AlignHiLoSegment(double openValues, double closeValues, double highValues, double lowValues)
        {
            double[] values = new double[2];
            // High value lesser than Open value
            if (highValues < openValues)
            {
                // Compare the High value with Open,Close and Low value.
                var tempHighValues = highValues;
                if (highValues < lowValues)
                {
                    highValues = lowValues;
                    lowValues = tempHighValues;
                }
                if (highValues < openValues)
                {
                    highValues = openValues;
                }
                if (highValues < closeValues)
                {
                    highValues = closeValues;
                }
            }
            // Low value greater than Close value
            if (lowValues > closeValues)
            {
                // Compare the Low value with Open,Close and High value
                var tempHighValues = highValues;
                if (lowValues > highValues)
                {
                    highValues = lowValues;
                    lowValues = tempHighValues;
                }
                if (lowValues > closeValues)
                {
                    lowValues = closeValues;
                }

                if (lowValues > openValues)
                {
                    lowValues = openValues;
                }
            }
            values[0] = highValues;
            values[1] = lowValues;
            return values;
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Called when Property changed 
        /// </summary>
        /// <param name="name"></param>
        protected virtual void OnPropertyChanged(string name)
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
        protected virtual void SetVisualBindings(Shape element)
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Interior");
            element.SetBinding(Shape.FillProperty, binding);
            element.SetBinding(Shape.StrokeProperty, binding);
            binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("StrokeThickness");
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
            (d as ChartSegment).OnPropertyChanged("Interior");
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
            (d as ChartSegment).OnPropertyChanged("Stroke");
        }

        private static void OnStrokeThicknessChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var segment = (d as ChartSegment);
            segment.OnPropertyChanged("StrokeThickness");

        }

#endregion

#endregion
    }

}
