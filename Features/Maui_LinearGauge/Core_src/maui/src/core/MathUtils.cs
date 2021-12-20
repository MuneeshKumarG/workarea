using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syncfusion.Maui.Core
{
    /// <summary>
    /// 
    /// </summary>
    internal class MathUtils
    {
        internal static double GetAngle(double x1, double x2, double y1, double y2)
        {
            double radians = Math.Atan2(-(y2 - y1), x2 - x1);
            radians = radians < 0 ? Math.Abs(radians) : (2 * Math.PI) - radians;
            return radians * (180 / Math.PI);
        }
    }
}
