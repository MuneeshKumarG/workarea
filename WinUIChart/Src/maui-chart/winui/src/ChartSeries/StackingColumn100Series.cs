
namespace Syncfusion.UI.Xaml.Charts
{
    ///<summary>
    /// Represents a 100% column series which is similar to regular column series except that the Y-values stack on top of each other.
    /// </summary>
    /// <remarks>
    /// StackedColumn100Series resembles <see cref="StackedColumnSeries"/>, but the cumulative portion of each stacked element always totals to 100%.
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfChart>
    ///
    ///           <chart:SfChart.PrimaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfChart.PrimaryAxis>
    ///
    ///           <chart:SfChart.SecondaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfChart.SecondaryAxis>
    ///
    ///           <chart:StackedColumn100Series
    ///               ItemsSource="{Binding Data}"
    ///               XBindingPath="XValue"
    ///               YBindingPath="YValue"/>
    ///               
    ///           <chart:StackedColumn100Series
    ///               ItemsSource="{Binding Data}"
    ///               XBindingPath="XValue"
    ///               YBindingPath="YValue1"/>
    ///                    
    ///     </chart:SfChart>
    /// ]]></code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfChart chart = new SfChart();
    ///     
    ///     NumericalAxis primaryAxis = new NumericalAxis();
    ///     NumericalAxis secondaryAxis = new NumericalAxis();
    ///     
    ///     chart.PrimaryAxis = primaryAxis;
    ///     chart.SecondaryAxis = secondaryAxis;
    ///     
    ///     StackedColumn100Series series = new StackedColumn100Series();
    ///     series.ItemsSource = viewmodel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    ///     
    ///     StackedColumn100Series series1 = new StackedColumn100Series();
    ///     series1.ItemsSource = viewmodel.Data;
    ///     series1.XBindingPath = "XValue";
    ///     series1.YBindingPath = "YValue1";
    ///     chart.Series.Add(series1);
    /// ]]></code>
    /// # [ViewModel.cs](#tab/tabid-3)
    /// <code><![CDATA[
    /// public ObservableCollection<Model> Data { get; set; }
    /// 
    /// public ViewModel()
    /// {
    ///    Data = new ObservableCollection<Model>();
    ///    Data.Add(new Model() { XValue = 10, YValue = 100, YValue1 = 110 });
    ///    Data.Add(new Model() { XValue = 20, YValue = 150, YValue1 = 100 });
    ///    Data.Add(new Model() { XValue = 30, YValue = 110, YValue1 = 130 });
    ///    Data.Add(new Model() { XValue = 40, YValue = 230, YValue1 = 180 });
    /// }
    /// ]]></code>
    /// ***
    /// </example>
    public class StackedColumn100Series : StackedColumnSeries
    {
        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of <see cref="StackedColumn100Series"/>.
        /// </summary>
        public override void CreateSegments()
        {
            base.CreateSegments();
            IsStacked100 = true;
        }

        #endregion
      
        #endregion
    }
}
