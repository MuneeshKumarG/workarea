using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using Syncfusion.Maui.Core.Internals;
using System;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class AreaBase : AbsoluteLayout, IArea
    {
        #region Fields

        private const double maxSize = 8388607.5;

        readonly CoreScheduler coreScheduler;

        private Rect areaBounds;

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public AreaBase()
        {
            coreScheduler = CoreScheduler.CreateScheduler(CoreSchedulerType.Frame);
        }

        #endregion

        #region Properties

        public Rect AreaBounds
        {
            get
            {
                return this.areaBounds;
            }
            set
            {
                if (areaBounds != value)
                {
                    this.areaBounds = value;
                    NeedsRelayout = true;
                }
            }
        }

        internal abstract IPlotArea PlotArea { get; }

        public bool NeedsRelayout { get; set; } = false;
       
        IPlotArea IArea.PlotArea => PlotArea;

        #endregion

        #region Methods 

        #region Protected Methods 

        protected virtual void UpdateAreaCore()
        {

        }

        protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
        {
            var size = this.ComputeDesiredSize(widthConstraint, heightConstraint);
            // Size.Height is 1.33 for windows platform, so checked condition with less than 1.
            bool isHeightNotContains = double.IsPositiveInfinity(heightConstraint) && Math.Round(size.Height) <= 1;
            bool isWidthNotContains = double.IsPositiveInfinity(widthConstraint) && Math.Round(size.Width) <= 1;

            if (isHeightNotContains || isWidthNotContains)
                DesiredSize = new Size(isWidthNotContains ? 300 : size.Width, isHeightNotContains ? 300 : size.Height);
            else
                DesiredSize = size;

            return DesiredSize;
        }

        protected override Size ArrangeOverride(Rect bounds)
        {
            if (!AreaBounds.Equals(bounds) && bounds.Width != maxSize && bounds.Height != maxSize)
            {
                AreaBounds = bounds;

                if (bounds.Width > 0 && bounds.Height > 0)
                {
                    ScheduleUpdateArea();
                }
            }

            return base.ArrangeOverride(bounds);
        }

        #endregion

        #region Internal Methods

        public void ScheduleUpdateArea()
        {
            if (NeedsRelayout && !areaBounds.IsEmpty)
                coreScheduler.ScheduleCallback(UpdateArea);
        }

        #endregion

        #region Private Method

        private void UpdateArea()
        {
            PlotArea.UpdateLegendItems();
            UpdateAreaCore();
            NeedsRelayout = false;
        }

        #endregion

        #endregion
    }

    internal interface IArea
    {
        Rect AreaBounds { get; set; }

        bool NeedsRelayout { get; set; }

        IPlotArea PlotArea { get; }

        void ScheduleUpdateArea();
    }
}
