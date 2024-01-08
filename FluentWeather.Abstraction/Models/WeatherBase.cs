namespace FluentWeather.Abstraction.Models;

public class WeatherBase
{
    public virtual WeatherType WeatherType { get; set; }
    public string Description { get; set; }
}
