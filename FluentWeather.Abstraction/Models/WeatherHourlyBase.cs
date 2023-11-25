using FluentWeather.Abstraction.Interfaces.Weather;
using System;

namespace FluentWeather.Abstraction.Models;

public class WeatherHourlyBase :WeatherBase, ITemperature, ITime, IWind, IHumidity, IPressure,IPrecipitationProbability
{
    public int Humidity { get; set; }
    public int Pressure { get; set; }
    public int Temperature { get; set; }
    public DateTime Time { get; set; }
    public string WindDirection { get; set; }
    public string WindScale { get; set; }
    public int WindSpeed { get; set; }
    public int? PrecipitationProbability { get; set; }

}