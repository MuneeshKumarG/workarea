#if WinUI
using Microsoft.UI.Xaml.Media;
using System.Data;
#else
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

#if WinUI
namespace Syncfusion.UI.Xaml.Charts
#else
namespace Syncfusion.Maui.Charts
#endif
{
#if WinUI
    public partial class ChartSeries
#else
    public partial class ChartSeries
#endif
    {
        #region Fields

        #region Internal Fields

        internal delegate object? GetReflectedProperty(object obj, string[] paths);

        #endregion

        #region Private Fields

        private bool isComplexYProperty;

        private ChartValueType xValueType;

        private bool isRepeatPoint;

        #endregion

        #endregion

        #region DataManager Interface Implementation
#if !WinUI
        internal List<string> YDataPaths = new List<string>();
        internal List<string> XStringList = null;
        internal List<double> XDoubleList = null;
        internal List<int> XIntegerList = null;
        internal List<List<double>> YDoubleList = null;
        internal List<double> XIndexedList = null;
        internal List<bool> XBooleanList = null;
        internal List<DateTime> XDateTimeList = null;
        internal ValueType XDataValueType;

        object IDataManagerDependent.ItemsSource => ItemsSource;
        string IDataManagerDependent.XPath => XBindingPath;
        List<string> IDataManagerDependent.YPaths => YDataPaths;
        int IDataManagerDependent.PointsCount { get => PointsCount; set => PointsCount = value; }
        bool IDataManagerDependent.IsLinearData { get => IsLinearData; set => IsLinearData = value; }
        List<string> IDataManagerDependent.XStringValues { get => XStringList; set => XStringList = value; }
        List<double> IDataManagerDependent.XDoubleValues { get => XDoubleList; set => XDoubleList = value; }
        List<DateTime> IDataManagerDependent.XDateTimeValues { get => XDateTimeList; set => XDateTimeList = value; }
        List<List<double>> IDataManagerDependent.YDoubleValues { get => YDoubleList; set => YDoubleList = value; }
        List<double> IDataManagerDependent.XIndexedList { get => XIndexedList; set => XIndexedList = value; }
        List<object> IDataManagerDependent.ActualData { get => ActualData; set => ActualData = value; }
        bool IDataManagerDependent.ListenPropertyChange => this.ListenPropertyChange;
        bool IDataManagerDependent.IsGroupedYPath => this is BoxAndWhiskerSeries;
        ValueType IDataManagerDependent.XValueType { get => XDataValueType; set => XDataValueType = value; }

        void IDataManagerDependent.UpdateArea()
        {
            if (this is CircularSeries series)
                series.UpdateLegendItems();

            SegmentsCreated = false;
            ScheduleUpdateChart();
        }

        #region Properties
        
        internal int PointsCount { get; set; }

        internal List<object> ActualData { get; set; }

        internal bool IsLinearData { get; set; }  

        #endregion
        
        #region Methods

        internal List<double>? GetActualXValues()
        {
            GetSeriesXValues();

            if (YDoubleList == null || YDoubleList.Count == 0) return XValues as List<double>;

            IList<double>[] seriesYValues = new IList<double>[YDoubleList.Count];
            
            if (this is XYDataSeries series && YDataPaths.Contains(series.YBindingPath))
            {
                int index = YDataPaths.IndexOf(series.YBindingPath);
                series.YValues = YDoubleList[index];
                seriesYValues[0] = YDoubleList[index];
            }
            else if (this is RangeSeriesBase rangeSeries)
            {
                int indexHigh = YDataPaths.IndexOf(rangeSeries.High);
                if (indexHigh >= 0) 
                    rangeSeries.HighValues = YDoubleList[indexHigh];

                int indexLow = YDataPaths.IndexOf(rangeSeries.Low);
                if (indexLow >= 0)  
                    rangeSeries.LowValues = YDoubleList[indexLow];

                for (int i = 0; i < YDoubleList.Count; i++)
                {
                    seriesYValues[i] = YDoubleList[i];
                }
            }
            else if (this is CircularSeries circularSeries && YDataPaths.Contains(circularSeries.YBindingPath))
            {
                int yIndex = YDataPaths.IndexOf(circularSeries.YBindingPath);
                circularSeries.YValues = YDoubleList[yIndex];
            }

            this.SeriesYValues = seriesYValues;
            return XValues as List<double>;
        }

        internal void GetSeriesXValues()
        {
            switch (XDataValueType)
            {
                case ValueType.String:
                    {
                        if (this is CartesianSeries series && series.ChartArea is CartesianChartArea cartesianChartArea && series.ActualXAxis is CategoryAxis categoryAxis && !categoryAxis.ArrangeByIndex)
                        {
                            XValues = ActualXValues = GroupedXValuesIndexes;
                        }
                        else
                        {
                            XValues = ActualXValues = XIndexedList;
                        }
                        break;
                    }
                case ValueType.Double:
                case ValueType.DateTime:
                case ValueType.Int:
                case ValueType.Long:
                case ValueType.Float:
                    {
                        if (this is CartesianSeries series && series.ChartArea is CartesianChartArea cartesianChartArea && series.ActualXAxis is CategoryAxis categoryAxis)
                        {
                            if (!categoryAxis.ArrangeByIndex)
                            {
                                XValues = ActualXValues = GroupedXValuesIndexes;
                            }
                            else
                            {
                                XValues = ActualXValues = XIndexedList;
                            }
                        }
                        else
                        {
                            XValues = ActualXValues = XDoubleList;
                        }

                        break;
                    }
            }
        }

        #endregion
#endif
        #endregion

        #region Properties

        #region Internal Properties

        internal readonly ObservableCollection<ChartSegment> Segments;

        internal virtual bool IsMultipleYPathRequired
        {
            get
            {
                return false;
            }
        }

        internal virtual bool IsSideBySide => false;

        internal double XData { get; set; }

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

#if WinUI
        internal int PointsCount { get; set; }

        internal List<object> ActualData { get; set; }
        
         internal bool IsLinearData { get; set; } = true;
#endif
        internal IEnumerable? XValues { get; set; }

        internal string[][]? YComplexPaths { get; private set; }

        internal IEnumerable? ActualXValues { get; set; }

        internal IList<double>[]? SeriesYValues { get; private set; }

        internal IList<double>[]? ActualSeriesYValues { get; private set; }

        internal string[]? YPaths { get; private set; }

        internal string[]? XComplexPaths { get; set; }

        internal bool IsDataPointAddedDynamically { get; set; } = false;

        #endregion

        #endregion

        #region Methods

        #region Internal Virtual Methods

        internal virtual void OnDataSourceChanged(object oldValue, object newValue)
        {
        }

        internal virtual void OnBindingPathChanged()
        {
#if WinUI
            canAnimate = true;
            isTotalCalculated = false;
#else
            UpdateLegendItems();
            SegmentsCreated = false;
#endif
            this.ScheduleUpdateChart();
        }

        internal virtual void GenerateDataPoints()
        {
        }

        internal virtual void OnDataSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            IsDataPointAddedDynamically = false;
            e.ApplyCollectionChanges((index, obj) => AddDataPoint(index, obj, e), (index, obj) => RemoveData(index, e), ResetDataPoint);
#if WinUI
            if (IsSingleAccumulationSeries)
                ActualArea.PlotArea.ShouldPopulateLegendItems = true;
#else
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

            InvalidateGroupValues();
            SegmentsCreated = false;
            UpdateLegendItems();
#endif
            ScheduleUpdateChart();
        }

        internal virtual void RemoveData(int index, NotifyCollectionChangedEventArgs e)
        {
#if WinUI
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

            if (ToggledLegendIndex.Count > 0 && IsSingleAccumulationSeries)
            {
                var toggledIndexes = new List<int>();
                foreach (var legendIndex in ToggledLegendIndex)
                {
                    if (e.OldStartingIndex < legendIndex)
                        toggledIndexes.Add(legendIndex - 1);
                    else
                    {
                        if (e.OldStartingIndex != legendIndex)
                            toggledIndexes.Add(legendIndex);
                    }
                }

                ToggledLegendIndex = toggledIndexes;
            }

            if (!this.isNotificationSuspended)
            {
                this.UpdateSegments(e.OldStartingIndex, e.Action);

                UnHookPropertyChangedEvent(ListenPropertyChange, e.OldItems[0]);
            }
            else
                UpdateStartedIndex = UpdateStartedIndex != 0 ? UpdateStartedIndex - e.OldItems.Count : UpdateStartedIndex;
#endif
        }

        internal virtual void UpdateRange()
        {
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

            InvalidateGroupValues();
#if WinUI
            UnHookPropertyChanged(oldValue);
#endif
        }

        internal void InvalidateGroupValues()
        {
#if !WinUI
            if (this is CartesianSeries series && series.ChartArea is CartesianChartArea cartesianChartArea && series.ActualXAxis is CategoryAxis categoryAxis && !categoryAxis.ArrangeByIndex)
            {
                if (cartesianChartArea.VisibleSeries != null)
                {
                    categoryAxis.GroupData();

                    if (categoryAxis.RegisteredSeries.Count > 0)
                    {
                        foreach (CartesianSeries chartSeries in categoryAxis.RegisteredSeries)
                        {
                            chartSeries.SegmentsCreated = false;
                            chartSeries.ChartArea?.UpdateVisibleSeries();
                        }
                    }
                }
            }
#endif
        }

        //TODO:Need to remove the replace parameter from this method,
        //becuase new notify collectionchanged event first remove
        //the data and then insert the data. So no need replace parameter here after.

        /// <summary>
        /// Method implementation for generate points to given index.
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="obj">object</param>
        /// <param name="replace">replace the data or not</param>
        internal virtual void SetIndividualPoint(int index, object obj, bool replace)
        {
#if WinUI          
            if (SeriesYValues != null && YPaths != null && ItemsSource != null)
            {
                double yData;
                var xvalueType = GetArrayPropertyValue(obj, XComplexPaths);
                if (xvalueType != null)
                    XValueType = GetDataType(xvalueType);
                if (IsMultipleYPathRequired)
                {
                    if (XValueType == ChartValueType.String)
                    {
                        if (!(this.XValues is List<string>))
                            this.XValues = this.ActualXValues = new List<string>();
                        var xValue = (List<string>)XValues;
                        var xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        var xData = xVal as string;
                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == xData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = xData.Tostring();
                        }
                        else
                        {
                            xValue.Insert(index, xData.Tostring());
                        }

                        for (int i = 0; i < YPaths.Count(); i++)
                        {
                            var yVal = YComplexPaths == null ? obj : GetArrayPropertyValue(obj, YComplexPaths[i]);
                            yData = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            if (replace && SeriesYValues[i].Count > index)
                            {
                                if (SeriesYValues[i][index] == yData && isRepeatPoint)
                                    isRepeatPoint = true;
                                else
                                {
                                    SeriesYValues[i][index] = yData;
                                    isRepeatPoint = false;
                                }
                            }
                            else
                            {
                                SeriesYValues[i].Insert(index, yData);
                            }
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.Double ||
                       XValueType == ChartValueType.Logarithmic)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        var xValue = (List<double>)this.XValues;
                        var xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        XData = Convert.ToDouble(xVal != null ? xVal : double.NaN);

                        // Check the Data Collection is linear or not
                        if (IsLinearData && (index > 0 && XData < xValue[index - 1]) || (index == 0 && xValue.Count > 0 && XData > xValue[0]))
                        {
                            IsLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        for (int i = 0; i < YPaths.Count(); i++)
                        {
                            var yVal = YComplexPaths == null ? obj : GetArrayPropertyValue(obj, YComplexPaths[i]);
                            yData = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            if (replace && SeriesYValues[i].Count > index)
                            {
                                if (SeriesYValues[i][index] == yData && isRepeatPoint)
                                    isRepeatPoint = true;
                                else
                                {
                                    SeriesYValues[i][index] = yData;
                                    isRepeatPoint = false;
                                }
                            }
                            else
                            {
                                SeriesYValues[i].Insert(index, yData);
                            }
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.DateTime)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        var xValue = (List<double>)this.XValues;
                        var xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        XData = Convert.ToDateTime(xVal).ToOADate();

                        // Check the Data Collection is linear or not
                        if (IsLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            IsLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        for (int i = 0; i < YPaths.Count(); i++)
                        {
                            var yVal = YComplexPaths == null ? obj : GetArrayPropertyValue(obj, YComplexPaths[i]);
                            yData = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            if (replace && SeriesYValues[i].Count > index)
                            {
                                if (SeriesYValues[i][index] == yData && isRepeatPoint)
                                    isRepeatPoint = true;
                                else
                                {
                                    SeriesYValues[i][index] = yData;
                                    isRepeatPoint = false;
                                }
                            }
                            else
                            {
                                SeriesYValues[i].Insert(index, yData);
                            }
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.TimeSpan)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        var xValue = (List<double>)this.XValues;
                        var xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        XData = ((TimeSpan)xVal).TotalMilliseconds;

                        // Check the Data Collection is linear or not
                        if (IsLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            IsLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        for (int i = 0; i < YPaths.Count(); i++)
                        {
                            var yVal = YComplexPaths == null ? obj : GetArrayPropertyValue(obj, YComplexPaths[i]);
                            yData = Convert.ToDouble(yVal != null ? yVal : double.NaN);
                            if (replace && SeriesYValues[i].Count > index)
                            {
                                if (SeriesYValues[i][index] == yData && isRepeatPoint)
                                    isRepeatPoint = true;
                                else
                                {
                                    SeriesYValues[i][index] = yData;
                                    isRepeatPoint = false;
                                }
                            }
                            else
                            {
                                SeriesYValues[i].Insert(index, yData);
                            }
                        }

                        PointsCount = xValue.Count;

                        //TODO: Implement on timespan support. 
                    }
                }
                else
                {
                    var tempYPath = YComplexPaths != null ? YComplexPaths[0] : null;
                    var yValue = SeriesYValues[0];
                    if (XValueType == ChartValueType.String)
                    {
                        if (!(this.XValues is List<string>))
                            this.XValues = this.ActualXValues = new List<string>();
                        var xValue = (List<string>)this.XValues;
                        var xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        var yVal = GetArrayPropertyValue(obj, tempYPath);
                        yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;
                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == xVal.Tostring())
                                isRepeatPoint = true;
                            else
                                xValue[index] = xVal.Tostring();
                        }
                        else
                        {
                            xValue.Insert(index, xVal.Tostring());
                        }

                        if (replace && yValue.Count > index)
                        {
                            if (yValue[index] == yData && isRepeatPoint)
                                isRepeatPoint = true;
                            else
                            {
                                yValue[index] = yData;
                                isRepeatPoint = false;
                            }
                        }
                        else
                        {
                            yValue.Insert(index, yData);
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.Double ||
                        XValueType == ChartValueType.Logarithmic)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        var xValue = (List<double>)this.XValues;
                        var xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        var yVal = GetArrayPropertyValue(obj, tempYPath);
                        XData = xVal != null ? Convert.ToDouble(xVal) : double.NaN;
                        yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

                        // Check the Data Collection is linear or not
                        if (IsLinearData && (index > 0 && XData < xValue[index - 1]) || (index == 0 && xValue.Count > 0 && XData > xValue[0]))
                        {
                            IsLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        if (replace && yValue.Count > index)
                        {
                            if (yValue[index] == yData && isRepeatPoint)
                                isRepeatPoint = true;
                            else
                            {
                                yValue[index] = yData;
                                isRepeatPoint = false;
                            }
                        }
                        else
                        {
                            yValue.Insert(index, yData);
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.DateTime)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        var xValue = (List<double>)this.XValues;
                        var xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        var yVal = GetArrayPropertyValue(obj, tempYPath);
                        XData = Convert.ToDateTime(xVal).ToOADate();
                        yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

                        // Check the Data Collection is linear or not
                        if (IsLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            IsLinearData = false;
                        }

                        if (replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else
                        {
                            xValue.Insert(index, XData);
                        }

                        if (replace && yValue.Count > index)
                        {
                            if (yValue[index] == yData && isRepeatPoint)
                                isRepeatPoint = true;
                            else
                            {
                                yValue[index] = yData;
                                isRepeatPoint = false;
                            }
                        }
                        else
                        {
                            yValue.Insert(index, yData);
                        }

                        PointsCount = xValue.Count;
                    }
                    else if (XValueType == ChartValueType.TimeSpan)
                    {
                        if (!(this.XValues is List<double>))
                            this.XValues = this.ActualXValues = new List<double>();
                        var xValue = (List<double>)this.XValues;
                        var xVal = GetArrayPropertyValue(obj, XComplexPaths);
                        var yVal = GetArrayPropertyValue(obj, tempYPath);
                        XData = ((TimeSpan)xVal).TotalMilliseconds;
                        yData = yVal != null ? Convert.ToDouble(yVal) : double.NaN;

                        // Check the Data Collection is linear or not
                        if (IsLinearData && index > 0 && XData < xValue[index - 1])
                        {
                            IsLinearData = false;
                        }

                        if (xVal != null && replace && xValue.Count > index)
                        {
                            if (xValue[index] == XData)
                                isRepeatPoint = true;
                            else
                                xValue[index] = XData;
                        }
                        else if (xVal != null)
                        {
                            xValue.Insert(index, XData);
                        }

                        if (replace && yValue.Count > index)
                        {
                            if (yValue[index] == yData && isRepeatPoint)
                                isRepeatPoint = true;
                            else
                            {
                                yValue[index] = yData;
                                isRepeatPoint = false;
                            }
                        }
                        else
                        {
                            yValue.Insert(index, yData);
                        }

                        PointsCount = xValue.Count;

                        //TODO: Ensure on time span implementation.
                    }
                }

                if (ActualData != null)
                {
                    if (replace && ActualData.Count > index)
                        ActualData[index] = obj;
                    else if (ActualData.Count == index)
                        ActualData.Add(obj);
                    else
                        ActualData.Insert(index, obj);
                }
                isTotalCalculated = false;
            }

            // TODO:Need to enable this method for MAUI when provide ListenPropertyChange support 
            HookPropertyChangedEvent(ListenPropertyChange, obj);
#endif
        }

        #endregion

        #region Internal Methods

        internal object? GetActualXValue(int index)
        {
#if WinUI
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
#else
            GetActualXValues();

            if (XValues == null || index > PointsCount)
            {
                return null;
            }

            switch (XDataValueType)
            {
                case ValueType.String:
                    return XStringList[index];

                case ValueType.DateTime:
                    DateTime xDateTime = XDateTimeList[index];
                    return xDateTime.ToString("MM/dd/yyyy");

                default:
                    return XDoubleList[index];
            }
#endif           
        }

        internal virtual void GeneratePoints(string[] yPaths, params IList<double>[] yValueLists)
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
#if WinUI
                if (ItemsSource is DataTable)
                    GenerateDataTablePoints(yPaths, yLists);
                else
#endif
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
#if WinUI
            if (ActualXValues is IList && XValues is IList)
            {
                ((IList)XValues).Clear();
                ((IList)ActualXValues).Clear();
            }

            ActualData?.Clear();

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

            ClearAdornments();
            if (ToggledLegendIndex != null)
                ToggledLegendIndex.Clear();
#endif
        }

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
            // GetArrayPropertyValue method is used to get value from the path of current object
            object? parentObj = GetArrayPropertyValue(enumerator.Current, paths);

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

        internal static object? GetArrayPropertyValue(object obj, string[]? paths)
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
        private void AddDataPoint(int index, object data, NotifyCollectionChangedEventArgs e)
        {
#if WinUI
            if (!this.isNotificationSuspended)
            {
                this.SetIndividualPoint(e.NewStartingIndex, e.NewItems[0], false);
                if (ToggledLegendIndex.Count > 0 && IsSingleAccumulationSeries)
                {
                    var toggledIndexes = new List<int>();
                    foreach (var legendIndex in ToggledLegendIndex)
                    {
                        if (e.NewStartingIndex <= legendIndex)
                            toggledIndexes.Add(legendIndex + 1);
                        else
                            toggledIndexes.Add(legendIndex);
                    }

                    ToggledLegendIndex = toggledIndexes;
                }

                this.UpdateSegments(e.NewStartingIndex, e.Action);
            }
            else if (!isUpdateStarted)
            {
                UpdateStartedIndex = e.NewStartingIndex;
                isUpdateStarted = true;
            }
#endif
        }

        private void ResetDataPoint()
        {
#if WinUI
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
#endif
        }

        internal virtual void GeneratePropertyPoints(string[] yPaths, IList<double>[] yLists)
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

        internal virtual void GenerateComplexPropertyPoints(string[] yPaths, IList<double>[] yLists, GetReflectedProperty? getPropertyValue)
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
                                XData = xVal != null ? ((DateTime)xVal).ToOADate() : double.NaN;

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

#endregion

#endregion
    }
}