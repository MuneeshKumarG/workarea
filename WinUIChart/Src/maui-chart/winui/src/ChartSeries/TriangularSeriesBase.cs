using Microsoft.UI.Xaml;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a base class for funnel and pyramid series. This type of chart is triangle with lines dividing it into sections to illustrate numerical proportions..
    /// </summary>
    public abstract class TriangularSeriesBase : AccumulationSeriesBase
    {

        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <c>GapRatio</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>GapRatio</c> dependency property.
        /// </value>   
        public static readonly DependencyProperty GapRatioProperty =
            DependencyProperty.Register(
                nameof(GapRatio),
                typeof(double),
                typeof(TriangularSeriesBase),
                new PropertyMetadata(0d, new PropertyChangedCallback(OnGapRatioChanged)));

        /// <summary>
        /// Identifies the <c>ExplodeOffset</c> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <c>ExplodeOffset</c> dependency property.
        /// </value>   
        public static readonly DependencyProperty ExplodeOffsetProperty =
            DependencyProperty.Register(
                nameof(ExplodeOffset), 
                typeof(double), 
                typeof(TriangularSeriesBase),
                new PropertyMetadata(40d, new PropertyChangedCallback(OnExplodeOffsetChanged)));

        #endregion

        #region Properties

        #region Public Properties


        /// <summary>
        /// Gets or sets the ratio of distance between the funnel or pyramid segment blocks.
        /// </summary>
        /// <value>Default value is 0 and its value ranges from 0 to 1.</value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfChart>
        ///
        ///          <chart:SfChart.DataContext>
        ///            <local:ViewModel/>
        ///          </chart:SfChart.DataContext>
        ///          
        ///          <chart:FunnelSeries
        ///               GapRatio="0.5"
        ///               ItemsSource="{Binding Data}"
        ///               XBindingPath="XValue"
        ///               YBindingPath="YValue"/>
        ///                    
        ///     </chart:SfChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfChart chart = new SfChart();
        ///     
        ///     ViewModel viewmodel = new ViewModel();
        ///     
        ///     FunnelSeries series = new FunnelSeries();
        ///     series.GapRatio = 0.5;
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        /// ]]></code>
        /// # [ViewModel.cs](#tab/tabid-3)
        /// <code><![CDATA[
        /// public ObservableCollection<Model> Data { get; set; }
        /// 
        /// public ViewModel()
        /// {
        ///    Data = new ObservableCollection<Model>();
        ///    Data.Add(new Model() { XValue = 10, YValue = 100 });
        ///    Data.Add(new Model() { XValue = 20, YValue = 150 });
        ///    Data.Add(new Model() { XValue = 30, YValue = 110 });
        ///    Data.Add(new Model() { XValue = 40, YValue = 230 });
        /// }
        /// ]]></code>
        /// ***
        /// </example>
        /// <remarks>Its used to provide the spacing between the segments.</remarks>
        public double GapRatio
        {
            get { return (double)GetValue(GapRatioProperty); }
            set { SetValue(GapRatioProperty, value); }
        }

        /// <summary>
        /// Gets or sets the distance where the segment is exploded from its origination positions when <c>ExplodeAll</c> is true or <c>ExplodeIndex</c> value is given.
        /// </summary>
        /// <value>Default value is 40.</value>
        public double ExplodeOffset
        {
            get { return (double)GetValue(ExplodeOffsetProperty); }
            set { SetValue(ExplodeOffsetProperty, value); }
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Override Methods

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            Point newPosition = new Point();

            if (Area == null || Area.RootPanelDesiredSize == null || Area.RootPanelDesiredSize.Value == null)
                return newPosition;

            var actualwidth = Area.RootPanelDesiredSize.Value.Width;
            var actualHeight = Area.RootPanelDesiredSize.Value.Height;

            newPosition.X = actualwidth / 2;

            if (tooltip.DataContext is FunnelSegment)
            {
                var funnelSegment = tooltip.DataContext as FunnelSegment;
                newPosition.Y = (funnelSegment.top * actualHeight) + funnelSegment.height;
            }
            else if (tooltip.DataContext is PyramidSegment)
            {
                var pyramidSegment = tooltip.DataContext as PyramidSegment;
                newPosition.Y = (pyramidSegment.y * actualHeight) + pyramidSegment.height;
            }

            return newPosition;
        }

        #endregion

        #region Private Static Methods

        private static void OnGapRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var triangularSeriesBase = d as TriangularSeriesBase;
            if (triangularSeriesBase != null && triangularSeriesBase.Area != null)
                triangularSeriesBase.Area.ScheduleUpdate();
        }
        
        private static void OnExplodeOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var triangularSeriesBase = d as TriangularSeriesBase;
            if (triangularSeriesBase != null && triangularSeriesBase.Area != null)
            {
                triangularSeriesBase.Area.ScheduleUpdate();
            }
        }

        #endregion

        #endregion
    }
}
