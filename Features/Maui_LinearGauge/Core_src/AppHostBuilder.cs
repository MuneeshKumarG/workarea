using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Syncfusion.Maui.Core;
using Syncfusion.Maui.Graphics.Internals;

namespace Syncfusion.Maui.Core.Hosting
{
    /// <summary>
    /// Represents application host extension, that used to configure handlers defined in Syncfusion maui core.
    /// </summary>
    public static class AppHostBuilderExtensions
    {
        /// <summary>
        /// Configures the implemented handlers in Syncfusion.Maui.Core.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static MauiAppBuilder ConfigureSyncfusionCore(this MauiAppBuilder builder)
        {
            builder.ConfigureMauiHandlers(handlers =>
            {
                handlers.AddHandler(typeof(IDrawableView), typeof(DrawableViewHandler));
            });

            return builder;
        }
    }
}
