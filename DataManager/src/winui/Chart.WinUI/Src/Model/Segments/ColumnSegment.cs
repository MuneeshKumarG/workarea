using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>    
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class ColumnSegment : ChartSegment
    {
        #region Fields

        #region Internal Fields

        internal DataTemplate customTemplate;

        #endregion

        #region Protected Fields

        /// <summary>
        /// Variables declarations
        /// </summary>
        internal double Left = 0d, Top = 0d, Bottom = 0d, Right = 0d;

        /// <summary>
        /// RectSegment property declarations
        /// </summary>
        internal Rectangle? RectSegment;

        /// <summary>
        /// Set corresponding content control.
        /// </summary>
        private ContentControl control;

        #endregion

        #region Private Fields

        private double rectX, rectY, width, height;

        private double yData, xData;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ColumnSegment()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="series"></param>
        public ColumnSegment(double x1, double y1, double x2, double y2, ColumnSeries series)
        {
            base.Series = series;
            customTemplate = series.CustomTemplate;
            SetData(x1, y1, x2, y2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public ColumnSegment(double x1, double y1, double x2, double y2)
        {
            SetData(x1, y1, x2, y2);
        }
        
        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a data point value that is bound with x for the segment.
        /// </summary>
        public double XData
        {
            get
            {
                return xData;
            }
            internal set
            {
                xData = value;
                OnPropertyChanged("XData");
            }
        }

        /// <summary>
        /// Gets a data point value that is bound with y for the segment.
        /// </summary>
        public double YData
        {
            get
            {
                return yData;
            }
            internal set
            {
                yData = value;
                OnPropertyChanged("YData");
            }
        }

        /// <summary>
        /// Gets or sets a value to change the width of the <c>ColumnSegment</c>.
        /// </summary>
        public double Width
        {

            get
            {

                return width;
            }
            set
            {
                width = value;
                OnPropertyChanged("Width");
            }
        }

        /// <summary>
        /// Gets or sets a value to change the height of the <c>ColumnSegment</c>.
        /// </summary>
        public double Height
        {
            get
            {

                return height;
            }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        /// <summary>
        /// Gets or sets the x position of the segment rect.
        /// </summary>
        public double RectX
        {
            get
            {
                return rectX;
            }
            set
            {
                rectX = value;
                OnPropertyChanged("RectX");
            }
        }

        /// <summary>
        /// Gets or sets the y position of the segment rect.
        /// </summary>
        public double RectY
        {
            get
            {

                return rectY;
            }
            set
            {
                rectY = value;
                OnPropertyChanged("RectY");
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Sets the values for this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="Values"></param>
        internal override void SetData(params double[] Values)
        {
            Left = Values[0];
            Top = Values[1];
            Right = Values[2];
            Bottom = Values[3];
            XRange = new DoubleRange(Left, Right);
            YRange = new DoubleRange(Top, Bottom);
        }

        /// <summary>
        /// Used for creating UIElement for rendering this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="size">Size of the panel</param>
        /// <returns>
        /// retuns UIElement
        /// </returns>

        internal override UIElement CreateVisual(Size size)
        {
            if (customTemplate == null)
            {
                RectSegment = new Rectangle();
                SetVisualBindings(RectSegment);
                RectSegment.Tag = this;
                return RectSegment;
            }
            control = new ContentControl
            {
                Content = this,
                Tag = this,
                ContentTemplate = customTemplate
            };
            return control;
        }

        /// <summary>
        /// Gets the UIElement used for rendering this segment.
        /// </summary>
        /// <returns>reurns UIElement</returns>

        internal override UIElement GetRenderedVisual()
        {
            if (customTemplate == null && RectSegment != null)
                return RectSegment;
            return control;
        }

        /// <summary>
        /// Updates the segments based on its data point value. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="transformer">Represents the view port of chart control.(refer <see cref="IChartTransformer"/>)</param>

        internal override void Update(IChartTransformer transformer)
        {
            if (transformer is ChartTransform.ChartCartesianTransformer cartesianTransformer)
            {
                double xStart = Math.Floor(cartesianTransformer.XAxis.VisibleRange.Start);
                double xEnd = Math.Ceiling(cartesianTransformer.XAxis.VisibleRange.End);

                if (!double.IsNaN(Top) && !double.IsNaN(Bottom)
                    && (Left <= xEnd && Right >= xStart)
                    && (!double.IsNaN(YData)))
                {
                    if(Series is ISegmentSpacing segmentSpacing)
                        { 
                        double spacing = segmentSpacing.SegmentSpacing;
                        Point tlpoint = transformer.TransformToVisible(Left, Top);
                        Point rbpoint = transformer.TransformToVisible(Right, Bottom);
                        rect = new Rect(tlpoint, rbpoint);

                        if (spacing > 0.0 && spacing <= 1)
                        {
                            if (Series.IsActualTransposed == true)
                            {
                                double leftpos = segmentSpacing.CalculateSegmentSpacing(spacing,
                                    rect.Bottom, rect.Top);
                                rect.Y = leftpos;
                                Height = rect.Height = (1 - spacing) * rect.Height;
                            }

                            else
                            {
                                double leftpos = segmentSpacing.CalculateSegmentSpacing(spacing, rect.Right,
                                    rect.Left);
                                rect.X = leftpos;
                                Width = rect.Width = (1 - spacing) * rect.Width;
                            }
                        }
                    }
                    if (RectSegment != null)
                    {
                        RectSegment.SetValue(Canvas.LeftProperty, rect.X);
                        RectSegment.SetValue(Canvas.TopProperty, rect.Y);
                        RectSegment.Visibility = Visibility.Visible;
                        Width = RectSegment.Width = rect.Width;
                        Height = RectSegment.Height = rect.Height;
                    }
                    else
                    {
                        control.Visibility = Visibility.Visible;
                        RectX = rect.X;
                        RectY = rect.Y;
                        Width = rect.Width;
                        Height = rect.Height;
                    }
                }
                else if (customTemplate == null && RectSegment != null)
                    RectSegment.Visibility = Visibility.Collapsed;
                else control.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Called whenever the segment's size changed. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="size"></param>

        internal override void OnSizeChanged(Size size)
        {

        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Method implementation for Set Bindings to properties in ColumnSegment.
        /// </summary>
        /// <param name="element"></param>

        internal override void SetVisualBindings(Shape element)
        {
            base.SetVisualBindings(element);
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Stroke");
            element.SetBinding(Shape.StrokeProperty, binding);
        }

        #endregion

        internal override void Dispose()
        {
            if(RectSegment != null)
            {
                RectSegment.Tag = null;
                RectSegment = null;
            }
            base.Dispose();
        }

        #endregion
    }
}
