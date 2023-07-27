using System;

namespace FluentWeather.Abstraction.Interfaces.Geolocation;

public interface ITimeZone
{
    /// <summary>
    /// 时区
    /// </summary>
    string TimeZone { get; set; }
    /// <summary>
    /// 与UTC时间偏移小时数
    /// </summary>
    TimeSpan UtcOffset { get; set; }
    /// <summary>
    /// 是否为夏令时
    /// </summary>
    bool IsDaylightSavingTime { get; set; }
}