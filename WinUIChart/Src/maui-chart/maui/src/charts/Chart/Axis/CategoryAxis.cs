
using Microsoft.Maui.Graphics;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CategoryAxis : ChartAxis
    {
        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public CategoryAxis()
        {
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected override double CalculateActualInterval(DoubleRange range, Size availableSize)
        {
            if (double.IsNaN(AxisInterval) || AxisInterval <= 0)
            {
                return Math.Max(1d, Math.Floor(range.Delta / GetActualDesiredIntervalsCount(availableSize)));
            }

            return AxisInterval;
        }

        /// <inheritdoc/>
        protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
        {
            return LabelPlacement == LabelPlacement.BetweenTicks ? new DoubleRange(-0.5, (int)range.End + 0.5) : range;
        }

        #endregion

        #region Internal Methods
        internal override void GenerateVisibleLabels()
        {
            if (VisibleRange.IsEmpty)
            {
                return;
            }

            var actualLabels = VisibleLabels;
            DoubleRange visibleRange = VisibleRange;
            double actualInterval = ActualInterval;
            double interval = VisibleInterval;
            double position = visibleRange.Start - (visibleRange.Start % actualInterval);
            var actualSeries = GetActualSeries();
            int maxDataCount = actualSeries != null ? actualSeries.PointsCount : 0;
            var roundInterval = Math.Ceiling(interval);

            for (; position <= visibleRange.End; position += roundInterval)
            {
                int pos = (int)position;
                if (visibleRange.Inside(pos) && pos < maxDataCount && pos > -1)
                {
                    var format = LabelStyle != null ? LabelStyle.LabelFormat : string.Empty;
                    var content = GetLabelContent(actualSeries, pos, format);

                    var axisLabel = new ChartAxisLabel(pos, content);
                    actualLabels?.Add(axisLabel);

                    if (LabelPlacement != LabelPlacement.BetweenTicks)
                    {
                        TickPositions.Add((double)pos);
                    }
                }
            }

            if (LabelPlacement == LabelPlacement.BetweenTicks)
            {
                double pos = 0;
                position = visibleRange.Start - (visibleRange.Start % actualInterval);

                for (; position <= visibleRange.End; position += 1)
                {
                    pos = ((int)Math.Round(position)) - 0.5;
                    if (visibleRange.Inside(pos) && pos < maxDataCount && pos > -1)
                    {
                        TickPositions.Add(pos);
                    }
                }

                pos += 1;
                if (visibleRange.Inside(pos) && pos < maxDataCount && pos > -1)
                {
                    TickPositions.Add(pos);
                }
            }
        }

        internal object? GetLabelContent(ChartSeries? chartSeries, int pos, string labelFormat)
        {
            var labelContent = string.Empty;

            if (chartSeries != null)
            {
                var values = chartSeries.XValues as IList;
                object label = string.Empty;

                if (values != null && values.Count > pos && pos >= 0)
                {
                    var xValue = values[pos];
                    if (xValue != null)
                    {
                        if (chartSeries.XValueType == ChartValueType.String)
                        {
                            label = xValue;
                        }
                        else if (chartSeries.XValueType == ChartValueType.DateTime)
                        {
                            if (string.IsNullOrEmpty(labelFormat))
                            {
                                labelFormat = "dd/MM/yyyy";
                            }

                            xValue = Convert.ToDouble(xValue);
                            label = GetFormattedAxisLabel(labelFormat, xValue);
                        }
                        else if (chartSeries.XValueType == ChartValueType.Double)
                        {
                            xValue = Convert.ToDouble(xValue);
                            label = GetActualLabelContent(xValue, labelFormat);
                        }
                    }
                }

                return label;
            }

            return labelContent;
        }

        #endregion

        #region Private Methods

        //Todo: Remove this method while implementing ArrangeByIndex feature.
        private ChartSeries? GetActualSeries()
        {
            var visibleSeries = Area?.VisibleSeries;
            if (visibleSeries == null) return null;

            int dataCount = 0;
            ChartSeries? selectedSeries = null;

            foreach (CartesianSeries series in visibleSeries)
            {
                if (series != null && series.ActualXAxis == this && series.PointsCount > dataCount)
                {
                    selectedSeries = series;
                    dataCount = series.PointsCount;
                }
            }

            return selectedSeries;
        }

        #endregion

        #endregion
    }
}
