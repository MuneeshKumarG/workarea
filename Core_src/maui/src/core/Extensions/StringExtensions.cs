using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Graphics.Internals
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textSize"></param>
        /// <param name="attributes"></param>
        /// <param name="fontFamily"></param>
        /// <returns></returns>
        public static Size Measure(this string text, float textSize, FontAttributes attributes = FontAttributes.None, string? fontFamily = null)
        {
            return TextMeasurer.Instance.MeasureText(text, textSize, attributes, fontFamily);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textElement"></param>
        /// <returns></returns>
        public static Size Measure(this string text, ITextElement textElement)
        {
            return TextMeasurer.Instance.MeasureText(text, textElement);
        }
    }
}
