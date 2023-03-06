using Microsoft.UI.Xaml;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="StackedArea100Series"/> is represents a 100% area series which is similar to regular area series except that the Y-values stack on top of each other.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="StackedArea100Series"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="Stroke"/>, <see cref="XyDataSeries.StrokeWidth"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="StackedArea100Series"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="StackedArea100Series"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
    /// <para> <b>Selection - </b> To enable the data point selection in a chart, create an instance of <see cref="DataPointSelectionBehavior"/> and set it to the <see cref="StackedAreaSeries.SelectionBehavior"/> property of the chart series. To highlight the selected segment data label, set the value for <see cref="ChartSelectionBehavior.SelectionBrush"/> property in <see cref="DataPointSelectionBehavior"/>.</para>
    /// <para> <b>Animation - </b> To animate the series, set <b>True</b> to the <see cref="ChartSeries.EnableAnimation"/> property.</para>
    /// <para> <b>LegendIcon - </b> To customize the legend icon using the <see cref="ChartSeries.LegendIcon"/> property.</para>
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.XAxes>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.XAxes>
    ///
    ///           <chart:SfCartesianChart.YAxes>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.YAxes>
    ///
    ///           <chart:StackedArea100Series ItemsSource="{Binding Data}"
    ///                                       XBindingPath="XValue"
    ///                                       YBindingPath="YValue"/>
    ///
    ///           <chart:StackedArea100Series ItemsSource="{Binding Data}"
    ///                                       XBindingPath="XValue"
    ///                                       YBindingPath="YValue1"/>
    /// 
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    /// 
    ///     NumericalAxis xAxis = new NumericalAxis();
    ///     NumericalAxis yAxis = new NumericalAxis();
    ///     
    ///     chart.XAxes.Add(xAxis);
    ///     chart.YAxes.Add(yAxis);
    /// 
    ///     ViewModel viewModel = new ViewModel();
    /// 
    ///     StackedArea100Series series = new StackedArea100Series();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    /// 
    ///     StackedArea100Series series1 = new StackedArea100Series();
    ///     series1.ItemsSource = viewModel.Data;
    ///     series1.XBindingPath = "XValue";
    ///     series1.YBindingPath = "YValue1";
    ///     chart.Series.Add(series1); 
    /// 
    /// ]]>
    /// </code>
    /// # [ViewModel](#tab/tabid-3)
    /// <code><![CDATA[
    ///     public ObservableCollection<Model> Data { get; set; }
    /// 
    ///     public ViewModel()
    ///     {
    ///        Data = new ObservableCollection<Model>();
    ///        Data.Add(new Model() { XValue = 10, YValue = 100, YValue1 = 110 });
    ///        Data.Add(new Model() { XValue = 20, YValue = 150, YValue1 = 100 });
    ///        Data.Add(new Model() { XValue = 30, YValue = 110, YValue1 = 130 });
    ///        Data.Add(new Model() { XValue = 40, YValue = 230, YValue1 = 180 });
    ///     }
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    /// <seealso cref="StackedAreaSegment"/>
    public class StackedArea100Series : StackedAreaSeries
    {
        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of <see cref="StackedArea100Series"/>.
        /// </summary>
        internal override void GenerateSegments()
        {
            base.GenerateSegments();
            IsStacked100 = true;
        }

        #endregion

        #endregion
    }
}
