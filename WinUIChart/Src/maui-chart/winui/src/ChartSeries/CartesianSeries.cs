﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using AdornmentSeries = Syncfusion.UI.Xaml.Charts.DataMarkerSeries;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Serves as a base class for all the Cartesian chart types used to visualize data points in Cartersian coordinate system.
    /// </summary>
    /// <example>
    /// <code language = "XAML" >
    ///       &lt;syncfusion:BarSeries ItemsSource="{Binding Data}" XBindingPath="Year" YBindingPath="Value"&gt;
    ///       &lt;/syncfusion:BarSeries&gt;
    /// </code>
    /// <code language= "C#" >
    ///       BarSeries series1 = new BarSeries();
    ///       series1.ItemsSource = viewmodel.Data;
    ///       series1.XBindingPath = "Year";
    ///       series1.YBindingPath = "Value";
    ///       chart.Series.Add(series1);
    ///    </code>
    /// </example>
    public abstract class CartesianSeries : AdornmentSeries, ISupportAxes2D
    {
        #region Dependency Property Registration
        /// <summary>
        /// The DependencyProperty for <see cref="CartesianDataLabelSettings"/> property.
        /// </summary>
        public static readonly DependencyProperty DataLabelSettingsProperty =
          DependencyProperty.Register(nameof(DataLabelSettings), typeof(CartesianDataLabelSettings), typeof(CartesianSeries),
          new PropertyMetadata(null, OnAdornmentsInfoChanged));


        /// <summary>
        /// The DependencyProperty for <see cref="Trendlines"/> property.
        /// </summary>
        internal static readonly DependencyProperty TrendlinesProperty =
           DependencyProperty.Register(nameof(Trendlines), typeof(ChartTrendLineCollection), typeof(CartesianSeries),
           new PropertyMetadata(null, OnTrendlinesChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="XAxis"/> property.
        /// </summary>
        public static readonly DependencyProperty XAxisProperty =
            DependencyProperty.Register(nameof(XAxis), typeof(ChartAxisBase2D), typeof(CartesianSeries),
            new PropertyMetadata(null, OnXAxisChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="YAxis"/> property.
        /// </summary>
        public static readonly DependencyProperty YAxisProperty =
            DependencyProperty.Register(nameof(YAxis), typeof(RangeAxisBase), typeof(CartesianSeries),
            new PropertyMetadata(null, OnYAxisChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="IsTransposed"/> property.
        /// </summary>
        public static readonly DependencyProperty IsTransposedProperty =
            DependencyProperty.Register(nameof(IsTransposed), typeof(bool), typeof(CartesianSeries),
            new PropertyMetadata(false, OnTransposeChanged));

        /// <summary>
        /// The DependencyProperty for <see cref="ShowTrackballInfo"/> property.
        /// </summary>
        public static readonly DependencyProperty ShowTrackballInfoProperty =
            DependencyProperty.Register(nameof(ShowTrackballInfo), typeof(bool), typeof(CartesianSeries), new PropertyMetadata(true));

#endregion

#region Constructor

        /// <summary>
        /// Called when instance created for CartesianSeries
        /// </summary>
        public CartesianSeries()
        {
            Trendlines = new ChartTrendLineCollection();
            DataLabelSettings = new CartesianDataLabelSettings();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// <para>Gets or sets data labels for the cartesian series.</para> <para>This allows us to customize the appearance of a data point by displaying labels, shapes and connector lines.</para>
        /// </summary>
        /// <value>
        /// The <see cref="ChartDataLabelSettings" /> value.
        /// </value>
        /// <example>
        /// # [MainWindow.xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <chart:SfCartesianChart.PrimaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.PrimaryAxis>
        ///
        ///           <chart:SfCartesianChart.SecondaryAxis>
        ///               <chart:NumericalAxis/>
        ///           </chart:SfCartesianChart.SecondaryAxis>
        ///
        ///           <chart:SfCartesianChart.Series>
        ///               <chart:ColumnSeries ItemsSource="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue">
        ///                    <syncfusion:ColumnSeries.DataLabelSettings>
        ///                          <chart:ChartDataLabelSettings Visible="True" />
        ///                    <syncfusion:ColumnSeries.DataLabelSettings>
        ///               </chart:ColumnSeries> 
        ///           </chart:SfCartesianChart.Series>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [MainWindow.cs](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     NumericalAxis primaryAxis = new NumericalAxis();
        ///     NumericalAxis secondaryAxis = new NumericalAxis();
        ///     
        ///     chart.PrimaryAxis = primaryAxis;
        ///     chart.SecondaryAxis = secondaryAxis;
        ///     
        ///     ColumnSeries series = new ColumnSeries();
        ///     series.ItemsSource = viewmodel.Data;
        ///     series.XBindingPath = "XValue";
        ///     series.YBindingPath = "YValue";
        ///     chart.Series.Add(series);
        ///     
        ///     series.DataLabelSettings = new ChartDataLabelSettings(){ Visible = true };
        /// ]]></code>
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

        internal override ChartDataLabelSettings AdornmentsInfo
        {
            get
            {
                return (CartesianDataLabelSettings)GetValue(DataLabelSettingsProperty);
            }
        }

        /// <summary>
        /// Gets or sets the trendline collection for this series.
        /// </summary>
        /// <remarks>
        /// Trends consider only non empty sets of corresponding <see cref="CartesianSeries"/>.
        /// </remarks>
        internal ChartTrendLineCollection Trendlines
        {
            get { return (ChartTrendLineCollection)GetValue(TrendlinesProperty); }
            set { SetValue(TrendlinesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the x axis range corresponding to this series.
        /// </summary>
        public DoubleRange XRange { get; internal set; }

        /// <summary>
        /// Gets or sets the y axis range corresponding to this series.
        /// </summary>
        public DoubleRange YRange { get; internal set; }
        
        /// <summary>
        /// Gets or sets the additional x axis for this series.
        /// </summary>
        /// <remarks>
        /// This property is used to add multiple axis in <see cref="ChartBase"/>.
        /// </remarks>
        /// <example>
        ///     <code language = "XAML">
        ///         &lt;syncfusion:ColumnSeries ItemsSource="{Binding Demands}" XBindingPath="Demand" YBindingPath="Year2010"&gt;
        ///              &lt;syncfusion:ColumnSeries.XAxis&gt;
        ///                   &lt;syncfusion:NumericalAxis Header="Additional X Axis"/&gt;
        ///              &lt;/syncfusion:ColumnSeries.XAxis&gt;
        ///          &lt;/syncfusion:ColumnSeries&gt;
        ///     </code>
        /// </example>
        public ChartAxisBase2D XAxis
        {
            get { return (ChartAxisBase2D)GetValue(XAxisProperty); }
            set { SetValue(XAxisProperty, value); }
        }

        /// <summary>
        /// Gets or sets the additional y axis for this series.
        /// </summary>
        /// <remarks>
        /// This property is used to add multiple axis in <see cref="ChartBase"/>.
        /// </remarks>
        /// <example>
        ///     <code language = "XAML">
        ///         &lt;syncfusion:ColumnSeries ItemsSource="{Binding Demands}" XBindingPath="Demand" YBindingPath="Year2010"&gt;
        ///              &lt;syncfusion:ColumnSeries.YAxis&gt;
        ///                   &lt;syncfusion:NumericalAxis Header="Additional Y Axis"/&gt;
        ///              &lt;/syncfusion:ColumnSeries.YAxis&gt;
        ///          &lt;/syncfusion:ColumnSeries&gt;
        ///     </code>
        /// </example>
        public RangeAxisBase YAxis
        {
            get { return (RangeAxisBase)GetValue(YAxisProperty); }
            set { SetValue(YAxisProperty, value); }
        }


        /// <summary>
        /// Gets or sets a value indicating whether to exchange the orientation of the series.
        /// </summary>
        /// <value>
        ///     <c>True</c> exchanges the horizontal axis to vertical and vice versa. 
        ///     <c>False</c> is the default behavior.
        /// </value>
        public bool IsTransposed
        {
            get { return (bool)GetValue(IsTransposedProperty); }
            set { SetValue(IsTransposedProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show/hide the series information.
        /// </summary>
        public bool ShowTrackballInfo
        {
            get { return (bool)GetValue(ShowTrackballInfoProperty); }
            set { SetValue(ShowTrackballInfoProperty, value); }
        }

#endregion

        ChartAxis ISupportAxes.ActualXAxis
        {
            get { return ActualXAxis; }
        }

        ChartAxis ISupportAxes.ActualYAxis
        {
            get { return ActualYAxis; }
        }

#endregion

#region Methods

#region Internal Virtual Methods

        /// <summary>
        /// Create trend line for series
        /// </summary>
        internal virtual void CreateTrendline()
        {
            if (Trendlines == null)
            {
                return;
            }

            foreach (var trend in Trendlines)
            {
                if (IsSeriesVisible && trend.IsTrendlineVisible)
                {
                    trend.ApplyTemplate();
                    trend.UpdateElements();
                }
            }
        }

#endregion

#region Internal Override Methods

        internal override void OnTransposeChanged(bool val)
        {
            IsActualTransposed = val;
        }

        internal override void UpdateRange()
        {
            XRange = DoubleRange.Empty;
            YRange = DoubleRange.Empty;

            if (Segments == null)
            {
                return;
            }

            if (Segments.Count > 0)
            {
                foreach (ChartSegment segment in Segments)
                {
                    XRange += segment.XRange;
                    YRange += segment.YRange;
                }

                if (IsSideBySide)
                {
                    if (SideBySideInfoRangePad != null && !SideBySideInfoRangePad.IsEmpty)
                    {
                        bool isAlterRange = ((this.ActualXAxis is NumericalAxis && (this.ActualXAxis as NumericalAxis).RangePadding == NumericalPadding.None)
                       || (this.ActualXAxis is DateTimeAxis && (this.ActualXAxis as DateTimeAxis).RangePadding == DateTimeRangePadding.None));
                        XRange = isAlterRange ? new DoubleRange(XRange.Start - SideBySideInfoRangePad.Start, XRange.End - SideBySideInfoRangePad.End)
                            : new DoubleRange(XRange.Start + SideBySideInfoRangePad.Start, XRange.End + SideBySideInfoRangePad.End);
                    }
                }
            }
            else if (this.DataCount == 1) // WPF-16569 condition for setting the range for single data with zero segments in cartesian series
            {
                var xValues = GetXValues();
                var yValues = this.ActualSeriesYValues[0] as List<double>;
                if (xValues.Count > 0 && yValues.Count > 0)
                {
                    double xValue = xValues[0];
                    double yValue = yValues[0];

                    XRange = new DoubleRange(xValue - 1, xValue + 1);
                    YRange = new DoubleRange(yValue - 1, yValue + 1);
                }
            }

            var visibleTrendlines = from line in Trendlines
                                    where line.Visibility == Visibility.Visible
                                    select line;

            foreach (var item in visibleTrendlines)
            {
                foreach (ChartSegment segment in item.TrendlineSegments)
                {
                    XRange += segment.XRange;
                    YRange += segment.YRange;
                }
            }
        }

        /// <summary>
        /// Update series bound
        /// </summary>
        /// <param name="size"></param>
        internal override void UpdateOnSeriesBoundChanged(Size size)
        {
            base.UpdateOnSeriesBoundChanged(size);

            if (Segments == null)
            {
                return;
            }

            foreach (var trend in Trendlines)
            {
                if (trend.TrendlinePanel != null)
                {
                    foreach (ChartSegment segment in trend.TrendlineSegments)
                    {
                        segment.OnSizeChanged(size);
                    }

                    trend.TrendlinePanel.Update(size);
                }
            }
        }

        /// <summary>
        /// Calculate Segments
        /// </summary>
        internal override void CalculateSegments()
        {
            base.CalculateSegments();
            CreateTrendline();
        }

        internal override int GetDataPointIndex(Point point)
        {
            Canvas canvas = Area.GetAdorningCanvas();
            double left = Area.ActualWidth - canvas.ActualWidth;
            double top = Area.ActualHeight - canvas.ActualHeight;

            point.X = point.X - left - Area.SeriesClipRect.Left + Area.Margin.Left;
            point.Y = point.Y - top - Area.SeriesClipRect.Top + Area.Margin.Top;
            double x, y, stackedValues;
            FindNearestChartPoint(point, out x, out y, out stackedValues);
            return GetXValues().IndexOf(x);
        }

#endregion

#region Internal Methods

        internal override void Dispose()
        {
            Trendlines = null;
            base.Dispose();
        }

        internal void OnVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
        {
            OnVisibleRangeChanged(e);
        }

#endregion

#region Protected Virtual Methods

        /// <summary>
        /// Called when VisibleRange property changed.
        /// </summary>
        protected virtual void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called when YAxis created or changed.
        /// </summary>
        /// <param name="oldAxis"></param>
        /// <param name="newAxis"></param>
        protected virtual void OnYAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
        {
            if (newAxis != null)
                newAxis.IsSecondaryAxis = true;

            if (XAxis != null)
            {
                if (Area != null && Area.InternalSecondaryAxis != null && Area.InternalSecondaryAxis.AssociatedAxes != null
                    && Area.InternalSecondaryAxis.AssociatedAxes.Contains(XAxis))
                {
                    Area.InternalSecondaryAxis.AssociatedAxes.Remove(XAxis);
                    if (XAxis.AssociatedAxes.Contains(Area.InternalSecondaryAxis))
                        XAxis.AssociatedAxes.Remove(Area.InternalSecondaryAxis);
                }
            }

            if (oldAxis != null && oldAxis.RegisteredSeries != null)
            {
                var axis = oldAxis as NumericalAxis;
                if (axis != null && axis.AxisScaleBreaks.Count > 0)
                    axis.ClearBreakElements();
                oldAxis.RegisteredSeries.Remove(this);

                if(oldAxis.RegisteredSeries.Count == 0 && Area != null)
                {
                    var cartersianArea = Area as SfCartesianChart;
#pragma warning disable CS0618 // Type or member is obsolete
                    var primaryAxis = cartersianArea != null ? cartersianArea.PrimaryAxis : (Area as SfChart).PrimaryAxis;
                    var secondaryAxis = cartersianArea != null ? cartersianArea.SecondaryAxis : (Area as SfChart).SecondaryAxis;
#pragma warning restore CS0618 // Type or member is obsolete
                    if (Area.InternalAxes.Contains(oldAxis) && primaryAxis != oldAxis && secondaryAxis != oldAxis)
                    {
                        Area.InternalAxes.RemoveItem(oldAxis, Area.DependentSeriesAxes.Contains(oldAxis));
                        Area.DependentSeriesAxes.Remove(oldAxis);
                    }
                }
            }
            else if (ActualArea != null && ActualArea.InternalSecondaryAxis != null
                    && ActualArea.InternalSecondaryAxis.RegisteredSeries.Contains(this))
                ActualArea.InternalSecondaryAxis.RegisteredSeries.Remove(this);

            if (newAxis != null)
            {
                if (Area != null)
                {
                    if (!Area.InternalAxes.Contains(newAxis))
                    {
                        Area.InternalAxes.Add(newAxis);
                        Area.DependentSeriesAxes.Add(newAxis);
                        newAxis.Area = Area;
                    }

                    if (!newAxis.RegisteredSeries.Contains(this))
                        newAxis.RegisteredSeries.Add(this);
                }
            }

            if (Area != null)
                Area.ScheduleUpdate();

            if (newAxis != null)
                newAxis.Orientation = IsActualTransposed ? Orientation.Horizontal : Orientation.Vertical;
        }

        /// <summary>
        /// Called when XAxis created or changed.
        /// </summary>
        /// <param name="oldAxis"></param>
        /// <param name="newAxis"></param>
        protected virtual void OnXAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
        {
            if (YAxis != null)
            {
                if (Area != null && Area.InternalPrimaryAxis != null && Area.InternalPrimaryAxis.AssociatedAxes != null
                    && Area.InternalPrimaryAxis.AssociatedAxes.Contains(YAxis))
                {
                    Area.InternalPrimaryAxis.AssociatedAxes.Remove(YAxis);
                    if (YAxis.AssociatedAxes.Contains(ActualArea.InternalPrimaryAxis))
                        YAxis.AssociatedAxes.Remove(ActualArea.InternalPrimaryAxis);
                }
            }

            if (oldAxis != null && oldAxis.RegisteredSeries != null)
            {
                oldAxis.VisibleRangeChanged -= OnVisibleRangeChanged;

                oldAxis.RegisteredSeries.Remove(this);

                if (oldAxis.RegisteredSeries.Count == 0 && Area != null)
                {
                    var cartersianArea = Area as SfCartesianChart;
#pragma warning disable CS0618 // Type or member is obsolete
                    var primaryAxis = cartersianArea != null ? cartersianArea.PrimaryAxis : (Area as SfChart).PrimaryAxis;
                    var secondaryAxis = cartersianArea != null ? cartersianArea.SecondaryAxis : (Area as SfChart).SecondaryAxis;
#pragma warning restore CS0618 // Type or member is obsolete
                    if (Area.InternalAxes.Contains(oldAxis) && primaryAxis != oldAxis && secondaryAxis != oldAxis)
                    {
                        Area.InternalAxes.RemoveItem(oldAxis, Area.DependentSeriesAxes.Contains(oldAxis));
                        Area.DependentSeriesAxes.Remove(oldAxis);
                    }
                }
            }
            else if (ActualArea != null && ActualArea.InternalPrimaryAxis != null
                     && ActualArea.InternalPrimaryAxis.RegisteredSeries.Contains(this))
                ActualArea.InternalPrimaryAxis.RegisteredSeries.Remove(this);

            if (newAxis != null)
            {
                if (Area != null)
                {
                    if (!Area.InternalAxes.Contains(newAxis) && newAxis != Area.InternalPrimaryAxis)
                    {
                        Area.InternalAxes.Add(newAxis);
                        Area.DependentSeriesAxes.Add(newAxis);
                        newAxis.Area = Area;
                    }

                    if (!newAxis.RegisteredSeries.Contains(this))
                        newAxis.RegisteredSeries.Add(this);
                }

                newAxis.VisibleRangeChanged += OnVisibleRangeChanged;
            }

            if (Area != null)
            {
                Area.SBSInfoCalculated = false;
                Area.ScheduleUpdate();
            }

            if (newAxis != null)
                newAxis.Orientation = IsActualTransposed ? Orientation.Vertical : Orientation.Horizontal;
        }

#endregion

#region Protected Override Methods

        /// <summary>
        /// Invoke to render chart series.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            foreach (var trend in Trendlines)
            {
                if (!this.SeriesPanel.Children.Contains(trend))
                {
                    trend.Series = this;
                    trend.UpdateLegendIconTemplate(true);
                    this.SeriesPanel.Children.Add(trend);
                    Canvas.SetZIndex(trend, 1);
                }
            }
        }

#endregion

#region Private Static Methods

        private static void OnTransposeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CartesianSeries).OnTransposeChanged(Convert.ToBoolean(e.NewValue));
        }

        private static void OnYAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CartesianSeries).OnYAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
        }

        private static void OnXAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CartesianSeries).OnXAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
        }

        private static void OnTrendlinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cartesianSeries = d as CartesianSeries;
            if (cartesianSeries != null) cartesianSeries.OnTrendlinesChanged(e);
        }

#endregion

#region Private Methods

        private void OnTrendlinesChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                if (SeriesPanel != null)
                    foreach (var trend in (e.OldValue as ChartTrendLineCollection))
                    {
                        if (SeriesPanel.Children.Contains(trend))
                            SeriesPanel.Children.Remove(trend);
                    }

               (e.OldValue as ChartTrendLineCollection).Clear();
                (e.OldValue as ChartTrendLineCollection).CollectionChanged -= Trendlines_CollectionChanged;
            }

            if (e.NewValue != null)
            {
                foreach (var trendline in e.NewValue as ChartTrendLineCollection)
                {
                    if (trendline != null)
                    {
                        trendline.Series = this;
                    }
                }

                Trendlines.CollectionChanged += Trendlines_CollectionChanged;
            }

            if (Area != null) Area.ScheduleUpdate();
        }

        private void Trendlines_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var trend = e.NewItems[0] as TrendlineBase;
                if (trend != null)
                {
                    trend.Series = this;
                    if (SeriesPanel != null && Area != null)
                    {
                        trend.UpdateLegendIconTemplate(true);
                        SeriesPanel.Children.Add(trend);
                        Area.UpdateLegend(Area.Legend, false);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var trend = e.OldItems[0] as TrendlineBase;
                if (trend != null && SeriesPanel != null)
                {
                    if (SeriesPanel.Children.Contains(trend))
                    {
                        if (Area.Legend != null && Area.LegendItems != null)
                        {
                            var filterSeriesLegend = Area.LegendItems.Where(item => item.Where(it => it.Series == this).Count() > 0).ToList();
                            if (filterSeriesLegend.Count > 0)
                            {
                                var index = Area.LegendItems.IndexOf(filterSeriesLegend[0]);
                                var containlegendtrenditem = Area.LegendItems[index].Where(it => it.Trendline == trend).ToList();
                                if (containlegendtrenditem.Count() > 0 && Area.LegendItems[index].Contains(containlegendtrenditem[0]))
                                    Area.LegendItems[index].Remove(containlegendtrenditem[0]);
                            }
                        }

                        SeriesPanel.Children.Remove(trend);
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                if (SeriesPanel != null)
                {
                    UIElement[] seriesPanelCollection = new UIElement[SeriesPanel.Children.Count];
                    SeriesPanel.Children.CopyTo(seriesPanelCollection, 0);
                    foreach (var trend in seriesPanelCollection.Where(it => (it is TrendlineBase)))
                    {
                        if (SeriesPanel.Children.Contains(trend))
                        {
                            if (Area.Legend != null && Area.LegendItems != null)
                            {
                                var filterSeriesLegend = Area.LegendItems.Where(item => item.Where(it => it.Series == this).Count() > 0).ToList();
                                if (filterSeriesLegend.Count > 0)
                                {
                                    var index = Area.LegendItems.IndexOf(filterSeriesLegend[0]);
                                    var containlegendtrenditem = Area.LegendItems[index].Where(it => it.Trendline == trend).ToList();
                                    if (containlegendtrenditem.Count() > 0 && Area.LegendItems[index].Contains(containlegendtrenditem[0]))
                                        Area.LegendItems[index].Remove(containlegendtrenditem[0]);
                                }
                            }

                            SeriesPanel.Children.Remove(trend);
                        }
                    }
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
            {
                var trend = e.OldItems[0] as TrendlineBase;
                if (trend != null && SeriesPanel != null && SeriesPanel.Children.Contains(trend))
                {
                    if (Area.Legend != null && Area.LegendItems != null)
                    {
                        var filterSeriesLegend = Area.LegendItems.Where(item => item.Where(it => it.Series == this).Count() > 0).ToList();
                        if (filterSeriesLegend.Count > 0)
                        {
                            var index = Area.LegendItems.IndexOf(filterSeriesLegend[0]);
                            var containlegendtrenditem = Area.LegendItems[index].Where(it => it.Trendline == trend).ToList();
                            if (containlegendtrenditem.Count() > 0 && Area.LegendItems[index].Contains(containlegendtrenditem[0]))
                                Area.LegendItems[index].Remove(containlegendtrenditem[0]);
                        }
                    }

                    SeriesPanel.Children.Remove(trend);
                }

                trend = e.NewItems[0] as TrendlineBase;
                if (trend != null)
                {
                    trend.Series = this;
                    if (SeriesPanel != null)
                    {
                        trend.UpdateLegendIconTemplate(true);
                        SeriesPanel.Children.Add(trend);
                        Area.UpdateLegend(Area.Legend, false);
                    }

                }
            }

            if (Area != null) Area.ScheduleUpdate();
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
        public List<object> GetDataPoints(Rect rectangle)
        {
            if (Area == null || Area.SeriesClipRect == null || ActualXAxis == null || ActualYAxis == null)
            {
                return null;
            }
            
            rectangle.Intersect(Area.SeriesClipRect);
            var xValues = GetXValues();
            if (xValues == null || (rectangle.Height <= 0 && rectangle.Width <= 0))
            {
                return null;
            }

            double startX = double.NaN, startY = double.NaN, endX = double.NaN, endY = double.NaN;
            ConvertRectToValue(ref startX, ref endX, ref startY, ref endY, rectangle);
            bool isVertical = false;
            if (ActualXAxis.Orientation == Orientation.Vertical)
            {
                isVertical = true;
            }

            if (rectangle.Width > 0 && rectangle.Height > 0)
            {
                return GetDataPoints(startX, endX, startY, endY);
            }
            else if ((!isVertical && rectangle.Height <= 0) || (isVertical && rectangle.Width <= 0))
            {
                if (isLinearData)
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
        public List<object> GetDataPoints(double startX, double endX, double startY, double endY)
        {
            var xValues = GetXValues();
            int minimum = 0, maximum = xValues.Count - 1;
            if (isLinearData)
            {
                CalculateNearestIndex(ref minimum, ref maximum, xValues, startX, endX);
            }

            return GetDataPoints(startX, endX, startY, endY, minimum, maximum, xValues, isLinearData);
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
            var seriesClipRect = Area.SeriesClipRect;
            double right = rect.X + rect.Width - seriesClipRect.Left;
            double bottom = rect.Y + rect.Height - seriesClipRect.Top;

            startX = Area.ActualPointToValue(ActualXAxis, new Point(rect.X - seriesClipRect.Left, rect.Y - seriesClipRect.Top));
            startY = Area.ActualPointToValue(ActualYAxis, new Point(rect.X - seriesClipRect.Left, rect.Y - seriesClipRect.Top));
            if (ActualXAxis.Orientation == Orientation.Vertical)
            {
                endX = Area.ActualPointToValue(ActualXAxis, new Point(rect.X - seriesClipRect.Left, bottom));
                endY = Area.ActualPointToValue(ActualYAxis, new Point(right, rect.Y - seriesClipRect.Top));
            }
            else
            {
                endX = Area.ActualPointToValue(ActualXAxis, new Point(right, rect.Y - seriesClipRect.Top));
                endY = Area.ActualPointToValue(ActualYAxis, new Point(rect.X - seriesClipRect.Left, bottom));
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
