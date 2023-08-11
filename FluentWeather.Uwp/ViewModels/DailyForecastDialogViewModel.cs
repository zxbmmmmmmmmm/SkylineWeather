using CommunityToolkit.Mvvm.ComponentModel;
using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using System;
using System.Collections.ObjectModel;

namespace FluentWeather.Uwp.ViewModels;

public partial class DailyForecastDialogViewModel:ObservableObject
{
    [ObservableProperty]
    ObservableCollection<WeatherBase> _dailyForecasts = new();
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SunRise))]
    [NotifyPropertyChangedFor(nameof(SunSet))]
    WeatherBase _selected;

    public DateTime SunRise => ((IAstronomic)Selected).SunRise;

    public DateTime SunSet => ((IAstronomic)Selected).SunSet;

}