using FluentWeather.Abstraction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentWeather.Abstraction.Interfaces.WeatherProvider;

public interface ICurrentWeatherProvider
{
    /// <summary>
    /// 根据经纬度获取当前天气
    /// </summary>
    /// <param name="lon"></param>
    /// <param name="lat"></param>
    /// <returns></returns>
    Task<WeatherNowBase> GetCurrentWeather(double lon, double lat);
}