#if WinUI
using RectF = Windows.Foundation.Rect;
using CartesianChartArea = Syncfusion.UI.Xaml.Charts.ChartBase;
using Windows.Foundation;
using Microsoft.UI.Xaml.Controls;
#else
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
#endif
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
#if WinUI
    public abstract partial class ChartAxis : Control
#else
    public abstract partial class ChartAxis : BindableObject
#endif
    {
        #region Fields
        private const int CRoundDecimals = 12;
        private bool isVertical;
        private RectF arrangeRect;
        private double previousLabelCoefficientValue = -1;
        private CartesianChartArea? cartesianArea;

        internal double ActualAutoScrollDelta { get; set; } = double.NaN;
        internal bool CanAutoScroll { get; set; } = false;

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

        #region Internal Properties

        /// <summary>
        /// 
        /// </summary>
        internal ObservableCollection<ChartAxisLabel> VisibleLabels { get; set; }

        internal float LeftOffset { get; set; }

        internal float TopOffset { get; set; }

        internal List<double> TickPositions { get; set; }

        internal double ActualPlotOffset { get; set; }

        internal double ActualPlotOffsetStart { get; set; }

        internal double ActualPlotOffsetEnd { get; set; }

        internal List<ChartAxis> AssociatedAxes { get; }

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

                LeftOffset = (float)(RenderedRect.Left - arrangeRect.Left);
                TopOffset = (float)(RenderedRect.Top - arrangeRect.Top);
                UpdateAxisLayout(); // This method created in both WinUI and MAUI partial classes.
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
#if WinUI
                if (cartesianArea is SfPolarChart)
                {
                    PolarAngle = (cartesianArea as SfPolarChart).StartAngle;
                }
#endif
            }
        }

        internal CartesianChartArea? CartesianArea { get => cartesianArea; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartAxis"/> class.
        /// </summary>
        public ChartAxis()
        {
            TickPositions = new List<double>();
            VisibleLabels = new ObservableCollection<ChartAxisLabel>();
            //TODO: need dispose on clrea axis.
            VisibleLabels.CollectionChanged += VisibleLabels_CollectionChanged;
            AssociatedAxes = new List<ChartAxis>();
            LabelsIntersectAction = AxisLabelsIntersectAction.Hide;
#if !WinUI
            RegisteredSeries = new List<CartesianSeries>();
#endif
            this.InitializeConstructor();
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
                return (float)(RenderedRect.Width * coefficient) + LeftOffset;
            }

            return (float)(RenderedRect.Height * (1 - coefficient)) + TopOffset;
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

            //// TODO: Need to check the reason why set niceInterval as 20 for the WinUI Stacked100 series.
#if WinUI
            if (niceInterval <= 10 && actualRange.Start < 0 && this.RegisteredSeries.Count > 0 && RegisteredSeries.All(series => series is ChartSeries && (series as ChartSeries).IsStacked100))
                niceInterval = 20;
#endif
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
#if WinUI
            if (RegisteredSeries.Count > 0 && RegisteredSeries[0] is PolarSeries)
            {
                double minimum = Math.Floor(range.Start / interval) * interval;
                double maximum = Math.Ceiling(range.End / interval) * interval;
                return new DoubleRange(minimum, maximum);
            }
#endif
            return range;
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void OnLabelCreated(ChartAxisLabel label)
        {

        }

        /// <summary>
        /// The RaiseCallBackActualRangeChanged method is used to update the ActualMinimum and ActualMaximum property value while changing the actual range of ChartAxis.
        /// </summary>
        internal virtual void RaiseCallBackActualRangeChanged()
        {

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


#if WinUI
            //// TODO: Need to revamp this method
            if (Area is SfCartesianChart)
            {
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
            }
            else
            {
                foreach (PolarSeries series in visibleSeries)
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
            }
#else
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
#endif
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

        internal virtual double CoefficientToValue(double coefficient)
        {
            double result;

            coefficient = IsInversed ? 1d - coefficient : coefficient;

            result = VisibleRange.Start + (VisibleRange.Delta * coefficient);

            return result;
        }

        internal virtual float ValueToCoefficient(double value)
        {
            double result;

            double start = VisibleRange.Start;
            double delta = VisibleRange.Delta;

            result = (value - start) / delta;

            return (float)(IsInversed ? 1f - result : result);
        }

        #endregion

        #region Internal Methods

        internal virtual DoubleRange AddDefaultRange(double start)
        {
            return new DoubleRange(start, start + 1);
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

        internal void CalculateRangeAndInterval(Size plotSize)
        {
            DoubleRange range = ValidateRange(CalculateActualRange());
            ActualInterval = CalculateActualInterval(range, plotSize);
            ActualRange = ApplyRangePadding(range, ActualInterval);

            var visibleRange = CalculateVisibleRange(ActualRange, plotSize);
            VisibleRange = visibleRange.IsEmpty ? ActualRange : visibleRange;

            var visibleInterval = CalculateVisibleInterval(VisibleRange, plotSize);
            VisibleInterval = double.IsNaN(visibleInterval) || visibleInterval == 0 ? ActualInterval : visibleInterval;

            RaiseCallBackActualRangeChanged();

            if (ActualRangeChanged != null)
            {
                RaiseActualRangeChangedEvent(visibleRange, plotSize);
            }

            if (!double.IsNaN(ActualAutoScrollDelta) && ActualAutoScrollDelta > 0 && CanAutoScroll)
            {
                UpdateAutoScrollingDelta(ActualRange, ActualAutoScrollDelta);
                CanAutoScroll = false;
            }

            UpdateAxisScale();
        }

        internal void UpdateSmallTickRequired(int value)
        {
            this.SmallTickRequired = value > 0;
        }

        internal void UpdateActualPlotOffsetStart(double offset)
        {
            this.ActualPlotOffsetStart = double.IsNaN(offset) ? 0 : offset;
        }

        internal void UpdateActualPlotOffsetEnd(double offset)
        {
            this.ActualPlotOffsetEnd = double.IsNaN(offset) ? 0 : offset;
        }

        //Made this calculation in a virtual method for implementing separate logic for logarithmic axis
        internal virtual void UpdateAxisScale()
        {
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
            if (double.IsNaN(scrollingDelta)) return;

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

            if (ChartDataUtils.EqualDoubleValues(range.Start, range.End))
            {
                range = AddDefaultRange(range.Start);
            }

            return range;
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

#if WinUI
            this.UpdateAxisElement();
#endif
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

        #endregion

        #endregion
    }
}
