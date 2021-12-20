using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Graphics.Internals
{
    internal partial class TextMeasurer : ITextMeasurer
    {
        public Size MeasureText(string text, float textSize, FontAttributes attributes = FontAttributes.None, string? fontFamily = null)
        {
            throw new NotImplementedException();
        }

        public Size MeasureText(string text, ITextElement textElement)
        {
            throw new NotImplementedException();
        }

        internal static partial ITextMeasurer CreateTextMeasurer()
        {
            throw new NotImplementedException();
        }
    }
}
