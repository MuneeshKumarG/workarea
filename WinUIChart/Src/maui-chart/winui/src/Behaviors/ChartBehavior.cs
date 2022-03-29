using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// ChartBehavior is an abstract base class for behaviors which can be added to <see cref="ChartBase"/> 
    /// </summary>
    /// <remarks>
    /// You can handle the <see cref="ChartBase"/> events directly in Chart behavior, which will be helpful in designing the Chart application in MVVM pattern. 
    /// You can add a custom behavior to the <see cref="ChartBase"/> by inheriting a class from the <see cref="ChartBehavior"/>.
    /// You can also add ui elements to the Chart by making use of the top layer canvas returned from <see cref="ChartBehavior.AdorningCanvas"/> property,which can be used to place the ui
    /// elements at desired positions in <see cref="ChartBase"/>.  
    /// </remarks>
    /// <seealso cref="ChartZoomPanBehavior"/>
    /// <seealso cref="ChartSelectionBehavior"/>
    /// <seealso cref="ChartTrackballBehavior"/>
    /// <seealso cref="ChartCrosshairBehavior"/>
    /// <example>
    /// This example, we are using <see cref="ChartZoomPanBehavior"/>.
    /// <code language="XAML">
    ///     &lt;syncfusion:SfChart&gt;
    ///         &lt;syncfusion:SfChart.Behaviors&gt;
    ///             &lt;syncfusion:ChartZoomPanBehavior/&gt;
    ///         &lt;/syncfusion:SfChart.Behaviors&gt;
    ///     &lt;/syncfusion:SfChart&gt;
    /// </code>
    /// <code language="C#">
    ///     ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior();
    ///     chartArea.Behaviors.Add(zoomPanBehavior);
    /// </code>
    /// </example>
    public abstract class ChartBehavior : DependencyObject
    {
        #region Fields

        #region Protected Fields

        /// <summary>
        /// Used to specify corresponding <see cref="ChartAxis"/>.
        /// </summary>
        protected ChartAxis chartAxis;

        #endregion

        #region Private Fields

        private const int axisTipHeight = 6;

        private Canvas adorningCanvas, bottomAdorningCanvas;

        private ChartBase chartArea;
        
        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// Called when instance created for ChartBehavior.
        /// </summary>
        public ChartBehavior()
        {
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the top layer Canvas.
        /// </summary>
        public Canvas AdorningCanvas
        {
            get
            {
                return adorningCanvas;
            }

            internal set
            {
                if (adorningCanvas != value)
                {
                    adorningCanvas = value;
                }
            }
        }

        /// <summary>
        /// Gets the bottom layer Canvas.
        /// </summary>
        public Canvas BottomAdorningCanvas
        {
            get
            {
                return bottomAdorningCanvas;
            }

            internal set
            {
                if (bottomAdorningCanvas != value)
                {
                    bottomAdorningCanvas = value;
                }
            }
        }

        /// <summary>
        /// Gets the owner Chart.
        /// </summary>
        public ChartBase ChartArea
        {
            get
            {
                return chartArea;
            }

            internal set
            {
                chartArea = value;
            }
        }

        #endregion

        #region Internal Properties

        internal bool IsReversed { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Internal Static Methods

        internal static Point ValidatePoint(Point point, Rect rect)
        {
            Point pt = new Point();
            if (point.X < rect.Left)
                pt.X = rect.Left;
            else if (point.X > rect.Right)
                pt.X = rect.Right;
            else
                pt.X = point.X;

            if (point.Y < rect.Top)
                pt.Y = rect.Top;
            else if (point.Y > rect.Bottom)
                pt.Y = rect.Bottom;
            else
                pt.Y = point.Y;

            return pt;
        }

        #endregion

        #region Internal Methods
        
        internal void Dispose()
        {
            this.DetachElements();
            chartAxis = null;
            ChartArea = null;
        }

        internal void InternalAttachElements()
        {
            this.AttachElements();
        }

        internal void AlignAxisToolTipPolygon(
                                              ContentControl control,
                                              ChartAlignment verticalAlignemnt,
                                              ChartAlignment horizontalAlignment,
                                              double x,
                                              double y,
                                              ChartBehavior behavior)
        {
            double labelHeight = control.DesiredSize.Height;
            double labelWidth = control.DesiredSize.Width;
            double axisTipHypotenuse = (2 * axisTipHeight) / Math.Sqrt(3);
            var isChartTrackBallBehavior = behavior is ChartTrackballBehavior;

            PointCollection polygonPoints = new PointCollection();
            polygonPoints.Add(new Point(0, 0));
            polygonPoints.Add(new Point(0, labelHeight));
            polygonPoints.Add(new Point(labelWidth, labelHeight));

            if (chartAxis.Orientation == Orientation.Horizontal)
            {
                polygonPoints.Add(new Point(labelWidth, 0));

                if (chartAxis.OpposedPosition)
                {
                    double tipPosition, leftTipWidth, rightTipWidth;
                    if ((control.Content as ChartPointInfo).Axis.CrosshairLabelTemplate == null && !isChartTrackBallBehavior)
                        control.Margin = new Thickness().GetThickness(0, -6, 0, 0);

                    if (((control.Content as ChartPointInfo).X - control.DesiredSize.Height <= ChartArea.SeriesClipRect.X))
                    {
                        if (isChartTrackBallBehavior)
                            AlignDefaultLabel(
                                ChartAlignment.Far, 
                                ChartAlignment.Far, 
                                ChartArea.SeriesClipRect.X,
                                (control.Content as ChartPointInfo).Y,
                                control);
                        else
                            AlignDefaultLabel(
                                verticalAlignemnt,
                                ChartAlignment.Far, 
                                ChartArea.SeriesClipRect.X,
                                (control.Content as ChartPointInfo).Y,
                                control);

                        tipPosition = (control.Content as ChartPointInfo).BaseX - ChartArea.SeriesClipRect.X;
                        leftTipWidth = tipPosition - axisTipHypotenuse / 2;
                        rightTipWidth = tipPosition + axisTipHypotenuse / 2;

                        if (leftTipWidth < 0)
                            leftTipWidth = 0;

                        polygonPoints.Insert(2, (new Point(leftTipWidth, labelHeight)));
                        polygonPoints.Insert(3, (new Point(tipPosition, labelHeight + axisTipHeight)));
                        polygonPoints.Insert(4, (new Point(rightTipWidth, labelHeight)));
                    }
                    else if ((control.Content as ChartPointInfo).X + control.DesiredSize.Width
                        >= ChartArea.SeriesClipRect.X + ChartArea.SeriesClipRect.Width)
                    {
                        if (isChartTrackBallBehavior)
                            AlignDefaultLabel(
                                ChartAlignment.Far,
                                ChartAlignment.Near, 
                                ChartArea.SeriesClipRect.X + ChartArea.SeriesClipRect.Width,
                                (control.Content as ChartPointInfo).Y,
                                control);
                        else
                            AlignDefaultLabel(
                                verticalAlignemnt, 
                                ChartAlignment.Near, 
                                ChartArea.SeriesClipRect.X + ChartArea.SeriesClipRect.Width,
                                (control.Content as ChartPointInfo).Y,
                                control);

                        tipPosition = (control.Content as ChartPointInfo).BaseX - (control.Content as ChartPointInfo).X;
                        leftTipWidth = tipPosition - axisTipHypotenuse / 2;
                        rightTipWidth = tipPosition + axisTipHypotenuse / 2;

                        if (rightTipWidth > control.DesiredSize.Width)
                            rightTipWidth = control.DesiredSize.Width;
                        if (leftTipWidth < 0)
                            leftTipWidth = 0;

                        polygonPoints.Insert(2, (new Point(leftTipWidth, labelHeight)));
                        polygonPoints.Insert(3, (new Point(tipPosition, labelHeight + axisTipHeight)));
                        polygonPoints.Insert(4, (new Point(rightTipWidth, labelHeight)));
                    }
                    else
                    {
                        AlignDefaultLabel(verticalAlignemnt, horizontalAlignment, x, y, control);

                        polygonPoints.Insert(2, new Point(labelWidth / 2 - axisTipHypotenuse / 2, labelHeight));
                        polygonPoints.Insert(3, new Point(labelWidth / 2, labelHeight + axisTipHeight));
                        polygonPoints.Insert(4, new Point(labelWidth / 2 + axisTipHypotenuse / 2, labelHeight));
                    }
                }
                else
                {
                    double tipPosition, leftTipWidth, rightTipWidth;
                    if ((control.Content as ChartPointInfo).Axis.CrosshairLabelTemplate == null && !isChartTrackBallBehavior)
                           control.Margin = new Thickness().GetThickness(0, 6, 0, 0);

                    if (((control.Content as ChartPointInfo).X - control.DesiredSize.Height <= ChartArea.SeriesClipRect.X))
                    {
                        AlignDefaultLabel(
                            ChartAlignment.Far, 
                            ChartAlignment.Far, 
                            ChartArea.SeriesClipRect.X,
                            (control.Content as ChartPointInfo).Y, 
                            control);

                        tipPosition = (control.Content as ChartPointInfo).BaseX - ChartArea.SeriesClipRect.X;
                        leftTipWidth = tipPosition - axisTipHypotenuse / 2;
                        rightTipWidth = tipPosition + axisTipHypotenuse / 2;

                        if (leftTipWidth < 0)
                            leftTipWidth = 0;

                        polygonPoints.Insert(4, (new Point(rightTipWidth, 0)));
                        polygonPoints.Insert(5, (new Point(tipPosition, -axisTipHeight)));
                        polygonPoints.Insert(6, (new Point(leftTipWidth, 0)));
                    }
                    else if ((control.Content as ChartPointInfo).X + control.DesiredSize.Width
                        >= ChartArea.SeriesClipRect.X + ChartArea.SeriesClipRect.Width)
                    {
                        AlignDefaultLabel(
                            ChartAlignment.Far,
                            ChartAlignment.Near,
                            ChartArea.SeriesClipRect.X + ChartArea.SeriesClipRect.Width,
                            (control.Content as ChartPointInfo).Y, 
                            control);

                        tipPosition = ((control.Content as ChartPointInfo).BaseX - (control.Content as ChartPointInfo).X);
                        leftTipWidth = tipPosition - axisTipHypotenuse / 2;
                        rightTipWidth = tipPosition + axisTipHypotenuse / 2;

                        if (rightTipWidth > control.DesiredSize.Width)
                            rightTipWidth = control.DesiredSize.Width;
                        if (leftTipWidth < 0)
                            leftTipWidth = 0;

                        polygonPoints.Insert(4, new Point(rightTipWidth, 0));
                        polygonPoints.Insert(5, new Point(tipPosition, -axisTipHeight));
                        polygonPoints.Insert(6, new Point(leftTipWidth, 0));
                    }
                    else
                    {
                        AlignDefaultLabel(verticalAlignemnt, horizontalAlignment, x, y, control);

                        polygonPoints.Insert(4, new Point(labelWidth / 2 + axisTipHypotenuse / 2, 0));
                        polygonPoints.Insert(5, new Point(labelWidth / 2, -axisTipHeight));
                        polygonPoints.Insert(6, new Point(labelWidth / 2 - axisTipHypotenuse / 2, 0));
                    }
                }

                polygonPoints.Add(new Point(0, 0));
                (control.Content as ChartPointInfo).PolygonPoints = polygonPoints;
            }
            else
            {
                if (chartAxis.OpposedPosition)
                {
                    if ((control.Content as ChartPointInfo).Axis.CrosshairLabelTemplate == null && !isChartTrackBallBehavior)
                        control.Margin = new Thickness().GetThickness(6, 0, 0, 0);

                    if (((control.Content as ChartPointInfo).Y - control.DesiredSize.Height <= ChartArea.SeriesClipRect.Y))
                    {
                        if (isChartTrackBallBehavior)
                            AlignDefaultLabel(
                                ChartAlignment.Far,
                                ChartAlignment.Far, 
                                (control.Content as ChartPointInfo).X,
                                ChartArea.SeriesClipRect.Y,
                                control);
                        else
                            AlignDefaultLabel(
                                ChartAlignment.Far,
                                horizontalAlignment, 
                                (control.Content as ChartPointInfo).X,
                                ChartArea.SeriesClipRect.Y, 
                                control);

                        double tipPosition = (control.Content as ChartPointInfo).BaseY - ChartArea.SeriesClipRect.Y;
                        double lefttipWidth = tipPosition - axisTipHypotenuse / 2;
                        double righttipWidth = tipPosition + axisTipHypotenuse / 2;

                        if (lefttipWidth < 0)
                            lefttipWidth = 0;
                        if (righttipWidth > labelHeight)
                            righttipWidth = labelHeight;

                        polygonPoints.Insert(1, new Point(0, lefttipWidth));
                        polygonPoints.Insert(2, new Point(-axisTipHeight, tipPosition));
                        polygonPoints.Insert(3, new Point(0, righttipWidth));
                    }
                    else if ((control.Content as ChartPointInfo).Y + control.DesiredSize.Height
                        >= ChartArea.SeriesClipRect.Y + ChartArea.SeriesClipRect.Height)
                    {
                        if (isChartTrackBallBehavior)
                            AlignDefaultLabel(
                                ChartAlignment.Near,
                                ChartAlignment.Far, 
                                (control.Content as ChartPointInfo).X,
                                ChartArea.SeriesClipRect.Y + ChartArea.SeriesClipRect.Height,
                                control);
                        else
                            AlignDefaultLabel(
                                ChartAlignment.Near,
                                horizontalAlignment, 
                                (control.Content as ChartPointInfo).X,
                                ChartArea.SeriesClipRect.Y + ChartArea.SeriesClipRect.Height,
                                control);

                        double tipPosition = (control.Content as ChartPointInfo).BaseY - (control.Content as ChartPointInfo).Y;
                        double lefttipWidth = tipPosition - axisTipHypotenuse / 2;
                        double righttipWidth = tipPosition + axisTipHypotenuse / 2;

                        if (righttipWidth > control.DesiredSize.Height)
                            righttipWidth = control.DesiredSize.Height;
                        if (lefttipWidth < 0)
                            lefttipWidth = 0;

                        polygonPoints.Insert(1, new Point(0, lefttipWidth));
                        polygonPoints.Insert(2, new Point(-axisTipHeight, tipPosition));
                        polygonPoints.Insert(3, new Point(0, righttipWidth));
                    }
                    else
                    {
                        if (!isChartTrackBallBehavior)
                            AlignDefaultLabel(verticalAlignemnt, horizontalAlignment, x, y, control);

                        polygonPoints.Insert(1, new Point(0, labelHeight / 3));
                        polygonPoints.Insert(2, new Point(-axisTipHeight, labelHeight / 2));
                        polygonPoints.Insert(3, new Point(0, labelHeight / 1.5));
                    }
                }
                else
                {
                    if ((control.Content as ChartPointInfo).Axis.CrosshairLabelTemplate == null && !isChartTrackBallBehavior)
                        control.Margin = new Thickness().GetThickness(-6, 0, 0, 0);

                    if (((control.Content as ChartPointInfo).Y - control.DesiredSize.Height <= ChartArea.SeriesClipRect.Y))
                    {
                        if (isChartTrackBallBehavior)
                            AlignDefaultLabel(
                                ChartAlignment.Far,
                                ChartAlignment.Far, 
                                (control.Content as ChartPointInfo).X,
                                ChartArea.SeriesClipRect.Y,
                                control);
                        else
                            AlignDefaultLabel(
                                ChartAlignment.Far,
                                horizontalAlignment,
                                (control.Content as ChartPointInfo).X,
                                ChartArea.SeriesClipRect.Y,
                                control);

                        double tipPosition = (control.Content as ChartPointInfo).BaseY - ChartArea.SeriesClipRect.Y;
                        double lefttipWidth = tipPosition - axisTipHypotenuse / 2;
                        double righttipWidth = tipPosition + axisTipHypotenuse / 2;

                        if (lefttipWidth < 0)
                            lefttipWidth = 0;
                        if (righttipWidth > labelHeight)
                            righttipWidth = labelHeight;

                        polygonPoints.Insert(3, new Point(labelWidth, righttipWidth));
                        polygonPoints.Insert(4, new Point(labelWidth + axisTipHeight, tipPosition));
                        polygonPoints.Insert(5, new Point(labelWidth, lefttipWidth));
                    }
                    else if ((control.Content as ChartPointInfo).Y + control.DesiredSize.Height
                        >= ChartArea.SeriesClipRect.Y + ChartArea.SeriesClipRect.Height)
                    {
                        if (isChartTrackBallBehavior)
                            AlignDefaultLabel(
                                ChartAlignment.Near, 
                                ChartAlignment.Far, 
                                (control.Content as ChartPointInfo).X,
                                ChartArea.SeriesClipRect.Y + ChartArea.SeriesClipRect.Height,
                                control);
                        else
                            AlignDefaultLabel(
                                ChartAlignment.Near, 
                                horizontalAlignment, 
                                (control.Content as ChartPointInfo).X,
                                ChartArea.SeriesClipRect.Y + ChartArea.SeriesClipRect.Height, 
                                control);

                        double tipPosition = (control.Content as ChartPointInfo).BaseY - (control.Content as ChartPointInfo).Y;
                        double lefttipWidth = tipPosition - axisTipHypotenuse / 2;
                        double righttipWidth = tipPosition + axisTipHypotenuse / 2;

                        if (lefttipWidth < 0)
                            lefttipWidth = 0;
                        if (righttipWidth > control.DesiredSize.Height)
                            righttipWidth = control.DesiredSize.Height;

                        polygonPoints.Add(new Point(labelWidth, righttipWidth));
                        polygonPoints.Add(new Point(labelWidth + axisTipHeight, tipPosition));
                        polygonPoints.Add(new Point(labelWidth, lefttipWidth));
                    }
                    else
                    {
                        if (!isChartTrackBallBehavior)
                            AlignDefaultLabel(verticalAlignemnt, horizontalAlignment, x, y, control);

                        polygonPoints.Add(new Point(labelWidth, labelHeight / 1.5));
                        polygonPoints.Add(new Point(labelWidth + axisTipHeight, labelHeight / 2));
                        polygonPoints.Add(new Point(labelWidth, labelHeight / 3));
                    }
                }

                polygonPoints.Add(new Point(labelWidth, 0));
                polygonPoints.Add(new Point(0, 0));

                (control.Content as ChartPointInfo).PolygonPoints = polygonPoints;
            }
        }

#endregion
                
        #region Protected Internal Virtual Methods

        /// <summary>
        /// Called when layout updated.
        /// </summary>
        protected internal virtual void OnLayoutUpdated()
        {
        }

        /// <summary>
        /// Method implementation for DetachElement.
        /// </summary>
        /// <param name="element">UIElement</param>
        protected internal virtual void DetachElement(UIElement element)
        {
            if (this.AdorningCanvas != null && this.AdorningCanvas.Children.Contains(element))
                this.AdorningCanvas.Children.Remove(element);
        }
            
        /// <summary>
        /// Method implementation for DetachElements.
        /// </summary>
        protected internal virtual void DetachElements()
        {
        }

        /// <summary>
        /// Called when size changed.
        /// </summary>
        /// <param name="e">SizeChangedEventArgs</param>
        protected internal virtual void OnSizeChanged(SizeChangedEventArgs e)
        {
        }

        /// <summary>
        /// Called when drag action enter into the chart area.
        /// </summary>
        /// <param name="e">DragEventArgs</param>
        protected internal virtual void OnDragEnter(DragEventArgs e)
        {
        }

        /// <summary>
        /// Called when drag action leave from the chart area.
        /// </summary>
        /// <param name="e">DragEventArgs</param>
        protected internal virtual void OnDragLeave(DragEventArgs e)
        {
        }

        /// <summary>
        /// Called when drag action over in the chart area.
        /// </summary>
        /// <param name="e">DragEventArgs</param>
        protected internal virtual void OnDragOver(DragEventArgs e)
        {
        }

        /// <summary>
        /// Called when drop the cursor in the chart area.
        /// </summary>
        /// <param name="e">DragEventArgs</param>
        protected internal virtual void OnDrop(DragEventArgs e)
        {
        }

        /// <summary>
        /// Called when got focus in UIElement.
        /// </summary>
        /// <param name="e">RoutedEventArgs</param>
        protected internal virtual void OnGotFocus(RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when lost the focus in the chart.
        /// </summary>
        /// <param name="e">RoutedEventArgs</param>
        protected internal virtual void OnLostFocus(RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Method implementation for OnDoubleTapped.
        /// </summary>
        /// <param name="e">DoubleTappedRoutedEventArgs</param>
        protected internal virtual void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when holding the focus in UIElement.
        /// </summary>
        /// <param name="e">HoldingRoutedEventArgs</param>
        protected internal virtual void OnHolding(HoldingRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when KeyDown in the chart.
        /// </summary>
        /// <param name="e">KeyRoutedEventArgs</param>
        protected internal virtual void OnKeyDown(KeyRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when KeyUp in the chart.
        /// </summary>
        /// <param name="e">KeyRoutedEventArgs</param>
        protected internal virtual void OnKeyUp(KeyRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when manipulation completed in the chart.
        /// </summary>
        /// <param name="e">ManipulationCompletedRoutedEventArgs</param>
        protected internal virtual void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when manipulation delta is changed in the chart.
        /// </summary>
        /// <param name="e">ManipulationDeltaRoutedEventArgs</param>
        protected internal virtual void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when manipulation action start in the chart.
        /// </summary>
        /// <param name="e">ManipulationInertiaStartingRoutedEventArgs</param>
        protected internal virtual void OnManipulationInertiaStarting(ManipulationInertiaStartingRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when manipulation started.
        /// </summary>
        /// <param name="e">ManipulationStartedRoutedEventArgs</param>
        protected internal virtual void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when manipulation starting.
        /// </summary>
        /// <param name="e">ManipulationStartingRoutedEventArgs</param>
        protected internal virtual void OnManipulationStarting(ManipulationStartingRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when pointer cancelled in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal virtual void OnPointerCanceled(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when pointer capture lost in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal virtual void OnPointerCaptureLost(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when pointer entered in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal virtual void OnPointerEntered(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when pointer exited in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal virtual void OnPointerExited(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when pointer moved in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal virtual void OnPointerMoved(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when pointer pressed in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal virtual void OnPointerPressed(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when pointer released in the chart.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal virtual void OnPointerReleased(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when pointer wheel changed.
        /// </summary>
        /// <param name="e">PointerRoutedEventArgs</param>
        protected internal virtual void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when right tapped on the chart.
        /// </summary>
        /// <param name="e">RightTappedRoutedEventArgs</param>
        protected internal virtual void OnRightTapped(RightTappedRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Called when tapped the chart.
        /// </summary>
        /// <param name="e">TappedRoutedEventArgs</param>
        protected internal virtual void OnTapped(TappedRoutedEventArgs e)
        {
        }

        /// <summary>
        /// Align the element inside the content control. 
        /// </summary>
        /// <param name="verticalAlignemnt">Used to specify <see cref="ChartAlignment"/> for vertical</param>
        /// <param name="horizontalAlignment">Used to specify <see cref="ChartAlignment"/> for horizontal</param>
        /// <param name="x">The X location</param>
        /// <param name="y">The Y location</param>
        /// <param name="control">Used to specify corresponding content control</param>
        protected internal virtual void AlignDefaultLabel(
            ChartAlignment verticalAlignemnt,
            ChartAlignment horizontalAlignment,
            double x,
            double y,
            ContentControl control)
        {
        }

#endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Method implementation for attach elements.
        /// </summary>
        protected virtual void AttachElements()
        {
        }

#endregion

        #region Protected Methods

        /// <summary>
        /// Return collection of double values from the given ChartSeries.
        /// </summary>
        /// <param name="x">x-position</param>
        /// <param name="series">ChartSeriesBase</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Reviewed")]
        protected IList<double> GetYValuesBasedOnIndex(double x, ChartSeriesBase series)
        {
            List<double> Values = new List<double>();
            if (x < series.DataCount)
            {
                for (int i = 0; i < series.ActualSeriesYValues.Count(); i++)
                {
                    Values.Add(series.ActualSeriesYValues[i][(int)x]);
                }
            }

            return Values;
        }

        /// <summary>
        /// Method implementation for UpdateArea in chart.
        /// </summary>
        protected void UpdateArea()
        {
            if (ChartArea != null)
            {
                ChartArea.ScheduleUpdate();
            }
        }
        
#endregion

#endregion
    }
}
