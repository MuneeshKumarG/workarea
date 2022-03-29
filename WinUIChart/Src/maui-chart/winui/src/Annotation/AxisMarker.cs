using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    internal class AxisMarker : ShapeAnnotation
    {
        #region Fields

        #region Internal Fields

        internal ContentControl markerContent;

        #endregion

        #endregion

        #region Properties

        #region Internal Properties

        internal Canvas MarkerCanvas { get; set; }

        internal ShapeAnnotation ParentAnnotation { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Updates the annotation
        /// </summary>
        public override void UpdateAnnotation()
        {
            if (markerContent != null)
            {
                Rect heightAndWidthRect;
                Point ensurePoint, positionedPoint;
                Size desiredSize;
                Point textPosition;
                ChartAxis axis = null;
                if ((XAxis != null && YAxis != null && X1 != null && Y1 != null && X2 != null && Y2 != null))
                {
                    markerContent.Visibility = Visibility.Visible;
                    if (XAxis.Orientation == Orientation.Vertical)
                    {
                        if (XAxis is LogarithmicAxis && ParentAnnotation is VerticalLineAnnotation)
                        {
                            x1 = Convert.ToDouble(X1);
                            x2 = Convert.ToDouble(X2);
                        }
                        else
                        {
                            x1 = ConvertData(X1, XAxis);
                            x2 = ConvertData(X2, XAxis);
                        }

                        if (YAxis is LogarithmicAxis && ParentAnnotation is HorizontalLineAnnotation)
                        {
                            y1 = Convert.ToDouble(Y1);
                            y2 = Convert.ToDouble(Y2);
                        }
                        else
                        {
                            y1 = ConvertData(Y1, YAxis);
                            y2 = ConvertData(Y2, YAxis);
                        }
                    }
                    else
                    {
                        if (XAxis is LogarithmicAxis && ParentAnnotation is HorizontalLineAnnotation)
                        {
                            x1 = Convert.ToDouble(X1);
                            x2 = Convert.ToDouble(X2);
                        }
                        else
                        {
                            x1 = ConvertData(X1, XAxis);
                            x2 = ConvertData(X2, XAxis);
                        }

                        if (YAxis is LogarithmicAxis && ParentAnnotation is VerticalLineAnnotation)
                        {
                            y1 = Convert.ToDouble(Y1);
                            y2 = Convert.ToDouble(Y2);
                        }
                        else
                        {
                            y1 = ConvertData(Y1, YAxis);
                            y2 = ConvertData(Y2, YAxis);
                        }
                    }

                    Point point = (XAxis.Orientation == Orientation.Horizontal)
                        ? new Point(
                          this.Chart.ValueToPointRelativeToAnnotation(XAxis, x1),
                          this.Chart.ValueToPointRelativeToAnnotation(YAxis, y1))
                        : new Point(
                          this.Chart.ValueToPointRelativeToAnnotation(YAxis, y1),
                          this.Chart.ValueToPointRelativeToAnnotation(XAxis, x1));
                    Point point2 = (XAxis.Orientation == Orientation.Horizontal) 
                        ? new Point(
                          Chart.ValueToPointRelativeToAnnotation(XAxis, x2),
                          this.Chart.ValueToPointRelativeToAnnotation(YAxis, y2)) 
                        : new Point(
                          this.Chart.ValueToPointRelativeToAnnotation(YAxis, y2),
                          Chart.ValueToPointRelativeToAnnotation(XAxis, x2));
                    point.Y = (double.IsNaN(point.Y)) ? 0 : point.Y;
                    point.X = (double.IsNaN(point.X)) ? 0 : point.X;
                    point2.Y = (double.IsNaN(point2.Y)) ? 0 : point2.Y;
                    point2.X = (double.IsNaN(point2.X)) ? 0 : point2.X;

                    if (ParentAnnotation is VerticalLineAnnotation)
                    {
                        markerContent.Content = GetXAxisContent();
                        axis = (XAxis.Orientation == Orientation.Horizontal) ? XAxis : YAxis;
                    }
                    else
                    {
                        axis = (XAxis.Orientation == Orientation.Vertical) ? XAxis : YAxis;
                        markerContent.Content = GetYAxisContent();
                    }

                    heightAndWidthRect = new Rect(point, point2);
                    ensurePoint = this.EnsurePoint(point, point2);
                    desiredSize = new Size(heightAndWidthRect.Width, heightAndWidthRect.Height);
                    positionedPoint = GetElementPosition(new Size(heightAndWidthRect.Width, heightAndWidthRect.Height), ensurePoint);
                    markerContent.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                    textPosition = GetAxisLabelPosition(desiredSize, positionedPoint, new Size(markerContent.DesiredSize.Width, markerContent.DesiredSize.Height));

                    if (axis.Visibility != Visibility.Collapsed)
                    {
                        Canvas.SetLeft(markerContent, textPosition.X);
                        Canvas.SetTop(markerContent, textPosition.Y);
                    }
                    else
                    {
                        markerContent.Visibility = Visibility.Collapsed;
                    }

                    RotatedRect = new Rect(
                        textPosition.X, 
                        textPosition.Y,
                        markerContent.DesiredSize.Width, 
                        markerContent.DesiredSize.Height);
                }
                else
                {
                    markerContent.Visibility = Visibility.Collapsed;
                }
            }
        }

        #endregion

        #region Internal Methods

        internal Point GetAxisLabelPosition(Size desiredSize, Point originalPosition, Size textSize)
        {
            Point point = originalPosition;
            if ((ParentAnnotation is HorizontalLineAnnotation && XAxis.Orientation == Orientation.Horizontal)||
                (ParentAnnotation is VerticalLineAnnotation && XAxis.Orientation == Orientation.Vertical))
            {
                ChartAxis axis = (XAxis.Orientation == Orientation.Vertical) ? XAxis : YAxis;
                ChartAxis axis1 = (XAxis.Orientation != Orientation.Vertical) ? XAxis : YAxis;
                point.Y -= GetHorizontalAxisLabelAlignment(textSize); 
                double left = 0;
                var chartAxes = Chart.InternalAxes.Where(axes => (axes.Orientation == axis.Orientation)).Where(position => (!position.OpposedPosition));
                point.X -= axis1.GetActualPlotOffsetStart();
                if (axis.OpposedPosition)
                {
                    if (axis.axisLabelsPanel != null)
                    {
                        point.X += axis.LabelPosition == AxisElementPosition.Inside ? axis.axisLabelsPanel.DesiredSize.Width : 0;
                    }

                    point.X += axis.TickLinesPosition == AxisElementPosition.Inside ? axis.TickLineSize : 0;
                    if (chartAxes.Count() > 0)
                        left = chartAxes.ElementAt(0).RenderedRect.Right;
                    point.X += (axis.RenderedRect.Left - left);
                    point.X -= (ParentAnnotation as StraightLineAnnotation).LabelPosition == AxisElementPosition.Outside ? 0 : textSize.Width;
                }
                else
                {
                    if (chartAxes.Count() > 0)
                        left = chartAxes.ElementAt(0).RenderedRect.Left;
                    point.X -= (textSize.Width + (left - axis.RenderedRect.Left));
                    point.X += (ParentAnnotation as StraightLineAnnotation).LabelPosition == AxisElementPosition.Outside ? 0 : textSize.Width;
                }
            }
            else
            {
                ChartAxis axis = (XAxis.Orientation == Orientation.Horizontal) ? XAxis : YAxis;
                ChartAxis axis1 = (XAxis.Orientation != Orientation.Horizontal) ? XAxis : YAxis;
                point.X += (desiredSize.Width / 2);
                point.X -= GetVerticalAxisLabelAlignment(textSize);
                point.Y = axis.OpposedPosition ? point.Y : 0;
                var chartAxes = Chart.InternalAxes.Where(axes => (axes.Orientation == axis.Orientation))
                        .Where(position => (position.OpposedPosition));
                double bottom = (chartAxes.Count() > 0) ? chartAxes.ElementAt(0).RenderedRect.Bottom : 0;
               
                if (axis.OpposedPosition)
                {
                    point.Y -= (textSize.Height);
                    if (Chart.InternalAxes.IndexOf(axis) != 0)
                        point.Y -= (bottom - axis.RenderedRect.Bottom);
                    point.Y -= axis1.GetActualPlotOffsetEnd();
                    point.Y += (ParentAnnotation as StraightLineAnnotation).LabelPosition == AxisElementPosition.Outside ? 0 : textSize.Height;
                }
                else
                {
                    if (axis.axisLabelsPanel != null)
                    {
                        point.Y += axis.LabelPosition == AxisElementPosition.Inside ? axis.axisLabelsPanel.DesiredSize.Height : 0;
                    }

                    point.Y += axis.TickLinesPosition == AxisElementPosition.Inside ? axis.TickLineSize : 0;
                    point.Y += (axis.RenderedRect.Top - bottom);
                    point.Y -= (ParentAnnotation as StraightLineAnnotation).LabelPosition == AxisElementPosition.Outside ? 0 : textSize.Height;
                } 
            }
            return point;
        }

        internal override UIElement CreateAnnotation()
        {
            if (MarkerCanvas == null)
            {
                MarkerCanvas = new Canvas();
                markerContent = new ContentControl();
                markerContent.ContentTemplate = (ParentAnnotation as StraightLineAnnotation).AxisLabelTemplate != null ? (ParentAnnotation as StraightLineAnnotation).AxisLabelTemplate :
                    ChartDictionaries.GenericCommonDictionary["AxisLabel"] as DataTemplate;
                this.CanDrag = ParentAnnotation.CanDrag;
                SetBindings();
                MarkerCanvas.Children.Add(markerContent);
            }

            return MarkerCanvas;
        }

        #endregion

        #region Private Methods

        private static bool CheckPointRange(double point, ChartAxis axis)
        {
            return (point <= axis.VisibleRange.End && point >= axis.VisibleRange.Start);
        }

        object GetXAxisContent()
        {
            markerContent.Visibility = !CheckPointRange(x1, XAxis) ? Visibility.Collapsed : Visibility.Visible;
            return XAxis is NumericalAxis || XAxis is LogarithmicAxis ? Convert.ToDecimal(X1).ToString("0.##") : XAxis.GetLabelContent(x1);
        }

        object GetYAxisContent()
        {
            markerContent.Visibility = !CheckPointRange(y1, YAxis) ? Visibility.Collapsed : Visibility.Visible;
            return YAxis is NumericalAxis || YAxis is LogarithmicAxis ? Convert.ToDecimal(Y1).ToString("0.##") : XAxis.GetLabelContent(y1);
        }

        private double GetVerticalAxisLabelAlignment(Size textSize)
        {
            double axisLabelPoinX = 0;
            var labelAlignment = (ParentAnnotation as StraightLineAnnotation).AxisLabelAlignment;

            switch (labelAlignment)
            {
                case LabelAlignment.Near:
                    axisLabelPoinX = textSize.Width;
                    break;
                case LabelAlignment.Center:
                    axisLabelPoinX = textSize.Width / 2;
                    break;
            }

            return axisLabelPoinX;
        }

        private double GetHorizontalAxisLabelAlignment(Size textSize)
        {
            double axisLabelPoinX = 0;
            var labelAlignment = (ParentAnnotation as StraightLineAnnotation).AxisLabelAlignment;
            switch (labelAlignment)
            {
                case LabelAlignment.Near:
                    axisLabelPoinX = textSize.Height;
                    break;
                case LabelAlignment.Center:
                    axisLabelPoinX = textSize.Height / 2;
                    break;
            }

            return axisLabelPoinX;
        }

        #endregion

        #endregion
    }
}
