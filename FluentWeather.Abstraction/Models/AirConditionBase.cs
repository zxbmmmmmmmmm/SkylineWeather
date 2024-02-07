using FluentWeather.Abstraction.Interfaces.Weather;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentWeather.Abstraction.Models;

public class AirConditionBase : IAirCondition, IAirPollutants
{
    public int Aqi { get; set; }
    public int? AqiLevel { get; set; }
    public virtual string? AqiCategory { get; set; }
    public double PM25 { get; set; }
    public double PM10 { get; set; }
    public double NO2 { get; set; }
    public double SO2 { get; set; }
    public double CO { get; set; }
    public double O3 { get; set; }
}

