using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;
using Windows.System;
using NativeColor = Windows.UI.Color;
using Microsoft.UI.Input;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// ZoomPanBehavior enables zooming and panning operations over a cartesian chart.
    /// </summary>
    /// <remarks>
    /// <para>To enable the zooming and panning in the chart, create an instance of <see cref="ChartZoomPanBehavior"/> and set it to the <c>ZoomPanBehavior</c> property of <see cref="SfCartesianChart"/>.</para>
    /// <para>It provides the following options to customize the chart zooming:</para>
    /// 
    /// <para><b>Zooming - </b> To zoom into the chart area, refer to the <see cref="EnablePinchZooming"/>, and <see cref="EnableMouseWheelZooming"/> properties.</para>
    /// <para> <b>ZoomMode - </b> To define the zooming direction, refer to the <see cref="ZoomMode"/> property.</para>
    /// <para> <b>EnablePanning - </b> To enable panning, refer to the <see cref="EnablePanning"/> property.</para>
    /// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
    /// </remarks>
    /// <example>
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    ///
    ///     <!--omitted for brevity-->
    ///
    ///     <chart:SfCartesianChart.ZoomPanBehavior>
    ///        <chart:ChartZoomPanBehavior EnablePanning = "True" EnablePinchZooming="True"/>
    ///     </chart:SfCartesianChart.ZoomPanBehavior>
    ///
    ///     <chart:LineSeries ItemsSource="{Binding Data}"
    ///                       XBindingPath="XValue"
    ///                       YBindingPath="YValue1"/>
    ///     <chart:LineSeries ItemsSource="{Binding Data}"
    ///                       XBindingPath="XValue"
    ///                       YBindingPath="YValue2"/>
    ///          
    /// </chart:SfCartesianChart>
    ///
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// ViewModel viewModel = new ViewModel();
    ///	
    /// // omitted for brevity
    /// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
    /// { 
    ///    EnablePinchZooming = true, 
    ///    EnablePanning = true,
    /// };
    /// 
    /// LineSeries series1 = new LineSeries()
    /// {
    ///    ItemsSource = viewModel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue1",
    /// };
    /// chart.Series.Add(series1);
    ///
    /// LineSeries series2 = new LineSeries()
    /// {
    ///    ItemsSource = viewmodel.Data,
    ///    XBindingPath = "XValue",
    ///    YBindingPath = "YValue2",
    /// };
    /// chart.Series.Add(series2);
    /// ]]>
    /// </code>
    /// ***
    /// </example>
    public class ChartZoomPanBehavior : ChartBehavior
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="ZoomRelativeToCursor"/> property.
        /// </summary>
        internal static readonly DependencyProperty ZoomRelativeToCursorProperty =
            DependencyProperty.Register(
                nameof(ZoomRelativeToCursor),
                typeof(bool),
                typeof(ChartZoomPanBehavior),
                new PropertyMetadata(true));

        /// <summary>
        /// The DependencyProperty for <see cref="EnablePinchZooming"/> property.
        /// </summary>
        public static readonly DependencyProperty EnablePinchZoomingProperty =
            DependencyProperty.Register(
                nameof(EnablePinchZooming),
                typeof(bool),
                typeof(ChartZoomPanBehavior),
                new PropertyMetadata(true));

        /// <summary>
        /// The DependencyProperty for <see cref="ZoomMode"/> property.
        /// </summary>
        public static readonly DependencyProperty ZoomModeProperty =
            DependencyProperty.Register(
                nameof(ZoomMode),
                typeof(ZoomMode),
                typeof(ChartZoomPanBehavior),
                new PropertyMetadata(ZoomMode.XY));

        /// <summary>
        /// The DependencyProperty for <see cref="EnableDirectionalZooming"/> property.
        /// </summary>
        public static readonly DependencyProperty EnableDirectionalZoomingProperty =
            DependencyProperty.Register(
                nameof(EnableDirectionalZooming),
                typeof(bool),
                typeof(ChartZoomPanBehavior),
                new PropertyMetadata(false));

        /// <summary>
        /// The DependencyProperty for <see cref="EnablePanning"/> property.
        /// </summary>
        public static readonly DependencyProperty EnablePanningProperty =
            DependencyProperty.Register(
                "EnablePanning",
                typeof(bool),
                typeof(ChartZoomPanBehavior),
                new PropertyMetadata(true));

        /// <summary>
        /// The DependencyProperty for <see cref="StrokeThickness"/> property.
        /// </summary>
        internal static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(
                nameof(StrokeThickness),
                typeof(double),
                typeof(ChartZoomPanBehavior),
                new PropertyMetadata(1d));

        /// <summary>
        /// The DependencyProperty for <see cref="MaximumZoomLevel"/> property.
        /// </summary>
        internal static readonly DependencyProperty MaximumZoomLevelProperty =
            DependencyProperty.Register(
                nameof(MaximumZoomLevel),
                typeof(double),
                typeof(ChartZoomPanBehavior),
                new PropertyMetadata(double.NaN));

        /// <summary>
        /// The DependencyProperty for <see cref="Stroke"/> property.
        /// </summary>
        internal static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register(
                nameof(Stroke),
                typeof(Brush),
                typeof(ChartZoomPanBehavior),
                null);

        /// <summary>
        /// The DependencyProperty for <see cref="Fill"/> property.
        /// </summary>
        internal static readonly DependencyProperty FillProperty =
            DependencyProperty.Register(
                nameof(Fill),
                typeof(Brush),
                typeof(ChartZoomPanBehavior),
                null);

        /// <summary>
        /// The DependencyProperty for <see cref="EnableSelectionZooming"/> property.
        /// </summary>
        internal static readonly DependencyProperty EnableSelectionZoomingProperty =
            DependencyProperty.Register(
                nameof(EnableSelectionZooming),
                typeof(bool),
                typeof(ChartZoomPanBehavior),
                new PropertyMetadata(false, new PropertyChangedCallback(OnZoomSelectionChanged)));

        /// <summary>
        /// The DependencyProperty for <see cref="ResetOnDoubleTap"/> property.
        /// </summary>
        internal static readonly DependencyProperty ResetOnDoubleTapProperty =
            DependencyProperty.Register(
                nameof(ResetOnDoubleTap),
                typeof(bool),
                typeof(ChartZoomPanBehavior),
                new PropertyMetadata(true));

        /// <summary>
        /// The DependencyProperty for <see cref="EnableMouseWheelZooming"/> property.
        /// </summary>
        public static readonly DependencyProperty EnableMouseWheelZoomingProperty =
            DependencyProperty.Register(
                nameof(EnableMouseWheelZooming),
                typeof(bool),
                typeof(ChartZoomPanBehavior),
                new PropertyMetadata(true));

        /// <summary>
        /// The DependencyProperty for <see cref="KeyModifiers"/> property.
        /// </summary>
        internal static readonly DependencyProperty KeyModifiersProperty =
            DependencyProperty.Register(
                nameof(KeyModifiers),
                typeof(VirtualKeyModifiers),
                typeof(ChartZoomPanBehavior),
                new PropertyMetadata(VirtualKeyModifiers.None));

        #endregion

        #region Fields

#region private Fields

        private bool enablePanning = true;

        private bool enableSelectionZooming = true;

        private Rectangle selectionRectangle;

        private ZoomChangingEventArgs zoomChangingEventArgs;

        private ZoomChangedEventArgs zoomChangedEventArgs;

        private PanChangingEventArgs panChangingEventArgs;

        private PanChangedEventArgs panChangedEventArgs;

        private SelectionZoomingStartEventArgs sel_ZoomingStartEventArgs;

        private SelectionZoomingDeltaEventArgs sel_ZoomingDeltaEventArgs;

        private SelectionZoomingEndEventArgs sel_ZoomingEndEventArgs;

        private ResetZoomEventArgs zoomingResetEventArgs;

        private bool isPanningChanged;

        private bool isReset;

        private Point startPoint;

        private bool isZooming;

        bool isTransposed;

        Rect zoomRect;

        private double previousScale;

        private Rect areaRect;

        private double angle;

        private List<PointerPoint> pointers = new List<PointerPoint>();

#endregion

#endregion

#region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartZoomPanBehavior"/>.
        /// </summary>
        public ChartZoomPanBehavior()
        {
            Fill = new SolidColorBrush(NativeColor.FromArgb(100, 210, 223, 242));
            Stroke = new SolidColorBrush(NativeColor.FromArgb(255,43, 87, 154));

            zoomChangingEventArgs = new ZoomChangingEventArgs();
            zoomChangedEventArgs = new ZoomChangedEventArgs();
            panChangedEventArgs = new PanChangedEventArgs();
            panChangingEventArgs = new PanChangingEventArgs();
            sel_ZoomingStartEventArgs = new SelectionZoomingStartEventArgs();
            sel_ZoomingDeltaEventArgs = new SelectionZoomingDeltaEventArgs();
            sel_ZoomingEndEventArgs = new SelectionZoomingEndEventArgs();
            zoomingResetEventArgs = new ResetZoomEventArgs();
            selectionRectangle = new Rectangle { IsHitTestVisible = false };
        }

#endregion

#region Properties

#region Public Properties


        /// <summary>
        /// 
        /// </summary>
        internal bool ZoomRelativeToCursor
        {
            get { return (bool)GetValue(ZoomRelativeToCursorProperty); }
            set { SetValue(ZoomRelativeToCursorProperty, value); }
        }

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

        /// <summary>
        /// Gets or sets a value indicating whether the directional zooming is enabled.
        /// </summary>
        /// <remarks>
        /// <para>If this property is false, zooming is performed based on <see cref="ZoomMode"/> property. If this property is true, zooming is performed based on pinch direction of the user.</para>
        /// <para>This property having effect only with <see cref="ZoomMode"/> value as <c>XY</c>.</para>
        /// </remarks>
        /// <value>
        /// It accepts the bool values and the default value is <c>false</c>.
        /// </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-7)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.ZoomPanBehavior>
        ///        <chart:ChartZoomPanBehavior EnableDirectionalZooming="True"/>
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
        /// # [MainPage.xaml.cs](#tab/tabid-8)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///	
        /// // omitted for brevity
        /// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
        /// { 
        ///    EnableDirectionalZooming = true, 
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
        public bool EnableDirectionalZooming
        {
            get { return (bool)GetValue(EnableDirectionalZoomingProperty); }
            set { SetValue(EnableDirectionalZoomingProperty, value); }
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
        /// 
        /// </summary>
        internal double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal double MaximumZoomLevel
        {
            get { return (double)GetValue(MaximumZoomLevelProperty); }
            set { SetValue(MaximumZoomLevelProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool EnableSelectionZooming
        {
            get { return (bool)GetValue(EnableSelectionZoomingProperty); }
            set { SetValue(EnableSelectionZoomingProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool ResetOnDoubleTap
        {
            get { return (bool)GetValue(ResetOnDoubleTapProperty); }
            set { SetValue(ResetOnDoubleTapProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the mouse wheel zooming is enabled.
        /// </summary>
        /// <value>
        /// It accepts the bool values and the default value is <c>true</c>.
        /// </value>
        /// <example>
        /// # [MainPage.xaml](#tab/tabid-11)
        /// <code><![CDATA[
        /// <chart:SfCartesianChart>
        ///
        ///     <!--omitted for brevity-->
        ///
        ///     <chart:SfCartesianChart.ZoomPanBehavior>
        ///        <chart:ChartZoomPanBehavior EnableMouseWheelZooming = "True" />
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
        /// # [MainPage.xaml.cs](#tab/tabid-12)
        /// <code><![CDATA[
        /// SfCartesianChart chart = new SfCartesianChart();
        /// ViewModel viewModel = new ViewModel();
        ///	
        /// // omitted for brevity
        /// chart.ZoomPanBehavior = new ChartZoomPanBehavior() 
        /// { 
        ///    EnableMouseWheelZooming = true,
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
        public bool EnableMouseWheelZooming
        {
            get { return (bool)GetValue(EnableMouseWheelZoomingProperty); }
            set { SetValue(EnableMouseWheelZoomingProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal VirtualKeyModifiers KeyModifiers
        {
            get { return (VirtualKeyModifiers)GetValue(KeyModifiersProperty); }
            set { SetValue(KeyModifiersProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal bool InternalEnablePanning
        {
            get { return enablePanning && EnablePanning; }
            set { enablePanning = value; }
        }
        
        internal bool InternalEnableSelectionZooming
        {
            get { return enableSelectionZooming && EnableSelectionZooming; }
            set { enableSelectionZooming = value; }
        }

#endregion

#region Private Properties

        private bool IsMaxZoomLevel
        {
            get { return !double.IsNaN(MaximumZoomLevel); }
        }
        
        private bool HorizontalMode
        {
            get { return (ZoomMode == ZoomMode.X && !isTransposed) || (ZoomMode == ZoomMode.Y && isTransposed); }
        }

        private bool VerticalMode
        {
            get { return (ZoomMode == ZoomMode.Y && !isTransposed) || (ZoomMode == ZoomMode.X && isTransposed); }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Virtual Methods

       
        internal virtual bool Zoom(double cumulativeScale, double origin, ChartAxis axis)
        {
            ////if (cumulativeScale >= 1d && cumulativeScale <= 4d)
            if (cumulativeScale >= 1d && axis != null)
            {
                double calcZoomPos = 0;
                double calcZoomFactor = 0;
                double previousPosition = axis.ZoomPosition;
                double previousFactor = axis.ZoomFactor;

                CalZoomFactors(cumulativeScale, origin, previousFactor, previousPosition, ref calcZoomPos, ref calcZoomFactor);

                var newZoomFactor = (calcZoomPos + calcZoomFactor) > 1 ? 1 - calcZoomPos : calcZoomFactor;

                RaiseZoomChangingEvent(calcZoomPos, newZoomFactor, axis);

                if (!zoomChangingEventArgs.Cancel && (axis.ZoomPosition != calcZoomPos || axis.ZoomFactor != calcZoomFactor))
                {
                    axis.ZoomPosition = calcZoomPos;
                    axis.ZoomFactor = newZoomFactor;
                    RaiseZoomChangedEvent(previousFactor, previousPosition, axis);
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Zooms the specified cumulative scale.
        /// </summary>
        /// <param name="cumulativeScale">The cumulative scale.</param>
        /// <param name="axis">The axis.</param>
        public bool Zoom(double cumulativeScale, ChartAxis axis)
        {
            return Zoom(cumulativeScale, 0.5, axis);
        }

        /// <summary>
        /// Resets the zoom factor and zoom position for all the axis.
        /// </summary>
        public void Reset()
        {
            if (Chart != null)
            {
                foreach (ChartAxis axis in Chart.InternalAxes)
                {
                    RaiseResetZoomingEvent(axis);

                    if (zoomingResetEventArgs.Cancel)
                        return;

                    axis.ZoomPosition = 0d;
                    axis.ZoomFactor = 1d;

                    RaiseZoomChangedEvent(axis.ZoomFactor, axis.ZoomPosition, axis);
                }
            }
        }

#endregion

#region Internal Methods
        
        internal void DisposeZoomEventArguments()
        {
            if (zoomChangedEventArgs != null)
            {
                zoomChangedEventArgs.Axis = null;
                zoomChangedEventArgs = null;
            }

            if (zoomChangingEventArgs != null)
            {
                zoomChangingEventArgs.Axis = null;
                zoomChangingEventArgs = null;
            }
    
            if (panChangedEventArgs != null)
            {
                panChangedEventArgs.Axis = null;
                panChangedEventArgs = null;
            }

            if (panChangingEventArgs != null)
            {
                panChangingEventArgs.Axis = null;
                panChangingEventArgs = null;
            }
        }

        #endregion

        #region Protected Internal Override Methods


        /// <inheritdoc />
        protected internal override void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
            if (Chart != null && Chart.SeriesClipRect.Contains(e.GetPosition(AdorningCanvas)))
            {
                isReset = true;
                isZooming = false;
            }
        }

        /// <inheritdoc />
        protected internal override void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
            if (Chart == null || Chart.AreaType != ChartAreaType.CartesianAxes)
                return;

            if (EnableMouseWheelZooming
                && e.Pointer.PointerDeviceType == PointerDeviceType.Mouse && this.KeyModifiers == e.KeyModifiers)
            {
                var point = e.GetCurrentPoint(AdorningCanvas);
                double direction = point.Properties.MouseWheelDelta > 0 ? 1 : -1;
                RemoveTooltip();
                MouseWheelZoom(point.Position, direction);
            }
        }

        /// <inheritdoc />
        protected internal override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if (Chart != null)
            {
                isPanningChanged = false;
                if (Chart.AreaType != ChartAreaType.CartesianAxes)
                    return;

                RemoveTooltip();
                if (EnableDirectionalZooming && EnablePinchZooming)
                {
                    pointers.Add(e.GetCurrentPoint(AdorningCanvas));
                    if (pointers.Count == 2)
                    {
                        angle = ChartMath.GetAngle(pointers[0].Position, pointers[1].Position);
                    }
                }

                if (InternalEnableSelectionZooming)
                {
                    var point = e.GetCurrentPoint(this.AdorningCanvas);

                    if (Chart.SeriesClipRect.Contains(point.Position))
                    {
                        zoomRect = Rect.Empty;
                        Canvas.SetLeft(selectionRectangle, point.Position.X);
                        Canvas.SetTop(selectionRectangle, point.Position.Y);

                        sel_ZoomingStartEventArgs.ZoomRect = zoomRect;

                        this.Chart.OnSelectionZoomingStart(sel_ZoomingStartEventArgs);

                        startPoint = point.Position;
                        AdorningCanvas.CapturePointer(e.Pointer);
                        isZooming = true;
                    }
                }
            }
        }

        /// <inheritdoc />
        protected internal override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            if (Chart != null)
            {
                if (pointers.Count == 2)
                {
                    var point = e.GetCurrentPoint(this.AdorningCanvas);

                    for (int i = 0; i < pointers.Count; i++)
                    {
                        if (pointers[i].PointerId == e.Pointer.PointerId)
                        {
                            pointers[i] = point;
                        }
                    }
                    
                    angle = ChartMath.GetAngle(pointers[0].Position, pointers[1].Position);
                }

                isTransposed = (from series in Chart.GetChartSeriesCollection() where series.IsActualTransposed select series).Any();

                if (isZooming)
                {                 
                    var point = e.GetCurrentPoint(this.AdorningCanvas);
                    Point pt = new Point(point.Position.X, point.Position.Y);

                    if (Chart.IsMultipleArea)
                    {
                        bool isPointInsideRect = false;
                        foreach (var axis in Chart.InternalAxes)
                        {
                            areaRect = ChartExtensionUtils.GetAxisArrangeRect(startPoint, axis, out isPointInsideRect);
                            if(isPointInsideRect)
                            {
                                break;
                            }
                        }

                        if(!isPointInsideRect)
                        {
                            areaRect = Chart.SeriesClipRect;
                        }

                        pt = ChartBehavior.ValidatePoint(pt, areaRect);
                        pt.X = ChartMath.MinMax(pt.X, areaRect.X, areaRect.Right);
                        pt.Y = ChartMath.MinMax(pt.Y, areaRect.Y, areaRect.Bottom);
                    }
                    else
                    {
                        areaRect = Chart.SeriesClipRect;
                        pt = ChartBehavior.ValidatePoint(pt, areaRect);
                        pt.X = ChartMath.MinMax(pt.X, 0, this.AdorningCanvas.Width);
                        pt.Y = ChartMath.MinMax(pt.Y, 0, this.AdorningCanvas.Height);
                    }

                    if (HorizontalMode)
                    {
                        zoomRect = new Rect(
                            new Point(startPoint.X, Chart.SeriesClipRect.Top),
                            new Point(pt.X, Chart.SeriesClipRect.Bottom));
                    }
                    else if (VerticalMode)
                    {
                        zoomRect = new Rect(
                            new Point(Chart.SeriesClipRect.Left, startPoint.Y),
                            new Point(Chart.SeriesClipRect.Right, pt.Y));
                    }
                    else
                    {
                        zoomRect = new Rect(startPoint, pt);
                    }

                    RaiseSelectionZoomDeltaEvent();

                    if (!sel_ZoomingDeltaEventArgs.Cancel)
                    {
                        selectionRectangle.Height = zoomRect.Height;
                        selectionRectangle.Width = zoomRect.Width;

                        Canvas.SetLeft(selectionRectangle, zoomRect.X);
                        Canvas.SetTop(selectionRectangle, zoomRect.Y);
                    }
                }
            }
        }

        /// <inheritdoc />
        protected internal override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            if (Chart != null)
            {
                foreach (ChartAxis axis in Chart.InternalAxes)
                {
                    if (isPanningChanged)
                        RaisePanChangedEvent(axis);
                }

                AdorningCanvas.ReleasePointerCapture(e.Pointer);

                if (isZooming)
                {
                    isZooming = false;
                    selectionRectangle.Width = 0;
                    selectionRectangle.Height = 0;

                    if (zoomRect.Width > 0 && zoomRect.Height > 0 && !sel_ZoomingDeltaEventArgs.Cancel)
                    {
                        foreach (ChartAxis axis in this.Chart.InternalAxes)
                        {
                            Zoom(zoomRect, axis);
                        }

                        sel_ZoomingEndEventArgs.ZoomRect = zoomRect;

                        this.Chart.OnSelectionZoomingEnd(sel_ZoomingEndEventArgs);
                    }
                }

                if (ResetOnDoubleTap && isReset)
                {
                    Reset();
                    isReset = false;
                }
            }

            foreach (var pointer in pointers)
            {
                if (pointer.PointerId == e.Pointer.PointerId)
                {
                    pointers.Remove(pointer);
                    return;
                }
            }
        }

        private bool CanDirectionalZoom(ChartAxis axis)
        {            
            if (EnableDirectionalZooming && ZoomMode == ZoomMode.XY)
            {
                bool isXDirection = (angle >= 340 && angle <= 360) || (angle >= 0 && angle <= 20) || (angle >= 160 && angle <= 200);
                bool isYDirection = (angle >= 70 && angle <= 110) || (angle >= 250 && angle <= 290);
                bool isBothDirection = (angle > 20 && angle < 70) || (angle > 110 && angle < 160) || (angle > 200 && angle < 250) || (angle > 290 && angle < 340);

                return (!axis.IsVertical && isXDirection) || (axis.IsVertical && isYDirection) || isBothDirection;
            }

            return ZoomMode == ZoomMode.XY;
        }

        /// <inheritdoc />
        protected internal override void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
            previousScale = e.Cumulative.Scale;
        }

        /// <inheritdoc />
        protected internal override void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {           
            if(Chart == null)
            {
                return;
            }

            if (Chart.HoldUpdate || Chart.AreaType != ChartAreaType.CartesianAxes)
                return;
            foreach (ChartAxis axis in this.Chart.InternalAxes)
            {
                ////double currentScale = ChartMath.MinMax(1 / ChartMath.MinMax(axis.ZoomFactor, 0, 1), 1, 4);

                double currentScale = Math.Max(1 / ChartMath.MinMax(axis.ZoomFactor, 0, 1), 1);

                double cumulativeScale = 0;

                double factor = currentScale * ((e.Cumulative.Scale - previousScale) / previousScale);

                if (EnablePinchZooming && e.Cumulative.Scale != previousScale &&
                    ((axis.IsVertical && VerticalMode) ||
                     (!axis.IsVertical && HorizontalMode) || CanDirectionalZoom(axis)))
                {
                    ////cumulativeScale = ChartMath.MinMax(currentScale + factor, 1, 4);
                    if (!double.IsNaN(factor) && !double.IsInfinity(factor))
                    {
                        cumulativeScale = ValMaxScaleLevel(Math.Max(currentScale + factor, 1));

                        if (cumulativeScale >= 1d)
                        {
                            double origin = 0.5;

                            if (!axis.IsVertical)
                            {
                                origin = e.Position.X / this.Chart.ActualWidth;
                            }
                            else
                            {
                                origin = 1 - (e.Position.Y / this.Chart.ActualHeight);
                            }

                            Zoom(cumulativeScale, origin, axis);
                        }
                    }
                }
                //The above condition failed and panning is activated at the end of pinch action. Checked e.Cumulative.Scale value to avoid panning at that time.
                else if (InternalEnablePanning && !isZooming && !isReset && e.Cumulative.Scale == 1)
                {
                    Translate(axis, e.Delta.Translation.X, e.Delta.Translation.Y, currentScale);
                }
            }

            previousScale = e.Cumulative.Scale;
        }

        /// <inheritdoc />
        protected internal override void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            if (Chart == null)
            {
                return;
            }

            foreach (ChartAxis axis in this.Chart.InternalAxes)
            {
                if (isPanningChanged)
                    RaisePanChangedEvent(axis);
            }
        }

        #endregion

#region Protected Override Methods

        internal override void AttachElements()
        {
            SelectionRectangleBinding();
            if (EnableSelectionZooming && this.AdorningCanvas != null &&
                !this.AdorningCanvas.Children.Contains(selectionRectangle))
            {
                this.AdorningCanvas.Children.Add(selectionRectangle);
            }
        }

#endregion

#region Private Static Methods

        private static void OnZoomSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChartZoomPanBehavior behavior)
            {
                if ((bool)e.NewValue)
                {
                    if (behavior.AdorningCanvas != null && !behavior.AdorningCanvas.Children.Contains(behavior.selectionRectangle))
                    {
                        behavior.AdorningCanvas.Children.Add(behavior.selectionRectangle);
                    }
                }
                else
                {
                    behavior.DetachElement(behavior.selectionRectangle);
                }
            }
        }

        private static void CalZoomFactors(
            double cumulativeScale,
            double origin,
            double currentZoomFactor,
            double currentZoomPos,
            ref double calcZoomPos,
            ref double calcZoomFactor)
        {
            if (cumulativeScale == 1)
            {
                calcZoomFactor = 1;
                calcZoomPos = 0;
            }
            else
            {
                calcZoomFactor = ChartMath.MinMax(1 / cumulativeScale, 0, 1);
                calcZoomPos = currentZoomPos + ((currentZoomFactor - calcZoomFactor) * origin);
            }
        }

#endregion

#region Private Methods

        private void SelectionRectangleBinding()
        {
            if (selectionRectangle != null)
            {
                Binding binding = new Binding();
                binding.Path = new PropertyPath("StrokeThickness");
                binding.Source = this;
                selectionRectangle.SetBinding(Rectangle.StrokeThicknessProperty, binding);

                binding = new Binding();
                binding.Path = new PropertyPath("Fill");
                binding.Source = this;
                selectionRectangle.SetBinding(Rectangle.FillProperty, binding);

                binding = new Binding();
                binding.Path = new PropertyPath("Stroke");
                binding.Source = this;
                selectionRectangle.SetBinding(Rectangle.StrokeProperty, binding);
            }
        }

        private void MouseWheelZoom(Point mousePoint, double direction)
        {
            if (Chart != null)
            {
                double origin;
                bool canUpdate = false;
                var seriesClipRect = Chart.SeriesClipRect;
                mousePoint = new Point(mousePoint.X - seriesClipRect.Left, mousePoint.Y - seriesClipRect.Top);

                bool canZoom = false;
                foreach (ChartAxis axis in this.Chart.InternalAxes)
                {
                    if (axis.RegisteredSeries.Count > 0 && (axis.RegisteredSeries[0] as CartesianSeries) != null 
                        && axis.RegisteredSeries[0] is CartesianSeries cartesianSeries && cartesianSeries.IsActualTransposed)
                    {
                        if ((!axis.IsVertical &&
                             (ZoomMode == ZoomMode.Y || ZoomMode == ZoomMode.XY)) ||
                            (axis.IsVertical &&
                             (ZoomMode == ZoomMode.X || ZoomMode == ZoomMode.XY)))
                        {
                            canZoom = true;
                        }
                    }
                    else
                    {
                        if ((axis.IsVertical &&
                             (ZoomMode == ZoomMode.Y || ZoomMode == ZoomMode.XY)) ||
                            (!axis.IsVertical &&
                             (ZoomMode == ZoomMode.X || ZoomMode == ZoomMode.XY)))
                        {
                            canZoom = true;
                        }
                    }

                    if (canZoom)
                    {
                        origin = 0.5;
                        ////double currentScale = ChartMath.MinMax(1 / ChartMath.MinMax(axis.ZoomFactor, 0, 1), 1, 4);

                        double currentScale = Math.Max(1 / ChartMath.MinMax(axis.ZoomFactor, 0, 1), 1);

                        ////double cumulativeScale = ChartMath.MinMax(currentScale + (0.25 * direction), 1, 4);

                        double cumulativeScale = ValMaxScaleLevel(Math.Max(currentScale + (0.25 * direction), 1));

                        if (ZoomRelativeToCursor)
                        {
                            if (!axis.IsVertical)
                                origin = mousePoint.X / seriesClipRect.Width;
                            else
                                origin = 1d - (mousePoint.Y / seriesClipRect.Height);
                        }

                        var value = axis.IsInversed ? 1d - origin : origin;
                        canUpdate = canUpdate | Zoom(cumulativeScale, value > 1d ? 1d : value < 0d ? 0d : value, axis);
                    }

                    canZoom = false;
                }

                if (canUpdate)
                    UpdateArea();
            }
        }

        private void RemoveTooltip()
        {
            var seriesCollection = Chart.GetSeriesCollection();
            foreach (ChartSeries series in seriesCollection)
            {
                series.RemoveTooltip();
            }
        }

        private void RaiseSelectionZoomDeltaEvent()
        {
            sel_ZoomingDeltaEventArgs.ZoomRect = zoomRect;
            sel_ZoomingDeltaEventArgs.Cancel = false;

            this.Chart.OnSelectionZoomingDelta(sel_ZoomingDeltaEventArgs);
        }

        private void Translate(ChartAxis axis, double translateX, double translateY, double currentScale)
        {
            double prevZoomPosition = axis.ZoomPosition;

            double offset, newZoomPosition;
            if (!axis.IsVertical)
            {
                offset = translateX / this.AdorningCanvas.ActualWidth / currentScale;
                newZoomPosition = ChartMath.MinMax(
                    axis.IsInversed == true? axis.ZoomPosition + offset : axis.ZoomPosition - offset, 
                    0, 
                    (1 - axis.ZoomFactor));
            }
            else
            {
                offset = translateY / this.AdorningCanvas.ActualHeight / currentScale;
                newZoomPosition = ChartMath.MinMax(
                    axis.IsInversed == true ? axis.ZoomPosition - offset: axis.ZoomPosition + offset, 
                    0, 
                    (1 - axis.ZoomFactor));
            }

            if (prevZoomPosition == newZoomPosition) return;

            RaisePanChangingEvent(axis, prevZoomPosition, newZoomPosition);

            if (!panChangingEventArgs.Cancel)
            {
                axis.ZoomPosition = newZoomPosition;
                isPanningChanged = true;
            }
        }

        private void RaisePanChangingEvent(ChartAxis axis, double preZoomPosition, double newZoomPosition)
        {
            panChangingEventArgs.Axis = axis;
            panChangingEventArgs.NewZoomPosition = newZoomPosition;
            panChangingEventArgs.OldZoomPosition = preZoomPosition;
            panChangingEventArgs.Cancel = false;

            this.Chart.OnPanChanging(panChangingEventArgs);
        }

        private void RaisePanChangedEvent(ChartAxis axis)
        {
            panChangedEventArgs.Axis = axis;
            panChangedEventArgs.NewZoomPosition = axis.ZoomPosition;

            this.Chart.OnPanChanged(panChangedEventArgs);
        }

        private void RaiseZoomChangedEvent(double zoomFactor, double zoomPosition, ChartAxis axis)
        {
            var newRange = axis.CalculateRange(axis.ActualRange, axis.ZoomPosition, axis.ZoomFactor);
            zoomChangedEventArgs.Axis = axis;
            zoomChangedEventArgs.CurrentFactor = axis.ZoomFactor;
            zoomChangedEventArgs.CurrentPosition = axis.ZoomPosition;
            if (axis is DateTimeAxis)
            {
                zoomChangedEventArgs.OldRange = new DateTimeRange(
                    axis.VisibleRange.Start.FromOADate(),
                    axis.VisibleRange.End.FromOADate());
                zoomChangedEventArgs.NewRange = new DateTimeRange(newRange.Start.FromOADate(), newRange.End.FromOADate());
            }
            else
            {
                zoomChangedEventArgs.OldRange = new DoubleRange(axis.VisibleRange.Start, axis.VisibleRange.End);
                zoomChangedEventArgs.NewRange = newRange;
            }

            zoomChangedEventArgs.PreviousFactor = zoomFactor;
            zoomChangedEventArgs.PreviousPosition = zoomPosition;

            this.Chart.OnZoomChanged(zoomChangedEventArgs);
        }

        private void RaiseZoomChangingEvent(double zoomPosition, double zoomFactor, ChartAxis axis)
        {
            zoomChangingEventArgs.Axis = axis;
            zoomChangingEventArgs.CurrentFactor = zoomFactor;
            zoomChangingEventArgs.PreviousFactor = axis.ZoomFactor;
            zoomChangingEventArgs.PreviousPosition = axis.ZoomPosition;
            zoomChangingEventArgs.CurrentPosition = zoomPosition;
            if (axis is DateTimeAxis)
            {
                zoomChangingEventArgs.OldRange = new DateTimeRange(axis.VisibleRange.Start.FromOADate(), axis.VisibleRange.End.FromOADate());
            }
            else
                zoomChangingEventArgs.OldRange = new DoubleRange(axis.VisibleRange.Start, axis.VisibleRange.End);
            zoomChangingEventArgs.Cancel = false;
            this.Chart.OnZoomChanging(zoomChangingEventArgs);
        }

        void Zoom(Rect zoomRect, ChartAxis axis)
        {
            double previousZoomFactor = axis.ZoomFactor;
            double previousZoomPosition = axis.ZoomPosition;
            double currentZoomFactor = 0d;
            var clipRect = new Rect();
            bool isPointInsideRect = false;
            double clipWidth = 0d;
            double clipHeight = 0d;

            if (!axis.IsVertical)
            {
                if (Chart.IsMultipleArea)
                {
                    ChartExtensionUtils.GetAxisArrangeRect(startPoint, axis, out isPointInsideRect);
                    if (!isPointInsideRect)
                    {
                        return;
                    }

                    clipWidth = areaRect.Width - axis.GetActualPlotOffset();
                    clipWidth = clipWidth > 0 ? clipWidth : 0;

                    clipRect = new Rect(
                        new Point(areaRect.X + axis.GetActualPlotOffsetStart(), areaRect.Y),
                        new Size(clipWidth, areaRect.Height));
                }
                else
                {
                    clipWidth = Chart.SeriesClipRect.Width - axis.GetActualPlotOffset();
                    clipWidth = clipWidth > 0 ? clipWidth : 0;

                    clipRect = new Rect(
                        new Point(Chart.SeriesClipRect.X + axis.GetActualPlotOffsetStart(), Chart.SeriesClipRect.Y),
                        new Size(clipWidth, Chart.SeriesClipRect.Height));
                }

                if (zoomRect.X < clipRect.X)
                {
                    var zoomWidth = zoomRect.Width - (clipRect.X - zoomRect.X);
                    zoomWidth = zoomWidth > 0 ? zoomWidth : ((StrokeThickness) > 0) ? StrokeThickness : 1;
                    zoomRect = new Rect(new Point(clipRect.X, zoomRect.Y), new Size(zoomWidth, zoomRect.Height));
                }

                if (zoomRect.Right > clipRect.Right)
                {
                    var zoomWidth = zoomRect.Width - (zoomRect.Right - clipRect.Right);
                    zoomWidth = zoomWidth > 0 ? zoomWidth : ((StrokeThickness) > 0) ? StrokeThickness : 1;
                    zoomRect = new Rect(new Point(zoomRect.X, zoomRect.Y), new Size(zoomWidth, zoomRect.Height));
                }

                currentZoomFactor = previousZoomFactor * (zoomRect.Width / clipRect.Width);

                if (currentZoomFactor != previousZoomFactor && ValMaxZoomLevel(currentZoomFactor))
                {
                    axis.ZoomFactor = !VerticalMode
                    ? currentZoomFactor
                    : 1;
                    axis.ZoomPosition = !VerticalMode
                        ? (previousZoomPosition +

                       Math.Abs((axis.IsInversed ? clipRect.Right - zoomRect.Right
                                                  : zoomRect.X - clipRect.Left)
                                                  / clipRect.Width) * previousZoomFactor) : 0;
                }
            }
            else
            {
                if (Chart.IsMultipleArea)
                {
                    ChartExtensionUtils.GetAxisArrangeRect(startPoint, axis, out isPointInsideRect);

                    if (!isPointInsideRect)
                    {
                        return;
                    }

                    clipHeight = areaRect.Height - axis.GetActualPlotOffset();
                    clipHeight = clipHeight > 0 ? clipHeight : 0;

                    clipRect = new Rect(
                               new Point(areaRect.X, areaRect.Y + axis.GetActualPlotOffsetEnd()),
                               new Size(areaRect.Width, clipHeight));
                }
                else
                {
                    clipHeight = Chart.SeriesClipRect.Height - axis.GetActualPlotOffset();
                    clipHeight = clipHeight > 0 ? clipHeight : 0;

                    clipRect = new Rect(
                               new Point(Chart.SeriesClipRect.X, Chart.SeriesClipRect.Y + axis.GetActualPlotOffsetEnd()),
                               new Size(Chart.SeriesClipRect.Width, clipHeight));
                }

                if (zoomRect.Y < clipRect.Y)
                {
                    var zoomHeight = zoomRect.Height - (clipRect.Y - zoomRect.Y);
                    zoomHeight = zoomHeight > 0 ? zoomHeight : StrokeThickness > 0 ? StrokeThickness : 1;
                    zoomRect = new Rect(new Point(zoomRect.X, clipRect.Y), new Size(zoomRect.Width, zoomHeight));
                }

                if (clipRect.Bottom < zoomRect.Bottom)
                {
                    var zoomHeight = zoomRect.Height - (zoomRect.Bottom - clipRect.Bottom);
                    zoomHeight = zoomHeight > 0 ? zoomHeight : StrokeThickness > 0 ? StrokeThickness : 1;
                    zoomRect = new Rect(new Point(zoomRect.X, zoomRect.Y), new Size(zoomRect.Width, zoomHeight));
                }

                currentZoomFactor = previousZoomFactor * zoomRect.Height / clipRect.Height;

                if (currentZoomFactor != previousZoomFactor && ValMaxZoomLevel(currentZoomFactor))
                {
                    axis.ZoomFactor = !HorizontalMode
                   ? currentZoomFactor
                   : 1;
                    axis.ZoomPosition = !HorizontalMode
                        ? previousZoomPosition +
                       (1 - Math.Abs(((axis.IsInversed ? zoomRect.Top - clipRect.Bottom
                                                         : zoomRect.Bottom - clipRect.Top))
                                                         / clipRect.Height)) * previousZoomFactor : 0;
                }
            }
        }

        internal bool CanZoom(ChartAxis axis)
        {
            bool canZoom = false;

            if (axis.RegisteredSeries.Count > 0 && (axis.RegisteredSeries[0] as CartesianSeries) != null 
               && axis.RegisteredSeries[0] is CartesianSeries cartesianSeries && cartesianSeries.IsActualTransposed)
            {
                if ((!axis.IsVertical && (ZoomMode == ZoomMode.Y || ZoomMode == ZoomMode.XY)) ||
                    (axis.IsVertical && (ZoomMode == ZoomMode.X || ZoomMode == ZoomMode.XY)))
                    canZoom = true;
            }
            else
            {
                if ((axis.IsVertical && (ZoomMode == ZoomMode.Y || ZoomMode == ZoomMode.XY)) ||
                    (!axis.IsVertical && (ZoomMode == ZoomMode.X || ZoomMode == ZoomMode.XY)))
                    canZoom = true;
            }

            return canZoom;
        }

        private void RaiseResetZoomingEvent(ChartAxis axis)
        {
            zoomingResetEventArgs.Axis = axis;
            if (axis is DateTimeAxis)
                zoomingResetEventArgs.PreviousZoomRange = new DateTimeRange(
                    axis.VisibleRange.Start.FromOADate(),
                    axis.VisibleRange.End.FromOADate());
            else
                zoomingResetEventArgs.PreviousZoomRange = new DoubleRange(axis.VisibleRange.Start, axis.VisibleRange.End);

            this.zoomingResetEventArgs.Cancel = false;
            this.Chart.OnResetZoom(zoomingResetEventArgs);
        }

        bool ValMaxZoomLevel(double currentZoomFactor)
        {
            if (IsMaxZoomLevel) // Selection Zooming not select the correct region -WPF-18131
                return ((1 / currentZoomFactor) <= MaximumZoomLevel ? true : false);
            return true;
        }

        double ValMaxScaleLevel(double cumulativeScale)
        {
            if (IsMaxZoomLevel)
                return ((cumulativeScale <= MaximumZoomLevel) ? cumulativeScale : MaximumZoomLevel);
            return cumulativeScale;
        }
 
#endregion

#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class ZoomEventArgs : EventArgs
    {
        #region Properties

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public double PreviousPosition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double CurrentPosition { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object OldRange { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double PreviousFactor { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public double CurrentFactor { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ChartAxis Axis { get; set; }

#endregion

#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class ZoomChangingEventArgs : ZoomEventArgs
    {
#region Properties

#region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public bool Cancel { get; set; }

#endregion

#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class ZoomChangedEventArgs : ZoomEventArgs
    {
#region Properties

#region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public object NewRange { get; internal set; }

#endregion

#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class SelectionZoomingEventArgs : EventArgs
    {
#region Properties

#region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public Rect ZoomRect { get; set; }
        
#endregion

#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class SelectionZoomingStartEventArgs : SelectionZoomingEventArgs
    {
    }

    /// <summary>
    /// 
    /// </summary>
    internal class SelectionZoomingDeltaEventArgs : SelectionZoomingEventArgs
    {
#region Properties

#region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public bool Cancel { get; set; }

#endregion

#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class SelectionZoomingEndEventArgs : SelectionZoomingEventArgs
    {
    }

    /// <summary>
    /// 
    /// </summary>
    internal class PanningEventArgs : EventArgs
    {
#region Properties

#region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public double NewZoomPosition { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ChartAxis Axis { get; internal set; }
        
#endregion

#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class PanChangingEventArgs : PanningEventArgs
    {
#region Properties

#region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public double OldZoomPosition { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Cancel { get; set; }

#endregion

#endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal class PanChangedEventArgs : PanningEventArgs
    {
    }

    /// <summary>
    /// 
    /// </summary>
    internal class ResetZoomEventArgs : EventArgs
    {
#region Properties

#region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public object PreviousZoomRange { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public ChartAxis Axis { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Cancel { get; set; }

#endregion

#endregion
    }
}
