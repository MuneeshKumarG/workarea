using System.Collections.ObjectModel;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// A collection class which holds ChartAxis.
    /// </summary>

    internal class ChartAxisCollection : ObservableCollection<ChartAxis>
    {
        /// <summary>
        /// return ChartAxis value from the given string
        /// </summary>
        /// <param name="name"></param>
        public ChartAxis this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    return null;

                foreach (ChartAxis axis in this)
                {
                    if (axis.Name == name)
                    {
                        return axis;
                    }
                }

                return null;
            }
        }

        /// <inheritdoc/>
        protected override void InsertItem(int index, ChartAxis item)
        {
            if (item != null && !Contains(item))
            {
                base.InsertItem(index, item);
            }

            if (Contains(item) && item.Area != null && item.Area.DependentSeriesAxes != null)
            {
                if (item.Area.DependentSeriesAxes.Contains(item))
                {
                    item.Area.DependentSeriesAxes.Remove(item);
                }
            }
        }

        internal void RemoveItem(ChartAxis axis, bool flag)
        {
            if (flag)
            {
                Remove(axis);
            }
        }
    }


    /// <summary>
    /// A collection class which holds CircularSeries.
    /// </summary>

    public class CircularSeriesCollection : ObservableCollection<CircularSeries>
    {
        /// <summary>
        /// return circular series from the given string.
        /// </summary>
        /// <param name="name">Used to specify the series name.</param>
        public CircularSeries this[string name]
        {
            get
            {
                foreach (CircularSeries series in this)
                {
                    if (series.Name == name)
                    {
                        return series;
                    }
                }

                return null;
            }
        }
    }

    /// <summary>
    /// A collection class which holds PolarSeries.
    /// </summary>

    public class PolarSeriesCollection : ObservableCollection<PolarSeries>
    {
        /// <summary>
        /// return polar series from the given string.
        /// </summary>
        /// <param name="name">Used to specify the series name.</param>
        public PolarSeries this[string name]
        {
            get
            {
                foreach (PolarSeries series in this)
                {
                    if (series.Name == name)
                    {
                        return series;
                    }
                }

                return null;
            }
        }
    }

    /// <summary>
    /// A collection class which holds CartesianSeries.
    /// </summary>
    public class CartesianSeriesCollection : ObservableCollection<CartesianSeries>
    {
        /// <summary>
        /// return cartesian series from the given string.
        /// </summary>
        /// <param name="name">Used to specify the series name.</param>
        public CartesianSeries this[string name]
        {
            get
            {
                foreach (CartesianSeries series in this)
                {
                    if (series.Name == name)
                    {
                        return series;
                    }
                }

                return null;
            }
        }
    }

    /// <summary>
    /// A collection class which holds ChartSeries.
    /// </summary>

    public class ChartSeriesCollection : ObservableCollection<ChartSeries>
    {
        /// <summary>
        /// return ChartSeries from the given string.
        /// </summary>
        /// <param name="name">Used to specify the series name.</param>
        public ChartSeries this[string name]
        {
            get
            {
                foreach (ChartSeries series in this)
                {
                    if (series.Name == name)
                    {
                        return series;
                    }
                }

                return null;
            }
        }
    }

    /// <summary>
    /// A collection class which holds ChartSeries 2D.
    /// </summary>
   
    internal class ChartVisibleSeriesCollection : ObservableCollection<ChartSeries>
    {
        /// <summary>
        /// return ChartSeries from the given string
        /// </summary>
        /// <param name="name"></param>
        public ChartSeries this[string name]
        {
            get
            {
                foreach (ChartSeries series in this)
                {
                    if (series.Name == name)
                    {
                        return series;
                    }
                }

                return null;
            }
        }
    }

    /// <summary>
    /// A collection class which holds ChartRowDefinitions
    /// </summary>
    internal class ChartRowDefinitions : ObservableCollection<ChartRowDefinition> 
    {
    }

    /// <summary>
    /// A collection class which holds ChartColumnDefinitions
    /// </summary>
    internal class ChartColumnDefinitions : ObservableCollection<ChartColumnDefinition>
    {
    }

    /// <summary>
    /// A collection class that holds ChartAxisRangeStyle.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812: Avoid uninstantiated internal classes")]
    internal class ChartAxisRangeStyleCollection : ObservableCollection<ChartAxisRangeStyle>
    {
    }
}
