using FluentWeather.Abstraction.Models;

namespace FluentWeather.Abstraction.Interfaces.Weather;

public interface IWeatherNight
{
    WeatherBase WeatherNight { get; set; }
}