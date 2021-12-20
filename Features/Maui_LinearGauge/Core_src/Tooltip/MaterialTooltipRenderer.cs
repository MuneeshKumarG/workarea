using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// Show the tooltip based on target rectangle.
    /// </summary>
    internal class MaterialToolTipRenderer
    {
        #region Fields

        private Point nosePoint = Point.Zero;

        private float noseOffset = 2f;

        private float noseHeight = 0f;

        private float noseWidth = 4f;

        private float fontSize = 14.0f;

        #endregion

        #region internal properties

        /// <summary>
        /// Gets or sets a value that indicates the position of tooltip.
        /// </summary>
        internal TooltipPosition Position { get; set; } = TooltipPosition.Auto;

        /// <summary>
        /// Gets or sets a value that indicates the padding of tooltip.
        /// </summary>
        internal Thickness Padding { get; set; } = new Thickness(15.0, 8.0);

        /// <summary>
        /// Gets or sets a value that indicates the corner radius of tooltip.
        /// </summary>
        internal float CornerRadius { get; set; } = 3f;

        internal Size ContentSize { get; set; }

        #endregion

        #region internal Methods

        /// <summary>
        /// The tooltip show the additional information about the control.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="containerRect"></param>
        /// <param name="targetRects"></param>
        /// <param name="texts"></param>
        /// <param name="animationValue"></param>
        /// <param name="paint"></param>
        internal void Show(ICanvas canvas, Rectangle containerRect, Rectangle[] targetRects, string[] texts, double animationValue, ToolTipPaint paint)
        {
            noseHeight = paint.NoseHeight;
            noseWidth = paint.NoseWidth;
            noseOffset = paint.NoseOffset;
            CornerRadius = paint.CornerRadius;
            Rectangle[] toolTipRects = new Rectangle[texts.Length];
            for (int i = 0; i < texts.Length; i++)
            {
                if (containerRect.IsEmpty || texts[i] == null) return;
                Size size = texts[i].Measure(fontSize);
#if WINDOWS
                ContentSize = new Size(size.Width + Padding.Left + Padding.Right, size.Height / 2 + Padding.Top + Padding.Bottom); 
#else
                ContentSize = new Size(size.Width + Padding.Left + Padding.Right, size.Height + Padding.Top + Padding.Bottom);
#endif
                toolTipRects[i] = GetToolTipRect(Position, targetRects[i], containerRect);
            }

            bool canDrawOverlappingStroke = false;
            if (toolTipRects.Length == 2)
            {
                canDrawOverlappingStroke = toolTipRects[1].IntersectsWith(toolTipRects[0]);
            }

            for (int i = 0; i < toolTipRects.Length; i++)
            {
                SetNosePoint(Position, targetRects[i], toolTipRects[i]);

                PathGeometry geometry = GetClipPathGeometry(GetGeometryPoints(Position, toolTipRects[i]), Position);
                PathF path = new PathF();
                Point textPoint;
                if (Position == TooltipPosition.Right)
                {
                    Point translatePoint = new Point(0, ((LineSegment)geometry.Figures[0].Segments[7]).Point.Y);
                    textPoint = new Point(noseOffset + noseHeight + Padding.Left, translatePoint.Y - Padding.Bottom);
                    AppendPath(path, geometry.Figures, translatePoint);
                }
                else
                {
                    Point translatePoint = new Point(((LineSegment)geometry.Figures[0].Segments[5]).Point.X, toolTipRects[i].Height);
                    textPoint = new Point(-translatePoint.X + Padding.Left, -(noseOffset + noseHeight + Padding.Bottom));
                    AppendPath(path, geometry.Figures, translatePoint);
                }

                canvas.SaveState();
                canvas.Translate((float)targetRects[i].X, (float)targetRects[i].Y);
                canvas.Scale((float)animationValue, (float)animationValue);

                if (((Paint)paint.FillColor).ToColor() != Colors.Transparent)
                {
                    canvas.FillColor = Colors.Transparent;
                    canvas.SetFillPaint(paint.FillColor, path.Bounds);
                    canvas.StrokeColor = Colors.Transparent;
                    canvas.FillPath(path);
                    canvas.DrawPath(path);
                }

                Paint strokePaint = paint.StrokeColor;
                if (strokePaint.ToColor() != Colors.Transparent)
                {
                    canvas.FillColor = Colors.Transparent;
                    canvas.StrokeColor = strokePaint.ToColor();
                    canvas.StrokeSize = paint.StrokeSize;
                    canvas.FillPath(path);
                    canvas.DrawPath(path);
                }

                if (canDrawOverlappingStroke)
                {
                    canvas.FillColor = Colors.Transparent;
                    canvas.StrokeColor = Colors.White;
                    canvas.StrokeSize = 0.5f;
                    canvas.FillPath(path);
                    canvas.DrawPath(path);
                }

                canvas.FontSize = fontSize;
                canvas.FontColor = ((Paint)paint.FillColor).ToColor()?.GetLuminosity() > 0.5 ? Colors.Black : Colors.White;
                canvas.DrawString(texts[i], (float)textPoint.X, (float)textPoint.Y, HorizontalAlignment.Left);
                canvas.RestoreState();
            }
        }

        internal void SetNosePoint(TooltipPosition position, Rectangle targetRect, Rectangle toolTipRect)
        {
            double noseOrigin = 0d;
            switch (position)
            {
                case TooltipPosition.Bottom:
                case TooltipPosition.Top:
                case TooltipPosition.Auto:
                    if (toolTipRect.Width < targetRect.Width)
                    {
                        noseOrigin = this.ContentSize.Width / 2;
                    }
                    else
                    {
                        noseOrigin = Math.Abs(toolTipRect.X - targetRect.X) + targetRect.Width / 2;
                    }

                    nosePoint = new Point(noseOrigin, (position == TooltipPosition.Auto || position == TooltipPosition.Top) ? toolTipRect.Height - noseOffset : noseOffset);
                    break;

                case TooltipPosition.Left:
                case TooltipPosition.Right:
                    if (toolTipRect.Height < targetRect.Height)
                    {
                        noseOrigin = this.ContentSize.Height / 2;
                    }
                    else
                    {
                        noseOrigin = Math.Abs(toolTipRect.Y - targetRect.Y) + targetRect.Height / 2;
                    }

                    nosePoint = new Point(position == TooltipPosition.Right ? noseOffset : toolTipRect.Width - noseOffset, noseOrigin);
                    break;
            }
        }

        internal Rectangle GetToolTipRect(TooltipPosition position, Rectangle targetRect, Rectangle containerRect)
        {
            double xPos = 0, yPos = 0;
            double width = ContentSize.Width;
            double height = ContentSize.Height;

            switch (position)
            {
                case TooltipPosition.Top:
                case TooltipPosition.Auto:
                    xPos = targetRect.Center.X - width / 2;
                    yPos = targetRect.Y - height - noseOffset - noseHeight;
                    height += noseOffset + noseHeight;
                    break;

                case TooltipPosition.Bottom:
                    xPos = targetRect.Center.X - width / 2;
                    yPos = targetRect.Bottom;
                    height += noseOffset + noseHeight;
                    break;

                case TooltipPosition.Right:
                    xPos = targetRect.Right;
                    yPos = targetRect.Center.Y - height / 2;
                    width += noseOffset + noseHeight;
                    break;

                case TooltipPosition.Left:
                    width += noseOffset + noseHeight;
                    xPos = targetRect.X - width;
                    yPos = targetRect.Center.Y - height / 2;
                    break;
            }

            var positionRect = new Rectangle(xPos, yPos, width, height);
            EdgedDetection(ref positionRect, containerRect);
            return positionRect;
        }

        internal PointCollection GetGeometryPoints(TooltipPosition position, Rectangle toolTipRect)
        {
            PointCollection points = new PointCollection();
            switch (position)
            {
                case TooltipPosition.Auto:
                case TooltipPosition.Top:
                    points.Add(new Point(0, 0));
                    points.Add(new Point(toolTipRect.Width, 0));
                    points.Add(new Point(toolTipRect.Width, toolTipRect.Height - noseHeight - noseOffset));
                    points.Add(new Point(nosePoint.X + noseWidth, nosePoint.Y - noseHeight));
                    points.Add(nosePoint);
                    points.Add(new Point(nosePoint.X - noseWidth, nosePoint.Y - noseHeight));
                    points.Add(new Point(0, toolTipRect.Height - noseHeight - noseOffset));
                    points.Add(new Point(0, 0));
                    points.Add(new Point(toolTipRect.Width, 0));
                    break;

                case TooltipPosition.Bottom:
                    points.Add(new Point(0, nosePoint.Y + noseHeight));
                    points.Add(new Point(nosePoint.X - noseWidth, nosePoint.Y + noseHeight));
                    points.Add(nosePoint);
                    points.Add(new Point(nosePoint.X + noseWidth, nosePoint.Y + noseHeight));
                    points.Add(new Point(toolTipRect.Width, nosePoint.Y + noseHeight));
                    points.Add(new Point(toolTipRect.Width, toolTipRect.Height));
                    points.Add(new Point(0, toolTipRect.Height));
                    points.Add(new Point(0, nosePoint.Y + noseHeight));
                    points.Add(new Point(nosePoint.X - noseWidth, nosePoint.Y + noseHeight));
                    break;

                case TooltipPosition.Right:
                    points.Add(new Point(nosePoint.X + noseHeight, 0));
                    points.Add(new Point(toolTipRect.Width, 0));
                    points.Add(new Point(toolTipRect.Width, toolTipRect.Height));
                    points.Add(new Point(nosePoint.X + noseHeight, toolTipRect.Height));
                    points.Add(new Point(nosePoint.X + noseHeight, nosePoint.Y + noseWidth));
                    points.Add(nosePoint);
                    points.Add(new Point(nosePoint.X + noseHeight, nosePoint.Y - noseWidth));
                    points.Add(new Point(nosePoint.X + noseHeight, 0));
                    points.Add(new Point(toolTipRect.Width, 0));
                    break;

                case TooltipPosition.Left:
                    points.Add(new Point(0, 0));
                    points.Add(new Point(toolTipRect.Width - noseHeight - noseOffset, 0));
                    points.Add(new Point(toolTipRect.Width - noseHeight - noseOffset, toolTipRect.Height / 2 - noseWidth));
                    points.Add(nosePoint);
                    points.Add(new Point(toolTipRect.Width - noseHeight - noseOffset, toolTipRect.Height / 2 + noseWidth));
                    points.Add(new Point(toolTipRect.Width - noseHeight - noseOffset, toolTipRect.Height));
                    points.Add(new Point(0, toolTipRect.Height));
                    points.Add(new Point(0, 0));
                    points.Add(new Point(toolTipRect.Width - noseHeight - noseOffset, 0));
                    break;
            }

            return points;
        }

        #endregion

        #region Private Methods

        private void EdgedDetection(ref Rectangle positionRect, Rectangle containerRect)
        {
            if (positionRect.X < 0)
            {
                positionRect.X = 0;
            }
            else if (positionRect.Right > containerRect.Width)
            {
                positionRect.X = containerRect.Width - positionRect.Width;
            }

            if (positionRect.Y < 0)
            {
                positionRect.Y = 0;
            }
            else if (positionRect.Bottom > containerRect.Height)
            {
                positionRect.Y = containerRect.Height - positionRect.Height;
            }
        }

        private PathGeometry GetClipPathGeometry(PointCollection points, TooltipPosition position)
        {
            PathFigure figure = new PathFigure();

            if (points.Count > 0)
            {
                figure.StartPoint = points[0];

                if (points.Count > 1)
                {
                    double desiredRadius = CornerRadius;
                    LineSegment line = new LineSegment();

                    for (int i = 1; i < (points.Count - 1); i++)
                    {
                        switch (position)
                        {
                            case TooltipPosition.Left:
                                if (i == 2 || i == 3 || i == 4)
                                {
                                    line = new LineSegment(points[i]);
                                    figure.Segments.Add(line);
                                    continue;
                                }
                                break;
                            case TooltipPosition.Auto:
                            case TooltipPosition.Top:
                                if (i == 3 || i == 4 || i == 5)
                                {
                                    line = new LineSegment(points[i]);
                                    figure.Segments.Add(line);
                                    continue;
                                }
                                break;
                            case TooltipPosition.Right:
                                if (i == 4 || i == 5 || i == 6)
                                {
                                    line = new LineSegment(points[i]);
                                    figure.Segments.Add(line);
                                    continue;
                                }
                                break;
                            case TooltipPosition.Bottom:
                                if (i == 1 || i == 2 || i == 3)
                                {
                                    line = new LineSegment(points[i]);
                                    figure.Segments.Add(line);
                                    continue;
                                }
                                break;
                        }

                        Vector2 v1 = new Vector2(points[i].X - points[i - 1].X, points[i].Y - points[i - 1].Y);
                        Vector2 v2 = new Vector2(points[i + 1].X - points[i].X, points[i + 1].Y - points[i].Y);
                        double radius = Math.Min(Math.Min(v1.Length, v2.Length) / 2, desiredRadius);

                        double len = v1.Length;
                        v1 = v1.Normalized;
                        v1 *= (len - radius);
                        line = new LineSegment(points[i - 1] + v1);
                        figure.Segments.Add(line);

                        v2 = v2.Normalized;
                        v2 *= radius;
                        SweepDirection direction = SweepDirection.Clockwise;
                        ArcSegment arc = new ArcSegment(points[i] + v2, new Size(radius, radius), 0, direction, false);
                        figure.Segments.Add(arc);
                    }

                    figure.Segments.Add(new LineSegment(points[points.Count - 1]));
                }
            }

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);
            return geometry;
        }

        private void AppendPath(PathF path, PathFigureCollection figures, Point translatePoint)
        {
            foreach (PathFigure pathFigure in figures)
            {
                for (int i = 0; i < pathFigure.Segments.Count - 1; i++)
                {
                    var pathSegment = pathFigure.Segments[i];
                    if (pathSegment is LineSegment lineSegment)
                    {
                        AddLine(path, lineSegment, translatePoint);
                    }
                    else if (pathSegment is ArcSegment arcSegment)
                    {
                        AddCurve(path, arcSegment, translatePoint, i);
                    }
                }
                path.Close();
            }
        }

        private void AddLine(PathF path, LineSegment lineSegment, Point translatePoint)
        {
            path.LineTo((float)(lineSegment.Point.X - translatePoint.X),
                (float)(lineSegment.Point.Y - translatePoint.Y));
        }

        private void AddCurve(PathF path, ArcSegment arcSegment, Point translatePoint, int index)
        {
            float x = (float)(arcSegment.Point.X - translatePoint.X);
            float y = (float)(arcSegment.Point.Y - translatePoint.Y);
            float desiredRadius = CornerRadius - (CornerRadius * 0.55f);
            if (index == 1)
            {
                path.CurveTo(
                    new PointF(x - desiredRadius, (float)y - CornerRadius),
                    new PointF(x, (float)y - CornerRadius + desiredRadius),
                    new PointF(x, y));
            }
            else if (index == 3)
            {
                path.CurveTo(
                   new PointF(x + CornerRadius, y - desiredRadius),
                   new PointF(x + CornerRadius - desiredRadius, y),
                   new PointF(x, y));
            }
            else if (index == 8 || index == 5)
            {
                path.CurveTo(
                   new PointF(x + desiredRadius, y + CornerRadius),
                   new PointF(x, y + CornerRadius - desiredRadius),
                   new PointF(x, y));
            }
            else if (index == 10)
            {
                path.CurveTo(
                   new PointF(x - CornerRadius, y + desiredRadius),
                   new PointF(x - CornerRadius + desiredRadius, y),
                   new PointF(x, y));
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents the style of the tooltip..
    /// </summary>
    internal class ToolTipPaint
    {
        /// <summary>
        /// Represents the color of the tooltip..
        /// </summary>
        internal Brush FillColor { get; set; } = new SolidColorBrush(Colors.Transparent);

        /// <summary>
        /// Represents the stroke color of the tooltip..
        /// </summary>
        internal Brush StrokeColor { get; set; } = new SolidColorBrush(Colors.Transparent);

        /// <summary>
        /// Represents the stroke size of the tooltip..
        /// </summary>
        internal float StrokeSize { get; set; } = 0f;

        /// <summary>
        /// Represents the nose offset of the tooltip..
        /// </summary>
        internal float NoseOffset { get; set; } = 2f;

        /// <summary>
        /// Represents the nose height of the tooltip..
        /// </summary>
        internal float NoseHeight { get; set; } = 0f;

        /// <summary>
        /// Represents the nose width of the tooltip..
        /// </summary>
        internal float NoseWidth { get; set; } = 4f;

        /// <summary>
        /// Represents the corner radius of the tooltip..
        /// </summary>
        internal float CornerRadius { get; set; } = 3f;

        internal bool HasStroke
        {
            get => StrokeSize > 0 && ((Paint)StrokeColor).ToColor() != Colors.Transparent;
        }
    }
}