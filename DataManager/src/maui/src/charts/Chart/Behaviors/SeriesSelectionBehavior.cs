using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Enables the selection of individual or multiple series in a <see cref="SfCartesianChart"/>.
    /// </summary>
    public class SeriesSelectionBehavior : ChartSelectionBehavior
    {
        #region Internal Properties

        internal SfCartesianChart? Chart { get; set; }

        internal IChartArea? chartArea { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesSelectionBehavior"/> class.
        /// </summary>
        public SeriesSelectionBehavior()
        {
        }

        #endregion

        #region Internal Methods

        internal bool OnTapped(float pointX, float pointY)
        {
            var visibleSeries = chartArea?.VisibleSeries;
            if (visibleSeries != null && Chart != null)
            {
                foreach (var series in visibleSeries.Reverse())
                {
                    RectF bounds = series.AreaBounds;
                    foreach(var segment in series.Segments)
                    {
                        if (segment.HitTest(pointX - bounds.Left, pointY - bounds.Top))
                        {
                            var index = visibleSeries.IndexOf(series);
                            if (IsSelectionChangingInvoked(Chart, index))
                            {
                                UpdateSelectionChanging(index);
                                InvokeSelectionChangedEvent(Chart, index);
                            }
                            
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        #endregion

        #region Internal Override Methods

        internal override void ChangeSelectionBrushColor(object newValue)
        {
            //TODO: Update for selection brush
        }

        internal override void UpdateSelectedItem(int index)
        {
            var visibleSeries = chartArea?.VisibleSeries;
            if (visibleSeries != null)
            {
                if (index < visibleSeries.Count && index > -1)
                {
                    var series = visibleSeries[index];
                    series.UpdateColor();
                    series.UpdateLegendIconColor(index);
                    Invalidate(series);
                }
            }
        }

        internal override void ResetMultiSelection()
        {
            var selectedIndexes = ActualSelectedIndexes.ToList();
            ActualSelectedIndexes.Clear();
            foreach (var index in selectedIndexes)
            {
                UpdateSelectedItem(index);
            }
        }

        internal override void SelectionIndexChanged(int oldValue, int newValue)
        {
            if (oldValue != -1)
            {
                UpdateSelectedItem(oldValue);
            }

            if (newValue != -1 && Chart != null)
            {
                UpdateSelectedItem(newValue);
                InvokeSelectionChangedEvent(Chart, newValue);
            }
        }

        #endregion
    }
}
