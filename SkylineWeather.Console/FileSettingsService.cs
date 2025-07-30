using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Services;

namespace SkylineWeather.Console;

public class FileSettingsService : ISettingsService2
{
    public Geolocation DefaultGeolocation { get; set; } = null!;
}