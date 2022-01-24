// <copyright file="Utils.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Syncfusion.Maui.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Flagged Enum extension.
    /// </summary>
    internal static partial class FlaggedEnumExt
    {
        /// <summary>
        /// Gets the all available items in given enum.
        /// </summary>
        /// <param name="targetEnum">Target enum.</param>
        /// <returns>Available enum values.</returns>
        internal static IEnumerable<Enum> GetAllItems(this Enum targetEnum)
        {
            foreach (Enum value in Enum.GetValues(targetEnum.GetType()))
            {
                if (targetEnum.HasFlag(value))
                {
                    yield return value;
                }
            }
        }

        /// <summary>
        /// Gets whether the given items contains none or selection enum type or not.
        /// </summary>
        /// <param name="source">Source enum.</param>
        /// <returns>Returns whether selection or none is given or not.</returns>
        internal static bool IsEmpty(this SfEffects source)
        {
            foreach (SfEffects value in source.GetAllItems())
            {
                if (value != SfEffects.None && value != SfEffects.Selection)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Adding the flagged enum.
        /// </summary>
        /// <param name="target">Target.</param>
        /// <param name="newItem">Adding item.</param>
        /// <returns>Returns merged value.</returns>
        internal static SfEffects Add(this SfEffects target, SfEffects newItem)
        {
            return target |= newItem;
        }

        /// <summary>
        /// Getting the complement of target from source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        /// <returns>The complement value.</returns>
        internal static SfEffects ComplementsOf(this SfEffects target, SfEffects source)
        {
            return target &= ~source;
        }

        /// <summary>
        /// Getting the complement of target from source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source1">The source 1.</param>
        /// <param name="source2">The source 2.</param>
        /// <returns>The complement value.</returns>
        internal static SfEffects ComplementsOf(this SfEffects target, SfEffects source1, SfEffects source2)
        {
            return target &= ~(source1 | source2);
        }
    }
}
