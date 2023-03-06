﻿using System;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    ///Defines methods and memebers to transform the screen co-ordinate to Chart co-ordinate.
    /// </summary>
    /// <exclude/>
    public interface IChartTransformer
    {
        /// <summary>
        /// Gets the viewport.
        /// </summary>
        /// <value>The viewport.</value>
        /// <exclude/>
        Size Viewport { get; }

        /// <summary>
        /// Transforms chart cordinates to real coordinates.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <returns>Visible point</returns>
        Point TransformToVisible(double x, double y);
    }

    /// <summary>
    /// Class implementation for Chartransform
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1034: Nested types should not be visible")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    internal static class ChartTransform
    {
        /// <summary>
        /// Represents ChartSimpleTransformer
        /// </summary>
        private class ChartSimpleTransformer : IChartTransformer
        {
            #region Members
            /// <summary>
            /// Initializes m_viewport
            /// </summary>
            private Size m_viewport = Size.Empty;
            #endregion

            #region Properties
            /// <summary>
            /// Gets the viewport.
            /// </summary>
            /// <value>The viewport.</value>
            public Size Viewport
            {
                get
                {
                    return m_viewport;
                }
            }
            #endregion

            #region Constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="ChartSimpleTransformer"/> class.
            /// </summary>
            /// <param name="viewport">The viewport.</param>
            public ChartSimpleTransformer(Size viewport)
            {
                m_viewport = viewport;
            }
            #endregion

            #region Public methods
            /// <summary>
            /// Transforms chart cordinates to real coordinates.
            /// </summary>
            /// <param name="x">The x value.</param>
            /// <param name="y">The y value.</param>
            /// <returns>visible point</returns>
            public Point TransformToVisible(double x, double y)
            {
                return new Point(x, y);
            }
            #endregion

        }

        /// <summary>
        /// Represents ChartCartesianTransformer
        /// </summary>
        internal class ChartCartesianTransformer : IChartTransformer
        {
            #region Members
            /// <summary>
            /// Initializes m_viewport
            /// </summary>
            private Size m_viewport = Size.Empty;

            /// <summary>
            /// Initializes m_xAxis
            /// </summary>
            public ChartAxis XAxis;

            /// <summary>
            /// Initializes m_yAxis
            /// </summary>
            public ChartAxis YAxis;

            /// <summary>
            /// Initializes m_IsRotated
            /// </summary>
            private bool m_IsRoated;

            #endregion

            #region Properties
            /// <summary>
            /// Gets the viewport.
            /// </summary>
            /// <value>The viewport.</value>
            public Size Viewport
            {
                get
                {
                    return m_viewport;
                }
            }
            #endregion

            #region Constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="ChartCartesianTransformer"/> class.
            /// </summary>
            /// <param name="viewport">The viewport.</param>
            /// <param name="xAxis">The x axis.</param>
            /// <param name="yAxis">The y axis.</param>
            public ChartCartesianTransformer(Size viewport, ChartAxis xAxis, ChartAxis yAxis)
            {
                m_viewport = viewport;
                XAxis = xAxis;
                YAxis = yAxis;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ChartCartesianTransformer"/> class.
            /// </summary>
            /// <param name="viewport">The viewport.</param>
            /// <param name="series">The series.</param>
            public ChartCartesianTransformer(Size viewport, ChartSeries series)
            {
                if (series.ActualXAxis == null || series.ActualYAxis == null) return;
                m_viewport = viewport;
                XAxis = series.ActualXAxis;
                YAxis = series.ActualYAxis;

                m_IsRoated = series.IsActualTransposed; //When we rotate Second series defined with XAxis and YAxis then series not render.

            }
            #endregion

            #region Public methods
            /// <summary>
            /// Transforms chart cordinates to real coordinates.
            /// </summary>
            /// <param name="x">The x value.</param>
            /// <param name="y">The y value.</param>
            /// <returns>The visible point</returns>
            public Point TransformToVisible(double x, double y)
            {
                if (XAxis != null && YAxis != null && double.IsNaN(m_viewport.Width) == false && double.IsNaN(m_viewport.Height) == false)
                {
                    ChartBase area = XAxis.Area;
                    
                    var isSfChart = area is ChartBase;
                    if (this.m_IsRoated)
                    {
                        double left = isSfChart ? YAxis.RenderedRect.Left - XAxis.Area.SeriesClipRect.Left : YAxis.RenderedRect.Left;
                        double top = isSfChart ? XAxis.RenderedRect.Top - YAxis.Area.SeriesClipRect.Top : XAxis.RenderedRect.Top;
                        return new Point(left + YAxis.RenderedRect.Width * YAxis.ValueToCoefficient(y), top + XAxis.RenderedRect.Height * (1 - XAxis.ValueToCoefficient(x)));
                    }
                    else
                    {
                        if (isSfChart)
                        {
                            double left = XAxis.RenderedRect.Left - XAxis.Area.SeriesClipRect.Left;
                            double top = YAxis.RenderedRect.Top - YAxis.Area.SeriesClipRect.Top;
                            return new Point(left + XAxis.RenderedRect.Width * XAxis.ValueToCoefficient(x), top + YAxis.RenderedRect.Height * (1 - YAxis.ValueToCoefficient(y)));
                        }
                        else
                        {
                            double left = XAxis.RenderedRect.Left;
                            double top = YAxis.RenderedRect.Top;
                            return new Point(left + Math.Round(XAxis.RenderedRect.Width * XAxis.ValueToCoefficient(x)), top + Math.Round(YAxis.RenderedRect.Height * (1 - YAxis.ValueToCoefficient(y))));
                        }
                    }
                }
                
                return new Point(0, 0);
            }

            #endregion
        }

        /// <summary>
        /// Represents ChartPolarTransformer
        /// </summary>
        internal class ChartPolarTransformer : IChartTransformer
        {
            #region Members

            
            /// <summary>
            /// Initializes m_viewport
            /// </summary>
            private Rect m_viewport = Rect.Empty;

            /// <summary>
            /// Initializes m_xAxis
            /// </summary>
            private ChartAxis m_xAxis;

            /// <summary>
            /// Initializes m_yAxis
            /// </summary>
            private ChartAxis m_yAxis;

            /// <summary>
            /// Initializes m_center
            /// </summary>
            private Point m_center = new Point();

            /// <summary>
            /// Initializes m_radius
            /// </summary>
            private double m_radius;

            #endregion

            #region Properties
            /// <summary>
            /// Gets the viewport.
            /// </summary>
            /// <value>The viewport.</value>
            public Rect Viewport
            {
                get
                {
                    return m_viewport;
                }
            }
            #endregion

            #region Constructor
            /// <summary>
            /// Initializes a new instance of the <see cref="ChartPolarTransformer"/> class.
            /// </summary>
            /// <param name="viewport">The viewport.</param>
            /// <param name="xAxis">The x axis.</param>
            /// <param name="yAxis">The y axis.</param>
            public ChartPolarTransformer(Rect viewport, ChartAxis xAxis, ChartAxis yAxis)
            {
                m_viewport = viewport;
                m_xAxis = xAxis;
                m_yAxis = yAxis;
                m_center = ChartLayoutUtils.GetCenter(m_viewport);
                m_radius = 0.5 * Math.Min(m_viewport.Width, m_viewport.Height);
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="ChartPolarTransformer"/> class.
            /// </summary>
            /// <param name="viewport">The viewport.</param>
            /// <param name="series">The series.</param>
            public ChartPolarTransformer(Rect viewport, ChartSeries series)
            {
                m_viewport = viewport;
                m_xAxis = series.ActualXAxis;
                m_yAxis = series.ActualYAxis;
                m_center = ChartLayoutUtils.GetCenter(m_viewport);
                m_radius = 0.5 * Math.Min(m_viewport.Width, m_viewport.Height);
               
            }
            #endregion

            #region Public methods
            /// <summary>
            /// Transforms chart cordinates to real coordinates.
            /// </summary>
            /// <param name="x">The x value.</param>
            /// <param name="y">The y value.</param>
            /// <returns>The visible point</returns>
            public Point TransformToVisible(double x, double y)
            {
                double radius = m_radius * m_yAxis.ValueToCoefficient(y);
                Point point = ChartTransform.ValueToVector(m_xAxis, x);
                return new Point(m_center.X + radius * point.X, m_center.Y + radius * point.Y);
            }
            #endregion

            Size IChartTransformer.Viewport
            {
                get { return Size.Empty; }
            }
        }

        #region Implementation
        /// <summary>
        /// Creates the Cartesian transformer.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <returns>The Chart Transformer</returns>
        public static IChartTransformer CreateSimple(Size viewport)
        {
            return new ChartSimpleTransformer(viewport);
        }
    
        /// <summary>
        /// Creates the Cartesian transformer.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <param name="series">The series.</param>
        /// <returns>The Cartesian Transformer</returns>
        public static IChartTransformer CreateCartesian(Size viewport, ChartSeries series)
        {
            return new ChartCartesianTransformer(viewport, series);
        }

        /// <summary>
        /// Creates the Cartesian transformer.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <param name="xAxis">The x axis.</param>
        /// <param name="yAxis">The y axis.</param> 
        /// <returns>The Cartesian Transformer</returns>
        public static IChartTransformer CreateCartesian(Size viewport, ChartAxis xAxis, ChartAxis yAxis)
        {
            return new ChartCartesianTransformer(viewport, xAxis, yAxis);
        }

        /// <summary>
        /// Creates the polar.
        /// </summary>
        /// <param name="viewport">The viewport.</param>
        /// <param name="series">The series.</param>
        /// <returns>The Polar Transformer</returns>
        public static IChartTransformer CreatePolar(Rect viewport, ChartSeries series)
        {
            return new ChartPolarTransformer(viewport, series);
        }

        /// <summary>
        /// Return point values from the given values 
        /// </summary>
        /// <param name="coefficient"></param>
        /// <returns></returns>
        public static Point CoefficientToVector(double coefficient)
        {
            double angle = Math.PI * (1.5 - 2 * coefficient);

            return new Point(Math.Cos(angle), Math.Sin(angle));
        }

        /// <summary>
        /// Return the Polar/Radar type Axis Coefficient Value from the given radian value
        /// </summary>
        /// <param name="radian"></param>
        /// <returns></returns>
        public static double RadianToPolarCoefficient(double radian)
        {
            if (radian <= ((3*Math.PI)/2))
                return (1.5 - (radian / Math.PI)) / 2;
            else
                return 0.75 + (0.75 - ((1.5 - (((2*Math.PI) - radian) / Math.PI)) / 2));
        }

        /// <summary>
        /// Return the Radian Value of Polar/Radar chart Mouse point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="center"></param>
        /// <returns></returns>
        public static double PointToPolarRadian(Point point,double center)
        {
            double diffX = Math.Abs((point.X) - center);
            double diffY = Math.Abs((point.Y) - center);
            double radi = Math.Atan2(diffY, diffX);
            if ((point.X - center) < 0 && (point.Y - center) < 0)
            {
                return Math.PI + radi;
            }
            else if((point.X - center) < 0 && (point.Y - center) > 0)
            {
                 return Math.PI - radi;
            }
            else if ((point.X - center) > 0 && (point.Y - center) < 0)
            {
                return (2*Math.PI )- radi;
            }
            else
            {
                return radi;
            }
        }

        /// <summary>
        /// Values to vector.
        /// </summary>
        /// <param name="axis">The axis value.</param>
        /// <param name="value">The value.</param>
        /// <returns>The vector value</returns>
        public static Point ValueToVector(ChartAxis axis, double value)
        {
                return CoefficientToVector( axis.ValueToPolarCoefficient(value));         
        }

        #endregion
    }
}
