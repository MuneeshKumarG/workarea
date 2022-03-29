using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using System.Collections.ObjectModel;

namespace Syncfusion.Maui.Charts
{
    internal class SeriesView : DrawableView
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
            SetBinding(IsVisibleProperty, new Binding(nameof(chartSeries.IsVisible)));
        }

        #endregion

        #region Methods

        #region Override Methods

        public override void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();
            canvas.ClipRectangle(dirtyRect);

            if (Series != null && Series.IsVisible)
            {
                Series.DrawSeries(canvas, new ReadOnlyObservableCollection<ChartSegment>(Series.Segments), dirtyRect);
            }

            canvas.RestoreState();
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
                AddSelectionBehavior();
            }
        }

        private void AddSelectionBehavior()
        {
            if (Series.Chart != null && Series.SelectedIndex > -1
                && Series.PreviousSelectedIndex != Series.SelectedIndex
                && Series.Segments.Count > Series.SelectedIndex)
            {
                var selectionBehavior = Series.Chart.ActualSelectionBehavior;
                if (selectionBehavior != null)
                {
                    selectionBehavior.UpdateSelection(Series, Series.SelectedIndex);
                }
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

            if (Series.CanAnimate())
                StartAnimation();

            //Todo: Need to check alternate solution.
            if (Series is CircularSeries && Series.ShowDataLabels && !Series.NeedToAnimateSeries)
            {
                (Series as CircularSeries)?.ChangeIntersectedLabelPosition();
            }
        }

        private void OnAnimationStart(double value)
        {
            if (value >= 0.0)
            {
                Series.AnimationValue = (float)value;

                if (Series.NeedToAnimateSeries)
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
            }
            else
            {
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