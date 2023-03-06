using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts 
{
    /// <summary>
    /// The CartesianDataLabelSettings class is used to customize the appearance of cartesian series data labels.
    /// </summary>
    /// <remarks>
    /// <para>To customize data labels, create an instance of <see cref="CartesianDataLabelSettings"/> class, and set it to the DataLabelSettings property of a cartesian series.</para>
    /// 
    /// <para><b>ShowDataLabels</b></para>
    /// <para>Data labels can be added to a chart series by enabling the <see cref="ChartSeries.ShowDataLabels"/> option.</para>
    /// 
    /// # [Xaml](#tab/tabid-1)
    /// <code><![CDATA[
    ///    <chart:SfCartesianChart>
    ///
    ///       <!-- ... Eliminated for simplicity-->
    ///       <chart:LineSeries ItemsSource ="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"
    ///                         ShowDataLabels="True"/>
    ///
    ///    </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-2)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     ViewModel viewModel = new ViewModel();
    ///     
    ///     // Eliminated for simplicity
    ///     LineSeries series = new PieSeries()
    ///     {
    ///        ItemsSource = viewModel.Data,
    ///        XBindingPath = "XValue",
    ///        YBindingPath = "YValue",
    ///        ShowDataLabels = true
    ///     };
    ///     
    ///     chart.Series.Add(series);
    ///
    /// ]]></code>
    /// ***
    /// 
    /// <para><b>Customization</b></para>
    /// <para>To change the appearance of data labels, it offers <see cref="ChartDataLabelSettings.LabelStyle"/> options.</para>
    ///
    /// # [Xaml](#tab/tabid-3)
    /// <code><![CDATA[
    ///    <chart:SfCartesianChart>
    ///
    ///     <!-- ... Eliminated for simplicity-->
    ///       <chart:LineSeries ItemsSource ="{Binding Data}" XBindingPath="XValue" YBindingPath="YValue"
    ///                        ShowDataLabels="True">
    ///          <chart:LineSeries.CartesianDataLabelSettings>
    ///               <chart:CartesianDataLabelSettings>
    ///                    <chart:CartesianDataLabelSettings.LabelStyle>
    ///                        <chart:ChartDataLabelStyle Background = "Red" FontSize="14" TextColor="Black" />
    ///                    </chart:CartesianDataLabelSettings.LabelStyle>
    ///                </chart:CartesianDataLabelSettings>
    ///          </chart:LineSeries.CartesianDataLabelSettings>
    ///       <chart:LineSeries />
    ///    </chart:SfCartesianChart>
    /// ]]></code>
    /// # [C#](#tab/tabid-4)
    /// <code><![CDATA[
    ///     SfCartesianChart chart = new SfCartesianChart();
    ///     ViewModel viewModel = new ViewModel();
    ///
    ///     // Eliminated for simplicity
    ///     LineSeries series = new LineSeries()
    ///     {
    ///        ItemsSource = viewModel.Data,
    ///        XBindingPath = "XValue",
    ///        YBindingPath = "YValue",
    ///        ShowDataLabels = true,
    ///     };
    ///
    ///     var labelStyle = new ChartDataLabelStyle()
    ///     { 
    ///         Background = new SolidColorBrush(Colors.Red),
    ///         TextColor = Colors.Black,
    ///         FontSize = 14,
    ///     };
    ///     series.DataLabelSettings = new CartesianDataLabelSettings()
    ///     {
    ///         LabelStyle = labelStyle,
    ///     };
    ///
    ///     chart.Series.Add(series);
    ///
    /// ]]></code>
    /// *** 
    /// </remarks>
    public class CartesianDataLabelSettings : ChartDataLabelSettings
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="BarAlignment"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty BarAlignmentProperty =
            BindableProperty.Create(nameof(BarAlignment), typeof(DataLabelAlignment), typeof(CartesianDataLabelSettings), DataLabelAlignment.Top, BindingMode.Default, null, null);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CartesianDataLabelSettings"/>.
        /// </summary>
        public CartesianDataLabelSettings()
        {
            IsNeedDataLabelMeasure.Add(nameof(BarAlignment));
            LabelStyle = new ChartDataLabelStyle() { FontSize = 12, Margin = new Thickness(5) };
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the value for a bar chart data label's alignment.
        /// </summary>
        /// <value>It accepts <see cref="DataLabelAlignment"/> values and has a default value of <see cref="DataLabelAlignment.Top"/>.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-5)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            ShowDataLabels="True">
        ///                <chart:ColumnSeries.DataLabelSettings>
        ///                    <chart:CartesianDataLabelSettings BarAlignment="Middle" />
        ///                </chart:ColumnSeries.DataLabelSettings>
        ///              </chart:ColumnSeries>
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-6)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///     var dataLabelSettings = new CartesianDataLabelSettings()
        ///     {
        ///         BarAlignment = DataLabelAlignment.Middle
        ///     };
        ///     ColumnSeries series = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           ShowDataLabels = true,
        ///           DataLabelSettings = dataLabelSettings,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public DataLabelAlignment BarAlignment
        {
            get { return (DataLabelAlignment)GetValue(BarAlignmentProperty); }
            set { SetValue(BarAlignmentProperty, value); }
        }

        #endregion

        #region Methods

        #region Internal Methods

        //Get the data label position.
        internal PointF CalculateDataLabelPoint(XYDataSeries xyDataSeries, ChartSegment dataLabel, PointF labelPoint, ChartDataLabelStyle labelStyle)
        {
            SizeF labelSize = labelStyle.MeasureLabel(dataLabel.DataLabel != null ? dataLabel.DataLabel : string.Empty);
            float padding = (float)labelStyle.LabelPadding;

            return xyDataSeries.GetDataLabelPosition(dataLabel, labelSize, labelPoint, padding);
        }

        //Get the label position for column series.
        internal PointF GetLabelPositionForRectangularSeries(XYDataSeries xyDataSeries, int index, SizeF labelSize, PointF labelPoint, float padding, DataLabelAlignment barAlignment)
        {
            if (xyDataSeries.ActualYAxis == null) return labelPoint;

            ChartDataLabelStyle labelstyle = LabelStyle;
            float x = labelPoint.X, y = labelPoint.Y;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;
            bool isPositive = xyDataSeries.YValues[index] >= 0;

            x = x + ((float)labelstyle.OffsetX);
            y = y + ((float)labelstyle.OffsetY);

            if (LabelPlacement == DataLabelPlacement.Inner || LabelPlacement == DataLabelPlacement.Auto)
            {
                if (xyDataSeries.ActualYAxis.IsInversed)
                {
                    y = isPositive ? y - (labelSize.Height / 2) - padding - borderWidth : y + (labelSize.Height / 2) + padding + borderWidth;
                }
                else
                {
                    y = isPositive ? y + (labelSize.Height / 2) + padding + borderWidth : y - (labelSize.Height / 2) - padding - borderWidth;
                }

                if (LabelPlacement == DataLabelPlacement.Auto)
                {
                    if ((y - (labelSize.Height / 2)) < 0)
                    {
                        if (xyDataSeries.ActualYAxis.IsInversed)
                        {
                            y = isPositive ? labelPoint.Y - (labelSize.Height / 2) - padding : labelPoint.Y + (labelSize.Height / 2) + padding;
                        }
                        else
                        {
                            y = isPositive ? labelPoint.Y + (labelSize.Height / 2) + padding : labelPoint.Y - (labelSize.Height / 2) - padding;
                        }
                    }
                    else if ((y + (labelSize.Height / 2)) > xyDataSeries.AreaBounds.Height)
                    {
                        if (xyDataSeries.ActualYAxis.IsInversed)
                        {
                            y = isPositive ? labelPoint.Y - (labelSize.Height / 2) - padding : labelPoint.Y + (labelSize.Height / 2) + padding;
                        }
                        else
                        {
                            y = isPositive ? labelPoint.Y + (labelSize.Height / 2) + padding : labelPoint.Y - (labelSize.Height / 2) - padding;
                        }
                    }

                    PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                    x = point.X;
                    y = point.Y;
                }
                else if (labelPoint.Y + (labelSize.Height / 2) >= xyDataSeries.AreaBounds.Height)
                {
                    y = labelPoint.Y - (labelSize.Height / 2) - padding - borderWidth;
                }
                else if (barAlignment == DataLabelAlignment.Bottom)
                {
                    y = isPositive ? labelPoint.Y - (labelSize.Height / 2) - padding - borderWidth : labelPoint.Y + (labelSize.Height / 2) + padding + borderWidth;
                }
            }
            else if (LabelPlacement == DataLabelPlacement.Outer)
            {
                if (xyDataSeries.ActualYAxis.IsInversed)
                {
                    y = isPositive ? y + (labelSize.Height / 2) + padding + borderWidth : y - (labelSize.Height / 2) - padding - borderWidth;
                }
                else
                {
                    y = isPositive ? y - (labelSize.Height / 2) - padding - borderWidth : y + (labelSize.Height / 2) + padding + borderWidth;
                }

                if (barAlignment == DataLabelAlignment.Bottom)
                {
                    y = isPositive ? labelPoint.Y + (labelSize.Height / 2) + padding + borderWidth : labelPoint.Y - (labelSize.Height / 2) - padding - borderWidth;
                }
            }

            return new PointF(x, y);
        }

        //Get the label position for transposed column series.
        internal PointF GetLabelPositionForTransposedRectangularSeries(XYDataSeries xyDataSeries, int index, SizeF labelSize, PointF labelPoint, float padding, DataLabelAlignment barAlignment)
        {
            if (xyDataSeries.ActualYAxis == null) return labelPoint;

            ChartDataLabelStyle labelstyle = LabelStyle;
            float x = labelPoint.X, y = labelPoint.Y;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;
            bool isPositive = xyDataSeries.YValues[index] >= 0;

            x = x + ((float)labelstyle.OffsetX);
            y = y + ((float)labelstyle.OffsetY);

            if (LabelPlacement == DataLabelPlacement.Inner || LabelPlacement == DataLabelPlacement.Auto)
            {
                if (xyDataSeries.ActualYAxis.IsInversed)
                {
                    x = isPositive ? x + (labelSize.Width / 2) + padding + borderWidth : x - (labelSize.Width / 2) - padding - borderWidth;
                }
                else
                {
                    x = isPositive ? x - (labelSize.Width / 2) - padding - borderWidth : x + (labelSize.Width / 2) + padding + borderWidth;
                }

                if (LabelPlacement == DataLabelPlacement.Auto)
                {
                    if ((x - (labelSize.Width / 2)) <= 0)
                    {
                        if (xyDataSeries.ActualYAxis.IsInversed)
                        {
                            x = isPositive ? labelPoint.X + (labelSize.Width / 2) + padding : labelPoint.X - (labelSize.Width / 2) - padding;
                        }
                        else
                        {
                            x = isPositive ? labelPoint.X - (labelSize.Width / 2) - padding : labelPoint.X + (labelSize.Width / 2) + padding;
                        }
                    }
                    else if ((x + (labelSize.Width / 2)) >= xyDataSeries.AreaBounds.Width)
                    {
                        if (xyDataSeries.ActualYAxis.IsInversed)
                        {
                            x = isPositive ? labelPoint.X + (labelSize.Width / 2) + padding : labelPoint.X - (labelSize.Width / 2) - padding;
                        }
                        else
                        {
                            x = isPositive ? labelPoint.X - (labelSize.Width / 2) - padding : labelPoint.X + (labelSize.Width / 2) + padding;
                        }
                    }

                    PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                    x = point.X;
                    y = point.Y;
                }
                else if (labelPoint.X - (labelSize.Width / 2) <= 0)
                {
                    x = labelPoint.X + (labelSize.Width / 2) + padding + borderWidth;
                }
                else if (barAlignment == DataLabelAlignment.Bottom)
                {
                    x = isPositive ? labelPoint.X + (labelSize.Width / 2) + padding + borderWidth : labelPoint.X - (labelSize.Width / 2) - padding - borderWidth;
                }
            }
            else if (LabelPlacement == DataLabelPlacement.Outer)
            {
                if (xyDataSeries.ActualYAxis.IsInversed)
                {
                    x = isPositive ? x - (labelSize.Width / 2) - padding - borderWidth : x + (labelSize.Width / 2) + padding + borderWidth;
                }
                else
                {
                    x = isPositive ? x + (labelSize.Width / 2) + padding + borderWidth : x - (labelSize.Width / 2) - padding - borderWidth;
                }

                if (labelPoint.X + (labelSize.Width / 2) >= xyDataSeries.AreaBounds.Width)
                {
                    x = labelPoint.X - (labelSize.Width / 2) - padding - borderWidth;
                }
                else if (barAlignment == DataLabelAlignment.Bottom)
                {
                    x = isPositive ? labelPoint.X - (labelSize.Width / 2) - padding - borderWidth : labelPoint.X + (labelSize.Width / 2) + padding + borderWidth;
                }
            }

            return new PointF(x, y);
        }

        //Get the label position for line, spline series.
        internal PointF GetLabelPositionForContinuousSeries(XYDataSeries xyDataSeries, int index, SizeF labelSize, PointF labelPoint, float padding)
        {
            bool isTop = IsTopWithLabelIndex(xyDataSeries, index);

            ChartDataLabelStyle labelstyle = LabelStyle;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;
            float x = labelPoint.X;
            float y = labelPoint.Y;

            x = x + ((float)labelstyle.OffsetX);
            y = y + ((float)labelstyle.OffsetY);
            var halfLabelSizeHeight = labelSize.Height / 2;

            if (LabelPlacement == DataLabelPlacement.Outer || LabelPlacement == DataLabelPlacement.Auto)
            {
                y = isTop ? y - halfLabelSizeHeight - padding - borderWidth : y + halfLabelSizeHeight + padding + borderWidth;

                if (LabelPlacement == DataLabelPlacement.Auto)
                {
                    PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                    x = point.X;
                    y = point.Y;
                }
            }
            else if (LabelPlacement == DataLabelPlacement.Inner)
            {
                y = isTop ? y + halfLabelSizeHeight + padding + borderWidth : y - halfLabelSizeHeight - padding - borderWidth;
            }

            return new PointF(x, y);
        }

        //Get the label position for scatter series.
        internal PointF GetLabelPositionForSeries(XYDataSeries xyDataSeries, SizeF labelSize, PointF labelPoint, float padding, SizeF scatterSize)
        {
            ChartDataLabelStyle labelstyle = LabelStyle;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;
            float radius = (float)scatterSize.Height / 2;
            float x = labelPoint.X;
            float y = labelPoint.Y;
            x = x + ((float)labelstyle.OffsetX);
            y = y + ((float)labelstyle.OffsetY);

            if (LabelPlacement == DataLabelPlacement.Outer)
            {
                y = y - radius - (labelSize.Height / 2) - padding - borderWidth;
            }
            else if (LabelPlacement == DataLabelPlacement.Inner)
            {
                y = y - radius + (labelSize.Height / 2) + padding + borderWidth;
            }
            else if (LabelPlacement == DataLabelPlacement.Auto)
            {
                y = y - radius - (labelSize.Height / 2) - padding - borderWidth;
                
                PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                x = point.X;
                y = point.Y;
            }

            return new PointF(x, y);
        }

        //Get the label position for area series.
        internal PointF GetLabelPositionForAreaSeries(XYDataSeries xyDataSeries, ChartSegment dataLabel, SizeF labelSize, PointF labelPoint, float padding)
        {
            if (xyDataSeries.ChartArea == null) return labelPoint;

            var index = dataLabel.Index;
            ChartDataLabelStyle labelstyle = LabelStyle;
            float x = labelPoint.X, y = labelPoint.Y;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;
            bool isPositive = xyDataSeries.YValues[index] >= 0;

            x = x + ((float)labelstyle.OffsetX);
            y = y + ((float)labelstyle.OffsetY);

            if (LabelPlacement == DataLabelPlacement.Outer)
            {
                //Todo: Need to remove this Chart.IsTransposed code in cartesian layout changes.
                if (xyDataSeries.ChartArea.IsTransposed)
                {
                    x = isPositive ? x + (labelSize.Width / 2) + padding + borderWidth : x - (labelSize.Width / 2) - padding - borderWidth;
                }
                else
                {
                    y = isPositive ? y - (labelSize.Height / 2) - padding - borderWidth : y + (labelSize.Height / 2) + padding + borderWidth;
                }

                if (LabelPlacement == DataLabelPlacement.Auto)
                {
                    PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                    x = point.X;
                    y = point.Y;
                }
            }
            else if (LabelPlacement == DataLabelPlacement.Inner)
            {
                if (xyDataSeries.ChartArea.IsTransposed)
                {
                    x = isPositive ? x - (labelSize.Width / 2) - padding - borderWidth : x + (labelSize.Width / 2) + padding + borderWidth;
                }
                else
                {
                    y = isPositive ? y + (labelSize.Height / 2) + padding + borderWidth : y - (labelSize.Height / 2) - padding - borderWidth;
                }

                if (LabelPlacement == DataLabelPlacement.Auto)
                {
                    PointF point = GetAutoLabelPosition(xyDataSeries, x, y, labelSize, padding, borderWidth);
                    x = point.X;
                    y = point.Y;
                }
            }

            return new PointF(x, y);
        }

        //Get the label position for range column series.
        internal PointF CalculateDataLabelPoint(RangeColumnSeries series, RangeColumnSegment dataLabel, PointF labelPoint, ChartDataLabelStyle labelStyle, string valueType)
        {
            if (series.ChartArea == null) return labelPoint;

            SizeF labelSize = labelStyle.MeasureLabel(dataLabel.DataLabels[0] != null ? dataLabel.DataLabels[0] : string.Empty);
            float padding = (float)labelStyle.LabelPadding;

            return GetLabelPositionForRangeColumnSeries(series, dataLabel.Index, labelSize, labelPoint, padding, valueType);
        }

        #endregion

        #region Private Methods

        private bool IsTopWithLabelIndex(XYDataSeries xyDataSeries, int index)
        {
            IList<double> yvalues = xyDataSeries.YValues;

            double yValue = yvalues[index];
            int count = yvalues.Count;
            double nextYValue = 0.0;
            double previousYValue = 0.0;

            if ((count - 1) > index)
            {
                nextYValue = yvalues[index + 1];
            }

            if (index > 0)
            {
                previousYValue = yvalues[index - 1];
            }

            if (count > 1)
            {
                if (index == 0)
                {
                    if (double.IsNaN(nextYValue))
                    {
                        return true;
                    }
                    else
                    {
                        return yValue > nextYValue;
                    }
                }

                if (count - 1 == index)
                {
                    if (double.IsNaN(previousYValue))
                    {
                        return true;
                    }
                    else
                    {
                        return yValue > previousYValue;
                    }
                }
                else
                {
                    if (double.IsNaN(previousYValue) && double.IsNaN(nextYValue))
                    {
                        return true;
                    }
                    else if (double.IsNaN(nextYValue))
                    {
                        return !(previousYValue > yValue);
                    }
                    else if (double.IsNaN(previousYValue))
                    {
                        return !(nextYValue > yValue);
                    }
                    else
                    {
                        double previousXValue = index - 1;
                        double nextXValue = index + 1;
                        double xValue = index;

                        double slope = (nextYValue - previousYValue) / (nextXValue - previousXValue);
                        double yIntercept = nextYValue - (slope * nextXValue);
                        double intersectY = (slope * xValue) + yIntercept;
                        return intersectY < yValue;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        private PointF GetLabelPositionForRangeColumnSeries(RangeColumnSeries rangeColumnSeries, int index, SizeF labelSize, PointF labelPoint, float padding, string valueType)
        {
            if (rangeColumnSeries == null) return labelPoint;

            ChartDataLabelStyle labelstyle = LabelStyle;
            float x = labelPoint.X, y = labelPoint.Y;
            var borderWidth = (float)labelstyle.StrokeWidth / 2;
            x = x + ((float)labelstyle.OffsetX);
            y = y + ((float)labelstyle.OffsetY);
            double highValue = rangeColumnSeries.IsMultipleYPathRequired ? rangeColumnSeries.HighValues[index] : rangeColumnSeries.HighValues.Count == 0 ? 0 : rangeColumnSeries.HighValues[index];
            double lowValue = rangeColumnSeries.IsMultipleYPathRequired ? rangeColumnSeries.LowValues[index] : rangeColumnSeries.LowValues.Count == 0 ? 0 : rangeColumnSeries.LowValues[index];

            switch (valueType)
            {
                case "HighType":
                    if (rangeColumnSeries.ChartArea != null && rangeColumnSeries.ChartArea.IsTransposed)
                    {
                        if (LabelPlacement == DataLabelPlacement.Auto || LabelPlacement == DataLabelPlacement.Inner)
                        {
                            if (rangeColumnSeries.ActualYAxis != null && rangeColumnSeries.ActualYAxis.IsInversed)
                            {
                                x = highValue > lowValue ? x + (labelSize.Width / 2) + padding + borderWidth : x - (labelSize.Width / 2) - padding - borderWidth;
                            }
                            else
                            {
                                x = highValue > lowValue ? x - (labelSize.Width / 2) - padding - borderWidth : x + (labelSize.Width / 2) + padding + borderWidth;
                            }
                           
                            PointF point = GetAutoLabelPosition(rangeColumnSeries, x, y, labelSize, padding, borderWidth);
                            x = point.X;
                            y = point.Y;
                        }
                        else if (LabelPlacement == DataLabelPlacement.Outer)
                        {
                            if (rangeColumnSeries.ActualYAxis != null && rangeColumnSeries.ActualYAxis.IsInversed)
                            {
                                x = highValue > lowValue ? x - (labelSize.Width / 2) - padding - borderWidth : x + (labelSize.Width / 2) + padding + borderWidth;
                            }
                            else
                            {
                                x = highValue > lowValue ? x + (labelSize.Width / 2) + padding + borderWidth : x - (labelSize.Width / 2) - padding - borderWidth;
                            }
                        }
                    }
                    else
                    {
                        if (LabelPlacement == DataLabelPlacement.Auto || LabelPlacement == DataLabelPlacement.Inner)
                        {
                            if (rangeColumnSeries.ActualYAxis!=null && rangeColumnSeries.ActualYAxis.IsInversed)
                            {
                                y = highValue > lowValue ? y - (labelSize.Height / 2) - padding - borderWidth : y + (labelSize.Height / 2) + padding + borderWidth;
                            }
                            else
                            {
                                y = highValue > lowValue ? y + (labelSize.Height / 2) + padding + borderWidth : y - (labelSize.Height / 2) - padding - borderWidth;
                            }
                          
                            PointF point = GetAutoLabelPosition(rangeColumnSeries, x, y, labelSize, padding, borderWidth);
                            x = point.X;
                            y = point.Y;
                        }
                        else if (LabelPlacement == DataLabelPlacement.Outer)
                        {
                            if (rangeColumnSeries.ActualYAxis != null && rangeColumnSeries.ActualYAxis.IsInversed)
                            {
                                y = highValue > lowValue ? y + (labelSize.Height / 2) + padding + borderWidth : y - (labelSize.Height / 2) - padding - borderWidth;
                            }
                            else
                            {
                                y = highValue > lowValue ? y - (labelSize.Height / 2) - padding - borderWidth : y + (labelSize.Height / 2) + padding + borderWidth;
                            }
                        }
                    }

                    break;
                case "LowType":
                    if (rangeColumnSeries.ChartArea != null && rangeColumnSeries.ChartArea.IsTransposed)
                    {
                        if (LabelPlacement == DataLabelPlacement.Auto || LabelPlacement == DataLabelPlacement.Inner)
                        {
                            if (rangeColumnSeries.ActualYAxis != null && rangeColumnSeries.ActualYAxis.IsInversed)
                            {
                                x = highValue > lowValue ? x - (labelSize.Width / 2) - padding + borderWidth : x + (labelSize.Width / 2) + padding + borderWidth;
                            }
                            else
                            {
                                x = highValue > lowValue ? x + (labelSize.Width / 2) + padding + borderWidth : x - (labelSize.Width / 2) - padding - borderWidth;
                            }
                            
                            PointF point = GetAutoLabelPosition(rangeColumnSeries, x, y, labelSize, padding, borderWidth);
                            x = point.X;
                            y = point.Y;
                        }
                        else if (LabelPlacement == DataLabelPlacement.Outer)
                        {
                            if (rangeColumnSeries.ActualYAxis != null && rangeColumnSeries.ActualYAxis.IsInversed)
                            {
                                x = highValue > lowValue ? x + (labelSize.Width / 2) + padding + borderWidth : x - (labelSize.Width / 2) - padding - borderWidth;
                            }
                            else
                            {
                                x = highValue > lowValue ? x - (labelSize.Width / 2) - padding - borderWidth : x + (labelSize.Width / 2) + padding + borderWidth;
                            }
                        }
                    }
                    else
                    {
                        if (LabelPlacement == DataLabelPlacement.Auto || LabelPlacement == DataLabelPlacement.Inner)
                        {
                            if (rangeColumnSeries.ActualYAxis != null && rangeColumnSeries.ActualYAxis.IsInversed)
                            {
                                y = highValue > lowValue ? y + (labelSize.Height / 2) + padding - borderWidth : y - (labelSize.Height / 2) - padding - borderWidth;
                            }
                            else
                            {
                                y = highValue > lowValue ? y - (labelSize.Height / 2) - padding - borderWidth : y + (labelSize.Height / 2) + padding + borderWidth;
                            }
                           
                            PointF point = GetAutoLabelPosition(rangeColumnSeries, x, y, labelSize, padding, borderWidth);
                            x = point.X;
                            y = point.Y;
                        }
                        else if (LabelPlacement == DataLabelPlacement.Outer)
                        {
                            if (rangeColumnSeries.ActualYAxis != null && rangeColumnSeries.ActualYAxis.IsInversed)
                            {
                                y = highValue > lowValue ? y - (labelSize.Height / 2) - padding - borderWidth : y + (labelSize.Height / 2) + padding + borderWidth;
                            }
                            else
                            {
                                y = highValue > lowValue ? y + (labelSize.Height / 2) + padding + borderWidth : y - (labelSize.Height / 2) - padding - borderWidth;
                            }
                        }
                    }

                    break;
            }

            return new PointF(x, y);
        }

        #endregion

        #endregion
    }
}
