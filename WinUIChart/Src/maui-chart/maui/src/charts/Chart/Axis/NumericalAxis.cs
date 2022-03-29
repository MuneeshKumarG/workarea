using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NumericalAxis : RangeAxisBase
    {
        #region Properties
        internal double ActualMinimum { get; set; } = double.NaN;

        internal double ActualMaximum { get; set; } = double.NaN;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public NumericalAxis()
		{
        }
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
            else if (!double.IsNaN(ActualMinimum) && !double.IsNaN(ActualMaximum))
            {
                //Executes when Minimum and Maximum are set.
                return new DoubleRange(ActualMinimum, ActualMaximum);
            }
            else
            {
                DoubleRange baseRange = base.CalculateActualRange();
                if (!double.IsNaN(ActualMinimum))
                {
                    return new DoubleRange(ActualMinimum, double.IsNaN(baseRange.End) ? ActualRange.Start + 1 : baseRange.End);
                }

                if (!double.IsNaN(ActualMaximum))
                {
                    return new DoubleRange(double.IsNaN(baseRange.Start) ? ActualRange.End - 1 : baseRange.Start, ActualMaximum);
                }

                return baseRange;
            }
        }

        /// <inheritdoc/>
		protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
		{
			if (!double.IsNaN(ActualMinimum) && !double.IsNaN(ActualMaximum))
            {
                return range;
            }
			
			if (double.IsNaN(ActualMinimum) && double.IsNaN(ActualMaximum))
            {
                return CalculateNumericalRangePadding(base.ApplyRangePadding(range, interval), interval);
            }
			else
			{
				DoubleRange baseRange = CalculateNumericalRangePadding(base.ApplyRangePadding(range, interval), interval);
				return !double.IsNaN(ActualMinimum) ? new DoubleRange(range.Start, baseRange.End) : new DoubleRange(baseRange.Start, range.End);
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
                    var labelContent = GetActualLabelContent(position, LabelStyle.LabelFormat);
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
                var labelContent = GetActualLabelContent(VisibleRange.End, LabelStyle.LabelFormat);
                var axisLabel =
                    new ChartAxisLabel(VisibleRange.End, labelContent);
                actualLabels.Add(axisLabel);

                if (labelContent.ToString() == axisLabel.Content.Tostring())
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
            if ((!double.IsNaN(ActualMinimum) && ZoomFactor.Equals(1.0f))
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
            if (!double.IsNaN(ActualMinimum) || !double.IsNaN(ActualMaximum))
            {
                double minimumValue = double.IsNaN(ActualMinimum) ? double.MinValue : ActualMinimum;
                double maximumValue = double.IsNaN(ActualMaximum) ? double.MaxValue : ActualMaximum;
                ActualRange = new DoubleRange(minimumValue, maximumValue);
            }
        }
        #endregion

        #region Private Methods
        private NumericalPadding ActualRangePadding()
        {
            var visibleSeries = CartesianArea?.VisibleSeries;
            //TODO: Change range based on series transpose.
            if (RangePadding == NumericalPadding.Auto && CartesianArea != null && visibleSeries != null && visibleSeries.Count > 0)
            {
                if ((IsVertical && !CartesianArea.IsTransposed) ||
                        (!IsVertical && CartesianArea.IsTransposed))
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

                double maximum = endRange + ((endRange - start) / 20);
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
        #endregion

        #endregion
    }
}