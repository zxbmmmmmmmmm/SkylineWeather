namespace SkylineWeather.Abstractions.Models.AirQuality;

public class ChinaAqi : IAqi
{
    public double Aqi { get; set; }
    public double Level { get; set; }
}