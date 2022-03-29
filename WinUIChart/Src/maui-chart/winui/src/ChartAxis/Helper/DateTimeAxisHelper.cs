﻿using System;
using System.Globalization;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    internal static class DateTimeAxisHelper
    {
        #region Methods

        #region Internal Static Methods

        /// <summary>
        /// Generates the visible labels.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="intervalType">Type of the interval.</param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// Method implementation for Create VisibleLabels for DateTime axis
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        internal static void GenerateVisibleLabels(ChartAxisBase2D axis, object minimum, object maximum, DateTimeIntervalType intervalType)
        {
            var startDate = axis.VisibleRange.Start.FromOADate();
            DateTime alignedDate;
            var interval = IncreaseInterval(startDate, axis.VisibleInterval, intervalType).ToOADate() - axis.VisibleRange.Start;

            if (axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible
               || (axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && !axis.IsZoomed))
                alignedDate = startDate;
            else
                alignedDate = AlignRangeStart(startDate, axis.VisibleInterval, intervalType);

            var dateTimeAxis = axis as DateTimeAxis;
            var days = "";
            double distinctDate = 0d;
            double position = alignedDate.ToOADate();

            if (dateTimeAxis != null && dateTimeAxis.EnableBusinessHours)
            {
                days = dateTimeAxis.InternalWorkingDays;
                alignedDate = new DateTime(startDate.Year, startDate.Month, startDate.Day).AddHours(dateTimeAxis.OpenTime);
                position = alignedDate.ToOADate();
            }

            double previousPosition = double.NaN;

            if (axis.IsDefaultRange && dateTimeAxis.Interval != 0)
            {
                if (intervalType == DateTimeIntervalType.Minutes)
                    axis.VisibleRange = new DoubleRange(0, 0.041);
                else if (intervalType == DateTimeIntervalType.Seconds)
                    axis.VisibleRange = new DoubleRange(0, 0.041 / 60);
                else if (intervalType == DateTimeIntervalType.Milliseconds)
                    axis.VisibleRange = new DoubleRange(0, 0.041 / 3600);
            }

            while (position <= axis.VisibleRange.End)
            {
                if (position == previousPosition)
                    break;

                if (axis.VisibleRange.Inside(position))
                {
                    var dateTimeAxisLabel = new DateTimeAxisLabel(position, alignedDate.ToString(axis.LabelFormat, CultureInfo.CurrentCulture), position);
                    dateTimeAxisLabel.IntervalType = dateTimeAxis.ActualIntervalType;
                    dateTimeAxisLabel.IsTransition = GetTransitionState(ref distinctDate, alignedDate, dateTimeAxis.ActualIntervalType);
                    axis.VisibleLabels.Add(dateTimeAxisLabel);
                }

                alignedDate = IncreaseNonWorkingInterval(days, dateTimeAxis, alignedDate, axis.VisibleInterval, intervalType);
                if (dateTimeAxis != null && dateTimeAxis.EnableBusinessHours)
                    alignedDate = NonWorkingDaysIntervals(dateTimeAxis, alignedDate);

                if (axis.smallTicksRequired)
                {
                    axis.AddSmallTicksPoint(position, interval);
                }

                previousPosition = position;
                position = alignedDate.ToOADate();
            }

            double rangeEnd = axis.VisibleRange.End;
            if (((maximum != null && rangeEnd.FromOADate().Equals(maximum))
                || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible
                || (axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && !axis.IsZoomed))
                && !rangeEnd.Equals(position - interval))
            {
                alignedDate = rangeEnd.FromOADate();
                var dateTimeAxisLabel = new DateTimeAxisLabel(
                    rangeEnd, 
                    alignedDate.ToString(
                        axis.LabelFormat, 
                        CultureInfo.CurrentCulture),
                        rangeEnd);
                dateTimeAxisLabel.IntervalType = dateTimeAxis.ActualIntervalType;
                dateTimeAxisLabel.IsTransition = GetTransitionState(ref distinctDate, alignedDate, dateTimeAxis.ActualIntervalType);
                axis.VisibleLabels.Add(dateTimeAxisLabel);
            }
        }

        /// <summary>
        /// Method to depict the distinct data in dateTime axis transition.
        /// </summary>
        /// <param name="distinctDate"></param>
        /// <param name="currentDate"></param>
        /// <param name="intervalType"></param>
        /// <returns></returns>
        internal static bool GetTransitionState(ref double distinctDate, DateTime currentDate, DateTimeIntervalType intervalType)
        {
            switch (intervalType)
            {
                case DateTimeIntervalType.Months:
                    if (distinctDate != currentDate.Year)
                    {
                        distinctDate = currentDate.Year;
                        return true;
                    }

                    break;

                case DateTimeIntervalType.Days:
                    if (distinctDate != currentDate.Month)
                    {
                        distinctDate = currentDate.Month;
                        return true;
                    }

                    break;

                case DateTimeIntervalType.Hours:
                    if (distinctDate != currentDate.Day)
                    {
                        distinctDate = currentDate.Day;
                        return true;
                    }

                    break;

                case DateTimeIntervalType.Minutes:
                    if (distinctDate != currentDate.Hour)
                    {
                        distinctDate = currentDate.Hour;
                        return true;
                    }

                    break;

                case DateTimeIntervalType.Seconds:
                    if (distinctDate != currentDate.Minute)
                    {
                        distinctDate = currentDate.Minute;
                        return true;
                    }

                    break;

                case DateTimeIntervalType.Milliseconds:
                    if (distinctDate != currentDate.Second)
                    {
                        distinctDate = currentDate.Second;
                        return true;
                    }

                    break;
            }

            return false;
        }

        internal static DateTime IncreaseInterval(DateTime date, double interval, DateTimeIntervalType intervalType)
        {
            TimeSpan span = new TimeSpan(0);
            DateTime result;

            if (intervalType == DateTimeIntervalType.Days)
            {
                span = TimeSpan.FromDays(interval);
            }
            else if (intervalType == DateTimeIntervalType.Hours)
            {
                span = TimeSpan.FromHours(interval);
            }
            else if (intervalType == DateTimeIntervalType.Milliseconds)
            {
                span = TimeSpan.FromMilliseconds(interval);
            }
            else if (intervalType == DateTimeIntervalType.Seconds)
            {
                span = TimeSpan.FromSeconds(interval);
            }
            else if (intervalType == DateTimeIntervalType.Minutes)
            {
                span = TimeSpan.FromMinutes(interval);
            }
            else if (intervalType == DateTimeIntervalType.Months)
            {
                date = date.AddMonths((int)Math.Floor(interval));
                span = TimeSpan.FromDays(30.0 * (interval - Math.Floor(interval)));
            }
            else if (intervalType == DateTimeIntervalType.Years)
            {
                date = date.AddYears((int)Math.Floor(interval));
                span = TimeSpan.FromDays(365.0 * (interval - Math.Floor(interval)));
            }

            result = date.Add(span);

            return result;
        }

        /// <summary>
        /// Calculates the visible range.
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="availableSize">Size of the available.</param>
        /// <param name="interval">The interval.</param>
        internal static void CalculateVisibleRange(ChartAxisBase2D axis, Size availableSize, double interval)
        {
            if (axis.ZoomFactor < 1 || axis.ZoomPosition > 0)
            {
                if (interval != 0 || !double.IsNaN(interval))
                {
                    axis.VisibleInterval = axis.EnableAutoIntervalOnZooming
                                            ? axis.CalculateNiceInterval(axis.VisibleRange, availableSize)
                                            : axis.ActualInterval;
                }
                else
                {
                    axis.VisibleInterval = axis.CalculateNiceInterval(axis.VisibleRange, availableSize);
                }
            }
            else
            {
                var dateTimeAxis = axis as DateTimeAxis;
                if (dateTimeAxis.IntervalType != DateTimeIntervalType.Auto)
                    dateTimeAxis.ActualIntervalType = dateTimeAxis.IntervalType;
                if (interval != 0 || !double.IsNaN(interval))
                    axis.VisibleInterval = axis.ActualInterval;
                else
                    axis.VisibleInterval = axis.CalculateNiceInterval(axis.VisibleRange, availableSize);
            }
        }

        /// <summary>
        /// Apply padding based on interval
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="range"></param>
        /// <param name="interval"></param>
        /// <param name="rangePadding"></param>
        /// <param name="intervalType"></param>
        /// <returns></returns>
        internal static DoubleRange ApplyRangePadding(ChartAxis axis, DoubleRange range, double interval, DateTimeRangePadding rangePadding, DateTimeIntervalType intervalType)
        {
            DateTime roundStartDate, additionalStartDate, startDate = range.Start.FromOADate();
            DateTime roundEndDate, additionalEndDate, endDate = range.End.FromOADate();
            var dateTimeAxis = axis as DateTimeAxis;
            if (dateTimeAxis != null && dateTimeAxis.EnableBusinessHours)
                return ApplyBusinessHoursRangePadding(dateTimeAxis, range, interval, rangePadding, intervalType);
            if (rangePadding != DateTimeRangePadding.Auto && rangePadding != DateTimeRangePadding.None)
            {
                switch (intervalType)
                {
                    case DateTimeIntervalType.Years:

                        int startYear = (int)((int)(startDate.Year / interval) * interval);
                        int endYear = endDate.Year + (startDate.Year - startYear);

                        if (startYear <= 0)
                        {
                            startYear = 1;
                        }

                        if (endYear <= 0)
                        {
                            endYear = 1;
                        }

                        roundStartDate = new DateTime(startYear, 1, 1, 0, 0, 0);
                        roundEndDate = new DateTime(endYear, 12, 31, 23, 59, 59);
                        additionalStartDate = new DateTime(startYear - (int)interval, 1, 1, 0, 0, 0);
                        additionalEndDate = new DateTime(endYear + (int)interval, 12, 31, 23, 59, 59);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Months:

                        int month = (int)((int)(startDate.Month / interval) * interval);
                        if (month <= 0)
                        {
                            month = 1;
                        }

                        int endMonth = range.End.FromOADate().Month + (startDate.Month - month);
                        if (endMonth <= 0)
                        {
                            endMonth = 1;
                        }

                        if (endMonth > 12)
                        {
                            endMonth = 12;
                        }

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            month, 
                            1,
                            0, 
                            0, 
                            0);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endMonth, 
                            endMonth == 2 ? 28 : 30, 
                            0,
                            0, 
                            0);
                        additionalStartDate = new DateTime(
                            startDate.Year, 
                            month, 
                            1,
                            0, 
                            0, 
                            0).AddMonths((int)(-interval));
                        additionalEndDate = new DateTime(
                            endDate.Year, 
                            endMonth, 
                            endMonth == 2 ? 28 : 30, 
                            0, 
                            0,
                            0).AddMonths((int)interval);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Days:

                        int day = (int)((int)(startDate.Day / interval) * interval);
                        if (day <= 0)
                        {
                            day = 1;
                        }

                        int endday = startDate.Day - day;
                        if (endday <= 0)
                        {
                            endday = 1;
                        }

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            day, 
                            0, 
                            0,
                            0);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            23, 
                            59, 
                            59);
                        additionalStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            day, 
                            0, 
                            0,
                            0).AddDays((int)-interval);
                        additionalEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            23, 
                            59,
                            59).AddDays((int)interval);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Hours:
                        int hour = (int)((int)(startDate.Hour / interval) * interval);
                        int endhour = endDate.Hour + (startDate.Hour - hour);

                        if (endhour > 23)
                        {
                            endhour = 23;
                        }

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            hour, 
                            0, 
                            0);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endhour, 
                            59,
                            59);
                        additionalStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            0,
                            0).AddHours((int)-interval);
                        additionalEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            59, 
                            59).AddHours((int)interval);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Minutes:
                        int minute = (int)((int)(startDate.Minute / interval) * interval);
                        int endminute = range.End.FromOADate().Minute + (startDate.Minute - minute);

                        if (endminute > 59)
                        {
                            endminute = 59;
                        }

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            minute, 
                            0);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            endminute, 
                            59);
                        additionalStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            startDate.Minute, 
                            0).AddMinutes(-interval);
                        additionalEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            endDate.Minute, 
                            59).AddMinutes(interval);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Seconds:
                        int second = (int)((int)(startDate.Second / interval) * interval);
                        int endsecond = range.End.FromOADate().Second + (startDate.Second - second);

                        if (endsecond > 59)
                        {
                            endsecond = 59;
                        }

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            startDate.Minute, 
                            second, 
                            0);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            endDate.Minute, 
                            endsecond, 
                            999);
                        additionalStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            startDate.Minute, 
                            startDate.Second, 
                            0).AddSeconds(-interval);
                        additionalEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            endDate.Minute, 
                            endDate.Second, 
                            999).AddSeconds(interval);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Milliseconds:
                        int milliseconds = (int)((int)(startDate.Millisecond / interval) * interval);
                        int endmilliseconds = range.End.FromOADate().Millisecond + (startDate.Millisecond - milliseconds);

                        if (endmilliseconds > 999)
                        {
                            endmilliseconds = 999;
                        }

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            startDate.Minute, 
                            startDate.Second, 
                            milliseconds);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            endDate.Minute, 
                            endDate.Second, 
                            endmilliseconds);
                        additionalStartDate = startDate.AddMilliseconds(-interval);
                        additionalEndDate = endDate.AddMilliseconds(interval);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);
                }

                return range;
            }

            return range;
        }

        private static DoubleRange ApplyRangePadding(DateTimeRangePadding rangePadding, DateTime additionalStartDate, DateTime additionalEndDate, DateTime roundStartDate, DateTime roundEndDate, DateTime startDate, DateTime endDate)
        {
            switch (rangePadding)
            {
                case DateTimeRangePadding.Round:
                    return new DoubleRange(roundStartDate.ToOADate(), roundEndDate.ToOADate());
                case DateTimeRangePadding.RoundStart:
                    return new DoubleRange(roundStartDate.ToOADate(), endDate.ToOADate());
                case DateTimeRangePadding.RoundEnd:
                    return new DoubleRange(startDate.ToOADate(), roundEndDate.ToOADate());
                case DateTimeRangePadding.PrependInterval:
                    return new DoubleRange(additionalStartDate.ToOADate(), endDate.ToOADate());
                case DateTimeRangePadding.AppendInterval:
                    return new DoubleRange(startDate.ToOADate(), additionalEndDate.ToOADate());
                case DateTimeRangePadding.Additional:
                    return new DoubleRange(additionalStartDate.ToOADate(), additionalEndDate.ToOADate());
                default:
                    return new DoubleRange(startDate.ToOADate(), endDate.ToOADate());
            }
        }

        internal static DoubleRange ApplyBusinessHoursRangePadding(DateTimeAxis axis, DoubleRange range, double interval, DateTimeRangePadding rangePadding, DateTimeIntervalType intervalType)
        {
            DateTime roundStartDate, additionalStartDate, startDate = range.Start.FromOADate();
            DateTime roundEndDate, additionalEndDate, endDate = range.End.FromOADate();
            var startTime = new TimeSpan((int)axis.OpenTime, 0, 0);
            var endTime = new TimeSpan((int)axis.CloseTime, 0, 0);

            if (rangePadding != DateTimeRangePadding.Auto && rangePadding != DateTimeRangePadding.None)
            {
                switch (intervalType)
                {
                    case DateTimeIntervalType.Years:

                        int startYear = (int)((int)(startDate.Year / interval) * interval);
                        int endYear = endDate.Year + (startDate.Year - startYear);

                        if (startYear <= 0)
                        {
                            startYear = 1;
                        }

                        if (endYear <= 0)
                        {
                            endYear = 1;
                        }

                        roundStartDate = new DateTime(startYear, 1, 1, 0, 0, 0);
                        roundEndDate = new DateTime(endYear, 12, 31, 23, 59, 59);
                        additionalStartDate = new DateTime(startYear - (int)interval, 1, 1, 0, 0, 0);
                        additionalEndDate = new DateTime(endYear + (int)interval, 12, 31, 23, 59, 59);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Months:
                        int month = (int)((int)(startDate.Month / interval) * interval);
                        if (month <= 0)
                        {
                            month = 1;
                        }

                        int endMonth = range.End.FromOADate().Month + (startDate.Month - month);
                        if (endMonth <= 0)
                        {
                            endMonth = 1;
                        }

                        if (endMonth > 12)
                        {
                            endMonth = 12;
                        }

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            month, 
                            1,
                            0,
                            0,
                            0);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endMonth,
                            endMonth == 2 ? 28 : 30, 
                            0,
                            0,
                            0);
                        additionalStartDate = new DateTime(
                            startDate.Year, 
                            month, 
                            1, 
                            0,
                            0,
                            0).AddMonths((int)(-interval));
                        additionalEndDate = new DateTime(
                            endDate.Year, 
                            endMonth, 
                            endMonth == 2 ? 28 : 30, 
                            0, 
                            0,
                            0).AddMonths((int)interval);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Days:
                        int day = (int)((int)(startDate.Day / interval) * interval);
                        if (day <= 0)
                        {
                            day = 1;
                        }

                        int endday = startDate.Day - day;
                        if (endday < 0)
                        {
                            endday = 1;
                        }

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            day, 
                            startTime.Hours, 
                            startTime.Minutes, 
                            startTime.Seconds);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endTime.Hours, 
                            endTime.Minutes, 
                            endTime.Seconds).AddDays(endday);
                        additionalStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month,
                            day, 
                            startTime.Hours, 
                            startTime.Minutes, 
                            startTime.Seconds).AddDays(-interval);
                        additionalEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endTime.Hours, 
                            endTime.Minutes, 
                            endTime.Seconds).AddDays(interval + endday);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Hours:

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startTime.Hours, 
                            0, 
                            0);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endTime.Hours, 
                            0,
                            0);
                        additionalStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            0,
                            0).AddHours(-interval);
                        additionalEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            0,
                            0).AddHours(interval);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Minutes:
                        int minute = (int)((int)(startDate.Minute / interval) * interval);
                        int endminute = range.End.FromOADate().Minute + (startDate.Minute - minute);

                        if (endminute > 59)
                        {
                            endminute = 59;
                        }

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            minute, 
                            0);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            endminute, 
                            59);
                        additionalStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            startDate.Minute, 
                            0).AddMinutes(-interval);
                        additionalEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            endDate.Minute, 
                            59).AddMinutes(interval);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Seconds:
                        int second = (int)((int)(startDate.Second / interval) * interval);
                        int endsecond = range.End.FromOADate().Second + (startDate.Second - second);

                        if (endsecond > 59)
                        {
                            endsecond = 59;
                        }

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            startDate.Minute, 
                            second, 
                            0);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            endDate.Minute, 
                            endsecond, 
                            999);
                        additionalStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            startDate.Minute, 
                            startDate.Second, 
                            0).AddSeconds(-interval);
                        additionalEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            endDate.Minute, 
                            endDate.Second, 
                            999).AddSeconds(interval);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);

                    case DateTimeIntervalType.Milliseconds:
                        int milliseconds = (int)((int)(startDate.Millisecond / interval) * interval);
                        int endmilliseconds = range.End.FromOADate().Millisecond + (startDate.Millisecond - milliseconds);

                        if (endmilliseconds > 999)
                        {
                            endmilliseconds = 999;
                        }

                        roundStartDate = new DateTime(
                            startDate.Year, 
                            startDate.Month, 
                            startDate.Day, 
                            startDate.Hour, 
                            startDate.Minute, 
                            startDate.Second, 
                            milliseconds);
                        roundEndDate = new DateTime(
                            endDate.Year, 
                            endDate.Month, 
                            endDate.Day, 
                            endDate.Hour, 
                            endDate.Minute, 
                            endDate.Second, 
                            endmilliseconds);
                        additionalStartDate = startDate.AddMilliseconds(-interval);
                        additionalEndDate = endDate.AddMilliseconds(interval);

                        return ApplyRangePadding(rangePadding, additionalStartDate, additionalEndDate, roundStartDate, roundEndDate, startDate, endDate);
                }

                var newStartDate = range.Start.FromOADate();
                var newEndDate = range.End.FromOADate();

                newStartDate = newStartDate.ValidateNonWorkingDate(axis.InternalWorkingDays, true, axis.NonWorkingDays.Count);
                newStartDate = newStartDate.ValidateNonWorkingHours(axis.OpenTime, axis.CloseTime, true);
                newStartDate = newStartDate.ValidateNonWorkingDate(axis.InternalWorkingDays, true, axis.NonWorkingDays.Count);

                newEndDate = newEndDate.ValidateNonWorkingDate(axis.InternalWorkingDays, false, axis.NonWorkingDays.Count);
                newEndDate = newEndDate.ValidateNonWorkingHours(axis.OpenTime, axis.CloseTime, false);
                newEndDate = newEndDate.ValidateNonWorkingDate(axis.InternalWorkingDays, false, axis.NonWorkingDays.Count);

                return new DoubleRange(newStartDate.ToOADate(), newEndDate.ToOADate());
            }

            return range;
        }

        internal static void OnMinMaxChanged(ChartAxis axis, object minimum, object maximum)
        {
            if (minimum != null || maximum != null)
            {
                double minimumValue = minimum == null ? DateTime.MinValue.ToOADate() : Convert.ToDateTime(minimum).ToOADate();
                double maximumValue = maximum == null ? DateTime.MaxValue.ToOADate() : Convert.ToDateTime(maximum).ToOADate();
                axis.ActualRange = new DoubleRange(minimumValue, maximumValue);
            }

            if (axis.Area != null)
                axis.Area.ScheduleUpdate();
        }

        #endregion

        #region Internal Private Methods


        private static DateTime AlignRangeStart(DateTime start, double intervalValue, DateTimeIntervalType type)
        {
            if (type == DateTimeIntervalType.Auto)
            {
                return start;
            }

            DateTime StartDate = start;

            switch (type)
            {
                case DateTimeIntervalType.Years:
                    int year = (int)((int)(StartDate.Year / intervalValue) * intervalValue);
                    if (year <= 0)
                    {
                        year = 1;
                    }

                    StartDate = new DateTime(year, 1, 1, 0, 0, 0);
                    break;

                case DateTimeIntervalType.Months:
                    int month = (int)((int)(StartDate.Month / intervalValue) * intervalValue);
                    if (month <= 0)
                    {
                        month = 1;
                    }

                    StartDate = new DateTime(StartDate.Year, month, 1, 0, 0, 0);
                    break;

                case DateTimeIntervalType.Days:
                    int day = (int)((int)(StartDate.Day / intervalValue) * intervalValue);
                    if (day <= 0)
                    {
                        day = 1;
                    }

                    StartDate = new DateTime(StartDate.Year, StartDate.Month, day, 0, 0, 0);
                    break;

                case DateTimeIntervalType.Hours:
                    int hour = (int)((int)(StartDate.Hour / intervalValue) * intervalValue);
                    StartDate = new DateTime(
                        StartDate.Year,
                        StartDate.Month,
                        StartDate.Day,
                        hour,
                        0,
                        0);
                    break;

                case DateTimeIntervalType.Minutes:
                    int minute = (int)((int)(StartDate.Minute / intervalValue) * intervalValue);
                    StartDate = new DateTime(
                        StartDate.Year,
                        StartDate.Month,
                        StartDate.Day,
                        StartDate.Hour,
                        minute,
                        0);
                    break;

                case DateTimeIntervalType.Seconds:
                    int second = (int)((int)(StartDate.Second / intervalValue) * intervalValue);
                    StartDate = new DateTime(
                        StartDate.Year,
                        StartDate.Month,
                        StartDate.Day,
                        StartDate.Hour,
                        StartDate.Minute,
                        second,
                        0);
                    break;

                case DateTimeIntervalType.Milliseconds:
                    int milliseconds = (int)((int)(StartDate.Millisecond / intervalValue) * intervalValue);
                    StartDate = new DateTime(
                        StartDate.Year,
                        StartDate.Month,
                        StartDate.Day,
                        StartDate.Hour,
                        StartDate.Minute,
                        StartDate.Second,
                        milliseconds);
                    break;
            }

            return StartDate;
        }

        private static DateTime IncreaseNonWorkingInterval(string days, DateTimeAxis axis, DateTime date, double interval, DateTimeIntervalType intervalType)
        {
            var result = IncreaseInterval(date, interval, intervalType);
            if (axis != null && axis.EnableBusinessHours && !days.Contains(result.DayOfWeek.ToString()))
                result = result.ValidateNonWorkingDate(days, false, axis.NonWorkingDays.Count);
            return result;
        }
        
        private static DateTime NonWorkingDaysIntervals(DateTimeAxis axis, DateTime date)
        {
            string days = axis.InternalWorkingDays;

            if (axis != null)
            {
                date = date.ValidateNonWorkingDate(days, false, axis.NonWorkingDays.Count);

                date = date.ValidateNonWorkingHours(axis.OpenTime, axis.CloseTime, false);

                date = date.ValidateNonWorkingDate(days, false, axis.NonWorkingDays.Count);
            }

            return date;
        }

        #endregion

        #endregion
    }
}
