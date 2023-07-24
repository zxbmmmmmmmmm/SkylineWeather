using FluentWeather.Abstraction.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluentWeather.Abstraction.Interfaces.GeolocationProvider;

public interface IGeolocationProvider
{
    /// <summary>
    /// 通过名称获取匹配的城市列表以及地理位置
    /// </summary>
    /// <param name="name">城市名称</param>
    /// <returns></returns>
    Task<List<GeolocationBase>> GetCitiesGeolocationByName(string name);
}