using CommunityToolkit.Mvvm.ComponentModel;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SkylineWeather.SDK;

public class CommonSettings(ISettingsService settingsService) : ObservableObject
{
    public Geolocation? DefaultGeolocation { get; set => SetAndSave(ref field, value); }
        = settingsService.GetOrCreateValue<Geolocation>(nameof(DefaultGeolocation));

    public ObservableCollection<Geolocation> SavedCities { get; set => SetAndSave(ref field, value); } 
        = settingsService.GetOrCreateValue<ObservableCollection<Geolocation>>(nameof(SavedCities), [])!;

    private void SetAndSave<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (SetProperty(ref field, value, propertyName))
        {
            settingsService.SetValue(propertyName!, value);
        }
    }
}
