using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentWeather.Abstraction;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;
using FluentWeather.Abstraction.Models;
using FluentWeather.Abstraction.Models.Exceptions;
using FluentWeather.Uwp.QWeatherProvider.Mappers;
using FluentWeather.Uwp.QWeatherProvider.Views;
using FluentWeather.Uwp.Shared;
using QWeatherApi;
using QWeatherApi.ApiContracts;
using QWeatherApi.Bases;

namespace FluentWeather.Uwp.QWeatherProvider;

public sealed class QWeatherProvider : ProviderBase,
    ICurrentWeatherProvider,
    IDailyForecastProvider,
    IHourlyForecastProvider,
    IWeatherWarningProvider,
    IIndicesProvider,
    IPrecipitationProvider,
    IAirConditionProvider,
    ITyphoonProvider,
    IGeolocationProvider
{
    public override string Name => "和风天气";

    public override string Id => "qweather";

    public ApiHandlerOption Option { get; set; } = new();


    public static QWeatherProvider Instance = null;
    public QWeatherProvider()
    {
        Instance = this;
        GetSettings();
    }
    public QWeatherProvider(string token,string domain,string language = null,string publicKey = null)
    {
        Instance = this;
        Option.Token = token;
        Option.Domain = domain;
        Option.Language = language;
        Option.PublicId = publicKey;
    }
    public void SetDomain(string domain)
    {
        Option.Domain = domain;
    }
    public void SetToken(string token)
    {
        Option.Token = token;
    }
    public void SetPublicKey(string key)
    {
        Option.PublicId = key;
    }
    public void GetSettings()
    {
        Option.Token = Common.Settings.QWeatherToken;
        Option.Domain = Common.Settings.QWeatherDomain;
        Option.PublicId = Common.Settings.QWeatherPublicId;
        var language = Common.Settings.Language;
        Option.Language = ConvertToApiCode(language);
    }
    public static string ConvertToApiCode(string cultureName)
    {
        var parts = cultureName.Split('-');
        var languageCode = parts[0].ToLowerInvariant();
        var regionCode = parts.Length > 1 ? parts[1].ToUpperInvariant() : null;

        switch (languageCode)
        {
            // 处理中文变体
            case "zh":
                return regionCode switch
                {
                    "CN" or "SG" => "zh",
                    "TW" or "HK" or "MO" => "zh-hant",
                    _ => "zh"
                };
            // 处理挪威语（no -> nb）
            case "no":
                return "nb";
        }
        return languageCode;

        //// 验证其他语言代码是否在支持的列表中
        //var supportedCodes = new HashSet<string>
        //{
        //    "en", "de", "es", "fr", "it", "ja", "ko", "ru",
        //    "hi", "th", "ar", "pt", "bn", "ms", "nl", "el",
        //    "la", "sv", "id", "pl", "tr", "cs", "et", "vi",
        //    "fil", "fi", "he", "is", "nb"
        //};

        //if (supportedCodes.Contains(languageCode))
        //    return languageCode;

        //throw new NotSupportedException($"Unsupported culture code: {cultureName}");
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
        var res = result.HourlyForecasts?.ConvertAll(p => (WeatherHourlyBase)p.MapToHourlyForecast());
        return res;
    }

    public async Task<List<WeatherWarningBase>> GetWeatherWarnings(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherWarningApi, new QWeatherRequest(lon, lat));
        var res = result.Warnings?.ConvertAll(p => (WeatherWarningBase)p.MapToWeatherWarningBase());
        return res;
    }
    public async Task<List<IndicesBase>> GetIndices(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.WeatherIndicesApi, new QWeatherRequest(lon, lat));
        var res = result.Indices?.ConvertAll(p => (IndicesBase)p.MapToQWeatherIndices());
        return res;
    }

    public async Task<PrecipitationBase> GetPrecipitations(double lon, double lat)
    {
        var result = await RequestAsync(QWeatherApis.PrecipitationApi, new QWeatherRequest(lon, lat));
        var res = result.MapToPrecipitationBase();
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
        var qtyp = typ.MapToTyphoonBase(name);
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

    public async Task<TResponse> RequestAsync<TRequest, TResponse>(
        ApiContractBase<TRequest, TResponse> contract, TRequest request) where TResponse : QWeatherResponseBase
    {
        var handler = new QWeatherApiHandler();
        var response = await handler.RequestAsync(contract, request, Option);
        if (response.Code is not null && !response.Code.StartsWith("2"))
        {
            if(response.Code is "401" or "402" or "403")
            {
                await DialogManager.OpenDialogAsync(new SetTokenDialog(), DialogShowingOption.ShowIfNoActive);
            }
            throw new HttpResponseException($"Request returned http status code {response.Code}", (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), response.Code));

        }
        return response;
    }
}
