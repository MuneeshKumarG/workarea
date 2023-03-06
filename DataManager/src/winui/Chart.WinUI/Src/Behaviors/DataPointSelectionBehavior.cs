using Microsoft.UI;
using Microsoft.UI.Xaml;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Enables the selection of individual or multiple data points in a series.
    /// </summary>
    /// <remarks>
    /// <para>To enable the data point selection in the chart, create an instance of <see cref="DataPointSelectionBehavior"/> and set it to the <c>SelectionBehavior</c> property of the chart or series. To highlight the selected segment data label, set the value for <see cref="ChartSelectionBehavior.SelectionBrush"/> property in <see cref="DataPointSelectionBehavior"/>.</para>
    /// <para>It provides the following options to customize the appearance of selected data point:</para>
    /// <para> <b>Type - </b> To select single or multiple data points in a chart, refer to the <see cref="ChartSelectionBehavior.Type"/> property.</para>
    /// <para> <b>SelectionBrush - </b> To highlight the selected data point, refer to the <see cref="ChartSelectionBehavior.SelectionBrush"/> property.</para>
    /// <para> <b>SelectedIndex - </b> To select a data point programmatically, refer to the <see cref="ChartSelectionBehavior.SelectedIndex"/> property.</para>
    /// <para> <b>SelectedIndexes - </b> To select multiple points programmatically, refer to the <see cref="ChartSelectionBehavior.SelectedIndexes"/> property.</para>
    /// <para> <b>Events - </b> The <see cref="ChartSelectionBehavior.SelectionChanging"/> event occurs before the data point is being selected. The <see cref="ChartSelectionBehavior.SelectionChanged"/> event occurs after a data point has been selected.</para>
    /// </remarks>
    /// <example>
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCircularChart>
    /// 
    ///         <chart:SfCircularChart.DataContext>
    ///             <local:ViewModel/>
    ///         </chart:SfCircularChart.DataContext>
    /// 
    ///         <chart:PieSeries ItemsSource="{Binding Data}"
    ///                          XBindingPath="XValue"
    ///                          YBindingPath="YValue">
    ///             <chart:PieSeries.SelectionBehavior>
    ///                 <chart:DataPointSelectionBehavior SelectionBrush = "Red" />
    ///         </chart:PieSeries.SelectionBehavior>
    ///         </chart:PieSeries>
    /// 
    /// </chart:SfCircularChart>
    /// ]]>
    /// </code>
    /// 
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///  SfCircularChart chart = new SfCircularChart();
    ///  ViewModel viewModel = new ViewModel();
    ///  
    ///  PieSeries series = new PieSeries()
    ///  {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue",
    ///  };
    ///  
    ///  series.SelectionBehavior = new DataPointSelectionBehavior()
    ///  {
    ///      SelectionBrush = new SolidColorBrush(Colors.Red),
    ///  };
    ///  
    ///  chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    public class DataPointSelectionBehavior : ChartSelectionBehavior
    {
        internal ChartSeries? Series { get; set; }

        #region Methods

        internal override void OnSelectionIndexChanged(DependencyPropertyChangedEventArgs args)
        {
            if (Series == null) return;

            if (!EnableMultiSelection)
                Series.SelectedIndexChanged((int)args.NewValue, (int)args.OldValue);
            else if ((int)args.NewValue != -1)
                Series.SelectedSegmentsIndexes.Add((int)args.NewValue);
        }

        internal override void DetachElements()
        {
            base.DetachElements();

            Series = null;
        }

        internal override void OnMouseDownSelection(ChartSeries series, object value)
        {
            if (Series!=null && Series.ActualArea.GetEnableSeriesSelection() || series != Series) return;

            Chart.CurrentSelectedSeries = series;

            var chartDataPointInfo = value as ChartDataPointInfo;
            
            if (Chart.CurrentSelectedSeries.GetEnableSegmentSelection()
                && ((value != null && value.GetType() == typeof(int)) || chartDataPointInfo != null))
            {
                int newIndex = value != null && value.GetType() == typeof(int) ? (int)value : chartDataPointInfo.Index;
                int oldIndex = Chart.CurrentSelectedSeries.SelectionBehavior.SelectedIndex;
                // Call OnSelectionChanging method to raise SelectionChanging event  

                bool isCancel = false;

                if (Type != ChartSelectionType.Single || newIndex != oldIndex)
                    isCancel = Chart.CurrentSelectedSeries.RaiseSelectionChanging(newIndex, oldIndex);

                if (!isCancel)
                {
                    if (EnableMultiSelection)
                    {
                        if (Chart.CurrentSelectedSeries.SelectedSegmentsIndexes.Contains(newIndex))
                            Chart.CurrentSelectedSeries.SelectedSegmentsIndexes.Remove(newIndex);
                        else
                        {
                            Chart.CurrentSelectedSeries.SelectedSegmentsIndexes.Add(newIndex);
                            Chart.PreviousSelectedSeries = Chart.CurrentSelectedSeries;
                        }
                    }
                    else
                    {
                        ChartSeries currentSelectedSeries = Chart.CurrentSelectedSeries;
                        if (currentSelectedSeries.SelectionBehavior.SelectedIndex == newIndex)
                        {
                            if (Type == ChartSelectionType.SingleDeselect)
                                currentSelectedSeries.SelectionBehavior.SelectedIndex = -1;
                        }
                        else
                        {
                            currentSelectedSeries.SelectionBehavior.SelectedIndex = newIndex;
                            Chart.PreviousSelectedSeries = Chart.CurrentSelectedSeries;
                        }
                    }
                }
            }
        }

        internal void UpdateSelectedIndexSelection(ChartSeries series)
        {
            if (SelectedIndex >= 0)
            {
                int index = SelectedIndex;
                if (!series.SelectedSegmentsIndexes.Contains(index))
                    series.SelectedSegmentsIndexes.Add(index);
            }
            else if (SelectedIndexes.Count > 0)
            {
                foreach (int index in SelectedIndexes)
                {
                    if (!series.SelectedSegmentsIndexes.Contains(index))
                        series.SelectedSegmentsIndexes.Add(index);
                }
            }

            series.triggerSelectionChangedEventOnLoad = true;
        }

        #endregion
    }
}
