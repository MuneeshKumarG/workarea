using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using AdornmentSeries = Syncfusion.UI.Xaml.Charts.DataMarkerSeries;
using ChartAdornment = Syncfusion.UI.Xaml.Charts.ChartDataLabel;
using AdornmentsPosition = Syncfusion.UI.Xaml.Charts.BarLabelAlignment;
using StackingSeriesBase = Syncfusion.UI.Xaml.Charts.StackedSeriesBase;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a ChartSeries that displays data in a customizable chart.
    /// </summary>
    public abstract class DataMarkerSeries : ChartSeries
    {
        #region Dependency Property Registration

        /// <summary>
        /// Identifies the <see cref="ShowDataLabels"/> dependency property.
        /// </summary>        
        /// <value>
        /// The identifier for <c>ShowDataLabels</c> dependency property and its default value is false.
        /// </value>   
        public static readonly DependencyProperty ShowDataLabelsProperty =
            DependencyProperty.Register(nameof(ShowDataLabels), typeof(bool), typeof(AdornmentSeries),
                new PropertyMetadata(false, new PropertyChangedCallback(OnShowDataLabelsChanged)));

        #endregion

        #region Properties

        #region Public Properties


        /// <summary>
        /// Gets or sets a value that indicates to enable the data labels for the series.
        /// </summary>
        /// <value>It takes the bool value and its default value is false.</value>
        public bool ShowDataLabels
        {
            get
            {
                return (bool)GetValue(ShowDataLabelsProperty);
            }

            set
            {
                SetValue(ShowDataLabelsProperty, value);
            }
        }

        /// <summary>
        /// <para>Gets or sets data labels for the series.</para> <para>This allows us to customize the appearance of a data point 
        /// by displaying labels, shapes and connector lines.</para>
        /// </summary>
        /// <value>
        /// The <see cref="ChartDataLabelSettings" /> value.
        /// </value>
        internal virtual ChartDataLabelSettings AdornmentsInfo
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Occurs when the datamarker labels is created. This event is used for internal purpose only.
        /// </summary>
        internal event EventHandler<DataMarkerLabelCreatedEventArgs> DataMarkerLabelCreated;

        internal bool IsAdornmentLabelCreatedEventHooked
        {
            get
            {
                return DataMarkerLabelCreated != null;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Override Methods

        /// <summary>
        /// An abstract method which will be called over to create segments.
        /// </summary>
        public override void CreateSegments()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Internal Override Methods

        internal override void UpdateOnSeriesBoundChanged(Size size)
        {
            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                if (IsAdornmentLabelCreatedEventHooked)
                    RaiseAdornmentLabelCreatedEvent();
                AdornmentsInfo.UpdateElements();
            }

            base.UpdateOnSeriesBoundChanged(size);

            if (AdornmentPresenter != null && AdornmentsInfo != null)
            {
                AdornmentPresenter.Update(size);
                AdornmentPresenter.Arrange(size);
            }
        }

        private void RaiseAdornmentLabelCreatedEvent()
        {
            for (var i = 0; i < this.Adornments.Count; i++)
            {
                ChartAdornment chartAdornment = Adornments[i];
                chartAdornment.Label = chartAdornment.GetTextContent().ToString();
                ChartDataLabelSettings adornmentLabel = adornmentInfo.CreateChartDataLabel();
                adornmentLabel.Rotation = AdornmentsInfo.Rotation;
                adornmentLabel.Background = AdornmentsInfo.Background;
                adornmentLabel.BorderBrush = AdornmentsInfo.BorderBrush;
                adornmentLabel.BorderThickness = AdornmentsInfo.BorderThickness;
                adornmentLabel.Data = chartAdornment.Item;
                adornmentLabel.FontFamily = AdornmentsInfo.FontFamily;
                adornmentLabel.FontSize = AdornmentsInfo.FontSize;
                adornmentLabel.FontStyle = AdornmentsInfo.FontStyle;
                adornmentLabel.Foreground = AdornmentsInfo.Foreground;
                adornmentLabel.Index = i;
                adornmentLabel.Label = chartAdornment.Label;
                adornmentLabel.LabelBackgroundBrush = AdornmentsInfo.Background;
                adornmentLabel.Format = AdornmentsInfo.Format;
                adornmentLabel.LabelPadding = AdornmentsInfo.LabelPadding;
                adornmentLabel.SetDataLabelPosition(AdornmentsInfo.GetDataLabelPosition());
                adornmentLabel.Margin = AdornmentsInfo.Margin;
                adornmentLabel.OffsetX = AdornmentsInfo.OffsetX;
                adornmentLabel.OffsetY = AdornmentsInfo.OffsetY;
                if (chartAdornment.Series != null && chartAdornment.Series.SeriesYValues != null)
                {
                    adornmentLabel.GrandTotal = chartAdornment.Series.SeriesYValues.Length > 1 ? chartAdornment.GrandTotal : chartAdornment.CalculateSumOfValues(chartAdornment.Series.SeriesYValues[0]);
                }

                adornmentLabel.MarkerType = AdornmentsInfo.MarkerType;
                adornmentLabel.MarkerHeight = AdornmentsInfo.MarkerHeight;
                adornmentLabel.MarkerInterior = AdornmentsInfo.MarkerInterior;
                adornmentLabel.MarkerStroke = AdornmentsInfo.MarkerStroke;
                adornmentLabel.MarkerWidth = AdornmentsInfo.MarkerWidth;
                adornmentLabel.XPosition = chartAdornment.XPos;
                adornmentLabel.YPosition = chartAdornment.YPos;

                DataMarkerLabelCreatedEventArgs eventArgs = new DataMarkerLabelCreatedEventArgs(adornmentLabel);
                DataMarkerLabelCreated.Invoke(this, eventArgs);

                adornmentLabel.Background = adornmentLabel.LabelBackgroundBrush != AdornmentsInfo.Background ? adornmentLabel.LabelBackgroundBrush : null;
                adornmentLabel.Format = adornmentLabel.Format != AdornmentsInfo.Format ? adornmentLabel.Format : null;
                adornmentLabel.Label = !chartAdornment.Label.Equals(adornmentLabel.Label) ? adornmentLabel.Label : null;
                chartAdornment.CustomAdornmentLabel = adornmentLabel;
            }
        }

        internal override void CalculateSegments()
        {
            base.CalculateSegments();

            // VisibleAdornments need to be cleared when segments are newly build while Zooming 
            if (VisibleAdornments.Count > 0)
            {
                VisibleAdornments.Clear();
            }
            if (DataCount == 0)
            {
                if (AdornmentsInfo != null)
                {
                    AdornmentsPosition markerPosition = this.adornmentInfo.GetAdornmentPosition();
                    if (markerPosition == AdornmentsPosition.Middle)
                        ClearUnUsedAdornments(this.DataCount * 4);
                    else
                        ClearUnUsedAdornments(this.DataCount * 2);
                }
            }
        }

        #endregion

        #region Protected Internal Override Methods

        /// <summary>
        /// Method implementation for GeneratePoints for series.
        /// </summary>
        protected internal override void GeneratePoints()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Protected Virtual Methods

        /// <summary>
        /// Method implementation for create DataMarkers.
        /// </summary>
        /// <param name="series">series</param>
        /// <param name="xVal">xvalue</param>
        /// <param name="yVal">yvalue</param>
        /// <param name="xPos">xposition</param>
        /// <param name="yPos">yposition</param>
        /// <returns>ChartAdornment</returns>
        protected virtual ChartAdornment CreateDataMarker(AdornmentSeries series, double xVal, double yVal, double xPos, double yPos)
        {
            ChartAdornment adornment = new ChartAdornment(xVal, yVal, xPos, yPos, series);
            adornment.XData = xVal;
            adornment.YData = yVal;
            adornment.XPos = xPos;
            adornment.YPos = yPos;
            adornment.Series = series;
            return adornment;
        }

        internal virtual ChartAdornment CreateAdornment(AdornmentSeries series, double xVal, double yVal, double xPos, double yPos)
        {
            return CreateDataMarker(series, xVal, yVal, xPos, yPos);
        }

        /// <summary>
        /// Method implementation for add ColumnAdornments in Chart.
        /// </summary>
        /// <param name="values">values</param>
        internal virtual void AddColumnAdornments(params double[] values)
        {
            ////values[0] -->   xData
            ////values[1] -->   yData
            ////values[2] -->   xPos
            ////values[3] -->   yPos
            ////values[4] -->   data point index
            ////values[5] -->   Median value.

            double adornposX = values[2] + values[5], adornposY = values[3];
            int pointIndex = (int)values[4];
            if ((EmptyPointIndexes != null && EmptyPointIndexes.Any() && EmptyPointIndexes[0].Contains(pointIndex) &&
                 (EmptyPointStyle == Charts.EmptyPointStyle.Symbol ||
                  EmptyPointStyle == Charts.EmptyPointStyle.SymbolAndInterior)))
                if (this is StackingSeriesBase)
                    adornposY = (EmptyPointValue == EmptyPointValue.Average) ? values[3] : values[1];
                else
                    adornposY = values[1]; // WPF-13874-EmptyPoint segment adornmentinfo positioning wrongly when EmptyPointValues is Average
            if (pointIndex < Adornments.Count)
            {
                Adornments[pointIndex].SetData(values[0], values[1], adornposX, adornposY);
            }
            else
            {
                Adornments.Add(CreateAdornment(this, values[0], values[1], adornposX, adornposY));
            }

            if (!(this is HistogramSeries))
            {
                if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed
                    && this.GroupedActualData.Count > 0)
                    Adornments[pointIndex].Item = this.GroupedActualData[pointIndex];
                else
                    Adornments[pointIndex].Item = ActualData[pointIndex];
            }
        }

        /// <summary>
        /// Method implementation for add Adornments at XY.
        /// </summary>
        /// <param name="x">xvalue</param>
        /// <param name="y">yvalue</param>
        /// <param name="pointindex">index</param>
        internal virtual void AddAdornmentAtXY(double x, double y, int pointindex)
        {
            double adornposX = x, adornposY = y;

            if (pointindex < Adornments.Count)
            {
                Adornments[pointindex].SetData(x, y, adornposX, adornposY);
            }
            else
            {
                Adornments.Add(CreateAdornment(this, x, y, adornposX, adornposY));
            }

            if (pointindex < ActualData.Count)
                Adornments[pointindex].Item = ActualData[pointindex];
        }

        /// <summary>
        /// Method implementation for add AreaAdornments in Chart.
        /// </summary>
        /// <param name="values">values</param>
        internal virtual void AddAreaAdornments(params IList<double>[] values)
        {
            IList<double> yValues = values[0];
            List<double> xValues = new List<double>();
            if (ActualXAxis is CategoryAxis && !(ActualXAxis as CategoryAxis).IsIndexed)
                xValues = GroupedXValuesIndexes;
            else
                xValues = GetXValues();

            if (values.Length == 1)
            {
                int i;
                for (i = 0; i < DataCount; i++)
                {
                    if (i < xValues.Count && i < yValues.Count)
                    {
                        double adornX = xValues[i];
                        double adornY = yValues[i];

                        if (i < Adornments.Count)
                        {
                            Adornments[i].SetData(xValues[i], yValues[i], adornX, adornY);
                        }
                        else
                        {
                            Adornments.Add(CreateAdornment(this, xValues[i], yValues[i], adornX, adornY));
                        }

                        Adornments[i].Item = ActualData[i];
                    }
                }
            }
        }

        #endregion

        #region Protected Override Methods

        /// <summary>
        /// Called this method when Adornments render on Series.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (!(this is ErrorBarSeries))
            {
                AdornmentPresenter.Series = this;
                if (Area != null && AdornmentsInfo != null)
                {
                    ////Panel panel = Area.GetMarkerPresenter();
                    Panel panel = AdornmentPresenter;
                    if (panel != null)
                    {
                        AdornmentsInfo.PanelChanged(panel);
                    }
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnDataSourceChanged(System.Collections.IEnumerable oldValue, System.Collections.IEnumerable newValue)
        {
            if (AdornmentsInfo != null)
            {
                VisibleAdornments.Clear();
                Adornments.Clear();
                AdornmentsInfo.UpdateElements();
            }

            base.OnDataSourceChanged(oldValue, newValue);

            var area = this.Area;
            if(area != null)
            {
                area.IsUpdateLegend = area.HasDataPointBasedLegend();
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Method implementation for clear unused adornments.
        /// </summary>
        /// <param name="startIndex"></param>
        protected void ClearUnUsedAdornments(int startIndex)
        {
            if (Adornments.Count > startIndex)
            {
                int count = Adornments.Count;

                for (int i = startIndex; i < count; i++)
                {
                    Adornments.RemoveAt(startIndex);
                }
            }
        }

        #endregion

        #region Private Static Methods

        internal static void OnAdornmentsInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var series = d as AdornmentSeries;

            if (e.OldValue != null)
            {
                var adornmentInfo = e.OldValue as ChartDataLabelSettings;
                if (series != null)
                {
                    series.Adornments.Clear();
                    series.VisibleAdornments.Clear();
                }

                if (adornmentInfo != null)
                {
                    adornmentInfo.ClearChildren();
                    adornmentInfo.Series = null;
                }
            }

            if (e.NewValue != null)
            {
                if (series != null)
                {
                    series.adornmentInfo = e.NewValue as ChartDataLabelSettings;
                    series.AdornmentsInfo.Series = series;
                    if (series.Area != null && series.AdornmentsInfo != null)
                    {
                        ////Panel panel = series.Area.GetMarkerPresenter();
                        Panel panel = series.AdornmentPresenter;
                        if (panel != null)
                        {
                            series.AdornmentsInfo.PanelChanged(panel);
                            series.Area.ScheduleUpdate();
                        }
                    }
                }
            }
        }

        #endregion

        private static void OnShowDataLabelsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AdornmentSeries series = d as AdornmentSeries;

            if (series != null)
            {
                series.SetDataLabelsVisibility(series.ShowDataLabels);

                if ((bool)e.NewValue && series.ActualArea != null && series.Adornments != null && series.Adornments.Count == 0)
                {
                    series.Invalidate();
                    Panel panel = series.AdornmentPresenter;

                    if (panel != null)
                    {
                        series.AdornmentsInfo?.PanelChanged(panel);
                    }

                    series.AdornmentsInfo?.OnAdornmentPropertyChanged();
                }
            }
        }


        #endregion
    }

    /// <summary>
    /// This class serves as an event data for the <see cref="DataMarkerSeries.DataMarkerLabelCreated"/> event. The event data holds the information when the adornment label is created. 
    /// </summary>
    /// <remarks>
    /// This class is used for internal purpose only.
    /// </remarks>
    internal class DataMarkerLabelCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the chart adornment label.
        /// </summary>
        public ChartDataLabelSettings DataMarkerLabel { get; internal set; }

        public DataMarkerLabelCreatedEventArgs(ChartDataLabelSettings adornmentLabel)
        {
            DataMarkerLabel = adornmentLabel;
        }
    }
}