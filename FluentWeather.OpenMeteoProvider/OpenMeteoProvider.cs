using FluentWeather.Abstraction;
using FluentWeather.Abstraction.Interfaces.Setting;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.OpenMeteoProvider.Mappers;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using OpenMeteoApi;
using System.Collections.Generic;
using FluentWeather.OpenMeteoProvider.Models;

namespace FluentWeather.OpenMeteoProvider;

public class OpenMeteoProvider : ProviderBase, ICurrentWeatherProvider, IAirConditionProvider,IDailyForecastProvider
{
    public override string Name => "OpenMeteo";
    public override string Id => "open-meteo";

    private readonly OpenMeteoClient _client = new();
    public async Task<WeatherNowBase> GetCurrentWeather(double lon, double lat)
    {
        var result = await _client.GetCurrentWeather(lat, lon);
        var now = result.MapToOpenMeteoWeatherNow();
        return now;
    }
    public async Task<AirConditionBase> GetAirCondition(double lon, double lat)
    {
        var result = await _client.GetCurrentAirQuality(lat, lon);
        return result.MapToOpenMeteoWeatherNow();
    }

    public static void RegisterRequiredServices()
    {
        Locator.ServiceDescriptors.AddSingleton(typeof(ICurrentWeatherProvider), typeof(OpenMeteoProvider));
        Locator.ServiceDescriptors.AddSingleton(typeof(IAirConditionProvider), typeof(OpenMeteoProvider));
        Locator.ServiceDescriptors.AddSingleton(typeof(IDailyForecastProvider), typeof(OpenMeteoProvider));
    }

    public async Task<List<WeatherDailyBase>> GetDailyForecasts(double lon, double lat)
    {
        var result = await _client.GetDailyForecasts(lat, lon);
        return result.ConvertAll<WeatherDailyBase>(p => p.MapToOpenMeteoDailyForecast());
    }
}