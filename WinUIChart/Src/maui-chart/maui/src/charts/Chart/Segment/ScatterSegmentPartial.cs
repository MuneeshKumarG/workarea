using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Charts
{
    public partial class ScatterSegment
    {
        #region Methods

        internal static void DrawShape(ICanvas canvas, RectF rect, ShapeType shapeType, bool hasBorder, bool isSaveState)
        {
            float x = rect.X;
            float y = rect.Y;
            float radius = rect.Width / 2;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);
            float midHeight = y + (rect.Height / 2);
            var center = rect.Center;

            switch (shapeType)
            {
                case ShapeType.Rectangle:
                    DrawRectangle(canvas, rect, hasBorder, isSaveState);
                    break;
                case ShapeType.Circle:
                    DrawCircle(canvas, center, radius, hasBorder, isSaveState);
                    break;
                case ShapeType.HorizontalLine:
                    DrawLine(canvas, new PointF(x, midHeight), new PointF(width, midHeight), isSaveState);
                    break;
                case ShapeType.VerticalLine:
                    DrawLine(canvas, new PointF(midWidth, y), new PointF(midWidth, height), isSaveState);
                    break;
                case ShapeType.Triangle:
                    canvas.DrawTriangle(rect, hasBorder);
                    break;
                case ShapeType.InvertedTriangle:
                    canvas.DrawInverseTriangle(rect, hasBorder);
                    break;
                case ShapeType.Cross:
                    canvas.DrawCross(rect, hasBorder);
                    break;
                case ShapeType.Plus:
                    canvas.DrawPlus(rect, hasBorder);
                    break;
                case ShapeType.Diamond:
                    canvas.DrawDiamond(rect, hasBorder);
                    break;
                case ShapeType.Hexagon:
                    canvas.DrawHexagon(rect, hasBorder);
                    break;
                case ShapeType.Pentagon:
                    canvas.DrawPentagon(rect, hasBorder);
                    break;
            }
        }

        private static void DrawLine(ICanvas canvas, PointF start, PointF end, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.SaveState();
            }

            canvas.DrawLine(start, end);

            if (isSaveState)
            {
                canvas.RestoreState();
            }
        }

        private static void DrawCircle(ICanvas canvas, PointF center, float radius, bool hasBorder, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.SaveState();
            }

            canvas.FillCircle(center, radius);

            if (hasBorder)
            {
                canvas.DrawCircle(center, radius);
            }

            if (isSaveState)
            {
                canvas.RestoreState();
            }
        }

        private static void DrawRectangle(ICanvas canvas, RectF rect, bool hasBorder, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.SaveState();
            }

            canvas.FillRectangle(rect);

            if (hasBorder)
            {
                canvas.DrawRectangle(rect);
            }

            if (isSaveState)
            {
                canvas.RestoreState();
            }
        }

        #endregion
    }
}
