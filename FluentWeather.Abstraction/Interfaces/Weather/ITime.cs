using System;

namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface ITime
{
    DateTime Time { get; set; }
}