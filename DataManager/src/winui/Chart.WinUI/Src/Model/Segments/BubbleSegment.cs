using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class BubbleSegment:ChartSegment
    {
        #region Fields

        private double radius, size;

        private Ellipse? ellipseSegment;

        private double xPos, yPos;

        private ContentControl control;

        private DataTemplate customTemplate;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public BubbleSegment()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xPos"></param>
        /// <param name="yPos"></param>
        /// <param name="size"></param>
        /// <param name="series"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        public BubbleSegment(double xPos, double yPos, double size, BubbleSeries series)
        {
            this.radius = size;
            this.customTemplate = series.CustomTemplate;
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the x-value for the bubble segment.
        /// </summary>
        public double XData
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the y-value for the bubble segment.
        /// </summary>
        public double YData
        {

            get;
            internal set;
        }

        /// <summary>
        /// Gets the size value of the bubble segment.
        /// </summary>
        public double Size
        {
            get { return size; }
            internal set
            {
                size = value;
                OnPropertyChanged("Size");
            }
        }

        /// <summary>
        /// Gets the radius value of the bubble segment.
        /// </summary>
        public double Radius
        {
            get
            {
                return radius;
            }
            internal set
            {
                radius = value;
                OnPropertyChanged("SegmentRadius");
            }
        }

        /// <summary>
        /// Gets the x position of the segment rect.
        /// </summary>
        public double RectX
        {
            get { return xPos; }
            internal set
            {
                xPos = value;
                OnPropertyChanged("RectX");
            }
        }

        /// <summary>
        /// Gets the y position of the segment rect.
        /// </summary>
        public double RectY
        {
            get { return yPos; }
            internal set
            {
                yPos = value;
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
            XData = Values[0];
            YData = Values[1];
            this.xPos = Values[0];
            this.yPos = Values[1];
            XRange = new DoubleRange(xPos, xPos);
            YRange = new DoubleRange(yPos, yPos);

            var actualBubbleSeries = Series as BubbleSeries;
            if (actualBubbleSeries != null)
                customTemplate = actualBubbleSeries.CustomTemplate;
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
                ellipseSegment = new Ellipse();
                SetVisualBindings(ellipseSegment);
                ellipseSegment.Tag = this;
                return ellipseSegment;
            }
            control = new ContentControl { Content = this, Tag = this, ContentTemplate = customTemplate };
            return control;
        }

        /// <summary>
        /// Gets the UIElement used for rendering this segment.
        /// </summary>
        /// <returns>reurns UIElement</returns>

        internal override UIElement GetRenderedVisual()
        {
            if (customTemplate == null && ellipseSegment != null)
                return ellipseSegment;
            return control;
        }

        /// <summary>
        /// Updates the segments based on its data point value. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="transformer">Reresents the view port of chart control.(refer <see cref="IChartTransformer"/>)</param>

        internal override void Update(IChartTransformer transformer)
        {
            if (transformer is ChartTransform.ChartCartesianTransformer cartesianTransformer)
            {
                double xStart = cartesianTransformer.XAxis.VisibleRange.Start;
                double xEnd = cartesianTransformer.XAxis.VisibleRange.End;

                if (xPos >= xStart && xPos <= xEnd && (!double.IsNaN(yPos)))
                {
                    Point point1 = transformer.TransformToVisible(xPos, yPos);

                    if (ellipseSegment != null)
                    {
                        ellipseSegment.Visibility = Visibility.Visible;
                        ellipseSegment.Height = ellipseSegment.Width = 2 * this.radius;
                        ellipseSegment.SetValue(Canvas.LeftProperty, point1.X - this.radius);
                        ellipseSegment.SetValue(Canvas.TopProperty, point1.Y - this.radius);
                    }
                    else
                    {
                        control.Visibility = Visibility.Visible;
                        RectX = point1.X - this.radius;
                        RectY = point1.Y - this.radius;
                        Size = Radius = 2 * this.radius;
                    }
                }
                else if (customTemplate == null && ellipseSegment != null)
                    ellipseSegment.Visibility = Visibility.Collapsed;
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
        /// Method implementation for Set Binding to visual elements. 
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
            if(ellipseSegment != null)
            {
                ellipseSegment.Tag = null;
                ellipseSegment = null;
            }
            base.Dispose();
        }

        #endregion
    }
}
