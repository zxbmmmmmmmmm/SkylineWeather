using LanguageExt.Common;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Models;

namespace SkylineWeather.Abstractions.Provider.Interfaces;

public interface IHourlyWeatherProvider
{
    public Task<Result<List<HourlyWeather>>> GetHourlyWeatherAsync(Location location);
}