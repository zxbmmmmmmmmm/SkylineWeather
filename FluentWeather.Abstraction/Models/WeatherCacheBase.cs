using FluentWeather.Abstraction.Interfaces.Weather;
using System;
using System.Collections.Generic;
using System.Text;
namespace FluentWeather.Abstraction.Models;
public class WeatherCacheBase
{
    public virtual List<WeatherDailyBase> DailyForecasts { get; set; } = new();
    public virtual List<WeatherHourlyBase> HourlyForecasts { get; set; } = new();
    public virtual List<WeatherWarningBase> Warnings { get; set; }
    public virtual WeatherBase WeatherNow { get; set; }
    public virtual string WeatherDescription { get; set; }
    public virtual DateTime SunRise { get; set; }
    public virtual DateTime SunSet { get; set; }
    public virtual GeolocationBase Location { get; set; }
    public virtual List<IndicesBase> Indices { get; set; }
    public virtual PrecipitationBase Precipitation { get; set; }
    public virtual AirConditionBase AirCondition { get; set; }
    public virtual DateTime UpdatedTime { get; set; }
}