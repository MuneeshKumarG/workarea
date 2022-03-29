using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
    public partial class ChartSeries
    {
        #region Fields

        private IChart? chart;

        private bool isComplexYProperty;

        private delegate object? GetReflectedProperty(object obj, string[] paths);

        private ChartValueType xValueType;

        const string animationName = "ChartAnimation";

        private int actualSelectedIndex = -1;

        internal readonly float DefaultSelectionStrokeWidth = 5;//Todo: check necessary this default value

        #endregion

        #region Internal Properties

        internal readonly ObservableCollection<ChartSegment> Segments;
        
        internal IChart? Chart { 
            get
            {
                return chart;
            }
            set
            {
                if(chart != null)
                {
                    if(value == null)
                    {
                        chart = value;
                    }
                    else
                    {
                        //Todo:Need to get content review below exception message.
                        throw new ArgumentException("ChartSeries is already the child of another Chart.");
                    }
                }
                else
                {
                    chart = value;
                }
            }
        }

        internal Rect AreaBounds => Chart != null ? Chart.ActualSeriesClipRect : Rect.Zero;

        internal virtual bool IsMultipleYPathRequired
        {
            get
            {
                return false;
            }
        }

        internal virtual bool IsSideBySide => false;

        internal double XData { get; set; }

        internal int PointsCount { get; set; }

        internal ChartValueType XValueType
        {
            get
            {
                return xValueType;
            }

            set
            {
                xValueType = value;
            }
        }

        internal IEnumerable? XValues { get; set; }

        internal bool SegmentsCreated { get; set; }

        internal IList<Brush>? PaletteColors { get; set; }

        internal DoubleRange VisibleXRange { get; set; }

        internal DoubleRange VisibleYRange { get; set; }

        internal string[][]? YComplexPaths { get; private set; }

        internal IEnumerable? ActualXValues { get; set; }

        internal virtual ChartDataLabelSettings? ChartDataLabelSettings
        {
            get
            {
                return null;
            }
        }

        internal IList<double>[]? SeriesYValues { get; private set; }

        internal IList<double>[]? ActualSeriesYValues { get; private set; }

        internal string[]? YPaths { get; private set; }

        internal List<object>? ActualData { get; set; }

        internal string[]? XComplexPaths { get; set; }

        internal bool IsLinearData { get; set; } = true;

        internal float AnimationValue { get; set; } = 1;

        internal Animation? SeriesAnimation { get; set; }

        internal bool NeedToAnimateSeries { get; set; }

        internal bool NeedToAnimateDataLabel { get; set; }

        internal bool IsDataPointAddedDynamically { get; set; } = false;

        internal ObservableCollection<ChartSegment>? OldSegments { get; set; }

        internal DoubleRange PreviousXRange { get; set; }

        internal ChartSegment? SelectedSegment { get; set; }

        internal int TooltipDataPointIndex { get; set; }

        internal int PreviousSelectedIndex { get; set; } = -1;

        internal int ActualSelectedIndex
        {
            get
            {
                return actualSelectedIndex;
            }
            set
            {
                if (actualSelectedIndex == value)
                {
                    return;
                }

                actualSelectedIndex = value;
                SelectedIndex = actualSelectedIndex;
            }
        }
        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public virtual int GetDataPointIndex(float pointX, float pointY)
        {
            int selectedDataPointIndex = -1;
            RectF seriesBounds = AreaBounds;
            
            for (int i = 0; i < Segments.Count; i++)
            {
                ChartSegment chartSegment = Segments[i];
                selectedDataPointIndex = chartSegment.HitTest(pointX - seriesBounds.Left, pointY - seriesBounds.Top);
                if (selectedDataPointIndex >= 0)
                {
                    return selectedDataPointIndex;
                }
            }

            return selectedDataPointIndex;
        }

        /// <summary>
        ///
        /// </summary>
        internal bool HitTest(float pointX, float pointY)
        {
            return GetDataPointIndex(pointX, pointY) >= 0;
        }

        #endregion

        #region Internal Virtual Methods

        internal virtual float TransformToVisibleX(double x, double y) => 0f;

        internal virtual float TransformToVisibleY(double x, double y) => 0f;

        internal virtual void OnDataSourceChanged(object oldValue, object newValue)
        {
        }

        internal virtual void GenerateDataPoints()
        {
        }

        internal virtual void OnDataSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            IsDataPointAddedDynamically = false;

            e.ApplyCollectionChanges((index, obj) => AddDataPoint(index, obj), (index, obj) => RemoveData(index), ResetDataPoint);

            if (e.Action == NotifyCollectionChangedAction.Add && EnableAnimation && AnimationDuration > 0)
            {
                IsDataPointAddedDynamically = true;
                NeedToAnimateSeries = true;
            }

            if (IsSideBySide)
            {
                CartesianSeries? series = this as CartesianSeries;

                if (series != null && series.ChartArea != null)
                {
                    series.InvalidateSideBySideSeries();
                    series.ChartArea.ResetSBSSegments();
                }
            }
            else
            {
                SegmentsCreated = false;
            }

            UpdateLegendItems();
            ScheduleUpdateChart();
        }

        internal virtual void RemoveData(int index)
        {
            if (XValues is IList<double>)
            {
                ((IList<double>)XValues).RemoveAt(index);
                PointsCount--;
            }
            else if (XValues is IList<string>)
            {
                ((IList<string>)XValues).RemoveAt(index);
                PointsCount--;
            }

            for (var i = 0; i < SeriesYValues?.Count(); i++)
            {
                SeriesYValues[i].RemoveAt(index);
            }

            ActualData?.RemoveAt(index);
        }

        internal virtual void UpdateRange()
        {
        }

        internal virtual void OnBindingPathChanged()
        {
            UpdateLegendItems();
            SegmentsCreated = false;
            ScheduleUpdateChart();
        }

        internal virtual void GenerateSegments(SeriesView seriesView)
        {
        }

        //TODO:Need to remove the replace parameter from this method,
        //becuase new notify collectionchanged event first remove
        //the data and then insert the data. So no need replace parameter here after.
        internal virtual void SetIndividualPoint(int index, object value, bool replace)
        {
            if (SeriesYValues != null && YPaths != null && ItemsSource != null)
            {
                var xvalueType = GetArrayPropertyValue(value, XComplexPaths);
                if (xvalueType != null)
                {
                    XValueType = GetDataType(xvalueType);
                }

                double yData;
                if (IsMultipleYPathRequired)
                {
                    switch (XValueType)
                    {
                        case ChartValueType.String:
                            {
                                if (!(XValues is List<string>))
                                {
                                    XValues = ActualXValues = new List<string>();
                                }

                                var xValue = (List<string>)XValues;
                                var xVal = GetArrayPropertyValue(value, XComplexPaths);
                                var xData = xVal as string;

                                if (replace && xValue.Count > index)
                                {
                                    xValue[index] = xData.Tostring();
                                }
                                else
                                {
                                    xValue.Insert(index, xData.Tostring());
                                }

                                for (var i = 0; i < YPaths.Length; i++)
                                {
                                    var yVal = YComplexPaths == null ? value : GetArrayPropertyValue(value, YComplexPaths[i]);

                                    yData = Convert.ToDouble(yVal ?? double.NaN);
                                    if (replace && SeriesYValues[i].Count > index)
                                    {
                                        SeriesYValues[i][index] = yData;
                                    }
                                    else
                                    {
                                        SeriesYValues[i].Insert(index, yData);
                                    }
                                }

                                PointsCount = xValue.Count;
                            }

                            break;
                        case ChartValueType.Double:
                        case ChartValueType.Logarithmic:
                            {
                                if (!(XValues is List<double>))
                                {
                                    XValues = ActualXValues = new List<double>();
                                }

                                IList<double> xValue = (List<double>)XValues;
                                var xVal = GetArrayPropertyValue(value, XComplexPaths);
                                XData = Convert.ToDouble(xVal ?? double.NaN);

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                if (replace && xValue.Count > index)
                                {
                                    xValue[index] = XData;
                                }
                                else
                                {
                                    xValue.Insert(index, XData);
                                }

                                for (var i = 0; i < YPaths.Length; i++)
                                {
                                    var yVal = YComplexPaths == null ? value : GetArrayPropertyValue(value, YComplexPaths[i]);
                                    yData = Convert.ToDouble(yVal ?? double.NaN);
                                    if (replace && SeriesYValues[i].Count > index)
                                    {
                                        SeriesYValues[i][index] = yData;
                                    }
                                    else
                                    {
                                        SeriesYValues[i].Insert(index, yData);
                                    }
                                }

                                PointsCount = xValue.Count;
                            }

                            break;
                        case ChartValueType.DateTime:
                            {
                                if (!(XValues is List<double>))
                                {
                                    XValues = ActualXValues = new List<double>();
                                }

                                IList<double> xValue = (List<double>)XValues;
                                var xVal = GetArrayPropertyValue(value, XComplexPaths);
                                XData = Convert.ToDateTime(xVal).ToOADate();

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                if (replace && xValue.Count > index)
                                {
                                    xValue[index] = XData;
                                }
                                else
                                {
                                    xValue.Insert(index, XData);
                                }

                                for (var i = 0; i < YPaths.Length; i++)
                                {
                                    var yVal = YComplexPaths == null ? value : GetArrayPropertyValue(value, YComplexPaths[i]);
                                    yData = Convert.ToDouble(yVal ?? double.NaN);
                                    if (replace && SeriesYValues[i].Count > index)
                                    {
                                        SeriesYValues[i][index] = yData;
                                    }
                                    else
                                    {
                                        SeriesYValues[i].Insert(index, yData);
                                    }
                                }

                                PointsCount = xValue.Count;
                            }

                            break;
                        case ChartValueType.TimeSpan:
                            {
                                //TODO: Implement on timespan support. 
                            }

                            break;
                    }
                }
                else
                {
                    var tempYPath = YComplexPaths != null ? YComplexPaths[0] : null;
                    var yValue = SeriesYValues[0];
                    switch (XValueType)
                    {
                        case ChartValueType.String:
                            {
                                if (!(this.XValues is List<string>))
                                {
                                    XValues = ActualXValues = new List<string>();
                                }

                                IList<string> xValue = (List<string>)XValues;
                                var xVal = GetArrayPropertyValue(value, XComplexPaths);
                                var yVal = GetArrayPropertyValue(value, tempYPath);
                                yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;
                                if (replace && xValue.Count > index)
                                {
                                    xValue[index] = xVal.Tostring();
                                }
                                else
                                {
                                    xValue.Insert(index, xVal.Tostring());
                                }

                                if (replace && yValue.Count > index)
                                {
                                    yValue[index] = yData;
                                }
                                else
                                {
                                    yValue.Insert(index, yData);
                                }

                                PointsCount = xValue.Count;
                            }

                            break;
                        case ChartValueType.Double:
                        case ChartValueType.Logarithmic:
                            {
                                if (!(XValues is List<double>))
                                {
                                    XValues = ActualXValues = new List<double>();
                                }

                                IList<double> xValue = (List<double>)XValues;
                                var xVal = GetArrayPropertyValue(value, XComplexPaths);
                                var yVal = GetArrayPropertyValue(value, tempYPath);
                                XData = xVal != null ? Convert.ToDouble(xVal) : double.NaN;
                                yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                if (replace && xValue.Count > index)
                                {
                                    xValue[index] = XData;
                                }
                                else
                                {
                                    xValue.Insert(index, XData);
                                }

                                if (replace && yValue.Count > index)
                                {
                                    yValue[index] = yData;
                                }
                                else
                                {
                                    yValue.Insert(index, yData);
                                }

                                PointsCount = xValue.Count;
                            }

                            break;
                        case ChartValueType.DateTime:
                            {
                                if (!(this.XValues is List<double>))
                                {
                                    this.XValues = ActualXValues = new List<double>();
                                }

                                IList<double> xValue = (List<double>)XValues;
                                var xVal = GetArrayPropertyValue(value, XComplexPaths);
                                var yVal = GetArrayPropertyValue(value, tempYPath);
                                XData = Convert.ToDateTime(xVal).ToOADate();
                                yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                if (replace && xValue.Count > index)
                                {
                                    xValue[index] = XData;
                                }
                                else
                                {
                                    xValue.Insert(index, XData);
                                }

                                if (replace && yValue.Count > index)
                                {
                                    yValue[index] = yData;
                                }
                                else
                                {
                                    yValue.Insert(index, yData);
                                }

                                PointsCount = xValue.Count;
                            }

                            break;
                        case ChartValueType.TimeSpan:
                            {
                                //TODO: Ensure on time span implementation.
                            }

                            break;
                    }
                }

                if (ActualData != null)
                {
                    if (replace && ActualData.Count > index)
                    {
                        ActualData[index] = value;
                    }
                    else if (ActualData.Count == index)
                    {
                        ActualData.Add(value);
                    }
                    else
                    {
                        ActualData.Insert(index, value);
                    }
                }
            }
        }

        internal virtual void SetStrokeColor(ChartSegment segment)
        {
        }

        internal virtual void SetStrokeWidth(ChartSegment segment)
        {
        }

        internal virtual void SetDashArray(ChartSegment segment)
        {
        }

        internal virtual Brush? GetFillColor(int index)
        {
            Brush? fillColor = null;

            if (Fill != null)
            {
                fillColor = Fill;
            }
            else if (PaletteColors != null)
            {
                fillColor = PaletteColors.Count > 0 ? PaletteColors[index % PaletteColors.Count()] : new SolidColorBrush(Colors.Transparent);
            }

            return fillColor;
        }

        private void SetFillColor(ChartSegment segment)
        {
            if (segment == null)
            {
                return;
            }

            var segmentIndex = Segments.IndexOf(segment);
            segment.Fill = IsIndividualSegment() && segmentIndex == SelectedIndex && SelectionBrush != null ? SelectionBrush : GetFillColor(segmentIndex);
        }

        internal virtual bool SeriesContainsPoint(PointF point)
        {
            if (PointsCount == 0)
            {
                return false;
            }

            TooltipDataPointIndex = GetDataPointIndex(point.X, point.Y);
            return TooltipDataPointIndex < PointsCount && TooltipDataPointIndex > -1;
        }

        internal virtual void Dispose()
        {
            HookAndUnhookCollectionChangedEvent(ItemsSource, null);

            if (SeriesAnimation != null)
            {
                SeriesAnimation = null;
            }

            var customBrushes = PaletteBrushes as ObservableCollection<Brush>;

            if (customBrushes != null)
            {
                customBrushes.CollectionChanged -= CustomBrushes_CollectionChanged;
            }
        }

        internal virtual TooltipInfo? GetTooltipInfo(ChartTooltipBehavior tooltipBehavior, float x, float y)
        {
            return null;
        }

        internal virtual void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect seriesBounds)
        {
            float xPosition = tooltipInfo.X;
            float yPosition = tooltipInfo.Y;
            float sizeValue = 1;
            float halfSizeValue = 0.5f;
            Rect targetRect = new Rect(xPosition - halfSizeValue, yPosition - halfSizeValue, sizeValue, sizeValue);
            tooltipInfo.TargetRect = targetRect;
        }

        #endregion

        #region Internal Methods

        internal object? GetActualXValue(int index)
        {
            if (XValues == null || index > PointsCount)
            {
                return null;
            }

            if (XValueType == ChartValueType.String)
            {
                return ((IList<string>)XValues)[index];
            }
            else if (XValueType == ChartValueType.DateTime)
            {
                return DateTime.FromOADate(((IList<double>)XValues)[index]).ToString("MM/dd/yyyy");
            }
            else if (XValueType == ChartValueType.Double || XValueType == ChartValueType.Logarithmic)
            {
                //Logic is to cut off the 0 decimal value from the number.
                object label = ((List<double>)XValues)[index];
                var actualVal = (double)label;

                if (actualVal == (long)actualVal)
                {
                    label = (long)actualVal;
                }

                return label;
            }
            else
            {
                return ((IList)XValues)[index];
            }
        }

        internal void InvalidateSeries()
        {
            var plotArea = Chart?.Area.PlotArea as ChartPlotArea;

            if (plotArea != null)
            {
                foreach (SeriesView seriesView in plotArea.SeriesViews.Children)
                {
                    if (seriesView != null && this == seriesView.Series)
                    {
                        seriesView.InvalidateDrawable();
                        break;
                    }
                }
            }
        }

        internal void Invalidate()
        {
            var plotArea = Chart?.Area.PlotArea as ChartPlotArea;

            if (plotArea != null)
            {
                foreach (SeriesView seriesView in plotArea.SeriesViews.Children)
                {
                    if (seriesView != null && this == seriesView.Series)
                    {
                        seriesView.Invalidate();
                        break;
                    }
                }
            }
        }

        internal void UpdateStrokeColor()
        {
            foreach (var segment in Segments)
            {
                SetStrokeColor(segment);
            }
        }

        internal void UpdateStrokeWidth()
        {
            foreach (var segment in Segments)
            {
                SetStrokeWidth(segment);
            }
        }

        internal void UpdateDashArray()
        {
            foreach (var segment in Segments)
            {
                SetDashArray(segment);
            }
        }

        internal void ScheduleUpdateChart()
        {
            var area = chart?.Area;
            if (area != null)
            {
               area.NeedsRelayout = true;
               area.ScheduleUpdateArea();
            }
        }

        internal void GeneratePoints(string[] yPaths, params IList<double>[] yValueLists)
        {
            if (yPaths == null)
            {
                return;
            }

            IList<double>[]? yLists = null;
            isComplexYProperty = false;
            bool isArrayProperty = false;
            YComplexPaths = new string[yPaths.Length][];
            for (int i = 0; i < yPaths.Length; i++)
            {
                if (string.IsNullOrEmpty(yPaths[i]))
                {
                    return;
                }

                YComplexPaths[i] = yPaths[i].Split(new char[] { '.' });
                if (yPaths[i].Contains("."))
                {
                    isComplexYProperty = true;
                }

                if (yPaths[i].Contains("["))
                {
                    isArrayProperty = true;
                }
            }

            SeriesYValues = ActualSeriesYValues = yLists = yValueLists;

            this.YPaths = yPaths;

            if (ActualData == null)
            {
                ActualData = new List<object>();
            }

            if (ItemsSource != null && !string.IsNullOrEmpty(XBindingPath))
            {
                if (ItemsSource is IEnumerable)
                {
                    if (XBindingPath.Contains("[") || isArrayProperty)
                    {
                        GenerateComplexPropertyPoints(yPaths, yLists, GetArrayPropertyValue);
                    }
                    else if (XBindingPath.Contains(".") || isComplexYProperty)
                    {
                        GenerateComplexPropertyPoints(yPaths, yLists, GetPropertyValue);
                    }
                    else
                    {
                        GeneratePropertyPoints(yPaths, yLists);
                    }
                }
            }
        }

        internal void ResetData()
        {
            if (ActualXValues is IList && XValues is IList)
            {
                ((IList)XValues).Clear();
                ((IList)ActualXValues).Clear();
            }

            if (ActualSeriesYValues != null && ActualSeriesYValues.Any())
            {
                foreach (var list in ActualSeriesYValues)
                {
                    list?.Clear();
                }

                if (SeriesYValues != null)
                {
                    foreach (var list in SeriesYValues)
                    {
                        list?.Clear();
                    }
                }
            }

            PointsCount = 0;

            if (XBindingPath != null && YPaths != null && YPaths.Any())
            {
                Segments.Clear();
            }
        }

        internal void UpdateColor()
        {
            foreach (var segment in Segments)
            {
                SetFillColor(segment);
            }
        }

        internal void UpdateAlpha()
        {
            foreach (var segment in Segments)
            {
                segment.Opacity = (float)Opacity;
            }
        }

        internal void AnimateSeries(Action<double> callback)
        {
            if (SeriesAnimation == null) return;

            Animation? customAnimation = CreateAnimation(callback);

            if (customAnimation != null)
                SeriesAnimation.Add(0, 1, customAnimation);
        }

        internal bool CanAnimate()
        {
            return EnableAnimation && NeedToAnimateSeries;
        }

        internal virtual bool IsIndividualSegment()
        {
            return true;
        }

        internal void AddDefaultBehaviors(IChart chart)
        {
            if(chart == null) return;
            
        }

        #region DataLabels methods

        internal virtual void DrawDataLabels(ICanvas canvas)
        {
            var dataLabelSettings = ChartDataLabelSettings;

            if (dataLabelSettings == null) return;

            ChartDataLabelStyle labelStyle = dataLabelSettings.LabelStyle;

            foreach (ChartSegment datalabel in Segments)
            {
                if (!datalabel.InVisibleRange || datalabel.IsEmpty) continue;
                UpdateDataLabelAppearance(canvas, datalabel, dataLabelSettings, labelStyle);
            }
        }

        internal void UpdateDataLabelAppearance(ICanvas canvas, ChartSegment datalabel, ChartDataLabelSettings dataLabelSettings, ChartDataLabelStyle labelStyle)
        {
            if (labelStyle.Angle != 0)
            {
                float angle = (float)(labelStyle.Angle > 360 ? labelStyle.Angle % 360 : labelStyle.Angle);
                canvas.SaveState();
                canvas.Rotate(angle, datalabel.LabelPositionPoint.X, datalabel.LabelPositionPoint.Y);
            }

            //Setting label background properties.
            //Todo:// Need to confirm colors
            canvas.StrokeSize = (float)labelStyle.StrokeWidth;
            canvas.StrokeColor = labelStyle.Stroke.ToColor();

            //Setting label properties.
            var fillcolor = datalabel.Index == SelectedIndex && SelectionBrush != null ? SelectionBrush : labelStyle.IsBackgroundColorUpdated ? labelStyle.Background : dataLabelSettings.UseSeriesPalette ? datalabel.Fill : labelStyle.Background;
            
            DrawDataLabel(canvas, fillcolor, datalabel.DataLabel != null ? datalabel.DataLabel : string.Empty, datalabel.LabelPositionPoint, datalabel.Index);
        }

        internal virtual PointF GetDataLabelPosition(ChartSegment dataLabel, SizeF labelSize, PointF labelPosition, float padding)
        {
            var dataLabelSettings = ChartDataLabelSettings;

            if (dataLabelSettings == null) return labelPosition;

            if (dataLabelSettings.LabelPlacement == DataLabelPlacement.Outer)
            {
                labelPosition.Y = labelPosition.Y - (labelSize.Height / 2) - padding;
            }
            else if (dataLabelSettings.LabelPlacement == DataLabelPlacement.Inner || (dataLabelSettings.LabelPlacement == DataLabelPlacement.Auto && this is ColumnSeries))
            {
                labelPosition.Y = labelPosition.Y + (labelSize.Height / 2) + padding;
            }

            return labelPosition;
        }

        internal void OnDataLabelSettingsPropertyChanged(ChartDataLabelSettings? oldValue, ChartDataLabelSettings? newValue)
        {
            if (oldValue != null)
            {
                oldValue.PropertyChanged -= DataLabelSettings_PropertyChanged;

                if (oldValue.LabelStyle != null)
                    oldValue.LabelStyle.PropertyChanged -= LabelStyle_PropertyChanged;
            }

            if (newValue != null)
            {
                newValue.PropertyChanged += DataLabelSettings_PropertyChanged;
                SetInheritedBindingContext(newValue, BindingContext);

                if (newValue.LabelStyle != null)
                {
                    newValue.LabelStyle.PropertyChanged += LabelStyle_PropertyChanged;
                    SetInheritedBindingContext(newValue.LabelStyle, BindingContext);
                }
            }

            if (AreaBounds != Rect.Zero)
            {
                InvalidateMeasureDataLabel();
                InvalidateDataLabel();
            }
        }

        internal void DataLabelSettings_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var dataLabelSettings = sender as ChartDataLabelSettings;

            if (dataLabelSettings == null) return;

            if (e.PropertyName != null && dataLabelSettings.IsNeedDataLabelMeasure.Contains(e.PropertyName))
            {
                InvalidateMeasureDataLabel();

                if (e.PropertyName == nameof(dataLabelSettings.LabelStyle))
                {
                    dataLabelSettings.LabelStyle.PropertyChanged += LabelStyle_PropertyChanged;
                }
            }            

            InvalidateDataLabel();
        }

        internal void UnhookDataLabelEvents(ChartDataLabelSettings dataLabelSettings)
        {
            if (dataLabelSettings != null)
            {
                if (dataLabelSettings.LabelStyle != null)
                {
                    dataLabelSettings.LabelStyle.PropertyChanged -= LabelStyle_PropertyChanged;
                }

                dataLabelSettings.PropertyChanged -= DataLabelSettings_PropertyChanged;
            }
        }

        internal void InvalidateMeasureDataLabel()
        {
            foreach (var segment in Segments)
            {
                segment.OnDataLabelLayout();
            }
        }

        internal void InvalidateDataLabel()
        {
            var chartPlotArea = chart?.Area.PlotArea as ChartPlotArea;

            if (chartPlotArea != null)
            {
                chartPlotArea.DataLabelView?.InvalidateDrawable();
            }
        }

        #endregion

        #endregion

        #region Internal Static Methods
        internal static ChartValueType GetDataType(object? xValue)
        {
            if (xValue is string || xValue is string[])
            {
                return ChartValueType.String;
            }
            else if (xValue is DateTime || xValue is DateTime[])
            {
                return ChartValueType.DateTime;
            }
            else if (xValue is TimeSpan || xValue is TimeSpan[])
            {
                return ChartValueType.TimeSpan;
            }
            else
            {
                return ChartValueType.Double;
            }
        }

        internal static ChartValueType GetDataType(IEnumerator enumerator, string[] paths)
        {
            object? parentObj = null;
            if (enumerator.MoveNext())
            {
                // GetArrayPropertyValue method is used to get value from the path of current object
                parentObj = GetArrayPropertyValue(enumerator.Current, paths);
            }

            return GetDataType(parentObj);
        }

        internal static ChartValueType GetDataType(FastReflection fastReflection, IEnumerable dataSource)
        {
            if (dataSource == null)
            {
                return ChartValueType.Double;
            }

            var enumerator = dataSource.GetEnumerator();
            object? obj = null;
            if (enumerator.MoveNext())
            {
                do
                {
                    obj = fastReflection.GetValue(enumerator.Current);
                }
                while (enumerator.MoveNext() && obj == null);
            }

            return GetDataType(obj);
        }

        #endregion

        #region Private Static Methods

        private static object? ReflectedObject(object? parentObj, string actualPath)
        {
            var fastReflection = new FastReflection();
            if (parentObj != null && fastReflection.SetPropertyName(actualPath, parentObj))
            {
                return fastReflection.GetValue(parentObj);
            }

            return null;
        }

        private static object? GetArrayPropertyValue(object obj, string[]? paths)
        {
            var parentObj = obj;

            if (paths == null)
                return parentObj;

            for (int i = 0; i < paths.Length; i++)
            {
                var path = paths[i];
                if (path.Contains("["))
                {
                    int index = Convert.ToInt32(path.Substring(path.IndexOf('[') + 1, path.IndexOf(']') - path.IndexOf('[') - 1));
                    string actualPath = path.Replace(path.Substring(path.IndexOf('[')), string.Empty);
                    parentObj = ReflectedObject(parentObj, actualPath);

                    if (parentObj == null)
                    {
                        return null;
                    }

                    var array = parentObj as IList;
                    if (array != null && array.Count > index)
                    {
                        parentObj = array[index];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    parentObj = ReflectedObject(parentObj, path);

                    if (parentObj == null)
                    {
                        return null;
                    }

                    if (parentObj.GetType().IsArray)
                    {
                        return null;
                    }
                }
            }

            return parentObj;
        }

        private static object? GetPropertyValue(object obj, string[] paths)
        {
            object? parentObj = obj;
            for (int i = 0; i < paths.Length; i++)
            {
                parentObj = ReflectedObject(parentObj, paths[i]);
            }

            if (parentObj != null)
            {
                if (parentObj.GetType().IsArray)
                {
                    return null;
                }
            }

            return parentObj;
        }


        #endregion

        #region Private Methods

        private void AddDataPoint(int index, object data)
        {
            SetIndividualPoint(index, data, false);
        }

        private void ResetDataPoint()
        {
            ResetData();

            if (ItemsSource != null)
            {
                var items = ItemsSource is IList ? ItemsSource as IList : null;
                if (items == null)
                {
                    var source = ItemsSource as IEnumerable;
                    if (source != null)
                    {
                        items = source.Cast<object>().ToList();
                    }
                }

                if (items != null && items.Count > 0)
                {
                    GenerateDataPoints();
                }
            }
        }

        private void PaletteColorsChanged()
        {
            UpdateColor();
            InvalidateSeries();

            if (ShowDataLabels)
            {
                InvalidateDataLabel();
            }

            UpdateLegendIconColor();
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

        internal virtual void HookAndUnhookCollectionChangedEvent(object oldValue, object? newValue)
        {
            if (newValue != null)
            {
                var newCollectionValue = newValue as INotifyCollectionChanged;
                if (newCollectionValue != null)
                {
                    newCollectionValue.CollectionChanged += OnDataSource_CollectionChanged;
                }
            }

            if (oldValue != null)
            {
                var oldCollectionValue = oldValue as INotifyCollectionChanged;
                if (oldCollectionValue != null)
                {
                    oldCollectionValue.CollectionChanged -= OnDataSource_CollectionChanged;
                }
            }
        }

        internal virtual void UpdateLegendIconColor()
        {

        }

        private void GeneratePropertyPoints(string[] yPaths, IList<double>[] yLists)
        {
            var enumerable = ItemsSource as IEnumerable;
            var enumerator = enumerable?.GetEnumerator();

            if (enumerable != null && enumerator != null && enumerator.MoveNext())
            {
                var currObj = enumerator.Current;

                FastReflection xProperty = new FastReflection();

                if (!xProperty.SetPropertyName(XBindingPath, currObj) || xProperty.IsArray(currObj))
                {
                    return;
                }

                XValueType = GetDataType(xProperty, enumerable);

                if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
                    XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
                {
                    if (!(ActualXValues is List<double>))
                    {
                        this.ActualXValues = this.XValues = new List<double>();
                    }
                }
                else
                {
                    if (!(ActualXValues is List<string>))
                    {
                        this.ActualXValues = this.XValues = new List<string>();
                    }
                }

                if (IsMultipleYPathRequired)
                {
                    var yPropertyAccessor = new List<FastReflection>();
                    if (string.IsNullOrEmpty(yPaths[0]))
                    {
                        return;
                    }

                    for (int i = 0; i < yPaths.Length; i++)
                    {
                        var fastReflection = new FastReflection();
                        if (!fastReflection.SetPropertyName(yPaths[i], currObj) || fastReflection.IsArray(currObj))
                        {
                            return;
                        }

                        yPropertyAccessor.Add(fastReflection);
                    }

                    if (XValueType == ChartValueType.String)
                    {
                        var xValue = this.XValues as List<string>;
                        if (xValue != null)
                        {
                            do
                            {
                                var xVal = xProperty.GetValue(enumerator.Current);
                                xValue.Add(xVal.Tostring());
                                for (int i = 0; i < yPropertyAccessor.Count; i++)
                                {
                                    var yVal = yPropertyAccessor[i].GetValue(enumerator.Current);
                                    yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                                }

                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }
                    else if (XValueType == ChartValueType.Double ||
                             XValueType == ChartValueType.Logarithmic)
                    {
                        var xValue = this.XValues as List<double>;
                        if (xValue != null)
                        {
                            do
                            {
                                var xVal = xProperty.GetValue(enumerator.Current);
                                XData = Convert.ToDouble(xVal ?? double.NaN);

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                xValue.Add(XData);
                                for (int i = 0; i < yPropertyAccessor.Count; i++)
                                {
                                    var yVal = yPropertyAccessor[i].GetValue(enumerator.Current);
                                    yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                                }

                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }
                    else if (XValueType == ChartValueType.DateTime)
                    {
                        var xValue = this.XValues as List<double>;
                        if (xValue != null)
                        {
                            do
                            {
                                var xVal = xProperty.GetValue(enumerator.Current);
                                XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                xValue.Add(XData);
                                for (int i = 0; i < yPropertyAccessor.Count; i++)
                                {
                                    var yVal = yPropertyAccessor[i].GetValue(enumerator.Current);
                                    yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                                }

                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }
                    else if (XValueType == ChartValueType.TimeSpan)
                    {
                        //TODO: Ensure while providing timespan support.
                    }
                }
                else
                {
                    string yPath;

                    if (string.IsNullOrEmpty(yPaths[0]))
                    {
                        return;
                    }
                    else
                    {
                        yPath = yPaths[0];
                    }

                    var yProperty = new FastReflection();

                    if (!yProperty.SetPropertyName(yPath, currObj) || yProperty.IsArray(currObj))
                    {
                        return;
                    }

                    IList<double> yValue = yLists[0];

                    if (XValueType == ChartValueType.String)
                    {
                        var xValue = this.XValues as List<string>;
                        if (xValue != null)
                        {
                            do
                            {
                                var xVal = xProperty.GetValue(enumerator.Current);
                                var yVal = yProperty.GetValue(enumerator.Current);
                                xValue.Add(xVal.Tostring());
                                yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }
                    else if (XValueType == ChartValueType.DateTime)
                    {
                        var xValue = this.XValues as List<double>;
                        if (xValue != null)
                        {
                            do
                            {
                                var xVal = xProperty.GetValue(enumerator.Current);
                                var yVal = yProperty.GetValue(enumerator.Current);

                                XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                xValue.Add(XData);
                                yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }
                    else if (XValueType == ChartValueType.Double ||
                             XValueType == ChartValueType.Logarithmic)
                    {
                        var xValue = this.XValues as List<double>;
                        if (xValue != null)
                        {
                            do
                            {
                                var xVal = xProperty.GetValue(enumerator.Current);
                                var yVal = yProperty.GetValue(enumerator.Current);
                                XData = Convert.ToDouble(xVal ?? double.NaN);

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                xValue.Add(XData);
                                yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }

                    else if (XValueType == ChartValueType.TimeSpan)
                    {
                       //TODO: ensure while implementing timespane.
                    }
                }
            }
        }

        private void GenerateComplexPropertyPoints(string[] yPaths, IList<double>[] yLists, GetReflectedProperty? getPropertyValue)
        {
            var enumerable = ItemsSource as IEnumerable;
            var enumerator = enumerable?.GetEnumerator();

            if (enumerable != null && enumerator != null && getPropertyValue != null && enumerator.MoveNext() && XComplexPaths != null && YComplexPaths != null)
            {
                XValueType = GetDataType(enumerator, XComplexPaths);
                if (XValueType == ChartValueType.DateTime || XValueType == ChartValueType.Double ||
                    XValueType == ChartValueType.Logarithmic || XValueType == ChartValueType.TimeSpan)
                {
                    if (!(XValues is List<double>))
                    {
                        this.ActualXValues = this.XValues = new List<double>();
                    }
                }
                else
                {
                    if (!(XValues is List<string>))
                    {
                        this.ActualXValues = this.XValues = new List<string>();
                    }
                }

                if (IsMultipleYPathRequired)
                {
                    if (string.IsNullOrEmpty(yPaths[0]))
                    {
                        return;
                    }

                    object? xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                    if (xVal == null)
                    {
                        return;
                    }

                    for (int i = 0; i < yPaths.Count(); i++)
                    {
                        var yPropertyValue = getPropertyValue(enumerator.Current, YComplexPaths[i]);
                        if (yPropertyValue == null)
                        {
                            return;
                        }
                    }

                    if (XValueType == ChartValueType.String)
                    {
                        var xValue = this.XValues as List<string>;
                        if (xValue != null)
                        {
                            do
                            {
                                xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                                xValue.Add(xVal.Tostring());
                                for (int i = 0; i < yPaths.Count(); i++)
                                {
                                    var yVal = getPropertyValue(enumerator.Current, YComplexPaths[i]);
                                    yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                                }

                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }
                    else if (XValueType == ChartValueType.Double ||
                       XValueType == ChartValueType.Logarithmic)
                    {
                        var xValue = this.XValues as List<double>;
                        if (xValue != null)
                        {
                            do
                            {
                                xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                                XData = Convert.ToDouble(xVal ?? double.NaN);

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                xValue.Add(XData);
                                for (int i = 0; i < yPaths.Count(); i++)
                                {
                                    var yVal = getPropertyValue(enumerator.Current, YComplexPaths[i]);
                                    yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                                }

                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }
                    else if (XValueType == ChartValueType.DateTime)
                    {
                        var xValue = this.XValues as List<double>;
                        if (xValue != null)
                        {
                            do
                            {
                                xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                                XData = xVal != null ? ((DateTime)xVal).ToOADate(): double.NaN;

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                xValue.Add(XData);
                                for (int i = 0; i < yPaths.Count(); i++)
                                {
                                    var yVal = getPropertyValue(enumerator.Current, YComplexPaths[i]);
                                    yLists[i].Add(Convert.ToDouble(yVal ?? double.NaN));
                                }

                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }
                    else if (XValueType == ChartValueType.TimeSpan)
                    {
                        //TODO:Validate on timespan implementation.
                    }
                }
                else
                {
                    string[] tempYPath = YComplexPaths[0];
                    if (string.IsNullOrEmpty(yPaths[0]))
                    {
                        return;
                    }

                    IList<double> yValue = yLists[0];
                    object? xVal, yVal;
                    if (XValueType == ChartValueType.String)
                    {
                        var xValue = this.XValues as List<string>;
                        if (xValue != null)
                        {
                            do
                            {
                                xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                                yVal = getPropertyValue(enumerator.Current, tempYPath);
                                if (xVal == null)
                                {
                                    return;
                                }

                                xValue.Add((string)xVal);
                                yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }
                    else if (XValueType == ChartValueType.Double ||
                        XValueType == ChartValueType.Logarithmic)
                    {
                        var xValue = this.XValues as List<double>;
                        if (xValue != null)
                        {
                            do
                            {
                                xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                                yVal = getPropertyValue(enumerator.Current, tempYPath);
                                XData = Convert.ToDouble(xVal ?? double.NaN);

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                xValue.Add(XData);
                                yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }
                    else if (XValueType == ChartValueType.DateTime)
                    {
                        var xValue = this.XValues as List<double>;
                        if (xValue != null)
                        {
                            do
                            {
                                xVal = getPropertyValue(enumerator.Current, XComplexPaths);
                                yVal = getPropertyValue(enumerator.Current, tempYPath);

                                XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

                                // Check the Data Collection is linear or not
                                if (IsLinearData && xValue.Count > 0 && XData <= xValue[xValue.Count - 1])
                                {
                                    IsLinearData = false;
                                }

                                xValue.Add(XData);
                                yValue.Add(Convert.ToDouble(yVal ?? double.NaN));
                                ActualData?.Add(enumerator.Current);
                            }
                            while (enumerator.MoveNext());
                            PointsCount = xValue.Count;
                        }
                    }
                    else if (XValueType == ChartValueType.TimeSpan)
                    {
                        //TODO: Ensure for timespan;
                    }
                }
            }
        }

        private void OnAnimationPropertyChanged()
        {
            //Todo: Need to move this code in series propertychanged event.
            //if (EnableAnimation && SeriesAnimation == null)
            //{
            //    SeriesAnimation = new Animation(OnAnimationStart);
            //}
            //else if (!EnableAnimation && SeriesAnimation != null)
            //{
            //    AbortAnimation();
            //}
        }

        

        private void LabelStyle_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var labelStyle = sender as ChartDataLabelStyle;

            if (e.PropertyName != null && labelStyle != null && labelStyle.NeedDataLabelMeasure(e.PropertyName))
            {
                InvalidateMeasureDataLabel();
            }

            InvalidateDataLabel();
        }

        private void UpdateLegendItems()
        {
            var area = chart?.Area;
            if (area != null && !area.AreaBounds.IsEmpty)
            {
                area.PlotArea.ShouldPopulateLegendItems = true;
            }

        }

        #endregion

        #endregion

    }
}
