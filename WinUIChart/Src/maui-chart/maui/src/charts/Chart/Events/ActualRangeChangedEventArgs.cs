using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class ActualRangeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public object ActualMaximum { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public object ActualMinimum { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        internal object? VisibleMaximum { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal object? VisibleMinimum { get; set; }

        internal ChartAxis? Axis { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ActualRangeChangedEventArgs(object actualMinimum, object actualMaximum)
        {
            ActualMinimum = actualMinimum;
            ActualMaximum = actualMaximum;
        }

        internal DoubleRange GetActualRange()
        {
            return new DoubleRange(ToDouble(ActualMinimum), ToDouble(ActualMaximum));
        }

        private double ToDouble(object? val)
        {
            if (val == null) { return double.NaN; }

            if (Axis is DateTimeAxis && val is DateTime)
            {
                DateTime date = (DateTime)val;
                return date.Ticks;
            }
            else
            {
                string? value = val.ToString();
                return string.IsNullOrEmpty(value) ? double.NaN : double.Parse(value);
            }
        }

        internal DoubleRange GetVisibleRange()
        {
            if (VisibleMinimum == null && VisibleMaximum == null)
            {
                return DoubleRange.Empty;
            }

            double start, end;

            if (VisibleMaximum == null)
            {
                start = ToDouble(VisibleMinimum);
                end = ToDouble(ActualMaximum);
            }
            else if (VisibleMinimum == null)
            {
                start = ToDouble(ActualMinimum);
                end = ToDouble(VisibleMaximum);
            }
            else
            {
                start = ToDouble(VisibleMinimum);
                end = ToDouble(VisibleMaximum);
            }

            DoubleRange actualRange = GetActualRange();

            if (start < actualRange.Start)
            {
                start = actualRange.Start;
            }

            if (end > actualRange.End)
            {
                end = actualRange.End;
            }

            if (start == end)
            {
                end = end + 1;
            }

            return new DoubleRange(start, end);
        }
    }

}
