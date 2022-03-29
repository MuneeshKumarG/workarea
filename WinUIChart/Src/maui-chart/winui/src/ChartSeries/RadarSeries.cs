using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a series which visualizes data in terms of values and angles. It provides options for visual comparison between several quantitative or qualitative aspects of a situation.
    /// </summary>
    /// <remarks>
    /// Unlike the <see cref="PolarSeries"/>, RadarSeries does not display data in terms of polar coordinates.
    /// RadarSeries is useful for comparisons between multiple series of category data.
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
    ///           <chart:RadarSeries
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
    ///     RadarSeries series = new RadarSeries();
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
    /// <seealso cref="PolarSeries"/>
    public class RadarSeries : PolarRadarSeriesBase
    {
        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Creates the Segments of RadarSeries.
        /// </summary>
        public override void CreateSegments()
        {
            Segments.Clear(); Segment = null;

            if (DrawType == ChartSeriesDrawType.Area)
            {
                double Origin = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0;
                List<double> xValues = GetXValues().ToList();
                List<double> tempYValues = new List<double>();
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
                        if (index < this.DataCount)
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

                            Adornments[i].Item = ActualData[i];
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

        #endregion

        #region Private Methods

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
}
