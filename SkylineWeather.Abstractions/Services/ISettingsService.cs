using SkylineWeather.Abstractions.Models;

namespace SkylineWeather.Abstractions.Services;

public interface ISettingsService2
{
    Geolocation DefaultGeolocation { get; set; }
}
public interface ISettingsService
{
    T GetOrCreateValue<T>(string key, T? defaultValue = default);

    void SetValue<T>(string key, T value);
}