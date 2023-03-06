// <copyright file="ChartPolarGridLinesPanel.cs" company="Syncfusion. Inc">
// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>
namespace Syncfusion.UI.Xaml.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media;
    using Microsoft.UI.Xaml.Shapes;
    using Windows.Foundation;
    using WindowsLinesegment = Microsoft.UI.Xaml.Media.LineSegment;
#if WinUI_Desktop
    using Microsoft.UI;
#else
using Windows.UI;
#endif  

    /// <summary>
    /// Represents ChartPolarGridLinesPanel
    /// </summary>
    internal class ChartPolarGridLinesPanel : ILayoutCalculator
    {
        #region Fields

        private Size desiredSize;
        private Panel panel;
        private UIElementsRecycler<Ellipse> ellipseRecycler;
        private UIElementsRecycler<Line> linesRecycler;
        private UIElementsRecycler<Line> ylinesRecycler;
        private UIElementsRecycler<Path> pathRecycler;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartPolarGridLinesPanel"/> class.
        /// </summary>
        /// <param name="panel">The Required Panel</param>
        /// <exception cref="ArgumentNullException"><see cref="ArgumentNullException"/> may be thrown</exception>
        public ChartPolarGridLinesPanel(Panel panel)
        {
            if (panel == null)
                throw new ArgumentNullException();

            this.panel = panel;
            ellipseRecycler = new UIElementsRecycler<Ellipse>(panel);
            linesRecycler = new UIElementsRecycler<Line>(panel);
            ylinesRecycler = new UIElementsRecycler<Line>(panel);
            pathRecycler = new UIElementsRecycler<Path>(panel);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the Series is Radar/Polar Series type.
        /// </summary>
        public bool IsRadar
        {
            get
            {
                if (Area != null && Area is SfPolarChart)
                {
                    return ((Area as SfPolarChart).GridLineType == PolarChartGridLineType.Polygon);
                }
                
                return false;
            }
        }

        /// <summary>
        /// Gets the panel.
        /// </summary>
        /// <value>
        /// The panel.
        /// </value>
        public Panel Panel
        {
            get { return panel; }
        }

        /// <summary>
        /// Gets or sets the chart area.
        /// </summary>
        internal ChartBase Area
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the x-axis of the chart.
        /// </summary>      
        public ChartAxis XAxis
        {
            get
            {
                return Area.InternalPrimaryAxis;
            }
        }

        /// <summary>
        ///Gets the y-axis of the chart.
        /// </summary>       
        public ChartAxis YAxis
        {
            get
            {
                return Area.InternalSecondaryAxis;
            }
        }

        /// <summary>
        /// Gets the desired position of the panel.
        /// </summary>       
        public Size DesiredSize
        {
            get { return desiredSize; }
        }

        /// <summary>
        /// Gets the children count in the panel.
        /// </summary> 
        public List<UIElement> Children
        {
            get
            {
                if (panel != null)
                {
                    return panel.Children.Cast<UIElement>().ToList();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public double Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public double Top
        {
            get;
            set;
        }

        #endregion

        #region Methods

        #region Public Methods  

        /// <summary>
        /// Measures the elements of a panel.
        /// </summary>
        /// <param name="availableSize">available size of the panel.</param>
        /// <returns>returns Size.</returns>      
        public Size Measure(Size availableSize)
        {
            desiredSize = new Size(Area.SeriesClipRect.Width, Area.SeriesClipRect.Height);

            if (!IsRadar)
                RenderCircles();

            return availableSize;
        }

        /// <summary>
        /// Arranges the elements of a panel.
        /// </summary>
        /// <param name="finalSize">final size of the panel.</param>
        /// <returns>returns Size</returns>
        public Size Arrange(Size finalSize)
        {
            RenderGridLines();
            return finalSize;
        }

        /// <summary>
        /// Seek the elements from the panel.
        /// </summary>
        public void DetachElements()
        {
            if (ellipseRecycler != null)
                ellipseRecycler.Clear();

            if (linesRecycler != null)
                linesRecycler.Clear();
            if (ylinesRecycler != null)
                ylinesRecycler.Clear();
            panel = null;
        }

        /// <summary>
        /// Adds the elements to the panel.
        /// </summary>
        public void UpdateElements()
        {
            int count = 0;
            if (this.YAxis != null)
                count = this.YAxis.VisibleLabels.Count;

            int totalLinesCount = 0;
            if (!linesRecycler.BindingProvider.Keys.Contains(Line.StyleProperty) && this.Area.InternalPrimaryAxis != null)
            {
                Binding binding = new Binding();
                binding.Path = new PropertyPath("MajorGridLineStyle");
                binding.Source = this.Area.InternalPrimaryAxis;
                linesRecycler.BindingProvider.Add(Line.StyleProperty, binding);
            }

            if (!IsRadar)
            {
                if (this.Area.InternalSecondaryAxis != null && this.Area.InternalSecondaryAxis.MajorGridLineStyle!= null && this.Area.InternalSecondaryAxis.MajorGridLineStyle.TargetType == typeof(Ellipse) && !ellipseRecycler.BindingProvider.Keys.Contains(Ellipse.StyleProperty))
                {
                    Binding binding = new Binding();
                    binding.Path = new PropertyPath("MajorGridLineStyle");
                    binding.Source = this.Area.InternalSecondaryAxis;
                    ellipseRecycler.BindingProvider.Add(Ellipse.StyleProperty, binding);
                    ellipseRecycler.GenerateElements(count);
                }
                else
                {
                    ylinesRecycler.Clear();
                    ellipseRecycler.GenerateElements(count);
                    foreach (Ellipse ellipse in ellipseRecycler)
                    {
                        ellipse.Stroke = new SolidColorBrush(Colors.Gray);
                        ellipse.StrokeThickness = 1;
                    }

                }
            }
            else if (this.XAxis != null)
            {
                ellipseRecycler.Clear();
                totalLinesCount = count * this.XAxis.VisibleLabels.Count;
                if (this.Area.InternalSecondaryAxis != null && !ylinesRecycler.BindingProvider.Keys.Contains(Line.StyleProperty))
                {
                    Binding binding = new Binding();
                    binding.Path = new PropertyPath("MajorGridLineStyle");
                    binding.Source = this.Area.InternalSecondaryAxis;
                    ylinesRecycler.BindingProvider.Add(Line.StyleProperty, binding);
                }

                ylinesRecycler.GenerateElements(totalLinesCount);
            }

            if (this.XAxis != null)
                count = this.XAxis.VisibleLabels.Count;

            linesRecycler.GenerateElements(count);
        }

        #endregion

        #region Internal Methods

        internal void Dispose()
        {
            Area = null;
            if (Children.Count > 0)
            {
                Children.Clear();
            }

            if (ellipseRecycler != null && ellipseRecycler.Count > 0)
            {
                ellipseRecycler.Clear();
            }

            if (linesRecycler != null && linesRecycler.Count > 0)
            {
                linesRecycler.Clear();
            }

            if (ylinesRecycler != null && ylinesRecycler.Count > 0)
            {
                ylinesRecycler.Clear();
            }

            if (pathRecycler != null && pathRecycler.Count > 0)
            {
                pathRecycler.Clear();
            }

            ellipseRecycler = null;
            linesRecycler = null;
            ylinesRecycler = null;
            pathRecycler = null;
        }

        /// <summary>
        /// Renders the circles.
        /// </summary>
        internal void RenderCircles()
        {
            ChartAxis yAxis = this.YAxis;

            double bigRadius = Math.Min(this.DesiredSize.Width, this.DesiredSize.Height) / 2;
            Point center = new Point(this.DesiredSize.Width / 2, this.DesiredSize.Height / 2);

            if (yAxis != null && yAxis.ShowMajorGridLines
                && ellipseRecycler.Count > 0)
            {
                int pos = 0;
                foreach (ChartAxisLabel label in yAxis.VisibleLabels)
                {
                    double radius = bigRadius * yAxis.ValueToCoefficient(label.Position);
                    Ellipse ellipse = ellipseRecycler[pos];
                    ellipse.Width = radius * 2;
                    ellipse.Height = radius * 2;

                    Canvas.SetLeft(ellipse, center.X - radius);
                    Canvas.SetTop(ellipse, center.Y - radius);

                    pos++;
                }
            }
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Creates the binding provider with the provided path and source.
        /// </summary>
        /// <param name="path">The path for binding</param>
        /// <param name="source">The source for binding</param>
        /// <returns>Returns the required binding provider.</returns>
        private static Binding CreateBinding(string path, object source)
        {
            var bindingProvider = new Binding
            {
                Path = new PropertyPath(path),
                Source = source,
                Mode = BindingMode.OneWay
            };
            return bindingProvider;
        }

        /// <summary>
        /// Renders the ellipse.
        /// </summary>
        /// <param name="center">The center point</param>
        /// <param name="start">The start value</param>
        /// <param name="end">The end value</param>
        /// <param name="group">The geometry group</param>
        private static void RenderEllipse(Point center, double start, double end, out GeometryGroup group)
        {
            EllipseGeometry outerEllipse = new EllipseGeometry() { Center = center, RadiusX = end, RadiusY = end };
            EllipseGeometry innerEllipse = new EllipseGeometry() { Center = center, RadiusX = start, RadiusY = start };
            group = new GeometryGroup();
            group.Children.Add(innerEllipse);
            group.Children.Add(outerEllipse);
        }

        /// <summary>
        /// Renders the segment path.
        /// </summary>
        /// <param name="start">The start value</param>
        /// <param name="end">The end value</param>
        /// <param name="angle">The angle</param>
        /// <param name="center">The Cneter point</param>
        /// <param name="vector1">The first vector point</param>
        /// <param name="vector2">The second vector point</param>
        /// <param name="innerArc">The inner arc</param>
        /// <param name="outerArc">the outer arc</param>
        /// <param name="pathGeometry">The path geometry</param>
        private static void RenderSegmentedPath(double start, double end, double angle, Point center, Point vector1, Point vector2, ArcSegment innerArc, ArcSegment outerArc, out PathGeometry pathGeometry)
        {
            Point innerArcStart = new Point(center.X + start * vector1.X, center.Y + start * vector1.Y);
            Point innerArcEnd = new Point(center.X + start * vector2.X, center.Y + start * vector2.Y);
            Point outerArcStart = new Point(center.X + end * vector1.X, center.Y + end * vector1.Y);
            Point outerArcEnd = new Point(center.X + end * vector2.X, center.Y + end * vector2.Y);

            pathGeometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = innerArcStart;
            ChartPolarGridLinesPanel.RenderArc(start, angle, innerArcEnd, out innerArc, SweepDirection.Clockwise);
            WindowsLinesegment line = new WindowsLinesegment();
            line.Point = outerArcEnd;
            ChartPolarGridLinesPanel.RenderArc(end, angle, outerArcStart, out outerArc, SweepDirection.Counterclockwise);
            figure.IsClosed = true;
            figure.Segments.Add(innerArc);
            figure.Segments.Add(line);
            figure.Segments.Add(outerArc);
            pathGeometry.Figures.Add(figure);
        }

        /// <summary>
        /// Calculates the angle between two vectors.
        /// </summary>
        /// <param name="vector1">The first vector</param>
        /// <param name="vector2">The second vector</param>
        /// <param name="angle">The angle</param>
        private static void CalculateAngle(Point vector1, Point vector2, out double angle)
        {
            angle = Math.Atan2(vector2.Y, vector2.X) - Math.Atan2(vector1.Y, vector1.X);
            angle = (angle * 180) / Math.PI;
            if (angle < 0)
                angle = angle + 360;
        }

        /// <summary>
        /// Renders the arc
        /// </summary>
        /// <param name="radius">The radius</param>
        /// <param name="angle">The angle</param>
        /// <param name="point">The point</param>
        /// <param name="arc">The arc</param>
        /// <param name="direction">The direction</param>
        private static void RenderArc(double radius, double angle, Point point, out ArcSegment arc, SweepDirection direction)
        {
            arc = new ArcSegment();
            arc.Size = new Size(radius, radius);
            arc.SweepDirection = direction;
            if (angle > 180)
                arc.IsLargeArc = true;
            arc.Point = point;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Renders the grid lines.
        /// </summary>
        private void RenderGridLines()
        {
            ChartAxis xAxis = this.XAxis;
            ChartAxis yAxis = this.YAxis;

            double bigRadius = Math.Min(this.DesiredSize.Width, this.DesiredSize.Height) / 2;
            Point center = new Point(this.DesiredSize.Width / 2, this.DesiredSize.Height / 2);
            int pos = 0;
            if (IsRadar && yAxis != null && yAxis.ShowMajorGridLines)
            {
                foreach (ChartAxisLabel label in yAxis.VisibleLabels)
                {
                    double radius = bigRadius * yAxis.ValueToCoefficient(label.Position);
                    for (int i = 0; i < xAxis.VisibleLabels.Count; i++)
                    {
                        Point vector = ChartTransform.ValueToVector(xAxis, xAxis.VisibleLabels[i].Position);
                        Point vector2 = new Point();
                        if ((i + 1) < xAxis.VisibleLabels.Count)
                        {
                            vector2 = ChartTransform.ValueToVector(xAxis, xAxis.VisibleLabels[i + 1].Position);
                        }
                        else
                        {
                            vector2 = ChartTransform.ValueToVector(xAxis, xAxis.VisibleLabels[0].Position);
                        }

                        Point connectPoint = new Point(center.X + radius * vector.X, center.Y + radius * vector.Y);
                        Point endPoint = new Point(center.X + radius * vector2.X, center.Y + radius * vector2.Y);

                        Line line = ylinesRecycler[pos];
                        line.X1 = connectPoint.X;
                        line.Y1 = connectPoint.Y;
                        line.X2 = endPoint.X;
                        line.Y2 = endPoint.Y;
                        pos++;
                    }
                }
            }

            if (xAxis != null && xAxis.ShowMajorGridLines)
            {
                int pos1 = 0;

                foreach (ChartAxisLabel label in xAxis.VisibleLabels)
                {
                    Point vector = ChartTransform.ValueToVector(xAxis, label.Position);
                    Line line = linesRecycler[pos1];
                    line.X1 = center.X;
                    line.Y1 = center.Y;
                    line.X2 = center.X + bigRadius * vector.X;
                    line.Y2 = center.Y + bigRadius * vector.Y;
                    pos1++;
                }
            }
        }

        #endregion

        #endregion
    }
}
