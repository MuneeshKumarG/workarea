using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents a segment of a range column chart.
    /// </summary>
    public class RangeColumnSegment : ColumnSegment
    {
        #region Fields

        private double x1, x2, y1, y2, xValue;
        internal PointF[] LabelPositionPoints = new PointF[2];
        internal string[] DataLabels = new string[2];

        #endregion

        #region Methods

        #region Internal Methods

        internal override void SetData(double[] values)
        {
            if (Series is not RangeColumnSeries series)
            {
                return;
            }

            x1 = values[0];
            x2 = values[1];
            y1 = values[2];
            y2 = values[3];
            xValue = values[4];
            Empty = double.IsNaN(x1) || double.IsNaN(x2) || double.IsNaN(y1) || double.IsNaN(y2);
            series.XRange += DoubleRange.Union(xValue);
            series.YRange += new DoubleRange(y1, y2);
        }

        internal override void OnDataLabelLayout()
        {
            if (Series is RangeColumnSeries series)
            {
                CalculateDataLabelsPosition(xValue, y1, y2, series);
            }
        }

        #endregion

        #region Protected Internal Methods

        /// <inheritdoc/>
        protected internal override void Draw(ICanvas canvas)
        {
            if (Series is not RangeColumnSeries rangeColumn || rangeColumn.ActualXAxis == null)
            {
                return;
            }

            if (!float.IsNaN(Left) && !float.IsNaN(Top) && !float.IsNaN(Right) && !float.IsNaN(Bottom))
            {
                if (rangeColumn.CanAnimate())
                {
                    Layout();
                }

                canvas.StrokeSize = (float)StrokeWidth;
                canvas.StrokeColor = Stroke.ToColor();
                canvas.StrokeDashPattern = StrokeDashArray?.ToFloatArray();
                canvas.Alpha = Opacity;
                CornerRadius cornerRadius = rangeColumn.CornerRadius;

                //segment Drawing
                var rect = new Rect() { Left = Left, Top = Top, Right = Right, Bottom = Bottom };
                var actualCrossingValue = rangeColumn.ActualXAxis.ActualCrossingValue;
                canvas.SetFillPaint(Fill, rect);
                if (cornerRadius.TopLeft > 0 || cornerRadius.TopRight > 0 || cornerRadius.BottomLeft > 0 || cornerRadius.BottomRight > 0)
                {
                    if (y1 < (double.IsNaN(actualCrossingValue) ? 0 : actualCrossingValue))
                    {
                        canvas.FillRoundedRectangle(rect, cornerRadius.BottomLeft, cornerRadius.BottomRight, cornerRadius.TopLeft, cornerRadius.TopRight);
                    }
                    else
                    {
                        canvas.FillRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
                    }
                }
                else
                {
                    canvas.FillRectangle(rect);
                }

                //Draw Stroke
                if (HasStroke)
                {
                    if (cornerRadius.TopLeft > 0 || cornerRadius.TopRight > 0 || cornerRadius.BottomLeft > 0 || cornerRadius.BottomRight > 0)
                    {
                        if (y1 < (double.IsNaN(actualCrossingValue) ? 0 : actualCrossingValue))
                        {
                            canvas.DrawRoundedRectangle(rect, cornerRadius.BottomLeft, cornerRadius.BottomRight, cornerRadius.TopLeft, cornerRadius.TopRight);
                        }
                        else
                        {
                            canvas.DrawRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
                        }
                    }
                    else
                    {
                        canvas.DrawRectangle(rect);
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected internal override void OnLayout()
        {
            Layout();
        }

        #endregion

        #region Private Methods

        private void Layout()
        {
            RangeColumnSeries? series = Series as RangeColumnSeries;
            var xAxis = series?.ActualXAxis;
            if (series == null || series.ChartArea == null || xAxis == null)
            {
                return;
            }

            var start = Math.Floor(xAxis.VisibleRange.Start);
            var end = Math.Ceiling(xAxis.VisibleRange.End);
            
            double y1Value = y1;
            double y2Value = y2;
            var midY = ((y1 + y2) / 2);

            if (series.CanAnimate())
            {
                var animationValue = series.AnimationValue;
                
                if (!series.XRange.Equals(series.PreviousXRange) || (float.IsNaN(PreviousY1) && float.IsNaN(PreviousY2)))
                {
                    y1Value = midY + ((y1Value - midY) * series.AnimationValue);
                    y2Value = midY - ((midY - y2Value) * series.AnimationValue);
                }
                else
                {
                    y1Value = GetColumnDynamicAnimationValue(animationValue, PreviousY1, y1);
                    y2Value = GetColumnDynamicAnimationValue(animationValue, PreviousY2, y2);
                }
            }

            Left = Top = Right = Bottom = float.NaN;
            if (x1 <= end && x2 >= start)
            {
                Left = series.TransformToVisibleX(x1, y1Value);
                Top = series.TransformToVisibleY(x1, y1Value);
                Right = series.TransformToVisibleX(x2, y2Value);
                Bottom = series.TransformToVisibleY(x2, y2Value);

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

                Y1 = (float)y1Value;
                Y2 = (float)y2Value;
            }
            else
            {
                this.Left = float.NaN;
            }

            SegmentBounds = new RectF(Left, Top, Right - Left, Bottom - Top);
        }

        private void CalculateDataLabelsPosition(double xvalue, double high, double low, RangeColumnSeries series)
        {
            var dataLabelSettings = series.DataLabelSettings;
            IsEmpty = double.IsNaN(high) && double.IsNaN(low);
            InVisibleRange = series.IsDataInVisibleRange(xvalue, high) && series.IsDataInVisibleRange(xvalue, low);
            double x = xvalue, x1 = xvalue, y = series.GetDataLabelPositionAtIndex(Index, high), y1 = series.GetDataLabelPositionAtIndex(Index, low);
            series.CalculateDataPointPosition(Index, ref x, ref y);
            series.CalculateDataPointPosition(Index, ref x1, ref y1);
            PointF highPoint = new PointF((float)x, (float)y);
            PointF lowPoint = new PointF((float)x1, (float)y1);
            DataLabels[0] = dataLabelSettings.GetLabelContent(high);
            DataLabels[1] = dataLabelSettings.GetLabelContent(low);
            LabelPositionPoints[0] = dataLabelSettings.CalculateDataLabelPoint(series, this, highPoint, dataLabelSettings.LabelStyle, "HighType");
            LabelPositionPoints[1] = dataLabelSettings.CalculateDataLabelPoint(series, this, lowPoint, dataLabelSettings.LabelStyle, "LowType");
        }

        private float GetColumnDynamicAnimationValue(float animationValue, double oldValue, double currentValue)
        {
            if (!double.IsNaN(oldValue) && !double.IsNaN(currentValue))
            {
                return (float)((currentValue > oldValue) ?
                    oldValue + ((currentValue - oldValue) * animationValue)
                    : oldValue - ((oldValue - currentValue) * animationValue));
            }
            else
            {
                return double.IsNaN(oldValue) ? (float)currentValue * animationValue : (float)(oldValue - (oldValue * animationValue));
            }
        }

        #endregion

        #endregion
    }
}