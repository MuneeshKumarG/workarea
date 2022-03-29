using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using Application = Microsoft.Maui.Controls.Application;

namespace SampleBrowser.Maui.Chart
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }
    }
}
