using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class ChartAxis : BindableObject
    {
        #region Fields
        private const int CRoundDecimals = 12;
        private bool isVertical;
        private RectF arrangeRect;
        private double previousLabelCoefficientValue = -1;
        private CartesianChartArea? cartesianArea;

        #region labelFormats
        private const string dayFormat = "MMM - dd";
        private const string monthFormat = "MMM-yyyy";
        private const string yearFormat = "yyyy";
        private const string hourFormat = "hh:mm:ss tt";
        private const string minutesFormat = "hh:mm:ss";
        private const string secondsFormat = "hh:mm:ss";
        private const string milliSecondsFormat = "ss.fff";
        private const string defaultFormat = "dd/MMM/yyyy";
        #endregion
        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        internal ObservableCollection<ChartAxisLabel> VisibleLabels { get; set; }

        #endregion

        #region Internal Properties

        internal float LeftOffset { get; set; }

        internal float TopOffset { get; set; }

        internal CartesianAxisLabelsRenderer? AxisLabelsRenderer { get; set; }

        internal CartesianAxisElementRenderer? AxisElementRenderer { get; set; }

        internal List<double> TickPositions { get; set; }

        internal CartesianAxisRenderer? AxisRenderer { get; set; }

        internal double ActualPlotOffset { get; set; }

        internal double ActualPlotOffsetStart { get; set; }

        internal double ActualPlotOffsetEnd { get; set; }

        internal List<CartesianSeries> RegisteredSeries { get; set; }

        internal List<ChartAxis> AssociatedAxes { get; }

        internal int SideBySideSeriesCount { get; set; }

        internal Size ComputedDesiredSize { get; set; }

        internal double InsidePadding { get; set; }

        internal Size AvailableSize { get; set; }

        internal double ActualInterval { get; set; }

        internal DoubleRange ActualRange { get; set; }

        internal double VisibleInterval { get; set; }

        internal static readonly int[] IntervalDivs = { 10, 5, 2, 1 };

        internal bool SmallTickRequired { get; set; }

        internal RectF RenderedRect { get; set; }

        internal double AxisInterval { get; set; } = double.NaN;

        internal DoubleRange VisibleRange { get; set; }

        internal double ActualCrossingValue { get; set; } = double.NaN;

        internal bool IsOpposed()
        {
            var crossAxis = GetCrossingAxis(cartesianArea);
            if (crossAxis != null)
            {
                var isInversedAxis = crossAxis.IsInversed;
                return ActualCrossingValue == double.MaxValue && !isInversedAxis ||
                    ((double.IsNaN(ActualCrossingValue) || ActualCrossingValue == double.MinValue) && isInversedAxis);
            }

            return false;
        }

        internal bool IsVertical
        {
            get
            {
                return isVertical;
            }

            set
            {
                if (isVertical != value)
                {
                    isVertical = value;
                }

               // UpdateLayout();
            }
        }

        internal Rect ArrangeRect
        {
            get { return arrangeRect; }
            set
            {
                arrangeRect = value;
                if (!IsVertical)
                {
                    double left = arrangeRect.Left + GetActualPlotOffsetStart();
                    double width = Math.Max(0, arrangeRect.Width - GetActualPlotOffset());
                    double top = arrangeRect.Top;
                    RenderedRect = new Rect(left, top, width, arrangeRect.Height);
                }
                else
                {
                    double left = arrangeRect.Left;
                    double top = arrangeRect.Top + GetActualPlotOffsetEnd();
                    double height = Math.Max(0, arrangeRect.Height - GetActualPlotOffset());
                    RenderedRect = new Rect(left, top, arrangeRect.Width, height);
                }

                LeftOffset = RenderedRect.Left - arrangeRect.Left;
                TopOffset = RenderedRect.Top - arrangeRect.Top;
                if (AxisRenderer != null)
                {
                    AxisRenderer.Layout(new Size(arrangeRect.Width, arrangeRect.Height));
                }
            }
        }

        internal CartesianChartArea? Area
        {

            get { return cartesianArea; }
            set
            {
                if (cartesianArea != value)
                {
                    //TODO: dispose events and dispose other values.
                }

                if (value is CartesianChartArea chartArea)
                    cartesianArea = chartArea;
            }
        }

        internal CartesianChartArea? CartesianArea { get => cartesianArea; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartAxis()
        {
#if WINDOWS || IOS || MACCATALYST
//Todo: Remove this code, After ClipsToBounds works in iOS and Windows.
            EdgeLabelsDrawingMode = EdgeLabelsDrawingMode.Shift; 
#endif

            LabelsIntersectAction = AxisLabelsIntersectAction.Hide;
            VisibleLabels = new ObservableCollection<ChartAxisLabel>();
            //TODO: need dispose on clrea axis.
            VisibleLabels.CollectionChanged += VisibleLabels_CollectionChanged; ;

            TickPositions = new List<double>();
            AxisLineStyle = new ChartLineStyle();
            LabelStyle = new ChartAxisLabelStyle();

            MajorGridLineStyle = new ChartLineStyle();
            MajorGridLineStyle.Stroke = new SolidColorBrush(Color.FromArgb("#EDEFF1"));

            MajorTickStyle = new ChartAxisTickStyle();
            RegisteredSeries = new List<CartesianSeries>();
            AssociatedAxes = new List<ChartAxis>();
        }
        #endregion

        #region Methods

        #region Public Methods
        /// <summary>
        /// 
        /// </summary>
        public float ValueToPoint(double value)
        {
            float coefficient = ValueToCoefficient(value);

            if (!IsVertical)
            {
                return (RenderedRect.Width * coefficient) + LeftOffset;
            }

            return (RenderedRect.Height * (1 - coefficient)) + TopOffset;
        }

        /// <summary>
        /// 
        /// </summary>
        public double PointToValue(double x, double y)
        {
            if (Area != null)
            {
                if (!IsVertical)
                {
                    return CoefficientToValue((x - LeftOffset) / RenderedRect.Width);
                }

                return CoefficientToValue(1d - ((y - TopOffset) / RenderedRect.Height));
            }

            return double.NaN;
        }
        #endregion

        #region Protected Overrides

        /// <summary>
        /// 
        /// </summary>
        protected virtual Size ComputeDesiredSize(Size availableSize)
        {
            if (AxisRenderer != null)
                return AxisRenderer.ComputeDesiredSize(availableSize);
            return availableSize;
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal double GetActualDesiredIntervalsCount(Size availableSize)
        {
            double size = !IsVertical ? availableSize.Width : availableSize.Height;
            double adjustedDesiredIntervalsCount = size * (!IsVertical ? 0.54 : 1.0) * MaximumLabels;
            var actualDesiredIntervalsCount = Math.Max(adjustedDesiredIntervalsCount / 100, 1.0);

            return actualDesiredIntervalsCount;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual double CalculateNiceInterval(DoubleRange actualRange, Size availableSize)
        {
            var delta = actualRange.Delta;
            var actualDesiredIntervalsCount = GetActualDesiredIntervalsCount(availableSize);
            var niceInterval = delta / actualDesiredIntervalsCount;
            var minInterval = Math.Pow(10, Math.Floor(Math.Log10(niceInterval)));

            foreach (int mul in IntervalDivs)
            {
                double currentInterval = minInterval * mul;

                if (actualDesiredIntervalsCount < (delta / currentInterval))
                {
                    break;
                }

                niceInterval = currentInterval;
            }

            return niceInterval;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual double CalculateActualInterval(DoubleRange range, Size availableSize)
        {
            return 1.0;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            return range;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual DoubleRange CalculateVisibleRange(DoubleRange actualRange, Size availableSize)
        {
            var visibleRange = actualRange;
            if (ZoomFactor < 1)
            {
                DoubleRange baseRange = actualRange;
                double start = baseRange.Start + (ZoomPosition * baseRange.Delta);
                double end = start + (ZoomFactor * baseRange.Delta);

                if (start < baseRange.Start)
                {
                    end = end + (baseRange.Start - start);
                    start = baseRange.Start;
                }

                if (end > baseRange.End)
                {
                    start = start - (end - baseRange.End);
                    end = baseRange.End;
                }

                visibleRange = new DoubleRange(start, end);
            }

            return visibleRange;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual DoubleRange CalculateActualRange()
        {
            var range = DoubleRange.Empty;
            var visibleSeries = cartesianArea?.VisibleSeries;

            if (visibleSeries == null)
            {
                return range;
            }

            foreach (CartesianSeries series in visibleSeries)
            {
                if (series.ActualXAxis == this)
                {
                    range += series.VisibleXRange;
                }
                else if (series.ActualYAxis == this)
                {
                    range += series.VisibleYRange;
                }
            }

            //TODO: Modify range on technical indicator or trendline added. 
            return range;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual double CalculateVisibleInterval(DoubleRange visibleRange, Size availableSize)
        {
            if (ZoomFactor < 1 || ZoomPosition > 0)
            {
                return EnableAutoIntervalOnZooming
                        ? CalculateNiceInterval(visibleRange, availableSize)
                        : ActualInterval;
            }

            return ActualInterval;
        }

        /// <summary>
        /// Method implementation for Generate Labels in ChartAxis
        /// </summary>
        internal virtual void GenerateVisibleLabels()
        {
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Calculate axis desired size.
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        internal void ComputeSize(Size size)
        {
            AvailableSize = size;

            var plotSize = GetPlotSize(size);

            CalculateRangeAndInterval(plotSize);
            if (this.IsVisible)
            {
                UpdateRenderers();
                UpdateLabels(); //Generate visible labels
                ComputedDesiredSize = ComputeDesiredSize(size);
            }
            else
            {
                //TODO: Need to validate desired size.
                UpdateLabels(); //Generate visible labels
                ActualPlotOffsetStart = double.IsNaN(PlotOffsetStart) ? 0f : PlotOffsetStart;
                ActualPlotOffsetEnd = double.IsNaN(PlotOffsetEnd) ? 0f : PlotOffsetEnd;
                ActualPlotOffset = ActualPlotOffsetStart + ActualPlotOffsetEnd;
                InsidePadding = 0;
                AxisRenderer = null;
                ComputedDesiredSize = !IsVertical ? new Size(size.Width, 0) : new Size(0, size.Height);
            }
        }

        internal void UpdateLayout()
        {
            if (Area != null)
            {
                Area.NeedsRelayout = true;
                Area.ScheduleUpdateArea();
            }            
        }

        internal bool CanRenderNextToCrossingValue()
        {
            return RenderNextToCrossingValue
                && !double.IsNaN(ActualCrossingValue)
                && ActualCrossingValue != double.MinValue
                && ActualCrossingValue != double.MaxValue;
        }

        internal ChartAxis? GetCrossingAxis(CartesianChartArea? area)
        {
            if (area == null) return null;

            if (CrossAxisName != null && CrossAxisName.Length != 0)
            {
                var axislayout = area.AxisLayout;
                var axes = IsVertical ? axislayout.HorizontalAxes : axislayout.VerticalAxes;
                foreach (var axis in axes)
                {
                    if (axis.Name != null && axis.Name.Equals(CrossAxisName))
                    {
                        return axis;
                    }
                }
            }

            if (AssociatedAxes.Count > 0)
            {
                return AssociatedAxes[0];
            }
            else
            {
                return isVertical ^ (RegisteredSeries.Count > 0 && area.IsTransposed)
                                                ? area.PrimaryAxis
                                                    : area.SecondaryAxis;
            }
        }

        internal virtual DoubleRange AddDefaultRange(double start)
        {
            return new DoubleRange(start, start + 1);
        }

        //Axes change on register series change. 
        internal void AddRegisteredSeries(CartesianSeries series, int index)
        {
            if (!RegisteredSeries.Contains(series))
            {
                RegisteredSeries.Insert(index, series);
            }
        }

        internal double GetActualPlotOffsetStart()
        {
            return ActualPlotOffset == 0 ? ActualPlotOffsetStart : ActualPlotOffset;
        }

        internal double GetActualPlotOffsetEnd()
        {
            return ActualPlotOffset == 0 ? ActualPlotOffsetEnd : ActualPlotOffset;
        }

        internal double GetActualPlotOffset()
        {
            return ActualPlotOffset == 0 ? ActualPlotOffsetStart + ActualPlotOffsetEnd : ActualPlotOffset * 2;
        }

        internal void RemoveRegisteredSeries(CartesianSeries series)
        {
            RegisteredSeries.Remove(series);
            if (series != null)
            {
                var xAxis = series.ActualXAxis;
                var area = cartesianArea;
                if (area != null && xAxis != null && series.ActualYAxis is RangeAxisBase yAxis)
                {
                    if (!area.YAxes.Contains(yAxis))
                    {
                        xAxis.AssociatedAxes.Remove(yAxis);
                    }

                    if (!area.XAxes.Contains(xAxis))
                    {
                        yAxis.AssociatedAxes.Remove(xAxis);
                    }
                }
            }
        }

        internal void CalculateRangeAndInterval(Size plotSize)
        {
            DoubleRange range = ValidateRange(CalculateActualRange());

            ActualInterval = CalculateActualInterval(range, plotSize);
            ActualRange = ApplyRangePadding(range, ActualInterval);

            var visibleRange = CalculateVisibleRange(ActualRange, plotSize);
            VisibleRange = visibleRange.IsEmpty ? ActualRange : visibleRange;

            var visibleInterval = CalculateVisibleInterval(VisibleRange, plotSize);
            VisibleInterval = double.IsNaN(visibleInterval) || visibleInterval == 0 ? ActualInterval : visibleInterval;

            if (ActualRangeChanged != null)
            {
                RaiseActualRangeChangedEvent(visibleRange, plotSize);
            }

            ZoomPosition = (VisibleRange.Start - ActualRange.Start) / ActualRange.Delta;
            ZoomFactor = (VisibleRange.End - VisibleRange.Start) / ActualRange.Delta;
        }

        /// <summary>
        ///  Update Auto Scrolling Delta value  based on auto scrolling delta mode option.
        /// </summary>
        /// <param name="actualRange"></param>
        /// <param name="scrollingDelta"></param>
        internal virtual void UpdateAutoScrollingDelta(DoubleRange actualRange, double scrollingDelta)
        {
            switch (AutoScrollingMode)
            {
                case ChartAutoScrollingMode.Start:
                    VisibleRange = new DoubleRange(ActualRange.Start, actualRange.Start + scrollingDelta);
                    ZoomFactor = VisibleRange.Delta / ActualRange.Delta;
                    ZoomPosition = 0;
                    break;
                case ChartAutoScrollingMode.End:
                    VisibleRange = new DoubleRange(actualRange.End - scrollingDelta, ActualRange.End);
                    ZoomFactor = VisibleRange.Delta / ActualRange.Delta;
                    ZoomPosition = 1 - ZoomFactor;
                    break;
            }
        }

        internal bool RenderRectContains(float x, float y)
        {
            Rect rect;
            var labelsRect = AxisLabelsRenderer?.LabelLayout?.LabelsRect;

            if (labelsRect == null || labelsRect.Count == 0)
            {
                return false;
            }

            int count = labelsRect.Count;

            if (IsVertical)
            {
                rect = new Rect(ArrangeRect.Left, ArrangeRect.Top - (labelsRect[count - 1].Height / 2), ArrangeRect.Width, ArrangeRect.Height + (labelsRect[count - 1].Height / 2) + (labelsRect[0].Height / 2));
            }
            else
            {
                rect = new Rect(ArrangeRect.Left - (labelsRect[0].Width / 2), ArrangeRect.Top, ArrangeRect.Width + (labelsRect[0].Width / 2) + (labelsRect[count - 1].Width / 2), ArrangeRect.Height);
            }

            if (LabelStyle.LabelsPosition == AxisElementPosition.Inside)
            {
                var isOpposed = IsOpposed();
                if (IsVertical && !isOpposed)
                {
                    rect.Width += InsidePadding;
                }
                else if (!IsVertical && isOpposed)
                {
                    rect.Height += InsidePadding;
                }
            }

            return rect.Contains(x, y);
        }

        internal static string GetActualLabelContent(object? value, string labelFormat)
        {
            var format = labelFormat ?? "";
            double position = double.NaN;
            
            if (value != null && double.TryParse(value.ToString(), out position))
            {
                position = Math.Round(position, CRoundDecimals);
            }

            if (string.IsNullOrEmpty(format))
            {
                return position.ToString();
            }

            return position.ToString(format);
        }

        internal bool CanDrawMajorGridLines()
        {
            return ShowMajorGridLines && MajorGridLineStyle.CanDraw();
        }

        /// <summary>
        /// Gets the specific formatted label depending on the interval type.
        /// </summary>
        /// <param name = "actualIntervalType" > The interval type.</param>
        /// <returns>The label format.</returns>
        internal string GetSpecificFormatedLabel(DateTimeIntervalType actualIntervalType)
        {
            switch (actualIntervalType)
            {
                case DateTimeIntervalType.Days:
                    return dayFormat;
                case DateTimeIntervalType.Months:
                    return monthFormat;
                case DateTimeIntervalType.Years:
                    return yearFormat;
                case DateTimeIntervalType.Hours:
                    return hourFormat;
                case DateTimeIntervalType.Minutes:
                    return minutesFormat;
                case DateTimeIntervalType.Seconds:
                    return secondsFormat;
                case DateTimeIntervalType.Milliseconds:
                    return milliSecondsFormat;
            }

            return defaultFormat;
        }

        internal static string GetFormattedAxisLabel(string labelFormat, object? currentDate)
        {
            double value = double.NaN;
            if (currentDate != null && double.TryParse(currentDate.ToString(), out value))
            {
                return DateTime.FromOADate(value).ToString(labelFormat);
            }

            return string.Empty;
        }

        #endregion

        #region Private Methods

        private DoubleRange ValidateRange(DoubleRange range)
        {
            if (range.IsEmpty)
            {
                range = AddDefaultRange(0d);
            }

            if (ChartUtils.EqualDoubleValues(range.Start, range.End))
            {
                range = AddDefaultRange(range.Start);
            }

            return range;
        }

        private Size GetPlotSize(Size availableSize)
        {
            if (!IsVertical)
            {
                return new Size(availableSize.Width - GetActualPlotOffset(), availableSize.Height);
            }
            else
            {
                return new Size(availableSize.Width, availableSize.Height - GetActualPlotOffset());
            }
        }

        //TODO: Check collection changes working fine or not
        private void VisibleLabels_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems == null)
                {
                    return;
                }

                var item = e.NewItems[0] as ChartAxisLabel;

                if (item != null)
                {
                    var currentLabelCoeffientValue = ValueToCoefficient(item.Position);

                    //Checking the position of the current label already has a label or not.
                    if (double.Equals(Math.Round(previousLabelCoefficientValue, 3), Math.Round(currentLabelCoeffientValue, 3)))
                    {
                        VisibleLabels?.Remove(item);
                    }
                    else
                    {
                        previousLabelCoefficientValue = currentLabelCoeffientValue;
                    }

                    InvokeLabelCreated(item);
                }
            }
        }

        /// <summary>
        /// Invoke label created overrides and created event. 
        /// </summary>
        /// <param name="item"></param>
        private void InvokeLabelCreated(ChartAxisLabel item)
        {
            OnLabelCreated(item);

            if (LabelCreated != null)
            {
                var content = item.Content != null ? item.Content.ToString() : string.Empty;
                var args = new ChartAxisLabelEventArgs(content, item.Position);
                LabelCreated(this, args);
                content = args.Label != null ? args.Label : string.Empty;
                item.Content = content;
                if (args.LabelStyle != null)
                {
                    item.LabelStyle = args.LabelStyle;
                }
            }
        }

        private void UpdateRenderers()
        {
            if (AxisLabelsRenderer == null)
            {
                AxisLabelsRenderer = new CartesianAxisLabelsRenderer(this);
            }

            if (AxisElementRenderer == null)
            {
                AxisElementRenderer = new CartesianAxisElementRenderer(this);
            }

            if (AxisRenderer != null)
            {
                AxisRenderer.LayoutCalculators.Clear();
            }
            else
            {
                AxisRenderer = new CartesianAxisRenderer(this);
            }

            AxisRenderer.LayoutCalculators.Add(AxisLabelsRenderer);
            AxisRenderer.LayoutCalculators.Add(AxisElementRenderer);
        }

        private void UpdateLabels()
        {
            if (VisibleRange.Delta > 0)
            {
                previousLabelCoefficientValue = -1;
                VisibleLabels?.Clear();
                TickPositions.Clear();
            }

            if (MaximumLabels > 0)
            {
                GenerateVisibleLabels();
            }
        }

        private void RaiseActualRangeChangedEvent(DoubleRange visibleRange, Size plotSize)
        {
            if (ActualRangeChanged == null) return;

            var actualRangeChangedEvent = new ActualRangeChangedEventArgs(ActualRange.Start, ActualRange.End);
            actualRangeChangedEvent.Axis = this;
            //Hook ActualRangeChanged event.
            ActualRangeChanged(this, actualRangeChangedEvent);

            var customActualRange = actualRangeChangedEvent.GetActualRange();

            if (customActualRange != ActualRange)
            {
                ActualRange = customActualRange;
                ActualInterval = CalculateActualInterval(ActualRange, plotSize);
            }

            visibleRange = actualRangeChangedEvent.GetVisibleRange();

            if (visibleRange.IsEmpty)
            {
                VisibleRange = CalculateVisibleRange(ActualRange, plotSize);
                VisibleInterval = ActualInterval;
            }
            else
            {
                VisibleRange = visibleRange;
                VisibleInterval = EnableAutoIntervalOnZooming ? CalculateNiceInterval(VisibleRange, plotSize) : ActualInterval;
            }
        }

        private void InitTitle(ChartAxisTitle title)
        {
            title.Axis = this;
        }

        #endregion
        
        #endregion
    }
}
