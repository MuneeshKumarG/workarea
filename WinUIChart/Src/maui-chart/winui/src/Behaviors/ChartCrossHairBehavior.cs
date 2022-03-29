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
using ChartCrossHairBehavior = Syncfusion.UI.Xaml.Charts.ChartCrosshairBehavior;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// ChartCrosshairBehavior enables viewing of informations related to Chart coordinates, at mouse over position or at touch contact point inside a Chart.
    /// </summary>
    /// <remarks>
    /// ChartCrosshairBehavior displays a vertical line, horizontal line and a popup like control displaying information about the data point
    /// at touch contact point or at mouse over position. You can also customize the look of cross hair and information displayed in a label.
    /// </remarks>
    /// <example>
    /// This example, we are using <see cref="ChartCrosshairBehavior"/>.
    /// <code language="XAML">
    ///     &lt;syncfusion:SfChart&gt;
    ///         &lt;syncfusion:SfChart.Behaviors&gt;
    ///             &lt;syncfusion:ChartCrosshairBehavior/&gt;
    ///         &lt;/syncfusion:SfChart.Behaviors&gt;
    ///     &lt;/syncfusion:SfChart&gt;
    /// </code>
    /// <code language="C#">
    ///     ChartCrosshairBehavior crosshair = new ChartCrosshairBehavior();
    ///     chartArea.Behaviors.Add(crosshair);
    /// </code>
    /// </example>
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
                typeof(ChartCrossHairBehavior),
                new PropertyMetadata(ChartAlignment.Center));

        /// <summary>
        ///  The DependencyProperty for <see cref="HorizontalAxisLabelAlignment"/> property.
        /// </summary>
        public static readonly DependencyProperty HorizontalAxisLabelAlignmentProperty =
            DependencyProperty.Register(
                nameof(HorizontalAxisLabelAlignment),
                typeof(ChartAlignment),
                typeof(ChartCrossHairBehavior),
                new PropertyMetadata(ChartAlignment.Center));

        /// <summary>
        /// The DependencyProperty for <see cref="HorizontalLineStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty HorizontalLineStyleProperty =
            DependencyProperty.Register(
                nameof(HorizontalLineStyle),
                typeof(Style),
                typeof(ChartCrossHairBehavior),
                new PropertyMetadata(ChartDictionaries.GenericCommonDictionary["SyncfusionChartCrosshairLineStyle"]));

        /// <summary>
        /// The DependencyProperty for <see cref="VerticalLineStyle"/> property.
        /// </summary>
        public static readonly DependencyProperty VerticalLineStyleProperty =
            DependencyProperty.Register(
                nameof(VerticalLineStyle),
                typeof(Style),
                typeof(ChartCrossHairBehavior),
                new PropertyMetadata(ChartDictionaries.GenericCommonDictionary["SyncfusionChartCrosshairLineStyle"]));

        #endregion

        #region Fields

        #region Protected Internal Fields

        /// <summary>
        /// Used to specify the current location. 
        /// </summary>
        protected internal Point CurrentPoint;

        #endregion

        #region Private Fields
        
        private Line verticalLine;
        private Line horizontalLine;

        private int fingerCount = 0;
        private bool isActivated;
        private List<ContentControl> labelElements;
        private ObservableCollection<ChartPointInfo> pointInfos;
        private List<FrameworkElement> elements;
        private string labelXValue;
        private string labelYValue;

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Called when instance created for <see cref="ChartCrosshairBehavior"/>.
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
        /// Gets or sets the alignment for the label appearing in vertical axis.
        /// </summary>
        /// <value>
        /// <see cref="ChartAlignment"/>
        /// </value>
        public ChartAlignment VerticalAxisLabelAlignment
        {
            get { return (ChartAlignment)GetValue(VerticalAxisLabelAlignmentProperty); }
            set { SetValue(VerticalAxisLabelAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets or sets the alignment for the label appearing in horizontal axis.
        /// </summary>
        /// <value>
        /// <see cref="ChartAlignment"/>
        /// </value>
        public ChartAlignment HorizontalAxisLabelAlignment
        {
            get { return (ChartAlignment)GetValue(HorizontalAxisLabelAlignmentProperty); }
            set { SetValue(HorizontalAxisLabelAlignmentProperty, value); }
        }

        /// <summary>
        /// Gets the collection of ChartPointInfo.
        /// </summary>
        public ObservableCollection<ChartPointInfo> PointInfos
        {
            get
            {
                return pointInfos;
            }

            internal set
            {
                pointInfos = value;
            }
        }

        /// <summary>
        /// Gets or sets the style for horizontal line.
        /// </summary>
        public Style HorizontalLineStyle
        {
            get { return (Style)GetValue(HorizontalLineStyleProperty); }
            set { SetValue(HorizontalLineStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the style for vertical line.
        /// </summary>
        public Style VerticalLineStyle
        {
            get { return (Style)GetValue(VerticalLineStyleProperty); }
            set { SetValue(VerticalLineStyleProperty, value); }
        }

        #endregion

        #region Protected Internal Properties

        /// <summary>
        /// Gets or sets a value indicating whether the crosshair is activated or not.
        /// </summary>
        protected internal bool IsActivated
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

        /// <summary>
        /// Method implementation for DetachElements.
        /// </summary>
        protected internal override void DetachElements()
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

        /// <summary>
        /// Called when size changed.
        /// </summary>
        /// <param name="e">SizeChangedEventArgs</param>
        protected internal override void OnSizeChanged(SizeChangedEventArgs e)
        {
            if (ChartArea != null && !string.IsNullOrEmpty(labelXValue) && !string.IsNullOrEmpty(labelYValue))
            {
                double y1 = this.ChartArea.ActualValueToPoint(this.ChartArea.InternalSecondaryAxis, (Convert.ToDouble(labelYValue)));
                double x1 = this.ChartArea.ActualValueToPoint(this.ChartArea.InternalPrimaryAxis, (Convert.ToDouble(labelXValue)));
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

        /// <summary>
        /// Called when holding the focus in UIElement.
        /// </summary>
        /// <param name="e">HoldingRoutedEventArgs</param>
        protected internal override void OnHolding(HoldingRoutedEventArgs e)
        {
            if(ChartArea == null)
            {
                return;
            }

            IsActivated = true;

            if (e.PointerDeviceType == PointerDeviceType.Touch)
                ChartArea.HoldUpdate = true;
            if (this.ChartArea != null && this.ChartArea.VisibleSeries.Count > 0 && this.ChartArea.VisibleSeries[0] is CartesianSeries && IsActivated)
            {
                Point point = e.GetPosition(this.AdorningCanvas);

                if (this.ChartArea.SeriesClipRect.Contains(point))
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

        /// <summary>
        /// Called when pointer pressed in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
                IsActivated = false;
            else
                fingerCount++;
        }

        /// <summary>
        /// Called when pointer moved in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal override void OnPointerMoved(PointerRoutedEventArgs e)
        {
            var pointer = e.GetCurrentPoint(this.AdorningCanvas);
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Touch)
                if (fingerCount > 1) return;
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse &&
                 !pointer.Properties.IsLeftButtonPressed)
                IsActivated = true;

            if (this.ChartArea != null && this.ChartArea.AreaType == ChartAreaType.CartesianAxes && IsActivated)
            {
                CurrentPoint = new Point(pointer.Position.X, pointer.Position.Y);

                if (this.ChartArea.SeriesClipRect.Contains(CurrentPoint))
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

        /// <summary>
        /// Called when pointer exited from the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal override void OnPointerExited(PointerRoutedEventArgs e)
        {
            if (IsActivated)
            {
                IsActivated = false;
            }

            fingerCount = 0;
        }

        /// <summary>
        /// Called when the layout updated from chart.
        /// </summary>
        protected internal override void OnLayoutUpdated()
        {
            if (this.ChartArea == null)
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

                if (this.ChartArea.SeriesClipRect.Contains(CurrentPoint))
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

        /// <summary>
        /// Called when pointer released in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal override void OnPointerReleased(PointerRoutedEventArgs e)
        {
            if (ChartArea == null)
            {
                return;
            }

            if (e.Pointer.PointerDeviceType != PointerDeviceType.Mouse)
            {
                if (IsActivated)
                {
                    IsActivated = false;
                    ChartArea.HoldUpdate = false;
                }

                fingerCount--;
            }
        }

        /// <summary>
        /// Align the element inside the content control. 
        /// </summary>
        /// <param name="verticalAlignemnt">Used to specify <see cref="ChartAlignment"/> for vertical</param>
        /// <param name="horizontalAlignment">Used to specify <see cref="ChartAlignment"/> for horizontal</param>
        /// <param name="x">The X location</param>
        /// <param name="y">The Y location</param>
        /// <param name="control">Used to specify corresponding content control</param>
        protected internal override void AlignDefaultLabel(
            ChartAlignment verticalAlignemnt, 
            ChartAlignment horizontalAlignment,
            double x, 
            double y, 
            ContentControl control)
        {
            if (horizontalAlignment == ChartAlignment.Near)
            {
                x = x - control.DesiredSize.Width;
                if (control is ContentControl)
                    ((control as ContentControl).Content as ChartPointInfo).X = x;
            }
            else if (horizontalAlignment == ChartAlignment.Center)
            {
                x = x - control.DesiredSize.Width / 2;
                if (control is ContentControl)
                    ((control as ContentControl).Content as ChartPointInfo).X = x;
            }

            if (verticalAlignemnt == ChartAlignment.Near)
            {
                y = y - control.DesiredSize.Height;
                if (control is ContentControl)
                    ((control as ContentControl).Content as ChartPointInfo).Y = y;
            }
            else if (verticalAlignemnt == ChartAlignment.Center)
            {
                y = y - control.DesiredSize.Height / 2;
                if (control is ContentControl)
                    ((control as ContentControl).Content as ChartPointInfo).Y = y;
            }

            Canvas.SetLeft(control, x);
            Canvas.SetTop(control, y);
        }

        #endregion

        #region Protected Internal Virtual Methods

        /// <summary>
        /// Method implementation for set positions for given point.
        /// </summary>
        /// <param name="point">Point</param>
        protected internal virtual void SetPosition(Point point)
        {
            if (AdorningCanvas == null || double.IsNaN(point.X) || double.IsNaN(point.Y) || !IsActivated) return;

            var seriesLeft = ChartArea.SeriesClipRect.Left;
            var seriesTop = ChartArea.SeriesClipRect.Top;

            double x = point.X;
            double y = point.Y;

            foreach (var element in elements)
            {
                element.Visibility = Visibility.Visible;
            }

            verticalLine.X1 = verticalLine.X2 = x > this.ChartArea.SeriesClipRect.Right ? this.ChartArea.SeriesClipRect.Right : x;
            verticalLine.Y1 = seriesTop;
            verticalLine.Y2 = this.ChartArea.SeriesClipRect.Height + seriesTop;
            elements.Add(verticalLine);

            horizontalLine.Y1 = horizontalLine.Y2 = y;
            horizontalLine.X1 = seriesLeft;
            horizontalLine.X2 = seriesLeft + this.ChartArea.SeriesClipRect.Width;
            elements.Add(horizontalLine);

            foreach (ChartAxis axis in ChartArea.InternalAxes)
            {
                if ((axis.RenderedRect.Left <= point.X && axis.RenderedRect.Right >= point.X)
                    || axis.RenderedRect.Top <= point.Y && axis.RenderedRect.Bottom >= point.Y)
                {
                    double val = this.ChartArea.ActualPointToValue(axis, new Point(point.X - seriesLeft, point.Y - seriesTop));
                    if (!double.IsNaN(val))
                    {
                        ChartPointInfo pointInfo = new ChartPointInfo();
                        pointInfo.Axis = axis;
                        var isDateTimeAxis = axis is DateTimeAxis;

                        if (axis.Orientation == Orientation.Horizontal)
                        {
                            if (ChartArea.VisibleSeries.Count > 0 && ChartArea.VisibleSeries[0].IsIndexed && !ChartArea.VisibleSeries[0].IsActualTransposed)
                            {
                                pointInfo.ValueX = axis.GetLabelContent((int)Math.Round(val)).ToString();
                                var x1 = this.ChartArea.ActualValueToPoint(axis, Math.Round(val));
                                x1 += seriesLeft;
                                pointInfo.X = verticalLine.X1 = verticalLine.X2 = x1 > this.ChartArea.SeriesClipRect.Right ?
                                    this.ChartArea.SeriesClipRect.Right : x1 < this.ChartArea.SeriesClipRect.Left ? this.ChartArea.SeriesClipRect.Left : x1;
                                pointInfo.BaseX = pointInfo.X;
                            }
                            else
                            {
                                pointInfo.ValueX = isDateTimeAxis ? axis.GetLabelContent(val).ToString() : axis.GetLabelContent(Math.Round(val, 2)).ToString();
                                pointInfo.X = point.X;
                                pointInfo.BaseX = pointInfo.X;
                            }

                            labelXValue = val.ToString();
                        }
                        else
                        {
                            if (ChartArea.VisibleSeries.Count > 0 && ChartArea.VisibleSeries[0].IsIndexed && ChartArea.VisibleSeries[0].IsActualTransposed)
                            {
                                pointInfo.ValueY = axis.GetLabelContent((int)Math.Round(val)).ToString();
                                var y1 = this.ChartArea.ActualValueToPoint(axis, Math.Round(val));
                                y1 += seriesTop;
                                pointInfo.Y = horizontalLine.Y1 = horizontalLine.Y2 = y1 > this.ChartArea.SeriesClipRect.Bottom ?
                                    this.ChartArea.SeriesClipRect.Bottom : y1 < this.ChartArea.SeriesClipRect.Top ? this.ChartArea.SeriesClipRect.Top : y1;
                                pointInfo.BaseY = pointInfo.Y;
                            }
                            else
                            {
                                pointInfo.ValueY = isDateTimeAxis ? axis.GetLabelContent(val).ToString() : axis.GetLabelContent(Math.Round(val, 2)).ToString();
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
        
        /// <summary>
        /// Method implementation for AttachElements.
        /// </summary>
        protected override void AttachElements()
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

        /// <summary>
        /// Method implementation to generate trackball label for axis.
        /// </summary>
        /// <param name="pointInfo">ChartPointInfo</param>
        /// <param name="axis">ChartAxis</param>
        protected virtual void GenerateLabel(ChartPointInfo pointInfo, ChartAxis axis)
        {
            if (axis.GetTrackballInfo())
            {
                double scrollbar = 0;
                chartAxis = axis;
                DataTemplate axisCrosshairLabelTemplate = axis.CrosshairLabelTemplate ??
                             ChartDictionaries.GenericCommonDictionary["SyncfusionChartAxisCrosshairLabelTemplate"] as DataTemplate;
                var chartAxisBase2D = axis as ChartAxisBase2D;

                if (chartAxisBase2D.EnableScrollBar && !chartAxisBase2D.EnableTouchMode)
                {
                    if (axis.Orientation == Orientation.Vertical)
                        scrollbar = chartAxisBase2D.sfChartResizableBar.DesiredSize.Width;
                    else
                        scrollbar = chartAxisBase2D.sfChartResizableBar.DesiredSize.Height;
                }

                if (axis.Orientation == Orientation.Vertical)
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

        /// <summary>
        /// Method implementation for adding labels in Crosshair.
        /// </summary>
        /// <param name="obj">ChartPointInfo</param>
        /// <param name="verticalAlignemnt">Used to specify <see cref="ChartAlignment"/> for vertical</param>
        /// <param name="horizontalAlignment">Used to specify <see cref="ChartAlignment"/> for horizontal</param>
        /// <param name="labelTemplate">DataTemplate</param>
        /// <param name="x">The X location</param>
        /// <param name="y">The Y location</param>
        protected virtual void AddLabel(
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

        /// <summary>
        /// Method implementation for adding label in crosshair.
        /// </summary>
        /// <param name="obj">ChartPointInfo</param>
        /// <param name="verticalAlignment">Used to specify <see cref="ChartAlignment"/> for vertical</param>
        /// <param name="horizontalAlignment">Used to specify <see cref="ChartAlignment"/> for horizontal</param>
        /// <param name="template">DataTemplate</param>
        protected void AddLabel(ChartPointInfo obj, ChartAlignment verticalAlignment, ChartAlignment horizontalAlignment, DataTemplate template)
        {
            if (obj != null && template != null)
            {
                AddLabel(obj, verticalAlignment, horizontalAlignment, template, obj.X, obj.Y);
            }
        }

        /// <summary>
        /// Method implementation for add elements in UIElement.
        /// </summary>
        /// <param name="element">UIElement</param>
        protected void AddElement(UIElement element)
        {
            if (!this.AdorningCanvas.Children.Contains(element))
            {
                this.AdorningCanvas.Children.Add(element);
                elements.Add(element as FrameworkElement);
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
