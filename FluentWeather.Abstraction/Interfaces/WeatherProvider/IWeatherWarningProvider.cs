using FluentWeather.Abstraction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentWeather.Abstraction.Interfaces.WeatherProvider;

public interface IWeatherWarningProvider
{
    /// <summary>
    /// 根据经度和纬度获取当地的天气预警
    /// </summary>
    /// <param name="lon">经度</param>
    /// <param name="lat">纬度</param>
    /// <returns></returns>
    Task<List<WeatherWarningBase>> GetWeatherWarnings(double lon, double lat);
}