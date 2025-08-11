using LanguageExt.Common;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Models;

namespace SkylineWeather.Abstractions.Provider.Interfaces;

public interface IHourlyWeatherProvider : IProvider
{
    public Task<Result<IReadOnlyList<HourlyWeather>>> GetHourlyWeatherAsync(Location location, CancellationToken cancellationToken = default);
}