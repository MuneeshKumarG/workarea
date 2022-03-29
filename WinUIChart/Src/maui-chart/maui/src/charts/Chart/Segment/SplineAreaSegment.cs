using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Maui.Charts
{
	/// <summary>
	/// 
	/// </summary>
	public class SplineAreaSegment : AreaSegment
	{
        #region Fields
        private double minY, maxY;
        #endregion

        #region Internal Properties

        internal double[] XVal { get; set; }

		internal double[] YVal { get; set; }

		internal double[] ControlStartX { get; set; }

		internal double[] ControlStartY { get; set; }

		internal double[] ControlEndX { get; set; }

		internal double[] ControlEndY { get; set; }

		internal List<float> StartControlPoints { get; set; }

		internal List<float> EndControlPoints { get; set; }

		internal List<float> StrokeControlStartPoints { get; set; }

		internal List<float> StrokeControlEndPoints { get; set; }

        #endregion

        #region Constructor

		/// <summary>
		/// 
		/// </summary>
        public SplineAreaSegment() :base()
        {
			this.YVal = new double[] { };
			this.XVal = new double[] { };
			this.ControlStartX = new double[] { };
			this.ControlStartY = new double[] { };
			this.ControlEndX = new double[] { };
			this.ControlEndY = new double[] { };
			this.StartControlPoints = new List<float>();
			this.EndControlPoints = new List<float>();
			this.StrokeControlStartPoints = new List<float>();
			this.StrokeControlEndPoints = new List<float>();
		}
		#endregion

		#region Methods
		/// <inheritdoc/>
		protected internal override void OnLayout()
		{
			if (XVal == null)
			{
				return;
			}

			this.CalculateInteriorPoints();

			if (HasStroke)
			{
				this.CalculateStrokePoints();
			}
		}

		/// <inheritdoc/>
		protected internal override void Draw(ICanvas canvas)
		{
			canvas.Alpha = Opacity;

			if (HasStroke)
			{
				canvas.StrokeSize = (float)StrokeWidth;
				canvas.StrokeColor = Stroke.ToColor();
			}

			if (StrokeDashArray != null)
			{
				canvas.StrokeDashPattern = StrokeDashArray.ToFloatArray();
			}

			DrawPath(canvas, FillPoints, StrokePoints);
		}

		/// <summary>
		/// Sets the values for this segment.
		/// </summary>
		/// <param name="xValues">The x values.</param>
		/// <param name="yValues">The y values.</param>
		/// <param name="startControlPointsX">The StartControlPointsX values.</param>
		/// <param name="startControlPointsY">The StartControlPointsY values.</param>
		/// <param name="endControlPointsX">The EndControlPointsX values.</param>
		/// <param name="endControlPointsY">The EndControlPointsY values.</param>
		internal override void SetData(IList? xValues, IList? yValues, IList? startControlPointsX, IList? startControlPointsY, IList? endControlPointsX, IList? endControlPointsY)
		{
			int count = 0;
			if (xValues != null)
			{
				count = xValues.Count;
			}

			this.YVal = new double[count];
			this.XVal = new double[count];
			this.ControlStartX = new double[count];
			this.ControlStartY = new double[count];
			this.ControlEndX = new double[count];
			this.ControlEndY = new double[count];

			xValues?.CopyTo(this.XVal, 0);
			yValues?.CopyTo(this.YVal, 0);
			startControlPointsX?.CopyTo(this.ControlStartX, 0);
			startControlPointsY?.CopyTo(this.ControlStartY, 0);
			endControlPointsX?.CopyTo(this.ControlEndX, 0);
			endControlPointsY?.CopyTo(this.ControlEndY, 0);

			UpdateRange();

			XValues = XVal;
			YValues = YVal;
		}

		/// <summary>
		/// Updates the range with control points
		/// </summary>
		private void UpdateRange()
		{
			var series = Series as CartesianSeries;
			if (series != null && series.ActualYAxis != null)
			{
				minY = this.YVal.Min();
				minY = double.IsNaN(minY) ? this.YVal.Any() ? this.YVal.Where(e => !double.IsNaN(e)).DefaultIfEmpty().Min() : 0 : minY;
				minY = minY == 0 ? series.ActualYAxis.VisibleRange.Start : minY;
				maxY = YVal.Max();

				double startControlMin = ControlStartY.Min();
				double startControlMax = ControlStartY.Max();

				double endControlMin = ControlEndY.Min();
				double endControlMax = ControlEndY.Max();

				if (maxY < startControlMax)
				{
					maxY = startControlMax;
				}

				if (minY > startControlMin)
				{
					minY = startControlMin;
				}

				if (maxY < endControlMax)
				{
					maxY = endControlMax;
				}

				if (minY > endControlMin)
				{
					minY = endControlMin;
				}

				series.XRange += new DoubleRange(this.XVal.Min(), this.XVal.Max());
				series.YRange += new DoubleRange(minY, maxY);
			}
		}

		/// <summary>
		/// Calculate interior points.
		/// </summary>
		private void CalculateInteriorPoints()
		{
			var cartesian = Series as CartesianSeries;
			if (cartesian == null || cartesian.ActualXAxis == null)
			{
				return;
			}

			var crossingValue = cartesian.ActualXAxis.ActualCrossingValue;
			crossingValue = double.IsNaN(crossingValue) ? 0 : crossingValue;

			var count = XVal.Count();
			this.FillPoints = new List<float>();
			this.StartControlPoints = new List<float>();
			this.EndControlPoints = new List<float>();
			double yValue = this.YVal[0], xValue = this.XVal[0], startX, startY, endX, endY;

			this.FillPoints.Add(cartesian.TransformToVisibleX(xValue, 0));
			FillPoints.Add(cartesian.TransformToVisibleY(xValue, crossingValue));

			for (int i = 0; i < count - 1; i++)
			{
				xValue = this.XVal[i];
				yValue = this.YVal[i];

				this.FillPoints.Add(cartesian.TransformToVisibleX(xValue, yValue));
				this.FillPoints.Add(cartesian.TransformToVisibleY(xValue, yValue));

				startX = ControlStartX[i];
				startY = ControlStartY[i];
				StartControlPoints.Add(cartesian.TransformToVisibleX(startX, startY));
				StartControlPoints.Add(cartesian.TransformToVisibleY(startX, startY));

				endX = ControlEndX[i];
				endY = ControlEndY[i];
				EndControlPoints.Add(cartesian.TransformToVisibleX(endX, endY));
				EndControlPoints.Add(cartesian.TransformToVisibleY(endX, endY));
			}

			xValue = this.XVal[count - 1];
			yValue = this.YVal[count - 1];
			this.FillPoints.Add(cartesian.TransformToVisibleX(xValue, yValue));
			this.FillPoints.Add(cartesian.TransformToVisibleY(xValue, yValue));
			this.FillPoints.Add(cartesian.TransformToVisibleX(xValue, 0));
			FillPoints.Add(cartesian.TransformToVisibleY(xValue, crossingValue));
		}

		private void CalculateStrokePoints()
		{
			var series = Series as CartesianSeries;
			if (series != null && SeriesView != null && series.ActualYAxis != null)
			{
				float x, y, ControlStartXVal, ControlStartYVal, ControlEndXVal, ControlEndYVal;
				this.StrokePoints = new List<float>();
				this.StrokeControlStartPoints = new List<float>();
				this.StrokeControlEndPoints = new List<float>();
				int segsCount = series.Segments.Count;
				var halfStrokeWidth = (float)StrokeWidth / 2;
				double yValue, xValue, startX, startY, endX, endY;

				var start = series.ActualYAxis.VisibleRange.Start;

				var count = XVal.Count();

				for (int i = 0; i < count; i++)
				{
					yValue = this.YVal[i];
					xValue = this.XVal[i];
					x = series.TransformToVisibleX(xValue, yValue);
					y = series.TransformToVisibleY(xValue, yValue);

					startX = ControlStartX[i];
					startY = ControlStartY[i];
					ControlStartXVal = series.TransformToVisibleX(startX, startY);
					ControlStartYVal = series.TransformToVisibleY(startX, startY);

					endX = ControlEndX[i];
					endY = ControlEndY[i];
					ControlEndXVal = series.TransformToVisibleX(endX, endY);
					ControlEndYVal = series.TransformToVisibleY(endX, endY);

					this.StrokePoints.Add(x);
					this.StrokePoints.Add(y += yValue >= 0 ? halfStrokeWidth : -halfStrokeWidth);

					StrokeControlStartPoints.Add(ControlStartXVal);
					StrokeControlStartPoints.Add(ControlStartYVal += startY >= 0 ? halfStrokeWidth : -halfStrokeWidth);
					StrokeControlEndPoints.Add(ControlEndXVal);
					StrokeControlEndPoints.Add(ControlEndYVal += endY >= 0 ? halfStrokeWidth : -halfStrokeWidth);
				}

				if (segsCount == 1 || series.Segments.Last() == this)
				{
					xValue = this.XVal[count - 1];
					x = series.TransformToVisibleX(xValue, start);
					y = series.TransformToVisibleY(xValue, start);
					this.StrokePoints.Add(x);
					this.StrokePoints.Add(y += start >= 0 ? halfStrokeWidth : -halfStrokeWidth);
				}
			}
		}

		private void DrawPath(ICanvas canvas, IList<float>? fillPoints, IList<float>? strokePoints)
		{
			var path = new PathF();

			if (Series == null || fillPoints == null)
			{
				return;
			}

			if (Series.CanAnimate())
			{
				AnimateSeriesClipRect(canvas, Series.AnimationValue);
			}

			path.MoveTo(fillPoints[0], fillPoints[1]);
			path.LineTo(fillPoints[2], fillPoints[3]);

			if (StartControlPoints != null && EndControlPoints != null)
			{
				for (int i = 0; i < StartControlPoints?.Count; i++)
				{
					var endPointX = fillPoints[i + 4];
					var endPointY = fillPoints[i + 5];

					var controlStartX = StartControlPoints[i];
					var controlStartY = StartControlPoints[i + 1];
					var controlEndX = EndControlPoints[i];
					var controlEndY = EndControlPoints[i + 1];

					path.CurveTo(controlStartX, controlStartY, controlEndX, controlEndY, endPointX, endPointY);
					i++;
				}
			}

			path.LineTo(fillPoints[fillPoints.Count - 2], fillPoints[fillPoints.Count - 1]);
			path.LineTo(fillPoints[0], fillPoints[1]);

			canvas.SetFillPaint(Fill, path.Bounds);
			canvas.FillPath(path);

			if (HasStroke && strokePoints != null)
			{
				path = new PathF();

				path.MoveTo(strokePoints[0], strokePoints[1]);

				if (StartControlPoints != null && EndControlPoints != null)
				{
					for (int i = 0; i < StartControlPoints?.Count; i++)
					{
						var endPointX = strokePoints[i + 2];
						var endPointY = strokePoints[i + 3];

						var controlStartX = StrokeControlStartPoints[i];
						var controlStartY = StrokeControlStartPoints[i + 1];
						var controlEndX = StrokeControlEndPoints[i];
						var controlEndY = StrokeControlEndPoints[i + 1];

						path.CurveTo(controlStartX, controlStartY, controlEndX, controlEndY, endPointX, endPointY);
						i++;
					}
				}

				canvas.DrawPath(path);
			}
		}

        #endregion
    }
}
