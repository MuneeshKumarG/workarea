using Microsoft.Maui;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    internal class CartesianAxisLayout : AbsoluteLayout, IAxisLayout
    {
        #region Fields
        private readonly CartesianChartArea Area;
        private double left, bottom, right, top;
        private List<double>? leftSizes; 
        private List<double>? rightSizes; 
        private List<double>? topSizes;
        private List<double>? bottomSizes;
        private ChartAxisView AxisView;
        #endregion

        #region Internal Fields
        internal ObservableCollection<ChartAxis> VerticalAxes { get; set; }
        internal ObservableCollection<ChartAxis> HorizontalAxes { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        public CartesianAxisLayout(CartesianChartArea area)
        {
            this.Area = area;
            AxisView = new ChartAxisView(area);
            AbsoluteLayout.SetLayoutBounds(AxisView, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(AxisView, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            Add(AxisView);
            VerticalAxes = new  ObservableCollection<ChartAxis>();
            HorizontalAxes = new  ObservableCollection<ChartAxis>();
            Init();
        }
        #endregion

        #region Methods

        internal void AssignAxisToSeries()
        {
            var visibleSeries = Area.VisibleSeries;
            if (visibleSeries == null) return;
            
            UpdateAxisTransposed(); //Implement axis transpose.

            ClearActualAxis(visibleSeries); //Clear actual axis of the series if required. 
            UpdateActualAxis(visibleSeries);//Assign axis to series

            Area.UpdateStackingSeries(); //Calculate stacking series values. 
            
            UpdateSeriesRange(visibleSeries); // Create segment and update range.
        }

        private void ClearActualAxis(ReadOnlyObservableCollection<ChartSeries> visibleSeries)
        {
            if (Area.RequiredAxisReset)
            {
                foreach (CartesianSeries series in visibleSeries)
                {
                    if (series != null)
                    {
                        series.ActualXAxis = null;
                        series.ActualYAxis = null;
                    }
                }

                Area.RequiredAxisReset = false;
            }
        }

        internal void LayoutAxis(Rect bounds)
        {
            Measure(bounds.Size);

            var thickness = Area.PlotAreaMargin;
            Area.SeriesClipRect = new Rect(
                bounds.X + thickness.Left,
                bounds.Y + thickness.Top,
                bounds.Width - thickness.Left - thickness.Right, bounds.Height - thickness.Top - thickness.Bottom);
        }

        internal void InvalidateRender()
        {
            AxisView?.InvalidateDrawable();
        }

        #region Private Methods
        private void Init()
        {
            leftSizes = new List<double>();
            topSizes = new List<double>();
            rightSizes = new List<double>();
            bottomSizes = new List<double>();
        }

        public Size Measure(Size availableSize)
        {
            left = right = top = bottom = 0;

            leftSizes?.Clear();
            rightSizes?.Clear();
            topSizes?.Clear();
            bottomSizes?.Clear();

            Area.ActualSeriesClipRect = MeasureAxis(availableSize);

            Area.PlotAreaMargin = new Thickness(left, top, right, bottom);

            UpdateArrangeRect(availableSize);

            UpdateCrossingAxes();
            
            return Size.Zero;
        }

        private void UpdateAxisTransposed()
        {
            VerticalAxes = Area.IsTransposed ? Area.XAxes : new ObservableCollection<ChartAxis>(from axis in Area.YAxes select axis);
            HorizontalAxes = Area.IsTransposed ? new ObservableCollection<ChartAxis>(from axis in Area.YAxes select axis) : Area.XAxes;

            foreach (var axis in VerticalAxes)
            {
                axis.IsVertical = true;
            }

            foreach (var axis in HorizontalAxes)
            {
                axis.IsVertical = false;
            }
        }

        private void UpdateArrangeRect(Size availableSize)
        {
            if (availableSize == Size.Zero)
            {
                return;
            }

            double currRight = availableSize.Width - right;
            double currLeft = left;
            double currTop = top;
            double currBottom = availableSize.Height - bottom;
            bool isFirstLeft = true, isFirstTop = true, isFirstRight = true, isFirstBottom = true;

            foreach (ChartAxis verticalAxis in VerticalAxes)
            {
                var canRenderAtCross = verticalAxis.CanRenderNextToCrossingValue();
                if (verticalAxis.IsOpposed())
                {
                    currRight -= isFirstRight ? verticalAxis.InsidePadding : 0;
                    double axisWidth = verticalAxis.ComputedDesiredSize.Width;
                    double axisHeight = verticalAxis.ComputedDesiredSize.Height;

                    verticalAxis.ArrangeRect = canRenderAtCross ? new Rect(0, top, axisWidth, axisHeight) : new Rect(currRight, top, axisWidth, axisHeight);
                    if (!canRenderAtCross)
                    {
                        currRight += axisWidth;
                    }

                    isFirstRight = false;
                }
                else
                {
                    double axisWidth = isFirstLeft ? verticalAxis.ComputedDesiredSize.Width - verticalAxis.InsidePadding :
                            verticalAxis.ComputedDesiredSize.Width;
                    double axisHeight = verticalAxis.ComputedDesiredSize.Height;

                    if (!canRenderAtCross)
                    {
                        currLeft -= axisWidth;
                    }

                    verticalAxis.ArrangeRect = canRenderAtCross ? new Rect(0, top, axisWidth, axisHeight) : new Rect(currLeft, top, axisWidth, axisHeight);
                    isFirstLeft = false;
                }
            }

            foreach (ChartAxis horizontalAxis in HorizontalAxes)
            {
                var canRenderAtCross = horizontalAxis.CanRenderNextToCrossingValue();
                if (horizontalAxis.IsOpposed())
                {
                    double axisWidth = horizontalAxis.ComputedDesiredSize.Width;
                    double axisHeight = isFirstTop ? horizontalAxis.ComputedDesiredSize.Height - horizontalAxis.InsidePadding :
                            horizontalAxis.ComputedDesiredSize.Height;
                    if (!canRenderAtCross)
                    {
                        currTop -= axisHeight;
                    }

                    horizontalAxis.ArrangeRect = canRenderAtCross ? new Rect(left, 0, axisWidth, axisHeight) : new Rect(left, currTop, axisWidth, axisHeight);
                    isFirstTop = false;
                }
                else
                {
                    currBottom -= isFirstBottom ? horizontalAxis.InsidePadding : 0;
                    double axisWidth = horizontalAxis.ComputedDesiredSize.Width;
                    double axisHeight = horizontalAxis.ComputedDesiredSize.Height;

                    horizontalAxis.ArrangeRect = canRenderAtCross ? new Rect(left, 0, axisWidth, axisHeight) : new Rect(left, currBottom, axisWidth, axisHeight);

                    if (!canRenderAtCross)
                    {
                        currBottom += axisHeight;
                    }

                    isFirstBottom = false;
                }
            }
        }

        private void UpdateCrossingAxes()
        {
            foreach (var axis in VerticalAxes)
            {
                if (axis.CanRenderNextToCrossingValue())
                {
                    var isOpposed = axis.IsOpposed();
                    var horizontalCrossing = ValidateMinMaxWithAxisCrossingValue(axis);
                    var axisWidth = axis.ComputedDesiredSize.Width;
                    var axisHeight = axis.ComputedDesiredSize.Height;

                    axis.ArrangeRect = isOpposed ? new Rect(horizontalCrossing + left, top, axisWidth, axisHeight) : new Rect(horizontalCrossing - (axisWidth - left), top, axisWidth, axisHeight);
                }
            }

            foreach (var axis in HorizontalAxes)
            {
                if (axis.CanRenderNextToCrossingValue())
                {
                    var isOpposed = axis.IsOpposed();
                    var verticalCrossing = ValidateMinMaxWithAxisCrossingValue(axis);
                    var axisWidth = axis.ComputedDesiredSize.Width;
                    var axisHeight = axis.ComputedDesiredSize.Height;

                    axis.ArrangeRect = isOpposed ? new Rect(left, verticalCrossing - (axisHeight - top), axisWidth, axisHeight) : new Rect(left, verticalCrossing + top, axisWidth, axisHeight);
                }
            }
        }

        private double ValidateMinMaxWithAxisCrossingValue(ChartAxis currentAxis)
        {
            double crossingPosition;
            var associatedAxis = currentAxis.GetCrossingAxis(Area);
            if (associatedAxis != null)
            {
                var minimum = associatedAxis.ActualRange.Start;
                var maximum = associatedAxis.ActualRange.End;
                crossingPosition = currentAxis.ActualCrossingValue < minimum ? minimum : currentAxis.ActualCrossingValue > maximum ? maximum : currentAxis.ActualCrossingValue;
                return associatedAxis.ValueToPoint(crossingPosition);
            }

            return double.NaN;
        }

        private Rect MeasureAxis(Size size)
        {
            bool needLayout = true;
            bool isFirstLayout = true;
            Rect currentClipRect;
            Rect seriesClipRect = new Rect(0, 0, size.Width, size.Height);

            while (needLayout)
            {
                top = bottom = left = right = 0f;
                needLayout = false;

                leftSizes?.Clear();
                rightSizes?.Clear();

                MeasureVerticalAxis(VerticalAxes, new Size(size.Width, seriesClipRect.Height));

                left = leftSizes != null ? leftSizes.Sum() : 0;
                right = rightSizes != null ? rightSizes.Sum() : 0;

                top = topSizes != null && topSizes.Count > 0 ? topSizes.Sum() : 0;
                bottom = bottomSizes != null && bottomSizes.Count > 0 ? bottomSizes.Sum() : 0;
                var thickness = new Thickness(left, top, right, bottom);
                currentClipRect = (new Rect(0, 0, size.Width, size.Height)).SubtractThickness(thickness);

                if (Math.Abs(seriesClipRect.Width - currentClipRect.Width) > 0.5 || isFirstLayout)
                {
                    topSizes?.Clear();
                    bottomSizes?.Clear();

                    seriesClipRect = currentClipRect;

                    MeasureHorizontalAxis(HorizontalAxes, new Size(seriesClipRect.Width, size.Height));

                    top = bottom = 0f;
                    top = topSizes != null ? topSizes.Sum() : 0;
                    bottom = bottomSizes != null ? bottomSizes.Sum() : 0;
                    currentClipRect = (new Rect(0, 0, size.Width, size.Height)).SubtractThickness(new Thickness(left, top, right, bottom));
                    needLayout = Math.Abs(seriesClipRect.Height - currentClipRect.Height) > 0.5;
                    seriesClipRect = currentClipRect;
                }

                isFirstLayout = false;
            }

            return seriesClipRect;
        }

        private void MeasureHorizontalAxis(ObservableCollection<ChartAxis> axes, Size size)
        {
            bool isFirstTop = true, isFirstBottom = true;

            foreach (ChartAxis chartAxis in axes)
            {
                chartAxis.ComputeSize(size);
                if (!chartAxis.CanRenderNextToCrossingValue())
                {
                    if (chartAxis.IsOpposed())
                    {
                        topSizes?.Add(isFirstTop ? chartAxis.ComputedDesiredSize.Height - chartAxis.InsidePadding :
                                chartAxis.ComputedDesiredSize.Height);
                        isFirstTop = false;
                    }
                    else
                    {
                        bottomSizes?.Add(isFirstBottom ? chartAxis.ComputedDesiredSize.Height - chartAxis.InsidePadding :
                                chartAxis.ComputedDesiredSize.Height);
                        isFirstBottom = false;
                    }
                }
            }
        }

        private void MeasureVerticalAxis(ObservableCollection<ChartAxis>? axes, Size size)
        {
            bool isFirstLeft = true;
            bool isFirstRight = true;

            if(axes == null)
            {
                return;
            }

            foreach (ChartAxis chartAxis in axes)
            {
                chartAxis.ComputeSize(size);
                if (!chartAxis.CanRenderNextToCrossingValue())
                {
                    if (chartAxis.IsOpposed())
                    {
                        rightSizes?.Add(isFirstRight ? chartAxis.ComputedDesiredSize.Width - chartAxis.InsidePadding :
                                chartAxis.ComputedDesiredSize.Width);
                        isFirstRight = false;
                    }
                    else
                    {
                        leftSizes?.Add(isFirstLeft ? chartAxis.ComputedDesiredSize.Width - chartAxis.InsidePadding :
                                chartAxis.ComputedDesiredSize.Width);
                        isFirstLeft = false;
                    }
                }
            }
        }

        public void OnDraw(ICanvas canvas)
        {
           
        }

        private void UpdateActualAxis(ReadOnlyObservableCollection<ChartSeries> visibleSeries)
        {
            foreach (CartesianSeries series in visibleSeries)
            {
                if (series != null)
                {
                    if (series.ActualXAxis == null)
                    {
                        var xName = series.XAxisName;
                        var axis = string.IsNullOrEmpty(xName) ? Area.PrimaryAxis : (GetAxisByName(xName, HorizontalAxes) ?? Area.PrimaryAxis);
                        series.ActualXAxis = axis;
                        axis?.AddRegisteredSeries(series);
                    }

                    if (series.ActualYAxis == null)
                    {
                        var yName = series.YAxisName;
                        var axis = string.IsNullOrEmpty(yName) ? Area.SecondaryAxis : (GetAxisByName(yName, VerticalAxes) ?? Area.SecondaryAxis);
                        series.ActualYAxis = axis;
                        axis?.AddRegisteredSeries(series);
                    }

                    series.UpdateAssociatedAxes();

                    if (series.ActualXAxis is CategoryAxis categoryAxis && !categoryAxis.ArrangeByIndex)
                    {
                        categoryAxis.GroupData();
                    }
                }
            }
        }

        private void UpdateSeriesRange(ReadOnlyObservableCollection<ChartSeries> visibleSeries)
        {
            foreach (CartesianSeries series in visibleSeries)
            {
                series.SegmentsCreated = false;

                if (!series.SegmentsCreated) //creates segment if segmentscreted is false. 
                {
                    series.XRange = DoubleRange.Empty;
                    series.YRange = DoubleRange.Empty;

                    if (series.SbsInfo.IsEmpty)
                    {
                        series.InvalidateSideBySideSeries();
                    }

                    if (series.IsSideBySide)
                    {
                        Area.CalculateSbsPosition();
                    }

                    InternalCreateSegments(series);
                }

                series.UpdateRange();
            }
        }

        private void InternalCreateSegments(ChartSeries series)
        {
            var area = Area.PlotArea as ChartPlotArea;

            if (area != null)
            {
                foreach (SeriesView seriesView in area.SeriesViews.Children)
                {
                    if (seriesView != null && seriesView.IsVisible && series == seriesView.Series)
                    {
                        seriesView.InternalCreateSegments();
                    }
                }
            }
        }

        private ChartAxis? GetAxisByName(string name, ObservableCollection<ChartAxis>? axes)
        {
            var item = (from x in axes where x.Name == name select x).ToList();
            if (item != null && item.Count > 0)
                return item[0];

            return null;
        }
        #endregion
        #endregion
    }
}
