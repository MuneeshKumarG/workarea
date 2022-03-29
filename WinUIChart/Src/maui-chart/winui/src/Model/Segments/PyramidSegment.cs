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
    /// Represents a chart segment which renders collection of points using pyramid shape.
    /// </summary>
    /// <seealso cref="PyramidSeries"/>
    /// <seealso cref="FunnelSeries"/>
    /// <seealso cref="FunnelSegment"/>  
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
        /// Initializes a new instance of the <see cref="PyramidSegment"/>.
        /// </summary>
        public PyramidSegment()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PyramidSegment"/>.
        /// </summary>
        /// <param name="y">Used to specify y value.</param>
        /// <param name="height">Used to specify pyamid height value.</param>
        /// <param name="explodedOffset">Used to specify ExplodedeOffset value.</param>
        /// <param name="series">Used to specify <see cref="PyramidSeries"/> instance.</param>
        /// <param name="isExploded">Used to specify the segment is exploded or not.</param>
#pragma warning disable CS0618 // Type or member is obsolete
        public PyramidSegment(double y, double height, double explodedOffset, PyramidSeries series, bool isExploded)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            base.Series = series;
            this.y = y;
            this.height = height;
            this.isExploded = isExploded;
            this.explodedOffset = explodedOffset;
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the data point value, which is used to bind with x value for this segment.
        /// </summary>
        /// <value>A data point value, which is used to bind with x value of the segment.</value>
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
        /// Gets or sets the data point value, which is used to bind with y value for this segment.
        /// </summary>
        /// <value>A data point value, which is used to bind with y value of the segment.</value>
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

        public override void SetData(params double[] Values)
        {
            this.y = Values[0];
            this.height = Values[1];
            this.explodedOffset = Values[2];
        }

        /// <inheritdoc/>      
        public override UIElement CreateVisual(Size size)
        {
            segmentPath = new Path();
            SetVisualBindings(segmentPath);
            segmentPath.Tag = this;
            return segmentPath;
        }

        /// <inheritdoc/>      
        public override void Update(IChartTransformer transformer)
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
                lineSeg3.Point = new Point(rect.X + (1 - bottomRadius) * rect.Width, rect.Y + bottom * rect.Height - series.StrokeThickness / 2);
                figure.Segments.Add(lineSeg3);
                Linesegment lineSeg4 = new Linesegment();
                lineSeg4.Point = new Point(rect.X + bottomRadius * rect.Width, rect.Y + bottom * rect.Height - series.StrokeThickness / 2);
                figure.Segments.Add(lineSeg4);
                figure.IsClosed = true;
                this.segmentGeometry = new PathGeometry();
                this.segmentGeometry.Figures = new PathFigureCollection() { figure };
                segmentPath.Data = segmentGeometry;

                height = ((bottom - top) * rect.Height) / 2;
            }
        }

        /// <inheritdoc/>      
        public override UIElement GetRenderedVisual()
        {
            return segmentPath;
        }

        /// <inheritdoc/>      
        public override void OnSizeChanged(Size size)
        {
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>     
        protected override void SetVisualBindings(Shape element)
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("Interior");
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
