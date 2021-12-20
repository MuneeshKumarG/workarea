using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using System;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// This class represents a content view to show tooltip in absolute layout.
    /// </summary>
    internal partial class SfTooltip : ContentView
    {
        private float cornerRadius = 3f;

        #region Methods

        private Microsoft.Maui.Controls.Shapes.Geometry GetClipPathGeometry(PointCollection points, Rectangle tooltipRect)
        {
            PathFigure figure = new PathFigure();
            PathFigure figure1 = new PathFigure();

            if (points.Count > 0)
            {
                figure.StartPoint = points[0];
                figure1.StartPoint = points[0];

                if (points.Count > 1)
                {
                    double desiredRadius = cornerRadius;
                    LineSegment line = new LineSegment();

                    for (int i = 1; i < (points.Count - 1); i++)
                    {
                        switch (ActualPosition)
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
                        figure1.Segments.Add(line);

                        v2 = v2.Normalized;
                        v2 *= radius;
                        SweepDirection direction = SweepDirection.Clockwise;
                        ArcSegment arc = new ArcSegment(points[i] + v2, new Size(radius, radius), 0, direction, false);
                        figure.Segments.Add(arc);
                    }

                    figure.Segments.Add(new LineSegment(points[points.Count - 1]));
                    figure1.Segments.Add(new LineSegment(points[points.Count - 1]));
                }
            }

            PathGeometry geometry = new PathGeometry();
            geometry.Figures.Add(figure);
            geometry.Figures.Add(figure1);
            return geometry;
        }

        #endregion
    }
}
