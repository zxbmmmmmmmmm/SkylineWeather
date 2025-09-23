using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SkylineWeather.Abstractions.Services;
using SkylineWeather.DataAnalyzer.Analyzers;
using SkylineWeather.DataAnalyzer.Models;
using SkylineWeather.SDK;
using SkylineWeather.SDK.Services;
using SkylineWeather.SDK.Utilities;
using SkylineWeather.ViewModels;
using UnitsNet;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SkylineWeather.WinUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;

        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .UseContentRoot(ApplicationData.Current.LocalFolder.Path)
            .ConfigureAppConfiguration(config =>
            {
                config.AddJsonFile("appsettings.json", false, true);
            })
            .ConfigureServices((context, services) => {
                services.AddSingleton<ISettingsService, ConfigurationSettingsService>();
                services.AddOptions<CommonSettings>()
                    .Bind(context.Configuration);
                services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<CommonSettings>>().Value);
                var settingsForConfiguration = new CommonSettings();
                context.Configuration.Bind(settingsForConfiguration);
                services.AddProviders(settingsForConfiguration, typeof(QWeatherProvider.QWeatherProvider), typeof(OpenMeteoProvider.OpenMeteoProvider));
                services.AddSingleton<ITrendAnalyzer<Temperature, TemperatureTrend>, SingleTemperatureTrendAnalyzer>()
                    .AddSingleton<ITrendAnalyzer<(Temperature, Temperature), TemperatureTrend>, CompositeTemperatureTrendAnalyzer>()
                    .AddSingleton<IAqiAnalyzer, ChinaAqiAnalyzer>()
                    .AddSingleton<IAqiAnalyzer, UsaAqiAnalyzer>()
                    .AddSingleton<IAqiAnalyzer, EuropeAqiAnalyzer>();
                services.AddSingleton<RootViewModel>()
                    .AddSingleton<WeatherViewModelFactory>();
            })
            .Build();

        public static T GetService<T>()
            where T : class
        {
            return (_host.Services.GetService(typeof(T)) as T) ?? throw new Exception("Cannot find service of specified type");
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
            this.UnhandledException +=(sender, e) =>
            {
                // Handle unhandled exceptions here
                // For example, log the exception or show a message to the user
                System.Diagnostics.Debug.WriteLine($"Unhandled exception: {e.Exception.Message}");
                e.Handled = true; // Prevents the app from crashing
            };
        }
    }
}
