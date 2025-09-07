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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SkylineWeather.WinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
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
                HighestTemperatureDate = DateOnly.FromDateTime(DateTime.Today),
                LowestTemperatureDate = DateOnly.FromDateTime(DateTime.Today),
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
