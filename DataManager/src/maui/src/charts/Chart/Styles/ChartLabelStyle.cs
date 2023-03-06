﻿using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using Rect = Microsoft.Maui.Graphics.RectF;
using Font = Microsoft.Maui.Font;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// It is a base class for the <see cref="ChartAxisLabelStyle"/>, <see cref="ChartAxisTitle"/>, and <see cref="ChartDataLabelStyle"/> classes.
    /// </summary>
    /// <remarks>
    /// It provides more options to customize the label.
    /// 
    /// <para> <b>TextColor - </b> To customize the text color, refer to this <see cref="ChartLabelStyle.TextColor"/> property. </para>
    /// <para> <b>Background - </b> To customize the background color, refer to this <see cref="ChartLabelStyle.Background"/> property. </para>
    /// <para> <b>Stroke - </b> To customize the stroke color, refer to this <see cref="ChartLabelStyle.Stroke"/> property. </para>
    /// <para> <b>StrokeWidth - </b> To modify the stroke width, refer to this <see cref="ChartLabelStyle.StrokeWidth"/> property. </para>
    /// <para> <b>Margin - </b> To adjust the outer margin for labels, refer to this <see cref="Margin"/> property. </para>
    /// <para> <b>LabelFormat - </b> To customize the label format for labels, refer to this <see cref="LabelFormat"/> property. </para>
    /// <para> <b>CornerRadius - </b> To defines the rounded corners for labels, refer to this <see cref="CornerRadius"/> property. </para>
    /// <para> <b>CornerRadius - </b> To change the text size for labels, refer to this <see cref="FontSize"/> property. </para>
    /// <para> <b>FontFamily - </b> To change the font family for labels, refer to this <see cref="FontFamily"/> property. </para>
    /// <para> <b>FontAttributes - </b> To change the font attributes for labels, refer to this <see cref="FontAttributes"/> property. </para>
    /// </remarks>
    public class ChartLabelStyle : BindableObject, ITextElement
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="TextColor"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ChartLabelStyle), Color.FromRgba(170, 170, 170, 255), BindingMode.Default, null, OnTextColorChanged, null, null);

        /// <summary>
        /// Identifies the <see cref="Background"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty BackgroundProperty = BindableProperty.Create(nameof(Background), typeof(Brush), typeof(ChartLabelStyle), new SolidColorBrush(Colors.Transparent), BindingMode.Default, null, OnBackgroundtColorChanged, null, null);

        /// <summary>
        /// Identifies the <see cref="Margin"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty MarginProperty = BindableProperty.Create(nameof(Margin), typeof(Thickness), typeof(ChartLabelStyle), new Thickness(3.5), BindingMode.Default, null, OnMarginChanged, null, null);

        /// <summary>
        /// Identifies the <see cref="StrokeWidth"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeWidthProperty = BindableProperty.Create(nameof(StrokeWidth), typeof(double), typeof(ChartLabelStyle), 0d, BindingMode.Default, null, OnBorderThicknessChanged, null, null);

        /// <summary>
        /// Identifies the <see cref="Stroke"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty StrokeProperty = BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(ChartLabelStyle), SolidColorBrush.Transparent, BindingMode.Default, null, OnBorderColorChanged, null, null);

        /// <summary>
        /// Identifies the <see cref="FontSize"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty FontSizeProperty = FontElement.FontSizeProperty;

        /// <summary>
        /// Identifies the <see cref="FontFamily"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty FontFamilyProperty = FontElement.FontFamilyProperty;

        /// <summary>
        /// Identifies the <see cref="FontAttributes"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty FontAttributesProperty = FontElement.FontAttributesProperty;

        /// <summary>
        /// Identifies the <see cref="LabelFormat"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty LabelFormatProperty = BindableProperty.Create(nameof(LabelFormat), typeof(string), typeof(ChartLabelStyle), string.Empty, BindingMode.Default, null, OnLabelFormatChanged, null, null);

        /// <summary>
        /// Identifies the <see cref="CornerRadius"/> bindable property.
        /// </summary>        
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(ChartLabelStyle), new CornerRadius(0), BindingMode.Default, null, OnCornerRadiusChanged, null, null);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to customize the appearance of the label's text color.
        /// </summary>
        /// <value>It accepts <see cref="Color"/> values.</value>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the appearance of the label's background.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values.</value>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates the margin of the label.
        /// </summary>
        /// <value>It accepts <see cref="Thickness"/> values and the default value is 3.5.</value>
        public Thickness Margin
        {
            get { return (Thickness)GetValue(MarginProperty); }
            set { SetValue(MarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates the stroke thickness of the label.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values and the default value is 0.</value>
        public double StrokeWidth
        {
            get { return (double)GetValue(StrokeWidthProperty); }
            set { SetValue(StrokeWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the outer stroke appearance of the label.
        /// </summary>
        /// <value>It accepts <see cref="Brush"/> values.</value>
        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the label's format.
        /// </summary>
        /// <value>It accepts <see cref="string"/> values.</value>
        public string LabelFormat
        {
            get { return (string)GetValue(LabelFormatProperty); }
            set { SetValue(LabelFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates the label's size.
        /// </summary>
        /// <value>It accepts <see cref="double"/> values.</value>
        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates the font family of the label.
        /// </summary>
        /// <value>It accepts <see cref="string"/> values.</value>
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value that indicates the font attributes of the label.
        /// </summary>
        /// <value>It accepts <see cref="Microsoft.Maui.Controls.FontAttributes"/> values.</value>
        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
            set { SetValue(FontAttributesProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value to customize the rounded corners for labels.
        /// </summary>
        /// <value>It accepts <see cref="Microsoft.Maui.CornerRadius"/> values and the default value is 0.</value>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        #endregion

        #region Internal Properties

        internal bool IsBackgroundColorUpdated { get; set; }

        internal bool IsStrokeColorUpdated { get; set; }

        internal bool IsTextColorUpdated { get; set; }

        internal bool HasCornerRadius { get; set; }

        internal Rect Rect { get; set; } = new Rect();

        #endregion
        
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartLabelStyle"/>.
        /// </summary>
        public ChartLabelStyle()
        {
        }

        #endregion

        #region Methods

        internal bool CanDraw()
        {
            return IsBackgroundColorUpdated || IsStrokeColorUpdated;
        }

        #region DrawLabels

        internal void DrawBackground(ICanvas canvas, string label, Brush? fillcolor, PointF point)
        {
            canvas.SaveState();

            float xPos = point.X;
            float yPos = point.Y;
            SizeF labelSize = MeasureLabel(label);
            float actualWidth = labelSize.Width;
            float actualHeight = labelSize.Height;
            float leftMargin = (float)((actualWidth / 2) + (Margin.Left / 2) - (Margin.Right / 2));
            float topMargin = (float)((actualHeight / 2) + (Margin.Top / 2) - (Margin.Bottom / 2));
            Rect backgroundRect = new Rect(xPos - leftMargin, yPos - topMargin, actualWidth, actualHeight);

            canvas.SetFillPaint(fillcolor, backgroundRect);
            //Todo: Need to check condition for label background
            if (HasCornerRadius)
                canvas.FillRoundedRectangle(backgroundRect, CornerRadius.TopLeft, CornerRadius.TopRight, CornerRadius.BottomLeft, CornerRadius.BottomRight);
            else
                canvas.FillRectangle(backgroundRect);

            //Todo: Need to check with border width and color in DrawLabel override method.
            if (StrokeWidth > 0 && !ChartColor.IsEmpty(Stroke.ToColor()))
            {
                if (HasCornerRadius)
                    canvas.DrawRoundedRectangle(backgroundRect, CornerRadius.TopLeft, CornerRadius.TopRight, CornerRadius.BottomLeft, CornerRadius.BottomRight);
                else
                    canvas.DrawRectangle(backgroundRect);
            }

            canvas.RestoreState();
        }

        internal SizeF MeasureLabel(string label)
        {
            Size size = label.Measure(this);
            Rect = new Rect() { Height = (float)size.Height, Width = (float)size.Width };

            return new SizeF((float)(Rect.Width + (float)Margin.Left + (float)Margin.Right), (float)(Rect.Height + (float)Margin.Top + (float)Margin.Bottom));
        }

        internal void DrawLabel(ICanvas canvas, string label, PointF point)
        {
            //Addded platform specific code for position the label.
#if ANDROID
            canvas.DrawText(label, point.X - (Rect.Width / 2), point.Y + Rect.Height / 2, this);
#else
            canvas.DrawText(label, point.X - (Rect.Width / 2), point.Y - Rect.Height / 2, this);
#endif
        }

        #endregion

        #region Private Methods
        private static void OnTextColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var style = bindable as ChartLabelStyle;
            if (style != null)
            {
                var color = newValue as Color;
                style.IsTextColorUpdated = color != null & color != Colors.Transparent;
            }
        }

        private static void OnBackgroundtColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var style = bindable as ChartLabelStyle;
            if (style != null)
            {
                var color = newValue as Brush;
                style.IsBackgroundColorUpdated = color != null & color != null;
            }
        }

        private static void OnMarginChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnBorderThicknessChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ChartLabelStyle style)
            {
                var color = newValue as SolidColorBrush;
                style.IsStrokeColorUpdated = color != null & color != SolidColorBrush.Transparent;
            }
        }

        private static void OnLabelFormatChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var style = bindable as ChartLabelStyle;
            if (style != null)
            {
                var radius = (CornerRadius)newValue;
                style.HasCornerRadius = radius.TopLeft > 0 || radius.TopRight > 0 || radius.BottomLeft > 0 || radius.BottomRight > 0;
            }
        }

        #endregion

        #endregion

        #region implement interface
        Font ITextElement.Font => (Font)GetValue(FontElement.FontProperty);

        double ITextElement.FontSizeDefaultValueCreator()
        {
            return 9.5d;
        }

        void ITextElement.OnFontFamilyChanged(string oldValue, string newValue)
        {
           
        }

        void ITextElement.OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue)
        {

        }

        void ITextElement.OnFontChanged(Font oldValue, Font newValue)
        {

        }

        void ITextElement.OnFontSizeChanged(double oldValue, double newValue)
        {

        }
        #endregion

    }
}
