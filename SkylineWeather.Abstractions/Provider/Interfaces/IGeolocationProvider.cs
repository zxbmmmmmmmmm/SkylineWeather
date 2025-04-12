using LanguageExt.Common;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Models;

namespace SkylineWeather.Abstractions.Provider.Interfaces;

public interface IGeolocationProvider
{
    public Task<Result<IReadOnlyList<Geolocation>>> GetGeolocationsAsync(Location location);
    public Task<Result<IReadOnlyList<Geolocation>>> GetGeolocationsAsync(string name);
}