using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherProvider.Helpers;

namespace FluentWeather.QWeatherProvider.Models;

public class QWeatherNow : WeatherNowBase, IVisibility,IApparentTemperature,ICloudAmount,IDew
{
    public override WeatherType WeatherType => WeatherTypeConverter.GetWeatherTypeByDescription(Description);
    public int Visibility { get; set; }
    public int ApparentTemperature { get ; set; }
    public int? CloudAmount { get ; set; }
    public int? DewPointTemperature { get ; set ; }
}