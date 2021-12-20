using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System.ComponentModel;

namespace Syncfusion.Maui.Graphics.Internals
{

    /// <summary>
    /// 
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface ITextElement
    {
        /// <summary>
        /// 
        /// </summary>
        FontAttributes FontAttributes { get; }

        /// <summary>
        /// 
        /// </summary>
        string FontFamily { get; }

        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
        double FontSize { get; }

        /// <summary>
        /// Gets the font family, style and size of the font.
        /// </summary>
        Font Font { get; }

        /// <summary>
        /// Gets the text color.
        /// </summary>
        Color TextColor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        void OnFontFamilyChanged(string oldValue, string newValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        void OnFontSizeChanged(double oldValue, double newValue);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        double FontSizeDefaultValueCreator();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        void OnFontAttributesChanged(FontAttributes oldValue, FontAttributes newValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        void OnFontChanged(Font oldValue, Font newValue);
    }
}
