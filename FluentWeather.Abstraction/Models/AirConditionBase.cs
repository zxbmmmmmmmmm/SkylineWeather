using FluentWeather.Abstraction.Interfaces.Weather;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentWeather.Abstraction.Models;

public class AirConditionBase : IAirCondition
{
    public required int Aqi { get ; set ; }
    public int? AqiLevel { get ; set ; }
    public string? AqiCategory { get ; set ; }
}

