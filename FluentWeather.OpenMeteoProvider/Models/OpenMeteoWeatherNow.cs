using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;

namespace FluentWeather.OpenMeteoProvider.Models;

public class OpenMeteoWeatherNow : WeatherNowBase, ICloudAmount, IApparentTemperature, IVisibility, IDew
{
    public int? CloudAmount { get; set; }
    public int ApparentTemperature { get; set; }
    public int? Visibility { get; set; }
    public int? DewPointTemperature { get; set; }
}