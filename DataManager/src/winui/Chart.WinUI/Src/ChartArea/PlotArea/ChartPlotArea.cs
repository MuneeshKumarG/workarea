using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;

namespace Syncfusion.UI.Xaml.Charts
{
    internal class ChartPlotArea : Grid, IPlotArea
    {
        #region Fields

        Binding binding;
        bool shouldPopulateLegendItems = true;
        private ILegend legend;
        private ObservableCollection<ILegendItem> legendItems;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartPlotArea"/> class.
        /// </summary>
        public ChartPlotArea()
        {
            LegendItems = new ObservableCollection<ILegendItem>();
        }

        #endregion

        #region Properties

        public ILegend Legend
        {
            get { return legend; }
            set 
            {
                legend = value;
                UpdateLegend();
            }
        }

        public bool ShouldPopulateLegendItems
        {
            get => shouldPopulateLegendItems; 
            set => shouldPopulateLegendItems = (bool)value; 
        }

        public ObservableCollection<ILegendItem> LegendItems
        {
            get { return legendItems; }
            set { legendItems = value; }
        }


        #endregion

        #region Methods

        private void UpdateLegend()
        {
            if (Legend is ChartLegend chartLegend)
            {
                shouldPopulateLegendItems = true;
                SetBinding(chartLegend, ChartLegend.ItemsSourceProperty, this, new PropertyPath(nameof(LegendItems)));
                PopulateLegendItems();
            }
        }

        internal virtual void UpdateLegendItemsSource()
        {

        }

        internal void BindLegendItemProperties(LegendItem legendItem, ChartSeries series, ChartSegment segment = null)
        {
            SetBinding(legendItem, LegendItem.IconTemplateProperty, series, new PropertyPath(nameof(series.LegendIconTemplate)));
            SetBinding(legendItem, LegendItem.StrokeProperty, series, new PropertyPath("Stroke"));
            SetBinding(legendItem, LegendItem.StrokeThicknessProperty, series, new PropertyPath("StrokeWidth"));
            SetBinding(legendItem, LegendItem.IconWidthProperty, Legend, new PropertyPath(nameof(Legend.IconWidth)));
            SetBinding(legendItem, LegendItem.IconHeightProperty, Legend, new PropertyPath(nameof(Legend.IconHeight)));
            SetBinding(legendItem, LegendItem.IconVisibilityProperty, Legend, new PropertyPath(nameof(Legend.IconVisibility)));
            SetBinding(legendItem, LegendItem.CheckBoxVisibilityProperty, Legend, new PropertyPath(nameof(Legend.CheckBoxVisibility)));
            SetBinding(legendItem, LegendItem.ItemMarginProperty, Legend, new PropertyPath(nameof(Legend.ItemMargin)));

            if (segment != null)
            {
                Brush solidColor = segment.Fill;
                legendItem.IconBrush = solidColor != null ? solidColor : new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                Binding binding = new Binding();
                binding.Source = series;
                binding.Path = new PropertyPath("Fill");
                binding.Converter = new InteriorConverter(series);
                binding.ConverterParameter = legendItem.Index;
                BindingOperations.SetBinding(legendItem, LegendItem.IconBrushProperty, binding);
            }
        }

        internal void SetBinding(DependencyObject targetClass, DependencyProperty targetProperty, object source, PropertyPath path)
        {
            binding = new Binding();
            binding.Source = source;
            binding.Path = path;
            binding.Mode = BindingMode.TwoWay;
            BindingOperations.SetBinding(targetClass, targetProperty, binding);
        }

        public void PopulateLegendItems()
        {
            if (shouldPopulateLegendItems)
            {
                UpdateLegendItemsSource();
            }
        }

        internal virtual void Dispose()
        {
            if (LegendItems != null)
            {
                foreach (ChartLegendItem legendItem in LegendItems)
                {
                    legendItem.Dispose();
                }
            }

            LegendItems?.Clear();
            LegendItems = null;
        }

        #endregion
    }
}
