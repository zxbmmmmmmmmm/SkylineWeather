using Microsoft.Extensions.Configuration;
using SkylineWeather.Abstractions.Services;
using System.Text.Json;

namespace SkylineWeather.Console;

internal class ConfigurationSettingsService(IConfiguration configuration) : ISettingsService
{
    public T? GetOrCreateValue<T>(string key, T? defaultValue = default)
    {
        return configuration.GetSection(key).Get<T>() ?? defaultValue;
    }

    public void SetValue<T>(string key, T value)
    {
        configuration[key] = JsonSerializer.Serialize(value);
    }
}