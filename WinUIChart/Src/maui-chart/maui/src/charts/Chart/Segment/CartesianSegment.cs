using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class CartesianSegment : ChartSegment
    {
        #region Methods

        #region Animation Methods

        internal float GetDynamicAnimationValue(float animationValue, float value, float oldValue, float newValue)
        {
            if (!double.IsNaN(oldValue) && !double.IsNaN(newValue))
            {
                return (float)((newValue > oldValue) ?
                    oldValue + ((newValue - oldValue) * animationValue)
                    : oldValue - ((oldValue - newValue) * animationValue));
            }
            else
            {
                return double.IsNaN(oldValue) ? (float)newValue : (float)(oldValue - (oldValue * animationValue));
            }
        }

        internal void AnimateSeriesClipRect(ICanvas canvas, float animationValue)
        {
            CartesianSeries? cartesianSeries = Series as CartesianSeries;

            if (cartesianSeries != null && cartesianSeries.EnableAnimation && cartesianSeries.ChartArea is CartesianChartArea chartArea)
            {
                RectF seriesClipRect = cartesianSeries.AreaBounds;

                if (chartArea.IsTransposed)
                {
                    canvas.ClipRectangle(0, seriesClipRect.Height - (seriesClipRect.Height * animationValue), seriesClipRect.Width, seriesClipRect.Height);
                }
                else
                {
                    canvas.ClipRectangle(0, 0, seriesClipRect.Right * animationValue, seriesClipRect.Bottom);
                }
            }
        }

        #endregion

        #region DataLabel Methods

        internal void CalculateDataLabelPosition(double xvalue, double yvalue)
        {
            var xyDataSeries = Series as XYDataSeries;
            var dataLabelSettings = xyDataSeries?.DataLabelSettings;

            if (xyDataSeries == null || xyDataSeries.ChartArea == null || !xyDataSeries.ShowDataLabels || dataLabelSettings == null) return;

            IsEmpty = double.IsNaN(yvalue);
            InVisibleRange = xyDataSeries.IsDataInVisibleRange(xvalue, yvalue);
            double x = xvalue, y = xyDataSeries.GetDataLabelPositionAtIndex(Index);
            xyDataSeries.CalculateDataPointPosition(Index, ref x, ref y);
            PointF labelPoint = new PointF((float)x, (float)y);
            DataLabelXPosition = x;
            DataLabelYPosition = y;
            DataLabel = dataLabelSettings.GetLabelContent(yvalue);
            LabelPositionPoint = dataLabelSettings.CalculateDataLabelPoint(xyDataSeries, this, labelPoint, dataLabelSettings.LabelStyle);
        }

        #endregion

        #endregion
    }
}
