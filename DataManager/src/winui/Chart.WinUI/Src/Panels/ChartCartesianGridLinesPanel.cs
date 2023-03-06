// <copyright file="ChartCartesianGridLinesPanel.cs" company="Syncfusion. Inc">
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
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media;
    using Microsoft.UI.Xaml.Shapes;
    using Windows.Foundation;
    using Rect = Windows.Foundation.Rect;

    /// <summary>
    /// Represents <see cref="ChartCartesianGridLinesPanel"/>.
    /// </summary>
    internal class ChartCartesianGridLinesPanel : ILayoutCalculator
    {
        #region Fields

        private Panel panel;

        private Size desiredSize;

        private readonly UIElementsRecycler<Border> stripLines;

        private Line xOriginLine;

        private Line yOriginLine;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartCartesianGridLinesPanel"/> class.
        /// </summary>
        /// <param name="panel">The Panel</param>
        /// <exception cref="ArgumentNullException"><see cref="ArgumentNullException"/> may be thrown</exception>
        public ChartCartesianGridLinesPanel(Panel panel)
        {
            // if (panel == null)
            //    throw new ArgumentNullException();

            this.panel = panel;

            stripLines = new UIElementsRecycler<Border>(this.panel);
            xOriginLine = new Line();
            yOriginLine = new Line();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets the desired size of the panel.
        /// </summary>
        public Size DesiredSize
        {
            get { return desiredSize; }
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

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public double Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public double Top
        {
            get;
            set;
        }

        #endregion

        #region Internal Properties

        /// <summary>
        /// Gets or sets the chart area.
        /// </summary>
        internal ChartBase Area { get; set; }

        #endregion

        #endregion

        #region Methods

        #region Public Methods
        
        /// <summary>
        /// Draws the Gridlines at definite intervals in <see cref="ChartAxis"/>
        /// </summary>
        /// <param name="axis">Relevant ChartAxis</param>
        public void DrawGridLines(ChartAxis axis)
        {
            if (axis == null)
                return;

            double left = axis.RenderedRect.Left - Area.AxisThickness.Left;
            double right = axis.RenderedRect.Right - Area.AxisThickness.Left;
            double top = axis.RenderedRect.Top - Area.AxisThickness.Top;
            double bottom = axis.RenderedRect.Bottom - Area.AxisThickness.Top;

            double width = 0d;
            double height = 0d;


            var values = (from label in axis.VisibleLabels
                          select label.Position).ToArray();

            var categoryAxis = axis as CategoryAxis;
            if (categoryAxis != null && categoryAxis.LabelPlacement == LabelPlacement.BetweenTicks)
                values = (from pointValues in axis.TickPositions select pointValues).ToArray();

            if (!axis.IsVertical)
            {
                width = right - left;
                IEnumerable<ChartAxis> selectedAxes = null;
                if (axis.RegisteredSeries.Count > 0)
                {
                    selectedAxes = Area.RowDefinitions.Count > 1 ? axis.AssociatedAxes : axis.AssociatedAxes.DistinctBy(Area.GetActualRow);
                }
                else
                {
                    if (Area.InternalPrimaryAxis != null)
                        selectedAxes = new List<ChartAxis> { Area.InternalSecondaryAxis };
                }

                int index = 0;
                int smallTickIndex = 0;
                if (selectedAxes != null)
                    foreach (ChartAxis supportAxis in selectedAxes)
                    {
                        if (supportAxis == null)
                            continue;
                        var count = axis.RegisteredSeries.Where(item => (item.ActualXAxis == supportAxis || item.ActualYAxis == supportAxis)).Count();
                        if (count == 0) count = 1;
                        if (count > 0)
                        {
                            top = supportAxis.ArrangeRect.Top - Area.AxisThickness.Top;
                            height = top + supportAxis.ArrangeRect.Height;
                            if (values.Length > 0)
                                DrawGridLines(axis, axis.MajorGridLinesPanel, left, top, width, height, values, axis.ShowMajorGridLines, index);

                            var rangeAxis = axis as RangeAxisBase;
                            if (axis.SmallTickRequired && rangeAxis !=null )
                            {
                                var smallTickvalues = (from pointValues in rangeAxis.SmallTickPoints select pointValues).ToArray();
                                if (smallTickvalues.Length > 0)
                                    DrawGridLines(axis, axis.MinorGridLinesPanel, left, top, width, height, smallTickvalues, rangeAxis.ShowMinorGridLines, smallTickIndex);
                                smallTickIndex += smallTickvalues.Length;
                            }
                            index += values.Length;
                        }
                    }
            }
            else
            {
                height = bottom - top;

                IEnumerable<ChartAxis> selectedAxes = null;
                if (axis.RegisteredSeries.Count > 0)
                {
                    selectedAxes = Area.ColumnDefinitions.Count > 1 ? axis.AssociatedAxes : axis.AssociatedAxes.DistinctBy(Area.GetActualColumn);
                }
                else
                {
                    if (Area.InternalPrimaryAxis != null)
                        selectedAxes = new List<ChartAxis> { Area.InternalPrimaryAxis };
                }

                int index = 0;
                int smallTickIndex = 0;
                if (selectedAxes != null)
                    foreach (ChartAxis supportAxis in selectedAxes)
                    {
                        var count = axis.RegisteredSeries.Where(item => (item.ActualXAxis == supportAxis || item.ActualYAxis == supportAxis)).Count();
                        if (count == 0) count = 1;
                        if (count > 0)
                        {
                            left = supportAxis.ArrangeRect.Left - Area.AxisThickness.Left;
                            width = left + supportAxis.ArrangeRect.Width;
                            if (values.Length > 0)
                                DrawGridLines(axis, axis.MajorGridLinesPanel, left, top, width, height, values, axis.ShowMajorGridLines, index);

                            var rangeAxis = axis as RangeAxisBase;
                            if (axis.SmallTickRequired && rangeAxis != null)
                            {
                                var smallTickvalues = (from pointValues in rangeAxis.SmallTickPoints select pointValues).ToArray();
                                if (smallTickvalues.Length > 0)
                                    DrawGridLines(axis, axis.MinorGridLinesPanel, left, top, width, height, smallTickvalues, rangeAxis.ShowMinorGridLines, smallTickIndex);
                                smallTickIndex += smallTickvalues.Length;
                            }
                            index += values.Length;
                        }
                    }
            }
        }

        /// <summary>
        /// Measures the elements in the panel.
        /// </summary>
        /// <param name="availableSize">Available size of the panel.</param>
        /// <returns>Returns Size</returns>
        public Size Measure(Size availableSize)
        {
            desiredSize = availableSize;
            return availableSize;
        }

        /// <summary>
        /// Arrranges the elements inside a panel.
        /// </summary>
        /// <param name="finalSize">final size of the panel.</param>
        /// <returns>Returns Size</returns>
        public Size Arrange(Size finalSize)
        {
            foreach (ChartAxis axis in Area.InternalAxes)
            {
                DrawGridLines(axis);
            }
            return finalSize;
        }

        /// <summary>
        /// Seek the elements.
        /// </summary>
        public void DetachElements()
        {
            panel.Children.Clear();
            panel = null;

            if (stripLines != null)
                stripLines.Clear();
        }

        /// <summary>
        /// Adds the elements in the panel.
        /// </summary>
        public void UpdateElements()
        {
            foreach (ChartAxis axis in Area.InternalAxes)
            {
                UpdateGridLines(axis);
            }
        }

        /// <summary>
        /// Adds the Gridlines for the axis.
        /// </summary>
        /// <param name="axis">The Axis</param>       
        public void UpdateGridLines(ChartAxis axis)
        {
            if (axis == null)
                return;
           
            int axesCount = 1;
            if (axis.RegisteredSeries.Count > 0)
            {
                axesCount = !axis.IsVertical
                    ? Area.RowDefinitions.Count > 1 ? axis.AssociatedAxes.Count : (axis.AssociatedAxes.DistinctBy(Area.GetActualRow)).Count()
                    : Area.ColumnDefinitions.Count > 1 ? axis.AssociatedAxes.Count : (axis.AssociatedAxes.DistinctBy(Area.GetActualColumn)).Count();
            }

            int tickCount = axis.TickPositions.Count * axesCount;

            var categoryAxis = axis as CategoryAxis;
            if (!(categoryAxis != null && categoryAxis.LabelPlacement == LabelPlacement.BetweenTicks))
                tickCount = axis.VisibleLabels.Count * axesCount;

            var rangeAxis = axis as RangeAxisBase;
            if (axis.SmallTickRequired && rangeAxis !=null)
                ChartCartesianGridLinesPanel.UpdateGridlines(rangeAxis, axis.MinorGridLinesPanel, rangeAxis.SmallTickPoints.Count * axesCount, rangeAxis.ShowMinorGridLines);

            ChartCartesianGridLinesPanel.UpdateGridlines(axis, axis.MajorGridLinesPanel, tickCount, axis.ShowMajorGridLines);                      
        }

        #endregion

        #region Private Static Methods

        /// <summary>
        /// Updates the gridlines.
        /// </summary>
        /// <param name="axis">The Relevant Axis</param>
        /// <param name="compositorPanel">Grid lines panel</param>
        /// <param name="requiredLinescount">The Required Lines Count</param>
        /// <param name="showGridLines">Axis gridlines visibility.</param>
        private static void UpdateGridlines(ChartAxis axis, CompositorLinesPanel compositorPanel, int requiredLinescount, bool showGridLines)
        {
            int totalLinesCount = requiredLinescount;

            if (axis == null || !showGridLines)
                return;

            compositorPanel?.GenerateLines(totalLinesCount);
        }

        /// <summary>
        /// Creates the binding provider with the specifed path and source.
        /// </summary>
        /// <param name="path">The Path</param>
        /// <param name="source">The Source</param>
        /// <returns>Returns the binding provider.</returns>
        private static Binding CreateBinding(string path, object source)
        {
            var bindingProvider = new Binding
            {
                Path = new PropertyPath(path),
                Source = source,
                Mode = BindingMode.OneWay
            };
            return bindingProvider;
        }
		
		private static Binding GetGridLineBinding(object source, bool isMajor)
        {
            Binding binding = new Binding();
            binding.Path = isMajor ? new PropertyPath("MajorGridLineStyle") :
                new PropertyPath("MinorGridLineStyle");
            binding.Source = source;
            return binding;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Draws the gridlines with the specified values.
        /// </summary>
        /// <param name="axis">The Axis</param>
        /// <param name="panel">Grid lines panel</param>
        /// <param name="left">The Left</param>
        /// <param name="top">The Top</param>
        /// <param name="width">The Width</param>
        /// <param name="height">The Height</param>
        /// <param name="values">The Values</param>
        /// <param name="drawOrigin">Check For Draw Origin</param>
        /// <param name="index">The Index</param>
        private void DrawGridLines(ChartAxis axis, CompositorLinesPanel panel, double left, double top, double width, double height, double[] values, bool showGridlines, int index)
        {
            if (panel == null || axis == null || !showGridlines)
                return;

            int labelsCount = values.Length;
            int linesCount = panel.LinesCount();
            List<float[]> drawValues = new List<float[]>();

            if (!axis.IsVertical)
            {
                int i;
                for (i = 0; i < labelsCount; i++)
                {
                    if (i < linesCount)
                    {
                        double value = axis.ValueToCoefficient(values[i]);
                        value = double.IsNaN(value) ? 0 : value;
                        float x1 = (float)((Math.Round(width * value)) + left);
                        drawValues.Add(new float[] { x1, (float)top, x1, (float)height });
                    }
                    index++;
                }

                if (panel.Children.Contains(xOriginLine))
                    panel.Children.Remove(xOriginLine);
            }
            else
            {
                int i;
                for (i = 0; i < labelsCount; i++)
                {
                    if (i < linesCount)
                    {
                        
                        double value = axis.ValueToCoefficient(values[i]);
                        value = double.IsNaN(value) ? 0 : value;
                        float y1 = (float)(Math.Round(height * (1 - value)) + 0.5 + top);
                        drawValues.Add(new float[] { (float)left, y1, (float)width, y1 });
                    }
                    index++;
                }

                if (panel.Children.Contains(yOriginLine))
                    panel.Children.Remove(yOriginLine);
            }

            panel.UpdateLines(drawValues);
        }

        #endregion

        #endregion
    }
}
