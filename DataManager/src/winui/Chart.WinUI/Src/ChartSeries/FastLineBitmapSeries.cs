﻿using System;
using System.Collections.Generic;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="FastLineBitmapSeries"/> is a special kind of line series that can render a collection with a large number of data points.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="FastLineBitmapSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XyDataSeries.StrokeWidth"/>, <see cref="FastLineBitmapSeries.StrokeDashArray"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="FastLineBitmapSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="FastLineBitmapSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
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
    ///           <chart:FastLineBitmapSeries ItemsSource="{Binding Data}"
    ///                                       XBindingPath="XValue"
    ///                                       YBindingPath="YValue"/>
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
    ///     FastLineBitmapSeries series = new FastLineBitmapSeries();
    ///     series.ItemsSource = viewModel.Data;
    ///     series.XBindingPath = "XValue";
    ///     series.YBindingPath = "YValue";
    ///     chart.Series.Add(series);
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
    ///        Data.Add(new Model() { XValue = 10, YValue = 100 });
    ///        Data.Add(new Model() { XValue = 20, YValue = 150 });
    ///        Data.Add(new Model() { XValue = 30, YValue = 110 });
    ///        Data.Add(new Model() { XValue = 40, YValue = 230 });
    ///     }
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    /// <seealso cref="FastLineBitmapSegment"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class FastLineBitmapSeries : XyDataSeries
    {
        #region Dependency Property Registration
        


        /// <summary>
        /// Identifies the EnableAntiAliasing dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for EnableAntiAliasing dependency property.
        /// </value> 
        public static readonly DependencyProperty EnableAntiAliasingProperty =
            DependencyProperty.Register(nameof(EnableAntiAliasing), typeof(bool), typeof(FastLineBitmapSeries),
            new PropertyMetadata(false, OnSeriesPropertyChanged));

        /// <summary>
        /// Identifies the StrokeDashArray dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for StrokeDashArray dependency property.
        /// </value> 
        public static readonly DependencyProperty StrokeDashArrayProperty =
            DependencyProperty.Register(nameof(StrokeDashArray), typeof(DoubleCollection),
            typeof(FastLineBitmapSeries), new PropertyMetadata(null, OnSeriesPropertyChanged));

        #endregion

        #region Fields

        #region Private Fields

        private IList<double> xValues;

        private Point hitPoint = new Point();

        bool isAdornmentPending;

        Polygon polygon = new Polygon();

        PointCollection polygonPoints = new PointCollection();

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to enable smooth line drawing for <see cref="FastLineBitmapSeries"/>.
        /// </summary>
        /// <value> It accepts bool values and the default value is false.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:FastLineBitmapSeries ItemsSource="{Binding Data}"
        ///                                      XBindingPath="XValue"
        ///                                      YBindingPath="YValue"
        ///                                      EnableAntiAliasing = "True"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-5)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     FastLineBitmapSeries series = new FastLineBitmapSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           EnableAntiAliasing = true,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool EnableAntiAliasing
        {
            get { return (bool)GetValue(EnableAntiAliasingProperty); }
            set { SetValue(EnableAntiAliasingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the stroke dash array to customize the appearance of a line stroke.
        /// </summary>
        /// <value>It accepts the <see cref="DoubleCollection"/> value and the default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-6)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:FastLineBitmapSeries ItemsSource="{Binding Data}"
        ///                                      XBindingPath="XValue"
        ///                                      YBindingPath="YValue"
        ///                                      StrokeDashArray="5,3" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-7)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     DoubleCollection doubleCollection = new DoubleCollection();
        ///     doubleCollection.Add(5);
        ///     doubleCollection.Add(3);
        ///     FastLineBitmapSeries series = new FastLineBitmapSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           StrokeDashArray = doubleCollection,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }

        #endregion

        #region Protected Internal Override Properties

        /// <inheritdoc/>
        internal override bool IsBitmapSeries
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Private Properties

        private FastLineBitmapSegment Segment { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Used to create the segment of <see cref="FastLineBitmapSeries"/>.
        /// </summary>
        internal override void GenerateSegments()
        {
            var isGrouped = ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).ArrangeByIndex;

            if (isGrouped)
                xValues = GroupedXValuesIndexes;
            else
                xValues = (ActualXValues is IList<double>) ? ActualXValues as IList<double> : GetXValues();

            if (isGrouped)
            {
                Segments.Clear();
                Adornments.Clear();

                if (Segment == null || Segments.Count == 0)
                {
                    FastLineBitmapSegment segment = CreateSegment() as FastLineBitmapSegment;
                    if (segment != null)
                    {
                        segment.Series = this;
                        if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                            segment.Item = GroupedActualData;
                        else
                            segment.Item = ActualData;
                        segment.SetData(xValues, GroupedSeriesYValues[0]);
                        Segment = segment;
                        Segments.Add(segment);
                    }
                }
            }
            else
            {
                ClearUnUsedAdornments(this.PointsCount);
                if (Segment == null || Segments.Count == 0)
                {
                    FastLineBitmapSegment segment = CreateSegment() as FastLineBitmapSegment;
                    if (segment != null)
                    {
                        segment.Series = this;
                        if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                            segment.Item = GroupedActualData;
                        else
                            segment.Item = ActualData;
                        segment.SetData(xValues, YValues);
                        Segment = segment;
                        Segments.Add(segment);
                    }
                }
                else if (ActualXValues != null)
                {
                    Segment.SetData(xValues, YValues);
                    Segment.Item = ActualData;
                }
            }

            isAdornmentPending = true;
        }

        #endregion

        #region Internal Override Methods

        /// <summary>
        /// Method used to return the hittest series while mouse action.
        /// </summary>
        /// <returns></returns>
        internal override bool IsHitTestSeries()
        {
            var point = Chart.adorningCanvasPoint;

            int low = 0;
            int high = PointsCount - 1;
            var xValues = GetXValues();
            var yValues = YValues;
            double xValue = ActualXAxis.PointToValue(point.X, point.Y);

            // Binary search algorithm
            while (low <= high)
            {
                int mid = (low + high) / 2;
                if (xValue < xValues[mid])
                    high = mid - 1;
                else if (xValue > xValues[mid])
                    low = mid + 1;
                else if (xValue == xValues[mid])
                {
                    return false;
                }
            }

            if (high > -1 && low > -1 && high < xValues.Count && low < xValues.Count)
            {
                double x1 = ActualXAxis.ValueToPoint(xValues[high]);
                double y1 = ActualYAxis.ValueToPoint(yValues[high]);

                double x2 = ActualXAxis.ValueToPoint(xValues[low]);
                double y2 = ActualYAxis.ValueToPoint(yValues[low]);

                if (Math.Abs(x1 - x2) > 2)
                {
                    double thickness = StrokeWidth / 2;
                    polygon.Points = GetPolygonPoints(x1, y1, x2, y2, thickness, thickness);

                    if (PointInsidePolygon(polygon, point.X, point.Y))
                        return true;
                }
                else if (Math.Round(y1) == Math.Round(point.Y) || Math.Round(y2) == Math.Round(point.Y))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// This method used to gets the chart data point at a position.
        /// </summary>
        /// <param name="mousePos"></param>
        /// <returns></returns>
        internal override ChartDataPointInfo GetDataPoint(Point mousePos)
        {
            Rect rect;
            int startIndex, endIndex;
            List<int> hitIndexes = new List<int>();
            IList<double> xValues = GetXValues();

            CalculateHittestRect(mousePos, out startIndex, out endIndex, out rect);

            for (int i = startIndex; i <= endIndex; i++)
            {
                hitPoint.X = IsIndexed ? i : xValues[i];
                hitPoint.Y = YValues[i];

                if (rect.Contains(hitPoint))
                    hitIndexes.Add(i);
            }

            if (hitIndexes.Count > 0)
            {
                int i = hitIndexes[hitIndexes.Count / 2];
                hitIndexes = null;

                dataPoint = new ChartDataPointInfo();
                dataPoint.Index = i;
                dataPoint.XData = xValues[i];
                dataPoint.YData = YValues[i];
                dataPoint.Series = this;

                if (i > -1 && ActualData.Count > i)
                    dataPoint.Item = ActualData[i];

                return dataPoint;
            }
            else
                return dataPoint;
        }

        internal override void UpdateTooltip(object originalSource)
        {
            if (EnableTooltip)
            {
                ChartDataPointInfo dataPoint = new ChartDataPointInfo();
                int index = ChartExtensionUtils.GetAdornmentIndex(originalSource);
                if (index > -1)
                {
                    dataPoint.Index = index;
                    if (xValues.Count > index)
                        dataPoint.XData = xValues[index];
                    if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed
                            && GroupedSeriesYValues[0].Count > index)
                        dataPoint.YData = GroupedSeriesYValues[0][index];
                    else if (YValues.Count > index)
                        dataPoint.YData = YValues[index];
                    dataPoint.Series = this;
                    if (ActualData.Count > index)
                        dataPoint.Item = ActualData[index];
                    UpdateSeriesTooltip(dataPoint);
                }
            }
        }

        internal override Point GetDataPointPosition(ChartTooltip tooltip)
        {
            ChartDataPointInfo chartPointinfo = ToolTipTag as ChartDataPointInfo;
            Point newPosition = new Point();
            Point point = ChartTransformer.TransformToVisible(chartPointinfo.XData, chartPointinfo.YData);
            newPosition.X = point.X + ActualArea.SeriesClipRect.Left;
            newPosition.Y = point.Y + ActualArea.SeriesClipRect.Top;
            return newPosition;
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            return new FastLineBitmapSegment();
        }

        /// <summary>
        /// Invoked when VisibleRange property changed.
        /// </summary>
        /// <param name="e"><see cref="VisibleRangeChangedEventArgs"/> that contains the event data.</param>
        internal override void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
        {
            if (AdornmentsInfo != null && ShowDataLabels && isAdornmentPending)
            {
                List<double> xValues = new List<double>();
                if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                    xValues = GroupedXValuesIndexes;
                else
                    xValues = GetXValues();

                if (xValues != null && ActualXAxis != null
                    && !ActualXAxis.VisibleRange.IsEmpty)
                {
                    double start = ActualXAxis.VisibleRange.Start;
                    double end = ActualXAxis.VisibleRange.End;

                    for (int i = 0; i < PointsCount; i++)
                    {
                        double x, y;
                        x = xValues[i];
                        y = YValues[i];

                        double edgeValue = x;

                        if (edgeValue >= start && edgeValue <= end)
                        {
                            if (i < Adornments.Count)
                            {
                                Adornments[i].SetData(x, y, x, y);
                            }
                            else
                            {
                                Adornments.Add(this.CreateAdornment(this, x, y, x, y));
                            }

                            Adornments[i].Item = ActualData[i];
                        }
                    }

                    isAdornmentPending = false;
                }
            }
        }

        /// <summary>
        /// Called when <see cref="ChartSeries.ItemsSource"/> property changed.
        /// </summary>
        /// <param name="oldValue">ItemsSource old value.</param>
        /// <param name="newValue">ItemsSource new value</param>
        /// <seealso cref="ChartSeries.XBindingPath"/>
        /// <seealso cref="XyDataSeries.YBindingPath"/>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            Segment = null;
            base.OnDataSourceChanged(oldValue, newValue);
        }

        /// <inheritdoc/>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            Canvas canvas = ActualArea.GetAdorningCanvas();
            mousePosition = e.GetCurrentPoint(canvas).Position;
            if (e.Pointer.PointerDeviceType != PointerDeviceType.Touch)
                UpdateTooltip(e.OriginalSource);
        }

#if NETFX_CORE
        /// <inheritdoc/>
        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Touch)
                UpdateTooltip(e.OriginalSource);
            base.OnTapped(e);
        }
#endif
        #endregion
        
        #region Private Static Methods

        /// <summary>
        /// Method used to check the point within the polygon or not.
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="pointX"></param>
        /// <param name="pointY"></param>
        private static bool PointInsidePolygon(Polygon polygon, double pointX, double pointY)
        {
            int i, j;
            var inside = false;
            var points = polygon.Points;

            for (i = 0, j = points.Count - 1; i < points.Count; j = i++)
            {
                if (((points[i].Y > pointY) != (points[j].Y > pointY)) &&
                    (pointX < (points[j].X - points[i].X) * (pointY - points[i].Y) /
                    (points[j].Y - points[i].Y) + points[i].X)) inside = !inside;
            }

            return inside;
        }

        private static void OnSeriesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FastLineBitmapSeries).ScheduleUpdateChart();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method used to get polygon points.
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="leftThickness"></param>
        /// <param name="rightThickness"></param>
        /// <returns></returns>
        private PointCollection GetPolygonPoints(double x1, double y1, double x2, double y2,
                                   double leftThickness, double rightThickness)
        {
            polygonPoints.Clear();
            var dx = x2 - x1;
            var dy = y2 - y1;
            var radian = Math.Atan2(dy, dx);
            var cos = Math.Cos(-radian);
            var sin = Math.Sin(-radian);
            var x11 = (x1 * cos) - (y1 * sin);
            var y11 = (x1 * sin) + (y1 * cos);
            var x12 = (x2 * cos) - (y2 * sin);
            var y12 = (x2 * sin) + (y2 * cos);
            cos = Math.Cos(radian);
            sin = Math.Sin(radian);

            var leftTopX = (x11 * cos) - ((y11 + leftThickness) * sin);
            var leftTopY = (x11 * sin) + ((y11 + leftThickness) * cos);
            var rightTopX = (x12 * cos) - ((y12 + leftThickness) * sin);
            var rightTopY = (x12 * sin) + ((y12 + leftThickness) * cos);
            var leftBottomX = (x11 * cos) - ((y11 - rightThickness) * sin);
            var leftBottomY = (x11 * sin) + ((y11 - rightThickness) * cos);
            var rightBottomX = (x12 * cos) - ((y12 - rightThickness) * sin);
            var rightBottomY = (x12 * sin) + ((y12 - rightThickness) * cos);

            polygonPoints.Add(new Point(leftTopX, leftTopY));
            polygonPoints.Add(new Point(rightTopX, rightTopY));
            polygonPoints.Add(new Point(rightBottomX, rightBottomY));
            polygonPoints.Add(new Point(leftBottomX, leftBottomY));
            polygonPoints.Add(new Point(leftTopX, leftTopY));

            return polygonPoints;
        }

        #endregion

        #endregion
    }
}
