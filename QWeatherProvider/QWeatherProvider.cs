using LanguageExt;
using LanguageExt.Common;
using QWeatherApi;
using QWeatherApi.ApiContracts;
using QWeatherApi.Bases;
using QWeatherProvider.Mappers;
using SkylineWeather.Abstractions.Models;
using SkylineWeather.Abstractions.Models.Alert;
using SkylineWeather.Abstractions.Models.Weather;
using SkylineWeather.Abstractions.Provider;
using SkylineWeather.Abstractions.Provider.Interfaces;

namespace QWeatherProvider;

[Provider("qweather")]
[ProviderConfiguration(typeof(QWeatherProviderConfig))]
public class QWeatherProvider(QWeatherProviderConfig config) :
    ProviderBase,
    IWeatherProvider,
    IAlertProvider,
    IGeolocationProvider
{
    public override string Name => "QWeather";


    private readonly QWeatherApiHandler _handler = new();
    private readonly ApiHandlerOption _option = new()
    {
        Domain = config.Domain,
        Language = config.Language,
        Token = config.Token
    };

    public async Task<Result<CurrentWeather>> GetCurrentWeatherAsync(Location location, CancellationToken cancellationToken = default)
    {
        var result = await _handler.RequestAsync(QWeatherApis.WeatherNowApi,new QWeatherRequest(location.Longitude,location.Latitude),_option);
        return result.WeatherNow.MapToCurrentWeather();
    }

    public async Task<Result<IReadOnlyList<DailyWeather>>> GetDailyWeatherAsync(Location location, CancellationToken cancellationToken = default)
    {
        var result = await _handler.RequestAsync(QWeatherApis.WeatherDailyApi, new QWeatherRequest(location.Longitude, location.Latitude), _option);
        return result.DailyForecasts.ConvertAll(p => p.MapToDailyWeather());
    }

    public async Task<Result<IReadOnlyList<HourlyWeather>>> GetHourlyWeatherAsync(Location location, CancellationToken cancellationToken = default)
    {
        var result = await _handler.RequestAsync(QWeatherApis.WeatherHourlyApi, new QWeatherRequest(location.Longitude, location.Latitude), _option);
        return result.HourlyForecasts.ConvertAll(p => p.MapToHourlyWeather());
    }

    public async Task<Result<IReadOnlyList<Alert>>> GetAlertsAsync(Location location, CancellationToken cancellationToken = default)
    {
        var result = await _handler.RequestAsync(QWeatherApis.WeatherWarningApi, new QWeatherRequest(location.Longitude, location.Latitude), _option);
        return result.Warnings.ConvertAll(p => p.MapToAlert());
    }

    public async Task<Result<IReadOnlyList<Geolocation>>> GetGeolocationsAsync(Location location, CancellationToken cancellationToken = default)
    {
        var result = await _handler.RequestAsync(
            new GeolocationApi<QGeolocationResponse>(), 
            new QGeolocationRequestByLocation { Lat = location.Latitude, Lon = location.Longitude },
            _option);
        return result.Locations.ConvertAll(p => p.MapToGeolocation());
    }

    public async Task<Result<IReadOnlyList<Geolocation>>> GetGeolocationsAsync(string name, CancellationToken cancellationToken = default)
    {
        var result = await _handler.RequestAsync(
            new GeolocationApi<QGeolocationResponse>(),
            new QGeolocationRequestByName { Name = name },
            _option);
        return result.Locations.ConvertAll(p => p.MapToGeolocation());
    }
}