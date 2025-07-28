using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Services;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace SkylineWeather.ViewModels;

public partial class RootViewModel : ObservableObject
{
    [ObservableProperty]
    public partial ObservableCollection<WeatherViewModel>? WeatherViewModels { get; private set; }

    [ObservableProperty]
    public partial WeatherViewModel? Selected { get; private set; }

    private readonly WeatherViewModelFactory _weatherViewModelFactory;

    public RootViewModel(IReadOnlyCollection<Geolocation> geolocations, WeatherViewModelFactory weatherViewModelFactory)
    {
        _weatherViewModelFactory = weatherViewModelFactory;
        WeatherViewModels = new ObservableCollection<WeatherViewModel>(geolocations.Select(p => _weatherViewModelFactory.Create(p)));
    }

    /// <summary>
    /// 获取所有城市当前的天气数据
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    public async Task UpdateAllCurrentAsync()
    {
        var tasks = WeatherViewModels?.Select(p => p.GetCurrentAsync());
        if (tasks is null)
            return;
        await Task.WhenAll(tasks);
    }

    [RelayCommand]
    public void AddCity(Geolocation geolocation)
    {
        WeatherViewModels?.Add(_weatherViewModelFactory.Create(geolocation));
    }

    [RelayCommand]
    public void RemoveCity(WeatherViewModel viewModel)
    {
        WeatherViewModels?.Remove(viewModel);
    }
}