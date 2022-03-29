using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a series which visualizes data in terms of values and angles. It provides options for visual comparison between several quantitative or qualitative aspects of a situation. 
    /// </summary>
    /// <remarks>
    /// Polar charts are most commonly used to plot polar data, where each data point is determined by an angle and a distance.
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfChart>
    ///  
    ///          <chart:SfChart.DataContext>
    ///            <local:ViewModel/>
    ///          </chart:SfChart.DataContext>
    ///
    ///           <chart:SfChart.PrimaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfChart.PrimaryAxis>
    ///
    ///           <chart:SfChart.SecondaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfChart.SecondaryAxis>
    ///
    ///           <chart:PolarSeries
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
    ///     NumericalAxis primaryAxis = new NumericalAxis();
    ///     NumericalAxis secondaryAxis = new NumericalAxis();
    ///     
    ///     chart.PrimaryAxis = primaryAxis;
    ///     chart.SecondaryAxis = secondaryAxis;
    ///     
    ///     PolarSeries series = new PolarSeries();
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
    /// <seealso cref="RadarSeries"/>
    public class PolarSeries : PolarRadarSeriesBase
    {
        #region Methods

        #region Public Override Methods
        
        /// <summary>
        /// Creates the segments of PolarSeries.
        /// </summary>
        public override void CreateSegments()
        {
            List<double> tempYValues = new List<double>();
            Segments.Clear(); Segment = null;

            if (DrawType == ChartSeriesDrawType.Area)
            {
                double Origin = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0;
                List<double> xValues = GetXValues().ToList();
                tempYValues = (from val in YValues select val).ToList();

                if (xValues != null)
                {
                    if (!IsClosed)
                    {
                        xValues.Insert((int)DataCount - 1, xValues[(int)DataCount - 1]);
                        xValues.Insert(0, xValues[0]);
                        tempYValues.Insert(0, Origin);
                        tempYValues.Insert(tempYValues.Count, Origin);
                    }
                    else
                    {
                        xValues.Insert(0, xValues[0]);
                        tempYValues.Insert(0, YValues[0]);
                        xValues.Insert(0, xValues[(int)DataCount]);
                        tempYValues.Insert(0, YValues[(int)DataCount - 1]);
                    }

                    if (Segment == null)
                    {
                        Segment = CreateSegment() as AreaSegment;
                        if (Segment != null)
                        {
                            Segment.Series = this;
                            Segment.SetData(xValues, tempYValues);
                            Segments.Add(Segment);
                        }
                    }
                    else
                        Segment.SetData(xValues, tempYValues);

                    if (AdornmentsInfo != null && ShowDataLabels)
                        AddAreaAdornments(YValues);
                }
            }
            else if (DrawType == ChartSeriesDrawType.Line)
            {
                int index = -1;
                int i = 0;
                double xIndexValues = 0d;
                List<double> xValues = ActualXValues as List<double>;

                if (IsIndexed || xValues == null)
                {
                    xValues = xValues != null ? (from val in (xValues) select (xIndexValues++)).ToList()
                          : (from val in (ActualXValues as List<string>) select (xIndexValues++)).ToList();
                }

                if (xValues != null)
                {
                    for (i = 0; i < this.DataCount; i++)
                    {
                        index = i + 1;

                        if (index < DataCount)
                        {
                            if (i < Segments.Count)
                            {
                                (Segments[i]).SetData(xValues[i], YValues[i], xValues[index], YValues[index]);
                            }
                            else
                            {
                                CreateSegment(new[] { xValues[i], YValues[i], xValues[index], YValues[index] });
                            }
                        }

                        if (AdornmentsInfo != null && ShowDataLabels)
                        {
                            if (i < Adornments.Count)
                            {
                                Adornments[i].SetData(xValues[i], YValues[i], xValues[i], YValues[i]);
                            }
                            else
                            {
                                Adornments.Add(this.CreateAdornment(this, xValues[i], YValues[i], xValues[i], YValues[i]));
                            }
                        }
                    }

                    if (IsClosed)
                    {
                        CreateSegment(new[] { xValues[0], YValues[0], xValues[i - 1], YValues[i - 1] });
                    }

                    if (ShowEmptyPoints)
                        UpdateEmptyPointSegments(xValues, false);
                }
            }
        }

        /// <summary>
        /// Add the <see cref="LineSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="values">The values.</param>
        private void CreateSegment(double[] values)
        {
            LineSegment segment = CreateSegment() as LineSegment;
            if (segment != null)
            {
                segment.Series = this;
                segment.Item = this;
                segment.SetData(values);
                Segment = segment;
                Segments.Add(segment);
            }
        }

        #endregion

        #region Protected Internal Override Methods

        /// <inheritdoc/>
        protected internal override IChartTransformer CreateTransformer(Size size, bool create)
        {
            if (create || ChartTransformer == null)
            {
                ChartTransformer = ChartTransform.CreatePolar(new Rect(new Point(), size), this);
            }

            return ChartTransformer;
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        protected override ChartSegment CreateSegment()
        {
            if (DrawType == ChartSeriesDrawType.Area)
            {
                return new AreaSegment();
            }
            else
            {
                return new LineSegment();
            } 
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Represents a series which visualizes data in terms of values and angles. It provides options for visual comparison between several quantitative or qualitative aspects of a situation. 
    /// </summary>
    /// <remarks>
    /// Polar charts are most commonly used to plot polar data, where each data point is determined by an angle and a distance.
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///  
    ///          <chart:SfPolarChart.DataContext>
    ///            <local:ViewModel/>
    ///          </chart:SfPolarChart.DataContext>
    ///
    ///           <chart:SfPolarChart.PrimaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfPolarChart.PrimaryAxis>
    ///
    ///           <chart:SfPolarChart.SecondaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfPolarChart.SecondaryAxis>
    ///
    ///           <chart:PolarLineSeries
    ///               ItemsSource="{Binding Data}"
    ///               XBindingPath="XValue"
    ///               YBindingPath="YValue"/>
    ///                    
    ///     </chart:SfPolarChart>
    /// ]]></code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     
    ///     ViewModel viewmodel = new ViewModel();
    ///     
    ///     NumericalAxis primaryAxis = new NumericalAxis();
    ///     NumericalAxis secondaryAxis = new NumericalAxis();
    ///     
    ///     chart.PrimaryAxis = primaryAxis;
    ///     chart.SecondaryAxis = secondaryAxis;
    ///     
    ///     PolarLineSeries series = new PolarLineSeries();
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
    /// <seealso cref="PolarAreaSeries"/>
    public class PolarLineSeries : PolarSeries
    {
        /// <summary>
        /// Initializes a new instance of the PolarLineSeries.
        /// </summary>
        public PolarLineSeries()
        {
            DrawType = ChartSeriesDrawType.Line;
        }
    }

    /// <summary>
    /// Represents a series which visualizes data in terms of values and angles. It provides options for visual comparison between several quantitative or qualitative aspects of a situation. 
    /// </summary>
    /// <remarks>
    /// Polar charts are most commonly used to plot polar data, where each data point is determined by an angle and a distance.
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfPolarChart>
    ///  
    ///          <chart:SfPolarChart.DataContext>
    ///            <local:ViewModel/>
    ///          </chart:SfPolarChart.DataContext>
    ///
    ///           <chart:SfPolarChart.PrimaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfPolarChart.PrimaryAxis>
    ///
    ///           <chart:SfPolarChart.SecondaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfPolarChart.SecondaryAxis>
    ///
    ///           <chart:PolarAreaSeries
    ///               ItemsSource="{Binding Data}"
    ///               XBindingPath="XValue"
    ///               YBindingPath="YValue"/>
    ///                    
    ///     </chart:SfPolarChart>
    /// ]]></code>
    /// # [MainWindow.cs](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfPolarChart chart = new SfPolarChart();
    ///     
    ///     ViewModel viewmodel = new ViewModel();
    ///     
    ///     NumericalAxis primaryAxis = new NumericalAxis();
    ///     NumericalAxis secondaryAxis = new NumericalAxis();
    ///     
    ///     chart.PrimaryAxis = primaryAxis;
    ///     chart.SecondaryAxis = secondaryAxis;
    ///     
    ///     PolarAreaSeries series = new PolarAreaSeries();
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
    /// <seealso cref="PolarLineSeries"/>
    public class PolarAreaSeries : PolarSeries
    {
        /// <summary>
        /// Initializes a new instance of the PolarAreaSeries.
        /// </summary>
        public PolarAreaSeries()
        {
            DrawType = ChartSeriesDrawType.Area;
        }
    }
}