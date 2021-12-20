using CoreGraphics;
using Foundation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using UIKit;

namespace Syncfusion.Maui.Graphics.Internals
{
    internal partial class TextMeasurer : ITextMeasurer
    {
        UIStringAttributes? uiStringAttributes;

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
            NSString? nsText = new NSString(text);

            UIFont? font = UIFont.SystemFontOfSize(textSize);
            if (uiStringAttributes == null)
                uiStringAttributes = new UIStringAttributes();
            if (fontFamily == null && attributes != FontAttributes.None)
                font = GetNativeFont(textSize, attributes);
            uiStringAttributes.Font = font;
            //TODO: Calculate the size with embedded fonts.
            CGSize sizeF = nsText.GetSizeUsingAttributes(uiStringAttributes);
            font.Dispose();
            nsText.Dispose();
            return new Size(sizeF.Width, sizeF.Height - 5);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textElement"></param>
        /// <returns></returns>
        public Size MeasureText(string text, ITextElement textElement)
        {
            NSString? nsText = new NSString(text);
            if (uiStringAttributes == null)
                uiStringAttributes = new UIStringAttributes();
            IFontManager? fontManager = MauiUIApplicationDelegate.Current.Services.GetRequiredService<IFontManager>();
            UIFont? uiFont = fontManager.GetFont(textElement.Font, textElement.FontSize);
            uiStringAttributes.Font = uiFont;
            CGSize sizeF = nsText.GetSizeUsingAttributes(uiStringAttributes);
            nsText.Dispose();
            return new Size(sizeF.Width, sizeF.Height);
        }

        private static UIFont GetNativeFont(float size, FontAttributes attributes)
        {
            bool bold = (attributes & FontAttributes.Bold) != 0;
            bool italic = (attributes & FontAttributes.Italic) != 0;
            if (bold && italic)
            {
                UIFont? defaultFont = UIFont.SystemFontOfSize(size);
                UIFontDescriptor? descriptor = defaultFont.FontDescriptor.CreateWithTraits(UIFontDescriptorSymbolicTraits.Bold | UIFontDescriptorSymbolicTraits.Italic);
                return UIFont.FromDescriptor(descriptor, 0);
            }

            if (italic)
                return UIFont.ItalicSystemFontOfSize(size);

            if (bold)
                return UIFont.BoldSystemFontOfSize(size);

            return UIFont.SystemFontOfSize(size);
        }
    }
}
