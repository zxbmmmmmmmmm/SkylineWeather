using FluentWeather.Abstraction;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Interfaces.Helpers;
using FluentWeather.Abstraction.Interfaces.Setting;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.DIContainer;
using FluentWeather.QGeoApi.ApiContracts;
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
using System.Threading.Tasks.Sources;

namespace FluentWeather.QWeatherProvider;

public sealed class QWeatherProvider : ProviderBase,
    ICurrentWeatherProvider,
    IDailyForecastProvider,
    IHourlyForecastProvider,
    IWeatherWarningProvider,
    IIndicesProvider,
    IPrecipitationProvider,
    IAirConditionProvider,
    ITyphoonProvider,
    IGeolocationProvider,
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
    public QWeatherProvider(string token,string domain,string language = null)
    {
        Instance = this;
        Option.Token = token;
        Option.Domain = domain;
        Option.Language = language;
    }
    public void GetSettings()
    {
        var settingsHelper = Locator.ServiceProvider.GetService<ISettingsHelper>();
        Option.Token = settingsHelper.ReadLocalSetting(Id + "." + QWeatherSettings.Token, "");
        Option.Domain = settingsHelper.ReadLocalSetting(Id + "." + QWeatherSettings.Domain, "devapi.qweather.com");
        var language = settingsHelper?.ReadLocalSetting<string>(QWeatherSettings.Language.ToString(), null);
        if (language is null) return;
        if (language.Contains("-") && !language.Contains("zh-hant"))
        {
            Option.Language = language.Remove(language.IndexOf("-", StringComparison.Ordinal));
        }
        else
        {
            Option.Language = language;
        }
    }

    public async Task<WeatherNowBase> GetCurrentWeather(double lon,double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherNowApi, new QWeatherRequest(lon, lat));
        return result.WeatherNow.MapToQweatherNow();
    }

    public async Task<List<WeatherDailyBase>> GetDailyForecasts(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherDailyApi, new QWeatherRequest(lon, lat));
        var res = result.DailyForecasts?.ConvertAll(p => (WeatherDailyBase)p.MapToQWeatherDailyForecast());
        return res;
    }

    public async Task<List<WeatherHourlyBase>> GetHourlyForecasts(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherHourlyApi, new QWeatherRequest(lon, lat));
        var res = result.HourlyForecasts?.ConvertAll(p => (WeatherHourlyBase)p.MapToQWeatherHourlyForecast());
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

    public async Task<AirConditionBase> GetAirCondition(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.AirConditionApi, new QWeatherRequest(lon, lat));
        var res = result.AirConditionNow?.MapToQAirCondition();
        if(res is not null)res.Link = result.FxLink;
        return res;
    }

    public async Task<List<TyphoonBase>> GetActiveTyphoons()
    {
        var list = await RequestAsync(QWeatherApis.TyphoonListApi, new RequestBase());
        var active = list.Typhoons.Where(p => p.IsActive == "1").ToList();
        List<Task> tasks = new List<Task>();
        foreach (var item in active)
        {
            tasks.Add(GetTyphoon(item.Id, item.Name));         
        }
        await Task.WhenAll(tasks);
        return tasks.ConvertAll(p => ((Task<TyphoonBase>)p).Result);
    }
    public async Task<TyphoonBase> GetTyphoon (string id,string name)
    {
        var typ = await RequestAsync(QWeatherApis.TyphoonTrackApi, new TyphoonTrackRequest { TyphoonId = id });
        var qtyp = typ.MapToQTyphoon(name);
        var forecast = await RequestAsync(QWeatherApis.TyphoonForecastApi, new TyphoonForecastRequest { TyphoonId = id});
        var qfor = forecast.Forecasts.ConvertAll(p => p.MapToQTyphoonTrack());
        qtyp.Forecast = qfor;
        return qtyp;
    }

    public async Task<List<GeolocationBase>> GetCitiesGeolocationByName(string name)
    {
        var result = await RequestAsync(new GeolocationApi<QGeolocationResponse>(), new QGeolocationRequestByName { Name = name });
        return result.Locations?.ConvertAll(p => p.MapToGeolocationBase());
    }
    public async Task<List<GeolocationBase>> GetCitiesGeolocationByLocation(double lat, double lon)
    {
        var result = await RequestAsync(new GeolocationApi<QGeolocationResponse>(), new QGeolocationRequestByLocation { Lat = lat, Lon = lon });
        return result.Locations.ConvertAll(p => p.MapToGeolocationBase());
    }
}
public enum QWeatherSettings
{
    Token,
    Domain,
    Language
}