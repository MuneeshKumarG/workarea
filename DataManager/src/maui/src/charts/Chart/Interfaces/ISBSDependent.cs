using Microsoft.Maui.Controls;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Charts
{
    internal interface ISBSDependent
    {
        public double Spacing { get; set; }

        public double Width { get; set; }

        public CornerRadius CornerRadius { get; set; }

    }
}
