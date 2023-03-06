using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Syncfusion.Maui.Core.Internals;
using Microsoft.Maui.Graphics;
using System.Collections.Specialized;
using Syncfusion.Maui.Core;
using Microsoft.Maui;

namespace Syncfusion.Maui.Charts
{
    internal interface ITooltipDependent
    {
        #region public Properties

        DataTemplate TooltipTemplate { get; set; }

        bool EnableTooltip { get; set; }

        #endregion

        #region Methods

        void SetTooltipTargetRect(TooltipInfo tooltipInfo, Rect chartBounds);

        DataTemplate? GetDefaultTooltipTemplate(TooltipInfo info)
        {
            return ChartUtils.GetDefaultTooltipTemplate(info);
        }

        #endregion
    }
}
