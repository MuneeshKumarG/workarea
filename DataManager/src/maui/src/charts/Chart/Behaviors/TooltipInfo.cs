using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Core;
using System.ComponentModel;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class TooltipInfo : INotifyPropertyChanged
    {
        #region Fields

        private int index;
        private object? item = null;
        private string text = string.Empty;
        private string fontFamily = string.Empty;
        private FontAttributes fontAttributes = FontAttributes.None;
        private Color textColor = Colors.White;
        private Brush? background = Brush.Black;
        private float fontSize = 12;
        private Thickness margin = new Thickness(0);

        #endregion

        #region Internal properties

        internal TooltipPosition Position { get; set; }

        internal Rect TargetRect { get; set; }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the x value for the tooltip position.
        /// </summary>
        public float X { get; internal set; }

        /// <summary>
        /// Gets the y value for the tooltip position.
        /// </summary>
        public float Y { get; internal set; }

        /// <summary>
        /// Gets the associated series.
        /// </summary>
        public readonly object Source;

        /// <summary>
        /// Gets or sets a value that displays on the tooltip.
        /// </summary>
        /// <value>It accepts string values.</value>
        public string Text
        {
            get { return text; }
            set
            {
                if (text == value)
                {
                    return;
                }

                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }

        /// <summary>
        /// Gets or sets a value to specify the FontFamily for the tooltip label.
        /// </summary>
        /// <value>It accepts string values.</value>
        public string FontFamily
        {
            get { return fontFamily; }
            set
            {
                if (fontFamily == value)
                {
                    return;
                }

                fontFamily = value;
                OnPropertyChanged(nameof(FontFamily));
            }
        }

        /// <summary>
        /// Gets or sets a value to specify the FontAttributes for the tooltip label.
        /// </summary>
        /// <value>It accepts <see cref="Microsoft.Maui.Controls.FontAttributes"/> values.</value>
        public FontAttributes FontAttributes
        {
            get { return fontAttributes; }
            set
            {
                if (fontAttributes == value)
                {
                    return;
                }

                fontAttributes = value;
                OnPropertyChanged(nameof(FontAttributes));
            }
        }

        /// <summary>
        /// Gets or sets the brush value to customize the tooltip background.
        /// </summary>
        /// <value>It accepts the <see cref="Brush"/> values.</value>
        public Brush? Background
        {
            get { return background; }

            set
            {
                if (background == value)
                {
                    return;
                }

                background = value;
                OnPropertyChanged(nameof(Background));
            }
        }

        /// <summary>
        /// Gets or sets the color value to customize the text color of the tooltip label.
        /// </summary>
        /// <value>It accepts the <see cref="Color"/> values.</value>
        public Color TextColor
        {
            get { return textColor; }

            set
            {
                if (textColor == value)
                {
                    return;
                }

                textColor = value;
                OnPropertyChanged(nameof(TextColor));
            }
        }

        /// <summary>
        /// Gets or sets a value to change the label's text size of the tooltip.
        /// </summary>
        /// <value>It accepts the float values and the default value is 12.</value>
        public float FontSize
        {
            get { return fontSize; }
            set
            {
                if (fontSize == value)
                {
                    return;
                }

                fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }

        /// <summary>
        /// Gets or sets a thickness value to adjust the tooltip margin.
        /// </summary>
        /// <value>It accepts the <see cref="Thickness"/> values and the default value is 0.</value>
        public Thickness Margin
        {
            get { return margin; }
            set
            {
                if (margin == value)
                {
                    return;
                }

                margin = value;
                OnPropertyChanged(nameof(Margin));
            }
        }

        /// <summary>
        /// Gets the index for the corresponding segment.
        /// </summary>
        public int Index
        {
            get { return index; }

            internal set
            {
                if (index == value)
                {
                    return;
                }

                index = value;
                OnPropertyChanged(nameof(Index));
            }
        }

        /// <summary>
        /// Gets the data object for the associated segment 
        /// </summary>
        public object? Item
        {
            get { return item; }

            internal set
            {
                if (item == value)
                {
                    return;
                }

                item = value;
                OnPropertyChanged(nameof(Item));
            }
        }

        #endregion

        #region Event

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="TooltipInfo"/> class.
        /// </summary>
        public TooltipInfo(object source)
        {
            Source = source;
        }

        #endregion

        #region Methods

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
