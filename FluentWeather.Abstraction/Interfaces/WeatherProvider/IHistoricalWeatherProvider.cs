using FluentWeather.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentWeather.Abstraction.Interfaces.WeatherProvider;

public interface IHistoricalWeatherProvider
{
    public Task<List<WeatherDailyBase>> GetHistoricalDailyWeather(double lon, double lat, DateTime startTime, DateTime endTime);
}