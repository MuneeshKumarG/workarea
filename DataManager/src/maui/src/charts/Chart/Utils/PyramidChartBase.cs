using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Dynamic;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    internal static class PyramidChartBase
    {
        #region Bindable properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty GapRatioProperty =
            BindableProperty.Create(nameof(IPyramidChartDependent.GapRatio), typeof(double), typeof(IPyramidChartDependent), null, BindingMode.Default, null, propertyChanged: OnGapRatioChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty PaletteBrushesProperty =
            BindableProperty.Create(nameof(IPyramidChartDependent.PaletteBrushes), typeof(IList<Brush>), typeof(IPyramidChartDependent), null, BindingMode.Default, null, propertyChanged: OnPaletteBrushesChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty StrokeProperty =
            BindableProperty.Create(nameof(IPyramidChartDependent.Stroke), typeof(Brush), typeof(IPyramidChartDependent), SolidColorBrush.Transparent, BindingMode.Default, null, propertyChanged: OnStrokeChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty =
           BindableProperty.Create(nameof(IPyramidChartDependent.StrokeWidth), typeof(double), typeof(IPyramidChartDependent), 2d, BindingMode.Default, null, propertyChanged: OnStrokeWidthChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty LegendIconProperty =
           BindableProperty.Create(nameof(IPyramidChartDependent.LegendIcon), typeof(ChartLegendIconType), typeof(IPyramidChartDependent), ChartLegendIconType.Circle, BindingMode.Default, null);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty TooltipTemplateProperty =
            BindableProperty.Create(nameof(IPyramidChartDependent.TooltipTemplate), typeof(DataTemplate), typeof(IPyramidChartDependent), null);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty EnableTooltipProperty =
            BindableProperty.Create(nameof(IPyramidChartDependent.EnableTooltip), typeof(bool), typeof(IPyramidChartDependent), false);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty SelectionBehaviorProperty =
          BindableProperty.Create(nameof(IPyramidChartDependent.SelectionBehavior), typeof(DataPointSelectionBehavior), typeof(IPyramidChartDependent), null, BindingMode.Default, null, OnSelectionBehaviorPropertyChanged);

        /// <summary>
        /// 
        /// </summary>
        public static readonly BindableProperty ShowDataLabelsProperty =
         BindableProperty.Create(nameof(IPyramidChartDependent.ShowDataLabels), typeof(bool), typeof(IPyramidChartDependent), false,BindingMode.Default, null,OnShowDataLabelsChanged);

        #endregion

        #region Call Back methods

        private static void OnGapRatioChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IPyramidChartDependent)bindable).OnGapRatioChanged((object)oldValue, (object)newValue);
        }

        private static void OnPaletteBrushesChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IPyramidChartDependent)bindable).OnPaletteBrushChanged((object)oldValue, (object)newValue);
        }

        private static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IPyramidChartDependent)bindable).OnStrokeChanged((object)oldValue, (object)newValue);
        }
        private static void OnStrokeWidthChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IPyramidChartDependent)bindable).OnStrOnStrokeWidthChanged((object)oldValue, (object)newValue); 
        }
        private static void OnSelectionBehaviorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IPyramidChartDependent)bindable).OnSelectionBehaviorPropertyChanged((object)oldValue, (object)newValue);
        }

        private static void OnShowDataLabelsChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((IPyramidChartDependent)bindable).OnShowDataLabelsChanged((object)oldValue, (object)newValue);
        }

        #endregion

        #region Internal methods

        internal static void InvokeSegmentsCollectionChanged(IPyramidChartDependent source, ObservableCollection<ChartSegment> Segments)
        {
            //TODO: Need to unhook the collection change.
            Segments.CollectionChanged += source.Segments_CollectionChanged;
        }

        internal static Brush? GetFillColor(IPyramidChartDependent source, int index)
        {
            return source.GetFillColor(index);
        }

        internal static void UpdateColor(this IPyramidChartDependent funnel)
        {
            funnel.UpdateColor();
        }

        internal static int GetDataPointIndex(this IPyramidChartDependent source, float x, float y)
        {
            return source.GetDataPointIndex(x, y);
        }

        #endregion
    }

    /// <summary>
    /// Layout and draw the data labels.
    /// </summary>
    internal class PyramidDataLabelHelper
    {
        const int spacing = 3;
        const float bendRatio = 0.05f;
        static float desiredWidth = float.MinValue;
        private readonly IPyramidChartDependent Chart;
        internal readonly ObservableCollection<IPyramidLabels> Segments;
        Dictionary<IPyramidLabels, RectF> labelRects;

        IPyramidDataLabelSettings dataLabelSettings { get => Chart.DataLabelSettings; }

        public PyramidDataLabelHelper(IPyramidChartDependent chart)
        {
            Chart = chart;
            Segments = new ObservableCollection<IPyramidLabels>();
            labelRects = new Dictionary<IPyramidLabels, RectF>();
        }

        //Laouting the data labels.
        internal void ArrangeElements()
        {
            //Datalabel clip bounds.
            var clip = new Rect(new Point(0, 0), Chart.AreaBounds.Size);
            var seriesBounds = Chart.SeriesBounds;
            desiredWidth = (float)(clip.Width - (seriesBounds.X + seriesBounds.Width));
            labelRects.Clear();

            foreach (var item in Segments)
            {
                if (dataLabelSettings != null)
                {
                    var style = dataLabelSettings.LabelStyle;
                    var labelSize = item.DataLabelSize;
                    var actualSize = item.ActualLabelSize;
                    var placement = dataLabelSettings.LabelPlacement == DataLabelPlacement.Auto ? item.Position : dataLabelSettings.LabelPlacement;
                    var labelRect = CalculateLabelRect(item, placement, actualSize);

                    if (item.IsLabelVisible)
                    {
                        var visible = clip.Contains(labelRect);

                        if (visible)
                        {
                            if (labelRects.ContainsKey(item))
                                labelRects[item] = labelRect;
                            else
                                labelRects.Add(item, labelRect);
                        }

                        item.IsLabelVisible = visible;
                    }

                    item.LabelRect = labelRect;
                }
            }
        }

        private Rect CalculateLabelRect(IPyramidLabels item, DataLabelPlacement placement, Size size)
        {
            var bounds = Chart.SeriesBounds;
            var labelRect = Rect.Zero;
            var acualPosition = DataLabelPlacement.Inner;
            item.LinePoints = null;
            switch (placement)
            {
                case DataLabelPlacement.Inner:
                case DataLabelPlacement.Center:
                case DataLabelPlacement.Auto:
                    var x = item.DataLabelX - size.Width / 2;
                    var y = item.DataLabelY - size.Height / 2;
                    labelRect = new Rect(new Point(x, y), size);
                    acualPosition = DataLabelPlacement.Inner;
                    break;
                case DataLabelPlacement.Outer:
                    var outerX = bounds.X + bounds.Width;
                    y = item.DataLabelY;

                    var linePoints = new Point[3];
                    linePoints[0] = new Point(outerX, y);
                    var bend = (outerX - bounds.Center.X) * bendRatio;
                    linePoints[1] = new Point(outerX - bend, y);
                    linePoints[2] = item.SlopePoint;

                    item.LinePoints = linePoints;
                    x = outerX;
                    y = item.DataLabelY - size.Height / 2;

                    var width = size.Width < desiredWidth ? size.Width : desiredWidth;
                    labelRect = new Rect(new Point(x, y), new Size( width , size.Height));
                    acualPosition = DataLabelPlacement.Outer;
                    break;
            }

            labelRect = ArrangeSmartLabel(item, acualPosition, labelRect);

            return labelRect;
        }

        private Rect ArrangeSmartLabel(IPyramidLabels item, DataLabelPlacement acualPosition, Rect labelRect)
        {
            foreach(var rect in labelRects)
            {
                var IsIntersected = labelRect.IsOverlap(rect.Value);
                if (IsIntersected && acualPosition == DataLabelPlacement.Auto)
                {
                    acualPosition = DataLabelPlacement.Outer;
                    item.IsLabelVisible = true;
                    labelRect = CalculateLabelRect(item, acualPosition, labelRect.Size);
                    return labelRect;
                }
                if (IsIntersected && acualPosition == DataLabelPlacement.Inner)
                {
                    item.IsLabelVisible = false;
                    return labelRect;
                }
                else if(IsIntersected && acualPosition == DataLabelPlacement.Outer)
                {
                    var adjusentRect = rect.Value;
                    item.IsLabelVisible = true;
                    item.DataLabelY = !Chart.ArrangeReverse ? adjusentRect.Y + adjusentRect.Height + spacing + (float)labelRect.Height / 2
                        : adjusentRect.Y - spacing - (float)labelRect.Height / 2;
                    labelRect = CalculateLabelRect(item, acualPosition, labelRect.Size);
                }
                else
                {
                    item.IsLabelVisible = true;
                }
            }

            return labelRect;
        }

        internal void OnDraw(ICanvas canvas, Rect dirtyRect)
        {
            //TODO:Check label empty
            //Check rotation angle
            //Canvas stroke size
            //Canvas stroke color
            //Set fill paint
            //Draw rectangle with fill & corner radius
            //Draw stroke with corner radius.
            //Canvas font color, set contrast fontcolor. 
            //Draw text.

            foreach(var item in Segments)
            {
                if (item.IsLabelVisible)
                {
                    //Draw Line
                    canvas.SaveState();
                    canvas.StrokeSize = 1;
                    canvas.StrokeColor = item.Fill?.ToColor();
                    canvas.StrokeLineCap = LineCap.Round;

                    var linePoint = item.LinePoints;
                    if (linePoint != null)
                    {
                        canvas.DrawLine(linePoint[0], linePoint[1]);
                        canvas.DrawLine(linePoint[1], linePoint[2]);
                    }

                    canvas.RestoreState();
                }
            }

            foreach (var item in Segments)
            {
                if (item.IsLabelVisible)
                {
                    var style = dataLabelSettings.LabelStyle;
                    var rect = item.LabelRect;
                    var angle = (float)style.Angle;
                    if (angle != 0)
                    {
                        angle = (float)(angle > 360 ? angle % 360 : angle);
                        canvas.SaveState();
                        canvas.Rotate(angle, (float)rect.X, (float)rect.Y);
                    }

                    if (style.StrokeWidth > 0)
                    {
                        canvas.StrokeSize = (float)style.StrokeWidth;
                        canvas.StrokeColor = style.Stroke.ToColor();
                    }

                    var fillcolor = style.IsBackgroundColorUpdated ? style.Background : dataLabelSettings.UseSeriesPalette ? item.Fill : style.Background;
                    DrawBackground(canvas, fillcolor ?? SolidColorBrush.Transparent, style, rect);

                    Color fontColor = style.TextColor;
                    if (fontColor == default(Color) || fontColor == Colors.Transparent)
                    {
                        fontColor = fillcolor == default(Brush) || fillcolor.ToColor() == Colors.Transparent ? 
                            (item.Position == DataLabelPlacement.Inner ? 
                            ChartUtils.GetContrastColor((item.Fill as SolidColorBrush).ToColor()) :
                            (Chart as IChart).GetTextColorBasedOnChartBackground()) :
                            ChartUtils.GetContrastColor((fillcolor as SolidColorBrush).ToColor());

                        //TODO: set animation value for fontcolor 
                        //Created new font family, as need to pass contrast text color for native font family rendering.
                        var labelStyle = new ChartDataLabelStyle();

                        //TODO: Need to add all values when it use for other cases. 
                        labelStyle.FontFamily = style.FontFamily;
                        labelStyle.FontAttributes = style.FontAttributes;
                        labelStyle.FontSize = style.FontSize;
                        labelStyle.Rect = style.Rect;
                        labelStyle.Margin = style.Margin;
                        labelStyle.TextColor = fontColor;
                        DrawLabel(canvas, item, rect, labelStyle);
                    }
                    else
                    {
                        DrawLabel(canvas, item, rect, style);
                    }
                }
            }
        }

        private void DrawLabel(ICanvas canvas, IPyramidLabels item, Rect rect, ChartDataLabelStyle style)
        {
#if ANDROID
            DrawLabel(canvas, item.DataLabel, new Point(rect.X + style.Margin.Left, rect.Y + style.Margin.Top + item.DataLabelSize.Height), style);
#else

            DrawLabel(canvas, item.DataLabel, new Point(rect.X + style.Margin.Left, rect.Y + style.Margin.Top), style);
#endif
        }

        private void DrawBackground(ICanvas canvas, Brush fill , ChartDataLabelStyle style, Rect backgroundRect)
        {
            canvas.SaveState();

            canvas.SetFillPaint(fill, backgroundRect);
            //Todo: Need to check condition for label background
            if (style.HasCornerRadius)
            {
                var cornerRadius = style.CornerRadius;
                canvas.FillRoundedRectangle(backgroundRect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
            }
            else
            {
                canvas.FillRectangle(backgroundRect);
            }

            //Todo: Need to check with border width and color in DrawLabel override method.
            if (style.StrokeWidth > 0 && style.IsStrokeColorUpdated)
            {
                if (style.HasCornerRadius)
                {
                    var cornerRadius = style.CornerRadius;
                    canvas.DrawRoundedRectangle(backgroundRect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
                }
                else
                    canvas.DrawRectangle(backgroundRect);
            }

            canvas.RestoreState();
        }

        private void DrawLabel(ICanvas canvas, string label, PointF point, ChartDataLabelStyle style)
        {
            canvas.DrawText(label, point.X , point.Y, style);
        }

        internal void AddLabel(IPyramidLabels segment)
        {
            if(!Segments.Contains(segment))
                Segments.Add(segment);
        }

        internal void ClearDefaultValues()
        {
            Segments.Clear();
            labelRects.Clear();
        }
    }
}

