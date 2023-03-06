using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;
using System.Collections.Generic;
using System.ComponentModel;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// This class contains information about the trackball labels.
    /// </summary>
    public class TrackballPointInfo : INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler? PropertyChanged;

        #region Fields

        private string text = string.Empty;
        private object dataItem = string.Empty;

        #endregion

        #region Internal Properties
        internal Rect TargetRect { get; set; } = default(Rect);
        internal Rect Rect { get; set; } = default(Rect);
        internal float X { get; set; }
        internal float Y { get; set; }
        internal double XValue { get; set; }
        internal TooltipHelper TooltipHelper { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the series associated with the trackball.
        /// </summary>
        public CartesianSeries Series
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the content to be displayed on trackball tooltip
        /// </summary>
        public string Label
        {
            get
            {
                return text;
            }
            set
            {
                if (text != value)
                {
                    text = value;
                    OnPropertyChanged(nameof(Label));
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="TrackballPointInfo"/> associated business model. 
        /// </summary>
        public object DataItem
        {
            get
            {
                return dataItem;
            }
            internal set
            {
                if (dataItem != value)
                {
                    dataItem = value;
                    OnPropertyChanged(nameof(DataItem));
                }
            }
        }

        internal ChartLabelStyle? LabelStyle { get; set; }

        internal ChartMarkerSettings? MarkerSettings { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackballPointInfo"/>.
        /// </summary>
        public TrackballPointInfo(CartesianSeries series)
        {
            Series = series;
            TooltipHelper = new TooltipHelper(Drawable) { Duration = int.MaxValue };
        }

        #endregion

        #region Methods
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal void ShowTrackballLabel(SfCartesianChart cartesianChart, Rect bounds)
        {
            TooltipHelper.Text = Label;
            if (!cartesianChart.IsTransposed)
            {
                TooltipHelper.PriorityPosition = TooltipPosition.Right;
                TooltipHelper.PriorityPositionList = new List<TooltipPosition>() { TooltipPosition.Left, TooltipPosition.Top, TooltipPosition.Bottom };
            }
            else
            {
                TooltipHelper.PriorityPosition = TooltipPosition.Top;
                TooltipHelper.PriorityPositionList = new List<TooltipPosition>() { TooltipPosition.Bottom, TooltipPosition.Right, TooltipPosition.Left };

            }

            var plotAreaMargin = cartesianChart.ChartArea.PlotAreaMargin;
            Rect = new Rect(plotAreaMargin.Left, plotAreaMargin.Top, bounds.Width, bounds.Height);
            TooltipHelper.Show(Rect, TargetRect, false);
        }

        internal void SetTargetRect(IMarkerDependent markerDependent)
        {
            if (markerDependent.ShowMarkers)
            {
                var settings = MarkerSettings ?? markerDependent.MarkerSettings;
                var width = settings.Width; var height = settings.Height;
                TargetRect = new Rect(X - width / 2, Y - height / 2, width, height);
            }
            else
            {
                TargetRect = new Rect(X - 1, Y - 1, 2, 2);
            }
        }

        void Drawable()
        {

        }

        #endregion
    }
}
