namespace Syncfusion.UI.Xaml.Charts
{
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Data;
    using Windows.Foundation;
    using Rect = Windows.Foundation.Rect;

    /// <summary>
    /// Represents the position of a child element in the chart.
    /// </summary>  
    public enum LegendPlacement
    {
        /// <summary>
        /// Docks element at the left side of panel.
        /// </summary>
        Left,

        /// <summary>
        /// Docks element at the top side of panel.
        /// </summary>
        Top,

        /// <summary>
        /// Docks element at the right side of panel.
        /// </summary>
        Right,

        /// <summary>
        /// Docks element at the bottom side of panel.
        /// </summary>
        Bottom,
    }

    /// <summary>
    /// 
    /// </summary>
    internal class LegendPanel : Panel
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="AreaPanel"/> property.
        /// </summary>
        public static readonly DependencyProperty AreaPanelProperty =
          DependencyProperty.Register(nameof(AreaPanel), typeof(AreaPanel), typeof(LegendPanel), new PropertyMetadata(null, OnAreaPanelChanged));

        public static readonly DependencyProperty LegendProperty =
          DependencyProperty.Register(nameof(Legend), typeof(ILegend), typeof(LegendPanel), new PropertyMetadata(null, OnLegendChanged));

        #endregion

        #region Fields

        private Size legendSize;
        internal SfLegend LegendView { get; set; }

        internal Rect ArrangeRect;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the area panel. This is a dependency property.
        /// </summary>
        /// <value>The AreaPanel.</value>
        /// 
        public AreaPanel AreaPanel
        {
            get { return (AreaPanel)GetValue(AreaPanelProperty); }
            set { SetValue(AreaPanelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the legend element. This is a dependency property.
        /// </summary>
        /// <value>The ILegend.</value>
        public ILegend Legend
        {
            get { return (ILegend)GetValue(LegendProperty); }
            set { SetValue(LegendProperty, value); }
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            if (LegendView != null)
            {
                LegendView.Measure(availableSize);
                legendSize = LegendView.GetLegendMaxSize(availableSize, 0.25);

                if (LegendView.Placement == LegendPlacement.Top || LegendView.Placement == LegendPlacement.Bottom)
                {
                    AreaPanel?.Measure(new Size(availableSize.Width, availableSize.Height - legendSize.Height));
                }
                else
                {
                    AreaPanel?.Measure(new Size(availableSize.Width - legendSize.Width, availableSize.Height));
                }
            }
            else
            {
                AreaPanel?.Measure(new Size(availableSize.Width, availableSize.Height));
            }

            return availableSize;
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var areaBounds = new Rect(0, 0, finalSize.Width, finalSize.Height);

            if (LegendView != null)
            {
                if (LegendView.Placement == LegendPlacement.Top)
                {
                    double width = finalSize.Width - ArrangeRect.Left;
                    LegendView.Arrange(new Rect(ArrangeRect.Left, 0, width, legendSize.Height));
                    areaBounds = new Rect(0, legendSize.Height, (float)finalSize.Width, finalSize.Height - legendSize.Height);
                }
                else if (LegendView.Placement == LegendPlacement.Bottom)
                {
                    double width = finalSize.Width - ArrangeRect.Left;
                    LegendView.Arrange(new Rect(ArrangeRect.Left, finalSize.Height - legendSize.Height, width, legendSize.Height));
                    areaBounds = new Rect(0, 0, finalSize.Width, finalSize.Height - legendSize.Height);
                }
                else if (LegendView.Placement == LegendPlacement.Left)
                {
                    LegendView.Arrange(new Rect(0, 0, legendSize.Width, finalSize.Height));
                    areaBounds = new Rect(legendSize.Width, 0, finalSize.Width - legendSize.Width, finalSize.Height);
                }
                else if (LegendView.Placement == LegendPlacement.Right)
                {
                    LegendView.Arrange(new Rect(finalSize.Width - legendSize.Width, 0, legendSize.Width, finalSize.Height));
                    areaBounds = new Rect(0, 0, finalSize.Width - legendSize.Width, finalSize.Height);
                }
            }

            AreaPanel?.Arrange(areaBounds);

            return base.ArrangeOverride(finalSize);
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Called when root element is changed.
        /// </summary>
        /// <param name="dpObj">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnAreaPanelChanged(DependencyObject dpObj, DependencyPropertyChangedEventArgs e)
        {
            if (dpObj is LegendPanel legendPanel)
            {
                if (e.OldValue != null)
                {
                    legendPanel.Children.Remove(e.OldValue as AreaPanel);
                }

                if (e.NewValue != null)
                {
                    legendPanel.Children.Add(e.NewValue as AreaPanel);
                }
            }
        }


        private static void OnLegendChanged(DependencyObject dpObj, DependencyPropertyChangedEventArgs e)
        {
            if (dpObj is LegendPanel legendPanel)
            {
                if (e.OldValue != null && legendPanel.LegendView != null && legendPanel.Children.Contains(legendPanel.LegendView))
                {
                    legendPanel.Children.Remove(legendPanel.LegendView);
                    legendPanel.LegendView = null;
                }

                if (e.NewValue != null)
                {
                    legendPanel.CreateLegend();
                }

            }
        }

        #endregion

        private void CreateLegend()
        {
            LegendView = new SfLegend();
            LegendView.LegendPanel = this;
            LegendView.DataContext = Legend;
            LegendView.SetBinding(SfLegend.PlacementProperty, new Binding() { Path = new PropertyPath(nameof(Legend.Placement)) });
            LegendView.SetBinding(SfLegend.ToggleVisibilityProperty, new Binding() { Path = new PropertyPath(nameof(Legend.ToggleSeriesVisibility)) });
            LegendView.SetBinding(SfLegend.HeaderProperty, new Binding() { Path = new PropertyPath(nameof(Legend.Header)) });
            LegendView.SetBinding(SfLegend.HeaderTemplateProperty, new Binding() { Path = new PropertyPath(nameof(Legend.HeaderTemplate)) });
            LegendView.SetBinding(SfLegend.IsVisibleProperty, new Binding() { Path = new PropertyPath(nameof(Legend.IsVisible)) });
            LegendView.SetBinding(SfLegend.CornerRadiusProperty, new Binding() { Path = new PropertyPath(nameof(Legend.CornerRadius)) });
            LegendView.SetBinding(SfLegend.ItemsSourceProperty, new Binding() { Path = new PropertyPath(nameof(Legend.ItemsSource)) });
            LegendView.SetBinding(SfLegend.BackgroundProperty, new Binding() { Path = new PropertyPath(nameof(Legend.Background)) });
            LegendView.SetBinding(SfLegend.BorderBrushProperty, new Binding() { Path = new PropertyPath(nameof(Legend.BorderBrush)) });
            LegendView.SetBinding(SfLegend.BorderThicknessProperty, new Binding() { Path = new PropertyPath(nameof(Legend.BorderThickness)) });
            LegendView.SetBinding(SfLegend.PaddingProperty, new Binding() { Path = new PropertyPath(nameof(Legend.Padding)) });
            LegendView.SetBinding(SfLegend.HorizontalHeaderAlignmentProperty, new Binding() { Path = new PropertyPath(nameof(Legend.HorizontalHeaderAlignment)) });
            LegendView.SetBinding(SfLegend.LegendItemTemplateProperty, new Binding() { Path = new PropertyPath(nameof(Legend.ItemTemplate)) });

            Children.Add(LegendView);
        }

        internal void Dispose()
        {
            LegendView?.Dispose();
            AreaPanel = null;
            LegendView = null;
        }

        #endregion
    }
}
