using Syncfusion.Maui.Graphics.Internals;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using Microsoft.Maui;
using System;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// This class represents the default shapes view drawing.
    /// </summary>
    internal class SfShapeView : DrawableView
    {
        #region Bindable Properties

        /// <summary>
        /// Gets or sets the shape type. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty ShapeTypeProperty = BindableProperty.Create(nameof(ShapeType), typeof(ShapeType), typeof(SfShapeView), ShapeType.Circle, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets the shape color. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty IconBrushProperty = BindableProperty.Create(nameof(IconBrush), typeof(Brush), typeof(SfShapeView), null, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets the shape stroke color. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Color), typeof(SfShapeView), null, BindingMode.Default, null, OnPropertyChanged);

        /// <summary>
        /// Gets or sets the shape stroke width value. This is a bindable property.
        /// </summary>
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(float), typeof(SfShapeView), 1f, BindingMode.Default, null, OnPropertyChanged);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the shape type.
        /// </summary>
        public ShapeType ShapeType
        {
            get { return (ShapeType)GetValue(ShapeTypeProperty); }
            set { SetValue(ShapeTypeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the shape view color.
        /// </summary>
        public Brush IconBrush
        {
            get { return (Brush)GetValue(IconBrushProperty); }
            set { SetValue(IconBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the shape stroke Color.
        /// </summary>
        public Color Stroke
        {
            get { return (Color)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the shape stroke width.
        /// </summary>
        public float StrokeWidth
        {
            get { return (float)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SfShapeView"/> class.
        /// </summary>
        public SfShapeView()
        {
        }

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Draw the shape based on the rect.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="dirtyRect"></param>
        public override void Draw(ICanvas canvas, RectangleF dirtyRect)
        {
            base.Draw(canvas, dirtyRect);
            if (dirtyRect.Width > 0 && dirtyRect.Height > 0 && ShapeType != ShapeType.Custom)
            {
                DrawShape(canvas, dirtyRect.Height, dirtyRect.Width, ShapeType, IconBrush, IconBrush);
            }
        }

        /// <summary>
        /// Draw the shape based on the shape type.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="iconHeight"></param>
        /// <param name="iconWidth"></param>
        /// <param name="shapeType"></param>
        /// <param name="strokeColor"></param>
        /// <param name="fillColor"></param>
        public virtual void DrawShape(ICanvas canvas, float iconHeight, float iconWidth, ShapeType shapeType, Brush strokeColor, Brush fillColor)
        {
            canvas.StrokeColor = (strokeColor as SolidColorBrush)?.Color;
            canvas.StrokeSize = StrokeWidth;

            var rect = new Rectangle(0, 0, iconWidth, iconHeight);
            canvas.SetFillPaint(fillColor, rect);
            DrawShape(canvas, rect, shapeType, fillColor, false, true);
        }

        /// <summary>
        /// Method used to draw the shape.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="rect"></param>
        /// <param name="shapeType"></param>
        /// <param name="fillColor"></param>
        /// <param name="hasBorder"></param>
        /// <param name="isSaveState"></param>
        public static void DrawShape(ICanvas canvas, RectangleF rect, ShapeType shapeType, Brush fillColor, bool hasBorder, bool isSaveState)
        {
            float x = rect.X;
            float y = rect.Y;
            float radius = Math.Min(rect.Width, rect.Height) / 2;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);
            float midHeight = y + (rect.Height / 2);
            var center = rect.Center;

            switch (shapeType)
            {
                case ShapeType.Rectangle:
                    DrawRectangle(canvas, rect, fillColor, hasBorder, isSaveState);
                    break;
                case ShapeType.Circle:
                    DrawCircle(canvas, rect, center, radius, fillColor, hasBorder, isSaveState);
                    break;
                case ShapeType.HorizontalLine:
                    DrawLine(canvas, new PointF(x, midHeight), new PointF(width, midHeight), isSaveState);
                    //DrawHorizontalLine(canvas, rect, hasBorder, isSaveState);
                    break;
                case ShapeType.VerticalLine:
                    DrawLine(canvas, new PointF(midWidth, y), new PointF(midWidth, height), isSaveState);
                    //DrawVerticalLine(canvas, rect, hasBorder, isSaveState);
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

        #endregion

        #region Private Methods

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

        private static void DrawHorizontalLine(ICanvas canvas, RectangleF rect, bool hasBorder, bool isSaveState)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);
            float midHeight = y + (rect.Height / 2);

            float lineWidth = rect.Width / 5;
            float lineHeight = rect.Height / 5;

            if (isSaveState)
            {
                canvas.SaveState();
            }

            var path = new PathF();
            path.MoveTo(x, midHeight - lineHeight);
            path.LineTo(width, midHeight - lineHeight);
            path.LineTo(width, midHeight + lineHeight);
            path.LineTo(x, midHeight + lineHeight);
            path.LineTo(x, midHeight - lineHeight);
            path.Close();
            canvas.FillPath(path);

            if (hasBorder)
            {
                canvas.DrawPath(path);
            }

            if (isSaveState)
            {
                canvas.RestoreState();
            }
        }

        private static void DrawVerticalLine(ICanvas canvas, RectangleF rect, bool hasBorder, bool isSaveState)
        {
            float x = rect.X;
            float y = rect.Y;
            float width = x + rect.Width;
            float height = y + rect.Height;
            float midWidth = x + (rect.Width / 2);
            float midHeight = y + (rect.Height / 2);

            float lineWidth = rect.Width / 5;
            float lineHeight = rect.Height / 5;

            if (isSaveState)
            {
                canvas.SaveState();
            }

            var path = new PathF();
            path.MoveTo(midWidth + lineWidth, y);
            path.LineTo(midWidth + lineWidth, height);
            path.LineTo(midWidth - lineWidth, height);
            path.LineTo(midWidth - lineWidth, x);
            path.LineTo(midWidth + lineWidth, x);
            path.Close();
            canvas.FillPath(path);

            if (hasBorder)
            {
                canvas.DrawPath(path);
            }

            if (isSaveState)
            {
                canvas.RestoreState();
            }
        }

        private static void DrawCircle(ICanvas canvas, RectangleF rect, PointF center, float radius, Brush fillColor, bool hasBorder, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.SaveState();
            }

            canvas.SetFillPaint(fillColor, rect);
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

        private static void DrawRectangle(ICanvas canvas, RectangleF rect, Brush fillColor, bool hasBorder, bool isSaveState)
        {
            if (isSaveState)
            {
                canvas.SaveState();
            }

            canvas.SetFillPaint(fillColor, rect);
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
        private static void OnPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is SfShapeView shape)
            {
                shape.InvalidateDrawable();
            }
        }

        private void DrawlineWithMarker(ShapeType iconType)
        {
            //Todo:Add line drawing code
        }

        #endregion
        
        #endregion
    }

}
