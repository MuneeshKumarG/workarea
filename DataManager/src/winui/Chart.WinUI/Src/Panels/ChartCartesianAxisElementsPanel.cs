// <copyright file="ChartCartesianAxisElementsPanel.cs" company="Syncfusion. Inc">
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
    using System.Text;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Shapes;
    using Windows.Foundation;

    /// <summary>
    /// Represents ChartCartesianAxisElementsPanel.
    /// </summary>
    /// <remarks>
    /// The elements inside the panel comprises of <see cref="ChartAxis"/> axis line, major ticklines and minor ticklines.
    /// </remarks>
    internal class ChartCartesianAxisElementsPanel : ILayoutCalculator
    {
        #region Fields

        private ChartAxis axis;

        internal CompositorLinesPanel MajorTicksPanel;

        internal CompositorLinesPanel MinorTicksPanel;

        private Size desiredSize;

        private Panel labelsPanels;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartCartesianAxisElementsPanel"/> class.
        /// </summary>
        /// <param name="panel">The Panel</param>
        public ChartCartesianAxisElementsPanel(Panel panel)
        {
            this.labelsPanels = panel;
            MainAxisLine = new Line();
            if (panel != null)
                panel.Children.Add(MainAxisLine);
                

            
        }

        #endregion

        #region Properties

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

        /// <summary>
        /// Gets the panel.
        /// </summary>
        /// <value>
        /// The panel.
        /// </value>
        public Panel Panel
        {
            get { return labelsPanels; }
        }

        /// <summary>
        /// Gets the desired size.
        /// </summary>
        public Size DesiredSize
        {
            get
            {
                return desiredSize;
            }
        }

        /// <summary>
        /// Gets the Children count in the panel.
        /// </summary>
        public List<UIElement> Children
        {
            get
            {
                if (labelsPanels != null)
                {
                    return labelsPanels.Children.Cast<UIElement>().ToList();
                }

                return null;
            }
        }

        /// <summary>
        /// Gets or sets the axis.
        /// </summary>
        internal ChartAxis Axis
        {
            get
            {
                return axis;
            }
            set
            {
                axis = value;
                SetAxisLineBinding();
                InitializeTicksPanel();
            }
        }

        /// <summary>
        /// Gets or sets the main axis line.
        /// </summary>
        internal Line MainAxisLine { get; set; }

        #endregion

        #region Methods

        #region Public Methods
            
        /// <summary>
        /// Method declaration for Measure
        /// </summary>
        /// <param name="availableSize">The Available Size</param>
        /// <returns>Returns the size available for arranging the elements.</returns>
        public Size Measure(Size availableSize)
        {
            Size size = Size.Empty;
            double smallTickLineSize = (Axis is CategoryAxis) ? 5 : (Axis as RangeAxisBase).MinorTickLineSize;
            if (!Axis.IsVertical)
            {
                size = new Size(availableSize.Width, Math.Max(Math.Max(Axis.TickLineSize, smallTickLineSize), 0) + MainAxisLine.StrokeThickness);
            }
            else
            {
                size = new Size(Math.Max(Math.Max(Axis.TickLineSize, smallTickLineSize), 0) + MainAxisLine.StrokeThickness, availableSize.Height);
            }

            desiredSize = size;

            return size;
        }

        /// <summary>
        /// Seek the elements.
        /// </summary>
        public void DetachElements()
        {
            if (labelsPanels != null)
            {
                if (MainAxisLine != null && labelsPanels.Children != null && labelsPanels.Children.Contains(MainAxisLine))
                    labelsPanels.Children.Remove(MainAxisLine);

                if (MajorTicksPanel != null)
                {
                    if (labelsPanels.Children != null && labelsPanels.Children.Contains(MajorTicksPanel))
                        labelsPanels.Children.Remove(MajorTicksPanel);

                    MajorTicksPanel.Dispose();
                    MajorTicksPanel=null;
                }

                if (MinorTicksPanel != null)
                {
                    if (labelsPanels.Children != null && labelsPanels.Children.Contains(MinorTicksPanel))
                        labelsPanels.Children.Remove(MinorTicksPanel);

                    MinorTicksPanel.Dispose();
                    MinorTicksPanel = null;
                }

                labelsPanels = null;
            }
        }

        /// <summary>
        /// Method declaration for Arrange
        /// </summary>
        /// <param name="finalSize">The Final Size</param>
        /// <returns>Returns the arranged size.</returns>
        public Size Arrange(Size finalSize)
        {
            double[] values = (from val in Axis.VisibleLabels
                               select val.Position).ToArray();

            if (Axis.Area is ChartBase)
            {
                RenderAxisLine(finalSize);
                if (Axis is CategoryAxis && (Axis as CategoryAxis).LabelPlacement == LabelPlacement.BetweenTicks)
                    values = (from val in Axis.TickPositions
                              select val).ToArray();
                RenderTicks(finalSize, MajorTicksPanel, Axis.IsVertical, Axis.TickLineSize, values);

                var rangeAxis = Axis as RangeAxisBase;
                if (Axis.SmallTickRequired && rangeAxis != null)
                {
                    values = (from val in rangeAxis.SmallTickPoints
                              select val).ToArray();
                    RenderTicks(finalSize, MinorTicksPanel, Axis.IsVertical, rangeAxis.MinorTickLineSize, values);
                }
            }

            return finalSize;
        }

        /// <summary>
        /// Method declaration for UpdateElements
        /// </summary>
        public void UpdateElements()
        {
            UpdateTicks();
        }

        #endregion

        #region Internal Methods

        internal void Dispose()
        {
            axis = null;

            if (MajorTicksPanel != null)
            {
                MajorTicksPanel.Dispose();
                MajorTicksPanel = null;
            }

            if (MinorTicksPanel != null)
            {
                MinorTicksPanel.Dispose();
                MinorTicksPanel = null;
            }
        }

        /// <summary>
        /// Updates the tick lines.
        /// </summary>
        internal void UpdateTicks()
        {
            int tickCount = Axis.TickPositions.Count;
            if (!(Axis is CategoryAxis && (Axis as CategoryAxis).LabelPlacement == LabelPlacement.BetweenTicks))
                tickCount = Axis.VisibleLabels.Count;

            UpdateTicks(tickCount, MajorTicksPanel);

            var rangeAxis = Axis as RangeAxisBase;
            if (Axis.SmallTickRequired && rangeAxis != null)
                UpdateTicks(rangeAxis.SmallTickPoints.Count, MinorTicksPanel);
        }

        #endregion

        #region Private Methods

        private void InitializeTicksPanel()
        {
            MajorTicksPanel = new CompositorLinesPanel(axis.MajorTickStyle);
            labelsPanels.Children.Add(MajorTicksPanel);

            if (axis is RangeAxisBase rangeAxis)
            {
                MinorTicksPanel = new CompositorLinesPanel(rangeAxis.MinorTickStyle);
                labelsPanels.Children.Add(MinorTicksPanel);
            }
        }

        /// <summary>
        /// Binds the axis line style with the <see cref="Line"/> <see cref="Shape"/> .
        /// </summary>
        private void SetAxisLineBinding()
        {
            Binding binding = new Binding();
            binding.Source = axis;
            binding.Path = new PropertyPath("AxisLineStyle");
            MainAxisLine.SetBinding(Line.StyleProperty, binding);
        }

        /// <summary>
        /// Updates the tick lines.
        /// </summary>
        /// <param name="linescount">The Tick Lines Count</param>
        /// <param name="ticksPanel">The Tick lines panel</param>
        private void UpdateTicks(int linescount, CompositorLinesPanel ticksPanel)
        {
            ticksPanel?.GenerateLines(linescount);
        }

        /// <summary>
        /// Renders the axis line.
        /// </summary>
        /// <param name="finalSize">The Final Size</param>
        private void RenderAxisLine(Size finalSize)
        {
            double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
            double width = finalSize.Width;
            double height = finalSize.Height;
            double strokeHalfFactor = MainAxisLine.StrokeThickness / 2;
            bool isOpposed = Axis.OpposedPosition;

            if (!Axis.IsVertical)
            {
                x1 = this.Axis.AxisLineOffset;
                x2 = width - this.Axis.AxisLineOffset;

				// Positioning axis line according to it's corner, not to it's center.
				y2 = y1 = isOpposed ? height - strokeHalfFactor : strokeHalfFactor;
			}
            else
            {
                // Positioning axis line according to it's corner, not to it's center.
                x2 = x1 = isOpposed ? strokeHalfFactor : width - strokeHalfFactor;
                y1 = this.Axis.AxisLineOffset;
                y2 = height - this.Axis.AxisLineOffset;
            }

            if (MainAxisLine != null)
            {
                MainAxisLine.X1 = x1;
                MainAxisLine.Y1 = y1;
                MainAxisLine.X2 = x2;
                MainAxisLine.Y2 = y2;
            }
        }

        /// <summary>
        /// Renders the tick lines.
        /// </summary>
        /// <param name="finalSize">The Final Size</param>
        /// <param name="ticksPanel">Tick lines panel</param>
        /// <param name="orientation">The Orientation</param>
        /// <param name="tickSize">The Tick Size</param>
        /// <param name="Values">The Values</param>
        private void RenderTicks(
            Size finalSize,
            CompositorLinesPanel ticksPanel,
            bool isvertical,
            double tickSize,
            double[] Values)
        {

            int labelsCount = Values.Length;
            int linesCount = ticksPanel.LinesCount();
            List<float[]> drawValues = new List<float[]>();

            for (int i = 0; i < labelsCount; i++)
            {
                if (i < linesCount)
                {
                    double x1 = 0, y1 = 0, x2 = 0, y2 = 0;
                    double value = this.Axis.ValueToCoefficient(Values[i]);
                    value = double.IsNaN(value) ? 0 : value;

                    if (!isvertical)
                    {
                        x1 = x2 = Axis.GetActualPlotOffsetStart() + Math.Round((this.Axis.RenderedRect.Width * value));
                    }
                    else
                    {
                        y1 = y2 = (Axis.GetActualPlotOffsetEnd() + Math.Round(this.Axis.RenderedRect.Height * (1 - value)) + 0.5);
                    }

                    CalculatePosition(tickSize, ref x1, ref y1, ref x2, ref y2);

                    drawValues.Add(new float[] { (float)x1, (float)y1, (float)x2, (float)y2 });

                }
            }

            ticksPanel.UpdateLines(drawValues);
        }

        /// <summary>
        /// Calcuates the tick position.
        /// </summary>
        /// <param name="tickSize">The Tick Size</param>
        /// <param name="x1">The x 1 value</param>
        /// <param name="y1">The y 1 value</param>
        /// <param name="x2">The x 2 value</param>
        /// <param name="y2">The y 2 value</param>
        private void CalculatePosition(
            double tickSize,
            ref double x1,
            ref double y1,
            ref double x2,
            ref double y2)
        {
            bool isOpposed = Axis.OpposedPosition;
            double strokeHalfFactor = MainAxisLine.StrokeThickness / 2;

            // Positioning ticksize according to the axis stroke thickness size.
            if (!Axis.IsVertical)
            {
                y1 = isOpposed ? MainAxisLine.Y1 - strokeHalfFactor : MainAxisLine.Y1 + strokeHalfFactor;

                y2 = isOpposed ? y1 - tickSize : y1 + tickSize;
            }
            else
            {
                x1 = isOpposed ? MainAxisLine.X1 + strokeHalfFactor : MainAxisLine.X1 - strokeHalfFactor;
                x2 = isOpposed ? x1 + tickSize : x1 - tickSize;
            }
        }

		private static Binding GetTickLineBinding(object source, string propertyPath)
        {
            Binding binding = new Binding();
            binding.Path = new PropertyPath(propertyPath);
            binding.Source = source;
            return binding;
        }
		
        #endregion

        #endregion
    }
}
