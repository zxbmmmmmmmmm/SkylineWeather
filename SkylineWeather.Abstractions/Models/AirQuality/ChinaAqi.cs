namespace SkylineWeather.Abstractions.Models.AirQuality;

public record ChinaAqi : IAqi
{
    public double Aqi { get; set; }
    public double Level { get; set; }
}