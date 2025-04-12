using LanguageExt.Common;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;

namespace SkylineWeather.Abstractions.Provider.Interfaces;

public interface IDailyWeatherProvider
{
    public Task<Result<IReadOnlyList<DailyWeather>>> GetDailyWeatherAsync(Location location);

}