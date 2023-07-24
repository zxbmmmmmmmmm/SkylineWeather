using FluentWeather.Abstraction;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentWeather.QWeatherProvider;

public class QWeatherProvider : ProviderBase,
    ICurrentWeatherProvider,
    IDailyForecastProvider,
    IHourlyForecastProvider,
    IWeatherWarningProvider
{
    public override string Name => "和风天气";

    public override string Id => "qweather";

    public Task<WeatherBase> GetCurrentWeather(double lon,double lat)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<WeatherBase>> GetDailyForecasts(double lon, double lat)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<WeatherBase>> GetHourlyForecasts(double lon, double lat)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<WeatherWarningBase>> GetWeatherWarnings(double lon, double lat)
    {
        throw new System.NotImplementedException();
    }
}