using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    internal interface IDatapointSelectionDependent
    {
        DataPointSelectionBehavior? SelectionBehavior { get; }

        ObservableCollection<ChartSegment> Segments { get; }

        Rect AreaBounds { get; }

        void UpdateSelectedItem(int index);

        void SetFillColor(ChartSegment segment);

        void Invalidate();
    }
}

