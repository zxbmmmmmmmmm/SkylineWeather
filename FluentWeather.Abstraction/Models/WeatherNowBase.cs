using FluentWeather.Abstraction.Helpers;
using FluentWeather.Abstraction.Interfaces.Weather;

namespace FluentWeather.Abstraction.Models;

public class WeatherNowBase : WeatherBase, IWind, ITemperature, IPressure, IHumidity, IApparentTemperature, IVisibility, ICloudAmount, IDew
{
    public int? Humidity { get; set; }
    public int? Pressure { get; set; }

    private string? _windDirectionDescription;

    public string WindDirectionDescription
    {
        get => _windDirectionDescription ?? ResourcesHelper.GetWindDirectionDescription(WindDirection);
        set => _windDirectionDescription = value;
    }

    public string? WindScale { get; set; }
    public int WindSpeed { get; set; }
    public int Temperature { get; set; }
    public  WindDirection WindDirection { get; set; }
    public int? ApparentTemperature { get; set; }
    public int? Visibility { get; set; }
    public int? CloudAmount { get; set; }
    public int? DewPointTemperature { get; set; }
}