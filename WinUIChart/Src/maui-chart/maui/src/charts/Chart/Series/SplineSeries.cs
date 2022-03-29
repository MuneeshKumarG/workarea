
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

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
    ///               <chart:SplineSeries
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
    ///     SplineSeries series = new SplineSeries();
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
    /// <seealso cref="SplineAreaSeries"/>
    /// <seealso cref="SplineAreaSegment"/>
    /// <seealso cref="SplineSegment"/>
    public partial class SplineSeries : XYDataSeries
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty TypeProperty =
            BindableProperty.Create(
                nameof(Type),
                typeof(SplineType),
                typeof(SplineSeries),
                SplineType.Natural,
                BindingMode.Default,
                null,
                OnSplineTypeChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty StrokeDashArrayProperty =
            BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(SplineSeries), null, BindingMode.Default, null, OnStrokeDashArrayPropertyChanged);

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

        /// <summary>
        /// 
        /// </summary>
        public SplineType Type
        {
            get { return (SplineType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override ChartSegment CreateSegment()
        {
            return new SplineSegment();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates the segments of <see cref="SplineSeries"/>.
        /// </summary>
        internal override void GenerateSegments(SeriesView seriesView)
        {
            var xValues = GetXValues();
            if (xValues == null || xValues.Count == 0)
            {
                return;
            }

            double[]? dx = null;

            if (PointsCount == 1)
            {
                CreateSegment(seriesView, new[] { xValues[0], YValues[0], double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN }, 0);
            }
            else
            {
                double[]? yCoef;
                if (Type == SplineType.Monotonic)
                {
                    yCoef = GetMonotonicSpline(xValues, YValues, out dx);
                }
                else if (Type == SplineType.Cardinal)
                {
                    yCoef = GetCardinalSpline(xValues, YValues);
                }
                else
                {
                    yCoef = NaturalSpline(YValues, Type);
                }

                if (yCoef == null)
                {
                    return;
                }

                for (var i = 0; i < PointsCount; i++)
                {
                    var x = xValues[i];
                    var y = YValues[i];
                    bool isLastData = i == PointsCount - 1;

                    var nextX = !isLastData ? xValues[i + 1] : double.NaN;
                    var nextY = !isLastData ? YValues[i + 1] : double.NaN;

                    List<double>? controlPoints;

                    if (isLastData)
                    {
                        controlPoints = new List<double>() { double.NaN, double.NaN, double.NaN, double.NaN };
                    }
                    else if (dx != null && Type == SplineType.Monotonic && dx.Length > 0)
                    {
                        controlPoints = CalculateControlPoints(x, y, nextX, nextY, yCoef[i], yCoef[i + 1], dx[i]);
                    }
                    else if (Type == SplineType.Cardinal)
                    {
                        controlPoints = CalculateControlPoints(x, y, nextX, nextY, yCoef[i], yCoef[i + 1]);
                    }
                    else
                    {
                        controlPoints = CalculateControlPoints(YValues, yCoef[i], yCoef[i + 1], i);
                    }

                    if (controlPoints != null)
                    {
                        if (i < Segments.Count)
                        {
                            Segments[i].SetData(new[] { x, y, controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3], nextX, nextY });
                        }
                        else
                        {
                            CreateSegment(seriesView, new[] { x, y, controlPoints[0], controlPoints[1], controlPoints[2], controlPoints[3], nextX, nextY }, i);
                        }
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

                SplineSegment? startSegment = null;
                SplineSegment? endSegment = null;
                var seriesClipRect = AreaBounds;
                point.X = point.X - ((float)seriesClipRect.Left);
                point.Y = point.Y - ((float)seriesClipRect.Top);

                if (TooltipDataPointIndex == 0)
                {
                    startSegment = Segments[TooltipDataPointIndex] as SplineSegment;
                }
                else if (TooltipDataPointIndex == PointsCount - 1)
                {
                    startSegment = Segments[TooltipDataPointIndex - 1] as SplineSegment;
                }
                else
                {
                    startSegment = Segments[TooltipDataPointIndex - 1] as SplineSegment;
                    endSegment = Segments[TooltipDataPointIndex] as SplineSegment;
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

        /// <summary>
        /// Add the <see cref="SplineSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="index">The index value.</param>
		/// <param name="seriesView">The seriesview.</param>
        private void CreateSegment(SeriesView seriesView, double[] values, int index)
        {
            var segment = CreateSegment() as SplineSegment;
            if (segment != null)
            {
                segment.Series = this;
                segment.SeriesView = seriesView;
                segment.Index = index;
                segment.SetData(values);
                Segments.Add(segment);

                if (OldSegments != null && OldSegments.Count > 0 && OldSegments.Count > index)
                {
                    SplineSegment? oldSegment = OldSegments[index] as SplineSegment;

                    if (oldSegment != null)
                        segment.SetPreviousData(new[] { oldSegment.X1, oldSegment.Y1, oldSegment.X2, oldSegment.Y2, oldSegment.StartControlX, oldSegment.StartControlY, oldSegment.EndControlX, oldSegment.EndControlY });
                }
            }
        }

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            return DataLabelSettings.GetLabelPositionForContinuousSeries(this, dataLabel.Index, labelSize, labelPosition, padding);
        }

        #endregion

        #region Private Methods
        private static void OnSplineTypeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as SplineSeries;
            if (series != null)
            {
                series.SegmentsCreated = false;
                series.ScheduleUpdateChart();
            }
        }

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
    }
}

