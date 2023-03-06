using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Syncfusion.Maui.Core.Internals;
using Microsoft.Maui.Graphics;
using System.Collections.Specialized;
using Syncfusion.Maui.Core;

namespace Syncfusion.Maui.Charts
{
    internal interface IPyramidChartDependent : IDatapointSelectionDependent, ITooltipDependent
    {
        #region Public Properties

        double GapRatio { get; set; }

        IList<Brush> PaletteBrushes { get; set; }

        Brush Stroke { get; set; }

        double StrokeWidth { get; set; }

        ChartLegendIconType LegendIcon { get; set; }

        bool ShowDataLabels { get; set; }

        Rect SeriesBounds { get; set; }
        
        #endregion

        #region Properties

        AreaBase Area { get; }

        ChartLegend? ChartLegend { get; }

        IPyramidDataLabelSettings DataLabelSettings { get; }

        PyramidDataLabelHelper DataLabelHelper { get; }

        bool ArrangeReverse { get; }

        internal bool SegmentsCreated { get; set; }

        #endregion

        #region Property Changed Methods

        void OnGapRatioChanged(object oldValue, object newValue)
        {
            SegmentsCreated=false;
            ScheduleUpdateChart();
        }

        void OnPaletteBrushChanged(object oldValue, object newValue)
        {
            if (Equals(oldValue, newValue))
            {
                return;
            }

            OnCustomBrushesChanged(oldValue as ObservableCollection<Brush>, newValue as ObservableCollection<Brush>);
            if (this is IChart iChart && iChart.ActualSeriesClipRect != Rect.Zero)//Not to call at load time
            {
                PaletteColorsChanged();
            }
        }

        void OnStrokeChanged(object oldValue, object newValue)
        {
            UpdateStrokeColor();
            InvalidateChart();
        }

        void OnStrOnStrokeWidthChanged(object oldValue, object newValue)
        {
            UpdateStrokeWidth();
            InvalidateChart();
        }

        #endregion

        #region Abstract methods

        void GenerateSegments();

        void UpdateLegendItemsSource(ObservableCollection<ILegendItem> legendItems);

        #endregion

        #region Methods

        private void UpdateColor()
        {
            foreach (var segment in Segments)
            {
                SetFillColor(segment);
            }
        }

        private void UpdateLegendIconColor()
        {
            var legend = ChartLegend;
            var legendItems = Area?.PlotArea.LegendItems;
            if (legend != null && legend.IsVisible && legendItems != null)
            {
                foreach (LegendItem legendItem in legendItems)
                {
                    if (legendItem != null)
                    {
                        legendItem.IconBrush = GetFillColor(legendItem.Index) ?? new SolidColorBrush(Colors.Transparent);
                    }
                }
            }
        }

        private void ResetSegment()
        {
            //Todo: Need to consider this case later.
        }

        private void AddSegment(object chartSegment)
        {
            var segment = chartSegment as ChartSegment;
            if (segment != null)
            {
                SetFillColor(segment);
                SetStrokeColor(segment);
                SetStrokeWidth(segment);
            }
        }

        private void RemoveSegment(object chartSegment)
        {
            //Todo: Need to consider this case later.
        }

        private void OnCustomBrushesChanged(ObservableCollection<Brush>? oldValue, ObservableCollection<Brush>? newValue)
        {
            if (oldValue != null)
            {
                oldValue.CollectionChanged -= CustomBrushes_CollectionChanged;
            }

            if (newValue != null)
            {
                newValue.CollectionChanged += CustomBrushes_CollectionChanged;
            }
        }

        private void CustomBrushes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (sender != null)
            {
                PaletteColorsChanged();
            }
        }

        private void PaletteColorsChanged()
        {
            UpdateColor();
            InvalidateChart();
            UpdateLegendIconColor();
        }

        private void UpdateStrokeColor()
        {
            foreach (var segment in Segments)
            {
                SetStrokeColor(segment);
            }
        }

        private void UpdateStrokeWidth()
        {
            foreach (var segment in Segments)
            {
                SetStrokeWidth(segment);
            }
        }

        private void SetStrokeWidth(ChartSegment segment)
        {
            segment.StrokeWidth = StrokeWidth;
        }

        private void SetStrokeColor(ChartSegment segment)
        {
            segment.Stroke = Stroke;
        }

        private Brush? GetSelectionBrush(int index)
        {
            if (SelectionBehavior != null)
            {
                return SelectionBehavior.GetSelectionBrush(index);
            }

            return null;
        }

        private void UpdateSelectedItems(int index)
        {
            SetFillColor(Segments[index]);
        }

        internal void Segments_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            e.ApplyCollectionChanges((index, obj) => AddSegment(obj), (index, obj) => RemoveSegment(obj), ResetSegment);
        }

        void ScheduleUpdateChart()
        {
            if (Area != null)
            {
                Area.NeedsRelayout = true;
                Area.ScheduleUpdateArea();
            }
        }

        Brush? GetFillColor(int index)
        {
            Brush? fillColor = null;

            //Source selection behavior check.
            if (fillColor == null)
                fillColor = GetSelectionBrush(index);

            if (fillColor == null && PaletteBrushes != null)
            {
                fillColor = PaletteBrushes.Count > 0 ? PaletteBrushes[index % PaletteBrushes.Count()] : new SolidColorBrush(Colors.Transparent);
            }

            return fillColor;
        }

        void LayoutSegments()
        {
            DataLabelHelper.ClearDefaultValues();

            foreach (var segment in Segments)
            {
                segment.OnLayout();
                if (ShowDataLabels)
                {
                    segment.OnDataLabelLayout();
                }
            }

            if (ShowDataLabels)
            {
                //Arrange smart alignment.
                DataLabelHelper.ArrangeElements();
            }
        }

        void ITooltipDependent.SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect chartBounds)
        {
            float xPosition = tooltipInfo.X;
            float yPosition = tooltipInfo.Y;
            float sizeValue = 1;
            float halfSizeValue = 0.5f;
            Rect targetRect = new Rect(xPosition - halfSizeValue, yPosition - halfSizeValue, sizeValue, sizeValue);
            tooltipInfo.TargetRect = targetRect;
        }

        internal int GetDataPointIndex(float pointX, float pointY)
        {
            int selectedDataPointIndex = -1;
            RectF chartBounds = this is IChart chart ? chart.ActualSeriesClipRect : Rect.Zero;
            for (int i = 0; i < Segments.Count; i++)
            {
                ChartSegment chartSegment = Segments[i];
                selectedDataPointIndex = chartSegment.GetDataPointIndex(pointX - chartBounds.Left - (float)SeriesBounds.Left, pointY - chartBounds.Top - (float)SeriesBounds.Top);
                if (selectedDataPointIndex >= 0)
                {
                    return selectedDataPointIndex;
                }
            }

            return selectedDataPointIndex;
        }

        void OnSelectionBehaviorPropertyChanged(object oldValue, object newValue);

        void OnShowDataLabelsChanged(object oldValue, object newValue)
        {
            SegmentsCreated = false;
            ScheduleUpdateChart();
        }

        #endregion

        #region Interface implementation

        void IDatapointSelectionDependent.SetFillColor(ChartSegment segment)
        {
            if (segment == null)
            {
                return;
            }

            var segmentIndex = segment.Index;
            segment.Fill = GetFillColor(segmentIndex);
        }

        void IDatapointSelectionDependent.UpdateSelectedItem(int index)
        {
            UpdateSelectedItems(index);
            UpdateLegendIconColor();
        }

        void InvalidateChart()
        {
            if (Area.PlotArea is PyramidChartArea plotArea)
            {
                plotArea.InvalidateChart();
            }
        }

        void IDatapointSelectionDependent.Invalidate()
        {
            InvalidateChart();
        }

        #endregion

        internal void DrawDataLabels(ICanvas canvas, Rect dirtyRect)
        {
            var dataLabelSettings = DataLabelSettings;
            if (dataLabelSettings == null) return;

            DataLabelHelper.OnDraw(canvas, dirtyRect);
        }
    }

    interface IPyramidLabels
    {
        // Data label at center position X
        float DataLabelX { get; set; }
        // Data label at center position Y
        float DataLabelY { get; set; }

        //Points to draw connector lines.
        Point[]? LinePoints { get; set; }

        //As pyramid, get mid point of the slope. 
        Point SlopePoint { get; }

        //Actual size of the label rect. 
        Rect LabelRect { get; set; }

        //Data label content. 
        string DataLabel { get; }

        //Measured size of the string / content. 
        Size DataLabelSize { get; set; }
       
        //Actual size of the data label with margin specified
        Size ActualLabelSize { get; set; }

        //Is label to visible or hide. 
        bool IsLabelVisible { get; set; }

        //Is label intersected with adjucent. 
        bool IsIntersected { get; set; }

        //Label string is null or empty.
        bool IsEmpty { get; set; }

        //Segment fill for data label brush.
        Brush Fill { get; }

        //Individual label placement. 
        DataLabelPlacement Position { get; set; }
    }
}