﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using WindowsLinesegment = Microsoft.UI.Xaml.Media.LineSegment;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class StepAreaSegment : ChartSegment
    {
        #region Fields

        #region Private Fields
        
        bool isEmpty, isSegmentUpdated;
        Path segPath;
        private Canvas segmentCanvas;
        List<ChartPoint> stepAreaPoints = new List<ChartPoint>();
        PathGeometry strokeGeometry;
        PathFigure strokeFigure;
        PolyLineSegment strokePolyLine;
        Path strokePath;
        private double _xData;
        private double _yData;

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public StepAreaSegment()
        {
            XRange = DoubleRange.Empty;
            YRange = DoubleRange.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointsCollection"></param>
        /// <param name="series"></param>
        [Obsolete("Use StepAreaSegment(List<ChartPoint> pointsCollection, StepAreaSeries series)")]
        public StepAreaSegment(List<Point> pointsCollection, StepAreaSeries series)
        {        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointsCollection"></param>
        /// <param name="series"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        public StepAreaSegment(List<ChartPoint> pointsCollection, StepAreaSeries series)
        {
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the or sets a data point value that is bound with x for the segment.
        /// </summary>
        public double XData
        {
            get { return _xData; }
            set
            {
                _xData = value;
                OnPropertyChanged("XData");
            }
        }


        /// <summary>
        /// Gets the or sets a data point value that is bound with y for the segment.
        /// </summary>
        public double YData
        {
            get { return _yData; }
            set
            {
                _yData = value;
                OnPropertyChanged("YData");
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
        /// <param name="StepAreaPoints"></param>
        [Obsolete("Use StepAreaSegment(List<ChartPoint> pointsCollection, StepAreaSeries series)")]
        internal override void SetData(List<Point> StepAreaPoints)
        {
            var pointCollection = new List<ChartPoint>();
            foreach (var point in StepAreaPoints)
                pointCollection.Add(new ChartPoint(point.X, point.Y));
            this.stepAreaPoints = pointCollection;
            isEmpty = false;
            foreach (ChartPoint pt in pointCollection)
            {
                XRange += pt.X;
                if (double.IsNaN(pt.Y)) { isEmpty = true; continue; }
                YRange += pt.Y;
            }
            if ((!isEmpty && segPath != null))
            {
                Binding binding = new Binding();
                binding.Source = this;
                binding.Path = new PropertyPath("StrokeWidth");
                segPath.SetBinding(Path.StrokeThicknessProperty, binding);
            }
        }

        /// <summary>
        /// Sets the values for this segment. This method is not intended to be called explicitly outside the Chart but it can be overriden by any derived class.
        /// </summary>
        internal override void SetData(List<ChartPoint> StepAreaPoints)
        {
            this.stepAreaPoints = StepAreaPoints;
            isEmpty = false;
            foreach (ChartPoint pt in StepAreaPoints)
            {
                XRange += pt.X;
                if (double.IsNaN(pt.Y)) { isEmpty = true; continue; }
                YRange += pt.Y;
            }
            if ((!isEmpty && segPath != null))
            {
                Binding binding = new Binding();
                binding.Source = this;
                binding.Path = new PropertyPath("StrokeWidth");
                segPath.SetBinding(Path.StrokeThicknessProperty, binding);
            }
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
            segmentCanvas = new Canvas();
            segPath = new Path();
            segPath.Tag = this;
            SetVisualBindings(segPath);
            segmentCanvas.Children.Add(segPath);
            return segmentCanvas;

        }

        /// <summary>
        /// Gets the UIElement used for rendering this segment.
        /// </summary>
        /// <returns>reurns UIElement</returns>
        internal override UIElement GetRenderedVisual()
        {
            return segPath;
        }

        /// <summary>
        /// Updates the segments based on its data point value. This method is not
        /// intended to be called explicitly outside the Chart but it can be overriden by
        /// any derived class.
        /// </summary>
        /// <param name="transformer">Reresents the view port of chart control.(refer <see cref="IChartTransformer"/>)</param>
        internal override void Update(IChartTransformer transformer)
        {
            PathFigure figure = new PathFigure();
            if (this.stepAreaPoints.Count > 0)
            {
                if (isSegmentUpdated)
                    Series.SeriesRootPanel.Clip = null;

                figure.StartPoint = transformer.TransformToVisible(this.stepAreaPoints[1].X, this.stepAreaPoints[1].Y);

                PathGeometry segmentGeometry = new PathGeometry();
                WindowsLinesegment linesegment;
                double origin = 0;

                strokeGeometry = new PathGeometry();
                strokeFigure = new PathFigure();
                strokePolyLine = new PolyLineSegment();
                strokePath = new Path();

                if (!(Series as StepAreaSeries).IsClosed && !double.IsNaN(this.stepAreaPoints[3].Y))
                {
                    AddStroke(transformer.TransformToVisible(this.stepAreaPoints[2].X, this.stepAreaPoints[3].Y));
                }
                else if (isEmpty)
                {
                    AddStroke(figure.StartPoint);
                }

                for (int i = 1; i < this.stepAreaPoints.Count; i += 2)
                {
                    if (!double.IsNaN(stepAreaPoints[i].Y) && !double.IsNaN(stepAreaPoints[i + 1].Y))
                    {
                        Point point1 = transformer.TransformToVisible(this.stepAreaPoints[i].X, stepAreaPoints[i].Y);
                        Point point2 = transformer.TransformToVisible(this.stepAreaPoints[i + 1].X, stepAreaPoints[i + 1].Y);
                        linesegment = new WindowsLinesegment();
                        linesegment.Point = point1;
                        figure.Segments.Add(linesegment);
                        linesegment = new WindowsLinesegment();
                        linesegment.Point = point2;
                        figure.Segments.Add(linesegment);
                        if (isEmpty && (Series as StepAreaSeries).IsClosed)
                        {
                            strokePolyLine.Points.Add(point1);
                            if (i <= stepAreaPoints.Count - 3 && !double.IsNaN(stepAreaPoints[i + 3].Y))
                                strokePolyLine.Points.Add(point2);
                        }
                        else if (!(Series as StepAreaSeries).IsClosed)
                        {
                            if (i > 1)
                            {
                                strokePolyLine.Points.Add(point1);
                                if (i <= stepAreaPoints.Count - 3 && !double.IsNaN(stepAreaPoints[i + 3].Y))
                                    strokePolyLine.Points.Add(point2);
                            }
                        }
                    }
                    else
                    {
                        linesegment = new WindowsLinesegment();
                        linesegment.Point = transformer.TransformToVisible(this.stepAreaPoints[i - 1].X, origin);
                        figure.Segments.Add(linesegment);
                        if (double.IsNaN(stepAreaPoints[i - 1].Y))
                        {
                            linesegment = new WindowsLinesegment();
                            linesegment.Point = transformer.TransformToVisible(this.stepAreaPoints[i - 1].X, origin);
                            figure.Segments.Add(linesegment);
                        }
                        if (i < stepAreaPoints.Count - 1) //WPF-14682 
                        {
                            linesegment = new WindowsLinesegment();
                            linesegment.Point = transformer.TransformToVisible(this.stepAreaPoints[i + 1].X, origin);
                            figure.Segments.Add(linesegment);

                            if (double.IsNaN(stepAreaPoints[i - 1].Y) && !double.IsNaN(stepAreaPoints[i + 1].Y))
                            {
                                Point point1 = transformer.TransformToVisible(this.stepAreaPoints[i].X,
                                    stepAreaPoints[i + 1].Y);
                                if ((Series as StepAreaSeries).IsClosed)
                                {
                                    strokeFigure = new PathFigure();
                                    strokePolyLine = new PolyLineSegment();
                                    strokeFigure.StartPoint = point1;
                                    strokeGeometry.Figures.Add(strokeFigure);
                                    strokeFigure.Segments.Add(strokePolyLine);
                                }
                                else if (!(Series as StepAreaSeries).IsClosed)
                                {
                                    if (i > 1)
                                    {
                                        strokeFigure = new PathFigure();
                                        strokePolyLine = new PolyLineSegment();
                                        strokeFigure.StartPoint = point1;
                                        strokeGeometry.Figures.Add(strokeFigure);
                                        strokeFigure.Segments.Add(strokePolyLine);
                                    }
                                }
                                linesegment = new WindowsLinesegment();
                                linesegment.Point = point1;
                                figure.Segments.Add(linesegment);
                            }
                        }
                    }
                }
                linesegment = new WindowsLinesegment();
                linesegment.Point = transformer.TransformToVisible(this.stepAreaPoints[stepAreaPoints.Count - 1].X, origin);
                figure.Segments.Add(linesegment);

                if ((Series as StepAreaSeries).IsClosed && (isEmpty && !double.IsNaN(stepAreaPoints[stepAreaPoints.Count - 1].Y)))
                    strokePolyLine.Points.Add(linesegment.Point);
                segmentGeometry.Figures.Add(figure);
                this.segPath.Data = segmentGeometry;

                isSegmentUpdated = true;
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

        #region Private Methods

        /// <summary>
        /// Called to add the stroke for step area series.
        /// </summary>
        /// <param name="startPoint"></param>
        private void AddStroke(Point startPoint)
        {
            if (segmentCanvas.Children.Count > 1)
                segmentCanvas.Children.RemoveAt(1); //remove the existing stroke path 
            segPath.StrokeThickness = 0;

            strokePath = new Path();

            strokeFigure.StartPoint = startPoint;
            strokeFigure.Segments.Add(strokePolyLine);
            strokeGeometry.Figures.Add(strokeFigure);

            strokePath.Data = strokeGeometry;
            segmentCanvas.Children.Add(strokePath);

            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Stroke");
            strokePath.SetBinding(Path.StrokeProperty, binding);
            binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("StrokeWidth");
            strokePath.SetBinding(Path.StrokeThicknessProperty, binding);
        }

        #endregion

        internal override void Dispose()
        {
            if (segmentCanvas != null)
            {
                segmentCanvas.Children.Clear();
                segmentCanvas = null;
            }
            if (segPath != null)
            {
                segPath.Tag = null;
                segPath = null;
            }
            base.Dispose();
        }

        #endregion
    }
}