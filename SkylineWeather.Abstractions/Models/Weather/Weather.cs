namespace SkylineWeather.Abstractions.Models.Weather;

public record Weather
{
    public required WeatherCode WeatherCode { get; set; }
}