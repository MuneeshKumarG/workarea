using System;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace Syncfusion.UI.Xaml.Charts
{
    /// <summary>
    /// Converts the angle value by series IsTransposed.
    /// </summary>
    public class ConnectorRotationAngleConverter : IValueConverter
    {
        private ChartSeries series;

        /// <summary>
        /// Called when instance created for ConnectorRotationAngleConverter
        /// </summary>
        /// <param name="series"></param>
        public ConnectorRotationAngleConverter(ChartSeries series)
        {
            this.series = series;
        }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns>The value to be passed to the target dependency property.</returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double angle = (double)value;
            if (double.IsNaN(angle))
            {
                if (series.IsActualTransposed)
                    return 0;
                else
                    return 90;
            }
            else
                return value;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns>The value to be passed to the source object.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

    /// <summary>
    /// Resolves the color of the series or segment dynamically.
    /// </summary>
    
    public class InteriorConverter : IValueConverter
    {
        private ChartSeries series;

        /// <summary>
        /// Called when instance created for InteriorConverter
        /// </summary>
        /// <param name="series"></param>
        public InteriorConverter(ChartSeries series)
        {
            this.series = series;
        }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int index = int.Parse(parameter.ToString());
            return ChartExtensionUtils.GetInterior(series, index);
        }

        /// <summary>
        ///  Modifies the target data before passing it to the source object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

    /// <summary>
    /// Resolves the SegmentSelectionBrush of the series dynamically.
    /// </summary>
    
    public class SegmentSelectionBrushConverter : IValueConverter
    {
        private ChartSeries series;

        /// <summary>
        /// Called when instance created for InteriorConverter
        /// </summary>
        /// <param name="series"></param>
        public SegmentSelectionBrushConverter(ChartSeries series)
        {
            this.series = series;
        }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
                return value;
            
            if(series !=null)
            { 
                if (series.Fill != null)
                    return series.Fill;
                else if (series.PaletteBrushes != null)
                {
                    int segmentIndex = int.Parse(parameter.ToString());
                    if (segmentIndex != -1)
                        return series.PaletteBrushes[segmentIndex % series.PaletteBrushes.Count];
                }
                else if (series.ActualArea != null
                    && !series.ActualArea.IsNullPaletteBrushes())
                {
                    int serIndex = series.ActualArea.GetSeriesIndex(series);
                    if (serIndex >= 0)
                        return series.ActualArea.GetPaletteBrush(serIndex);
                }
            }
            
            return null;
        }

        /// <summary>
        ///  Modifies the target data before passing it to the source object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

    /// <summary>
    /// Resolves the SeriesSelectionBrush of the series dynamically.
    /// </summary>
   
    public class SeriesSelectionBrushConverter : IValueConverter
    {
        private ChartSeries series;

        /// <summary>
        /// Called when instance created for InteriorConverter
        /// </summary>
        /// <param name="series"></param>
        public SeriesSelectionBrushConverter(ChartSeries series)
        {
            this.series = series;
        }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (series != null)
            {
                Brush brush=series.ActualArea.GetSeriesSelectionBrush(series);
                if (brush != null)
                    return brush;
                else if (series.Fill != null)
                    return series.Fill;
                else if (series.PaletteBrushes != null)
                {
                    int segmentIndex = int.Parse(parameter.ToString());
                    if (segmentIndex != -1)
                        return series.PaletteBrushes[segmentIndex % series.PaletteBrushes.Count];
                }
                else if (series.ActualArea != null
                     && !series.ActualArea.IsNullPaletteBrushes())
                {
                    int serIndex = series.ActualArea.GetSeriesIndex(series);
                    if (serIndex >= 0)
                        return series.ActualArea.GetPaletteBrush(serIndex);
                }
            }

            return new SolidColorBrush(Colors.Transparent);
        }

        /// <summary>
        ///  Modifies the target data before passing it to the source object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

    /// <summary>
    /// Returns the brush to be used based on the <see cref="ChartSeries.Fill"/> property value.
    /// </summary>
  
    internal class MultiInteriorConverter : IValueConverter
    {

        /// <summary>
        ///  Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return parameter;
            return value;
        }

        /// <summary>
        ///  Modifies the target data before passing it to the source object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

    /// <summary>
    /// Modifies the chart adornment label based on content path. 
    /// </summary>
     public class LabelContentPathConverter : IValueConverter
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            ChartDataLabel adornment = value as ChartDataLabel;
            if (adornment == null) return value;

            return adornment.GetTextContent();          
        }

        /// <summary>
        ///  Modifies the target data before passing it to the source object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

    /// <summary>
    /// Represents a converter that returns the brush to axis label. 
    /// </summary>
    /// <seealso cref="IValueConverter" />
    public class LabelBackgroundConverter : IValueConverter
    {
         /// <summary>
         /// Modifies the source data before passing it to the target for display in the UI.
         /// </summary>
         /// <param name="value"></param>
         /// <param name="targetType"></param>
         /// <param name="parameter"></param>
         /// <param name="language"></param>
         /// <returns></returns>
         public object Convert(object value, Type targetType, object parameter, string language)
        {
            ChartDataLabel adornment = value as ChartDataLabel;

            if (adornment == null)
                return value;
            if (adornment.CanHideLabel)
                return new SolidColorBrush(Colors.Transparent);
            else if (adornment.Series.ActualArea.SelectedSeriesCollection.Contains(adornment.Series)
                && adornment.Series.ActualArea.GetSeriesSelectionBrush(adornment.Series) != null
                && adornment.Series.adornmentInfo.HighlightOnSelection && adornment.Series is ChartSeries
                && adornment.Series.ActualArea.GetEnableSeriesSelection()
                && (adornment.Series.adornmentInfo.UseSeriesPalette || adornment.Background != null))
                return adornment.Series.ActualArea.GetSeriesSelectionBrush(adornment.Series);
            else if (IsAdornmentSelected(adornment) && adornment.Series.SelectionBehavior != null)
                return adornment.Series.SelectionBehavior.SelectionBrush;
            else if (adornment.Series.adornmentInfo.UseSeriesPalette && adornment.Background == null)
                return adornment.Fill;

            return adornment.Background;
        }

        /// <summary>
        /// Method used to get the given adornment is selected or not
        /// </summary>
        /// <param name="adornment"></param>
        /// <returns></returns>
        private static bool IsAdornmentSelected(ChartDataLabel adornment)
        {
            return adornment.Series.SelectedSegmentsIndexes.Contains(adornment.Series.ActualData.IndexOf(adornment.Item))
                   && adornment.Series.adornmentInfo.HighlightOnSelection
                   && adornment.Series.SelectionBehavior != null && adornment.Series.SelectionBehavior.SelectionBrush != null
                   && (adornment.Series.adornmentInfo.UseSeriesPalette || adornment.Background != null);
        }

         /// <summary>
         ///  Modifies the target data before passing it to the source object
         /// </summary>
         /// <param name="value"></param>
         /// <param name="targetType"></param>
         /// <param name="parameter"></param>
         /// <param name="language"></param>
         /// <returns></returns>
         /// <exception cref="NotImplementedException"></exception>
         public object ConvertBack(object value, Type targetType, object parameter, string language)
         {
            return value;
         }
    }

    /// <summary>
    /// Sets the chart adornment border brush. 
    /// </summary>
    public class LabelBorderBrushConverter : IValueConverter
    {
         /// <summary>
         /// Modifies the source data before passing it to the target for display in the UI.
         /// </summary>
         /// <param name="value"></param>
         /// <param name="targetType"></param>
         /// <param name="parameter"></param>
         /// <param name="language"></param>
         /// <returns></returns>
         public object Convert(object value, Type targetType, object parameter, string language)
        {
            ChartDataLabel adornment = value as ChartDataLabel;
            if (adornment == null)
                return value;

            ChartSeries series = adornment.Series;
            ChartBase area = series.ActualArea;
           
            if (area.SelectedSeriesCollection.Contains(series)
                && area.GetSeriesSelectionBrush(series) != null && series is ChartSeries
                && series.adornmentInfo.HighlightOnSelection && area.GetEnableSeriesSelection())
                return area.GetSeriesSelectionBrush(series);
            else if (series.SelectedSegmentsIndexes.Contains(series.ActualData.IndexOf(adornment.Item))
               && series.adornmentInfo.HighlightOnSelection
               && series.SelectionBehavior != null
               && series.SelectionBehavior.SelectionBrush != null)
                return series.SelectionBehavior.SelectionBrush;
            else
                return adornment.BorderBrush;
        }

         /// <summary>
         ///  Modifies the target data before passing it to the source object
         /// </summary>
         /// <param name="value"></param>
         /// <param name="targetType"></param>
         /// <param name="parameter"></param>
         /// <param name="language"></param>
         /// <returns></returns>
         /// <exception cref="NotImplementedException"></exception>
         public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }

}
