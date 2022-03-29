using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Linesegment = Microsoft.UI.Xaml.Media.LineSegment;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a chart segment which renders collection of points using funnel shape.
    /// </summary>
    /// <seealso cref="FunnelSeries"/>
    /// <seealso cref="PyramidSegment"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class FunnelSegment : ChartSegment
    {
        #region Fields

        #region Internal Fields

        /// WP-1076[Data marker label position support for funnel series]
        internal double height = 0d;

        internal double top = 0d;

        #endregion

        #region Private Fields

        private double bottom, explodedOffset, minimumWidth, topRadius, bottomRadius;

        private Path segmentPath;

        private PathGeometry segmentGeometry;

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FunnelSegment"/>.
        /// </summary>
        public FunnelSegment()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunnelSegment"/>.
        /// </summary>
        /// <param name="y">Used to specify the segment y position.</param>
        /// <param name="height">Height of the funnel.</param>
        /// <param name="funnelSeries">Used to specify the <see cref="FunnelSeries"/> instance.</param>
        /// <param name="isExploded">Used to specify the segment IsExploded or not.</param>
#pragma warning disable CS0618 // Type or member is obsolete
        public FunnelSegment(double y, double height, FunnelSeries funnelSeries, bool isExploded)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            base.Series = funnelSeries;
            top = y;
            bottom = y + height;
            topRadius = y / 2;
            bottomRadius = (y + height) / 2;
            minimumWidth = funnelSeries.MinWidth;
            explodedOffset = funnelSeries.ExplodeOffset;
            this.IsExploded = isExploded;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FunnelSegment"/>.
        /// </summary>
        /// <param name="y">Used to specify the segment y position.</param>
        /// <param name="height">Height of the funnel.</param>
        /// <param name="funnelSeries">Used to specify the <see cref="FunnelSeries"/> instance.</param>
        /// <param name="isExploded">Used to specify the segment IsExploded or not.</param>
        /// <param name="widthTop">>Used to specify the segment width top.</param>
        /// <param name="widthBottom">>Used to specify the segment width bottom.</param>
#pragma warning disable CS0618 // Type or member is obsolete
        public FunnelSegment(double y, double height, double widthTop, double widthBottom, FunnelSeries funnelSeries, bool isExploded)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            base.Series = funnelSeries;
            top = y;
            bottom = y + height;
            topRadius = (1 - widthTop) / 2;
            bottomRadius = (1 - widthBottom) / 2;
            minimumWidth = funnelSeries.MinWidth;
            explodedOffset = funnelSeries.ExplodeOffset;
            this.IsExploded = isExploded;
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether this segment is exploded in the user interface (UI).
        /// </summary>
        /// <value><c>true</c> if the segment is exploded; otherwise, <c>false</c>. The default value is <c>false</c>.</value>
        internal bool IsExploded { get; set; }

        /// <summary>
        /// Gets or sets the data point value, which is used to bind with x value for this segment.
        /// </summary>
        /// <value>A data point value, which is used to bind with x value of the segment.</value>
        public double XData { get; set; }

        /// <summary>
        /// Gets or sets the data point value, which is used to bind with y value for this segment.
        /// </summary>
        /// <value>A data point value, which is used to bind with y value of the segment.</value>
        public double YData { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <inheritdoc/>

        public override void SetData(params double[] Values)
        {
            top = Values[0];
            bottom = Values[1];
            topRadius = Values[2];
            bottomRadius = Values[3];
            minimumWidth = Values[4];
            explodedOffset = Values[5];
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
        public override UIElement GetRenderedVisual()
        {
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
            if (rect.IsEmpty)
                segmentPath.Data = null;
            else
            {
                if (this.IsExploded)
                    rect.X = this.explodedOffset;
                PathFigure figure = new PathFigure();
#pragma warning disable CS0618 // Type or member is obsolete
                FunnelSeries series = Series as FunnelSeries;
#pragma warning restore CS0618 // Type or member is obsolete
                if (series == null) return;
                double minRadius = 0.5 * (1d - minimumWidth / rect.Width);
                bool isBroken = (topRadius >= minRadius) ^ (bottomRadius > minRadius) && series.FunnelMode == ChartFunnelMode.ValueIsHeight;
                double bottomY = minRadius * (bottom - top) / (bottomRadius - topRadius);
                topRadius = Math.Min(topRadius, minRadius);
                bottomRadius = Math.Min(bottomRadius, minRadius);
                figure.StartPoint = new Point(rect.X + topRadius * rect.Width, rect.Y + top * rect.Height);
                Linesegment lineSeg1 = new Linesegment();
                lineSeg1.Point = new Point(rect.X + (1 - topRadius) * rect.Width, rect.Y + top * rect.Height);
                figure.Segments.Add(lineSeg1);
                if (isBroken)
                {
                    Linesegment lineSeg2 = new Linesegment();
                    lineSeg2.Point = new Point(rect.X + (1 - minRadius) * rect.Width, rect.Y + bottomY * rect.Height);
                    figure.Segments.Add(lineSeg2);
                }

                Linesegment lineSeg3 = new Linesegment();
                lineSeg3.Point = new Point(rect.X + (1 - bottomRadius) * rect.Width, rect.Y + bottom * rect.Height - series.StrokeThickness / 2);
                figure.Segments.Add(lineSeg3);
                Linesegment lineSeg4 = new Linesegment();
                lineSeg4.Point = new Point(rect.X + bottomRadius * rect.Width, rect.Y + bottom * rect.Height - series.StrokeThickness / 2);
                figure.Segments.Add(lineSeg4);

                if (isBroken)
                {
                    Linesegment lineSeg5 = new Linesegment();
                    lineSeg5.Point = new Point(rect.X + minRadius * rect.Width, rect.Y + bottomY * rect.Height);
                    figure.Segments.Add(lineSeg5);
                }

                figure.IsClosed = true;
                this.segmentGeometry = new PathGeometry();
                this.segmentGeometry.Figures = new PathFigureCollection() { figure };
                segmentPath.Data = segmentGeometry;

                height = ((bottom - top) * rect.Height) / 2;
            }
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
