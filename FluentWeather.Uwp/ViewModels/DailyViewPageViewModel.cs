using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.Uwp.ViewModels;

public sealed partial class DailyViewViewModel : ObservableObject
{
    [ObservableProperty]
    private partial List<WeatherBase> DailyForecasts { get; set; }

    [ObservableProperty]
    private partial List<WeatherBase> HourlyForecasts { get; set; }

    [ObservableProperty]
    private partial WeatherBase Selected { get; set; }
    public DailyViewViewModel(List<WeatherBase> daily,List<WeatherBase> hourly)
    {
        DailyForecasts = daily;
        HourlyForecasts = hourly;
    }
}