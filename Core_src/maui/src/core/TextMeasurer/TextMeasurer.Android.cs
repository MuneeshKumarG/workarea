using Android.Graphics;
using Android.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Font = Microsoft.Maui.Font;

namespace Syncfusion.Maui.Graphics.Internals
{

    internal partial class TextMeasurer : ITextMeasurer
    {

        internal static partial ITextMeasurer CreateTextMeasurer()
        {
            return new TextMeasurer();
        }

        
        /// <summary>
        /// This method returns the text's measured size 
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="textSize">text size</param>
        /// <param name="attributes">attributes</param>
        /// <param name="fontFamily">font family</param>
        /// <returns>Measured size</returns>
        public Size MeasureText(string text, float textSize, FontAttributes attributes = FontAttributes.None, string? fontFamily = null)
        {
            TextPaint? paint = TextUtils.TextPaintCache;
            paint.Reset();
            if (fontFamily == null && attributes != FontAttributes.None)
            {
                var style = ToTypefaceStyle(attributes);
                TextUtils.TextPaintCache.SetTypeface(Typeface.Create(Typeface.Default, style));
            }
            //TODO: Calculate the size with embedded fonts.  

            paint.TextSize = textSize;
            Rect? bounds = TextUtils.BoundsCache;
            paint.GetTextBounds(text, 0, text.Length, bounds);
            return new Size(bounds.Width(), bounds.Height());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textElement"></param>
        /// <returns></returns>
        public Size MeasureText(string text, ITextElement textElement)
        {
            TextPaint? paint = TextUtils.TextPaintCache;
            paint.Reset();
            IFontManager? fontManager = MauiApplication.Current.Services.GetRequiredService<IFontManager>();
            Font font = textElement.Font;
            Typeface? tf = fontManager.GetTypeface(font);
            paint.SetTypeface(tf);
            paint.TextSize = (float)textElement.FontSize;
            Rect? bounds = TextUtils.BoundsCache;
            paint.GetTextBounds(text, 0, text.Length, bounds);
            return new Size(bounds.Width(), bounds.Height());
        }

        private static TypefaceStyle ToTypefaceStyle(FontAttributes attributes)
        {
            TypefaceStyle style = TypefaceStyle.Normal;
            if ((attributes & (FontAttributes.Bold | FontAttributes.Italic)) == (FontAttributes.Bold | FontAttributes.Italic))
            {
                style = TypefaceStyle.BoldItalic;
            }
            else if ((attributes & FontAttributes.Bold) != 0)
            {
                style = TypefaceStyle.Bold;
            }
            else if ((attributes & FontAttributes.Italic) != 0)
            {
                style = TypefaceStyle.Italic;
            }

            return style;
        }

    }


    internal static class TextUtils
    {
        internal static readonly TextPaint TextPaintCache = new TextPaint();

        internal static readonly Rect BoundsCache = new Rect();
    }
}
