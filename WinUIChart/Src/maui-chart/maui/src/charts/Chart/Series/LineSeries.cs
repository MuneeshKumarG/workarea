using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

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
    ///           <chart:SfCartesianChart.XAxes>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.XAxes>
    ///
    ///           <chart:SfCartesianChart.YAxes>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfCartesianChart.YAxes>
    ///
    ///           <chart:SfCartesianChart.Series>
    ///               <chart:LineSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"/>
    ///           </chart:SfCartesianChart.Series>  
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
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
    ///     LineSeries series = new LineSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
    ///     
    /// ]]></code>
    /// # [ViewModel](#tab/tabid-3)
    /// <code><![CDATA[
    ///     public ObservableCollection<Model> Data { get; set; }
    /// 
    ///     public ViewModel()
    ///     {
    ///        Data = new ObservableCollection<Model>();
    ///        Data.Add(new Model() { XValue = 10, YValue = 100 });
    ///        Data.Add(new Model() { XValue = 20, YValue = 150 });
    ///        Data.Add(new Model() { XValue = 30, YValue = 110 });
    ///        Data.Add(new Model() { XValue = 40, YValue = 230 });
    ///     }
    /// ]]></code>
    /// ***
    /// </example>
    /// <seealso cref="LineSegment"/>
    /// <seealso cref="SplineSeries"/>
    public partial class LineSeries : XYDataSeries
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty StrokeDashArrayProperty =
            BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(LineSeries), null, BindingMode.Default, null, OnStrokeDashArrayPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public LineSeries() : base()
        {

        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// 
        /// </summary>
        protected override ChartSegment CreateSegment()
        {
            return new LineSegment();
        }

        #endregion

        #region Internal Methods

        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();

            if (xValues == null)
            {
                return;
            }

            if (PointsCount == 1)
            {
                CreateSegment(seriesView, new[] { xValues[0], YValues[0], double.NaN, double.NaN }, 0);
            }
            else
            {
                for (int i = 0; i < PointsCount; i++)
                {
                    if (i < Segments.Count)
                    {
                        Segments[i].SetData(new[] { xValues[i], YValues[i], xValues[i + 1], YValues[i + 1] });
                    }
                    else
                    {
                        if (i == PointsCount - 1)
                        {
                            CreateSegment(seriesView, new[] { xValues[i], YValues[i], double.NaN, double.NaN }, i);
                        }
                        else
                            CreateSegment(seriesView, new[] { xValues[i], YValues[i], xValues[i + 1], YValues[i + 1] }, i);
                    }
                }
            }
        }

        internal override bool IsIndividualSegment()
        {
            return false;
        }

        internal override void SetDashArray(ChartSegment segment)
        {
            segment.StrokeDashArray = StrokeDashArray;
        }

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            return DataLabelSettings.GetLabelPositionForContinuousSeries(this, dataLabel.Index, labelSize, labelPosition, padding);
        }

        internal override bool SeriesContainsPoint(PointF point)
        {
            if (Chart != null)
            {
                if (base.SeriesContainsPoint(point))
                {
                    return true;
                }

                var dataPoint = FindNearestChartPoint((float)point.X, (float)point.Y);

                if (dataPoint == null || ActualData == null)
                {
                    return false;
                }

                TooltipDataPointIndex = ActualData.IndexOf(dataPoint);

                if (Segments.Count == 0 || TooltipDataPointIndex < 0 || double.IsNaN(YValues[TooltipDataPointIndex]))
                {
                    return false;
                }

                LineSegment? startSegment = null;
                LineSegment? endSegment = null;
                var seriesClipRect = AreaBounds;
                point.X = point.X - ((float)seriesClipRect.Left);
                point.Y = point.Y - ((float)seriesClipRect.Top);

                if (TooltipDataPointIndex == 0)
                {
                    startSegment = Segments[TooltipDataPointIndex] as LineSegment;
                }
                else if (TooltipDataPointIndex == PointsCount - 1)
                {
                    startSegment = Segments[TooltipDataPointIndex - 1] as LineSegment;
                }
                else
                {
                    startSegment = Segments[TooltipDataPointIndex - 1] as LineSegment;
                    endSegment = Segments[TooltipDataPointIndex] as LineSegment;
                }

                if (startSegment != null && ChartUtils.SegmentContains(startSegment, point, this))
                {
                    return true;
                }

                if (endSegment != null)
                {
                    return ChartUtils.SegmentContains(endSegment, point, this);
                }
            }

            return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Add the <see cref="LineSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="index">The index value.</param>
        /// <param name="seriesView">The seriesview.</param>
        private void CreateSegment(SeriesView seriesView, double[] values, int index)
        {
            var segment = CreateSegment() as LineSegment;
            if (segment != null)
            {
                segment.Series = this;
                segment.SeriesView = seriesView;
                segment.Index = index;
                segment.SetData(values);
                Segments.Add(segment);

                if (OldSegments != null && OldSegments.Count > 0 && OldSegments.Count > index)
                {
                    LineSegment? oldSegment = OldSegments[index] as LineSegment;

                    if (oldSegment != null)
                        segment.SetPreviousData(new[] { oldSegment.X1, oldSegment.Y1, oldSegment.X2, oldSegment.Y2 });
                }
            }
        }

        #region Private Callback
        private static void OnStrokeDashArrayPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as LineSeries;
            if (series != null)
            {
                series.UpdateDashArray();
                series.InvalidateSeries();
            }
        }
        #endregion

        #endregion

        #endregion
    }
}
