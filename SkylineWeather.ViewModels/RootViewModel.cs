using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Services;
using System.Collections.ObjectModel;
using System.Security.Cryptography.X509Certificates;

namespace SkylineWeather.ViewModels;

public partial class RootViewModel(
    Geolocation defaultGeolocation,
    ICacheService cacheService,
    WeatherViewModelFactory weatherViewModelFactory) : ObservableObject
{
    [ObservableProperty]
    public partial ObservableCollection<WeatherViewModel>? WeatherViewModels { get; private set; }

    [ObservableProperty]
    public partial WeatherViewModel? Selected { get; private set; }

    private async Task LoadAsync()
    {
        WeatherViewModels = await cacheService.GetCacheAsync<ObservableCollection<WeatherViewModel>>("WeatherViewModels");
        WeatherViewModels ??= [weatherViewModelFactory.Create(defaultGeolocation)];
    }

    [RelayCommand]
    public void AddCity(Geolocation geolocation)
    {
        WeatherViewModels?.Add(weatherViewModelFactory.Create(geolocation));
    }

    [RelayCommand]
    public void RemoveCity(WeatherViewModel viewModel)
    {
        WeatherViewModels?.Remove(viewModel);
    }
}