using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rectangle = Microsoft.Maui.Graphics.Rectangle;

namespace Syncfusion.Maui.Core
{
    internal partial class SfTooltip : ContentView
    {
        private Microsoft.Maui.Controls.Shapes.Geometry GetClipPathGeometry(PointCollection points, Rectangle tooltipRect)
        {
            return new PathGeometry();
        }
    }
}
