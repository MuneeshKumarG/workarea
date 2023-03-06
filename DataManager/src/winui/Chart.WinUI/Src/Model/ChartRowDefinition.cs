using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a row definition.
    /// </summary>
    /// <remarks>
    /// The height of the row can be defined either in terms of fixed pixels units mode or auto adjust mode, by using <see cref="ChartRowDefinition.Unit"/> property.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    internal class ChartRowDefinition : DependencyObject
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="Height"/> property.
        /// </summary>
        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register(
                "Height", 
                typeof(double),
                typeof(ChartRowDefinition),
                new PropertyMetadata(1d, OnRowPropertyChanged));

        /// <summary>
        ///  The DependencyProperty for <see cref="Unit"/> property.
        /// </summary>
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register(
                "Unit",
                typeof(ChartUnitType), 
                typeof(ChartRowDefinition),
                new PropertyMetadata(ChartUnitType.Star, OnRowPropertyChanged));

        /// <summary>
        ///  The DependencyProperty for <see cref="BorderThickness"/> property.
        /// </summary>
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(
                "BorderThickness", 
                typeof(double),
                typeof(ChartRowDefinition),
                new PropertyMetadata(0d));

        /// <summary>
        ///  The DependencyProperty for <see cref="BorderStroke"/> property.
        /// </summary>
        public static readonly DependencyProperty BorderStrokeProperty =
            DependencyProperty.Register(
                "BorderStroke", 
                typeof(Brush), 
                typeof(ChartRowDefinition),
                new PropertyMetadata(new SolidColorBrush(Colors.Red)));
        #endregion

        #region Fields

        #region Internal Fields

        internal Line BorderLine;

        #endregion

        #region Private Fields

        private List<ChartAxis> axis;

        private double computedHeight = 0;

        private double computedTop = 0;

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// Called when instance created for ChartRowdefinitions
        /// </summary>
        public ChartRowDefinition()
        {
            axis = new List<ChartAxis>();
        }

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets RowTap property
        /// </summary>
        public double RowTop
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets height of this row.
        /// </summary>
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets unit of the value to be specified for row height.
        /// </summary>
        /// <value>
        /// <see cref="ChartUnitType"/>
        /// </value>
        public ChartUnitType Unit
        {
            get { return (ChartUnitType)GetValue(UnitProperty); }
            set { SetValue(UnitProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness of the border.
        /// </summary>
        public double BorderThickness
        {
            get { return (double)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush for the border of the row.
        /// </summary>
        /// <value>
        /// The <see cref="Brush"/> value.
        /// </value>
        public Brush BorderStroke
        {
            get { return (Brush)GetValue(BorderStrokeProperty); }
            set { SetValue(BorderStrokeProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal double ComputedHeight
        {
            get { return computedHeight; }
            set { computedHeight = value; }
        }


        internal double ComputedTop
        {
            get { return computedTop; }
            set { computedTop = value; }
        }

        internal List<ChartAxis> Axis
        {
            get
            {
                return axis;
            }
            set
            {
                axis = value;
            }
        }

        #endregion

        #endregion

        #region Methods

        #region Internal Methods

        internal void Measure(Size size, List<double> nearSizes, List<double> farSizes, bool isFirstLayout)
        {
            int nearIndex = 0;
            int farIndex = 0;
            bool isOpposedFirstElement = true;
            bool isFirstElement = true;
            double innerPadding = 0;
            double axisHeight = 0, top = 0;

            foreach (ChartAxis content in axis)
            {
                if (content != null)
                {

                    if (content.Area != null)
                    {
                        CalcRowSpanAxisWidthandTop(top, content.Area.GetActualRowSpan(content), size.Height, content, out top, out axisHeight);
                        if (content.Area.GetActualRow(content) == content.Area.RowDefinitions.IndexOf(this))
                            content.ComputeDesiredSize(new Size(size.Width, axisHeight));
                    }

                    if (content.OpposedPosition)
                    {
                        innerPadding = isOpposedFirstElement ? content.InsidePadding : 0;
                        if (farSizes.Count <= farIndex)
                        {
                            farSizes.Add(content.ComputedDesiredSize.Width - innerPadding);
                        }
                        else if (farSizes[farIndex] < (content.ComputedDesiredSize.Width - innerPadding))
                        {
                            farSizes[farIndex] = content.ComputedDesiredSize.Width - innerPadding;
                        }
                        farIndex++;
                        isOpposedFirstElement = false;
                    }
                    else
                    {
                        innerPadding = isFirstElement ? content.InsidePadding : 0;
                        if (nearSizes.Count <= nearIndex)
                        {
                            nearSizes.Add(content.ComputedDesiredSize.Width - innerPadding);
                        }
                        else if (nearSizes[nearIndex] < (content.ComputedDesiredSize.Width - innerPadding))
                        {
                            nearSizes[nearIndex] = content.ComputedDesiredSize.Width - innerPadding;
                        }
                        nearIndex++;
                        isFirstElement = false;
                    }
                }
            }
        }

        internal void UpdateArrangeRect(double top, double height, double areaWidth, List<double> nearSizes, List<double> farSizes)
        {
            int nearIndex = 0;
            int farIndex = 0;
            bool isOpposedFirstElement = true;
            bool isFirstElement = true;
            double nearTotalSize = nearSizes.Sum();
            double farTotalSize = farSizes.Sum();
            double innerPadding = 0;
            double axisHeight = 0;
            double axisTop = 0;
            int actualRowIndex = 0, elementRowIndex = 0;

            for (int i = 0; i < Axis.Count; i++)
            {
                ChartAxis element = Axis[i];
                if (element != null)
                {
                    //Set RowSpan height and top value
                    if (element.Area != null)
                    {
                        elementRowIndex = element.Area.GetActualRow(element);
                        actualRowIndex = element.Area.RowDefinitions.IndexOf(this);
                        CalcRowSpanAxisWidthandTop(top, element.Area.GetActualRowSpan(element), height, element, out axisTop, out axisHeight);
                    }

                    Size desiredSize = element.ComputedDesiredSize;
                    try
                    {
                        if (element.OpposedPosition)
                        {
                            innerPadding = isOpposedFirstElement ? element.InsidePadding : 0;
                            if (elementRowIndex == actualRowIndex)
                                element.ArrangeRect = new Rect(
                                    (areaWidth - farTotalSize) - innerPadding,
                                    axisTop,
                                    desiredSize.Width,
                                    axisHeight);
                            element.Measure(new Size(element.ArrangeRect.Width, element.ArrangeRect.Height));
                            farTotalSize -= farSizes[farIndex];
                            farIndex++;
                            isOpposedFirstElement = false;
                        }
                        else
                        {
                            innerPadding = isFirstElement ? element.InsidePadding : 0;
                            if (elementRowIndex == actualRowIndex)
                                element.ArrangeRect = new Rect(
                                    (nearTotalSize - desiredSize.Width) + innerPadding,
                                    axisTop,
                                    desiredSize.Width,
                                    axisHeight);
                            element.Measure(new Size(element.ArrangeRect.Width, element.ArrangeRect.Height));
                            nearTotalSize -= nearSizes[nearIndex];
                            nearIndex++;
                            isFirstElement = false;
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        internal void Arrange()
        {
            foreach (ChartAxis chartAxis in Axis)
            {
                Canvas.SetLeft(chartAxis, chartAxis.ArrangeRect.Left);
                Canvas.SetTop(chartAxis, chartAxis.ArrangeRect.Top);
            }

            RenderBorderLine();
        }

        internal void Dispose()
        {
            if (Axis != null)
            {
                Axis.Clear();
                Axis = null;
            }
        }

        #endregion

        #region Private Static Methods
        
        private static void OnRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var row = d as ChartRowDefinition;

            if(row != null && row.Axis != null && row.Axis.Count > 0)
            {
                var area = row.Axis[0].Area;
                area.ScheduleUpdate();
            }
        }

        #endregion

        #region Private Methods

        //Calculate row span height and top value
        private void CalcRowSpanAxisWidthandTop(double oldTop, int rowSpan, double oldHeight, ChartAxis axis, out double newTop, out double newHeight)
        {
            int row = axis.Area.GetActualRow(axis);
            if (axis.Area != null && rowSpan > 1 && row == axis.Area.RowDefinitions.IndexOf(this))
            {
                var rows = axis.Area.RowDefinitions;
                int j = rows.IndexOf(this), i = 0;
                newTop = 0;
                newHeight = 0;
                for (; j < rows.Count; j++)
                {
                    if (i < rowSpan)
                    {
                        newHeight += rows[j].computedHeight;
                        newTop = rows[j].ComputedTop;
                        i++;
                    }

                }
            }
            else
            {
                newTop = oldTop;
                newHeight = oldHeight;
            }
        }

        private void RenderBorderLine()
        {
            if (BorderLine == null)
            {
                BorderLine = new Line();
                BindBorder(BorderLine);
            }
            if (Axis != null && this.Axis.Count > 0)
            {
                ChartAxis element = this.Axis.FirstOrDefault();
                if (element.Area != null)
                {
                    BorderLine.X1 = 0;
                    BorderLine.X2 = element.Area.SeriesClipRect.Width;
                    BorderLine.Y1 = BorderLine.Y2 = element.ArrangeRect.Top - element.Area.SeriesClipRect.Top;
                }
            }
        }

        private void BindBorder(UIElement element)
        {
            Binding binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("BorderStroke");
            BindingOperations.SetBinding(element, Line.StrokeProperty, binding);

            binding = new Binding();
            binding.Source = this;
            binding.Path = new PropertyPath("BorderThickness");
            BindingOperations.SetBinding(element, Line.StrokeThicknessProperty, binding);
        }

        #endregion

        #endregion
    }
}
