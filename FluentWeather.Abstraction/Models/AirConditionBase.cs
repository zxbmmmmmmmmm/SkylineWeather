using FluentWeather.Abstraction.Interfaces.Weather;
using System.Collections.Generic;

namespace FluentWeather.Abstraction.Models;

public class AirConditionBase : IAirCondition, IAirPollutants
{
    public int Aqi { get; set; }
    public int? AqiLevel { get; set; }
    public virtual string? AqiCategory { get; set; }
    public List<Pollutant> Pollutants { get; set; } = new();
}

