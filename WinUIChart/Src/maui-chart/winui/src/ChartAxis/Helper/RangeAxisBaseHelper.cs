using System;

namespace Syncfusion.UI.Xaml.Charts
{
    internal static class RangeAxisBaseHelper
    {
        #region Methods

        #region Internal Static Methods

        /// <summary>
        /// Method implementation for Add smallTicks to axis
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="position">The position.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="smallTicksPerInterval">The small ticks per interval.</param>
        internal static void AddSmallTicksPoint(ChartAxis axis, double position, double interval, int smallTicksPerInterval)
        {
            int tickCount = smallTicksPerInterval + 1;
            double tickInterval = interval / tickCount;
            double endPosition = position + interval;
            double tickPosition = position;
            var end = axis.VisibleRange.End;
            var start = axis.VisibleRange.Start;
            var visibleRange = axis.VisibleRange;
            var linearAxis = axis as NumericalAxis;

            if (linearAxis != null && linearAxis.IsSecondaryAxis && linearAxis.BreakRangesInfo.Count > 0)
            {
                var ranges = linearAxis.AxisRanges;
                for (int i = 0; i < ranges.Count; i++)
                {
                    if (!ranges[i].Inside(position)) continue;
                    end = ranges[i].End;
                    visibleRange = ranges[i];
                    break;
                }
            }

            int currentTickIndex = 0; 

            if (axis.m_smalltickPoints.Count == 0 && tickPosition > start)
            {
                double tickStartPosition = position;

                while (tickStartPosition > start && tickStartPosition < end && currentTickIndex < tickCount)
                {
                    if (!(tickStartPosition == position))
                    {
                        axis.m_smalltickPoints.Add(tickStartPosition);
                    }

                    currentTickIndex++;

                    tickStartPosition -= tickInterval;
                }
            }

            currentTickIndex = 0;
            while (tickPosition < endPosition && tickPosition < end && currentTickIndex < tickCount)
            {
                if (!(tickPosition == position) && visibleRange.Inside(tickPosition))
                {
                    axis.m_smalltickPoints.Add(tickPosition);
                }

                currentTickIndex++;

                tickPosition += tickInterval;

                double roundTickposition = Math.Round(tickPosition * 100000000) / 100000000.0;

                if (roundTickposition >= endPosition)
                {
                    tickPosition = endPosition;
                }
            }
        }

        /// <summary>
        /// Method implementation for Generate Labels in ChartAxis
        /// </summary>
        /// <param name="axis">The axis.</param>
        /// <param name="smallTicksPerInterval">The small ticks per interval.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1801: Review unused parameters")]
        internal static void GenerateVisibleLabels(ChartAxis axis, double smallTicksPerInterval)
        {
            double interval = axis.VisibleInterval;
            double position = axis.VisibleRange.Start - (axis.VisibleRange.Start % interval);

            for (; position <= axis.VisibleRange.End; position += interval)
            {
                if (axis.VisibleRange.Inside(position))
                {
                    axis.VisibleLabels.Add(new ChartAxisLabel(position, axis.GetActualLabelContent(position), position));
                }

                if (axis.smallTicksRequired)
                {
                    axis.AddSmallTicksPoint(position);
                }
            }
        }

        #endregion

        #endregion

    }
}
