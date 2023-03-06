using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Core.Internals;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    internal class CartesianChartArea : AreaBase, ICartesianChartArea
    {
        #region Private Fields
        private readonly CartesianPlotArea cartesianPlotArea;
        internal readonly CartesianAxisLayout AxisLayout;
        internal readonly ObservableCollection<ChartAxis> XAxes;
        internal readonly ObservableCollection<RangeAxisBase> YAxes;
        RectF actualSeriesClipRect;
        private bool isSbsWithOneData = false;
        private ChartSeriesCollection? series;
        private bool isTransposed = false;
        private bool enableSideBySideSeriesPlacement = true;

        #endregion

        #region Internal Properties

        internal Thickness PlotAreaMargin { get; set; } = Thickness.Zero;

        #region Chart Properties

        #region Public Properties

        public ChartAxis? PrimaryAxis { get; set; }

        public RangeAxisBase? SecondaryAxis { get; set; }

        public bool IsTransposed 
        { 
            get
            {
                return isTransposed;
            }
            set
            {
                if (value != isTransposed)
                {
                    isTransposed = value;
                    NeedsRelayout = true;
                    ScheduleUpdateArea();
                }
            }
        } 

        public bool EnableSideBySideSeriesPlacement
        {
            get
            {
                return enableSideBySideSeriesPlacement;
            }
            set
            {
                if (value != enableSideBySideSeriesPlacement)
                {
                    enableSideBySideSeriesPlacement = value;
                    ResetSBSSegments();
                    NeedsRelayout = true;
                    ScheduleUpdateArea();
                }
            }
        }

        internal void OnPaletteColorChanged()
        {
            if (Series != null)
            {
                foreach (var series in Series)
                {
                    var cartesian = series as CartesianSeries;
                    if (cartesian != null && cartesian.Chart != null)
                    {
                        series.UpdateColor();
                        series.InvalidateSeries();

                        if (series.ShowDataLabels)
                        {
                            series.InvalidateDataLabel();
                        }

                        series.UpdateLegendIconColor();
                    }
                }
            }
        }

        public ChartSeriesCollection? Series
        {
            get
            {
                return series;
            }
            set
            {
                if (value != series)
                {
                    series = value;
                    cartesianPlotArea.Series = value;
                }
            }
        }

        public ReadOnlyObservableCollection<ChartSeries>? VisibleSeries => Series?.GetVisibleSeries();

        #endregion

        /// <summary>
        /// Boolean used to clear associated axis and register series. 
        /// </summary>
        internal bool RequiredAxisReset { get; set; } = true;

        internal IList<Brush>? PaletteColors { get; set; }

        internal List<ChartAxis> DependentSeriesAxes { get; set; }

        internal Rect SeriesClipRect { get; set; }

        internal RectF ActualSeriesClipRect { get { return actualSeriesClipRect; } set { actualSeriesClipRect = value; } }

        internal Dictionary<object, List<CartesianSeries>>? SideBySideSeriesPosition { get; set; }

        internal double PreviousSBSMinWidth { get; set; }

        internal double SideBySideMinWidth { get; set; }

        public override IPlotArea PlotArea => cartesianPlotArea;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public CartesianChartArea(SfCartesianChart chart)
        {
            BatchBegin();
            XAxes = chart.XAxes;
            XAxes.CollectionChanged += XAxes_CollectionChanged;
            YAxes = chart.YAxes;
            YAxes.CollectionChanged += YAxes_CollectionChanged;
            DependentSeriesAxes = new List<ChartAxis>();
            PaletteColors = ChartColorModel.DefaultBrushes;
            cartesianPlotArea = new CartesianPlotArea(this);
            cartesianPlotArea.Chart = chart;
            AxisLayout = new CartesianAxisLayout(this);
            AbsoluteLayout.SetLayoutBounds(AxisLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(AxisLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(cartesianPlotArea, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(cartesianPlotArea, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            Add(cartesianPlotArea);
            Add(AxisLayout);
            AbsoluteLayout.SetLayoutBounds(chart.BehaviorLayout, new Rect(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(chart.BehaviorLayout, Microsoft.Maui.Layouts.AbsoluteLayoutFlags.All);
            cartesianPlotArea.Add(chart.BehaviorLayout);
            BatchCommit();
        }

        #endregion

        #region Methods

        #region Protected Methods
        protected override void UpdateAreaCore()
        {
            cartesianPlotArea.Chart?.ResetTooltip();

            if (XAxes.Count <= 0 || YAxes.Count <= 0)
                return;
            
            cartesianPlotArea.PopulateDataManager();
            AxisLayout.AssignAxisToSeries();
            AxisLayout.LayoutAxis(AreaBounds);
            cartesianPlotArea.Padding = PlotAreaMargin;

            if (cartesianPlotArea.Chart != null)
                cartesianPlotArea.Chart.ActualSeriesClipRect = ChartUtils.GetSeriesClipRect(AreaBounds.SubtractThickness(PlotAreaMargin), cartesianPlotArea.Chart.TitleHeight);
 
            UpdateVisibleSeries(); //internal createsegment

            AxisLayout.InvalidateRender();
            cartesianPlotArea.InvalidateRender();
        }

        public void UpdateVisibleSeries()
        {
            cartesianPlotArea.UpdateVisibleSeries();
        }

        #endregion

        #region SBS Members

        //TODO: Need to implement and simplify SBS calculations.
        internal void CalculateSbsPosition()
        {
            int index = -1;
            var visibleSeries = VisibleSeries;

            if (SideBySideSeriesPosition != null || visibleSeries == null)
            {
                return;
            }

            SideBySideSeriesPosition = new Dictionary<object, List<CartesianSeries>>();
            var cartesianSeriesCollection = visibleSeries.OfType<CartesianSeries>();

            foreach (var cartesianSeries in cartesianSeriesCollection)
            {
                cartesianSeries.IsSbsValueCalculated = false;
            }

            foreach (var cartesianSeries in cartesianSeriesCollection)
            {
                if (cartesianSeries.ActualXAxis == null)
                {
                    continue;
                }

                var groupingKeys = new Dictionary<string, int>();

                foreach (var xAxisRegSeries in cartesianSeries.ActualXAxis.RegisteredSeries)
                {
                    if (xAxisRegSeries.IsSideBySide)
                    {
                        if (!xAxisRegSeries.IsSbsValueCalculated && xAxisRegSeries.ActualXAxis != null)
                        {
                            //TODO: Need to update on stacking series. 
                            if (xAxisRegSeries is StackingSeriesBase baseSeries)
                            {
                                if (baseSeries.ActualYAxis == null || baseSeries.ActualXAxis == null)
                                {
                                    continue;
                                }

                                foreach (var yAxisRegSeries in baseSeries.ActualYAxis.RegisteredSeries)
                                {
                                    if (!baseSeries.ActualXAxis.RegisteredSeries.Contains(yAxisRegSeries))
                                    {
                                        continue;
                                    }

                                    if (yAxisRegSeries is StackingSeriesBase stackingSeries)
                                    {
                                        if (!stackingSeries.IsSbsValueCalculated)
                                        {
                                            string groupID = stackingSeries.GroupingLabel + "";
                                            StackingSeriesBase stackingSeriesBase;
                                            int size = SideBySideSeriesPosition.Count > 0 && groupingKeys.Count > 0 && groupingKeys.ContainsKey(groupID)
                                                    ? SideBySideSeriesPosition[groupingKeys[groupID]].Count : 0;
                                            var isSameType = false;
                                            if (size > 0)
                                            {
                                                stackingSeriesBase = (StackingSeriesBase)SideBySideSeriesPosition[groupingKeys[groupID]][size - 1];
                                                isSameType = stackingSeriesBase != null
                                                    && stackingSeriesBase.ActualYAxis != null && stackingSeriesBase.ActualYAxis.RegisteredSeries.Contains(stackingSeries)
                                                    && stackingSeriesBase.CompareStackingSeries(stackingSeries);
                                            }

                                            if (groupingKeys.ContainsKey(groupID) && isSameType)
                                            {
                                                SideBySideSeriesPosition[groupingKeys[groupID]].Add(stackingSeries);
                                                stackingSeries.SideBySideIndex = groupingKeys[groupID];
                                                stackingSeries.IsSbsValueCalculated = true;
                                            }

                                            else
                                            {
                                                if (groupingKeys.Keys.Contains(groupID))
                                                {
                                                    groupingKeys[groupID] = ++index;
                                                }
                                                else
                                                {
                                                    groupingKeys.Add(groupID, ++index);
                                                }

                                                baseSeries.ActualXAxis.SideBySideSeriesCount = index + 1;
                                                if (SideBySideSeriesPosition.ContainsKey(index))
                                                {
                                                    SideBySideSeriesPosition[index] = new List<CartesianSeries>();
                                                }
                                                else
                                                {
                                                    SideBySideSeriesPosition.Add(index, new List<CartesianSeries>());
                                                }

                                                SideBySideSeriesPosition[index].Add(stackingSeries);
                                                stackingSeries.SideBySideIndex = index;
                                                stackingSeries.IsSbsValueCalculated = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (SideBySideSeriesPosition.ContainsKey(++index))
                                {
                                    SideBySideSeriesPosition[index] = new List<CartesianSeries>();
                                }
                                else
                                {
                                    SideBySideSeriesPosition.Add(index, new List<CartesianSeries>());
                                }

                                xAxisRegSeries.ActualXAxis.SideBySideSeriesCount = index + 1;
                                SideBySideSeriesPosition[index].Add(xAxisRegSeries);
                                xAxisRegSeries.SideBySideIndex = index;
                                xAxisRegSeries.IsSbsValueCalculated = true;
                            }
                        }
                    }
                }

                UpdateSBS();
                index = -1;
            }
        }

        internal void UpdateSBS()
        {
            if (SideBySideSeriesPosition != null)
            {
                double totalWidth = GetTotalWidth() / SideBySideSeriesPosition.Count;
                double startPosition = 0, end = 0;

                for (int i = 0; i < SideBySideSeriesPosition.Count; i++)
                {
                    var seriesGroup = SideBySideSeriesPosition.Values.ToList()[i];
                    double sbsMaxWidth = GetSBSMaxWidth(seriesGroup);

                    foreach (ChartSeries chartSeries in seriesGroup)
                    {
                        CartesianSeries cartesianSeries = (CartesianSeries)chartSeries;

                        double spacing = cartesianSeries.GetActualSpacing() < 0 ? 0 : cartesianSeries.GetActualSpacing() > 1 ? 1 : cartesianSeries.GetActualSpacing();
                        double width = cartesianSeries.GetActualWidth() < 0 ? 0 : cartesianSeries.GetActualWidth() > 1 ? 1 : cartesianSeries.GetActualWidth();

                        if (!EnableSideBySideSeriesPlacement)
                        {
                            cartesianSeries.SbsInfo = new DoubleRange((-width * SideBySideMinWidth) / 2, (width * SideBySideMinWidth) / 2);
                            continue;
                        }

                        if (cartesianSeries.ActualXAxis == null)
                        {
                            cartesianSeries.SbsInfo = DoubleRange.Empty;
                        }

                        double sideBySideMinWidth = SideBySideMinWidth == double.MaxValue ? 1 : SideBySideMinWidth;

                        int seriesCount = cartesianSeries.ActualXAxis != null ? cartesianSeries.ActualXAxis.SideBySideSeriesCount : 0;

                        if (cartesianSeries.SideBySideIndex == 0)
                        {
                            startPosition = -sideBySideMinWidth * (totalWidth / 2);
                        }

                        double space = (sbsMaxWidth - width) / seriesCount;
                        double start = startPosition + ((space * sideBySideMinWidth) / 2);

                        end = start + ((width / seriesCount) * sideBySideMinWidth);
                        spacing = (spacing * sideBySideMinWidth) / seriesCount;

                        if (!double.IsNaN(start))
                        {
                            start += spacing / 2;
                            end -= spacing / 2;
                        }

                        cartesianSeries.SbsInfo = new DoubleRange(start, end);
                        end += spacing / 2;
                    }

                    startPosition = end;
                }
            }
        }

        internal void UpdateMinWidth(CartesianSeries cartesianSeries, ref double minWidth, List<double> xValues)
        {
            int dataCount = xValues.Count;
            var visibleSeries = VisibleSeries;

            if (dataCount == 1)
            {
                isSbsWithOneData = true;
            }

            if (xValues != null && xValues.Count > 0 && !cartesianSeries.IsIndexed)
            {
                var dateTimeAxis = cartesianSeries.ActualXAxis as DateTimeAxis;

                //ColumnSeries looks tiny when series have single data point with IntervalType as Month and RangePadding as Additional.
                if (visibleSeries != null && dataCount == 1 && dateTimeAxis != null)
                {
                    var seriescollection = new ObservableCollection<CartesianSeries>();
                    foreach (CartesianSeries series in visibleSeries)
                    {
                        if (series != null && series.IsSideBySide && series.PointsCount > 0)
                        {
                            seriescollection.Add(series);
                        }
                    }

                    //When we set Minimum and Maximum property of DateTimeAxis with single data set for Stacking column series, segment has not been shown.
                    if (seriescollection != null && seriescollection.All(x => x.PointsCount == 1 && x.GetXValues()?[0] == xValues[0]))
                    {
                        minWidth = dateTimeAxis.GetMinWidthForSingleDataPoint();
                    }
                }

                var xData = new double[xValues.Count];
                xValues.CopyTo(xData, 0);
                Array.Sort(xData, 0, dataCount);
                for (int i = 0; i < dataCount - 1; i++)
                {
                    double xValue = xData[i];
                    double nextXValue = xData[i + 1];
                    double delta = nextXValue - xValue;
                    minWidth = Math.Min(delta == 0 ? minWidth : Math.Abs(delta), minWidth);
                }
            }
            else
            {
                minWidth = 1;
            }
        }

        internal void InvalidateMinWidth()
        {
            PreviousSBSMinWidth = SideBySideMinWidth;
            SideBySideMinWidth = double.MaxValue;
            double minWidth = double.MaxValue;
            
            var visibleSeries = VisibleSeries;
            if (visibleSeries != null)
            {
                foreach (ChartSeries chartSeries in visibleSeries)
                {
                    var cartesianSeries = chartSeries as CartesianSeries;

                    if (cartesianSeries != null && cartesianSeries.IsSideBySide && cartesianSeries.ItemsSource != null)
                    {
                        var dateTimeAxis = cartesianSeries.ActualXAxis as DateTimeAxis;

                        if (cartesianSeries.PointsCount >= 1)
                        {
                            List<double>? xvalues = cartesianSeries.GetXValues();

                            if (xvalues != null)
                                UpdateMinWidth(cartesianSeries, ref minWidth, xvalues);
                        }
                        else if (dateTimeAxis != null)
                        {
                            SideBySideMinWidth = dateTimeAxis.GetMinWidthForSingleDataPoint();
                        }
                        else if (SideBySideMinWidth == double.MaxValue)
                        {
                            SideBySideMinWidth = 1;
                        }
                    }
                }

                if (visibleSeries != null && visibleSeries.Count > 1 && isSbsWithOneData)
                {
                    List<double> previousXValues = new List<double>();

                    foreach (var chartSeries in visibleSeries)
                    {
                        var cartesianSeries = chartSeries as CartesianSeries;

                        if (cartesianSeries != null && !cartesianSeries.IsIndexed && cartesianSeries.IsSideBySide)
                        {
                            var xvalues = cartesianSeries.GetXValues();

                            if (xvalues != null && xvalues.Count > 0)
                            {
                                //DateTimeAxis not rendered properly when series have single datapoint with different x position
                                var actualXValues = xvalues.ToList();
                                previousXValues.AddRange(actualXValues);
                                UpdateMinWidth(cartesianSeries, ref minWidth, previousXValues);
                                previousXValues = actualXValues;
                            }
                        }
                    }

                    isSbsWithOneData = false;
                }

                minWidth = minWidth == double.MaxValue ? 1 : minWidth;
                SideBySideMinWidth = Math.Min(SideBySideMinWidth, minWidth);
            }
        }

        internal void ResetSBSSegments()
        {
            var sideBySideSeries = VisibleSeries?.Where(series=> series.IsSideBySide).ToList();
            
            if (sideBySideSeries != null && sideBySideSeries.Count > 0)
            {
                SideBySideSeriesPosition = null;

                foreach (var chartSeries in sideBySideSeries)
                {
                    chartSeries.SegmentsCreated = false;
                }
            }
        }

        private double GetTotalWidth()
        {
            double totalWidth = 0;

            if (SideBySideSeriesPosition != null)
            {
                for (int i = 0; i < SideBySideSeriesPosition.Count; i++)
                {
                    double maxWidth = 0;
                    foreach (ChartSeries sideBySideSeries in SideBySideSeriesPosition.Values.ToList()[i])
                    {
                        CartesianSeries cartesianSeries = (CartesianSeries)sideBySideSeries;
                        double width = cartesianSeries.GetActualWidth();
                        maxWidth = maxWidth > width ? maxWidth : width;
                    }

                    totalWidth += maxWidth;
                }
            }

            return totalWidth;
        }

        private static double GetSBSMaxWidth(List<CartesianSeries> seriesGroup)
        {
            double maxWidth = 0;

            foreach (var cartesianSeries in seriesGroup)
            {
                double width = cartesianSeries.GetActualWidth();
                maxWidth = maxWidth > width ? maxWidth : width;
            }

            return maxWidth;
        }

        internal void UpdateStackingSeries()
        {
            //if visible series count is 0 or not contain any stacking series then return.
            if (VisibleSeries == null || VisibleSeries.Count == 0 || !VisibleSeries.Any(series => series is StackingSeriesBase && !series.SegmentsCreated))
                return;
            
            var seriesGroup = new Dictionary<string, List<StackingSeriesBase>>();
            foreach (var series in VisibleSeries)
            {
                if (series is StackingSeriesBase stackingSeries && stackingSeries.IsVisible)
                {
                    var stackingGroup = stackingSeries.GroupingLabel;
                    var stackingXAxis = stackingSeries.ActualXAxis;
                    var stackingYAxis = stackingSeries.ActualYAxis;

                    if (string.IsNullOrEmpty(stackingGroup))
                    {
                        stackingGroup = "chart_default";
                    }

                    if (seriesGroup.ContainsKey(stackingGroup))
                    {
                        var seriesList = seriesGroup[stackingGroup];
                        if (seriesList.Any(x => x.ActualXAxis != stackingXAxis || x.ActualYAxis != stackingYAxis))
                        {
                            seriesGroup.Add(stackingGroup + seriesGroup.Count, new List<StackingSeriesBase>() { stackingSeries });
                        }
                        else
                        {
                            seriesList.Add(stackingSeries);
                        }
                    }
                    else
                    {
                        seriesGroup.Add(stackingGroup, new List<StackingSeriesBase>() { stackingSeries });
                    }
                }
            }

            CalculateStackingValues(seriesGroup);
        }

        private static void CalculateStackingValues(Dictionary<string, List<StackingSeriesBase>> seriesGroup)
        {
            foreach (var seriesList in seriesGroup.Values)
            {
                var xValues = seriesList[0].GetXValues();
                var positiveYValues = new Dictionary<double, double>();
                var negativeYValues = new Dictionary<double, double>();

                double axisCross = 0;

                if (seriesList[0].ActualXAxis is ChartAxis axis)
                {
                    if (!double.IsNaN(axis.ActualCrossingValue))
                    {
                        axisCross = axis.ActualCrossingValue;
                    }
                    else if(axis is LogarithmicAxis)
                    {
                        axisCross = 1;
                    }
                }

                if (xValues != null)
                {
                    foreach (var series in seriesList)
                    {
                        var yValues = series.YValues;
                        var bottomValues = new List<double>();
                        var topValues = new List<double>();

                        for (int i = 0; i < xValues.Count; i++)
                        {
                            var xValue = xValues[i];
                            var yValue = yValues[i];
                            
                            if (yValue >= axisCross)
                            {
                                if (positiveYValues.ContainsKey(xValue))
                                {
                                    bottomValues.Add(positiveYValues[xValue]);
                                    positiveYValues[xValue] += yValue;
                                }
                                else
                                {
                                    bottomValues.Add(axisCross);
                                    positiveYValues.Add(xValue, yValue);
                                }

                                topValues.Add(positiveYValues[xValue]);
                            }
                            else
                            {
                                if (negativeYValues.ContainsKey(xValue))
                                {
                                    bottomValues.Add(negativeYValues[xValue]);
                                    negativeYValues[xValue] += yValue;
                                }
                                else
                                {
                                    bottomValues.Add(axisCross);
                                    negativeYValues.Add(xValue, yValue);
                                }

                                topValues.Add(negativeYValues[xValue]);
                            }
                        }

                        series.BottomValues = bottomValues;
                        series.TopValues = topValues;
                    }
                }
            }
        }

        #endregion

        #region Axes Collection Changed
        private void XAxes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((index, obj) => AddAxes(obj), (index, obj) => RemoveAxes(obj), ResetAxes);
            var axes = sender as ObservableCollection<ChartAxis>;
            if (axes != null && axes.Count > 0)
            {
                PrimaryAxis = axes[0];
            }
            NeedsRelayout = true;
            ScheduleUpdateArea();
        }

        private void YAxes_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((index, obj) => AddAxes(obj), (index, obj) => RemoveAxes(obj), ResetAxes);

            var axes = sender as ObservableCollection<RangeAxisBase>;
            if (axes != null && axes.Count > 0)
            {
                SecondaryAxis = axes[0];
            }

            NeedsRelayout = true;
            ScheduleUpdateArea();
        }

        private void AddAxes(object obj)
        {
            var axis = obj as ChartAxis;
            if (axis != null)
            {
                axis.Area = this;
                RequiredAxisReset = true;
                SetInheritedBindingContext(axis, BindingContext);
            }
        }

        private void RemoveAxes(object obj)
        {
            var axis = obj as ChartAxis;
            if (axis != null)
            {
                RequiredAxisReset = true;
                axis.Area = null;
                //TODO:Need to unhook if any event hooked.
            }
        }

        private void ResetAxes()
        {
            //Axes not be reset.
        }
        #endregion
        
        #endregion
    }
}
