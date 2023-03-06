using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Microsoft.UI.Xaml.Data;
using Linesegment = Microsoft.UI.Xaml.Media.LineSegment;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class PyramidSegment : ChartSegment
    {
        #region Fields
        
        #region Internal Fields

        internal bool isExploded;

        /// WP-1076[Data marker label position support for pyramid series]
        internal double height = 0d;

        internal double y = 0d;

        #endregion

        #region Private Fields

        private double explodedOffset = 0d;

        private Path segmentPath;

        private PathGeometry segmentGeometry;

        private double xData;

        private double yData;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public PyramidSegment()
        {

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

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <inheritdoc/>

        internal override void SetData(params double[] Values)
        {
            this.y = Values[0];
            this.height = Values[1];
            this.explodedOffset = Values[2];
        }

        /// <inheritdoc/>      
        internal override UIElement CreateVisual(Size size)
        {
            segmentPath = new Path();
            SetVisualBindings(segmentPath);
            segmentPath.Tag = this;
            return segmentPath;
        }

        /// <inheritdoc/>      
        internal override void Update(IChartTransformer transformer)
        {
            if (!this.IsSegmentVisible)
                segmentPath.Visibility = Visibility.Collapsed;
            else
                segmentPath.Visibility = Visibility.Visible;

            Rect rect = new Rect(0, 0, transformer.Viewport.Width, transformer.Viewport.Height);
#pragma warning disable CS0618 // Type or member is obsolete
            PyramidSeries series = Series as PyramidSeries;
#pragma warning restore CS0618 // Type or member is obsolete
            if (rect.IsEmpty)
                this.segmentPath.Data = null;
            else
            {
                if (this.isExploded)
                {
                    rect.X += explodedOffset;
                }
                double top = y;
                double bottom = y + height;
                double topRadius = 0.5d * (1d - y);
                double bottomRadius = 0.5d * (1d - bottom);
                PathFigure figure = new PathFigure();
                figure.StartPoint = new Point(rect.X + topRadius * rect.Width, rect.Y + top * rect.Height);
                Linesegment lineSeg1 = new Linesegment();
                lineSeg1.Point = new Point(rect.X + (1 - topRadius) * rect.Width, rect.Y + top * rect.Height);
                figure.Segments.Add(lineSeg1);
                Linesegment lineSeg3 = new Linesegment();
                lineSeg3.Point = new Point(rect.X + (1 - bottomRadius) * rect.Width, rect.Y + bottom * rect.Height - StrokeWidth);
                figure.Segments.Add(lineSeg3);
                Linesegment lineSeg4 = new Linesegment();
                lineSeg4.Point = new Point(rect.X + bottomRadius * rect.Width, rect.Y + bottom * rect.Height - StrokeWidth);
                figure.Segments.Add(lineSeg4);
                figure.IsClosed = true;
                this.segmentGeometry = new PathGeometry();
                this.segmentGeometry.Figures = new PathFigureCollection() { figure };
                segmentPath.Data = segmentGeometry;

                height = ((bottom - top) * rect.Height) / 2;
            }
        }

        /// <inheritdoc/>      
        internal override UIElement GetRenderedVisual()
        {
            return segmentPath;
        }

        /// <inheritdoc/>      
        internal override void OnSizeChanged(Size size)
        {
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>     
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
            binding.Path = new PropertyPath("StrokeThickness");
            element.SetBinding(Shape.StrokeThicknessProperty, binding);
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
