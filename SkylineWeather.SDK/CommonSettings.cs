using CommunityToolkit.Mvvm.ComponentModel;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace SkylineWeather.SDK;

public class CommonSettings(ISettingsService settingsService) : ObservableObject
{
    public Geolocation? DefaultGeolocation { get; set => SetAndSave(ref field, value); }
        = settingsService.GetOrCreateValue<Geolocation>(nameof(DefaultGeolocation));

    public ObservableCollection<Geolocation> SavedCities { get; set => SetAndSave(ref field, value); } 
        = settingsService.GetOrCreateValue<ObservableCollection<Geolocation>>(nameof(SavedCities), [])!;

    public Dictionary<string, JsonElement> ProviderConfigurations { get; set => SetAndSave(ref field, value); }
        = settingsService.GetOrCreateValue<Dictionary<string, JsonElement>>(nameof(ProviderConfigurations), [])!;

    public Dictionary<string, string> ProviderMappings { get; set => SetAndSave(ref field, value); }
        = settingsService.GetOrCreateValue<Dictionary<string, string>>(nameof(ProviderMappings), [])!;

    private void SetAndSave<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (SetProperty(ref field, value, propertyName))
        {
            settingsService.SetValue(propertyName!, value);
        }
    }
}
