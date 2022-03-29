using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Syncfusion.Maui.Charts
{ 
    internal class ChartTitleView : Frame
    {
        internal ChartTitleView()
        {
            HasShadow = false;
            Padding = 0; //For default size to be empty
            BackgroundColor = Colors.Transparent;
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Center;
        }

        internal void InitTitle(string? content)
        {
            var label = new Label()
            {
                Text = content,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                TextColor = Color.FromArgb("CC000000"),
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center,
            };

            this.Content = label;
        }
    }
}
