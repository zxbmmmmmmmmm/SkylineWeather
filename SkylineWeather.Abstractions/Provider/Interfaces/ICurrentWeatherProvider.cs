using LanguageExt;
using LanguageExt.Common;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;
namespace SkylineWeather.Abstractions.Provider.Interfaces;

public interface ICurrentWeatherProvider
{
    public Task<Result<CurrentWeather>> GetCurrentWeatherAsync(Location location);
}