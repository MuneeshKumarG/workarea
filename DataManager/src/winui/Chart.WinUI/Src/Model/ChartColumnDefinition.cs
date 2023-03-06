using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents a column definition.
    /// </summary>
    /// <remarks>
    /// The width of the row can be defined either in terms of fixed pixels units mode or in auto adjust mode, by using <see cref="ChartColumnDefinition.Unit"/> property.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805: Do not initialize unnecessarily")]
    internal class ChartColumnDefinition : DependencyObject
    {
        #region Dependency Property Registration

        /// <summary>
        /// The DependencyProperty for <see cref="Width"/> property.
        /// </summary>
        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register(
                "Width",
                typeof(double), 
                typeof(ChartColumnDefinition),
                new PropertyMetadata(1d, OnColumnPropertyChanged));
        
        /// <summary>
        ///  The DependencyProperty for <see cref="Unit"/> property.
        /// </summary>
        public static readonly DependencyProperty UnitProperty =
            DependencyProperty.Register(
                "Unit",
                typeof(ChartUnitType),
                typeof(ChartColumnDefinition),
                new PropertyMetadata(ChartUnitType.Star, OnColumnPropertyChanged));
        
        /// <summary>
        /// The DependencyProperty for <see cref="BorderThickness"/> property.
        /// </summary>
        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(
                "BorderThickness", 
                typeof(double),
                typeof(
                ChartColumnDefinition),
                new PropertyMetadata(0d));
        
        /// <summary>
        /// The DependencyProperty for <see cref="BorderStroke"/> property.
        /// </summary>
        public static readonly DependencyProperty BorderStrokeProperty =
            DependencyProperty.Register(
                "BorderStroke", 
                typeof(Brush),
                typeof(ChartColumnDefinition),
                new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        #endregion

        #region Fields

        #region Internal Fields

        internal Line BorderLine;

        #endregion

        #region Private Fields

        private List<ChartAxis> axis;

        private double computedWidth = 0;

        private double computedLeft = 0;

        #endregion
        
        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Called when instance created for ChartColumnDefinition
        /// </summary>
        public ChartColumnDefinition()
        {
            axis = new List<ChartAxis>();
        }

        /// <summary>
        /// Gets or sets the width of this column.
        /// </summary>
        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets unit of the value to be specified for row width.
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
        /// Gets or sets the thickness of the column border.
        /// </summary>
        public double BorderThickness
        {
            get { return (double)GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }
        
        /// <summary>
        /// Gets or sets the border stroke.
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

        internal double ComputedWidth
        {
            get { return computedWidth; }
            set { computedWidth = value; }
        }
        
        internal double ComputedLeft
        {
            get { return computedLeft; }
            set { computedLeft = value; }
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        internal void Measure(Size size, List<double> nearSizes, List<double> farSizes)
        {
            int nearIndex = 0;
            int farIndex = 0;

            bool isOpposedFirstElement = true;
            bool isFirstElement = true;
            double innerPadding = 0;
            double axisWidth = 0;

            foreach (ChartAxis content in axis)
            {
                if (content != null)
                {
                    int columnSpan = content.Area != null ? content.Area.GetActualColumnSpan(content) : 0;
                    axisWidth = CalcColumnSpanAxisWidth(size.Width, content, columnSpan);

                    if (content.Area != null && content.Area.GetActualColumn(content) == content.Area.ColumnDefinitions.IndexOf(this))
                        content.ComputeDesiredSize(new Size(axisWidth, size.Height));

                    if (content.OpposedPosition)
                    {
                        innerPadding = isOpposedFirstElement ? content.InsidePadding : 0;
                        if (farSizes.Count <= farIndex)
                        {
                            farSizes.Add(content.ComputedDesiredSize.Height - innerPadding);
                        }
                        else if (farSizes[farIndex] < (content.ComputedDesiredSize.Height - innerPadding))
                        {
                            farSizes[farIndex] = content.ComputedDesiredSize.Height - innerPadding;
                        }
                        farIndex++;
                        isOpposedFirstElement = false;
                    }
                    else
                    {
                        innerPadding = isFirstElement ? content.InsidePadding : 0;
                        if (nearSizes.Count <= nearIndex)
                        {
                            nearSizes.Add(content.ComputedDesiredSize.Height - innerPadding);
                        }
                        else if (nearSizes[nearIndex] < (content.ComputedDesiredSize.Height - innerPadding))
                        {
                            nearSizes[nearIndex] = content.ComputedDesiredSize.Height - innerPadding;
                        }
                        nearIndex++;
                        isFirstElement = false;
                    }
                }
            }
        }

        internal void UpdateArrangeRect(
            double left,
            double width,
            double areaHeight,
            List<double> nearSizes,
            List<double> farSizes)
        {
            int nearIndex = 0;
            int farIndex = 0;

            //To get the 0 left when the depth axis is set.
            bool isOpposedFirstElement = true;
            bool isFirstElement = true;
            double nearTotalSize = nearSizes.Sum();
            double farTotalSize = farSizes.Sum();
            double innerPadding = 0;
            double axisWidth = 0;
            int actualColumnIndex = 0;
            int elementColumnIndex = 0;

            for (int i = 0; i < Axis.Count; i++)
            {
                ChartAxis element = this.Axis[i];
                if (element != null)
                {
                    //Set ColumnSpan width value
                    if (element.Area != null)
                    {
                        axisWidth = CalcColumnSpanAxisWidth(width, element, element.Area.GetActualColumnSpan(element));
                        actualColumnIndex = element.Area.ColumnDefinitions.IndexOf(this);
                        elementColumnIndex = element.Area.GetActualColumn(element);
                    }
                    Size desiredSize = element.ComputedDesiredSize;
                    try
                    {
                        if (element.OpposedPosition)
                        {
                            innerPadding = isOpposedFirstElement ? element.InsidePadding : 0;
                            if (elementColumnIndex == actualColumnIndex)
                                element.ArrangeRect = new Rect(
                                    left,
                                    (farTotalSize - desiredSize.Height) + innerPadding,
                                    axisWidth,
                                    desiredSize.Height);
                            element.Measure(new Size(element.ArrangeRect.Width, element.ArrangeRect.Height));
                            farTotalSize -= farSizes[farIndex];
                            farIndex++;
                            isOpposedFirstElement = false;
                        }
                        else
                        {
                            innerPadding = isFirstElement ? element.InsidePadding : 0;
                            if (elementColumnIndex == actualColumnIndex)
                                element.ArrangeRect = new Rect(
                                    left,
                                    (areaHeight - nearTotalSize) - innerPadding,
                                    axisWidth,
                                    desiredSize.Height);
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

        private static void OnColumnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var column = d as ChartColumnDefinition;

            if (column != null && column.Axis != null && column.Axis.Count > 0)
            {
                var area = column.Axis[0].Area;
                area.ScheduleUpdate();
            }
        }

        #endregion

        #region Private Methods

        //Calculate ColumnSpan width value
        private double CalcColumnSpanAxisWidth(double width, ChartAxis axis, int columnSpan)
        {
            if (axis.Area != null)
            {
                int column = axis.Area.GetActualColumn(axis);
                if (axis.Area != null && columnSpan > 1 && column == axis.Area.ColumnDefinitions.IndexOf(this))
                {
                    var cols = axis.Area.ColumnDefinitions;
                    int j = cols.IndexOf(this), i = 0;
                    width = 0;
                    for (; j < cols.Count; j++)
                    {
                        if (i < columnSpan)
                        {
                            width += cols[j].ComputedWidth;
                            i++;
                        }
                    }
                }
            }

            return width;
        }

        private void RenderBorderLine()
        {
            if (BorderLine == null)
            {
                BorderLine = new Line();
                BindBorder(BorderLine);
            }
            if (Axis != null && Axis.Count > 0)
            {
                ChartAxis element = this.Axis.FirstOrDefault();
                if (element.Area != null)
                {
                    BorderLine.X1 = BorderLine.X2 = element.ArrangeRect.Left - element.Area.SeriesClipRect.Left;
                    BorderLine.Y1 = 0;
                    BorderLine.Y2 = element.Area.SeriesClipRect.Height;
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
