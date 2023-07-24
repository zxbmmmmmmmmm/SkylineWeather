using System;

namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface IAstronomic
{
    /// <summary>
    /// 日出时间
    /// </summary>
    DateTime SunRise { get; set; }
    /// <summary>
    /// 日落时间
    /// </summary>
    DateTime SunSet { get; set; }
}