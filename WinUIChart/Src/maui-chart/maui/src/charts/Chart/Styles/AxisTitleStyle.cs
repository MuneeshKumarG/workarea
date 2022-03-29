using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using Rect = Microsoft.Maui.Graphics.RectF;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// 
    /// </summary>
    public class ChartAxisTitle: ChartLabelStyle
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>        
        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text),
            typeof(string),
            typeof(ChartAxisTitle),
            string.Empty,
            BindingMode.Default,
            null,
            OnTextPropertyChanged);

        /// <summary>
		/// 
        /// </summary>
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private SizeF textSize;

        private Rect measuringRect;

        internal float Left { get; set; }

        internal float Top { get; set; }

        internal ChartAxis? Axis { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public ChartAxisTitle() : base()
        {
            TextColor = Color.FromArgb("#99000000");
            FontSize = 14;
            Margin = new Microsoft.Maui.Thickness(5, 12, 5, 2);
        }

        #endregion

        #region Methods

        internal void Measure()
        {
            if (Axis == null)
            {
                return;
            }

            SizeF measuredSize = Text.Measure(this);
            measuringRect = new Rect(new PointF(0, 0), measuredSize);

            if (Axis.IsVertical)
            {
                textSize = new SizeF(measuredSize.Height, measuredSize.Width);
            }
            else
            {
                textSize = new SizeF(measuredSize.Width, measuredSize.Height);
            }
        }

        internal Size GetDesiredSize()
        {
            if (textSize.Width == 0 && string.IsNullOrEmpty(Text))
            {
                return new SizeF(textSize.Width, textSize.Height);
            }

            if (Axis != null && Axis.IsVertical)
            {
                return new Size(textSize.Width + Margin.Top + Margin.Bottom, textSize.Height + Margin.Left + Margin.Right);
            }

            return new Size(textSize.Width + Margin.Left + Margin.Right, textSize.Height + Margin.Top + Margin.Bottom);
        }

        internal void Draw(ICanvas canvas)
        {
            if (measuringRect == RectF.Zero)
            {
                measuringRect = new Rect();
            }

            canvas.SaveState();

            if (Axis != null && Axis.IsVertical)
            {
                float x, y;
                bool opposedPosition = Axis.IsOpposed();

                x = Left + (float)Margin.Bottom -
                           (opposedPosition ? measuringRect.Bottom : -measuringRect.Bottom) - (textSize.Height / 2);
                //Addded platform specific code for position the label.
#if ANDROID
                x += textSize.Width / 2;
#endif
                y = Top + (textSize.Width / 2) - measuringRect.Bottom;

                canvas.Rotate(opposedPosition ? 90 : -90, x + measuringRect.Center.X, y + measuringRect.Center.Y);

                if (CanDraw())
                {
                    DrawTitleBackground(canvas, measuringRect, x, y);
                }

                canvas.DrawText(Text, x, y, this);
            }
            else
            {
                float x = Left - measuringRect.Width / 2;
                float y = Top + (float)Margin.Top;
#if ANDROID
                //Addded platform specific code for position the label.
                y += measuringRect.Height / 2;
#endif
                if (CanDraw())
                {
                    DrawTitleBackground(canvas, measuringRect, x, y);
                }

                canvas.DrawText(Text, x, y, this);
                
            }

            canvas.RestoreState();
        }

        internal void DrawTitleBackground(ICanvas canvas, Rect bounds, float x, float y)
        {
            canvas.StrokeSize = (float)StrokeWidth;
            canvas.StrokeColor = Stroke.ToColor();
            canvas.FillColor = (Background as SolidColorBrush)?.Color;

            if (Axis != null && Axis.IsVertical)
            {
                bool isOpposed = Axis.IsOpposed();

                var top = isOpposed ? Margin.Bottom : Margin.Top;
                var bottom = isOpposed ? Margin.Top : Margin.Bottom;
                var left = isOpposed ? Margin.Right : Margin.Left;
                var right = isOpposed ? Margin.Left : Margin.Right; ;
                canvas.DrawRectangle(
                    x - (float)left,
                    y - bounds.Height - (float)bottom,
                    bounds.Width + (float)left + (float)right, 
                    bounds.Height + (float)bottom + (float)top
                    );
            }
            else
            {
                canvas.DrawRectangle(
                    x - (float)Margin.Left,
                    y - (float)Margin.Top - (float)Margin.Bottom - bounds.Height,
                    bounds.Width + (float)Margin.Right + (float)Margin.Left,
                    (float)Margin.Bottom + (float)Margin.Top + bounds.Height);
            }
        }

        private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        #endregion
    }
}
