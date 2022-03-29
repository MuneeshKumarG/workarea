using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using RectF = Microsoft.Maui.Graphics.RectF;

namespace Syncfusion.Maui.Charts
{
    internal class AxisLabelLayout
    {
        #region Internal Properties
        internal bool NeedToRotate { get; set; }

        internal ChartAxis Axis { get; set; }

        internal List<RectF>? LabelsRect { get; set; }

        internal List<SizeF>? ComputedSizes { get; set; }

        internal List<SizeF>? DesiredSizes { get; set; }

        internal double DesiredHeight { get; set; }

        internal double DesiredWidth { get; set; }

        internal List<Dictionary<int, RectF>>? RectByRowsAndCols { get; set; }

        internal float MarginTop { get; set; }

        internal float MarginBottom { get; set; }

        internal float MarginLeft { get; set; }

        internal float MarginRight { get; set; }

        internal float Offset { get; set; }

        internal AxisLabelLayout(ChartAxis axis)
        {
            Axis = axis;
        }
        #endregion

        #region Methods

        #region Internal Methods
        internal SizeF GetLabelSize(string label, ChartAxisLabelStyle labelStyle)
        {
            return label.Measure(labelStyle);
        }

        internal void DrawLabelBackground(ICanvas canvas, float x, float y, SizeF size, ChartAxisLabelStyle labelStyle)
        {
            float halfStrokeWidth = (float)labelStyle.StrokeWidth / 2;

            CornerRadius cornerRadius = labelStyle.CornerRadius;

            var rect = new Rect(x, y, size.Width, size.Height);

            canvas.SetFillPaint(labelStyle.Background, rect);

            if (!labelStyle.HasCornerRadius && labelStyle.IsBackgroundColorUpdated)
            {
                canvas.FillRectangle(rect);
            }
            else
            {
                canvas.FillRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
            }

            if (labelStyle.IsStrokeColorUpdated)
            {
                canvas.StrokeColor = labelStyle.Stroke.ToColor();
                canvas.StrokeSize = halfStrokeWidth;
                if (labelStyle.HasCornerRadius)
                {
                    canvas.DrawRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
                }
                else
                {
                    canvas.DrawRectangle(rect);
                }
            }
        }

        /// <summary>
        /// Method used to Align Axis label at Start and End position
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="size">The size.</param>
        /// <param name="labelStyle">The label style.</param>
        /// <returns>The position</returns>
        internal static float OnAxisLabelAlignment(float position, float size, ChartAxisLabelStyle labelStyle)
        {
            var labelAlignment = labelStyle.LabelAlignment;

            switch (labelAlignment)
            {
                case ChartAxisLabelAlignment.Start:
                    position -= size / 2;
                    break;
                case ChartAxisLabelAlignment.End:
                    position += size / 2;
                    break;
            }

            return position;
        }

        /// <summary>
        /// Method used to create the axis layout.
        /// </summary>
        /// <param name="chartAxis">The ChartAxis.</param>
        /// <returns>The layout.</returns>
        internal static AxisLabelLayout CreateAxisLayout(ChartAxis chartAxis)
        {
            if (!chartAxis.IsVertical)
            {
                return new HorizontalLabelLayout(chartAxis);
            }

            return new VerticalLabelLayout(chartAxis);
        }

        internal virtual SizeF Measure(SizeF availableSize)
        {
            double rotateAngle = Axis.LabelRotation;
            ComputedSizes = new List<SizeF>();
            DesiredSizes = ComputedSizes;

            NeedToRotate = !double.IsNaN(rotateAngle) &&
                                    Math.Abs(rotateAngle % 360) > double.Epsilon;

            if (NeedToRotate)
            {
                ComputedSizes = new List<SizeF>();//TODO: Need of creting cumpute size
            }

            if (Axis != null && Axis.VisibleLabels != null && Axis.VisibleLabels.Count > 0)
            {
                foreach (var axisLabel in Axis.VisibleLabels)
                {
                    ChartAxisLabelStyle labelStyle = axisLabel.LabelStyle != null ? axisLabel.LabelStyle : Axis.LabelStyle;
                    labelStyle.LabelsIntersectAction = Axis.LabelsIntersectAction;
                    labelStyle.WrapWidthCollection = new Dictionary<string, double>();

                    MarginTop = (float)labelStyle.Margin.Top;
                    MarginLeft = (float)labelStyle.Margin.Left;
                    MarginRight = (float)labelStyle.Margin.Right;
                    MarginBottom = (float)labelStyle.Margin.Bottom;

                    var label = axisLabel.Content.Tostring();
                    SizeF measuredSize = GetLabelSize(label, labelStyle);
                    SizeF desiredSize = new SizeF(measuredSize.Width + MarginLeft + MarginRight, measuredSize.Height + MarginTop + MarginBottom);

                    DesiredWidth = desiredSize.Width > DesiredWidth ? desiredSize.Width : DesiredWidth;
                    DesiredHeight = desiredSize.Height > DesiredHeight ? desiredSize.Height : DesiredHeight;

                    DesiredSizes.Add(desiredSize);

                    if (NeedToRotate)
                    {
                        ComputedSizes.Add(GetRotatedSize(desiredSize, rotateAngle));
                    }
                }

                CalculateActualPlotOffset(availableSize);
            }

            return new SizeF();
        }

        internal bool IsOpposed()
        {
            if (Axis == null)
            {
                return false;
            }

            var opposedPos = Axis.IsOpposed();
            var labelPos = Axis.LabelStyle.LabelsPosition;
            return (opposedPos && labelPos == AxisElementPosition.Outside)
                || (!opposedPos && labelPos == AxisElementPosition.Inside);
        }

        internal virtual float LayoutElements()
        {
            int i = 1;
            int prevIndex = 0;
            var labels = Axis.VisibleLabels;

            if (labels == null || Axis.LabelsIntersectAction == AxisLabelsIntersectAction.None)
            {
                return 0;
            }

            if (Axis.LabelsIntersectAction == AxisLabelsIntersectAction.Hide)
            {
                int length = labels.Count;
                for (; i < length; i++)
                {
                    var label = labels[i];
                    var rowColElement = RectByRowsAndCols?.ElementAt(0);
                    if (rowColElement != null && IntersectsWith(rowColElement[prevIndex], rowColElement[i], prevIndex, i))
                    {
                        var rangeAxis = Axis as RangeAxisBase;
                        if (i == length - 1 && rangeAxis != null)
                        {
                            if (rangeAxis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible ||
                                 ((rangeAxis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible) && !(Axis.ZoomFactor < 1)))
                            {
                                if ((rangeAxis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift || rangeAxis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Fit) && LabelsRect != null)
                                {
                                    var lastRect = LabelsRect[i];
                                    for (int j = i - 1; j >= 0; j--)
                                    {
                                        RectF rectbefore = LabelsRect[j];

                                        if (IntersectsWith(rectbefore, lastRect, j, i))
                                        {
                                            labels[j].IsVisible = false;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    labels[i - 1].IsVisible = false;
                                }
                            }
                            else
                            {
                                label.IsVisible = false;
                            }
                        }
                        else
                        {
                            label.IsVisible = false;
                        }
                    }
                    else
                    {
                        prevIndex = i;
                    }
                }
            }
            else if (Axis.LabelsIntersectAction == AxisLabelsIntersectAction.MultipleRows || Axis.LabelsIntersectAction == AxisLabelsIntersectAction.Wrap)
            {
                i = 1;
                prevIndex = 0;
                int length = labels.Count;

                for (; i < length; i++)
                {
                    var rectByRowsAndCol = RectByRowsAndCols?.ElementAt(0);
                    if (rectByRowsAndCol != null && IntersectsWith(rectByRowsAndCol[prevIndex], rectByRowsAndCol[i], prevIndex, i)
                        && labels[i].IsVisible && labels[prevIndex].IsVisible)
                    {
                        RectF rect = rectByRowsAndCol[i];
                        rectByRowsAndCol.Remove(i);
                        InsertToRowOrColumn(1, i, rect);
                    }
                    else
                    {
                        prevIndex = i;
                    }
                }
            }
      
            return 0;
        }

        internal void CalculateWrapLabelRect()
        {
            bool isLabelWrap;
            int previousIndex = 0;
            int i = 1;
            var labels = Axis.VisibleLabels;

            if (labels != null && LabelsRect != null)
            {
                var length = labels.Count;

                for (; i < length; i++)
                {
                    ChartAxisLabel currentLabel = labels[i];
                    ChartAxisLabel previousLabel = labels[previousIndex];
                    ChartAxisLabelStyle labelStyle = previousLabel.LabelStyle != null ? previousLabel.LabelStyle : Axis.LabelStyle;

                    RectF previousRect = LabelsRect[previousIndex];
                    RectF currentRect = LabelsRect[i];

                    if (IntersectsWith(previousRect, currentRect, previousIndex, i)
                        && currentLabel.IsVisible && previousLabel.IsVisible)
                    {
                        MarginTop = (float)labelStyle.Margin.Top;
                        MarginLeft = (float)labelStyle.Margin.Left;
                        MarginRight = (float)labelStyle.Margin.Right;
                        MarginBottom = (float)labelStyle.Margin.Bottom;

                        var prevWrapWidth = (currentRect.Left - previousRect.Left) - (MarginLeft + MarginRight);
                        isLabelWrap = LabelContainWrapWidth(previousLabel.Content.Tostring(), prevWrapWidth, labelStyle);

                        if (isLabelWrap || (isLabelWrap && i == length - 1))
                        {
                            var text = previousLabel.Content.Tostring();
                            CalculateWrapLabelSize(text, prevWrapWidth, labelStyle, previousIndex, previousRect);

                            if (i == length - 1)
                            {
                                var x = currentRect.Left + (previousRect.Right - currentRect.Left);
                                var wrapWidth = (currentRect.Right - x) - (MarginLeft + MarginRight);
                                isLabelWrap = LabelContainWrapWidth(currentLabel.Content.Tostring(), wrapWidth, labelStyle);
                                var label = currentLabel.Content.Tostring();

                                if (isLabelWrap)
                                {
                                    CalculateWrapLabelSize(label, wrapWidth, labelStyle, i, currentRect);
                                }
                                else
                                {
                                    CalculateNonIntersectLabelWidth(i, previousLabel, currentLabel, previousRect, currentRect, length, labelStyle);
                                }
                            }
                        }
                        else
                        {
                            CalculateNonIntersectLabelWidth(i, previousLabel, currentLabel, previousRect, currentRect, length, labelStyle);
                        }
                    }
                    else
                    {
                        CalculateNonIntersectLabelWidth(i, previousLabel, currentLabel, previousRect, currentRect, length, labelStyle);
                    }

                    previousIndex = i;
                }
            }
        }

        internal bool IntersectsWith(RectF r1, RectF r2, int prevIndex, int currentIndex)
        {
            //TODO:validate rect to position next label.
            return !(r2.Left > r1.Right || r2.Right < r1.Left || r2.Top > r1.Bottom || r2.Bottom < r1.Top);
        }

        internal virtual void CalculateActualPlotOffset(SizeF availableSize)
        {
            Axis.ActualPlotOffset = 0f;
            Axis.ActualPlotOffsetStart = (float)(double.IsNaN(Axis.PlotOffsetStart) ? 0f : Axis.PlotOffsetStart);
            Axis.ActualPlotOffsetEnd = (float)(double.IsNaN(Axis.PlotOffsetEnd) ? 0f : Axis.PlotOffsetEnd);
        }

        internal virtual void OnDraw(ICanvas drawing, SizeF finalSize)
        {
        }
        #endregion

        #region Private Methods
        private void CalculateNonIntersectLabelWidth(int currIndex, ChartAxisLabel previousLabel, ChartAxisLabel currentLabel, RectF previousRect, RectF currentRect, int labelCount, ChartAxisLabelStyle labelStyle)
        {
            var text = previousLabel.Content.Tostring();
            var previousIndex = currIndex - 1;

            if (labelStyle.WrapWidthCollection != null && !labelStyle.WrapWidthCollection.ContainsKey(text))
            {
                var wrapWidth = previousRect.Width + MarginLeft + MarginRight;
                CalculateWrapLabelSize(text, wrapWidth, labelStyle, previousIndex, previousRect);
            }

            if (currIndex == labelCount - 1)
            {
                var label = currentLabel.Content.Tostring();

                if (labelStyle.WrapWidthCollection != null && !labelStyle.WrapWidthCollection.ContainsKey(label))
                {
                    var wrapWidth = currentRect.Width + MarginLeft + MarginRight;
                    CalculateWrapLabelSize(label, wrapWidth, labelStyle, currIndex, currentRect);
                }
            }
        }

        private void CalculateWrapLabelSize(string text, double wrapWidth, ChartAxisLabelStyle labelStyle, int index, RectF rect)
        {
            if (DesiredSizes == null || LabelsRect == null)
            {
                return;
            }

            if (labelStyle.WrapWidthCollection != null && !labelStyle.WrapWidthCollection.ContainsKey(text))
            {
                labelStyle.WrapWidthCollection.Add(text, wrapWidth);
            }

            var measuredSize = GetLabelSize(text, labelStyle);
            var desiredSize = new SizeF(measuredSize.Width + MarginLeft + MarginRight, measuredSize.Height + MarginTop + MarginBottom);
            DesiredSizes[index] = desiredSize;
            rect.Size = desiredSize;
            LabelsRect[index] = new RectF(rect.X, rect.Y, rect.Width, rect.Height);

            if (NeedToRotate)
            {
                ComputedSizes?.Add(GetRotatedSize(desiredSize, Axis.LabelRotation));
            }

            if (RectByRowsAndCols != null)
            {
                RectByRowsAndCols[0].Remove(index);
                RectByRowsAndCols[0].Add(index, rect);
            }
        }

        private static bool LabelContainWrapWidth(string label, double wrapWidth, ChartAxisLabelStyle labelStyle)
        {
            string[] words = label.Split(new char[] { ' ' });
            SizeF labelSize = SizeF.Zero;

            for (int j = 0; j < words.Length; j++)
            {
                //var paint = labelStyle.GetTextPaint();
                //string word = words[j];
                //labelSize.Width = paint.MeasureText(word, 0, word.Length);

                if (labelSize.Width > wrapWidth)
                {
                    return false;
                }
            }

            return true;
        }

        private void InsertToRowOrColumn(int rowOrColIndex, int itemIndex, RectF rect)
        {
            if (RectByRowsAndCols == null)
            {
                return;
            }

            if (RectByRowsAndCols.Count <= rowOrColIndex)
            {
                RectByRowsAndCols.Add(new Dictionary<int, RectF>());
                RectByRowsAndCols[rowOrColIndex].Add(itemIndex, rect);
            }
            else
            {
                var lastRowOrColumn = RectByRowsAndCols[rowOrColIndex];
                int lastIndex = lastRowOrColumn.Count - 1;
                var lastKey = lastRowOrColumn.Keys.ToArray()[lastIndex];
                RectF prevRect = lastRowOrColumn[lastKey];

                if (IntersectsWith(prevRect, rect, lastIndex, itemIndex))
                {
                    InsertToRowOrColumn(++rowOrColIndex, itemIndex, rect);
                }
                else
                {
                    RectByRowsAndCols[rowOrColIndex].Add(itemIndex, rect);
                }
            }
        }

        //TODO:Calculate rotation size for label.
        private static SizeF GetRotatedSize(SizeF size, double degrees)
        {
            return ChartUtils.GetRotatedSize(new Size(size.Width, size.Height), (float)degrees);
        }

        private static bool IntersectsWith(List<PointF> shape1Points, List<PointF> shape2Points)
        {
            for (int i = 0; i < shape1Points.Count; i++)
            {
                PointF point11 = shape1Points[i];
                int nextIndex = i == shape1Points.Count - 1 ? 0 : i + 1;
                PointF point12 = shape1Points[nextIndex];
                for (int j = 0; j < shape2Points.Count; j++)
                {
                    PointF point21 = shape2Points[j];
                    nextIndex = j == shape2Points.Count - 1 ? 0 : j + 1;
                    PointF point22 = shape2Points[nextIndex];
                    if (DoLinesIntersect(point11, point12, point21, point22))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool DoLinesIntersect(PointF point11, PointF point12, PointF point21, PointF point22)
        {
            double d = ((point22.Y - point21.Y) * (point12.X - point11.X)) -
                    ((point22.X - point21.X) * (point12.Y - point11.Y));
            double na = ((point22.X - point21.X) * (point11.Y - point21.Y)) -
                    ((point22.Y - point21.Y) * (point11.X - point21.X));
            double nb = ((point12.X - point11.X) * (point11.Y - point21.Y)) -
                    ((point12.Y - point11.Y) * (point11.X - point21.X));

            if (d == 0)
            {
                return false;
            }

            double ua = na / d;
            double ub = nb / d;

            return ua >= 0d && ua <= 1d && ub >= 0d && ub <= 1d;
        }
        #endregion
        #endregion
    }

    internal class HorizontalLabelLayout : AxisLabelLayout
    {
        #region Fields
        private float desiredSize;
        #endregion

        #region Internal Properties
        internal SizeF AvailabelSize { get; set; }
        #endregion

        #region Constructor
        internal HorizontalLabelLayout(ChartAxis axis)
            : base(axis)
        {
        }
        #endregion

        #region Methods

        internal override void OnDraw(ICanvas canvas, SizeF finalSize)
        {
            if (RectByRowsAndCols == null || Axis.VisibleLabels == null || DesiredSizes == null || ComputedSizes == null)
            {
                return;
            }

            bool isOpposed = IsOpposed();
            double left = Axis.GetActualPlotOffsetStart();
            double rotateAngle = Axis.LabelRotation % 360;
            double maxHeight = 0, top = isOpposed ? finalSize.Height - Offset : Offset;

            foreach (var rowsOrColumn in RectByRowsAndCols)
            {
                foreach (var rowsOrCols in rowsOrColumn)
               {
                    int i = rowsOrCols.Key;
                    var label = Axis.VisibleLabels[i];
                    var labelStyle = label.LabelStyle != null ? label.LabelStyle : Axis.LabelStyle;
                    MarginLeft = (float)labelStyle.Margin.Left ;
                    MarginBottom = (float)labelStyle.Margin.Bottom;

                    var content = label.Content.Tostring();

                    if (!label.IsVisible || string.IsNullOrEmpty(content))
                    {
                        continue;
                    }

                    SizeF actualSize = DesiredSizes[i];
                    SizeF rotatedSize = ComputedSizes[i];
                    RectF labelRect = rowsOrCols.Value;
                    float yPos = (float)top;

                    float xPos = labelRect.Left + (float)left - (actualSize.Width / 2) + (rotatedSize.Width / 2);

                    yPos -= isOpposed ? rotatedSize.Height : 0;

                    SizeF measuredSize = GetLabelSize(content, labelStyle);

                    float height = rotatedSize.Height;
                    float textX = (float)xPos + MarginLeft;
                    //Addded platform specific code for position the label.
#if ANDROID
                    float textY = (float)yPos + (float)DesiredHeight - MarginBottom;
#else
                    float textY = (float)yPos + (measuredSize.Height / 2) - MarginBottom;
#endif

                    float rotateOriginX = textX;
                    float rotateOriginY = textY;

                    if (NeedToRotate)
                    {
                        if (Math.Abs(rotateAngle) == 180)
                        {
                            rotateOriginX = textX + measuredSize.Width;
                            rotateOriginY = textY - measuredSize.Height;
                            textX = textX + measuredSize.Width;
                            textY = textY - measuredSize.Height;
                            xPos += measuredSize.Width;
                            yPos -= measuredSize.Height;
                        }
                        else if ((!Axis.IsOpposed() && Axis.LabelStyle.LabelsPosition == AxisElementPosition.Inside) ||
                              (Axis.IsOpposed() && Axis.LabelStyle.LabelsPosition == AxisElementPosition.Outside))
                        {
                            yPos += rotatedSize.Height;
                            textY += rotatedSize.Height;
                            rotateOriginY += rotatedSize.Height;

                            yPos -= actualSize.Height;
                            textY -= actualSize.Height;
                            rotateOriginY -= actualSize.Height;
                            rotateOriginX = textX + (measuredSize.Width / 2);

                            if ((rotateAngle > 0 && rotateAngle < 180) || (rotateAngle < -180 && rotateAngle > -360))
                            {
                                textX -= measuredSize.Width / 2;
                                xPos -= measuredSize.Width / 2;

                                textY += measuredSize.Height / 2;
                                yPos += measuredSize.Height / 2;
                            }
                            else
                            {
                                textX += measuredSize.Width / 2;
                                xPos += measuredSize.Width / 2;

                                textY += measuredSize.Height / 2;
                                yPos += measuredSize.Height / 2;
                            }
                        }
                        else
                        {
                            rotateOriginX = textX + (measuredSize.Width / 2);
                            rotateOriginY = textY - (measuredSize.Height / 2);

                            if ((rotateAngle > 0 && rotateAngle < 180) || (rotateAngle < -180 && rotateAngle > -360))
                            {
                                textX += measuredSize.Width / 2;
                                xPos += measuredSize.Width / 2;
                            }
                            else
                            {
                                textX -= measuredSize.Width / 2;
                                xPos -= measuredSize.Width / 2;
                            }
                        }

                        textY += measuredSize.Height / 2;
                    }

                    label.RotateOriginX = rotateOriginX;
                    label.RotateOriginY = rotateOriginY;

                    canvas.SaveState();

                    canvas.Rotate((float)rotateAngle, (float)rotateOriginX, (float)rotateOriginY);

                    if (labelStyle.CanDraw())
                    {
                        DrawLabelBackground(canvas, xPos, yPos, actualSize, labelStyle);
                    }

                    canvas.DrawText(content, textX, textY, labelStyle);
                    

                    if (LabelsRect != null)
                    {
                        LabelsRect[i] = new RectF(xPos, yPos, actualSize.Width, actualSize.Height);
                    }

                    canvas.RestoreState();

                    if (height > maxHeight)
                    {
                        maxHeight = height;
                    }
                }

                if (isOpposed)
                {
                    top -= maxHeight;
                }
                else
                {
                    top += maxHeight;
                }
            }
        }

        internal override SizeF Measure(SizeF availableSize)
        {
            if (Axis.VisibleLabels != null && Axis.VisibleLabels.Count > 0)
            {
                AvailabelSize = availableSize;
                base.Measure(availableSize);
                CalcBounds(availableSize.Width - (float)Axis.GetActualPlotOffset());
                desiredSize = Math.Max(LayoutElements(), (float)Axis.LabelExtent);
                return new SizeF(availableSize.Width, desiredSize);
            }

            return new SizeF(availableSize.Width, 0);
        }

        internal override void CalculateActualPlotOffset(SizeF availableSize)
        {
            if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Fit && ComputedSizes != null && Axis.VisibleLabels != null)
            {
                float plotOffset = 0f;
                SizeF computedSize = ComputedSizes[0];
                var label = Axis.VisibleLabels[0];
                float coeff = Axis.ValueToCoefficient(label.Position);
                float position = (coeff * availableSize.Width) - (computedSize.Width / 2);

                float firstElementWidth = 0;
                float lastElementWidth = 0;
                float width = Axis.IsInversed ? availableSize.Width : 0f;

                if ((position - (computedSize.Width / 2) + plotOffset) < width)
                {
                    firstElementWidth = computedSize.Width;
                }

                int index = Axis.VisibleLabels.Count - 1;
                computedSize = ComputedSizes[index];
                width = Axis.IsInversed ? 0f : availableSize.Width;

                if ((position + (computedSize.Width / 2) - plotOffset) > width)
                {
                    lastElementWidth = computedSize.Width;
                }

                float offset = Math.Max(firstElementWidth / 2, lastElementWidth / 2);
                Axis.ActualPlotOffset = Math.Max(offset, plotOffset);
            }
            else
            {
                base.CalculateActualPlotOffset(availableSize);
            }
        }

        internal override float LayoutElements()
        {
            if (Axis.LabelsIntersectAction == AxisLabelsIntersectAction.Wrap)
            {
                CalculateWrapLabelRect();
                CalcBounds(AvailabelSize.Width - (float)Axis.GetActualPlotOffset());
            }

            base.LayoutElements();
            float totalHeight = 0;

            if (RectByRowsAndCols != null)
            {
                foreach (var rowsOrColumns in RectByRowsAndCols)
                {
                    float max = 0;

                    foreach (var rowOrCol in rowsOrColumns)
                    {
                        var currValue = rowOrCol.Value.Height;
                        if (currValue > max)
                        {
                            max = currValue;
                        }
                    }

                    totalHeight += max;
                }
            }

            return totalHeight;
        }

        private void CalcBounds(float size)
        {
            RectByRowsAndCols = new List<Dictionary<int, RectF>>();
            Dictionary<int, RectF> rowsOrColumn = new Dictionary<int, RectF>();
            RectByRowsAndCols.Add(rowsOrColumn);
            var axisLabels = Axis.VisibleLabels;

            if (axisLabels == null || ComputedSizes == null)
            {
                return;
            }

            int length = axisLabels.Count;

            LabelsRect = new List<RectF>();

            for (int i = 0; i < length; i++)
            {
                var chartAxisLabel = axisLabels[i];
                var labelStyle = chartAxisLabel.LabelStyle != null ? chartAxisLabel.LabelStyle : Axis.LabelStyle;
                MarginLeft = (float)labelStyle.Margin.Left;
                MarginRight = (float)labelStyle.Margin.Right;

                float coeff = (float)Axis.ValueToCoefficient(chartAxisLabel.Position);
                float position = (coeff * size) - (ComputedSizes[i].Width / 2); //TODO: ensure the width need to calculate,
                position -= (MarginLeft - MarginRight) / 2;
                position = OnAxisLabelAlignment(position, ComputedSizes[i].Width + (float)labelStyle.StrokeWidth, labelStyle);
                RectF rect = new RectF(position, 0, ComputedSizes[i].Width, ComputedSizes[i].Height);
                rowsOrColumn.Add(i, rect);
                LabelsRect.Add(rect);
            }

            if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift)
            {
                var computedSize = ComputedSizes[0];

                var rectByRowAndCols = RectByRowsAndCols.ElementAt(0);
                if (rectByRowAndCols[0].Left < 0)
                {
                    LabelsRect[0] = rectByRowAndCols[0] = new RectF(0, 0, computedSize.Width, computedSize.Height);
                }

                int index = axisLabels.Count - 1;
                if (rectByRowAndCols[index].Right > size)
                {
                    computedSize = ComputedSizes[index];
                    LabelsRect[index] = rectByRowAndCols[index] = new RectF(size - computedSize.Width, 0, computedSize.Width, computedSize.Height);
                }
            }
            else if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Hide)
            {
                var rectByRowAndCols = RectByRowsAndCols.ElementAt(0);
                if (rectByRowAndCols[0].Left < 0)
                {
                    rectByRowAndCols[0] = RectF.Zero;
                    LabelsRect[0] = RectF.Zero;
                    axisLabels[0].IsVisible = false;
                }

                int index = axisLabels.Count - 1;
                if (rectByRowAndCols[index].Right > size)
                {
                    rectByRowAndCols[index] = RectF.Zero;
                    LabelsRect[index] = RectF.Zero;
                    axisLabels[index].IsVisible = false;
                }
            }
        }

        #endregion
    }

    internal class VerticalLabelLayout : AxisLabelLayout
    {
        #region Fields
        private float desiredSize;
        #endregion

        #region Internal Properties
        internal SizeF AvailabelSize { get; set; }
        #endregion

        #region Constructor
        internal VerticalLabelLayout(ChartAxis axis)
            : base(axis)
        {
        }
        #endregion

        #region Methods

        internal override void OnDraw(ICanvas canvas, SizeF finalSize)
        {
            if (RectByRowsAndCols == null || Axis.VisibleLabels == null || DesiredSizes == null || ComputedSizes == null)
            {
                return;
            }

            bool isOpposed = IsOpposed();
            float top = (float)Axis.GetActualPlotOffsetEnd();
            float rotateAngle = (float)Axis.LabelRotation % 360;
            float maxWidth = 0, left = isOpposed ? Offset : finalSize.Width - Offset;

            foreach (var rowsOrColumn in RectByRowsAndCols)
            {
                foreach (var rowsOrCols in rowsOrColumn)
                {
                    int i = rowsOrCols.Key;
                    var label = Axis.VisibleLabels[i];
                    var labelStyle = label.LabelStyle != null ? label.LabelStyle : Axis.LabelStyle;
                    MarginTop = (float)labelStyle.Margin.Top;
                    MarginRight = (float)labelStyle.Margin.Right;

                    var content = label.Content.Tostring();

                    if (!label.IsVisible || string.IsNullOrEmpty(content))
                    {
                        continue;
                    }

                    SizeF actualSize = DesiredSizes[i];
                    SizeF rotatedSize = ComputedSizes[i];
                    RectF labelRect = rowsOrCols.Value;
                    float xPos, yPos, textX, textY;

                    var measuredSize = GetLabelSize(content, labelStyle);
                    xPos = left;
                    yPos = labelRect.Top + top - actualSize.Height + rotatedSize.Height;
                    xPos -= isOpposed ? 0 : actualSize.Width;

                    textX = xPos + MarginRight; //TODO: ensure the width need to calculate,
                    //Addded platform specific code for position the label.
#if ANDROID
                    textY = yPos + actualSize.Height - MarginTop;
#else
                    textY = yPos + (measuredSize.Height / 2) - MarginTop;
#endif

                    float width = rotatedSize.Width;
                    float rotateOriginX = textX;
                    float rotateOriginY = textY;

                    //TODO: if label need to rotate.
                    if (NeedToRotate)
                    {
                        if ((Axis.IsOpposed() && Axis.LabelStyle.LabelsPosition == AxisElementPosition.Inside) ||
                            (!Axis.IsOpposed() && Axis.LabelStyle.LabelsPosition == AxisElementPosition.Outside))
                        {
                            if (Math.Abs(rotateAngle) == 90)
                            {
                                rotateOriginX += measuredSize.Width;
                                rotateOriginY -= measuredSize.Height / 2;
                                textX += measuredSize.Width / 2;
                                textY += measuredSize.Height / 2;
                                xPos += measuredSize.Width / 2;
                                yPos += measuredSize.Height / 2;
                            }
                            else if (Math.Abs(rotateAngle) == 270)
                            {
                                rotateOriginX += measuredSize.Width;
                                rotateOriginY -= measuredSize.Height / 2;
                                textX += measuredSize.Width / 2;
                                textY -= measuredSize.Height / 2;
                                xPos += measuredSize.Width / 2;
                                yPos -= measuredSize.Height / 2;
                            }
                            else if (Math.Abs(rotateAngle) > 0 && Math.Abs(rotateAngle) < 90)
                            {
                                rotateOriginX += measuredSize.Width;
                                rotateOriginY -= measuredSize.Height / 2;
                                textX -= measuredSize.Height / 2;
                                xPos -= measuredSize.Height / 2;
                            }
                            else if (Math.Abs(rotateAngle) > 90 && Math.Abs(rotateAngle) < 270)
                            {
                                rotateOriginX += measuredSize.Width;
                                rotateOriginY -= measuredSize.Height / 2;
                                textX += measuredSize.Width;
                                xPos += measuredSize.Width;
                            }
                            else if (Math.Abs(rotateAngle) > 270 && Math.Abs(rotateAngle) < 360)
                            {
                                rotateOriginX += measuredSize.Width;
                                rotateOriginY -= measuredSize.Height / 2;
                            }
                        }
                        else
                        {
                            if (rotateAngle == 90)
                            {
                                rotateOriginY -= measuredSize.Height / 2;
                                textX -= measuredSize.Width / 2;
                                textY -= measuredSize.Height / 2;
                                xPos -= measuredSize.Width / 2;
                                yPos -= measuredSize.Height / 2;
                            }
                            else if (rotateAngle == 270)
                            {
                                rotateOriginX += measuredSize.Width / 2;
                                rotateOriginY -= measuredSize.Height / 2;
                                textY -= measuredSize.Height;
                                yPos -= measuredSize.Height;
                            }
                            else if (rotateAngle > 0 && rotateAngle < 90)
                            {
                                rotateOriginX += measuredSize.Height / 2;
                                rotateOriginY -= measuredSize.Height / 2;
                                textX += measuredSize.Height / 2;
                                xPos += measuredSize.Height / 2;
                            }
                            else if (rotateAngle > 90 && rotateAngle < 270)
                            {
                                textX -= measuredSize.Width + (measuredSize.Height / 2);
                                xPos -= measuredSize.Width + (measuredSize.Height / 2);
                                rotateOriginY -= measuredSize.Height / 2;
                            }
                            else if (rotateAngle > 270 && rotateAngle < 360)
                            {
                                rotateOriginX += measuredSize.Height / 2;
                                xPos += measuredSize.Height / 2;
                                textX += measuredSize.Height / 2;
                                rotateOriginY -= measuredSize.Height / 2;
                            }
                        }

                        textY -= measuredSize.Width / 2;
                        textX += measuredSize.Height + measuredSize.Height / 2;
                    }

                    label.RotateOriginX = rotateOriginX;
                    label.RotateOriginY = rotateOriginY;

                    canvas.SaveState();
                    canvas.Rotate(rotateAngle, rotateOriginX, rotateOriginY);

                    if (labelStyle.CanDraw())
                    {
                        DrawLabelBackground(canvas, xPos, yPos, actualSize, labelStyle);
                    }

                    canvas.DrawText(content, textX, textY, labelStyle);

                    if (LabelsRect != null)
                    {
                        LabelsRect[i] = new RectF(xPos, yPos, actualSize.Width, actualSize.Height);
                    }

                    canvas.RestoreState();

                    if (width > maxWidth)
                    {
                        maxWidth = width;
                    }
                }

                if (isOpposed)
                {
                    left += maxWidth;
                }
                else
                {
                    left -= maxWidth;
                }
            }
        }

        internal override SizeF Measure(SizeF availableSize)
        {
            if (Axis.VisibleLabels != null && Axis.VisibleLabels.Count > 0)
            {
                AvailabelSize = availableSize;
                base.Measure(availableSize);
                CalcBounds(availableSize.Height - (float)Axis.GetActualPlotOffset());
                desiredSize = Math.Max(LayoutElements(), (float)Axis.LabelExtent);
                return new SizeF(desiredSize, availableSize.Height);
            }

            return new SizeF(0, availableSize.Height);
        }

        internal override void CalculateActualPlotOffset(SizeF availableSize)
        {
            if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Fit && ComputedSizes != null && Axis.VisibleLabels != null)
            {
                float plotOffset = 0f;
                SizeF computedSize = ComputedSizes[0];
                var label = Axis.VisibleLabels[0];
                float coeff = Axis.ValueToCoefficient(label.Position);
                float position = ((1 - coeff) * availableSize.Height) - (computedSize.Height / 2);

                float firstElementHeight = 0;
                float lastElementHeight = 0;
                float height = Axis.IsInversed ? 0f : availableSize.Height;

                if (position + (computedSize.Height / 2) - plotOffset > height)
                {
                    firstElementHeight = computedSize.Height;
                }

                int index = Axis.VisibleLabels.Count - 1;
                label = Axis.VisibleLabels[index];
                coeff = Axis.ValueToCoefficient(label.Position);
                computedSize = ComputedSizes[index];
                position = ((1 - coeff) * availableSize.Height) - (computedSize.Height / 2);
                height = Axis.IsInversed ? availableSize.Height : 0f;

                if (position - (computedSize.Height / 2) + plotOffset < height)
                {
                    lastElementHeight = computedSize.Height;
                }

                float offset = Math.Max(firstElementHeight / 2, lastElementHeight / 2);
                Axis.ActualPlotOffset = Math.Max(offset, plotOffset);
            }
            else
            {
                base.CalculateActualPlotOffset(availableSize);
            }
        }

        /// <summary>
        /// Returns desired width
        /// </summary>
        /// <returns>The total width.</returns>
        internal override float LayoutElements()
        {
            if (Axis.LabelsIntersectAction == AxisLabelsIntersectAction.Wrap)
            {
                CalculateWrapLabelRect();
                CalcBounds(AvailabelSize.Height - (float)Axis.GetActualPlotOffset());
            }

            base.LayoutElements();
            float totalWidth = 0;

            if (RectByRowsAndCols != null)
            {
                foreach (var rowsOrColumns in RectByRowsAndCols)
                {
                    float max = 0;

                    foreach (var rowOrCol in rowsOrColumns)
                    {
                        var currValue = rowOrCol.Value.Width;

                        if (currValue > max)
                        {
                            max = currValue;
                        }
                    }

                    totalWidth += max;
                }
            }

            return totalWidth;
        }

        private void CalcBounds(float size)
        {
            RectByRowsAndCols = new List<Dictionary<int, RectF>>();
            Dictionary<int, RectF> rowsOrColumn = new Dictionary<int, RectF>();
            RectByRowsAndCols.Add(rowsOrColumn);
            var axisLabels = Axis.VisibleLabels;
            var computedSizes = ComputedSizes;
            LabelsRect = new List<RectF>();

            if (axisLabels == null || computedSizes == null)
            {
                return;
            }

            if (axisLabels != null)
            {
                int length = axisLabels.Count;

                for (int i = 0; i < length; i++)
                {
                    var chartAxisLabel = axisLabels[i];
                    float position;
                    float coeff = (float)Axis.ValueToCoefficient(chartAxisLabel.Position);
                    var computedSize = computedSizes[i];
                    float height = computedSize.Height, width = computedSize.Width;
                    var labelStyle = chartAxisLabel.LabelStyle != null ? chartAxisLabel.LabelStyle : Axis.LabelStyle;
                    MarginTop = (float)labelStyle.Margin.Top;
                    MarginBottom = (float)labelStyle.Margin.Bottom;

                    position = ((1 - coeff) * size) - (height / 2);

                    position -= (MarginTop - MarginBottom) / 2;
                    position = OnAxisLabelAlignment(position, computedSizes[i].Height, labelStyle);
                    RectF rect = new RectF(0, position, width, height);
                    rowsOrColumn.Add(i, rect);
                    LabelsRect.Add(rect);
                }
            }

            if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift && computedSizes != null)
            {
                var computedSize = computedSizes[0];

                var rectByRowAndCols = RectByRowsAndCols.ElementAt(0);
                if (rectByRowAndCols[0].Bottom > size)
                {
                    rectByRowAndCols[0] = new RectF(0, size - computedSize.Height, computedSize.Width, computedSize.Height);
                }

                if (axisLabels != null)
                {
                    int index = axisLabels.Count - 1;
                    if (rectByRowAndCols[index].Top < 0)
                    {
                        computedSize = computedSizes[index];
                        rectByRowAndCols[index] = new RectF(0, 0, computedSize.Width, computedSize.Height);
                    }
                }
            }
            else if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Hide)
            {
                var rectByRowAndCols = RectByRowsAndCols.ElementAt(0);
                if (axisLabels != null && axisLabels.Count > 0)
                {
                    if (rectByRowAndCols[0].Bottom > size)
                    {
                        rectByRowAndCols[0] = RectF.Zero;
                        LabelsRect[0] = RectF.Zero;
                        axisLabels[0].IsVisible = false;
                    }

                    int index = axisLabels.Count - 1;
                    if (rectByRowAndCols[index].Top < 0)
                    {
                        rectByRowAndCols[index] = RectF.Zero;
                        LabelsRect[index] = RectF.Zero;
                        axisLabels[index].IsVisible = false;
                    }
                }
            }
        }

        #endregion
    }
}
