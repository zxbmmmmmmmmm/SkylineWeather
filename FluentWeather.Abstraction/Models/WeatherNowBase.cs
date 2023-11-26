using FluentWeather.Abstraction.Interfaces.Weather;

namespace FluentWeather.Abstraction.Models;

public class WeatherNowBase:WeatherBase, IWind, ITemperature, IPressure, IHumidity
{
    public int Humidity { get; set; }
    public int Pressure { get; set; }
    public string WindDirection { get; set; }
    public string WindScale { get; set; }
    public int WindSpeed { get; set; }
    public int Temperature { get; set; }

}