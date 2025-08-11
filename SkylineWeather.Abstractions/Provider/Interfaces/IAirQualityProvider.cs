using LanguageExt.Common;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.AirQuality;

namespace SkylineWeather.Abstractions.Provider.Interfaces;

public interface IAirQualityProvider : IProvider
{
    public Task<Result<AirQuality>> GetCurrentAirQualityAsync(Location location, CancellationToken cancellationToken = default);
}