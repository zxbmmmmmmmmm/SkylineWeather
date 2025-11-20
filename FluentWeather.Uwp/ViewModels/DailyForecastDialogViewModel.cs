using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.Uwp.ViewModels;

public sealed partial class DailyForecastDialogViewModel : ObservableObject
{
    [ObservableProperty]
    public partial List<WeatherDailyBase> DailyForecasts { get; set; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SunRise))]
    [NotifyPropertyChangedFor(nameof(SunSet))]
    public partial WeatherDailyBase Selected { get; set; }

    public DateTime? SunRise => ((IAstronomic)Selected).SunRise;

    public DateTime? SunSet => ((IAstronomic)Selected).SunSet;

    [ObservableProperty]
    public partial List<WeatherHourlyBase> HourlyForecasts { get; set; } = new();

}