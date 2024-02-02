using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Abstraction.Models;
using Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarSymbols;

namespace FluentWeather.Uwp.ViewModels;

public sealed partial class DailyViewViewModel : ObservableObject
{
    [ObservableProperty]
    private List<WeatherBase> _dailyForecasts;

    [ObservableProperty]
    private List<WeatherBase> _hourlyForecasts;

    [ObservableProperty]
    private WeatherBase _selected;
    public DailyViewViewModel(List<WeatherBase> daily,List<WeatherBase> hourly)
    {
        DailyForecasts = daily;
        HourlyForecasts = hourly;
    }
}