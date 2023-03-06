using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CartesianSeries : ChartSeries
    {
        #region Dependency Property Registration
        /// <summary>
        /// The DependencyProperty for <see cref="CartesianDataLabelSettings"/> property.
        /// </summary>
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(CartesianDataLabelSettings), typeof(CartesianSeries),
          new PropertyMetadata(null, OnAdornmentsInfoChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ShowTrackballLabel"/> property.
        /// </summary>
        public static readonly DependencyProperty ShowTrackballLabelProperty =
            DependencyProperty.Register(nameof(ShowTrackballLabel), typeof(bool), typeof(CartesianSeries), new PropertyMetadata(true));

        /// <summary>
        /// The DependencyProperty for <see cref="XAxisName"/> property.
        /// </summary>
        public static readonly DependencyProperty XAxisNameProperty =
            DependencyProperty.Register(nameof(XAxisName), typeof(string), typeof(CartesianSeries),
            new PropertyMetadata(null, OnXAxisNameChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="YAxisName"/> property.
        /// </summary>
        public static readonly DependencyProperty YAxisNameProperty =
            DependencyProperty.Register(nameof(YAxisName), typeof(string), typeof(CartesianSeries),
            new PropertyMetadata(null, OnYAxisNameChanged));
        #endregion

        #region Constructor

        /// <summary>
        /// Called when instance created for CartesianSeries.
        /// </summary>
        public CartesianSeries()
        {
            DataLabelSettings = new CartesianDataLabelSettings();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to customize the appearance of the displaying data labels in the cartesian series.
        /// </summary>
        /// <remarks> This allows us to change the look of the displaying labels' content, shapes, and connector lines at the data point.</remarks>
        /// <value>
        /// It takes the <see cref="CartesianDataLabelSettings"/>.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:AreaSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue">
        ///              <chart:AreaSeries.DataLabelSettings>
        ///                  <chart:CartesianDataLabelSettings />
        ///              </chart:AreaSeries.DataLabelSettings>
        ///          </chart:AreaSeries>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     AreaSeries series = new AreaSeries();
        ///     series.ItemsSource = viewModel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     series.DataLabelSettings = new CartesianDataLabelSettings();
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public CartesianDataLabelSettings DataLabelSettings
        {
            get
            {
                return (CartesianDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }

            set
            {
                SetValue(DataLabelSettingsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the (horizontal) axis in the <see cref="SfCartesianChart.XAxes"/> collection which is used to plot the series with particular axis.
        /// </summary>
        /// <example>
        /// # [Xaml](#tab/tabid-3)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis x:Name="XAxis1"/>
        ///        <chart:NumericalAxis x:Name="XAxis2"/>
        ///    </chart:SfCartesianChart.XAxes>
        ///    
        ///    <chart:SfCartesianChart.YAxes>
        ///       <chart:NumericalAxis />
        ///   </chart:SfCartesianChart.YAxes>
        ///
        ///          <chart:SplineSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue" />
        ///          <chart:ColumnSeries ItemsSource = "{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              XAxisName="XAxis2" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     var XAxis1 = new CategoryAxis() { Name = "XAXis1" };
        ///     var XAxis2 = new NumericalAxis() { Name = "XAXis2" }; 
        ///     chart.XAxes.Add(XAxis1);
        ///     chart.XAxes.Add(XAxis2);
        ///     var YAxis = new NumericalAxis();
        ///     chart.YAxes.Add(YAxis);
        ///
        ///     SplineSeries splineSeries = new SplineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           XAxisName = "XAXis2",
        ///     };
        ///     
        ///     chart.Series.Add(splineSeries);
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string XAxisName
        {
            get { return (string)GetValue(XAxisNameProperty); }
            set { SetValue(XAxisNameProperty, value); }
        }

        /// <summary>
        /// Gets or sets the name of the (vertical) axis in the <see cref="SfCartesianChart.YAxes"/> collection which is used to plot the series with particular axis.
        /// </summary>
        /// <value>It takes the string value and its default value is null.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <chart:SfCartesianChart.XAxes>
        ///        <chart:CategoryAxis />
        ///    </chart:SfCartesianChart.XAxes>
        ///    
        ///    <chart:SfCartesianChart.YAxes>
        ///       <chart:NumericalAxis x:Name="YAxis1"/>
        ///       <chart:NumericalAxis x:Name="YAxis2"/>
        ///   </chart:SfCartesianChart.YAxes>
        ///
        ///          <chart:SplineSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue" />
        ///          <chart:ColumnSeries ItemsSource = "{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              YAxisName="YAxis2" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     var XAxis = new CategoryAxis();
        ///     chart.XAxes.Add(XAxis);
        ///     var YAxis1 = new NumericalAxis(){Name = "YAXis1"};
        ///     var YAxis2 = new NumericalAxis(){Name = "YAXis2"};
        ///     chart.YAxes.Add(YAxis1);
        ///     chart.YAxes.Add(YAxis2);
        ///
        ///     SplineSeries splineSeries = new SplineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           YAxisName = "YAXis2",
        ///     };
        ///     
        ///     chart.Series.Add(splineSeries);
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public string YAxisName
        {
            get { return (string)GetValue(YAxisNameProperty); }
            set { SetValue(YAxisNameProperty, value); }
        }

        internal override ChartDataLabelSettings AdornmentsInfo
        {
            get
            {
                return (CartesianDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }
        }

        /// <summary>
        /// Gets or sets the x axis range corresponding to this series.
        /// </summary>
        internal DoubleRange VisibleXRange { get; set; }

        /// <summary>
        /// Gets or sets the y axis range corresponding to this series.
        /// </summary>
        internal DoubleRange VisibleYRange { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether to show or hide the series information when the trackball is shown.
        /// </summary>
        /// <value>It takes bool values and its default value is true.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-7)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:SplineSeries ItemsSource="{Binding Data}"
        ///                              XBindingPath="XValue"
        ///                              YBindingPath="YValue"
        ///                              ShowTrackballLabel = "True"/>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///     
        ///     SplineSeries splineSeries = new SplineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowTrackballLabel = true,
        ///     };
        ///     
        ///     chart.Series.Add(splineSeries);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool ShowTrackballLabel
        {
            get { return (bool)GetValue(ShowTrackballLabelProperty); }
            set { SetValue(ShowTrackballLabelProperty, value); }
        }

#endregion

        #endregion

        #region Methods

        #region Internal Override Methods

        internal override void UpdateRange()
        {
            VisibleXRange = DoubleRange.Empty;
            VisibleYRange = DoubleRange.Empty;

            if (Segments == null)
            {
                return;
            }

            if (Segments.Count > 0)
            {
                foreach (ChartSegment segment in Segments)
                {
                    VisibleXRange += segment.XRange;
                    VisibleYRange += segment.YRange;
                }

                if (IsSideBySide)
                {
                    if (!SideBySideInfoRangePad.IsEmpty)
                    {
                        bool isAlterRange = ((this.ActualXAxis is NumericalAxis && (this.ActualXAxis as NumericalAxis).RangePadding == NumericalPadding.None)
                       || (this.ActualXAxis is DateTimeAxis && (this.ActualXAxis as DateTimeAxis).RangePadding == DateTimeRangePadding.None));
                        VisibleXRange = isAlterRange ? new DoubleRange(VisibleXRange.Start - SideBySideInfoRangePad.Start, VisibleXRange.End - SideBySideInfoRangePad.End)
                            : new DoubleRange(VisibleXRange.Start + SideBySideInfoRangePad.Start, VisibleXRange.End + SideBySideInfoRangePad.End);
                    }
                }
            }
            else if (this.PointsCount == 1) // WPF-16569 condition for setting the range for single data with zero segments in cartesian series
            {
                var xValues = GetXValues();
                var yValues = this.ActualSeriesYValues[0] as List<double>;
                if (xValues.Count > 0 && yValues.Count > 0)
                {
                    double xValue = xValues[0];
                    double yValue = yValues[0];

                    VisibleXRange = new DoubleRange(xValue - 1, xValue + 1);
                    VisibleYRange = new DoubleRange(yValue - 1, yValue + 1);
                }
            }

        }

        /// <summary>
        /// Update series bound
        /// </summary>
        /// <param name="size"></param>
        internal override void UpdateOnSeriesBoundChanged(Size size)
        {
            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                AdornmentsInfo.UpdateElements();
            }

            base.UpdateOnSeriesBoundChanged(size);

            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                AdornmentPresenter.Update(size);
                AdornmentPresenter.Arrange(size);
            }

            if (Segments == null)
            {
                return;
            }

        }

        /// <summary>
        /// Calculate Segments
        /// </summary>
        internal override void CalculateSegments()
        {
            base.CalculateSegments();


            // VisibleAdornments need to be cleared when segments are newly build while Zooming 
            if (VisibleAdornments.Count > 0)
            {
                VisibleAdornments.Clear();
            }
            if (PointsCount == 0)
            {
                if (AdornmentsInfo != null)
                {
                    BarLabelAlignment markerPosition = this.adornmentInfo.GetAdornmentPosition();
                    if (markerPosition == BarLabelAlignment.Middle)
                        ClearUnUsedAdornments(this.PointsCount * 4);
                    else
                        ClearUnUsedAdornments(this.PointsCount * 2);
                }
            }
        }

        internal override int GetDataPointIndex(Point point)
        {
            Canvas canvas = Chart.GetAdorningCanvas();
            double left = Chart.ActualWidth - canvas.ActualWidth;
            double top = Chart.ActualHeight - canvas.ActualHeight;

            point.X = point.X - left - Chart.SeriesClipRect.Left + Chart.Margin.Left;
            point.Y = point.Y - top - Chart.SeriesClipRect.Top + Chart.Margin.Top;
            double x, y, stackedValues;
            FindNearestChartPoint(point, out x, out y, out stackedValues);
            return GetXValues().IndexOf(x);
        }

        /// <inheritdoc/>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            if (AdornmentsInfo != null)
            {
                VisibleAdornments.Clear();
                Adornments.Clear();
                AdornmentsInfo.UpdateElements();
            }

            base.OnDataSourceChanged(oldValue, newValue);
        }

        #endregion

        #region Internal Methods

        internal override void Dispose()
        {
            base.Dispose();
        }

        internal void OnVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
        {
            OnVisibleRangeChanged(e);
        }

        #endregion

        #region Protected Virtual Methods


        /// <summary>
        /// Method implementation for add Adornments at XY.
        /// </summary>
        /// <param name="x">xvalue</param>
        /// <param name="y">yvalue</param>
        /// <param name="pointindex">index</param>
        internal virtual void AddAdornmentAtXY(double x, double y, int pointindex)
        {
            double adornposX = x, adornposY = y;

            if (pointindex < Adornments.Count)
            {
                Adornments[pointindex].SetData(x, y, adornposX, adornposY);
            }
            else
            {
                Adornments.Add(CreateAdornment(this, x, y, adornposX, adornposY));
            }

            if (pointindex < ActualData.Count)
                Adornments[pointindex].Item = ActualData[pointindex];
        }

        /// <summary>
        /// Method implementation for add AreaAdornments in Chart.
        /// </summary>
        /// <param name="values">values</param>
        internal virtual void AddAreaAdornments(params IList<double>[] values)
        {
            IList<double> yValues = values[0];
            List<double> xValues = new List<double>();
            if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();

            if (values.Length == 1)
            {
                int i;
                for (i = 0; i < PointsCount; i++)
                {
                    if (i < xValues.Count && i < yValues.Count)
                    {
                        double adornX = xValues[i];
                        double adornY = yValues[i];

                        if (i < Adornments.Count)
                        {
                            Adornments[i].SetData(xValues[i], yValues[i], adornX, adornY);
                        }
                        else
                        {
                            Adornments.Add(CreateAdornment(this, xValues[i], yValues[i], adornX, adornY));
                        }

                        Adornments[i].Item = ActualData[i];
                    }
                }
            }
        }


        /// <summary>
        /// Called when VisibleRange property changed.
        /// </summary>
        internal virtual void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
        {
        }

        /// <summary>
        /// Method implementation for add ColumnAdornments in Chart.
        /// </summary>
        /// <param name="values">values</param>
        internal virtual void AddColumnAdornments(params double[] values)
        {
            ////values[0] -->   xData
            ////values[1] -->   yData
            ////values[2] -->   xPos
            ////values[3] -->   yPos
            ////values[4] -->   data point index
            ////values[5] -->   Median value.

            double adornposX = values[2] + values[5], adornposY = values[3];
            int pointIndex = (int)values[4];
            if (pointIndex < Adornments.Count)
            {
                Adornments[pointIndex].SetData(values[0], values[1], adornposX, adornposY);
            }
            else
            {
                Adornments.Add(CreateAdornment(this, values[0], values[1], adornposX, adornposY));
            }

            {
                if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed
                    && this.GroupedActualData.Count > 0)
                    Adornments[pointIndex].Item = this.GroupedActualData[pointIndex];
                else
                    Adornments[pointIndex].Item = ActualData[pointIndex];
            }
        }

        #endregion

        #region Protected Override Methods

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            AdornmentPresenter = new ChartDataMarkerPresenter();
            AdornmentPresenter.Series = this;

            if (Chart != null && AdornmentsInfo != null && ShowDataLabels)
            {
                Chart.DataLabelPresenter.Children.Add(AdornmentPresenter);
                AdornmentsInfo.PanelChanged(AdornmentPresenter);
            }
        }

#endregion

        internal ChartAxis GetXAxisByName(string name, ObservableCollection<ChartAxis> axes)
        {
            var item = (from x in axes where x.Name == name select x).ToList();
            if (item != null && item.Count > 0)
                return item[0];

            return null;
        }

        internal RangeAxisBase GetYAxisByName(string name, ObservableCollection<RangeAxisBase> axes)
        {
            var item = (from x in axes where x.Name == name select x).ToList();
            if (item != null && item.Count > 0)
                return item[0];

            return null;
        }

        internal void ResetXAxis()
        {
            ActualXAxis = null;
            VisibleAdornments.Clear();
            if (ChartTransformer is ChartTransform.ChartCartesianTransformer tranformer)
                tranformer.XAxis = null;
            Refresh();
        }

        internal void ResetYAxis()
        {
            ActualYAxis = null;
            VisibleAdornments.Clear();
            if (ChartTransformer is ChartTransform.ChartCartesianTransformer tranformer)
                tranformer.YAxis = null;
            Refresh();
        }

#region Private Static Methods

        private static void OnXAxisNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CartesianSeries cartesianSeries && cartesianSeries.ActualArea is SfCartesianChart cartesianChart)
            {
                var oldAxis = cartesianSeries.ActualXAxis;
                if (oldAxis != null && oldAxis.RegisteredSeries.Contains(cartesianSeries))
                {
                    oldAxis.RegisteredSeries.Remove(cartesianSeries);
                }

                var newAxis = cartesianSeries.GetXAxisByName(cartesianSeries.XAxisName, cartesianChart.XAxes) ?? cartesianChart.InternalPrimaryAxis;
                cartesianSeries.ActualXAxis = newAxis;

                if (newAxis != null && !newAxis.RegisteredSeries.Contains(cartesianSeries))
                    newAxis.RegisteredSeries.Add(cartesianSeries);

                cartesianChart.ScheduleUpdate();
            }
        }

        private static void OnYAxisNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CartesianSeries cartesianSeries && cartesianSeries.ActualArea is SfCartesianChart cartesianChart)
            {
                var oldAxis = cartesianSeries.ActualYAxis;
                if (oldAxis != null && oldAxis.RegisteredSeries.Contains(cartesianSeries))
                {
                    oldAxis.RegisteredSeries.Remove(cartesianSeries);
                }

                var newAxis = cartesianSeries.GetYAxisByName(cartesianSeries.YAxisName, cartesianChart.YAxes) ?? (RangeAxisBase)cartesianChart.InternalSecondaryAxis;
                cartesianSeries.ActualYAxis = newAxis;

                if (newAxis != null && !newAxis.RegisteredSeries.Contains(cartesianSeries))
                    newAxis.RegisteredSeries.Add(cartesianSeries);

                cartesianChart.ScheduleUpdate();
            }
        }

#endregion


#endregion

        /// <summary>
        /// Method to get the visible range data points.
        /// </summary>
        /// <returns>The data points.</returns>
        /// <param name="rectangle">The Rectangle.</param>
        /// <remarks>
        /// This method will work only after render the series in visual.
        /// </remarks>
        internal List<object> GetDataPoints(Rect rectangle)
        {
            if (Chart == null || ActualXAxis == null || ActualYAxis == null)
            {
                return null;
            }
            
            rectangle.Intersect(Chart.SeriesClipRect);
            var xValues = GetXValues();
            if (xValues == null || (rectangle.Height <= 0 && rectangle.Width <= 0))
            {
                return null;
            }

            double startX = double.NaN, startY = double.NaN, endX = double.NaN, endY = double.NaN;
            ConvertRectToValue(ref startX, ref endX, ref startY, ref endY, rectangle);

            if (rectangle.Width > 0 && rectangle.Height > 0)
            {
                return GetDataPoints(startX, endX, startY, endY);
            }
            else if ((!ActualXAxis.IsVertical && rectangle.Height <= 0) || (ActualXAxis.IsVertical && rectangle.Width <= 0))
            {
                if (IsLinearData)
                {
                    int minimum = 0, maximum = xValues.Count - 1;
                    CalculateNearestIndex(ref minimum, ref maximum, xValues, startX, endX);
                    return ActualData.GetRange(minimum, (maximum - minimum) + 1);
                }
                else
                {
                    List<object> dataPoints = new List<object>();
                    for (int i = 0; i < xValues.Count; i++)
                    {
                        double value = xValues[i];
                        if (startX <= value && value <= endX)
                        {
                            dataPoints.Add(ActualData[i]);
                        }
                    }
                    
                    return dataPoints;
                }
            }
            else
            {
                return GetDataPoints(startX, endX, startY, endY, 0, xValues.Count - 1, xValues, true);
            }            
        }

        /// <summary>
        /// Method to get the data points from the given range.
        /// </summary>
        /// <param name="startX">start x</param>
        /// <param name="endX">end x</param>
        /// <param name="startY">start y</param>
        /// <param name="endY">end y</param>
        /// <returns>The data points</returns>
        /// <remarks>
        /// This method will work only after render the series in visual.
        /// </remarks>
        internal List<object> GetDataPoints(double startX, double endX, double startY, double endY)
        {
            var xValues = GetXValues();
            int minimum = 0, maximum = xValues.Count - 1;
            if (IsLinearData)
            {
                CalculateNearestIndex(ref minimum, ref maximum, xValues, startX, endX);
            }

            return GetDataPoints(startX, endX, startY, endY, minimum, maximum, xValues, IsLinearData);
        }

        internal override void SetDataLabelsVisibility(bool isShowDataLabels)
        {
            if (DataLabelSettings != null)
            {
                DataLabelSettings.Visible = isShowDataLabels;
            }
        }

        private static void CalculateNearestIndex(ref int minimum, ref int maximum, List<double> xValues, double startX, double endX)
        {
            minimum = ChartExtensionUtils.BinarySearch(xValues, startX, 0, maximum);
            maximum = ChartExtensionUtils.BinarySearch(xValues, endX, 0, maximum);
            minimum = startX <= xValues[minimum] ? minimum : minimum + 1;
            maximum = endX >= xValues[maximum] ? maximum : maximum - 1;
        }

        private void ConvertRectToValue(ref double startX, ref double endX, ref double startY, ref double endY, Rect rect)
        {
            var seriesClipRect = Chart.SeriesClipRect;
            double right = rect.X + rect.Width - seriesClipRect.Left;
            double bottom = rect.Y + rect.Height - seriesClipRect.Top;

            startX = Chart.ActualPointToValue(ActualXAxis, new Point(rect.X - seriesClipRect.Left, rect.Y - seriesClipRect.Top));
            startY = Chart.ActualPointToValue(ActualYAxis, new Point(rect.X - seriesClipRect.Left, rect.Y - seriesClipRect.Top));
            if (ActualXAxis.IsVertical)
            {
                endX = Chart.ActualPointToValue(ActualXAxis, new Point(rect.X - seriesClipRect.Left, bottom));
                endY = Chart.ActualPointToValue(ActualYAxis, new Point(right, rect.Y - seriesClipRect.Top));
            }
            else
            {
                endX = Chart.ActualPointToValue(ActualXAxis, new Point(right, rect.Y - seriesClipRect.Top));
                endY = Chart.ActualPointToValue(ActualYAxis, new Point(rect.X - seriesClipRect.Left, bottom));
            }

            if (startX > endX)
            {
                double temp = endX;
                endX = startX;
                startX = temp;
            }

            if (startY > endY)
            {
                double temp = endY;
                endY = startY;
                startY = temp;
            }
        }

        internal virtual List<object> GetDataPoints(double startX, double endX, double startY, double endY, int minimum, int maximum, List<double> xValues, bool validateYValues)
        {
            List<object> dataPoints = new List<object>();
            if (xValues.Count != ActualSeriesYValues[0].Count)
            {
                return null;
            }

            var yValues = ActualSeriesYValues[0];
            for (int i = minimum; i <= maximum; i++)
            {
                double xValue = xValues[i];
                if (validateYValues || (startX <= xValue && xValue <= endX))
                {
                    double yValue = yValues[i];
                    if (startY <= yValue && yValue <= endY)
                    {
                        dataPoints.Add(ActualData[i]);
                    }
                }
            }

            return dataPoints;
        }
    }
}
