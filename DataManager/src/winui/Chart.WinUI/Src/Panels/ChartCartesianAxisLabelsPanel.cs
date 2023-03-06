// <copyright file="ChartCartesianAxisLabelsPanel.cs" company="Syncfusion. Inc">
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
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media;
    using Microsoft.UI.Xaml.Shapes;
    using Windows.Foundation;
    using System.Threading.Tasks;
    using Rect = Windows.Foundation.Rect;

    /// <summary>
    /// Represents layout panel for chart axis labels.
    /// </summary>
    /// <remarks>
    /// The elements inside the panel comprises of <see cref="ChartAxis"/> labels.You can customize the label elements appearance using  
    /// <see cref="ChartAxis.LabelTemplate"/> property.
    /// </remarks>
    internal class ChartCartesianAxisLabelsPanel : ILayoutCalculator
    {
        #region Fields

        private Rect bounds;

        private Panel labelsPanels;

        private Size desiredSize;

        private UIElementsRecycler<TextBlock> textBlockRecycler;

        private UIElementsRecycler<ContentControl> contentControlRecycler;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartCartesianAxisLabelsPanel"/>.
        /// </summary>
        /// <param name="panel">The Panel</param>
        public ChartCartesianAxisLabelsPanel(Panel panel)
        {
            LabelLayout = null;
            labelsPanels = panel;
            textBlockRecycler = new UIElementsRecycler<TextBlock>(panel);
            contentControlRecycler = new UIElementsRecycler<ContentControl>(panel);
        }

        #endregion

        #region Properties

        #region Public Properties

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
        /// Gets the desired size of the panel.
        /// </summary>
        public Size DesiredSize
        {
            get
            {
                return desiredSize;
            }
        }

        /// <summary>
        /// Gets or sets the chart axis of the panel.
        /// </summary>
        public ChartAxis Axis { get; set; }

        /// <summary>
        /// Gets the children count in the panel.
        /// </summary>
        public List<UIElement> Children
        {
            get
            {
                children = textBlockRecycler.generatedElements.Cast<UIElement>().ToList();
                if (children.Count < 1)
                    children = contentControlRecycler.generatedElements.Cast<UIElement>().ToList();
                return children;
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
        /// Gets or sets the direct children of <see cref="ChartCartesianAxisLabelsPanel"/>
        /// </summary>
        internal List<UIElement> children { get; set; }
        
        /// <summary>
        /// Gets or sets the <see cref="AxisLabelLayout"/>.
        /// </summary>
        internal AxisLabelLayout LabelLayout { get; set; }

        internal Rect Bounds
        {
            get { return bounds; }
            set { bounds = value; }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Method declaration for Measure
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        public Size Measure(Size availableSize)
        {
            LabelLayout = AxisLabelLayout.CreateAxisLayout(Axis, Children);
            desiredSize = LabelLayout.Measure(availableSize);
            return desiredSize;
        }

        /// <summary>
        /// Method declaration for Arrange
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        public Size Arrange(Size finalSize)
        {
            if (LabelLayout != null)
            {
                LabelLayout.Left = Left;
                LabelLayout.Top = Top;
                LabelLayout.Arrange(DesiredSize);
                LabelLayout = null;
            }
            return finalSize;
        }

        /// <summary>
        /// Seek the elements.
        /// </summary>
        public void DetachElements()
        {
            labelsPanels = null;
            if (textBlockRecycler != null)
                textBlockRecycler.Clear();
            if (contentControlRecycler != null)
                contentControlRecycler.Clear();
        }

        /// <summary>
        /// Method declaration for UpdateElements
        /// </summary>
        public void UpdateElements()
        {
            GenerateContainers();
        }

        #endregion

        #region Internal Methods
        
        internal void Dispose()
        {
            UnbindAndDetachContentControlRecyclerElements(true);

            if (textBlockRecycler != null && textBlockRecycler.Count > 0)
            {
                foreach (var textBlock in textBlockRecycler)
                {
                    textBlock.ClearValue(TextBlock.TagProperty);
                }

                textBlockRecycler.Clear();
                textBlockRecycler = null;
            }

            Axis = null;
        }

        internal void SetOffsetValues(double left, double top, double width, double height)
        {
            int count = Children.Count;
            if (count == 0) return;

            if (!Axis.IsVertical)
            {
                var firstLabelLeft = Canvas.GetLeft(Children[0]);
                var lastLabelRight = Canvas.GetLeft(Children[count - 1]) + Children[count - 1].DesiredSize.Width;

                if (firstLabelLeft < Axis.ArrangeRect.Left)
                {
                    left = firstLabelLeft;
                    width += (Axis.ArrangeRect.Left - firstLabelLeft);
                }

                if (Axis.ArrangeRect.Right < lastLabelRight)
                    width += (lastLabelRight - Axis.ArrangeRect.Width);
            }
            else
            {
                var lastLabelTop = Canvas.GetTop(this.Children[count - 1]);
                var firstLabelBottom = Canvas.GetTop(Children[0]) + Children[0].DesiredSize.Height;
                if (lastLabelTop < Axis.ArrangeRect.Top)
                {
                    top = lastLabelTop;
                    height += (Axis.ArrangeRect.Top - lastLabelTop);
                }
                if (Axis.ArrangeRect.Bottom < firstLabelBottom)
                    height += (firstLabelBottom - DesiredSize.Height);
            }
            Bounds = new Rect(left, top, width, height);
        }

        internal void GenerateContainers()
        {
            int pos = 0;

            ObservableCollection<ChartAxisLabel> visibleLabels = Axis.VisibleLabels;
            if (Axis.LabelTemplate == null && Axis.LabelStyle == null)
            {
                contentControlRecycler.Clear();
                textBlockRecycler.GenerateElements(visibleLabels.Count);

                foreach (var item in visibleLabels)
                {
                    if (item.Content != null)
                    {
                        var textblock = textBlockRecycler[pos];
                        textblock.HighContrastAdjustment = ElementHighContrastAdjustment.None;
                        textblock.Text = item.Content.ToString();
                        textblock.Tag = visibleLabels[pos];
                    }
                    pos++;
                }
            }
            else if (this.Axis.LabelTemplate == null)
            {
                textBlockRecycler.Clear();
                UnbindAndDetachContentControlRecyclerElements(false);
                contentControlRecycler.GenerateElements(visibleLabels.Count);

                foreach (var item in visibleLabels)
                {
                    ClearLabelBinding(contentControlRecycler[pos]);

                    ContentControl control = contentControlRecycler[pos];
                    SetLabelStyle(item, control);
                    control.Content = item;
                    control.Tag = visibleLabels[pos];
                    control.ContentTemplate = ChartDictionaries.GenericCommonDictionary["SyncfusionChartAxisLabelsTemplate"] as DataTemplate;
                    control.ApplyTemplate();
                    pos++;
                }
            }
            else
            {
                textBlockRecycler.Clear();
                contentControlRecycler.GenerateElements(visibleLabels.Count);
                foreach (var item in visibleLabels)
                {
                    ContentControl control = contentControlRecycler[pos];
                    control.ContentTemplate = this.Axis.LabelTemplate;
                    control.ApplyTemplate();
                    control.Content = item;
                    control.Tag = visibleLabels[pos];
                    pos++;
                }
            }
        }

        #endregion

        #region Private Methods

        private void UnbindAndDetachContentControlRecyclerElements(bool isDisposing)
        {
            if (contentControlRecycler != null && contentControlRecycler.Count > 0)
            {
                foreach (var contentControl in contentControlRecycler)
                {
                    contentControl.ClearValue(ContentControl.ForegroundProperty);
                    contentControl.ClearValue(ContentControl.FontSizeProperty);
                    contentControl.ClearValue(ContentControl.FontFamilyProperty);
                    contentControl.ClearValue(ContentControl.TagProperty);
                    contentControl.ClearValue(ContentControl.ContentProperty);
                    contentControl.ClearValue(ContentControl.DataContextProperty);

                    contentControl.Content = null;
                }

                if(isDisposing)
                {
                    contentControlRecycler.Clear();
                }
            }
        }
        
        private void SetLabelStyle(ChartAxisLabel chartAxisLabel, ContentControl control)
        {
            var style = Axis.LabelStyle;
            if (style != null)
            {
                if (style.Foreground != null)
                {
                    var foregroundBinding = new Binding { Source = style, Path = new PropertyPath("Foreground") };
                    control.SetBinding(Control.ForegroundProperty, foregroundBinding);
                }
                if (style.FontSize > 0.0)
                {
                    var fontSizeBinding = new Binding { Source = style, Path = new PropertyPath("FontSize") };
                    control.SetBinding(Control.FontSizeProperty, fontSizeBinding);
                }
                if (style.FontFamily != null)
                {
                    var fontFamilyBinding = new Binding { Source = style, Path = new PropertyPath("FontFamily") };
                    control.SetBinding(Control.FontFamilyProperty, fontFamilyBinding);
                }
            }
        }

        private static void ClearLabelBinding(ContentControl contentControl)
        {
            contentControl.ClearValue(ContentControl.ForegroundProperty);
            contentControl.ClearValue(ContentControl.FontSizeProperty);
            contentControl.ClearValue(ContentControl.FontFamilyProperty);
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Represents a base of chart axis label layout. 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    internal abstract class AxisLabelLayout
    {
        #region Fields

        private readonly List<UIElement> children;

        /// <summary>
        /// Specifies padding for label border.
        /// </summary>
        internal double BorderPadding = 10;

        /// <summary>
        /// Specifies auto rotation angle for label.
        /// </summary>
        internal double AngleForAutoRotate = 0;

        /// <summary>
        /// Specifies label margin.
        /// </summary>
        internal Thickness Margin = new Thickness().GetThickness(2, 2, 2, 2);

        #endregion

        #region Constructor

        /// <summary>
        /// initializes a new instance of the <see cref="ChartCartesianAxisLabelsPanel"/> class.
        /// </summary>
        /// <param name="axis">The Axis</param>
        /// <param name="elements">The Elements</param>
        internal AxisLabelLayout(ChartAxis axis, List<UIElement> elements)
        {
            Axis = axis;
            children = elements;
            DesiredSizes = new List<Size>();
        }

#endregion

#region Properties

#region Internal Properties

        /// <summary>
        /// Gets or sets the left of the <see cref="ChartCartesianAxisLabelsPanel"/>
        /// </summary>
        internal double Left { get; set; }

        /// <summary>
        /// Gets or sets the top of the <see cref="ChartCartesianAxisLabelsPanel"/>
        /// </summary>
        internal double Top { get; set; }

        internal Size AvailableSize { get; set; }

#endregion

#region Protected Properties
        /// <summary>
        /// Gets or sets the rects of rows and columns of labels.
        /// </summary>
        internal List<Dictionary<int, Rect>> RectssByRowsAndCols { get; set; }

        /// <summary>
        /// Gets or sets the width and height of the element after rotating.
        /// </summary>
        internal List<Size> ComputedSizes { get; set; }

        /// <summary>
        /// Gets or sets the width and height of the element without rotating.
        /// </summary>
        internal List<Size> DesiredSizes { get; set; }

        /// <summary>
        /// Gets or sets the axis of the <see cref="ChartCartesianAxisLabelsPanel"/>
        /// </summary>
        internal ChartAxis Axis { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ChartCartesianAxisLabelsPanel"/> children.
        /// </summary>
        internal List<UIElement> Children
        {
            get { return children; }
        }
#endregion

#endregion

#region Methods

#region Public Methods

        /// <summary>
        /// Method used to create the axis layout.
        /// </summary>
        /// <param name="chartAxis">The Chart Axis</param>
        /// <param name="elements">The Elements</param>
        /// <returns>Returns the created layout.</returns>
        public static AxisLabelLayout CreateAxisLayout(ChartAxis chartAxis, List<UIElement> elements)
        {
            if (!chartAxis.IsVertical)
            {
                return new HorizontalLabelLayout(chartAxis, elements);
            }
            return new VerticalLabelLayout(chartAxis, elements);
        }

        /// <summary>
        /// Method declaration for Measure.
        /// </summary>
        /// <param name="availableSize">The Available Size</param>
        /// <returns>Returns the size required for arranging the elements.</returns>
        internal virtual Size Measure(Size availableSize)
        {
            if (Axis != null && Children.Count > 0)
            {
                bool needToRotate = (!double.IsNaN(Axis.LabelRotation) && Axis.LabelRotation != 0.0) || AngleForAutoRotate != 0;
                ComputedSizes = DesiredSizes;
                if (needToRotate) ComputedSizes = new List<Size>();
                AxisLabelsVisibilityBinding(); // Axis elements visibilty binding done here
                var labelMeaureSize = new Size();

                foreach (FrameworkElement element in Children)
                {
                    // WPF-41527 Axis label get shift after resizing window to minimum size with label rotation angle.
                    labelMeaureSize.Width = Math.Max(availableSize.Width, element.DesiredSize.Width);
                    labelMeaureSize.Height = Math.Max(availableSize.Height, element.DesiredSize.Height);
                    element.Measure(labelMeaureSize);

                    DesiredSizes.Add(element.DesiredSize);

                    if (needToRotate)
                    {
                        int labelIndex = Children.IndexOf(element);
                        double angle;
                        double tempAngle;
                        if (AngleForAutoRotate != 0)
                        {
                            angle = tempAngle = AngleForAutoRotate;
                        }
                        else
                        {
                            angle = Axis.LabelRotation;
                            tempAngle = Math.Abs(Axis.LabelRotation);
                        }

                        TransformGroup transformGroup;
                        TranslateTransform translateTransform;
                        RotateTransform rotateTransform;

                        if (angle < -360 || angle > 360)
                            angle %= 360;

                        element.RenderTransformOrigin = new Point(0.5, 0.5);

                        rotateTransform = new RotateTransform() { Angle = angle };

                        double tempAngleForRadians = angle;

                        if (!Axis.OpposedPosition)
                        {
                            if (!Axis.IsVertical)
                            {
                                if ((tempAngleForRadians > 180 && tempAngleForRadians < 360)
                                    || (tempAngleForRadians < 0 && tempAngleForRadians > -180))
                                    tempAngleForRadians -= 180;
                            }
                            else
                            {
                                if ((tempAngle > 0 && tempAngle < 90) || (tempAngle > 270 && tempAngle < 360))
                                    tempAngleForRadians += 180;
                            }
                        }
                        else
                        {
                            if (!Axis.IsVertical)
                            {
                                if ((tempAngleForRadians > 0 && tempAngleForRadians < 180)
                                    || (tempAngleForRadians < -180 && tempAngleForRadians > -360))
                                    tempAngleForRadians += 180;
                            }
                            else
                            {
                                if ((tempAngle > 90 && tempAngle < 180) || (tempAngle > 180 && tempAngle < 270))
                                    tempAngleForRadians += 180;
                            }
                        }

                        double angleRadians = (Math.PI * tempAngleForRadians) / 180;
                        double hypotenuse;
                        hypotenuse = element.DesiredSize.Width / 2;
                        double opposite = Math.Sin(angleRadians) * hypotenuse;
                        double adjacent = Math.Cos(angleRadians) * hypotenuse;

                        if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift && (labelIndex == 0 || labelIndex == Children.Count - 1))
                        {
                            adjacent = 0;
                            opposite = 0;
                        }

                        if (!Axis.IsVertical)
                        {
                            if ((tempAngle >= 0 && tempAngle < 1) || (tempAngle > 359 && tempAngle <= 360)
                                || (tempAngle > 179 && tempAngle < 181)
                                || (Axis is CategoryAxis && (Axis as CategoryAxis).LabelPlacement == LabelPlacement.BetweenTicks))
                            {
                                translateTransform = new TranslateTransform();
                            }
                            else
                            {
                                translateTransform = new TranslateTransform() { X = adjacent };
                            }
                        }
                        else
                        {
                            if ((tempAngle > 89 && tempAngle < 91) || (tempAngle > 269 && tempAngle < 271))
                            {
                                translateTransform = new TranslateTransform();
                            }
                            else
                            {
                                translateTransform = new TranslateTransform() { Y = opposite };
                            }
                        }

                        transformGroup = new TransformGroup();
                        transformGroup.Children.Add(rotateTransform);
                        transformGroup.Children.Add(translateTransform);
                        element.RenderTransform = transformGroup;

                        ComputedSizes.Add(AxisLabelLayout.GetRotatedSize(angle, DesiredSizes.Last()));
                    }
                    else
                    {
                        element.RenderTransform = null;
                    }
                }

                CalculateActualPlotOffset(availableSize);
            }

            return new Size();
        }

        /// <summary>
        /// Method declaration for Arrange.
        /// </summary>
        /// <param name="finalSize">The Final Size</param>
        internal virtual void Arrange(Size finalSize)
        {
        }

#endregion

#region Protected Methods

        /// <summary>
        /// Checks for the side by side series.
        /// </summary>
        /// <returns>Returns true when any of registered series is side by side series</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal bool CheckCartesianSeries()
        {
            return (Axis.RegisteredSeries != null && Axis.RegisteredSeries.Count > 0 &&
                  Axis.RegisteredSeries.Any(
                     series => (series is CartesianSeries) &&
                     (series as CartesianSeries).IsSideBySide));
        }

        /// <summary>
        /// Checks for the intersection of the rectangles.
        /// </summary>
        /// <param name="r1">The First Rectangle</param>
        /// <param name="r2">The Second Rectangle</param>
        /// <param name="prevIndex">The Previous Index</param>
        /// <param name="currentIndex">The Current Index</param>
        /// <returns>Returns a value indicating whether the rectanges are intersected.</returns>
        internal bool IntersectsWith(Rect r1, Rect r2, int prevIndex, int currentIndex)
        {
            double angle = AngleForAutoRotate == 45 ? 45 : Axis.LabelRotation;
            if (angle != 0)
            {
                var shape1Points = GetRotatedPoints(r1, prevIndex, angle);
                var shape2Points = GetRotatedPoints(r2, currentIndex, angle);

                return AxisLabelLayout.IntersectsWith(shape1Points, shape2Points);
            }

            return !(r2.Left > r1.Right ||
                     r2.Right < r1.Left ||
                     r2.Top > r1.Bottom ||
                     r2.Bottom < r1.Top);
        }

        /// <summary>
        /// Insert the <see cref="Rect"/> at the given row column index.
        /// </summary>
        /// <param name="rowOrColIndex">The Row Column Index</param>
        /// <param name="itemIndex">The Item Index</param>
        /// <param name="rect">The Rect</param>
        internal void InsertToRowOrColumn(int rowOrColIndex, int itemIndex, Rect rect)
        {
            if (RectssByRowsAndCols.Count <= rowOrColIndex)
            {
                RectssByRowsAndCols.Add(new Dictionary<int, Rect>());
                RectssByRowsAndCols[rowOrColIndex].Add(itemIndex, rect);
            }
            else
            {
                var rowOrColumn = RectssByRowsAndCols[rowOrColIndex].Last();
                Rect prevRect = rowOrColumn.Value;

                if (IntersectsWith(prevRect, rect, rowOrColumn.Key, itemIndex))
                {
                    InsertToRowOrColumn(++rowOrColIndex, itemIndex, rect);
                }
                else
                {
                    RectssByRowsAndCols[rowOrColIndex].Add(itemIndex, rect);
                }
            }
        }

        /// <summary>
        /// Calulates the bounds
        /// </summary>
        /// <param name="size">The Size</param>
        internal virtual void CalcBounds(double size)
        {
        }

        /// <summary>
        /// Checks the actual opposed position of the labels.
        /// </summary>
        /// <param name="axis">The Axis</param>
        /// <param name="isAxisOpposed">The Axis Opposed Indication</param>
        /// <returns>Returns the actual opposed position.</returns>
        internal static bool IsOpposed(ChartAxis axis, bool isAxisOpposed)
        {
            if (axis != null)
            {
                return isAxisOpposed;
            }

            return false;
        }

        /// <summary>
        /// Layuouts the axis labels.
        /// </summary>
        /// <returns>Returns desired height</returns>
        internal virtual double LayoutElements()
        {
            int i = 1;
            int prevIndex = 0;

            if (Axis.GetLabelIntersection() == AxisLabelsIntersectAction.Hide || AngleForAutoRotate == 90)
            {
                for (; i < Children.Count; i++)
                {
                    if (IntersectsWith(RectssByRowsAndCols[0][prevIndex], RectssByRowsAndCols[0][i], prevIndex, i))
                    {
                        var rangeAxis = Axis as RangeAxisBase;
                        if (rangeAxis != null && i == Children.Count - 1 &&
                            (rangeAxis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible
                            || (rangeAxis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && !Axis.IsZoomed)))
                            Children[prevIndex].Visibility = Visibility.Collapsed;
                        else
                        {
                            Children[i].Visibility = Visibility.Collapsed;

                        }
                    }
                    else
                    {
                        prevIndex = i;
                    }
                }
            }
            else if (Axis.GetLabelIntersection() == AxisLabelsIntersectAction.MultipleRows || Axis.GetLabelIntersection() == AxisLabelsIntersectAction.Wrap)
            {
                i = 1;
                prevIndex = 0;

                for (; i < Children.Count; i++)
                {
                    if (IntersectsWith(RectssByRowsAndCols[0][prevIndex], RectssByRowsAndCols[0][i], prevIndex, i))
                    {
                        Rect rect = RectssByRowsAndCols[0][i];
                        RectssByRowsAndCols[0].Remove(i);
                        InsertToRowOrColumn(1, i, rect);
                    }
                    else
                    {
                        prevIndex = i;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Calculates the actual plotoffset.
        /// </summary>
        /// <param name="availableSize">The Available Size</param>
        internal virtual void CalculateActualPlotOffset(Size availableSize)
        {
            Axis.ActualPlotOffset = 0;
            Axis.ActualPlotOffsetStart = Axis.PlotOffsetStart < 0 ? 0 : Axis.PlotOffsetStart;
            Axis.ActualPlotOffsetEnd = Axis.PlotOffsetEnd < 0 ? 0 : Axis.PlotOffsetEnd;
        }

#endregion

#region Private Static Methods

        /// <summary>
        /// Checks whether two line segments are intersecting
        /// </summary>
        /// <param name="point11">The Point 11</param>
        /// <param name="point12">The Point 12</param>
        /// <param name="point21">The Point 21</param>
        /// <param name="point22">The Point 22</param>
        /// <returns>Returns a value indicating whether the lines are intersecting.</returns>
        private static bool DoLinesIntersect(Point point11, Point point12, Point point21, Point point22)
        {
            double d = (point22.Y - point21.Y) * (point12.X - point11.X) -
                (point22.X - point21.X) * (point12.Y - point11.Y);
            double na = (point22.X - point21.X) * (point11.Y - point21.Y) -
                         (point22.Y - point21.Y) * (point11.X - point21.X);
            double nb = (point12.X - point11.X) * (point11.Y - point21.Y) -
                         (point12.Y - point11.Y) * (point11.X - point21.X);

            if (d == 0)
                return false;

            double ua = na / d;
            double ub = nb / d;

            return (ua >= 0d && ua <= 1d && ub >= 0d && ub <= 1d);
        }

        /// <summary>
        /// Calculates the rotated size.
        /// </summary>
        /// <param name="angle">The Angle</param>
        /// <param name="size">The Size</param>
        /// <returns>Returns the rotated size.</returns>
        private static Size GetRotatedSize(double angle, Size size)
        {
            var angleRadians = (2 * Math.PI * angle) / 360;
            var sine = Math.Sin(angleRadians);
            var cosine = Math.Cos(angleRadians);
#if !WinUI_UWP
            var matrix = new Matrix(cosine, sine, -sine, cosine, 0, 0);
#else
            var matrix = MatrixHelper.FromElements(cosine, sine, -sine, cosine, 0, 0);
#endif
#if !WinUI_UWP
            var leftTop = matrix.Transform(new Point(0, 0));
            var rightTop = matrix.Transform(new Point(size.Width, 0));
            var leftBottom = matrix.Transform(new Point(0, size.Height));
            var rightBottom = matrix.Transform(new Point(size.Width, size.Height));
#else
            var leftTop = MatrixHelper.Transform(matrix, new Point(0, 0));
            var rightTop = MatrixHelper.Transform(matrix, new Point(size.Width, 0));
            var leftBottom = MatrixHelper.Transform(matrix, new Point(0, size.Height));
            var rightBottom = MatrixHelper.Transform(matrix, new Point(size.Width, size.Height));
#endif
            var left = Math.Min(Math.Min(leftTop.X, rightTop.X), Math.Min(leftBottom.X, rightBottom.X));
            var top = Math.Min(Math.Min(leftTop.Y, rightTop.Y), Math.Min(leftBottom.Y, rightBottom.Y));
            var right = Math.Max(Math.Max(leftTop.X, rightTop.X), Math.Max(leftBottom.X, rightBottom.X));
            var bottom = Math.Max(Math.Max(leftTop.Y, rightTop.Y), Math.Max(leftBottom.Y, rightBottom.Y));

            return new Size(right - left, bottom - top);
        }

        /// <summary>
        /// Returns the points after translating the rect about (0,0) and then translating it by some x and y.
        /// </summary>
        /// <param name="angle">Angle to rotate</param>
        /// <param name="rect">Rect</param>
        /// <param name="translateX">Offset x to be translated after rotating</param>
        /// <param name="translateY">Offset y to be translated after rotating</param>
        /// <returns>Returns the rotated points.</returns>
        private static List<Point> GetRotatedPoints(double angle, Rect rect, double translateX, double translateY)
        {
            var angleRadians = (2 * Math.PI * angle) / 360;
            var sine = Math.Sin(angleRadians);
            var cosine = Math.Cos(angleRadians);
#if !WinUI_UWP
            var matrix = new Matrix(cosine, sine, -sine, cosine, translateX, translateY);
             var transformedPoints = new List<Point>();
            transformedPoints.Add(matrix.Transform(new Point(rect.Left, rect.Top)));
            transformedPoints.Add(matrix.Transform(new Point(rect.Right, rect.Top)));
            transformedPoints.Add(matrix.Transform(new Point(rect.Right, rect.Bottom)));
            transformedPoints.Add(matrix.Transform(new Point(rect.Left, rect.Bottom)));
#else
            var matrix = MatrixHelper.FromElements(cosine, sine, -sine, cosine, translateX, translateY);
            var transformedPoints = new List<Point>();
            transformedPoints.Add(MatrixHelper.Transform(matrix, new Point(rect.Left, rect.Top)));
            transformedPoints.Add(MatrixHelper.Transform(matrix, new Point(rect.Right, rect.Top)));
            transformedPoints.Add(MatrixHelper.Transform(matrix, new Point(rect.Right, rect.Bottom)));
            transformedPoints.Add(MatrixHelper.Transform(matrix, new Point(rect.Left, rect.Bottom)));
#endif

            return transformedPoints;
        }

        /// <summary>
        /// Checks whether two polygons intersects.
        /// </summary>
        /// <param name="shape1Points">Polygon</param>
        /// <param name="shape2Points">Polygon</param>
        /// <returns></returns>
        private static bool IntersectsWith(List<Point> shape1Points, List<Point> shape2Points)
        {
            //Checks whether two lines from both the shapes intersects. 
            //If it intersects, it means both the shapes are intersecting.
            for (int i = 0; i < shape1Points.Count; i++)
            {
                var point11 = shape1Points[i];
                var nextIndex = i == shape1Points.Count - 1 ? 0 : i + 1;
                var point12 = shape1Points[nextIndex];
                for (int j = 0; j < shape2Points.Count; j++)
                {
                    var point21 = shape2Points[j];
                    nextIndex = j == shape2Points.Count - 1 ? 0 : j + 1;
                    var point22 = shape2Points[nextIndex];

                    if (AxisLabelLayout.DoLinesIntersect(point11, point12, point21, point22))
                        return true;
                }
            }

            return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Returns the points after rotating a rectangle.
        /// </summary>
        /// <param name="rect">The Rectangle</param>
        /// <param name="index">The Index</param>
        /// <param name="angle">Used to indicate label rotation angle</param>
        /// <returns>Returns the rotated points.</returns>
        private List<Point> GetRotatedPoints(Rect rect, int index, double angle)
        {
            //Get actual left and actual top of the element without rotating
            var left = rect.Left + (ComputedSizes[index].Width - DesiredSizes[index].Width) / 2;
            var top = rect.Top + (ComputedSizes[index].Height - DesiredSizes[index].Height) / 2;

            //Rotating the points about the origin (0,0) and translating it to actual left and top
            var offsetX = DesiredSizes[index].Width / 2;
            var offsetY = DesiredSizes[index].Height / 2;
            rect = new Rect(-offsetX, -offsetY, DesiredSizes[index].Width, DesiredSizes[index].Height);
            var translateX = left + offsetX;
            var translateY = top + offsetY;

            return AxisLabelLayout.GetRotatedPoints(angle, rect, translateX, translateY);
        }

        /// <summary>
        /// Binds the visiblilty of the axis labels with <see cref="TextBlock"/>.
        /// </summary>
        private void AxisLabelsVisibilityBinding()
        {
            foreach (FrameworkElement element in Children)
            {
                Binding binding = new Binding();
                binding.Source = Axis;
                binding.Path = new PropertyPath("Visibility");
                element.SetBinding(TextBlock.VisibilityProperty, binding);
            }
        }

#endregion

        internal void CalculateWrapLabelRect()
        {
            bool isLabelWrap = true;
            int previousIndex = 0;
            int i = 1;
            List<Dictionary<int, Rect>> labelRects = RectssByRowsAndCols;
            ObservableCollection<ChartAxisLabel> labels = Axis.VisibleLabels;
            var length = labels.Count;

            for (; i < length; i++)
            {
                var previousRect = labelRects[0][previousIndex];
                var currentRect = labelRects[0][i];

                if (IntersectsWith(previousRect, currentRect, previousIndex, i))
                {
                    var prevWrapWidth = (currentRect.Left - previousRect.Left) - (Margin.Left + Margin.Right);
                    isLabelWrap = LabelContainWrapWidth(labels[previousIndex].Content.ToString(), prevWrapWidth);

                    if (isLabelWrap || (isLabelWrap && i == length - 1))
                    {
                        Size availableSize = new Size(prevWrapWidth, double.MaxValue);
                        (Children[previousIndex] as UIElement).Measure(availableSize);
                        ComputedSizes[previousIndex] = (Children[previousIndex] as UIElement).DesiredSize;

                        RectssByRowsAndCols[0].Remove(previousIndex);
                        RectssByRowsAndCols[0].Add(previousIndex, new Rect(previousRect.X, previousRect.Y, ComputedSizes[previousIndex].Width, ComputedSizes[previousIndex].Height));

                        if (i == length - 1)
                        {
                            var x = currentRect.Left + (previousRect.Right - currentRect.Left);
                            var wrapWidth = (currentRect.Right - x) - (Margin.Left + Margin.Right);

                            isLabelWrap = LabelContainWrapWidth(labels[i].Content.ToString(), wrapWidth);

                            if (isLabelWrap)
                            {
                                Size wrapSize = new Size(wrapWidth, double.MaxValue);
                                (Children[i] as UIElement).Measure(wrapSize);
                                ComputedSizes[i] = (Children[i] as UIElement).DesiredSize;

                                RectssByRowsAndCols[0].Remove(i);
                                RectssByRowsAndCols[0].Add(i, new Rect(currentRect.X, currentRect.Y, ComputedSizes[i].Width, ComputedSizes[i].Height));
                            }                       
                        }
                    }
                }

                previousIndex = i;
            }
        }

        private static bool LabelContainWrapWidth(string label, double wrapWidth)
        {
            string[] words = label.Split(' ');

            for (int i = 0; i < words.Length; i++)
            {
                TextBlock text = new TextBlock() { Text = words[i] };
                text.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var labelSize = text.DesiredSize;

                if (labelSize.Width > wrapWidth)
                {
                    return false;
                }
            }

            return true;
        }

#endregion
    }

    /// <summary>
    /// Represents a axis layout in chart control that indicates the layout orientation as horizontal. 
    /// </summary>
    /// <seealso cref="AxisLabelLayout" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    internal class HorizontalLabelLayout : AxisLabelLayout
    {
#region Fields

        private bool isOpposed;
        private double maxHeight;

#endregion

#region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalLabelLayout"/> class.
        /// </summary>
        /// <param name="axis">The Axis</param>
        /// <param name="elements">The Elements</param>
        public HorizontalLabelLayout(ChartAxis axis, List<UIElement> elements) : base(axis, elements)
        {
        }

#endregion
        
#region Methods

#region Public Methods

        /// <summary>
        /// Measures the labels in the <see cref="HorizontalLabelLayout"/>.
        /// </summary>
        /// <param name="availableSize">The Available Size</param>
        /// <returns>Returns the size required to arrange the elements</returns>
        internal override Size Measure(Size availableSize)
        {
            double desiredHeight = 0;

            if (Axis != null && Children.Count > 0)
            {
                AvailableSize = availableSize;
                base.Measure(availableSize);
                CalcBounds(availableSize.Width - Axis.GetActualPlotOffset());
                if (Axis.GetLabelIntersection() == AxisLabelsIntersectAction.Auto
                    && Axis.LabelRotation == 0 && AngleForAutoRotate != 90)
                {
                    int prevIndex = 0;
                    double angle = 0;

                    for (int i = 1; i < Children.Count; i++)
                    {
                        if (IntersectsWith(RectssByRowsAndCols[0][prevIndex], RectssByRowsAndCols[0][i], prevIndex, i))
                        {
                            angle = AngleForAutoRotate == 45 ? 90 : 45;
                        }
                        else
                        {
                            prevIndex = i;
                        }
                    }

                    if (angle != 0)
                    {
                        AngleForAutoRotate = angle;
                        Measure(availableSize);
                    }
                }

                desiredHeight = LayoutElements();
                desiredHeight = Math.Max(desiredHeight, Axis.LabelExtent);

                desiredHeight += ((Margin.Top + Margin.Bottom) * RectssByRowsAndCols.Count);
                return new Size(availableSize.Width, desiredHeight);
            }

            return new Size(availableSize.Width, 0);
        }

        /// <summary>
        /// Arranges the labels in the <see cref="HorizontalLabelLayout"/>
        /// </summary>
        /// <param name="finalSize"></param>
        internal override void Arrange(Size finalSize)
        {
            if (RectssByRowsAndCols == null)
                return;

            isOpposed = false;
            bool needToRotate = (!double.IsNaN(Axis.LabelRotation) && Axis.LabelRotation != 0.0) || AngleForAutoRotate != 0;

            int row = 0;
            double top = 0;
            isOpposed = AxisLabelLayout.IsOpposed(Axis, Axis.OpposedPosition);
            top = isOpposed ? finalSize.Height - Margin.Bottom : Margin.Top;

            maxHeight = RectssByRowsAndCols.Select(dictionary =>
             dictionary.Values.Max(rect => rect.Height)).FirstOrDefault();
            maxHeight = RectssByRowsAndCols.Count > 1 ? maxHeight + Margin.Top :
              maxHeight + BorderPadding;

            foreach (Dictionary<int, Rect> dictionary in RectssByRowsAndCols)
            {
                foreach (KeyValuePair<int, Rect> keyValue in dictionary)
                {
                    UIElement element = Children[keyValue.Key];
                    double actualTop = isOpposed ? top - ComputedSizes[keyValue.Key].Height : top;

                    double actualLeft = keyValue.Value.Left + Axis.GetActualPlotOffsetStart();

                    if (needToRotate)
                    {
                        actualTop += (ComputedSizes[keyValue.Key].Height - DesiredSizes[keyValue.Key].Height) / 2;
                        actualLeft += (ComputedSizes[keyValue.Key].Width - DesiredSizes[keyValue.Key].Width) / 2;
                    }

                    Canvas.SetLeft(element, actualLeft);
                    Canvas.SetTop(element, actualTop);
                }
                if (isOpposed)
                    top -= (dictionary.Values.Max(rect => rect.Height) + Margin.Bottom);
                else
                    top += (dictionary.Values.Max(rect => rect.Height) + Margin.Top);

                row++;
            }
        }

#endregion

#region Protected Methods

        /// <summary>
        /// Calculates the actual plot offset.
        /// </summary>
        /// <param name="availableSize">The Available Size</param>
        internal override void CalculateActualPlotOffset(Size availableSize)
        {
            if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Fit)
            {
                double coeff = Axis.ValueToCoefficient(Axis.VisibleLabels[0].Position);
                double position = (coeff * availableSize.Width) - (ComputedSizes[0].Width / 2);
                double firstElementWidth = 0;
                double lastElementWidth = 0;

                if ((position - ComputedSizes[0].Width / 2) < 0)
                {
                    firstElementWidth = ComputedSizes[0].Width;
                }

                int index = Children.Count - 1;
                if ((position + ComputedSizes[index].Width / 2) < availableSize.Width)
                {
                    lastElementWidth = ComputedSizes[index].Width;
                }

                double offset = Math.Max(firstElementWidth / 2, lastElementWidth / 2);
                Axis.ActualPlotOffset = Math.Max(offset, 0);
            }
            else
            {
                base.CalculateActualPlotOffset(availableSize);
            }
        }

        /// <summary>
        /// Layouts the elements
        /// </summary>
        /// <returns>Returns the desired height.</returns>
        internal override double LayoutElements()
        {
            if (Axis.GetLabelIntersection() == AxisLabelsIntersectAction.Wrap)
            {
                CalculateWrapLabelRect();
                CalcBounds(AvailableSize.Width - Axis.GetActualPlotOffset());
            }

            base.LayoutElements();

            return RectssByRowsAndCols.Sum(dictionary => dictionary.Values.Max(rect => rect.Height));
        }

        /// <summary>
        /// Calculates the bounds.
        /// </summary>
        /// <param name="availableWidth">The Available Width</param>
        internal override void CalcBounds(double availableWidth)
        {
            RectssByRowsAndCols = new List<Dictionary<int, Rect>>();
            RectssByRowsAndCols.Add(new Dictionary<int, Rect>());

            for (int j = 0; j < Children.Count; j++)
            {
                double position = 0d;
                var linearAxis = Axis as NumericalAxis;

                {
                    double coeff = Axis.ValueToCoefficient(Axis.VisibleLabels[j].Position);
                    position = (coeff * availableWidth) - (ComputedSizes[j].Width / 2);
                }

                RectssByRowsAndCols[0].Add(j, new Rect(new Point(position, 0), ComputedSizes[j]));
            }

            if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift)
            {
                if (RectssByRowsAndCols[0][0].Left < 0)
                {
                    RectssByRowsAndCols[0][0] = new Rect(0, 0, ComputedSizes[0].Width, ComputedSizes[0].Height);
                }
                int index = Children.Count - 1;
                if (RectssByRowsAndCols[0][index].Right > availableWidth)
                {
                    double position = availableWidth - ComputedSizes[Children.Count - 1].Width;
                    RectssByRowsAndCols[0][index] = new Rect(position, 0, ComputedSizes[index].Width,
                                                             ComputedSizes[index].Height);
                }
            }
            else if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Hide)
            {
                if (RectssByRowsAndCols[0][0].Left < 0)
                {
                    RectssByRowsAndCols[0][0] = new Rect(0, 0, 0, 0);
                    Children[0].Visibility = Visibility.Collapsed;
                }
                int index = Children.Count - 1;
                if (RectssByRowsAndCols[0][index].Right > availableWidth)
                {
                    RectssByRowsAndCols[0][index] = new Rect(0, 0, 0, 0);
                    Children[index].Visibility = Visibility.Collapsed;
                }
            }
        }

#endregion

#region Private Static Methods

        /// <summary>
        /// Positions the labels back.
        /// </summary>
        /// <param name="isOpposed">Is Oppoosed Indication</param>
        /// <param name="leftElementShift">The Left Element Shift.</param>
        /// <param name="topElementShift">The Top Element Shift</param>
        /// <param name="actualTiltAngle">The Actual Tilt Angle</param>
        private static void PositionLabelsBack(bool isOpposed, ref UIElementLeftShift leftElementShift, ref UIElementTopShift topElementShift, double actualTiltAngle)
        {
            leftElementShift = UIElementLeftShift.LeftHalfShift;

            if (isOpposed)
            {
                if (actualTiltAngle >= 45 && actualTiltAngle < 315)
                    topElementShift = UIElementTopShift.Default;
            }
            else
            {
                if (actualTiltAngle >= 45 && actualTiltAngle < 315)
                    topElementShift = UIElementTopShift.TopShift;
            }
        }

        /// <summary>
        /// Positions the label right.
        /// </summary>
        /// <param name="leftElementShift">The Left Element Shift</param>
        /// <param name="topElementShift">The Top Element Shift</param>
        /// <param name="actualTiltAngle">The Actaul Tilt Angle</param>
        private static void PositionLabelsRight(ref UIElementLeftShift leftElementShift, ref UIElementTopShift topElementShift, double actualTiltAngle)
        {
            leftElementShift = UIElementLeftShift.Default;

            if (actualTiltAngle >= 45 && actualTiltAngle < 315)
                topElementShift = UIElementTopShift.TopHalfShift;
        }

        /// <summary>
        /// Positions the label left.
        /// </summary>
        /// <param name="leftElementShift">The Left Element Shift</param>
        /// <param name="topElementShift">The Top Element Shift</param>
        /// <param name="actualTiltAngle">The Actual Tilt Angle</param>
        private static void PositionLabelsLeft(ref UIElementLeftShift leftElementShift, ref UIElementTopShift topElementShift, double actualTiltAngle)
        {
            leftElementShift = UIElementLeftShift.LeftShift;

            if (actualTiltAngle >= 45 && actualTiltAngle < 315)
                topElementShift = UIElementTopShift.TopHalfShift;
        }

        /// <summary>
        /// Positions the label front.
        /// </summary>
        /// <param name="isOpposed">Is Opposed Indication</param>
        /// <param name="leftElementShift">The Left Element Shift </param>
        /// <param name="topElementShift">The Top Element Shift</param>
        /// <param name="actualTiltAngle">The Actual Tilt Angle</param>
        private static void PositionLabelsFront(bool isOpposed, ref UIElementLeftShift leftElementShift, ref UIElementTopShift topElementShift, double actualTiltAngle)
        {
            leftElementShift = UIElementLeftShift.LeftHalfShift;

            if (isOpposed)
            {
                if (actualTiltAngle >= 45 && actualTiltAngle < 315)
                    topElementShift = UIElementTopShift.TopShift;
            }
        }

#endregion

#region Private Methods

        /// <summary>
        /// Calcuales the point.
        /// </summary>
        /// <param name="value">The Value</param>
        /// <returns>Returns the calculated point.</returns>
        private double CalculatePoint(double value)
        {
            return Axis.GetActualPlotOffsetStart() + Math.Round(Axis.RenderedRect.Width * Axis.ValueToCoefficient(value));
        }

#endregion

#endregion
    }

    /// <summary>
    /// Represents the <see cref="VerticalLabelLayout"/> class.
    /// </summary>
    internal class VerticalLabelLayout : AxisLabelLayout
    {
#region Fields

        private bool isOpposed;
        private double maxWidth;

#endregion
        
#region Constructor

        /// <summary>
        /// Called when instance created for <see cref="VerticalLabelLayout"/>.
        /// </summary>
        /// <param name="axis">Used to indicates corresponding axis.</param>
        /// <param name="elements">Uesd to indicates elements to be layout.</param>
        public VerticalLabelLayout(ChartAxis axis, List<UIElement> elements) : base(axis, elements)
        {
        }

#endregion
        
#region Methods

#region Public Methods

        /// <summary>
        /// Method declaration for Measure.
        /// </summary>
        /// <param name="availableSize">The Available Size</param>
        /// <returns>Returns the desired height</returns>
        internal override Size Measure(Size availableSize)
        {
            double desiredWidth = 0;
            if (Axis != null && Children.Count > 0)
            {
                AvailableSize = availableSize;
                base.Measure(availableSize);
                CalcBounds(availableSize.Height - Axis.GetActualPlotOffset());
                desiredWidth = LayoutElements();
                desiredWidth = Math.Max(desiredWidth, Axis.LabelExtent);
                desiredWidth += ((Margin.Left + Margin.Right) * RectssByRowsAndCols.Count);
                return new Size(desiredWidth, availableSize.Height);
            }
            return new Size(0, availableSize.Height);
        }

        /// <summary>
        /// Method declaration for Arrange.
        /// </summary>
        /// <param name="finalSize">The Final Size.</param>
        internal override void Arrange(Size finalSize)
        {
            if (RectssByRowsAndCols == null)
                return;

            isOpposed = AxisLabelLayout.IsOpposed(Axis, Axis.OpposedPosition);
            bool needToRotate = !double.IsNaN(Axis.LabelRotation) && Axis.LabelRotation != 0.0;
            double left = isOpposed ? Margin.Left : finalSize.Width - Margin.Right;

            int row = 0;

            maxWidth = RectssByRowsAndCols.Select(dictionary => dictionary.Values.Max(rect => rect.Width)).FirstOrDefault();
            maxWidth = RectssByRowsAndCols.Count > 1 ? maxWidth : maxWidth + BorderPadding;

            foreach (Dictionary<int, Rect> dictionary in RectssByRowsAndCols)
            {
                foreach (KeyValuePair<int, Rect> keyValue in dictionary)
                {
                    UIElement element = Children[keyValue.Key];

                    double actualLeft = isOpposed ? left : left - ComputedSizes[keyValue.Key].Width;

                    double actualTop = keyValue.Value.Top + Axis.GetActualPlotOffsetEnd();

                    if (needToRotate)
                    {
                        actualLeft += (ComputedSizes[keyValue.Key].Width - DesiredSizes[keyValue.Key].Width) / 2;
                        actualTop += (ComputedSizes[keyValue.Key].Height - DesiredSizes[keyValue.Key].Height) / 2;
                    }

                    Canvas.SetLeft(element, actualLeft);
                    Canvas.SetTop(element, actualTop);
                }

                if (isOpposed)
                    left += (dictionary.Values.Max(rect => rect.Width) + Margin.Left);
                else
                    left -= (dictionary.Values.Max(rect => rect.Width) + Margin.Right);
                row++;
            }
        }

#endregion
        
#region Protected Methods

        /// <summary>
        /// Returns desired width
        /// </summary>
        /// <returns>Returns the total width of the rows and columns collection</returns>
        internal override double LayoutElements()
        {
            if (Axis.GetLabelIntersection() == AxisLabelsIntersectAction.Wrap)
            {
                CalculateWrapLabelRect();
                CalcBounds(AvailableSize.Height - Axis.GetActualPlotOffset());
            }

            base.LayoutElements();

            return RectssByRowsAndCols.Sum(dictionary => dictionary.Values.Max(rect => rect.Width));
        }

        /// <summary>
        /// Calculates the bounds.
        /// </summary>
        /// <param name="availableHeight">The Available Height</param>
        internal override void CalcBounds(double availableHeight)
        {
            RectssByRowsAndCols = new List<Dictionary<int, Rect>>();
            RectssByRowsAndCols.Add(new Dictionary<int, Rect>());

            for (int j = 0; j < Children.Count; j++)
            {
                double position = 0d;
                double coeff;
                var axis = Axis as NumericalAxis;
                
                {
                    coeff = Axis.ValueToCoefficient(Axis.VisibleLabels[j].Position);
                    position = ((1 - coeff) * availableHeight) - (ComputedSizes[j].Height / 2);
                }

                var labelAlignment = LabelAlignment.Center;
                if (labelAlignment == LabelAlignment.Far)
                {
                    position += ComputedSizes[j].Height / 2;
                }
                else if (labelAlignment == LabelAlignment.Near)
                {
                    position -= ComputedSizes[j].Height / 2;
                }

                RectssByRowsAndCols[0].Add(j, new Rect(new Point(0, position), ComputedSizes[j]));
            }

            if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Shift)
            {
                if (RectssByRowsAndCols[0][0].Bottom > availableHeight)
                {
                    double position = availableHeight - ComputedSizes[0].Height;
                    RectssByRowsAndCols[0][0] = new Rect(0, position, ComputedSizes[0].Width, ComputedSizes[0].Height);
                }

                int index = Children.Count - 1;
                if (RectssByRowsAndCols[0][index].Top < 0)
                {
                    RectssByRowsAndCols[0][index] = new Rect(0, 0, ComputedSizes[index].Width,
                                                             ComputedSizes[index].Height);
                }
            }
            else if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Hide)
            {
                if (RectssByRowsAndCols[0][0].Bottom > availableHeight)
                {
                    RectssByRowsAndCols[0][0] = new Rect(0, 0, 0, 0);
                    Children[0].Visibility = Visibility.Collapsed;
                }

                int index = Children.Count - 1;
                if (RectssByRowsAndCols[0][index].Top < 0)
                {
                    RectssByRowsAndCols[0][index] = new Rect(0, 0, 0, 0);
                    Children[index].Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Calculates the actual plot offset.
        /// </summary>
        /// <param name="availableSize">The Available Size</param>
        internal override void CalculateActualPlotOffset(Size availableSize)
        {
            if (Axis.EdgeLabelsDrawingMode == EdgeLabelsDrawingMode.Fit)
            {
                double coeff = Axis.ValueToCoefficient(Axis.VisibleLabels[0].Position);
                double position = ((1 - coeff) * availableSize.Height) - (ComputedSizes[0].Height / 2);
                double firstElementHeight = 0;
                double lastElementHeight = 0;
                if ((position + ComputedSizes[0].Height / 2) > availableSize.Height)
                {
                    firstElementHeight = ComputedSizes[0].Height;
                }

                int index = Children.Count - 1;
                coeff = Axis.ValueToCoefficient(Axis.VisibleLabels[index].Position);
                position = ((1 - coeff) * availableSize.Height) - (ComputedSizes[index].Height / 2);

                if ((position - ComputedSizes[index].Height / 2) > availableSize.Height)
                {
                    lastElementHeight = ComputedSizes[index].Height;
                }

                double offset = Math.Max(firstElementHeight / 2, lastElementHeight / 2);
                Axis.ActualPlotOffset = Math.Max(offset, 0);
            }
            else
            {
                base.CalculateActualPlotOffset(availableSize);
            }
        }

#endregion

#region Private Methods

        /// <summary>
        /// Calculates the corresponding screen co-ordinate value.
        /// </summary>
        /// <param name="value">The Value</param>
        /// <returns>Returns corresponding screen co-ordinate value</returns>
        private double CalculatePoint(double value)
        {
            return  Axis.GetActualPlotOffsetEnd() + Math.Round(Axis.RenderedRect.Height * (1 - Axis.ValueToCoefficient(value)));
        }

#endregion
        
#endregion
    }
}
