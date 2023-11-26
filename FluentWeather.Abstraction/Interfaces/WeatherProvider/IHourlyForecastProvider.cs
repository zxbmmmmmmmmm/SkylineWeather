using FluentWeather.Abstraction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentWeather.Abstraction.Interfaces.WeatherProvider;

public interface IHourlyForecastProvider
{
    /// <summary>
    /// 根据经纬度获取每小时天气预报
    /// </summary>
    /// <param name="lon">经度</param>
    /// <param name="lat">纬度</param>
    /// <returns></returns>
    Task<List<WeatherHourlyBase>> GetHourlyForecasts(double lon,double lat);
}