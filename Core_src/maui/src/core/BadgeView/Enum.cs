namespace Syncfusion.Maui.Core
{
    #region enum

    /// <summary>
    /// Defines the badge type for the badge control for predefined style.
    /// </summary>
    public enum BadgeType
    {
        /// <summary>
        /// Defines the badge type as dark for the badge control.
        /// </summary>
        Dark,

        /// <summary>
        /// Defines the badge type as danger for the badge control.
        /// </summary>
        Error,

        /// <summary>
        /// Defines the badge type as info for the badge control.
        /// </summary>
        Info,

        /// <summary>
        /// Defines the badge type as light for the badge control.
        /// </summary>
        Light,

        /// <summary>
        ///  Defines the badge type as none for the badge control.
        /// </summary>
        None,

        /// <summary>
        /// Defines the badge type as primary for the badge control.
        /// </summary>
        Primary,

        /// <summary>
        /// Defines the badge type as secondary for the badge control.
        /// </summary>
        Secondary,

        /// <summary>
        /// Defines the badge type as success for the badge control.
        /// </summary>
        Success,

        /// <summary>
        /// Defines the badge type as warning for the badge control.
        /// </summary>
        Warning,
    }

    /// <summary>
    /// Defines the position of the badge over the badge view control.
    /// </summary>
    public enum BadgePosition
    {
        /// <summary>
        /// To set the badge control at the bottom left corner to the badge content.
        /// </summary>
        BottomLeft,

        /// <summary>
        /// To set the badge control at the bottom right corner to the badge content.
        /// </summary>
        BottomRight,

        /// <summary>
        /// To set the badge control at the top left corner to the content.
        /// </summary>
        TopLeft,

        /// <summary>
        /// To set the badge control at the top right corner to the badge content.
        /// </summary>
        TopRight,
    }

    /// <summary>
    /// Defines the animation for the badge control.
    /// </summary>
    public enum BadgeAnimation
    {
        /// <summary>
        /// To define the badge animation as none that no animation will be happened.
        /// </summary>
        None,

        /// <summary>
        /// To define the badge animation as scale.
        /// </summary>
        Scale,
    }

    /// <summary>
    /// Defines the alignment for the badge.
    /// </summary>
    public enum BadgeAlignment
    {
        /// <summary>
        /// To define the badge alignment as start
        /// </summary>
        Start,

        /// <summary>
        /// To define the badge alignment as center
        /// </summary>
        Center,

        /// <summary>
        /// To define the badge alignment as end
        /// </summary>
        End,
    }

    /// <summary>
    /// Defines the predefined icons for badge.
    /// </summary>
    //TODO: Need to fix the font icon issue in library or need to draw the icons
    internal enum BadgeIcon
    {
        /// <summary>
        /// To define the badge icon as dot.
        /// </summary>
        Dot,

        /// <summary>
        /// To define the badge icon as None.
        /// </summary>
        None,
    }

    #endregion
}
