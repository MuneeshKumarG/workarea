using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// ZoomPanBehavior enables zooming and panning operations over a cartesian chart.
    /// </summary>
    /// <remarks>
    /// To enable the zooming and panning in the chart, create an instance of <see cref="ChartZoomPanBehavior"/> and set it to the <c>ZoomPanBehavior</c> property of <see cref="SfCartesianChart"/>.
    /// </remarks>
    /// <example>
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.ZoomPanBehavior>
    ///               <chart:ChartZoomPanBehavior />
    ///           </chart:SfCartesianChart.ZoomPanBehavior>
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     
    ///     chart.ZoomPanBehavior = new ChartZoomPanBehavior();
    ///     
    /// ]]></code>
    /// ***
    /// </example>
    public partial class ChartZoomPanBehavior : ChartBehavior
    {
        #region Fields
        private GestureStatus pinchStatus = GestureStatus.Canceled;
        private bool isPinchZoomingActivated = false;
        private float cumulativeZoomLevel = 1f;
        private bool enableDirectionalZooming = false;

        #endregion

        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="EnablePinchZooming"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EnablePinchZoomingProperty = BindableProperty.Create(nameof(EnablePinchZooming), typeof(bool), typeof(ChartZoomPanBehavior), true, BindingMode.Default, null, OnEnableZoomingPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EnablePanning"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EnablePanningProperty = BindableProperty.Create(nameof(EnablePanning), typeof(bool), typeof(ChartZoomPanBehavior), true, BindingMode.Default, null, OnEnablePanningPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="EnableDoubleTap"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty EnableDoubleTapProperty = BindableProperty.Create(nameof(EnableDoubleTap), typeof(bool), typeof(ChartZoomPanBehavior), true, BindingMode.Default, null, OnEnableDoubleTapPropertyChanged);

        /// <summary>
        /// Identifies the <see cref="ZoomMode"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty ZoomModeProperty = BindableProperty.Create(nameof(ZoomMode), typeof(ZoomMode), typeof(ChartZoomPanBehavior), ZoomMode.XY, BindingMode.Default, null, OnZoomModePropertyChanged);

        /// <summary>
        /// Gets or sets a value indicating whether the finger gesture is enabled or disabled.
        /// </summary>
        /// <value>
        /// It accepts the bool values and its default value is <c>true</c>.
        /// </value>
        /// <remarks>
        /// If this property is true, zooming is performed based on the pinch gesture of the user. If this property is false, zooming is performed based on the mouse wheel of the user.
        /// </remarks>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-3)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.ZoomPanBehavior>
        ///        <chart:ChartZoomPanBehavior EnablePinchZooming="True"/>
        ///     </chart:SfCartesianChart.ZoomPanBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"/>
        /// 
        /// </chart:SfCartesianChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-4)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        /// 
        /// // omitted for brevity
        /// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
        /// { 
        ///    EnablePinchZooming = true, 
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool EnablePinchZooming
        {
            get { return (bool)GetValue(EnablePinchZoomingProperty); }
            set { SetValue(EnablePinchZoomingProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the panning is enabled.
        /// </summary>
        /// <value>
        /// It accepts the bool values and the default value is <c>true</c>.
        /// </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.ZoomPanBehavior>
        ///        <chart:ChartZoomPanBehavior EnablePanning = "True" />
        ///     </chart:SfCartesianChart.ZoomPanBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"/>
        /// 
        /// </chart:SfCartesianChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-10)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        /// 
        /// // omitted for brevity
        /// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
        /// { 
        ///    EnablePanning = true,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool EnablePanning
        {
            get { return (bool)GetValue(EnablePanningProperty); }
            set { SetValue(EnablePanningProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether zooming is enabled through double tapping.
        /// </summary>
        /// <value>
        /// It accepts the bool values and the default value is <c>true</c>.
        /// </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-9)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.ZoomPanBehavior>
        ///        <chart:ChartZoomPanBehavior EnableDoubleTap = "True" />
        ///     </chart:SfCartesianChart.ZoomPanBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"/>
        /// 
        /// </chart:SfCartesianChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-10)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        /// 
        /// // omitted for brevity
        /// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
        /// { 
        ///    EnableDoubleTap = true,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public bool EnableDoubleTap
        {
            get { return (bool)GetValue(EnableDoubleTapProperty); }
            set { SetValue(EnableDoubleTapProperty, value); }
        }

        /// <summary>
        /// Gets or sets the mode for zooming direction.
        /// </summary>
        /// <remarks>The zooming can be done both horizontally and vertically.</remarks>
        /// <value>
        /// It accepts the <see cref="ZoomMode"/> values and the default value is <see cref="ZoomMode.XY"/>.
        /// </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-5)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.ZoomPanBehavior>
        ///        <chart:ChartZoomPanBehavior ZoomMode="X"/>
        ///     </chart:SfCartesianChart.ZoomPanBehavior>
        ///
        ///     <chart:LineSeries ItemsSource="{Binding Data}"
        ///                       XBindingPath="XValue"
        ///                       YBindingPath="YValue"/>
        ///          
        /// </chart:SfCartesianChart>
        ///
        /// ]]>
        /// </code>
        /// # [MainPage.xaml.cs](#tab/tabid-6)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        /// 
        /// // omitted for brevity
        /// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
        /// { 
        ///    ZoomMode = ZoomMode.X,
        /// };
        /// 
        /// LineSeries series = new LineSeries()
        /// {
        ///    ItemsSource = viewModel.Data,
        ///    XBindingPath = "XValue",
        ///    YBindingPath = "YValue",
        /// };
        /// chart.Series.Add(series);
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ZoomMode ZoomMode
        {
            get { return (ZoomMode)GetValue(ZoomModeProperty); }
            set { SetValue(ZoomModeProperty, value); }
        }

        #region internal properties
        /// <summary>
        /// Gets or sets the value that determines the maximum zoom level of the chart. 
        /// </summary>
        internal static readonly BindableProperty MaximumZoomLevelProperty = BindableProperty.Create(nameof(MaximumZoomLevel), typeof(float), typeof(ChartZoomPanBehavior), float.NaN, BindingMode.Default, null, OnMaximumZoomLevelPropertyChanged);

        /// <summary>
        /// Gets or sets the value that determines the maximum zoom level of the chart. 
        /// </summary>
        /// <value>This property takes the float value.</value>
        internal float MaximumZoomLevel
        {
            get { return (float)GetValue(MaximumZoomLevelProperty); }
            set { SetValue(MaximumZoomLevelProperty, value); }
        }

        #endregion

        #endregion

        #region Internal Properties

        internal bool EnableDirectionalZooming
        {
            get { return enableDirectionalZooming; }
            set
            {
                if (enableDirectionalZooming != value)
                {
                    enableDirectionalZooming = value;
                }
            }
        }

        internal SfCartesianChart? Chart { get; set; }

        #endregion

        #region Methods

        #region Internal Methods

        internal override void SetTouchHandled(IChart chart)
        {
#if MONOANDROID || WINDOWS
            var cartesianChart = chart as SfCartesianChart;
            if (cartesianChart != null && EnablePanning)
            {
                cartesianChart.IsHandled = true;
            }
#endif
        }

        internal override void OnDoubleTap(IChart chart, float x, float y)
        {
            base.OnDoubleTap(chart, x, y);

            var clipRect = chart.ActualSeriesClipRect;
            if (clipRect.Contains(x, y) && chart is SfCartesianChart cartesianChart)
                OnDoubleTap(cartesianChart.ChartArea, clipRect, x, y);
        }

        internal void OnScrollChanged(IChart chart, Point touchPoint, Point translatePoint)
        {
            var clipRect = chart.ActualSeriesClipRect;
            if (chart is SfCartesianChart cartesianChart)
            {
                cartesianChart.IsHandled = TouchHandled(cartesianChart, translatePoint);

                if (clipRect.Contains(touchPoint))
                {
                    PanTranslate(cartesianChart, clipRect, translatePoint);
                }
            }
        }

        internal void OnMouseWheelChanged(IChart chart, Point position, double delta)
        {
            var width = (float)chart.ActualSeriesClipRect.Width;
            var height = (float)chart.ActualSeriesClipRect.Height;

            if (EnablePinchZooming && chart.ActualSeriesClipRect.Contains(position) && chart.Area is CartesianChartArea area)
            {
                var direction = delta > 0 ? 1 : -1;

                cumulativeZoomLevel = Math.Max(cumulativeZoomLevel + (0.25f * direction), 1);

                foreach (var chartAxis in area.XAxes)
                {
                    if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
                    {
                        var origin = GetOrigin((float)position.X, (float)position.Y, width, height, chartAxis);
                        Zoom(cumulativeZoomLevel, origin, chartAxis);
                    }
                } 
                
                foreach (var chartAxis in area.YAxes)
                {
                    if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
                    {
                        var origin = GetOrigin((float)position.X, (float)position.Y, width, height, chartAxis);
                        Zoom(cumulativeZoomLevel, origin, chartAxis);
                    }
                }
            }
        }

        bool TouchHandled(SfCartesianChart cartesian, Point velocity)
        {
            var area = cartesian.ChartArea;

            if (!EnablePanning || area == null)
            {
                return false;
            }

            bool isPanEnd = true;

            if (Math.Abs(velocity.Y) < Math.Abs(velocity.X))
            {
                foreach (ChartAxis axis in area.XAxes)
                {
                    double position = axis.ZoomPosition;
                    double factor = 1.0f - axis.ZoomFactor;
                    bool velocityIsCrossed = axis.IsInversed ? velocity.X > 0 : velocity.X < 0;

                    if ((position == factor && velocityIsCrossed) || (position == 0 && !velocityIsCrossed))
                    {
                        isPanEnd = false;
                    }
                    else
                    {
                        isPanEnd = true;
                        break;
                    }
                }

                return isPanEnd;
            }
            else
            {
                foreach (ChartAxis axis in area.YAxes)
                {
                    double position = axis.ZoomPosition;
                    double factor = 1.0f - axis.ZoomFactor;
#if MONOANDROID || WINDOWS
                    bool velocityIsCrossed = axis.IsInversed ? velocity.Y < 0 : velocity.Y > 0;

                    if ((position == factor && velocity.Y < 0) || (position == 0 && velocityIsCrossed))
#else
                    bool velocityIsCrossed = axis.IsInversed ? velocity.Y > 0 : velocity.Y <= 0;

                    if ((position == factor && velocity.Y > 0) || (position == 0 && velocityIsCrossed))
#endif
                    {
                        isPanEnd = false;
                    }
                    else
                    {
                        isPanEnd = true;
                        break;
                    }
                }

                return isPanEnd;
            }
        }

        internal void OnPinchStateChanged(IChart chart, GestureStatus action, Point location, double angle, float scale)
        {
            var clipRect = chart.ActualSeriesClipRect;
            pinchStatus = action;

            if ((pinchStatus != GestureStatus.Completed) && EnablePinchZooming && clipRect.Contains(location) && chart is SfCartesianChart cartesianChart)
            {
                PinchZoom(cartesianChart, clipRect, location, angle, scale);
            }
            else
            {
                isPinchZoomingActivated = false;
            }

#if WINDOWS
              if (pinchStatus == GestureStatus.Completed && chart is SfCartesianChart sfCartesianChart)
                sfCartesianChart.IsHandled = false;
#endif
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Zooms in on the chart.
        /// </summary>
        /// <remarks>This method increases the zoom level of the chart.</remarks>
        public void ZoomIn()
        {
            if (Chart == null)
                return;

            var area = Chart.ChartArea;
            if (area.XAxes.Count > 0 && area.XAxes[0].ZoomPosition < 1)
            {
                cumulativeZoomLevel = cumulativeZoomLevel + 0.25f;
            }

            var origin = 0.5f;

            foreach (var chartAxis in area.XAxes)
            {
                if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
                {
                    Zoom(cumulativeZoomLevel, origin, chartAxis);
                }
            }

            foreach (var chartAxis in area.YAxes)
            {
                if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
                {
                    Zoom(cumulativeZoomLevel, origin, chartAxis);
                }
            }
        }

        /// <summary>
        /// Zooms out from the chart.
        /// </summary>
        /// <remarks>This method decreases the zoom level of the chart.</remarks>
        public void ZoomOut()
        {
            if (Chart == null)
                return;

            var area = Chart.ChartArea;

            if (area.XAxes.Count > 0 && area.XAxes[0].ZoomPosition > 0)
            {
                cumulativeZoomLevel = cumulativeZoomLevel - 0.25f;
            }

            var origin = 0.5f;

            foreach (var chartAxis in area.XAxes)
            {
                if (chartAxis != null && CanZoom(chartAxis, double.NaN, area.IsTransposed))
                {
                    Zoom(cumulativeZoomLevel, origin, chartAxis);
                }
            }

            foreach (var chartAxis in area.YAxes)
            {
                if (chartAxis != null && CanZoom(chartAxis, double.NaN, area.IsTransposed))
                {
                    Zoom(cumulativeZoomLevel, origin, chartAxis);
                }
            }
        }

        /// <summary>
        ///  Resets the zoom factor and zoom position for all the axis.
        /// </summary>
        public void Reset()
        {
            if (Chart == null) return;
            var area = Chart.ChartArea;
            var isTransposed = area.IsTransposed;
            var layout = area.AxisLayout;

            Reset(layout.HorizontalAxes, isTransposed);
            Reset(layout.VerticalAxes, isTransposed);
        }

        /// <summary>
        /// Zooms the chart by a specified range.
        /// </summary>
        /// <remarks>This method changes the zoom position and zoom factor of the given chart axis by a specified range.</remarks>
        public void ZoomByRange(ChartAxis chartAxis, double start, double end)
        {
            if (chartAxis != null && chartAxis.CartesianArea != null)
            {
                if (CanZoom(chartAxis, double.NaN, chartAxis.CartesianArea.IsTransposed))
                {
                    if (start > end)
                    {
                        var temp = start;
                        start = end;
                        end = temp;
                    }

                    DoubleRange axisRange = chartAxis.ActualRange;

                    if (start >= axisRange.End || end <= axisRange.Start)
                    {
                        return;
                    }

                    if (start < axisRange.Start)
                    {
                        start = axisRange.Start;
                    }

                    if (end > axisRange.End)
                    {
                        end = axisRange.End;
                    }

                    chartAxis.ZoomPosition = (start - axisRange.Start) / axisRange.Delta;
                    chartAxis.ZoomFactor = (end - start) / axisRange.Delta;
                }
            }
        }

        /// <summary>
        /// Zooms the chart by a specified range.
        /// </summary>
        /// <remarks>This method changes the zoom position and zoom factor of the given date time axis by a specified range.</remarks>
        public void ZoomByRange(DateTimeAxis dateTimeAxis, DateTime start, DateTime end)
        {
            ZoomByRange(dateTimeAxis, start.ToOADate(), end.ToOADate());
        }

        /// <summary>
        /// Zooms the chart by the specified zoom position and zoom factor.
        /// </summary>
        /// <remarks>This method changes the zoom position and zoom factor of the given chart axis.</remarks>
        public void ZoomToFactor(ChartAxis chartAxis, double zoomPosition, double zoomFactor)
        {
            if (chartAxis.CartesianArea != null &&
                (chartAxis.ZoomFactor != zoomFactor || chartAxis.ZoomPosition != zoomPosition))
            {
                if (CanZoom(chartAxis, double.NaN, chartAxis.CartesianArea.IsTransposed))
                {
                    chartAxis.ZoomFactor = zoomFactor;
                    chartAxis.ZoomPosition = zoomPosition;
                }
            }
        }

        /// <summary>
        /// Zooms the chart by the specified zoom factor.
        /// </summary>
        /// <remarks>This method changes the zoom factor of the horizontal and verticle axes.</remarks>
        public void ZoomToFactor(double zoomFactor)
        {
            if (Chart == null) return;
            var area = Chart.ChartArea;
            var isTransposed = area.IsTransposed;
            var layout = area.AxisLayout;

            Zoom(layout.HorizontalAxes, zoomFactor, isTransposed);
            Zoom(layout.VerticalAxes, zoomFactor, isTransposed);
        }

        private void Zoom(ObservableCollection<ChartAxis> axes, double zoomFactor, bool transposed)
        {
            foreach (var chartAxis in axes)
            {
                if (CanZoom(chartAxis, double.NaN, transposed))
                {
                    if (chartAxis.ZoomFactor <= 1 && chartAxis.ZoomFactor >= 0.1)
                    {
                        chartAxis.ZoomFactor = zoomFactor;
                        chartAxis.ZoomPosition = 0.5f;
                    }
                }
            }
        }

        #endregion

        #region Private Methods
        private bool CanReset(ObservableCollection<ChartAxis> axes)
        {
            if (axes != null)
            {
                return axes.Any(axis => axis.ZoomFactor < 1);
            }

            return false;
        }

        private void Reset(ObservableCollection<ChartAxis> axes, bool transpose)
        {
            foreach (var chartAxis in axes)
            {
                if (CanZoom(chartAxis, double.NaN, transpose))
                {
                    chartAxis.ZoomPosition = 0;
                    chartAxis.ZoomFactor = 1;
                    cumulativeZoomLevel = 1f;
                }
            }
        }

        private void OnDoubleTap(CartesianChartArea area, Rect clipRect, float pointX, float pointY)
        {
            if (EnableDoubleTap)
            {
                var clip = clipRect;
                var axislayout = area.AxisLayout;
                var xAxes = axislayout.HorizontalAxes;
                var yAxes = axislayout.VerticalAxes;

                float manipulationX = (float)(pointX - clip.Left);
                float manipulationY = (float)(pointY - clip.Top);

                float width = (float)clip.Width;
                float height = (float)clip.Height;

                if (CanReset(xAxes) || CanReset(yAxes))
                {
                    Reset(xAxes, area.IsTransposed);
                    Reset(yAxes, area.IsTransposed);
                }
                else
                {
                    foreach (var chartAxis in xAxes)
                    {
                        if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
                        {
                            var origin = GetOrigin(manipulationX, manipulationY, width, height, chartAxis);
                            Zoom(2.5f, origin, chartAxis);
                        }
                    }

                    foreach (var chartAxis in yAxes)
                    {
                        if (CanZoom(chartAxis, double.NaN, area.IsTransposed))
                        {
                            var origin = GetOrigin(manipulationX, manipulationY, width, height, chartAxis);

                            Zoom(2.5f, origin, chartAxis);
                        }
                    }

                    cumulativeZoomLevel = 2.5f;
                }
            }
        }

        private bool CanZoom(ChartAxis chartAxis, double angle, bool transposed)
        {
            bool canDirectionalZoom = ZoomMode == ZoomMode.XY;

            if (isPinchZoomingActivated && !double.IsNaN(angle) && EnableDirectionalZooming && canDirectionalZoom)
            {
                bool isXDirection = (angle >= 340 && angle <= 360) || (angle >= 0 && angle <= 20) || (angle >= 160 && angle <= 200);
                bool isYDirection = (angle >= 70 && angle <= 110) || (angle >= 250 && angle <= 290);
                bool isBothDirection = (angle > 20 && angle < 70) || (angle > 110 && angle < 160) || (angle > 200 && angle < 250) || (angle > 290 && angle < 340);

                canDirectionalZoom = (!chartAxis.IsVertical && isXDirection) || (chartAxis.IsVertical && isYDirection) || isBothDirection;
            }

            if (chartAxis.RegisteredSeries.Count > 0 && chartAxis.RegisteredSeries[0] != null
                && transposed)
            {
                if ((!chartAxis.IsVertical && ZoomMode == ZoomMode.Y) || (chartAxis.IsVertical && ZoomMode == ZoomMode.X) || canDirectionalZoom)
                {
                    return true;
                }
            }
            else
            {
                if ((chartAxis.IsVertical && ZoomMode == ZoomMode.Y) || (!chartAxis.IsVertical && ZoomMode == ZoomMode.X) || canDirectionalZoom)
                {
                    return true;
                }
            }

            return false;
        }

        private static float GetOrigin(float manipulationX, float manipulationY, float width, float height, ChartAxis chartAxis)
        {
            float origin;
            double plotOffsetStart = chartAxis.ActualPlotOffsetStart;
            double plotOffsetEnd = chartAxis.ActualPlotOffsetEnd;

            if (chartAxis.IsVertical)
            {
                origin = (float)(chartAxis.IsInversed
                    ? ((manipulationY - plotOffsetEnd) / height)
                    : 1 - ((manipulationY - plotOffsetStart) / height));
            }
            else
            {
                origin = (float)(chartAxis.IsInversed
                    ? 1.0 - ((manipulationX - plotOffsetEnd) / width)
                    : (manipulationX - plotOffsetStart) / width);
            }

            return origin;
        }

        private void PinchZoom(SfCartesianChart cartesianChart, Rect clipRect, Point scaleOrgin, double angle, double scale)
        {
            var clip = clipRect;
            var chartArea = cartesianChart.ChartArea;

            foreach (var chartAxis in chartArea.XAxes)
            {
                PinchZoom(chartAxis, scaleOrgin, angle, clip.Size, scale, chartArea.IsTransposed);
            }

            foreach (var chartAxis in chartArea.YAxes)
            {
                PinchZoom(chartAxis, scaleOrgin, angle, clip.Size, scale, chartArea.IsTransposed);
            }

            isPinchZoomingActivated = true;
        }

        private void PinchZoom(ChartAxis chartAxis, Point scaleOrgin, double angle, SizeF size, double scale, bool transpose)
        {
            if (CanZoom(chartAxis, angle, transpose))
            {
                var zoomFactor = chartAxis.ZoomFactor;
                var currentScale = (float)Math.Max(1f / ChartMath.MinMax(zoomFactor, 0f, 1f), 1f);
                currentScale *= (float)scale;
                if (!float.IsNaN(MaximumZoomLevel))
                {
                    currentScale = currentScale <= MaximumZoomLevel ? currentScale : MaximumZoomLevel;
                }

                var origin = GetOrigin((float)scaleOrgin.X, (float)scaleOrgin.Y, size.Width, size.Height, chartAxis);

                Zoom(currentScale, origin, chartAxis);
            }
        }

        private void PanTranslate(SfCartesianChart cartesianChart, Rect clipRect, Point translatePoint)
        {
            var area = cartesianChart.ChartArea;

            foreach (var axis in area.XAxes)
            {
                if (EnablePanning && !isPinchZoomingActivated && CanZoom(axis, double.NaN, area.IsTransposed))
                {
                    Translate(axis, clipRect, translatePoint, Math.Max(1 / ChartMath.MinMax(axis.ZoomFactor, 0, 1), 1));
                }
            }
            
            foreach (var axis in area.YAxes)
            {
                if (EnablePanning && !isPinchZoomingActivated && CanZoom(axis, double.NaN, area.IsTransposed))
                {
                    Translate(axis, clipRect, translatePoint, Math.Max(1 / ChartMath.MinMax(axis.ZoomFactor, 0, 1), 1));
                }
            }
        }

        private void Translate(ChartAxis axis, RectF clip, Point translatePoint, double currentScale)
        {
            double previousZoomPosition = axis.ZoomPosition;
            double currentZoomPosition;

            //Todo : Need to check the translate value with android and iOS.
            if(axis.IsVertical)
            {
                double offset = translatePoint.Y / clip.Height / currentScale;
#if MONOANDROID
                offset = axis.IsInversed ? previousZoomPosition + offset : previousZoomPosition - offset;
#else
                offset = axis.IsInversed ? previousZoomPosition - offset : previousZoomPosition + offset;
#endif
                currentZoomPosition = ChartMath.MinMax(offset, 0, 1 - axis.ZoomFactor);
            }
            else
            {
                double offset = translatePoint.X / clip.Width / currentScale;
#if MONOANDROID
                offset = axis.IsInversed ? previousZoomPosition - offset : previousZoomPosition + offset;
#else
                offset = axis.IsInversed ? previousZoomPosition + offset : previousZoomPosition - offset;
#endif
                currentZoomPosition = ChartMath.MinMax(offset, 0, 1 - axis.ZoomFactor);
            }

            if ((pinchStatus == GestureStatus.Completed || pinchStatus == GestureStatus.Canceled) && previousZoomPosition != currentZoomPosition)
            {
                axis.ZoomPosition = currentZoomPosition;
            }
        }
        
        /// <summary>
        /// Zoom at cumulative level to the corresponding origin.
        /// </summary>
        /// <param name="cumulativeLevel"></param>
        /// <param name="origin"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        private static bool Zoom(float cumulativeLevel, float origin, ChartAxis axis)
        {
            if (axis != null)
            {
                double calcZoomPos;
                double calcZoomFactor;

                if (cumulativeLevel == 1)
                {
                    calcZoomFactor = 1;
                    calcZoomPos = 0;
                }
                else
                {
                    calcZoomFactor = ChartMath.MinMax(1 / cumulativeLevel, 0, 1);
                    calcZoomPos = axis.ZoomPosition + ((axis.ZoomFactor - calcZoomFactor) * origin);
                }

                if (axis.ZoomPosition != calcZoomPos || axis.ZoomFactor != calcZoomFactor)
                {
                    axis.ZoomPosition = calcZoomPos;
                    axis.ZoomFactor = calcZoomPos + calcZoomFactor > 1 ? 1 - calcZoomPos : calcZoomFactor;
                    return true;
                }
            }

            return false;
        }

        private static void OnEnableDoubleTapPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnEnableZoomingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnEnablePanningPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnZoomModePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnMaximumZoomLevelPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }
        #endregion

        #endregion
    }
}
