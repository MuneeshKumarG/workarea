using Microsoft.Maui.Controls;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///       <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="Demand"
    ///                    YBindingPath="Year2010"
    ///                    SelectionBrush="Green"/>
    ///
    ///           <chart:SfCartesianChart.SelectionBehavior>
    ///               <chart:ChartSelectionBehavior />
    ///           </chart:SfCartesianChart.SelectionBehavior>
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///
    ///     ColumnSeries series = new ColumnSeries()
    ///     {
    ///        ItemsSource = new ViewModel().Data,
    ///        XBindingPath = "Demand",
    ///        YBindingPath = "Year2010",
    ///        SelectionBrush = Brush.Green
    ///     };
    ///
    ///     chart.Series.Add(series);
    ///     
    ///     chart.SelectionBehavior = new ChartSelectionBehavior();
    ///     
    /// ]]></code>
    /// ***
    /// </example>
    public class ChartSelectionBehavior : ChartBehavior 
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        internal static readonly BindableProperty TypeProperty =
            BindableProperty.Create(nameof(Type), typeof(SelectionType), typeof(ChartSelectionBehavior), SelectionType.Point, BindingMode.Default, null, null);

        /// <summary>
        /// 
        /// </summary>
        internal SelectionType Type
        {
            get { return (SelectionType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        #endregion

        #region Properties

        internal bool IsDefault { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartSelectionBehavior()
        {
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSelectionChanging(ChartSelectionChangingEventArgs chartSelectionEvent)
        {
            if (chartSelectionEvent == null)
            {
                return;
            }

            if (!chartSelectionEvent.Cancel)
            {
                ChartSeries? series = chartSelectionEvent.Series;
                if (Type == SelectionType.Point)
                {
                    UpdateSegmentSelection(series, chartSelectionEvent.CurrentIndex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSelectionChanged(ChartSelectionChangedEventArgs chartSelectionEvent)
        {
        }

        #endregion

        #region Internal Override Methods

        internal override void OnSingleTap(IChart chart, float pointX, float pointY)
        {
            base.OnSingleTap(chart, pointX, pointY);
            var visibleSeries = (chart.Area as IChartArea)?.VisibleSeries;

            if (visibleSeries != null && Type == SelectionType.Point)
            {
                for (int i = visibleSeries.Count - 1; i >= 0; i--)
                {
                    ChartSeries chartSeries = visibleSeries[i];
                    var dataPointIndex = chartSeries.GetDataPointIndex(pointX, pointY);
                    if (dataPointIndex >= 0 && chartSeries != null)
                    {
                        ChartSelectionChangingEventArgs selectionChangingEvent = new ChartSelectionChangingEventArgs();
                        selectionChangingEvent.CurrentIndex = dataPointIndex;
                        selectionChangingEvent.PreviousIndex = chartSeries.PreviousSelectedIndex;
                        selectionChangingEvent.Series = chartSeries;
                        chartSeries.Chart?.RaiseSelectionChangingEvent(selectionChangingEvent);
                        OnSelectionChanging(selectionChangingEvent);

                        if (!selectionChangingEvent.Cancel)
                        {
                            RaiseSelectionChangedEvent(chartSeries, dataPointIndex);
                        }

                        break;
                    }
                }
            }
        }

        #endregion

        #region Internal Methods

        internal void UpdateSelection(ChartSeries series, int index)
        {
            if (Type == SelectionType.Point && series.Segments.Count > 0)
            {
                UpdateSegmentSelection(series, index);
                RaiseSelectionChangedEvent(series, index);
            }
        }

        internal void UpdateSegmentColor(ChartSeries series, int index)
        {
            if (series.SelectionBrush != null && series.Segments.Count > 0)
            {
                ChartSegment chartSegment = series.Segments[index];
                chartSegment.IsSelected = true;
                var selectionColor = series.SelectionBrush;
                chartSegment.Fill = selectionColor;

                //TODO:Need to move this code to circular series class.
                if (series.Chart != null && series.Chart.Area.PlotArea is CircularPlotArea plotArea)
                {
                    if (plotArea.legend != null && plotArea.legend.IsVisible && plotArea.LegendItems.Count > index)
                    {
                        plotArea.LegendItems[index].IconBrush = selectionColor;
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void UpdateSegmentSelection(ChartSeries? series, int selectedIndex)
        {
            if (series != null)
            {
                if (selectedIndex == series.PreviousSelectedIndex)
                {
                    series.ActualSelectedIndex = -1;
                    ResetColor(series, selectedIndex);
                    series.InvalidateSeries();

                    if (series.ShowDataLabels)
                        series.InvalidateDataLabel();
                    return;
                }

                if (series.PreviousSelectedIndex != -1)
                {
                    ResetColor(series, series.PreviousSelectedIndex);
                }

                if (series.ActualSelectedIndex != selectedIndex)
                {
                    if (series.ActualSelectedIndex != -1)
                    {
                        ResetColor(series, series.ActualSelectedIndex);
                    }

                    series.ActualSelectedIndex = selectedIndex;
                }

                if (series.IsIndividualSegment())
                {
                    UpdateSegmentColor(series, selectedIndex);
                }

                series.InvalidateSeries();

                if (series.ShowDataLabels)
                    series.InvalidateDataLabel();
            }
        }

        private void ResetColor(ChartSeries series, int index)
        {
            if (series.IsIndividualSegment() && series.Segments.Count > index)
            {
                ChartSegment chartSegment = series.Segments[index];
                chartSegment.IsSelected = false;

                if (chartSegment != null)
                {
                    chartSegment.Fill = series.GetFillColor(index);
                    var plotArea = series.Chart?.Area.PlotArea;

                    if (plotArea != null && chartSegment.Fill != null && plotArea is CircularPlotArea circularPlotArea)
                    {
                        if (circularPlotArea.legend != null && circularPlotArea.legend.IsVisible && circularPlotArea.LegendItems.Count > index)
                        {
                            circularPlotArea.LegendItems[index].IconBrush = chartSegment.Fill;
                        }
                    }                    
                }
            }
        }

        private void RaiseSelectionChangedEvent(ChartSeries series, int index)
        {
            ChartSelectionChangedEventArgs chartSelectionEvent = new ChartSelectionChangedEventArgs();
            chartSelectionEvent.CurrentIndex = index;
            chartSelectionEvent.PreviousIndex = series.PreviousSelectedIndex;
            chartSelectionEvent.Series = series;

            series.Chart?.RaiseSelectionChangedEvent(chartSelectionEvent);
            OnSelectionChanged(chartSelectionEvent);
            series.PreviousSelectedIndex = series.SelectedIndex;
        }

        #endregion

        #endregion
    }
}
