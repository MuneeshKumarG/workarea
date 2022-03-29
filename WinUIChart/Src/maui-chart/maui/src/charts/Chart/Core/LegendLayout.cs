using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;
using System;
using System.Collections.Specialized;

namespace Syncfusion.Maui.Charts
{
    internal class LegendLayout : AbsoluteLayout
    {
        #region Fields
        private ILegend? legend;
        private IPlotArea plotArea;
        private SfLegend? legendItemsView;
        internal readonly AreaBase AreaBase;
        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="area"></param>
        public LegendLayout(AreaBase? area)
        {
            if (area == null)
                throw new ArgumentNullException("Chart area cannot be null");

            this.AreaBase = area;
            if (this.AreaBase is not IView chartAreaView)
                throw new ArgumentException("Chart area should be a view");

            plotArea = AreaBase.PlotArea;

            //Todo: Need to check collection changes needed.
            var legendItems = plotArea.LegendItems as INotifyCollectionChanged;
            if (legendItems != null)
                legendItems.CollectionChanged += OnLegendItemsCollectionChanged;

            plotArea.LegendItemsUpdated += OnLegendItemsUpdated;

            Add(chartAreaView);
        }

        #endregion

        #region Properties

        public ILegend? Legend
        {
            get
            {
                return legend;
            }
            internal set
            {
                if (legend != value)
                {
                    legend = value;
                    plotArea.Legend = value;
                    CreateLegendView();
                }
            }
        }

        #endregion

        #region Methods

        protected override Size ArrangeOverride(Rect bounds)
        {
            //Calculate LegendItems before calling AreaBase's layout.
            plotArea.UpdateLegendItems();

            var areaBounds = new Rect(0, 0, bounds.Width, bounds.Height);

            if (legend != null)
            {
                if (legendItemsView != null)
                {
                    var legendRectangle = SfLegend.GetLegendRectangle(legendItemsView, new Rect(0, 0, bounds.Width, bounds.Height), 0.25);

                    if (legendItemsView.Placement == LegendPlacement.Top)
                    {
                        AbsoluteLayout.SetLayoutBounds(legendItemsView, new Rect(0, 0, bounds.Width, legendRectangle.Height));

                        areaBounds = new Rect(0, legendRectangle.Height, bounds.Width, bounds.Height - legendRectangle.Height);

                    }
                    else if (legendItemsView.Placement == LegendPlacement.Bottom)
                    {
                        AbsoluteLayout.SetLayoutBounds(legendItemsView, new Rect(0, bounds.Height - legendRectangle.Height, bounds.Width, legendRectangle.Height));

                        areaBounds = new Rect(0, 0, bounds.Width, bounds.Height - legendRectangle.Height);
                    }
                    else if (legendItemsView.Placement == LegendPlacement.Left)
                    {
                        AbsoluteLayout.SetLayoutBounds(legendItemsView, new Rect(0, 0, legendRectangle.Width, bounds.Height));
                        areaBounds = new Rect(legendRectangle.Width, 0, bounds.Width - legendRectangle.Width, bounds.Height);
                    }
                    else if (legendItemsView.Placement == LegendPlacement.Right)
                    {
                        AbsoluteLayout.SetLayoutBounds(legendItemsView, new Rect(bounds.Width - legendRectangle.Width, 0, legendRectangle.Width, bounds.Height));
                        areaBounds = new Rect(0, 0, bounds.Width - legendRectangle.Width, bounds.Height);
                    }

                    AbsoluteLayout.SetLayoutBounds(AreaBase, areaBounds);
                }
            }
            else
            {
                AbsoluteLayout.SetLayoutBounds(AreaBase, areaBounds);
            }

            return base.ArrangeOverride(bounds);
        }

        private void OnLegendItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {

        }

        private void OnLegendItemsUpdated(object? sender, EventArgs e)
        {
            if (Legend != null)
            {
                //Todo: For Placement and Oriention property changes
                //
                //legendItemsView.Orientation = Legend.Orientation;
                //legendItemsView.Placement = Legend.Placement;
                //legendItemsView.ToggleVisibility = Legend.ToggleVisibility;
                //legendItemsView.ItemsSource = plotArea.LegendItems;
            }
        }

        private void CreateLegendView()
        {
            //Todo: We need to reimplement the legend feature in chart, once fixed the Bindable layout issue and dynamically change scroll view content issue.
            //https://github.com/dotnet/maui/issues/1393
            if (legendItemsView != null)
                Remove(legendItemsView);

            if (Legend != null)
            {
                legendItemsView = new SfLegend();
                legendItemsView.Orientation = Legend.Orientation;
                legendItemsView.Placement = Legend.Placement;
                legendItemsView.ToggleVisibility = Legend.ToggleVisibility;
                legendItemsView.ItemsSource = plotArea.LegendItems;
                legendItemsView.ItemClicked += OnLegendItemToggled;
                Add(legendItemsView);
            }
        }

        private void OnLegendItemToggled(object? sender, LegendItemClickedEventArgs e)
        {
            ToggleLegendItem(e.LegendItem);
        }

        private void ToggleLegendItem(ILegendItem? legendItem)
        {
            if (legendItem != null)
            {
                plotArea.LegendItemToggleHandler.Invoke(legendItem);
            }
        }

        #endregion
    }
}
