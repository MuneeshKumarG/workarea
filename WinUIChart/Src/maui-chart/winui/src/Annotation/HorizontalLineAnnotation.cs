using System;
using Windows.Foundation;
#if WinUI_Desktop
using Microsoft.UI.Xaml.Controls;
#else
using Windows.UI.Xaml.Controls;
#endif
namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Provides a light weight UIElement that displays a horizontal line on chart. 
    /// </summary>
    /// <seealso cref="Syncfusion.UI.Xaml.Charts.StraightLineAnnotation" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes")]
    internal class HorizontalLineAnnotation : StraightLineAnnotation
    {
        #region Methods

        #region Public Override Methods

        /// <summary>
        /// Updates the annotation
        /// </summary>
        public override void UpdateAnnotation()
        {
            if (shape != null)
            {
                ValidateSelection();
                switch (CoordinateUnit)
                {
                    case CoordinateUnit.Axis:
                        SetAxisFromName();
                        if (XAxis != null && YAxis != null)
                        {
                            if (Chart.AnnotationManager != null && ShowAxisLabel
                                && !Chart.ChartAnnotationCanvas.Children.Contains(AxisMarkerObject.MarkerCanvas))
                                Chart.AnnotationManager.AddOrRemoveAnnotations(AxisMarkerObject, false);
                            if (XAxis.Orientation.Equals(Orientation.Vertical))
                            {
                                if (Y1 == null) break;
                                x1 = (X1 == null) ? XAxis.VisibleRange.Start : Annotation.ConvertData(X1, XAxis);
                                x2 = (X2 == null) ? XAxis.VisibleRange.End : Annotation.ConvertData(X2, XAxis);
                                Y2 = Y1;
                                y1 = Annotation.ConvertData(Y1, YAxis);
                                y2 = Annotation.ConvertData(Y2, YAxis);
                                if (ShowAxisLabel)
                                    SetAxisMarkerValue(XAxis.VisibleRange.End, XAxis.VisibleRange.Start, Y1, Y2, AxisMode.Horizontal);
                                this.DraggingMode = AxisMode.Horizontal;
                            }
                            else
                            {
                                if (Y1 == null) break;
                                x1 = (X1 == null) ? XAxis.VisibleRange.Start : Annotation.ConvertData(X1, XAxis);
                                x2 = (X2 == null) ? XAxis.VisibleRange.End : Annotation.ConvertData(X2, XAxis);
                                Y2 = Y1;
                                y1 = Annotation.ConvertData(Y1, YAxis);
                                y2 = Annotation.ConvertData(Y2, YAxis);
                                if (ShowAxisLabel)
                                    SetAxisMarkerValue(XAxis.VisibleRange.Start, XAxis.VisibleRange.End, Y1, Y2, AxisMode.Vertical);
                                this.DraggingMode = AxisMode.Vertical;
                            }

                            if (ShowAxisLabel)
                                AxisMarkerObject.UpdateAnnotation();
                            if (ShowLine)
                            {
                                if (CoordinateUnit == CoordinateUnit.Axis && EnableClipping)
                                {
                                    x1 = GetClippingValues(x1, XAxis);
                                    y1 = GetClippingValues(y1, YAxis);
                                    x2 = GetClippingValues(x2, XAxis);
                                    y2 = GetClippingValues(y2, YAxis);
                                }

                                Point point = (XAxis.Orientation.Equals(Orientation.Horizontal))
                                    ? new Point(
                                        this.Chart.ValueToPointRelativeToAnnotation(XAxis, x1) - XAxis.GetActualPlotOffsetStart(),
                                        this.Chart.ValueToPointRelativeToAnnotation(YAxis, y1))
                                    : new Point(
                                        this.Chart.ValueToPointRelativeToAnnotation(YAxis, y1),
                                        this.Chart.ValueToPointRelativeToAnnotation(XAxis, x1) + YAxis.GetActualPlotOffsetStart());
                                Point point2 = (XAxis.Orientation.Equals(Orientation.Horizontal))
                                    ? new Point(
                                        Chart.ValueToPointRelativeToAnnotation(XAxis, x2) + XAxis.GetActualPlotOffsetEnd(),
                                        this.Chart.ValueToPointRelativeToAnnotation(YAxis, y2))
                                    : new Point(
                                        this.Chart.ValueToPointRelativeToAnnotation(YAxis, y2),
                                        Chart.ValueToPointRelativeToAnnotation(XAxis, x2) - YAxis.GetActualPlotOffsetEnd());

                                DrawLine(point, point2, shape);
                            }
                        }

                        break;
                    case CoordinateUnit.Pixel:
                        if (ShowLine && this.Chart != null && this.Chart.AnnotationManager != null && Y1 != null)
                        {
                            this.DraggingMode = AxisMode.Vertical;
                            if (Y1 == null)
                                Y1 = 0;
                            Y2 = Y1;
                            X1 = (X1 == null) ? 0 : X1;
                            X2 = (X2 == null || Convert.ToDouble(X2) == 0) ? this.Chart.DesiredSize.Width : X2;
                            Point elementPoint1 = new Point(Convert.ToDouble(X1), Convert.ToDouble(Y1));
                            Point elementPoint2 = new Point(Convert.ToDouble(X2), Convert.ToDouble(Y2));
                            DrawLine(elementPoint1, elementPoint2, shape);
                        }

                        break;
                }

                if ((XAxis != null && XAxis.Orientation.Equals(Orientation.Vertical) && X1 == null && Y1 == null))
                {
                    ClearValues();
                    if (ShowAxisLabel)
                    {
                        AxisMarkerObject.ClearValue(AxisMarker.X1Property);
                        AxisMarkerObject.ClearValue(AxisMarker.X2Property);
                        AxisMarkerObject.ClearValue(AxisMarker.Y1Property);
                        AxisMarkerObject.ClearValue(AxisMarker.Y2Property);
                    }
                }
            }
        }

#endregion

#region Internal Override Methods
        
        /// <summary>
        /// Upates the hit rect.
        /// </summary>
        internal override void UpdateHitArea()
        {
            if (LinePoints == null) return;

            var p1 = LinePoints[0];
            var p2 = LinePoints[1];
            var ensurePoint = EnsurePoint(p1, p2);
            var width = Math.Abs(p2.X - p1.X);
            var height = Math.Abs(p2.Y - p1.Y);
            RotatedRect = new Rect(ensurePoint.X, ensurePoint.Y - GrabExtent, width, height + 2 * GrabExtent);
        }

#endregion

#endregion
    }
}
