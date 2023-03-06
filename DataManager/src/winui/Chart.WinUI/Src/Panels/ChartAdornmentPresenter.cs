﻿// <copyright file="ChartDataMarkerPresenter.cs" company="Syncfusion. Inc">
// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>
namespace Syncfusion.UI.Xaml.Charts
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media;
    using Windows.Foundation;
    using Microsoft.UI.Xaml.Shapes;
    using Microsoft.UI.Xaml.Input;

    /// <summary>
    /// Represents <see cref="ChartDataMarkerPresenter"/> class.
    /// </summary>
    public class ChartDataMarkerPresenter : Canvas
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="VisibleSeries"/> property.
        /// </summary>
        public static readonly DependencyProperty VisibleSeriesProperty =
            DependencyProperty.Register(
                "VisibleSeries",
                typeof(ObservableCollection<ChartSeries>),
                typeof(ChartDataMarkerPresenter), new PropertyMetadata(null,
                new PropertyChangedCallback(OnVisibleSeriesPropertyChanged)));

        /// <summary>
        ///  The DependencyProperty for <see cref="Series"/> property.
        /// </summary>
        public static readonly DependencyProperty SeriesProperty =
            DependencyProperty.Register(
                "Series",
                typeof(ChartSeries),
                typeof(ChartDataMarkerPresenter),
                new PropertyMetadata(null));

        #endregion

        #region Fields

        private List<int> resetIndexes = new List<int>();

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDataMarkerPresenter"/> class.
        /// </summary>
        public ChartDataMarkerPresenter()
        {
            this.PointerMoved += ChartDataMarkerPresenter_PointerMoved;
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the VisibleSeries. This is a dependency property.
        /// </summary>
        /// <value>The VisibleSeries.</value>
        public ObservableCollection<ChartSeries> VisibleSeries
        {
            get { return (ObservableCollection<ChartSeries>)GetValue(VisibleSeriesProperty); }
            set { SetValue(VisibleSeriesProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Series collection in Chart.
        /// </summary>
        public ChartSeries Series
        {
            get { return (ChartSeries)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Methods

        /// <summary>
        /// Method is used to highlight the adornment.
        /// </summary>
        /// <param name="selectedAdornmentIndexes">The Adornment Index</param>
        /// <param name="isDataPointSelection">Used to indicate whether the corresponding data point selected or not</param>
        internal void UpdateAdornmentSelection(List<int> selectedAdornmentIndexes, bool isDataPointSelection)
        {
            Brush selectionBrush = null;

            if (Series.ActualArea.GetEnableSeriesSelection() && !isDataPointSelection)
                selectionBrush = Series.ActualArea.GetSeriesSelectionBrush(Series);
            else if (Series.GetEnableSegmentSelection())
                selectionBrush = Series.SelectionBehavior.SelectionBrush;

            if (selectionBrush != null)
            {
                if (selectedAdornmentIndexes != null && selectedAdornmentIndexes.Count > 0)
                {
                    foreach (var index in selectedAdornmentIndexes)
                    {
                        // Set selection brush to adornment label.
                        if (Series.adornmentInfo.Visible && Series.adornmentInfo.ContentTemplate == null
                         && (Series.adornmentInfo.UseSeriesPalette || Series.adornmentInfo.Background != null || Series.adornmentInfo.BorderBrush != null))
                        {
                            Border border = null;

                            if (Series.adornmentInfo.LabelPresenters.Count > 0 && VisualTreeHelper.GetChildrenCount(Series.adornmentInfo.LabelPresenters[index]) > 0)
                            {
                                ContentPresenter contentPresenter = VisualTreeHelper.GetChild(Series.adornmentInfo.LabelPresenters[index], 0) as ContentPresenter;

                                if (VisualTreeHelper.GetChildrenCount(contentPresenter) > 0)
                                {
                                    border = VisualTreeHelper.GetChild(contentPresenter, 0) as Border;
                                }
                            }

                            if (border != null)
                            {
                                if (border.Background != null)
                                    border.Background = selectionBrush;
                                if (border.BorderBrush != null)
                                    border.BorderBrush = selectionBrush;
                            }

                            var adornment = Series.Adornments[index];
                            if (adornment.ContrastForeground != null)
                            {
                                adornment.Foreground = selectionBrush.GetContrastColor();
                                adornment.ContrastForeground = adornment.Foreground;
                            }
                        }

                        // Set selection brush to adornment symbol
                        if (Series.adornmentInfo.ShowMarker && Series.adornmentInfo.adormentContainers.Count > 0)
                        {
                            ChartDataMarkerContainer symbol = Series.adornmentInfo.adormentContainers[index];

                            if (symbol.PredefinedSymbol != null)
                            {
                                symbol.PredefinedSymbol.Background = selectionBrush;
                                symbol.PredefinedSymbol.BorderBrush = selectionBrush;
                            }
                        }

                        // Set selection brush to adornment connector line.
                        if (Series.adornmentInfo.ConnectorLines.Count > 0 && Series.adornmentInfo.ShowConnectorLine)
                        {
                            Path line = Series.adornmentInfo.ConnectorLines[index];
                            line.Stroke = selectionBrush;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method is used to reset the adornment.
        /// </summary>
        internal void ResetAdornmentSelection(int? selectedIndex, bool isResetAll)
        {
            if (!isResetAll && selectedIndex != null
                && Series.ActualArea.SelectedSeriesCollection.Contains(Series)
                && Series.ActualArea.GetEnableSeriesSelection())
            {
                List<int> indexes = (from adorment in Series.Adornments
                                     where Series.ActualData[selectedIndex.Value] == adorment.Item
                                     select Series.Adornments.IndexOf(adorment)).ToList();

                UpdateAdornmentSelection(indexes, false);
            }
            else if (selectedIndex < Series.PointsCount)
            {
                resetIndexes.Clear();

                if (selectedIndex != null && !isResetAll)
                    if (Series is CircularSeries && !double.IsNaN(((CircularSeries)Series).GroupTo))
                        resetIndexes = (from adorment in Series.Adornments
                                        where Series.Segments[selectedIndex.Value].Item == adorment.Item
                                        select Series.Adornments.IndexOf(adorment)).ToList();
                    else if (Series.ActualXAxis is CategoryAxis && !(Series.ActualXAxis as CategoryAxis).IsIndexed
                    && Series.IsSideBySide)
                        resetIndexes = (from adorment in Series.Adornments
                                        where Series.GroupedActualData[selectedIndex.Value] == adorment.Item
                                        select Series.Adornments.IndexOf(adorment)).ToList();
                    else
                        resetIndexes = (from adorment in Series.Adornments
                                        where Series.ActualData[selectedIndex.Value] == adorment.Item
                                        select Series.Adornments.IndexOf(adorment)).ToList();
                else if (isResetAll)
                    resetIndexes = (from adorment in Series.Adornments
                                    select Series.Adornments.IndexOf(adorment)).ToList();

                foreach (var index in resetIndexes)
                {
                    UpdateAdormentBackground(index);
                }
            }
            else
            {
                for (int i = 0; i < Series.Adornments.Count; i++)
                {
                    UpdateAdormentBackground(i);
                }
            }
        }

        private void UpdateAdormentBackground(int index)
        {
            // Reset the adornment label
            if (Series.adornmentInfo.LabelPresenters.Count > index && Series.adornmentInfo.Visible
                && (Series.adornmentInfo.UseSeriesPalette || Series.adornmentInfo.Background != null || Series.adornmentInfo.BorderBrush != null))
            {
                Border border = null;
                if (VisualTreeHelper.GetChildrenCount(Series.adornmentInfo.LabelPresenters[index]) > 0)
                {
                    ContentPresenter contentPresenter = VisualTreeHelper.GetChild(Series.adornmentInfo.LabelPresenters[index], 0) as ContentPresenter; ;

                    if (VisualTreeHelper.GetChildrenCount(contentPresenter) > 0)
                        border = VisualTreeHelper.GetChild(contentPresenter as ContentPresenter, 0) as Border;
                }

                var adornment = Series.Adornments[index];

                if (border != null)
                {
                    if (Series.Adornments[index].Background != null)
                        border.Background = adornment.Background;
                    else if (Series.adornmentInfo.UseSeriesPalette)
                        border.Background = adornment.Fill;

                    if (Series.adornmentInfo.BorderBrush != null)
                        border.BorderBrush = adornment.BorderBrush;
                    else if (Series.adornmentInfo.UseSeriesPalette)
                        border.BorderBrush = adornment.Fill;
                }

                Series.adornmentInfo.UpdateForeground(adornment);
            }

            // Reset the adornment connector line
            if (Series.adornmentInfo.ConnectorLines.Count > index && Series.adornmentInfo.ShowConnectorLine)
            {
                Path path = Series.adornmentInfo.ConnectorLines[index];

                if (Series.adornmentInfo.UseSeriesPalette && Series.adornmentInfo.ConnectorLineStyle == null)
                    path.Stroke = Series.Adornments[index].Fill;
                else
                    path.ClearValue(Line.StrokeProperty);
            }

            // Reset the adornment symbol
            if (Series.adornmentInfo.adormentContainers.Count > index)
            {
                SymbolControl symbol = Series.adornmentInfo.adormentContainers[index].PredefinedSymbol;

                if (symbol != null && Series.adornmentInfo.ShowMarker)
                {
                    Binding binding;

                    if (Series.adornmentInfo.MarkerInterior == null)
                    {
                        binding = new Binding();
                        binding.Source = Series.Adornments[index];
                        binding.Path = new PropertyPath("Fill");
                        symbol.SetBinding(SymbolControl.BackgroundProperty, binding);
                    }
                    else
                    {
                        binding = new Binding();
                        binding.Source = Series.adornmentInfo;
                        binding.Path = new PropertyPath("MarkerInterior");
                        symbol.SetBinding(SymbolControl.BackgroundProperty, binding);
                    }

                    if (Series.adornmentInfo.MarkerStroke == null)
                    {
                        binding = new Binding();
                        binding.Source = Series.Adornments[index];
                        binding.Path = new PropertyPath("Fill");
                        symbol.SetBinding(SymbolControl.BorderBrushProperty, binding);
                    }
                    else
                    {
                        binding = new Binding();
                        binding.Source = Series.adornmentInfo;
                        binding.Path = new PropertyPath("MarkerStroke");
                        symbol.SetBinding(SymbolControl.BorderBrushProperty, binding);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the <see cref="ChartDataMarkerPresenter"/>.
        /// </summary>
        /// <param name="availableSize">The Available Size</param>
        internal void Update(Size availableSize)
        {
            if (Series != null && Series.adornmentInfo != null)
            {
                Series.adornmentInfo.Measure(availableSize, this);
            }
        }

        /// <summary>
        /// Arranges the adornment elements.
        /// </summary>
        /// <param name="finalSize">The Final Size.</param>
        internal void Arrange(Size finalSize)
        {
            if (Series != null && Series.adornmentInfo != null)
            {
                Series.adornmentInfo.Arrange(finalSize);
            }
        }

        private void ChartDataMarkerPresenter_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Series?.SeriesPointerMoved(e);
        }

        internal void UnHookEvents()
        {
            this.PointerMoved -= ChartDataMarkerPresenter_PointerMoved;
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Updates the visibility of the <see cref="ChartDataMarkerPresenter"/>.
        /// </summary>
        /// <param name="d">The Dependency Object</param>
        /// <param name="e">The Event Arguments</param>
        private static void OnVisibleSeriesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        #endregion

        #endregion
    }
}
