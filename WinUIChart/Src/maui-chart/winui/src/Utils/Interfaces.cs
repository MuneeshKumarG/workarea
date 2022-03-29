using System;
using System.Collections;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Inteface implementation for IRangeAxis
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRangeAxis<T> where T : IComparable
    {
        /// <summary>
        /// Gets or sets Minimum property
        /// </summary>
        T Minimum
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets Maximum property
        /// </summary>
        T Maximum
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Interface implementation for IRangeAxis
    /// </summary>
    public interface IRangeAxis
    {
        /// <summary>
        /// Gets Range property
        /// </summary>
        DoubleRange Range { get; }
    }

    /// <summary>
    /// Interface implementation for IChartAxis
    /// </summary>
    public interface IChartAxis
    {
        /// <summary>
        /// Gets or sets VisibleLabels property
        /// </summary>
        ChartAxisLabelCollection VisibleLabels
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Interface implementation for IChartSeries
    /// </summary>
    public interface IChartSeries
    {
        /// <summary>
        /// Gets or sets ItemsSource property
        /// </summary>
        IEnumerable ItemsSource
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Interface implementation for ISupportAxes
    /// </summary>
    public interface ISupportAxes
    {
        /// <summary>
        /// Gets XRange property
        /// </summary>
        DoubleRange XRange
        {
            get;
        }

        /// <summary>
        /// Gets YRange property
        /// </summary>
        DoubleRange YRange
        {
            get;
        }

        /// <summary>
        /// Gets ActualXAxis property.
        /// </summary>
        /// <value>It takes the <c>ChartAxis</c> value.</value>
        ChartAxis ActualXAxis { get; }
        /// <summary>
        /// Gets ActualYAxis property.
        /// </summary>
        /// <value>It takes the <c>ChartAxis</c> value.</value>
        ChartAxis ActualYAxis { get; }
    }

    /// <summary>
    /// Interface implementation for support axis for 2D chart 
    /// </summary>
    public interface ISupportAxes2D : ISupportAxes
    {
        ///<summary>
        /// Gets or sets YAxis property
        /// </summary>
        /// <value>It takes the <see cref="RangeAxisBase"/> value.</value> 
        RangeAxisBase YAxis
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets XAxis property
        /// </summary>
        /// <value>It takes the <see cref="ChartAxisBase2D"/> value.</value>
        ChartAxisBase2D XAxis
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Interface implementation for support axis for 3D chart
    /// </summary>
    public interface ISupportAxes3D : ISupportAxes
    {
        
    }

}
