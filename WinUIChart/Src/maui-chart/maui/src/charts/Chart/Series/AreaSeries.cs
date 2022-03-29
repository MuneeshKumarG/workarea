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
    ///               <chart:AreaSeries
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
    ///     AreaSeries series = new AreaSeries();
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
    /// <seealso cref="AreaSegment"/>
    /// <seealso cref="SplineAreaSeries"/>
    public partial class AreaSeries : XYDataSeries
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty StrokeProperty =
                BindableProperty.Create(
                nameof(Stroke),
                typeof(Brush),
                typeof(ScatterSeries),
                null,
                BindingMode.Default,
                null,
                OnStrokeChanged);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty StrokeDashArrayProperty =
            BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(AreaSeries), null, BindingMode.Default, null, OnStrokeDashArrayPropertyChanged);

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
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        #endregion

        #region Methods

        #region Public Override Methods

        /// <inheritdoc/>
        public override int GetDataPointIndex(float pointX, float pointY)
        {
            if (ActualXAxis != null && ActualYAxis != null)
            {
                RectF seriesBounds = AreaBounds;
                float xPos = pointX - seriesBounds.Left;
                float yPos = pointY - seriesBounds.Top;
                for (int i = 0; i < PointsCount; i++)
                {
                    var xValues = GetXValues();

                    if (xValues == null) return -1;

                    var xval = xValues[i];
                    var yval = YValues[i];
                    float xPoint = TransformToVisibleX(xval, yval);
                    float yPoint = TransformToVisibleY(xval, yval);
                    if (ChartSegment.IsRectContains(xPoint, yPoint, xPos, yPos, (float)StrokeWidth))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        ///
        /// </summary>
        protected override ChartSegment CreateSegment()
        {
            return new AreaSegment();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Creates the segments of <see cref="AreaSeries"/>.
        /// </summary>
        internal override void GenerateSegments(SeriesView seriesView)
        {
            var actualXValues = GetXValues();
            if (actualXValues == null)
            {
                return;
            }

            List<double>? xValues = null, yValues = null;
            for (int i = 0; i < PointsCount; i++)
            {
                if (!double.IsNaN(YValues[i]))
                {
                    if (xValues == null)
                    {
                        xValues = new List<double>();
                        yValues = new List<double>();
                    }

                    xValues.Add(actualXValues[i]);
                    yValues?.Add(YValues[i]);
                }

                if (double.IsNaN(YValues[i]) || i == PointsCount - 1)
                {
                    if (xValues != null)
                    {
                        var segment = CreateSegment() as AreaSegment;
                        if (segment != null)
                        {
                            segment.Series = this;
                            segment.SeriesView = seriesView;
                            if (yValues != null)
                                segment.SetData(xValues, yValues);
                            Segments.Add(segment);

                            if (OldSegments != null && OldSegments.Count > 0)
                            {
                                AreaSegment? oldSegment = OldSegments[0] as AreaSegment;

                                if (oldSegment != null)
                                {
                                    segment.PreviousFillPoints = oldSegment.FillPoints;
                                    segment.PreviousStrokePoints = oldSegment.StrokePoints;
                                }
                            }
                        }

                        yValues = xValues = null;
                    }

                    if (double.IsNaN(YValues[i]))
                    {
                        xValues = new List<double> { actualXValues[i] };
                        yValues = new List<double> { YValues[i] };
                        var segment = (AreaSegment)CreateSegment();
                        segment.Series = this;
                        segment.SetData(xValues, yValues);
                        yValues = xValues = null;
                    }
                }
            }
        }

        internal override void SetDashArray(ChartSegment segment)
        {
            segment.StrokeDashArray = StrokeDashArray;
        }

        internal override void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        internal override void UpdateRange()
        {
            double yStart = YRange.Start;
            double yEnd = YRange.End;

            if (yStart > 0)
            {
                yStart = 0;
            }

            if (yEnd < 0)
            {
                yEnd = 0;
            }

            YRange = new DoubleRange(yStart, yEnd);
            base.UpdateRange();
        }

        internal override bool IsIndividualSegment()
        {
            return false;
        }

        internal override bool SeriesContainsPoint(PointF point)
        {
            //Todo: Need to implement the get tooltip index from FindNearestPoint method.

            return base.SeriesContainsPoint(point);
        }

        internal override PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            return DataLabelSettings.GetLabelPositionForAreaSeries(this, dataLabel, labelSize, labelPosition, padding);
        }

        internal override void DrawDataLabels(ICanvas canvas)
        {
            var dataLabeSettings = DataLabelSettings;

            if (dataLabeSettings == null || Segments == null || Segments.Count <= 0) return;

            ChartDataLabelStyle labelStyle = DataLabelSettings.LabelStyle;
            AreaSegment? datalabel = Segments[0] as AreaSegment;

            if (datalabel == null || datalabel.XValues == null || datalabel.YValues == null) return;

            for (int i = 0; i < PointsCount; i++)
            {
                double x = datalabel.XValues[i], y = datalabel.YValues[i];

                if (double.IsNaN(y)) continue;

                CalculateDataPointPosition(i, ref x, ref y);
                PointF labelPoint = new PointF((float)x, (float)y);
                datalabel.DataLabel = dataLabeSettings.GetLabelContent(datalabel.YValues[i]);
                datalabel.LabelPositionPoint = dataLabeSettings.CalculateDataLabelPoint(this, datalabel, labelPoint, labelStyle);
                UpdateDataLabelAppearance(canvas, datalabel, dataLabeSettings, labelStyle);
            }
        }

        #endregion

        #region Private Methods

        private static void OnStrokeChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as ScatterSeries;
            if (series != null)
            {
                series.UpdateStrokeColor();
                series.InvalidateSeries();
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
