using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using Syncfusion.Maui.Graphics.Internals;
using Rect = Microsoft.Maui.Graphics.RectF;

namespace Syncfusion.Maui.Charts
{
    /// <summary>
    /// Represents the chart axis's title class.
    /// </summary>
    /// <remarks>
    /// <para>To customize the chart axis's title, add the <see cref="ChartAxisTitle"/> instance to the <see cref="ChartAxis.Title"/> property as shown in the following code sample.</para>
    /// # [MainPage.xaml](#tab/tabid-1)
    /// <code><![CDATA[
    /// <chart:SfCartesianChart>
    /// 
    ///     <chart:SfCartesianChart.XAxes>
    ///         <chart:CategoryAxis>
    ///            <chart:CategoryAxis.Title>
    ///                <chart:ChartAxisTitle Text="AxisTitle" 
    ///                                      TextColor="Red"
    ///                                      Background="Yellow/>
    ///            </chart:CategoryAxis.Title>
    ///        </chart:CategoryAxis>
    ///     </chart:SfCartesianChart.XAxes>
    /// 
    /// </chart:SfCartesianChart>
    /// ]]>
    /// </code>
    /// # [MainPage.xaml.cs](#tab/tabid-2)
    /// <code><![CDATA[
    /// SfCartesianChart chart = new SfCartesianChart();
    /// 
    /// CategoryAxis xaxis = new CategoryAxis();
    /// xaxis.Title = new ChartAxisTitle()
    /// {
    ///     Text = "AxisTitle",
    ///     TextColor = Colors.Red,
    ///     Background = new SolidColorBrush(Colors.Yellow)
    /// };
    /// chart.XAxes.Add(xaxis);
    ///
    /// ]]>
    /// </code>
    /// *** 
    /// 
    /// <para>It provides more options to customize the chart axis title.</para>
    /// 
    /// <para> <b>Text - </b> To sets the title for axis, refer to this <see cref="Text"/> property.</para>
    /// <para> <b>TextColor - </b> To customize the text color, refer to this <see cref="ChartLabelStyle.TextColor"/> property. </para>
    /// <para> <b>Background - </b> To customize the background color, refer to this <see cref="ChartLabelStyle.Background"/> property. </para>
    /// <para> <b>Stroke - </b> To customize the stroke color, refer to this <see cref="ChartLabelStyle.Stroke"/> property. </para>
    /// <para> <b>StrokeWidth - </b> To modify the stroke width, refer to this <see cref="ChartLabelStyle.StrokeWidth"/> property. </para>
    /// 
    /// </remarks>
    public class ChartAxisTitle: ChartLabelStyle
    {
        #region Properties

        /// <summary>
        /// Identifies the <see cref="Text"/> bindable property.
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
        /// Gets or sets a value that displays the content for the axis title.
        /// </summary>
        /// <value>It accepts string values.</value>
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
        /// Initializes a new instance of the <see cref="ChartAxisTitle"/>.
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

                if(opposedPosition)
                {
                   y -= (float)Margin.Left;
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

#if ANDROID
                canvas.DrawText(Text, x, y, this);
#else
                canvas.DrawText(Text, x, y - (float)Margin.Left, this);
#endif
            }

            canvas.RestoreState();
        }

        internal void DrawTitleBackground(ICanvas canvas, Rect bounds, float x, float y)
        {
            canvas.StrokeSize = (float)StrokeWidth;
            canvas.StrokeColor = Stroke.ToColor();
            canvas.FillColor = (Background as SolidColorBrush)?.Color;
            CornerRadius cornerRadius = CornerRadius;
            Rect rect;

            if (Axis != null && Axis.IsVertical)
            {
                bool isOpposed = Axis.IsOpposed();

                var top = isOpposed ? Margin.Bottom : Margin.Top;
                var bottom = isOpposed ? Margin.Top : Margin.Bottom;
                var left = isOpposed ? Margin.Right : Margin.Left;
                var right = isOpposed ? Margin.Left : Margin.Right; ;
#if ANDROID
                    rect = new Rect( x - (float)left,
                    y - bounds.Height - (float)bottom,
                    bounds.Width + (float)left + (float)right,
                    bounds.Height + (float)bottom + (float)top);
#else
                    rect = new Rect(x - (float)left,
                    y - (float)right,
                    bounds.Width + (float)left + (float)right,
                    bounds.Height + (float)bottom / 2 + (float)top / 2);
#endif
            }
            else
            {
#if ANDROID
                    rect = new Rect(x - (float)Margin.Left,
                    y - (float)Margin.Top + (float)Margin.Left - bounds.Height,
                    bounds.Width + (float)Margin.Right + (float)Margin.Left,
                    (float)Margin.Bottom  + (float)Margin.Top  + bounds.Height);
#else
                    rect = new Rect(x - (float)Margin.Left,
                    y - (float)Margin.Top,
                    bounds.Width + (float)Margin.Right + (float)Margin.Left,
                    (float)Margin.Bottom / 2 + (float)Margin.Top / 2 + bounds.Height);
#endif
            }

            if (cornerRadius.TopLeft > 0)
            {
                canvas.FillRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
            }
            else
            {
                canvas.FillRectangle(rect);
            }

            if (StrokeWidth > 0)
            {
                if (cornerRadius.TopLeft > 0)
                {
                    canvas.DrawRoundedRectangle(rect, cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomLeft, cornerRadius.BottomRight);
                }
                else
                {
                    canvas.DrawRectangle(rect);
                }
            }
        }

        private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
        }

        #endregion
    }
}
