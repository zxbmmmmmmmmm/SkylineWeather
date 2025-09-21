using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnitsNet;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SkylineWeather.Abstractions.Services;
using SkylineWeather.DataAnalyzer.Analyzers;
using SkylineWeather.DataAnalyzer.Models;
using SkylineWeather.SDK;
using SkylineWeather.SDK.Services;
using SkylineWeather.SDK.Utilities;
using Windows.ApplicationModel;
using Windows.Storage;
using SkylineWeather.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SkylineWeather.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
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


        public MainWindow()
        {
            InitializeComponent();
            _ = GetSampleData();
        }

        private async Task GetSampleData()
        {
            HistoricalChart.Historical = new HistoricalWeather()
            {
                AverageHighTemperature = Temperature.FromDegreesCelsius(30),
                AverageLowTemperature = Temperature.FromDegreesCelsius(20),
                HighestTemperature = Temperature.FromDegreesCelsius(35),
                LowestTemperature = Temperature.FromDegreesCelsius(15),
                HighestTemperatureDate = DateOnly.FromDateTime(DateTime.MinValue),
                LowestTemperatureDate = DateOnly.FromDateTime(DateTime.MaxValue),
            };
            HistoricalChart.Current = new CurrentWeather()
            {
                Temperature = Temperature.FromDegreesCelsius(25),
                WeatherCode = WeatherCode.Clear,
            };
            HistoricalChart.Today = new DailyWeather()
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                HighTemperature = Temperature.FromDegreesCelsius(32),
                LowTemperature = Temperature.FromDegreesCelsius(16)
            };
            var provider = new OpenMeteoProvider.OpenMeteoProvider();
            var hourly = await provider.GetHourlyWeatherAsync(new Abstractions.Models.Location(28, 119));
            hourly.IfSucc(result =>
            {
                HourlyChart.Data = result.Take(24).ToList();
            });

            var daily = await provider.GetDailyWeatherAsync(new Abstractions.Models.Location(28, 119));
            daily.IfSucc(result =>
            {
                DailyChart.Data = result;
            });
        }

    }
}
