using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class ChartSegment : INotifyPropertyChanged
    {
        #region Fields

        private Brush? fill;
        private Brush? stroke;
        private double strokeWidth = 1;
        private bool isVisible = true;
        private DoubleCollection? strokeDashArray;
        //Todo: Need to change this pixel value for mouse move state.
        private const int touchPixel = 10;
        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        public Brush? Fill
        {
            get { return fill; }
            set
            {
                fill = value;
                //Todo://
                // Series?.InvalidateRender();
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ChartSeries? Series { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public Brush? Stroke
        {
            get { return stroke; }

            set
            {
                stroke = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double StrokeWidth
        {
            get { return strokeWidth; }

            set
            {
                strokeWidth = value;
                NotifyPropertyChanged();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public DoubleCollection? StrokeDashArray
        {
            get { return strokeDashArray; }

            set
            {
                strokeDashArray = value;
                NotifyPropertyChanged();
            }
        }

        private float opacity = 1;

        /// <summary>
        /// 
        /// </summary>
        public float Opacity
        {
            get { return opacity; }
            set
            {
                if (opacity == value || value < 0 || value > 1)
                {
                    return;
                }

                opacity = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public float AnimatedValue
        {
            get
            {
                if (Series != null)
                {
                    return Series.AnimationValue;
                }

                return 0;
            }
        }
        #endregion

        #region Internal Properties

        internal bool IsVisible
        {
            get { return isVisible; }
            set
            {
                if (value == isVisible)
                {
                    return;
                }

                isVisible = value;
            }
        }

        internal bool Empty { get; set; } = false;

        internal RectF SegmentBounds { get; set; }

        internal bool HasStroke
        {
            get { return StrokeWidth > 0 && !ChartColor.IsEmpty(Stroke.ToColor()); }
        }

        internal bool IsSelected { get; set; } = false;

        internal int Index { get; set; }

        internal double DataLabelXPosition { get; set; }

        internal double DataLabelYPosition { get; set; }

        internal string? DataLabel { get; set; }

        internal List<float> XPoints { get; set; }

        internal List<float> YPoints { get; set; }

        internal bool InVisibleRange { get; set; } = true;

        internal PointF LabelPositionPoint { get; set; }

        internal bool IsEmpty { get; set; }

        internal SeriesView? SeriesView { get; set; }

        #endregion

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartSegment()
        {
            XPoints = new List<float>();
            YPoints = new List<float>();
        }

        #endregion

        #region Methods

        #region Protected Methods
        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void OnLayout()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected internal virtual void Draw(ICanvas canvas)
        {
            
        }

        internal virtual void SetData(double[] values)
        {
        }

        internal virtual void SetData(double[] values, bool isTrue)
        {
        }

        internal virtual void SetData(IList xValues, IList yValues)
        {
        }

        internal virtual void SetData(double[] values, bool isTrue, bool isFalse)
        {
        }

        internal virtual void SetData(IList xValues, IList yValues, IList startControlPointsX, IList startControlPointsY, IList endControlPointsX, IList endControlPointsY)
        {
        }

        internal virtual int HitTest(float valueX, float valueY)
        {
            return GetDataPointIndex(valueX, valueY);
        }

        internal virtual int GetDataPointIndex(float valueX, float valueY)
        {
            return -1;
        }

        internal static bool IsRectContains(float valueX1, float valueY1, float valueX2, float valueY2, float valueX, float valueY)
        {
            return valueX1 < valueX && valueX < valueX2 && valueY1 < valueY && valueY < valueY2;
        }

        internal static bool IsRectContains(float xPoint, float yPoint, float valueX, float valueY, float strokeWidth)
        {
			float depth = (strokeWidth < touchPixel) ? touchPixel : strokeWidth;
            float x1 = xPoint - depth;
            float y1 = yPoint - depth;
            float x2 = xPoint + depth;
            float y2 = yPoint + depth;
            return x1 < valueX && valueX < x2 && y1 < valueY && valueY < y2;
        }

        internal static bool IsRectContains(float x1Point, float y1Point, float x2Point, float y2Point, float valueX, float valueY, float StrokeWidth)
        {
            float depth = (StrokeWidth < touchPixel) ? touchPixel : StrokeWidth;
            float x1 = x1Point - depth;
            float y1 = y1Point - depth;
            float x2 = x2Point + depth;
            float y2 = y2Point + depth;
            return x1 < valueX && valueX < x2 && y1 < valueY && valueY < y2;
        }

        internal virtual void OnDataLabelLayout()
        {
            
        }

        #endregion

        #region Private Methods

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #endregion
    }

}
