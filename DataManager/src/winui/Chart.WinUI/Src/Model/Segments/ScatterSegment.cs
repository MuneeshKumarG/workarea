using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using Windows.UI;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>

    public class ScatterSegment : ChartSegment
    {
        #region Fields
        
        #region Internal Fields

        internal DataTemplate CustomTemplate;

        #endregion

        #region Protected Fields

        /// <summary>
        /// EllipseSegment property declarations
        /// </summary>
        internal Ellipse EllipseSegment;

        #endregion

        #region Private Fields

        private double yData, xData;

        private double pointWidth;

        private double pointHeight;

        private double xPos, yPos;

        private ContentControl control;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ScatterSegment()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xpos"></param>
        /// <param name="ypos"></param>
        /// <param name="series"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        public ScatterSegment(double xpos, double ypos, ScatterSeries series)
        {
            CustomTemplate = series.CustomTemplate;
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
        /// Gets or sets a value to customize the width of the scatter segment.
        /// </summary>
        public double PointWidth
        {
            get { return pointWidth; }
            set
            {
                if (pointWidth != value)
                {
                    pointWidth = value;
                    OnPropertyChanged("PointWidth");
                }
            }
        }


        /// <summary>
        /// Gets or sets a value to customize the height of the scatter segment.
        /// </summary>
        public double PointHeight
        {
            get { return pointHeight; }
            set
            {
                if (pointHeight != value)
                {
                    pointHeight = value;
                    OnPropertyChanged("PointHeight");
                }
            }
        }


        /// <summary>
        /// Gets the x position of the segment rect.
        /// </summary>
        public double RectX
        {
            get { return xPos; }
            set
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
            set
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
            xPos = Values[0];
            yPos = Values[1];
            if (!double.IsNaN(xPos))
                XRange = DoubleRange.Union(xPos);
            else
                XRange = DoubleRange.Empty;
            if (!double.IsNaN(yPos))
                YRange = DoubleRange.Union(yPos);
            else
                YRange = DoubleRange.Empty;
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
            if (CustomTemplate == null)
            {
                EllipseSegment = new Ellipse();
                Binding binding = new Binding();
                binding.Source = this;
                binding.Path = new PropertyPath("PointWidth");
                EllipseSegment.Tag = this;
                EllipseSegment.SetBinding(Ellipse.WidthProperty, binding);
                binding = new Binding();
                binding.Source = this;
                binding.Path = new PropertyPath("PointHeight");
                EllipseSegment.SetBinding(Ellipse.HeightProperty, binding);
                EllipseSegment.Tag = this;
                SetVisualBindings(EllipseSegment);
                return EllipseSegment;
            }
            control = new ContentControl { Content = this, Tag = this, ContentTemplate = CustomTemplate };
            return control;
        }

        /// <summary>
        /// Gets the UIElement used for rendering this segment.
        /// </summary>
        /// <returns>reurns UIElement</returns>

        internal override UIElement GetRenderedVisual()
        {
            if (CustomTemplate == null)
                return EllipseSegment;
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
            ChartTransform.ChartCartesianTransformer cartesianTransformer =
                transformer as ChartTransform.ChartCartesianTransformer;
            if (cartesianTransformer != null)
            {
               
                double xStart = cartesianTransformer.XAxis.VisibleRange.Start;
                double xEnd = cartesianTransformer.XAxis.VisibleRange.End;
                double edgeValue = xPos;
                if (edgeValue >= xStart && edgeValue <= xEnd && (!double.IsNaN(YData)))
                {
                    Point point1 = transformer.TransformToVisible(xPos, yPos);
                    // To prevent the issues when scattersegment is used in other series.
                    var series = (Series as ScatterSeries);
                    if (series != null)
                    {
                        PointHeight = series.PointHeight;
                        PointWidth = series.PointWidth;
                    }

                    if (EllipseSegment != null)
                    {
                        EllipseSegment.SetValue(Canvas.LeftProperty, point1.X - PointWidth / 2);
                        EllipseSegment.SetValue(Canvas.TopProperty, point1.Y - PointHeight / 2);
                    }
                    else
                    {
                        control.Visibility = Visibility.Visible;
                        RectX = point1.X - (PointWidth / 2);
                        RectY = point1.Y - (PointHeight / 2);
                    }
                }
                else if (CustomTemplate == null)
                {
                    PointHeight = 0;
                    PointWidth = 0;
                }
                else
                    control.Visibility = Visibility.Collapsed;
            }
            else
            {
                PointWidth = 0;
                PointHeight = 0;
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
        /// Method Implementation for set Binding to ChartSegments properties.
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
            if(EllipseSegment != null)
            {
                EllipseSegment.Tag = null;
                EllipseSegment = null;
            }
            if(control != null)
            {
                control.Tag = null;
                control = null;
            }
            base.Dispose();
        }

        #endregion
    }
}
