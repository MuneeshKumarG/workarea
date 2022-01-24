using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Platform;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Graphics.Internals
{
    internal partial class TextMeasurer : ITextMeasurer
    {
        TextBlock? textBlock = null;

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
            if (textBlock == null)
                textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = textSize;
            textBlock.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
            return new Size((float)textBlock.DesiredSize.Width,(float)textBlock.DesiredSize.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textElement"></param>
        /// <returns></returns>
        public Size MeasureText(string text, ITextElement textElement)
        {
            IFontManager? fontManager = MauiWinUIApplication.Current.Services.GetRequiredService<IFontManager>();
            if (textBlock == null)
                textBlock = new TextBlock();
            textBlock.Text = text;
            textBlock.FontSize = textElement.FontSize;
            var font = textElement.Font;
            textBlock.FontFamily = fontManager.GetFontFamily(font);
            textBlock.FontStyle = font.ToFontStyle();
            textBlock.FontWeight = font.ToFontWeight();
            textBlock.Measure(new Windows.Foundation.Size(double.PositiveInfinity, double.PositiveInfinity));
            return new Size((float)textBlock.DesiredSize.Width, (float)textBlock.DesiredSize.Height);
        }
    }
}
