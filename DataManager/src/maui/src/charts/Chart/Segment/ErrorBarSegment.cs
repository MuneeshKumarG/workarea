using Microsoft.Maui.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents a segment of a <see cref="ErrorBarSeries"/> type charts.
    /// </summary>
    public class ErrorBarSegment : ChartSegment
    {
        #region Fields

        #region Private Fields

        private DoubleRange XRange { get; set; }
        private DoubleRange YRange { get; set; }
        private  int index = 0;
        private float strokeWidth;
        private Color? strokeColor;
        private double horizontalErrorValue;
        private double verticalErrorValue;
        List<Point> leftPointCollection = new();
        List<Point> rightPointCollection = new();
        List<Point> topPointCollection = new();
        List<Point> bottomPointCollection = new();
        #endregion

        #endregion

        #region Properties

        #region Internal Properties

        internal List<ErrorSegmentPoint[]> ErrorSegmentPoints { get; set; }

        #endregion

        #endregion


        #region Constructor

        #region Public  Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorBarSegment"/> class.
        /// </summary>
        public ErrorBarSegment()
        {
            ErrorSegmentPoints = new List<ErrorSegmentPoint[]>();
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Methods

        internal override void SetData(IList xList, IList yList)
        {
            var xValues = (IList<double>)xList;
            var yValues = (IList<double>)yList;
            index = xValues.Count;
            
            if (Series is ErrorBarSeries errorBarSeries)
            {
                var horSdValue = GetSdErrorValue(xValues);
                var verSdValue = GetSdErrorValue(yValues);
                
                if (errorBarSeries.Type == ErrorBarType.StandardDeviation || errorBarSeries.Type == ErrorBarType.StandardError)
                {
                    horizontalErrorValue = horSdValue[1];
                    verticalErrorValue = verSdValue[1];
                }
                else if (errorBarSeries.Type == ErrorBarType.Custom)
                {
                    horizontalErrorValue = errorBarSeries.HorizontalErrorValues!.Max();
                    verticalErrorValue = errorBarSeries.VerticalErrorValues!.Max();
                }
                else
                {
                    horizontalErrorValue = errorBarSeries.HorizontalErrorValue;
                    verticalErrorValue = errorBarSeries.VerticalErrorValue;
                }

                double leftPointMin = xValues.Min() - horizontalErrorValue;
                double leftPointMax = xValues.Max() - horizontalErrorValue;
                double rightPointMin = xValues.Min() + horizontalErrorValue;
                double rightPointMax = xValues.Max() + horizontalErrorValue;
                double bottomPointMin = yValues.Min() - verticalErrorValue;
                double bottomPointMax = yValues.Max() - verticalErrorValue;
                double topPointMin = yValues.Min() + verticalErrorValue;
                double topPointMax = yValues.Max() + verticalErrorValue;
                
                XRange = new DoubleRange(Math.Min(leftPointMin, rightPointMin), Math.Max(leftPointMax, rightPointMax));
                YRange = new DoubleRange(Math.Min(bottomPointMin, topPointMin), Math.Max(bottomPointMax, topPointMax));
                
                errorBarSeries.XRange += XRange;
                errorBarSeries.YRange += YRange;
                
                for (int i = 0; i < index; i++)
                {
                    if (errorBarSeries.Type == ErrorBarType.Percentage)
                    {
                        if ((errorBarSeries.ActualXAxis is DateTimeAxis) || (errorBarSeries.ActualYAxis is CategoryAxis))
                        {
                            horizontalErrorValue = (i + 1) * (errorBarSeries.HorizontalErrorValue / 100);
                            verticalErrorValue = yValues[i] * (errorBarSeries.VerticalErrorValue / 100);
                        }
                        else
                        {
                            horizontalErrorValue = xValues[i] * (errorBarSeries.HorizontalErrorValue / 100);
                            verticalErrorValue = yValues[i] * (errorBarSeries.VerticalErrorValue / 100);
                        }
                    }
                    else if (errorBarSeries.Type == ErrorBarType.Custom)
                    {
                        horizontalErrorValue = errorBarSeries.HorizontalErrorValues?.Count() > 0 ? errorBarSeries.HorizontalErrorValues[i] : 0;
                        verticalErrorValue = errorBarSeries.VerticalErrorValues?.Count() > 0 ? errorBarSeries.VerticalErrorValues[i] : 0;
                    }
                    
                    if (errorBarSeries.Type == ErrorBarType.StandardDeviation)
                    {
                        if (errorBarSeries.HorizontalDirection == ErrorBarDirection.Plus)
                        {
                            leftPointCollection.Add(new Point(horSdValue[0], yValues[i]));
                            rightPointCollection.Add(new Point(horSdValue[0] + horizontalErrorValue, yValues[i]));
                        }
                        else if (errorBarSeries.HorizontalDirection == ErrorBarDirection.Minus)
                        {
                            leftPointCollection.Add(new Point(horSdValue[0] - horizontalErrorValue, yValues[i]));
                            rightPointCollection.Add(new Point(horSdValue[0], yValues[i]));
                        }
                        else
                        {
                            leftPointCollection.Add(new Point(horSdValue[0] - horizontalErrorValue, yValues[i]));
                            rightPointCollection.Add(new Point(horSdValue[0] + horizontalErrorValue, yValues[i]));
                        }
                        
                        if (errorBarSeries.VerticalDirection == ErrorBarDirection.Plus)
                        {
                            bottomPointCollection.Add(new Point(xValues[i], verSdValue[0]));
                            topPointCollection.Add(new Point(xValues[i], verSdValue[0] + verticalErrorValue));
                        }
                        else if (errorBarSeries.VerticalDirection == ErrorBarDirection.Minus)
                        {
                            bottomPointCollection.Add(new Point(xValues[i], verSdValue[0] - verticalErrorValue));
                            topPointCollection.Add(new Point(xValues[i], verSdValue[0]));
                        }
                        else
                        {
                            bottomPointCollection.Add(new Point(xValues[i], verSdValue[0] - verticalErrorValue));
                            topPointCollection.Add(new Point(xValues[i], verSdValue[0] + verticalErrorValue));
                        }
                    }
                    else
                    {
                        if (errorBarSeries.HorizontalDirection == ErrorBarDirection.Plus)
                        {
                            leftPointCollection.Add(new Point(xValues[i], yValues[i]));
                            rightPointCollection.Add(new Point(xValues[i] + horizontalErrorValue, yValues[i]));
                        }
                        else if (errorBarSeries.HorizontalDirection == ErrorBarDirection.Minus)
                        {
                            leftPointCollection.Add(new Point(xValues[i] - horizontalErrorValue, yValues[i]));
                            rightPointCollection.Add(new Point(xValues[i], yValues[i]));
                        }
                        else
                        {
                            leftPointCollection.Add(new Point(xValues[i] - horizontalErrorValue, yValues[i]));
                            rightPointCollection.Add(new Point(xValues[i] + horizontalErrorValue, yValues[i]));
                        }
                        
                        if (errorBarSeries.VerticalDirection == ErrorBarDirection.Plus)
                        {
                            bottomPointCollection.Add(new Point(xValues[i], yValues[i]));
                            topPointCollection.Add(new Point(xValues[i], yValues[i] + verticalErrorValue));
                        }
                        else if (errorBarSeries.VerticalDirection == ErrorBarDirection.Minus)
                        {
                            bottomPointCollection.Add(new Point(xValues[i], yValues[i] - verticalErrorValue));
                            topPointCollection.Add(new Point(xValues[i], yValues[i]));
                        }
                        else
                        {
                            bottomPointCollection.Add(new Point(xValues[i], yValues[i] - verticalErrorValue));
                            topPointCollection.Add(new Point(xValues[i], yValues[i] + verticalErrorValue));
                        }
                    }
                }
            }
        }
        #endregion

        #region Protected Internal Methods

        /// <summary>
        /// 
        /// </summary>
        protected internal override void OnLayout()
        {
            var errorBarSeries = Series as ErrorBarSeries;
            if (errorBarSeries != null)
            {
                ErrorSegmentPoints.Clear();
                for (int i = 0; i < index; i++)
                {
                    float horCapLinesize = errorBarSeries.HorizontalCapLineStyle != null
                        ? (float)(errorBarSeries.HorizontalCapLineStyle.CapLineSize) : 10;
                    float verLineCapSize = errorBarSeries.VerticalCapLineStyle != null
                        ? (float)(errorBarSeries.VerticalCapLineStyle.CapLineSize) : (float)(10);
                    ErrorSegmentPoint[] errorSegment = new ErrorSegmentPoint[2];
                    
                    if (!(double.IsNaN(leftPointCollection[i].Y) || double.IsNaN(rightPointCollection[i].Y)))
                    {
                        float horRightPoint1 = errorBarSeries.TransformToVisibleX(rightPointCollection[i].X, rightPointCollection[i].Y);
                        float horRightPoint2 = errorBarSeries.TransformToVisibleY(rightPointCollection[i].X, rightPointCollection[i].Y);
                        float horLeftPoint3 = errorBarSeries.TransformToVisibleX(leftPointCollection[i].X, leftPointCollection[i].Y);
                        float horLeftPoint4 = errorBarSeries.TransformToVisibleY(leftPointCollection[i].X, leftPointCollection[i].Y);
                        errorSegment[0] = new ErrorSegmentPoint();
                        errorSegment[0].X1 = horRightPoint1;
                        errorSegment[0].Y1 = horRightPoint2;
                        errorSegment[0].X2 = horLeftPoint3;
                        errorSegment[0].Y2 = horLeftPoint4;
                        //Horizontal Right Cap
                        RectF rightRectangle = new RectF(horRightPoint1 - (horCapLinesize / 2), horRightPoint2 - (horCapLinesize / 2), horCapLinesize, horCapLinesize);
                        errorSegment[0].RightRect = rightRectangle;
                        RectF leftRectangle = new RectF(horLeftPoint3 - (horCapLinesize / 2), horLeftPoint4 - (horCapLinesize / 2), horCapLinesize, horCapLinesize);
                        errorSegment[0].LeftRect = leftRectangle;
                    }

                    if (!(double.IsNaN(topPointCollection[i].Y) || double.IsNaN(bottomPointCollection[i].Y)))
                    {
                        float verBottomPoint1 = errorBarSeries.TransformToVisibleX(bottomPointCollection[i].X, bottomPointCollection[i].Y);
                        float verBottomPoint2 = errorBarSeries.TransformToVisibleY(bottomPointCollection[i].X, bottomPointCollection[i].Y);
                        float verTopPoint1 = errorBarSeries.TransformToVisibleX(topPointCollection[i].X, topPointCollection[i].Y);
                        float verTopPoint2 = errorBarSeries.TransformToVisibleY(topPointCollection[i].X, topPointCollection[i].Y);
                        errorSegment[1] = new ErrorSegmentPoint();
                        errorSegment[1].X1 = verBottomPoint1;
                        errorSegment[1].Y1 = verBottomPoint2;
                        errorSegment[1].X2 = verTopPoint1;
                        errorSegment[1].Y2 = verTopPoint2;
                        RectF bottomRectangle = new RectF(verBottomPoint1 - (verLineCapSize / 2), verBottomPoint2 - (verLineCapSize / 2), verLineCapSize, verLineCapSize);
                        errorSegment[1].BottomRect = bottomRectangle;
                        RectF topRectangle = new RectF(verTopPoint1 - (verLineCapSize / 2), verTopPoint2 - (verLineCapSize / 2), verLineCapSize, verLineCapSize);
                        errorSegment[1].TopRect = topRectangle;
                    }

                    ErrorSegmentPoints.Add(errorSegment);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canvas"></param>
        protected internal override void Draw(ICanvas canvas)
        {
            var errorBarSeries = Series as ErrorBarSeries;
            if (errorBarSeries == null || errorBarSeries.ChartArea == null)
            {
                return;
            }

            var isTransposed = errorBarSeries.ChartArea.IsTransposed;

            canvas.Alpha = Opacity;

            for (int i = 0; i < index; i++)
            {
                if (errorBarSeries.Mode == ErrorBarMode.Horizontal)
                {
                    DrawErrorBar(canvas, i, false, errorBarSeries.HorizontalLineStyle, errorBarSeries.HorizontalCapLineStyle, isTransposed);
                }
                else if (errorBarSeries.Mode == ErrorBarMode.Vertical)
                {
                    DrawErrorBar(canvas, i, true, errorBarSeries.VerticalLineStyle, errorBarSeries.VerticalCapLineStyle, isTransposed);
                }
                else
                {
                    DrawErrorBar(canvas, i, false, errorBarSeries.HorizontalLineStyle, errorBarSeries.HorizontalCapLineStyle, isTransposed);
                    DrawErrorBar(canvas, i, true, errorBarSeries.VerticalLineStyle, errorBarSeries.VerticalCapLineStyle, isTransposed);
                }
            }
        }

        #endregion

        #region Private Methods

        private void DrawErrorBar(ICanvas canvas, int index, bool isVertical, ErrorBarLineStyle? lineStyle, ErrorBarCapLineStyle? capStyle, bool isTransposed)
        {
            strokeColor = lineStyle != null ? lineStyle.Stroke.ToColor() : Fill.ToColor();
            strokeWidth = lineStyle != null ? (float)lineStyle.StrokeWidth : (float)StrokeWidth;

            int vertical = isVertical ? 1 : 0;

            DrawLine(canvas, index, vertical, strokeColor, strokeWidth);

            strokeColor = Fill.ToColor();
            strokeWidth = (float)StrokeWidth;

            if (capStyle == null)
            {
                DrawCapLine(canvas, index, vertical, strokeColor, strokeWidth, isTransposed);
            }
            else if (capStyle.IsVisible)
            {
                strokeColor = capStyle.Stroke.ToColor();
                strokeWidth = (float)capStyle.StrokeWidth;
                if (lineStyle != null)
                {
                    canvas.StrokeLineCap = lineStyle.StrokeCap switch
                    {
                        ErrorBarStrokeCap.Square => LineCap.Square,
                        ErrorBarStrokeCap.Round => LineCap.Round,
                        _ => LineCap.Butt
                    };
                }

                DrawCapLine(canvas, index, vertical, strokeColor, strokeWidth, isTransposed);
            }
        }
       
        private void DrawLine(ICanvas canvas, int i, int j, Color strokeColor, float strokeWidth)
        {
            canvas.StrokeColor = strokeColor;
            canvas.StrokeSize = strokeWidth;
            canvas.DrawLine(ErrorSegmentPoints[i][j].X1, ErrorSegmentPoints[i][j].Y1, ErrorSegmentPoints[i][j].X2, ErrorSegmentPoints[i][j].Y2);
        }

        private void DrawCapLine(ICanvas canvas, int i, int j, Color strokeColor, float strokeWidth, bool isTransposed)
        {
            canvas.StrokeColor = strokeColor;
            canvas.StrokeSize = strokeWidth;

            var shapeType = j == 0 ? isTransposed ? ShapeType.HorizontalLine : ShapeType.VerticalLine : isTransposed ? ShapeType.VerticalLine : ShapeType.HorizontalLine;

            var rectTop = j == 0 ? ErrorSegmentPoints[i][j].LeftRect : ErrorSegmentPoints[i][j].BottomRect;
            var rectBottom = j == 0 ? ErrorSegmentPoints[i][j].RightRect : ErrorSegmentPoints[i][j].TopRect;

            canvas.DrawShape(rectTop, shapeType, HasStroke, false);
            canvas.DrawShape(rectBottom, shapeType, HasStroke, false);
        }

        /// <summary>
        /// Standard Deviation Methods for the xValues and yValues
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        private double[] GetSdErrorValue(IList<double> values)
        {
            ErrorBarSeries? errorBarSeries = Series as ErrorBarSeries;
            var sum = values.Sum();
            var mean = sum / values.Count;
            var dev = new List<double>();
            var sQDev = new List<double>();
            for (var i = 0; i < values.Count; i++)
            {
                dev.Add(values[i] - mean);
                sQDev.Add(dev[i] * dev[i]);
            }
            
            var sumSqDev = sQDev.Sum(x => x);
            var valueDoubles = new double[2];
            var sDValue = Math.Sqrt(sumSqDev / (values.Count - 1));
            var sDerrorValue = sDValue / Math.Sqrt(values.Count);
            valueDoubles[0] = mean;
            valueDoubles[1] = errorBarSeries?.Type == ErrorBarType.StandardDeviation ? sDValue : sDerrorValue;
            return valueDoubles;
        }
    }

    #endregion

    #endregion

    /// <summary>
    /// 
    /// </summary>
    internal class ErrorSegmentPoint
    {
        #region Properties

        #region Public  Properties

        public float X1 { get; set; }
        public float Y1 { get; set; }
        public float X2 { get; set; }
        public float Y2 { get; set; }
        public RectF LeftRect { get; set; }
        public RectF RightRect { get; set; }
        public RectF TopRect { get; set; }
        public RectF BottomRect { get; set; }

        #endregion

        #endregion
    }
}
