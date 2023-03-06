// <copyright file="ChartCartesianAxisPanel.cs" company="Syncfusion. Inc">
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
    using System.Diagnostics;
    using System.Linq;
    using System.Numerics;
    using Microsoft.UI;
    using Microsoft.UI.Composition;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Hosting;
    using Microsoft.UI.Xaml.Media;
    using Microsoft.UI.Xaml.Shapes;
    using Windows.Foundation;

    /// <summary>
    /// This interfaces defines the members and methods to create and arrange the child elements in a panel.
    /// </summary>
    public interface ILayoutCalculator
    {
        #region Properties

        /// <summary>
        /// Gets Children property
        /// </summary>
        List<UIElement> Children { get; }

        /// <summary>
        /// Gets the panel.
        /// </summary>
        /// <value>
        /// The panel.
        /// </value>
        Panel Panel { get; }

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        double Left { get; set; }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        double Top { get; set; }

        /// <summary>
        /// Gets desiredSize property
        /// </summary>
        Size DesiredSize { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Method to measure the panel.
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        Size Measure(Size availableSize);

        /// <summary>
        /// Method to arrage the elements in panel
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        Size Arrange(Size finalSize);

        /// <summary>
        /// Method to update the elements (Children ) in the panel.
        /// </summary>
        void UpdateElements();

        /// <summary>
        /// Method to detachs elements from the panel.
        /// </summary>
        void DetachElements();

        #endregion
    }

    /// <summary>
    /// Represents <see cref="ChartCartesianAxisPanel"/> class.
    /// </summary>  
    internal class ChartCartesianAxisPanel : Canvas
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartCartesianAxisPanel"/> class.
        /// </summary>
        public ChartCartesianAxisPanel()
        {
            LayoutCalc = new List<ILayoutCalculator>();
        }

        #endregion

        #region Properties

        #region Internal Properties

        /// <summary>
        /// Gets or sets the chart axis.
        /// </summary>
        internal ChartAxis Axis { get; set; }

        /// <summary>
        /// Gets or sets the calculated layout.
        /// </summary>
        internal List<ILayoutCalculator> LayoutCalc { get; set; }

        #endregion

        #endregion

        #region Methods
        
        #region Internal Methods

        /// <summary>
        /// Computes the size of the <see cref="ChartCartesianAxisPanel"/>.
        /// </summary>
        /// <param name="availableSize">The Available Size</param>
        /// <returns>Returns the computed size.</returns>
        internal Size ComputeSize(Size availableSize)
        {
            Size size = Size.Empty;

            if (Axis.AxisLayoutPanel is ChartPolarAxisLayoutPanel)
            {
                foreach (UIElement element in this.Children)
                {
                    element.Measure(availableSize);
                }
                this.Children[0].Visibility = Visibility.Collapsed;//Collapsing the visibility of polar/radar series X-Axis header
                foreach (ILayoutCalculator element in this.LayoutCalc)
                {
                    element.Measure(availableSize);
                    size = element.DesiredSize;

                    ChartPolarAxisLayoutPanel axisLayoutPanel = Axis.AxisLayoutPanel as ChartPolarAxisLayoutPanel;
                    ChartCircularAxisPanel circularAxisPanel = element as ChartCircularAxisPanel;
                    axisLayoutPanel.Radius = circularAxisPanel.Radius;
                }
            }
            else
            {
                double horizontalPadding = 0;
                double verticalPadding = 0;
                double width = 0;
                double height = 0;
                double angle = 0d; //double.IsNaN(this.Axis.HeaderRotationAngle) ? 0d : this.Axis.HeaderRotationAngle;
                double direction = 1;

                if (Axis.headerContent != null)
                {
                    Axis.headerContent.HorizontalAlignment = HorizontalAlignment.Center;
                    Axis.headerContent.VerticalAlignment = VerticalAlignment.Center;

                    if (Axis.IsVertical)
                    {
                        direction = this.Axis.OpposedPosition ? 1 : -1;
                        angle = direction * 90;
                        var transform = new RotateTransform { Angle = angle };
                        Axis.headerContent.RenderTransform = transform;
                    }
                    else
                        Axis.headerContent.RenderTransform = null;
                }

                foreach (UIElement element in this.Children)
                {
                    bool isHeader = Axis.headerContent == element
                                    && this.Axis.IsVertical;

                    var measureSize = availableSize;
                    if (isHeader)
                    {
                        measureSize.Width = Math.Max(availableSize.Width, element.DesiredSize.Width);
                        measureSize.Height = Math.Max(availableSize.Height, element.DesiredSize.Height);
                    }

                    element.Measure(measureSize);

                    width += isHeader ? element.DesiredSize.Height : element.DesiredSize.Width;
                    height += isHeader ? element.DesiredSize.Width : element.DesiredSize.Height;
                }

                foreach (ILayoutCalculator element in LayoutCalc)
                {
                    element.Measure(availableSize);
                    width += element.DesiredSize.Width;
                    height += element.DesiredSize.Height;
                }


                if (Axis.IsVertical)
                {
                    Axis.InsidePadding = horizontalPadding;
                    size = new Size(width, availableSize.Height);
                }
                else
                {
                    Axis.InsidePadding = verticalPadding;
                    size = new Size(availableSize.Width, height);
                }
            }

            return ChartLayoutUtils.CheckSize(size);
        }

        /// <summary>
        /// Arranges the elements of the <see cref="ChartCartesianAxisPanel"/>
        /// </summary>
        /// <param name="finalSize">The Final Size</param>
        internal void ArrangeElements(Size finalSize)
        {
            if (Axis.AxisLayoutPanel is ChartPolarAxisLayoutPanel)
            {
                foreach (UIElement element in this.Children)
                {
                    element.Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
                    SetLeft(element, 0);
                    SetTop(element, 0);
                }

                foreach (ILayoutCalculator layout in this.LayoutCalc)
                {
                    layout.Arrange(finalSize);

                    ChartPolarAxisLayoutPanel axisLayoutPanel = Axis.AxisLayoutPanel as ChartPolarAxisLayoutPanel;
                    ChartCircularAxisPanel circularAxisPanel = layout as ChartCircularAxisPanel;
                    axisLayoutPanel.Radius = circularAxisPanel.Radius;
                }
            }
            else
            {
                ArrangeCartesianElements(finalSize);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Arranges the cartesian elements.
        /// </summary>
        /// <param name="finalSize">The Final Size</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals", Justification = "Reviewed")]
        private void ArrangeCartesianElements(Size finalSize)
        {
            if (Axis == null)
                return;

            foreach (UIElement element in Children)
            {
                SetLeft(element, 0);
                SetTop(element, 0);
            }

            var headerContent = Children[0];
            var labelsPanel = LayoutCalc.Count > 0 ? LayoutCalc[0] : null;
            var elementsPanel = LayoutCalc.Count > 1 ? LayoutCalc[1] : null;

            var elements = new List<UIElement>();
            var sizes = new List<Size>();
            var isInversed = Axis.OpposedPosition ^ Axis.IsVertical;

            if (elementsPanel != null)
            {
                elements.Add(elementsPanel.Panel);
                sizes.Add(elementsPanel.DesiredSize);
            }
            if (labelsPanel != null)
            {
                elements.Add(labelsPanel.Panel);
                sizes.Add(labelsPanel.DesiredSize);
            }
            
            if (headerContent != null)
            {
                elements.Add(headerContent);
                Size headerSize = headerContent.DesiredSize;
                if (Axis.IsVertical && headerContent.GetType() == typeof(ContentControl))
                {
                    headerSize = new Size(headerSize.Height, headerSize.Width);
                }
                sizes.Add(headerSize);
            }

            if (isInversed)
            {
                elements.Reverse();
                sizes.Reverse();
            }

            double currentPos = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                UIElement element = elements[i];
                double headerHeight = 0;
                double headerWidth = 0;
                if (Axis.IsVertical)
                {
                    if (element == headerContent)
                    {
                        double leftPosition = currentPos - ((element.DesiredSize.Width - sizes[i].Width) / 2);
                        headerHeight = element.DesiredSize.Height;
                        headerWidth = element.DesiredSize.Width;
                        SetTop(element, (finalSize.Height - element.DesiredSize.Height) / 2);

                        SetLeft(element, leftPosition);
                    }
                    else
                        SetLeft(element, currentPos);

                    currentPos += sizes[i].Width;
                }
                else
                {
                    if (element == headerContent)
                    {
                        SetLeft(element, (finalSize.Width - sizes[i].Width) / 2);
                        headerHeight = element.DesiredSize.Height;

                        SetTop(element, currentPos);
                    }
                    else
                        SetTop(element, currentPos);
                    currentPos += sizes[i].Height;
                }
            }

            foreach (ILayoutCalculator layout in this.LayoutCalc)
            {
                layout.Arrange(layout.DesiredSize);
            }


            foreach (UIElement element in elements)
            {
                if ((element as FrameworkElement).Name == "SyncfusionChartAxisLabelsPanel")
                {
                    SetLabelsPanelBounds(element, labelsPanel);
                    break;
                }
            }
        }
        
        /// <summary>
        /// Sets the labels panel bounds.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="labelsPanel"></param>
        private void SetLabelsPanelBounds(UIElement element, ILayoutCalculator labelsPanel)
        {
            var left = Canvas.GetLeft(element);
            var top = Canvas.GetTop(element);
            var width = labelsPanel.DesiredSize.Width;
            var height = labelsPanel.DesiredSize.Height;
            var cartesianAxisLabelsPanel = labelsPanel as ChartCartesianAxisLabelsPanel;

            if (!Axis.IsVertical)
            {
                left += Axis.ArrangeRect.Left;
                top += Axis.ArrangeRect.Top;
                cartesianAxisLabelsPanel.SetOffsetValues(left, top, width, height);
            }
            else
            {
                left += Axis.ArrangeRect.Left;
                top += Axis.ArrangeRect.Top;
                cartesianAxisLabelsPanel.SetOffsetValues(left, top, width, height);
            }
        }

        #endregion

        #endregion
    }


    /// <summary>
    /// Panel used to render line elements in compositor panel. 
    /// </summary>
    internal class CompositorLinesPanel : Panel
    {
        #region Fields
        private Compositor compositor;
        private ShapeVisual shapeVisual;
        private CompositionContainerShape compositionContainer;
        private Line line;
        private float strokeThickness;
        private CompositionColorBrush colorBrush;
        private List<float> strokeDashArray;
        private Brush lineStroke;
        #endregion

        #region Constructor
        public CompositorLinesPanel(Style style)
        {
            var visual = ElementCompositionPreview.GetElementVisual(this);
            compositor = visual.Compositor;
            var container = compositor.CreateContainerVisual();
            ElementCompositionPreview.SetElementChildVisual(this, container);
            shapeVisual = compositor.CreateShapeVisual();
            compositionContainer = compositor.CreateContainerShape();

            shapeVisual.Shapes.Add(compositionContainer);
            container.Children.InsertAtTop(shapeVisual);
            strokeDashArray = new List<float>();
            line = new Line();
            this.Children.Add(line);

            UpdateLineStyle(style);
        }
        #endregion

        #region Methods

        protected override Size MeasureOverride(Size availableSize)
        {
            if (shapeVisual != null)
                shapeVisual.Size = new Vector2((float)availableSize.Width, (float)availableSize.Height);

            if (lineStroke != line.Stroke && line.Stroke is SolidColorBrush brush)
            {
                lineStroke = line.Stroke;
                colorBrush = compositor.CreateColorBrush(brush.Color);
                foreach (CompositionSpriteShape shape in compositionContainer.Shapes)
                {
                    shape.StrokeBrush = colorBrush;
                }
            }

            return base.MeasureOverride(availableSize);
        }

        internal void GenerateLines(int totalLinesCount)
        {
            if (totalLinesCount > compositionContainer.Shapes.Count)
            {
                totalLinesCount = totalLinesCount - compositionContainer.Shapes.Count;

                for (int i = 0; i < totalLinesCount; i++)
                {
                    var geometry = compositor.CreateLineGeometry();
                    var shape = compositor.CreateSpriteShape(geometry);
                    UpdateLineStyle(shape);
                    compositionContainer.Shapes.Add(shape);
                }
            }
            else if (totalLinesCount < compositionContainer.Shapes.Count)
            {
                var count = compositionContainer.Shapes.Count - totalLinesCount;

                for (int i = 0; i < count; i++)
                {
                    compositionContainer.Shapes.RemoveAt(0);
                }
            }
        }

        internal void UpdateLineStyle(Style style)
        {
            if (style != null)
            {
                line.Style = style;
               
                strokeThickness = (float)line.StrokeThickness;

                lineStroke = line.Stroke;

                if (lineStroke is SolidColorBrush brush)
                    colorBrush = compositor.CreateColorBrush(brush.Color);

                if (line.StrokeDashArray != null)
                {
                    foreach (float item in line.StrokeDashArray)
                    {
                        strokeDashArray.Add(item);
                    }
                }
            }
            else
            {
                strokeThickness = 0;
            }
        }

        internal void ApplyLineStyle(Style style)
        {
            UpdateLineStyle(style);

            UpdateLineShapes();
        }

        internal void UpdateLineShapes()
        {
            foreach (CompositionSpriteShape shape in compositionContainer.Shapes)
            {
                UpdateLineStyle(shape);
            }
        }

        internal void UpdateLineStyle(CompositionSpriteShape shape)
        {
            shape.StrokeThickness = strokeThickness;
            shape.StrokeBrush = colorBrush;
            shape.StrokeDashArray.Clear();
            foreach (var item in strokeDashArray)
            {
                shape.StrokeDashArray.Add(item);
            }
        }

        internal void UpdateLines(List<float[]> values)
        {
            for (int i = 0; i < compositionContainer.Shapes.Count; i++)
            {
                var shape = compositionContainer.Shapes[i];

                if (shape is CompositionSpriteShape spriteShape)
                {
                    if (spriteShape.Geometry is CompositionLineGeometry line)
                    {
                        line.Start = new Vector2(values[i][0], values[i][1]);
                        line.End = new Vector2(values[i][2], values[i][3]);
                    }
                }
            }
        }

        internal void Clear()
        {
            if (compositionContainer != null)
                compositionContainer.Shapes?.Clear();
        }

        internal void Dispose()
        {
            compositor = null;
            shapeVisual = null;
            Clear();
            compositionContainer = null;
            line = null;
            colorBrush = null;
            strokeDashArray?.Clear();
            strokeDashArray = null;
            Children.Clear();
        }

        internal int LinesCount()
        {
            if (compositionContainer != null && compositionContainer.Shapes != null)
                return compositionContainer.Shapes.Count;
            else
                return 0;
        }
        #endregion
    }
}
