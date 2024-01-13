using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentWeather.Abstraction;
using FluentWeather.Abstraction.Interfaces.GeolocationProvider;
using FluentWeather.Abstraction.Interfaces.WeatherProvider;

namespace FluentWeather.Uwp.Shared;

public static class DataProviderHelper
{
    public static List<KeyValuePair<string, string>> QWeatherConfig = new()
    {
        new(nameof(ICurrentWeatherProvider),"qweather"),
        new(nameof(IDailyForecastProvider),"qweather"),
        new(nameof(IHourlyForecastProvider),"qweather"),
        new(nameof(IWeatherWarningProvider),"qweather"),
        new(nameof(IIndicesProvider),"qweather"),
        new(nameof(IPrecipitationProvider),"qweather"),
        new(nameof(IAirConditionProvider),"qweather"),
        new(nameof(ITyphoonProvider),"qweather"),
        new(nameof(IGeolocationProvider),"qgeoapi"),
    };
    public static List<KeyValuePair<string, string>> OpenMeteoConfig = new()
    {
        new(nameof(ICurrentWeatherProvider),"open-meteo"),
        new(nameof(IDailyForecastProvider),"open-meteo"),
        new(nameof(IHourlyForecastProvider),"open-meteo"),
        new(nameof(IWeatherWarningProvider),"qweather"),
        new(nameof(IIndicesProvider),"qweather"),
        new(nameof(IPrecipitationProvider),"qweather"),
        new(nameof(IAirConditionProvider),"open-meteo"),
        new(nameof(ITyphoonProvider),"qweather"),
        new(nameof(IGeolocationProvider),"qgeoapi"),
    };
    public static Type GetProviderInterfaceByName(string name)
    {
        var assembly = typeof(ProviderBase).Assembly;
        return assembly.ExportedTypes.FirstOrDefault(p => p.Name == name);
    }
    public static Type GetProviderTypeById(string id)
    {
        return id switch
        {
            "qweather" => typeof(QWeatherProvider.QWeatherProvider),
            "open-meteo" => typeof(OpenMeteoProvider.OpenMeteoProvider),
            "qgeoapi" => typeof(QGeoProvider.QGeoProvider),
            _ => typeof(QWeatherProvider.QWeatherProvider)
        };
    }
}