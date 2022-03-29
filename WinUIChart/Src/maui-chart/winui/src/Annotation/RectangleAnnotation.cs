using System.Windows;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Shapes;

namespace Syncfusion.UI.Xaml.Charts
{
    internal class RectangleAnnotation : SolidShapeAnnotation
    {
        #region Methods

        #region Internal Override Methods

        internal override UIElement CreateAnnotation()
        {
            if (AnnotationElement != null && AnnotationElement.Children.Count == 0)
            {
                shape = new Rectangle();
                shape.Tag = this;
                SetBindings();
                AnnotationElement.Children.Add(shape);
                TextElementCanvas.Children.Add(TextElement);
                AnnotationElement.Children.Add(TextElementCanvas);
            }

            return AnnotationElement;
        }

        #endregion

        #endregion
    }
}
