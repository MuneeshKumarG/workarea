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
        /// 
        /// </summary>
        public float X { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public float Y { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        public readonly ChartSeries Series;

        /// <summary>
        /// 
        /// </summary>
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
        /// 
        /// </summary>
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
        /// 
        /// </summary>
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
        /// 
        /// </summary>
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
        /// 
        /// </summary>
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
        /// 
        /// </summary>
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
        /// 
        /// </summary>
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
        /// 
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
        /// 
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
        /// 
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public TooltipInfo(ChartSeries chartSeries)
        {
            Series = chartSeries;   
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
