using FluentWeather.Abstraction;
using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Abstraction.Interfaces.Setting;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.QWeatherApi;
using FluentWeather.QWeatherApi.ApiContracts;
using FluentWeather.QWeatherApi.Bases;
using FluentWeather.QWeatherProvider.Mappers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace FluentWeather.QWeatherProvider;

public class QWeatherProvider : ProviderBase,
    ICurrentWeatherProvider,
    IDailyForecastProvider,
    IHourlyForecastProvider,
    IWeatherWarningProvider,
    IIndicesProvider,
    IPrecipitationProvider,
    ISetting
{
    public override string Name => "和风天气";

    public override string Id => "qweather";
    public Enum Settings => new QWeatherSettings();

    public ApiHandlerOption Option { get; set; } = new();
    
    public static QWeatherProvider Instance = null;
    public QWeatherProvider()
    {
        Instance = this;
        GetSettings();

    }
    public static void RegisterRequiredServices()
    {
        Locator.ServiceDescriptors.AddSingleton(typeof(ICurrentWeatherProvider),typeof(QWeatherProvider));
        Locator.ServiceDescriptors.AddSingleton(typeof(IDailyForecastProvider), typeof(QWeatherProvider));
        Locator.ServiceDescriptors.AddSingleton(typeof(IHourlyForecastProvider), typeof(QWeatherProvider));
        Locator.ServiceDescriptors.AddSingleton(typeof(IWeatherWarningProvider), typeof(QWeatherProvider));
        Locator.ServiceDescriptors.AddSingleton(typeof(IIndicesProvider), typeof(QWeatherProvider));
        Locator.ServiceDescriptors.AddSingleton(typeof(IPrecipitationProvider), typeof(QWeatherProvider));
        Locator.ServiceDescriptors.AddSingleton(typeof(ISetting), typeof(QWeatherProvider));
    }
    public void GetSettings()
    {
        var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
        settingsHelper.DeleteLocalSetting(Id + "." + QWeatherSettings.Token);
        Option.Token = settingsHelper.ReadLocalSetting(Id + "." + QWeatherSettings.Token, "");
    }

    public async Task<WeatherBase> GetCurrentWeather(double lon,double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherNowApi, new QWeatherRequest(lon, lat));
        return result.WeatherNow.MapToQweatherNow();
    }

    public async Task<List<WeatherBase>> GetDailyForecasts(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherDailyApi, new QWeatherRequest(lon, lat));
        var res = result.DailyForecasts?.ConvertAll(p => (WeatherBase)p.MapToQWeatherDailyForecast());
        return res;
    }

    public async Task<List<WeatherBase>> GetHourlyForecasts(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherHourlyApi, new QWeatherRequest(lon, lat));
        var res = result.HourlyForecasts?.ConvertAll(p => (WeatherBase)p.MapToQWeatherHourlyForecast());
        return res;
    }

    public async Task<List<WeatherWarningBase>> GetWeatherWarnings(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherWarningApi, new QWeatherRequest(lon, lat));
        var res = result.Warnings?.ConvertAll(p => (WeatherWarningBase)p.MapToQWeatherWarning());
        return res;
    }
    public async Task<List<IndicesBase>> GetIndices(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherIndicesApi, new QWeatherRequest(lon, lat));
        var res = result.Indices?.ConvertAll(p => (IndicesBase)p.MapToQWeatherIndices());
        return res;
    }
    public async Task<TResponse> RequestAsync<TRequest, TResponse>(
        ApiContractBase<TRequest, TResponse> contract, TRequest request)
    {
        var handler = new QWeatherApiHandler();
        return await handler.RequestAsync(contract, request, Option);
    }

    public async Task<PrecipitationBase> GetPrecipitations(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.PrecipitationApi, new QWeatherRequest(lon, lat));
        var res = result.MapToQweatherPrecipitation();
        return res;
    }
}
public enum QWeatherSettings
{
    Token,
}