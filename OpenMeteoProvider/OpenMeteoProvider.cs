using LanguageExt.Common;
using OpenMeteoApi;
using OpenMeteoProvider.Mappers;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.AirQuality;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Provider;
using SkylineWeather.Abstractions.Provider.Interfaces;

namespace OpenMeteoProvider;

[Provider("open-meteo")]
[ProviderConfiguration(typeof(OpenMeteoProviderConfig))]
public sealed class OpenMeteoProvider(OpenMeteoProviderConfig? config = null) :
    ProviderBase,
    ICurrentWeatherProvider,
    IDailyWeatherProvider,
    IHourlyWeatherProvider,
    IAirQualityProvider
{
    private readonly OpenMeteoClient _client = new();
    public OpenMeteoProviderConfig Config { get; set; } = config ?? OpenMeteoProviderConfig.Default;
    public override string Name => "OpenMeteo";

    public async Task<Result<IReadOnlyList<DailyWeather>>> GetDailyWeatherAsync(Location location, CancellationToken cancellationToken = default)
    {
        var data = await _client.GetDailyForecasts(location.Latitude, location.Longitude);
        return data.ConvertAll(p => p.MapToDailyWeather());
    }

    public async Task<Result<IReadOnlyList<HourlyWeather>>> GetHourlyWeatherAsync(Location location, CancellationToken cancellationToken = default)
    {
        var data = await _client.GetHourlyForecasts(location.Latitude, location.Longitude);
        return data.ConvertAll(p => p.MapToHourlyWeather());
    }

    public async Task<Result<CurrentWeather>> GetCurrentWeatherAsync(Location location, CancellationToken cancellationToken = default)
    {
        var data = await _client.GetCurrentWeather(location.Latitude, location.Longitude);
        return data.MapToCurrentWeather();
    }

    public async Task<Result<AirQuality>> GetCurrentAirQualityAsync(Location location, CancellationToken cancellationToken = default)
    {
        var data = await _client.GetAirQualityData(location.Latitude, location.Longitude,
            ["pm10", "pm2_5", "carbon_monoxide", "nitrogen_dioxide", "carbon_dioxide", "sulphur_dioxide", "ozone", "european_aqi", "us_aqi"]);
        return data.MapToAirQuality();
    }
}
