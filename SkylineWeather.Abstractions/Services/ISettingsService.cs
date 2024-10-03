using SkylineWeather.Abstractions.Models;

namespace SkylineWeather.Abstractions.Services;

public interface ISettingsService
{
    public Geolocation DefaultGeolocation { get; set; }
}