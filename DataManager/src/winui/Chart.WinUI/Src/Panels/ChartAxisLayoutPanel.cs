// <copyright file="ChartAxisLayoutPanel.cs" company="Syncfusion. Inc">
// Copyright Syncfusion Inc. 2001 - 2017. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
// </copyright>
namespace Syncfusion.UI.Xaml.Charts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.Foundation;
#if WinUI_Desktop
    using Microsoft.UI.Xaml.Controls;
#else
    using Windows.UI.Xaml.Controls;
#endif

    /// <summary>
    /// Represents <see cref="ChartAxisLayoutPanel"/> class. 
    /// </summary>
    internal class ChartAxisLayoutPanel : Panel
    {
        #region Properties

        /// <summary>
        /// Gets or sets AxisLayout property
        /// </summary>

        public ILayoutCalculator AxisLayout
        {
            get;
            set;
        }

        #endregion

        #region Methods

        #region Protected Methods

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            availableSize = ChartLayoutUtils.CheckSize(availableSize);
            if (AxisLayout != null)
            {
                AxisLayout.Measure(availableSize);
            }
            return availableSize;
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (AxisLayout != null)
            {
                AxisLayout.Arrange(finalSize);
            }
            return finalSize;
        }

        #endregion

        #endregion
    }
}
