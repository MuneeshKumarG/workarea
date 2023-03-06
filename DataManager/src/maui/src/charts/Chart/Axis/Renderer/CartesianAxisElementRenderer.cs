using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    internal class CartesianAxisElementRenderer : ILayoutCalculator
    {
        #region Fields
        private SizeF desiredSize;

        private double left, top;

        private ChartAxis chartAxis;

        bool ILayoutCalculator.IsVisible { get; set; } = true;

        #endregion

        #region Constructor
        public CartesianAxisElementRenderer(ChartAxis axis)
        {
            chartAxis = axis;
        }
        #endregion

        #region Methods
        public void OnDraw(ICanvas canvas, Size finalSize)
		{
			if (chartAxis.AxisLineStyle.CanDraw())
            {
                canvas.SaveState();
                DrawAxisLine(canvas);
                canvas.RestoreState();
            }

			if (chartAxis.MajorTickStyle.CanDraw())
            {
                canvas.SaveState();
                DrawTicks(canvas, chartAxis.MajorTickStyle, chartAxis.TickPositions, false);
                canvas.RestoreState();
            }

            var rangeAxis = chartAxis as RangeAxisBase;
			if (chartAxis.SmallTickRequired && rangeAxis != null && rangeAxis.MinorTickStyle.CanDraw())
            {
                canvas.SaveState();
                DrawTicks(canvas, rangeAxis.MinorTickStyle, rangeAxis.SmallTickPoints, true);
                canvas.RestoreState();
            }
		}

        public double GetLeft()
        {
            return left;
        }

        public void SetLeft(double _left)
        {
            left = _left;
        }

        public double GetTop()
        {
            return top;
        }

        public void SetTop(double _top)
        {
            top = _top;
        }

        public Size GetDesiredSize()
        {
            return desiredSize;
        }

        public Size Measure(Size availableSize)
        {
            SizeF size;
            double smallTickLineSize = 0;

            if (chartAxis.SmallTickRequired)
            {
                smallTickLineSize = ((RangeAxisBase)chartAxis).MinorTickStyle.TickSize;
            }

            if (chartAxis.IsVertical)
            {
                size = new Size(Math.Max(chartAxis.MajorTickStyle.TickSize, smallTickLineSize) + chartAxis.AxisLineStyle.StrokeWidth, availableSize.Width);
            }
            else
            {
                size = new Size(availableSize.Height, Math.Max(chartAxis.MajorTickStyle.TickSize, smallTickLineSize) + chartAxis.AxisLineStyle.StrokeWidth);
            }

            desiredSize = size;
            return size;
        }

        #region private members
        private void DrawTicks(ICanvas canvas, ChartAxisTickStyle tickStyle, List<double> tickPoints, bool isDrawMiortick)
        {
            AxisElementPosition elementPosition = chartAxis.TickPosition;
            bool isOpposed = chartAxis.IsOpposed();

            float tickSize = (float)tickStyle.TickSize;
            float axisLineWidth = (float)chartAxis.AxisLineStyle.StrokeWidth;

            float x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            if (tickPoints.Count > 0)
            {
                if (!chartAxis.IsVertical)
                {
                    y1 = elementPosition == AxisElementPosition.Inside ? isOpposed ? axisLineWidth : desiredSize.Height - tickSize - axisLineWidth : isOpposed ? desiredSize.Height - tickSize - axisLineWidth : axisLineWidth;
                    y2 = y1 + tickSize;
                }
                else
                {
                    x1 = elementPosition == AxisElementPosition.Inside ? isOpposed ? desiredSize.Width - tickSize - axisLineWidth : axisLineWidth : isOpposed ? axisLineWidth : desiredSize.Width - tickSize - axisLineWidth;
                    x2 = x1 + tickSize;
                }
            }

            foreach (var position in tickPoints)
            {
                double value = chartAxis.ValueToCoefficient(position);

                if (!chartAxis.IsVertical)
                {
                    x1 = x2 = (float)chartAxis.GetActualPlotOffsetStart() + (float)Math.Round(chartAxis.RenderedRect.Width * value);
                }
                else
                {
                    y1 = y2 = (float)chartAxis.GetActualPlotOffsetEnd() + (float)Math.Round(chartAxis.RenderedRect.Height * (1 - value));
                }


                canvas.StrokeSize = (float)tickStyle.StrokeWidth;
                canvas.StrokeColor = tickStyle.Stroke.ToColor();

                if (!isDrawMiortick)
                {
                    chartAxis.DrawMajorTick(canvas, position, new PointF(x1, y1), new PointF(x2, y2));
                }
                else
                {
                    chartAxis.DrawMinorTick(canvas, position, new PointF(x1, y1), new PointF(x2, y2));
                }
            }
        }

        private void DrawAxisLine(ICanvas canvas)
        {
            float x1, y1, x2, y2;
            float width = (float)chartAxis.ArrangeRect.Width;
            float height = (float)chartAxis.ArrangeRect.Height;

            var style = chartAxis.AxisLineStyle;

            bool isOpposed = chartAxis.IsOpposed() ^ chartAxis.TickPosition == AxisElementPosition.Inside;
            float axisLineOffset = (float)chartAxis.AxisLineOffset;
            float offset = ((float)style.StrokeWidth) / 2;

            if (!chartAxis.IsVertical)
            {
                x1 = axisLineOffset;
                x2 = width - axisLineOffset;
                y1 = y2 = isOpposed ? desiredSize.Height - offset : offset;
            }
            else
            {

                x1 = x2 = isOpposed ? offset : desiredSize.Width - offset;
                y1 = axisLineOffset;
                y2 = height - axisLineOffset;
            }

            canvas.StrokeColor = style.Stroke.ToColor();
            canvas.StrokeSize = (float)style.StrokeWidth;

            if (style.StrokeDashArray != null)
            {
                canvas.StrokeDashPattern = style.StrokeDashArray.ToFloatArray();
            }

            chartAxis.DrawAxisLine(canvas, x1, y1, x2, y2);
        }
        #endregion
        #endregion
    }
}
