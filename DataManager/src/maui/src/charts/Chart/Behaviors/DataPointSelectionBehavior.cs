using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Charts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Enables the selection of individual or multiple data points in a series.
    /// </summary>
    public class DataPointSelectionBehavior : ChartSelectionBehavior
    {
        #region Internal Fields

       internal IDatapointSelectionDependent? Source { get; set; }

        #endregion

        #region Internal Method

        internal bool OnTapped(float pointX, float pointY)
        {
            if (Source != null)
            {
                RectF bounds = Source.AreaBounds;
                foreach (var segment in Source.Segments)
                {
                    if (segment.HitTest(pointX - bounds.Left, pointY - bounds.Top))
                    {
                        var index = segment.Index;
                        if (IsSelectionChangingInvoked(Source, index))
                        {
                            UpdateSelectionChanging(index);
                            InvokeSelectionChangedEvent(Source, index);
                        }

                        return true;
                    }
                }
            }

            return false;
        }

        #endregion

        #region Internal Override Methods

        internal override void UpdateSelectedItem(int index)
        {
            if (Source != null && index != -1)
            {
                if (index < Source.Segments.Count && index > -1)
                {
                    Source.UpdateSelectedItem(index);
                    Source.Invalidate();
                }
            }
        }

        internal override void ChangeSelectionBrushColor(object newValue)
        {
            //TODO: Update for selection brush
        }

        internal override void ResetMultiSelection()
        {
            if (Source != null)
            {
                var selectedIndexes = ActualSelectedIndexes.ToList();
                ActualSelectedIndexes.Clear();
                foreach (var index in selectedIndexes)
                {
                    if (index < Source.Segments.Count && index > -1)
                        Source.SetFillColor(Source.Segments[index]);
                }
            }
        }

        internal override void SelectionIndexChanged(int oldValue, int newValue)
        {
            if (oldValue != -1)
            {
                UpdateSelectedItem(oldValue);
            }

            if (newValue != -1 && Source != null)
            {
                UpdateSelectedItem(newValue);
                InvokeSelectionChangedEvent(Source, newValue);
            }
        }

        #endregion
    }
}

