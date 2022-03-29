using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
	/// <summary>
	/// 
	/// </summary>
	/// <example>
	/// # [Xaml](#tab/tabid-1)
	/// <code><![CDATA[
	///     <chart:SfCartesianChart>
	///
	///           <chart:SfCartesianChart.XAxes>
	///               <chart:NumericalAxis/>
	///           </chart:SfCartesianChart.XAxes>
	///
	///           <chart:SfCartesianChart.YAxes>
	///               <chart:NumericalAxis/>
	///           </chart:SfCartesianChart.YAxes>
	///
	///           <chart:SfCartesianChart.Series>
	///               <chart:SplineAreaSeries
	///                   ItemsSource="{Binding Data}"
	///                   XBindingPath="XValue"
	///                   YBindingPath="YValue"/>
	///           </chart:SfCartesianChart.Series>  
	///           
	///     </chart:SfCartesianChart>
	/// ]]></code>
	/// # [C#](#tab/tabid-2)
	/// <code><![CDATA[
	///     SfCartesianChart chart = new SfCartesianChart();
	///     
	///     NumericalAxis xAxis = new NumericalAxis();
	///     NumericalAxis yAxis = new NumericalAxis();
	///     
	///     chart.XAxes.Add(xAxis);
	///     chart.YAxes.Add(yAxis);
	///     
	///     ViewModel viewModel = new ViewModel();
	/// 
	///     SplineAreaSeries series = new SplineAreaSeries();
	///     series.ItemsSource = viewModel.Data;
	///     series.XBindingPath = "XValue";
	///     series.YBindingPath = "YValue";
	///     chart.Series.Add(series);
	///     
	/// ]]></code>
	/// # [ViewModel](#tab/tabid-3)
	/// <code><![CDATA[
	///     public ObservableCollection<Model> Data { get; set; }
	/// 
	///     public ViewModel()
	///     {
	///        Data = new ObservableCollection<Model>();
	///        Data.Add(new Model() { XValue = 10, YValue = 100 });
	///        Data.Add(new Model() { XValue = 20, YValue = 150 });
	///        Data.Add(new Model() { XValue = 30, YValue = 110 });
	///        Data.Add(new Model() { XValue = 40, YValue = 230 });
	///     }
	/// ]]></code>
	/// ***
	/// </example>
	/// <seealso cref="SplineAreaSegment"/>
	/// <seealso cref="SplineSeries"/>
	/// <seealso cref="AreaSeries"/>
	public partial class SplineAreaSeries : AreaSeries
	{
		#region Properties
		/// <summary>
		/// 
		/// </summary>        
		public static readonly BindableProperty TypeProperty =
			BindableProperty.Create(
				nameof(Type),
				typeof(SplineType),
				typeof(SplineSeries),
				SplineType.Natural,
				BindingMode.Default,
				null,
				OnSplineTypeChanged);

		/// <summary>
		/// 
		/// </summary>
		public SplineType Type
		{
			get { return (SplineType)GetValue(TypeProperty); }
			set { SetValue(TypeProperty, value); }
		}
		#endregion

		#region Methods

		#region Protected Methods

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		protected override ChartSegment CreateSegment()
		{
			return new SplineAreaSegment();
		}

		#endregion

		#region Internal Methods

		/// <summary>
		/// Creates the segments of <see cref="SplineAreaSeries"/>.
		/// </summary>
		internal override void GenerateSegments(SeriesView seriesView)
		{
			var xValues = GetXValues();
			if (xValues == null || xValues.Count == 0)
			{
				return;
			}

			double[] startControlPointsX = new double[PointsCount - 1];
			double[] startControlPointsY = new double[PointsCount - 1];
			double[] endControlPointsX = new double[PointsCount - 1];
			double[] endControlPointsY = new double[PointsCount - 1];
			double[]? dx = null;

			double[]? yCoef;

			if (Type == SplineType.Monotonic)
			{
				yCoef = GetMonotonicSpline(xValues, YValues, out dx);
			}
			else if (Type == SplineType.Cardinal)
			{
				yCoef = GetCardinalSpline(xValues, YValues);
			}
			else
			{
				yCoef = NaturalSpline(YValues, Type);
			}

			if (yCoef == null)
			{
				return;
			}

			for (int i = 0; i < PointsCount - 1; i++)
			{
				var x = xValues[i];
				var y = YValues[i];

				var nextX = xValues[i + 1];
				var nextY = YValues[i + 1];

				List<double>? controlPoints;

				if (dx != null && Type == SplineType.Monotonic && dx.Length > 0)
				{
					controlPoints = CalculateControlPoints(x, y, nextX, nextY, yCoef[i], yCoef[i + 1], dx[i]);
				}
				else if (Type == SplineType.Cardinal)
				{
					controlPoints = CalculateControlPoints(x, y, nextX, nextY, yCoef[i], yCoef[i + 1]);
				}
				else
				{
					controlPoints = CalculateControlPoints(YValues, yCoef[i], yCoef[i + 1], i);
				}

				if (controlPoints != null)
				{
					startControlPointsX[i] = controlPoints[0];
					startControlPointsY[i] = controlPoints[1];
					endControlPointsX[i] = controlPoints[2];
					endControlPointsY[i] = controlPoints[3];
				}
			}

			CreateSegment(seriesView, xValues, YValues, startControlPointsX, startControlPointsY, endControlPointsX, endControlPointsY);
		}

		internal override void SetStrokeColor(ChartSegment segment)
		{
			segment.Stroke = Stroke;
		}

		internal override bool IsIndividualSegment()
		{
			return false;
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Add the <see cref="SplineAreaSegment"/> into the Segments collection.
		/// </summary>
		/// <param name="xVals">The x values.</param>
		/// <param name="yVals">The y values.</param>
		/// <param name="startPointsX">Start point x.</param>
		/// <param name="startPointsY">Start point y.</param>
		/// <param name="endPointsX">End point x.</param>
		/// <param name="endPointsY">End point y.</param>
		/// <param name="seriesView">The seriesview.</param>
		private void CreateSegment(SeriesView seriesView, IList<double> xVals, IList<double> yVals, double[] startPointsX, double[] startPointsY, double[] endPointsX, double[] endPointsY)
		{
			List<double>? xValues = null, yValues = null, startControlPointsX = null, startControlPointsY = null,
				endControlPointsX = null, endControlPointsY = null;

			for (int i = 0; i < PointsCount; i++)
			{
				if (!double.IsNaN(YValues[i]))
				{
					if (xValues == null)
					{
						xValues = new List<double>();
						yValues = new List<double>();
						startControlPointsX = new List<double>();
						startControlPointsY = new List<double>();
						endControlPointsX = new List<double>();
						endControlPointsY = new List<double>();
					}

					xValues.Add(xVals[i]);
					yValues?.Add(yVals[i]);

					if (i != PointsCount - 1)
					{
						startControlPointsX?.Add(startPointsX[i]);
						startControlPointsY?.Add(startPointsY[i]);
						endControlPointsX?.Add(endPointsX[i]);
						endControlPointsY?.Add(endPointsY[i]);
					}
				}

				if (double.IsNaN(YValues[i]) || i == PointsCount - 1)
				{
					if (xValues != null)
					{
						var segment = CreateSegment() as SplineAreaSegment;
						if (segment != null)
						{
							segment.Series = this;
							segment.SeriesView = seriesView;
							segment.SetData(xValues, yValues, startControlPointsX, startControlPointsY, endControlPointsX, endControlPointsY);
							Segments.Add(segment);
						}

						yValues = xValues = startControlPointsX = startControlPointsY = endControlPointsX = endControlPointsY = null;
					}

					if (double.IsNaN(YValues[i]))
					{
						xValues = new List<double> { xVals[i] };
						yValues = new List<double> { yVals[i] };
						startControlPointsX = new List<double>();
						startControlPointsY = new List<double>();
						endControlPointsX = new List<double>();
						endControlPointsY = new List<double>();

						var segment = (SplineAreaSegment)CreateSegment();
						segment.Series = this;
						segment.SeriesView = seriesView;
						segment.SetData(xValues, yValues, startControlPointsX, startControlPointsY, endControlPointsX, endControlPointsY);
						yValues = xValues = startControlPointsX = startControlPointsY = endControlPointsX = endControlPointsY = null;
					}
				}
			}
		}

		#endregion

		#region Callback Methods
		private static void OnSplineTypeChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var series = bindable as SplineSeries;
			if (series != null)
			{
				series.SegmentsCreated = false;
				series.ScheduleUpdateChart();
			}
		}
		#endregion

		#endregion
	}
}
