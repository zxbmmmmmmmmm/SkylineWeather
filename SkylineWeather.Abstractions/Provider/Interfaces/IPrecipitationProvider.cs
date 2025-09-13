using LanguageExt.Common;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;

namespace SkylineWeather.Abstractions.Provider.Interfaces;

public interface IPrecipitationProvider : IProvider
{
    Task<Result<IReadOnlyList<Precipitation>>> GetPrecipitationAsync(Location location, CancellationToken cancellationToken = default);
}