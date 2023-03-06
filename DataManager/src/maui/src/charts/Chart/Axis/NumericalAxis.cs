﻿#if WinUI
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
#else
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
    public partial class NumericalAxis : RangeAxisBase
    {
        #region Properties
        internal double DefaultMinimum { get; set; } = double.NaN;

        internal double DefaultMaximum { get; set; } = double.NaN;
        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected override DoubleRange CalculateActualRange()
        {
            if (ActualRange.IsEmpty)
            {
                //Executes when Minimum and Maximum aren't set.
                return base.CalculateActualRange();
            }
            else if (!double.IsNaN(DefaultMinimum) && !double.IsNaN(DefaultMaximum))
            {
                //Executes when Minimum and Maximum are set.
                return new DoubleRange(DefaultMinimum, DefaultMaximum);
            }
            else
            {
                DoubleRange baseRange = base.CalculateActualRange();
                if (!double.IsNaN(DefaultMinimum))
                {
                    return new DoubleRange(DefaultMinimum, double.IsNaN(baseRange.End) ? ActualRange.Start + 1 : baseRange.End);
                }

                if (!double.IsNaN(DefaultMaximum))
                {
                    return new DoubleRange(double.IsNaN(baseRange.Start) ? ActualRange.End - 1 : baseRange.Start, DefaultMaximum);
                }

                return baseRange;
            }
        }

        /// <inheritdoc/>
		protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            if (!double.IsNaN(DefaultMinimum) && !double.IsNaN(DefaultMaximum))
            {
                return range;
            }

            if (double.IsNaN(DefaultMinimum) && double.IsNaN(DefaultMaximum))
            {
                return CalculateNumericalRangePadding(base.ApplyRangePadding(range, interval), interval);
            }
            else
            {
                DoubleRange baseRange = CalculateNumericalRangePadding(base.ApplyRangePadding(range, interval), interval);
                return !double.IsNaN(DefaultMinimum) ? new DoubleRange(range.Start, baseRange.End) : new DoubleRange(baseRange.Start, range.End);
            }
        }

        /// <inheritdoc/>
        protected override double CalculateActualInterval(DoubleRange range, Size availableSize)
        {
            if (double.IsNaN(AxisInterval) || AxisInterval == 0)
            {
                return CalculateNiceInterval(range, availableSize);
            }

            return AxisInterval;
        }

        /// <inheritdoc/>
        protected override DoubleRange CalculateVisibleRange(DoubleRange range, Size availableSize)
        {
            var visibleRange = base.CalculateVisibleRange(range, availableSize);

            if (ZoomFactor < 1 || ZoomPosition > 0)
            {
                if (!double.IsNaN(AxisInterval))
                {
                    double actualInterval = AxisInterval;
                    VisibleInterval = EnableAutoIntervalOnZooming
                            ? CalculateNiceInterval(visibleRange, availableSize)
                            : actualInterval;
                }
                else
                {
                    VisibleInterval = CalculateNiceInterval(visibleRange, availableSize);
                }
            }

            return visibleRange;
        }

        /// <summary>
        /// Update the ActualMinimum and ActualMaximum property value of NumericalAxis.
        /// </summary>
        internal override void RaiseCallBackActualRangeChanged()
        {
            if (!ActualRange.IsEmpty)
            {
                ActualMinimum = ActualRange.Start;
                ActualMaximum = ActualRange.End;
            }
        }

        #endregion

        #region Internal Methods
        internal override void GenerateVisibleLabels()
        {
            var actualLabels = VisibleLabels;

            if (VisibleRange.IsEmpty || actualLabels == null)
            {
                return;
            }

            SmallTickPoints.Clear();
            double interval = VisibleInterval, position;
            var edgeLabelVisibility = EdgeLabelsVisibilityMode;
            var isEdgeLabelAlwaysVisible = edgeLabelVisibility == EdgeLabelsVisibilityMode.AlwaysVisible;
            var isEdgeLabelVisible = edgeLabelVisibility == EdgeLabelsVisibilityMode.Visible && ZoomFactor.Equals(1.0f);

            position = GetStartPosition();

            while (position <= VisibleRange.End && !double.IsInfinity(position))
            {
                if (VisibleRange.Inside(position))
                {
                    var labelFormat = LabelStyle != null ? LabelStyle.LabelFormat : string.Empty;
                    var labelContent = GetActualLabelContent(position, labelFormat);
                    var axisLabel = new ChartAxisLabel(position, labelContent);
                    actualLabels.Add(axisLabel);

                    TickPositions.Add(position);
                }

                if (MinorTicksPerInterval > 0)
                {
                    AddSmallTicksPoint(position, interval);
                }

                position += interval;
            }

            var count = actualLabels.Count - 1;
            if (count < 0)
            {
                return;
            }

            double pos = Math.Round(actualLabels[count].Position, 6);
            double endValue = Math.Round(VisibleRange.End, 6);

            if ((actualLabels.Count > 0 && pos != endValue && isEdgeLabelAlwaysVisible)
                || (isEdgeLabelVisible && actualLabels[count].Position > VisibleRange.End))
            {
                var format = LabelStyle != null ? LabelStyle.LabelFormat : String.Empty;
                var labelContent = GetActualLabelContent(VisibleRange.End, format);
                var axisLabel =
                    new ChartAxisLabel(VisibleRange.End, labelContent);
                actualLabels.Add(axisLabel);

                if (labelContent.ToString() == axisLabel.Content.ToString())
                {
                    var labelStyle = axisLabel.LabelStyle;

                    if (labelStyle != null)
                    {
                        if (!string.IsNullOrEmpty(labelStyle.LabelFormat))
                        {
                            axisLabel.Content = GetActualLabelContent(VisibleRange.End, labelStyle.LabelFormat);
                        }
                    }
                }

                TickPositions.Add(VisibleRange.End);
            }
        }

        internal double GetStartPosition()
        {
            if ((!double.IsNaN(DefaultMinimum) && ZoomFactor.Equals(1.0f))
             || EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible ||
             (EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && ZoomFactor.Equals(1.0f)))
            {
                return VisibleRange.Start;
            }
            else
            {
                return VisibleRange.Start - (VisibleRange.Start % VisibleInterval);
            }
        }

        internal void OnMinMaxChanged()
        {
            if (!double.IsNaN(DefaultMinimum) || !double.IsNaN(DefaultMaximum))
            {
                double minimumValue = double.IsNaN(DefaultMinimum) ? double.MinValue : DefaultMinimum;
                double maximumValue = double.IsNaN(DefaultMaximum) ? double.MaxValue : DefaultMaximum;
                ActualRange = new DoubleRange(minimumValue, maximumValue);
            }
        }
        #endregion

        #region Private Methods
        private NumericalPadding ActualRangePadding()
        {
            var visibleSeries = CartesianArea?.VisibleSeries;
            bool isTransposed;
            //TODO: Change range based on series transpose.
            if (RangePadding == NumericalPadding.Auto && CartesianArea != null && visibleSeries != null && visibleSeries.Count > 0)
            {
#if WinUI
                isTransposed = CartesianArea is SfCartesianChart ? (CartesianArea as SfCartesianChart).IsTransposed : false;
#else
                isTransposed = CartesianArea.IsTransposed;
#endif
                if ((IsVertical && !isTransposed) || (!IsVertical && isTransposed))
                {
                    return NumericalPadding.Round;
                }
            }
            return RangePadding;
        }

        private DoubleRange CalculateNumericalRangePadding(DoubleRange range, double interval)
        {
            var actualRangePadding = ActualRangePadding();
            double startRange = range.Start;
            double endRange = range.End;

            if (actualRangePadding == NumericalPadding.Normal)
            {
                double minimum, remaining, start = startRange;

                if (startRange < 0)
                {
                    start = 0;
                    minimum = startRange + (startRange / 20);
                    remaining = interval + (minimum % interval);

                    if ((0.365 * interval) >= remaining)
                    {
                        minimum -= interval;
                    }

                    if (minimum % interval < 0)
                    {
                        minimum = (minimum - interval) - (minimum % interval);
                    }
                }
                else
                {
                    minimum = startRange < ((5.0 / 6.0) * endRange) ? 0 : (startRange - ((endRange - startRange) / 2));

                    if (minimum % interval > 0)
                    {
                        minimum -= minimum % interval;
                    }
                }

                double rangeDiff = endRange - start;
                double adjustment = rangeDiff / 20.0;
                double maximum = endRange + ((endRange > 0) ? adjustment : -adjustment);
                remaining = interval - (maximum % interval);

                if ((0.365 * interval) >= remaining)
                {
                    maximum += interval;
                }

                if (maximum % interval > 0)
                {
                    maximum = (maximum + interval) - (maximum % interval);
                }

                range = new DoubleRange(minimum, maximum);

                if (minimum == 0d)
                {
                    ActualInterval = CalculateActualInterval(range, AvailableSize);
                    return new DoubleRange(0, Math.Ceiling(maximum / ActualInterval) * ActualInterval);
                }
            }
            else if (actualRangePadding != NumericalPadding.Auto && actualRangePadding != NumericalPadding.None && actualRangePadding != NumericalPadding.Normal)
            {
                double minimum = Math.Floor(startRange / interval) * interval;
                double maximum = Math.Ceiling(endRange / interval) * interval;
                double additionalMinimum = minimum - interval;
                double additionalMaximum = maximum + interval;

                switch (actualRangePadding)
                {
                    case NumericalPadding.Round:
                        return new DoubleRange(minimum, maximum);
                    case NumericalPadding.RoundStart:
                        return new DoubleRange(minimum, endRange);
                    case NumericalPadding.RoundEnd:
                        return new DoubleRange(startRange, maximum);
                    case NumericalPadding.PrependInterval:
                        return new DoubleRange(additionalMinimum, endRange);
                    case NumericalPadding.AppendInterval:
                        return new DoubleRange(startRange, additionalMaximum);
                    default:
                        return new DoubleRange(additionalMinimum, additionalMaximum);
                }
            }

            return range;
        }

        /// <summary>
        /// Methods to update axis interval.
        /// </summary>
        /// <param name="interval">The axis interval.</param>
        private void UpdateAxisInterval(double interval)
        {
            this.AxisInterval = interval;
        }

        /// <summary>
        /// Methods to update the default minimum value.
        /// </summary>
        /// <param name="minimum">The minimum value.</param>
        private void UpdateDefaultMinimum(double? minimum)
        {
            this.DefaultMinimum = Convert.ToDouble(minimum ?? double.NaN);
        }

        /// <summary>
        /// Methods to update the default minimum value.
        /// </summary>
        /// <param name="maximum">The minimum value.</param>
        private void UpdateDefaultMaximum(double? maximum)
        {
            this.DefaultMaximum = Convert.ToDouble(maximum ?? double.NaN);
        }

        #endregion

        #endregion
    }
}