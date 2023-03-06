#if WinUI
using Windows.Foundation;
#else
using Microsoft.Maui.Graphics;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
    public partial class CategoryAxis : ChartAxis
    {
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
        internal void GroupData()
        {
            List<string> groupingValues = new List<string>();
            List<object> groupedDatas = new List<object>();

            foreach (CartesianSeries series in RegisteredSeries)
            {
#if WinUI
                //TODO : Need to remove ActualXValues, after DataManager integration. 

                if (series.ActualXValues is List<string> xValues)
                {
                    GetGroupedValues(groupedDatas, groupingValues, xValues);
                }
                else if (series.ActualXValues != null)
                {
                    groupingValues.AddRange(from val in (series.ActualXValues as List<double>) select val.ToString());
                }
#else
                if (series.XStringList != null)
                {
                    GetGroupedValues(groupedDatas, groupingValues, series.XStringList);
                }
                else if (series.XDoubleList != null)
                {
                    var xValues = from val in (series.XDoubleList) select val.ToString();
                    groupingValues.AddRange(xValues);
                }
#endif
                if (groupingValues.Count != groupedDatas.Count)
                {
                    groupedDatas.AddRange(series.ActualData ?? groupedDatas);
                }
            }
            
            var distinctXValues = groupingValues.Distinct().ToList();

            foreach (CartesianSeries series in RegisteredSeries)
            {
#if WinUI
                if (series.ActualXValues is List<string> list)
                {
                    series.GroupedXValuesIndexes = (from val in list select (double)distinctXValues.IndexOf(val)).ToList();
                }
                else if (series.ActualXValues != null)
                {
                    series.GroupedXValuesIndexes = (from val in series.ActualXValues as List<double> select (double)distinctXValues.IndexOf(val.ToString())).ToList();
                }
#else
                if (series.XStringList != null)
                {
                    series.GroupedXValuesIndexes = (from val in series.XStringList select (double)distinctXValues.IndexOf(val)).ToList();
                }
                else if (series.XDoubleList != null)
                {
                    series.GroupedXValuesIndexes = (from val in (series.XDoubleList) select (double)distinctXValues.IndexOf(val.ToString())).ToList();
                }
#endif
                series.GroupedXValues = distinctXValues;

                //TODO: Remove group values for dynamic update. 
                
                series.GroupedActualData = groupedDatas;
            }
        }
        
        internal void GetGroupedValues(List<object> groupedDatas, List<string> groupingValues, List<string> xValues)
        {
            if (groupedDatas.Count != 0)
            {
                for (int i = 0; i <= groupingValues.Count - 1; i++)
                {
                    for (int j = 0; j <= xValues.Count - 1; j++)
                    {
                        if (!groupingValues.Contains(xValues[j]))
                        {
                            groupingValues.Add(xValues[i]);
                        }
                    }
                }
            }
            else
            {
                groupingValues.AddRange(xValues);
            }
        }

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

            var roundInterval = Math.Ceiling(interval);

            var values = ArrangeByIndex ? actualSeries?.XValues as IList : actualSeries?.GroupedXValues as IList;

            for (; position <= values?.Count - 1; position += roundInterval)
            {
                int pos = (int)position;
                if (visibleRange.Inside(pos) && pos < values?.Count && pos > -1)
                {
                    var format = LabelStyle != null ? LabelStyle.LabelFormat : string.Empty;
                    var content = GetLabelContent(actualSeries, pos, format);
                    var axisLabel = new ChartAxisLabel(pos, content != null ? content : string.Empty);
                    actualLabels?.Add(axisLabel);
                    if (LabelPlacement != LabelPlacement.BetweenTicks)
                    {
                        TickPositions.Add((double)pos);
                    }

                    if (LabelPlacement == LabelPlacement.BetweenTicks)
                    {
                        double pos1 = 0;
                        pos1 = ((int)Math.Round(position)) - 0.5;
                        TickPositions.Add(pos1);
                    }
                }
            }
        }

        internal string GetLabelContent(ChartSeries? chartSeries, int pos, string labelFormat)
        {
            var labelContent = string.Empty;
            int count = 0;

            foreach (var series in RegisteredSeries)
            {
                string label = string.Empty;
#if WinUI
                //TODO : Need to remove this section, after DataManager integration. 
                var cartesianSeries = series as ChartSeries;

                var values = ArrangeByIndex ? cartesianSeries.XValues as IList : cartesianSeries.GroupedXValues as IList;

                if (values != null && values.Count > pos && pos >= 0)
                {
                    var xValue = values[pos];
                    if (xValue != null)
                    {
                        if (cartesianSeries.XValueType == ChartValueType.String)
                        {
                            label = (string)xValue;
                        }
                        else if (cartesianSeries.XValueType == ChartValueType.DateTime)
                        {
                            if (string.IsNullOrEmpty(labelFormat))
                            {
                                labelFormat = "dd/MM/yyyy";
                            }
                            xValue = Convert.ToDouble(xValue);
                            label = GetFormattedAxisLabel(labelFormat, xValue);
                        }
                        else if (cartesianSeries?.XValueType == ChartValueType.Double)
                        {
                            xValue = Convert.ToDouble(xValue);
                            label = GetActualLabelContent(xValue, labelFormat);
                        }

                        if (!string.IsNullOrEmpty(label.ToString()) && !labelContent.Equals(label) && ArrangeByIndex)
                        {
                            labelContent = count > 0 && !string.IsNullOrEmpty(labelContent) ? labelContent + ", " + label : label.ToString();
                        }

                        if (!ArrangeByIndex)
                        {
                            return label;
                        }

                        count++;
                    }
                }
#else
                var values = ArrangeByIndex ? series.ActualData as IList : series.GroupedActualData as IList;

                if (values != null && values.Count > pos && pos >= 0)
                {
                    var xValue = values[pos];
                    if (xValue != null)
                    {
                        label = GetLabel(series, pos, labelFormat);
               
                        if (!string.IsNullOrEmpty(label.ToString()) && !labelContent.Equals(label) && ArrangeByIndex)
                        {
                            labelContent = count > 0 && !string.IsNullOrEmpty(labelContent) ? labelContent + ", " + label : label.ToString();
                        }

                        if (!ArrangeByIndex)
                        {
                            return label;
                        }

                        count++;
                    }
                }
#endif
            }

            return labelContent;
        }
 
        #endregion

        #region Private Methods

        //Todo: Remove this method while implementing ArrangeByIndex feature.
        internal ChartSeries? GetActualSeries()
        {
            var visibleSeries = Area?.VisibleSeries;
            if (visibleSeries == null) return null;

            int dataCount = 0;
            ChartSeries? selectedSeries = null;
#if WinUI
            // In WinUI, We need to consider both cartesian and polar series.
            foreach (ChartSeries series in visibleSeries)
#else
            foreach (CartesianSeries series in visibleSeries)
#endif
            {
                if (series != null && series.ActualXAxis == this && series.PointsCount > dataCount)
                {
                    selectedSeries = series;
                    dataCount = series.PointsCount;
                }
            }
            return selectedSeries;
        }

        /// <summary>
        /// Methods to update axis interval.
        /// </summary>
        /// <param name="interval">The axis interval.</param>
        private void UpdateAxisInterval(double interval)
        {
            this.AxisInterval = interval;
        }

        #endregion

        #endregion
    }
}
