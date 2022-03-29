using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts 
{
    /// <summary>
    /// 
    /// </summary>
    public class CartesianDataLabelSettings : ChartDataLabelSettings
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty BarAlignmentProperty =
            BindableProperty.Create(nameof(BarAlignment), typeof(DataLabelAlignment), typeof(CartesianDataLabelSettings), DataLabelAlignment.Top, BindingMode.Default, null, null);

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public CartesianDataLabelSettings()
        {
            IsNeedDataLabelMeasure.Add(nameof(BarAlignment));
            LabelStyle = new ChartDataLabelStyle() { FontSize = 12, Margin = new Thickness(5) };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public DataLabelAlignment BarAlignment
        {
            get { return (DataLabelAlignment)GetValue(BarAlignmentProperty); }
            set { SetValue(BarAlignmentProperty, value); }
        }

        #endregion

        #region Methods

        #region Internal Methods

        //Get the data label position.
        internal PointF CalculateDataLabelPoint(XYDataSeries xyDataSeries, ChartSegment dataLabel, PointF labelPoint, ChartDataLabelStyle labelStyle)
        {
            SizeF labelSize = labelStyle.MeasureLabel(dataLabel.DataLabel != null ? dataLabel.DataLabel : string.Empty);
            float padding = (float)labelStyle.LabelPadding;

            return xyDataSeries.GetDataLabelPosition(dataLabel, labelSize, labelPoint, padding);
        }

        //Get the label position for column series.
        internal PointF GetLabelPositionForRectangularSeries(XYDataSeries xyDataSeries, int index, SizeF labelSize, PointF labelPoint, float padding, DataLabelAlignment barAlignment)
        {
            if (xyDataSeries.ActualYAxis == null) return labelPoint;

            ChartDataLabelStyle labelstyle = LabelStyle;
            float x = labelPoint.X, y = labelPoint.Y;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;
            bool isPositive = xyDataSeries.YValues[index] >= 0;

            x = x + ((float)labelstyle.OffsetX);
            y = y + ((float)labelstyle.OffsetY);

            if (LabelPlacement == DataLabelPlacement.Inner || LabelPlacement == DataLabelPlacement.Auto)
            {
                if (xyDataSeries.ActualYAxis.IsInversed)
                {
                    y = isPositive ? y - (labelSize.Height / 2) - padding - borderWidth : y + (labelSize.Height / 2) + padding + borderWidth;
                }
                else
                {
                    y = isPositive ? y + (labelSize.Height / 2) + padding + borderWidth : y - (labelSize.Height / 2) - padding - borderWidth;
                }

                if (LabelPlacement == DataLabelPlacement.Auto)
                {
                    if ((y - (labelSize.Height / 2)) < 0)
                    {
                        if (xyDataSeries.ActualYAxis.IsInversed)
                        {
                            y = isPositive ? labelPoint.Y - (labelSize.Height / 2) - padding : labelPoint.Y + (labelSize.Height / 2) + padding;
                        }
                        else
                        {
                            y = isPositive ? labelPoint.Y + (labelSize.Height / 2) + padding : labelPoint.Y - (labelSize.Height / 2) - padding;
                        }
                    }
                    else if ((y + (labelSize.Height / 2)) > xyDataSeries.AreaBounds.Height)
                    {
                        if (xyDataSeries.ActualYAxis.IsInversed)
                        {
                            y = isPositive ? labelPoint.Y - (labelSize.Height / 2) - padding : labelPoint.Y + (labelSize.Height / 2) + padding;
                        }
                        else
                        {
                            y = isPositive ? labelPoint.Y + (labelSize.Height / 2) + padding : labelPoint.Y - (labelSize.Height / 2) - padding;
                        }
                    }

                    PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                    x = point.X;
                    y = point.Y;
                }
                else if (labelPoint.Y + (labelSize.Height / 2) >= xyDataSeries.AreaBounds.Height)
                {
                    y = labelPoint.Y - (labelSize.Height / 2) - padding - borderWidth;
                }
                else if (barAlignment == DataLabelAlignment.Bottom)
                {
                    y = isPositive ? labelPoint.Y - (labelSize.Height / 2) - padding - borderWidth : labelPoint.Y + (labelSize.Height / 2) + padding + borderWidth;
                }
            }
            else if (LabelPlacement == DataLabelPlacement.Outer)
            {
                if (xyDataSeries.ActualYAxis.IsInversed)
                {
                    y = isPositive ? y + (labelSize.Height / 2) + padding + borderWidth : y - (labelSize.Height / 2) - padding - borderWidth;
                }
                else
                {
                    y = isPositive ? y - (labelSize.Height / 2) - padding - borderWidth : y + (labelSize.Height / 2) + padding + borderWidth;
                }

                if (barAlignment == DataLabelAlignment.Bottom)
                {
                    y = isPositive ? labelPoint.Y + (labelSize.Height / 2) + padding + borderWidth : labelPoint.Y - (labelSize.Height / 2) - padding - borderWidth;
                }
            }

            return new PointF(x, y);
        }

        //Get the label position for transposed column series.
        internal PointF GetLabelPositionForTransposedRectangularSeries(XYDataSeries xyDataSeries, int index, SizeF labelSize, PointF labelPoint, float padding, DataLabelAlignment barAlignment)
        {
            if (xyDataSeries.ActualYAxis == null) return labelPoint;

            ChartDataLabelStyle labelstyle = LabelStyle;
            float x = labelPoint.X, y = labelPoint.Y;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;
            bool isPositive = xyDataSeries.YValues[index] >= 0;

            x = x + ((float)labelstyle.OffsetX);
            y = y + ((float)labelstyle.OffsetY);

            if (LabelPlacement == DataLabelPlacement.Inner || LabelPlacement == DataLabelPlacement.Auto)
            {
                if (xyDataSeries.ActualYAxis.IsInversed)
                {
                    x = isPositive ? x + (labelSize.Width / 2) + padding + borderWidth : x - (labelSize.Width / 2) - padding - borderWidth;
                }
                else
                {
                    x = isPositive ? x - (labelSize.Width / 2) - padding - borderWidth : x + (labelSize.Width / 2) + padding + borderWidth;
                }

                if (LabelPlacement == DataLabelPlacement.Auto)
                {
                    if ((x - (labelSize.Width / 2)) <= 0)
                    {
                        if (xyDataSeries.ActualYAxis.IsInversed)
                        {
                            x = isPositive ? labelPoint.X + (labelSize.Width / 2) + padding : labelPoint.X - (labelSize.Width / 2) - padding;
                        }
                        else
                        {
                            x = isPositive ? labelPoint.X - (labelSize.Width / 2) - padding : labelPoint.X + (labelSize.Width / 2) + padding;
                        }
                    }
                    else if ((x + (labelSize.Width / 2)) >= xyDataSeries.AreaBounds.Width)
                    {
                        if (xyDataSeries.ActualYAxis.IsInversed)
                        {
                            x = isPositive ? labelPoint.X + (labelSize.Width / 2) + padding : labelPoint.X - (labelSize.Width / 2) - padding;
                        }
                        else
                        {
                            x = isPositive ? labelPoint.X - (labelSize.Width / 2) - padding : labelPoint.X + (labelSize.Width / 2) + padding;
                        }
                    }

                    PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                    x = point.X;
                    y = point.Y;
                }
                else if (labelPoint.X - (labelSize.Width / 2) <= 0)
                {
                    x = labelPoint.X + (labelSize.Width / 2) + padding + borderWidth;
                }
                else if (barAlignment == DataLabelAlignment.Bottom)
                {
                    x = isPositive ? labelPoint.X + (labelSize.Width / 2) + padding + borderWidth : labelPoint.X - (labelSize.Width / 2) - padding - borderWidth;
                }
            }
            else if (LabelPlacement == DataLabelPlacement.Outer)
            {
                if (xyDataSeries.ActualYAxis.IsInversed)
                {
                    x = isPositive ? x - (labelSize.Width / 2) - padding - borderWidth : x + (labelSize.Width / 2) + padding + borderWidth;
                }
                else
                {
                    x = isPositive ? x + (labelSize.Width / 2) + padding + borderWidth : x - (labelSize.Width / 2) - padding - borderWidth;
                }

                if (labelPoint.X + (labelSize.Width / 2) >= xyDataSeries.AreaBounds.Width)
                {
                    x = labelPoint.X - (labelSize.Width / 2) - padding - borderWidth;
                }
                else if (barAlignment == DataLabelAlignment.Bottom)
                {
                    x = isPositive ? labelPoint.X - (labelSize.Width / 2) - padding - borderWidth : labelPoint.X + (labelSize.Width / 2) + padding + borderWidth;
                }
            }

            return new PointF(x, y);
        }

        //Get the label position for line, spline series.
        internal PointF GetLabelPositionForContinuousSeries(XYDataSeries xyDataSeries, int index, SizeF labelSize, PointF labelPoint, float padding)
        {
            bool isTop = IsTopWithLabelIndex(xyDataSeries, index);

            ChartDataLabelStyle labelstyle = LabelStyle;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;
            float x = labelPoint.X;
            float y = labelPoint.Y;

            x = x + ((float)labelstyle.OffsetX);
            y = y + ((float)labelstyle.OffsetY);
            var halfLabelSizeHeight = labelSize.Height / 2;

            if (LabelPlacement == DataLabelPlacement.Outer || LabelPlacement == DataLabelPlacement.Auto)
            {
                y = isTop ? y - halfLabelSizeHeight - padding - borderWidth : y + halfLabelSizeHeight + padding + borderWidth;

                if (LabelPlacement == DataLabelPlacement.Auto)
                {
                    PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                    x = point.X;
                    y = point.Y;
                }
            }
            else if (LabelPlacement == DataLabelPlacement.Inner)
            {
                y = isTop ? y + halfLabelSizeHeight + padding + borderWidth : y - halfLabelSizeHeight - padding - borderWidth;
            }

            return new PointF(x, y);
        }

        //Get the label position for scatter series.
        internal PointF GetLabelPositionForSeries(XYDataSeries xyDataSeries, SizeF labelSize, PointF labelPoint, float padding, SizeF scatterSize)
        {
            ChartDataLabelStyle labelstyle = LabelStyle;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;
            float radius = (float)scatterSize.Height / 2;
            float x = labelPoint.X;
            float y = labelPoint.Y;
            x = x + ((float)labelstyle.OffsetX);
            y = y + ((float)labelstyle.OffsetY);

            if (LabelPlacement == DataLabelPlacement.Outer)
            {
                y = y - radius - (labelSize.Height / 2) - padding - borderWidth;
            }
            else if (LabelPlacement == DataLabelPlacement.Inner)
            {
                y = y - radius + (labelSize.Height / 2) + padding + borderWidth;
            }
            else if (LabelPlacement == DataLabelPlacement.Auto)
            {
                y = y - radius - (labelSize.Height / 2) - padding - borderWidth;

                PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                x = point.X;
                y = point.Y;
            }

            return new PointF(x, y);
        }

        //Get the label position for area series.
        internal PointF GetLabelPositionForAreaSeries(XYDataSeries xyDataSeries, ChartSegment dataLabel, SizeF labelSize, PointF labelPoint, float padding)
        {
            if (xyDataSeries.ChartArea == null) return labelPoint;

            var index = dataLabel.Index;
            ChartDataLabelStyle labelstyle = LabelStyle;
            float x = labelPoint.X, y = labelPoint.Y;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;
            bool isPositive = xyDataSeries.YValues[index] >= 0;

            x = x + ((float)labelstyle.OffsetX);
            y = y + ((float)labelstyle.OffsetY);

            if (LabelPlacement == DataLabelPlacement.Outer)
            {
                //Todo: Need to remove this Chart.IsTransposed code in cartesian layout changes.
                if (xyDataSeries.ChartArea.IsTransposed)
                {
                    x = isPositive ? x + (labelSize.Width / 2) + padding + borderWidth : x - (labelSize.Width / 2) - padding - borderWidth;
                }
                else
                {
                    y = isPositive ? y - (labelSize.Height / 2) - padding - borderWidth : y + (labelSize.Height / 2) + padding + borderWidth;
                }

                if (LabelPlacement == DataLabelPlacement.Auto)
                {
                    PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                    x = point.X;
                    y = point.Y;
                }
            }
            else if (LabelPlacement == DataLabelPlacement.Inner)
            {
                if (xyDataSeries.ChartArea.IsTransposed)
                {
                    x = isPositive ? x - (labelSize.Width / 2) - padding - borderWidth : x + (labelSize.Width / 2) + padding + borderWidth;
                }
                else
                {
                    y = isPositive ? y + (labelSize.Height / 2) + padding + borderWidth : y - (labelSize.Height / 2) - padding - borderWidth;
                }

                if (LabelPlacement == DataLabelPlacement.Auto)
                {
                    PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                    x = point.X;
                    y = point.Y;
                }
            }

            return new PointF(x, y);
        }

        #endregion

        #region Private Methods

        private bool IsTopWithLabelIndex(XYDataSeries xyDataSeries, int index)
        {
            IList<double> yvalues = xyDataSeries.YValues;

            double yValue = yvalues[index];
            int count = yvalues.Count;
            double nextYValue = 0.0;
            double previousYValue = 0.0;

            if ((count - 1) > index)
            {
                nextYValue = yvalues[index + 1];
            }

            if (index > 0)
            {
                previousYValue = yvalues[index - 1];
            }

            if (count > 1)
            {
                if (index == 0)
                {
                    if (double.IsNaN(nextYValue))
                    {
                        return true;
                    }
                    else
                    {
                        return yValue > nextYValue;
                    }
                }

                if (count - 1 == index)
                {
                    if (double.IsNaN(previousYValue))
                    {
                        return true;
                    }
                    else
                    {
                        return yValue > previousYValue;
                    }
                }
                else
                {
                    if (double.IsNaN(previousYValue) && double.IsNaN(nextYValue))
                    {
                        return true;
                    }
                    else if (double.IsNaN(nextYValue))
                    {
                        return !(previousYValue > yValue);
                    }
                    else if (double.IsNaN(previousYValue))
                    {
                        return !(nextYValue > yValue);
                    }
                    else
                    {
                        double previousXValue = index - 1;
                        double nextXValue = index + 1;
                        double xValue = index;

                        double slope = (nextYValue - previousYValue) / (nextXValue - previousXValue);
                        double yIntercept = nextYValue - (slope * nextXValue);
                        double intersectY = (slope * xValue) + yIntercept;
                        return intersectY < yValue;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        #endregion

        #endregion
    }
}
