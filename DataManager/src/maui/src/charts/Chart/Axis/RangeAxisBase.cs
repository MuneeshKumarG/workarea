#if !WinUI
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
#endif
using System;
using System.Collections.Generic;

#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
    public abstract partial class RangeAxisBase : ChartAxis
    {
        #region Fields
        internal List<double> SmallTickPoints { get; }
        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        internal virtual void AddSmallTicksPoint(double position, double interval)
        {
            double tickInterval = interval / (MinorTicksPerInterval + 1);
            double endPosition = position + interval;
            double tickPosition = position;

            if (SmallTickPoints.Count == 0 && tickPosition > VisibleRange.Start)
            {
                double tickStartPosition = position;

                while (tickStartPosition > VisibleRange.Start &&
                       tickStartPosition < VisibleRange.End)
                {
                    if (!(tickStartPosition == position))
                    {
                        SmallTickPoints.Add(tickStartPosition);
                    }

                    tickStartPosition -= tickInterval;
                }
            }

            while (tickPosition < endPosition && tickPosition < VisibleRange.End)
            {
                if (!(tickPosition == position) && VisibleRange.Inside(tickPosition))
                {
                    SmallTickPoints.Add(tickPosition);
                }

                tickPosition += tickInterval;

                double roundTickposition = Math.Round(tickPosition * 100000000) / 100000000.0;

                if (roundTickposition >= endPosition)
                {
                    tickPosition = endPosition;
                }
            }
        }
        #endregion
    }
}
