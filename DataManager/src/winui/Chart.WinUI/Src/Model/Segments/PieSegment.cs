using System;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using WindowsLineSegment = Microsoft.UI.Xaml.Media.LineSegment;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
  
    public class PieSegment : ChartSegment
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="IsExploded"/> property. 
        /// </summary>
        public static readonly DependencyProperty IsExplodedProperty =
            DependencyProperty.Register("IsExploded", typeof(bool), typeof(PieSegment),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsExplodedChanged)));

        /// <summary>
        ///  The DependencyProperty for <see cref="ActualStartAngle"/> property. 
        /// </summary>
        internal static readonly DependencyProperty ActualStartAngleProperty =
            DependencyProperty.Register("ActualStartAngle", typeof(double), typeof(PieSegment),
            new PropertyMetadata(0d, OnAngleChanged));

        /// <summary>
        ///  The DependencyProperty for <see cref="ActualEndAngle"/> property. 
        /// </summary>
        internal static readonly DependencyProperty ActualEndAngleProperty =
            DependencyProperty.Register("ActualEndAngle", typeof(double), typeof(PieSegment),
            new PropertyMetadata(0d, OnAngleChanged));

        #endregion

        #region Fields

        #region Private Fields

        private double startAngle;

        private PieSeries parentSeries;

        private Path segmentPath;

        internal Point startPoint;

        private bool isInitializing = true;

        private PathGeometry segmentGeometry;

        private int pieSeriesCount;

        private int pieIndex;

        private double angleOfSlice;

        private double xData;

        private double yData;

        private double endAngle;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public PieSegment()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arcStartAngle"></param>
        /// <param name="arcEndAngle"></param>
        /// <param name="series"></param>
        /// <param name="item"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        public PieSegment(double arcStartAngle, double arcEndAngle, PieSeries series, object item)
        {
        }

        internal void SetData(double arcStartAngle, double arcEndAngle, PieSeries pieSeries, object item)
        {
            base.Series = pieSeries;
            this.StartAngle = arcStartAngle;
            this.EndAngle = arcEndAngle;
            this.parentSeries = pieSeries;
            pieSeriesCount = pieSeries.GetPieSeriesCount();
            pieIndex = GetPieSeriesIndex(pieSeries);
            base.Item = item;
            isInitializing = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arcStartAngle"></param>
        /// <param name="arcEndAngle"></param>
        /// <param name="isEmptyInterior"></param>
        /// <param name="series"></param>
        /// <param name="item"></param>
        public PieSegment(double arcStartAngle, double arcEndAngle, bool isEmptyInterior, PieSeries series, object item)
            : this(arcStartAngle, arcEndAngle, series, item)
        {
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value that indicates whether this segment can be exploded or not.
        /// </summary>
        /// <value>It accepts boolean values and its default value is false.</value>
        public bool IsExploded
        {
            get { return (bool)GetValue(IsExplodedProperty); }
            set { SetValue(IsExplodedProperty, value); }
        }

        /// <summary>
        ///  Gets the start angle of this segment slice.
        /// </summary>
        public double StartAngle
        {
            get
            {
                return startAngle;
            }
            internal set
            {
                startAngle = value;
                if (Series != null && !Series.CanAnimate)
                    ActualStartAngle = value;
                OnPropertyChanged("StartAngle");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal double ActualStartAngle
        {
            get { return (double)GetValue(ActualStartAngleProperty); }
            set { SetValue(ActualStartAngleProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal double ActualEndAngle
        {
            get { return (double)GetValue(ActualEndAngleProperty); }
            set { SetValue(ActualEndAngleProperty, value); }
        }

        /// <summary>
        ///  Gets the end angle of this segment slice.
        /// </summary>
        public double EndAngle
        {
            get
            {
                return endAngle;
            }
            internal set
            {
                endAngle = value;
                if (Series != null && !Series.CanAnimate)
                    ActualEndAngle = value;
                OnPropertyChanged("EndAngle");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal double AngleOfSlice
        {
            get
            {
                return angleOfSlice;
            }
            set
            {
                angleOfSlice = value;
                OnPropertyChanged("AngleOfSlice");
            }
        }

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

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Used for creating UIElement for rendering this segment. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="size">Size of the panel</param>
        /// <returns>
        /// returns UIElement
        /// </returns>       
        internal override UIElement CreateVisual(Size size)
        {
            segmentPath = new Path();
            SetVisualBindings(segmentPath);
            segmentPath.Tag = this;
            return segmentPath;
        }

        /// <summary>
        /// Gets the UIElement used for rendering this segment.
        /// </summary>
        /// <returns>reurns UIElement</returns>       
        internal override UIElement GetRenderedVisual()
        {
            return segmentPath;
        }

        /// <summary>
        /// Updates the segments based on its data point value. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="transformer">Represents the view port of chart control.(refer <see cref="IChartTransformer"/>)</param>
        internal override void Update(IChartTransformer transformer)
        {
            if (!this.IsSegmentVisible)
                segmentPath.Visibility = Visibility.Collapsed;
            else
                segmentPath.Visibility = Visibility.Visible;

            segmentPath.StrokeLineJoin = PenLineJoin.Round;
            segmentPath.Width = transformer.Viewport.Width;
            segmentPath.Height = transformer.Viewport.Height;
            segmentPath.VerticalAlignment = VerticalAlignment.Center;
            segmentPath.HorizontalAlignment = HorizontalAlignment.Center;
            double actualRadius = Math.Min(transformer.Viewport.Width, transformer.Viewport.Height) / 2;
            double equalParts = actualRadius / (pieSeriesCount);

            if (pieIndex == 0)
            {
                Point center;
                if (pieSeriesCount == 1)
                    center = (Series as CircularSeries).Center;
                else
                    center = ChartLayoutUtils.GetCenter(transformer.Viewport);

                double radius = parentSeries.InternalPieCoefficient * (equalParts);
                parentSeries.CircularRadius = radius;
                if (Math.Round((ActualEndAngle - ActualStartAngle), 2) == 6.28)
                {
                    EllipseGeometry ellipseGeometry = new EllipseGeometry()
                    {
                        Center = center,
                        RadiusX = radius,
                        RadiusY = radius
                    };
                    this.segmentPath.Data = ellipseGeometry;
                }
                else if ((ActualEndAngle - ActualStartAngle) != 0)
                {
                    if (this.IsExploded)
                    {
                        center = new Point(center.X + (parentSeries.ExplodeRadius * Math.Cos(AngleOfSlice)), center.Y + (parentSeries.ExplodeRadius * Math.Sin(AngleOfSlice)));
                    }
                    startPoint = new Point(center.X + radius * Math.Cos(ActualStartAngle), center.Y + radius * Math.Sin(ActualStartAngle));
                    Point endPoint = new Point(center.X + radius * Math.Cos(ActualEndAngle), center.Y + radius * Math.Sin(ActualEndAngle));
                    PathFigure figure = new PathFigure();
                    figure.StartPoint = center;
                    WindowsLineSegment line = new WindowsLineSegment();
                    line.Point = startPoint;
                    figure.Segments.Add(line);

                    ArcSegment seg = new ArcSegment();
                    seg.Point = endPoint;
                    seg.Size = new Size(radius, radius);
                    seg.RotationAngle = ChartMath.ParseAtInvarientCulture(ActualEndAngle + ActualStartAngle);
                    seg.IsLargeArc = ActualEndAngle - ActualStartAngle > Math.PI;
                    seg.SweepDirection = StartAngle > EndAngle ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;
                    figure.Segments.Add(seg);

                    figure.IsClosed = true;
                    this.segmentGeometry = new PathGeometry();
                    segmentGeometry.Figures = new PathFigureCollection() { figure };
                    this.segmentPath.Data = segmentGeometry;
                }
                else
                    this.segmentPath.Data = null;
            }
            else if (pieIndex >= 1)
            {
                double radius = (equalParts * (pieIndex + 1)) - (equalParts * (1 - parentSeries.InternalPieCoefficient));
                double innerRadius = equalParts * pieIndex;
                parentSeries.CircularRadius = radius;

                Point center = ChartLayoutUtils.GetCenter(transformer.Viewport);

                if (this.IsExploded)
                {
                    center = new Point(center.X + (parentSeries.ExplodeRadius * Math.Cos(AngleOfSlice)), center.Y + (parentSeries.ExplodeRadius * Math.Sin(AngleOfSlice)));
                }
                startPoint = new Point(center.X + radius * Math.Cos(ActualStartAngle), center.Y + radius * Math.Sin(ActualStartAngle));
                Point endPoint = new Point(center.X + radius * Math.Cos(ActualEndAngle), center.Y + radius * Math.Sin(ActualEndAngle));

                if (Math.Round((ActualEndAngle - ActualStartAngle), 2) == 6.28)
                {
                    GeometryGroup geometryGroup = new GeometryGroup();
                    geometryGroup.Children.Add(new EllipseGeometry()
                    {
                        Center = center,
                        RadiusX = radius,
                        RadiusY = radius
                    });
                    geometryGroup.Children.Add(new EllipseGeometry()
                    {
                        Center = center,
                        RadiusX = innerRadius,
                        RadiusY = innerRadius
                    });
                    this.segmentPath.Data = geometryGroup;
                }
                else if ((ActualEndAngle - ActualStartAngle) != 0)
                {
                    Point startDPoint = new Point(center.X + innerRadius * Math.Cos(ActualStartAngle), center.Y + innerRadius * Math.Sin(ActualStartAngle));
                    Point endDPoint = new Point(center.X + innerRadius * Math.Cos(ActualEndAngle), center.Y + innerRadius * Math.Sin(ActualEndAngle));

                    PathFigure figure = new PathFigure();
                    figure.StartPoint = startPoint;

                    ArcSegment arcseg = new ArcSegment();
                    arcseg.Point = endPoint;
                    arcseg.Size = new Size(radius, radius);
                    arcseg.RotationAngle = ChartMath.ParseAtInvarientCulture(ActualEndAngle - ActualStartAngle);
                    arcseg.IsLargeArc = ActualEndAngle - ActualStartAngle > Math.PI;
                    arcseg.SweepDirection = StartAngle > EndAngle ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;
                    figure.Segments.Add(arcseg);

                    WindowsLineSegment line = new WindowsLineSegment();
                    line.Point = endDPoint;
                    figure.Segments.Add(line);

                    arcseg = new ArcSegment();
                    arcseg.Point = startDPoint;
                    arcseg.Size = new Size(innerRadius, innerRadius);
                    arcseg.RotationAngle = ChartMath.ParseAtInvarientCulture(ActualEndAngle - ActualStartAngle);
                    arcseg.IsLargeArc = ActualEndAngle - ActualStartAngle > Math.PI;
                    arcseg.SweepDirection = StartAngle < EndAngle ? SweepDirection.Counterclockwise : SweepDirection.Clockwise;
                    figure.Segments.Add(arcseg);

                    figure.IsClosed = true;
                    this.segmentGeometry = new PathGeometry();
                    segmentGeometry.Figures = new PathFigureCollection() { figure };
                    this.segmentPath.Data = segmentGeometry;
                }
                else
                    this.segmentPath.Data = null;
            }
        }

        /// <summary>
        /// Called whenever the segment's size changed. This method is not
        /// intended to be called explicitly outside the Chart but it can be overridden by
        /// any derived class.
        /// </summary>
        /// <param name="size"></param>

        internal override void OnSizeChanged(Size size)
        {

        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Method used to check the given co-ordinates lies in pie segment or not
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        internal bool IsPointInPieSegment(double x, double y)
        {
            Canvas canvas = Series.ActualArea.GetAdorningCanvas();
            Size size = new Size(canvas.ActualWidth, canvas.ActualHeight);
            Point center = new Point(size.Width * 0.5, size.Height * 0.5);
            double radius = Math.Min(center.X, center.Y) * (Series as PieSeries).InternalPieCoefficient;
            double dx = x - center.X;
            double dy = y - center.Y;
            double pointLength = Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2));

            if (pointLength < radius)
            {
                double angle = Math.Atan2(dy, dx);
                double arcLength = 3.1415 * 2;
                double start = StartAngle;
                double end = EndAngle;

                if (angle < 0)
                {
                    double degreeangle = (angle / (Math.PI / 180)) + 360;
                    angle = degreeangle * Math.PI / 180;
                }

                if (StartAngle > 0 && end > arcLength && angle < StartAngle)
                {
                    angle = angle + arcLength;
                }

                if (angle > start && angle < end)
                {
                    return true;
                }
            }

            return false;
        }

        internal int GetPieSeriesIndex(ChartSeries currentSeries)
        {
            int index = 0;
            var pieSeries = (from series in parentSeries.Chart.VisibleSeries where series is PieSeries select series).ToList();
            return (index = pieSeries.IndexOf(currentSeries)) >= 0 ? index : -1;
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Method Implementation for set Binding to ChartSegments properties.
        /// </summary>
        /// <param name="element"></param>
        internal override void SetVisualBindings(Shape element)
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Fill");
            element.SetBinding(Shape.FillProperty, binding);
            binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Stroke");
            element.SetBinding(Shape.StrokeProperty, binding);
            binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("StrokeWidth");
            element.SetBinding(Shape.StrokeThicknessProperty, binding);
        }

        #endregion

        #region Private Static Methods

        private static void OnIsExplodedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PieSegment).OnPropertyChanged("IsExploded");
        }

        private static void OnAngleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PieSegment).OnAngleChanged(e);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        private void OnAngleChanged(DependencyPropertyChangedEventArgs e)
        {
            if (this.Series != null && !isInitializing)
            {
                this.Update(this.Series.CreateTransformer(new Size(), false));
            }
        }

        #endregion

        internal override void Dispose()
        {
            if(segmentPath != null)
            {
                segmentPath.Tag = null;
                segmentPath = null;
            }
            base.Dispose();
        }

        #endregion
    }
}
