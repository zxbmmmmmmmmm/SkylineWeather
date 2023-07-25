using FluentWeather.Abstraction;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherApi;
using FluentWeather.QWeatherApi.ApiContracts;
using FluentWeather.QWeatherApi.Bases;
using FluentWeather.QWeatherProvider.Mappers;
using System.Collections.Generic;
using System.Linq;
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

    public ApiHandlerOption Option { get; set; }

    public static QWeatherProvider Instance = null;
    public QWeatherProvider(string token)
    {
        Instance = this;
        Option = new ApiHandlerOption { Token = token };
    }

    public async Task<WeatherBase> GetCurrentWeather(double lon,double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherNowApi, new QWeatherRequest(lon, lat));
        return result.WeatherNow.MapToQweatherNow();
    }

    public async Task<List<WeatherBase>> GetDailyForecasts(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherDailyApi, new QWeatherRequest(lon, lat));
        var res = result.DailyForecasts.ConvertAll(p => (WeatherBase)p.MapToQWeatherDailyForecast());
        return res;
    }

    public async Task<List<WeatherBase>> GetHourlyForecasts(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherHourlyApi, new QWeatherRequest(lon, lat));
        var res = result.HourlyForecasts.ConvertAll(p => (WeatherBase)p.MapToQWeatherHourlyForecast());
        return res;
    }

    public async Task<List<WeatherWarningBase>> GetWeatherWarnings(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherWarningApi, new QWeatherRequest(lon, lat));
        var res = result.Warnings.ConvertAll(p => (WeatherWarningBase)p.MapToQWeatherWarning());
        return res;
    }
    public async Task<TResponse> RequestAsync<TRequest, TResponse>(
        ApiContractBase<TRequest, TResponse> contract, TRequest request)
    {
        var handler = new QWeatherApiHandler();
        return await handler.RequestAsync(contract, request, Option);
    }

}