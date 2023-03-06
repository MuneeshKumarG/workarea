using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    internal class CartesianAxisRenderer
    {
        #region Fields
        private ChartAxis chartAxis;

        internal List<ILayoutCalculator> LayoutCalculators { get; set; }
        #endregion

        #region Constructor
        public CartesianAxisRenderer(ChartAxis axis)
        {
            LayoutCalculators = new List<ILayoutCalculator>();
            chartAxis = axis;
        }
        #endregion

        #region Internal Methods
        internal SizeF ComputeDesiredSize(SizeF availableSize)
        {
            double width = 0;
            double height = 0;
            ChartAxisTitle title = chartAxis.Title;
            double horizontalPadding = 0;
            double verticalPadding = 0;

            SizeF size;
            foreach (ILayoutCalculator layout in LayoutCalculators)
            {
                layout.SetLeft(0);
                layout.SetTop(0);
                layout.Measure(availableSize);
                var desiredSize = layout.GetDesiredSize();

                if (layout is CartesianAxisLabelsRenderer && chartAxis.LabelStyle.LabelsPosition == AxisElementPosition.Inside)
                {
                    horizontalPadding += desiredSize.Width;
                    verticalPadding += desiredSize.Height;
                }

                if (layout is CartesianAxisElementRenderer && chartAxis.TickPosition == AxisElementPosition.Inside)
                {
                    horizontalPadding += desiredSize.Width - chartAxis.AxisLineStyle.StrokeWidth;
                    verticalPadding += desiredSize.Height - chartAxis.AxisLineStyle.StrokeWidth;
                }

                width += desiredSize.Width;
                height += desiredSize.Height;
            }

            if (title != null && !string.IsNullOrEmpty(title.Text))
            {
                title.Measure();
                SizeF titleSize = title.GetDesiredSize();
                width += titleSize.Width;
                height += titleSize.Height;
            }

            if (chartAxis.IsVertical)
            {
                chartAxis.InsidePadding = horizontalPadding;
                size = new Size(width, availableSize.Height);
            }
            else
            {
                chartAxis.InsidePadding = verticalPadding;
                size = new Size(availableSize.Width, height);
            }

            return size;
        }

        internal void UpdateRendererVisible(bool visible)
        {
            foreach (ILayoutCalculator layout in LayoutCalculators)
            {
                if (!(layout is CartesianAxisLabelsRenderer))
                {
                    layout.IsVisible = visible;
                }
            }
        }

        internal void OnDraw(ICanvas canvas)
        {
            foreach (ILayoutCalculator layout in LayoutCalculators)
            {
                if (layout.IsVisible)
                {
                    canvas.SaveState();
                    canvas.Translate((float)layout.GetLeft(), (float)layout.GetTop());
                    layout.OnDraw(canvas, layout.GetDesiredSize());
                    canvas.RestoreState();
                }
            }

            ChartAxisTitle title = chartAxis.Title;
            if (title == null || string.IsNullOrEmpty(title.Text))
            {
                return;
            }

            title.Draw(canvas);
        }

        internal void Layout(SizeF size)
        {
            ILayoutCalculator labelsRenderer = LayoutCalculators[0];
            ILayoutCalculator elementsRender = LayoutCalculators[1];

            List<object> elements = new List<object>();
            List<SizeF> sizes = new List<SizeF>();
            bool isVertical = chartAxis.IsVertical;
            bool isInversed = chartAxis.IsOpposed() ^ isVertical;

            if (chartAxis.TickPosition == AxisElementPosition.Inside)
            {
                elements.Insert(0, elementsRender);
                sizes.Insert(0, elementsRender.GetDesiredSize());
            }
            else
            {
                elements.Add(elementsRender);
                sizes.Add(elementsRender.GetDesiredSize());
            }

            if (chartAxis.LabelStyle.LabelsPosition == AxisElementPosition.Inside)
            {
                elements.Insert(0, labelsRenderer);
                sizes.Insert(0, labelsRenderer.GetDesiredSize());
            }
            else
            {
                elements.Add(labelsRenderer);
                sizes.Add(labelsRenderer.GetDesiredSize());
            }

            ChartAxisTitle title = chartAxis.Title;

            if (title != null && !string.IsNullOrEmpty(title.Text))
            {
                elements.Add(title);
                sizes.Add(title.GetDesiredSize());
            }

            if (isInversed)
            {
                elements.Reverse();
                sizes.Reverse();
            }

            float currentPosition = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                object element = elements[i];

                if (isVertical)
                {
                    if (element == title)
                    {
                        title.Top = size.Height / 2;

#if !ANDROID
                        if (chartAxis.IsOpposed())
                        {
                            currentPosition += sizes[i].Width;
                        }
#endif

                        title.Left = currentPosition;
                    }
                    else
                    {
                        ILayoutCalculator layout = (ILayoutCalculator)element;
                        layout.SetLeft(currentPosition);
                    }

                    currentPosition += sizes[i].Width;
                }
                else
                {
                    if (element == title)
                    {
                        title.Left = size.Width / 2;
                        title.Top = currentPosition;
                    }
                    else
                    {
                        ILayoutCalculator layout = (ILayoutCalculator)element;
                        layout.SetTop(currentPosition);
                    }

                    currentPosition += sizes[i].Height;
                }
            }
        }
        #endregion
    }
}
