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
using OpenMeteoApi.Variables;
using System;
using System.Linq;

namespace FluentWeather.OpenMeteoProvider;

public sealed class OpenMeteoProvider : ProviderBase, ICurrentWeatherProvider, IAirConditionProvider, IDailyForecastProvider, IHourlyForecastProvider, IPrecipitationProvider, IHistoricalWeatherProvider
{
    public override string Name => "OpenMeteo";
    public override string Id => "open-meteo";

    public static OpenMeteoClient Client { get; }
    static OpenMeteoProvider()
    {
        Client = new();
        Client.ForecastParameters.Add("forecast_minutely_15", "8");
    }
    public async Task<WeatherNowBase> GetCurrentWeather(double lon, double lat)
    {
        var result = await Client.GetWeatherForecastData(lat, lon, currentVariables: CurrentVariables.All, hourlyVariables:new[]{ HourlyVariables.Visibility, "dew_point_2m" });
        var now = result.CurrentWeather!.MapToOpenMeteoWeatherNow();
        now.DewPointTemperature = (int)result.HourlyForecast?.DewPoint2m?[0]!;
        now.Visibility = (int)(result.HourlyForecast?.Visibility?[0]!/1000);
        return now;
    }
    public async Task<AirConditionBase> GetAirCondition(double lon, double lat)
    {
        var result = await Client.GetCurrentAirQuality(lat, lon);
        return result.MapToOpenMeteoWeatherNow();
    }

    public async Task<List<WeatherDailyBase>> GetDailyForecasts(double lon, double lat)
    {
        var result = await Client.GetDailyForecasts(lat, lon);
        return result.ConvertAll<WeatherDailyBase>(p => p.MapToOpenMeteoDailyForecast());
    }

    public async Task<List<WeatherHourlyBase>> GetHourlyForecasts(double lon, double lat)
    {
        var result = await Client.GetHourlyForecasts(lat, lon);
        return result.ConvertAll(p => p.MapToHourlyForecast());
    }

    public async Task<PrecipitationBase> GetPrecipitations(double lon, double lat)
    {
        var result = await Client.GetMinutelyForecasts(lat, lon);
        var precip = new PrecipitationBase
        {
            Precipitations = result.ConvertAll(p  => new PrecipitationItemBase(DateTime.Parse(p.Time),p.Precipitation!.Value)),
        };
        return precip;
    }

    public async Task<List<WeatherDailyBase>> GetHistoricalDailyWeather(double lon, double lat, DateTime startTime, DateTime endTime)
    {
        var list = new List<WeatherDailyBase>();
        var data = await Client.GetHistoricalWeatherData(lat, lon, startTime, endTime, 
            dailyVariables: [DailyVariables.WeatherCode,DailyVariables.Temperature2mMax,DailyVariables.Temperature2mMin,DailyVariables.PrecipitationSum,DailyVariables.PrecipitationHours]
            );
        for (var i = 0; i < data.DailyForecast!.Time!.Count(); i++)
        {
            var item = new WeatherDailyBase
            {
                Time = data.DailyForecast.Time![i]!.Value,
                Precipitation = data.DailyForecast.PrecipitationSum![i],
                PrecipitationHours = data.DailyForecast.PrecipitationHours![i],
                MaxTemperature = (int)Math.Round(data.DailyForecast.Temperature2mMax![i]!.Value),
                MinTemperature = (int)Math.Round(data.DailyForecast.Temperature2mMin![i]!.Value),
            };
            list.Add(item);
        }
        return list;
    }
}