using FluentWeather.Abstraction.Helpers;
using FluentWeather.Abstraction.Interfaces.Weather;
using System;

namespace FluentWeather.Abstraction.Models;

public class WeatherHourlyBase : WeatherBase, ITemperature, ITime, IWind, IHumidity, IPressure, IPrecipitationProbability, ICloudAmount, IVisibility
{
    public int? Humidity { get; set; }
    public int? Pressure { get; set; }
    public  int Temperature { get; set; }
    public  DateTime Time { get; set; }

    private string? _windDirectionDescription;

    public string WindDirectionDescription
    {
        get => _windDirectionDescription ?? ResourcesHelper.GetWindDirectionDescription(WindDirection);
        set => _windDirectionDescription = value;
    }

    public string? WindScale { get; set; }
    public  int WindSpeed { get; set; }
    public int? PrecipitationProbability { get; set; }
    public  WindDirection WindDirection { get; set; }
    public int? CloudAmount { get ; set ; }
    public int? Visibility { get ; set ; }
}