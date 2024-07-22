using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.Uwp.ViewModels;

public sealed partial class DailyForecastDialogViewModel:ObservableObject
{
    [ObservableProperty]
    List<WeatherDailyBase> _dailyForecasts = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SunRise))]
    [NotifyPropertyChangedFor(nameof(SunSet))]
    WeatherDailyBase _selected;

    public DateTime? SunRise => ((IAstronomic)Selected).SunRise;

    public DateTime? SunSet => ((IAstronomic)Selected).SunSet;

    [ObservableProperty]
    List<WeatherHourlyBase> _hourlyForecasts = new();

}