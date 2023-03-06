using Microsoft.Maui.Controls;
using System.Collections;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// XYDataSeries represents a set of (x,y) data points in a chart.
    /// </summary>
    public abstract class XYDataSeries : CartesianSeries
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="YBindingPath"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty YBindingPathProperty =
                BindableProperty.Create(
                nameof(YBindingPath),
                typeof(string),
                typeof(XYDataSeries),
                null,
                BindingMode.Default,
                null,
                OnYBindingPathChanged);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeWidthProperty =
            BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(ChartSeries), 1d, BindingMode.Default, null, OnStrokeWidthPropertyChanged);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a path value on the source object to serve a y value to the series.
        /// </summary>
        /// <value>
        /// The string that represents the property name for the y plotting data, and its default value is null.
        /// </value>
        /// <example>
        /// # [Xaml](#tab/tabid-1)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:ColumnSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]></code>
        /// # [C#](#tab/tabid-2)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     ColumnSeries columnSeries = new ColumnSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///     };
        ///     
        ///     chart.Series.Add(columnSeries);
        ///
        /// ]]></code>
        /// ***
        /// </example>
        public string YBindingPath
        {
            get { return (string)GetValue(YBindingPathProperty); }
            set { SetValue(YBindingPathProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to specify the stroke width of a chart series.
        /// </summary>
        /// <value>It accepts double values and its default value is 1.</value>
        /// <example>
        /// # [Xaml](#tab/tabid-28)
        /// <code><![CDATA[
        ///     <chart:SfCartesianChart>
        ///
        ///     <!-- ... Eliminated for simplicity-->
        ///
        ///          <chart:LineSeries ItemsSource="{Binding Data}"
        ///                            XBindingPath="XValue"
        ///                            YBindingPath="YValue"
        ///                            StrokeWidth = "3" />
        ///
        ///     </chart:SfCartesianChart>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-29)
        /// <code><![CDATA[
        ///     SfCartesianChart chart = new SfCartesianChart();
        ///     ViewModel viewModel = new ViewModel();
        ///
        ///     // Eliminated for simplicity
        ///
        ///     LineSeries series = new LineSeries()
        ///     {
        ///           ItemsSource = viewModel.Data,
        ///           XBindingPath = "XValue",
        ///           YBindingPath = "YValue",
        ///           StrokeWidth = 3,
        ///     };
        ///     
        ///     chart.Series.Add(series);
        ///
        /// ]]>
        /// </code>
        /// ***
        /// </example>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        internal IList<double> YValues { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="XYDataSeries"/>.
        /// </summary>
        protected XYDataSeries(): base()
        {
            YValues = new List<double>();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when the data source was changed.
        /// </summary>
        /// <param name="oldValue">Old value.</param>
        /// <param name="newValue">New value.</param>
        internal override void OnDataSourceChanged(object oldValue, object newValue)
        {
            base.OnDataSourceChanged(oldValue, newValue);
        }

        internal override void OnBindingPathChanged()
        {
            base.OnBindingPathChanged();
        }

        internal override void SetStrokeWidth(ChartSegment segment)
        {
            segment.StrokeWidth = StrokeWidth;
        }

        internal virtual double GetDataLabelPositionAtIndex(int index)
        {
            return YValues == null ? 0f : YValues[index];
        }

        #region Spline base
        internal double[]? GetMonotonicSpline(List<double> xValues, IList<double> yValues, out double[] dx)
        {
            int count = PointsCount, index = -1;
            dx = new double[count - 1];
            double[] slope = new double[count - 1];
            double[] coefficent = new double[count];

            // Find the slope between the values.
            for (int i = 0; i < count - 1; i++)
            {
                if (!double.IsNaN(yValues[i + 1]) && !double.IsNaN(yValues[i])
                    && !double.IsNaN(xValues[i + 1]) && !double.IsNaN(xValues[i]))
                {
                    dx[i] = xValues[i + 1] - xValues[i];
                    slope[i] = (yValues[i + 1] - yValues[i]) / dx[i];
                    if (double.IsInfinity(slope[i]))
                    {
                        slope[i] = 0;
                    }
                }
            }

            // Add the first and last coefficent value as Slope[0] and Slope[n-1]
            if (slope.Length == 0)
            {
                return null;
            }

            coefficent[++index] = double.IsNaN(slope[0]) ? 0 : slope[0];

            for (int i = 0; i < dx.Length - 1; i++)
            {
                if (slope.Length > i + 1)
                {
                    double m = slope[i], next = slope[i + 1];
                    if (m * next <= 0)
                    {
                        coefficent[++index] = 0;
                    }
                    else
                    {
                        if (dx[i] == 0)
                        {
                            coefficent[++index] = 0;
                        }
                        else
                        {
                            double firstPoint = dx[i], nextPoint = dx[i + 1];
                            double interPoint = firstPoint + nextPoint;
                            coefficent[++index] = 3 * interPoint / (((interPoint + nextPoint) / m) + ((interPoint + firstPoint) / next));
                        }
                    }
                }
            }

            coefficent[++index] = double.IsNaN(slope[slope.Length - 1]) ? 0 : slope[slope.Length - 1];

            return coefficent;
        }

        internal double[] GetCardinalSpline(List<double> xValues, IList<double> yValues)
        {
            int count = PointsCount;
            double[] tangentsX = new double[count];

            for (int i = 0; i < count; i++)
            {
                if (i == 0 && xValues.Count > 2)
                {
                    tangentsX[i] = 0.5 * (xValues[i + 2] - xValues[i]);
                }
                else if (i == count - 1 && count - 3 >= 0)
                {
                    tangentsX[i] = 0.5 * (xValues[count - 1] - xValues[count - 3]);
                }
                else if (i - 1 >= 0 && xValues.Count > i + 1)
                {
                    tangentsX[i] = 0.5 * (xValues[i + 1] - xValues[i - 1]);
                }

                if (double.IsNaN(tangentsX[i]))
                {
                    tangentsX[i] = 0;
                }
            }

            return tangentsX;
        }

        /// <summary>
        /// Calculate the co-efficient values for the natural Spline
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="splineType">The spline type.</param>
        /// <returns>The spline.</returns>
        internal double[]? NaturalSpline(IList<double> values, SplineType splineType)
        {
            double a = 6;
            int count = PointsCount;
            var xValues = GetXValues();
            if (xValues == null)
            {
                return null;
            }

            double[] yCoeff = new double[count], u = new double[count];

            if (splineType == SplineType.Clamped && xValues.Count > 1)
            {
                u[0] = 0.5;
                double d0 = (xValues[1] - xValues[0]) / (values[1] - values[0]);
                double dn = (xValues[count - 1] - xValues[count - 2]) / (values[count - 1] - values[count - 2]);
                yCoeff[0] = (3 * (values[1] - values[0]) / (xValues[1] - xValues[0])) - (3 * d0);
                yCoeff[count - 1] = (3 * dn) - ((3 * (values[count - 1] - values[count - 2])) / (xValues[count - 1] - xValues[count - 2]));
                if (double.IsInfinity(yCoeff[0]) || double.IsNaN(yCoeff[0]))
                {
                    yCoeff[0] = 0;
                }

                if (double.IsInfinity(yCoeff[count - 1]) || double.IsNaN(yCoeff[count - 1]))
                {
                    yCoeff[count - 1] = 0;
                }
            }
            else
            {
                yCoeff[0] = u[0] = 0;
                yCoeff[count - 1] = 0;
            }

            for (int i = 1; i < count - 1; i++)
            {
                if (!double.IsNaN(values[i + 1]) && !double.IsNaN(values[i - 1]) && !double.IsNaN(values[i]))
                {
                    double d1 = xValues[i] - xValues[i - 1];
                    double d2 = xValues[i + 1] - xValues[i - 1];
                    double d3 = xValues[i + 1] - xValues[i];
                    double dy1 = values[i + 1] - values[i];
                    double dy2 = values[i] - values[i - 1];
                    if (xValues[i] == xValues[i - 1] || xValues[i] == xValues[i + 1])
                    {
                        yCoeff[i] = 0;
                        u[i] = 0;
                    }
                    else
                    {
                        var p = 1 / ((d1 * yCoeff[i - 1]) + (2 * d2));
                        yCoeff[i] = -p * d3;
                        u[i] = p * ((a * ((dy1 / d3) - (dy2 / d1))) - (d1 * u[i - 1]));
                    }
                }
            }

            for (int k = count - 2; k >= 0; k--)
            {
                yCoeff[k] = (yCoeff[k] * yCoeff[k + 1]) + u[k];
            }

            return yCoeff;
        }

        internal List<double>? CalculateControlPoints(IList<double> values, double yCoef, double nextyCoef, int i)
        {
            List<double> controlPoints = new List<double>();
            var xValues = GetXValues();

            if (xValues == null)
            {
                return null;
            }

            double yCoeff1 = yCoef;
            double yCoeff2 = nextyCoef;
            double x = xValues[i];
            double y = values[i];
            double nextX = xValues[i + 1];
            double nextY = values[i + 1];

            double one_third = 1 / 3.0;

            double deltaX2 = nextX - x;

            deltaX2 = deltaX2 * deltaX2;

            double dx1 = (2 * x) + nextX;
            double dx2 = x + (2 * nextX);

            double dy1 = (2 * y) + nextY;
            double dy2 = y + (2 * nextY);

            double y1 = one_third * (dy1 - (one_third * deltaX2 * (yCoeff1 + (0.5 * yCoeff2))));
            double y2 = one_third * (dy2 - (one_third * deltaX2 * ((0.5 * yCoeff1) + yCoeff2)));

            var startControlPointsX = dx1 * one_third;
            var startControlPointsY = y1;
            var endControlPointsX = dx2 * one_third;
            var endControlPointsY = y2;

            controlPoints.Add(startControlPointsX);
            controlPoints.Add(startControlPointsY);
            controlPoints.Add(endControlPointsX);
            controlPoints.Add(endControlPointsY);

            return controlPoints;
        }

        internal List<double> CalculateControlPoints(double pointX, double pointY, double pointX1, double pointY1, double coefficientY, double coefficientY1)
        {
            return new List<double>(4) { pointX + (coefficientY / 3), pointY + (coefficientY / 3), pointX1 - (coefficientY1 / 3), pointY1 - (coefficientY1 / 3) };
        }

        internal List<double> CalculateControlPoints(double pointX, double pointY, double pointX1, double pointY1, double coefficientY, double coefficientY1, double dx)
        {
            var value = dx / 3;
            return new List<double>(4) { pointX + value, pointY + (coefficientY * value), pointX1 - value, pointY1 - (coefficientY1 * value) };
        }

        #endregion

        internal static void OnYBindingPathChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as XYDataSeries;
            if (series != null)
            {
                // UpdateIsLoadingProperties();
                if (newValue != null && newValue is string value)
                {
                    series.YDataPaths.Add(value);
                }

                if (series is ChartSeries chartSeries && chartSeries.Chart != null)
                {
                    chartSeries.Chart.DataManager.OnBindingPathChanged(series, (string)oldValue, newValue, chartSeries.Chart.IsDataPopulated, false);
                }

                series.OnBindingPathChanged();
            }
        }
        
        private static void OnStrokeWidthPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var series = bindable as ChartSeries;
            if (series != null)
            {
                series.UpdateStrokeWidth();
                series.InvalidateSeries();
            }
        }
        #endregion
    }
}
