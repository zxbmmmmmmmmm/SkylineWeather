using FluentWeather.Abstraction.Models;
using System.Collections.Generic;

namespace FluentWeather.Abstraction.Interfaces.Weather;

/// <summary>
/// 空气污染物
/// </summary>
public interface IAirPollutants
{
    List<Pollutant> Pollutants { get; set; }
}
