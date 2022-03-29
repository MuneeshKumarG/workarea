﻿using System;
#if !WPF
using Windows.Foundation;
#endif

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Class for <c>TrendlineSegment</c>.
    /// </summary>
    internal class TrendlineSegment : SplineSegment
    {
        #region Constructors

        [Obsolete("Use TrendlineSegment(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4, ChartSeriesBase series): base(point1, point2, point3, point4, series)")]
        public TrendlineSegment(Point point1, Point point2, Point point3, Point point4, ChartSeriesBase series)
             : base(point1, point2, point3, point4, series)
        {

        }

        public TrendlineSegment(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4, ChartSeriesBase series)
             : base(point1, point2, point3, point4, series)
        {

        }

        #endregion
    }
}
