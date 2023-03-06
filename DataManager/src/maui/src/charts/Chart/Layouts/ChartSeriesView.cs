using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    internal class SeriesView : SfDrawableView
    {
        #region Fields

        const string animationName = "ChartAnimation";

        internal readonly ChartSeries Series;

        readonly ChartPlotArea chartPlotArea;

        #endregion

        #region Constructor

        internal SeriesView(ChartSeries chartSeries, ChartPlotArea plotArea)
        {
            Series = chartSeries;
            chartPlotArea = plotArea;
            SetBinding(IsVisibleProperty, new Binding() { Source = chartSeries, Path = ChartSeries.IsVisibleProperty.PropertyName });
        }

        #endregion

        #region Methods

        #region Override Methods

        protected override void OnDraw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            canvas.ClipRectangle(dirtyRect);

            if (Series != null)
            {
                Series.DrawSeries(canvas, new ReadOnlyObservableCollection<ChartSegment>(Series.Segments), dirtyRect);
            }

            canvas.RestoreState();

            if (Series is IMarkerDependent series && !Series.NeedToAnimateSeries && series.ShowMarkers && series.MarkerSettings != null)
            {
                canvas.SaveState();
                canvas.ClipRectangle(dirtyRect);
                canvas.Alpha = Series.AnimationValue;
                series.OnDrawMarker(canvas, new ReadOnlyObservableCollection<ChartSegment>(Series.Segments), dirtyRect);
                canvas.RestoreState();
            }
        }

        protected override Size ArrangeOverride(Rect bounds)
        {
            //Will be removed AreaBounds property, after the alternative way.
            //Series.AreaBounds = bounds;
            return base.ArrangeOverride(bounds);
        }

        #endregion

        #region Internal Methods

        internal void UpdateSeries()
        {
            InternalCreateSegments();
            Invalidate();

            if (!Series.NeedToAnimateSeries)
                InvalidateDrawable();
        }

        internal void Animate()
        {
            if (Series.EnableAnimation && Series.SeriesAnimation == null)
            {
                Series.SeriesAnimation = new Animation(OnAnimationStart);
            }
            else
                AbortAnimation();

            if (Series.OldSegments != null)
            {
                Series.OldSegments.Clear();
                Series.OldSegments = null;
            }

            Series.NeedToAnimateSeries = true;

            if (Series.ShowDataLabels)
            {
                //Disable data label before the series dynamic animation.
                chartPlotArea.DataLabelView.InvalidateDrawable();
            }

            StartAnimation();
            InvalidateDrawable();
        }

        internal void AbortAnimation()
        {
            this.AbortAnimation(animationName);
            Series.NeedToAnimateSeries = false;
            Series.IsDataPointAddedDynamically = false;
        }

        #endregion

        #region Private Methods

        internal void InternalCreateSegments()
        {
            if (!Series.SegmentsCreated)
            {
                if (Series.Segments.Count != 0)
                {
                    Series.Segments.Clear();
                }

                Series.GenerateSegments(this);

                if (Series.OldSegments != null)
                {
                    Series.OldSegments.Clear();
                    Series.OldSegments = null;
                }

                Series.SegmentsCreated = true;
            }
        }

        internal void Invalidate()
        {
            foreach (var segment in Series.Segments)
            {
                segment.OnLayout();
                
                if (Series.ShowDataLabels && !Series.NeedToAnimateSeries)
                {
                    segment.OnDataLabelLayout();
                }                
            }

            Series.OnSeriesLayout();

            if (Series.CanAnimate())
                StartAnimation();

            //Todo: Need to check alternate solution.
            if (Series is CircularSeries circular && circular.ShowDataLabels && !circular.NeedToAnimateSeries)
            {
                circular.ChangeIntersectedLabelPosition();
            }
        }

        private void OnAnimationStart(double value)
        {
            if (value >= 0.0)
            {
                Series.AnimationValue = (float)value;

                if (Series.NeedToAnimateSeries || (Series is IMarkerDependent series && series.NeedToAnimateMarker))
                {
                    InvalidateDrawable();
                }
                else if (Series.NeedToAnimateDataLabel)
                {
                    chartPlotArea.DataLabelView.InvalidateDrawable();
                }
            }
        }

        private void OnAnimationFinished(double value, bool isCompleted)
        {
            if (Series.NeedToAnimateSeries)
            {
                Series.NeedToAnimateSeries = false;
                Invalidate();
                AbortAnimation();

                if (Series.ShowDataLabels)
                {
                    Series.NeedToAnimateDataLabel = true;
                    Series.SeriesAnimation?.Commit(this, animationName, 16, 1000, null, OnAnimationFinished, () => false);
                }

                if (Series is IMarkerDependent series && series.ShowMarkers)
                {
                    series.NeedToAnimateMarker = true;
                    Series.SeriesAnimation?.Commit(this, animationName, 16, 1000, null, OnAnimationFinished, () => false);
                }
            }
            else
            {
                if (Series is IMarkerDependent series && series.ShowMarkers)
                {
                    series.NeedToAnimateMarker = false;
                }

                Series.NeedToAnimateDataLabel = false;
                AbortAnimation();
            }
        }

        private void StartAnimation()
        {
            //Todo: Need to move this code to series propertychanged event. Fow now added here.
            if (Series.EnableAnimation && Series.SeriesAnimation == null)
            {
                Series.SeriesAnimation = new Animation(OnAnimationStart);
            }
            else if (!Series.EnableAnimation && Series.SeriesAnimation != null)
            {
                AbortAnimation();
            }

            if (Series.SeriesAnimation != null)
            {
                Series.AnimateSeries(OnAnimationStart);
                Series.SeriesAnimation.Commit(this, animationName, 16, (uint)(Series.AnimationDuration * 1000), null, OnAnimationFinished, () => false);
            }
        }

        #endregion

        #endregion
    }
}