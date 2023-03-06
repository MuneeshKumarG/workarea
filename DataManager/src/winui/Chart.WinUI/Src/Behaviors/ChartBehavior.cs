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
    /// 
    /// </summary>
    public abstract class ChartBehavior : DependencyObject
    {
        #region Fields

        #region Protected Fields

        /// <summary>
        /// 
        /// </summary>
        internal ChartAxis chartAxis;

        #endregion

        #region Private Fields

        private const int axisTipHeight = 6;

        private Canvas adorningCanvas;

        private ChartBase chartArea;

        #endregion

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public ChartBehavior()
        {
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        internal Canvas AdorningCanvas
        {
            get
            {
                return adorningCanvas;
            }

            set
            {
                if (adorningCanvas != value)
                {
                    adorningCanvas = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal ChartBase Chart
        {
            get
            {
                return chartArea;
            }

            set
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
            Chart = null;
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

            if (control.Content is ChartPointInfo chartPointInfo)
            {
                if (!chartAxis.IsVertical)
                {
                    polygonPoints.Add(new Point(labelWidth, 0));

                    if (chartAxis.OpposedPosition)
                    {
                        double tipPosition, leftTipWidth, rightTipWidth;
                        if (chartPointInfo.Axis.CrosshairLabelTemplate == null && !isChartTrackBallBehavior)
                            control.Margin = new Thickness().GetThickness(0, -6, 0, 0);

                        if (chartPointInfo.X - control.DesiredSize.Height <= Chart.SeriesClipRect.X)
                        {
                            if (isChartTrackBallBehavior)
                                AlignDefaultLabel(
                                    ChartAlignment.Far,
                                    ChartAlignment.Far,
                                    Chart.SeriesClipRect.X,
                                    chartPointInfo.Y,
                                    control);
                            else
                                AlignDefaultLabel(
                                    verticalAlignemnt,
                                    ChartAlignment.Far,
                                    Chart.SeriesClipRect.X,
                                    chartPointInfo.Y,
                                    control);

                            tipPosition = chartPointInfo.BaseX - Chart.SeriesClipRect.X;
                            leftTipWidth = tipPosition - axisTipHypotenuse / 2;
                            rightTipWidth = tipPosition + axisTipHypotenuse / 2;

                            if (leftTipWidth < 0)
                                leftTipWidth = 0;

                            polygonPoints.Insert(2, (new Point(leftTipWidth, labelHeight)));
                            polygonPoints.Insert(3, (new Point(tipPosition, labelHeight + axisTipHeight)));
                            polygonPoints.Insert(4, (new Point(rightTipWidth, labelHeight)));
                        }
                        else if (chartPointInfo.X + control.DesiredSize.Width
                            >= Chart.SeriesClipRect.X + Chart.SeriesClipRect.Width)
                        {
                            if (isChartTrackBallBehavior)
                                AlignDefaultLabel(
                                    ChartAlignment.Far,
                                    ChartAlignment.Near,
                                    Chart.SeriesClipRect.X + Chart.SeriesClipRect.Width,
                                    chartPointInfo.Y,
                                    control);
                            else
                                AlignDefaultLabel(
                                    verticalAlignemnt,
                                    ChartAlignment.Near,
                                    Chart.SeriesClipRect.X + Chart.SeriesClipRect.Width,
                                    chartPointInfo.Y,
                                    control);

                            tipPosition = chartPointInfo.BaseX - chartPointInfo.X;
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
                        if (chartPointInfo.Axis.CrosshairLabelTemplate == null && !isChartTrackBallBehavior)
                            control.Margin = new Thickness().GetThickness(0, 6, 0, 0);

                        if (chartPointInfo.X - control.DesiredSize.Height <= Chart.SeriesClipRect.X)
                        {
                            AlignDefaultLabel(
                                ChartAlignment.Far,
                                ChartAlignment.Far,
                                Chart.SeriesClipRect.X,
                                chartPointInfo.Y,
                                control);

                            tipPosition = chartPointInfo.BaseX - Chart.SeriesClipRect.X;
                            leftTipWidth = tipPosition - axisTipHypotenuse / 2;
                            rightTipWidth = tipPosition + axisTipHypotenuse / 2;

                            if (leftTipWidth < 0)
                                leftTipWidth = 0;

                            polygonPoints.Insert(4, (new Point(rightTipWidth, 0)));
                            polygonPoints.Insert(5, (new Point(tipPosition, -axisTipHeight)));
                            polygonPoints.Insert(6, (new Point(leftTipWidth, 0)));
                        }
                        else if (chartPointInfo.X + control.DesiredSize.Width
                            >= Chart.SeriesClipRect.X + Chart.SeriesClipRect.Width)
                        {
                            AlignDefaultLabel(
                                ChartAlignment.Far,
                                ChartAlignment.Near,
                                Chart.SeriesClipRect.X + Chart.SeriesClipRect.Width,
                                chartPointInfo.Y,
                                control);

                            tipPosition = (chartPointInfo.BaseX - chartPointInfo.X);
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
                    chartPointInfo.PolygonPoints = polygonPoints;
                }
                else
                {
                    if (chartAxis.OpposedPosition)
                    {
                        if (chartPointInfo.Axis.CrosshairLabelTemplate == null && !isChartTrackBallBehavior)
                            control.Margin = new Thickness().GetThickness(6, 0, 0, 0);

                        if (chartPointInfo.Y - control.DesiredSize.Height <= Chart.SeriesClipRect.Y)
                        {
                            if (isChartTrackBallBehavior)
                                AlignDefaultLabel(
                                    ChartAlignment.Far,
                                    ChartAlignment.Far,
                                    chartPointInfo.X,
                                    Chart.SeriesClipRect.Y,
                                    control);
                            else
                                AlignDefaultLabel(
                                    ChartAlignment.Far,
                                    horizontalAlignment,
                                    chartPointInfo.X,
                                    Chart.SeriesClipRect.Y,
                                    control);

                            double tipPosition = chartPointInfo.BaseY - Chart.SeriesClipRect.Y;
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
                        else if (chartPointInfo.Y + control.DesiredSize.Height
                            >= Chart.SeriesClipRect.Y + Chart.SeriesClipRect.Height)
                        {
                            if (isChartTrackBallBehavior)
                                AlignDefaultLabel(
                                    ChartAlignment.Near,
                                    ChartAlignment.Far,
                                    chartPointInfo.X,
                                    Chart.SeriesClipRect.Y + Chart.SeriesClipRect.Height,
                                    control);
                            else
                                AlignDefaultLabel(
                                    ChartAlignment.Near,
                                    horizontalAlignment,
                                    chartPointInfo.X,
                                    Chart.SeriesClipRect.Y + Chart.SeriesClipRect.Height,
                                    control);

                            double tipPosition = chartPointInfo.BaseY - chartPointInfo.Y;
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
                        if (chartPointInfo.Axis.CrosshairLabelTemplate == null && !isChartTrackBallBehavior)
                            control.Margin = new Thickness().GetThickness(-6, 0, 0, 0);

                        if (chartPointInfo.Y - control.DesiredSize.Height <= Chart.SeriesClipRect.Y)
                        {
                            if (isChartTrackBallBehavior)
                                AlignDefaultLabel(
                                    ChartAlignment.Far,
                                    ChartAlignment.Far,
                                    chartPointInfo.X,
                                    Chart.SeriesClipRect.Y,
                                    control);
                            else
                                AlignDefaultLabel(
                                    ChartAlignment.Far,
                                    horizontalAlignment,
                                    chartPointInfo.X,
                                    Chart.SeriesClipRect.Y,
                                    control);

                            double tipPosition = chartPointInfo.BaseY - Chart.SeriesClipRect.Y;
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
                        else if (chartPointInfo.Y + control.DesiredSize.Height
                            >= Chart.SeriesClipRect.Y + Chart.SeriesClipRect.Height)
                        {
                            if (isChartTrackBallBehavior)
                                AlignDefaultLabel(
                                    ChartAlignment.Near,
                                    ChartAlignment.Far,
                                    chartPointInfo.X,
                                    Chart.SeriesClipRect.Y + Chart.SeriesClipRect.Height,
                                    control);
                            else
                                AlignDefaultLabel(
                                    ChartAlignment.Near,
                                    horizontalAlignment,
                                    chartPointInfo.X,
                                    Chart.SeriesClipRect.Y + Chart.SeriesClipRect.Height,
                                    control);

                            double tipPosition = chartPointInfo.BaseY - chartPointInfo.Y;
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

                    chartPointInfo.PolygonPoints = polygonPoints;
                }
            }
        }

#endregion
                
        #region Protected Internal Virtual Methods

        
        internal virtual void OnLayoutUpdated()
        {
        }

        
        internal virtual void DetachElement(UIElement element)
        {
            if (this.AdorningCanvas != null && this.AdorningCanvas.Children.Contains(element))
                this.AdorningCanvas.Children.Remove(element);
        }
            
        internal virtual void DetachElements()
        {
        }

       
        internal virtual void OnSizeChanged(SizeChangedEventArgs e)
        {
        }

        internal virtual void OnDragEnter(DragEventArgs e)
        {
        }

        internal virtual void OnDragLeave(DragEventArgs e)
        {
        }

        internal virtual void OnDragOver(DragEventArgs e)
        {
        }

        internal virtual void OnDrop(DragEventArgs e)
        {
        }

        internal virtual void OnGotFocus(RoutedEventArgs e)
        {
        }

        internal virtual void OnLostFocus(RoutedEventArgs e)
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnDoubleTapped(DoubleTappedRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnHolding(HoldingRoutedEventArgs e)
        {
        }

        internal virtual void OnKeyDown(KeyRoutedEventArgs e)
        {
        }

        internal virtual void OnKeyUp(KeyRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="e"></param>
        protected internal virtual void OnManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
        }

        internal virtual void OnManipulationInertiaStarting(ManipulationInertiaStartingRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnManipulationStarting(ManipulationStartingRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnPointerCanceled(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        internal virtual void OnPointerCaptureLost(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnPointerEntered(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnPointerExited(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnPointerMoved(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnPointerPressed(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnPointerReleased(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnPointerWheelChanged(PointerRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnRightTapped(RightTappedRoutedEventArgs e)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected internal virtual void OnTapped(TappedRoutedEventArgs e)
        {
        }

        internal virtual void AlignDefaultLabel(
            ChartAlignment verticalAlignemnt,
            ChartAlignment horizontalAlignment,
            double x,
            double y,
            ContentControl control)
        {
        }

#endregion

        internal virtual void AttachElements()
        {
        }

        #region Protected Methods

        internal void UpdateArea()
        {
            if (Chart != null)
            {
                Chart.ScheduleUpdate();
            }
        }
        
#endregion

#endregion
    }
}
