﻿using System;

namespace Syncfusion.Maui.Graphics.Internals
{
    /// <summary>
    /// Represents a class that can be used to calculate the text size.
    /// </summary>
    internal partial class TextMeasurer
    {
        [ThreadStatic]
        private static ITextMeasurer? instance;

        internal static partial ITextMeasurer CreateTextMeasurer();

        /// <summary>
        /// Static instance for TextMeasurer. 
        /// </summary>
        public static ITextMeasurer Instance
        {
            get
            {
                if (instance == null)
                    instance = CreateTextMeasurer();
                return instance;
            }
        }
    }
}
