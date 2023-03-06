using Microsoft.UI.Xaml;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Enables the selection of individual or multiple series in a <see cref="SfCartesianChart"/>.
    /// </summary>
    /// <remarks>
    /// <para>To enable the series selection in the chart, create an instance of <see cref="SeriesSelectionBehavior"/> and set it to the <c>SelectionBehavior</c> property of chart. To highlight the selected series, set the value for <see cref="ChartSelectionBehavior.SelectionBrush"/> property in <see cref="SeriesSelectionBehavior"/>.</para>
    /// <para>It is applicable only for <see cref="SfCartesianChart"/>.</para>
    /// <para>It provides the following options to customize the appearance of selected series:</para>
    /// <para> <b>Type - </b> To select single or multiple series in a chart, refer to the <see cref="ChartSelectionBehavior.Type"/> property.</para>
    /// <para> <b>SelectionBrush - </b> To highlight the selected series, refer to the <see cref="ChartSelectionBehavior.SelectionBrush"/> property.</para>
    /// <para> <b>SelectedIndex - </b> To select a series programmatically, refer to the <see cref="ChartSelectionBehavior.SelectedIndex"/> property.</para>
    /// <para> <b>SelectedIndexes - </b> To select multiple series programmatically, refer to the <see cref="ChartSelectionBehavior.SelectedIndexes"/> property.</para>
    /// <para> <b>Events - </b> The <see cref="ChartSelectionBehavior.SelectionChanging"/> event occurs before the series is selected. The <see cref="ChartSelectionBehavior.SelectionChanged"/> event occurs after a series has been selected.</para>
    /// </remarks>
    /// <example>
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///        <chart:SfCartesianChart.SelectionBehavior>
    ///           <chart:SeriesSelectionBehavior SelectionBrush ="Red" />
    ///         </chart:SfCartesianChart.SelectionBehavior>
    ///
    ///        <chart:LineSeries ItemsSource ="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue1" />
    ///        <chart:LineSeries ItemsSource ="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue2" />
    ///
    ///    </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    ///
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///  SfCartesianChart chart = new SfCartesianChart();
    ///  ViewModel viewModel = new ViewModel();
    ///
    ///  chart.SelectionBehavior = new SeriesSelectionBehavior()
    ///  {
    ///      SelectionBrush = new SolidColorBrush(Colors.Red),
    ///  };
    ///
    ///  LineSeries series = new LineSeries()
    ///  {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue1",
    ///  };
    ///
    ///  LineSeries series = new LineSeries()
    ///  {
    ///     ItemsSource = viewModel.Data,
    ///     XBindingPath = "XValue",
    ///     YBindingPath = "YValue2",
    ///  };
    ///
    ///  chart.Series.Add(series);
    ///
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    public class SeriesSelectionBehavior : ChartSelectionBehavior
    {
        #region Methods

        internal override void OnSelectionIndexChanged(DependencyPropertyChangedEventArgs args)
        {
            Chart?.SeriesSelectedIndexChanged((int)args.NewValue, (int)args.OldValue);
        }

        internal void UpdateSelectedIndexSelection()
        {
            if (SelectedIndex >= 0)
            {
                ChartSeries series = Chart.VisibleSeries[SelectedIndex];

                if (!Chart.SelectedSeriesCollection.Contains(series))
                    Chart.SelectedSeriesCollection.Add(series);
            }
            else if (SelectedIndexes.Count > 0)
            {
                foreach (int index in SelectedIndexes)
                {
                    if (index < Chart.VisibleSeries.Count)
                    {
                        ChartSeries series = Chart.VisibleSeries[index];

                        if (!Chart.SelectedSeriesCollection.Contains(series))
                            Chart.SelectedSeriesCollection.Add(series);
                    }
                }
            }
        }

        #endregion
    }
}
