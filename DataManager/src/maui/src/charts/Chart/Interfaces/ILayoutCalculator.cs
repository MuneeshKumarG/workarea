
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{
    internal interface ILayoutCalculator
    {
        bool IsVisible { get; set; }

        void OnDraw(ICanvas canvas, Size finalSize);

        double GetLeft();

        void SetLeft(double left);

        double GetTop();

        void SetTop(double top);

        Size GetDesiredSize();

        Size Measure(Size availableSize);
    }

    internal interface IAxisLayout
    {
        Size Measure(Size availableSize);

        void OnDraw(ICanvas canvas);
    }
}
