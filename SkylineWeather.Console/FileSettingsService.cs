using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Services;

namespace SkylineWeather.Console;

public class FileSettingsService : ISettingsService
{
    public Geolocation DefaultGeolocation { get; set; } = null!;
}