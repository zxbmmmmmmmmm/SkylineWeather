namespace FluentWeather.Abstraction.Models;

public class WeatherBase
{
    public virtual WeatherCode WeatherType { get; set; }
    public string? Description { get; set; }
}
