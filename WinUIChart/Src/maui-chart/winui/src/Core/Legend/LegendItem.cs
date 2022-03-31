namespace Syncfusion.UI.Xaml.Charts
{
    using Microsoft.UI;
    using Microsoft.UI.Text;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Media;
    using System.ComponentModel;
    using Windows.UI;
    using Windows.UI.Text;

    public class LegendItem : INotifyPropertyChanged, ILegendItem
    {
        #region Fields

        private int index;
        private object item = null;
        private string text = string.Empty;
        private string fontFamily = string.Empty;
        private FontWeight fontAttributes = FontWeights.Black;
        private Brush iconBrush = new SolidColorBrush(Colors.Transparent);
        private Color textColor = Colors.Black;
        private ShapeType iconType = ShapeType.Rectangle;
        private double iconHeight = 12;
        private double iconWidth = 12;
        private float fontSize = 12;
        private Thickness textMargin = new Thickness(0);
        private bool isToggled = false;
        private bool isIconVisible = true;
        private Brush disableBrush = new SolidColorBrush(Colors.Gray);

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the corresponding label for legend item.
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
        /// Gets or sets the font family name for the legend item label.
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
        /// Gets or sets the font family name for the legend item label.
        /// </summary>
        public FontWeight FontWeight
        {
            get { return fontAttributes; }
            set
            {
                if (fontAttributes == value)
                {
                    return;
                }

                fontAttributes = value;
                OnPropertyChanged(nameof(FontWeight));
            }
        }

        /// <summary>
        /// Gets or sets the corresponding icon color for legend item.
        /// </summary>
        public Brush IconBrush
        {
            get { return iconBrush; }

            set
            {
                if (iconBrush == value)
                {
                    return;
                }

                iconBrush = value;
                OnPropertyChanged(nameof(IconBrush));
            }
        }

        /// <summary>
        /// Gets or sets the corresponding text color for legend item.
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
        /// Gets or sets the width of the legend icon.
        /// </summary>
        /// <value>This property takes double value.</value>
        public double IconWidth
        {
            get { return iconWidth; }
            set
            {
                if (iconWidth == value)
                {
                    return;
                }

                iconWidth = value;
                OnPropertyChanged(nameof(IconWidth));
            }
        }

        /// <summary>
        /// Gets or sets the height of the legend icon.
        /// </summary>
        /// <value>This property takes double value.</value>
        public double IconHeight
        {
            get { return iconHeight; }
            set
            {
                if (iconHeight == value)
                {
                    return;
                }

                iconHeight = value;
                OnPropertyChanged(nameof(IconHeight));
            }
        }

        /// <summary>
        /// Gets or sets the font size for the legend label text. 
        /// </summary>
        /// <value>This property takes float value.</value>
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
        /// Gets or sets the margin of the legend text.
        /// </summary>
        public Thickness TextMargin
        {
            get { return textMargin; }
            set
            {
                if (textMargin == value)
                {
                    return;
                }

                textMargin = value;
                OnPropertyChanged(nameof(TextMargin));
            }
        }

        /// <summary>
        /// Gets or sets the icon type in legend.
        /// </summary>
        public ShapeType IconType
        {
            get { return iconType; }
            set
            {
                if (iconType == value)
                {
                    return;
                }

                iconType = value;
                OnPropertyChanged(nameof(IconType));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the legend item is toggled or not.
        /// </summary>
        public bool IsToggled
        {
            get { return isToggled; }

            set
            {
                if (isToggled == value)
                {
                    return;
                }

                isToggled = value;
                OnPropertyChanged(nameof(IsToggled));
            }
        }

        /// <summary>
        /// Gets or sets the legend icon and text disable color when toggled.
        /// </summary>
        public Brush DisableBrush
        {
            get { return disableBrush; }

            set
            {
                if (disableBrush == value)
                {
                    return;
                }

                disableBrush = value;
                OnPropertyChanged(nameof(DisableBrush));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display the legend icon.
        /// </summary>
        public bool IsIconVisible
        {
            get { return isIconVisible; }

            set
            {
                if (isIconVisible == value)
                {
                    return;
                }

                isIconVisible = value;
                OnPropertyChanged(nameof(IsIconVisible));
            }
        }

        /// <summary>
        /// Gets the corresponding index for legend item.
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
        /// Gets the corresponding data point for series.
        /// </summary>
        public object Item
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

        Brush ILegendItem.IconBrush { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        Color ILegendItem.TextColor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        Brush ILegendItem.DisableBrush { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="LegendItem"/> class.
        /// </summary>
        public LegendItem()
        {
        }

        #endregion

        #region event

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
