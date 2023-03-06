using System;
using System.Collections.Generic;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// The <see cref="FastStepLineBitmapSeries"/> is a special kind of step line series that can render a collection with a large number of data points.
    /// </summary>
    /// <remarks>
    /// <para>To render a series, create an instance of <see cref="FastStepLineBitmapSeries"/> class, and add it to the <see cref="SfCartesianChart.Series"/> collection.</para>
    /// 
    /// <para>It provides options for <see cref="ChartSeries.Fill"/>, <see cref="ChartSeries.PaletteBrushes"/>, <see cref="XyDataSeries.StrokeThickness"/>, and opacity to customize the appearance.</para>
    /// 
    /// <para> <b>EnableTooltip - </b> A tooltip displays information while tapping or mouse hovering over a segment. To display the tooltip on chart, need to set the <see cref="ChartSeries.EnableTooltip"/> property as <b>true</b> in <see cref="FastStepLineBitmapSeries"/> class, and also refer <seealso cref="ChartBase.TooltipBehavior"/> property.</para>
    /// <para> <b>Data Label - </b> Data labels are used to display values related to a chart segment. To render the data labels, you need to set the <see cref="DataMarkerSeries.ShowDataLabels"/> property as <b>true</b> in <see cref="FastStepLineBitmapSeries"/> class. To customize the chart data labels alignment, placement, and label styles, you need to create an instance of <see cref="CartesianDataLabelSettings"/> and set to the <see cref="CartesianSeries.DataLabelSettings"/> property. </para>
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
    ///           <chart:SfCartesianChart.Series>
    ///               <chart:FastStepLineBitmapSeries
    ///                   ItemsSource="{Binding Data}"
    ///                   XBindingPath="XValue"
    ///                   YBindingPath="YValue"/>
    ///           </chart:SfCartesianChart.Series>  
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
    ///     FastLineBitmapSeries series = new FastStepLineBitmapSeries();
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
    /// <seealso cref="FastStepLineBitmapSegment"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class FastStepLineBitmapSeries : XyDataSeries
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the EnableAntiAliasing dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for EnableAntiAliasing dependency property.
        /// </value> 
        public static readonly DependencyProperty EnableAntiAliasingProperty =
            DependencyProperty.Register(nameof(EnableAntiAliasing), typeof(bool), typeof(FastStepLineBitmapSeries),
            new PropertyMetadata(false));

        #endregion

        #region Fields

        #region Private Fields

        private IList<double> xValues;

        private Point hitPoint = new Point();
        
        bool isAdornmentPending;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to enable smooth line drawing for <see cref="FastStepLineBitmapSeries"/>.
        /// </summary>
        /// <value> It accepts bool values and the default value is false.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-4)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:FastStepLineBitmapSeries ItemsSource="{Binding Data}"
        ///                                          XBindingPath="XValue"
        ///                                          YBindingPath="YValue"
        ///                                          EnableAntiAliasing = "True"/>
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
        ///     FastStepLineBitmapSeries series = new FastStepLineBitmapSeries()
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

        private FastStepLineBitmapSegment Segment { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Used to create the segment of <see cref="FastStepLineBitmapSeries"/>.
        /// </summary>
        internal override void GenerateSegments()
        {
            bool isGrouping = this.ActualXAxis is CategoryAxis ? (this.ActualXAxis as CategoryAxis).ArrangeByIndex : true;
            if (!isGrouping)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();
            if (!isGrouping)
            {
                Segments.Clear();
                Adornments.Clear();
                if (Segment == null || Segments.Count == 0)
                {
                    CreateSegment(xValues, GroupedSeriesYValues[0]);
                }
            }
            else
            {
                ClearUnUsedAdornments(PointsCount);
                if (Segment == null || Segments.Count == 0)
                {
                    CreateSegment(xValues, YValues);
                }
                else if (ActualXValues != null)
                {
                    Segment.SetData(xValues, YValues);
                    Segment.Item = ActualData;
                }
            }

            isAdornmentPending = true;
        }

        /// <summary>
        /// Add the <see cref="FastStepLineBitmapSegment"/> into the Segments collection.
        /// </summary>
        /// <param name="xValues">The xValues.</param>
        /// <param name="yValues">The yValues.</param>
        private void CreateSegment(IList<double> xValues, IList<double> yValues)
        {
            Segment = CreateSegment() as FastStepLineBitmapSegment;
            if (Segment != null)
            { 
                Segment.Series = this;
                Segment.SetData(xValues, yValues);
                Segment.Item = ActualData;
                Segments.Add(Segment);
            }
        }

        #endregion

        #region Internal Override Methods

        internal override void UpdateTooltip(object originalSource)
        {
            if (EnableTooltip)
            {
                ChartDataPointInfo dataPoint = new ChartDataPointInfo();
                int index = ChartExtensionUtils.GetAdornmentIndex(originalSource);
                if (index > -1)
                {
                    dataPoint.Index = index;
                    if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                    {
                        if (GroupedXValuesIndexes.Count > index)
                            dataPoint.XData = GroupedXValuesIndexes[index];
                        if (GroupedSeriesYValues[0].Count > index)
                            dataPoint.YData = GroupedSeriesYValues[0][index];
                    }
                    else
                    {
                        if (xValues.Count > index)
                            dataPoint.XData = xValues[index];
                        if (YValues.Count > index)
                            dataPoint.YData = YValues[index];
                    }

                    dataPoint.Series = this;
                    if (ActualData.Count > index)
                        dataPoint.Item = ActualData[index];
                    UpdateSeriesTooltip(dataPoint);
                }
            }
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

        #region Protected Internal Override MEthods

        /// <inheritdoc/>
        internal override ChartSegment CreateSegment()
        {
            return new FastStepLineBitmapSegment();
        }

        #endregion

        #region Protected Override Methods

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
        }
#endif

        /// <summary>
        /// Invoked when VisibleRange property changed.
        /// </summary>
        /// <param name="e"><see cref="VisibleRangeChangedEventArgs"/> that contains the event data.</param>
        internal override void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
        {
            if (AdornmentsInfo != null && ShowDataLabels && isAdornmentPending)
            {
                List<double> xValues = null;
                var isGrouped = ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed;

                if (isGrouped)
                    xValues = GroupedXValuesIndexes;
                else
                    xValues = GetXValues();

                if (xValues != null && ActualXAxis != null
                    && !ActualXAxis.VisibleRange.IsEmpty)
                {
                    double start = ActualXAxis.VisibleRange.Start;
                    double end = ActualXAxis.VisibleRange.End;

                    if (isGrouped)
                    {
                        for (int i = 0; i < xValues.Count; i++)
                        {
                            if (i < xValues.Count)
                            {
                                double x = xValues[i];
                                double actualX = x;

                                if (actualX >= start && actualX <= end)
                                {
                                    double y = GroupedSeriesYValues[0][i];

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
                        }
                    }
                    else
                    {
                        for (int i = 0; i < PointsCount; i++)
                        {
                            double x = xValues[i];
                            double actualX = x;

                            if (actualX >= start && actualX <= end)
                            {
                                double y = YValues[i];

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
                    }

                    isAdornmentPending = false;
                }
            }
        }

        #endregion

        #endregion        
    }
}