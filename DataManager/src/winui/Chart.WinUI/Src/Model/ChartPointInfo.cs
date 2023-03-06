using System.ComponentModel;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;
using System;
using Microsoft.UI.Xaml;
using Windows.Foundation;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Represents the information for a single point in a chart.
    /// </summary>
  
    public class ChartPointInfo: INotifyPropertyChanged
    {
        #region Fields

        #region Private Fields

        private ChartSeries series;
        private ChartAxis axis;
        private string valueX;
        private string valueY;
        private string high;
        private string low;
        private string open;
        private string close;
        private string upperLine;
        private string lowerLine;
        private string signalLine;
        private Brush interior;
        private Brush foreground;
        private Brush borderBrush;
        private PointCollection polygonPoints;
        private object item;
        private ObservableCollection<string> seriesvalues;
        private ChartAlignment verticalAlignment;
        private ChartAlignment horizontalAlignment;
        private string median;

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets a collection of values for the chart's series.
        /// </summary>
        public ObservableCollection<string> SeriesValues
        {
            get
            {
                if (seriesvalues == null)
                {
                    seriesvalues = new ObservableCollection<string>();
                }
                return seriesvalues;
            }
            internal set
            {
                value = seriesvalues;
            }
        }
        
        /// <summary>
        /// Gets the series associated with this chart point.
        /// </summary>
        public ChartSeries Series
        {
            get
            {
                return series;
            }
            internal set
            {
                if (value != series)
                {
                    series = value;
                    OnPropertyChanged("Series");
                }
            }
        }

        /// <summary>
        /// Gets the axis associated with this chart point.
        /// </summary>
        public ChartAxis Axis
        {
            get
            {
                return axis;
            }
            internal set
            {
                axis = value;
            }
        }
        
        /// <summary>
        /// Gets the data object for the chart point.
        /// </summary>
        public object Item
        {
            get
            {
                return item;
            }
            internal set
            {
                item = value;
                OnPropertyChanged("Item");
            }
        }

        /// <summary>
        /// Gets the brush used to fill the interior of a chart point.
        /// </summary>
        public Brush Interior
        {
            get
            {
                return interior;
            }
            internal set
            {
                interior = value;
                OnPropertyChanged("Interior");
            }
        }

        /// <summary>
        /// Gets the brush used to color the text of a chart point.
        /// </summary>
        public Brush Foreground
        {
            get
            {
                return foreground;
            }
            internal set
            {
                foreground = value;
                OnPropertyChanged("Foreground");
            }
        }

        /// <summary>
        /// Gets the brush used to color the border of a chart point.
        /// </summary>
        public Brush BorderBrush
        {
            get
            {
                return borderBrush;
            }
            internal set
            {
                borderBrush = value;
                OnPropertyChanged("BorderBrush");
            }
        }
        
        /// <summary>
        /// Gets the x-value that is displayed on the chart point.
        /// </summary>
        public string ValueX
        {
            get
            {
                return valueX;
            }
            internal set
            {
                if (value != valueX)
                {
                    valueX = value;
                    OnPropertyChanged("ValueX");
                }
            }
        }

        /// <summary>
        /// Gets the y-value that is displayed on the chart point.
        /// </summary>
        public string ValueY
        {
            get
            {
                return valueY;
            }
            internal set
            {
                if (value != valueY)
                {
                    valueY = value;
                    OnPropertyChanged("ValueY");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        internal string High
        {
            get
            {
                return high;
            }
            set
            {
                if (value != high)
                {
                    high = value;
                    OnPropertyChanged("High");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal string Low
        {
            get
            {
                return low;
            }
            set
            {
                if (value != low)
                {
                    low = value;
                    OnPropertyChanged("Low");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal string Open
        {
            get
            {
                return open;
            }
            set
            {
                if (value != open)
                {
                    open = value;
                    OnPropertyChanged("Open");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        internal string Close
        {
            get
            {
                return close;
            }
            set
            {
                if (value != close)
                {
                    close = value;
                    OnPropertyChanged("Close");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal string Median
        {
            get
            {
                return median;
            }
            set
            {
                if (value != median)
                {
                    median = value;
                    OnPropertyChanged("Median");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        internal string UpperLine
        {
            get
            {
                return upperLine;
            }
            set
            {
                if (value != upperLine)
                {
                    upperLine = value;
                    OnPropertyChanged("UpperLine");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal string LowerLine
        {
            get
            {
                return lowerLine;
            }
            set
            {
                if (value != lowerLine)
                {
                    lowerLine = value;
                    OnPropertyChanged("LowerLine");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        internal string SignalLine
        {
            get
            {
                return signalLine;
            }
            set
            {
                if (value != signalLine)
                {
                    signalLine = value;
                    OnPropertyChanged("SignalLine");
                }
            }
        }

        /// <summary>
        /// Gets the base x-coordinate for a chart point.
        /// </summary>
        public double BaseX
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the base x-coordinate for a chart point.
        /// </summary>
        public double BaseY
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the x-coordinate of a chart point.
        /// </summary>
        public double X
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the y-coordinate of a chart point.
        /// </summary>
        public double Y
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets the collection of points that define a polygon shape.
        /// </summary>
        public PointCollection PolygonPoints
        {
            get
            {
                return polygonPoints;
            }
            internal set
            {
                polygonPoints = value;
                OnPropertyChanged("PolygonPoints");
            }
        }

        #endregion

        #region Internal Properties

        internal ChartAlignment VerticalAlignment
        {
            get { return verticalAlignment; }
            set { verticalAlignment = value; }
        }

        internal ChartAlignment HorizontalAlignment
        {
            get { return horizontalAlignment; }
            set { horizontalAlignment = value; }
        }

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Define the members that used in SfChart data.
    /// </summary>
    internal class ChartDataPointInfo : ChartSegment
    {
        #region Fields

        #region Public Fields

        /// <summary>
        /// Define the index of the data point.
        /// </summary>
        public int Index = -1;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Gets or sets the x-axis data of the series data point.
        /// </summary>
        public double XData { get; set; }

        /// <summary>
        /// Gets or sets the y-axis data of the XY data series data point.
        /// </summary>
        public double YData { get; set; }

        #endregion

        #endregion

        #region Public Override Methods

        /// <summary>
        /// Method implementation for CreateVisual method.
        /// </summary>
        /// <param name="size">Size</param>
        /// <returns>UIElement</returns>
        internal override UIElement CreateVisual(Size size)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method implementation for GetRenderedVisual method.
        /// </summary>
        /// <returns>UIElement</returns>
        internal override UIElement GetRenderedVisual()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method implementation for Update method.
        /// </summary>
        /// <param name="transformer"></param>
        internal override void Update(IChartTransformer transformer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method implementation for OnSizeChanged method.
        /// </summary>
        /// <param name="size">Size</param>
        internal override void OnSizeChanged(Size size)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
