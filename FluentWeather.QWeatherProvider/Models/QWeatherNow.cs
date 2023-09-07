using FluentWeather.Abstraction.Interfaces.Weather;
using FluentWeather.Abstraction.Models;
using FluentWeather.QWeatherProvider.Helpers;

namespace FluentWeather.QWeatherProvider.Models;

public class QWeatherNow : WeatherBase, IWind,ITemperature, IPressure, IHumidity, IVisibility,IApparentTemperature,ICloudAmount
{
    public override WeatherType WeatherType => WeatherTypeConverter.GetWeatherTypeByDescription(Description);
    public int Humidity { get; set; }
    public int Pressure { get; set; }
    public int Visibility { get; set; }
    public string WindDirection { get; set; }
    public string WindScale { get; set; }
    public int WindSpeed { get; set; }
    public int Temperature { get ; set ; }
    public int ApparentTemperature { get ; set; }
    public int? CloudAmount { get ; set; }
}