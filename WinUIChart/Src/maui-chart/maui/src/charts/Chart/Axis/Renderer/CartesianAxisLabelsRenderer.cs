using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    internal class CartesianAxisLabelsRenderer : ILayoutCalculator
    {
        #region Fields
        private SizeF desiredSize;

        private double left, top;

        private ChartAxis chartAxis;

        internal AxisLabelLayout? LabelLayout
        {
            get;
            set;
        }

        bool ILayoutCalculator.IsVisible { get; set; } = true;

        #endregion

        #region Constructor
        public CartesianAxisLabelsRenderer(ChartAxis axis)
        {
            chartAxis = axis;
        }
        #endregion

        #region public Methods
        public void OnDraw(ICanvas drawing, Size finalSize)
        {
            if (LabelLayout != null)
            {
                LabelLayout.OnDraw(drawing, finalSize);
            }
        }

        public double GetLeft()
        {
            return left;
        }

        public void SetLeft(double _left)
        {
            left = _left;
        }

        public double GetTop()
        {
            return top;
        }

        public void SetTop(double _top)
        {
            top = _top;
        }

        public Size GetDesiredSize()
        {
            return desiredSize;
        }

        public Size Measure(Size availableSize)
        {
            LabelLayout = AxisLabelLayout.CreateAxisLayout(chartAxis);
            desiredSize = LabelLayout.Measure(availableSize);
            return desiredSize;
        }
        #endregion
    }
}
