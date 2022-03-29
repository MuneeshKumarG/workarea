
namespace Syncfusion.UI.Xaml.Charts
{
    ///<summary>
    /// Represents a 100% bar series which is similar to regular bar series except that the Y-values stack on top of each other.
    /// </summary>
    /// <remarks>
    /// StackedBar100Series resembles <see cref="StackedBarSeries"/>, but the cumulative portion of each stacked element always totals to 100%.
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
    ///           <chart:StackedBar100Series
    ///               ItemsSource="{Binding Data}"
    ///               XBindingPath="XValue"
    ///               YBindingPath="YValue"/>
    ///               
    ///           <chart:StackedBar100Series
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
    ///     StackedBar100Series series = new StackedBar100Series();
    ///     series.ItemsSource = viewmodel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    ///     
    ///     StackedBar100Series series1 = new StackedBar100Series();
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
    public class StackedBar100Series : StackedBarSeries
    {
        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the segments of <see cref="StackedBar100Series"/>.
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
