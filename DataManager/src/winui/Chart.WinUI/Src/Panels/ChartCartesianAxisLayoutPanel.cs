// <copyright file="ChartCartesianAxisLayoutPanel.cs" company="Syncfusion. Inc">
// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>
namespace Syncfusion.UI.Xaml.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Media;
    using Windows.Foundation;

    /// <summary>
    /// Represents ChartCartesianAxisLayoutPanel
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    internal class ChartCartesianAxisLayoutPanel : ILayoutCalculator
    {
        #region Fields

        private double left = 0;
        private double bottom = 0;
        private double right = 0;
        private double top = 0;
        private Size _desiredSize;
        private Panel panel;

        private List<double> leftSizes = new List<double>();
        private List<double> rightSizes = new List<double>();
        private List<double> topSizes = new List<double>();
        private List<double> bottomSizes = new List<double>();

        #endregion

        #region Consturctor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartCartesianAxisLayoutPanel"/> class.
        /// </summary>
        /// <param name="panel">The Panel</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ChartCartesianAxisLayoutPanel(Panel panel)
        {
            //if(panel == null)
            //    throw new ArgumentNullException();

            this.panel = panel;
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the chart area where the panel is bounded.
        /// </summary>
        public ChartBase Area
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the panel.
        /// </summary>
        /// <value>
        /// The panel.
        /// </value>
        public Panel Panel
        {
            get { return panel; }
        }

        /// <summary>
        /// Gets the desired size of the panel.
        /// </summary>

        public Size DesiredSize
        {
            get
            {
                return _desiredSize; ;
            }
        }

        /// <summary>
        /// Gets the children count in the panel.
        /// </summary>

        public List<UIElement> Children
        {
            get
            {
                if (panel != null)
                {
                    return panel.Children.Cast<UIElement>().ToList();
                }

                return null;
            }
        }
        double ILayoutCalculator.Left
        {
            get { return Left; }
            set { Left = value; }
        }

        double ILayoutCalculator.Top
        {
            get { return Top; }
            set { Top = value; }
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets or sets the left value for <see cref="ILayoutCalculator"/>.
        /// </summary>
        internal double Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the right value for <see cref="ILayoutCalculator"/>.
        /// </summary>
        internal double Top
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Measures the elements in the panel.
        /// </summary>
        /// <param name="availableSize">available size of the panel.</param>
        /// <returns>Returns the desired size</returns>
        public Size Measure(Size availableSize)
        {
            left = right = top = bottom = 0;

            if (Area.ColumnDefinitions.Count == 0)
            {
                Area.ColumnDefinitions.Add(new ChartColumnDefinition());
            }

            if (Area.RowDefinitions.Count == 0)
            {
                Area.RowDefinitions.Add(new ChartRowDefinition());
            }

            foreach (ChartColumnDefinition column in Area.ColumnDefinitions)
            {
                column.Axis.Clear();
            }

            foreach (ChartRowDefinition row in Area.RowDefinitions)
            {
                row.Axis.Clear();
            }

            foreach (ChartAxis content in Area.InternalAxes)
            {
                if (!content.IsVertical)
                {
                    Area.ColumnDefinitions[Area.GetActualColumn(content)].Axis.Add(content);
                }
                else
                {
                    Area.RowDefinitions[Area.GetActualRow(content)].Axis.Add(content);
                }
            }

            // Spanning calculation for each axis
            if (Area != null)
                AxisSpanCalculation();
            leftSizes.Clear();
            rightSizes.Clear();
            topSizes.Clear();
            bottomSizes.Clear();

            MeasureAxis(availableSize, new Rect(new Point(0, 0), availableSize));
            Area.AxisThickness = new Thickness().GetThickness(left, top, right, bottom);

            // Area.SeriesClipRect = rect;

            UpdateArrangeRect(availableSize);

            Rect rect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0, 0), availableSize), this.Area.AxisThickness);

            Area.SeriesClipRect = rect;

            foreach (ChartSeries series in Area.VisibleSeries.OfType<ChartSeries>())
            {
                if (series.ActualXAxis != null && series.ActualYAxis != null)
                {
                    ChartAxis xAxis = series.ActualXAxis.IsVertical
                                          ? series.ActualYAxis
                                          : series.ActualXAxis;
                    ChartAxis yAxis = series.ActualYAxis.IsVertical
                                          ? series.ActualYAxis
                                          : series.ActualXAxis;

                    var x = xAxis.ArrangeRect.Left - rect.Left;
                    var y = yAxis.ArrangeRect.Top - rect.Top;
                    var width = xAxis.ArrangeRect.Width;
                    var height = yAxis.ArrangeRect.Height;

                    series.Clip = new RectangleGeometry()
                    {
                        Rect = new Rect(x, y, width + 0.5, height + 0.5)
                    };
                }
            }

            _desiredSize = availableSize;
            return availableSize;
        }

        /// <summary>
        /// Seek the elements from the panel.
        /// </summary>
        public void DetachElements()
        {
            foreach (var axis in Area.InternalAxes)
            {
                if (axis.MajorGridLinesPanel != null)
                    axis.MajorGridLinesPanel.Clear();
                if (axis.MinorGridLinesPanel != null)
                    axis.MinorGridLinesPanel.Clear();
            }
            panel.Children.Clear();
            panel = null;
        }

        /// <summary>
        /// Layouts the axis.
        /// </summary>
        /// <param name="finalSize">The final size used to arrange axes.</param>
        public Size Arrange(Size finalSize)
        {
            ArrangeAxes();
            return finalSize;
        }

        /// <summary>
        /// Adds the elements inside the panel.
        /// </summary>
        public void UpdateElements()
        {
            List<UIElement> removedElements = new List<UIElement>();
            if (Children == null) return;
            foreach (UIElement element in Children)
            {
                ChartAxis chartAxis = element as ChartAxis;

                if (chartAxis != null
                    && !Area.InternalAxes.Contains(chartAxis))
                {
                    if (chartAxis.MajorGridLinesPanel != null)
                        chartAxis.MajorGridLinesPanel.Clear();
                    if (chartAxis.MinorGridLinesPanel != null)
                        chartAxis.MinorGridLinesPanel.Clear();
                    removedElements.Add(chartAxis);
                }
            }

            foreach (UIElement removedElement in removedElements)
            {
                panel.Children.Remove(removedElement);
            }

            removedElements.Clear();

            var children = Children;

            foreach (ChartAxis content in this.Area.InternalAxes)
            {
                content.AxisLayoutPanel = null;
                if (!children.Contains(content))
                {
                    panel.Children.Add(content);
                }

            }
        }

#endregion

#region Private Methods

        /// <summary>
        /// Spanning Calculation for each axis.
        /// </summary>
        private void AxisSpanCalculation()
        {
            var spanningColAxes = (from column in Area.ColumnDefinitions
                                   from axis in column.Axis
                                   where Area.GetActualColumnSpan(axis) > 1
          && Area.GetActualColumn(axis) == Area.ColumnDefinitions.IndexOf(column)
                                   select axis).DefaultIfEmpty();
            foreach (var axis in spanningColAxes)
            {
                if (axis == null) break;
                int columnSpan = Area.GetActualColumnSpan(axis);
                int column = Area.GetActualColumn(axis);
                int i = Area.ColumnDefinitions[column].Axis.IndexOf(axis);
                for (int k = 1, m = column + 1; k < columnSpan && m < Area.ColumnDefinitions.Count; k++, m++)
                {
                    if (Area.ColumnDefinitions[m].Axis.Count > i)
                        Area.ColumnDefinitions[m].Axis.Insert(i, axis);
                }
            }

            var spanningRowAxes = (from row in Area.RowDefinitions
                                   from axis in row.Axis
                                   where Area.GetActualRowSpan(axis) > 1
              && Area.GetActualRow(axis) == Area.RowDefinitions.IndexOf(row)
                                   select axis).DefaultIfEmpty();
            foreach (var axis in spanningRowAxes)
            {
                if (axis == null) break;
                int rowSpan = Area.GetActualRowSpan(axis);
                int row = Area.GetActualRow(axis);
                int i = Area.RowDefinitions[row].Axis.IndexOf(axis);
                for (int k = 1, m = row + 1; k < rowSpan && m < Area.RowDefinitions.Count; k++, m++)
                {
                    if (Area.RowDefinitions[m].Axis.Count > i)
                        Area.RowDefinitions[m].Axis.Insert(i, axis);
                }
            }
        }

        /// <summary>
        /// Measures the axis and it's elements
        /// </summary>
        /// <param name="availableSize">The Available Size</param>
        /// <param name="seriesClipRect">The Series Clip Rectangle</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        private void MeasureAxis(Size availableSize, Rect seriesClipRect)
        {
            bool needLayout = true;
            bool isFirstLayout = true;
            Rect currectClipRect;
            while (needLayout)
            {
                needLayout = false;

                leftSizes.Clear();
                rightSizes.Clear();

                CalcRowSize(seriesClipRect);
                foreach (ChartRowDefinition row in Area.RowDefinitions)
                {
                    var rowSize = new Size(availableSize.Width, row.ComputedHeight);
                    row.Measure(rowSize, leftSizes, rightSizes, isFirstLayout);
                }

                left = leftSizes.Sum();
                right = rightSizes.Sum();
                top = topSizes.Count > 0 ? topSizes.Sum() : 0;
                bottom = bottomSizes.Count > 0 ? bottomSizes.Sum() : 0;
                var thickness = new Thickness().GetThickness(left, top, right, bottom);

                currectClipRect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0, 0), availableSize), thickness);

                if (Math.Abs(seriesClipRect.Width - currectClipRect.Width) > 0.5 || isFirstLayout)
                {
                    topSizes.Clear();
                    bottomSizes.Clear();

                    seriesClipRect = currectClipRect;

                    CalcColumnSize(seriesClipRect);
                    foreach (ChartColumnDefinition column in Area.ColumnDefinitions)
                    {
                        var columnSize = new Size(column.ComputedWidth, availableSize.Height);
                        column.Measure(columnSize, bottomSizes, topSizes);
                    }

                    left = leftSizes.Sum();
                    right = rightSizes.Sum();
                    top = topSizes.Sum();
                    bottom = bottomSizes.Sum();
                    thickness = new Thickness().GetThickness(left, top, right, bottom);
                    currectClipRect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0, 0), availableSize), thickness);

                    needLayout = Math.Abs(seriesClipRect.Height - currectClipRect.Height) > 0.5;

                    seriesClipRect = currectClipRect;
                }

                isFirstLayout = false;
            }
        }

        /// <summary>
        /// Calculates the row size.
        /// </summary>
        /// <param name="rect">The Rectangle.</param>
        private void CalcRowSize(Rect rect)
        {
            //Arranging vertical axis
            double rowTop = rect.Top;
            double usedHeight = 0;
            double rowStarCount = Area.RowDefinitions.Sum((rowDef)
                =>
            {
                if (rowDef.Unit == ChartUnitType.Star)
                    return rowDef.Height;
                return 0;
            });

            double rowFixedHeight = Area.RowDefinitions.Sum((rowDef)
                =>
            {
                if (rowDef.Unit == ChartUnitType.Pixels)
                    return rowDef.Height;
                return 0;
            });

            double remainingHeight = Math.Max(0, rect.Height - rowFixedHeight);
            double singleStarHeight = remainingHeight / rowStarCount;

            for (int i = Area.RowDefinitions.Count - 1; i >= 0; i--)
            {
                ChartRowDefinition row = Area.RowDefinitions[i];
                double remainingSize = rect.Height - usedHeight;
                double height = 0;
                if (row.Unit == ChartUnitType.Star)
                {
                    height = Math.Min(remainingSize, row.Height * singleStarHeight);
                }
                else
                {
                    height = Math.Min(remainingSize, row.Height);
                }
                row.ComputedHeight = height;
                row.ComputedTop = rowTop;
                usedHeight += double.IsNaN(height) ? 1d : height;
                row.RowTop = rowTop;
                rowTop += double.IsNaN(height) ? 1d : height;
            }
        }

        /// <summary>
        /// Calculates the column size.
        /// </summary>
        /// <param name="rect">The Rectangle</param>
        private void CalcColumnSize(Rect rect)
        {
            //Arranging horizontal axis

            double columnLeft = rect.Left;
            double usedWidth = 0;
            double columnStarCount = Area.ColumnDefinitions.Sum((columnDef)
                =>
            {
                if (columnDef.Unit == ChartUnitType.Star)
                    return columnDef.Width;
                return 0;
            });

            double columnFixedWidth = Area.ColumnDefinitions.Sum((columnDef)
                =>
            {
                if (columnDef.Unit == ChartUnitType.Pixels)
                    return columnDef.Width;
                return 0;
            });

            double remainingWidth = Math.Max(0, rect.Width - columnFixedWidth);
            double singleStarWidth = remainingWidth / columnStarCount;

            for (int i = 0; i < Area.ColumnDefinitions.Count; i++)
            {
                ChartColumnDefinition column = Area.ColumnDefinitions[i];
                double remainingSize = rect.Width - usedWidth;
                double width = 0;
                if (column.Unit == ChartUnitType.Star)
                {
                    width = Math.Min(remainingSize, column.Width * singleStarWidth);
                }
                else
                {
                    width = Math.Min(remainingSize, column.Width);
                }

                column.ComputedWidth = width;
                column.ComputedLeft = columnLeft;
                usedWidth += width;
                columnLeft += width;
            }
        }

        /// <summary>
        /// Arranges the elements in the panel.
        /// </summary>
        /// <param name="availableSize">Final size of the panel.</param>
        /// <returns>Returns Size</returns>
        private void UpdateArrangeRect(Size availableSize)
        {
            foreach (var row in Area.RowDefinitions)
            {
                row.UpdateArrangeRect(row.ComputedTop, row.ComputedHeight, availableSize.Width, leftSizes, rightSizes);
            }

            foreach (var column in Area.ColumnDefinitions)
            {
                column.UpdateArrangeRect(column.ComputedLeft, column.ComputedWidth, availableSize.Height, bottomSizes, topSizes);
            }
        }

        /// <summary>
        /// Arranges the axes.
        /// </summary>
        private void ArrangeAxes()
        {
            if (this.Area.InternalPrimaryAxis == null || this.Area.InternalSecondaryAxis == null)
                return;

            foreach (ChartColumnDefinition column in Area.ColumnDefinitions)
            {
                column.Arrange();
            }

            foreach (ChartRowDefinition row in Area.RowDefinitions)
            {
                row.Arrange();
            }
        }
        
#endregion

#endregion
    }
}
