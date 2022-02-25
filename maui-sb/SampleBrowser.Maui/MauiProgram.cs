﻿using Microsoft.Maui;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Syncfusion.Maui.Core.Hosting;
using Syncfusion.Maui.ListView.Hosting;

namespace SampleBrowser.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("Font Gauge Icons.ttf", "FontGaugeIcons");
                    fonts.AddFont("Scheduler Icons.ttf", "SchedulerIcons");
                });

            builder.ConfigureSyncfusionCore();
            builder.ConfigureSyncfusionListView();

            return builder.Build();
        }
    }
}