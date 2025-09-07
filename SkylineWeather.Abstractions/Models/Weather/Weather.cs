namespace SkylineWeather.Abstractions.Models.Weather;

public record Weather
{
    // required
    public WeatherCode WeatherCode { get; set; }
}