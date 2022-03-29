using System;
using System.Collections.Generic;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Foundation;
using SelectionStyle = Syncfusion.UI.Xaml.Charts.SelectionType;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a special kind of stepline series which uses writeablebitmap for rendering chart points. FastStepLineBitmapSeries allows to render a collection with large number of data points.
    /// </summary>
    /// <remarks>
    /// FastStepLineBitmapSeries renders large quantity of data in fraction of milliseconds using writeablebitmap. 
    /// </remarks>
    /// <example>
    /// # [MainWindow.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfChart>
    ///
    ///           <chart:SfChart.PrimaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfChart.NumericalAxis>
    ///
    ///           <chart:SfChart.SecondaryAxis>
    ///               <chart:NumericalAxis/>
    ///           </chart:SfChart.SecondaryAxis>
    ///
    ///           <chart:FastStepLineBitmapSeries
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
    ///     NumericalAxis primaryAxis = new NumericalAxis();
    ///     NumericalAxis secondaryAxis = new NumericalAxis();
    ///     
    ///     chart.PrimaryAxis = primaryAxis;
    ///     chart.SecondaryAxis = secondaryAxis;
    ///     
    ///     FastStepLineBitmapSeries series = new FastStepLineBitmapSeries();
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
        /// Gets or sets a value that indicates whether to enable anti-aliasing for <see cref="FastStepLineBitmapSeries"/>, to draw smooth edges.
        /// </summary>
        /// <value>
        /// Default value is false.
        /// </value>
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

        /// <inheritdoc/>
        internal override bool IsLinear
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
        public override void CreateSegments()
        {
            bool isGrouping = this.ActualXAxis is CategoryAxis ? (this.ActualXAxis as CategoryAxis).IsIndexed : true;
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
                ClearUnUsedAdornments(DataCount);
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
            if (ShowTooltip)
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
        protected override ChartSegment CreateSegment()
        {
            return new FastStepLineBitmapSegment();
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Called when pointer or mouse move on chart area.
        /// </summary>
        /// <param name="e">Event args that contains the event data.</param>
        protected override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            Canvas canvas = ActualArea.GetAdorningCanvas();

            mousePos = e.GetCurrentPoint(canvas).Position;
            if (e.Pointer.PointerDeviceType != PointerDeviceType.Touch)
                UpdateTooltip(e.OriginalSource);
        }

#if NETFX_CORE
        /// <summary>
        /// Used to update the series tooltip when tapping on the series segments.
        /// </summary>
        /// <param name="e"><see cref="TappedRoutedEventArgs"/> that contains the event data.</param>
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
        protected override void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
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
                    double xBase = ActualXAxis.IsLogarithmic ? (ActualXAxis as LogarithmicAxis).LogarithmicBase : 1;
                    double start = ActualXAxis.VisibleRange.Start;
                    double end = ActualXAxis.VisibleRange.End;

                    if (isGrouped)
                    {
                        for (int i = 0; i < xValues.Count; i++)
                        {
                            if (i < xValues.Count)
                            {
                                double x = xValues[i];
                                double actualX = ActualXAxis.IsLogarithmic ? Math.Log(x, xBase) : x;

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
                        for (int i = 0; i < DataCount; i++)
                        {
                            double x = xValues[i];
                            double actualX = ActualXAxis.IsLogarithmic ? Math.Log(x, xBase) : x;

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
                }

                isAdornmentPending = false;
            }
        }

        #endregion

        #endregion        
    }
}