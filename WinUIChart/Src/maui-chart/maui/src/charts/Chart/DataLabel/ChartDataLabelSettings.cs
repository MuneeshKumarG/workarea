﻿using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.Collections.Generic;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ChartDataLabelSettings : BindableObject
    {
        #region Bindable Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty UseSeriesPaletteProperty =
            BindableProperty.Create(nameof(UseSeriesPalette), typeof(bool), typeof(ChartDataLabelSettings), true, BindingMode.Default, null, null);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty LabelStyleProperty =
            BindableProperty.Create(nameof(LabelStyle), typeof(ChartDataLabelStyle), typeof(ChartDataLabelSettings), null, BindingMode.Default, null, null);

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty LabelPlacementProperty =
            BindableProperty.Create(nameof(LabelPlacement), typeof(DataLabelPlacement), typeof(ChartDataLabelSettings), DataLabelPlacement.Auto, BindingMode.Default, null, null);

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public ChartDataLabelSettings()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public bool UseSeriesPalette
        {
            get { return (bool)GetValue(UseSeriesPaletteProperty); }
            set { SetValue(UseSeriesPaletteProperty, value); }
        }

        /// <summary>
		///
        /// </summary>
        public ChartDataLabelStyle LabelStyle
        {
            get { return (ChartDataLabelStyle)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public DataLabelPlacement LabelPlacement
        {
            get { return (DataLabelPlacement)GetValue(LabelPlacementProperty); }
            set { SetValue(LabelPlacementProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal List<string> IsNeedDataLabelMeasure { get; set; } = new List<string>() { nameof(LabelPlacement), nameof(LabelStyle)}; 

        #endregion

        #region Methods

        #region Protected Methods

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (LabelStyle != null)
            {
                SetInheritedBindingContext(LabelStyle, BindingContext);
            }
        }

        #endregion

        #region Internal DataLabel Calculation Methods

        internal Color GetContrastTextColor(ChartSeries series, Brush? background, Brush? segmentBrush)
        {
            Color textColor = Colors.Black;

            if (background != null && (background as SolidColorBrush)?.Color != Colors.Transparent)
            {
                textColor = background is SolidColorBrush ? ChartUtils.GetContrastColor((background as SolidColorBrush)?.Color) : textColor;
            }
            else
            {
                var cartesianLabelSettings = this as CartesianDataLabelSettings;

                if (LabelPlacement == DataLabelPlacement.Inner || LabelPlacement == DataLabelPlacement.Center || (cartesianLabelSettings != null && (cartesianLabelSettings.BarAlignment == DataLabelAlignment.Middle || cartesianLabelSettings.BarAlignment == DataLabelAlignment.Bottom)))
                {
                    textColor = series.IsIndividualSegment() ? segmentBrush is SolidColorBrush ? ChartUtils.GetContrastColor((segmentBrush as SolidColorBrush)?.Color) : textColor : GetTextColorBasedOnChartBackground(series?.Chart);
                }
                else if (LabelPlacement == DataLabelPlacement.Outer)
                {
                    textColor = GetTextColorBasedOnChartBackground(series?.Chart);
                }
            }

            return textColor;
        }

        internal PointF GetAutoLabelPosition(ChartSeries chartSeries, float x, float y, SizeF labelSize, float padding, float borderWidth)
        {
            if (chartSeries == null) return PointF.Zero;

            var scatter = chartSeries as ScatterSeries; 
            Size size = scatter != null ? new Size(scatter.PointWidth, scatter.PointHeight) : Size.Zero;
            float width = (float)size.Width;
            float height = (float)size.Height / 2;
            Rect clipRect = chartSeries.AreaBounds;

            if ((x - ((labelSize.Width / 2) + padding)) <= 0)
            {
                x = (labelSize.Width / 2) + padding + borderWidth + width;
            }
            else if ((x + ((labelSize.Width / 2) + padding)) >= clipRect.Width)
            {
                x = (float)clipRect.Width - (labelSize.Width / 2) - padding - borderWidth - width;
            }

            if ((y - ((labelSize.Height / 2) + padding)) <= 0)
            {
                y = (labelSize.Height / 2) + padding + borderWidth + height;
            }
            else if ((y + ((labelSize.Height / 2) + padding)) >= clipRect.Height)
            {
                y = (float)clipRect.Height - (labelSize.Height / 2) - padding - borderWidth - height;
            }

            return new PointF(x, y);
        }

        internal string GetLabelContent(double value)
        {
            string labelContent = string.Empty;

            if (double.IsNaN(value))
            {
                return labelContent;
            }

            if (LabelStyle != null && !string.IsNullOrEmpty(LabelStyle.LabelFormat))
            {
                labelContent = value.ToString(LabelStyle.LabelFormat);
            }
            else
                labelContent = value.ToString("#.##");

            return labelContent;
        }

        #endregion

        #region Private Method

        private Color GetTextColorBasedOnChartBackground(IChart? chart)
        {
            var backgroundColor = chart?.BackgroundColor;
            return backgroundColor != null && backgroundColor != Colors.Transparent ? ChartUtils.GetContrastColor(backgroundColor) : Colors.Black;
        }

        #endregion

        #endregion
    }
}