using LanguageExt.Common;
using OpenMeteoApi;
using OpenMeteoProvider.Mappers;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Provider;
using SkylineWeather.Abstractions.Provider.Interfaces;

namespace OpenMeteoProvider;

public sealed class OpenMeteoProvider(OpenMeteoProviderConfig? config = null) :
    ProviderBase,
    IWeatherProvider
{
    private readonly OpenMeteoClient _client = new();
    public OpenMeteoProviderConfig Config { get; set; } = config ?? OpenMeteoProviderConfig.Default;
    public override string Name => "OpenMeteo";
    public override string Id => "open-meteo";

    public async Task<Result<IReadOnlyList<DailyWeather>>> GetDailyWeatherAsync(Location location)
    {
        var data = await _client.GetDailyForecasts(location.Latitude, location.Longitude);
        return data.ConvertAll(p => p.MapToDailyWeather());
    }

    public async Task<Result<IReadOnlyList<HourlyWeather>>> GetHourlyWeatherAsync(Location location)
    {
        var data = await _client.GetHourlyForecasts(location.Latitude, location.Longitude);
        return data.ConvertAll(p => p.MapToHourlyWeather());
    }

    public async Task<Result<CurrentWeather>> GetCurrentWeatherAsync(Location location)
    {
        var data = await _client.GetCurrentWeather(location.Latitude, location.Longitude);
        return data.MapToCurrentWeather();
    }
}
