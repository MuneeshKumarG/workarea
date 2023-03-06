using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Creates the segments for <see cref="BoxAndWhiskerSeries"/>
    /// <para>It gets the bounds, stroke color and fill color from the <c>series</c> to render the segment.</para>
    /// </summary>
    public class BoxAndWhiskerSegment : CartesianSegment
    {
        #region Fields

        #region Private Fields

        private double x1, y1, x2, y2;
        private double average;
        private readonly float crossWidth = 10;
        private readonly float crossHeight = 10;
        private readonly float outlierHeight = 10;
        private readonly float outlierWidth = 10;
        private float showMedianPointX;
        private float showMedianPointY;
        private float midPoint;
        private float medianLinePointX;
        private float medianLinePointY;
        private float maximumLinePointX;
        private float maximumLinePointY;
        private float minimumLinePointX;
        private float minimumLinePointY;
        private float lowerQuartileLinePointX;
        private float lowerQuartileLinePointY;
        private float upperQuartileLinePointX;
        private float upperQuartileLinePointY;
        private PointF outlierPoints;
        private readonly List<RectF> outlierSegmentBounds;

        #endregion

        #region Internal Fields

        internal int outlierIndex;
        internal List<double> Outliers;
        #endregion

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the maximum value for the box plot.
        /// </summary>
        public double Maximum
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the minimum value for the box plot.
        /// </summary>
        public double Minimum
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the median value for the box plot.
        /// </summary>
        public double Median
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the lower quartile value for the box plot.
        /// </summary>
        public double LowerQuartile
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the upper quartile value for the box plot.
        /// </summary>
        public double UpperQuartile
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the left value for the box plot.
        /// </summary>
        public float Left
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the right value for the box plot.
        /// </summary>
        public float Right
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the top value for the box plot.
        /// </summary>
        public float Top
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the bottom value for the box plot.
        /// </summary>
        public float Bottom
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the center value for the box plot.
        /// </summary>
        public double Center
        {
            get;
            internal set;
        }
              
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public BoxAndWhiskerSegment()
        {         
            Outliers = new List<double>();           
            outlierSegmentBounds= new List<RectF>();           
        }

        #endregion

        #region Methods

        #region Internal Override Methods

        internal override void SetData(double[] values)
        {
            if (Series is not BoxAndWhiskerSeries series)
            {
                return;
            }

            x1 = values[0];
            x2 = values[1];
            y1 = values[2];
            y2 = values[8];
            
            Minimum = values[3];
            LowerQuartile = values[4];
            Median = values[5];
            UpperQuartile = values[6];
            Maximum = values[7];
            Center = values[9];
            average = values[10];

            series.XRange += new DoubleRange(x1, x2);
            series.YRange += new DoubleRange(y1, y2);
        }

        internal override int GetDataPointIndex(float x, float y)
        {
            if (Series is not BoxAndWhiskerSeries series)
            {
                return -1;
            }

            bool horizontalTop = IsRectContains(Left, maximumLinePointY, Right, maximumLinePointY, x, y, (float)StrokeWidth);
            bool horizontalBottom = IsRectContains(Left, minimumLinePointY, Right, minimumLinePointY, x, y, (float)StrokeWidth);
            bool verticalTop = IsRectContains(midPoint, maximumLinePointY, midPoint, upperQuartileLinePointY, x, y, (float)StrokeWidth);
            bool verticalBottom = IsRectContains(midPoint, minimumLinePointY, midPoint, lowerQuartileLinePointY, x, y, (float)StrokeWidth);

            if (Series != null && (SegmentBounds.Contains(x, y) || horizontalTop || horizontalBottom || verticalTop || verticalBottom))
            {            
                series.IsOutlierTouch = false;
                return Series.Segments.IndexOf(this);
            }         
            else if (Series != null && outlierSegmentBounds.Count > 0) 
            {              
                for(int i=0;i<outlierSegmentBounds.Count;i++) 
                {
                    if (outlierSegmentBounds[i].Contains(x,y))
                    {
                        outlierIndex = i;
                        series.IsOutlierTouch = true;
                        return Series.Segments.IndexOf(this);                     
                    }                
                }
            }

            return -1;
        }

        #endregion

        #region Protected  Internal Override Methods

        /// <summary>
        /// 
        /// </summary>
        protected internal override void OnLayout()
        {
            if (Series is BoxAndWhiskerSeries series)
                Layout(series);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        protected internal override void Draw(ICanvas canvas)
        {
            BoxAndWhiskerSeries? series = Series as BoxAndWhiskerSeries;

            if (series == null)
            {
                return;
            }

            if (series.CanAnimate())
            {
                Layout(series);              
            }

            if (!float.IsNaN(Left) && !float.IsNaN(Top) && !float.IsNaN(Right) && !float.IsNaN(Bottom))
            {
                //Stroke Width is Zero it gives the width=1
                if (StrokeWidth <= 0)
                {
                    canvas.StrokeSize = 1;
                }
                else
                {
                    canvas.StrokeSize = (float)StrokeWidth;
                }

                canvas.StrokeColor = Stroke.ToColor();
                canvas.Alpha = Opacity;

                //Drawing segment.
                var rect = new Rect() { Left = Left, Top = Top, Right = Right, Bottom = Bottom };
                canvas.SetFillPaint(Fill, rect);
                canvas.FillRectangle(rect);

                // Draw the Rectangle
                canvas.DrawRectangle(rect);

                //Set ShowMedian=true ,the cross Mark will be drawn with respect to average value of segment.
                if (series.ShowMedian)
                {
                    var rectMedian = new RectF(showMedianPointX - (crossWidth / 2), showMedianPointY - (crossHeight / 2), crossWidth, crossHeight);                  
                    canvas.DrawShape(rectMedian, ShapeType.Cross, true, true);
                }

                //Set an IsTransposed=True Cartesian chart will be drawn in Horizontal.
                if (series?.ChartArea?.IsTransposed is true)
                {
                    canvas.DrawLine(medianLinePointX, Top, medianLinePointX, Bottom);
                    canvas.DrawLine(maximumLinePointX, Top, maximumLinePointX, Bottom);
                    canvas.DrawLine(minimumLinePointX, Top, minimumLinePointX, Bottom);
                    float centerPoint = (Top + Bottom) / 2;
                    canvas.DrawLine(maximumLinePointX, centerPoint, upperQuartileLinePointX, centerPoint);
                    canvas.DrawLine(minimumLinePointX, centerPoint, lowerQuartileLinePointX, centerPoint);
                }
                else
                {
                    //Draw the Median line
                    canvas.DrawLine(Left, medianLinePointY, Right, medianLinePointY);

                    //Draw the Maximum Line
                    canvas.DrawLine(Left, maximumLinePointY, Right, maximumLinePointY);

                    //Draw the Minimum Line 
                    canvas.DrawLine(Left, minimumLinePointY, Right, minimumLinePointY);

                    midPoint = (Left + Right) / 2;

                    //Draw the vertical Line over the UpperQuartile to Maximum point
                    canvas.DrawLine(midPoint, maximumLinePointY, midPoint, upperQuartileLinePointY);

                    //Draw the vertical Line over the LowerQuartile to Minimum point
                    canvas.DrawLine(midPoint, minimumLinePointY, midPoint, lowerQuartileLinePointY);
                }                

                //Drawing ellipse above or below in the Box and Whisker segment.                            
                if(this.outlierSegmentBounds.Count>0)
                {
                    for(int i=0;i<outlierSegmentBounds.Count;i++)
                    {
                        canvas.SetFillPaint(Fill, outlierSegmentBounds[i]);
                        canvas.DrawShape(outlierSegmentBounds[i], series!.OutlierShapeType, true, false);
                    }
                }
               
            }
        }

        #endregion

        #region Private methods
        private void Layout(BoxAndWhiskerSeries? series)
        {
            var xAxis = series?.ActualXAxis;

            if (series == null || series.ChartArea == null || xAxis == null)
            {
                return;
            }

            var start = Math.Floor(xAxis.VisibleRange.Start);
            var end = Math.Ceiling(xAxis.VisibleRange.End);
            double y1Value = LowerQuartile;
            double y2Value = UpperQuartile;

            Left = Top = Bottom = Right = float.NaN;

            if (x1 <= end && x2 >= start)
            {
                Left = series.TransformToVisibleX(x1, y1Value);
                Top = series.TransformToVisibleY(x1, y1Value);
                Right = series.TransformToVisibleX(x2, y2Value);
                Bottom = series.TransformToVisibleY(x2, y2Value);

                medianLinePointX = series.TransformToVisibleX(x1, Median);
                medianLinePointY = series.TransformToVisibleY(x2, Median);

                maximumLinePointX = series.TransformToVisibleX(x1, Maximum);
                maximumLinePointY = series.TransformToVisibleY(x2, Maximum);

                minimumLinePointX = series.TransformToVisibleX(x1, Minimum);
                minimumLinePointY = series.TransformToVisibleY(x2, Minimum);

                upperQuartileLinePointX = series.TransformToVisibleX(x1, UpperQuartile);
                upperQuartileLinePointY = series.TransformToVisibleY(x2, UpperQuartile);

                lowerQuartileLinePointX = series.TransformToVisibleX(x1, LowerQuartile);
                lowerQuartileLinePointY = series.TransformToVisibleY(x2, LowerQuartile);

                showMedianPointX = series.TransformToVisibleX(Center, average);
                showMedianPointY = series.TransformToVisibleY(Center, average);

                //Calculated animation values for column and line segment
                float MedianPoint = ((Top + Bottom) / 2) - Top;
                Top += (MedianPoint * (1 - series.AnimationValue));
                Bottom -= (MedianPoint * (1 - series.AnimationValue));

                float MedianLinePoint = ((maximumLinePointY + minimumLinePointY) / 2) - maximumLinePointY;
                maximumLinePointY += (MedianLinePoint * (1 - series.AnimationValue));
                minimumLinePointY -= (MedianLinePoint * (1 - series.AnimationValue));

                upperQuartileLinePointY = upperQuartileLinePointY - (MedianPoint * (1 - series.AnimationValue));
                lowerQuartileLinePointY = lowerQuartileLinePointY + (MedianPoint * (1 - series.AnimationValue));
                maximumLinePointY += (MedianPoint / 6) * (1 - series.AnimationValue);

                if (!series.CanAnimate() && Outliers.Count > 0)
                {
                    outlierSegmentBounds.Clear();
                    for (int i = 0; i < Outliers.Count; i++)
                    {
                        outlierPoints = new PointF(series.TransformToVisibleX(Center, Outliers[i]), series.TransformToVisibleY(Center, Outliers[i]));
                        var rectF = new RectF(outlierPoints.X - (outlierHeight / 2), outlierPoints.Y - (outlierWidth / 2), outlierWidth, outlierHeight);
                        outlierSegmentBounds.Add(rectF);
                    }
                }

                if (Left > Right)
                {
                    var temp = Left;
                    Left = Right;
                    Right = temp;
                }
                if (Top > Bottom)
                {
                    var temp = Top;
                    Top = Bottom;
                    Bottom = temp;
                }
            }
            else
            {
                this.Left = float.NaN;
            }

            SegmentBounds = new RectF(Left, Top, Right - Left, Bottom - Top);
        }
        #endregion

        #endregion
    }
}
