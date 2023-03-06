using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
	internal class CartesianGridLineLayout : SfDrawableView, IAxisLayout
	{
        #region Fields

		internal readonly CartesianChartArea Area;

		#endregion
		public CartesianGridLineLayout(CartesianChartArea area)
		{
			Area = area;
		}

		#region Methods

		protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
		{
			base.OnDraw(canvas, dirtyRect);
			OnDraw(canvas);
		}

		public Size Measure(Size availableSize)
		{
			return availableSize;
		}

		public void OnDraw(ICanvas canvas)
		{
			if (Area != null)
			{
				canvas.SaveState();
				canvas.Translate(0, 0);

				foreach (var axis in Area.XAxes)
				{
					DrawGridLines(axis, canvas);
				}

				foreach (var axis in Area.YAxes)
				{
					DrawGridLines(axis, canvas);
				}

				canvas.RestoreState();
			}
		}

		private void DrawGridLines(ChartAxis axis, ICanvas canvas)
		{
			List<double> tickPositions = axis.TickPositions;
			var renderRect = new RectF(axis.RenderedRect.Location, axis.RenderedRect.Size);
			var thickness = Area.PlotAreaMargin;
			if (tickPositions.Count > 0)
			{
				float left = renderRect.Left - (float)thickness.Left;
				float right = renderRect.Right - (float)thickness.Left;
				float top = renderRect.Top - (float)thickness.Top;
				float bottom = renderRect.Bottom - (float)thickness.Top;

				float width;
				float height;

				var gridLineStyle = axis.MajorGridLineStyle;
				ChartLineStyle? minorGirdLineStyle = null;
				var rangeAxisBase = axis as RangeAxisBase;

				if (axis.SmallTickRequired && rangeAxisBase != null)
				{
					minorGirdLineStyle = rangeAxisBase.MinorGridLineStyle;
				}

				List<ChartAxis> selectedAxes;

				if (axis.RegisteredSeries.Count > 0)
				{
					selectedAxes = axis.AssociatedAxes;
				}
				else
				{
					selectedAxes = new List<ChartAxis>();
					if (axis.IsVertical && Area.PrimaryAxis != null)
						selectedAxes.Add(Area.PrimaryAxis);
					else if (Area.SecondaryAxis != null)
						selectedAxes.Add(Area.SecondaryAxis);
				}

				if (!axis.IsVertical)
				{
					width = right - left;

					foreach (ChartAxis supportAxis in selectedAxes)
					{
						if (supportAxis != null)
						{
							top = (float)supportAxis.ArrangeRect.Top - (float)thickness.Top;
							height = (float)top + (float)supportAxis.ArrangeRect.Height;

							if (axis.CanDrawMajorGridLines())
							{
								DrawVerticalGridLines(canvas, tickPositions, axis, left, top, width, height, gridLineStyle);
							}

							if (axis.SmallTickRequired && rangeAxisBase != null && rangeAxisBase.CanDrawMinorGridLines())
							{
								List<double> smallTicks = rangeAxisBase.SmallTickPoints;
								DrawVerticalGridLines(canvas, smallTicks, axis, left, top, width, height, minorGirdLineStyle);
							}
						}
					}
				}
				else
				{
					height = bottom - top;

					foreach (ChartAxis supportAxis in selectedAxes)
					{
						if (supportAxis != null)
						{
							left = (float)supportAxis.ArrangeRect.Left - (float)thickness.Left;
							width = (float)left + (float)supportAxis.ArrangeRect.Width;

							if (axis.CanDrawMajorGridLines())
							{
								DrawHorizontalGridLines(canvas, tickPositions, axis, left, top, width, height, gridLineStyle);
							}

							if (axis.SmallTickRequired && rangeAxisBase != null && rangeAxisBase.CanDrawMinorGridLines())
							{
								List<double> smallTicks = rangeAxisBase.SmallTickPoints;
								DrawHorizontalGridLines(canvas, smallTicks, axis, left, top, width, height, minorGirdLineStyle);
							}
						}
					}
				}
			}
		}

		private void DrawVerticalGridLines(ICanvas canvas, List<double> tickPositions, ChartAxis axis, float left, float top, float width, float height, ChartLineStyle? gridLineStyle)
        {
			foreach (var tickPosition in tickPositions)
			{
				double value = axis.ValueToCoefficient(tickPosition);
				float x = (float)Math.Round(width * value) + left;

				if (gridLineStyle != null && gridLineStyle.CanDraw())
				{
					canvas.StrokeSize = (float)gridLineStyle.StrokeWidth;
					canvas.StrokeColor = gridLineStyle.Stroke.ToColor();
					if (gridLineStyle.StrokeDashArray != null)
					{
						canvas.StrokeDashPattern = gridLineStyle.StrokeDashArray.ToFloatArray();
					}
				}

				axis.DrawGridLine(canvas, tickPosition, x, top, x, height);
			}
		}

		private void DrawHorizontalGridLines(ICanvas canvas, List<double> tickPositions, ChartAxis axis, float left, float top, float width, float height, ChartLineStyle? gridLineStyle)
        {
			foreach (var tickPosition in tickPositions)
			{
				double value = axis.ValueToCoefficient(tickPosition);
				float y = (float)Math.Round(height * (1f - value)) + top;
				//TODO: Set range base style if range style not null
				if (gridLineStyle != null && gridLineStyle.CanDraw())
				{
					canvas.StrokeSize = (float)gridLineStyle.StrokeWidth;
					canvas.StrokeColor = gridLineStyle.Stroke.ToColor();
					if (gridLineStyle.StrokeDashArray != null)
					{
						canvas.StrokeDashPattern = gridLineStyle.StrokeDashArray.ToFloatArray();
					}
				}

				axis.DrawGridLine(canvas, tickPosition, left, y, width, y);
			}
		}

        #endregion
    }
}