using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ScatterSegment : CartesianSegment
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ShapeType Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public float PointWidth { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public float PointHeight { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public float CenterX { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public float CenterY { get; internal set; }

        internal RectF PreviousSegmentBounds { get; set; } = RectF.Zero;

        /// <summary>
        /// Initialize the double fields.
        /// </summary>
        private double x, y;

        private Rect actualRectF;

        #endregion

        #region Methods

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            var scatterSeries = Series as ScatterSeries;
            if (scatterSeries != null)
            {
                PointWidth = (float)scatterSeries.PointWidth;
                PointHeight = (float)scatterSeries.PointHeight;

                CenterX = scatterSeries.TransformToVisibleX(x, y);
                CenterY = scatterSeries.TransformToVisibleY(x, y);
                RectF rectF = new RectF(CenterX - (PointWidth / 2), CenterY - (PointHeight / 2), PointWidth, PointHeight);

                RectF actualSeriesClipRect = scatterSeries.AreaBounds;

                if (rectF.X + rectF.Width > 0 && rectF.X - rectF.Width < actualSeriesClipRect.Width &&
                    rectF.Y + rectF.Height > 0 && rectF.Y - rectF.Height < actualSeriesClipRect.Height)
                {
                    SegmentBounds = rectF;
                }
                else
                {
                    SegmentBounds = Rect.Zero;
                }
            }
        }

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            actualRectF = SegmentBounds;
            if (Series == null || actualRectF.IsEmpty)
            {
                return;
            }

            if (Series.CanAnimate())
            {
                float animationValue = Series.AnimationValue;

                if (!Series.XRange.Equals(Series.PreviousXRange) || PreviousSegmentBounds == RectF.Zero)
                {
                    float newWidth = SegmentBounds.Width * animationValue;
                    float newHeight = SegmentBounds.Height * animationValue;
                    actualRectF = new RectF((SegmentBounds.Left + (SegmentBounds.Width / 2)) - (newWidth / 2), (SegmentBounds.Top + (SegmentBounds.Height / 2)) - (newHeight / 2), newWidth, newHeight);
                }
                else
                {
                    float x = GetDynamicAnimationValue(animationValue, SegmentBounds.X, PreviousSegmentBounds.X, SegmentBounds.X);
                    float y = GetDynamicAnimationValue(animationValue, SegmentBounds.Y, PreviousSegmentBounds.Y, SegmentBounds.Y);
                    actualRectF = new RectF(x, y, (float)actualRectF.Width, (float)actualRectF.Height);
                }
            }

            canvas.SetFillPaint(Fill, SegmentBounds);
            canvas.Alpha = Opacity;

            if (HasStroke)
            {
                canvas.StrokeSize = (float)StrokeWidth;
                canvas.StrokeColor = Stroke.ToColor();
            }

            DrawShape(canvas, actualRectF, shapeType: Type, HasStroke, false);
        }

        /// <summary>
        /// Sets the values for this segment.
        /// </summary>
        internal override void SetData(double[] values)
        {
            this.x = values[0];
            this.y = values[1];
            if (Series != null)
            {
                this.Series.XRange += !double.IsNaN(this.x) ? DoubleRange.Union(this.x) : DoubleRange.Empty;
                this.Series.YRange += !double.IsNaN(this.y) ? DoubleRange.Union(this.y) : DoubleRange.Empty;
            }
        }

        internal override int GetDataPointIndex(float valueX, float valueY)
        {
            float defaultSize = 10;
            RectF touchPointRect = new RectF(valueX - defaultSize / 2, valueY - defaultSize / 2, defaultSize, defaultSize);

            if (Series != null && SegmentBounds.IntersectsWith(touchPointRect))
            {
                return Series.Segments.IndexOf(this);
            }

            return -1;
        }

        internal override void OnDataLabelLayout()
        {
            CalculateDataLabelPosition(x, y);
        }

        #endregion
    }
}