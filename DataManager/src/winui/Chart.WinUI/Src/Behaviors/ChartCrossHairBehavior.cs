using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// ChartCrosshairBehavior enables viewing of information related to chart coordinates at the mouse hover position or at the touch contact point inside a chart.
    /// </summary>
    /// <remarks>
    /// <para>To add the crosshair in the chart, create an instance of <see cref="ChartCrosshairBehavior"/> and set it to the <see cref="SfCartesianChart.CrosshairBehavior"/> property of the chart.</para>
    /// <para>ChartCrosshairBehavior displays a vertical line, horizontal line, and a popup-like control displaying information about the data point at the touch contact point or at the mouse hover position.</para>
    /// <para>It provides options to customize the appearance of vertical and horizontal lines.</para>
    ///
    /// <b>HorizontalLineStyle</b>
    /// 
    /// <para>It is used to change the look of the horizontal line in the Crosshair.</para>
    ///
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <!--omitted for brevity-->
    ///           
    ///           <chart:SfCartesianChart.CrosshairBehavior>
    ///               <chart:ChartCrosshairBehavior>
    ///                   <chart:ChartCrosshairBehavior.HorizontalLineStyle>
    ///                       <Style TargetType="Line">
    ///                           <Setter Property="StrokeDashArray" Value="5,2"/>
    ///                           <Setter Property="Stroke" Value="Red"/>
    ///                   </Style>
    ///                </chart:ChartCrosshairBehavior.HorizontalLineStyle>
    ///            </chart:ChartCrosshairBehavior>
    ///           </chart:SfCartesianChart.CrosshairBehavior>
    ///           
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     
    ///     // omitted for brevity
    ///     DoubleCollection doubleCollection = new DoubleCollection();
    ///     doubleCollection.Add(5);
    ///     doubleCollection.Add(3);
    ///     var lineStyle = new Style() { TargetType = typeof(Line) };
    ///     lineStyle.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Red)));
    ///     lineStyle.Setters.Add(new Setter(Path.StrokeDashArrayProperty, doubleCollection));
    ///     chart.CrosshairBehavior = new ChartCrosshairBehavior()
    ///     {
    ///        HorizontalLineStyle = lineStyle,
    ///     };
    ///
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <b>VerticalLineStyle</b>
    /// 
    /// <para>It is used to change the look of the vertical line in the Crosshair.</para>
    ///
    /// # [Xaml](#tab/tabid-3)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <!--omitted for brevity-->
    ///
    ///           <chart:SfCartesianChart.CrosshairBehavior>
    ///               <chart:ChartCrosshairBehavior>
    ///                   <chart:ChartCrosshairBehavior.VerticalLineStyle>
    ///                       <Style TargetType="Line">
    ///                           <Setter Property="StrokeDashArray" Value="5,2"/>
    ///                           <Setter Property="Stroke" Value="Red"/>
    ///                   </Style>
    ///                </chart:ChartCrosshairBehavior.VerticalLineStyle>
    ///            </chart:ChartCrosshairBehavior>
    ///           </chart:SfCartesianChart.CrosshairBehavior>
    ///
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-4)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     
    ///     // omitted for brevity
    ///     DoubleCollection doubleCollection = new DoubleCollection();
    ///     doubleCollection.Add(5);
    ///     doubleCollection.Add(3);
    ///     var lineStyle = new Style() { TargetType = typeof(Line) };
    ///     lineStyle.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Red)));
    ///     lineStyle.Setters.Add(new Setter(Path.StrokeDashArrayProperty, doubleCollection));
    ///     chart.CrosshairBehavior = new ChartCrosshairBehavior()
    ///     {
    ///        VerticalLineStyle = lineStyle,
    ///     };
    ///
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <b>ShowTrackballLabel</b>
    ///
    /// <para>The axis label will be viewed when the ShowTrackballLabel property is set to true. The default value of ShowTrackballLabel is false</para>
    ///
    /// # [Xaml](#tab/tabid-5)
    /// <code><![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.XAxes>
    ///               <chart:NumericalAxis ShowTrackballLabel="True"/>
    ///           </chart:SfCartesianChart.XAxes>
    ///
    ///           <chart:SfCartesianChart.YAxes>
    ///               <chart:NumericalAxis ShowTrackballLabel="True"/>
    ///           </chart:SfCartesianChart.YAxes>
    ///
    ///           <chart:SfCartesianChart.CrosshairBehavior>
    ///               <chart:ChartCrosshairBehavior />
    ///           </chart:SfCartesianChart.CrosshairBehavior>
    ///
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [C#](#tab/tabid-6)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///
    ///     NumericalAxis xAxis = new NumericalAxis();
    ///     xAxis.ShowTrackballLabel = true;
    ///     NumericalAxis yAxis = new NumericalAxis();
    ///     yAxis.ShowTrackballLabel = true;
    ///
    ///     chart.XAxes.Add(xAxis);
    ///     chart.YAxes.Add(yAxis);
    ///
    ///     chart.CrosshairBehavior = new ChartCrosshairBehavior();
    ///
    /// ]]>
    /// </code>
    /// ***
    ///
    /// <b>CrosshairLabelTemplate</b>
    ///
    /// <para>The appearance of the crosshair axis labels can be customized by using the CrosshairLabelTemplate property of chart axis.</para>
    ///
    /// # [Xaml](#tab/tabid-7)
    /// <code>
    /// <![CDATA[
    ///     <chart:SfCartesianChart>
    ///
    ///           <chart:SfCartesianChart.XAxes>
    ///               <chart:NumericalAxis ShowTrackballLabel="True">
    ///                <chart:NumericalAxis.CrosshairLabelTemplate>
    ///                    <DataTemplate>
    ///                        <Border Background = "Orange" CornerRadius="4"    
    ///                                BorderThickness="1" BorderBrush="Black">
    ///                            <TextBlock Margin = "2" Text="{Binding ValueX}"/>
    ///                        </Border>
    ///                    </DataTemplate>
    ///                </chart:NumericalAxis.CrosshairLabelTemplate>
    ///            </chart:NumericalAxis>
    ///           </chart:SfCartesianChart.XAxes>
    ///
    ///           <chart:SfCartesianChart.YAxes>
    ///               <chart:NumericalAxis ShowTrackballLabel="True">
    ///                <chart:NumericalAxis.CrosshairLabelTemplate>
    ///                    <DataTemplate>
    ///                        <Border Background = "Orange" CornerRadius="4"    
    ///                                BorderThickness="1" BorderBrush="Black">
    ///                            <TextBlock Margin = "2" Text="{Binding ValueY}"/>
    ///                        </Border>
    ///                    </DataTemplate>
    ///                </chart:NumericalAxis.CrosshairLabelTemplate>
    ///            </chart:NumericalAxis>
    ///           </chart:SfCartesianChart.YAxes>
    ///
    ///           <chart:SfCartesianChart.CrosshairBehavior>
    ///               <chart:ChartCrosshairBehavior />
    ///           </chart:SfCartesianChart.CrosshairBehavior>
    ///
    ///     </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// *** 
    ///
    /// <para> <b>Note:</b> This is only applicable for <see cref="SfCartesianChart"/>.</para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    public class ChartCrosshairBehavior : ChartBehavior
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="VerticalAxisLabelAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty VerticalAxisLabelAlignmentProperty =
            DependencyProperty.Register(
                nameof(VerticalAxisLabelAlignment),
                typeof(ChartAlignment),
                typeof(ChartCrosshairBehavior),
                new PropertyMetadata(ChartAlignment.Center));

        /// <summary>
        ///  The DependencyProperty for <see cref="HorizontalAxisLabelAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty HorizontalAxisLabelAlignmentProperty =
            DependencyProperty.Register(
                nameof(HorizontalAxisLabelAlignment),
                typeof(ChartAlignment),
                typeof(ChartCrosshairBehavior),
                new PropertyMetadata(ChartAlignment.Center));

        /// <summary>
        /// The DependencyProperty for <see cref="HorizontalLineStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty HorizontalLineStyleProperty =
            DependencyProperty.Register(
                nameof(HorizontalLineStyle),
                typeof(Style),
                typeof(ChartCrosshairBehavior),
                new PropertyMetadata(ChartDictionaries.GenericCommonDictionary["SyncfusionChartCrosshairLineStyle"]));

        /// <summary>
        /// The DependencyProperty for <see cref="VerticalLineStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty VerticalLineStyleProperty =
            DependencyProperty.Register(
                nameof(VerticalLineStyle),
                typeof(Style),
                typeof(ChartCrosshairBehavior),
                new PropertyMetadata(ChartDictionaries.GenericCommonDictionary["SyncfusionChartCrosshairLineStyle"]));

        #endregion

        #region Fields

        #region Protected Internal Fields

        internal Point CurrentPoint;

        #endregion

        #region Private Fields

        private Line verticalLine;
        private Line horizontalLine;

        private int fingerCount = 0;
        private bool isActivated;
        private List<ContentControl> labelElements;
        private ObservableCollection<ChartPointInfo> pointInfos;
        private List<FrameworkElement> elements;
        private string? labelXValue;
        private string? labelYValue;

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartCrosshairBehavior"/> class.
        /// </summary>
        public ChartCrosshairBehavior()
        {
            elements = new List<FrameworkElement>();
            verticalLine = new Line();
            horizontalLine = new Line();
            labelElements = new List<ContentControl>();
            pointInfos = new ObservableCollection<ChartPointInfo>();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the <see cref="ChartAlignment"/> for the label appearing in vertical axis.
        /// </summary>
        /// <value>This property takes the <see cref="ChartAlignment"/> value and its default value is <see cref="ChartAlignment.Center"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-8)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///           
        ///           <chart:SfCartesianChart.CrosshairBehavior>
        ///               <chart:ChartCrosshairBehavior VerticalAxisLabelAlignment="Near"/>
        ///           </chart:SfCartesianChart.CrosshairBehavior>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-9)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     // omitted for brevity
        ///     chart.CrosshairBehavior = new ChartCrosshairBehavior()
        ///     {
        ///        VerticalAxisLabelAlignment = ChartAlignment.Near,
        ///     };
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartAlignment VerticalAxisLabelAlignment
        {
            get { return (ChartAlignment)GetValue(VerticalAxisLabelAlignmentProperty); }
            set { SetValue(VerticalAxisLabelAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the <see cref="ChartAlignment"/> for the label appearing in the horizontal axis.
        /// </summary>
        /// <value>This property takes the <see cref="ChartAlignment"/> value and its default value is <see cref="ChartAlignment.Center"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-10)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///           
        ///           <chart:SfCartesianChart.CrosshairBehavior>
        ///               <chart:ChartCrosshairBehavior HorizontalAxisLabelAlignment="Near"/>
        ///           </chart:SfCartesianChart.CrosshairBehavior>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-11)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     
        ///     // omitted for brevity
        ///     chart.CrosshairBehavior = new ChartCrosshairBehavior()
        ///     {
        ///        HorizontalAxisLabelAlignment = ChartAlignment.Near,
        ///     };
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public ChartAlignment HorizontalAxisLabelAlignment
        {
            get { return (ChartAlignment)GetValue(HorizontalAxisLabelAlignmentProperty); }
            set { SetValue(HorizontalAxisLabelAlignmentProperty, value); }
        }

        internal ObservableCollection<ChartPointInfo> PointInfos
        {
            get
            {
                return pointInfos;
            }

            set
            {
                pointInfos = value;
            }
        }

        /// <summary>
        /// Gets or sets the style that can be used to customize the appearance of the horizontal line.
        /// </summary>
        /// <value>It takes <see cref="Style"/> value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-12)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///
        ///           <chart:SfCartesianChart.CrosshairBehavior>
        ///               <chart:ChartCrosshairBehavior>
        ///                   <chart:ChartCrosshairBehavior.HorizontalLineStyle>
        ///                       <Style TargetType="Line">
        ///                           <Setter Property="StrokeDashArray" Value="5,2"/>
        ///                           <Setter Property="Stroke" Value="Red"/>
        ///                   </Style>
        ///                </chart:ChartCrosshairBehavior.HorizontalLineStyle>
        ///            </chart:ChartCrosshairBehavior>
        ///           </chart:SfCartesianChart.CrosshairBehavior> 
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-13)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///
        ///     // omitted for brevity
        ///     DoubleCollection doubleCollection = new DoubleCollection(){ 5, 2 };
        ///     var lineStyle = new Style() { TargetType = typeof(Line) };
        ///     lineStyle.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Red)));
        ///     lineStyle.Setters.Add(new Setter(Path.StrokeDashArrayProperty, doubleCollection));
        ///     chart.CrosshairBehavior = new ChartCrosshairBehavior()
        ///     {
        ///        HorizontalLineStyle = lineStyle,
        ///     };
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public Style HorizontalLineStyle
        {
            get { return (Style)GetValue(HorizontalLineStyleProperty); }
            set { SetValue(HorizontalLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style that can be used to customize the appearance of vertical line.
        /// </summary>
        /// <value>It takes <see cref="Style"/> value.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-14)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///           <!--omitted for brevity-->
        ///
        ///           <chart:SfCartesianChart.CrosshairBehavior>
        ///               <chart:ChartCrosshairBehavior>
        ///                   <chart:ChartCrosshairBehavior.VerticalLineStyle>
        ///                       <Style TargetType="Line">
        ///                           <Setter Property="StrokeDashArray" Value="5,2"/>
        ///                           <Setter Property="Stroke" Value="Red"/>
        ///                   </Style>
        ///                </chart:ChartCrosshairBehavior.VerticalLineStyle>
        ///            </chart:ChartCrosshairBehavior>
        ///           </chart:SfCartesianChart.CrosshairBehavior>
        ///           
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-15)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///
        ///     // omitted for brevity
        ///     DoubleCollection doubleCollection = new DoubleCollection(){ 5, 2 };
        ///     var lineStyle = new Style() { TargetType = typeof(Line) };
        ///     lineStyle.Setters.Add(new Setter(Path.StrokeProperty, new SolidColorBrush(Colors.Red)));
        ///     lineStyle.Setters.Add(new Setter(Path.StrokeDashArrayProperty, doubleCollection));
        ///     chart.CrosshairBehavior = new ChartCrosshairBehavior()
        ///     {
        ///        VerticalLineStyle = lineStyle,
        ///     };
        ///
        /// ]]>
        /// </code>
        /// *** 
        /// </example>
        public Style VerticalLineStyle
        {
            get { return (Style)GetValue(VerticalLineStyleProperty); }
            set { SetValue(VerticalLineStyleProperty, value); }
        }

        #endregion

        #region Protected Internal Properties

        internal bool IsActivated
        {
            get
            {
                return isActivated;
            }

            set
            {
                isActivated = value;
                Activate(isActivated);
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Protected Internal Override Methods

        internal override void DetachElements()
        {
            if (this.AdorningCanvas != null)
            {
                foreach (var element in elements)
                {
                    this.AdorningCanvas.Children.Remove(element);
                }

                elements.Clear();
            }
        }

        internal override void OnSizeChanged(SizeChangedEventArgs e)
        {
            if (Chart != null && !string.IsNullOrEmpty(labelXValue) && !string.IsNullOrEmpty(labelYValue))
            {
                double y1 = this.Chart.ActualValueToPoint(this.Chart.InternalSecondaryAxis, (Convert.ToDouble(labelYValue)));
                double x1 = this.Chart.ActualValueToPoint(this.Chart.InternalPrimaryAxis, (Convert.ToDouble(labelXValue)));
                if (!double.IsNaN(y1) && !double.IsNaN(x1))
                {
                    foreach (ContentControl control in labelElements)
                    {
                        DetachElement(control);
                    }

                    this.labelElements.Clear();
                    this.pointInfos.Clear();
                    elements.Clear();
                    this.SetPosition(new Point(x1, y1));
                }
            }
        }

        /// <inheritdoc />
        protected internal override void OnHolding(HoldingRoutedEventArgs e)
        {
            if(Chart == null)
            {
                return;
            }

            IsActivated = true;

            if (e.PointerDeviceType == PointerDeviceType.Touch)
                Chart.HoldUpdate = true;
            if (this.Chart != null && this.Chart.VisibleSeries.Count > 0 && this.Chart.VisibleSeries[0] is CartesianSeries && IsActivated)
            {
                Point point = e.GetPosition(this.AdorningCanvas);

                if (this.Chart.SeriesClipRect.Contains(point))
                {
                    foreach (ContentControl control in labelElements)
                    {
                        DetachElement(control);
                    }

                    labelElements.Clear();

                    pointInfos.Clear();

                    elements.Clear();
                    SetPosition(point);
                }
            }
        }

        /// <inheritdoc />
        protected internal override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                IsActivated = false;
            else
                fingerCount++;
        }

        /// <inheritdoc />
        protected internal override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(this.AdorningCanvas);
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
                if (fingerCount > 1) return;
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse &&
                 !pointer.Properties.IsLeftButtonPressed)
                IsActivated = true;

            if (this.Chart != null && this.Chart.AreaType == ChartAreaType.CartesianAxes && IsActivated)
            {
                CurrentPoint = new Point(pointer.Position.X, pointer.Position.Y);

                if (this.Chart.SeriesClipRect.Contains(CurrentPoint))
                {
                    foreach (ContentControl control in labelElements)
                    {
                        DetachElement(control);
                    }

                    labelElements.Clear();

                    pointInfos.Clear();

                    elements.Clear();
                    SetPosition(CurrentPoint);
                }
                else
                {
                    IsActivated = false;
                }
            }
        }

        /// <inheritdoc />
        protected internal override void OnPointerExited(PointerRoutedEventArgs e)
        {
            if (IsActivated)
            {
                IsActivated = false;
            }

            fingerCount = 0;
        }

        internal override void OnLayoutUpdated()
        {
            if (this.Chart == null)
            {
                return;
            }

            if (IsActivated)
            {
                foreach (ContentControl control in labelElements)
                {
                    DetachElement(control);
                }

                labelElements.Clear();

                pointInfos.Clear();

                if (this.Chart.SeriesClipRect.Contains(CurrentPoint))
                {
                    SetPosition(CurrentPoint);
                }
                else
                {
                    foreach (var element in elements)
                    {
                        element.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        /// <inheritdoc />
        protected internal override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            if (Chart == null)
            {
                return;
            }

            if (e.Pointer.PointerDeviceType != PointerDeviceType.Mouse)
            {
                if (IsActivated)
                {
                    IsActivated = false;
                    Chart.HoldUpdate = false;
                }

                fingerCount--;
            }
        }

        internal override void AlignDefaultLabel(
            ChartAlignment verticalAlignemnt, 
            ChartAlignment horizontalAlignment,
            double x, 
            double y, 
            ContentControl control)
        {
            if ((control as ContentControl).Content is ChartPointInfo content)
            {
                if (horizontalAlignment == ChartAlignment.Near)
                {
                    x = x - control.DesiredSize.Width;
                    if (control is ContentControl)
                        content.X = x;
                }
                else if (horizontalAlignment == ChartAlignment.Center)
                {
                    x = x - control.DesiredSize.Width / 2;
                    if (control is ContentControl)
                        content.X = x;
                }

                if (verticalAlignemnt == ChartAlignment.Near)
                {
                    y = y - control.DesiredSize.Height;
                    if (control is ContentControl)
                        content.Y = y;
                }
                else if (verticalAlignemnt == ChartAlignment.Center)
                {
                    y = y - control.DesiredSize.Height / 2;
                    if (control is ContentControl)
                        content.Y = y;
                }
            }

            Canvas.SetLeft(control, x);
            Canvas.SetTop(control, y);
        }

        #endregion

        #region Protected Internal Virtual Methods

        internal virtual void SetPosition(Point point)
        {
            if (AdorningCanvas == null || double.IsNaN(point.X) || double.IsNaN(point.Y) || !IsActivated) return;

            var seriesLeft = Chart.SeriesClipRect.Left;
            var seriesTop = Chart.SeriesClipRect.Top;

            double x = point.X;
            double y = point.Y;

            foreach (var element in elements)
            {
                element.Visibility = Visibility.Visible;
            }

            verticalLine.X1 = verticalLine.X2 = x > this.Chart.SeriesClipRect.Right ? this.Chart.SeriesClipRect.Right : x;
            verticalLine.Y1 = seriesTop;
            verticalLine.Y2 = this.Chart.SeriesClipRect.Height + seriesTop;
            elements.Add(verticalLine);

            horizontalLine.Y1 = horizontalLine.Y2 = y;
            horizontalLine.X1 = seriesLeft;
            horizontalLine.X2 = seriesLeft + this.Chart.SeriesClipRect.Width;
            elements.Add(horizontalLine);

            foreach (ChartAxis axis in Chart.InternalAxes)
            {
                if ((axis.RenderedRect.Left <= point.X && axis.RenderedRect.Right >= point.X)
                    || axis.RenderedRect.Top <= point.Y && axis.RenderedRect.Bottom >= point.Y)
                {
                    double val = this.Chart.ActualPointToValue(axis, new Point(point.X - seriesLeft, point.Y - seriesTop));
                    if (!double.IsNaN(val))
                    {
                        ChartPointInfo pointInfo = new ChartPointInfo();
                        pointInfo.Axis = axis;
                        var isDateTimeAxis = axis is DateTimeAxis;
                        var labelContent = axis.GetLabelContent(val).ToString();
                        var roundedLabel = axis.GetLabelContent((int)Math.Round(val)).ToString();
                        var doubleDigitLabel = axis.GetLabelContent(Math.Round(val, 2)).ToString();

                        if (!axis.IsVertical)
                        {
                            if (Chart.VisibleSeries.Count > 0 && Chart.VisibleSeries[0].IsIndexed && !Chart.VisibleSeries[0].IsActualTransposed)
                            {
                                if (roundedLabel != null)
                                {
                                    pointInfo.ValueX = roundedLabel;
                                }
                                var x1 = this.Chart.ActualValueToPoint(axis, Math.Round(val));
                                x1 += seriesLeft;
                                pointInfo.X = verticalLine.X1 = verticalLine.X2 = x1 > this.Chart.SeriesClipRect.Right ?
                                    this.Chart.SeriesClipRect.Right : x1 < this.Chart.SeriesClipRect.Left ? this.Chart.SeriesClipRect.Left : x1;
                                pointInfo.BaseX = pointInfo.X;
                            }
                            else
                            {
                                if (isDateTimeAxis && labelContent != null)
                                {
                                    pointInfo.ValueX = labelContent;
                                }
                                else
                                {
                                    if(doubleDigitLabel!=null)
                                        pointInfo.ValueX = doubleDigitLabel;
                                }
                                pointInfo.X = point.X;
                                pointInfo.BaseX = pointInfo.X;
                            }

                            labelXValue = val.ToString();
                        }
                        else
                        {
                            if (Chart.VisibleSeries.Count > 0 && Chart.VisibleSeries[0].IsIndexed && Chart.VisibleSeries[0].IsActualTransposed)
                            {
                                if(roundedLabel!= null) 
                                {
                                    pointInfo.ValueY = roundedLabel;
                                }
                                var y1 = this.Chart.ActualValueToPoint(axis, Math.Round(val));
                                y1 += seriesTop;
                                pointInfo.Y = horizontalLine.Y1 = horizontalLine.Y2 = y1 > this.Chart.SeriesClipRect.Bottom ?
                                    this.Chart.SeriesClipRect.Bottom : y1 < this.Chart.SeriesClipRect.Top ? this.Chart.SeriesClipRect.Top : y1;
                                pointInfo.BaseY = pointInfo.Y;
                            }
                            else
                            {
                                if(isDateTimeAxis && labelContent!=null) 
                                {
                                    pointInfo.ValueY = labelContent;
                                }
                                else
                                {
                                    if(doubleDigitLabel != null) 
                                        pointInfo.ValueY = doubleDigitLabel;
                                }
                                pointInfo.Y = point.Y;
                                pointInfo.BaseY = pointInfo.Y;
                            }

                            labelYValue = val.ToString();
                        }

                        GenerateLabel(pointInfo, axis);
                    }
                }
            }
        }

        #endregion

        #region Protected Override Methods
        
        internal override void AttachElements()
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("VerticalLineStyle");
            verticalLine.SetBinding(Line.StyleProperty, binding);

            horizontalLine = new Line(); binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("HorizontalLineStyle");
            horizontalLine.SetBinding(Line.StyleProperty, binding);

            if (this.AdorningCanvas != null && !this.AdorningCanvas.Children.Contains(verticalLine))
            {
                this.AdorningCanvas.Children.Add(verticalLine);
                elements.Add(verticalLine);
            }

            if (this.AdorningCanvas != null && !this.AdorningCanvas.Children.Contains(horizontalLine))
            {
                this.AdorningCanvas.Children.Add(horizontalLine);
                elements.Add(horizontalLine);
            }

            IsActivated = false;
        }

        #endregion

        #region Protected Virutal Methods

        private void GenerateLabel(ChartPointInfo pointInfo, ChartAxis axis)
        {
            if (axis.GetTrackballInfo())
            {
                double scrollbar = 0;
                chartAxis = axis;
                DataTemplate axisCrosshairLabelTemplate = axis.CrosshairLabelTemplate ?? 
                             ChartDictionaries.GenericCommonDictionary["SyncfusionChartAxisCrosshairLabelTemplate"] as DataTemplate;

                if (axis.IsVertical)
                {
                    pointInfo.X = axis.OpposedPosition ? axis.ArrangeRect.Left + scrollbar : axis.ArrangeRect.Right - scrollbar;
                    AddLabel(
                        pointInfo, 
                        VerticalAxisLabelAlignment,
                        GetChartAlignment(axis.OpposedPosition, ChartAlignment.Near),
                        axisCrosshairLabelTemplate);
                }
                else
                {
                    pointInfo.Y = axis.OpposedPosition ? axis.ArrangeRect.Bottom - scrollbar : axis.ArrangeRect.Top + scrollbar;
                    AddLabel(
                        pointInfo, 
                        GetChartAlignment(axis.OpposedPosition, ChartAlignment.Far),
                        HorizontalAxisLabelAlignment,
                        axisCrosshairLabelTemplate);
                }
            }
        }

        private void AddLabel(
            object obj, 
            ChartAlignment verticalAlignemnt,
            ChartAlignment horizontalAlignment,
            DataTemplate labelTemplate,
            double x,
            double y)
        {
            ContentControl control = new ContentControl();

            control.Content = obj;
            control.ContentTemplate = labelTemplate;
            AddElement(control);
            labelElements.Add(control);
            control.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            AlignAxisToolTipPolygon(control, verticalAlignemnt, horizontalAlignment, x, y, this);

#if WinUI && !WinUI_Desktop
	// PolygonPoints does not bind with updated values.So, Reapplied the ContentTemplate,Content of ContentControl.
            control.ContentTemplate = null;
            control.Content = null;
            control.Content = obj;
            control.ContentTemplate = labelTemplate;
            control.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

#endif
        }

        #endregion

        #region Protected Methods

        private void AddLabel(ChartPointInfo obj, ChartAlignment verticalAlignment, ChartAlignment horizontalAlignment, DataTemplate template)
        {
            if (obj != null && template != null)
            {
                AddLabel(obj, verticalAlignment, horizontalAlignment, template, obj.X, obj.Y);
            }
        }

        private void AddElement(UIElement element)
        {
            if (!this.AdorningCanvas.Children.Contains(element) && element is FrameworkElement frameworkElement)
            {
                this.AdorningCanvas.Children.Add(element);
                elements.Add(frameworkElement);
            }
        }

        #endregion

        #region Private Static Methods

        private static ChartAlignment GetChartAlignment(bool isOpposed, ChartAlignment alignment)
        {
            if (isOpposed)
            {
                if (alignment == ChartAlignment.Near)
                    return ChartAlignment.Far;
                else if (alignment == ChartAlignment.Far)
                    return ChartAlignment.Near;
                else
                    return ChartAlignment.Center;
            }
            else
                return alignment;
        }

        #endregion

        #region Private Methods

        private void Activate(bool activate)
        {
            foreach (UIElement element in elements)
            {
                element.Visibility = activate ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        #endregion

        #endregion
    }
}
